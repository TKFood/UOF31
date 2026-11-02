<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DLLConfigManager.aspx.cs" MasterPageFile="~/Master/DefaultMasterPage.master" Inherits="CDS_SCSHR_WebPages_DLLConfigManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        div.box {
            width: 1140px;
            margin: auto;
            max-width: 100%;
        }

            div.box > div:nth-child(2) {
                background: -webkit-linear-gradient(#e8f2ff,#aacfff);
                background: -o-linear-gradient(#e8f2ff,#aacfff);
                background: -moz-linear-gradient(#e8f2ff,#aacfff);
                background: linear-gradient(#e8f2ff,#aacfff);
            }

            div.box > div {
                padding: 5px 20px;
                border: 1px solid #336699;
                border-top: none;
                background-color: #e8f2ff;
            }

                div.box > div:first-child {
                    border-top: 1px solid #336699;
                }

                    div.box > div:first-child > select {
                        margin: 0px 5px 7px -5px;
                        padding: 5px;
                    }
                /*
                div.box > div:nth-last-child(1) {
                    position: fixed;
                    width: 1140px;
                    max-width: 100%;
                    bottom: 0;
                    padding: 10px 20px;
                    border: 1px solid #336699;
                }
                */

                div.box > div:nth-last-child(2) {
                    padding-bottom: 30px;
                }

                div.box > div:nth-last-child(1) > input[type=submit] {
                    color: #fff;
                    background-color: #5b98e6;
                    border-color: #4d6584;
                    padding: 4px 10px;
                    border-radius: 5px;
                }

                    div.box > div:nth-last-child(1) > input[type=submit]:hover {
                        background-color: #0069d9;
                        border-color: #0062cc;
                    }

        div.PageSelect {
            display: flex;
        }

            div.PageSelect > label {
                border: 1px solid #5b98e6;
                border-radius: 5px 5px 0 0;
                padding: 5px 20px;
            }

        div.box > div.PageBox {
            background-color: #f7fbff;
        }

        div.PageBox > input#RadioText:checked ~ div#PageText,
        div.PageBox > input#RadioBoolean:checked ~ div#PageBoolean,
        div.PageBox > input#RadioJson:checked ~ div#PageJson,
        div.PageBox > input#RadioNumber:checked ~ div#PageNumber,
        div.PageBox > input#RadioDateTime:checked ~ div#PageDateTime,
        div.PageBox > input#RadioList:checked ~ div#PageList {
            display: block;
        }

        div.PageBox > input#RadioText:checked ~ div.PageSelect > label:nth-child(1),
        div.PageBox > input#RadioBoolean:checked ~ div.PageSelect > label:nth-child(2),
        div.PageBox > input#RadioJson:checked ~ div.PageSelect > label:nth-child(3),
        div.PageBox > input#RadioNumber:checked ~ div.PageSelect > label:nth-child(4),
        div.PageBox > input#RadioDateTime:checked ~ div.PageSelect > label:nth-child(5),
        div.PageBox > input#RadioList:checked ~ div.PageSelect > label:nth-child(6) {
            color: #fff;
            background-color: #5b98e6;
            border-color: #4d6584;
            border-bottom: none;
            margin-bottom: -2px;
        }

        div.PageDiv {
            border: 1px solid #4d6584;
            display: flex;
            justify-content: center;
            align-items: center;
            display: none;
        }

            div.PageDiv table {
                flex: 100%;
                max-width: 100%;
            }

            div.PageDiv tr > th {
                background-color: #5b98e6;
                color: white;
                vertical-align: middle;
                border: 1px solid #4d6584;
                border-left: none;
                height: 25px;
                padding: 5px;
                white-space: nowrap;
            }

            div.PageDiv table, div.PageDiv table td input {
                width: 100%;
                text-align: center;
            }

                div.PageDiv table > tbody > tr:nth-child(even) > td {
                    background-color: white;
                }

                div.PageDiv table > tbody > tr:nth-child(odd) > td {
                    background-color: #f3f8ff;
                }

                div.PageDiv table > tbody > tr > td {
                    border: 1px solid #a8adb3;
                }

                    div.PageDiv table > tbody > tr > td > input {
                        box-shadow: none;
                        border: none;
                        padding: 5px;
                        background-color: rgba(255,255,255,0);
                    }

                        div.PageDiv table > tbody > tr > td > input:not([readonly]):focus {
                            border: 1px solid #788ca2;
                            box-shadow: inset 0px 0px 8px 0px #4f98ff;
                            margin: -1px;
                        }

                        div.PageDiv table > tbody > tr > td > input:not([readonly]) {
                            color: firebrick;
                        }

            /* Boolean */
            div.PageDiv.Bool table > tbody > tr > td {
                width: 50%;
            }

                div.PageDiv.Bool table > tbody > tr > td:nth-child(2) > div {
                    display: flex;
                    justify-content: center;
                    align-items: center;
                }

            div.PageDiv.Bool input[type=submit] {
                width: 100px;
                border: none;
                background-color: rgba(255,255,255,0);
                padding: 3px 0px;
                margin: 5px 0px;
                border-radius: 10px;
            }

                div.PageDiv.Bool input[type=submit]:first-child.selected {
                    background-color: #99ff98;
                }

                div.PageDiv.Bool input[type=submit]:nth-child(2).selected {
                    background-color: #ff7b7b;
                }
            /* DateTime */

            div.PageDiv.DateTime > div > table > tbody > tr > td {
                width: 50%;
            }

                div.PageDiv.DateTime > div > table > tbody > tr > td > div {
                    display: inline-flex;
                    padding: 2px 0;
                    width: 100%;
                    justify-content: center;
                    align-items: center;
                    min-height: 30px;
                }

                    div.PageDiv.DateTime > div > table > tbody > tr > td > div > span:nth-child(6) {
                        width: 20px;
                    }

                    div.PageDiv.DateTime > div > table > tbody > tr > td > div > input {
                        width: 40px;
                        border: none;
                        background-color: rgba(255,255,255,0);
                        color: firebrick;
                    }

                        div.PageDiv.DateTime > div > table > tbody > tr > td > div > input:not(:first-child) {
                            width: 25px;
                        }

                        div.PageDiv.DateTime > div > table > tbody > tr > td > div > input:hover {
                            box-shadow: 0px 0px 5px 0px #4f98ff;
                        }

                        div.PageDiv.DateTime > div > table > tbody > tr > td > div > input:focus {
                            border: 1px solid #788ca2;
                            box-shadow: inset 0px 0px 5px 0px #4f98ff;
                        }

        div.date > span:nth-child(6),
        div.date > input:nth-child(7),
        div.date > span:nth-child(8),
        div.date > input:nth-child(9) {
            display: none;
        }

        div.time > input:nth-child(1),
        div.time > span:nth-child(2),
        div.time > input:nth-child(3),
        div.time > span:nth-child(4),
        div.time > input:nth-child(5),
        div.time > span:nth-child(6) {
            display: none;
        }

        /* Json */
        div.PageDiv.Json > div {
            display: flex;
        }

            div.PageDiv.Json > div > div:first-child {
                flex-basis: 30%;
                border-right: 1px solid #a8adb3;
                background-color: white;
            }

            div.PageDiv.Json > div > div:nth-child(2) {
                flex-basis: 70%;
                position: relative;
            }

            div.PageDiv.Json > div > div > table {
            }

                div.PageDiv.Json > div > div > table td {
                    background-color: white;
                    border: none;
                }

                    div.PageDiv.Json > div > div > table td input {
                        color: black;
                        padding: 10px 0;
                    }

                        div.PageDiv.Json > div > div > table td input.Selected {
                            border: 1px solid #4d6584;
                            color: white;
                            background-color: #0069d9;
                        }

            div.PageDiv.Json > div > div.RightCol > div > table {
                /* border: 1px solid black; */
                margin-right: 0px;
                padding: 0px;
            }

                div.PageDiv.Json > div > div.RightCol > div > table td {
                    border: none;
                    display: flex;
                    position: relative;
                }

                    div.PageDiv.Json > div > div.RightCol > div > table td input:first-child {
                        flex-basis: 7%;
                        border: 1px solid #4d6584;
                        color: white;
                        margin: 7px 20px;
                        padding: 2px 0px;
                        border-radius: 5px;
                        background-color: #5b98e6;
                    }

                        div.PageDiv.Json > div > div.RightCol > div > table td input:first-child:hover {
                            color: #fff;
                            background-color: #0069d9;
                            border-color: #0062cc;
                        }

                    div.PageDiv.Json > div > div.RightCol > div > table td input:nth-child(2) {
                        flex-basis: 25%;
                    }

                    div.PageDiv.Json > div > div.RightCol > div > table td input:nth-child(3) {
                        flex-basis: 65%;
                    }

                    div.PageDiv.Json > div > div.RightCol > div > table td input[type=text] {
                        margin: 5px 10px;
                        padding: 0px;
                    }

                        div.PageDiv.Json > div > div.RightCol > div > table td input[type=text]:hover {
                            box-shadow: 1px 1px 4px 0px #5b98e6;
                        }

            div.PageDiv.Json > div > div.RightCol > div.btns {
                display: flex;
                justify-content: center;
                align-items: center;
                height: 50px;
            }

                div.PageDiv.Json > div > div.RightCol > div.btns > input {
                    color: #fff;
                    background-color: #5b98e6;
                    border-color: #4d6584;
                    border: 1px solid #4d6584;
                    border-radius: 5px;
                    padding: 3px 7px;
                }

                    div.PageDiv.Json > div > div.RightCol > div.btns > input:hover {
                        color: #fff;
                        background-color: #0069d9;
                        border-color: #0062cc;
                    }

            div.PageDiv.Json > div > div.RightCol > div > table td > div.btnJsonItem > input {
                border: 1px solid #4d6584;
                color: white;
                margin: 7px 20px;
                padding: 2px 0px;
                border-radius: 5px;
                background-color: #30517b;
                position: absolute;
                display: block;
                width: 90px;
            }

                div.PageDiv.Json > div > div.RightCol > div > table td > div.btnJsonItem > input:hover {
                    color: #fff;
                    background-color: #0069d9;
                    border-color: #0062cc;
                }

                div.PageDiv.Json > div > div.RightCol > div > table td > div.btnJsonItem > input:first-child,
                div.PageDiv.Json > div > div.RightCol > div > table td > div.btnJsonItem > input:nth-child(2) {
                    top: 21px;
                    right: -1px;
                }

                div.PageDiv.Json > div > div.RightCol > div > table td > div.btnJsonItem > input:first-child {
                    width: 50px;
                    background-color: #8e1212;
                }

                    div.PageDiv.Json > div > div.RightCol > div > table td > div.btnJsonItem > input:first-child:hover {
                        background-color: #c71c1c;
                    }

            div.PageDiv.Json > div > div.RightCol > div > table td > div:first-child {
                width: 100%;
                margin: 25px 15px;
                /* border: 15px solid #e8f2ff; */
                /* border-radius: 20px; */
            }

            div.PageDiv.Json > div > div.RightCol > div > table > tbody > tr:nth-child(even) > td > div:first-child {
                /* background-color: azure; */
                box-shadow: 2px 2px 3px 0px;
            }

            div.PageDiv.Json > div > div.RightCol > div > table > tbody > tr:nth-child(odd) > td > div:first-child {
                /* background-color: floralwhite; */
                box-shadow: 1px 1px 3px 0px;
            }

        label.JsonEditBtn {
            position: absolute;
            right: 10px;
            top: 4px;
            color: white;
        }

            label.JsonEditBtn:before {
                content: "+";
            }

        input#JsonEditchk:checked + label.JsonEditBtn {
            color: #5f1414;
        }

            input#JsonEditchk:checked + label.JsonEditBtn:before {
                content: "-";
            }

        input#JsonEditchk:checked ~ div > table div.btnJsonItem > input:nth-child(2) {
            display: none;
        }
        /* List */
        div.PageDiv.Json.List > div > div.RightCol > div > table td input:nth-child(2) {
            flex-basis: 90%;
        }
        /* Other */
        .hidden {
            display: none !important;
        }
    </style>
    <script>
        $(function () {
            ClickLimit();
            $(document).keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });
        });
        function ClickLimit() {
            $(".NumberOnly").keypress(function (evt) {
                return isNumberKey(evt);
            });
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        var PageRadio;
        //Register Begin Request and End Request 
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Get The Div Scroll Position
        function BeginRequestHandler(sender, args) {
            var elements = document.getElementsByName("PageRadio");
            for (var i = 0, len = elements.length; i < len; ++i) {
                if (elements[i].checked) {
                    PageRadio = elements[i].id;
                    return;
                }
            }
        }
        //Set The Div Scroll Position
        function EndRequestHandler(sender, args) {
            var elements = document.getElementsByName("PageRadio");
            for (var i = 0, len = elements.length; i < len; ++i) {
                elements[i].checked = elements[i].id == PageRadio;
            }
            ClickLimit();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="box">
                <div>
                    <asp:DropDownList ID="ddlAllDDLConfig" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAllDDLConfig_SelectedIndexChanged"></asp:DropDownList>
                    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Font-Size="XX-Large"></asp:Label>

                </div>
                <div>
                    <asp:Label ID="lblConfigName" runat="server" Text=""></asp:Label>
                </div>
                <div class="PageBox">
                    <input type="radio" id="RadioText" name="PageRadio" hidden checked />
                    <input type="radio" id="RadioBoolean" name="PageRadio" hidden />
                    <input type="radio" id="RadioJson" name="PageRadio" hidden />
                    <input type="radio" id="RadioNumber" name="PageRadio" hidden />
                    <input type="radio" id="RadioDateTime" name="PageRadio" hidden />
                    <input type="radio" id="RadioList" name="PageRadio" hidden />
                    <div class="PageSelect">
                        <label runat="server" id="LabelText" class="hidden" for="RadioText">Text</label>
                        <label runat="server" id="LabelBoolean" class="hidden" for="RadioBoolean">Boolean</label>
                        <label runat="server" id="LabelJson" class="hidden" for="RadioJson">Json</label>
                        <label runat="server" id="LabelNumber" class="hidden" for="RadioNumber">Number</label>
                        <label runat="server" id="LabelDateTime" class="hidden" for="RadioDateTime">DateTime</label>
                        <label runat="server" id="LabelList" class="hidden" for="RadioList">List</label>
                    </div>
                    <div id="PageText" class="PageDiv">
                        <asp:GridView runat="server" ID="gvItemsText"
                            AutoGenerateColumns="false"
                            ShowHeader="true">
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                <asp:Label ID="Label03" runat="server" ForeColor="Red" Font-Bold="true" Text="請選擇資料類型"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Key">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtKEY" ReadOnly="true" Text='<%#Bind("NAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtVALUE" FieldName="VALUE" Text='<%#Bind("VALUE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="PageBoolean" class="PageDiv Bool">
                        <asp:GridView runat="server" ID="gvItemsBoolean"
                            AutoGenerateColumns="false"
                            ShowHeader="true"
                            OnRowDataBound="gvItemsBoolean_RowDataBound">
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                <asp:Label ID="Label03" runat="server" ForeColor="Red" Font-Bold="true" Text="請選擇資料類型"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Key">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtKEY" ReadOnly="true" Text='<%#Bind("NAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value">
                                    <ItemTemplate>
                                        <div>
                                            <asp:Button runat="server" ID="btnChangeA" Text="是" OnClick="btnBoolChange_Click" />
                                            <asp:Button runat="server" ID="btnChangeB" Text="否" OnClick="btnBoolChange_Click" />
                                        </div>
                                        <asp:TextBox runat="server" ID="txtVALUE" CssClass="hidden" Text='<%#Bind("VALUE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="PageJson" class="PageDiv Json">
                        <div>
                            <asp:GridView runat="server" ID="gvItemsJson"
                                AutoGenerateColumns="false"
                                ShowHeader="true">
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <asp:Label ID="Label03" runat="server" ForeColor="Red" Font-Bold="true" Text="請選擇資料類型"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="Title">
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnTITLE" Text='<%#Bind("NAME")%>' OnClick="btnJSONTITLE_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="RightCol">
                                <input type="checkbox" id="JsonEditchk" style="display: none;" />
                                <label runat="server" id="lblEditBtn" for="JsonEditchk" class="JsonEditBtn">編輯</label>
                                <asp:GridView runat="server" ID="gvItemsJson_Sub"
                                    AutoGenerateColumns="false"
                                    ShowHeader="true">
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="Label03" runat="server" ForeColor="Red" Font-Bold="true" Text="目前無相關資料"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Content">
                                            <ItemTemplate>
                                                <asp:GridView runat="server" ID="gvItemsJson_Item" CssClass="divJsonItem"
                                                    AutoGenerateColumns="false"
                                                    ShowHeader="true">
                                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="Label03" runat="server" ForeColor="Red" Font-Bold="true" Text="目前無相關資料"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Json物件">
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnJsonItemDelete" Text="刪除" OnClick="btnJsonItemDelete_Click" />
                                                                <asp:TextBox runat="server" ID="txtKEY" Text='<%#Bind("KEY")%>' />
                                                                <asp:TextBox runat="server" ID="txtVALUE" Text='<%#Bind("VALUE")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <div class="btnJsonItem">
                                                    <asp:Button runat="server" ID="btnJsonSubDelete" Text="刪除" OnClick="btnJsonSubDelete_Click" />
                                                    <asp:Button runat="server" ID="btnJsonItemNew" Text="新增一筆" OnClick="btnJsonItemNew_Click" />
                                                </div>
                                                <asp:HiddenField runat="server" ID="JsonItemIndex" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="btns">
                                    <asp:Button runat="server" ID="btnJsonSubNew" Text="新增Json" OnClick="btnJsonSubNew_Click" />
                                    <asp:Button runat="server" ID="btnJsonSubNew2" Text="複製Json" OnClick="btnJsonSubNew2_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="PageNumber" class="PageDiv">
                        <asp:GridView runat="server" ID="gvItemsNumber"
                            AutoGenerateColumns="false"
                            ShowHeader="true">
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                <asp:Label ID="Label03" runat="server" ForeColor="Red" Font-Bold="true" Text="請選擇資料類型"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Key">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtKEY" ReadOnly="true" Text='<%#Bind("NAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtVALUE" FieldName="VALUE" Text='<%#Bind("VALUE")%>' CssClass="NumberOnly" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="PageDateTime" class="PageDiv DateTime">
                        <asp:GridView runat="server" ID="gvItemsDateTime"
                            AutoGenerateColumns="false"
                            ShowHeader="true"
                            OnRowDataBound="gvItemsDateTime_RowDataBound">
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                <asp:Label ID="Label03" runat="server" ForeColor="Red" Font-Bold="true" Text="請選擇資料類型"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Key">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtKEY" ReadOnly="true" Text='<%#Bind("NAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value">
                                    <ItemTemplate>
                                        <div runat="server" id="valueDiv">
                                            <asp:TextBox runat="server" ID="txtVALUE1" Text="2019" CssClass="NumberOnly" />
                                            <asp:Label runat="server" ID="connect1" Text="/" />
                                            <asp:TextBox runat="server" ID="txtVALUE2" Text="02" CssClass="NumberOnly" />
                                            <asp:Label runat="server" ID="connect2" Text="/" />
                                            <asp:TextBox runat="server" ID="txtVALUE3" Text="03" CssClass="NumberOnly" />
                                            <asp:Label runat="server" ID="connect3" Text=" " />
                                            <asp:TextBox runat="server" ID="txtVALUE4" Text="04" CssClass="NumberOnly" />
                                            <asp:Label runat="server" ID="connect4" Text=":" />
                                            <asp:TextBox runat="server" ID="txtVALUE5" Text="05" CssClass="NumberOnly" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="PageList" class="PageDiv Json List">
                        <div>
                            <asp:GridView runat="server" ID="gvItemsList"
                                AutoGenerateColumns="false"
                                ShowHeader="true">
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <asp:Label ID="Label03" runat="server" ForeColor="Red" Font-Bold="true" Text="請選擇資料類型"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="Title">
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnTITLE" Text='<%#Bind("NAME")%>' OnClick="btnLISTTITLE_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="RightCol">
                                <asp:GridView runat="server" ID="gvItemsList_Sub"
                                    AutoGenerateColumns="false"
                                    ShowHeader="true">
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="Label03" runat="server" ForeColor="Red" Font-Bold="true" Text="目前無相關資料"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Content">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btnListDelete" Text="刪除" OnClick="btnListDelete_Click" />
                                                <asp:TextBox runat="server" ID="txtVALUE" Text='<%#Bind("VALUE")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div>
                                    <asp:Button runat="server" ID="Button1" Text="新增一筆" OnClick="btnListNew_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="clear: both;">
                    <asp:Button ID="btnSave" runat="server" Text="儲存" OnClick="btnSave_Click" />
                    <asp:Label ID="lblSaveMessage" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
