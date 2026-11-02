using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Globalization;
using Ede.Uof.EIP.PrivateMessage;


public partial class CDS_WebPage_RESEARCH_TKRESEARCH_COST : Ede.Uof.Utility.Page.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SETYEARSWEEKS();
            BindDropDownList();
            //BindGrid1("","","");
            //BindGrid2("", "", "");

        }
        else
        {


        }




    }
    #region FUNCTION
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PARAID", typeof(String));
        dt.Columns.Add("PARANAME", typeof(String));

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"SELECT  [ID],[KIND],[PARAID],[PARANAME] FROM [TKRESEARCH].[dbo].[TBPARA] WHERE [KIND]='人工記錄成本' ORDER BY  [ID] ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "PARANAME";
            DropDownList1.DataValueField = "PARANAME";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }


    public void SETYEARSWEEKS()
    {
        txtDate1.Text = DateTime.Now.ToString("yyyy");
        txtDate2.Text = DateTime.Now.ToString("yyyy");

    }

    private void BindGrid1(string YM, string MB001, string MB002)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder SQUERY = new StringBuilder();



        //查詢條件
        if (!string.IsNullOrEmpty(MB001))
        {
            SQUERY.AppendFormat(@" AND T1.TA001 LIKE '%{0}%' ", MB001);
        }
        else
        {
            SQUERY.AppendFormat(@"");
        }

        if (!string.IsNullOrEmpty(MB002))
        {
            SQUERY.AppendFormat(@" AND T3.MB002 LIKE '%{0}%' ", MB002);
        }
        else
        {
            SQUERY.AppendFormat(@"");
        }

        if (!string.IsNullOrEmpty(YM) && !string.IsNullOrEmpty(SQUERY.ToString()))
        {
            cmdTxt.AppendFormat(@"
                                -- 區塊 A: 計算每個月份的詳細數據 (Detail Rows)
                                SELECT
                                    T1.TA002 AS '年月',
                                    T1.TA001 AS '品號',
                                    T3.MB002 AS '品名',
                                    T3.MB003 AS '規格',
                                    T3.MB004 AS '單位',
                                    -- 加總欄位
                                    SUM(T1.生產入庫數) AS 生產入庫數,
                                    SUM(T1.託外入庫數) AS 託外入庫數,
                                    SUM(T2.ME005) AS '在製約量_材料',
                                    SUM(T1.本階人工成本) AS 本階人工成本,
                                    SUM(T1.本階製造費用) AS 本階製造費用,
                                    SUM(T2.ME007) AS '材料成本',
                                    SUM(T2.ME008) AS '人工成本',
                                    SUM(T2.ME009) AS '製造費用',
                                    SUM(T2.ME010) AS '加工費用',

                                    -- 單位總成本計算
                                      CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數) + SUM(T2.ME005)) > 0
                                            THEN SUM(T2.ME007) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數) + SUM(T2.ME005))
                                            ELSE 0
                                        END
                                    )+
	                                CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME008) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    )+
	                                 CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME009) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    )+
	                                 CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME010) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    ) AS 單位成本,

                                    -- 單位材料成本
                                    CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數) + SUM(T2.ME005)) > 0
                                            THEN SUM(T2.ME007) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數) + SUM(T2.ME005))
                                            ELSE 0
                                        END
                                    ) AS 單位材料成本,

                                    -- 單位人工成本
                                    CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME008) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    ) AS 單位人工成本,

                                    -- 單位製造成本
                                    CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME009) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    ) AS 單位製造成本,

                                    -- 單位加工成本
                                    CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME010) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    ) AS 單位加工成本,

                                    -- 成本中心相關欄位
                                    T3.MB068,
                                    T3.MB047,

                                    -- 平均包裝人工成本
                                    (CASE
                                        WHEN T3.MB068 IN ('09') AND (SUM(T1.生產入庫數) + SUM(T2.ME005)) > 0
                                        THEN SUM(T1.本階人工成本) / (SUM(T1.生產入庫數) + SUM(T2.ME005))
                                        ELSE 0
                                    END) AS 平均包裝人工成本,

                                    -- 平均包裝製造費用
                                    (CASE
                                        WHEN T3.MB068 IN ('09') AND (SUM(T1.生產入庫數) + SUM(T2.ME005)) > 0
                                        THEN SUM(T1.本階製造費用) / (SUM(T1.生產入庫數) + SUM(T2.ME005))
                                        ELSE 0
                                    END) AS 平均包裝製造費用,

                                    -- 平均小線人工成本
                                    (CASE
                                        WHEN T3.MB068 IN ('03') AND (SUM(T1.生產入庫數) + SUM(T2.ME005)) > 0
                                        THEN SUM(T1.本階人工成本) / (SUM(T1.生產入庫數) + SUM(T2.ME005))
                                        ELSE 0
                                    END) AS 平均小線人工成本,

                                    -- 平均小線製造費用
                                    (CASE
                                        WHEN T3.MB068 IN ('03') AND (SUM(T1.生產入庫數) + SUM(T2.ME005)) > 0
                                        THEN SUM(T1.本階製造費用) / (SUM(T1.生產入庫數) + SUM(T2.ME005))
                                        ELSE 0
                                    END) AS 平均小線製造費用,

                                    -- 平均大線人工成本
                                    (CASE
                                        WHEN T3.MB068 IN ('02') AND (SUM(T1.生產入庫數) + SUM(T2.ME005)) > 0
                                        THEN SUM(T1.本階人工成本) / (SUM(T1.生產入庫數) + SUM(T2.ME005))
                                        ELSE 0
                                    END) AS 平均大線人工成本,

                                    -- 平均大線製造費用
                                    (CASE
                                        WHEN T3.MB068 IN ('02') AND (SUM(T1.生產入庫數) + SUM(T2.ME005)) > 0
                                        THEN SUM(T1.本階製造費用) / (SUM(T1.生產入庫數) + SUM(T2.ME005))
                                        ELSE 0
                                    END) AS 平均大線製造費用,

                                    -- 彙總生產工時
                                    SUM(T4.SUMPROTIMES) AS 'SUMPROTIMES'

                                FROM
                                    -- 子查詢 T1、T2、T3、T4 ... (FROM/JOIN 區塊與原查詢相同)
                                    (
                                        SELECT TA002, TA001, SUM(TA012) AS '生產入庫數', SUM(TA013) AS '託外入庫數', 
                                               SUM(TA016 - TA019) AS '本階人工成本', SUM(TA017 - TA020) AS '本階製造費用'
                                        FROM [TK].dbo.CSTTA
                                        WHERE TA002 LIKE '{0}%'
                                        GROUP BY TA002, TA001
                                    ) AS T1
                                LEFT JOIN [TK].dbo.CSTME AS T2 ON T2.ME001 = T1.TA001 AND T2.ME002 = T1.TA002
                                LEFT JOIN [TK].dbo.INVMB AS T3 ON T3.MB001 = T1.TA001
                                LEFT JOIN 
                                    (
                                        SELECT SUBSTRING(MB002,1,6) MB002,MB007, ISNULL(SUM(MB005 + MB006), 0) AS SUMPROTIMES
										FROM [TK].dbo.CSTMB
										GROUP BY SUBSTRING(MB002,1,6),MB007
                                    ) AS T4 ON T4.MB007 = T1.TA001 AND T4.MB002 LIKE T1.TA002 + '%'

                                WHERE 1=1
                                   {1}

                                GROUP BY
                                    T1.TA002, T1.TA001, T3.MB002, T3.MB003, T3.MB004, T3.MB068, T3.MB047
                                HAVING
                                    (SUM(T1.生產入庫數) + SUM(T1.託外入庫數) + SUM(T2.ME005)) > 0 

                                UNION ALL

                                -- 區塊 B: 計算總計行 (Total Row)
                                SELECT
                                    '小計' AS '年月', -- 標記為 '小計'
                                    MAX(T1.TA001) AS '品號', -- 修正 8120 錯誤：使用 MAX()
                                    MAX(T3.MB002) AS '品名', -- 修正 8120 錯誤：使用 MAX()
                                    MAX(T3.MB003) AS '規格', -- 修正 8120 錯誤：使用 MAX()
                                    MAX(T3.MB004) AS '單位', -- 修正 8120 錯誤：使用 MAX()
    
                                    -- 總計行的 SUM 聚合欄位
                                    SUM(T1.生產入庫數) AS 生產入庫數,
                                    SUM(T1.託外入庫數) AS 託外入庫數,
                                    SUM(T2.ME005) AS '在製約量_材料',
                                    SUM(T1.本階人工成本) AS 本階人工成本,
                                    SUM(T1.本階製造費用) AS 本階製造費用,
                                    SUM(T2.ME007) AS '材料成本',
                                    SUM(T2.ME008) AS '人工成本',
                                    SUM(T2.ME009) AS '製造費用',
                                    SUM(T2.ME010) AS '加工費用',

                                    -- 單位總成本計算
                                    CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數) + SUM(T2.ME005)) > 0
                                            THEN SUM(T2.ME007) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數) + SUM(T2.ME005))
                                            ELSE 0
                                        END
                                    )+
	                                CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME008) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    )+
	                                 CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME009) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    )+
	                                 CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME010) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    )
	                                AS 單位成本,

                                    -- 單位材料成本
                                    CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數) + SUM(T2.ME005)) > 0
                                            THEN SUM(T2.ME007) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數) + SUM(T2.ME005))
                                            ELSE 0
                                        END
                                    ) AS 單位材料成本,

                                    -- 單位人工成本
                                    CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME008) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    ) AS 單位人工成本,

                                    -- 單位製造成本
                                    CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME009) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    ) AS 單位製造成本,

                                    -- 單位加工成本
                                    CONVERT(DECIMAL(16, 2),
                                        CASE
                                            WHEN (SUM(T1.生產入庫數) + SUM(T1.託外入庫數)) > 0
                                            THEN SUM(T2.ME010) / (SUM(T1.生產入庫數) + SUM(T1.託外入庫數))
                                            ELSE 0
                                        END
                                    ) AS 單位加工成本,

                                    -- 非聚合欄位在總計行應為 NULL
                                    NULL AS MB068,
                                    NULL AS MB047,

                                    -- 平均成本類欄位 (因為 MB068 是 NULL，所以平均成本結果會是 0)
                                    0 AS 平均包裝人工成本,
                                    0 AS 平均包裝製造費用,
                                    0 AS 平均小線人工成本,
                                    0 AS 平均小線製造費用,
                                    0 AS 平均大線人工成本,
                                    0 AS 平均大線製造費用,

                                    -- 彙總生產工時
                                    SUM(T4.SUMPROTIMES) AS 'SUMPROTIMES'

                                FROM
                                    -- 子查詢 T1、T2、T3、T4 ... (FROM/JOIN 區塊與區塊 A 相同)
                                    (
                                        SELECT TA002, TA001, SUM(TA012) AS '生產入庫數', SUM(TA013) AS '託外入庫數', 
                                               SUM(TA016 - TA019) AS '本階人工成本', SUM(TA017 - TA020) AS '本階製造費用'
                                        FROM [TK].dbo.CSTTA
                                        WHERE TA002 LIKE '{0}%'
                                        GROUP BY TA002, TA001
                                    ) AS T1
                                LEFT JOIN [TK].dbo.CSTME AS T2 ON T2.ME001 = T1.TA001 AND T2.ME002 = T1.TA002
                                LEFT JOIN [TK].dbo.INVMB AS T3 ON T3.MB001 = T1.TA001
                                LEFT JOIN 
                                    (
                                        SELECT SUBSTRING(MB002,1,6) MB002,MB007, ISNULL(SUM(MB005 + MB006), 0) AS SUMPROTIMES
										FROM [TK].dbo.CSTMB
										GROUP BY SUBSTRING(MB002,1,6),MB007
                                    ) AS T4 ON T4.MB007 = T1.TA001 AND T4.MB002 LIKE T1.TA002 + '%'

                                WHERE 1=1
                                   {1}

                                HAVING
                                    -- 確保總計的分母不是零 (如果總數為零，則不顯示總計行)
                                    (SUM(T1.生產入庫數) + SUM(T1.託外入庫數) + SUM(T2.ME005)) > 0


 

                                    ", YM, SQUERY.ToString());

            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            Grid1.DataSource = dt;
            Grid1.DataBind();

        }
        else
        {

        }



    }




    protected void grid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 找到 KINDS Label 控制項
            Label lblKinds = (Label)e.Row.FindControl("年月");

            // 檢查是否找到了 Label 控制項
            if (lblKinds != null)
            {
                string KINDS = lblKinds.Text;
                if (KINDS.Equals("小計"))
                {
                    e.Row.BackColor = System.Drawing.Color.LightPink;
                }
            }
        }

    }
    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        //if (e.CommandName == "GWButton1")
        //{

        //    BindGrid1("");

        //}


    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

        SETEXCEL();

    }

    private void BindGrid2(string YM, string MB001, string MB002)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder SQUERY = new StringBuilder();



        //查詢條件
        if (!string.IsNullOrEmpty(MB001))
        {
            //SQUERY.AppendFormat(@"   AND 成品品號 LIKE '%{0}%'", MB001);
            SQUERY.AppendFormat(@"
                                AND 成品品號 IN
                                    (
                                    SELECT TA001 
                                    FROM 
                                    (
                                    SELECT TA002,TA001,SUM(TA012) '生產入庫數'
                                    FROM [TK].dbo.CSTTA
                                    WHERE TA002 LIKE '{0}%'
                                    GROUP BY TA002,TA001
                                    ) AS TEMP
                                    LEFT JOIN [TK].dbo.CSTME ON ME001=TA001 AND ME002=TA002
                                    LEFT JOIN [TK].dbo.INVMB ON MB001=TA001
                                    WHERE 1=1
                                    AND MB001 LIKE '%{1}%'

                                    AND (生產入庫數+ME005)>0
                                    GROUP BY TA001
                                    )
                                    ", YM, MB001);
        }
        else
        {
            SQUERY.AppendFormat(@"");
        }

        if (!string.IsNullOrEmpty(MB002))
        {
            //SQUERY.AppendFormat(@"  AND 成品品名 LIKE '%{0}%'", MB002);

            SQUERY.AppendFormat(@"
                                AND 成品品號 IN
                                    (
                                    SELECT TA001 
                                    FROM 
                                    (
                                    SELECT TA002,TA001,SUM(TA012) '生產入庫數'
                                    FROM [TK].dbo.CSTTA
                                    WHERE TA002 LIKE '{0}%'
                                    GROUP BY TA002,TA001
                                    ) AS TEMP
                                    LEFT JOIN [TK].dbo.CSTME ON ME001=TA001 AND ME002=TA002
                                    LEFT JOIN [TK].dbo.INVMB ON MB001=TA001
                                    WHERE 1=1
                                    AND MB002 LIKE '%{1}%'

                                    AND (生產入庫數+ME005)>0
                                    GROUP BY TA001
                                    )
                                    ", YM, MB002);
        }
        else
        {
            SQUERY.AppendFormat(@"");
        }

        if (!string.IsNullOrEmpty(YM) && !string.IsNullOrEmpty(SQUERY.ToString()))
        {
            cmdTxt.AppendFormat(@"


                                   SELECT *
                                    ,CONVERT(NVARCHAR,CONVERT(DECIMAL(16,4),(CASE WHEN 總成品平均成本>0 THEN 分攤成本/總成品平均成本 ELSE 0 END))*100)+'%' AS '各百分比' 
                                    ,CONVERT(DECIMAL(16,2),分攤成本) AS 分攤成本
                                    FROM 
                                    (
                                    SELECT '{0}'AS '年度',MC001 AS '成品品號',MB1.MB002  AS '成品品名',MB1.MB003  AS '成品規格',MB1.MB004  AS '成品單位' ,MC004,MD003  AS '使用品號',MB2.MB002  AS '使用品名',MD006,MD007
                                    ,總成品平均成本
                                    ,材料平均成本
                                    ,人工平均成本
                                    ,製造平均成本
                                    ,加工平均成本
                                    ,各採購單位成本
                                    ,總採購單位成本
                                    ,總半成品重
                                    ,(CASE WHEN 總成品平均成本>0 THEN (CASE WHEN (MB2.MB001 LIKE '3%' OR MB2.MB001 LIKE '4%')THEN ((材料平均成本-總採購單位成本)*MD006/MD007/總半成品重) ELSE 各採購單位成本*MD006/MD007/MC004 END) ELSE 0 END) AS '分攤成本' 
                                    ,(CASE WHEN MD003 LIKE '1%' THEN '1原料'  WHEN MD003 LIKE '2%' THEN '2物料' WHEN (MD003 LIKE '3%' OR MD003 LIKE '4%') THEN '3半成品'END ) AS '分類'
                                    FROM
                                    (
                                    SELECT MC001,MC004,MD003,MD006,MD007
                                    ,ISNULL((SELECT AVG((ME007)/(ME003+ME005+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MD001 AND (ME003+ME005+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0)+
                                        ISNULL((SELECT AVG((ME008)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MD001 AND (ME003+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) +
                                        ISNULL((SELECT AVG((ME009)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MD001 AND (ME003+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0)+
                                        ISNULL((SELECT AVG((ME010)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MD001 AND (ME003+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0)
                                        AS '總成品平均成本'
                                    ,ISNULL((SELECT AVG((ME007)/(ME003+ME005+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MD001 AND (ME003+ME005+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '材料平均成本'
                                    ,ISNULL((SELECT AVG((ME008)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MD001 AND (ME003+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '人工平均成本'
                                    ,ISNULL((SELECT AVG((ME009)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MD001 AND (ME003+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '製造平均成本'
                                    ,ISNULL((SELECT AVG((ME010)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MD001 AND (ME003+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '加工平均成本'
                                    ,(CASE WHEN ( MB2.MB001 LIKE '1%' OR MB2.MB001 LIKE '2%') AND MB2.MB064>0 AND MB2.MB065 >0 THEN MB2.MB065/MB2.MB064*MD006/MD007/MC004 ELSE MB2.MB050*MD006/MD007/MC004 END ) AS '各採購單位成本'
                                    ,(SELECT SUM (CASE WHEN  ( MB001 LIKE '1%' OR MB001 LIKE '2%') AND MB064>0 AND MB065 >0 THEN MB065/MB064*MD006/MD007/MC004 ELSE MB050*MD006/MD007/MC004 END) FROM [TK].dbo.BOMMC MC, [TK].dbo.BOMMD MD ,[TK].dbo.INVMB MB WHERE  MC.MC001=MD.MD001 AND MD.MD003=MB.MB001 AND MC.MC001=BOMMC.MC001)   AS '總採購單位成本'
                                    ,ISNULL((SELECT SUM (MD006/MD007) FROM [TK].dbo.BOMMC MC, [TK].dbo.BOMMD MD ,[TK].dbo.INVMB MB WHERE  MC.MC001=MD.MD001 AND MD.MD003=MB.MB001 AND MC.MC001=BOMMC.MC001 AND (MB.MB001 LIKE '3%' OR MB.MB001 LIKE '4%')),0)  AS '總半成品重'
                                    FROM [TK].dbo.BOMMC
                                    LEFT JOIN [TK].dbo.INVMB MB1 ON MB1.MB001=BOMMC.MC001
                                    , [TK].dbo.BOMMD
                                    LEFT JOIN [TK].dbo.INVMB MB2 ON MB2.MB001=BOMMD.MD003
                                    WHERE MC001=MD001
                                    ) AS TEMP
                                    LEFT JOIN [TK].dbo.INVMB MB1 ON MB1.MB001=TEMP.MC001
                                    LEFT JOIN [TK].dbo.INVMB MB2 ON MB2.MB001=TEMP.MD003
                                    UNION ALL
                                    SELECT '{0}',MC001 AS '成品品號',MB002  AS '成品品名',MB003  AS '成品規格',MB004  AS '成品單位',0 ,''  AS '使用品號','' AS '使用品名',0,0
                                    ,ISNULL((SELECT AVG((ME007+ME008+ME009+ME010)/(ME003+ME005+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME005+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '總成品平均成本'
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,ISNULL((SELECT AVG((ME008)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '成本'
                                    ,'4人工' AS '分類'
                                    FROM [TK].dbo.BOMMC,[TK].dbo.INVMB
                                    WHERE  MC001=MB001
                                    AND (MC001 LIKE '3%' OR MC001 LIKE '4%' OR MC001 LIKE '5%') 
                                    UNION ALL
                                    SELECT '{0}',MC001 AS '成品品號',MB002  AS '成品品名',MB003  AS '成品規格',MB004  AS '成品單位',0 ,''  AS '使用品號','' AS '使用品名',0,0
                                    ,ISNULL((SELECT AVG((ME007+ME008+ME009+ME010)/(ME003+ME005+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME005+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '總成品平均成本'
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,ISNULL((SELECT AVG((ME009)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '成本'
                                    ,'5製造' AS '分類'
                                    FROM [TK].dbo.BOMMC,[TK].dbo.INVMB
                                    WHERE  MC001=MB001
                                    AND (MC001 LIKE '3%' OR MC001 LIKE '4%' OR MC001 LIKE '5%') 
                                    UNION ALL
                                    SELECT '{0}',MC001 AS '成品品號',MB002  AS '成品品名',MB003  AS '成品規格',MB004  AS '成品單位',0 ,''  AS '使用品號','' AS '使用品名',0,0
                                    ,ISNULL((SELECT AVG((ME007+ME008+ME009+ME010)/(ME003+ME005+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME005+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '總成品平均成本'
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,ISNULL((SELECT AVG((ME010)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '成本'
                                    ,'6加工' AS '分類'
                                    FROM [TK].dbo.BOMMC,[TK].dbo.INVMB
                                    WHERE  MC001=MB001
                                    AND (MC001 LIKE '3%' OR MC001 LIKE '4%' OR MC001 LIKE '5%') 

                                    UNION ALL
                                    SELECT '{0}',MC001 AS '成品品號',MB002  AS '成品品名',MB003  AS '成品規格',MB004  AS '成品單位',0 ,''  AS '使用品號','' AS '使用品名',0,0
                                    ,ISNULL((SELECT AVG((ME007+ME008+ME009+ME010)/(ME003+ME005+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME005+ME004)>0 AND (ME007+ME008+ME009+ME010)>0 AND ME002 LIKE '{0}%'),0) AS '總成品平均成本'
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,0
                                    ,ISNULL((SELECT AVG((ME007)/(ME003+ME005+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME005+ME004)>0 AND (ME007)>0 AND ME002 LIKE '{0}%'),0) +
ISNULL((SELECT AVG((ME008)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME004)>0 AND (ME008)>0 AND ME002 LIKE '{0}%'),0)+
ISNULL((SELECT AVG((ME009)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME004)>0 AND (ME009)>0 AND ME002 LIKE '{0}%'),0)+
ISNULL((SELECT AVG((ME010)/(ME003+ME004)) FROM [TK].dbo.CSTME WHERE  ME001=MC001 AND (ME003+ME004)>0 AND (ME010)>0 AND ME002 LIKE '{0}%'),0)
AS '成本'
                                    ,'9合計' AS '分類'
                                    FROM [TK].dbo.BOMMC,[TK].dbo.INVMB
                                    WHERE  MC001=MB001
                                    AND (MC001 LIKE '3%' OR MC001 LIKE '4%' OR MC001 LIKE '5%') 
                                ) AS TEMP2
                                WHERE 1=1
                                {1}
                                ORDER BY 成品品號,分類,使用品號

                                    ", YM, SQUERY.ToString());

            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            Grid2.DataSource = dt;
            Grid2.DataBind();


        }
        else
        {

        }



    }

    protected void grid2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string KINDS = e.Row.Cells[5].Text.ToString();
            if (KINDS.Equals("9合計"))
            {
                e.Row.BackColor = System.Drawing.Color.LightPink;
            }
        }

    }
    protected void Grid2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        //if (e.CommandName == "GWButton1")
        //{

        //    BindGrid1("");

        //}


    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {



    }

    private void BindGrid3(string MB001,string YEARS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder SQUERY = new StringBuilder();



        ////查詢條件
        //if (!string.IsNullOrEmpty(MB001))
        //{
        //    //SQUERY.AppendFormat(@"   AND 成品品號 LIKE '%{0}%'", MB001);     
        //}
        //else
        //{
        //    SQUERY.AppendFormat(@"");
        //}



        if (!string.IsNullOrEmpty(MB001))
        {
            cmdTxt.AppendFormat(@"
                              SELECT MC001 AS '成品品號',MB1.MB002 AS '成品品名',MD003 AS '組件品號',MB2.MB002 AS '組件品名',MB2.MB004 AS '組件單位',CONVERT(decimal(16,2),MB2.MB050) AS '最近進價',MB2.MB102  AS '進價是否含稅',MC004 AS '標準批量',MD006 AS '組成用量',MD007 AS '底數',MD008 AS '損秏率'
                            ,(SELECT TOP 1 '最近進貨日:'+TG003+' 廠商:'+TG005+' '+MA002 FROM [TK].dbo.PURTG,[TK].dbo.PURTH,[TK].dbo.PURMA WHERE TG001=TH001 AND TG002=TH002 AND TG005=MA001 AND TH004=MD003 ORDER BY TG003 DESC) AS 'MA002'
                            ,(CASE WHEN MD003 LIKE '1%' OR MD003 LIKE '2%' THEN(CONVERT(decimal(16,2),MB2.MB050*MD006/MD007*(1+MD008)/MC004)) ELSE 0 END) AS '分攤單位進貨成本'
                            ,CONVERT(decimal(16,2),(CASE WHEN MD003 NOT LIKE '1%' THEN 
                                (CASE WHEN MD003 NOT LIKE '2%' THEN 
                                ((SELECT AVG(LB010) FROM [TK].dbo.INVLB WHERE LB001=MD003 AND LB002 LIKE '{1}%' GROUP BY LB001)*MD006/MD007*(1+MD008)/MC004) 
                                ELSE 0 END)
                                ELSE 0 END)) AS '非採購單位成本'

                            ,CONVERT(decimal(16,2),(SELECT SUM(MB050*MD006/MD007*(1+MD008)/MC004) FROM [TK].dbo.BOMMC MC,[TK].dbo.BOMMD MD,[TK].dbo.INVMB MB WHERE MC.MC001=MD.MD001 AND MB.MB001=MD.MD003 AND MD.MD001=BOMMC.MC001 ))  AS '成品單位進貨成本'
                            ,CONVERT(decimal(16,2),(SELECT AVG(LB010) LB010
                                FROM [TK].dbo.INVLB
                                WHERE LB001=MC001
                                AND LB002 LIKE '{1}%'
                                GROUP BY LB001)) AS '單位成本-材料'
                            FROM [TK].dbo.BOMMC
                            LEFT JOIN [TK].dbo.INVMB MB1 ON MB1.MB001=MC001
                            ,[TK].dbo.BOMMD
                            LEFT JOIN [TK].dbo.INVMB MB2 ON MB2.MB001=MD003
                            WHERE MC001=MD001
                            AND (MC001 LIKE '%{0}%' OR MB1.MB002 LIKE '%{0}%')
                            ORDER BY MC001,MD003






                                    ", MB001, YEARS);

            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            Grid3.DataSource = dt;
            Grid3.DataBind();


        }
        else
        {

        }



    }

    protected void grid3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid3_RowDataBound(object sender, GridViewRowEventArgs e)
    {


    }
    protected void Grid3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        //if (e.CommandName == "GWButton1")
        //{

        //    BindGrid1("");

        //}


    }

    public void OnBeforeExport3(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {



    }


    public void SETEXCEL()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        //
        //if (!string.IsNullOrEmpty(TextBox1.Text))
        //{
        //    QUERYS.AppendFormat(@"", TextBox1.Text);
        //}


        cmdTxt.AppendFormat(@" 
                            
                               
                                ", QUERYS.ToString());



        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        if (dt.Rows.Count > 0)
        {
            //檔案名稱
            var fileName = "資料" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知

            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {

                // 建立分頁
                var ws = excel.Workbook.Worksheets.Add("list" + DateTime.Now.ToShortDateString());


                //預設行高
                //ws.DefaultRowHeight = 60;

                // 寫入資料試試
                //ws.Cells[2, 1].Value = "測試測試";
                int ROWS = 2;
                int COLUMNS = 1;


                //excel標題
                ws.Cells[1, 1].Value = "目前簽核者";
                ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                ws.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                ws.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線


                foreach (DataRow od in dt.Rows)
                {
                    ws.Cells[ROWS, 1].Value = od["NAME"].ToString();
                    ws.Cells[ROWS, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; //高度置中
                    ws.Cells[ROWS, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); //儲存格框線


                    ROWS++;
                }




                ////預設列寬、行高
                //sheet.DefaultColWidth = 10; //預設列寬
                //sheet.DefaultRowHeight = 30; //預設行高

                //// 遇\n或(char)10自動斷行
                //ws.Cells.Style.WrapText = true;

                //自適應寬度設定
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                //自適應高度設定
                ws.Row(1).CustomHeight = true;



                //儲存Excel
                //Byte[] bin = excel.GetAsByteArray();
                //File.WriteAllBytes(@"C:\TEMP\" + fileName, bin);

                //儲存和歸來的Excel檔案作為一個ByteArray
                var data = excel.GetAsByteArray();
                HttpResponse response = HttpContext.Current.Response;
                Response.Clear();

                //輸出標頭檔案　　
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName + "");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(data);
                Response.Flush();
                Response.End();
                //package.Save();//這個方法是直接下載到本地
            }
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知
            //                                                            // 沒設置的話會跳出 Please set the excelpackage.licensecontext property


            ////var file = new FileInfo(fileName);
            //using (var excel = new ExcelPackage(file))
            //{

            //}
        }

    }

    private void BindGrid4(string MB002, string ISCLOSED)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder SQUERY = new StringBuilder();



        //查詢條件
        if (!string.IsNullOrEmpty(MB002))
        {
            SQUERY.AppendFormat(@" AND MB002 LIKE '%{0}%' ", MB002);
        }
        else
        {
            SQUERY.AppendFormat(@"");
        }

        if (!string.IsNullOrEmpty(ISCLOSED))
        {
            SQUERY.AppendFormat(@" AND [ISCLOSED]='{0}' ", ISCLOSED);
        }
        else
        {
            SQUERY.AppendFormat(@"");
        }

        if (!string.IsNullOrEmpty(SQUERY.ToString()))
        {
            cmdTxt.AppendFormat(@"
                                 SELECT 
                                [MB001] AS '品號'
                                ,[MB002] AS '品名'
                                ,[MB003] AS '規格'
                                ,[COSTROW] AS '單位原料成本'
                                ,[COSTMAT] AS '單位物料成本'
                                ,[COSTHR] AS '單位人工成本'
                                ,[COSTMANU] AS '單位製造成本'
                                ,[COSTPRO] AS '單位加工成本'
                                ,([COSTROW]+[COSTMAT]+[COSTHR]+[COSTMANU]+[COSTPRO])  AS '單位成本'
                                ,[COMMEMTS] AS '備註'
                                ,[ISCLOSED] AS '是否結案'
                                ,(
                                SELECT '品名:'+[MB002]+' 規格:'+[MB003]+' 單價:'+CONVERT(nvarchar,[COSTROW])+CHAR(10)
                                FROM [TKRESEARCH].[dbo].[TBCOSTRECORDSROWS] 
                                WHERE MMB001=[TKRESEARCH].[dbo].[TBCOSTRECORDS].[MB001]
                                FOR XML PATH('')
                                ) AS 'TBCOSTRECORDSROWS'
                                ,(
                                SELECT '品名:'+[MB002]+' 規格:'+[MB003]+' 單價:'+CONVERT(nvarchar,[COSTROW])+CHAR(10)
                                FROM [TKRESEARCH].[dbo].TBCOSTRECORDSMAT 
                                WHERE MMB001=[TKRESEARCH].[dbo].[TBCOSTRECORDS].[MB001]
                                FOR XML PATH('')
                                ) AS 'TBCOSTRECORDSMAT'

                                FROM [TKRESEARCH].[dbo].[TBCOSTRECORDS]
                                WHERE 1=1
                                {0}

                                ORDER BY [MB001] 

 

                                    ", SQUERY.ToString());

            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            Grid4.DataSource = dt;
            Grid4.DataBind();

        }
        else
        {

        }



    }




    protected void grid4_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Button1
            //Get the button that raised the event
            Button btn = (Button)e.Row.FindControl("GV4Button1");
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue = btn.CommandArgument;
            DataRowView row = (DataRowView)e.Row.DataItem;
            Button lbtnName = (Button)e.Row.FindControl("GV4Button1");
            ExpandoObject param = new { ID = Cellvalue }.ToExpando();
            //Grid開窗是用RowDataBound事件再開窗
            Dialog.Open2(lbtnName, "~/CDS/WebPage/RESEARCH/TKRESEARCH_COSTDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

            //Button1
            //Get the button that raised the event
            btn = (Button)e.Row.FindControl("GV4Button2");
            //Get the row that contains this button
            gvr = (GridViewRow)btn.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            Cellvalue = btn.CommandArgument;
            row = (DataRowView)e.Row.DataItem;
            lbtnName = (Button)e.Row.FindControl("GV4Button2");
            param = new { ID = Cellvalue }.ToExpando();
            //Grid開窗是用RowDataBound事件再開窗
            //Dialog.Open2(lbtnName, "~/CDS/WebPage/RESEARCH/TKRESEARCH_COSTDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
            Dialog.Open2(lbtnName, "~/CDS/WebPage/RESEARCH/TKRESEARCH_COSTDialogROWS.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

            //Button1
            //Get the button that raised the event
            btn = (Button)e.Row.FindControl("GV4Button3");
            //Get the row that contains this button
            gvr = (GridViewRow)btn.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            Cellvalue = btn.CommandArgument;
            row = (DataRowView)e.Row.DataItem;
            lbtnName = (Button)e.Row.FindControl("GV4Button3");
            param = new { ID = Cellvalue }.ToExpando();
            //Grid開窗是用RowDataBound事件再開窗
            //Dialog.Open2(lbtnName, "~/CDS/WebPage/RESEARCH/TKRESEARCH_COSTDialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
            Dialog.Open2(lbtnName, "~/CDS/WebPage/RESEARCH/TKRESEARCH_COSTDialogMATS.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);


        }


        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
        }
    }
    protected void Grid4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "GV4Button1")
        {

            BindGrid4(TextBox3.Text.ToString(), DropDownList1.Text.ToString());

        }
        if (e.CommandName == "GV4Button2")
        {

            BindGrid4(TextBox3.Text.ToString(), DropDownList1.Text.ToString());

        }
        if (e.CommandName == "GV4Button3")
        {

            BindGrid4(TextBox3.Text.ToString(), DropDownList1.Text.ToString());

        }


    }


    public void OnBeforeExport4(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

        //SETEXCEL();

    }
    public void ADD_TBCOSTRECORDS(
                           string MB002, string MB003, string COSTROW, string COSTMAT, string COSTHR, string COSTMANU, string COSTPRO, string COMMEMTS, string ISCLOSED
                              )
    {


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                       INSERT [TKRESEARCH].[dbo].[TBCOSTRECORDS]
                       ([MB002],[MB003],[COSTROW],[COSTMAT],[COSTHR],[COSTMANU],[COSTPRO],[COMMEMTS],[ISCLOSED])
                        VALUES
                        (@MB002,@MB003,@COSTROW,@COSTMAT,@COSTHR,@COSTMANU,@COSTPRO,@COMMEMTS,@ISCLOSED)
              
                       
                        
                            ";

       
        m_db.AddParameter("@MB002", MB002);
        m_db.AddParameter("@MB003", MB003);
        m_db.AddParameter("@COSTROW", COSTROW);
        m_db.AddParameter("@COSTMAT", COSTMAT);
        m_db.AddParameter("@COSTHR", COSTHR);
        m_db.AddParameter("@COSTMANU", COSTMANU);
        m_db.AddParameter("@COSTPRO", COSTPRO);
        m_db.AddParameter("@COMMEMTS", COMMEMTS);
        m_db.AddParameter("@ISCLOSED", ISCLOSED);


        m_db.ExecuteNonQuery(cmdTxt);


    }

    public void MsgBox(String ex, Page pg, Object obj)
    {
        //MsgBox("Button1", this.Page, this);
        string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        Type cstype = obj.GetType();
        ClientScriptManager cs = pg.ClientScript;
        cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }

    #endregion

    #region BUTTON

    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid1(txtDate1.Text.ToString(), TextBox1.Text.ToString(), TextBox2.Text.ToString());
        BindGrid2(txtDate1.Text.ToString(), TextBox1.Text.ToString(), TextBox2.Text.ToString());
    }

    protected void btn2_Click(object sender, EventArgs e)
    {
        BindGrid3(TextBox4.Text.ToString(), txtDate2.Text.ToString());
    }

    protected void btn3_Click(object sender, EventArgs e)
    {
        BindGrid4(TextBox3.Text.ToString(), DropDownList1.Text.ToString());
    }
    protected void btn4_Click(object sender, EventArgs e)
    {
        //MsgBox("OK", this.Page, this);

        string MB002 = TextBox5.Text;
        string MB003 = TextBox6.Text;
        string COSTROW = TextBox7.Text;
        string COSTMAT = TextBox8.Text;
        string COSTHR = TextBox9.Text;
        string COSTMANU = TextBox10.Text;
        string COSTPRO = TextBox11.Text;
        string COMMEMTS = TextBox12.Text;
        string ISCLOSED = "N";

        ADD_TBCOSTRECORDS(MB002, MB003, COSTROW, COSTMAT, COSTHR, COSTMANU, COSTPRO, COMMEMTS, ISCLOSED);

        BindGrid4(TextBox3.Text.ToString(), DropDownList1.Text.ToString());

        PrivateMessageUCO SEND = new PrivateMessageUCO();
        SEND.SendOneNewMessage("","","","");
    }


    #endregion
}