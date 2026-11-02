<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TKRESEARCHTBSAMPLEDialogEDITDEL.aspx.cs" Inherits="CDS_WebPage_TKRESEARCHTBSAMPLEDialogEDITDEL" %>

<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/trontastic/jquery-ui.css">

    <script>
        $(function () {

        });

    </script>
    <div style="overflow-x: auto; width: 800px">
    </div>
    <table class="PopTable">
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label1" runat="server" Text="CDS_WebPage_TKRESEARCHTBSAMPLEDialogEDITDEL"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan="2">
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>
        </tr>

    </table>
    <table>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label2" runat="server" Text="ID"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server" Text="ID" Width="100%" Row="1" Style="height: 20px;" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label3" runat="server" Text="表單編號"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label5" runat="server" Text="申請類別(DV01)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label6" runat="server" Text="申請單位(DV02)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label7" runat="server" Text="申請日期(DV03)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label4" runat="server" Text="需求類別(DV05)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label8" runat="server" Text="原料來源(DV06)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox7" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label9" runat="server" Text="通路(DV07)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox8" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label10" runat="server" Text="是否提供產品成份(DV08)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox9" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label11" runat="server" Text="產品開發目的及方向(DV09)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox10" runat="server" Text="" Rows="5" Width="100%" Row="1" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label12" runat="server" Text="申請人(DV10)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox11" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label13" runat="server" Text="產品名稱DVV01"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox12" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label14" runat="server" Text="單位DVV02"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox13" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label15" runat="server" Text="產品規格/尺寸DVV03"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox14" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label16" runat="server" Text="重量DVV04"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox15" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label17" runat="server" Text="需求數量DVV05"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox16" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label18" runat="server" Text="產品預計末售價DVV06"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox17" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label19" runat="server" Text="預計出貨日期DVV07"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox18" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label20" runat="server" Text="需求數量超量說明DVV08"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox19" runat="server" Text="" Rows="5" Width="100%" Row="1" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label22" runat="server" Text="進度"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox21" runat="server" Text="" Rows="5" Width="100%" Row="1" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label23" runat="server" Text="更新日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox22" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label24" runat="server" Text="表單核準日期"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox23" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label25" runat="server" Text="預計生產日期" ForeColor="red"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <%--<asp:TextBox ID="TextBox24" runat="server" Text=""  Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>--%>
                <telerik:RadDatePicker ID="RadDatePicker1" runat="server" Width="120px"></telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label21" runat="server" Text="是否結案 Y/N" ForeColor="red"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox20" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>

    </table>

    <table>
        <%--  <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label4" runat="server" Text="是否刪除"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:Button ID="Button1" runat="server" Text="刪除" ForeColor="red"
                    OnClick="btn1_Click" meta:resourcekey="btn1Resource1" />
            </td>
        </tr>--%>
    </table>

</asp:Content>

