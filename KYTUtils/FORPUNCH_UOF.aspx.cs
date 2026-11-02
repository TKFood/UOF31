using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

/**
* 修改時間：2020/04/09
* 修改人員：陳緯榕
* 修改項目：
    * 歸屬日和打卡時間不同時，歸屬日會寫入打卡時間的日期
* 發生原因：
    * 寫入資料庫的歸屬年、月、日都是使用rdtpPUNCH_TIME的時間，所以出錯
* 修改位置：
    * 「lbtnModify_Click」中，〈SQL〉的參數型〈YEAR〉、〈MONTH〉、〈DAY〉都由〈dtPUNCH〉改用〈dtVESTING〉取得
* **/

/**
* 修改時間：2020/04/07
* 修改人員：陳緯榕
* 修改項目：
    * 修改日和歸屬日的判斷只需要比對日期
* 發生原因：
    * 客戶歸屬日選擇2020/03/16，時間選擇2020/03/17 05:01，但出現「時間需為歸屬日的±1天」
* 修改位置：
    * 「lbtnModify_Click」中，〈rdtpPUNCH_TIME〉(時間)判斷時需要使用屬性〈Date〉來和〈rdpVESTING_DATE〉(歸屬日)使用屬性〈Date〉來比較日期
* **/

/// <summary>
/// UOF的打卡資料庫(TB_EIP_PUNCH)刷卡時間修正
/// </summary>
public partial class CDS_KYTUtils_FORPUNCH_UOF : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("UOF補卡修正", Request.Url.AbsoluteUri);
        }
    }

    protected void bcoAccount_EditButtonOnClick(string[] choiceResult)
    {
        txtAccount.Text = choiceResult[1];
        hidGuid.Value = choiceResult[0];
    }

    protected void lbtnModify_Click(object sender, EventArgs e)
    {
        string errorMsg = "";
        if (string.IsNullOrWhiteSpace(txtAccount.Text.Trim()) ||
            string.IsNullOrWhiteSpace(hidGuid.Value))
        {
            errorMsg += "帳號不可為空";
        }
        if (rdpVESTING_DATE.SelectedDate == null)
        {
            errorMsg += string.IsNullOrEmpty(errorMsg) ? "歸屬日不可為空" : "；歸屬日不可為空";
        }
        if (rdtpPUNCH_TIME.SelectedDate == null)
        {
            errorMsg += string.IsNullOrEmpty(errorMsg) ? "時間不可為空" : "；時間不可為空";
        }
        if (string.IsNullOrWhiteSpace(errorMsg))
        {
            DateTime dtVESTING = (DateTime)rdpVESTING_DATE.SelectedDate;
            DateTime dtPUNCH = (DateTime)rdtpPUNCH_TIME.SelectedDate;
            if (dtPUNCH.Date > dtVESTING.AddDays(1).Date ||
                dtPUNCH.Date < dtVESTING.AddDays(-1).Date)
            {
                errorMsg += string.IsNullOrEmpty(errorMsg) ? "時間需為歸屬日的±1天" : "；時間需為歸屬日的±1天";
            }
        }

        if (!string.IsNullOrWhiteSpace(errorMsg))
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"alert('{0}');", errorMsg), true);
        }
        else
        {
            DateTimeOffset dtVESTING = (DateTime)rdpVESTING_DATE.SelectedDate;
            DateTimeOffset dtPUNCH = (DateTime)rdtpPUNCH_TIME.SelectedDate;

            using (SqlDataAdapter sda = new SqlDataAdapter(@"
                SET NOCOUNT ON;
                DELETE FROM TB_EIP_PUNCH -- 先刪除
                      WHERE USER_GUID = @USER_GUID
                        AND YEAR = @YEAR
                        AND MONTH = @MONTH
                        AND DAY = @DAY
                        AND TYPE = @TYPE

                INSERT INTO [TB_EIP_PUNCH] ([PUNCH_GUID], [USER_GUID], [GROUP_GUID], [YEAR], [MONTH], [DAY], [TIME], [TYPE], [SOURCE], [MODIFY_DATE], [MODIFY_FROM], [MODIFY_USER], [CREATE_DATE], [CREATE_FROM], [CREATE_USER], [CLOCK_CODE], [CLOCK_TIME], [LATITUDE], [LONGITUDE]) 
                     SELECT @PUNCH_GUID, 
                            @USER_GUID, 
                            GROUP_ID, 
                            @YEAR,
                            @MONTH, 
                            @DAY, 
                            @TIME, 
                            @TYPE, 
                            @SOURCE, 
                            GETDATE(), 
                            @MODIFY_FROM, 
                            @MODIFY_USER, 
                            GETDATE(), 
                            @CREATE_FROM, 
                            @CREATE_USER, 
                            '', 
                            @CLOCK_TIME, 
                            @LATITUDE, 
                            @LONGITUDE
                       FROM TB_EB_EMPL_DEP
                      WHERE USER_GUID = @USER_GUID
                        AND ORDERS = 0
                ", new DatabaseHelper().Command.Connection.ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@PUNCH_GUID", Guid.NewGuid().ToString()); // 刷卡資料代碼
                sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", hidGuid.Value); // 使用者代碼
                sda.SelectCommand.Parameters.AddWithValue("@YEAR", dtVESTING.Year); // 歸屬年
                sda.SelectCommand.Parameters.AddWithValue("@MONTH", dtVESTING.Month); // 歸屬月
                sda.SelectCommand.Parameters.AddWithValue("@DAY", dtVESTING.Day); // 歸屬日
                sda.SelectCommand.Parameters.AddWithValue("@TIME", (dtPUNCH - dtVESTING).TotalMinutes); // 歸屬時間(分鐘數從00:00算)
                sda.SelectCommand.Parameters.AddWithValue("@TYPE", ddlWORKOFF.SelectedValue); // 類型,Work:上班,Off:下班
                sda.SelectCommand.Parameters.AddWithValue("@SOURCE", "External"); // 來源,Online:線上,External:外部,Manua
                sda.SelectCommand.Parameters.AddWithValue("@MODIFY_FROM", "::1"); // 異動IP
                sda.SelectCommand.Parameters.AddWithValue("@MODIFY_USER", Current.UserGUID); // 異動者
                sda.SelectCommand.Parameters.AddWithValue("@CREATE_FROM", "::1"); // 建立者IP
                sda.SelectCommand.Parameters.AddWithValue("@CREATE_USER", Current.UserGUID); // 建立者
                sda.SelectCommand.Parameters.AddWithValue("@CLOCK_TIME", dtPUNCH); // 刷卡時間
                sda.SelectCommand.Parameters.AddWithValue("@LATITUDE", Convert.DBNull); // 緯度
                sda.SelectCommand.Parameters.AddWithValue("@LONGITUDE", Convert.DBNull); // 經度
                try
                {
                    sda.Fill(ds);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"alert('打卡成功！');"), true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"alert('錯誤：{0}');", ex.Message), true);
                }
            }
        }
    }
}
