using Ede.Uof.Utility.Page;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.UI;

public partial class CDS_KYTUtils_KYTGridViewTable : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("將KYTGridView轉換為資料表", Request.Url.AbsoluteUri);
        }
    }

    protected void btnConvert_Click(object sender, EventArgs e)
    {
        byte[] bytes = Convert.FromBase64String((string)txtKYTGridViewStruct.Text.Trim());
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            txtOutPutTable.Text = Newtonsoft.Json.JsonConvert.SerializeObject(formatter.Deserialize(ms));
        }
    }
}
