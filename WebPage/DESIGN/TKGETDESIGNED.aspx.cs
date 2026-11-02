using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public partial class CDS_WebPage_DESIGN_TKGETDESIGNED : Ede.Uof.Utility.Page.BasePage
{
    // 設定實體路徑 Y 槽
    private string RootPath = @"Y:\老楊食品\04.門市";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                // 1. 確保 Y 槽已經掛載 (請填入正確帳密)
                // 注意：這裡掛載到根目錄即可
                NetworkDrive.MapDrive("Y:", @"\\192.168.1.199\美工檔案區", @"tkfood-tw\ecd_01", "at160115@@");

                string relPath = Request.QueryString["path"] ?? "";
                BindData(relPath);
            }
            catch (Exception ex)
            {
                Response.Write("磁碟連線失敗: " + ex.Message);
            }
        }
    }

    private void BindData(string fullPath)
    {
        try
        {
            // 增加一個防呆：如果路徑真的為空，就不要執行讀取
            if (string.IsNullOrEmpty(fullPath))
            {
                litCurrentPath.Text = "請在上方輸入路徑並點擊讀取。";
                rptItems.DataSource = null;
                rptItems.DataBind();
                return;
            }

            // 確保磁碟機掛載 (只需掛載一次 Y:)
            NetworkDrive.MapDrive("Y:", @"\\192.168.1.199\美工檔案區", @"tkfood-tw\ecd_01", "at160115@@");

            if (!Directory.Exists(fullPath))
            {
                litCurrentPath.Text = "錯誤：找不到路徑 " + fullPath;
                return;
            }

            DirectoryInfo di = new DirectoryInfo(fullPath);
            litCurrentPath.Text = "目前位置: " + di.FullName;

            // 返回上一層邏輯
            if (di.Parent != null && di.FullName.Length > 3) // 不超過 Y:\
            {
                hlBack.Visible = true;
                hlBack.NavigateUrl = "TKGETDESIGNED.aspx?path=" + HttpUtility.UrlEncode(di.Parent.FullName);
            }
            else { hlBack.Visible = false; }

            // 1. 取得子目錄
            var folders = di.GetDirectories().Select(d => new {
                Name = d.Name,
                FullPath = d.FullName,
                Type = "Folder",
                // 補上前端 Eval 需要的 LinkUrl
                LinkUrl = "TKGETDESIGNED.aspx?path=" + HttpUtility.UrlEncode(d.FullName)
            });

            // 2. 取得圖片
            string[] exts = { ".jpg", ".jpeg", ".png", ".gif" };
            var files = di.GetFiles().Where(f => exts.Contains(f.Extension.ToLower())).Select(f => new {
                Name = f.Name,
                FullPath = f.FullName,
                Type = "File",
                // 檔案的 LinkUrl 其實就是呼叫 ashx
                LinkUrl = "TKGETDESIGNED_ImageHandler.ashx?fullPath=" + HttpUtility.UrlEncode(f.FullName)
            });

            // 合併並綁定
            var allItems = folders.Cast<object>().Concat(files.Cast<object>()).ToList();
            rptItems.DataSource = allItems;
            rptItems.DataBind();
        }
        catch (Exception ex)
        {
            litCurrentPath.Text = "執行錯誤: " + ex.Message;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        BindData(txtPath.Text.Trim());
    }
}