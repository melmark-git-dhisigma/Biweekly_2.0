<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LessonManagement.aspx.cs" Inherits="LessonManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <script src="scripts/01_3.js"></script>
    <script src="scripts/01_4.js"></script>
    <link href="styles/LessonDesignNew.css" rel="stylesheet" />
    <link href="styles/LandDesign.css" rel="stylesheet" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="styles/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />
    <%-- <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />--%>
    <%-- <script type="text/javascript" src="../Administration/js/jquery.min.js"></script>--%>
    <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>
    <script type="text/javascript">
        $(function ($) {
            $('.jclock').jclock();
        });


    </script>


    <%--   <script src="scripts/jquery-1.8.0.js"></script>--%>
    <style type="text/css">
        table td {
            padding: 0px;
        }


        .PagerStyle {
            background-color: #0c7843;
            height: 26px;
            width: 90%;
        }

        #closeNew {
            display: block;
            height: 23px;
            overflow: hidden;
            position: absolute;
            right: 4px;
            top: 2px;
            width: 24px;
        }

        a {
            text-decoration: none;
        }
    </style>


    <style>
        .clear {
            margin: 0;
            padding: 0;
            clear: both;
            border: 0;
        }

        .frameMrk {
            width: 98%;
            min-height: 250px;
            height: auto !important;
            height: 250px;
            margin: 0 auto;
            background: #fff;
            padding: 8px;
        }
            /*-------------------Left Container Start---------------------*/
            .frameMrk div.lftListContainer {
                width: 20%!important;
                min-height: 250px;
                height: auto !important;
                height: 250px;
                float: left!important;
                padding-right: 5px;
                display: none;
            }


                .frameMrk div.lftListContainer a.lCTop,
                .frameMrk div.lftListContainer a.lCTop:link,
                .frameMrk div.lftListContainer a.lCTop:visited {
                    background: url(images/lisicon01.PNG) 10px top no-repeat;
                    padding-left: 30px;
                    font-size: 12px;
                    font-weight: bold;
                    color: #666;
                    text-align: left;
                    text-decoration: none;
                    width: 85%;
                    border-bottom: 1px solid #cee3e5;
                    padding-bottom: 5px;
                    margin: 5px auto;
                    font-family: Arial, Helvetica, sans-serif;
                    display: block;
                }

                    .frameMrk div.lftListContainer a.lCTop:hover {
                        color: #05658d;
                        background: url(images/lisicon01.PNG) 10px top no-repeat;
                    }

                .frameMrk div.lftListContainer a.lC,
                .frameMrk div.lftListContainer a.lC:link,
                .frameMrk div.lftListContainer a.lC:visited {
                    background: url(images/list.PNG) 10px 5px no-repeat;
                    padding: 5px 0 5px 30px;
                    font-size: 12px;
                    font-weight: normal;
                    color: #666;
                    text-align: left;
                    text-decoration: none;
                    width: 85%;
                    border-bottom: 1px solid #cee3e5;
                    margin: 0 auto;
                    font-family: Arial, Helvetica, sans-serif;
                    display: block;
                }

                .frameMrk div.lftListContainer input.lC:hover {
                    color: #05658d;
                    background: #efefef url(images/list.PNG) 10px 5px no-repeat;
                }
            /*-------------------Left Container End---------------------*/

            /*-------------------Right Container Start---------------------*/
            .frameMrk div.RighListContainer {
                width: 100%!important;
                min-height: 250px;
                height: auto !important;
                height: 250px;
                float: left;
                margin-left: 50px;
                /*background: url(images/dtlinbg.PNG) left top repeat-y;*/
            }

                .frameMrk div.RighListContainer div.topMenuBar {
                    width: 92%!important;
                    height: 46px;
                    float: left;
                    margin-left: 10px!important;
                }

                    .frameMrk div.RighListContainer div.topMenuBar input.al {
                        background: url(images/all.png) left top!important;
                        width: 149px;
                        height: 45px;
                        float: left;
                        cursor: pointer;
                        border: 0;
                        background: 0 -0px;
                        display: block;
                        margin-right: 8px;
                    }

                        .frameMrk div.RighListContainer div.topMenuBar input.al:hover {
                            background-position: 0 -51px!important;
                        }

                        .frameMrk div.RighListContainer div.topMenuBar input.al.current {
                            background-position: 0 -51px!important;
                        }

        .selctButtonAll {
            background: url(images/all.png) left top!important;
            width: 149px;
            height: 45px;
            float: left;
            cursor: pointer;
            border: 0;
            background: 0 -0;
            display: block;
            margin-right: 8px;
            background-position: 0 -101px!important;
        }

        .frameMrk div.RighListContainer div.topMenuBar {
            width: 98%;
            margin: 0 auto;
            height: 45px;
        }

            .frameMrk div.RighListContainer div.topMenuBar input.co {
                background: url(images/coin.png) left top!important;
                width: 149px;
                height: 45px;
                float: left;
                cursor: pointer;
                border: 0;
                background: 0 -0;
                display: block;
                margin-right: 8px;
            }

                .frameMrk div.RighListContainer div.topMenuBar input.co:hover {
                    background-position: 0 -51px!important;
                }

                .frameMrk div.RighListContainer div.topMenuBar input.co.current {
                    background-position: 0 -51px!important;
                }

        .selctButtonCoin {
            background: url(images/coin.png) left top!important;
            width: 149px;
            height: 45px;
            float: left;
            cursor: pointer;
            border: 0;
            background: 0 -0;
            display: block;
            margin-right: 8px;
            background-position: 0 -101px!important;
        }

        .frameMrk div.RighListContainer div.topMenuBar input.mo {
            background: url(images/mouse.png) left top!important;
            width: 149px;
            height: 45px;
            float: left;
            cursor: pointer;
            border: 0;
            background: 0 -0;
            display: block;
            margin-right: 8px;
        }

            .frameMrk div.RighListContainer div.topMenuBar input.mo:hover {
                background-position: 0 -51px!important;
            }

            .frameMrk div.RighListContainer div.topMenuBar input.mo.current {
                background-position: 0 -51px!important;
            }

        .selctButtommouse {
            background: url(images/mouse.png) left top!important;
            width: 149px;
            height: 45px;
            float: left;
            cursor: pointer;
            border: 0;
            background: 0 -0;
            display: block;
            margin-right: 8px;
            background-position: 0 -101px!important;
        }

        .frameMrk div.RighListContainer div.topMenuBar input.ma {
            background: url(images/matching.png) left top!important;
            width: 149px;
            height: 45px;
            float: left;
            cursor: pointer;
            border: 0;
            background: 0 -0;
            display: block;
            margin-right: 8px;
        }



            .frameMrk div.RighListContainer div.topMenuBar input.ma:hover {
                background-position: 0 -51px!important;
            }

            .frameMrk div.RighListContainer div.topMenuBar input.ma.current {
                background-position: 0 -51px!important;
            }

        .selctButtonMatch {
            background: url(images/matching.png) left top!important;
            width: 149px;
            height: 45px;
            float: left;
            cursor: pointer;
            border: 0;
            background: 0 -0;
            display: block;
            margin-right: 8px;
            background-position: 0 -101px!important;
        }



        .frameMrk div.RighListContainer div.topMenuBar input.ti {
            background: url(images/time.png) left top!important;
            width: 149px;
            height: 45px;
            float: left;
            cursor: pointer;
            border: 0;
            background: 0 -0;
            display: block;
            margin-right: 8px;
        }

            .frameMrk div.RighListContainer div.topMenuBar input.ti:hover {
                background-position: 0 -51px!important;
            }

            .frameMrk div.RighListContainer div.topMenuBar input.ti.current {
                background-position: 0 -51px!important;
            }

        .selctButtonTime {
            background: url(images/time.png) left top!important;
            width: 149px;
            height: 45px;
            float: left;
            cursor: pointer;
            border: 0;
            background: 0 -0;
            display: block;
            margin-right: 8px;
            background-position: 0 -101px!important;
        }

        .frameMrk div.RighListContainer div.topMenuBar input.con {
            background: url(images/content.png) left top!important;
            width: 149px;
            height: 45px;
            float: left;
            cursor: pointer;
            border: 0;
            background: 0 -0;
            display: block;
        }

            .frameMrk div.RighListContainer div.topMenuBar input.con:hover {
                background-position: 0 -51px!important;
            }

            .frameMrk div.RighListContainer div.topMenuBar input.con.current {
                background-position: 0 -51px!important;
            }

        .selctBtnContent {
            background: url(images/content.png) left top!important;
            width: 149px;
            height: 45px;
            float: left;
            cursor: pointer;
            border: 0;
            background: 0 -0;
            display: block;
            background-position: 0 -101px!important;
        }

        .frameMrk div.RighListContainer div.secBtnpartContainer {
            width: 88%;
            height: 30px;
            float: left;
            margin-left: 10px;
            padding: 6px 0px;
        }

            .frameMrk div.RighListContainer div.secBtnpartContainer input.tRbx {
                background-color: white;
                border: 1px solid #D7CECE;
                border-radius: 3px;
                color: #676767;
                font-family: Arial,Helvetica,sans-serif;
                font-size: 13px;
                height: 29px;
                float: left;
                line-height: 26px;
                padding: 0 5px 0 10px;
                width: 225px;
            }

            .frameMrk div.RighListContainer div.secBtnpartContainer select.tRbx {
                background-color: white;
                border: 1px solid #D7CECE;
                border-radius: 3px 3px 3px 3px;
                color: #676767;
                font-family: Arial,Helvetica,sans-serif;
                font-size: 13px;
                height: 27px;
                float: left !important;
                line-height: 26px;
                padding: 2px 5px 2px 10px;
                width: 225px;
                margin: 0 5px 0 0;
                display: block;
            }

            .frameMrk div.RighListContainer div.secBtnpartContainer .btn-orange, .btn-orange:visited {
                background: url(images/zoombt.PNG) left top no-repeat;
                display: block;
                float: left;
                font-size: 0;
                height: 26px !important;
                margin: 0 0 0 6px !important;
                padding: 0;
                border: 0;
                width: 26px !important;
                cursor: pointer;
            }

            .frameMrk div.RighListContainer div.secBtnpartContainer input.blbx {
                border-style: none;
                border-color: inherit;
                border-width: medium;
                background: url('images/timemechine.PNG') no-repeat left top;
                height: 30px;
                float: right;
                display: block;
                padding: 0 !important;
                width: 30px;
            }


        .frameMrk div.RighListContainer div.c_Container_Box {
            width: 90%;
            height: 75px;
            float: left;
            background: #f8f8f8;
            padding: 0 0 5px 10px !important;
            border-right: 3PX solid #efefef;
        }

        .frameMrk div.RighListContainer div.parttwo {
            background: #efefef;
        }

        .frameMrk div.RighListContainer div.c_Container_Box h2 {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: bold;
            text-decoration: none;
            text-align: left;
            color: #05658d;
            letter-spacing: 1px;
            margin: 0 0 5px 0;
            border: none !important;
        }

        .frameMrk div.RighListContainer div.c_Container_Box p {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            text-align: justify;
            color: #666;
            margin: 0;
            padding: 0;
        }

            .frameMrk div.RighListContainer div.c_Container_Box p.bx {
                width: 150px;
                height: 23px;
                float: left;
            }

        /*.frameMrk div.RighListContainer div.c_Container_Box span {
                    color: #a80000;
                    margin-left: 25px;
                }*/

        .frameMrk div.RighListContainer div.c_Container_Box div.righIc_Container {
            width: 90px;
            height: 20px;
            float: right;
            top: -100px;
            margin-top: 10px;
        }

            .frameMrk div.RighListContainer div.c_Container_Box div.righIc_Container input.ed {
                border-style: none;
                border-color: inherit;
                border-width: medium;
                background: url('images/edit0_1.png') 0 -21px !important;
                width: 15px;
                display: block;
                float: left;
                height: 15px;
            }

                .frameMrk div.RighListContainer div.c_Container_Box div.righIc_Container input.ed:hover {
                    background-position: 0 -0!important;
                }

            .frameMrk div.RighListContainer div.c_Container_Box div.righIc_Container input.de {
                background: url(images/delet0_2.png) left top!important;
                width: 15px;
                height: 15px;
                background-position: 0 -21px !important;
                display: block;
                float: left;
                margin-left: 15px;
                border: none;
            }

                .frameMrk div.RighListContainer div.c_Container_Box div.righIc_Container input.de:hover {
                    background-position: 0 -0!important;
                }

            .frameMrk div.RighListContainer div.c_Container_Box div.righIc_Container input.cl {
                background: url(images/cale0_3.png) left top!important;
                width: 15px;
                height: 15px;
                background-position: 0 -21px!important;
                display: block;
                float: left;
                margin-left: 15px;
                border: none;
            }

                .frameMrk div.RighListContainer div.c_Container_Box div.righIc_Container input.cl:hover {
                    background-position: 0 -0!important;
                }
        /*-------------------Right Container End---------------------*/

        .borderRadius {
            border-radius: 3px;
            float: left;
        }

        .borderRadiusTp {
            border-radius: 3px;
            float: left;
        }

        .borderRadiusBtm {
            border-radius: 3px;
            float: left;
        }

        .ddcommon {
            position: relative;
            display: -moz-inline-stack;
            zoom: 1;
            display: inline-block;
            *display: inline;
            cursor: default;
        }

            .ddcommon ul {
                padding: 0;
                margin: 0;
            }

                .ddcommon ul li {
                    list-style-type: none;
                }



            .ddcommon .clear {
                clear: both;
            }

            .ddcommon input.text {
                color: #7e7e7e;
                padding: 0 0 0 0;
                position: absolute;
                background: #fff;
                display: block;
                width: 98%;
                height: 98%;
                left: 2px;
                top: 0;
                border: none;
            }

        .ddOutOfVision {
            float: left;
        }

        img.fnone {
            float: none !important;
        }

        .ddcommon .divider {
            width: 0;
            height: 100%;
            position: absolute;
        }

        .ddcommon .arrow {
            display: inline-block;
            position: absolute;
            top: 50%;
            right: 4px;
        }

            .ddcommon .arrow:hover {
                background-position: 0 100%;
            }

        .ddcommon .ddTitle {
            padding: 0;
            position: relative;
            display: inline-block;
            width: 100%;
        }

            .ddcommon .ddTitle .ddTitleText {
                display: block;
            }

                .ddcommon .ddTitle .ddTitleText .ddTitleText {
                    padding: 0;
                }

            .ddcommon .ddTitle .description {
                display: block;
            }

            .ddcommon .ddTitle .ddTitleText img {
                position: relative;
                vertical-align: middle;
                float: left;
            }

        .ddcommon .ddChild {
            position: absolute;
            display: none;
            width: 100%;
            overflow-y: auto;
            overflow-x: hidden;
            zoom: 1;
            z-index: 9999;
        }

            .ddcommon .ddChild li {
                clear: both;
            }

                .ddcommon .ddChild li .description {
                    display: block;
                }

                .ddcommon .ddChild li img {
                    border: 0 none;
                    position: relative;
                    vertical-align: middle;
                    float: left;
                }

                .ddcommon .ddChild li.optgroup {
                    padding: 0;
                }

                    .ddcommon .ddChild li.optgroup .optgroupTitle {
                        padding: 0 5px;
                        font-weight: bold;
                        font-style: italic;
                    }

                    .ddcommon .ddChild li.optgroup ul li {
                        padding: 5px 5px 5px 15px;
                    }

        .ddcommon .noBorderTop {
            border-top: none 0 !important;
            padding: 0;
            margin: 0;
        }

        #pages_child {
            display: block;
            max-height: 200px;
            height: auto !important;
            position: absolute;
            top: 30px !important;
            visibility: visible;
            z-index: 9999;
            overflow: auto;
        }

        /*************** default theme **********************/
        .dd {
            border: 1px solid #c3c3c3;
            margin-right: 5px;
            padding: 2px 0 3px 0;
        }

            .dd .divider {
                border-left: 1px solid #c3c3c3;
                border-right: 1px solid #fff;
                right: 24px;
            }

            .dd .arrow {
                width: 16px;
                height: 16px;
                margin-top: -8px;
                background: url(images/dd_arrow.gif) no-repeat;
            }

                .dd .arrow:hover {
                    background-position: 0 100%;
                }

            .dd .ddTitle {
                color: #666;
                font-family: Arial, Helvetica, sans-serif;
                font-size: 12px;
                font-weight: normal;
            }

                .dd .ddTitle .ddTitleText {
                    padding: 3px 20px 2px 5px;
                    float: left;
                }

                    .dd .ddTitle .ddTitleText .ddTitleText {
                        padding: 0;
                    }

                .dd .ddTitle .description {
                    font-size: 12px;
                    color: #666;
                }

                .dd .ddTitle .ddTitleText img {
                    padding-right: 5px;
                }

            .dd .ddChild {
                border: 1px solid #c3c3c3;
                background-color: #fff;
                left: -1px;
            }

                .dd .ddChild li {
                    padding: 2px 5px 2px 5px;
                    background-color: #fff;
                    border-bottom: 1px solid #c3c3c3;
                }

                    .dd .ddChild li .description {
                        color: #666;
                    }

                    .dd .ddChild li .ddlabel {
                        color: #666;
                        font-family: Arial, Helvetica, sans-serif;
                        font-size: 12px;
                        text-align: left;
                        font-weight: normal;
                    }

                    .dd .ddChild li.hover {
                        background-color: #b5cdeb;
                    }

                    .dd .ddChild li img {
                        padding: 0 6px 0 0;
                    }

                    .dd .ddChild li.optgroup {
                        padding: 0;
                    }

                        .dd .ddChild li.optgroup .optgroupTitle {
                            padding: 0 5px;
                            font-weight: bold;
                            font-style: italic;
                        }

                        .dd .ddChild li.optgroup ul li {
                            padding: 5px 5px 5px 15px;
                        }

                    .dd .ddChild li.selected {
                        background-color: #a0c0ce;
                        color: #000;
                    }



        a.homecls, a.homecls:link, a.homecls:visited {
            background: url("images/hme.png")repeat scroll 0 0 transparent;
            float: right;
            height: 50px;
            margin: 0 2% 0 0;
            padding: 0;
            width: 48px;
            cursor: pointer;
            display: block;
        }

            a.homecls:hover {
                background-position: 0 -57px;
            }


        a.lessnClass, a.lessnClass:link, a.lessnClass:visited {
            background: url("images/lessonmanagement.png") repeat scroll 0 0 transparent;
            float: right;
            height: 50px;
            margin: 0 1% 0 0;
            padding: 0;
            width: 48px;
            cursor: pointer;
            display: block;
        }

            a.lessnClass:hover {
                background-position: 0 -57px;
            }

        a.repocls, a.repocls:link, a.repocls:visited {
            background: url("images/repo.png") repeat scroll 0 0 transparent;
            float: right;
            height: 50px;
            margin: 0 1% 0 0;
            padding: 0;
            width: 48px;
            cursor: pointer;
            display: block;
        }

            a.repocls:hover {
                background-position: 0 -57px;
            }

        /*#dashboard-content-panel {
            margin: 0 auto 0 35px;
            width: 1352px;
        }*/

        .alertGreen {
            background-color: #8deb9e;
            border: 1px solid #157e28;
            color: #157e28;
            left: 44%;
            padding: 5px;
            position: fixed;
            top: 2px;
        }
        .alertRed {
            background-color: #FF99AF;
            border: 1px solid #FF0034;
            color: #FF0034;
            left: 44%;
            padding: 5px;
            position: fixed;
            top: 2px;
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
 function loadmaster() {

            self.location = '../../LoginContinue.aspx';



        }
    </script>


    <script type="text/javascript">

        $(document).ready(function () {
            $('#close_x').click(function () {
                $('#dialog').animate({ top: "-300%" }, function () {
                    document.getElementById("<%=txtLessonName.ClientID%>").value == "";
                    $('#overlay').fadeOut('slow');

                });
            });

            
        });




    
    function fadeAlert() {
        var seconds = 5;
        setTimeout(function () {
            $("#<%=lblAlert.ClientID %>").fadeOut('slow');
        }, seconds * 1000);

        $("#<%=lblAlert.ClientID %>").click(function () {
            $(this).fadeOut();
        });

    }

    </script>




</head>

<body>
    <!-- top panel -->
    <form runat="server" id="form1">

        <div id="dashboard-top-panel">

            <div id="top-panel-container">
                <ul>

                    <li class="user" style="width: 50%;">
                        <asp:Label ID="lblLoginName" runat="server" Text="Label"></asp:Label>
                    </li>

                    <li class="timeSs">
                        <div>
                            <div style="float: left; width: auto;">
                            </div>
                            <div style="float: left; width: auto;" class="jclock"></div>
                        </div>
                    </li>
                    <li>
                        <a href="#" title="StartUp Page" onclick="loadmaster();">
                            <img src="images/StartHome.png" width="10" height="10" align="baseline" />
                            Landing Portal</a>

                    </li>
                    <li class="box40">
                        <a href="../Administration/AdminHome.aspx">Administration</a>
                    </li>




                </ul>






            </div>
        </div>
        <!-- dashboard container panel -->
        <div id="db-container">
            <!-- header -->
            <div id="header-panel">
                <div class="Dashboard-logo">
                    <img src="../Administration/images/student-logo.jpg" alt="">

                    <a href="homePage.aspx" class="homecls" title="Home"></a>
                    <a href="repository-manag.aspx" class="repocls" title="Repository Management"></a>
                </div>
            </div>
            <!-- content panel -->
            <div id="dashboard-content-panel">
                <!-- content Left side -->



                <!-- content right side -->
                <div id="dashboard-RHS">

                    <div class="dashboard-RHS-content">



                        <div id="main-heading-panel" style="">Lesson Management</div>

                        <div class="clear"></div>
                        <hr />
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                        <div class="frameMrk">
                            <!------------LFT Container Start------------------->
                            <div class="lftListContainer">

                                <asp:DataList ID="DListDomains" runat="server" Width="100%">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="lblDomainName" CssClass="lC" runat="server" Text='<%# Eval("AsmntCatName") %>' CommandArgument='<%# Eval("domainId") %>'></asp:LinkButton>

                                    </ItemTemplate>
                                </asp:DataList>

                            </div>
                            <!------------LFT Container End------------------->
                            <!------------Righ Container Start------------------->
                            <div class="RighListContainer">
                                <div class="topMenuBar">
                                    <asp:Button ID="btnAllCat" runat="server" CssClass="al" OnClick="btnAllCat_Click" />
                                    <asp:Button ID="btnCoin" runat="server" CssClass="co" OnClick="btnCoin_Click" />
                                    <asp:Button ID="btnMouse" runat="server" CssClass="mo" OnClick="btnMouse_Click" />
                                    <asp:Button ID="btnMatch" runat="server" CssClass="ma" OnClick="btnMatch_Click" />
                                    <asp:Button ID="btnTime" runat="server" CssClass="ti" OnClick="btnTime_Click" />
                                    <asp:Button ID="btnContents" runat="server" CssClass="con" OnClick="btnContents_Click" />
                                    <%-- <a class="al current" href="#"></a>
                                    <a class="co" href="#"></a>
                                    <a class="mo" href="#"></a>
                                    <a class="ma" href="#"></a>
                                    <a class="ti" href="#"></a>
                                    <a class="con" href="#"></a>--%>
                                </div>
                                <div class="clear">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblAlert" runat="server" Font-Names="Times New Roman" Font-Size="15px" Visible="false" Style="margin-left: 25px;"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="secBtnpartContainer">


                                    <asp:DropDownList ID="ddlDomain" runat="server" CssClass="tRbx" OnSelectedIndexChanged="ddlDomain_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>




                                    <asp:TextBox ID="txtSearch" runat="server" Height="25" CssClass="tRbx"></asp:TextBox>

                                    <asp:Button ID="imgSearch" runat="server" CssClass="btn btn-orange" OnClick="imgSearch_Click" ToolTip="search lesson name" />
                                    <%--   <input id="ctl00_PageContent_TextBox_StudentName" class="tRbx" type="text" maxlength="30" name="ctl00$PageContent$TextBox_StudentName">
                                    <input id="ctl00_PageContent_Button_Search" class="btn btn-orange" type="submit" value="" name="ctl00$PageContent$Button_Search">--%>


                                    <asp:Button ID="btnCreateNew" runat="server" CssClass="blbx" OnClick="btnCreateNew_Click" />



                                </div>
                                <div class="clear"></div>

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="c_Container_Box" style="height: auto;">
                                            <asp:GridView ID="grdLessonPlans" runat="server" AutoGenerateColumns="False" Width="100%" EmptyDataText="No Data Found.." AllowPaging="True" OnPageIndexChanging="grdLessonPlans_PageIndexChanging" GridLines="None" OnRowCommand="grdLessonPlans_RowCommand" OnRowEditing="grdLessonPlans_RowEditing" OnRowDeleting="grdLessonPlans_RowDeleting" ShowHeader="False" OnRowDataBound="grdLessonPlans_RowDataBound" OnSelectedIndexChanged="grdLessonPlans_SelectedIndexChanged">
                                                <AlternatingRowStyle CssClass="odd" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="LessonPlan Name">
                                                        <ItemTemplate>


                                                            <div class="righIc_Container">

                                                                <asp:Button ID="imgEdit" runat="server" class="ed" CommandName="Edit" CommandArgument='<%# Eval("lpId") %>' />
                                                                <asp:Button ID="imgDelete" runat="server" class="de" CommandName="Delete" CommandArgument='<%# Eval("lpId") %>' />
                                                                <asp:Button ID="imgCopy" runat="server" class="cl" CommandName="Copy" ToolTip="Copy LessonPlan" CommandArgument='<%# Eval("lpId") %>' OnClientClick="getPopup();" />
                                                                <asp:HiddenField ID="hfLPid" runat="server" Value='<%# Eval("lpId") %>' />
                                                                <%-- <a href="#" class="ed"></a>
                                                                <a href="#" class="de"></a>
                                                                <a href="#" class="cl"></a>--%>
                                                            </div>

                                                            <h2>[<asp:Label ID="lblLessonName" runat="server" Font-Overline="False" Text='<%# Eval("lpName") %>'></asp:Label>] - 
                                                                [<asp:Label ID="lblLessonId" runat="server" Text='<%# Eval("lpId") %>'></asp:Label>] -
                                                                <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("studName") %>'></asp:Label>
                                                            </h2>

                                                            <p class="bx">Domain :<asp:Label ID="lblDomainName" runat="server" ForeColor="#BF0000" Text='<%# Eval("domName") %>'></asp:Label></p>
                                                            <p>Category :<asp:Label ID="lblCatName" runat="server" ForeColor="#BF0000" Text='<%# Eval("categryName") %>'></asp:Label></p>

                                                            <div class="clear"></div>
                                                            <p>
                                                                <asp:Label ID="lblLessonDesc" runat="server" Text='<%# Eval("desc") %>'></asp:Label>
                                                            </p>



                                                            <%--    </ContentTemplate>
                                                            </asp:UpdatePanel>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="HeaderStyle" />

                                                <RowStyle CssClass="even" Height="6em" />
                                                <FooterStyle CssClass="FooterStyle" />
                                                <SelectedRowStyle CssClass="SelectedRowStyle" />
                                                <PagerStyle CssClass="PagerStyle" />
                                                <EditRowStyle CssClass="even" />
                                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                            </asp:GridView>

                                        </div>



                                        <div id="overlay" class="web_dialog_overlay">
                                        </div>
                                        <div id="dialog" class="web_dialog" style="width: 700px; height: 250px; left: 38%;">
                                            <div id="sign_up5">
                                                <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                                                <h3>Rename Option

                                       
                                                </h3>
                                                <hr />

                                                <table style="width: 100%;">

                                                    <tr>
                                                        <td style="width: 50%;">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td style="width: 40%;"></td>
                                                                    <td class="tdText">Current Lesson Name
                                                                    </td>
                                                                </tr>

                                                            </table>


                                                        </td>
                                                        <td class="tdText">
                                                            <asp:Label ID="lblCurrentLessonName" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 50%;">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td style="width: 40%;"></td>
                                                                    <td class="tdText">New Lesson Name
                                                                    </td>
                                                                </tr>

                                                            </table>

                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtLessonName" runat="server" CssClass="textClass" MaxLength="45"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="text-align: center;">
                                                            <asp:Button ID="btnRenameDone" runat="server" CssClass="NFButtonBi" OnClick="btnRenameDone_Click" Text="Done" />
                                                        </td>

                                                    </tr>

                                                </table>



                                            </div>



                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>







                                <div class="clear"></div>

                            </div>
                            <!------------Righ Container End------------------->
                            <div class="clear"></div>
                        </div>



                    </div>

                </div>
            </div>


            <!-- footer -->
            <div id="footer-panel">
                <ul>
                    <li>&copy; Copyright 2015, Melmark, Inc. All rights reserved.</li>
                </ul>
            </div>


        </div>
    </form>
</body>


<script type="text/javascript">

    function getPopup() {



    }
   
</script>

<%--<script>
    function createByJson() {
        var jsonData = [
                        { description: 'Choos your payment gateway', value: '', text: 'Payment Gateway' },

        ];
        $("#byjson").msDropDown({ byJson: { data: jsonData, name: 'payments2' } }).data("dd");
    }
    $(document).ready(function (e) {
        //no use
        try {
            var pages = $("#ddlDomain").msDropdown({
                on: {
                    change: function (data, ui) {
                        var val = data.value;

                    }
                }
            }).data("dd");

            var pagename = document.location.pathname.toString();
            pagename = pagename.split("/");
            pages.setIndexByValue(pagename[pagename.length - 1]);
            $("#ver").html(msBeautify.version.msDropdown);
        } catch (e) {
            //console.log(e);	
        }

        $("#ver").html(msBeautify.version.msDropdown);

        //convert
        $("select").msDropdown();
        createByJson();
        $("#tech").data("dd");
    });
    function showValue(h) {
        console.log(h.name, h.value);
    }
    $("#tech").change(function () {
        console.log("by jquery: ", this.value);
    })
    //
</script>--%>
</html>
