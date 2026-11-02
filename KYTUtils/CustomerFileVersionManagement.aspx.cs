using Ede.Uof.Utility.Page;
using Ionic.Zip; // DotNetZip的ClassName
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

public partial class CDS_KYTUtils_CustomerFileVersionManagement : BasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(Button1);
        if (!Page.IsPostBack)
        {
            AddSiteMapNode("", Request.Url.AbsoluteUri);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (FileUpload1.PostedFiles.Count > 0)
        {
            foreach (HttpPostedFile file in FileUpload1.PostedFiles)
            {
                switch (file.ContentType)
                {
                    case "application/zip":
                        {
                            // 有點浪費資源的寫法，但目前想不到更適合的
                            /**
                            1. 先巡覽zip，然後找出所有檔案，並解析LastDatetime和checksum
                            2. 如果和DB紀錄的不同，寫入DB
                            3. 寫入DB後，將FileName記錄到集合中
                            4. 在巡覽zip一次，不存在的Floder建立，並且由上傳時間當作根目錄的名稱(這樣才不會有舊的資料被覆蓋)(timestamp)
                             */
                             /***
                              * checksum = MD5(SHA256 + ! + lastmodifydate + $% + filenamewithextend + *&)
                              * **/
                            using (ZipFile zips = ZipFile.Read(file.InputStream))
                            {
                                foreach (ZipEntry entity in zips)
                                {
                                    using (MemoryStream outputStream = new MemoryStream())
                                    {
                                        entity.Extract(outputStream);
                                        string _path = Path.Combine(@"D:\Temp\", entity.FileName);
                                        bool isDirectory = false;
                                        bool needUpdate = false;
                                        if (entity.UncompressedSize == 0) // 如果沒壓縮前的大小是0，表示是資料夾
                                        {
                                            isDirectory = true;
                                            //if (!Directory.Exists(_path))
                                            //{
                                            //    Directory.CreateDirectory(_path);
                                            //}
                                        }
                                        else
                                        {
                                            isDirectory = false;
                                            string SHA256 = System.BitConverter.ToString(System.Security.Cryptography.SHA256.Create().ComputeHash(outputStream.GetBuffer())).Replace("-", string.Empty).ToUpper();
                                            string str = string.Format(@"{{0}!{1}$%{2}*&}", SHA256, entity.LastModified.ToString("yyyy-MM-dd HH:mm:ss"), Path.GetFileName(_path));
                                            string signature = System.BitConverter.ToString(System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(str))).Replace("-", string.Empty).ToUpper();
                                            // TODO 查詢DB，PK = signature
                                            //// 使用 DotNetZip 會導致 MemoryStream 的 Position 不在最前面
                                            //// 這會導致後續抓不到檔案，所以讀取後必須把 Position 歸零！
                                            //outputStream.Position = 0;
                                            //using (BinaryReader reader = new BinaryReader(outputStream))
                                            //{
                                            //    File.WriteAllBytes(
                                            //        _path,
                                            //        reader.ReadBytes((int)outputStream.Length));
                                            //}
                                        }

                                        #region 先處理是否要更新
                                        if (!isDirectory)
                                        {

                                        }
                                        #endregion 先處理是否要更新

                                        //System.IO.Directory.Exists("CDS/SCSHR/WKFFields/OVERTIME")
                                        // TODO 上傳路徑由dll.config控制
                                    }
                                }
                            }
                                
                        }
                        break;
                }
            }
        }
    }

}
