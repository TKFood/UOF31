<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_PRODUCTSDialogADD.aspx.cs" Inherits="CDS_WebPage_TBBU_PRODUCTSDialogADD" %>

<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

    <script>
        $(function () {

        });
        function validateInput(event) {
            var char = String.fromCharCode(event.which);
            var textbox = event.target;

            if (!/[0-9.]/.test(char)) {
                event.preventDefault();
                return false;
            }

            // Allow only one decimal point
            if (char === '.' && textbox.value.includes('.')) {
                event.preventDefault();
                return false;
            }

            return true;
        }

    </script>
    <div style="overflow-x: auto; width: 100%">
    </div>
    <table class="PopTable">
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label1" runat="server" Text="CDS_WebPage_TBBU_PRODUCTSDialogADD"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan="2">
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>
        </tr>

    </table>
    <table>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label2" runat="server" Text="品號"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server" Text="" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label3" runat="server" Text="銷售重點"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server" Text=" " Width="200%" TextMode="MultiLine" Row="5" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label5" runat="server" Text="銷售通路"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server" Text=" " Width="200%" TextMode="MultiLine" Row="5" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label4" runat="server" Text="建議售價"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server" Text="0" Width="200%" TextMode="MultiLine" Row="5" Style="height: 120px;" OnKeyPress="return validateInput(event);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label6" runat="server" Text="IP價"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server" Text="0" Width="200%" TextMode="MultiLine" Row="5" Style="height: 120px; " OnKeyPress="return validateInput(event);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label7" runat="server" Text="DM價"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server" Text="0" Width="200%" TextMode="MultiLine" Row="5" Style="height: 120px;" OnKeyPress="return validateInput(event);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label9" runat="server" Text="實際MOQ"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox7" runat="server" Text="0" Width="200%" TextMode="MultiLine" Row="1" Style="height: 120px;" OnKeyPress="return validateInput(event);"></asp:TextBox>
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

