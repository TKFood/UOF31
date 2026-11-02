using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using JGlobalLibs.Types;
using KYTLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Xml;

public partial class CDS_SCSHR_WebPages_Temp_FixTravelTBY : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("", Request.Url.AbsoluteUri);
        }
    }

    protected void btnFix_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDOC_NBR.Text.Trim()))
        {
            KYTJsonDict dict = null;
            JObject joInnerText = null;
            string TASK_ID = "";
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SELECT TOP 1 * 
                  FROM TB_WKF_TASK 
                 WHERE DOC_NBR = @DOC_NBR 
            ", new DatabaseHelper().Command.Connection.ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", txtDOC_NBR.Text.Trim());
                try
                {
                    if (sda.Fill(ds) > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(dr["CURRENT_DOC"].ToString());
                        XmlNode node = doc.SelectSingleNode("//Form/FormFieldValue/FieldItem[@fieldId='KYTI_SCSHR_TRAVEL']"); // 取出外掛欄位資料
                        dict = JsonConvert.DeserializeObject<KYTJsonDict>(HttpUtility.HtmlDecode(node.InnerText));
                        joInnerText = JObject.Parse(HttpUtility.HtmlDecode(node.InnerText));
                        TASK_ID = dr["TASK_ID"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format(@"Temp_FixTravelTBY.btnFix_Click.TB_WKF_TASK.SELECT.ERROR:{0}", ex.Message);
                    DebugLog.Log(DebugLog.LogLevel.Error, message);
                }

                DataTable _dtPLs = dict.GetDataTable("gvPLs");
                foreach (DataRow dr in _dtPLs.Rows)
                {
                    dr["TBY"] = "高鐵";
                }

                if (joInnerText != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        try
                        {
                            formatter.Serialize(ms, _dtPLs);
                            joInnerText["gvPLs"]["FieldValue"] = Convert.ToBase64String(ms.ToArray());
                        }
                        catch (Exception ex)
                        {
                            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"Temp_FixTravelTBY.btnFix_Click.MemoryStream.Serialize._dtPLs.ERROR:{0}", ex.Message));
                        }
                    }
                }
                JGlobalLibs.UOFUtils.ReplaceDocumentContent(TASK_ID, "KYTI_SCSHR_TRAVEL", JsonConvert.SerializeObject(joInnerText));

            }
        }
    }
}
