<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" CodeFile="findOVERTIME.aspx.cs" Inherits="CDS_SCSHR_WKFFields_QUERYWINDOWS_findOVERTIME" %>

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
            <asp:GridView ID="gvMain" runat="server" 
                CssClass="grid" Width="100%" 
                ShowHeader="true" ShowHeaderWhenEmpty="true"
                AutoGenerateColumns="false" 
                OnRowDataBound="gvMain_RowDataBound" 
                ForeColor="#333333">
                <PagerStyle CssClass="GridPager" />
                <AlternatingRowStyle BackColor="#ffffcc" />
                <Columns>
                   <asp:BoundField HeaderText="員工ID" DataField="ACCOUNT" />
                   <asp:BoundField HeaderText="員工姓名" DataField="USER_NAME" />
                   <asp:BoundField HeaderText="加班日期" DataField="OT_DATE" />
                   <asp:BoundField HeaderText="加班起始" DataField="OT_START" />
                   <asp:BoundField HeaderText="加班迄止" DataField="OT_END" />
                   <asp:BoundField HeaderText="加班時數" DataField="OT_TIMES" />
                   <asp:BoundField HeaderText="加班類別" DataField="CHANGETYPE_DESC" />
                   <asp:BoundField HeaderText="簽核結果" DataField="SIGN_RESULT" />
                    
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
