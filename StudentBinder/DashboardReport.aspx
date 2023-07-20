<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardReport.aspx.cs" Inherits="Graph" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>


    <script src="js/jquery-2.1.0.js"></script>
    <script src="js/d3.v3.js"></script>
    <script src="js/nv.d3.js"></script>
    <link href="../Administration/CSS/GraphStyle.css" rel="stylesheet" />
    <link href="CSS/nv.d3.css" rel="stylesheet" />

    <style type="text/css">
        .nv-controlsWrap {
            visibility: hidden;
        }

        .nv-legendWrap {
            visibility: hidden;
        }

        .nvtooltip {
            left: 100px !important;
            position: fixed !important;
        }
    </style>


    <script type="text/javascript">

        window.onload = function () {


            var d = new Date();
            var strDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
            $('#lblDates').html(strDate);
            $('#lblDate').html(strDate);

        };
        $(function () {

            drawGraph();
             drawGraphs();
            var isiPad = navigator.userAgent.match(/iPad/i);
            if (isiPad != null) {
                $(".Head").attr("style", "padding-left: 25%")
            }

            $(".nv-controlsWrap").hide();

        });

        function drawGraph() {
            var data =<%= JsonSData %>;
                    var actualDataless=[];
                    var actualDataBehav=[];
                    actualDataless.push(data[0]);
                    actualDataBehav.push(data[1]);
                    nv.addGraph(function () {
                        var chart = nv.models.multiBarHorizontalChart()
                            .x(function (d) { return d.label })
                            .y(function (d) { return d.value })
                            .margin({ top: 30, right: 20, bottom: 50, left: 175 })
                            .showValues(true)           //Show bar value next to each bar.
                            .tooltips(false)
                        //Show tooltips on hover.
                        //.transitionDuration(350)
                        //.showControls(true);        //Allow user to switch between "Grouped" and "Stacked" mode.

                        chart.yAxis
                            .tickFormat(d3.format(',.2f'));

                        d3.select('#Student svg')
                            .datum(actualDataless)
                            .call(chart);

                        nv.utils.windowResize(chart.update);

                        return chart;
                  
                    });
                    $('#StudentBehv').css('height',$('#Student').css('height'));
                    nv.addGraph(function () {
                        var chart2 = nv.models.multiBarHorizontalChart()
                            .x(function (d) { return d.label })
                            .y(function (d) { return d.value })
                            .margin({ top: 30, right: 20, bottom: 50, left: 175 })
                            .showValues(true)           //Show bar value next to each bar.
                            .tooltips(false)
                        //Show tooltips on hover.
                        //.transitionDuration(350)
                        //.showControls(true);        //Allow user to switch between "Grouped" and "Stacked" mode.

                        chart2.yAxis
                            .tickFormat(d3.format(',.2f'));

                        d3.select('#StudentBehv svg')
                            .datum(actualDataBehav)
                            .call(chart2);

                        nv.utils.windowResize(chart2.update);

                        return chart2;
                    });

               
               
                }
        function drawGraphs() {
            var data =<%= JsonTData %>

               nv.addGraph(function () {
                   var chart = nv.models.multiBarHorizontalChart()
                       .x(function (d) { return d.label })
                       .y(function (d) { return d.value })
                       .margin({ top: 30, right: 20, bottom: 50, left: 175 })
                       .showValues(true)           //Show bar value next to each bar.
                      .tooltips(false)             //Show tooltips on hover.
                   //.transitionDuration(350)
                   //.showControls(true);        //Allow user to switch between "Grouped" and "Stacked" mode.

                   chart.yAxis
                       .tickFormat(d3.format(',.2f'));

                   d3.select('#Teacher svg')
                       .datum(data)
                       .call(chart);

                   nv.utils.windowResize(chart.update);

                   return chart;
               });
        }
        function setSGraph() {
            $('.mainDivS').slideDown();
            $('.mainDivU').slideUp();

            $('#liBeh').html("<div class='session1'></div> Behavior Count ");
            $('#liLess').html(" <div class='session2'></div> Lesson Plans % ");

        }
        function setUGraph() {

            var d = new Date();
            var strDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();


            $('.mainDivS').slideUp();
            $('.mainDivU').slideDown();
            $(".mainDivU").attr("style", "visibility: visible");
            //   $('.mainDivU').attr("visibility", "visible");
            $('#divHeading').html("<font color='#1F77B4'>Number of Teaching Sessions  on " + strDate + " </font>");

            $('#liBehav').html("<div class='session1'></div> Behavior Count ");
            $('#liLessons').html("<div class='session2'></div>Lesson Plan Count ");
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="Head" style="position: absolute; width: 300px; height: 30px; left: 44%; top: 3%;">
            <a class="student" href="#" onclick="setSGraph();">Teaching Progress</a>
            <a class="user" href="#" onclick="setUGraph();">Teaching Sessions</a>
        </div>

        <div class="mainDivS">
            <div class="Head" style="float: none; font-size: 29px; margin-left: 35%; color: #1F77B4;">
                Teaching Progress for Student on
                <label id="lblDate"></label>
            </div>
            <div class="headings" style="width:100%;float:left;">

                <ul style="float: right">
                    <li id="liBeh">
                        <div class="session1"></div>
                        Behavior Count
                        
                    </li>
                    <li id="liLess">
                        <div class="session2"></div>
                        Lesson Plans %  
                       
                    </li>
                </ul>
            </div>

            <div style="width: 45%;display:inline-block; float:left" id="Student" runat="server">
                <svg></svg>
            </div>
            <div  style="width: 45%; display:inline-block; float:left;margin-left:1%" id="StudentBehv" runat="server">
                <svg></svg>
            </div>


        </div>


        <div class="mainDivU" style="visibility: hidden;">
            <div id="divHeading" class="Head" style="float: none; font-size: 29px; margin-left: 35%; color: #1F77B4;">
                User Reports on
                <label id="lblDates"></label>
            </div>
            <div class="headings">

                <ul style="float: right">
                    <li id="liBehav">Behavior<div class="session1"></div>
                    </li>
                    <li id="liLessons">Lesson Plans<div class="session2"></div>
                    </li>
                </ul>
            </div>

            <div style="width: 99%;" id="Teacher" runat="server">
                <svg></svg>
            </div>

        </div>

    </form>
</body>
</html>
