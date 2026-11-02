using Ede.Uof.Utility.Page;
using KYTIDocument;
using KYTLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UOFAssist.WKF;

/// <summary>
/// 使用冠永騰規格建立SQL語法
/// </summary>
public partial class CDS_KYTUtils_TestTemplateWordDownload : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("使用冠永騰模板套印的測試下載工具", Request.Url.AbsoluteUri);

            if (gvHeader.DataSource == null) { gvHeader.DataSource = this.CreateTable(); gvHeader.DataBind(); }
            if (gvContent.DataSource == null) { gvContent.DataSource = this.CreateTable(); gvContent.DataBind(); }
            HttpContext current = HttpContext.Current;
            if (string.IsNullOrEmpty(txtPath.Text)) txtPath.Text = current.Server.MapPath(string.Format(@"~\CDS\KYTUtils\Assets\Template\{0}.docx", "模板")); //預設模板路徑
        }
    }

    #region 非控制事項事件
    /// <summary>
    /// GridView新增明細
    /// </summary>
    private void GridViewRowAdd(KYTGridView _kgv, Dictionary<string, object> param = null)
    {
        DataTable dt = _kgv.DataTable;
        DataRow dr = dt.NewRow();
        if (param != null) foreach (KeyValuePair<string, object> item in param) dr[item.Key] = item.Value; //放入自訂欄位值
        dt.Rows.Add(dr);
        //if (!string.IsNullOrEmpty(ITEMNO)) this.ResetGridViewITEMNO(dt, ITEMNO); //重新排序項目編號
        _kgv.DataSource = dt;
        _kgv.DataBind();
    }

    /// <summary>
    /// GridView刪除明細
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewRowDelete(object sender, KYTGridView _kgv)
    {
        Button imgDelPMCostOne = (Button)sender;
        GridViewRow gr = imgDelPMCostOne.NamingContainer as GridViewRow;
        DataTable dt = _kgv.DataTable;
        dt.Rows[gr.RowIndex].Delete();
        dt.AcceptChanges();
        //if (!string.IsNullOrEmpty(ITEMNO)) this.ResetGridViewITEMNO(dt, ITEMNO);
        _kgv.DataSource = dt;
        _kgv.DataBind();
    }

    //private void SaveDataTable()
    //{
    //    DataTable dt = gvHeader.DataTable;
    //    foreach (GridViewRow gvr in gvHeader.Rows)
    //    {
    //        dt.Rows[gvr.DataItemIndex]["ID"] = gvr.FindControl("txtID");
    //            }
    //}
    #endregion 非控制事項事件

    /// <summary>
    /// 建立明細
    /// </summary>
    /// <returns></returns>
    private DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ID", typeof(String)));
        dt.Columns.Add(new DataColumn("VALUE", typeof(String)));
        return dt;
    }


    /// <summary>
    /// 下載列印模板
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Dictionary<string, KYTIDocxData> InputDatas = new Dictionary<string, KYTIDocxData>();

        foreach (DataRow dr in gvHeader.DataTable.Rows) { string ID = dr["ID"].ToString(); if (!string.IsNullOrEmpty(ID)) InputDatas.Add(ID, new KYTIDocxData(dr["VALUE"].ToString())); }

        DataTable dt = new DataTable();
        foreach (DataRow dr in gvContent.DataTable.Rows) dt.Columns.Add(new DataColumn(dr["ID"].ToString(), typeof(String)));
        int count = 0;
        int.TryParse(this.txtContentCount.Text, out count);
        for (int i = 0; i < count; i++)
        {
            DataRow row = dt.NewRow();
            foreach (DataRow dr in gvContent.DataTable.Rows)
            {
                row[dr["ID"].ToString()] = dr["VALUE"].ToString();
            }
            dt.Rows.Add(row);
        }
        string[] dtID = this.txtContentID.Text.Split(',');
        foreach (string id in dtID) InputDatas.Add(id, new KYTIDocxData(dt));

        HttpContext current = HttpContext.Current;
        string templatepath = this.txtPath.Text; //模板路徑

        string tempFolder = current.Server.MapPath(@"~\App_Data\KYTUtils\Temp"); //Temp資料夾路徑
        string docpath = string.Format(@"{0}\{1}_{2}", tempFolder, DateTime.Now.ToString("yyyyMMddHHmmss"), Path.GetFileName(templatepath)); //輸出word檔路徑
        if (!Directory.Exists(tempFolder)) // 若暫存資料夾不存在則建立
        {
            try { Directory.CreateDirectory(tempFolder); }
            catch (Exception ex) { KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, string.Format("TestTemplateWordDownload::列印::建立資料夾失敗::原因:{0}", ex.Message)); };
        }
        File.WriteAllBytes(docpath, KYTIDocument.KYTIDocx.MakeDocx(templatepath, InputDatas));

        ScriptManager.RegisterClientScriptBlock(
             UpdatePanel1,
             UpdatePanel1.GetType(),
             Guid.NewGuid().ToString(),
             string.Format(@"
                document.addEventListener('DOMContentLoaded', function() {{
                    window.location = '{0}?filepath={1}';
                }});
             "
             , Page.ResolveUrl("~/CDS/KYTUtils/WebService/FORMPRINT/DownFileWithPath.ashx")
             , HttpUtility.UrlEncode(docpath)),
             true);
    }

    protected void btnHeaderNew_Click(object sender, EventArgs e)
    {
        this.GridViewRowAdd(gvHeader);
    }

    protected void btnContentNew_Click(object sender, EventArgs e)
    {
        this.GridViewRowAdd(gvContent);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.GridViewRowDelete(sender, gvHeader);
    }

    protected void btnDelete2_Click(object sender, EventArgs e)
    {
        this.GridViewRowDelete(sender, gvContent);
    }

    protected void gvHeader_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gvHeader.ViewType = KYTViewType.Input;
    }

    protected void gvContent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gvContent.ViewType = KYTViewType.Input;
    }
}
