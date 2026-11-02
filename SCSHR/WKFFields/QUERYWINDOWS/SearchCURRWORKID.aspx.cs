using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using KYTLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;


public partial class CDS_PFBPM_WKFFields_QUERYWINDOW_SearchCURRWORKID : BasePage
{
    /// <summary>
    /// 資料庫連通字串
    /// </summary>
    private string ConnectionString;

    // static string USER_GUID;

    protected void Page_Load(object sender, EventArgs e)
    {
        ((Master_DialogMasterPage)this.Master).Button2Text = ""; // Button2不顯示
        ((Master_DialogMasterPage)this.Master).Button1Text = ""; // Button1不顯示

        // 取得資料庫連通字串
        ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;

        if (!Page.IsPostBack) // 首次載入網頁
        {
            //  USER_GUID = Request["USER_GUID"] != null ? Request["USER_GUID"] : "";
            gvMain.DataSource = RefreshgvMain();
            gvMain.DataBind();
        }
        else // 如果是POSTBACK
        {
        }
    }

    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMain.PageIndex = e.NewPageIndex;
        gvMain.DataSource = ViewState[gvMain.ID];
        gvMain.DataBind();
    }

    /// <summary>
    /// 取回事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnGet_Click(object sender, EventArgs e)
    {
        LinkButton lbSelect = sender as LinkButton; // 取得點擊按鈕
        GridViewRow gr = lbSelect.NamingContainer as GridViewRow; // 取得所在GridViewRow
        DataTable tblCURRWORKID = ViewState[gvMain.ID] as DataTable; // 取得先前記住的table
        DataRow row = tblCURRWORKID.Rows[gr.DataItemIndex]; // 取得對映DataRow
        DataTable dtReturn = new DataTable();
        DataRow ndr = dtReturn.NewRow();
        foreach (DataColumn dc in tblCURRWORKID.Columns)
        {
            if (!dtReturn.Columns.Contains(dc.ColumnName)) // dc.ColumnName不存在於dtReturn
                dtReturn.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
            if (dc.DataType == typeof(string))
            {
                ndr[dc.ColumnName] = row[dc.ColumnName].ToString().Replace("'", "#&#&##").Replace("\"", "&#&#&&");
            }
            //ndr[dc.ColumnName] = row[dc.ColumnName].ToString().Replace("'", "#&#&##").Replace("\"", "&#&#&&");
        }
        dtReturn.Rows.Add(ndr);
        Dialog.SetReturnValue2(Newtonsoft.Json.JsonConvert.SerializeObject(dtReturn));
        Dialog.Close(this);
    }

    /// <summary>
    /// 輸入查詢條件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        gvMain.DataSource = RefreshgvMain();
        gvMain.DataBind();
    }

    /// <summary>
    /// 尋找任務
    /// </summary>
    /// <returns></returns>
    private DataTable RefreshgvMain()
    {
        // 呼叫 飛騰WS取得班別
        SCSServicesProxy service = ConstructorCommonSettings.setSCSSServiceProxDefault();
        DataTable dtScource = new DataTable(); // call飛騰帶回來的資料
        DataTable dtNew = new DataTable(); // 查詢班別


        Exception ex = null;
        dtScource = service.BOFind("ATT0010500", "*", out ex);
        if (ex != null)
        {
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_TRAVEL.getBOFind.service.Error:{0}", ex.Message));
            return dtNew;
        }

        dtScource.Columns.Add(new DataColumn("HTYPE", typeof(string)));
        // 休息日
        DataRow ndr = dtScource.NewRow();
        ndr["SYS_VIEWID"] = "H"; // 班別代號
        ndr["HTYPE"] = "1"; // HTYPE
        ndr["SYS_NAME"] = "休息日"; // 班別名稱
        dtScource.Rows.Add(ndr);

        // 例假日
        ndr = dtScource.NewRow();
        ndr["SYS_VIEWID"] = "H2"; // 班別代號
        ndr["HTYPE"] = "2"; // HTYPE
        ndr["SYS_NAME"] = "例假日"; // 班別名稱
        dtScource.Rows.Add(ndr);

        // 變形休息日
        ndr = dtScource.NewRow();
        ndr["SYS_VIEWID"] = "H3"; // 班別代號
        ndr["HTYPE"] = "3"; // HTYPE
        ndr["SYS_NAME"] = "變形休息日"; // 班別名稱
        dtScource.Rows.Add(ndr);

        // 國定假日
        ndr = dtScource.NewRow();
        ndr["SYS_VIEWID"] = "H4"; // 班別代號
        ndr["HTYPE"] = "4"; // HTYPE
        ndr["SYS_NAME"] = "國定假日"; // 班別名稱
        dtScource.Rows.Add(ndr);

        // 主表有資料 → 主表Select(filter)查詢 → 給DataRow arrRow → 跑arrRow → 新增DataRow ndrr → 跑主表的Columns
        // → if(新的dtNew是否有存在主表的columnName) → 不存在(新增主表的ColumnName) → 存在(主表的ColumnName新的ndrr)
        // → Column塞回新的dtNew → dr塞回DataRow → 查詢的結果ViewSatate、return。
        // 無輸入查詢也會查,但就是sql條件為空會查全部

        // (1).call飛騰WS取得資料
        // (2).存放到dtSource
        // (3).建立ndr存放帶回來的資料(SYS_VIEWID、SYS_NAME、STARTTIME、ENDTIME)
        // (4).判斷是否有資料
        // (5).在從dtSource Select語法。(第一次進入跳窗則為空，會查所有資料)
        // (6).



        if (dtScource.Rows.Count > 0) // 是否有資料
        {
            // 查詢班別代號、班別名稱、上班時間、下班時間
            string filter = "";
            filter += "SYS_VIEWID LIKE '%" + txtSearch.Text + "%' OR SYS_NAME LIKE '%" + txtSearch.Text + "%' OR STARTTIME LIKE '%" + txtSearch.Text + "%' OR ENDTIME LIKE '%" + txtSearch.Text + "%'";
            DataRow[] arrRow = dtScource.Select(filter, "HTYPE ASC");
            foreach (DataRow dr in arrRow)
            {
                DataRow ndr_New = dtNew.NewRow();
                foreach (DataColumn dc in dtScource.Columns)
                {
                    if (!dtNew.Columns.Contains(dc.ColumnName))
                    {
                        dtNew.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                    }
                    if (dc.ColumnName == "HTYPE" && dr[dc.ColumnName].ToString() == "")
                    {
                        ndr_New[dc.ColumnName] = "0";
                    }
                    else
                    {
                        ndr_New[dc.ColumnName] = dr[dc.ColumnName];
                    }
                }
                dtNew.Rows.Add(ndr_New);
            }
        }
        ViewState[gvMain.ID] = dtNew;
        return dtNew;
    }
}
