using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Log;
using Ede.Uof.Utility.Page.Common;

/// <summary>
/// 訂單協調－烘焙品（COPTC/COPTD）
/// 分頁：1.訂單協調  2.生管用  3.採購用  4.業務用  5.不需生產庫存出貨
/// 本頁只顯示烘焙品（分頁 1~4）；非烘焙品請用 TBBU_TBCOPTDCHECK。
/// </summary>
public partial class CDS_WebPage_COP_TBBU_TBCOPTDCHECK_BAKING : Ede.Uof.Utility.Page.BasePage
{
    #region 常數與欄位

    private const string ERP_CONNECTION = "ERPconnectionstring";
    private const string UOF_CONNECTION = "connectionstring";

    /// <summary>UOF 資料庫名稱（測試時可改為 UOFTEST）</summary>
    private const string DBNAME = "UOF";

    /// <summary>送簽的 UOF 表單名稱</summary>
    private const string UOF_FORM_NAME = "COP10.訂單";

    private const string DIALOG_EDIT_URL = "~/CDS/WebPage/COWORK/TBBU_TBCOPTDCHECKDialogEDIT.aspx";

    private const string STATUS_UNCONFIRMED = "未核單";
    private const string STATUS_CONFIRMED = "已核單";

    /// <summary>單身「是否生產」欄位</summary>
    private const string PRODUCE_FIELD = "COPTDUDF01";

    private string ACCOUNT = null;
    private string NAME = null;
    private string ROLES = null;

    // 送簽時使用的訂單單別、單號（訊息顯示用）
    private string TC001 = "";
    private string TC002 = "";

    #endregion

    #region 查詢條件區塊

    /// <summary>
    /// 每個分頁的查詢條件控制項。
    /// 頁面上的控制項 ID 沿用原本的編號，這裡集中對應成有意義的名稱。
    /// </summary>
    private class SearchPanel
    {
        public TextBox Year;
        public TextBox Month;
        public DropDownList Status;     // 未核單/已核單
        public DropDownList Produce;    // 是否生產 Y/N；null 代表此分頁不提供
        public TextBox OrderNo;
        public TextBox Customer;
        public TextBox Product;
        public GridView Grid;
        public bool OnlyBaking;         // 是否只顯示烘焙品
        public bool OnlyNotProduce;     // 只顯示不需生產的明細
    }

    /// <summary>1.訂單協調</summary>
    private SearchPanel PanelOrder
    {
        get
        {
            return new SearchPanel
            {
                Year = TextBox1, Month = TextBox2, Status = DropDownList1, Produce = DropDownList2,
                OrderNo = TextBox9, Customer = TextBox16, Product = TextBox17, Grid = Grid1,
                OnlyBaking = true
            };
        }
    }

    /// <summary>2.生管用</summary>
    private SearchPanel PanelMoc
    {
        get
        {
            return new SearchPanel
            {
                Year = TextBox3, Month = TextBox4, Status = DropDownList3, Produce = DropDownList4,
                OrderNo = TextBox10, Customer = TextBox18, Product = TextBox19, Grid = Grid2,
                OnlyBaking = true
            };
        }
    }

    /// <summary>3.採購用</summary>
    private SearchPanel PanelPur
    {
        get
        {
            return new SearchPanel
            {
                Year = TextBox5, Month = TextBox6, Status = DropDownList5, Produce = DropDownList6,
                OrderNo = TextBox11, Customer = TextBox20, Product = TextBox21, Grid = Grid3,
                OnlyBaking = true
            };
        }
    }

    /// <summary>4.業務用</summary>
    private SearchPanel PanelSales
    {
        get
        {
            return new SearchPanel
            {
                Year = TextBox7, Month = TextBox8, Status = DropDownList7, Produce = DropDownList8,
                OrderNo = TextBox12, Customer = TextBox22, Product = TextBox23, Grid = Grid4,
                OnlyBaking = true
            };
        }
    }

    /// <summary>5.不需生產庫存出貨</summary>
    private SearchPanel PanelStock
    {
        get
        {
            return new SearchPanel
            {
                Year = TextBox13, Month = TextBox14, Status = DropDownList9, Produce = null,
                OrderNo = TextBox15, Customer = TextBox24, Product = TextBox25, Grid = Grid5,
                OnlyBaking = false, OnlyNotProduce = true
            };
        }
    }

    #endregion

    #region 頁面載入

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;
        ROLES = SEARCHROLES(ACCOUNT.Trim());

        ApplyRolePermissions();

        if (!IsPostBack)
        {
            TextBox15.Text = DateTime.Now.ToString("yyyyMMdd");

            SetDefaultDates();

            foreach (SearchPanel panel in new[] { PanelOrder, PanelMoc, PanelPur, PanelSales, PanelStock })
            {
                FillStatusItems(panel.Status);
                if (panel.Produce != null)
                {
                    FillYesNoItems(panel.Produce);
                }
            }

            // 進頁面只查第一個分頁，其他分頁切換過去時才查詢
            BindPanel(PanelOrder);
            MarkPageViewLoaded("RadPageView1");
        }
    }

    /// <summary>
    /// 依角色開放批次更新按鈕：Button4=生管、Button6=採購、Button8=業務。
    /// 沒有對應角色時維持頁面預設值（Button4 一律先關閉）。
    /// </summary>
    private void ApplyRolePermissions()
    {
        Button4.Enabled = false;

        switch (ROLES)
        {
            case "ADMIN":
                SetBatchButtons(true, true, true);
                break;
            case "MOC":
                SetBatchButtons(true, false, false);
                break;
            case "PUR":
                SetBatchButtons(false, true, false);
                break;
            case "SLAES":   // 角色表中業務的代碼即為 SLAES（沿用原設定）
                SetBatchButtons(false, false, true);
                break;
        }
    }

    private void SetBatchButtons(bool moc, bool pur, bool sales)
    {
        Button4.Enabled = moc;
        Button6.Enabled = pur;
        Button8.Enabled = sales;
    }

    private void SetDefaultDates()
    {
        string year = DateTime.Now.ToString("yyyy");
        string month = DateTime.Now.ToString("MM");

        foreach (SearchPanel panel in new[] { PanelOrder, PanelMoc, PanelPur, PanelSales, PanelStock })
        {
            panel.Year.Text = year;
            panel.Month.Text = month;
        }
    }

    private static void FillStatusItems(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add(STATUS_UNCONFIRMED);
        ddl.Items.Add(STATUS_CONFIRMED);
    }

    private static void FillYesNoItems(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add("Y");
        ddl.Items.Add("N");
    }

    #endregion

    #region 查詢

    /// <summary>
    /// 每筆訂單明細最新一筆協調紀錄（TBCOPTDCHECK 依 ID 取最新），
    /// 以及整張訂單是否可以送簽（ORDER_MOC_READY、ORDER_NO_PRODUCE）
    /// </summary>
    private const string LATEST_CHECK_COLUMNS = @"
        ,(SELECT TOP 1 ISNULL([MOCCHECKDATES],'') FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003 ORDER BY ID DESC) AS 'MOCCHECKDATES'
        ,(SELECT TOP 1 [MOCCHECKS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003 ORDER BY ID DESC) AS 'MOCCHECKS'
        ,(SELECT TOP 1 [MOCCHECKSCOMMENTS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
        ,(SELECT TOP 1 [PURCHECKDATES] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003 ORDER BY ID DESC) AS 'PURCHECKDATES'
        ,(SELECT TOP 1 [PURCHECKS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003 ORDER BY ID DESC) AS 'PURCHECKS'
        ,(SELECT TOP 1 [PURCHECKSCOMMENTS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
        ,(SELECT TOP 1 [SALESCHECKDATES] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003 ORDER BY ID DESC) AS 'SALESCHECKDATES'
        ,(SELECT TOP 1 [SALESCHECKSCOMMENTS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'
        ,CASE WHEN EXISTS (
            SELECT 1 FROM [TK].dbo.COPTD AS ORDERLINE
            WHERE ORDERLINE.TD001=COPTD.TD001 AND ORDERLINE.TD002=COPTD.TD002
            AND ORDERLINE.UDF01 IN ('Y','y')
            AND ISNULL((SELECT TOP 1 ISNULL([MOCCHECKS],'') FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=ORDERLINE.TD001 AND [TBCOPTDCHECK].TD002=ORDERLINE.TD002 AND [TBCOPTDCHECK].TD003=ORDERLINE.TD003 ORDER BY ID DESC),'') NOT IN ('Y')
          ) THEN 'N' ELSE 'Y' END AS 'ORDER_MOC_READY'
        ,CASE WHEN EXISTS (
            SELECT 1 FROM [TK].dbo.COPTD AS ORDERLINE
            WHERE ORDERLINE.TD001=COPTD.TD001 AND ORDERLINE.TD002=COPTD.TD002
            AND ORDERLINE.UDF01 IN ('Y','y')
          ) THEN 'N' ELSE 'Y' END AS 'ORDER_NO_PRODUCE'";

    /// <summary>
    /// 依分頁的查詢條件撈訂單明細並綁定到 Grid
    /// </summary>
    private void BindPanel(SearchPanel panel)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        StringBuilder where = new StringBuilder();

        // 年月（單號前 6 碼）
        if (!string.IsNullOrEmpty(panel.Year.Text) && !string.IsNullOrEmpty(panel.Month.Text))
        {
            if (panel.Month.Text.Length == 1)
            {
                panel.Month.Text = "0" + panel.Month.Text;
            }
            where.Append(" AND TD002 LIKE @YEARMONTH + '%'");
            db.AddParameter("@YEARMONTH", panel.Year.Text.Trim() + panel.Month.Text.Trim());
        }

        // 是否核單
        if (panel.Status.Text == STATUS_UNCONFIRMED)
        {
            where.Append(" AND TD021='N'");
        }
        else if (panel.Status.Text == STATUS_CONFIRMED)
        {
            where.Append(" AND TD021='Y'");
        }

        // 是否生產
        if (panel.Produce != null)
        {
            if (panel.Produce.Text == "Y")
            {
                where.Append(" AND COPTD.UDF01 IN ('Y','y')");
            }
            else if (panel.Produce.Text == "N")
            {
                where.Append(" AND COPTD.UDF01 NOT IN ('Y','y')");
            }
        }
        if (panel.OnlyNotProduce)
        {
            where.Append(" AND COPTD.UDF01 NOT IN ('Y','y')");
        }

        // 訂單單號
        if (!string.IsNullOrEmpty(panel.OrderNo.Text))
        {
            where.Append(" AND TD002 LIKE @ORDERNO + '%'");
            db.AddParameter("@ORDERNO", panel.OrderNo.Text.Trim());
        }

        // 客戶名稱
        if (!string.IsNullOrEmpty(panel.Customer.Text))
        {
            where.Append(" AND TC053 LIKE '%' + @CUSTOMER + '%'");
            db.AddParameter("@CUSTOMER", panel.Customer.Text.Trim());
        }

        // 品名
        if (!string.IsNullOrEmpty(panel.Product.Text))
        {
            where.Append(" AND TD005 LIKE '%' + @PRODUCT + '%'");
            db.AddParameter("@PRODUCT", panel.Product.Text.Trim());
        }

        // 烘焙品
        if (panel.OnlyBaking)
        {
            AppendBakingFilter(where);
        }

        string sql = @"
            SELECT
             LTRIM(RTRIM(TD001))+LTRIM(RTRIM(TD002))+LTRIM(RTRIM(TD003)) AS 'TD123'
            ,LTRIM(RTRIM(TD001))+LTRIM(RTRIM(TD002)) AS 'TD12'
            ,*
            " + LATEST_CHECK_COLUMNS + @"
            ,COPTD.UDF01 AS 'COPTDUDF01'
            FROM [TK].dbo.COPTC
            LEFT JOIN [TK].dbo.COPMA ON MA001=TC004
            ,[TK].dbo.COPTD
            WHERE TC001=TD001 AND TC002=TD002
            " + where + @"
            ORDER BY TD002,TD001,TD003";

        panel.Grid.DataSource = Query(db, sql);
        panel.Grid.DataBind();
    }

    /// <summary>烘焙品品號前綴清單（MOCMANULINEMB001LIKES + ZINVMBBAKING）</summary>
    private const string BAKING_PREFIXES = @"
        SELECT LTRIM(RTRIM([MB001])) AS MB001
        FROM
        (
            SELECT [MB001] FROM [TKMOC].[dbo].[MOCMANULINEMB001LIKES]
            UNION
            SELECT [MB001] FROM [TK].[dbo].[ZINVMBBAKING]
        ) AS BAKINGLIST";

    /// <summary>
    /// 只顯示烘焙品：品號開頭符合烘焙品清單的明細（清單是空的時候不過濾）
    /// </summary>
    private static void AppendBakingFilter(StringBuilder where)
    {
        where.Append(" AND (NOT EXISTS (" + BAKING_PREFIXES + ")"
                   + " OR EXISTS (SELECT 1 FROM (" + BAKING_PREFIXES + ") AS BAKING WHERE TD004 LIKE BAKING.MB001 + '%'))");
    }

    #endregion

    #region 分頁切換與列表樣式

    /// <summary>分頁（PageView）對應的查詢條件區塊</summary>
    private SearchPanel PanelOfPageView(string pageViewId)
    {
        switch (pageViewId)
        {
            case "RadPageView1": return PanelOrder;
            case "RadPageView2": return PanelMoc;
            case "RadPageView3": return PanelPur;
            case "RadPageView4": return PanelSales;
            case "RadPageView5": return PanelStock;
            default: return null;
        }
    }

    /// <summary>
    /// 切換分頁：第一次進入某個分頁時才查詢，避免開頁時一次載入全部分頁。
    /// 已載入過的分頁不重查，保留使用者在列表中輸入到一半的備註。
    /// </summary>
    protected void RadTabStrip2_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        string pageViewId = e.Tab.PageViewID;
        if (IsPageViewLoaded(pageViewId))
        {
            return;
        }

        SearchPanel panel = PanelOfPageView(pageViewId);
        if (panel != null)
        {
            BindPanel(panel);
        }
        MarkPageViewLoaded(pageViewId);
    }

    private bool IsPageViewLoaded(string pageViewId)
    {
        string loaded = ViewState["LoadedPageViews"] as string ?? "";
        return loaded.Contains("|" + pageViewId + "|");
    }

    private void MarkPageViewLoaded(string pageViewId)
    {
        string loaded = ViewState["LoadedPageViews"] as string ?? "|";
        if (!loaded.Contains("|" + pageViewId + "|"))
        {
            ViewState["LoadedPageViews"] = loaded + pageViewId + "|";
        }
    }

    /// <summary>
    /// 列表樣式：表頭加 class；資料列依「是否生產」與「生管核准」上底色，生管核准欄位上字色。
    /// 綠＝需生產且生管已核准（可送簽）、黃＝需生產但生管尚未核准、白＝不需生產。
    /// </summary>
    private static void DecorateRow(GridView grid, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "grid-head";
            return;
        }
        if (e.Row.RowType != DataControlRowType.DataRow)
        {
            return;
        }

        DataRowView data = e.Row.DataItem as DataRowView;
        if (data == null)
        {
            return;
        }

        bool needProduce = FieldText(data, PRODUCE_FIELD) == "Y";
        bool mocApproved = FieldText(data, "MOCCHECKS") == "Y";

        if (needProduce)
        {
            e.Row.CssClass = mocApproved ? "row-ready" : "row-wait";

            int col = FindColumnIndex(grid, "生管核準");
            if (col >= 0)
            {
                // 設定 CssClass 會蓋掉欄位的 ItemStyle-CssClass，所以要一起帶上 col-flag
                e.Row.Cells[col].CssClass = "col-flag " + (mocApproved ? "st-ok" : "st-wait");
            }
        }
    }

    /// <summary>
    /// 送簽按鈕只在「送得出去」時顯示（條件與送簽時的檢查相同）：
    /// ORDER_MOC_READY＝整張單需要生產的明細都已生管核准；ORDER_NO_PRODUCE＝整張單都不需生產。
    /// </summary>
    private static void ShowSendButtonWhen(GridViewRowEventArgs e, string buttonId, string orderFlagField)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
        {
            return;
        }

        Button btnSend = e.Row.FindControl(buttonId) as Button;
        DataRowView data = e.Row.DataItem as DataRowView;
        if (btnSend != null && data != null)
        {
            btnSend.Visible = FieldText(data, orderFlagField) == "Y";
        }
    }

    private static string FieldText(DataRowView data, string field)
    {
        return data.Row.Table.Columns.Contains(field) ? data[field].ToString() : "";
    }

    private static int FindColumnIndex(GridView grid, string headerText)
    {
        for (int i = 0; i < grid.Columns.Count; i++)
        {
            if (grid.Columns[i].Visible && grid.Columns[i].HeaderText == headerText)
            {
                return i;
            }
        }
        return -1;
    }

    #endregion

    #region Grid 事件

    // 分頁大小設為 1000，不需處理換頁
    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e) { }
    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e) { }
    protected void grid_PageIndexChanging3(object sender, GridViewPageEventArgs e) { }
    protected void grid_PageIndexChanging4(object sender, GridViewPageEventArgs e) { }
    protected void grid_PageIndexChanging5(object sender, GridViewPageEventArgs e) { }

    // 匯出 Excel 使用 Grid 預設內容
    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }
    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }
    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }
    public void OnBeforeExport4(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }
    public void OnBeforeExport5(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }

    /// <summary>
    /// 1.訂單協調：「更新」開協調視窗；「送簽」只在整張單需要生產的明細都已生管核准時顯示
    /// </summary>
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DecorateRow(Grid1, e);

        if (e.Row.RowType != DataControlRowType.DataRow)
        {
            return;
        }

        Button btnEdit = (Button)e.Row.FindControl("Button1");
        ExpandoObject param = new { ID = btnEdit.CommandArgument }.ToExpando();
        Dialog.Open2(btnEdit, DIALOG_EDIT_URL, "", 800, 600, Dialog.PostBackType.Allows, param);

        ShowSendButtonWhen(e, "Button2", "ORDER_MOC_READY");
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Button1")
        {
            BindPanel(PanelOrder);
        }
        else if (e.CommandName == "Button2")
        {
            string orderNo = e.CommandArgument.ToString();

            if (IsOrderInSigning(orderNo))
            {
                MsgBox(orderNo + " 重覆送單 \r\n 此訂單已送出簽核了", this.Page, this);
                return;
            }

            // 檢查並送出 UOF
            CHECKTBCOPTDCHECK(orderNo);

            // 送單後提示信用額度
            ShowCreditWarning(orderNo);
        }
    }

    /// <summary>2.生管用：下拉預設為目前的生管核准狀態</summary>
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DecorateRow(Grid2, e);

        SelectRowDropDown(e, "GRIDVIEWDropDownList1", "MOCCHECKS");
    }

    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Button3")
        {
            BindPanel(PanelMoc);
        }
        else if (e.CommandName == "Button4")
        {
            CHECKTBCOPTDCHECK(e.CommandArgument.ToString());
        }
    }

    /// <summary>3.採購用：下拉預設為目前的採購核准狀態</summary>
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DecorateRow(Grid3, e);

        SelectRowDropDown(e, "GRIDVIEW2DropDownList1", "PURCHECKS");
    }

    protected void Grid3_RowCommand(object sender, GridViewCommandEventArgs e) { }

    protected void Grid4_RowDataBound(object sender, GridViewRowEventArgs e) 
    {
        DecorateRow(Grid4, e);
        ShowSendButtonWhen(e, "GRIDVIEW4Button1", "ORDER_MOC_READY");
    }

    protected void Grid4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "GRIDVIEW4Button1")
        {
            CHECKTBCOPTDCHECK(e.CommandArgument.ToString());
        }
    }

    protected void Grid5_RowDataBound(object sender, GridViewRowEventArgs e) 
    {
        DecorateRow(Grid5, e);
        ShowSendButtonWhen(e, "GRIDVIEW5Button1", "ORDER_NO_PRODUCE");
    }

    protected void Grid5_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "GRIDVIEW5Button1")
        {
            CHECKTBCOPTDCHECK2(e.CommandArgument.ToString());
        }
    }

    private static void SelectRowDropDown(GridViewRowEventArgs e, string dropDownId, string field)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
        {
            return;
        }

        DropDownList ddl = (DropDownList)e.Row.FindControl(dropDownId);
        if (ddl != null)
        {
            ddl.SelectedValue = System.Web.UI.DataBinder.Eval(e.Row.DataItem, field).ToString();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // 允許 Grid 在 form 外 Render（匯出用）
    }

    #endregion

    #region 按鈕

    protected void Button1_Click(object sender, EventArgs e)
    {
        BindPanel(PanelOrder);
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        BindPanel(PanelMoc);
    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        BindPanel(PanelPur);
    }

    protected void Button7_Click(object sender, EventArgs e)
    {
        BindPanel(PanelSales);
    }

    protected void Button9_Click(object sender, EventArgs e)
    {
        BindPanel(PanelStock);
    }

    /// <summary>生管批次更新</summary>
    protected void Button4_Click(object sender, EventArgs e)
    {
        ADDMOC();
        BindPanel(PanelMoc);
    }

    /// <summary>採購批次更新</summary>
    protected void Button6_Click(object sender, EventArgs e)
    {
        ADDPUR();
        BindPanel(PanelPur);
    }

    /// <summary>業務批次更新</summary>
    protected void Button8_Click(object sender, EventArgs e)
    {
        ADDSALES();
        BindPanel(PanelSales);
    }

    #endregion

    #region 批次更新協調紀錄

    /// <summary>生管：有填備註的列，新增一筆生管核准紀錄</summary>
    public void ADDMOC()
    {
        foreach (GridViewRow gvr in Grid2.Rows)
        {
            TextBox txtComment = (TextBox)gvr.FindControl("GRIDVIEWTextBox1");
            DropDownList ddlCheck = (DropDownList)gvr.FindControl("GRIDVIEWDropDownList1");

            if (!string.IsNullOrEmpty(txtComment.Text))
            {
                ADDTBCOPTDCHECKMOC(RowKey(gvr, 1), RowKey(gvr, 2), RowKey(gvr, 3), null, ddlCheck.SelectedValue, txtComment.Text);
            }
        }

        ClearRowTextBoxes(Grid2, "GRIDVIEWTextBox1");
    }

    /// <summary>採購：有填備註的列，新增一筆採購核准紀錄</summary>
    public void ADDPUR()
    {
        foreach (GridViewRow gvr in Grid3.Rows)
        {
            TextBox txtComment = (TextBox)gvr.FindControl("GRIDVIEW2TextBox1");
            DropDownList ddlCheck = (DropDownList)gvr.FindControl("GRIDVIEW2DropDownList1");

            if (!string.IsNullOrEmpty(txtComment.Text))
            {
                ADDTBCOPTDCHECKPUR(RowKey(gvr, 1), RowKey(gvr, 2), RowKey(gvr, 3), null, ddlCheck.SelectedValue, txtComment.Text);
            }
        }

        ClearRowTextBoxes(Grid3, "GRIDVIEW2TextBox1");
    }

    /// <summary>業務：有填備註的列，新增一筆業務備註紀錄</summary>
    public void ADDSALES()
    {
        foreach (GridViewRow gvr in Grid4.Rows)
        {
            TextBox txtComment = (TextBox)gvr.FindControl("GRIDVIEW4TextBox1");

            if (!string.IsNullOrEmpty(txtComment.Text))
            {
                ADDTBCOPTDCHECKSALES(RowKey(gvr, 1), RowKey(gvr, 2), RowKey(gvr, 3), null, txtComment.Text);
            }
        }

        ClearRowTextBoxes(Grid4, "GRIDVIEW4TextBox1");
    }

    /// <summary>取 Grid 列的單別/單號/序號欄位（第 1~3 欄）</summary>
    private static string RowKey(GridViewRow row, int cellIndex)
    {
        return row.Cells[cellIndex].Text.Trim();
    }

    private static void ClearRowTextBoxes(GridView grid, string textBoxId)
    {
        foreach (GridViewRow row in grid.Rows)
        {
            ((TextBox)row.FindControl(textBoxId)).Text = "";
        }
    }

    /// <summary>
    /// 新增協調紀錄的共用 SQL：沿用最新一筆紀錄的其他部門欄位，只更新 {0} 指定的部門欄位。
    /// </summary>
    private void InsertCheckRecord(string TD001, string TD002, string TD003,
                                   string mocColumns, string purColumns, string salesColumns,
                                   Dictionary<string, string> parameters)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);

        string sql = @"
            INSERT INTO [TKBUSINESS].[dbo].[TBCOPTDCHECK]
            ([TD001],[TD002],[TD003],[TD004],[TD005],[TD008],[TD009],[TD010],[TD011],[TD012],[TD013],[TD024],[TD025],[TC015],[TD020]
            ,[MOCCHECKDATES],[MOCCHECKS],[MOCCHECKSCOMMENTS]
            ,[PURCHECKDATES],[PURCHECKS],[PURCHECKSCOMMENTS]
            ,[SALESCHECKDATES],[SALESCHECKSCOMMENTS])
            SELECT
            [TD001],[TD002],[TD003],[TD004],[TD005],[TD008],[TD009],[TD010],[TD011],[TD012],[TD013],[TD024],[TD025],[TC015],[TD020]
            " + mocColumns + @"
            " + purColumns + @"
            " + salesColumns + @"
            FROM [TK].dbo.COPTD,[TK].dbo.COPTC
            WHERE TC001=TD001 AND TC002=TD002
            AND TD001=@TD001 AND TD002=@TD002 AND TD003=@TD003";

        db.AddParameter("@TD001", TD001);
        db.AddParameter("@TD002", TD002);
        db.AddParameter("@TD003", TD003);
        foreach (KeyValuePair<string, string> p in parameters)
        {
            db.AddParameter(p.Key, p.Value);
        }

        db.ExecuteNonQuery(sql);
    }

    /// <summary>沿用最新一筆紀錄的欄位值</summary>
    private static string LatestValue(string column)
    {
        return ",(SELECT TOP 1 [" + column + "] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003 ORDER BY ID DESC) AS [" + column + "]";
    }

    private static string NowText()
    {
        return DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
    }

    public void ADDTBCOPTDCHECKMOC(string TD001, string TD002, string TD003, string MOCCHECKDATES, string MOCCHECKS, string MOCCHECKSCOMMENTS)
    {
        InsertCheckRecord(TD001, TD002, TD003,
            ",@MOCCHECKDATES AS [MOCCHECKDATES],@MOCCHECKS AS [MOCCHECKS],@MOCCHECKSCOMMENTS AS [MOCCHECKSCOMMENTS]",
            LatestValue("PURCHECKDATES") + LatestValue("PURCHECKS") + LatestValue("PURCHECKSCOMMENTS"),
            LatestValue("SALESCHECKDATES") + LatestValue("SALESCHECKSCOMMENTS"),
            new Dictionary<string, string>
            {
                { "@MOCCHECKDATES", NowText() },
                { "@MOCCHECKS", MOCCHECKS },
                { "@MOCCHECKSCOMMENTS", MOCCHECKSCOMMENTS }
            });
    }

    public void ADDTBCOPTDCHECKPUR(string TD001, string TD002, string TD003, string PURCHECKDATES, string PURCHECKS, string PURCHECKSCOMMENTS)
    {
        InsertCheckRecord(TD001, TD002, TD003,
            LatestValue("MOCCHECKDATES") + LatestValue("MOCCHECKS") + LatestValue("MOCCHECKSCOMMENTS"),
            ",@PURCHECKDATES AS [PURCHECKDATES],@PURCHECKS AS [PURCHECKS],@PURCHECKSCOMMENTS AS [PURCHECKSCOMMENTS]",
            LatestValue("SALESCHECKDATES") + LatestValue("SALESCHECKSCOMMENTS"),
            new Dictionary<string, string>
            {
                { "@PURCHECKDATES", NowText() },
                { "@PURCHECKS", PURCHECKS },
                { "@PURCHECKSCOMMENTS", PURCHECKSCOMMENTS }
            });
    }

    public void ADDTBCOPTDCHECKSALES(string TD001, string TD002, string TD003, string SALESCHECKDATES, string SALESCHECKSCOMMENTS)
    {
        InsertCheckRecord(TD001, TD002, TD003,
            LatestValue("MOCCHECKDATES") + LatestValue("MOCCHECKS") + LatestValue("MOCCHECKSCOMMENTS"),
            LatestValue("PURCHECKDATES") + LatestValue("PURCHECKS") + LatestValue("PURCHECKSCOMMENTS"),
            ",@SALESCHECKDATES AS [SALESCHECKDATES],@SALESCHECKSCOMMENTS AS [SALESCHECKSCOMMENTS]",
            new Dictionary<string, string>
            {
                { "@SALESCHECKDATES", NowText() },
                { "@SALESCHECKSCOMMENTS", SALESCHECKSCOMMENTS }
            });
    }

    #endregion

    #region 送簽

    /// <summary>
    /// 送簽（訂單協調、生管用、業務用）：
    /// 單身有需要生產的品項，必須生管核准後才能送簽；都不需生產則直接送簽。（已取消採購核準檢查）
    /// </summary>
    public void CHECKTBCOPTDCHECK(string TD001TD002)
    {
        try
        {
            DatabaseHelper db = CreateDb(ERP_CONNECTION);

            // 單身需要生產、但生管尚未核准的明細
            string sql = @"
                SELECT *
                FROM
                (
                    SELECT COPTD.UDF01 AS 'COPTDUDF01',TD001,TD002,TD003
                    ,(SELECT TOP 1 ISNULL([MOCCHECKS],'') FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003 ORDER BY ID DESC) AS 'MOCCHECKS'
                    FROM [TK].dbo.COPTC,[TK].dbo.COPTD
                    WHERE TC001=TD001 AND TC002=TD002
                ) AS TEMP
                WHERE COPTDUDF01 IN ('Y','y')
                AND ISNULL([MOCCHECKS],'') NOT IN ('Y')
                AND LTRIM(RTRIM(TD001))+LTRIM(RTRIM(TD002))=@TD001TD002";
            db.AddParameter("@TD001TD002", TD001TD002);

            DataTable pending = Query(db, sql);

            SplitOrderNo(TD001TD002);

            if (pending.Rows.Count == 0)
            {
                SendToUofIfNotSigning(TD001TD002);
            }
            else
            {
                MsgBox("送單失敗!!!! \r\n  送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n \r\n訂單的單身有需要生產的，需經生管同意" + TC001 + TC002, this.Page, this);
            }
        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
    }

    /// <summary>
    /// 送簽（不需生產庫存出貨）：單身只要有需要生產的品項就不能從這裡送簽。
    /// </summary>
    public void CHECKTBCOPTDCHECK2(string TD001TD002)
    {
        try
        {
            DatabaseHelper db = CreateDb(ERP_CONNECTION);

            string sql = @"
                SELECT TD001,TD002,TD003
                FROM [TK].dbo.COPTC,[TK].dbo.COPTD
                WHERE TC001=TD001 AND TC002=TD002
                AND COPTD.UDF01 IN ('Y','y')
                AND LTRIM(RTRIM(TD001))+LTRIM(RTRIM(TD002))=@TD001TD002";
            db.AddParameter("@TD001TD002", TD001TD002);

            DataTable needProduce = Query(db, sql);

            SplitOrderNo(TD001TD002);

            if (needProduce.Rows.Count == 0)
            {
                SendToUofIfNotSigning(TD001TD002);
            }
            else
            {
                MsgBox("送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!!  \r\n  \r\n訂單的單身有需要生產的，需經生管、採購同意" + TC001 + TC002, this.Page, this);
            }
        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
    }

    /// <summary>單別 4 碼 + 單號 11 碼</summary>
    private void SplitOrderNo(string TD001TD002)
    {
        TC001 = TD001TD002.Substring(0, 4);
        TC002 = TD001TD002.Substring(4, 11);
    }

    private void SendToUofIfNotSigning(string TD001TD002)
    {
        if (IsOrderInSigning(TD001TD002))
        {
            MsgBox(TD001TD002 + " 重覆送單 \r\n 此訂單已送出簽核了", this.Page, this);
        }
        else
        {
            ADDTB_WKF_EXTERNAL_TASK_COPTCCOPTD(TC001, TC002);
        }
    }

    private bool IsOrderInSigning(string TC001TC002)
    {
        DataTable dt = CHECK_TB_WKF_TASK(TC001TC002);
        return dt != null && dt.Rows.Count >= 1;
    }

    /// <summary>送單後顯示：訂單金額 + 已用信用額度 超過客戶信用額度時提示</summary>
    private void ShowCreditWarning(string TC001TC002)
    {
        DataTable order = FINDCOPTCTC004(TC001TC002);
        string TC004 = order.Rows[0]["TC004"].ToString();

        int orderAmount = Convert.ToInt32(Convert.ToDecimal(order.Rows[0]["COPTCMONEYS"].ToString()));
        int creditLimit = Convert.ToInt32(FINDCOPMATOTALLIMITS(TC004));
        int creditUsed = Convert.ToInt32(FINDCREDITS(TC004));

        if (creditLimit < (creditUsed + orderAmount))
        {
            MsgBox(TC001TC002 + " 訂單金額=" + orderAmount.ToString("0,0")
                + " \r\n客代:" + TC004
                + " \r\n目前設定的信用額度=" + creditLimit.ToString("0,0")
                + " \r\n已花費的信用額度=" + creditUsed.ToString("0,0"), this.Page, this);
        }
    }

    /// <summary>
    /// 組成 UOF「COP10.訂單」表單 XML 並起單
    /// </summary>
    public void ADDTB_WKF_EXTERNAL_TASK_COPTCCOPTD(string TC001, string TC002)
    {
        DataTable DT = SEARCHCOPTCCOPTD(TC001, TC002);

        // 申請人必須有 UOF 部門（GROUP_CODE 不可空白），查不到時中斷送單
        if (SEARCHUOFDEP(DT.Rows[0]["TC006"].ToString()) == null)
        {
            throw new Exception("申請人 " + DT.Rows[0]["TC006"].ToString() + " 在 UOF 沒有部門代號，無法送單");
        }

        DataTable dtBaking = CHECK_COPTD_ZINVMBBAKING(TC001, TC002);
        bool hasBaking = dtBaking != null && dtBaking.Rows.Count >= 1;

        XmlElement form = BuildOrderFormXml(DT, hasBaking, SEARCHFORM_VERSION_ID(UOF_FORM_NAME));

        ADDTACK(form);
    }

    /// <summary>
    /// 依訂單資料組成 UOF 表單 XML（不連資料庫，方便檢查欄位）
    /// </summary>
    private static XmlElement BuildOrderFormXml(DataTable DT, bool hasBaking, string formVersionId)
    {
        DataRow head = DT.Rows[0];

        string account = head["TC006"].ToString();
        string fillerUserGuid = head["USER_GUID"].ToString();
        string BA = head["BA"].ToString();
        string BANAME = head["BANAME"].ToString();

        // 單身只要有一筆需要生產（大寫 Y），單頭就標記為需要生產
        string COPTCUDF01 = "N";
        foreach (DataRow od in DT.Rows)
        {
            if (od["COPTDUDF01"].ToString().Equals("Y"))
            {
                COPTCUDF01 = "Y";
                break;
            }
        }

        UofFormBuilder fb = new UofFormBuilder(formVersionId, account, head["GROUP_ID"].ToString(), head["TITLE_ID"].ToString(),
                                               head["NAME"].ToString(), fillerUserGuid);

        fb.AddField("ID", "");
        fb.AddField("TC001NAMES", head["MQ002"].ToString());
        fb.AddField("TC001", head["TC001"].ToString());
        fb.AddField("TC002", head["TC002"].ToString());
        fb.AddField("CHECKMOC", hasBaking ? "烘培" : "生產", "", "@null");
        fb.AddField("COPTCUDF01", COPTCUDF01);
        fb.AddField("TC003", head["TC003"].ToString());
        fb.AddField("TC004", head["TC004"].ToString());
        fb.AddField("TC005", head["TC005"].ToString());
        fb.AddField("TC005NAME", head["ME002"].ToString());
        fb.AddField("TC053", head["TC053"].ToString());
        fb.AddField("TC006", head["NAME"].ToString() + "(" + head["TC006"].ToString() + ")", UofFormBuilder.UserSet(fillerUserGuid));
        fb.AddField("MV002", head["NAME"].ToString());
        fb.AddField("BA", BANAME + "(" + BA + ")", UofFormBuilder.UserSet(head["BA_USER_GUID"].ToString()));
        fb.AddField("BANAME", BANAME);
        fb.AddField("TC015", head["TC015"].ToString());
        fb.AddField("TC008", head["TC008"].ToString());
        fb.AddField("TC009", head["TC009"].ToString());
        fb.AddField("TC045", head["TC045"].ToString());
        fb.AddField("TC029", head["TC029"].ToString());
        fb.AddField("TC030", head["TC030"].ToString());
        fb.AddField("TC041", head["TC041"].ToString());
        fb.AddField("TC016", head["NEWTC016"].ToString());
        fb.AddField("TC124", head["TC124"].ToString());
        fb.AddField("TC031", head["TC031"].ToString());
        fb.AddField("TC043", head["TC043"].ToString());
        fb.AddField("TC044", head["TC044"].ToString());
        fb.AddField("TC046", head["TC046"].ToString());
        fb.AddField("TC018", head["TC018"].ToString());
        fb.AddField("TC010", head["TC010"].ToString());
        fb.AddField("TC012", head["TC012"].ToString());
        fb.AddField("TC035", head["TC035"].ToString());
        fb.AddField("TC054", head["TC054"].ToString());
        fb.AddField("TC055", head["TC055"].ToString());
        fb.AddField("TC065", head["TC065"].ToString());
        fb.AddField("TC042", head["TC042"].ToString());
        fb.AddField("TC014", head["TC014"].ToString());
        fb.AddField("TC019", head["TC019"].ToString());
        fb.AddField("TC032", head["TC032"].ToString());
        fb.AddField("TC033", head["TC033"].ToString());
        fb.AddField("TC039", head["TC039"].ToString());
        fb.AddField("TC121", head["NEWTC016"].ToString());   // 注意：原本就是帶 NEWTC016（發票聯數可能應為 NEWTC121）
        fb.AddField("TC094", head["TC094"].ToString());
        fb.AddField("TC063", head["TC063"].ToString());
        fb.AddField("TC115", head["TC115"].ToString());
        fb.AddField("TC116", head["TC116"].ToString());
        fb.AddField("MOC", "");
        fb.AddField("PUR", "");
        XmlElement details = fb.AddField("DETAILS", "");

        // 單身明細
        string[] detailColumns = { "TD003", "TD004", "TD005", "TD006", "TD008", "TD024", "TD009", "TD025", "TD010", "TD011", "TD026", "TD012", "TD013", "TD017", "TD018", "TD019", "TD020" };
        XmlElement grid = fb.AddDataGrid(details);
        int order = 0;
        foreach (DataRow od in DT.Rows)
        {
            XmlElement row = fb.AddRow(grid, order++);
            fb.AddCell(row, "UDF01", od["COPTDUDF01"].ToString());
            foreach (string col in detailColumns)
            {
                fb.AddCell(row, col, od[col].ToString());
            }
        }

        return fb.Form;
    }

    /// <summary>
    /// 呼叫 UOF WebService 起單
    /// </summary>
    public void ADDTACK(XmlElement Form)
    {
        Ede.Uof.WKF.Utility.TaskUtilityUCO taskUCO = new Ede.Uof.WKF.Utility.TaskUtilityUCO();

        string result = taskUCO.WebService_CreateTask(Form.OuterXml);
        XElement resultXE = XElement.Parse(result);

        if (resultXE.Element("Status").Value == "1")
        {
            string formNBR = resultXE.Element("FormNumber").Value;

            Logger.Write("起單", "送單成功!" + formNBR);
            MsgBox("送單成功 \r\n" + TC001 + TC002 + " > " + formNBR, this.Page, this);
        }
        else
        {
            string status = "送單失敗!";
            string error = resultXE.Element("Exception").Element("Message").Value;

            Logger.Write("起單", status + error + "\r\n" + Form.OuterXml);
            MsgBox("失敗了，無法送單!!!!    " + error + "\r\n" + Form.OuterXml, this.Page, this);

            throw new Exception(status + error + "\r\n" + Form.OuterXml);
        }
    }

    /// <summary>
    /// UOF 外部起單 XML 產生器：
    /// 每個 FieldItem 固定帶 realValue、enableSearch 與填表人資訊。
    /// </summary>
    private class UofFormBuilder
    {
        private readonly XmlDocument doc = new XmlDocument();
        private readonly XmlElement fieldValues;
        private readonly string fillerName;
        private readonly string fillerUserGuid;
        private readonly string fillerAccount;

        public XmlElement Form { get; private set; }

        public UofFormBuilder(string formVersionId, string account, string groupId, string jobTitleId, string fillerName, string fillerUserGuid)
        {
            this.fillerName = fillerName;
            this.fillerUserGuid = fillerUserGuid;
            this.fillerAccount = account;

            Form = doc.CreateElement("Form");
            if (!string.IsNullOrEmpty(formVersionId))
            {
                Form.SetAttribute("formVersionId", formVersionId);
            }
            Form.SetAttribute("urgentLevel", "2");
            doc.AppendChild(Form);

            XmlElement applicant = doc.CreateElement("Applicant");
            applicant.SetAttribute("account", account);
            applicant.SetAttribute("groupId", groupId);
            applicant.SetAttribute("jobTitleId", jobTitleId);
            Form.AppendChild(applicant);

            XmlElement comment = doc.CreateElement("Comment");
            comment.InnerText = "申請者意見";
            applicant.AppendChild(comment);

            fieldValues = doc.CreateElement("FormFieldValue");
            Form.AppendChild(fieldValues);
        }

        public XmlElement AddField(string fieldId, string fieldValue, string realValue = "", string customValue = null)
        {
            XmlElement item = doc.CreateElement("FieldItem");
            item.SetAttribute("fieldId", fieldId);
            item.SetAttribute("fieldValue", fieldValue);
            item.SetAttribute("realValue", realValue);
            item.SetAttribute("enableSearch", "True");
            if (customValue != null)
            {
                item.SetAttribute("customValue", customValue);
            }
            item.SetAttribute("fillerName", fillerName);
            item.SetAttribute("fillerUserGuid", fillerUserGuid);
            item.SetAttribute("fillerAccount", fillerAccount);
            item.SetAttribute("fillSiteId", "");
            fieldValues.AppendChild(item);
            return item;
        }

        public XmlElement AddDataGrid(XmlElement field)
        {
            XmlElement grid = doc.CreateElement("DataGrid");
            field.AppendChild(grid);
            return grid;
        }

        public XmlElement AddRow(XmlElement grid, int order)
        {
            XmlElement row = doc.CreateElement("Row");
            row.SetAttribute("order", order.ToString());
            grid.AppendChild(row);
            return row;
        }

        public void AddCell(XmlElement row, string fieldId, string fieldValue)
        {
            XmlElement cell = doc.CreateElement("Cell");
            cell.SetAttribute("fieldId", fieldId);
            cell.SetAttribute("fieldValue", fieldValue);
            cell.SetAttribute("realValue", "");
            cell.SetAttribute("customValue", "");
            cell.SetAttribute("enableSearch", "True");
            row.AppendChild(cell);
        }

        /// <summary>人員欄位的 realValue：&lt;UserSet&gt;&lt;Element type="user"&gt;&lt;userId&gt;…</summary>
        public static string UserSet(string userGuid)
        {
            return new XElement("UserSet",
                       new XElement("Element", new XAttribute("type", "user"),
                           new XElement("userId", userGuid))).ToString();
        }
    }

    #endregion

    #region 資料查詢

    private static DatabaseHelper CreateDb(string connectionName)
    {
        return new DatabaseHelper(ConfigurationManager.ConnectionStrings[connectionName].ToString());
    }

    private static DataTable Query(DatabaseHelper db, string sql)
    {
        DataTable dt = new DataTable();
        dt.Load(db.ExecuteReader(sql));
        return dt;
    }

    /// <summary>有資料回傳 DataTable，沒有資料或發生錯誤回傳 null</summary>
    private static DataTable QueryOrNull(DatabaseHelper db, string sql)
    {
        try
        {
            DataTable dt = Query(db, sql);
            return dt.Rows.Count >= 1 ? dt : null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>使用者在協調角色表中的角色；找不到回傳 NOROLES</summary>
    public string SEARCHROLES(string ACCOUNT)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@ACCOUNT", ACCOUNT);

        DataTable dt = Query(db, @"
            SELECT [ID],[ROLES],[MV001],[MV002]
            FROM [TKBUSINESS].[dbo].[TBCOPTDCHECKROLES]
            WHERE MV001 LIKE @ACCOUNT + '%'");

        return dt.Rows.Count > 0 ? dt.Rows[0]["ROLES"].ToString().Trim() : "NOROLES";
    }

    /// <summary>訂單單頭＋單身（含 UOF 人員、部門、業務窗口），送簽組表單用</summary>
    public DataTable SEARCHCOPTCCOPTD(string TC001, string TC002)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@TC001", TC001);
        db.AddParameter("@TC002", TC002);

        string sql = string.Format(@"
            SELECT
            COMPANY,CREATOR,USR_GROUP,CREATE_DATE,MODIFIER,MODI_DATE,FLAG,CREATE_TIME,MODI_TIME,TRANS_TYPE,TRANS_NAME
            ,sync_date,sync_time,sync_mark,sync_count,DataUser,DataGroup
            ,TC001,TC002,TC003,TC004,TC005,TC006,TC007,TC008,TC009,TC010
            ,TC011,TC012,TC013,TC014,TC015,TC016,TC017,TC018,TC019,TC020
            ,TC021,TC022,TC023,TC024,TC025,TC026,TC027,TC028,TC029,TC030
            ,TC031,TC032,TC033,TC034,TC035,TC036,TC037,TC038,TC039,TC040
            ,TC041,TC042,TC043,TC044,TC045,TC046,TC047,TC048,TC049,TC050
            ,TC051,TC052,TC053,TC054,TC055,TC056,TC057,TC058,TC059,TC060
            ,TC061,TC062,TC063,TC064,TC065,TC066,TC067,TC068,TC069,TC070
            ,TC071,TC072,TC073,TC074,TC075,TC076,TC077,TC078,TC079,TC080
            ,TC081,TC082,TC083,TC084,TC085,TC086,TC087,TC088,TC089,TC090
            ,TC091,TC092,TC093,TC094,TC095,TC096,TC097,TC098,TC099,TC100
            ,TC101,TC102,TC103,TC104,TC105,TC106,TC107,TC108,TC109,TC110
            ,TC111,TC112,TC113,TC114,TC115,TC116,TC117,TC118,TC119,TC120
            ,TC121,TC122,TC123,TC124,TC125,TC126,TC127,TC128,TC129,TC130
            ,TC131,TC132,TC133,TC134,TC135,TC136,TC137,TC138,TC139,TC140
            ,TC141,TC142,TC143,TC144,TC145,TC146
            ,UDF01,UDF02,UDF03,UDF04,UDF05,UDF06,UDF07,UDF08,UDF09,UDF10
            ,TD001,TD002,TD003,TD004,TD005,TD006,TD007,TD008,TD009,TD010
            ,TD011,TD012,TD013,TD014,TD015,TD016,TD017,TD018,TD019,TD020
            ,TD021,TD022,TD023,TD024,TD025,TD026,TD027,TD028,TD029,TD030
            ,TD031,TD032,TD033,TD034,TD035,TD036,TD037,TD038,TD039,TD040
            ,TD041,TD042,TD043,TD044,TD045,TD046,TD047,TD048,TD049,TD050
            ,TD051,TD052,TD053,TD054,TD055,TD056,TD057,TD058,TD059,TD060
            ,TD061,TD062,TD063,TD064,TD065,TD066,TD067,TD068,TD069,TD070
            ,TD071,TD072,TD073,TD074,TD075,TD076,TD077,TD078,TD079,TD080
            ,TD081,TD082,TD083,TD084,TD085,TD086,TD087,TD088,TD089,TD090
            ,TD091,TD092,TD093,TD094,TD095,TD096,TD097,TD098,TD099,TD100
            ,TD101,TD102,TD103,TD104,TD105,TD106,TD107,TD108,TD109,TD110
            ,TD111,TD112,TD113
            ,COPTDUDF01,COPTDUDF02,COPTDUDF03,COPTDUDF04,COPTDUDF05,COPTDUDF06,COPTDUDF07,COPTDUDF08,COPTDUDF09,COPTDUDF10,TD200
            ,USER_GUID,NAME
            ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
            ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'
            ,MA002
            ,CASE TC016 WHEN '1' THEN '1.應稅內含' WHEN '2' THEN '2.應稅外加' WHEN '3' THEN '3.零稅率' WHEN '4' THEN '4.免稅' WHEN '9' THEN '9.不計稅' ELSE '' END AS 'NEWTC016'
            ,CASE TC121 WHEN '1' THEN '1.二聯式' WHEN '2' THEN '2.三聯式' WHEN '3' THEN '3.二聯式收銀機發票' WHEN '4' THEN '4.三聯式收銀機發票' WHEN '5' THEN '5.電子計算機發票' WHEN '6' THEN '6.免用統一發票' WHEN '7' THEN '7.電子發票' ELSE '' END AS 'NEWTC121'
            ,BA
            ,BANAME
            ,(SELECT TOP 1 [USER_GUID] FROM [192.168.1.223].[UOF].[dbo].[TB_EB_USER] WHERE [ACCOUNT]=BA COLLATE Chinese_Taiwan_Stroke_BIN) AS 'BA_USER_GUID'
            ,ME002
            ,MQ002
            FROM
            (
                SELECT [COPTC].[COMPANY],[COPTC].[CREATOR],[COPTC].[USR_GROUP],[COPTC].[CREATE_DATE],[COPTC].[MODIFIER],[COPTC].[MODI_DATE],[COPTC].[FLAG],[COPTC].[CREATE_TIME],[COPTC].[MODI_TIME],[COPTC].[TRANS_TYPE],[COPTC].[TRANS_NAME]
                ,[COPTC].[sync_date],[COPTC].[sync_time],[COPTC].[sync_mark],[COPTC].[sync_count],[COPTC].[DataUser],[COPTC].[DataGroup]
                ,[COPTC].[TC001],[COPTC].[TC002],[COPTC].[TC003],[COPTC].[TC004],[COPTC].[TC005],[COPTC].[TC006],[COPTC].[TC007],[COPTC].[TC008],[COPTC].[TC009],[COPTC].[TC010]
                ,[COPTC].[TC011],[COPTC].[TC012],[COPTC].[TC013],[COPTC].[TC014],[COPTC].[TC015],[COPTC].[TC016],[COPTC].[TC017],[COPTC].[TC018],[COPTC].[TC019],[COPTC].[TC020]
                ,[COPTC].[TC021],[COPTC].[TC022],[COPTC].[TC023],[COPTC].[TC024],[COPTC].[TC025],[COPTC].[TC026],[COPTC].[TC027],[COPTC].[TC028],[COPTC].[TC029],[COPTC].[TC030]
                ,[COPTC].[TC031],[COPTC].[TC032],[COPTC].[TC033],[COPTC].[TC034],[COPTC].[TC035],[COPTC].[TC036],[COPTC].[TC037],[COPTC].[TC038],[COPTC].[TC039],[COPTC].[TC040]
                ,[COPTC].[TC041],[COPTC].[TC042],[COPTC].[TC043],[COPTC].[TC044],[COPTC].[TC045],[COPTC].[TC046],[COPTC].[TC047],[COPTC].[TC048],[COPTC].[TC049],[COPTC].[TC050]
                ,[COPTC].[TC051],[COPTC].[TC052],[COPTC].[TC053],[COPTC].[TC054],[COPTC].[TC055],[COPTC].[TC056],[COPTC].[TC057],[COPTC].[TC058],[COPTC].[TC059],[COPTC].[TC060]
                ,[COPTC].[TC061],[COPTC].[TC062],[COPTC].[TC063],[COPTC].[TC064],[COPTC].[TC065],[COPTC].[TC066],[COPTC].[TC067],[COPTC].[TC068],[COPTC].[TC069],[COPTC].[TC070]
                ,[COPTC].[TC071],[COPTC].[TC072],[COPTC].[TC073],[COPTC].[TC074],[COPTC].[TC075],[COPTC].[TC076],[COPTC].[TC077],[COPTC].[TC078],[COPTC].[TC079],[COPTC].[TC080]
                ,[COPTC].[TC081],[COPTC].[TC082],[COPTC].[TC083],[COPTC].[TC084],[COPTC].[TC085],[COPTC].[TC086],[COPTC].[TC087],[COPTC].[TC088],[COPTC].[TC089],[COPTC].[TC090]
                ,[COPTC].[TC091],[COPTC].[TC092],[COPTC].[TC093],[COPTC].[TC094],[COPTC].[TC095],[COPTC].[TC096],[COPTC].[TC097],[COPTC].[TC098],[COPTC].[TC099],[COPTC].[TC100]
                ,[COPTC].[TC101],[COPTC].[TC102],[COPTC].[TC103],[COPTC].[TC104],[COPTC].[TC105],[COPTC].[TC106],[COPTC].[TC107],[COPTC].[TC108],[COPTC].[TC109],[COPTC].[TC110]
                ,[COPTC].[TC111],[COPTC].[TC112],[COPTC].[TC113],[COPTC].[TC114],[COPTC].[TC115],[COPTC].[TC116],[COPTC].[TC117],[COPTC].[TC118],[COPTC].[TC119],[COPTC].[TC120]
                ,[COPTC].[TC121],[COPTC].[TC122],[COPTC].[TC123],[COPTC].[TC124],[COPTC].[TC125],[COPTC].[TC126],[COPTC].[TC127],[COPTC].[TC128],[COPTC].[TC129],[COPTC].[TC130]
                ,[COPTC].[TC131],[COPTC].[TC132],[COPTC].[TC133],[COPTC].[TC134],[COPTC].[TC135],[COPTC].[TC136],[COPTC].[TC137],[COPTC].[TC138],[COPTC].[TC139],[COPTC].[TC140]
                ,[COPTC].[TC141],[COPTC].[TC142],[COPTC].[TC143],[COPTC].[TC144],[COPTC].[TC145],[COPTC].[TC146]
                ,[COPTC].[UDF01],[COPTC].[UDF02],[COPTC].[UDF03],[COPTC].[UDF04],[COPTC].[UDF05],[COPTC].[UDF06],[COPTC].[UDF07],[COPTC].[UDF08],[COPTC].[UDF09],[COPTC].[UDF10]
                ,[COPTD].[TD001],[COPTD].[TD002],[COPTD].[TD003],[COPTD].[TD004],[COPTD].[TD005],[COPTD].[TD006],[COPTD].[TD007],[COPTD].[TD008],[COPTD].[TD009],[COPTD].[TD010]
                ,[COPTD].[TD011],[COPTD].[TD012],[COPTD].[TD013],[COPTD].[TD014],[COPTD].[TD015],[COPTD].[TD016],[COPTD].[TD017],[COPTD].[TD018],[COPTD].[TD019],[COPTD].[TD020]
                ,[COPTD].[TD021],[COPTD].[TD022],[COPTD].[TD023],[COPTD].[TD024],[COPTD].[TD025],[COPTD].[TD026],[COPTD].[TD027],[COPTD].[TD028],[COPTD].[TD029],[COPTD].[TD030]
                ,[COPTD].[TD031],[COPTD].[TD032],[COPTD].[TD033],[COPTD].[TD034],[COPTD].[TD035],[COPTD].[TD036],[COPTD].[TD037],[COPTD].[TD038],[COPTD].[TD039],[COPTD].[TD040]
                ,[COPTD].[TD041],[COPTD].[TD042],[COPTD].[TD043],[COPTD].[TD044],[COPTD].[TD045],[COPTD].[TD046],[COPTD].[TD047],[COPTD].[TD048],[COPTD].[TD049],[COPTD].[TD050]
                ,[COPTD].[TD051],[COPTD].[TD052],[COPTD].[TD053],[COPTD].[TD054],[COPTD].[TD055],[COPTD].[TD056],[COPTD].[TD057],[COPTD].[TD058],[COPTD].[TD059],[COPTD].[TD060]
                ,[COPTD].[TD061],[COPTD].[TD062],[COPTD].[TD063],[COPTD].[TD064],[COPTD].[TD065],[COPTD].[TD066],[COPTD].[TD067],[COPTD].[TD068],[COPTD].[TD069],[COPTD].[TD070]
                ,[COPTD].[TD071],[COPTD].[TD072],[COPTD].[TD073],[COPTD].[TD074],[COPTD].[TD075],[COPTD].[TD076],[COPTD].[TD077],[COPTD].[TD078],[COPTD].[TD079],[COPTD].[TD080]
                ,[COPTD].[TD081],[COPTD].[TD082],[COPTD].[TD083],[COPTD].[TD084],[COPTD].[TD085],[COPTD].[TD086],[COPTD].[TD087],[COPTD].[TD088],[COPTD].[TD089],[COPTD].[TD090]
                ,[COPTD].[TD091],[COPTD].[TD092],[COPTD].[TD093],[COPTD].[TD094],[COPTD].[TD095],[COPTD].[TD096],[COPTD].[TD097],[COPTD].[TD098],[COPTD].[TD099],[COPTD].[TD100]
                ,[COPTD].[TD101],[COPTD].[TD102],[COPTD].[TD103],[COPTD].[TD104],[COPTD].[TD105],[COPTD].[TD106],[COPTD].[TD107],[COPTD].[TD108],[COPTD].[TD109],[COPTD].[TD110]
                ,[COPTD].[TD111],[COPTD].[TD112],[COPTD].[TD113]
                ,[COPTD].[UDF01] AS 'COPTDUDF01',[COPTD].[UDF02] AS 'COPTDUDF02',[COPTD].[UDF03] AS 'COPTDUDF03',[COPTD].[UDF04] AS 'COPTDUDF04',[COPTD].[UDF05] AS 'COPTDUDF05',[COPTD].[UDF06] AS 'COPTDUDF06',[COPTD].[UDF07] AS 'COPTDUDF07',[COPTD].[UDF08] AS 'COPTDUDF08',[COPTD].[UDF09] AS 'COPTDUDF09',[COPTD].[UDF10] AS 'COPTDUDF10',[COPTD].[TD200] AS 'TD200'
                ,[TB_EB_USER].USER_GUID,NAME
                ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TC006) AS 'MV002'
                ,(SELECT TOP 1 MA002 FROM [TK].dbo.COPMA WHERE MA001=TC004) AS 'MA002'
                ,(SELECT TOP 1 COPMA.UDF04 FROM [TK].dbo.COPMA,[TK].dbo.CMSMV WHERE COPMA.UDF04=CMSMV.MV001 AND COPMA.MA001=TC004) AS 'BA'
                ,(SELECT TOP 1 CMSMV.MV002 FROM [TK].dbo.COPMA,[TK].dbo.CMSMV WHERE COPMA.UDF04=CMSMV.MV001 AND COPMA.MA001=TC004) AS 'BANAME'
                ,ME002
                ,MQ002
                FROM [TK].dbo.COPTD,[TK].dbo.COPTC
                LEFT JOIN [192.168.1.223].[{0}].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= TC006 COLLATE Chinese_Taiwan_Stroke_BIN
                LEFT JOIN [TK].dbo.CMSME ON ME001=TC005
                LEFT JOIN [TK].dbo.CMSMQ ON TC001=MQ001
                WHERE TC001=TD001 AND TC002=TD002
                AND TC001=@TC001 AND TC002=@TC002
            ) AS TEMP", DBNAME);

        return QueryOrNull(db, sql);
    }

    /// <summary>UOF 人員所屬部門（只取有部門代號 GROUP_CODE 的部門）</summary>
    public DataTable SEARCHUOFDEP(string ACCOUNT)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@ACCOUNT", ACCOUNT);

        string sql = string.Format(@"
            SELECT
            [GROUP_NAME] AS 'DEPNAME'
            ,[TB_EB_EMPL_DEP].[GROUP_ID]+','+[GROUP_NAME]+',False' AS 'DEPNO'
            ,[TB_EB_USER].[USER_GUID],[ACCOUNT],[NAME],[TB_EB_EMPL_DEP].[GROUP_ID],[TITLE_ID],[GROUP_NAME],[GROUP_CODE]
            FROM [192.168.1.223].[{0}].[dbo].[TB_EB_USER],[192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP],[192.168.1.223].[{0}].[dbo].[TB_EB_GROUP]
            WHERE [TB_EB_USER].[USER_GUID]=[TB_EB_EMPL_DEP].[USER_GUID]
            AND [TB_EB_EMPL_DEP].[GROUP_ID]=[TB_EB_GROUP].[GROUP_ID]
            AND ISNULL([TB_EB_GROUP].[GROUP_CODE],'')<>''
            AND [ACCOUNT]=@ACCOUNT", DBNAME);

        return QueryOrNull(db, sql);
    }

    /// <summary>UOF 表單目前發行中的版本 ID；找不到回傳空字串</summary>
    public string SEARCHFORM_VERSION_ID(string FORM_NAME)
    {
        DatabaseHelper db = CreateDb(UOF_CONNECTION);
        db.AddParameter("@FORM_NAME", FORM_NAME);

        DataTable dt = QueryOrNull(db, @"
            SELECT TOP 1 RTRIM(LTRIM(TB_WKF_FORM_VERSION.FORM_VERSION_ID)) FORM_VERSION_ID
            FROM [UOF].dbo.TB_WKF_FORM_VERSION,[UOF].dbo.TB_WKF_FORM
            WHERE TB_WKF_FORM_VERSION.FORM_ID=TB_WKF_FORM.FORM_ID
            AND TB_WKF_FORM_VERSION.ISSUE_CTL=1
            AND FORM_NAME=@FORM_NAME
            ORDER BY TB_WKF_FORM_VERSION.FORM_ID,TB_WKF_FORM_VERSION.VERSION DESC");

        return dt != null ? dt.Rows[0]["FORM_VERSION_ID"].ToString() : "";
    }

    /// <summary>訂單客代與訂單金額（未稅+稅額）</summary>
    public DataTable FINDCOPTCTC004(string TC001TC002)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@TC001TC002", TC001TC002);

        DataTable dt = Query(db, @"
            SELECT TOP 1 TC004,SUM(TC029+TC030) AS COPTCMONEYS
            FROM [TK].dbo.COPTC
            WHERE TC001+TC002=@TC001TC002
            GROUP BY TC004");

        return dt.Rows.Count > 0 ? dt : null;
    }

    /// <summary>客戶（含同歸屬客戶 MA065）目前已使用的信用額度</summary>
    public decimal FINDCREDITS(string MA001)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@MA001", MA001);

        DataTable dt = Query(db, @"
            SELECT MA001,ISNULL(SUM(NOT_TC_SUMFINAL),0) AS 'TOTALSUM'
            FROM
            (
                SELECT MA001,'未兌現應收票據' AS KIND
                ,ISNULL(SUM(ROUND(ISNULL(TC003,0)*ISNULL(TC027,0),0)),0) AS NOT_TC_SUM
                ,ISNULL(SUM(ROUND(ISNULL(TC003,0)*ISNULL(TC027,0),0)),0)*MA091 AS NOT_TC_SUMFINAL
                FROM [TK].dbo.NOTTC AS NOTTC
                INNER JOIN [TK].dbo.COPMA AS COPMA ON TC013=MA001
                WHERE TC012 IN ('1','2','3','5','9','A')
                AND TC003 IS NOT NULL AND TC027 IS NOT NULL
                AND (MA065=@MA001 OR MA001=@MA001)
                GROUP BY MA001,MA091

                UNION ALL
                SELECT MA001,'應收帳款'
                ,ISNULL(SUM((TA041+TA042-TA058)*ISNULL(MQ010,0)),0) AS ACR_TA_SUM
                ,ISNULL(SUM((TA041+TA042-TA058)*ISNULL(MQ010,0)),0)*MA092 AS ACR_TA_SUMFINAL
                FROM [TK].dbo.ACRTA AS ACRTA
                INNER JOIN [TK].dbo.COPMA AS COPMA ON TA004=MA001
                LEFT JOIN [TK].dbo.CMSMQ AS CMSMQ ON MQ001=TA001
                WHERE TA027='N' AND TA025='Y'
                AND (MA065=@MA001 OR MA001=@MA001)
                GROUP BY MA001,MA092

                UNION ALL
                SELECT MA001,'未結帳銷貨'
                ,ISNULL(SUM(ISNULL(TH037,0)+ISNULL(TH038,0)),0) AS TH013A
                ,ISNULL(SUM(ISNULL(TH037,0)+ISNULL(TH038,0)),0)*MA093 AS TH013AFINAL
                FROM [TK].dbo.COPTG AS COPTG
                INNER JOIN [TK].dbo.COPTH AS COPTH ON TG001=TH001 AND TG002=TH002
                INNER JOIN [TK].dbo.COPMA AS COPMA ON TG004=MA001
                WHERE TG023='Y' AND TH026='N'
                AND (MA065=@MA001 OR MA001=@MA001)
                AND TG034<>'Y'
                GROUP BY MA001,MA093

                UNION ALL
                SELECT MA001,'未結帳銷退1'
                ,ISNULL(SUM(ISNULL(TJ033,0)+ISNULL(TJ034,0)),0) AS TJ012A
                ,ISNULL(SUM(ISNULL(TJ033,0)+ISNULL(TJ034,0)),0)*MA093*-1 AS TJ012AFINAL
                FROM [TK].dbo.COPTI AS COPTI
                INNER JOIN [TK].dbo.COPTJ AS COPTJ ON TI001=TJ001 AND TI002=TJ002
                INNER JOIN [TK].dbo.COPTG AS COPTG ON TG001=TJ015 AND TG002=TJ016
                INNER JOIN [TK].dbo.COPMA AS COPMA ON MA001=TI004
                WHERE TI019='Y' AND TJ024='N'
                AND (MA065=@MA001 OR MA001=@MA001)
                AND TG034<>'Y'
                GROUP BY MA001,MA093

                UNION ALL
                SELECT MA001,'未結帳銷退2'
                ,ISNULL(SUM(ISNULL(TJ033,0)+ISNULL(TJ034,0)),0) AS TJ012A
                ,ISNULL(SUM(ISNULL(TJ033,0)+ISNULL(TJ034,0)),0)*MA093*-1 AS TJ012AFINAL
                FROM [TK].dbo.COPTI AS COPTI
                INNER JOIN [TK].dbo.COPTJ AS COPTJ ON TI001=TJ001 AND TI002=TJ002
                INNER JOIN [TK].dbo.COPMA AS COPMA ON MA001=TI004
                WHERE TI019='Y' AND TJ024='N'
                AND (MA065=@MA001 OR MA001=@MA001)
                AND (TJ015+TJ016='')
                GROUP BY MA001,MA093

                UNION ALL
                SELECT MA001,'未出貨訂單'
                ,ISNULL(SUM(ROUND(ISNULL(TD011,0)*ISNULL(TC009,0)*(ISNULL(TD008,0)-ISNULL(TD009,0))*ISNULL(TD026,0),0)),0) AS TD011A
                ,ISNULL(SUM(ROUND(ISNULL(TD011,0)*ISNULL(TC009,0)*(ISNULL(TD008,0)-ISNULL(TD009,0))*ISNULL(TD026,0),0)),0)*MA094 AS TD011AFINAL
                FROM [TK].dbo.COPTC AS COPTC
                INNER JOIN [TK].dbo.COPTD AS COPTD ON TC001=TD001 AND TC002=TD002
                INNER JOIN [TK].dbo.COPMA AS COPMA ON TC004=MA001
                WHERE TC027='Y' AND TD016='N'
                AND (MA065=@MA001 OR MA001=@MA001)
                GROUP BY MA001,MA094

                UNION ALL
                SELECT MA001,'未歸還暫出'
                ,ISNULL(SUM(ISNULL(TG012,0)*ISNULL(TF012,0)*(ISNULL(TG009,0)-ISNULL(TG021,0)-ISNULL(TG020,0))),0) AS TG012A
                ,ISNULL(SUM(ISNULL(TG012,0)*ISNULL(TF012,0)*(ISNULL(TG009,0)-ISNULL(TG021,0)-ISNULL(TG020,0))),0)*MA132 AS TG012AFINAL
                FROM [TK].dbo.INVTF INVTF
                INNER JOIN [TK].dbo.INVTG INVTG ON TF001=TG001 AND TG002=TF002
                INNER JOIN [TK].dbo.COPMA AS COPMA ON TF005=MA001
                INNER JOIN [TK].dbo.CMSMQ CMSMQ ON MQ001=TG001
                WHERE TF020='Y' AND TF004='1' AND TG024='N'
                AND (MA065=@MA001 OR MA001=@MA001)
                AND MQ003='13' AND (TG014='' AND TG015='' AND TG016='')
                GROUP BY MA001,MA132
            ) AS TEMP
            GROUP BY MA001");

        return dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0]["TOTALSUM"].ToString()) : 0;
    }

    /// <summary>客戶信用額度上限 = 信用額度 × (1 + 可超出率)</summary>
    public decimal FINDCOPMATOTALLIMITS(string MA001)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@MA001", MA001);

        DataTable dt = Query(db, @"
            SELECT MA033*(1+MA034) AS 'TOTALLIMITS'
            FROM [TK].dbo.COPMA
            WHERE MA001=@MA001");

        return dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0]["TOTALLIMITS"].ToString()) : 0;
    }

    /// <summary>UOF 中簽核進行中（TASK_STATUS=1）的同一張訂單</summary>
    public DataTable CHECK_TB_WKF_TASK(string TC001TC002)
    {
        DatabaseHelper db = CreateDb(UOF_CONNECTION);
        db.AddParameter("@TC001", TC001TC002.Substring(0, 4));
        db.AddParameter("@TC002", TC001TC002.Substring(4, 11));

        DataTable dt = Query(db, @"
            SELECT CURRENT_DOC
            ,CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TC001""]/@fieldValue)[1]', 'nvarchar(max)') AS TC001Value
            ,CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TC002""]/@fieldValue)[1]', 'nvarchar(max)') AS TC002Value
            FROM [UOF].[dbo].TB_WKF_TASK
            WHERE TASK_STATUS = '1'
            AND CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TC001""]/@fieldValue)[1]', 'nvarchar(max)') LIKE '%' + @TC001 + '%'
            AND CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TC002""]/@fieldValue)[1]', 'nvarchar(max)') LIKE '%' + @TC002 + '%'");

        return dt.Rows.Count > 0 ? dt : null;
    }

    /// <summary>訂單單身中屬於烘焙品（ZINVMBBAKING）的明細</summary>
    public DataTable CHECK_COPTD_ZINVMBBAKING(string TC001, string TC002)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@TC001", TC001);
        db.AddParameter("@TC002", TC002);

        return QueryOrNull(db, @"
            SELECT *
            FROM [TK].dbo.COPTD
            WHERE TD001=@TC001 AND TD002=@TC002
            AND TD004 IN (SELECT [MB001] FROM [TK].[dbo].[ZINVMBBAKING])");
    }

    #endregion

    #region 共用

    public void MsgBox(String ex, Page pg, Object obj)
    {
        string script = "alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "');";
        ScriptManager.RegisterStartupScript(pg, obj.GetType(), "AlertScript", script, true);
    }

    #endregion
}
