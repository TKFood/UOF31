using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CDS_WebPart_UC_TB_EIP_SCH_WORK : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime FirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime LastDay = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1);
         //   DateTime.DaysInMonth()
            BindGrid(FirstDay.ToString("yyyy/MM/dd"), LastDay.ToString("yyyy/MM/dd"));

            txtDate1.SelectedDate = FirstDay;
            txtDate2.SelectedDate = LastDay;
        }
        else
        {
            BindGrid(txtDate1.SelectedDate.Value.ToString("yyyy/MM/dd"), txtDate2.SelectedDate.Value.ToString("yyyy/MM/dd"));
        }

           
    }
    #region FUNCTION

    private void BindGrid(string SDATE, string EDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"
                        SELECT 
                        USER1.[NAME] AS 'NAME1',REPLACE([TB_EIP_SCH_WORK].[SUBJECT],char(10),'<br/>') AS   SUBJECT
                        ,(SELECT TOP 1 ISNULL([DESCRIPTION],'') FROM [UOF].[dbo].[TB_EIP_SCH_WORK_RECORD] WHERE [TB_EIP_SCH_WORK_RECORD].[WORK_GUID]=[TB_EIP_SCH_WORK].[WORK_GUID] ORDER BY CREATE_TIME DESC) AS 'DESCRIPTION'
                        ,CONVERT(NVARCHAR,[TB_EIP_SCH_WORK].[END_TIME],111)  AS END_TIME,DATEDIFF(day, [TB_EIP_SCH_WORK].[END_TIME],GETDATE()) AS DIFFDATES,USER2.[NAME]  AS 'NAME2'
                        ,CASE WHEN [TB_EIP_SCH_WORK].[WORK_STATE]='NotYetBegin' THEN '當未開始' ELSE '進行中' END AS 'STATUS'
                        ,[TB_EIP_SCH_WORK].[WORK_STATE],[TB_EIP_SCH_WORK].[EXECUTE_USER],[TB_EIP_SCH_WORK].[SOURCE_USER]
                        ,[TB_EIP_SCH_WORK].WORK_GUID
                        FROM [UOF].[dbo].[TB_EIP_SCH_WORK]
                        LEFT JOIN [UOF].[dbo].[TB_EB_USER] USER1 ON USER1.USER_GUID=[TB_EIP_SCH_WORK].[EXECUTE_USER]
                        LEFT JOIN [UOF].[dbo].[TB_EB_USER] USER2 ON USER2.USER_GUID=[TB_EIP_SCH_WORK].[SOURCE_USER]
                        WHERE [WORK_STATE] IN ('NotYetBegin','Proceeding','Audit')
                        AND USER1.[NAME] IN ('洪櫻芬','王琇平','葉枋俐','何姍怡','林琪琪','林杏育','張釋予','蔡顏鴻','陳帟靜','黃鈺涵')
                        AND CONVERT(NVARCHAR,[TB_EIP_SCH_WORK].[END_TIME],111) >=@SDATE AND CONVERT(NVARCHAR,[TB_EIP_SCH_WORK].[END_TIME],111) <=@EDATE
                        ORDER BY [EXECUTE_USER],[TB_EIP_SCH_WORK].[END_TIME],[SUBJECT]

                        ";

        //string cmdTxt = @"
        //                SELECT TOP 1 
        //                'NAME1'AS 'NAME1','SUBJECT' AS SUBJECT
        //                ,'DESCRIPTION' AS 'DESCRIPTION'
        //                ,''  AS END_TIME
        //                ,''  AS DIFFDATES
        //                ,'NAME2'  AS 'NAME2'
        //                ,'STATUS' AS 'STATUS'
        //                ,'1' 'WORK_STATE','' 'EXECUTE_USER','' 'SOURCE_USER'
        //                ,'' 'WORK_GUID'
        //                FROM [UOF].[dbo].TB_DMS_AGENCY_SUBSCRIBE

        //                ";

        m_db.AddParameter("@SDATE", SDATE);
        m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }



   
    protected void grid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       
    }



  
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       

    }

   

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        
    }

    private void BindGrid2(string SDATE, string EDATE)
    {

    }

    protected void grid2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {



    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }
    #endregion

    #region BUTTON
    protected void btn_Click(object sender, EventArgs e)
    {

    }
    protected void btn1_Click(object sender, EventArgs e)
    {
    }

    #endregion
}