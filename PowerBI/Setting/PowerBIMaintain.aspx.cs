using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eAI.PowerBI.UCO;
using eAI.Base.UCO;
using eAI.PowerBI.Models;
using System.Data;
using Ede.Uof.Utility.Page.Common;

public partial class CDS_PowerBI_Setting_PowerBIMaintain : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 系統管理者 PowerBI管理者
            if (!new BaseUCO().IsUserInRoles(new[] { "SystemAdmin", "PowerBIManager" }))
            {
                Response.Redirect("~/Default.aspx");
            }

            AddSiteMapNode("我的首頁", "~/Homepage.aspx");
            AddSiteMapNode("大數據分析");
            AddSiteMapNode("大數據分析");
            AddSiteMapNode("Power BI 設定", "~/CDS/PowerBI/Setting/PowerBIMaintain.aspx");

            ApplicatoinBind();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        txtPassword.Attributes.Add("Value", txtPassword.Text);
    }

    protected void RadTabStrip1_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        if (RadTabStrip1.SelectedIndex == 0)
        {
            ApplicatoinBind();
        }

        if (RadTabStrip1.SelectedIndex == 1)
        {
            ReportBind();
        }
    }

    protected void RadToolBar1_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
    {
        PowerBIMaintainUCO UCO = new PowerBIMaintainUCO();
        switch (e.Item.Value)
        {
            case "Save":
                if (UCO.UploadApplicationID(hfApplicationID.Value, txtApplicationID.Text.Trim()))
                {
                    lblAuthorityUrl2.Text = "https://login.microsoftonline.com/" + txtAuthorityUrl.Text + "/";
                    if (UCO.SaveTB_EAI_PowerBIApplication(
                         new TB_EAI_PowerBIApplication()
                         {
                             ApplicationID = txtApplicationID.Text.Trim(),
                             ApplicationDesc = txtApplicationDesc.Text.Trim(),
                             UserName = txtUserName.Text.Trim(),
                             Password = txtPassword.Text.Trim(),
                             ApiUrl = txtApiUrl.Text.Trim(),
                             ResourceUrl = txtResourceUrl.Text.Trim(),
                             AuthorityUrl = lblAuthorityUrl2.Text.Trim()
                         }
                        , hfApplicationID.Value == ""))
                    {
                        ScriptManager.RegisterStartupScript(
                            this.Page
                            , this.Page.GetType()
                            , Guid.NewGuid().ToString()
                            , "alert('" + lblSaveSuccess.Text + "');"
                            , true);
                        ApplicatoinBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(
                            this.Page
                            , this.Page.GetType()
                            , Guid.NewGuid().ToString()
                            , "alert('" + lblSaveError.Text + "');"
                            , true);
                    }
                }
                break;
        }
    }

    protected void Grid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid1.PageIndex = e.NewPageIndex;
        ReportBind();
    }

    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            LinkButton lbtnReportID = (LinkButton)e.Row.FindControl("lbtnReportID");
            Dialog.Open2(lbtnReportID,
                       "~/CDS/PowerBI/Setting/ModifyPowerBI.aspx",
                       "修改維護視窗",
                      600,
                      300,
                      Dialog.PostBackType.AfterReturn,
                     new
                     {
                         Mode = "Update",
                         ReportID = drv["ReportID"].ToString(),
                         ApplicationID = drv["ApplicationID"].ToString(),
                         WorkSpaceID = drv["WorkSpaceID"].ToString(),
                     }.ToExpando());
        }
    }

    private void ApplicatoinBind()
    {
        PowerBIMaintainUCO UCO = new PowerBIMaintainUCO();
        TB_EAI_PowerBIApplication Data = UCO.GetTB_EAI_PowerBIApplication();
        if (Data != null)
        {
            hfApplicationID.Value = Data.ApplicationID;
            txtApplicationID.Text = Data.ApplicationID;
            txtApplicationDesc.Text = Data.ApplicationDesc;
            txtUserName.Text = Data.UserName;
            txtPassword.Text = Data.Password;
            txtAuthorityUrl.Text = Data.AuthorityUrl.Replace("https://login.microsoftonline.com/", "").Replace("/", "");
            lblAuthorityUrl2.Text = Data.AuthorityUrl;
            txtResourceUrl.Text = Data.ResourceUrl;
            txtApiUrl.Text = Data.ApiUrl;
        }
        else
        {
            hfApplicationID.Value = "";
            txtApplicationID.Text = "";
            txtApplicationDesc.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtAuthorityUrl.Text = "";
            lblAuthorityUrl2.Text = "https://login.microsoftonline.com//";
            txtResourceUrl.Text = "https://analysis.windows.net/powerbi/api";
            txtApiUrl.Text = "https://api.powerbi.com/";
        }
    }

    protected void Grid1_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["Sort1"] = string.Format("{0} {1}"
            , e.SortExpression
            , e.SortDirection == SortDirection.Ascending
                   ? "ASC"
                   : "DESC");
        ReportBind();
    }

    private void ReportBind()
    {
        PowerBIMaintainUCO UCO = new PowerBIMaintainUCO();
        DataTable dt = UCO.GetTB_EAI_PowerBIReport();
        string sort = (string)ViewState["Sort1"];
        dt.DefaultView.Sort = string.IsNullOrEmpty(sort) ? "ReportNO" : sort;
        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void lbtnReportID_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            ReportBind();
        }
    }

    protected void RadToolBar2_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
    {
        switch (e.Item.Value)
        {
            case "Insert":
                ReportBind();
                break;
            case "Delete":
                PowerBIMaintainUCO UCO = new PowerBIMaintainUCO();

                System.Collections.Specialized.IOrderedDictionary[] dataIDs = Grid1.GetSelectedRowsKeys();
                List<TB_EAI_PowerBIReport> Datas = new List<TB_EAI_PowerBIReport>();
                if (dataIDs.Length > 0)
                {
                    for (int i = 0; i < dataIDs.Length; i++)
                    {
                        Datas.Add(
                             new TB_EAI_PowerBIReport
                             {
                                 ApplicationID = dataIDs[i]["ApplicationID"].ToString(),
                                 WorkSpaceID = dataIDs[i]["WorkSpaceID"].ToString(),
                                 ReportID = dataIDs[i]["ReportID"].ToString()
                             });
                    }

                    if (UCO.DeleteTB_EAI_PowerBIReport(Datas))
                    {
                        ReportBind();
                    }
                }
                break;
        }
    }


}