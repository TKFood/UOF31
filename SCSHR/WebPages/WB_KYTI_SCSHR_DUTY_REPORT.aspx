<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WB_KYTI_SCSHR_DUTY_REPORT.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="WB_KYTI_SCSHR_DUTY_REPORT" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%@ Register Src="~/Common/ChoiceCenter/UC_ChoiceList.ascx" TagPrefix="uc1" TagName="UC_ChoiceList" %>
    <%@ Register Src="~/Common/ChoiceCenter/UC_ChoiceListMobile.ascx" TagPrefix="uc1" TagName="UC_ChoiceListMobile" %>
    <%@ Register Src="~/CDS/SCSHR/WKFFields/QUERYWINDOWS/_UC_SearchGroupWithGroup.ascx" TagPrefix="uc1" TagName="_UC_SearchGroupWithGroup" %>


    <link href="<%=Page.ResolveUrl("~/CDS/SCSHR/Assets/css/SCSHR.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>
    <meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;">

    <style>
        .hidden {
            display: none;
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

        div.selectHidden > div > div {
            display: none;
        }

        html {
            overflow: hidden;
        }

        .limitHeight {
            overflow: auto;
        }

        div.showMOBILE {
            display: none;
        }

        div.showPC {
            display: block;
        }

        @media screen and (max-width: 768px) {
            div.showMOBILE {
                display: block;
            }

            div.showPC {
                display: none;
            }
        }

        ul.TokensContainer {
            height: 100% !important;
        }
    </style>
    <script>
        $(document).ready(function () {
            SetGvItems();
        });

        function SetGvItems() {
            if ($(".multipleHeader").length > 0) {
                $("<tr><th scope='col' rowspan='3'>員工工號</th><th scope='col' rowspan='3'>員工姓名</th><th scope='col' rowspan='3'>部門編號</th><th scope='col' rowspan='3'>部門名稱</th><th scope='col' rowspan='3'>出勤日期</th><th scope='col' rowspan='3'>出勤班別代碼</th><th scope='col' rowspan='3'>出勤班別</th><th scope='col' rowspan='3'>刷卡卡號</th><th scope='col' colspan='5'>班前(A)</th><th scope='col' colspan='6'>班內(B)</th><th scope='col' colspan='5'>班後(C)</th><th scope='col' rowspan='3'>缺勤時數</th><th scope='col' rowspan='3'>請假時數</th><th scope='col' rowspan='3'>簽核中的請假時數</th></tr><tr><th scope='col' colspan='2'>加班上班(A1)</th><th scope='col' colspan='2'>加班下班(A2)</th><th scope='col'>加班資訊(A)</th><th scope='col' colspan='3'>上班(B1)</th><th scope='col' colspan='3'>下班(B2)</th><th scope='col' colspan='2'>加班上班(C1)</th><th scope='col' colspan='2'>加班下班(C2)</th><th scope='col'>加班資訊(C)</th></tr><tr><th scope='col'>出勤時間(A1)</th><th scope='col'>出勤狀態(A1)</th><th scope='col'>出勤時間(A2)</th><th scope='col'>出勤狀態(A2)</th><th scope='col'>加班狀態(A)</th><th scope='col'>出勤時間(B1)</th><th scope='col'>出勤狀態(B1)</th><th scope='col'>處理狀態(B1)</th><th scope='col'>出勤時間(B2)</th><th scope='col'>出勤狀態(B2)</th><th scope='col'>處理狀態(B2)</th><th scope='col'>出勤時間(C1)</th><th scope='col'>出勤狀態(C1)</th><th scope='col'>出勤時間(C2)</th><th scope='col'>出勤狀態(C2)</th><th scope='col'>加班狀態(C)</th></tr>").insertAfter(".multipleHeader");
                $(".multipleHeader").remove();
            }
            LimitHeight();
        }
        function LimitHeight() {
            $(".limitHeight").css("height", "calc(100vh - " + $("#MasterHeader").css("height") + " - " + $("#MasterFooter").css("height"));
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnDLExcel" />
    </Triggers>
        <ContentTemplate>

            <div class="col-md-12 limitHeight" style="width: 100vw;">
                <div class="row">
                    <div class="col-md-12 divheader2">
                        個人出缺勤狀況查詢
                    </div>
                </div>
                <div class="row" style="margin-top: 12px;">
                    <div class="col-sm-2" style="flex-basis: 150px;">
                        開始時間:
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadDateTimePicker runat="server" ID="kdtpSTARTTIME" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2" style="flex-basis: 150px;">
                        結束時間:
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadDateTimePicker runat="server" ID="kdtpENDTIME" />
                    </div>
                </div>
                <div class="row" style="align-items: center;">
                    <div class="col-md-2" style="flex-basis: 150px;">
                        帳號:
                    </div>
                    <div style="margin: 0 2px; padding: 0 15px;">
                        <asp:Label runat="server" ID="lblAccount" />
                    </div>
                    <uc1:_UC_SearchGroupWithGroup runat="server" ID="_UC_SearchGroupWithGroup" Visible="false" OnDialogReturn="_UC_SearchGroupWithGroup_DialogReturn" />
                </div>
                <div class="row">
                    <div class="col-sm-2" style="flex-basis: 150px;">
                        計算缺勤時數:
                    </div>
                    <div class="col-sm-10">
                        <asp:DropDownList runat="server" ID="ddlHours">
                            <asp:ListItem Value="0">不計算</asp:ListItem>
                            <asp:ListItem Value="1">計算</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2" style="flex-basis: 150px;">
                        納入出勤異常資料:
                    </div>
                    <div class="col-sm-10">
                        <asp:DropDownList runat="server" ID="ddlAbnorma">
                            <asp:ListItem Value="0">不納入</asp:ListItem>
                            <asp:ListItem Value="1">納入</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2" style="margin: 12px 0;">
                        <asp:Button runat="server" ID="btnSearch" Text="查詢" OnClick="btnSearch_Click" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 divheader2">
                        查詢結果
                        <asp:Button ID="btnDLExcel" runat="server" Text="下載" class="btn btn-success btn-sm" OnClick="btnDLExcel_Click" CausesValidation="false" />
                    </div>
                </div>
                <div class="row">
                    <!--gvItems-->
                    <div class="col-md-12 GridViewNormal">
                        <asp:GridView runat="server" ID="gvItems"
                            CssClass="tsGridView2 SGridView horzFull"
                            AutoGenerateColumns="false"
                            ShowHeader="true"
                            ShowHeaderWhenEmpty="false"
                            HeaderStyle-CssClass="multipleHeader"
                            OnRowDataBound="gvItems_RowDataBound">
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                <asp:Label ID="lblTip" runat="server" ForeColor="Red" Font-Bold="true" Text="無個人出缺勤資料"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="員工工號">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblEMPLOYEEVIEWID" Text='<%#Bind("EMPLOYEEVIEWID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="員工姓名">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblEMPLOYEENAME" Text='<%#Bind("EMPLOYEENAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="部門編號">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDEPARTVIEWID" Text='<%#Bind("DEPARTVIEWID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="部門名稱">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDEPARTNAME" Text='<%#Bind("DEPARTNAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤日期">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblATTENDDATE" Text='<%#Bind("ATTENDDATE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤班別代碼">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblTMP_WORKID" Text='<%#Bind("TMP_WORKID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤班別">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblTMP_WORKNAME" Text='<%#Bind("TMP_WORKNAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="刷卡卡號">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCARDNO" Text='<%#Bind("CARDNO")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤時間(A1)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblBOVERTIME" Text='<%#Bind("BOVERTIME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤狀態(A1)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblBOVERTIMESTATUS" />
                                        <asp:HiddenField runat="server" ID="hidBOVERTIMESTATUS" Value='<%#Bind("BOVERTIMESTATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤時間(A2)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblBOFFOVERTIME" Text='<%#Bind("BOFFOVERTIME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤狀態(A2)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblBOFFOVERTIMESTATUS" />
                                        <asp:HiddenField runat="server" ID="hidBOFFOVERTIMESTATUS" Value='<%#Bind("BOFFOVERTIMESTATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="加班狀態(A)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblBSTATUS" />
                                        <asp:HiddenField runat="server" ID="hidBSTATUS" Value='<%#Bind("BSTATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤時間(B1)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblWORKTIME" Text='<%#Bind("WORKTIME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤狀態(B1)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblWORKTIMESTATUS" />
                                        <asp:HiddenField runat="server" ID="hidWORKTIMESTATUS" Value='<%#Bind("WORKTIMESTATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="處理狀態(B1)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSTATUS" />
                                        <asp:HiddenField runat="server" ID="hidSTATUS" Value='<%#Bind("STATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤時間(B2)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOFFWORKTIME" Text='<%#Bind("OFFWORKTIME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤狀態(B2)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOFFWORKTIMESTATUS" />
                                        <asp:HiddenField runat="server" ID="hidOFFWORKTIMESTATUS" Value='<%#Bind("OFFWORKTIMESTATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="處理狀態(B2)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSTATUS2" />
                                        <asp:HiddenField runat="server" ID="hidSTATUS2" Value='<%#Bind("STATUS2")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤時間(C1)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblAOVERTIME" Text='<%#Bind("AOVERTIME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤狀態(C1)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblAOVERTIMESTATUS" />
                                        <asp:HiddenField runat="server" ID="hidAOVERTIMESTATUS" Value='<%#Bind("AOVERTIMESTATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤時間(C2)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblAOFFOVERTIME" Text='<%#Bind("AOFFOVERTIME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤狀態(C2)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblAOFFOVERTIMESTATUS" />
                                        <asp:HiddenField runat="server" ID="hidAOFFOVERTIMESTATUS" Value='<%#Bind("AOFFOVERTIMESTATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="加班狀態(C)">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblASTATUS" />
                                        <asp:HiddenField runat="server" ID="hidASTATUS" Value='<%#Bind("ASTATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="缺勤時數">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCHOURS" Text='<%#Bind("CHOURS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="請假時數">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblLHOURS" Text='<%#Bind("LHOURS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="簽核中的請假時數">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblFHOURS" Text='<%#Bind("FHOURS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <!--gvItems-->
                </div>
            </div>
            <asp:Label ID="CustomValidator1" runat="server" CssClass="Error_message" Font-Bold="true" ForeColor="Red" Display="Dynamic" Text=""></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
