<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP9.aspx.cs" Inherits="StudentBinder_CreateIEP9" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />
    <script type="text/javascript" src="../Administration/JS/jquery-1.8.0.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.datepicker.js"></script>
    <link href="../Administration/CSS/demos.css" type="text/css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />

    <link href="../Administration/CSS/jquery.ui.base.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/jquery.ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="../Administration/JS/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.fileupload.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.iframe-transport.js" type="text/javascript"></script>

    <script src="../Administration/JS/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />

    <title></title>
    <style type="text/css">
        .style1 {
            width: 903px;
        }

        .style2 {
            width: 20%;
        }

        .auto-style2 {
            height: 35px;
            width: 181px;
        }

        .auto-style3 {
            font-family: Arial;
            color: #000;
            line-height: 22px;
            font-weight: normal;
            font-size: 12px;
            padding-right: 1px;
            text-align: left;
            height: 35px;
            width: 181px;
        }

        .auto-style4 {
            width: 181px;
        }

        .FreeTextDivContent {
            width: 98%;
            min-height: 200px;
            height: 200px;
            padding: 2%;
            border: 1px solid rgb(228, 228, 228);
            border-radius: 8px 8px 8px 8px;
            overflow:auto;
        }

        .auto-style5 {
            height: 37px;
        }

        .auto-style6 {
            height: 35px;
        }

        .chk-style {
            display: block;
            float: left;
        }

        .tdText {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            line-height: 25px;
            height: 10px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("[id$=txtDateOne]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../StudentBinder/img/Calendar24.png'
            });
        });

        $(function () {
            $("[id$=txtDateTwo]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../StudentBinder/img/Calendar24.png'
            });
        });

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

        <div id="divIep9" style="height: auto; border-radius: 5px; width: 100%;">
            <table style="width: 100%;" cellpadding="0" cellspacing="0">

                <tr>
                    <td colspan="4" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px;" class="auto-style6">Placement Consent Form - PL1: 3-5 year olds
                    </td>

                </tr>
                <tr>
                    <td runat="server" id="tdMsg" style="color: #FF0000" colspan="1"></td>
                </tr>

                <tr>
                    <td colspan="4" class="auto-style5">Use either section 1, 2 or 3 as appropriate to the child’s educational placement.
                    </td>

                </tr>
                <tr>
                    <td class="tdText" colspan="3" style="background-color: #E3E3E3; text-align: center"><strong>1.The child attends an inclusive early childhood program that includes children with and without disabilities.</strong></td>
                </tr>
                <tr class="RowStyle">
                    <td style="width: 60%;">The child attends an early childhood program and special education services are provided:</td>
                    <td style="width: 40%;">
                        <br />
                        <asp:CheckBox ID="chkEarlyChild" runat="server" Text=" " CssClass="" />In the early childhood program<br />
                        <asp:CheckBox ID="chkSeparate" runat="server" Text=" " />Separate from the early childhood program<br />
                        <asp:CheckBox ID="chkBoth" runat="server" Text=" " />Both in and out of the early childhood program<br />
                        <br />
                    </td>
                </tr>
                <tr class="AltRowStyle">
                    <td>Hours per week in the early childhood program
                       <asp:TextBox ID="txtStateOrDistrict" runat="server" Width="30%"></asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <asp:CheckBox ID="chkEnrolled" runat="server" Text=" " />Enrolled by the parent<br />
                        <asp:CheckBox ID="chkPlaced" runat="server" Text=" " />Placed by the Team<br />
                        <br />
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>All together the child will be participating in an inclusive environment (taking into account the early childhood program and special education services):</td>
                    <td>
                        <br />
                        <asp:CheckBox ID="chkTimeMore" runat="server" Text=" " />80% of the time or more<br />
                        <asp:CheckBox ID="chkOfTime" runat="server" Text=" " />40 – 79% of the time<br />
                        <asp:CheckBox ID="chk39Time" runat="server" Text=" " />0 – 39% of the time<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td class="tdText" colspan="3" style="background-color: #E3E3E3; text-align: center"><strong>2. The child does not attend an inclusive early childhood program.</strong></td>
                </tr>
                <tr class="RowStyle">
                    <td>
                        <br />
                        The Team identified that the child should attend a special education class that only serves children with disabilities.<br />
                        <br />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkSubstancialy" runat="server" Text=" " />Substantially Separate Class<br />
                        <br />
                    </td>
                </tr>
                <tr class="AltRowStyle">
                    <td>
                        <br />
                        The Team identified that the child should attend a full-day special education program in a public or private separate day school that only serves children with disabilities.<br />
                        <br />
                    </td>
                    <td>
                        <br />
                        <asp:CheckBox ID="chkDaySchool" runat="server" Text=" " />Separate Day School<br />
                        <span style="margin-left: 25px;">
                            <asp:CheckBox ID="chkPublic" runat="server" Text=" " />Public or</span>
                        <asp:CheckBox ID="chkPrivate" runat="server" Text=" " />Private<br />
                        <br />
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>
                        <br />
                        The Team identified that the child should attend a special education program in a residential facility that only serves children with disabilities.<br />
                        <br />
                    </td>
                    <td>
                        <br />
                        <asp:CheckBox ID="chkResidential" runat="server" Text=" " />Residential Facility<br />
                        <br />
                    </td>
                </tr>
                <tr class="AltRowStyle">
                    <td>
                        <br />
                        The Team identified IEP services to be provided in a program in the home for a child who is 3 to 5 years of age.<br />
                        <br />
                    </td>
                    <td>
                        <br />
                        <asp:CheckBox ID="chkHome" runat="server" Text=" " />Home<br />
                        <br />
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>
                        <br />
                        The Team identified IEP services to be provided outside the home in a clinicians office, school office, hospital facility, or other community location.<br />
                        <br />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkService" runat="server" Text=" " />Service Provider Location<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td class="tdText" colspan="3" style="background-color: #E3E3E3; text-align: center"><strong>3. Other Authority Required Placements</strong>
                        <br />
                        <span style="font-size: 12px;">Note: These non-educational placements are not determined by the Team and therefore service delivery may be limited.</span>
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>The placement has been made by a state agency to an institutionalized setting for non-educational reasons.</td>
                    <td>
                        <br />
                        <asp:CheckBox ID="chkDepartment" runat="server" Text=" " CssClass="chk-style" />The Department of Mental Health has placed the child in a hospital psychiatric unit or residential treatment program.<br />
                        <br />
                        <asp:CheckBox ID="chkMassachusetts" runat="server" Text=" " CssClass="chk-style" />The Department of Public Health has placed the child in the Massachusetts Hospital School.<br />
                        <span style="margin-left: 25px;">
                            <asp:CheckBox ID="chkDay" runat="server" Text=" " />Day or</span>
                        <asp:CheckBox ID="chkRes" runat="server" Text=" " />Residential<br />
                        <br />
                    </td>
                </tr>
                <tr class="AltRowStyle">
                    <td>
                        <br />
                        A doctor has determined that the child must be served in a home setting.<br />
                        <br />
                    </td>
                    <td>
                        <br />
                        <asp:CheckBox ID="chkHmeBasedPgm" runat="server" Text=" " />Home-based Program<br />
                        <br />
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>
                        <br />
                        A doctor has determined that the child must be served in a hospital setting.<br />
                        <br />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkHospital" runat="server" Text=" " />Hospital-based Program<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td colspan="4" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px;" class="auto-style6">Placement Consent Form</td>
                </tr>
                <tr>
                    <td>Location(s) for Service Provision and Dates:<asp:TextBox ID="txtLocationService" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdText" colspan="3" style="background-color: #E3E3E3; text-align: center"><strong>Parent Options / Responses</strong></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                        It is important that the district knows your decision as soon as possible. Please indicate your response by checking at least one (1) box and returning a signed copy to the district along with your response to the IEP. Thank you.
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkConcent" runat="server" Text=" " />
                        I consent to the placement.</td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkPlacement" runat="server" Text=" " />
                        I refuse the placement.</td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkRefused" runat="server" Text=" " />I request a meeting to discuss the refused placement.
                        
                    </td>
                    
                    
                </tr>
                <tr><td colspan="3"><hr /></td></tr>
                <tr>
                    <td>Signature of Parent, Guardian, Educational Surrogate Parent  Student 18 and Over *Required signature once a student reaches 18 unless there is a court appointed guardian.
                        <asp:TextBox ID="txtSignParentOne" runat="server" Width="30%"></asp:TextBox>
                    </td>
                    <td>Date
                        <asp:TextBox ID="txtDateOne" runat="server" Width="30%" CssClass="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td colspan="4" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px;" class="auto-style6">Placement Consent Form – PL1: 6-21 year olds</td>
                </tr>
                <tr>
                    <td class="tdText" colspan="3" style="background-color: #E3E3E3; text-align: center;"><strong>Team Recommended Educational Placements</strong>
                        <span style="text-align: center; margin-left: 200px;"><strong>Corresponding Placement</strong></span>
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>
                        <br />
                        The Team identified that IEP services are provided outside the general education classroom less than 21% of the time (80% inclusion).<br />
                        <br />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkFullPgm" runat="server" Text=" " />Full Inclusion Program<br />
                        <br />
                    </td>
                </tr>
                <tr class="AltRowStyle">
                    <td>
                        <br />
                        The Team identified that IEP services are provided outside the general education classroom at least 21% of the time, but no more than 60% of the time.<br />
                        <br />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkPartialPgm" runat="server" Text=" " />Partial Inclusion Program<br />
                        <br />
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>
                        <br />
                        The Team identified that IEP services are provided outside the general education classroom for more than 60% of the time.<br />
                        <br />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkSepClass" runat="server" Text=" " />Substantially Separate Classroom<br />
                        <br />
                    </td>
                </tr>
                <tr class="AltRowStyle">
                    <td>
                        <br />
                        The Team identified that all IEP services should be provided outside the general education classroom and in a public or private separate school that only serves students with disabilities.<br />
                        <br />
                    </td>
                    <td>
                        <br /><asp:CheckBox ID="chkSepDaySchool" runat="server" Text=" " />Separate Day School
                        <br />
                        <span style="margin-left: 15px;">
                            <asp:CheckBox ID="chkPublicSep" runat="server" Text=" " />Public or</span>
                        <asp:CheckBox ID="chkPrivateSep" runat="server" Text=" " />Private<br />
                        <br />

                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>
                        <br />
                        The Team identified that IEP services require a 24-hour special education program.<br />
                        <br />
                    </td>
                    <td>
                        <br /><asp:CheckBox ID="chkResSchool" runat="server" Text=" " />Residential School<br />
                        <br />
                    </td>
                </tr>
                <tr class="AltRowStyle">
                    <td>
                        <br />
                        The Team has identified a mix of IEP services that are not provided in primarily school-based settings but are in a neutral or community-based setting.<br />
                        <br />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkOtherSep" runat="server" Text=" " />Other<asp:TextBox ID="txtOther" runat="server" Width="30%"></asp:TextBox><br />
                        
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="tdText" colspan="3" style="background-color: #E3E3E3; text-align: center"><strong>Other Authority Required Placements</strong>
                        <br />
                        <span style="font-size: 12px;">Note: These non-educational placements are not determined by the Team and therefore service delivery may be limited.</span>
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>The placement has been made by a state agency to an institutionalized setting for non-educational reasons.</td>
                    <td>
                        <br /><asp:CheckBox ID="chkDetained" runat="server" Text=" " CssClass="chk-style" />The Department of Youth Services has placed the student in a facility for committed or detained youth.<br />
                        <br />
                        <asp:CheckBox ID="chkTreatment" runat="server" Text=" " CssClass="chk-style" />The Department of Mental Health has placed the student in a hospital psychiatric unit or residential treatment program.<br />
                        <br />
                        <asp:CheckBox ID="chkHospitalSchool" runat="server" Text=" " CssClass="chk-style" />The Department of Public Health has placed the student in the Massachusetts Hospital School<br />
                        <span style="margin-left: 15px;">
                            <asp:CheckBox ID="chkDayPlace" runat="server" Text=" " />Day or</span>
                        <asp:CheckBox ID="chkResPlace" runat="server" Text=" " />Residential<br />
                        <br />
                        <asp:CheckBox ID="chkFacility" runat="server" Text=" " CssClass="chk-style" />The student is incarcerated in the county house of corrections or in a department of corrections facility.<br />
                        <br />
                    </td>
                </tr>
                <tr class="AltRowStyle">
                    <td>
                        <br />
                        A doctor has determined that the student must be served in a home setting.<br />
                        <br />
                    </td>
                    <td>
                        <br /><asp:CheckBox ID="chkHomeDoctor" runat="server" Text=" " />Home-based Program<br />
                        <br />
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>
                        <br />
                        A doctor has determined that the student must be served in a hospital setting.<br />
                        <br />
                    </td>
                    <td>
                        <br /><asp:CheckBox ID="chkHospitalDoctor" runat="server" Text=" " />Hospital-based Program<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px;" class="auto-style6">Placement Consent Form</td>
                </tr>
                <tr>
                    <td>Location(s) for Service Provision and Dates:<asp:TextBox ID="txtLocationService2" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdText" colspan="3" style="background-color: #E3E3E3; text-align: center"><strong>Parent Options / Responses</strong></td>
                </tr>

                <tr>
                    <td colspan="3">
                        <br />
                        It is important that the district knows your decision as soon as possible. Please indicate your response by checking at least one (1) box and returning a signed copy to the district along with your response to the IEP. Thank you.
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkConsent2" runat="server" Text=" " />
                        I consent to the placement.</td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkRefuse2" runat="server" Text=" " />
                        I refuse the placement.</td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkPlacement2" runat="server" Text=" " />I request a meeting to discuss the refused placement. </td>
                </tr>
                <tr><td colspan="3">
                    <hr />
                    </td></tr>
                <tr>
                    <td>Signature of Parent, Guardian, Educational Surrogate Parent  Student 18 and Over *Required signature once a student reaches 18 unless there is a court appointed guardian.
                        <asp:TextBox ID="txtSignParentTwo" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        Date
                        <asp:TextBox ID="txtDateTwo"  runat="server" Width="30%" CssClass="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="float:right;">
                        <asp:Button ID="btnSaveIep9" runat="server" CssClass="NFButtonWithNoImage" Text="Save and Continue" OnClick="btnSaveIep9_Click" />

                        <%--<asp:Button ID="btnSaveIep9_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy" OnClick="btnSaveIep9_hdn_Click"  style="display:none;"/>--%>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
