using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Page;
using SCSHR;
using System.Data;
using JGlobalLibs;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

/// <summary>
/// UOF Training 自定選單 UOF 內頁設計
/// </summary>
public partial class cds_dreamful_wstest : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            base.AddSiteMapNode("WSTEST");
			
            SCSServicesProxy service = new SCSServicesProxy(SCSHRConfiguration.SCSServicesURL, SCSHRConfiguration.SCSServicesCompanyId, SCSHRConfiguration.SCSServicesAccount, SCSHRConfiguration.SCSServicesPassword);
            Exception ex;
            DataTable tblResult = service.BOFind("HUM0010300", "TMP_PDEPARTID,TMP_PDEPARTNAME,PDEPARTID,MANAGERID,TMP_MANAGERID,TMP_MANAGERNAME", out ex);
            if (ex == null)
            {
                gvTest.DataSource = tblResult;
                gvTest.DataBind();
            }
            tblResult = service.BOFind("HUM0020100", "TMP_VTITLEDEPARTID,TMP_DEPARTID,TMP_DEPARTNAME,JOBSTATUS,StartDate,SEX,PSNEMAIL,COMMTEL,TMP_DutyNAME,SeparationDate,IDNO,BIRTHDATE", out ex);
            if (ex == null)
            {
                gvTest2.DataSource = tblResult;
                gvTest2.DataBind();
            }
            service.Logout();
        }
    }

    protected void btnClearAllGroupAndMember_Click(object sender, EventArgs e)
    {
        string deptName = "";
        try
        {
            //System.Configuration.Configuration appConfig =
            //       ConfigurationManager.OpenExeConfiguration(
            //           new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            //deptName = appConfig.AppSettings.Settings["暫存部門"].Value;
            deptName = "aaaa千附";
            if (!string.IsNullOrEmpty(deptName))
            {
                using (SqlConnection conn = new SqlConnection(new Ede.Uof.Utility.Data.DatabaseHelper().Command.Connection.ConnectionString))
                {
                    try { conn.Open(); }
                    catch (Exception ex) { }
                    if (conn.State == ConnectionState.Open)
                    {
                        using (SqlTransaction trans = conn.BeginTransaction())
                        using (SqlDataAdapter sda = new SqlDataAdapter(string.Format(@"
                    
DECLARE @PARENT_GROUP_ID NVARCHAR(64)

SELECT @PARENT_GROUP_ID = GROUP_ID FROM TB_EB_GROUP WHERE GROUP_NAME = '{0}'

SET NOCOUNT ON
            -- TRUNCATE TABLE ZTB_EB_GROUP -- 刪除匯入旗標
            DELETE -- 刪除指定部門的第一層子部門
              FROM TB_EB_GROUP
             WHERE PARENT_GROUP_ID = @PARENT_GROUP_ID
            WHILE @@ROWCOUNT > 0 BEGIN -- 如果上一道指令有刪除記錄
                DELETE -- 刪除沒有父層的子部門
                  FROM TB_EB_GROUP
                 WHERE PARENT_GROUP_ID NOT IN (SELECT GROUP_ID
                                                 FROM TB_EB_GROUP)
            END
            DELETE -- 刪除部門不存在的帳號
              FROM TB_EB_USER
             WHERE USER_GUID IN (SELECT USER_GUID
                                   FROM TB_EB_EMPL_DEP
                                  WHERE GROUP_ID NOT IN (SELECT GROUP_ID
                                                           FROM TB_EB_GROUP))
            DELETE -- 刪除沒有部門的帳號
              FROM TB_EB_USER
             WHERE USER_GUID NOT IN (SELECT USER_GUID FROM TB_EB_EMPL_DEP)

            DELETE -- 刪除部門不存在的部門使用者關聯資料
              FROM TB_EB_EMPL_DEP
             WHERE GROUP_ID NOT IN (SELECT GROUP_ID
                                      FROM TB_EB_GROUP)
            DELETE -- 刪除部門不存在的部門使用者職務資料
              FROM TB_EB_EMPL_FUNC
             WHERE GROUP_ID NOT IN (SELECT GROUP_ID
                                      FROM TB_EB_GROUP)

            DELETE -- 刪除帳號不存在的HR資料
              FROM TB_EB_EMPL
             WHERE USER_GUID NOT IN (SELECT USER_GUID
                                       FROM TB_EB_USER)

            DELETE -- 刪除帳號不存在的HR資料
              FROM TB_EB_EMPL_HR
             WHERE USER_GUID NOT IN (SELECT USER_GUID
                                       FROM TB_EB_USER)

   EXEC SP_EB_ORG_Rebuild
                    ", deptName), conn))
                        using (DataSet ds = new DataSet())
                        {
                            sda.SelectCommand.Transaction = trans;
                            sda.SelectCommand.Parameters.Add(new SqlParameter("", ""));
                            try
                            {
                                sda.Fill(ds);
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
            DebugLog.Log(string.Format(@"wstest.btnClearAllGroupAndMember_Click.Rollback: {0}", ex.Message));
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            DebugLog.Log(string.Format(@"wstest.btnClearAllGroupAndMember_Click.Error: {0}", ex.Message));
            DebugLog.Log(string.Format(@"wstest.btnClearAllGroupAndMember_Click.StackTrace: {0}", ex.StackTrace));
        }
        
        
    }
}