using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using JGlobalLibs;
using OfficeOpenXml;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CDS_WebPage_SCSHR : Ede.Uof.Utility.Page.BasePage
{
    public class ConstructorCommonSettings
    {

        public static void setCommonSettings()
        {

        }
        public static SCSServicesProxy setSCSSServiceProxDefault(string SCSURL = "", string SCSCompanyId = "", string SCSAccount = "", string SCSPassword = "")
        {
            return null;
        }
    }

    private string ConnectionString;

    SCSServicesProxy service = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionString = ConfigurationManager.ConnectionStrings["connectionstringUOF"].ConnectionString;
        service = ConstructorCommonSettings.setSCSSServiceProxDefault();

        if (!Page.IsPostBack) // 網頁首次載入
        {
            DateTime Current = DateTime.Today;
            this.kdtpSTARTTIME.SelectedDate = new DateTime(Current.Year, Current.Month, 1);
            this.kdtpENDTIME.SelectedDate = new DateTime(Current.Year, Current.Month, DateTime.DaysInMonth(Current.Year, Current.Month));
            this.RadDatePicker1.SelectedDate = new DateTime(Current.Year, Current.Month, 1);
            this.RadDatePicker2.SelectedDate = new DateTime(Current.Year, Current.Month, DateTime.DaysInMonth(Current.Year, Current.Month));
            //EBUser user = new UserUCO().GetEBUser(Page.User.Identity.Name);
            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(Page.User.Identity.Name); // 人員
            string[] Account = KUser.Account.Split('\\');

        }
    }

    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            switch (((HiddenField)gr.FindControl("hidBOVERTIMESTATUS")).Value) //出勤狀態(A1)
            {
                case "0": ((Label)gr.FindControl("lblBOVERTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblBOVERTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblBOVERTIMESTATUS")).Text = "早到"; break;
                case "3": ((Label)gr.FindControl("lblBOVERTIMESTATUS")).Text = "加班上班未刷卡"; break;
            }
            switch (((HiddenField)gr.FindControl("hidBOFFOVERTIMESTATUS")).Value) //出勤狀態(A2)
            {
                case "0": ((Label)gr.FindControl("lblBOFFOVERTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblBOFFOVERTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblBOFFOVERTIMESTATUS")).Text = "加班下班未刷卡"; break;
            }
            switch (((HiddenField)gr.FindControl("hidBSTATUS")).Value) //加班狀態(A)
            {
                case "0": ((Label)gr.FindControl("lblBSTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblBSTATUS")).Text = "無加班單"; break;
                case "2": ((Label)gr.FindControl("lblBSTATUS")).Text = "有加班單"; break;
            }
            switch (((HiddenField)gr.FindControl("hidWORKTIMESTATUS")).Value) //出勤狀態(B1)
            {
                case "0": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "未刷卡"; break;
                case "3": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "遲到"; break;
                case "4": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "上班未刷卡"; break;
                case "5": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "免刷卡"; break;
                case "6": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "休息日"; break;
                case "7": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "休息日加班"; break;
                case "8": ((Label)gr.FindControl("lblWORKTIMESTATUS")).Text = "晚到(要請假)"; break;
            }
            string keyB1 = ((HiddenField)gr.FindControl("hidWORKTIMESTATUS")).Value;
            ((Label)gr.FindControl("lblWORKTIMESTATUS")).ForeColor = keyB1 != "0" && keyB1 != "1" && keyB1 != "6" ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            switch (((HiddenField)gr.FindControl("hidSTATUS")).Value) //處理狀態(B1)
            {
                case "0": ((Label)gr.FindControl("lblSTATUS")).Text = "未處理"; break;
                case "1": ((Label)gr.FindControl("lblSTATUS")).Text = "已處理"; break;
                case "2": ((Label)gr.FindControl("lblSTATUS")).Text = "正常"; break;
            }
            switch (((HiddenField)gr.FindControl("hidOFFWORKTIMESTATUS")).Value) //出勤狀態(B2)
            {
                case "0": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "未刷卡"; break;
                case "3": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "早退"; break;
                case "4": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "下班未刷卡"; break;
                case "5": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "免刷卡"; break;
                case "6": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "休息日"; break;
                case "7": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "休息日加班"; break;
                case "8": ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).Text = "晚到(要請假)"; break;
            }
            string keyB2 = ((HiddenField)gr.FindControl("hidOFFWORKTIMESTATUS")).Value;
            ((Label)gr.FindControl("lblOFFWORKTIMESTATUS")).ForeColor = keyB2 != "0" && keyB2 != "1" && keyB2 != "6" ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            switch (((HiddenField)gr.FindControl("hidSTATUS2")).Value) //處理狀態(B2)
            {
                case "0": ((Label)gr.FindControl("lblSTATUS2")).Text = "未處理"; break;
                case "1": ((Label)gr.FindControl("lblSTATUS2")).Text = "已處理"; break;
                case "2": ((Label)gr.FindControl("lblSTATUS2")).Text = "正常"; break;
            }
            switch (((HiddenField)gr.FindControl("hidAOVERTIMESTATUS")).Value) //出勤狀態(C1)
            {
                case "0": ((Label)gr.FindControl("lblAOVERTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblAOVERTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblAOVERTIMESTATUS")).Text = "加班上班未刷卡"; break;
            }
            switch (((HiddenField)gr.FindControl("hidAOFFOVERTIMESTATUS")).Value) //出勤狀態(C2)
            {
                case "0": ((Label)gr.FindControl("lblAOFFOVERTIMESTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblAOFFOVERTIMESTATUS")).Text = "正常"; break;
                case "2": ((Label)gr.FindControl("lblAOFFOVERTIMESTATUS")).Text = "晚退"; break;
                case "3": ((Label)gr.FindControl("lblAOFFOVERTIMESTATUS")).Text = "加班下班未刷卡"; break;
            }
            switch (((HiddenField)gr.FindControl("hidASTATUS")).Value) //加班狀態(C)
            {
                case "0": ((Label)gr.FindControl("lblASTATUS")).Text = "空白"; break;
                case "1": ((Label)gr.FindControl("lblASTATUS")).Text = "無加班單"; break;
                case "2": ((Label)gr.FindControl("lblASTATUS")).Text = "有加班單"; break;
            }
        }
    }

    /// <summary>
    /// 按下選擇帳號
    /// </summary>
    protected void UC_ChoiceList1_EditButtonOnClick(UserSet userSet)
    {
        if (userSet.Items.Count <= 0) return;
        //EBUser user = new UserUCO().GetEBUser(Page.User.Identity.Name);
        KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(Page.User.Identity.Name); // 人員
        //檢查是否為部門主管
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SELECT *
                  FROM TB_EB_EMPL_FUNC
                 WHERE FUNC_ID = 'Superior'
				   AND USER_GUID = @GUID
            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@GUID", KUser.UserGUID); //大類名稱
            try
            {
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0) //是部門主管
                {
                    lblAccount.Text = "";
                    string[] Account = userSet.Items[0].EBUsers[0].Account.Split('\\');
                    if (Account.Length > 1) lblAccount.Text = Account[1];
                    else lblAccount.Text = Account[0];
                    for (int i = 1; i < userSet.Items.Count; i++)
                    {
                        lblAccount.Text += ";";
                        Account = userSet.Items[i].EBUsers[0].Account.Split('\\');
                        if (Account.Length > 1) lblAccount.Text += Account[1];
                        else lblAccount.Text += Account[0];
                    }
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// 按下查詢按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
            DataTable dtDBTKHR = new DataTable();
            DataTable dt = new DataTable();
            string accounts = null;
            string addDataColumnflag = "Y";
            DataRow dr = null;
            DataRow row = null;
            StringBuilder cmdTxt = new StringBuilder();
            cmdTxt.AppendFormat(@"
                            SELECT  [EMPID] FROM [TKHR].[dbo].[SCSHR]
                            ");

            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);


            dtDBTKHR.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            Exception ex = null; // 初始化
            DataTable dtResult = null;

            foreach (DataRow drDBTKHR in dtDBTKHR.Rows)
            {
                dtResult = null;
                accounts = drDBTKHR["EMPID"].ToString();

                SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(
                "ATT0021700",
                "GetAttendData_Web",
                SCSHR.Types.SCSParameter.getPatameters(new
                {
                    StartDate = ((DateTime)kdtpSTARTTIME.SelectedDate).ToString("yyyyMMdd"),
                    EndDate = ((DateTime)kdtpENDTIME.SelectedDate).ToString("yyyyMMdd"),
                    CalcCHours = ddlHours.SelectedValue,
                    ShowAbnormal = ddlAbnorma.SelectedValue,
                    EmployeeID = accounts
                }),
                out ex);

                if (ex != null)
                    JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_DUTY_REPORT.btnSearch_Click.BOExecFunc.ERROR:{0}", ex.Message));
                if (parameters != null &&
                    parameters.Length > 0)
                {
                    if (parameters[0].DataType.ToString() == "DataTable")
                    {
                        //DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.btnCal_Click.BOExecFunc.Result.XML:{0}", parameters[0].Xml));

                        dtResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);

                        if (addDataColumnflag.Equals("Y"))
                        {
                            foreach (DataColumn dc in dtResult.Columns)
                            {
                                dt.Columns.Add(new DataColumn(dc.ColumnName, typeof(String)));
                            }

                            addDataColumnflag = "N";
                        }

                        for (int i = 0; i < dtResult.Rows.Count; i++)
                        {
                            dr = dtResult.Rows[i];
                            row = dt.NewRow();
                            foreach (DataColumn dc in dtResult.Columns)
                            {
                                row[dc.ColumnName] = dr[dc.ColumnName];
                                if (dc.DataType == typeof(DateTime) && dc.ColumnName.Length >= 4)
                                {
                                    string title = string.Format("{0}TIME", dc.ColumnName.Substring(0, dc.ColumnName.Length - 4));
                                    string date = row[dc.ColumnName].ToString();
                                    if (dtResult.Columns.Contains(title))
                                    {
                                        string time = dr[title].ToString();
                                        row[dc.ColumnName] = string.Format("{0} {1}", date.Length > 10 ? date.Substring(0, 10) : date, time.Length >= 3 ? time.Insert(2, ":") : time);
                                    }
                                    else
                                    {
                                        row[dc.ColumnName] = date.Length > 10 ? date.Substring(0, 10) : date;
                                    }
                                }
                            }
                            dt.Rows.Add(row);
                        }
                    }
                }
            }




            if (dtResult != null)
            {
                DataView dv = new DataView(dt);
                dv.Sort = "EMPLOYEEVIEWID";
                ViewState["gvitems"] = dv.ToTable();
                gvItems.DataSource = dv;
                gvItems.DataBind();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), "SetGvItems()", true);
                return;
            }
            ViewState["gvitems"] = dtResult;
            gvItems.DataSource = dtResult;
            gvItems.DataBind();
        }
        catch 
        { 
        
        }
        finally
        {

        }
       

        //if (kdtpSTARTTIME.SelectedDate == null || kdtpENDTIME.SelectedDate == null || string.IsNullOrEmpty(lblAccount.Text))
        //{
        //    gvItems.DataSource = CreateGvitem();
        //    gvItems.DataBind();
        //    return;
        //}

        //string accounts = lblAccount.Text;

        //UserSet US = UC_ChoiceListMobile.UserSet;
        //if (US.GetAllUsers().Rows.Count > 0)
        //{
        //    accounts = "";
        //    foreach (DataRow dr in US.GetAllUsers().Rows)
        //    {
        //        EBUser user = new UserUCO().GetEBUser(dr["USER_GUID"].ToString());
        //        if (!string.IsNullOrEmpty(accounts)) accounts += ";";
        //        accounts += user.Account;
        //    }
        //}
        //Exception ex = null; // 初始化
        //DataTable dtResult = null;

        //SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(
        //"ATT0021700",
        //"GetAttendData_Web",
        //SCSHR.Types.SCSParameter.getPatameters(new
        //{
        //    StartDate ="20211201",
        //    EndDate = "20211231",
        //    CalcCHours = "0",
        //    ShowAbnormal ="0",
        //    EmployeeID = "190074"
        //}),
        //out ex);

        ////正確查詢
        //SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(
        //"ATT0021700",
        //"GetAttendData_Web",
        //SCSHR.Types.SCSParameter.getPatameters(new
        //{
        //    StartDate = ((DateTime)kdtpSTARTTIME.SelectedDate).ToString("yyyyMMdd"),
        //    EndDate = ((DateTime)kdtpENDTIME.SelectedDate).ToString("yyyyMMdd"),
        //    CalcCHours = ddlHours.SelectedValue,
        //    ShowAbnormal = ddlAbnorma.SelectedValue,
        //    EmployeeID = accounts
        //}),
        //out ex);


        //JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_DUTY_REPORT.btnSearch_Click.BOExecFunc::{0}::Send::{1}", "ATT0021700", ((DateTime)kdtpSTARTTIME.SelectedDate).ToString("yyyyMMdd") + " , " + ((DateTime)kdtpENDTIME.SelectedDate).ToString("yyyyMMdd") + " , " + ddlHours.SelectedValue + " , " + ddlAbnorma.SelectedValue + " , " + accounts));
        //JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_DUTY_REPORT.btnSearch_Click.BOExecFunc::{0}::Result::{1}", "ATT0021700", Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));

        //if (ex != null)
        //    JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_DUTY_REPORT.btnSearch_Click.BOExecFunc.ERROR:{0}", ex.Message));
        //if (parameters != null &&
        //    parameters.Length > 0)
        //{
        //    if (parameters[0].DataType.ToString() == "DataTable")
        //    {
        //        //DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.btnCal_Click.BOExecFunc.Result.XML:{0}", parameters[0].Xml));

        //        dtResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
        //    }
        //}

        //DataTable dt = new DataTable();
        //foreach (DataColumn dc in dtResult.Columns)
        //{
        //    dt.Columns.Add(new DataColumn(dc.ColumnName, typeof(String)));
        //}
        //for (int i = 0; i < dtResult.Rows.Count; i++)
        //{
        //    DataRow dr = dtResult.Rows[i];
        //    DataRow row = dt.NewRow();
        //    foreach (DataColumn dc in dtResult.Columns)
        //    {
        //        row[dc.ColumnName] = dr[dc.ColumnName];
        //        if (dc.DataType == typeof(DateTime) && dc.ColumnName.Length >= 4)
        //        {
        //            string title = string.Format("{0}TIME", dc.ColumnName.Substring(0, dc.ColumnName.Length - 4));
        //            string date = row[dc.ColumnName].ToString();
        //            if (dtResult.Columns.Contains(title))
        //            {
        //                string time = dr[title].ToString();
        //                row[dc.ColumnName] = string.Format("{0} {1}", date.Length > 10 ? date.Substring(0, 10) : date, time.Length >= 3 ? time.Insert(2, ":") : time);
        //            }
        //            else
        //            {
        //                row[dc.ColumnName] = date.Length > 10 ? date.Substring(0, 10) : date;
        //            }
        //        }
        //    }
        //    dt.Rows.Add(row);
        //}
        //if (dtResult != null)
        //{
        //    DataView dv = new DataView(dt);
        //    dv.Sort = "EMPLOYEEVIEWID";
        //    ViewState["gvitems"] = dv.ToTable();
        //    gvItems.DataSource = dv;
        //    gvItems.DataBind();
        //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), "SetGvItems()", true);
        //    return;
        //}
        //ViewState["gvitems"] = dtResult;
        //gvItems.DataSource = dtResult;
        //gvItems.DataBind();
    }

    /// <summary>
    /// 轉入DB做運算
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDB_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        DataTable dtDBTKHR = new DataTable();
        DataTable dt = new DataTable();
        string accounts = null;
        string addDataColumnflag = "Y";
        DataRow dr = null;
        DataRow row = null;
        StringBuilder cmdTxt = new StringBuilder();
        cmdTxt.AppendFormat(@"
                            SELECT  [EMPID] FROM [TKHR].[dbo].[SCSHR]
                            ");

        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);


        dtDBTKHR.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Exception ex = null; // 初始化
        DataTable dtResult = null;

        foreach (DataRow drDBTKHR in dtDBTKHR.Rows)
        {
            dtResult = null;
            accounts = drDBTKHR["EMPID"].ToString();

            SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(
            "ATT0021700",
            "GetAttendData_Web",
            SCSHR.Types.SCSParameter.getPatameters(new
            {
                StartDate = ((DateTime)kdtpSTARTTIME.SelectedDate).ToString("yyyyMMdd"),
                EndDate = ((DateTime)kdtpENDTIME.SelectedDate).ToString("yyyyMMdd"),
                CalcCHours = ddlHours.SelectedValue,
                ShowAbnormal = ddlAbnorma.SelectedValue,
                EmployeeID = accounts
            }),
            out ex);

            if (ex != null)
                JGlobalLibs.DebugLog.Log(string.Format(@"WB_KYTI_SCSHR_DUTY_REPORT.btnSearch_Click.BOExecFunc.ERROR:{0}", ex.Message));
            if (parameters != null &&
                parameters.Length > 0)
            {
                if (parameters[0].DataType.ToString() == "DataTable")
                {
                    //DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.btnCal_Click.BOExecFunc.Result.XML:{0}", parameters[0].Xml));

                    dtResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);

                    if (addDataColumnflag.Equals("Y"))
                    {
                        foreach (DataColumn dc in dtResult.Columns)
                        {
                            dt.Columns.Add(new DataColumn(dc.ColumnName, typeof(String)));
                        }

                        addDataColumnflag = "N";
                    }

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        dr = dtResult.Rows[i];
                        row = dt.NewRow();
                        foreach (DataColumn dc in dtResult.Columns)
                        {
                            row[dc.ColumnName] = dr[dc.ColumnName];
                            if (dc.DataType == typeof(DateTime) && dc.ColumnName.Length >= 4)
                            {
                                string title = string.Format("{0}TIME", dc.ColumnName.Substring(0, dc.ColumnName.Length - 4));
                                string date = row[dc.ColumnName].ToString();
                                if (dtResult.Columns.Contains(title))
                                {
                                    string time = dr[title].ToString();
                                    row[dc.ColumnName] = string.Format("{0} {1}", date.Length > 10 ? date.Substring(0, 10) : date, time.Length >= 3 ? time.Insert(2, ":") : time);
                                }
                                else
                                {
                                    row[dc.ColumnName] = date.Length > 10 ? date.Substring(0, 10) : date;
                                }
                            }
                        }
                        dt.Rows.Add(row);
                    }
                }
            }
        }


        DELETETKHRSCSHREMP(((DateTime)kdtpSTARTTIME.SelectedDate).ToString("yyyyMMdd"), ((DateTime)kdtpENDTIME.SelectedDate).ToString("yyyyMMdd"));

        ADDTKHRSCSHREMP(dt);

        //if (dtResult != null)
        //{
        //    DataView dv = new DataView(dt);
        //    dv.Sort = "EMPLOYEEVIEWID";
        //    ViewState["gvitems"] = dv.ToTable();
        //    gvItems.DataSource = dv;
        //    gvItems.DataBind();
        //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), "SetGvItems()", true);
        //    return;
        //}
        //ViewState["gvitems"] = dtResult;
        //gvItems.DataSource = dtResult;
        //gvItems.DataBind();
    }

    public void DELETETKHRSCSHREMP(string ATTENDDATESTART,string ATTENDDATEEND)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                        DELETE [TKHR].[dbo].[SCSHREMP]
                        WHERE CONVERT(NVARCHAR,ATTENDDATE,112)>=@ATTENDDATESTART AND CONVERT(NVARCHAR,ATTENDDATE,112)<=@ATTENDDATEEND
                         ";



        m_db.AddParameter("@ATTENDDATESTART", ATTENDDATESTART);
        m_db.AddParameter("@ATTENDDATEEND", ATTENDDATEEND);
    



        m_db.ExecuteNonQuery(cmdTxt);
    }

    public void ADDTKHRSCSHREMP(DataTable dt)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        StringBuilder cmdTxt = new StringBuilder();

        //cmdTxt = @"  
        //              INSERT INTO [TKHR].[dbo].[SCSHREMP]
        //              ([EMPLOYEEVIEWID],[ATTENDDATE])
        //              VALUES
        //              ('1','2021/12/22')
        //             ";

        //cmdTxt.AppendFormat(@"  
        //                            INSERT INTO [TKHR].[dbo].[SCSHREMP]
        //                            ([EMPLOYEEVIEWID],[ATTENDDATE])
        //                            VALUES
        //                            (@EMPLOYEEVIEWID,@ATTENDDATE)
        //                            ");
        //m_db.AddParameter("@EMPLOYEEVIEWID", dt.Rows[0]["EMPLOYEEVIEWID"].ToString());
        //m_db.AddParameter("@ATTENDDATE", dt.Rows[0]["ATTENDDATE"].ToString());

        foreach (DataRow dr in dt.Rows)
        {
            if (!string.IsNullOrEmpty(dr["ATTENDDATE"].ToString())&& !string.IsNullOrEmpty(dr["CARDNO"].ToString())  )
            {
                cmdTxt.AppendFormat(@"  
                                     INSERT INTO [TKHR].[dbo].[SCSHREMP]
                                    ([EMPLOYEEVIEWID],[ATTENDDATE],[EMPLOYEENAME],[DEPARTVIEWID],[DEPARTNAME],[CARDNO],[WORKTIME],[OFFWORKTIME],[CREATEDATES])
                                    VALUES
                                    ('{0}','{1}',N'{2}','{3}','{4}','{5}','{6}','{7}','{8}')
                                    ", dr["EMPLOYEEVIEWID"].ToString(), dr["ATTENDDATE"].ToString(), dr["EMPLOYEENAME"].ToString(), dr["DEPARTVIEWID"].ToString(), dr["DEPARTNAME"].ToString(), dr["CARDNO"].ToString(), dr["ATTENDDATE"].ToString()+" "+dr["WORKTIME"].ToString(), dr["ATTENDDATE"].ToString() + " " + dr["OFFWORKTIME"].ToString(), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));



                //m_db.AddParameter("@EMPLOYEEVIEWID", dr["EMPLOYEEVIEWID"].ToString());
                //m_db.AddParameter("@ATTENDDATE", dr["ATTENDDATE"].ToString());
                //m_db.AddParameter("@EMPLOYEENAME", dr["EMPLOYEENAME"].ToString());
                //m_db.AddParameter("@DEPARTVIEWID", dr["DEPARTVIEWID"].ToString());
                //m_db.AddParameter("@DEPARTNAME", dr["DEPARTNAME"].ToString());
                //m_db.AddParameter("@CARDNO", dr["CARDNO"].ToString());
                //m_db.AddParameter("@WORKTIME", dr["WORKTIME"].ToString());
                //m_db.AddParameter("@FFWORKTIME", dr["FFWORKTIME"].ToString());
            }
        }





        try
        {
            m_db.ExecuteNonQuery(cmdTxt.ToString());
            Label1.Text = "成功";
        }
        catch (Exception ex)
        {
            Label1.Text = "失敗"+ex.Message;
        }
        finally
        {

        }
        
    }
    private DataTable CreateGvitem()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("EMPLOYEEVIEWID", typeof(String))); // 員工工號
        dt.Columns.Add(new DataColumn("EMPLOYEENAME", typeof(String))); // 員工姓名
        dt.Columns.Add(new DataColumn("DEPARTVIEWID", typeof(String))); // 部門編號
        dt.Columns.Add(new DataColumn("DEPARTNAME", typeof(String))); // 部門名稱
        dt.Columns.Add(new DataColumn("ATTENDDATE", typeof(String))); // 出勤日期
        dt.Columns.Add(new DataColumn("TMP_WORKID", typeof(String))); // 出勤班別代碼
        dt.Columns.Add(new DataColumn("TMP_WORKNAME", typeof(String))); // 出勤班別
        dt.Columns.Add(new DataColumn("CARDNO", typeof(String))); // 刷卡卡號
        dt.Columns.Add(new DataColumn("BOVERTIME", typeof(String))); // 出勤時間(A1)
        dt.Columns.Add(new DataColumn("BOVERTIMESTATUS", typeof(String))); // 出勤狀態(A1)
        dt.Columns.Add(new DataColumn("BOFFOVERTIME", typeof(String))); // 出勤時間(A2)
        dt.Columns.Add(new DataColumn("BOFFOVERTIMESTATUS", typeof(String))); // 出勤狀態(A2)
        dt.Columns.Add(new DataColumn("BSTATUS", typeof(String))); // 加班狀態(A)
        dt.Columns.Add(new DataColumn("WORKTIME", typeof(String))); // 出勤時間(B1) 
        dt.Columns.Add(new DataColumn("WORKTIMESTATUS", typeof(String))); // 出勤狀態(B1) 
        dt.Columns.Add(new DataColumn("STATUS", typeof(String))); // 處理狀態(B1) 
        dt.Columns.Add(new DataColumn("OFFWORKTIME", typeof(String))); // 出勤時間(B2) 
        dt.Columns.Add(new DataColumn("OFFWORKTIMESTATUS", typeof(String))); // 出勤狀態(B2) 
        dt.Columns.Add(new DataColumn("STATUS2", typeof(String))); // 處理狀態(B2) 
        dt.Columns.Add(new DataColumn("AOVERTIME", typeof(String))); // 出勤時間(C1) 
        dt.Columns.Add(new DataColumn("AOVERTIMESTATUS", typeof(String))); // 出勤狀態(C1) 
        dt.Columns.Add(new DataColumn("AOFFOVERTIME", typeof(String))); // 出勤時間(C2) 
        dt.Columns.Add(new DataColumn("AOFFOVERTIMESTATUS", typeof(String))); // 出勤狀態(C2) 
        dt.Columns.Add(new DataColumn("ASTATUS", typeof(String))); // 加班狀態(C) 
        dt.Columns.Add(new DataColumn("CHOURS", typeof(String))); // 缺勤時數 
        dt.Columns.Add(new DataColumn("LHOURS", typeof(String))); // 請假時數 
        dt.Columns.Add(new DataColumn("FHOURS", typeof(String))); // 簽核中的請假時數 
        ViewState["gvitems"] = dt;
        return dt;
    }

    /// <summary>
    /// 按下載按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDLExcel_Click(object sender, EventArgs e)
    {
        if (ViewState["gvitems"] == null)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), @"
                alert('無資料可供匯出');
            ", true);
            return;
        }
        DataTable objtable = ViewState["gvitems"] as DataTable;
        if (objtable.Rows.Count == 0)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), @"
                alert('無資料可供匯出');
            ", true);
            return;
        }
        string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString() + ".xlsx";
        string folder = Server.MapPath("~/CDS/SCSHR/Temp/");
        if (!Directory.Exists(folder))
        {
            try
            {
                Directory.CreateDirectory(folder);
            }
            catch { }
        }
        string filePath = folder + filename;
        using (MemoryStream stream = new MemoryStream())
        {
            WebUtils.GridViewExportToExcel(stream, "個人出缺勤狀況查詢", gvItems, "微軟正黑體", "A1");
            stream.Seek(0, SeekOrigin.Begin);
            File.WriteAllBytes(filePath, stream.ToArray());
        }

        ScriptManager.RegisterClientScriptBlock(
           UpdatePanel1,
           UpdatePanel1.GetType(),
           Guid.NewGuid().ToString(),
           string.Format(@"
                    document.addEventListener('DOMContentLoaded', function() {{
                        window.location = '{0}?filepath={1}';
                    }});
                "
           , Page.ResolveUrl("~/CDS/SCSHR/WKFFields/FORMPRINT/DownFileWithPath.ashx")
           , HttpUtility.UrlEncode(filePath)),
           true);
    }

    protected void _UC_SearchGroupWithGroup_DialogReturn(object sender, string result)
    {
        DataTable dtResult = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(result);
        GridViewRow gr = ((Button)sender).Parent.Parent.Parent as GridViewRow;
        if (dtResult == null) return;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            sb.Append(dtResult.Rows[i]["ACCOUNT"]);
            if (i < dtResult.Rows.Count - 1) sb.Append(';');
        }
        this.lblAccount.Text = sb.ToString();
    }


    #region 新增
    /// <summary>
    /// BindGrid
    /// </summary>
    /// <param name="SALESFOCUS"></param>
    private void BindGrid()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();


        cmdTxt.AppendFormat(@"
                              SELECT [EMPLOYEEVIEWID],[EMPLOYEENAME],[DEPARTVIEWID],[DEPARTNAME],SUM(WORKHRS) AS SUMWORKHRS
                            ,(SELECT CONVERT(INT,ISNULL(SUM([LEAHOURS]),0)) FROM  [192.168.1.223].[UOF].[dbo].[Z_SCSHR_LEAVE] WHERE ISNULL([LEACODENAME],'')<>'' AND [LEACODENAME] NOT LIKE '%外出%' AND [LEACODENAME] NOT LIKE '%出差%'AND [LEACODENAME] NOT LIKE '%訓練%' AND [TASK_RESULT]='0' AND [APPLICANT]=[EMPLOYEEVIEWID] COLLATE Chinese_Taiwan_Stroke_BIN AND CONVERT(NVARCHAR,[STARTTIME],112)>=@ATTENDDATESTART AND CONVERT(NVARCHAR,[ENDTIME],112)<=@ATTENDDATEEND ) [LEAHOURS]
                            ,(SELECT CONVERT(INT,ISNULL(SUM([OT_TIMES]),0)) FROM  [192.168.1.223].[UOF].[dbo].[Z_SCSHR_OVERTIME] WHERE [TASK_RESULT]='0'  AND [APPLICANT]=[EMPLOYEEVIEWID] COLLATE Chinese_Taiwan_Stroke_BIN AND CONVERT(NVARCHAR,[OT_START],112)>=@ATTENDDATESTART AND CONVERT(NVARCHAR,[OT_END],112)<=@ATTENDDATEEND ) AS [OT_TIMES]
                            FROM (
                            SELECT 
                            [EMPLOYEEVIEWID]
                            ,[ATTENDDATE]
                            ,[EMPLOYEENAME]
                            ,[DEPARTVIEWID]
                            ,[DEPARTNAME]
                            ,[CARDNO]
                            ,[WORKTIME]
                            ,[OFFWORKTIME]
                            ,[CREATEDATES]
                            ,DATEDIFF(hour,[WORKTIME],[OFFWORKTIME])  AS WORKHRS
                            FROM [TKHR].[dbo].[SCSHREMP]
                            WHERE DATEDIFF(hour,[WORKTIME],[OFFWORKTIME])>0
                            AND CONVERT(NVARCHAR,[ATTENDDATE],112)>=@ATTENDDATESTART AND  CONVERT(NVARCHAR,[ATTENDDATE],112)<=@ATTENDDATEEND 
                            ) AS TEMP
                            GROUP BY [EMPLOYEEVIEWID],[EMPLOYEENAME],[DEPARTVIEWID],[DEPARTNAME]
                           ORDER BY [DEPARTVIEWID],[EMPLOYEEVIEWID]
                             ");



        m_db.AddParameter("@ATTENDDATESTART", ((DateTime)RadDatePicker1.SelectedDate).ToString("yyyyMMdd"));
        m_db.AddParameter("@ATTENDDATEEND", ((DateTime)RadDatePicker2.SelectedDate).ToString("yyyyMMdd"));


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }
    /// <summary>
    /// grid1_PageIndexChanging
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    /// <summary>
    /// Grid1_RowDataBound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {



    }
    /// <summary>
    /// OnBeforeExport1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);


        //string cmdTxt = @" 
        //               SELECT [PRODUCTS].[MB001],[PRODUCTSFEATURES],[SALESFOCUS],[COPYWRITINGS],[PICPATHS]
        //                ,MB002,MB003,MB004,MA003,ISNULL(MD007,0) AS MD007,CONVERT(NVARCHAR,MB023)+(CASE WHEN MB198='1' THEN '天' ELSE (CASE WHEN MB198='2' THEN '月' ELSE '年' END ) END ) AS 'VALIDITYPERIOD',CONVERT(decimal(16,3),ISNULL(MB047,0)) AS MB047,MB013
        //                ,[ALBUM_GUID], [PHOTO_GUID],[PHOTO_DESC],[FILE_ID],[RESIZE_FILE_ID],[THUMBNAIL_FILE_ID]
        //                FROM [TKBUSINESS].[dbo].[PRODUCTS]
        //                LEFT JOIN [TK].dbo.[INVMB] ON [PRODUCTS].[MB001]=[INVMB].[MB001]
        //                LEFT JOIN [TK].dbo.INVMA ON MA001='9' AND MA002=MB115
        //                LEFT JOIN [TK].dbo.BOMMD ON MD001=[INVMB].[MB001] AND MD003 LIKE '201%'
        //                LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EIP_ALBUM_PHOTO] ON [PHOTO_TOPIC]=[PRODUCTS].[MB001] COLLATE Chinese_Taiwan_Stroke_BIN
        //                ORDER BY [PRODUCTS].[MB001]
        //                ";



        //DataTable dt = new DataTable();

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    dt.Columns[0].Caption = "ID";


        //    e.Datasource = dt;
        //}
    }


    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    public void SETEXCEL()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        cmdTxt.AppendFormat(@"
                              SELECT [EMPLOYEEVIEWID],[EMPLOYEENAME],[DEPARTVIEWID],[DEPARTNAME],SUM(WORKHRS) AS SUMWORKHRS
                            ,(SELECT CONVERT(INT,ISNULL(SUM([LEAHOURS]),0)) FROM  [192.168.1.223].[UOF].[dbo].[Z_SCSHR_LEAVE] WHERE ISNULL([LEACODENAME],'')<>'' AND [LEACODENAME] NOT LIKE '%外出%' AND [LEACODENAME] NOT LIKE '%出差%'AND [LEACODENAME] NOT LIKE '%訓練%' AND [TASK_RESULT]='0' AND [APPLICANT]=[EMPLOYEEVIEWID] COLLATE Chinese_Taiwan_Stroke_BIN AND CONVERT(NVARCHAR,[STARTTIME],112)>=@ATTENDDATESTART AND CONVERT(NVARCHAR,[ENDTIME],112)<=@ATTENDDATEEND ) [LEAHOURS]
                            ,(SELECT CONVERT(INT,ISNULL(SUM([OT_TIMES]),0)) FROM  [192.168.1.223].[UOF].[dbo].[Z_SCSHR_OVERTIME] WHERE [TASK_RESULT]='0'  AND [APPLICANT]=[EMPLOYEEVIEWID] COLLATE Chinese_Taiwan_Stroke_BIN AND CONVERT(NVARCHAR,[OT_START],112)>=@ATTENDDATESTART AND CONVERT(NVARCHAR,[OT_END],112)<=@ATTENDDATEEND ) AS [OT_TIMES]
                            FROM (
                            SELECT 
                            [EMPLOYEEVIEWID]
                            ,[ATTENDDATE]
                            ,[EMPLOYEENAME]
                            ,[DEPARTVIEWID]
                            ,[DEPARTNAME]
                            ,[CARDNO]
                            ,[WORKTIME]
                            ,[OFFWORKTIME]
                            ,[CREATEDATES]
                            ,DATEDIFF(hour,[WORKTIME],[OFFWORKTIME])  AS WORKHRS
                            FROM [TKHR].[dbo].[SCSHREMP]
                            WHERE DATEDIFF(hour,[WORKTIME],[OFFWORKTIME])>0
                            AND CONVERT(NVARCHAR,[ATTENDDATE],112)>=@ATTENDDATESTART AND  CONVERT(NVARCHAR,[ATTENDDATE],112)<=@ATTENDDATEEND 
                            ) AS TEMP
                            GROUP BY [EMPLOYEEVIEWID],[EMPLOYEENAME],[DEPARTVIEWID],[DEPARTNAME]
                            ORDER BY [DEPARTVIEWID],[EMPLOYEEVIEWID]
                             ");



        m_db.AddParameter("@ATTENDDATESTART", ((DateTime)RadDatePicker1.SelectedDate).ToString("yyyyMMdd"));
        m_db.AddParameter("@ATTENDDATEEND", ((DateTime)RadDatePicker2.SelectedDate).ToString("yyyyMMdd"));


        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            //檔案名稱
            var fileName = "工時" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知

            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {

                // 建立分頁
                var ws = excel.Workbook.Worksheets.Add("list" + DateTime.Now.ToShortDateString());


                //預設行高
                ws.DefaultRowHeight = 20;

                // 寫入資料試試
                //ws.Cells[2, 1].Value = "測試測試";
                int ROWS = 2;
                int COLUMNS = 1;


                //excel標題
                ws.Cells[1, 1].Value = "日期起";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 2].Value = "日期迄";
                ws.Cells[1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 3].Value = "工號";
                ws.Cells[1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 4].Value = "姓名";
                ws.Cells[1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 5].Value = "部門";
                ws.Cells[1, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 6].Value = "總工時";
                ws.Cells[1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 7].Value = "請假總時數";
                ws.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                ws.Cells[1, 8].Value = "加班總時數";
                ws.Cells[1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                foreach (DataRow od in dt.Rows)
                {
                    ws.Cells[ROWS, 1].Value = ((DateTime)RadDatePicker1.SelectedDate).ToString("yyyyMMdd");
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 2].Value = ((DateTime)RadDatePicker2.SelectedDate).ToString("yyyyMMdd");
                    ws.Cells[ROWS, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線

                    ws.Cells[ROWS, 3].Value = od["EMPLOYEEVIEWID"].ToString();
                    ws.Cells[ROWS, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 4].Value = od["EMPLOYEENAME"].ToString();
                    ws.Cells[ROWS, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 5].Value = od["DEPARTNAME"].ToString();
                    ws.Cells[ROWS, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 6].Value = od["SUMWORKHRS"].ToString();
                    ws.Cells[ROWS, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 7].Value = od["LEAHOURS"].ToString();
                    ws.Cells[ROWS, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                    ws.Cells[ROWS, 8].Value = od["OT_TIMES"].ToString();
                    ws.Cells[ROWS, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線
                  


                    ROWS++;
                }




                ////預設列寬、行高
                //sheet.DefaultColWidth = 10; //預設列寬
                //sheet.DefaultRowHeight = 30; //預設行高

                //// 遇\n或(char)10自動斷行
                //ws.Cells.Style.WrapText = true;

                //自適應寬度設定
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //自適應高度設定
                ws.Row(1).CustomHeight = true;



                //儲存Excel
                //Byte[] bin = excel.GetAsByteArray();
                //File.WriteAllBytes(@"C:\TEMP\" + fileName, bin);

                //儲存和歸來的Excel檔案作為一個ByteArray
                var data = excel.GetAsByteArray();
                HttpResponse response = HttpContext.Current.Response;
                Response.Clear();

                //輸出標頭檔案　　
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName + "");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(data);
                Response.Flush();
                Response.End();
                //package.Save();//這個方法是直接下載到本地
            }
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知
            //                                                            // 沒設置的話會跳出 Please set the excelpackage.licensecontext property


            ////var file = new FileInfo(fileName);
            //using (var excel = new ExcelPackage(file))
            //{

            //}
        }

    }

    private void BindGrid2()
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        //StringBuilder cmdTxt = new StringBuilder();


        //cmdTxt.AppendFormat(@"
        //                      SELECT [EMPLOYEEVIEWID],[EMPLOYEENAME],[DEPARTVIEWID],[DEPARTNAME],SUM(WORKHRS) AS SUMWORKHRS
        //                    ,(SELECT CONVERT(INT,ISNULL(SUM([LEAHOURS]),0)) FROM  [192.168.1.223].[UOF].[dbo].[Z_SCSHR_LEAVE] WHERE ISNULL([LEACODENAME],'')<>'' AND [LEACODENAME] NOT LIKE '%外出%' AND [LEACODENAME] NOT LIKE '%出差%'AND [LEACODENAME] NOT LIKE '%訓練%' AND [TASK_RESULT]='0' AND [APPLICANT]=[EMPLOYEEVIEWID] COLLATE Chinese_Taiwan_Stroke_BIN AND CONVERT(NVARCHAR,[STARTTIME],112)>=@ATTENDDATESTART AND CONVERT(NVARCHAR,[ENDTIME],112)<=@ATTENDDATEEND ) [LEAHOURS]
        //                    ,(SELECT CONVERT(INT,ISNULL(SUM([OT_TIMES]),0)) FROM  [192.168.1.223].[UOF].[dbo].[Z_SCSHR_OVERTIME] WHERE [TASK_RESULT]='0'  AND [APPLICANT]=[EMPLOYEEVIEWID] COLLATE Chinese_Taiwan_Stroke_BIN AND CONVERT(NVARCHAR,[OT_START],112)>=@ATTENDDATESTART AND CONVERT(NVARCHAR,[OT_END],112)<=@ATTENDDATEEND ) AS [OT_TIMES]
        //                    FROM (
        //                    SELECT 
        //                    [EMPLOYEEVIEWID]
        //                    ,[ATTENDDATE]
        //                    ,[EMPLOYEENAME]
        //                    ,[DEPARTVIEWID]
        //                    ,[DEPARTNAME]
        //                    ,[CARDNO]
        //                    ,[WORKTIME]
        //                    ,[OFFWORKTIME]
        //                    ,[CREATEDATES]
        //                    ,DATEDIFF(hour,[WORKTIME],[OFFWORKTIME])  AS WORKHRS
        //                    FROM [TKHR].[dbo].[SCSHREMP]
        //                    WHERE DATEDIFF(hour,[WORKTIME],[OFFWORKTIME])>0
        //                    AND CONVERT(NVARCHAR,[ATTENDDATE],112)>=@ATTENDDATESTART AND  CONVERT(NVARCHAR,[ATTENDDATE],112)<=@ATTENDDATEEND 
        //                    ) AS TEMP
        //                    GROUP BY [EMPLOYEEVIEWID],[EMPLOYEENAME],[DEPARTVIEWID],[DEPARTNAME]
        //                   ORDER BY [DEPARTVIEWID],[EMPLOYEEVIEWID]
        //                     ");



        //m_db.AddParameter("@ATTENDDATESTART", ((DateTime)RadDatePicker1.SelectedDate).ToString("yyyyMMdd"));
        //m_db.AddParameter("@ATTENDDATEEND", ((DateTime)RadDatePicker2.SelectedDate).ToString("yyyyMMdd"));

               
        //DataTable dtResult = new DataTable();

        //dtResult.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        //if (dtResult != null)
        //{
        //    DataView dv = new DataView(dtResult);
        //    dv.Sort = "EMPLOYEEVIEWID";
        //    ViewState["GridView1"] = dv.ToTable();
        //    GridView1.DataSource = dv;
        //    GridView1.DataBind();
        //    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), "SetGvItems()", true);
        //    return;
        //}
        //ViewState["GridView1"] = dtResult;
        //GridView1.DataSource = dtResult;
        //GridView1.DataBind();
    }

    /// <summary>
    /// 查詢報表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn1_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid();
            //BindGrid2();

        }
        catch
        {

        }
        finally
        {

        }
    }

    #endregion
}