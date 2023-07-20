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
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />



    <style type="text/css">
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


    </script>

    <script type="text/javascript">


        function HideAndDisplay() {

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







                <div id="graphPopup" class="web_dialog" >
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

                                    <asp:CheckBox ID="chkbxevents" runat="server" Text="Display Events"></asp:CheckBox>
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
                                    <asp:RadioButtonList ID="rbtnClassTypeall" runat="server" RepeatDirection="Horizontal" Visible="false">
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

                <div id="downloadPopup" class="web_dialog" style="width: 600px;">

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

                                    <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" OnClientClick="HideWait();" />

                                </td>
                                <td style="text-align: left">
                                    <input type="button" value="Done" class="NFButton" id="btnDone" onclick="CloseDownload();" />
                                    <%--  <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton"  onclientclick="CloseDownload();" />--%>

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
                            <asp:Button ID="btnPrevious" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0px 250px" Visible="false" Text="Previous" OnClick="btnPrevious_Click" />
                             <asp:DropDownList ID="ddlLessonplan" runat="server" CssClass="drpClass" Style="float: left; margin: 0 1px 0px 10px" Height="26px" Width="290px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlLessonplan_SelectedIndexChanged">
                             </asp:DropDownList>
                            <asp:Button ID="btnNext" runat="server" CssClass="NFButton" Style="float: left; margin: 0 1px 0 10px" Visible="false" Text="Next" OnClick="btnNext_Click" />

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
            <div style="text-align: center; width: 100%;">                
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


        <asp:HiddenField ID="hdnType" runat="server" Value="" />
        <asp:HiddenField ID="hdnExport" runat="server" Value="" />
    </form>

</body>

</html>
