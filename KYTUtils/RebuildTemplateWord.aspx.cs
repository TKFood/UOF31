using Ede.Uof.Utility.Page;
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
public partial class CDS_KYTUtils_RebuildTemplateWord : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("使用冠永騰模板套印的前置作業工具", Request.Url.AbsoluteUri);
        }
    }

    #region 非控制事項事件

    #endregion 非控制事項事件

    #region 控制事項事件
    /// <summary>
    /// 選擇檔案後觸發的上傳檔案事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rauDOCXUpload_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
    {
        if (rauDOCXUpload.UploadedFiles.Count > 0) // 判斷是否有上傳檔案
        {
            foreach (Telerik.Web.UI.UploadedFile postedFile in rauDOCXUpload.UploadedFiles) // 巡覽上傳的所有檔案
            {
                if (postedFile != null)
                {
                    string extensionName = System.IO.Path.GetExtension(postedFile.FileName);
                    if (extensionName.Equals(".doc") || extensionName.Equals(".docx")) // 沒有
                    {
                        HttpContext current = HttpContext.Current;
                        string tempFile = current.Server.MapPath(@"~\App_Data\temp.docx"); // 輸出資料夾路徑
                        if (!Directory.Exists(Path.GetDirectoryName(tempFile))) // 若暫存資料夾不存在則建立
                        {
                            try { Directory.CreateDirectory(Path.GetDirectoryName(tempFile)); }
                            catch (Exception ex) { KYTUtilLibs.KYTDebugLog.Log(DebugLog.LogLevel.Error, string.Format("UC_TAIYUEN_FEE_N::列印::建立資料夾失敗::原因:{0}", ex.Message)); };
                        }
                        if (File.Exists(tempFile)) File.Delete(tempFile);
                        using (FileStream filestream = postedFile.InputStream as FileStream)
                        {
                            using (var outputFileStream = File.Create(tempFile))
                            {
                                filestream.Seek(0, SeekOrigin.Begin);
                                filestream.CopyTo(outputFileStream);
                            }
                        }
                        if (chkForce.Checked)
                        {
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[$<", ">$]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[<", ">$]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[$<", ">]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[$<", "$]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[$", ">$]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[<", ">]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[", ">]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[<", "]"));
                            File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[", "]"));
                        }
                        else
                        {
                            File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[$", "$]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[$<", ">$]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[<", ">$]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[$<", ">]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[$<", "$]"));
                            //File.WriteAllBytes(tempFile, KYTIDocument.KYTIDocx.MakeTemplate(tempFile, "[$", ">$]"));
                        }

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
                             , HttpUtility.UrlEncode(tempFile)),
                             true);

                    }

                }
            }
        }
    }

    #endregion 控制事項事件


}
