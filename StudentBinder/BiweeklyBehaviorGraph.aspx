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
             <div id="downloadPopup" class="web_dialog" style="width: 600px; ">

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

                                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" onclientclick="HideWait();" />

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

        <asp:HiddenField ID="hdnType" runat="server" Value="" />
        <asp:hiddenfield id="hdnExport" runat="server" value="" />

        <iframe id="iFramePdf" src="" style="display: none"></iframe>
    </form>
</body>
</html>
