<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/Home.aspx.cs" Inherits="Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <meta name="viewport" content="width=device-width, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttonshome.css" rel="stylesheet" type="text/css" />
    <script src="../Administration/JS/123.js" type="text/javascript"></script>
    <script src="../Administration/JS/script.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <script src="../Administration/JS/jquery-ui.js"></script>
    <script src="../Administration/JS/fastclick.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.min.js"></script>
    <script type="text/javascript" src="../Administration/JS/ddaccordion.js"></script>
    <link href="../Administration/CSS/toolTip.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src="../Administration/JS/toolTipScripts.js"></script>
    <script src="../Administration/JS/jquery111110005.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery_004.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery_002.js" type="text/javascript"></script>
    <script src="../Administration/JS/homSevond.js" type="text/javascript"></script>
    <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" type="text/css" />


    <style type="text/css">
        .lftPartContainer {
            margin: 0 auto;
            height: 0 auto;
            width: 75%;
            height: auto !important;
            min-height: 655px;
            border: 3px solid #0D668E;
            float: left;
        }

        .rightPartContainer {
            border: 3px solid #0D668E;
            float: left;
            height: auto !important;
            margin: 0 auto 0 0.2%;
            min-height: 500px;
            width: 23%;
            display: none;
        }

        .lftPartContainerIpad {
            margin: 0 auto;
            height: 0 auto;
            width: 71%;
            height: auto !important;
            min-height: 500px;
            border: 3px solid #0D668E;
            float: left;
            padding-right: 1%;
        }

        .rightPartContainerIpad {
            border: 3px solid #0D668E;
            float: right;
            height: auto !important;
            margin: 0 auto 0 0.2%;
            min-height: 500px;
            width: 26.5%;
            display: none;
        }

        .rdBox {
            float: right;
            margin-left: 3px;
            width: 25px;
            display: block;
        }

            .rdBox div {
                color: blue;
                float: left;
                margin-left: 3px;
                display: block;
            }

        .lpStatus {
            display: block;
            float: right;
            text-align: right;
            width: 80px;
        }

        .lpName {
            float: left;
            /*width: 216px;*/
            padding: 3px;
        }
        .lpName2 {
            float: left;
            /*width: 216px;*/
            padding: 3px;
            width:200px;

        }
        .lpName1 {
            float: left;
            /*width: 216px;*/
            padding: 3px;
            height:74px;
            background:orange;
            width:56px;
            word-wrap:break-word;
            font-size:11px;
        }
        .rightBox {
            /*background-color: #bce773;*/
            float: left;
            height: 20px;
            margin-left: 195px;
            margin-top: 50px;
            position: absolute;
            width: 108px;
            font-size: 11px;
        }
        .rightBox1 {
            /*background-color: #bce773;*/
            float: left;
            height: 20px;
            margin-right: 50px;
            margin-top: 50px;
            position: absolute;
            width: 108px;
            font-size: 11px;
        }

        .ioaNeed, .mtNeed {
            background-color: #FF6060;
            color: white;
            font-family: consolas;
            float: left;
            padding: 0 3px 0 3px;
            margin-left: 2px;
        }

        .leftDiv {
            margin-top: 50px;
            position: absolute;
            left: 0px;
            margin-right:50px;
        }

        .bhvOpt {
            width: 30px;
            height: 20px;
            padding: 3px;
            border: 1px solid white;
            float: left;
        }

        .notifyBox {
            background-color: red;
            border: 1px solid red;
            color: white;
            font-size: 18px;
            height: 17px;
            padding: 3px;
            position: absolute;
            text-align: center;
            display: none;
        }

        .students2 {
            background-color: #0d668e;
            width: 100%;
            float: left;
        }

        .stdSel {
            background-color: #FFCC00;
        }

        .stdAct {
            background-color: #00CCFF;
        }

    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad) {

                document.getElementById("newBehFrame").style.width = "250px";
                document.getElementById("bhvTitle").style.width = "92px";
                document.getElementById("bhvTitle").style.fontSize = "20px";
                document.getElementById("newBehTop").style.width = "252px";
                document.getElementById("newBehStdList").style.width = "250px";
                width = "248px";
            } else {
                width = "314px";
            }

            $(window).bind('resize', function (event) {
                //listing all datasheets
                var datasheets = $('[id^="divTF"]').not('#divTFGraph');
                if (datasheets.length > 0) {
                    for (var i = 0; i < datasheets.length; i++) {

                        //Looking for any visible datasheet.

                        if ($(datasheets[i]).css('display') == 'block') {
                            var lftPartContainer_W = $('.lftPartContainer').width() + 15;
                            var windowWidth = $(window).width();
                            if ((lftPartContainer_W + 318) < windowWidth && lftPartContainer_W > 0 > 300) {
                                $('#newBehFrame').css('right', 'auto').css('margin-left', lftPartContainer_W + 'px');
                                $('#newBehTop').css('right', 'auto').css('margin-left', lftPartContainer_W + 'px');

                                return;
                            }


                        }
                    }
                }
                // If code get this far, then there is no visible datasheet.

                $('#newBehFrame').css('right', '0');

                $('#newBehTop').css('right', '0');
            });
        })
        //New Graph landing Page added on 12-08-2020 Dev 1 start ---
        function setName1(n) {
            document.getElementById('<%= graphdivClose.ClientID%>').value = "1";
            var title="";
            if (n == 1) { title = "Academic Report"; $("#Academic").trigger('click'); }
            if (n == 2) {title = "Clinical Report"; $("#Clinical").trigger('click');}
            if (n == 3) { title = "Session-based Graph"; $("#Session-based").trigger('click'); }
            if (n == 4) { title = ""; $("#ChainGraphTab").trigger('click'); }
            if (n == 5) {title = "Excel View Report"; $("#ExcelView").trigger('click'); }
            if (n == 6) {title = "Progress Summary Report"; $("#ProgressSummaryReport").trigger('click'); }
            if (n == 7) { title = "Progress Summary Report"; $("#ClinicalProgressSummaryReport").trigger('click'); }
            if (n == 8) { $("#GraphTab").trigger('click'); }
            document.getElementById('tdTitle').innerHTML = title;
        }
        //New Graph landing Page added on 12-08-2020 Dev 1 END ---

        function setBehavPanelLeft(lftPartContainer_W) {
            var windowWidth = $(window).width();
            if ((lftPartContainer_W + 318) < windowWidth && lftPartContainer_W > 300) {
                $('#newBehFrame').css('right', 'auto').css('margin-left', lftPartContainer_W + 'px');
                $('#newBehTop').css('right', 'auto').css('margin-left', lftPartContainer_W + 'px');
            }
        }

        var isiPad = navigator.userAgent.match(/iPad/i);

        if (isiPad != null) {
            window.addEventListener('load', function () {
                new FastClick(document.body);
            }, false);
        }
    </script>

    <script type="text/javascript">
        function Notify(name) {
            $('.notifyBox').show();
            var $el = $('.stdTimerButton'), x = 3000, originalColor = "#0D668E", OrginalText = "Student Timers", OrginaltColor = "White";

            $el.css("background", "#FFCC00");
            $el.html(name + " was expired!");
            $el.css('color', 'black');
            setTimeout(function () {
                $el.css("background-color", originalColor);
                $el.html(OrginalText);
                $el.css('color', OrginaltColor);
            }, x);
            //stdTimerButton

            var initCount = parseInt($('.notifyBox').html());
            $('.notifyBox').html(initCount + 1);
        }
        function listLessonPlan() {
            $('#DATASHEETS').trigger('click');
        }

        function goLeft() {

            $('.second').find('.scrollableArea').css('margin-left', function (index, curValue) {
                return parseInt(curValue, 10) + 100 + 'px';
            });
        }
        function goRight() {
            $('.second').find('.scrollableArea').css('margin-left', function (index, curValue) {
                return parseInt(curValue, 10) - 100 + 'px';
            });
        }
        function resetScroll() {
            $('.second').find('.scrollableArea').css('margin-left', '0px');
        }

        $(window).resize(function () {

            window.resizeTo(screen.availWidth + 20, screen.availHeight + 20);

        });

        function ClosePopup() {
            $('#dialog').animate({ top: "-300%" }, function () {
                $('#overlay').fadeOut('slow');
            });
            $('#LessonPlanDetails').html("");
            //[2019-02-25] disable DashBoard
            //document.getElementById('tdTitle').innerHTML = "Dashboard";
        }

        function loadmaster() {

            var currHref = window.location.href.replace('http://', '');

            var currHref_splitted = currHref.split('/');

            var redirectUrl = currHref_splitted[0] + '/';

            for (var i = 0; i < currHref_splitted.length; i++) {
                if (currHref_splitted[i].substring(0, 2) == '(S') {
                    redirectUrl += currHref_splitted[i] + '/';
                }

            }
            redirectUrl += 'LoginContinue.aspx';

            //alert(redirectUrl);

            self.location = redirectUrl;



        }

        function SearchLessonPopup1() {
            $('#LessonPlanDetails').html("");
            var type = $('#PopupType').val();
            var searchtext = $('#txtLesson').val();

            var dayFlag = true;
            var resFlag = true;

            if (type != "DatasheetTab") {

                dayFlag = $('#chk_day').is(":checked");
                resFlag = $('#chk_res').is(":checked");

            }
            else {

                dayFlag = $('#chk_day_ds').is(":checked");
                resFlag = $('#chk_res_ds').is(":checked");
            }

            var option = 0;

            if (dayFlag == true && resFlag == true) {
                option = 0;
            }
            if (dayFlag == true && resFlag == false) {
                option = 1;
            }
            if (dayFlag == false && resFlag == true) {
                option = 2;
            }
            if (dayFlag == false && resFlag == false) {
                option = 3;
            }


            $.ajax(
                  {
                      type: "POST",
                      url: "Home.aspx/SearchLessonPlanList",
                      data: "{'Tab':'" + type + "','SearchCondition':'" + searchtext + "','option':'" + option + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,
                      success: function (data) {
                          // $('#LessonPlanDetails').append(data.d);

                          $('.lessonDSDetails').empty();
                          $('.fillLessons1').hide();
                          $('.fillLessons2').hide();
                          $('.fillLessons').show();
                          $('.lessonDSDetails').append(data.d);
                      },
                      error: function (request, status, error) {
                          alert("Error");
                      }
                  });
            // $('.LessonPlanList[title]').tooltip();
        }

        function SearchLessonPopup() {
            $('#LessonPlanDetails').html("");
            var type = $('#PopupType').val();
            var searchtext = $('#txtLessonName').val();

            var dayFlag = true;
            var resFlag = true;

            if (type != "DatasheetTab") {

                dayFlag = $('#chk_day').is(":checked");
                resFlag = $('#chk_res').is(":checked");

            }
            else {

                dayFlag = $('#chk_day_ds').is(":checked");
                resFlag = $('#chk_res_ds').is(":checked");
            }

            var option = 0;

            if (dayFlag == true && resFlag == true) {
                option = 0;
            }
            if (dayFlag == true && resFlag == false) {
                option = 1;
            }
            if (dayFlag == false && resFlag == true) {
                option = 2;
            }
            if (dayFlag == false && resFlag == false) {
                option = 3;
            }


            $.ajax(
                  {
                      type: "POST",
                      url: "Home.aspx/SearchLessonPlanList",
                      data: "{'Tab':'" + type + "','SearchCondition':'" + searchtext + "','option':'" + option + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,
                      success: function (data) {
                          // $('#LessonPlanDetails').append(data.d);

                          $('.lessonListDetails').empty();
                          $('.fillLessons1').show();
                          $('.fillLessons2').hide();
                          $('.fillLessons').hide();
                          $('.lessonListDetails').append(data.d);
                      },
                      error: function (request, status, error) {
                          alert("Error");
                      }
                  });
            // $('.LessonPlanList[title]').tooltip();
        }





        function ListLessonPopup(lessontype) {


            $('.filterOption span').css('color', 'blue');

            $('#LessonPlanDetails').html("");
            var type = $('#PopupType').val();
            var actualType = lessontype;




            var dayFlag = true;
            var resFlag = true;

            if (type != "DatasheetTab") {

                dayFlag = $('#chk_day').is(":checked");
                resFlag = $('#chk_res').is(":checked");

            }
            else {

                dayFlag = $('#chk_day_ds').is(":checked");
                resFlag = $('#chk_res_ds').is(":checked");
            }

            var option = 0;

            if (dayFlag == true && resFlag == true) {
                option = 0;
            }
            if (dayFlag == true && resFlag == false) {
                option = 1;
            }
            if (dayFlag == false && resFlag == true) {
                option = 2;
            }
            if (dayFlag == false && resFlag == false) {
                option = 3;
            }



            if (type == "LessonPlanTab") {

                if (lessontype == "Inactive") {
                    actualType = "LessonPlanTab_inactive";
                    $('.filterOption_inac').css('color', 'red');
                }
                else if (lessontype == "Approve") {
                    actualType = "LessonPlanTab_approve";
                    $('.filterOption_appr').css('color', 'red');
                }
                else if (lessontype == "Maintenance") {
                    actualType = "LessonPlanTab_maintenance";
                    $('.filterOption_main').css('color', 'red');
                }
                else if (lessontype == "All") {

                    var vatValue = "";
                    $('#txtLesson').val(vatValue);

                    SearchLessonPopup();

                    actualType = "LessonPlanTab";
                    $('.filterOption_all').css('color', 'red');
                }
                else if (lessontype == "rd") {
                    actualType = "LessonPlanTab_rd";
                    $('.filterOption_rd').css('color', 'red');
                }
                else if (lessontype == "Next") {
                    actualType = "Next";
                    $('.filterOption_rd').css('color', 'red');
                }
                else if (lessontype == "Previous") {
                    actualType = "Previous";
                    $('.filterOption_rd').css('color', 'red');
                }
            }
            else if (type == "GraphTab") {
                if (lessontype == "Inactive") {
                    actualType = "GraphTab_inactive";
                    $('.filterOption_inac').css('color', 'red');
                }
                else if (lessontype == "Approve") {
                    actualType = "GraphTab_approved";
                    $('.filterOption_appr').css('color', 'red');
                }
                else if (lessontype == "Maintenance") {
                    actualType = "GraphTab_maintenance";
                    $('.filterOption_main').css('color', 'red');
                }
                else if (lessontype == "All") {
                    actualType = "GraphTab";
                    $('.filterOption_all').css('color', 'red');
                }
                else if (lessontype == "rd") {
                    actualType = "GraphTab_rd";
                    $('.filterOption_rd').css('color', 'red');
                }
            }
            else if (type == "ChainGraphTab") {
                if (lessontype == "Inactive") {
                    actualType = "ChainGraphTab_inactive";
                    $('.filterOption_inac').css('color', 'red');
                }
                else if (lessontype == "Approve") {
                    actualType = "ChainGraphTab_approved";
                    $('.filterOption_appr').css('color', 'red');
                }
                else if (lessontype == "Maintenance") {
                    actualType = "ChainGraphTab_maintenance";
                    $('.filterOption_main').css('color', 'red');
                }
                else if (lessontype == "All") {
                    actualType = "ChainGraphTab";
                    $('.filterOption_all').css('color', 'red');
                }
                else if (lessontype == "rd") {
                    actualType = "ChainGraphTab_rd";
                    $('.filterOption_rd').css('color', 'red');
                }
            }
            else if (type == "DatasheetTab") {
                if (lessontype == "all") {
                    var vatValue = "";
                    $('#txtLesson').val(vatValue);

                    SearchLessonPopup1();
                    actualType = "DatasheetTab";
                    $('.filterOption_all').css('color', 'red');

                }
                else if (lessontype == "Approve") {
                    actualType = "DatasheetTab_approved";
                    $('.filterOption_appr').css('color', 'red');
                }
                else if (lessontype == "Maintenance") {
                    actualType = "DatasheetTab_maintenance";
                    $('.filterOption_main').css('color', 'red');
                }
                else if (lessontype == "rd") {
                    actualType = "DatasheetTab_rd";
                    $('.filterOption_rd').css('color', 'red');
                }
            }


            var searchtext = $('#txtLessonName').val();
            if (searchtext == "") {
                var searchtext = $('#txtLesson').val();
            }
            $.ajax(
                  {
                      type: "POST",
                      url: "Home.aspx/SearchLessonPlanList",
                      data: "{'Tab':'" + actualType + "','SearchCondition':'" + searchtext + "','option':'" + option + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,
                      success: function (data) {
                          //$('#LessonPlanDetails').append(data.d);



                          if (type == "DatasheetTab") {
                              $('.lessonDSDetails').empty();
                              $('.lessonDSDetails').append(data.d);
                          }
                          else {
                              $('.lessonListDetails').empty();
                              $('.lessonSearchOptions').show();
                              $('.lessonListDetails').append(data.d);
                          }
                      },
                      error: function (request, status, error) {
                          alert("Error");
                      }
                  });
            // $('.LessonPlanList[title]').tooltip();
        }



        function CloseOverlayOnSelect() {
            $('#dialog').animate({ top: "-300%" }, function () {
                $('#overlay').fadeOut('slow');
            });
            $('#LessonPlanDetails').html("");
        }
        function PopupLessonPlans(Type) {
            $('#txtLessonName').val('');
            $('#txtLesson').val('');
            $('#PopupType').val(Type);
            $('.fillLessons1').hide();
            $('.fillLessons2').hide();
            $('.fillLessons').hide();

            $('#divContentPages > div').hide();
            //$('.filterOptions span').css('color', 'blue');
            $('.filterOption_main').css('color', 'blue');
            $('.filterOption_appr').css('color', 'blue');
            $('.filterOption_inac').css('color', 'blue');
            $('.filterOption_all').css('color', 'red');

            if (Type == "GraphTab") {
                $('#tdTitle').html('');
                $('#submenu_active').removeAttr('id');
                $('#Graph').parent().attr('id', 'submenu_active');

            }

            // $('#overlay').fadeIn('slow', function () {
            //  $('#dialog').animate({ top: '20%' }, { duration: 'slow', easing: 'linear' })
            // });
            // alert(Type);
            $.ajax(
                  {
                      type: "POST",
                      url: "Home.aspx/SelectLessonPlanList",
                      data: "{'Tab':'" + Type + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,

                      success: function (data) {
                          //$('#LessonPlanDetails').append(data.d);

                          $('.lessonListDetails').empty();
                          $('.lessonSearchOptions').show();
                          $('.fillLessons1').show();
                          $('.lessonListDetails').append(data.d);
                      },
                      error: function (request, status, error) {
                          alert("Error");
                      }
                  });
            //$('.LessonPlanList[title]').tooltip();
        }

        function PopupLessonPlans1(Type) {
            $('#txtLessonName').val('');
            $('#txtLesson').val('');
            $('#PopupType').val(Type);
            $('.fillLessons1').hide();
            $('.fillLessons').hide();
            $('.fillLessons2').hide();

            $('#divContentPages > div').hide();

            //$('#overlay').fadeIn('slow', function () {
            //    $('#dialog').animate({ top: '20%' }, { duration: 'slow', easing: 'linear' })
            //});
            $('.filterOption_main').css('color', 'blue');
            $('.filterOption_appr').css('color', 'blue');
            $('.filterOption_all').css('color', 'red');
            $.ajax(
                  {
                      type: "POST",
                      url: "Home.aspx/SelectLessonPlanList",
                      data: "{'Tab':'" + Type + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,
                      success: function (data) {
                          $('.fillLessons').show();

                          $('.lessonDSDetails').empty();
                          $('.lessonDSDetails').append(data.d);
                      },
                      error: function (request, status, error) {
                          alert("Error");
                      }
                  });
            //$('.LessonPlanList[title]').tooltip();
        }
        function PopupLessonPlans2(Type) {

            $('.Schedule_View').css('color', 'blue');
            $('.List_View').css('color', 'grey');
            $('#txtLessonName').val('');
            $('#txtLesson').val('');
            $('#PopupType').val(Type);
            $('.fillLessons1').hide();
            $('.fillLessons').hide();
            $('.fillLessons2').hide();

            $('#divContentPages > div').hide();

            //$('#overlay').fadeIn('slow', function () {
            //    $('#dialog').animate({ top: '20%' }, { duration: 'slow', easing: 'linear' })
            //});
            $('.filterOption_main').css('color', 'blue');
            $('.filterOption_appr').css('color', 'blue');
            $('.filterOption_all').css('color', 'red');
            $.ajax(
                  {
                      type: "POST",
                      url: "Home.aspx/SelectLessonPlanList",
                      data: "{'Tab':'" + Type + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,
                      success: function (data) {
                          $('.fillLessons2').show();

                          $('.lessonDSDetails').empty();
                          $('.lessonDSDetails').append(data.d);
                      },
                      error: function (request, status, error) {
                          alert("Error");
                      }
                  });
            //$('.LessonPlanList[title]').tooltip();
        }
        function StudentMouseOver(Id) {

            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad == null) {


                var e = document.getElementById(Id);
                var divS = e;
                var offset = { x: 0, y: 0 };
                while (e) {
                    offset.x += e.offsetLeft;
                    offset.y += e.offsetTop;
                    e = e.offsetParent;

                }

                if (document.documentElement && (document.documentElement.scrollTop || document.documentElement.scrollLeft)) {
                    offset.x -= document.documentElement.scrollLeft;
                    offset.y -= document.documentElement.scrollTop;
                }
                else if (document.body && (document.body.scrollTop || document.body.scrollLeft)) {
                    offset.x -= document.body.scrollLeft;
                    offset.y -= document.body.scrollTop;
                }
                else if (window.pageXOffset || window.pageYOffset) {
                    offset.x -= window.pageXOffset;
                    offset.y -= window.pageYOffset;
                }


                //alert(offset.x + '\n' + offset.y);

                var img = $(divS).find('img').attr('src');
                var pName = $(divS).find('div');

                var imgS = document.createElement('img');
                imgS.setAttribute('id', 'imgId');
                imgS.setAttribute('src', img);

                document.getElementById('dvStudPopup').innerHTML = "";

                $("#dvStudPopup").css('top', offset.y - 55 + "px");
                $("#dvStudPopup").css('left', offset.x + "px");


                document.getElementById('dvStudPopup').appendChild(imgS);
                $('#dvStudPopup').append('<div class="clear"></div>');
                $('#dvStudPopup').append('<p id="pS">' + pName.text() + '</p>');

                $('#dvStudPopup').show();
            }

        }


        function StudentMouseOut() {
            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad == null) {
                $('#dvStudPopup').fadeOut('slow');
            }

        }


        function error() {
            alert("Access Denied..!");
        }

        function adjustStyle(width) {
            width = parseInt(width);
            if (width >= 1100) {
                $('#menu').slideDown();
                document.getElementById('EDIT').style.display = "block";

                $("#sized").attr("href", "../Administration/CSS/homestyle.css");
                document.getElementById('hdIsipad').value = 0;

                return;
            }
            if (width < 1100) {

                document.getElementById('EDIT').style.display = "none";
                $("#sized").attr("href", "../Administration/CSS/tablet.css");
                document.getElementById('hdIsipad').value = 1;
                $('#slider').css("top", '73%');
                $('#dropper').removeAttr('onclick');

                $('.checkPopUp').css("width", '600px');
                $('#A3').css('margin-left', '580px');
                $('#A3').css('position', 'absolute');
                $('#A3').css('top', '-24px');
                $('#A3').removeClass('close sprited');
                $('#stCheckIn').css('width', '100%');
                $('#stCheckIn').css('height', '450px');


            }
        }



        $(function () {
            adjustStyle($(this).width());
            $(window).resize(function () {
                adjustStyle($(this).width());
            });
        });

        function FillDischargedStudents(stdId, classId) {
            $.ajax(
                  {
                      type: "POST",
                      url: "Home.aspx/selectDischargedStudent",
                      data: "{'stdId':'" + stdId +"'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,
                      success: function (data) {
                      },
                      error: function (request, status, error) {
                          alert("Error");
                      }
                  });
            window.location.href = "Home.aspx";
        }

        function ChangeClassId(ClassId) {
            $.ajax(
                   {
                       type: "POST",
                       url: "Home.aspx/ChangeClassId",
                       data: "{'ClassId':'" + ClassId + "'}",
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       async: false,
                       success: function (data) {
                       },
                       error: function (request, status, error) {
                           alert("Error");
                       }
                   });
            window.location.href = "Home.aspx";

        }
        function classBind() {
            $.ajax(
                  {
                      type: "POST",
                      url: "Home.aspx/fillclass",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,
                      success: function (data) {
                          $('#DlClass').empty();
                          $('#DlClass').append(data.d);
                      },
                      error: function (request, status, error) {
                          alert("Error");
                      }
                  });
        }
        function openDischargeDiv() {
            $(".classDiv").hide();
            $(".classDivNew").show();
            $("#imgSearchDsch").click()
        }
        function ConfirmChange() {
            $(".classDivNew").hide();
            var confirmResult = confirm("Are you Sure want to leave this class?");
            if (confirmResult.toString() == "true") {
                classBind();
                $(".classDiv").show();
            }
            else {
                $(".classDiv").hide();
            }
            $('.alertsPopUp').slideUp('fast');
            $('.checkPopUp').slideUp('fast');
        }

        function setStatusMenuPages(Id) {
            document.getElementById('tdTitle').innerText = "IEP Documentation";
            document.getElementById('Iframe1').src = "IEPDocumentaion.aspx?pageid=0&studid=" + currentStud.id + "";
            $('#stMenu').slideToggle();
            $('#stPage').slideToggle();
        }
        function EditIEP(type) {


            var studVal = currentStud.id;
            var strVals = studVal.split('-');
            var Id = strVals[1];



            var Count = parseInt(document.getElementById('hidOpenWindows').value) + 1;
            document.getElementById('hidOpenWindows').value = Count;

            if (type == 1) {

                var pagen = "IEPDocumentaion.aspx?pageid=0&studid=" + Id;
                var divname = "";
                var dname = "";
                var iname = "";

                dname = "divIEPEdit" + Id + "" + Count + "";
                iname = "divIIEPEdit" + Id + "" + Count + "";
                divname = "divSIEPEdit" + Id;

                $("#ulStudents").append("<div id='" + 'Stud' + dname + "' title='llll' style='cursor:pointer;' onclick='selSidemenu(" + dname + "," + Id + "," + ID + ",0);'><li class='sublist' id='" + 'li' + dname + "'><a href='#'>IEP Documentation</a></li></div></div>");

            }
            else if (type == 2) {

                var pagen = "Welcome.aspx?pageid=0&studid=" + Id;
                var divname = "";
                var dname = "";
                var iname = "";

                dname = "divW" + Id + "" + Count + "";
                iname = "divIW" + Id + "" + Count + "";
                divname = "divSW" + Id;
                // toggle();
            }
            var ID = 0;

            addFrame(dname, iname, pagen, Id);
            setVisiblityHideAll();
            document.getElementById(dname).style.display = "block";
            var iepedit = document.getElementById("IEPEdit");
            iepedit.style.visibility = "hidden";
        }


        function keyDownEvents() {
            jQuery(document).ready(function () {


                $(document).keypress(function (e) {
                    switch (e.keyC) {
                        // user presses the "a"
                        case 112: alert("Press a"); break;
                    }
                });
            });
        }

        function saveStartTime(elem) {

            var pageWidth = $(window).width();
            var msgLeft = ((pageWidth / 2) - 50);
            var thisParent = $(elem).parent().attr('id');
            var measureId = $(elem).find('.measurementId').html();
            var c_studId = $(elem).find('.c_studentId').html();

            var thisElement = $(elem).parents().eq(4);

            var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; background-color:yellow; position:fixed; top:10px; left:" + msgLeft + "px;'></div>";
            $(thisElement).find('.catFreqDiv').html("<img src='../Administration/images/34.gif'/>");
            $.ajax(
            {

                type: "POST",
                url: "Home.aspx/saveDurationStartTime",
                data: "{'MeasurementId':'" + measureId + "','StudentId':'" + c_studId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != '-1') {
                        $(elem).find('.retId').html(data.d);
                        $('body').append(alertBox);
                        $('.alertBox').html('Clock Started').fadeOut(10000, function () {
                            $(this).remove();
                        });


                        $(thisElement).find('.catFreqDiv').html("<img src='../Administration/images/clockRunning.gif'/>");
                    }
                    else {
                        $(thisElement).find('.catFreqDiv').html("<img src='../Administration/images/notsave.png'/>");

                        $(elem).remove();
                        addStopwatch(thisParent);

                        $('body').append(alertBox);
                        $('.alertBox').html('Clock Starting Failed. Try again!').fadeOut(10000, function () {
                            $(this).remove();
                        });
                    }


                },
                error: function (request, status, error) {
                    alert("Error");
                    $(thisElement).find('.catFreqDiv').html("");

                }
            });
        }

    </script>

    <script type="text/javascript">
        $(function ($) {
            $('.jclock').jclock();
        });


    </script>






    <script type="text/javascript">
        var currentMenu = null;
        var currentStud = null;
        var callType = 0;
        var _submenu = null;
        $(document).ready(function () {

            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad != null) {
                $('.box4').hide();
                $('#stdTimerFrame').attr('src', 'userTimers_ipad.aspx');
            }
            else {
                $('#stdTimerFrame').attr('src', 'userTimers.aspx');
            }

            $('.checkPopUp').slideUp('fast');

            //currentMenu = document.getElementById("menu_active");



            //$('#js-news').ticker();


            $('#showdiv').click(function () {
                //getBehaviour(4009);
                // var pageHeight = window.innerHeight;
                document.getElementById("scrolldiv").style.position = 'absolute';
                document.getElementById("scrolldiv").style.top = '0px';

                $('#scrolldiv').show();
                $('#scrolldiv').animate({ right: '0px' }, 500, 'linear');
            });

            $('#closediv').click(function () {
                $('#scrolldiv').animate({ right: '-300px' }, 500, 'linear', function () {
                    $('#scrolldiv').hide();
                });

            });
            $('.closeThis').click(function () {
                $('.classDrop').fadeOut('slow');
            });

            //PageMethods.FillStudnt(OnStudReturn, OnFailure);
            var url = 'DashboardReportNew.aspx?dashtype=Dashboard';
            var isiPad = navigator.userAgent.match(/iPad/i);

            //if (isiPad == null) {
            //[2019-02-25] disable DashBoard
                //addFrame('divTFGraph', 'divIDashboard', url, 0);
            //}
            //[2019-02-25] disable DashBoard
            //document.getElementById('tdTitle').innerHTML = "Dashboard";
        });

        //THIS FUNCTION IS TO SAVE THE TIMER VALUE IN THE DATABASE. CALLED WHEN TIMER IS STOPPED.
        function saveTimeInDB(timeValue, clickedButton, behavId) {
            var durationid = '0';

            var thisElement = $(clickedButton).parents().eq(4);
            var pageWidth = $(window).width();
            var msgLeft = ((pageWidth / 2) - 50);

            var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; background-color:yellow; width:100px; position:fixed; top:10px; left:" + msgLeft + "px;'></div>";

            $(thisElement).find('.catFreqDiv').html("<img src='../Administration/images/34.gif'/>");

            $.ajax(
            {

                type: "POST",
                url: "Home.aspx/getResultDuration",
                data: "{'BehaviourId':'" + behavId + "','Durationid':'" + durationid + "','DurationTime':'" + timeValue + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                    $('body').append(alertBox);
                    $('.alertBox').html('Clock Stopped').fadeOut(10000, function () {
                        $(this).remove();
                    });

                    $(thisElement).find('.clockSpan').html("[" + timeValue + " Sec]");
                    $(thisElement).find('.catFreqDiv').html("<img src='../Administration/images/ok.gif'/>");
                    $(thisElement).find('.catFreqDiv img').fadeOut(3000);
                },
                error: function (request, status, error) {
                    alert("Error");

                    $(thisElement).find('.catFreqDiv').html("<img src='../Administration/images/notsave.png'/>");
                    $(thisElement).find('.catFreqDiv img').fadeOut(3000);
                }
            });
        }
        function getDashBoardReport() {
            $('.fillLessons,.fillLessons1,.fillLessons2').hide();

            var divC = document.getElementById('divContentPages');

            for (var i = 0; i < divC.childNodes.length; i++) {
                document.getElementById(divC.childNodes[i].id).style.display = "none";
            }
            $('#divContentPages').find('#divTFGraph').remove();
            var url = 'DashboardReportNew.aspx?dashtype=Dashboard';
            addFrame('divTFGraph', 'divIDashboard', url, 0);
            document.getElementById('divTFGraph').style.display = 'block';
            document.getElementById('tdTitle').innerHTML = "Dashboard";


            //selSidemenu('divTFGraph', 1, 0);


        }
        function OnStudReturn(response) {
            //$("#studentslist").empty();
            //for (var i = 0; i < response.length; i++) {


            //    $("#studentslist").append("<div id=stud-" + response[i].StudID + " class='students' onclick='selStudent(this,0);'><ul>" +
            //                            "<li class='studentphoto'><a href='#'><img src='../Administration/StudentsPhoto/" + response[i].Img + "' width='36' height='37'></a></li>" +
            //                            "<li><a href='#'>" + response[i].StudName + "</a>" +
            //                            "<div class='close'><img id=studImg-" + response[i].StudID + " src='../Administration/images/RDdot.png' onclick='setStudentChecked(this," + response[i].StudID + ")' width='12' height='12'></div></li>" +
            //                            "</ul></div>");
            //}

            var url = 'DashboardReportNew.aspx?dashtype=Dashboard';
            var isiPad = navigator.userAgent.match(/iPad/i);

            //if (isiPad == null) {
            //[2019-02-25] disable DashBoard
                //addFrame('divTFGraph', 'divIDashboard', url, 0);
            //}
            //[2019-02-25] disable DashBoard
            //document.getElementById('tdTitle').innerHTML = "Dashboard";
        }
        function getDash() {
            //var statdiv = document.getElementById('divStatus');
            //if (statdiv.style.display == 'block')
            //    statdiv.style.display = "none";
            document.getElementById('dash_area').style.display = 'block';
            var dashtype = document.getElementById('<%=hfDashType.ClientID%>');

            var divC = document.getElementById('divContentPages');
            var str = divC.innerHTML;
            for (var i = 0; i < divC.childNodes.length; i++) {
                document.getElementById(divC.childNodes[i].id).style.display = "none";
            }
            var url;
            if (dashtype.value == 'Dashboard') {
                url = 'Dashboard.aspx';
            }
            if (dashtype.value == 'Calendar') {
                url = 'Calender.aspx?mode=Day';
            }

            if (document.getElementById('if_dash') == null) {

                ni = document.getElementById('dash_area');
                var ifr = document.createElement('iframe');
                ifr.setAttribute('id', 'if_dash');
                ifr.setAttribute('width', '100%');
                ifr.setAttribute('height', '720px');
                ifr.setAttribute('frameborder', '0');
                ifr.setAttribute('src', url);

                ni.appendChild(ifr);
            }
        }
        var selStdId = 0;


        function setStudentChecked(stdImg, Id) {

            var values = document.getElementById('hidActiveStudents').value;
            document.getElementById('hidActiveStudents').value = values + "," + Id;
            stdImg.src = "../Administration/images/GRdot.png";
            PageMethods.setActiveStudents(Id);
        }
        function setVisiblityHideAll() {

            var divC = document.getElementById('divContentPages');
            var searchEles = document.getElementById("divContentPages").children;

            if (searchEles.length > 0) {

                for (var i = 0; i < divC.childNodes.length; i++) {

                    document.getElementById(divC.childNodes[i].id).style.display = "none";
                }

            }
        }

        function showAlertStatus(stdId) {
            $.ajax({
                type: "POST",
                url: "Home.aspx/checkAlertCount",
                data: "{'studId':'" + stdId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var alertStatus = JSON.parse(data.d);
                    if(alertStatus){
                        //if((source.playbackState === source.PLAYING_STATE || source.playbackState === source.FINISHED_STATE)) {
                            document.getElementById("play").src="../Administration/images/icons8-bell-on.png";
                        //}
                    }else{
                        document.getElementById("play").src="../Administration/images/icons8-bell-off.png";
                    }
                },
                Error: function () {
                }
            });
        }

        function selStudent(div, opt) {                                                  // "div" get the selected students div, "opt": 1 to hide "divStatus"; 0 to show "divStatus"
            //[2019-02-25] disable DashBoard -Start
            $('.fillLessons,.fillLessons1,.fillLessons1').hide();
            var divC = document.getElementById('divContentPages');
            for (var i = 0; i < divC.childNodes.length; i++) {
                document.getElementById(divC.childNodes[i].id).style.display = "none";
            }
            $('#divContentPages').find('#divTFGraph').remove();//End
            //getDashBoardReport();
            document.getElementById('dash_area').style.display = 'none';

            hideAllPopUps();

            $("#ulsubmenu").empty();

            $('.students-active').addClass('students');
            $('.students-active').removeClass('students-active');

            $(div).removeClass('students');
            $(div).addClass('students-active');


            currentStud = div;


            selStdId = div.id;
            document.getElementById('hidCureentStudent').value = selStdId


            PageMethods.SetStudentID(div.id, OnSuccessStdAssign, OnFailure);
            PageMethods.setTemplateSess(div.id);


            PageMethods.setOverAllStatus(div.id, sucess);
            //var frame = document.getElementById('ifContent');


            //frame.src = 'BlankPage.aspx';
            getBehaviour(selStdId);


            //var IsPad = document.getElementById('hdIsipad').value;

            //if (IsPad == 1) {
            //    selMenuForDatasheet("DATASHEETS");
            //    $('#menu').slideUp();
            //}
            //else {

            //    $('#menu,#submenu').slideDown();
            //    $('#dropper').removeClass('bottodown');
            //    $('#dropper').addClass('bottomtop');

            //}

            selStudent2(div, opt);

            showAlertStatus(selStdId);
            
            var ctrl = document.getElementById("<%= lblLoginName.ClientID %>").id
            PageMethods.Setllabelname(ctrl,sucesslbl);
            
        }

        function selStudent2(div, opt) {
            if (opt == 3) {
                selStdId = div;
                $('#bhvSelImg').attr('src', $('.scrollableArea').find('.students-active').find('.imgStudPhoto').attr('src'));
            } else {
                $(div).parent().find('.students2').removeClass('stdSel');
                $(div).addClass('stdSel');
                $('#bhvSelImg').attr('src', $(div).find('.imgStudPhoto').attr('src'));

                if (opt == 1) {
                    //currentStud = div;
                    selStdId = div.id;
                    $('#newBehStdList').fadeToggle();
                }
                else {
                    var str = div.id;
                    var sstr = str.substring(5);
                    selStdId = sstr;

                    $('#newBehStdList').find('.students2').removeClass('stdSel');
                    $('#' + selStdId).addClass('stdSel');
                }
            }
            if (opt == 0) {
                $('#newBehFrame').fadeOut();
            }
            newBehav(selStdId);

        }

        function newBehav(id) {

            var idd = document.getElementById('stu' + id);
            if (idd === null) {
                $('#newBehFrame').find('.studBhvfrm').hide();
                var width = null;
                if (isiPad) {
                    document.getElementById("newBehFrame").style.width = "250px";
                    document.getElementById("bhvTitle").style.width = "92px";
                    document.getElementById("bhvTitle").style.fontSize = "20px";
                    document.getElementById("newBehTop").style.width = "252px";
                    document.getElementById("newBehStdList").style.width = "250px";
                    width = "248px";
                }
                else {
                    width = "314px";
                }
                $('#newBehFrame').empty(); //Function for avoiding the duplication of the div(zero out behaviour button) each and every time when a student is selected
                var varSubDiv = '<iframe class="studBhvfrm" id=stu' + id + ' style="width: ' + width + ';float:left;height:653px;">id is ' + id + ' </iframe>;<div style="text-align:center"><input type="button" style="width:200px; display:none" class="NFButton" Value="End Of Day Zero Out Behaviors " OnClick="SaveBehaviorForStudentfn(' + id + ')" /></div>'
                $('#newBehFrame').append(varSubDiv);
                if (isiPad) {
                    $('#stu' + id).attr("src", "DatasheetTimerIpad.aspx?stid=" + id);
                } else {
                    $('#stu' + id).attr("src", "dataSheetTimer.aspx?stid=" + id);
                }
            } else {
                $('#newBehFrame').find('.studBhvfrm').hide();
                $('#stu' + id).show();
            }

        }

        function getSelStudBehav() {
            var sid = $('#studentContainer').find('.students-active').attr('id');
            var sstr = sid.substring(5);
            $('#newBehStdList').find('.students2').removeClass('stdSel');
            $('#' + sstr).addClass('stdSel');
            $('#bhvSelImg').attr('src', $(sstr).find('.imgStudPhoto').attr('src'));
            selStudent2(sstr, 3);
        }
        
        function sucesslbl(result) {

            document.getElementById('lblLoginName').innerHTML=result;

        }
        function sucess(result) {
            //if ($('#divStatus').length > 0) {
            //    $('#stMenu').remove();
            //}
            //var status = result;
            //$('#divStatus').append(status);


            //document.getElementById('divStatus').innerHTML = status;

        }
        function OnSuccessStdAssign(result) {

            var activeMenu = $(document).find('#menu_active').find('a').attr('id');


            if ($(document).find('#menu_active').length > 0) {

                PageMethods.FillSubMenu(activeMenu, OnSuccess, OnFailure);
                resetScroll();
            }


            ////
            /// Function to show tool tip for Status on Dash bord
            ////
            /////



        }


        function selMenuForDatasheet(Menu) {


            document.getElementById('dash_area').style.display = 'none';
            hideAllPopUps();

            var divC = document.getElementById('divContentPages');
            for (var i = 0; i < divC.childNodes.length; i++) {
                document.getElementById(divC.childNodes[i].id).style.display = "none";
            }

            if (currentStud != null) {

                PageMethods.FillSubMenu(Menu, OnSuccessDatasheet, OnFailure);
                resetScroll();
                PageMethods.setMenuSess(Menu);

            }

        }


        function datasheetFuntion() {
            $("#DATASHEETS").trigger('click');
            $('.Schedule_View').css('color', 'grey');
            $('.List_View').css('color', 'blue');
            //var menu = $("#DATASHEETS");

            //$("div#makeMeScrollable").smoothDivScroll("moveToElement", "first");

            //document.getElementById('dash_area').style.display = 'none';
            //hideAllPopUps();
            //var iepedit = document.getElementById("IEPEdit");
            //iepedit.style.visibility = "hidden";



            //currentMenu = "datasheet_allLesson";
            //var Menu = $(menu).attr('id'); //menu.textContent;
            //var divC = document.getElementById('divContentPages');
            //for (var i = 0; i < divC.childNodes.length; i++) {
            //    document.getElementById(divC.childNodes[i].id).style.display = "none";
            //}
            //// document.getElementById('tdTitle').innerHTML = "Welcome Dashboard Area";


            //if (currentStud != null) {
            //    //var frame = document.getElementById('ifContent');
            //    //frame.src = 'Dashboard.aspx';
            //    if (Menu.trim() == "DATASHEETS") {
            //        PageMethods.FillSubMenu(Menu, OnSuccessDatasheet, OnFailure);
            //    }
            //    else {
            //        PageMethods.FillSubMenu(Menu, OnSuccess, OnFailure);
            //    }
            //    PageMethods.setMenuSess(Menu);
            //}
        }



        function selMenu(menu) {


            //  $("div#makeMeScrollable").smoothDivScroll("moveToElement", "first");

            document.getElementById('dash_area').style.display = 'none';
            hideAllPopUps();
            var iepedit = document.getElementById("IEPEdit");
            iepedit.style.visibility = "hidden";
            currentMenu = document.getElementById("menu_active");
            if (currentMenu != null)
                currentMenu.removeAttribute("id");
            z = document.createAttribute('id');
            z.value = 'menu_active';
            menu.parentNode.setAttributeNode(z);
            currentMenu = menu;
            var Menu = $(menu).attr('id'); //menu.textContent;
            var divC = document.getElementById('divContentPages');
            for (var i = 0; i < divC.childNodes.length; i++) {
                document.getElementById(divC.childNodes[i].id).style.display = "none";
            }
            // document.getElementById('tdTitle').innerHTML = "Welcome Dashboard Area";


            if (currentStud != null) {
                //var frame = document.getElementById('ifContent');
                //frame.src = 'Dashboard.aspx';
                if (Menu.trim() == "DATASHEETS") {
                    $('.Schedule_View').css('color', 'grey');
                    $('.List_View').css('color', 'blue');
                    $('#tdTitle').html('');
                    PageMethods.FillSubMenu(Menu, OnSuccessDatasheet, OnFailure);
                    resetScroll();
                }
                else {
                    PageMethods.FillSubMenu(Menu, OnSuccess, OnFailure);
                    resetScroll();
                    $('#tdTitle').html('');

                }
                PageMethods.setMenuSess(Menu);
            }
            if (Menu.trim() == "IEPS" || Menu.trim() == "COVERSHEETS" || Menu.trim() == "EDIT" || Menu.trim() == "GRAPHS") {
                //[2019-02-25] disable DashBoard -Start
                $('.fillLessons,.fillLessons1,.fillLessons2').hide();
                var divC = document.getElementById('divContentPages');
                for (var i = 0; i < divC.childNodes.length; i++) {
                    document.getElementById(divC.childNodes[i].id).style.display = "none";
                }
                $('#divContentPages').find('#divTFGraph').remove();//End
                //getDashBoardReport();
            }
            var ctrl = document.getElementById("<%= lblLoginName.ClientID %>").id
            PageMethods.Setllabelname(ctrl,sucesslbl);
        }

        function OnSuccessDatasheet(response) {
            $("#ulsubmenu").empty();



            if (response == null) {

                var flag = confirm("Student not Checked In, Do you want the Student to be Checked In ?");
                if (flag) {
                    PageMethods.setCheckInn(document.getElementById('hidCureentStudent').value, OnSuccessCheckIn);

                    $('.fillLessons1').hide();
                    $('.fillLessons2').hide();
                    $('.fillLessons').hide();
                    PopupLessonPlans1('DatasheetTab');
                }

                //alert("Student not Checked Inn");
            }
            else {

                PopupLessonPlans1('DatasheetTab');
                if (response.length <= 0) {

                    selSubmenuforBehavior();
                }
                else {
                    for (var i = 0; i < response.length; i++) {
                        if (response[i].Url == "Graph" || response[i].Url == "Datasheet" || response[i].Url == "Lesson") {
                            $("#ulsubmenu").append("<li class='alpha'><a id=" + response[i].Url + " ><span><span>" + response[i].Submenu +
                                                        "</span></span> </a></li>");
                        }
                        else {
                            $("#ulsubmenu").append("<li class='alpha'><a id=" + response[i].Url + " onclick='selSubmenu(this," + response[i].ID + "," + response[i].StudID + ");'><span><span>" + response[i].Submenu +
                                                        "</span></span> </a></li>");
                        }
                    }
                }
            }

        }
        function OnSuccessCheckIn(result) {
            var dtsht = document.getElementById('DATASHEETS');
            if (dtsht != null) {
                selMenu(dtsht);
            }
        }
        function showtip(e, message) {
            var x = 0;
            var y = 0;
            var m;
            var h;
            if (!e)
                var e = window.event;
            if (e.pageX || e.pageY) { x = e.pageX; y = e.pageY; }
            else if (e.clientX || e.clientY)
            { x = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft; y = e.clientY + document.body.scrollTop + document.documentElement.scrollTop; }
            m = document.getElementById('myTooltip');
            if ((y > 10) && (y < 450))
            { m.style.top = y - 4 + "px"; }
            else { m.style.top = y + 4 + "px"; }
            var messageHeigth = (message.length / 20) * 10 + 25;
            if ((e.clientY + messageHeigth) > 510)
            { m.style.top = y - messageHeigth + "px"; }
            if (x < 850) { m.style.left = x + 20 + "px"; }
            else { m.style.left = x - 170 + "px"; }
            m.innerHTML = message; m.style.display = "block"; m.style.zIndex = 203;
        }

        ////
        /// Function to hide tool tip for Status on Dash bord
        ////
        /////
        function hidetip() {
            var m;
            m = document.getElementById('myTooltip');
            m.style.display = "none";
        }


        function OnSuccess(response) {
            $("#ulsubmenu").empty();

            if (response != null) {

                for (var i = 0; i < response.length; i++) {
                    if (response[i].Url == "Graph" || response[i].Url == "Datasheet" || response[i].Url == "Lesson") {
                        $("#ulsubmenu").append("<li class='alpha'><a id=" + response[i].Url + " ><span><span>" + response[i].Submenu +
                                                    "</span></span> </a></li>");

                    }
                    else {
                        $("#ulsubmenu").append("<li class='alpha'><a id=" + response[i].Url + " onclick='selSubmenu(this," + response[i].ID + "," + response[i].StudID + ");'><span><span>" + response[i].Submenu +
                                                    "</span></span> </a></li>");

                    }


                    if (response[i].Url == "Lesson Plans") {

                        $('#LessonPlanTab').trigger('click');
                    }
                    if (response[i].Url == "Graph") {
                        //Removed Default selection to maintenance [2019-03-08]
                        $('#GraphDashBoard').trigger('click');// New Graph landing Page added on 12-08-2020 Dev 1
                        //$('#Graph').parent().attr('id', 'submenu_active');
                    }



                }



                if (response.length == 1) {

                    $('#ulsubmenu').find('li.alpha').find('a').trigger('click');
                }

            }

        }
        function OnFailure(error) {
            if (error) {
                alert(error._message);
            }
        }
        function ShowManual() {

            $('#overlay1').fadeIn('slow', function () {
                $('#dialog1').fadeIn('slow');
            });
            $('#close_x1').click(function () {
                $('#dialog1').fadeOut('slow');
                $('#overlay1').fadeOut('slow');

            });
        }
        function ShowReminder() {
            $('.alertsPopUp').slideUp('fast');
            $('.checkPopUp').slideToggle('slow');
            $('.box').slideUp('fast');
        }
        function AletsRem1() {

            $('.alertsPopUp').slideToggle('slow');
            $('.checkPopUp').slideUp('fast');
            $('.box').slideUp('fast');
        }


        function AletsRem() {

            $('.alertsPopUp').slideToggle('slow');
            $('.checkPopUp').slideUp('fast');
            $('.box').slideUp('fast');
			removeColor();	
        }
        function removeColor() {	
            $("#alertBtn").css({	
                "border-radius": "", "height": "",	
                "background-color": "", "margin": "",	
                "display": "", "align-items": ""	
            });	
        }
        function ShowIOA() {
            $('.overlay1').slideToggle('slow');
        }
        function hideAllPopUps() {
            $('.alertsPopUp').slideUp('fast');
            $('.checkPopUp').slideUp('fast');
            $('.box').slideUp('fast');
        }

        function selSubmenuforBehavior() {
            var ID = 0;
            PageMethods.setIDSess(ID, MyMethod_Result);
            var submenus = document.getElementsByClassName("alpha");
            var Count = parseInt(document.getElementById('hidOpenWindows').value) + 1;
            document.getElementById('hidOpenWindows').value = Count;
            var isiPad = navigator.userAgent.match(/iPad/i);
            if (isiPad != null) {
                var submenu = "DatasheetTimerIpad.aspx";
            }
            else {
                var submenu = "dataSheetTimer.aspx";
            }
            var divC = document.getElementById('divContentPages');
            var studVal = currentStud.id;
            var strVals = studVal.split('-');
            var Id = strVals[1];

            x = document.createAttribute('src');
            x.value = submenu + "?pageid=" + ID + "&studid=" + Id;


            var divname = "";
            var dname = "";
            var iname = "";
            var menucontent = "";


            dname = "divBehScore" + Id + "";
            iname = "divIIBehScore" + Id + "" + Count + "";
            divname = "divBehScore" + Id;
            menucontent = "Behavior Scoring";





            var str = divC.innerHTML;
            for (var i = 0; i < divC.childNodes.length; i++) {
                document.getElementById(divC.childNodes[i].id).style.display = "none";
            }

            if (str != "") {

                var n = str.indexOf(dname);
                if (n >= 0) {
                    document.getElementById(dname).style.display = "block";
                }
                else {
                    addFrame(dname, iname, x.value, Id);
                    $("#ulStudents").append("<div id='" + 'Stud' + dname + "' title='" + menucontent + "' style='cursor:pointer;' onclick='selSidemenu(" + dname + "," + Id + "," + ID + ",0);'><li class='sublist' style='background: url(../Administration/images/BEHAVIOR.PNG) no-repeat left;' id='" + 'li' + dname + "'><a href='#'>" + menucontent + "</a></li></div></div>");
                    document.getElementById(dname).style.display = "block";

                }


            }
            else {
                var studnameactive = "";
                addFrame(dname, iname, x.value, Id);
                var studdata = currentStud.textContent.split('..');
                if (currentStud.textContent.indexOf("..") != -1)
                    studnameactive = studdata[1];
                else
                    studnameactive = currentStud.textContent;
                $("#ulStudents").append("<div id=divStud" + Id + " ><li><a href='#'>" + studnameactive + "</a></li><div id='" + 'Stud' + dname + "' title='" + menucontent + "' style='cursor:pointer;' onclick='selSidemenu(" + dname + "," + Id + "," + ID + ",0);'><li class='sublist'  style='background: url(../Administration/images/BEHAVIOR.PNG) no-repeat left;' id='" + 'li' + dname + "'><a href='#'>" + menucontent + "</a></li></div></div>");

            }

            document.getElementById('tdTitle').innerHTML = "Behavior Scoring";

        }
        function selSubmenu1(submenu, ID, studentId) {


            //document.getElementById('dash_area').style.display = 'none';
            var MenuIds = ID;
            var MenuName = "";
            if (currentMenu != null) {
                MenuName = currentMenu.textContent.toString().trim();
                MenuName = MenuName.replace(/\s/g, '');
            }
            else {
                MenuName = "DATASHEETS";
            }

            var classname = "";
            if (MenuName == "STUDENTINFO")
                classname = "style='background: url(../Administration/images/STUDENTINFO.PNG) no-repeat left;'";
            if (MenuName == 'ASSESSMENTS')
                classname = "style='background: url(../Administration/images/Assessments.PNG) no-repeat left;'";
            if (MenuName == 'IEPS')
                classname = "style='background: url(../Administration/images/IEPS1.PNG) no-repeat left;'";
            if (MenuName == 'BSP')
                classname = "style='background: url(../Administration/images/IEPS1.PNG) no-repeat left;'";
            if (MenuName == 'LESSONPLANS')
                classname = "style='background: url(../Administration/images/LESSONPLANS.PNG) no-repeat left;'";
            if (MenuName == 'DATASHEETS')
                classname = "style='background: url(../Administration/images/DATASHEETS.PNG) no-repeat left;'";
            if (MenuName == 'BEHAVIOR')
                classname = "style='background: url(../Administration/images/BEHAVIOR.PNG) no-repeat left;'";
            if (MenuName == 'GRAPHS')
                classname = "style='background: url(../Administration/images/GRAPHS.PNG) no-repeat left;'";
            if (MenuName == 'COVERSHEETS')
                classname = "style='background: url(../Administration/images/COVERSHEETS.PNG) no-repeat left;'";
            if (MenuName == 'EDIT')
                classname = "style='background: url(../Administration/images/EDIT.PNG) no-repeat left;'";



            hideAllPopUps();
            // document.getElementById('divStatus').style.display = "none";
            PageMethods.setIDSess(ID, MyMethod_Result);
            var submenus = document.getElementsByClassName("alpha");


            for (var i = 0; i < submenus.length; i++) {
                submenus[i].removeAttribute("id");

            }

            for (var i = 0; i < submenus.length; i++) {

                submenus[i].removeAttribute("id");

                var Count = parseInt(document.getElementById('hidOpenWindows').value) + 1;
                document.getElementById('hidOpenWindows').value = Count;



                var divC = document.getElementById('divContentPages');
                var studVal = currentStud.id;
                var strVals = studVal.split('-');
                var Id = strVals[1];


                z = document.createAttribute('id');
                z.value = '';

                if (submenu.parentNode == submenus[i]) {

                    z = document.createAttribute('id');
                    z.value = 'submenu_active';
                    submenus[i].setAttributeNode(z);

                    var submenuList = submenu.id.split('/');

                    x = document.createAttribute('src');
                    x.value = submenuList[0] + "?pageid=" + ID + "&studid=" + Id;

                    var Ids = "?pageid=" + ID + "&studid=" + Id;
                    var divname = "";
                    var dname = "";
                    var iname = "";

                    if (x.value == ("Demography.aspx" + Ids).toString()) {

                        dname = "divS" + Id + "" + MenuIds + ""
                        iname = "divIS" + Id + "" + MenuIds + "";
                        divname = "divS" + Id;

                    }
                    else if (x.value == ("ReviewAssessmnt.aspx" + Ids).toString()) {


                        dname = "divR" + Id + "" + MenuIds + ""
                        iname = "divIR" + Id + "" + MenuIds + "";
                        divname = "divR" + Id;
                    }
                    else if (x.value == ("FormAssess.aspx" + Ids).toString()) {
                        dname = "divF" + Id + "" + MenuIds + ""
                        iname = "divFR" + Id + "" + MenuIds + "";
                        divname = "divF" + Id;
                    }
                    else if (x.value == ("GoalAssess.aspx" + Ids).toString()) {
                        dname = "divG" + Id + "" + MenuIds + ""
                        iname = "divIG" + Id + "" + MenuIds + "";
                        divname = "divG" + Id;
                    }
                    else if (x.value == ("LessonPlanAttributes.aspx" + Ids).toString()) {

                        dname = "divT" + Id + "" + MenuIds + ""
                        iname = "divIT" + Id + "" + MenuIds + "";
                        divname = "divT" + Id;
                    }
                    else if (x.value == ("IEPView.aspx" + Ids).toString()) {
                        document.getElementById('IEPEdit').style.display = "block";
                        dname = "divI" + Id + "" + MenuIds + ""
                        iname = "divII" + Id + "" + MenuIds + "";
                        divname = "divI" + Id;
                    }
                    else if (x.value == ("Datasheet.aspx" + Ids).toString()) {

                        dname = "divTF" + Id + "" + MenuIds + ""
                        iname = "divITF" + Id + "" + MenuIds + "";
                        divname = "divTF" + Id;
                    }
                    else if (x.value == ("BehaviourCalc.aspx" + Ids).toString()) {
                        dname = "divBPBC" + Id + "" + MenuIds + ""
                        iname = "divIBPBC" + Id + "" + MenuIds + "";
                        divname = "divBPBC" + Id;
                    }
                    else if (x.value == ("ManBehaviorSet.aspx" + Ids).toString()) {
                        dname = "divBPBM" + Id + "" + MenuIds + ""
                        iname = "divIBPBM" + Id + "" + MenuIds + "";
                        divname = "divBPBM" + Id;
                    }
                    else if (x.value == ("Dashboard.aspx" + Ids).toString()) {
                        dname = "divBPB" + Id + "" + MenuIds + ""
                        iname = "divIBPB" + Id + "" + MenuIds + "";
                        divname = "divBPB" + Id;
                    }
                    else if (x.value == ("EventTracker.aspx" + Ids).toString()) {
                        dname = "divET" + Id + "" + MenuIds + ""
                        iname = "divIET" + Id + "" + MenuIds + "";
                        divname = "divET" + Id;
                    }
                    else if (x.value == ("ProgressReport.aspx" + Ids).toString()) {
                        dname = "divPRPT" + Id + "" + MenuIds + ""
                        iname = "divIPRPT" + Id + "" + MenuIds + "";
                        divname = "divPRPT" + Id;
                    }
                    else if (x.value == ("LessonReportsWithPaging.aspx" + Ids).toString()) {
                        dname = "divGR" + Id + "" + MenuIds + ""
                        iname = "divIGR" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("IEPView.aspx" + Ids).toString()) {
                        dname = "divIEP" + Id + "" + MenuIds + ""
                        iname = "divIIEP" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("PhaseLines.aspx" + Ids).toString()) {
                        dname = "divPL" + Id + "" + MenuIds + ""
                        iname = "divIPL" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("BiweeklyBehaviorGraph.aspx" + Ids).toString()) {
                        dname = "divMG" + Id + "" + MenuIds + ""
                        iname = "divIMG" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("VTLessonSelectionPage.aspx" + Ids).toString()) {
                        dname = "divVT" + Id + "" + MenuIds + ""
                        iname = "divVT" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("Calender.aspx" + Ids).toString()) {
                        dname = "divCAL" + Id + "" + MenuIds + ""
                        iname = "divICAL" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("StudentLessonPlan.aspx" + Ids).toString()) {
                        dname = "divSLP" + Id + "" + MenuIds + ""
                        iname = "divISLP" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("ACSheet.aspx" + Ids).toString()) {
                        dname = "divACG" + Id + "" + MenuIds + ""
                        iname = "divIACG" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("BehaviourMaintanance.aspx" + Ids).toString()) {
                        dname = "divBHRM" + Id + "" + MenuIds + ""
                        iname = "divIBHRM" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("ClinicalSheetNew.aspx" + Ids).toString()) {
                        dname = "divCLN" + Id + "" + MenuIds + ""
                        iname = "divICLN" + Id + "" + MenuIds + "";
                    }

                    else if (x.value == ("GrandRoundCvrsht.aspx" + Ids).toString()) {
                        dname = "divGRND" + Id + "" + MenuIds + ""
                        iname = "divIGRND" + Id + "" + MenuIds + "";
                    }

                    else if (x.value == ("CreateCustomIEP.aspx" + Ids).toString()) {
                        dname = "divEIEP" + Id + "" + MenuIds + ""
                        iname = "divIEIEP" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("AsmntReview.aspx" + Ids).toString()) {
                        dname = "divCGLP" + Id + "" + MenuIds + ""
                        iname = "divICGLP" + Id + "" + MenuIds + "";

                    }
                    else if (x.value == ("CustomizeTemplateEditor.aspx" + Ids).toString()) {
                        dname = "divCTER" + Id + "" + MenuIds + ""
                        iname = "divICTER" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("CreateCustomIEP.aspx" + Ids).toString()) {
                        dname = "divCCIEP" + Id + "" + MenuIds + ""
                        iname = "divICCIEP" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("AcademicSessionReport.aspx" + Ids).toString()) {
                        dname = "divBSRG" + Id + "" + MenuIds + ""
                        iname = "divIBSRG" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("DSTempHistory.aspx" + Ids).toString()) {
                        dname = "divDSTEMP" + Id + "" + MenuIds + ""
                        iname = "divIDSTEMP" + Id + "" + MenuIds + "";
                    }
                    //else if (x.value == ("SharePointRedirect.aspx" + Ids).toString()) {
                    //    dname = "divSPR" + Id + "" + MenuIds + ""
                    //    iname = "divISPR" + Id + "" + MenuIds + "";
                    //}
                    //else if (x.value == ("SharePointBehavior.aspx" + Ids).toString()) {
                    //    dname = "divSPB" + Id + "" + MenuIds + ""
                    //    iname = "divISPB" + Id + "" + MenuIds + "";
                    //}
                    else if (x.value == ("ExcelViewReport.aspx" + Ids).toString()) {
                        dname = "divEVR" + Id + "" + MenuIds + ""
                        iname = "divIEVR" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("ProgressSummaryReport.aspx" + Ids).toString()) {
                        dname = "divPSR" + Id + "" + MenuIds + ""
                        iname = "divIPSR" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("ProgressSummaryReportClinical.aspx" + Ids).toString()) {
                        dname = "divPSRC" + Id + "" + MenuIds + ""
                        iname = "divIPSRC" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("TextBased.aspx" + Ids).toString()) {
                        dname = "divTB" + Id + "" + MenuIds + ""
                        iname = "divITB" + Id + "" + MenuIds + "";
                    }
                    else if (x.value == ("ExportLessons.aspx" + Ids).toString()) {
                        dname = "divEXP" + Id + "" + MenuIds + ""
                        iname = "divIEXP" + Id + "" + MenuIds + "";
                    }                 
                    else {
                        dname = "divBHRDET" + Id + "" + MenuIds + ""
                        iname = "divIBHRDET" + Id + "" + MenuIds + "";
                    }

                    setName(dname);

                    var searchEles = document.getElementById("divContentPages").children;

                    if (searchEles.length > 0) {

                        for (var i = 0; i < divC.childNodes.length; i++) {
                            document.getElementById(divC.childNodes[i].id).style.display = "none";
                        }

                    }


                    var stud = $("#divStud" + Id + "");

                    var menucontent = submenu.textContent;

                    var divs = currentStud.id;
                    var strVals = divs.split('-');
                    var c = strVals[1];


                    var str = divC.innerHTML;
                    for (var i = 0; i < divC.childNodes.length; i++) {
                        document.getElementById(divC.childNodes[i].id).style.display = "none";
                    }


                    // doubtful code

                    if ($('#divAll' + Id).length != 0) {

                        var n = str.toString().trim().indexOf(dname.toString().trim());
                        if (n >= 0) {
                            document.getElementById(dname).style.display = "block";
                            return false;
                        }
                        else {

                            if (submenuList.length == 1) {
                                if (MenuName == 'DATASHEETS') {

                                    if (dname.indexOf('divCAL') < 0) {
                                        $("#divStud" + Id + "").append("<div id='" + 'Stud' + dname + "' title='" + menucontent + "' style='cursor:pointer;' onclick='selSidemenu(" + dname + "," + Id + "," + ID + ",0);'><li class='sublist' id='" + 'li' + dname + "' " + classname + "><a href='#' >" + menucontent + "</a></li></div>");
                                    }
                                }
                            }
                            addFrame(dname, iname, x.value, Id);
                        }
                    }

                    else {
                        var studnameactive = "";
                        var studdata = currentStud.textContent.split('..');
                        if (currentStud.textContent.indexOf("..") != -1) {
                            studnameactive = studdata[1];
                        }
                        else {
                            var studVal = currentStud.id;
                            var strVals = studVal.split('-');
                            var Id = strVals[1];
                            var b = "HoverDiv" + Id;
                            studnameactive = document.getElementById(b).innerHTML;
                        }
                        if (submenuList.length == 1) {

                            if (MenuName == 'DATASHEETS') {
                                if (dname.indexOf('divCAL') < 0) {
                                    $("#ulStudents").append("<div id=divStud" + Id + " ><li><a href='#'>" + studnameactive + "</a></li><div id='" + 'Stud' + dname + "' title='" + menucontent + "' style='cursor:pointer;' onclick='selSidemenu(" + dname + "," + c + "," + ID + ",0);'><li class='sublist' id='" + 'li' + dname + "' " + classname + "><a href='#'>" + menucontent + "</a></li></div></div>");
                                }
                            }


                            if (submenuList != 'Calender.aspx') {
                                $("#ulStudents").append("<div id=divAll" + Id + " style='display:none' ></div>");
                            }

                        }
                        addFrame(dname, iname, x.value, Id);

                        //getNotifier();
                    }

                    setName(dname);
                }
                else {

                    submenus[i].removeAttribute("id");
                    //getNotifier();
                }
            }

        }

        function selSubmenu(submenu, ID, studentId) {
            $('.alpha').removeAttr('id');
            $('.fillLessons').hide();
            $('.fillLessons1').hide();
            $('.fillLessons2').hide();

            if ($(submenu).hasClass('LessonPlanList')) {
                $('#Datasheet').parent().attr('id', 'submenu_active');
            }
            else {

                $(submenu).parent().attr('id', 'submenu_active');
            }
            if (submenu.id == "ChainGraph") {
                $('#tdTitle').html('');
                PopupLessonPlans('ChainGraphTab');
            }
            else if (submenu.id != "Lesson") {
                var MenuIds = ID;
                var MenuName = "";

                if (currentMenu != null) {
                    if (currentMenu != 'datasheet_allLesson') {
                        MenuName = currentMenu.textContent.toString().trim();
                        MenuName = MenuName.replace(/\s/g, '');
                    }
                    else {
                        MenuName = "";
                    }
                }
                else {
                    MenuName = "DATASHEETS";
                }


                var menucontent = submenu.textContent;



                var submenuList = submenu.id.split('/');


                var studVal = currentStud.id;
                var strVals = studVal.split('-');
                var Id = strVals[1];
                if (strVals.length == 1) {
                    Id = studVal;
                }
                else {
                    Id = strVals[1];
                }
                //  var c = strVals[i];

                var classname = "";

                if (MenuName == 'DATASHEETS')
                    classname = "style='background: url(../Administration/images/DATASHEETS.PNG) no-repeat left;'";


                x = document.createAttribute('src');
                x.value = submenuList[0] + "?pageid=" + ID + "&studid=" + Id;


                var Ids = "?pageid=" + ID + "&studid=" + Id;
                var divname = "";
                var dname = "";
                var iname = "";



                if (x.value == ("Demography.aspx" + Ids).toString()) {

                    dname = "divS" + Id + "" + MenuIds + ""
                    iname = "divIS" + Id + "" + MenuIds + "";
                    divname = "divS" + Id;

                }
                else if (x.value == ("ReviewAssessmnt.aspx" + Ids).toString()) {

                    dname = "divR" + Id + "" + MenuIds + ""
                    iname = "divIR" + Id + "" + MenuIds + "";
                    divname = "divR" + Id;
                }
                else if (x.value == ("FormAssess.aspx" + Ids).toString()) {
                    closeMessage();
                    dname = "divF" + Id + "" + MenuIds + ""
                    iname = "divFR" + Id + "" + MenuIds + "";
                    divname = "divF" + Id;
                }
                else if (x.value == ("GoalAssess.aspx" + Ids).toString()) {
                    dname = "divG" + Id + "" + MenuIds + ""
                    iname = "divIG" + Id + "" + MenuIds + "";
                    divname = "divG" + Id;
                }
                else if (x.value == ("LessonPlanAttributes.aspx" + Ids).toString()) {
                   
                    dname = "divT" + Id + "" + MenuIds + ""
                    iname = "divIT" + Id + "" + MenuIds + "";
                    divname = "divT" + Id;
                }
                else if (x.value == ("IEPView.aspx" + Ids).toString()) {
                    document.getElementById('IEPEdit').style.display = "block";
                    dname = "divI" + Id + "" + MenuIds + ""
                    iname = "divII" + Id + "" + MenuIds + "";
                    divname = "divI" + Id;
                }
                else if (x.value == ("Datasheet.aspx" + Ids).toString()) {

                    dname = "divTF" + Id + "";
                    iname = "divITF" + Id + "" + MenuIds + "";
                    divname = "divTF" + Id;
                }
                else if (x.value == ("BehaviourCalc.aspx" + Ids).toString()) {
                    dname = "divBPBC" + Id + "" + MenuIds + ""
                    iname = "divIBPBC" + Id + "" + MenuIds + "";
                    divname = "divBPBC" + Id;
                }
                else if (x.value == ("ManBehaviorSet.aspx" + Ids).toString()) {
                    dname = "divBPBM" + Id + "" + MenuIds + ""
                    iname = "divIBPBM" + Id + "" + MenuIds + "";
                    divname = "divBPBM" + Id;
                }

                else if (x.value == ("Dashboard.aspx" + Ids).toString()) {
                    dname = "divBPB" + Id + "" + MenuIds + ""
                    iname = "divIBPB" + Id + "" + MenuIds + "";
                    divname = "divBPB" + Id;
                }
                else if (x.value == ("EventTracker.aspx" + Ids).toString()) {
                    dname = "divET" + Id + "" + MenuIds + ""
                    iname = "divIET" + Id + "" + MenuIds + "";
                    divname = "divET" + Id;
                }
                else if (x.value == ("LessonReportsWithPaging.aspx" + Ids).toString()) {
                    dname = "divGR" + Id + "" + MenuIds + ""
                    iname = "divIGR" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("IEPView.aspx" + Ids).toString()) {
                    dname = "divIEP" + Id + "" + MenuIds + ""
                    iname = "divIIEP" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("PAIEPView.aspx" + Ids).toString()) {
                    dname = "divIEPPE" + Id + "" + MenuIds + ""
                    iname = "divIIEPPE" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("PhaseLines.aspx" + Ids).toString()) {
                    dname = "divPL" + Id + "" + MenuIds + ""
                    iname = "divIPL" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("BiweeklyBehaviorGraph.aspx" + Ids).toString()) {
                    dname = "divMG" + Id + "" + MenuIds + ""
                    iname = "divIMG" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("VTLessonSelectionPage.aspx" + Ids).toString()) {
                    dname = "divVT" + Id + "" + MenuIds + ""
                    iname = "divVT" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("Calender.aspx" + Ids).toString()) {
                    dname = "divCAL" + Id + "" + MenuIds + ""
                    iname = "divICAL" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("StudentLessonPlan.aspx" + Ids).toString()) {
                    dname = "divSLP" + Id + "" + MenuIds + ""
                    iname = "divISLP" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("ACSheet.aspx" + Ids).toString()) {
                    dname = "divACG" + Id + "" + MenuIds + ""
                    iname = "divIACG" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("BehaviourMaintanance.aspx" + Ids).toString()) {
                    closeMessage();
                    dname = "divBHRM" + Id + "" + MenuIds + ""
                    iname = "divIBHRM" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("ClinicalSheetNew.aspx" + Ids).toString()) {
                    dname = "divCLN" + Id + "" + MenuIds + ""
                    iname = "divICLN" + Id + "" + MenuIds + "";
                }

                else if (x.value == ("GrandRoundCvrsht.aspx" + Ids).toString()) {
                    dname = "divGRND" + Id + "" + MenuIds + ""
                    iname = "divIGRND" + Id + "" + MenuIds + "";
                }

                else if (x.value == ("CreateCustomIEP.aspx" + Ids).toString()) {
                    closeMessage();
                    dname = "divEIEP" + Id + "" + MenuIds + ""
                    iname = "divIEIEP" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("AsmntReview.aspx" + Ids).toString()) {

                    showMessage();//Loading......
                   
                    dname = "divCGLP" + Id + "" + MenuIds + "";
                    iname = "divICGLP" + Id + "" + MenuIds + "";

                }
                else if (x.value == ("CustomizeTemplateEditor.aspx" + Ids).toString()) {
                    closeMessage();
                    dname = "divCTER" + Id + "" + MenuIds + ""
                    iname = "divICTER" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("CreateCustomIEP.aspx" + Ids).toString()) {
                    dname = "divCCIEP" + Id + "" + MenuIds + ""
                    iname = "divICCIEP" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("AcademicSessionReport.aspx" + Ids).toString()) {
                    dname = "divBSRG" + Id + "" + MenuIds + ""
                    iname = "divIBSRG" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("DSTempHistory.aspx" + Ids).toString()) {
                    dname = "divDSTEMP" + Id + "" + MenuIds + ""
                    iname = "divIDSTEMP" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("CreateCustomIepPE.aspx" + Ids).toString()) {
                    dname = "divCCIEPPE" + Id + "" + MenuIds + ""
                    iname = "divICCIEPPE" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("SharePointRedirect.aspx" + Ids).toString()) {
                    dname = "divSPR" + Id + "" + MenuIds + ""
                    iname = "divISPR" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("SharePointBehavior.aspx" + Ids).toString()) {
                    dname = "divSPB" + Id + "" + MenuIds + ""
                    iname = "divISPB" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("ExcelViewReport.aspx" + Ids).toString()) {
                    dname = "divEVR" + Id + "" + MenuIds + ""
                    iname = "divIEVR" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("ProgressSummaryReport.aspx" + Ids).toString()) {
                    dname = "divPSR" + Id + "" + MenuIds + ""
                    iname = "divIPSR" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("ProgressSummaryReportClinical.aspx" + Ids).toString()) {
                    dname = "divPSR" + Id + "" + MenuIds + ""
                    iname = "divIPSR" + Id + "" + MenuIds + "";
                }
                else if (x.value == ("TextBased.aspx" + Ids).toString()) {
                    dname = "divTB" + Id + "" + MenuIds + ""
                    iname = "divITB" + Id + "" + MenuIds + "";
                }
                
                else if (x.value == ("ProgressReport.aspx" + Ids).toString()) {
                    dname = "divPRPT" + Id + "" + MenuIds + ""
                    iname = "divIPRPT" + Id + "" + MenuIds + "";
                    divname = "divPRPT" + Id;
                }
                else if (x.value == ("ChainedBarGraphReport.aspx" + Ids).toString()) {
                    dname = "divCHGHRT" + Id + "" + MenuIds + ""
                    iname = "divICHGHRT" + Id + "" + MenuIds + "";
                    divname = "divCHGHRT" + Id + "" + MenuIds + ""
                }
                else if (x.value == ("DatasheetBSPForms.aspx" + Ids).toString()) {
                    dname = "divBSP" + Id + "" + MenuIds + ""
                    iname = "divIBSP" + Id + "" + MenuIds + "";
                    divname = "divBSP" + Id;
                }
                else if (x.value == ("GraphTileGrid.aspx" + Ids).toString()) {
                    dname = "divGTG" + Id + "" + MenuIds + ""
                    iname = "divIGTG" + Id + "" + MenuIds + "";
                    divname = "divGTG" + Id;
                }
                else if (x.value == ("ExportLessons.aspx" + Ids).toString()) {
                    dname = "divEXP" + Id + "" + MenuIds + ""
                    iname = "divIEXP" + Id + "" + MenuIds + "";
                    divname = "divEXP" + Id;
                }
                else {
                    dname = "divTFBEH" + Id + "" + MenuIds + ""
                    iname = "divTFBEH" + Id + "" + MenuIds + "";
                    divname = "divTFBEH" + Id + "" + MenuIds + ""
                }

                var divC = document.getElementById('divContentPages');
                $(divC).children().hide();

                if (divname != "") {
                    var isExist = $('#divContentPages').find('#' + divname).size();
                    if (isExist > 0) {

                        if (divname.indexOf('TFBEH') > 0) {
                            setBehVisible(Id);
                            return false;
                        }
                        else if (divname.indexOf('TF') > 0) {
                            var newName = "sheetDiv-" + Id + "-" + ID.toString();
                            var isSheet = $('#divContentPages').find('#divTF' + Id).find('#LeftDiv' + Id).find('#' + newName).size();
                            if (isSheet > 0) {

                                setDatasheetVisible(Id, ID);
                                return false;
                            }
                        }
                    }
                }

                var leftDiv = "LeftDiv" + Id;
                var noOfLessons = ID;




                //ACTIVE SESSION-- Adds a row in the active session if its a datasheet. //

                var divC = document.getElementById('divContentPages');
                removeAllDivs();
                $(divC).children().hide();

                if ($('#divAll' + Id).length != 0) {

                    var n = $(divC).find('#' + dname.toString().trim()).length;

                    if (n > 0) {
                        if (submenuList != 'Calender.aspx' && submenuList != 'DSTempHistory.aspx') {
                            if (dname.indexOf('divCAL') < 0) {
                                var divcont = $('<div>' + submenu.innerHTML + '</div>');
                                menucontent = $(divcont).find('.lpName').html();
                                //if ($(divcont).find('.leftDiv').length > 0) {
                                //    menucontent = menucontent + '<div style="float:right;">' + $(divcont).find('.leftDiv').html() + '</div>';
                                //}
                                $("#divStud" + Id + "").append("<div id='" + 'Stud' + dname + noOfLessons + "' title='" + menucontent + "' style='cursor:pointer;' onclick='selSidemenu(\"" + dname + "\"," + Id + "," + ID + "," + noOfLessons + ");'><li class='sublist' id='" + 'li' + dname + "' " + classname + "><a href='#' >" + menucontent + "</a></li></div>");
                            }
                        }

                        addLDtoExisting(dname, iname, x.value, Id, noOfLessons, ID);

                        return false;
                    }
                    else {

                        if (submenuList.length == 1) {
                            if (MenuName == 'DATASHEETS') {
                                if (submenuList != 'Calender.aspx' && submenuList != 'DSTempHistory.aspx') {
                                    if (dname.indexOf('divCAL') < 0) {
                                        $("#divStud" + Id + "").append("<div id='" + 'Stud' + dname + noOfLessons + "' title='" + menucontent + "' style='cursor:pointer;' onclick='selSidemenu(\"" + dname + "\"," + Id + "," + ID + "," + noOfLessons + ");'><li class='sublist' id='" + 'li' + dname + "' " + classname + "><a href='#' >" + menucontent + "</a></li></div>");
                                    }
                                }
                            }
                        }

                    }
                }
                else {
                    var studnameactive = "";
                    var studdata = currentStud.textContent.split('..');
                    if (currentStud.textContent.indexOf("..") != -1) {
                        studnameactive = studdata[1];
                    }
                    else {
                        var studVal = currentStud.id;
                        var strVals = studVal.split('-');
                        var Id = strVals[1];
                        var b = "HoverDiv" + Id;
                        studnameactive = document.getElementById(b).innerHTML;
                    }
                    if (submenuList.length == 1) {
                        if (MenuName == 'DATASHEETS') {
                            if (submenuList != 'Calender.aspx' && submenuList != 'DSTempHistory.aspx') {
                                //$("#ulStudents").append("<div id=divStud" + Id + " ><li><a href='#'>" + studnameactive + "</a></li><div id='" + 'Stud' + dname + noOfLessons + "' title='" + menucontent + "' style='cursor:pointer;' onclick='selSidemenu(\"" + dname + "\"," + Id + "," + ID + "," + noOfLessons + ");'><li class='sublist' id='" + 'li' + dname + "' " + classname + "><a href='#'>" + menucontent + "</a></li></div></div>");
                                $("#ulStudents").append("<div id=divStud" + Id + " ><li><a href='#'>" + studnameactive + "</a></li><div id='" + 'Stud' + dname + noOfLessons + "' title='" + menucontent + "' style='cursor:pointer;' onclick='selSidemenu(\"" + dname + "\"," + Id + "," + ID + "," + noOfLessons + ");'>");

                                $("#ulStudents").append("<div id=divAll" + Id + " style='display:none' ></div>");
                            }
                        }



                    }


                }


                //-- ACTIVE SESSION --//

                // Setting submenu active//
                //      $('#ulsubmenu').find('.alpha').removeAttr('id');
                //      $('#ulsubmenu').find('span').filter(function () { return ($(this).text().indexOf(menucontent.trim()) > -1) }).eq(1).parents('.alpha').attr('id', 'submenu_active');
                //-- setting submenu active --//

                setName(dname); // set the title of the page.
                if (x.value == ("Datasheet.aspx" + Ids).toString()) {
                    addDatasheetFrame(dname, iname, x.value, Id, noOfLessons);

                    //addFrame(dname, iname, x.value, Id);
                    
                }
                else {
                    addFrame(dname, iname, x.value, Id);
                    //addDatasheetFrame(dname, iname, x.value, Id, noOfLessons);
                }


            }
            else {

                $('.fillLessons1').show();
            }
        }
        ///

        function showandHideMenu() {


            if ($('#ulWrapper').css('display') == 'block') {


            }
            else {

            }

        }


        function showMessage() {
            $('#CGLLoadingImage').show();
        }

        function closeMessage() {
            $('#CGLLoadingImage').hide();
        }


        function setMenuHide() {
            $("#slider").remove();

            var newItem = "";
            $('#dashboard-top-panel').append(newItem);
            var newItem1 = "<img id='imgSlider' onclick='toggle()' src='../Administration/images/menuDwn.png' style='left:5%; height: 28px;width:127px;z-index:100; position: absolute; top: 3px;cursor: pointer;'  />";
            $('#slider').append(newItem1);
        }
        function showHidedMenu() {
            $('#topArrow').attr('src', '');
            var top = "";
            if ($('#slider').hasClass('up')) {
                $('#topArrow').attr('src', '../Administration/images/arrowTop.png');
                $('#slider').removeClass('up');
                $('#slider').addClass('down');
                $('#slider').css("top", '59%');

            }
            else {


                $('#topArrow').attr('src', '../Administration/images/arrowDown.png');
                $('#slider').removeClass('down');
                $('#slider').addClass('up');

                top = top - 180;
                $('#slider').css("top", '326%');
            }
        }
        function setName(dname) {

            var top = $("#submenu").offset().top;
            $(".slideImg").remove();

            var title = "";
            document.getElementById('IEPEdit').style.display = "none";

            if (dname.indexOf('divS') >= 0) {
                 <% var sesso = (clsSession)HttpContext.Current.Session["UserSession"];
                    if (sesso.SchoolId == 2)
                    { %>
                title = "Protocol Summary";

            <% }
                    else
                    {%>
                title = "Facesheet";
                <%}%>
            }
            if (dname.indexOf('divI') >= 0) {
                title = "Individualized Education Program ";
                document.getElementById('IEPEdit').style.display = "block";
            }
            if (dname.indexOf('divT') >= 0) {
                title = "Lesson Summary";
            }
            if (dname.indexOf('divR') >= 0) {

                title = "View Assessment";
            }
            if (dname.indexOf('divPRPT') >= 0) {
                title = "Progress Report";
            }
            if (dname.indexOf('divTB') >= 0) {
                title = "Step/Trial Data";
            }
            if (dname.indexOf('divCHGHRT') >= 0) {
                title = "Chained Bar Graph";
            }
            if (dname.indexOf('divF') >= 0) {
                title = "Assessment";
            }
            if (dname.indexOf('divG') >= 0) {
                title = "Goal Assess";
            }
            if (dname.indexOf('divTF') >= 0) {

                title = "Datasheet Editor";
            }
            if (dname.indexOf('divBPB') >= 0) {
                //  title = "Set Reminder and IOA";
                title = "Interval Timers";
            }
            if (dname.indexOf('divGR') >= 0) {
                title = "Academic Report";
            }
            if (dname.indexOf('divET') >= 0) {
                title = "Event Tracker";
            }
            if (dname.indexOf('divBPBM') >= 0) {
                title = "Behavior Manual";
            }
            if (dname.indexOf('divPL') >= 0) {
                title = "Phase Line Graph Report";
            }
            if (dname.indexOf('divMG') >= 0) {
                title = "Clinical Report";
            }
            if (dname.indexOf('divAddAss') >= 0) {
                title = "Manage Assessments";
            }
            if (dname.indexOf('divAssList') >= 0) {
                title = "Assessment List";
            }
            if (dname.indexOf('divIEPList') >= 0) {
                title = "IEP List";
            }
            if (dname.indexOf('divCAL') >= 0) {
                title = "Block Schedule";
            }
            if (dname.indexOf('divSLP') >= 0) {
                title = "Student Lesson Plan";
            }
            if (dname.indexOf('divACG') >= 0) {
                title = "Academic Coversheets";
            }
            if (dname.indexOf('divCLN') >= 0) {
                title = "Clinical Coversheets";
            }
            if (dname.indexOf('divBHRM') >= 0) {
                title = "Behaviors";
            }
            if (dname.indexOf('divTFBEH') >= 0) {
                title = "Behavior Screen";
            }
            if (dname.indexOf('divBSP') >= 0) {
                title = "BSP Forms";
            }
            if (dname.indexOf('divEIEP') >= 0) {
                title = "Create IEP";
            }
            if (dname.indexOf('divCGLP') >= 0) {
                title = "Customize Goals And Lessons";
            }
            if (dname.indexOf('divCTER') >= 0) {
                title = "";
            }
            if (dname.indexOf('divCCIEP') >= 0) {
                title = "Customize IEP";
            }
            if (dname.indexOf('divASMNT') >= 0) {
                title = "Customize Goals And Lessons";
            }
            if (dname.indexOf('divBSRG') >= 0) {
                title = "Session-Based Graph";
            }
            if (dname.indexOf('divDSTEMP') >= 0) {
                title = "Prior Sessions";
            }
            //if (dname.indexOf('divSPR') >= 0) {
            //    title = "Dynamic Academic Reports";
            //}
            //if (dname.indexOf('divSPB') >= 0) {
            //    title = "Dynamic Clinical Reports";
            //}
            if (dname.indexOf('divEVR') >= 0) {
                title = "Excel View Report";
            }
            if (dname.indexOf('divPSR') >= 0) {
                title = "Progress Summary Report";
            }
            if (dname.indexOf('divPSRC') >= 0) {
                title = "Clinical Progress Summary Report";
            }
            if (dname.indexOf('divTB') >= 0) {
                title = "Step/Trial Data";
            }
            if (dname.indexOf('divGRND') >= 0) {
                title = "RTF Coversheet";
            }

            if (dname.indexOf('divGTG') >= 0) {
                title = "Graph Grid";
            }
            if (dname.indexOf('divEXP') >= 0) {
                title = "";
            }

            if (dname != "") {

                document.getElementById('tdTitle').innerHTML = title;
            }
            else {
                //[2019-02-25] disable DashBoard
                //document.getElementById('tdTitle').innerHTML = "Welcome to Dashboard Area";
            }

        }
        function selectaccord(th) {
            var insubmenulists = document.getElementsByClassName('sublist');
            var parent = th.parentNode;
            $(parent).find(insubmenulists).hide();

        }

        function resetOverrideSessionfn(sheetId) {
            $.ajax({
                type: "POST",
                url: "Datasheet.aspx/resetOverrideSession",
                data: "{'sheetId':'" + sheetId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                },
                Error: function () {
                }
            });
        }

        ///function for zero out behaviour
        ///
        function SaveBehaviorForStudentfn(studentId) {

            r = confirm("Warning! This action will zero out all behaviors with no data taken today.");
            if (r == true) {

                $.ajax({
                    type: "POST",
                    url: "Home.aspx/SaveBehaviorForStudent",
                    data: "{'StudentId':'" + studentId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function () {
                        alert("Zero has been added successfully");

                    },
                    Error: function () {
                    }
                });
            }
            else {
                return;
            }

        }

        function removeFrame(name, sheetNo, pageType) {
            var frameId;

            if (pageType == "DS") {
                frameId = name;
                var newName = "sheetDiv-" + name + "-" + sheetNo.toString();
                var isSheet = $('#divContentPages').find('#divTF' + name).find('#LeftDiv' + name).children().size();




                var sheetId;
                $("#divContentPages").find('#divTF' + name).find('#LeftDiv' + name).children().each(function (n, i) {

                    if ($(this).css('display') == 'block') {
                        sheetId = $(this).attr('id');
                        var valNew = sheetId.split('-');
                        sheetId = valNew[2];
                    }

                })
                var newName;
                if (sheetId != null) { newName = "sheetDiv-" + name + "-" + sheetId.toString(); }


                var r = false;
                //if (isSheet == 1) {
                //    r = confirm("Are you sure ?");
                //    if (r == true) {
                //        $('#divContentPages').find('#divTF' + frameId).remove();
                //    }
                //    else {
                //        return;
                //    }
                //}
                //else {
                //    r == true;
                //    $('#divContentPages').find('#divTF' + name).find('#LeftDiv' + name).find('#' + newName).remove();
                //}


                //var isActive = $('#ulStudents').find('#divStud' + frameId).children().size();
                //if (isActive == 2) {
                //    $('#ulStudents').find('#divStud' + frameId).remove();
                //    $('#ulStudents').find('#divAll' + frameId).remove();
                //} else {
                //    $('#ulStudents').find('#divStud' + frameId).find('#StuddivTF' + frameId + sheetId).remove();
                //}
               
                    
                
                r = confirm("Are you sure ?");
                if (r == true) {
                    $('#divContentPages').find('#divTF' + frameId).remove();
                    $('#ulStudents').find('#divStud' + frameId).remove();
                    $('#ulStudents').find('#divAll' + frameId).remove();
                }
                else {
                    return;
                }
                

            }
            else if (pageType == "BE") {

                frameId = name;
                var ct = $('#divStud' + frameId).children().size();

                $('#divContentPages').find('#divTFBEH' + frameId + "0").remove();

                if (ct == 2) {
                    $('#ulStudents').find('#divStud' + frameId).remove();
                    $('#ulStudents').find('#divAll' + frameId).remove();
                } else {
                    $('#ulStudents').find('#StuddivTFBEH' + frameId + "00").remove();
                }
            }
            else {
                frameId = $(name).attr('id');
                // To Set Back to Graph Tile Menu - Dev 2 [19-Jul-2020] - Start
                
                var ChkMatch1 = frameId.match("divGTG");
                var ChkMatch2 = frameId.match("divCHGHRT");
                var ChkMatch3 = frameId.match("divBSRG");
                var ChkMatch4 = frameId.match("divGR");
                var ChkMatch5 = frameId.match("divET");
                var ChkMatch6 = frameId.match("divMG");
                var ChkMatch7 = frameId.match("divPRPT");
                var ChkMatch8 = frameId.match("divPSR");
                var ChkMatch9 = frameId.match("divPSRC");
                var ChkMatch10 = frameId.match("divEVR");
                var x= document.getElementById('<%= graphdivClose.ClientID%>');
                if (ChkMatch1 && x.value == "1" || ChkMatch2 || ChkMatch3 || ChkMatch4 || ChkMatch5 || ChkMatch6 || ChkMatch7 || ChkMatch8 || ChkMatch9 || ChkMatch10) {
                    x.value = "0";
                    var Matchcount;
                    if (ChkMatch1) Matchcount = ChkMatch1.length;
                    else if (ChkMatch2) Matchcount = ChkMatch2.length;
                    else if (ChkMatch3) Matchcount = ChkMatch3.length;
                    else if (ChkMatch4) Matchcount = ChkMatch4.length;
                    else if (ChkMatch5) Matchcount = ChkMatch5.length;
                    else if (ChkMatch6) Matchcount = ChkMatch6.length;
                    else if (ChkMatch7) Matchcount = ChkMatch7.length;
                    else if (ChkMatch8) Matchcount = ChkMatch8.length;
                    else if (ChkMatch9) Matchcount = ChkMatch9.length;
                    else if (ChkMatch10) Matchcount = ChkMatch10.length;
                    if (Matchcount > 0) {
                        $("#GraphDashBoard").trigger('click');
                    }
                }
                else {
                    $("#divContentPages").children().hide();
                    $('#divContentPages').find('#' + frameId).remove();
                }
                // To Set Back to Graph Tile Menu - Dev 2 [19-Jul-2020] - End
                
                
            }

            $('#contactForm').hide();
            //[2019-02-25] disable DashBoard
            //getDashBoardReport();
            // $('#divContentPages').find('#divTFGraph').show();

            setName("");


            $('.alpha').removeAttr('id');

            $('.fillLessons').hide();
            $('.fillLessons2').hide();

            //function to clear the tempOverrideHT session details
            resetOverrideSessionfn(sheetId);

            //var datasheets = $('[id^="divTF"]').not('#divTFGraph');
            //if (datasheets.length > 0) {
            //    for (var i = 0; i < datasheets.length; i++) {
            //        alert($(datasheets[i]).attr('id'));
            //    }
            //}
            //else {
            //    $('#newBehFrame').css('right', '0');
            //    $('#newBehTop').css('right', '0');
            //}

            if (pageType == "DS" && sheetId > 0) {
                listLessonPlan();
            }
        }



        function removeFrame1(name, sheetNo, pageType) {
            var frameId;
            if (pageType == "DS") {
                frameId = name;
                var newName = "sheetDiv-" + name + "-" + sheetNo.toString();
                var isSheet = $('#divContentPages').find('#divTF' + name).find('#LeftDiv' + name).children().size();

                var sheetId;
                $("#divContentPages").find('#divTF' + name).find('#LeftDiv' + name).children().each(function (n, i) {

                    if ($(this).css('display') == 'block') {
                        sheetId = $(this).attr('id');
                        var valNew = sheetId.split('-');
                        sheetId = valNew[2];
                    }

                })
                var newName;
                if (sheetId != null) { newName = "sheetDiv-" + name + "-" + sheetId.toString(); }
                    
                    $('#divContentPages').find('#divTF' + frameId).remove();
                    $('#ulStudents').find('#divStud' + frameId).remove();
                    $('#ulStudents').find('#divAll' + frameId).remove();

            }
            
        $('#contactForm').hide();
             //[2019-02-25] disable DashBoard
             //getDashBoardReport();
             // $('#divContentPages').find('#divTFGraph').show();

        setName("");


        $('.alpha').removeAttr('id');

        $('.fillLessons').hide();
        $('.fillLessons2').hide();

             //function to clear the tempOverrideHT session details
        resetOverrideSessionfn(sheetId);

          

        if (pageType == "DS" && sheetId > 0) {
            listLessonPlan();
        }
    }
       

        function addBehaviour() {
            $('#ulsubmenu #Datasheet\\.aspx').trigger('click');
        }

        function addLDtoExisting(name, iname, page, StudentId, sheetCount, selSubMenu_ID) {
            $('#contactForm').hide();
            var sDExit = $('#divContentPages').find('#' + name).length;//Student Datasheet Exit
            if (sDExit > 0) {


                $('#divTF' + StudentId).show();

                var leftDiv = "LeftDiv" + StudentId;
                $(leftDiv).show();
                var divL = document.getElementById(leftDiv);
                $(divL).children().hide();


                var newName = "sheetDiv-" + StudentId + "-" + sheetCount.toString();
                var sheetDiv = document.createElement('div');
                sheetDiv.setAttribute('id', newName);

                divL.appendChild(sheetDiv);
                if (selSubMenu_ID > 0) {
                    var ifr = document.createElement('iframe');
                    ifr.setAttribute('id', "leftIframe");
                    ifr.setAttribute('width', '100%');
                    ifr.setAttribute('height', '1550px');
                    ifr.setAttribute('frameborder', '0');
                    ifr.setAttribute('src', page);
                    sheetDiv.appendChild(ifr);

                    //$('#leftIframe').hide();
                }


            }
        }

        function ChangeAcitveSess(tempId) {

            if ($("#StuddivTF" + tempId).length > 0) {
                $("#StuddivTF" + tempId).remove();
            }

        }

        function addDatasheetFrame(name, iname, page, StudentId, sheetCount) {
            $('#contactForm').hide();
            var ni = document.getElementById('divContentPages');
            var newdiv = document.createElement('div');




            newdiv.setAttribute('id', name);
            newdiv.style.position = 'relative';



            var img = document.createElement('img');
            img.setAttribute('src', '../Administration/images/button_red_close.png');
            img.style.position = 'absolute';
            var isiPad = navigator.userAgent.match(/iPad/i);

            //img.style.left = '99%';
            //img.style.marginTop = '-4%';
            if (isiPad != null) {
                img.style.left = '71%';
                img.style.marginTop = '-1%';
            }
            else {
                img.style.left = '73%';
                img.style.marginTop = '0';
            }

            img.style.width = '25px';
            img.style.height = '25px';
            img.setAttribute('onclick', 'removeFrame(' + StudentId + ',' + sheetCount + ',"DS")');

            newdiv.appendChild(img);




            var leftName = 'LeftDiv' + StudentId;

            var leftDiv = document.createElement('div');
            leftDiv.setAttribute('id', leftName);
            leftDiv.setAttribute('class', 'lftPartContainer');
            var newName = "sheetDiv-" + StudentId + "-" + sheetCount.toString();
            var sheetDiv = document.createElement('div');
            sheetDiv.setAttribute('id', newName);
            leftDiv.appendChild(sheetDiv);

            if (sheetCount > 0) {
                var ifr = document.createElement('iframe');
                ifr.setAttribute('id', "leftIframe");
                ifr.setAttribute('width', '100%');
                ifr.setAttribute('height', '650px');
                ifr.setAttribute('frameborder', '0');

                if (page.indexOf('Demography.aspx') == 0) page = "../Administration/Facesheet.aspx";
                ifr.setAttribute('src', page);
                sheetDiv.appendChild(ifr);
            }

            newdiv.appendChild(leftDiv);


            var rightDiv = document.createElement('div');
            rightDiv.setAttribute('id', 'rightDiv');


            var ifr = document.createElement('iframe');
            ifr.setAttribute('id', "rightIframe");
            ifr.setAttribute('width', '101%');
            ifr.setAttribute('height', '650px');
            ifr.setAttribute('frameborder', '0');
            var isiPad = navigator.userAgent.match(/iPad/i);
            var SelStud = "";
            $('.students2').each(
                function () {
                    var status = $(this).hasClass('stdSel');
                    if (status == true) {
                        SelStud = this.id;
                    }
                }
            )
            var Selstudid = "stud-" + SelStud;
            if (isiPad != null) {
                leftDiv.setAttribute('class', 'lftPartContainerIpad');
                rightDiv.setAttribute('class', 'rightPartContainerIpad');
                ifr.setAttribute('src', "DatasheetTimerIpad.aspx?stid=" + Selstudid);
            } else {
                leftDiv.setAttribute('class', 'lftPartContainer');
                rightDiv.setAttribute('class', 'rightPartContainer');
                ifr.setAttribute('src', "dataSheetTimer.aspx?stid=" + Selstudid);
            }
            rightDiv.appendChild(ifr);
            newdiv.appendChild(rightDiv);

            ni.appendChild(newdiv);

            $('#newBehFrame').fadeIn();

        }


        function addFrame(name, iname, page, StudentId) {

            $('#contactForm').hide();
            var ni = document.getElementById('divContentPages');




            var newdiv = document.createElement('div');
            newdiv.setAttribute('id', name);
            newdiv.style.position = 'relative';





            var temp = page.split('?');
            if (name != "divTFGraph") {

                var img = document.createElement('img');
                img.setAttribute('src', '../Administration/images/button_red_close.png');
                img.style.position = 'absolute';
                var isiPad = navigator.userAgent.match(/iPad/i);
                if (isiPad != null && temp[0] == "ACSheet.aspx") {
                    img.style.top = '-1.5%';
                }
                else {
                    img.style.top = '-1.25%';
                }
                img.style.left = '99%';
                img.style.width = '25px';
                img.style.height = '25px';

                if (name.indexOf('divTFBEH') >= 0) {
                    img.setAttribute('onclick', 'removeFrame(' + StudentId + ',0,"BE")');
                } else {
                    img.setAttribute('onclick', 'removeFrame(' + name + ',0,"OP")');
                }
                newdiv.appendChild(img);

            }

            ni.appendChild(newdiv);

            ni = document.getElementById(name);
            var ifr = document.createElement('iframe');
            ifr.setAttribute('id', iname);
            ifr.setAttribute('width', '100%');
            ifr.setAttribute('height', '1550px');
            ifr.setAttribute("style", "overflow:hidden;");

            ifr.setAttribute('frameborder', '0');

            <% sesso = (clsSession)HttpContext.Current.Session["UserSession"];
               if (sesso.SchoolId == 2)
               { %>

            if (page.indexOf('Demography.aspx') >= 0) {

                page = "../../../ClientDB/(S(<%: Session.SessionID%>))/ProtocolSummary/GetProtocolSummary?StudentId=" + StudentId;
            }
            else if (page.indexOf('ProgressReport') >= 0) {
                page = "../../../ClientDB/(S(<%: Session.SessionID%>))/ProgressReport/GetProgRpt?studentid=" + StudentId;
            }
            <% }%>

            ifr.setAttribute('src', page);

            ni.appendChild(ifr);

        }
        function removeElement(divNum) {
            var d = document.getElementById('myDiv');
            var olddiv = document.getElementById(divNum);
            d.removeChild(olddiv);
        }
        function MyMethod_Result(ResultString) {

            var IepEdit = document.getElementById("IEPEdit");
            if (ResultString == true) {
                IepEdit.style.visibility = "visible";
            }
            else
                IepEdit.style.visibility = "hidden";
        }

        var currStudentFlag = 0;

        function OnSuccessStdAssignNext(result) {

            z = document.createAttribute('class');
            z.value = 'students-active';
            result.setAttributeNode(z);


            //y = document.createAttribute('class');
            //y.value = 'studentphoto-active';
            //result.childNodes[0].childNodes[0].setAttributeNode(y);
            //var img = result.childNodes[0].childNodes[1].getElementsByTagName("img");
            //img[0].src = '../Administration/images/close-green.png';
            if (currentMenu != null) {

                PageMethods.FillSubMenu(currentMenu.textContent, OnSuccess, OnFailure);
                resetScroll();
            }
        }
        function OnSuccessStdAssignPrev(result) {

            z = document.createAttribute('class');
            z.value = 'students';
            result.setAttributeNode(z);


            //y = document.createAttribute('class');
            //y.value = 'studentphoto';
            //result.childNodes[0].childNodes[0].setAttributeNode(y);
            //var img = result.childNodes[0].childNodes[1].getElementsByTagName("img");
            //img[0].src = '../Administration/images/close-red.png';
            if (currentMenu != null) {
                PageMethods.FillSubMenu(currentMenu.textContent, OnSuccess, OnFailure);
                resetScroll();
            }
        }

        //test

        function selSidemenu1(divName, stud, id) {

            //if ($('#if_dash') != null)
            //    $('#if_dash').remove();
            document.getElementById('dash_area').style.display = 'none';
            //document.getElementById('divStatus').style.display = "none";
            stud = "stud-" + stud;
            var val = document.getElementById('hidCureentStudent').value;
            var Next = document.getElementById(stud)
            var Prev = document.getElementById(val)

            if (stud != val) {

                selStudent(Next, 1);


                OnSuccessStdAssignNext(Next);
                OnSuccessStdAssignPrev(Prev);
            }



            var divC = document.getElementById('divContentPages');
            for (var i = 0; i < divC.childNodes.length; i++) {
                document.getElementById(divC.childNodes[i].id).style.display = "none";
            }

            showandHideMenu();

            divName.style.display = "block";
            setName(divName.id);
            document.getElementById('hidCureentStudent').value = stud;

            var actSessId = $(divName).attr('id');
            var actSesstitle = $('#ulStudents').find('#Stud' + actSessId).attr('title');

            $('#ulsubmenu').find('.alpha').removeAttr('id');

            var spanList = $('#ulsubmenu').find('span').filter(function () {
                return $(this).text() == actSesstitle.trim();
            });

            $(spanList[0]).parent().parent().attr('id', 'submenu_active');

            // alert(actSesstitle);

        }
        function removeAllDivs() {
            $("#divContentPages").children().each(function (n, i) {
                var id = this.id;
                if (id.indexOf('TF') < 2) {
                    $("#" + id).remove();
                }
            });
        }
        function setBehVisible(ID) {
            $('#divContentPages').find('#divTFBEH' + ID + '0').show();
            setName('divTFBEH');
        }
        function setDatasheetVisible(ID, sheetCount) {
            $('#divTF' + ID).show();
            var leftDiv = "LeftDiv" + ID;
            $(leftDiv).show();
            var divL = document.getElementById(leftDiv);
            $(divL).children().hide();

            var newName = "sheetDiv-" + ID + "-" + sheetCount.toString();

            $('#' + newName).show();
            setName('divTF');
        }
        function selSidemenu(divName, stud, id, sheetCount) {
            var ID = stud;
            stud = "stud-" + stud;
            var val = document.getElementById('hidCureentStudent').value;
            var Next = document.getElementById(stud)
            var Prev = document.getElementById(val)

            if (stud != val) {

                selStudent(Next, 1);
                OnSuccessStdAssignNext(Next);
                OnSuccessStdAssignPrev(Prev);
            }



            var divNameId = $(divName).attr('id');

            removeAllDivs();

            if (divNameId == null) divNameId = divName;

            $("#divContentPages").children().hide();

            if (divNameId.indexOf('TFBEH') > 0) {

                setBehVisible(ID);

            }
            else {

                setDatasheetVisible(ID, sheetCount)
            }

            $('#divTFGraph').hide();
            $('#contactForm').hide();


            var sideMenuText = $('#ulStudents').find('#Stud' + divNameId.trim()).find('.sublist').find('a').html();


            $('#ulsubmenu').find('.alpha').removeAttr('id');
            if (sideMenuText != null) {
                $('#ulsubmenu').find('span').filter(function () { return ($(this).text().indexOf(sideMenuText.trim()) > -1) }).eq(1).parents('.alpha').attr('id', 'submenu_active');
            }


            $('.fillLessons').hide();
            $('.fillLessons1').hide();
            $('.fillLessons2').hide();
        }



        function slideDOWNorUP(studid) {
            var children = document.getElementById("divStud" + studid + "").getElementsByTagName('div');
            var time = children[0].firstChild.nodeValue;

        }
        function selStudentbySide(div) {
            $("#ulsubmenu").empty();

            if (currentStud != null) {
                a = document.createAttribute('class');
                a.value = 'students';

            }
            currentStud = div;

            PageMethods.SetStudentID(div.id, OnSuccessStdAssignBySide, OnFailure);


        }
        function OnSuccessStdAssignBySide(result) {
            z = document.createAttribute('class');
            z.value = 'students-active';
            currentStud.setAttributeNode(z);
            //y = document.createAttribute('class');
            //y.value = 'studentphoto-active';
            //currentStud.childNodes[0].childNodes[0].setAttributeNode(y);
            //var img = currentStud.childNodes[0].childNodes[1].getElementsByTagName("img");
            //img[0].src = '../Administration/images/close-green.png';

        }
        function selMenubySide(menu) {
            //if ($('#if_dash') != null)
            //    $('#if_dash').remove();
            document.getElementById('dash_area').style.display = 'none';
            currentMenu = document.getElementById("menu_active");
            if (currentMenu != null)
                currentMenu.removeAttribute("id");
            z = document.createAttribute('id');
            z.value = 'menu_active';
            menu.setAttributeNode(z);
            currentMenu = menu;
            var Menu = menu.textContent;
            if (currentStud != null)
                PageMethods.FillSubMenu(Menu, OnSuccessbySide, OnFailure);
            resetScroll();
        }
        function OnSuccessbySide(response) {
            $("#ulsubmenu").empty();
            for (var i = 0; i < response.length; i++) {
                if (response[i].Url == "Graph" || response[i].Url == "Datasheet" || response[i].Url == "Lesson") {
                    $("#ulsubmenu").append("<li class='alpha'><a id=" + response[i].Url + " ><span><span>" + response[i].Submenu +
                                                "</span></span> </a></li>");
                }
                else {
                    $("#ulsubmenu").append("<li class='alpha'><a id=" + response[i].Url + " onclick='selSubmenu(this," + response[i].ID + "," + response[i].StudID + ");'><span><span>" + response[i].Submenu +
                                                "</span></span> </a></li>");
                }
            }
            if (callType == 1) {
                var submenus = document.getElementsByClassName("alpha");

                for (var i = 0; i < submenus.length; i++) {
                    if (_submenu == submenus[i].textContent) {
                        z = document.createAttribute('id');
                        z.value = 'submenu_active';
                        submenus[i].setAttributeNode(z);
                        var childs = submenus[i].getElementsByTagName('a')[0];

                        x = document.createAttribute('src');
                        x.value = submenus[i].childNodes[0].id;
                        var frame = document.getElementById('ifContent');
                        frame.setAttributeNode(x);
                    }
                    else {
                        submenus[i].removeAttribute("id");
                    }
                }
                callType = 0;
            }
        }


    </script>
    <script type="text/javascript">

        function getBehaviour(StudentId) {
            $.ajax(
            {

                type: "POST",
                url: "Home.aspx/getbehaviourdata",
                data: "{'studentid':'" + StudentId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                    var list = JSON.parse(data.d);


                    $('#behaviourdetail').children().remove();

                    if (list.length > 0) {
                        $('#showdiv').show();
                    }
                    else {
                        $('#showdiv').hide();
                    }

                    $.each(list, function (MeasurementId, Behaviour, frequency) {
                        var frequencyduration = "Duration";
                        if (Behaviour.Frequency == "True") frequencyduration = "Frequency";
                        $('#behaviourdetail').append('<div id="' + "div" + Behaviour.MeasurementId + '" ><table class="slideDivTable" style="width:100%; " align="left"><tr><td style="width:80%"><a  style="text-decoration: underline; color: blue; font-size: 14px;" id="' + Behaviour.MeasurementId + '" onclick="javascript:behaviourdetails(this);">' + Behaviour.Behaviour + '</a> - <span id="' + "span" + Behaviour.MeasurementId + '" >' + frequencyduration + '</span><div id="' + "durationdiv" + Behaviour.MeasurementId + '" style="visibility:hidden" >0</div> </td></tr></table></div>');
                    });
                },
                error: function (request, status, error) {
                    alert("Error");
                }
            });
        }
        var goal;
        var frequency;
        function behaviourdetails(event) {

            goal = event.id;



            var selType = $(event).attr('class');

            if (selType == "Frequency") {
                var submenuItem = "frequency/" + $(event).html();
                selSubmenu('frequency/' + $(event).html(), 0);

            }
            else if (selType == "Duration") {
                var submenuItem = "duration/" + $(event).html();
                selSubmenu('duration/' + $(event).html(), 0);


            }
            var top = $("#submenu").offset().top;

        }
    </script>


    <%--    For Setting Behaviou AlarmAlert--%>

    <style>
        .smallDiv {
            text-align: center;
            margin: 0 0 10px 0;
            width: 300px;
            -webkit-border-radius: 5px;
            border-radius: 5px;
            -moz-box-shadow: -1px -1px 4px #1c1c1c;
            -webkit-box-shadow: -1px -1px 4px #1c1c1c;
            box-shadow: -1px -1px 4px #1c1c1c;
        }

        .lightbox {
            background: url("../Administration/images/shdo.PNG") no-repeat scroll center bottom transparent;
            height: auto !important;
            margin: 0 auto;
            min-height: 35px;
            padding: 0 0 16px;
            width: 252px;
        }

        .content {
            background: none repeat scroll 0 0 #EEEEEE;
            border: 1px solid #C6C7C7;
            border-radius: 5px 5px 5px 5px;
            height: auto !important;
            min-height: 40px;
            padding: 5px;
            width: 250px;
        }

        .notifier {
            border: 1px solid black;
            background-color: red;
            color: white;
            float: left;
            font-size: 10px;
            height: 16px;
            margin-left: -5px;
            margin-top: -10px;
            text-align: center;
            width: 16px;
            display: none;
            /*background: url("images/bubble.png") no-repeat scroll left top rgba(0, 0, 0, 0);*/
        }
    </style>
    <script type="text/javascript">
        var checkmin = 0;
        var noOfAlarms = 0;
        var myVar = setInterval(function () { myTimer() }, 1000);
        //var myGeTime = setInterval(function () { getAlarm() }, 300000);
        var myGeTime = setInterval(function () { getAlarm() }, 58000);
        var timalrm = '';
        var alartimecheck;
        var noOfRows;
        var alarmMessage;
        var alarmId;
        var alrmIdNow;
        var snoozid;
        var snoozTime;
        var snoozMsg = 0;
        var snoozMsgtoset;
        var snoozidtoset;
        var alartimecheckFinal = {};
        var noOfActiveAlarms = 0;
        function addMinAlarm() { };

        //second alarm
        var alarmMessage1 = '';
        var alarmId1;
        var noOfRows1 = 0;

        function myTimer() {

            var CurrtDateTime = new Date();
            var CurTime = CurrtDateTime.toTimeString().substring(0, 8);
            $('#BehaviourTimes').val(CurTime);
            //  document.getElementById("BehaviourTimes").value = CurTime;

            // document.getElementById("NoOfalarm")
            if ($('NoOfalarm') != null) {
                $('NoOfalarm').html(noOfAlarms)
            }
            // alert($('NoOfalarm').html());
            //document.getElementById("NoOfalarm").innerHTML = noOfAlarms;

            if (noOfRows > 0) {
                for (var iCounterNoOfAlrm = 0; iCounterNoOfAlrm < noOfRows; iCounterNoOfAlrm++) {
                    if (alartimecheckFinal[iCounterNoOfAlrm] == CurTime) {
                        alrmIdNow = alarmId[iCounterNoOfAlrm];
                        snoozidtoset = alarmId[iCounterNoOfAlrm];
                        snoozMsgtoset = alarmMessage[iCounterNoOfAlrm];
                    }
                }
            }

        }

        function addMinAlarm() {
            if (noOfRows > 0) {
                var finalAlmTime = "";
                for (var iCounter = 0; iCounter < noOfRows; iCounter++) {

                    var tenpAlTime = alartimecheck[iCounter].split(':');
                    var hours = parseInt(tenpAlTime[0]);
                    var minutes = parseInt(tenpAlTime[1]);
                    minutes = minutes - 5;
                    if (minutes < 0) {
                        minutes = 60 + minutes;
                        hours = hours - 1;
                        if (hours < 0)
                            hours = 0;
                    }
                    if (minutes < 10)
                        minutes = "0" + minutes
                    if (hours < 10)
                        hours = "0" + hours;
                    finalAlmTime = finalAlmTime + hours + ':' + minutes + ':00,';
                }
                alartimecheckFinal = finalAlmTime.split(',');
            }
        }


        function getAlarm() {

            $.ajax({
                url: "Home.aspx/getReminder",
                data: "{ 'TimeNow': '" + document.getElementById("BehaviourTimes").value + "','StudId': '2'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {

                    var contents = data.d;

                    if (contents.indexOf("System.IndexOutOfRangeException") == -1) {
                        var altTime = "";
                        var altMsg = "";
                        var altId = "";
                        var tabVal1and2 = contents.split('$');
                        var tabVal = tabVal1and2[0].split(',,');
                        var newAlert = tabVal1and2[2];

                        var prevAlarm = noOfAlarms;	
                        noOfRows = tabVal[1];
                        noOfAlarms = tabVal[0];
                        //if (noOfAlarms > prevAlarm )
                        if (newAlert == 1) {
                            var parntHeight = document.getElementById("dashboard-top-panel").offsetHeight;	
                            $("#alertBtn").css({	
                                "display": "grid",	
                                "align-items": "center", "border-radius": "3px",	
                                "height": "" + parntHeight - 5 + "px",	
                                "background-color": "red", "margin": "-10px 6.3px"	
                            });	
                            //playSound("../StudentBinder/Sounds/mixkit-software-interface-start-2574.wav");
                            //    var play = function play(audioBuffer) {
                            //        var source = context.createBufferSource();
                            //        source.buffer = audioBuffer;
                            //        source.connect(context.destination);
                            //        source.start();
                            //    };
                            var isiPad = navigator.userAgent.match(/iPad/i);

                            if (isiPad) {
                                $("#unmute").trigger("click");
                            }
                            $("#audioPlay").trigger("click");
                            $('.alertsPopUp').css({"display":"none"});	
                        }	
                        //var btnHeight = $("#dashboard-top-panel").height();	
                        //$("#alertBtn").height = btnHeight;
                        if (tabVal[1] > 0) {
                            document.getElementById("NoOfalarm").innerHTML = tabVal[1];
                            for (var iIndexTab = 0; iIndexTab < tabVal[1]; iIndexTab++) {
                                var tabValRow = tabVal[iIndexTab + 2].split(',');

                                altTime = altTime + tabValRow[5] + ',';

                                var time12 = tabValRow[5].split(':');
                                //it is pm if hours from 12 onwards
                                var hours = parseInt(time12[0])
                                suffex = (hours >= 12) ? 'PM' : 'AM';
                                //only -12 from hours if it is greater than 12 (if not back at mid night)
                                hours = (hours > 12) ? hours - 12 : hours;
                                //if 00 then it is 12 am
                                hours = (hours == '00') ? 12 : hours;

                                var endTime12 = tabValRow[9].split(':');
                                //it is pm if hours from 12 onwards
                                var endHours = parseInt(endTime12[0])
                                endSuffex = (endHours >= 12) ? 'PM' : 'AM';
                                //only -12 from hours if it is greater than 12 (if not back at mid night)
                                endHours = (endHours > 12) ? endHours - 12 : endHours;
                                //if 00 then it is 12 am
                                endHours = (endHours == '00') ? 12 : endHours;

                                //var alarmTimeMsg = hours + ':' + time12[1] + ':' + time12[2] + suffex;
                                var alarmTimeMsg = hours + ':' + time12[1] + " " + suffex + ' - ' + endHours + ':' + endTime12[1] + " " +endSuffex;

                                if (tabValRow[7] == 'True') {


                                    altMsg = altMsg + tabValRow[6] + '<br/> ' + tabValRow[3] + ' <br/> ' + alarmTimeMsg + ';' + '<br/> IOA : ' + tabValRow[8] + ' ,';
                                }
                                else {
                                    altMsg = altMsg + tabValRow[6] + ' <br/> ' + tabValRow[3] + ' <br/> ' + alarmTimeMsg + ';' + ',';
                                }
                                altId = altId + tabValRow[1] + ',';

                            }

                            alartimecheck = altTime.split(',');
                            alarmMessage = altMsg.split(',');
                            alarmId = altId.split(',');
                        }

                        //alarm for Datasheet
                        if (tabVal1and2[1] != null) {
                            var tabVal2 = tabVal1and2[1].split(',,');
                            // alert(tabVal2[3]);
                            noOfRows1 = tabVal2[0];
                            // alert(tabVal2[0]);
                            noOfAlarms = parseInt(noOfAlarms) + parseInt(tabVal2[0]);
                            if (parseInt(tabVal2[0]) > 0) {

                                for (var iIndexTab1 = 0; iIndexTab1 < tabVal2[0]; iIndexTab1++) {
                                    // alert(tabVal2[parseInt( iIndexTab1) + 3]);
                                    var tabValRow1 = tabVal2[iIndexTab1 + 1].split(',');

                                    alarmMessage1 = alarmMessage1 + tabValRow1[0] + '</br>' + tabValRow1[1] + '</br>' + tabValRow1[2] + ',';
                                    // alarmId1 = alarmId1 + tabValRow1[0] + ',';

                                }

                            }
                            LoadDatasheetAlarm();
                        }


                        LoadAlarm();

                    }
                    else {
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }

            });

            addMinAlarm();

        }

        function getNotifier() {

            var sublistLength = $('#ulStudents').find('.sublist').length;
            if (sublistLength > 0) {

                $('.notifier').fadeIn('fast', function () {
                    $('.notifier').html(sublistLength);
                });
            }
            else {
                $('.notifier').hide();
            }


        }

		function playSound(url) {	
            var audio = new Audio(url);	
            audio.play();	
        }

        function updateAlarmStatus(indexVal) {

            $.ajax({
                url: "Home.aspx/AlarmStatusUpdate",
                data: "{ 'ReminderId': '" + alarmId[indexVal] + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {


                    var contents = data.d;
                    if (contents.indexOf("System.IndexOutOfRangeException") == -1) {
                        snoozMsg = 0;
                        $('#AlAllDiv' + indexVal + '').hide();
                        getAlarm();

                    }

                    else {
                        // alert('Error');
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //   alert(textStatus);
                }

            });



            return false;
        }

        var contentReturn;
        function addMin(indexVal) {
            var currentTime = new Date();
            var hours = currentTime.getHours()
            var minutes = currentTime.getMinutes()
            minutes = minutes + 10;
            if (minutes > 60) {
                minutes = minutes - 60;
                hours = hours + 1;
                if (hours > 24)
                    hours = hours - 24;
            }
            if (minutes < 10)
                minutes = "0" + minutes
            if (hours < 10)
                hours = "0" + hours;
            snoozTime = "" + hours + ":" + minutes + ":00";
            updateSnooZStatus(snoozTime, 'check', indexVal);
            snoozMsg = snoozMsgtoset;
            snoozid = snoozidtoset;
            $('#AlAllDiv' + indexVal + '').hide();
            return false;

        }

        function updateSnooZStatus(snzTime, condition, indexVal) {
            $.ajax({
                url: "Home.aspx/AlarmSnooZUpdate",
                data: "{ 'ReminderId': '" + alarmId[indexVal] + "','snoozTime': '" + snzTime + "','conditionTest': '" + condition + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    var contents = data.d;
                    if (contents.indexOf("System.IndexOutOfRangeException") == -1) {

                        $('#AlAllDiv' + indexVal + '').hide();
                        LoadAlarm();
                        contentReturn = contents;
                        if (contents == 'existing') {
                            var cnfrm = confirm("Alarm already present. Do you want to continue?");
                            if (cnfrm == true) {
                                updateSnooZStatus(snzTime, 'updateIt', indexVal);

                            }
                            else {
                                updateAlarmStatus(indexVal);
                            }
                        }
                    }

                    else {

                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }

            });


            return false;
        }


        function LoadAlarm() {
            $('#containerAlarm').empty();
            if ((noOfRows + noOfRows1) > 0) {

                for (var iDivIndex = 0; iDivIndex < noOfRows; iDivIndex++) {
                    var spltMsg = alarmMessage[iDivIndex].split(';');
                    $('#containerAlarm').append('<div class="lightbox" id="AlAllDiv' + iDivIndex + '"><div class="content"  > <table class="auto-style1" Width="100%"><tr><td><asp:Label ID="Label1"   runat="server" Width="150px" Text="' + spltMsg[0] + '" ></asp:Label></td><td style="vertical-align: top;" rowspan="2" Width="50px"><img id="BtnDismiss" src="../Administration/images/dissmis.png" alt="Dismiss" onclick="return updateAlarmStatus(' + iDivIndex + ');"/>' +
                        //'<img id="BtnSnooze" src="../Administration/images/snooz.png" alt="Snooze" onclick="return addMin(' + iDivIndex + ');" />' +
                        '</td></tr><tr><td><asp:Label ID="LabelMessagebehav"  Width="170px" runat="server" Text="' + spltMsg[1] + '" ></asp:Label></td></tr></div></div>');
                }


            }
            else {
                $('#containerAlarm').append('<div class="lightbox" id="AlAllDivNo"><div class="content"  > <asp:Label ID="Labelno"   runat="server" Width="150px" Text="No Alarms Present" ></asp:Label></div></div>');
                //AletsRem();
                removeColor();
                document.getElementById("NoOfalarm").innerHTML = 0;
                $('.alertsPopUp').css({ "display": "none" });
            }
        }

        function LoadDatasheetAlarm() {
            // alert(alarmMessage1);
            if (noOfRows1 > 0) {
                var spltMsg = alarmMessage1.split(',');

                for (var iDivIndex = 0; iDivIndex < noOfRows1; iDivIndex++) {


                    // var id = alarmId1.split(',');
                    $('#containerAlarm').append('<div class="lightbox" id="AlAllDivDatasheet' + iDivIndex + '"><div class="content"  > <table class="auto-style1" Width="100%">' +
                        '<tr><td><asp:Label ID="Label12"   runat="server" Width="100px" Text="' + spltMsg[iDivIndex] + '" ></asp:Label></td><td rowspan="2" Width="100px">' +
                        '<img id="BtnDismiss" src="../Administration/images/dissmis.png" alt="Dismiss" onclick="return updateDatasheetAlarmStatusFormsg(' + iDivIndex + ');"/>' +
                        '</td></tr><tr><td></td></tr></div></div>');
                }

                alarmMessage1 = '';
                alarmId1 = '';
            }
        }
        function updateDatasheetAlarmStatusFormsg(id) {

        }

        function enableAllAlerts() {
            console.log("enableAllAlerts")
            var sid = $('#studentContainer').find('.students-active').attr('id');
            if(sid=="undefined"||sid==undefined){
                failMsg("Please select a student");
                return;
            }
            $.ajax(
                  {
                      type: "POST",
                      url: "Home.aspx/enableAllAlert",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,
                      success: function (data) {
                          var alertmsg = JSON.parse(data.d);
                          //alert("alertmsg:"+alertmsg);
                          if(alertmsg=="1"){
                              //alert("Please select a student");
                              failMsg("Please select a student");
                          }else if(alertmsg=="2"){
                              failMsg("No Data Found");
                          }else{
                              succesMsg("Timer On");
                              $('#TIMERS').trigger('click');
                              document.getElementById("play").src="../Administration/images/icons8-bell-on.png";
                              getAlarm();
                          }

                      },
                      error: function (request, status, error) {
                          alert("Error");
                      }
                  });
        }

        function succesMsg(msg) {
            //if (msg == "Student Reminder Saved Successfully") {
            //    parent.getAlarm();
            //}
            //alert(msg);
            document.getElementById('tdMsg').innerHTML = "<div class='valid_box'>" + msg + ".</div>";
            HideLabel('tdMsg');
        }

        function failMsg(msg) {
            //if (msg == "Timer On") {
            //    parent.getAlarm();
            //}
            //alert(msg);
            document.getElementById('tdMsg').innerHTML = "<div class='error_box_head'>" + msg + ".</div>";
            HideLabel('tdMsg');
        }

        function alertDisabled() {
            document.getElementById('play').src='../Administration/images/icons8-bell-off.png';
        }

        function HideLabel(label) {
            var seconds = 5;
            setTimeout(function () {
                //document.getElementById(""+label).style.display = "none";
                document.getElementById("" + label).innerHTML = "";
            }, seconds * 1000);
        };
    </script>

    <%--    For Setting Behaviou Alarm--%>





    <style type="text/css">
        @font-face {
            font-family: Oswald;
            src: url("../Administration/Oswald.ttf");
        }

        @font-face {
            font-family: Orienta;
            src: url("../Administration/Orienta-Regular.otf");
        }
    </style>
    <script type="text/javascript">
        function resizeIframe(obj) {
            obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
        }
    </script>
    <script type="text/javascript">
        function droper() {

            if (document.getElementById('menu').style.display == "none") {
                $('#menu,#submenu').slideDown();
                $('#dropper').removeClass('bottodown');
                $('#dropper').addClass('bottomtop');
            }
            else {
                $('#menu,#submenu').slideUp();
                $('#dropper').removeClass('bottomtop');
                $('#dropper').addClass('bottodown');
            }



        }
        function studclsSearch() {
            $('.classDiv').slideToggle('slow');
            $('#<%=txtSname.ClientID %>').val("");
        }

        function closeDivDsch() {
            $('.classDivNew').slideToggle('slow');
            $('#<%=txtSnameDsch.ClientID %>').val("");
        }

        function NoDatasheetAcess() {
            alert('No Permission , Access Denied');
        }
        function responseload() {

            window.location.href = "../Administration/AdminHome.aspx";
        }
    </script>
    <style type="text/css">
        #myTooltip {
            padding: 5px;
            background-color: #FFF8DC;
            border: 1px solid #DEB887;
            width: 180px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #6b6b6b;
            display: none;
            position: absolute;
            left: 0px;
            top: 0px;
        }
    </style>
</head>
<body onkeydown="keyDownEvents();">
<button id="unmute" style="display:none">Unmute</button>


    <form id="form1" runat="server">


        <div class="StudPopup" id="dvStudPopup" style='display: none;'>
            <p></p>
        </div>
        <asp:HiddenField ID="hfDashType" runat="server" />
        <input type="hidden" value="0" id="hidActiveStudents" />
        <input type="hidden" value="0" id="hidCureentStudent" />
        <input type="hidden" value="0" id="hidOpenWindows" />
        <input type="hidden" value="0" id="hfMenuList" />
        <input type="hidden" value="0" id="hdIsipad" />

        <div id="myTooltip"></div>

        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
            <!-- top panel -->
            <div id="dashboard-top-panel">

                <div id="top-panel-container">
                    <ul>

                        <li class="user">
                            <asp:Label ID="lblLoginName" runat="server" Text="Label"></asp:Label>
                        </li>
                        <%-- <li class="box4" id="lnkAdm">
                            <%--<a href="../Administration/AdminHome.aspx">--%>
                        <%--Home</a>--%>
                        <%--<asp:LinkButton ID="lnk_home" Font-Underline="False" OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);$('.box').hide();" runat="server" Text="Administration" Font-Overline="False" OnClick="lnk_home_Click1"></asp:LinkButton>--%>
                        <%--</li>--%>

                        <li class="box4" id="Li1">
                            <%--<a href="../Administration/AdminHome.aspx"> OnClientClick="window.document.forms[0].target='_blank'; setTimeout(function(){window.document.forms[0].target='';}, 500);$('.box').hide(); lnk_home_Click1"--%>
                            <%--Home</a>--%><a id="A4" font-underline="False" text="Administration" font-overline="False" onclick="responseload();">Administration</a>
                        </li>

                        <%--<li class="box0">
                            <a id="dashboard" onclick="getdash();">dashboard</a>
                        </li>--%>

                        <li class="timeS">
                            <div>
                                <div style="float: left;">
                                </div>
                                <div style="float: left; padding: 0 20px;" class="jclock"></div>
                            </div>
                        </li>
                        <li class="box1">
                            <a href="#" onclick="ConfirmChange();">
                                    <asp:Label ID="LblClass" runat="server" Text=""></asp:Label>
                                <asp:linkbutton ID="btnDischarge" onclick="btnOpenDischargeDiv" ForeColor="white" width="76px" height="15px" style="padding-left: 1px; padding-bottom: -2px" runat="server" BackColor="#1cae41">Dis-Students</asp:linkbutton>

                            
                            </a>


                            <div class="classDiv">
                                <a id="A2" class="close sprited" href="#" style="margin-top: -13px; margin-right: -14px;">
                                    <img onclick="javascript:studclsSearch();" src="../Administration/images/closebtn.png" style="border: 0px;" width="24px" />
                                </a>
                                <hr />
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtSname" runat="server" CssClass="textClass" Width="170px" value="Student Name" onBlur="if(this.value=='') this.value='Student Name'" onFocus="if(this.value =='Student Name' ) this.value=''"></asp:TextBox>
                                        <asp:ImageButton ID="imgsearch" runat="server" CssClass="searchbtnimg" ImageUrl="img/searchbtn.png" OnClick="imgsearch_Click" />
                                        <br />
                                        <asp:Label ID="LBLClassnotfound" runat="server" ForeColor="Black"></asp:Label>
                                        <div style="height: auto;" id="classDivs">

                                            <div style="width: 100%; margin-top: 10px;" id="DlClass"></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                            <div class="classDivNew" style="overflow-wrap:break-word; overflow-y: scroll; height: 240px !important; width: 255px; overflow-y: scroll;">
                                <a id="A5" class="close sprited" href="#" style="margin-top: -13px; margin-right: -14px;">
                                    <img onclick="javascript:closeDivDsch();" src="../Administration/images/closebtn.png" style="border: 0px; padding-top: 13px" width="24px;" />
                                </a>
                                <hr />
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtSnameDsch" runat="server" CssClass="textClass" Width="170px" value="Choose Discharged Client" onBlur="if(this.value=='') this.value='Choose Discharged Client'" onFocus="if(this.value =='Choose Discharged Client' ) this.value=''"></asp:TextBox>
                                        <asp:ImageButton ID="imgSearchDsch" runat="server" CssClass="searchbtnimg" ImageUrl="img/searchbtn.png" OnClick="imgSearchDsch_Click" />
                                        <br />
                                        <asp:Label ID="LblStudentNotFound" runat="server" ForeColor="Black"></asp:Label>
                                        <div style="height: auto;" id="Div5">

                                            <div style="width: 100%; margin-top: 10px;" id="DlClassDsch"></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                        </li>

                        <li class="box2" id="alertBtn">
                            <a href="#" onclick="AletsRem();">Alerts(
                                <label id="NoOfalarm">0</label>)</a>
                            <div class="alertsPopUp">
                                <hr />
                                <div style="height: 375px; width: 100%">
                                    <div id="containerAlarm" style="height: 375px; overflow-y: auto; width: 100%; margin-top: 10px">
                                        <div id="divWait" style="margin-left: 93px; margin-top: 100px; z-index: 200">
                                            <img src="../Administration/images/32.gif" style="width: 80px; height: 80px" />
                                        </div>
                                    </div>
                                </div>
                                <asp:HiddenField ID="BehaviourTimes" runat="server"></asp:HiddenField>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <div id="Div2" style="width: 310px;">
                                            <a id="A1" class="close sprited" href="#">
                                                <img onclick="javascript: $('.alertsPopUp').slideToggle('slow');" src="../Administration/images/closebtn.png" style="border: 0px;" width="24px" /></a>
                                            <div style="text-align: right; background: #F30;"></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </li>
                        <li class="dashboard">
                            <a id="lnkDashboard" onclick="getDashBoardReport();">Dashboard</a>
                        </li>

                        <li class="box3">
                            <a href="#" onclick="ShowReminder();">CheckIn/CheckOut </a>
                            <div class="checkPopUp">
                                <hr />

                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div id="Div3">
                                            <a id="A3" class="close sprited" href="#">
                                                <img onclick="javascript: $('.checkPopUp').slideToggle('slow');" src="../Administration/images/closebtn.png" style="border: 0px;" width="24px" /></a>
                                            <iframe id="stCheckIn" src="StudentCheckin.aspx" height="375px" scrolling="no" frameborder="0"></iframe>
                                            <div style="text-align: right;">
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="clear"></div>
                            </div>
                        </li>
                        <li class="boxLand">
                            <a href="#" title="StartUp Page" onclick="loadmaster();">Landing Portal</a>

                        </li>

                        <li class="box5">
                            <asp:LinkButton ID="lnk_logout" Font-Underline="False" runat="server" Text="Logout" Font-Overline="False" OnClick="lnk_logout_Click1"></asp:LinkButton>
                        </li>

                        <li>
                            <%--<a id="play">Enable</a>--%>
                            <%--<button id="play">Play</button>--%>
                            <div  class="bell" id="bell" title="Enable alert sound" style="text-align: center; color: white;display:none;">
                            <img id="play" style="margin: 0px 0px 0px 0px!important;width: 20px; height: 20px;" src="../Administration/images/icons8-bell-off.png" />
                                <img id="audioPlay" style="display:none"/>
                            </div>
                    </div>
                        </li>


                    </ul>




                </div>
            </div>
            <script>

                (function () {

                    // Check if the browser supports web audio. Safari wants a prefix.
                    if ('AudioContext' in window || 'webkitAudioContext' in window) {

                        //////////////////////////////////////////////////
                        // Here's the part for just playing an audio file.
                        //////////////////////////////////////////////////
                        var play = function play(audioBuffer) {
                            var source = context.createBufferSource();
                            source.buffer = audioBuffer;
                            source.connect(context.destination);
                            source.start();
                        };

                        var URL = '../StudentBinder/Sounds/mixkit-software-interface-start-2574.wav';
                        var AudioContext = window.AudioContext || window.webkitAudioContext;
                        var context = new AudioContext(); // Make it crossbrowser
                        var gainNode = context.createGain();
                        gainNode.gain.value = 1; // set volume to 100%
                        var playButton = document.querySelector('#play');
                        var audioPlay = document.querySelector('#audioPlay');
                        var yodelBuffer = void 0;

                        // The Promise-based syntax for BaseAudioContext.decodeAudioData() is not supported in Safari(Webkit).
                        window.fetch(URL)
                          .then(response => response.arrayBuffer())
                          .then(arrayBuffer => context.decodeAudioData(arrayBuffer,
                             audioBuffer => {
                                 yodelBuffer = audioBuffer;
                    },
                          error =>
                            console.error(error)
                        ))

                    var isiPad = navigator.userAgent.match(/iPad/i);

                    if (isiPad) {
                        document.getElementById('bell').style.display = "block";
                    }
                    //if((source.playbackState === source.PLAYING_STATE || source.playbackState === source.FINISHED_STATE)) {
                    //if (confirm("Enable alert Sound!")) {
                    //    play(yodelBuffer);
                    //}
                    //}
                    //}

                    playButton.addEventListener('click', enableAllAlerts);

                    playButton.onclick = function () {
                        return play(yodelBuffer);
                    };

                    audioPlay.onclick = function () {
                        return play(yodelBuffer);
                    };

                    // Play the file every 2 seconds. You won't hear it in iOS until the audio context is unlocked.
                    //window.setInterval(function(){
                    //    play(yodelBuffer);
                    //}, 5000);


                    //////////////////////////////////////////////////
                    // Here's the part for unlocking the audio context, probably for iOS only
                    //////////////////////////////////////////////////

                    // From https://paulbakaus.com/tutorials/html5/web-audio-on-ios/
                    // "The only way to unmute the Web Audio context is to call noteOn() right after a user interaction. This can be a click or any of the touch events (AFAIK – I only tested click and touchstart)."
      
                    var unmute = document.getElementById('unmute');
                    var bell = document.getElementById('bell');
                    playButton.addEventListener('click', unlock);
         
                    function unlock() {
                        console.log("unlocking")
                        // create empty buffer and play it
                        var buffer = context.createBuffer(1, 1, 22050);
                        var source = context.createBufferSource();
                        source.buffer = buffer;
                        source.connect(context.destination);

                        // play the file. noteOn is the older version of start()
                        source.start ? source.start(0) : source.noteOn(0);

                        // by checking the play state after some time, we know if we're really unlocked
                        //setTimeout(function() {
                        //    if((source.playbackState === source.PLAYING_STATE || source.playbackState === source.FINISHED_STATE)) {
                        //        document.getElementById("play").src="../Administration/images/icons8-bell-on.png";
                        //        //bell.style.display = "none";
                        //        //document.getElementById("play").disabled=true;
                        //    }
                        //}, 0);
                    }

                    // Try to unlock, so the unmute is hidden when not necessary (in most browsers).
                    unlock();
         
                }
                }
)();
 
 </script>
            <!-- dashboard container panel -->
            <div id="db-container">
                <!-- header -->
                <div id="header-panel">
                    <div class="Dashboard-logo">
                    </div>

                </div>

                <!-- menu panel -->


                <div id="menu-panel" style="margin: 0 auto; width: 96%; position: relative;">
                    <!-- student Menu list -->

                    <br />
                    <div class="clear"></div>

                    <div id="makeMeScrollable" class="upperScroll">
                        <div style="display: block;" class="scrollingHotSpotLeft lfMarg">
                            <img src="../Administration/images/left-arow.PNG" alt="" width="16" height="45" />
                        </div>
                        <div style="display: block;" class="scrollingHotSpotRight img">
                            <img src="../Administration/images/right-arow.PNG" class="png" width="16" height="45" />
                        </div>
                        <div class="clear"></div>
                        <div class="scrollWrapper">
                            <div style="width: 32040px;" class="scrollableArea" id="studentContainer" runat="server">
                            </div>
                        </div>
                    </div>


                    <div width="100%" id="tdMsg" runat="server"></div>



                    <div id="menuWrapper">



                        <div id="menu" runat="server">
                            <ul>
                                <li class='alpha1'><a href='#' id='STUDENT INFO' onclick='selMenu(this);'><span>
                                    <img src='../Administration/images/chideren.PNG' alt='' align='left'>STUDENT&nbsp;INFO</span></a></li>

                                <li class='alpha1'><a href='#' id='ASSESSMENTS' onclick='selMenu(this);'><span>
                                    <img src='../Administration/images/asignment.PNG' alt='' align='left'>ASSESSMENTS</span></a></li>
                                <li class='alpha1'><a href='#' id='IEPS' onclick='selMenu(this);'><span>
                                    <img src='../Administration/images/ieps.PNG' alt='' align='left'>IEPS</span></a></li>
                                <li class='alpha1'><a href='#' id='BSP' onclick='selMenu(this);'><span>
                                    <img src='../Administration/images/BSPForms.PNG' alt='' align='left'>BSP FORMS</span></a></li>
                                <li class='alpha1'><a href='#' id='LESSON PLANS' onclick='selMenu(this);'><span>
                                    <img src='../Administration/images/lessonplan.PNG' alt='' align='left'>LESSON&nbsp;PLANS</span></a></li>
                                <li class='alpha1'><a href='#' id='DATASHEETS' onclick='selMenu(this);'><span>
                                    <img src='../Administration/images/listt.PNG' alt='' align='left'>DATASHEETS</span></a></li>
                                <li class='alpha1'><a href='#' id='BEHAVIOR' onclick='selMenu(this);'><span>
                                    <img src='../Administration/images/behaver.PNG' alt='' align='left'>BEHAVIOR</span></a></li>
                                <li class='alpha1'><a href='#' id='GRAPHS' onclick='selMenu(this);'><span>
                                    <img src='../Administration/images/shaire.PNG' alt='' align='left'>GRAPHS</span></a></li>
                                <li class='alpha1'><a href='#' id='COVERSHEETS' onclick='selMenu(this);'><span>
                                    <img src='../Administration/images/transilate.PNG' alt='' align='left'>COVERSHEETS</span></a></li>
                                <li class='alpha1'><a href='#' id='EDIT' onclick='selMenu(this);'><span>
                                    <img src='../Administration/images/edit-icon.PNG' alt='' align='left'>EDIT</span></a></li>
                            </ul>
                        </div>




                        <div id="submenu">
                            <div id="Div1" class="lowerScroll">
                                <div style="display: block;" class="scrollingHotSpotLeft">
                                    <img width="21px" height="31px" src="../Administration/images/lfarro_03.PNG" onclick="goLeft();">
                                </div>
                                <div style="display: block;" class="scrollingHotSpotRight" onclick="goRight();">
                                    <img width="21px" height="31px" src="../Administration/images/lfarro_04.PNG" />
                                </div>

                                <div class="scrollWrapper second">
                                    <div style="width: 3204px;" class="scrollableArea">
                                        <ul id="ulsubmenu"></ul>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                    <!--------------------------------------- Droper Start------------------------------------------->

                    <div id="dropper" class="bottomtop" onclick="droper();">

                        <div class="clear"></div>
                        <!---------------------------------------------------------- LFT Slider Menu Start-------------------------------------------------------------->



                        <!-----------------------------------------------------------Slidmenu END----------------------------------------------------------------------->


                    </div>
                    <!--------------------------------------- Droper End------------------------------------------->
                    <script>
                        function show_hide_timer(elm) {
                            $('#stdTimerFrame').slideToggle('fast');

                            $('.notifyBox').html('0');
                            $('.notifyBox').hide();
                        }
                    </script>
                    <%--<div class="classDiv">
                                <%--<a id="A5" class="close sprited" href="#" style="margin-top: -63px; margin-right: -64px;">
                                    <img onclick="javascript:studclsSearch();" src="../Administration/images/closebtn.png" style="border: 0px;" width="24px" />
                                </a>--%>
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>--%>
                    <%--<asp:TextBox ID="TextBox1" runat="server" CssClass="textClass" Width="170px" value="Student Name" onBlur="if(this.value=='') this.value='Student Name'" onFocus="if(this.value =='Student Name' ) this.value=''"></asp:TextBox>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CssClass="searchbtnimg" ImageUrl="img/searchbtn.png" OnClick="imgsearch_Click" />
                                        <br />
                                        <asp:Label ID="Label11" runat="server" ForeColor="Black"></asp:Label>
                                        <div style="height: auto;" id="Div6">

                                            <div style="width: 100%; margin-top: 10px;" id="Div7"></div>
                                        </div>--%>
                    <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>

                    <%--</div>--%>

                    <div id="newBehTop" runat="server" style="width: 318px; z-index: 1001; height: auto; position: absolute; right: 0px; background-color: #60c658; border: 1px solid #0D668E; margin-top: 23px; cursor: pointer;">
                        <div class="bhvOpt" title="Selected Student">
                            <img id="bhvSelImg" src="../Administration/images/Client.gif" style="width: 30px; height: 20px;" /></div>
                        <div class="bhvOpt" title="List Students" style="text-align: center; color: white;" onclick="javascript:$('#newBehStdList').fadeToggle();">
                            <img style="width: 20px; height: 20px;" id="ImgListStudents" src="../Administration/images/StudentsList.png" /></div>
                        <div class="bhvOpt" id="bhvTitle" style="width: 156px; text-align: center; background-color: #0d668e; color: white; font-size: 24px;">Behaviors</div>
                        <div class="bhvOpt" title="Current Student" style="text-align: center; color: white;" onclick="getSelStudBehav();">
                            <img style="width: 18px; height: 18px;" id="Img1" src="../Administration/images/SelectedStudent.png" /></div>
                        <div class="bhvOpt" title="Open/Close Behavior Panel" style="text-align: center; color: white;" onclick="javascript:$('#newBehFrame').fadeToggle();">
                            <img style="width: 20px; height: 20px;" id="Img2" src="../Administration/images/ShowHideDSTimer.png" /></div>
                    </div>
                    <div id="newBehStdList" runat="server" style="width: 316px; display: none; z-index: 1001; height: auto; position: absolute; right: 0px; background-color: white; border: 1px solid #0D668E; margin-top: 57px; cursor: pointer;"></div>

                    <div id="newBehFrame" runat="server" style="width: 316px; z-index: 1000; height: auto; right: 0px; position: absolute; background-color: white; border: 2px solid #0D668E; margin-top: 57px; cursor: pointer;"></div>

                    <div id="timerDiv" style="width: 270px; height: auto; position: absolute; right: 0; z-index: 1002; background-color: white; border: 2px solid #0D668E; margin-top: -15px; cursor: pointer;">
                        <div class="notifyBox">0</div>
                        <div class="stdTimerButton" onclick="show_hide_timer(this)" style="text-align: center; padding: 3px; background-color: #0D668E; color: white;">Timers</div>
                        <iframe id="stdTimerFrame" src="userTimers_ipad.aspx" style="width: 270px; height: 300px; border: 0px solid; display: none; overflow-y: scroll; position: relative; float: left;"></iframe>
                    </div>
                    <div id="contactFormContainer">
                        <div id="contactForm">
                            <ul id="ulStudents"></ul>
                        </div>
                        <div class="contact">
                            <div class="notifier"></div>
                        </div>

                    </div>

                </div>
            </div>
        </div>


        <!-- content panel -->
        <div id="dashboard-content-panel">
            <!-- content Left side -->
            <%--<div class="slideTogglebox" style="display: none" onMouseOut="">

                            <div id="student-LHS">
                                <div id="ulWrapper">
                                    <ul id="ulStudents">
                                    </ul>
                                </div>
                            </div>
                        </div>--%>

            <!-- content right side -->
            <div class="clear"></div>
            <div id="dashboard-RHS">

                <!--------------------------------------------------------------------Data Sheet START-------------------------------------------------------------------------->

                <%-- <span class="dashboard-RHS-top"></span>--%>
                <div class="dashboard-RHS-content">
                 <h2 id="CGLLoadingImage" style="font-family: Calibri, sans-serif; color:#6C7598; font-size: 20px; margin-top: 15px; text-align: center; display:none;">Loading......</h2>
                    <h2 id="tdTitle" style="font-family: Calibri, sans-serif; color: #6b6b6b; font-size: 20px; margin-top: 15px;"><!--//[2019-02-25] disable DashBoard//Welcome to Dashboard Area--></h2>
                    <asp:HiddenField ID="graphdivClose" runat="server" value="0"/>
                    <hr />
                    <div class="content-panel">
                        <div id="Div4" style="text-align: right;">

                            <table style="width: 100%;">
                                <tr>
                                    <td align="left" style="width: 90%;"></td>

                                    <td style="width: 10%;">
                                        <div style="display: none">
                                            <div id="IEPEdit" style="text-align: right; display: none;">
                                                <img id="IEPDocumentaion.aspx" alt="" src="../Administration/images/EditIEPP2.png" style="width: 30px; height: 30px;" onclick="EditIEP('1');" />
                                                <%-- <asp:ImageButton ID="btn_edit" runat="server" AlternateText="Edit " Text="Edit Individualized Education Program" Width="30px" BackColor="#33CC33" ForeColor="White" ImageUrl="~/Administration/images/EditIEPP2.png" Font-Bold="True" onclick="btn_edit_Click" />--%>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%-- <div id="divStatus" style="width: 100%; height: auto; display:block;">
                            </div>--%>
                        </div>

                        <%--  <div class="fillDSLessons">
                            <div class="filterOption">
                                <span style="cursor:pointer;font-style:italic;color:blue; text-decoration:underline;" onclick="ListLessonPopup('Inactive');">Inactive</span>|
                                <span style="cursor:pointer;font-style:italic;color:blue; text-decoration:underline;" onclick="ListLessonPopup('Maintenance');">Maintenance</span>|
                                <span style="cursor:pointer;font-style:italic;color:blue; text-decoration:underline;" onclick="ListLessonPopup('Approve');">Approve</span>
                                    </div>--%>
                        <div class="fillLessons" style="margin: auto; width: 637px; display: none;">
                           <table style="width: 100%">
                            <tr>
                            <td colspan="3">
                            <div>
                                <%--class="filterOption"--%>                              
                                    
                                        <span class="Schedule_View" style="color: grey;cursor: pointer;font-size:13px;" onclick="PopupLessonPlans2('Schedule_View');">Schedule View</span> |
                                        <%--<td | </td>--%>
                                        <span class="List_View"style="cursor: pointer;color: grey;font-size:13px;" onclick="datasheetFuntion();">List View</span>
                                
                                        <%--<td style="width: 20%">List View</td>--%>
                                     
                                       
                            </div>
                                </td>
                                </tr>
                               <tr></tr>
                                </table>
                            <div class="lessonSearchOptions">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 20%">Lesson Plan Name</td>
                                        <td style="width: 25%">
                                            <input type="text" id="txtLesson" /></td>
                                        <td style="width: 5%"></td>
                                        <td>
                                            <input type="button" onclick="SearchLessonPopup1();" value="Search" class="NFButton" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div class="filterOption">
                                                <div style="float: right;margin-right:60px;">
                                             
                                                <span class="filterOption_main" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('Maintenance');">MAINTENANCE</span>|
                                                <span class="filterOption_appr" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('Approve');">ACTIVE</span>|
                                                <span class="filterOption_rd" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('rd');">ALL</span>|
                                                 <span class="filterOption_all" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('all');">Reset</span>
                                                </div>
                                                <div style="float: left;">
                                                    <input id="chk_day_ds" type="checkbox" checked="checked" /><span>DAY</span>
                                                    <input id="chk_res_ds" type="checkbox" checked="checked" /><span>RESIDENCE</span>
                                                    <script>
                                                        // FUNCTION TO CHECK WHETHER BOTH THE CHECKBOXES ARE UNCHECKED. IF SO DISPLAY A MESSAGE
                                                        function checkValid_ds() {
                                                            var dayFlag = $('#chk_day_ds').is(":checked");
                                                            var resFlag = $('#chk_res_ds').is(":checked");

                                                            if (dayFlag == false && resFlag == false) {
                                                                alert('Please select atleast one option.');
                                                                return false;
                                                            }

                                                            return true;
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="lessonDSDetails"></div>
                            </div>
                        </div>
                                                <div class="fillLessons2" style="margin: auto; width: 637px; display: none;">
                            <div>
                                <table style="width: 100%">
                                    <tr>
                                        <%--<td style="width: 20%">Lesson Plan Name1</td>--%>
                                        <td style="width: 25%">
<%--                                            <input type="text" id="Text1" /></td>--%>
                                             <h3><p id="date" style="margin-left:210px;color:darkblue;font-family:Calibri;font-size:20px;font-weight:bold;"></p></h3>
                                           <script>
                                               var n = new Date();
                                               var weekday = new Array(7);
                                               weekday[0] = "Sunday";
                                               weekday[1] = "Monday";
                                               weekday[2] = "Tuesday";
                                               weekday[3] = "Wednesday";
                                               weekday[4] = "Thursday";
                                               weekday[5] = "Friday";
                                               weekday[6] = "Saturday";

                                               var w = weekday[n.getDay()];
                                               //n = new Date();
                                               //d = n
                                               y = n.getFullYear();
                                               m = n.getMonth() + 1;
                                               d = n.getDate();
                                               document.getElementById("date").innerHTML = w + " " + m + "/" + d + "/" + y;
                                               //document.getElementById("date").innerHTML = Date();
                                                   </script>
                                        <td style="width: 5%"></td>
                                        <%--<td>
                                            <input type="button" onclick="SearchLessonPopup1();" value="Search" class="NFButton" />

                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <table style="width: 100%">
                            <tr>
                            <td colspan="3">
                            <div>
                                <%--class="filterOption"--%>                              
                                    
                                        <span class="Schedule_View" style="color: grey;cursor: pointer;font-size:13px;" onclick="PopupLessonPlans2('Schedule_View');">Schedule View</span> |
                                        <%--<td | </td>--%>
                                        <span class="List_View"style="cursor: pointer;color: grey;font-size:13px;" onclick="datasheetFuntion();">List View</span>
                               
                                                                               <%--<td style="width: 20%">List View</td>--%>
                                     
                                       
                            </div>
                                </td>
                                </tr>
                                </table>
                                        <%--<td colspan="4">
                                            <div class="filterOption">
                                                <div style="float: right;margin-right:60px;">
                                             
                                                <span class="filterOption_main" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('Maintenance');">MAINTENANCE</span>|
                                                <span class="filterOption_appr" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('Approve');">ACTIVE</span>|
                                                <span class="filterOption_rd" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('rd');">ALL</span>|
                                                 <span class="filterOption_all" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('all');">Reset</span>
                                                </div>
                                                <div style="float: left;">
                                                    <input id="Checkbox1" type="checkbox" checked="checked" /><span>DAY</span>
                                                    <input id="Checkbox2" type="checkbox" checked="checked" /><span>RESIDENCE</span>
                                                    <script>
                                                        // FUNCTION TO CHECK WHETHER BOTH THE CHECKBOXES ARE UNCHECKED. IF SO DISPLAY A MESSAGE
                                                        function checkValid_ds() {
                                                            var dayFlag = $('#chk_day_ds').is(":checked");
                                                            var resFlag = $('#chk_res_ds').is(":checked");

                                                            if (dayFlag == false && resFlag == false) {
                                                                alert('Please select atleast one option.');
                                                                return false;
                                                            }

                                                            return true;
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </td>--%>
                                    </tr>
                                </table>
                                <div class="lessonDSDetails"></div>
                            </div>
                        </div>

                        <div class="fillLessons1" style="margin: auto; width: 637px; display: none;">
                            <div class="lessonSearchOptions">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 20%">Lesson Plan Name</td>
                                        <td style="width: 25%">
                                            <input type="text" id="txtLessonName" /></td>
                                        <td style="width: 5%"></td>
                                        <td>
                                            <input type="button" onclick="SearchLessonPopup();" value="Search" class="NFButton" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div class="filterOption">
                                                <div style="float: right;">
                                                    <span class="filterOption_inac" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('Inactive');">INACTIVE</span>|
                                                <span class="filterOption_main" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('Maintenance');">MAINTENANCE</span>|
                                                <span class="filterOption_appr" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('Approve');">ACTIVE</span>|
                                                <span class="filterOption_rd" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('rd');">ALL</span>|
                                                <span class="filterOption_all" style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('All');">Reset</span>
                                                </div>
                                                <div style="float: left;">
                                                    <input id="chk_day" type="checkbox" checked="checked" /><span>DAY</span>
                                                    <input id="chk_res" type="checkbox" checked="checked" /><span>RESIDENCE</span>
                                                    <script>
                                                        // FUNCTION TO CHECK WHETHER BOTH THE CHECKBOXES ARE UNCHECKED. IF SO DISPLAY A MESSAGE
                                                        function checkValid() {
                                                            var dayFlag = $('#chk_day').is(":checked");
                                                            var resFlag = $('#chk_res').is(":checked");

                                                            if (dayFlag == false && resFlag == false) {
                                                                alert('Please select atleast one option.');
                                                                return false;
                                                            }

                                                            return true;
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="lessonListDetails"></div>
                        </div>
                        <div runat="server" id="divContentPages"></div>

                        <div id="dash_area"></div>
                        <iframe id="ifContent" width="100%" frameborder="0" onload='javascript:resizeIframe(this);'></iframe>

                    </div>

                </div>




                <!--------------------------------------------------------------------Data Sheet END-------------------------------------------------------------------------->



                <%--  <div id="footer-panel">
           <%-- <table style="width:100%"><tr><td style="text-align:left"><img src="../Administration/images/student-logo.jpg" alt=""></td>
                <td style="text-align:right">COPYRIGHT &copy; 2012 Melmark.org, All rights reserveded</td></tr></table>
            
            

        </div>--%>
                <div class="dashboard-RHS-bottom"></div>


            </div>
        </div>


        <!-- footer -->

        <div class="clear"></div>
        <div id="footer-panel" style="">
            <table style="width: 100%">
                <tr>
                    <td style="text-align: left;">
                        <img src="../Administration/images/student-logo.jpg" width="100px" height="16px;" style="padding: 5px;" alt="" /></td>
                    <td style="text-align: right">&copy; Copyright 2015, Melmark, Inc. All rights reserved.</td>
                </tr>
            </table>
        </div>

        <div id="overlay" class="web_dialog_overlay">
        </div>

        <div id="dialog" class="web_dialog" style="left: 30%">

            <div id="sign_up5">
                <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px; height: 25px; width: 25px;" onclick="ClosePopup()">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="25" height="25" alt="" /></a>
                <h3>Lesson Plans</h3>
                <hr />
                <div style="height: 10px;"></div>
                <div>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 20%">Lesson Plan Name</td>
                            <td style="width: 25%">
                                <input type="text" id="txtLessonName1" /></td>
                            <td style="width: 5%"></td>
                            <td>
                                <input type="button" onclick="SearchLessonPopup();" value="Search" class="NFButton" />

                                <span style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('LessonPlanTab_inactive');">INACTIVE</span>/
                                <span style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('LessonPlanTab_maintenance');">MAINTENANCE</span>/
                                <span style="cursor: pointer; font-style: italic; color: blue; text-decoration: underline;" onclick="ListLessonPopup('LessonPlanTab_approve');">ACTIVE</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 5px;"></div>
                <div id="LessonPlanDetails" style="height: 200px; overflow-y: auto;"></div>
                <input type="hidden" id="PopupType" />
            </div>
        </div>


    </form>

</body>
<script>
      
</script>
</html>
