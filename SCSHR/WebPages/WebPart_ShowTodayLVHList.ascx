<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebPart_ShowTodayLVHList.ascx.cs" Inherits="CDS_SCSHR_WebPages_WebPart_ShowTodayLVHList" %>

<style>
    .kyti-GridPager a, .kyti-GridPager span {
        display: block;
        height: 25px;
        width: 25px;
        font-weight: bold;
        text-align: center;
        text-decoration: none;
    }

    .kyti-GridPager a {
        background-color: #f5f5f5;
        color: #969696;
        border: 1px solid #969696;
    }

    .kyti-GridPager span {
        background-color: #A1DCF2;
        color: #ff0000;
        border: 1px solid #3AC0F2;
    }

    table .grid th {
        border: 1px solid black;
        height: 25px;
        background-color: #336699;
        color: white;
        padding: 5px;
    }

    table .grid tr td {
        border: 1px solid black;
        height: 25px;
        padding: 5px;
    }

    table .grid tr:last-child td {
        border: 1px solid black;
        height: 25px;
        padding: 5px;
    }

    table .grid tr:last-child table {
        border: 0px;
    }
</style>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>
            <asp:CheckBox ID="chkIncludeSubGroup" runat="server" Text="包含子部門" AutoPostBack="true" OnCheckedChanged="chkIncludeSubGroup_CheckedChanged" />
        </div>
        <div>
            <asp:GridView ID="gvMain" CssClass="grid" runat="server" 
                Width="100%" 
                AutoGenerateColumns="False" 
                ShowHeaderWhenEmpty="True" 
                ForeColor="#333333" 
                AllowPaging="true" 
                PageSize="10" 
                OnPageIndexChanging="gvMain_PageIndexChanging">
                <PagerStyle CssClass="kyti-GridPager" />
                <AlternatingRowStyle BackColor="#ffffcc" />
                <Columns>
                   <asp:BoundField HeaderText="姓名" DataField="NAME" ReadOnlY="true"  />
                   <asp:BoundField HeaderText="請假" DataField="LVHTIME" ReadOnlY="true"  />
                   <asp:BoundField HeaderText="出差" DataField="TRAVELTIME" ReadOnlY="true"  />
                </Columns>
            </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
