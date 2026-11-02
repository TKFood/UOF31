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

public partial class CDS_SCSHR_WebPages_WEB_KYTI_SCSHR_OVERTIME_BATCH_ERR_FIX : BasePage
{
    private string ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;

        if (!Page.IsPostBack)
        {
            ConstructorCommonSettings.setCommonSettings();
            AddSiteMapNode("批次加班單異常處理", Request.Url.AbsoluteUri);
        }
    }

    private DataTable getDetailData(string DOC_NBR)
    {
        DataTable dtReturn = new DataTable();
        KYTJsonDict dict = null;
        bool isRightField = false;

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT * FROM TB_WKF_TASK WHERE DOC_NBR = @DOC_NBR
            ", ConnectionString))
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
                    XmlNode node = doc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='KYTI_SCSHR_OVERTIME_BATCH']"); // 取出外掛欄位資料
                    isRightField = node != null;
                    if (isRightField)
                        dict = JsonConvert.DeserializeObject<KYTJsonDict>(HttpUtility.HtmlDecode(node.InnerText));
                }
            }
            catch (Exception ex)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"WEB_KYTI_SCSHR_OVERTIME_BATCH_ERR_FIX.getDetailData.TB_WKF_TASK.ERROR:{0}", ex.Message));
            }
            if (isRightField)
            {
                dtReturn = dict.GetDataTable("gvOVs");
                dtReturn.Columns.Add(new DataColumn("SUB_DOC_NBR", typeof(string)));
                dtReturn.Columns.Add(new DataColumn("ZTABLE_STATUS", typeof(string)));
                dtReturn.Columns.Add(new DataColumn("WS_STATUS", typeof(string)));
                for (int i = 0; i < dtReturn.Rows.Count; i++)
                {
                    DataRow dr = dtReturn.Rows[i];
                    dr["SUB_DOC_NBR"] = DOC_NBR + (i + 1).ToString("00");
                }
            }

        }
        return dtReturn;
    }

    protected void rptOVHBatch_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
    {
        RepeaterItem item = e.Item;
        if (item.ItemType == ListItemType.Item ||
            item.ItemType == ListItemType.AlternatingItem)
        {
            DataTable dtSource = ViewState["rptOVHBatch"] as DataTable;
            DataRow row = dtSource.Rows[item.ItemIndex];

            Label lblDOC_NBR = item.FindControl("lblDOC_NBR") as Label;
            Label lblSTATUS = item.FindControl("lblSTATUS") as Label;
            GridView gvOVHBatch = item.FindControl("gvOVHBatch") as GridView;

            gvOVHBatch.DataSource = getDetailData(row["DOC_NBR"].ToString());
            gvOVHBatch.DataBind();
        }
    }

    protected void gvOVHBatch_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;
            Label lblSUB_DOC_NBR = gr.FindControl("lblSUB_DOC_NBR") as Label;
            lblSUB_DOC_NBR.Text = row["SUB_DOC_NBR"].ToString();
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
                 WHERE DOC_NBR LIKE '{0}%' ", txtDOC_NBR.Text.Trim());
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
                    ViewState["rptOVHBatch"] = ds.Tables[0];
                    rptOVHBatch.DataSource = (DataTable)ViewState["rptOVHBatch"];
                    rptOVHBatch.DataBind();
                }
                catch (Exception ex)
                {
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"WEB_KYTI_SCSHR_OVERTIME_BATCH_ERR_FIX.btnSearch_Click.TB_WKF_TASK.ERROR:{0}", ex.Message));
                }
            }
        }

    }

    protected void btnReBuild_Click(object sender, EventArgs e)
    {
        if (rptOVHBatch.Items.Count > 0)
        {
            foreach (RepeaterItem ri in rptOVHBatch.Items)
            {
                Label lblDOC_NBR = ri.FindControl("lblDOC_NBR") as Label;
                Label lblError = ri.FindControl("lblError") as Label;
                GridView gvOVHBatch = ri.FindControl("gvOVHBatch") as GridView;
                // 重新計算
                //SCSHR.Utils.ReBuildSCSHRForm.ReCalcWSAndReplaceOVHBatchForm(lblDOC_NBR.Text);
                // 重新呼叫
                Dictionary<string, Dictionary<string, string>> lsMessage = new Dictionary<string, Dictionary<string, string>>();
                lblError.Text = "";
                lsMessage = SCSHR.Utils.ReBuildSCSHRForm.ReBuildOVHBatchWSUpdateStatus(lblDOC_NBR.Text);
                if (lsMessage.ContainsKey(lblDOC_NBR.Text))
                {
                    lblError.Text = lsMessage[lblDOC_NBR.Text]["MAIN_ERROR"];
                }
                else
                {
                    foreach (GridViewRow gr in gvOVHBatch.Rows)
                    {
                        Label lblZTABLE_STATUS = gr.FindControl("lblZTABLE_STATUS") as Label;
                        Label lblWS_STATUS = gr.FindControl("lblWS_STATUS") as Label;
                        Label lblSUB_DOC_NBR = gr.FindControl("lblSUB_DOC_NBR") as Label;
                        Label lblMSG = gr.FindControl("lblMSG") as Label;
                        
                        if (lsMessage.ContainsKey(lblSUB_DOC_NBR.Text))
                        {
                            if (!string.IsNullOrEmpty(lsMessage[lblSUB_DOC_NBR.Text]["ZTABLE_MSG"]))
                            {
                                lblZTABLE_STATUS.Text = lsMessage[lblSUB_DOC_NBR.Text]["ZTABLE_MSG"] == "0" ? "O" : "X";
                                lblZTABLE_STATUS.ForeColor = lsMessage[lblSUB_DOC_NBR.Text]["ZTABLE_MSG"] == "0" ? Color.Green : Color.Red;
                                lblMSG.Text = lblZTABLE_STATUS.Text != "O" ? lsMessage[lblSUB_DOC_NBR.Text]["ZTABLE_MSG"] + "<br />" : "";
                            }
                            else
                            {
                                lblZTABLE_STATUS.Text = "?";
                                lblZTABLE_STATUS.ForeColor = Color.Yellow;
                            }
                            if (!string.IsNullOrEmpty(lsMessage[lblSUB_DOC_NBR.Text]["ZWS_MSG"]))
                            {
                                lblWS_STATUS.Text = lsMessage[lblSUB_DOC_NBR.Text]["ZWS_MSG"] == "0" ? "O" : "X";
                                lblWS_STATUS.ForeColor = lsMessage[lblSUB_DOC_NBR.Text]["ZWS_MSG"] == "0" ? Color.Green : Color.Red;
                                lblMSG.Text += lblWS_STATUS.Text != "O" ? lsMessage[lblSUB_DOC_NBR.Text]["ZWS_MSG"] + "<br />" : "";
                            }
                            else
                            {
                                lblWS_STATUS.Text = "?";
                                lblWS_STATUS.ForeColor = Color.Yellow;
                            }
                        }
                    }
                }
                Thread.Sleep(3000);
            }
        }
    }

    protected void btnReCalc_Click(object sender, EventArgs e)
    {
        if (rptOVHBatch.Items.Count > 0)
        {
            foreach (RepeaterItem ri in rptOVHBatch.Items)
            {
                Label lblDOC_NBR = ri.FindControl("lblDOC_NBR") as Label;
                SCSHR.Utils.ReBuildSCSHRForm.ReCalcWSAndReplaceOVHBatchForm(lblDOC_NBR.Text);
            }
        }
    }

    protected void btnCheckAll_Click(object sender, EventArgs e)
    {
        if (rptOVHBatch.Items.Count > 0)
        {
            foreach (RepeaterItem ri in rptOVHBatch.Items)
            {
                Label lblDOC_NBR = ri.FindControl("lblDOC_NBR") as Label;
                Label lblError = ri.FindControl("lblError") as Label;
                GridView gvOVHBatch = ri.FindControl("gvOVHBatch") as GridView;

                foreach (GridViewRow gr in gvOVHBatch.Rows)
                {
                    Label lblZTABLE_STATUS = gr.FindControl("lblZTABLE_STATUS") as Label;
                    Label lblWS_STATUS = gr.FindControl("lblWS_STATUS") as Label;
                    Label lblSUB_DOC_NBR = gr.FindControl("lblSUB_DOC_NBR") as Label;
                    Label lblMSG = gr.FindControl("lblMSG") as Label;

                    string ZTABLE_STATUS = ReBuildSCSHRForm.CheckZTableHasData(lblDOC_NBR.Text, "KYTI_SCSHR_OVERTIME_BATCH");
                    string WS_STATUS = ReBuildSCSHRForm.CheckWSHasData(lblSUB_DOC_NBR.Text, "KYTI_SCSHR_OVERTIME_BATCH");

                    lblZTABLE_STATUS.Text = !string.IsNullOrEmpty(ZTABLE_STATUS) ? ZTABLE_STATUS == "0" ? "O" : "X" : "?";
                    lblZTABLE_STATUS.ForeColor = !string.IsNullOrEmpty(ZTABLE_STATUS) ? ZTABLE_STATUS == "0" ? Color.Green : Color.Red : Color.Yellow;

                    lblWS_STATUS.Text = !string.IsNullOrEmpty(WS_STATUS) ? WS_STATUS == "0" ? "O" : "X" : "?";
                    lblWS_STATUS.ForeColor = !string.IsNullOrEmpty(WS_STATUS) ? WS_STATUS == "0" ? Color.Green : Color.Red : Color.Yellow;
                }
            }
        }
    }

    protected void btnReCalcByBOFind_Click(object sender, EventArgs e)
    {
        if (rptOVHBatch.Items.Count > 0)
        {
            foreach (RepeaterItem ri in rptOVHBatch.Items)
            {
                Label lblDOC_NBR = ri.FindControl("lblDOC_NBR") as Label;
                SCSHR.Utils.ReBuildSCSHRForm.ReCalcWSAndReplaceOVHBatchFormByBOFind(lblDOC_NBR.Text);
            }
        }
    }
}
