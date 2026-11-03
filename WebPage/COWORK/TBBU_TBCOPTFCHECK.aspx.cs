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
/// 訂單變更協調（COPTE/COPTF）
/// 分頁：1.訂單變更協調  2.生管用  3.採購用  4.業務用  5.不需生產庫存出貨  6.管理者送簽  7.變更單沒有明細
/// 本頁排除烘焙品（分頁 1~3）；烘焙品請用 TBBU_TBCOPTFCHECK_BAKING。
/// </summary>
public partial class CDS_WebPage_COP_TBBU_TBCOPTFCHECK : Ede.Uof.Utility.Page.BasePage
{
    #region 常數與欄位

    private const string ERP_CONNECTION = "ERPconnectionstring";
    private const string UOF_CONNECTION = "connectionstring";

    /// <summary>UOF 資料庫名稱（測試時可改為 UOFTEST）</summary>
    private const string DBNAME = "UOF";

    /// <summary>送簽的 UOF 表單名稱</summary>
    private const string UOF_FORM_NAME = "COP20.訂單變更單";

    private const string DIALOG_EDIT_URL = "~/CDS/WebPage/COWORK/TBBU_TBCOPTFCHECKDialogEDIT.aspx";

    private const string STATUS_UNCONFIRMED = "未核單";
    private const string STATUS_CONFIRMED = "已核單";

    private string ACCOUNT = null;
    private string NAME = null;
    private string ROLES = null;

    // 送簽時使用的變更單單別、單號、變更版次（訊息顯示用）
    private string TF001 = "";
    private string TF002 = "";
    private string TF003 = "";

    #endregion

    #region 查詢條件區塊

    /// <summary>
    /// 每個分頁的查詢條件控制項。
    /// 頁面上的控制項 ID 沿用原本的編號，這裡集中對應成有意義的名稱；沒有的條件為 null。
    /// </summary>
    private class SearchPanel
    {
        public TextBox Year;            // 單號前 4 碼（年度）
        public DropDownList Status;     // 未核單/已核單（TE029）
        public DropDownList Produce;    // 是否生產 Y/N
        public TextBox OrderNo;
        public TextBox Customer;
        public TextBox Product;
        public GridView Grid;
        public bool BakingFilter;       // 是否套用烘焙品篩選
    }

    /// <summary>1.訂單變更協調</summary>
    private SearchPanel PanelOrder
    {
        get
        {
            return new SearchPanel
            {
                Year = TextBox1, Status = DropDownList1, Produce = DropDownList2,
                OrderNo = TextBox9, Customer = TextBox24, Product = TextBox25, Grid = Grid1,
                BakingFilter = true
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
                Year = TextBox3, Status = DropDownList3, Produce = DropDownList4,
                OrderNo = TextBox10, Customer = TextBox2, Product = TextBox6, Grid = Grid2,
                BakingFilter = true
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
                Year = TextBox5, Status = DropDownList5, Produce = DropDownList6,
                OrderNo = TextBox11, Customer = TextBox8, Product = TextBox14, Grid = Grid3,
                BakingFilter = true
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
                Year = TextBox7, Status = DropDownList7, Produce = DropDownList8,
                OrderNo = TextBox12, Customer = TextBox16, Product = TextBox17, Grid = Grid4
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
                Year = TextBox13, Status = DropDownList9,
                OrderNo = TextBox15, Customer = TextBox18, Product = TextBox19, Grid = Grid5
            };
        }
    }

    /// <summary>6.管理者直接送簽（只有 ADMIN 看得到）</summary>
    private SearchPanel PanelAdmin
    {
        get
        {
            return new SearchPanel { OrderNo = TextBox4, Grid = Grid6 };
        }
    }

    /// <summary>分頁 1~5 一起重新查詢（批次更新後各分頁的狀態都會變）</summary>
    private void BindMainPanels()
    {
        foreach (SearchPanel panel in new[] { PanelOrder, PanelMoc, PanelPur, PanelSales, PanelStock })
        {
            BindPanel(panel);
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

            string year = DateTime.Now.ToString("yyyy");
            foreach (TextBox txtYear in new[] { TextBox1, TextBox3, TextBox5, TextBox7, TextBox13, TextBox26 })
            {
                txtYear.Text = year;
            }

            foreach (DropDownList ddl in new[] { DropDownList1, DropDownList3, DropDownList5, DropDownList7, DropDownList9, DropDownList10 })
            {
                FillStatusItems(ddl);
            }
            foreach (DropDownList ddl in new[] { DropDownList2, DropDownList4, DropDownList6, DropDownList8 })
            {
                FillYesNoItems(ddl);
            }

            // 進頁面先載入 1.訂單變更協調、2.生管用、7.訂單變更單未填明細
            BindPanel(PanelOrder);
            BindPanel(PanelMoc);
            BindGrid7();
        }
    }

    /// <summary>
    /// 依角色開放批次更新按鈕：Button4=生管、Button6=採購、Button8=業務；
    /// 管理者另外可看到分頁 6。
    /// </summary>
    private void ApplyRolePermissions()
    {
        Button4.Enabled = false;
        RadPageView6.Visible = false;

        switch (ROLES)
        {
            case "ADMIN":
                SetBatchButtons(true, true, true);
                RadPageView6.Visible = true;
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
    /// 每筆變更單明細最新一筆協調紀錄（TBCOPTFCHECK 依 ID 取最新）
    /// </summary>
    private const string LATEST_CHECK_COLUMNS = @"
        ,(SELECT TOP 1 ISNULL(MOCCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKDATES'
        ,(SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKS'
        ,(SELECT TOP 1 ISNULL(MOCCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
        ,(SELECT TOP 1 ISNULL(PURCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKDATES'
        ,(SELECT TOP 1 ISNULL(PURCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKS'
        ,(SELECT TOP 1 ISNULL(PURCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
        ,(SELECT TOP 1 ISNULL(SALESCHECKDATES,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKDATES'
        ,(SELECT TOP 1 ISNULL(SALESCHECKSCOMMENTS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'
        ,COPTF.UDF01 AS 'COPTFUDF01'";

    /// <summary>
    /// 依分頁的查詢條件撈變更單明細並綁定到 Grid（分頁 1~6）
    /// </summary>
    private void BindPanel(SearchPanel panel)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        StringBuilder where = new StringBuilder();

        // 年度（單號前 4 碼）
        if (panel.Year != null && !string.IsNullOrEmpty(panel.Year.Text))
        {
            where.Append(" AND TF002 LIKE @YEAR + '%'");
            db.AddParameter("@YEAR", panel.Year.Text.Trim());
        }

        // 是否核單
        if (panel.Status != null)
        {
            AppendStatusFilter(where, panel.Status);
        }

        // 是否生產
        if (panel.Produce != null)
        {
            if (panel.Produce.Text == "Y")
            {
                where.Append(" AND COPTF.UDF01 IN ('Y','y')");
            }
            else if (panel.Produce.Text == "N")
            {
                where.Append(" AND COPTF.UDF01 NOT IN ('Y','y')");
            }
        }

        // 訂單單號
        if (!string.IsNullOrEmpty(panel.OrderNo.Text))
        {
            where.Append(" AND TF002 LIKE @ORDERNO + '%'");
            db.AddParameter("@ORDERNO", panel.OrderNo.Text.Trim());
        }

        // 客戶名稱
        if (panel.Customer != null && !string.IsNullOrEmpty(panel.Customer.Text))
        {
            where.Append(" AND TC053 LIKE '%' + @CUSTOMER + '%'");
            db.AddParameter("@CUSTOMER", panel.Customer.Text.Trim());
        }

        // 品名
        if (panel.Product != null && !string.IsNullOrEmpty(panel.Product.Text))
        {
            where.Append(" AND TD005 LIKE '%' + @PRODUCT + '%'");
            db.AddParameter("@PRODUCT", panel.Product.Text.Trim());
        }

        // 烘焙品
        if (panel.BakingFilter)
        {
            AppendBakingFilter(where);
        }

        string sql = @"
            SELECT
             LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
            ,LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003)) AS 'TF123'
            ,*
            " + LATEST_CHECK_COLUMNS + @"
            FROM [TK].dbo.COPTE,[TK].dbo.COPTF
            LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
            LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
            WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
            " + where;

        panel.Grid.DataSource = Query(db, sql);
        panel.Grid.DataBind();
    }

    /// <summary>
    /// 7.訂單變更單沒有明細（或明細沒有序號）的單頭
    /// </summary>
    private void BindGrid7()
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        StringBuilder where = new StringBuilder();

        AppendStatusFilter(where, DropDownList10);

        if (!string.IsNullOrEmpty(TextBox26.Text))
        {
            where.Append(" AND TE002 LIKE @YEAR + '%'");
            db.AddParameter("@YEAR", TextBox26.Text.Trim());
        }
        if (!string.IsNullOrEmpty(TextBox22.Text))
        {
            where.Append(" AND TE002 LIKE @ORDERNO + '%'");
            db.AddParameter("@ORDERNO", TextBox22.Text.Trim());
        }
        if (!string.IsNullOrEmpty(TextBox23.Text))
        {
            where.Append(" AND TC053 LIKE '%' + @CUSTOMER + '%'");
            db.AddParameter("@CUSTOMER", TextBox23.Text.Trim());
        }

        string sql = @"
            SELECT
             LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))+LTRIM(RTRIM(TF004)) AS 'TF1234'
            ,LTRIM(RTRIM(TE001))+LTRIM(RTRIM(TE002))+LTRIM(RTRIM(TE003)) AS 'TF123'
            ,*
            " + LATEST_CHECK_COLUMNS + @"
            FROM [TK].dbo.COPTE
            LEFT JOIN [TK].dbo.COPTF ON TE001=TF001 AND TE002=TF002 AND TE003=TF003
            LEFT JOIN [TK].dbo.COPTC ON TC001=TE001 AND TC002=TE002
            LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
            WHERE ISNULL(TF004,'')=''
            " + where;

        Grid7.DataSource = Query(db, sql);
        Grid7.DataBind();
    }

    private static void AppendStatusFilter(StringBuilder where, DropDownList ddl)
    {
        if (ddl.Text == STATUS_UNCONFIRMED)
        {
            where.Append(" AND TE029='N'");
        }
        else if (ddl.Text == STATUS_CONFIRMED)
        {
            where.Append(" AND TE029='Y'");
        }
    }

    /// <summary>
    /// 排除烘焙品：品號開頭符合烘焙品清單（MOCMANULINEMB001LIKES）的明細不顯示
    /// </summary>
    private static void AppendBakingFilter(StringBuilder where)
    {
        where.Append(" AND NOT EXISTS (SELECT 1 FROM [TKMOC].[dbo].[MOCMANULINEMB001LIKES] AS BAKING WHERE TF005 LIKE BAKING.MB001 + '%')");
    }

    #endregion

    #region Grid 事件

    // 分頁大小設為 1000，不需處理換頁
    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e) { }
    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e) { }
    protected void grid_PageIndexChanging3(object sender, GridViewPageEventArgs e) { }
    protected void grid_PageIndexChanging4(object sender, GridViewPageEventArgs e) { }
    protected void grid_PageIndexChanging5(object sender, GridViewPageEventArgs e) { }
    protected void grid_PageIndexChanging6(object sender, GridViewPageEventArgs e) { }
    protected void grid_PageIndexChanging7(object sender, GridViewPageEventArgs e) { }

    // 匯出 Excel 使用 Grid 預設內容
    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }
    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }
    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }
    public void OnBeforeExport4(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }
    public void OnBeforeExport5(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }
    public void OnBeforeExport6(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }
    public void OnBeforeExport7(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e) { }

    /// <summary>
    /// 1.訂單變更協調：「更新」開協調視窗；「送簽」只在不需生產、或生管已核准時顯示
    /// </summary>
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
        {
            return;
        }

        Button btnEdit = (Button)e.Row.FindControl("Button1");
        ExpandoObject param = new { ID = btnEdit.CommandArgument }.ToExpando();
        Dialog.Open2(btnEdit, DIALOG_EDIT_URL, "", 800, 600, Dialog.PostBackType.Allows, param);

        Label lblProduce = (Label)e.Row.FindControl("Label_COPTFUDF01");
        Label lblMocCheck = (Label)e.Row.FindControl("Label_MOCCHECKS");
        Button btnSend = (Button)e.Row.FindControl("Button2");

        bool needProduce = lblProduce.Text.Equals("Y");
        btnSend.Visible = !needProduce || lblMocCheck.Text.Equals("Y");
    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Button1")
        {
            BindPanel(PanelOrder);
        }
        else if (e.CommandName == "Button2")
        {
            string changeNo = e.CommandArgument.ToString();

            if (IsInSigning(changeNo))
            {
                MsgBox(changeNo + "重覆送單 \r\n 此訂單已送出簽核了", this.Page, this);
                return;
            }

            // 檢查並送出 UOF
            CHECKTBCOPTFCHECK(changeNo);

            // 送單後提示信用額度
            ShowCreditWarning(changeNo);
        }
    }

    /// <summary>2.生管用：下拉預設為目前的生管核准狀態</summary>
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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
            CHECKTBCOPTFCHECK(e.CommandArgument.ToString());
        }
    }

    /// <summary>3.採購用：下拉預設為目前的採購核准狀態</summary>
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        SelectRowDropDown(e, "GRIDVIEW2DropDownList1", "PURCHECKS");
    }

    protected void Grid3_RowCommand(object sender, GridViewCommandEventArgs e) { }

    protected void Grid4_RowDataBound(object sender, GridViewRowEventArgs e) { }

    protected void Grid4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "GRIDVIEW4Button1")
        {
            CHECKTBCOPTFCHECK(e.CommandArgument.ToString());
        }
    }

    protected void Grid5_RowDataBound(object sender, GridViewRowEventArgs e) { }

    protected void Grid5_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "GRIDVIEW5Button1")
        {
            CHECKTBCOPTFCHECK2(e.CommandArgument.ToString());
        }
    }

    protected void Grid6_RowDataBound(object sender, GridViewRowEventArgs e) { }

    protected void Grid6_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "GRIDVIEW6Button1")
        {
            CHECKTBCOPTFCHECK3(e.CommandArgument.ToString());
        }
    }

    protected void Grid7_RowDataBound(object sender, GridViewRowEventArgs e) { }

    protected void Grid7_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "GRIDVIEW7Button1")
        {
            string changeNo = e.CommandArgument.ToString();

            if (IsInSigning(changeNo))
            {
                MsgBox(changeNo + " 重覆送單 \r\n 此訂單已送出簽核了", this.Page, this);
                return;
            }

            CHECKTBCOPTFCHECK7(changeNo);

            ShowCreditWarning(changeNo);
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

    // 分頁 1~5 的查詢按鈕
    protected void Button1_Click(object sender, EventArgs e) { BindMainPanels(); }
    protected void Button3_Click(object sender, EventArgs e) { BindMainPanels(); }
    protected void Button5_Click(object sender, EventArgs e) { BindMainPanels(); }
    protected void Button7_Click(object sender, EventArgs e) { BindMainPanels(); }

    /// <summary>生管批次更新</summary>
    protected void Button4_Click(object sender, EventArgs e)
    {
        ADDMOC();
        BindMainPanels();
    }

    /// <summary>採購批次更新</summary>
    protected void Button6_Click(object sender, EventArgs e)
    {
        ADDPUR();
        BindMainPanels();
    }

    /// <summary>業務批次更新</summary>
    protected void Button8_Click(object sender, EventArgs e)
    {
        ADDSALES();
        BindMainPanels();
    }

    /// <summary>6.管理者查詢</summary>
    protected void Button10_Click(object sender, EventArgs e)
    {
        BindPanel(PanelAdmin);
    }

    /// <summary>7.查詢</summary>
    protected void Button11_Click(object sender, EventArgs e)
    {
        BindGrid7();
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
                ADDTBCOPTFCHECKMOC(RowKey(gvr, 1), RowKey(gvr, 2), RowKey(gvr, 5), RowKey(gvr, 4), null, ddlCheck.SelectedValue, txtComment.Text);
            }
        }
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
                ADDTBCOPTDCHECKPUR(RowKey(gvr, 1), RowKey(gvr, 2), RowKey(gvr, 5), RowKey(gvr, 4), null, ddlCheck.SelectedValue, txtComment.Text);
            }
        }
    }

    /// <summary>業務：有填備註的列，新增一筆業務備註紀錄</summary>
    public void ADDSALES()
    {
        foreach (GridViewRow gvr in Grid4.Rows)
        {
            TextBox txtComment = (TextBox)gvr.FindControl("GRIDVIEW4TextBox1");

            if (!string.IsNullOrEmpty(txtComment.Text))
            {
                ADDTBCOPTDCHECKSALES(RowKey(gvr, 1), RowKey(gvr, 2), RowKey(gvr, 5), RowKey(gvr, 4), null, txtComment.Text);
            }
        }
    }

    /// <summary>取 Grid 列的欄位：第 1 欄單別、第 2 欄單號、第 4 欄變更序號、第 5 欄變更版次</summary>
    private static string RowKey(GridViewRow row, int cellIndex)
    {
        return row.Cells[cellIndex].Text.Trim();
    }

    /// <summary>
    /// 新增協調紀錄的共用 SQL：沿用最新一筆紀錄的其他部門欄位，只寫入指定部門的欄位。
    /// </summary>
    private void InsertCheckRecord(string TF001, string TF002, string TF003, string TF004,
                                   string mocColumns, string purColumns, string salesColumns,
                                   Dictionary<string, string> parameters)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);

        string sql = @"
            INSERT INTO [TKBUSINESS].[dbo].[TBCOPTFCHECK]
            ([TF001],[TF002],[TF003],[TF004],[TF005],[TF006],[TF007],[TF009],[TF010],[TF013],[TF014],[TF015],[TF018],[TF032],[TF045],[TF104],[TE006],[TE050]
            ,[MOCCHECKDATES],[MOCCHECKS],[MOCCHECKSCOMMENTS]
            ,[PURCHECKDATES],[PURCHECKS],[PURCHECKSCOMMENTS]
            ,[SALESCHECKDATES],[SALESCHECKSCOMMENTS])
            SELECT
            [TF001],[TF002],[TF003],[TF004],[TF005],[TF006],[TF007],[TF009],[TF010],[TF013],[TF014],[TF015],[TF018],[TF032],[TF045],[TF104],[TE006],[TE050]
            " + mocColumns + @"
            " + purColumns + @"
            " + salesColumns + @"
            FROM [TK].dbo.COPTE,[TK].dbo.COPTF
            LEFT JOIN [TK].dbo.COPTC ON TC001=TF001 AND TC002=TF002
            LEFT JOIN [TK].dbo.COPTD ON TD001=TF001 AND TD002=TF002 AND TD003=TF104
            WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
            AND LTRIM(RTRIM(TF001))=@TF001
            AND LTRIM(RTRIM(TF002))=@TF002
            AND LTRIM(RTRIM(TF003))=@TF003
            AND LTRIM(RTRIM(TF004))=@TF004";

        db.AddParameter("@TF001", TF001);
        db.AddParameter("@TF002", TF002);
        db.AddParameter("@TF003", TF003);
        db.AddParameter("@TF004", TF004);
        foreach (KeyValuePair<string, string> p in parameters)
        {
            db.AddParameter(p.Key, p.Value);
        }

        db.ExecuteNonQuery(sql);
    }

    /// <summary>沿用最新一筆紀錄的欄位值（空值以空字串代替）</summary>
    private static string LatestValue(string column)
    {
        return ",(SELECT TOP 1 ISNULL(" + column + ",'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC) AS '" + column + "'";
    }

    private static string NowText()
    {
        return DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
    }

    public void ADDTBCOPTFCHECKMOC(string TF001, string TF002, string TF003, string TF004, string MOCCHECKDATES, string MOCCHECKS, string MOCCHECKSCOMMENTS)
    {
        InsertCheckRecord(TF001, TF002, TF003, TF004,
            ",@MOCCHECKDATES,@MOCCHECKS,@MOCCHECKSCOMMENTS",
            LatestValue("PURCHECKDATES") + LatestValue("PURCHECKS") + LatestValue("PURCHECKSCOMMENTS"),
            LatestValue("SALESCHECKDATES") + LatestValue("SALESCHECKSCOMMENTS"),
            new Dictionary<string, string>
            {
                { "@MOCCHECKDATES", NowText() },
                { "@MOCCHECKS", MOCCHECKS },
                { "@MOCCHECKSCOMMENTS", MOCCHECKSCOMMENTS }
            });
    }

    public void ADDTBCOPTDCHECKPUR(string TF001, string TF002, string TF003, string TF004, string PURCHECKDATES, string PURCHECKS, string PURCHECKSCOMMENTS)
    {
        InsertCheckRecord(TF001, TF002, TF003, TF004,
            LatestValue("MOCCHECKDATES") + LatestValue("MOCCHECKS") + LatestValue("MOCCHECKSCOMMENTS"),
            ",@PURCHECKDATES,@PURCHECKS,@PURCHECKSCOMMENTS",
            LatestValue("SALESCHECKDATES") + LatestValue("SALESCHECKSCOMMENTS"),
            new Dictionary<string, string>
            {
                { "@PURCHECKDATES", NowText() },
                { "@PURCHECKS", PURCHECKS },
                { "@PURCHECKSCOMMENTS", PURCHECKSCOMMENTS }
            });
    }

    public void ADDTBCOPTDCHECKSALES(string TF001, string TF002, string TF003, string TF004, string SALESCHECKDATES, string SALESCHECKSCOMMENTS)
    {
        InsertCheckRecord(TF001, TF002, TF003, TF004,
            LatestValue("MOCCHECKDATES") + LatestValue("MOCCHECKS") + LatestValue("MOCCHECKSCOMMENTS"),
            LatestValue("PURCHECKDATES") + LatestValue("PURCHECKS") + LatestValue("PURCHECKSCOMMENTS"),
            ",@SALESCHECKDATES,@SALESCHECKSCOMMENTS",
            new Dictionary<string, string>
            {
                { "@SALESCHECKDATES", NowText() },
                { "@SALESCHECKSCOMMENTS", SALESCHECKSCOMMENTS }
            });
    }

    #endregion

    #region 送簽

    /// <summary>變更單明細的 key：單別+單號+變更版次</summary>
    private const string CHANGE_KEY = "LTRIM(RTRIM(TF001))+LTRIM(RTRIM(TF002))+LTRIM(RTRIM(TF003))";

    /// <summary>
    /// 送簽（訂單變更協調、生管用、業務用）：
    /// 明細有需要生產的品項，必須生管核准後才能送簽；都不需生產則直接送簽。
    /// </summary>
    public void CHECKTBCOPTFCHECK(string TF001002003)
    {
        try
        {
            DatabaseHelper db = CreateDb(ERP_CONNECTION);
            db.AddParameter("@KEY", TF001002003);

            // 需要生產、但生管尚未核准的明細
            DataTable pending = Query(db, @"
                SELECT TF001,TF002,TF003,TF004
                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                AND COPTF.UDF01 IN ('Y','y')
                AND ISNULL((SELECT TOP 1 ISNULL(MOCCHECKS,'') FROM [TKBUSINESS].[dbo].[TBCOPTFCHECK] WHERE TBCOPTFCHECK.TF001=COPTF.TF001 AND TBCOPTFCHECK.TF002=COPTF.TF002 AND TBCOPTFCHECK.TF003=COPTF.TF003 AND TBCOPTFCHECK.TF004=COPTF.TF004 ORDER BY ID DESC),'') NOT IN ('Y')
                AND " + CHANGE_KEY + "=@KEY");

            SplitChangeNo(TF001002003);

            if (pending.Rows.Count == 0)
            {
                SendToUofIfNotSigning("  重覆送單");
            }
            else
            {
                MsgBox("送單失敗!!!! \r\n 送單失敗!!!!\r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!!       \r\n \r\n訂單的單身有需要生產的，需經生管同意" + TF001 + TF002 + TF003, this.Page, this);
            }
        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
    }

    /// <summary>
    /// 送簽（不需生產庫存出貨）：明細只要有需要生產的品項就不能從這裡送簽。
    /// </summary>
    public void CHECKTBCOPTFCHECK2(string TF001002003)
    {
        try
        {
            DatabaseHelper db = CreateDb(ERP_CONNECTION);
            db.AddParameter("@KEY", TF001002003);

            DataTable needProduce = Query(db, @"
                SELECT TF001,TF002,TF003,TF004
                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                AND COPTF.UDF01 IN ('Y','y')
                AND " + CHANGE_KEY + "=@KEY");

            SplitChangeNo(TF001002003);

            if (needProduce.Rows.Count == 0)
            {
                SendToUofIfNotSigning(" 重覆送單");
            }
            else
            {
                MsgBox("送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n 送單失敗!!!! \r\n \r\n訂單的單身有需要生產的，需經生管、採購同意" + TF001 + TF002 + TF003, this.Page, this);
            }
        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
    }

    /// <summary>
    /// 送簽（管理者）：不檢查生管核准，只要變更單有明細就送簽。
    /// </summary>
    public void CHECKTBCOPTFCHECK3(string TF001002003)
    {
        try
        {
            DatabaseHelper db = CreateDb(ERP_CONNECTION);
            db.AddParameter("@KEY", TF001002003);

            DataTable lines = Query(db, @"
                SELECT TF001,TF002,TF003,TF004
                FROM [TK].dbo.COPTE,[TK].dbo.COPTF
                WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                AND " + CHANGE_KEY + "=@KEY");

            SplitChangeNo(TF001002003);

            if (lines.Rows.Count > 0)
            {
                SendToUofIfNotSigning(" 重覆送單");
            }
        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
    }

    /// <summary>
    /// 送簽（7.變更單沒有明細）：直接送簽。
    /// </summary>
    public void CHECKTBCOPTFCHECK7(string TF001002003)
    {
        try
        {
            SplitChangeNo(TF001002003);
            ADDTB_WKF_EXTERNAL_TASK_COPTECOPTF7(TF001, TF002, TF003);
        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
    }

    /// <summary>單別 4 碼 + 單號 11 碼 + 變更版次 4 碼</summary>
    private void SplitChangeNo(string TF001002003)
    {
        TF001 = TF001002003.Substring(0, 4);
        TF002 = TF001002003.Substring(4, 11);
        TF003 = TF001002003.Substring(15, 4);
    }

    private void SendToUofIfNotSigning(string duplicateMessage)
    {
        if (IsInSigning(TF001 + TF002))
        {
            MsgBox(TF001 + TF002 + duplicateMessage + " \r\n 此訂單已送出簽核了", this.Page, this);
        }
        else
        {
            ADDTB_WKF_EXTERNAL_TASK_COPTECOPTF(TF001, TF002, TF003);
        }
    }

    private bool IsInSigning(string TC001TC002)
    {
        DataTable dt = CHECK_TB_WKF_TASK(TC001TC002);
        return dt != null && dt.Rows.Count >= 1;
    }

    /// <summary>
    /// 送單後顯示：變更金額 + 原訂單金額 - 已用信用額度 超過客戶信用額度時提示（沿用原本的計算方式）
    /// </summary>
    private void ShowCreditWarning(string TE001002003)
    {
        DataTable change = FINDCOPTE(TE001002003);
        string TC004 = change.Rows[0]["TE007"].ToString();

        int orderAmount = Convert.ToInt32(Convert.ToDecimal(change.Rows[0]["COPTCMONEYS"].ToString()));
        int changeAmount = Convert.ToInt32(Convert.ToDecimal(change.Rows[0]["COPTEMONEYS"].ToString()));
        int creditLimit = Convert.ToInt32(FINDCOPMATOTALLIMITS(TC004));
        int creditUsed = Convert.ToInt32(FINDCREDITS(TC004));

        if (creditLimit < (changeAmount + (-1 * creditUsed) + orderAmount))
        {
            MsgBox(TE001002003 + " 訂單變更金額=" + changeAmount.ToString("0,0")
                + " \r\n原訂單金額=" + orderAmount.ToString("0,0")
                + " \r\n客代:" + TC004
                + " \r\n目前設定的信用額度=" + creditLimit.ToString("0,0")
                + " \r\n已花費的信用額度=" + creditUsed.ToString("0,0"), this.Page, this);
        }
    }

    /// <summary>
    /// 組成 UOF「COP20.訂單變更單」表單 XML 並起單（變更單有明細）
    /// </summary>
    public void ADDTB_WKF_EXTERNAL_TASK_COPTECOPTF(string TE001, string TE002, string TE003)
    {
        DataTable DT = SEARCHCOPTECOPTF(TE001, TE002, TE003);
        EnsureApplicantHasDepartment(DT);

        DataTable dtBaking = CHECK_COPTF_ZINVMBBAKING(TE001, TE002, TE003);
        bool hasBaking = dtBaking != null && dtBaking.Rows.Count >= 1;

        ADDTACK(BuildChangeFormXml(DT, SEARCHFORM_VERSION_ID(UOF_FORM_NAME), false, hasBaking));
    }

    /// <summary>
    /// 組成 UOF「COP20.訂單變更單」表單 XML 並起單（7.變更單沒有明細）
    /// </summary>
    public void ADDTB_WKF_EXTERNAL_TASK_COPTECOPTF7(string TE001, string TE002, string TE003)
    {
        DataTable DT = SEARCHCOPTECOPTF7(TE001, TE002, TE003);
        EnsureApplicantHasDepartment(DT);

        ADDTACK(BuildChangeFormXml(DT, SEARCHFORM_VERSION_ID(UOF_FORM_NAME), true, false));
    }

    /// <summary>申請人必須有 UOF 部門（GROUP_CODE 不可空白），查不到時中斷送單</summary>
    private void EnsureApplicantHasDepartment(DataTable DT)
    {
        if (SEARCHUOFDEP(DT.Rows[0]["TE009"].ToString()) == null)
        {
            throw new Exception("申請人 " + DT.Rows[0]["TE009"].ToString() + " 在 UOF 沒有部門代號，無法送單");
        }
    }

    /// <summary>
    /// 依變更單資料組成 UOF 表單 XML（不連資料庫，方便檢查欄位）
    /// noDetail=true 為「7.變更單沒有明細」：不帶單別名稱、生產別固定為生產、不判斷數量/金額減少。
    /// </summary>
    private static XmlElement BuildChangeFormXml(DataTable DT, string formVersionId, bool noDetail, bool hasBaking)
    {
        DataRow head = DT.Rows[0];

        string BA = head["BA"].ToString();
        string BANAME = head["BANAME"].ToString();
        string BA_USER_GUID = head["BA_USER_GUID"].ToString();

        // 明細只要有一筆需要生產（大寫 Y），單頭就標記為需要生產
        string COPTCUDF01 = "N";
        foreach (DataRow od in DT.Rows)
        {
            if (od["COPTFUDF01"].ToString().Equals("Y"))
            {
                COPTCUDF01 = "Y";
                break;
            }
        }

        // 明細有任一筆 數量 或 金額 比變更前少，標記為 Y
        string COPCALL1 = "N";
        if (!noDetail)
        {
            foreach (DataRow od in DT.Rows)
            {
                decimal TF009 = Convert.ToDecimal(od["TF009"].ToString());
                decimal TF109 = Convert.ToDecimal(od["TF109"].ToString());
                decimal TF014 = Convert.ToDecimal(od["TF014"].ToString());
                decimal TF114 = Convert.ToDecimal(od["TF114"].ToString());
                if (TF009 < TF109 || TF014 < TF114)
                {
                    COPCALL1 = "Y";
                    break;
                }
            }
        }

        UofFormBuilder fb = new UofFormBuilder(formVersionId, head["TE009"].ToString(), head["GROUP_ID"].ToString(), head["TITLE_ID"].ToString(),
                                               head["NAME"].ToString(), head["USER_GUID"].ToString());

        fb.AddField("ID", "");
        if (!noDetail)
        {
            fb.AddField("TE001NAMES", head["MQ002"].ToString(), "", "@null");
        }
        fb.AddField("CHECKMOC", hasBaking ? "烘培" : "生產", "", "@null");
        fb.AddField("TE006", head["TE006"].ToString());
        fb.AddField("TE001", head["TE001"].ToString());
        fb.AddField("TE002", head["TE002"].ToString());
        fb.AddField("TE003", head["TE003"].ToString());
        fb.AddField("COPTCUDF01", COPTCUDF01);
        fb.AddField("COPCALL1", COPCALL1);
        fb.AddField("TE038", head["TE038"].ToString());
        fb.AddField("TE007", head["TE007"].ToString());
        fb.AddField("MA002", head["MA002"].ToString());
        fb.AddField("TE011", head["TE011"].ToString());
        fb.AddField("TE111", head["TE111"].ToString());
        fb.AddField("TE012", head["TE012"].ToString());
        fb.AddField("TE112", head["TE112"].ToString());
        fb.AddField("TE041", head["TE041"].ToString());
        fb.AddField("TE041C", head["TE017"].ToString());
        fb.AddField("TE137", head["TE137"].ToString());
        fb.AddField("TE137C", head["TE117"].ToString());
        fb.AddField("TE015", head["TE015"].ToString());
        fb.AddField("TE115", head["TE115"].ToString());
        fb.AddField("TE018", head["NEWTE018"].ToString());
        fb.AddField("TE118", head["NEWTE118"].ToString());
        fb.AddField("TE008", head["TE008"].ToString());
        fb.AddField("CMSME002A", head["CMSME002A"].ToString());
        fb.AddField("TE108", head["TE108"].ToString());
        fb.AddField("CMSME002B", head["CMSME002B"].ToString());
        // 注意：原本申請人欄位的 realValue 就是帶業務窗口（BA）的 USER_GUID
        fb.AddField("TE009", head["CMSMV002A"].ToString() + "(" + head["TE009"].ToString() + ")", UofFormBuilder.UserSet(BA_USER_GUID));
        fb.AddField("CMSMV002A", head["CMSMV002A"].ToString());
        fb.AddField("TE109", head["TE109"].ToString());
        fb.AddField("CMSMV002B", head["CMSMV002B"].ToString());
        fb.AddField("BA", BANAME + "(" + BA + ")", UofFormBuilder.UserSet(BA_USER_GUID));
        fb.AddField("BANAME", BANAME);
        fb.AddField("TE040", head["TE040"].ToString());
        fb.AddField("TE136", head["TE136"].ToString());
        fb.AddField("TE013", head["TE013"].ToString());
        fb.AddField("TE113", head["TE113"].ToString());
        fb.AddField("TE050", head["TE050"].ToString());
        fb.AddField("UDF05", head["COPTCUDF05"].ToString());
        fb.AddField("MOC", "");
        fb.AddField("PUR", "");
        XmlElement details = fb.AddField("DETAILS", "");

        // 明細
        string[] detailColumns = { "COTTDUDF01", "COPTFUDF01", "TF004", "TF005", "TF006", "TF007", "TF009", "TF020", "TF010", "TF013", "TF021", "TF014", "TF015", "TF024", "TF025", "TF018",
                                   "TF105", "TF106", "TF107", "TF109", "TF120", "TF110", "TF113", "TF121", "TF114", "TF115", "TF126", "TF127" };
        XmlElement grid = fb.AddDataGrid(details);
        int order = 0;
        foreach (DataRow od in DT.Rows)
        {
            XmlElement row = fb.AddRow(grid, order++);
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

            Logger.Write("起單", "起單成功!" + formNBR);
            MsgBox("起單成功 \r\n" + TF001 + TF002 + TF003 + " > " + formNBR, this.Page, this);
        }
        else
        {
            string status = "起單失敗!";
            string error = resultXE.Element("Exception").Element("Message").Value;

            Logger.Write("起單", status + error + "\r\n" + Form.OuterXml);
            MsgBox("失敗了!!!!   " + error + "\r\n" + Form.OuterXml, this.Page, this);

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

    /// <summary>變更單單頭＋明細（含 UOF 人員、部門、業務窗口），送簽組表單用</summary>
    public DataTable SEARCHCOPTECOPTF(string TE001, string TE002, string TE003)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@TE001", TE001);
        db.AddParameter("@TE002", TE002);
        db.AddParameter("@TE003", TE003);

        string sql = string.Format(@"
            SELECT
            ISNULL((SELECT TOP 1 UDF01 FROM  [TK].dbo.COPTD WHERE TD001=TF001 AND TD002=TF002 AND TD003=TF104),'') AS COTTDUDF01
            ,COPTFUDF01 AS COPTFUDF01
            ,CASE WHEN COPTFUDF01 IN ('N','n','') AND ISNULL((SELECT TOP 1 UDF01 FROM  [TK].dbo.COPTD WHERE TD001=TF001 AND TD002=TF002 AND TD003=TF104),'N') IN ('N','n','') THEN 'N' ELSE 'Y' END AS 'COPTFUDF01CHECK'
            ,[COMPANY],[CREATOR],[USR_GROUP],[CREATE_DATE],[MODIFIER],[MODI_DATE],[FLAG],[CREATE_TIME],[MODI_TIME],[TRANS_TYPE],[TRANS_NAME]
            ,[sync_date],[sync_time],[sync_mark],[sync_count],[DataUser],[DataGroup]
            ,[TE001],[TE002],[TE003],[TE004],[TE005],[TE006],[TE007],[TE008],[TE009],[TE010]
            ,[TE011],[TE012],[TE013],[TE014],[TE015],[TE016],[TE017],[TE018],[TE019],[TE020]
            ,[TE021],[TE022],[TE023],[TE024],[TE025],[TE026],[TE027],[TE028],[TE029],[TE030]
            ,[TE031],[TE032],[TE033],[TE034],[TE035],[TE036],[TE037],[TE038],[TE039],[TE040]
            ,[TE041],[TE042],[TE043],[TE044],[TE045],[TE046],[TE047],[TE048],[TE049],[TE050]
            ,[TE051],[TE052],[TE053],[TE054],[TE055],[TE056],[TE057],[TE058],[TE059],[TE060]
            ,[TE061],[TE062],[TE063],[TE064],[TE065],[TE066],[TE067],[TE068],[TE069],[TE070]
            ,[TE071],[TE072],[TE073],[TE074],[TE075],[TE076],[TE077],[TE078],[TE079],[TE080]
            ,[TE081],[TE082],[TE083],[TE084],[TE085],[TE086],[TE087],[TE088]
            ,[TE103],[TE107],[TE108],[TE109],[TE110]
            ,[TE111],[TE112],[TE113],[TE114],[TE115],[TE116],[TE117],[TE118],[TE119],[TE120]
            ,[TE121],[TE122],[TE123],[TE124],[TE125],[TE126],[TE127],[TE128],[TE129],[TE130]
            ,[TE131],[TE132],[TE133],[TE134],[TE135],[TE136],[TE137],[TE138],[TE139],[TE140]
            ,[TE141],[TE142],[TE143],[TE144],[TE145],[TE146],[TE147],[TE148],[TE149],[TE150]
            ,[TE151],[TE152],[TE163],[TE164],[TE165],[TE166],[TE167],[TE168],[TE169],[TE170]
            ,[TE171],[TE172],[TE173],[TE174],[TE175],[TE176],[TE177],[TE178],[TE179],[TE180]
            ,[TE181],[TE182],[TE183],[TE184],[TE185],[TE186],[TE187],[TE188],[TE189],[TE190]
            ,[TE191],[TE192],[TE193],[TE194],[TE195],[TE196],[TE197],[TE198],[TE199]
            ,[UDF01],[UDF02],[UDF03],[UDF04],[UDF05],[UDF06],[UDF07],[UDF08],[UDF09],[UDF10]
            ,[TF001],[TF002],[TF003],[TF004],[TF005],[TF006],[TF007],[TF008],[TF009],[TF010]
            ,[TF011],[TF012],[TF013],[TF014],[TF015],[TF016],[TF017],[TF018],[TF019],[TF020]
            ,[TF021],[TF022],[TF023],[TF024],[TF025],[TF026],[TF027],[TF028],[TF029],[TF030]
            ,[TF031],[TF032],[TF034],[TF035],[TF036],[TF037],[TF038],[TF039],[TF040],[TF041]
            ,[TF042],[TF043],[TF044],[TF045],[TF046],[TF048],[TF049],[TF050]
            ,[TF051],[TF052],[TF053],[TF054],[TF055],[TF056],[TF057],[TF058],[TF059],[TF060]
            ,[TF061],[TF062],[TF063],[TF064],[TF065],[TF066],[TF067],[TF068],[TF069],[TF070]
            ,[TF071],[TF072],[TF073],[TF074],[TF075],[TF076],[TF077],[TF078],[TF079],[TF080]
            ,[TF104],[TF105],[TF106],[TF107],[TF108],[TF109],[TF110]
            ,[TF111],[TF112],[TF113],[TF114],[TF115],[TF116],[TF117],[TF120]
            ,[TF121],[TF122],[TF123],[TF124],[TF125],[TF126],[TF127],[TF128],[TF129],[TF130]
            ,[TF131],[TF132],[TF133],[TF134],[TF135],[TF136],[TF137],[TF138],[TF139],[TF140]
            ,[TF141],[TF142],[TF143],[TF144],[TF145],[TF146],[TF147],[TF148],[TF149],[TF150]
            ,[TF151],[TF152],[TF153],[TF154],[TF155],[TF156],[TF157],[TF158],[TF159],[TF160]
            ,[TF161],[TF162],[TF163],[TF164],[TF165],[TF166],[TF167],[TF168],[TF169],[TF170]
            ,[TF171],[TF172],[TF173],[TF174],[TF175],[TF176],[TF177],[TF178],[TF179],[TF180]
            ,[TF181],[TF182],[TF183],[TF184],[TF185],[TF186],[TF187],[TF188],[TF189],[TF190]
            ,[TF191],[TF192],[TF193],[TF194],[TF195],[TF196],[TF197],[TF198],[TF199]
            ,[TF200],[TF300]
            ,COPTFUDF01,COPTFUDF02,COPTFUDF03,COPTFUDF04,COPTFUDF05,COPTFUDF06,COPTFUDF07,COPTFUDF08,COPTFUDF09,COPTFUDF10
            ,USER_GUID,NAME
            ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
            ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'
            ,MA002
            ,CASE WHEN TE018='1' THEN '1.應稅內含'  ELSE (CASE WHEN TE018='2' THEN '2.應稅外加'  ELSE (CASE WHEN TE018='3' THEN '3.零稅率'  ELSE (CASE WHEN TE018='4' THEN '4.免稅'  ELSE (CASE WHEN TE018='9' THEN '9.不計稅'  ELSE '' END) END) END) END ) END AS 'NEWTE018'
            ,CASE WHEN TE118='1' THEN '1.應稅內含'  ELSE (CASE WHEN TE118='2' THEN '2.應稅外加'  ELSE (CASE WHEN TE118='3' THEN '3.零稅率'  ELSE (CASE WHEN TE118='4' THEN '4.免稅'  ELSE (CASE WHEN TE118='9' THEN '9.不計稅'  ELSE '' END) END) END) END ) END AS 'NEWTE118'
            ,(SELECT TOP 1 ME002 FROM [TK].dbo.CMSME WHERE ME001=TE008) AS 'CMSME002A'
            ,(SELECT TOP 1 ME002 FROM [TK].dbo.CMSME WHERE ME001=TE108) AS 'CMSME002B'
            ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE009) AS 'CMSMV002A'
            ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE109) AS 'CMSMV002B'
            ,(SELECT TOP 1 COPTC.UDF05 FROM [TK].dbo.COPTC WHERE TC001=TE001 AND TC002=TE002) AS 'COPTCUDF05'
            ,BA
            ,BANAME
            ,(SELECT TOP 1 [USER_GUID] FROM [192.168.1.223].[UOF].[dbo].[TB_EB_USER] WHERE [ACCOUNT]=BA COLLATE Chinese_Taiwan_Stroke_BIN) AS 'BA_USER_GUID'
            ,MQ002
            FROM
            (
            SELECT
            [COPTE].[COMPANY],[COPTE].[CREATOR],[COPTE].[USR_GROUP],[COPTE].[CREATE_DATE],[COPTE].[MODIFIER],[COPTE].[MODI_DATE],[COPTE].[FLAG],[COPTE].[CREATE_TIME],[COPTE].[MODI_TIME],[COPTE].[TRANS_TYPE],[COPTE].[TRANS_NAME]
            ,[COPTE].[sync_date],[COPTE].[sync_time],[COPTE].[sync_mark],[COPTE].[sync_count],[COPTE].[DataUser],[COPTE].[DataGroup]
            ,[COPTE].[TE001],[COPTE].[TE002],[COPTE].[TE003],[COPTE].[TE004],[COPTE].[TE005],[COPTE].[TE006],[COPTE].[TE007],[COPTE].[TE008],[COPTE].[TE009],[COPTE].[TE010]
            ,[COPTE].[TE011],[COPTE].[TE012],[COPTE].[TE013],[COPTE].[TE014],[COPTE].[TE015],[COPTE].[TE016],[COPTE].[TE017],[COPTE].[TE018],[COPTE].[TE019],[COPTE].[TE020]
            ,[COPTE].[TE021],[COPTE].[TE022],[COPTE].[TE023],[COPTE].[TE024],[COPTE].[TE025],[COPTE].[TE026],[COPTE].[TE027],[COPTE].[TE028],[COPTE].[TE029],[COPTE].[TE030]
            ,[COPTE].[TE031],[COPTE].[TE032],[COPTE].[TE033],[COPTE].[TE034],[COPTE].[TE035],[COPTE].[TE036],[COPTE].[TE037],[COPTE].[TE038],[COPTE].[TE039],[COPTE].[TE040]
            ,[COPTE].[TE041],[COPTE].[TE042],[COPTE].[TE043],[COPTE].[TE044],[COPTE].[TE045],[COPTE].[TE046],[COPTE].[TE047],[COPTE].[TE048],[COPTE].[TE049],[COPTE].[TE050]
            ,[COPTE].[TE051],[COPTE].[TE052],[COPTE].[TE053],[COPTE].[TE054],[COPTE].[TE055],[COPTE].[TE056],[COPTE].[TE057],[COPTE].[TE058],[COPTE].[TE059],[COPTE].[TE060]
            ,[COPTE].[TE061],[COPTE].[TE062],[COPTE].[TE063],[COPTE].[TE064],[COPTE].[TE065],[COPTE].[TE066],[COPTE].[TE067],[COPTE].[TE068],[COPTE].[TE069],[COPTE].[TE070]
            ,[COPTE].[TE071],[COPTE].[TE072],[COPTE].[TE073],[COPTE].[TE074],[COPTE].[TE075],[COPTE].[TE076],[COPTE].[TE077],[COPTE].[TE078],[COPTE].[TE079],[COPTE].[TE080]
            ,[COPTE].[TE081],[COPTE].[TE082],[COPTE].[TE083],[COPTE].[TE084],[COPTE].[TE085],[COPTE].[TE086],[COPTE].[TE087],[COPTE].[TE088]
            ,[COPTE].[TE103],[COPTE].[TE107],[COPTE].[TE108],[COPTE].[TE109],[COPTE].[TE110]
            ,[COPTE].[TE111],[COPTE].[TE112],[COPTE].[TE113],[COPTE].[TE114],[COPTE].[TE115],[COPTE].[TE116],[COPTE].[TE117],[COPTE].[TE118],[COPTE].[TE119],[COPTE].[TE120]
            ,[COPTE].[TE121],[COPTE].[TE122],[COPTE].[TE123],[COPTE].[TE124],[COPTE].[TE125],[COPTE].[TE126],[COPTE].[TE127],[COPTE].[TE128],[COPTE].[TE129],[COPTE].[TE130]
            ,[COPTE].[TE131],[COPTE].[TE132],[COPTE].[TE133],[COPTE].[TE134],[COPTE].[TE135],[COPTE].[TE136],[COPTE].[TE137],[COPTE].[TE138],[COPTE].[TE139],[COPTE].[TE140]
            ,[COPTE].[TE141],[COPTE].[TE142],[COPTE].[TE143],[COPTE].[TE144],[COPTE].[TE145],[COPTE].[TE146],[COPTE].[TE147],[COPTE].[TE148],[COPTE].[TE149],[COPTE].[TE150]
            ,[COPTE].[TE151],[COPTE].[TE152],[COPTE].[TE163],[COPTE].[TE164],[COPTE].[TE165],[COPTE].[TE166],[COPTE].[TE167],[COPTE].[TE168],[COPTE].[TE169],[COPTE].[TE170]
            ,[COPTE].[TE171],[COPTE].[TE172],[COPTE].[TE173],[COPTE].[TE174],[COPTE].[TE175],[COPTE].[TE176],[COPTE].[TE177],[COPTE].[TE178],[COPTE].[TE179],[COPTE].[TE180]
            ,[COPTE].[TE181],[COPTE].[TE182],[COPTE].[TE183],[COPTE].[TE184],[COPTE].[TE185],[COPTE].[TE186],[COPTE].[TE187],[COPTE].[TE188],[COPTE].[TE189],[COPTE].[TE190]
            ,[COPTE].[TE191],[COPTE].[TE192],[COPTE].[TE193],[COPTE].[TE194],[COPTE].[TE195],[COPTE].[TE196],[COPTE].[TE197],[COPTE].[TE198],[COPTE].[TE199]
            ,[COPTE].[UDF01],[COPTE].[UDF02],[COPTE].[UDF03],[COPTE].[UDF04],[COPTE].[UDF05],[COPTE].[UDF06],[COPTE].[UDF07],[COPTE].[UDF08],[COPTE].[UDF09],[COPTE].[UDF10]
            ,[COPTF].[TF001],[COPTF].[TF002],[COPTF].[TF003],[COPTF].[TF004],[COPTF].[TF005],[COPTF].[TF006],[COPTF].[TF007],[COPTF].[TF008],[COPTF].[TF009],[COPTF].[TF010]
            ,[COPTF].[TF011],[COPTF].[TF012],[COPTF].[TF013],[COPTF].[TF014],[COPTF].[TF015],[COPTF].[TF016],[COPTF].[TF017],[COPTF].[TF018],[COPTF].[TF019],[COPTF].[TF020]
            ,[COPTF].[TF021],[COPTF].[TF022],[COPTF].[TF023],[COPTF].[TF024],[COPTF].[TF025],[COPTF].[TF026],[COPTF].[TF027],[COPTF].[TF028],[COPTF].[TF029],[COPTF].[TF030]
            ,[COPTF].[TF031],[COPTF].[TF032],[COPTF].[TF034],[COPTF].[TF035],[COPTF].[TF036],[COPTF].[TF037],[COPTF].[TF038],[COPTF].[TF039],[COPTF].[TF040],[COPTF].[TF041]
            ,[COPTF].[TF042],[COPTF].[TF043],[COPTF].[TF044],[COPTF].[TF045],[COPTF].[TF046],[COPTF].[TF048],[COPTF].[TF049],[COPTF].[TF050]
            ,[COPTF].[TF051],[COPTF].[TF052],[COPTF].[TF053],[COPTF].[TF054],[COPTF].[TF055],[COPTF].[TF056],[COPTF].[TF057],[COPTF].[TF058],[COPTF].[TF059],[COPTF].[TF060]
            ,[COPTF].[TF061],[COPTF].[TF062],[COPTF].[TF063],[COPTF].[TF064],[COPTF].[TF065],[COPTF].[TF066],[COPTF].[TF067],[COPTF].[TF068],[COPTF].[TF069],[COPTF].[TF070]
            ,[COPTF].[TF071],[COPTF].[TF072],[COPTF].[TF073],[COPTF].[TF074],[COPTF].[TF075],[COPTF].[TF076],[COPTF].[TF077],[COPTF].[TF078],[COPTF].[TF079],[COPTF].[TF080]
            ,[COPTF].[TF104],[COPTF].[TF105],[COPTF].[TF106],[COPTF].[TF107],[COPTF].[TF108],[COPTF].[TF109],[COPTF].[TF110]
            ,[COPTF].[TF111],[COPTF].[TF112],[COPTF].[TF113],[COPTF].[TF114],[COPTF].[TF115],[COPTF].[TF116],[COPTF].[TF117],[COPTF].[TF120]
            ,[COPTF].[TF121],[COPTF].[TF122],[COPTF].[TF123],[COPTF].[TF124],[COPTF].[TF125],[COPTF].[TF126],[COPTF].[TF127],[COPTF].[TF128],[COPTF].[TF129],[COPTF].[TF130]
            ,[COPTF].[TF131],[COPTF].[TF132],[COPTF].[TF133],[COPTF].[TF134],[COPTF].[TF135],[COPTF].[TF136],[COPTF].[TF137],[COPTF].[TF138],[COPTF].[TF139],[COPTF].[TF140]
            ,[COPTF].[TF141],[COPTF].[TF142],[COPTF].[TF143],[COPTF].[TF144],[COPTF].[TF145],[COPTF].[TF146],[COPTF].[TF147],[COPTF].[TF148],[COPTF].[TF149],[COPTF].[TF150]
            ,[COPTF].[TF151],[COPTF].[TF152],[COPTF].[TF153],[COPTF].[TF154],[COPTF].[TF155],[COPTF].[TF156],[COPTF].[TF157],[COPTF].[TF158],[COPTF].[TF159],[COPTF].[TF160]
            ,[COPTF].[TF161],[COPTF].[TF162],[COPTF].[TF163],[COPTF].[TF164],[COPTF].[TF165],[COPTF].[TF166],[COPTF].[TF167],[COPTF].[TF168],[COPTF].[TF169],[COPTF].[TF170]
            ,[COPTF].[TF171],[COPTF].[TF172],[COPTF].[TF173],[COPTF].[TF174],[COPTF].[TF175],[COPTF].[TF176],[COPTF].[TF177],[COPTF].[TF178],[COPTF].[TF179],[COPTF].[TF180]
            ,[COPTF].[TF181],[COPTF].[TF182],[COPTF].[TF183],[COPTF].[TF184],[COPTF].[TF185],[COPTF].[TF186],[COPTF].[TF187],[COPTF].[TF188],[COPTF].[TF189],[COPTF].[TF190]
            ,[COPTF].[TF191],[COPTF].[TF192],[COPTF].[TF193],[COPTF].[TF194],[COPTF].[TF195],[COPTF].[TF196],[COPTF].[TF197],[COPTF].[TF198],[COPTF].[TF199]
            ,[COPTF].[TF200],[COPTF].[TF300]
            ,[COPTF].[UDF01] AS 'COPTFUDF01',[COPTF].[UDF02] AS 'COPTFUDF02',[COPTF].[UDF03] AS 'COPTFUDF03',[COPTF].[UDF04] AS 'COPTFUDF04',[COPTF].[UDF05] AS 'COPTFUDF05',[COPTF].[UDF06] AS 'COPTFUDF06',[COPTF].[UDF07] AS 'COPTFUDF07',[COPTF].[UDF08] AS 'COPTFUDF08',[COPTF].[UDF09] AS 'COPTFUDF09',[COPTF].[UDF10] AS 'COPTFUDF10'
            ,[TB_EB_USER].USER_GUID,NAME
            ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE009) AS 'MV002'
            ,(SELECT TOP 1 MA002 FROM [TK].dbo.COPMA WHERE MA001=TE007) AS 'MA002'
            ,(SELECT TOP 1 COPMA.UDF04 FROM [TK].dbo.COPMA,[TK].dbo.CMSMV WHERE COPMA.UDF04=CMSMV.MV001 AND COPMA.MA001=TE007) AS 'BA'
            ,(SELECT TOP 1 CMSMV.MV002 FROM [TK].dbo.COPMA,[TK].dbo.CMSMV WHERE COPMA.UDF04=CMSMV.MV001 AND COPMA.MA001=TE007) AS 'BANAME'
            ,MQ002
            FROM [TK].dbo.COPTF,[TK].dbo.COPTE
            LEFT JOIN [192.168.1.223].[{0}].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= TE009 COLLATE Chinese_Taiwan_Stroke_BIN
            LEFT JOIN [TK].dbo.CMSMQ ON TE001=MQ001
            WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
            AND TE001=@TE001 AND TE002=@TE002 AND TE003=@TE003
            ) AS TEMP", DBNAME);

        return QueryOrNull(db, sql);
    }

    /// <summary>變更單單頭（明細可能沒有），7.變更單沒有明細 送簽用</summary>
    public DataTable SEARCHCOPTECOPTF7(string TE001, string TE002, string TE003)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@TE001", TE001);
        db.AddParameter("@TE002", TE002);
        db.AddParameter("@TE003", TE003);

        string sql = string.Format(@"
            SELECT
            ISNULL((SELECT TOP 1 UDF01 FROM  [TK].dbo.COPTD WHERE TD001=TF001 AND TD002=TF002 AND TD003=TF104),'') AS COTTDUDF01
            ,COPTFUDF01 AS COPTFUDF01
            ,CASE WHEN COPTFUDF01 IN ('N','n','') AND ISNULL((SELECT TOP 1 UDF01 FROM  [TK].dbo.COPTD WHERE TD001=TF001 AND TD002=TF002 AND TD003=TF104),'N') IN ('N','n','') THEN 'N' ELSE 'Y' END AS 'COPTFUDF01CHECK'
            ,[COMPANY],[CREATOR],[USR_GROUP],[CREATE_DATE],[MODIFIER],[MODI_DATE],[FLAG],[CREATE_TIME],[MODI_TIME],[TRANS_TYPE],[TRANS_NAME]
            ,[sync_date],[sync_time],[sync_mark],[sync_count],[DataUser],[DataGroup]
            ,[TE001],[TE002],[TE003],[TE004],[TE005],[TE006],[TE007],[TE008],[TE009],[TE010]
            ,[TE011],[TE012],[TE013],[TE014],[TE015],[TE016],[TE017],[TE018],[TE019],[TE020]
            ,[TE021],[TE022],[TE023],[TE024],[TE025],[TE026],[TE027],[TE028],[TE029],[TE030]
            ,[TE031],[TE032],[TE033],[TE034],[TE035],[TE036],[TE037],[TE038],[TE039],[TE040]
            ,[TE041],[TE042],[TE043],[TE044],[TE045],[TE046],[TE047],[TE048],[TE049],[TE050]
            ,[TE051],[TE052],[TE053],[TE054],[TE055],[TE056],[TE057],[TE058],[TE059],[TE060]
            ,[TE061],[TE062],[TE063],[TE064],[TE065],[TE066],[TE067],[TE068],[TE069],[TE070]
            ,[TE071],[TE072],[TE073],[TE074],[TE075],[TE076],[TE077],[TE078],[TE079],[TE080]
            ,[TE081],[TE082],[TE083],[TE084],[TE085],[TE086],[TE087],[TE088]
            ,[TE103],[TE107],[TE108],[TE109],[TE110]
            ,[TE111],[TE112],[TE113],[TE114],[TE115],[TE116],[TE117],[TE118],[TE119],[TE120]
            ,[TE121],[TE122],[TE123],[TE124],[TE125],[TE126],[TE127],[TE128],[TE129],[TE130]
            ,[TE131],[TE132],[TE133],[TE134],[TE135],[TE136],[TE137],[TE138],[TE139],[TE140]
            ,[TE141],[TE142],[TE143],[TE144],[TE145],[TE146],[TE147],[TE148],[TE149],[TE150]
            ,[TE151],[TE152],[TE163],[TE164],[TE165],[TE166],[TE167],[TE168],[TE169],[TE170]
            ,[TE171],[TE172],[TE173],[TE174],[TE175],[TE176],[TE177],[TE178],[TE179],[TE180]
            ,[TE181],[TE182],[TE183],[TE184],[TE185],[TE186],[TE187],[TE188],[TE189],[TE190]
            ,[TE191],[TE192],[TE193],[TE194],[TE195],[TE196],[TE197],[TE198],[TE199]
            ,[UDF01],[UDF02],[UDF03],[UDF04],[UDF05],[UDF06],[UDF07],[UDF08],[UDF09],[UDF10]
            ,[TF001],[TF002],[TF003],[TF004],[TF005],[TF006],[TF007],[TF008],[TF009],[TF010]
            ,[TF011],[TF012],[TF013],[TF014],[TF015],[TF016],[TF017],[TF018],[TF019],[TF020]
            ,[TF021],[TF022],[TF023],[TF024],[TF025],[TF026],[TF027],[TF028],[TF029],[TF030]
            ,[TF031],[TF032],[TF034],[TF035],[TF036],[TF037],[TF038],[TF039],[TF040],[TF041]
            ,[TF042],[TF043],[TF044],[TF045],[TF046],[TF048],[TF049],[TF050]
            ,[TF051],[TF052],[TF053],[TF054],[TF055],[TF056],[TF057],[TF058],[TF059],[TF060]
            ,[TF061],[TF062],[TF063],[TF064],[TF065],[TF066],[TF067],[TF068],[TF069],[TF070]
            ,[TF071],[TF072],[TF073],[TF074],[TF075],[TF076],[TF077],[TF078],[TF079],[TF080]
            ,[TF104],[TF105],[TF106],[TF107],[TF108],[TF109],[TF110]
            ,[TF111],[TF112],[TF113],[TF114],[TF115],[TF116],[TF117],[TF120]
            ,[TF121],[TF122],[TF123],[TF124],[TF125],[TF126],[TF127],[TF128],[TF129],[TF130]
            ,[TF131],[TF132],[TF133],[TF134],[TF135],[TF136],[TF137],[TF138],[TF139],[TF140]
            ,[TF141],[TF142],[TF143],[TF144],[TF145],[TF146],[TF147],[TF148],[TF149],[TF150]
            ,[TF151],[TF152],[TF153],[TF154],[TF155],[TF156],[TF157],[TF158],[TF159],[TF160]
            ,[TF161],[TF162],[TF163],[TF164],[TF165],[TF166],[TF167],[TF168],[TF169],[TF170]
            ,[TF171],[TF172],[TF173],[TF174],[TF175],[TF176],[TF177],[TF178],[TF179],[TF180]
            ,[TF181],[TF182],[TF183],[TF184],[TF185],[TF186],[TF187],[TF188],[TF189],[TF190]
            ,[TF191],[TF192],[TF193],[TF194],[TF195],[TF196],[TF197],[TF198],[TF199]
            ,[TF200],[TF300]
            ,COPTFUDF01,COPTFUDF02,COPTFUDF03,COPTFUDF04,COPTFUDF05,COPTFUDF06,COPTFUDF07,COPTFUDF08,COPTFUDF09,COPTFUDF10
            ,USER_GUID,NAME
            ,(SELECT TOP 1 GROUP_ID FROM [192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'GROUP_ID'
            ,(SELECT TOP 1 TITLE_ID FROM [192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP] WHERE [TB_EB_EMPL_DEP].USER_GUID=TEMP.USER_GUID) AS 'TITLE_ID'
            ,MA002
            ,CASE WHEN TE018='1' THEN '1.應稅內含'  ELSE (CASE WHEN TE018='2' THEN '2.應稅外加'  ELSE (CASE WHEN TE018='3' THEN '3.零稅率'  ELSE (CASE WHEN TE018='4' THEN '4.免稅'  ELSE (CASE WHEN TE018='9' THEN '9.不計稅'  ELSE '' END) END) END) END ) END AS 'NEWTE018'
            ,CASE WHEN TE118='1' THEN '1.應稅內含'  ELSE (CASE WHEN TE118='2' THEN '2.應稅外加'  ELSE (CASE WHEN TE118='3' THEN '3.零稅率'  ELSE (CASE WHEN TE118='4' THEN '4.免稅'  ELSE (CASE WHEN TE118='9' THEN '9.不計稅'  ELSE '' END) END) END) END ) END AS 'NEWTE118'
            ,(SELECT TOP 1 ME002 FROM [TK].dbo.CMSME WHERE ME001=TE008) AS 'CMSME002A'
            ,(SELECT TOP 1 ME002 FROM [TK].dbo.CMSME WHERE ME001=TE108) AS 'CMSME002B'
            ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE009) AS 'CMSMV002A'
            ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE109) AS 'CMSMV002B'
            ,(SELECT TOP 1 COPTC.UDF05 FROM [TK].dbo.COPTC WHERE TC001=TE001 AND TC002=TE002) AS 'COPTCUDF05'
            ,BA
            ,BANAME
            ,(SELECT TOP 1 [USER_GUID] FROM [192.168.1.223].[UOF].[dbo].[TB_EB_USER] WHERE [ACCOUNT]=BA COLLATE Chinese_Taiwan_Stroke_BIN) AS 'BA_USER_GUID'
            FROM
            (
            SELECT
            [COPTE].[COMPANY],[COPTE].[CREATOR],[COPTE].[USR_GROUP],[COPTE].[CREATE_DATE],[COPTE].[MODIFIER],[COPTE].[MODI_DATE],[COPTE].[FLAG],[COPTE].[CREATE_TIME],[COPTE].[MODI_TIME],[COPTE].[TRANS_TYPE],[COPTE].[TRANS_NAME]
            ,[COPTE].[sync_date],[COPTE].[sync_time],[COPTE].[sync_mark],[COPTE].[sync_count],[COPTE].[DataUser],[COPTE].[DataGroup]
            ,[COPTE].[TE001],[COPTE].[TE002],[COPTE].[TE003],[COPTE].[TE004],[COPTE].[TE005],[COPTE].[TE006],[COPTE].[TE007],[COPTE].[TE008],[COPTE].[TE009],[COPTE].[TE010]
            ,[COPTE].[TE011],[COPTE].[TE012],[COPTE].[TE013],[COPTE].[TE014],[COPTE].[TE015],[COPTE].[TE016],[COPTE].[TE017],[COPTE].[TE018],[COPTE].[TE019],[COPTE].[TE020]
            ,[COPTE].[TE021],[COPTE].[TE022],[COPTE].[TE023],[COPTE].[TE024],[COPTE].[TE025],[COPTE].[TE026],[COPTE].[TE027],[COPTE].[TE028],[COPTE].[TE029],[COPTE].[TE030]
            ,[COPTE].[TE031],[COPTE].[TE032],[COPTE].[TE033],[COPTE].[TE034],[COPTE].[TE035],[COPTE].[TE036],[COPTE].[TE037],[COPTE].[TE038],[COPTE].[TE039],[COPTE].[TE040]
            ,[COPTE].[TE041],[COPTE].[TE042],[COPTE].[TE043],[COPTE].[TE044],[COPTE].[TE045],[COPTE].[TE046],[COPTE].[TE047],[COPTE].[TE048],[COPTE].[TE049],[COPTE].[TE050]
            ,[COPTE].[TE051],[COPTE].[TE052],[COPTE].[TE053],[COPTE].[TE054],[COPTE].[TE055],[COPTE].[TE056],[COPTE].[TE057],[COPTE].[TE058],[COPTE].[TE059],[COPTE].[TE060]
            ,[COPTE].[TE061],[COPTE].[TE062],[COPTE].[TE063],[COPTE].[TE064],[COPTE].[TE065],[COPTE].[TE066],[COPTE].[TE067],[COPTE].[TE068],[COPTE].[TE069],[COPTE].[TE070]
            ,[COPTE].[TE071],[COPTE].[TE072],[COPTE].[TE073],[COPTE].[TE074],[COPTE].[TE075],[COPTE].[TE076],[COPTE].[TE077],[COPTE].[TE078],[COPTE].[TE079],[COPTE].[TE080]
            ,[COPTE].[TE081],[COPTE].[TE082],[COPTE].[TE083],[COPTE].[TE084],[COPTE].[TE085],[COPTE].[TE086],[COPTE].[TE087],[COPTE].[TE088]
            ,[COPTE].[TE103],[COPTE].[TE107],[COPTE].[TE108],[COPTE].[TE109],[COPTE].[TE110]
            ,[COPTE].[TE111],[COPTE].[TE112],[COPTE].[TE113],[COPTE].[TE114],[COPTE].[TE115],[COPTE].[TE116],[COPTE].[TE117],[COPTE].[TE118],[COPTE].[TE119],[COPTE].[TE120]
            ,[COPTE].[TE121],[COPTE].[TE122],[COPTE].[TE123],[COPTE].[TE124],[COPTE].[TE125],[COPTE].[TE126],[COPTE].[TE127],[COPTE].[TE128],[COPTE].[TE129],[COPTE].[TE130]
            ,[COPTE].[TE131],[COPTE].[TE132],[COPTE].[TE133],[COPTE].[TE134],[COPTE].[TE135],[COPTE].[TE136],[COPTE].[TE137],[COPTE].[TE138],[COPTE].[TE139],[COPTE].[TE140]
            ,[COPTE].[TE141],[COPTE].[TE142],[COPTE].[TE143],[COPTE].[TE144],[COPTE].[TE145],[COPTE].[TE146],[COPTE].[TE147],[COPTE].[TE148],[COPTE].[TE149],[COPTE].[TE150]
            ,[COPTE].[TE151],[COPTE].[TE152],[COPTE].[TE163],[COPTE].[TE164],[COPTE].[TE165],[COPTE].[TE166],[COPTE].[TE167],[COPTE].[TE168],[COPTE].[TE169],[COPTE].[TE170]
            ,[COPTE].[TE171],[COPTE].[TE172],[COPTE].[TE173],[COPTE].[TE174],[COPTE].[TE175],[COPTE].[TE176],[COPTE].[TE177],[COPTE].[TE178],[COPTE].[TE179],[COPTE].[TE180]
            ,[COPTE].[TE181],[COPTE].[TE182],[COPTE].[TE183],[COPTE].[TE184],[COPTE].[TE185],[COPTE].[TE186],[COPTE].[TE187],[COPTE].[TE188],[COPTE].[TE189],[COPTE].[TE190]
            ,[COPTE].[TE191],[COPTE].[TE192],[COPTE].[TE193],[COPTE].[TE194],[COPTE].[TE195],[COPTE].[TE196],[COPTE].[TE197],[COPTE].[TE198],[COPTE].[TE199]
            ,[COPTE].[UDF01],[COPTE].[UDF02],[COPTE].[UDF03],[COPTE].[UDF04],[COPTE].[UDF05],[COPTE].[UDF06],[COPTE].[UDF07],[COPTE].[UDF08],[COPTE].[UDF09],[COPTE].[UDF10]
            ,[COPTF].[TF001],[COPTF].[TF002],[COPTF].[TF003],[COPTF].[TF004],[COPTF].[TF005],[COPTF].[TF006],[COPTF].[TF007],[COPTF].[TF008],[COPTF].[TF009],[COPTF].[TF010]
            ,[COPTF].[TF011],[COPTF].[TF012],[COPTF].[TF013],[COPTF].[TF014],[COPTF].[TF015],[COPTF].[TF016],[COPTF].[TF017],[COPTF].[TF018],[COPTF].[TF019],[COPTF].[TF020]
            ,[COPTF].[TF021],[COPTF].[TF022],[COPTF].[TF023],[COPTF].[TF024],[COPTF].[TF025],[COPTF].[TF026],[COPTF].[TF027],[COPTF].[TF028],[COPTF].[TF029],[COPTF].[TF030]
            ,[COPTF].[TF031],[COPTF].[TF032],[COPTF].[TF034],[COPTF].[TF035],[COPTF].[TF036],[COPTF].[TF037],[COPTF].[TF038],[COPTF].[TF039],[COPTF].[TF040],[COPTF].[TF041]
            ,[COPTF].[TF042],[COPTF].[TF043],[COPTF].[TF044],[COPTF].[TF045],[COPTF].[TF046],[COPTF].[TF048],[COPTF].[TF049],[COPTF].[TF050]
            ,[COPTF].[TF051],[COPTF].[TF052],[COPTF].[TF053],[COPTF].[TF054],[COPTF].[TF055],[COPTF].[TF056],[COPTF].[TF057],[COPTF].[TF058],[COPTF].[TF059],[COPTF].[TF060]
            ,[COPTF].[TF061],[COPTF].[TF062],[COPTF].[TF063],[COPTF].[TF064],[COPTF].[TF065],[COPTF].[TF066],[COPTF].[TF067],[COPTF].[TF068],[COPTF].[TF069],[COPTF].[TF070]
            ,[COPTF].[TF071],[COPTF].[TF072],[COPTF].[TF073],[COPTF].[TF074],[COPTF].[TF075],[COPTF].[TF076],[COPTF].[TF077],[COPTF].[TF078],[COPTF].[TF079],[COPTF].[TF080]
            ,[COPTF].[TF104],[COPTF].[TF105],[COPTF].[TF106],[COPTF].[TF107],[COPTF].[TF108],[COPTF].[TF109],[COPTF].[TF110]
            ,[COPTF].[TF111],[COPTF].[TF112],[COPTF].[TF113],[COPTF].[TF114],[COPTF].[TF115],[COPTF].[TF116],[COPTF].[TF117],[COPTF].[TF120]
            ,[COPTF].[TF121],[COPTF].[TF122],[COPTF].[TF123],[COPTF].[TF124],[COPTF].[TF125],[COPTF].[TF126],[COPTF].[TF127],[COPTF].[TF128],[COPTF].[TF129],[COPTF].[TF130]
            ,[COPTF].[TF131],[COPTF].[TF132],[COPTF].[TF133],[COPTF].[TF134],[COPTF].[TF135],[COPTF].[TF136],[COPTF].[TF137],[COPTF].[TF138],[COPTF].[TF139],[COPTF].[TF140]
            ,[COPTF].[TF141],[COPTF].[TF142],[COPTF].[TF143],[COPTF].[TF144],[COPTF].[TF145],[COPTF].[TF146],[COPTF].[TF147],[COPTF].[TF148],[COPTF].[TF149],[COPTF].[TF150]
            ,[COPTF].[TF151],[COPTF].[TF152],[COPTF].[TF153],[COPTF].[TF154],[COPTF].[TF155],[COPTF].[TF156],[COPTF].[TF157],[COPTF].[TF158],[COPTF].[TF159],[COPTF].[TF160]
            ,[COPTF].[TF161],[COPTF].[TF162],[COPTF].[TF163],[COPTF].[TF164],[COPTF].[TF165],[COPTF].[TF166],[COPTF].[TF167],[COPTF].[TF168],[COPTF].[TF169],[COPTF].[TF170]
            ,[COPTF].[TF171],[COPTF].[TF172],[COPTF].[TF173],[COPTF].[TF174],[COPTF].[TF175],[COPTF].[TF176],[COPTF].[TF177],[COPTF].[TF178],[COPTF].[TF179],[COPTF].[TF180]
            ,[COPTF].[TF181],[COPTF].[TF182],[COPTF].[TF183],[COPTF].[TF184],[COPTF].[TF185],[COPTF].[TF186],[COPTF].[TF187],[COPTF].[TF188],[COPTF].[TF189],[COPTF].[TF190]
            ,[COPTF].[TF191],[COPTF].[TF192],[COPTF].[TF193],[COPTF].[TF194],[COPTF].[TF195],[COPTF].[TF196],[COPTF].[TF197],[COPTF].[TF198],[COPTF].[TF199]
            ,[COPTF].[TF200],[COPTF].[TF300]
            ,[COPTF].[UDF01] AS 'COPTFUDF01',[COPTF].[UDF02] AS 'COPTFUDF02',[COPTF].[UDF03] AS 'COPTFUDF03',[COPTF].[UDF04] AS 'COPTFUDF04',[COPTF].[UDF05] AS 'COPTFUDF05',[COPTF].[UDF06] AS 'COPTFUDF06',[COPTF].[UDF07] AS 'COPTFUDF07',[COPTF].[UDF08] AS 'COPTFUDF08',[COPTF].[UDF09] AS 'COPTFUDF09',[COPTF].[UDF10] AS 'COPTFUDF10'
            ,[TB_EB_USER].USER_GUID,NAME
            ,(SELECT TOP 1 MV002 FROM [TK].dbo.CMSMV WHERE MV001=TE009) AS 'MV002'
            ,(SELECT TOP 1 MA002 FROM [TK].dbo.COPMA WHERE MA001=TE007) AS 'MA002'
            ,(SELECT TOP 1 COPMA.UDF04 FROM [TK].dbo.COPMA,[TK].dbo.CMSMV WHERE COPMA.UDF04=CMSMV.MV001 AND COPMA.MA001=TE007) AS 'BA'
            ,(SELECT TOP 1 CMSMV.MV002 FROM [TK].dbo.COPMA,[TK].dbo.CMSMV WHERE COPMA.UDF04=CMSMV.MV001 AND COPMA.MA001=TE007) AS 'BANAME'
            FROM [TK].dbo.COPTE
            LEFT JOIN [TK].dbo.COPTF ON TE001=TF001 AND TE002=TF002 AND TE003=TF003
            LEFT JOIN [192.168.1.223].[{0}].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= TE009 COLLATE Chinese_Taiwan_Stroke_BIN
            WHERE 1=1
            AND TE001=@TE001 AND TE002=@TE002 AND TE003=@TE003
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

    /// <summary>變更單客代、變更後明細金額、原訂單金額</summary>
    public DataTable FINDCOPTE(string TE001002003)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@KEY", TE001002003);

        DataTable dt = Query(db, @"
            SELECT TE007
            ,ISNULL((SELECT SUM(TF014) FROM [TK].dbo.COPTF WHERE TF001=TE001 AND TF002=TE002 AND TF003=TE003),0) AS COPTEMONEYS
            ,ISNULL((SELECT SUM(TC029+TC030) FROM [TK].dbo.COPTC WHERE TC001=TE001 AND TC002=TE002),0) AS COPTCMONEYS
            FROM [TK].dbo.COPTE
            WHERE TE001+TE002+TE003=@KEY");

        return dt.Rows.Count > 0 ? dt : null;
    }

    /// <summary>UOF 中簽核進行中（TASK_STATUS=1）的同一張訂單變更單</summary>
    public DataTable CHECK_TB_WKF_TASK(string TC001TC002)
    {
        DatabaseHelper db = CreateDb(UOF_CONNECTION);
        db.AddParameter("@TE001", TC001TC002.Substring(0, 4));
        db.AddParameter("@TE002", TC001TC002.Substring(4, 11));

        DataTable dt = Query(db, @"
            SELECT CURRENT_DOC
            ,CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE001""]/@fieldValue)[1]', 'nvarchar(max)') AS TE001Value
            ,CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE002""]/@fieldValue)[1]', 'nvarchar(max)') AS TE002Value
            ,TASK_STATUS
            FROM [UOF].[dbo].TB_WKF_TASK
            WHERE TASK_STATUS = '1'
            AND CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE001""]/@fieldValue)[1]', 'nvarchar(max)') LIKE '%' + @TE001 + '%'
            AND CURRENT_DOC.value('(/Form/FormFieldValue/FieldItem[@fieldId=""TE002""]/@fieldValue)[1]', 'nvarchar(max)') LIKE '%' + @TE002 + '%'");

        return dt.Rows.Count > 0 ? dt : null;
    }

    /// <summary>變更單明細中屬於烘焙品（ZINVMBBAKING）的品項</summary>
    public DataTable CHECK_COPTF_ZINVMBBAKING(string TE001, string TE002, string TE003)
    {
        DatabaseHelper db = CreateDb(ERP_CONNECTION);
        db.AddParameter("@TE001", TE001);
        db.AddParameter("@TE002", TE002);
        db.AddParameter("@TE003", TE003);

        return QueryOrNull(db, @"
            SELECT *
            FROM [TK].dbo.COPTF
            WHERE TF001=@TE001 AND TF002=@TE002 AND TF003=@TE003
            AND TF005 IN (SELECT [MB001] FROM [TK].[dbo].[ZINVMBBAKING])");
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
