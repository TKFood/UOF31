<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TEST2.aspx.cs" Inherits="CDS_WebPage_IT_TEST2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- 先載入js錄音庫，注意：你應該把js clone到本地使用 -->
    <meta charset="utf-8" />
    <script src="https://xiangyuecn.github.io/Recorder/recorder.mp3.min.js"></script>

    <input type="button" onclick="startRec()" value="開始錄音">
    <hr>
    <input type="button" onclick="playRec()" value="結束並播放">
    <input type="button" onclick="uploadRec()" value="結束並上傳">

    <input type="text" x-webkit-speech onwebkitspeechchange="foo()"/>

    <input type="text" x-webkit-speech /> 

    <script>
        if (document.createElement("input").webkitSpeech === undefined) {
                alert("Speech input is not supported in your browser.");           
        } 

        function foo() {
               alert('changed');            
        } 

        var rec;
        function startRec() {
            rec = Recorder();//使用預設配置，mp3格式

            //開啟麥克風授權獲得相關資源
            rec.open(function () {
                //開始錄音
                rec.start();
            }, function (msg, isUserNotAllow) {
                //使用者拒絕了許可權或瀏覽器不支援
                alert((isUserNotAllow ? "使用者拒絕了許可權，" : "") + "無法錄音:" + msg);
            });
        };

        function uploadRec() {
            //停止錄音，得到了錄音檔案blob二進位制物件，想幹嘛就幹嘛
            rec.stop(function (blob, duration) {
                /*
                blob檔案物件，可以用FileReader讀取出內容
                ，或者用FormData上傳，本例直接上傳二進位制檔案
                ，對於普通application/x-www-form-urlencoded表單上傳
                ，請參考github裡面的例子
                */
                var form = new FormData();
                form.append("upfile", blob, "recorder.mp3"); //和普通form表單並無二致，後端接收到upfile引數的檔案，檔名為recorder.mp3

                //直接用ajax上傳
                var xhr = new XMLHttpRequest();
                xhr.open("POST", "http://baidu.com/修改成你的上傳地址");//這個假地址在控制檯network中能看到請求資料和格式，請求結果無關緊要
                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 4) {
                        alert(xhr.status == 200 ? "上傳成功" : "測試請先開啟瀏覽器控制檯，然後重新錄音，在network選項卡里面就能看到上傳請求資料和格式了");
                    }
                }
                xhr.send(form);
            }, function (msg) {
                alert("錄音失敗:" + msg);
            });
        };

        function playRec() {
            //停止錄音，得到了錄音檔案blob二進位制物件，想幹嘛就幹嘛
            rec.stop(function (blob, duration) {
                var audio = document.createElement("audio");
                audio.controls = true;
                document.body.appendChild(audio);

                //非常簡單的就能拿到blob音訊url
                audio.src = URL.createObjectURL(blob);
                audio.play();
            }, function (msg) {
                alert("錄音失敗:" + msg);
            });
        };


    </script>

</asp:Content>

