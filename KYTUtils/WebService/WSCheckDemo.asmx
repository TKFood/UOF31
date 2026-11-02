<%@ WebService Language="C#" Class="WSCheckDemo" %>
// 教學：Class的WSCheckDemo依照更改的檔名修改，必須相同
using System;
using System.Web;
using System.Web.Services;
using System.Xml;
using Ede.Uof.EIP.Organization.Util;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Diagnostics;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class WSCheckDemo : System.Web.Services.WebService
{
// 教學：public class的WSCheckDemo依照更改的檔名修改，必須相同

    [WebMethod]
    public string Check(string formInfo)
    {
        // 教學：這是一個標準的UOF表單的XML，以此為基準取得資料，無須理會
        /*
         <Form formVersionId="9dd4d7b5-f8c5-45cf-bec5-ea1b3c5f8f97">
           <FormFieldValue>
             <FieldItem fieldId="DocNbr" fieldValue="OVE170900001" realValue="" />
             <FieldItem fieldId="Applicant" fieldValue="系統管理員(admin)" realValue="&lt;UserSet&gt;&lt;Element type='user'&gt; &lt;userId&gt;admin&lt;/userId&gt;&lt;/Element&gt;&lt;/UserSet&gt;&#xD;&#xA;" />
             <FieldItem fieldId="ApplicantDept" fieldValue="My Company" realValue="Company,My Company,False" />
             <FieldItem fieldId="ApplicantTitle" fieldValue="經理" realValue="" />
             <FieldItem fieldId="OverTimeWorkInfo" ConditionValue="&lt;ReturnValue&gt;&lt;OvertimeSourse&gt;DefaultOT&lt;/OvertimeSourse&gt;&lt;OvertimeType&gt;Personal&lt;/OvertimeType&gt;&lt;ActualOvertimeHours&gt;15.5&lt;/ActualOvertimeHours&gt;&lt;ExpectOvertimeHours&gt;15.5&lt;/ExpectOvertimeHours&gt;&lt;BU&gt;&lt;/BU&gt;&lt;/ReturnValue&gt;" realValue="" fillerName="系統管理員" fillerUserGuid="admin" fillerAccount="admin" fillSiteId="">
               <OverTimeWorkInfo OvertimeSource="0" OvertimeType="0" OvertimeUserSet="&lt;UserSet&gt;&lt;/UserSet&gt;&#xD;&#xA;" ExpStartDateTime="2017/09/21 00:00" ExpEndDateTime="2017/09/21 15:30" ExpEatHours="0" ExpHours="15.5" ActualStartDateTime="2017/09/21 00:00" ActualEndDateTime="2017/09/21 15:30" ActualEatHours="0" ActualWorkHours="15.5" ActHours="15.5" BelnogDate="2017/09/21" WorkType="N" ChangeType="TimeOff" />
             </FieldItem>
             <FieldItem fieldId="flexible" fieldValue="NO" realValue="" customValue="@null" fillerName="系統管理員" fillerUserGuid="admin" fillerAccount="admin" fillSiteId="" />
             <FieldItem fieldId="Reason" fieldValue="d" realValue="" fillerName="系統管理員" fillerUserGuid="admin" fillerAccount="admin" fillSiteId="" />
           </FormFieldValue>
         </Form>
         */
        XmlDocument result = new XmlDocument();
        // 教學：WS回傳的格式是固定的，不可修改
        result.LoadXml(@"<ReturnValue>
                             <Status>0</Status>
                             <Exception>
                                 <Message />
                             </Exception>
                         </ReturnValue>");
        XmlNode nodeStatus = result.SelectSingleNode("ReturnValue/Status");
        XmlNode nodeException = result.SelectSingleNode("ReturnValue/Exception");
        XmlNode nodeMessage = result.SelectSingleNode("ReturnValue/Exception/Message");

        // 載入表單xml
        XmlDocument doc = new XmlDocument();
        try { doc.LoadXml(formInfo); } // 教學：將表單解析成標準XML
        catch { doc = null; }

        if (doc != null) // 如果載入表單xml成功
        {
            // 教學：變更時建議對nodeTotal修改為「node對應欄位名稱」，例：欄位TEST= nodeTEST；欄位Account = nodeAccount
            // 教學：SelectSingleNode只要更改「@fieldId='total'」內的total就可，例：欄位TEST= @fieldId='TEST'；欄位Account = @fieldId='Account'
            // 教學：為了方便維護，同時建議把「//」後的說明文字也改為正確的描述
            XmlNode nodeTotal = doc.SelectSingleNode("Form/FormFieldValue/FieldItem[@fieldId='total']"); // 取出總申請金額
            if (nodeTotal != null) // 如果有找到總申請金額
            {
                // 教學：UOF的結構中，每個節點要的內容可能不同，如果需要的是帳號，那Attributes內的文字就會是.Attributes["realValue"]
                // 教學：節點中的屬性fieldValue還要使用「Value」來拿到實際存的文字
                // 教學：所有取出來的內容都是文字，不是數字、日期、或其他，所以取出來之後如果要判斷，建議轉型成正確的型別
                string strTotal = nodeTotal.Attributes["fieldValue"].Value; // 取出總申請金額
                int Itotal = 0;
                int.TryParse(strTotal, out Itotal); // 將文字版的總申請金額變成數字版(能夠進行運算的)的總申請金額
                // 教學：每個需求都不同，這裡的需求是檢查總申請金額是否大於2000
                if (Itotal > 2000)
                {
                    nodeMessage.InnerText = "總申請金額不得大於2000"; // 要送回去顯示的訊息
                    goto 失敗離開; // 教學：這會直接跳到下面的「失敗離開」，失敗離開到這裡所有的程式碼都不會被執行
                }
                // 到此驗證過關
                result.DocumentElement.RemoveChild(nodeException);
                nodeStatus.InnerText = "1";
            }
            else // 如果沒找到總申請金額
            {
                // 教學：這裡的「總申請金額」文字依照需求更改
                nodeMessage.InnerText = "錯誤的表單格式, 缺少總申請金額欄位";
            }
        }
        else // 如果載入表單xml失敗
        {
            nodeMessage.InnerText = "呼叫WebService參數錯誤";
        }

    失敗離開:;
        // 教學：LOG內的WSCheckDemo依照更改的檔名修改，必須相同
        JGlobalLibs.DebugLog.Log(string.Format("WSCheckDemo.asmx::Check::準備被WS回傳的內容::{0}", result.OuterXml));
        return result.OuterXml;
    }

}