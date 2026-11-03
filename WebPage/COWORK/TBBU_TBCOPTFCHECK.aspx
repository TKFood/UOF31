<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TBBU_TBCOPTFCHECK.aspx.cs" Inherits="CDS_WebPage_COP_TBBU_TBCOPTFCHECK" %>

<%@ Register Assembly="Ede.Uof.Utility.Component.Grid" Namespace="Ede.Uof.Utility.Component" TagPrefix="Fast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- 本頁樣式直接內嵌在頁面中，部署時不用另外複製 CSS 檔 --%>
    <style type="text/css">
        /* 訂單協調／訂單變更協調頁面共用樣式
           使用頁面：COWORK/TBBU_TBCOPTDCHECK(_BAKING).aspx、COWORK/TBBU_TBCOPTFCHECK(_BAKING).aspx */

        /* ---------- 顏色說明 ---------- */
        .check-legend {
            margin: 6px 0 8px;
            font-size: 15px;
            color: #555;
        }

        .check-legend span {
            display: inline-block;
            margin-right: 14px;
        }

        .check-legend i {
            display: inline-block;
            width: 16px;
            height: 16px;
            margin-right: 4px;
            vertical-align: -2px;
            border: 1px solid #c8d0da;
        }

        /* ---------- 列表外框：固定高度、表頭不捲動 ---------- */
        .grid-wrap {
            width: 100%;
            max-height: calc(100vh - 280px);
            min-height: 200px;
            overflow: auto;
            border: 1px solid #d9dee5;
        }

        /* GridView 預設會輸出 border-collapse:collapse，固定表頭需要改成 separate */
        /* 表格至少跟畫面一樣寬，欄位多時可左右捲動 */
        .grid-wrap table {
            width: auto !important;
            min-width: 100%;
            border-collapse: separate !important;
            border-spacing: 0;
            border-left: 1px solid #b8c4d2;
            font-size: 15px;
        }

        /* 格線：每格都有右框線與下框線 */
        .grid-wrap th,
        .grid-wrap td {
            border-top: 0 !important;
            border-left: 0 !important;
            border-right: 1px solid #b8c4d2 !important;
            border-bottom: 1px solid #b8c4d2 !important;
        }

        /* 表頭：深藍底白字，往下捲動時固定在上方 */
        .grid-wrap th {
            position: sticky;
            top: 0;
            z-index: 2;
            padding: 8px 8px;
            color: #fff !important;
            background: #2f5f8f !important;
            border-right-color: #6f8fae !important;
            border-bottom: 2px solid #1f4369 !important;
            font-weight: bold;
            white-space: nowrap;
            text-align: center;
        }

        /* 可排序欄位的表頭文字是連結 */
        .grid-wrap th a,
        .grid-wrap th a:link,
        .grid-wrap th a:visited {
            color: #fff !important;
            text-decoration: none;
        }

        .grid-wrap th a:hover {
            text-decoration: underline;
        }

        .grid-wrap td {
            padding: 6px 8px;
            background-color: #fff;
            vertical-align: top;
            line-height: 1.5;
        }

        /* 滑過的列加深，方便對照同一列的欄位 */
        .grid-wrap tr:hover > td {
            background-color: #e8f0fa;
        }

        /* ---------- 欄位類型：數字靠右、文字靠左，Y/N 與按鈕置中（表頭跟著欄位對齊） ---------- */
        .grid-wrap .col-num {
            text-align: right;
            white-space: nowrap;
        }

        .grid-wrap .col-text,
        .grid-wrap .col-code,
        .grid-wrap .col-item,
        .grid-wrap .col-name,
        .grid-wrap .col-note,
        .grid-wrap .col-date {
            text-align: left;
        }

        .grid-wrap .col-flag,
        .grid-wrap .col-action {
            text-align: center;
            white-space: nowrap;
        }

        .grid-wrap .col-code {
            white-space: nowrap;
        }

        /* 更新日期是「日期 時間」，空白處換成兩行 */
        .grid-wrap .col-date {
            min-width: 90px;
        }

        .grid-wrap .col-text {
            min-width: 110px;
        }

        .grid-wrap .col-name {
            min-width: 200px;
        }

        .grid-wrap .col-item {
            white-space: nowrap;
        }

        .grid-wrap .col-note {
            min-width: 220px;
            max-width: 320px;
            white-space: pre-line;
            word-break: break-all;
        }

        .grid-wrap .col-input textarea {
            width: 220px;
            font-size: 15px;
            box-sizing: border-box;
        }

        /* ---------- 生管核准狀態 ---------- */
        .grid-wrap tr.row-ready > td { background-color: #eaf6ea; }   /* 需生產，生管已核准 */
        .grid-wrap tr.row-wait > td  { background-color: #fff6dc; }   /* 需生產，等待生管核准 */

        /* 剛按過「更新」「送簽」的那一列：上下加橘色框線 */
        .grid-wrap tr.row-current > td {
            box-shadow: inset 0 2px 0 #f08c00, inset 0 -2px 0 #f08c00;
        }

        .grid-wrap td.st-ok   { color: #1a7f37; font-weight: bold; }
        .grid-wrap td.st-wait { color: #c62828; font-weight: bold; }

        .legend-ready { background-color: #eaf6ea; }
        .legend-wait  { background-color: #fff6dc; }
        .legend-free  { background-color: #fff; }

        .check-legend .legend-note {
            color: #1f5fa8;
        }

        /* ---------- 按鈕 ---------- */
        .btn-edit,
        .btn-send {
            padding: 4px 12px;
            font-size: 15px;
            border-radius: 3px;
            cursor: pointer;
            white-space: nowrap;
        }

        .btn-edit {
            color: #1f5fa8;
            background-color: #fff;
            border: 1px solid #1f5fa8;
        }

        .btn-send {
            color: #fff;
            background-color: #1f5fa8;
            border: 1px solid #1f5fa8;
        }

        .btn-edit:hover { background-color: #eef4fb; }
        .btn-send:hover { background-color: #174a84; }

        /* ---------- 查詢條件區 ---------- */
        .PopTable,
        .PopTable input,
        .PopTable select {
            font-size: 15px;
        }

        .PopTable input[type=text] {
            padding: 3px 4px;
        }

        .PopTable input[type=submit] {
            padding: 4px 14px;
        }

        /* 分頁標籤 */
        .RadTabStrip .rtsTxt {
            font-size: 15px;
        }

        /* 列表內的下拉選單 */
        .grid-wrap select {
            font-size: 15px;
        }
    </style>
    <script type="text/javascript">
        /* 按下列表中的「更新」「送簽」等按鈕後，畫面停在原本的位置，並標示剛操作的那一列 */
        (function () {
            var STORAGE_KEY = 'coptcheck.position.' + location.pathname;

            function gridWraps() {
                return document.querySelectorAll('.grid-wrap');
            }

            function indexOf(list, item) {
                for (var i = 0; i < list.length; i++) {
                    if (list[i] === item) { return i; }
                }
                return -1;
            }

            // 記住每個列表的捲動位置、整頁的捲動位置，以及按下的按鈕在第幾列
            function savePosition(sourceElement) {
                var wraps = gridWraps();
                var state = { grids: [], winX: window.pageXOffset, winY: window.pageYOffset, row: null };

                for (var i = 0; i < wraps.length; i++) {
                    state.grids.push({ top: wraps[i].scrollTop, left: wraps[i].scrollLeft });
                }

                if (sourceElement && sourceElement.closest) {
                    var wrap = sourceElement.closest('.grid-wrap');
                    var tr = sourceElement.closest('tr');
                    if (wrap && tr) {
                        state.row = { grid: indexOf(wraps, wrap), index: tr.rowIndex };
                    }
                }

                try { sessionStorage.setItem(STORAGE_KEY, JSON.stringify(state)); } catch (e) { }
            }

            function restorePosition() {
                var state = null;
                try {
                    state = JSON.parse(sessionStorage.getItem(STORAGE_KEY));
                    sessionStorage.removeItem(STORAGE_KEY);
                } catch (e) { }
                if (!state) { return; }

                var wraps = gridWraps();
                for (var i = 0; i < wraps.length && i < state.grids.length; i++) {
                    wraps[i].scrollTop = state.grids[i].top;
                    wraps[i].scrollLeft = state.grids[i].left;
                }
                window.scrollTo(state.winX, state.winY);

                if (state.row && wraps[state.row.grid]) {
                    var table = wraps[state.row.grid].querySelector('table');
                    var tr = table && table.rows[state.row.index];
                    if (tr) { tr.className += ' row-current'; }
                }
            }

            // 按了列表外的按鈕（查詢、批次更新），清掉之前記住的位置，避免開窗後沒存檔留下舊位置
            function clearPosition() {
                try { sessionStorage.removeItem(STORAGE_KEY); } catch (e) { }
            }

            // 只處理列表內的按鈕；查詢、批次更新等按鈕照常回到最上面
            function isGridButton(element) {
                return element && element.closest && element.closest('.grid-wrap') !== null;
            }

            // 一般送出（例如開窗更新後回到本頁）
            document.addEventListener('click', function (e) {
                var target = e.target;
                if (target.tagName !== 'INPUT' && target.tagName !== 'BUTTON') { return; }
                if (isGridButton(target)) {
                    savePosition(target);
                } else if (target.type === 'submit') {
                    clearPosition();
                }
            }, true);

            function hookAjax() {
                if (typeof Sys === 'undefined' || !Sys.WebForms || !Sys.WebForms.PageRequestManager) { return; }
                var prm = Sys.WebForms.PageRequestManager.getInstance();

                // UpdatePanel 局部更新前後
                prm.add_beginRequest(function (sender, args) {
                    var source = args.get_postBackElement();
                    if (isGridButton(source)) {
                        savePosition(source);
                    } else {
                        clearPosition();
                    }
                });
                prm.add_pageLoaded(function () {
                    restorePosition();
                });
            }

            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', function () { hookAjax(); restorePosition(); });
            } else {
                hookAjax();
                restorePosition();
            }
        })();
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage" SelectedIndex="0" AutoPostBack="True" OnTabClick="RadTabStrip2_TabClick">
                <Tabs>
                    <telerik:RadTab Text="訂單變更協調" PageViewID="RadPageView1"></telerik:RadTab>
                    <telerik:RadTab Text="生管用" PageViewID="RadPageView2"></telerik:RadTab>
                    <telerik:RadTab Text="採購用" PageViewID="RadPageView3" Visible="false"></telerik:RadTab>
                    <telerik:RadTab Text="業務用" PageViewID="RadPageView4"></telerik:RadTab>
                    <telerik:RadTab Text="不需生產庫存出貨" PageViewID="RadPageView5"></telerik:RadTab>
                    <telerik:RadTab Text="只有單頭" PageViewID="RadPageView7"></telerik:RadTab>
                    <telerik:RadTab Text="強制送簽（主管）" PageViewID="RadPageView6" Value="ADMIN"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <div class="check-legend">
                <span><i class="legend-ready"></i>需生產，生管已核准</span>
                <span><i class="legend-wait"></i>需生產，等待生管核准</span>
                <span><i class="legend-free"></i>不需生產</span>
                <span class="legend-note">整張單需要生產的明細都經生管核准後，才會出現「送簽」按鈕</span>
            </div>
            <telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
                    <div id="tabs-1">
                        <table class="PopTable">
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="年度: "></asp:Label>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="是否確認: "></asp:Label>
                                    <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="是否生產: "></asp:Label>
                                    <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="訂單單號: "></asp:Label>
                                    <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label33" runat="server" Text="客戶名稱: "></asp:Label>
                                    <asp:TextBox ID="TextBox24" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Label ID="Label34" runat="server" Text="品名: "></asp:Label>
                                    <asp:TextBox ID="TextBox25" runat="server"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="PopTableLeftTD"></td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text=" 查詢 " OnClick="Button1_Click"
                                        meta:resourcekey="btn1Resource1" />
                                </td>

                            </tr>
                        </table>

                        <table class="PopTable">
                            <tr>
                                <td colspan="2" class="PopTableRightTD">
                                    <div class="grid-wrap">
                                        <Fast:Grid ID="Grid1" OnRowDataBound="Grid1_RowDataBound" OnRowCommand="Grid1_RowCommand" runat="server" OnBeforeExport="OnBeforeExport1" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource1" OnPageIndexChanging="grid_PageIndexChanging1">
                                            <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                            <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                            <Columns>
                                                <asp:BoundField HeaderText="客戶" DataField="TC053" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                <asp:BoundField HeaderText="單別" DataField="TF001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                <asp:BoundField HeaderText="單號" DataField="TF002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                <asp:BoundField HeaderText="原序號" DataField="TF104" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                <asp:BoundField HeaderText="新序號" DataField="TF004" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                <asp:BoundField HeaderText="變更版次" DataField="TF003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                <asp:TemplateField HeaderText="是否生產" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_COPTFUDF01" runat="server" Text='<%# Bind("COPTFUDF01") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="品號" DataField="TF005" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                <asp:BoundField HeaderText="品名" DataField="TF006" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                <asp:BoundField HeaderText="新訂單數量" DataField="TF009" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                <asp:BoundField HeaderText="新贈品量" DataField="TF020" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                <asp:BoundField HeaderText="單位" DataField="TF010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                <asp:BoundField HeaderText="預交日" DataField="TF015" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                <asp:BoundField HeaderText="單頭備註" DataField="TE050" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                <asp:BoundField HeaderText="單頭變更原因" DataField="TE006" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                <asp:BoundField HeaderText="單身備註" DataField="TF032" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                <asp:BoundField HeaderText="單身變更原因" DataField="TF018" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                <asp:TemplateField HeaderText="生管核準" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_MOCCHECKS" runat="server" Text='<%# Bind("MOCCHECKS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                <asp:TemplateField HeaderText="更新進度" HeaderStyle-CssClass="col-action" ItemStyle-CssClass="col-action">
                                                    <ItemTemplate>
                                                        <asp:Button CssClass="btn-edit" ID="Button1" runat="server" Text="更新" CommandName="Button1" CommandArgument='<%# Eval("TF1234") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="是否送簽" HeaderStyle-CssClass="col-action" ItemStyle-CssClass="col-action">
                                                    <ItemTemplate>
                                                        <asp:Button CssClass="btn-send" ID="Button2" runat="server" Text="送簽" CommandName="Button2" CommandArgument='<%# Eval("TF123") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </Fast:Grid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>

                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView2" runat="server">
                    <div id="tabs-2">
                        <table class="PopTable">
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="年度: "></asp:Label>
                                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="是否確認: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList3" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="是否生產: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList4" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" Text="訂單單號: "></asp:Label>
                                        <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="客戶名稱: "></asp:Label>
                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="品名: "></asp:Label>
                                        <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                                    </td>

                                </tr>

                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button3" runat="server" Text=" 查詢 " OnClick="Button3_Click"
                                            meta:resourcekey="btn5Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button4" runat="server" Text=" 生管批次更新 " OnClick="Button4_Click"
                                            meta:resourcekey="btn4Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td colspan="2" class="PopTableRightTD">
                                        <div class="grid-wrap">
                                            <Fast:Grid ID="Grid2" OnRowDataBound="Grid2_RowDataBound" OnRowCommand="Grid2_RowCommand" runat="server" OnBeforeExport="OnBeforeExport2" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource2" OnPageIndexChanging="grid_PageIndexChanging2">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="客戶" DataField="TC053" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                    <asp:BoundField HeaderText="單別" DataField="TF001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="單號" DataField="TF002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="原序號" DataField="TF104" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="新序號" DataField="TF004" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="變更版次" DataField="TF003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:TemplateField HeaderText="是否生產" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_COPTFUDF01" runat="server" Text='<%# Bind("COPTFUDF01") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="品號" DataField="TF005" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                    <asp:BoundField HeaderText="品名" DataField="TF006" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField HeaderText="新訂單數量" DataField="TF009" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="新贈品量" DataField="TF020" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="單位" DataField="TF010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="預交日" DataField="TF015" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="單頭備註" DataField="TE050" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單頭變更原因" DataField="TE006" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                    <asp:BoundField HeaderText="單身備註" DataField="TF032" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身變更原因" DataField="TF018" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                    <asp:BoundField HeaderText="生管核準" DataField="MOCCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="採購核準" DataField="PURCHECKS" Visible="false" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="採購更新日期" DataField="PURCHECKDATES" Visible="false" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="採購備註" DataField="PURCHECKSCOMMENTS" Visible="false" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:TemplateField HeaderText="生管備註填寫" HeaderStyle-CssClass="col-input" ItemStyle-CssClass="col-input">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="GRIDVIEWTextBox1" runat="server" Text="" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="生管核準填寫" HeaderStyle-CssClass="col-input" ItemStyle-CssClass="col-input">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="GRIDVIEWDropDownList1" runat="server">
                                                                <asp:ListItem>N</asp:ListItem>
                                                                <asp:ListItem>Y</asp:ListItem>

                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView3" runat="server">
                    <div id="tabs-2">
                        <table class="PopTable">
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="年度: "></asp:Label>
                                        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="是否確認: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList5" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="是否生產: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList6" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label19" runat="server" Text="訂單單號: "></asp:Label>
                                        <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="客戶名稱: "></asp:Label>
                                        <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label22" runat="server" Text="品名: "></asp:Label>
                                        <asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button5" runat="server" Text=" 查詢 " OnClick="Button5_Click"
                                            meta:resourcekey="btn5Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button6" runat="server" Text=" 採購批次更新 " OnClick="Button6_Click"
                                            meta:resourcekey="btn6Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td colspan="2" class="PopTableRightTD">
                                        <div class="grid-wrap">
                                            <Fast:Grid ID="Grid3" OnRowDataBound="Grid3_RowDataBound" OnRowCommand="Grid3_RowCommand" runat="server" OnBeforeExport="OnBeforeExport3" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource3" OnPageIndexChanging="grid_PageIndexChanging3">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="客戶" DataField="TC053" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                    <asp:BoundField HeaderText="單別" DataField="TF001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="單號" DataField="TF002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="原序號" DataField="TF104" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="新序號" DataField="TF004" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="變更版次" DataField="TF003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:TemplateField HeaderText="是否生產" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_COPTFUDF01" runat="server" Text='<%# Bind("COPTFUDF01") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="品號" DataField="TF005" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                    <asp:BoundField HeaderText="品名" DataField="TF006" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField HeaderText="新訂單數量" DataField="TF009" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="新贈品量" DataField="TF020" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="單位" DataField="TF010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="預交日" DataField="TF015" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="單頭備註" DataField="TE050" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單頭變更原因" DataField="TE006" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                    <asp:BoundField HeaderText="單身備註" DataField="TF032" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身變更原因" DataField="TF018" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                    <asp:BoundField HeaderText="生管核準" DataField="MOCCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="採購核準" DataField="PURCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="採購更新日期" DataField="PURCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="採購備註" DataField="PURCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:TemplateField HeaderText="採購備註填寫" HeaderStyle-CssClass="col-input" ItemStyle-CssClass="col-input">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="GRIDVIEW2TextBox1" runat="server" Text="" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="採購核準填寫" HeaderStyle-CssClass="col-input" ItemStyle-CssClass="col-input">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="GRIDVIEW2DropDownList1" runat="server">
                                                                <asp:ListItem>N</asp:ListItem>
                                                                <asp:ListItem>Y</asp:ListItem>

                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView4" runat="server">
                    <div id="tabs-2">
                        <table class="PopTable">
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="年度: "></asp:Label>
                                        <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label15" runat="server" Text="是否確認: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList7" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text="是否生產: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList8" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label20" runat="server" Text="訂單單號: "></asp:Label>
                                        <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label24" runat="server" Text="客戶名稱: "></asp:Label>
                                        <asp:TextBox ID="TextBox16" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label26" runat="server" Text="品名: "></asp:Label>
                                        <asp:TextBox ID="TextBox17" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button7" runat="server" Text=" 查詢 " OnClick="Button7_Click"
                                            meta:resourcekey="btn7Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button8" runat="server" Text=" 業務批次更新 " OnClick="Button8_Click"
                                            meta:resourcekey="btn8Resource1" />
                                    </td>

                                </tr>
                            </table>
                            <table class="PopTable">
                                <tr>
                                    <td colspan="2" class="PopTableRightTD">
                                        <div class="grid-wrap">
                                            <Fast:Grid ID="Grid4" OnRowDataBound="Grid4_RowDataBound" OnRowCommand="Grid4_RowCommand" runat="server" OnBeforeExport="OnBeforeExport4" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource4" OnPageIndexChanging="grid_PageIndexChanging4">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="客戶" DataField="TC053" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                    <asp:BoundField HeaderText="單別" DataField="TF001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="單號" DataField="TF002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="原序號" DataField="TF104" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="新序號" DataField="TF004" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="變更版次" DataField="TF003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:TemplateField HeaderText="是否生產" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_COPTFUDF01" runat="server" Text='<%# Bind("COPTFUDF01") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="品號" DataField="TF005" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                    <asp:BoundField HeaderText="品名" DataField="TF006" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField HeaderText="新訂單數量" DataField="TF009" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="新贈品量" DataField="TF020" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="單位" DataField="TF010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="預交日" DataField="TF015" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="單頭備註" DataField="TE050" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單頭變更原因" DataField="TE006" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                    <asp:BoundField HeaderText="單身備註" DataField="TF032" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身變更原因" DataField="TF018" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                    <asp:BoundField HeaderText="生管核準" DataField="MOCCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="採購核準" DataField="PURCHECKS" Visible="false" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="採購更新日期" DataField="PURCHECKDATES" Visible="false" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="採購備註" DataField="PURCHECKSCOMMENTS" Visible="false" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:TemplateField HeaderText="業務備註填寫" HeaderStyle-CssClass="col-input" ItemStyle-CssClass="col-input">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="GRIDVIEW4TextBox1" runat="server" Text="" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="是否送簽" HeaderStyle-CssClass="col-action" ItemStyle-CssClass="col-action">
                                                        <ItemTemplate>
                                                            <asp:Button CssClass="btn-send" ID="GRIDVIEW4Button1" runat="server" Text="送簽" CommandName="GRIDVIEW4Button1" CommandArgument='<%# Eval("TF123") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView5" runat="server">
                    <div id="tabs-2">
                        <table class="PopTable">
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label21" runat="server" Text="年度: "></asp:Label>
                                        <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label23" runat="server" Text="是否確認: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList9" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>

                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label25" runat="server" Text="訂單單號: "></asp:Label>
                                        <asp:TextBox ID="TextBox15" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Text="客戶名稱: "></asp:Label>
                                        <asp:TextBox ID="TextBox18" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label28" runat="server" Text="品名: "></asp:Label>
                                        <asp:TextBox ID="TextBox19" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button9" runat="server" Text=" 查詢 " OnClick="Button7_Click"
                                            meta:resourcekey="btn7Resource1" />
                                    </td>

                                </tr>
                            </table>

                            <table class="PopTable">
                                <tr>
                                    <td colspan="2" class="PopTableRightTD">
                                        <div class="grid-wrap">
                                            <Fast:Grid ID="Grid5" OnRowDataBound="Grid5_RowDataBound" OnRowCommand="Grid5_RowCommand" runat="server" OnBeforeExport="OnBeforeExport5" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource5" OnPageIndexChanging="grid_PageIndexChanging5">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="客戶" DataField="TC053" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                    <asp:BoundField HeaderText="單別" DataField="TF001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="單號" DataField="TF002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="原序號" DataField="TF104" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="新序號" DataField="TF004" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="變更版次" DataField="TF003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:TemplateField HeaderText="是否生產" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_COPTFUDF01" runat="server" Text='<%# Bind("COPTFUDF01") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="品號" DataField="TF005" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                    <asp:BoundField HeaderText="品名" DataField="TF006" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField HeaderText="新訂單數量" DataField="TF009" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="新贈品量" DataField="TF020" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="單位" DataField="TF010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="預交日" DataField="TF015" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="單頭備註" DataField="TE050" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單頭變更原因" DataField="TE006" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                    <asp:BoundField HeaderText="單身備註" DataField="TF032" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身變更原因" DataField="TF018" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                    <asp:BoundField HeaderText="生管核準" DataField="MOCCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="採購核準" DataField="PURCHECKS" Visible="false" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="採購更新日期" DataField="PURCHECKDATES" Visible="false" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="採購備註" DataField="PURCHECKSCOMMENTS" Visible="false" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:TemplateField HeaderText="是否送簽" HeaderStyle-CssClass="col-action" ItemStyle-CssClass="col-action">
                                                        <ItemTemplate>
                                                            <asp:Button CssClass="btn-send" ID="GRIDVIEW5Button1" runat="server" Text="送簽" CommandName="GRIDVIEW5Button1" CommandArgument='<%# Eval("TF123") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView7" runat="server">
                    <div id="tabs6">
                        <table class="PopTable">
                            <table class="PopTable">
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label35" runat="server" Text="是否確認: "></asp:Label>
                                        <asp:DropDownList ID="DropDownList10" runat="server"></asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label36" runat="server" Text="年度: "></asp:Label>
                                        <asp:TextBox ID="TextBox26" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label31" runat="server" Text="訂單單號: "></asp:Label>
                                        <asp:TextBox ID="TextBox22" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label32" runat="server" Text="客戶名稱: "></asp:Label>
                                        <asp:TextBox ID="TextBox23" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button11" runat="server" Text=" 查詢 " OnClick="Button11_Click"
                                            meta:resourcekey="btn11Resource1" />
                                    </td>

                                </tr>
                            </table>

                            <table class="PopTable">
                                <tr>
                                    <td colspan="2" class="PopTableRightTD">
                                        <div class="grid-wrap">
                                            <Fast:Grid ID="Grid7" OnRowDataBound="Grid7_RowDataBound" OnRowCommand="Grid7_RowCommand" runat="server" OnBeforeExport="OnBeforeExport7" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource7" OnPageIndexChanging="grid_PageIndexChanging7">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="客戶" DataField="TC053" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                    <asp:BoundField HeaderText="單別" DataField="TE001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="單號" DataField="TE002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="原序號" DataField="TF104" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="新序號" DataField="TF004" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="變更版次" DataField="TE003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:TemplateField HeaderText="是否生產" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_COPTFUDF01" runat="server" Text='<%# Bind("COPTFUDF01") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="品號" DataField="TF005" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                    <asp:BoundField HeaderText="品名" DataField="TF006" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField HeaderText="新訂單數量" DataField="TF009" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="新贈品量" DataField="TF020" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="單位" DataField="TF010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="預交日" DataField="TF015" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="單頭備註" DataField="TE050" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單頭變更原因" DataField="TE006" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身備註" DataField="TF032" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身變更原因" DataField="TF018" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="生管核準" DataField="MOCCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="採購核準" DataField="PURCHECKS" Visible="false" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="採購更新日期" DataField="PURCHECKDATES" Visible="false" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="採購備註" DataField="PURCHECKSCOMMENTS" Visible="false" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:TemplateField HeaderText="是否送簽" HeaderStyle-CssClass="col-action" ItemStyle-CssClass="col-action">
                                                        <ItemTemplate>
                                                            <asp:Button CssClass="btn-send" ID="GRIDVIEW7Button1" runat="server" Text="送簽" CommandName="GRIDVIEW7Button1" CommandArgument='<%# Eval("TF123") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView6" runat="server">
                    <div id="tabs6">
                        <table class="PopTable">
                            <table class="PopTable">

                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="訂單單號: "></asp:Label>
                                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label29" runat="server" Text="客戶名稱: "></asp:Label>
                                        <asp:TextBox ID="TextBox20" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Label ID="Label30" runat="server" Text="品名: "></asp:Label>
                                        <asp:TextBox ID="TextBox21" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="PopTableLeftTD"></td>
                                    <td>
                                        <asp:Button ID="Button10" runat="server" Text=" 查詢 " OnClick="Button10_Click"
                                            meta:resourcekey="btn10Resource1" />
                                    </td>

                                </tr>
                            </table>

                            <table class="PopTable">
                                <tr>
                                    <td colspan="2" class="PopTableRightTD">
                                        <div class="grid-wrap">
                                            <Fast:Grid ID="Grid6" OnRowDataBound="Grid6_RowDataBound" OnRowCommand="Grid6_RowCommand" runat="server" OnBeforeExport="OnBeforeExport6" AllowPaging="true" AutoGenerateCheckBoxColumn="False" AllowSorting="True" AutoGenerateColumns="False" CustomDropDownListPage="False" DataKeyOnClientWithCheckBox="False" DefaultSortDirection="Ascending" EmptyDataText="No data found" EnhancePager="True" KeepSelectedRows="False" PageSize="1000" SelectedRowColor="" UnSelectedRowColor="" meta:resourcekey="Grid1Resource6" OnPageIndexChanging="grid_PageIndexChanging6">
                                                <EnhancePagerSettings FirstImageUrl="" FirstAltImageUrl="" PreviousImageUrl="" NextImageUrl="" LastImageUrl="" LastAltImage="" PageNumberCssClass="" PageNumberCurrentCssClass="" PageInfoCssClass="" PageRedirectCssClass="" NextIAltImageUrl="" PreviousAltImageUrl="" ShowHeaderPager="True"></EnhancePagerSettings>
                                                <ExportExcelSettings AllowExportToExcel="true" ExportType="GridContent"></ExportExcelSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="客戶" DataField="TC053" HeaderStyle-CssClass="col-text" ItemStyle-CssClass="col-text" />
                                                    <asp:BoundField HeaderText="單別" DataField="TF001" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="單號" DataField="TF002" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="原序號" DataField="TF104" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="新序號" DataField="TF004" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="變更版次" DataField="TF003" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="品號" DataField="TF005" HeaderStyle-CssClass="col-item" ItemStyle-CssClass="col-item" />
                                                    <asp:BoundField HeaderText="品名" DataField="TF006" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField HeaderText="新訂單數量" DataField="TF009" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="新贈品量" DataField="TF020" HeaderStyle-CssClass="col-num" ItemStyle-CssClass="col-num" />
                                                    <asp:BoundField HeaderText="單位" DataField="TF010" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code" />
                                                    <asp:BoundField HeaderText="預交日" DataField="TF015" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="單頭備註" DataField="TE050" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單頭變更原因" DataField="TE006" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                    <asp:BoundField HeaderText="單身備註" DataField="TF032" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="單身變更原因" DataField="TF018" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />

                                                    <asp:BoundField HeaderText="生管核準" DataField="MOCCHECKS" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="生管更新日期" DataField="MOCCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="生管備註" DataField="MOCCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="採購核準" DataField="PURCHECKS" Visible="false" HeaderStyle-CssClass="col-flag" ItemStyle-CssClass="col-flag" />
                                                    <asp:BoundField HeaderText="採購更新日期" DataField="PURCHECKDATES" Visible="false" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="採購備註" DataField="PURCHECKSCOMMENTS" Visible="false" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:BoundField HeaderText="業務更新日期" DataField="SALESCHECKDATES" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                                                    <asp:BoundField HeaderText="業務備註" DataField="SALESCHECKSCOMMENTS" HeaderStyle-CssClass="col-note" ItemStyle-CssClass="col-note" />
                                                    <asp:TemplateField HeaderText="是否送簽" HeaderStyle-CssClass="col-action" ItemStyle-CssClass="col-action">
                                                        <ItemTemplate>
                                                            <asp:Button CssClass="btn-send" ID="GRIDVIEW6Button1" runat="server" Text="送簽" CommandName="GRIDVIEW6Button1" CommandArgument='<%# Eval("TF123") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Fast:Grid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                    </div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

