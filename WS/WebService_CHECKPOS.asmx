<%@ WebService Language="C#" Class="WebService_CHECKPOS" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Xml;
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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Log;
using Ede.Uof.Utility.Page.Common;
using Ede.Uof.WKF.Utility;
using Ede.Uof.WKF.Engine;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class WebService_CHECKPOS : System.Web.Services.WebService
{

    //[WebMethod]
    //public string GetFieldValue(string formInfo)
    //{
    //}



    //[WebMethod]
    //public string GetFieldValueByDDL(string formInfo)
    //{
    //}


    [WebMethod]
    public string CheckedForm(string formInfo)
    {

        // - <Form formVersionId="684034be-3cf5-4d47-9710-2ae8a090875c">
        //- <FormFieldValue>
        //  <FieldItem fieldId="NO" fieldValue="" realValue="" /> 
        //  <FieldItem fieldId="A01" fieldValue="1" realValue="" fillerName="湯尼" fillerUserGuid="cb7b9862-6591-4b28-a6e6-c54d57eb00c7" fillerAccount="Tony" fillSiteId="" /> 
        //  <FieldItem fieldId="A02" fieldValue="1" realValue="" fillerName="湯尼" fillerUserGuid="cb7b9862-6591-4b28-a6e6-c54d57eb00c7" fillerAccount="Tony" fillSiteId="" /> 
        //  <FieldItem fieldId="A03" fieldValue="1" realValue="" fillerName="湯尼" fillerUserGuid="cb7b9862-6591-4b28-a6e6-c54d57eb00c7" fillerAccount="Tony" fillSiteId="" /> 
        //  <FieldItem fieldId="A04" fieldValue="" realValue="" /> 
        //  </FormFieldValue>
        //  </Form>
        formInfo = HttpUtility.UrlDecode(formInfo);
        XmlDocument formInfoxmlDoc = new XmlDocument();
        formInfoxmlDoc.LoadXml(formInfo);
        string DOC_NBR = formInfoxmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='ID']").Attributes["fieldValue"].Value;
        string MB003 = formInfoxmlDoc.SelectSingleNode("/Form/FormFieldValue/FieldItem[@fieldId='FIELD002']").Attributes["fieldValue"].Value;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(formInfo);

        XmlDocument returnXmlDoc = new XmlDocument();
        XmlElement returnValueElement = returnXmlDoc.CreateElement("ReturnValue");
        XmlElement statusElement = returnXmlDoc.CreateElement("Status");
        XmlElement exceptionElement = returnXmlDoc.CreateElement("Exception");
        XmlElement messageElement = returnXmlDoc.CreateElement("Message");

        returnValueElement.AppendChild(statusElement);
        exceptionElement.AppendChild(messageElement);
        returnValueElement.AppendChild(exceptionElement);
        returnXmlDoc.AppendChild(returnValueElement);

        try
        {
            StringBuilder MESSAGES = new StringBuilder();
            //有重疊日期的品號
            DataTable DT = CHECK_POSMB_POSMI_POSMO(MB003);
            if (DT != null && DT.Rows.Count >= 1)
            {

                MESSAGES.AppendFormat(@"");
                foreach (DataRow DR in DT.Rows)
                {
                    MESSAGES.AppendFormat(@"已退簽給申請者  ");
                    MESSAGES.AppendFormat(@"-------------------- ");
                    MESSAGES.AppendFormat(@"有錯誤，無法簽核  ");
                    MESSAGES.AppendFormat(@"代號 ={0} 的 品號 ={1} 有重疊的活動日期 ", DR["MB2MB003"].ToString().Trim(), DR["MC004"].ToString().Trim());
                }
                //不允許簽核
                returnValueElement.SelectSingleNode("/ReturnValue/Status").InnerText = "0";
                returnValueElement.SelectSingleNode("/ReturnValue/Exception/Message").InnerText = MESSAGES.ToString();

                DataTable DT_FIND_UOD_TASKID_SITEID = FIND_UOD_TASKID_SITEID(DOC_NBR);
                if (DT_FIND_UOD_TASKID_SITEID != null && DT_FIND_UOD_TASKID_SITEID.Rows.Count >= 1)
                {
                    string TASK_ID = DT_FIND_UOD_TASKID_SITEID.Rows[0]["TASK_ID"].ToString();
                    string CURRENT_SITE_ID = DT_FIND_UOD_TASKID_SITEID.Rows[0]["CURRENT_SITE_ID"].ToString();
                    int NODE_SEQ = Convert.ToInt32(DT_FIND_UOD_TASKID_SITEID.Rows[0]["NODE_SEQ"].ToString());
                    string USER_GUID = DT_FIND_UOD_TASKID_SITEID.Rows[0]["USER_GUID"].ToString();

                    UPDATE_TB_WKF_TASK_NODE_COMMENT(TASK_ID, CURRENT_SITE_ID, MESSAGES.ToString());

                    ReturnSignUCO returnUCO = new ReturnSignUCO();
                    /// <summary>
                    /// 退回到申請者
                    /// </summary>
                    /// <param name="taskId">taskId</param>
                    /// <param name="currentSiteId">目前站點</param>
                    /// <param name="currentNodeSeq">目前節點</param>
                    /// <param name="actualSigner"></param>
                    /// <param name="isFreeTask">是否為自由流程</param>
                    /// <param name="source">表單處理來源</param>

                    //returnUCO.ReturnToApplicant(
                    //TASK_ID,
                    //CURRENT_SITE_ID,
                    //NODE_SEQ,
                    //Current.UserGUID,
                    //false,
                    //Source.External.ToString());

                    returnUCO.ReturnToApplicant(
                    TASK_ID,
                    CURRENT_SITE_ID,
                    0,
                    USER_GUID,
                    false,
                    Source.External.ToString());

                    //Dialog.SetReturnValue2("PostBack");                            
                    //Dialog.Close(this);
                }

            }
            else
            {
                //允許簽核
                returnValueElement.SelectSingleNode("/ReturnValue/Status").InnerText = "1";
            }


        }
        catch (Exception ce)
        {
            returnValueElement.SelectSingleNode("/ReturnValue/Status").InnerText = "0";
            returnValueElement.SelectSingleNode("/ReturnValue/Exception/Message").InnerText = ce.Message;
        }

        return returnValueElement.OuterXml;
    }


    //[WebMethod]
    //public string FormSignEvent(string formInfo)
    //{
    //}


    //[WebMethod]
    //public string EndFormEvent(string formInfo)
    //{
    //}

    /// <summary>
    /// POSMB 商品促銷設定單頭檔
    /// POSMI 組合品搭贈設定單頭檔
    /// POSMO 配對搭贈設定單頭檔
    /// </summary>
    /// <param name="MB003"></param>
    /// <returns></returns>
    public DataTable CHECK_POSMB_POSMI_POSMO(string MB003)
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
                                    SELECT MB001,MB003,MB002,MB012,MB013,MC004,MF004,MB2MB001,MB2MB003,MB2MB012,MB2MB013,MC2MC004,MF2MF004
                                    FROM [TK].dbo.POSMB
                                    INNER JOIN [TK].dbo.POSMC ON MB003 = MC003
                                    LEFT JOIN [TK].dbo.POSMF ON MF003=MB003
                                    INNER JOIN
                                    (    
                                    SELECT MB2.MB001 AS MB2MB001,MB2.MB003 AS MB2MB003,MB2.MB002 AS MB2MB002,MB2.MB012 AS MB2MB012 ,MB2.MB013 AS MB2MB013, MC2.MC004 AS MC2MC004,MF2.MF004 AS'MF2MF004'
                                    FROM [TK].dbo.POSMB AS MB2
                                    INNER JOIN [TK].dbo.POSMC AS MC2 ON MB2.MB003 = MC2.MC003
                                    LEFT JOIN [TK].dbo.POSMF AS MF2 ON MF2.MF003=MB2.MB003
                                    ) 
                                    AS TEMP  ON  TEMP.MC2MC004 IS NOT NULL
                                    AND TEMP.MB2MB003 <> POSMB.MB003
                                    AND TEMP.MC2MC004 = POSMC.MC004
                                    AND ((TEMP.MF2MF004=POSMF.MF004) OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')<>'') OR (ISNULL(TEMP.MF2MF004,'')<>'' AND ISNULL(POSMF.MF004,'')='') OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')=''))
                                    AND 
                                    (
                                    MB012 BETWEEN MB2MB012 AND MB2MB013
                                    OR MB013 BETWEEN MB2MB012 AND MB2MB013
                                    OR (MB2MB012 BETWEEN MB012 AND MB013)
                                    OR (MB2MB013 BETWEEN MB012 AND MB013)
                                    )
                                    WHERE MC004 IS NOT NULL
                                    AND MB003 = '{0}'
                                    UNION ALL

                                    SELECT MI001,MI003,MI002,MI005,MI006,MJ004,MF004,MI2MI001,MI2MI003,MI2MI005,MI2MI006,MJ2MJ004,MF2MF004
                                    FROM [TK].dbo.POSMI
                                    INNER JOIN [TK].dbo.POSMJ ON MI003 = MJ003
                                    LEFT JOIN [TK].dbo.POSMF ON MF003=MI003
                                    INNER JOIN
                                    (    
                                    SELECT MI2.MI001 AS MI2MI001,MI2.MI003 AS MI2MI003,MI2.MI002 AS MI2MI002,MI2.MI005 AS MI2MI005 ,MI2.MI006 AS MI2MI006, MJ2.MJ004 AS MJ2MJ004,MF2.MF004 AS'MF2MF004'
                                    FROM [TK].dbo.POSMI AS MI2
                                    INNER JOIN [TK].dbo.POSMJ AS MJ2 ON MI2.MI003 = MJ2.MJ003
                                    LEFT JOIN [TK].dbo.POSMF AS MF2 ON MF2.MF003=MI2.MI003
                                    ) 
                                    AS TEMP  ON  TEMP.MJ2MJ004 IS NOT NULL
                                    AND TEMP.MI2MI003 <> POSMI.MI003
                                    AND TEMP.MJ2MJ004 = POSMJ.MJ004
                                    AND ((TEMP.MF2MF004=POSMF.MF004) OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')<>'') OR (ISNULL(TEMP.MF2MF004,'')<>'' AND ISNULL(POSMF.MF004,'')='') OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')=''))
                                    AND 
                                    (
                                    MI005 BETWEEN MI2MI005 AND MI2MI006
                                    OR MI006 BETWEEN MI2MI005 AND MI2MI006
                                    OR (MI2MI005 BETWEEN MI005 AND MI006)
                                    OR (MI2MI006 BETWEEN MI005 AND MI006)
                                    )
                                    WHERE MJ004 IS NOT NULL
                                    AND MI003 = '{0}'

                                    UNION ALL
                                    SELECT MO001,MO003,MO002,MO011,MO012,MP005,MF004,MO2MO001,MO2MO003,MO2MO011,MO2MO012,MP2MP005,MF2MF004
                                    FROM [TK].dbo.POSMO
                                    INNER JOIN [TK].dbo.POSMP ON MO003 = MP003
                                    LEFT JOIN [TK].dbo.POSMF ON MF003=MO003
                                    INNER JOIN
                                    (    
                                    SELECT MO2.MO001 AS MO2MO001,MO2.MO003 AS MO2MO003,MO2.MO002 AS MO2MO002,MO2.MO011 AS MO2MO011 ,MO2.MO012 AS MO2MO012, MP2.MP005 AS MP2MP005,MF2.MF004 AS'MF2MF004'
                                    FROM [TK].dbo.POSMO AS MO2
                                    INNER JOIN [TK].dbo.POSMP AS MP2 ON MO2.MO003 = MP2.MP003
                                    LEFT JOIN [TK].dbo.POSMF AS MF2 ON MF2.MF003=MO2.MO003
                                    ) 
                                    AS TEMP  ON  TEMP.MP2MP005 IS NOT NULL
                                    AND TEMP.MO2MO003 <> POSMO.MO003
                                    AND TEMP.MP2MP005 = POSMP.MP005
                                    AND ((TEMP.MF2MF004=POSMF.MF004) OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')<>'') OR (ISNULL(TEMP.MF2MF004,'')<>'' AND ISNULL(POSMF.MF004,'')='') OR (ISNULL(TEMP.MF2MF004,'')='' AND ISNULL(POSMF.MF004,'')=''))
                                    AND 
                                    (
                                    MO011 BETWEEN MO2MO011 AND MO2MO012
                                    OR MO012 BETWEEN MO2MO011 AND MO2MO012
                                    OR (MO2MO011 BETWEEN MO011 AND MO012)
                                    OR (MO2MO012 BETWEEN MO011 AND MO012)
                                    )
                                    WHERE MP005 IS NOT NULL
                                    AND MO003 =  '{0}'
                                    ) AS TEMP

                              
                                    ", MB003);




            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
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
        { }
    }

    public DataTable FIND_UOD_TASKID_SITEID(string DOC_NBR)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        StringBuilder cmdTxt = new StringBuilder();
        StringBuilder QUERYS = new StringBuilder();
        try
        {

            cmdTxt.AppendFormat(@"                                    
                                SELECT DOC_NBR,TB_WKF_TASK.TASK_ID,CURRENT_SITE_ID,SITE_ID,NODE_SEQ,USER_GUID
                                FROM [UOF].dbo.TB_WKF_TASK,[UOF].dbo.TB_WKF_TASK_NODE 
                                WHERE 1=1
                                AND TB_WKF_TASK.TASK_ID=TB_WKF_TASK_NODE.TASK_ID
                                AND TB_WKF_TASK.CURRENT_SITE_ID=TB_WKF_TASK_NODE.SITE_ID
                                AND DOC_NBR='{0}'


                              
                                    ", DOC_NBR);

            //m_db.AddParameter("@SDATE", SDATE);
            //m_db.AddParameter("@EDATE", EDATE);

            DataTable dt = new DataTable();

            dt.Load(m_db.ExecuteReader(cmdTxt.ToString()));

            if (dt != null && dt.Rows.Count >= 1)
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
        { }
    }

    public void UPDATE_TB_WKF_TASK_NODE_COMMENT(string TASK_ID, string SITE_ID,string COMMENT)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"   ";


        cmdTxt = @"
                    UPDATE [UOF].dbo.TB_WKF_TASK_NODE 
                    SET COMMENT=@COMMENT
                    WHERE TASK_ID=@TASK_ID AND SITE_ID=@SITE_ID
                        ";

        m_db.AddParameter("@TASK_ID", TASK_ID);
        m_db.AddParameter("@SITE_ID", SITE_ID);
        m_db.AddParameter("@COMMENT", COMMENT);

        m_db.ExecuteNonQuery(cmdTxt);
    }

}

