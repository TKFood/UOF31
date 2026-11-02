using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Web.UI;

public partial class CDS_SCSHR_WebPages_SCSHRReBuildForm : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("飛騰表單出錯修復", Request.Url.AbsoluteUri);
        }
    }

    protected void btnReCalcOverTime_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDOC_NBR_OV.Text.Trim()))
        {
            lblOVMessage.Text = SCSHR.Utils.ReBuildSCSHRForm.ReCalcWSAndReplaceOVHForm(txtDOC_NBR_OV.Text.Trim());
        }
    }

    protected void btnReImportOverTime_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDOC_NBR_OV.Text.Trim()))
        {
            lblOVMessage.Text = SCSHR.Utils.ReBuildSCSHRForm.ReCallWSUpdateStatus(txtDOC_NBR_OV.Text.Trim(), SCSHR.Utils.FormType.OverTime);
        }
    }

    protected void btnAutoFix_Click(object sender, EventArgs e)
    {
        Dictionary<string, Dictionary<string, string>> dicResult = new Dictionary<string, Dictionary<string, string>>();
        dicResult = SCSHR.Utils.ReBuildSCSHRForm.AutoFixSCSHRForm();
        // TODO 2019/2/26 將輸出的歷程顯示出來
    }

    protected void btnReImportLeave_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDOC_NBR_OV.Text.Trim()))
        {
            lblLEVMessage.Text = SCSHR.Utils.ReBuildSCSHRForm.ReCallWSUpdateStatus(txtDOC_NBR_LEV.Text.Trim(), SCSHR.Utils.FormType.Leave);
        }
    }

    protected void btnReImportTravel_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDOC_NBR_OV.Text.Trim()))
        {
            lblTRAMessage.Text = SCSHR.Utils.ReBuildSCSHRForm.ReCallWSUpdateStatus(txtDOC_NBR_TRA.Text.Trim(), SCSHR.Utils.FormType.Travel);
        }
    }
}
