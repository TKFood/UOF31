<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ItemStockPersonUC.ascx.cs" Inherits="WKF_OptionalFields_ItemStockUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<!-- Bootstrap core CSS -->
<link href="https://getbootstrap.com/docs/4.4/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
<style type="text/css">
    .mytable {
        border-collapse: collapse;
        width: 100%;
        margin-top: 20px;
        margin-bottom: 20px;
        padding: 30px;
    }

        .mytable, .mytable th, .mytable td {
            border: 1px solid #808080;
            color: #403d3d;
            height: 26px;
            line-height: 26px;
            padding: 5px;
        }
</style>
<%--<div class="row" style="width: 80%" id="divEdit" runat="server">--%>
<div class="row" style="width: 80%" runat="server">
    <div class="col-lg-3 col-md-3 col-6">發放部門</div>
    <div class="col-lg-3 col-md-3 col-6">
        <select id="Dept"></select>
        <div id="divDept"></div>
    </div>
    <div class="col-lg-3 col-md-3 col-6">接收人員</div>
    <div class="col-lg-3 col-md-3 col-6">
        <select id="DeptPerson"></select>
        <div id="divDeptPerson"></div>
    </div>
</div>

<asp:HiddenField ID="hdContent" runat="server" />
<asp:HiddenField ID="hdStatus" runat="server" />
<asp:HiddenField ID="hdSource" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdIsAudit" Value="N" runat="server" />

<table id="divContent" runat="server" class="mytable">
</table>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js"></script>
<script>

    var bl = 0;
    var b2 = 0;
    var ckdeptname = "";//確認部門
    var deptname = "";//部門名稱
    var receiver = "";//接收人員
    var specmap = {};
    var itemsarr = [];
    var itemsmap = {};
    var itemstockmap = {};
    var availableItems = [];        
    var status = $("#<%=hdStatus.ClientID %>").val();//表單狀態

    $(document).ready(function () {
        if (bl == 1) {
            return;
        }
        else
        {
            LoadDept();
            bl = 1;
            $("#<%=divContent.ClientID %>").append("<tr style='background-color:#0088A8'>" +
                "<th style='width:20%;color:#fff;text-align:center'>品號</th >" +
                "<th style='width:30%;color:#fff;text-align:center'>品名</th>" +
                "<th style='width:30%;color:#fff;text-align:center'>規格</th>" +
                "<th style='width:10%;color:#fff;text-align:center'>庫存數量</th>" +
                "<th style='width:10%;color:#fff;text-align:center'>申請數量</th>" +
                "</tr >");     
            loadSourceData();
        }   
    });

    //載入部門名稱
    function loadDeptCallback(response) {
        $.each(response, function (i, data) {
            if (i == 'data') {
                $('#Dept').append($('<option>', {
                    value: 0,
                    text: "請選擇部門"
                }));
                $.each(data, function (j, item) {
                    $('#Dept').append($('<option>', {
                        value: item.GROUP_NAME,
                        text: item.GROUP_NAME
                    }));
                });
            }
        });
    }

    //部門下拉選單事件
    $("#Dept").change(function () {
        $('#DeptPerson').empty();
        if ($("#Dept").val() == "0") {
            alert("請先選擇部門!");
            return;
        }
        $("#<%=divContent.ClientID %>  tr:not(:first)").html("");
        if (ckdeptname != $("#Dept").val()) {
            var dept = $("#Dept").val();
            ajax_load_json('/RM/Item/GetStockByDept?dept=' + dept, null, getStockByDeptCallback);
            ajax_load_json('/RM/Item/GetDeptPerson?dept=' + dept, null, loadDeptPersonCallback);
        }
        ckdeptname = $("#Dept").val();
    });

    //TD下拉選單事件
    function tdchgSelect(obj) {
        if ($("#DeptPerson").val() == 0) {
            alert("請選擇部門人員");
            return;
        }

        var tdqty = $(obj).attr('id'); //申請品號數量 
        var tddeptqty = "#TD_deptqty" + tdqty.replace('TD_qty', '');//庫存品號數量    
        var tdno = "#TD_no" + tdqty.replace('TD_qty', '');//庫存品號 
        if (parseInt($("#" + tdqty).val().trim()) > parseInt($(tddeptqty).text().trim())) {
            alert("申請數量不可大於庫存數量!!!");
            $("#" + tdqty).val("");
            return;
        }
        //更新數量
        itemsarr.forEach(function (item) {
            if ($("#DeptPerson").val() == null) {
                item.receiver = $("#divDeptPerson").text().trim();
            } else {
                item.receiver = $("#DeptPerson").val().trim();
            }
            if (item.no == $(tdno).text().trim()) {
                item.qty = $("#" + tdqty).val().trim();
                itemsarr[tdqty.replace('TD_qty', '')] = item;
            }
        });
        $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
    }

    //取得已做此表單內容
    function loadSourceData() {
        var num = 0;
        var jsontext = $("#<%=hdContent.ClientID %>").val();
        var data = $.parseJSON(jsontext);
        //alert(jsontext);
        $.each(data, function (i, item) {
            var obj = { "no": item.no, "name": item.name, "spec": item.spec, "deptqty": item.deptqty, "qty": item.qty, "deptname": item.deptname, "receiver": item.receiver };
            itemsarr.push(obj);

            var no = item.no;
            var itemname = item.name;
            var spec = item.spec;
            var deptqty = item.deptqty;//庫存數量
            var qty = item.qty;//申請數量
            deptname = item.deptname;
            receiver = item.receiver;
            if (status == "APPLY") {               
                $("#<%=divContent.ClientID %>").append("<tr id='" + no + "'>" +
                    "<td id='TD_no" + num + "'> " + no + "</td >" +
                    "<td>" + itemname + "</td>" +
                    "<td>" + spec + "</td>" +
                    "<td id='TD_deptqty" + num + "'>" + deptqty + "</td>" +
                    "<td><input id='TD_qty" + num + "'type='number' value='" + qty + "' onchange='javascript:tdchgSelect(this)'></td>" +
                    "<td style='display: none'>" + deptname + "</td>" +
                    "<td style='display: none'>" + receiver + "</td>" +
                    "</tr>");
            }
            else {
                $("#<%=divContent.ClientID %>").append("<tr id='" + no + "'>" +
                    "<td id='TD_no" + num + "'>" + no + "</td >" +
                    "<td>" + itemname + "</td>" +
                    "<td>" + spec + "</td>" +
                    "<td>" + deptqty + "</td>" +
                    "<td>" + qty + "</td>" +
                    "<td style='display: none'>" + deptname + "</td>" +
                    "<td style='display: none'>" + receiver + "</td>" +
                    "</tr>");
            }
            num++;
        });
        $("#Dept").append($("<option></option>").attr("value", deptname).text(deptname)).prop("disabled", true).hide();
        $("#DeptPerson").append($("<option></option>").attr("value", receiver).text(receiver)).prop("disabled", true).hide();
        $("#divDept").text(deptname);
        $("#divDeptPerson").text(receiver);
        $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
    }

    //載入部門人員
    function loadDeptPersonCallback(response) {
        $.each(response, function (i, data) {
            if (i == 'data') {
                $('#DeptPerson').empty();
                $('#DeptPerson').append($('<option>', {
                    value: 0,
                    text: "請選擇人員"
                }));
                $.each(data, function (j, item) {
                    $('#DeptPerson').append($('<option>', {
                        value: item.name,
                        text: item.name
                    }));
                });
            }
        });
    }

    //取部門倉
    function getStockByDeptCallback(response) {
        var num = 0;
        $.each(response, function (i, data) {
            if (i == 'data') {
                $.each(data, function (j, item) {
                    var no = item.ITEM_NO;
                    var itemname = item.ITEM_NAME;
                    var spec = item.SPECIFICATION;
                    var deptqty = item.QUANTITY;//庫存數量
                    var qty = "";//申請數量
                    deptname = $("#Dept").val();
                    receiver = $('#DeptPerson').val();
                    $("#<%=divContent.ClientID %>").append("<tr id='" + no + "'>" +
                        "<td id='TD_no" + num + "'> " + no + "</td >" +
                        "<td>" + itemname + "</td>" +
                        "<td>" + spec + "</td>" +
                        "<td id='TD_deptqty" + num + "'>" + deptqty + "</td>" +
                        "<td><input id='TD_qty" + num + "'type='number' value='" + qty + "' onchange='javascript:tdchgSelect(this)'></td>" +
                        "<td style='display: none'>" + deptname + "</td>" +
                        "<td style='display: none'>" + receiver + "</td>" +
                        "</tr>");

                    var obj = { "no": item.ITEM_NO, "name": item.ITEM_NAME, "spec": item.SPECIFICATION, "deptqty": item.QUANTITY, "qty": qty, "deptname": deptname, "receiver": receiver };
                    itemsarr.push(obj);
                    num++;
                });
                $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
            }
        });
    }

    //取所有部門
    function LoadDept() {
        ajax_load_json('/RM/Item/GetDept', null, loadDeptCallback);
    }

    function ajax_load_json(url, data_array, callback) {
         //url = "http://60.249.251.160:8080" + url;//老楊
          url = "https://eip.tkfood.com.tw" + url;//老楊
        $.ajax({
            type: "Post",
            url: url,
            data: data_array,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (response) {
                callback(response);
            },
            error: function () {
                alert("載入失敗");
            }
        });
    }
</script>

<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>


