<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchAllKYTILogsV2.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_SearchAllKYTILogsV2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
            <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
            <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
            <link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/KYTI.css")%>" rel="stylesheet" />
            <link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
            <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
            <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>

            <style>
                /*設定div樣式的整體佈局*/
                .pageno-icon {
                    margin: 20px; /*設定距離頂部20畫素*/
                    font-size: 0; /*修復行內元素之間空隙間隔*/
                    text-align: center; /*設定內容居中顯示*/
                }

                    /*設定共有的的樣式佈局，主要是進行程式碼優化，提高執行效率*/
                    .pageno-icon a, .pageno-disabled {
                        border: 1px solid #ccc;
                        border-radius: 3px;
                        padding: 4px 10px 5px;
                        font-size: 14PX; /*修復行內元素之間空隙間隔*/
                        margin-right: 10px;
                    }


                    /*對 a 標籤進行樣式佈局 */
                    .pageno-icon a {
                        text-decoration: none; /*取消連結的下劃線*/
                        color: #005aa0;
                    }

                .pageno-next {
                    background-color: rgba(239, 130, 255, 0.50);
                }

                .pageno-current {
                    background-color: rgba(237, 74, 34, 0.30)
                }

                .pageno-disabled {
                    color: #ccc;
                }

                    .pageno-next i, .pageno-disabled i {
                        display: inline-block; /*設定顯示的方式為行內塊元素*/
                        width: 5px;
                        height: 9px;
                    }

                    .pageno-disabled i {
                        background-position: -80px -608px;
                        margin-right: 3px;
                    }

                .pageno-next i {
                    background-position: -62px -608px;
                    margin-left: 3px;
                    background-color: brown;
                }

                .filterTarger {
                    color: orange;
                    background-color: yellow;
                }

                .msgResult {
                    width: 600px;
                    height: 250px;
                    padding: 10px;
                    background-color: #444;
                    color: white;
                    font-size: 14px;
                    font-family: monospace;
                    overflow: scroll;
                }
            </style>
            <script>
                function MarkEachFilter() {
                    var txtFilter = $("#<%=txtFilter.ClientID%>");
                    debugger;
                    if (txtFilter.val() != "") {
                        var searchTexts = txtFilter.val().split("+");

                        searchTexts.forEach(
                            searchText => {
                                var content = $("#<%=gvLogs.ClientID%>").html();
                                var regExp = new RegExp(searchText, 'g');
                                var newHtml = content.replace(regExp, "<span class='filterTarger'>" + searchText + "&nbsp;</span>");
                                $("#<%=gvLogs.ClientID%>").html(newHtml);
                            }
                        );
                    }
                }
               
            </script>

            <div style="padding: 0px 10px 0px 20px; max-width: 98vw;">
                <div class="row">
                    <div class="col-md-12 divheader2">
                        <%=Page_NAME %>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 divheader">
                        過濾條件
                    </div>
                </div>
                <div>
                    <div class="row">
                        <div class="col-md-2">
                            時間區間
                        </div>
                        <div class="col-md-10" style="display: inline-block">
                            <input runat="server" id="startDateTime" type="datetime-local" step="1">&nbsp;~&nbsp;<input runat="server" id="endDateTime" type="datetime-local" step="1">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            Log等級
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlLogLevel" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            檔案名稱
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlFileName" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            NameSpace
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlNamespace" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            方法名稱
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlMethodName" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            訊息區間
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtKYTTaskStartMsg" runat="server" placeholder="查詢排程區間訊息-開頭" Width="500px"></asp:TextBox>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtKYTTaskEndMsg" runat="server" placeholder="查詢排程區間訊息-結尾" Width="500px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:TextBox ID="txtFilter" runat="server" placeholder="內容關鍵字查詢(用「+」連結就意味著要同時擁有複數條件)" Width="500px"></asp:TextBox>
                            <asp:Button ID="btnFilter" runat="server" Text="依條件查詢" OnClick="btnFilter_Click" CssClass="btn btn-primary btn-sm" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 divheader">
                            查詢結果
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                <asp:ListItem Value="5">每頁 5 筆</asp:ListItem>
                                <asp:ListItem Value="10">每頁 10 筆</asp:ListItem>
                                <asp:ListItem Value="15">每頁 15 筆</asp:ListItem>
                                <asp:ListItem Value="20">每頁 20 筆</asp:ListItem>
                                <asp:ListItem Value="25" Selected="True">每頁 25 筆</asp:ListItem>
                                <asp:ListItem Value="30">每頁 30 筆</asp:ListItem>
                                <asp:ListItem Value="40">每頁 40 筆</asp:ListItem>
                                <asp:ListItem Value="50">每頁 50 筆</asp:ListItem>
                                <asp:ListItem Value="100">每頁 100 筆</asp:ListItem>
                                <asp:ListItem Value="150">每頁 150 筆</asp:ListItem>
                                <asp:ListItem Value="200">每頁 200 筆</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;
                            找到資料筆數為：&nbsp;<asp:Label ID="lblSearchCount" runat="server" Text="0" ForeColor="Red"></asp:Label>&nbsp;筆
                        </div>
                    </div>
                </div>
            </div>
            <div style="padding: 0px 10px 0px 20px; max-width: 98vw;">
                <div class="row">
                    <div class="pageno-icon">
                        <asp:Repeater ID="rptPages" runat="server" OnItemDataBound="rptPages_ItemDataBound">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lbtnTop" runat="server" OnClick="lbtnTop_Click" CssClass="pageno-next">1</asp:LinkButton>
                                <asp:LinkButton ID="lbtnPrev" runat="server" OnClick="lbtnPrev_Click" CssClass="pageno-next">上一頁</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnCurrent" runat="server" OnClick="lbtnCurrent_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lbtnNext_Click" CssClass="pageno-next">下一頁</asp:LinkButton>
                                <asp:LinkButton ID="lbtnBottom" runat="server" OnClick="lbtnBottom_Click" CssClass="pageno-next"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <div>
                <asp:GridView runat="server" ID="gvLogs"
                    AutoGenerateColumns="false"
                    CssClass="tsGridView2"
                    ShowHeader="true"
                    ShowHeaderWhenEmpty="false"
                    OnRowDataBound="gvLogs_RowDataBound">
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
                                <%--<asp:TextBox runat="server" ID="txtValues" TextMode="MultiLine" ReadOnly="true" Rows="10" Width="100%"></asp:TextBox>--%>
                                <div runat="server" id="divValues" class="msgResult"></div>
                                <%--<asp:Label ID="lblValues" runat="server" Text=""></asp:Label>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div style="padding: 0px 10px 0px 20px; max-width: 98vw;">
                <div class="row">
                    <div class="pageno-icon">
                        <asp:Repeater ID="rptPagesBottom" runat="server" OnItemDataBound="rptPages_ItemDataBound">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lbtnTop" runat="server" OnClick="lbtnTop_Click" CssClass="pageno-next">1</asp:LinkButton>
                                <asp:LinkButton ID="lbtnPrev" runat="server" OnClick="lbtnPrev_Click" CssClass="pageno-next">上一頁</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCurrentPrev" runat="server" Text="...."></asp:Label>
                                <asp:LinkButton ID="lbtnCurrent" runat="server" OnClick="lbtnCurrent_Click"></asp:LinkButton>
                                <asp:Label ID="lblCurrentNext" runat="server" Text="...."></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lbtnNext_Click" CssClass="pageno-next">下一頁</asp:LinkButton>
                                <asp:LinkButton ID="lbtnBottom" runat="server" OnClick="lbtnBottom_Click" CssClass="pageno-next"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
