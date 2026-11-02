using Ede.Uof.EIP.Organization;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using Ede.Uof.WKF.Design;
using KYTLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using UOFAssist.WKF;


/**
* 修改時間：2021/06/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.簽核站點="STARTOV"時(預加班後要起加班單)，給付方式開放更改
    * 2.所有有使用EBUser的地方，都改為呼叫通用方法取得人員資訊
    * 3.呼叫飛騰，傳入參數-TMP_EMPLOYEEID，原給「ACCOUNT」 -> 改給「EMPLOYEEID」
* 修改原因：
    * 1.修改規格，因送飛騰檢查會出現「無法使用此給付方式」，所以改為讓申請者可更改
    * 2.修改規格，UOF的EBUser有時候會異常取不到人員資訊，以防再多花時間去查明原因，改為通用方法直接查SQL方式取得人員資訊
    * 3.規格修正，飛騰目前都改為固定傳入「EMPLOYEEID」
* 修改位置：
    * 1.「SetField() -> FieldMode=Signin -> SiteCode == "STARTOV"」中，註解給付方式「krblCHANGETYPE.ViewType = KYTViewType.ReadOnly;」
    * 2.「SetField()、setDefaultUserInfo()、btnAPPLICANT_DialogReturn()」中，註解所有EBUser，改為KYT_EBUser
    * 3.「btnCal_Click()、CheckSignLevel()」，傳入TMP_EMPLOYEEID，的值 -> 改傳入EmployeeNo
* **/

/**
* 修改時間：2021/06/07
* 修改人員：梁夢慈
* 修改項目：
    * 1. 是否預加班(ktxtSignResult)的顯示文字「是/否」
* 修改原因：
    * 1. BUG修正，取得是否加班時間預設值內，有一段判斷顯示文字，需改於FieldMode之後，因為在起單狀態會顯示文字(正確不應該顯示)
* 修改位置：
    * 1.「SetField()」中，將顯示文字的三元運算子程式段，移到FieldMod之後「kcbIsPre.Checked ? "是" : "否"」
* **/

/**
* 修改時間：2021/06/04
* 修改人員：梁夢慈
* 修改項目：
    * 1. 計算本月、本日的加班單總計，SQL語法條件設定取當月/當日
* 修改原因：
    * 1. BUG修正，因原只給區間，所以計算出來的總計會是區間內的數值
* 修改位置：
    * 1.「getOverTimeHours()」中，SQL語法中，刪除區間，新增DATEPART(yyyy,DateTime)、DATEPART(mm,DateTime)、DATEPART(dd,DateTime)
* **/

/**
* 修改時間：2021/06/03
* 修改人員：梁夢慈
* 修改項目：
    * 1. 當載入網頁、重新選擇加班時間(起)，本月累積時數顯示值，重新計算當月已加班時數 <-- 依加班(起)的月份
    * 2. 計算當月內的加班單，只要計算加班(起)月份的總時數
    * 3. 跳窗-加班狀況，Dialog.Open傳入參數新增START_DATE，更改顯示標題-新增(顯示前後30天範圍內的所有加班單)
    * 4. 將是否預加班(kcbIsPre)，程式段移動至判斷判斷FieldMode之前
* 修改原因：
    * 1. BUG修正，本月累積時數，因沒有重新取得，所以預設都會給現在時間的月份
    * 2. 新增規格，依歸屬日(加班起)的月份，計算當月累積時數
    * 3. 新增規格，依歸屬日(加班起)的月份，顯示前後1個月所有加班單
    * 4. 因為此設定中，會有給加班時間的預設值，加班跳窗、加班累計時數會依加班(起)or預加班(起)當作條件
* 修改位置：
    * 1.「SetField -> 起單/退回申請者、kdtpOT_START_TextChanged()」中，重新計算當月已加班時數getOverTimeHours()」
    * 2.「getOverTimeHours()」中，新增傳入參數bool值-IsGetMonth，如果是true，只要計算起的月份即可
    * 3.「SetField()、kdtpOT_START_TextChanged()」中，設定加班狀況跳窗按鈕(rbtnFindOVERTIME)，傳入的參數、顯示標題
    * 4.「SetField()」中，取得飛騰參數-IS_OVH_PRE_SHOW的相關程式段，移動至判斷判斷FieldMode之前
* **/

/**
* 修改時間：2021/05/27
* 修改人員：梁夢慈
* 修改項目：
    * 1. 請假人選擇按鈕(btnAPPLICANT)隱藏不顯示。
* 修改原因：
    * 1. BUG修正，因上次修改，誤以為一般加班單也要改為可選所有人，(只有批次才改為可選所有人)
* 修改位置：
    * 1. 「SetField -> 起單/退回申請者」中，註解btnAPPLICANT.Visible = true
* **/

/**
* 修改時間：2021/03/31
* 修改人員：梁夢慈
* 修改項目：
    * 1. 新增一個選人元件(btnAPPLICANT)，可選擇所有人
* 發生原因：
    * 1. 新增規格，操作上更便利，改用冠永騰版本的選人元件
* 修改位置：
    * 「前端網頁」中，於申請人欄位後面新增一個選人元件(btnAPPLICANT)
* **/

/**
* 修改時間：2021/01/20
* 修改人員：陳緯榕
* 修改項目：
    * 預加班單再postback後，不應該還原「kdtpPreOT_START」、「kdtpPreOT_END」的資料
* 修改原因：
    * 2020/01/18的修改造成的問題
* 修改位置：
    * 「SetField」中，當現在是POSTBACK時，判斷〈起單或退回申請者〉，但不應該呼叫〈kcbIsPre_CheckedChanged〉，而是要判斷〈kcbIsPre〉，預加班時〈kdtpOT_START〉、〈kdtpOT_END〉不能輸入
* **/

/**
* 修改時間：2021/01/18
* 修改人員：陳緯榕
* 修改項目：
    * POSTBACK時，要確保預加班選項的按鈕開放設定不變
* 修改原因：
    * 有POSTBACK後，原本預加班不能填寫加班時間被當作能夠填寫
* 修改位置：
    * 「SetField」中，當現在是POSTBACK時，判斷〈起單或退回申請者〉，執行〈kcbIsPre_CheckedChanged〉
* **/

/**
* 修改時間：2020/12/15
* 修改人員：陳緯榕
* 修改項目：
    * STARTOV時，kdtpOT_START、kdtpOT_END在POSTBACK要能輸入
    * 加班時間起訖相減後，要取TimeSpan的TotalDays屬性的值，並比較天數是否大於0，或小時數大於24
* 修改原因：
    * POSTBACK後又被鎖定了
    * ******
    * 加班時間起訖超過好幾天卻不會被視為大於24小時
* 修改位置：
    * 「SetField」中，當現在是〈postback〉時，判斷是〈簽核中〉且是〈STARTOV〉關卡時，〈kdtpOT_START〉、〈kdtpOT_END〉先將屬性〈ViewType〉設為〈Input〉；再將屬性〈TextBoxReadOnly〉依照〈SCSHRConfiguration.IS_PICKER_READONLY〉設定
    * **********
    * 「kdtpOT_START_TextChanged」中，〈起訖時間相差超過24小時〉的判斷要用〈TimeSpan〉相減後取〈TotalDays〉是否大於等於1或〈TotalHours〉是否大於24
    * 「kdtpOT_END_TextChanged」中，〈起訖時間相差超過24小時〉的判斷要用〈TimeSpan〉相減後取〈TotalDays〉是否大於等於1或〈TotalHours〉是否大於24
* **/

/**
* 修改時間：2020/12/07
* 修改人員：陳緯榕
* 修改項目：
    * STARTOV時，kdtpOT_START、kdtpOT_END要能夠遵照config的時間輸入框是否能輸入的設定
* 修改原因：
    * 預加班通過後，加班時間無法遵照config的時間輸入框是否能輸入的設定
* 修改位置：
    * 「SetField」中，當現在是〈簽核中〉且是〈STARTOV〉關卡時，〈kdtpOT_START〉、〈kdtpOT_END〉的屬性〈TextBoxReadOnly〉要依照〈SCSHRConfiguration.IS_PICKER_READONLY〉設定
* **/

/**
* 修改時間：2020/10/28
* 修改人員：陳緯榕
* 修改項目：
    * 預加班時間在「IS_OVH_PRE_SHOW(是否顯示預加班選項)」設定為「N」時，表單的「預加班資訊」區塊會顯示
* 修改原因：
    * 上次沒處理好的BUG
* 修改位置：
    * 「前端網頁」中，〈div:divH1〉增加屬性〈Visible="false"〉
* **/

/**
* 修改時間：2020/10/22
* 修改人員：陳緯榕
* 修改項目：
    * 預加班時間在「IS_OVH_PRE_SHOW(是否顯示預加班選項)」設定為「N」時，表單的「預加班時間(起)」區塊會顯示
* 修改原因：
    * 上次沒處理好的BUG
* 修改位置：
    * 「前端網頁」中，〈div:divPreOT〉增加屬性〈Visible="false"〉
* **/

/**
* 修改時間：2020/10/08
* 修改人員：陳緯榕
* 修改項目：
    * 加班單的是否預加班的顯示由config來控制
* 發生原因：
    * 新規格
* 修改位置：
    * 「前端網頁」中，〈是否預加班〉設定id為〈divPreTitle〉、同一組的div區塊，設定id為〈divPre〉
    * 「SetField」中，當〈SCSHRConfiguration.IS_OVH_PRE_SHOW〉是〈Y〉時，將〈divPre〉和〈divPreTitle〉的屬性〈Visible〉設為〈false〉，並且不執行和〈kcbIsPre〉有關的動作
* **/

/**
* 修改時間：2020/04/16
* 修改人員：陳緯榕
* 修改項目：
    * 新規格：新增條件-GROUPCODE，起單人的部門代碼
    * 新規格：標題修改
* 發生原因：
    * 新規格
    * *******
    * 新規格
* 修改位置：
    * 「ConditionValue」中，Conditinovalue加入〈GROUPCODE〉，內容值為〈hidGROUPCODE.Value〉
    * ********
    * 「前端網頁」中，〈SCSHR加班資訊〉改為〈加班資訊〉
* **/

/**
* 修改時間：2020/03/13
* 修改人員：陳緯榕
* 修改項目：
    * ktxtSignResult顯示需為紅色
* 發生原因：
    * ，紅色是屬於警告意味強的顏色，故使用紅色顯示錯誤
* 修改位置：
    * 「前端網頁」中，新增〈CSS：msgColor〉；並將〈ktxtSignResult〉的屬性〈LabelCssClass〉設為〈msgColor〉
* **/


/**
* 修改時間：2020/03/10
* 修改人員：陳緯榕
* 修改項目：
    * 飛騰加班單加參數，隱藏預加班「DEFAULT_OV_PE」；預設給付方式 X不預設 0加班費 1補休 2加班費及補休
* 發生原因：
    * 新規格，要讓客戶好處理
* 修改位置：
    * 「reBindCHANGETYPE」中，〈krblCHANGETYPE〉的選取值，由〈SCSHRConfiguration.DEFAULT_OV_PE〉取得
* **/

/**
* 修改時間：2020/02/17
* 修改人員：陳緯榕
* 修改項目：
    * 當月累計時數無法查到資料
* 發生原因：
    * 簽核中，預加班時間填入加班時間的順序是在取得當月累計時數後，所以不能用kdtpOT_START來當時間的基準
* 修改位置：
    * 「SetField」中，當〈FieldMode.Signin〉而且〈SiteCode==STARTOV〉時，〈dtStart〉的來源是〈kdtpPreOT_START〉
* **/

/**
* 修改時間：2020/02/06
* 修改人員：陳緯榕
* 修改項目：
    * 當月累計時數無法查到資料(昨天沒改到簽核中)
* 發生原因：
    * 給予的起迄時間是0001/01/01到0001/01/31，這表示「DateTime.Now.Year」取得的是0001，原因『未知』
* 修改位置：
    * 「SetField」中，當〈FieldMode.Signin〉而且〈SiteCode==STARTOV〉時，當月累計時數的起迄由〈dtStart.Year〉改為〈dtStart.ToString().Substring(0, 4)〉(取現在加班時間(起)，然後切割字串，取前4碼，最後轉成int)
* **/

/**
* 修改時間：2020/02/05
* 修改人員：陳緯榕
* 修改項目：
    * 當月累計時數無法查到資料
* 發生原因：
    * 給予的起迄時間是0001/01/01到0001/01/31，這表示「DateTime.Now.Year」取得的是0001，原因『未知』
* 修改位置：
    * 「SetField」中，當月累計時數的起迄由〈DateTime.Now.Year〉改為〈DateTime.Now.ToString().Substring(0, 4)〉(取現在時間，然後切割字串，取前4碼，最後轉成int)
* **/

/**
* 修改時間：2020/01/06
* 修改人員：陳緯榕
* 修改項目：
    * 當月累計時數改用加班起始時間當作當月
* 發生原因：
    * 2019/12/31起的加班單，在2020/1月份簽核時都會讓當月累計時數變為0，因為簽核時取的是「當下」的月份
* 修改位置：
    * 「SetField」中，當簽核關卡時，取〈kdtpOT_START〉當作〈ktxtApplyHoursThemon(當月累計時數)〉的年和月
* **/

/**
* 修改時間：2019/12/17
* 修改人員：陳緯榕
* 修改項目：
    * 當月加班時數顯示錯誤
* 發生原因：
    * (12/11沒改到的)原本的當月最後一天是先找本年度下個月的第一天再減一天，但是這樣個寫法在跨年度中會變成去年。例：2019/1/1 - 1 = 2018/12/31
* 修改位置：
    * 「SetField」中，當簽核中到〈STARTOV〉關卡時，〈ktxtApplyHoursThemon〉呼叫〈getOverTimeHours〉時給的結束時間改成〈這個月的第一天〉再〈加一個月〉再〈減一天〉
* **/

/**
* 修改時間：2019/12/11
* 修改人員：陳緯榕
* 修改項目：
    * 當月加班時數顯示錯誤
* 發生原因：
    * 原本的當月最後一天是先找本年度下個月的第一天再減一天，但是這樣個寫法在跨年度中會變成去年。例：2019/1/1 - 1 = 2018/12/31
* 修改位置：
    * 「SetField」中，〈ktxtApplyHoursThemon〉呼叫〈getOverTimeHours〉時給的結束時間改成〈這個月的第一天〉再〈加一個月〉再〈減一天〉
* **/

/**
* 修改時間：2019/11/21
* 修改人員：Jay
* 修改項目：
    * Condition Value
* 修改位置：
    * Add DoOverType 加班別
* **/

/**
* 修改時間：2019/09/12
* 修改人員：陳緯榕
* 修改項目：
    * krblCHANGETYPE要預設為補休
* 修改位置：
    * 「reBindCHANGETYPE」中，最後要綁定預設〈krblCHANGETYPE.SelectedValue = "1"〉
* **/

/**
* 修改時間：2019/08/16
* 修改人員：陳緯榕
* 修改項目：
    * 無法送單
* 發生原因：
    * 使用者無法識別訊息是沒按下檢查
* 修改位置：
    * 「前端網頁」「CheckVal」中，先檢查〈API結果〉再檢查〈加班時數〉
    * 「前端網頁」「CheckVal」中，當〈hidAPIResult.val() == ""〉時，訊息為〈必須按下計算檢查〉；當〈hidAPIResult〉不是OK時，訊息為〈檢查失敗，請檢查輸入內容〉
* **/

/**
* 修改時間：2019/07/23
* 修改人員：陳緯榕
* 修改項目：
    * 如果表單不是退回申請者，而是退回能夠申請檢查的關卡，那要先刪除在飛騰的表單，之後再申請
* 發生原因：
    * 預加班申請時，如果經過STARTOV關卡後再退回到STARTOV關卡，按下計算就會無法通過，是因為表單已存在，而檢查(BOImport)沒有使用SYSVIEW_ID，所以才出錯
    * 但是如果單純的加上SYSVIEW_ID，會無法分辨加班和預加班還有是不是退回到這一關的申請，所以改用先查詢後刪除，最後再重新申請
* 修改位置：
    * 「btnCal_Click」中，計算完時數後，先檢查是否有〈FormNumber〉、關卡是否是〈STARTOV〉，接著使用〈BOFind〉找到表單後，再使用〈BODelete〉刪除表單
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
* 修改時間：2019/07/04
* 修改人員：陳緯榕
* 修改項目：
    * 飛騰回傳「,不允許修改」
* 發生原因：
    * 不明，所以先跳過所有檢查
* 修改位置：
    * 「CheckSignLevel」中，直接使用〈goto toEnd〉跳過所有檢查
* **/

/**
* 修改時間：2019/06/27
* 修改人員：陳緯榕
* 修改項目：
    * 增加：當時間(起)的日期更動時，同時變更時間(迄)的日期；更動時間時不處理
    * 改變畫面
* 修改位置：
    * 「kdtpOT_START_TextChanged」中，當〈SCSHRConfiguration.IS_COPY_TIMESTART_DATE_TO_TIMEEND_DATE〉的值為〈Y〉時，判斷〈如果上次的日期不等於現在的日期，更動迄止時間〉是否成立，成立時修改時間(迄)的日期
    * 「kdtpPreOT_START_TextChanged」中，當〈SCSHRConfiguration.IS_COPY_TIMESTART_DATE_TO_TIMEEND_DATE〉的值為〈Y〉時，判斷〈如果上次的日期不等於現在的日期，更動迄止時間〉是否成立，成立時修改時間(迄)的日期
    * ************
    * 「前端網頁」中，將〈加班狀況-rbtnFindOVERTIME〉從〈加班時間(迄)〉區塊移動到〈加班時數〉區塊
    * 「前端網頁」中，將〈加班內容說明〉區塊的〈CLASS〉改為〈col-md-12 divheader〉並且去除〈STYLE〉屬性
* **/

/**
* 修改時間：2019/06/13
* 修改人員：陳緯榕
* 修改項目：
    * 前端不檢查加班原因
* 發生原因：
    * 
* 修改位置：
    * 「前端網頁」「checkVal」中，不檢查〈kddlREMARK〉和〈ktxtREMARKOther〉
    * 「btnCal_Click」中，WS參數〈TMP_ATTREASONID〉要先判斷〈kddlREMARK〉是否有值
* **/

/**
* 修改時間：2019/05/08
* 修改人員：陳緯榕
* 修改項目：
    * KYTPicker在Postback後無法再手動輸入
* 修改位置：
    * 「前端網頁」中，將KYTPicker的屬性〈TextBoxReadOnly〉去除
* **/

/**
* 修改時間：2019/05/07
* 修改人員：陳緯榕
* 修改項目：
    * 更動顯示主旨
* 修改位置：
    * 「DisplayTitle」中：
        * 實際加班改為：《加班時間yyyy/MM/dd HH:mm~yyyy/MM/dd HH:mm，時數Times；加班折換方式：CHANGETYPE；申請人》
        * 預加班改為：《預加班時間yyyy/MM/dd HH:mm~yyyy/MM/dd HH:mm；加班折換方式：CHANGETYPE；申請人》
    * 「Message」中：
        * 實際加班改為：《加班時間yyyy/MM/dd HH:mm~yyyy/MM/dd HH:mm，時數Times；加班折換方式：CHANGETYPE；申請人》
        * 預加班改為：《預加班時間yyyy/MM/dd HH:mm~yyyy/MM/dd HH:mm；加班折換方式：CHANGETYPE；申請人》
* **/

/**
* 修改時間：2019/05/06
* 修改人員：陳緯榕
* 修改項目：
    * 累計時數可能會出問題的地方
* 修改位置：
    * 「getOverTimeHours」中，SQL中多查詢〈CANCEL_DOC_NBR = ''〉的條件
* **/

/**
* 修改時間：2019/04/30
* 修改人員：陳緯榕
* 修改項目：
    * 簽核檢查修正
* 發生原因：
    * 昨天的版本是有問題的
* 修改位置：
    * 改變方法：「[WebMethod]CheckSignLevel(string DOC_NBR)」變更為「[WebMethod]CheckSignLevel(string DOC_NBR, string startPunch, string offPunch, string DoOverType, string MFreeDate, string OverCompHours)」
    * 「前端網頁」「CheckVal」中，檢查前還需要〈ktxtWorkPunch〉、〈ktxtOffWorkPunch〉、〈hidDoOverType〉、〈ktxtMFreeDate〉、〈ktxtOverCompHours〉等參數的值
    ******* 附註：一等一開發的「$uof.pageMethod.syncUc」，當傳送的參數只有一個，而且內容是〈空字串〉時，會《忽略》這個參數，造成的結果就是找不到這個方法，會被當另一個多載
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
    * 「前端網頁」「CheckVal」中，當現在不是〈預加班〉而且〈canSend = true〉，呼叫〈CheckSignLevel〉檢查
    * 「SetField」中，簽核狀態時，記住現在的單號
* **/

/**
* 修改時間：2019/04/19
* 修改人員：陳緯榕
* 修改項目：
    * 簽核中跳出僅能補休的擋單訊息
* 修改位置：
    * 「前端網頁」「checkVal」中，〈krblCHANGETYPE〉存在才要檢查〈hidAllowCalcType〉
* **/

/**
* 修改時間：2019/04/18
* 修改人員：陳緯榕
* 修改項目：
    * 修改預加班時的顯示畫面
    * 是否場外加班預設不顯示
* 修改位置：
    * 「前端網頁」中，在〈SCSHR加班資訊〉後面加上〈預加班資訊〉，〈預加班時間〉後面加上〈加班資訊〉
    * 「kcbIsPre_CheckedChanged」中，〈kcbIsPre〉為〈true〉時，顯示〈divH1〉、〈divH2〉
    * ********
    * 「SetField」中，〈!Page.IsPostBack〉時，〈div1〉不顯示
* **/

/**
* 修改時間：2019/04/12
* 修改人員：陳緯榕
* 修改項目：
    * 選預加班起單後，在簽核的第一關就不能送出了
    * 會員僅能補休，但是選擇加班費卻能送出
    * DispalyTitle顯示錯誤
* 修改原因：
    * 預加班起單沒有按計算，所以沒有OK
* 修改位置：
    * 「前端網頁」「checkVal」中，檢查〈hidAPIResult〉前，先判斷加班時間〈kdtpOT_START〉是否存在，存再表示要按計算，所以要檢查
    * *********
    * 「前端網頁」「checkVal」中，〈switch (hidAllowCalcType.val())〉是〈字串〉不是〈數值〉
    * *********
    * 「DispalyTitle」中，當有加班時間時，顯示〈是(非)預加班 申請人 加班折換方式 加班時間 時數〉；反之，顯示〈預加班 申請人 加班折換方式 預加班時間〉
    * 「Message」中，當有加班時間時，顯示〈是(非)預加班 申請人 加班折換方式 加班時間 時數〉；反之，顯示〈預加班 申請人 加班折換方式 預加班時間〉
* **/

/**
* 修改時間：2019/04/11
* 修改人員：陳緯榕
* 修改項目：
    * 簽核時，計算按鈕會跑出來
    * DateTimePicker的輸入框是否能輸入由dll.config控制
* 修改位置：
    * 「kcbIsPre_CheckedChanged」中，〈btnCal〉要在〈STARTOV〉關卡才顯示
    * *********
    * 「SetField」中，當〈!Page.IsPostBack〉時，〈kdtpOT_START〉、〈kdtpOT_END〉、〈kdtpPreOT_START〉、〈kdtpPreOT_END〉的屬性〈TextBoxReadOnly〉由〈SCSHRConfiguration.IS_PICKER_READONLY〉控制
* **/

/**
* 修改時間：2019/04/10
* 修改人員：陳緯榕
* 修改項目：
    * 預加班，時間可否預設時間
    * 預加班填寫的時間，自動帶到加班時間
    * DISPLAY_TITLE 增加顯示預加班是/否
* 修改位置：
    * 「kcbIsPre_CheckedChanged」中，當起單時，如果選了預加班〈kdtpPreOT_START〉、〈kdtpPreOT_END〉都給預設時間；反之，如果沒選預加班就是〈kdtpOT_START〉、〈kdtpOT_END〉給預設時間
    * *********
    * 「kcbIsPre_CheckedChanged」中，當現在是簽核中，而且選了預加班，就判斷現在是〈STARTOV〉就將預加班的內容複製給加班時間
    * *********
    * 「DisplayTitle」中，由〈kcbIsPre〉來判斷要顯示是或否
    * 「Message」中，由〈kcbIsPre〉來判斷要顯示是或否
* **/

/**
* 修改時間：2019/04/08
* 修改人員：陳緯榕
* 修改項目：
    * 點預加班時，實際加班反灰，不可以填寫
* 修改位置：
    * 「kcbIsPre_CheckedChanged」中，使用DateTimePicker的屬性〈PickerVisible〉、〈PickerVisibleColor〉、〈PickerNormalColor〉來控制
* **/

/**
* 修改時間：2019/04/03
* 修改人員：陳緯榕
* 修改項目：
    * 當起單關，是否預加班被勾選時，不要顯示計算按鈕
* 修改位置：
    * 「kcbIsPre_CheckedChanged」中，當表單為起單時，〈kcbIsPre.Checked = true〉時，隱藏計算按鈕〈btnCal〉
* **/

/**
* 修改時間：2019/03/22
* 修改人員：陳緯榕
* 修改項目：
    * 是否預加班要能設定預設值
* 修改位置：
    * 「SetField」中，當表單為起單時，〈kcbIsPre〉由〈SCSHRConfiguration.DEFAULT_PRE〉來判斷要不要有值
* **/

/**
* 修改時間：2019/03/21
* 修改人員：陳緯榕
* 修改項目：
    * 預設時間錯誤
* 修改位置：
    * 「SetField」中，設定預設時間時，預設起始時間不應該塞迄；預設迄止時間不應該塞起
* **/

/**
* 修改時間：2019/03/20
* 修改人員：陳緯榕
* 修改項目：
    * 由旗標來判斷是否要檢查預加班是否符合加班時間
* 修改位置：
    * 「前端網頁」中，新增〈hidIS_CHECK_PRE_TIME〉物件
    * 「SetField」中，〈!Page.IsPostBack〉時，指定〈SCSHRConfiguration.IS_CHECK_PRE_TIME〉給〈hidIS_CHECK_PRE_TIME〉
    * 「前端網頁」「checkVal」中，當〈hidIS_CHECK_PRE_TIME.val() == "Y"〉時才檢查加班時間和預加班時間的合理性
* **/

/**
* 修改時間：2019/03/18
* 修改人員：陳緯榕
* 修改項目：
    * 修正剛起單時，表單的反應
    * 加上預設時間
* 修改位置：
    * 「SetField」中，需要判斷的是〈kytController.FieldValue〉為空字串
    * *******
    * 「SetField」中，當現在是〈起單狀態〉，從〈SCSHRConfiguration.FORM_DEFAULT_START_TIME〉和〈SCSHRConfiguration.FORM_DEFAULT_END_TIME〉取得預設時間，並填入起訖時間
* **/

/**
* 修改時間：2019/03/05
* 修改人員：陳緯榕
* 修改項目：
    * 加班時數有誤
* 發生原因：BOImport傳送RESTMINS的值為0
* 修改位置：
    * 「callWSBOImport」中，〈BOImport〉將〈RESTMINS〉註解不傳
* **/

/**
* 修改時間：2019/02/26
* 修改人員：陳緯榕
* 修改項目：
    * 刷卡時間顯示的是錯誤的
* 修改位置：
    * 「前端網頁」中，物件〈ktxtWorkDate〉首筆刷卡日期、〈ktxtWorkTime〉首筆刷卡時間、〈ktxtOffWorkDate〉尾筆刷卡日期、〈ktxtOffWorkTime〉尾筆刷卡時間修改為〈ktxtWorkPunch〉、〈ktxtOffWorkPunch〉
    * 「前端網頁」「checkVal」中，檢查刷卡時間改為檢查〈ktxtWorkPunch〉、〈ktxtOffWorkPunch〉
    * 「btnCal_Click」中，計算時數後，先取得〈WorkDate〉(時間型別)、〈WorkTime〉(4碼字串)、〈OffWorkDate〉(時間型別)、〈OffWorkTime〉(4碼字串)
        * 組合日期字串，刷卡日期取前10碼，刷卡時間index = 2插入〈:〉
        * 〈BOImport〉前，需要的參數使用方才組合的時間來切割
    * 「krblCHANGETYPE_SelectedIndexChanged」中，清空〈ktxtWorkPunch〉、〈ktxtOffWorkPunch〉
* **/

/**
* 修改時間：2019/02/25
* 修改人員：陳緯榕
* 修改項目：
    * 加班單要檢查刷卡時間，並且要能控制是否能檢查
    * 當簽核呼叫WS失敗時，前端網頁必須顯示「SCSHR未起單成功」
    * 飛騰加班單呼叫格式變更
* 修改位置：
    * 「前端網頁」中，新增物件〈hidIS_OV_CHK_PUNCH〉
    * 「前端網頁」「checkVal」中，檢查加班時間時，判斷是否檢查刷卡時間，並且判斷刷卡時間和請假時間之間是否合理
    * 「SetField」中，將〈hidIS_OV_CHK_PUNCH〉填入〈SCSHRConfiguration.IS_OV_CHK_PUNCH〉
    * *******
    * 「前端網頁」中，新增〈ktxtSignResult〉物件
    * *******
    * 「btnCal_Click」中，，增加〈DoOverType〉、〈WorkDate〉、〈WorkTime〉、〈OffWorkDate〉、〈OffWorkTime〉、〈MFreeDate〉、〈OverCompHours〉
* **/

/**
* 修改時間：2019/02/23
* 修改人員：陳緯榕
* 修改項目：
    * 飛騰更新WS
    * 加班時間不可相差24小時以上
* 修改位置：
    * 「前端網頁」中，新增〈ktxtOverCompHours〉補休時數、〈ktxtMFreeDate〉補休可休期限、〈ktxtWorkDate〉首筆刷卡日期、〈ktxtWorkTime〉首筆刷卡時間、〈ktxtOffWorkDate〉尾筆刷卡日期、〈ktxtOffWorkTime〉尾筆刷卡時間、〈ktxtDoOverType〉加班別名稱、〈hidDoOverType〉加班別代碼、〈hidAllowCalcType〉加班給付方式允許類型
    * 「前端網頁」「checkVal」中，新增檢查〈hidAllowCalcType〉，當加班給付方式允許類型和所選加班給付方式不同時不可送單
    * 「btnCal_Click」中，呼叫WS的〈BOExecFunc〉〈GetOverTimeHours〉時，新增參數〈CalcType〉
    * 「btnCal_Click」中，收到WS〈BOExecFunc〉回傳值後，將回傳的內容填入新增的物件中
    * *******
    * 「kdtpOT_START_TextChanged」中，當結束時間大於24小時時，將結束時間改為和起始時間相距24小時
    * 「kdtpOT_END_TextChanged」中，當結束時間大於24小時時，將結束時間改為和起始時間相距24小時
* **/

/**
* 修改時間：2019/01/30
* 修改人員：陳緯榕
* 修改項目：
    * 加班原因要隱藏
    * 本月、本日累積時數不可更改
* 修改位置：
    * 「前端網頁」中，隱藏〈加班原因〉所屬的兩個〈div〉區塊
    * 「SetField」中，〈ktxtApplyHoursTODAY〉、〈ktxtApplyHoursThemon〉設唯讀
* **/

/**
* 修改時間：2019/01/28
* 修改人員：陳緯榕
* 修改項目：
    * 新增預加班功能
    * 加班單日期按下檢查後再更改，需設定在按一次檢查才可送單
    * 加班單事由須設定必填
* 修改位置：
    * 「前端網頁」中，新增〈預加班〉區塊，當物件〈kcbIsPre〉被選取時，顯示預加班區塊
    * 「前端網頁」「checkVal」中，當現在不是預加班送出關卡時，先檢查加班時間(起)，再檢查預加班(起)的Label是否存在，存在就檢查〈加班時間(起)是否小於預加班時間(起)〉
    * 「前端網頁」「checkVal」中，當現在不是預加班送出關卡時，先檢查加班時間(迄)，再檢查預加班(迄)的Label是否存在，存在就檢查〈加班時間(迄)是否小於預加班時間(迄)〉
    * 「前端網頁」「checkVal」中，能夠檢查加班時數時，同時檢查是否有詢問API，並且得到正確結果
    * 「新增方法」〈kcbIsPre_CheckedChanged〉變更是否預加班事件
    * 「SetField」中，當表單簽核關卡是〈STARTOV〉時，顯示加班區塊，並且在最後判斷預加班的選擇按鈕是否顯示文字
    * 「ConditionValue」中，新增〈IsPreOT〉屬性，以物件〈kcbIsPre〉來判斷
    * ****
    * 「新增方法」〈kdtpPreOT_START_TextChanged〉預加班時間(起)變更事件、〈kdtpOT_START_TextChanged〉加班時間(起)變更事件、〈kdtpOT_END_TextChanged〉加班時間(迄)變更事件
    * ****
    * 「前端網頁」「checkVal」中，新增檢查〈ktxtNOTE〉
* **/

/**
* 修改時間：2019/01/26
* 修改人員：陳緯榕
* 修改項目：
    * 表單無法送出
* 修改位置：
    * 「前端網頁」中，刪除〈kdtpOT_STA〉、〈kdtpOT_END_Selected〉、〈CheckREMARKOther〉事件
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
* 修改時間：2018/12/04
* 修改人員：陳緯榕
* 修改項目：
    * 重複送單卻不會擋單
* 修改位置：
    * 「checkVal」檢查是否有重複，改用〈 (CANCEL_DOC_NBR = '' OR CANCEL_DOC_NBR IS NULL)〉
* **/

/**
* 修改時間：2018/12/03
* 修改人員：陳緯榕
* 修改項目：加班內容說明改為非必填
* 修改位置：
    * 「前端網頁」〈checkNote〉方法註解掉
    * 「前端網頁」加班內容的〈CustomValidator〉註解掉
* **/

/**
* 修改時間：2018/09/21
* 修改人員：陳緯榕
* 修改項目：部門進行兼職切換時，基本訊息也要切換
* 修改位置：
    * 新增「setDefaultUserInfo」方法
    * 「SetField」如果是剛起單狀態，就算是postback也要更新基本訊息
* **/

/// <summary>
/// 飛騰加班資訊
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_OVERTIME : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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

    /// <summary>
    /// 表單單號
    /// </summary>
    public string FormNumber;

    /// <summary>
    /// 當前的FieldValue
    /// </summary>
    string my_FieldValue = "";

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
             .Add("GROUPCODE", hidGROUPCODE.Value)
             .Add("changetype", krblCHANGETYPE.SelectedValue)
             .Add("OverTimeHours", ktxtOverTimeHours.Text)
             .Add("IsPreOT", kcbIsPre.Checked ? "Y" : "N")
             .Add("DoOverType", ktxtDoOverType.Text)
             .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME.ConditionValue.cv:{0}", cv));
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
            string message = "";
            if (!string.IsNullOrEmpty(kdtpOT_START.Text))
                message = string.Format(@"加班時間{2}~{3}，時數{4}；加班折換方式：{1}；{0}", ktxtAPPLICANT.Text, krblCHANGETYPE.SelectedIndex > -1 ? krblCHANGETYPE.SelectedItem.Text : "未選擇", kdtpOT_START.Text, kdtpOT_END.Text, ktxtOverTimeHours.Text);
            else
                message = string.Format(@"預加班時間{2}~{3}；加班折換方式：{1}；{0}", ktxtAPPLICANT.Text, krblCHANGETYPE.SelectedIndex > -1 ? krblCHANGETYPE.SelectedItem.Text : "未選擇", kdtpPreOT_START.Text, kdtpPreOT_END.Text);
            return message;
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
            string message = "";
            if (!string.IsNullOrEmpty(kdtpOT_START.Text))
                message = string.Format(@"加班時間{2}~{3}，時數{4}；加班折換方式：{1}；{0}", ktxtAPPLICANT.Text, krblCHANGETYPE.SelectedIndex > -1 ? krblCHANGETYPE.SelectedItem.Text : "未選擇", kdtpOT_START.Text, kdtpOT_END.Text, ktxtOverTimeHours.Text);
            else
                message = string.Format(@"預加班時間{2}~{3}；加班折換方式：{1}；{0}", ktxtAPPLICANT.Text, krblCHANGETYPE.SelectedIndex > -1 ? krblCHANGETYPE.SelectedItem.Text : "未選擇", kdtpPreOT_START.Text, kdtpPreOT_END.Text);
            return message;
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
            return String.Empty;
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

            if (!Page.IsPostBack) // 網頁首次載入
            {
                my_FieldValue = fieldOptional.FieldValue;
                if (!string.IsNullOrEmpty(fieldOptional.FieldValue))
                    kytController.FieldValue = fieldOptional.FieldValue;

                kytController.SetAllViewType(KYTViewType.ReadOnly); // 設定所有KYT物件唯讀
                btnCal.Visible = false; // 隱藏計算
                rbtnFindOVERTIME.Visible = false; // 隱藏加班狀況
                btnAPPLICANT.Visible = false; // 隱藏申請人
                hidIS_OV_CHK_PUNCH.Value = SCSHRConfiguration.IS_OV_CHK_PUNCH;
                hidIS_CHECK_PRE_TIME.Value = SCSHRConfiguration.IS_CHECK_PRE_TIME;
                div1.Visible = false; // 是否場外加班區塊
                ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值

                bool is_show_pre = SCSHRConfiguration.IS_OVH_PRE_SHOW.ToUpper() == "Y";
                divPre.Visible = is_show_pre;
                divPreTitle.Visible = is_show_pre;
                if (is_show_pre)
                {
                    if (string.IsNullOrEmpty(my_FieldValue))
                    {
                        // 加班單預設預加班時間
                        DateTime dtDefaultStart = DateTime.MinValue;
                        DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_START_TIME), out dtDefaultStart);
                        if (dtDefaultStart > DateTime.MinValue)
                        {
                            kdtpPreOT_START.Text = dtDefaultStart.ToString("yyyy/MM/dd HH:mm");
                        }
                        DateTime dtDefaultEnd = DateTime.MinValue;
                        DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_END_TIME), out dtDefaultEnd);
                        if (dtDefaultEnd > DateTime.MinValue)
                        {
                            kdtpPreOT_END.Text = dtDefaultEnd.ToString("yyyy/MM/dd HH:mm");
                        }
                    }
                }

                DateTime dtStart = DateTime.Now;
                if (kcbIsPre.Checked) // 是否預加班
                {
                    DateTime.TryParse(kdtpPreOT_START.Text, out dtStart);
                }
                else
                {
                    DateTime.TryParse(kdtpOT_START.Text, out dtStart);
                }
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        hidAPIResult.Value = ""; // 清掉先前的旗標
                        kytController.SetAllViewType(KYTViewType.Input); // 設定所有KYT物件可輸入
                        ktxtOT_CLASSTYPE_NAME.ViewType = KYTViewType.ReadOnly;
                        ktxtAPPLICANTDEPT.ReadOnly = true; // 部門唯讀
                        ktxtAPPLICANT.ReadOnly = true; // 申請人唯讀
                        ktxtApplyHours.ReadOnly = true; // 申請時數唯讀
                        ktxtOverTimeHours.ReadOnly = true; // 加班時數唯讀
                        ktxtApplyHoursTODAY.ReadOnly = true; // 本日累積時數唯讀
                        ktxtApplyHoursThemon.ReadOnly = true; // 本月累積時數唯讀

                        btnCal.Visible = true; // 顯示計算
                        rbtnFindOVERTIME.Visible = true; // 顯示加班狀況
                        //btnAPPLICANT.Visible = true; // 顯示申請人

                        if (fieldOptional.FieldMode == FieldMode.Applicant &&
                             string.IsNullOrEmpty(fieldOptional.FieldValue)) // 起單時專用操作狀態或初始值
                        {
                            //setDefaultUserInfo(this.ApplicantGuid, this.ApplicantGroupId);
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                            //hidAPPLICANTDEPTName.Value = JGlobalLibs.UOFUtils.GetGroupNameByDepartmentID(ApplicantGroupId); // 部門名稱
                            //ktxtAPPLICANTDEPT.Text = hidAPPLICANTDEPTName.Value;
                            ktxtAPPLICANTDEPT.Text = JGlobalLibs.UOFUtils.GetGroupNameByDepartmentID(ApplicantGroupId); // 部門名稱
                            hidAPPLICANTDEPT.Value = ApplicantGroupId; // 部門代碼
                            ktxtAPPLICANT.Text = string.Format(@"{0} ({1})", KUser.Name, KUser.Account); // 申請人姓名
                            hidAPPLICANTGuid.Value = KUser.UserGUID; // 申請人代碼
                            hidAPPLICANT.Value = KUser.Account; // 申請人帳號
                            string[] sAccount = hidAPPLICANT.Value.Split('\\');
                            hidAPPLICANT.Value = sAccount[sAccount.Length - 1];
                            hidGROUPCODE.Value = JGlobalLibs.UOFUtils.GetGroupCodeByDepartmentID(ApplicantGroupId);
                            hidCompanyNo.Value = KUser.CompanyNo;
                            hidAPPLICANTDATE.Value = DateTime.Now.ToString("yyyy/MM/dd"); // 申請日期
                            DataTable dtSource = getOVERTIMEType(SCSHRConfiguration.SCSSOverTimeTypeProgID, "SYS_ViewID,SYS_Name,SYS_EngName");
                            kddlREMARK.DataSource = dtSource;
                            kddlREMARK.DataValueField = "SYS_VIEWID";
                            kddlREMARK.DataTextField = "SYS_NAME";
                            kddlREMARK.DataBind();
                            //kcbIsPre.Checked = true;
                            reBindCHANGETYPE();
                            // 當日已加班時數
                            ktxtApplyHoursTODAY.Text = getOverTimeHours(hidAPPLICANT.Value, DateTime.Now.Date, DateTime.Now.Date, false).ToString();
                            // 當月已加班時數
                            ktxtApplyHoursThemon.Text = getOverTimeHours(hidAPPLICANT.Value,
                                    new DateTime(int.Parse(DateTime.Now.ToString().Substring(0, 4)), DateTime.Now.Month, 1),
                                    new DateTime(int.Parse(DateTime.Now.ToString().Substring(0, 4)), DateTime.Now.Month, 1).AddMonths(1).AddDays(-1),
                                    true).ToString();

                            // 加班單預設加班時間
                            DateTime dtDefaultStart = DateTime.MinValue;
                            DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_START_TIME), out dtDefaultStart);
                            if (dtDefaultStart > DateTime.MinValue)
                            {
                                kdtpOT_START.Text = dtDefaultStart.ToString("yyyy/MM/dd HH:mm");
                            }
                            DateTime dtDefaultEnd = DateTime.MinValue;
                            DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_END_TIME), out dtDefaultEnd);
                            if (dtDefaultEnd > DateTime.MinValue)
                            {
                                kdtpOT_END.Text = dtDefaultEnd.ToString("yyyy/MM/dd HH:mm");
                            }
                            kcbIsPre.Checked = SCSHRConfiguration.DEFAULT_PRE.ToUpper() == "Y"; // 是否預加班預設值
                        }
                        kddlREMARK_SelectedIndexChanged(kddlREMARK, null);
                        // 設定Picker是否能輸入
                        kdtpOT_START.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpOT_END.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpPreOT_END.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpPreOT_START.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";

                        // 當日已加班時數
                        ktxtApplyHoursTODAY.Text = getOverTimeHours(hidAPPLICANT.Value, dtStart.Date, dtStart.Date, false).ToString();
                        // 當月已加班時數
                        ktxtApplyHoursThemon.Text = getOverTimeHours(hidAPPLICANT.Value,
                               new DateTime(int.Parse(dtStart.ToString().Substring(0, 4)), dtStart.Month, 1),
                               new DateTime(int.Parse(dtStart.ToString().Substring(0, 4)), dtStart.Month, 1).AddMonths(1).AddDays(-1),
                               true).ToString();
                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Print: // 表單列印
                        break;
                    case FieldMode.Signin: // 表單簽核
                        if (taskObj.CurrentSite.SiteCode == "STARTOV") // 當關卡是STARTOV時，表示現在是預加班後要起加班單
                        {
                            hidAPIResult.Value = ""; // 清掉先前的旗標
                            kytController.SetAllViewType(KYTViewType.Input); // 設定所有KYT物件可輸入
                            //krblCHANGETYPE.ViewType = KYTViewType.ReadOnly; // 給付方式不可更動
                            ktxtOT_CLASSTYPE_NAME.ViewType = KYTViewType.ReadOnly;
                            ktxtAPPLICANTDEPT.ReadOnly = true; // 部門唯讀
                            ktxtAPPLICANT.ReadOnly = true; // 申請人唯讀
                            ktxtApplyHours.ReadOnly = true; // 申請時數唯讀
                            ktxtOverTimeHours.ReadOnly = true; // 加班時數唯讀
                            ktxtApplyHoursTODAY.ReadOnly = true; // 本日累積時數唯讀
                            ktxtApplyHoursThemon.ReadOnly = true; // 本月累積時數唯讀
                            btnCal.Visible = true; // 顯示計算
                            kcbIsPre.ViewType = KYTViewType.ReadOnly; // 預加班唯讀
                            kdtpPreOT_START.ViewType = KYTViewType.ReadOnly; // 預加班(起)不可更動
                            kdtpPreOT_END.ViewType = KYTViewType.ReadOnly; // 預加班(迄)不可更動
                            // 重新綁定假別選單
                            DataTable dtSource = getOVERTIMEType(SCSHRConfiguration.SCSSOverTimeTypeProgID, "SYS_ViewID,SYS_Name,SYS_EngName");
                            kddlREMARK.DataSource = dtSource;
                            kddlREMARK.DataValueField = "SYS_VIEWID";
                            kddlREMARK.DataTextField = "SYS_NAME";
                            kddlREMARK.DataBind();
                            // 重新計算當日已加班時數
                            // 當日已加班時數
                            ktxtApplyHoursTODAY.Text = getOverTimeHours(hidAPPLICANT.Value, dtStart.Date, dtStart.Date, false).ToString();
                            // 重新計算當月已加班時數
                            //DateTime dtStart = DateTime.Now;
                            //DateTime.TryParse(kdtpPreOT_START.Text, out dtStart);
                            ktxtApplyHoursThemon.Text = getOverTimeHours(hidAPPLICANT.Value,
                                   new DateTime(int.Parse(dtStart.ToString().Substring(0, 4)), dtStart.Month, 1),
                                   new DateTime(int.Parse(dtStart.ToString().Substring(0, 4)), dtStart.Month, 1).AddMonths(1).AddDays(-1),
                                   true).ToString();
                            kddlREMARK_SelectedIndexChanged(kddlREMARK, null);
                            kdtpOT_START.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                            kdtpOT_END.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";

                        }
                        FormNumber = taskObj.FormNumber;
                        break;
                    case FieldMode.Verify: // Verify
                        break;
                    case FieldMode.View: // 表單觀看
                        break;
                }
                Dialog.Open2(rbtnFindOVERTIME,
                    string.Format(@"~/CDS/SCSHR/WKFFields/QUERYWINDOWS/findOVERTIME.aspx"),
                    "已加班時數查詢 (顯示前後30天範圍內的所有加班單)",
                    800,
                    600,
                    Dialog.PostBackType.AfterReturn,
                    new { ACCOUNT = hidAPPLICANT.Value, START_DATE = dtStart.Date }.ToExpando());
                //bool is_show_pre = SCSHRConfiguration.IS_OVH_PRE_SHOW.ToUpper() == "Y";
                divPre.Visible = is_show_pre;
                divPreTitle.Visible = is_show_pre;
                if (is_show_pre)
                {
                    kcbIsPre_CheckedChanged(kcbIsPre, null);
                    kcbIsPre.Text = kcbIsPre.ViewType == KYTViewType.ReadOnly ? kcbIsPre.Checked ? "是" : "否" : "";
                }

                ktxtSignResult.ViewType = KYTViewType.ReadOnly;
                kcbIsPre.Text = kcbIsPre.ViewType == KYTViewType.ReadOnly ? kcbIsPre.Checked ? "是" : "否" : "";

                //kdtpOT_START.Text = "2018/01/29 00:00";
                //kdtpOT_END.Text = "2018/01/29 00:00";
                //hidAPIResult.Value = "OK";
                //ktxtOverTimeHours.Text = "1";
                //ktxtApplyHours.Text = "1";
            }
            else // 如果網頁POSTBACK
            {
                JGlobalLibs.WebUtils.RequestHiddenFields(UpdatePanel1); // 取回HiddenField的值

                if (this.FormFieldMode == FieldMode.Applicant) // 如果是剛起單
                {
                    //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                    KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
                    //hidAPPLICANTDEPTName.Value = JGlobalLibs.UOFUtils.GetGroupNameByDepartmentID(ApplicantGroupId); // 部門名稱
                    //ktxtAPPLICANTDEPT.Text = hidAPPLICANTDEPTName.Value;
                    ktxtAPPLICANTDEPT.Text = JGlobalLibs.UOFUtils.GetGroupNameByDepartmentID(ApplicantGroupId); // 部門名稱
                    hidAPPLICANTDEPT.Value = ApplicantGroupId; // 部門代碼
                    ktxtAPPLICANT.Text = KUser.Name; // 申請人姓名
                    hidAPPLICANTGuid.Value = KUser.UserGUID; // 申請人代碼
                    hidAPPLICANT.Value = KUser.Account; // 申請人帳號
                    string[] sAccount = hidAPPLICANT.Value.Split('\\');
                    hidAPPLICANT.Value = sAccount[sAccount.Length - 1];
                    hidGROUPCODE.Value = JGlobalLibs.UOFUtils.GetGroupCodeByDepartmentID(ApplicantGroupId);
                    hidCompanyNo.Value = KUser.CompanyNo;
                    //setDefaultUserInfo(this.ApplicantGuid, this.ApplicantGroupId);
                }
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:
                        // 設定Picker是否能輸入
                        kdtpOT_START.ViewType = KYTViewType.Input;
                        kdtpOT_END.ViewType = KYTViewType.Input;
                        kdtpPreOT_END.ViewType = KYTViewType.Input;
                        kdtpPreOT_START.ViewType = KYTViewType.Input;
                        kdtpOT_START.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpOT_END.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpPreOT_END.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        kdtpPreOT_START.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        if (kcbIsPre.Checked)
                        {
                            btnCal.Visible = false;
                            kdtpOT_START.PickerVisible = false;
                            kdtpOT_END.PickerVisible = false;
                            kdtpOT_START.Text = "";
                            kdtpOT_END.Text = "";
                        }
                        break;
                    case FieldMode.Signin: // 表單簽核
                        if (taskObj.CurrentSite.SiteCode == "STARTOV") // 當關卡是STARTOV時，表示現在是預加班後要起加班單
                        {
                            kdtpOT_START.ViewType = KYTViewType.Input;
                            kdtpOT_END.ViewType = KYTViewType.Input;
                            kdtpOT_START.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                            kdtpOT_END.TextBoxReadOnly = SCSHRConfiguration.IS_PICKER_READONLY.ToUpper() == "Y";
                        }
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

    /// <summary>
    /// 設定部門基本訊息
    /// </summary>
    /// <param name="ApplicantGuid"></param>
    /// <param name="ApplicantGroupId"></param>
    private void setDefaultUserInfo(string ApplicantGuid, string ApplicantGroupId)
    {
        //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
        KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
        //hidAPPLICANTDEPTName.Value = JGlobalLibs.UOFUtils.GetGroupNameByDepartmentID(ApplicantGroupId); // 部門名稱
        //ktxtAPPLICANTDEPT.Text = hidAPPLICANTDEPTName.Value;
        ktxtAPPLICANTDEPT.Text = JGlobalLibs.UOFUtils.GetGroupNameByDepartmentID(ApplicantGroupId); // 部門名稱
        hidAPPLICANTDEPT.Value = ApplicantGroupId; // 部門代碼
        ktxtAPPLICANT.Text = KUser.Name; // 申請人姓名
        hidAPPLICANTGuid.Value = KUser.UserGUID; // 申請人代碼
        hidAPPLICANT.Value = KUser.Account; // 申請人帳號
        string[] sAccount = hidAPPLICANT.Value.Split('\\');
        hidAPPLICANT.Value = sAccount[sAccount.Length - 1];
        hidGROUPCODE.Value = JGlobalLibs.UOFUtils.GetGroupCodeByDepartmentID(ApplicantGroupId);
        hidCompanyNo.Value = KUser.CompanyNo;
    }

    private void reBindCHANGETYPE()
    {
        bool isTIMEOFF = false;
        bool isPAYMENT = false;
        bool isBOTH_TP = false;
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT * 
              FROM TB_EIP_DUTY_SETTING_OVERTIME_HOURS 
             WHERE GROUP_GUID = 'Company' 
               AND TYPE = 'Workday'
            ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("", "");
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    isTIMEOFF = (bool)dr["ALLOW_TIMEOFF"];
                    isPAYMENT = (bool)dr["ALLOW_PAYMENT"];
                    isBOTH_TP = (bool)dr["ALLOW_BOTH_TP"];
                }
            }
            catch (Exception e)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME.reBindCHANGETYPE.ERROR:{0}", e.Message));
            }
        }

        if (isTIMEOFF && isPAYMENT)
        {
            JGlobalLibs.WebUtils.DynamicListItemBind(krblCHANGETYPE, new object[] {
                new { Text= "加班費", Value= "0" },
                new { Text= "補休", Value= "1" }
            });
        }
        else if (isTIMEOFF)
        {
            JGlobalLibs.WebUtils.DynamicListItemBind(krblCHANGETYPE, new object[] {
                new { Text= "補休", Value= "1" }
            });
        }
        else if (isPAYMENT)
        {
            JGlobalLibs.WebUtils.DynamicListItemBind(krblCHANGETYPE, new object[] {
                new { Text= "加班費", Value= "0" }
            });
        }
        else if (isBOTH_TP)
        {
            JGlobalLibs.WebUtils.DynamicListItemBind(krblCHANGETYPE, new object[] {
                new { Text= "加班費及補休", Value= "2" }
            });
        }
        int defaultType = 1;
        int.TryParse(SCSHRConfiguration.DEFAULT_OV_PE, out defaultType);
        krblCHANGETYPE.SelectedValue = defaultType.ToString(); // 預設由設定檔取得

    }

    /// <summary>
    /// 計算時間範圍內的加班總時數
    /// </summary>
    /// <param name="account"></param>
    /// <param name="dtStart"></param>
    /// <param name="dtOff"></param>
    /// <param name="IsGetMonth"></param>
    /// <returns></returns>
    private decimal getOverTimeHours(string account, DateTime dtStart, DateTime dtOff, bool IsGetMonth)
    {
        decimal totalHours = 0;
        DebugLog.Log(DebugLog.LogLevel.DetailInfo, string.Format(@"UC_KYTI_SCSHR_OVERTIME.getOverTimeHours.account:{0}；dtStart:{1}；dtOff:{2}", account, dtStart, dtOff));
        //using (SqlDataAdapter sda = new SqlDataAdapter(@"
        //    -- 找到時間範圍內的加班單
        //    SELECT ISNULL(SUM(OT_TIMES), 0)  AS 'TOTALHOURS'
        //      FROM Z_SCSHR_OVERTIME 
        //     WHERE APPLICANT = @ACCOUNT
        //       AND (OT_START BETWEEN @START 
        //                         AND @OFF
        //            OR OT_END BETWEEN @START 
        //                          AND @OFF)
        //       AND ((TASK_STATUS = 1 AND TASK_RESULT IS NULL)
        //            OR (TASK_STATUS = 2 AND TASK_RESULT = 0))
        //       AND (CANCEL_DOC_NBR IS NULL OR CANCEL_DOC_NBR = '')

        //    ", ConnectionString))
        //using (DataSet ds = new DataSet())
        //{
        //    sda.SelectCommand.Parameters.AddWithValue("@ACCOUNT", account);
        //    sda.SelectCommand.Parameters.AddWithValue("@START", dtStart);
        //    sda.SelectCommand.Parameters.AddWithValue("@OFF", dtOff);
        //    try
        //    {
        //        if (sda.Fill(ds) > 0)
        //        {
        //            totalHours = (decimal)ds.Tables[0].Rows[0]["TOTALHOURS"];
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME.getOverTimeHours::TimeRange::ERROR:{0}", e.Message));
        //    }
        //}
        if (!IsGetMonth) // 計算時間起(當日)時數
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
            -- 找到時間範圍內的加班單
            SELECT ISNULL(SUM(OT_TIMES), 0)  AS 'TOTALHOURS'
              FROM Z_SCSHR_OVERTIME 
             WHERE APPLICANT = @ACCOUNT
               AND DATEPART(yyyy,OT_START) = DATEPART(yyyy, @START)
               AND DATEPART(mm,OT_START) = DATEPART(mm, @START)
               AND DATEPART(dd,OT_START) = DATEPART(dd, @START)
               AND ((TASK_STATUS = 1 AND TASK_RESULT IS NULL)
                    OR (TASK_STATUS = 2 AND TASK_RESULT = 0))
               AND (CANCEL_DOC_NBR IS NULL OR CANCEL_DOC_NBR = '')

            ", ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@ACCOUNT", account);
                sda.SelectCommand.Parameters.AddWithValue("@START", dtStart);
                sda.SelectCommand.Parameters.AddWithValue("@OFF", dtOff);
                try
                {
                    if (sda.Fill(ds) > 0)
                    {
                        totalHours = (decimal)ds.Tables[0].Rows[0]["TOTALHOURS"];
                    }
                }
                catch (Exception e)
                {
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME.getOverTimeHours::TimeRange::ERROR:{0}", e.Message));
                }
            }
        }
        else // 計算時間起，當月份總時數
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(@"
            -- 找到時間範圍內的加班單
            SELECT ISNULL(SUM(OT_TIMES), 0)  AS 'TOTALHOURS'
              FROM Z_SCSHR_OVERTIME 
             WHERE APPLICANT = @ACCOUNT
               AND DATEPART(yyyy,OT_START) = DATEPART(yyyy, @START)
               AND DATEPART(mm,OT_START) = DATEPART(mm, @START)
               AND ((TASK_STATUS = 1 AND TASK_RESULT IS NULL)
                    OR (TASK_STATUS = 2 AND TASK_RESULT = 0))
               AND (CANCEL_DOC_NBR IS NULL OR CANCEL_DOC_NBR = '')

            ", ConnectionString))
            using (DataSet ds = new DataSet())
            {
                sda.SelectCommand.Parameters.AddWithValue("@ACCOUNT", account);
                sda.SelectCommand.Parameters.AddWithValue("@START", dtStart);
                sda.SelectCommand.Parameters.AddWithValue("@OFF", dtOff);
                try
                {
                    if (sda.Fill(ds) > 0)
                    {
                        totalHours = (decimal)ds.Tables[0].Rows[0]["TOTALHOURS"];
                    }
                }
                catch (Exception e)
                {
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME.getOverTimeHours::month::ERROR:{0}", e.Message));
                }
            }
        }
        return totalHours;
    }

    private DataTable getOVERTIMEType(string progId, string selectFields)
    {
        DataTable dtScource = new DataTable();
        Exception ex = null;
        dtScource = service.BOFind(progId, selectFields, out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME.getLEAVEType.service.Error:{0}", ex.Message));

        return dtScource;
    }

    protected void kddlREMARK_SelectedIndexChanged(object sender, EventArgs e)
    {
        KYTDropDownList kddlREMARK = (KYTDropDownList)sender;
        ktxtREMARKOther.Visible = false;
        ktxtREMARKOther.Text = "";
        if (kddlREMARK.SelectedValue == "5")
            ktxtREMARKOther.Visible = true;
    }

    protected void btnCal_Click(object sender, EventArgs e)
    {
        ktxtApplyHours.Text = "0";
        ktxtOverTimeHours.Text = "0";
        hidAPIResult.Value = "";
        lblOT_TIMESMSG.Text = "";
        //hidC_DAY.Value = "";
        hidStyle.Value = "";
        hidConfirm.Value = "";
        if (!string.IsNullOrEmpty(kdtpOT_END.Text) &&
            !string.IsNullOrEmpty(kdtpOT_START.Text))
        {
            DataTable dtCalcResult = null;
            DateTime dtStart = DateTime.MinValue;
            DateTime dtEnd = DateTime.MinValue;
            DateTime.TryParse(kdtpOT_START.Text, out dtStart);
            DateTime.TryParse(kdtpOT_END.Text, out dtEnd);
            string DoOverType = "";

            Exception ex = null;
            #region 計算時數
            SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc(SCSHRConfiguration.SCSSGetOverTimeHoursProdID,
            "GetOverTimeHours",
            SCSHR.Types.SCSParameter.getPatameters(
                new
                {
                    TMP_EmployeeID = hidAPPLICANT.Value,
                    StartDate = dtStart.ToString("yyyyMMdd"),
                    StartTime = dtStart.ToString("HHmm"),
                    EndDate = dtEnd.ToString("yyyyMMdd"),
                    EndTime = dtEnd.ToString("HHmm"),
                    CalcType = krblCHANGETYPE.SelectedValue
                }),
            out ex);
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME.btnCal_Click.BOExecFunc.GetOverTimeHours.計算時間.Parameter:{0}", new
            {
                TMP_EmployeeID = hidAPPLICANT.Value,
                StartDate = dtStart.ToString("yyyyMMdd"),
                StartTime = dtStart.ToString("HHmm"),
                EndDate = dtEnd.ToString("yyyyMMdd"),
                EndTime = dtEnd.ToString("HHmm"),
                CalcType = krblCHANGETYPE.SelectedValue
            }.ToString()));

            if (ex != null)
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME.btnCal_Click.GetOverTimeHours.計算時間.ERROR:{0}", ex.Message));
            if (parameters != null &&
                parameters.Length > 0)
            {
                if (parameters[0].DataType.ToString() == "DataTable")
                {
                    dtCalcResult = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                }
            }
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME.btnCal_Click.BOExecFunc.GetOverTimeHours.計算時間.Result:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtCalcResult)));
            if (dtCalcResult != null &&
                dtCalcResult.Rows.Count > 0)
            {
                // 填入申請時數、加班時數
                ktxtApplyHours.Text = dtCalcResult.Rows[0]["ApplyHours"].ToString();
                ktxtOverTimeHours.Text = dtCalcResult.Rows[0]["OverTimeHours"].ToString();
                hidOT_CLASSTYPE.Value = dtCalcResult.Rows[0]["TMP_WorkID"].ToString();
                ktxtOT_CLASSTYPE_NAME.Text = dtCalcResult.Rows[0]["TMP_WorkName"].ToString();

                ktxtOverCompHours.Text = dtCalcResult.Columns.Contains("OverCompHours") ? dtCalcResult.Rows[0]["OverCompHours"].ToString() : ""; // 實際補休時數
                ktxtMFreeDate.Text = dtCalcResult.Columns.Contains("MFreeDate") ? dtCalcResult.Rows[0]["MFreeDate"].ToString() : ""; // 補休可休期限

                string WorkDate = dtCalcResult.Columns.Contains("WorkDate") ? dtCalcResult.Rows[0]["WorkDate"].ToString() : ""; // 首筆刷卡日期
                string WorkTime = dtCalcResult.Columns.Contains("WorkTime") ? dtCalcResult.Rows[0]["WorkTime"].ToString() : ""; //首筆刷卡時間
                if (WorkDate.Length > 10)
                    ktxtWorkPunch.Text = string.Format(@"{0} {1}", WorkDate.Substring(0, 10), WorkTime.Insert(2, ":")); // 組合出時間

                string OffWorkDate = dtCalcResult.Columns.Contains("OffWorkDate") ? dtCalcResult.Rows[0]["OffWorkDate"].ToString() : ""; // 尾筆刷卡日期
                string OffWorkTime = dtCalcResult.Columns.Contains("OffWorkTime") ? dtCalcResult.Rows[0]["OffWorkTime"].ToString() : ""; // 尾筆刷卡時間
                if (OffWorkDate.Length > 10)
                    ktxtOffWorkPunch.Text = string.Format(@"{0} {1}", OffWorkDate.Substring(0, 10), OffWorkTime.Insert(2, ":")); // 組合出時間

                hidDoOverType.Value = dtCalcResult.Columns.Contains("DoOverType") ? dtCalcResult.Rows[0]["DoOverType"].ToString() : ""; // 加班別
                hidAllowCalcType.Value = dtCalcResult.Columns.Contains("AllowCalcType") ? dtCalcResult.Rows[0]["AllowCalcType"].ToString() : ""; // 加班給付方式允許類型
                ktxtDoOverType.Text = hidDoOverType.Value == "0" ? "平日" : hidDoOverType.Value == "1" ? "休息日" : hidDoOverType.Value == "2" ? "特殊日" : hidDoOverType.Value == "3" ? "例假日" : hidDoOverType.Value == "4" ? "變形休息日" : hidDoOverType.Value == "5" ? "國定假日" : "";
            }
            #endregion 計算時數

            if (!string.IsNullOrEmpty(hidOT_CLASSTYPE.Value))
            {
                DateTime dtStartPunch = DateTime.MinValue;
                DateTime.TryParse(ktxtWorkPunch.Text.Trim(), out dtStartPunch);
                DateTime dtOffPunch = DateTime.MinValue;
                DateTime.TryParse(ktxtOffWorkPunch.Text.Trim(), out dtOffPunch);

                if (taskObj != null &&
                    !string.IsNullOrEmpty(taskObj.FormNumber) && // 如果有表單單號
                    taskObj.CurrentSite.SiteCode == "STARTOV") // 而且關卡代號是STARTOV
                {
                    // 確認表單是否有在飛騰建立過表單
                    Exception _oex = null;
                    List<SCSHR.net.azurewebsites.scsservices_beta.FilterItem> lsItems = new List<SCSHR.net.azurewebsites.scsservices_beta.FilterItem>();
                    SCSHR.net.azurewebsites.scsservices_beta.FilterItem item = new SCSHR.net.azurewebsites.scsservices_beta.FilterItem();
                    item.FieldName = "SYS_VIEWID";
                    item.FilterValue = taskObj.FormNumber;
                    lsItems.Add(item);
                    DataTable dtFind = service.BOFind(SCSHRConfiguration.SCSSOverTimeProgID, "*", lsItems.ToArray(), out _oex);
                    _oex = null;
                    if (_oex != null)
                    {

                        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME.btnCal_Click.DELETE.查詢表單單號: {0}:: 失敗::{1}", taskObj.FormNumber, _oex.Message));
                    }
                    if (dtFind.Rows.Count > 0) // 如果有在飛騰找到這張表單，表示現在是退回到能夠申請請假單的關卡，所以刪除舊單
                    {
                        _oex = null;
                        service.BODelete(SCSHRConfiguration.SCSSOverTimeProgID, taskObj.FormNumber, false, true, out _oex);
                        if (_oex != null)
                        {
                            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME.btnCal_Click.DELETE::BODelete::StackTrace::{0}::ERROR::{1}", _oex.StackTrace, _oex.Message));
                        }
                    }
                }

                bool resultStatus = false;
                // 檢核資料
                JArray jaTable = new JArray();
                JObject _joTable = new JObject();
                _joTable.Add(new JProperty("USERNO", "1"));
                _joTable.Add(new JProperty("SYS_VIEWID", ""));
                _joTable.Add(new JProperty("SYS_DATE", DateTime.Parse(hidAPPLICANTDATE.Value).ToString("yyyyMMdd"))); // 日期
                //_joTable.Add(new JProperty("TMP_EMPLOYEEID", hidAPPLICANT.Value)); // 加班人員
                _joTable.Add(new JProperty("TMP_EMPLOYEEID", new KYT_UserPO().GetUserDetailByAccount(hidAPPLICANT.Value).EmployeeNo)); // 加班人員
                _joTable.Add(new JProperty("STARTDATE", dtStart.ToString("yyyyMMdd"))); // 起始日期
                _joTable.Add(new JProperty("STARTTIME", dtStart.ToString("HHmm"))); // 起始時間
                _joTable.Add(new JProperty("ENDTIME", dtEnd.ToString("HHmm"))); // 結束時間
                _joTable.Add(new JProperty("DoOverType", hidDoOverType.Value)); // 加班別
                _joTable.Add(new JProperty("WorkDate", dtStartPunch.ToString("yyyyMMdd"))); // 首筆刷卡日期
                _joTable.Add(new JProperty("WorkTime", dtStartPunch.ToString("HHmm"))); // 首筆刷卡時間
                _joTable.Add(new JProperty("OffWorkDate", dtOffPunch.ToString("yyyyMMdd"))); // 尾筆刷卡日期
                _joTable.Add(new JProperty("OffWorkTime", dtOffPunch.ToString("HHmm"))); // 尾筆刷卡時間
                _joTable.Add(new JProperty("MFreeDate", ktxtMFreeDate.Text)); // 計算後補休可休期限
                _joTable.Add(new JProperty("OverCompHours", ktxtOverCompHours.Text)); // 計算後的實際補休時數
                _joTable.Add(new JProperty("TMP_ATTREASONID", "")); // 加班原因
                _joTable.Add(new JProperty("TMP_WORKID", hidOT_CLASSTYPE.Value)); // 加班班別
                //_joTable.Add(new JProperty("RESTMINS", "0")); // 休息時間(分鐘)
                _joTable.Add(new JProperty("CALCTYPE", krblCHANGETYPE.SelectedValue)); // 給付方式
                _joTable.Add(new JProperty("NOTE", ktxtNOTE.Text)); // 加班內容說明
                jaTable.Add(_joTable);
                DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                dtSource.TableName = SCSHRConfiguration.SCSSOverTimeProgID;
                DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSOverTimeProgID);
                dsSource.Tables.Add(dtSource);
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME.btnCal_Click.dtSource:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtSource)));
                Exception oex = null;
                DataTable dtResult = service.BOImport(SCSHRConfiguration.SCSSOverTimeProgID, false, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out oex);
                DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME.btnCal_Click.Result:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
                if (oex != null)
                {
                    DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME::btnCal_Click::BOImportWS::StackTrace::{0}::ERROR::{1}", oex.StackTrace, oex.Message));
                    hidAPIResult.Value = "ERROR";
                    lblMessage.Text = "檢查失敗";
                }
                else
                {
                    if (dtResult != null &&
                        dtResult.Rows.Count > 0)
                    {
                        resultStatus = dtResult.Rows[0]["_STATUS"].ToString() == "0";
                        if (!resultStatus)
                            lblMessage.Text = dtResult.Rows[0]["_MESSAGE"].ToString();
                    }

                }
                if (resultStatus)
                {
                    hidAPIResult.Value = "OK";
                    lblMessage.Text = "檢查成功";
                }
            }

        }
        else // 沒選加班時間
        {
            // do nothing
        }
    }

    //[WebMethod]
    //public static string checkVal(string USER_GUID, string OT_START, string OT_END)
    //{
    //    ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值

    //    JObject joMessage = new JObject();
    //    string connectionstring = new DatabaseHelper().Command.Connection.ConnectionString;
    //    bool isNoError = true;

    //    DateTime dtOT_START = DateTime.MinValue;
    //    DateTime.TryParse(OT_START, out dtOT_START);
    //    DateTime dtOT_END = DateTime.MinValue;
    //    DateTime.TryParse(OT_END, out dtOT_END);

    //    if (dtOT_START > DateTime.MinValue &&
    //        dtOT_END > DateTime.MinValue)
    //    {
    //        using (SqlDataAdapter sda = new SqlDataAdapter(@"
    //            -- 檢查加班時間是否有重複
    //            SELECT DOC_NBR
    //              FROM Z_SCSHR_OVERTIME
    //              WHERE APPLICANTGUID = @USER_GUID
    //             AND ((OT_START >= @OT_START AND OT_START < @OT_END)
    //                     OR (OT_END > @OT_START AND OT_END <= @OT_END)
    //                     OR (OT_START <= @OT_START AND OT_END >= @OT_END))
    //                AND (CANCEL_DOC_NBR = '' OR CANCEL_DOC_NBR IS NULL)
    //                AND ((TASK_STATUS = 1 AND TASK_RESULT = 0)
    //                    OR (TASK_STATUS = 2 AND TASK_RESULT = 0)
    //                    OR (TASK_STATUS = 1 AND TASK_RESULT IS NULL))
    //        ", connectionstring))
    //        using (DataSet ds = new DataSet())
    //        {
    //            sda.SelectCommand.Parameters.AddWithValue("@USER_GUID", USER_GUID);
    //            sda.SelectCommand.Parameters.AddWithValue("@OT_START", dtOT_START);
    //            sda.SelectCommand.Parameters.AddWithValue("@OT_END", dtOT_END);
    //            try
    //            {
    //                sda.Fill(ds);

    //                if (ds.Tables[0].Rows.Count > 0) // 有重複申請的加班時間
    //                {
    //                    joMessage.Add(new JProperty("docrepeat", string.Format(@"[加班時間]重複 (單號：{0} 日期：{1} ~ {2})",
    //                    (string)ds.Tables[2].Rows[0]["DOC_NBR"], dtOT_START.ToString(), dtOT_END.ToString())));
    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME checkVal Error: {0}", e.Message));
    //                isNoError = false;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        isNoError = false;
    //    }

    //    if (!isNoError)
    //    {
    //        joMessage.Add(new JProperty("isError", "發生錯誤"));
    //    }

    //    return joMessage.ToString();
    //}

    [WebMethod]
    public static string CheckSignLevel(string DOC_NBR, string startPunch, string offPunch, string DoOverType, string MFreeDate, string OverCompHours)
    {
        ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
        string cstring = new DatabaseHelper().Command.Connection.ConnectionString;
        JObject joMessage = new JObject();
        goto toEnd;

        using (SqlDataAdapter sda = new SqlDataAdapter(@"
            SELECT * FROM Z_SCSHR_OVERTIME WHERE DOC_NBR = @DOC_NBR
            ", cstring))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("@DOC_NBR", DOC_NBR);
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    DateTime dtSTART_TIME = (DateTime)dr["OT_START"];
                    DateTime dtEND_TIME = (DateTime)dr["OT_END"];
                    DateTime dtStartPunch = DateTime.MinValue;
                    DateTime dtOffPunch = DateTime.MinValue;
                    DateTime.TryParse(startPunch, out dtStartPunch);
                    DateTime.TryParse(offPunch, out dtOffPunch);
                    SCSServicesProxy service = ConstructorCommonSettings.setSCSSServiceProxDefault();
                    DataTable dtResult = null;
                    JArray jaTable = new JArray();
                    JObject _joTable = new JObject();
                    _joTable.Add(new JProperty("USERNO", "1"));
                    _joTable.Add(new JProperty("SYS_VIEWID", DOC_NBR));
                    _joTable.Add(new JProperty("SYS_DATE", ((DateTime)dr["APPLICANTDATE"]).ToString("yyyyMMdd"))); // 日期
                    //_joTable.Add(new JProperty("TMP_EMPLOYEEID", dr["APPLICANT"].ToString())); // 加班人員
                    _joTable.Add(new JProperty("TMP_EMPLOYEEID", new KYT_UserPO().GetUserDetailByAccount(dr["APPLICANT"].ToString()).EmployeeNo)); // 加班人員
                    _joTable.Add(new JProperty("STARTDATE", dtSTART_TIME.ToString("yyyyMMdd"))); // 起始日期
                    _joTable.Add(new JProperty("STARTTIME", dtSTART_TIME.ToString("HHmm"))); // 起始時間
                    _joTable.Add(new JProperty("ENDTIME", dtEND_TIME.ToString("HHmm"))); // 結束時間
                    _joTable.Add(new JProperty("DoOverType", DoOverType)); // 加班別
                    _joTable.Add(new JProperty("WorkDate", dtStartPunch.ToString("yyyyMMdd"))); // 首筆刷卡日期
                    _joTable.Add(new JProperty("WorkTime", dtStartPunch.ToString("HHmm"))); // 首筆刷卡時間
                    _joTable.Add(new JProperty("OffWorkDate", dtOffPunch.ToString("yyyyMMdd"))); // 尾筆刷卡日期
                    _joTable.Add(new JProperty("OffWorkTime", dtOffPunch.ToString("HHmm"))); // 尾筆刷卡時間
                    _joTable.Add(new JProperty("MFreeDate", MFreeDate)); // 計算後補休可休期限
                    _joTable.Add(new JProperty("OverCompHours", OverCompHours)); // 計算後的實際補休時數
                    _joTable.Add(new JProperty("TMP_ATTREASONID", dr["OT_REASON"].ToString())); // 加班原因
                    _joTable.Add(new JProperty("TMP_WORKID", dr["OT_WORKID"].ToString())); // 加班班別
                                                                                           //_joTable.Add(new JProperty("RESTMINS", "0")); // 休息時間(分鐘)
                    _joTable.Add(new JProperty("CALCTYPE", dr["CHANGETYPE"].ToString())); // 給付方式
                    _joTable.Add(new JProperty("NOTE", dr["REMARK"].ToString())); // 加班內容說明
                    jaTable.Add(_joTable);
                    DataTable dtSource = JGlobalLibs.SQLUtils.jsonToTable(jaTable);
                    dtSource.TableName = SCSHRConfiguration.SCSSOverTimeProgID;
                    DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSOverTimeProgID);
                    dsSource.Tables.Add(dtSource);
                    DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME.CheckSignLevel.dtSource:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(dtSource)));
                    Exception oex = null;
                    dtResult = service.BOImport(SCSHRConfiguration.SCSSOverTimeProgID, false, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out oex);
                    if (oex != null)
                    {
                        DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME.CheckSignLevel.BOImport.ERROR:{0}", oex.Message));
                        joMessage.Add(new JProperty("Error", "發生錯誤，請通知管理員"));
                        goto toEnd;
                    }
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
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_OVERTIME.CheckSignLevel.SELECT.Z_SCSHR_LEAVE.ERROR: {0}", e.Message));
                joMessage.Add(new JProperty("Error", "發生錯誤，請通知管理員"));
                goto toEnd;
            }
        }
    toEnd:

        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_OVERTIME.CheckSignLevel.Result:{0}", joMessage.ToString()));
        return joMessage.ToString();
    }

    protected void rbtnFindOVERTIME_Click(object sender, EventArgs e)
    {
        // do nothing
    }



    /// <summary>
    /// 變更是否預加班事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void kcbIsPre_CheckedChanged(object sender, EventArgs e)
    {
        KYTCheckBox _kcbIsPre = (KYTCheckBox)sender;
        //kdtpOT_START.PickerVisibleColor = System.Drawing.Color.GhostWhite;
        //kdtpOT_END.PickerVisibleColor = System.Drawing.Color.GhostWhite;
        if (_kcbIsPre.Checked)
        {
            divPreOT.Visible = true;
            divH1.Visible = true;
            divH2.Visible = true;
            if (this.FormFieldMode == FieldMode.Applicant || // 起單
               this.FormFieldMode == FieldMode.ReturnApplicant)
            {
                btnCal.Visible = false;
                kdtpOT_START.PickerVisible = false;
                kdtpOT_END.PickerVisible = false;

                if (string.IsNullOrEmpty(my_FieldValue))
                {
                    // 加班單預設預加班時間
                    DateTime dtDefaultStart = DateTime.MinValue;
                    DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_START_TIME), out dtDefaultStart);
                    if (dtDefaultStart > DateTime.MinValue)
                    {
                        kdtpPreOT_START.Text = dtDefaultStart.ToString("yyyy/MM/dd HH:mm");
                    }
                    DateTime dtDefaultEnd = DateTime.MinValue;
                    DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_END_TIME), out dtDefaultEnd);
                    if (dtDefaultEnd > DateTime.MinValue)
                    {
                        kdtpPreOT_END.Text = dtDefaultEnd.ToString("yyyy/MM/dd HH:mm");
                    }
                }

                kdtpOT_START.Text = "";
                kdtpOT_END.Text = "";
            }
            else
            {
                if (this.FormFieldMode == FieldMode.Signin) // 簽核中
                {
                    if (taskObj.CurrentSite.SiteCode == "STARTOV") // 當關卡是STARTOV時，表示現在是預加班後要起加班單
                    {
                        btnCal.Visible = true;
                        kdtpOT_START.PickerVisible = true;
                        kdtpOT_END.PickerVisible = true;
                        kdtpOT_START.Text = kdtpPreOT_START.Text;
                        kdtpOT_END.Text = kdtpPreOT_END.Text;
                    }
                }
            }
        }
        else
        {
            divH1.Visible = false;
            divH2.Visible = false;
            divPreOT.Visible = false;
            kdtpPreOT_START.Text = "";
            kdtpPreOT_END.Text = "";
            if (this.FormFieldMode == FieldMode.Applicant || // 起單
             this.FormFieldMode == FieldMode.ReturnApplicant)
            {
                btnCal.Visible = true;
                kdtpOT_START.PickerVisible = true;
                kdtpOT_END.PickerVisible = true;
                // 加班單預設加班時間
                DateTime dtDefaultStart = DateTime.MinValue;
                DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_START_TIME), out dtDefaultStart);
                if (dtDefaultStart > DateTime.MinValue)
                {
                    kdtpOT_START.Text = dtDefaultStart.ToString("yyyy/MM/dd HH:mm");
                }
                DateTime dtDefaultEnd = DateTime.MinValue;
                DateTime.TryParse(string.Format(@"{0} {1}", System.DateTime.Now.Date.ToString("yyyy/MM/dd"), SCSHR.SCSHRConfiguration.FORM_DEFAULT_END_TIME), out dtDefaultEnd);
                if (dtDefaultEnd > DateTime.MinValue)
                {
                    kdtpOT_END.Text = dtDefaultEnd.ToString("yyyy/MM/dd HH:mm");
                }
            }
        }

    }

    /// <summary>
    /// 變更加班時間(起)事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void kdtpOT_START_TextChanged(object sender, EventArgs e)
    {
        KYTDateTimePicker _kdtpOT_START = (KYTDateTimePicker)sender;
        //if (string.IsNullOrEmpty(kdtpOT_END.Text))
        //    kdtpOT_END.Text = _kdtpOT_START.Text;
        if (SCSHRConfiguration.IS_COPY_TIMESTART_DATE_TO_TIMEEND_DATE.ToString().ToUpper() == "Y")
        {
            DateTime dtPreSTARTTIME = DateTime.MinValue;
            DateTime.TryParse(ViewState["STARTTIME"] != null ? ViewState["STARTTIME"].ToString() : "", out dtPreSTARTTIME); // 如果沒有紀錄上次的時間，就記錄成時間最小值
            DateTime dtSTARTTIME = DateTime.MinValue;
            DateTime.TryParse(_kdtpOT_START.Text, out dtSTARTTIME);
            if (dtPreSTARTTIME.Date != dtSTARTTIME.Date) // 如果上次的日期不等於現在的日期，更動迄止時間
            {
                DateTime dtENDTIME = DateTime.MinValue;
                DateTime.TryParse(kdtpOT_END.Text, out dtENDTIME);
                kdtpOT_END.Text = string.Format(@"{0} {1}", dtSTARTTIME.ToString("yyyy/MM/dd"), dtENDTIME.ToString("HH:mm")); // 只更動日期
                ViewState["STARTTIME"] = _kdtpOT_START.Text; // 紀錄這次的時間
            }
        }
        // 每次變更皆需要重新呼叫
        hidAPIResult.Value = "";
        ktxtOverTimeHours.Text = "0";
        ktxtApplyHours.Text = "0";
        // 計算和起始時間有沒有超過24HR
        DateTime dtOTStart = DateTime.MinValue;
        DateTime.TryParse(kdtpOT_START.Text, out dtOTStart);
        DateTime dtOTEnd = DateTime.MinValue;
        DateTime.TryParse(kdtpOT_END.Text, out dtOTEnd);
        TimeSpan ts = new TimeSpan(dtOTEnd.Ticks - dtOTStart.Ticks);
        // 當結束時間大於24小時就改結束時間
        if (dtOTStart > DateTime.MinValue &&
            (ts.TotalDays >= 1 ||
            ts.TotalHours > 24))
        {
            kdtpOT_END.Text = string.Format(@"{0} {1}", dtOTEnd.ToString("yyyy/MM/dd"), dtOTStart.ToString("HH:mm"));
        }

        // 重新計算本月累積時數
        DateTime dtStart = DateTime.Now;
        DateTime.TryParse(kdtpOT_START.Text, out dtStart);
        // 當日已加班時數
        ktxtApplyHoursTODAY.Text = getOverTimeHours(hidAPPLICANT.Value, dtStart.Date, dtStart.Date, false).ToString();
        // 當月已加班時數
        ktxtApplyHoursThemon.Text = getOverTimeHours(hidAPPLICANT.Value,
               new DateTime(int.Parse(dtStart.ToString().Substring(0, 4)), dtStart.Month, 1),
               new DateTime(int.Parse(dtStart.ToString().Substring(0, 4)), dtStart.Month, 1).AddMonths(1).AddDays(-1),
               true).ToString();

        Dialog.Open2(rbtnFindOVERTIME,
            string.Format(@"~/CDS/SCSHR/WKFFields/QUERYWINDOWS/findOVERTIME.aspx"),
            "已加班時數查詢 (顯示前後30天範圍內的所有加班單)",
            800,
            600,
            Dialog.PostBackType.AfterReturn,
            new { ACCOUNT = hidAPPLICANT.Value, START_DATE = kdtpOT_START.Text }.ToExpando());

    }

    /// <summary>
    /// 變更加班(迄)事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void kdtpOT_END_TextChanged(object sender, EventArgs e)
    {
        // 每次變更皆需要重新呼叫
        hidAPIResult.Value = "";
        ktxtOverTimeHours.Text = "0";
        ktxtApplyHours.Text = "0";
        // 計算和起始時間有沒有超過24HR
        DateTime dtOTStart = DateTime.MinValue;
        DateTime.TryParse(kdtpOT_START.Text, out dtOTStart);
        DateTime dtOTEnd = DateTime.MinValue;
        DateTime.TryParse(kdtpOT_END.Text, out dtOTEnd);
        TimeSpan ts = new TimeSpan(dtOTEnd.Ticks - dtOTStart.Ticks);
        // 當結束時間大於24小時就改結束時間
        if (dtOTStart > DateTime.MinValue &&
            (ts.TotalDays >= 1 ||
            ts.TotalHours > 24))
        {
            kdtpOT_END.Text = string.Format(@"{0} {1}", dtOTEnd.ToString("yyyy/MM/dd"), dtOTStart.ToString("HH:mm"));
        }
    }

    /// <summary>
    /// 變更預加班(起)事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void kdtpPreOT_START_TextChanged(object sender, EventArgs e)
    {
        KYTDateTimePicker _kdtpPreOT_START = (KYTDateTimePicker)sender;
        //if (string.IsNullOrEmpty(kdtpPreOT_END.Text))
        //    kdtpPreOT_END.Text = _kdtpPreOT_START.Text;
        if (SCSHRConfiguration.IS_COPY_TIMESTART_DATE_TO_TIMEEND_DATE.ToString().ToUpper() == "Y")
        {
            DateTime dtPreSTARTTIME = DateTime.MinValue;
            DateTime.TryParse(ViewState["PRESTARTTIME"] != null ? ViewState["PRESTARTTIME"].ToString() : "", out dtPreSTARTTIME); // 如果沒有紀錄上次的時間，就記錄成時間最小值
            DateTime dtSTARTTIME = DateTime.MinValue;
            DateTime.TryParse(_kdtpPreOT_START.Text, out dtSTARTTIME);
            if (dtPreSTARTTIME.Date != dtSTARTTIME.Date) // 如果上次的日期不等於現在的日期，更動迄止時間
            {
                DateTime dtENDTIME = DateTime.MinValue;
                DateTime.TryParse(kdtpPreOT_END.Text, out dtENDTIME);
                kdtpPreOT_END.Text = string.Format(@"{0} {1}", dtSTARTTIME.ToString("yyyy/MM/dd"), dtENDTIME.ToString("HH:mm")); // 只更動日期
                ViewState["PRESTARTTIME"] = _kdtpPreOT_START.Text; // 紀錄這次的時間
            }
        }

    }


    /// <summary>
    /// 變更給付方式事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void krblCHANGETYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 清空所有欄位
        ktxtOT_CLASSTYPE_NAME.Text = ""; // 加班班別
        hidOT_CLASSTYPE.Value = ""; // 加班班別
        ktxtApplyHours.Text = ""; // 申請時數
        ktxtOverTimeHours.Text = ""; // 加班時數
        hidAPIResult.Value = ""; // 呼叫WS結果
        hidAllowCalcType.Value = ""; // 加班給付方式允許類型
        ktxtOverCompHours.Text = ""; // 補休時數
        ktxtMFreeDate.Text = ""; // 補休期限
        ktxtWorkPunch.Text = ""; // 首筆刷卡時間
        ktxtOffWorkPunch.Text = ""; // 尾筆刷卡時間
        hidDoOverType.Value = ""; // 加班別
        ktxtDoOverType.Text = ""; // 加班別
    }

    /// <summary>
    /// 申請人_選擇
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    protected void btnAPPLICANT_DialogReturn(object sender, string result)
    {
        JArray jArray = (JArray)JsonConvert.DeserializeObject(result);
        if (jArray.Count > 0)
        {
            //EBUser user = new UserUCO().GetEBUser(jArray[0]["GUID"].ToString());
            KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid); // 人員
            ktxtAPPLICANT.Text = KUser.Name; // 申請人名稱
            hidAPPLICANTGuid.Value = KUser.UserGUID; // 申請人代號
            hidAPPLICANT.Value = KUser.Account; // 申請人帳號

            ktxtAPPLICANTDEPT.Text = KUser.GroupName[0]; // 部門名稱
            hidAPPLICANTDEPTName.Value = KUser.GroupName[0]; // 部門名稱
            hidAPPLICANTDEPT.Value = KUser.GroupID[0]; // UOF部門代號
            hidGROUPCODE.Value = KUser.GroupCode[0]; // 部門代號
        }
        else
        {
            ktxtAPPLICANT.Text = ""; // 申請人名稱
            hidAPPLICANTGuid.Value = ""; // 申請人代號
            hidAPPLICANT.Value = ""; // 申請人帳號

            ktxtAPPLICANTDEPT.Text = ""; // 部門名稱
            hidAPPLICANTDEPTName.Value = ""; // 部門名稱
            hidAPPLICANTDEPT.Value = ""; // UOF部門代號
            hidGROUPCODE.Value = ""; // 部門代號
        }
    }


}
