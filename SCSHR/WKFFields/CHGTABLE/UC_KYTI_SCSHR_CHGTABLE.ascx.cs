using Ede.Uof.EIP.Organization;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using Ede.Uof.WKF.Design;
using KYTLog;
using KYTUtilLibs.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UOFAssist.WKF;

/**
* 修改時間：2026/08/10
* 修改人員：陳郁承
* 修改項目：
    * 1. 議題3:修改調班日期時，沒有將hidCURRWORKID_ID清空，導致新增明細時檢查[原班別不可與新班別相同]拿上次的hidCURRWORKID_ID來檢查
* 修改原因：
    * 1. BUG修正
* 修改位置：
    * 「kdpDATE_SelectedIndexChanged」中，新增hidCURRWORKID_ID = ""
* **/

/**
* 修改時間：2023/08/02
* 修改人員：黃仕聿
* 修改項目：
    * 前端移除多餘的CustomValidator
* 修改原因：
    * 新增明細時，卡控的必填欄位未填卻仍執行新增，因多餘的CustomValidator沒有對應的卡控方法(已被註解)
* 修改位置： 
    * 其端將「<asp:CustomValidator ID="CustomValidator4" 」整行註解
* **/

/**
* 修改時間：2023/02/13
* 修改人員：黃仕聿
* 修改項目：
    * 1.新增表單條件OPTION1~6
* 修改原因：
    * 飛騰套件所有表單(請假、加班、銷假、批次加班、出差、補刷卡、調班、批次請假)，新增表單條件 option3~6 (TB_EB_USER-OPTION3~6)
* 修改位置： 
    * 1.public override string ConditionValue → .Add("OPTION1", KUser.Option1)  加上 1~6
* **/

/**
* 修改時間：2022/12/12
* 修改人員：高常昇
* 修改項目：
    * 1.將JGlobalLibs改為使用KYTUtilLibs
* 修改原因：
    * 1.更新
* 修改位置： 
    * 1.「整份文件」中，使用到「JGlobalLibs」的部分改為「KYTUtilLibs」方法
* **/

/**
* 修改時間：2022/05/23
* 修改人員：梁夢慈
* 修改項目：
    * 1.新增呼叫飛騰(BOImport)檢查調班單是否可以申請，檢查成功再加入明細項中
* 修改原因：
    * 1.新增規格，先在表單呼叫飛騰做檢查，避免於事件要真正寫入飛騰時才發現錯誤
 * 修改位置： 
    * 1.「btnNewGvItemD1_Click() -> if (string.IsNullOrEmpty(msg_date.Text) || msg_date.Text == "")」中，
    *    新增判斷式，檢查方法「CheckSCSHR_CHGTABLE()」，無回傳訊息，才可以新增於明細項中
* **/

/**
* 修改時間：2022/04/18
* 修改人員：梁夢慈
* 修改項目：
    * 1.明細調班日期(DATE)，寫入格式改為「yyyy/MM/dd」
    * 2.明細欄位-SYS_VIEWID、DATE，型別原為:「DateTime」-> 改為:「String」
* 修改原因：
    * 1、2.因客戶環境不知明原因，隨機會出現日期格式轉換失敗，但再次重送事件又可以成功，所以猜測原因來自於客戶主機設定問題，一勞永逸方法將日期欄位使用固定格式寫入
 * 修改位置： 
    * 1.*「btnNewGvItemD1_Click() -> if (isDATE == false && kdpDATE.Text != "" && kdpEND_DATE.Text != "") // 新增明細」中，
    *     參數start、end，原使用「DateTime.ParseExact()」，改使用KYT套件中的方法TryParseDateTime()轉型，並於寫入DATE欄位值時，使用「start.ToString("yyyy/MM/dd")」
    * 2.*「CreatePT_gvItemsD1()」中，更改欄位型別-SYS_VIEWID、DATE，型別原為:「DateTime」-> 改為:「String」
* **/

/**
* 修改時間：2021/12/09
* 修改人員：莊仁豪
* 修改項目：
    * 1.增加DisplayTitle、Message → 調班人員、調班日期、原班別、新班別。
* 修改原因：
    * 1.遺漏規格(調班人員、原班別、新班別)、Ardi提增加調班日期顯示。
 * 修改位置： 
    * 1.「DisplayTitle、Message」中，增加調班人員、調班日期、原班別、新班別，只取明細第一筆。
* **/

/**
* 修改時間：2021/12/08
* 修改人員：莊仁豪
* 修改項目：
    * 1.原班別不可與新班別相同在個人調班才需檢查，調班人員班別與調班對象班別不可在人員互調才需要檢查、更改調班性質時清除新班別。
    * 2.個人調班明細的新班Htype應為新班別的Htype。
    * 3.明細增加「對象新班Htype」欄位，「對象新班Htype」為調班人員的原Htype，且須相容於舊單。
* 修改原因：
    * 1.新增個人調班明細切換人員互調新增明細時，顯示原班別不可與新班別相同的訊息。
    * 2.個人調班明細的新班Htype應為新班別的Htype，原本為舊班別的Htype。
    * 3.飛騰端Htype未確實互調、Ardi提出的新需求，原本欄位顯示只會顯示與新班Htype及調班對象Htyp，但卻無調班對象的新班Htype，使用者在看的時候會覺得不合理。
 * 修改位置： 
    * 1.「getSCSHRWORKID」中，原班別不可與新班別相同程式區段移動至個人調班，調班人員班別與調班對象班別不可相同程式區段移動至人員互調。
    *   「krblTYPE_SelectedIndexChanged → 人員互調」更改調班性質，須清除新班別(hidTMP_CURRWORKID_ID)原只有清除(ktxtTMP_CURRWORKID)少清除(hidTMP_CURRWORKID_ID)。
    * 2.「btnNewGvItemD1_Click → type == "0"個人調班」中，原新增之明細ndr["HTYPE"] 改為取新班的Htype(hidTMP_CURRWORKID_Htype.Value)。
    * 3.「前端網頁」中，(gvItemsD1)增加(NTHTYPE)對象新班Htype欄位。
    *   「CreatePT_gvItemsD1()」中，add(NTHTYPE)對象新班Htype欄位
    *   「btnNewGvItemD1_Click() 新增明細 → type=1人員互調」，明細增加調班對象新班別ndr["NTHTYPE"] = hidCURRWORKID_Htype由調班人員原*Htype取得。
    *   「SetField()」中，當(FieldValue)有值時(簽核、觀看)，判斷gvItemsD1.DataTable.Columns.Contains("NTHTYPE")調班對象新班別不存在時add columns NTHTYPE。
* **/

/**
* 修改時間：2021/12/07
* 修改人員：莊仁豪
* 修改項目：
    * 1.明細增加「調班對象新班別」欄位，「調班對象新班別」為調班人員的原班別，且須相容於舊單。
    * 2.條件ConditionValue增加(ADJUSTTYPE)調班性質。
    * 3.組織欄位站點RealValue增加(TMP_TARGETID)調班對象。
    * 4.新增明細前增加檢查表頭調班對象與明細調班對象是否相同，若相同在檢查調班日期(起)、調班日期(迄)是否相同，條件成立不可新增明細。

* 修改原因：
    * 1.Ardi提出的新需球，原本欄位顯示只會顯示調班人員的原班別與新班別及調班對象的原班別，但卻無調班對象的新班別欄位，使用者在看的時候會覺得不合理。
    * 2.Ardi提出的新需球，條件ConditionValue增加(ADJUSTTYPE)調班性質。
    * 3.Ardi提出的新需球，組織欄位站點RealValue增加(TMP_TARGETID)調班對象。
    * 4.BUG，相同的調班日期，但表頭的調班人員與調班對象對調仍可新增明細。

 * 修改位置： 
    * 1.「前端網頁」中，(gvItemsD1)增加(TMP_NTCURRWORKID)調班對象新班別欄位。
    *   「CreatePT_gvItemsD1()」中，add(TMP_NTCURRWORKID)調班對象新班別欄位
    *   「btnNewGvItemD1_Click() 新增明細 → type=1人員互調」，明細增加調班對象新班別ndr["TMP_NTCURRWORKID"] = ktxtNEW_CURRWORKID由調班人員原班別取得
    *   「SetField()」中，當(FieldValue)有值時(簽核、觀看)，判斷gvItemsD1.DataTable.Columns.Contains("TMP_NTCURRWORKID")調班對象新班別不存在時add columns TMP_NTCURRWORKID。
    * 2.「ConditionValue()」中，ADD (ADJUSTTYPE)調班性質，由表頭調班性質(kddlADJUSTTYPE.SelectedValue)取得，0=個人調班, 1=人員互調。
    * 3.「RealValue()」中，ADD (TMP_TARGETID)調班對象，(hidTMP_TARGET_GUID)取得。
    * 4.「btnNewGvItemD1_Click() → 檢查表單明細」，增加3.檢查表頭調班對象帳號(ACCOUNT)與明細調班人員帳號(ACCOUNT)是否相同(hidTMP_TARGET_ACCOUNT==ndr["TMP_EMPLOYEEID_ACCOUNT"])，
    *    4.若相同，在檢查調班日期(起)~調班日期(迄)的日期與明細的調班日期任一筆日期相同時，則檔住不可新增明細。
* **/

/**
* 修改時間：2021/12/03
* 修改人員：莊仁豪
* 修改項目：
    * 1.選擇調班人員與調班對象時清除調班人員班別與調班對象班別、同時清除調班人員與調班對象相同的訊息。
    * 2.調班人員班與調班對象班別是否相同檢查方式改為call飛騰後取得班別再進行檢查、移除前端檢查。
    * 3.alert跳窗「該調班日期查無班別」時，清除調班日期(迄)。
    * 4.新增明細前增加判斷調班日期(起)、調班日期(迄)是否有填寫。
    * 5.原班別與新班別相同時不可新增明細。
    * 6.新增明細時，任一筆調班日期的原班別與新班相同時，跳過不處理，但之後的調班日期原班別與新班別相同時，需要可以新增明細。
* 修改原因：
    * 1.老楊反應_明細原有班別會帶入舊班別。
    * 2.更改調班人員、調班對象、調班日期(起)、調班日期(迄)後，調班人員班別與調班對象班別皆為空，導致檢查條件成立而無法新增明細。
    * 3.原本只有清除調班日期(起)，調班日期(迄)為後來新增欄位，為統一故調成調班日期(迄)也清除。
    * 4.若未填寫調班日期(起)、調班日期(迄)直接點選新增按鈕會報錯。
    * 5.bug，原本別與新班別相同卻還可以新增明細。
    * 6.bug，調班日期為多天，一次新增多筆明細，當其中一筆明細原班別與新班別相同時，會造成後面原班別與新班別不同的資料無法新增。
 * 修改位置： 
    * 1.「SelectTMP_EMPLOYEEID_DialogReturn」與「SelectTMP_TARGETID_DialogReturn」，增加(ktxtCURRWORKID)調班人員班別與(ktxtTMP_TCURRWORKID)調班對象班別與(msg_date)訊息給予空字串。
    * 2.「btnNewGvItemD1_Click → 檢查調班人員班別與調班對象班別是否相同」，將該檢查條件移動至「getSCSHRWORKID → 飛騰取得班別後檢查hidCURRWORKID_ID.Value == hidTMP_TCURRWORKID_ID.Value是否相同」。
    *   「btnNewGvItemD1_Click → type=1人員互調 → 新增明細前」，檢查(msg_date)調班人員班別與調班對象班別相同訊息是否為空，條件成立才可新增明細。
    * 3.「btnNewGvItemD1_Click → alert該調班日其查無班別」，增加(kdpEND_DATE)調班日期(迄)給予空值。
    * 4.「btnNewGvItemD1_Click → 檢查表單明細 取得調班日期(起)、調班日期(迄)」前，先判斷(kdpDATE)調班日期(起)與(kdpEND_DATE)調班日期(迄)是否不為空。
    *   「btnNewGvItemD1_Click → 新增明細前，先判斷(kdpDATE)調班日期(起)與(kdpEND_DATE)調班日期(迄)是否不為空，才可新增明細。
    *   「btnNewGvItemD1_Click → 若無選擇調班日期直接新增明細會跳出alert顯示已存在...相同調班日期...，增加判斷若(kdpDATE)調班日期(起)與(kdpEND_DATE)調班日期(迄)為空時，顯示「請填寫必填欄位」。
    * 5.「btnNewGvItemD1_Click → 個人調班 → 先檢查訊息是否為空，若有訊息([原班別不可與新班別相同])表示不可新增明細」→ 新增明細前增加判斷，檢查(msg_date)是否有訊息(原班別不可與新班別相同)，有訊息則不可新增明細。
    * 6.「btnNewGvItemD1_Click → 個人調班」增加bool(b_msg_date)作為明細是否有原班別與新班別相同的旗標，但其中一筆明細原班別與新班別相同時，則為true，明細新增後，顯示該原班別不可與新班別相同的訊息。
* **/

/**
* 修改時間：2021/12/02
* 修改人員：莊仁豪
* 修改項目：
    * 1.「調班人員與調班對象不可相同」警示訊息，成立條件改為比對選擇的調班人員與調班對象。(原為申請者與調班對象比對)
* 修改原因：
    * 1.調班人員與調班對象不同，但新增明細時顯示「調班人員與調班對象不可相同」警示訊息。
 * 修改位置： 
    * 1.「前端網頁 → javascript function(checkUser)」中，原判斷申請者帳號與調班對象帳號相同則成立顯示警示訊息if(hidAPPLICANTACCOUNT==hidTMP_TARGET_ACCOUNT)，改為依調班人員與調班對象帳號進行比對if(hidTMP_EMPLOYEEID_ACCOUNT==hidTMP_TARGET_ACCOUNT)。
* **/

/**
* 修改時間：2021/12/01
* 修改人員：莊仁豪
* 修改項目：
    * 1.更改調班日期(起)or調班日期(迄)時，清除調班人員班別與調班對象班別。
    * 2.調班日期(迄)改為必填。
* 修改原因：
    * 1.原新增多筆明細時，只會顯示第一筆明細之班別，但造成在新增第二筆明細時，會將第一筆調班對象班別帶入明細的調班對象班別，故調整為更改調班日期時清除調班人員班別與調班對象班別。
    * 2.未選擇調班日期(迄)而新增明細時報錯→新增明細會取得調班日期(起)與調班日期(迄)call飛騰並新增明細，但因為沒有調班日期(迄)所以造成報錯。
 * 修改位置： 
    * 1.「kdpDATE_SelectedIndexChanged()」，將調班人員班別(ktxtCURRWORKID)與調班對象班別(ktxtTMP_TCURRWORKID)給予空字串。
    * 2.「前端網頁」中，增加javascript function(checkkdpEND_DATE) → 卡控調班日期(迄)必填。
* **/

/**
* 修改時間：2021/11/30
* 修改人員：莊仁豪
* 修改項目：
    * 1. 個人調班 or 人員互調 選擇 調班人員 or 調班對象   表頭調班日期(起)改為不清除。
* 修改原因：
    * 1. 老楊反應重選調班人員後，會清除調班日期(起) → 造成每重選一次人就要重選一次日期。
* 修改位置： 
    * 1.「SelectTMP_EMPLOYEEID_DialogReturn()」、「SelectTMP_TARGETID_DialogReturn()」，註解調班日期為空kdpDATE.Text = ""。
* **/

/**
* 修改時間：2021/11/29
* 修改人員：莊仁豪
* 修改項目：
    * 1. 調班日期新增時先檢查調班人員與明細的調班人員是否相同，若相同在檢查調班日期是否相同。
* 修改原因：
    * 1. BUG，不同的調班人員，但調班日期與明細調班日期相同時無法新增明細。
* 修改位置： 
    * 1.「btnNewGvItemD1_Click → 檢查表單明細」 增加判斷if(表頭調班人員hidTMP_EMPLOYEEID_ACCOUNT == 明細調班人員ndr["TMP_EMPLOYEEID_ACCOUNT"])，條件成立在檢查調班日期是否相同。
* **/

/**
* 修改時間：2021/11/26
* 修改人員：莊仁豪
* 修改項目：
    * 1. 若飛騰端為空班別也要可以調班別。
* 修改原因：
    * 1. 老楊需求，空班要可以調班別。
* 修改位置： 
    * 1.「btnNewGvItemD1_Click → type=="0"個人調班 → 檢查班別代號為空時不新增明細」，註解該段程式碼(飛騰端回傳調班人員代號為空時表示為空班，原本會進行擋單，改為不檢查此條件)。
    *   「btnNewGvItemD1_Click → type=="1"調班對象 → 檢查班別代號為空或調班對象班別代號為空時不新增明細」，註解該段程式碼(飛騰端回傳調班人員或調班對象代號為空時表示為空班，原本會進行擋單，改為不檢查此條件)。
* **/

/**
* 修改時間：2021/10/06
* 修改人員：莊仁豪
* 修改項目：
    * 1. 明細調班人員班別要顯示該調班日期的調班人員班別。
    * 2. 第一次新增明細時檢查調班人員班別與新班別與調班對象班別是否相同。
* 修改原因：
    * 1. bug，因為表頭調班人員班別只需顯示第一筆資料的班別，所以才把調班人員班別帶入明細的調班人員班別，但當調班日期區間內的班別不同時，明細的調班人員班別會與該人員實際的班別不同。
    * 2. 第一次新增明細時調班人員班別與新班別或調班對象班別相同時，仍可以新增明細，增加。
* 修改位置： 
    * 1.「前端網頁」，增加(ktxtNEW_CURRWORKID)隱藏欄位，存放該人員該日期的班別。當呼叫飛騰「getSCSHRWORKID」時，該日期取得的調班人員班別存放至隱藏欄位中，明細依隱藏欄位顯示班別。
    * 2. 「btnNewGvItemD1_Click」，當調班代號有值後，先檢查調班人員班別與新班別或調班對象班別不可相同，當相同時顯示原班別不可與新班別相同 or 調班人員班別與調班對象班別不可相同。
    *   「前端網頁」，增加kdpDATE_SelectedIndexChanged事件，當重新選擇調班日期起、迄時，原班別不可與新班別相同的訊息隱藏。
* **/

/**
* 修改時間：2021/10/04
* 修改人員：莊仁豪
* 修改項目：
    * 1. 當明細任一筆調班日期與表頭選擇的調班日期(起)~調班日期(迄)區間相同時，不可新增明細。
* 修改原因：
    * 1. 原來只有檢查調班日期(一天)與明細調班日期區間是否相同。因為改為可選擇調班日期(起)~調班日期(迄)區間新增明細，所以需要檢查該調班日期區間是否與明細調班日期相同。
* 修改位置： 
    * 1.「btnNewGvItemD1_Click」，將原來「檢查現有表單」程式註解，新增「檢查現有表單，當調班日期(起)~調班日期(迄)區間的日期與明細的調班日期任一筆相同時，則檔住不可新增明細」。
    *    當tblgvItems_D1.Rows的調班日期包含在調班日期(起)與調班日期(迄)區間時，不可新增明細。
* **/

/**
* 修改時間：2021/10/01
* 修改人員：莊仁豪
* 修改項目：
    * 1. 原點選調班日期要CALL飛騰取得班別移除。
	* 2. 調班日期區分調班日期(起)與調班日期(迄)。
	* 3. 人員互調_調班人員班別與調班對象班別不可相同。
	* 4. 當一次新增多筆日期調班區間時，調班人員班別只需顯示第一筆調班人員班別。
	* 5. 當一次新增多筆日期調班區間時，調班對象班別只需顯示第一筆調班人員班別。
	* 6. 改為一次新增多筆調班日期區間的資料。
	* 7. 明細調班對象班別改為顯示調班對象的班別而不是調班人員的班別。
* 修改原因：
    * 1. 每點選一次調班日期要call一次飛騰，導致使用者若選錯再次選擇新的日期又會再次call飛騰讀取取得班別，造成使用者使用會覺得延遲。
	* 2. (新需求)客戶老楊反應，會有一次新增多天調班日期的需求，所以改為可一次選擇調班日期(起)與調班日期(迄)進行新增。
	* 3. 因為改為一次新增多筆調班日期，所以改為先判斷調班人員班別與調班對象班別是否為空班，在檢查是否相同。
	* 4. 因為會一次新增多筆明細，原本是是新增單筆明細，所以才顯示該調班人員班別，但改為多筆明細時，統一改為顯示第一筆班別資料。
	* 5. 因為會一次新增多筆明細，原本是是新增單筆明細，所以才顯示該調班對象班別，但改為多筆明細時，統一改為顯示第一筆班別資料。
	* 6. (新需求)同2.。
	* 7. bug，調班對象班別顯示調班人員班別，應顯示調班對象班別。
* 修改位置： 
    * 1.「kdpDATE_SelectedIndexChanged」事件移除。(前端網頁)刪除OnTextChange事件。
	* 2.「前端網頁」，原本調班日期改為調班日期(起)並增加調班日期(迄)(kdpEND_DATE)欄位。
	* 3. 「前端網頁」，js (checkObjectWorkID) function原本只判斷hidCURRWORKID_ID.val()與hidTMP_TCURRWORKID_ID.val()相同則條件成立，
		  改為先判斷hidCURRWORKID_ID.val與hidTMP_TCURRWORKID_ID.val()不為空時，在檢查與hidTMP_TCURRWORKID_ID.val()與hidTMP_TCURRWORKID_ID.val()是否相同。
	* 4. 「getSCSHRWORKID」，增加if判斷當調班人員班別(ktxtCURRWORKID)為空或為"(~)"字串時時，表示第一次近來查詢該班別所以顯示該班別資料。
	* 5. 「getSCSHRWORKID」，增加if判斷當調班人員班別(ktxtTMP_TCURRWORKID)為空或為"(~)"字串時時，表示第一次近來查詢該班別所以顯示該班別資料。
	* 6. 「btnNewGvItemD1_Click」原進入時直接call(getSCSHRWORKID)取得班別移除，改為先取得調班日期(起)與(迄)並依該區間跑迴圈，
	*     每次進入該迴圈都call 飛騰(getSCSHRWORKID)取得班別，並給予單一日期逐筆取得班別，當該日期查無班別時，取得該日期並記錄於msg.label中，
	*     當新增的日期區間中有一筆(含)以上查無班別時，則顯示該筆調班日期查無班別。
	* 7. 「btnNewGvItemD1_Click」，明細ndr["TMP_TCURRWORKID"]改為給調班對象班別名稱(ktxtTMP_TCURRWORKID)，明細ndr["TMP_TCURRWORKID_ID"]改為給調班對象班別代號，
	*     明細ndr["THTYPE"]改為給調班對象班別Htype。
* **/

/**
* 修改時間：2021/07/23
* 修改人員：梁夢慈
* 修改項目：
    * 1.調班人員GUID、ACCOUNT預設表單申請者
* 修改原因：
    * 1.BUG修正，原沒有預設值
* 修改位置： 
    * 1.「SetField() -> 如果欄位沒有值」中，設定調班人員預設值
* **/

/**
* 修改時間：2021/06/17
* 修改人員：梁夢慈
* 修改項目：
    * 1.所有有使用EBUser的地方，都改為呼叫通用方法取得人員資訊
* 修改原因：
    * 1.修改規格，UOF的EBUser有時候會異常取不到人員資訊，以防再多花時間去查明原因，改為通用方法直接查SQL方式取得人員資訊
* 修改位置： 
    * 1.「SetField()、SelectTMP_EMPLOYEEID_DialogReturn()、SelectTMP_TARGETID_DialogReturn()」中，註解所有EBUser，改為KYT_EBUser
* **/

/**
* 修改時間：2021/03/31
* 修改人員：莊仁豪
* 修改項目：
    * 1. 畫面調整，飛騰表單欄位間距調整、增加必填欄位紅色*符號。
* 修改原因：
    * 1. 飛騰表單畫面統一。
* 修改位置： 
    * 1. 「前端網頁」中，將前端bootstrap class由1,3改為2,4，調整欄位間距。
    * 2. 「前端網頁」中，必填欄位增加<span class="color_red">*</span>，紅色*符號。
    * 3. 「前端網頁」中，增加css語法(.divtitle、.msgColor)，調整欄位間距。
    * 4. 「前端網頁」中，增加框線，引用的CSS程式由「~/CDS/KYTUtils/Assets/css/KYTI.css」 改使用 「~/CDS/SCSHR/Assets/css/SCSHR.css」。
* **/

/**
* 修改時間：2021/03/31
* 修改人員：梁夢慈
* 修改項目：
    * 1. 將前端所有驗證CustomValidator(除了checkGridViewEmpty方法)外，都新增屬性ValidationGroup="NewGvItemD1"
    *    並將新增明細按紐(btnNewGvItemD1)，新增屬性ValidationGroup="NewGvItemD1"，當按鈕觸發時，只會檢查此群組的所有驗證方法
* 修改原因：
    * 1. BUG修正，送出不需要驗證表頭欄位
* 修改位置： 
    * 1. 「前端網頁」中，將前端所有驗證CustomValidator(除了checkGridViewEmpty方法)外、新增明細按紐(btnNewGvItemD1)，都新增屬性ValidationGroup="NewGvItemD1"
* **/

/**
* 修改時間：2021/03/19
* 修改人員：莊仁豪
* 修改項目：
    * 1. 檢查調班人員班別與調班對象班別是否相同，相同不可以新增明細。
* 修改原因：
    * 1. 規格遺漏。
* 修改位置： 
    * 1. 增加JavaScript事件「checkObjectWorkID」，判斷調班人員班別ID與調班對象班別ID是否相同，相同不新增明細，顯示「調班人員班別與調班對象班別不可相同」訊息。
* **/


/// <summary>
/// 飛騰-調班單
/// </summary>
public partial class WKF_OptionalFields_UC_KYTI_SCSHR_CHGTABLE : WKF_FormManagement_VersionFieldUserControl_VersionFieldUC
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

            SCSHR.Utils.KYT_EBUser KUser = new SCSHR.Utils.KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid);

            string cv = new KYTUtilLibs.Utils.UOFUtils.UOFForm.ConditionValue()
                .Add("ADJUSTTYPE", kddlADJUSTTYPE.SelectedValue) // 調班性質(0=個人調班, 1=人員互調)
                .Add("OPTION1", KUser.Option1)
                .Add("OPTION2", KUser.Option2)
                .Add("OPTION3", KUser.Option3)
                .Add("OPTION4", KUser.Option4)
                .Add("OPTION5", KUser.Option5)
                .Add("OPTION6", KUser.Option6)
                .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CHGTABLE::ConditionValue.cv:{0}", cv));
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
            DataTable dt = gvItemsD1.DataTable;
            string ADJUSTTYPE = ""; // 調班性質
            string TMP_EMPLOYEEID = ""; // 調班人員
            string TMP_EMPLOYEEWORK = ""; // 調班人員_原班別
            string TMP_CURRWORKID = ""; // 新班別 or 調班對象班別
            string DATE = ""; // 調班日期
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0]; // 取第一筆明細
                    ADJUSTTYPE = dr["ADJUSTTYPE"].ToString(); // 調班性質
                    TMP_EMPLOYEEID = dr["TMP_EMPLOYEEID"].ToString(); // 調班人員
                    TMP_EMPLOYEEWORK = dr["TMP_EMPLOYEEWORK"].ToString(); // 調班人員_原班別
                    DATE = dr["DATE"].ToString().Substring(0, 10);  // 調班日期

                    if (ADJUSTTYPE == "個人調班")  // 個人調班_只取新班別
                    {
                        TMP_CURRWORKID = dr["TMP_CURRWORKID"].ToString(); // 新班別
                    }
                    else  // 人員互調_只取調班對象班別
                    {
                        TMP_CURRWORKID = dr["TMP_TCURRWORKID"].ToString(); // 調班對象班別
                    }
                }
            }
            return string.Format(@"調班人員:{0},調班日期:{1}, 原班別:{2}, 新班別:{3}",
                TMP_EMPLOYEEID,
                DATE,
                TMP_EMPLOYEEWORK,
                TMP_CURRWORKID);
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
            DataTable dt = gvItemsD1.DataTable;
            string ADJUSTTYPE = ""; // 調班性質
            string TMP_EMPLOYEEID = ""; // 調班人員
            string TMP_EMPLOYEEWORK = ""; // 調班人員_原班別
            string TMP_CURRWORKID = ""; // 新班別 or 調班對象班別
            string DATE = ""; // 調班日期
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0]; // 取第一筆明細
                    ADJUSTTYPE = dr["ADJUSTTYPE"].ToString(); // 調班性質
                    TMP_EMPLOYEEID = dr["TMP_EMPLOYEEID"].ToString(); // 調班人員
                    TMP_EMPLOYEEWORK = dr["TMP_EMPLOYEEWORK"].ToString(); // 調班人員_原班別
                    DATE = dr["DATE"].ToString().Substring(0, 10);  // 調班日期

                    if (ADJUSTTYPE == "個人調班")  // 個人調班_只取新班別
                    {
                        TMP_CURRWORKID = dr["TMP_CURRWORKID"].ToString(); // 新班別
                    }
                    else  // 人員互調_只取調班對象班別
                    {
                        TMP_CURRWORKID = dr["TMP_TCURRWORKID"].ToString(); // 調班對象班別
                    }
                }
            }
            return string.Format(@"調班人員:{0}, 調班日期:{1}, 原班別:{2}, 新班別:{3}",
                TMP_EMPLOYEEID,
                DATE,
                TMP_EMPLOYEEWORK,
                TMP_CURRWORKID);
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
            string rv = new KYTUtilLibs.Utils.UOFUtils.UOFForm.RealValue()
                .AddPerson("TMP_TARGETID", hidTMP_TARGET_GUID.Value) // 調班對象
                .ToString();
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CHGTABLE::RealValue::rv:{0}", rv));
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
                //表單初始化狀態
                gvItemsD1.DataSource = CreatePT_gvItemsD1();
                gvItemsD1.DataBind();
                showHead.Visible = false; // 隱藏表頭
                if (!string.IsNullOrEmpty(fieldOptional.FieldValue)) //如果欄位有值
                {
                    kytController.FieldValue = fieldOptional.FieldValue;
                    // 調班對象新班別
                    if (!gvItemsD1.DataTable.Columns.Contains("TMP_NTCURRWORKID"))
                    {
                        gvItemsD1.DataTable.Columns.Add(new DataColumn("TMP_NTCURRWORKID", typeof(string)));
                    }
                    // 對象新班Htype
                    if (!gvItemsD1.DataTable.Columns.Contains("NTHTYPE"))
                    {
                        gvItemsD1.DataTable.Columns.Add(new DataColumn("NTHTYPE", typeof(string)));
                    }
                    //Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(HttpUtility.HtmlDecode(fieldOptional.FieldValue));
                    //ktxtSUM.Text = dict.ContainsKey("ktxtSUM") ? (string)dict["ktxtSUM"] : ""; //總計
                }

                kytController.SetAllViewType(KYTViewType.ReadOnly); // 設定所有KYT物件唯讀

                if (!IsModify())
                {
                    btnNewGvItemD1.Visible = false;
                    gvItemsD1.Columns[0].Visible = false;
                }
                msg_date.Visible = false; // 訊息隱藏
                switch (fieldOptional.FieldMode) // 判斷FieldMode
                {
                    case FieldMode.Applicant: // 起單或退回申請者
                    case FieldMode.ReturnApplicant:

                        kytController.SetAllViewType(KYTViewType.Input);// 設定所有KYT物件可輸入
                        ktxtMessage.ViewType = KYTViewType.ReadOnly;
                        if (string.IsNullOrEmpty(fieldOptional.FieldValue)) //如果欄位沒有值
                        {
                            //EBUser user = new UserUCO().GetEBUser(this.ApplicantGuid);
                            SCSHR.Utils.KYT_EBUser KUser = new SCSHR.Utils.KYT_UserPO().GetUserDetailByUserGuid(this.ApplicantGuid);

                            //string GroupCode = KYTUtilLibs.Utils.UOFUtils.UOFGroup.GetGroupCodeByDepartmentID(user.GroupID);
                            ktxtTMP_EMPLOYEEID.Text = KUser.NameWithAccount;
                            hidGROUPCODE.Value = KUser.GroupCode[0]; // GROUPCODE
                            hidAPPLICANTACCOUNT.Value = KUser.Account; // 申請者帳號
                            hidAPPLICANTGUID.Value = KUser.UserGUID; // 申請者GUID
                            hidAPPLICANTDEPT.Value = KUser.GroupID[0]; // 申請者部門(部門GUID)
                            hidTMP_EMPLOYEEID_DEP.Value = KUser.GroupCode[0]; //調班人員部門_group_code
                            ktxtTMP_EMPLOYEEID_DEP.Text = KUser.GroupName[0]; // 調班人員部門
                            kdpApplicantDate.Text = DateTime.Now.ToString("yyyy/MM/dd");

                            // 調班人員預設表單申請者
                            hidTMP_EMPLOYEEID_GUID.Value = KUser.UserGUID;
                            hidTMP_EMPLOYEEID_ACCOUNT.Value = KUser.Account;
                        }

                        ktxtTMP_EMPLOYEEID.ReadOnly = true; // 調班人員_唯讀
                        ktxtTMP_EMPLOYEEID_DEP.ReadOnly = true; // 調班人員部門_唯讀 
                        ktxtTMP_TARGETID.ReadOnly = true; // 調班對象_唯讀
                        ktxtTMP_TARGETID_DEP.ReadOnly = true; // 調班對象部門_唯讀
                        ktxtCURRWORKID.ReadOnly = true; // 調班人員班別_唯讀
                        ktxtTMP_CURRWORKID.ReadOnly = true; // 新班別_唯讀
                        ktxtCURRWORKID.ReadOnly = true; // 調班對象班別_唯讀
                        SelectTMPTARGETID.Visible = false; // 調班對象按鈕_隱藏
                        ktxtTMP_TCURRWORKID.ReadOnly = true; // 調班對象班別_唯讀
                        lblTMP_TCURRWORKID.Visible = false; // 調班對象班別_隱藏
                        lblTMP_CURRWORKID.Visible = true; // 新班別_顯示
                        ktxtTMP_TCURRWORKID.Visible = false; // 調班對象班別_隱藏
                        kdpDATE.Text = "";
                        Dialog.Open2(ibtnTMP_CURRWORKID, string.Format(@"~/CDS/SCSHR/WKFFields/QUERYWINDOWS/SearchCURRWORKID.aspx"), "選擇新班別", 800, 600, Dialog.PostBackType.AfterReturn, new { }.ToExpando());
                        krblTYPE_SelectedIndexChanged(null, null);
                        // 草稿起單，一律清空明細(避免班別有調換，而舊有的班別還存在於明細中，導致該人員實際的班別與現行班別不同)
                        gvItemsD1.DataSource = CreatePT_gvItemsD1();
                        gvItemsD1.DataBind();
                        showHead.Visible = true; // 顯示表頭
                        break;
                    case FieldMode.Design: // 表單設計階段
                        break;
                    case FieldMode.Print: // 表單列印
                        break;
                    case FieldMode.Signin: // 表單簽核
                        kdpApplicantDate.Visible = false;
                        ktxtTMP_EMPLOYEEID_DEP.Visible = false;
                        break;
                    case FieldMode.Verify: // Verify
                        break;
                    case FieldMode.View: // 表單觀看
                        break;
                }
            }
            else // 如果網頁POSTBACK
            {
                if (fieldOptional.FieldMode == FieldMode.Applicant ||
                    fieldOptional.FieldMode == FieldMode.ReturnApplicant)
                {

                }
            }
        }
    }

    #region 非控制項功能

    /// <summary>
    /// 是否為起單或者退回申請者
    /// </summary>
    /// <returns></returns>
    private bool IsModify()
    {
        return this.FormFieldMode == FieldMode.Applicant || this.FormFieldMode == FieldMode.ReturnApplicant;
    }

    private string CheckSCSHR_CHGTABLE(string DATE)
    {
        string MSG = "";

        #region 呼叫飛騰-檢查是否可調單
        string TMP_TARGETID = ""; // 調班對象ACCOUNT
        string TMP_TDEPARTID = ""; // 調班對象部門
        string TMP_CURRWORKID_ID = "";
        string HTYPE = "";
        string TMP_TCURRWORKID = ""; // 調班對象(新)班別
        string THTYPE = ""; // 對象新班Htype(調班人員原*Htype)
        if (kddlADJUSTTYPE.SelectedValue == "0") // 個人調班
        {
            TMP_CURRWORKID_ID = hidTMP_CURRWORKID_ID.Value; // 新班別ID(個人調班=新班別ID；人員互調=對象班別ID)
            HTYPE = hidTMP_CURRWORKID_Htype.Value; // 新班Hype(個人調班=新班別Htype)
        }
        else // 人員互調
        {
            KYT_EBUser Kuser = new KYT_UserPO().GetUserDetailByAccount(hidTMP_TARGET_ACCOUNT.Value);
            TMP_TARGETID = Kuser != null ? Kuser.EmployeeNo : ""; // 調班對象ACCOUNT
            TMP_TDEPARTID = hidTMP_TARGETID_DEP.Value; // 調班對象部門
            TMP_CURRWORKID_ID = hidTMP_TCURRWORKID_ID.Value; // 新班別ID(個人調班=新班別ID；人員互調=對象班別ID)
            HTYPE = hidTMP_TCURRWORKID_Htype.Value; // 新班Htype(個人調班=新班別Htype；人員互調=對象班別Htype)
            TMP_TCURRWORKID = hidCURRWORKID_ID.Value; // 調班對象新班別(調班人員原*班別)
            THTYPE = hidCURRWORKID_Htype.Value; // 對象新班Htype (對象新班是放NTHTYPE但傳飛騰欄位是THTYPE)
        }


        JArray jaTable = new JArray();
        JObject _joTable = new JObject();
        _joTable.Add(new JProperty("USERNO", "1")); // 自訂序號
        _joTable.Add(new JProperty("SYS_VIEWID", "")); // 編號
        _joTable.Add(new JProperty("SYS_DATE", ((DateTime)kdpApplicantDate.SelectedDate).ToString("yyyyMMdd"))); //日期
        _joTable.Add(new JProperty("ADJUSTTYPE", kddlADJUSTTYPE.SelectedValue)); // 調班性質
        _joTable.Add(new JProperty("TMP_EMPLOYEEID", new KYT_UserPO().GetUserDetailByAccount(hidTMP_EMPLOYEEID_ACCOUNT.Value).EmployeeNo)); // 調班人員
        _joTable.Add(new JProperty("TMP_DEPARTID", hidTMP_EMPLOYEEID_DEP.Value)); // 調班人員部門
        _joTable.Add(new JProperty("TMP_TARGETID", TMP_TARGETID)); // 調班對象ACCOUNT
        _joTable.Add(new JProperty("TMP_TDEPARTID", TMP_TDEPARTID)); // 調班對象部門
        _joTable.Add(new JProperty("NOTE", ktxtBKTXT.Text)); // 備註
        jaTable.Add(_joTable);
        DataTable dtHeadSource = KYTUtilLibs.Utils.CSharpUtils.CSharpDataTableUtils.jsonToTable(jaTable);
        dtHeadSource.TableName = SCSHRConfiguration.SCSSChgTableProgID;
        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CHGTABLE.IsCheckSCSHR_CHGTABLE().dtHeadSource:{0}", jaTable));

        // 表身 SUB
        jaTable = new JArray();
        _joTable = new JObject();
        _joTable.Add(new JProperty("USERNO", "1")); // 自訂序號
        _joTable.Add(new JProperty("DATE", DATE)); // 調班日期
        _joTable.Add(new JProperty("TMP_CURRWORKID", TMP_CURRWORKID_ID)); // 調班人員(新)班別
        _joTable.Add(new JProperty("HTYPE", HTYPE)); // 調班人員(新)空班類型
        _joTable.Add(new JProperty("TMP_TCURRWORKID", TMP_TCURRWORKID)); // 調班對象新班別(調班人員原*班別)
        _joTable.Add(new JProperty("THTYPE", THTYPE)); // 對象新班Htype (對象新班是放NTHTYPE但傳飛騰欄位是THTYPE)
        _joTable.Add(new JProperty("NOTE", ktxtBKTXT.Text)); // 備註

        jaTable.Add(_joTable);
        DataTable dtSubSource = KYTUtilLibs.Utils.CSharpUtils.CSharpDataTableUtils.jsonToTable(jaTable);
        dtSubSource.TableName = string.Format(@"{0}{1}", SCSHRConfiguration.SCSSChgTableProgID, "SUB");
        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CHGTABLE.IsCheckSCSHR_CHGTABLE().dtSubSource:{0}", jaTable));
        // 建立DataSet
        DataSet dsSource = new DataSet(SCSHRConfiguration.SCSSChgTableProgID);
        dsSource.Tables.Add(dtHeadSource);
        dsSource.Tables.Add(dtSubSource);
        Exception ex = null;
        DataTable dtResult = null;
        bool resultStatus = false;
        dtResult = service.BOImport(SCSHRConfiguration.SCSSChgTableProgID, true, SCSHR.net.azurewebsites.scsservices_beta.EFormFlowAction.Draft, true, dsSource, out ex);
        DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CHGTABLE.btnCal_Click.BOImport::{0}::Result::{1}", SCSHRConfiguration.SCSSChgTableProgID, Newtonsoft.Json.JsonConvert.SerializeObject(dtResult)));
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_CHGTABLE.btnCal_Click.BOImport.ERROR:{0}", ex.Message));
        if (dtResult != null &&
            dtResult.Rows.Count > 0)
        {
            resultStatus = dtResult.Rows[0]["_STATUS"].ToString() == "0";
            if (!resultStatus)
            {
                MSG = dtResult.Rows[0]["_MESSAGE"].ToString();
            }
        }

        #endregion 呼叫飛騰檢查-是否可調單

        return MSG;
    }

    #endregion 非控制

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
    /// 調班人員選擇
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    protected void SelectTMP_EMPLOYEEID_DialogReturn(object sender, string result)
    {
        // kdpDATE.Text = ""; // 調班日期_清除
        // 調班人員班別_清除
        ktxtCURRWORKID.Text = "";
        hidCURRWORKID_ID.Value = "";
        // 調班對象班別_清除
        ktxtTMP_TCURRWORKID.Text = "";
        hidTMP_TCURRWORKID_ID.Value = "";
        msg_date.Text = ""; // 更換調班人員時，清除調班人員班別與調班對象班別不可相同訊息

        DataTable value = JsonConvert.DeserializeObject<DataTable>(result);
        if (value.Rows.Count <= 0) return;
        DataRow row = value.Rows[0];
        ktxtTMP_EMPLOYEEID.Text = string.Format("{0}", row["NAME"].ToString());
        // ktxtTMPEMPLOYEEID.Text = string.Format("{0}-({1})", row["NAME"].ToString(), row["ACCOUNT"].ToString());
        KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(row["GUID"].ToString()); // 人員
        //EBUser user = new UserUCO().GetEBUser(row["GUID"].ToString()); // 人員
        string GroupCode = KYTUtilLibs.Utils.UOFUtils.UOFGroup.GetGroupCodeByDepartmentID(KUser.GroupID[0]);  // 部門
        hidTMP_EMPLOYEEID_GUID.Value = row["GUID"].ToString();
        hidTMP_EMPLOYEEID_ACCOUNT.Value = row["ACCOUNT"].ToString();
        ktxtTMP_EMPLOYEEID_DEP.Text = KUser.GroupName[0]; // 調班人員部門
        hidTMP_EMPLOYEEID_DEP.Value = GroupCode; // 調班人員部門group_code
    }

    /// <summary>
    /// 調班對象選擇
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    protected void SelectTMP_TARGETID_DialogReturn(object sender, string result)
    {
        // kdpDATE.Text = ""; // 調班日期_清除
        // 調班人員班別_清除
        ktxtCURRWORKID.Text = "";
        hidCURRWORKID_ID.Value = "";
        // 調班對象班別_清除
        ktxtTMP_TCURRWORKID.Text = "";
        hidTMP_TCURRWORKID_ID.Value = "";
        msg_date.Text = ""; // 更換調班對象時，清除調班人員班別與調班對象班別不可相同訊息

        DataTable value = JsonConvert.DeserializeObject<DataTable>(result);
        if (value.Rows.Count <= 0) return;
        DataRow row = value.Rows[0];
        ktxtTMP_TARGETID.Text = row["NAME"].ToString(); // 調班對象
        KYT_EBUser KUser = new KYT_UserPO().GetUserDetailByUserGuid(row["GUID"].ToString()); // 人員
        //EBUser user = new UserUCO().GetEBUser(row["GUID"].ToString()); // 人員
        //string GroupCode = KYTUtilLibs.Utils.UOFUtils.UOFGroup.GetGroupCodeByDepartmentID(KUser.GroupID[0]);  // 部門
        hidTMP_TARGET_GUID.Value = row["GUID"].ToString();  // 調班對象GUID
        hidTMP_TARGET_ACCOUNT.Value = row["ACCOUNT"].ToString(); // 調班對象ACCOUNT
        hidTMP_TARGETID_DEP.Value = KUser.GroupCode[0]; // 調班對象部門group_code
        ktxtTMP_TARGETID_DEP.Text = KUser.GroupName[0]; // 調班對象部門
    }

    /// <summary>
    /// 調班性質_選擇
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void krblTYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        kdpDATE.Text = ""; // 調班日期(起)_清空
        kdpEND_DATE.Text = ""; // 調班日期(迄)_清空
        msg_date.Visible = false; // 切換調班性質時訊息隱藏
        if (kddlADJUSTTYPE.SelectedValue == "0") // 個人調班
        {
            SelectTMPTARGETID.Visible = false; // 調班對象_按鈕顯示
            ibtnTMP_CURRWORKID.Visible = true; // 新班別_按鈕顯示
            lblTMP_TCURRWORKID.Visible = false; // 調班對象班別_隱藏
            lblTMP_CURRWORKID.Visible = true; // 新班別_顯示
            ktxtTMP_TCURRWORKID.Visible = false; // 調班對象班別_隱藏
            ktxtTMP_CURRWORKID.Visible = true; // 新班別_顯示

            ktxtTMP_TARGETID.Text = ""; // 調班對象_清空
            ktxtTMP_TCURRWORKID.Text = ""; // 調班對象班別_清空
            ktxtTMP_TARGETID_DEP.Text = ""; // 調班對象部門_清空
        }
        else if (kddlADJUSTTYPE.SelectedValue == "1") // 人員互調
        {
            SelectTMPTARGETID.Visible = true; // 調班對象_按鈕顯示
            ibtnTMP_CURRWORKID.Visible = false; // 新班別_按鈕隱藏
            lblTMP_TCURRWORKID.Visible = true; // 調班對象班別_顯示
            lblTMP_CURRWORKID.Visible = false; // 新班別_隱藏
            ktxtTMP_CURRWORKID.Visible = false; // 新班別_隱藏
            ktxtTMP_TCURRWORKID.Visible = true; // 調班對象班別_顯示
            ktxtTMP_CURRWORKID.Text = ""; // 新班別_清空    
            hidTMP_CURRWORKID_ID.Value = ""; // 新班別_清空    
        }
    }

    /// <summary>
    /// 調班日期_選擇
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void kdpDATE_SelectedIndexChanged(object sender, EventArgs e)
    {
        msg_date.Visible = false; // 原班別不可與新班別相同
        ktxtCURRWORKID.Text = ""; // 調班人員班別清除
        hidCURRWORKID_ID.Value = ""; // 調班人員班別_清除
        ktxtTMP_TCURRWORKID.Text = ""; //調班對象班別_清除
        msg_date.Text = ""; // 更換調班日期時，清除調班人員班別與調班對象班別不可相同訊息
    }

    /// <summary>
    /// 呼叫飛騰，取得調班日期班別
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void getSCSHRWORKID(string StartDate, string EndDate, string EmployeeID, string Employee)
    {
        // 呼叫飛騰，取得班別
        SCSServicesProxy service = ConstructorCommonSettings.setSCSSServiceProxDefault();
        Exception ex = null;
        SCSHR.net.azurewebsites.scsservices_beta.Parameter[] parameters = service.BOExecFunc("ATT0021700",
            "GetEmpWorkInfoData_WS",
            SCSHR.Types.SCSParameter.getPatameters(new
            {
                StartDate = StartDate,
                EndDate = EndDate,
                EmployeeID = EmployeeID
            }),
            out ex);
        if (ex != null)
            DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_CHGTABLE.kdpDATE_SelectedIndexChanged.ERROR:{0}", ex.Message));
        if (parameters != null &&
                    parameters.Length > 0)
        {
            DebugLog.Log(DebugLog.LogLevel.Info, string.Format(@"UC_KYTI_SCSHR_CHGTABLE.kdpDATE_SelectedIndexChanged.CheckLeaveLRData.result:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(parameters)));
            if (parameters[0].DataType.ToString() == "DataTable")
            {
                DataTable dtSource = SCSHRUtils.XmlToDataTable(parameters[0].Xml);
                if (dtSource.Rows.Count > 0)
                {
                    string msg = "";
                    DataRow dr = dtSource.Rows[0];
                    if (Employee == "ORI")
                    {
                        hidCURRWORKID_ID.Value = dr["TMP_WorkID"].ToString(); // 班別代號
                        hidCURRWORKID_NAME.Value = dr["TMP_WorkName"].ToString(); // 班別名稱
                        hidCURRWORKID_Htype.Value = dr["Htype"].ToString(); // Htype
                        hidCURRWORKID_Start.Value = dr["StartTime"].ToString(); // 上班時間
                        hidCURRWORKID_End.Value = dr["EndTime"].ToString(); // 下班時間
                        // 存放該日期最新的調班人員班別for明細顯示該日期的調班人員班別
                        ktxtNEW_CURRWORKID.Text = string.Format("{0}({1}~{2})", hidCURRWORKID_ID.Value, hidCURRWORKID_Start.Value, hidCURRWORKID_End.Value);
                        if (string.IsNullOrEmpty(ktxtCURRWORKID.Text) || ktxtCURRWORKID.Text == "(~)") // 調班人員班別 → 因為多筆明細，所以只需要顯示第一筆調班人員班別
                        {
                            ktxtCURRWORKID.Text = string.Format("{0}({1}~{2})", hidCURRWORKID_ID.Value, hidCURRWORKID_Start.Value, hidCURRWORKID_End.Value);
                        }
                        if (!string.IsNullOrEmpty(hidCURRWORKID_ID.Value) && !string.IsNullOrEmpty(hidTMP_CURRWORKID_ID.Value))
                        {
                            if (hidCURRWORKID_ID.Value == hidTMP_CURRWORKID_ID.Value)
                            {
                                msg = "[原班別不可與新班別相同]";
                                msg_date.Visible = true; // 欄位顯示
                                msg_date.Text = msg;
                            }
                        }
                    }
                    else if (Employee == "NEW")
                    {
                        // (new)增加判斷飛騰給的TMP_WorkID班別代號是否為空，因為傳給飛騰會有資料，但給的資料卻沒有TMP_WorkID情況(表示有資料但沒有維護班別)
                        //if (!string.IsNullOrEmpty(dr["TMP_WorkID"].ToString()) || dr["TMP_WorkID"].ToString() != "")
                        //{
                        //    hidTMP_TCURRWORKID_ID.Value = dr["TMP_WorkID"].ToString(); // 班別代號
                        //    hidTMP_TCURRWORKID_NAME.Value = dr["TMP_WorkName"].ToString(); // 班別名稱
                        //    hidTMP_TCURRWORKID_Htype.Value = dr["Htype"].ToString(); // Htype
                        //    hidTMP_TCURRWORKID_Start.Value = dr["StartTime"].ToString(); // 上班時間
                        //    hidTMP_TCURRWORKID_End.Value = dr["EndTime"].ToString(); // 下班時間

                        //    if (string.IsNullOrEmpty(ktxtTMP_TCURRWORKID.Text) || ktxtTMP_TCURRWORKID.Text == "(~)") // 調班對象班別→ 因為多筆明，所以只需要顯示第一筆調班對象班別
                        //    {
                        //        ktxtTMP_TCURRWORKID.Text = string.Format("{0}({1}~{2})", hidTMP_TCURRWORKID_ID.Value, hidTMP_TCURRWORKID_Start.Value, hidTMP_TCURRWORKID_End.Value);
                        //    }
                        //}
                        hidTMP_TCURRWORKID_ID.Value = dr["TMP_WorkID"].ToString(); // 班別代號
                        hidTMP_TCURRWORKID_NAME.Value = dr["TMP_WorkName"].ToString(); // 班別名稱
                        hidTMP_TCURRWORKID_Htype.Value = dr["Htype"].ToString(); // Htype
                        hidTMP_TCURRWORKID_Start.Value = dr["StartTime"].ToString(); // 上班時間
                        hidTMP_TCURRWORKID_End.Value = dr["EndTime"].ToString(); // 下班時間

                        if (string.IsNullOrEmpty(ktxtTMP_TCURRWORKID.Text) || ktxtTMP_TCURRWORKID.Text == "(~)") // 調班對象班別→ 因為多筆明，所以只需要顯示第一筆調班對象班別
                        {
                            ktxtTMP_TCURRWORKID.Text = string.Format("{0}({1}~{2})", hidTMP_TCURRWORKID_ID.Value, hidTMP_TCURRWORKID_Start.Value, hidTMP_TCURRWORKID_End.Value);
                        }
                        if (hidCURRWORKID_ID.Value == hidTMP_TCURRWORKID_ID.Value)
                        {
                            msg = "[調班人員班別與調班對象班別不可相同]";
                            msg_date.Visible = true; // 欄位顯示
                            msg_date.Text = msg;
                        }
                    }
                    //string msg = "";
                    // 檢查調班人員班別與調班對象班別是否相同
                    //if (hidCURRWORKID_ID.Value == hidTMP_TCURRWORKID_ID.Value)
                    //{
                    //    msg = "[調班人員班別與調班對象班別不可相同]";
                    //    msg_date.Visible = true; // 欄位顯示
                    //    msg_date.Text = msg;
                    //}
                    //else if (hidCURRWORKID_ID.Value == hidTMP_CURRWORKID_ID.Value) 
                    //{
                    //    msg = "[原班別不可與新班別相同]";
                    //    msg_date.Visible = true; // 欄位顯示
                    //    msg_date.Text = msg;
                    //}
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), @"alert('該調班日期查無班別');", true);
                    kdpDATE.Text = ""; // 調班日期(起)
                    kdpEND_DATE.Text = ""; // 調班日期(迄)
                }
            }
        }
    }

    /// <summary>
    /// 跳窗Dialog選取新班別
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ibtnTMP_CURRWORKID_Click(object sender, ImageClickEventArgs e)
    {
        msg_date.Text = ""; // 更換新班別時，清除原班別不可與新班別相同訊息。
        string value = Dialog.GetReturnValue();
        if (string.IsNullOrEmpty(value)) return;
        string returnValue = "";
        returnValue = value.Replace("&#&#&&", "''").Replace("#&#&##", "'");
        DataTable result = JsonConvert.DeserializeObject<DataTable>(returnValue);
        DataRow dr = result.Rows[0];
        hidTMP_CURRWORKID_ID.Value = dr["SYS_VIEWID"].ToString(); // 班別代號
        hidTMP_CURRWORKID_NAME.Value = dr["SYS_NAME"].ToString(); // 班別名稱
        hidTMP_CURRWORKID_Htype.Value = dr["Htype"].ToString(); // Htype***
        hidTMP_CURRWORKID_Start.Value = dr["STARTTIME"].ToString(); // 上班時間
        hidTMP_CURRWORKID_End.Value = dr["ENDTIME"].ToString(); // 下班時間
        ktxtTMP_CURRWORKID.Text = string.Format("{0}({1}~{2})", hidTMP_CURRWORKID_ID.Value, hidTMP_CURRWORKID_Start.Value, hidTMP_CURRWORKID_End.Value);
    }

    /// <summary>
    /// 建立申請明細
    /// </summary>
    /// <returns></returns>
    private DataTable CreatePT_gvItemsD1()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ITEM_NO", typeof(String))); // 項次
        dt.Columns.Add(new DataColumn("SYS_VIEWID", typeof(String))); // 編號
        dt.Columns.Add(new DataColumn("SYS_DATE", typeof(String))); // 日期
        dt.Columns.Add(new DataColumn("ADJUSTTYPE", typeof(String))); // 調班性質
        dt.Columns.Add(new DataColumn("DATE", typeof(String))); // 調班日期

        dt.Columns.Add(new DataColumn("TMP_EMPLOYEEID", typeof(String))); // 調班人員*
        dt.Columns.Add(new DataColumn("TMP_EMPLOYEEID_GUID", typeof(String))); // 調班人員GUID
        dt.Columns.Add(new DataColumn("TMP_EMPLOYEEID_ACCOUNT", typeof(String))); // 調班人員ACCOUNT
        dt.Columns.Add(new DataColumn("TMP_DEPARTID", typeof(String))); // 調班人員部門
        dt.Columns.Add(new DataColumn("TMP_DEPARTID_CODE", typeof(String))); // 調班人員部門GroupCode
        dt.Columns.Add(new DataColumn("TMP_EMPLOYEEWORK", typeof(String))); // 調班人員原班別
        dt.Columns.Add(new DataColumn("TMP_EMPLOYEEWORK_ID", typeof(String))); // 調班人員原班別ID

        dt.Columns.Add(new DataColumn("TMP_TARGETID", typeof(String))); // 調班對象*
        dt.Columns.Add(new DataColumn("TMP_TARGETID_GUID", typeof(String))); // 調班對象GUID
        dt.Columns.Add(new DataColumn("TMP_TARGETID_ACCOUNT", typeof(String))); // 調班對象ACCOUNT
        dt.Columns.Add(new DataColumn("TMP_TDEPARTID", typeof(String))); // 調班對象部門
        dt.Columns.Add(new DataColumn("TMP_TDEPARTID_CODE", typeof(String))); // 調班部門GroupCode

        dt.Columns.Add(new DataColumn("TMP_TCURRWORKID", typeof(String))); // 調班對象班別*新(原調班人員班別)
        dt.Columns.Add(new DataColumn("TMP_TCURRWORKID_ID", typeof(String))); // 調班對象班別ID*新(原調班人員班別ID)
        dt.Columns.Add(new DataColumn("THTYPE", typeof(String))); // 調班對象班Htype*新(原調班人員班別ID)

        dt.Columns.Add(new DataColumn("TMP_CURRWORKID", typeof(String))); // 新班別(個人調班=新班別；人員互調=對象班別)
        dt.Columns.Add(new DataColumn("TMP_CURRWORKID_ID", typeof(String))); // 新班別ID(個人調班=新班別ID；人員互調=對象班別ID)
        dt.Columns.Add(new DataColumn("HTYPE", typeof(String))); // 新班Htype(個人調班=新班別Htype；人員互調=對象班別Htype)

        dt.Columns.Add(new DataColumn("TMP_NTCURRWORKID", typeof(String))); // 調班對象新班別(調班人員原*班別)
        dt.Columns.Add(new DataColumn("NTHTYPE", typeof(String))); // 對象新班Htype(調班人員原*Htype)

        dt.Columns.Add(new DataColumn("NOTE", typeof(String))); // 備註
        dt.Columns.Add(new DataColumn("MSG", typeof(String))); // 訊息
        dt.Columns.Add(new DataColumn("ReSend", typeof(String))); // 重拋
        return dt;
    }

    /// <summary>
    /// 申請明細
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvItemsD1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblgvItemsD1 = gvItemsD1.DataTable; //取出先前記住的 gvItems
            DataRow row = tblgvItemsD1.Rows[gr.DataItemIndex]; //取出對應的資料列

            ImageButton btnDeleteD1 = gr.FindControl("btnDeleteD1") as ImageButton; // 刪除

            KYTTextBox ktxtITEM_NO = gr.FindControl("ktxtITEM_NO") as KYTTextBox; // 項次
            KYTTextBox ktxtSYS_VIEWID = gr.FindControl("ktxtSYS_VIEWID") as KYTTextBox; // 編號
            KYTDatePicker ktxtSYS_DATE = gr.FindControl("ktxtSYS_DATE") as KYTDatePicker; // 日期
            KYTTextBox ktxtADJUSTTYPE = gr.FindControl("ktxtADJUSTTYPE") as KYTTextBox; // 調班性質
            KYTTextBox ktxtTMP_EMPLOYEEID = gr.FindControl("ktxtTMP_EMPLOYEEID") as KYTTextBox; // 調班人員
            KYTTextBox ktxtTMP_DEPARTID = gr.FindControl("ktxtTMP_DEPARTID") as KYTTextBox; // 人員部門
            KYTTextBox ktxtTMP_TARGETID = gr.FindControl("ktxtTMP_TARGETID") as KYTTextBox; // 調班對象
            KYTTextBox ktxtTMP_TDEPARTID = gr.FindControl("ktxtTMP_TDEPARTID") as KYTTextBox; // 對象部門
            KYTDatePicker ktxtDATE = gr.FindControl("ktxtDATE") as KYTDatePicker; // 調班日期
            KYTTextBox ktxtTMP_EMPLOYEEWORK = gr.FindControl("ktxtTMP_EMPLOYEEWORK") as KYTTextBox; // 調班人員班別
            KYTTextBox ktxtTMP_CURRWORKID = gr.FindControl("ktxtTMP_CURRWORKID") as KYTTextBox; // 新班別
            KYTTextBox ktxtHTYPE = gr.FindControl("ktxtHTYPE") as KYTTextBox; // 新班Htype
            KYTTextBox ktxtTMP_TCURRWORKID = gr.FindControl("ktxtTMP_TCURRWORKID") as KYTTextBox; // 調班對象班別
            KYTTextBox ktxtTHTYPE = gr.FindControl("ktxtTHTYPE") as KYTTextBox; // 對象班Htype

            KYTTextBox ktxtTMP_NTCURRWORKID = gr.FindControl("ktxtTMP_NTCURRWORKID") as KYTTextBox; // 調班對象新班別
            KYTTextBox ktxtNTHTYPE = gr.FindControl("ktxtNTHTYPE") as KYTTextBox; // 對象新班Htype

            KYTTextBox ktxtNOTE = gr.FindControl("ktxtNOTE") as KYTTextBox; // 備註
            KYTTextBox ktxtMSG = gr.FindControl("ktxtMSG") as KYTTextBox; // 訊息
            KYTTextBox ktxtReSend = gr.FindControl("ktxtReSend") as KYTTextBox; // 重拋

            btnDeleteD1.Visible = false; // 刪除按鈕隱藏

            ktxtITEM_NO.ViewType = KYTViewType.ReadOnly; // 項次
            ktxtSYS_VIEWID.ViewType = KYTViewType.ReadOnly; // 編號
            ktxtSYS_DATE.ViewType = KYTViewType.ReadOnly; // 日期
            ktxtADJUSTTYPE.ViewType = KYTViewType.ReadOnly; // 調班性質
            ktxtTMP_EMPLOYEEID.ViewType = KYTViewType.ReadOnly; // 調班人員
            ktxtTMP_DEPARTID.ViewType = KYTViewType.ReadOnly; // 人員部門
            ktxtTMP_TARGETID.ViewType = KYTViewType.ReadOnly; // 調班對象
            ktxtTMP_TDEPARTID.ViewType = KYTViewType.ReadOnly; // 對象部門
            ktxtDATE.ViewType = KYTViewType.ReadOnly; // 調班日期
            ktxtTMP_EMPLOYEEWORK.ViewType = KYTViewType.ReadOnly; // 調班人員班別
            ktxtTMP_CURRWORKID.ViewType = KYTViewType.ReadOnly; // 新班別
            ktxtHTYPE.ViewType = KYTViewType.ReadOnly; // 新班Htype
            ktxtTMP_TCURRWORKID.ViewType = KYTViewType.ReadOnly; // 調班對象班別
            ktxtTHTYPE.ViewType = KYTViewType.ReadOnly; // 對象班Htype
            ktxtTMP_NTCURRWORKID.ViewType = KYTViewType.ReadOnly; // 調班對象新班別
            ktxtNTHTYPE.ViewType = KYTViewType.ReadOnly; // 對象新班Htype
            ktxtNOTE.ViewType = KYTViewType.ReadOnly; // 備註
            ktxtMSG.ViewType = KYTViewType.ReadOnly; // 訊息
            ktxtReSend.ViewType = KYTViewType.ReadOnly; // 重拋

            if (this.IsModify())
            {
                btnDeleteD1.Visible = true; // 刪除按鈕顯示
            }

            // tblgvItemsD1.Columns.Contains("TMP_NTCURRWORKID") ? row["TMP_NTCURRWORKID"].ToString() : "";

        }
    }

    /// <summary>
    /// 建立新的申請明細-D1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNewGvItemD1_Click(object sender, EventArgs e)
    {
        // 1.檢查調班日期日否重複
        DataTable tblgvItems_D1 = gvItemsD1.DataTable;
        // step1. 檢查調班日期日否重複 
        // step2. 檢查調班人員、調班對象是否存再明細內
        bool isDATE = false; // false→新增明細，true→不新增明細
        List<string> LisOri = new List<string>();
        LisOri.Add(hidTMP_EMPLOYEEID_GUID.Value); // 調班人員-表頭
        if (!string.IsNullOrEmpty(hidTMP_TARGET_GUID.Value))
            LisOri.Add(hidTMP_TARGET_GUID.Value); // 調班對象-表頭


        // 取得正在簽核中表單
        DataTable dtSign = new DataTable();
        using (SqlDataAdapter sda = new SqlDataAdapter(@"
         SELECT *
           FROM [Z_SCSHR_CHGTABLE_D]
          WHERE DOC_NBR IN (SELECT DOC_NBR 
                              FROM Z_SCSHR_CHGTABLE 
                             WHERE TASK_STATUS = '1' 
                               AND TASK_RESULT IS NULL)
        ", ConnectionString))
        using (DataSet ds = new DataSet())
        {
            try
            {
                sda.Fill(ds);
                dtSign = ds.Tables[0];
            }
            catch (Exception ex)
            {
                DebugLog.Log(DebugLog.LogLevel.Error, string.Format(@"UC_KYTI_SCSHR_CHGTABLE::btnNewGvItemD1_Click()::檢查簽核中表單失敗:ERROR::{0}", ex.Message));
            }
        }
        // 檢查正在簽核中表單
        foreach (DataRow dr in dtSign.Rows)
        {
            DateTime dtDATE = DateTime.MinValue; // 明細調班日期
            DateTime.TryParse(dr["DATE"].ToString(), out dtDATE);
            if (!string.IsNullOrEmpty(kdpDATE.Text) || kdpDATE.Text != "")
            {
                if (DateTime.Compare(dtDATE, (DateTime)kdpDATE.SelectedDate) == 0)
                {
                    // 檢查人員該調班日期是否已申請
                    if (LisOri.Contains(dr["TMP_EMPLOYEEID"].ToString()) || LisOri.Contains(dr["TMP_TARGETID"].ToString()))
                    {
                        isDATE = true;
                        break;
                    }
                }
            }
        }
        // 檢查現有表單  -> 原來只由檢查表頭調班日期與所有明細的調班日期是否相同
        //foreach (DataRow dr in tblgvItems_D1.Rows)
        //{
        //    DateTime dtDATE = DateTime.MinValue; // 明細調班日期
        //    DateTime.TryParse(dr["DATE"].ToString(), out dtDATE);
        //    if (DateTime.Compare(dtDATE, (DateTime)kdpDATE.SelectedDate) == 0)
        //    {
        //        // 檢查人員該調班日期是否已申請
        //        if (LisOri.Contains(dr["TMP_EMPLOYEEID_GUID"].ToString()) || LisOri.Contains(dr["TMP_TARGETID_GUID"].ToString()))
        //        {
        //            isDATE = true;
        //            break;
        //        }
        //    }
        //}


        // 檢查表單明細
        // 1. 先檢查表頭調班人員帳號(ACCOUNT)與明細的調班人員帳號(ACCOUNT)是否相同
        // 2. 若相同，在檢查調班日期(起)~調班日期(迄)的日期與明細的調班日期任一筆日期相同時，則檔住不可新增明細。
        DateTime new_start = DateTime.MinValue;
        DateTime new_end = DateTime.MinValue;
        if ((!string.IsNullOrEmpty(kdpDATE.Text) || kdpDATE.Text != "") && (!string.IsNullOrEmpty(kdpEND_DATE.Text) || kdpEND_DATE.Text != ""))
        {
            new_start = DateTime.ParseExact(kdpDATE.Text, "yyyy/MM/dd", null); // 取得調班日期(起)
            new_end = DateTime.ParseExact(kdpEND_DATE.Text, "yyyy/MM/dd", null); // 取得調班日期(迄)
        }

        int new_gap_day = (new_end - new_start).Days; // 起訖之間的天數

        for (int i = 0; i <= new_gap_day; i++) // 區間
        {
            string new_date = new_start.ToString("yyyy/MM/dd").Replace("/", ""); // 可變動之調班日期(一筆日期一筆日期傳)
            // 尋覽每一筆明細
            foreach (DataRow ndr in tblgvItems_D1.Rows)
            {
                DateTime dtDATE = DateTime.MinValue; // 明細調班日期
                DateTime.TryParse(ndr["DATE"].ToString(), out dtDATE); // 取得明細的調班日期
                // 1. 先檢查表頭調班人員帳號(ACCOUNT)與明細的調班人員帳號(ACCOUNT)是否相同
                if (hidTMP_EMPLOYEEID_ACCOUNT.Value == ndr["TMP_EMPLOYEEID_ACCOUNT"].ToString())
                {
                    // 2. 若相同，在檢查調班日期(起)~調班日期(迄)的日期與明細的調班日期任一筆日期相同時，則檔住不可新增明細。
                    if (new_start == dtDATE)
                    {
                        isDATE = true; // 不可新增明細
                        break;
                    }
                }
                // 3.檢查表頭調班對象帳號(ACCOUNT)與明細調班人員帳號(ACCOUNT)是否相同
                if (hidTMP_TARGET_ACCOUNT.Value == ndr["TMP_EMPLOYEEID_ACCOUNT"].ToString())
                {
                    // 4. 若相同，在檢查調班日期(起)~調班日期(迄)的日期與明細的調班日期任一筆日期相同時，則檔住不可新增明細。
                    if (new_start == dtDATE)
                    {
                        isDATE = true; // 不可新增明細
                        break;
                    }
                }
            }
            new_start = new_start.AddDays(+1);
        }

        //string msg = "[調班日期： ";
        msg_date.Text = "";
        msg_date.Visible = false;
        if (isDATE == false && kdpDATE.Text != "" && kdpEND_DATE.Text != "") // 新增明細
        {
            DateTime start = KYTUtilLibs.Utils.CSharpUtils.TryParseDateTime(kdpDATE.Text); // 取得調班日期(起)
            DateTime end = KYTUtilLibs.Utils.CSharpUtils.TryParseDateTime(kdpEND_DATE.Text); // 取得調班日期(迄)
            int gap_day = (end - start).Days; // 起訖之間的天數

            // 當新增明細時才需要CALL飛騰
            // bool msg_visible = false; // 訊息旗標(預設不顯示false，顯示true)，當日期區間中有一筆(含)以上的資料查無班別時即為True
            string type = kddlADJUSTTYPE.SelectedValue; // 調班類型 0=個人調班；1=人員互調
            ktxtMessage.Text = "";
            string SCS_msg_ALL = "";
            if (type == "0") // 個人調班
            {
                if (hidTMP_CURRWORKID_ID.Value != "")
                {
                    bool b_msg_date = false;
                    for (int i = 0; i <= gap_day; i++)
                    {
                        string new_date = start.ToString("yyyy/MM/dd").Replace("/", ""); // 可變動之調班日期(一筆日期一筆日期傳)
                        msg_date.Text = ""; // 清除原班別不可與新班別相同訊息
                        getSCSHRWORKID(new_date, new_date, hidTMP_EMPLOYEEID_ACCOUNT.Value, "ORI"); // ORI個人調班


                        // 檢查班別代號為空時不新增明細
                        //if (string.IsNullOrEmpty(hidCURRWORKID_ID.Value) || hidCURRWORKID_ID.Value == "")
                        //{
                        //    // 該調班日期檢查為空顯示訊息
                        //    msg += start.ToString("yyyy/MM/dd") + "  ";
                        //    msg_date.Visible = true; // 欄位顯示
                        //    msg_visible = true; // 欄位顯示旗標
                        //}
                        // 檢查調班人員班別與新班別不可相同(第一次新增明細時)
                        //if (hidCURRWORKID_ID.Value == hidTMP_CURRWORKID_ID.Value)
                        //{
                        //    msg = "[原班別不可與新班別相同]";
                        //    msg_date.Visible = true; // 欄位顯示
                        //    msg_date.Text = msg;
                        //}
                        //else  // 新增明細
                        //{ }

                        // 先檢查訊息是否為空，若有訊息([原班別不可與新班別相同])表示不可新增明細
                        if (string.IsNullOrEmpty(msg_date.Text) || msg_date.Text == "")
                        {
                            // 呼叫飛騰檢查，成功 -> 再加入明細項中
                            string SCS_msg = CheckSCSHR_CHGTABLE(start.ToString("yyyyMMdd"));
                            if (string.IsNullOrEmpty(SCS_msg))
                            {
                                DataRow ndr = tblgvItems_D1.NewRow();
                                ndr["ITEM_NO"] = "01"; // 項次
                                ndr["SYS_DATE"] = kdpApplicantDate.Text; // 申請日期
                                ndr["ADJUSTTYPE"] = kddlADJUSTTYPE.SelectedValue == "0" ? "個人調班" : "人員互調"; // 調班性質
                                ndr["TMP_EMPLOYEEID"] = ktxtTMP_EMPLOYEEID.Text; // 調班人員
                                ndr["TMP_EMPLOYEEID_GUID"] = hidTMP_EMPLOYEEID_GUID.Value; // 調班人員GUID
                                ndr["TMP_EMPLOYEEID_ACCOUNT"] = hidTMP_EMPLOYEEID_ACCOUNT.Value; // 調班人員ACCOUNT
                                ndr["TMP_DEPARTID"] = ktxtTMP_EMPLOYEEID_DEP.Text; // 調班人員部門
                                ndr["TMP_DEPARTID_CODE"] = hidTMP_EMPLOYEEID_DEP.Value; // 調班人員部門GroupCode

                                ndr["DATE"] = start.ToString("yyyy/MM/dd"); // 調班日期

                                ndr["TMP_EMPLOYEEWORK"] = ktxtNEW_CURRWORKID.Text; // 調班人員班別
                                ndr["TMP_CURRWORKID"] = ktxtTMP_CURRWORKID.Text; // 新班別
                                ndr["TMP_CURRWORKID_ID"] = hidTMP_CURRWORKID_ID.Value; // 新班別ID(個人調班=新班別ID；人員互調=對象班別ID)
                                ndr["HTYPE"] = hidTMP_CURRWORKID_Htype.Value; // 新班Hype(個人調班=新班別Htype)
                                ndr["NOTE"] = ktxtBKTXT.Text; // 備註
                                tblgvItems_D1.Rows.Add(ndr);
                            }
                            else
                            {
                                SCS_msg_ALL += string.Format("{0}{1}調班日期:{2}，錯誤訊息:{3}", 
                                    SCS_msg_ALL,
                                    !string.IsNullOrEmpty(SCS_msg_ALL) ? "。" : "",
                                    start.ToString("yyyy/MM/dd"),
                                    SCS_msg);
                                KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, "UC_KYTI_SCSHR_CHGTABLE::btnNewGvItemD1_Click()::呼叫飛騰檢查::BOImport::失敗:{0}", SCS_msg);
                            }
                        }
                        else
                        {
                            b_msg_date = true; // 表示有明細「原班別不可與新班別相同」成立
                        }
                        start = start.AddDays(+1); // 後一天
                    }
                    if (b_msg_date)
                    {
                        string msg = "[原班別不可與新班別相同]";
                        msg_date.Text = msg;
                    }
                    ktxtMessage.Text = SCS_msg_ALL;
                }
                else
                {
                    // 未選擇新班別直接新增明細
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), @"alert('請填寫必填欄位');", true);
                }
            }
            else if (type == "1") // 人員互調
            {
                for (int i = 0; i <= gap_day; i++)
                {
                    //  string date = kdpDATE.Text.Replace("/", "");

                    string new_date = start.ToString("yyyy/MM/dd").Replace("/", ""); // 可變動之調班日期(一筆日期一筆日期傳)

                    getSCSHRWORKID(new_date, new_date, hidTMP_EMPLOYEEID_ACCOUNT.Value, "ORI");
                    getSCSHRWORKID(new_date, new_date, hidTMP_TARGET_ACCOUNT.Value, "NEW"); // ORI個人調班

                    // 檢查班別代號為空或調班對象班別代號為空時不新增明細
                    //if (string.IsNullOrEmpty(hidCURRWORKID_ID.Value) || hidCURRWORKID_ID.Value == ""
                    //    || string.IsNullOrEmpty(hidTMP_TCURRWORKID_ID.Value) || hidTMP_TCURRWORKID_ID.Value == "")
                    //{
                    //    // 該調班日期檢查為空顯示訊息
                    //    msg += start.ToString("yyyy/MM/dd") + "  ";
                    //    msg_date.Visible = true;
                    //    msg_visible = true; // 顯示訊息旗標
                    //}

                    // 檢查調班人員班別與調班對象班別不可相同(第一次新增明細時)
                    //if (hidCURRWORKID_ID.Value == hidTMP_TCURRWORKID_ID.Value)
                    //{
                    //    msg = "[調班人員班別與調班對象班別不可相同]";
                    //    msg_date.Visible = true; // 欄位顯示
                    //    msg_date.Text = msg;
                    //}
                    //else // 新增明細
                    //{ }
                    // 當(msg_date)有訊息表示飛騰檢查調班人員班別與調班對象班別相同，不可新增明細
                    if (string.IsNullOrEmpty(msg_date.Text) || msg_date.Text == "")
                    {
                        // 呼叫飛騰檢查，成功 -> 再加入明細項中
                        string SCS_msg = CheckSCSHR_CHGTABLE(start.ToString("yyyyMMdd"));
                        if (string.IsNullOrEmpty(SCS_msg))
                        {
                            DataRow ndr = tblgvItems_D1.NewRow();
                            ndr["ITEM_NO"] = "01"; // 項次
                            ndr["SYS_DATE"] = kdpApplicantDate.Text; // 申請日期

                            ndr["ADJUSTTYPE"] = type == "0" ? "個人調班" : "人員互調"; // 調班性質
                            ndr["DATE"] = start.ToString("yyyy/MM/dd"); // 調班日期

                            ndr["TMP_EMPLOYEEWORK"] = ktxtNEW_CURRWORKID.Text; // 調班人員原班別
                            ndr["TMP_EMPLOYEEWORK_ID"] = hidCURRWORKID_ID.Value; // 調班人員原班別ID

                            ndr["TMP_EMPLOYEEID"] = ktxtTMP_EMPLOYEEID.Text; // 調班人員
                            ndr["TMP_EMPLOYEEID_GUID"] = hidTMP_EMPLOYEEID_GUID.Value; // 調班人員GUID
                            ndr["TMP_EMPLOYEEID_ACCOUNT"] = hidTMP_EMPLOYEEID_ACCOUNT.Value; // 調班人員ACCOUNT
                            ndr["TMP_DEPARTID"] = ktxtTMP_EMPLOYEEID_DEP.Text; // 調班人員部門
                            ndr["TMP_DEPARTID_CODE"] = hidTMP_EMPLOYEEID_DEP.Value; // 調班人員部門GroupCode

                            ndr["TMP_TARGETID"] = ktxtTMP_TARGETID.Text; // 調班對象
                            ndr["TMP_TARGETID_GUID"] = hidTMP_TARGET_GUID.Value; // 調班對象GUID
                            ndr["TMP_TARGETID_ACCOUNT"] = hidTMP_TARGET_ACCOUNT.Value; // 調班對象ACCOUNT
                            ndr["TMP_TDEPARTID"] = ktxtTMP_TARGETID_DEP.Text; // 調班對象部門
                            ndr["TMP_TDEPARTID_CODE"] = hidTMP_TARGETID_DEP.Value; // 調班對象部門GroupCode

                            ndr["TMP_TCURRWORKID"] = ktxtTMP_TCURRWORKID.Text; // 調班對象班別*新(原調班人員班別)
                            ndr["TMP_TCURRWORKID_ID"] = hidTMP_TCURRWORKID_ID.Value; // 調班對象班別ID*新(原調班人員班別ID)
                            ndr["THTYPE"] = hidTMP_TCURRWORKID_Htype.Value; // 調班對象班Htype*新(原調班人員班別ID)

                            ndr["TMP_CURRWORKID"] = ktxtTMP_TCURRWORKID.Text; // 新班別(個人調班=新班別；人員互調=對象班別)
                            ndr["TMP_CURRWORKID_ID"] = hidTMP_TCURRWORKID_ID.Value; // 新班別ID(個人調班=新班別ID；人員互調=對象班別ID)
                            ndr["HTYPE"] = hidTMP_TCURRWORKID_Htype.Value; // 新班Htype(個人調班=新班別Htype；人員互調=對象班別Htype)

                            ndr["TMP_NTCURRWORKID"] = ktxtNEW_CURRWORKID.Text; // 調班對象新班別(調班人員原班別)
                            ndr["NTHTYPE"] = hidCURRWORKID_Htype.Value; // 對象新班Htype(調班人員原*Htype)

                            ndr["NOTE"] = ktxtBKTXT.Text; // 備註
                            tblgvItems_D1.Rows.Add(ndr); // 新增該明細
                        }
                        else
                        {
                            SCS_msg_ALL += string.Format("{0}{1}調班日期:{2}，錯誤訊息:{3}",
                                SCS_msg_ALL,
                                !string.IsNullOrEmpty(SCS_msg_ALL) ? "。" : "",
                                start.ToString("yyyy/MM/dd"),
                                SCS_msg);
                            KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, "UC_KYTI_SCSHR_CHGTABLE::btnNewGvItemD1_Click()::呼叫飛騰檢查::BOImport::失敗:{0}", SCS_msg);
                        }
                    }
                    start = start.AddDays(+1); // 後一天
                }
                ktxtMessage.Text = SCS_msg_ALL;
            }
            //if (msg_visible) // 當日期區間中，有一筆(含)以上無班別資料時，即為True要顯示查無班別日期
            //{
            //    msg += " 查無班別]";
            //    msg_date.Visible = true;
            //    msg_date.Text = msg;
            //}
            //tblgvItems_D1.Rows.Add(ndr);

            this.ResetGridViewITEMNO(tblgvItems_D1, "ITEM_NO"); //重新排序項次編號
                                                                // gvItemsD1.BindDataOnly = true;
            gvItemsD1.DataSource = tblgvItems_D1;
            gvItemsD1.DataBind();
        }
        else if (kdpDATE.Text == "" || kdpEND_DATE.Text == "")
        {
            // 第一次進入表單直接按新增按鈕
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), @"alert('請填寫必填欄位');", true);
        }
        else
        {
            // 已存在相同調班日期
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), @"alert('簽核中表單或此明細項內，調班人員/對象已存在此調班日期');", true);
        }

    }

    /// <summary>
    /// 重新排序Gridview的項次編號
    /// </summary>
    private void ResetGridViewITEMNO(DataTable dt, string ITEM_NO)
    {
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i][ITEM_NO] = (i * 1 + 1).ToString().PadLeft(2, '0');
        }
    }

    /// <summary>
    /// gvItemD1_刪除按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteD1_Click(object sender, EventArgs e)
    {
        if (!this.IsModify()) return;
        ImageButton imgDelPMCostOne = (ImageButton)sender;
        GridViewRow gr = imgDelPMCostOne.NamingContainer as GridViewRow;
        DataTable tblDataTable = (DataTable)gvItemsD1.DataSource;
        tblDataTable.Rows[gr.RowIndex].Delete();
        tblDataTable.AcceptChanges();
        ResetGridViewITEMNO(tblDataTable, "ITEM_NO");
        gvItemsD1.BindDataOnly = true;
        gvItemsD1.DataSource = tblDataTable;
        gvItemsD1.DataBind();
    }

}
