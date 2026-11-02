using Ede.Uof.EIP.Organization;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using Ede.Uof.WKF.Design;
using JGlobalLibs.UOF.Org;
using KYTLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using UOFAssist.WKF;


/**
* 修改時間：2021/11/01
* 修改人員：梁夢慈
* 修改項目：
    * 1.新增config參數(IS_SHOWAGENT)判斷是否顯示啟動代理(divSetAgentTitle、divSetAgent)
* 修改原因：
    * 1.新增規格
* 修改位置： 
    * 1.「SetField() -> 代理人顯示開關」中，取得參數(IS_SHOWAGENT)，並重新設定啟動代理的屬性Visible
* **/

/**
* 修改時間：2021/10/15
* 修改人員：梁夢慈
* 修改項目：
    * 1.呼叫飛騰取得假別資訊，傳入起迄日期改為 --> 起:請假日(起) or 當天 ； 迄: 起+天數(config參數-LEV_INFO_RANGE)
* 修改原因：
    * 1.新增規格
* 修改位置： 
    * 1.「RefreshMainData()」中，依請假日起(LEVDATE_START) & 請假日起+參數天數 的日期，格式為"yyyy/MM/dd"，做為呼叫飛騰請假資訊的起迄日期
* **/

/**
* 修改時間：2021/10/14
* 修改人員：高常昇
* 修改項目：
    * 1. 前端網頁css的class
* 修改原因：
    * 1. 手機板畫面標題不在，原因是 手機板 的架構與電腦版相差大，需要精準的指定
* 修改位置： 
    * 1.「前端網頁」中，「table .gv」改成「table.gv」
* **/

/**
* 修改時間：2021/10/14
* 修改人員：莊仁豪
* 修改項目：
    * 1. 參數「DEFAULT_DAYOFF_AGENT」」(請假單代理人預設自動帶入第一代理人)為Y時，啟動代理不需要預設勾選。
* 修改原因：
    * 1. (新需求)當該參數為Y時，原有預設勾選啟動代理。但因壹東需求，調整為不需要預設勾選啟動代理。
* 修改位置： 
    * 1.「SetField」中，參數設定中(DEFAULT_DAYOFF_AGENT.ToUpper() == "Y")時，原(kchkAGENT)=true拿掉，不預設勾選啟動代理。
* **/

/**
* 修改時間：2021/10/05
* 修改人員：梁夢慈
* 修改項目：
    * 1.請假時間起迄前端變更事件，新增判斷當下的按鈕是否是符合觸發方法本身的按鈕
    * 2.刪除2021/10/01新增之規則
    * 3.呼叫飛騰取得假別資訊，傳入起迄日期改為: 例: 請假日(起) 2021/10/04  當年度 2021/1/1 ~ 隔年2022/12/31 
* 修改原因：
    * 1.BUG修正，手機版按下"送單"後會去觸發此事件，導致清空請假天數&時數猜測原因UOF將手機版執行的事件與電腦版不符
    * 2.修正第1點項目後，就不需要第2點了
    * 3.新增規格
* 修改位置： 
    * 1.「前端網頁 -> kdtpSTARTTIME_Selected()、kdtpENDTIME_Selected()」中，取得當下事件按鈕ID「var _target = $(event.target)」，新增判斷條件再進入方法「if ($(_target).prop('id') == _kdtpENDTIME) 」
    * 2.「btnCal_Click() -> 於最初行」中，註解此程式「新增判斷式檢查請假說明(ktxtREMARK)是否有值，無則return」
    * 3.「RefreshMainData()」中，依請假日起(LEVDATE_START)，取得當年度1/1 & 次年度12/31 的日期，格式為"yyyy/MM/dd"，做為呼叫飛騰請假資訊的起迄日期
* **/

/**
* 修改時間：2021/10/01
* 修改人員：梁夢慈
* 修改項目：
    * 1.新增按下計算按鈕(btnCal)前，檢查請假說明(ktxtREMARK)是否有值，無則直接跳出不進入計算
* 修改原因：
    * 1.BUG修正，因電腦版不會清旗標(呼叫飛騰是否計算成功)，手機版會清，導致送單錯誤
* 修改位置： 
    * 1.「btnCal_Click() -> 於最初行」中，新增判斷式檢查請假說明(ktxtREMARK)是否有值，無則return
* **/

/**
* 修改時間：2021/09/27
* 修改人員：梁夢慈
* 修改項目：
    * 1.新增起迄時間變更時檢查時間範圍日否正確("起"必須小於"迄")，不正確顯示「迄止時間小於起始時間」
* 修改原因：
    * 1.新增規格，因應客戶新需求
* 修改位置： 
    * 1.「SetField() -> // 起單或退回申請者、kdtpSTARTTIME_TextChanged()、kdtpENDTIME_TextChanged()」中，新增三元運算子判斷方法「CheckTimeRange()」，如成立就回寫訊息"迄止時間小於起始時間"
* **/

/**
* 修改時間：2021/09/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.檢查按鈕(btnCal)，文字左方新增紅色*
* 修改原因：
    * 1.新增規格，因應客戶新需求
* 修改位置： 
    * 1.「前端網頁 -> CSS語法」中，新增語法「a.star:before、.star」，創造偽元素； 
    *   「前端網頁 -> btnCal按鈕」中，新增CSS「star」，並將按鈕標籤原為:"asp:Button" -> 改為"asp:LinkButton" (因偽元素只能使用在非Input標籤)
* **/

/**
* 修改時間：2021/09/13
* 修改人員：梁夢慈
* 修改項目：
    * 1.選人元件(btnLEAAGENT)重新綁訂時，需再次設定屬性(isSearchSelfGroupUsers)
* 修改原因：
    * 1.BUG修正，因選人元件屬性(isSearchSelfGroupUsers)，如果重新導向時，沒有再次設定會是預設值(true)
* 修改位置： 
    * 1.「SetField() -> // 網頁首次載入 -> // 使用UOF代理人設定("AGENT") -> 當有找到代理人」，新增屬性設定「btnLEAAGENT.isSearchSelfGroupUsers = false;」
* **/

/**
* 修改時間：2021/09/03
* 修改人員：梁夢慈
* 修改項目：
    * 1.呼叫飛騰檢查/申請[請假單]給予的參數，更改參數順序，將TMP_AGENTID移到時間欄位的後面
* 修改原因：
    * 1.BUG修正，因飛騰端程式處理順序，讀取時間欄位的值，會飛騰系統端會重新抓取代理人(取預設值:第一代理人)
* 修改位置： 
    * 1.「btnCal_Click()、checkVal()、CheckSignLevel()」，將TMP_AGENTID移到時間欄位的後面
* **/

/**
* 修改時間：2021/08/16
* 修改人員：梁夢慈
* 修改項目：
    * 1.請假時間(起)(迄)改變時，移除「呼叫飛騰取得請假資訊」
    * 2.選假別呼叫WS取得假別資訊傳入參數更改，改為:預設帶入，起:當天、迄:當天+一個月
* 修改原因：
    * 1.修改規格，因呼叫飛騰WS非常耗時
    * 2.新增規格
* 修改位置： 
    * 1.「kdtpSTARTTIME_TextChanged()、kdtpENDTIME_TextChanged()」，註解「kddlLEACODE_SelectedIndexChanged(kddlLEACODE, null);」
    * 2.「kddlLEACODE_SelectedIndexChanged() -> if (SCSHRConfiguration.IS_LEAVE_DETAIL == "Y") -> RefreshMainData()」，傳入LEVDATE_START、LEVDATE_END，改為:預設帶入，起:當天、迄:當天+一個月
* **/

/**
* 修改時間：2021/07/22
* 修改人員：莊仁豪
* 修改項目：
    * 1.跳窗帶回的資料顯示「姓名 (帳號)」。
* 修改原因：
    * 1.設定DEFAULT_DAYOFF_AGENT = Y參數，帶入的第一代理人有顯示帳號(姓名)，但使用跳窗帶入的只顯示姓名，需統一。
* 修改位置： 
    * 1.「btnLEAAGENT_DialogReturn()」，ktxtLEAAGENT.Text = KUser.Name 改為 → KUser.Name + " (" + KUser.Account  + ")"; 
* **/


/**
* 修改時間：2021/07/21
* 修改人員：莊仁豪
* 修改項目：
    * 1.代理人欄位顯示「姓名 (帳號)」。
* 修改原因：
    * 1.新需求。
* 修改位置： 
    * 1.「SetField」中，查表(TB_EB_USER)增加查詢(ACCOUNT)。查詢結果放入括號內顯示 -> NAME (ACCOUNT)。 
* **/

/**
* 修改時間：2021/07/20
* 修改人員：莊仁豪
* 修改項目：
    * 1.參數設定增加「DEFAULT_DAYOFF_AGENT」(請假單代理人預設自動帶入第一代理人)。請假單判斷該參數設定是否為Y，當為Y時則自動帶入第一筆代理人，若查不到則不需帶入，同時啟動代理需勾選。
* 修改原因：
    * 1.新需求。
* 修改位置： 
    * 1.「SetField」中，增加判斷，參數設定中(DEFAULT_DAYOFF_AGENT.ToUpper() == "Y")，若為Y則取得申請人的GUID並於(TB_EB_USER_AGENT-USER_GUID)找到代理人(TB_EB_USER_AGENT-AGENT_USER)，並取出第一筆代理人。存放至(hidLEAAGENT)
    * 2.「SetField」中，1.取得代理人後，再用上述取得的(TB_EB_USER_AGENT-AGENT_USER)代理人的GUID查(TB_EB_USER-USER_GUID)取出代理人姓名(NAME)。存放至(ktxtLEAAGENT)
    * 3.「SetField」中，當上述1.條件成立時，kchkAGENT.Checked = true。啟動代理checkbox = true勾選。
* **/

/**
* 修改時間：2021/06/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.所有有使用EBUser的地方，都改為呼叫通用方法取得人員資訊
    * 2.呼叫飛騰，傳入參數-TMP_EMPLOYEEID、TMP_AGENTID，原給「ACCOUNT」 -> 改給「EMPLOYEEID」
* 修改原因：
    * 1.修改規格，UOF的EBUser有時候會異常取不到人員資訊，以防再多花時間去查明原因，改為通用方法直接查SQL方式取得人員資訊
    * 2.規格修正，飛騰目前都改為固定傳入「EMPLOYEEID」
* 修改位置： 
    * 2.「btnCal_Click()、checkVal()、CheckSignLevel()」，傳入TMP_EMPLOYEEID、TMP_AGENTID，的值 -> 改傳入EmployeeNo
* **/

/**
* 修改時間：2021/04/27
* 修改人員：梁夢慈
* 修改項目：
    * 1. 取得假別資訊前，新增判斷式，檢查設定值如果="Y"，才進入方法去呼叫飛騰
* 修改原因：
    * 1. 新增規格
* 修改位置：
    * 「kddlLEACODE_SelectedIndexChanged()」中，於呼叫RefreshMainData前，新增判斷式「SCSHRConfiguration.IS_LEAVE_DETAIL == "Y"」
* **/

/**
* 修改時間：2021/04/20
* 修改人員：梁夢慈
* 修改項目：
    * 1. 取得假別資訊，當為「一般假」時，明細-假別，內容值為回傳DataTable欄位"SVacationName"，「特殊假」保持原規則不變
* 修改原因：
    * 1. 新增規格
* 修改位置：
    * 「RefreshMainData()」中，取得假別資訊回傳果後，新增判斷式當為「一般假」時，"LEVNAME"內容值，為回傳的"SVacationName"內容值
* **/

/**
* 修改時間：2021/04/14
* 修改人員：莊仁豪
* 修改項目：
    * 1. 請假人選擇按鈕(btnLEAEMP)隱藏不顯示。
* 修改原因：
    * 1. 台北旅店反應請假人都是申請者，不應該可以選所有人。(不合理)
* 修改位置：
    * 1. 「SetField」中，新增btnLEAEMP.Visible = false，將請假人按鈕隱藏。
* **/

/**
* 修改時間：2021/03/31
* 修改人員：梁夢慈
* 修改項目：
    * 1. 請假人(ktxtLEAEMP)改為選人元件，可選所有人
* 修改原因：
    * 1. 新增規格
* 修改位置：
    * 「前端網頁」中，於申請人欄位後面新增一個選人元件(btnLEAEMP)
* **/

/**
* 修改時間：2021/03/29
* 修改人員：梁夢慈
* 修改項目：
    * 1. 取假別(非特別假)餘額的FundID改為GetEmpAllLRData(原為CheckLeaveLRData)
* 修改原因：
    * 1. 飛騰端修改傳入ID，不知道他們為什麼要改，反正就是改了
* 修改位置：
    * 「RefreshMainData」中，當不是特別假，funcId給予"GetEmpAllLRData"
* **/

/**
* 修改時間：2021/02/05
* 修改人員：陳緯榕
* 修改項目：
    * 送給飛騰檢查時，代理人為空就給admin
* 修改原因：
    * 為了解決飛騰API BUG，造成呼叫API後的瓶頸過度嚴重，所以用這種不太正確的修正方式處理
* 修改位置：
    * 「checkVal」中，〈TMP_AGENTID〉如果來源是空的，就給admin，用來讓飛騰API加速處理
    * 「CheckSignLevel」中，〈TMP_AGENTID〉如果來源是空的，就給admin，用來讓飛騰API加速處理
    * 「btnCal_Click」中，〈TMP_AGENTID〉如果來源是空的，就給admin，用來讓飛騰API加速處理
* **/

/**
* 修改時間：2020/10/15
* 修改人員：陳緯榕
* 修改項目：
    * 請假單的代理人區塊的顯示由config來控制
* 發生原因：
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈代理人〉設定id為〈divAgentTitle〉、同一組的div區塊，設定id為〈divAgent〉；〈啟動代理〉設定id為〈divSetAgentTitle〉、同一組的div區塊，設定id為〈divSetAgent〉
    * 「SetField」中，當〈SCSHRConfiguration.IS_LEV_AGENT_SHOW〉是〈Y〉時，將〈divAgentTitle〉、〈divAgent〉、〈divSetAgentTitle〉、〈divSetAgent〉的屬性〈Visible〉設為〈false〉
* **/

/**
* 修改時間：2020/10/14
* 修改人員：陳緯榕
* 修改項目：
    * 迄止時間不可小於開始時間
* 修改原因：
    * 飛騰竟然能接受這個，雖然說要改成當迄止小於開始，時間對調，但是不能相信飛騰
* 修改位置：
    * 「CheckVal」中，當〈迄止時間大於開始時間時〉才處理原本的邏輯，所以當〈迄止時間小於開始時間〉時，輸出錯誤的JObject〈STARTTIME_BIGGER〉
    * 「前端網頁」「CheckWork」中，詢問完〈CheckVal〉後，新增處理〈STARTTIME_BIGGER〉的錯誤訊息
* **/

/**
* 修改時間：2020/08/28
* 修改人員：陳緯榕
* 修改項目：
    * 「SCSHRConfiguration.LEV_TYPE_FILTER」過濾假別時，要用「Equals」來比對「SYS_VIEWID」
* 發生原因：
    * 在「config」的「SCSHRConfiguration.LEV_TYPE_FILTER」設定了假別號碼，但設定了「15」會把「151」、「152」也過濾掉
* 修改位置：
    * 「getLEAVEType」中，過濾假別時，比對〈SYS_VIEWID〉要從〈Contains〉改用〈Equals〉
* **/

/**
* 修改時間：2020/04/30
* 修改人員：陳緯榕
* 修改項目：
    * 代理人按鈕，當設定檔〈SCSHRConfiguration.AGENT_FUNC〉設為〈AGENT〉時，需要判斷〈TB_EB_USER_AGENT〉是否沒資料，沒資料時，查詢現在所選部門的部門人員
* 發生原因：
    * 展基的時候有這個功能，但換成新的選人元件後就沒了，但是查詢同部門人員用的語法長度過長，必須修改常昇自製版的選人元件
* 修改位置：
    * 「SetField」中，當設定檔〈SCSHRConfiguration.AGENT_FUNC〉設為〈AGENT〉時，使用SQL查詢〈TB_EB_USER_AGENT〉(UOF個人設定→設定→設定代理人)是否有設定代理人員，
        * 有的話人員的來源為〈TB_EB_USER_AGENT〉
        * 查不到設定時，選人元件〈_UC_SelectUserFilterGroup：btnLEAAGENT〉
            * 屬性〈GroupID〉設為現在的代理人的部門；
            * 屬性〈isSearchSelfGroupUsers〉給予〈true〉
* **/

/**
* 修改時間：2020/04/16
* 修改人員：陳緯榕
* 修改項目：
    * 新規格：標題修改
* 發生原因：
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈SCSHR請假資訊〉改為〈請假資訊〉
* **/

/**
* 修改時間：2020/03/26
* 修改人員：陳緯榕
* 修改項目：
    * 申請請假單時，部分假別需要有性別的控制管理，「ATT0011400」中取得「TMP_LIMITSEXAPPLY」判斷該假別判斷能夠申請的性別(0是不限，1是男性，2是女性)
* 發生原因：
    * 先前判斷固定代碼，而所有客戶的假別代碼都不同，所以判斷的結果會是錯的，最終改成呼叫飛騰子假別(ATT0011400)，取得欄位「TMP_LIMITSEXAPPLY」
* 修改位置：
    * 「前端網頁」中，新增〈HiddenField：hidSEXAPPLY〉，用來儲存現在所選的假別的需求性別
    * 「前端網頁」「CheckWork(sender, args)」中，呼叫〈checkVal〉時，新增最後一個參數〈hidSEXAPPLY.val()〉，並且在回傳的json中取出〈SEXAPPLY〉判斷是否有擋單
    * 「checkIsSpecDate」中，新增參數〈out string SEXAPPLY〉，並且在巡覽資料表時，取出〈TMP_LIMITSEXAPPLY〉回填到〈SEXAPPLY〉
    * 「kddlLEACODE_SelectedIndexChanged」中，將呼叫〈kddlLEACODE_SelectedIndexChanged〉取出的〈SEXAPPLY〉填入〈hidSEXAPPLY.Value〉中；同時清除按下檢查後的訊息〈lblAPIResultError〉
    * 「checkVal」中，新增參數〈string SEXAPPLY〉
    * 「checkVal」中，當〈SEX〉性別的值是男性〈M〉而且〈SEXAPPLY〉的值是〈2〉時阻擋送簽，並顯示訊息〈男性不能申請〉
    * 「checkVal」中，當〈SEX〉性別的值是女性〈F〉而且〈SEXAPPLY〉的值是〈1〉時阻擋送簽，並顯示訊息〈女性不能申請〉
* **/

/**
* 修改時間：2020/03/20
* 修改人員：陳緯榕
* 修改項目：
    * 無印反應公傷假出現了「男性不能申請」的問題
* 發生原因：
    * 開發時針對的客戶，其代碼06是生理假、08是產假；而現在確定了，各家客戶代碼不同，所以性別限制全部拿掉
* 修改位置：
    * 「前端網頁」「CheckWork」中，判斷〈LEA_SEX〉是男性的假別限制註解掉
* **/

/**
* 修改時間：2020/03/13
* 修改人員：陳緯榕
* 修改項目：
    * kxtMessage顯示需為紅色
* 發生原因：
    * ，紅色是屬於警告意味強的顏色，故使用紅色顯示錯誤
* 修改位置：
    * 「前端網頁」中，新增〈CSS：msgColor〉；並將〈ktxtMessage〉的屬性〈LabelCssClass〉設為〈msgColor〉
* **/

/**
* 修改時間：2020/03/10
* 修改人員：陳緯榕
* 修改項目：
    * 當假別資訊查無資料時，不顯示
* 發生原因：
    * 修改規格
* 修改位置：
    * 「RefreshMainData」中，當〈gvMain〉綁定完成後，判斷資料數量是否大於0，大於0顯示gvMain
* **/

/**
* 修改時間：2020/02/06
* 修改人員：陳緯榕
* 修改項目：
    * 時間重疊到的兩張單，先申請的那張單在簽核中出現錯誤
* 發生原因：
    * 推測是前一張單的起單事件有錯誤，所以沒有呼叫到飛騰，而後一張單則是起單時成功寫入飛騰，所以前一張單才被當作錯誤；
    * 發生的原因是因為「2019/09/25」的前端檢查，不檢查中繼表的重複性，而且沒有改變「沒有在中繼表找到重複表單就去詢問飛騰是否有重複」的邏輯，造成只要有重複表單，表單都一定『能過』
* 修改位置：
    * 「CheckVal」中，SQL檢查是否在中繼表有重複的語法〈不刪除〉，因為不把程式做重大的更動；所以現在只改成〈必定〉檢查飛騰
* **/

/**
* 修改時間：2020/01/20
* 修改人員：陳緯榕
* 修改項目：
    * 無印反應 請假單：先填寫特殊日後，再去填寫請假日期後，特殊日期會變成空白
* 發生原因：
    * 觸發「請假時間(起)」的後端事件會觸發假別的後端事件，而當假別視特殊假時，清空特殊日期
* 修改位置：
    * 「kddlLEACODE_SelectedIndexChanged」中，當〈現在是特別假〉時，〈kdpSP_DATE〉不要清空
* **/

/**
* 修改時間：2019/12/23
* 修改人員：陳緯榕
* 修改項目：
    * 代理人選擇畫面錯誤
* 發生原因：
    * config設定的「AGENT_FUNC」設為「ALL」，但是應該要用另一個選人元件「_UC_SearchGroupWithGroup」
* 修改位置：
    * 「前端網頁」中，新增〈uc1:_UC_SearchGroupWithGroup：btnLEAAGENT_Group〉
    * 「SetField」中，先隱藏〈btnLEAAGENT〉、〈btnLEAAGENT_Group〉物件，當〈SCSHRConfiguration.AGENT_FUNC〉為〈AGENT〉時，顯示〈btnLEAAGENT〉；當為〈ALL〉時，顯示〈btnLEAAGENT_Group〉
    * 「SetField」中，〈btnLEAAGENT_Group〉的〈UserSql〉使用預設值
* **/

/**
* 修改時間：2019/12/20
* 修改人員：陳緯榕
* 修改項目：
    * 沒有代理人可選
* 發生原因：
    * config設定的「AGENT_FUNC」設為「ALL」，但是在〈2019/02/15〉寫的時候是使用一等一的選人元件，所以預設是選擇所有人；而現在使用的選人是沒有預設的
* 修改位置：
    * 「SetField」中，當〈SCSHR.SCSHRConfiguration.AGENT_FUNC.ToUpper() == "ALL"〉時，〈btnLEAAGENT〉的屬性〈UserSql〉直接設為〈TB_EB_USER〉所有人
* **/

/**
* 修改時間：2019/12/13
* 修改人員：陳緯榕
* 修改項目：
    * 日期起訖改要用8碼
* 發生原因：
    * 改規格?
* 修改位置：
    * 「RefreshMainData」中，〈getPatameters〉所需要的參數〈StartDate〉、〈EndDate〉改用〈8碼〉的日期格式
* **/

/**
* 修改時間：2019/11/27
* 修改人員：陳緯榕
* 修改項目：
    * checkIsSpecDate方法會用到的變數命名成「dtSource」出錯
* 發生原因：
    * 1303行(當時的行號)，A local variable named 'dtSource' is already defined in this scope
* 修改位置：
    * 「kddlLEACODE_SelectedIndexChanged」中，準備呼叫〈checkIsSpecDate〉用的第一個參數的變數名稱由〈dtSource〉改為〈dtLEASource〉
    * 「btnCal_Click」中，準備呼叫〈checkIsSpecDate〉用的第一個參數的變數名稱由〈dtSource〉改為〈dtLEASource〉
* **/

/**
* 修改時間：2019/11/25
* 修改人員：陳緯榕
* 修改項目：
    * 計算時，判斷特別假的方式修正
* 發生原因：
    * 特別假的判斷不應該寫死
* 修改位置：
    * 新增方法「checkIsSpecDate(DataTable dtSource, string selectedValue, out string needAttach)」檢查是否是特別假
    * 「kddlLEACODE_SelectedIndexChanged」中，呼叫〈checkIsSpecDate〉，用來檢查是否是特別假和是否需要〈上傳附件〉
    * 「btnCal_Click」中，呼叫〈checkIsSpecDate〉，用來檢查是否是特別假
    * 附註：此方法會比較占用記憶體，但可以避免新舊單中的物件差異問題和經由全域變數儲存的生命週期問題
* **/

/**
* 修改時間：2019/11/22
* 修改人員：JAY
* 修改項目：
    * 增加是否有選代理人的條件has_agent
* 發生原因：
    * 新規格
* 修改位置：
    * 「ConditionValue」中，新增〈has_agent〉，使用〈ktxtLEAAGENT〉是否為空當條件
* **/

/**
* 修改時間：2019/10/24
* 修改人員：陳緯榕
* 修改項目：
    * 當沒有選擇代理人，並且簽核關卡送出時，會跳出alert-undefined
    * 可休假餘額改由前端直接顯示，因為只有一筆，當初規劃時是當作HCP那樣使用
* 發生原因：
    * $uof的呼叫方式是ajax，它觸發了「fail」區域，所以跳出alert，但又無法解析「jqXhr.responseText」所以才會是〈undefined〉
    * BOImport使用沒問題，但是卻是觸發fail的原因?
    * ***********
    * 更改規格
* 修改位置：
    * 「前端網頁」中，所有的註解都刪除
    * 「CheckSignLevel」中，〈service.BOImport〉這一行註解，暫時放棄簽核時檢查請假單是否重複
    * ************
    * 「前端網頁」中，〈請假說明〉區塊下面新增〈asp:GridView：gvMain〉，用來顯示可休假餘額
    * 「前端網頁」中，刪除〈btnCANLEAS〉可休假餘額
    * 新增方法「RefreshMainData(string EMPID, string LEVCODE, string LEVNAME, string LEVDATE_START, string LEVDATE_END, string IS_SPEC)」
    * 「kddlLEACODE_SelectedIndexChanged」中，選完假別後，呼叫〈RefreshMainData〉
* **/

/**
* 修改時間：2019/10/15
* 修改人員：陳緯榕
* 修改項目：
    * 按下計算出錯《System.NullReferenceException: 並未將物件參考設定為物件的執行個體。
                    於 JGlobalLibs.SQLUtils.jsonToTable(JArray jArray》
* 發生原因：
    * 代理人的帳號回傳NULL
* 修改位置：
    * 「btnCal_Click」中，〈AGENTID〉填入前，判斷帳號是否是NULL，是的話給空字串
* **/

/**
* 修改時間：2019/09/30
* 修改人員：陳緯榕
* 修改項目：
    * 選人元件選完之後沒出現代理人
    * 選人元件按下第二次就會出錯
* 發生原因：
    * 沒顯示代理人
    * ********
    * 選人元件有錯
* 修改位置：
    * 「SetField」中，不隱藏〈ktxtLEAAGENT〉
    * 「SetField」中，〈btnLEAAGENT.UserSql〉的語法必須包含〈ACCOUNT〉、〈GROUP_ID〉、〈NAME〉、〈GUID〉
* **/

/**
* 修改時間：2019/09/30
* 修改人員：陳緯榕
* 修改項目：
    * 選人元件改用常昇開發的半完成品選人元件(並非元件，只是網頁，需要路徑接正確才能使用)
* 發生原因：
    * 一等一的選人元件有問題，依照他們提供的方法，在13版測試沒問題，但是現在卻一直出錯，所以直接棄用
* 修改位置：
    * 「前端網頁」中，新增〈_UC_SelectUserFilterGroup:btnLEAAGENT〉
    * 「前端網頁」中，刪除〈UC_ChoiceListMobile:btnMobLEAAGENT〉、〈RadButton:btnLEAAGENT〉、〈UC_ChoiceList:ubtnLEAAGENT〉、〈HiddenField:hidIsMobileUI〉物件
    * 「前端網頁」中，移除方法〈btnLEAAGENT_OnClientClicked〉
    * 「前端網頁」「CheckLEAANDAPP」中，去除〈hidIsMobileUI〉、〈btnMobLEAAGENT〉的檢查
    * 「前端網頁」「CheckWork」中，去除〈hidIsMobileUI〉、〈btnMobLEAAGENT〉的檢查
    * 「FieldValue」中，額外處理的內容都刪除
    * 「SetField」中，〈hidIsMobileUI〉、〈btnMobLEAAGENT〉相關的邏輯全部刪除
    * 「SetField」中，〈使用UOF代理人設定〉的判斷中，〈btnLEAAGENT〉給予屬性〈UserSql〉，內容給予查詢〈TB_EB_USER_AGENT〉並且直接給〈hidLEAEMP〉當作USER_GUID
    * 刪除方法「ubtnLEAAGENT_EditButtonOnClick(UserSet userSet)」
    * 「btnCal_Click」中，〈TMP_AGENTID〉內容直接取〈hidLEAAGENT〉的ACCOUNT
    * 新增方法「btnLEAAGENT_DialogReturn(object sender, string result)」
* **/

/**
* 修改時間：2019/09/25
* 修改人員：高常昇
* 修改項目：
    * 不用檢查重複的請假日期
* 發生原因：
    * 飛騰那邊已經銷假，但我們這邊仍然檔案單
* 修改位置：
    * 「checkVal」中，當日期重複則Mark掉 --暫不使用
* **/

/**
* 修改時間：2019/07/17
* 修改人員：陳緯榕
* 修改項目：
    * KYTDateTimePicker在dll.Config設定的IS_PICKER_READONLY設定無效
* 發生原因：
    * KYTPicker要設定TextBoxReadOnly的特性是ViewType.Input，但原本的位置狀態會是ViewType.ReadOnly
* 修改位置：
    * 「SetField」中，當〈!Page.IsPostBack(開啟表單)〉而且〈fieldOptional.FieldMode〉是〈Applicant〉或〈ReturnApplicant〉時，程式片段的最後設定〈TextBoxReadOnly〉屬性
    * 「SetField」中，當〈Page.IsPostBack(操作表單)〉而且〈fieldOptional.FieldMode〉是〈Applicant〉或〈ReturnApplicant〉時，設定〈ViewType = Input〉和〈TextBoxReadOnly〉屬性
* **/

/**
* 修改時間：2019/07/01
* 修改人員：陳緯榕
* 修改項目：
    * 請假單退回後，附件檢查無法通過
* 修改位置：
    * 「checkVal」中，當〈取回或退回申請者起單時有上傳個人附件〉，回傳〈0〉
* **/

/**
* 修改時間：2019/06/27
* 修改人員：陳緯榕
* 修改項目：
    * 增加：當時間(起)的日期更動時，同時變更時間(迄)的日期；更動時間時不處理
* 修改位置：
    * 「kdtpSTARTTIME_TextChanged」中，當〈SCSHRConfiguration.IS_COPY_TIMESTART_DATE_TO_TIMEEND_DATE〉的值為〈Y〉時，判斷〈如果上次的日期不等於現在的日期，更動迄止時間〉是否成立，成立時修改時間(迄)的日期
* **/

/**
* 修改時間：2019/06/06
* 修改人員：陳緯榕
* 修改項目：
    * 按下計算後，更改假別能繼續送出
    * 簽核時出現沒有代理人的錯誤
* 發生原因：
    * *********
    * 呼叫WS沒有送代理人
* 修改位置：
    * 「kddlLEACODE_SelectedIndexChanged」中，清空〈hidAPIResult〉
    * **********
    * 「btnCal_Click」中，〈TMP_AGENTID〉還要判斷當〈hidLEAAGENT〉為空時，給予〈btnMobLEAAGENT.UserSet.Items[0].Key〉
    * 「checkVal」中，新增參數〈string AGENT_GUID〉
    * 「checkVal」中，WS要給予〈TMP_AGENTID〉，來源是〈Z_SCSHR_LEAVE-AGENT_GUID〉
    * 「CheckSignLevel」中，WS要給予〈TMP_AGENTID〉，來源是〈LEAAGENT〉
    * 「前端網頁」「CheckWork」中，取得〈hidLEAAGENT〉、〈hidIsMobileUI〉、〈btnMobLEAAGENT〉
    * 「前端網頁」「CheckWork」中，當〈hidIsMobileUI〉是〈Y〉時，從〈btnMobLEAAGENT.val().split(',')[0].split('|')[1]〉取值
    * 「前端網頁」「CheckWork」中，呼叫WS〈checkVal〉給予最後一個參數〈Agent_ID〉
* **/

/**
* 修改時間：2019/06/03
* 修改人員：陳緯榕
* 修改項目：
    * 檢查是否有上傳附件結果錯誤
* 發生原因：
    * 使用相關附件會在簽核中被刪除，而且查詢結果錯誤
* 修改位置：
    * 「checkVal」中，在SQL中檢查，只要不是〈0〉就是沒有上傳附件
        * 在『沒有單號』時，
            * 使用〈SCRIPT_ID〉檢查〈TB_WKF_SCRIPT-PERSONAL_ATTACH_ID〉
            * 然後使用〈SCRIPT_ID〉檢查〈TB_EB_FILE_STORE〉，條件是〈DELETED <> 1〉
        * 在『有單號』時，
            * 使用〈DOC_NBR〉檢查〈TB_WKF_TASK_NODE-ATTACH_ID〉，條件是〈NODE_STATUS = 4〉
            * 然後再用〈DOC_NBR〉檢查〈TB_WKF_TASK_NODE-ATTACH_ID〉，條件是〈NODE_STATUS = 4〉和〈ATTACH_ID != '' OR ATTACH_ID IS NOT NULL〉
            * 最後使用〈DOC_NBR〉檢查〈TB_WKF_TASK-TASK_ID - DOC_NBR = TASK_ID〉→〈TB_WKF_TASK_NODE-TASK_ID = ATTACH_ID〉→〈TB_EB_FILE_STORE-FILE_GROUP_ID〉，條件是〈DELETED <> 1〉
* **/

/**
* 修改時間：2019/05/08
* 修改人員：陳緯榕
* 修改項目：
    * KYTPicker在Postback後無法再手動輸入
* 修改位置：
    * 「前端網頁」中，將KYTPicker的屬性〈TextBoxReadOnly〉去除
    * 「SetField」中，將KYTPicker的〈TextBoxReadOnly〉放到〈網頁首次載入〉之外，讓其每次都執行
* **/

/**
* 修改時間：2019/05/06
* 修改人員：陳緯榕
* 修改項目：
    * 有特殊假簽核無法過
* 發生原因：
    * 送單檢查的特別日期格是有問題
* 修改位置：
    * 「CheckSignLevel」中，〈SPECIALDATE〉的格式改為〈yyyy/MM/dd〉
* **/

/**
* 修改時間：2019/04/30
* 修改人員：陳緯榕
* 修改項目：
    * 手機填單後，不管在電腦還是手機簽核，代理人都會消失
    * 預防性的處理送飛騰用的「帳號」
* 發生原因：
    * FieldValue會被覆寫，因為起單之外的關卡，hidIsMobileUI只會在起單關管理
    * ******
    * 如果使用EBUser取帳號，會有domain在前面
* 修改位置：
    * 「FieldValue」中，處理〈btnMobLEAAGENT〉填入的值之前，要先判斷是否是MobileUI還有代理人輸入框是否還在
    * 「SetField」中，將〈hidIsMobileUI〉的判斷拉到表單狀態的判斷外
    * **********
    * 「CheckSignLevel」中，WS參數〈TMP_EMPLOYEEID〉所需要的帳號使用〈JGlobalLibs.UOFUtils.getUserAccount〉來取得
* **/

/**
* 修改時間：2019/04/29
* 修改人員：陳緯榕
* 修改項目：
    * 簽核的時候檢查
* 發生原因：
    * 新功能：送單前檢查
* 修改位置：
    * 新增「[WebMethod]CheckSignLevel(string DOC_NBR)」方法
    * 「前端網頁」「CheckVal」中，當現在是〈起單之外的狀態檢查事項〉，呼叫〈CheckSignLevel〉檢查
    * 「SetField」中，簽核狀態時，記住現在的單號
* **/

/**
* 修改時間：2019/04/09
* 修改人員：陳緯榕
* 修改項目：
    * DateTimePicker的輸入框是否能輸入由dll.config控制
* 修改位置：
    * 「SetField」中，當〈!Page.IsPostBack〉時，〈kdtpSTARTTIME〉和〈kdtpENDTIME〉的屬性〈TextBoxReadOnly〉由〈SCSHRConfiguration.IS_PICKER_READONLY〉控制
* **/

/**
* 修改時間：2019/04/02
* 修改人員：陳緯榕
* 修改項目：
    * 請假時間按下送出檢查都會出錯
    * 請假單不檢查是否有填寫代理人
    * 只管理請假時間(迄)是否在限定範圍內
* 修改位置：
    * 「checkVal」中，〈dtStartLimit〉、〈dtOffLimit〉必須是yyyy/MM/dd去組合字串，而不是完整DateTime.Now
    * 「checkVal」中，〈dtStartLimit〉使用〈-Pre〉、〈dtOffLimit〉使用〈Next〉
    * 「checkVal」中，判斷式為〈dtSTART_TIME < dtStartLimit〉、〈dtEND_TIME > dtOffLimit〉
    * *********
    * 「前端網頁」中，將〈CustomValidator4〉、〈CheckLEAAGENT〉註解
    * *********
    * 「checkVal」中，判斷式改為〈dtEND_TIME < dtStartLimit〉
* **/

/**
* 修改時間：2019/04/01
* 修改人員：陳緯榕
* 修改項目：
    * 請假時間要能阻擋起訖區間
* 修改位置：
    * 「checkVal」中，當旗標〈NEED_BLOCK_LEV_LIMIT_TIME〉為〈Y〉時，要檢查起訖區間
    * 「checkVal」中，當天數旗標〈LEV_PREVIOUS_DAYS〉、〈LEV_NEXT_DAYS〉有設定時，檢查往前和往後的時間
    * 「前端網頁」「CheckWork」中，新增〈START_LIMIT〉、〈OFF_LIMIT〉
* **/

/**
* 修改時間：2019/03/21
* 修改人員：陳緯榕
* 修改項目：
    * 修改錯誤訊息
    * 預設時間錯誤
* 修改位置：
    * 「SetField」中，使用UOF代理人設定的ErrorLog的〈TB_EB_EMPL_DEP〉改為〈TB_EB_USER_AGENT〉
    * ******
    * 「SetField」中，設定預設時間時，預設起始時間不應該塞迄；預設迄止時間不應該塞起
* **/

/**
* 修改時間：2019/03/18
* 修改人員：陳緯榕
* 修改項目：
    * 加上預設時間
    * 觀看事件看不到特殊假
* 修改位置：
    * 「SetField」中，當現在是〈起單狀態〉，從〈SCSHRConfiguration.FORM_DEFAULT_START_TIME〉和〈SCSHRConfiguration.FORM_DEFAULT_END_TIME〉取得預設時間，並填入起訖時間
    * *****
    * 「SetField」中，觀看時，重新綁定假別
* **/

/**
* 修改時間：2019/03/07
* 修改人員：陳緯榕
* 修改項目：
    * 新增功能：開起表單時，需要預先填寫時間
    * 簽核關卡時，請假人消失了
* 發生原因：
    * 簽核時，是使用MOBILE簽核的，這時候btnMobLEAAGENT不會有值，所以人員被寫回空字串
* 修改位置：
    * 「SetField」中，當起單時，組合〈今天日期+SCSHRConfiguration.LEV_DEFAULT_TIME〉，填入請假起迄
    * **********
    * 「SetField」中，〈hidIsMobileUI〉只在起單關使用
* **/

/**
* 修改時間：2019/02/15
* 修改人員：陳緯榕
* 修改項目：
    * 代理人需要能控制是否使用UOF代理人管理
    * 開啟表單時，過濾的假別要能通用
* 修改位置：
    * 「SetField」中，判斷〈SCSHRConfiguration.AGENT_FUNC〉是否是〈AGENT〉，查詢〈TB_EB_USER_AGENT〉取得代理人資料，並且〈LimitChoice〉設定為〈WithOutUserDept〉
    * 「ubtnLEAAGENT_EditButtonOnClick」中，沒有選擇人員時，清空顯示
    * *******
    * 「getLEAVEType」中，先取出〈SCSHRConfiguration.LEV_TYPE_FILTER〉設定，再用〈,〉切割，用〈LINQ〉判斷〈SYS_VIEWID〉是否存在於設定中，存在就過濾該假別
* **/

/**
* 修改時間：2019/02/12
* 修改人員：陳緯榕
* 修改項目：
    * 代理人簽核時會消失
* 修改位置：
    * 「SetField」中，將〈ktxtLEAAGENT〉的是否隱藏放在起單時
* **/

/**
* 修改時間：2019/01/30
* 修改人員：陳緯榕
* 修改項目：
    * UOF選人元件必須區別一般及MOBILE版
* 修改位置：
    * 「前端網頁」新增物件〈btnMobLEAAGENT〉、〈hidIsMobileUI〉
    * 「前端網頁」「CheckLEAAGENT」中，先取得〈hidIsMobileUI〉是否為〈Y〉，為Y時，檢查〈btnMobLEAAGENT〉是否有內容；為N時，檢查〈hidLEAAGENT〉是否有內容
    * 「前端網頁」「CheckLEAANDAPP」中，先取得〈hidIsMobileUI〉是否為〈Y〉，為Y時，檢查〈btnMobLEAAGENT〉是否和〈hidLEAEMP〉內容一致；為N時，檢查〈hidLEAAGENT〉是否和〈hidLEAEMP〉內容一致
    * 「SetField」中，將〈base.MobileUI〉結果填入〈hidIsMobileUI〉
    * 「SetField」中，先隱藏所有的代理人員物件
    * 「SetField」中，由〈hidIsMobileUI〉判斷現在要顯示哪一種物件
    * 「FieldValue」中，因為〈UC_ChoiceListMobile〉沒有回傳事件，所以當〈hidIsMobileUI〉為〈Y〉時，將代理人員的結果回寫
* **/

/**
* 修改時間：2019/01/29
* 修改人員：陳緯榕
* 修改項目：
    * 檢查完畢後，需出現檢查成功
    * 在表單設計 點選驗證流程 出現錯誤
* 修改位置：
    * 「btnCal_Click」中，當〈hidAPIResult〉為OK時，加上〈lblAPIResultError〉給予〈檢查成功〉
    * *****
    * 「SetField」中，當〈FieldMode〉是〈Verify〉時，不做任何事
* **/

/**
* 修改時間：2019/01/28
* 修改人員：陳緯榕
* 修改項目：
    * 只有起單能改變代理人
* 修改位置：
    * 「SetField」中，開啟表單時，隱藏代理人按鈕〈btnLEAAGENT〉，起單時顯示
* **/

/**
* 修改時間：2019/01/25
* 修改人員：陳緯榕
* 修改項目：
    * 代理人全部開放
    * 加入是否代理申請判斷式
* 修改位置：
    * 「SetField」中，將〈ubtnLEAAGENT.LimitXML〉及取得資料的部分註解
    * 「ConditionValue」中，新增〈agentapply〉，用〈Current.UserGUID != this.ApplicantGuid ? "Y" : "N"〉來判斷
* **/

/**
* 修改時間：2019/01/24
* 修改人員：陳緯榕
* 修改項目：
    * 帳號包含著網域，造成後續許多問題
* 修改位置：
    * 「SetField」中，將user.Account用〈//〉切割後取最右邊，意圖將網域名稱去除
* **/

/**
* 修改時間：2019/01/21
* 修改人員：陳緯榕
* 修改項目：
    * 表單起單就出錯
* 修改位置：
    * 「SetField」中，呼叫WS取得的欄位改用〈*〉，以避免新增欄位卻因為找不到該欄位而出錯的問題
    * 「SetField」中，〈將過期或尚未到達的可用假別去掉〉，C#的「DateTime.TryParse」再來源字串為空字串時，〈out〉所給的值卻是〈0001/1/1〉，所以下一行用回傳的bool來判斷是否要將〈dtEnd〉改為DateTime.MaxValue
* **/

/**
* 修改時間：2019/01/19
* 修改人員：陳緯榕
* 修改項目：
    * 不能出現不能用的假別
* 修改位置：
    * 「SetField」將過期或尚未到達的可用假別去掉
* **/

/**
* 修改時間：2019/01/15
* 修改人員：陳緯榕
* 修改項目：
    * 不需要附件的假別被要求附件
* 修改位置：
    * 「checkVal」判斷〈NEEDATTACH〉全部轉為大寫
* **/

/**
* 修改時間：2018/12/26
* 修改人員：陳緯榕
* 修改項目：
    * 婚假「代理人」不可為空值
* 修改位置：
    * 「btnCal_Click」WS參數〈TMP_AGENTID〉不可給予NULL
* **/

/**
* 修改時間：2018/12/25
* 修改人員：陳緯榕
* 修改項目：
    * 婚假「代理人」不可為空值
* 修改位置：
    * 「btnCal_Click」WS多傳送一個參數〈TMP_AGENTID〉，內容為代理人〈hidLEAAGENT〉(USER_GUID)的帳號
* **/

/**
* 修改時間：2018/12/24
* 修改人員：陳緯榕
* 修改項目：
    * 啟動代理看不到
* 修改位置：
    * 「前端網頁」新增物件〈Label〉〈lblAGENT〉
    * 「SetField」表單列印、Verify、表單觀看、表單簽核，以上要看到啟動代理的訊息
* **/

/**
* 修改時間：2018/12/22
* 修改人員：陳緯榕
* 修改項目：
    * 新功能：啟動代理
* 修改位置：
    * 「前端網頁」新增物件〈KYTCheckBox〉〈kchkAGENT〉
* **/

/**
* 修改時間：2018/12/21
* 修改人員：陳緯榕
* 修改項目：
    * 請假單假別的特別假要參照飛騰上的設定
* 修改位置：
    * 「SetField」綁定假別前，一並呼叫〈ATT0011300〉用來取得〈SFSTARTDATE〉
    * 「SetField」WS〈ATT0011300〉的〈SYS_ID〉的外來鍵是〈ATT0011400〉的〈VACATIONID〉
    * 「SetField」巡覽從WS取得的假別，並將兩者合併，以〈ATT0011400〉為主
    * 「kddlLEACODE_SelectedIndexChanged」巡覽假別的來源資料，取出現在所選的假別是否為特別假，是的話做特別假的處理
* **/

/**
* 修改時間：2018/12/20
* 修改人員：陳緯榕
* 修改項目：
    * 依靠假別判斷是否要上傳附件證明
* 修改位置：
    * 「前端網頁」新增HiddenField物件〈hidNeedAttach〉、〈hidDOC_NBR〉、〈hidFormScriptID〉
    * 「前端網頁」script〈CheckWork〉新增參數〈確認需不需要檢查附件〉、〈表單單號〉、〈表單草稿單號〉
    * 「checkVal」新增參數〈SCRIPTID〉、〈DOC_NBR〉、〈NEEDATTACH〉
    * 「checkVal」SqlCommand查詢是否有上傳附件，需要附件卻沒上傳時，回傳〈nofile〉
    * 「SetField」起單時，給予草稿ID〈hidFormScriptID〉目前的〈Request["scriptId"]〉
    * 「SetField」起單，但屬於退回申請者時，給予表單單號〈hidDOC_NBR〉
    * 「SetField」假別綁定呼叫WS時，ProgId改為動態，並且新增參數〈TMP_NEEDATTACH〉
    * 「SetField」起單時，先紀錄目前假別的值，然後重新綁定子假別並且呼叫選擇後事件
    * 「kddlLEACODE_SelectedIndexChanged」有選擇假別時，取出目前下拉式選單的DataSource，並且巡覽取出該假別是否需要上傳附件
* **/

/**
* 修改時間：2018/12/14
* 修改人員：陳緯榕
* 修改項目：
    * 重複送單卻不會擋單(還是發生)
* 修改位置：
    * 「前端網頁」script〈CheckWork〉新增參數〈特殊日對象〉、〈特殊日期〉、〈請假說明〉、〈申請日期〉
    * 「checkVal」新增參數〈LEACODE〉、〈APPLICANTDATE〉、〈REMARK〉、〈SP_DATE〉、〈SP_NAME〉
    * 「checkVal」檢查完中繼表後，中繼表表示能送單，再繼續詢問WS
* **/

/**
* 修改時間：2018/12/05
* 修改人員：陳緯榕
* 修改項目：請假時間重複時，顯示文字錯誤
* 修改位置：
    * 「checkVal」加班字眼改為請假
* **/

/**
* 修改時間：2018/12/04
* 修改人員：陳緯榕
* 修改項目：
    * 請假說明限定在50字內
    * 重複送單卻不會擋單
* 修改位置：
    * 「前端網頁」請假說明〈ktxtREMARK〉加上〈size=50〉
    * 「checkVal」檢查是否有重複，改用〈 (CANCEL_DOC_NBR = '' OR CANCEL_DOC_NBR IS NULL)〉
* **/

/**
* 修改時間：2018/11/2
* 修改人員：陳緯榕
* 修改項目：在UOF APP端，選人元件點選之後不會帶資料回來
* 發生原因：RadButton如果只用預設參數值，在APP端時，不會觸發EVENT
* 修改位置：
    * 「前端網頁」可以加上參數，但因為不會使用telerik，所以處理方式是將需要有回傳功能的按鈕改為asp:Button物件；並且在未來禁用telerik:RadButton
* **/

/**
* 修改時間：2018/10/31
* 修改人員：陳緯榕
* 修改項目：可休假餘額查不到特殊日
* 修正位置：
    * 「kddlLEACODE_SelectedIndexChanged」如果是特殊日，〈IS_SPEC〉字串給值，並且加進〈Dialog.Open2〉的網址〈listLEAVE_TIMES〉參數〈IS_SPEC〉中
    * 「SetField」起單時，按鈕綁定改為呼叫〈kddlLEACODE_SelectedIndexChanged〉事件
    * 「kdtpSTARTTIME_TextChanged」的按鈕綁定改為呼叫〈kddlLEACODE_SelectedIndexChanged〉事件
    * 「kdtpENDTIME_TextChanged」的按鈕綁定改為呼叫〈kddlLEACODE_SelectedIndexChanged〉事件
* **/

/**
* 修改時間：2018/10/30
* 修改人員：陳緯榕
* 修改項目：特殊日期在簽核關時不見了
* 修正位置：
    * 「kddlLEACODE_SelectedIndexChanged」判斷為判斷是否為起單或退回申請者，是才清空特殊日
* **/

/**
* 修改時間：2018/10/26
* 修改人員：陳緯榕
* 修改項目：特殊日、特殊日對象必須顯示必填提示
* 修正位置：
    * 「前端網頁」〈特殊日期〉區塊新增一個〈Label〉用來提示，顏色藍色
    * 「前端網頁」〈特殊日期對像〉區塊新增一個〈Label〉用來提示，顏色藍色
    * 「前端網頁」CustomValidator〈必須選擇特殊日期〉移動到〈特殊日期〉區塊
    * 「kddlLEACODE_SelectedIndexChanged」判斷為特別假時，顯示該區塊所有東西〈lblSP_DATE〉、〈lblSP_NAME〉
* **/

/**
 * 修改時間：2018/09/11
 * 修改人員：陳緯榕
 * 修改項目：按下計算後，沒有訊息
 * 修正位置：
    * 「btnCal_Click」將原本顯示用的〈ktxtMessage〉改為〈lblAPIResultError〉
 * **/

/// <summary>
/// 飛騰請假資訊
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_LEAVE : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
{
    /// <summary>
    /// 資料庫連通字串
    /// </summary>
    string ConnectionString;


    SCSServicesProxy service = null;

    /// <summary>
    /// 當前FieldMode
    /// </summary>
    FieldMode FormFieldMode;

    /// <summary>
    /// KYT控制元件
    /// </summary>
    KYTController kytController;

    #region ==============公開方法及屬性==============
    //表單設計時
    //如果為False時,表示是在表單設計時
    private bool m_ShowGetValueButton = true;
    public bool ShowGetValueButton
    {
        get { return this.m_ShowGetValueButton; }
        set { this.m_ShowGetValueButton = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //這裡不用修改
        //欄位的初始化資料都到SetField Method去做
        SetField(m_versionField);
    }

    /// <summary>
    /// 外掛欄位的條件值
    /// </summary>
    public override string ConditionValue
    {
        get
        {
            //回傳字串
            //此字串的內容將會被表單拿來當做條件判斷的值
            string cv = new JGlobalLibs.UOFUtils.ConditionValue()
                .Add("dept", ktxtAPPLICANTDEPT.Text)
                .Add("leacode", kddlLEACODE.SelectedValue)
                .Add("leatimes", ktxtLEAHOURS.Text)
                .Add("leadays", ktxtLEADAYS.Text)
                .Add("title_name", hidLEAEMPTitleName.Value)
                .Add("agentapply", Current.UserGUID != this.ApplicantGuid ? "Y" : "N")
                .Add("has_agent", ktxtLEAAGENT.Text != "" ? "Y" : "N")
                .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.ConditionValue.cv:{0}", cv));

            return cv;
        }
    }

    /// <summary>
    /// 是否被修改
    /// </summary>
    public override bool IsModified
    {
        get
        {
            //請自行判斷欄位內容是否有被修改
            //有修改回傳True
            //沒有修改回傳False
            return false;
        }
    }

    /// <summary>
    /// 查詢顯示的標題
    /// </summary>
    public override string DisplayTitle
    {
        get
        {
            //表單查詢或WebPart顯示的標題
            //回傳字串
            return string.Format(@"{0}, 假別:{1}, 請假時間:{2}~{3}, 天數:{4}, 時數:{5}",
                ktxtLEAEMP.Text,
                kddlLEACODE.SelectedItem.Text,
                kdtpSTARTTIME.Text,
                kdtpENDTIME.Text,
                ktxtLEADAYS.Text,
                ktxtLEAHOURS.Text);
        }
    }

    /// <summary>
    /// 訊息通知的內容
    /// </summary>
    public override string Message
    {
        get
        {
            //表單訊息通知顯示的內容
            //回傳字串
            return string.Format(@"{0}, 假別:{1}, 請假時間:{2}~{3}, 天數:{4}, 時數:{5}",
                ktxtLEAEMP.Text,
                kddlLEACODE.SelectedItem.Text,
                kdtpSTARTTIME.Text,
                kdtpENDTIME.Text,
                ktxtLEADAYS.Text,
                ktxtLEAHOURS.Text);
        }
    }


    /// <summary>
    /// 真實的值
    /// </summary>
    public override string RealValue
    {
        get
        {
            //回傳字串
            //取得表單欄位簽核者的UsetSet字串
            //內容必須符合EB UserSet的格式
            string rv = new JGlobalLibs.UOFUtils.RealValue()
                .AddPerson("LeaAgent", hidLEAAGENT.Value)
                .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.RealValue.rv:{0}", rv));
            return rv;
        }
        set
        {
            //這個屬性不用修改
            base.m_fieldValue = value;
        }
    }


    /// <summary>
    /// 欄位的內容
    /// </summary>
    public override string FieldValue
    {
        get
        {
            //回傳字串
            //取得表單欄位填寫的內容
            // 2019/01/31 因為〈UC_ChoiceListMobile〉沒有任何能夠觸發的事件，所以寫在第一個被觸發的點
            //if (hidIsMobileUI.Value == "Y" &&
            //    btnMobLEAAGENT.Visible)
            //{
            //    if (btnMobLEAAGENT.UserSet.Items.Count > 0) // 選擇代理人員
            //    {
            //        ktxtLEAAGENT.Text = btnMobLEAAGENT.UserSet.Items[0].Name;
            //        hidLEAAGENT.Value = btnMobLEAAGENT.UserSet.Items[0].Key;
            //    }
            //    else
            //    {
            //        ktxtLEAAGENT.Text = "";
            //        hidLEAAGENT.Value = "";
            //    }
            //}
            return kytController.FieldValue;
        }
        set
        {
            //這個屬性不用修改
            base.m_fieldValue = value;
        }
    }

    /// <summary>
    /// 是否為第一次填寫
    /// </summary>
    public override bool IsFirstTimeWrite
    {
        get
        {
            //這裡請自行判斷是否為第一次填寫
            return false;
        }
        set
        {
            //這個屬性不用修改
            base.IsFirstTimeWrite = value;
        }
    }

    /// <summary>
    /// 顯示時欄位初始值
    /// </summary>
    /// <param name="versionField">欄位集合</param>
    public override void SetField(Ede.Uof.WKF.Design.VersionField versionField)
    {
        FieldOptional fieldOptional = versionField as FieldOptional;

        if (fieldOptional != null)
        {
            #region ==============屬性說明==============『』
            //fieldOptional.IsRequiredField『是否為必填欄位,如果是必填(True),如果不是必填(False)』
            //fieldOptional.DisplayOnly『是否為純顯示,如果是(True),如果不是(False),一般在觀看表單及列印表單時,屬性為True』
            //fieldOptional.HasAuthority『是否有填寫權限,如果有填寫權限(True),如果沒有填寫權限(False)』
            //fieldOptional.FieldValue『如果已有人填寫過欄位,則此屬性為記錄其內容』
            //fieldOptional.FieldDefault『如果欄位有預設值,則此屬性為記錄其內容』
            //fieldOptional.FieldModify『是否允許修改,如果允許(fieldOptional.FieldModify=FieldModifyType.yes),如果不允許(fieldOptional.FieldModify=FieldModifyType.no)』
            //fieldOptional.Modifier『如果欄位有被修改過,則Modifier的內容為EBUser,如果沒有被修改過,則會等於Null』
            #endregion

            //#region ==============如果沒有填寫權限時,就要顯示有填寫權限人員的清單,只要把以下註解拿掉即可==============
            //if (!fieldOptional.HasAuthority『是否有填寫權限)
            //{
            //    string strItemName = String.Empty;
            //    Ede.Uof.EIP.Organization.Util.UserSet userSet = ((FieldOptional)versionField).FieldControlData;

            //    for (int i = 0; i < userSet.Items.Count; i++)
            //    {
            //        if (i == userSet.Items.Count - 1)
            //        {
            //            strItemName += userSet.Items[i].Name;
            //        }
            //        else
            //        {
            //            strItemName += userSet.Items[i].Name + "、";
            //        }
            //    }

            //    lblHasNoAuthority.ToolTip = lblAuthorityMsg.Text + "：" + strItemName;
            //}
            //#endregion

            #region ==============如果有修改，要顯示修改者資訊==============
            if (fieldOptional.Modifier != null)
            {
                lblModifier.Visible = true;
                lblModifier.ForeColor = System.Drawing.Color.Red;
                lblModifier.Text = String.Format("( {0}：{1} )", this.lblMsgSigner.Text, fieldOptional.Modifier.Name);
            }
            #endregion

            this.FormFieldMode = fieldOptional.FieldMode; // 記住本次 FieldMode

            // 初始化kytcontroller
            kytController = new KYTController(UpdatePanel1);

            // 取得資料庫連通字串
            ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
            service = ConstructorCommonSettings.setSCSSServiceProxDefault();

            //hidIsMobileUI.Value = base.MobileUI ? "Y" : "N"; // 判斷現在是否是MobileUI
            //hidAPIResult.Value = "OK";

            if (!Page.IsPostBack) // 網頁首次載入
            {
                if (!string.IsNullOrEmpty(fieldOptional.FieldValue))
                    kytController.FieldValue = fieldOptional.FieldValue;

                kytController.SetAllViewType(KYTViewType.ReadOnly); // 設定所有KYT物件唯讀
                ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
                btnCal.Visible = false; // 隱藏計算
                //btnCANLEAS.Visible = false; // 隱藏可休假餘額
                btnLEAAGENT.Visible = false; // 隱藏代理人選人按鈕
                btnLEAAGENT_Group.Visible = false; // 隱藏代理人選人按鈕
                //btnMobLEAAGENT.Visible = false; // MOBILEUI版選擇代理人員
                btnLEAEMP.isSearchSelfGroupUsers = false;
                btnLEAEMP.Visible = false; // 請假人選人按鈕隱藏

                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        hidAPIResult.Value = ""; // 清掉先前的旗標
                        kytController.SetAllViewType(KYTViewType.Input); // 設定所有KYT物件可輸入
                        ktxtMessage.ViewType = KYTViewType.ReadOnly;
                        ktxtAPPLICANTDATE.ReadOnly = true; // 申請日期唯讀
                        ktxtAPPLICANTDEPT.ReadOnly = true; // 部門唯讀
                        ktxtLEAEMP.ReadOnly = true; // 請假人唯讀
                        //ktxtLEAAGENT.Visible = false; // 隱藏代理人
                        ktxtLEAAGENT.ReadOnly = true; // 代理人唯讀
                        ktxtLEAHOURS.ReadOnly = true; // 請假時數唯讀
                        ktxtLEADAYS.ReadOnly = true; // 請假天數唯讀
                        btnCal.Visible = true; // 顯示計算
                        //btnCANLEAS.Visible = true; // 顯示可休假餘額
                        //if (hidIsMobileUI.Value == "N") // 現在是MOBILEUI之外的畫面
                        //{
                        //    btnLEAAGENT.Visible = true; // 隱藏代理人選人按鈕
                        //    btnMobLEAAGENT.Visible = false; // MOBILEUI版選擇代理人員
                        //    ktxtLEAAGENT.Visible = true; // 隱藏代理人
                        //}
                        //else
                        //{
                        //    btnLEAAGENT.Visible = false; // 隱藏代理人選人按鈕
                        //    btnMobLEAAGENT.Visible = true; // MOBILEUI版選擇代理人員
                        //    ktxtLEAAGENT.Visible = false; // 隱藏代理人
                        //}
                        if (fieldOptional.FieldMode == FieldMode.Applicant &&
                            string.IsNullOrEmpty(fieldOptional.FieldValue)) // 起單時專用操作狀態或初始值
                        {
                            // 草稿ID，查詢有無上傳附件
                            hidFormScriptID.Value = Request["scriptId"] != null ? (string)Request["scriptId"] : "";
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                            ktxtAPPLICANTDATE.Text = DateTime.Now.ToString("yyyy/MM/dd"); // 設定申請日期
                            ktxtLEAEMP.Text = string.Format(@"{0} ({1})", KUser.Name, KUser.Account);  // 設定請假人資訊
                            hidLEAEMP.Value = KUser.UserGUID;
                            ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 設定部門資訊
                            hidAPPLICANTDEPT.Value = KUser.GroupID[0];
                            hidLEAEMPTitleId.Value = KUser.Title_ID[0];
                            hidLEAEMPTitleName.Value = KUser.Title_Name[0];
                            hidLEAEMPAccount.Value = KUser.Account;
                            string[] sAccount = hidLEAEMPAccount.Value.Split('\\');
                            hidLEAEMPAccount.Value = sAccount[sAccount.Length - 1];
                            hidGROUPCODE.Value = KUser.GroupCode[0];


                            // 請假單預設請假時間
                            DateTime dtDefaultStart = DateTime.MinValue;
                            DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_START_TIME), out dtDefaultStart);
                            if (dtDefaultStart > DateTime.MinValue)
                            {
                                kdtpSTARTTIME.Text = dtDefaultStart.ToString("yyyy/MM/dd HH:mm");
                            }
                            DateTime dtDefaultEnd = DateTime.MinValue;
                            DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_END_TIME), out dtDefaultEnd);
                            if (dtDefaultEnd > DateTime.MinValue)
                            {
                                kdtpENDTIME.Text = dtDefaultEnd.ToString("yyyy/MM/dd HH:mm");
                            }
                        }
                        else // 退回申請者
                        {
                            hidDOC_NBR.Value = taskObj != null ? taskObj.FormNumber : "";
                        }
                        // 使用UOF代理人設定
                        if (SCSHR.SCSHRConfiguration.AGENT_FUNC.ToUpper() == "AGENT")
                        {
                            btnLEAAGENT.Visible = true; // 顯示代理人選人按鈕
                            using (SqlDataAdapter sda = new SqlDataAdapter(@"SELECT COUNT(*) 
                                                                               FROM TB_EB_USER_AGENT
                                                                              WHERE USER_GUID = @USER_GUID", ConnectionString))
                            using (DataSet ds = new DataSet())
                            {
                                sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", hidLEAEMP.Value);
                                try
                                {
                                    if (sda.Fill(ds) > 0)
                                    {
                                        if (((int)ds.Tables[0].Rows[0][0]) > 0)
                                        {
                                            btnLEAAGENT.UserSql = string.Format(@"SELECT ACCOUNT,NAME,USER_GUID,'' AS 'GROUP_ID' FROM TB_EB_USER WHERE USER_GUID IN (SELECT AGENT_USER FROM TB_EB_USER_AGENT WHERE USER_GUID = '{0}') AND IS_SUSPENDED = 0 AND (EXPIRE_DATE IS NULL OR EXPIRE_DATE > GETDATE())",
                                                     hidLEAEMP.Value);
                                            btnLEAAGENT.isSearchSelfGroupUsers = false;
                                        }
                                        else
                                        {
                                            btnLEAAGENT.GroupID = this.ApplicantGroupId;
                                            btnLEAAGENT.isSearchSelfGroupUsers = true;
                                        }
                                    }

                                }
                                catch (Exception e)
                                {
                                    KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.SetField.SELECT.TB_EB_USER_AGENT.ERROR:{0}", e.Message));
                                    KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.SetField.SELECT.TB_EB_USER_AGENT.ERROR.STACKTRACE:{0}", e.StackTrace));
                                }
                            }
                        }
                        else if (SCSHR.SCSHRConfiguration.AGENT_FUNC.ToUpper() == "ALL")
                        {
                            btnLEAAGENT_Group.Visible = true; // 顯示代理人選人按鈕
                            // 選人語法用預設
                            //btnLEAAGENT_Group.UserSql = string.Format(@"
                            //SELECT A.ACCOUNT,A.[NAME],A.USER_GUID, B.GROUP_ID
                            //  FROM TB_EB_USER AS A, TB_EB_EMPL_DEP AS B
                            // WHERE B.GROUP_ID = @GROUP_ID
                            //   AND A.IS_SUSPENDED = 0 
                            //   AND (A.EXPIRE_DATE IS NULL 
                            //       OR A.EXPIRE_DATE > GETDATE())
                            //");
                        }

                        // 參數設定中「DEFAULT_DAYOFF_AGENT」請假單代理人預設自動帶入第一代理人 = "Y"時，表頭代理人欄位自動帶入該請假人之第一代理人，若查不到就不帶入。
                        // 同時啟動代理的checkbox需勾選。
                        if (SCSHR.SCSHRConfiguration.DEFAULT_DAYOFF_AGENT.ToUpper() == "Y")
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter(@"SELECT AGENT_USER 
                                                                               FROM TB_EB_USER_AGENT
                                                                              WHERE USER_GUID = @USER_GUID", ConnectionString))
                            using (DataSet ds = new DataSet())
                            {
                                sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", hidLEAEMP.Value);
                                try
                                {
                                    if (sda.Fill(ds) > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            hidLEAAGENT.Value = ds.Tables[0].Rows[0]["AGENT_USER"].ToString();
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.SetField.SCSHR.SCSHRConfiguration.DEFAULT_DAYOFF_AGENT.ERROR:{0}", e.Message));
                                }
                            }
                            using (SqlDataAdapter sda = new SqlDataAdapter(@"SELECT ACCOUNT, NAME 
                                                                               FROM TB_EB_USER
                                                                              WHERE USER_GUID = @USER_GUID", ConnectionString))
                            using (DataSet ds = new DataSet())
                            {
                                sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", hidLEAAGENT.Value);
                                try
                                {
                                    if (sda.Fill(ds) > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            //string temp_name = ds.Tables[0].Rows[0]["NAME"].ToString();
                                            //string temp_account = ds.Tables[0].Rows[0]["ACCOUNT"].ToString();
                                            //ktxtLEAAGENT.Text = temp_name + " (" + temp_account + ")";
                                            ktxtLEAAGENT.Text = ds.Tables[0].Rows[0]["NAME"].ToString() + " (" + ds.Tables[0].Rows[0]["ACCOUNT"].ToString() + ")";
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.SetField.SCSHR.SCSHRConfiguration.DEFAULT_DAYOFF_AGENT.ERROR:{0}", e.Message));
                                }
                            }
                        }
                        // 2018/12/20 因為KYT的缺陷(暫不修改)的問題，每次都重新綁定DataSource
                        string leaValue = kddlLEACODE.SelectedValue;
                        // 接收TMP_NEEDATTACH，用來判斷是否要檢查附件
                        DataTable dtLEASource = getLEAVEType(SCSHRConfiguration.SCSSLeaveTypeProgID, "*");
                        DataTable dtSpecSettingSource = getLEAVEType("ATT0011300", "*");
                        // 將SFSTARTDATE對應到dtLEASource中
                        foreach (DataColumn dc in dtSpecSettingSource.Columns)
                        {
                            if (!dtLEASource.Columns.Contains(dc.ColumnName))
                                dtLEASource.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                        }
                        foreach (DataRow dr in dtLEASource.Rows)
                        {
                            foreach (DataRow _dr in dtSpecSettingSource.Rows)
                            {
                                if (dr["VACATIONID"].ToString().Equals(_dr["SYS_ID"].ToString()))
                                {
                                    dr["SFSTARTDATE"] = _dr["SFSTARTDATE"];
                                    break;
                                }
                            }
                        }
                        // 將過期或尚未到達的可用假別去掉
                        for (int i = dtLEASource.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dtLEASource.Rows[i];
                            DateTime dtBegin = DateTime.MinValue;
                            DateTime dtEnd = DateTime.MaxValue;
                            DateTime.TryParse(dr["SYS_BEGINDATE"].ToString(), out dtBegin);
                            bool isNotEndMin = DateTime.TryParse(dr["SYS_ENDDATE"].ToString(), out dtEnd);
                            if (!isNotEndMin) dtEnd = DateTime.MaxValue;
                            if (DateTime.Now.Date < dtBegin.Date || // 如果現在日期大於等於生效日
                                    DateTime.Now.Date > dtEnd.Date) // 或者現在日期小於等於到期日
                            {
                                dtLEASource.Rows.RemoveAt(i); // 移除這筆資料
                            }
                        }
                        // 新增請選擇選項
                        DataRow ndr = dtLEASource.NewRow();
                        ndr["SYS_VIEWID"] = "";
                        ndr["SYS_NAME"] = "===請選擇===";
                        dtLEASource.Rows.InsertAt(ndr, 0);
                        // 綁定子假別
                        kddlLEACODE.DataSource = dtLEASource;
                        kddlLEACODE.DataValueField = "SYS_VIEWID";
                        kddlLEACODE.DataTextField = "SYS_NAME";
                        kddlLEACODE.DataBind();
                        kddlLEACODE.SelectedValue = leaValue;
                        kddlLEACODE_SelectedIndexChanged(kddlLEACODE, null);
                        KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.SetField.Applicant.dtLEASource:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtLEASource)));
                        // 設定Picker是否能輸入
                        kdtpSTARTTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpENDTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        ktxtMessage.Text = CheckTimeRange() ? "迄止時間小於起始時間" : "";
                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Verify: // Verify
                        break;
                    case FieldMode.Print: // 表單列印
                    case FieldMode.View: // 表單觀看
                    case FieldMode.Signin: // 表單簽核
                                           // 2019/03/18 因為KYT的缺陷(暫不修改)的問題，每次都重新綁定DataSource

                        // 接收TMP_NEEDATTACH，用來判斷是否要檢查附件
                        DataTable _dtLEASource = getLEAVEType(SCSHRConfiguration.SCSSLeaveTypeProgID, "*");
                        DataTable _dtSpecSettingSource = getLEAVEType("ATT0011300", "*");
                        // 將SFSTARTDATE對應到dtLEASource中
                        foreach (DataColumn dc in _dtSpecSettingSource.Columns)
                        {
                            if (!_dtLEASource.Columns.Contains(dc.ColumnName))
                                _dtLEASource.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                        }
                        foreach (DataRow dr in _dtLEASource.Rows)
                        {
                            foreach (DataRow _dr in _dtSpecSettingSource.Rows)
                            {
                                if (dr["VACATIONID"].ToString().Equals(_dr["SYS_ID"].ToString()))
                                {
                                    dr["SFSTARTDATE"] = _dr["SFSTARTDATE"];
                                    break;
                                }
                            }
                        }
                        // 將過期或尚未到達的可用假別去掉
                        for (int i = _dtLEASource.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = _dtLEASource.Rows[i];
                            DateTime dtBegin = DateTime.MinValue;
                            DateTime dtEnd = DateTime.MaxValue;
                            DateTime.TryParse(dr["SYS_BEGINDATE"].ToString(), out dtBegin);
                            bool isNotEndMin = DateTime.TryParse(dr["SYS_ENDDATE"].ToString(), out dtEnd);
                            if (!isNotEndMin) dtEnd = DateTime.MaxValue;
                            if (DateTime.Now.Date < dtBegin.Date || // 如果現在日期大於等於生效日
                                    DateTime.Now.Date > dtEnd.Date) // 或者現在日期小於等於到期日
                            {
                                _dtLEASource.Rows.RemoveAt(i); // 移除這筆資料
                            }
                        }
                        // 新增請選擇選項
                        DataRow _ndr = _dtLEASource.NewRow();
                        _ndr["SYS_VIEWID"] = "";
                        _ndr["SYS_NAME"] = "===請選擇===";
                        _dtLEASource.Rows.InsertAt(_ndr, 0);
                        // 綁定子假別
                        kddlLEACODE.DataSource = _dtLEASource;
                        kddlLEACODE.DataValueField = "SYS_VIEWID";
                        kddlLEACODE.DataTextField = "SYS_NAME";
                        kddlLEACODE.DataBind();
                        kddlLEACODE.SelectedValue = kddlLEACODE.SelectedValue;
                        kddlLEACODE_SelectedIndexChanged(kddlLEACODE, null);
                        lblAGENT.Text = kchkAGENT.Checked ? "是" : "否";
                        hidDOC_NBR.Value = taskObj.FormNumber;
                        break;
                }
                #region 代理人顯示開關

                bool is_show_Agent = SCSHRConfiguration.IS_LEV_AGENT_SHOW.ToUpper() == "Y";
                divAgentTitle.Visible = is_show_Agent;
                divAgent.Visible = is_show_Agent;
                divSetAgentTitle.Visible = is_show_Agent;
                divSetAgent.Visible = is_show_Agent;
                bool is_showAgent = SCSHRConfiguration.IS_SHOWAGENT.ToUpper() == "Y";
                divSetAgentTitle.Visible = is_showAgent;
                divSetAgent.Visible = is_showAgent;

                #endregion 代理人顯示開關
            }
            else // 如果網頁POSTBACK
            {
                JGlobalLibs.WebUtils.RequestHiddenFields(UpdatePanel1); // 取回HiddenField的值
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        // 設定Picker是否能輸入
                        kdtpSTARTTIME.ViewType = KYTViewType.Input;
                        kdtpENDTIME.ViewType = KYTViewType.Input;
                        kdtpSTARTTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpENDTIME.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        break;
                }
            }

        }
    }

    #region ==============修改權限LinkButton的事件==============
    protected void lnk_Edit_Click(object sender, EventArgs e)
    {
        //這裡還要加入控制項的隱藏或顯示

        lnk_Cannel.Visible = true;
        lnk_Edit.Visible = false;
        lnk_Submit.Visible = true;
    }
    protected void lnk_Cannel_Click(object sender, EventArgs e)
    {
        //這裡還要加入控制項的隱藏或顯示

        SetField(m_versionField);

        lnk_Cannel.Visible = false;
        lnk_Edit.Visible = true;
        lnk_Submit.Visible = false;
    }
    protected void lnk_Submit_Click(object sender, EventArgs e)
    {
        //這裡還要加入控制項的隱藏或顯示

        lnk_Cannel.Visible = false;
        lnk_Edit.Visible = true;
        lnk_Submit.Visible = false;

        //儲存表單資料
        if (base._IFieldOOServer.Count == 0) return;
        ((IFieldCompetenceServer)base._IFieldOOServer[0]).SaveForm();
    }
    #endregion                

    private DataTable getLEAVEType(string progId, string selectFields)
    {
        DataTable dtScource = new DataTable();
        Exception ex = null;
        dtScource = service.BOFind(progId, selectFields, out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.getLEAVEType.service.Error:{0}", ex.Message));
        for (int i = dtScource.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = dtScource.Rows[i];
            string lev_type = SCSHR.SCSHRConfiguration.LEV_TYPE_FILTER;
            string[] lev_filters = lev_type.Split(',');
            if (!string.IsNullOrEmpty(lev_type) &&
                lev_filters.Any(levtype => ((string)dr["SYS_VIEWID"]).Equals(levtype)))
            {
                dtScource.Rows.RemoveAt(i);
            }

            //switch ((string)dr["SYS_VIEWID"])
            //{
            //    case "2101": // 曠職
            //    case "2201": // 早退
            //    case "2202": // 遲到
            //    //case "2301": // 忘帶卡
            //        dtScource.Rows.RemoveAt(i);
            //        break;
            //}
        }
        return dtScource;
    }

    private void RefreshMainData(string EMPID, string LEVCODE, string LEVNAME, string LEVDATE_START, string LEVDATE_END, string IS_SPEC)
    {
        //DateTime dtStart = DateTime.MinValue;
        //DateTime.TryParse(LEVDATE_START, out dtStart);
        //DateTime dtEnd = DateTime.MinValue;
        //DateTime.TryParse(LEVDATE_END, out dtEnd);
        //Exception ex = null;
        //SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSLeaveProgID,
        //   string.IsNullOrEmpty(IS_SPEC) ? "GetEmpAllLRData" : "GetLeaveSpecInfo",
        //    SCSHR.Types.SCSParameter.getPatameters(new { TMP_EmployeeID = EMPID, Tmp_svacationID = LEVCODE, StartDate = dtStart.ToString("yyyyMMdd"), EndDate = dtEnd.ToString("yyyyMMdd") }),
        //    out ex);

        // 傳入飛騰請假資訊，起:請假日(起) or 當天 ； 迄: 起+天數(config參數-LEV_INFO_RANGE)
        DateTime dateLEVDATE_S = DateTime.Now;
        DateTime.TryParse(LEVDATE_START, out dateLEVDATE_S);
        int LEV_INFO_RANGE = 0;
        int.TryParse(SCSHRConfiguration.LEV_INFO_RANGE, out LEV_INFO_RANGE);

        string dateStart = dateLEVDATE_S.ToString("yyyy/MM/dd");
        string dateEnd = dateLEVDATE_S.AddDays(LEV_INFO_RANGE).ToString("yyyy/MM/dd"); ;
        Exception ex = null;
        SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSLeaveProgID,
           string.IsNullOrEmpty(IS_SPEC) ? "GetEmpAllLRData" : "GetLeaveSpecInfo",
            SCSHR.Types.SCSParameter.getPatameters(new { TMP_EmployeeID = EMPID, Tmp_svacationID = LEVCODE, StartDate = dateStart, EndDate = dateEnd }),
            out ex);

        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"listLEAVE_TIMES.RefreshMainData.GetEmpAllLRData.ERROR:{0}", ex.Message));
        if (parameters != null &&
                    parameters.Length > 0)
        {
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"listLEAVE_TIMES.RefreshMainData.GetEmpAllLRData.result:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));
            if (parameters[0].DataType.ToString() == "DataTable")
            {
                DataTable dtSource = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                dtSource.Columns.Add("LEVNAME", typeof(string));
                if (string.IsNullOrEmpty(IS_SPEC))
                {
                    foreach (DataRow dr in dtSource.Rows)
                    {
                        dr["LEVNAME"] = dr["SVacationName"];
                    }
                }
                else
                {
                    foreach (DataRow dr in dtSource.Rows)
                    {
                        dr["LEVNAME"] = LEVNAME;
                    }
                }
                gvMain.DataSource = dtSource;
                gvMain.DataBind();
                gvMain.Visible = dtSource.Rows.Count > 0;
            }
        }

    }

    /// <summary>
    /// 檢查是否是特別假
    /// </summary>
    /// <param name="dtSource"></param>
    /// <param name="selectedValue"></param>
    /// <param name="needAttach"></param>
    /// <returns></returns>
    private bool checkIsSpecDate(DataTable dtSource, string selectedValue, out string needAttach, out string SEXAPPLY)
    {
        bool isSFDate = false;
        needAttach = "";
        SEXAPPLY = "0";
        if (dtSource != null)
        {
            // 用Select都會有奇妙的錯，所以自己跑回圈找，非常浪費效能
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr["SYS_VIEWID"].ToString().Equals(selectedValue))
                {
                    isSFDate = dr["SFSTARTDATE"] != Convert.DBNull ? (bool)dr["SFSTARTDATE"] : false;
                    needAttach = dr["TMP_NEEDATTACH"].ToString();
                    SEXAPPLY = dr["TMP_LIMITSEXAPPLY"].ToString();
                    break;
                }
            }
        }
        return isSFDate;
    }

    protected void kddlLEACODE_SelectedIndexChanged(object sender, EventArgs e)
    {
        KYTDropDownList _kddlLEACODE = (KYTDropDownList)sender;
        DataTable dtLEASource = _kddlLEACODE.DataSource as DataTable;

        bool isSFDate = false;
        string needAttach = "";
        string SEXAPPLY = "";
        isSFDate = checkIsSpecDate(dtLEASource, _kddlLEACODE.SelectedValue, out needAttach, out SEXAPPLY);
        hidNeedAttach.Value = needAttach;
        hidSEXAPPLY.Value = SEXAPPLY;
        string IS_SPEC = "";
        if (isSFDate) // 現在是特別假
        {
            IS_SPEC = "1";
            //if (this.FormFieldMode == FieldMode.Applicant ||
            //    this.FormFieldMode == FieldMode.ReturnApplicant) // 起單關才清除
            //    kdpSP_DATE.Text = "";
            divSP.Visible = true;
        }
        else // 一般假別
        {
            if (this.FormFieldMode == FieldMode.Applicant ||
                   this.FormFieldMode == FieldMode.ReturnApplicant)  // 起單關才清除
                kdpSP_DATE.Text = "";
            ktxtSP_NAME.Text = "";
            divSP.Visible = false;
        }
        hidAPIResult.Value = ""; // 有更動就清空
        lblAPIResultError.Text = "";

        if (SCSHRConfiguration.IS_LEAVE_DETAIL == "Y")
        {
            DateTime today = DateTime.Now;
            RefreshMainData(hidLEAEMPAccount.Value, _kddlLEACODE.SelectedValue, kddlLEACODE.SelectedItem.Text, today.ToString("yyyy/MM/dd"), today.AddMonths(1).ToString("yyyy/MM/dd"), IS_SPEC);

            //// 請假時間(起)(迄)，皆為預設時
            //if (((DateTime)kdtpSTARTTIME.SelectedDate).ToString("yyyy/MM/dd") == today.ToString("yyyy/MM/dd") && ((DateTime)kdtpENDTIME.SelectedDate).ToString("yyyy/MM/dd") == today.ToString("yyyy/MM/dd"))
            //{
            //    RefreshMainData(hidLEAEMPAccount.Value, _kddlLEACODE.SelectedValue, kddlLEACODE.SelectedItem.Text, today.ToString("yyyy/MM/dd"), today.AddMonths(1).ToString("yyyy/MM/dd"), IS_SPEC);
            //}
            //else // 請假時間(起)(迄)，有選擇時
            //{
            //    RefreshMainData(hidLEAEMPAccount.Value, _kddlLEACODE.SelectedValue, kddlLEACODE.SelectedItem.Text, kdtpSTARTTIME.Text, kdtpENDTIME.Text, IS_SPEC);
            //}
        }

    }

    //protected void ubtnLEAAGENT_EditButtonOnClick(UserSet userSet)
    //{
    //    if (userSet.Items.Count > 0)
    //    {
    //        // 只取第一筆
    //        ktxtLEAAGENT.Text = userSet.Items[0].Name;
    //        hidLEAAGENT.Value = userSet.Items[0].Key;
    //    }
    //    else
    //    {
    //        ktxtLEAAGENT.Text = "";
    //        hidLEAAGENT.Value = "";
    //    }
    //}

    protected void btnCal_Click(object sender, EventArgs e)
    {
        //if (string.IsNullOrEmpty(ktxtREMARK.Text))
        //{
        //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"
        //            alert('必須填寫請假說明');
        //            "), true);
        //    return;
        //}
        ktxtMessage.Text = "";
        ktxtLEAHOURS.Text = "0";
        ktxtLEADAYS.Text = "0";
        hidAPIResult.Value = "";
        lblAPIResultError.Text = "";
        hidConfirm.Value = "";
        if (!string.IsNullOrEmpty(kdtpSTARTTIME.Text) &&
            !string.IsNullOrEmpty(kdtpENDTIME.Text))
        {
            DataTable dtLEASource = kddlLEACODE.DataSource as DataTable;

            bool isSFDate = false;
            string needAttach = "";
            string SEXAPPLY = "";
            isSFDate = checkIsSpecDate(dtLEASource, kddlLEACODE.SelectedValue, out needAttach, out SEXAPPLY);

            if (isSFDate && // 當現在是特別假
                string.IsNullOrEmpty(kdpSP_DATE.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"
                    alert('必須選擇特殊日期');
                    "), true);
                return;
            }

            if (isSFDate && // 當現在是特別假
                string.IsNullOrEmpty(ktxtSP_NAME.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"
                    alert('必須選擇特殊日對象');
                    "), true);
                return;
            }

            DataTable dtResult = null;
            bool resultStatus = false;
            hidAPIResult.Value = ""; // 清空之前的API查詢結果
            // 資料檢核
            DateTime dtStart = DateTime.MinValue;
            DateTime.TryParse(kdtpSTARTTIME.Text, out dtStart);
            DateTime dtEnd = DateTime.MinValue;
            DateTime.TryParse(kdtpENDTIME.Text, out dtEnd);
            string[] sAccount = hidLEAEMPAccount.Value.Split('\\');
            JArray jaTable = new JArray();
            JObject _joTable = new JObject();
            _joTable.Add(new JProperty("USERNO", "1"));
            _joTable.Add(new JProperty("SYS_VIEWID", ""));
            _joTable.Add(new JProperty("SYS_DATE", DateTime.Parse(ktxtAPPLICANTDATE.Text).ToString("yyyyMMdd")));
            //_joTable.Add(new JProperty("TMP_EMPLOYEEID", sAccount[sAccount.Length - 1])); // 取最右邊
            _joTable.Add(new JProperty("TMP_EMPLOYEEID", new KYT_UserPO().GetUserDetailByAccount(sAccount[sAccount.Length - 1]).EmployeeNo)); // 取最右邊
            _joTable.Add(new JProperty("TMP_SVACATIONID", kddlLEACODE.SelectedValue));
            //_joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(hidLEAAGENT.Value) ?
            //    JGlobalLibs.UOFUtils.getUserAccount(hidLEAAGENT.Value) :
            //    btnMobLEAAGENT.UserSet.Items.Count > 0 ?
            //    !string.IsNullOrEmpty(btnMobLEAAGENT.UserSet.Items[0].Key) ?
            //    btnMobLEAAGENT.UserSet.Items[0].Key :
            //    "" :
            //    ""));
            //string AGENTID = JGlobalLibs.UOFUtils.getUserAccount(hidLEAAGENT.Value);
            string AGENTID = hidLEAAGENT.Value;
            _joTable.Add(new JProperty("STARTDATE", dtStart.ToString("yyyyMMdd")));
            _joTable.Add(new JProperty("STARTTIME", dtStart.ToString("HHmm")));
            _joTable.Add(new JProperty("ENDDATE", dtEnd.ToString("yyyyMMdd")));
            _joTable.Add(new JProperty("ENDTIME", dtEnd.ToString("HHmm")));
            _joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(AGENTID) ? new KYT_UserPO().GetUserDetailByUserGuid(AGENTID).EmployeeNo : "admin"));
            _joTable.Add(new JProperty("NOTE", ktxtREMARK.Text.Trim()));
            if (!string.IsNullOrEmpty(ktxtSP_NAME.Text))
            {
                //_joTable.Add(new JProperty("SPECIALDATE", kddlSP_DATE.SelectedValue));
                _joTable.Add(new JProperty("SPECIALDATE", kdpSP_DATE.Text));
                _joTable.Add(new JProperty("STARGETNAME", ktxtSP_NAME.Text));
            }
            jaTable.Add(_joTable);
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.btnCal_Click.JATABLE:{0}", jaTable));
            DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
            dtSource.TableName = SCSHRConfiguration.SCSSLeaveProgID;
            DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSLeaveProgID);
            dsSource.Tables.Add(dtSource);
            Exception ex = null;
            dtResult = service.BOImport(SCSHRConfiguration.SCSSLeaveProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.btnCal_Click.BOImport::{0}::Result::{1}", SCSHRConfiguration.SCSSLeaveProgID, Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
            if (ex != null)
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.btnCal_Click.BOImport.ERROR:{0}", ex.Message));
            if (dtResult != null &&
                dtResult.Rows.Count > 0)
            {
                resultStatus = dtResult.Rows[0]["_STATUS"].ToString() == "0";
                if (!resultStatus)
                    lblAPIResultError.Text = dtResult.Rows[0]["_MESSAGE"].ToString();
            }

            if (resultStatus)
            {
                hidAPIResult.Value = "OK";
                // 計算時間
                ex = null; // 初始化
                DataTable dtCalcResult = null;
                SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSGetLeaveHourProgID,
                "GetLeaveHours",
                SCSHR.Types.SCSParameter.getPatameters(new
                {
                    TMP_EmployeeID = sAccount[sAccount.Length - 1],
                    TMP_SVacationID = kddlLEACODE.SelectedValue,
                    StartDate = dtStart.ToString("yyyyMMdd"),
                    StartTime = dtStart.ToString("HHmm"),
                    EndDate = dtEnd.ToString("yyyyMMdd"),
                    EndTime = dtEnd.ToString("HHmm")
                }),
                out ex);
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.btnCal_Click.BOExecFunc::{0}::Result::{1}", SCSHRConfiguration.SCSSGetLeaveHourProgID, Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));

                if (ex != null)
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.btnCal_Click.BOExecFunc.ERROR:{0}", ex.Message));
                if (parameters != null &&
                    parameters.Length > 0)
                {
                    if (parameters[0].DataType.ToString() == "DataTable")
                    {
                        //DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.btnCal_Click.BOExecFunc.Result.XML:{0}", parameters[0].Xml));

                        dtCalcResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                    }
                }
                if (dtCalcResult != null &&
                    dtCalcResult.Rows.Count > 0)
                {
                    ktxtLEAHOURS.Text = dtCalcResult.Rows[0]["LeaveHours"].ToString();
                    ktxtLEADAYS.Text = dtCalcResult.Rows[0]["LeaveDays"].ToString();
                }

                // 為了滿足檢查完畢後需出現檢查成功
                lblAPIResultError.Text = string.IsNullOrEmpty(lblAPIResultError.Text) ? "檢查成功" : lblAPIResultError.Text;
            }
            else
                lblAPIResultError.Text = string.IsNullOrEmpty(lblAPIResultError.Text) ? "檢查失敗" : lblAPIResultError.Text;
        }
        else // 沒選請假時間
        {
            // do nothing
        }
    }

    [WebMethod]
    public static string checkVal(string USER_GUID, string START_TIME, string END_TIME, string LEACODE, string APPLICANTDATE, string REMARK, string SP_DATE, string SP_NAME, string SCRIPTID, string DOC_NBR, string NEEDATTACH, string AGENT_GUID, string SEXAPPLY)
    {
        ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
        JObject joMessage = new JObject();
        string connectionstring = new DatabaseHelper().Command.Connection.ConnectionString;
        bool isNoError = true;
        string SEX = "M";
        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.checkVal.USER_GUID: {0}, START_TIME: {1}, END_TIME: {2}", USER_GUID, START_TIME, END_TIME));
        DateTime dtSTART_TIME = DateTime.MinValue;
        DateTime.TryParse(START_TIME, out dtSTART_TIME);
        DateTime dtEND_TIME = DateTime.MinValue;
        DateTime.TryParse(END_TIME, out dtEND_TIME);
        if (dtEND_TIME > dtSTART_TIME) // 迄止時間大於開始時間時
        {
            if (dtSTART_TIME > DateTime.MinValue &&
            dtEND_TIME > DateTime.MinValue)
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(@"
                -- 檢查請假時間是否有重複
                SELECT DOC_NBR
                  FROM Z_SCSHR_LEAVE
                  WHERE APPLICANTGUID = @USER_GUID
	                AND ((STARTTIME >= @STARTTIME AND STARTTIME < @ENDTIME)
                         OR (ENDTIME > @STARTTIME AND ENDTIME <= @ENDTIME)
                         OR (STARTTIME <= @STARTTIME AND ENDTIME >= @ENDTIME))
                    AND (CANCEL_DOC_NBR = '' OR CANCEL_DOC_NBR IS NULL)
                    AND ((TASK_STATUS = 1 AND TASK_RESULT = 0)
                        OR (TASK_STATUS = 2 AND TASK_RESULT = 0)
                        OR (TASK_STATUS = 1 AND TASK_RESULT IS NULL))
                
                -- 查性別
                SELECT SEX FROM TB_EB_EMPL WHERE USER_GUID = @USER_GUID

                -- 查附件
                IF (@DOC_NBR = '') -- 草稿起單
	                BEGIN
		                IF EXISTS (SELECT * 
					                 FROM TB_WKF_SCRIPT 
				                    WHERE SCRIPT_ID = @SCRIPT_ID 
					                  AND (PERSONAL_ATTACH_ID IS NOT NULL 
					                      OR PERSONAL_ATTACH_ID != ''))
			                BEGIN
				                IF EXISTS (SELECT * -- 檔案有沒有被刪掉
							                 FROM TB_EB_FILE_STORE 
						                    WHERE FILE_GROUP_ID = (SELECT PERSONAL_ATTACH_ID
													                 FROM TB_WKF_SCRIPT 
													                WHERE SCRIPT_ID = @SCRIPT_ID)
							                  AND DELETED <> 1)
					                BEGIN
						                SELECT 0 -- 草稿起單時有上傳個人附件，通過
					                  END
				                ELSE
				                    BEGIN
						                SELECT 1 -- 草稿起單時沒有上傳個人附件，沒通過
					                  END
			                  END
		                 ELSE -- 草稿起單時沒有上傳個人附件
			                BEGIN
				                SELECT 2 -- 草稿起單時沒有上傳個人附件，沒通過
			                  END
	                  END
                ELSE -- 不是草稿起單
	                BEGIN
		                IF EXISTS(SELECT ATTACH_ID -- 是退回申請者或取回
	                                FROM TB_WKF_TASK_NODE 
	                               WHERE TASK_ID = (SELECT TASK_ID 
									                  FROM TB_WKF_TASK 
									                 WHERE DOC_NBR = @DOC_NBR)
					                 AND NODE_STATUS = 4)
	                          BEGIN
					                IF EXISTS(SELECT ATTACH_ID -- 是退回申請者或取回有沒有上傳過附件
							                    FROM TB_WKF_TASK_NODE 
						                       WHERE TASK_ID = (SELECT TASK_ID 
												                  FROM TB_WKF_TASK 
												                 WHERE DOC_NBR = @DOC_NBR)
								                 AND NODE_STATUS = 4
								                 AND (ATTACH_ID != '' 
									                 OR ATTACH_ID IS NOT NULL))
						                BEGIN
							                IF EXISTS(SELECT * -- 檔案有沒有被刪掉
										                     FROM TB_EB_FILE_STORE 
										                    WHERE FILE_GROUP_ID = (SELECT ATTACH_ID 
										  	  					                     FROM TB_WKF_TASK_NODE 
										  						                    WHERE TASK_ID = (SELECT TASK_ID 
										  											                   FROM TB_WKF_TASK 
										  											                  WHERE DOC_NBR = @DOC_NBR)
										  							                  AND NODE_STATUS = 4
										  							                  AND (ATTACH_ID != '' 
										  								                  OR ATTACH_ID IS NOT NULL))
									                          AND DELETED <> 1)
									                BEGIN
										                SELECT 0 -- 取回或退回申請者起單時有上傳個人附件，通過
									                END
								                ELSE
									                BEGIN
										                SELECT 4 -- 取回或退回申請者起單時沒有上傳個人附件，不通過
									                END	 
						                END
					                ELSE
						                BEGIN
							                SELECT 5 -- 是退回申請者或取回有未曾上傳過附件
						                END
				                END
		                ELSE
		                    BEGIN
		   		                SELECT 0 -- 不是取回或退回申請者起單，不檢查
		                     END
	                END
            ", connectionstring))
                using (DataSet ds = new DataSet())
                {
                    sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", USER_GUID);
                    sda.SelectCommand.Parameters.AddWithValue("@STARTTIME", dtSTART_TIME);
                    sda.SelectCommand.Parameters.AddWithValue("@ENDTIME", dtEND_TIME);
                    sda.SelectCommand.Parameters.AddWithValue("@SCRIPT_ID", SCRIPTID);
                    sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
                    try
                    {
                        sda.Fill(ds);

                        #region 直接送給飛騰檢查是否有重複

                        SCSServicesProxy service = ConstructorCommonSettings.setSCSSServiceProxDefault();
                        DataTable dtResult = null;
                        JArray jaTable = new JArray();
                        JObject _joTable = new JObject();
                        _joTable.Add(new JProperty("USERNO", "1"));
                        _joTable.Add(new JProperty("SYS_VIEWID", ""));
                        _joTable.Add(new JProperty("SYS_DATE", DateTime.Parse(APPLICANTDATE).ToString("yyyyMMdd")));
                        //_joTable.Add(new JProperty("TMP_EMPLOYEEID", new UserUCO().GetEBUser(USER_GUID).Account));
                        //_joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(AGENT_GUID) ? JGlobalLibs.UOFUtils.getUserAccount(AGENT_GUID) : "admin"));
                        _joTable.Add(new JProperty("TMP_EMPLOYEEID", new KYT_UserPO().GetUserDetailByUserGuid(USER_GUID).EmployeeNo));
                        _joTable.Add(new JProperty("TMP_SVACATIONID", LEACODE));
                        _joTable.Add(new JProperty("STARTDATE", dtSTART_TIME.ToString("yyyyMMdd")));
                        _joTable.Add(new JProperty("STARTTIME", dtSTART_TIME.ToString("HHmm")));
                        _joTable.Add(new JProperty("ENDDATE", dtEND_TIME.ToString("yyyyMMdd")));
                        _joTable.Add(new JProperty("ENDTIME", dtEND_TIME.ToString("HHmm")));
                        _joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(AGENT_GUID) ? new KYT_UserPO().GetUserDetailByUserGuid(AGENT_GUID).EmployeeNo : "admin"));
                        _joTable.Add(new JProperty("NOTE", REMARK.Trim()));
                        if (!string.IsNullOrEmpty(SP_NAME))
                        {
                            //_joTable.Add(new JProperty("SPECIALDATE", kddlSP_DATE.SelectedValue));
                            _joTable.Add(new JProperty("SPECIALDATE", SP_DATE));
                            _joTable.Add(new JProperty("STARGETNAME", SP_NAME));
                        }
                        jaTable.Add(_joTable);
                        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.checkVal.JATABLE:{0}", jaTable));
                        DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                        dtSource.TableName = SCSHRConfiguration.SCSSLeaveProgID;
                        DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSLeaveProgID);
                        dsSource.Tables.Add(dtSource);
                        Exception ex = null;
                        dtResult = service.BOImport(SCSHRConfiguration.SCSSLeaveProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
                        if (ex != null)
                            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.checkVal.BOImport.ERROR:{0}", ex.Message));
                        if (dtResult != null &&
                            dtResult.Rows.Count > 0)
                        {
                            if (dtResult.Rows[0]["_STATUS"].ToString() == "0")
                                joMessage.Add(new JProperty("wsrepeat", dtResult.Rows[0]["_MESSAGE"].ToString()));
                        }

                        #endregion 直接送給飛騰檢查是否有重複
                        //if (ds.Tables[0].Rows.Count > 0) // 有重複申請的請假時間
                        //{
                        //    //joMessage.Add(new JProperty("docrepeat", string.Format(@"<br />[請假時間]重複 (單號：{0} 日期：{1} ~ {2})",
                        //    //(string)ds.Tables[0].Rows[0]["DOC_NBR"], dtSTART_TIME.ToString(), dtEND_TIME.ToString())));
                        //}
                        //else if (ds.Tables[0].Rows.Count == 0) // 中繼表找不到
                        //{

                        //}
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            SEX = ds.Tables[1].Rows[0]["SEX"].ToString();
                        }
                        if (NEEDATTACH.ToUpper() == "TRUE")
                        {
                            if ((int)ds.Tables[2].Rows[0][0] != 0) // 沒有上傳附件
                            {
                                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.checkVal.檢查上傳附件結果: {0}", (int)ds.Tables[2].Rows[0][0]));
                                joMessage.Add(new JProperty("nofile", "需在個人附件上傳請假證明\n"));
                            }
                        }
                        // 需要阻擋請假時間起訖區間
                        if (SCSHRConfiguration.NEED_BLOCK_LEV_LIMIT_TIME.ToUpper() == "Y")
                        {
                            int preDays = 0;
                            int nextDays = 0;
                            int.TryParse(SCSHRConfiguration.LEV_PREVIOUS_DAYS, out preDays);
                            int.TryParse(SCSHRConfiguration.LEV_NEXT_DAYS, out nextDays);
                            DateTime dtStartLimit = DateTime.Parse(string.Format("{0} 00:00:00", DateTime.Now.AddDays(-preDays).ToString("yyyy/MM/dd")));
                            DateTime dtOffLimit = DateTime.Parse(string.Format("{0} 23:59:59", DateTime.Now.AddDays(nextDays).ToString("yyyy/MM/dd")));
                            if (preDays > 0)
                            {
                                if (dtEND_TIME < dtStartLimit)
                                {
                                    joMessage.Add(new JProperty("START_LIMIT", string.Format(@"已超過請假需 {0} 日內送單申請", preDays)));
                                }
                            }
                            if (nextDays > 0)
                            {
                                if (dtEND_TIME > dtOffLimit)
                                {
                                    joMessage.Add(new JProperty("OFF_LIMIT", string.Format(@"已超過請假需 {0} 日前送單申請", nextDays)));
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE checkVal Error: {0}", e.Message));
                        isNoError = false;
                    }
                }
            }
            else
            {
                isNoError = false;
            }
        }
        else // 迄止時間小於開始時間
        {
            joMessage.Add(new JProperty("STARTTIME_BIGGER", "迄止時間小於開始時間"));
        }

        if (!isNoError)
        {
            joMessage.Add(new JProperty("isError", "<br />發生錯誤"));
        }
        if (SEX == "M" && SEXAPPLY == "2") // 如果是男性，假別卻要求女性
        {
            joMessage.Add(new JProperty("SEXAPPLY", "男性不能申請"));
        }
        else if (SEX == "F" && SEXAPPLY == "1") // 如果是女性，假別卻要求男性
        {
            joMessage.Add(new JProperty("SEXAPPLY", "女性不能申請"));
        }
        //joMessage.Add(new JProperty("LEA_SEX", SEX));
        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.checkVal.Result:{0}", joMessage.ToString()));

        return joMessage.ToString();
    }


    [WebMethod]
    public static string CheckSignLevel(string DOC_NBR)
    {
        ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
        string cstring = new DatabaseHelper().Command.Connection.ConnectionString;
        JObject joMessage = new JObject();

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT * FROM Z_SCSHR_LEAVE WHERE DOC_NBR = @DOC_NBR
            ", cstring))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    DateTime dtSTART_TIME = (DateTime)dr["STARTTIME"];
                    DateTime dtEND_TIME = (DateTime)dr["ENDTIME"];
                    SCSServicesProxy service = ConstructorCommonSettings.setSCSSServiceProxDefault();
                    DataTable dtResult = null;
                    JArray jaTable = new JArray();
                    JObject _joTable = new JObject();
                    _joTable.Add(new JProperty("USERNO", "1"));
                    _joTable.Add(new JProperty("SYS_VIEWID", DOC_NBR));
                    _joTable.Add(new JProperty("SYS_DATE", ((DateTime)dr["APPLICANTDATE"]).ToString("yyyyMMdd")));
                    //_joTable.Add(new JProperty("TMP_EMPLOYEEID", JGlobalLibs.UOFUtils.getUserAccount(dr["LEAEMP"].ToString())));
                    //_joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(dr["LEAAGENT"].ToString()) ? JGlobalLibs.UOFUtils.getUserAccount(dr["LEAAGENT"].ToString()) : "admin"));
                    _joTable.Add(new JProperty("TMP_EMPLOYEEID", new KYT_UserPO().GetUserDetailByUserGuid(dr["LEAEMP"].ToString()).EmployeeNo));
                    _joTable.Add(new JProperty("TMP_SVACATIONID", dr["LEACODE"].ToString()));
                    _joTable.Add(new JProperty("STARTDATE", dtSTART_TIME.ToString("yyyyMMdd")));
                    _joTable.Add(new JProperty("STARTTIME", dtSTART_TIME.ToString("HHmm")));
                    _joTable.Add(new JProperty("ENDDATE", dtEND_TIME.ToString("yyyyMMdd")));
                    _joTable.Add(new JProperty("ENDTIME", dtEND_TIME.ToString("HHmm")));
                    _joTable.Add(new JProperty("TMP_AGENTID", !string.IsNullOrEmpty(dr["LEAAGENT"].ToString()) ? new KYT_UserPO().GetUserDetailByUserGuid(dr["LEAAGENT"].ToString()).EmployeeNo : "admin"));
                    _joTable.Add(new JProperty("NOTE", dr["REMARK"].ToString().Trim()));
                    if (!string.IsNullOrEmpty(dr["SP_NAME"].ToString()))
                    {
                        //_joTable.Add(new JProperty("SPECIALDATE", kddlSP_DATE.SelectedValue));
                        _joTable.Add(new JProperty("SPECIALDATE", ((DateTime)dr["SP_DATE"]).ToString("yyyy/MM/dd")));
                        _joTable.Add(new JProperty("STARGETNAME", dr["SP_NAME"].ToString()));
                    }
                    jaTable.Add(_joTable);
                    DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.CheckSignLevel.JATABLE:{0}", jaTable));
                    DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                    dtSource.TableName = SCSHRConfiguration.SCSSLeaveProgID;
                    DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSLeaveProgID);
                    dsSource.Tables.Add(dtSource);
                    Exception ex = null;
                    //dtResult = service.BOImport(SCSHRConfiguration.SCSSLeaveProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
                    if (ex != null)
                    {
                        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.CheckSignLevel.BOImport.ERROR:{0}", ex.Message));
                        joMessage.Add(new JProperty("Error", "發生錯誤，請通知管理員"));
                        goto toEnd;
                    }
                    DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.CheckSignLevel.dtResult:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
                    if (dtResult != null &&
                        dtResult.Rows.Count > 0)
                    {
                        if (dtResult.Rows[0]["_STATUS"].ToString() != "0")
                            joMessage.Add(new JProperty("CheckWSError", dtResult.Rows[0]["_MESSAGE"].ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_LEAVE.CheckSignLevel.SELECT.Z_SCSHR_LEAVE.ERROR: {0}", e.Message));
                joMessage.Add(new JProperty("Error", "發生錯誤，請通知管理員"));
                goto toEnd;
            }
        }
    toEnd:
        if (joMessage.Count == 0)
            joMessage.Add(new JProperty("checkMsg", "success"));
        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_LEAVE.CheckSignLevel.Result:{0}", joMessage.ToString()));
        return joMessage.ToString();
    }


    protected void kdtpSTARTTIME_TextChanged(object sender, EventArgs e)
    {
        KYTDateTimePicker _kdtpSTARTTIME = (KYTDateTimePicker)sender;
        //kddlLEACODE_SelectedIndexChanged(kddlLEACODE, null);
        if (SCSHRConfiguration.IS_COPY_TIMESTART_DATE_TO_TIMEEND_DATE.ToString().ToUpper() == "Y")
        {
            DateTime dtPreSTARTTIME = DateTime.MinValue;
            DateTime.TryParse(ViewState["STARTTIME"] != null ? ViewState["STARTTIME"].ToString() : "", out dtPreSTARTTIME); // 如果沒有紀錄上次的時間，就記錄成時間最小值
            DateTime dtSTARTTIME = DateTime.MinValue;
            DateTime.TryParse(_kdtpSTARTTIME.Text, out dtSTARTTIME);
            if (dtPreSTARTTIME.Date != dtSTARTTIME.Date) // 如果上次的日期不等於現在的日期，更動迄止時間
            {
                DateTime dtENDTIME = DateTime.MinValue;
                DateTime.TryParse(kdtpENDTIME.Text, out dtENDTIME);
                kdtpENDTIME.Text = string.Format(@"{0} {1}", dtSTARTTIME.ToString("yyyy/MM/dd"), dtENDTIME.ToString("HH:mm")); // 只更動日期
                ViewState["STARTTIME"] = _kdtpSTARTTIME.Text; // 紀錄這次的時間
            }
        }
        ktxtMessage.Text = CheckTimeRange() ? "迄止時間小於起始時間" : "";
    }

    protected void kdtpENDTIME_TextChanged(object sender, EventArgs e)
    {
        KYTDateTimePicker _kdtpENDTIME = (KYTDateTimePicker)sender;
        //kddlLEACODE_SelectedIndexChanged(kddlLEACODE, null);
        ktxtMessage.Text = CheckTimeRange() ? "迄止時間小於起始時間" : "";

    }

    protected void kddlSP_DATE_SelectedIndexChanged(object sender, EventArgs e)
    {
        KYTDropDownList _kddlSP_DATE = (KYTDropDownList)sender;
        DataTable dtSource = _kddlSP_DATE.DataSource as DataTable;
        foreach (DataRow dr in dtSource.Rows)
        {
            if (dr["SPECIALDATE"].ToString() == _kddlSP_DATE.SelectedValue)
            {
                ktxtSP_NAME.Text = dr["STARGETNAME"].ToString();
            }
        }
    }

    protected void btnLEAAGENT_DialogReturn(object sender, string result)
    {
        JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
        if (jArray.Count > 0)
        {
            //EBUser user = new UserUCO().GetEBUser(jArray[0]["GUID"].ToString());
            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(jArray[0]["GUID"].ToString()); // 人員
            ktxtLEAAGENT.Text = KUser.Name + " (" + KUser.Account + ")";
            hidLEAAGENT.Value = KUser.UserGUID;
        }
        else
        {
            ktxtLEAAGENT.Text = "";
            hidLEAAGENT.Value = "";
        }
    }

    /// <summary>
    /// 申請人_選擇
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    protected void btnLEAEMP_DialogReturn(object sender, string result)
    {
        JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
        if (jArray.Count > 0)
        {
            //EBUser user = new UserUCO().GetEBUser(jArray[0]["GUID"].ToString());
            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(jArray[0]["GUID"].ToString()); // 人員

            ktxtLEAEMP.Text = string.Format(@"{0} ({1})", KUser.Name, KUser.Account);  // 設定請假人資訊
            hidLEAEMP.Value = KUser.UserGUID;
            ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 設定部門資訊
            hidAPPLICANTDEPT.Value = KUser.GroupID[0];
            hidLEAEMPTitleId.Value = KUser.Title_ID[0];
            hidLEAEMPTitleName.Value = KUser.Title_Name[0];
            hidLEAEMPAccount.Value = KUser.Account;
            string[] sAccount = hidLEAEMPAccount.Value.Split('\\');
            hidLEAEMPAccount.Value = sAccount[sAccount.Length - 1];
            hidGROUPCODE.Value = KUser.GroupCode[0];
            btnLEAAGENT.GroupID = KUser.GroupID[0];
            btnLEAAGENT.isSearchSelfGroupUsers = true;
        }
        else
        {
            ktxtLEAEMP.Text = ""; // 設定請假人資訊
            hidLEAEMP.Value = "";
            ktxtAPPLICANTDEPT.Text = ""; // 設定部門資訊
            hidAPPLICANTDEPT.Value = "";
            hidLEAEMPTitleId.Value = "";
            hidLEAEMPTitleName.Value = "";
            hidLEAEMPAccount.Value = "";
            hidLEAEMPAccount.Value = "";
            hidGROUPCODE.Value = "";
        }
    }

    /// <summary>
    /// 檢查請假起迄時間是否("起"大於"迄")
    /// </summary>
    /// <returns></returns>
    private bool CheckTimeRange()
    {
        bool IsAlert = false;
        if (kdtpSTARTTIME.SelectedDate != null && kdtpENDTIME.SelectedDate != null)
        {
            if (DateTime.Compare((DateTime)kdtpSTARTTIME.SelectedDate, (DateTime)kdtpENDTIME.SelectedDate) > 0)
            {
                IsAlert = true;
            }
        }
        return IsAlert;
    }

}
