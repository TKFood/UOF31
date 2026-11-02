using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Ede.Uof.Utility.Page.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using UOFAssist.WKF;

/**
* 修改時間：2020/09/25
* 修改人員：陳緯榕
* 修改項目：
    * 按下變更狀態後，需判斷是否有勾選才要處理
* 發生原因：
    * 巡覽時，只判斷是否有勾選鈕就當作要變更了
* 修改位置：
    * 「btnRUN_Click」中，判斷是否有勾選鈕還要使用〈Request〉來判斷〈kcbSelect〉是否有被勾選
* **/

public partial class CDS_KYTUtils_RFC_STATUS_REPORT : BasePage
{
    protected string FORM_NAME = "SAP RFC 交易查詢與作業";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode(FORM_NAME, Request.Url.AbsoluteUri);
            #region 綁定表單名稱下拉選單

            kddlSFORM_NAME.DataSource = getFORMNAME();
            kddlSFORM_NAME.DataTextField = "FORM_NAME";
            kddlSFORM_NAME.DataValueField = "FORM_ID";
            kddlSFORM_NAME.DataBind();

            #endregion 綁定表單名稱下拉選單

            kdpSTART.Text = DateTime.Now.ToLongDateString();
            kdpSTART.ViewType = UOFAssist.WKF.KYTViewType.Input;
            kdpEND.Text = DateTime.Now.ToLongDateString();
            kdpEND.ViewType = UOFAssist.WKF.KYTViewType.Input;
        }
    }

    /// <summary>
    /// 取得有維護的表單名稱
    /// </summary>
    /// <returns></returns>
    private DataTable getFORMNAME()
    {
        DataTable dtReturn = new DataTable();
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT DISTINCT FORM_NAME, FORM_ID FROM ZUOF_RFC_MA
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
                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"RFC_STATUS_REPORT.getFORMNAME.SELECT.ERROR:{0}", e.Message));
            }
        }
        DataRow ndr = dtReturn.NewRow();
        ndr["FORM_NAME"] = "====請選擇====";
        dtReturn.Rows.InsertAt(ndr, 0);
        return dtReturn;
    }

    /// <summary>
    /// 明細項資料綁定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvItems_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        gr.Attributes.Add("onmouseover", "this.bkc=this.style.backgroundColor;this.style.backgroundColor='#ccffff'"); // 設定每筆資料在滑鼠移動進去時的背景顏色
        gr.Attributes.Add("onmouseout", "this.style.backgroundColor=this.bkc"); // 設定每筆資料在滑鼠移出時的背景色
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblItems = gvItems.DataTable;
            DataRow row = tblItems.Rows[gr.DataItemIndex];
            LinkButton lbtnReSend = (LinkButton)gr.FindControl("lbtnReSend"); // 重送按鈕
            KYTCheckBox kcbSelect = (KYTCheckBox)gr.FindControl("kcbSelect"); // RFC傳送狀態
            KYTDropDownList kddlRFC_STATUS = (KYTDropDownList)gr.FindControl("kddlRFC_STATUS"); // 選取
            LinkButton lbtnDOC_NBR = gr.FindControl("lbtnDOC_NBR") as LinkButton; // 表單編號

            lbtnReSend.Visible = false;
            kcbSelect.Visible = false;
            kddlRFC_STATUS.Visible = false;
            kcbSelect.Checked = false;

            if (row["TRIGGER_STATUS"].ToString() == "0")
            {
                lbtnReSend.Visible = true;
            }
            else
            {
                kddlRFC_STATUS.Visible = true;
                kcbSelect.Visible = true;
            }
            //KYTUtilLibs.Utils.WebUtils.DynamicListItemBind(kddlRFC_STATUS, new object[] {
            //    new { Text = "成功", Value = "1" },
            //    new { Text = "失敗", Value = "0" },
            //    new { Text = "取消", Value = "NULL" }
            //});
            
            kddlRFC_STATUS.SelectedValue = row["SEND_RESULT"].ToString() == "True" ? "1" : row["SEND_RESULT"].ToString() == "False" ? "0" : "NULL";

            string url = "";
            //if () // 現在是APP
            //    url = "~/WKF/FormUse/Mobile/ViewForm.aspx";
            //else
            url = "~/WKF/FormUse/ViewForm.aspx";
            Ede.Uof.Utility.Page.Common.Dialog.Open2(lbtnDOC_NBR, url, row["DOC_NBR"].ToString(), 960, 720, Ede.Uof.Utility.Page.Common.Dialog.PostBackType.None, new { TASK_ID = row["TASK_ID"].ToString() }.ToExpando());
        }
    }

    /// <summary>
    /// 按下搜尋按鈕後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string sqlExtend_form = "";
        string sqlExtend_docnbr = "";
        string sqlExtend = "";
        if (!string.IsNullOrWhiteSpace(kddlSFORM_NAME.SelectedValue))
        {
            sqlExtend_form = " WHERE FORM_ID = @FORM_ID -- 只找特定表單";
        }
        if (!string.IsNullOrWhiteSpace(ktxtSDOC_NBR.Text.Trim()))
        {
            sqlExtend_docnbr = " AND DOC_NBR LIKE '%' + @POSBIBLE_DOC_NBR + '%'";
        }
        if (!string.IsNullOrEmpty(ktxtSDISPLAY_TITLE.Text.Trim()))
        {
            sqlExtend += sqlExtend.Length == 0 ? " WHERE " : "AND";
            sqlExtend += " DISPLAY_TITLE LIKE '%' + @POSBIBLE_DISPLAY + '%' ";
        }
        if (!string.IsNullOrEmpty(kddlRFC_STATUS.SelectedValue))
        {
            sqlExtend += sqlExtend.Length == 0 ? " WHERE " : "AND";
            sqlExtend += kddlRFC_STATUS.SelectedValue == "NULL" ? " SEND_RESULT IS NULL " : " SEND_RESULT = @SEND_RESULT ";
        }
        if (!string.IsNullOrEmpty(kdpSTART.Text.Trim()) &&
            !string.IsNullOrEmpty(kdpEND.Text.Trim()))
        {
            sqlExtend += sqlExtend.Length == 0 ? " WHERE " : "AND";
            sqlExtend += " CAST(CONVERT(VARCHAR, APPLICANTDATE, 102) AS DATE) BETWEEN @START_TIME AND @END_TIME ";
        }


        using (SqlDataAdapter sda = new SqlDataAdapter(string.Format(@"
            DECLARE @ALL_FORM TABLE(
	            TASK_ID NVARCHAR(50)
            )
            INSERT INTO @ALL_FORM
            SELECT TASK_ID -- 找出所有表單
              FROM TB_WKF_TASK 
             WHERE FORM_VERSION_ID IN (SELECT FORM_VERSION_ID -- 找到對應的表單的所有版本
							             FROM TB_WKF_FORM_VERSION
							            WHERE FORM_ID IN (SELECT DISTINCT FORM_ID 
												            FROM ZUOF_RFC_MA
											               {0}
											              ))
            {1}

            SELECT * FROM (
	             SELECT F.TASK_ID, 
                        F.FORM_VERSION_ID,
			            F.DOC_NBR, -- 表單單號
			            ISNULL((SELECT TOP 1 FORM_NAME 
					              FROM ZUOF_RFC_MA 
					             WHERE FORM_ID = F.FORM_ID), '') AS 'FORM_NAME', -- 表單名稱
			            F.FORM_ID, -- 表單代碼
			            F.DISPLAY_TITLE, -- 表單標題
			            F.[VERSION], -- 表單版本
			            F.BEGIN_TIME AS 'APPLICANTDATE', -- 表單起單時間
			            F.END_TIME, -- 表單結案時間 
			            F.TRIGGER_STATUS,  -- 表單事件的狀態
			            ZR.SEND_RESULT, -- RFC呼叫結果
			            ZR.MODIFY_TIME, -- RFC呼叫時間
			            ZR.MSG_EXCEPTION, -- RFC呼叫結果的回傳訊息
			            ZR.[TYPE] -- RFC名稱
	               FROM (SELECT T.TASK_ID, 
					            T.DOC_NBR, 
					            T.END_TIME, 
					            T.BEGIN_TIME, 
					            T.DISPLAY_TITLE,
                                T.FORM_VERSION_ID,
					            ISNULL((SELECT [VERSION] 
							              FROM TB_WKF_FORM_VERSION 
							             WHERE FORM_VERSION_ID = T.FORM_VERSION_ID), 0) AS 'VERSION', -- 表單版本
					            ISNULL((SELECT FORM_ID 
							              FROM TB_WKF_FORM_VERSION 
							             WHERE FORM_VERSION_ID = T.FORM_VERSION_ID), '') AS 'FORM_ID', -- 表單代碼
					            CASE WHEN ISNULL((SELECT COUNT(*)
										            FROM TB_WKF_TASK_TRIGGER_RECORD -- 查詢事件紀錄
									               WHERE TASK_ID = T.TASK_ID
										             AND [STATUS] = 1 -- 結果是成功的
										             AND FORM_TYPE = 'END_FORM' -- 而且是結案事件
									             ), 0) > 0 -- 如果結案事件成功的次數大於0，那就表示ZUOF_RFC也是成功的
						             THEN 1 -- 成功
						             ELSE 0 -- 失敗
						              END AS 'TRIGGER_STATUS' -- 呼叫事件的狀態
			               FROM TB_WKF_TASK AS T 
			              WHERE T.TASK_ID IN (SELECT TASK_ID FROM @ALL_FORM)) AS F -- 找到所有符合條件的表單
              LEFT JOIN ZUOF_RFC AS ZR -- 如果有查到ZUOF_RFC的對應資料，那就會加到查詢結果內，如果沒有符合的值也會加入
		             ON F.TASK_ID = ZR.TASK_ID -- 表單的TASK_ID
		            AND ZR.TYPE IN (SELECT RFC_NAME -- 找到表單所維護的RFC名稱
						              FROM ZUOF_RFC_MA 
						              WHERE FORM_ID IN (SELECT FORM_ID -- 找到表單版本所對應的表單代號
											              FROM TB_WKF_FORM_VERSION 
											             WHERE FORM_VERSION_ID IN (SELECT FORM_VERSION_ID -- 用TASK_ID找到對應的表單版本代號
																		             FROM TB_WKF_TASK 
																		            WHERE TASK_ID = F.TASK_ID)))
   
               ) AS A
               {2}
               ORDER BY END_TIME DESC -- 結案時間 倒排
            ", sqlExtend_form, sqlExtend_docnbr, sqlExtend)
            , new DatabaseHelper().Command.Connection.ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@FORM_ID", kddlSFORM_NAME.SelectedValue);
            sda.SelectCommand.Parameters.AddWithValue("@POSBIBLE_DOC_NBR", ktxtSDOC_NBR.Text.Trim());
            sda.SelectCommand.Parameters.AddWithValue("@POSBIBLE_DISPLAY", ktxtSDISPLAY_TITLE.Text.Trim());
            sda.SelectCommand.Parameters.AddWithValue("@SEND_RESULT", kddlRFC_STATUS.SelectedValue);
            sda.SelectCommand.Parameters.AddWithValue("@START_TIME", kdpSTART.Text.Trim());
            sda.SelectCommand.Parameters.AddWithValue("@END_TIME", kdpEND.Text.Trim());
            try
            {
                sda.Fill(ds);
                gvItems.DataSource = ds.Tables[0];
                gvItems.DataBind();
                gvItems.BindDataOnly = true;

            }
            catch (Exception ex)
            {
                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"RFC_STATUS_REPORT.btnSearch_Click.ERROR: {0}", ex.Message));
            }
        }
    }

    /// <summary>
    /// 按下變更RFC狀態按鈕後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRUN_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gvItems.Rows)
        {
            DataTable tblItems = gvItems.DataTable;
            DataRow row = tblItems.Rows[gr.DataItemIndex];
            KYTCheckBox kcbSelect = (KYTCheckBox)gr.FindControl("kcbSelect"); // RFC傳送狀態
            KYTDropDownList kddlRFC_STATUS = (KYTDropDownList)gr.FindControl("kddlRFC_STATUS"); // 選取
            if (kcbSelect.Visible &&
                Request[kcbSelect.UniqueID + "$CheckBox1"] != null &&
                kddlRFC_STATUS.Visible)
            {
                string RFC_STATUS = Request[kddlRFC_STATUS.UniqueID + "$DropDownList1"];
                using (SqlDataAdapter sda = new SqlDataAdapter(@"
                    IF EXISTS (SELECT * 
                                 FROM ZUOF_RFC 
                                WHERE TASK_ID = @TASK_ID
                                  AND [TYPE] = @TYPE)
                            BEGIN
                                UPDATE ZUOF_RFC
                                   SET SEND_RESULT = @SEND_RESULT,
                                       MODIFIER = @MODIFIER,
                                       MODIFY_TIME = GETDATE(),
                                       MSG_EXCEPTION = '手動變更狀態'
                                 WHERE TASK_ID = @TASK_ID
                                   AND [TYPE] = @TYPE
                            END
                        ELSE
                            BEGIN
                                INSERT INTO ZUOF_RFC ([TASK_ID], [TYPE], [FORM_NBR], [SEND_RESULT], [SEND_TIME], [MSG_EXCEPTION], [CREATOR], [CREATE_TIME], [MODIFIER], [MODIFY_TIME], [FORM_VERSION_ID], [FORM_NAME])
                                     SELECT @TASK_ID, 
                                            RFC_NAME, 
                                            @FORM_NBR, 
                                            @SEND_RESULT, 
                                            GETDATE(), 
                                            '手動變更狀態', 
                                            @MODIFIER, 
                                            GETDATE(), 
                                            @MODIFIER, 
                                            GETDATE(), 
                                            @FORM_VERSION_ID, 
                                            @FORM_NAME
                                       FROM ZUOF_RFC_MA 
                                      WHERE FORM_ID = (SELECT FORM_ID 
				                                         FROM TB_WKF_FORM_VERSION
				                                        WHERE FORM_VERSION_ID = @FORM_VERSION_ID)
                            END
                    
                ", new DatabaseHelper().Command.Connection.ConnectionString))
                using (DataSet ds = new DataSet())
                {
                    sda.SelectCommand.Parameters.AddWithValue("@SEND_RESULT", RFC_STATUS == "NULL" ? Convert.DBNull : RFC_STATUS == "1");
                    sda.SelectCommand.Parameters.AddWithValue("@MODIFIER", Ede.Uof.EIP.SystemInfo.Current.UserGUID);
                    sda.SelectCommand.Parameters.AddWithValue("@TASK_ID", row["TASK_ID"].ToString());
                    sda.SelectCommand.Parameters.AddWithValue("@TYPE", row["TYPE"].ToString());
                    sda.SelectCommand.Parameters.AddWithValue("@FORM_NBR", row["DOC_NBR"].ToString());
                    sda.SelectCommand.Parameters.AddWithValue("@FORM_VERSION_ID", row["FORM_VERSION_ID"].ToString());
                    sda.SelectCommand.Parameters.AddWithValue("@FORM_NAME", row["FORM_NAME"].ToString());
                    try
                    {
                        sda.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"RFC_STATUS_REPORT.btnRUN_Click.foreach.TASK_ID: {0}.DOC_NBR: {1}.TYPE: {2}. RFC變更狀態由 : {3}→{4}. DOC_NBR: {6}. FORM_VERSION_ID: {7}. FORM_NAME: {8} .ERROR: {5}",
                            row["TASK_ID"].ToString(),
                            row["DOC_NBR"].ToString(),
                            row["TYPE"].ToString(),
                            row["SEND_RESULT"].ToString() == "True" ? "成功" : row["SEND_RESULT"].ToString() == "False" ? "失敗" : "取消",
                            kddlRFC_STATUS.SelectedItem.Text,
                            ex.Message,
                            row["DOC_NBR"].ToString(),
                            row["FORM_VERSION_ID"].ToString(),
                            row["FORM_NAME"].ToString()
                            ));
                    }
                }
            }
        }
        btnSearch_Click(btnSearch, null); // 重新整理明細項
    }

    protected void lbtnReSend_Click(object sender, EventArgs e)
    {
        LinkButton lbtnReSend = (LinkButton)sender;
        GridViewRow gr = lbtnReSend.NamingContainer as GridViewRow;
        DataTable tblItems = gvItems.DataTable;
        DataRow row = tblItems.Rows[gr.DataItemIndex];
        //string[] taskids = new string[1] { row["TASK_ID"].ToString() };
        //Ede.Uof.WKF.Utility.CallbackUCO callbackuco = new Ede.Uof.WKF.Utility.CallbackUCO();
        //callbackuco.BatchrecallbackByEndForm(taskids);
        //btnSearch_Click(btnSearch, null); // 重新整理明細項

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT * FROM ZUOF_RFC_MA WHERE FORM_ID = @FORM_ID
            ", new DatabaseHelper().Command.Connection.ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@FORM_ID", row["FORM_ID"].ToString());
            try
            {
                sda.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    KYTUtilLibs.Utils.UOFUtils.UOFFormTrigger.ProcessTriggerRFCReprocess(dr["DLL_FILE_NAME"].ToString(), dr["DLL_PATH"].ToString(), dr["RE_PROCESS_NAME"].ToString(), new object[] { row["TASK_ID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"RFC_STATUS_REPORT.lbtnReSend_Click.SELECT.ERROR:{0}", ex.Message));
            }
        }


    }
}
