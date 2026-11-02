using eAI.PowerBI;
using System.Threading.Tasks;
using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eAI.PowerBI.UCO;
using eAI.PowerBI.Models;
using eAI.Base.UCO;

public partial class CDS_PowerBI_Setting_PowerBIReport : BasePage
{

    private readonly EmbedService _embedService = new EmbedService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 系統管理者 PowerBI查閱者
            if (!new BaseUCO().IsUserInRoles(new[] { "SystemAdmin", "PowerBIUser" }))
            {
                Response.Redirect("~/Default.aspx");
            }

            PowerBIMaintainUCO UCO = new PowerBIMaintainUCO();
            TB_EAI_PowerBIReport Report = UCO.GetTB_EAI_PowerBIReportOne(new TB_EAI_PowerBIReport()
            {
                ReportNO = Request["Report"]
            });
            AddSiteMapNode("我的首頁", "~/Homepage.aspx");
            AddSiteMapNode("大數據分析");
            AddSiteMapNode("大數據分析");
            AddSiteMapNode(Report.ReportDesc, "~/CDS/PowerBI/Setting/PowerBIReport.aspx?Report=" + Report.ReportNO);

            Page.RegisterAsyncTask(new PageAsyncTask(GetReport));
        }
    }

    private async Task GetReport()
    {
        PowerBIMaintainUCO UCO = new PowerBIMaintainUCO();
        TB_EAI_PowerBIApplication App = UCO.GetTB_EAI_PowerBIApplication();
        TB_EAI_PowerBIReport Report = UCO.GetTB_EAI_PowerBIReportOne(new TB_EAI_PowerBIReport()
        {
            ApplicationID = App.ApplicationID,
            ReportNO = Request["Report"]
        });

        await this._embedService.EmbedReport(
            "",
            Report.WorkSpaceID,
            Report.ReportID,
            App.AuthorityUrl,
            App.ResourceUrl,
            Report.ApplicationID,
            App.ApiUrl,
            App.UserName,
            App.Password);

        ScriptManager.RegisterStartupScript(
                      this.Page
                      , this.Page.GetType()
                      , Guid.NewGuid().ToString()
                      , string.Format("onSuccess('{0}','{1}','{2}');"
                        , this._embedService.EmbedConfig.EmbedToken.Token
                        , this._embedService.EmbedConfig.EmbedUrl
                        , this._embedService.EmbedConfig.Id)
                      , true);
    }
}