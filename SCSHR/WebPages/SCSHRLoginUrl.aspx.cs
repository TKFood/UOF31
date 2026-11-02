using Ede.Uof.EIP.SystemInfo;
using Ede.Uof.Utility.Page;
using SCSHR;
using SCSHR.Utils;
using System;
using System.Web.UI;

public partial class CDS_SCSHR_WebPages_SCSHRLoginUrl : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("", Request.Url.AbsoluteUri);
            SCSServicesProxy service = ConstructorCommonSettings.setSCSSServiceProxDefault();
            ConstructorCommonSettings.setCommonSettings(); // 設定DebugLog初始值
            string sessionGuid = service.CreateSessionGuid(SCSHR.SCSHRConfiguration.SCSServicesCompanyId, Current.Account, "");
            string RedirectUrl = string.Format(@"{0}?SessionGuid={1}", SCSHRConfiguration.ScshrLoginUrl, sessionGuid);

            Page.Response.Redirect(RedirectUrl);
        }
    }
}
