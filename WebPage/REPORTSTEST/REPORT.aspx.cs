using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using FastReport;
using FastReport.Web;
using FastReport.Data;
using System.Text;

public partial class CDS_WebPage_REPORT_REPORT : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SETFASTREPORT();
        }
            
    }

    #region FUNCTION
    public void SETFASTREPORT()
    {
        string SDATE = DateTime.Now.ToString("yyyyMM") + "01";
        string EDATE = DateTime.Now.ToString("yyyyMM") + "31";
        string YEARS = DateTime.Now.Year.ToString();
        string MONTHS = DateTime.Now.Month.ToString();

        WebReport1.Report.SetParameterValue("SDATE", SDATE);
        WebReport1.Report.SetParameterValue("EDATE", EDATE);
        WebReport1.Report.SetParameterValue("YEARS", YEARS);
        WebReport1.Report.SetParameterValue("MONTHS", MONTHS);


    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        string SDATE = txtDate1.Text.ToString()+ txtDate2.Text.ToString().PadLeft(2, '0') + "01";
        string EDATE = txtDate1.Text.ToString() + txtDate2.Text.ToString().PadLeft(2, '0') + "31";
        string YEARS = txtDate1.Text.ToString();
        string MONTHS = txtDate2.Text.ToString();

        WebReport1.Report.SetParameterValue("SDATE", SDATE);
        WebReport1.Report.SetParameterValue("EDATE", EDATE);
        WebReport1.Report.SetParameterValue("YEARS", YEARS);
        WebReport1.Report.SetParameterValue("MONTHS", MONTHS);
    }

    #endregion



}