<%@ WebHandler Language="C#" Class="TKGETDESIGNED_ImageHandler" %>

using System;
using System.Web;
using System.IO;
using System.Runtime.InteropServices;

public class TKGETDESIGNED_ImageHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string fullPath = context.Request.QueryString["fullPath"];
        if (string.IsNullOrEmpty(fullPath)) return;

        try
        {
            // 【關鍵優化 1】：預先判斷 Y 槽是否已通。
            // 如果目錄已存在，就跳過 MapDrive，避免重複執行 Win32 API 導致的競爭鎖定。
            if (!System.IO.Directory.Exists("Y:\\"))
            {
                NetworkDrive.MapDrive("Y:", @"\\192.168.1.199\美工檔案區", @"tkfood-tw\ecd_01", "at160115@@");
            }

            if (File.Exists(fullPath))
            {
                // 【關鍵優化 2】：增加瀏覽器快取機制。
                // 讓瀏覽器記住圖片，下次刷頁面就不會重新跟伺服器要圖，減輕伺服器負擔。
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetMaxAge(TimeSpan.FromHours(2));
                context.Response.Cache.SetLastModified(File.GetLastWriteTime(fullPath));

                string ext = Path.GetExtension(fullPath).ToLower();
                context.Response.ContentType = GetMimeType(ext);

                // 【關鍵優化 3】：使用 TransmitFile 代替 WriteFile。
                // TransmitFile 是針對大檔案設計的，它直接將檔案從磁碟送往網路卡，不佔用伺服器記憶體。
                context.Response.TransmitFile(fullPath);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.Write("ASHX 錯誤: " + ex.Message);
        }
    }

    private string GetMimeType(string ext)
    {
        switch (ext)
        {
            case ".jpg": case ".jpeg": return "image/jpeg";
            case ".png": return "image/png";
            case ".gif": return "image/gif";
            default: return "application/octet-stream";
        }
    }

    // --- Win32 API 掛載邏輯 (直接放在 ASHX 內部最保險) ---
    [DllImport("mpr.dll")]
    private static extern int WNetAddConnection2(NetResource netResource, string password, string username, int flags);

    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public int Scope; public int Type; public int DisplayType; public int Usage;
        public string LocalName; public string RemoteName; public string Comment; public string Provider;
    }

    private void MapDrive(string local, string remote, string user, string pass)
    {

        // 先檢查路徑是否已經通了，通了就直接回傳，不要重複執行 WNetAddConnection2
        if (Directory.Exists(local)) return;

        NetResource nr = new NetResource();
        nr.Type = 1; // RESOURCETYPE_DISK
        nr.LocalName = local;
        nr.RemoteName = remote;
        int res = WNetAddConnection2(nr, pass, user, 0);
        // 85 代表已經連線，0 代表成功，這兩個都算 OK
        if (res != 0 && res != 85) throw new Exception("掛載失敗，代碼: " + res);
    }

    public bool IsReusable { get { return false; } }
}