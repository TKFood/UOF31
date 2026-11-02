<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MobileMasterPage.master" AutoEventWireup="true" CodeFile="Mobile_TEST2.aspx.cs" Inherits="CDS_WebPage_Mobile_Mobile_TEST2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
    <html>

    <head>
        <title>拍照範例</title>
    </head>
    <body>
        <div>
            <table>
                <tr>
                    <td>
                        <h2>拜訪拍照</h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="file" accept="image/*" capture="camera" id="photoInput" style="display: none" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <button type="button" id="takePhotoButton">拍照</button>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h2>照片</h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img id="previewImage" style="max-width: 50%;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="客戶: "></asp:Label>
                        <asp:TextBox ID="myTextcontent" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <button type="button" id="btnUpload">上傳存檔</button>
                    </td>
                </tr>
            </table>



        </div>
    </body>

    
    <script>
        window.onload = function () {
            var photoInput = document.getElementById("photoInput");
            var takePhotoButton = document.getElementById("takePhotoButton");
            var previewImage = document.getElementById("previewImage");

            takePhotoButton.onclick = function () {
                photoInput.click();
            };

            photoInput.onchange = function () {
                if (photoInput.files && photoInput.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        previewImage.src = e.target.result;
                    };
                    reader.readAsDataURL(photoInput.files[0]);
                }
            };
        };

        //JS執行成功Success
        function Success(result) {
            alert(result);
        }
        //JS執行失敗
        function Failure(error) {
            alert(error);
        }

        $(function () {
            $("#btnUpload").click(function () {
                var myTextcontent = document.getElementById('<%=myTextcontent.ClientID%>').value;
                var previewImage = document.getElementById("previewImage");
                var imgCapture = $("#previewImage")[0].src;
        

                // 壓縮圖片的函數
                function compressImage(image, quality, callback) {
                    const canvas = document.createElement('canvas');
                    const context = canvas.getContext('2d');
                    canvas.width = image.width;
                    canvas.height = image.height;
                    context.drawImage(image, 0, 0, canvas.width, canvas.height);

                    canvas.toBlob(function (blob) {
                        callback(blob);
                    }, 'image/jpeg', quality);
                }

                // 壓縮圖片並使用 PageMethods.SaveCapturedImage 上傳
                compressImage(previewImage, 0.5, function (compressedBlob) {
                    // 將壓縮後的圖片轉換為Base64字串
                    const reader = new FileReader();
                    reader.onload = function () {
                        const compressedBase64 = reader.result;

                        // 使用 PageMethods.SaveCapturedImage 上傳壓縮後的圖片
                        PageMethods.SaveCapturedImage(myTextcontent, compressedBase64, Success, Failure);
                    };
                    reader.readAsDataURL(compressedBlob);
                });

                //PageMethods.SaveCapturedImage(myTextcontent, imgCapture, Success, Failure);
                //PageMethods.TEST(Success, Failure);

            });
        });
    </script>

    </html>



</asp:Content>

