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
using System.Xml;
using System.Xml.Linq;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Log;
using Ede.Uof.Utility.Page.Common;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

public partial class CDS_WebPage_COP_TBBU_TBCOPTDCHECK : Ede.Uof.Utility.Page.BasePage
{
    string DBNAME = "UOF";
    //string DBNAME = "UOFTEST";

    string TC001 = "";
    string TC002 = "";

    protected void Page_Load(object sender, EventArgs e)
    {       

        if (!IsPostBack)
        {
            SETDATES();
            BindDropDownList();
            BindDropDownList2();
            BindGrid("");
        }
        else
        {

        }




    }
    #region FUNCTION
    public void SETDATES()
    {
        TextBox1.Text = DateTime.Now.ToString("yyyy");
        TextBox2.Text = DateTime.Now.ToString("MM");
    }
    private void BindDropDownList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT '未核單' AS 'STATUS' UNION ALL SELECT '已核單' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "STATUS";
            DropDownList1.DataValueField = "STATUS";
            DropDownList1.DataBind();

        }
        else
        {

        }



    }

    private void BindDropDownList2()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STATUS", typeof(String));


        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @" SELECT 'Y' AS 'STATUS' UNION ALL SELECT 'N' AS 'STATUS' ";

        dt.Load(m_db.ExecuteReader(cmdTxt));

        if (dt.Rows.Count > 0)
        {
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "STATUS";
            DropDownList2.DataValueField = "STATUS";
            DropDownList2.DataBind();

        }
        else
        {

        }



    }


    private void BindGrid(string SALESFOCUS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        //日期
        if (!string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox2.Text))
        {
            if (TextBox2.Text.Length == 1)
            {
                TextBox2.Text = "0" + TextBox2.Text;
            }
            QUERYS.AppendFormat(@" AND TD002 LIKE '{0}%'", TextBox1.Text.Trim() + TextBox2.Text.Trim());

        }

        //核單
        if (!string.IsNullOrEmpty(DropDownList1.Text))
        {
            if (DropDownList1.Text.Equals("未核單"))
            {
                QUERYS.AppendFormat(@" AND TD021='N'");
            }
            else if (DropDownList1.Text.Equals("已核單"))
            {
                QUERYS.AppendFormat(@"  AND TD021='Y'");
            }
        }


        //是否生產
        if (!string.IsNullOrEmpty(DropDownList2.Text))
        {
            if (DropDownList2.Text.Equals("Y"))
            {
                QUERYS.AppendFormat(@" AND COPTD.UDF01 IN ('Y','y') ");
            }
            else if (DropDownList2.Text.Equals("N"))
            {
                QUERYS.AppendFormat(@" AND COPTD.UDF01 NOT IN ('Y','y')  ");
            }
        }


        cmdTxt.AppendFormat(@" 
                                SELECT  
                                LTRIM(RTRIM(TD001))+LTRIM(RTRIM(TD002))+LTRIM(RTRIM(TD003)) AS 'TD123'
                                ,LTRIM(RTRIM(TD001))+LTRIM(RTRIM(TD002)) AS 'TD12'
                                ,*
                                ,(SELECT TOP 1 ISNULL([MOCCHECKDATES],'') FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 [MOCCHECKS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 [MOCCHECKSCOMMENTS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 [PURCHECKDATES] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 [PURCHECKS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 [PURCHECKSCOMMENTS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 [SALESCHECKDATES] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 [SALESCHECKSCOMMENTS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'

                                FROM [TK].dbo.COPTC,[TK].dbo.COPTD
                                WHERE TC001=TD001 AND TC002=TD002
                                AND 1=1
                                
                                {0}

                                ORDER BY TD001,TD002,TD003

                                ", QUERYS.ToString());




        //m_db.AddParameter("@SDATE", SDATE);
        //m_db.AddParameter("@EDATE", EDATE);

        DataTable dt = new DataTable();

        dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));      

        

        Grid1.DataSource = dt;
        Grid1.DataBind();
    }

    protected void grid_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        //Grid1.PageIndex = e.NewPageIndex;
        //BindGrid();
    }
    protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ///Get the button that raised the event
            Button btn = (Button)e.Row.FindControl("Button1");
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue = btn.CommandArgument;
            DataRowView row = (DataRowView)e.Row.DataItem;
            Button lbtnName = (Button)e.Row.FindControl("Button1");
            ExpandoObject param = new { ID = Cellvalue }.ToExpando();
            //Grid開窗是用RowDataBound事件再開窗
            // Dialog.PostBackType.AfterReturn
            //Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBCOPTDCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.AfterReturn, param);

            // Dialog.PostBackType.Allows
            Dialog.Open2(lbtnName, "~/CDS/WebPage/COP/TBBU_TBCOPTDCHECKDialogEDIT.aspx", "", 800, 600, Dialog.PostBackType.Allows, param);


            //Button2
            //Get the button that raised the event
            Button btn2 = (Button)e.Row.FindControl("Button2");
            //Get the row that contains this button
            GridViewRow gvr2 = (GridViewRow)btn2.NamingContainer;
            //string cellvalue = gvr.Cells[2].Text.Trim();
            string Cellvalue2 = btn2.CommandArgument;
            DataRowView row2 = (DataRowView)e.Row.DataItem;
            Button lbtnName2 = (Button)e.Row.FindControl("Button2");
            ExpandoObject param2 = new { ID = Cellvalue }.ToExpando();
            


        }





        //StringBuilder PATH = new StringBuilder();

        //System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView row = (DataRowView)e.Row.DataItem;
        //    System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");



        //    if (!string.IsNullOrEmpty(row["PHOTO_GUID"].ToString()))
        //    {
        //        //img.ImageUrl = "https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id=8b2a033b-c301-419b-938d-e6cfedf28b82&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name=40100010650490.png";


        //        //PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/common/filecenter/v3/handler/downloadhandler.ashx?id={0}&path=ALBUM%5C2021%5C03&contentType=image%2Fpng&name={1}
        //        //                ", row["RESIZE_FILE_ID"].ToString(), row["PHOTO_DESC"].ToString());

        //        PATH.AppendFormat(@"https://eip.tkfood.com.tw/UOF/Common/FileCenter/V3/Handler/FileControlHandler.ashx?id={0}
        //                        ", row["RESIZE_FILE_ID"].ToString());
        //        img.ImageUrl = PATH.ToString();

        //        //img.ImageUrl  = Request.ApplicationPath + "/Common/FileCenter/ShowImage.aspx?id=" + row["THUMBNAIL_FILE_ID"].ToString();

        //        //img.ImageUrl = string.Format("~/Common/FileCenter/Downloadfile.ashx?id={0}", row["THUMBNAIL_FILE_ID"].ToString());

        //        //e.Row.Cells[0].Text = row["THUMBNAIL_FILE_ID"].ToString();
        //        ////獲取當前行的圖片路徑
        //        //string ImgUrl = img.ImageUrl;
        //        ////給帶圖片的單元格添加點擊事件
        //        //e.Row.Cells[3].Attributes.Add("onclick", e.Row.Cells[3].ClientID.ToString()
        //        //    + ".checked=true;CellClick('" + ImgUrl + "')");

        //        //  img.ImageUrl = "https://eip.tkfood.com.tw/BM/upload/note/20200926112527.jpg";
        //    }


        //}


    }

    protected void Grid1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = -1;

        if (e.CommandName == "Button1")
        {
            //MsgBox("Button1", this.Page, this);
            BindGrid("");
        }
        else if (e.CommandName == "Button2")
        {
            CHECKTBCOPTDCHECK(e.CommandArgument.ToString());
            //MsgBox(e.CommandArgument.ToString(), this.Page, this);           
        }

    }


    public void OnBeforeExport1(object sender, Ede.Uof.Utility.Component.BeforeExportEventArgs e)
    {
        SETEXCEL();

        //string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);


        //string cmdTxt = @" 
        //               SELECT [PRODUCTS].[MB001],[PRODUCTSFEATURES],[SALESFOCUS],[COPYWRITINGS],[PICPATHS]
        //                ,MB002,MB003,MB004,MA003,ISNULL(MD007,0) AS MD007,CONVERT(NVARCHAR,MB023)+(CASE WHEN MB198='1' THEN '天' ELSE (CASE WHEN MB198='2' THEN '月' ELSE '年' END ) END ) AS 'VALIDITYPERIOD',CONVERT(decimal(16,3),ISNULL(MB047,0)) AS MB047,MB013
        //                ,[ALBUM_GUID], [PHOTO_GUID],[PHOTO_DESC],[FILE_ID],[RESIZE_FILE_ID],[THUMBNAIL_FILE_ID]
        //                FROM [TKBUSINESS].[dbo].[PRODUCTS]
        //                LEFT JOIN [TK].dbo.[INVMB] ON [PRODUCTS].[MB001]=[INVMB].[MB001]
        //                LEFT JOIN [TK].dbo.INVMA ON MA001='9' AND MA002=MB115
        //                LEFT JOIN [TK].dbo.BOMMD ON MD001=[INVMB].[MB001] AND MD003 LIKE '201%'
        //                LEFT JOIN [192.168.1.223].[UOF].[dbo].[TB_EIP_ALBUM_PHOTO] ON [PHOTO_TOPIC]=[PRODUCTS].[MB001] COLLATE Chinese_Taiwan_Stroke_BIN
        //                ORDER BY [PRODUCTS].[MB001]
        //                ";



        //DataTable dt = new DataTable();

        //dt.Load(m_db.ExecuteReader(cmdTxt));

        //if (dt.Rows.Count > 0)
        //{
        //    dt.Columns[0].Caption = "ID";


        //    e.Datasource = dt;
        //}
    }

    //private void AddImage(ExcelWorksheet oSheet, int rowIndex, int colIndex, string imagePath)
    //{
    //    Bitmap image = new Bitmap(imagePath);
    //    ExcelPicture excelImage = null;
    //    if (image != null)
    //    {
    //        excelImage = oSheet.Drawings.AddPicture("Debopam Pal", image);
    //        excelImage.From.Column = colIndex;
    //        excelImage.From.Row = rowIndex;
    //        excelImage.SetSize(100, 100);
    //        //2x2 px space for better alignment
    //        excelImage.From.ColumnOff = Pixel2MTU(2);
    //        excelImage.From.RowOff = Pixel2MTU(2);
    //    }
    //}

    //public int Pixel2MTU(int pixels)
    //{
    //    int mtus = pixels * 9525;
    //    return mtus;
    //}

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

    public void CHECKTBCOPTDCHECK(string TD001TD002)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();

        try
        {
            cmdTxt.AppendFormat(@"                              
                                SELECT *
                                FROM 
                                (
                                SELECT  LTRIM(RTRIM(TD001))+LTRIM(RTRIM(TD002))+LTRIM(RTRIM(TD003)) AS 'TD123',TD001,TD002,TD003
                                ,(SELECT TOP 1 ISNULL([MOCCHECKDATES],'') FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'MOCCHECKDATES'
                                ,(SELECT TOP 1 [MOCCHECKS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'MOCCHECKS'
                                ,(SELECT TOP 1 [MOCCHECKSCOMMENTS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'MOCCHECKSCOMMENTS'
                                ,(SELECT TOP 1 [PURCHECKDATES] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'PURCHECKDATES'
                                ,(SELECT TOP 1 [PURCHECKS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'PURCHECKS'
                                ,(SELECT TOP 1 [PURCHECKSCOMMENTS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'PURCHECKSCOMMENTS'
                                ,(SELECT TOP 1 [SALESCHECKDATES] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'SALESCHECKDATES'
                                ,(SELECT TOP 1 [SALESCHECKSCOMMENTS] FROM [TKBUSINESS].[dbo].[TBCOPTDCHECK] WHERE [TBCOPTDCHECK].TD001=COPTD.TD001 AND [TBCOPTDCHECK].TD002=COPTD.TD002 AND [TBCOPTDCHECK].TD003=COPTD.TD003  ORDER BY ID DESC) AS 'SALESCHECKSCOMMENTS'

                                FROM [TK].dbo.COPTC,[TK].dbo.COPTD
                                WHERE TC001=TD001 AND TC002=TD002
                                AND 1=1
                                ) 
                                AS TEMP
                                WHERE [MOCCHECKS]='Y' AND [PURCHECKS]='Y'
                                AND LTRIM(RTRIM(TD001))+LTRIM(RTRIM(TD002))='{0}'

                                ", TD001TD002);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt.Rows.Count>0)
            {
                string TD001 = dt.Rows[0]["TD001"].ToString().Trim();
                string TD002 = dt.Rows[0]["TD002"].ToString().Trim();

                TC001 = TD001;
                TC002 = TD002;

                ADDTB_WKF_EXTERNAL_TASK_COPTCCOPTD(TD001, TD002);

                //MsgBox("OK "+ TD001+ TD002, this.Page, this);
            }
            else
            {
                MsgBox("NG ", this.Page, this);
            }
        }
        catch
        {
            MsgBox("catch NG", this.Page, this);
        }
        finally
        {

        }

       

    }

    public void ADDTB_WKF_EXTERNAL_TASK_COPTCCOPTD(string TC001, string TC002)
    {

        DataTable DT = SEARCHCOPTCCOPTD(TC001, TC002);
        DataTable DTUPFDEP = SEARCHUOFDEP(DT.Rows[0]["TC006"].ToString());

        string account = DT.Rows[0]["TC006"].ToString();
        string groupId = DT.Rows[0]["GROUP_ID"].ToString();
        string jobTitleId = DT.Rows[0]["TITLE_ID"].ToString();
        string fillerName = DT.Rows[0]["NAME"].ToString();
        string fillerUserGuid = DT.Rows[0]["USER_GUID"].ToString();

        string BA = DT.Rows[0]["BA"].ToString();
        string BANAME = DT.Rows[0]["BANAME"].ToString();
        string BA_USER_GUID = DT.Rows[0]["BA_USER_GUID"].ToString();

        string DEPNAME = DTUPFDEP.Rows[0]["DEPNAME"].ToString();
        string DEPNO = DTUPFDEP.Rows[0]["DEPNO"].ToString();

        string EXTERNAL_FORM_NBR = DT.Rows[0]["TC001"].ToString().Trim() + DT.Rows[0]["TC002"].ToString().Trim();

        int rowscounts = 0;

        string COPTCUDF01 = "N";

        foreach (DataRow od in DT.Rows)
        {
            if (od["COPTDUDF01"].ToString().Equals("Y"))
            {
                COPTCUDF01 = "Y";
                break;
            }
            else
            {
                COPTCUDF01 = "N";
            }
        }

        XmlDocument xmlDoc = new XmlDocument();
        //建立根節點
        XmlElement Form = xmlDoc.CreateElement("Form");

        //正式的id
        string  COPID = SEARCHFORM_VERSION_ID("訂單");
        //string COPID = "24c10c88-32ff-4db1-8900-abf7e4f61471";

        if (!string.IsNullOrEmpty(COPID))
        {
            Form.SetAttribute("formVersionId", COPID);
        }


        Form.SetAttribute("urgentLevel", "2");
        //加入節點底下
        xmlDoc.AppendChild(Form);

        ////建立節點Applicant
        XmlElement Applicant = xmlDoc.CreateElement("Applicant");
        Applicant.SetAttribute("account", account);
        Applicant.SetAttribute("groupId", groupId);
        Applicant.SetAttribute("jobTitleId", jobTitleId);
        //加入節點底下
        Form.AppendChild(Applicant);

        //建立節點 Comment
        XmlElement Comment = xmlDoc.CreateElement("Comment");
        Comment.InnerText = "申請者意見";
        //加入至節點底下
        Applicant.AppendChild(Comment);

        //建立節點 FormFieldValue
        XmlElement FormFieldValue = xmlDoc.CreateElement("FormFieldValue");
        //加入至節點底下
        Form.AppendChild(FormFieldValue);

        //建立節點FieldItem
        //ID 表單編號	
        XmlElement FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "ID");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);



        //建立節點FieldItem
        //TC001 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC001");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC001"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC002 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC002"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //COPTCUDF01 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "COPTCUDF01");
        FieldItem.SetAttribute("fieldValue", COPTCUDF01);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC003 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC003");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC003"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC004 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC004");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC004"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC053 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC053");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC053"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立userset
        var xElement = new XElement(
              new XElement("UserSet",
                  new XElement("Element", new XAttribute("type", "user"),
                      new XElement("userId", fillerUserGuid)
                      )
                      )
                    );



        //XmlDocument doc = new XmlDocument();
        //XmlElement UserSet = doc.CreateElement("UserSet");

        //XmlElement Element = doc.CreateElement("Element");
        //Element.SetAttribute("type", "user");//設定屬性
        //UserSet.AppendChild(Element);

        //XmlElement userId = doc.CreateElement("userId", fillerUserGuid);
        //Element.AppendChild(userId);

        //建立節點FieldItem
        //TC006 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC006");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["NAME"].ToString() + "(" + DT.Rows[0]["TC006"].ToString() + ")");
        FieldItem.SetAttribute("realValue", xElement.ToString());
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //MV002 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "MV002");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["NAME"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立userset
        var xElement_BA = new XElement(
              new XElement("UserSet",
                  new XElement("Element", new XAttribute("type", "user"),
                      new XElement("userId", BA_USER_GUID)
                      )
                      )
                    );

        //建立節點FieldItem
        //BA 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "BA");
        FieldItem.SetAttribute("fieldValue", BANAME + "(" + BA + ")");
        FieldItem.SetAttribute("realValue", xElement_BA.ToString());
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //BANAME 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "BANAME");
        FieldItem.SetAttribute("fieldValue", BANAME);
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC015 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC015");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC015"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC008 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC008");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC008"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC009 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC009");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC009"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC045 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC045");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC045"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC029 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC029");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC029"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC030 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC030");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC030"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC041 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC041");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC041"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC016 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC016");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["NEWTC016"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC124 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC124");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC124"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC031 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC031");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC031"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC043 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC043");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC043"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC044 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC044");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC044"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC046 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC046");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC046"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC018 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC018");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC018"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC010 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC010");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC010"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC012 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC012");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC012"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC035 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC035");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC035"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC054 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC054");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC054"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC055 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC055");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC055"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC065 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC065");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC065"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC042 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC042");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC042"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC014 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC014");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC014"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC019 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC019");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC019"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC032 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC032");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC032"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC033 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC033");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC033"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC039 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC039");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC039"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC121 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC121");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["NEWTC016"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC094 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC094");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC094"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC063 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC063");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC063"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC115 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC115");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC115"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //TC116 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "TC116");
        FieldItem.SetAttribute("fieldValue", DT.Rows[0]["TC116"].ToString());
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //MOC 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "MOC");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點FieldItem
        //PUR 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "PUR");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);



        //建立節點FieldItem
        //DETAILS 表單編號	
        FieldItem = xmlDoc.CreateElement("FieldItem");
        FieldItem.SetAttribute("fieldId", "DETAILS");
        FieldItem.SetAttribute("fieldValue", "");
        FieldItem.SetAttribute("realValue", "");
        FieldItem.SetAttribute("enableSearch", "True");
        FieldItem.SetAttribute("fillerName", fillerName);
        FieldItem.SetAttribute("fillerUserGuid", fillerUserGuid);
        FieldItem.SetAttribute("fillerAccount", account);
        FieldItem.SetAttribute("fillSiteId", "");
        //加入至members節點底下
        FormFieldValue.AppendChild(FieldItem);

        //建立節點 DataGrid
        XmlElement DataGrid = xmlDoc.CreateElement("DataGrid");
        //DataGrid 加入至 TB 節點底下
        XmlNode DETAILS = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='DETAILS']");
        DETAILS.AppendChild(DataGrid);



        foreach (DataRow od in DT.Rows)
        {
            // 新增 Row
            XmlElement Row = xmlDoc.CreateElement("Row");
            Row.SetAttribute("order", (rowscounts).ToString());

            //Row	UDF01
            XmlElement Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "UDF01");
            Cell.SetAttribute("fieldValue", od["COPTDUDF01"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD003
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD003");
            Cell.SetAttribute("fieldValue", od["TD003"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD004
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD004");
            Cell.SetAttribute("fieldValue", od["TD004"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD005
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD005");
            Cell.SetAttribute("fieldValue", od["TD005"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD006
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD006");
            Cell.SetAttribute("fieldValue", od["TD006"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD008
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD008");
            Cell.SetAttribute("fieldValue", od["TD008"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD024
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD024");
            Cell.SetAttribute("fieldValue", od["TD024"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD009
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD009");
            Cell.SetAttribute("fieldValue", od["TD009"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD025
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD025");
            Cell.SetAttribute("fieldValue", od["TD025"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);


            //Row	TD010
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD010");
            Cell.SetAttribute("fieldValue", od["TD010"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD011
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD011");
            Cell.SetAttribute("fieldValue", od["TD011"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD026
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD026");
            Cell.SetAttribute("fieldValue", od["TD026"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD012
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD012");
            Cell.SetAttribute("fieldValue", od["TD012"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);


            //Row	TD013
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD013");
            Cell.SetAttribute("fieldValue", od["TD013"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD017
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD017");
            Cell.SetAttribute("fieldValue", od["TD017"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD018
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD018");
            Cell.SetAttribute("fieldValue", od["TD018"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD019
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD019");
            Cell.SetAttribute("fieldValue", od["TD019"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);

            //Row	TD020
            Cell = xmlDoc.CreateElement("Cell");
            Cell.SetAttribute("fieldId", "TD020");
            Cell.SetAttribute("fieldValue", od["TD020"].ToString());
            Cell.SetAttribute("realValue", "");
            Cell.SetAttribute("customValue", "");
            Cell.SetAttribute("enableSearch", "True");
            //Row
            Row.AppendChild(Cell);


            rowscounts = rowscounts + 1;

            XmlNode DataGridS = xmlDoc.SelectSingleNode("./Form/FormFieldValue/FieldItem[@fieldId='DETAILS']/DataGrid");
            DataGridS.AppendChild(Row);

        }

        //用ADDTACK，直接啟動起單
        ADDTACK(Form);

        ////ADD TO DB
        //string connectionString = ConfigurationManager.ConnectionStrings["connectionstringUOF"].ToString();
        //Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);
        
        //StringBuilder queryString = new StringBuilder();


        //try
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {

        //        SqlCommand command = new SqlCommand(queryString.ToString(), connection);
        //        command.Parameters.Add("@XML", SqlDbType.NVarChar).Value = Form.OuterXml;

        //        command.Connection.Open();

        //        int count = command.ExecuteNonQuery();

        //        connection.Close();
        //        connection.Dispose();

        //    }
        //}
        //catch
        //{

        //}
        //finally
        //{

        //}
    }

    public void ADDTACK(XmlElement Form)
    {
        Ede.Uof.WKF.Utility.TaskUtilityUCO taskUCO = new Ede.Uof.WKF.Utility.TaskUtilityUCO();

        string result = taskUCO.WebService_CreateTask(Form.OuterXml);

        XElement resultXE = XElement.Parse(result);

        string status = "";
        string formNBR = "";
        string error = "";
       
        string NEWTASK_ID = "";

        if (resultXE.Element("Status").Value == "1")
        {
            status = "起單成功!";
            formNBR = resultXE.Element("FormNumber").Value;           

            NEWTASK_ID = formNBR;

            Logger.Write("TEST", status + formNBR);
            MsgBox("起單成功 "  +TC001 + TC002 + " > " + formNBR , this.Page, this);

        }
        else
        {
            status = "起單失敗!";
            error = resultXE.Element("Exception").Element("Message").Value;

            Logger.Write("TEST", status + error + "\r\n" + Form.OuterXml);

            MsgBox("起單失敗 " + error + "\r\n" + Form.OuterXml, this.Page, this);

            throw new Exception(status + error + "\r\n" + Form.OuterXml);

        }
    }

   

    public DataTable SEARCHCOPTCCOPTD(string TC001, string TC002)
    {
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();
      

        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            StringBuilder QUERYS = new StringBuilder();

          

            cmdTxt.AppendFormat(@" 
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
                                ,CASE WHEN TC016='1' THEN '1.應稅內含'  ELSE (CASE WHEN TC016='2' THEN '2.應稅外加'  ELSE (CASE WHEN TC016='3' THEN '3.零稅率'  ELSE (CASE WHEN TC016='4' THEN '4.免稅'  ELSE (CASE WHEN TC016='9' THEN '9.不計稅'  ELSE '' END) END) END) END ) END AS 'NEWTC016'
                                ,CASE WHEN TC121='1' THEN '1.二聯式' ELSE (CASE WHEN TC121='2' THEN '2.三聯式' ELSE (CASE WHEN TC121='3' THEN '3.二聯式收銀機發票' ELSE (CASE WHEN TC121='4' THEN '4.三聯式收銀機發票' ELSE (CASE WHEN TC121='5' THEN '5.電子計算機發票' ELSE (CASE WHEN TC121='6' THEN '6.免用統一發票' ELSE (CASE WHEN TC121='7' THEN '7.電子發票' ELSE '' END) END) END) END) END) END) END AS 'NEWTC121'
                                ,BA
                                ,BANAME
                                ,(SELECT TOP 1 [USER_GUID] FROM [192.168.1.223].[UOF].[dbo].[TB_EB_USER] WHERE [ACCOUNT]=BA COLLATE Chinese_Taiwan_Stroke_BIN) AS 'BA_USER_GUID'
    
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
                                FROM [TK].dbo.COPTD,[TK].dbo.COPTC
                                LEFT JOIN [192.168.1.223].[{0}].[dbo].[TB_EB_USER] ON [TB_EB_USER].ACCOUNT= TC006 COLLATE Chinese_Taiwan_Stroke_BIN
                                WHERE TC001=TD001 AND TC002=TD002
                                AND TC001='{1}' AND TC002='{2}'
                                ) AS TEMP   
                              
                                ", DBNAME, TC001, TC002);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            

            if (dt.Rows.Count >= 1)
            {
                return dt;

            }
            else
            {
                return null;
            }

        }
        catch
        {
            return null;
        }
        finally
        {
           
        }
    }

    public DataTable SEARCHUOFDEP(string ACCOUNT)
    {
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder();
        DataSet ds1 = new DataSet();

        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            StringBuilder QUERYS = new StringBuilder();



            cmdTxt.AppendFormat(@" 
                                 SELECT 
                                [GROUP_NAME] AS 'DEPNAME'
                                ,[TB_EB_EMPL_DEP].[GROUP_ID]+','+[GROUP_NAME]+',False' AS 'DEPNO'
                                ,[TB_EB_USER].[USER_GUID]
                                ,[ACCOUNT]
                                ,[NAME]
                                ,[TB_EB_EMPL_DEP].[GROUP_ID]
                                ,[TITLE_ID]     
                                ,[GROUP_NAME]
                                ,[GROUP_CODE]
                                FROM [192.168.1.223].[{0}].[dbo].[TB_EB_USER],[192.168.1.223].[{0}].[dbo].[TB_EB_EMPL_DEP],[192.168.1.223].[{0}].[dbo].[TB_EB_GROUP]
                                WHERE [TB_EB_USER].[USER_GUID]=[TB_EB_EMPL_DEP].[USER_GUID]
                                AND [TB_EB_EMPL_DEP].[GROUP_ID]=[TB_EB_GROUP].[GROUP_ID]
                                AND ISNULL([TB_EB_GROUP].[GROUP_CODE],'')<>''
                                AND [ACCOUNT]='{1}'
                              
                                ", DBNAME, ACCOUNT);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));
           

            if (dt.Rows.Count >= 1)
            {
                return dt;

            }
            else
            {
                return null;
            }

        }
        catch
        {
            return null;
        }
        finally
        {
           
        }
    }

    public string SEARCHFORM_VERSION_ID(string FORM_NAME)
    {
        try
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
            Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

            StringBuilder cmdTxt = new StringBuilder();
            StringBuilder QUERYS = new StringBuilder();



            cmdTxt.AppendFormat(@" 
                                   SELECT 
                                   RTRIM(LTRIM([FORM_VERSION_ID])) AS FORM_VERSION_ID
                                   ,[FORM_NAME]
                                   FROM [TKIT].[dbo].[UOF_FORM_VERSION_ID]
                                   WHERE [FORM_NAME]='{0}'
                                   ", FORM_NAME);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt.Rows.Count >= 1)
            {
                return dt.Rows[0]["FORM_VERSION_ID"].ToString();
            }
            else
            {
                return "";
            }

        }
        catch
        {
            return "";
        }
        finally
        {
           
        }
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
        //this.Session["SDATE"] = txtDate1.Text.Trim();
        //this.Session["EDATE"] = txtDate2.Text.Trim();
    }

    protected void btn2_Click(object sender, EventArgs e)
    {
        SETEXCEL();
    }
    protected void btn3_Click(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=test.xls");
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
        HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=big5>");
        HttpContext.Current.Response.Write("<head><meta http-equiv=Content-Type content=text/html;charset=big5></head>");
        Response.Charset = "big5";
        Response.ContentType = "application/excel";


        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        Grid1.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
    protected void MyButtonClick(object sender, System.EventArgs e)
    {


    }

    protected void btn5_Click(object sender, EventArgs e)
    {
        BindGrid("");
    }
    #endregion
}