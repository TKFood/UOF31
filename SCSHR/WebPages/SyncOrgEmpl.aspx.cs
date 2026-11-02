using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Page;
using SCSHR.Task;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using JGlobalLibs;
/// <summary>
/// 手動同步人事資料
/// </summary>
public partial class cds_SCSHR_WebPages_SyncOrgEmpl : BasePage
{
    private string ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;

        if (!Page.IsPostBack)
        {
            base.AddSiteMapNode("人員組織同步");

        }
    }
    /// <summary>
    /// 按下開始同步
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnStart_Click(object sender, EventArgs e)
    {
        Task_OrgEmployeeSync.RunTask();
        lblMsg.Text = "同步完畢";
    }
}