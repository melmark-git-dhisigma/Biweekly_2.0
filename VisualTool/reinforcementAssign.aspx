<%@ Page Language="C#" AutoEventWireup="true" CodeFile="reinforcementAssign.aspx.cs" Inherits="reinforcementAssign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title runat="server" id="TitleName">Melmark Pennsylvania</title>

    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="styles/LandDesign.css" rel="stylesheet" />
    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="styles/ContentPage.css" rel="stylesheet" type="text/css" />
    <link href="styles/repmanage.css" rel="stylesheet" />
    <link href="styles/jquery.mCustomScrollbar.css" rel="stylesheet" />
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />

    <script type="text/javascript" src="scripts/swfobject.js"></script>

    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <script src="scripts/checkbox.js" type="text/javascript"></script>

    <script type="text/javascript" src="scripts/jquery.mousewheel.min.js"></script>
    <script type="text/javascript" src="scripts/jquery.mCustomScrollbar.js"></script>

    <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>
    <script type="text/javascript">
        $(function ($) {
            $('.jclock').jclock();
        });

 function loadmaster() {

            self.location = '../../LoginContinue.aspx';



        }
    </script>

    <style type="text/css">
        .imgPreviewtp {
            width: 25px;
            height: 25px;
            margin: 2px 5px 0 0;
            background: #F2F2F2;
            font-weight: bold;
            color: #fff !important;
            cursor: pointer;
            border: none;
        }

            .imgPreviewtp:hover {
                cursor: pointer;
                background-color: lightblue;
            }

        h2 {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 14px;
        }

        .draggable1, .draggable2 {
            background: none repeat scroll 0 0 #E8E8E8;
            float: left;
            margin: 4px;
            padding: 6px;
        }

        .content span input {
            margin: 0 0 0 12px;
            display: block;
            width: 16px;
            float: left;
        }

        .imgPreview {
            margin: 0 19px 0 0;
            display: block;
            width: 14px;
            height: 14px;
            float: right;
        }

        .droppableDiv1 input,
        .droppableDiv2 input {
            margin: 0 15px 0 20px;
            display: block;
            width: 16px;
            float: left;
        }

        .droppableDiv1.imgPreview {
            margin: 0 0 0 0 !important;
            display: block;
            width: 14px;
            height: 14px;
            float: right;
        }
        /*----------------------------------------------------------------------*/
        #header-panel {
            width: 100%;
            height: auto;
            margin: 0px auto;
            padding: 0px;
        }

        #top-panel-container {
            width: 93%;
            height: 47px;
            margin: 0px auto;
            padding: 0px;
        }

            #top-panel-container ul {
                margin: 1% 0 0 0;
                padding: 0;
                list-style: none;
                float: right;
                width: 100%;
                height: 20px;
                font-family: Arial, Helvetica, sans-serif;
                font-weight: normal;
                text-align: left;
                color: #fff;
                font-size: 100%;
                float: left !important;
                position: static;
                z-index: 999;
            }

                #top-panel-container ul li.timeS {
                    width: 5%;
                    height: 20px;
                    color: #fff;
                    font-weight: normal;
                    float: left;
                    background: url(../images/clock.PNG) left 3px no-repeat;
                    padding: 0 0 0 18px;
                    letter-spacing: 1px;
                    margin: 0 5px;
                    cursor: pointer;
                    display: block;
                    float: left;
                    margin-left: 2%;
                }


            #top-panel-container li.box0 {
                width: 5%;
                float: left;
                height: 20px;
                background: url(../images/chkintop.PNG) left 4px no-repeat;
                padding: 0 1% 0 2%;
                text-align: left;
                letter-spacing: 1px;
                cursor: pointer;
                display: block;
                float: left;
                margin-left: 1%;
            }

            #top-panel-container li.box1 {
                width: 6%;
                float: left;
                height: 20px;
                background: url(../images/chkintop.PNG) left 4px no-repeat;
                padding: 0 1% 0 2%;
                text-align: left;
                letter-spacing: 1px;
                cursor: pointer;
                display: block;
                float: left;
                margin-left: 1%;
            }

            #top-panel-container li.box2 {
                width: 5%;
                float: left;
                height: 20px;
                background: url(../images/bellsmall.PNG) left 4px no-repeat;
                padding: 0 1% 0 2%;
                text-align: left;
                letter-spacing: 1px;
                cursor: pointer;
                display: block;
                float: left;
                margin-left: 1%;
            }

            #top-panel-container li.box3 {
                width: 7%;
                float: left;
                height: 20px;
                background: url(../images/chkinsml.PNG) left 4px no-repeat;
                padding: 0 2% 0 2%;
                text-align: left;
                letter-spacing: 1px;
                z-index: 100;
                cursor: pointer;
                display: block;
                float: left;
                margin-left: 1%;
            }

            #top-panel-container li.box4 {
                width: 6%;
                float: left;
                height: 20px;
                background: url(../images/tophome.PNG) left 2px no-repeat;
                padding: 0 2% 0 2%;
                text-align: left;
                letter-spacing: 1px;
                z-index: 100;
                cursor: pointer;
                display: block;
                float: left;
                margin-left: 1%;
            }

            #top-panel-container li.box5 {
                width: 2%;
                float: left;
                height: 20px;
                background: url(../images/logontop.PNG) left 3px no-repeat;
                padding: 0 2% 0 2%;
                text-align: left;
                letter-spacing: 1px;
                margin: 0 0 0 5px;
                z-index: 100;
                cursor: pointer;
                display: block;
                float: left;
                margin-left: 1%;
            }

            #top-panel-container li a,
            #top-panel-container li a:link,
            #top-panel-container li a:visited {
                color: #fff;
                text-decoration: none;
                text-align: left;
                font-size: 11px;
                letter-spacing: 1px;
                margin: 0;
                padding: 0;
            }

                #top-panel-container li a:hover {
                    color: #CCC;
                    text-decoration: none;
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
                    <a href="LessonManagement.aspx" class="lessnClass" title="Lesson Management"></a>
                    <a href="repository-manag.aspx" class="repocls" title="Repository Management"></a>
                </div>


            </div>

            <!-- content panel -->

            <!-- content Left side -->

            <div class="clear"></div>
            <!-- content right side -->
            <div id="dashboard-RHSVin">

                <div class="dashboard-RHS-conten">
                    <div id="main-heading-panel">
                        Assign Reinforcement - 
                        <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label>
                    </div>
                    <!-- CHANGABLE -->

                    <div class="clear"></div>
                    <hr />

                    <span id="lblAlert" style="display: block;"></span>
                    <!-- CHANGABLE -->


                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <asp:HiddenField ID="hfCorrect" runat="server" />
                    <asp:HiddenField ID="hfWrong" runat="server" />
                    <div style="float: left; margin-left: -36px; padding-right: 2px; height: 50px; width: 15px;">
                        <tfoot>
                            <tr class="odd">
                                <th colspan="8">



                                    <div id="btn_finish" style="margin-top: 2px; cursor: pointer;" title="Save" class="with-tip">
                                        <div>
                                            <img src="images/savebtnn.png" height="15px" width="15px">
                                        </div>
                                    </div>
                                    <div id="btn_close" style="margin-top: 2px; cursor: pointer;" title="Close" class="with-tip">
                                        <div>
                                            <img src="images/deletnwblk.png" height="15px" width="15px">
                                        </div>
                                    </div>

                                </th>
                            </tr>
                        </tfoot>
                    </div>
                    <div id="mainContainer">
                        <%--<div>
                <table width="100%;">
                    <tr>
                        <td align="center" style="width: 10%; border-right: 3px dashed #FFB765;">

                            
                        </td>
                        <td style="vertical-align: top; background-color: bisque;">
                            <div style="margin-left: 5px;">
                                <asp:Label ID="lblName" runat="server" Text="Student Name"></asp:Label>
                                <br />
                                <asp:Label ID="lblClass" runat="server" Text="Student Name"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>--%>

                        <div class="correctAnswerDiv">
                            <div class="divHeader">Correct Answer Template</div>
                            <table width="100%;">
                                <tr>
                                    <td width="65%">
                                        <div class="draggableDiv">

                                            <%--<div style="position: absolute; float: left;">
                                    <input style="position: absolute;" id="btn_Addd" type="button" value="Add" class="finalButtons" />
                                </div>--%>
                                            <div id="Div2" class="content">
                                                <asp:DataList ID="dl_corrAns" runat="server"
                                                    Width="100%" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <ItemTemplate>
                                                        <div style="float: left; margin: 0; padding: 0; height: 140px; width: 140px;" class='divSingleImage'>
                                                            <table class="display">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:Image ID="Image1" runat="server" CssClass="draggable1 Image1" ondblclick="PreviewSWF(this);" ImageUrl='icons/flashicon2.png' Width="100px" Height="100px" Style="margin: 3px 10px 3px 10px;" class="selectable draggable" AlternateText='<%# Eval("MediaId") %>' ToolTip='<%# "Name: "+Eval("Name")+"\nDescription: "+Eval("Description") %>' /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <input id="delete" type="checkbox" runat="server" title="Click to select or unselect" /></td>
                                                                    <td><span class="imageName"><%# Eval("Name") %></span></td>
                                                                    <td>
                                                                        <img alt='<%# Eval("Path") %>' src="images/zoome2r.png" class="imgPreview" height="14px" width="14px" onclick="PopUp(this.alt);" /></td>
                                                                </tr>
                                                            </table>
                                                        </div>

                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="width: 2%; padding: 0px 0px 0px 0px;">
                                        <div style="float: right; vertical-align: middle;">

                                            <img id="btn_Add" src="icons/right.png" width="30px" height="30px" class="imgPreviewtp" style="border: 1px solid Gray;" />
                                        </div>
                                        <div style="float: left;">

                                            <img id="btn_Remove" src="icons/leftarrow.png" width="30px" height="30px" class="imgPreviewtp" style="border: 1px solid Gray;" />
                                        </div>
                                    </td>
                                    <td style="width: 35%;">

                                        <div class="droppableDiv1 dropable">
                                        </div>
                                    </td>
                                </tr>

                            </table>
                        </div>
                        <div>

                            <div class="divHeader">WRONG ANSWER TEMPLATE</div>
                            <table width="100%;">
                                <tr>
                                    <td width="65%">
                                        <div class="draggableDiv">

                                            <%--<div style="position: absolute; float: left;">
                                    <input style="position: absolute;" id="btn_AdddWrng" type="button" value="Add" class="finalButtons" />
                                </div>--%>
                                            <div id="Div1" class="content">
                                                <asp:DataList ID="dl_wrngAns" runat="server"
                                                    Width="100%" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <ItemTemplate>
                                                        <div style="float: left; width: 140px; height: 140px;" class='divSingleImage'>

                                                            <table class="display">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:Image ID="Image1" runat="server" CssClass="draggable2 Image1" ondblclick="PreviewSWF(this);" ImageUrl='icons/flashicon2.png' Width="100px" Height="100px" Style="margin: 3px 10px 3px 10px;" class="selectable draggable" AlternateText='<%# Eval("MediaId") %>' ToolTip='<%# "Name: "+Eval("Name")+"\nDescription: "+Eval("Description") %>' /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <input id="delete" type="checkbox" runat="server" title="Click to select or unselect" /></td>
                                                                    <td><span class="imageName"><%# Eval("Name") %></span></td>
                                                                    <td>
                                                                        <img alt='<%# Eval("Path") %>' src="images/zoome2r.png" class="imgPreview" height="14px" width="14px" onclick="PopUp(this.alt);" /></td>
                                                                </tr>
                                                            </table>

                                                        </div>
                                                    </ItemTemplate>
                                                </asp:DataList>

                                            </div>
                                        </div>

                                    </td>
                                    <td style="width: 2%; padding: 0px 0px 0px 0px;">
                                        <div style="float: right;">

                                            <img id="btn_AddWrng" src="icons/right.png" width="30px" height="30px" class="imgPreviewtp" style="border: 1px solid Gray;" />
                                        </div>
                                        <div style="float: left;">

                                            <img id="btn_RemWrng" src="icons/leftarrow.png" width="30px" height="30px" class="imgPreviewtp" style="border: 1px solid Gray;" />
                                        </div>
                                    </td>
                                    <td style="width: 35%;">

                                        <div class="droppableDiv2 dropable">

                                            <%--<div style="position: absolute; float: left;">
                                <input id="btn_RemWrngg" style="position: absolute;" type="button" value="Remove" class="finalButtons" />
                            </div>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="finalButtons">
                            <%--<input id="btn_finish" type="button" value="Finish" class="finalButtons" />
                <input id="btn_close" type="button" value="Back" class="finalButtons" onclick="javascript:window.location='StudentLessonAndScore.aspx'" />--%>
                        </div>
                    </div>
                    <div class="overlay">
                    </div>
                    <div id="previewSWF" class="popUp">
                        <div class="close-popUp">
                            <img alt="" src="icons/close.png" />
                        </div>
                        <div class="box">
                            <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" ViewStateMode="Enabled">
                            <ContentTemplate>--%>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <div id="content-container" style="width: 100%; height: 255px; margin-top: 0px;">
                                            <div id="content"></div>
                                        </div>
                                    </td>
                                </tr>

                            </table>
                            <%--   </ContentTemplate>
                          
                        </asp:UpdatePanel>--%>
                        </div>
                    </div>





                    <!-- CHANGEABLE -->
                </div>
                <div class="clear"></div>
            </div>



            <!-- footer -->
            <div id="footer-panel">
                <ul>
                    <li>&copy; Copyright 2015, Melmark, Inc. All rights reserved.</li>
                </ul>
            </div>


        </div>
    </form>

    <script type="text/javascript">

        // $(document).ready();
        function makeDraggable() {

            $('.selectedDraggable').mousedown(function () {
                $('.trashBox').fadeIn('fast');
            });
            $('.selectedDraggable,.trashBox').mouseup(function () {
                $('.trashBox').fadeOut('slow');
            });
        }
        function PopUp(file) {
            $('.overlay').fadeIn('slow', function () {
                //$('#previewSWF').fadeIn('slow');
                if (swfobject.hasFlashPlayerVersion("6")) {

                    //file = "Repository/reinforcement/tangramone.swf";
                    // check if SWF hasn't been removed, if this is the case, create a new alternative content container
                    var c = document.getElementById("content");
                    if (!c) {
                        var d = document.createElement("div");
                        d.setAttribute("id", "content");
                        document.getElementById("content-container").appendChild(d);
                    }
                    var extension = file.substr((file.lastIndexOf('.') + 1));

                    switch (extension) {
                        case 'swf':// create SWF
                            file = file.replace('~/', '');
                            $('#previewSWF').fadeIn('slow');
                            var att = { data: file, width: "325", height: "250" };
                            var par = { menu: "false" };
                            var id = "content";
                            swfobject.createSWF(att, par, id);
                            break;
                        case 'gif':// create img for gif
                            $('#previewSWF').fadeIn('slow');
                            var c = document.getElementById("content");
                            if (c == null) {
                                var d = document.createElement("div");
                                d.setAttribute("id", "content");
                                document.getElementById("content-container").appendChild(d);
                                c = document.getElementById("content");
                            }
                            c.innerHTML = "<img src='" + file + "' height='250px' width='325px' />";
                            break;
                        default:
                            alert('Invalid file format');
                            $('.overlay').fadeOut('fast');
                    }

                }
            });
        }



        $(document).ready(function () {

            $('.close-popUp').click(function () {
                $('#previewSWF').fadeOut('slow');
                $('.overlay').fadeOut('slow');

            });
            //DRAGGING AND DROPING EVENTS
            $(".draggable1,.draggable2").draggable({
                revert: "invalid",
                helper: "clone"
            });



            $(".droppableDiv1").droppable({
                accept: ".draggable1",
                drop: function (event, ui) {

                    var imageName = $(ui.draggable).parents('div.divSingleImage').find('.imageName').html();
                    //var newElement = ui.draggable.clone();
                    var newElement = ui.draggable;

                    //$(ui.draggable).parent().parent().remove();

                    var src = $(ui.draggable).parents('div.divSingleImage').find('.draggable1').attr('src');
                    var alt = $(ui.draggable).parents('div.divSingleImage').find('.draggable1').attr('alt');
                    var path = $(ui.draggable).parents('div.divSingleImage').find('.imgPreview').attr('alt');
                    var title = $(ui.draggable).parents('div.divSingleImage').find('.draggable1').attr('title');

                    $(ui.draggable).parents('div.divSingleImage').parent().remove();



                    var newDiv = "<div style='float: right;width:85px;' class='divSingleImage'>" +
                                "<img class='draggable1 Image1' src='" + src + "' ondblclick='PreviewSWF(this);' width='50px' height='50px' style='margin: 3px 10px 3px 10px;' class='selectable draggable' alt='" + alt + "' title='" + title + "' />" +
                                "<br />" +
                                "<input type='checkbox' title='Click to select or unselect' /><span class='imageName' title='" + imageName + "'></span>" +
                                "<img alt='" + path + "' src='images/zoome2r.png' class='imgPreview' height='12px' width='12px' onclick='PopUp(this.alt);' />" +
                                "</div>";
                    $(this).append(newDiv);

                    //$(newElement).parent().remove();

                    //$(newElement).removeClass('draggable');
                    //$(".selectedDraggable").draggable({
                    //    revert: "invalid"
                    //});

                    //makeDraggable();

                }
            }).sortable();

            $(".droppableDiv2").droppable({
                accept: ".draggable2",
                drop: function (event, ui) {

                    var imageName = $(ui.draggable).parents('div.divSingleImage').find('.imageName').html();


                    var src = $(ui.draggable).parents('div.divSingleImage').find('.draggable2').attr('src');
                    var alt = $(ui.draggable).parents('div.divSingleImage').find('.draggable2').attr('alt');
                    var path = $(ui.draggable).parents('div.divSingleImage').find('.imgPreview').attr('alt');
                    var title = $(ui.draggable).parents('div.divSingleImage').find('.draggable2').attr('title');

                    $(ui.draggable).parents('div.divSingleImage').parent().remove();

                    var newDiv = "<div style='float: right;width:85px;' class='divSingleImage'>" +
                                "<img class='draggable1 Image1' src='" + src + "' ondblclick='PreviewSWF(this);' width='50px' height='50px' style='margin: 3px 10px 3px 10px;' class='selectable draggable' alt='" + alt + "' title='" + title + "' />" +
                                "<br />" +
                                "<input type='checkbox' title='Click to select or unselect' /><span class='imageName' title='" + imageName + "'></span>" +
                                "<img alt='" + path + "' src='images/zoome2r.png' class='imgPreview' height='12px' width='12px' onclick='PopUp(this.alt);' />" +
                                "</div>";
                    $(this).append(newDiv);


                    //$(newElement).removeClass('draggable');
                    //$(".selectedDraggable").draggable({
                    //    revert: "invalid"
                    //});

                    //makeDraggable();
                }
            });

            //DRAGGING AND DROPPIN EVENTS END

            $(".deleteDropable").droppable({
                accept: ".selectedDraggable",
                drop: function (event, ui) {
                    var elemnt = ui.draggable;
                    $(elemnt).remove();
                }
            });

            $('#btn_finish').click(function () {
                var length1 = $('.droppableDiv1 .Image1').length;
                var length2 = $('.droppableDiv2 .Image1').length;


                var imgList1 = '0';
                var imgList2 = '0';

                for (var i = 0; i < length1; i++) {
                    var elem = $('.droppableDiv1 .Image1').eq(i);
                    imgList1 = imgList1 + ',' + elem.attr('alt');
                }

                for (var i = 0; i < length2; i++) {
                    var elem = $('.droppableDiv2 .Image1').eq(i);
                    imgList2 = imgList2 + ',' + elem.attr('alt');
                }
                imgList1 = imgList1.substring(2, imgList1.length);
                imgList2 = imgList2.substring(2, imgList2.length);

                saveAjax(imgList1, imgList2);
            });
            $('#btn_close').click(function () {
                window.location = 'StudentLessonAndScore.aspx';
            });
            $('#btn_Add').click(function () {
                var cbList = $('#dl_corrAns input');
                for (var i = 0; i < cbList.length; i++) {
                    if (cbList[i].checked == true) {

                        var div = $('.droppableDiv1');
                        $(cbList[i]).parents('div.divSingleImage').parent().remove();


                        var src = $(cbList[i]).parents('div.divSingleImage').find('img.draggable1').attr('src');
                        var alt = $(cbList[i]).parents('div.divSingleImage').find('img.draggable1').attr('alt');
                        var path = $(cbList[i]).parents('div.divSingleImage').find('img.imgPreview').attr('alt');
                        var title = $(cbList[i]).parents('div.divSingleImage').find('img.draggable1').attr('title');
                        var imageName = $(cbList[i]).parents('div.divSingleImage').find('.imageName').html();
                        var newDiv = "<div style='float: right;width:85px;' class='divSingleImage'>" +
                                "<img class='draggable1 Image1' src='" + src + "' width='50px' height='50px' ondblclick='PreviewSWF(this);' style='margin: 3px 10px 3px 10px;' class='selectable draggable' alt='" + alt + "' title='" + title + "' />" +
                                "<br />" +
                                "<input type='checkbox' title='Click to select or unselect' /><span class='imageName' title='" + imageName + "'></span>" +
                                "<img alt='" + path + "' src='images/zoome2r.png' class='imgPreview' height='12px' width='12px' onclick='PopUp(this.alt);' />" +
                                "</div>";
                        div.append(newDiv);
                    }
                }
            });
            $('#btn_Remove').click(function () {
                var cbList = $('.droppableDiv1 input');
                var imgList = $('.droppableDiv1 img');
                for (var i = 0; i < cbList.length; i++) {
                    if (cbList[i].checked == true) {
                        var parnt = $(cbList[i]).parent()[0];
                        var img = $(cbList[i]).parent().find('.draggable1');
                        var imageName = $(cbList[i]).parent().find('.imageName').attr('title');

                        var src = $(cbList[i]).parent().find('.draggable1').attr('src');
                        var alt = $(cbList[i]).parent().find('.draggable1').attr('alt');
                        var path = $(cbList[i]).parent().find('.imgPreview').attr('alt');
                        var title = $(cbList[i]).parent().find('.draggable1').attr('title');
                        var div = $('#dl_corrAns');
                     

                        var newDiv = "<span>" +
" <div class='divSingleImage' style='float: left; margin: 0; padding: 0; height: 140px; width: 140px;'>" +
                                                           "  <table class='display'>" +
                                                              " <tbody><tr>" +
                                                                 "  <td colspan='3'><img class='draggable1' src='" + src + "' width='100px' height='100px' ondblclick='PreviewSWF(this);' style='margin: 3px 10px 3px 10px;' class='selectable draggable' alt='" + alt + "' title='" + title + "' /></td>" +
                                                                                    "  </tr>" +
                                                                                     " <tr>" +
                                                                                      "    <td><input type='checkbox' title='Click to select or unselect' /></td>" +
                                                                                       "   <td><span class='imageName'>" + imageName + "</span></td>" +
                                                                                        "  <td><img alt='" + path + "' src='images/zoome2r.png' class='imgPreview' height='14px' width='14px' onclick='PopUp(this.alt);' /></td>" +
                                                                                      "</tr>" +
                                                                                  "</tbody></table>" +
                                                                                  "</div>" +
                                                       "</span>";

                        div.append(newDiv);
                        $(parnt).remove();

                    }
                }
                $(".draggable1").draggable({
                    revert: "invalid",
                    helper: "clone"
                });
            });
            $('#btn_AddWrng').click(function () {
                var cbList = $('#dl_wrngAns input');
                for (var i = 0; i < cbList.length; i++) {
                    if (cbList[i].checked == true) {

                        var div = $('.droppableDiv2');
                        $(cbList[i]).parents('div.divSingleImage').parent().remove();


                        var src = $(cbList[i]).parents('div.divSingleImage').find('img.draggable2').attr('src');
                        var alt = $(cbList[i]).parents('div.divSingleImage').find('img.draggable2').attr('alt');
                        var path = $(cbList[i]).parents('div.divSingleImage').find('img.imgPreview').attr('alt');
                        var title = $(cbList[i]).parents('div.divSingleImage').find('img.draggable2').attr('title');
                        var imageName = $(cbList[i]).parents('div.divSingleImage').find('.imageName').html();
                        var newDiv = "<div style='float: right;width:85px;' class='divSingleImage'>" +
                                "<img class='draggable2 Image1' src='" + src + "' width='50px' height='50px' ondblclick='PreviewSWF(this);' style='margin: 3px 10px 3px 10px;' class='selectable draggable' alt='" + alt + "' title='" + title + "' />" +
                                "<br />" +
                                "<input type='checkbox' title='Click to select or unselect' /><span class='imageName' title='" + imageName + "'></span>" +
                                "<img alt='" + path + "' src='images/zoome2r.png' class='imgPreview' height='12px' width='12px' onclick='PopUp(this.alt);' />" +
                                "</div>";
                        div.append(newDiv);
                    }
                }
            });
            $('#btn_RemWrng').click(function () {
                var cbList = $('.droppableDiv2 input');
                var imgList = $('.droppableDiv2 img');
                for (var i = 0; i < cbList.length; i++) {
                    if (cbList[i].checked == true) {
                        var parnt = $(cbList[i]).parent()[0];
                        var img = $(cbList[i]).parent().find('.draggable2');
                        var imageName = $(cbList[i]).parent().find('.imageName').attr('title');

                        var src = $(cbList[i]).parent().find('.draggable2').attr('src');
                        var alt = $(cbList[i]).parent().find('.draggable2').attr('alt');
                        var path = $(cbList[i]).parent().find('.imgPreview').attr('alt');
                        var title = $(cbList[i]).parent().find('.draggable2').attr('title');
                        var div = $('#dl_wrngAns');


                        var newDiv = "<span>" +
" <div class='divSingleImage' style='float: left; margin: 0; padding: 0; height: 140px; width: 140px;'>" +
                                                           "  <table class='display'>" +
                                                              " <tbody><tr>" +
                                                                 "  <td colspan='3'><img class='draggable2' src='" + src + "' width='100px' height='100px' ondblclick='PreviewSWF(this);' style='margin: 3px 10px 3px 10px;' class='selectable draggable' alt='" + alt + "' title='" + title + "' /></td>" +
                                                                                    "  </tr>" +
                                                                                     " <tr>" +
                                                                                      "    <td><input type='checkbox' title='Click to select or unselect' /></td>" +
                                                                                       "   <td><span class='imageName'>" + imageName + "</span></td>" +
                                                                                        "  <td><img alt='" + path + "' src='images/zoome2r.png' class='imgPreview' height='14px' width='14px' onclick='PopUp(this.alt);' /></td>" +
                                                                                      "</tr>" +
                                                                                  "</tbody></table>" +
                                                                                  "</div>" +
                                                       "</span>";

                        div.append(newDiv);
                        $(parnt).remove();

                    }
                }
                $(".draggable2").draggable({
                    revert: "invalid",
                    helper: "clone"
                });

            });

        });

        function PreviewSWF(img) {

            var path = $(img).parents('div.divSingleImage').find('.imgPreview').attr('alt');
            PopUp(path);
        }

        function saveAjax(imgList1, imgList2) {
            $.ajax({
                url: "reinforcementAssign.aspx/saveFile",
                data: "{ 'correctAns': '" + imgList1 + "','wrongAns':'" + imgList2 + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    save = true;
                    if (data.d == "success") {
                        var seconds = 5;
                        $('#lblAlert').removeClass('alertRed');
                        $('#lblAlert').addClass('alertGreen');
                        $('#lblAlert').html("Data Saved");
                        $('#lblAlert').fadeIn();
                        setTimeout(function () {
                            $("#lblAlert").fadeOut('slow');
                        }, seconds * 1000);

                    }
                    else {
                        var seconds = 5;
                        $('#lblAlert').removeClass('alertGreen');
                        $('#lblAlert').addClass('alertRed');
                        $('#lblAlert').html("Data Not Saved");
                        $('#lblAlert').fadeIn();
                        setTimeout(function () {
                            $("#lblAlert").fadeOut('slow');
                        }, seconds * 1000);

                    }


                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        }





    </script>
    <script>
        //call after page loaded
        window.onload = function loadReinOfStud() {
            var wrong = document.getElementById('<%=hfWrong.ClientID%>').value;

            var wrongList = wrong.split(',');
            var cbList = $('#dl_wrngAns input');
            for (var i = 0; i < cbList.length; i++) {

                var div = $('.droppableDiv2');

                var alt = $(cbList[i]).parents('div.divSingleImage').find('img.draggable2').attr('alt');
                for (var j = 0; j < wrongList.length; j++) {

                    if (alt == wrongList[j]) {
                        $(cbList[i]).parents('div.divSingleImage').parent().remove();
                        var src = $(cbList[i]).parents('div.divSingleImage').find('img.draggable2').attr('src');

                        var path = $(cbList[i]).parents('div.divSingleImage').find('img.imgPreview').attr('alt');
                        var title = $(cbList[i]).parents('div.divSingleImage').find('img.draggable2').attr('title');
                        var imageName = $(cbList[i]).parents('div.divSingleImage').find('.imageName').html();
                        var newDiv = "<div style='float: right;width:85px;' class='divSingleImage'>" +
                                "<img class='draggable2 Image1' src='" + src + "' width='50px' height='50px' ondblclick='PreviewSWF(this);' style='margin: 3px 10px 3px 10px;' class='selectable draggable' alt='" + alt + "' title='" + title + "' />" +
                                "<br />" +
                                "<input type='checkbox' title='Click to select or unselect' /><span class='imageName' title='" + imageName + "'></span>" +
                                "<img alt='" + path + "' src='images/zoome2r.png' class='imgPreview' height='12px' width='12px' onclick='PopUp(this.alt);' />" +
                                "</div>";
                        div.append(newDiv);
                    }
                }
            }

            var correct = document.getElementById('<%=hfCorrect.ClientID%>').value;

            var corrctList = correct.split(',');
            var cbList = $('#dl_corrAns input');
            for (var i = 0; i < cbList.length; i++) {
                var div = $('.droppableDiv1');
                var alt = $(cbList[i]).parents('div.divSingleImage').find('img.draggable1').attr('alt');
                for (var j = 0; j < corrctList.length; j++) {

                    if (alt == corrctList[j]) {
                        $(cbList[i]).parents('div.divSingleImage').parent().remove();
                        var src = $(cbList[i]).parents('div.divSingleImage').find('img.draggable1').attr('src');
                        var alt = $(cbList[i]).parents('div.divSingleImage').find('img.draggable1').attr('alt');
                        var path = $(cbList[i]).parents('div.divSingleImage').find('img.imgPreview').attr('alt');
                        var title = $(cbList[i]).parents('div.divSingleImage').find('img.draggable1').attr('title');
                        var imageName = $(cbList[i]).parents('div.divSingleImage').find('.imageName').html();
                        var newDiv = "<div style='float: right;width:85px;' class='divSingleImage'>" +
                                "<img class='draggable1 Image1' src='" + src + "' width='50px' height='50px' ondblclick='PreviewSWF(this);' style='margin: 3px 10px 3px 10px;' class='selectable draggable' alt='" + alt + "' title='" + title + "' />" +
                                "<br />" +
                                "<input type='checkbox' title='Click to select or unselect' /><span class='imageName' title='" + imageName + "'></span>" +
                                "<img alt='" + path + "' src='images/zoome2r.png' class='imgPreview' height='12px' width='12px' onclick='PopUp(this.alt);' />" +
                                "</div>";
                        div.append(newDiv);
                    }
                }
            }
        }
    </script>

</body>
</html>

