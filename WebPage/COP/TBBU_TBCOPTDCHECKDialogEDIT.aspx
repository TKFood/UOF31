<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_TBCOPTDCHECKDialogEDIT.aspx.cs" Inherits="CDS_WebPage_TBBU_PRODUCTSDialogEDITDEL" %>

<%@ Register Src="~/Common/HtmlEditor/UC_HtmlEditor.ascx" TagPrefix="uc1" TagName="UC_HtmlEditor" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

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
                <asp:Label ID="Label1" runat="server" Text="TBBU_TBCOPTDCHECKDialogEDIT"></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan="2">
                <asp:Label ID="lblParam" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
            </td>
            <td class="PopTableRightTD" colspan="2">
                <asp:Label ID="ACCOUNTLabel" runat="server" Text=""></asp:Label>
            </td>

        </tr>


        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label7" runat="server" Text="客戶"></asp:Label>

            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox15" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label19" runat="server" Text="單別"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox1" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label20" runat="server" Text="單號"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox2" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label21" runat="server" Text="序號"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox3" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label22" runat="server" Text="品號"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox4" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label23" runat="server" Text="品名"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox5" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label24" runat="server" Text="訂單數量"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox6" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label8" runat="server" Text="贈品量"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox18" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label9" runat="server" Text="已交數量"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox19" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label10" runat="server" Text="贈品已交"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox20" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label25" runat="server" Text="單位"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox7" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label26" runat="server" Text="單價"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox8" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label27" runat="server" Text="金額"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox9" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label28" runat="server" Text="預交日"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox10" runat="server" Text="" Width="100%" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label11" runat="server" Text="單頭備註"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox21" runat="server" Text="" Width="100%" ReadOnly="True" TextMode="MultiLine" Row="5" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="PopTableLeftTD">
                <asp:Label ID="Label29" runat="server" Text="單身備註"></asp:Label>
            </td>
            <td class="PopTableRightTD">
                <asp:TextBox ID="TextBox22" runat="server" Text="" Width="100%" ReadOnly="True" TextMode="MultiLine" Row="5" Style="height: 120px;"></asp:TextBox>
            </td>
        </tr>
    </table>

    <div style="background-color: #FFD382; padding: 10px; margin-bottom: 5px;">
        生管區
        <table>
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label12" runat="server" Text="生管更新日期"></asp:Label>
                </td>
                <td class="PopTableRightTD">
                    <asp:TextBox ID="TextBox11" runat="server" Text="" Width="100%" ReadOnly="True" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label13" runat="server" Text="生管是否核準"></asp:Label>
                </td>
                <td class="PopTableRightTD">
                    <%--<asp:TextBox ID="TextBox12" runat="server" Text="" Width="100%" ></asp:TextBox>--%>
                    <asp:DropDownList ID="DropDownList1" runat="server" values="">
                        <asp:ListItem Selected="True" Value="N">N</asp:ListItem>
                        <asp:ListItem Value="Y">Y</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label14" runat="server" Text="生管備註"></asp:Label>
                </td>
                <td class="PopTableRightTD">
                    <asp:TextBox ID="TextBox13" runat="server" Text="" Width="100%" TextMode="MultiLine" Row="5" Style="height: 120px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label2" runat="server" Text="更新:"></asp:Label>
                </td>
                <td class="PopTableRightTD">
                    <asp:Button ID="Button1" runat="server" Text="更新" ForeColor="red"
                        OnClick="Button1_Click" meta:resourcekey="btn1Resource1" OnClientClick="return confirm('是否更新? ');" />
                </td>
            </tr>






        </table>
    </div>
    <div style="background-color: #82FF82; padding: 10px; margin-bottom: 5px;">
        採購區
        <table>
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label15" runat="server" Text="採購更新日期"></asp:Label>
                </td>
                <td class="PopTableRightTD">
                    <asp:TextBox ID="TextBox14" runat="server" Text="" Width="100%" ReadOnly="True" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label16" runat="server" Text="採購是否核準"></asp:Label>
                </td>
                <td class="PopTableRightTD">
                    <%-- <asp:TextBox ID="TextBox15" runat="server" Text="" Width="100%"></asp:TextBox>--%>
                    <asp:DropDownList ID="DropDownList2" runat="server" values="">
                        <asp:ListItem Selected="True" Value="N">N</asp:ListItem>
                        <asp:ListItem Value="Y">Y</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label17" runat="server" Text="採購備註"></asp:Label>
                </td>
                <td class="PopTableRightTD">
                    <asp:TextBox ID="TextBox16" runat="server" Text="" Width="100%" TextMode="MultiLine" Row="5" Style="height: 120px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="PopTableLeftTD">
                    <asp:Label ID="Label4" runat="server" Text="更新:"></asp:Label>
                </td>
                <td class="PopTableRightTD">
                    <asp:Button ID="Button2" runat="server" Text="更新" ForeColor="red"
                        OnClick="Button2_Click" meta:resourcekey="btn2Resource1" OnClientClick="return confirm('是否更新? ');" />
                </td>
            </tr>
        </table>
    </div>
    <div style="background-color: #30FFFF; padding: 10px; 5">
        業務區
          <table>
              <tr>
                  <td class="PopTableLeftTD">
                      <asp:Label ID="Label6" runat="server" Text="業務更新日期"></asp:Label>
                  </td>
                  <td class="PopTableRightTD">
                      <asp:TextBox ID="TextBox12" runat="server" Text="" Width="100%" ReadOnly="True" Enabled="false"></asp:TextBox>
                  </td>
              </tr>
              <tr>
                  <td class="PopTableLeftTD">
                      <asp:Label ID="Label18" runat="server" Text="業務備註"></asp:Label>
                  </td>
                  <td class="PopTableRightTD">
                      <asp:TextBox ID="TextBox17" runat="server" Text="" Width="100%" TextMode="MultiLine" Row="5" Style="height: 120px;"></asp:TextBox>
                  </td>
              </tr>
              <tr>
                  <td class="PopTableLeftTD">
                      <asp:Label ID="Label5" runat="server" Text="更新:"></asp:Label>
                  </td>
                  <td class="PopTableRightTD">
                      <asp:Button ID="Button3" runat="server" Text="更新" ForeColor="red"
                          OnClick="Button3_Click" meta:resourcekey="btn3Resource1" OnClientClick="return confirm('是否更新? ');" />
                  </td>
              </tr>
          </table>
    </div>



</asp:Content>

