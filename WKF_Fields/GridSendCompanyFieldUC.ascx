<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridSendCompanyFieldUC.ascx.cs" Inherits="WKF_OptionalFields_GridSendCompanyFieldUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<link rel="canonical" href="https://getbootstrap.com/docs/4.4/components/modal/">
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

    .form-control {
        width: 100%;
        height: 30px !important;
    }
</style>

<asp:Label Visible="true" runat="server" Text=""></asp:Label>


<div class="row" style="width: 80%" id="divEdit" runat="server">
    <div class="col-lg-2 col-md-3 col-6">從採購單號載入</div>
    <div class="col-lg-2 col-md-6 col-6">
        <input type="text" id="FormNo" value="" placeholder="請輸入採購單號" class="form-control" />       
    </div>
    <div class="col-lg-4 col-md-6 col-6">
        <button type="button" class="btn btn-info btn-sm" id="btnSearch">搜尋</button>
    </div>

</div>

<table id="divContent" runat="server" class="mytable">
</table>

<!-- 沒用到 -->
<asp:TextBox ID="SiteID" type="hidden" runat="server"></asp:TextBox>
<!-- 沒用到 -->
<asp:HiddenField ID="hdSource" runat="server"></asp:HiddenField>

<asp:HiddenField ID="hdContent" runat="server" />
<asp:HiddenField ID="hdStatus" runat="server" />

<asp:HiddenField ID="hdDept" runat="server" />
<asp:HiddenField ID="hdApplicant" runat="server" />


<!-- 沒用到 -->
<asp:HiddenField ID="hdIsAudit" Value="N" runat="server" />

<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>

<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js"></script>

<style>
    ul {
        list-style-type: none;
    }

    .ui-autocomplete {
        border: 1px solid #666;
        display: inline-block;
        z-index: 9999;
        background-color: #fff;
    }

    .ui-menu-item {
        background-color: #fff;
        color: #0094ff;
    }

    .ui-state-focus {
        background-color: #0094ff;
        color: #fff;
        display: block;
    }

    .btn-danger {
        color: #fff;
    }
</style>

<script>
    var ckformno = "";
    var ckload = "N";
    var itemsarr = [];
    var fromnomap = [];
    var itemstockmap = {};//總倉庫存
    var bl = 0;

    $(document).ready(function () {
        if (bl == 1) {
            return;
        }
        else {       
           
            bl = 1;
            var status = $("#<%=hdStatus.ClientID %>").val();

            $("#<%=divContent.ClientID %>").append("<tr style='background-color:#0088A8'>"
                + "<th style='width:20%;color:#fff;text-align:center'>品號</th>"
                + "<th style='width:10%;color:#fff;text-align:center'>品名</th>"
                + "<th style='width:10%;color:#fff;text-align:center'>申請數量</th>"
                + "<th style='width:10%;color:#fff;text-align:center'>已入庫數量</th>"
                + "<th style='width:10%;color:#fff;text-align:center'>剩餘數量</th>"              
                + "<th style='width:10%;color:#fff;text-align:center'>以舊換新</th>"
                + "<th style='width:10%;color:#fff;text-align:center'>回收數量</th>"
                + "<th style='width:10%;color:#fff;text-align:center'>入庫數量</th>"
                + "<th style='width:10%;color:#fff;text-align:center'>總倉庫存</th>"
                + "</tr>");
           
            loadApplySourceData();           
        }
    
    });

    //載入採購單號
    function loadApplySourceData() {
        ajax_load_json('/RM/ComApply/GetFromNoList', null, loadApplySourceDataCallback);
    }

    //採購單回傳
    function loadApplySourceDataCallback(response) {
        fromnomap = [];
        $.each(response, function (i, data) {
            if (i == 'data') {
                $.each(data, function (j, item) {
                    fromnomap.push(item.FORM_NO + " | " + ConvertDate(item.CREATE_DATETIME).toString());
                   
                });

                $("#FormNo").autocomplete({
                    source: fromnomap,
                    select: function (event, ui) {
                        var arr = (ui.item.label).split("|");
                        $("#FormNo").val(arr[0]);                      
                        return false;
                    },
                    minLength: 0
                });
            }
        });
        loadAvailableStock();
    }

    //採購單搜尋事件
    $("#btnSearch").click(function () {
        if ($("#FormNo").val() == "") {
            alert("請輸入採購單號");
        }
        if (ckformno != $("#FormNo").val()) {
            var formno = $("#FormNo").val();
            $("#<%=divContent.ClientID %>  tr:not(:first)").html("");
            ajax_load_json("/RM/ComApply/Get?formno=" + formno, null, getDeptApplyStockCallback);
        }
        ckformno = $("#FormNo").val();
    });

    //載入總倉庫存
    function loadAvailableStock() {
        ajax_load_json('/RM/Item/GetDeptStock?dept=', null, loadAvailableStockCallback);
    }

    //品號總倉庫存回傳
    function loadAvailableStockCallback(response) {
        itemstockmap = {};
        $.each(response, function (i, data) {
            if (i == 'data') {
                $.each(data, function (j, item) {
                    itemstockmap[item.ITEM_NO] = item.QUANTITY;//總倉品號庫存
                });
            }
        });
        loadSourceData();
    }

    //採購單回傳資料
    function getDeptApplyStockCallback(response) {
        var chktransnew = "";
        var num = 0;
        var sendqty = "";
      
        if (ckload == "Y") {
            $("#btnSearch").hide();
            $("#FormNo").attr("disabled", "disabled");
        }
        $.each(response, function (i, item) {
            var stockqty = 0;
            var leftqty = parseInt(item.qty) - parseInt(item.sendqty);  //剩餘數量
            sendqty = parseInt(item.sendqty) + leftqty;//已入庫數量       

            if (item.transnew == "0") {
                //以舊換新 顯示回收數量
                chktransnew = "是";
            } else {
                //不顯示回收數量
                chktransnew = "否";
            }
            stockqty = itemstockmap[item.no];
            if (stockqty == undefined) stockqty = 0;
            if (leftqty == 0) {
                if (item.transnew == "0") {
                    $("#<%=divContent.ClientID %>").append("<tr>"
                        + "<td>" + item.no + "</td>"
                        + "<td>" + item.name + "</td>"
                        + "<td>" + item.qty + "</td>" //申請數量
                        + "<td>" + sendqty + "</td>"//已入庫數量
                        + "<td>" + leftqty + "</td>"//剩餘數量                      
                        + "<td>" + chktransnew + "</td>"
                        + "<td>" + item.sendqty + "</td>" //回收數量
                        + "<td>" + item.qty + "</td>"//入庫數量(手動)
                        + "<td>" + stockqty + "</td>"
                        + "</tr>");
                } else {
                    $("#<%=divContent.ClientID %>").append("<tr>"
                        + "<td>" + item.no + "</td>"
                        + "<td>" + item.name + "</td>"
                        + "<td>" + item.qty + "</td>" //申請數量
                        + "<td>" + sendqty + "</td>"//已入庫數量
                        + "<td>" + leftqty + "</td>"//剩餘數量                  
                        + "<td>" + chktransnew + "</td>"
                        + "<td>" + "</td>" //回收數量
                        + "<td>" + item.qty + "</td>"//入庫數量(手動)
                        + "<td>" + stockqty + "</td>"
                        + "</tr>");
                }
              
            } else {    
                if (item.transnew == "0") {
                    //以舊換新 顯示回收數量
                    $("#<%=divContent.ClientID %>").append("<tr>"
                        + "<td id='TD_no" + num + "'> " + item.no + "</td >"
                        + "<td>" + item.name + "</td>"
                        + "<td id='TD_qty" + num + "'>" + item.qty + "</td>"//申請數量
                        + "<td id='TD_sendqty" + num + "'>" + item.sendqty + "</td>"//已入庫數量
                        + "<td id='TD_leftqty" + num + "'>" + leftqty + "</td>"//剩餘數量
                        + "<td>" + chktransnew + "</td>"
                        + "<td><input id='TD_backqty" + num + "'type='number' value='" + leftqty + "' onchange='javascript:tdbackqtySelect(this)'></td >"//回收數量
                        + "<td><input id='TD_applyqty" + num + "'type='number' value='" + leftqty + "' onchange='javascript:tdchgSelect(this)' disabled='disabled'></td >"//入庫數量(手動)  readonly='readonly'
                        + "<td>" + stockqty + "</td>"
                        + "</tr>");
                } else {
                    //不顯示回收數量
                    $("#<%=divContent.ClientID %>").append("<tr>"
                        + "<td id='TD_no" + num + "'> " + item.no + "</td >"
                        + "<td>" + item.name + "</td>"
                        + "<td id='TD_qty" + num + "'>" + item.qty + "</td>"//申請數量
                        + "<td id='TD_sendqty" + num + "'>" + item.sendqty + "</td>"//已入庫數量
                        + "<td id='TD_leftqty" + num + "'>" + leftqty + "</td>"//剩餘數量                     
                        + "<td>" + chktransnew + "</td>"
                        + "<td>" + "</td>" //回收數量
                        + "<td><input id='TD_applyqty" + num + "'type='number' value='" + leftqty + "' onchange='javascript:tdchgSelect(this)'></td >"//入庫數量(手動)
                        + "<td>" + stockqty + "</td>"
                        + "</tr>");
                }

                //var obj = { "no": item.no, "name": item.name, "qty": item.qty, "sendqty": sendqty, "transnew": item.transnew, "applyqty": leftqty, "dept": item.dept, "FormInfoFormNo": ckformno, "backqty": leftqty };
                var obj = { "no": item.no, "name": item.name, "qty": item.qty, "sendqty": sendqty, "transnew": item.transnew, "applyqty": leftqty, "FormInfoFormNo": ckformno, "backqty": leftqty, "price": item.price};
                itemsarr.push(obj);
                 num++;
             }         
            $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
        })
    }

    // 更新數量
    function tdchgSelect(obj) {

        var tdapplyqty = $(obj).attr('id'); //入庫品號數量  
        var tdleftqty = "#TD_leftqty" + tdapplyqty.replace('TD_applyqty', '');//剩餘數量
        var tdsendqty = "#TD_sendqty" + tdapplyqty.replace('TD_applyqty', '');//已入庫數量
        var tdno = "#TD_no" + tdapplyqty.replace('TD_applyqty', '');//庫存品號    
        var tdbackqty = "#TD_backqty" + tdapplyqty.replace('TD_applyqty', '');//庫存品號    
        if (parseInt($("#" + tdapplyqty).val().trim()) > parseInt($(tdleftqty).text())) {
            alert("入庫數量不可大於剩餘數量!!!");
            $("#" + tdapplyqty).val("");
            return;
        }
        if (parseInt($("#" + tdapplyqty).val().trim()) > parseInt($(tdbackqty).val())) {
            alert("入庫數量不可大於回收數量!!!");  
            $("#" + tdapplyqty).val($(tdbackqty).val());
            return;
        }
        if (isNaN(parseInt($("#" + tdapplyqty).val())) || $("#" + tdapplyqty).val().trim() == null) {
            $("#" + tdapplyqty).val($(tdbackqty).val());
            return;
        }

        itemsarr.forEach(function (item) {
            if (item.no == $(tdno).text().trim()) {
                item.sendqty = parseInt($(tdsendqty).text().trim()) + parseInt($("#" + tdapplyqty).val().trim());//已入庫數量 =已入庫數量+申請數量            
                item.applyqty = $("#" + tdapplyqty).val().trim();//入庫數量                
                itemsarr[tdapplyqty.replace('TD_applyqty', '')] = item;
            }
        });

        $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
    }

    // 更新回收數量
    function tdbackqtySelect(obj) {
        var tdbackqty = $(obj).attr('id'); //回收數量  
        var tdqty = "#TD_qty" + tdbackqty.replace('TD_backqty', '');//申請數量
        var tdapplyqty = "#TD_applyqty" + tdbackqty.replace('TD_backqty', '');//入庫數量數量
        var tdno = "#TD_no" + tdbackqty.replace('TD_backqty', '');//庫存品號 
        var tdsendqty = "#TD_sendqty" + tdbackqty.replace('TD_backqty', '');//已入庫數量
        if (parseInt($("#" + tdbackqty).val()) > parseInt($(tdqty).text())) {
            alert("回收數量不可大於申請數量!!!");
            $("#" + tdbackqty).val("");
            return;
        }

        if (isNaN(parseInt($("#" + tdbackqty).val())) || $("#" + tdbackqty).val().trim() == null) {
            $("#" + tdbackqty).val($(tdapplyqty).val());
            return;
        }

        $(tdapplyqty).val($("#" + tdbackqty).val().trim());
        itemsarr.forEach(function (item) {
            if (item.no == $(tdno).text().trim()) {     
                item.sendqty = parseInt($(tdsendqty).text().trim()) + parseInt($(tdapplyqty).val());//已入庫數量 =已入庫數量+申請數量
                item.backqty = $("#" + tdbackqty).val().trim();//回收數量 
                item.applyqty = $(tdapplyqty).val();//入庫數量       
                itemsarr[tdbackqty.replace('TD_backqty', '')] = item;
            }
        });

        $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
    }

    //載入表單資料
    function loadSourceData() {
        var jsontext = $("#<%=hdContent.ClientID %>").val();
        var data = $.parseJSON(jsontext);
        $.each(data, function (i, item) {
            $("#FormNo").val(item.FormInfoFormNo);
            if ($("#FormNo").val() != "" && $("#FormNo").val() != "GA1006") {
                ckload = "Y";
                $("#btnSearch").trigger("click");
                return false;
            }
        });
    }

    function ConvertDate(dt) {
        dt = dt.replace('/Date(', '');
        dt = dt.replace(')/', '');
        var currentTime = new Date(parseInt(dt));
        var month = ("0" + (currentTime.getMonth() + 1)).slice(-2);
        var day = ("0" + currentTime.getDate()).slice(-2);
        var year = currentTime.getFullYear();
     
        return year + month + day;
    }


    //[ ajax_load_json ]
    function ajax_load_json(url, data_array, callback) {
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
                alert("載入失敗或無資料");
            }
        });
    }    
</script>
