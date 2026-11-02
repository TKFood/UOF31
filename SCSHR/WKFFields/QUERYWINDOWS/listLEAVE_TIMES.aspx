<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" CodeFile="listLEAVE_TIMES.aspx.cs" Inherits="CDS_SCSHR_WKFFields_QUERYWINDOWS_listLEAVE_TIMES" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .GridPager a, .GridPager span {
            display: block;
            height: 25px;
            width: 25px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .GridPager a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .GridPager span {
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
            查詢範圍：
            <asp:Label ID="lblSearchRange" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
            </ br>
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
            <asp:GridView ID="gvMain" runat="server" 
                CssClass="grid" Width="100%" 
                ShowHeader="true" ShowHeaderWhenEmpty="true"
                AutoGenerateColumns="false" 
                OnRowDataBound="gvMain_RowDataBound" 
                ForeColor="#333333">
                <PagerStyle CssClass="GridPager" />
                <AlternatingRowStyle BackColor="#ffffcc" />
                <Columns>
                   <asp:BoundField HeaderText="假別" DataField="LEVNAME" />
                   <asp:BoundField HeaderText="可休時數" DataField="HOURS" />
                   <asp:BoundField HeaderText="已休/已結時數" DataField="LEAVEHOURS" />
                   <asp:BoundField HeaderText="剩餘時數" DataField="RESIDUEHOURS" />
                   <asp:BoundField HeaderText="生效起始日期" DataField="STARTDATE" />
                   <asp:BoundField HeaderText="生效結束日期" DataField="ENDDATE" />                    
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
