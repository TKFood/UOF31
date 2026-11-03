using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.EIP.SystemInfo;

/// <summary>
/// 採購預交行事曆：依採購單身預交日(TD012)把採購明細排在月曆上
/// </summary>
public partial class CDS_WebPage_PUR_TK_PURTD_CALENDAR : Ede.Uof.Utility.Page.BasePage
{
    private const string MonthFormat = "yyyy-MM";

    /// <summary>目前顯示的月份（每月 1 日）</summary>
    private DateTime CurrentMonth
    {
        get
        {
            object v = ViewState["CurrentMonth"];
            return v == null ? FirstDay(DateTime.Today) : (DateTime)v;
        }
        set { ViewState["CurrentMonth"] = FirstDay(value); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 可用 ?ym=202611 直接開指定月份
            DateTime month;
            string ym = Request.QueryString["ym"];
            if (!string.IsNullOrEmpty(ym)
                && DateTime.TryParseExact(ym, "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out month))
                CurrentMonth = month;
            else
                CurrentMonth = DateTime.Today;

            BindCalendar();
        }
    }

    #region 按鈕
    protected void btnPrev_Click(object sender, EventArgs e)
    {
        CurrentMonth = CurrentMonth.AddMonths(-1);
        BindCalendar();
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        CurrentMonth = CurrentMonth.AddMonths(1);
        BindCalendar();
    }

    protected void btnThisMonth_Click(object sender, EventArgs e)
    {
        CurrentMonth = DateTime.Today;
        BindCalendar();
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        DateTime month;
        if (DateTime.TryParseExact(txtMonth.Text.Trim(), MonthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out month))
            CurrentMonth = month;
        BindCalendar();
    }
    #endregion

    #region 資料
    private DataTable LoadData(DateTime month, string vendor, string item)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder sql = new StringBuilder(@"
            SELECT TD012 AS '預交日',TC004 AS '供應廠商',MA002 AS '廠商',TD004 AS '品號',TD005 AS '品名'
                  ,TD008 AS '採購數量',TD009 AS '單位',TD001 AS '採購單別',TD002 AS '採購單號'
            FROM [TK].dbo.PURTC,[TK].dbo.PURTD,[TK].dbo.PURMA
            WHERE TC001=TD001 AND TC002=TD002 AND MA001=TC004
            AND TC014 IN ('Y','N')
            AND TD012>=@SDATE AND TD012<=@EDATE");

        m_db.AddParameter("@SDATE", month.ToString("yyyyMMdd"));
        m_db.AddParameter("@EDATE", month.AddMonths(1).AddDays(-1).ToString("yyyyMMdd"));

        if (!string.IsNullOrEmpty(vendor))
        {
            sql.Append(" AND (TC004 LIKE @VENDOR OR MA002 LIKE @VENDOR)");
            m_db.AddParameter("@VENDOR", "%" + vendor + "%");
        }
        if (!string.IsNullOrEmpty(item))
        {
            sql.Append(" AND (TD004 LIKE @ITEM OR TD005 LIKE @ITEM)");
            m_db.AddParameter("@ITEM", "%" + item + "%");
        }
        sql.Append(" ORDER BY TD012,TD004");

        DataTable dt = new DataTable();
        dt.Load(m_db.ExecuteReader(sql.ToString()));
        return dt;
    }
    #endregion

    #region 畫面
    private void BindCalendar()
    {
        DateTime month = CurrentMonth;
        txtMonth.Text = month.ToString(MonthFormat);
        lblMonth.Text = month.ToString("yyyy 年 MM 月");

        DataTable dt = LoadData(month, txtVendor.Text.Trim(), txtItem.Text.Trim());

        // 依預交日分組
        Dictionary<string, List<DataRow>> byDate = new Dictionary<string, List<DataRow>>();
        foreach (DataRow r in dt.Rows)
        {
            string key = Convert.ToString(r["預交日"]).Trim();
            List<DataRow> list;
            if (!byDate.TryGetValue(key, out list))
            {
                list = new List<DataRow>();
                byDate.Add(key, list);
            }
            list.Add(r);
        }

        lblSummary.Text = string.Format("本月共 {0} 筆採購明細，{1} 天有預交", dt.Rows.Count, byDate.Count);
        litCalendar.Text = BuildCalendarHtml(month, byDate);
        litDetail.Text = BuildDetailHtml(dt);
    }

    private string BuildCalendarHtml(DateTime month, Dictionary<string, List<DataRow>> byDate)
    {
        string[] weekNames = { "日", "一", "二", "三", "四", "五", "六" };
        StringBuilder sb = new StringBuilder();
        sb.Append("<table class=\"cal\"><thead><tr>");
        for (int i = 0; i < 7; i++)
            sb.AppendFormat("<th{0}>{1}</th>", (i == 0 || i == 6) ? " class=\"wk-end\"" : "", weekNames[i]);
        sb.Append("</tr></thead><tbody>");

        // 週日為每週第一天；從月初所在週的週日開始，畫到月底所在週的週六
        DateTime start = month.AddDays(-(int)month.DayOfWeek);
        DateTime last = month.AddMonths(1).AddDays(-1);
        DateTime end = last.AddDays(6 - (int)last.DayOfWeek);

        for (DateTime d = start; d <= end; d = d.AddDays(1))
        {
            if (d.DayOfWeek == DayOfWeek.Sunday) sb.Append("<tr>");

            string key = d.ToString("yyyyMMdd");
            bool inMonth = d.Month == month.Month;
            List<DataRow> rows = null;
            bool has = inMonth && byDate.TryGetValue(key, out rows);

            List<string> css = new List<string>();
            if (!inMonth) css.Add("out");
            else if (d.DayOfWeek == DayOfWeek.Sunday || d.DayOfWeek == DayOfWeek.Saturday) css.Add("wk-end");
            if (has) css.Add("has");
            if (d == DateTime.Today) css.Add("today");

            sb.AppendFormat("<td class=\"{0}\" data-date=\"{1}\"{2}>", string.Join(" ", css), key,
                has ? " onclick=\"calPick(this)\"" : "");
            sb.Append("<div class=\"d-head\">");
            sb.AppendFormat("<span class=\"d-num\">{0}</span>", inMonth ? d.Day.ToString() : d.ToString("M/d"));
            if (has) sb.AppendFormat("<span class=\"d-cnt\">{0} 筆</span>", rows.Count);
            sb.Append("</div>");

            if (has)
            {
                sb.Append("<div class=\"d-list\">");
                foreach (DataRow r in rows)
                {
                    string tip = string.Format("{0}-{1}　{2} {3}　{4} {5}",
                        r["採購單別"], r["採購單號"], r["供應廠商"], r["廠商"], r["品號"], r["品名"]);
                    sb.AppendFormat("<div class=\"ent\" title=\"{0}\"><b>{1}</b> {2} <span class=\"q\">{3} {4}</span></div>",
                        H(tip), H(r["廠商"]), H(r["品名"]), Qty(r["採購數量"]), H(r["單位"]));
                }
                sb.Append("</div>");
            }
            sb.Append("</td>");

            if (d.DayOfWeek == DayOfWeek.Saturday) sb.Append("</tr>");
        }
        sb.Append("</tbody></table>");
        return sb.ToString();
    }

    private string BuildDetailHtml(DataTable dt)
    {
        if (dt.Rows.Count == 0)
            return "<p style=\"color:#888\">本月沒有符合條件的採購預交資料。</p>";

        StringBuilder sb = new StringBuilder();
        sb.Append("<table><thead><tr><th>預交日</th><th>供應廠商</th><th>廠商</th><th>品號</th><th>品名</th>"
                + "<th>採購數量</th><th>單位</th><th>採購單別</th><th>採購單號</th></tr></thead><tbody>");
        foreach (DataRow r in dt.Rows)
        {
            string key = Convert.ToString(r["預交日"]).Trim();
            sb.AppendFormat("<tr data-date=\"{0}\"><td class=\"c\">{1}</td><td class=\"c\">{2}</td><td>{3}</td>"
                + "<td class=\"c\">{4}</td><td>{5}</td><td class=\"n\">{6}</td><td class=\"c\">{7}</td>"
                + "<td class=\"c\">{8}</td><td class=\"c\">{9}</td></tr>",
                H(key), H(FormatDate(key)), H(r["供應廠商"]), H(r["廠商"]), H(r["品號"]), H(r["品名"]),
                Qty(r["採購數量"]), H(r["單位"]), H(r["採購單別"]), H(r["採購單號"]));
        }
        sb.Append("</tbody></table>");
        return sb.ToString();
    }
    #endregion

    #region 工具
    private static DateTime FirstDay(DateTime d)
    {
        return new DateTime(d.Year, d.Month, 1);
    }

    private static string H(object v)
    {
        return HttpUtility.HtmlEncode(Convert.ToString(v).Trim());
    }

    private static string Qty(object v)
    {
        decimal q;
        if (v != null && v != DBNull.Value && decimal.TryParse(Convert.ToString(v), out q))
            return q.ToString("#,##0.###");
        return H(v);
    }

    private static string FormatDate(string yyyymmdd)
    {
        DateTime d;
        if (DateTime.TryParseExact(yyyymmdd, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
            return d.ToString("yyyy/MM/dd");
        return yyyymmdd;
    }
    #endregion
}
