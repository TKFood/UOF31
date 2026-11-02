using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;

public partial class CDS_WebPage_SIGN_COPTCCOPTD : Ede.Uof.Utility.Page.BasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid(DateTime.Now.ToString("yyyyMMdd"));
            txtDate1.Text = DateTime.Now.ToString("yyyyMMdd");

            BindGrid2(DateTime.Now.ToString("yyyy"));
            txtDate2.Text = DateTime.Now.ToString("yyyy");

        }
        else
        {
            //BindGrid(txtDate1.Text);

        }
    }

    #region FUNCTION
    private void BindGrid(string SDATE)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT MQ002,TC001,TC002,CONVERT(INT,TC029) TC029,CONVERT(INT,TC030) TC030,CONVERT(INT,(TC029+TC030)) AS MONEYS,TC053 AS TC053
                        , (     
                            SELECT CASE
                                        WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                        ELSE '<br />'
                                    END +'序號-'+CONVERT(NVARCHAR,TD003)+' '+CONVERT(NVARCHAR,TD005)+'-訂單數量'+CONVERT(NVARCHAR,CONVERT(int,TD008))+'-單價'+CONVERT(NVARCHAR,CONVERT(decimal(16,3),TD011))+'-贈品量'+CONVERT(NVARCHAR,CONVERT(INT,TD024)) AS 'data()'
                            FROM  [TK].dbo.COPTD WHERE TD001=TC001 AND TD002=TC002 
                            FOR XML PATH(''), TYPE  
                        ).value('.','nvarchar(max)')  As DETAILS 
                        FROM [TK].dbo.COPTC,[TK].dbo.CMSMQ
                        WHERE TC001=MQ001
                        AND TC027='N' 
                        AND TC003=@SDATE
                        ORDER BY TC001,TC002
                        ";

        m_db.AddParameter("@SDATE", SDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid1.PageIndex = e.NewPageIndex;
        BindGrid(this.Session["SDATE"].ToString());
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }

    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        
    }

    private void BindGrid2(string SYEARS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" 
                        SELECT MQ002,TE001,TE002,TE003,MA002
                        , (     
                            SELECT CASE
                                        WHEN ROW_NUMBER() OVER (ORDER BY (SELECT 0)) = 1 THEN ''
                                        ELSE '<br />'
                                    END +'序號-'+CONVERT(NVARCHAR,TF104)+'-'+CONVERT(NVARCHAR,TF006)+'-新訂單數量'+CONVERT(NVARCHAR,CONVERT(INT,TF009))+'-新單價'+CONVERT(NVARCHAR,TF013)+'-新贈品量'+CONVERT(NVARCHAR,CONVERT(INT,TF020)) AS 'data()'
                            FROM  [TK].dbo.COPTF WHERE TE001=TF001 AND TE002=TF002 AND TE003=TF003
                            FOR XML PATH(''), TYPE  
                        ).value('.','nvarchar(max)')  As DETAILS 
                        FROM [TK].dbo.COPTE,[TK].dbo.COPMA,[TK].dbo.CMSMQ
                        WHERE TE007=MA001
                        AND TE001=MQ001
                        AND TE005='N' AND TE029='N'
                        AND TE002 LIKE @SYEARS+'%'
                        ORDER BY TE001,TE002,TE003
                        ";

        m_db.AddParameter("@SYEARS", SYEARS);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt));

        Grid2.DataSource = dt;
        Grid2.DataBind();
    }

    protected void grid_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        Grid2.PageIndex = e.NewPageIndex;
        BindGrid2(this.Session["SYEARS"].ToString());
    }
    protected void Grid2_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DataRowView row = (DataRowView)e.Row.DataItem;
        //        LinkButton lbtnName = (LinkButton)e.Row.FindControl("lbtnName");

        //        ExpandoObject param = new { ID = row["ID"].ToString() }.ToExpando();

        //        //Grid開窗是用RowDataBound事件再開窗
        //        Dialog.Open2(lbtnName, "~/CDS/WebPage/TKRESEARCHTBDEVMEMODialogEDITDEL.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);
        //    }


    }

    public void OnBeforeExport2(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {

    }


    public void UPDATECOPTCCOPTD(string SQL)
    {
        string TC027 = "Y";
        string TC048 = "N";
        string TD021 = "Y";
        string COMPANY = "TK";
        string MODIFIER = "160115";
        string MODI_DATE = DateTime.Now.ToString("yyyyMMdd");
        string MODI_TIME = DateTime.Now.ToString("HH:mm:dd");

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = SQL;

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {

                    cmd.Parameters.AddWithValue("@TC027", TC027);
                    cmd.Parameters.AddWithValue("@TC048", TC048);
                    cmd.Parameters.AddWithValue("@TD021", TD021);

                    cmd.Parameters.AddWithValue("@COMPANY", COMPANY);
                    cmd.Parameters.AddWithValue("@MODIFIER", MODIFIER);
                    cmd.Parameters.AddWithValue("@MODI_DATE", MODI_DATE);
                    cmd.Parameters.AddWithValue("@MODI_TIME", MODI_TIME);

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected>=1)
                    {
                        Label3.Text = "完成";
                    }
                }
            }
        }

        catch
        {

        }
        finally
        {

        }
        
    }

    public void UPDATECOPTCCOPTD2(string SQL)
    {
        string TE029 = "Y";
        string TE044 = "N";
        string TF019 = "Y";
        string COMPANY = "TK";
        string MODIFIER = "160115";
        string MODI_DATE = DateTime.Now.ToString("yyyyMMdd");
        string MODI_TIME = DateTime.Now.ToString("HH:mm:dd");

        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();

        var SQLCOMMAND = SQL;

        try
        {
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLCOMMAND, cnn))
                {

                    cmd.Parameters.AddWithValue("@TE029", TE029);
                    cmd.Parameters.AddWithValue("@TE044", TE044);
                    cmd.Parameters.AddWithValue("@TF019", TF019);

                    cmd.Parameters.AddWithValue("@COMPANY", COMPANY);
                    cmd.Parameters.AddWithValue("@MODIFIER", MODIFIER);
                    cmd.Parameters.AddWithValue("@MODI_DATE", MODI_DATE);
                    cmd.Parameters.AddWithValue("@MODI_TIME", MODI_TIME);

                    cnn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected >= 1)
                    {
                        Label5.Text = "完成";
                        Label7.Text = "";
                    }
                }
            }
        }

        catch
        {

        }
        finally
        {

        }

    }

    public string SETSQL(string TC001TC002)
    {
        StringBuilder SQL = new StringBuilder();

        SQL.AppendFormat(@"
                        UPDATE [TK].dbo.COPTC
                        SET TC027=@TC027,TC048=@TC048, FLAG=FLAG+1,COMPANY=@COMPANY,MODIFIER=@MODIFIER ,MODI_DATE=@MODI_DATE, MODI_TIME=@MODI_TIME 
                        WHERE TC001+TC002 IN ({0})

                        UPDATE [TK].dbo.COPTD 
                        SET TD021=@TD021, FLAG=FLAG+1,COMPANY=@COMPANY,MODIFIER=@MODIFIER ,MODI_DATE=@MODI_DATE, MODI_TIME=@MODI_TIME 
                        WHERE TD001+TD002 IN ({0})
                        ", TC001TC002);

        return SQL.ToString();
    }

    public string SETSQL2(string TE001TE002TE003, string TE001TE002)
    {
        StringBuilder SQL = new StringBuilder();

        SQL.AppendFormat(@"
                        --INSERT COPTD
                        INSERT INTO [TK].dbo.COPTD
                        (
                        COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG,CREATE_TIME,MODI_TIME,TRANS_TYPE,TRANS_NAME,DataGroup
                        ,TD001,TD002,TD003,TD004,TD005,TD006,TD007,TD008,TD010
                        ,TD011,TD012,TD013,TD014,TD015,TD016,TD020
                        ,TD021,TD023,TD024,TD026,TD027,TD028,TD029,TD030
                        ,TD031,TD032,TD034,TD036,TD049,TD050,TD052
                        ,TD088,TD089,TD090,TD091,TD092,TD093,TD094,TD095,TD096 ,TD097
                        ) 

                        SELECT COMPANY ,CREATOR ,USR_GROUP ,CREATE_DATE ,FLAG ,CREATE_TIME, MODI_TIME, TRANS_TYPE, TRANS_NAME,DataGroup
                        ,TF001 TD001,TF002 TD002,TF104 TD003,TF005 TD004,TF006 TD005,TF007 TD006,TF008 TD007,TF009 TD008,TF010 TD010
                        ,TF013 TD011,TF014 TD012,TF015 TD013,TF016 TD014,TF063 TD015,TF017 TD016,TF032 TD020
                        ,'Y' TD021,TF012 TD023,TF020 TD026,TF021 TD027,TF022 TD028,TF064 TD029,TF023 TD029,TF024 TD030
                        ,TF024 TD031,TF026 TD032,TF027 TD034,TF028 TD036,TF044 TD049,TF045 TD050,TF046 TD052
                        ,TF183 TD088,TF184 TD089,TF185 TD090,TF186 TD091,TF187 TD092,TF188 TD093,TF189 TD094,TF194 TD095,TF195 TD096,TF137 TD097
                        FROM [TK].dbo.COPTF
                        WHERE TF001+TF002+TF104 NOT IN (SELECT TD001+TD002+TD003 FROM [TK].dbo.COPTD  WHERE TD001+TD002 IN ({1}))
                        AND TF001+TF002+TF003 IN ({0})

                        --更新COPTC
                        UPDATE [TK].dbo.COPTC
                        SET TC004=TE007,TC005=TE008,TC006=TE009,TC007=TE010,TC008=TE011
                        ,TC009=TE012,TC010=TE013,TC011=TE014,TC012=TE015,TC013=TE016
                        ,TC014=TE017,TC015=TE050,TC016=TE018,TC017=TE019,TC018=TE020
                        ,TC019=TE021,TC020=TE022,TC021=TE023,TC022=TE024,TC023=TE025
                        ,TC024=TE026,TC025=TE027,TC026=TE028,TC032=TE031,TC033=TE032
                        ,TC034=TE033,TC035=TE034,TC036=TE035,TC037=TE036,TC038=TE037
                        ,TC041=TE040,TC042=TE041,TC045=TE042,TC047=TE043,TC053=TE055
                        ,TC054=TE047,TC055=TE048,TC056=TE049,TC057=TE183,TC058=TE184
                        ,TC063=TE056,TC064=TE057,TC065=TE058,TC066=TE059,TC070=TE079 
                        ,TC074=TE085,TC079=TE070,TC094=TE081,TC095=TE082,TC099=TE185
                        ,TC104=TE084,TC105=TE086,TC113=TE087,TC114=TE088,TC115=TE196
                        ,TC116=TE197
                        ,FLAG=COPTC.FLAG+1
                        FROM [TK].dbo.COPTE
                        WHERE TC001=TE001 AND TC002=TE002
                        AND TE001+TE002+TE003 IN ({0})

                        --更新COPTD
                        UPDATE [TK].dbo.COPTD
                        SET TD004=TF005,TD005=TF006,TD006=TF007,TD007=TF008,TD008=TF009
                        ,TD010=TF010,TD011=TF013,TD012=TF014,TD013=TF015,TD014=TF016
                        ,TD015=TF063,TD016=TF017,TD020=TF032,TD023=TF012,TD024=TF020
                        ,TD026=TF021,TD027=TF022,TD028=TF064,TD029=TF023,TD030=TF024
                        ,TD031=TF025,TD032=TF026,TD034=TF027,TD036=TF028,TD049=TF044
                        ,TD050=TF045,TD052=TF046,TD088=TF183,TD089=TF184,TD091=TF186
                        ,TD092=TF187,TD093=TF188,TD094=TF189,TD095=TF194,TD096=TF195 
                        ,TD097=TF137,TD090=TF185 
                        ,FLAG=COPTD.FLAG+1
                        FROM [TK].dbo.COPTF
                        WHERE TD001+TD002+TD003=TF001+TF002+TF104
                        AND TF001+TF002+TF003 IN ({0})

                        UPDATE [TK].dbo.COPTE
                        SET TE029=@TE029,TE044=@TE044, FLAG=FLAG+1,COMPANY=@COMPANY,MODIFIER=@MODIFIER ,MODI_DATE=@MODI_DATE, MODI_TIME=@MODI_TIME 
                        WHERE TE001+TE002+TE003 IN ({0})

                        UPDATE [TK].dbo.COPTF 
                        SET TF019=@TF019, FLAG=FLAG+1,COMPANY=@COMPANY,MODIFIER=@MODIFIER ,MODI_DATE=@MODI_DATE, MODI_TIME=@MODI_TIME 
                        WHERE TF001+TF002+TF003 IN ({0})

                        --更新COPTC的未稅、稅額、總金額
                        UPDATE [TK].dbo.COPTC
                        SET TC029=(CASE WHEN TC016='1' THEN (SELECT ISNULL(ROUND(SUM(TD012)/(1+TC041),0),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        ELSE CASE WHEN TC016='2' THEN (SELECT ISNULL(SUM(TD012),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        ELSE CASE WHEN TC016='3' THEN (SELECT ISNULL(SUM(TD012),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        ELSE CASE WHEN TC016='4' THEN (SELECT ISNULL(SUM(TD012),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        ELSE CASE WHEN TC016='9' THEN (SELECT ISNULL(SUM(TD012),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        END
                        END
                        END 
                        END
                        END)
                        ,TC030=(CASE WHEN TC016='1' THEN (SELECT (ISNULL(SUM(TD012),0)-ISNULL(ROUND(SUM(TD012)/(1+TC041),0),0)) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        ELSE CASE WHEN TC016='2' THEN (SELECT ISNULL(ROUND(SUM(TD012)*TC041,0),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        ELSE CASE WHEN TC016='3' THEN 0 
                        ELSE CASE WHEN TC016='4' THEN 0
                        ELSE CASE WHEN TC016='9' THEN 0 
                        END
                        END
                        END 
                        END
                        END)
                        ,TC031=(CASE WHEN TC016='1' THEN (SELECT ISNULL(SUM(TD012),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        ELSE CASE WHEN TC016='2' THEN (SELECT (ISNULL(SUM(TD012),0)+ISNULL(ROUND(SUM(TD012)*TC041,0),0)) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        ELSE CASE WHEN TC016='3' THEN (SELECT ISNULL(SUM(TD012),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        ELSE CASE WHEN TC016='4' THEN (SELECT ISNULL(SUM(TD012),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002) 
                        ELSE CASE WHEN TC016='9' THEN (SELECT ISNULL(SUM(TD012),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002)  
                        END
                        END
                        END 
                        END
                        END)
                        WHERE TC001+TC002 IN ({1})

                        --更新COPTC總數量總數量、毛重(Kg)、材積(CUFT)
                        UPDATE [TK].dbo.COPTC
                        SET TC031=(SELECT ISNULL(SUM(TD008+TD024),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002)
                        ,TC043=(SELECT ISNULL(SUM(TD030),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002)
                        ,TC044=(SELECT ISNULL(SUM(TD031),0) FROM [TK].dbo.COPTD WHERE TD001+TD002=TC001+TC002)
                        WHERE TC001+TC002 IN ({1})

                        UPDATE [TK].dbo.COPTD
                        SET TD016='y'
                        FROM [TK].dbo.COPTE
                        WHERE TD001+TD002=TE001+TE002
                        AND TE005='Y'
                        AND TE001+TE002 IN ({1})

                       
                      
                        ", TE001TE002TE003, TE001TE002);

        return SQL.ToString();
    }

    #endregion


    #region BUTTON
    protected void btn1_Click(object sender, EventArgs e)
    {
        BindGrid(txtDate1.Text);
    }

    protected void btn2_Click(object sender, EventArgs e)
    {
        string TC001TC002 = "";
        Grid1.EditIndex = -1;

        foreach (GridViewRow gvr in this.Grid1.Rows)
        {
            Control ctl = gvr.FindControl("CheckBox");
            CheckBox ck = (CheckBox)ctl;
            if (ck.Checked)
            {
                TableCellCollection cell = gvr.Cells;
                TC001TC002 += "'"+cell[3].Text+ cell[4].Text + "',";
            }
        }
        TC001TC002 += "''";
        Label3.Text = TC001TC002.ToString();

        string SQL = SETSQL(Label3.Text);

        UPDATECOPTCCOPTD(SQL);

        BindGrid(txtDate1.Text);
    }

    protected void btn3_Click(object sender, EventArgs e)
    {
        string TE001TE002 = "";
        string TE001TE002TE003 = "";
        Grid2.EditIndex = -1;

        foreach (GridViewRow gvr in this.Grid2.Rows)
        {
            Control ctl = gvr.FindControl("CheckBox");
            CheckBox ck = (CheckBox)ctl;
            if (ck.Checked)
            {
                TableCellCollection cell = gvr.Cells;
                TE001TE002 += "'" + cell[2].Text + cell[3].Text+ "',";
            }
        }

        foreach (GridViewRow gvr in this.Grid2.Rows)
        {
            Control ctl = gvr.FindControl("CheckBox");
            CheckBox ck = (CheckBox)ctl;
            if (ck.Checked)
            {
                TableCellCollection cell = gvr.Cells;
                TE001TE002TE003 += "'" + cell[2].Text + cell[3].Text + cell[4].Text + "',";
            }
        }

        TE001TE002 += "''";
        TE001TE002TE003 += "''";

        Label5.Text = TE001TE002.ToString();
        Label7.Text = TE001TE002TE003.ToString();

        string SQL = SETSQL2(Label7.Text,Label5.Text);

        UPDATECOPTCCOPTD2(SQL);

        BindGrid2(txtDate2.Text);
    }

    protected void btn4_Click(object sender, EventArgs e)
    {
        BindGrid2(txtDate2.Text);
    }
    #endregion
}