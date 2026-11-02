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
using System.Web.UI.HtmlControls;
using Ede.Uof.EIP.SystemInfo;

public partial class CDS_WebPage_TKBUSINESS_TK_TB_EIP_FORUM_ARTICLE : Ede.Uof.Utility.Page.BasePage
{
   
    string ACCOUNT = null;
    string NAME = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        ACCOUNT = Current.Account;
        NAME = Current.User.Name;

        

        if (!IsPostBack)
        {
            TextBox1.Text = DateTime.Now.Year.ToString();
            BindGrid(TextBox1.Text, TextBox2.Text);
        }
        else
        {

        }




    }
    #region FUNCTION
   

    private void BindGrid(string YEARS,string SUBJECTS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder Query1 = new StringBuilder();
        StringBuilder Query2 = new StringBuilder();

        if (!string.IsNullOrEmpty(YEARS))
        {
            Query1.AppendFormat(@" AND  CONVERT(NVARCHAR,TB_EIP_FORUM_TOPIC.CREATE_DATE,112)>='{0}'", YEARS+"0101");
        }
        else
        {
            Query1.AppendFormat(@"");
        }
        if (!string.IsNullOrEmpty(SUBJECTS))
        {
            Query2.AppendFormat(@" AND TB_EIP_FORUM_ARTICLE.SUBJECT LIKE '%{0}%'", SUBJECTS);
        }
        else
        {
            Query2.AppendFormat(@"");
        }

        cmdTxt.AppendFormat(@"
                          -- - 20240514 找討論區文章

                            SELECT TB_EIP_FORUM_BOARD.BOARD_NAME
                            ,CONVERT(NVARCHAR,TB_EIP_FORUM_TOPIC.CREATE_DATE,112) CREATE_DATE
                            ,TB_EIP_FORUM_ARTICLE.SUBJECT
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('設計'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '設計'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('財務'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '財務'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('生管'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '生管'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('資材'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '資材'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('品保'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '品保'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('研發'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '研發'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('硯微墨'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '硯微墨'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('門市'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '門市'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('觀光'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '觀光'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('國外'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '國外'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('國內'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '國內'
                            ,ISNULL((
                            SELECT TOP 1
                            [NAME]+':'+CHAR(13)+CHAR(10)+CONVERT(NVARCHAR,[CREATE_DATE],112)+CHAR(13)+CHAR(10)++REPLACE(dbo.udf_StripHTML([cleaned_img_content]),'&nbsp;','')
                            FROM [UOF].[dbo].[View_SUB_TB_EIP_FORUM_ARTICLE]
                            WHERE [View_SUB_TB_EIP_FORUM_ARTICLE].[SUBJECT]=TB_EIP_FORUM_ARTICLE.SUBJECT
                            AND [View_SUB_TB_EIP_FORUM_ARTICLE].[GROUP_NAME] IN (SELECT [DEPNAMES] FROM [UOF].[dbo].[Z_UOF_FORUM_ARTICLE_DEP] WHERE [DEPKINDS] IN ('新事業發展'))
                            ORDER BY [View_SUB_TB_EIP_FORUM_ARTICLE].[FLOORS] DESC
                            ),'') AS '新事業發展'

                            FROM [UOF].dbo.TB_EIP_FORUM_AREA
                            ,[UOF].dbo.TB_EIP_FORUM_BOARD
                            ,[UOF].dbo.TB_EIP_FORUM_TOPIC
                            ,[UOF].dbo.TB_EIP_FORUM_ARTICLE
                            ,[UOF].[dbo].[TB_EB_USER]
                            ,[UOF].[dbo].[TB_EB_EMPL_DEP]
                            ,[UOF].[dbo].[TB_EB_GROUP]
                            WHERE TB_EIP_FORUM_AREA.AREA_GUID=TB_EIP_FORUM_BOARD.AREA_GUID
                            AND TB_EIP_FORUM_BOARD.BOARD_GUID=TB_EIP_FORUM_TOPIC.BOARD_GUID
                            AND TB_EIP_FORUM_ARTICLE.TOPIC_GUID=TB_EIP_FORUM_TOPIC.TOPIC_GUID
                            AND TB_EB_USER.USER_GUID=TB_EIP_FORUM_ARTICLE.ANNOUNCER
                            AND TB_EB_EMPL_DEP.USER_GUID=TB_EB_USER.USER_GUID AND TB_EB_EMPL_DEP.ORDERS='0'
                            AND TB_EB_GROUP.GROUP_ID=TB_EB_EMPL_DEP.GROUP_ID
                            --AND TB_EIP_FORUM_ARTICLE.SUBJECT='20240425-01 2024秋暮追月中秋禮盒(門市款)'
                            AND (TB_EIP_FORUM_BOARD.BOARD_NAME LIKE '%校稿%' OR TB_EIP_FORUM_BOARD.BOARD_NAME LIKE '%設計%' )
                            {0}
                            {1}

                            GROUP BY TB_EIP_FORUM_BOARD.BOARD_NAME,CONVERT(NVARCHAR,TB_EIP_FORUM_TOPIC.CREATE_DATE,112),TB_EIP_FORUM_ARTICLE.SUBJECT
                            ORDER BY TB_EIP_FORUM_BOARD.BOARD_NAME,CONVERT(NVARCHAR,TB_EIP_FORUM_TOPIC.CREATE_DATE,112),TB_EIP_FORUM_ARTICLE.SUBJECT

                              
                            ", Query1.ToString(), Query2.ToString());


        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid("");
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {   
       
    }

    protected void Grid1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();


    }


    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    public void SETEXCEL()
    {
        

    }

    public void MsgBox(String ex, Page pg, Object obj)
    {
        string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
        Type cstype = obj.GetType();
        ClientScriptManager cs = pg.ClientScript;
        cs.RegisterClientScriptBlock(cstype, s, s.ToString());
    }

   
    #endregion

    #region BUTTON
    protected void btn_Click(object sender, EventArgs e)
    {


        //開窗後回傳參數
        if (!string.IsNullOrEmpty(Dialog.GetReturnValue()))
        {
            //txtReturnValue.Text = Dialog.GetReturnValue();
        }

    }


    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid(TextBox1.Text, TextBox2.Text);
    }



    #endregion
}