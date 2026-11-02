using Ede.Uof.Utility.Data;
using Ede.Uof.Utility.Page.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Web.Security.AntiXss;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Web.Services;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Net.Http;

public partial class CDS_WebPage_STOCK_COPTGTHDialogEDIT : Ede.Uof.Utility.Page.BasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        //設定回傳值
        Dialog.SetReturnValue2("");

        //註冊Dialog的Button 狀態
        ((Master_DialogMasterPage)this.Master).Button1CausesValidation = false;
        ((Master_DialogMasterPage)this.Master).Button1AutoCloseWindow = false;
        ((Master_DialogMasterPage)this.Master).Button1OnClick += CDS_WebPage_Dialog_Button1OnClick;
        ((Master_DialogMasterPage)this.Master).Button2OnClick += Button2OnClick;

        ((Master_DialogMasterPage)this.Master).Button2Text = string.Empty;

        if (!IsPostBack)
        {           
            //接收主頁面傳遞之參數
            lblParam.Text = Request["ID"];
            myTextcontent.Value= Request["ID"];

            if (!string.IsNullOrEmpty(lblParam.Text))
            {
                ID = Request["ID"];
            }

        }

    }




    #region BUTTON
    void CDS_WebPage_Dialog_Button1OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);      

        Dialog.SetReturnValue2("REFRESH");
        Dialog.Close(this);

    }


    void Button2OnClick()
    {
        //設定回傳值並關閉視窗
        //Dialog.SetReturnValue2(txtReturnValue.Text);

        Dialog.SetReturnValue2("REFRESH");
        Dialog.Close(this);


    }




    #endregion

    #region FUNCTION
    [WebMethod()]
    public static string SaveCapturedImage(string ID, string data )
    {      
        string NOWTIMES = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //UPLOAD_TO_PIC();

        string ORI1 = "";
        string ORI2 = "";
        string ORI3 = "";
        ////Convert Base64 Encoded string to Byte Array.       
        byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);
        ORI1 = imageBytes.Length.ToString();
        //加上浮水印
        byte[] imageBytes2 = GetWatermarkPic(imageBytes, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        ORI2 = imageBytes2.Length.ToString();
        ////壓縮圖片
        //byte[] imageBytes3 = CutImage(imageBytes2, 50, 50);
        //ORI3 = imageBytes3.Length.ToString();

        //照片上傳到資料庫
        STATICADDPACKAGEBOXSPHOTO(ID, NOWTIMES, imageBytes2);

        //照片傳到附件
        UploadImage(imageBytes2, "", ID);

        ////Save the Byte Array as Image File.
        //string filePath = HttpContext.Current.Server.MapPath(string.Format("~/PIC/{0}.jpg", ID));
        //string filePath = HttpContext.Current.Server.MapPath(string.Format("~/PIC/test.jpg"));
        //File.WriteAllBytes(filePath, imageBytes2);
        //return true;

        string MESSAGE = NOWTIMES + " 拍照成功";
        return MESSAGE;
    }
    ///給圖片加浮水印
    public static byte[] GetWatermarkPic(byte[] bytes, string txt)
    {
        MemoryStream ms = new MemoryStream(bytes);
        System.Drawing.Image pic = System.Drawing.Image.FromStream(ms);
        //創建一把刷子
        //防止索引圖元格式引發異常的圖元格式
        Bitmap bmp = new Bitmap(pic.Width, pic.Height, PixelFormat.Format32bppArgb);
        Graphics grap = Graphics.FromImage(bmp);
        grap.DrawImage(pic, 0, 0, pic.Width, pic.Height);
        //計算字體大小
        int fontSize = 40;
        if (pic.Width <= txt.Length * fontSize)
        {
            while (pic.Width <= (txt.Length * fontSize + 10))
            {
                fontSize = fontSize - 2;
            }
        }
        //浮水印寬度
        int txtWidth = txt.Length * fontSize;
        //字體
        var font = new Font("黑體", fontSize);
        Brush brush = new SolidBrush(Color.FromArgb(120, 193, 143, 8));
        grap.RotateTransform(0);
        grap.DrawString(txt, font, brush, float.Parse((pic.Width / 2 - txtWidth * 0.3).ToString()), 0);
        grap.Dispose();
        //返回有浮水印的圖片流
        MemoryStream stream = new MemoryStream();
        bmp.Save(stream, ImageFormat.Png);
        return stream.GetBuffer();
    }
    public static string UploadImage(byte[] imageBytes, string uploadFolderPath,string ID)
    {
        //DateTime DT = DateTime.Now;

        string FolderPathYEARS = ID.Substring(4, 4);
        StringBuilder FolderPath = new StringBuilder();
        FolderPath.AppendFormat(@"~/UPLOAD/COPTGCOPTH/{0}", FolderPathYEARS);


        uploadFolderPath = HttpContext.Current.Server.MapPath(FolderPath.ToString());

        // 檢查路徑是否存在，若不存在則建立目錄
        if (!Directory.Exists(uploadFolderPath))
        {
            Directory.CreateDirectory(uploadFolderPath);         
        }
        try
        {
            // 將 byte 陣列轉換為圖像
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                // 創建一個唯一的檔案名稱，例如使用 GUID
                string uniqueFileName = ID + ".jpg";

                // 將圖像保存到伺服器上的資料夾中
                string filePath = Path.Combine(uploadFolderPath, uniqueFileName);
                File.WriteAllBytes(filePath, imageBytes);
                //image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                return filePath; // 返回保存的文件路徑
            }
        }
        catch (Exception ex)
        {
            // 處理錯誤，例如記錄錯誤信息
            //Console.WriteLine("上傳圖像時出現錯誤：" + ex.Message);
            return null;
        }
    }

    public static void STATICADDPACKAGEBOXSPHOTO(string NO, string CTIMES, byte[] PHOTOS)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ERPconnectionstring"].ToString();
        Ede.Uof.Utility.Data.DatabaseHelper m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectionString);

        string cmdTxt = @"  
                       INSERT INTO [TKWAREHOUSE].[dbo].[PACKAGEBOXSPHOTO]
                        ([NO],[CTIMES],[PHOTOS])
                        VALUES
                        (@CHECKSPOINT,@CHECKSTIME,@PHOTOS)
                            ";


        m_db.AddParameter("@CHECKSPOINT", NO);
        m_db.AddParameter("@CHECKSTIME", CTIMES);
        m_db.AddParameter("@PHOTOS", PHOTOS);


        m_db.ExecuteNonQuery(cmdTxt);

    }
    public static async Task UPLOAD_TO_PIC()
    {
        string imageUrl = "https://eip.tkfood.com.tw/UOF/CDS/WebPage/STOCK/PIC/logo.jpg";
        using (var webClient = new WebClient())
        {
            byte[] imageBytes = webClient.DownloadData(imageUrl);
            UploadImage(imageBytes, "", "A23020230908003");
        }

    }

   
    #endregion


}
