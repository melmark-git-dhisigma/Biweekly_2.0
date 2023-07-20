<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardReportNew.aspx.cs" Inherits="Graph" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>


    <script src="js/jquery-2.1.0.js"></script>
    <script src="js/d3.v3.js"></script>
    <script src="js/nv.d3.js"></script>
    <link href="../Administration/CSS/GraphStyle.css" rel="stylesheet" />
    <link href="CSS/nv.d3.css" rel="stylesheet" />

    <style type="text/css">

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
     
    </style>


    <script type="text/javascript">

        window.onload = function () {


            var d = new Date();
            var strDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
            $('#lblDates').html(strDate);
            $('#lblDate').html(strDate);

        };
        function loadWait() {
            $('.loading').fadeIn('fast');//, function () { });
        }
        function HideWait() {
            $('.loading').fadeOut('fast');//, function () { });
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
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="180000"> </asp:ScriptManager>
        <div class="mainDivS">
            <div class="Head" style="float: none; font-size: 35px; text-align:center; font-family:Tahoma; color: #00549f;margin-top:-45px">
                <h3 style="font-family:Tahoma">Dashboard - Today's <!--(<label id="lblDate"></label>)--> Progress</h3>
            </div>
            <div style="width:100%">
                <div class="divdrop" style="margin-top:-30px">
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
                </div>
                <div class="divbtns" style="padding-top:60px;padding-right: 140px;margin-top:-50px">
                    <table>
                        <tr>
                            <td colspan="3" style="text-align:center;padding-left: 200px;width:15%">
                                 <span style="text-align:center;font-weight:bolder;font-size:15px;color: #00549f;margin-left:-150px">Choose Reports:</span>
                            </td>
                            <td colspan="3" style="text-align:left;width: 20%;">
                                 <asp:button id="BtnClientAcademic" runat="server" text="" cssclass="NFButton" tooltip="Academic by Client" OnClick="BtnClientAcademic_Click" BackColor="#00549F"  />
                                 <asp:button id="BtnStaffAcademic" runat="server" text="" cssclass="NFButton" tooltip="Academic by Staff" OnClick="BtnStaffAcademic_Click" BackColor="#00549F"   />
                                 <asp:button id="BtnClientClinical" runat="server" text="" cssclass="NFButton" tooltip="Clinical by Client" OnClick="BtnClientClinical_Click" BackColor="#00549F"   />
                                 <asp:Button id="BtnStaffClinical" runat="server" text="" cssclass="NFButton" style="width:102px" tooltip="Clinical by Staff" OnClick="BtnStaffClinical_Click" BackColor="#00549F"  />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkbx_Mistrial" runat="server" style=" color: #00549f;font-size:12px" Checked="True" OnCheckedChanged="chkbx_Mistrial_CheckedChanged"/>
                                <span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom;">Count Mistrial</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="divchkbox" style="padding-top:20px;margin-top:-30px">
                    <table>
                        <tr>
                            <td>
                                 <asp:button id="BtnRefresh" runat="server" text="Refresh" cssclass="NFButton" style="background-color:#34b233" tooltip="Refresh the Dashboard" OnClick="BtnRefresh_Click"      />
                            </td>
                            <td style="text-align:center;width:70%">
                                <div id="chkbox-selection" style="padding-top:10px">
                                <span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom;">Show progress as: </span>
                                <asp:CheckBox ID="chkbx_leson_deliverd" runat="server" Text="total lessons delivered today or" style=" color: #00549f;font-size:12px" AutoPostBack="True" Checked="True" Enabled="False" OnCheckedChanged="chkbx_leson_deliverd_CheckedChanged"/>
                                <asp:CheckBox ID="chkbx_block_sch" runat="server" Text="percentage of block-scheduled lessons" style=" color: #00549f;font-size:12px" AutoPostBack="True" OnCheckedChanged="chkbx_block_sch_CheckedChanged"/>
                                </div>
                                <div id="selection-label" style="margin-top:0px;max-width:1100px;word-wrap: break-word;" >
                                <p><span style="text-align:right; font-size:12px;color: #00549f;vertical-align: text-bottom">
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
                                
                                </td><td id="tdMsg" runat="server" style="width: 97%;font-size: 15px;color:green;text-align:center;font-weight:bold"></td>
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
                <!--olddashboard-view--end-->
            </div>
            <hr style="color:#1f497d;font-size:smaller;margin-top:-20px"/>
          
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
        <!--olddashboard-teacher--end-->
    </form>
</body>
</html>
