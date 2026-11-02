using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Xml;

public partial class CDS_KYTUtils_CreateKYTJSONStructUsedInStartForm : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("建立KYT自動起單用的欄位字串", Request.Url.AbsoluteUri);
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        string outValue = "";
        string formName = txtFromNumber.Text.Trim();
        string formNumber = txtFromNumber.Text.Trim();

        string formVersion = "";
        Dictionary<string, KYTUtilLibs.Types.UOFFormField> formFields = KYTUtilLibs.Utils.UOFUtils.UOFForm.AnalyzeFormFields(formName, out formVersion);
        lblFormVersion.Text = formVersion;

        using (SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM TB_WKF_TASK WHERE DOC_NBR = @DOC_NBR", new DatabaseHelper().Command.Connection.ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", formNumber);
            try
            {
                sda.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string CURRENT_DOC = dr["CURRENT_DOC"].ToString();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(CURRENT_DOC);
                    foreach (XmlNode node in doc.SelectNodes("//Form/FormFieldValue/FieldItem"))
                    {
                        string fieldId = node.Attributes["fieldId"] != null ? node.Attributes["fieldId"].Value : "";
                        //string fieldValue = node.Attributes["fieldValue"] != null ? node.Attributes["fieldValue"].Value : "";
                        if (!string.IsNullOrEmpty(fieldId) &&
                            fieldId == txtFieldID.Text.Trim())
                        {
                            JObject joFormValue = JObject.Parse(node.InnerText);
                            outValue += @"JObject joMain = new JObject();";
                            foreach (JProperty property in joFormValue.Properties())
                            {
                                string _str = string.Format(@"// {0}
                         JObject joSub_{0} = new JObject();
                         joSub_{0}.Add(new JProperty(""TableName"", null));
                         joSub_{0}.Add(new JProperty(""FieldName"", null));
                         joSub_{0}.Add(new JProperty(""FieldValue"", dr[""{1}""].ToString()));
                         joMain.Add(new JProperty(""{0}"", joSub_{0}));
                                ", property.Name,
                                property.Name.Replace("ktxt", "").Replace("kddl", "").Replace("kdp", "").Replace("krbl", "").Replace("kdtp", "").Replace("hid", ""));
                                outValue += Environment.NewLine + _str;
                            }
                            outValue += Environment.NewLine + string.Format(@"dict[""{0}""].fieldValue = user_name;
                                         dict[""{0}""].innerText = joMain.ToString(); ", fieldId);

                        }
                    }
                }
            }
            catch { }
        }
        txtShowStruct.Text = outValue;
    }
}
