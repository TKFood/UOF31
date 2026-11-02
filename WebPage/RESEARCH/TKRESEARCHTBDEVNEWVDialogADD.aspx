<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TKRESEARCHTBDEVNEWVDialogADD.aspx.cs" Inherits="CDS_WebPage_TKRESEARCHTBDEVNEWVDialogADD" %>

<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

    <script>
        $(function () {


        });
    </script>

    <table class="PopTable">
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label1" runat="server" Text="CDS_WebPage_TKRESEARCHTBDEVNEWVDialogADD"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan="2">
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label8" runat="server" Text="狀態"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:DropDownList ID="DropDownList1" runat="server" Width="100%"></asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label3" runat="server" Text="產品品項"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server" Text="" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label2" runat="server" Text="樣品試作/試吃結果"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server" Text="" Width="100%"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label4" runat="server" Text="口味確認"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <%--<asp:TextBox ID="TextBox3" runat="server" Text="" Width="100%"></asp:TextBox>--%>
                <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label5" runat="server" Text="包裝型式重量"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server" Text="" Width="100%"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label6" runat="server" Text="可行性"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <%--<asp:TextBox ID="TextBox5" runat="server" Text="" Width="100%"></asp:TextBox>--%>
                <asp:DropDownList ID="DropDownList3" runat="server"></asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label7" runat="server" Text="設計需求"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <%--<asp:TextBox ID="TextBox6" runat="server" Text="" Width="100%"></asp:TextBox>--%>
                <asp:DropDownList ID="DropDownList4" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label18" runat="server" Text="設計需求日期  D+7"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server" Text="" Width="100%"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label9" runat="server" Text="成本試算"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <%--<asp:TextBox ID="TextBox7" runat="server" Text="" Width="100%"></asp:TextBox>--%>
                <asp:DropDownList ID="DropDownList5" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label19" runat="server" Text="成本試算日期  D+3"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server" Text="" Width="100%"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label10" runat="server" Text="校稿完成"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <%--<asp:TextBox ID="TextBox8" runat="server" Text="" Width="100%"></asp:TextBox>--%>
                <asp:DropDownList ID="DropDownList6" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label20" runat="server" Text="校稿完成日期  D+7"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server" Text="" Width="100%"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label11" runat="server" Text="試量產"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <%--<asp:TextBox ID="TextBox9" runat="server" Text="" Width="100%"></asp:TextBox>--%>
                <asp:DropDownList ID="DropDownList7" runat="server"></asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label12" runat="server" Text="正式量產"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <%--<asp:TextBox ID="TextBox10" runat="server" Text="" Width="100%"></asp:TextBox>--%>
                <asp:DropDownList ID="DropDownList8" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label16" runat="server" Text="原料驗收作業"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <%--<asp:TextBox ID="TextBox10" runat="server" Text="" Width="100%"></asp:TextBox>--%>
                <asp:DropDownList ID="DropDownList9" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label21" runat="server" Text="原料驗收日期 D+5"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox7" runat="server" Text="" Width="100%"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label17" runat="server" Text="營養標示作業"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <%--<asp:TextBox ID="TextBox10" runat="server" Text="" Width="100%"></asp:TextBox>--%>
                <asp:DropDownList ID="DropDownList10" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label22" runat="server" Text="營養標示日期 D+2"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox8" runat="server" Text="" Width="100%"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label13" runat="server" Text="負責業務"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox11" runat="server" Text="" Width="100%"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label14" runat="server" Text="備註"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox12" runat="server" Text="" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label15" runat="server" Text="結案日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox13" runat="server" Text="" Width="100%"></asp:TextBox>
            </td>
        </tr>


    </table>


</asp:Content>

