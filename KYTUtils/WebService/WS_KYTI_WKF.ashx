<%@ WebHandler Language="C#" Class="WS_KYTI_WKF" %>

using System;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class WS_KYTI_WKF : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string method = !string.IsNullOrEmpty(context.Request["method"]) ? HttpUtility.UrlDecode(context.Request["method"]) : "WS_KYTI_WKF_GET";
        string SYS_ID = !string.IsNullOrEmpty(context.Request["SYS_ID"]) ? HttpUtility.UrlDecode(context.Request["SYS_ID"]) : "";
        string EXT_DOCNBR = !string.IsNullOrEmpty(context.Request["EXT_DOCNBR"]) ? HttpUtility.UrlDecode(context.Request["EXT_DOCNBR"]) : "";
        string DOC_NBR = !string.IsNullOrEmpty(context.Request["DOC_NBR"]) ? HttpUtility.UrlDecode(context.Request["DOC_NBR"]) : "";
        string FORM_NAME = !string.IsNullOrEmpty(context.Request["FORM_NAME"]) ? HttpUtility.UrlDecode(context.Request["FORM_NAME"]) : "";
        string ACCOUNT = !string.IsNullOrEmpty(context.Request["ACCOUNT"]) ? HttpUtility.UrlDecode(context.Request["ACCOUNT"]) : "";
        string FORM_DATA = !string.IsNullOrEmpty(context.Request["FORM_DATA"]) ? HttpUtility.UrlDecode(context.Request["FORM_DATA"]) : "";

        JGlobalLibs.DebugLog.Log(string.Format(@"METHOD:{0}, 
            SYS_ID: {1},
            EXT_DOCNBR: {2},
            DOC_NBR: {3},
            FORM_NAME: {4},
            ACCOUNT: {5},
            FORM_DATA: {6}",
                    method,
                    SYS_ID,
                    EXT_DOCNBR,
                    DOC_NBR,
                    FORM_NAME,
                    ACCOUNT,
                    FORM_DATA
            ));
        string returnVal = "";

        string uofurl = JGlobalLibs.UOFUtils.getUOFConfig("SiteUrl");
        string wsurl = string.Format(@"{0}/CDS/GD/WebService/WS_KYTI_WKF.asmx", uofurl);
        //string wsurl = @"http://service.kyti.com.tw:888/UOF1251/CDS/GD/WebService/WS_KYTI_WKF.asmx";
        string result = "";
        string[] args;
        switch (method)
        {
            case "WS_KYTI_WKF_GET":
                JGlobalLibs.SoapWebService sws = new JGlobalLibs.SoapWebService().SetURL(wsurl).SetMethod("WS_KYTI_WKF_GET");
                result = sws.Invoke<string>(new { SYS_ID = SYS_ID, EXT_DOCNBR = EXT_DOCNBR, DOC_NBR = DOC_NBR });
                //args = new string[] { SYS_ID, EXT_DOCNBR, DOC_NBR };
                //result = JGlobalLibs.WebService.InvokeWebService(wsurl, "WS_KYTI_WKF_GET", args).ToString();
                break;
            case "WS_KYTI_WKF_ADD":
                JGlobalLibs.SoapWebService _sws = new JGlobalLibs.SoapWebService().SetURL(wsurl).SetMethod("WS_KYTI_WKF_ADD");
                result = _sws.Invoke<string>(new { SYS_ID = SYS_ID, EXT_DOCNBR = EXT_DOCNBR, FORM_NAME = FORM_NAME, ACCOUNT = ACCOUNT, FORM_DATA = FORM_DATA });
                //args = new string[] { EXT_DOCNBR, SYS_ID, FORM_NAME, ACCOUNT, FORM_DATA };
                //result = JGlobalLibs.WebService.InvokeWebService(wsurl, "WS_KYTI_WKF_ADD", args).ToString();
                break;
            default:
                result = "{'ErrorCode':'99998'}";
                break;
        }
        try
        {
            returnVal = result.ToString();
        }
        catch
        {
            returnVal = "{'ErrorCode':'99999'}";
        }
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Write(returnVal);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}