<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestTemplateWordDownload.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_TestTemplateWordDownload" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
    <%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
    <%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
    <%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
    <%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
    <%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
    <%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
    <%@ Register Src="~/KYTControl/KYTGridView.ascx" TagPrefix="uc1" TagName="KYTGridView" %>
    <%@ Register Src="~/KYTControl/KYTRadioButtonList.ascx" TagPrefix="uc1" TagName="KYTRadioButtonList" %>
    <link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>
    <link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/KYTI.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
    <style>
        .tsGridView2 > tbody > tr > td {
            background: #FFF;
        }
    </style>
    <script>
</script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 10px 20px;">
                <div>
                    模板路徑:
                    <asp:TextBox runat="server" ID="txtPath" />
                    <asp:Button runat="server" ID="btnDownload" Text="下載" OnClick="btnDownload_Click" />
                </div>
                <div>
                    <asp:Button runat="server" ID="btnHeaderNew" Text="新增" OnClick="btnHeaderNew_Click" />
                    <uc1:KYTGridView runat="server" ID="gvHeader"
                        CssClass="tsGridView2 horzFull gvItems"
                        AutoGenerateColumns="false"
                        ShowHeader="true"
                        ShowHeaderWhenEmpty="false"
                        OnRowDataBound="gvHeader_RowDataBound">
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="Label11" runat="server" ForeColor="Blue" Font-Bold="true" Text="目前沒有明細資料"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="操作" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnDelete" Text="刪除" OnClick="btnDelete_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="txtID" FieldName="ID" Text='<%#Bind("ID")%>'></uc1:KYTTextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VALUE" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="txtVALUE" FieldName="VALUE" Text='<%#Bind("VALUE")%>'></uc1:KYTTextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </uc1:KYTGridView>
                </div>
                <div>
                    表格產生數量:
                    <asp:TextBox runat="server" ID="txtContentCount" />
                    表格ID:
                    <asp:TextBox runat="server" ID="txtContentID" />
                </div>
                <div>
                    <asp:Button runat="server" ID="btnContentNew" Text="新增" OnClick="btnContentNew_Click" />
                    <uc1:KYTGridView runat="server" ID="gvContent"
                        CssClass="tsGridView2 horzFull gvItems"
                        AutoGenerateColumns="false"
                        ShowHeader="true"
                        ShowHeaderWhenEmpty="false"
                        OnRowDataBound="gvContent_RowDataBound">
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="Label11" runat="server" ForeColor="Blue" Font-Bold="true" Text="目前沒有明細資料"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="操作" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btnDelete2" Text="刪除" OnClick="btnDelete2_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="txtID" FieldName="ID" Text='<%#Bind("ID")%>'></uc1:KYTTextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VALUE" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <uc1:KYTTextBox runat="server" ID="txtVALUE" FieldName="VALUE" Text='<%#Bind("VALUE")%>'></uc1:KYTTextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </uc1:KYTGridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
