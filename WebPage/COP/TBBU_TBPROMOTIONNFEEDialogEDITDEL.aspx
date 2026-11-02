<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_TBPROMOTIONNFEEDialogEDITDEL.aspx.cs" Inherits="CDS_WebPage_TBBU_TBBU_TBPROMOTIONNFEEDialogEDITDEL" %>

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
                <asp:Label ID="Label1" runat="server" Text="TBBU_TBPROMOTIONNFEEDialogEDITDEL"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan="2">
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>
        </tr>

    </table>
    <table>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label2" runat="server" Text="年度"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server" Text="" MaxLength="4" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label16" runat="server" Text="申請部門"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox14" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label17" runat="server" Text="職務"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox15" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label3" runat="server" Text="申請人"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label5" runat="server" Text="活動/目的"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server" Text="" TextMode="MultiLine" Rows="5" Width="100%" Style="height: 80px;"></asp:TextBox>
            </td>
        </tr>

        <%-- <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label5" runat="server" Text="類別"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>                
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label6" runat="server" Text="方式"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>                
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label7" runat="server" Text="搭贈方式"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:DropDownList ID="DropDownList3" runat="server"></asp:DropDownList>               
            </td>
        </tr>--%>

        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label9" runat="server" Text="日期:起~迄"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox7" runat="server" Text="" TextMode="MultiLine" Rows="5" Width="100%" Style="height: 80px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label10" runat="server" Text="對象(經銷商)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox8" runat="server" Text="" TextMode="MultiLine" Rows="5" Width="100%" Style="height: 80px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label11" runat="server" Text="通路"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox9" runat="server" Text="" TextMode="MultiLine" Rows="5" Width="100%" Style="height: 80px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label6" runat="server" Text="活動內容(詳述)"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server" Text="" TextMode="MultiLine" Rows="10" Width="100%" Style="height: 200px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label20" runat="server" Text="商品"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox18" runat="server" Text="" TextMode="MultiLine" Rows="5" Width="100%" Style="height: 80px;"></asp:TextBox>
            </td>
        </tr>
        <%--        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label12" runat="server" Text="銷量預估數量"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox10" runat="server" Text="" Width="100%" Row="1" Style="height: 20px;" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label13" runat="server" Text="預估-總收入"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox11" runat="server" Text="0" Width="100%" Row="1" Style="height: 20px;" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label18" runat="server" Text="預估-總成本"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server" Text="0" Width="100%" Row="1" Style="height: 20px;" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label19" runat="server" Text="預估-總費用"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server" Text="0" Width="100%" Row="1" Style="height: 20px;" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label14" runat="server" Text="預估-利潤預估"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox12" runat="server" Text="0" Width="100%" Row="1" Style="height: 20px;" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>

        <%-- <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label15" runat="server" Text="說明"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox13" runat="server" Text="" Width="100%" Row="5" TextMode="MultiLine" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label7" runat="server" Text="實際-總收入"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox10" runat="server" Text="0" Width="100%" Row="1" Style="height: 20px;" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label8" runat="server" Text="實際-總成本"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox13" runat="server" Text="0" Width="100%" Row="1" Style="height: 20px;" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label12" runat="server" Text="實際-總費用"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox16" runat="server" Text="0" Width="100%" Row="1" Style="height: 20px;" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label15" runat="server" Text="實際-利潤"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox17" runat="server" Text="0" Width="100%" Row="1" Style="height: 20px;" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        </tr>
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label21" runat="server" Text="是否結案"></asp:Label>
                </td>
                <td class="PopTableRightTD">
                    <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                </td>
            </tr>
    </table>

    <table>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label4" runat="server" Text="是否刪除"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:Button ID="Button1" runat="server" Text="刪除" ForeColor="red"
                    OnClick="btn1_Click" meta:resourcekey="btn1Resource1" OnClientClick="return confirm('是否刪除 ?');" />
            </td>
        </tr>
    </table>

</asp:Content>

