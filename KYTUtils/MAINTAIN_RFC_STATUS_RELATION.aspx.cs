using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using UOFAssist.WKF;

public partial class CDS_KYTUtils_MAINTAIN_RFC_STATUS_RELATION : BasePage
{
    protected string FORM_NAME = "SAP RFC與UOF關聯維護";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode(FORM_NAME, Request.Url.AbsoluteUri);
            gvForms.DataSource = getZUOF_RFC_MA();
            gvForms.DataBind();
        }
    }

    private DataTable getZUOF_RFC_MA()
    {
        DataTable dtReturn = new DataTable();


        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT FORM_ID,
                   RFC_NAME,
                   FORM_NAME,
                   DLL_PATH,
                   DLL_FILE_NAME,
                   RE_PROCESS_NAME,
                   ZTABLE_NAME,
                   'Y' AS 'IS_DBDATA'
              FROM ZUOF_RFC_MA
            ", new DatabaseHelper().Command.Connection.ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("", "");
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    dtReturn = ds.Tables[0];
                }
                else
                    dtReturn = CreateTableSchema();
            }
            catch (Exception e)
            {
                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"MAINTAIN_RFC_STATUS_RELATION.getZUOF_RFC_MA.SELECT.ERROR:{0}", e.Message));
            }
        }

        return dtReturn;
    }

    private DataTable CreateTableSchema()
    {
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("FORM_ID", typeof(string))); // UOF FORM_ID
        dtSource.Columns.Add(new DataColumn("RFC_NAME", typeof(string))); // RFC方法名稱
        dtSource.Columns.Add(new DataColumn("FORM_NAME", typeof(string))); // 表單名稱
        dtSource.Columns.Add(new DataColumn("DLL_PATH", typeof(string))); // DLL組件路徑
        dtSource.Columns.Add(new DataColumn("DLL_FILE_NAME", typeof(string))); // DLL檔案名稱
        dtSource.Columns.Add(new DataColumn("RE_PROCESS_NAME", typeof(string))); // 重送RFC方法名稱
        dtSource.Columns.Add(new DataColumn("ZTABLE_NAME", typeof(string))); // 中繼表主表表格名稱
        dtSource.Columns.Add(new DataColumn("IS_DBDATA", typeof(string))); // 源自於DB的資料

        return dtSource;
    }

    private DataTable getFORMNAME()
    {
        DataTable dtReturn = new DataTable();
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
          
            SELECT [VERSION], -- 最大的表單版本
	               FORM_ID, -- 表單的ID
	               ISNULL((SELECT FORM_NAME -- 表單名稱
	   		                 FROM TB_WKF_FORM 
	   		                WHERE FORM_ID = V.FORM_ID), '') AS 'FORM_NAME',
	               ISNULL((SELECT [CATEGORY_NAME] -- 取得表單類別名稱
	   		                 FROM [TB_WKF_FORM_CATEGORY] 
	   		                WHERE CATEGORY_ID = (SELECT CATEGORY_ID 
	   								               FROM TB_WKF_FORM 
	   							                  WHERE FORM_ID = V.FORM_ID)), '') AS 'CATEGORY_NAME'
              FROM (SELECT MAX([VERSION]) AS 'VERSION', 
			               FORM_ID
		              FROM TB_WKF_FORM_VERSION
		             WHERE FORM_ID IN (SELECT FORM_ID 
							             FROM TB_WKF_FORM)
		               AND ISSUE_CTL = 1
		               AND [VERSION] >= 1
                  GROUP BY FORM_ID) AS V
            ORDER BY CATEGORY_NAME, FORM_NAME
            ", new DatabaseHelper().Command.Connection.ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("", "");
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    dtReturn = ds.Tables[0];
                }
            }
            catch (Exception e)
            {
                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"MAINTAIN_RFC_STATUS_RELATION.getFORMNAME.SELECT.ERROR:{0}", e.Message));
            }
        }
        DataRow ndr = dtReturn.NewRow();
        ndr["FORM_NAME"] = "====請選擇====";
        dtReturn.Rows.InsertAt(ndr, 0);
        return dtReturn;
    }

    /// <summary>
    /// 取得TextBox物件所能設定的最大寬度
    /// </summary>
    /// <param name="ktxtControl">KYTTextBox物件</param>
    /// <param name="maxCharLength">最大字元長度</param>
    /// <param name="minimumWidth">最小寬度</param>
    /// <param name="fontSize">輸入框內字元大小</param>
    public int getTextBoxWidth(KYTTextBox ktxtControl, int maxCharLength, int minimumWidth, float fontSize = 12.0F)
    {
        string maxlengthStr = ktxtControl.Text.Trim().Length <= maxCharLength ? ktxtControl.Text.Trim() : ktxtControl.Text.Trim().Substring(0, maxCharLength);
        System.Drawing.Size size = System.Windows.Forms.TextRenderer.MeasureText(maxlengthStr, new System.Drawing.Font(ktxtControl.TextBox1.Font.Name, fontSize));
       return size.Width < minimumWidth ? minimumWidth : size.Width;
    }

    /// <summary>
    /// 按下新增按鈕後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DataTable dtSource = gvForms.DataTable;
        DataRow ndr = dtSource.NewRow();
        foreach (DataColumn dc in dtSource.Columns)
        {

            ndr[dc.ColumnName] = "";
        }
        dtSource.Rows.Add(ndr);
        gvForms.BindDataOnly = true; // 不要收納現在網頁上的內容
        gvForms.DataSource = dtSource;
        gvForms.DataBind();
    }

    /// <summary>
    /// 按下儲存按鈕後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        foreach (DataRow dr in gvForms.DataTable.Rows)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SET NOCOUNT ON;
                IF NOT EXISTS (SELECT * 
                                 FROM [ZUOF_RFC_MA] 
                                WHERE FORM_ID = @FORM_ID
                                  AND RFC_NAME = @RFC_NAME)
                            BEGIN
                                INSERT INTO [ZUOF_RFC_MA] ([FORM_ID], [RFC_NAME], [FORM_NAME], [DLL_PATH], [ZTABLE_NAME], [DLL_FILE_NAME], [RE_PROCESS_NAME], [MODIFY_TIME], [MODIFIER]) 
                                     SELECT @FORM_ID, @RFC_NAME, FORM_NAME, @DLL_PATH, @ZTABLE_NAME, @DLL_FILE_NAME, @RE_PROCESS_NAME, GETDATE(), @MODIFIER
                                       FROM TB_WKF_FORM
                                      WHERE FORM_ID = @FORM_ID
                              END
                         ELSE
                            BEGIN
                                UPDATE [ZUOF_RFC_MA] 
                                   SET [DLL_PATH] = @DLL_PATH, 
                                       [ZTABLE_NAME] = @ZTABLE_NAME,
                                       [MODIFY_TIME] = GETDATE(),
                                       [MODIFIER] = @MODIFIER,
                                       [DLL_FILE_NAME] = @DLL_FILE_NAME,
                                       [RE_PROCESS_NAME] = @RE_PROCESS_NAME
                                 WHERE [FORM_ID] = @FORM_ID 
                                   AND [RFC_NAME] = @RFC_NAME 
                              END
            ", new DatabaseHelper().Command.Connection.ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@FORM_ID", dr["FORM_ID"]); // UOF FORM_ID
                sda.SelectCommand.Parameters.AddWithValue("@RFC_NAME", dr["RFC_NAME"]); // RFC方法名稱
                sda.SelectCommand.Parameters.AddWithValue("@FORM_NAME", dr["FORM_NAME"]); // 表單名稱
                sda.SelectCommand.Parameters.AddWithValue("@DLL_PATH", dr["DLL_PATH"]); // DLL檔案名稱
                sda.SelectCommand.Parameters.AddWithValue("@DLL_FILE_NAME", dr["DLL_FILE_NAME"]); // DLL檔案名稱
                sda.SelectCommand.Parameters.AddWithValue("@RE_PROCESS_NAME", dr["RE_PROCESS_NAME"]); // 重送RFC方法名稱
                sda.SelectCommand.Parameters.AddWithValue("@ZTABLE_NAME", dr["ZTABLE_NAME"]); // 中繼表主表表格名稱
                sda.SelectCommand.Parameters.AddWithValue("@MODIFIER", Current.Account); // 中繼表主表表格名稱
                try
                {
                    sda.Fill(ds);
                    gvForms.DataSource = getZUOF_RFC_MA();
                    gvForms.DataBind();
                }
                catch (Exception ex)
                {
                    KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"MAINTAIN_RFC_STATUS_RELATION.btnSave_Click.MODIFY.ZUOF_RFC_MA.ERROR::{0}", ex.Message));
                }
            }
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), "alert('儲存成功');", true);
    }

    protected void lbtnCOPY_Click(object sender, EventArgs e)
    {
        LinkButton _lbtnCOPY = (LinkButton)sender;
        GridViewRow gr = _lbtnCOPY.NamingContainer as GridViewRow;

        DataTable dtSource = gvForms.DataTable;

        DataRow ndr = dtSource.NewRow();
        foreach (DataColumn dc in dtSource.Columns)
        {
            switch (dc.ColumnName)
            {
                case "IS_DBDATA":
                    ndr[dc.ColumnName] = "N";
                    break;
                default:
                    ndr[dc.ColumnName] = dtSource.Rows[gr.DataItemIndex][dc.ColumnName];
                    break;
            }
        }
        dtSource.Rows.InsertAt(ndr, gr.DataItemIndex + 1); // 新增在下面
        gvForms.BindDataOnly = true; // 不要收納現在網頁上的內容
        gvForms.DataSource = dtSource;
        gvForms.DataBind();
    }

    protected void lbtnDEL_Click(object sender, EventArgs e)
    {
        LinkButton _lbtnDEL = (LinkButton)sender;
        GridViewRow gr = _lbtnDEL.NamingContainer as GridViewRow;
        KYTDropDownList kddlFormName = (KYTDropDownList)gr.FindControl("kddlFormName"); // 表單名稱
        KYTTextBox ktxtRFC_NAME = (KYTTextBox)gr.FindControl("ktxtRFC_NAME"); // RFC方法名稱
        KYTTextBox ktxtDLL_PATH = (KYTTextBox)gr.FindControl("ktxtDLL_PATH"); // DLL組件路徑
        KYTTextBox ktxtZTABLE_NAME = (KYTTextBox)gr.FindControl("ktxtZTABLE_NAME"); // 中繼表主表表格名稱

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SET NOCOUNT ON;
                DELETE
                  FROM [ZUOF_RFC_MA] 
                 WHERE [FORM_ID] = @FORM_ID 
                   AND [RFC_NAME] = @RFC_NAME 
            ", new DatabaseHelper().Command.Connection.ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@FORM_ID", kddlFormName.SelectedValue);
            sda.SelectCommand.Parameters.AddWithValue("@RFC_NAME", ktxtRFC_NAME.Text.Trim());
            try
            {
                sda.Fill(ds);
                DataTable dtSource = gvForms.DataTable;
                dtSource.Rows.RemoveAt(gr.DataItemIndex);

                gvForms.BindDataOnly = true; // 不要收納現在網頁上的內容
                gvForms.DataSource = dtSource;
                gvForms.DataBind();
            }
            catch (Exception ex)
            {
                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"MAINTAIN_RFC_STATUS_RELATION.lbtnDEL_Click.DELETE.ZUOF_RFC_MA.ERROR::{0}", ex.Message));
            }
        }
    }

    protected void gvForms_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        gr.Attributes.Add("onmouseover", "this.bkc=this.style.backgroundColor;this.style.backgroundColor='#ccffff'"); // 設定每筆資料在滑鼠移動進去時的背景顏色
        gr.Attributes.Add("onmouseout", "this.style.backgroundColor=this.bkc"); // 設定每筆資料在滑鼠移出時的背景色
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblItems = gvForms.DataTable;
            DataRow row = tblItems.Rows[gr.DataItemIndex];
            LinkButton lbtnCOPY = (LinkButton)gr.FindControl("lbtnCOPY"); // 複製按鈕
            LinkButton lbtnDEL = (LinkButton)gr.FindControl("lbtnDEL"); // 刪除按鈕
            KYTDropDownList kddlFormName = (KYTDropDownList)gr.FindControl("kddlFormName"); // 表單名稱
            KYTTextBox ktxtRFC_NAME = (KYTTextBox)gr.FindControl("ktxtRFC_NAME"); // RFC方法名稱
            KYTTextBox ktxtDLL_FILE_NAME = (KYTTextBox)gr.FindControl("ktxtDLL_FILE_NAME"); // DLL檔案名稱
            KYTTextBox ktxtDLL_PATH = (KYTTextBox)gr.FindControl("ktxtDLL_PATH"); // DLL組件路徑
            KYTTextBox ktxtRE_PROCESS_NAME = (KYTTextBox)gr.FindControl("ktxtRE_PROCESS_NAME"); // 重送RFC方法名稱
            KYTTextBox ktxtZTABLE_NAME = (KYTTextBox)gr.FindControl("ktxtZTABLE_NAME"); // 中繼表主表表格名稱
            HiddenField hidDBData = (HiddenField)gr.FindControl("hidDBData"); // 在DB有資料
            hidDBData.Value = row["IS_DBDATA"].ToString() == "Y" ? "Y" : "N";
            if (kddlFormName.DataSource == null)
            {
                kddlFormName.DataSource = getFORMNAME();
                kddlFormName.DataTextField = "FORM_NAME";
                kddlFormName.DataValueField = "FORM_ID";
                kddlFormName.DataBind();
            }
            kddlFormName.SelectedValue = row["FORM_ID"].ToString();
            kddlFormName.DropDownList1.Width = 150;
            if (row["IS_DBDATA"].ToString() == "Y")
            {
                lbtnDEL.OnClientClick = string.Format(@"return confirm('是否要刪除？\n, 表單名稱： {0}, RFC： {1}');",
                   kddlFormName.SelectedItem.Text,
                   !string.IsNullOrEmpty(ktxtRFC_NAME.Text.Trim()) ? ktxtRFC_NAME.Text.Trim() : "未填寫");
            }
           
        }
        else if (gr.RowType == DataControlRowType.Footer)
        {
            #region 設定輸入框最大寬度
            int MAX_RFC_NAME_Width = 0;
            int MAX_DLL_FILE_NAME_Width = 0;
            int MAX_DLL_PATH_Width = 0;
            int MAX_RE_PROCESS_NAMEE_Width = 0;
            int MAX_ZTABLE_NAME_Width = 0;
            foreach (GridViewRow _gr in gvForms.Rows)
            {
                KYTTextBox ktxtRFC_NAME = (KYTTextBox)_gr.FindControl("ktxtRFC_NAME"); // RFC方法名稱
                KYTTextBox ktxtDLL_FILE_NAME = (KYTTextBox)_gr.FindControl("ktxtDLL_FILE_NAME"); // DLL檔案名稱
                KYTTextBox ktxtDLL_PATH = (KYTTextBox)_gr.FindControl("ktxtDLL_PATH"); // DLL組件路徑
                KYTTextBox ktxtRE_PROCESS_NAME = (KYTTextBox)_gr.FindControl("ktxtRE_PROCESS_NAME"); // 重送RFC方法名稱
                KYTTextBox ktxtZTABLE_NAME = (KYTTextBox)_gr.FindControl("ktxtZTABLE_NAME"); // 中繼表主表表格名稱
                #region 取得最大寬度
                int RFC_NAME_Width = getTextBoxWidth(ktxtRFC_NAME, 30, 50);
                MAX_RFC_NAME_Width = RFC_NAME_Width > MAX_RFC_NAME_Width ? RFC_NAME_Width : MAX_RFC_NAME_Width;

                int DLL_FILE_NAME_Width = getTextBoxWidth(ktxtDLL_FILE_NAME, 15, 50);
                MAX_DLL_FILE_NAME_Width = DLL_FILE_NAME_Width > MAX_DLL_FILE_NAME_Width ? DLL_FILE_NAME_Width : MAX_DLL_FILE_NAME_Width;

                int DLL_PATH_Width = getTextBoxWidth(ktxtDLL_PATH, 50, 50);
                MAX_DLL_PATH_Width = DLL_PATH_Width > MAX_DLL_PATH_Width ? DLL_PATH_Width : MAX_DLL_PATH_Width;

                int RE_PROCESS_NAMEE_Width = getTextBoxWidth(ktxtRE_PROCESS_NAME, 20, 50);
                MAX_RE_PROCESS_NAMEE_Width = RE_PROCESS_NAMEE_Width > MAX_RE_PROCESS_NAMEE_Width ? RE_PROCESS_NAMEE_Width : MAX_RE_PROCESS_NAMEE_Width;

                int ZTABLE_NAME_Width = getTextBoxWidth(ktxtZTABLE_NAME, 25, 50);
                MAX_ZTABLE_NAME_Width = ZTABLE_NAME_Width > MAX_ZTABLE_NAME_Width ? ZTABLE_NAME_Width : MAX_ZTABLE_NAME_Width;
                #endregion 取得最大寬度
            }
            foreach (GridViewRow _gr in gvForms.Rows)
            {
                KYTTextBox ktxtRFC_NAME = (KYTTextBox)_gr.FindControl("ktxtRFC_NAME"); // RFC方法名稱
                KYTTextBox ktxtDLL_FILE_NAME = (KYTTextBox)_gr.FindControl("ktxtDLL_FILE_NAME"); // DLL檔案名稱
                KYTTextBox ktxtDLL_PATH = (KYTTextBox)_gr.FindControl("ktxtDLL_PATH"); // DLL組件路徑
                KYTTextBox ktxtRE_PROCESS_NAME = (KYTTextBox)_gr.FindControl("ktxtRE_PROCESS_NAME"); // 重送RFC方法名稱
                KYTTextBox ktxtZTABLE_NAME = (KYTTextBox)_gr.FindControl("ktxtZTABLE_NAME"); // 中繼表主表表格名稱
                ktxtRFC_NAME.Width = MAX_RFC_NAME_Width;
                ktxtDLL_FILE_NAME.Width = MAX_DLL_FILE_NAME_Width;
                ktxtDLL_PATH.Width = MAX_DLL_PATH_Width;
                ktxtRE_PROCESS_NAME.Width = MAX_RE_PROCESS_NAMEE_Width;
                ktxtZTABLE_NAME.Width = MAX_ZTABLE_NAME_Width;
            }
            #endregion 設定輸入框最大寬度
        }
    }
}
