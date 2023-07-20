<%@ Page Language="VB" AutoEventWireup="false" CodeFile="repository_management.aspx.vb" Inherits="repository_management" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>Home</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="styles/cloud.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="styles/style.css" type="text/css" media="screen" />
    <!--[if IE 6]><link rel="stylesheet" href="styles/style.ie6.css" type="text/css" media="screen" /><![endif]-->
    <!--[if IE 7]><link rel="stylesheet" href="styles/style.ie7.css" type="text/css" media="screen" /><![endif]-->
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />

    <script type="text/javascript" src="scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript" src="scripts/script.js"></script>
    <style type="text/css">
        .style1 {
            width: 138px;
        }

        .style2 {
        }

        .style3 {
            width: 77px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">

        <div id="art-page-background-glare">
            <div id="art-page-background-glare-image">
                <div id="art-main">

                    <div class="art-sheet">
                        <!--  <div id="clouds">
        <div class="cloud x1">
        </div>
       
        <div class="cloud x2">
        </div>
        <div class="cloud x3">
        </div>
        <div class="cloud x4">
        </div>
        <div class="cloud x5">
        </div>
    </div>  CLOUDS -->
                        <div class="art-sheet-body">
                            <div class="art-nav">
                                <ul class="art-menu">
                                    <li><a href="#" class="active"><span class="l"></span><span class="r"></span><span
                                        class="t">Home</span></a></li>
                                    <li><a href="#"><span class="l"></span><span class="r"></span><span class="t">Whats
                                    new</span></a></li>
                                    <li><a href="#"><span class="l"></span><span class="r"></span><span class="t">Contact
                                    Us</span></a></li>
                                </ul>
                            </div>
                            <div class="art-content-layout">
                                <div class="art-content-layout-row">
                                    <div class="art-layout-cell art-content">
                                        <div class="art-post">
                                            <div class="art-post-tl">
                                            </div>
                                            <div class="art-post-tr">
                                            </div>
                                            <div class="art-post-bl">
                                            </div>
                                            <div class="art-post-br">
                                            </div>
                                            <div class="art-post-tc">
                                            </div>
                                            <div class="art-post-bc">
                                            </div>
                                            <div class="art-post-cl">
                                            </div>
                                            <div class="art-post-cr">
                                            </div>
                                            <div class="art-post-cc">
                                            </div>
                                            <div class="art-post-body">
                                                <div class="art-post-inner art-article">
                                                    <div class="art-postcontent">
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td style="width: 20%; height: 300px; border-right-style: dashed; border-right-width: medium; border-right-color: #666666;">
                                                                    <ul class="nonAcc-menu" style="margin: 0px;">
                                                                        <li>Images</li>
                                                                        <li>Videos</li>
                                                                        <li>Audios</li>
                                                                    </ul>
                                                                </td>
                                                                <td>
                                                                    <div id="opt-container">
                                                                        <div class="option-icons">
                                                                            <img id="opt-search" src="icons/search.png" />
                                                                        </div>
                                                                        <div class="option-icons">
                                                                            <img id="opt-add" src="icons/plus.png" />
                                                                        </div>
                                                                        <div class="option-icons">
                                                                            <img id="opt-grid" src="icons/grid-view.png" />
                                                                        </div>
                                                                    </div>
                                                                    <div id="thumb-container">
                                                                        fadsfdas
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="cleared">
                                                    </div>
                                                </div>
                                                <div class="cleared">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="cleared">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="cleared">
                            </div>
                            <div class="art-footer">
                                <div class="art-footer-body">
                                    <div class="art-footer-text">
                                        <p>
                                            Copyright © 2012, Melmark. All Rights Reserved.
                                        </p>
                                    </div>
                                    <div class="cleared">
                                    </div>
                                </div>
                            </div>
                            <div class="cleared">
                            </div>
                        </div>
                    </div>
                    <div class="cleared">
                    </div>
                    <p class="art-page-footer">
                        Powered by <a href="#">www.m2ctech.com</a> and Template created by Pramod Kumar
                    </p>
                </div>
            </div>
        </div>


        <div id="overlay"></div>
        <!-- THE FADED DIV -->

        <div id="loading">
            <span>Please Wait</span>
            <span class="l-1"></span>
            <span class="l-2"></span>
            <span class="l-3"></span>
            <span class="l-4"></span>
            <span class="l-5"></span>
            <span class="l-6"></span>
        </div>
        <!-- TO SHOW THE LOADING ANIMATION -->

        <div id="image-upload" class="popUp">
            <div class="close-popUp">
                <img alt="" src="icons/close.png" />
            </div>
            <div class="box">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%; height: 154px;">
                            <tr>
                                <td class="style3">File Name</td>
                                <td class="style1">
                                    <asp:TextBox ID="TextBox2" runat="server" Width="150px"></asp:TextBox>
                                </td>
                                <td rowspan="2">
                                    <img alt="" src="" style="width: 100%; height: 106px" /></td>
                            </tr>
                            <tr>
                                <td class="style3">Description</td>
                                <td class="style1">
                                    <asp:TextBox ID="TextBox3" runat="server" Height="45px" TextMode="MultiLine"
                                        Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">Keywords</td>
                                <td class="style1">
                                    <asp:TextBox ID="TextBox4" runat="server" Height="45px" TextMode="MultiLine"
                                        Width="150px"></asp:TextBox>
                                </td>
                                <td rowspan="2">
                                    <asp:Button ID="Button1" runat="server" Text="Save" Width="130px"
                                        Height="30px" />
                                    <br />
                                    <asp:Button ID="Button2" runat="server" Text="Cancel" Width="130px"
                                        Height="30px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style2" colspan="2">
                                    <asp:FileUpload ID="FileUpload1" runat="server" Height="20px" Width="193px" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>

    </form>

</body>

<script>
    $(document).ready(function () {
        $('#opt-add').click(function () {
            $('#overlay').fadeIn('slow', function () {
                $('#image-upload').fadeIn('slow');
            });
        });

        $('.close-popUp').click(function () {
            $('#image-upload').fadeOut('slow', function () {
                $('#overlay').fadeOut('slow');
            });
        });

        $('#opt-grid').click(function () {
            $('#overlay').fadeIn('slow', function () {
                $('#loading').fadeIn('slow');
            });
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
</html>
