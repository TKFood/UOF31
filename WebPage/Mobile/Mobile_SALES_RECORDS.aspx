<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MobileMasterPage.master" AutoEventWireup="true" CodeFile="Mobile_SALES_RECORDS.aspx.cs" Inherits="CDS_WebPage_Mobile_SALES_RECORDS" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- 在CSS文件中定义样式 -->
    <style>
        .custom-button {
            background-color: #FF5733;
            color: #FFFFFF;
            width: 150px; /* 设置宽度 */
            height: 40px; /* 设置高度 */
            font-size: 16px; /* 设置字体大小 */
        }

        .fast-grid {
        }

            /* 表头样式 */
            .fast-grid th {
                background-color: dodgerblue; /* 背景颜色 */
                color: white; /* 字体颜色 */
                font-weight: bold; /* 字体粗体 */
                border: 2px solid white; /* 边框粗线 */
                padding: 5px;
                text-align: center;
            }

            /* 单元格框线 */
            .fast-grid td {
                border: 1px solid #000;
                padding: 5px;
                text-align: left; /* 根据需要更改文本对齐方式 */
            }
    </style>
    <html>

    <head>
        <title>業務</title>
    </head>
    <body>
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
        <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" OnClientTabSelected="OnClientTabSelected">
            <Tabs>
                <telerik:RadTab Text="業務" PageViewID="RadPageView1">
                </telerik:RadTab>
                <telerik:RadTab Text="報表" PageViewID="RadPageView2">
                </telerik:RadTab>
                <telerik:RadTab Text="交辨回覆" PageViewID="RadPageView3">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
            <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true" PageViewID="0">
                <div id="tabs-1">
                    <table class="PopTable">
                        <tr>
                            <td>
                                <h2>拜訪</h2>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="業務員"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListSALESNAMES" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListSALESNAMES_SelectedIndexChanged" Style="width: 200px;"></asp:DropDownList>
                                <asp:Label ID="SALESID" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="舊客戶(有客代): "></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="關鍵字 "></asp:Label>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                <asp:Button ID="Button1" runat="server" Text="查客戶 " OnClick="btn1_Click" meta:resourcekey="btn1_Resource1" />
                                <asp:DropDownList ID="DropDownListCLIENTSNAMES" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListCLIENTSNAMES_SelectedIndexChanged" Style="width: 200px;"></asp:DropDownList>
                                <asp:Label ID="CLIENTSID" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="新客戶(無客代): "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="NEWCLIENTSNAMES" runat="server" Style="width: 200px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="拜訪目的: "></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListKINDS" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="客戶拜訪內容: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="RECORDS" runat="server" TextMode="MultiLine" Rows="4" Columns="40"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="客戶拜訪日期 "></asp:Label>

                            </td>
                            <td>
                                <asp:TextBox ID="RECORDSDATES" runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td>
                                <input type="file" accept="image/*" capture="camera" id="photoInputold" style="display: none" />
                            </td>
                        </tr>
                        <%--   <tr>
                            <td></td>
                            <td>
                                <button type="button" id="takePhotoButtonold">拍照</button>
                            </td>
                        </tr>
                        <tr>
                            <td>照片
                            </td>
                            <td>
                                <img id="previewImage" style="max-width: 50%;" />
                            </td>
                        </tr>--%>


                        <tr>
                            <td></td>
                            <td>
                                <div>
                                    <!-- Button to trigger taking a photo -->
                                    <input type="file" id="photoInput" style="display: none;" accept="image/*" />
                                    <button type="button" id="takePhotoButton">拍照</button>

                                    <!-- Container to hold the photos -->
                                    <div id="photoContainer">
                                        <!-- Dynamic photos will be added here -->
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <button type="button" id="btnUpload" class="custom-button">存檔</button>
                            </td>
                        </tr>
                    </table>

                </div>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView2" runat="server" Selected="false" PageViewID="1">
                <div id="tabs-2">
                    <table class="PopTable">
                        <tr>
                            <td class="PopTableLeftTD">
                                <asp:Label ID="Label8" runat="server" Text="日期:" meta:resourcekey="Label4Resource1"></asp:Label>
                            </td>
                            <td class="PopTableRightTD">
                                <asp:TextBox ID="txtDate1" runat="server" Width="100px"></asp:TextBox>
                                <asp:Label ID="Label11" runat="server" Text="~"></asp:Label>
                                <asp:TextBox ID="txtDate2" runat="server" Width="100px"></asp:TextBox>
                                <asp:Label ID="Label12" runat="server" Text=" "></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="PopTableLeftTD"></td>
                            <td>
                                <asp:Button ID="Button2" runat="server" Text=" 查詢 " OnClick="btn2_Click" meta:resourcekey="btn2_Resource1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="PopTableLeftTD"></td>
                            <td>
                                <asp:Button ID="Button3" runat="server" Text=" 匯出 " OnClick="btn3_Click" meta:resourcekey="btn3_Resource1" />
                            </td>
                        </tr>

                    </table>
                    <table style="width: 100%">
                        <tr style="width: 100%">
                            <td colspan="2" style="width: 100%">
                                <div style="overflow-x: auto; width: 100%">
                                    <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_OnRowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1" CssClass="fast-grid">

                                        <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                                        <Columns>
                                            <asp:BoundField HeaderText="業務員" DataField="業務員" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="舊客(有客代)" DataField="客戶" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="新客" DataField="新客" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="拜訪目的" DataField="拜訪目的" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="訪談內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="訪談內容" runat="server" Text='<%# Bind("訪談內容") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="訪談日期" DataField="訪談日期" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="圖片">
                                                <ItemTemplate>
                                                    <div runat="server" id="ImageContainer" class="image-container">
                                                        <!-- Images will be added here dynamically -->
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="是否刪除" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="Grid1Button1" runat="server" Text="刪除" CommandName="Grid1Button1" ForeColor="Red" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('確定要刪除嗎？');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView3" runat="server" Selected="false" PageViewID="3">
                <div id="tabs-2">
                    <table class="PopTable">
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="客戶: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox_CLIENTS" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="是否結案: "></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListISCLOSE" runat="server" AutoPostBack="true" Style="width: 200px;"></asp:DropDownList>

                            </td>
                        </tr>
                        <tr>
                            <td class="PopTableLeftTD"></td>
                            <td>
                                <asp:Button ID="Button4" runat="server" Text=" 查詢 " OnClick="btn4_Click" meta:resourcekey="btn4_Resource1" />
                            </td>
                        </tr>


                    </table>
                    <table style="width: 100%">
                        <tr style="width: 100%">
                            <td colspan="2" style="width: 100%">
                                <div style="overflow-x: auto; width: 100%">
                                    <Fast:Grid ID="Grid3" OnRowDataBound="Grid3_RowDataBound" OnRowCommand="Grid3_OnRowCommand" runat="server" OnBeforeExport="OnBeforeExport3" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1" CssClass="fast-grid">
                                        <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource"></ExportExcelSettings>
                                        <Columns>
                                             <asp:TemplateField HeaderText="業務員" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="業務員" runat="server" Text='<%# Bind("SALES") %>' Style="word-break: break-all; white-space: pre-line" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="客戶" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="客戶" runat="server" Text='<%# Bind("CLIENTS") %>' Style="word-break: break-all; white-space: pre-line" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="回覆期限" DataField="EDAYS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="交辨內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="交辨內容" runat="server" Text='<%# Bind("EVENTS") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="已回覆內容" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="Label10" runat="server" Text='<%# Bind("COMMENTS") %>' Style="word-break: break-all; white-space: pre-line;" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="是否結案" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="是否結案" runat="server" Text='<%# Bind("ISCLOSE") %>' Style="word-break: break-all; white-space: pre-line;" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="輸入回覆" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNewField" runat="server" Text='<%# Bind("COMMENTS") %>' TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="新增回覆" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="Grid3Button1" runat="server" Text="新增回覆" CommandName="Grid3Button1" ForeColor="Red" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('確定？');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>                                       
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>' Style="word-break: break-all; white-space: pre-line" Width="30px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                           
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </telerik:RadPageView>

        </telerik:RadMultiPage>​
           
    </body>

    <script src="compressor/compressor.min.js"></script>

    <script>
        //window.onload = function () {
        //    var photoInput = document.getElementById("photoInput");
        //    var takePhotoButton = document.getElementById("takePhotoButton");
        //    var previewImage = document.getElementById("previewImage");

        //    takePhotoButton.onclick = function () {
        //        photoInput.click();
        //    };

        //    photoInput.onchange = function () {
        //        if (photoInput.files && photoInput.files[0]) {
        //            var reader = new FileReader();
        //            reader.onload = function (e) {
        //                previewImage.src = e.target.result;
        //            };
        //            reader.readAsDataURL(photoInput.files[0]);
        //        }
        //    };
        //};

        window.onload = function () {
            var photoInput = document.getElementById("photoInput");
            var takePhotoButton = document.getElementById("takePhotoButton");
            var photoContainer = document.getElementById("photoContainer");

            takePhotoButton.onclick = function () {
                photoInput.click();
            };

            photoInput.onchange = function () {
                if (photoInput.files && photoInput.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var photoUrl = e.target.result;

                        // 创建新的行元素
                        var row = document.createElement("div");
                        row.style.clear = "both"; // 清除浮动

                        // 创建新的 img 元素
                        var img = document.createElement("img");
                        img.src = photoUrl;
                        img.style.maxWidth = "50%";
                        img.style.float = "left"; // 设置为左浮动                       

                        // 将 img 元素添加到行元素中
                        row.appendChild(img);

                        // 将行元素添加到 photoContainer 中
                        photoContainer.appendChild(row);
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
            alert('AJAX request failed:', error);
        }


        // 壓縮圖片的函數
        function compressImage(image, quality, callback) {
            const canvas = document.createElement('canvas');
            const context = canvas.getContext('2d');
            canvas.width = 200;
            canvas.height = 200;
            context.drawImage(image, 0, 0, canvas.width, canvas.height);

            canvas.toBlob(function (blob) {
                callback(blob);
            }, 'image/jpeg', quality);
        }


        $(function () {
            $("#btnUpload").click(function () {
                //SET_NEW_PHOTOSID
                //PageMethods.SET_NEW_PHOTOSID();
                // 取得當前日期時間
                var currentDate = new Date();

                // 格式化日期時間，例如：yyyyMMddHHmmss
                var formattedDateTime =
                    currentDate.getFullYear().toString() +
                    padZero(currentDate.getMonth() + 1) +
                    padZero(currentDate.getDate()) +
                    padZero(currentDate.getHours()) +
                    padZero(currentDate.getMinutes()) +
                    padZero(currentDate.getSeconds());

                // 函數：補零，確保單位數的月、日、時、分、秒有兩位數
                function padZero(number) {
                    return number < 10 ? "0" + number : number.toString();
                }

                // 使用 formattedDateTime
                var PHOTOSID = formattedDateTime;

                // DropDownListSALESNAMES获取 DropDownList 的元素
                var SALESNAMES = document.getElementById('<%=DropDownListSALESNAMES.ClientID%>');
                if (SALESNAMES) {
                    // 如果SALESNAMES不为null或undefined，执行以下代码
                    // 获取选中项的索引              
                    var selectedIndex_SALESNAMES = SALESNAMES.selectedIndex;
                    // 获取选中项的文本
                    var selectedText_SALESNAMES = SALESNAMES.options[selectedIndex_SALESNAMES].text;

                    // 在这里执行其他操作
                } else {
                    // SALESNAMES为null或undefined，可以执行其他操作或者不执行任何操作
                }
                //DropDownListCLIENTSNAMES 获取 DropDownList 的元素
                var CLIENTSNAMES = document.getElementById('<%=DropDownListCLIENTSNAMES.ClientID%>');
                if (CLIENTSNAMES) {
                    // 如果SALESNAMES不为null或undefined，执行以下代码
                    // 获取选中项的索引
                    var selectedIndex_CLIENTSNAMES = CLIENTSNAMES.selectedIndex;
                    // 获取选中项的文本
                    var selectedText_CLIENTSNAMES = CLIENTSNAMES.options[selectedIndex_CLIENTSNAMES].text;

                    // 在这里执行其他操作
                } else {
                    // SALESNAMES为null或undefined，可以执行其他操作或者不执行任何操作
                }

                //DropDownListKINDS 获取 DropDownList 的元素
                var KINDS = document.getElementById('<%=DropDownListKINDS.ClientID%>');
                if (KINDS) {
                    // 获取选中项的索引
                    var selectedIndex_KINDS = KINDS.selectedIndex;
                    // 获取选中项的文本
                    var selectedText_KINDS = KINDS.options[selectedIndex_KINDS].text;
                    // 在这里执行其他操作
                } else {
                    // SALESNAMES为null或undefined，可以执行其他操作或者不执行任何操作
                }



                var CLIENTSID = document.getElementById('<%=CLIENTSID.ClientID%>').innerHTML;
                var NEWCLIENTSNAMES = document.getElementById('<%=NEWCLIENTSNAMES.ClientID%>').value;
                var RECORDS = document.getElementById('<%=RECORDS.ClientID%>').value;
                var RECORDSDATES = document.getElementById('<%=RECORDSDATES.ClientID%>').value;

                //var previewImage = document.getElementById("previewImage");
                //var imgCapture = $("#previewImage")[0].src;

                //存檔-客戶記錄-SaveCapturedImage_NOIMAGE
                PageMethods.SaveCapturedImage_NOIMAGE(selectedText_SALESNAMES, CLIENTSID, selectedText_CLIENTSNAMES, NEWCLIENTSNAMES, selectedText_KINDS, RECORDS, RECORDSDATES, PHOTOSID, Success, Failure);

                // 取得 photoContainer 中的所有 img 元素
                var imgElements = $("#photoContainer img");

                //// 迴圈處理每個 img 元素
                //imgElements.each(function () {
                //    // 取得圖片的路徑
                //    var imagePath = $(this).attr("src");

                //    // 使用 fetch 取得圖片
                //    fetch(imagePath)
                //        .then(response => response.blob())
                //        .then(blob => {
                //            // 計算原始大小
                //            const originalSizeKB = Math.ceil(blob.size / 1024);
                //            //alert('imagePath  ' + imagePath);
                //            // 使用 Compressor.js 進行圖片壓縮
                //            new Compressor(blob, {
                //                quality: 1,
                //                maxWidth: 200,      // 調整最大寬度，影響圖片大小
                //                maxHeight: 200,     // 調整最大高度，影響圖片大小
                //                //convertSize: 200,   // 調整轉換大小，影響圖片大小
                //                success(result) {
                //                    // result 是壓縮後的 Blob 物件

                //                    // 將壓縮後的 Blob 轉換為 Base64 字串
                //                    const reader = new FileReader();
                //                    reader.onload = function () {
                //                        const compressedBase64 = reader.result;
                //                        //alert('compressedBase64 ' + compressedBase64);
                //                        // 計算壓縮後大小（以KB為單位）
                //                        const compressedSizeKB = Math.ceil((compressedBase64.length * 3) / 4 / 1024);

                //                        //// 顯示大小資訊
                //                        //alert('originalSizeKB ' + originalSizeKB);
                //                        //alert('compressedSizeKB ' + compressedSizeKB);

                //                        // 使用 PageMethods.SaveCapturedImage 上傳壓縮後的圖片
                //                        PageMethods.SaveCapturedImage_TB_SALES_RECORDS_PHOTOS(compressedBase64, Success, Failure);
                //                    };
                //                    reader.readAsDataURL(result);
                //                },
                //                error(err) {
                //                    console.error(err.message);
                //                },
                //            });
                //        })
                //        .catch(error => console.error(error));
                //});



                // 迴圈處理每個 img 元素
                imgElements.each(function () {
                    // 取得圖片的路徑
                    var imagePath = $(this).attr("src");
                    // 載入圖片
                    var image = new Image();
                    image.src = imagePath;
                    if (image !== "" && image !== undefined) {
                        //PageMethods.SaveCapturedImage_TB_SALES_RECORDS_PHOTOS(imagePath, Success, Failure)
                        const originalSizeKB = Math.ceil((image.src.length));
                        // 壓縮圖片並使用 PageMethods.SaveCapturedImage 上傳
                        compressImage(image, 0.3, function (compressedBlob) {
                            // 將壓縮後的圖片轉換為Base64字串
                            const reader = new FileReader();
                            reader.onload = function () {
                                const compressedBase64 = reader.result;
                                const compressedSizeKB = Math.ceil((compressedBase64.length));

                                //alert('originalSizeKB ' + Math.ceil(originalSizeKB / 1024));
                                //alert('compressedSizeKB ' + Math.ceil(compressedSizeKB / 1024));

                                // 使用 PageMethods.SaveCapturedImage 上傳壓縮後的圖片
                                //alert('BEFORE');
                                PageMethods.SaveCapturedImage_TB_SALES_RECORDS_PHOTOS(compressedBase64, PHOTOSID)

                                //alert('AFTER');
                            };
                            reader.readAsDataURL(compressedBlob);
                        });
                    }
                });


                ////圖片!== ""
                //if (imgCapture !== "" && imgCapture !== undefined) {
                //    // 壓縮圖片並使用 PageMethods.SaveCapturedImage 上傳
                //    compressImage(previewImage, 0.5, function (compressedBlob) {
                //        // 將壓縮後的圖片轉換為Base64字串
                //        const reader = new FileReader();
                //        reader.onload = function () {
                //            const compressedBase64 = reader.result;
                //            // 使用 PageMethods.SaveCapturedImage 上傳壓縮後的圖片
                //            PageMethods.SaveCapturedImage(selectedText_SALESNAMES, CLIENTSID, selectedText_CLIENTSNAMES, NEWCLIENTSNAMES, selectedText_KINDS, RECORDS, RECORDSDATES, PHOTOSID,compressedBase64, Success, Failure);
                //        };
                //        reader.readAsDataURL(compressedBlob);
                //    });
                //}
                //else {
                //    PageMethods.SaveCapturedImage_NOIMAGE(selectedText_SALESNAMES, CLIENTSID, selectedText_CLIENTSNAMES, NEWCLIENTSNAMES, selectedText_KINDS, RECORDS, RECORDSDATES, Success, Failure);
                //    reader.readAsDataURL(compressedBlob);
                //}


                //PageMethods.SaveCapturedImage(myTextcontent, imgCapture, Success, Failure);
                //PageMethods.TEST(Success, Failure);
              
            });
        });
        $(function () {
            $("#<%= RECORDSDATES.ClientID %>").datepicker({ dateFormat: "yy/mm/dd", });

        });

        function OnClientTabSelected(sender, eventArgs) {
            var tabStrip = sender;
            var tab = eventArgs.get_tab();
            //alert(tab.get_text());

            if (tab.get_text() == '報表') {               
                // 模拟调用ASP.NET按钮点击事件
                $("#<%= Button2.ClientID %>").click();
            }
        }
    </script>

    </html>



</asp:Content>

