using Ede.Uof.Utility.Page;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UOFAssist.WKF;

/**
* 修改時間：2020/10/27
* 修改人員：陳緯榕
* 修改項目：
    * 移除JGlobalLibs參考
* 修改原因：
    * 移除JGlobalLibs參考
* 修改位置：
    * 移除「JGlobalLibs」，改用「UOFAssist」
* **/

public partial class CDS_KYTUtils_QickCheckCompareVersion_Simple : BasePage
{
    private static string Current_CPUID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            System.Management.ManagementObjectSearcher wmiSearcher
                        = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            // 使用 ManagementObjectSearcher 的 Get 方法取得所有集合
            foreach (System.Management.ManagementObject obj in wmiSearcher.Get())
            {
                // 取得CPU 序號
                Current_CPUID = obj["ProcessorId"].ToString();
                break;
            }
            AddSiteMapNode("檢查檔案版本", Request.Url.AbsoluteUri);
            CreateChoiceList();
        }
    }

    #region 非控制項事件函式
    /// <summary>
    /// 建立檔案清單下拉選單
    /// </summary>
    private void CreateChoiceList()
    {
        divChoiceList.Visible = false; // 隱藏能變換檔案清單的區塊
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add(new DataColumn("SHOWNAME", typeof(string))); // 顯示的名稱
        dtSource.Columns.Add(new DataColumn("FILE_PATH", typeof(string))); // 絕對路徑

        string DirPath = Path.Combine(Request.PhysicalApplicationPath, "CDS", "Setting");
        string DirPaths = Path.Combine(DirPath, "KYTLISTs");
        if (CheckDirectoryExists(DirPath))
        {
            if (Directory.Exists(DirPaths))
            {
                divChoiceList.Visible = true; // 顯示能變換檔案清單的區塊
                rauVersionListUpload.AllowedFileExtensions = new string[] { "kytlist", "rar", "zip" };
                #region 巡覽全部的檔案清單

                foreach (string _fpath in Directory.GetFiles(DirPaths))
                {
                    DataRow ndr = dtSource.NewRow();
                    string[] filename = Path.GetFileNameWithoutExtension(_fpath).Split('.');
                    ndr["SHOWNAME"] = filename[filename.Length - 1];
                    ndr["FILE_PATH"] = _fpath;
                    dtSource.Rows.Add(ndr);
                }

                #endregion 巡覽全部的檔案清單
            }
            else // 多客戶檔案清單不存在
            {
                rauVersionListUpload.AllowedFileExtensions = new string[] { "kytlist" };
                string fileName = "FileVersionList.kytlist";
                string FilePath = Path.Combine(DirPath, fileName);
                DataRow ndr = dtSource.NewRow();
                ndr["SHOWNAME"] = "";
                ndr["FILE_PATH"] = FilePath;
                dtSource.Rows.Add(ndr);
            }
            ddlKYTLists.DataSource = dtSource;
            ddlKYTLists.DataTextField = "SHOWNAME";
            ddlKYTLists.DataValueField = "FILE_PATH";
            ddlKYTLists.DataBind();
            ddlKYTLists_SelectedIndexChanged(ddlKYTLists, null);
        }
    }

    /// <summary>
    /// 檢查WinRAR是否有被安裝
    /// </summary>
    /// <returns></returns>
    private string ExistsWinRar()
    {
        string result = string.Empty;

        string key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key))
        {
            if (registryKey != null)
            {
                result = registryKey.GetValue("").ToString();
            }
        }

        return result;
    }

    /// <summary>
    /// 將格式為rar的壓縮檔案解壓到指定的目錄
    /// </summary>
    /// <param name="rarFileName">要解壓rar檔案的路徑</param>
    /// <param name="saveDir">解壓後要儲存到的目錄</param>
    private void DeCompressRar(string rarFileName, string saveDir)
    {
        if (!string.IsNullOrEmpty(ExistsWinRar()))
        {
            string winrarDir = System.IO.Path.GetDirectoryName(ExistsWinRar());
            if (CheckDirectoryExists(saveDir) && // 準備複製的目錄存在
                File.Exists(rarFileName)) // 且要解壓縮的檔案也存在
            {
                string commandOptions = string.Format("x {0} {1} -y", rarFileName, saveDir);
                //String commandOptions = string.Format("l {0}", rarFileName);

                using (Process process = new Process())
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = System.IO.Path.Combine(winrarDir, "rar.exe");
                    processStartInfo.Arguments = commandOptions;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    processStartInfo.UseShellExecute = false;
                    processStartInfo.RedirectStandardOutput = true;
                    process.StartInfo = processStartInfo;
                    process.Start();
                    KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.DetailInfo, string.Format(@"QickCheckCompareVersion_Simple.DeCompressRar.process.Info:{0}", process.StandardOutput.ReadToEnd()));
                    //while (!process.StandardOutput.EndOfStream)
                    //{
                    //    string line = process.StandardOutput.ReadLine();
                    //}
                    process.WaitForExit();

                }
            }
        }
    }

    /// <summary>
    /// 取得檔案清單的資料表
    /// </summary>
    /// <param name="fv_path"></param>
    /// <returns></returns>
    private DataTable getFileList(string fv_path)
    {
        DataTable dtReturn = new DataTable();
        dtReturn.Columns.Add(new DataColumn("FILE_ID", typeof(string))); // 檔案所屬的KEY
        dtReturn.Columns.Add(new DataColumn("FILE_NAME", typeof(string))); // 檔案名稱
        dtReturn.Columns.Add(new DataColumn("FILE_PATH", typeof(string))); // 檔案相對路徑
        dtReturn.Columns.Add(new DataColumn("FILE_VERSION", typeof(string))); // 檔案版本
        dtReturn.Columns.Add(new DataColumn("FILE_SHA", typeof(string))); // 檔案雜湊值
        dtReturn.Columns.Add(new DataColumn("OLDER_FILE_SHA", typeof(string))); // 舊的檔案雜湊值
        dtReturn.Columns.Add(new DataColumn("PROCESSOR_KEY", typeof(string))); // CPU ID
        dtReturn.Columns.Add(new DataColumn("FILE_COMPARE_SHA", typeof(string))); // 檔案雜湊值
        dtReturn.Columns.Add(new DataColumn("IS_NEWFILE", typeof(bool))); // 是否是新檔案
        dtReturn.Columns.Add(new DataColumn("MODIFY_TIME", typeof(string))); // 最後驗證時間
        if (File.Exists(fv_path))
        {
            /***
            * FileVersionList.kytlist
            * [
            *  {
            *      "FILE_ID": "CDS_TAIFLEX_WKFFIELDS_UC_TAIFLEX_PRK_ASCX",
            *      "FILE_NAME": "UC_TAIFLEX_PRK.ascx",
            *      "FILE_PATH": "CDS\TAIFLEX\WKFFIELDS\UC_TAIFLEX_PRK.ascx",
            *      "FILE_VERSION": "1.0.0.0",
            *      "FILE_SHA": "SHA-2",
            *      "MODIFY_TIME": "2020/06/18 16:59",
            *      "PROCESSOR_KEY": ""
            *  },
            *  {
            *      "FILE_ID": "CDS_TAIFLEX_WKFFIELDS_UC_TAIFLEX_PRK_ASCX_CS",
            *      "FILE_NAME": "UC_TAIFLEX_PRK.ascx.cs",
            *      "FILE_PATH": "CDS\TAIFLEX\WKFFIELDS\UC_TAIFLEX_PRK.ascx.cs",
            *      "FILE_VERSION": "1.0.0.0",
            *      "FILE_SHA": "SHA-2",
            *      "MODIFY_TIME": "2020/06/18 16:59",
            *      "PROCESSOR_KEY": ""
            *  }
            * ]
            * 
            * ***/
            using (StreamReader sReader = new StreamReader(fv_path))
            {
                JArray jaKYTList = null;
                try
                {
                    jaKYTList = JArray.Parse(sReader.ReadToEnd());
                    foreach (JObject _joKYTFile in jaKYTList.Children<JObject>())
                    {
                        if (_joKYTFile.Property("FILE_ID") != null) // 這一組裡面有FILE_ID才做
                        {
                            DataRow[] _drs = dtReturn.Select(string.Format("FILE_ID = '{0}'", _joKYTFile["FILE_ID"])); // FILE_ID是PK，有找到就是已經存在
                            if (_drs.Length == 0)
                            {
                                DataRow ndr = dtReturn.NewRow();
                                foreach (JProperty _jpKYTFile in _joKYTFile.Properties())
                                {
                                    if (!dtReturn.Columns.Contains(_jpKYTFile.Name))
                                        dtReturn.Columns.Add(new DataColumn(_jpKYTFile.Name, typeof(string)));
                                    ndr[_jpKYTFile.Name] = _jpKYTFile.Value;
                                }
                                dtReturn.Rows.Add(ndr);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"QickCheckCompareVersion_Simple.getFileList.ParseJSON.ERROR:{0}", ex.Message));
                }
            }

        }
        return dtReturn;
    }

    /// <summary>
    /// 使用檔案流取得SHA256雜湊值的Base64字串
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private string getSHA256String(FileStream stream)
    {
        byte[] buffer;
        int bytesRead;
        //size = stream.Length;
        using (HashAlgorithm hasher = SHA256.Create())
        {
            do
            {

                buffer = new byte[4096]; // 一次讀取的量
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                hasher.TransformBlock(buffer, 0, bytesRead, null, 0);
                //totalBytesRead += bytesRead;
                //backgroundWorker1.ReportProgress((int)((double)totalBytesRead / size * 100));
                //showStatusMessage(string.Format(@"目前進度： {0} %", (int)((double)totalBytesRead / size * 100)));
                //if (isStop) break;
            }
            while (bytesRead != 0);
            hasher.TransformFinalBlock(buffer, 0, 0);
            return MakeHashString(hasher.Hash);
        }
        //SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
        //byte[] crypto = sha256.ComputeHash(stream);
        //return Convert.ToBase64String(crypto);
    }

    /// <summary>
    /// 將雜湊值的二進位轉換成字串
    /// </summary>
    /// <param name="hashBytes"></param>
    /// <returns></returns>
    private string MakeHashString(byte[] hashBytes)
    {
        StringBuilder hash = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            hash.Append(b.ToString("X2").ToLower()); // byte[] 轉 string
        }
        return hash.ToString();
    }

    /// <summary>
    /// 將串流寫入檔案中
    /// </summary>
    /// <param name="path"></param>
    private void SystemStreamWriter(string path, string content)
    {
        using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
        {
            sw.WriteLine(content);
        }
    }

    /// <summary>
    /// 檢查(建立)資料夾是否存在
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    private bool CheckDirectoryExists(string dirPath)
    {
        bool isExists = true;
        if (!Directory.Exists(dirPath))
        {
            try
            {
                Directory.CreateDirectory(dirPath);
            }
            catch (Exception)
            {
                isExists = false;
                throw;
            }
        }
        return isExists;
    }
    #endregion 非控制項事件函式

    #region 控制項事件函式

    /// <summary>
    /// 上傳檔案後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rauVersionListUpload_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
    {
        if (rauVersionListUpload.UploadedFiles.Count > 0) // 判斷是否有上傳檔案
        {
            DataTable dtSource = gvFILE_VERSION.DataTable;
            //List<string> lsTempPath = new List<string>();


            foreach (Telerik.Web.UI.UploadedFile postedFile in rauVersionListUpload.UploadedFiles) // 巡覽上傳的所有檔案
            {
                if (postedFile != null)
                {
                    string extensionName = System.IO.Path.GetExtension(postedFile.FileName);
                    if (extensionName.Equals(".kytlist"))
                    {
                        using (FileStream filestream = postedFile.InputStream as FileStream)
                        using (StreamReader sReader = new StreamReader(filestream, Encoding.UTF8))
                        {
                            /***
                            * FileVersionList.kytlist
                            * [
                            *  {
                            *      "FILE_ID": "CDS_TAIFLEX_WKFFIELDS_UC_TAIFLEX_PRK_ASCX",
                            *      "FILE_NAME": "UC_TAIFLEX_PRK.ascx",
                            *      "FILE_PATH": "CDS\TAIFLEX\WKFFIELDS\UC_TAIFLEX_PRK.ascx",
                            *      "FILE_VERSION": "1.0.0.0",
                            *      "FILE_SHA": "SHA-2",
                            *      "MODIFY_TIME": "2020/06/18 16:59",
                            *      "PROCESSOR_KEY": ""
                            *  },
                            *  {
                            *      "FILE_ID": "CDS_TAIFLEX_WKFFIELDS_UC_TAIFLEX_PRK_ASCX_CS",
                            *      "FILE_NAME": "UC_TAIFLEX_PRK.ascx.cs",
                            *      "FILE_PATH": "CDS\TAIFLEX\WKFFIELDS\UC_TAIFLEX_PRK.ascx.cs",
                            *      "FILE_VERSION": "1.0.0.0",
                            *      "FILE_SHA": "SHA-2",
                            *      "MODIFY_TIME": "2020/06/18 16:59",
                            *      "PROCESSOR_KEY": ""
                            *  }
                            * ]
                            * 
                            * ***/
                            /***
                             * FileVersionList.kytlist
                             * [
                             *     {
                             *         "FILE_ID": "CDS_TAIFLEX_WKFFIELDS_UC_TAIFLEX_PRK_ASCX",
                             *         "FILE_NAME": "UC_TAIFLEX_PRK.ascx",
                             *         "FILE_PATH": "CDS\TAIFLEX\WKFFIELDS\UC_TAIFLEX_PRK.ascx"
                             *     },
                             *     {
                             *         "FILE_ID": "CDS_TAIFLEX_WKFFIELDS_UC_TAIFLEX_PRK_ASCX_CS",
                             *         "FILE_NAME": "UC_TAIFLEX_PRK.ascx.cs",
                             *         "FILE_PATH": "CDS\TAIFLEX\WKFFIELDS\UC_TAIFLEX_PRK.ascx.cs"
                             *     }
                             * ]
                             * ***/
                            JArray jaKYTList = null;
                            try
                            {
                                jaKYTList = JArray.Parse(sReader.ReadToEnd());
                                foreach (JObject _joKYTFile in jaKYTList.Children<JObject>())
                                {
                                    if (_joKYTFile.Property("FILE_ID") != null) // 這一組裡面有FILE_ID才做
                                    {
                                        DataRow[] _drs = dtSource.Select(string.Format("FILE_ID = '{0}'", _joKYTFile["FILE_ID"])); // FILE_ID是PK，有找到就是已經存在
                                        if (_drs.Length == 0) // 不存在於現在的清單中
                                        {
                                            DataRow ndr = dtSource.NewRow();
                                            foreach (JProperty _jpKYTFile in _joKYTFile.Properties())
                                            {
                                                if (!dtSource.Columns.Contains(_jpKYTFile.Name))
                                                    dtSource.Columns.Add(new DataColumn(_jpKYTFile.Name, typeof(string)));
                                                switch (_jpKYTFile.Name)
                                                {
                                                    case "FILE_ID":
                                                    case "FILE_NAME":
                                                    case "FILE_PATH":
                                                        ndr[_jpKYTFile.Name] = _jpKYTFile.Value; // 新增新的檔案清單
                                                        break;
                                                    case "PROCESSOR_KEY":
                                                        ndr[_jpKYTFile.Name] = Current_CPUID; // 記錄自己的CPU ID
                                                        break;
                                                    case "FILE_SHA":
                                                        ndr["FILE_COMPARE_SHA"] = _jpKYTFile.Value; // 將來源資料的雜湊值記錄到比對的欄位內
                                                        break;
                                                }
                                            }
                                            ndr["IS_NEWFILE"] = true; // 是新檔案
                                            dtSource.Rows.Add(ndr);
                                        }
                                        else // 現在的清單已經有了
                                        {
                                            _drs[0]["FILE_COMPARE_SHA"] = _joKYTFile["FILE_SHA"]; // 將來源資料的雜湊值記錄到比對的欄位內
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"QickCheckCompareVersion_Simple.rauVersionListUpload_FileUploaded.ParseJSON.ERROR:{0}", ex.Message));
                            }

                        }
                    }
                    else if (extensionName.Equals(".zip") || extensionName.Equals(".rar")) // 是RAR或ZIP壓縮檔
                    {
                        #region 將上傳檔案複製，並且解壓縮

                        string DirPath = Path.Combine(Request.PhysicalApplicationPath, "CDS", "TempExtract");
                        if (CheckDirectoryExists(DirPath))
                        {
                            string filePath = Path.Combine(DirPath, postedFile.FileName);
                            postedFile.SaveAs(filePath);
                            string ExtractPath = Path.Combine(DirPath, Path.GetFileNameWithoutExtension(postedFile.FileName)); // 解壓縮到壓縮檔名稱的資料夾
                            DeCompressRar(filePath, ExtractPath);

                            #endregion 將上傳檔案複製，並且解壓縮

                            #region 將收納檔案輸出成kytlist格式

                            JArray jaMainSource = new JArray();

                            #region 巡覽資料夾及檔案並收納

                            string[] files = Directory.GetFiles(ExtractPath, "*.*", SearchOption.AllDirectories);
                            foreach (string _file in files)
                            {
                                JObject joOneFile = new JObject();

                                string relativePath = _file.Replace(ExtractPath, "");
                                string FILE_PATH = relativePath.Substring(1, relativePath.Length - 1);
                                string FILE_ID = FILE_PATH.Replace("\\", "_");
                                string FILE_NAME = Path.GetFileName(_file);
                                string FILE_SHA = "";
                                using (FileStream stream = File.OpenRead(_file))
                                {
                                    FILE_SHA = getSHA256String(stream);
                                }
                                joOneFile.Add(new JProperty("FILE_ID", FILE_ID)); // 檔案所屬的KEY
                                joOneFile.Add(new JProperty("FILE_NAME", FILE_NAME)); // 檔案名稱
                                joOneFile.Add(new JProperty("FILE_PATH", FILE_PATH)); // 檔案相對路徑
                                joOneFile.Add(new JProperty("FILE_VERSION", "1.0.0.0")); // 檔案版本
                                joOneFile.Add(new JProperty("FILE_SHA", FILE_SHA)); // 檔案雜湊值
                                joOneFile.Add(new JProperty("PROCESSOR_KEY", Current_CPUID)); // CPU ID
                                joOneFile.Add(new JProperty("MODIFY_TIME", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))); // 最後驗證時間
                                jaMainSource.Add(joOneFile);
                            }

                            #endregion 巡覽資料夾及檔案並收納

                            #endregion 將收納檔案輸出成kytlist格式

                            #region 儲存到暫存位置

                            string kytlistPath = Path.Combine(DirPath, string.Format("FileVersionList.{0}.kytlist", Path.GetFileNameWithoutExtension(postedFile.FileName)));
                            //using (StreamWriter sw = new StreamWriter(kytlistPath, false, Encoding.UTF8))
                            //{
                            //    sw.WriteLine(jaMainSource.ToString());
                            //}
                            SystemStreamWriter(kytlistPath, jaMainSource.ToString());

                            #endregion 儲存到暫存位置


                            #region 下載kytlist檔案清單

                            if (File.Exists(kytlistPath))
                            {
                                ScriptManager.RegisterClientScriptBlock(
                               UpdatePanel1,
                               UpdatePanel1.GetType(),
                               Guid.NewGuid().ToString(),
                               string.Format(@"
                                window.location = '{0}?filepath={1}';
                            "
                               , Page.ResolveUrl("~/CDS/KYTUtils/WebService/FORMPRINT/DownFileWithPath.ashx")
                               , HttpUtility.UrlEncode(kytlistPath)),
                               true);
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), @"
                                document.addEventListener('DOMContentLoaded', function() {
                                    alert('找不到清單檔, 請聯繫系統管理員');
                                });
                            ", true);

                            #endregion 下載kytlist檔案清單

                            #region 刪除壓縮檔以及解壓縮的資料夾

                            try
                            {
                                File.Delete(filePath);
                                Directory.Delete(ExtractPath, true);
                            }
                            catch (Exception ex)
                            {
                                KYTUtilLibs.KYTDebugLog.Log(KYTLog.DebugLog.LogLevel.Error, string.Format(@"QickCheckCompareVersion_Simple.rauVersionListUpload_FileUploaded.Extract.Delete.ERROR:{0}", ex.Message));
                            }


                            #endregion 刪除壓縮檔以及解壓縮的資料夾
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), string.Format(@"
                                document.addEventListener('DOMContentLoaded', function() {
                                    alert('無法建立{0}');
                                });
                            ", DirPath), true);
                        } 

                    }

                }
            }
            // 刪除暫存檔
            //foreach (string _tmpPath in lsTempPath)
            //{
            //    File.Delete(_tmpPath);
            //}

            gvFILE_VERSION.DataSource = dtSource;
            gvFILE_VERSION.DataBind();
        }
    }

    /// <summary>
    /// 更新自己的版本按鈕按下後觸發事件
    /// 會將新的檔案清單寫入KYTLIST中，並且「只會」更新「新的」檔案清單的雜湊值
    /// 20200619 註解，理由是，當時規劃是因為認為一次檢查所有的雜湊值(包含dll)會很久，所以才需要，但現在用不到
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void lbtnRefresh_Click(object sender, EventArgs e)
    //{
    //    #region 巡覽GridView的DataTable的IS_NEWFILE = true的項目

    //    DataTable dtSource = gvFILE_VERSION.DataTable;
    //    foreach (DataRow dr in dtSource.Rows)
    //    {
    //        if (dtSource.Columns.Contains("IS_NEWFILE") &&
    //            (bool)dr["IS_NEWFILE"])
    //        {
    //            if (dtSource.Columns.Contains("OLDER_FILE_SHA"))
    //            {
    //                dr["OLDER_FILE_SHA"] = dr["FILE_SHA"]; // 紀錄更新前的雜湊值
    //            }
    //            #region 取出FILE_PATH並組合後取SHA-2
    //            string filepath = Path.Combine(Request.PhysicalApplicationPath, dr["FILE_PATH"].ToString());
    //            using (FileStream stream = File.OpenRead(filepath))
    //            {
    //                string result = getSHA256String(stream);
    //                #region 更新DataRow，並且更新OLD-SHA

    //                dr["FILE_SHA"] = result;
    //                dr["MODIFY_TIME"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

    //                #endregion 更新DataRow，並且更新OLD-SHA
    //            }
    //            #endregion 取出FILE_PATH並組合後取SHA-2
    //        }
    //    }

    //    #endregion 巡覽GridView的DataTable的IS_NEWFILE = true的項目

    //    #region 儲存到kytlist中

    //    JArray jaMainSource = new JArray();

    //    foreach (DataRow dr in dtSource.Rows)
    //    {
    //        JObject joOneFile = new JObject();
    //        foreach (DataColumn dc in dtSource.Columns)
    //        {
    //            switch (dc.ColumnName)
    //            {
    //                case "FILE_ID": // 檔案所屬的KEY
    //                case "FILE_NAME": // 檔案名稱
    //                case "FILE_PATH": // 檔案相對路徑
    //                case "FILE_VERSION": // 檔案版本
    //                case "FILE_SHA": // 檔案雜湊值
    //                case "MODIFY_TIME": // 取得雜湊值的時間
    //                    joOneFile.Add(new JProperty(dc.ColumnName, dr[dc.ColumnName].ToString()));
    //                    break;
    //                case "PROCESSOR_KEY": // CPU ID
    //                    joOneFile.Add(new JProperty(dc.ColumnName, Current_CPUID));
    //                    break;

    //            }
    //        }
    //        jaMainSource.Add(joOneFile);
    //    }
    //    //if (!File.Exists(ddlKYTLists.SelectedValue)) // 清單檔不存在
    //    //{
    //    //    File.Create(ddlKYTLists.SelectedValue); 
    //    //}
    //    //File.WriteAllText(ddlKYTLists.SelectedValue, jaMainSource.ToString(), Encoding.UTF8);

    //    // 新增清單檔
    //    //using (StreamWriter sw = new StreamWriter(ddlKYTLists.SelectedValue, false, Encoding.UTF8))
    //    //{
    //    //    sw.WriteLine(jaMainSource.ToString());
    //    //}
    //    SystemStreamWriter(ddlKYTLists.SelectedValue, jaMainSource.ToString());
    //    #endregion 儲存到kytlist中

    //    #region 更新資訊

    //    gvFILE_VERSION.DataSource = dtSource;
    //    gvFILE_VERSION.DataBind();

    //    #endregion 更新資訊
    //}

    /// <summary>
    /// 輸出按鈕按下後觸發事件
    /// 輸出不包含比對雜湊值的kytlist檔案
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnExport_Click(object sender, EventArgs e)
    {
        #region 儲存到kytlist中
        DataTable dtSource = gvFILE_VERSION.DataTable;
        JArray jaMainSource = new JArray();

        foreach (DataRow dr in dtSource.Rows)
        {
            JObject joOneFile = new JObject();
            foreach (DataColumn dc in dtSource.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "FILE_ID": // 檔案所屬的KEY
                    case "FILE_NAME": // 檔案名稱
                    case "FILE_PATH": // 檔案相對路徑
                    case "FILE_VERSION": // 檔案版本
                    case "FILE_SHA": // 檔案雜湊值
                    case "MODIFY_TIME": // 取得雜湊值的時間
                        joOneFile.Add(new JProperty(dc.ColumnName, dr[dc.ColumnName].ToString()));
                        break;
                    case "PROCESSOR_KEY": // CPU ID
                        joOneFile.Add(new JProperty(dc.ColumnName, Current_CPUID));
                        break;

                }
            }
            jaMainSource.Add(joOneFile);
        }
        //if (!File.Exists(ddlKYTLists.SelectedValue)) // 清單檔不存在
        //{
        //    File.Create(ddlKYTLists.SelectedValue); // 新增清單檔
        //}
        //File.WriteAllText(ddlKYTLists.SelectedValue, jaMainSource.ToString(), Encoding.UTF8);

        // 新增清單檔
        //using (StreamWriter sw = new StreamWriter(ddlKYTLists.SelectedValue, false, Encoding.UTF8))
        //{
        //    sw.WriteLine(jaMainSource.ToString());
        //}
        SystemStreamWriter(ddlKYTLists.SelectedValue, jaMainSource.ToString());

        #endregion 儲存到kytlist中
        #region 下載kytlist

        string filePath = ddlKYTLists.SelectedValue;
        if (File.Exists(filePath))
        {
            ScriptManager.RegisterClientScriptBlock(
           UpdatePanel1,
           UpdatePanel1.GetType(),
           Guid.NewGuid().ToString(),
           string.Format(@"
                window.location = '{0}?filepath={1}';
            "
           , Page.ResolveUrl("~/CDS/KYTUtils/WebService/FORMPRINT/DownFileWithPath.ashx")
           , HttpUtility.UrlEncode(filePath)),
           true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), @"
                document.addEventListener('DOMContentLoaded', function() {
                    alert('找不到清單檔, 請聯繫系統管理員');
                });
            ", true);

        #endregion 下載kytlist
    }

    /// <summary>
    /// 重新驗證版本按鈕按下後觸發事件
    /// 會將新的檔案清單寫入KYTLIST中，並且會更新檔案清單內「所有」的雜湊值
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnReVerify_Click(object sender, EventArgs e)
    {
        #region 巡覽GridView的DataTable

        DataTable dtSource = gvFILE_VERSION.DataTable;
        foreach (DataRow dr in dtSource.Rows)
        {
            if (dtSource.Columns.Contains("OLDER_FILE_SHA"))
            {
                dr["OLDER_FILE_SHA"] = dr["FILE_SHA"]; // 紀錄更新前的雜湊值
            }
            #region 取出FILE_PATH並組合後取SHA-2
            string filepath = Path.Combine(Request.PhysicalApplicationPath, dr["FILE_PATH"].ToString());
            using (FileStream stream = File.OpenRead(filepath))
            {
                string result = getSHA256String(stream);

                #region 更新DataRow，並且更新OLD-SHA

                dr["FILE_SHA"] = result;
                dr["MODIFY_TIME"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                #endregion 更新DataRow，並且更新OLD-SHA
            }
            #endregion 取出FILE_PATH並組合後取SHA-2
        }

        #endregion 巡覽GridView的DataTable

        #region 儲存到kytlist中

        JArray jaMainSource = new JArray();

        foreach (DataRow dr in dtSource.Rows)
        {
            JObject joOneFile = new JObject();
            foreach (DataColumn dc in dtSource.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "FILE_ID": // 檔案所屬的KEY
                    case "FILE_NAME": // 檔案名稱
                    case "FILE_PATH": // 檔案相對路徑
                    case "FILE_VERSION": // 檔案版本
                    case "FILE_SHA": // 檔案雜湊值
                    case "MODIFY_TIME": // 取得雜湊值的時間
                        joOneFile.Add(new JProperty(dc.ColumnName, dr[dc.ColumnName].ToString()));
                        break;
                    case "PROCESSOR_KEY": // CPU ID
                        joOneFile.Add(new JProperty(dc.ColumnName, Current_CPUID));
                        break;

                }
            }
            jaMainSource.Add(joOneFile);
        }
        //if (!File.Exists(ddlKYTLists.SelectedValue)) // 清單檔不存在
        //{
        //    File.Create(ddlKYTLists.SelectedValue); // 新增清單檔
        //}
        //File.WriteAllText(ddlKYTLists.SelectedValue, jaMainSource.ToString(), Encoding.UTF8);

        // 新增清單檔
        //using (StreamWriter sw = new StreamWriter(ddlKYTLists.SelectedValue, false, Encoding.UTF8))
        //{
        //    sw.WriteLine(jaMainSource.ToString());
        //}
        SystemStreamWriter(ddlKYTLists.SelectedValue, jaMainSource.ToString());

        #endregion 儲存到kytlist中

        #region 更新資訊

        gvFILE_VERSION.DataSource = dtSource;
        gvFILE_VERSION.DataBind();

        #endregion 更新資訊
    }

    /// <summary>
    /// 檔案清單版本明細每筆資料綁定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvFILE_VERSION_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridViewRow gr = e.Row;
        gr.Attributes.Add("onmouseover", "this.bkc=this.style.backgroundColor;this.style.backgroundColor='#ccffff'"); // 設定每筆資料在滑鼠移動進去時的背景顏色
        gr.Attributes.Add("onmouseout", "this.style.backgroundColor=this.bkc"); // 設定每筆資料在滑鼠移出時的背景色
        if (gr.RowType == DataControlRowType.DataRow)
        {
            DataTable tblItems = gvFILE_VERSION.DataTable;
            DataRow row = tblItems.Rows[gr.DataItemIndex];

            KYTTextBox ktxtFileName = gr.FindControl("ktxtFileName") as KYTTextBox; // 檔案名稱
            KYTTextBox ktxtMODIFY_TIME = gr.FindControl("ktxtMODIFY_TIME") as KYTTextBox; // 最後驗證時間
            KYTTextBox ktxtFILE_VERSION = gr.FindControl("ktxtFILE_VERSION") as KYTTextBox; // 版本雜湊值
            KYTTextBox ktxtFILE_SHA = gr.FindControl("ktxtFILE_SHA") as KYTTextBox; // 版本雜湊值
            KYTTextBox ktxtFILE_COMPARE_SHA = gr.FindControl("ktxtFILE_COMPARE_SHA") as KYTTextBox; // 比對雜湊值
            gr.BackColor = Color.White; // 設定底色
            if (!string.IsNullOrWhiteSpace(row["IS_NEWFILE"].ToString()) &&
                (bool)row["IS_NEWFILE"])
            {
                gr.BackColor = Color.PaleGreen;
            }
            else if (!string.IsNullOrWhiteSpace(row["OLDER_FILE_SHA"].ToString()) && // 有更新過檔案
                row["FILE_SHA"].ToString() != row["OLDER_FILE_SHA"].ToString()) // 更新前後檔案不同
            {
                gr.BackColor = Color.BlueViolet;
            }
            else if (!string.IsNullOrEmpty(row["FILE_COMPARE_SHA"].ToString()) &&
                row["FILE_SHA"].ToString() != row["FILE_COMPARE_SHA"].ToString()) // 比對後檔案不相同
            {
                gr.BackColor = Color.Crimson;
            }

        }
    }
    /// <summary>
    /// 選擇不同公司的檔案清單後觸發事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlKYTLists_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList _ddlKYTLists = (DropDownList)sender;
        gvFILE_VERSION.DataSource = getFileList(_ddlKYTLists.SelectedValue);
        gvFILE_VERSION.DataBind();
    }

    #endregion 控制項事件函式
}
