<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModifyForm.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_TEST_WebPages_ModifyForm" %>

<%@ Register Src="~/Common/ChoiceCenter/UC_BtnChoiceOnce.ascx" TagPrefix="uc1" TagName="UC_BtnChoiceOnce" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%@ Register Src="~/KYTControl/KYTCheckBox.ascx" TagPrefix="uc1" TagName="KYTCheckBox" %>
    <%@ Register Src="~/KYTControl/KYTCheckBoxList.ascx" TagPrefix="uc1" TagName="KYTCheckBoxList" %>
    <%@ Register Src="~/KYTControl/KYTDatePicker.ascx" TagPrefix="uc1" TagName="KYTDatePicker" %>
    <%@ Register Src="~/KYTControl/KYTDateTimePicker.ascx" TagPrefix="uc1" TagName="KYTDateTimePicker" %>
    <%@ Register Src="~/KYTControl/KYTDropDownList.ascx" TagPrefix="uc1" TagName="KYTDropDownList" %>
    <%@ Register Src="~/KYTControl/KYTTextBox.ascx" TagPrefix="uc1" TagName="KYTTextBox" %>
    <%@ Register Src="~/KYTControl/KYTTimePicker.ascx" TagPrefix="uc1" TagName="KYTTimePicker" %>
    <%@ Register Src="~/KYTControl/KYTRadioButtonList.ascx" TagPrefix="uc1" TagName="KYTRadioButtonList" %>
    <%@ Register Src="~/KYTControl/KYTGridView.ascx" TagPrefix="uc1" TagName="KYTGridView" %>
    <%@ Register Src="~/CDS/TAIYUEN/WKFFields/QUERYWINDOW/_UC_SelectUserFilterGroup.ascx" TagPrefix="uc1" TagName="_UC_SelectUserFilterGroup" %>
    <%@ Register Src="~/CDS/TAIYUEN/WKFFields/QUERYWINDOW/_UC_SearchGroupWithGroup.ascx" TagPrefix="uc1" TagName="_UC_SearchGroupWithGroup" %>

    <link href="<%=Page.ResolveUrl("~/KYTControl/css/gemps.ui.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/KYTControl/css/font-awesome.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/KYTControl/js/gemps.ui.js")%>"></script>

    <!--引用bootstrap -->
    <link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>
    <link href="<%=Page.ResolveUrl("~/CDS/TAIYUEN/Assets/css/WebPage.css")%>" rel="stylesheet" />
    <style>
        div.tabletitle2 {
            background-color: #e8f2ff;
            background: -webkit-linear-gradient(#e8f2ff,#aacfff);
            background: -o-linear-gradient(#e8f2ff,#aacfff);
            background: -moz-linear-gradient(#e8f2ff,#aacfff);
            background: linear-gradient(#e8f2ff,#aacfff);
        }

            div.tabletitle2 > div:nth-child(1) {
                text-align: left;
            }

            div.tabletitle2 > div:nth-child(2) {
                text-align: center;
            }

            div.tabletitle2 > div:nth-child(3) {
                text-align: right;
            }

        table.custom tr > td {
            white-space: nowrap;
        }

            table.custom tr > td[colspan='10'] {
                padding-left: 10px;
            }

        .signtable {
            display: none !important;
        }

            .signtable td {
                width: 14.285%;
            }


        .bg-light {
            background-color: #d9e3ec !important;
        }

        .divtitle {
            border-width: 2px;
            border-color: white;
            padding: 1px;
            border-style: solid;
            border-radius: 3px;
            padding-left: 13px;
        }

        .Right {
            text-align: right !important;
        }

        .Left {
            text-align: left;
        }
    </style>
    <script>

</script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            <div class="container vertical-center limitHeight">
                <!--container-->
                <div class="row">
                    <div class="col-md-12 divheader2">
                        修改表單內容
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2 bg-light divtitle">
                        表單編號
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox runat="server" ID="ktxtDOC_NBR" OnTextChanged="ktxtDOC_NBR_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hidTaskID" />
                    </div>
                    <div class="col-md-2 bg-light divtitle">
                        表單FieldID
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTTextBox runat="server" ID="ktxtFieldID" />
                    </div>
                </div>
                <div class="row">
                    <asp:Button ID="btnGetForm" runat="server" class="btn btn-primary btn-sm" Text="取得表單欄位" OnClick="btnGetForm_Click" />
                </div>
                <div class="row">
                    <div class="col-md-2 bg-light divtitle">
                        表單欄位
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTDropDownList runat="server" ID="kddlItems" DataTextField="ITEMS" DataValueField="ITEMS" OnSelectedIndexChanged="kddlItems_SelectedIndexChanged"></uc1:KYTDropDownList>
                    </div>
                </div>
                <div class="row" style="display: none">
                    <div class="col-md-2 bg-light divtitle">
                        明細項次
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTDropDownList runat="server" ID="kddlRows" DataTextField="ITEMS" DataValueField="ITEMS"></uc1:KYTDropDownList>
                    </div>
                    <div class="col-md-2 bg-light divtitle">
                        欄位名稱
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTDropDownList runat="server" ID="kddlRowName" DataTextField="TXT" DataValueField="TXT"></uc1:KYTDropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2 bg-light divtitle">
                        原本值
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTTextBox runat="server" ID="ktxtValue_Ori" Rows="15" Width="100%" TextMode="MultiLine" />
                    </div>
                    <div class="col-md-2 bg-light divtitle">
                        更新值
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTTextBox runat="server" ID="ktxtValue" Rows="15" Width="100%" TextMode="MultiLine" />
                    </div>
                </div>
                <div class="row">
                    <asp:Button ID="btnUpdate" runat="server" class="btn btn-primary btn-sm" Text="更新表單" OnClick="btnUpdate_Click" />
                </div>
                <div class="row">
                    <div class="col-md-2 bg-light divtitle">
                        訊息
                    </div>
                    <div class="col-md-4">
                        <uc1:KYTTextBox runat="server" ID="ktxtMSG" ForeColor="Red" Width="100%" />
                    </div>
                </div>

                <!--gvItems-->
            </div>
            <!--container-fluid-->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
