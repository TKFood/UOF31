using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eAI.PowerBI.Models;
using eAI.PowerBI.UCO;
using Ede.Uof.Utility.Page.Common;

public partial class CDS_PowerBI_Setting_ModifyPowerBI : BasePage
{
    PowerBIMaintainUCO UCO = new PowerBIMaintainUCO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ((Master_DialogMasterPage)this.Master).Button1OnClick += CDS_PowerBI_Setting_ModifyPowerBI_Button1OnClick;
        ((Master_DialogMasterPage)this.Master).Button2Text = "";

        if (!IsPostBack)
        {
            hfApplicationID.Value = Request["ApplicationID"].ToString();

            if (Request["Mode"].ToString() == "Insert")
            {
                txtReportID.Text = "";
                txtReportDesc.Text = "";
                txtReportNO.Text = "";
                txtWorkSpaceID.Text = "";
            }


            if (Request["Mode"].ToString() == "Update")
            {
                txtReportID.Text = Request["ReportID"].ToString();
                txtReportDesc.Text = "";
                txtReportNO.Text = "";
                txtWorkSpaceID.Text = Request["WorkSpaceID"].ToString();

                TB_EAI_PowerBIReport Data = UCO.GetTB_EAI_PowerBIReportOne(
                    new TB_EAI_PowerBIReport()
                    {
                        ApplicationID = hfApplicationID.Value,
                        ReportID = txtReportID.Text,
                        WorkSpaceID = txtWorkSpaceID.Text
                    });

                if (Data != null)
                {
                    txtReportDesc.Text = Data.ReportDesc;
                    txtReportNO.Text = Data.ReportNO;
                    hfReportID.Value = Data.ReportID;
                    hfWorkSpaceID.Value = Data.WorkSpaceID;
                }
            }
        }
    }

    private void CDS_PowerBI_Setting_ModifyPowerBI_Button1OnClick()
    {
        TB_EAI_PowerBIReport Data = new TB_EAI_PowerBIReport()
        {
            ApplicationID = hfApplicationID.Value,
            WorkSpaceID = txtWorkSpaceID.Text.Trim(),
            ReportID = txtReportID.Text.Trim(),
            ReportDesc = txtReportDesc.Text.Trim(),
            ReportNO = txtReportNO.Text.Trim()
        };


        if (Request["Mode"].ToString() == "Insert")
        {
            if (CheckDuplicate(Data))
            {
                if (UCO.InsertTB_EAI_PowerBIReport(Data))
                {
                    Dialog.SetReturnValue2("true");
                }
            }
        }
        else
        {
            if ((txtWorkSpaceID.Text == hfWorkSpaceID.Value
                && txtReportID.Text == hfReportID.Value)
                || CheckDuplicate(Data))
            {
                if (UCO.DeleteTB_EAI_PowerBIReportOne(new TB_EAI_PowerBIReport()
                {
                    ApplicationID = hfApplicationID.Value,
                    WorkSpaceID = hfWorkSpaceID.Value,
                    ReportID = hfReportID.Value
                }))
                {
                    if (UCO.InsertTB_EAI_PowerBIReport(Data))
                    {
                        Dialog.SetReturnValue2("true");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 檢查資料是否重複
    /// </summary>
    /// <param name="workSpaceId"></param>
    /// <param name="reportId"></param>
    /// <returns></returns>
    private bool CheckDuplicate(TB_EAI_PowerBIReport Data)
    {
        TB_EAI_PowerBIReport checkData = UCO.GetTB_EAI_PowerBIReportOne(
            new TB_EAI_PowerBIReport()
            {
                ApplicationID = Data.ApplicationID,
                WorkSpaceID = Data.WorkSpaceID,
                ReportID = Data.ReportID
            });

        cvData.IsValid = checkData == null;
        return cvData.IsValid;
    }
}