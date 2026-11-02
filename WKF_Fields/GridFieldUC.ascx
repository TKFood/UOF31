<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridFieldUC.ascx.cs" Inherits="WKF_OptionalFields_GridFieldUC" %>
<%@ Reference Control="~/WKF/FormManagement/VersionFieldUserControl/VersionFieldUC.ascx" %>
<link rel="canonical" href="https://getbootstrap.com/docs/4.4/components/modal/">
<!-- Bootstrap core CSS -->
<link href="https://getbootstrap.com/docs/4.4/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
<style type="text/css">
.mytable {
    border-collapse: collapse;
    width:100%;
    margin-top:20px;
    margin-bottom:20px;
    padding:30px;
}

.mytable, .mytable th,.mytable td {
    border: 1px solid #808080;
    color:#403d3d;
    height:26px;
    line-height:26px;
    padding:5px;
}
</style>

<asp:Label Visible="true" runat="server" Text=""></asp:Label>

<div class="row" style="width:80%" id="divEdit" runat="server">
    <div class="col-lg-1 col-md-3 col-6">類別</div>
    <div class="col-lg-2 col-md-3 col-6"><select id="ItemCategory"></select></div>
    <div class="col-lg-1 col-md-3 col-6">品號</div>
    <div class="col-lg-2 col-md-3 col-6"><input type="text" id="ItemNo" value="" /></div>
    <div class="col-lg-1 col-md-3 col-6">數量</div>
    <div class="col-lg-2 col-md-3 col-6"><input type="number" id="Qty" value="1" /></div>
    <div class="col-lg-3 col-md-6 col-12">
        <input type="button" class="btn btn-success btn-sm" value="新增" id="btnAdd" />
        <input type="hidden" value="" id="ItemID" />
    </div>
</div>


<table ID="divContent" runat="server" class="mytable">

</table>
<!-- 沒用到 -->
<asp:TextBox ID="SiteID" type="hidden" runat="server"></asp:TextBox>
<!-- 沒用到 -->
<asp:HiddenField ID="hdSource" runat="server"></asp:HiddenField>

<asp:HiddenField ID="hdContent" runat="server" />
<asp:HiddenField ID="hdStatus" runat="server" />

<!-- 沒用到 -->
<asp:HiddenField ID="hdIsAudit" Value="N" runat="server" />


<asp:Label ID="lblHasNoAuthority" runat="server" Text="無填寫權限" ForeColor="Red" Visible="False" meta:resourcekey="lblHasNoAuthorityResource1"></asp:Label>
<asp:Label ID="lblToolTipMsg" runat="server" Text="不允許修改(唯讀)" Visible="False" meta:resourcekey="lblToolTipMsgResource1"></asp:Label>
<asp:Label ID="lblModifier" runat="server" Visible="False" meta:resourcekey="lblModifierResource1"></asp:Label>
<asp:Label ID="lblMsgSigner" runat="server" Text="填寫者" Visible="False" meta:resourcekey="lblMsgSignerResource1"></asp:Label>
<asp:Label ID="lblAuthorityMsg" runat="server" Text="具填寫權限人員" Visible="False" meta:resourcekey="lblAuthorityMsgResource1"></asp:Label>

<!-- Button trigger modal -->
<!--
<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#staticBackdrop">
  Launch static backdrop modal
</button>-->

<!-- Modal -->
<!--
<div class="modal fade" id="staticBackdrop" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="staticBackdropLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="staticBackdropLabel">Modal title</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        ...
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary">Understood</button>
      </div>
    </div>
  </div>
</div>
-->

<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js"></script>

<style>
    ul {
      list-style-type: none;
    }
    .ui-autocomplete{
        border:1px solid #666;
        display:inline-block;
        z-index:9999;
        background-color:#fff;
    }
    .ui-menu-item{
        background-color:#fff;
        color:#0094ff;
    }
    .ui-state-focus{
        background-color:#0094ff;
        color:#fff;
        display:block;
    }
    .btn-danger{
        color:#fff;
    }
    .redtext{ 
         color:#ff0000;
    }
</style>

<script>
    var availableItems = [];
    var itemsmap = {};
    var specmap = {};
    var transnewmap = {};
    var itemsarr = [];
    var itemstockmap = {};
    
    $(document).ready(function () {        
        setReasonRedText();
        var status = $("#<%=hdStatus.ClientID %>").val();
        if (status == "APPLY") {
            $("#<%=divContent.ClientID %>").append("<tr style='background-color:#0088A8'><th style='width:20%;color:#fff;text-align:center'>品號</th><th style='width:20%;color:#fff;text-align:center'>品名</th><th style='width:30%;color:#fff;text-align:center'>規格</th><th style = 'width:10%;color:#fff;text-align:center'> 以舊換新</th><th style='width:10%;color:#fff;text-align:center'>數量</th><th style='width:10%;color:#fff;text-align:center'>刪除</th></tr>");
        } else {
            $("#<%=divContent.ClientID %>").append("<tr style='background-color:#0088A8'><th style='width:20%;color:#fff;text-align:center'>品號</th><th style='width:20%;color:#fff;text-align:center'>品名</th><th style='width:30%;color:#fff;text-align:center'>規格</th><th style = 'width:10%;color:#fff;text-align:center'> 以舊換新</th><th style='width:10%;color:#fff;text-align:center'>數量</th><th style='width:10%;color:#fff;text-align:center'>總倉庫存</th></tr>");
        }
        loadAvailableItemCategory();
        loadSourceData();
    });

    function loadSourceData() {
        var jsontext = $("#<%=hdContent.ClientID %>").val();
        var data = $.parseJSON(jsontext);
        var status = $("#<%=hdStatus.ClientID %>").val();
        var item_no_list = "";
        $.each(data, function (i, item) {
            var obj = { "no": item.no, "name": item.name, "spec": item.spec, "qty": item.qty, "transnew": item.transnew };
            itemsarr.push(obj);
            var no = item.no;
            if (status == "APPLY") {
                if (item.transnew == "0") {
                    //以舊換新
                    chktransnew = "是";
                } else {
                    chktransnew = "否";
                }
                $("#<%=divContent.ClientID %>").append("<tr id='" + no + "'><td>" + item.no + "</td><td>" + item.name + "</td><td>" + item.spec + "</td><td>" + chktransnew + "</td><td><input type='number'onchange='javascript:updateqty(\"" + no + "\",this.value)' value='" + item.qty + "'></td>><td style='text-align:center'><button onclick='javascript:delitem(\"" + no + "\")' class='btn btn-danger btn-sm'>刪除</button></td></tr>");
            } else {
                item_no_list += item.no + ",";
            }
        });
        if (status == "APPLY") {
            $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
        } else {
            getStock(item_no_list);
            //alert(item_no_list);
        }
    }

    function getStock(str) {        
        ajax_load_json("/RM/Item/GetStock?items="+str, null, getStockCallback);
    }

    function getStockCallback(response) {
        
        itemstockmap = {};
        $.each(response, function (i, data) {
            if (i == 'data') {
                $.each(data, function (j, item) {
                    itemstockmap[item.ITEM_NO] = item.QUANTITY;                    
                });
            }
        });

        var jsontext = $("#<%=hdContent.ClientID %>").val();
        var data = $.parseJSON(jsontext);
        var stockqty = 0;
        $.each(data, function (i, item) {         
            var obj = { "no": item.no, "name": item.name, "spec": item.spec, "qty": item.qty, "transnew": item.transnew };
            itemsarr.push(obj);
            var no = item.no;
            if (item.transnew == "0") {
                //以舊換新
                chktransnew = "是";
            } else {
                chktransnew = "否";
            }
            if (status != "APPLY") {
                stockqty = itemstockmap[no];
                if (stockqty == undefined) stockqty = 0;
                $("#<%=divContent.ClientID %>").append("<tr id='" + no + "'><td>" + item.no + "</td><td>" + item.name + "</td><td>" + item.spec + "</td><td>" + chktransnew + "</td><td style='text-align:center'>" + item.qty + "</td><td style='text-align:center'>" + stockqty + "</td></tr>");               
            }
        });
    }

    function delitem(no) {
        if (confirm("確定刪除此筆?")) {
            $('table#<%=divContent.ClientID %> tr#' + no).remove();
            
            // 從陣列中移除
            itemsarr = itemsarr.filter(item => item.no !== no);
            $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
        }
    }    

    // 更新數量
    function updateqty(no, qty) {
        var idx = 0;
        itemsarr.forEach(function (obj) {
            if (obj.no == no) {
                obj.qty = qty;
                itemsarr[idx] = obj;
            }
            idx++;
        });
        $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
    }

    //$("input[type='number']").bind("input", function () {
    //$(".num").bind("input", function () {
        //alert("Value changed");
    //});
    // 檢查是否已加入
    function checknoexist(no) {
        var boolresult = false;
        itemsarr.forEach(function (obj) {
            if (obj.no == no) {
                alert(obj.name + "已加入!請勿重複選擇!");
                boolresult= true;
            }
        });
        return boolresult;
    }

    $("#btnAdd").click(function () {
        if ($("#ItemNo").val() == "") {
            alert("請先輸入品號!");
            return;
        }

        var itemno_name = $("#ItemNo").val();
        var qty = $("#Qty").val();
        var no = itemno_name.substring(0, itemno_name.lastIndexOf("("));
        var itemname = itemno_name.substring(
            itemno_name.lastIndexOf("(") + 1,
            itemno_name.lastIndexOf(")")
        );
        var spec = specmap[no];
        var id = itemsmap[no];
        var transnew = transnewmap[no];

        if (checknoexist(no)) {
        } else {
            if (transnew == "0") {
                //以舊換新
                chktransnew = "是";
            } else {
                chktransnew = "否";
            }
            $("#<%=divContent.ClientID %>").append("<tr id='" + no + "'><td>" + no + "</td><td>" + itemname + "</td><td>" + spec + "</td><td>" + chktransnew + "</td><td><input type='number' value='" + qty + "' onchange='javascript:updateqty(\"" + no + "\",this.value)'></td><td style='text-align:center'><button onclick='javascript:delitem(\"" + no + "\")' class='btn btn-danger btn-sm'>刪除</button></td></tr>");
            $("#Qty").val("1");
            //////////////////////////////
            var obj = { "no": no, "name": itemname, "spec": spec, "qty": qty, "transnew": transnew };
            itemsarr.push(obj);

            $("#<%=hdContent.ClientID %>").val(JSON.stringify(itemsarr));
        }
        
        $("#ItemNo").val("");

        return false;

    });

    function loadAvailableItemCategory() {
        ajax_load_json('/RM/ItemCategory/GetList', null, loadAvailableItemCategoryCallback);
    }

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

    $("#ItemCategory").change(function () {
        var id = this.value;
        loadAvailableItems(id);
    });

    function loadAvailableItems(id) {       
        ajax_load_json('/RM/Item/GetListByCategory/'+id, null, loadAvailableItemsCallback);
    }

    function loadAvailableItemsCallback(response) {
        availableItems = [];
        itemsmap = {};
        specmap = {};
        transnewmap = {};
        $.each(response, function (i, data) {
            if (i == 'data') {
                $.each(data, function (j, item) {                   
                    availableItems.push(item.ITEM_NO + "(" + item.ITEM_NAME +")");
                    itemsmap[item.ITEM_NO] = item.ITEM_ID;
                    specmap[item.ITEM_NO] = item.SPECIFICATION;
                    transnewmap[item.ITEM_NO] = item.TRANSNEW;
                });
                
                $("#ItemNo").autocomplete({
                    source: availableItems,
                    minLength: 0
                });
            }            
        });
    }

    $("#ItemNo").on("keydown", function (event) {
        if (event.which == 13) {
            //alert("cxc");
            //$("#Qty").focus();
            var itemno_name = $("#ItemNo").val();
            var no = itemno_name.substring(0, itemno_name.lastIndexOf("("));
            var id = itemsmap[no];
            if (id == null) {
                alert("該品號不存在!請重新輸入或選擇");
                $("#ItemNo").val("");
                $("#ItemNo").focus();
            } else {
                //$("#ItemID").val(id);
                //return true;
            }
        }
        return true;

    });

    $("#ItemNo").on("blur", function (event) {
        if (this.value != "") {
            var itemno_name = $("#ItemNo").val();
            var no = itemno_name.substring(0, itemno_name.lastIndexOf("("));
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
    
    //[ ajax_load_json ]
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

    function setReasonRedText() {
        $(".TitleFont").each(function (index) {
            if ($(this).text() == "領用事由") {
                $(this).addClass("redtext");
            }
        });
    }
</script>