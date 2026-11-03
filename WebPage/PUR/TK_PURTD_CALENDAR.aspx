<%@ Page Title="採購預交行事曆" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TK_PURTD_CALENDAR.aspx.cs" Inherits="CDS_WebPage_PUR_TK_PURTD_CALENDAR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        /* ---------- 查詢列 ---------- */
        .cal-bar { display: flex; flex-wrap: wrap; align-items: center; gap: 6px 10px; margin: 6px 0 10px; font-size: 15px; }
        .cal-bar input[type=text], .cal-bar input[type=month] { font-size: 15px; padding: 3px 4px; }
        .cal-bar .cal-title { font-size: 22px; font-weight: bold; color: #1f4369; margin: 0 8px; min-width: 7em; text-align: center; }
        .cal-btn { padding: 4px 12px; font-size: 15px; border-radius: 3px; cursor: pointer; color: #1f5fa8; background: #fff; border: 1px solid #1f5fa8; }
        .cal-btn:hover { background: #eef4fb; }
        .cal-btn-main { color: #fff; background: #1f5fa8; }
        .cal-btn-main:hover { background: #174a84; }
        .cal-sum { color: #555; margin-left: auto; }

        /* ---------- 月曆 ---------- */
        .cal { width: 100%; table-layout: fixed; border-collapse: collapse; font-size: 14px; }
        .cal th { background: #2f5f8f; color: #fff; padding: 6px 0; border: 1px solid #6f8fae; font-size: 15px; }
        .cal th.wk-end { background: #476f97; }
        .cal td { border: 1px solid #b8c4d2; vertical-align: top; height: 120px; padding: 0; background: #fff; }
        .cal td.out { background: #f3f5f8; }
        .cal td.wk-end { background: #fafbfd; }
        .cal td.has { cursor: pointer; }
        .cal td.has:hover { background: #eef4fb; }
        .cal td.today { box-shadow: inset 0 0 0 2px #f08c00; }
        .cal td.sel { background: #e1ecf8 !important; }
        .cal .d-head { display: flex; justify-content: space-between; align-items: center; padding: 3px 6px; }
        .cal .d-num { font-weight: bold; font-size: 15px; color: #333; }
        .cal td.out .d-num { color: #aaa; }
        .cal .d-cnt { font-size: 12px; color: #fff; background: #1f5fa8; border-radius: 10px; padding: 0 7px; }
        .cal .d-list { padding: 0 4px 4px; }   /* 當天全部顯示，不出現捲軸，格子依筆數自動變高 */
        .cal .ent { margin: 2px 0; padding: 2px 4px; border-left: 3px solid #1f5fa8; background: #f2f6fb; line-height: 1.35; overflow-wrap: anywhere; }
        .cal .ent b { color: #1f4369; }
        .cal .ent .q { color: #c62828; white-space: nowrap; }

        /* ---------- 明細 ---------- */
        .cal-detail { margin-top: 14px; }
        .cal-detail h3 { font-size: 17px; color: #1f4369; margin: 0 0 6px; }
        .cal-detail table { width: 100%; border-collapse: collapse; font-size: 14px; }
        .cal-detail th { background: #2f5f8f; color: #fff; padding: 6px 4px; border: 1px solid #6f8fae; position: sticky; top: 0; }
        .cal-detail td { border: 1px solid #b8c4d2; padding: 4px 5px; }
        .cal-detail tr:hover > td { background: #e8f0fa; }
        .cal-detail .n { text-align: right; white-space: nowrap; }
        .cal-detail .c { white-space: nowrap; }

        @media (max-width: 1450px) {
            .cal, .cal-detail table { font-size: 12px; }
            .cal td { height: 100px; }
        }
    </style>

    <div class="cal-bar">
        <asp:Button ID="btnPrev" runat="server" Text="◀ 上個月" CssClass="cal-btn" OnClick="btnPrev_Click" />
        <asp:Label ID="lblMonth" runat="server" CssClass="cal-title"></asp:Label>
        <asp:Button ID="btnNext" runat="server" Text="下個月 ▶" CssClass="cal-btn" OnClick="btnNext_Click" />
        <asp:Button ID="btnThisMonth" runat="server" Text="本月" CssClass="cal-btn" OnClick="btnThisMonth_Click" />
        <span>月份</span>
        <asp:TextBox ID="txtMonth" runat="server" TextMode="Month" Width="150px"></asp:TextBox>
        <span>廠商</span>
        <asp:TextBox ID="txtVendor" runat="server" Width="120px" placeholder="代號/名稱"></asp:TextBox>
        <span>品號/品名</span>
        <asp:TextBox ID="txtItem" runat="server" Width="140px"></asp:TextBox>
        <asp:Button ID="btnQuery" runat="server" Text="查詢" CssClass="cal-btn cal-btn-main" OnClick="btnQuery_Click" />
        <asp:Label ID="lblSummary" runat="server" CssClass="cal-sum"></asp:Label>
    </div>

    <asp:Literal ID="litCalendar" runat="server"></asp:Literal>

    <div class="cal-detail">
        <h3><span id="detailTitle">本月明細</span>
            <button type="button" class="cal-btn" id="btnShowAll" onclick="calShowAll()" style="display:none;margin-left:8px;">顯示整月</button>
        </h3>
        <asp:Literal ID="litDetail" runat="server"></asp:Literal>
    </div>

    <script>
        // 點日期：明細只顯示當天；再點一次或按「顯示整月」恢復
        function calPick(td) {
            var key = td.getAttribute('data-date');
            var on = td.classList.contains('sel');
            var cells = document.querySelectorAll('.cal td.sel');
            for (var i = 0; i < cells.length; i++) cells[i].classList.remove('sel');
            if (on) { calShowAll(); return; }
            td.classList.add('sel');
            var rows = document.querySelectorAll('.cal-detail tbody tr');
            for (var j = 0; j < rows.length; j++)
                rows[j].style.display = rows[j].getAttribute('data-date') === key ? '' : 'none';
            document.getElementById('detailTitle').innerText = key.substr(0, 4) + '/' + key.substr(4, 2) + '/' + key.substr(6, 2) + ' 預交明細';
            document.getElementById('btnShowAll').style.display = '';
            document.querySelector('.cal-detail').scrollIntoView({ behavior: 'smooth' });
        }
        function calShowAll() {
            var cells = document.querySelectorAll('.cal td.sel');
            for (var i = 0; i < cells.length; i++) cells[i].classList.remove('sel');
            var rows = document.querySelectorAll('.cal-detail tbody tr');
            for (var j = 0; j < rows.length; j++) rows[j].style.display = '';
            document.getElementById('detailTitle').innerText = '本月明細';
            document.getElementById('btnShowAll').style.display = 'none';
        }
    </script>
</asp:Content>
