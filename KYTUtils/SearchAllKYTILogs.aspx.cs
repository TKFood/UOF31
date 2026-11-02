using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using UOFAssist.WKF;

// TODO: 2020/03/17 內容的部分最好還可以轉換(base64轉文字、json和XML可讀性提高(換行)、URL Decode)

/**
* 修改時間：2020/04/15
* 修改人員：陳緯榕
* 修改項目：
    * LOG等級「DETAILINFO」無法顯示
* 發生原因：
    * KYTLog寫入的是「[DETAILINFO ]」，而不是「DETAILINFO」
* 修改位置：
    * 「btnRead_Click」中，設定正規化的規則時，〈[A-Za-z\s]{4,10}〉改為〈[A-Za-z\s]{4,11}〉取最大11碼
* **/

/**
* 修改時間：2020/03/19
* 修改人員：陳緯榕
* 修改項目：
    * 明細項改用分頁處理；因為當讀取的檔案太大，會出現「 已經從要求中讀取最大數目的表單、查詢字串或張貼檔案項目。若要變更允許的要求集合數上限的目前值2000，請變更 "aspnet:MaxHttpCollectionKeys" 設定」的錯誤
    * 要有重新讀取下拉選單的功能
    * 重新讀取檔案時，清空關鍵字
* 發生原因：
    * 集合多到超出範圍，不去改設定檔的理由是，根本不知道LOG最多會到多少
    * 新規格
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈gvLogs〉新增屬性〈AllowPaging="True"〉開啟分頁功能、〈PageSize="20"〉每頁N筆、〈OnPageIndexChanging〉點擊頁碼功能、〈PagerSettings：Position="TopAndBottom"〉頁碼連結顯示在最頂和最底、〈PagerStyle〉頁碼的元素設定、〈PagerTemplate〉自定義頁碼功能
    * 新增方法「gvLogs_PageIndexChanging(object sender, GridViewPageEventArgs e)」明細項分頁點選頁碼後觸發事件
    * 「btnFliter_Click」中，新增〈ViewState["gbLogsFliter"]〉，用以儲存查詢到的資料(因為有分頁)
    * 「btnRead_Click」中，讀取檔案前，清除先前的ViewState〈ViewState["gbLogs"]〉、〈ViewState["gbLogsFliter"] 〉
    * 「btnRead_Click」中，明細載入完成後，清除剛才使用的集合〈lsLogInfo.Clear()〉，用來釋放資源
    * ******
    * 新增方法「btnRefresh_Click(object sender, EventArgs e)」按下重整檔案清單按鈕觸發事件
    * *******
    * 「btnRead_Click」中，清除〈txtFliter〉
* **/

/// <summary>
/// 冠永騰專用LOG查詢(可過濾資料)
/// </summary>
public partial class CDS_KYTUtils_SearchAllKYTILogs : BasePage
{
    private static string appConfigPath = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("冠永騰專用LOG查詢", Request.Url.AbsoluteUri);
            #region 綁定設定檔名稱
            ddlAllDDLConfig.DataSource = getDDLConfigName();
            ddlAllDDLConfig.DataTextField = "FILENAME_WEXT";
            ddlAllDDLConfig.DataValueField = "FILEPATH";
            ddlAllDDLConfig.DataBind();
            ddlAllDDLConfig_SelectedIndexChanged(ddlAllDDLConfig, null);
            #endregion 綁定設定檔名稱
            divFliter.Visible = false; // 隱藏過濾區塊
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
            string fileName = Path.GetFileName(_file).Split('.')[0];
            if (fileName.ToUpper() != "EDE")
            {
                DataRow ndr = dtSource.NewRow();
                ndr["FILEPATH"] = _file;
                ndr["FILENAME_WEXT"] = fileName;
                dtSource.Rows.Add(ndr);
            }
        }
        return dtSource;
    }

    /// <summary>
    /// 取得資料夾內的LOG檔案名稱
    /// </summary>
    /// <param name="floderPath"></param>
    /// <returns></returns>
    private DataTable getLogFileName(string floderPath)
    {
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("FILEPATH", typeof(string))); // 檔名
        dtSource.Columns.Add(new DataColumn("FILENAME_WEXT", typeof(string))); // 檔名去副檔名
        DataRow ondr = dtSource.NewRow();
        ondr["FILEPATH"] = "";
        ondr["FILENAME_WEXT"] = "===請選擇===";
        dtSource.Rows.Add(ondr);
        if (Directory.Exists(floderPath))
        {
            foreach (string _file in Directory.GetFiles(floderPath, "*.log"))
            {
                DataRow ndr = dtSource.NewRow();
                ndr["FILEPATH"] = _file;
                ndr["FILENAME_WEXT"] = Path.GetFileName(_file).Split('.')[0];
                dtSource.Rows.Add(ndr);
            }
        }

        return dtSource;
    }
    /// <summary>
    /// 將LogInfo型別轉換為DataTable型別
    /// </summary>
    /// <param name="logInfos"></param>
    /// <returns></returns>
    private DataTable convertLogInfoToDataTable(LogInfo[] logInfos)
    {
        DataTable dtAllLogs = new DataTable();
        dtAllLogs.Columns.Add(new DataColumn("RowNo", typeof(int))); // 序列號
        dtAllLogs.Columns.Add(new DataColumn("LogTime", typeof(DateTime))); // Log時間
        dtAllLogs.Columns.Add(new DataColumn("LogLevel", typeof(string))); // LOG等級
        dtAllLogs.Columns.Add(new DataColumn("FileName", typeof(string))); // 檔案名稱
        dtAllLogs.Columns.Add(new DataColumn("NameSpace", typeof(string))); // namespace
        dtAllLogs.Columns.Add(new DataColumn("MethodName", typeof(string))); // 方法名稱
        dtAllLogs.Columns.Add(new DataColumn("LineNumber", typeof(int))); // 行號
        dtAllLogs.Columns.Add(new DataColumn("Values", typeof(string))); // 內容
        int i = 1;
        foreach (LogInfo _li in logInfos)
        {
            DataRow ndr = dtAllLogs.NewRow();
            ndr["RowNo"] = i++;
            ndr["LogTime"] = _li.LogTime;
            ndr["LogLevel"] = _li.LogLevel;
            ndr["FileName"] = _li.FileName;
            ndr["NameSpace"] = _li.NameSpace;
            ndr["MethodName"] = _li.MethodName;
            ndr["LineNumber"] = _li.LineNumber;
            ndr["Values"] = _li.Values;
            dtAllLogs.Rows.Add(ndr);
        }
        return dtAllLogs;
    }
    /// <summary>
    /// 建立過濾區塊的下拉選單資料
    /// </summary>
    /// <param name="dtSource"></param>
    /// <param name="columnName"></param>
    /// <returns></returns>
    private DataTable createDDLDataTable(DataTable dtSource, string columnName)
    {
        DataTable dtReturn = new DataTable();
        DataColumn[] dcs = new DataColumn[1];
        dcs[0] = new DataColumn(columnName, typeof(string));
        dtReturn.Columns.Add(dcs[0]);
        dtReturn.PrimaryKey = dcs; // 設定資料表的PK
        DataRow ndr = dtReturn.NewRow();
        ndr[columnName] = "===無過濾===";
        dtReturn.Rows.Add(ndr);

        foreach (DataRow dr in dtSource.Rows)
        {
            if (!dtReturn.Rows.Contains(dr[columnName].ToString())) // ※必須要有PK才能使用
            {
                DataRow _ndr = dtReturn.NewRow();
                _ndr[columnName] = dr[columnName].ToString();
                dtReturn.Rows.Add(_ndr);
            }
        }

        return dtReturn;
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
        if (_ddlAllDDLConfig.Items.Count > 0)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_ddlAllDDLConfig.SelectedValue);
            XmlNode node = doc.SelectSingleNode("//configuration/appSettings/add[@key='LOG路徑']");
            appConfigPath = node != null ? node.Attributes["value"].Value : "";
        }
        else
        {
            lblMessage.Text = "找不到任何設定檔";
        }

    }
    /// <summary>
    /// 選擇LOG日期變更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdpLocation_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(appConfigPath))
        {
            if (rdpLocation.SelectedDate.HasValue) // 當有選擇日期
            {
                // 設定資料夾內檔名下拉式選單
                ddlAllFiles.DataSource = getLogFileName(Path.Combine(appConfigPath, rdpLocation.SelectedDate.Value.ToString("yyyyMMdd")));
                ddlAllFiles.DataTextField = "FILENAME_WEXT";
                ddlAllFiles.DataValueField = "FILEPATH";
                ddlAllFiles.DataBind();
            }
        }

    }

    List<LogInfo> lsLogInfo = new List<LogInfo>(); // 半全域變數，用來收納所有的「真．每筆LOG資料」
    /// <summary>
    /// 按下讀取檔案後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRead_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlAllFiles.SelectedValue)) // 當有選擇LOG檔案時
        {
            if (File.Exists(ddlAllFiles.SelectedValue)) // LOG檔案真實存在於資料夾中
            {
                ViewState["gbLogsFliter"] = null;
                ViewState["gbLogs"] = null;
                //txtLogMsg.Text = File.ReadAllText(ddlAllFiles.SelectedValue);
                LogInfo li = null;
                foreach (string oneLine in File.ReadAllLines(ddlAllFiles.SelectedValue)) // 讀取每一行的內容
                {
                    string _value = oneLine;
                    //2020\/03\/13 10:[0-5][0-9]:[0-5][0-9]\[[A-Za-z\s]{4,10}\]
                    //\[\d{1,5}\]
                    //\[[A-Za-z]{1,}\]
                    // \[[A-Za-z\_\.]{1,}\]
                    string pattan = "";
                    string sDateTime = string.Format(@"^{0}\/{1}\/{2} {3}",
                        rdpLocation.SelectedDate.Value.ToString("yyyy"),
                        rdpLocation.SelectedDate.Value.ToString("MM"),
                        rdpLocation.SelectedDate.Value.ToString("dd"),
                        ddlAllFiles.SelectedItem.Text);
                    pattan = sDateTime + @":[0-5][0-9]:[0-5][0-9]\[[A-Za-z\s]{4,11}\]|" + sDateTime + @":[0-5][0-9]:[0-5][0-9]"; // 建立尋找「開頭為『被選定的日期和小時』或者包含『[INFO ]之類的標籤』」的關鍵字

                    //string pattan = @"\[[A-Za-z\\_\.]{1,}\]";
                    System.Text.RegularExpressions.Match ma = System.Text.RegularExpressions.Regex.Match(_value, pattan); // 使用正規表示式尋找
                    if (ma.Length > 0) // 如果有找到，表示這是每一筆資料的第一行
                    {
                        string[] header = _value.Substring(ma.Index, ma.Length).Split('['); // 切割時間和[標籤]
                        li = new LogInfo(); // 建立LogInfo型別，用來儲存所有資料
                        lsLogInfo.Add(li); // 將這筆資料收納起來
                        li.LogTime = Convert.ToDateTime(header[0]); // 將時間轉成DateTime型別後存入
                        li.LogLevel = header.Length > 1 ? header[1].Split(']')[0].Trim().ToUpper() : "無等級"; // 存入標籤
                        _value = _value.Substring(ma.Length, _value.Length - ma.Length); // 切割剩下的內文
                        pattan = @"\[[A-Za-z\\_\.]{1,}\]|\[[A-Za-z]{1,}\]|\[\d{1,5}\]"; // 建立尋找「[當前方法之特定階層類別(父方)所屬的檔案名稱][當前方法之特定階層類別(父方)的Full class Name][當前方法之特定階層類別(父方)的Function Name][當前方法之特定階層類別(父方)的行號]」
                        System.Text.RegularExpressions.MatchCollection mas = System.Text.RegularExpressions.Regex.Matches(_value, pattan); // 使用尋找「多筆」資料的方式
                        if (mas.Count == 4) // 同時找到四個才是正確的
                        {
                            li.FileName = mas[0].Value.Split('[')[1].Split(']')[0]; // 切割出檔案名稱並存入
                            li.NameSpace = mas[1].Value.Split('[')[1].Split(']')[0]; // 切割出Namespace並存入
                            li.MethodName = mas[2].Value.Split('[')[1].Split(']')[0]; // 切割出方法並存入
                            int linenumber = -1;
                            int.TryParse(mas[3].Value.Split('[')[1].Split(']')[0], out linenumber); // 轉換行號
                            li.LineNumber = linenumber; // 存入行號
                            _value = _value.Substring(mas[3].Index + mas[3].Length, _value.Length - mas[3].Index - mas[3].Length); // 切割剩下的內文
                        }
                        else
                        {
                            #region 表示該筆是舊的
                            li.FileName = "無紀錄";
                            li.NameSpace = "無紀錄";
                            li.MethodName = "無紀錄";
                            li.LineNumber = -1;
                            #endregion 表示該筆是舊的
                        }
                        li.setValues(_value.Trim()); // 將現在的內文填入
                    }
                    else
                    {
                        if (li != null) // 只有檔案內文的第一筆才會是null
                            li.setValues(_value.Trim()); // 表示檔案讀取到下一行後，這一行的資料還是屬於「這一筆」資料
                    }

                }

                ViewState["gbLogs"] = convertLogInfoToDataTable(lsLogInfo.ToArray()); // 將剛才收納的LogInfo轉換為DataTable，並存在網頁上(避免之後的搜尋過濾功能又要使用讀取檔案的方式)
                lblSearchCount.Text = (ViewState["gbLogs"] as DataTable).Rows.Count.ToString(); // 設定找到幾筆資料
                gvLogs.DataSource = ViewState["gbLogs"] as DataTable; // 綁定明細項
                gvLogs.DataBind();

                divFliter.Visible = true; // 顯示過濾區塊

                ddlLogLevel.DataSource = createDDLDataTable(ViewState["gbLogs"] as DataTable, "LogLevel"); // 設定這個檔案所找到的所有不重複的LOG等級
                ddlLogLevel.DataTextField = "LogLevel";
                ddlLogLevel.DataValueField = "LogLevel";
                ddlLogLevel.DataBind();

                ddlFileName.DataSource = createDDLDataTable(ViewState["gbLogs"] as DataTable, "FileName");// 設定這個檔案所找到的所有不重複的檔案名稱
                ddlFileName.DataTextField = "FileName";
                ddlFileName.DataValueField = "FileName";
                ddlFileName.DataBind();

                ddlNamespace.DataSource = createDDLDataTable(ViewState["gbLogs"] as DataTable, "Namespace"); // 設定這個檔案所找到的所有不重複的NameSpace
                ddlNamespace.DataTextField = "Namespace";
                ddlNamespace.DataValueField = "Namespace";
                ddlNamespace.DataBind();

                ddlMethodName.DataSource = createDDLDataTable(ViewState["gbLogs"] as DataTable, "MethodName"); // 設定這個檔案所找到的所有不重複的方法名稱
                ddlMethodName.DataTextField = "MethodName";
                ddlMethodName.DataValueField = "MethodName";
                ddlMethodName.DataBind();

                // 設定搜尋過濾用的時間區間
                txtStartTime.Text = string.Format(@"{0} {1}:00:00", rdpLocation.SelectedDate.Value.ToString("yyyy/MM/dd"), ddlAllFiles.SelectedItem.Text);
                txtEndTime.Text = string.Format(@"{0} {1}:59:59", rdpLocation.SelectedDate.Value.ToString("yyyy/MM/dd"), ddlAllFiles.SelectedItem.Text);

                txtFliter.Text = "";

                lsLogInfo.Clear();
            }
        }
    }

    /// <summary>
    /// 按下過濾搜尋按鈕後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnFliter_Click(object sender, EventArgs e)
    {
        string fliter = ""; // 要在DataTable用Select過濾用的文字

        try
        {
            if (!string.IsNullOrWhiteSpace(txtStartTime.Text) && // 如果有起始時間
           !string.IsNullOrWhiteSpace(txtEndTime.Text)) // 如果有結束時間
            {
                // 如果起始時間及結束時間是合法的時間格式，並且時間在目前所選的時間範圍外，將時間設定回來
                if (!(DateTime.Parse(txtStartTime.Text) >= Convert.ToDateTime(string.Format(@"{0} {1}:00:00", rdpLocation.SelectedDate.Value.ToString("yyyy/MM/dd"), ddlAllFiles.SelectedItem.Text)) &&
                    DateTime.Parse(txtEndTime.Text) <= Convert.ToDateTime(string.Format(@"{0} {1}:59:59", rdpLocation.SelectedDate.Value.ToString("yyyy/MM/dd"), ddlAllFiles.SelectedItem.Text))))
                {
                    txtStartTime.Text = string.Format(@"{0} {1}:00:00", rdpLocation.SelectedDate.Value.ToString("yyyy/MM/dd"), ddlAllFiles.SelectedItem.Text);
                    txtEndTime.Text = string.Format(@"{0} {1}:59:59", rdpLocation.SelectedDate.Value.ToString("yyyy/MM/dd"), ddlAllFiles.SelectedItem.Text);
                }
                if (!string.IsNullOrEmpty(fliter)) fliter += " AND "; // 如果前面已經有設定過「過濾用文字」，就需要用AND來連接文字
                fliter += string.Format(@"(#{0}# <= LogTime AND LogTime <= #{1}#)", txtStartTime.Text, txtEndTime.Text); // 當LogTime介於起始和結束時間
            }
        }
        catch
        {
            // 出錯就表示時間格式有問題，設定回原本的最大時間區間
            txtStartTime.Text = string.Format(@"{0} {1}:00:00", rdpLocation.SelectedDate.Value.ToString("yyyy/MM/dd"), ddlAllFiles.SelectedItem.Text);
            txtEndTime.Text = string.Format(@"{0} {1}:59:59", rdpLocation.SelectedDate.Value.ToString("yyyy/MM/dd"), ddlAllFiles.SelectedItem.Text);
        }


        if (ddlLogLevel.SelectedIndex != 0) // 如果有選擇LOG等級
        {
            if (!string.IsNullOrEmpty(fliter)) fliter += " AND "; // 如果前面已經有設定過「過濾用文字」，就需要用AND來連接文字
            fliter += string.Format(@"LogLevel = '{0}'", ddlLogLevel.SelectedValue);
        }
        if (ddlFileName.SelectedIndex != 0)
        {
            if (!string.IsNullOrEmpty(fliter)) fliter += " AND "; // 如果前面已經有設定過「過濾用文字」，就需要用AND來連接文字
            fliter += string.Format(@"FileName = '{0}'", ddlFileName.SelectedValue);
        }
        if (ddlNamespace.SelectedIndex != 0)
        {
            if (!string.IsNullOrEmpty(fliter)) fliter += " AND "; // 如果前面已經有設定過「過濾用文字」，就需要用AND來連接文字
            fliter += string.Format(@"Namespace = '{0}'", ddlNamespace.SelectedValue);
        }
        if (ddlMethodName.SelectedIndex != 0)
        {
            if (!string.IsNullOrEmpty(fliter)) fliter += " AND "; // 如果前面已經有設定過「過濾用文字」，就需要用AND來連接文字
            fliter += string.Format(@"MethodName = '{0}'", ddlMethodName.SelectedValue);
        }
        // 為了不修改到最原始的資料，並且是因為DataTable比DataRow陣列好操作
        DataTable dtSource = new DataTable();

        DataRow[] drSources = (ViewState["gbLogs"] as DataTable).Select(fliter); // 搜尋原始資料
        foreach (DataRow dr in drSources) // 尋覽查詢到的資料
        {
            DataRow ndr = dtSource.NewRow(); // 並將資料塞入dtSource的DataTable中
            foreach (DataColumn dc in (ViewState["gbLogs"] as DataTable).Columns)
            {
                if (!dtSource.Columns.Contains(dc.ColumnName))
                    dtSource.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                ndr[dc.ColumnName] = dr[dc.ColumnName];
            }
            dtSource.Rows.Add(ndr);
        }

        DataTable dtResult = new DataTable(); // 建立結果用的DataTable
        //foreach (DataColumn dc in dtSource.Columns)
        //{
        //    dtResult.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
        //}

        if (!string.IsNullOrEmpty(txtFliter.Text.Trim())) // 如果有關鍵字
        {
            Dictionary<int, DataRow> lsDR = new Dictionary<int, DataRow>(); // 收納不符合條件的資料
            foreach (string _fliter in txtFliter.Text.Trim().Split('+')) // 如果有用「+」連結，表示要使用複合式查詢，要同時擁有所有條件的資料才可以
            {
                foreach (DataRow dr in dtSource.Rows) // 巡覽所有的資料
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(dr["Values"].ToString(), _fliter)) // 找到不符合條件的資料
                    {
                        if (!lsDR.ContainsKey((int)dr["RowNo"])) // 如果這個RowNo還沒被收納過
                            lsDR.Add((int)dr["RowNo"], dr); // 收納該筆資料
                    }
                }
            }
            foreach (DataRow dr in lsDR.Values)
            {
                dtSource.Rows.Remove(dr); // 刪除沒找到的
            }

            //foreach (DataRow dr in dtSource.Rows) // 巡覽被查詢出來剩下的資料
            //{
            //    DataRow ndr = dtResult.NewRow(); // 將這些資料建立為結果
            //    foreach (DataColumn dc in dtSource.Columns)
            //    {
            //        if (!dtResult.Columns.Contains(dc.ColumnName))
            //            dtResult.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
            //        ndr[dc.ColumnName] = dr[dc.ColumnName];
            //    }
            //    dtResult.Rows.Add(ndr);
            //}
        }
        else
        {
            //foreach (DataRow dr in dtSource.Rows)
            //{
            //    DataRow ndr = dtResult.NewRow();
            //    foreach (DataColumn dc in dtSource.Columns)
            //    {
            //        ndr[dc.ColumnName] = dr[dc.ColumnName];
            //    }
            //    dtResult.Rows.Add(ndr);
            //}
        }

        for (int i = 0; i < dtSource.Rows.Count; i++) // 巡覽剩下的資料
        {
            DataRow dr = dtSource.Rows[i];
            dr["RowNo"] = i + 1; // 重新定義每筆資料的序列號
        }
        lblSearchCount.Text = dtSource.Rows.Count.ToString(); // 設定這次查詢到的資料筆數
        ViewState["gbLogsFliter"] = dtSource;
        gvLogs.DataSource = ViewState["gbLogsFliter"] as DataTable; // 綁定明細項
        gvLogs.DataBind();
    }

    /// <summary>
    /// 明細項的每筆資料的反應
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLogs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        gr.Attributes.Add("onmouseover", "this.bkc=this.style.backgroundColor;this.style.backgroundColor='#ccffff'"); // 設定每筆資料在滑鼠移動進去時的背景顏色
        gr.Attributes.Add("onmouseout", "this.style.backgroundColor=this.bkc"); // 設定每筆資料在滑鼠移出時的背景色
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataRowView row = (DataRowView)e.Row.DataItem;

            Label lblRowNo = gr.FindControl("lblRowNo") as Label; // 序列號
            Label lblDateTime = gr.FindControl("lblDateTime") as Label; // 時間
            Label lblLevel = gr.FindControl("lblLevel") as Label; // LOG等級
            Label lblFileName = gr.FindControl("lblFileName") as Label; // 檔案名稱
            Label lblNameSpace = gr.FindControl("lblNameSpace") as Label; // NameSpace
            Label lblMethodName = gr.FindControl("lblMethodName") as Label; // 方法名稱
            Label lblLineNumber = gr.FindControl("lblLineNumber") as Label; // 行號
            TextBox txtValues = gr.FindControl("txtValues") as TextBox; // 內容

            Color _color = Color.Black;
            switch (row["LogLevel"].ToString().ToUpper()) // 不同的LOG等級顯示不同的文字顏色
            {
                case "ERROR":
                    _color = Color.Red;
                    break;
                case "DEBUG":
                    _color = Color.Yellow;
                    break;
                case "INFO":
                    _color = Color.Blue;
                    break;
                case "DETAILINFO":
                    _color = Color.Green;
                    break;
                case "MAXINFO":
                    _color = Color.Gray;
                    break;
                case "無等級":
                    _color = Color.Brown;
                    break;
            }
            lblRowNo.Text = row["RowNo"].ToString();
            lblDateTime.Text = ((DateTime)row["LogTime"]).ToString("yyyy/MM/dd HH:mm:ss");
            lblLevel.Text = row["LogLevel"].ToString();
            lblLevel.ForeColor = _color;
            lblFileName.Text = row["FileName"].ToString();
            lblNameSpace.Text = row["NameSpace"].ToString();
            lblMethodName.Text = row["MethodName"].ToString();
            lblLineNumber.Text = row["LineNumber"].ToString();
            txtValues.Text = row["Values"].ToString();
        }
    }
    /// <summary>
    /// 按下重整檔案清單按鈕觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        ddlAllDDLConfig_SelectedIndexChanged(ddlAllDDLConfig, null);
        rdpLocation_SelectedDateChanged(rdpLocation, null);
    }

    /// <summary>
    /// 明細項分頁點選頁碼後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvLogs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dtSource = ViewState["gbLogsFliter"] as DataTable;
        dtSource = dtSource == null ? ViewState["gbLogs"] as DataTable : dtSource;
        gvLogs.PageIndex = e.NewPageIndex;
        gvLogs.DataSource = dtSource;
        gvLogs.DataBind();
    }
    #endregion 控制項事件
}

/// <summary>
/// 收納LOG內每筆資料用的型別
/// </summary>
public class LogInfo
{
    private string _LogTime;
    /// <summary>
    /// 每筆LOG資料被記錄的時間
    /// </summary>
    public DateTime LogTime
    {
        set { _LogTime = ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss"); }
        get
        {
            DateTime _dtLogTime = DateTime.MinValue;
            DateTime.TryParse(_LogTime, out _dtLogTime);
            return _dtLogTime;
        }
    }
    /// <summary>
    /// 每筆LOG資料的LOG等級
    /// </summary>
    public string LogLevel { get; set; }
    /// <summary>
    /// 每筆LOG資料的檔案名稱
    /// </summary>
    public string FileName { get; set; }
    /// <summary>
    /// 每筆LOG資料的NameSpace
    /// </summary>
    public string NameSpace { get; set; }
    /// <summary>
    /// 每筆LOG資料的方法
    /// </summary>
    public string MethodName { get; set; }
    /// <summary>
    /// 每筆LOG資料的行號
    /// </summary>
    public int LineNumber { get; set; }

    private string _values;
    /// <summary>
    /// 每筆LOG資料的內文
    /// </summary>
    public string Values
    {
        get
        {
            return _values;
        }
        set
        {
            _values = value;
        }
    }
    /// <summary>
    /// 寫入每筆資料的內文(堆疊內文)
    /// </summary>
    /// <param name="_val"></param>
    public void setValues(string _val)
    {
        _values += _val + Environment.NewLine;
    }
}
