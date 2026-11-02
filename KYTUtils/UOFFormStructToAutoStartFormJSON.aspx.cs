using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using JGlobalLibs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Windows.Forms;
using System.Xml;

/**
* 修改時間：2021/01/16
* 修改人員：陳緯榕
* 修改項目：
    * 新增「輸出成自動起單C#片段」
* 修改原因：
    * 要寫標準表單的自動起單
* 修改位置：
    * 「前端網頁」中，新增〈TextBox：txtAFCSharp〉
    * 新增「getFieldTypeCSharpStruct(string fieldID, string fieldType, XmlElement xmlElement, string filterTypes)」取得C#程式用的結構
    * 新增「resolveUOFXMLAndConvertToCSharp(string VERSION_FIELD)」過濾UOF不填資料的欄位
* **/

public partial class CDS_KYTUtils_UOFFormStructToAutoStartFormJSON : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("UOF表單結構轉換為自動起單用JSON", Request.Url.AbsoluteUri);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string returnValue = "";
        string returnCSharpCut = "";
        lblErrorMsg.Text = "";
        switch (rbtnFormType.SelectedValue)
        {
            case "0": // 外掛欄位
                if (!string.IsNullOrEmpty(txtField_ID.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(txtVERSION_FIELD.Text.Trim()))
                    {
                        returnValue = resolveKYTXMLAndConvert(txtField_ID.Text.Trim(), txtVERSION_FIELD.Text.Trim());
                        // TODO 輸出KYT用
                    }
                    else
                        lblErrorMsg.Text = "請輸入TB_WKF_FORM_VERSION-VERSION_FIELD";
                }
                else
                    lblErrorMsg.Text = "請輸入Field_ID";
                break;
            case "1": // 標準欄位
                if (!string.IsNullOrEmpty(txtVERSION_FIELD.Text.Trim()))
                {
                    returnValue = resolveUOFXMLAndConvertToJSON(txtVERSION_FIELD.Text.Trim());
                    returnCSharpCut = resolveUOFXMLAndConvertToCSharp(txtVERSION_FIELD.Text.Trim());
                }
                else
                    lblErrorMsg.Text = "請輸入TB_WKF_FORM_VERSION-VERSION_FIELD";
                break;
            default:
                lblErrorMsg.Text = "請選擇欄位形式";
                break;
        }
        txtAFJSON.Text = returnValue;
        txtAFCSharp.Text = returnCSharpCut;
        if (!string.IsNullOrEmpty(returnValue))
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1,
                                UpdatePanel1.GetType(),
                                Guid.NewGuid().ToString(),
                                string.Format(@"alert('轉換完成');"),
                                true);

        }
    }

    /// <summary>
    /// 取得結構
    /// </summary>
    /// <param name="fieldType"></param>
    /// <param name="xmlElement"></param>
    /// <param name="filterTypes"></param>
    /// <returns></returns>
    private JObject getFieldTypeJSONStruct(string fieldType, XmlElement xmlElement, string filterTypes)
    {
        bool needOption = chkGetDefault.Checked;
        JObject joStruct = new JObject();
        switch (fieldType)
        {
            case "allUser": // 選人物件
                joStruct.Add(new JProperty("type", "SELECT_USER"));
                joStruct.Add(new JProperty("value", ""));
                if (needOption)
                {
                    joStruct.Add(new JProperty("DefaultVlue", xmlElement.GetAttribute("fieldDefault")));
                    joStruct.Add(new JProperty("DESC", xmlElement.GetAttribute("fieldName")));
                }
                break;
            case "singleLineText": // 單行文字
            case "multiLineText": // 多行文字
                joStruct.Add(new JProperty("type", "TEXT"));
                joStruct.Add(new JProperty("value", ""));
                if (needOption)
                {
                    joStruct.Add(new JProperty("DefaultVlue", xmlElement.GetAttribute("fieldDefault")));
                    joStruct.Add(new JProperty("DESC", xmlElement.GetAttribute("fieldName")));
                }
                break;
            case "dataGrid": // 明細
                joStruct.Add(new JProperty("type", "DETAIL"));
                JArray jaDetails = new JArray();
                joStruct.Add(new JProperty("1", jaDetails));
                XmlNodeList xnl = xmlElement.SelectNodes("dataGrid/DataGridItem");
                foreach (XmlNode _gn in xnl)
                {
                    XmlElement _element = (XmlElement)_gn;
                    string _fieldType = _element.GetAttribute("fieldType");
                    string _fieldId = _element.GetAttribute("fieldId");
                    if (!filterTypes.Contains(fieldType)) // 不須過濾的欄位格式
                    {
                        JObject _joStruct = new JObject();
                        JArray _jaStruct = new JArray();
                        JObject __joStruct = getFieldTypeJSONStruct(_fieldType, _element, filterTypes);
                        _jaStruct.Add(__joStruct);
                        _joStruct.Add(new JProperty(_fieldId, _jaStruct));
                        jaDetails.Add(_joStruct);
                    }
                }
                break;
            case "dateSelect": // 日期
                joStruct.Add(new JProperty("type", "DATE"));
                joStruct.Add(new JProperty("value", ""));
                if (needOption)
                {
                    joStruct.Add(new JProperty("DefaultVlue", xmlElement.GetAttribute("fieldDefault")));
                    joStruct.Add(new JProperty("DESC", xmlElement.GetAttribute("fieldName")));
                }
                break;
            case "timeSelect": // 時間
                joStruct.Add(new JProperty("type", "TIME"));
                joStruct.Add(new JProperty("value", ""));
                if (needOption)
                {
                    joStruct.Add(new JProperty("DefaultVlue", xmlElement.GetAttribute("fieldDefault")));
                    joStruct.Add(new JProperty("DESC", xmlElement.GetAttribute("fieldName")));
                }
                break;
            case "checkBox": // 多選
                joStruct.Add(new JProperty("type", "CHECKBOX"));
                joStruct.Add(new JProperty("value", ""));
                if (needOption)
                {
                    joStruct.Add(new JProperty("DefaultVlue", xmlElement.GetAttribute("fieldDefault")));
                    joStruct.Add(new JProperty("DESC", xmlElement.GetAttribute("fieldName")));
                    XmlNodeList items = xmlElement.SelectNodes("fieldSource/ItemList");

                    if (items != null &&
                        items.Count > 0)
                    {
                        string _options = "";
                        foreach (XmlNode __xn in items)
                        {
                            _options += __xn.InnerText + "|";
                        }
                        joStruct.Add(new JProperty("Options", _options.Substring(0, _options.Length - 1)));
                    }
                }
                break;
            case "radioButton": // 單選
                joStruct.Add(new JProperty("type", "RADIOBUTTON"));
                joStruct.Add(new JProperty("value", ""));
                if (needOption)
                {
                    joStruct.Add(new JProperty("DefaultVlue", xmlElement.GetAttribute("fieldDefault")));
                    joStruct.Add(new JProperty("DESC", xmlElement.GetAttribute("fieldName")));
                    XmlNodeList items = xmlElement.SelectNodes("fieldSource/ItemList");

                    if (items != null &&
                        items.Count > 0)
                    {
                        string _options = "";
                        foreach (XmlNode __xn in items)
                        {
                            _options += __xn.InnerText + "|";
                        }
                        joStruct.Add(new JProperty("Options", _options.Substring(0, _options.Length - 1)));
                    }
                }
                break;
            case "dropDownList": // 下拉選單
                joStruct.Add(new JProperty("type", "DROPDOWNLIST"));
                joStruct.Add(new JProperty("value", ""));
                if (needOption)
                {
                    joStruct.Add(new JProperty("DefaultVlue", xmlElement.GetAttribute("fieldDefault")));
                    joStruct.Add(new JProperty("DESC", xmlElement.GetAttribute("fieldName")));
                    XmlNodeList items = xmlElement.SelectNodes("fieldSource/ItemList");

                    if (items != null &&
                        items.Count > 0)
                    {
                        string _options = "";
                        foreach (XmlNode __xn in items)
                        {
                            _options += __xn.InnerText + "|";
                        }
                        joStruct.Add(new JProperty("Options", _options.Substring(0, _options.Length - 1)));
                    }
                }
                break;
            case "htmlEditor": // 文字編輯
                joStruct.Add(new JProperty("type", "HTML_EDIT"));
                joStruct.Add(new JProperty("value", ""));
                if (needOption)
                {
                    joStruct.Add(new JProperty("DefaultVlue", xmlElement.GetAttribute("fieldDefault")));
                    joStruct.Add(new JProperty("DESC", xmlElement.GetAttribute("fieldName")));
                }
                break;
            case "numberText": // 數值
                joStruct.Add(new JProperty("type", "NUMERIC"));
                joStruct.Add(new JProperty("value", ""));
                if (needOption)
                {
                    joStruct.Add(new JProperty("DefaultVlue", xmlElement.GetAttribute("fieldDefault")));
                    joStruct.Add(new JProperty("DESC", xmlElement.GetAttribute("fieldName")));
                }
                break;
            case "hyperLink": // 超鏈結
                joStruct.Add(new JProperty("type", "URL"));
                joStruct.Add(new JProperty("value", ""));
                joStruct.Add(new JProperty("view_name", ""));
                if (needOption)
                {
                    joStruct.Add(new JProperty("DefaultVlue", xmlElement.GetAttribute("fieldDefault")));
                    joStruct.Add(new JProperty("DESC", xmlElement.GetAttribute("fieldName")));
                }
                break;
        }
        return joStruct;
    }

    /// <summary>
    /// 取得C#程式用的結構
    /// </summary>
    /// <param name="fieldID"></param>
    /// <param name="fieldType"></param>
    /// <param name="xmlElement"></param>
    /// <param name="filterTypes"></param>
    /// <returns></returns>
    private string getFieldTypeCSharpStruct(string fieldID, string fieldType, XmlElement xmlElement, string filterTypes)
    {
        bool needOption = chkGetDefault.Checked;
        List<string> lsResult = new List<string>();
        switch (fieldType)
        {
            case "allUser": // 選人物件
                lsResult.Add(string.Format(@"// {0}", xmlElement.GetAttribute("fieldName")));
                lsResult.Add(string.Format(@"// 預設： {0}", xmlElement.GetAttribute("fieldDefault")));
                lsResult.Add(@"// 帳號：test2,Oli03,Wu0107");
                lsResult.Add(string.Format(@"form[""{0}""].fieldValue = """"; // 顯示：test2(test2)、Oli03(Oli03)、陳小紅(Wu0107)", fieldID));
                lsResult.Add(string.Format(@"form[""{0}""].realValue = """"; // UserSet.GetXML()", fieldID));

                break;
            case "dataGrid": // 明細
                lsResult.Add(string.Format(@"// {0}", xmlElement.GetAttribute("fieldName")));
                lsResult.Add(@"// 建立xml");
                lsResult.Add(@"XmlDocument doc = new XmlDocument();");
                lsResult.Add(@"// 建立根節點");
                lsResult.Add(@"XmlElement datagrid = doc.CreateElement(""DataGrid"");");
                lsResult.Add(@"doc.AppendChild(datagrid);");
                

                lsResult.Add(@"// foreach() { // 巡覽明細項填資料");
                lsResult.Add(@"// 建立子節點");
                lsResult.Add(string.Format(@"XmlElement grow = doc.CreateElement(""Row"");", fieldID));
                lsResult.Add(@"grow.SetAttribute(""order"", ""0"");");
                lsResult.Add(@"// 建立末節點");
                lsResult.Add(@"XmlElement gcell = doc.CreateElement(""Cell"");");
                XmlNodeList xnl = xmlElement.SelectNodes("dataGrid/DataGridItem");
                foreach (XmlNode _gn in xnl)
                {
                    XmlElement _element = (XmlElement)_gn;
                    string _fieldType = _element.GetAttribute("fieldType");
                    string _fieldId = _element.GetAttribute("fieldId");
                    if (!filterTypes.Contains(_fieldType)) // 不須過濾的欄位格式
                    {
                        switch (_fieldType)
                        {
                            case "allUser": // 選人物件
                                lsResult.Add(string.Format(@"// {0}", _element.GetAttribute("fieldName")));
                                lsResult.Add(string.Format(@"// 預設： {0}", _element.GetAttribute("fieldDefault")));
                                lsResult.Add(@"// 帳號：test2,Oli03,Wu0107");
                                lsResult.Add(@"gcell = doc.CreateElement(""Cell"");");
                                lsResult.Add(string.Format(@"gcell.SetAttribute(""fieldId"", ""{0}"");", _fieldId));
                                lsResult.Add(string.Format(@"gcell.SetAttribute(""fieldValue"", """"); // 顯示：test2(test2)、Oli03(Oli03)、陳小紅(Wu0107)"));
                                lsResult.Add(string.Format(@"gcell.SetAttribute(""realValue"", """");  // UserSet.GetXML()"));
                                break;
                            case "checkBox": // 多選
                            case "dropDownList": // 下拉選單
                            case "radioButton": // 單選
                                XmlNodeList _items = xmlElement.SelectNodes("fieldSource/ItemList");
                                string __options = "";
                                if (_items != null &&
                                       _items.Count > 0)
                                {
                                    foreach (XmlNode __xn in _items)
                                    {
                                        __options += __xn.InnerText + "|";
                                    }
                                    __options = __options.Substring(0, __options.Length - 1);
                                }
                                lsResult.Add(string.Format(@"// {0}", _element.GetAttribute("fieldName")));
                                lsResult.Add(string.Format(@"// 預設： {0}", _element.GetAttribute("fieldDefault")));
                                lsResult.Add(string.Format(@"// 選項： {0}", __options));
                                lsResult.Add(@"gcell = doc.CreateElement(""Cell"");");
                                lsResult.Add(string.Format(@"gcell.SetAttribute(""fieldId"", ""{0}"");", _fieldId));
                                lsResult.Add(string.Format(@"gcell.SetAttribute(""fieldValue"", ""{0}"");", _element.GetAttribute("fieldDefault")));
                                lsResult.Add(string.Format(@"gcell.SetAttribute(""realValue"", """");"));
                                break;
                            case "dateSelect": // 日期
                            case "timeSelect": // 時間
                            case "singleLineText": // 單行文字
                            case "multiLineText": // 多行文字
                            case "htmlEditor": // 文字編輯
                            case "numberText": // 數值
                            case "hyperLink": // 超鏈結
                           
                                lsResult.Add(string.Format(@"// {0}", _element.GetAttribute("fieldName")));
                                lsResult.Add(string.Format(@"// 預設： {0}", _element.GetAttribute("fieldDefault")));
                                if (fieldType == "hyperLink")
                                    lsResult.Add(string.Format(@"// 超連結： 顯示文字@網址； 測試@http://www.google.com"));
                                lsResult.Add(@"gcell = doc.CreateElement(""Cell"");");
                                lsResult.Add(string.Format(@"gcell.SetAttribute(""fieldId"", ""{0}"");", _fieldId));
                                lsResult.Add(string.Format(@"gcell.SetAttribute(""fieldValue"", ""{0}"");", _element.GetAttribute("fieldDefault")));
                                lsResult.Add(string.Format(@"gcell.SetAttribute(""realValue"", """");"));
                                break;
                        }

                        lsResult.Add(@"gcell.SetAttribute(""customValue"", """");");
                        lsResult.Add(@"// 加入到Row的子節點下");
                        lsResult.Add(@"grow.AppendChild(gcell);");
                        lsResult.Add(@"// 加入到DataGrid節點底下");
                        lsResult.Add(@"datagrid.AppendChild(grow);");
                    }
                }
               
                lsResult.Add(@"// } // 巡覽明細項填資料");

                lsResult.Add(string.Format(@"form[""{0}""].fieldValue = """"; // 起單者名稱", fieldID));
                lsResult.Add(string.Format(@"form[""{0}""].innerText = doc.InnerXml;", fieldID));

                break;
            case "checkBox": // 多選
            case "dropDownList": // 下拉選單
            case "radioButton": // 單選
                XmlNodeList items = xmlElement.SelectNodes("fieldSource/ItemList");
                string _options = "";
                if (items != null &&
                       items.Count > 0)
                {
                    foreach (XmlNode __xn in items)
                    {
                        _options += __xn.InnerText + "|";
                    }
                    _options = _options.Substring(0, _options.Length - 1);
                }
                lsResult.Add(string.Format(@"// {0}", xmlElement.GetAttribute("fieldName")));
                lsResult.Add(string.Format(@"// 預設： {0}", xmlElement.GetAttribute("fieldDefault")));
                lsResult.Add(string.Format(@"// 選項： {0}", _options));
                lsResult.Add(string.Format(@"form[""{0}""].fieldValue = ""{1}"";", fieldID, xmlElement.GetAttribute("fieldDefault")));
                lsResult.Add(string.Format(@"form[""{0}""].realValue = """";", fieldID));
                break;
            case "dateSelect": // 日期
            case "timeSelect": // 時間
            case "singleLineText": // 單行文字
            case "multiLineText": // 多行文字
            case "htmlEditor": // 文字編輯
            case "numberText": // 數值
            case "hyperLink": // 超鏈結
                lsResult.Add(string.Format(@"// {0}", xmlElement.GetAttribute("fieldName")));
                lsResult.Add(string.Format(@"// 預設： {0}", xmlElement.GetAttribute("fieldDefault")));
                if (fieldType == "hyperLink")
                    lsResult.Add(string.Format(@"// 超連結： 顯示文字@網址； 測試@http://www.google.com"));
                lsResult.Add(string.Format(@"form[""{0}""].fieldValue = ""{1}"";", fieldID, xmlElement.GetAttribute("fieldDefault")));
                lsResult.Add(string.Format(@"form[""{0}""].realValue = """";", fieldID));
                break;
        }
        return string.Join(Environment.NewLine, lsResult.ToArray());
    }

    private string resolveUOFXMLAndConvertToJSON(string VERSION_FIELD)
    {
        string filterTypes = "autoNumber,userDept,userProposer,hiddenField,fileButton";
        JObject joReturn = new JObject();
        // 組出Data層
        JArray jaData = new JArray();
        joReturn.Add(new JProperty("data", jaData));

        XmlDocument xmlVersion_field = new XmlDocument();
        xmlVersion_field.LoadXml(VERSION_FIELD);

        if (xmlVersion_field != null)
        {
            XmlNodeList xnl = xmlVersion_field.SelectNodes("//VersionField/FieldItem");
            foreach (XmlNode _xn in xnl)
            {
                XmlElement _element = (XmlElement)_xn;
                string fieldType = _element.GetAttribute("fieldType");
                string fieldId = _element.GetAttribute("fieldId");
                if (!filterTypes.Contains(fieldType)) // 不須過濾的欄位格式
                {
                    JObject joStruct = new JObject();
                    JArray _jaStruct = new JArray();

                    JObject _joStruct = getFieldTypeJSONStruct(fieldType, _element, filterTypes);
                    _jaStruct.Add(_joStruct);

                    joStruct.Add(new JProperty(fieldId, _jaStruct));
                    jaData.Add(joStruct);
                }
            }

        }
        return joReturn.ToString();
    }

    /// <summary>
    /// 過濾UOF不填資料的欄位
    /// </summary>
    /// <param name="VERSION_FIELD"></param>
    /// <returns></returns>
    private string resolveUOFXMLAndConvertToCSharp(string VERSION_FIELD)
    {
        string filterTypes = "autoNumber,userDept,userProposer,hiddenField,fileButton";
        List<string> lsResult = new List<string>();

        XmlDocument xmlVersion_field = new XmlDocument();
        xmlVersion_field.LoadXml(VERSION_FIELD);

        if (xmlVersion_field != null)
        {
            XmlNodeList xnl = xmlVersion_field.SelectNodes("//VersionField/FieldItem");
            foreach (XmlNode _xn in xnl)
            {
                XmlElement _element = (XmlElement)_xn;
                string fieldType = _element.GetAttribute("fieldType");
                string fieldId = _element.GetAttribute("fieldId");
                if (!filterTypes.Contains(fieldType)) // 如果是不須過濾的欄位格式
                {
                    lsResult.Add(getFieldTypeCSharpStruct(fieldId, fieldType, _element, filterTypes));
                }
            }

        }
        return string.Join(Environment.NewLine, lsResult.ToArray());
    }

    private string resolveKYTXMLAndConvert(string field_id, string xml)
    {
        JObject joReturn = new JObject();
        //// 組出Data層
        //JArray jaData = new JArray();
        //joReturn.Add(new JProperty("data", jaData));
        //// 組出兩個起單屬性(一般或外掛)
        //JArray jaType1 = new JArray();
        //jaData.Add(jaType1);
        //// 組出一個一般表單的空結構
        //JObject joType1 = new JObject();
        //joType1.Add(new JProperty("formtype", "builtIn"));
        //joType1.Add(new JProperty("formdata", new JObject()));
        //jaType1.Add(joType1);
        //// 組出外掛表單結構
        //JArray jaType2 = new JArray();
        //jaData.Add(jaType2);
        //JObject joType2 = new JObject();
        //jaType2.Add(joType2);
        //// 組出一個上傳檔案的空架構
        //JObject joFileUpload = new JObject();
        //jaType2.Add(joFileUpload);
        //joFileUpload.Add(new JProperty("filedownload", ""));
        //joFileUpload.Add(new JProperty("filename", ""));
        //// 開始建立表單網頁物件結構
        //JObject joFormData = new JObject();
        //joType2.Add(new JProperty("formtype", "plugIn"));
        //joType2.Add(new JProperty("formdata", joFormData));
        //if (!string.IsNullOrEmpty(field_id))
        //{
        //    XmlDocument xdoc = new XmlDocument();
        //    xdoc.LoadXml(xml);
        //    XmlNodeList nodes = xdoc.SelectNodes("Form/FormFieldValue/FieldItem");
        //    foreach (XmlNode _node in nodes)
        //    {
        //        XmlElement element = _node as XmlElement;
        //        string fieldId = element.GetAttribute("fieldId");
        //        if (fieldId.Equals(field_id))
        //        {
        //            // 表單物件結構
        //            JArray jaFieldValue = new JArray();
        //            joFormData.Add(new JProperty(fieldId, jaFieldValue));
        //            JObject joHtmlObj = new JObject();
        //            jaFieldValue.Add(joHtmlObj);
        //            string kytJson = element.InnerText;
        //            if (!string.IsNullOrEmpty(kytJson))
        //            {
        //                JObject JoKyt = JObject.Parse(kytJson);
        //                foreach (var _jo in JoKyt)
        //                {
        //                    JObject _joObjStruct = JObject.Parse(_jo.Value.ToString());
        //                    string fieldValue = _joObjStruct["FieldValue"].ToString();

        //                    if (IsBasee64Encoded(fieldValue)) // 判斷是GV
        //                    {
        //                        JArray jaGridView = new JArray();
        //                        joHtmlObj.Add(new JProperty(_jo.Key, jaGridView));
        //                        JObject joGV = new JObject();
        //                        jaGridView.Add(joGV);
        //                        if (!string.IsNullOrEmpty(fieldValue))
        //                        {
        //                            try
        //                            {
        //                                byte[] bytes = Convert.FromBase64String(fieldValue);
        //                                using (MemoryStream ms = new MemoryStream(bytes))
        //                                {
        //                                    BinaryFormatter formatter = new BinaryFormatter();
        //                                    DataTable dtGV = formatter.Deserialize(ms) as DataTable;
        //                                    if (dtGV.Rows.Count > 0)
        //                                    {
        //                                        joGV.Add(new JProperty("type", "detail"));
        //                                        for (int i = 0; i < dtGV.Rows.Count; i++)
        //                                        {
        //                                            DataRow dr = dtGV.Rows[i];
        //                                            JArray _jaGV = new JArray();
        //                                            joGV.Add((i + 1).ToString(), _jaGV);
        //                                            JObject _joGVObj = new JObject();
        //                                            _jaGV.Add(_joGVObj);
        //                                            foreach (DataColumn dc in dtGV.Columns)
        //                                            {
        //                                                _joGVObj.Add(new JProperty(dc.ColumnName, dr[dc.ColumnName]));
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                DebugLog.Log(string.Format(@"WKFConvertToAFJSON.resolveKYTXMLAndConvert.ConvertToGVDataTable.Error:{0}", ex.Message));
        //                            }

        //                        }
        //                    }
        //                    else // 不是GridView物件
        //                    {
        //                        joHtmlObj.Add(new JProperty(_jo.Key, fieldValue));

        //                    }
        //                }
        //            }
        //        }
        //    }

        //}
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

    protected void btnGetFormField_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtFormName.Text.Trim()))
        {
            string errorMessage = "";
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SELECT TOP 1 VERSION_FIELD 
                  FROM TB_WKF_FORM_VERSION 
                 WHERE FORM_ID = (SELECT FORM_ID 
					                FROM TB_WKF_FORM 
				                   WHERE FORM_NAME = @FORM_NAME)
              ORDER BY [VERSION] DESC
                ", new DatabaseHelper().Command.Connection.ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@FORM_NAME", txtFormName.Text.Trim());
                try
                {
                    if (sda.Fill(ds) > 0)
                    {
                        txtVERSION_FIELD.Text = ds.Tables[0].Rows[0]["VERSION_FIELD"].ToString();
                    }
                    else
                        errorMessage = string.Format("表單名稱：「{0}」，找不到", txtFormName.Text.Trim());
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1,
                                                  UpdatePanel1.GetType(),
                                                  Guid.NewGuid().ToString(),
                                                  string.Format(@"alert('失敗：{0}');", errorMessage),
                                                  true);
                }
                lblErrorMsg.Text = errorMessage;
            }
        }
    }
}
