using Ede.Uof.Utility.Page;
using JGlobalLibs;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Windows.Forms;
using System.Xml;

public partial class CDS_gemps_WKFConvertToAFJSON : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("", Request.Url.AbsoluteUri);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string returnValue = "";
        lblErrorMsg.Text = "";
        switch (rbtnFormType.SelectedValue)
        {
            case "0": // 外掛欄位
                if (!string.IsNullOrEmpty(txtField_ID.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(txtCURRENT_DOC.Text.Trim()))
                    {
                        returnValue = resolveKYTXMLAndConvert(txtField_ID.Text.Trim(), txtCURRENT_DOC.Text.Trim());
                    }
                    else
                        lblErrorMsg.Text = "請輸入CURRENT_DOC";

                }
                else
                    lblErrorMsg.Text = "請輸入Field_ID";
                break;
            case "1": // 標準欄位
                break;
            default:
                lblErrorMsg.Text = "請選擇欄位形式";
                break;
        }
        txtAFJSON.Text = returnValue;
        if (!string.IsNullOrEmpty(returnValue))
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, 
                                UpdatePanel1.GetType(), 
                                Guid.NewGuid().ToString(), 
                                string.Format(@"alert('轉換完成');"), 
                                true);

        }
    }

    private string resolveKYTXMLAndConvert(string field_id, string xml)
    {
        JObject joReturn = new JObject();
        // 組出Data層
        JArray jaData = new JArray();
        joReturn.Add(new JProperty("data", jaData));
        // 組出兩個起單屬性(一般或外掛)
        JArray jaType1 = new JArray();
        jaData.Add(jaType1);
        // 組出一個一般表單的空結構
        JObject joType1 = new JObject();
        joType1.Add(new JProperty("formtype", "builtIn"));
        joType1.Add(new JProperty("formdata", new JObject()));
        jaType1.Add(joType1);
        // 組出外掛表單結構
        JArray jaType2 = new JArray();
        jaData.Add(jaType2);
        JObject joType2 = new JObject();
        jaType2.Add(joType2);
        // 組出一個上傳檔案的空架構
        JObject joFileUpload = new JObject();
        jaType2.Add(joFileUpload);
        joFileUpload.Add(new JProperty("filedownload", ""));
        joFileUpload.Add(new JProperty("filename", ""));
        // 開始建立表單網頁物件結構
        JObject joFormData = new JObject();
        joType2.Add(new JProperty("formtype", "plugIn"));
        joType2.Add(new JProperty("formdata", joFormData));
        if (!string.IsNullOrEmpty(field_id))
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            XmlNodeList nodes = xdoc.SelectNodes("Form/FormFieldValue/FieldItem");
            foreach (XmlNode _node in nodes)
            {
                XmlElement element = _node as XmlElement;
                string fieldId = element.GetAttribute("fieldId");
                if (fieldId.Equals(field_id))
                {
                    // 表單物件結構
                    JArray jaFieldValue = new JArray();
                    joFormData.Add(new JProperty(fieldId, jaFieldValue));
                    JObject joHtmlObj = new JObject();
                    jaFieldValue.Add(joHtmlObj);
                    string kytJson = element.InnerText;
                    if (!string.IsNullOrEmpty(kytJson))
                    {
                        JObject JoKyt = JObject.Parse(kytJson);
                        foreach (var _jo in JoKyt)
                        {
                            JObject _joObjStruct = JObject.Parse(_jo.Value.ToString());
                            string fieldValue = _joObjStruct["FieldValue"].ToString();

                            if (IsBasee64Encoded(fieldValue)) // 判斷是GV
                            {
                                JArray jaGridView = new JArray();
                                joHtmlObj.Add(new JProperty(_jo.Key, jaGridView));
                                JObject joGV = new JObject();
                                jaGridView.Add(joGV);
                                if (!string.IsNullOrEmpty(fieldValue))
                                {
                                    try
                                    {
                                        byte[] bytes = Convert.FromBase64String(fieldValue);
                                        using (MemoryStream ms = new MemoryStream(bytes))
                                        {
                                            BinaryFormatter formatter = new BinaryFormatter();
                                            DataTable dtGV = formatter.Deserialize(ms) as DataTable;
                                            if (dtGV.Rows.Count > 0)
                                            {
                                                joGV.Add(new JProperty("type", "detail"));
                                                for (int i = 0; i < dtGV.Rows.Count; i++)
                                                {
                                                    DataRow dr = dtGV.Rows[i];
                                                    JArray _jaGV = new JArray();
                                                    joGV.Add((i + 1).ToString(), _jaGV);
                                                    JObject _joGVObj = new JObject();
                                                    _jaGV.Add(_joGVObj);
                                                    foreach (DataColumn dc in dtGV.Columns)
                                                    {
                                                        _joGVObj.Add(new JProperty(dc.ColumnName, dr[dc.ColumnName]));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DebugLog.Log(string.Format(@"WKFConvertToAFJSON.resolveKYTXMLAndConvert.ConvertToGVDataTable.Error:{0}", ex.Message));
                                    }

                                }
                            }
                            else // 不是GridView物件
                            {
                                joHtmlObj.Add(new JProperty(_jo.Key, fieldValue));

                            }
                        }
                    }
                }
            }

        }
        return joReturn.ToString();
    }

    private bool IsBasee64Encoded(string str)
    {
        try
        {
            byte[] data = Convert.FromBase64String(str);
            /**
             判斷base64的條件
                1. 所有的字元有'A'~'Z'或'a'~'z'或'0'~'9'或'+/'
                2. 最後會有0~2個'='
                3. 字串長度必須是4的倍數
             */
            // TODO 正規式判斷最後的等號寫不出來
            // P.S. 因為判斷的對象是DataTable，所以大小寫和數字都一定有

            return (str.Replace(" ", "").Length % 4 == 0) &&
                Regex.IsMatch(str, @"^(?=.*?[A-Z])(?=.*?[a-z\+/])(?=.*?[0-9])(?=.*\={0,2}$)", RegexOptions.None) &&
                (Regex.Matches(str, @"\=").Count >= 0 &&
                Regex.Matches(str, @"\=").Count < 3);
        }
        catch
        {
            return false;
        }
    }
}
