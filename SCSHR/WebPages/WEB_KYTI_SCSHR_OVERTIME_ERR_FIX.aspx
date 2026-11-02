<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WEB_KYTI_SCSHR_OVERTIME_ERR_FIX.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_SCSHR_WebPages_WEB_KYTI_SCSHR_OVERTIME_ERR_FIX" %>
<%@ Register Src="~/Common/ChoiceCenter/UC_BtnChoiceOnce.ascx" TagPrefix="uc1" TagName="UC_BtnChoiceOnce" %>
<%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>
    <link href="<%=Page.ResolveUrl("~/CDS/SCSHR/Assets/css/SCSHR.css")%>" rel="stylesheet" />
    <!-- 引用bootstrap -->
    <link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>
    <style>
        .hidden {
            display: none;
        }

        div.RowNormal:first-child {
            margin-top: 12px;
        }

        div.RowNormal:not(:first-child) {
            margin-top: 5px;
        }

        div.RowNormal > div:first-child {
            flex-basis: 135px;
        }

        div.RowNormal > div:not(:first-child) {
            padding: 0;
        }

        div.RowNormal.special {
            margin-top: -2px;
        }

            div.RowNormal.special > div:first-child {
                margin-top: 5px;
            }

            div.RowNormal.special > div:nth-child(2) > span {
                margin-top: 5px;
            }

        div.inputBorder .RadPicker input {
            border-color: #A9A9A9;
        }

        div.GridViewNormal {
            padding: 0;
            overflow-x: inherit;
        }

            div.GridViewNormal td {
                background-color: white;
            }

            div.GridViewNormal table tr > th {
                text-align: center;
            }

            div.GridViewNormal table > tbody > tr:nth-child(odd) > td {
                background-color: #ffffff;
            }

            div.GridViewNormal table > tbody > tr:nth-child(even) > td {
                background-color: #f7ffe7;
            }

        div.selectHidden > div > div {
            display: none;
        }

        html {
            overflow: hidden;
        }

        .limitHeight {
            overflow: auto;
        }
    </style>
    <script>
        $(document).ready(function () {
            LimitHeight();
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(LimitHeight);
        function LimitHeight() {
            $(".limitHeight").css("height", "calc(100vh - " + $("#MasterHeader").css("height") + " - " + $("#MasterFooter").css("height"));
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="col-md-12 limitHeight" style="width: 100vw;">
                <div class="row">
                    <div class="col-md-12 divheader2">
                        加班單處理
                    </div>
                </div>
                <div class="row RowNormal">
                    <div class="col-sm-2">
                        加班單號:
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtDOC_NBR" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row RowNormal">
                    <div class="col-sm-2">
                        加班單申請時間:
                    </div>
                    <div class="col-sm-10 inputBorder">
                        <telerik:RadDateTimePicker runat="server" ID="dtpStart" />
                        ~
                        <telerik:RadDateTimePicker runat="server" ID="dtpEnd" />
                    </div>
                </div>
                <div class="row RowNormal special">
                    <div class="col-sm-2">
                        申請人:
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtAPPLICANT" runat="server" ReadOnly="true"></asp:TextBox>
                        <asp:HiddenField ID="hidAPPLICANTGUID" runat="server" />
                        <uc1:uc_btnchoiceonce runat="server" id="bcoMan" buttontext="選人" choiceoncekind="Employee" oneditbuttononclick="bcoMan_EditButtonOnClick" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" style="margin: 12px 0;">
                        <asp:Button ID="btnSearch" runat="server" Text="搜尋" OnClick="btnSearch_Click" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" style="margin: 12px 0;">
                        <asp:Button ID="btnReCalcByBOFind" runat="server" Text="同步BOFind" OnClick="btnReCalcByBOFind_Click" />
                        <asp:Button ID="btnReCalc" runat="server" Text="重新詢問飛騰計算" OnClick="btnReCalc_Click" />
                        <asp:Button ID="btnPartReBuild" runat="server" Text="失敗單號重轉" OnClick="btnPartReBuild_Click" />
                        <asp:Button ID="btnReBuild" runat="server" Text="重轉" OnClick="btnReBuild_Click" />
                    </div>
                </div>
                <div>
                    <div class="row">
                        <div class="col-md-12 GridViewNormal">
                            <asp:GridView runat="server" ID="gvOVH"
                                CssClass="tsGridView2 horzFull"
                                AutoGenerateColumns="false"
                                ShowHeader="true"
                                ShowHeaderWhenEmpty="false"
                                OnRowDataBound="gvOVH_RowDataBound">
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <asp:Label ID="Label01" runat="server" ForeColor="Red" Font-Bold="true" Text="目前沒有明細資料"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                     <asp:TemplateField HeaderText="重新計算時數狀態">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRECALC_STATUS" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="中繼表更新狀態">
                                        <ItemTemplate>
                                            <asp:Label ID="lblZTABLE_STATUS" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="飛騰更新狀態">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWS_STATUS" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="加班單單單號">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDOC_NBR" runat="server" Text='<%#Bind("DOC_NBR")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="申請人">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAPPLICANT_NAME" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="加班折換方式">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCHANGETYPE" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="加班時間(起)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOT_START" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="加班時間(迄)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOT_END" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="訊息">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMSG" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
