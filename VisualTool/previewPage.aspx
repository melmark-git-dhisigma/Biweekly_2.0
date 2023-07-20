<%@ Page Language="C#" AutoEventWireup="true" CodeFile="previewPage.aspx.cs" Inherits="previewPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <style type="text/css">
        .workSpaceTable {
            width: 100%;
        }

        body {
            margin: 0px;
        }

        div {
            vertical-align: top;
        }

        #musicPlayer {
            top: 0px;
            right: -170px;
            position: absolute;
        }

        .demo {
            position: absolute;
        }
    </style>
    <link href="styles/ContentPage.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" media="screen" type="text/css" href="styles/colorpicker.css" />
    <link href="styles/jquery.ui.resizable.css" rel="stylesheet" />

    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <script type="text/javascript" src="scripts/colorpicker.js"></script>
    <script src="scripts/cookies.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.resizable.js"></script>
    <script type="text/javascript">
        //Code Starts
        function GetQueryStringParams(sParam) {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                if (sParameterName[0] == sParam) {
                    return sParameterName[1];
                }
            }
        }
        //Code Ends
        $(document).ready(function () {
            var type = GetQueryStringParams('type');
            var slideNo = GetQueryStringParams('pageNo');



            $.ajax({
                url: "previewPage.aspx/getSlideDatas",
                data: "{ 'slideNo': '" + slideNo + "','type':'" + type + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {

                    var contents = data.d;
                    // alert(contents);
                    if (contents.indexOf("System.IndexOutOfRangeException") == -1) {
                        var content_array = contents.split('^');
                        var listNew = $(content_array[0]).find('.saveElem');

                        for (var i = 0; i < listNew.length; i++) {

                            var height = $(listNew[i]).find('.height').html();
                            var width = $(listNew[i]).find('.width').html();
                            var top = $(listNew[i]).find('.top').html();
                            var left = $(listNew[i]).find('.left').html();
                            var data = $(listNew[i]).find('.data').html();

                            data = data.replace(/&gt;/g, '>');
                            data = data.replace(/&lt;/g, '<');

                            $('#workSpacePreview').append('<div class = "demo" style = "height:' + height + '; width:' + width + ';top:' + top + 'px;left:' + left + 'px; position:absolute;">' + data + '</div>');
                            if (content_array[1] != '') {
                                $('#musicPlayer').html('<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="200" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=' + content_array[1] + '&showslider=0&width=25&autoplay=1" /></object>');
                            }
                        }
                    }
                    else {
                        alert('No slide to display');
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }

            });
        });

    </script>

    <script type="text/javascript">

        $(function () {
            // Patch fractional .x, .y form parameters for IE10.
            if (typeof (Sys) !== 'undefined' && Sys.Browser.agent === Sys.Browser.InternetExplorer && Sys.Browser.version === 10) {
                Sys.WebForms.PageRequestManager.getInstance()._onFormElementActive = function Sys$WebForms$PageRequestManager$_onFormElementActive(element, offsetX, offsetY) {
                    if (element.disabled) {
                        return;
                    }
                    this._activeElement = element;
                    this._postBackSettings = this._getPostBackSettings(element, element.name);
                    if (element.name) {
                        var tagName = element.tagName.toUpperCase();
                        if (tagName === 'INPUT') {
                            var type = element.type;
                            if (type === 'submit') {
                                this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                            }
                            else if (type === 'image') {
                                this._additionalInput = encodeURIComponent(element.name) + '.x=' + Math.floor(offsetX) + '&' + encodeURIComponent(element.name) + '.y=' + Math.floor(offsetY);
                            }
                        }
                        else if ((tagName === 'BUTTON') && (element.name.length !== 0) && (element.type === 'submit')) {
                            this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                        }
                    }
                };
            }
        });

    </script>
</head>
<body style="padding: 3px;">
    <form id="form1" runat="server">
        <div id="musicPlayer">
            <%--<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="200" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=&showslider=0&width=25" /></object>--%>
        </div>
        <div style="visibility: hidden; position: absolute;">
            <asp:Label ID="lblMp3" runat="server"></asp:Label></div>
        <div style="width: 100%; height: 100%;">

            <div id="workSpacePreview"></div>

        </div>
    </form>
</body>
<script>
    // $(document).ready(function () {
    //     var filename = $('#lblMp3').html();
    //     if (filename != '') {
    //         $('#musicPlayer').append('<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="200" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=' + filename + '&showslider=0&width=25&autoplay=1" /></object>');
    //     }
    // });
</script>
</html>
