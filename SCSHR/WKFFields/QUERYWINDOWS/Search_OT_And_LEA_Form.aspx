<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" CodeFile="Search_OT_And_LEA_Form.aspx.cs" Inherits="CDS_SCSHR_WKFFields_QUERYWINDOWS_Search_OT_And_LEA_Form" %>

    <%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/QUERYWINDOWS.css")%>" rel="stylesheet" />
    
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>

    <style>

    .inline {
        display: inline-flex !important;
    }

    </style>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                表單單號:
                <asp:TextBox ID="txtDOC_NBR" runat="server" AutoPostBack="true" OnTextChanged="btnSEARCH_Click"></asp:TextBox>
                日期時間(起)~(迄):
                <div class="inline">
                <uc1:KYTDateTimePicker runat="server" ID="dateStart" AutoPostBack="true" OnTextChanged="btnSEARCH_Click"/>
                ~ 
                <uc1:KYTDateTimePicker runat="server" ID="dateEnd" AutoPostBack="true" OnTextChanged="btnSEARCH_Click"/>

                </div>
            </div>
            <div class="row only-col1">
                <div class="col-md-12 center">
                    <asp:Button ID="btnSEARCH" runat="server" Text="查詢" Style="padding: 5px 20px; margin: 5px;" class="btn btn-primary btn-sm" Visible="false" OnClick="btnSEARCH_Click" />
                </div>
            </div>

            <asp:GridView ID="gvMain" CssClass="grid" runat="server" Width="100%" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" ForeColor="#333333" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvMain_PageIndexChanging">
                <PagerStyle CssClass="GridPager" />
                <AlternatingRowStyle BackColor="#ffffcc" />
                <Columns>
                    <asp:TemplateField HeaderText="選擇" HeaderStyle-Width="1%" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnGet" runat="server" OnClick="lbtnGet_Click">選擇</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="表單單號" HeaderStyle-Width="1%" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDOC_NBR" ViewType="ReadOnly" FieldName="DOC_NBR" Text='<%#Bind("DOC_NBR")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="日期時間(起)" HeaderStyle-Width="1%" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSTART_TIME" ViewType="ReadOnly" FieldName="START_TIME" Text='<%#Bind("START_TIME")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="日期時間(迄)" HeaderStyle-Width="1%" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblEND_TIME" ViewType="ReadOnly" FieldName="END_TIME" Text='<%#Bind("END_TIME")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div>
                <asp:Label runat="server" ID="lblMSG" ForeColor="Red"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
