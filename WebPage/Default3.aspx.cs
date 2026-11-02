using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CDS_WebPage_Default3 :BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ((Master_MobileMasterPage)this.Master).Button1Text = "";
    }
}