<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UOFFormStructToAutoStartFormJSON.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_KYTUtils_UOFFormStructToAutoStartFormJSON" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Label ID="Label3" runat="server" ForeColor="Green" Text="* 以下為操作步驟"></asp:Label>
                <br />
                <asp:Label ID="Label2" runat="server" ForeColor="Blue" Text="1. 輸入表單名稱，取得表單結構"></asp:Label>
                <br />
                <asp:Label ID="Label5" runat="server" ForeColor="Blue" Text="1-1. 自行貼上結構"></asp:Label>
                <br />
                <asp:Label ID="Label14" runat="server" ForeColor="Blue" Text="2. 選擇是外掛欄位或是標準欄位"></asp:Label>
                <br />
                <asp:Label ID="Label6" runat="server" ForeColor="Blue" Text="3. 選擇外掛欄位時，輸入外掛欄位的欄位代號"></asp:Label>
                <br />
                <asp:Label ID="Label4" runat="server" ForeColor="Blue" Text="4. 如果要輸出說明，需勾選「取得預設值和欄位內容清單」"></asp:Label>
                <br />
                <asp:Label ID="Label7" runat="server" ForeColor="Blue" Text="5. 按下轉換"></asp:Label>
            </div>
            <div>
                <asp:Label ID="Label10" runat="server" ForeColor="Green" Text="* 開始操作"></asp:Label>
                <br />
                <asp:Label ID="Label11" runat="server" ForeColor="Red" Text="錯誤訊息："></asp:Label>
                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Size="Large" Text=""></asp:Label>
            </div>
            <div>
                <asp:Label ID="Label13" runat="server" Text="表單名稱："></asp:Label>
                <asp:TextBox ID="txtFormName" runat="server" placeHolder="輸入表單名稱"></asp:TextBox>
                <asp:Button ID="btnGetFormField" runat="server" Text="取得最新版本的表單結構" OnClick="btnGetFormField_Click" />

            </div>
            <div>
                <asp:Label ID="Label1" runat="server" Text="欄位形式："></asp:Label>
                <asp:RadioButtonList ID="rbtnFormType" runat="server" RepeatDirection="Horizontal">
                    <%--<asp:ListItem Text="外掛欄位" Value="0"></asp:ListItem>--%>
                    <asp:ListItem Text="標準欄位" Value="1" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div>
                <asp:CheckBox ID="chkGetDefault" runat="server" Text="取得預設值和欄位內容清單" />
            </div>
            <div>
                <asp:Button ID="Button1" runat="server" Text="轉換" OnClick="Button1_Click" />
            </div>
            <div>
                <asp:Label ID="Label12" runat="server" Text="Field_ID："></asp:Label>
                <asp:TextBox ID="txtField_ID" runat="server" placeHolder="外掛欄位的FieldID"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="Label8" runat="server" Text="VERSION_FIELD："></asp:Label>
                <br />
                <asp:TextBox ID="txtVERSION_FIELD" runat="server" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox>
            </div>

            <div>
                <asp:Label ID="Label9" runat="server" Text="輸出成自動起單JSON："></asp:Label>
                <br />
                <asp:TextBox ID="txtAFJSON" runat="server" TextMode="MultiLine" ReadOnly="true" Rows="10" Width="100%"></asp:TextBox>
            </div>
             <div>
                <asp:Label ID="Label15" runat="server" Text="輸出成自動起單C#片段："></asp:Label>
                <br />
                <asp:TextBox ID="txtAFCSharp" runat="server" TextMode="MultiLine" ReadOnly="true" Rows="10" Width="100%"></asp:TextBox>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
