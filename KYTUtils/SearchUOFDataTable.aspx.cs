using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using System.Web.UI;

public partial class CDS_KYTUtils_SearchUOFDataTable : BasePage
{
    string ConnectionString = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionString = new DatabaseHelper().Command.Connection.ConnectionString;
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("", Request.Url.AbsoluteUri);

            
        }
    }

    [WebMethod]
    public static string getAllTableName(string wtf)
    {
        JObject joTables = new JObject();
        JArray jaTables = new JArray();
        joTables.Add(new JProperty("TABLES", jaTables));
        using (SqlDataAdapter sda = new SqlDataAdapter(@" 
                SELECT TABLE_NAME
                  FROM INFORMATION_SCHEMA.TABLES
                 ORDER BY TABLE_NAME;", new DatabaseHelper().Command.Connection.ConnectionString))
        using (DataSet ds = new DataSet())
        {
            sda.SelectCommand.Parameters.AddWithValue("", "");
            try
            {
                if (sda.Fill(ds) > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        JObject _jTable = new JObject();
                        _jTable.Add("label", dr["TABLE_NAME"].ToString());
                        jaTables.Add(_jTable);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        return joTables.ToString();
    }
}
