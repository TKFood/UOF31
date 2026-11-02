using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using KYTLog;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/**
 * 修改時間：2018/09/11
 * 修改人員：陳緯榕
 * 修改項目：儲存的表單查不到資料
 * 修正位置：
    * 因為查詢的區間
    * 前端新增顯示查詢日期範圍
    * 「Page_Load」新增查詢日期範圍，以及記錄LOG
 * **/
/**
* 修改時間：2018/10/31
* 修改人員：陳緯榕
* 修改項目：特殊日期必須呼叫另一個〈funcid〉
* 修正位置：
   * 「Page_Load」接收參數〈IS_SPEC〉
   * 「RefreshMainData」新增參數〈string IS_SPEC〉
   * 「RefreshMainData」呼叫WS時，判斷是否有〈IS_SPEC〉，有的話呼叫的Method是〈GetLeaveSpecInfo〉；沒有則是〈CheckLeaveLRData〉
* **/

/**
* 修改時間：2019/04/03
* 修改人員：陳緯榕
* 修改項目：
    * 請假單呼叫飛騰資料出現錯誤
* 修正位置：
   * 「RefreshMainData」中，當〈parameters〉有值時，加上〈DebugLog〉記錄現在的〈parameters〉；然後就正確了........原因未知
* **/
public partial class CDS_SCSHR_WKFFields_QUERYWINDOWS_listLEAVE_TIMES : BasePage
{
    /// <summary>
    /// 資料庫連通字串
    /// </summary>
    private string ConnectionString;

    SCSServicesProxy service = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        ((Master_DialogMasterPage)this.Master).Button2Text = ""; // Button2不顯示
        ((Master_DialogMasterPage)this.Master).Button1Text = ""; // Button1不顯示
        // 取出資料庫連通字串
        ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
        service = ConstructorCommonSettings.setSCSSServiceProxDefault();

        if (!Page.IsPostBack) // 首次載入網頁
        {
            ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
            string EMPID = Request["EMPID"] != null ? Request["EMPID"] : "";
            string LEVCODE = Request["LEVCODE"] != null ? Request["LEVCODE"] : "";
            string LEVNAME = Request["LEVNAME"] != null ? Request["LEVNAME"] : "";
            string LEVDATE_START = Request["LEVDATE_START"] != null ? Request["LEVDATE_START"] : "";
            string LEVDATE_END = Request["LEVDATE_END"] != null ? Request["LEVDATE_END"] : "";
            string IS_SPEC = Request["IS_SPEC"] != null ? Request["IS_SPEC"] : "";
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"listLEAVE_TIMES.Page_Load.!Page.IsPostBack.Params:
               EMPID: {0}, 
               LEVCODE: {1},
               LEVNAME: {2}, 
               LEVDATE_START: {3}, 
               LEVDATE_END: {4},
               IS_SPEC: {5}", EMPID, LEVCODE, LEVNAME, LEVDATE_START, LEVDATE_END, IS_SPEC));
            if (!string.IsNullOrEmpty(EMPID) &&
                !string.IsNullOrEmpty(LEVCODE))
            {
                DateTime dtSt = DateTime.MinValue;
                DateTime.TryParse(LEVDATE_START, out dtSt);
                if (dtSt == DateTime.MinValue)
                    dtSt = DateTime.Now;
                DateTime dtEnd = DateTime.MinValue;
                if (!string.IsNullOrEmpty(LEVDATE_END))
                    DateTime.TryParse(LEVDATE_END, out dtEnd);
                else
                {
                    // 如果沒有給結束時間，就查這個月的前後半年
                    dtEnd = dtSt.AddMonths(6);
                    dtSt = dtSt.AddMonths(-6);
                }
                lblSearchRange.Text = string.Format("{0} ~ {1}", dtSt.ToString("yyyy/MM/dd"), dtEnd.ToString("yyyy/MM/dd"));
                RefreshMainData(EMPID, LEVCODE, LEVNAME, dtSt.ToString("yyyyMMdd"), dtEnd.ToString("yyyyMMdd"), IS_SPEC);
            }
            else
                lblMessage.Text = "無法查詢";
        }
        else // 如果是POSTBACK
        {
        }
    }

    private void RefreshMainData(string EMPID, string LEVCODE, string LEVNAME, string LEVDATE_START, string LEVDATE_END, string IS_SPEC)
    {
        Exception ex = null;
        SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSLeaveProgID,
           string.IsNullOrEmpty(IS_SPEC) ? "CheckLeaveLRData" : "GetLeaveSpecInfo",
            SCSHR.Types.SCSParameter.getPatameters(new { TMP_EmployeeID = EMPID, Tmp_svacationID = LEVCODE, StartDate = LEVDATE_START, EndDate = LEVDATE_END }),
            out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"listLEAVE_TIMES.RefreshMainData.CheckLeaveLRData.ERROR:{0}", ex.Message));
        if (parameters != null &&
                    parameters.Length > 0)
        {
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"listLEAVE_TIMES.RefreshMainData.CheckLeaveLRData.result:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));
            if (parameters[0].DataType.ToString() == "DataTable")
            {
                DataTable dtSource = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                dtSource.Columns.Add("LEVNAME", typeof(string));
                foreach (DataRow dr in dtSource.Rows)
                {
                    dr["LEVNAME"] = LEVNAME;
                }
                gvMain.DataSource = dtSource;
                gvMain.DataBind();
            }
        }

    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}
