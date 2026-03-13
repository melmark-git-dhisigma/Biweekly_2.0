<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TimeCycleChart.aspx.cs" Inherits="StudentBinder_TimeCycleChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="../Administration/CSS/jsDatePickforGraph.css" rel="stylesheet" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js"></script>
    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <script src="../Administration/JS/jquery.min.js"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js"></script>
      <script src="../Scripts/highcharts/7.1.2/highcharts.js"></script>
   <script src="../Scripts/highcharts/7.1.2/modules/accessibility.js"></script>
    <script src="../Scripts/highcharts/7.1.2/modules/xrange.js"></script>
    <script src="../Scripts/highcharts/7.1.2/modules/exporting.js"></script>
<%--    <script src="../Scripts/highcharts/7.1.2/modules/export-data.js"></script>--%>
     <script src="../Scripts/highcharts/7.1.2/modules/offline-exporting.js"></script>
   
<!-- jsPDF (IE-compatible v1.3.4) -->
            <script src="../Scripts/es6-promise.auto.min.js"></script>
        <script src="../Scripts/es6-promise.min.js"></script>
         <script src="../Scripts/jsPDF.js"></script>

     <style type="text/css">
          .chart-container {
            width: 90%;
            margin: 20px auto;
            height: 400px;
            border: 1px solid #ccc;
            border-radius: 10px;
            padding: 10px;
        }
   .overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.5);
            z-index: 1000;
        }

        .popup {
            background-color: #fff;
            width: 450px;
            margin: 100px auto;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.3);
            position: relative;
        }

        .popup h3 {
            margin-top: 0;
            text-align: center;
        }

        .form-row {
            display: flex;
            justify-content: space-between;
            margin-bottom: 12px;
        }

        .form-row label {
            font-weight: bold;
            margin-bottom: 4px;
            display: block;
        }

        .form-row .form-group {
            flex: 1;
            margin-right: 10px;
        }

        .form-row .form-group:last-child {
            margin-right: 0;
        }

        input[type="date"], select {
            width: 100%;
            padding: 6px;
            box-sizing: border-box;
        }

        .btn {
            padding: 6px 12px;
            border: none;
            background-color: #007bff;
            color: #fff;
            cursor: pointer;
            border-radius: 4px;
        }

        .btn:hover {
            background-color: #0056b3;
        }

        .btn-close {
            position: absolute;
            top: 8px;
            right: 10px;
            background: none;
            border: none;
            font-size: 18px;
            cursor: pointer;
        }

        /* Radio button label on left */
       .radio-left-label input[type="radio"] {
    margin-right: 20px;   
}

.radio-left-label label {
    margin-right: 20px;  
}

       .checkbox-left-label {
    display: flex;
    align-items: center;
}

.checkbox-left-label label {
    margin-right: 5px; 
    cursor: pointer;  
}

.checkbox-left-label input[type="checkbox"] {
    margin: 0; 
}
.checkbox-group {
    display: flex;
    gap: 15px; /* space between checkboxes */
    margin-bottom: 5px; /* space between checkboxes and dropdown */
}

.checkbox-group label {
    cursor: pointer;
}
.multi-select-dropdown {
    width: 250px;
    position: relative;
    cursor: pointer;
}
.selected-items {
    padding: 5px;
    border: 1px solid #ccc;
    background: #fff;
}
.dropdown-list div {
    display: flex;
    align-items: center;
    padding: 3px 0;
}

/* Label first, checkbox after */
.dropdown-list div label {
    margin-right: 5px;
}

/* Checkbox appears after label using order */
.dropdown-list div input[type=checkbox] {
    order: 2;
    margin-left: auto; /* optional spacing */
}
.dropdown-list div label {
    order: 1;
}
.dropdown-list div:hover {
    background-color: #f1f1f1;
}
 #ovrly {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.6);
        z-index: 9999;
        display: none;
    }

    /* 🔹 Centered text or spinner */
    .overlay-content {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        color: #fff;
        font-size: 20px;
        font-weight: bold;
        text-align: center;
    }
    </style>
        <script type="text/javascript">

            window.onload = function () {
                if (document.getElementById("<%= hdnFlag.ClientID %>").value == "1") {
                    document.getElementById("<%= hdnFlag.ClientID %>").value = "0";
                    hidewait();
                }
                else {
                    openPopup();

                    updateTimeFields();

                    var radios = document.querySelectorAll('input[name="<%= rblOption.UniqueID %>"]');
                    radios.forEach(function (radio) {
                        radio.addEventListener('change', updateTimeFields);
                    });

                    var chk = document.getElementById('<%= chk24Hours.ClientID %>');
                    chk.addEventListener('change', function () {
                        var startTime = document.getElementById('<%= ddlStartTime.ClientID %>');
                        var endTime = document.getElementById('<%= ddlEndTime.ClientID %>');

                        startTime.disabled = chk.checked;
                        endTime.disabled = chk.checked;
                    });
                }
            };
           
            function onOptionChanged() {
                updateTimeFields();
            }
           
            function toggleTimeFields() {
                var chk = document.getElementById('<%= chk24Hours.ClientID %>');
                 var startTime = document.getElementById('<%= ddlStartTime.ClientID %>');
                 var endTime = document.getElementById('<%= ddlEndTime.ClientID %>');

                 startTime.disabled = chk.checked;
                 endTime.disabled = chk.checked;
            }
            function updateTimeFields() {
                var radios = document.querySelectorAll('input[name="<%= rblOption.UniqueID %>"]');
               var selectedValue = '';
               radios.forEach(function (radio) {
                   if (radio.checked) selectedValue = radio.value;
               });

               var startTime = document.getElementById('<%= ddlStartTime.ClientID %>');
        var endTime = document.getElementById('<%= ddlEndTime.ClientID %>');
               var chk24 = document.getElementById('<%= chk24Hours.ClientID %>');

                if (selectedValue === 'DayofWeek') {
                   startTime.disabled = true;
                   endTime.disabled = true;
                   chk24.disabled = true;
                } else {

                   startTime.disabled = chk24.checked;
                   endTime.disabled = chk24.checked;
                    chk24.disabled = false;
               }
           }
            function toggleDropdown() {
                var list = document.getElementById('behDropdownList');
                list.style.display = (list.style.display === 'none' ? 'block' : 'none');
            }

            function toggleAll(allCheckbox) {
                document.getElementById("lblerror").innerText = "";
                var container = document.getElementById('<%= behDropdownList.ClientID %>');
               var inputs = container.getElementsByTagName('input');

               for (var i = 0; i < inputs.length; i++) {
                   if (inputs[i].type === 'checkbox' && inputs[i].name === 'behavior') {
                       inputs[i].checked = allCheckbox.checked;
                   }
               }
           }

            function updateAllCheckbox() {
                document.getElementById("lblerror").innerText = "";
               var container = document.getElementById('<%= behDropdownList.ClientID %>');
    var inputs = container.getElementsByTagName('input');
    var allCheckbox = document.getElementById('chkAll');
    var allChecked = true;

    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type === 'checkbox' && inputs[i].name === 'behavior') {
            if (!inputs[i].checked) {
                allChecked = false;
                break;
            }
        }
    }

    allCheckbox.checked = allChecked;
}
           
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <div id="ovrly" style="display:none;">
    <div class="overlay-content">
        <span>Please wait...</span>
    </div>
    </div>
        <div id="overlay" class="overlay">
            <div class="popup">
                <button type="button" class="btn-close" onclick="closePopup()">×</button>
                <asp:ScriptManager ID="ScriptManager1" runat="server" />

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

                <div class="form-row">
                    <asp:RadioButtonList ID="rblOption" runat="server" RepeatDirection="Horizontal" CssClass="radio-left-label" OnClientClick="updateTimeFields();">                     
                           <asp:ListItem Text="Time of Day" Value="TimeofDay"></asp:ListItem>
                        <asp:ListItem Text="Day of Week" Value="DayofWeek"></asp:ListItem>
                    </asp:RadioButtonList>
                </div> 

                <div class="form-row">
                   <div class="form-group">
                        <label for="txtStartDate">Start Date:</label>
                        <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtEndDate">End Date:</label>
                        <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>
                    </div>
                </div>

        

               <div class="form-row">
                                       <div class="form-group checkbox-left-label">
                        <asp:CheckBox ID="chk24Hours" runat="server" onchange="updateTimeFields();"/>
                        <label for="<%= chk24Hours.ClientID %>">24-hours</label>
                    </div>
                    <div class="form-group" style="display:flex; align-items:center; padding:0 10px;">
                        <label style="margin:0;">Or</label>
                    </div>
                    <div class="form-group">
                        <label>Start Time:</label>
                        <asp:DropDownList ID="ddlStartTime" runat="server"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>End Time:</label>
                        <asp:DropDownList ID="ddlEndTime" runat="server"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label for="ddlCategory">Behaviors:</label>
        
                        <!-- Grouped Checkboxes -->
                        <div class="checkbox-group">
                            <asp:CheckBox ID="chkActive" runat="server" Text="Active" Checked="true" OnCheckedChanged="chkActive_CheckedChanged"  AutoPostBack="true"/>
                            <asp:CheckBox ID="chkInactive" runat="server" Text="Inactive" Checked="false" OnCheckedChanged="chkActive_CheckedChanged" AutoPostBack="true"/>
                        </div>
                         <div class="multi-select-dropdown" id="divCategoryDropdown">
                                <div class="selected-items" onclick="toggleDropdown()">
                                    Select Behaviors
                                </div>
                                <div class="dropdown-list" id="behDropdownList" runat="server" style="display:none; border:1px solid #ccc; padding:5px; max-height:200px; overflow:auto;" >
                                </div>
                            </div>
                              <asp:Button ID="btnsubmit" Style="float: right; margin: 0 1px 0 1px" runat="server" ToolTip="Show Graph" CssClass="showgraph" OnClick="btnsubmit_Click"/>

                    </div>
                </div>
                <div class="form-row">
                   <div class="form-group">
                    <asp:Label 
                        ID="lblerror" 
                        runat="server" 
                        Text="" 
                        style="color:red; text-align:left; display:block;">
                    </asp:Label>               
                   </div>
                </div>
         </ContentTemplate>

    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="chk24Hours" EventName="CheckedChanged" />
        <asp:AsyncPostBackTrigger ControlID="chkActive" EventName="CheckedChanged" />
        <asp:AsyncPostBackTrigger ControlID="chkInactive" EventName="CheckedChanged" />
    </Triggers>
</asp:UpdatePanel>
            </div>
        </div>
    </div>
        <div>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: right">
                            <asp:UpdatePanel ID="dropdownpanel" runat="server">
                                    <ContentTemplate>
                            <asp:Button ID="btnPrevious" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0px 250px" Visible="true" Text="Previous"  OnClick="btnPrevious_Click" onClientClick="loadwait();"/>
                             <asp:DropDownList ID="ddlLessonplan" runat="server" CssClass="drpClass" Style="float: left; margin: 0 1px 0px 10px" Height="26px" Width="290px" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="ddlLessonplan_SelectedIndexChanged" onClientClick="loadwait();">
                             </asp:DropDownList>
                            <asp:Button ID="btnNext" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0 10px" Visible="true" Text="Next" OnClick="btnNext_Click" onClientClick="loadwait();" />
                                </ContentTemplate>
                                </asp:UpdatePanel> 
                            <input type="button" id="btnRefresh" style="float: right; margin: 0 1px 0 1px" class="refresh"  onclick="openPopup()"/>
                            <asp:Button ID="btnExport" runat="server" Style="float: right; margin: 0 1px 0 1px" ToolTip="Export To PDF" CssClass="pdfPrint" OnClick="btnExport_Click" OnClientClick="showOverlay()"/>
                        </td>
                        <td style="width: 10%"></td>
                    </tr>
                </table>
            </div>
        <asp:HiddenField ID="hdnFlag" runat="server" Value="0" />
           <div>
               <asp:UpdatePanel ID="highchartupdate" runat="server">
                                                       <ContentTemplate>

            <center>
        <div id="container" runat="server" style="width:1000px; height:750px;" visible="false"></div>
                <div id="chartArea" runat="server"></div>
            </center>
                                                             </ContentTemplate>

                                </asp:UpdatePanel>
           </div>
         <script>
             function loadchart(jsArr1, jsArr2, jsArr3, primaryaxis, secondaryaxis, title) {
                 var xlabel = "";
                 var radios = document.getElementsByName("<%= rblOption.UniqueID %>");
                 for (var i = 0; i < radios.length; i++) {
                     if (radios[i].checked) {
                         xlabel= radios[i].nextSibling.innerText || radios[i].nextSibling.textContent;
                     }
                 }
                 var sec=false;
                 if(secondaryaxis=="dur")
                 {
                     sec=true;
                 }
                 var prim=true;
                 if(primaryaxis=="")
                 {
                     prim=false;
                 }


                 var seriesArr = [];
                 if (primaryaxis != "")
                 {
                     seriesArr.push({
                         name: primaryaxis,
                         type: 'column',
                         yAxis: 0,
                         data: jsArr2,
                         color: 'blue',
                         tooltip: { valueSuffix: '' }
                     });
                 }

                 if (sec) {
                     seriesArr.push({
                         name: 'Duration',
                         type: 'spline',
                         yAxis: 1,
                         data: jsArr3,
                         color: 'red',  
                         marker: { enabled: false }, 
                         tooltip: { valueSuffix: '' }
                     });
                 }

                 Highcharts.chart('container', {
                     credits: {
                         enabled: false
                     },
                     chart: {
                         zoomType: 'xy'
                     },
                     
                     title: {
                         text: '<div style="text-align:center; line-height:1.4;">' +
                '<span style="font-size:16px;">' +
                    title.split('<br/>')[0] +
                '</span><br/>' +
                '<span style="font-size:12px; color:#666;">' +
                    title.split('<br/>')[1] +
                '</span>' +
            '</div>',
                         useHTML: true
                     },
                     xAxis: [{
                         categories: jsArr1,
                         title: {
                             text: xlabel,
                              style: {
                                 color: Highcharts.getOptions().colors[1]
                             }
                         },
                         labels: {
                             rotation: -90,
                             align: 'right'
                         }
                     },],
                     yAxis: [{ // Primary yAxis
                         visible:prim,
                         labels: {
                             format: '{value}',
                             style: {
                                 color: Highcharts.getOptions().colors[1]
                             }
                         },
                         title: {
                             text: primaryaxis,
                             style: {
                                 color: Highcharts.getOptions().colors[1]
                             }
                         }
                        
                     }, { // Secondary yAxis
                         title: {
                             text: 'Duration  (in seconds)',
                             style: {
                                 color: 'black'
                             }
                         },
                         labels: {
                             format: '{value}',
                             style: 
                                 {
                                  color: 'black' 
                             }
                         },
                         opposite: true,
                         visible: sec
                     }],
                     tooltip: {
                         shared: true
                     },
                     series: seriesArr,
                     exporting: {
                         enabled: false
                         
                     }
                 });
             }

 


             function renderMultipleCharts(chartData) {
        var wrapper = document.getElementById("chartArea");
        wrapper.innerHTML = "";
        allCharts = [];
        var filename = "";
        // Create jsPDF instance in landscape ('l'), points ('pt'), A4
        var pdf = new window.jspdf.jsPDF('l', 'pt', 'a4');
        var pdfWidth = pdf.internal.pageSize.getWidth();
        var pdfHeight = pdf.internal.pageSize.getHeight();

        var chartPromises = [];

        for (var i = 0; i < chartData.length; i++) {
            (function (c, index) {
                var div = document.createElement("div");
                div.id = "container_" + c.id;
                div.style.height = "400px";
                div.style.marginBottom = "30px";
                wrapper.appendChild(div);
                filename = c.sname;
                var chart = loadchartexp(
                    c.jsArr1, c.jsArr2, c.jsArr3,
                    c.primaryaxis, c.secondaryaxis,
                    c.title, div.id
                );

                chartPromises.push(
                    new Promise(function (resolve) {
                        setTimeout(function () {
                            var svg = chart.getSVG({
                                exporting: { sourceWidth: 1200, sourceHeight: 600 } 
                            });

                            var canvas = document.createElement("canvas");
                            canvas.width = 1200;
                            canvas.height = 600;
                            var ctx = canvas.getContext("2d");

                            var DOMURL = window.URL || window.webkitURL || window;
                            var img = new Image();
                            var svgBlob = new Blob([svg], { type: "image/svg+xml;charset=utf-8" });
                            var url = DOMURL.createObjectURL(svgBlob);

                            img.onload = function () {
                                ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                                DOMURL.revokeObjectURL(url);

                                var imgData = canvas.toDataURL("imagePNG", 1.0);

                                var ratio = Math.min(pdfWidth / canvas.width, pdfHeight / canvas.height);
                                var imgWidth = canvas.width * ratio;
                                var imgHeight = canvas.height * ratio;
                                var xOffset = (pdfWidth - imgWidth) / 2;
                                var yOffset = (pdfHeight - imgHeight) / 2;

                                if (index !== 0) pdf.addPage();
                                pdf.addImage(imgData, 'PNG', xOffset, yOffset, imgWidth, imgHeight);

                                resolve();
                            };
                            img.src = url;
                        }, 500);
                    })
                );
            })(chartData[i], i);
        }
        filename = filename + ".pdf";

        Promise.all(chartPromises).then(function () {
            pdf.save(filename);
            setTimeout(function () {
                alert("PDF download completed.");
            }, 500);
        });
        document.getElementById("<%= hdnFlag.ClientID %>").value = "1";
                 document.getElementById("btnsubmit").click();

    }



             function loadchartexp(jsArr1, jsArr2, jsArr3, primaryaxis, secondaryaxis, title, containerId) {
                 var xlabel = "";
                 var radios = document.getElementsByName("<%= rblOption.UniqueID %>");
                 for (var i = 0; i < radios.length; i++) {
                     if (radios[i].checked) {
                         xlabel = radios[i].nextSibling.innerText || radios[i].nextSibling.textContent;
                     }
                 }
                 var sec = (secondaryaxis === "dur");
                 var prim = (primaryaxis !== "");

                 var seriesArr = [];

                 if (primaryaxis !== "") {
                     seriesArr.push({
                         name: primaryaxis,
                         type: 'column',
                         yAxis: 0,
                         data: jsArr2,
                         color: 'blue',
                         tooltip: { valueSuffix: '' }
                     });
                 }

                 if (sec) {
                     seriesArr.push({
                         name: 'Duration',
                         type: 'spline',
                         yAxis: 1,
                         data: jsArr3,
                         color: 'red',
                         marker: { enabled: false },
                         tooltip: { valueSuffix: '' }
                     });
                 }

                 return Highcharts.chart(containerId, {
                     credits: {
                         enabled: false
                     },
                     chart: { zoomType: 'xy' },
                     title: {
                         text: title.split('<br/>')[0], // replace <br/> with \n
                         align: 'center',
                         verticalAlign: 'top',
                         
                     },
                     subtitle: {
                         text: title.split('<br/>')[1], // second line
                         align: 'center',
                         y: 30
                         
                     },
                     xAxis: [{
                         categories: jsArr1,
                         title: {
                             text: xlabel,
                             style: {
                                 color: Highcharts.getOptions().colors[1]
                             }
                         },
                         labels: {
                             rotation: -90,
                             align: 'right'
                         }
                     },],
                     yAxis: [{
                         visible: prim,
                         labels: { format: '{value}', style: { color: Highcharts.getOptions().colors[1] } },
                         title: { text: primaryaxis, style: { color: Highcharts.getOptions().colors[1] } }
                     }, {
                         title: { text: 'Duration   (in seconds)', style: { color: 'black' } },
                         labels: { format: '{value}', style: { color: 'black' } },
                         opposite: true,
                         visible: sec
                     }],
                     tooltip: { shared: true },
                     series: seriesArr,
                     exporting: {
                         enabled: true,
                         fallbackToExportServer: false, 
                         buttons: {
                             contextButton: {
                                 menuItems: [
                                     'downloadPNG',
                                     'downloadJPEG',
                                     'downloadPDF',
                                     'downloadSVG'
                                 ]
                             }
                         }
                     }
                 });
             }
             function showOverlay() {
                 document.getElementById('overlay').style.display = 'none';
                 var overlay = document.getElementById('ovrly');
                 if (overlay) overlay.style.display = 'block';
             }
             function hideOverlay() {
                 var overlay = document.getElementById('ovrly');
                 if (overlay) overlay.style.display = 'none';
             }
             function openPopup() {
                 document.getElementById('overlay').style.display = 'block';
             }
             function onOptionChanged() {
                 updateTimeFields();
             }
             function closePopup() {
                 document.getElementById('overlay').style.display = 'none';
             }
             function loadwait()
             {
                 document.getElementById('ovrly').style.display = 'block';
             }
             function hidewait() {
                 document.getElementById('ovrly').style.display = 'none';
             }
             document.addEventListener("click", function (event) {
                 var dropdown = document.getElementById("divCategoryDropdown");

                 // if click is outside dropdown
                 if (!dropdown.contains(event.target)) {
                     var list = document.getElementById("behDropdownList");
                     if (list) {
                         list.style.display = "none";
                     }
                 }
             });

             function btnExport_Click() {

                 // show loader
                 loadwait();

                 // hide container
                 document.getElementById("container").style.display = "none";

                 // show chart area
                 document.getElementById("chartArea").style.display = "block";

                 // get all dropdown values
                 var ddl = document.getElementById("ddlLessonplan");
                 var values = [];

                 for (var i = 0; i < ddl.options.length; i++) {
                     values.push(ddl.options[i].value);
                 }

                 var allValues = values.join(",");

                 // call existing function
                 Loaddata(allValues, true);
             }

         </script>
    </form>
</body>
</html>
