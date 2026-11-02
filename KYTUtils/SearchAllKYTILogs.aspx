<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchAllKYTILogs.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_SearchAllKYTILogs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/KYTI.css")%>" rel="stylesheet" />
            <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
            <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
            <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
            <style>
            </style>
            <div>
                <div>取得Config內設定的LOG檔案儲存位置</div>
                <div>
                    <asp:DropDownList ID="ddlAllDDLConfig" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAllDDLConfig_SelectedIndexChanged"></asp:DropDownList>
                    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Font-Size="XX-Large"></asp:Label>
                </div>
            </div>
            <div>
                <div>選擇LOG日期</div>
                <div>
                    <%--<uc1:KYTDatePicker runat="server" ID="kytdpLocation" ViewType="Input" AutoPostBack="true" OnTextChanged="kytdpLocation_TextChanged" />--%>
                    <telerik:RadDatePicker runat="server" ID="rdpLocation" AutoPostBack="true" OnSelectedDateChanged="rdpLocation_SelectedDateChanged"></telerik:RadDatePicker>
                </div>
                <div>
                    <asp:DropDownList ID="ddlAllFiles" runat="server"></asp:DropDownList>
                    <asp:Button ID="btnRefresh" runat="server" Text="重整檔案清單" OnClick="btnRefresh_Click" />
                    <asp:Button ID="btnRead" runat="server" Text="讀取檔案" OnClick="btnRead_Click" />
                </div>
            </div>
            <div runat="server" id="divFliter">
                <div>過濾條件</div>
                <div>
                    <div runat="server" id="divTimeInterval">
                        時間區間：
                            <asp:TextBox ID="txtStartTime" runat="server"></asp:TextBox>~
                            <asp:TextBox ID="txtEndTime" runat="server"></asp:TextBox>
                    </div>
                    <div runat="server" id="divLogLevel">
                        Log等級：<asp:DropDownList ID="ddlLogLevel" runat="server"></asp:DropDownList>
                    </div>
                    <div runat="server" id="divFileName">
                        檔案名稱：<asp:DropDownList ID="ddlFileName" runat="server"></asp:DropDownList>
                    </div>
                    <div runat="server" id="divNameSpace">
                        NameSpace：<asp:DropDownList ID="ddlNamespace" runat="server"></asp:DropDownList>
                    </div>
                    <div runat="server" id="divMethod">
                        方法名稱：<asp:DropDownList ID="ddlMethodName" runat="server"></asp:DropDownList>
                    </div>
                    <div runat="server" id="divSearch">
                        <asp:TextBox ID="txtFliter" runat="server" placeholder="內容關鍵字查詢(用「+」連結就意味著要同時擁有複數條件)" Width="500px"></asp:TextBox>
                        <asp:Button ID="btnFliter" runat="server" Text="依條件查詢" OnClick="btnFliter_Click" />
                    </div>
                    <div>
                        找到資料筆數為：<asp:Label ID="lblSearchCount" runat="server" Text="" ForeColor="Red"></asp:Label>
                        筆
                    </div>
                </div>
            </div>
            <div>
                <asp:GridView runat="server" ID="gvLogs"
                    AutoGenerateColumns="false"
                    CssClass="tsGridView2"
                    ShowHeader="true"
                    ShowHeaderWhenEmpty="false"
                    OnRowDataBound="gvLogs_RowDataBound"
                    AllowPaging="True" PageSize="20"
                    OnPageIndexChanging="gvLogs_PageIndexChanging">
                    <PagerSettings Mode="NumericFirstLast"
                        Position="TopAndBottom" />
                    <PagerStyle HorizontalAlign="Center" Font-Size="20" Font-Italic="False" Font-Strikeout="False" Font-Underline="False" ForeColor="Black" />
                    <PagerTemplate>
                        <asp:LinkButton ID="lbnFirst" runat="Server" Text="第一頁" Visible='<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>' CommandName="Page" CommandArgument="First"></asp:LinkButton>
                        <asp:LinkButton ID="lbnPrev" runat="server" Text="上一頁" Visible='<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>' CommandName="Page" CommandArgument="Prev"></asp:LinkButton>
                        <asp:LinkButton ID="lbnNext" runat="Server" Text="下一頁" Visible='<%# ((GridView)Container.NamingContainer).PageIndex != (((GridView)Container.NamingContainer).PageCount - 1) %>' CommandName="Page" CommandArgument="Next"></asp:LinkButton>
                        <asp:LinkButton ID="lbnLast" runat="Server" Text="最末頁" Visible='<%# ((GridView)Container.NamingContainer).PageIndex != (((GridView)Container.NamingContainer).PageCount - 1) %>' CommandName="Page" CommandArgument="Last"></asp:LinkButton>
                        <asp:Label ID="lblPage"  runat="server" Text='<%# "第" + (((GridView)Container.NamingContainer).PageIndex + 1)  + "頁/共" + (((GridView)Container.NamingContainer).PageCount) + "頁" %> '></asp:Label>
                    </PagerTemplate>

                    <EmptyDataRowStyle HorizontalAlign="Center" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Bold="true" Text="目前沒有查到任何相關的Log資料"></asp:Label>
                    </EmptyDataTemplate>
                    <AlternatingRowStyle BackColor="#f5f5f5" />
                    <Columns>
                        <asp:TemplateField HeaderText="序列號" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNo" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="時間" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblDateTime" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LOG等級" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblLevel" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="檔案名稱" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NameSpace" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblNameSpace" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="方法名稱" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblMethodName" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="行號" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblLineNumber" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="內容" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50%">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtValues" TextMode="MultiLine" ReadOnly="true" Rows="10" Width="100%"></asp:TextBox>
                                <%--<asp:Label ID="lblValues" runat="server" Text=""></asp:Label>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
