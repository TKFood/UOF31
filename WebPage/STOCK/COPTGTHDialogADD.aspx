<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="COPTGTHDialogADD.aspx.cs" Inherits="CDS_WebPage_STOCK_COPTGTHDialogADD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <html>

    <head>
        <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">
        <div style="overflow-x: auto; width: 800px">
        </div>
        <table class="PopTable">
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label1" runat="server" Text="ID"></asp:Label>
                </td>
                <td class="PopTableRightTD" colspan="2">
                    <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>


        <title>Html-Qrcode</title>
        <body>

            <div>
                <table class="PopTable">
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label2" runat="server" Text="銷貨單別-單號"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <input type="text" id="myTextcontent" runat="server" value="">
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label3" runat="server" Text="箱號"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label4" runat="server" Text="網購包材重量(KG)A"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label5" runat="server" Text="箱商品總重量(KG)B號"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label6" runat="server" Text="秤總重(A+B)"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label7" runat="server" Text="實際比值"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label8" runat="server" Text="商品總重量比值分類"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label9" runat="server" Text="規定比值"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label10" runat="server" Text="是否符合"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label11" runat="server" Text="使用包材名稱/規格"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label12" runat="server" Text="使用包材來源"></asp:Label>
                        </td>
                        <td class="PopTableRightTD" colspan="2">
                            <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
                        </td>
                    </tr>

                </table>

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
                        <td></td>
                    </tr>
                </table>
            </div>
        </body>

        <%-- 呼叫https://unpkg.com/html5-qrcode 來開啟手機相機 打卡 QR CODE --%>


        <script src="WebCam/WebCam.js"></script>

        <script>       
            //JS GetName
            function GetName() {
                PageMethods.Name(Success, Failure);
            }


            //JS GetTAKEPIC_ADDTKGAFFAIRSCHECKSPOOINT
            function GetTAKEPIC_ADDCHECKSPOOINT(myTextcontent) {
                //alert(myTextcontent);
                //把QR CODE的值傳入到C#的TAKEPIC_ADDCHECKSPOOINT
                PageMethods.GetTAKEPIC_ADDCHECKSPOOINT(myTextcontent, Success, Failure);
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
                    constraints: {
                        //開啟後鏡頭
                        facingMode: 'environment'
                    }
                });
                Webcam.attach('#webcam');
                $("#btnCapture").click(function () {
                    Webcam.snap(function (data_uri) {
                        $("#imgCapture")[0].src = data_uri;

                    });
                });
                $("#btnUpload").click(function () {
                    var myTextcontent = document.getElementById('<%=myTextcontent.ClientID%>').value;                
                    var NO = document.getElementById('<%=myTextcontent.ClientID%>').value + "-" + document.getElementById('<%=TextBox1.ClientID%>').value;

                    var imgCapture = $("#imgCapture")[0].src;
                    PageMethods.SaveCapturedImage(NO, imgCapture, Success, Failure);

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


