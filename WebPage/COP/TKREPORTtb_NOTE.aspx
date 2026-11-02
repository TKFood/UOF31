<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TKREPORTtb_NOTE.aspx.cs" Inherits="CDS_WebPage_TKREPORTtb_NOTE" %>
<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<link href="CSS/NEWStyleSheet.css" type="text/css" rel="stylesheet" />


<script>    
    $(function () {
         
    });

    function ImageClick(img) {
        //alert(img);
        var clientId = '#' + img.id

        var widthString = $(clientId).css("width");
        var heightString = $(clientId).css("height");
        var widthUnit = widthString.replace("px", "");
        var heightUnit = heightString.replace("px", "");
        var width = parseInt(widthUnit, 10);
        var height = parseInt(heightUnit, 10);



        if ($(clientId).hasClass("BigImage")) {
            width = (width / 10);
            height = (height / 10);
            $(clientId).css("width", width + "px");
            $(clientId).css("height", height + "px");
            $(clientId).removeClass("BigImage");
        }
        else {
            width = (width * 10);
            height = (height * 10);
            $(clientId).css("width", width + "px");
            $(clientId).css("height", height + "px");
            $(clientId).addClass("BigImage");
        }

    }

    function CellClick(ImgUrl) {
        //alert(ImgUrl);
        $(".Show_imgStyle").attr("src", ImgUrl);
        $("#Show_img_Max").css("display", "block");
       
    }
    function divClick() {
        //alert("Show_img_Max");
        $("#Show_img_Max").css("display", "none");
    }
</script>
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server"></telerik:RadTabStrip>
    <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0">
	    <Tabs>
		    <telerik:RadTab Text="客情資料整理">
		    </telerik:RadTab>
		    <telerik:RadTab Text="明細資料">
		    </telerik:RadTab>
            <telerik:RadTab Text="客情明細資料">
		    </telerik:RadTab>
	    </Tabs>
    </telerik:RadTabStrip>
	<telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
	    <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
		   <div id="tabs-1">
                <table class="PopTable" >         
                     <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label2" runat="server" Text="日期:" meta:resourcekey="Label4Resource1"></asp:Label>             
                        </td>
                        <td class="PopTableRightTD">                    
                            <telerik:RadDatePicker ID="txtDate1"  runat="server" Width = "120px"></telerik:RadDatePicker>
                            <asp:Label ID="Label11" runat="server" Text="~"></asp:Label>
                            <telerik:RadDatePicker ID="txtDate2"  runat="server" Width = "120px"></telerik:RadDatePicker>
                            <asp:Label ID="Label12" runat="server" Text=" "></asp:Label>
                            <asp:Button ID="Button1" runat="server" Text=" 查詢 "
                                onclick="btn1_Click" meta:resourcekey="btn4Resource1" />                
                        </td>            
                    </tr>
                </table>

                 <label>當月累積數</label>
                <table class="PopTable">
                      <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid8" OnRowDataBound="Grid8_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource8" OnPageIndexChanging="grid8_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                             <asp:BoundField HeaderText="業務員" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>  
                                              <asp:BoundField HeaderText="負責客戶數" DataField="SALES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                              <asp:BoundField HeaderText="拜訪客戶數" DataField="COMS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                              <asp:BoundField HeaderText="拜訪次數" DataField="NOTES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                              <asp:BoundField HeaderText="拜訪客戶完成率%" DataField="PCTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                
                                
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>

                <label>記錄當週筆數</label>
                <table class="PopTable">
                      <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid5" OnRowDataBound="Grid5_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource5" OnPageIndexChanging="grid5_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                             <asp:BoundField HeaderText="客情記錄次數" DataField="COUNTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                
                                
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>

               <label>業務員記錄</label>
                <table class="PopTable">
                      <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid6" OnRowDataBound="Grid6_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource6" OnPageIndexChanging="grid6_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                             <asp:BoundField HeaderText="業務員" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>  
                                              <asp:BoundField HeaderText="預計拜訪次數" DataField="PRECOUNTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                              <asp:BoundField HeaderText="實際記錄次數" DataField="COUNTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                              <asp:BoundField HeaderText="完成度%" DataField="PCTS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField> 
                                
                                
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>
                <label>下週預排拜訪</label>
                <table class="PopTable">
                      <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid7" OnRowDataBound="Grid7_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource7" OnPageIndexChanging="grid7_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                             <asp:BoundField HeaderText="業務員" DataField="NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>  
                                              <asp:BoundField HeaderText="預計拜訪日期" DataField="DAYS" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                              <asp:BoundField HeaderText="拜訪客戶" DataField="SUBJECT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                            </asp:BoundField>   
                                
                                
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>

                <label>客戶記錄-主管決議:是</label>
                <table class="PopTable">
                      <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                             <asp:BoundField HeaderText="業務" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                             <asp:BoundField HeaderText="客戶" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>   
                                            <asp:TemplateField HeaderText="記錄" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemTemplate>
                                                    <asp:Label ID="CONTENT" runat="server" Text='<%# Bind("NOTE_CONTENT") %>' style="text-align:left" HorizontalAlign="Left" Width="600px" ItemStyle-HorizontalAlign="Left"></asp:Label>
                                                    <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                                </ItemTemplate>                                        
                                             </asp:TemplateField>
                                           <%--  <asp:BoundField HeaderText="記錄" DataField="NOTE_CONTENT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                            </asp:BoundField> --%>  
                                             <asp:BoundField HeaderText="記錄日期" DataField="CREATE_DATETIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField> 
                               
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" runat="server"  HorizontalAlign="Center"  Length="100px" Width="100px" onclick="ImageClick(this)"/>
                                                </ItemTemplate >
                                            </asp:TemplateField>
                                
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>
                <label>專案記錄-主管決議:是</label>
                  <table class="PopTable">
                        <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid3" OnRowDataBound="Grid3_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource3" OnPageIndexChanging="grid3_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                            <asp:BoundField HeaderText="業務" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                             <asp:BoundField HeaderText="客戶名稱" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="專案名稱" DataField="OPPORTUNITY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="商品" DataField="PRODUCT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                                            </asp:BoundField>   
                                            <asp:BoundField HeaderText="專案金額" DataField="AMOUNT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            </asp:BoundField> 
                                             <asp:TemplateField HeaderText="記錄" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemTemplate>
                                                    <asp:Label ID="CONTENT" runat="server" Text='<%# Bind("NOTE_CONTENT") %>' style="text-align:left" HorizontalAlign="Left" Width="600px" ItemStyle-HorizontalAlign="Left"></asp:Label>
                                                    <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                                </ItemTemplate>                                        
                                             </asp:TemplateField>

                                           <%--  <asp:BoundField HeaderText="記錄" DataField="NOTE_CONTENT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                            </asp:BoundField>   --%>
                                             <asp:BoundField HeaderText="記錄日期" DataField="CREATE_DATETIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>   
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>

                <label>客戶記錄-主管決議:否</label>
                 <table class="PopTable">
                       <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid2" OnRowDataBound="Grid2_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource2" OnPageIndexChanging="grid2_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                             <asp:BoundField HeaderText="業務" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                             <asp:BoundField HeaderText="客戶" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>   
                                            <asp:TemplateField HeaderText="記錄" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemTemplate>
                                                    <asp:Label ID="CONTENT" runat="server" Text='<%# Bind("NOTE_CONTENT") %>' style="text-align:left" HorizontalAlign="Left" Width="600px" ItemStyle-HorizontalAlign="Left"></asp:Label>
                                                    <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                                </ItemTemplate>                                        
                                             </asp:TemplateField>
                                             <asp:BoundField HeaderText="記錄日期" DataField="CREATE_DATETIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField> 
                               
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%--<asp:Image ID="Image2" runat="server"  HorizontalAlign="Center"  Length="100px" Width="100px" onclick="ImageClick(this)"/>--%>
                                                    <asp:Image ID="Image2" runat="server"  HorizontalAlign="Center"  Length="100px" Width="100px"/>                                  
                                                </ItemTemplate >
                                            </asp:TemplateField>
                                
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>

                 <label>專案記錄-主管決議:否</label>
                  <table class="PopTable">
                      <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid4" OnRowDataBound="Grid4_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource4" OnPageIndexChanging="grid4_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                            <asp:BoundField HeaderText="業務" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                             <asp:BoundField HeaderText="客戶名稱" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="專案名稱" DataField="OPPORTUNITY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>  
                                             <asp:BoundField HeaderText="商品" DataField="PRODUCT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                                            </asp:BoundField>   
                                            <asp:BoundField HeaderText="專案金額" DataField="AMOUNT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            </asp:BoundField> 
                                             <asp:TemplateField HeaderText="記錄" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemTemplate>
                                                    <asp:Label ID="CONTENT" runat="server" Text='<%# Bind("NOTE_CONTENT") %>' style="text-align:left" HorizontalAlign="Left" Width="600px" ItemStyle-HorizontalAlign="Left"></asp:Label>
                                                    <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                                </ItemTemplate>                                        
                                             </asp:TemplateField>

                                           <%--  <asp:BoundField HeaderText="記錄" DataField="NOTE_CONTENT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                            </asp:BoundField>   --%>
                                             <asp:BoundField HeaderText="記錄日期" DataField="CREATE_DATETIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>   
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>

                <table>
                    <div id="Show_img_Max" onclick="divClick()">
                        <div>                
                            <img src="" class="Show_imgStyle" />
                        </div>
                    </div>

                </table>   
               
                 </div>
	    </telerik:RadPageView>
	    <telerik:RadPageView ID="RadPageView2" runat="server">
		    <div id="tabs-2">
              <table class="PopTable">
                 <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label1" runat="server" Text="資料" meta:resourcekey="Label4Resource1"></asp:Label>
                        </td>
                                  
                    </tr> 
                  </table>
                <label>當月客戶已訪數</label>
                <table class="PopTable">
                      <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid9" OnRowDataBound="Grid9_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource9" OnPageIndexChanging="grid9_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                             <asp:BoundField HeaderText="業務員" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>  
                                              <asp:BoundField HeaderText="客戶" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                                            </asp:BoundField>   
                                              <asp:BoundField HeaderText="拜訪客戶數" DataField="NOTES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                
                                
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>

                 <label>當月客戶未訪數</label>
                <table class="PopTable">
                      <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid10" OnRowDataBound="Grid10_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource10" OnPageIndexChanging="grid10_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                             <asp:BoundField HeaderText="業務員" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>  
                                              <asp:BoundField HeaderText="客戶" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="300px"></ItemStyle>
                                            </asp:BoundField>   
                                              <asp:BoundField HeaderText="拜訪客戶數" DataField="NOTES" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                
                                
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>

          </div>
	    </telerik:RadPageView>

    <telerik:RadPageView ID="RadPageView3" runat="server">
		    <div id="tabs-3">
                  <table class="PopTable" >         
                     <tr>
                        <td class="PopTableLeftTD">
                            <asp:Label ID="Label4" runat="server" Text="日期:" meta:resourcekey="Label4Resource1"></asp:Label>             
                        </td>
                        <td class="PopTableRightTD">                    
                            <telerik:RadDatePicker ID="txtDate3"  runat="server" Width = "120px"></telerik:RadDatePicker>
                            <asp:Label ID="Label5" runat="server" Text="~"></asp:Label>
                            <telerik:RadDatePicker ID="txtDate4"  runat="server" Width = "120px"></telerik:RadDatePicker>
                            <asp:Label ID="Label6" runat="server" Text=" "></asp:Label>

                             <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                              <asp:Button ID="Button2" runat="server" Text=" 查詢 " OnClientClick="return btn2_Click(this)" 
                                 meta:resourcekey="btn2Resource1" />   
                            

                        </td>            
                    </tr>
                </table>
                <label>明細</label>
                 <table class="PopTable">
                      <tr>
                        <td colspan="2" class="PopTableRightTD" >
                            <div style="overflow-x:auto;width:100%">
                                <Fast:Grid ID="Grid11" OnRowDataBound="Grid11_RowDataBound" runat="server"  OnBeforeExport="OnBeforeExport" AllowPaging="true"  AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="100" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource11" OnPageIndexChanging="grid11_PageIndexChanging" >
                                    <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                 
                                        <ExportExcelSettings AllowExportToExcel="true" ExportType="DataSource" ></ExportExcelSettings>
                                        <Columns>
                                             <asp:BoundField HeaderText="業務" DataField="USER_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="140px"></ItemStyle>
                                            </asp:BoundField>   
                                             <asp:BoundField HeaderText="客戶" DataField="COMPANY_NAME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField>   
                                            <asp:TemplateField HeaderText="記錄" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemTemplate>
                                                    <asp:Label ID="CONTENT" runat="server" Text='<%# Bind("NOTE_CONTENT") %>' style="text-align:left" HorizontalAlign="Left" Width="600px" ItemStyle-HorizontalAlign="Left"></asp:Label>
                                                    <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                                </ItemTemplate>                                        
                                             </asp:TemplateField>
                                           <%--  <asp:BoundField HeaderText="記錄" DataField="NOTE_CONTENT" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
                                            </asp:BoundField> --%>  
                                             <asp:BoundField HeaderText="記錄日期" DataField="CREATE_DATETIME" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                                <ItemStyle HorizontalAlign="Center" Width="200px"></ItemStyle>
                                            </asp:BoundField> 
                               
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" runat="server"  HorizontalAlign="Center"  Length="100px" Width="100px" onclick="ImageClick(this)"/>
                                                </ItemTemplate >
                                            </asp:TemplateField>
                                
                                        </Columns>
                                    </Fast:Grid>
                                </div>
                        </td>
                    </tr>
                </table>

          </div>
	    </telerik:RadPageView>
    </telerik:RadMultiPage>​
</asp:Content>

