<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WKFConvertToAFJSON.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_gemps_WKFConvertToAFJSON" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="* 以下為操作步驟"></asp:Label>
                <br />
                <asp:Label ID="Label2" runat="server" ForeColor="Blue" Text="1. 將要轉換的表單手動起單送出"></asp:Label>
                <br />
                <asp:Label ID="Label5" runat="server" ForeColor="Blue" Text="2. 選擇是外掛欄位或是標準欄位"></asp:Label>
                <br />
                <asp:Label ID="Label6" runat="server" ForeColor="Blue" Text="3. 選擇外掛欄位時，輸入外掛欄位的欄位代號"></asp:Label>
                <br />
                <asp:Label ID="Label4" runat="server" ForeColor="Blue" Text="4. 將「TB_WKF_TASK」中的「CURRENT_DOC」內容取出"></asp:Label>
                <br />
                <asp:Label ID="Label7" runat="server" ForeColor="Blue" Text="5. 按下轉換"></asp:Label>
            </div>
            <div>
                <asp:Label ID="Label10" runat="server" ForeColor="Red" Text="* 開始操作"></asp:Label>
                <br />
                <asp:Label ID="Label11" runat="server" ForeColor="Blue" Text="錯誤訊息："></asp:Label>
                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Size="Large" Text=""></asp:Label>
            </div>
            <div>
                <asp:Button ID="Button1" runat="server" Text="轉換" OnClick="Button1_Click" />
            </div>
            <div>
                <asp:Label ID="Label1" runat="server" Text="欄位形式："></asp:Label>
                <asp:RadioButtonList ID="rbtnFormType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="外掛欄位" Value="0"></asp:ListItem>
                    <asp:ListItem Text="標準欄位" Value="1"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div>
                <asp:Label ID="Label12" runat="server" Text="Field_ID："></asp:Label>
                <asp:TextBox ID="txtField_ID" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="Label8" runat="server" Text="CURRENT_DOC："></asp:Label>
                <br />
                <asp:TextBox ID="txtCURRENT_DOC" runat="server" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox>
            </div>
            
            <div>
                <asp:Label ID="Label9" runat="server" Text="輸出成自動啟單JSON："></asp:Label>
                <br />
                <asp:TextBox ID="txtAFJSON" runat="server" TextMode="MultiLine" ReadOnly="true" Rows="10" Width="100%"></asp:TextBox>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
