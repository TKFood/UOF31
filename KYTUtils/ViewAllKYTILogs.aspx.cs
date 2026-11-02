using Ede.Uof.Utility.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using UOFAssist.WKF;

public partial class CDS_KYTUtils_ViewAllKYTILogs : BasePage
{
    private static string appConfigPath = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("冠永騰專用LOG觀看", Request.Url.AbsoluteUri);
            //// 綁定設定檔名稱
            ddlAllDDLConfig.DataSource = getDDLConfigName();
            ddlAllDDLConfig.DataTextField = "FILENAME_WEXT";
            ddlAllDDLConfig.DataValueField = "FILEPATH";
            ddlAllDDLConfig.DataBind();
            ddlAllDDLConfig_SelectedIndexChanged(ddlAllDDLConfig, null);
        }
    }

    #region 非控制項事件
    /// <summary>
    /// 找到所有的設定檔名稱及路徑
    /// </summary>
    /// <returns></returns>
    private DataTable getDDLConfigName()
    {
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("FILEPATH", typeof(string))); // 檔名
        dtSource.Columns.Add(new DataColumn("FILENAME_WEXT", typeof(string))); // 檔名去副檔名
        foreach (string _file in Directory.GetFiles(Path.Combine(Request.PhysicalApplicationPath, "bin"), "*.dll.config"))
        {
            DataRow ndr = dtSource.NewRow();
            ndr["FILEPATH"] = _file;
            ndr["FILENAME_WEXT"] = Path.GetFileName(_file).Split('.')[0];
            dtSource.Rows.Add(ndr);
        }
        return dtSource;
    }

    private DataTable getLogFileName(string floderPath)
    {
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("FILEPATH", typeof(string))); // 檔名
        dtSource.Columns.Add(new DataColumn("FILENAME_WEXT", typeof(string))); // 檔名去副檔名
        DataRow ondr = dtSource.NewRow();
        ondr["FILEPATH"] = "";
        ondr["FILENAME_WEXT"] = "===請選擇===";
        dtSource.Rows.Add(ondr);
        if (Directory.Exists(floderPath))
        {
            foreach (string _file in Directory.GetFiles(floderPath, "*.log"))
            {
                DataRow ndr = dtSource.NewRow();
                ndr["FILEPATH"] = _file;
                ndr["FILENAME_WEXT"] = Path.GetFileName(_file).Split('.')[0];
                dtSource.Rows.Add(ndr);
            }
        }

        return dtSource;
    }
    #endregion 非控制項事件

    #region 控制項事件
    /// <summary>
    /// 選擇設定檔下拉事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAllDDLConfig_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList _ddlAllDDLConfig = (DropDownList)sender;
        lblMessage.Text = "";
        if (_ddlAllDDLConfig.Items.Count > 0)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_ddlAllDDLConfig.SelectedValue);
            XmlNode node = doc.SelectSingleNode("//configuration/appSettings/add[@key='LOG路徑']");
            appConfigPath = node != null ? node.Attributes["value"].Value : "";
        }
        else
        {
            lblMessage.Text = "找不到任何設定檔";
        }

    }

    protected void kytdpLocation_TextChanged(object sender, EventArgs e)
    {
        KYTDatePicker _kytdpLocation = (KYTDatePicker)sender;
        if (!string.IsNullOrEmpty(_kytdpLocation.Text.Trim()))
        {
            ddlAllFiles.DataSource = getLogFileName(Path.Combine(appConfigPath, _kytdpLocation.Text.Trim()));
            ddlAllFiles.DataTextField = "FILENAME_WEXT";
            ddlAllFiles.DataValueField = "FILEPATH";
            ddlAllFiles.DataBind();
        }
    }

    protected void rdpLocation_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(appConfigPath))
        {
            if (rdpLocation.SelectedDate.HasValue)
            {
                ddlAllFiles.DataSource = getLogFileName(Path.Combine(appConfigPath, rdpLocation.SelectedDate.Value.ToString("yyyyMMdd")));
                ddlAllFiles.DataTextField = "FILENAME_WEXT";
                ddlAllFiles.DataValueField = "FILEPATH";
                ddlAllFiles.DataBind();
                txtLogMsg.Text = "";
            }
        }

    }

    protected void btnRead_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlAllFiles.SelectedValue))
        {
            if (File.Exists(ddlAllFiles.SelectedValue))
            {
                txtLogMsg.Text = File.ReadAllText(ddlAllFiles.SelectedValue);
            }
        }
    }

    #endregion 控制項事件
}

