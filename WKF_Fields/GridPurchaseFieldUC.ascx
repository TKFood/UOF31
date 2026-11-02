<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridPurchaseFieldUC.ascx.cs" Inherits="WKF_OptionalFields_GridPurchaseFieldUC" %>
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
        .form-control{width:100%;height:30px !important;}
</style>

<asp:Label Visible="true" runat="server" Text=""></asp:Label>

<div id="Block1">
<div class="row" style="width: 80%" id="divEdit" runat="server">
    <div class="col-lg-2 col-md-3 col-6">從領用單號載入</div>
    <div class="col-lg-2 col-md-6 col-6">
        <input type="text" id="FormNo" value="" placeholder="請輸入領用單號" class="form-control" />        
    </div>   
     <div class="col-lg-4 col-md-6 col-6">
        <button type="button" class="btn btn-info btn-sm" id="btnSearch">搜尋</button>
    </div>  
</div>
<hr />
<div class="row" style="width: 80%" id="div2" runat="server">
    <div class="col-lg-2 col-md-3 col-6">或直接新增</div>    
</div>
<div class="row" style="width: 80%" id="div1" runat="server">    
    <div class="col-lg-1 col-md-3 col-6">類別</div>
    <div class="col-lg-2 col-md-3 col-6">
        <select id="ItemCategory" class="form-control"></select></div>
    <div class="col-lg-1 col-md-3 col-6">品號</div>
    <div class="col-lg-3 col-md-3 col-6">
        <input type="text" id="ItemNo" value="" placeholder="請輸入品號或品名關鍵字或上下鍵選擇"  class="form-control" autocomplete="off" /></div>
    <div class="col-lg-1 col-md-3 col-6">數量</div>
    <div class="col-lg-1 col-md-3 col-6">
        <input type="number" id="Qty" value="1" class="form-control" style="width:100%" /></div>
    <div class="col-lg-3 col-md-6 col-12">
        <input type="button" class="btn btn-success btn-sm" value="新增" id="btnAdd" />
        <input type="hidden" value="" id="ItemID" />
    </div>
</div>
</div>

<table id="divContent" runat="server" class="mytable">
</table>

<div class="row" style="width: 80%">
     <div class="col-lg-12 col-md-12 col-12" >總價: <span id="talPrice"></span></div>
</div>
   
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
    var availableItems = [];
    var itemsmap = {};
    var specmap = {};
    var itemsarr = [];
    var itemstockmap = {};
    var itempricestockmap = {};//所有品號單價
    var itemfacstockmap = {};//所有品號廠商
    var talprice = 0;//總價
    var price = 0;//單價
    var facname = "";
    var bl = 0;

    $(document).ready(function () {
        if (bl == 1) {
            return;
        }
        else {
            bl = 1;
            var status = $("#<%=hdStatus.ClientID %>").val();
        
            if (status == "APPLY") {
                $("#<%=divContent.ClientID %>").append("<tr style='background-color:#0088A8'>" +
                    "<th style='width:15%;color:#fff;text-align:center'>品號</th>" +
                    "<th style='width:20%;color:#fff;text-align:center'>品名</th>" +
                    "<th style='width:20%;color:#fff;text-align:center'>規格</th>" +
                    "<th style='width:10%;color:#fff;text-align:center'>廠商</th>" +
                    "<th style='width:10%;color:#fff;text-align:center'>數量</th>" +
                    "<th style='width:5%;color:#fff;text-align:center'>單價</th>" +
                    "<th style='width:10%;color:#fff;text-align:center'>小計</th>" +
                    "<th style='width:10%;color:#fff;text-align:center'>刪除</th>" +
                    "</tr>");
            } else {             
                $("#<%=divContent.ClientID %>").append("<tr style='background-color:#0088A8'>" +
                    "<th style='width:15%;color:#fff;text-align:center'>品號</th>" +
                    "<th style='width:20%;color:#fff;text-align:center'>品名</th>" +
                    "<th style='width:20%;color:#fff;text-align:center'>規格</th>" +
                    "<th style='width:10%;color:#fff;text-align:center'>廠商</th>" +
                    "<th style='width:10%;color:#fff;text-align:center'>數量</th>" +
                    "<th style='width:5%;color:#fff;text-align:center'>單價</th>" +
                    "<th style='width:10%;color:#fff;text-align:center'>小計</th>" +
                    "<th style='width:10%;color:#fff;text-align:center'>總倉庫存</th>" +
                    "</tr>");
                $("#Block1").hide();
            }
            loadApplySourceData();
            loadAvailableItemPrice();
            //loadAvailableItemFacname();
            loadAvailableItemCategory();
        }
    });
       
    //領用單回傳資料
    function getDeptApplyStockCallback(response) {    
        var subtotal = 0;
        talprice = 0;
        itemsarr = [];
        $.each(JSON.parse(response), function (i, item) {            
            price = itempricestockmap[item.no];
            facname = itemfacstockmap[item.no];
            subtotal = parseInt(item.qty) * parseInt(price);         
            var obj = { "no": item.no, "name": item.name, "spec": item.spec, "qty": item.qty, "price": price, "subtotal": subtotal, "facname": facname };
            itemsarr.push(obj);
            $("#<%=divContent.ClientID %>").append("<tr id='" + item.no + "'>"
                +"<td>" + item.no + "</td>" 
                +"<td>" + item.name + "</td>"
                +"<td>" + item.spec + "</td>"
                +"<td>" + facname + "</td>"
                +"<td><input type='number' onchange='javascript:updateqty(\"" + item.no + "\",this.value)' value='" + item.qty + "'></td >"
                +"<td><input type='number' onchange='javascript:updateprice(\"" + item.no + "\",this.value)' value='" + price + "'></td>"
                +"<td id='Subtotal" + item.no + "'>" + subtotal + "</td>"
                +"<td style='text-align:center'><button onclick='javascript:delitem(\"" + item.no + "\")' class='btn btn-danger btn-sm'>刪除</button></td>"
                +"</tr>");
            talprice += parseInt(subtotal);
            $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
           
        })     
        $("#talPrice").html(0);
        $("#talPrice").html(talprice);
    }

    //載入資料
    function loadSourceData() {
        var jsontext = $("#<%=hdContent.ClientID %>").val();
        var data = $.parseJSON(jsontext);
        var status = $("#<%=hdStatus.ClientID %>").val();
        var item_no_list = "";
        var subtotal = 0;
        talprice = 0;
      
        $.each(data, function (i, item) {          
            var no = item.no;
            if (status == "APPLY") {               
              
                subtotal = parseInt(item.qty) * parseInt(item.price);                         
                var obj = { "no": item.no, "name": item.name, "spec": item.spec, "qty": item.qty, "price": item.price, "subtotal": subtotal, "facname": item.facname };
                itemsarr.push(obj);
                $("#<%=divContent.ClientID %>").append("<tr id='" + no + "'>"
                    +"<td>" + item.no + "</td>" 
                    +"<td>" + item.name + "</td>"
                    + "<td>" + item.spec + "</td>"
                    + "<td>" + item.facname + "</td>"
                    +"<td><input type='number' onchange='javascript:updateqty(\"" + no + "\",this.value)' value='" + item.qty + "'></td>"
                    + "<td><input type='number' onchange='javascript:updateprice(\"" + no + "\",this.value)' value='" + item.price + "'></td>"
                    +"<td id='Subtotal" + no + "'>" + subtotal + "</td>"                   
                    +"<td style ='text-align:center' > <button onclick='javascript:delitem(\"" + no + "\")' class='btn btn-danger btn-sm'>刪除</button></td>"
                    + "</tr>");
                talprice += parseInt(subtotal);
            } else {               
                item_no_list += item.no + ",";
            }
        });
        if (status == "APPLY") {
            $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
            $("#talPrice").html(0);
            $("#talPrice").html(talprice);
        } else {
            getStock(item_no_list);
        }
    }

    //載入領用單編號
    function loadApplySourceData() {
        ajax_load_json('/RM/DeptApply/GetFromNoList', null, loadApplySourceDataCallback);
    }

    //領用單回傳
    function loadApplySourceDataCallback(response) {
        fromnomap = [];        
        $.each(response, function (i, data) {
            if (i == 'data') {
                $.each(data, function (j, item) {
                    fromnomap.push(item.FORM_NO + " | " + item.DEPARTMENT_NAME + " | " + ConvertDate(item.CREATE_DATETIME).toString());
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
    }

    function getStock(str) {
        ajax_load_json("/RM/Item/GetStock?items=" + str, null, getStockCallback);
    }

    function getStockCallback(response) {
        itemstockmap = {};
        $.each(response, function (i, data) {
            if (i == 'data') {
                $.each(data, function (j, item) {
                    itemstockmap[item.ITEM_NO] = item.QUANTITY;//更新總倉數量
                });
            }
        });

        var jsontext = $("#<%=hdContent.ClientID %>").val();
        var data = $.parseJSON(jsontext);
        var stockqty = 0;
        talprice = 0;
        $.each(data, function (i, item) {
            var no = item.no;
            if (status != "APPLY") {
                stockqty = itemstockmap[no];
                if (stockqty == undefined) stockqty = 0;
                $("#<%=divContent.ClientID %>").append("<tr id='" + no + "'>"
                    + "<td>" + item.no + "</td >"
                    + "<td>" + item.name + "</td>"
                    + "<td>" + item.spec + "</td>"
                    + "<td>" + item.facname + "</td>"
                    + "<td style='text-align:center'>" + item.qty + "</td>"
                    + "<td>" + item.price + "</td>"
                    + "<td>" + item.subtotal + "</td>"               
                    + "<td style='text-align:center'>" + stockqty + "</td>"
                    + "</tr>");
            }
            talprice += parseInt(item.subtotal);
            var obj = { "no": item.no, "name": item.name, "spec": item.spec, "qty": item.qty, "price": item.price, "subtotal": item.subtotal, "facname": facname };
            itemsarr.push(obj);
        });       
        $("#talPrice").html(0);
        $("#talPrice").html(talprice);
    }

    //刪除事件
    function delitem(no) {
        var idx = 0;
        var subtotal = 0;
        talprice = 0;
        if (confirm("確定刪除此筆?")) {
            $('table#<%=divContent.ClientID %> tr#' + no).remove();
            // 從陣列中移除
            itemsarr = itemsarr.filter(item => item.no !== no);
            itemsarr.forEach(function (obj) {
                if (obj.no == no) {
                    //obj.qty = qty;
                    //obj.price = price;
                    //obj.price = itempricestockmap[no];
                    subtotal = parseInt(obj.qty) * parseInt(obj.price);
                    obj.subtotal = subtotal;
                    itemsarr[idx] = obj;
                    $("#Subtotal" + no).html(obj.subtotal);
                }
                talprice += parseInt(obj.subtotal);
                idx++;
            });          
            $("#talPrice").html(0);
            $("#talPrice").html(parseInt(talprice));
            $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
        }     
    }

    // 更新數量 
    function updateqty(no, qty) {
        if (isNaN(qty)) {
            alert("請確認為数字");
        } 
        var idx = 0;
        var subtotal = 0;
        talprice = 0;
        itemsarr.forEach(function (obj) {
            if (obj.no == no) {
                obj.qty = qty;
                subtotal = parseInt(qty) * parseInt(itempricestockmap[no]);
                obj.subtotal = subtotal;
                itemsarr[idx] = obj;
                $("#Subtotal" + no).html(obj.subtotal);
            }
            talprice += parseInt(obj.subtotal);
            idx++;
        });
     
        $("#talPrice").html(0);
        $("#talPrice").html(parseInt(talprice));
        $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
    }

    // 更新單價
    function updateprice(no, price) {
        if (isNaN(price)) {
            alert("請確認為数字");
        }
        var idx = 0;
        var subtotal = 0;
        talprice = 0;
        itemsarr.forEach(function (obj) {
            if (obj.no == no) {
                obj.price = price;
                itempricestockmap[no] = price;
                subtotal = parseInt(price) * parseInt(obj.qty);
                obj.subtotal = subtotal;
                itemsarr[idx] = obj;
                $("#Subtotal" + no).html(obj.subtotal);
            }
            talprice += parseInt(obj.subtotal);
            idx++;
        });
      
        $("#talPrice").html(0);
        $("#talPrice").html(parseInt(talprice));
        $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
    }
   
    // 檢查是否已加入
    function checknoexist(no) {
        var boolresult = false;
        itemsarr.forEach(function (obj) {
            if (obj.no == no) {
                alert(obj.name + "已加入!請勿重複選擇!");
                boolresult = true;
            }
        });
        return boolresult;
    }

    //領用單事件
    $("#FormNo").on("keydown", function (event) {
        //if (event.which == 13) {
        //    $("#btnSearch").trigger("click");
        //}
    });

    //領用單搜尋事件
    $("#btnSearch").click(function () {
        if ($("#FormNo").val() == "") {
            alert("請輸入領用單號");
        }
       
        if (ckformno != $("#FormNo").val()) {
            var formno = $("#FormNo").val();
            $("#<%=divContent.ClientID %>  tr:not(:first)").html("");
            ajax_load_json("/RM/UOFWKF/GetTask?no=" + formno, null, getDeptApplyStockCallback);
            $("#talPrice").html(0);
        }
        ckformno = $("#FormNo").val();
    });


    //新增鍵
    $("#btnAdd").click(function () {      
        if ($("#ItemNo").val() == "") {
            alert("請先輸入品號!");
            return;
        }

        var itemno_name = $("#ItemNo").val();
        var qty = $("#Qty").val();
        var no = itemno_name.substring(0, itemno_name.indexOf("("));
        var itemname = itemno_name.substring(
            itemno_name.indexOf("(") + 1,
            itemno_name.lastIndexOf(")")
        );
        var spec = specmap[no];
        var id = itemsmap[no];
        var subtotal = 0;
        if (checknoexist(no)) {
        } else {
            price = 0;
            price = parseInt(itempricestockmap[no]);
            facname = itemfacstockmap[no];
            subtotal = parseInt(qty) * price;
            $("#<%=divContent.ClientID %>").append("<tr id='" + no + "'>"
                + "<td>" + no + "</td >"
                + "<td>" + itemname + "</td>"
                + "<td>" + spec + "</td>"
                + "<td>" + facname + "</td>"
                + "<td><input type='number' value='" + qty + "' onchange='javascript:updateqty(\"" + no + "\",this.value)'></td>"
                + "<td><input type='number' onchange='javascript:updateprice(\"" + no + "\",this.value)' value='" + price + "'></td>"
                + "<td id='Subtotal" + no + "'>" + subtotal + "</td>"             
                + "<td style='text-align:center'><button onclick='javascript:delitem(\"" + no + "\")' class='btn btn-danger btn-sm'>刪除</button></td>"
                + "</tr>");
            $("#Qty").val("1");         
            var obj = { "no": no, "name": itemname, "spec": spec, "qty": qty, "price": price, "subtotal": subtotal, "facname": facname};
            itemsarr.push(obj);
            $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
        }
    
        if ($("#talPrice").html() == "") {
            $("#talPrice").html(0);
        }
        $("#talPrice").html(parseInt($("#talPrice").html()) + parseInt(subtotal));
        $("#ItemNo").val("");
        return false;
    });

    //類別鍵事件
    $("#ItemCategory").change(function () {
        var id = this.value;
        loadAvailableItems(id);
        $("#ItemNo").val("");
    });

    $("#ItemNo").on("keydown", function (event) {
        if (event.which == 13) {
            var itemno_name = $("#ItemNo").val();
            var no = itemno_name.substring(0, itemno_name.indexOf("("));
            var id = itemsmap[no];
            if (id == null) {
                alert("該品號不存在!請重新輸入或選擇");
                $("#ItemNo").val("");
                $("#ItemNo").focus();
            } else {
            }
        }
        return true;
    });

    $("#ItemNo").on("blur", function (event) {
        if (this.value != "") {
            var itemno_name = $("#ItemNo").val();
            var no = itemno_name.substring(0, itemno_name.indexOf("("));
            var id = itemsmap[no];
            if (id == null) {
                alert("該品號不存在!請重新輸入或選擇");
                $("#ItemNo").val("");
                $("#ItemNo").focus();
            } else {
                $("#ItemID").val(id);
            }
        }
    });

    //載入品號單價
    function loadAvailableItemPrice() {
        ajax_load_json('/RM/Item/GetPriceList', null, loadAvailableItemPriceCallback);
    }

    //品號單價回傳
    function loadAvailableItemPriceCallback(response) {
        itempricestockmap = {};
        $.each(response, function (i, data) {
            if (i == 'data') {
                $.each(data, function (j, item) {
                    itempricestockmap[item.ITEM_NO] = item.PRICE;//單價
                });
            }
        });
        loadAvailableItemFacname();
    }

    //載入品號廠商
    function loadAvailableItemFacname() {
        ajax_load_json('/RM/Item/GetFacnameList', null, loadAvailableItemFacnameCallback);
    }

    //品號廠商回傳
    function loadAvailableItemFacnameCallback(response) {
        itemfacstockmap = {};
        $.each(response, function (i, data) {
            if (i == 'data') {
                $.each(data, function (j, item) {
                    itemfacstockmap[item.ITEM_NO] = item.MANUFACTURER_NAME;//廠商
                });
            }
        });
        loadSourceData();
    }

    //載入類別資料
    function loadAvailableItemCategory() {
        ajax_load_json('/RM/ItemCategory/GetList', null, loadAvailableItemCategoryCallback);
    }

    //類別資料回傳
    function loadAvailableItemCategoryCallback(response) {
        $.each(response, function (i, data) {
            if (i == 'data') {
                $('#ItemCategory').append($('<option>', {
                    value: 0,
                    text: "請選擇"
                }));
                $.each(data, function (j, item) {
                    $('#ItemCategory').append($('<option>', {
                        value: item.CATEGORY_ID,
                        text: item.CATEGORY_NAME
                    }));
                });
            }
        });
    }

    //載入類別資料
    function loadAvailableItems(id) {
        ajax_load_json('/RM/Item/GetListByCategory/' + id, null, loadAvailableItemsCallback);
    }

    //回傳類別資料
    function loadAvailableItemsCallback(response) {
        availableItems = [];
        itemsmap = {};
        specmap = {};
        $.each(response, function (i, data) {
            if (i == 'data') {
                $.each(data, function (j, item) {
                    availableItems.push(item.ITEM_NO + "(" + item.ITEM_NAME + ")");
                    itemsmap[item.ITEM_NO] = item.ITEM_ID;
                    specmap[item.ITEM_NO] = item.SPECIFICATION;
                });

                $("#ItemNo").autocomplete({
                    source: availableItems,
                    minLength: 0
                });
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
