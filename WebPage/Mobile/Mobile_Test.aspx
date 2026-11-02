<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Mobile_TEST.aspx.cs" Inherits="CDS_WebPage_Mobile_Mobile_Test" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>instascan：純前端掃描 QR Code</title>
    <%--    <script src="https://rawgit.com/schmich/instascan-builds/master/instascan.min.js"></script>--%>
    <%--    
    <script src="../../instascan.min/instascan.js"></script>--%>
    <%--<script src="../../instascan.min/instascan.min.js"></script>--%>
    <script src="../../instascan.min/IOS15instascan.min.min.js"></script>
</head>

<body>
     <input type="text" id="myText" value="">
    <video id="preview"  width="100" height="100"></video>

    <script type="text/javascript">
        let scanner = new Instascan.Scanner({ video: document.getElementById('preview') });
        scanner.addListener('scan', function (content) {
            document.getElementById("myText").value = content;  
            console.log(content);
        });
        Instascan.Camera.getCameras().then(function (cameras) {
            if (cameras.length > 0) {
                scanner.start(cameras[0]);
            } else {
                console.error('No cameras found.');
            }
        }).catch(function (e) {           
            alert('catch');
        });
    </script> 
  </body>

</html>
