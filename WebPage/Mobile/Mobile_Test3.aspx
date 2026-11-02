<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MobileMasterPage.master" AutoEventWireup="true" CodeFile="Mobile_TEST3.aspx.cs" Inherits="CDS_WebPage_Mobile_Mobile_TEST3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <html>

    <head>

        <title>Html-Qrcode</title>
        <body>
            <div>
                <table>
                    <tr>
                        <td>
                            <div id="qr-reader" style="width: 500px"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="檢查點: "></asp:Label>
                            <asp:TextBox ID="myTextcontent" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <button onclick="myFunction()">重拍</button>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="現在日期時間: "></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="打卡存檔" OnClick="Button1_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="qr-reader-results"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="AA1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text="Get Name" OnClientClick='GetName();return false;' />
            </div>
            </td>
                    </tr>
                </table>
            </div>


            <div>
                <%--<input type="text" id="myTextcontent" runat="server" value="">--%>
            </div>

            <div>
            </div>
            <div>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <th align="center"><u>Live Camera</u></th>
                        <th align="center"><u>Captured Picture</u></th>
                    </tr>
                    <tr>
                        <td>
                            <div id="webcam"></div>
                        </td>
                        <td>
                            <img id="imgCapture" /></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <input type="button" id="btnCapture" value="Capture" />
                        </td>
                        <td align="center">
                            <input type="button" id="btnUpload" value="Upload" />
                        </td>
                    </tr>
                    <tr>
                        <td>

                        </td>
                    </tr>
                    <tr>
                        <td>
                             <input type="button" id="btnUpload" value="Upload" />
                        </td>
                    </tr>
                </table>
            </div>
        </body>

        <%-- 呼叫https://unpkg.com/html5-qrcode 來開啟手機相機 打卡 QR CODE --%>
        <%--<script src="https://unpkg.com/html5-qrcode"></script>--%>
        <script src="HTML5/html5-qrcode.min.js"></script>
        
        <script src="WebCam/WebCam.js"></script>

        <script>
            function myFunction() {
                var resultContainer = document.getElementById('qr-reader-results');
                var lastResult, countResults = 0;
                function onScanSuccess(decodedText, decodedResult) {
                    if (decodedText !== lastResult) {
                        ++countResults;
                        lastResult = decodedText;
                        // Handle on success condition with the decoded message.
                        //console.log(`Scan result ${decodedText}`, decodedResult);

                        //alert(decodedText);
                        //呼叫JS的GetTAKEPIC_ADDTKGAFFAIRSCHECKSPOOINT
                        GetTAKEPIC_ADDTKGAFFAIRSCHECKSPOOINT();

                        //SET ASP.NET的myTextcontent值=QR CODE打卡點
                        document.getElementById('<%=myTextcontent.ClientID%>').value = decodedText;

                        html5QrcodeScanner.clear();
                    }
                }

                //拍照的畫面大小
                var html5QrcodeScanner = new Html5QrcodeScanner(
                    "qr-reader", { fps: 10, qrbox: 250 });
                html5QrcodeScanner.render(onScanSuccess);

                document.getElementById('<%=myTextcontent.ClientID%>').value = null;
            }

            ///dom docReady
            function docReady(fn) {
                // see if DOM is already available
                if (document.readyState === "complete"
                    || document.readyState === "interactive") {
                    // call on next available tick
                    setTimeout(fn, 1);
                } else {

                    document.addEventListener("DOMContentLoaded", fn);

                }
            }

            //dom docReady後，開啟相機掃QR CODE
            docReady(function () {

                var resultContainer = document.getElementById('qr-reader-results');
                var lastResult, countResults = 0;
                function onScanSuccess(decodedText, decodedResult) {
                    if (decodedText !== lastResult) {
                        ++countResults;
                        lastResult = decodedText;
                        // Handle on success condition with the decoded message.
                        //console.log(`Scan result ${decodedText}`, decodedResult);

                        //alert(decodedText);
                        Webcam.snap(function (data_uri) {
                            $("#imgCapture")[0].src = data_uri;

                        });

                        var myTextcontent = decodedText;
                        //alert(myTextcontent);      

                        GetTAKEPIC_ADDTKGAFFAIRSCHECKSPOOINT(myTextcontent);

                        document.getElementById('<%=myTextcontent.ClientID%>').value = decodedText;

                        html5QrcodeScanner.clear();
                    }
                }

                var html5QrcodeScanner = new Html5QrcodeScanner(
                    "qr-reader", { fps: 10, qrbox: 250 });
                html5QrcodeScanner.render(onScanSuccess);

                document.getElementById('<%=myTextcontent.ClientID%>').value = null;
            });

            //JS GetName
            function GetName() {
                PageMethods.Name(Success, Failure);
            }


            //JS GetTAKEPIC_ADDTKGAFFAIRSCHECKSPOOINT
            function GetTAKEPIC_ADDTKGAFFAIRSCHECKSPOOINT(myTextcontent) {
                //alert(myTextcontent);
                //把QR CODE的值傳入到C#的TAKEPIC_ADDTKGAFFAIRSCHECKSPOOINT
                PageMethods.TAKEPIC_ADDTKGAFFAIRSCHECKSPOOINT(myTextcontent, Success, Failure);
            }

            //JS執行成功Success
            function Success(result) {
                alert(result);
            }
            //JS執行失敗
            function Failure(error) {
                alert(error);
            }

            $(function () {
                Webcam.set({
                    width: 160,
                    height: 120,
                    image_format: 'jpeg',
                    jpeg_quality: 90,
                    facingMode: 'environment'
                });
                Webcam.attach('#webcam');
                $("#btnCapture").click(function () {
                    Webcam.snap(function (data_uri) {
                        $("#imgCapture")[0].src = data_uri;
                        
                    });
                });
                $("#btnUpload").click(function () {
                    var myTextcontent = document.getElementById('<%=myTextcontent.ClientID%>').value;
                    var imgCapture = $("#imgCapture")[0].src;
                    PageMethods.SaveCapturedImage(myTextcontent, imgCapture, Success, Failure);

                    //$.ajax({
                    //    type: "POST",
                    //    url: "Mobile_Test3.aspx/SaveCapturedImage",
                    //    data: "{data: '" + $("#imgCapture")[0].src + "'}",
                    //    contentType: "application/json; charset=utf-8",
                    //    dataType: "json",
                    //    success: function (r) { }
                    //});
                });
            });
        </script>
    </head>
    </html>



</asp:Content>

