using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/**
 * 掛載方式：
 * `CDS\setting\Webpart.xml`設定
    <CustomControl ControlType="CDS_SCSHR_WebPages_WebPart_ShowTodayLVHList" ControlPath="~/CDS/SCSHR/WebPages/WebPart_ShowTodayLVHList.ascx" DefaultTitle="飛騰當天請假公差顯示" TitleStyle="j"
       ZoneIndex="0">
        <Resource Name="zh-CN" Value="飛騰當天請假公差顯示"/>
        <Resource Name="zh-TW" Value="飛騰當天請假公差顯示"/>
        <Resource Name="en-US" Value="飛騰當天請假公差顯示"/>
        <Resource Name="vi" Value="飛騰當天請假公差顯示"/>
        <Resource Name="id" Value="飛騰當天請假公差顯示"/>
        <Resource Name="mt-MT" Value="飛騰當天請假公差顯示"/>
        <Resource Name="ja-jp" Value="飛騰當天請假公差顯示"/>
      </CustomControl>
 * 資料庫資料表`TB_EB_WEBPART`使用`Edit`模式新增
    * CDS_SCSHR_WebPages_WebPart_ShowTodayLVHList	WebPart_ShowTodayLVHList	True	0	True	False	0	False
 * `系統管理 » 內部企業面板設定`操作
 * **/

/**
* 修改時間：2025/02/20
* 修改人員：黃仕聿
* 修改項目：
    * SQL內將日期欄位轉型後的型別由字串(VARCHAR(10))改為日期(DATE)
* 修改原因：
    * 無法正確取得當天的請假資訊，因為日期比對是轉型成字串(VARCHAR(10))進行非精準比對
        * 不確定為何2024年之前沒有問題，字串比對的規則暫不追究
* 修改位置：
    * SQL內，「CONVERT(VARCHAR(10), [DATE], 111)」都改成「CONVERT(DATE, [DATE], 111)」
* **/

/**
* 修改時間：2023/04/21
* 修改人員：林皓謙
* 修改項目：
    * 請假單的CANCEL_DOC_NBR 預設為NULL，判斷查詢到的請假單的欄位「CANCEL_DOC_NBR」是否存在於當日銷假資料時，若當日有銷假資料，則比對是否存在時會出錯
* 修改原因：
    * 弘裕提出，webpart在昨日會顯示請假人員，今日突然查詢不到
* 修改位置：
    * 「getLVHList」中，在原SQL AND CANCEL_DOC_NBR 更新為  AND (CANCEL_DOC_NBR ..... (SELECT.....)
    *                                                           OR CANCEL_DOC_NBR IS NULL)
* **/

/**
* 修改時間：2022/03/09
* 修改人員：高常昇
* 修改項目：
    * 過濾被銷假的請假單
* 修改原因：
    * 規格缺陷，導致已經被銷假的請假單沒有排除(A)
* 修改位置：
    * 「getLVHList」中，SQL更新(SH)
* **/

/**
* 修改時間：2021/04/19
* 修改人員：高常昇
* 修改項目：
    * UOF的Class名稱存在GridPager，與之衝突所以修改
* 修改位置：
    * 「前端網頁」中，將所有的「GridPager」改為「kyti-GridPager」
* **/

/// <summary>
/// 飛騰當天請假公差顯示
/// </summary>
public partial class CDS_SCSHR_WebPages_WebPart_ShowTodayLVHList : System.Web.UI.UserControl
{
    private string ConnectionString = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
        bool isShowChecked = false;
        ViewState["LVHList"] = getLVHList(out isShowChecked);
        gvMain.DataSource = ViewState["LVHList"] as DataTable;
        gvMain.DataBind();
        chkIncludeSubGroup.Visible = isShowChecked;
    }

    protected void chkIncludeSubGroup_CheckedChanged(object sender, EventArgs e)
    {
        bool isShowChecked = false;
        ViewState["LVHList"] = getLVHList(out isShowChecked);
        gvMain.DataSource = ViewState["LVHList"] as DataTable;
        gvMain.DataBind();
    }

    protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMain.PageIndex = e.NewPageIndex;
        gvMain.DataSource = ViewState["LVHList"];
        gvMain.DataBind();
    }

    private DataTable getLVHList(out bool isShowChecked)
    {
        isShowChecked = false;
        DataTable dtReturn = new DataTable();
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
             SET NOCOUNT ON;
               DECLARE @ALLGROUP TABLE ( -- 建立收納所有部門的虛擬表
                     GROUP_ID NVARCHAR(50)
                    )
                    IF (@IS_SHOW_SUB = '1')
                        BEGIN
                            INSERT INTO @ALLGROUP(GROUP_ID) -- 加進收納的虛擬表中
                              SELECT GROUP_ID 
                                FROM TB_EB_EMPL_FUNC
                               WHERE USER_GUID = @USER_GUID
                                    AND FUNC_ID = 'Superior' -- 如果登入者是部門主管

                             WHILE(@@ROWCOUNT > 0) 
                              BEGIN
                               INSERT INTO @ALLGROUP(GROUP_ID)
                                    SELECT GROUP_ID 
                                   FROM TB_EB_GROUP 
                                  WHERE PARENT_GROUP_ID IN (SELECT GROUP_ID -- 找到所有上層部門存在於收納的虛擬表中的部門
                                         FROM @ALLGROUP)
                                 AND GROUP_ID NOT IN (SELECT GROUP_ID -- 同時不找已經存在的部門
                                         FROM @ALLGROUP)
                              END
                          END
                 SELECT ISNULL((SELECT [NAME] + '(' + ACCOUNT + ')'  -- 找到請假人
                      FROM TB_EB_USER 
                     WHERE USER_GUID = LVH.LEAEMP), '') AS 'NAME',
                     CONVERT(VARCHAR, LVH.STARTTIME, 120) + ' ~ ' + CONVERT(VARCHAR, LVH.ENDTIME, 120) AS 'LVHTIME', -- 請假單請假時間
                     '' AS 'TRAVELTIME' -- 出差單出差時間
                   FROM Z_SCSHR_LEAVE AS LVH
                  WHERE (LEAEMP IN  (SELECT USER_GUID 
                          FROM TB_EB_EMPL_DEP
                         WHERE GROUP_ID IN (SELECT GROUP_ID FROM @ALLGROUP)) -- 而且要顯示子部門
                   OR LEAEMP IN (SELECT USER_GUID -- 或者只找自己部門內所有人
                         FROM TB_EB_EMPL_DEP
                        WHERE GROUP_ID IN (SELECT GROUP_ID 
                                FROM TB_EB_EMPL_DEP 
                            WHERE USER_GUID = @USER_GUID)))
                    AND CONVERT(DATE, GETDATE(), 111) BETWEEN CONVERT(DATE, STARTTIME, 111)  -- 今天的日期介於 請假單請假起始日期
                             AND CONVERT(DATE, ENDTIME, 111)  -- 和請假單請假結束日期
                    AND TASK_STATUS = 2 -- 同意
                    AND TASK_RESULT = 0 -- 且結案
              AND (CANCEL_DOC_NBR NOT IN (SELECT DOC_NBR 
                      FROM Z_SCSHR_CLEAVE 
                     WHERE CONVERT(DATE, GETDATE(), 111) BETWEEN CONVERT(DATE, STARTTIME, 111)  -- 今天的日期介於 請假單請假起始日期
                             AND CONVERT(DATE, ENDTIME, 111)  -- 和請假單請假結束日期
                     AND TASK_STATUS = 2 -- 同意
                     AND TASK_RESULT = 0 -- 且結案
                )
                OR CANCEL_DOC_NBR IS NULL)
                 UNION ALL -- 合併
                 SELECT ISNULL((SELECT [NAME] + '(' + ACCOUNT + ')'  -- 找到出差人
                      FROM TB_EB_USER 
                     WHERE USER_GUID = TRAVEL.TRAVEL_MEN), '') AS 'NAME',
                     '' AS 'LVHTIME', -- 請假單請假時間
                     CONVERT(VARCHAR, TRAVEL.STARTTIME, 120) + ' ~ ' + CONVERT(VARCHAR, TRAVEL.ENDTIME, 120) AS 'TRAVELTIME' -- 出差單出差時間
                   FROM Z_SCSHR_TRAVEL AS TRAVEL
                  WHERE (TRAVEL_MEN IN (SELECT USER_GUID 
                         FROM TB_EB_EMPL_DEP
                        WHERE GROUP_ID IN (SELECT GROUP_ID 
                              FROM TB_EB_EMPL_FUNC
                             WHERE USER_GUID = @USER_GUID
                               AND FUNC_ID = 'Superior' -- 如果登入者是部門主管
                               AND @IS_SHOW_SUB = '1')) -- 而且要顯示子部門
                     OR TRAVEL_MEN IN (SELECT USER_GUID -- 或者只找自己部門內所有人
                         FROM TB_EB_EMPL_DEP
                        WHERE GROUP_ID IN (SELECT GROUP_ID 
                                FROM TB_EB_EMPL_DEP 
                            WHERE USER_GUID = @USER_GUID)))
                    AND CONVERT(DATE, GETDATE(), 111) BETWEEN CONVERT(DATE, STARTTIME, 111) -- 今天的日期介於 出差單出差起始日期
                             AND CONVERT(DATE, ENDTIME, 111)  -- 和出差單出差結束日期
                    AND TASK_STATUS = 2 -- 同意
                    AND TASK_RESULT = 0 -- 且結案
            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@IS_SHOW_SUB", chkIncludeSubGroup.Checked ? "1" : "0");
            sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", Current.UserGUID);
            try
            {
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dtReturn = ds.Tables[0];
                }
                isShowChecked = (int)ds.Tables[1].Rows[0]["SUPERIOR"] > 0;

            }
            catch (Exception e)
            {
            }
        }

        return dtReturn;
    }
}