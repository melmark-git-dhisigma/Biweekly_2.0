<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="StudentBinder_Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <script src="jqPlot/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="jqPlot/jquery.jqplot.js" type="text/javascript"></script>
    <link href="jqPlot/jquery.jqplot.css" rel="stylesheet" />
    <script src="jqPlot/plugins/jqplot.barRenderer.js"></script>
    <script src="jqPlot/plugins/jqplot.categoryAxisRenderer.js"></script>
    <script src="jqPlot/plugins/jqplot.pointLabels.js"></script>

    <script type="text/javascript" src="jqPlot/plugins/jqplot.dateAxisRenderer.min.js"></script>
    <script type="text/javascript" src="jqPlot/plugins/jqplot.canvasTextRenderer.min.js"></script>
    <script type="text/javascript" src="jqPlot/plugins/jqplot.canvasAxisTickRenderer.min.js"></script>
    <script type="text/javascript" src="jqPlot/plugins/jqplot.categoryAxisRenderer.min.js"></script>
    <script type="text/javascript" src="jqPlot/plugins/jqplot.barRenderer.min.js"></script>
    <script type="text/javascript" src="jqPlot/plugins/jqplot.pieRenderer.min.js"></script>
    <script type="text/javascript" src="jqPlot/plugins/jqplot.donutRenderer.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var s1 = new Array();
            var s2 = new Array();
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/getIEPExpGraph",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //alert(result.d.Length);
                    for (var i = 0; i < 4; i++) {
                        s1[i] = result.d[i][1];
                        s2[i] = result.d[i][0];
                    }
                    drawIEPExpiryGraph(s1, s2);
                },
                error: function () {
                }
            });
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/getLPStatus",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //alert(result.d.Length);
                    if ((result.d.length > 0) && (result.d != null)) {
                        drawLPStat(result.d);
                    }
                },
                error: function () {
                }
            });
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/getIEPStatus",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //alert(result.d.Length);
                    if ((result.d.length > 0) && (result.d != null)) {
                        drawIEPStat(result.d);
                    }
                },
                error: function () {
                }
            });
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/getLPSchedule",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //alert(result.d.Length);
                    if ((result.d.length > 0) && (result.d != null)) {
                        drawLPSched(result.d);
                    }
                },
                error: function () {
                }
            });

            function drawLPSched(data) {
                var maxvalue = Math.max.apply(Math, data[1]);
                var max = (parseInt((maxvalue / 10), 10) * 10) + 10;
                var intrvl = parseInt((max / 5), 10);
                
                var actual = data[2];
                var total = data[1];
                var ticks = data[0];
                if ((actual.length > 0) && (total.length > 0) && (ticks.length > 0)) {
                    var plot2 = $.jqplot('LPSched', [total, actual], {
                        title: 'LessonPlan Scheduling Report',
                        series: [{ renderer: $.jqplot.BarRenderer }, { xaxis: 'xaxis', yaxis: 'yaxis' }],
                        seriesDefaults: {
                            renderer: $.jqplot.BarRenderer,
                            rendererOptions: {
                                // Put a 30 pixel margin between bars.
                                barMargin: 20,
                                // Highlight bars when mouse button pressed.
                                // Disables default highlighting on mouse over.
                                highlightMouseDown: true,
                                fillToZero: true
                            },
                            pointLabels: { show: false }
                        },
                        axesDefaults: {
                            tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                            tickOptions: {

                            }
                        },
                        legend: {
                            show: true,
                            placement: 'outside'
                        },
                        series: [
                        { label: 'Scheduled LP' },
                        { label: 'Completed LP' }
                        ],
                        axes: {
                            xaxis: {
                                renderer: $.jqplot.CategoryAxisRenderer,
                                ticks: ticks,
                                tickOptions: {
                                    angle: 30
                                },
                                label: 'Students'
                            },

                            yaxis: {
                                padMin: 0,
                                pad: 0,
                                show: true,
                                min: 0,
                                max: max,
                                tickInterval: intrvl,
                                label: 'LP Count'
                            }
                        }
                    });
                }

            }
            function drawIEPStat(data) {

                var plot1 = jQuery.jqplot('IEPStat', [data],
                  {
                      title: 'IEP Status',
                      seriesDefaults: {
                          // Make this a pie chart.
                          renderer: jQuery.jqplot.PieRenderer,
                          rendererOptions: {
                              // Put data labels on the pie slices.
                              // By default, labels show the percentage of the slice.
                              showDataLabels: true
                          }
                      },
                      legend: { show: true, location: 'e' }
                  }
                );
            }
            function drawLPStat(data) {

                var plot1 = jQuery.jqplot('LPStat', [data],
                  {
                      title: 'LessonPlan Status',
                      seriesDefaults: {
                          // Make this a pie chart.
                          renderer: jQuery.jqplot.PieRenderer,
                          rendererOptions: {
                              // Put data labels on the pie slices.
                              // By default, labels show the percentage of the slice.
                              showDataLabels: true
                          }
                      },
                      legend: { show: true, location: 'e' }
                  }
                );
            }
            function drawIEPExpiryGraph(counts, dur) {
                //var s1 = [2, 6, 7, 10];
                //var s2 = [7, 5, 3, 4];
                //var s3 = [14, 9, 3, 8];
                var maxvalue = Math.max.apply(Math, counts);
                var max = (parseInt((maxvalue / 10), 10) * 10) + 10;
                var intrvl = parseInt((max / 5), 10);
                var ticks = dur;
                plot3 = $.jqplot('IEPGraph', [counts], {
                    // Tell the plot to stack the bars.
                    
                    captureRightClick: true,
                    title: 'IEP Expiring Report',
                    seriesDefaults: {
                        renderer: $.jqplot.BarRenderer,
                        rendererOptions: {
                            // Put a 30 pixel margin between bars.
                            barMargin: 35,
                            // Highlight bars when mouse button pressed.
                            // Disables default highlighting on mouse over.
                            highlightMouseDown: true,
                            fillToZero: true
                        },
                        pointLabels: { show: true }
                    },
                    axes: {
                        xaxis: {
                            renderer: $.jqplot.CategoryAxisRenderer,
                            ticks: ticks,
                            label:'Duration'
                        },
                        yaxis: {
                            // Don't pad out the bottom of the data range.  By default,
                            // axes scaled as if data extended 10% above and below the
                            // actual range to prevent data points right on grid boundaries.
                            // Don't want to do that here.
                            pad: 1.05,
                            padMin: 0,
                            show: true,
                            min: 0,
                            max: max,
                            tickInterval: intrvl,
                            tickOptions: { formatString: '%d' },
                            label:'IEP'
                        }
                    },
                    series: [
                            { label: 'IEP' }
                    ],
                    legend: {
                        show: true,
                        location: 'e',
                        placement: 'outside',
                    }
                });

            }
        });

        

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width:95%;" align="center">
            <table style="width: 60%;">
                <tr style="border-right:1px dashed gray;">
                    <td align="center" style="vertical-align:top;padding-top:50px;">
                        <div id="IEPGraph" style="height: 280px; width: 400px;margin-left:-40px;"></div>
                    </td>
                    <td align="center" style="vertical-align:top;padding-top:50px;">
                        <div id="LPStat" style="height: 250px; width: 360px;margin-left:60px;"></div>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="vertical-align:top;padding-top:50px;">
                        <div id="IEPStat" style="height: 250px; width: 360px;"></div>
                    </td>
                    <td align="center" style="vertical-align:top;padding-top:50px;">
                        <div id="LPSched" style="height: 310px; width: 410px;"></div>
                    </td>
                </tr>
            </table>


        </div>
    </form>
</body>
</html>
