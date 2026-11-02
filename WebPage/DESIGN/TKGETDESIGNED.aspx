<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TKGETDESIGNED.aspx.cs" Inherits="CDS_WebPage_DESIGN_TKGETDESIGNED" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>產品圖瀏覽 - 美工檔案區(Y:\)</title>
    <style>
        body {
            font-family: "Microsoft JhengHei", sans-serif;
            background: #f4f4f4;
            padding: 20px;
        }

        .container {
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }

        .path-bar {
            background: #eee;
            padding: 10px;
            margin-bottom: 20px;
            font-weight: bold;
            border-radius: 4px;
        }

        .grid {
            display: flex;
            flex-wrap: wrap;
            gap: 15px;
        }

        .card {
            width: 180px;
            border: 1px solid #ddd;
            border-radius: 5px;
            background: #fff;
            text-align: center;
            padding: 10px;
        }

            .card img {
                width: 100%;
                height: 120px;
                object-fit: cover;
            }

        .folder {
            font-size: 50px;
            color: #f39c12;
            text-decoration: none;
            display: block;
        }

        .name {
            font-size: 12px;
            margin-top: 5px;
            word-break: break-all;
            display: block;
        }

        .back-link {
            display: inline-block;
            margin-bottom: 10px;
            color: #007bff;
            text-decoration: none;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>老楊食品 - 產品圖瀏覽 (Y:\)</h2>

            <div class="search-box">
                路徑：<asp:TextBox ID="txtPath" Text=" Y:\老楊食品" runat="server" Width="400px"></asp:TextBox>
                <asp:Button ID="btnGo" runat="server" Text="讀取" OnClick="btnGo_Click" CssClass="btn-go" />
                <br />
                <small>(範例: Y:\老楊食品)</small>
            </div>

            <div class="path-bar">
                <asp:Literal ID="litCurrentPath" runat="server"></asp:Literal>
            </div>

            <asp:HyperLink ID="hlBack" runat="server" CssClass="back-link" Visible="false">⬅ 返回上一層</asp:HyperLink>

            <div class="grid">
                <%-- 這裡的 ID 必須是 rptItems --%>
                <asp:Repeater ID="rptItems" runat="server">
                    <ItemTemplate>
                        <div class="card">
                            <%-- 點擊資料夾會導向自己(aspx)，點擊檔案會導向圖片(ashx) --%>
                            <a href='<%# Eval("LinkUrl") %>' style="text-decoration: none; color: inherit;">
                                <div class="thumb-area">
                                    <%# Eval("Type").ToString() == "Folder" ? 
                        "<span class='folder'>📁</span>" : 
                        "<img src='" + Eval("LinkUrl") + "' loading='lazy' />" %>
                                </div>
                                <span class="name" title='<%# Eval("Name") %>'><%# Eval("Name") %></span>
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
</body>
</html>
