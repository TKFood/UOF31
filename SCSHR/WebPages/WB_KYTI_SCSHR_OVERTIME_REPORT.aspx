<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WB_KYTI_SCSHR_OVERTIME_REPORT.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="WB_KYTI_SCSHR_OVERTIME_REPORT" %>



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
            LimitHeight();
        });
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
                        個人加班資料查詢
                    </div>
                </div>
                <div class="row" style="margin-top: 12px;">
                    <div class="col-sm-2" style="flex-basis: 100px;">
                        開始時間:
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadDateTimePicker runat="server" ID="kdtpSTARTTIME" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2" style="flex-basis: 100px;">
                        結束時間:
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadDateTimePicker runat="server" ID="kdtpENDTIME" />
                    </div>
                </div>
                <div class="row" style="align-items: center;">
                    <div class="col-md-2" style="flex-basis: 100px;">
                        帳號:
                    </div>
                    <div style="margin: 0 2px; padding: 0 15px;">
                        <asp:Label runat="server" ID="lblAccount" />
                    </div>
                    <uc1:_UC_SearchGroupWithGroup runat="server" ID="_UC_SearchGroupWithGroup" Visible="false" OnDialogReturn="_UC_SearchGroupWithGroup_DialogReturn" />
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
                <!--gvItems-->
                <div class="row">
                    <div class="col-md-12 GridViewNormal">
                        <asp:GridView runat="server" ID="gvItems"
                            CssClass="tsGridView2 SGridView horzFull"
                            AutoGenerateColumns="false"
                            ShowHeader="true"
                            ShowHeaderWhenEmpty="false"
                            OnRowDataBound="gvItems_RowDataBound">
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                <asp:Label ID="lblTip" runat="server" ForeColor="Red" Font-Bold="true" Text="無加班資料"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="單況">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSYS_FLOWFORMSTATUS" />
                                        <asp:HiddenField runat="server" ID="hidSYS_FLOWFORMSTATUS" Value='<%#Bind("SYS_FLOWFORMSTATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="加班單號">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOVERTIMEID" Text='<%#Bind("OVERTIMEID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="員工工號">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblEMPLOYEEID" Text='<%#Bind("EMPLOYEEID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="人員">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblEMPLOYEENAME" Text='<%#Bind("EMPLOYEENAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="申請日期">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDATE" Text='<%#Bind("DATE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="部門編號">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblTMP_DEPARTID" Text='<%#Bind("TMP_DEPARTID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="部門名稱">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDEPARTNAME" Text='<%#Bind("DEPARTNAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="加班班別">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblWORKNAME" Text='<%#Bind("WORKNAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="起始日期">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSTARTDATE" Text='<%#Bind("STARTDATE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="結束日期">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblENDDATE" Text='<%#Bind("ENDDATE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="加班時數">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOVERTIMEHOURS" Text='<%#Bind("OVERTIMEHOURS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="加班屬性">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCALCTYPE" Text='<%#Bind("CALCTYPE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="補休可休時數">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOVERCOMPHOURS" Text='<%#Bind("OVERCOMPHOURS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="補休已休時數">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblLEAVEHOURS" Text='<%#Bind("LEAVEHOURS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="補休結清時數">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCLOSEHOURS" Text='<%#Bind("CLOSEHOURS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="補休剩餘時數">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblROVERCOMPHOURS" Text='<%#Bind("ROVERCOMPHOURS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="補休可休期限">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMFREEDATE" Text='<%#Bind("MFREEDATE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="加班說明">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblNOTE" Text='<%#Bind("NOTE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出勤彙總單單號">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblTMP_ATTENDSUMID" Text='<%#Bind("TMP_ATTENDSUMID")%>' />
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
