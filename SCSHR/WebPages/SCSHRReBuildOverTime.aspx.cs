using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Web.UI;

public partial class CDS_SCSHR_WebPages_SCSHRReBuildOverTime : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("飛騰加班單出錯修復", Request.Url.AbsoluteUri);
        }
    }

    protected void btnReCalc_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDOC_NBR.Text.Trim()))
        {
            lblMessage.Text = SCSHR.Utils.ReBuildOVHForm.ReCalcWSAndReplaceOVHForm(txtDOC_NBR.Text.Trim());
        }
    }

    protected void btnReImport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDOC_NBR.Text.Trim()))
        {
            lblMessage.Text = SCSHR.Utils.ReBuildOVHForm.ReCallWSUpdateStatus(txtDOC_NBR.Text.Trim());
        }
    }

    protected void btnAutoFix_Click(object sender, EventArgs e)
    {
        Dictionary<string, Dictionary<string, string>> dicResult = new Dictionary<string, Dictionary<string, string>>();
        dicResult = SCSHR.Utils.ReBuildOVHForm.AutoFixSCSHROverTime();
        // TODO 2019/2/26 將輸出的歷程顯示出來
    }
}
