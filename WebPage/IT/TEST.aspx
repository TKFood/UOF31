<%@ Page Title="" Language="C#" MasterPageFile="~/Master/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="TEST.aspx.cs" Inherits="CDS_WebPage_IT_TEST" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <html>
<h1>How To Convert HTML To PDF Using JavaScript - Techsolutionstuff</h1>
<form class="form">
    <table>
        <tbody>
            <tr>
                <th>Company Name</th>
                <th>Employee Name</th>
                <th>Country</th>
            </tr>
            <tr>
                <td>Dell</td>
                <td>Maria</td>
                <td>Germany</td>
            </tr>
            <tr>
                <td>Asus</td>
                <td>Francisco</td>
                <td>Mexico</td>
            </tr>
            <tr>
                <td>Apple</td>
                <td>Roland</td>
                <td>Austria</td>
            </tr>
            <tr>
                <td>HP</td>
                <td>Helen</td>
                <td>UK</td>
            </tr>
            <tr>
                <td>Lenovo</td>
                <td>Yoshi</td>
                <td>Canada</td>
            </tr>
        </tbody>
    </table><br>
    <input type="button" id="create_pdf" value="Generate PDF">
</form>
</html>


<style>
    /*table {
        font-family: arial, sans-serif;
        border-collapse: collapse;
        width: 100%;
    }

    td {
        border: 1px solid #dddddd;
        text-align: left;
        padding: 8px;
    }

    th {
        border: 1px solid #dddddd;
        text-align: left;
        padding: 8px;
        background-color: #111;
        color: white;
    }

    tr:nth-child(odd) {
        background-color: #dddddd;
    }*/
</style>

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.5/jspdf.min.js"></script>
<script>
    $(document).ready(function () {
        var form = $('.form'),
            cache_width = form.width(),
            a4 = [595.28, 841.89]; // for a4 size paper width and height

        $('#create_pdf').on('click', function () {
            $('body').scrollTop(0);
            createPDF();
        });

        function createPDF() {
            getCanvas().then(function (canvas) {
                var
                    img = canvas.toDataURL("image/png"),
                    doc = new jsPDF({
                        unit: 'px',
                        format: 'a4'
                    });
                doc.addImage(img, 'JPEG', 20, 20);
                doc.save('Bhavdip-html-to-pdf.pdf');
                form.width(cache_width);
            });
        }

        function getCanvas() {
            form.width((a4[0] * 1.33333) - 80).css('max-width', 'none');
            return html2canvas(form, {
                imageTimeout: 2000,
                removeContainer: true
            });
        }
    });
</script>

</asp:Content>

