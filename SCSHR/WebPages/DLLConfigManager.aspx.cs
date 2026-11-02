using Ede.Uof.Utility.Page;
using KYTLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

/**
* 修改時間：2019/04/10
* 修改人員：高常昇
* 修改項目：
    * JsonArray與JsonObject差異，應給予各自Json行為
* 修改位置： 
    * 「CreateJsonSub」內，新增欄位「REMARK」用於判斷類型
    * 「btnJSONTITLE_Click」內，點選Json的時候判斷是否要顯示可以新增Json按鈕「lblEditBtn」「btnJsonSubNew」、「btnJsonSubNew2」，如果不為JArray就不顯示新增按鈕
    * 「btnSave_Click」內，將儲存的Json做處理，分別儲存成JArray或JObject
* **/

/**
* 修改時間：2019/11/08
* 修改人員：高常昇
* 修改項目：
    * 1.排除Ede的dll.config
* 修改位置： 
    * 1.「getDDLConfigName」中，取出的Filename如果是Ede則跳過
* **/

/**
* 修改時間：2019/06/11
* 修改人員：高常昇
* 修改項目：
    * 修改Json資料，使之可以使用JArray
* 修改位置：
    * 前端網頁新增「gvitemJson_Item」用於新架構 -- 可以使用JArray
    * 修改及新增方法，為了符合新架構
    * 「btnSave_Click」、將最終結果儲存下來的方法
    * 「CreateJsonSub」、新增JArray的方法
    * 「CreateJsonItem」、新增JObject的方法
    * 「GetJsonItemDataTable」、將字串轉換成DataTable的方法
    * 「SaveCurrentJson」、儲存目前GridView的數值到ViewState
    * 「JsonSubDataBind」、將Json物件解析後呈現在前端網頁上
    * 「btnJSONTITLE_Click」、切換JSON頁面方法
    * 「btnJsonSubDelete_Click」、刪除JArray的方法
    * 「btnJsonSubNew_Click」、「JsonSubNewWithValue」、新增JArray的方法
    * 「btnJsonSubNew2_Click」、--延伸為新增連線資訊的方法
    * 「btnJsonItemDelete_Click」、刪除JObject的方法
    * 「btnJsonItemNew_Click」新增JObject的方法
* **/

/**
* 修改時間：2019/05/28
* 修改人員：陳緯榕
* 修改項目：
    * 當type是time時，不可為空
* 修改位置：
    * 「gvItemsDateTime_RowDataBound」中，當〈type == "time"〉時，判斷〈valueSplit.Length > 1〉才處理
* **/

/**
* 修改時間：2019/05/21
* 修改人員：高常昇
* 修改項目：更新Config管理頁面呈現
* 修改位置： 
    * 「前端網頁」內調整將「Repeater」改為「DataTable顯示」並且加入標籤控制
* **/

/**
* 修改時間：2019/04/30
* 修改人員：高常昇
* 修改項目：為了讓客戶編輯Dll.config內的Json資料
* 修改位置： 
    * 「前端網頁」內新增「hidNodeTYPE」、「JsonDiv」GridView、「btnAddArea」Div使用
    * 「getAllConfigNode」內新增Type屬性使用，判斷是否為type = json
    * 產生節點「rptConfigNode_ItemDataBound」內，新增判斷式，如果是json則建立GridView使用
    * 「btnAdd_Click」新增一筆GridView使用
    * 「btnSave_Click」新增對於Json類型的判斷及儲存，並在儲存完畢後刷新頁面
* **/

/// <summary>
/// 專案設定檔(*.dll.config)管理介面
/// </summary>
public partial class CDS_SCSHR_WebPages_DLLConfigManager : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("專案設定檔管理介面", Request.Url.AbsoluteUri);
            //// 綁定設定檔名稱
            ddlAllDDLConfig.DataSource = getDDLConfigName();
            ddlAllDDLConfig.DataTextField = "FILENAME_WEXT";
            ddlAllDDLConfig.DataValueField = "FILEPATH";
            ddlAllDDLConfig.DataBind();
            ddlAllDDLConfig_SelectedIndexChanged(ddlAllDDLConfig, null);
        }
    }
    #region 非控制項事件
    /// <summary>
    /// 找到所有的設定檔名稱及路徑
    /// </summary>
    /// <returns></returns>
    private DataTable getDDLConfigName()
    {
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("FILEPATH", typeof(string))); // 檔名
        dtSource.Columns.Add(new DataColumn("FILENAME_WEXT", typeof(string))); // 檔名去副檔名
        foreach (string _file in Directory.GetFiles(Path.Combine(Request.PhysicalApplicationPath, "bin"), "*.dll.config"))
        {
            string FILENAME = Path.GetFileName(_file).Split('.')[0];
            if (FILENAME == "Ede") continue;
            DataRow ndr = dtSource.NewRow();
            ndr["FILEPATH"] = _file;
            ndr["FILENAME_WEXT"] = FILENAME;
            dtSource.Rows.Add(ndr);
        }
        return dtSource;
    }
    /// <summary>
    /// 取得所有configuration/appSettings/add節點下的資料
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private void SetPage(string filepath)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(filepath);
        XmlNodeList nodeLists = doc.SelectNodes("configuration/appSettings/add");
        XmlNodeList nodeNameLists = doc.SelectNodes("configuration/appSettingsName/add");

        //將資料收集
        Dictionary<string, List<string>> NodeValue = new Dictionary<string, List<string>>();
        foreach (XmlNode _node in nodeLists)
        {
            if (!NodeValue.ContainsKey(_node.Attributes["key"].Value))
            {
                NodeValue.Add(_node.Attributes["key"].Value, new List<string>()
                {
                    _node.Attributes["key"].Value,
                    _node.Attributes["value"].Value,
                    _node.Attributes["key"].Value,
                    "text"
                });
            }
        }
        foreach (XmlNode _node in nodeNameLists)
        {
            if (NodeValue.ContainsKey(_node.Attributes["key"].Value))
            {
                NodeValue[_node.Attributes["key"].Value][2] = _node.Attributes["value"].Value; //NAME
                NodeValue[_node.Attributes["key"].Value][3] = _node.Attributes["type"] != null ? _node.Attributes["type"].Value : "text";
            }
        }

        //將資料作為類型分類
        Dictionary<string, List<List<string>>> dicNames = new Dictionary<string, List<List<string>>>();
        #region 客製資料添加
        List<string> RemoveStr = new List<string>();
        foreach (KeyValuePair<string, List<string>> _item in NodeValue)
        {
            string key = _item.Value[3];
            if (key == "datetime" || key == "date" || key == "time")
            {
                if (!dicNames.ContainsKey("datetime"))
                {
                    dicNames.Add("datetime", new List<List<string>>());
                }
                dicNames["datetime"].Add(_item.Value);
                RemoveStr.Add(_item.Key);
            }
        }
        foreach (string str in RemoveStr) if (NodeValue.ContainsKey(str)) NodeValue.Remove(str);
        #endregion
        foreach (KeyValuePair<string, List<string>> _item in NodeValue)
        {
            if (!dicNames.ContainsKey(_item.Value[3]))
            {
                dicNames.Add(_item.Value[3], new List<List<string>>());
            }
            dicNames[_item.Value[3]].Add(_item.Value);
        }
        foreach (KeyValuePair<string, List<List<string>>> item in dicNames) //所有類型資料
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add(new DataColumn("KEY", typeof(string))); // 節點ID
            dtSource.Columns.Add(new DataColumn("VALUE", typeof(string))); // 節點內容
            dtSource.Columns.Add(new DataColumn("NAME", typeof(string))); // 節點代稱
            dtSource.Columns.Add(new DataColumn("TYPE", typeof(string))); // 節點類型
            foreach (List<string> values in item.Value)
            {
                DataRow dr = dtSource.NewRow();
                dr["KEY"] = values[0];
                dr["VALUE"] = values[1];
                dr["NAME"] = values[2];
                dr["TYPE"] = values[3];
                dtSource.Rows.Add(dr);
            }
            ViewState[item.Key] = dtSource;
        }
        string[] AllType = new string[] { "text", "boolean", "json", "number", "datetime", "list" };
        foreach (string key in AllType) if (!dicNames.ContainsKey(key)) ViewState[key] = null;

        #region Text
        DataTable dtText = (DataTable)ViewState["text"];
        LabelText.Attributes["class"] = dtText != null ? "" : "hidden";
        gvItemsText.DataSource = dtText;
        gvItemsText.DataBind();
        #endregion
        #region Boolean
        DataTable dtBoolean = (DataTable)ViewState["boolean"];
        LabelBoolean.Attributes["class"] = dtBoolean != null ? "" : "hidden";
        gvItemsBoolean.DataSource = dtBoolean;
        gvItemsBoolean.DataBind();
        #endregion
        #region Json
        DataTable dtJson = (DataTable)ViewState["json"];
        LabelJson.Attributes["class"] = dtJson != null ? "" : "hidden";
        gvItemsJson.DataSource = dtJson;
        gvItemsJson.DataBind();
        ViewState["LastJson"] = "";
        if (dtJson != null) this.btnJSONTITLE_Click(gvItemsJson.Rows[0].FindControl("btnTITLE"), null);
        #endregion
        #region Number
        DataTable dtNumber = (DataTable)ViewState["number"];
        LabelNumber.Attributes["class"] = dtNumber != null ? "" : "hidden";
        gvItemsNumber.DataSource = dtNumber;
        gvItemsNumber.DataBind();
        #endregion
        #region DateTime
        DataTable dtDateTime = (DataTable)ViewState["datetime"];
        LabelDateTime.Attributes["class"] = dtDateTime != null ? "" : "hidden";
        gvItemsDateTime.DataSource = dtDateTime;
        gvItemsDateTime.DataBind();
        #endregion
        #region List
        DataTable dtList = (DataTable)ViewState["list"];
        LabelList.Attributes["class"] = dtList != null ? "" : "hidden";
        gvItemsList.DataSource = dtList;
        gvItemsList.DataBind();
        ViewState["LastList"] = "";
        if (dtList != null) this.btnLISTTITLE_Click(gvItemsList.Rows[0].FindControl("btnTITLE"), null);
        #endregion
    }

    #endregion 非控制項事件
    #region 控制項事件
    /// <summary>
    /// 選擇設定檔下拉事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAllDDLConfig_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList _ddlAllDDLConfig = (DropDownList)sender;
        lblMessage.Text = "";
        lblSaveMessage.Text = "";
        if (_ddlAllDDLConfig.Items.Count > 0)
        {
            lblConfigName.Text = _ddlAllDDLConfig.SelectedItem.Text;
            this.SetPage(_ddlAllDDLConfig.SelectedValue);
            //rptConfigNode.DataSource = getAllConfigNode(_ddlAllDDLConfig.SelectedValue);
            //rptConfigNode.DataBind();
        }
        else
        {
            lblMessage.Text = "找不到任何設定檔";
        }

    }
    ///// <summary>
    ///// 重複顯示所有節點
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void rptConfigNode_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    RepeaterItem item = e.Item;
    //    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
    //    {
    //        DataRowView row = item.DataItem as DataRowView;
    //        Label lblNodeName = item.FindControl("lblNodeName") as Label;
    //        HiddenField hidNodeID = item.FindControl("hidNodeID") as HiddenField;
    //        HiddenField hidNodeTYPE = item.FindControl("hidNodeTYPE") as HiddenField;
    //        TextBox txtNodeValue = item.FindControl("txtNodeValue") as TextBox;
    //        lblNodeName.Text = row["NAME"].ToString();
    //        hidNodeID.Value = row["KEY"].ToString();
    //        hidNodeTYPE.Value = row["TYPE"].ToString();
    //        txtNodeValue.Text = row["VALUE"].ToString();
    //        switch (hidNodeTYPE.Value)
    //        {
    //            case "json":
    //                (item.FindControl("TextDiv") as HtmlGenericControl).Attributes["class"] = "hidden"; ;
    //                (item.FindControl("JsonDiv") as HtmlGenericControl).Attributes["class"] = "DCContent";
    //                (item.FindControl("btnAddArea") as HtmlGenericControl).Attributes["class"] = "";
    //                GridView gvitem = item.FindControl("gvItems") as GridView;
    //                DataTable dt = new DataTable();
    //                dt.Columns.Add(new DataColumn("KEY", typeof(String))); // 請假代碼
    //                dt.Columns.Add(new DataColumn("VALUE", typeof(String))); // 假別名稱
    //                if (gvitem.Rows.Count <= 0)
    //                {
    //                    string[] jsonValue = txtNodeValue.Text.Split(':', ',');

    //                    if (jsonValue.Length > 1) //有JSON
    //                    {
    //                        for (int i = 0; i < jsonValue.Length; i++) jsonValue[i] = jsonValue[i].Replace("'", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty).Replace(@"""", string.Empty); //將特殊字元移除
    //                        for (int i = 0; i < jsonValue.Length / 2; i++)
    //                        {
    //                            DataRow dr = dt.NewRow();
    //                            dr["KEY"] = jsonValue[i * 2];
    //                            dr["VALUE"] = jsonValue[i * 2 + 1];
    //                            dt.Rows.Add(dr);
    //                        }
    //                    }
    //                    else //空值
    //                    {
    //                        DataRow dr = dt.NewRow();
    //                        dt.Rows.Add(dr);
    //                    }
    //                    ViewState["dt" + item.ItemIndex] = dt;
    //                    gvitem.DataSource = dt;
    //                    gvitem.DataBind();
    //                }
    //                break;
    //            case "text":
    //                (item.FindControl("btnAddArea") as HtmlGenericControl).Attributes["class"] = "hidden";
    //                break;
    //        }
    //    }
    //}

    /// <summary>
    /// 新增一筆GridView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Button _btn = (Button)sender;
        RepeaterItem item = _btn.NamingContainer as RepeaterItem;
        GridView gvitem = item.FindControl("gvItems") as GridView;
        DataTable dt = (DataTable)ViewState["dt" + item.ItemIndex]; //取得DataTable
        foreach (GridViewRow gvr in gvitem.Rows)
        {
            string key = (gvr.FindControl("txtKEY") as TextBox).Text.Trim();
            string value = (gvr.FindControl("txtVALUE") as TextBox).Text.Trim();
            dt.Rows[gvr.RowIndex]["KEY"] = key;
            dt.Rows[gvr.RowIndex]["VALUE"] = value;
        }
        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        ViewState["dt" + item.ItemIndex] = dt;
        gvitem.DataSource = dt;
        gvitem.DataBind();
    }

    /// <summary>
    /// 將GridView的值存入DataTable
    /// </summary>
    private void GridViewDataSaveValue(GridView gv, string VSIndex)
    {
        DataTable dt = (DataTable)ViewState[VSIndex];
        if (dt == null) return;
        foreach (GridViewRow gvr in gv.Rows)
        {
            dt.Rows[gvr.RowIndex]["VALUE"] = ((TextBox)gvr.FindControl("txtVALUE")).Text;
        }
        ViewState[VSIndex] = dt;
    }

    /// <summary>
    /// 儲存設定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(ddlAllDDLConfig.SelectedValue);
        XmlNodeList nodeLists = doc.SelectNodes("configuration/appSettings/add");

        this.GridViewDataSaveValue(gvItemsText, "text");
        this.GridViewDataSaveValue(gvItemsBoolean, "boolean");
        this.GridViewDataSaveValue(gvItemsNumber, "number");

        DataTable dtDateTime = (DataTable)ViewState["datetime"];
        if (dtDateTime != null)
        {
            foreach (GridViewRow gvr in gvItemsDateTime.Rows)
            {
                DataRow dr = dtDateTime.Rows[gvr.RowIndex];
                string value1 = ((TextBox)gvr.FindControl("txtVALUE1")).Text;
                string value2 = ((TextBox)gvr.FindControl("txtVALUE2")).Text;
                string value3 = ((TextBox)gvr.FindControl("txtVALUE3")).Text;
                string value4 = ((TextBox)gvr.FindControl("txtVALUE4")).Text;
                string value5 = ((TextBox)gvr.FindControl("txtVALUE5")).Text;
                switch (dr["TYPE"].ToString())
                {
                    case "date":
                        dr["VALUE"] = string.Format("{0}/{1}/{2}", value1, value2, value3);
                        break;
                    case "time":
                        dr["VALUE"] = string.Format("{0}:{1}", value4, value5);
                        break;
                    case "datetime":
                        dr["VALUE"] = string.Format("{0}/{1}/{2} {3}:{4}", value1, value2, value3, value4, value5);
                        break;
                }
            }
            ViewState["datetime"] = dtDateTime;
        }
        DataTable dtJson = (DataTable)ViewState["json"];
        if (dtJson != null)
        {
            this.SaveCurrentJson();
            foreach (DataRow dr in dtJson.Rows)
            {
                DataTable sub = (DataTable)ViewState["json_" + dr["KEY"].ToString()];
                if (sub == null) continue;
                string result = "";
                foreach (DataRow subDr in sub.Rows)
                {
                    //string SKEY = subDr["KEY"].ToString();
                    //if (string.IsNullOrEmpty(SKEY) || chkResult.Contains(SKEY)) continue; //如果KEY為空或是重複
                    if (!string.IsNullOrEmpty(result)) result += ",";
                    result += subDr["VALUE"];
                    //chkResult.Add(SKEY);
                }
                dr["VALUE"] = result;
                if (sub.Rows.Count > 0) if (sub.Rows[0]["REMARK"].ToString() == "Y") dr["VALUE"] = "[" + result + "]";
            }
            ViewState["json"] = dtJson;
        }
        DataTable dtList = (DataTable)ViewState["list"];
        if (dtList != null)
        {
            this.SaveCurrentList();
            foreach (DataRow dr in dtList.Rows)
            {
                DataTable sub = (DataTable)ViewState["list_" + dr["KEY"].ToString()];
                if (sub == null) continue;
                string result = "";
                foreach (DataRow subDr in sub.Rows)
                {
                    string SVALUE = subDr["VALUE"].ToString();
                    if (string.IsNullOrEmpty(SVALUE)) continue; //如果VALUE為空
                    if (!string.IsNullOrEmpty(result)) result += ",";
                    result += SVALUE;
                }
                dr["VALUE"] = result;
            }
            ViewState["list"] = dtList;
        }


        string[] AllViewState = new string[] { "text", "boolean", "json", "number", "datetime", "list" };
        foreach (string key in AllViewState)
        {
            DataTable dt = (DataTable)ViewState[key];
            if (dt == null) continue;
            foreach (DataRow dr in dt.Rows)
            {
                string k = dr["KEY"].ToString().Trim();
                foreach (XmlNode _node in nodeLists)
                {
                    if (_node.Attributes["key"].Value.Equals(k))
                    {
                        _node.Attributes["value"].Value = dr["VALUE"].ToString();
                        break;
                    }
                }
            }
        }

        //foreach (RepeaterItem _item in rptConfigNode.Items)
        //{
        //    Label lblNodeName = _item.FindControl("lblNodeName") as Label;
        //    HiddenField hidNodeID = _item.FindControl("hidNodeID") as HiddenField;
        //    HiddenField hidNodeTYPE = _item.FindControl("hidNodeTYPE") as HiddenField;
        //    TextBox txtNodeValue = _item.FindControl("txtNodeValue") as TextBox;
        //    string result = txtNodeValue.Text.Trim();
        //    switch (hidNodeTYPE.Value)
        //    {
        //        case "json":
        //            result = "";
        //            GridView gvitem = _item.FindControl("gvItems") as GridView;
        //            foreach (GridViewRow gvr in gvitem.Rows)
        //            {
        //                string key = (gvr.FindControl("txtKEY") as TextBox).Text.Trim();
        //                string value = (gvr.FindControl("txtVALUE") as TextBox).Text.Trim();
        //                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) continue;
        //                if (string.IsNullOrEmpty(result)) result = "{";
        //                else result += ",";
        //                result += string.Format("'{0}':'{1}'", key, value);
        //            }
        //            if (!string.IsNullOrEmpty(result)) result += "}";
        //            break;
        //    }
        //    foreach (XmlNode _node in nodeLists)
        //    {
        //        if (_node.Attributes["key"].Value.Equals(hidNodeID.Value.Trim()))
        //        {
        //            _node.Attributes["value"].Value = result;
        //            break;
        //        }
        //    }
        //}
        doc.Save(ddlAllDDLConfig.SelectedValue);
        ddlAllDDLConfig_SelectedIndexChanged(ddlAllDDLConfig, null);
        lblSaveMessage.Text = "已儲存成功";
    }
    #endregion 控制項事件

    /// <summary>
    /// 布林控制項
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvItemsBoolean_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblgvItem = (DataTable)ViewState["boolean"];
            bool isOpen = tblgvItem.Rows[gr.RowIndex]["VALUE"].ToString() == "Y";
            ((Button)gr.FindControl("btnChangeA")).CssClass = isOpen ? "selected" : "";
            ((Button)gr.FindControl("btnChangeB")).CssClass = isOpen ? "" : "selected";
        }
    }

    /// <summary>
    /// 布林按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBoolChange_Click(object sender, EventArgs e)
    {
        Button _btn = (Button)sender;
        GridViewRow gr = _btn.NamingContainer as GridViewRow;
        DataTable tblgvItem = (DataTable)ViewState["boolean"];
        tblgvItem.Rows[gr.RowIndex]["VALUE"] = tblgvItem.Rows[gr.RowIndex]["VALUE"].ToString() == "Y" ? "N" : "Y";
        ViewState["boolean"] = tblgvItem;
        gvItemsBoolean.DataSource = tblgvItem;
        gvItemsBoolean.DataBind();
        lblSaveMessage.Text = "";
    }

    /// <summary>
    /// DateTime控制項
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvItemsDateTime_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblgvItem = (DataTable)ViewState["datetime"];
            string type = tblgvItem.Rows[gr.RowIndex]["TYPE"].ToString();
            string value = tblgvItem.Rows[gr.RowIndex]["VALUE"].ToString();
            string[] YMDHM = new string[5];
            if (type != "time")
            {
                DateTime datetime = DateTime.MinValue;
                DateTime.TryParse(value, out datetime);
                YMDHM[0] = datetime.Year.ToString("0000");
                YMDHM[1] = datetime.Month.ToString("00");
                YMDHM[2] = datetime.Day.ToString("00");
                YMDHM[3] = datetime.Hour.ToString("00");
                YMDHM[4] = datetime.Minute.ToString("00");
            }
            else
            {
                string[] valueSplit = value.Split(':');
                if (valueSplit.Length > 1)
                {
                    YMDHM[3] = valueSplit[0];
                    YMDHM[4] = valueSplit[1];
                }
            }
            ((HtmlGenericControl)gr.FindControl("valueDiv")).Attributes["Class"] = type;
            ((TextBox)gr.FindControl("txtVALUE1")).Text = YMDHM[0];
            ((TextBox)gr.FindControl("txtVALUE2")).Text = YMDHM[1];
            ((TextBox)gr.FindControl("txtVALUE3")).Text = YMDHM[2];
            ((TextBox)gr.FindControl("txtVALUE4")).Text = YMDHM[3];
            ((TextBox)gr.FindControl("txtVALUE5")).Text = YMDHM[4];
        }
        lblSaveMessage.Text = "";
    }

    /// <summary>
    /// 建立Json資料用
    /// </summary>
    private DataTable CreateJsonSub()
    {
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("VALUE", typeof(string))); // 節點組合後內容
        dtSource.Columns.Add(new DataColumn("REMARK", typeof(string))); // 判斷是否為JArray
        return dtSource;
    }

    /// <summary>
    /// 建立Json物件用
    /// </summary>
    private DataTable CreateJsonItem()
    {
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("KEY", typeof(string))); // 節點ID
        dtSource.Columns.Add(new DataColumn("VALUE", typeof(string))); // 節點內容
        return dtSource;
    }

    /// <summary>
    /// 將Json格式字串轉換成DataTable
    /// </summary>
    /// <returns></returns>
    private DataTable GetJsonItemDataTable(string json)
    {
        DataTable dt = CreateJsonItem();
        try
        {
            JObject JItems = JsonConvert.DeserializeObject(json) as JObject;
            Dictionary<string, string> dictObj = JItems.ToObject<Dictionary<string, string>>();
            foreach (KeyValuePair<string, string> item in dictObj)
            {
                DataRow dr = dt.NewRow();
                dr["KEY"] = item.Key;
                dr["VALUE"] = item.Value;
                dt.Rows.Add(dr);
            }
        }
        catch { }
        if (dt.Rows.Count <= 0)
        {
            DataRow dr = dt.NewRow();
            dr["KEY"] = "";
            dr["VALUE"] = "";
            dt.Rows.Add(dr);
        }
        return dt;
    }

    /// <summary>
    /// 將目前的Json存下
    /// </summary>
    private void SaveCurrentJson()
    {
        string selectJson = ViewState["LastJson"].ToString();
        if (!string.IsNullOrEmpty(selectJson))
        {
            DataTable dt = (DataTable)ViewState[selectJson];
            for (int i = 0; i < gvItemsJson_Sub.Rows.Count; i++)
            {
                GridViewRow gvr = gvItemsJson_Sub.Rows[i];
                GridView jsonItemGv = ((GridView)gvr.FindControl("gvItemsJson_Item"));
                string result = "";
                List<string> chkKey = new List<string>();
                foreach (GridViewRow jigvr in jsonItemGv.Rows)
                {
                    string SKEY = ((TextBox)jigvr.FindControl("txtKEY")).Text;
                    if (chkKey.Contains(SKEY) || string.IsNullOrEmpty(SKEY)) continue; //如果有重複值或是空值，則排除

                    if (string.IsNullOrEmpty(result)) result = "{";
                    else result += ",";
                    result += string.Format("'{0}':'{1}'", SKEY, ((TextBox)jigvr.FindControl("txtVALUE")).Text);
                }
                if (!string.IsNullOrEmpty(result)) result += "}";
                DataRow dr = dt.Rows[i];
                dr["VALUE"] = result;
                DataTable aws = this.GetJsonItemDataTable(result);
                string jsonItemIndex = selectJson + i;
                ViewState[jsonItemIndex] = aws;
                ((HiddenField)gvr.FindControl("JsonItemIndex")).Value = jsonItemIndex;
                jsonItemGv.DataSource = aws;
                jsonItemGv.DataBind();
            }
            ViewState[selectJson] = dt;
        }
        lblSaveMessage.Text = "";
    }

    /// <summary>
    /// JsonSubDataBind方法
    /// </summary>
    private void JsonSubDataBind(DataTable dataTable, string currentJson)
    {
        ViewState[currentJson] = dataTable;
        gvItemsJson_Sub.DataSource = dataTable;
        gvItemsJson_Sub.DataBind();
        foreach (GridViewRow gvr in gvItemsJson_Sub.Rows)
        {
            GridView gv = (GridView)gvr.FindControl("gvItemsJson_Item");
            DataTable aws = this.GetJsonItemDataTable(dataTable.Rows[gvr.RowIndex]["VALUE"].ToString());
            ViewState[currentJson + gvr.RowIndex] = aws;
            gv.DataSource = aws;
            gv.DataBind();
        }
    }

    /// <summary>
    /// Json控制項_按鈕選擇Title
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnJSONTITLE_Click(object sender, EventArgs e)
    {
        Button _btn = (Button)sender;
        GridViewRow gr = _btn.NamingContainer as GridViewRow;
        DataTable tblgvItem = (DataTable)ViewState["json"];

        this.SaveCurrentJson();

        //前端顯示畫面
        foreach (GridViewRow gvr in gvItemsJson.Rows)
        {
            ((Button)gvr.FindControl("btnTITLE")).CssClass = "";
        }
        _btn.CssClass = "Selected";

        string key = tblgvItem.Rows[gr.RowIndex]["KEY"].ToString();
        string VSIndex = "json_" + key;
        string result = tblgvItem.Rows[gr.RowIndex]["VALUE"].ToString();
        string[] results = result.Split("},{", "}, {", "} ,{");
        //foreach (string r in results)
        //{
        //    DataRow subRow = dtSource.NewRow();
        //string str = r.Replace("'", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty).Replace(@"""", string.Empty).Replace("]", string.Empty).Replace("[", string.Empty); //將特殊字元移除
        //string[] jsonValue = str.Split(':', ',');
        //for (int i = 0; i < jsonValue.Length / 2; i++)
        //{
        //    DataRow dr = dtSource.NewRow();
        //    dr["KEY"] = jsonValue[i * 2];
        //    dr["VALUE"] = jsonValue[i * 2 + 1];
        //    dtSource.Rows.Add(dr);
        //}
        //dtSource.Rows.Add(subRow);
        //}
        string remark = result.IndexOf("[") >= 0 ? "Y" : "N";
        lblEditBtn.Visible = remark == "Y";
        btnJsonSubNew.Visible = remark == "Y";
        btnJsonSubNew2.Visible = remark == "Y";
        DataTable dtSource = (DataTable)ViewState[VSIndex];
        if (dtSource == null)
        {
            dtSource = CreateJsonSub();
            for (int i = 0; i < results.Length; i++)
            {
                string str = results[i];
                if (results.Length > 1)
                {
                    if (i == 0) str += "}";
                    else if (i == results.Length - 1) str = "{" + str;
                    else str = "{" + str + "}";
                }
                DataRow dr = dtSource.NewRow();
                dr["REMARK"] = remark;
                dr["VALUE"] = str.Replace("[", string.Empty).Replace("]", string.Empty);
                dtSource.Rows.Add(dr);
            }
        }
        ViewState["LastJson"] = VSIndex;
        this.JsonSubDataBind(dtSource, VSIndex);
    }

    /// <summary>
    /// 刪除Json資料
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnJsonSubDelete_Click(object sender, EventArgs e)
    {
        Button _btn = (Button)sender;
        GridViewRow gr = _btn.NamingContainer as GridViewRow;
        string current = ViewState["LastJson"].ToString();
        DataTable tblgvItem = (DataTable)ViewState[current];
        this.SaveCurrentJson();
        tblgvItem.Rows.RemoveAt(gr.RowIndex);
        tblgvItem.AcceptChanges();
        this.JsonSubDataBind(tblgvItem, current);
    }

    /// <summary>
    /// 新增一筆Json資料
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnJsonSubNew_Click(object sender, EventArgs e)
    {
        this.JsonSubNewWithValue();
    }

    /// <summary>
    /// 新增一筆連線資訊
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnJsonSubNew2_Click(object sender, EventArgs e)
    {
        this.SaveCurrentJson();
        string current = ViewState["LastJson"].ToString();
        DataTable tblgvItem = (DataTable)ViewState[current];
        DataRow dr = tblgvItem.Rows[tblgvItem.Rows.Count - 1];

        this.JsonSubNewWithValue(dr["VALUE"].ToString());
    }

    /// <summary>
    /// 建立一筆Json資料，並填入Value
    /// </summary>
    /// <param name="value"></param>
    private void JsonSubNewWithValue(string value = "")
    {
        this.SaveCurrentJson();
        string current = ViewState["LastJson"].ToString();
        DataTable tblgvItem = (DataTable)ViewState[current];
        DataRow dr = tblgvItem.NewRow();
        dr["VALUE"] = value;
        tblgvItem.Rows.Add(dr);
        this.JsonSubDataBind(tblgvItem, current);
    }

    /// <summary>
    /// 刪除一筆Json物件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnJsonItemDelete_Click(object sender, EventArgs e)
    {
        Button _btn = (Button)sender;
        GridViewRow gr = _btn.NamingContainer as GridViewRow;
        ((TextBox)gr.FindControl("txtKEY")).Text = ""; //將key值設定為空值 -- 自然會刪除
        this.SaveCurrentJson();
    }

    /// <summary>
    /// 新增一筆Json物件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnJsonItemNew_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        this.SaveCurrentJson();
        string current = ViewState["LastJson"].ToString();
        GridViewRow gr = btn.NamingContainer as GridViewRow;
        DataTable tblgvItem = (DataTable)ViewState[current];
        string JsonItemIndex = ((HiddenField)gr.FindControl("JsonItemIndex")).Value;
        DataTable dt = (DataTable)ViewState[JsonItemIndex];
        if (dt == null) return;

        dt.Rows.Add(dt.NewRow());
        GridView gv = (GridView)gr.FindControl("gvItemsJson_Item");
        ViewState[JsonItemIndex] = dt;
        gv.DataSource = dt;
        gv.DataBind();
    }

    /// <summary>
    /// 建立List資料用
    /// </summary>
    private DataTable CreateListSub()
    {
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("VALUE", typeof(string))); // 節點內容
        return dtSource;
    }

    /// <summary>
    /// 將目前的List存下
    /// </summary>
    private void SaveCurrentList()
    {
        string selectList = ViewState["LastList"].ToString();
        if (!string.IsNullOrEmpty(selectList))
        {
            DataTable dt = (DataTable)ViewState[selectList];
            for (int i = 0; i < gvItemsList_Sub.Rows.Count; i++)
            {
                GridViewRow gvr = gvItemsList_Sub.Rows[i];
                DataRow dr = dt.Rows[i];
                dr["VALUE"] = ((TextBox)gvr.FindControl("txtVALUE")).Text;
            }
            ViewState[selectList] = dt;
        }
        lblSaveMessage.Text = "";
    }

    /// <summary>
    /// List控制項_按鈕選擇Title
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLISTTITLE_Click(object sender, EventArgs e)
    {
        Button _btn = (Button)sender;
        GridViewRow gr = _btn.NamingContainer as GridViewRow;
        DataTable tblgvItem = (DataTable)ViewState["list"];

        this.SaveCurrentList();

        //前端顯示畫面
        foreach (GridViewRow gvr in gvItemsList.Rows)
        {
            ((Button)gvr.FindControl("btnTITLE")).CssClass = "";
        }
        _btn.CssClass = "Selected";


        string key = tblgvItem.Rows[gr.RowIndex]["KEY"].ToString();
        string VSIndex = "list_" + key;
        DataTable dtSource = (DataTable)ViewState[VSIndex];
        if (dtSource == null)
        {
            //對應DataBind
            string value = tblgvItem.Rows[gr.RowIndex]["VALUE"].ToString();
            dtSource = this.CreateListSub();
            string[] listValue = value.Split(',');
            for (int i = 0; i < listValue.Length; i++)
            {
                DataRow dr = dtSource.NewRow();
                dr["VALUE"] = listValue[i];
                dtSource.Rows.Add(dr);
            }
            ViewState[VSIndex] = dtSource;
        }
        ViewState["LastList"] = VSIndex;
        gvItemsList_Sub.DataSource = dtSource;
        gvItemsList_Sub.DataBind();
    }

    /// <summary>
    /// 刪除List資料
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnListDelete_Click(object sender, EventArgs e)
    {
        Button _btn = (Button)sender;
        GridViewRow gr = _btn.NamingContainer as GridViewRow;
        string current = ViewState["LastList"].ToString();
        DataTable tblgvItem = (DataTable)ViewState[current];
        this.SaveCurrentList();
        tblgvItem.Rows.RemoveAt(gr.RowIndex);
        tblgvItem.AcceptChanges();
        ViewState[current] = tblgvItem;
        gvItemsList_Sub.DataSource = tblgvItem;
        gvItemsList_Sub.DataBind();
    }

    /// <summary>
    /// 新增一筆List資料
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnListNew_Click(object sender, EventArgs e)
    {
        this.SaveCurrentList();
        string current = ViewState["LastList"].ToString();
        DataTable tblgvItem = (DataTable)ViewState[current];
        DataRow dr = tblgvItem.NewRow();
        tblgvItem.Rows.Add(dr);
        ViewState[current] = tblgvItem;
        gvItemsList_Sub.DataSource = tblgvItem;
        gvItemsList_Sub.DataBind();
    }
}
