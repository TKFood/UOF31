using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using SCSHR.Utils;
using System.Web.UI.WebControls;
using KYTLog;
using System.Xml;
using System.Web;
using JGlobalLibs.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using Ede.Uof.EIP.Organization.Util;
using System.Threading;


/**
* 修改時間：2021/06/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.所有有使用EBUser的地方，都改為呼叫通用方法取得人員資訊
* 修改原因：
    * 1.修改規格，UOF的EBUser有時候會異常取不到人員資訊，以防再多花時間去查明原因，改為通用方法直接查SQL方式取得人員資訊
* 修改位置： 
    * 1.「bcoMan_EditButtonOnClick()」中，註解所有EBUser，改為KYT_EBUser
* **/

public partial class CDS_SCSHR_WebPages_WEB_KYTI_SCSHR_TRAVEL_ERR_FIX : BasePage
{
    private string ConnectionString;
    private string Field_ID;
    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
        Field_ID = "KYTI_SCSHR_TRAVEL";
        if (!Page.IsPostBack)
        {
            ConstructorCommonSettings.setCommonSettings();
            AddSiteMapNode("出差單異常處理", Request.Url.AbsoluteUri);
        }
    }

    /// <summary>
    /// 過濾屬於表單的單號
    /// </summary>
    /// <param name="dtSource"></param>
    /// <param name="fieldID"></param>
    /// <returns></returns>
    private DataTable filterBelongToForm(DataTable dtSource, string fieldID)
    {
        DataTable dtReturn = new DataTable();
        foreach (DataRow dr in dtSource.Rows)
        {
            string DOC_NBR = dr["DOC_NBR"].ToString();
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SELECT TOP 1 * 
                  FROM TB_WKF_TASK 
                 WHERE DOC_NBR = @DOC_NBR 
            ", new DatabaseHelper().Command.Connection.ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
                try
                {
                    if (sda.Fill(ds) > 0)
                    {
                        DataRow _dr = ds.Tables[0].Rows[0];
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(_dr["CURRENT_DOC"].ToString());
                        XmlNode node = doc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='" + fieldID + "']"); // 取出外掛欄位資料
                        if (node != null)
                        {
                            DataRow ndr = dtReturn.NewRow();
                            foreach (DataColumn dc in dtSource.Columns)
                            {
                                if (!dtReturn.Columns.Contains(dc.ColumnName))
                                    dtReturn.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                                ndr[dc.ColumnName] = dr[dc.ColumnName];
                            }
                            dtReturn.Rows.Add(ndr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"WEB_KYTI_SCSHR_TRAVEL_ERR_FIX.filterBelongToForm.TB_WKF_TASK.SELECT.ERROR:{0}", ex.Message));
                }
            }
        }
        return dtReturn;
    }

    protected void gvTravel_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            KYTJsonDict dict = null;
            DataRowView row = (DataRowView)e.Row.DataItem;
            Label lblZTABLE_STATUS = gr.FindControl("lblZTABLE_STATUS") as Label;
            Label lblWS_STATUS = gr.FindControl("lblWS_STATUS") as Label;
            Label lblDOC_NBR = gr.FindControl("lblDOC_NBR") as Label;
            Label lblAPPLICANT_NAME = gr.FindControl("lblAPPLICANT_NAME") as Label;
            Label lblTRAVEL_NAME = gr.FindControl("lblTRAVEL_NAME") as Label;
            Label lblAGENT_NAME = gr.FindControl("lblAGENT_NAME") as Label;
            Label lblTRAVEL_POINT_NAME = gr.FindControl("lblTRAVEL_POINT_NAME") as Label;
            Label lblTRAVELCURR_NAME = gr.FindControl("lblTRAVELCURR_NAME") as Label;
            Label lblTRAVELFD = gr.FindControl("lblTRAVELFD") as Label;
            Label lblSTARTTIME = gr.FindControl("lblSTARTTIME") as Label;
            Label lblENDTIME = gr.FindControl("lblENDTIME") as Label;

            bool isRightField = false;
            string DOC_NBR = row["DOC_NBR"].ToString();
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SELECT TOP 1 * 
                  FROM TB_WKF_TASK 
                 WHERE DOC_NBR = @DOC_NBR 
            ", new DatabaseHelper().Command.Connection.ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
                try
                {
                    if (sda.Fill(ds) > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(dr["CURRENT_DOC"].ToString());
                        XmlNode node = doc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='" + Field_ID + "']"); // 取出外掛欄位資料
                        isRightField = node != null;
                        if (isRightField)
                            dict = JsonConvert.DeserializeObject<KYTJsonDict>(HttpUtility.HtmlDecode(node.InnerText));
                    }
                }
                catch (Exception ex)
                {
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"WEB_KYTI_SCSHR_TRAVEL_ERR_FIX.gvTravel_RowDataBound.TB_WKF_TASK.SELECT.ERROR:{0}", ex.Message));
                }
            }
            if (isRightField)
            {
                string ZTABLE_STATUS = ReBuildSCSHRForm.CheckZTableHasData(DOC_NBR, Field_ID);
                string WS_STATUS = ReBuildSCSHRForm.CheckWSHasData(DOC_NBR, Field_ID);

                lblZTABLE_STATUS.Text = !string.IsNullOrEmpty(ZTABLE_STATUS) ? ZTABLE_STATUS == "0" ? "O" : "X" : "?";
                lblZTABLE_STATUS.ForeColor = !string.IsNullOrEmpty(ZTABLE_STATUS) ? ZTABLE_STATUS == "0" ? Color.Green : Color.Red : Color.Yellow;

                lblWS_STATUS.Text = !string.IsNullOrEmpty(WS_STATUS) ? WS_STATUS == "0" ? "O" : "X" : "?";
                lblWS_STATUS.ForeColor = !string.IsNullOrEmpty(WS_STATUS) ? WS_STATUS == "0" ? Color.Green : Color.Red : Color.Yellow;
                lblAPPLICANT_NAME.Text = (string)dict.GetString("ktxtAPPLICANT");
                lblTRAVEL_NAME.Text = (string)dict.GetString("ktxtTRAVEL_MEN");
                lblAGENT_NAME.Text = (string)dict.GetString("ktxtTRAVEL_AGENT");
                lblTRAVEL_POINT_NAME.Text = ReBuildSCSHRForm.FindDropDownListText(dict.GetDataTable("kddlTRAVEL_POINT"), dict.GetString("kddlTRAVEL_POINT").ToString());
                lblTRAVELCURR_NAME.Text = ReBuildSCSHRForm.FindDropDownListText(dict.GetDataTable("kddlTRAVELCURR"), dict.GetString("kddlTRAVELCURR").ToString());
                lblTRAVELFD.Text = (string)dict.GetString("krblDOMESTIC");
                lblSTARTTIME.Text = (string)dict.GetString("kdtpSTARTTIME");
                lblENDTIME.Text = (string)dict.GetString("kdtpENDTIME");
            }
        }
    }

    protected void bcoMan_EditButtonOnClick(string[] choiceResult)
    {
        if (choiceResult.Length >= 1)
        {
            UserUCO userUCO = new UserUCO();
            //EBUser eBUser = userUCO.GetEBUser(choiceResult[0]);
            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(choiceResult[0]); // 人員
            txtAPPLICANT.Text = KUser.Name;
            hidAPPLICANTGUID.Value = KUser.UserGUID;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDOC_NBR.Text.Trim()))
        {
            string sqlCommand = string.Format(@"
                SELECT DOC_NBR,
             CASE WHEN TASK_STATUS = 1  -- 簽核狀態
			       AND TASK_RESULT IS NULL 
			      THEN '簽核中'
			      WHEN TASK_STATUS = 2 
			       AND TASK_RESULT = 0 
			      THEN '核准'
			      WHEN TASK_STATUS = 2 
			       AND TASK_RESULT = 1 
			      THEN '否決'
			      WHEN TASK_STATUS = 2 
			       AND TASK_RESULT = 2 
		          THEN '作廢'
			      WHEN TASK_STATUS = 3 
			      THEN '異常'
			      WHEN TASK_STATUS = 4 
			      THEN '退回'
			      ELSE '錯誤' 
			       END AS 'STATUS'
                  FROM TB_WKF_TASK 
                 WHERE DOC_NBR LIKE '{0}%' 
                   AND ((TASK_STATUS = 1 AND TASK_RESULT IS NULL) -- 簽核中
                    OR (TASK_STATUS = 2 AND TASK_RESULT = 0) -- 同意
                    OR (TASK_STATUS = 2 AND TASK_RESULT = 1) -- 否決
                    OR (TASK_STATUS = 2 AND TASK_RESULT = 2)) -- 作廢
                ", txtDOC_NBR.Text.Trim());
            if (dtpStart.SelectedDate != null &&
                dtpEnd.SelectedDate != null)
            {
                sqlCommand += " AND BEGIN_TIME BETWEEN @START AND @END";
            }
            if (!string.IsNullOrEmpty(hidAPPLICANTGUID.Value))
            {
                sqlCommand += " AND USER_GUID = @USER_GUID";
            }
            sqlCommand += @" ORDER BY DOC_NBR ASC";
            using (SqlDataAdapter sda = new SqlDataAdapter(sqlCommand, ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", hidAPPLICANTGUID.Value);
                sda.SelectCommand.Parameters.AddWithValue("@START", dtpStart.SelectedDate != null ? ((DateTime)dtpStart.SelectedDate).ToString("yyyy/MM/dd HH:mm:ss") : Convert.DBNull);
                sda.SelectCommand.Parameters.AddWithValue("@END", dtpEnd.SelectedDate != null ? ((DateTime)dtpEnd.SelectedDate).ToString("yyyy/MM/dd HH:mm:ss") : Convert.DBNull);
                try
                {
                    sda.Fill(ds);
                    ViewState["gvTravel"] = filterBelongToForm(ds.Tables[0], Field_ID);
                    gvTravel.DataSource = (DataTable)ViewState["gvTravel"];
                    gvTravel.DataBind();
                }
                catch (Exception ex)
                {
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"WEB_KYTI_SCSHR_TRAVEL_ERR_FIX.btnSearch_Click.TB_WKF_TASK.ERROR:{0}", ex.Message));
                }
            }
        }

    }

    protected void btnReBuild_Click(object sender, EventArgs e)
    {
        if (gvTravel.Rows.Count > 0)
        {
            foreach (GridViewRow gr in gvTravel.Rows)
            {
                Label lblZTABLE_STATUS = gr.FindControl("lblZTABLE_STATUS") as Label;
                Label lblWS_STATUS = gr.FindControl("lblWS_STATUS") as Label;
                Label lblDOC_NBR = gr.FindControl("lblDOC_NBR") as Label;
                Label lblAPPLICANT_NAME = gr.FindControl("lblAPPLICANT_NAME") as Label;
                Label lblTRAVEL_NAME = gr.FindControl("lblTRAVEL_NAME") as Label;
                Label lblAGENT_NAME = gr.FindControl("lblAGENT_NAME") as Label;
                Label lblTRAVEL_POINT_NAME = gr.FindControl("lblTRAVEL_POINT_NAME") as Label;
                Label lblTRAVELCURR_NAME = gr.FindControl("lblTRAVELCURR_NAME") as Label;
                Label lblTRAVELFD = gr.FindControl("lblTRAVELFD") as Label;
                Label lblSTARTTIME = gr.FindControl("lblSTARTTIME") as Label;
                Label lblENDTIME = gr.FindControl("lblENDTIME") as Label;
                Label lblMSG = gr.FindControl("lblMSG") as Label;
                if (!string.IsNullOrEmpty(lblDOC_NBR.Text))
                {
                    // 重新呼叫
                    //string fixDetail = ReBuildSCSHRForm.FixTravelDetail(lblDOC_NBR.Text, Field_ID);
                    //lblMSG.Text = fixDetail != "0" ? fixDetail + "<br />" : "";
                    string fixTravel = ReBuildSCSHRForm.FixTravelTravelIsEmpty(lblDOC_NBR.Text, Field_ID);
                    lblMSG.Text = fixTravel != "0" ? fixTravel + "<br />" : "";
                    if (fixTravel == "0" &&
                        string.IsNullOrEmpty(lblTRAVEL_NAME.Text))
                        lblTRAVEL_NAME.Text = lblAPPLICANT_NAME.Text;
                    if (string.IsNullOrEmpty(lblMSG.Text.Trim()))
                    {
                        string ZTABLE_STATUS = ReBuildSCSHRForm.ReUpdateZTable(lblDOC_NBR.Text, Field_ID);
                        string WS_STATUS = ReBuildSCSHRForm.ReCallWSUpdateStatus(lblDOC_NBR.Text, Field_ID);

                        lblZTABLE_STATUS.Text = !string.IsNullOrEmpty(ZTABLE_STATUS) ? ZTABLE_STATUS == "0" ? "O" : "X" : "?";
                        lblZTABLE_STATUS.ForeColor = !string.IsNullOrEmpty(ZTABLE_STATUS) ? ZTABLE_STATUS == "0" ? Color.Green : Color.Red : Color.Yellow;
                        lblMSG.Text = lblZTABLE_STATUS.Text != "O" ? ZTABLE_STATUS + "<br />" : "";

                        lblWS_STATUS.Text = !string.IsNullOrEmpty(WS_STATUS) ? WS_STATUS == "0" ? "O" : "X" : "?";
                        lblWS_STATUS.ForeColor = !string.IsNullOrEmpty(WS_STATUS) ? WS_STATUS == "0" ? Color.Green : Color.Red : Color.Yellow;
                        lblMSG.Text += lblWS_STATUS.Text != "O" ? WS_STATUS + "<br />" : "";


                        Thread.Sleep(3000);
                    }
                }
            }
        }
    }


    protected void btnPartReBuild_Click(object sender, EventArgs e)
    {
        if (gvTravel.Rows.Count > 0)
        {
            foreach (GridViewRow gr in gvTravel.Rows)
            {
                Label lblZTABLE_STATUS = gr.FindControl("lblZTABLE_STATUS") as Label;
                Label lblWS_STATUS = gr.FindControl("lblWS_STATUS") as Label;
                Label lblDOC_NBR = gr.FindControl("lblDOC_NBR") as Label;
                Label lblAPPLICANT_NAME = gr.FindControl("lblAPPLICANT_NAME") as Label;
                Label lblTRAVEL_NAME = gr.FindControl("lblTRAVEL_NAME") as Label;
                Label lblAGENT_NAME = gr.FindControl("lblAGENT_NAME") as Label;
                Label lblTRAVEL_POINT_NAME = gr.FindControl("lblTRAVEL_POINT_NAME") as Label;
                Label lblTRAVELCURR_NAME = gr.FindControl("lblTRAVELCURR_NAME") as Label;
                Label lblTRAVELFD = gr.FindControl("lblTRAVELFD") as Label;
                Label lblSTARTTIME = gr.FindControl("lblSTARTTIME") as Label;
                Label lblENDTIME = gr.FindControl("lblENDTIME") as Label;
                Label lblMSG = gr.FindControl("lblMSG") as Label;
                if (!string.IsNullOrEmpty(lblDOC_NBR.Text))
                {
                    // 重新呼叫
                    //string fixDetail = ReBuildSCSHRForm.FixTravelDetail(lblDOC_NBR.Text, Field_ID);
                    //lblMSG.Text = fixDetail != "0" ? fixDetail + "<br />" : "";

                    string fixTravel = ReBuildSCSHRForm.FixTravelTravelIsEmpty(lblDOC_NBR.Text, Field_ID);
                    lblMSG.Text = fixTravel != "0" ? fixTravel + "<br />" : "";
                    if (fixTravel == "0" &&
                        string.IsNullOrEmpty(lblTRAVEL_NAME.Text))
                        lblTRAVEL_NAME.Text = lblAPPLICANT_NAME.Text;
                    if (string.IsNullOrEmpty(lblMSG.Text.Trim()))
                    {
                        if (lblZTABLE_STATUS.Text != "O")
                        {
                            string ZTABLE_STATUS = ReBuildSCSHRForm.ReUpdateZTable(lblDOC_NBR.Text, Field_ID);
                            lblZTABLE_STATUS.Text = !string.IsNullOrEmpty(ZTABLE_STATUS) ? ZTABLE_STATUS == "0" ? "O" : "X" : "?";
                            lblZTABLE_STATUS.ForeColor = !string.IsNullOrEmpty(ZTABLE_STATUS) ? ZTABLE_STATUS == "0" ? Color.Green : Color.Red : Color.Yellow;
                            lblMSG.Text = lblZTABLE_STATUS.Text != "O" ? ZTABLE_STATUS + "<br />" : "";
                        }
                        if (lblWS_STATUS.Text != "O")
                        {
                            string WS_STATUS = ReBuildSCSHRForm.ReCallWSUpdateStatus(lblDOC_NBR.Text, Field_ID);
                            lblWS_STATUS.Text = !string.IsNullOrEmpty(WS_STATUS) ? WS_STATUS == "0" ? "O" : "X" : "?";
                            lblWS_STATUS.ForeColor = !string.IsNullOrEmpty(WS_STATUS) ? WS_STATUS == "0" ? Color.Green : Color.Red : Color.Yellow;
                            lblMSG.Text += lblWS_STATUS.Text != "O" ? WS_STATUS + "<br />" : "";
                            Thread.Sleep(3000); // 呼叫WS才需要休眠
                        }
                    }

                }
            }
        }
    }
}
