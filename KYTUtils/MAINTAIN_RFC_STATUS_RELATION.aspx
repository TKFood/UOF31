<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MAINTAIN_RFC_STATUS_RELATION.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_MAINTAIN_RFC_STATUS_RELATION" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
    <%@ Register Src="~/KYTControl/KYTGridView.ascx" TagPrefix="uc1" TagName="KYTGridView" %>
    <%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
    <link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/KYTI.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>

    <script>
 /**
    * 檢查明細項
    * @param sender
    * @param args
    */
        function checkgvForms(sender, args) {
            var canSend = true;
            var gvMsg = "";
            var gvForms = $('#<%=gvForms.ClientID%>_GridView1'); // 明細項

            var gvIndex = 0;

            gvForms.find('TR').each(function (index, e) { // 巡覽TR
                var tr = $(e);
                if (tr.prop("innerText") == "目前沒有明細資料") {
                    gvMsg = "沒有資料";
                    return;
                }
                if (index == 0) return; // 標題不處理
                debugger;
                gvIndex++;
                var kddlFormName = tr.find('select[id*=kddlFormName]'); // 找表單名稱
                $(kddlFormName).css("background-color", "white");
                var ktxtRFC_NAME = tr.find('input[id*=ktxtRFC_NAME_TextBox1]'); // 找RFC_NAME
                $(ktxtRFC_NAME).css("background-color", "white");
                var ktxtDLL_PATH = tr.find('input[id*=ktxtDLL_PATH_TextBox1]'); // 找DLL_NAME
                $(ktxtDLL_PATH).css("background-color", "white");
                var ktxtZTABLE_NAME = tr.find('input[id*=ktxtZTABLE_NAME_TextBox1]'); // 找CLASS_NAME
                $(ktxtZTABLE_NAME).css("background-color", "white");

                var single_message = $.KYTValidators.format('第 {0} 筆資料', gvIndex);
                var valid_length = single_message.length; // 無錯誤時的 single_message 長度
                
                
                gvForms.find('TR').each(function (index, e) { // 巡覽TR
                    var tr = $(e);
                    if (tr.prop("innerText") == "目前沒有明細資料") {
                        gvMsg = "沒有資料";
                        return;
                    }
                    if (index <= gvIndex) return; // 跳過之前的，並且自己不檢查
                    //gvIndex++;
                    debugger;
                    var __kddlFormName = tr.find('select[id*=kddlFormName]'); // 找表單名稱
                    var __ktxtRFC_NAME = tr.find('input[id*=ktxtRFC_NAME_TextBox1]'); // 找RFC_NAME
                    var __ktxtDLL_PATH = tr.find('input[id*=ktxtDLL_PATH_TextBox1]'); // 找DLL_NAME
                    var __ktxtZTABLE_NAME = tr.find('input[id*=ktxtZTABLE_NAME_TextBox1]'); // 找CLASS_NAME

                    var valid_length = single_message.length; // 無錯誤時的 single_message 長度
                    if (kddlFormName.val() == __kddlFormName.val()) { // 一張表單一組RFC
                        __kddlFormName.val(''); // 清空表單名稱
                        __ktxtRFC_NAME.val(''); // 清空RFC_NAME
                        __ktxtDLL_PATH.val(''); // 清空DLL_NAME
                        __ktxtZTABLE_NAME.val(''); // 清空CLASS_NAME
                    }
                });

                if (kddlFormName.val() == "") {
                    single_message += ", 未選擇表單";
                    $(kddlFormName).css("background-color", "#F0775D");
                }
                
                if (ktxtRFC_NAME.val() == "") {
                    single_message += ", RFC_NAME沒填寫";
                    $(ktxtRFC_NAME).css("background-color", "#F0775D");
                }
                if (ktxtDLL_PATH.val() == "") {
                    single_message += ", DLL_NAME沒填寫";
                    $(ktxtDLL_PATH).css("background-color", "#F0775D");
                }
                if (ktxtZTABLE_NAME.val() == "") {
                    single_message += ", CLASS_NAME沒填寫";
                    $(ktxtZTABLE_NAME).css("background-color", "#F0775D");
                }

                if (single_message.length > valid_length)
                    gvMsg += single_message + '\n';
            });

            if (gvMsg != "") {
                canSend = false;
            }
            if (!canSend) { // 如果有錯誤訊息
                args.IsValid = false;
                // var message = "無法送單，請檢查";
                sender.innerText = gvMsg;
                alert(gvMsg);
            }
        }
    </script>
    <style>
        .AlternatingRowColor tr:nth-child(odd) {
              background-color: #f5f5f5;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 0px 10px 0px 20px;max-width:98vw;">
                <div class="row">
                    <div class="col-md-12 divheader">
                        <%=FORM_NAME %>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 divheader2">
                        <asp:Button ID="btnAdd" runat="server" Text="新增" OnClick="btnAdd_Click" CausesValidation="false" class="btn btn-primary btn-sm" />
                        <asp:Button ID="btnSave" runat="server" Text="儲存" OnClick="btnSave_Click" class="btn btn-success btn-sm" />
                    </div>
                </div>
                <div class="row" style="overflow-x:auto;">
                    <uc1:KYTGridView ID="gvForms" runat="server"
                        CssClass="tsGridView2 AlternatingRowColor" Width="100%"
                        ShowHeader="true" ShowHeaderWhenEmpty="true"
                        AutoGenerateColumns="false"
                        ForeColor="#333333"
                        OnRowDataBound="gvForms_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lbtnCOPY" Text="COPY" CausesValidation="false" OnClick="lbtnCOPY_Click" class="btn btn-warning btn-sm" />
                                    <asp:LinkButton runat="server" ID="lbtnDEL" Text="DEL" CausesValidation="false" OnClick="lbtnDEL_Click" class="btn btn-danger btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="表單名稱">
                                <ItemTemplate>
                                    <uc1:KYTDropDownList runat="server" ID="kddlFormName" FieldName="FORM_ID" ViewType="Input" />
                                    <asp:HiddenField runat="server" ID="hidDBData" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RFC名稱">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtRFC_NAME" FieldName="RFC_NAME" Text='<%#Bind("RFC_NAME")%>' ViewType="Input" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DLL檔案名稱">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtDLL_FILE_NAME"  FieldName="DLL_FILE_NAME" Text='<%#Bind("DLL_FILE_NAME")%>' ViewType="Input" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DLL組件路徑">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtDLL_PATH" FieldName="DLL_PATH" Text='<%#Bind("DLL_PATH")%>' ViewType="Input" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="重送RFC方法名稱">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtRE_PROCESS_NAME" FieldName="RE_PROCESS_NAME" Text='<%#Bind("RE_PROCESS_NAME")%>' ViewType="Input" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="中繼表主表表格名稱">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtZTABLE_NAME"  FieldName="ZTABLE_NAME" Text='<%#Bind("ZTABLE_NAME")%>' ViewType="Input" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </uc1:KYTGridView>
                </div>
                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Font-Bold="true" ForeColor="Red" Display="Dynamic" ClientValidationFunction="checkgvForms"></asp:CustomValidator>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
