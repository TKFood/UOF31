using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/**
* 修改時間：2020/12/02
* 修改人員：高常昇
* 修改項目：
    * 將選擇按鈕放大，且預設為可選人的畫面而非以選擇畫面
* 修改位置：
    * 「前端網頁」中，調整「label.btnOpen.SelectUser」內的「padding」，「OpenSelect」預設為「checked="checked"」
* **/

public partial class CDS_KYTUtils_QUERYWINDOWS_SelectUserFilterGroup : BasePage
{
    private bool isSingle;
    private bool isGroup;
    private Dictionary<string, string> GroupInfo = new Dictionary<string, string>();
    private Dictionary<string, string> UserInfo = new Dictionary<string, string>();
    private Dictionary<string, string> SearchInfo = new Dictionary<string, string>();
    private string ConnectionString;
    private static string CurrentGroup = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
        ((Master_DialogMasterPage)this.Master).Button2Text = ""; // Button2不顯示
        ((Master_DialogMasterPage)this.Master).Button1Text = ""; // Button1不顯示

        string groupID = Request["GROUPID"] != null ? HttpUtility.UrlDecode(Request["GROUPID"]) : "Company";
        string selectedUsers = Request["selectedUsers"] != null ? HttpUtility.UrlDecode(Request["selectedUsers"]) : "";
        JArray jArray = null;
        try { jArray = JArray.Parse(selectedUsers); } catch { }
        isSingle = Request["isSingle"] != null ? HttpUtility.UrlDecode(Request["isSingle"]).ToUpper() == "Y" : false;
        isGroup = Request["isGroup"] != null ? HttpUtility.UrlDecode(Request["isGroup"]).ToUpper() == "Y" : false;
        string groupConnectionString = this.ConnectionString;
        string userConnectionString = this.ConnectionString;
        string groupSql = Request["GroupSql"] != null ? HttpUtility.UrlDecode(Request["GroupSql"]) : "";
        string userSql = Request["UserSql"] != null ? HttpUtility.UrlDecode(Request["UserSql"]) : "";
        string searchSql = Request["SearchSql"] != null ? HttpUtility.UrlDecode(Request["SearchSql"]) : "";
        bool isSearchSelfGroupUsers = Request["isSearchSelfGroupUsers"] != null ? HttpUtility.UrlDecode(Request["isSearchSelfGroupUsers"]).ToUpper() == "Y" : false;
        if (isSearchSelfGroupUsers)
        {
            userSql = string.Format(@"
                SELECT [ACCOUNT],
	                   [NAME] + '(' +[TB_EB_USER].[ACCOUNT] + ')' AS 'NAME',
	                   [USER_GUID],
	                   '' AS 'GROUP_ID' 
                  FROM TB_EB_USER 
                 WHERE USER_GUID IN (SELECT USER_GUID
				                        FROM TB_EB_EMPL_DEP 
				                       WHERE GROUP_ID = @GROUP_ID) 
                   AND IS_SUSPENDED = 0 
                   AND (EXPIRE_DATE IS NULL 
	                   OR EXPIRE_DATE > GETDATE())");
        }
       
        bool isUserOnly = !string.IsNullOrEmpty(userSql);
        if (string.IsNullOrEmpty(groupSql)) groupSql = @"
             DECLARE @TABLE TABLE(
                    GROUP_ID NVARCHAR(50)
                    )

        INSERT INTO @TABLE
             SELECT GROUP_ID
               FROM TB_EB_GROUP
              WHERE GROUP_ID = @GROUP_ID
              WHILE @@ROWCOUNT > 0 
                BEGIN
                  INSERT INTO @TABLE --找到所有下層部門
                       SELECT GROUP_ID
                         FROM TB_EB_GROUP
                        WHERE PARENT_GROUP_ID IN(SELECT GROUP_ID
                                                   FROM @TABLE)
                          AND GROUP_ID NOT IN(SELECT GROUP_ID
                                                FROM @TABLE)
                          AND (ACTIVE = 1 OR ACTIVE IS NULL) --表示沒停用
                END
                SELECT * FROM TB_EB_GROUP WHERE GROUP_ID IN (SELECT GROUP_ID FROM @TABLE)
        ";
        if (string.IsNullOrEmpty(userSql)) userSql = @"
	        SELECT [TB_EB_USER].[USER_GUID],
				   [GROUP_ID],
		           [NAME] + '(' +[TB_EB_USER].[ACCOUNT] + ')' AS 'NAME',
		           [TB_EB_USER].[ACCOUNT] AS 'ACCOUNT'
	          FROM [TB_EB_EMPL_DEP]
	    INNER JOIN [TB_EB_USER]
				ON TB_EB_EMPL_DEP.GROUP_ID = @GROUP_ID
			   AND TB_EB_EMPL_DEP.USER_GUID = TB_EB_USER.USER_GUID
			   AND (IS_SUSPENDED = 0 
				OR IS_SUSPENDED IS NULL)
			   AND (EXPIRE_DATE IS NULL 
				OR EXPIRE_DATE > GETDATE())
        ";
        if (string.IsNullOrEmpty(searchSql)) searchSql = @"
	        SELECT [TB_EB_USER].[USER_GUID],
				   [GROUP_ID],
		           [NAME] + '(' +[TB_EB_USER].[ACCOUNT] + ')' AS 'NAME',
		           [TB_EB_USER].[ACCOUNT] AS 'ACCOUNT'
	          FROM [TB_EB_EMPL_DEP]
	    INNER JOIN [TB_EB_USER]
				ON TB_EB_EMPL_DEP.GROUP_ID IN ({0})
               AND ([TB_EB_USER].[ACCOUNT] LIKE '%' + @SEARCHTEXT + '%' OR [TB_EB_USER].[NAME] LIKE '%' + @SEARCHTEXT + '%')
			   AND TB_EB_EMPL_DEP.USER_GUID = TB_EB_USER.USER_GUID
			   AND (IS_SUSPENDED = 0 
				OR IS_SUSPENDED IS NULL)
			   AND (EXPIRE_DATE IS NULL 
				OR EXPIRE_DATE > GETDATE())
        ";
        this.GroupInfo = this.DataToDictionary(string.Format("{0}^{1}", groupConnectionString, groupSql));
        this.UserInfo = this.DataToDictionary(string.Format("{0}^{1}", userConnectionString, userSql));
        this.SearchInfo = this.DataToDictionary(string.Format("{0}^{1}", userConnectionString, searchSql));

        if (this.isGroup)
        {
            Right1.Attributes["Class"] = "Right1 SelectGroup";
            Right2.Attributes["Class"] = "Right2 hidden";
        }
        else
        {
            Right1.Attributes["Class"] = "Right1";
            Right2.Attributes["Class"] = "Right2 SelectUser";
            btnOpen.Attributes["Class"] = "hidden btnOpen SelectUser";
        }

        if (!Page.IsPostBack) // 首次載入網頁
        {
            gvMain.DataSource = this.GetGroup(groupID);
            gvMain.DataBind();
            gvSelect.DataSource = this.CreategvSelect("gvSelect");
            gvSelect.DataBind();
            if (jArray != null)
            {
                foreach (JObject jObj in jArray)
                {
                    string str_group = jObj["Group"].ToString();
                    DataTable dt = (DataTable)ViewState["gvSelect"];
                    foreach (string str in jObj["Users"])
                    {
                        DataRow dr = dt.NewRow();
                        EBUser user = new UserUCO().GetEBUser(str);
                        dr["GROUP_ID"] = str_group;
                        string[] AccStr = user.Account.Split('\\');
                        dr["ACCOUNT"] = AccStr.Length > 1 ? AccStr[1] : AccStr[0];
                        dr["GUID"] = str;
                        dr["NAME"] = string.Format("{0}({1})", user.Name, user.Account);
                        dt.Rows.Add(dr);
                    }
                    ViewState["gvSelect"] = dt;
                    gvSelect.DataSource = dt;
                    gvSelect.DataBind();
                }
            }
            if (gvMain.Rows.Count > 0)
            {
                chkGROUPID_CheckedChanged(gvMain.Rows[0].FindControl("chkGROUPID"), null);
            }
            Right1.Visible = !isUserOnly;
        }
        else // 如果是POSTBACK
        {
        }
    }

    /// <summary>
    /// ,分隔資料
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, string> DataToDictionary(string data)
    {
        string[] datas = data.Split('^');
        Dictionary<string, string> result = new Dictionary<string, string>();
        result.Add("ConnectionString", datas[0]);
        result.Add("SqlString", datas[1]);
        return result;
    }

    /// <summary>
    /// 取得需要的部門
    /// </summary>
    /// <returns></returns>
    private DataTable GetGroup(string id)
    {

        using (SqlDataAdapter sda = new SqlDataAdapter(this.GroupInfo["SqlString"], this.GroupInfo["ConnectionString"]))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@GROUP_ID", id);
            try
            {
                sda.Fill(ds);
                DataView dv = new DataView(ds.Tables[0]);
                dv.Sort = "RGT DESC";
                dv.Sort = "LEV";
                dv.Sort = "LFT";
                ViewState["gvitems"] = dv.ToTable();
                return dv.ToTable();
            }
            catch (Exception ex) { }
        }
        return null;
    }

    private DataTable CreategvSelect(string vs)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("GROUP_ID", typeof(String))); //群組編號
        dt.Columns.Add(new DataColumn("ACCOUNT", typeof(String))); //會員編號
        dt.Columns.Add(new DataColumn("GUID", typeof(String))); //會員編號
        dt.Columns.Add(new DataColumn("NAME", typeof(String))); //會員顯示名稱
        ViewState[vs] = dt;
        return dt;
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["gvitems"];
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            HtmlGenericControl FItemDiv = (HtmlGenericControl)gr.FindControl("FItemDiv");
            FItemDiv.Style["padding-left"] = int.Parse(dt.Rows[gr.RowIndex]["LEV"].ToString()) * 20 + "px";
        }
    }

    protected void gvSub_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["gvSub"];
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblgvSelect = (DataTable)ViewState["gvSelect"];
            List<string> consSelect = new List<string>();
            foreach (DataRow row in tblgvSelect.Rows) consSelect.Add(row["ACCOUNT"].ToString());
            CheckBox chkUser = (CheckBox)gr.FindControl("chkUSERNAME");
            chkUser.Checked = consSelect.Contains(dt.Rows[gr.RowIndex]["ACCOUNT"].ToString());
        }
    }

    protected void gvSelect_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    /// <summary>
    /// 當選擇部門
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkGROUPID_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox _chk = (CheckBox)sender;
        GridViewRow gr = _chk.NamingContainer as GridViewRow;
        txtSearch.Text = "";
        string GROUPID = ((HiddenField)gr.FindControl("hidGROUPID")).Value;
        CurrentGroup = GROUPID;
        if (this.isGroup) //選擇部門 -- 帶入使用者
        {
            DataTable tblgvItem = (DataTable)ViewState["gvSelect"];
            if (_chk.Checked)
            {
                DataTable dt = this.GetUserWithGroup(GROUPID);
                List<string> checkName = new List<string>();
                foreach (DataRow drs in tblgvItem.Rows) checkName.Add(drs["ACCOUNT"].ToString());
                foreach (DataRow row in dt.Rows)
                {
                    string account = row["ACCOUNT"].ToString();
                    if (!checkName.Contains(account))
                    {
                        DataRow dr = tblgvItem.NewRow();
                        dr["GROUP_ID"] = GROUPID;
                        dr["ACCOUNT"] = account;
                        dr["GUID"] = row["USER_GUID"];
                        dr["NAME"] = row["NAME"];
                        tblgvItem.Rows.Add(dr);
                    }
                }
                if (!this.isGroup) _chk.Checked = false;
            }
            else
            {
                int Count = tblgvItem.Rows.Count;
                for (int i = Count - 1; i >= 0; i--)
                {
                    DataRow dr = tblgvItem.Rows[i];
                    string ID = dr["GROUP_ID"].ToString();
                    if (ID == GROUPID)
                    {
                        tblgvItem.Rows.Remove(dr);
                    }
                }
                tblgvItem.AcceptChanges();
            }
            ViewState["gvSelect"] = tblgvItem;
            gvSelect.DataSource = tblgvItem;
            gvSelect.DataBind();
        }
        else //選擇使用者
        {
            DataTable tblgvItem = this.CreategvSelect("gvSub");
            DataTable dt = this.GetUserWithGroup(GROUPID);
            foreach (DataRow row in dt.Rows)
            {
                string guid = row["USER_GUID"].ToString();
                DataRow dr = tblgvItem.NewRow();
                dr["GROUP_ID"] = GROUPID;
                dr["ACCOUNT"] = row["ACCOUNT"];
                dr["GUID"] = row["USER_GUID"];
                dr["NAME"] = row["NAME"];
                tblgvItem.Rows.Add(dr);
            }
            ViewState["gvSub"] = tblgvItem;
            gvSub.DataSource = tblgvItem;
            gvSub.DataBind();
        }
    }

    /// <summary>
    /// 取消使用者
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkMember_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox _chk = (CheckBox)sender;
        DataTable tblgvItem = (DataTable)ViewState["gvSelect"];
        GridViewRow gr = _chk.NamingContainer as GridViewRow;
        tblgvItem.Rows.RemoveAt(gr.RowIndex);
        tblgvItem.AcceptChanges();
        gvSelect.DataSource = tblgvItem;
        gvSelect.DataBind();
        DataTable dtSub = (DataTable)ViewState["gvSub"];
        if (dtSub != null)
        {
            gvSub.DataSource = dtSub;
            gvSub.DataBind();
        }
    }

    /// <summary>
    /// 取得角色With部門
    /// </summary>
    /// <param name="isDepth"></param>
    /// <returns></returns>
    private DataTable GetUserWithGroup(string GROUPID)
    {
        using (SqlDataAdapter sda = new SqlDataAdapter(this.UserInfo["SqlString"], this.UserInfo["ConnectionString"]))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@GROUP_ID", GROUPID);
            try
            {
                sda.Fill(ds);
                return ds.Tables[0];
            }
            catch { }
        }
        return null;
    }

    /// <summary>
    /// 取得角色With搜尋
    /// </summary>
    /// <param name="isDepth"></param>
    /// <returns></returns>
    private DataTable GetUserWithSearch()
    {
        List<string> ItemGroup = new List<string>();
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            -- 建立暫存表
                DECLARE @GROUP TABLE(
	                GROUP_ID NVARCHAR(100),
	                PRIMARY KEY (GROUP_ID)
                )
                -- 找到所有下層部門
                INSERT INTO @GROUP (GROUP_ID)
                VALUES (@GROUP_ID)

                WHILE (@@ROWCOUNT > 0)
                BEGIN
	                INSERT INTO @GROUP (GROUP_ID)
		                 SELECT GROUP_ID 
		                   FROM TB_EB_GROUP 
		                  WHERE PARENT_GROUP_ID IN (SELECT GROUP_ID 
									                  FROM @GROUP)
			                AND GROUP_ID NOT IN (SELECT GROUP_ID 
								                   FROM @GROUP)
                END
				SELECT * FROM @GROUP
            ", this.GroupInfo["ConnectionString"]))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@GROUP_ID", CurrentGroup);
            try
            {
                sda.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ItemGroup.Add(dr["GROUP_ID"].ToString());
                }
            }
            catch (Exception e)
            {
            }
        }
        string resultGROUP = string.Join("','", ItemGroup.ToArray());
        string SearchGroupIDs = string.Format("'{0}'", resultGROUP);
        this.SearchInfo["SqlString"] = string.Format(this.SearchInfo["SqlString"], SearchGroupIDs);

        using (SqlDataAdapter sda = new SqlDataAdapter(this.SearchInfo["SqlString"], this.SearchInfo["ConnectionString"]))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@SEARCHTEXT", this.txtSearch.Text);
            try
            {
                sda.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex) { }
        }
        return null;
    }

    protected void lbtnGet_Click(object sender, EventArgs e)
    {
        DataTable tblgvItem = (DataTable)ViewState["gvSelect"];
        Dialog.SetReturnValue2(JsonConvert.SerializeObject(tblgvItem));
        Dialog.Close(this);
    }

    protected void chkUSERNAME_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox _chk = (CheckBox)sender;
        GridViewRow gr = _chk.NamingContainer as GridViewRow;
        DataTable tblgvSub = (DataTable)ViewState["gvSub"];
        DataTable tblgvSelect = (DataTable)ViewState["gvSelect"];
        if (_chk.Checked)
        {
            if (isSingle) tblgvSelect = CreategvSelect("gvSelect");
            DataRow dr = tblgvSelect.NewRow();
            DataRow row = tblgvSub.Rows[gr.RowIndex];
            dr["GROUP_ID"] = row["GROUP_ID"];
            dr["ACCOUNT"] = row["ACCOUNT"];
            dr["GUID"] = row["GUID"];
            dr["NAME"] = row["NAME"];
            tblgvSelect.Rows.Add(dr);
        }
        else
        {
            int count = tblgvSelect.Rows.Count;
            string key = tblgvSub.Rows[gr.RowIndex]["ACCOUNT"].ToString();
            for (int i = 0; i < count; i++)
            {
                if (tblgvSelect.Rows[i]["ACCOUNT"].ToString() == key)
                {
                    tblgvSelect.Rows.RemoveAt(i);
                    tblgvSelect.AcceptChanges();
                    break;
                }
            }
        }
        ViewState["gvSelect"] = tblgvSelect;
        gvSelect.DataSource = tblgvSelect;
        gvSelect.DataBind();
    }

    /// <summary>
    /// 搜尋輸入資料
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        DataTable tblgvItem = this.CreategvSelect("gvSub");
        DataTable dt = this.GetUserWithSearch();
        foreach (DataRow row in dt.Rows)
        {
            string guid = row["USER_GUID"].ToString();
            DataRow dr = tblgvItem.NewRow();
            dr["GROUP_ID"] = row["GROUP_ID"];
            dr["ACCOUNT"] = row["ACCOUNT"];
            dr["GUID"] = row["USER_GUID"];
            dr["NAME"] = row["NAME"];
            tblgvItem.Rows.Add(dr);
        }
        ViewState["gvSub"] = tblgvItem;
        gvSub.DataSource = tblgvItem;
        gvSub.DataBind();
    }
}