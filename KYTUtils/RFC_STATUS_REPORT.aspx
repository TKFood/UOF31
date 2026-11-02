<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RFC_STATUS_REPORT.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_RFC_STATUS_REPORT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
    <%@ Register Src="~/KYTControl/KYTGridView.ascx" TagPrefix="uc1" TagName="KYTGridView" %>
    <%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
    <%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
    <%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>


    <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
    <link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/KYTI.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>
    <style>
        .pickerblock > div {
            display: inline-block; 
        }
        .AlternatingRowColor tr:nth-child(odd) {
              background-color: #f5f5f5;
        }
    </style>
    <script>

    function CheckAllC(oCheckbox)
     {
        var HeaderCheck = $("#" + oCheckbox.id).prop("checked");
        var gvItems = $('#<%=gvItems.ClientID%>_GridView1'); // 明細項
        gvItems.find('TR').each(function (index, e) { // 巡覽TR
            var tr = $(e);
            if (tr.prop("innerText") == "目前沒有明細資料") {
                gvMsg = "沒有資料";
                return;
            }
            if (index == 0) return; // 標題不處理
            var kcbSelect = tr.find('input[id*=kcbSelect_CheckBox1]'); // 找勾選
            if (kcbSelect.length > 0) {
               kcbSelect.prop("checked", HeaderCheck);
            }

        });
      
      }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div  style="padding: 0px 10px 0px 20px;max-width:98vw;">
                <div class="row">
                    <div class="col-md-12 divheader2">
                        <%=FORM_NAME %>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">
                        表單名稱
                    </div>
                    <div class="col-md-10">
                        <uc1:KYTDropDownList runat="server" ID="kddlSFORM_NAME" ViewType="Input" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        表單編號
                    </div>
                    <div class="col-md-10">
                        <uc1:KYTTextBox runat="server" ID="ktxtSDOC_NBR" Width="300px" ViewType="Input" placeholder="輸入表單編號(可搜尋不完整的編號)" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        標題
                    </div>
                    <div class="col-md-10">
                        <uc1:KYTTextBox runat="server" ID="ktxtSDISPLAY_TITLE" Width="300px" ViewType="Input" placeholder="輸入表單標題(可搜尋標題關鍵字)" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        狀態
                    </div>
                    <div class="col-md-10">
                        <uc1:KYTDropDownList runat="server" ID="kddlRFC_STATUS" ViewType="Input">
                            <asp:ListItem Value="" Selected="True">全部</asp:ListItem>
                            <asp:ListItem Value="1">成功</asp:ListItem>
                            <asp:ListItem Value="0">失敗</asp:ListItem>
                            <asp:ListItem Value="NULL">取消</asp:ListItem>
                        </uc1:KYTDropDownList>

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        申請時間
                    </div>
                    <div class="col-md-10 pickerblock" >
                        <uc1:KYTDatePicker runat="server" ID="kdpSTART" TextBoxReadOnly="true" />
                        ~
                        <uc1:KYTDatePicker runat="server" ID="kdpEND" TextBoxReadOnly="true" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" CausesValidation="false" class="btn btn-warning btn-block"  />
                    </div>
                    <div class="col-md-6">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 divheader">
                        <asp:Button ID="btnRUN" runat="server" Text="變更狀態" OnClick="btnRUN_Click" CausesValidation="false" class="btn btn-success btn-sm" />
                    </div>
                </div>
                <div class="row" style="overflow-x:auto;">

                    <uc1:KYTGridView ID="gvItems" runat="server"
                        CssClass="tsGridView2 AlternatingRowColor" Width="100%"
                        ShowHeader="true" ShowHeaderWhenEmpty="true"
                        AutoGenerateColumns="false"
                        ForeColor="#333333"
                        OnRowDataBound="gvItems_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="Checkbox2" runat="server" type="checkbox" onclick="CheckAllC(this)" />
                                    全選
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <uc1:KYTCheckBox runat="server" ID="kcbSelect" CheckedValue="X" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <uc1:KYTDropDownList runat="server" ID="kddlRFC_STATUS" ViewType="Input" FieldName="SEND_RESULT">
                                        <asp:ListItem Value="1">成功</asp:ListItem>
                                        <asp:ListItem Value="0">失敗</asp:ListItem>
                                        <asp:ListItem Value="NULL">取消</asp:ListItem>
                                    </uc1:KYTDropDownList>
                                    <asp:LinkButton runat="server" ID="lbtnReSend" Text="重送" CausesValidation="false" class="btn btn-danger btn-sm" OnClick="lbtnReSend_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="表單名稱" DataField="FORM_NAME" />
                            <asp:BoundField HeaderText="表單版本" DataField="VERSION" />
                            <asp:TemplateField HeaderText="表單編號">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnDOC_NBR" runat="server"  Text='<%#Bind("DOC_NBR")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="標題">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtDISPLAY_TITLE" Width="300px" FieldName="DISPLAY_TITLE" Text='<%#Bind("DISPLAY_TITLE")%>' ViewType="ReadOnly" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="申請時間" DataField="APPLICANTDATE" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
                            <asp:BoundField HeaderText="RFC呼叫時間" DataField="MODIFY_TIME" />
                            <asp:TemplateField HeaderText="RFC訊息">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="ktxtMSG_EXCEPTION" Width="300px" FieldName="MSG_EXCEPTION" Text='<%#Bind("MSG_EXCEPTION")%>' ViewType="ReadOnly" />
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
