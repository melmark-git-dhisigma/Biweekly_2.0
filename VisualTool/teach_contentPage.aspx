<%@ Page Language="C#" AutoEventWireup="true" CodeFile="teach_contentPage.aspx.cs" Inherits="teach_contentPage" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="styles/ContentPage.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" media="screen" type="text/css" href="styles/colorpicker.css" />
    <link href="styles/jquery.ui.resizable.css" rel="stylesheet" />

    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <script type="text/javascript" src="scripts/colorpicker.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.resizable.js"></script>

    <script src="scripts/jsForTextEditor/tiny_mce/tiny_mce.js" type="text/javascript"></script>
    <%--<script src="scripts/content_page.js" type="text/javascript"></script>--%>

    <style type="text/css">
        .style1 {
            width: 454px;
        }

        .style2 {
            width: 675px;
        }

        #Text1 {
            width: 53px;
        }

        #Text2 {
            width: 53px;
        }

        #Text3 {
            width: 53px;
        }

        .style3 {
            height: 236px;
        }

        .style4 {
            height: 21px;
        }

        .style5 {
            height: 26px;
        }

        .style7 {
            height: 26px;
            width: 80px;
        }

        .style8 {
            height: 21px;
            width: 80px;
        }

        .style9 {
            width: 80px;
        }

        .style10 {
            width: 50px;
        }


        .auto-style1 {
            width: 128px;
        }

        .auto-style2 {
            height: 26px;
            width: 128px;
        }

        .auto-style3 {
            height: 21px;
            width: 128px;
        }

        .auto-style4 {
            width: 127px;
        }

        #Text2 {
            width: 53px;
        }

        #Text3 {
            width: 53px;
        }

        .thin {
            font-size: small;
            font-weight: bold;
            color: red;
        }

        .medium {
        }

        .thick {
        }

        iframe {
            border: medium none;
            height: 560px;
            overflow-x: hidden;
            width: 100%;
        }

        .resizable td > div {
            height: 100%;
        }

        .resizable {
            width: 100%;
        }
    </style>

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

            alert(type)
            alert(slideNo)

            $.ajax({
                url: "teach_contentPage.aspx/getSlideDatas",
                data: "{ 'slideNo': '" + slideNo + "','type':'" + type + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {

                    var contents = data.d;
                    if (contents.indexOf("System.IndexOutOfRangeException") == -1) {
                        var content_array = contents.split('^');
                        var listNew = $(content_array[0]).find('.saveElem');
                        var heightWorkArea = $(contents).find('.workSpaceHeight').html();
                        $('#workSpace').css("height", heightWorkArea);

                        alert(heightWorkArea);
                        var W = $('#workSpace').offset();


                        for (var i = 0; i < listNew.length; i++) {

                            var height = $(listNew[i]).find('.height').html();
                            var width = $(listNew[i]).find('.width').html();
                            var top = parseFloat($(listNew[i]).find('.top').html()) + W.top;
                            var left = parseFloat($(listNew[i]).find('.left').html()) + W.left;
                            var data = $(listNew[i]).find('.data').html();



                            data = data.replace(/&gt;/g, '>');
                            data = data.replace(/&lt;/g, '<');

                            $('#workSpace').append('<div class = "demo" style = "height:' + height + '; width:' + width + ';top:' + top + 'px;left:' + left + 'px; position:absolute;">' + data + '</div>');
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
</head>

<body>
    <!-- top panel -->
    <form runat="server" id="form1">
        <div id="dashboard-top-panel">
            <div id="top-panel-container">
                <ul>
                    <li class="user">
                        <img src="images/admin-icon.png" width="16" height="16" align="baseline">Super Admin</li>
                    <li>
                        <img src="images/time-icon.png" width="14" height="14" align="baseline">5:41 PM</li>
                    <li><a href="#">
                        <img src="images/srch-icon.png" width="16" height="16" align="baseline">Search</a></li>
                    <li></li>
                </ul>
            </div>
        </div>

        <!-- dashboard container panel -->
        <div id="db-container">
            <!-- header -->
            <div id="header-panel">
                <div class="Dashboard-logo">
                    <img src="images/dashboard-logo.jpg" class="dash-logo">
                </div>
                <div class="header-links">
                    <ul>
                        <li><a href="homePage.aspx">
                            <img src="icons/home.png" width="28" height="25"><br>
                            Home</a></li>
                    </ul>
                </div>
            </div>


            <!-- content panel -->
            <div id="dashboard-content-panel">
                <!-- content Left side -->


                <!-- content right side -->
                <div id="dashboard-RHS">

                    <div class="dashboard-RHS-content" style="padding: 5px; width: 810px; margin-left: 141px;">


                        <!-- CHANGABLE -->

                        <div id="musicPlayer">
                            <%--<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="200" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=&showslider=0&width=25" /></object>--%>
                        </div>
                        <div style="visibility: hidden; position: absolute;">
                            <asp:Label ID="lblMp3" runat="server"></asp:Label>
                        </div>
                        <div style="width: 810px; min-height: 590px; background-color: #EAEAEA;">

                            <div id="workSpace" style="height: auto;"></div>

                        </div>

                        <!-- CHANGEABLE -->
                    </div>

                </div>
            </div>


            <!-- footer -->
            <div id="footer-panel">
                <ul>
                    <li>COPYRIGHT &copy; 2012 Melmark Inc. All rights reserved</li>
                </ul>
            </div>


        </div>
    </form>

    <div id="loadingOverlay"></div>
    <div id="loading">
        <div>Please Wait...</div>
        <img src="images/29.gif" style="width: 50px; height: 50px;" />
    </div>
</body>

<script type="text/javascript" src="scripts/jquery.shuffleLetters.js"></script>
</html>
