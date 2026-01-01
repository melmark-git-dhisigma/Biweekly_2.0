<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/LessonReportsWithPaging.aspx.cs" Inherits="StudentBinder_LessonReportsWithPaging" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="../Administration/CSS/jsDatePickforGraph.css" rel="stylesheet" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js"></script>
    <script src="../Administration/JS/jquery.min.js"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js"></script>
 <script src="../Scripts/highcharts/7.1.2/highcharts.js"></script>
   <script src="../Scripts/highcharts/7.1.2/modules/accessibility.js"></script>
    <script src="../Scripts/highcharts/7.1.2/modules/xrange.js"></script>
    <script src="../Scripts/highcharts/7.1.2/modules/exporting.js"></script>
        <script src="../Scripts/html2canvas.min.js"></script>
            <script src="../Scripts/es6-promise.auto.min.js"></script>
        <script src="../Scripts/es6-promise.min.js"></script>
        <script src="../Scripts/jsPDF.js"></script>


            <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
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


        .styleBorder {
            border:none;
        }
        .block { 
            display:inline-block;
            vertical-align:top;
            overflow-x:scroll;
            overflow-y:scroll;
            border:solid grey 1px;
            width:300px;
            height:150px;
        }
         .block select { 
            padding:10px; 
            margin:0px 0px 0px -10px;
        }
        #AcademicGraphReports div {
            height: auto !important;
            /*overflow: visible !important;*/
        }

        .divGrid {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: auto;
            height: auto;
        }

        .divGrid1 {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: auto;
            height: auto;
            display: none;
        }

        .divBackgrnd {
            padding: 26px 16px 16px 16px;
            width: 90%;
            height: 400px;
            overflow-y: scroll;
            overflow-x: hidden;
            -webkit-border-radius: 24px 24px 24px 24px;
            -moz-border-radius: 24px 24px 24px 24px;
            border-radius: 24px 24px 24px 24px;
            background: rgba(87,197,239,0.2);
            -webkit-box-shadow: #68A1B3 8px 8px 8px;
            -moz-box-shadow: #68A1B3 8px 8px 8px;
            box-shadow: #68A1B3 8px 8px 8px;
        }

        .pnlCSS {
        }
        /* FOR LOADING IMAGE AT PAGE LOAD */
        .loading {
            display: block;
            position: absolute;
            width: 100%;
            height: 800px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            background-image: url("../Administration/images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
        }

        #fullContents {
            display: none;
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

        .web_dialog {
            background: url("../images/smalllgomlmark.JPG") no-repeat scroll right bottom #F8F7FC;
            border: 5px solid #B2CCCA;
            color: #333333;
            display: block;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 100%;
            height: auto;
            left: 30%;
            top: 5%;
            margin-left: -190px;
            padding: 5px 5px 30px;
            position: fixed;
            display: none;
            width: 800px;
            z-index: 102;
        }


        .web_dialog_overlay {
            background: none repeat scroll 0 0 #000000;
            bottom: 0;
            display: none;
            height: 100%;
            left: 0;
            margin: 0;
            opacity: 0.15;
            padding: 0;
            position: fixed;
            right: 0;
            top: 0;
            width: 100%;
            z-index: 101;
        }

        /*LOADING IMAGE CLOSE */
    </style>
    <script type="text/javascript">
        //$(window).load(function () {

        //});

        //$("#RV_LPReport").css("height", $(window).height());
        $(document).ready(function () {
            $('.loading').fadeOut('slow', function () {
                $('#fullContents').fadeIn('fast');
            });

            $("#chkrepevents").toggle(
         function () {
             if ($('#<%=chkrepevents.ClientID %>').is(':checked')) {
                 $('#<%=chkrepmajor.ClientID %>').attr('checked', true);
                 $('#<%=chkrepminor.ClientID %>').attr('checked', true);
                 $('#<%=chkreparrow.ClientID %>').attr('checked', true);

             }
             else {
                 $('#divevents').slideToggle("slow");
             }
         });



            $("#chkbxevents").toggle(
       function () {
           if ($('#<%=chkbxevents.ClientID %>').is(':checked')) {
               $('#<%=chkbxmajor.ClientID %>').attr('checked', true);
               $('#<%=chkbxminor.ClientID %>').attr('checked', true);
               $('#<%=chkbxarrow.ClientID %>').attr('checked', true);

           }
           else {
               $('#divpopupEvents').slideToggle("slow");
           }
       });

            $("#chkrepevents").change(function () {
                if (this.checked) {
                    $('#divevents').slideToggle("slow");
                    $('#<%=chkrepmajor.ClientID %>').attr('checked', true);
                    $('#<%=chkrepminor.ClientID %>').attr('checked', true);
                    $('#<%=chkreparrow.ClientID %>').attr('checked', true);
                }
                else {
                    $('#divevents').slideToggle("slow");
                    $('#<%=chkrepmajor.ClientID %>').attr('checked', false);
                    $('#<%=chkrepminor.ClientID %>').attr('checked', false);
                    $('#<%=chkreparrow.ClientID %>').attr('checked', false);
                }
            });

            $("#chkbxevents").change(function () {
                if (this.checked) {
                    $('#divpopupEvents').slideToggle("slow");
                    $('#<%=chkbxmajor.ClientID %>').attr('checked', true);
                    $('#<%=chkbxminor.ClientID %>').attr('checked', true);
                    $('#<%=chkbxarrow.ClientID %>').attr('checked', true);
                }
                else {
                    $('#divpopupEvents').slideToggle("slow");
                    $('#<%=chkbxmajor.ClientID %>').attr('checked', false);
                    $('#<%=chkbxminor.ClientID %>').attr('checked', false);
                    $('#<%=chkbxarrow.ClientID %>').attr('checked', false);
                }
            });


            $('#<%=btnExport.ClientID %>').click(function () {
                $('.loading').fadeIn('fast');
            });


        });

        function repevent() {
            $('#<%=chkrepevents.ClientID %>').attr('checked', false);

            if ($('#<%=chkrepmajor.ClientID %>').is(':checked') && $('#<%=chkrepminor.ClientID %>').is(':checked') && $('#<%=chkreparrow.ClientID %>').is(':checked')) {
                $('#<%=chkrepevents.ClientID %>').attr('checked', true);
            }
        }


        function eventinpopup() {
            $('#<%=chkbxevents.ClientID %>').attr('checked', false);

            if ($('#<%=chkbxmajor.ClientID %>').is(':checked') && $('#<%=chkbxminor.ClientID %>').is(':checked') && $('#<%=chkbxarrow.ClientID %>').is(':checked')) {
                $('#<%=chkbxevents.ClientID %>').attr('checked', true);
            }
        }


        $(function () {
            adjustStyle();

        });

        

        function adjustStyle() {
            var isiPad = navigator.userAgent.match(/iPad/i);
            if (isiPad != null) {
                $('#graphPopup').css('width', '91% !Important');
                //$('#graphPopup').css('left', ' 100px !Important');//
                //$('img').css('width', '80%');
                //$('#RV_LPReport').css('width', '90%');
            }
            else {
                $('#graphPopup').css('width', '63% !Important');
                //style = "width: 63%;"
            }

        }
        function warningmsgpop() {
            $('#graphPopup').show();
            $('#overlay').show();
            var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
            hchart.style.display = 'none';
            var popup = document.getElementById('popup');
            popup.style.display = 'none';
        }
        function showPopups() {
            document.getElementById("hfPopUpValue").value = true;
            $('#graphPopup').hide();
            $('#overlay').hide();
            var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
             hchart.style.display = 'none';
             var popup = document.getElementById('popup');
             popup.style.display = 'block';
         }
         function showPopup() {
             $('#graphPopup').hide();
             $('#overlay').hide();
             var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
             hchart.style.display = 'none';
             var popup = document.getElementById('popup');
             popup.style.display = 'block';
         }

         function closePopup() {
             var popup = document.getElementById('popup');
             popup.style.display = 'none';
             var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
             hchart.style.display = 'block';
             $('#graphPopup').hide();
             $('#overlay').hide();
         }
        function loadWait() {
            $('.loading').fadeIn('fast');//, function () { });
        }
        function HideWait() {
            $('.loading').fadeOut('fast');//, function () { });
        }


        function PrintDivData(elementId) {

            var printContents = document.getElementById(elementId).innerHTML;
            var originalContents = document.body.innerHTML;

            document.body.innerHTML = printContents;

            window.print();

            document.body.innerHTML = originalContents;
        }
        $(document).ready(function () {
            
            $('#drpSetname_sl').css('width', '250px');
            $('#drpColumn_sl').css('width', '250px');
            var selecteddata = [];
            $("[id*=drpSetname] input:checked").each(function () {
                selecteddata.push($(this).next().html());
            });
           
            $("#drpSetname_sl").change(function () {
                var selectedValues = [];
                $("[id*=drpSetname] input:checked").each(function () {
                    selectedValues.push($(this).next().html());
                });
                var $ctrls = $("[id*=drpSetname]");
                if (selectedValues.length > 0) {                   
                    if (selectedValues == "Normal Graph View") {
                        for (var i = 1; i < selectedValues.length; i++) {
                            $ctrls.find('label:contains("' + selectedValues[i] + '")').prev().prop('checked', false);
                        }
                        $ctrls.find('label:contains("Normal Graph View")').prev().prop('checked', true);
                    }
                    else if (selectedValues != "Normal Graph View") {
                        if (selecteddata == "Normal Graph View") {
                            selecteddata = "";
                            for (var i = 1; i < selectedValues.length; i++) {
                                $ctrls.find('label:contains("' + selectedValues[i] + '")').prev().prop('checked', true);                            
                                 selecteddata[i-1]=selectedValues[i];
                                                            }
                            $ctrls.find('label:contains("Normal Graph View")').prev().prop('checked', false);
                        }
                        else {
                            if ($ctrls.find('label:contains("Normal Graph View")').prev().prop('checked') == true) {
                                selecteddata = "";
                                selecteddata = "Normal Graph View";
                                for (var i = 1; i < selectedValues.length; i++) {
                                    $ctrls.find('label:contains("' + selectedValues[i] + '")').prev().prop('checked', false);
                                }
                            }
                        }
                    }
                }
                else {
                    $ctrls.find('label:contains("Normal Graph View")').prev().prop('checked', true);
                }
                
            });

        });
        function showEvents() {
            if ($('#<%=chkbxevents.ClientID %>').is(':checked')) {
                  //$('#divpopupEvents').slideToggle("slow");
                  $('#<%=chkbxmajor.ClientID %>').attr('checked', true);
                    $('#<%=chkbxminor.ClientID %>').attr('checked', true);
                    $('#<%=chkbxarrow.ClientID %>').attr('checked', true);
                }
                else {
                    $('#divpopupEvents').slideToggle("slow");
                    $('#<%=chkbxmajor.ClientID %>').attr('checked', false);
                    $('#<%=chkbxminor.ClientID %>').attr('checked', false);
                    $('#<%=chkbxarrow.ClientID %>').attr('checked', false);
                }
                if ($('#<%=chkbxevents.ClientID %>').is(':checked')) {
                  //$('#divpopupEvents').slideToggle("slow");
              }
              else {
                  $('#<%=chkbxmajor.ClientID %>').attr('checked', false);
                $('#<%=chkbxminor.ClientID %>').attr('checked', false);
                $('#<%=chkbxarrow.ClientID %>').attr('checked', false);
            }
        }


    </script>

    <script type="text/javascript">


        function HideAndDisplay() {
            btnsubmitVisibility();
            var val = document.getElementById("hfPopUpValue").value;
            var valLesson = document.getElementById("hdnallLesson").value;
            if (valLesson == "AllLessons") {
                $('#graphPopup').fadeIn();
            }
        }
        window.onload = function () {

            var val = document.getElementById("hfPopUpValue").value;
            var valLesson = document.getElementById("hdnallLesson").value;
            var valExport = document.getElementById("hdnExport").value;
            if (valLesson == "AllLessons") {
                $('#btnRefresh').css("display", "block");
                $('#Button7').css("display", "none");
                $('#btnLessonSubmit').css("display", "none");
            }
            else {
                $('#btnRefresh').css("display", "none");
                $('#Button7').css("display", "block");
                $('#btnLessonSubmit').css("display", "block");
            }
            if (val != "true") {
                if (valLesson == "AllLessons") {
                    $('#overlay').fadeIn('slow', function () {
                        $('#graphPopup').fadeIn('fast');
                    });
                    $('#close_x').click(function () {
                        $('#graphPopup').fadeOut('slow', function () {
                            $('#overlay').fadeOut('slow');
                        });
                    });
                }
                else {
                    $('#graphPopup').hide();
                    $('#overlay').hide();
                }
            }
            if (val == "true") {
                //$('#overlay').fadeIn('slow', function () {
                //    $('#graphPopup').fadeIn('fast');
                //    $('#tblListbox').css("display", "none");
                //});
                $('#graphPopup').hide();
                $('#overlay').hide();
            }
            $('#close_x').click(function () {
                $('#graphPopup').fadeOut('slow', function () {
                    $('#overlay').fadeOut('slow');
                });
            });


            new JsDatePick({
                useMode: 2,
                target: "<%=txtSdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtEdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtRepStart.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtrepEdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });

            if (valExport == "true") {
                $('#overlay').show();
            }

        };

        //----------------alphonsa-------------------//
        function disp() {

            new JsDatePick({
                useMode: 2,
                target: "<%=txtSdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
                        });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtEdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtRepStart.ClientID%>",
                dateFormat: "%m/%d/%Y",
                        });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtrepEdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
        }
        //-------------------------------------------//

        function DownloadPopup() {
            $('.loading').fadeOut('fast');
            $('#overlay').fadeIn('fast', function () {
                $('#downloadPopup').fadeIn('fast');
            });
        }

        function CloseDownload() {
            document.getElementById("hdnExport").value = "";
            $('#overlay').fadeOut('fast', function () {
                $('#downloadPopup').fadeOut('fast');
            });
        }


    </script>
    <script>
        function allowBackSpace(e, obj) {
            var evt = e || window.event;
            if (evt.keyCode == 8) {
                obj.value = '';
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfPopUpValue" runat="server" />

        <div class="loading">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
        <div id="popup" class="popup" >
    <div class="popup-content" id="popup-content">
        <p>Please wait...</p>
    </div>
</div>
        <div id="fullContents" style="width: 98%; margin-left: 1%">
            <div>
                <table style="width: 100%">


                    <tr>
                        <td style="text-align: center"></td>
                        <td style="text-align: center">


                            <asp:HiddenField ID="hdnallLesson" runat="server" />


                        </td>
                        <td style="text-align: center">


                            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut= "300">
                            </asp:ScriptManager>
                        </td>
                        <td></td>
                    </tr>

                </table>
                <div runat="server" id="LessonDiv" visible="false">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="4" id="tdMsg1" runat="server"></td>
                        </tr>

                        <tr>
                            <td style="text-align: center; width: 25%" class="tdText">Report
                Start Date
                                <br />
                                <br />
                                <br />
                                Report End Date
                            </td>
                            <td style="text-align: left; width: 25%">
                                <asp:TextBox ID="txtRepStart" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                <br />
                                <br />
                                <asp:TextBox ID="txtrepEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                            </td>
                            <td style="text-align: center" class="tdText"></td>
                            <td style="text-align: left; width: 25%" rowspan="2">

                                <asp:CheckBox ID="chkrepevents" runat="server" Text="Display All Events" AutoPostBack="false"></asp:CheckBox>
                                <br />
                                <div id="divevents" style="display: none">
                                    <asp:CheckBox ID="chkrepmajor" runat="server" Text="Display Major Events" onclick="repevent();"></asp:CheckBox>
                                    <br />
                                    <asp:CheckBox ID="chkrepminor" runat="server" Text="Display Minor Events" onclick="repevent();"></asp:CheckBox>
                                    <br />
                                    <asp:CheckBox ID="chkreparrow" runat="server" Text="Display Arrow Notes" onclick="repevent();"></asp:CheckBox>
                                    <br />
                                </div>

                                <asp:CheckBox ID="chkrepioa" runat="server" Text="Include IOA"></asp:CheckBox>
                                <br />
                                <asp:CheckBox ID="chkreptrend" runat="server" Text="Include Trend Line"></asp:CheckBox>
                                <br />
                                <asp:CheckBox ID="chkrepmedi" runat="server" Text="Include Medication"></asp:CheckBox>
                                <br />


                            </td>
                            <td style="text-align: left; width: 25%" rowspan="2">
                                <label></label>
                                <%-- <asp:DropDownList id="drpSetname" style="width:207px;" runat="server">
                                    <asp:ListItem Value="0">....Select....</asp:ListItem>
                                </asp:DropDownList>--%>
                                <fieldset>
                                <legend >
                                <asp:CheckBox  class="chb" Text ="Maintenance Only" id="rbtnmainonly" runat="server"  />
                                </legend>
                                <table style = "width: 100%">
                                <tr>
                                <td>  <asp:DropDownCheckBoxes ID="drpSetname" Width="250px" UseSelectAllNode="false"  runat="server"  AddJQueryReference="true">
                                    <Texts SelectBoxCaption="---------- Select Maintenance Set(s) ----------" />                                       
                                </asp:DropDownCheckBoxes>
                                </td>
                                </tr>
                                <tr><td>
                                <asp:DropDownCheckBoxes ID="drpColumn" Width="250px" UseSelectAllNode="true"  runat="server" >
                                    <Texts SelectBoxCaption="---------- Select Column ----------" />
                                </asp:DropDownCheckBoxes> 
                                </td>
                                </tr>
                                </table>
                                </fieldset>

                                <asp:RadioButtonList ID="rbtnClassType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Day">Day</asp:ListItem>
                                    <asp:ListItem Value="Residence">Residence</asp:ListItem>
                                    <asp:ListItem Value="Day,Residence" Selected="True">Both</asp:ListItem>
                                </asp:RadioButtonList>

                                <asp:RadioButtonList ID="rbtnIncidentalRegular" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Incidental">Incidental Teaching Graph</asp:ListItem>
                                    <asp:ListItem Value="Regular" Selected="True">Regular Graph</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>


                    </table>
                </div>


                <div id="overlay" class="web_dialog_overlay">
                </div>







                <div id="graphPopup" runat="server" class="web_dialog" >
                    <div>

                        <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                        <asp:UpdatePanel ID="updtepnllesson" runat="server">
                             <Triggers>
                <asp:PostBackTrigger ControlID="btnsubmit" />
            </Triggers>
                            <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td colspan="4" runat="server" id="tdMsg"></td>
                            </tr>
                            <tr>
                                <td style="text-align: center" class="tdText">Report
                Start Date
                                     <br />
                                    <br />
                                    <br />
                                    Report End Date
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtSdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                    <br />
                                    <br />

                                    <asp:TextBox ID="txtEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                </td>
                                <td style="text-align: center" class="tdText"></td>
                                <td style="text-align: left" rowspan="2">

                                    <asp:CheckBox ID="chkbxevents" runat="server" Text="Display Events" OnCheckedChanged="EvntCheckBox_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                    <br />
                                    <div id="divpopupEvents" style="display: none">
                                        <asp:CheckBox ID="chkbxmajor" runat="server" Text="Display Major Events" onclick="eventinpopup();"></asp:CheckBox>
                                        <br />
                                        <asp:CheckBox ID="chkbxminor" runat="server" Text="Display Minor Events" onclick="eventinpopup();"></asp:CheckBox>
                                        <br />
                                        <asp:CheckBox ID="chkbxarrow" runat="server" Text="Display Arrow Notes" onclick="eventinpopup();"></asp:CheckBox>
                                        <br />
                                    </div>

                                    <asp:CheckBox ID="chkbxIOA" runat="server" Text="Include IOA"></asp:CheckBox>
                                    <br />
                                    <asp:CheckBox ID="chktrend" runat="server" Text="Include Trend Line"></asp:CheckBox>
                                    <br />
                                    <asp:CheckBox ID="chkmedication" runat="server" Text="Include Medication"></asp:CheckBox>
                                    <br />
                                    <asp:RadioButtonList ID="rbtnClassTypeall" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="Day">Day</asp:ListItem>
                                        <asp:ListItem Value="Residence">Residence</asp:ListItem>
                                        <asp:ListItem Value="Day,Residence" Selected="True">Both</asp:ListItem>
                                    </asp:RadioButtonList>

                                    <asp:RadioButtonList ID="rbtnIncidentalRegularall" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="Incidental">Incidental Teaching Graph</asp:ListItem>
                                        <asp:ListItem Value="Regular" Selected="True">Regular Graph</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="text-align: left; vertical-align: top;" rowspan="2">

                                    <%--<asp:Button ID="Button1" runat="server" Text="Execute" OnClick="Button1_Click" CssClass="NFButton" />--%>
                                    <asp:Button ID="btnsubmit" runat="server" Text="" CssClass="showgraph" ToolTip="Show Graph" OnClick="btnsubmit_Click" Style="float: right;" OnClientClick="javascript:loadWait();" />
                                     <asp:Button ID="btnsubmith" runat="server" Text="" CssClass="showgraph" ToolTip="Show Graph" OnClick="btnsubmit_Click" Style="float: right; display: none;" OnClientClick="javascript:showPopup();" />
                                    <asp:CheckBox id="highcheck" runat="server" Text="" onchange="btnsubmitVisibility();"></asp:CheckBox>
                                </td>  
                               
                            </tr>

                            <tr>
                                <td style="text-align: center" class="tdText"></td>
                                <td style="text-align: center"> <asp:CheckBoxList ID="chkStatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="chkStatus_SelectedIndexChanged">
                                <asp:ListItem Selected="True">Active</asp:ListItem>
                                <asp:ListItem Selected="True">Maintenance</asp:ListItem>
                                <asp:ListItem>Inactive</asp:ListItem>
                            </asp:CheckBoxList></td>
                                <td style="text-align: center" class="tdText">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: center;" class="tdText" colspan="5">
                                    <table style="width: 100%; text-align: center;">
                                        <tr>
                                            <td style="text-align: right;">
                                                <div class="block"> 
                                               <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple" Width="500px" Font-Names="Arial Narrow" CssClass="styleBorder" ></asp:ListBox>
                                            </div>
                                                    </td>
                                            <td>
                                                <asp:Button ID="Button2" CssClass="NFButton" runat="server" Text=">" OnClick="Button11_Click" />
                                                <br />
                                                <br />
                                                <asp:Button ID="Button3" CssClass="NFButton" runat="server" Text=">>" OnClick="Button2_Click" />
                                                <br />
                                                <br />
                                                <asp:Button ID="Button4" CssClass="NFButton" runat="server" Text="<" OnClick="Button3_Click" />
                                                <br />
                                                <br />
                                                <asp:Button ID="Button5" CssClass="NFButton" runat="server" Text="<<" OnClick="Button4_Click" />
                                            </td>
                                            <td style="text-align: left;">
                                                <div class="block"> 
                                                <asp:ListBox ID="ListBox2" runat="server" SelectionMode="Multiple" Width="500px" Font-Names="Arial Narrow" CssClass="styleBorder"></asp:ListBox>
                                           <//div> 
                                                     </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lbltxt" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                    </div>
                </div>

                <div id="downloadPopup" runat="server" class="web_dialog" style="width: 600px;">

                    <div id="Div53" style="width: 700px;">


                        <table style="width: 97%">
                            <tr>
                                <td colspan="2">
                                    <table style="width: 85%">
                                        <tr>
                                            <td runat="server" id="tdMsgExport" style="height: 50px"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" OnClientClick="CloseDownload();" />
                                </td>
                                <td style="text-align: left">
                                   <%-- <input type="button" value="Done" class="NFButton" id="btnDone" onclick="CloseDownload();" />--%>
                                      <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton"  onclientclick="CloseDownload();" onClick="btnDone_Click"/>

                                </td>
                                     
                            </tr>
                        </table>

                    </div>
                </div>
            </div>
            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: right">
                             <asp:UpdatePanel ID="dropdownpanel" runat="server">
                                    <ContentTemplate>
                            <asp:Button ID="btnPrevious" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0px 250px" Visible="true" Text="Previous" OnClick="btnPrevious_Click" OnClientClick="javascript:showPopup();"/>
                               
                                        <asp:DropDownList ID="ddlLessonplan" runat="server" CssClass="drpClass" Style="float: left; margin: 0 1px 0px 10px" Height="26px" Width="290px" AutoPostBack="true" OnSelectedIndexChanged="ddlLessonplan_SelectedIndexChanged"  EnableViewState="true" onchange="showPopup()">
                             </asp:DropDownList>
                                                            
                             <asp:Button ID="btnNext" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0 10px" Visible="true" Text="Next"  OnClick="btnNext_Click" OnClientClick="javascript:showPopup();"/>
                               </ContentTemplate>
                                </asp:UpdatePanel> 
                            <%--<asp:Button ID="Button7" style="float:right;margin:0 1px 0 1px" runat="server" Text="Execute" OnClick="Button1_Click" CssClass="NFButton" />--%>
                            <input type="button" id="btnRefresh" style="float: right; margin: 0 1px 0 1px" class="refresh" onclick="HideAndDisplay()" />
                            <asp:Button ID="btnLessonSubmit" runat="server" class="showgraph" Style="float: right; margin: 0 1px 0 1px" Text="" CssClass="showgraph" OnClick="btnLessonSubmit_Click" />
                            <asp:Button ID="btnExport" runat="server" Style="float: right; margin: 0 1px 0 1px" ToolTip="Export To PDF" CssClass="pdfPrint" OnClick="btnExport_Click" />                            
                            <asp:Button ID="btnPrint" runat="server" Style="float: right; margin: 0 1px 0 1px" CssClass="print" ToolTip="Print" OnClick="btnPrint_Click" Visible="false" />
                            <%-- <input id="Button6" type="button" name="Print" style="float:right;margin:0 1px 0 1px" class="pdfPrint" onclick="javascript: PrintDivData('AcademicGraphReports');" />--%>
                           



                            <%-- <input type="image" style="border-width: 0px; float: right" onclick="HideAndDisplay()" src="../Administration/images/RefreshStudentBinder.png" value="Refresh" id="btnRefresh" name="btnRefresh" />--%>
                        </td>
                        <td style="width: 10%"></td>
                    </tr>
                </table>
            </div>
             <asp:UpdatePanel ID="reportpanel" runat="server">
                                    <ContentTemplate>
            <div style="text-align: center; width: 100%;"   runat="server">                
                <table style="width: 100%">

                    <tr>

                        <td style="text-align: center">
                            <div style="overflow: visible; width: 100%;" id="AcademicGraphReports">
                                <rsweb:ReportViewer ID="RV_LPReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="false" ShowCredentialPrompts="false" ShowDocumentMapButton="true" ShowExportControls="false" ShowFindControls="false" ShowPageNavigationControls="true" ShowParameterPrompts="true" ShowPrintButton="false" ShowPromptAreaButton="true" ShowRefreshButton="false" ShowToolBar="true" ShowWaitControlCancelLink="true" ShowZoomControl="false" SizeToReportContent="true" Width="100%" Visible="false" AsyncRendering="true">

                                    <ServerReport ReportServerUrl="<%$ appSettings:ReportUrl %>" />

                                    <%--    ReportPath="<%$ appSettings:ReportPath %>"--%>
                                </rsweb:ReportViewer>
                            </div>
                        </td>
                    </tr>
                </table>               
            </div>
        </div>
                                        </ContentTemplate>
                                </asp:UpdatePanel> 
      <asp:UpdatePanel ID="highchartupdate" runat="server">
                                    <ContentTemplate>
         <div id="HighchartGraph" runat="server" style="background-color: white;">
               <center>
                <br />
            <asp:label id="sname" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;" visibility="false"></asp:label>
                <br />
            <asp:label id="lnam" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;" visibility="false" ></asp:label>
                <br />
            <asp:label id="daterang" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;" visibility="false"></asp:label>
                </center>
         <%-- <asp:label id="treat" runat="server" text=""></asp:label>--%>
            <center>
           <asp:label runat="server" text="" id="mednodata"  Font-Bold="true" font-size="15px"></asp:label>
                <br />
           <div id = "medcont" runat="server"  style = "width: 935.43307pt; margin: 0 auto"></div>
                <br />
                <asp:Label ID="trtmnt" runat="server" Text="" Font-Bold="true" visibility="false" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:Label>                <br />
                <asp:Label ID="lbgraph" runat="server" Text="" Font-Bold="true" font-size="18px"></asp:Label>
                <span id="lbgraphSpan"></span>
                <br />
            <div id = "cont" runat="server"  style = "width: 935.43pt; height:688.901215752pt; margin: 0 auto;background-color: white;"></div>
                <br /><br /><br />
        </center>
             <div style = "margin-left:275px;width: 1280px;"><asp:label id="deftxt" runat="server" text="" style ="font-family: Arial; font-size: 12px;color:black;margin-left:150px;"  Font-Bold="true"></asp:label></div>
                
                <br />
       <asp:label id="mel" runat="server" text="" Font-Bold="true" style ="font-family: Arial; font-size: 12px;color:black;margin-left:1300px;"></asp:label>
                </div>
        </div>
                                         </ContentTemplate>
                                </asp:UpdatePanel> 
        <input type="hidden" id="reloadhid" runat="server" />

        <asp:HiddenField ID="hdnType" runat="server" Value="" />
        <asp:HiddenField ID="hdnExport" runat="server" Value="" />
         <script>

             function btnsubmitVisibility() {
                 var checkbox = document.getElementById('<%= highcheck.ClientID %>');
                  var button1 = document.getElementById('<%= btnsubmit.ClientID %>');
                  var button2 = document.getElementById('<%= btnsubmith.ClientID %>');
                  if (checkbox.checked) {
                      button1.style.display = 'none';
                      button2.style.display = 'block';
                  } else {
                      button1.style.display = 'block';
                      button2.style.display = 'none';
                  }
              }

             var chart1, chart2;
             var def = '';
             var titlename = '';
             var titlelesson = '';
             var titledate = '';
             var medtitle = '';
             var maintitle = '';
             var sizeOffont = '12px';
             function loadchart(sDate, eDate, sid, lid, scid, evnt, trend, ioa, cls, med, lpstatus, medno, reptype, inctype, medhead) {
                 var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
                 hchart.style.display = 'block'; 
                         var treatment = '';
                 //medication report size start
                         if (medno == "false" || medno == "False") {
                     var meddiv = document.getElementById('medcont');
                             if (med == "true" || med == "True") {
                                 if (reptype == "true" || reptype == "True") {
                             meddiv.style.width = '671.976pt';
                             meddiv.style.height = '87.807911808pt';

                 }
                         else {
                             meddiv.style.width = '935.43307pt';
                             meddiv.style.height = '164.05358746pt';
                         }
                     }
                 }
                 // medication report size end
                 // set cont size start
                 var myDiv = document.getElementById('cont');
                         if (reptype == "true" || reptype == "True") {
                     myDiv.style.width = '671.976pt';
                     myDiv.style.height = '510.2364pt';
                 } else {
                             myDiv.style.width = '935.43307pt';
                     myDiv.style.height = '688.901215752pt';
                 }
                 // set cont size End

                 // chart student name, class, date and type set start
                         var lplan = lid;
                         var stid = sid;
                         var lpstatus = lpstatus;
                 var jsonData = JSON.stringify({ lplan: lplan, studid: stid, lpstatus: lpstatus });
                 $.ajax({
                     type: "POST",
                     url: "LessonReportsWithPaging.aspx/getAcademicReportOther",
                     data: jsonData,
                     contentType: "application/json; charset=utf-8",
                     async: false,
                     dataType: "json",
                     success: function (response) {
                                 var inputsDate = sDate;
                         inputsDate = inputsDate.replace(/-/g, '/');
                                 var inputeDate = eDate;
                         inputeDate = inputeDate.replace(/-/g, '/');
                         var obj = JSON.parse(response.d);
                         $.map(obj, function (item, index) {
                                     titlename = item['StudentName'];
                                     stname = item['StudentName'];
                                     titlelesson = item['LessonPlanName'];
                                     lsname = item['LessonPlanName'];
                                     titledate = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                                     rangedate = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate)
                                     if ((medno == "false" || medno == "False")&&(med == "True" || med == "true")) {
                                         medtitle = titlename + '<br>' + titlelesson + '<br>' + titledate;
                                         maintitle = '';
                                     }
                                     else {
                                         maintitle = titlename + '<br>' + titlelesson + '<br>' + titledate;
                                         medtitle = '';
                                     }
                                     if (scid == 1) {
                             document.getElementById('mel').innerHTML = 'Melmark New England';
                                     }
                                     else {
                                         document.getElementById('mel').innerHTML = 'Melmark Pennsylvania';
                                     }
                                     if (item['Deftn'] != null) {
                                         document.getElementById('deftxt').innerHTML = ' Definition:    ' + item['Deftn'];
                             }
                                     else {
                                         document.getElementById('deftxt').innerHTML = '  Definition:    ';
                             }
                                     treatment = item['Treatment'];
                         });
                     },
                     error: OnErrorCall_
                 });
                 function OnErrorCall_(response) {

                     alert("something went wrong!");
                 }
                 // chart student name, class, date and type set start
                 //start chart
                         var sdate = sDate;
                         var edate = eDate;
                         var sid = sid;
                         var lid = lid;
                         var scid = scid;
                         var evnt = evnt;
                         var trend = trend;
                         var ioa = ioa;
                         var cls = cls;
                 var xmin;
                 var xmax;
                 var jsonData = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, AllLesson: lid, SchoolId: scid, Events: evnt, Trendtype: trend, IncludeIOA: ioa, Clstype: cls });
                 $.ajax({
                     type: "POST",
                     url: "LessonReportsWithPaging.aspx/getAcademicReport",
                     data: jsonData,
                     contentType: "application/json; charset=utf-8",
                     async: false,
                     dataType: "json",
                     success: function (response) {
                         var res = JSON.parse(response.d);
                         if (res.length === 0) {
                             var hchart = document.getElementById('<%= cont.ClientID %>');
                             if (hchart) {
                                 hchart.style.display = 'none';
                             }
                             var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                             if (hchart2) {
                                 hchart2.style.display = 'none';
                             }
                            
                             var hchart4 = document.getElementById('<%= deftxt.ClientID %>');
                             if (hchart4) {
                                 hchart4.style.display = 'none';
                             }
                             var hchart5 = document.getElementById('<%= mel.ClientID %>');
                             if (hchart5) {
                                 hchart5.style.display = 'none';
                             }
                             var hchart6 = document.getElementById('<%= mednodata.ClientID %>');
                             if (hchart6) {
                                 hchart6.style.display = 'none';
                             }
                             var span = document.getElementById('lbgraphSpan');
                             span.innerHTML = "No Data Available";
                             span.style.fontSize = '14px';
                             $('#sname').css("display", "block");
                             $('#trtmnt').css("display", "block");
                             $('#cont').css("display", "none");
                             $('#lnam').css("display", "block");
                             $('#daterang').css("display", "block");
                             $('#mel').css("display", "block");
                             $('#deftxt').css("display", "block");

                             var inputsDates = sdate;
                             inputsDates = inputsDates.replace(/-/g, '/');
                             var inputeDates = edate;
                             inputeDates = inputeDates.replace(/-/g, '/');


                             $.ajax({
                                 type: "POST",
                                 url: "LessonReportsWithPaging.aspx/getAcademicReportNext",
                                 data: JSON.stringify({ lplan: lid, studid: sid }),
                                 contentType: "application/json; charset=utf-8",
                                 async: false,
                                 dataType: "json",
                                 success: function (resp) {
                                     var resu = JSON.parse(resp.d);
                                     $.map(resu, function (items, index) {
                                         var trtmnt = document.getElementById('trtmnt');
                                         trtmnt.style.fontSize = '12px';
                                         if (items['Treatment'] != null) {
                                             document.getElementById('trtmnt').innerHTML = items['Treatment'];
                                         } else {
                                             document.getElementById('trtmnt').innerHTML = 'Tx: ';
                         }

                                         document.getElementById('lnam').innerHTML = items['LessonPlanName'];
                                         var bname = document.getElementById('lnam');
                                         bname.style.fontSize = '12px';

                                         var deftxt = document.getElementById('deftxt');
                                         deftxt.style.fontSize = '12px';
                                         if (items['Deftn'] != null) {
                                             document.getElementById('deftxt').innerHTML = 'Definition:    ' + items['Deftn'];

                                         }
                                 else {
                                             document.getElementById('deftxt').innerHTML = 'Definition:    ';
                                         }
                                         document.getElementById('sname').innerHTML = items['StudentName'];
                                         var studname = document.getElementById('sname');
                                         studname.style.fontSize = '12px';
                                     });

                                 },
                                 error: function (xhr, status, error) {

                                     console.error("AJAX request failed:", error);
                                 }
                             });
                             document.getElementById('daterang').innerHTML = convertDateFormat(inputsDates) + "-" + convertDateFormat(inputeDates);
                             if (scid == 1) {
                                 document.getElementById('mel').innerHTML = 'Melmark New England';
                                 document.getElementById('mel').style.fontSize = '12px';
                             }
                             else {
                                 document.getElementById('mel').innerHTML = 'Melmark Pennsylvania';
                                 document.getElementById('mel').style.fontSize = '12px';
                             }
                             var drange = document.getElementById('daterang');
                             drange.style.fontSize = '12px';

                         }
                         else {
                             $('#sname').css("display", "none");
                             $('#trtmnt').css("display", "none");
                             $('#lnam').css("display", "none");
                             $('#daterang').css("display", "none");
                             var hchart = document.getElementById('<%= cont.ClientID %>');
                             if (hchart) {
                                 hchart.style.display = 'block';
                             }
                             var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                             if (hchart2) {
                                 hchart2.style.display = 'block';
                             }

                             var hchart4 = document.getElementById('<%= deftxt.ClientID %>');
                             if (hchart4) {
                                 hchart4.style.display = 'block';
                             }
                             var hchart5 = document.getElementById('<%= mel.ClientID %>');
                             if (hchart5) {
                                 hchart5.style.display = 'block';
                             }
                             var hchart6 = document.getElementById('<%= mednodata.ClientID %>');
                             if (hchart6) {
                                 hchart6.style.display = 'block';
                             }
                                     var secstatus = true;
                             // primary and secondary y axis start.
                             var leftY = res[0].LeftYaxis;
                             var rightY = res[0].RightYaxis;
                                     if (rightY == null) {
                                         secstatus = false;
                             }
                             // primary and secondary y axis end.

                             // Plot Data Start
                             var data = [];
                             var yAxis = [];
                             var color = [];
                                     var symbol = [];
                                     var calc = []; var perc = []; var nonperc = [];
                                     var dummy = [];
                             $.map(res, function (item, index) {
                                 if (item['CalcType'] != 'Event') {
                                     var yax;

                                     if (item['LeftYaxis'] != null && item['RightYaxis'] == null || item['LeftYaxis'] == null && item['RightYaxis'] != null) {
                                         yax = 0;
                                     }
                                             else if (item['CalcType'] != "Total Duration" && item['CalcType'] != "Total Correct" && item['CalcType'] != "Total Incorrect" && item['CalcType'] != "Frequency" && item['CalcType'] != "Avg Duration" && item['CalcType'] != "Customize" && item['CalcType'] != "Event" && item['CalcType'] != item['RightYaxis'] || item['RptLabel'] == item['LeftYaxis']) {
                                         yax = 0;
                                     }
                                     else {
                                         yax = 1;
                                     }

                                     if (yax == 0 || item['RptLabel'] == item['LeftYaxis']) {
                                         var catdata = {};
                                                 var symb = {};
                                     catdata['date'] = extractdate(item['AggredatedDate']);
                                     catdata['name'] = item['ColName'] + '-' + item['RptLabel'];
                                     catdata['Color'] = item['Color'];
                                     catdata['IOAPerc'] = item['IOAPerc'];
                                     catdata['CalcType'] = item['CalcType'];
                                     catdata['PercntCount'] = item['PercntCount'];
                                     catdata['NonPercntCount'] = item['NonPercntCount'];
                                     catdata['DummyScore'] = item['DummyScore'];
                                                 symb['symbol'] = item['Shape'].toLowerCase();
                                                 symb['enabled'] = true;
                                                 symb['states'] = { hover: { enabled: true } }
                                                 catdata['symbol'] = symb;
                                      catdata['yAxis'] = 0;
                                      catdata['value'] = item['Score'];
                                      data.push(catdata);
                                      yAxis[catdata['name']] = catdata['yAxis'];
                                                 color[catdata['name']] = catdata['Color'];
                                                 symbol[catdata['name']] = catdata['symbol'];
                                                 calc[catdata['name']] = catdata['CalcType'];
                                                 perc[catdata['name']] = catdata['PercntCount'];
                                                 nonperc[catdata['name']] = catdata['NonPercntCount'];
                                                 dummy[catdata['name']] = catdata['DummyScore'];
                                     } else {
                                         var catdata = {};
                                                 var symb = {};
                                         catdata['date'] = extractdate(item['AggredatedDate']);
                                         catdata['name'] = item['ColName'] + '-' + item['RptLabel'];
                                         catdata['Color'] = item['Color'];
                                         catdata['IOAPerc'] = item['IOAPerc'];
                                         catdata['CalcType'] = item['CalcType'];
                                         catdata['PercntCount'] = item['PercntCount'];
                                         catdata['NonPercntCount'] = item['NonPercntCount'];
                                         catdata['DummyScore'] = item['DummyScore'];
                                                 symb['symbol'] = item['Shape'].toLowerCase();
                                                 symb['enabled'] = true;
                                                 symb['states'] = { hover: { enabled: true } }
                                                 catdata['symbol'] = symb;
                                         catdata['yAxis'] = 1;
                                         catdata['value'] = item['DummyScore'];
                                         data.push(catdata);
                                         yAxis[catdata['name']] = catdata['yAxis'];
                                                 color[catdata['name']] = catdata['Color'];
                                                 symbol[catdata['name']] = catdata['symbol'];
                                                 calc[catdata['name']] = catdata['CalcType'];
                                                 perc[catdata['name']] = catdata['PercntCount'];
                                                 nonperc[catdata['name']] = catdata['NonPercntCount'];
                                                 dummy[catdata['name']] = catdata['DummyScore'];
                                     }

                                 }
                             });

                             // var newdata = [...new Set(data.map(item => item.name))];
                             var newData = []; var names = [];
                                     data.forEach(function (item) {
                                 if (!names[item.name]) {
                                     names[item.name] = true;
                                     newData.push(item.name);
                                 }
                             });
                                     if (inctype == "True" || inctype == "true") {
                                 var typechart = 'scatter';
                             }
                                     else {
                                 var typechart = 'line';
                             }
                                     var ser = newData.map(function (name) {
                                 return {
                                             type: typechart,
                                     name: name,
                                     yAxis: yAxis[name],
                                     color: color[name],
                                             marker: symbol[name],
                                             dataGrouping: { enabled: true },
                                             data: data.filter(function (item) {
                                         return item.name === name;
                                             }).map(function (item) {
                                         return {
                                             x: item.date,
                                             y: item.value,
                                                     IOAPerc: item.IOAPerc,
                                         };
                                     })

                                 };
                         });
                         //find maximum of y axis start
                                     var yaxisval = newData.map(function (name) {
                             return {
                                 yAxis: yAxis[name],
                                 calc: calc[name],
                                             perc: perc[name],
                                             nonperc: nonperc[name],
                                             dummy: dummy[name]
                             }
                         });
                         var lmax = -Infinity; var rmax = -Infinity;
                         for (var i = 0; i < res.length; i++) {
                             var primeY = res[i].Score;
                             var SecY = res[i].DummyScore;
                             if (primeY > lmax) {
                                 lmax = primeY;
                             }
                             if (SecY > rmax) {
                                 rmax = SecY;
                             }
                         }
                         var leftmax; var rightmax; var rintrvl; var lintrvl;
                         for (var i = 0; i < yaxisval.length; i++) {
                                         if (yaxisval[i].yAxis == 0) {
                                 var pattern = /%/;
                                             if (pattern.test(yaxisval[i].calc) || yaxisval[i].calc == "percent" || yaxisval[i].calc == "Percent") {
                                                 leftmax = 100;
                                 }
                                             else {
                                                 if (lmax == 0) {
                                                     leftmax = 1;
                                     }
                                                 else {
                                                     leftmax = lmax
                                     }

                                 }
                                 if (yaxisval[i].perc > 0) {
                                     lintrvl = 10;
                                 } else if (lmax === 0) {
                                     lintrvl = 1;
                                 } else if (lmax === 1) {
                                     lintrvl = 1;
                                 } else if (lmax >= 50) {
                                     lintrvl = 10;
                                 } else if (lmax >= 20) {
                                     lintrvl = 5;
                                 } else {
                                     lintrvl = 1;
                                 }
                             }
                                         if (yaxisval[i].yAxis == 1) {
                                             var pattern = /%/;
                                             if (pattern.test(yaxisval[i].calc) || yaxisval[i].calc == "percent" || yaxisval[i].calc == "Percent") {
                                                 rightmax = 100
                                             }
                                             else {
                                                 if (rmax == 0) {
                                                     rightmax = 1;
                                                 }
                                                 else {
                                                     rightmax = rmax
                                                 }

                                             }
                                 if (yaxisval[i].perc === 2 && yaxisval[i].nonperc === 0) {
                                     rintrvl = 10;
                                 }
                                             else {
                                                 rightmax = rmax;
                                                 if (rmax === 0) {
                                         rintrvl = 1;
                                     } else if (rmax === 1) {
                                         rintrvl = 1;
                                     } else if (rmax >= 1000) {
                                         rintrvl = 20;
                                     } else if (rmax >= 50) {
                                         rintrvl = 10;
                                     } else if (rmax >= 20) {
                                         rintrvl = 5;
                                     } else {
                                         rintrvl = 1;
                                     }
                                 }
                             }
                         }
                         //find maximum of y axis end
                         //set duplicate series (case:no series) start
                                     dupdata = [];
                                     if (data.length === 0) {
                             var samdata = {};
                                         var sd = extractdateOther(sdate); var ed = extractdateOther(edate);
                                         samdata['data'] = [{ x: sd, y: null }, { x: ed, y: null }];
                                         samdata['showInLegend'] = false;
                                         dupdata.push(samdata);
                             ser = dupdata;
                             leftmax = 1; rightmax = 1;
                         }
                         //set duplicate series (case:no series) end
                         //trend start
                                     var arrData = []; var yAxis1 = []; var color1 = []; var symbol1 = [];
                         $.map(res, function (item, index) {
                             var trnddata = {};
                                         var symbarr = {};
                                         if (item['CalcType'] != 'Event' && trend == "Quarter") {
                                 var yax;

                                 if (item['LeftYaxis'] != null && item['RightYaxis'] == null || item['LeftYaxis'] == null && item['RightYaxis'] != null) {
                                     yax = 0;
                                 }
                                             else if (item['CalcType'] != "Total Duration" && item['CalcType'] != "Total Correct" && item['CalcType'] != "Total Incorrect" && item['CalcType'] != "Frequency" && item['CalcType'] != "Avg Duration" && item['CalcType'] != "Customize" && item['CalcType'] != "Event" && item['CalcType'] != item['RightYaxis'] || item['RptLabel'] == item['LeftYaxis']) {
                                     yax = 0;
                                 }
                                 else {
                                     yax = 1;
                                 }

                                 if (yax == 0 || item['RptLabel'] == item['LeftYaxis']) {
                                 trnddata['date'] = extractdate(item['AggredatedDate']);
                                     trnddata['name'] = item['ColName'] + '-' + item['CalcType'] + 'trend';
                                 trnddata['value'] = item['Trend'];
                                 trnddata['Color'] = "Gray";
                                     symbarr['enabled'] = false;
                                     trnddata['symbol'] = symbarr;
                                     trnddata['yAxis1'] = 0;
                                     arrData.push(trnddata);
                                     yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                     color1[trnddata['name']] = trnddata['Color'];
                                     symbol1[trnddata['name']] = trnddata['symbol'];
                                 }
                                 else {
                                     trnddata['date'] = extractdate(item['AggredatedDate']);
                                     trnddata['name'] = item['ColName'] + '-' + item['CalcType'] + 'trend';
                                     trnddata['value'] = item['Trend'];
                                     trnddata['Color'] = "Gray";
                                     symbarr['enabled'] = false;
                                     trnddata['symbol'] = symbarr;
                                     trnddata['yAxis1'] = 1;
                                 arrData.push(trnddata);
                                     yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                     color1[trnddata['name']] = trnddata['Color'];
                                     symbol1[trnddata['name']] = trnddata['symbol'];
                             }
                             }
                         });
                                     var newData1 = []; var name1 = [];
                             // var newdata1 = [...new Set(arrData.map(item => item.name))];
                                     arrData.forEach(function (item) {
                                 if (!name1[item.name]) {
                                     name1[item.name] = true;
                                     newData1.push(item.name);
                                 }
                             });
                             var ser1 = newData1.map(function (name) {
                             return {
                                 name: "",
                                 yAxis: yAxis1[name],
                                 color: color1[name],
                                             marker: symbol1[name],
                                             dashStyle: "DashDotDot",
                                 lineWidth: 2,
                                             showInLegend: false,
                                             data: arrData.filter(function (item) {
                                     return item.name === name;
                                             }).map(function (item) {
                                     return {
                                         x: item.date,
                                         y: item.value
                                     };
                                 })
                             };
                     });
                 //trend  end
                 ser = ser.concat(ser1);
                 //Plot Data End

                 //Event plot Start
                 var plotdata = [];
                 var uniqueItems = [];
                                     var maxYPosition = 300;
                 var currentYPosition = 0;
                 $.map(res, function (item, index) {
                     var style; var wid;
                                         if (item['CalcType'] == "Event" && item['EventType'] != null) {
                         var isUnique = true;
                         for (var i = 0; i < uniqueItems.length; i++) {
                             var uniqueItem = uniqueItems[i];
                             if (
                                 uniqueItem['EventName'] === item['EventName'] &&
                                 uniqueItem['AggredatedDate'] === item['AggredatedDate'] &&
                                 uniqueItem['EventType'] === item['EventType']
                             ) {
                                 isUnique = false;
                                 break;
                             }
                         }

                         if (isUnique) {
                             uniqueItems.push(item);
                             if (item['EventType'] == "Major") {
                                 style = 'solid';
                                 wid = 1;
                             }
                             else {
                                 style = 'dot';
                                 wid = 2;
                             }

                             var plotdict = new Object;
                             plotdict['color'] = "black";
                             plotdict['dashStyle'] = style;
                             plotdict['value'] = extractdate(item['AggredatedDate']);
                             plotdict['width'] = wid;
                             if (item['EventName'] == null) {
                                 item['EventName'] = "";
                             }
                             var datescorealign = findmaxscorefordate(res);
                             var arr = checkarrow(res);
                             var str = item['EventName'];
                             var txtal; var xax;
                             var valign;
                             if (item['EventType'] == "Major") {
                                 txtal = 'right';
                                 xax = -2;
                                 valign = getValueByAggredatedDate(item['AggredatedDate'], datescorealign, arr)
                             }
                             else {
                                 txtal = 'left';
                                 xax = 9;

                                 valign = getValueByAggredatedDate(item['AggredatedDate'], datescorealign, arr);
                             }
                             plotdict['label'] = {
                                 text: str,
                                // textAlign: txtal,
                                 formatter: function () {
                                     var labelText = this.options.text;
                                     labelText = labelText.replace(/↑/g, '<span style="font-size: 15px;">↑</span>')
                                         .replace(/↓/g, '<span style="font-size: 15px;">↓</span>');
                                     return labelText;
                                 },
                                 useHTML: true,
                                 style: {
                                     fontWeight: 'normal',
                                     color: 'black',
                                                         fontSize: sizeOffont,
                                     fontFamily: 'Arial',
                                     textShadow: 'none',
                                 },
                                 // align: 'right',
                                 //  textAlign: 'right',
                                 x: xax,
                                 verticalAlign: valign,
                                 rotation: -90
                             };
                             plotdict['label']['overflow'] = 'justify';
                             plotdict['label']['crop'] = false;
                             plotdata.push(plotdict);

                         }
                     }
                     plotdata = plotlinedata(plotdata);
                 });
                 //arrow start

                                     var arrows = [];
                 var plotarr = new Object;
                 plotarr['type'] = 'scatter';
                                     plotarr['showInLegend'] = false;
                                     if (inctype == "True" || inctype == "true") {
                                         plotarr['color'] = 'black',
                     plotarr['marker'] =   {
                         symbol: 'circle',
                                             radius: 4,
                     }
                 }
                                     else {
                     plotarr['marker'] =   {
                         symbol: 'url(../Scripts/arrownote.png)',
                         width: 8,
                         height: 10,
                     }
                                     }
                 $.map(res, function (item, index) {
                                         if (item['ArrowNote'] != null) {
                                             arrows.push({ x: extractdate(item['AggredatedDate']), y: item['Score'], arrowval: item['ArrowNote'] })
                                             arrows.push({ x: extractdate(item['AggredatedDate']), y: item['DummyScore'], arrowval: item['ArrowNote'] })
                                         }
                 });
                 var mergedarr = [];
                 var mergedKeys = {};

                 for (var i = 0; i < arrows.length; i++) {
                     var key = arrows[i].x + '-' + arrows[i].y;
                     if (!mergedKeys[key]) {
                         mergedarr.push(arrows[i]);
                         mergedKeys[key] = true;
                     } else {
                         for (var j = 0; j < mergedarr.length; j++) {
                             if (mergedarr[j].x === arrows[i].x && mergedarr[j].y === arrows[i].y) {
                                 mergedarr[j].arrowval += ',' + arrows[i].arrowval;
                                 break;
                             }
                         }
                     }
                 }
                 plotarr['data'] = mergedarr;

                 ser.push(plotarr);
                 //arrow end
                 //Event plot End
                             //Start Highchart
                                     chart1 = Highcharts.chart('cont', {
                     chart: {
                         plotBorderWidth: 2,
                         plotBorderColor: 'black',
                         spacingTop: 20,
                         spacingRight: 20,
                         spacingBottom: 20,
                         spacingLeft: 20,
                         marginTop: 100,
                     },

                     title: {
                                             text: maintitle,
                                             useHTML: true,
                                             style: {
                                                 fontWeight: 'bold',
                                                 color: 'black',
                                                 fontSize: sizeOffont,
                                                 fontFamily: 'Arial'
                     },
                                         },
                                         subtitle: {
                         text: treatment,
                                             x: 50,
                         useHTML: true,
                         style: {
                             fontWeight: 'bold',
                             color: 'black',
                                                 fontSize: sizeOffont,
                             fontFamily: 'Arial'
                         },
                                             align: 'left',
                     },
                     credits: {
                         enabled: false
                     },
                     yAxis:
               [{ // Primary yAxis 
                                       min: 0,
                   gridLineWidth: 0,
                                       tickInterval: lintrvl,
                                       softMin: 0,
                                       max: leftmax,
                   title: {
                       text: leftY,
                       useHTML: true,

                       style: {
                           fontWeight: 'bold',
                           color: 'black',
                                               fontSize: sizeOffont,
                           fontFamily: 'Arial'

                       }
                   },
                   labels: {
                       style: {
                           color: 'black',
                                               fontSize: sizeOffont,
                           fontWeight: 'normal',
                           fontFamily: 'Arial'

                       }
                   },

               }, { // Secondary yAxis
                                       visible: secstatus,
                   gridLineWidth: 0,
                                       tickInterval: rintrvl,
                   min: 0,
                                       softMin: 0,
                                       max: rightmax,
                   title: {
                       text: rightY,
                       useHTML: true,
                       style: {
                           fontWeight: 'bold',
                           color: 'black',
                                               fontSize: sizeOffont,
                           fontFamily: 'Arial'
                       }
                   },
                   labels: {
                       style: {
                                               color: 'black',
                                               fontSize: sizeOffont,
                           fontWeight: 'normal',
                           fontFamily: 'Arial'

                       }
                   },
                   opposite: true,
               }],

                     xAxis: {
                         lineColor: 'black',
                         lineWidth: 1,
                         title: {
                             text: 'Dates',
                             useHTML: true,
                             style: {
                                 fontWeight: 'bold',
                                 color: 'black',
                                                     fontSize: sizeOffont,
                                 fontFamily: 'Arial'

                             }

                         },
                                             min: extractdateOther(sdate.replace('/', '-')),
                                             max: extractdateOther(edate.replace('/', '-')),
                                             plotLines: plotdata,
                         tickInterval: 2 * 24 * 3600 * 1000,
                         labels: {
                             rotation: -90,
                             formatter: function () {
                                 return Highcharts.dateFormat('%m/%d/%Y', this.value); // Format the date as desired
                             },
                             style: {
                                 fontWeight: 'normal',
                                 color: 'black',
                                                     fontSize: sizeOffont,
                                 fontFamily: 'Arial'

                         }
                         }
                     },
                     legend: {
                         layout: 'horizontal',
                         align: 'right',
                         verticalAlign: 'bottom',
                                             labelFormatter: function () {
                                                 return '<span style="color: black; font-weight: normal;font-size:' + sizeOffont + '; font-family:Arial">' + this.name + '</span>';
                 }
                     },
                     plotOptions: {
                         series: {
                                                 turboThreshold: 5000,
                             dataLabels: {
                                 allowOverlap: true,
                                 enabled: true,
                                 rotation: -90,
                                 align: 'top',
                                 y: -5,
                                 overflow: 'justify',
                                 crop: false,
                                 //style: {
                                 //    textOutline: '1px contrast' 
                                 //},

                                 formatter: function () {
                                     var point = this.point;
                                     if (point.IOAPerc) {
                                         if (point.y > 0) {
                                             var words = point.IOAPerc.split(' ');
                                             var newpoint = '';
                                             for (var i = 0; i < words.length; i++) {
                                                 if (words[i].endsWith('%') && i < words.length - 1) {
                                                                         newpoint += '<span style="font-size:' + sizeOffont + '; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span><br> ';
                                                 } else {
                                                                         newpoint += '<span style="font-size:' + sizeOffont + '; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span> ';
                                     }
                                         }
                                                                 return '<div style="white-space: nowrap; font-weight: normal;font-size:' + sizeOffont + '; font-family: Arial; color: black">' + newpoint + '</div>';
                                         }
                                         else {
                                                                 return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size:' + sizeOffont + ';font-family:Arial;color:black;text-shadow: none;font-weight:normal;">' + point.IOAPerc + '</span>';
                                         }
                                     }
                                     else if (point.arrowval) {
                                                             return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size:' + sizeOffont + '; font-family:Arial;font-color:black;text-shadow: none;font-weight:normal;">' + point.arrowval;
                                     }
                                     else {
                                         return '';
                                     }
                                 },
                             },
                         },
                     },

                     tooltip: {
                         formatter: function () {
                                                 return '<b>' + Highcharts.dateFormat('%m/%d/%Y', new Date(this.x)) + '</b>: ' + this.y;
                         }
                     },

                     series: ser,
                     exporting: {
                         sourceWidth: 935.43307,
                         sourceHeight: 688.901215752,
                         // scale: 2 (default)
                         //chartOptions: {
                         //    subtitle: null
                         //}
                     }
                 });
             }

                                 var medjson = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, SchoolId: scid });

                                 if ((med == "true" || med == "True") && (medno == "false" || medno == "False")) {
                  $.ajax({
                      type: "POST",
                      url: "LessonReportsWithPaging.aspx/getmedAcademicReport",
                      data: medjson,
                      contentType: "application/json; charset=utf-8",
                      async: false,
                      dataType: "json",
                      success: function (response) {
                                             var resp = JSON.parse(response.d);
                                             var medicData = []; var val = 0;
                          resp.forEach(function (item) {
                              item.duration = extractdate(item.EndTime) - extractdate(item.EvntTs);
                          });
                          resp.sort(function (a, b) {
                              return b.duration - a.duration;
                          });
                          $.map(resp, function (item, index) {
                                                 var meddata = {};
                                                 meddata['name'] = item['EventName'] + item['Comment'];
                              meddata['x'] = extractdate(item['EvntTs']);
                              meddata['x2'] = extractdate(item['EndTime']);
                                                 meddata['y'] = val;
                              medicData.push(meddata);
                                                 val = val + 1
                          });
                                             chart2 = Highcharts.chart('medcont', {
                              chart: {
                                  type: 'xrange',
                                  plotBorderWidth: 2,
                                  plotBorderColor: 'black',
                                  spacingTop: 20,
                                  spacingRight: 20,
                                  spacingBottom: 20,
                                  spacingLeft: 20,
                              },
                              title: {
                                                     text: medtitle,
                                                     useHTML: true,
                                                     style: {
                                                         fontWeight: 'bold',
                                                         color: 'black',
                                                         fontSize: sizeOffont,
                                                         fontFamily: 'Arial'
                              },
                                                 },
                                                 subtitle: {
                                                     text: ' Medication Details',
                                                     x: 50,
                                                     align: 'left',
                                  fontSize: '55px',
                                  style: {
                                      color: '#000000',
                                     fontWeight: 'bold',
                                     fontSize: '12px',
                                     fontFamily: 'Arial'

                                  }
                              },
                              xAxis: {

                                  //visible: false,
                                  type: 'datetime',
                                  //linkedTo: 0,
                                                     min: extractdateOther(sdate.replace('/', '-')),
                                                     max: extractdateOther(edate.replace('/', '-')),
                                  tickInterval: 2 * 24 * 3600 * 1000,
                                  tickPositioner: function () {
                                    var ticks = [];
                                    var interval = this.options.tickInterval;
                                      var currentTick = this.min;

                                      while (currentTick <= this.max) {
                                          ticks.push(currentTick);
                                          currentTick += interval;
                                      }

                                      return ticks;
                                  },
                                  lineWidth: 0,
                                  minorGridLineWidth: 0,
                                  lineColor: 'transparent',
                                  minorTickLength: 0,
                                  tickLength: 0,
                                  startOnTick: true,
                                  endOnTick: true,
                                  labels: {
                                      enabled: false,
                                    //  rotation: -90,
                                      //formatter: function () {
                                      //    return Highcharts.dateFormat('%m/%d/%Y', this.value);
                                      //}
                                  }
                              },
                                                 credits: {
                                                     enabled: false
                              },
                              yAxis: {
                                 gridLineWidth: 0,
                                  offset: 60,
                                  title: {
                                      text: ''
                                  },
                                  labels: {
                                      enabled: false
                                  },
                                  reversed: true
                              },
                              tooltip: {
                              formatter: function () {
                                                         return Highcharts.dateFormat('%m/%d/%Y', this.point.x) + '-' + Highcharts.dateFormat('%m/%d/%Y', this.point.x2);

                              }
                          },
                              series: [{
                                  data: medicData,
                                  showInLegend: false,
                                  dataLabels: {
                                      enabled: true,
                                                         formatter: function () {
                                          return this.point.name;
                                      }
                                  },

                              }],
                              exporting: {
                                  sourceWidth: 935.43307,
                                  sourceHeight: 164.05358746,
                                  // scale: 2 (default)
                                  //chartOptions: {
                                  //    subtitle: null
                                  //}
                              }

                          });
                      },
                      error: OnErrorCall
                  });
                  function OnErrorCall(response) {

                      alert("Something went wrong! Error: ");

                  }

              }
                     },
                     error: OnErrorCall_
                 });
                 function OnErrorCall_(response) {

                     alert("Something went wrong! Error: ");
                    // alert("Something went wrong! Error: " + response.responseText);

                 }
                         closePopup();
                         HideWait();
             }
                 function extractdate(dateString) {
                     var regex = /\/Date\((\d+)\)\//;
                     var match = dateString.match(regex);
                     var milliseconds = parseInt(match[1]);
                     var date = new Date(milliseconds);
                     var year = date.getFullYear();
                     var month = date.getMonth() ;
                     var day = date.getDate();
                     return Date.UTC(year, month, day);
                 }
                 function extractdateOther(dateString) {
                     var dateParts = dateString.split("-");
                     var year = parseInt(dateParts[0]);
                     var month = parseInt(dateParts[1]) - 1;
                     var day = parseInt(dateParts[2]);
                     var sdateval = Date.UTC( year , month ,day);
                     return sdateval
                 }
                 function plotlinedata(plotdata) {
                     var combinedArray = [];

                     for (var i = 0; i < plotdata.length; i++) {
                         var item = plotdata[i];
                         var existingItem = findExistingItem(combinedArray, item);

                         if (existingItem) {
                             existingItem.label += ',<br>' + item.label;
                         } else {
                             combinedArray.push(item);
                         }
                     }
                     return combinedArray;

                     function findExistingItem(arr, item) {
                         for (var j = 0; j < arr.length; j++) {
                             var combinedItem = arr[j];
                             if (
                                 combinedItem.color === item.color &&
                                 combinedItem.dashStyle === item.dashStyle &&
                                 combinedItem.value === item.value &&
                                 combinedItem.width === item.width
                             ) {
                                 return combinedItem;
                             }
                         }
                         return null;
                     }
                 }
                 function convertDateFormat(inputDate) {
      var parts = inputDate.split('/');
                     var year = parseInt(parts[0], 10);
                     var month = parseInt(parts[1], 10);
                     var day = parseInt(parts[2], 10);
                     var formattedDate = (
    (month < 10 ? "0" : "") + month + '/' +
    (day < 10 ? "0" : "") + day + '/' +
    year
  );

                     return formattedDate;
                 }
                 var charts1, charts2;
                 var lessgroup = "<%=lid%>";
             var stname="<%=stname%>";
                 var less = lessgroup.split(",").map(Number);
                 var lessnum = less.length;
                 var i = 0;
                 function exportChart() {
                     sizeOffont = '14px';
                         processNextChart();
             }
                 //function processNextChart() {
             //        if (i < lessnum) {
             //            generateHighchart(less[i], function () {
             //                //captureAndSend(less[i], function () {
             //                //    i++;
             //                //    //processNextChart();
             //                //});
             //                html2canvas(document.getElementById("HighchartGraph")).then(function (canvas) {
             //                    var imgData = canvas.toDataURL("image/png");
             //                    chartImages.push(imgData); // save image

             //                    i++;
             //                    processNextChart(); // next iteration
             //                });

             //            });
             //        }
             //        else {
             //            generatePDF();
             //            DownloadPopup();
             //            closePopup();
             //        }
                    
                     //    }

                       

                 var doc = new window.jspdf.jsPDF('l', 'pt', 'a4'); // 'l' = landscape
                 var margin = 10; // small border (~3.5 mm)
                 var i = 0;

                 function processNextChart() {
                     if (i < lessnum) {
                         generateHighchart(less[i], function () {
                             var chartElement = document.getElementById("HighchartGraph");

                             // Capture chart at fixed pixel density, independent of browser zoom
                             html2canvas(chartElement, {
                                 scale: 2,            // high resolution
                                 useCORS: true,
                                 windowWidth: chartElement.scrollWidth,  // ensures full width capture
                                 windowHeight: chartElement.scrollHeight // ensures full height capture
                             }).then(function (canvas) {
                                 var imgData = canvas.toDataURL("image/jpeg", 0.95);

                                 // PDF page size (landscape A4)
                                 var pageWidth = doc.internal.pageSize.getWidth();
                                 var pageHeight = doc.internal.pageSize.getHeight();

                                 // Compute image dimensions to fit nicely
                                 var imgWidth = pageWidth - 2 * margin;
                                 var imgHeight = (canvas.height * imgWidth) / canvas.width;

                                 // Fit by height if necessary
                                 if (imgHeight > pageHeight - 2 * margin) {
                                     imgHeight = pageHeight - 2 * margin;
                                     imgWidth = (canvas.width * imgHeight) / canvas.height;
                                 }

                                 // Center image
                                 var posX = (pageWidth - imgWidth) / 2;
                                 var posY = (pageHeight - imgHeight) / 2;

                                 // Add a new page if needed
                                 if (i > 0) doc.addPage();

                                 // Add the image
                                 doc.addImage(imgData, 'JPEG', posX, posY, imgWidth, imgHeight);
                                 i++;
                                 processNextChart();
                             }).catch(function (err) {
                                 console.error("html2canvas failed:", err);
                                 i++;
                                 processNextChart();
                             });
                         });
                     } else {
                         var filename = stname + ".pdf";
                         doc.save(filename);
                         closePopup();
                         generateHighchart(less[0]);
                     }
             }


             function generateHighchart(lessid, callback) {

                 var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
                 hchart.style.display = 'block';
                 var span = document.getElementById('lbgraphSpan');
                 span.innerHTML = "";
                 $('#mel').css("display", "block");
                 $('#deftxt').css("display", "block");
                 var treatment = '';
                 //medication report size start
                 if ("<%=medno%>" == "false" || "<%=medno%>" == "False") {
                     var meddiv = document.getElementById('medcont');
                     if ("<%=med%>" == "true" || "<%=med%>" == "True") {
                                  if ("<%=reptype%>" == "true" || "<%=reptype%>" == "True") {
                                      meddiv.style.width = '671.976pt';
                                      meddiv.style.height = '87.807911808pt';

                                  }
                                  else {
                                      meddiv.style.width = '935.43307pt';
                                      meddiv.style.height = '164.05358746pt';
                                  }
                              }
                          }
                 // medication report size end
                 // set cont size start
                          var myDiv = document.getElementById('cont');
                          if ("<%=reptype%>" == "true" || "<%=reptype%>" == "True") {
                     myDiv.style.width = '671.976pt';
                     myDiv.style.height = '510.2364pt';
                 } else {
                              myDiv.style.width = '935.43307pt';
                              myDiv.style.height = '688.901215752pt';
                 }
                 // set cont size End

                 // chart student name, class, date and type set start
                 var lplan = lessid;
                 var stid = "<%=sid%>";
                 var lpstatus = "<%=lpstatus%>";
                 var jsonData = JSON.stringify({ lplan: lplan, studid: stid, lpstatus: lpstatus });
                 $.ajax({
                     type: "POST",
                     url: "LessonReportsWithPaging.aspx/getAcademicReportOther",
                     data: jsonData,
                     contentType: "application/json; charset=utf-8",
                     async: false,
                     dataType: "json",
                     success: function (response) {
                         var inputsDate = "<%=sDate%>";
                                 inputsDate = inputsDate.replace(/-/g, '/');
                                 var inputeDate = "<%=eDate%>";
                         inputeDate = inputeDate.replace(/-/g, '/');
                         var obj = JSON.parse(response.d);
                         $.map(obj, function (item, index) {
                             titlename = item['StudentName'];
                             stname = item['StudentName'];
                             titlelesson = item['LessonPlanName'];
                             lsname = item['LessonPlanName'];
                             titledate = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                             rangedate = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                             if (("<%=medno%>" == "false" || "<%=medno%>" == "False") && ("<%=med%>" == "true" || "<%=med%>" == "True")) {
                                 medtitle = titlename + '<br>' + titlelesson + '<br>' + titledate;
                                 maintitle = '';
                             }
                             else {
                                 medtitle = '';
                                 maintitle = titlename + '<br>' + titlelesson + '<br>' + titledate;
                             }
                             if (item['SchoolId'] == 1) {
                             document.getElementById('mel').innerHTML = 'Melmark New England';
                             }
                             else {
                                 document.getElementById('mel').innerHTML = 'Melmark Pennsylvania';
                             }
                             if (item['Deftn'] != null) {
                                 document.getElementById('deftxt').innerHTML = ' Definition:    ' + item['Deftn'];
                             }
                             else {
                                 document.getElementById('deftxt').innerHTML = '  Definition:    ';
                             }
                             treatment = item['Treatment'];
                         });
                             },
                             error: OnErrorCall_
                         });
                 function OnErrorCall_(response) {

                     alert("something went wrong!");
                 }
                 // chart student name, class, date and type set start
                 //start chart
                 var sdate = "<%=sDate%>";
                 var edate = "<%=eDate%>";
                 var sid = "<%=sid%>";
                 var lid = lessid;
                 var scid = "<%=scid%>";
                         var evnt = "<%=evnt%>";
                 var trend = "<%=trend%>";
                 var ioa = "<%=ioa%>";
                 var cls = "<%=cls%>";
                 var xmin;
                 var xmax;
                 var jsonData = JSON.stringify({ AllLesson: lid});
                 $.ajax({
                     type: "POST",
                     url: "LessonReportsWithPaging.aspx/getAcademicReportBysid",
                     data: jsonData,
                     contentType: "application/json; charset=utf-8",
                     async: false,
                     dataType: "json",
                     success: function (response) {
                         var res = JSON.parse(response.d);
                         if (res.length === 0) {
                             var hchart = document.getElementById('<%= cont.ClientID %>');
                             if (hchart) {
                                 hchart.style.display = 'none';
                             }
                             var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                             if (hchart2) {
                                 hchart2.style.display = 'none';
                             }

                             var hchart4 = document.getElementById('<%= deftxt.ClientID %>');
                             if (hchart4) {
                                 hchart4.style.display = 'none';
                             }
                             var hchart5 = document.getElementById('<%= mel.ClientID %>');
                             if (hchart5) {
                                 hchart5.style.display = 'none';
                             }
                             var hchart6 = document.getElementById('<%= mednodata.ClientID %>');
                             if (hchart6) {
                                 hchart6.style.display = 'none';
                             }
                             var span = document.getElementById('lbgraphSpan');
                             span.innerHTML = "No Data Available";
                             span.style.fontSize = '18px';
                             $('#sname').css("display", "block");
                             $('#trtmnt').css("display", "block");
                             $('#cont').css("display", "none");
                             $('#lnam').css("display", "block");
                             $('#daterang').css("display", "block");
                             $('#mel').css("display", "block");
                             $('#deftxt').css("display", "block");

                             var inputsDates = sdate;
                             inputsDates = inputsDates.replace(/-/g, '/');
                             var inputeDates = edate;
                             inputeDates = inputeDates.replace(/-/g, '/');


                             $.ajax({
                                 type: "POST",
                                 url: "LessonReportsWithPaging.aspx/getAcademicReportNext",
                                 data: JSON.stringify({ lplan: lid, studid: sid }),
                                 contentType: "application/json; charset=utf-8",
                                 async: false,
                                 dataType: "json",
                                 success: function (resp) {
                                     var resu = JSON.parse(resp.d);
                                     $.map(resu, function (items, index) {
                                         var trtmnt = document.getElementById('trtmnt');
                                         trtmnt.style.fontSize = '16px';
                                         if (items['Treatment'] != null) {
                                             document.getElementById('trtmnt').innerHTML = items['Treatment'];
                                         } else {
                                             document.getElementById('trtmnt').innerHTML = 'Tx: ';
                         }

                                         document.getElementById('lnam').innerHTML = items['LessonPlanName'];
                                         var bname = document.getElementById('lnam');
                                         bname.style.fontSize = '16px';

                                         var deftxt = document.getElementById('deftxt');
                                         deftxt.style.fontSize = '16px';
                                         if (items['Deftn'] != null) {
                                             document.getElementById('deftxt').innerHTML = 'Definition:    ' + items['Deftn'];

                                         }
                         else {
                                             document.getElementById('deftxt').innerHTML = 'Definition:    ';
                                         }
                                         document.getElementById('sname').innerHTML = items['StudentName'];
                                         var studname = document.getElementById('sname');
                                         studname.style.fontSize = '16px';
                                     });

                                 },
                                 error: function (xhr, status, error) {

                                     console.error("AJAX request failed:", error);
                                 }
                             });
                             document.getElementById('daterang').innerHTML = convertDateFormat(inputsDates) + "-" + convertDateFormat(inputeDates);
                             if (scid == 1) {
                                 document.getElementById('mel').innerHTML = 'Melmark New England';
                                 document.getElementById('mel').style.fontSize = '16px';
                             }
                             else {
                                 document.getElementById('mel').innerHTML = 'Melmark Pennsylvania';
                                 document.getElementById('mel').style.fontSize = '16px';
                             }
                             var drange = document.getElementById('daterang');
                             drange.style.fontSize = '16px';
                             callback();
                         }
                         else {
                             $('#sname').css("display", "none");
                             $('#trtmnt').css("display", "none");
                             $('#lnam').css("display", "none");
                             $('#daterang').css("display", "none");
                             var hchart = document.getElementById('<%= cont.ClientID %>');
                             if (hchart) {
                                 hchart.style.display = 'block';
                             }
                             var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                             if (hchart2) {
                                 hchart2.style.display = 'block';
                             }

                             var hchart4 = document.getElementById('<%= deftxt.ClientID %>');
                             if (hchart4) {
                                 hchart4.style.display = 'block';
                             }
                             var hchart5 = document.getElementById('<%= mel.ClientID %>');
                             if (hchart5) {
                                 hchart5.style.display = 'block';
                             }
                             var hchart6 = document.getElementById('<%= mednodata.ClientID %>');
                             if (hchart6) {
                                 hchart6.style.display = 'block';
                             }
                             var secstatus = true;
                             // primary and secondary y axis start.
                             var leftY = res[0].LeftYaxis;
                             var rightY = res[0].RightYaxis;
                             if (rightY == null) {
                                 secstatus = false;
                             }
                             // primary and secondary y axis end.

                             // Plot Data Start
                             var data = [];
                             var yAxis = [];
                             var color = [];
                             var symbol = [];
                             var calc = []; var perc = []; var nonperc = [];
                             var dummy = [];
                             $.map(res, function (item, index) {
                                 if (item['CalcType'] != 'Event') {
                                     var yax;

                                     if (item['LeftYaxis'] != null && item['RightYaxis'] == null || item['LeftYaxis'] == null && item['RightYaxis'] != null) {
                                         yax = 0;
                                     }
                                     else if (item['CalcType'] != "Total Duration" && item['CalcType'] != "Total Correct" && item['CalcType'] != "Total Incorrect" && item['CalcType'] != "Frequency" && item['CalcType'] != "Avg Duration" && item['CalcType'] != "Customize" && item['CalcType'] != "Event" && item['CalcType'] != item['RightYaxis'] || item['RptLabel'] == item['LeftYaxis']) {
                                         yax = 0;
                                     }
                                     else {
                                         yax = 1;
                                     }

                                     if (yax == 0 || item['RptLabel'] == item['LeftYaxis']) {
                                         var catdata = {};
                                         var symb = {};
                                         catdata['date'] = extractdate(item['AggredatedDate']);
                                         catdata['name'] = item['ColName'] + '-' + item['RptLabel'];
                                         catdata['Color'] = item['Color'];
                                         catdata['IOAPerc'] = item['IOAPerc'];
                                         catdata['CalcType'] = item['CalcType'];
                                         catdata['PercntCount'] = item['PercntCount'];
                                         catdata['NonPercntCount'] = item['NonPercntCount'];
                                         catdata['DummyScore'] = item['DummyScore'];
                                         symb['symbol'] = item['Shape'].toLowerCase();
                                         symb['enabled'] = true;
                                         symb['states'] = { hover: { enabled: true } }
                                         catdata['symbol'] = symb;
                                         catdata['yAxis'] = 0;
                                         catdata['value'] = item['Score'];
                                         data.push(catdata);
                                         yAxis[catdata['name']] = catdata['yAxis'];
                                         color[catdata['name']] = catdata['Color'];
                                         symbol[catdata['name']] = catdata['symbol'];
                                         calc[catdata['name']] = catdata['CalcType'];
                                         perc[catdata['name']] = catdata['PercntCount'];
                                         nonperc[catdata['name']] = catdata['NonPercntCount'];
                                         dummy[catdata['name']] = catdata['DummyScore'];
                                     } else {
                                         var catdata = {};
                                         var symb = {};
                                         catdata['date'] = extractdate(item['AggredatedDate']);
                                         catdata['name'] = item['ColName'] + '-' + item['RptLabel'];
                                         catdata['Color'] = item['Color'];
                                         catdata['IOAPerc'] = item['IOAPerc'];
                                         catdata['CalcType'] = item['CalcType'];
                                         catdata['PercntCount'] = item['PercntCount'];
                                         catdata['NonPercntCount'] = item['NonPercntCount'];
                                         catdata['DummyScore'] = item['DummyScore'];
                                         symb['symbol'] = item['Shape'].toLowerCase();
                                         symb['enabled'] = true;
                                         symb['states'] = { hover: { enabled: true } }
                                         catdata['symbol'] = symb;
                                         catdata['yAxis'] = 1;
                                         catdata['value'] = item['DummyScore'];
                                         data.push(catdata);
                                         yAxis[catdata['name']] = catdata['yAxis'];
                                         color[catdata['name']] = catdata['Color'];
                                         symbol[catdata['name']] = catdata['symbol'];
                                         calc[catdata['name']] = catdata['CalcType'];
                                         perc[catdata['name']] = catdata['PercntCount'];
                                         nonperc[catdata['name']] = catdata['NonPercntCount'];
                                         dummy[catdata['name']] = catdata['DummyScore'];
                                     }

                                 }
                             });

                             // var newdata = [...new Set(data.map(item => item.name))];
                             var newData = []; var names = [];
                             data.forEach(function (item) {
                                 if (!names[item.name]) {
                                     names[item.name] = true;
                                     newData.push(item.name);
                                 }
                             });
                             if ("<%=inctype%>" == "True" || "<%=inctype%>" == "true") {
                                         var typechart = 'scatter';
                                     }
                                     else {
                                         var typechart = 'line';
                                     }
                                     var ser = newData.map(function (name) {
                                         return {
                                             type: typechart,
                                             name: name,
                                             yAxis: yAxis[name],
                                             color: color[name],
                                             marker: symbol[name],
                                             dataGrouping: { enabled: true },
                                             data: data.filter(function (item) {
                                                 return item.name === name;
                                             }).map(function (item) {
                                                 return {
                                                     x: item.date,
                                                     y: item.value,
                                                     IOAPerc: item.IOAPerc,
                                                 };
                                             })

                                         };
                                     });
                                     //find maximum of y axis start
                                     var yaxisval = newData.map(function (name) {
                                         return {
                                             yAxis: yAxis[name],
                                             calc: calc[name],
                                             perc: perc[name],
                                             nonperc: nonperc[name],
                                             dummy: dummy[name]
                                         }
                                     });
                                     var lmax = -Infinity; var rmax = -Infinity;
                                     for (var i = 0; i < res.length; i++) {
                                         var primeY = res[i].Score;
                                         var SecY = res[i].DummyScore;
                                         if (primeY > lmax) {
                                             lmax = primeY;
                                         }
                                         if (SecY > rmax) {
                                             rmax = SecY;
                                         }
                                     }
                                     var leftmax; var rightmax; var rintrvl; var lintrvl;
                                     for (var i = 0; i < yaxisval.length; i++) {
                                         if (yaxisval[i].yAxis == 0) {
                                             var pattern = /%/;
                                             if (pattern.test(yaxisval[i].calc) || yaxisval[i].calc == "percent" || yaxisval[i].calc == "Percent") {
                                                 leftmax = 100
                                             }
                                             else {
                                                 if (lmax == 0) {
                                                     leftmax = 1;
                                                 }
                                                 else {
                                                     leftmax = lmax
                                                 }

                                             }
                                             if (yaxisval[i].perc > 0) {
                                                 lintrvl = 10;
                                             } else if (lmax === 0) {
                                                 lintrvl = 1;
                                             } else if (lmax === 1) {
                                                 lintrvl = 1;
                                             } else if (lmax >= 50) {
                                                 lintrvl = 10;
                                             } else if (lmax >= 20) {
                                                 lintrvl = 5;
                                             } else {
                                                 lintrvl = 1;
                                             }
                                         }
                                         if (yaxisval[i].yAxis == 1) {
                                             var pattern = /%/;
                                             if (pattern.test(yaxisval[i].calc) || yaxisval[i].calc == "percent" || yaxisval[i].calc == "Percent") {
                                                 rightmax = 100
                                             }
                                             else {
                                                 if (rmax == 0) {
                                                     rightmax = 1;
                                                 }
                                                 else {
                                                     rightmax = rmax
                                                 }

                                             }
                                             if (yaxisval[i].perc === 2 && yaxisval[i].nonperc === 0) {
                                                 rintrvl = 10;
                                             }
                                             else {
                                                 rightmax = rmax;
                                                 if (rmax === 0) {
                                                     rintrvl = 1;
                                                 } else if (rmax === 1) {
                                                     rintrvl = 1;
                                                 } else if (rmax >= 1000) {
                                                     rintrvl = 20;
                                                 } else if (rmax >= 50) {
                                                     rintrvl = 10;
                                                 } else if (rmax >= 20) {
                                                     rintrvl = 5;
                                                 } else {
                                                     rintrvl = 1;
                                                 }
                                             }
                                         }
                                     }
                                     //find maximum of y axis end
                                     //set duplicate series (case:no series) start
                                     dupdata = [];
                                     if (data.length === 0) {
                                         var samdata = {};
                                         var sd = extractdateOther(sdate); var ed = extractdateOther(edate);
                                         samdata['data'] = [{ x: sd, y: null }, { x: ed, y: null }];
                                         samdata['showInLegend'] = false;
                                         dupdata.push(samdata);
                                         ser = dupdata;
                                         leftmax = 1; rightmax = 1;
                                     }
                                     //set duplicate series (case:no series) end
                                     //trend start
                                     var arrData = []; var yAxis1 = []; var color1 = []; var symbol1 = [];
                                     $.map(res, function (item, index) {
                                         var trnddata = {};
                                         var symbarr = {};
                                         if (item['CalcType'] != 'Event' && "<%=trend%>" == "Quarter") {
                                     var yax;

                                     if (item['LeftYaxis'] != null && item['RightYaxis'] == null || item['LeftYaxis'] == null && item['RightYaxis'] != null) {
                                         yax = 0;
                                     }
                                     else if (item['CalcType'] != "Total Duration" && item['CalcType'] != "Total Correct" && item['CalcType'] != "Total Incorrect" && item['CalcType'] != "Frequency" && item['CalcType'] != "Avg Duration" && item['CalcType'] != "Customize" && item['CalcType'] != "Event" && item['CalcType'] != item['RightYaxis'] || item['RptLabel'] == item['LeftYaxis']) {
                                         yax = 0;
                                     }
                                     else {
                                         yax = 1;
                                     }

                                     if (yax == 0 || item['RptLabel'] == item['LeftYaxis']) {
                                         trnddata['date'] = extractdate(item['AggredatedDate']);
                                         trnddata['name'] = item['ColName'] + '-' + item['CalcType'] + 'trend';
                                         trnddata['value'] = item['Trend'];
                                         trnddata['Color'] = "Gray";
                                         symbarr['enabled'] = false;
                                         trnddata['symbol'] = symbarr;
                                         trnddata['yAxis1'] = 0;
                                         arrData.push(trnddata);
                                         yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                         color1[trnddata['name']] = trnddata['Color'];
                                         symbol1[trnddata['name']] = trnddata['symbol'];
                                     }
                                     else {
                                         trnddata['date'] = extractdate(item['AggredatedDate']);
                                         trnddata['name'] = item['ColName'] + '-' + item['CalcType'] + 'trend';
                                         trnddata['value'] = item['Trend'];
                                         trnddata['Color'] = "Gray";
                                         symbarr['enabled'] = false;
                                         trnddata['symbol'] = symbarr;
                                         trnddata['yAxis1'] = 1;
                                         arrData.push(trnddata);
                                         yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                         color1[trnddata['name']] = trnddata['Color'];
                                         symbol1[trnddata['name']] = trnddata['symbol'];
                                     }
                                 }
                             });
                             var newData1 = []; var name1 = [];
                                     // var newdata1 = [...new Set(arrData.map(item => item.name))];
                             arrData.forEach(function (item) {
                                 if (!name1[item.name]) {
                                     name1[item.name] = true;
                                     newData1.push(item.name);
                                 }
                             });
                             var ser1 = newData1.map(function (name) {
                                 return {
                                     name: "",
                                     yAxis: yAxis1[name],
                                     color: color1[name],
                                     marker: symbol1[name],
                                     dashStyle: "DashDotDot",
                                     lineWidth: 2,
                                     showInLegend: false,
                                     data: arrData.filter(function (item) {
                                         return item.name === name;
                                     }).map(function (item) {
                                         return {
                                             x: item.date,
                                             y: item.value
                                         };
                                     })
                                 };
                             });
                                     //trend  end
                             ser = ser.concat(ser1);
                                     //Plot Data End

                                     //Event plot Start
                             var plotdata = [];
                             var uniqueItems = [];
                             var maxYPosition = 300;
                             var currentYPosition = 0;
                             $.map(res, function (item, index) {
                                 var style; var wid;
                                 if (item['CalcType'] == "Event" && item['EventType'] != null) {
                                     var isUnique = true;
                                     for (var i = 0; i < uniqueItems.length; i++) {
                                         var uniqueItem = uniqueItems[i];
                                         if (
                                             uniqueItem['EventName'] === item['EventName'] &&
                                             uniqueItem['AggredatedDate'] === item['AggredatedDate'] &&
                                             uniqueItem['EventType'] === item['EventType']
                                         ) {
                                             isUnique = false;
                                             break;
                                         }
                                     }

                                     if (isUnique) {
                                         uniqueItems.push(item);
                                         if (item['EventType'] == "Major") {
                                             style = 'solid';
                                             wid = 1;
                                         }
                                         else {
                                             style = 'dot';
                                             wid = 2;
                                         }

                                         var plotdict = new Object;
                                         plotdict['color'] = "black";
                                         plotdict['dashStyle'] = style;
                                         plotdict['value'] = extractdate(item['AggredatedDate']);
                                         plotdict['width'] = wid;
                                         if (item['EventName'] == null) {
                                             item['EventName'] = "";
                                         }

                                         var datescorealign = findmaxscorefordate(res);
                                         var arr = checkarrow(res);
                                         var str = item['EventName'];
                                         var txtal; var xax;
                                         var valign;
                                         if (item['EventType'] == "Major") {
                                             txtal = 'right';
                                             xax = -2;
                                             valign = getValueByAggredatedDate(item['AggredatedDate'], datescorealign, arr)
                                         }
                                         else {
                                             txtal = 'left';
                                             xax = 9;
                                             valign = getValueByAggredatedDate(item['AggredatedDate'], datescorealign, arr);
                                         }
                                         plotdict['label'] = {
                                             text: str,
                                             // textAlign: txtal,
                                             formatter: function () {
                                                 var labelText = this.options.text;
                                                 labelText = labelText.replace(/↑/g, '<span style="font-size: 15px;">↑</span>')
                                                     .replace(/↓/g, '<span style="font-size: 15px;">↓</span>');
                                                 return labelText;
                                             },
                                             useHTML: true,
                                             style: {
                                                 fontWeight: 'normal',
                                                 color: 'black',
                                                 fontSize: sizeOffont,
                                                 fontFamily: 'Arial',
                                                 textShadow: 'none',
                                             },
                                             // align: 'right',
                                             //  textAlign: 'right',
                                             x: xax,
                                             verticalAlign: valign,
                                             rotation: -90
                                         };
                                         plotdict['label']['overflow'] = 'justify';
                                         plotdict['label']['crop'] = false;
                                         plotdata.push(plotdict);

                                     }
                                 }
                                 plotdata = plotlinedata(plotdata);
                             });
                                     //arrow start

                             var arrows = [];
                             var plotarr = new Object;
                             plotarr['type'] = 'scatter';
                             plotarr['showInLegend'] = false;
                             if ("<%=inctype%>" == "True" || "<%=inctype%>" == "true") {
                             plotarr['color'] = 'black',
                             plotarr['marker'] = {
                                 symbol: 'circle',
                                 radius: 4,
                             }
                         }
                         else {
                             plotarr['marker'] = {
                                 symbol: 'url(../Scripts/arrownote.png)',
                                 width: 8,
                                 height: 10,
                             }
                         }
                         $.map(res, function (item, index) {
                             if (item['ArrowNote'] != null) {
                                 arrows.push({ x: extractdate(item['AggredatedDate']), y: item['Score'], arrowval: item['ArrowNote'] })
                                 arrows.push({ x: extractdate(item['AggredatedDate']), y: item['DummyScore'], arrowval: item['ArrowNote'] })
                             }
                         });
                         var mergedarr = [];
                         var mergedKeys = {};

                         for (var i = 0; i < arrows.length; i++) {
                             var key = arrows[i].x + '-' + arrows[i].y;
                             if (!mergedKeys[key]) {
                                 mergedarr.push(arrows[i]);
                                 mergedKeys[key] = true;
                             } else {
                                 for (var j = 0; j < mergedarr.length; j++) {
                                     if (mergedarr[j].x === arrows[i].x && mergedarr[j].y === arrows[i].y) {
                                         mergedarr[j].arrowval += ',' + arrows[i].arrowval;
                                         break;
                                     }
                                 }
                             }
                         }
                         plotarr['data'] = mergedarr;

                         ser.push(plotarr);
                                     //arrow end
                                     //Event plot End
                                     //Start Highchart
                         charts1 = Highcharts.chart('cont', {
                             chart: {
                                 plotBorderWidth: 2,
                                 plotBorderColor: 'black',
                                 spacingTop: 20,
                                 spacingRight: 20,
                                 spacingBottom: 20,
                                 spacingLeft: 20,
                                 marginTop: 100,
                                 events: {
                                     load: function () {
                                         var chart = this;
                                         var completed = 0;
                                         var totalSeries = chart.series.length;

                                         // If no series, call immediately
                                         if (totalSeries === 0) {
                                             if (typeof callback === 'function') callback();
                                             return;
                                         }

                                         // Wait until all series animations are finished
                                         for (var i = 0; i < chart.series.length; i++) {
                                             (function (series) {
                                                 Highcharts.addEvent(series, 'afterAnimate', function () {
                                                     completed++;
                                                     if (completed === totalSeries) {
                                                         if (typeof callback === 'function') callback();
                                                     }
                                                 });
                                             })(chart.series[i]);
                                         }

                                         // If animations are disabled, call immediately
                                         var animationEnabled = true;
                                         if (chart.options.plotOptions &&
                                             chart.options.plotOptions.series &&
                                             chart.options.plotOptions.series.animation === false) {
                                             animationEnabled = false;
                                         }
                                         if (!animationEnabled) {
                                             if (typeof callback === 'function') callback();
                                         }
                                     }
                                 }
                             },

                             title: {
                                 text: maintitle,
                                 useHTML: true,
                                 style: {
                                     fontWeight: 'bold',
                                     color: 'black',
                                     fontSize: sizeOffont,
                                     fontFamily: 'Arial'
                                 },
                             },
                             subtitle: {
                                 text: treatment,
                                 x: 50,
                                 useHTML: true,
                                 style: {
                                     fontWeight: 'bold',
                                     color: 'black',
                                     fontSize: sizeOffont,
                                     fontFamily: 'Arial'
                                 },
                                 align: 'left',
                             },
                             credits: {
                                 enabled: false
                             },
                             yAxis:
                       [{ // Primary yAxis 
                           min: 0,
                           gridLineWidth: 0,
                           tickInterval: lintrvl,
                           softMin: 0,
                           max: leftmax,
                           title: {
                               text: leftY,
                               useHTML: true,

                               style: {
                                   fontWeight: 'bold',
                                   color: 'black',
                                   fontSize: sizeOffont,
                                   fontFamily: 'Arial'

                               }
                           },
                           labels: {
                               style: {
                                   color: 'black',
                                   fontSize: sizeOffont,
                                   fontWeight: 'normal',
                                   fontFamily: 'Arial'

                               }
                           },

                       }, { // Secondary yAxis
                           visible: secstatus,
                           gridLineWidth: 0,
                           tickInterval: rintrvl,
                           min: 0,
                           softMin: 0,
                           max: rightmax,
                           title: {
                               text: rightY,
                               useHTML: true,
                               style: {
                                   fontWeight: 'bold',
                                   color: 'black',
                                   fontSize: sizeOffont,
                                   fontFamily: 'Arial'
                               }
                           },
                           labels: {
                               style: {
                                   color: 'black',
                                   fontSize: sizeOffont,
                                   fontWeight: 'normal',
                                   fontFamily: 'Arial'

                               }
                           },
                           opposite: true,
                       }],

                             xAxis: {
                                 lineColor: 'black',
                                 lineWidth: 1,
                                 title: {
                                     text: 'Dates',
                                     useHTML: true,
                                     style: {
                                         fontWeight: 'bold',
                                         color: 'black',
                                         fontSize: sizeOffont,
                                         fontFamily: 'Arial'

                                     }

                                 },
                                 min: extractdateOther(sdate.replace('/', '-')),
                                 max: extractdateOther(edate.replace('/', '-')),
                                 plotLines: plotdata,
                                 tickInterval: 2 * 24 * 3600 * 1000,
                                 labels: {
                                     rotation: -90,
                                     formatter: function () {
                                         return Highcharts.dateFormat('%m/%d/%Y', this.value); // Format the date as desired
                                     },
                                     style: {
                                         fontWeight: 'normal',
                                         color: 'black',
                                         fontSize: sizeOffont,
                                         fontFamily: 'Arial'

                                     }
                                 }
                             },
                             legend: {
                                 layout: 'horizontal',
                                 align: 'right',
                                 verticalAlign: 'bottom',
                                 labelFormatter: function () {
                                     return '<span style="color: black; font-weight: normal;font-size:' + sizeOffont + '; font-family:Arial">' + this.name + '</span>';
                                 }
                             },
                             plotOptions: {
                                 series: {
                                     turboThreshold: 5000,
                                     dataLabels: {
                                         allowOverlap: true,
                                         enabled: true,
                                         rotation: -90,
                                         align: 'top',
                                         y: -5,
                                         overflow: 'justify',
                                         crop: false,
                                         //style: {
                                         //    textOutline: '1px contrast' 
                                         //},

                                         formatter: function () {
                                             var point = this.point;
                                             if (point.IOAPerc) {
                                                 if (point.y > 0) {
                                                     var words = point.IOAPerc.split(' ');
                                                     var newpoint = '';
                                                     for (var i = 0; i < words.length; i++) {
                                                         if (words[i].endsWith('%') && i < words.length - 1) {
                                                             newpoint += '<span style="font-size:' + sizeOffont + '; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span><br> ';
                                                         } else {
                                                             newpoint += '<span style="font-size:' + sizeOffont + '; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span> ';
                                                         }
                                                     }
                                                     return '<div style="white-space: nowrap; font-weight: normal;font-size:' + sizeOffont + '; font-family: Arial; color: black">' + newpoint + '</div>';
                                                 }
                                                 else {
                                                     return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size:' + sizeOffont + ';font-family:Arial;color:black;text-shadow: none;font-weight:normal;">' + point.IOAPerc + '</span>';
                                                 }
                                             }
                                             else if (point.arrowval) {
                                                 return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size:' + sizeOffont + '; font-family:Arial;font-color:black;text-shadow: none;font-weight:normal;">' + point.arrowval;
                                             }
                                             else {
                                                 return '';
                                             }
                                         },
                                     },
                                 },
                             },

                             tooltip: {
                                 formatter: function () {
                                     return '<b>' + Highcharts.dateFormat('%m/%d/%Y', new Date(this.x)) + '</b>: ' + this.y;
                                 }
                             },

                             series: ser,
                             exporting: {
                                 sourceWidth: 935.43307,
                                 sourceHeight: 688.901215752,
                                 // scale: 2 (default)
                                 //chartOptions: {
                                 //    subtitle: null
                                 //}
                             }
                         });
                         }
                         var medjson = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, SchoolId: scid });
                                 if (("<%=med%>" == "true" || "<%=med%>" == "True") && ("<%=medno%>" == "false" || "<%=medno%>" == "False")) {
                             $.ajax({
                                 type: "POST",
                                 url: "LessonReportsWithPaging.aspx/getmedAcademicReport",
                                 data: medjson,
                                 contentType: "application/json; charset=utf-8",
                                 async: false,
                                 dataType: "json",
                                 success: function (response) {
                                     var resp = JSON.parse(response.d);
                                     var medicData = []; var val = 0;
                                     resp.forEach(function (item) {
                                         item.duration = extractdate(item.EndTime) - extractdate(item.EvntTs);
                                     });
                                     resp.sort(function (a, b) {
                                         return b.duration - a.duration;
                                     });
                                     $.map(resp, function (item, index) {
                                         var meddata = {};
                                         meddata['name'] = item['EventName'] + item['Comment'];
                                         meddata['x'] = extractdate(item['EvntTs']);
                                         meddata['x2'] = extractdate(item['EndTime']);
                                         meddata['y'] = val;
                                         medicData.push(meddata);
                                         val = val + 1
                                     });
                                     charts2 = Highcharts.chart('medcont', {
                                         chart: {
                                             type: 'xrange',
                                             plotBorderWidth: 2,
                                             plotBorderColor: 'black',
                                             spacingTop: 20,
                                             spacingRight: 20,
                                             spacingBottom: 20,
                                             spacingLeft: 20,
                                             
                                         },
                                         title: {
                                             text: medtitle,
                                             useHTML: true,
                                             style: {
                                                 fontWeight: 'bold',
                                                 color: 'black',
                                                 fontSize: sizeOffont,
                                                 fontFamily: 'Arial'
                                             },
                                         },
                                         subtitle: {
                                             text: ' Medication Details',
                                             x: 50,
                                             align: 'left',
                                             fontSize: '55px',
                                             style: {
                                                 color: '#000000',
                                                 fontWeight: 'bold',
                                                 fontSize: sizeOffont,
                                                 fontFamily: 'Arial'

                                             }
                                         },
                                         xAxis: {

                                             //visible: false,
                                             type: 'datetime',
                                             //linkedTo: 0,
                                             min: extractdateOther(sdate.replace('/', '-')),
                                             max: extractdateOther(edate.replace('/', '-')),
                                             tickInterval: 2 * 24 * 3600 * 1000,
                                             tickPositioner: function () {
                                                 var ticks = [];
                                                 var interval = this.options.tickInterval;
                                                 var currentTick = this.min;

                                                 while (currentTick <= this.max) {
                                                     ticks.push(currentTick);
                                                     currentTick += interval;
                                                 }

                                                 return ticks;
                                             },
                                             lineWidth: 0,
                                             minorGridLineWidth: 0,
                                             lineColor: 'transparent',
                                             minorTickLength: 0,
                                             tickLength: 0,
                                             startOnTick: true,
                                             endOnTick: true,
                                             labels: {
                                                 enabled: false,
                                                 //  rotation: -90,
                                                 //formatter: function () {
                                                 //    return Highcharts.dateFormat('%m/%d/%Y', this.value);
                                                 //}
                                             }
                                         },
                                         credits: {
                                             enabled: false
                                         },
                                         yAxis: {
                                             gridLineWidth: 0,
                                             offset: 60,
                                             title: {
                                                 text: ''
                                             },
                                             labels: {
                                                 enabled: false
                                             },
                                             reversed: true
                                         },
                                         tooltip: {
                                             formatter: function () {
                                                 return Highcharts.dateFormat('%m/%d/%Y', this.point.x) + '-' + Highcharts.dateFormat('%m/%d/%Y', this.point.x2);

                                             }
                                         },
                                         series: [{
                                             data: medicData,
                                             showInLegend: false,
                                             dataLabels: {
                                                 enabled: true,
                                                 formatter: function () {
                                                     return this.point.name;
                                                 }
                                             },

                                         }],
                                         exporting: {
                                             sourceWidth: 935.43307,
                                             sourceHeight: 164.05358746,
                                             // scale: 2 (default)
                                             //chartOptions: {
                                             //    subtitle: null
                                             //}
                                         }
                                     });
                                 },
                                 error: OnErrorCall
                             });
                             function OnErrorCall(response) {

                                 alert("Something went wrong! Error: ");

                             }

                         }
                             },
                             error: OnErrorCall_
                         });
                 function OnErrorCall_(response) {

                     alert("Something went wrong! Error: ");
                 }
              
                 //setTimeout(callback, 700);
             }
             function captureAndSend(lessid, callback) {
                 //html2canvas($("#HighchartGraph")[0]).then(function (canvas) {
                 //    base64 = canvas.toDataURL();
                 //    var dataToSend = {
                 //        base64: base64, chartId: lessid
                 //    };

                 var element = document.getElementById("HighchartGraph");

                 var scale = window.devicePixelRatio;
                 html2canvas(element, {
                     scale: scale,
                     width: element.offsetWidth * scale,
                     height: element.offsetHeight * scale
                 }).then(function (canvas) {
                     var base64 = canvas.toDataURL();
                     var dataToSend = {
                         base64: base64,
                         chartId: lessid
                     };
                   
                     $.ajax({
                         type: "POST",
                         url: "LessonReportsWithPaging.aspx/getgraphs",
                         data: JSON.stringify(dataToSend),
                         contentType: "application/json; charset=utf-8",
                         async: false,
                         dataType: "json",
                         success: function (response) {

                         },
                         error: function (error) {
                             console.error("Error sending images to the server: " + error);
                         },
                         
                     });
                 });
                 setTimeout(callback, 300);
             }
            

        

             function findmaxscorefordate(data)
             {
                 var resultObj = {};

                 for (var i = 0; i < data.length; i++) {
                     var item = data[i];
                     var date = item.AggredatedDate;

                     if (!resultObj[date]) {
                         resultObj[date] = {
                             AggredatedDate: date,
                             maxScore: item.Score
                         };
                     } else {
                         if (item.Score > resultObj[date].maxScore) {
                             resultObj[date].maxScore = item.Score;
                         }
                     }
                 }

                 var result = [];
                 for (var key in resultObj) {

                     var maxScore = resultObj[key].maxScore;

                     var value = (maxScore !== null && maxScore > 0)
                         ? "middle"
                         : "bottom";

                     result.push({
                         AggredatedDate: key,
                         value: value
                     });
                 }
                 return result;
             }
             function getValueByAggredatedDate(dateToSearch, resultArray, arr) {
                 for (var i = 0; i < resultArray.length; i++) {
                     if (resultArray[i].AggredatedDate == dateToSearch) {
                         if (arr.includes(dateToSearch)) {
                             return "middle";
                         }
                         else{
                             return "bottom";
                         }
                     }
                 }
                 return "middle"; 
             }
             function checkarrow(data) {
                 var dateArray = [];

                 for (var i = 0; i < data.length; i++) {
                     var item = data[i];

                     if (item.ArrowNote !== null && item.Score >= 0) {
                         dateArray.push(item.AggredatedDate);
                     }
                 }
                 return dateArray;
             }
        </script>
    </form>
</body>
    
</html>
