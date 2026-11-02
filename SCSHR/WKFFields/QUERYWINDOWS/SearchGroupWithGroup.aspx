<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DialogMasterPage.master" CodeFile="SearchGroupWithGroup.aspx.cs" Inherits="CDS_FTV_WKFFields_QUERYWINDOWS_SearchGroupWithGroup" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .hidden {
            display: none;
        }

        div.Content {
            display: flex;
            height: 100%;
            flex-direction: column;
        }

            div.Content > div.Main {
                height: 100%;
                display: flex;
            }

            div.Content > div.BtnArea {
                display: flex;
                align-items: center;
                justify-content: center;
            }

                div.Content > div.BtnArea > a {
                    position: absolute;
                    bottom: 8px;
                    color: black;
                    background-color: #ffe878;
                    padding: 2px 12px;
                    border-radius: 3px;
                }

        td.auto-style1 {
            display: none;
        }

        div.Main {
            position: relative;
        }

            div.Main > div {
                width: 100%;
                padding: 20px;
                max-height: 100%;
                overflow: auto;
            }

                div.Main > div.Left {
                    background-color: #f8fbff;
                }

                div.Main > div > div:first-child {
                    background-color: #355a8c;
                    color: white;
                    margin: -20px;
                    padding: 10px 20px;
                    text-align: center;
                }

                div.Main > div.Left > div:first-child {
                    background-color: #2e6dc1;
                }

                div.Main > div:not(.Left) {
                    border-left: 1px solid;
                }

            div.Main table td {
                padding: 5px 0;
            }

        table.gvSelect td > input[type=checkbox] {
            display: none;
        }

            table.gvSelect td > input[type=checkbox] + Label {
                padding-left: 20px;
                position: relative;
                display: block;
            }

                table.gvSelect td > input[type=checkbox] + Label:before {
                    content: url("../../../../Common/Images/Icon/icon_m17.png");
                    display: block;
                    position: absolute;
                    left: 0px;
                    top: 3px;
                }

        div.FolderItem > input[type=checkbox] {
            display: none;
        }

            div.FolderItem > input[type=checkbox] + label {
                padding-left: 13px;
                position: relative;
                color: #464646;
                white-space: nowrap;
            }

                div.FolderItem > input[type=checkbox] + label:before {
                    content: url("../../../../Common/Images/Icon/icon_m01.png");
                    display: block;
                    position: absolute;
                    left: -7px;
                    top: 2px;
                }

        div.SelectGroup div.FolderItem > input[type=checkbox]:checked + Label {
            color: #2da8af;
            font-weight: bold;
        }

            div.SelectGroup div.FolderItem > input[type=checkbox]:checked + label:before {
                transform: rotateZ(-20deg);
            }

        div.SelectUser input[type=checkbox]:checked + Label {
            color: #d6864b;
            font-weight: bold;
        }

            div.SelectUser input[type=checkbox]:checked + label:before {
                transform: rotateZ(-20deg);
            }

        @media (max-width:500px) {

            div.Main > div:not(.Left):not(.SelectGroup) {
                position: absolute;
                display: none;
                background-color: white;
                width: 50%;
                height: 100%;
            }

            div.Right1 {
                left: 0;
            }

            div.Right2 {
                right: 0;
            }

            label.btnOpen.SelectUser {
                position: absolute;
                display: block;
                background-color: white;
                border: 1px solid;
                padding: 0 6px 2px 6px;
                right: 30px;
                bottom: 10px;
                border-radius: 3px;
                -webkit-user-select: none; /* Chrome all / Safari all */
                -moz-user-select: none; /* Firefox all */
                -ms-user-select: none; /* IE 10+ */
                user-select: none; /* Likely future */
            }

            label.btnOpen:hover {
                background-color: #394e7d;
                color: white;
            }

            input#OpenSelect:checked ~ div.Right1,
            input#OpenSelect:checked ~ div.Right2 {
                display: block;
            }
        }
    </style>
    <script>
        var isOpen = false;
        var scrollTop = [];
        //Register Begin Request and End Request 
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Get The Div Scroll Position
        function BeginRequestHandler(sender, args) {
            isOpen = $('input#OpenSelect').prop("checked");
            var m = $('div.Main > div');
            if (scrollTop.length < m) {
                for (var i = 0; i < m.length; i++) {
                    scrollTop.push(0);
                }
            }
            for (var i = 0; i < m.length; i++) {
                scrollTop[i] = m[i].scrollTop;
            }
        }
        //Set The Div Scroll Position
        function EndRequestHandler(sender, args) {
            $('input#OpenSelect').prop("checked", isOpen);
            var m = $('div.Main > div');
            for (var i = 0; i < m.length; i++) {
                m[i].scrollTop = scrollTop[i];
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="height: 100%">
        <ContentTemplate>
            <div class="Content">
                <div class="Main">
                    <div class="Left">
                        <div>已選擇員工</div>
                        <asp:GridView ID="gvSelect" runat="server" CssClass="gvSelect" Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="false" AutoGenerateColumns="false" OnRowDataBound="gvSelect_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkMember" Text='<%#Bind("NAME")%>' AutoPostBack="true" OnCheckedChanged="chkMember_CheckedChanged"></asp:CheckBox>
                                        <asp:HiddenField ID="hidGROUPID" runat="server" Value='<%#Bind("GROUP_ID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <input type="checkbox" id="OpenSelect" hidden />
                    <div runat="server" id="Right1" class="Right1">
                        <div>部門</div>
                        <asp:GridView ID="gvMain" runat="server" CssClass="gvMain" Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="false" AutoGenerateColumns="false" OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div runat="server" id="FItemDiv" class="FolderItem">
                                            <asp:CheckBox ID="chkGROUPID" runat="server" Text='<%#Bind("GROUP_NAME")%>' AutoPostBack="true" OnCheckedChanged="chkGROUPID_CheckedChanged" />
                                            <asp:HiddenField ID="hidGROUPID" runat="server" Value='<%#Bind("GROUP_ID")%>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div runat="server" id="Right2" class="Right2">
                        <div>員工</div>
                        <asp:TextBox runat="server" ID="txtSearch" style="margin-top:25px;" Width ="100%" placeholder="搜尋..." OnTextChanged="txtSearch_TextChanged" />
                        <asp:GridView ID="gvSub" runat="server" CssClass="gvSelect" Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="false" AutoGenerateColumns="false" OnRowDataBound="gvSub_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkUSERNAME" runat="server" Text='<%#Bind("NAME")%>' AutoPostBack="true" OnCheckedChanged="chkUSERNAME_CheckedChanged" />
                                        <asp:HiddenField ID="hidGROUPID" runat="server" Value='<%#Bind("GROUP_ID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <label runat="server" id="btnOpen" for="OpenSelect" class="hidden btnOpen">≡選擇</label>
                </div>
                <div class="BtnArea">
                    <asp:LinkButton ID="lbtnGet" runat="server" OnClick="lbtnGet_Click">選擇</asp:LinkButton>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
