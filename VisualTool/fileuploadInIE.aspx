<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fileuploadInIE.aspx.cs" Inherits="fileuploadInIE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%--<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="scripts/jquery.fileupload.js" type="text/javascript"></script>
    <script src="scripts/jquery.iframe-transport.js" type="text/javascript"></script>

    <script>
        $(document).ready(function () {


            var extension = "jpg,jpeg,png,gif";

            $('#fileupload').fileupload({
                replaceFileInput: false,
                dataType: 'json',
                url: '<%= ResolveUrl("AjaxFileHandler2.ashx?type=Temp&id=hello")%>',
                add: function (e, data) {

                    $.each(data.files, function (index, file) {

                        ext = getExt(file.name.toLowerCase());

                        if (extension.indexOf(ext) != -1) {
                            data.submit();
                        }
                        else {
                            alert('It seems you have selected wrong type of file. Allowed extensions: ' + extension);
                        }
                    });

                },
                done: function (e, data) {

                    alert('hello');
                    $.each(data.result, function (index, file) {
                        $('<p/>').text(file).appendTo('body');
                    });

                    //alert('<%= ResolveUrl("repository-manag.aspx") %>');
                }
            });


            function getExt(fileName) {

                var ext = fileName.split('.').pop();
                return ext;
            }


        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    
    <input type="file" id="fileupload" name="file" multiple="multiple" />

    </form>



</body>
</html>--%>
<html>
<head>
 <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="scripts/jquery.fileupload.js" type="text/javascript"></script>
    <script src="scripts/jquery.iframe-transport.js" type="text/javascript"></script>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    

</head>
<body>
<script type="text/javascript">
    $(function () {

        var extension = "jpg,jpeg,png,gif";
        var ext = "";

        $('#fileupload').fileupload({
            replaceFileFormat: false,
            dataType: 'json',
            url: '<%=ResolveUrl("AjaxFileHandler2.aspx")%>',
            add: function (e, data) {

                $.each(data.files, function (index, file) {

                    ext = getExt(file.name.toLowerCase());

                    if (extension.indexOf(ext) != -1) {
                        data.submit();
                    }
                    else {
                        alert('It seems you have selected wrong type of file. Allowed extensions: ' + extension);
                    }
                });

            },
            done: function (e, data) {
                $.each(data.result, function (index, file) {
                    alert('hello');
                    $('<p/>').text(file).appendTo('body');
                });
            }
        });
    });
    function getExt(fileName) {

        var ext = fileName.split('.').pop();
        return ext;
    }
    function upload() {
        var file = document.getElementById('fileupload1');
        alert(file.fileName);
    }
    </script>
<form id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<input id="fileupload" type="file" name="file" multiple="multiple" />
<input id="button" type="button" value="ok" onclick="upload()"/>
</form>
</body>
</html>



