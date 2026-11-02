using Ede.Uof.Utility.Page;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.UI;


// TODO:: 還沒解決收納的階層關係；目前只有上一層和現在的這一層，但如果是「CDS/Temp/SSS」和「QQQ/Temp/SSS」會被視為一樣，要怎麼避免呢?
public partial class CDS_KYTUtils_ShowRARFileList : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("", Request.Url.AbsoluteUri);
            DeCompressRar(@"D:\產品發佈\KYTI冠永騰_TAIFLEX台虹_202004141703.rar", @"D:\產品發佈\testdata");
        }
    }

    /// <summary>
    /// 檢查WinRAR是否有被安裝
    /// </summary>
    /// <returns></returns>
    public static string ExistsWinRar()
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
    public static void DeCompressRar(string rarFileName, string saveDir)
    {
        if (!string.IsNullOrEmpty(ExistsWinRar()))
        {
            string winrarDir = System.IO.Path.GetDirectoryName(ExistsWinRar());
            bool isDirExists = false;
            if (!Directory.Exists(saveDir))
            {
                try
                {
                    Directory.CreateDirectory(saveDir);
                    isDirExists = true;
                }
                catch
                {

                }
            }
            else
            {
                isDirExists = true;
            }
            if (isDirExists && // 準備複製的目錄存在
                File.Exists(rarFileName)) // 且要解壓縮的檔案也存在
            {
                //String commandOptions = string.Format("x {0} {1} -y", rarFileName, saveDir);
                String commandOptions = string.Format("l {0}", rarFileName);
                /**
                 * RAR 5.61 x64   版權所有(C) 1993-2018 Alexander Roshal   30 Sep 2018
                    註冊給 Administrator

                    壓縮檔: D:\產品發佈\KYTI冠永騰_TAIFLEX台虹_202004141703.rar
                    詳細內容: RAR 5

                     屬性            大小     日期    時間   名稱
                    ----------- ---------  ---------- -----  ----
                        ..A....     22003  2020-04-14 17:01  CDS\TAIFLEX\WKFFields\UC_TAIFLEX_PO.ascx
                        ..A....     17493  2020-04-14 17:02  CDS\TAIFLEX\WKFFields\UC_TAIFLEX_PO.ascx.cs
                        ..A....     12350  2020-04-14 17:02  CDS\TAIFLEX\WKFFields\UC_TAIFLEX_PRM.ascx
                        ..A....     13498  2020-04-14 17:02  CDS\TAIFLEX\WKFFields\UC_TAIFLEX_PRM.ascx.cs
                        ..A....     14930  2020-04-14 17:02  CDS\TAIFLEX\WKFFields\UC_TAIFLEX_QA.ascx
                        ..A....     15192  2020-04-14 17:02  CDS\TAIFLEX\WKFFields\UC_TAIFLEX_QA.ascx.cs
                        ..AD...         0  2020-04-14 17:03  CDS\TAIFLEX\WKFFields
                        ..AD...         0  2020-04-14 17:03  CDS\TAIFLEX
                        ...D...         0  2020-04-14 17:03  CDS
                        ..A....      1625  2020-04-14 17:01  plugin.txt
                    ----------- ---------  ---------- -----  ----
                                    97091                    10
                 * 
                 * **/
                List<DicArchiveList> lsDicA = new List<DicArchiveList>();
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
                    bool isContinue = false;
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();

                        if (isContinue)
                        {
                            string[] strs = line.Split(" ");
                            int index = strs.Length;
                            string[] FolderOrFiles = strs[strs.Length - 1].Split('\\');
                            for (int i = 0; i < FolderOrFiles.Length; i++)
                            {
                                string _FOF = FolderOrFiles[i];
                                bool needAdd = true;
                                DicArchiveList dal = new DicArchiveList();
                                dal.PathLevel = i;
                                dal.PathParentDiretory = i > 0 ? FolderOrFiles[i - 1] : "";
                                dal.PathSelfName = _FOF;
                                foreach (DicArchiveList _dal in lsDicA)
                                {
                                    if (_dal.PathLevel == i)
                                    {
                                        if (_dal.PathParentDiretory.Equals(dal.PathParentDiretory))

                                    }
                                }
                                if (needAdd)
                                {
                                    lsDicA.Add(dal);
                                }
                            }
                        }
                        if (line.Split("---").Length > 1) // WinRAR用來區隔用的符號
                        {
                            isContinue = !isContinue;
                        }
                    }
                    process.WaitForExit();

                }
            }


        }

    }
}

public class DicArchiveList
{
    public int PathLevel = 0;

    public string PathParentDiretory = "";

    public string PathSelfName = "";
}
