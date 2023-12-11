<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewClinicalClient.aspx.cs" Inherits="StudentBinder_ViewClinicalClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        Array.prototype.includes = null;
        Array.prototype.indexOf = null;
        if (!Array.prototype.includes) {
            Array.prototype.includes = function (search) {
                return !!~this.indexOf(search);
            }
        }
        if (!Array.prototype.indexOf) {
            Array.prototype.indexOf = (function (Object, max, min) {"use strict";
                return function indexOf(member, fromIndex) {
                    if (this === null || this === undefined) throw TypeError("Array.prototype.indexOf called on null or undefined");
                    var that = Object(this), Len = that.length >>> 0, i = min(fromIndex | 0, Len);
                    if (i < 0) i = max(0, Len + i); else if (i >= Len) return -1;
                    if (member === void 0) {
                        for (; i !== Len; ++i) if (that[i] === void 0 && i in that) return i;
                    } else if (member !== member) {
                        for (; i !== Len; ++i) if (that[i] !== that[i]) return i;
                    } else for (; i !== Len; ++i) if (that[i] === member) return i;
                    return -1;
                };
            })(Object, Math.max, Math.min);
        }
    </script>
 <script src="../Administration/JS/jquery.min.js"></script>
    <script src="../Scripts/highcharts/7.1.2/highcharts.js"></script>
   <script src="../Scripts/highcharts/7.1.2/modules/accessibility.js"></script>
    <style type="text/css">
        .popup {
    display: none;
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.7);
}

.popup-content {
    background-color: white;
    position: absolute;
    top: 3%;
    left: 50%;
    transform: translate(-50%, -50%);
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 5px;
    text-align: center;
}

.close {
    position: absolute;
    top: 0;
    right: 0;
    padding: 10px;
    cursor: pointer;
}
</style>
    <script type="text/javascript">
        function showPopup() {
            $('#graphPopup').hide();
            $('#overlay').hide();
            var hchart = document.getElementById('<%= container.ClientID %>');
            hchart.style.display = 'none';
            var popup = document.getElementById('popup');
            popup.style.display = 'block';
        }

        function closePopup() {
            var popup = document.getElementById('popup');
            popup.style.display = 'none';
            var hchart = document.getElementById('<%= container.ClientID %>');
             hchart.style.display = 'block';
             $('#graphPopup').hide();
             $('#overlay').hide();
         }
        </script>
</head>
<body>
    <form id="form1" runat="server">
         <div id="popup" class="popup" >
    <div class="popup-content" id="popup-content">
        <p>Please wait...</p>
    </div>
</div>
        <div>
    <asp:Label ID="lblData" runat="server" Text=""></asp:Label>
            </div>
     <div id = "container" style = "width: 1116.899603148px; height: 578.268px; margin: 0 auto" runat="server"></div>
        <script type="text/javascript">
            showPopup();
            var cid = "<%=cid %>";
            var sid = "<%=sid %>";
            var jsonData = JSON.stringify({ cid: cid, sid: sid});
            $.ajax({
                type: "POST",
                url: "ViewClinicalClient.aspx/getAClientClinic",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                success: function (response) {
                    var parsed = JSON.parse(response.d);
                    var aData = parsed;
                    var behav=[];
                    var categoriesdata = [];
                    var seriesdata = [];
                    var result = [];
                    $.map(aData, function (item, index) {
                        categoriesdata.push(item['StudentName']);
                    });
                    $.map(categoriesdata, function (item, index) {
                        if (!result.includes(item)) {
                            result.push(item);
                        }

                    });
                    var len = result.length;
                    $.map(aData, function (item, index) {
                        if (result.includes(item['StudentName'])) {
                            var index;
                            result.some(function (elem, inx) {
                                if (elem === item['StudentName']) {
                                    index = inx;
                                }
                            });
                            var dt = [];


                            var i = 0;
                            while (i < len) {
                                if (i == index) {

                                    dt.push({ y: item['BehaviourSession'], name: item['BehaviourName'] });
                                }
                                else {
                                    dt.push({ y: 0, name: '' });
                                }


                                i = i + 1;

                            }
                        
                            var daa = JSON.parse(JSON.stringify(dt));
                            var seriesdict = new Object;
                            var ser = new Object;
                            seriesdict['name'] = item['BehaviourName'];                         
                            seriesdict['data'] = daa;
                            seriesdata.push(seriesdict);
                            behav.push(item['BehaviorNameToolTip']);

                            while (dt.length > 0) {
                                dt.pop();
                            }
                        }


                    });
                    var jsonArray = JSON.parse(JSON.stringify(result));

                    var dat = JSON.parse(JSON.stringify(seriesdata));
                    var beh = JSON.parse(JSON.stringify(behav));
                    drawChart(jsonArray, dat, beh);
                    closePopup();
                },
                error: OnErrorCall_
            });


            function OnErrorCall_(response) {

                alert("Whoops something went wrong!");

            }
            function drawChart(cat, da,be) {

                Highcharts.chart('container', {
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: 'Clinical by client',
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        categories: cat,
                        title: {
                            text: 'Client Name',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'

                            }
                        },
                        labels: {
                            enabled: true,
                            useHTML: true,
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            }
                        },
                    },
                    yAxis: {
                        min: 0,
                        tickInterval: 1,
                        title: {
                            text: 'Total Behavior Count',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'

                            }
                        },
                        labels: {
                            enabled: true,
                            useHTML: true,
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            }
                        },
                    },

                    legend: {
                        reversed: true,
                        enabled: false,
                    },
                    plotOptions: {
                        series: {
                            stacking: 'normal'
                        },
                        bar: {
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    return this.point.name;
                                },
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '12px',
                                    fontFamily: 'Arial',
                                    color: 'black',
                                }
                            }
                        },
                    },
                    tooltip: {
                        formatter: function () {
                            var serieI = this.series.index;
                            return be[serieI];
                        }

                    },
                    
                    series: da

                });

            }
      </script>
    </form>
</body>
</html>
