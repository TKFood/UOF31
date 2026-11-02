using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UOFAssist.WKF;

public partial class CDS_TEST_WebPages_ModifyForm : BasePage
{
    /// <summary>
    /// 資料庫連通字串
    /// </summary>
    string ConnectionString;

    /// <summary>
    /// KYT控制元件
    /// </summary>
    KYTController kytController;

    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
        kytController = new KYTController(UpdatePanel1);
        kytController.SetAllViewType(KYTViewType.Input);
        //ktxtDOC_NBR.Text = "BPM210200019";
        //ktxtFieldID.Text = "SHERATON_REPAIR";
        ktxtValue_Ori.ReadOnly = true;
        if (!Page.IsPostBack)
        {

        }

    }


    /// <summary>
    /// 取得表單結構
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGetForm_Click(object sender, EventArgs e)
    {
        SetTaskID();
        string TaskID = "";
        TaskID = ViewState["TaskID"].ToString();
        if (!string.IsNullOrEmpty(TaskID))
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ITEMS", typeof(string)));
            dt.Columns.Add(new DataColumn("VALUE", typeof(string)));
            dt.Columns.Add(new DataColumn("DataSource", typeof(string)));
            string joInnerText = KYTUtilLibs.Utils.KYTAssetsUtils.GetFieldNode(TaskID, ktxtFieldID.Text).InnerText;
            if (!string.IsNullOrEmpty(joInnerText))
            {
                JObject jobj = JObject.Parse(joInnerText);
                foreach (JProperty property in jobj.Properties())
                {
                    DataRow ndr = dt.NewRow();
                    ndr["ITEMS"] = property.Name;
                    JObject joALL = JObject.Parse(property.Value.ToString());
                    if (joALL["FieldValue"].ToString().Contains("{"))
                    {
                        if (joALL["FieldValue"].ToString().Contains("冠永騰收納用") || joALL["FieldValue"].ToString().Contains("["))
                        {
                            // 明細項
                            DataTable dtResult = JsonConvert.DeserializeObject<DataTable>(joALL["FieldValue"].ToString());

                            ndr["VALUE"] = joALL["FieldValue"].ToString();
                            //DataTable dtgvItems = new DataTable();
                            //DataTable dtRows = new DataTable();
                            //DataTable dtRowName = new DataTable();
                            //dtRows.Columns.Add(new DataColumn("ITEMS", typeof(string)));
                            //dtRowName.Columns.Add(new DataColumn("TXT", typeof(string)));
                            //int items = 0;
                            //foreach (DataColumn dc in dtResult.Columns)
                            //{
                            //    items++;
                            //    dtgvItems.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                            //    dtRowName.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                            //    DataRow ndrRowName = dtRowName.NewRow();
                            //    DataRow ndrRows = dtRows.NewRow();
                            //    ndrRows["ITEMS"] = items;
                            //    ndrRowName["TXT"] = dc.ColumnName;
                            //    dtRows.Rows.Add(ndrRows);
                            //    dtRowName.Rows.Add(ndrRowName);
                            //}
                            //kddlRows.DataSource = dtRows;
                            //kddlRows.DataBind();
                            //kddlRowName.DataSource = dtRowName;
                            //kddlRowName.DataBind();
                        }
                        else if (joALL["FieldValue"].ToString().Contains("UserSet"))
                        {
                            // Userset
                            ndr["VALUE"] = joALL["FieldValue"].ToString();
                            UserSet us = new UserSet();
                        }
                        else
                        {
                            // DropDownList or RadioButton
                            JObject joFieldValue = JObject.Parse(joALL["FieldValue"].ToString());
                            ndr["VALUE"] = joFieldValue["SelectedValue"].ToString();
                            ndr["DataSource"] = joFieldValue["DataSource"].ToString();

                        }
                    }
                    else
                    {
                        ndr["VALUE"] = joALL["FieldValue"].ToString();
                    }
                    dt.Rows.Add(ndr);
                }
                kddlItems.DataSource = dt;
                kddlItems.DataBind();
                ViewState["FOMR_Field"] = dt;
            }
            kddlItems.BindDataOnly = true;
            ktxtValue_Ori.Text = dt.Rows[0]["VALUE"].ToString();
        }
    }

    /// <summary>
    /// 更新表單
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string TaskID = "";
        TaskID = ViewState["TaskID"].ToString();
        if (!string.IsNullOrEmpty(TaskID))
        {
            JObject joInnerText = JObject.Parse(KYTUtilLibs.Utils.KYTAssetsUtils.GetFieldNode(TaskID, ktxtFieldID.Text).InnerText);
            foreach (JProperty property in joInnerText.Properties())
            {
                if (kddlItems.SelectedValue == property.Name)
                {
                    JObject joALL = JObject.Parse(property.Value.ToString());
                    if (joALL["FieldValue"].ToString().Contains("{"))
                    {
                        if (joALL["FieldValue"].ToString().Contains("冠永騰收納用") || joALL["FieldValue"].ToString().Contains("["))
                        {
                            // 明細項
                            joInnerText[kddlItems.SelectedValue]["FieldValue"] = ktxtValue.Text;

                        }
                        else if (joALL["FieldValue"].ToString().Contains("UserSet"))
                        {
                            // Userset
                            UserSet us = new UserSet();
                        }
                        else
                        {
                            // DropDownList or RadioButton
                            JObject joFieldValue = JObject.Parse(joALL["FieldValue"].ToString());
                            joInnerText[kddlItems.SelectedValue]["FieldValue"]["SelectedValue"] = ktxtValue.Text;

                        }
                    }
                    else
                    {
                        joInnerText[kddlItems.SelectedValue]["FieldValue"] = ktxtValue.Text;
                    }
                }
            }
            KYTUtilLibs.Utils.UOFUtils.UOFForm.ReplaceDocumentContent(TaskID, ktxtFieldID.Text, JsonConvert.SerializeObject(joInnerText));
            string select = kddlItems.SelectedValue;
            btnGetForm_Click(null, null);
            kddlItems.BindDataOnly = true;
            kddlItems.SelectedValue = select;
            kddlItems_SelectedIndexChanged(null, null);
        }
    }

    /// <summary>
    /// 表單欄位_變更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void kddlItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["FOMR_Field"];
        DataRow[] arr_row = dt.Select("ITEMS = '" + kddlItems.SelectedValue + "'");
        ktxtValue_Ori.Text = arr_row[0]["VALUE"].ToString();
    }

    /// <summary>
    /// 表單編號_改變
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ktxtDOC_NBR_TextChanged(object sender, EventArgs e)
    {
        SetTaskID();
        // 清空欄位
        kddlItems.ItemClear();
        ktxtValue_Ori.Text = "";
        ktxtValue.Text = "";
    }

    /// <summary>
    /// 取得表單TaskID
    /// </summary>
    private void SetTaskID()
    {
        // 取得表單Task_ID
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
        SELECT *
          FROM [TB_WKF_TASK] 
         WHERE DOC_NBR = @DOC_NBR
        ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", ktxtDOC_NBR.Text);
            try
            {
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["TaskID"] = ds.Tables[0].Rows[0]["TASK_ID"];
                }
                else
                {
                    ViewState["TaskID"] = "";
                    ktxtMSG.Text = "此表單編號不存在";
                }
            }
            catch (Exception ex)
            {
                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format("ModifyForm::ktxtDOC_NBR_TextChanged()::取得表單TaskID::錯誤:{0}", ex.Message));
            }
        }
    }
}