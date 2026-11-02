using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using JGlobalLibs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using UOFAssist.WKF;


/**
* 修改時間：2021/06/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.所有有使用EBUser的地方，都改為呼叫通用方法取得人員資訊
* 修改原因：
    * 1.修改規格，UOF的EBUser有時候會異常取不到人員資訊，以防再多花時間去查明原因，改為通用方法直接查SQL方式取得人員資訊
* 修改位置： 
    * 1.「Page_Load()」中，註解所有EBUser，改為KYT_EBUser
* **/

/**
* 修改時間：2020/04/30
* 修改人員：陳緯榕
* 修改項目：
    * 要能夠查詢「自己的部門」
* 發生原因：
    * 飛騰請假單需要在代理人找不到時，查詢自己的部門人員，而在前端給予SQL就超出HTML GET長度限制
* 修改位置：
    * 「Page_Load」中，抓取Request參數〈isSearchSelfGroupUsers〉的值
    * 「Page_Load」中，當〈isSearchSelfGroupUsers〉為〈true〉時，給予〈userSql〉查詢的SQL，查詢對象先去查詢〈TB_EB_EMPL_DEP-GROUP_ID〉取出所有的〈USER_GUID〉
    * 「Page_Load」中，要讓〈isSearchSelfGroupUsers〉的效果生效，需搭配〈GROUP_ID〉
* **/

/**
* 修改時間：2020/02/20
* 修改人員：陳緯榕
* 修改項目：
    * 搜尋功能必須是：只能搜尋自己部門和底下的部門人員
* 發生原因：
    * 常昇原先的寫法會找「部門」區塊所有的部門，所以是「找全部」
* 修改位置：
    * 新增全域變數「STRING CurrentGroup」
    * 「chkGROUPID_CheckedChanged」中，使用〈CurrentGroup〉紀錄當前〈hidGROUPID〉的值
    * 「GetUserWithSearch」中，使用〈SQL〉找到所有下層部門，並且存入〈ItemGroup〉集合中
* **/

public partial class CDS_SCSHR_WKFFields_QUERYWINDOWS_SelectUserFilterGroup : BasePage
{
    private bool isSingle;
    private bool isGroup;
    private Dictionary<string, string> GroupInfo = new Dictionary<string, string>();
    private Dictionary<string, string> UserInfo = new Dictionary<string, string>();
    private Dictionary<string, string> SearchInfo = new Dictionary<string, string>();
    private string ConnectionString;
    private static string CurrentGroup = "";
    //private Dictionary<string, string> GroupParams = new Dictionary<string, string>();
    //private Dictionary<string, string> UserParams = new Dictionary<string, string>();

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
	                   [NAME],
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

        //string selectUser = Request["SelectUser"] != null ? Request["SelectUser"] : "";
        //string selectGroup = Request["SelectGroup"] != null ? Request["SelectGroup"] : "";
        //string groupConnectionString = Request["GroupConnectionString"] != null ? Request["GroupConnectionString"] : "";
        //string userConnectionString = Request["UserConnectionString"] != null ? Request["UserConnectionString"] : "";
        //string groupParams = Request["GroupParams"] != null ? Request["GroupParams"] : "";
        //string userParams = Request["UserParams"] != null ? Request["UserParams"] : "";
        //if (string.IsNullOrEmpty(groupConnectionString)) groupConnectionString = this.ConnectionString;
        //if (string.IsNullOrEmpty(userConnectionString)) userConnectionString = this.ConnectionString;
        //this.GroupParams = this.ParamsToDictionary(groupParams);
        //this.UserParams = this.ParamsToDictionary(userParams);
        // 取得資料庫連通字串
        //ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
        //ConnectionString = ConfigurationManager.ConnectionStrings["SHERATONstring"].ConnectionString;

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
                        //EBUser user = new UserUCO().GetEBUser(str);
                        KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(str); // 人員
                        dr["GROUP_ID"] = str_group;
                        string[] AccStr = KUser.Account.Split('\\');
                        dr["ACCOUNT"] = AccStr.Length > 1 ? AccStr[1] : AccStr[0];
                        dr["GUID"] = str;
                        dr["NAME"] = string.Format("{0}({1})", KUser.Name, KUser.Account);
                        dt.Rows.Add(dr);
                    }
                    ViewState["gvSelect"] = dt;
                    gvSelect.DataSource = dt;
                    gvSelect.DataBind();
                }
                //string[] users = selectUser.Split(',');
                //string[] groups = selectGroup.Split(',');
                //if (users.Length == groups.Length)
                //{
                //    DataTable dt = (DataTable)ViewState["gvSelect"];
                //    for (int i = 0; i < users.Length; i++)
                //    {
                //        DataRow dr = dt.NewRow();
                //        EBUser user = new UserUCO().GetEBUser(users[i]);
                //        dr["GROUP_ID"] = groups[i];
                //        dr["ACCOUNT"] = user.Account;
                //        dr["GUID"] = users[i];
                //        dt.Rows.Add(dr);
                //    }
                //    ViewState["gvSelect"] = dt;
                //    gvSelect.DataSource = dt;
                //    gvSelect.DataBind();
                //}
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

    ///// <summary>
    ///// ,分隔資料Key,Value,Key,Value,Key,Value
    ///// </summary>
    ///// <returns></returns>
    //private Dictionary<string, string> ParamsToDictionary(string keyvalue)
    //{
    //    string[] keyvalues = keyvalue.Split(',');
    //    Dictionary<string, string> result = new Dictionary<string, string>();
    //    if (keyvalues.Length % 2 != 0) return result;
    //    for (int i = 0; i < keyvalues.Length; i += 2)
    //    {
    //        result.Add(keyvalues[i], keyvalues[i + 1]);
    //    }
    //    return result;
    //}

    /// <summary>
    /// 取得需要的部門
    /// </summary>
    /// <returns></returns>
    private DataTable GetGroup(string id)
    {
        //List<string> Deprecated = new List<string>();
        //List<string> UseAllGroup = new List<string>();
        //foreach (string guid in guids) UseAllGroup.Add(guid);

        using (SqlDataAdapter sda = new SqlDataAdapter(this.GroupInfo["SqlString"], this.GroupInfo["ConnectionString"]))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@GROUP_ID", id);
            try
            {
                sda.Fill(ds);
                //List<string> ItemGroup = new List<string>();
                //foreach (DataRow dr in ds.Tables[0].Rows) ItemGroup.Add(dr["GROUP_ID"].ToString());
                //string resultGROUP = string.Join("','", ItemGroup.ToArray());
                //this.SearchGroupIDs = string.Format("'{0}'", resultGROUP);
                //this.SearchInfo["SqlString"] = string.Format(this.SearchInfo["SqlString"], this.SearchGroupIDs);
                DataView dv = new DataView(ds.Tables[0]);
                dv.Sort = "RGT DESC";
                dv.Sort = "LEV";
                dv.Sort = "LFT";
                ViewState["gvitems"] = dv.ToTable();
                return dv.ToTable();
                //return tblGroup;
                //Dictionary<string, string> datas = new Dictionary<string, string>();
                //foreach (DataRow dr in ds.Tables[0].Rows) datas.Add(dr["GROUP_ID"].ToString(), dr["PARENT_GROUP_ID"] != Convert.DBNull ? dr["PARENT_GROUP_ID"].ToString() : "0");
                //while (datas.Count > 0)
                //{
                //    List<string> keys = new List<string>(datas.Keys);
                //    foreach (string key in keys)
                //    {
                //        string id = key;
                //        string parent = datas[key];
                //        if (Deprecated.Contains(parent)) //當此父群組為不需要物件
                //        {
                //            Deprecated.Add(id); //將此群組也設定為不需要
                //            datas.Remove(id); //此群組已經分類不需要;
                //            continue;
                //        }
                //        else //有可能是需要物件
                //        {
                //            if (parent == "0" && UseAllGroup.Contains(id)) //如果是最外層公司
                //            {
                //                datas.Remove(id); //已經分類
                //                continue;
                //            }
                //            else if (UseAllGroup.Contains(parent)) //該群組是需要的
                //            {
                //                UseAllGroup.Add(id);
                //                datas.Remove(id);
                //                continue;
                //            }
                //            else if (parent == "0") //最外層的公司不要的狀況
                //            {
                //                Deprecated.Add(id);
                //                datas.Remove(id); //此群組已經分類不需要;
                //                continue;
                //            }
                //        }
                //    }
                //}


                //DataTable dt = new DataTable();
                //dt.Columns.Add(new DataColumn("GROUP_ID", typeof(String))); //群組編號
                //dt.Columns.Add(new DataColumn("GROUP_NAME", typeof(String))); //群組編號
                //dt.Columns.Add(new DataColumn("PARENT_GROUP_ID", typeof(String))); //群組編號
                //dt.Columns.Add(new DataColumn("LEV", typeof(Int32))); //階層
                //dt.Columns.Add(new DataColumn("LFT", typeof(Int32))); //排序Left
                //dt.Columns.Add(new DataColumn("RGT", typeof(Int32))); //排序Right
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    string key = dr["GROUP_ID"].ToString();
                //    if (UseAllGroup.Contains(key))
                //    {
                //        DataRow row = dt.NewRow();
                //        row["GROUP_ID"] = dr["GROUP_ID"];
                //        row["GROUP_NAME"] = dr["GROUP_NAME"];
                //        row["PARENT_GROUP_ID"] = dr["PARENT_GROUP_ID"];
                //        row["LEV"] = dr["LEV"];
                //        row["LFT"] = dr["LFT"];
                //        row["RGT"] = dr["RGT"];
                //        dt.Rows.Add(row);
                //    }
                //}

                //DataView dv = new DataView(dt);
                //dv.Sort = "RGT DESC";
                //dv.Sort = "LEV";
                //dv.Sort = "LFT";
                //ViewState["gvitems"] = dv.ToTable();
                //return dv.ToTable();
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
            //DataTable tblItems = (DataTable)ViewState["gvItems"]; //取出先前記住的 gvItems_voucher
            //DataRow row = tblItems.Rows[gr.DataItemIndex]; //取出對應的資料列
            //((KYTTextBox)gr.FindControl("ktxtAID")).Text = row["ACCNO"].ToString(); //設定代碼
            //((KYTTextBox)gr.FindControl("ktxtACCOUNT")).Text = row["ACCTXT"].ToString(); //設定代碼
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

        //using (SqlDataAdapter sda = new SqlDataAdapter(string.Format(this.SearchInfo["SqlString"], this.SearchGroupIDs), this.SearchInfo["ConnectionString"]))
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
        //StringBuilder sb = new StringBuilder();
        //for (int i = 0; i < tblgvItem.Rows.Count; i++)
        //{
        //    sb.Append(tblgvItem.Rows[i]["GUID"].ToString());
        //    if (i < tblgvItem.Rows.Count - 1) sb.Append(",");
        //}
        //string back = sb.ToString(); //設定代碼
        //Dialog.SetReturnValue2(back);

        //DataTable tblTB_EB_USER = null;
        //DataTable tblTB_EB_GROUP = null;

        //foreach (DataRow row in tblgvItem.Rows)
        //{
        //    using (SqlDataAdapter sda = new SqlDataAdapter(@"
        //        SELECT *
        //          FROM TB_EB_USER
        //         WHERE USER_GUID = @USER_GUID
        //    ", this.ConnectionString))
        //    using (DataSet ds = new DataSet())
        //    {
        //        sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", row["GUID"]);
        //        try
        //        {
        //            sda.Fill(ds);
        //            if (tblTB_EB_USER == null)
        //            {
        //                tblTB_EB_USER = ds.Tables[0];
        //            }
        //            else if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                DataRow oldRow = ds.Tables[0].Rows[0];
        //                DataRow newRow = tblTB_EB_USER.NewRow();
        //                foreach (DataColumn dc in tblTB_EB_USER.Columns)
        //                {
        //                    newRow[dc.ColumnName] = oldRow[dc.ColumnName];
        //                }
        //                tblTB_EB_USER.Rows.Add(newRow);
        //            }
        //        }
        //        catch (Exception ex) { }
        //    }

        //    using (SqlDataAdapter sda = new SqlDataAdapter(@"
        //        SELECT *
        //          FROM TB_EB_GROUP
        //         WHERE GROUP_ID = @GROUP_ID
        //    ", this.ConnectionString))
        //    using (DataSet ds = new DataSet())
        //    {
        //        sda.SelectCommand.Parameters.AddWithValue("@GROUP_ID", row["GROUP_ID"]);
        //        try
        //        {
        //            sda.Fill(ds);
        //            if (tblTB_EB_GROUP == null)
        //            {
        //                tblTB_EB_GROUP = ds.Tables[0];
        //            }
        //            else if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                DataRow oldRow = ds.Tables[0].Rows[0];
        //                DataRow newRow = tblTB_EB_GROUP.NewRow();
        //                foreach (DataColumn dc in tblTB_EB_GROUP.Columns)
        //                {
        //                    newRow[dc.ColumnName] = oldRow[dc.ColumnName];
        //                }
        //                tblTB_EB_GROUP.Rows.Add(newRow);
        //            }
        //        }
        //        catch { }
        //    }
        //}

        //DataSet DS = new DataSet();
        //if (tblTB_EB_USER != null)
        //{
        //    tblTB_EB_USER.TableName = "TB_EB_USER";
        //    DS.Tables.Add(tblTB_EB_USER.Copy());
        //}
        //if (tblTB_EB_GROUP != null)
        //{
        //    tblTB_EB_GROUP.TableName = "TB_EB_GROUP";
        //    DS.Tables.Add(tblTB_EB_GROUP.Copy());
        //}

        //Dialog.SetReturnValue2(JsonConvert.SerializeObject(DS));
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