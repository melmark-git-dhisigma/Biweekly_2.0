<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BiweeklyBehaviorGraph.aspx.cs" Inherits="StudentBinder_BiweeklyBehaviorGraph" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        <script src="../Scripts/html2canvas.min.js"></script>
            <script src="../Scripts/es6-promise.auto.min.js"></script>
        <script src="../Scripts/es6-promise.min.js"></script>


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
        #prdiv div {
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
        /*LOADING IMAGE CLOSE */
        #RV_Behavior img
        {
            width:100% !important;
            height:100% !important;
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
    </style>
    <script type="text/javascript">
        //$(window).load(function () {

        //});
        $(document).ready(function () {
            $('.loading').fadeOut('slow', function () {
                $('#fullContents').fadeIn('fast');
            });
            $('.homeloading').fadeOut('fast');

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


            

            $('#<%=btnExport.ClientID %>').click(function () {
                $('.loading').fadeIn('fast');
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
        function LoadGraph() {
            $('.loading').fadeOut('slow', function () {
                $('#fullContents').fadeIn('fast');
            });
            $('.homeloading').fadeOut('fast');
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

        function printTrigger(elementsrc) {
            window.document.forms[0].target = '_blank'; setTimeout(function () { window.document.forms[0].target = ''; }, 500);
        }

        function eventinpopup() {
            $('#<%=chkbxevents.ClientID %>').attr('checked', false);

             if ($('#<%=chkbxmajor.ClientID %>').is(':checked') && $('#<%=chkbxminor.ClientID %>').is(':checked') && $('#<%=chkbxarrow.ClientID %>').is(':checked')) {
                 $('#<%=chkbxevents.ClientID %>').attr('checked', true);
            }
        }

        $(function () {

            adjustStyle();
            $(window).resize(function () {
                adjustStyle();
            });
        });


        function adjustStyle() {
            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad != null) {

                $('#graphPopup').css('width', '91% !Important');
                $('#RV_LPReport_ctl09').css("overflow", 'scroll');

            }
            else {
                $('#graphPopup').css('width', '63% !Important');

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
                //$('#Button7').css("display", "none");
                //$('#btnLessonSubmit').css("display", "none");
            }
            else {
                $('#btnRefresh').css("display", "none");
                //$('#Button7').css("display", "block");
                //$('#btnLessonSubmit').css("display", "block");
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
			if (val != "true") {
                    $('#overlay').fadeIn('slow', function () {
                    });
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

            if (valExport == "true") {
                $('#overlay').show();
            }
        };

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
        }
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
        <div id="popup" class="popup" runat="server" >
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


                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                        </td>
                        <td></td>
                    </tr>

                </table>
                <div runat="server" id="LessonDiv" visible="false">
                    <table style="width: 100%">
                            <tr>
                                <td colspan="4" runat="server" id="tdMsg1"></td>
                            </tr>
                            <%--<tr style="visibility: hidden;">
                        <td style="text-align: center" class="tdText">Report
                Start Date
                            <br />
                            <br />
                            <br />
                            Report End Date
                            <br />
                            <br />
                            <br />
                            Select Behavior
                        </td>
                        <td style="text-align: left;width:25%">
                            <asp:TextBox ID="txtSdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                            <br />
                            <br />
                            <asp:TextBox ID="txtEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                            <br />
                            <br />
                            <ContentTemplate>
                               <asp:CheckBox ID="CheckBoxRep1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Active" Checked="True" AutoPostBack="true"/>
                               <asp:CheckBox ID="CheckBoxRep2" runat="server" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Inactive"  Checked="False" AutoPostBack="true"/> 
                               <asp:DropDownList ID="ddlRepBehavior" runat="server" CssClass="drpClass"></asp:DropDownList>
                            </ContentTemplate>
                        </td>
                        <td style="text-align: left;line-height:0px" class="tdText" rowspan="2" >
                            <asp:RadioButtonList ID="rbtnIncidentalRepRegular" runat="server" RepeatDirection="Horizontal"  >
                            <asp:ListItem Value="Incidental">Incidental Teaching Graph</asp:ListItem>
                            <asp:ListItem Value="Regular" Selected="True">Regular Graph</asp:ListItem>
                        </asp:RadioButtonList>
                             <asp:RadioButtonList ID="rbtnRepClassType" runat="server" RepeatDirection="Horizontal" Visible="true" >
                            <asp:ListItem Value="Day">Day</asp:ListItem>
                            <asp:ListItem Value="Residence">Residence</asp:ListItem>
                            <asp:ListItem Value="Day,Residence" Selected="True">Both</asp:ListItem>
                        </asp:RadioButtonList>
                            <br/>
                            <asp:CheckBox ID="chkbxRepevents" runat="server" Text="Display Events"></asp:CheckBox><br />
                             <div id="divpopupRepEvents" style="display:none">
                                    <asp:checkbox id="chkbxRepmajor" runat="server" text="Display Major Events" onclick="eventinpopup();" ></asp:checkbox>
                                    <br /> 
                                    <asp:checkbox id="chkbxRepminor" runat="server" text="Display Minor Events" onclick="eventinpopup();" ></asp:checkbox>
                                    <br />
                                    <asp:checkbox id="chkbxReparrow" runat="server" text="Display Arrow Notes" onclick="eventinpopup();" ></asp:checkbox>
                                    <br />
                                </div>
                            <asp:CheckBox ID="chkbxRepIOA" runat="server" Text="Include IOA"></asp:CheckBox><br />
                            <asp:CheckBox ID="chkrepReptrend" runat="server" Text="Include Trend Line"></asp:CheckBox>
                            <br />
                            <asp:CheckBox ID="chkRepmedication" runat="server" Text="Include Medication"></asp:CheckBox><br />
                            <asp:CheckBox ID="chkReprate" runat="server" Text="Include Rate Graph" ></asp:CheckBox>
                           
                            
                        </td>
                        <td style="text-align: right" rowspan="2">
                            <asp:Button ID="btnBehSubmit" Style="float: right; margin: 0 1px 0 1px" runat="server" ToolTip="Show Graph" CssClass="showgraph" OnClick="btnBehSubmit_Click" />
                            <%--<asp:Button ID="btnBehExport" Style="float: right; margin: 0 1px 0 1px" runat="server" ToolTip="Export To PDF" CssClass="pdfPrint" OnClick="btnBehExport_Click" />
                            <asp:Button ID="btnBehPrint" Style="float: right; margin: 0 1px 0 1px" runat="server" ToolTip="Print" CssClass="print" OnClick="btnBehPrint_Click" Visible="false" />
                        </td>
                        <td style="text-align: left;width:10%" rowspan="2"></td>
                    </tr>--%>

                    <tr>
                        <td style="text-align: center" class="tdText">
                &nbsp;</td>
                        <td style="text-align: left">
                            

                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: center" class="tdText">&nbsp;&nbsp; &nbsp;</td>
                        <td style="text-align: left">
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
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtSdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                            <br />
                            <br />
                            <asp:TextBox ID="txtEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                        </td>
                        <td style="text-align: center" class="tdText"></td>
                        <td style="text-align: left" rowspan="2">
                    <asp:CheckBox ID="chkbxevents" runat="server" Text="Display Events" OnCheckedChanged="EvntCheckBox_CheckedChanged" AutoPostBack="true"></asp:CheckBox><br />
                        <div id="divpopupEvents" style="display:none">
                            <asp:checkbox id="chkbxmajor" runat="server" text="Display Major Events" onclick="eventinpopup();" ></asp:checkbox>
                            <br /> 
                            <asp:checkbox id="chkbxminor" runat="server" text="Display Minor Events" onclick="eventinpopup();" ></asp:checkbox>
                            <br />
                            <asp:checkbox id="chkbxarrow" runat="server" text="Display Arrow Notes" onclick="eventinpopup();" ></asp:checkbox>
                            <br />
                        </div>
                    <asp:CheckBox ID="chkbxIOA" runat="server" Text="Include IOA"></asp:CheckBox><br />
                    <asp:CheckBox ID="chkreptrend" runat="server" Text="Include Trend Line"></asp:CheckBox>
                    <br />
                    <asp:CheckBox ID="chkmedication" runat="server" Text="Include Medication"></asp:CheckBox><br />
                    <asp:CheckBox ID="chkrate" runat="server" Text="Include Rate Graph" ></asp:CheckBox>
                            <br/>
                        <asp:RadioButtonList ID="rbtnClassType" runat="server" RepeatDirection="Horizontal" Visible="true" >
                    <asp:ListItem Value="Day">Day</asp:ListItem>
                    <asp:ListItem Value="Residence">Residence</asp:ListItem>
                    <asp:ListItem Value="Day,Residence" Selected="True">Both</asp:ListItem>
                </asp:RadioButtonList>
                            <asp:RadioButtonList ID="rbtnIncidentalRegular" runat="server" RepeatDirection="Horizontal"  >
                    <asp:ListItem Value="Incidental">Incidental Teaching Graph</asp:ListItem>
                    <asp:ListItem Value="Regular" Selected="True">Regular Graph</asp:ListItem>
                </asp:RadioButtonList>
                    
                           
                            
                </td>
                        <td style="text-align: left; vertical-align: top;" rowspan="2">
                            <asp:Button ID="btnsubmit" Style="float: right; margin: 0 1px 0 1px" runat="server" ToolTip="Show Graph" CssClass="showgraph" OnClick="btnsubmit_Click" OnClientClick="loadWait();"/>
                            <asp:Button ID="btnsubmith" Style="float: right;display: none; margin: 0 1px 0 1px" runat="server" ToolTip="Show Graph" CssClass="showgraph" OnClick="btnsubmit_Click" OnClientClick="showPopup();"/>
                            <asp:CheckBox id="highcheck" runat="server" Text="" onchange="btnsubmitVisibility();"></asp:CheckBox>
                            <%--<asp:Button ID="btnExport" Style="float: right; margin: 0 1px 0 1px" runat="server" ToolTip="Export To PDF" CssClass="pdfPrint" OnClick="btnExport_Click" />--%>
                            <%--<asp:Button ID="btnPrint" Style="float: right; margin: 0 1px 0 1px" runat="server" ToolTip="Print" CssClass="print" OnClick="btnPrint_Click" Visible="false" />--%>
                        </td>
                    </tr>
                            <tr>
                                <td style="text-align: center" class="tdText"></td>
                                <td style="text-align: center"><asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Active" Checked="True" AutoPostBack="true"/>
                                        <asp:CheckBox ID="CheckBox2" runat="server" OnCheckedChanged="CheckBox2_CheckedChanged" Text="Inactive"  Checked="False" AutoPostBack="true"/> 
                                        <%--<asp:DropDownList ID="ddlBehavior" runat="server" CssClass="drpClass"></asp:DropDownList>--%>
                                    </ContentTemplate>
                                    </asp:UpdatePanel></td>
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
             <div id="downloadPopup" runat="server" class="web_dialog" style="width: 600px; ">

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
                                             <%--<input type="button" value="Done" class="NFButton" id="btnDone" onclick="CloseDownload();" />--%>
                                            <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton"  onclientclick="CloseDownload();" onClick="btnDone_Click" />

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
                            <asp:Button ID="btnPrevious" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0px 250px" Visible="false" Text="Previous" OnClick="btnPrevious_Click" OnClientClick="javascript:showPopup();" />
                             <asp:DropDownList ID="ddlLessonplan" runat="server" CssClass="drpClass" Style="float: left; margin: 0 1px 0px 10px" Height="26px" Width="290px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlLessonplan_SelectedIndexChanged"  onchange="showPopup()">
                             </asp:DropDownList>
                            <asp:Button ID="btnNext" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0 10px" Visible="false" Text="Next" OnClick="btnNext_Click" OnClientClick="javascript:showPopup();" />
                                </ContentTemplate>
                                </asp:UpdatePanel> 
                            <%--<asp:Button ID="Button7" style="float:right;margin:0 1px 0 1px" runat="server" Text="Execute" OnClick="Button1_Click" CssClass="NFButton" />--%>
                            <input type="button" id="btnRefresh" style="float: right; margin: 0 1px 0 1px" class="refresh" onclick="HideAndDisplay()" />
                            <%--<asp:Button ID="btnLessonSubmit" runat="server" class="showgraph" Style="float: right; margin: 0 1px 0 1px" Text="" CssClass="showgraph" OnClick="btnLessonSubmit_Click" />--%>
                            <asp:Button ID="btnExport" runat="server" Style="float: right; margin: 0 1px 0 1px" ToolTip="Export To PDF" CssClass="pdfPrint" OnClick="btnExport_Click" />
                            <%--<asp:Button ID="Button5" runat="server" Style="float: right; margin: 0 1px 0 1px" CssClass="print" ToolTip="Print" OnClick="btnPrint_Click" Visible="false" />--%>
                            <%-- <input id="Button6" type="button" name="Print" style="float:right;margin:0 1px 0 1px" class="pdfPrint" onclick="javascript: PrintDivData('AcademicGraphReports');" />--%>



                            <%-- <input type="image" style="border-width: 0px; float: right" onclick="HideAndDisplay()" src="../Administration/images/RefreshStudentBinder.png" value="Refresh" id="btnRefresh" name="btnRefresh" />--%>
                        </td>
                        <td style="width: 10%"></td>
                    </tr>
                </table>
            </div>
            <asp:UpdatePanel ID="reportpanel" runat="server">
                                    <ContentTemplate>
            <div style="text-align: center; width: 100%;">
                <table style="width: 100%">
                    
                    <tr>
                        <td style="text-align: center">
                            <div id="prdiv" style ="overflow:visible ">
                                <rsweb:ReportViewer ID="RV_Behavior" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="false" ShowCredentialPrompts="false" ShowDocumentMapButton="true" ShowExportControls="false" ShowFindControls="false" ShowPageNavigationControls="true" ShowParameterPrompts="true" ShowPrintButton="false" ShowPromptAreaButton="true" ShowRefreshButton="false" ShowToolBar="true" ShowWaitControlCancelLink="true" ShowZoomControl="false" SizeToReportContent="true" Width="100%" Height="100" ZoomMode ="FullPage" AsyncRendering="true">

                                    <ServerReport ReportServerUrl="<%$ appSettings:ReportUrl %>" />

                                </rsweb:ReportViewer>
                                <script lang ="javascript" type ="text/javascript">
                                      //  ResizeReport();
                                      //  function ResizeReport() {
                                      //    var viewer = document.getElementById("<%=RV_Behavior.ClientID%>");
                                      //    var htmlheight = document.documentElement.clientHeight;
                                      //   viewer.style.height = (htmlheight - 30) + "px";
                                      //  }
                                      // window.onresize = function resize() { ResizeReport(); }
                                      //</script>
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
        <div id="HighchartGraph" runat="server">
            <center>
                <br />
            <asp:label id="sname" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:label>
                <br />
            <asp:label id="behnam" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;" ></asp:label>
                <br />
            <asp:label id="daterang" runat="server" text="" Font-Bold="true" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:label>
                </center>
            <br />
         <%-- <asp:label id="treat" runat="server" text=""></asp:label>--%>
            <center>
           <asp:label runat="server" text="" id="mednodata"  Font-Bold="true" font-size="15px"></asp:label>
                <br />
           <div id = "medcont" runat="server"  style = "width: 935.43pt; margin: 0 auto"></div>
                 <br />
                <asp:Label ID="stgy" runat="server" Text="" Font-Bold="true" visibility="false" style="font-family: Arial; font-size: 12px; color: #000000;"></asp:Label>
                <br />
                <asp:Label ID="lbgraph" runat="server" Text="" Font-Bold="true" font-size="18px"></asp:Label>
                <span id="lbgraphSpan"></span>
            <div id = "cont" runat="server"  style = "width: 935.43pt; height:574.08pt; margin: 0 auto"></div>
            <br />
           <div id = "rategraph" runat="server"  style = "width: 935.43pt; margin: 0 auto"></div>
<%--                 <asp:label runat="server" text="" id="Label2"  Font-Bold="true" font-size="15px"></asp:label>--%>
                <br />
        </center>
            <div style = "margin-left:275px;width: 1280px;"><asp:label id="deftxt" runat="server" text="" style ="font-family: Arial; font-size: 12px;color:black;margin-left:150px;"  Font-Bold="true"></asp:label></div>
                
                <br />
       <asp:label id="mel" runat="server" text="" style ="font-family: Arial; font-size: 12px;color:black;margin-left:1300px;" Font-Bold="true"></asp:label>
                </div>
        
        </div>
                                         </ContentTemplate>
                                </asp:UpdatePanel> 
        <asp:HiddenField ID="hdnType" runat="server" Value="" />
        <asp:hiddenfield id="hdnExport" runat="server" value="" />

        <iframe id="iFramePdf" src="" style="display: none"></iframe>
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

            function loadchart(sdate, edate, sid, behid, scid, events, trend, ioa, clstype, med, gategraph, medno, reptype, inctype, stname) {
                 stname = stname.replace(/\*\*/g, "'");
                //medication report size start
                var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
                hchart.style.display = 'block';
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
                // set rategraph size start
                    var ratediv = document.getElementById('rategraph');
                    if (gategraph == "1") {
                        if (reptype == "true") {
                        ratediv.style.width = '676.47628pt';
                        ratediv.style.height = '368.5pt';
                    }
                    else {
                        ratediv.style.width = '935.433pt';
                        ratediv.style.height = '688.901215752pt';
                    }
                }
                // set rategraph size End
                var sdate = sdate;
                var edate = edate;
                var sid = sid;
                var bid = behid;
                var scid = scid;
                var evnt = events;
                var trend = trend;
                var cls = clstype;
                var xmin;
                var xmax;
                var restemp;
                var jsonData = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, Behav: bid, SchoolId: scid, Events: evnt, Trendtype: trend, Clstype: cls });
                $.ajax({
                    type: "POST",
                    url: "BiweeklyBehaviorGraph.aspx/getClinicalcReport",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: function (response) {
                        var res = JSON.parse(response.d);
                        restemp = res;
                        if (res.length == 0) {
                            // Case: No data
                                var hchart = document.getElementById('<%= cont.ClientID %>');
                                if(hchart){
                                    hchart.style.display = 'none';
                                }
                                var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                                if(hchart2){
                                    hchart2.style.display = 'none';
                                }
                                var hchart3 = document.getElementById('<%= rategraph.ClientID %>');
                                if(hchart3){
                                    hchart3.style.display = 'none';
                                }
                                var hchart4 = document.getElementById('<%= deftxt.ClientID %>');
                                if(hchart4){
                                    hchart4.style.display = 'none';
                                }
                                var hchart5 = document.getElementById('<%= mel.ClientID %>');
                                if(hchart5)
                                {
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
                                $('#stgy').css("display", "block");
                            $('#cont').css("display", "none");
                                $('#behnam').css("display", "block");
                                $('#daterang').css("display", "block");
                                $('#mel').css("display", "block");
                                $('#deftxt').css("display", "block");

                                var inputsDates =sdate;
                            inputsDates = inputsDates.replace(/-/g, '/');
                            var inputeDates = edate;
                            inputeDates = inputeDates.replace(/-/g, '/');


                            $.ajax({
                                type: "POST",
                                url: "BiweeklyBehaviorGraph.aspx/Getbehaviourname",
                                data: JSON.stringify({ bid: bid }),
                                contentType: "application/json; charset=utf-8",
                                async: false,
                                dataType: "json",
                                success: function (resp) {
                                    var resu = JSON.parse(resp.d);
                                    $.map(resu, function (items, index) {
                                        var stgy = document.getElementById('stgy');
                                        stgy.style.fontSize = '12px';
                                        if (items['BehavDefinition'] != null) {
                                            document.getElementById('stgy').innerHTML = 'Stgy: ' + items['BehavStrategy'];
                                        } else {
                                            document.getElementById('stgy').innerHTML = 'Stgy: ';
                        }

                                        document.getElementById('behnam').innerHTML = items['Behaviour'];
                                        var bname = document.getElementById('behnam');
                                        bname.style.fontSize = '12px';

                                        var deftxt = document.getElementById('deftxt');
                                        deftxt.style.fontSize = '12px';
                                        if (items['BehavDefinition'] != null) {
                                            document.getElementById('deftxt').innerHTML = 'Definition:    ' + items['BehavDefinition'];

                                        }
                        else {
                                            document.getElementById('deftxt').innerHTML = 'Definition:    ';
                                        }
                                        
                                        if (scid == 1) {
                                            document.getElementById('mel').innerHTML = 'Melmark New England';
                                            document.getElementById('mel').style.fontSize = '12px';
                                        }
                                        else {
                                            document.getElementById('mel').innerHTML = 'Melmark Pennsylvania';
                                            document.getElementById('mel').style.fontSize = '12px';
                                        }
                                    });

                                },
                                error: function (xhr, status, error) {

                                    console.error("AJAX request failed:", error);
                                }
                            });
                            document.getElementById('sname').innerHTML = stname;
                            var studname = document.getElementById('sname');
                            studname.style.fontSize = '12px';
                            document.getElementById('daterang').innerHTML = convertDateFormat(inputsDates) + "-" + convertDateFormat(inputeDates);
                            var drange = document.getElementById('daterang');
                            drange.style.fontSize = '12px';
                        }
                        else {
                            $('#stgy').css("display", "none");
                                var hchart = document.getElementById('<%= cont.ClientID %>');
                                if(hchart){
                                    hchart.style.display = 'block';
                                }
                                var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                                if(hchart2){
                                    hchart2.style.display = 'block';
                                }
                                var hchart3 = document.getElementById('<%= rategraph.ClientID %>');
                                if(hchart3){
                                    hchart3.style.display = 'block';
                                }
                                var hchart4 = document.getElementById('<%= deftxt.ClientID %>');
                                if(hchart4){
                                    hchart4.style.display = 'block';
                                }
                                var hchart5 = document.getElementById('<%= mel.ClientID %>');
                                if(hchart5)
                                {
                                    hchart5.style.display = 'block';
                                }
                                var hchart6 = document.getElementById('<%= mednodata.ClientID %>');
                                if (hchart6) {
                                    hchart6.style.display = 'block';
                                }
                            // start other labels
                            var inputsDate = sdate;
                            inputsDate = inputsDate.replace(/-/g, '/');
                            var inputeDate = edate;
                            inputeDate = inputeDate.replace(/-/g, '/');
                            $.map(res, function (item, index) {
                                document.getElementById('sname').innerHTML = stname;
                                document.getElementById('behnam').innerHTML = item['behaviour'];
                                document.getElementById('daterang').innerHTML = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                                if (scid == 1) {
                                document.getElementById('mel').innerHTML = 'Melmark New England';
                                }
                                else {
                                    document.getElementById('mel').innerHTML = 'Melmark Pennsylvania';
                                }
                                if (item['Deftn'] != null) {
                                    document.getElementById('deftxt').innerHTML = 'Definition:    ' + item['Deftn'];
                                }
                                else {
                                    document.getElementById('deftxt').innerHTML = 'Definition:    ';
                                }
                            });
                            // end other labels
                            // max of left and right y start
                            var lmax = -Infinity; var rmax = -Infinity;
                            for (var i = 0; i < res.length; i++) {
                                var freq = res[i].frequency;
                                var dur = res[i].duration;
                                if (freq > lmax) {
                                    lmax = freq;
                                }
                                if (dur > rmax) {
                                    rmax = dur;
                                }
                            }
                           
                            // max of left and right y end
                           
                            //left y interval start
                            var Linterval = 1;
                            if (lmax == 0) {
                                Linterval = 1;
                            } else if (lmax == 1) {
                                Linterval = 1;
                            } else if (lmax >= 1000) {
                                Linterval = 20;
                            } else if (lmax >= 50) {
                                Linterval = 10;
                            } else if (lmax >= 20) {
                                Linterval = 5;
                            } else {
                                Linterval = 1;
                            }
                            //left y interval end
                            //right y interval start
                            var Rinterval = 1;
                            if (rmax == 0) {
                                Rinterval = 1;
                            } else if (rmax == 1) {
                                Rinterval = 1;
                            } else if (rmax < 1) {
                                Rinterval = 0.1;
                            } else if (rmax >= 50) {
                                Rinterval = 10;
                            } else if (rmax >= 20) {
                                Rinterval = 5;
                            } else {
                                Rinterval = 1;
                            }
                            //right y interval end
                            var leftY = '';
                            var rightY = ''; var yvisible = true;
                            var leftmax = -Infinity;
                            var rightmax = -Infinity;
                            //left y axis name and max value
                            var frqstat = res[0].frqstat;
                            if (frqstat == 1) {
                                leftY = "Frequency";
                                if(lmax==0)
                                {
                                    leftmax=1;
                                }
                                else{
                                    leftmax = lmax;
                                }
                            } else if (frqstat == 2) {
                                leftY = "%Interval";
                                leftmax = 100;
                            } else {
                                leftY = "Frequency";
                                if(lmax==0)
                                {
                                    leftmax=1;
                                }
                                else{
                                    leftmax = lmax;
                                }
                            }
							var subtt =  res[0].Stratgy;
                            //left y axis name and max end
                            //right y axis name and max value
                            var durstat = res[0].DuratnStat;
                            if (durstat == false)
                            {
                                yvisible = false;
                            }
                            else
                            {
                                yvisible = true;
                                rightY = 'Total Daily Duration (Minutes)';
                            }
                            if (rmax == 0)
                            {
                                rightmax = 1;
                            }
                            else if (rmax == 1)
                            {
                                rightmax = 1;
                            } else if (rmax < 1) {
                                rightmax = 0.1;
                            } else if (rmax >= 50) {
                                rightmax = 10;
                            } else if (rmax >= 20) {
                                rightmax = 5;
                            } else {
                                rightmax = 1;
                            }
                            // Plot Data Start
                            var changedData = [];var yAxis = []; var symbol=[]; var color=[];
                            res.forEach(function(item){
                                if (item.eventtype == null) {
                                    date = extractdate(item.aggredateddate);
                                    if (leftY != '') {
                                var catdata = {};
                                        catdata['name'] = item.behaviour + '-frequency';
                                        catdata['value'] = item.frequency;
                                        catdata['color'] = 'blue';
                                        catdata['symbol'] = { symbol: 'diamond', enabled: true, hover: { enabled: true } };
                                        catdata['IOAPerc'] = item.ioapercfrq;
                                        catdata['date'] = date;
                                        catdata['yAxis'] = 0;
                                changedData.push(catdata);
                                yAxis[catdata['name']] = catdata['yAxis'];
                                        symbol[catdata['name']] = catdata['symbol'];
                                        color[catdata['name']] = catdata['color'];
                            }
                                    if (yvisible == true) {
                                var catdata = {};
                                        catdata['name'] = item.behaviour + '-duration';
                                        catdata['value'] = item.duration;
                                        catdata['color'] = 'red';
                                        catdata['symbol'] = { symbol: 'square', enabled: true, hover: { enabled: true } };
                                        catdata['IOAPerc'] = item.ioapercdur;
                                        catdata['date'] = date;
                                        catdata['yAxis'] = 1;
                                changedData.push(catdata);
                                yAxis[catdata['name']] = catdata['yAxis'];
                                        symbol[catdata['name']] = catdata['symbol'];
                                        color[catdata['name']] = catdata['color'];
                            }
                        }
                    });
                            //  var newdata = [...new Set(changedData.map(item => item.name))];
                            var newdata = []; var names = [];
                            changedData.forEach(function(item) {
                                if (!names[item.name]) {
                                    names[item.name] = true;
                                    newdata.push(item.name);
                                }
                            });
                            if (inctype == "True" || inctype == "true") {
                                var typechart = 'scatter';
                            }
                            else {
                                var typechart = 'line';
                            }
                            var ser = newdata.map(function(name) {
                            return {
                                type:typechart,
                                name: name,
                                yAxis: yAxis[name],
                                color: color[name],
                                marker:symbol[name],
                                dataGrouping:{enabled: true},
                                data: changedData.filter(function(item) {
                                    return item.name === name;
                                }).map(function(item) {
                                    return {
                                       x: item.date,
                                       y: item.value,
                                       IOAPerc:item.IOAPerc,
                                    };
                                })
                                    
                            };
            });
            //    //set duplicate series (case:no series) start
                           var dupdata=[];
                if(ser.length==0)
                {
                    var samdata = {};
                    var sd=extractdateOther(sdate); var ed=extractdateOther(edate);
                    samdata['data']=[{x:sd,y:null},{x:ed,y:null}];
                    samdata['showInLegend']=false;
                    dupdata.push(samdata);  
                    ser = dupdata;
                    leftmax = 1; rightmax = 1;
                }
                //set duplicate series (case:no series) end

            
             //   trend start
                var trData = []; var yAxis1 = []; var symbol1 = [];
                var tenddupfreq = [];
                var tenddupdur = [];
                $.map(res, function (item, index) {
                    
                                if(trend=="Quarter")
                        {
                    //if(leftY!='')
                    //{
                        if (item['trendfrequency'] != null) {
                            var uniq = checkArrayval(tenddupfreq, item['aggredateddate'])
                            if (!uniq) {
                                tenddupfreq.push(item['aggredateddate'])
                        var trnddata = {};
                                var symbarr = {};
                        trnddata['date'] = extractdate(item['aggredateddate']);
                        trnddata['name'] = 'freuency trend';
                        trnddata['value'] = item['trendfrequency'];
                                symbarr['enabled'] = false;
                                trnddata['symbol'] = symbarr;
                        trnddata['yAxis1'] = 0;
                        trData.push(trnddata);
                        yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                symbol1[trnddata['name']] = trnddata['symbol'];
                            }
                        }
                  //  }  
                        if (item['trendduration'] != null) {
                            if (durstat == false) {
                                var uniq = checkArrayval(tenddupdur, item['aggredateddate'])
                                if (!uniq) {
                                    tenddupdur.push(item['aggredateddate'])
                    var trnddata = {};
                                    var symbarr = {};
                        trnddata['date'] = extractdate(item['aggredateddate']);
                        trnddata['name'] = 'duration trend';
                        trnddata['value'] = item['trendduration'];
                                    symbarr['enabled'] = false;
                                    trnddata['symbol'] = symbarr;
                        trnddata['yAxis1'] = 1;
                        trData.push(trnddata);
                        yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                    symbol1[trnddata['name']] = trnddata['symbol'];
                    }
                }
                        }
                }

                });
  
                            var newdata1=[]; var name1=[];
                            // var newdata1 = [...new Set(arrData.map(item => item.name))];
                            trData.forEach(function(item) {
                                if (!name1[item.name]) {
                                    name1[item.name] = true;
                                    newdata1.push(item.name);
                                }
                            });
                            var ser1 = newdata1.map(function(name) {
                    return {
                        name: "",
                        yAxis: yAxis1[name],
                        color: "Gray",
                        marker:symbol1[name],
                        dashStyle:"DashDotDot",
                        lineWidth: 2,
                        showInLegend:false,
                        data: trData.filter(function(item) {
                            return item.name === name;
                        }).map(function(item) {
                            return {
                               x: item.date,
                               y: item.value,
            
                            };
                        })
                    };
                            });
          //  alert(JSON.stringify(ser1));
                ser = ser.concat(ser1);
            ////trend  end
            ////Plot Data End
            //    //Event plot Start
                var plotdata = [];
                var uniqueItems = [];
                var maxYPosition = 300; 
                var currentYPosition = 0;
                $.map(res, function (item, index) {
                    var style;
                    if (item['eventtype'] != null && item['eventtype'] != 'Arrow notes') {
                        var isUnique = true;
                        for (var i = 0; i < uniqueItems.length; i++) {
                            var uniqueItem = uniqueItems[i];
                            if (
                                uniqueItem['eventname'] === item['eventname'] &&
                                uniqueItem['aggredateddate'] === item['aggredateddate'] &&
                                uniqueItem['eventtype'] === item['eventtype']
                            ) {
                                isUnique = false;
                                break;
                            }
                        }

                        if (isUnique) {
                            uniqueItems.push(item);
                            if (item['eventtype'] == "Major") {
                                style = 'solid';
                            }
                            else {
                                style = 'longdashdot';
                            }
                       
                            var plotdict = new Object;
                            plotdict['color'] = "black";
                            plotdict['dashStyle'] = style;
                            plotdict['value'] = extractdate(item['aggredateddate']);
                            plotdict['width'] = 1;
                            var str = item['eventname'];
                            var txtal; var xax, yax;
                            var valign;
                            if (item['eventtype'] == "Major") {
                                txtal = 'right';
                                xax = -2;
                                valign = 'middle'
                            }
                            else {
                                txtal = 'left';
                                xax = 9;
                                valign = 'middle';
                            }
                            plotdict['label'] = {
                                text: str,
                                //textAlign: txtal,
                                formatter: function () {
                                    var labelText = this.options.text;
                                    labelText = labelText.replace(/↑/g, '<span style="font-size: larger;">↑</span>')
                                                       .replace(/↓/g, '<span style="font-size: larger;">↓</span>');
                                    return labelText;
                                },
                                useHTML: true,
                                style: {
                                    fontWeight: 'normal',
                                    color: 'black',
                                    fontSize: '12px',
                                    fontFamily: 'Arial',
                                    textShadow: 'none',
                                },
                                // align: 'right',
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
                var ar=[]; 
                var plotarr = new Object;
                plotarr['type'] = "scatter";
                plotarr['color'] = "black";
                plotarr['showInLegend']=false;
                plotarr['marker'] =   {
                    symbol: 'square',
                    radius:4
                },
                $.map(res, function (item, index) {
                    if (item['eventtype'] == "Arrow notes") {
                        if (item['frequency'] != null) {
                            ar.push({ x: extractdate(item['aggredateddate']), y: item['frequency'], arrowval: item['eventname'] });
                    }
                        else
                            {
                            ar.push({ x: extractdate(item['aggredateddate']), y: item['duration'], arrowval: item['eventname'] });
                        }
                    }
                });
                var mergedarr = [];
                var mergedKeys = {};

                for (var i = 0; i < ar.length; i++) {
                    var key = ar[i].x + '-' + ar[i].y;
                    if (!mergedKeys.hasOwnProperty(key)) { // Check if the key exists in mergedKeys
                        mergedarr.push(ar[i]);
                        mergedKeys[key] = true;
                    } else {
                        for (var j = 0; j < mergedarr.length; j++) {
                            if (mergedarr[j].x === ar[i].x && mergedarr[j].y === ar[i].y) {
                                mergedarr[j].arrowval += ',' + ar[i].arrowval;
                                break;
                            }
                        }
                    }
                }
                plotarr['data'] = mergedarr;
                ser.push(plotarr);
            //    //arrow end
            ////Event plot End
                Highcharts.chart('cont', {
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
                        text: ''
                    },
                    subtitle: {
                        useHTML: true,
                        x:50,
                        align: 'left',
                        text: subtt,
                        align: 'left',
                        style: {
                            fontWeight: 'bold',
                            color: 'black',
                            fontSize: 12,
                            fontFamily: 'Arial'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    yAxis:
              [{ // Primary yAxis    
                  min: 0,
                  tickInterval:Linterval,
                  gridLineWidth: 0,
                  max: leftmax,
                  title: {
                      text: leftY,
                      useHTML: true,
                      style: {
                          fontWeight: 'bold',
                          color: 'black',
                          fontSize: '12px',
                          fontFamily: 'Arial'

                      }
                  },
                  labels: {
                      style: {
                          color: 'black',
                          fontSize: '12px',
                          fontWeight: 'normal',
                          fontFamily: 'Arial'

                      }
                  }

                      
              }, { // Secondary yAxis

                  visible: yvisible,
                  gridLineWidth: 0,
                  min: 0,
                  tickInterval: Rinterval,
                  max: rightmax,
                  title: {
                      text: rightY,
                      useHTML: true,
                      style: {
                          fontWeight: 'bold',
                          color: 'black',
                          fontSize: '12px',
                          fontFamily: 'Arial'

                      }
                  },
                  labels: {
                      style: {
                          color: 'black', 
                          fontSize: '12px',
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
                                fontWeight: 'normal',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'
                            }

                        },
                        min:extractdateOther(sdate.replace('/','-')),
                        max:extractdateOther(edate.replace('/','-')),
                        plotLines:plotdata,
                        tickInterval: 2 * 24 * 3600 * 1000,
                        labels: {
                            rotation: -90,
                            formatter: function () {
                                return Highcharts.dateFormat('%m/%d/%Y', this.value); // Format the date as desired
                            },
                            style: {
                                color: 'black',
                                fontSize: '12px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            },
                                
                        }
                    },
                    legend: {
                        layout: 'horizontal',
                        align: 'right',
                        verticalAlign: 'bottom',
                        labelFormatter: function () {
                            return '<span style="color: black; font-weight: normal;font-size: 12px; font-family:Arial">' + this.name + '</span>';
                        }
                    },
                    plotOptions: {
                        series: {
                            turboThreshold: 5000 ,
                            dataLabels: {
                                allowOverlap: true,
                                enabled: true,
                                rotation: -90,
                                align: 'top',
                                y: -3,
                                overflow: 'justify',
                                crop: false,
                                formatter: function () {
                                    var point = this.point;
                                    if (point.IOAPerc) {
                                        if (point.y > 0) {
                                            var words = point.IOAPerc.split(' ');
                                            var newpoint = '';
                                            for (var i = 0; i < words.length; i++) {
                                                if (words[i].endsWith('%') && i < words.length - 1) {
                                                    newpoint += '<span style="font-size: 12px; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span><br> ';
                                                } else {
                                                    newpoint += '<span style="font-size: 12px; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span> ';
                                    }
                                        }
                                            return '<div style="white-space: nowrap; font-weight: normal;font-size: 12px; font-family: Arial; color: black">' + newpoint + '</div>';
                                        }
                                        else {
                                            return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size: 12px;font-family:Arial;color:black;text-shadow: none;font-weight:normal;">' + point.IOAPerc + '</span>';
                                        }
                                    }
                                    else if (point.arrowval) {
                                        return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size: 12px; font-family:Arial;font-color:black;text-shadow: none;font-weight:normal;">' + point.arrowval;
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
                            return '<b>' + Highcharts.dateFormat('%m/%d/%Y', new Date(this.x)) + '</b>: ' + this.y; // Format the date and display the y-value
                        }
                    },
                    series:ser
                });
                        }
            //rate  graph data start
                        if (gategraph == "1" && restemp !='') {
                var ratearray=[];var ratedata = {}; var ratefinal=[];
                ratedata['name']=res[0].behaviour;
                ratedata['color']='blue';
                ratedata['marker']={symbol:'diamond',enabled:true};
                res.forEach(function (item) {
                    ratedict={};
                daterate=extractdate(item.aggredateddate);
                ratedict['x']=daterate;
                ratedict['y']=item.rate
                ratearray.push(ratedict);
            });
                ratedata['data']=ratearray;
                ratefinal.push(ratedata);
         //   rate graph data end
         //    rate duplicate data start
                var duprate=[];
                if(ratefinal.length==0)
                {
                    var samdata = {};
                    var sd=extractdateOther(sdate); var ed=extractdateOther(edate);
                    samdata['data']=[{x:sd,y:null},{x:ed,y:null}];
                    samdata['showInLegend']=false;
                    duprate.push(samdata);  
                    ratefinal=duprate;
                }
               // alert(ratefinal);
            // rate duplicate data start
                
                
                    Highcharts.chart('rategraph', {
                        chart: {
                            plotBorderWidth: 2,
                            plotBorderColor: 'black',
                            spacingTop: 20,
                            spacingRight: 20,
                            spacingBottom: 20,
                            spacingLeft: 20,
                        },
                        title: {
                            text: ''
                        },
                        subtitle:{
                            text: ' ',
                            x: 50,
                            align: 'left',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '12px',
                                fontFamily: 'Arial'
                            }
                        },
                        xAxis: {
                            title: {
                                text: 'Dates',
                                useHTML: true,
                                style: {
                                    fontWeight: 'bold',
                                    color: 'black',
                                    fontSize: '12px',
                                    fontFamily: 'Arial'

                                }
                            },
                            //visible: false,
                            type: 'datetime',
                            //linkedTo: 0,
                            min:extractdateOther(sdate.replace('/','-')),
                            max:extractdateOther(edate.replace('/','-')),
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
                           
                            labels: {
                                enabled: true,
                                  rotation: -90,
                                formatter: function () {
                                    return Highcharts.dateFormat('%m/%d/%Y', this.value);
                                },
                                style: {
                                    fontWeight: 'bold',
                                    color: 'black',
                                    fontSize: '12px',
                                    fontFamily: 'Arial'

                                }
                            }
                        },
                        credits:{
                            enabled:false
                        },
                        yAxis: {
                            offset: 60,
                            title: {
                                text: 'Rate (In Minutes)',
                                useHTML: true,
                                style: {
                                    fontWeight: 'bold',
                                    color: 'black',
                                    fontSize: '12px',
                                    fontFamily: 'Arial'
                                }
                            },
                            
                        },
                        tooltip: {
                            formatter: function () {
                                return  Highcharts.dateFormat('%m/%d/%Y', this.point.x) +'-'+ Highcharts.dateFormat('%m/%d/%Y', this.point.x2);
                                         
                            }
                        },
                        series: ratefinal,
                    });
                }
                        
                var medjson=JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid,SchoolId: scid});

                if ((med == "true" || med == "True") && (medno == "false" || medno == "False")) {
             
                 $.ajax({
                     type: "POST",
                     url: "BiweeklyBehaviorGraph.aspx/getmedClinicalReport",
                     data: medjson,
                     contentType: "application/json; charset=utf-8",
                     async: false,
                     dataType: "json",
                     success: function (response) {
                         var resp=JSON.parse(response.d);
                         var medicData=[]; var val=0;
                         resp.forEach(function (item) {
                             item.duration = extractdate(item.EndTime) - extractdate(item.EvntTs);
                         });
                         resp.sort(function (a, b) {
                             return b.duration - a.duration;
                         });
                         $.map(resp, function (item, index) {
                             var meddata = {}; 
                             meddata['name'] = item['EventName']+item['Comment'];
                             meddata['x'] = extractdate(item['EvntTs']);
                             meddata['x2'] = extractdate(item['EndTime']);
                             meddata['y'] =val ;
                             medicData.push(meddata);
                             val=val+1
                         });

                         Highcharts.chart('medcont', {
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
                                 text: ''
                             },
                             subtitle:{
                                 text: ' Medication Details',
                                 x: 50,
                                 align:'left',
                                 useHTML: true,
                                 style: {
                                     fontWeight: 'bold',
                                     color: 'black',
                                     fontSize: '12px',
                                     fontFamily: 'Arial'
                                 }
                             },
                             xAxis: {
                                  
                                 //visible: false,
                                 type: 'datetime',
                                 //linkedTo: 0,
                                 min:extractdateOther(sdate.replace('/','-')),
                                 max:extractdateOther(edate.replace('/','-')),
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
                             credits:{
                                 enabled:false
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
                                     return  Highcharts.dateFormat('%m/%d/%Y', this.point.x) +'-'+ Highcharts.dateFormat('%m/%d/%Y', this.point.x2);
                                         
                                 }
                             },
                             series: [{
                                 data: medicData,
                                 showInLegend: false,
                                 dataLabels: {
                                     enabled: true,
                                     formatter: function() {
                                         return this.point.name;
                                     }
                                 },
                                  
                             }]
                         });
                     },
                     error: OnErrorCall
                 });
                 if (restemp == '') {
                     var medchart = document.getElementById('<%= medcont.ClientID %>');
                     medchart.style.display = 'none';
                     var ratechart = document.getElementById('<%= rategraph.ClientID %>');
                     ratechart.style.display = 'none';
                 }
                 function OnErrorCall(response) {

                     alert("Something went wrong! Error: ");
                        
                 }
                 
             }
                        },
                        error: OnErrorCall_
                    });
                function OnErrorCall_(response) {

                    alert("something went wrong!");
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
                function checkArrayval(trendDate, currDate)
                {
                    var status = false;
                    if (trendDate.length == 0) {
                        status = false
                    }
                    else {
                        for (var i = 0; i < trendDate.length; i++) {
                            if (trendDate[i] === currDate) {
                                status = true;
                                break;
                            }
                        }
                    }
                    return status;
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
                var lessgroup = "<%=behid%>";
            var less = lessgroup.split(",").map(Number);
            var lessnum = less.length;
            var i = 0;
            function exportChart() {
                alert("Please Wait..it takes a few Seconds ");
                    processNextChart();
            }
            function processNextChart() {
                if (i < lessnum) {
                    generateHighchart(less[i], function () {
                        captureAndSend(less[i], function () {
                            i++;
                            processNextChart();
                        });
                    });
                }
                else {
                  
                    DownloadPopup();
                    closePopup();
                }

            }
            function generateHighchart(lessid, callback) {
                var hchart = document.getElementById('<%= HighchartGraph.ClientID %>');
                hchart.style.display = 'block';
                var span = document.getElementById('lbgraphSpan');
                span.innerHTML = "";
                $('#sname').css("display", "block");
                $('#cont').css("display", "block");
                $('#behnam').css("display", "block");
                $('#daterang').css("display", "block");
                $('#mel').css("display", "block");
                $('#deftxt').css("display", "block");
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
                // set rategraph size start
                if ("<%=rategraph%>" == "1") {
                    var ratediv = document.getElementById('rategraph');
                    if ("<%=reptype%>" == "true") {
                        ratediv.style.width = '676.47628pt';
                        ratediv.style.height = '368.5pt';
                    }
                    else {
                        ratediv.style.width = '935.433pt';
                        ratediv.style.height = '688.901215752pt';
                    }
                }

                // set rategraph size End
                var sdate = "<%=sdate%>";
                var edate = "<%=edate%>";
                var sid = "<%=sid%>";
                var bid = lessid;
                var scid = "<%=scid%>";
                var evnt = "<%=events%>";
                var trend = "<%=trend%>";
                var cls = "<%=clstype%>";
                var xmin;
                var xmax;
                var restemp;
                var jsonData = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, Behav: bid, SchoolId: scid, Events: evnt, Trendtype: trend, Clstype: cls });
                $.ajax({
                    type: "POST",
                    url: "BiweeklyBehaviorGraph.aspx/getClinicalcReport",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: function (response) {
                        var res = JSON.parse(response.d);
                        restemp = res;
                        if (res.length === 0) { // Case: No data
                            var hchart = document.getElementById('<%= cont.ClientID %>');
                            if (hchart) {
                                hchart.style.display = 'none';
                            }
                            var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                                if (hchart2) {
                                    hchart2.style.display = 'none';
                                }
                                var hchart3 = document.getElementById('<%= rategraph.ClientID %>');
                                if (hchart3) {
                                    hchart3.style.display = 'none';
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
                            $('#stgy').css("display", "block");
                            $('#cont').css("display", "none");
                            $('#behnam').css("display", "block");
                            $('#daterang').css("display", "block");
                            $('#mel').css("display", "block");
                            $('#deftxt').css("display", "block");

                            var inputsDates = "<%=sdate%>";
                            inputsDates = inputsDates.replace(/-/g, '/');
                            var inputeDates = "<%=edate%>";
                            inputeDates = inputeDates.replace(/-/g, '/');


                            $.ajax({
                                type: "POST",
                                url: "BiweeklyBehaviorGraph.aspx/Getbehaviourname",
                                data: JSON.stringify({bid: bid}),
                                contentType: "application/json; charset=utf-8",
                                async: false,
                                dataType: "json",
                                success: function (resp) {
                                    var resu = JSON.parse(resp.d);
                                    $.map(resu, function (items, index) {
                                        var stgy = document.getElementById('stgy');
                                        stgy.style.fontSize = '16px';
                                        if (items['BehavDefinition'] != null) {
                                            document.getElementById('stgy').innerHTML ='Stgy: '+ items['BehavStrategy'];
                                        } else {
                                            document.getElementById('stgy').innerHTML = 'Stgy: ';
                        }

                                        document.getElementById('behnam').innerHTML = items['Behaviour'];
                                        var bname = document.getElementById('behnam');
                                        bname.style.fontSize = '16px';

                                        var deftxt = document.getElementById('deftxt');
                                        deftxt.style.fontSize = '16px';
                                        if (items['BehavDefinition'] != null) {
                                            document.getElementById('deftxt').innerHTML = 'Definition:    ' + items['BehavDefinition'];

                                        }
                        else {
                                            document.getElementById('deftxt').innerHTML = 'Definition:    ';
                                        }
                                       
                                        if (scid == 1) {
                                            document.getElementById('mel').innerHTML = 'Melmark New England';
                                        }
                                        else {
                                            document.getElementById('mel').innerHTML = 'Melmark Pennsylvania';
                                        }
                                    });
                           
                                },
                                error: function (xhr, status, error) {
                                   
                                    console.error("AJAX request failed:", error);
                                }
                            });

                            document.getElementById('sname').innerHTML = "<%=stname%>";
                            var studname = document.getElementById('sname');
                            studname.style.fontSize = '16px';
                            document.getElementById('daterang').innerHTML = convertDateFormat(inputsDates) + "-" + convertDateFormat(inputeDates);
                            var drange = document.getElementById('daterang');
                            drange.style.fontSize = '16px';
                        }
                        else {
                            $('#stgy').css("display", "none");
                           
                                var hchart = document.getElementById('<%= cont.ClientID %>');
                            if (hchart) {
                                hchart.style.display = 'block';
                            }
                            var hchart2 = document.getElementById('<%= medcont.ClientID %>');
                                if (hchart2) {
                                    hchart2.style.display = 'block';
                                }
                                var hchart3 = document.getElementById('<%= rategraph.ClientID %>');
                                if (hchart3) {
                                    hchart3.style.display = 'block';
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
                            // start other labels
                            var inputsDate = "<%=sdate%>";
                            inputsDate = inputsDate.replace(/-/g, '/');
                            var inputeDate = "<%=edate%>";
                            inputeDate = inputeDate.replace(/-/g, '/');
                            $.map(res, function (item, index) {
                                document.getElementById('sname').innerHTML = "<%=stname%>";
                                var studname = document.getElementById('sname');
                                studname.style.fontSize = '16px';
                                document.getElementById('behnam').innerHTML = item['behaviour'];
                                var bname = document.getElementById('behnam');
                                bname.style.fontSize = '16px';
                                document.getElementById('daterang').innerHTML = convertDateFormat(inputsDate) + "-" + convertDateFormat(inputeDate);
                                if (scid == 1) {
                                    document.getElementById('mel').innerHTML = 'Melmark New England';
                                    document.getElementById('mel').style.fontSize = '12px';
                                }
                                else {
                                    document.getElementById('mel').innerHTML = 'Melmark Pennsylvania';
                                    document.getElementById('mel').style.fontSize = '12px';
                                }
                                var drange = document.getElementById('daterang');
                                drange.style.fontSize = '16px';
                                if (item['Deftn'] != null) {
                                    document.getElementById('deftxt').innerHTML = 'Definition:    ' + item['Deftn'];
                                }
                                else {
                                    document.getElementById('deftxt').innerHTML = 'Definition:    ';
                                }
                            });
                            // end other labels
                            // max of left and right y start
                            var lmax = -Infinity; var rmax = -Infinity;
                            for (var i = 0; i < res.length; i++) {
                                var freq = res[i].frequency;
                                var dur = res[i].duration;
                                if (freq > lmax) {
                                    lmax = freq;
                                }
                                if (dur > rmax) {
                                    rmax = dur;
                                }
                            }

                            // max of left and right y end

                            //left y interval start
                            var Linterval = 1;
                            if (lmax == 0) {
                                Linterval = 1;
                            } else if (lmax == 1) {
                                Linterval = 1;
                            } else if (lmax >= 1000) {
                                Linterval = 20;
                            } else if (lmax >= 50) {
                                Linterval = 10;
                            } else if (lmax >= 20) {
                                Linterval = 5;
                            } else {
                                Linterval = 1;
                            }
                            //left y interval end
                            //right y interval start
                            var Rinterval = 1;
                            if (rmax == 0) {
                                Rinterval = 1;
                            } else if (rmax == 1) {
                                Rinterval = 1;
                            } else if (rmax < 1) {
                                Rinterval = 0.1;
                            } else if (rmax >= 50) {
                                Rinterval = 10;
                            } else if (rmax >= 20) {
                                Rinterval = 5;
                            } else {
                                Rinterval = 1;
                            }
                            //right y interval end
                            var leftY = '';
                            var rightY = ''; var yvisible = true;
                            var leftmax = -Infinity;
                            var rightmax = -Infinity;
                            //left y axis name and max value
                            var frqstat = res[0].frqstat;
                            if (frqstat == 1) {
                                leftY = "Frequency";
                                if (lmax == 0) {
                                    leftmax = 1;
                                }
                                else {
                                    leftmax = lmax;
                                }
                            } else if (frqstat == 2) {
                                leftY = "%Interval";
                                leftmax = 100;
                            } else {
                                leftY = "Frequency";
                                if (lmax == 0) {
                                    leftmax = 1;
                                }
                                else {
                                    leftmax = lmax;
                                }
                            }
							var subtt = res[0].Stratgy;
                            //left y axis name and max end
                            //right y axis name and max value
                            var durstat = res[0].DuratnStat;
                            if (durstat == false) {
                                yvisible = false;
                            }
                            else {
                                yvisible = true;
                                rightY = 'Total Daily Duration (Minutes)';
                            }
                            if (rmax == 0) {
                                rightmax = 1;
                            }
                            else if (rmax == 1) {
                                rightmax = 1;
                            } else if (rmax < 1) {
                                rightmax = 0.1;
                            } else if (rmax >= 50) {
                                rightmax = 10;
                            } else if (rmax >= 20) {
                                rightmax = 5;
                            } else {
                                rightmax = 1;
                            }
                            // Plot Data Start
                            var changedData = []; var yAxis = []; var symbol = []; var color = [];
                            res.forEach(function (item) {
                                if (item.eventtype == null) {
                                    date = extractdate(item.aggredateddate);
                                    if (leftY != '') {
                                        var catdata = {};
                                        catdata['name'] = item.behaviour + '-frequency';
                                        catdata['value'] = item.frequency;
                                        catdata['color'] = 'blue';
                                        catdata['symbol'] = { symbol: 'diamond', enabled: true, hover: { enabled: true } };
                                        catdata['IOAPerc'] = item.ioapercfrq;
                                        catdata['date'] = date;
                                        catdata['yAxis'] = 0;
                                        changedData.push(catdata);
                                        yAxis[catdata['name']] = catdata['yAxis'];
                                        symbol[catdata['name']] = catdata['symbol'];
                                        color[catdata['name']] = catdata['color'];
                                    }
                                    if (yvisible == true) {
                                        var catdata = {};
                                        catdata['name'] = item.behaviour + '-duration';
                                        catdata['value'] = item.duration;
                                        catdata['color'] = 'red';
                                        catdata['symbol'] = { symbol: 'square', enabled: true, hover: { enabled: true } };
                                        catdata['IOAPerc'] = item.ioapercdur;
                                        catdata['date'] = date;
                                        catdata['yAxis'] = 1;
                                        changedData.push(catdata);
                                        yAxis[catdata['name']] = catdata['yAxis'];
                                        symbol[catdata['name']] = catdata['symbol'];
                                        color[catdata['name']] = catdata['color'];
                                    }
                                }
                            });
                            //  var newdata = [...new Set(changedData.map(item => item.name))];
                            var newdata = []; var names = [];
                            changedData.forEach(function (item) {
                                if (!names[item.name]) {
                                    names[item.name] = true;
                                    newdata.push(item.name);
                                }
                            });
                            if ("<%=inctype%>" == "True" || "<%=inctype%>" == "true") {
                                var typechart = 'scatter';
                            }
                            else {
                                var typechart = 'line';
                            }
                            var ser = newdata.map(function (name) {
                                return {
                                    type: typechart,
                                    name: name,
                                    yAxis: yAxis[name],
                                    color: color[name],
                                    marker: symbol[name],
                                    dataGrouping: { enabled: true },
                                    data: changedData.filter(function (item) {
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
                            //    //set duplicate series (case:no series) start
                            var dupdata = [];
                            if (ser.length == 0) {
                                var samdata = {};
                                var sd = extractdateOther(sdate); var ed = extractdateOther(edate);
                                samdata['data'] = [{ x: sd, y: null }, { x: ed, y: null }];
                                samdata['showInLegend'] = false;
                                dupdata.push(samdata);
                                ser = dupdata;
                                leftmax = 1; rightmax = 1;
                            }
                            //set duplicate series (case:no series) end


                            //   trend start
                            var trData = []; var yAxis1 = []; var symbol1 = [];
                            var tenddupfreq = [];
                            var tenddupdur = [];
                            $.map(res, function (item, index) {

                                if ("<%=trend%>" == "Quarter") {
                        //if(leftY!='')
                        //{
                        if (item['trendfrequency'] != null) {
                            var uniq = checkArrayval(tenddupfreq, item['aggredateddate'])
                            if (!uniq) {
                                tenddupfreq.push(item['aggredateddate'])
                                var trnddata = {};
                                var symbarr = {};
                                trnddata['date'] = extractdate(item['aggredateddate']);
                                trnddata['name'] = 'freuency trend';
                                trnddata['value'] = item['trendfrequency'];
                                symbarr['enabled'] = false;
                                trnddata['symbol'] = symbarr;
                                trnddata['yAxis1'] = 0;
                                trData.push(trnddata);
                                yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                symbol1[trnddata['name']] = trnddata['symbol'];
                            }
                        }
                        //  }  
                        if (item['trendduration'] != null) {
                            if (durstat == false) {
                                var uniq = checkArrayval(tenddupdur, item['aggredateddate'])
                                if (!uniq) {
                                    tenddupdur.push(item['aggredateddate'])
                                    var trnddata = {};
                                    var symbarr = {};
                                    trnddata['date'] = extractdate(item['aggredateddate']);
                                    trnddata['name'] = 'duration trend';
                                    trnddata['value'] = item['trendduration'];
                                    symbarr['enabled'] = false;
                                    trnddata['symbol'] = symbarr;
                                    trnddata['yAxis1'] = 1;
                                    trData.push(trnddata);
                                    yAxis1[trnddata['name']] = trnddata['yAxis1'];
                                    symbol1[trnddata['name']] = trnddata['symbol'];
                                }
                            }
                        }
                    }

                });

                var newdata1 = []; var name1 = [];
                            // var newdata1 = [...new Set(arrData.map(item => item.name))];
                trData.forEach(function (item) {
                    if (!name1[item.name]) {
                        name1[item.name] = true;
                        newdata1.push(item.name);
                    }
                });
                var ser1 = newdata1.map(function (name) {
                    return {
                        name: "",
                        yAxis: yAxis1[name],
                        color: "Gray",
                        marker: symbol1[name],
                        dashStyle: "DashDotDot",
                        lineWidth: 2,
                        showInLegend: false,
                        data: trData.filter(function (item) {
                            return item.name === name;
                        }).map(function (item) {
                            return {
                                x: item.date,
                                y: item.value,

                            };
                        })
                    };
                });
                            //  alert(JSON.stringify(ser1));
                ser = ser.concat(ser1);
                            ////trend  end
                            ////Plot Data End
                            //    //Event plot Start
                var plotdata = [];
                var uniqueItems = [];
                var maxYPosition = 300;
                var currentYPosition = 0;
                $.map(res, function (item, index) {
                    var style;
                    if (item['eventtype'] != null && item['eventtype'] != 'Arrow notes') {
                        var isUnique = true;
                        for (var i = 0; i < uniqueItems.length; i++) {
                            var uniqueItem = uniqueItems[i];
                            if (
                                uniqueItem['eventname'] === item['eventname'] &&
                                uniqueItem['aggredateddate'] === item['aggredateddate'] &&
                                uniqueItem['eventtype'] === item['eventtype']
                            ) {
                                isUnique = false;
                                break;
                            }
                        }

                        if (isUnique) {
                            uniqueItems.push(item);
                            if (item['eventtype'] == "Major") {
                                style = 'solid';
                            }
                            else {
                                style = 'longdashdot';
                            }

                            var plotdict = new Object;
                            plotdict['color'] = "black";
                            plotdict['dashStyle'] = style;
                            plotdict['value'] = extractdate(item['aggredateddate']);
                            plotdict['width'] = 1;
                            var str = item['eventname'];
                            var txtal; var xax, yax;
                            var valign;
                            if (item['eventtype'] == "Major") {
                                txtal = 'right';
                                xax = -2;
                                valign = 'middle'
                            }
                            else {
                                txtal = 'left';
                                xax = 9;
                                valign = 'middle';
                            }
                            plotdict['label'] = {
                                text: str,
                                //textAlign: txtal,
                                formatter: function () {
                                    var labelText = this.options.text;
                                    labelText = labelText.replace(/↑/g, '<span style="font-size: larger;">↑</span>')
                                                       .replace(/↓/g, '<span style="font-size: larger;">↓</span>');
                                    return labelText;
                                },
                                useHTML: true,
                                style: {
                                    fontWeight: 'normal',
                                    color: 'black',
                                    fontSize: '14px',
                                    fontFamily: 'Arial',
                                    textShadow: 'none',
                                },
                                // align: 'right',
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
                var ar = [];
                var plotarr = new Object;
                plotarr['type'] = "scatter";
                plotarr['color'] = "black";
                plotarr['showInLegend'] = false;
                plotarr['marker'] = {
                    symbol: 'square',
                    radius: 4
                },
                $.map(res, function (item, index) {
                    if (item['eventtype'] == "Arrow notes") {
                        if (item['frequency'] != null) {
                            ar.push({ x: extractdate(item['aggredateddate']), y: item['frequency'], arrowval: item['eventname'] });
                        }
                        else {
                            ar.push({ x: extractdate(item['aggredateddate']), y: item['duration'], arrowval: item['eventname'] });
                        }
                    }
                });
                var mergedarr = [];
                var mergedKeys = {};

                for (var i = 0; i < ar.length; i++) {
                    var key = ar[i].x + '-' + ar[i].y;
                    if (!mergedKeys.hasOwnProperty(key)) { // Check if the key exists in mergedKeys
                        mergedarr.push(ar[i]);
                        mergedKeys[key] = true;
                    } else {
                        for (var j = 0; j < mergedarr.length; j++) {
                            if (mergedarr[j].x === ar[i].x && mergedarr[j].y === ar[i].y) {
                                mergedarr[j].arrowval += ',' + ar[i].arrowval;
                                break;
                            }
                        }
                    }
                }
                plotarr['data'] = mergedarr;
                ser.push(plotarr);
                            //    //arrow end

                            ////Event plot End
                Highcharts.chart('cont', {
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
                        text: ''
                    },
                    subtitle: {
                        useHTML: true,
                        x: 50,
                        align: 'left',
                        text: subtt,
                        align: 'left',
                        style: {
                            fontWeight: 'bold',
                            color: 'black',
                            fontSize: 12,
                            fontFamily: 'Arial'
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    yAxis:
              [{ // Primary yAxis    
                  min: 0,
                  tickInterval: Linterval,
                  gridLineWidth: 0,
                  max: leftmax,
                  title: {
                      text: leftY,
                      useHTML: true,
                      style: {
                          fontWeight: 'bold',
                          color: 'black',
                          fontSize: '14px',
                          fontFamily: 'Arial'

                      }
                  },
                  labels: {
                      style: {
                          color: 'black',
                          fontSize: '14px',
                          fontWeight: 'normal',
                          fontFamily: 'Arial'

                      }
                  }


              }, { // Secondary yAxis

                  visible: yvisible,
                  gridLineWidth: 0,
                  min: 0,
                  tickInterval: Rinterval,
                  max: rightmax,
                  title: {
                      text: rightY,
                      useHTML: true,
                      style: {
                          fontWeight: 'bold',
                          color: 'black',
                          fontSize: '14px',
                          fontFamily: 'Arial'

                      }
                  },
                  labels: {
                      style: {
                          color: 'black',
                          fontSize: '14px',
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
                                fontWeight: 'normal',
                                color: 'black',
                                fontSize: '14px',
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
                                color: 'black',
                                fontSize: '14px',
                                fontWeight: 'bold',
                                fontFamily: 'Arial'

                            },

                        }
                    },
                    legend: {
                        layout: 'horizontal',
                        align: 'right',
                        verticalAlign: 'bottom',
                        labelFormatter: function () {
                            return '<span style="color: black; font-weight: normal;font-size: 14px; font-family:Arial">' + this.name + '</span>';
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
                                y: -3,
                                overflow: 'justify',
                                crop: false,
                                formatter: function () {
                                    var point = this.point;
                                    if (point.IOAPerc) {
                                        if (point.y > 0) {
                                            var words = point.IOAPerc.split(' ');
                                            var newpoint = '';
                                            for (var i = 0; i < words.length; i++) {
                                                if (words[i].endsWith('%') && i < words.length - 1) {
                                                    newpoint += '<span style="font-size: 14px; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span><br> ';
                                                } else {
                                                    newpoint += '<span style="font-size: 14px; font-family: Arial; color: black; font-weight:normal">' + words[i] + '</span> ';
                                                }
                                            }
                                            return '<div style="white-space: nowrap; font-weight: normal;font-size: 14px; font-family: Arial; color: black">' + newpoint + '</div>';
                                        }
                                        else {
                                            return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size: 14px;font-family:Arial;color:black;text-shadow: none;font-weight:normal;">' + point.IOAPerc + '</span>';
                                        }
                                    }
                                    else if (point.arrowval) {
                                        return '<span style="transform: rotate(-45deg); display: block; white-space: nowrap;font-size: 14px; font-family:Arial;font-color:black;text-shadow: none;font-weight:normal;">' + point.arrowval;
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
                            return '<b>' + Highcharts.dateFormat('%m/%d/%Y', new Date(this.x)) + '</b>: ' + this.y; // Format the date and display the y-value
                        }
                    },
                    series: ser
                });
                        }
                        if ("<%=gategraph%>" == "1" && restemp!='') {
                        //rate  graph data start
                        var ratearray = []; var ratedata = {}; var ratefinal = [];
                        ratedata['name'] = res[0].behaviour;
                        ratedata['color'] = 'blue';
                        ratedata['marker'] = { symbol: 'diamond', enabled: true };
                        res.forEach(function (item) {
                            ratedict = {};
                            daterate = extractdate(item.aggredateddate);
                            ratedict['x'] = daterate;
                            ratedict['y'] = item.rate
                            ratearray.push(ratedict);
                        });
                        ratedata['data'] = ratearray;
                        ratefinal.push(ratedata);
                        //   rate graph data end
                        //    rate duplicate data start
                        var duprate = [];
                        if (ratefinal.length == 0) {
                            var samdata = {};
                            var sd = extractdateOther(sdate); var ed = extractdateOther(edate);
                            samdata['data'] = [{ x: sd, y: null }, { x: ed, y: null }];
                            samdata['showInLegend'] = false;
                            duprate.push(samdata);
                            ratefinal = duprate;
                        }
                        // alert(ratefinal);
                        // rate duplicate data start
                    Highcharts.chart('rategraph', {
                        chart: {
                            plotBorderWidth: 2,
                            plotBorderColor: 'black',
                            spacingTop: 20,
                            spacingRight: 20,
                            spacingBottom: 20,
                            spacingLeft: 20,
                        },
                        title: {
                            text: ''
                        },
                        subtitle: {
                            text: ' ',
                            x: 50,
                            align: 'left',
                            useHTML: true,
                            style: {
                                fontWeight: 'bold',
                                color: 'black',
                                fontSize: '14px',
                                fontFamily: 'Arial'
                            }
                        },
                        xAxis: {
                            title: {
                                text: 'Dates',
                                useHTML: true,
                                style: {
                                    fontWeight: 'bold',
                                    color: 'black',
                                    fontSize: '14px',
                                    fontFamily: 'Arial'

                                }
                            },
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

                            labels: {
                                enabled: true,
                                rotation: -90,
                                formatter: function () {
                                    return Highcharts.dateFormat('%m/%d/%Y', this.value);
                                },
                                style: {
                                    fontWeight: 'bold',
                                    color: 'black',
                                    fontSize: '14px',
                                    fontFamily: 'Arial'

                                }
                            }
                        },
                        credits: {
                            enabled: false
                        },
                        yAxis: {
                            offset: 60,
                            title: {
                                text: 'Rate (In Minutes)',
                                useHTML: true,
                                style: {
                                    fontWeight: 'bold',
                                    color: 'black',
                                    fontSize: '14px',
                                    fontFamily: 'Arial'
                                }
                            },

                        },
                        tooltip: {
                            formatter: function () {
                                return Highcharts.dateFormat('%m/%d/%Y', this.point.x) + '-' + Highcharts.dateFormat('%m/%d/%Y', this.point.x2);

                            }
                        },
                        series: ratefinal,
                    });
                }
                var medjson = JSON.stringify({ StartDate: sdate, enddate: edate, studid: sid, SchoolId: scid });

                if (("<%=med%>" == "true" || "<%=med%>" == "True") && ("<%=medno%>" == "false" || "<%=medno%>" == "False")) {

                    $.ajax({
                        type: "POST",
                        url: "BiweeklyBehaviorGraph.aspx/getmedClinicalReport",
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

                            Highcharts.chart('medcont', {
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
                                    text: ''
                                },
                                subtitle: {
                                    text: ' Medication Details',
                                    x: 50,
                                    align: 'left',
                                    useHTML: true,
                                    style: {
                                        fontWeight: 'bold',
                                        color: 'black',
                                        fontSize: '14px',
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

                                }]
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
                if (restemp == '') {
                    var medchart = document.getElementById('<%= medcont.ClientID %>');
                    medchart.style.display = 'none';
                    var ratechart = document.getElementById('<%= rategraph.ClientID %>');
                     ratechart.style.display = 'none';
                 }
        function OnErrorCall_(response) {

            alert("something went wrong!");
        }
        setTimeout(callback, 1000);
            }
            function captureAndSend(lessid, callback) {
                html2canvas($("#HighchartGraph")[0]).then(function (canvas) {
                    base64 = canvas.toDataURL();
                    var dataToSend = {
                        base64: base64, chartId: lessid
                    };
                   
                    $.ajax({
                        type: "POST",
                        url: "BiweeklyBehaviorGraph.aspx/getgraphs",
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
                setTimeout(callback, 1000);
            }

        </script>
    </form>
</body>
</html>
