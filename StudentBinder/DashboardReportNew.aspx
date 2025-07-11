<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardReportNew.aspx.cs" Inherits="Graph" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            Array.prototype.indexOf = (function (Object, max, min) {
                "use strict";
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

    <%--<script src="js/jquery-2.1.0.js"></script>--%>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
     <script src="../Scripts/highcharts/7.1.2/highcharts.js"></script>
   <script src="../Scripts/highcharts/7.1.2/modules/accessibility.js"></script>
    <script src="js/d3.v3.js"></script>
    <script src="js/nv.d3.js"></script>
    <link href="../Administration/CSS/GraphStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/jsDatePickforGraph.css" rel="stylesheet" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js"></script>
    <link href="CSS/nv.d3.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    
   
    <style type="text/css">

        .loading {
            display: none;
            position: absolute;
            width: 100%;
            height: 800px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            background-image: url("../Administration/images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
        }
        .innerLoading {
            margin: auto;
            height: 50px;
            width: 250px;
            text-align: center;
            font-weight: bold;
            font-size: large;
        }

            .innerLoading img {
                margin-top: 200px;
                height: 10px;
                width: 100px;
            }

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

        .NFButton {            
            /*background-color: #00549f;*/
            width: 105px;
            height: 50px;
            color: #fff;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: bold;
            text-decoration: none;
            word-wrap:break-word;
            background-position: 0 0;
            margin-left:10px;
            border: 10px;
            cursor: pointer;
        }

        .divchkbox label {
          vertical-align: text-bottom;
        }

  .graph-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 20px;
    width: 100%;
    max-width: 100%;
    padding: 20px;
    box-sizing: border-box;
  }

  .graph-container {
    width: 100%;
    height: 100%;
    aspect-ratio: 16 / 9; 
    background-color: #f4f4f4; 
    border: 1px solid #ccc;
  }
    </style>
    <style>
        /*Date Range Filter Styling*/
        .date-range-container {
    display: block;
    flex-align: center;
    font-size: 14px;
    font-weight: bold;
    color: #00549f;
}

.date-label {
    margin-right: 5px;
}

.date-input {
    width: 100px;
    padding: 5px;
    border: 1px solid #00549f;
    border-radius: 3px;
    text-align: center;
    font-weight: bold;
    color: #00549f;
}

        /*Button Styling*/
        .btnGo {
    background-color: #007bff;
    color: white; 
    padding: 15px 20px; 
    border: none;
    border-radius: 10px;
    font-family: Arial, Helvetica, sans-serif;
    font-size: 12px;
    font-weight: bold; 
    cursor: pointer;
    transition: background-color 0.3s ease-in-out;
}

/* Hover Effect */
.btnGo:hover {
    background-color: #0056b3;
}

/* Active (Click) Effect */
.btnGo:active {
    background-color: #004494;
}

/* GridView Styling */
        .tab-content{
            max-height:550px;
            overflow-y:auto;
        }
.gridview {
    max-height:550px;
    overflow-y:auto;
    width: auto;
    border-collapse: collapse;
}
        .gridview td {
    border: 1px solid black;

        }
.gridview th{
    text-align: center;
    border: 1px solid black;
    font-weight: bold;
}

.gridview th:nth-child(1), 
.gridview th:nth-child(2){
    writing-mode:horizontal-lr;

    padding:5px;
    white-space:nowrap;
}
.gridview td{
        text-align: center;
        }
/* Rotated Headers for Date Columns */
.gridview th:nth-child(n+3) {
    writing-mode:vertical-lr;
    -webkit-writing-mode: vertical-lr;
    padding-top: 2px;
    padding-bottom: 2px;
}
.summary-row {
border-top: 2px solid blue;
border-bottom: 2px solid blue;
        }
/* Sticky last row */
/*.gridview tbody tr:last-child {
    position: sticky;
    bottom: 0;
    background-color: white;
    z-index: 2;
}*/
/* Sticky last column */
/*.gridview th:last-child, 
.gridview td:last-child {
    position: sticky;
    right: 0;
    background-color: white;
    z-index: 2;
}*/


/*Dropdown Styling*/
.class-dropdown {
    width: 200px;
    padding: 5px; 
    font-size: 14px;
    border: 1px solid #ccc; 
    border-radius: 4px; 
    background-color: #fff;
    background-position: right 10px center; 
    background-repeat: no-repeat; 
}
    </style>
    <script>
        function toggleRows(className) {
            var safeClassName = className.replace(/ /g, "_");

            var rows = document.querySelectorAll("tr.class-" + CSS.escape(safeClassName));

            if (rows.length === 0) {
                return;
            }

            // Find the button inside the summary row
            var button = document.querySelector("tr.class-" + CSS.escape(safeClassName) + ".summary-row .collapse-button");

            // Check if rows are currently visible
            var isVisible = false;
            for (var i = 0; i < rows.length; i++) {
                if (!rows[i].classList.contains("summary-row") && rows[i].style.display !== "none") {
                    isVisible = true;
                    break;
                }
            }

            // Toggle visibility for all rows except the summary row
            for (var i = 0; i < rows.length; i++) {
                if (!rows[i].classList.contains("summary-row")) {
                    rows[i].style.display = isVisible ? "none" : "table-row";
                }
            }

            // Update the collapse button arrow
            if (button) {
                var rawClassName = button.textContent.replace(/^▼|►/, "").trim();
                button.innerHTML = isVisible ? "&#9658; " + rawClassName : "&#9660; " + rawClassName;
                //button.innerHTML = isVisible ? "► " + rawClassName : "▼ " + rawClassName;
            }
        }
        //$(function () {
        //    // Get current date
        //    var endDate = new Date();
        //    // Get date 7 days prior
        //    var startDate = new Date();
        //    startDate.setDate(endDate.getDate() - 7);

        //    // Format date as MM/DD/YYYY
        //    var formatDate = function (date) {
        //        var dd = String(date.getDate()).padStart(2, '0');
        //        var mm = String(date.getMonth() + 1).padStart(2, '0');
        //        var yyyy = date.getFullYear();
        //        return mm + '/' + dd + '/' + yyyy;
        //    };

        //    // Set default values
        //    $('#txtstartDate').val(formatDate(startDate));
        //    $('#txtendDate').val(formatDate(endDate));

        //});

        function setDateFields(startDate, endDate) {
            function parseDate(dateStr) {
                if (typeof dateStr === 'string') {
                    var parts = dateStr.split("-");
                    // Proceed with processing
                } else {
                    console.error("Expected a string but received:", typeof dateStr);
                }
                var month = parseInt(parts[0], 10) - 1; // Months are zero-based
                var day = parseInt(parts[1], 10);
                var year = parseInt(parts[2], 10);
                return new Date(year, month, day);
            }

            // Helper function to format a Date object as "MM/DD/YYYY"
            function formatDateToMMDDYYYY(date) {
                var month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
                var day = String(date.getDate()).padStart(2, '0');
                var year = date.getFullYear();
                return month + '/' + day + '/' + year;
            }

            // Parse the input date strings
            var startDateObject = parseDate(startDate);
            var endDateObject = parseDate(endDate);

            // Format the Date objects to "MM/DD/YYYY"
            var formattedStartDate = formatDateToMMDDYYYY(startDateObject);
            var formattedEndDate = formatDateToMMDDYYYY(endDateObject);

            // Set the formatted dates as values of the input fields
            $('#txtstartDate').val(formattedStartDate);
            $('#txtendDate').val(formattedEndDate);
        }

</script>
    <script type="text/javascript">
        window.onload = function () {


            var d = new Date();
            var strDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
            $('#lblDates').html(strDate);
            $('#lblDate').html(strDate);
            new JsDatePick({
                useMode: 2,
                target: "<%=txtstartDate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtendDate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });

        };
        
        function loadWait() {
            var checkbox = document.getElementById('<%= highcheck.ClientID %>');
           if (checkbox && checkbox.checked) {
               $('.loading').fadeIn('fast');
           }
        }
        function showLoader() {
            $('.loading').fadeIn('fast');
        }
        function enableLoader() {
            var startDate = new Date(document.getElementById('<%= txtstartDate.ClientID %>').value);
            var endDate = new Date(document.getElementById('<%= txtendDate.ClientID %>').value);
            var errorLabel = document.getElementById("dateError");
            if (startDate > endDate) {
                errorLabel.innerText = "Start date must be before end date.";
                errorLabel.style.display = "block";
                return false;
            }
            var maxEndDate = new Date(startDate);
            maxEndDate.setMonth(maxEndDate.getMonth() + 3);
            if (endDate > maxEndDate) {
                errorLabel.innerText = "Date range cannot exceed 3 months.";
                errorLabel.style.display = "block";
                return false;
            }
            errorLabel.style.display = "none";
            $('.loading').fadeIn('fast');
            return true

        }
        function HideWait() {
            $('.loading').fadeOut('fast');
        }
        function HideWaitOG() {
            var checkbox = document.getElementById('<%= highcheck.ClientID %>');
            if (checkbox.checked) {
                $('.loading').fadeOut('fast');//, function () { });
            }
        }
        function disableLoader() {
                $('.loading').fadeOut('fast');
        }
        document.addEventListener('DOMContentLoaded', function () {
            var activeTabId = document.getElementById('hdnActiveTab').value;
            if (activeTabId) {
                var tabElement = document.querySelector('a[href="' + activeTabId + '"]');
                if (tabElement) {
                    var tabTrigger = new bootstrap.Tab(tabElement);
                    tabTrigger.show();
                }
            }

            var tabs = document.querySelectorAll('#gridTabs a[data-bs-toggle="tab"]');
            tabs.forEach(function (tab) {
                tab.addEventListener('shown.bs.tab', function (e) {
                    var selectedTab = e.target.getAttribute('href');
                    document.getElementById('hdnActiveTab').value = selectedTab;
                });
            });
        });

        window.onbeforeunload = function () {
            var active = document.querySelector('#gridTabs a.nav-link.active');
            if (active) {
                document.getElementById('hdnActiveTab').value = active.getAttribute('href');
            }
        };

        function ChartView() {
            document.querySelector('.divchkbox').style.display = 'block';
            document.getElementById('chkbox-selection').style.display = 'block';
            document.getElementById('selection-label').style.display = 'block';
            document.querySelector('.graph-grid').style.display = 'grid';
            
            document.querySelector('.gvDivClass').style.display = 'none';
            document.querySelector('.date-range-container').style.display = 'none';
            document.querySelector('.divbtns').style.display = 'none';

        }
        function TableView() {
            document.querySelector('.divchkbox').style.display = 'none';
            document.querySelector('.divbtns').style.display = 'block';
            document.getElementById('chkbox-selection').style.display = 'none';
            document.getElementById('selection-label').style.display = 'none';
            document.querySelector('.graph-grid').style.display = 'none';


            document.querySelector('.gvDivClass').style.display = 'block';
            document.querySelector('.date-range-container').style.display = 'block';
        }
        $(function () {

            //drawGraph();
            //drawGraphs();
            var isiPad = navigator.userAgent.match(/iPad/i);
            if (isiPad != null) {
                $(".Head").attr("style", "padding-left: 25%")
            }

            $(".nv-controlsWrap").hide();

        });


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

        $(document).ready(function () {
            $('#rbtnClassType').change(function () {
                var selected_value = $('#<%=rbtnClassType.ClientID %> input:checked').val();
                //alert('Radio Box has been changed! ' + selected_value);
                $('#<%=Txt_clstype.ClientID %>').val(selected_value);
                $('#btn_clstype_Click').click();
            });
        });

        function onSelectionChange() {
            var selectedVals = '';
            $.each($('#ddllanguage input[type=checkbox]:checked'), function () {
                selectedVals += $("label[for='" + this.id + "']").html() + ",";
            });
            $('#txtValue').val(selectedVals);
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="loading">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="180000"> </asp:ScriptManager>
        <div class="mainDivS">
            <div class="Head" style="float: none; font-size: 35px; text-align:center; font-family:Tahoma; color: #00549f;margin-top:-45px">
                <h3 style="font-family:Tahoma">Dashboard - Today's <!--(<label id="lblDate"></label>)--> Progress</h3>
            </div>
            <div style="width:100%">
               <%-- <div class="divdrop" style="margin-top:-30px">
                    <table>
                        <tr>
                            <td style="text-align:center;width:25%">
                                <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <span style="text-align:right;font-weight:bolder;font-size:15px;color: #00549f;padding-right: 50px;padding-left: 45px;">Choose Clients:</span>
                                    <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Location(s) :</span> 
		                            <asp:DropDownCheckBoxes ID="ddcb_clas" runat="server" TabIndex="1" AddJQueryReference="True" UseButtons="true" UseSelectAllNode="false" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="ddcb_clas_SelectedIndexChanged">
			                            <Style SelectBoxWidth="200px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight="400" DropDownBoxCssClass="ddchkLesson"/>
			                            <Texts SelectBoxCaption="Select Location"/>
		                            </asp:DropDownCheckBoxes>		      
		                            <span style="text-align:right;font-weight:bolder;font-size:12px;padding-left:15px;margin-right:15px;color: #00549f;">or</span>
                                    <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Client Name(s) :</span> 
		                            <asp:DropDownCheckBoxes ID="ddcb_stud" runat="server" TabIndex="1" AddJQueryReference="True" UseButtons="true" UseSelectAllNode="false" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="ddcb_stud_SelectedIndexChanged" OnDataBound="ddcb_stud_DataBound">
			                            <Style SelectBoxWidth="200px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight="400" DropDownBoxCssClass="ddchkLesson"/>
			                            <Texts SelectBoxCaption="Select Client"/>
		                            </asp:DropDownCheckBoxes> 
                                </ContentTemplate>
                                </asp:UpdatePanel>  
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbtnClassType" runat="server" RepeatDirection="Horizontal" BorderColor="#00549f" BorderStyle="None" BorderWidth="1px" style="margin-left:-90px; vertical-align: bottom; display: inline-table" RepeatLayout="Flow" OnSelectedIndexChanged="rbtnClassType_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="DAY" style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;padding-right:10px">Day</asp:ListItem>
                                    <asp:ListItem Value="RES" style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;padding-right:10px">Residence</asp:ListItem>                                
                                    <asp:ListItem Value="BOTH" Selected="True" style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;padding-right:10px">Both</asp:ListItem>                                
                                </asp:RadioButtonList>    
                            </td>
                        </tr>
                    </table>
                </div>--%>
                <asp:Button ID="BtnSwitchTableChart" cssclass="btnGo" style="width:100px; position: absolute; top:20px; right:80px; height:45px;" runat="server" OnClick="BtnSwitchTableChart_Click" Text="" />
                    <asp:button id="BtnRefresh" runat="server" text="Refresh" cssclass="NFButton" style="border-radius:10px; height:45px; background-color:#34b233; position: absolute; top:20px; right:190px;" tooltip="Refresh the Dashboard" OnClick="BtnRefresh_Click"      />
                <div class="divbtns" style="padding-top:60px;padding-right: 140px;margin-top:-50px; padding-left:65px; display:none;">
                    <table>
                        <tr>
                            <%--<td colspan="3" style="text-align:center;padding-left: 200px;width:15%">
                                 <span style="display:none; text-align:center;font-weight:bolder;font-size:15px;color: #00549f;margin-left:-150px">Choose Reports:</span>
                            </td>--%>
                            <td colspan="2" style="text-align:left;width: 20%;">
                                 <asp:button id="BtnClientAcademic" runat="server" text="" visible="false" cssclass="NFButton" tooltip="Academic by Client" OnClick="BtnClientAcademic_Click" BackColor="#00549F"  />
                                 <asp:button id="BtnStaffAcademic" runat="server" text="" visible="false" cssclass="NFButton" tooltip="Academic by Staff" OnClick="BtnStaffAcademic_Click" BackColor="#00549F"   />
                                 <asp:button id="BtnClientClinical" runat="server" text="" visible="false" cssclass="NFButton" tooltip="Clinical by Client" OnClick="BtnClientClinical_Click" BackColor="#00549F"   />
                                 <asp:Button id="BtnStaffClinical" runat="server" text="" visible="false" cssclass="NFButton" style="width:102px" tooltip="Clinical by Staff" OnClick="BtnStaffClinical_Click" OnClientClick="loadWait();" BackColor="#00549F"  />
                                <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <span style="text-align:right;font-weight:bolder;font-size:15px;color: #00549f;padding-right: 50px;">Choose Clients:</span>
                                    <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Location(s) :</span> 
		                            <asp:DropDownCheckBoxes ID="ddcb_clas" runat="server" TabIndex="1" AddJQueryReference="True" UseButtons="true" UseSelectAllNode="false" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="ddcb_clas_SelectedIndexChanged">
			                            <Style SelectBoxWidth="200px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight="400" DropDownBoxCssClass="ddchkLesson"/>
			                            <Texts SelectBoxCaption="Select Location"/>
		                            </asp:DropDownCheckBoxes>		      
		                            <span style="text-align:right;font-weight:bolder;font-size:12px;padding-left:15px;margin-right:15px;color: #00549f;">or</span>
                                    <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Client Name(s) :</span> 
		                            <asp:DropDownCheckBoxes ID="ddcb_stud" runat="server" TabIndex="1" AddJQueryReference="True" UseButtons="true" UseSelectAllNode="false" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="ddcb_stud_SelectedIndexChanged" OnDataBound="ddcb_stud_DataBound">
			                            <Style SelectBoxWidth="200px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight="400" DropDownBoxCssClass="ddchkLesson"/>
			                            <Texts SelectBoxCaption="Select Client"/>
		                            </asp:DropDownCheckBoxes> 
                                </ContentTemplate>
                                </asp:UpdatePanel>  
                            </td>
                            <td>
                                <div style="color:#00549f">Filter the Location Dropdown List by:</div>
                                    <asp:radiobuttonlist id="rbtnClassType" runat="server" repeatdirection="Horizontal" bordercolor="#00549f" borderstyle="None" borderwidth="1px" style="margin-left: 0px; margin-top: 0px; vertical-align: bottom; display: inline-table" repeatlayout="Flow" onselectedindexchanged="rbtnClassType_SelectedIndexChanged" autopostback="True" onchange="showLoader();">
                                    <asp:ListItem Value="DAY" style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;padding-right:10px">Day</asp:ListItem>
                                    <asp:ListItem Value="RES" style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;padding-right:10px">Residence</asp:ListItem>                                
                                    <asp:ListItem Value="BOTH" Selected="True" style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;padding-right:10px">Both</asp:ListItem>                                
                                </asp:RadioButtonList>    
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="date-range-container" style="display:none">
                                    <label class="date-label">Date Range:</label>
                                    <asp:TextBox ID="txtstartDate" runat="server" class="date-input"></asp:TextBox>
                                    <span>to</span>
                                    <asp:TextBox ID="txtendDate" runat="server" class="date-input"></asp:TextBox>
                                    <br />
                                    <span id="dateError" style="color: red; display: none;"></span>
                                </div>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkbx_Mistrial" runat="server" style=" color: #00549f;font-size:12px" Checked="True" OnCheckedChanged="chkbx_Mistrial_CheckedChanged"/>
                                <span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom;">Count Mistrial</span>
                            </td>
                            <td>
                                        <asp:CheckBox id="highcheck" checked="true" visible = "false" runat="server"  Text=""></asp:CheckBox>
                                        <asp:Button ID="ButtonGo" cssclass="btnGo" visible ="false"  runat="server" OnClick="ButtonGo_Click" onclientclick="return enableLoader()" Text="Go" />
                                <asp:ImageButton id="btnExportToExcel" runat="server" visible ="false" style="vertical-align:bottom;" ImageUrl="~/Administration/images/Excelexp.png"  onclick="btnExportToExcel_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="divchkbox" style=" padding-top:20px;margin-top:-30px">
                    <table>
                        <tr>
                                
                            <td style="text-align:center;width:70%">
                                <div id="chkbox-selection" style=" padding-top:10px;">
                                <span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom;">Show progress as: </span>
                                <asp:CheckBox ID="chkbx_leson_deliverd" runat="server" Text="total lessons delivered today or" style=" color: #00549f;font-size:12px" AutoPostBack="True" Checked="True" Enabled="False" OnCheckedChanged="chkbx_leson_deliverd_CheckedChanged"/>
                                <asp:CheckBox ID="chkbx_block_sch" runat="server" Text="percentage of block-scheduled lessons" style=" color: #00549f;font-size:12px" AutoPostBack="True" OnCheckedChanged="chkbx_block_sch_CheckedChanged"/>
                                    <asp:dropdownlist id="ddlClassrooms" cssclass="class-dropdown" runat="server"></asp:dropdownlist>

                                </div>
                                <div id="selection-label" style="margin-top:0px;max-width:1100px;word-wrap: break-word;" >
                                <p id="selectionP" style="display:none;>"><span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom">
                                    <asp:Label ID="Label1_CrntSelctn" runat="server" style="font-weight:100">Current Selections:</asp:Label>
                                    <span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom;font-weight:bold;padding-left:10px">Location :
                                    <asp:Label ID="Label_location" runat="server" style="font-weight:100"></asp:Label>
                                    <span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom;font-weight:bold;padding-left:10px">Client :
                                    <asp:Label ID="Label_Client" runat="server" style="font-weight:100"></asp:Label>
                                    </span></p>
                                 </div>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                
                                </td><td id="tdMsg" runat="server" style="width: 97%;font-size: 15px;color:green;text-align:center;font-weight:bold; display: none"></td>
                            <td style="text-align:center;width:70%">
                                <div id="valdisplay" style="visibility: visible;display:none;">
                                <asp:TextBox ID="Txt_StudSelcted" runat="server" BackColor="#FF9966" Width="70px"></asp:TextBox>
                                <asp:TextBox ID="Txt_All" runat="server" ReadOnly="True" Width="50px" BackColor="#77FF66">0</asp:TextBox>
                                <asp:TextBox ID="Txt_Clasid" runat="server" ReadOnly="True" Width="218px" BackColor="#99FF66"></asp:TextBox>
                                <asp:TextBox ID="Txt_Studid" runat="server" ReadOnly="True" Width="249px" BackColor="#00CCFF"></asp:TextBox>
                                <asp:TextBox ID="Txt_Userid" runat="server" ReadOnly="True" Width="204px" BackColor="#99FF66"></asp:TextBox>
                                <asp:TextBox ID="Txt_clstype" runat="server" BackColor="#FF9966" Width="70px">BOTH</asp:TextBox>
                                <asp:TextBox ID="Txt_graphid" runat="server" BackColor="#CC9966" Width="70px"></asp:TextBox>
                                </div>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <!--olddashboard-view--start-->
                <div id="olddashboard-view" style="display:none">
                     <!--<div class="Head" style="position: absolute; width: 300px; height: 30px; left: 40%; top: 28%;">-->
                        <a class="student" href="#" onclick="setSGraph();">Teaching Progress</a>
                        <a class="user" href="#" onclick="setUGraph();">Teaching Sessions</a>
                    <!--</div>-->
                </div>
                <div class="container mt-3 gvDivClass" style="display:none">
                    <asp:HiddenField ID="hdnActiveTab" runat="server" ClientIDMode="Static" />
                    <!-- Tab Navigation -->
                    <ul class="nav nav-tabs" id="gridTabs" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" id="tab1-tab" data-bs-toggle="tab" href="#tab1" role="tab">Programs by Client</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="tab2-tab" data-bs-toggle="tab" href="#tab2" role="tab">Programs by Staff</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="tab3-tab" data-bs-toggle="tab" href="#tab3" role="tab">Clinical by Client</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="tab4-tab" data-bs-toggle="tab" href="#tab4" role="tab">Clinical by Staff</a>
                        </li>
                    </ul>

                    <!-- Tab Content -->
                    <div class="tab-content mt-3">
                        <!-- Tab 1 -->
                        <div class="tab-pane fade show active" id="tab1" role="tabpanel">
                            <asp:gridview id="gvProgramsByClient" runat="server" autogeneratecolumns="false" cssclass="gridview"
                                showheader="true" datakeynames="ClassName" onrowdatabound="gvProgramsByClient_RowDataBound">
            </asp:gridview>
                        </div>

                        <!-- Tab 2 -->
                        <div class="tab-pane fade" id="tab2" role="tabpanel">
                            <asp:gridview id="gvProgramsByStaff" runat="server" autogeneratecolumns="false" cssclass="gridview"
                                showheader="true" datakeynames="ClassName" onrowdatabound="gvProgramsByStaff_RowDataBound">
            </asp:gridview>
                        </div>

                        <!-- Tab 3 -->
                        <div class="tab-pane fade" id="tab3" role="tabpanel">
                            <asp:gridview id="gvClinicalByClient" runat="server" autogeneratecolumns="false" cssclass="gridview"
                                showheader="true" datakeynames="ClassName" onrowdatabound="gvClinicalByClient_RowDataBound">
            </asp:gridview>
                        </div>

                        <!-- Tab 4 -->
                        <div class="tab-pane fade" id="tab4" role="tabpanel">
                            <asp:gridview id="gvClinicalByStaff" runat="server" autogeneratecolumns="false" cssclass="gridview"
                                showheader="true" datakeynames="ClassName" onrowdatabound="gvClinicalByStaff_RowDataBound">
            </asp:gridview>
                        </div>
                    </div>
                </div>

                <!--olddashboard-view--end-->
            </div>
            <hr style="display:none; color:#1f497d;font-size:smaller;margin-top:-20px"/>
          
            <div class="main-dashboard" >
                 <div>
                    <table style="width: 100%">   
                        <tr>
                            <td style="text-align: center">
                                 <div id="prdiv" style="margin-left:-50px; position:absolute; height: 100%"">
                                    <rsweb:ReportViewer ID="RV_DBReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowToolBar="false" ShowWaitControlCancelLink="true"  Width="100%"  Height="100%" Visible="False" ShowParameterPrompts="False">
                                        <ServerReport ReportServerUrl="<%$ appSettings:ReportUrl %>" />
                                    </rsweb:ReportViewer>
                                 </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

        <!--olddashboard-student--start-->
        <div id="olddashboard-student" style="visibility: hidden;display:none"> 
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
        <!--olddashboard-student--end-->

        <!--olddashboard-teacher--start-->
        <div id="olddashboard-teacher" style="visibility: hidden;display:none"> 
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
        </div> 
        <div>
             <%--<asp:Label ID="lblNoData" runat="server" Text=""></asp:Label>--%>
            <div class="graph-grid">
                <div id="graphContainerAcademic" class="graph-container" runat="server"></div>
                <div id="graphContainerClinical" class="graph-container" runat="server"></div>
                <div id="graphContainerAcademicStaff" class="graph-container" runat="server"></div>
                <div id="graphContainerClinicalStaff" class="graph-container" runat="server"></div>
            </div>
        </div>
        <!--olddashboard-teacher--end-->
        <script>
            function loadClinicbyClient(classid, studentid) {
    var jsonData = JSON.stringify({ cid: classid, sid: studentid });
    $.ajax({
        type: "POST",
        url: "DashboardReportNew.aspx/getClientClinic",
        data: jsonData,
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        success: function (response) {
            var parsed = JSON.parse(response.d);
            if (parsed.length != 0) {
                var aData = parsed;
                var behav = [];
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
                var cat = JSON.parse(JSON.stringify(result));
                var dat = JSON.parse(JSON.stringify(seriesdata));
                var beh = JSON.parse(JSON.stringify(behav));
                var titletext = 'Clinical by Client';
                document.querySelector('.graph-grid').style.display = 'grid';
                drawChartClinic(cat, dat, beh,titletext);
                HideWait();
            }
            else {
                HideWait();
                //var nodata = document.getElementById('lblNoData');
                graphContainerClinical.innerHTML = "No Data Available";
            }
        },
        error: OnErrorCall_
    });

    function OnErrorCall_(response) {
        alert("Whoops something went wrong!");

    }
}
            function loadAcbyClient(classid, studentid, mistrial) {
                var jsonData = JSON.stringify({ classid: classid, studentid: studentid, mistrial: mistrial });
                $.ajax({
                    type: "POST",
                    url: "DashboardReportNew.aspx/getAClient",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: function (response) {
                        var parsed = JSON.parse(response.d);
                        if (parsed.length != 0) {
                            var aData = parsed;
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

                                            dt.push({ y: item['SessionCount'], name: item['LessonName'], ToolTip: item['LessonNameToolTip']});
                                        }
                                        else {
                                            dt.push({ y: 0, name: '' });
                                        }
                                        i = i + 1;
                                    }
                                    var daa = JSON.parse(JSON.stringify(dt));
                                    var seriesdict = new Object;
                                    seriesdict['name'] = item['LessonName'];
                                    seriesdict['data'] = daa;
                                    seriesdata.push(seriesdict);
                                    while (dt.length > 0) {
                                        dt.pop();
                                    }
                                }
                            });
                            var cat = JSON.parse(JSON.stringify(result));
                            var dat = JSON.parse(JSON.stringify(seriesdata));
                            drawChartAc(cat, dat);
                            HideWait();
                        }
                        else {
                            //var nodata = document.getElementById('lblNoData');
                            graphContainerAcademic.innerHTML = "No Data Available";
                            HideWait();
                        }
                        },

                    error: OnErrorCall_
                });
                function OnErrorCall_(response) {
                    alert("Whoops something went wrong!");

                }
            }
            function drawChartClinic(cat, da, be, titletext) {
                var intrvl = 1;
                var maxy = null;
                var ytext = '';
                if (titletext != 'Clinical by Client') {
                    intrvl = 10;
                    maxy = 100;
                }
                else {
                    ytext = 'Total Behaviors submitted';
                }
                Highcharts.chart('graphContainerClinical', {
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: titletext,
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
                        max: maxy,
                        tickInterval: intrvl,
                        opposite:true,
                        title: {
                            text: ytext,
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
                                    if (titletext != 'Clinical by Client') {
                                        return this.point.y + "%";
                                    } else {
                                        var pointWidth = this.point.shapeArgs.height;
                                        var serieI = this.series.index;
                                        var maxLabelLength = Math.floor(pointWidth / 8);
                                        if (pointWidth > (be[serieI].length*8)) {
                                            return be[serieI];
                                        }
                                        else {
                                            return be[serieI].substring(0, maxLabelLength);
                                        }
                                    }
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
                        enabled: titletext != 'Clinical by Client' ? false : true,
                        formatter: function () {
                            if (titletext != 'Clinical by Client') {
                                return this.point.ToolTip;
                            }
                            else {
                                var serieI = this.series.index;
                                return be[serieI];
                            }
                        }

                    },

                    series: da

                });

            }
            function drawChartAc(cat, da) {

                Highcharts.chart('graphContainerAcademic', {
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: 'Programs by Client'
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        categories: cat,
                        //   categories:['a','b'],
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
                        opposite:true,
                        title: {
                            text: 'Total Sessions',
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
                        bar: {
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    //return this.point.name;
                                    var label = this.point.ToolTip || '';
                                    var pointHeight = (this.point.shapeArgs && this.point.shapeArgs.height) ? this.point.shapeArgs.height : 20;
                                    var maxLabelLength = Math.floor(pointHeight / 8);

                                    if (label.length * 8 < pointHeight) {
                                        return label;
                                    } else {
                                        return label.substring(0, maxLabelLength);
                                    }
                                },
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '12px',
                                    fontFamily: 'Arial',
                                    color: 'black',
                                }
                            }
                        },
                        series: {
                            stacking: 'normal'
                        }

                    },
                    tooltip: {
                        enabled: true,
                        formatter: function () {
                            return this.point.ToolTip;
                        }
                    },
                    series: da
                });

            }
            function loadAcbyClientPerc(classid, studentid) {
                var jsonData = JSON.stringify({ classid: classid, studentid: studentid });
                $.ajax({
                    type: "POST",
                    url: "DashboardReportNew.aspx/getAClientPerc",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: function (response) {
                        var parsed = JSON.parse(response.d);
                        if (parsed.length != 0) {
                            var aData = parsed;
                            var categoriesdata = [];
                            var seriesdata = [];
                            var result = [];
                            tooltiparr = [];
                            $.map(aData, function (item, index) {
                                categoriesdata.push(item['StudentName']);
                            });
                            $.map(categoriesdata, function (item, index) {
                                if (!result.includes(item)) {
                                    result.push(item);
                                }
                            });
                            var len = result.length;
                            var percData = []; var avgscore = [];
                            result.forEach(function (item) {
                                var score = 0, count = 0;
                                var tooltip = '';
                                aData.forEach(function (it) {
                                    if (item == it.StudentName) {
                                        score = score + it.Percentage;
                                        count = count + 1;
                                        tooltip=it.LessonNameToolTip;
                                    }
                                });
                                var ydict = new Object;
                                ydict['y'] = score / count;
                                ydict['ToolTip'] = tooltip;
                                avgscore.push(ydict);
                            });
                            var seriesdict = new Object;
                            seriesdict['name'] = "Percentage";
                            seriesdict['data'] = avgscore;
                            seriesdict['colorByPoint'] = true;
                            percData.push(seriesdict);
                            var cat = JSON.parse(JSON.stringify(result));
                            var dat = JSON.parse(JSON.stringify(percData));
                            //document.querySelector('.graph-grid').style.display = 'block';
                            var titletext = 'Percentage of Block Scheduled Lessons';
                            drawChartClinic(cat, dat, tooltiparr,titletext);
                            HideWait();
                        }
                        else {
                            //var nodata = document.getElementById('lblNoData');
                            graphContainerClinical.innerHTML = "No Data Available";
                            HideWait();
                        }
                    },

                    error: OnErrorCall_
                });
                function OnErrorCall_(response) {
                    alert("Whoops something went wrong!");

                }
            
            }


            function loadAcademicbyStaff(acstaffdata)
            {
                var aData = JSON.parse(acstaffdata);
                if (aData.length != 0) {
                    aData = aData.reverse();
                    var categoriesdata = [];
                    var seriesdata = [];
                    var result = [];
                    var lessonname = []; var uniqueless = [];
                    $.map(aData, function (item, index) {
                        categoriesdata.push(item['StaffName']);
                    });
                    $.map(categoriesdata, function (item, index) {
                        if (!result.includes(item)) {
                            result.push(item);
                        }
                    });
                    $.map(aData, function (item, index) {
                        lessonname.push(item['LessonNameToolTip']);
                    });
                    $.map(lessonname, function (item, index) {
                        if (!uniqueless.includes(item)) {
                            uniqueless.push(item);
                        }
                    });
                    var colorDictionary = createDictionary(uniqueless);
                    var len = result.length;
                    $.map(aData, function (item, index) {
                        if (result.includes(item['StaffName'])) {
                            var index;
                            result.some(function (elem, inx) {
                                if (elem === item['StaffName']) {
                                    index = inx;
                                }
                            });
                            var dt = [];
                            var i = 0;
                            while (i < len) {
                                if (i == index) {
                                    var key = item['LessonNameToolTip'];
                                    var color = colorDictionary[key];
                                    dt.push({ y: item['SessionCount'], name: item['LessonName'], ToolTip: item['LessonNameToolTip'] + '(' + item['StudentName'] + ')', color: color });
                                }
                                else {
                                    dt.push({ y: 0, name: '' });
                                }
                                i = i + 1;
                            }
                            var daa = JSON.parse(JSON.stringify(dt));
                            var seriesdict = new Object;
                            seriesdict['name'] = item['LessonName'];
                            seriesdict['data'] = daa;
                            seriesdata.push(seriesdict);
                            while (dt.length > 0) {
                                dt.pop();
                            }
                        }
                    });
                    var cat = JSON.parse(JSON.stringify(result));
                    var dat = JSON.parse(JSON.stringify(seriesdata));
                    DrawAcStaff(cat, dat);
                    HideWait();
                }
                else {
                    HideWait();
                    //var nodata = document.getElementById('lblNoData');
                    graphContainerAcademicStaff.innerHTML = "No Data Available";
                }
            }
            function DrawAcStaff(cat, da) {

                Highcharts.chart('graphContainerAcademicStaff', {
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: 'Programs by Staff'
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        categories: cat,
                        title: {
                            text: 'Staff Name',
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
                        opposite: true,
                        title: {
                            text: 'Total Sessions',
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
                    tooltip: {
                        formatter: function () {
                            return this.point.ToolTip;
                        }
                    },
                    plotOptions: {
                        bar: {
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    var label = this.point.ToolTip || '';
                                    var pointHeight = (this.point.shapeArgs && this.point.shapeArgs.height) ? this.point.shapeArgs.height : 20;
                                    var maxLabelLength = Math.floor(pointHeight / 8);

                                    if (label.length * 8 < pointHeight) {
                                        return label;
                                    } else {
                                        return label.substring(0, maxLabelLength);
                                    }
                                },
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '12px',
                                    fontFamily: 'Arial',
                                    color: 'black',
                                }
                            }
                        },
                        series: {
                            stacking: 'normal'
                        }

                    },
                    series: da
                });

            }
            function createDictionary(uniqueless) {
                var dictionary = {};
                for (var i = 0; i < uniqueless.length; i++) {
                    var item = uniqueless[i];
                    dictionary[item] = generateRandomColor();
                }

                return dictionary;
            }
            function generateRandomColor() {
                var randomColor = '#' + Math.floor(Math.random() * 16777215).toString(16);
                return randomColor;
            }
            function loadClinicalbyStaff(clstaffdata) {
                var aData = JSON.parse(clstaffdata);
                if (aData.length != 0) {
                    aData = aData.reverse();
                    var categoriesdata = [];
                    var seriesdata = [];
                    var result = [];
                    var lessonname = []; var uniqueless = [];
                    $.map(aData, function (item, index) {
                        categoriesdata.push(item['StaffName']);
                    });
                    $.map(categoriesdata, function (item, index) {
                        if (!result.includes(item)) {
                            result.push(item);
                        }
                    });
                    $.map(aData, function (item, index) {
                        lessonname.push(item['BehaviorNameToolTip']);
                    });
                    $.map(lessonname, function (item, index) {
                        if (!uniqueless.includes(item)) {
                            uniqueless.push(item);
                        }
                    });
                    var colorDictionary = createDictionary(uniqueless);
                    var len = result.length;
                    $.map(aData, function (item, index) {
                        if (result.includes(item['StaffName'])) {
                            var index;
                            result.some(function (elem, inx) {
                                if (elem === item['StaffName']) {
                                    index = inx;
                                }
                            });
                            var dt = [];
                            var i = 0;
                            while (i < len) {
                                if (i == index) {
                                    var key = item['BehaviorNameToolTip'];
                                    var color = colorDictionary[key];
                                    dt.push({ y: item['MeasurementCount'], name: item['BehaviourName'], ToolTip: item['BehaviorNameToolTip'] + '(' + item['StudentName'] + ')', color: color });
                                }
                                else {
                                    dt.push({ y: 0, name: '' });
                                }
                                i = i + 1;
                            }
                            var daa = JSON.parse(JSON.stringify(dt));
                            var seriesdict = new Object;
                            seriesdict['name'] = item['BehaviourName'];
                            seriesdict['data'] = daa;
                            seriesdata.push(seriesdict);
                            while (dt.length > 0) {
                                dt.pop();
                            }
                        }
                    });
                    var cat = JSON.parse(JSON.stringify(result));
                    var dat = JSON.parse(JSON.stringify(seriesdata));
                    DrawClStaff(cat, dat);
                    HideWait();
                }
                else {
                    HideWait();
                    //var nodata = document.getElementById('lblNoData');
                    graphContainerClinicalStaff.innerHTML = "No Data Available";
                }
            }
            function DrawClStaff(cat, da) {

                Highcharts.chart('graphContainerClinicalStaff', {
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: 'Clinical by Staff'
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        categories: cat,
                        title: {
                            text: 'Staff Name',
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
                        opposite: true,
                        title: {
                            text: 'Total Behaviors submitted',
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
                    tooltip: {
                        formatter: function () {
                            return this.point.ToolTip;
                        }
                    },
                    plotOptions: {
                        bar: {
                            dataLabels: {
                                enabled: true,
                                formatter: function () {
                                    if (this.point.y === 0) return null;
                                    var label = this.point.ToolTip || '';
                                    var pointHeight = (this.point.shapeArgs && this.point.shapeArgs.height) ? this.point.shapeArgs.height : 20;
                                    var maxLabelLength = Math.floor(pointHeight / 8);

                                    if (label.length * 8 < pointHeight) {
                                        return label;
                                    } else {
                                        return label.substring(0, maxLabelLength);
                                    }
                                    //return this.point.name;
                                },
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '12px',
                                    fontFamily: 'Arial',
                                    color: 'black',
                                }
                            }
                        },
                        series: {
                            stacking: 'normal'
                        }

                    },
                    series: da
                });

            }
        </script>
    </form>
</body>
</html>
