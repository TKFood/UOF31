using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// UOF Training 自定選單 UOF 內頁設計
/// </summary>
public partial class CDS_KYTUtils_TerminateTasks : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
        }
        else
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT '' AS 'TASK_ID',
                    '--全部作廢--' AS 'DISPLAY'
            UNION ALL
            SELECT TASK_ID AS 'TASK_ID',
                   DOC_NBR + '/' + ISNULL((SELECT TOP 1 ACCOUNT
                                             FROM TB_EB_USER
                                            WHERE USER_GUID = TB_WKF_TASK.USER_GUID), '') AS 'A.DISPLAY'
              FROM TB_WKF_TASK
             WHERE TASK_STATUS = 1
        ", ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString))
        using (DataSet ds = new DataSet())
        {
            try { sda.Fill(ds); }
            catch { }
            ddlTasks.DataTextField = "DISPLAY";
            ddlTasks.DataValueField = "TASK_ID";
            ddlTasks.DataSource = ds.Tables[0];
            ddlTasks.DataBind();

            lblInfo.Text = string.Format("共 {0} 張表單", ds.Tables[0].Rows.Count - 1);
        }
    }

    protected void btnTerminate_Click(object sender, EventArgs e)
    {
        List<dynamic> lstTasks = new List<dynamic>();

        if (ddlTasks.SelectedIndex == 0)
        {
            for (int i = 1; i < ddlTasks.Items.Count; i++)
            {
                ListItem item = ddlTasks.Items[i];
                if (!string.IsNullOrEmpty(item.Text))
                {
                    string DOC_NBR = item.Text.Split('/')[0];
                    string ACCOUNT = item.Text.Split('/')[1];
                    string TASK_ID = item.Value;
                    lstTasks.Add(new
                    {
                        DOC_NBR = DOC_NBR,
                        ACCOUNT = ACCOUNT,
                        TASK_ID = TASK_ID
                    });
                }
            }
        }
        else
        {
            ListItem item = ddlTasks.SelectedItem;
            if (!string.IsNullOrEmpty(item.Text))
            {
                string DOC_NBR = item.Text.Split('/')[0];
                string ACCOUNT = item.Text.Split('/')[1];
                string TASK_ID = item.Value;
                lstTasks.Add(new
                {
                    DOC_NBR = DOC_NBR,
                    ACCOUNT = ACCOUNT,
                    TASK_ID = TASK_ID
                });
            }
        }

        int total = lstTasks.Count;
        int current = 0;
        foreach (dynamic task in lstTasks)
        {
            current++;
            UOFAssist.UOF.WebServices.TransferFormWS tfs = new UOFAssist.UOF.WebServices.TransferFormWS();
            string uofurl = KYTUtilLibs.Utils.UOFUtils.UOFSettingData.getUOFConfig("SiteUrl");
            if (!string.IsNullOrEmpty(uofurl))
            {
                uofurl += uofurl.EndsWith("/") ? "wkf/webservice/TransferFormWS.asmx" : "/wkf/webservice/TransferFormWS.asmx";

                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Info, string.Format(@"TerminateTasks.btnTerminate_Click.taskId:{0}, account:{1}, result:Cancel, reason:主單作廢", (string)task.TASK_ID, (string)task.ACCOUNT));
                KYTUtilLibs.Tools.SoapWebService sws = new KYTUtilLibs.Tools.SoapWebService().SetURL(uofurl).SetMethod("TerminateTask").SetNamespaceURI("http://www.1st-excellence.com");
                string result = sws.Invoke<string>(new { taskId = (string)task.TASK_ID, account = (string)task.ACCOUNT, result = "Cancel", reason = "主單作廢" });
            }
        }

        Refresh();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dtTest = new DataTable();
        dtTest.Columns.Add(new DataColumn("DOC_NBR", typeof(string)));
        dtTest.Columns.Add(new DataColumn("TASK_STATUS", typeof(int)));
        dtTest.Columns.Add(new DataColumn("TASK_RESULT", typeof(int)));
        dtTest.Columns.Add(new DataColumn("APPLICANT", typeof(string)));
        dtTest.Columns.Add(new DataColumn("APPLICANTDEPT", typeof(string)));
        dtTest.Columns.Add(new DataColumn("APPLICANTDATE", typeof(DateTime)));
        dtTest.Columns.Add(new DataColumn("UOF_TITLE", typeof(string)));

        for (int i = 0; i < 10000; i++)
        {
            DataRow ndr = dtTest.NewRow();
            ndr["DOC_NBR"] = i.ToString();
            ndr["TASK_STATUS"] = 1;
            ndr["TASK_RESULT"] = 0;
            ndr["APPLICANT"] = "test";
            ndr["APPLICANTDEPT"] = "test";
            ndr["APPLICANTDATE"] = DateTime.Now;
            ndr["UOF_TITLE"] = "title";
            dtTest.Rows.Add(ndr);
        }

        using (SqlConnection conn = new SqlConnection(new DatabaseHelper().Command.Connection.ConnectionString))
        {
            try { conn.Open(); }
            catch (Exception ex) { }
            if (conn.State == System.Data.ConnectionState.Open)
            {
                using (SqlDataAdapter sda = new SqlDataAdapter("DELETE FROM Z_TELERIK_DEMO WHERE DOC_NBR NOT LIKE 'BUL%'", conn))
                using (DataSet ds = new DataSet())
                {
                    sda.SelectCommand.Parameters.Add(new SqlParameter("", ""));
                    try
                    {
                        sda.Fill(ds);
                    }
                    catch (Exception eX)
                    {
                    }
                }
                //using (SqlTransaction trans = conn.BeginTransaction())
                using (SqlBulkCopy sqlBC = new SqlBulkCopy(conn))
                {
                    //設定一個批次量寫入多少筆資料
                    sqlBC.BatchSize = 1000;
                    //設定逾時的秒數
                    sqlBC.BulkCopyTimeout = 60;

                    //設定要寫入的資料庫
                    sqlBC.DestinationTableName = "dbo.Z_TELERIK_DEMO";
                    //開始寫入
                    sqlBC.WriteToServer(dtTest);
                    //trans.Commit();

                }
            }
        }
    }
}