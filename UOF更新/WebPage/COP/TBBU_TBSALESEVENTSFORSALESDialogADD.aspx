<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_TBSALESEVENTSFORSALESDialogADD.aspx.cs" Inherits="CDS_WebPage_TBBU_TBSALESEVENTSFORSALESDialogADD" %>

<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

    <script>
        $(function () {

        });

    </script>
    <div style="overflow-x: auto; width: 100%">
    </div>
    <table class="PopTable">
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label1" runat="server" Text="CDS_WebPage_TBBU_TBSALESEVENTSDialogADD"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan="2">
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>
        </tr>

    </table>
    <table>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label4" runat="server" Text="業務員"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                <%--<asp:TextBox ID="TextBox1" runat="server" Text=""  Width="200%"  Row="1" Style="height: 20px;"></asp:TextBox>--%>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label5" runat="server" Text="類別"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
                <%--<asp:TextBox ID="TextBox2" runat="server" Text=""   Width="200%"  Row="1" Style="height: 20px;"></asp:TextBox>--%>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label2" runat="server" Text="客戶"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <telerik:RadAutoCompleteBox RenderMode="Lightweight" runat="server" ID="RadAutoCompleteBox1" Width="400" DropDownHeight="150"
                    EmptyMessage="請輸入名稱">
                    <WebServiceSettings Method="GetCompanyNames" Path="TBBU_TBSALESEVENTSDialogADD.aspx" />
                </telerik:RadAutoCompleteBox>
                <%--<asp:TextBox ID="TextBox1" runat="server" Text="" Width="100%" Style="height: 20px;"></asp:TextBox>--%>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label3" runat="server" Text="專案"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server" Text="" Width="200%" TextMode="MultiLine" Row="5" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label6" runat="server" Text="待辦事件"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server" Text="" Width="200%" TextMode="MultiLine" Row="5" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label7" runat="server" Text="起始日"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <telerik:RadDatePicker ID="RadDatePicker1" runat="server" Width="120px"></telerik:RadDatePicker>
                <%--<asp:TextBox ID="TextBox4" runat="server" Text="" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label9" runat="server" Text="結案日"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <telerik:RadDatePicker ID="RadDatePicker2" runat="server" Width="120px"></telerik:RadDatePicker>
                <%--<asp:TextBox ID="TextBox5" runat="server" Text="" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label10" runat="server" Text="進度內容"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server" Text="" Width="200%" TextMode="MultiLine" Row="5" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label11" runat="server" Text="是否結案"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:DropDownList ID="DropDownList3" runat="server" Enabled="false"></asp:DropDownList>
                <%--<asp:TextBox ID="TextBox7" runat="server" Text="N" Width="200%"  Style="height: 20px;"></asp:TextBox>--%>
            </td>
        </tr>


        
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label12" runat="server" Text="選擇圖片"></asp:Label>

            </td>
            <td>
                <asp:FileUpload ID="FileUpload" runat="server" />
                <asp:Label ID="LabelNAME" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label14" runat="server" Text=""></asp:Label>
                <asp:Label ID="LabelISSTATUS" runat="server" Text="N"></asp:Label>
                <%--<uc1:UC_FileCenter runat="server" ID="UC_FileCenter" ModuleName="CDS" SubFolder="FileCenter" OnClientUploaded="onClientUploaded" />--%>
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="上傳圖片" OnClick="btnSave_Click" />
            </td>
        </tr>

    </table>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>


</asp:Content>


