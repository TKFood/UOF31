<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FORPUNCH_UOF.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_FORPUNCH_UOF" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<%@ Register Src="~/Common/ChoiceCenter/UC_BtnChoiceOnce.ascx" TagPrefix="uc1" TagName="UC_BtnChoiceOnce" %>

<link href="<%=Page.ResolveUrl("~/CDS/KYTUtils/Assets/css/KYTI.css")%>" rel="stylesheet" />
<link href="<%=Page.ResolveUrl("~/CDS/bootstrap/css/bootstrap.min.css")%>" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/popper.min.js")%>"></script>
<script src="<%=Page.ResolveUrl("~/CDS/bootstrap/js/bootstrap.min.js")%>"></script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">

				<div class="row">
					<div class="col-md-4 divheader">
						補卡修正
					</div>
				</div>
				<div class="row">
					<div class="col-md-1 bg-light divtitle">
						帳號
					</div>
					<div class="col-md-3">
						<asp:TextBox ID="txtAccount" runat="server" Enabled="false"></asp:TextBox>
						<asp:HiddenField ID="hidGuid" runat="server" />
						<uc1:UC_BtnChoiceOnce runat="server" ID="bcoAccount" ButtonText="選擇" ChoiceOnceKind="Employee" OnEditButtonOnClick="bcoAccount_EditButtonOnClick"/>
					</div>
				</div>
				<div class="row">
					<div class="col-md-1 bg-light divtitle">
						歸屬日
					</div>
					<div class="col-md-3">
						<telerik:RadDatePicker runat="server" ID="rdpVESTING_DATE"></telerik:RadDatePicker>
					</div>
				</div>
				<div class="row">
					<div class="col-md-1 bg-light divtitle">
						上下班卡
					</div>
					<div class="col-md-3">
								<asp:DropDownList ID="ddlWORKOFF" runat="server">
									<asp:ListItem Value="WORK">修上班卡</asp:ListItem>
									<asp:ListItem Value="OFF">修下班卡</asp:ListItem>
								</asp:DropDownList>
					</div>
				</div>
				<div class="row">
					<div class="col-md-1 bg-light divtitle">
						時間
					</div>
					<div class="col-md-3">
								<telerik:RadDateTimePicker runat="server" ID="rdtpPUNCH_TIME" ></telerik:RadDateTimePicker>
					</div>
				</div>
				<div class="row">
					<div class="col-md-1 ">
						
					</div>
					<div class="col-md-3">
								<asp:Button class="btn btn-primary btn-sm" ID="lbtnModify" runat="server" Text="修正" OnClick="lbtnModify_Click" />
					</div>
				</div>


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
