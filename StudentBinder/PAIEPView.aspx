<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/PAIEPView.aspx.cs" Inherits="StudentBinder_PAIEPView" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <script src="../Administration/JS/jquery-1.8.0.min.js"></script>
    <link href="CSS/StylePE15.css" rel="stylesheet" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <title></title>
    <script type="text/javascript">

        function showWait() {



            $('#btnExport').attr('disable', 'disable');
            //$('.loading').show();
            //$('#fullContents').hide();
        }
        function HideWait() {

            $('#fullContents').show();
        }

        function showMessage(msg) {
            alert('dfgd');
            alert(msg);
        }

        function DownloadDone() {
            alert('hi');
            HideWait();
        }


        function checkPostbackExport() {

            showWait();
            return true;
        }

        function listUsers() {
            $('#overlay').fadeIn('slow', function () {
                $('#SignPopups').fadeIn();
            });
        }

        function Prompt() {
            alert("Hello");
            $('#overlay').show();
            $('.web_dialog2').show();
        }

        function HidePopup() {
            $('#overlay').hide();
            $('.web_dialog2').hide();
        }
        function closePOP() {
            $('#SignPopups').fadeOut('slow', function () {
                $('#overlay').fadeOut('fast');
            });
        }

        $(document).ready(function () {

            $('.tpContant').click(function () {

                $('#iepsidemenu').find('a').attr("class", "tpContant");
                $('#' + this.id).attr("class", "tpselected");
            });
        });


    </script>
    <style type="text/css">
        div.ContentAreaContainer table td {
            padding: 5px 1% !important;
            text-align: left;
        }

        /*LOADING IMAGE CLOSE */
        .web_dialog_overlay {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000000;
            opacity: .15;
            filter: alpha(opacity=15);
            -moz-opacity: .15;
            z-index: 101;
            display: none;
        }

        .web_dialog {
            /*display: block;
    position: fixed;
    top: -100%;
    left: 40%;
    margin-left: -190px;
    z-index: 102;
        */
            display: block;
            position: fixed;
            width: 800px;
            height: auto;
            top: -100%;
            left: 40%;
            margin-left: -190px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 999;
            background: #f8f7fc url(../images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
        }

        a.tpContant:link, a.tpContant:visited, a.tpContant:visited {
            border: 1px solid;
            width: 50px;
            margin: 2px;
            float: left;
        }


        ul {
            list-style: outside none disc !important;
            margin-left: 10px;
            padding: 0;
            text-align: left;
        }

        .tpselected {
            border: 1px solid;
            width: 50px;
            margin: 2px;
            float: left;
            background-color: #D2D2D2;
        }

        .web_dialog2 {
            display: none;
            position: fixed;
            min-width: 290px;
            min-height: 325px;
            left: 21%;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 1001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
            width: 650px;
            top: 2%;
            z-index: 9999;
        }
    </style>



</head>
<body>
    <form id="form1" runat="server">

        <%-- <div id="loadDiv" class="loading" runat="server">
            <div class="innerLoading">
                
                Please Wait...
            </div>
        </div>--%>
        <div id="fullContents" runat="server">
            <div>
                <div style="width: 100%">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <table style="width: 100%">
                        <tr style="height: 40px">
                            <td></td>

                            <td>
                                <asp:Button ID="btnSign" runat="server" Text="Sign" CssClass="NFButton" Style="float: right; margin-left: 6px;" OnClick="btnSign_Click" />
                                <asp:Button ID="btnSignDetails" runat="server" Text="Sign Details" CssClass="NFButton" Style="float: right; margin-left: 6px;" OnClick="btnSignDetails_Click" />
                                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="NFButton" OnClick="btnExport_Click" OnClientClick="return checkPostbackExport();" Style="float: right" />
                                <asp:Button ID="btnBSP" runat="server" Text="BSP Forms" CssClass="NFButton" OnClick="btnBSP_Click" Style="float: right;" Visible="false" />

                            </td>
                            <td style="width: 1%">
                                <asp:ImageButton ID="btnRefresh" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" Style="float: right" />
                            </td>
                        </tr>
                    </table>
                </div>





                <div id="iepsidemenu" style="text-align: center; width: 99px; margin-left: 64px; position: fixed;">

                    <a id="1" class="tpContant" href="#C1">IEP 1</a>
                    <a id="2" class="tpContant" href="#C2">IEP 2</a>
                    <a id="3" class="tpContant" href="#C3">IEP 3</a>
                    <a id="4" class="tpContant" href="#C4">IEP 4</a>
                    <a id="5" class="tpContant" href="#C5">IEP 5</a>
                    <a id="6" class="tpContant" href="#C6">IEP 6</a>
                    <a id="7" class="tpContant" href="#C7">IEP 7</a>
                    <a id="8" class="tpContant" href="#C8">IEP 8</a>
                    <a id="9" class="tpContant" href="#C9">IEP 9</a>
                    <a id="10" class="tpContant" href="#C10">IEP 10</a>
                    <a id="11" class="tpContant" href="#C11">IEP 11</a>
                    <a id="12" class="tpContant" href="#C12">IEP 12</a>
                    <a id="13" class="tpContant" href="#C13">IEP 13</a>
                    <a id="14" class="tpContant" href="#C14">IEP 14</a>
                    <a id="15" class="tpContant" href="#C15">IEP 15</a>
                </div>
                <div id="content" style="height: auto; margin-top: .2%; padding-top: .2%;" class="smalldashboard-RHS">



                    <a id="C1"></a>
                    <div id="iep1" tabindex="-1">

                        <div class="ContentAreaContainer">
                            <br />
                            <div class="clear"></div>
                            <table class="table" border="0" cellpadding="0" cellspacing="0">

                                <tr>
                                    <td class="righ top" style="text-align: center">
                                        <h2 class="simble">Individualized Education Program (IEP)</h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Student’s Name:</strong></span>
                                        <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label>

                                    </td>

                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>IEP Team Meeting Date (mm/dd/yy):</strong></span>
                                        <asp:Label ID="lblIEPTeamMeetingDate" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>IEP Implementation Date (Projected Date when Services and Programs Will Begin):</strong></span>
                                        <asp:Label ID="lblIEPImplementationDate" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Anticipated Duration of Services and Programs:</strong></span>
                                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Date of Birth:</strong></span>
                                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Age:</strong></span>
                                        <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Grade:</strong></span>
                                        <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Anticipated Year of Graduation:</strong></span>
                                        <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Local Education Agency (LEA):</strong></span>
                                        <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Country of Residence:</strong></span>
                                        <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Name and Address of Parent/Guardian/Surrogate:</strong></span>
                                        <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Phone (Home):</strong></span>
                                        <asp:Label ID="Label9" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Phone (Work):</strong></span>
                                        <asp:Label ID="Label10" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Other Information:</strong></span>
                                        <asp:Label ID="Label11" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="alignLeft righ"><span class="fontBoldSmall"><strong>The LEA and parent have agreed to make the following changes to the IEP without convening an IEP meeting, as documented by:</strong></span>
                                        <asp:Label ID="Label12" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>

                            </table>
                        </div>


                        <br />
                        <div class="splCtr" style="width: 92%;">



                            <asp:Repeater ID="Repeater1" runat="server">
                                <HeaderTemplate>

                                    <div class="ContentAreaContainer">
                                        <br />
                                        <div class="clear"></div>
                                        <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                            <tr class="fontBoldMedium">
                                                <td class="borderBox top1">Date of Revision(s)</td>
                                                <td class="borderBox top1">Participants/Roles</td>
                                                <td class="borderBox top1">IEP Section(s) Amended</td>

                                            </tr>
                                            <div class="clear"></div>
                                        </table>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>


                                    <table id="tableRepeat" class="table" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr class="borderBox">
                                            <td class="borderBox top"><%#DataBinder.Eval(Container,"DataItem.DateOfRevisions")%>
                                            </td>
                                            <td class="borderBox top"><%#DataBinder.Eval(Container,"DataItem.Participants")%>
                                            </td>
                                            <td class="borderBox top righ"><%#DataBinder.Eval(Container,"DataItem.IEPSections")%>
                                            </td>

                                        </tr>
                                    </table>

                                </ItemTemplate>
                            </asp:Repeater>

                        </div>

                        <br />
                        <a id="C2"></a>
                        <div id="iep2">
                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>

                                <table class="table" border="0" cellpadding="0" cellspacing="0">

                                    <tr>
                                        <td colspan="2" class="border top righ" style="text-align: center">
                                            <h2 class="simble">IEP TEAM/SIGNATURES</h2>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="border righ">The Individualized Education Program team makes the decisions about the student’s program and placement.
                             The student’s parent(s), the student’s special education teacher, and a representative from the Local Education Agency are required
                             members of this team. Signature on this IEP documents attendance, not agreement.</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <table style="border: thin;" border="0" cellpadding="0" cellspacing="0" class="container">
                                                <tr>
                                                    <td class="top1">Role</td>
                                                    <td class="top1">Printed Name</td>
                                                    <td class="top1 righ">Signature</td>
                                                </tr>
                                                <tr>
                                                    <td class="top2">Parent/Guardian/Surrogate</td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label13" runat="server" Text=""></asp:Label></td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label14" runat="server" Text=""></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="top2">Student*</td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label17" runat="server" Text=""></asp:Label></td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label18" runat="server" Text=""></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="top2">Regular Education Teacher**</td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label19" runat="server" Text=""></asp:Label></td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label20" runat="server" Text=""></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="top2">Special Education Teacher</td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label21" runat="server" Text=""></asp:Label></td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label22" runat="server" Text=""></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="top2">Local Ed Agency Rep</td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label23" runat="server" Text=""></asp:Label></td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label24" runat="server" Text=""></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="top2">Career/Tech Ed Rep***</td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label25" runat="server" Text=""></asp:Label></td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label26" runat="server" Text=""></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="top2">Community Agency Rep</td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label27" runat="server" Text=""></asp:Label></td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label28" runat="server" Text=""></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="top2">Teacher of the Gifted****</td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label29" runat="server" Text=""></asp:Label></td>
                                                    <td class="top2">
                                                        <asp:Label ID="Label30" runat="server" Text=""></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="border righ">*	The IEP team must invite the student if transition services are being planned or if the parents choose to have the student participate.<br />
                                            **	If the student is, or may be, participating in the regular education environment<br />
                                            ***	As determined by the LEA as needed for transition services and other community services<br />
                                            ****	A teacher of the gifted is required when writing an IEP for a student with a disability who also is gifted.<br />
                                            One individual listed above must be able to interpret the instructional implications of any evaluation results.

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="border">Written input received from the following members:&nbsp;<asp:Label ID="Label31" runat="server" Text=""></asp:Label></td>
                                        <td class="righ"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="border righ">Transfer of Rights at Age of Majority</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="border righ">For purposes of education, the age of majority is reached in Pennsylvania when the individual
                             reaches 21 years of age. Likewise, for purposes of the Individuals
                             with Disabilities Education Act, the age of majority is reached for students with disabilities when they reach 21 years of age.</td>
                                    </tr>

                                </table>
                                <div class="clear"></div>
                            </div>
                        </div>

                        <br />
                        <br />
                        <a id="C3"></a>
                        <div id="iep3">

                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table" border="0" cellpadding="0" cellspacing="0">

                                    <tr>
                                        <td colspan="2" class="border top righ">
                                            <h2 class="simble">PROCEDURAL SAFEGUARDS NOTICE</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="border righ">I have received a copy of the Procedural Safeguards Notice during this school year.
                             The Procedural Safeguards Notice provides information about my rights, including the process for disagreeing with the IEP.
                             The school has informed me whom I may contact if I need more information. </td>
                                    </tr>
                                    <tr>
                                        <td class="border">Signature of Parent/Guardian/Surrogate:&nbsp;<asp:Label ID="Label32" runat="server" Text=""></asp:Label></td>
                                        <td class="righ"></td>
                                    </tr>

                                </table>
                                <div class="clear"></div>
                            </div>
                        </div>

                        <br />
                        <a id="C4"></a>
                        <div id="iep4">

                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table" border="0" cellpadding="0" cellspacing="0">

                                    <tr>
                                        <td colspan="3" class="border top righ">
                                            <h2 class="simble">I. SPECIAL CONSIDERATIONS THE IEP TEAM MUST CONSIDER BEFORE DEVELOPING THE IEP. ANY FACTORS CHECKED AS “YES” MUST BE ADDRESSED IN THE IEP.</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="border righ" style="text-align: left">Is the student blind or visually impaired?</td>
                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="3">

                                            <table border="0" cellpadding="0" cellspacing="0" class="container">
                                                <tr>
                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox1" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">Yes</td>
                                                    <td style="width: 80%;" class="top2">The IEP must include a description of the instruction in Braille and the use of Braille unless the IEP team determines, 
                               after an evaluation of the student’s reading and writing skills, needs, and appropriate reading and writing media (including an
                               evaluation of the student’s future needs for instruction in Braille or the use of 
                               Braille), that instruction in Braille or the use of Braille is not appropriate for the student.</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox2" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">No</td>
                                                    <td style="width: 80%;" class="top2">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                        <td class="righ">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="border righ" style="text-align: left">Is the student deaf or hard of hearing?</td>
                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="3">

                                            <table border="0" cellpadding="0" cellspacing="0" class="container">
                                                <tr>
                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox3" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">Yes</td>
                                                    <td style="width: 80%;" class="top2">The IEP must include a communication plan to address the following: language and communication needs; opportunities 
                               for direct communications with peers and professional personnel in the student’s language and communication mode;
                               academic level; full range of needs, including opportunities for direct instruction in the student’s language and 
                               communication mode; and assistive technology devices and services. Indicate in which section of the IEP these 
                               considerations are addressed.  The Communication Plan must be completed and is available at <a href="http://www.patten.net">www.pattan.net</a></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox4" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">No</td>
                                                    <td style="width: 80%;" class="top2">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                        <td class="righ">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="border righ" style="text-align: left">Does the student have communication needs?</td>
                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="3">
                                            <table border="0" cellpadding="0" cellspacing="0" class="container">
                                                <tr>
                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox5" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">Yes</td>
                                                    <td style="width: 80%;" class="top2">Student needs must be addressed in the IEP (i.e., present levels, specially designed instruction (SDI), annual goals, etc.)</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox6" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">No</td>
                                                    <td style="width: 80%;" class="top2">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                        <td class="righ">&nbsp;</td>

                                    </tr>
                                    <tr>
                                        <td colspan="3" class="border righ" style="text-align: left">Does the student need assistive technology devices and/or services?</td>
                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="3">
                                            <table border="0" cellpadding="0" cellspacing="0" class="container">
                                                <tr>
                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox7" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">Yes</td>
                                                    <td style="width: 80%;" class="top2">Student needs must be addressed in the IEP (i.e., present levels, specially designed instruction, annual goals, etc.)</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox8" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">No</td>
                                                    <td style="width: 80%;" class="top2">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                        <td class="righ">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="border righ" style="text-align: left">Does the student have limited English proficiency? </td>
                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="3">
                                            <table border="0" cellpadding="0" cellspacing="0" class="container">
                                                <tr>
                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox9" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">Yes</td>
                                                    <td style="width: 80%;" class="top2">The IEP team must address the student’s language needs and how those needs relate to the IEP.</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox10" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">No</td>
                                                    <td style="width: 80%;" class="top2">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                        <td class="righ">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="border righ" style="text-align: left">Does the student exhibit behaviors that impede his/her learning or that of others?</td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="righ">

                                            <table border="0" cellpadding="0" cellspacing="0" class="container">
                                                <tr>

                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox11" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">Yes</td>
                                                    <td style="width: 80%;" class="top2">The IEP team must develop a Positive Behavior Support Plan that is based on a functional assessment of behavior
                               and that utilizes positive behavior techniques. Results of the functional assessment of behavior may be listed in the Present Levels section of
                               the IEP with a clear measurable plan to address the behavior in the Goals and Specially Designed Instruction sections of the IEP or in the Positive
                               Behavior Support Plan if this is a separate document that is attached to the IEP. A Positive Behavior Support Plan and a Functional Behavioral
                              Assessment form are available at <a href="http://www.patten.net">www.pattan.net</a></td>
                                                </tr>
                                                <tr>

                                                    <td style="width: 10%;" class="top2">
                                                        <input runat="server" id="Checkbox12" type="checkbox" /></td>
                                                    <td style="width: 10%;" class="top2">No</td>
                                                    <td style="width: 80%;" class="top2">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                        <td class="righ">&nbsp;</td>
                                    </tr>
                                </table>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <a id="C5"></a>
                        <div id="iep5">

                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table container" border="0" cellpadding="0" cellspacing="0">

                                    <tr>
                                        <td class="border top2" style="text-align: left">Other (specify): </td>
                                        <td class="top2">
                                            <asp:Label ID="Label33" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="bordertop2 top1">
                                            <h2 class="simble">II. PRESENT LEVELS OF ACADEMIC ACHIEVEMENT AND FUNCTIONAL PERFORMANCE</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="border top2">Include the following information related to the student:</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="border top2">
                                            <ul style="list-style-position: outside">
                                                <li>Present levels of academic achievement (e.g., most recent evaluation of the student, results of formative assessments,curriculum-based assessments, transition assessments, progress toward current goals)</li>
                                                <li>Present levels of functional performance (e.g., results from a functional behavioral assessment, results of ecologicalassessments, progress toward current goals)</li>
                                                <li>Present levels related to current postsecondary transition goals if the student’s age is 14 or younger if determined 
                                    appropriate by the IEP team (e.g., results of formative assessments, curriculum-based assessments, progress toward current goals)</li>
                                                <li>concerns for enhancing the education of the student</li>
                                                <li>How the student’s disability affects involvement and progress in the general education curriculum</li>
                                                <li>Strengths</li>
                                                <li>Academic, developmental, and functional needs related to student’s disability</li>
                                            </ul>
                                            <%--<asp:Label ID="Label34" runat="server" Text=""></asp:Label>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="border top2">
                                            <h2 class="simble">III. TRANSITION SERVICES – </h2>
                                            <strong>This is required for students age 14 or younger if determined appropriate by the IEP team.</strong>
                                            If the student does not attend the IEP meeting, the school must take other steps to ensure that the student’s
                               preferences and interests are considered. Transition services are a coordinated set of activities for a student
                               with a disability that is designed to be within a results oriented process, that is focused on improving the academic
                               and functional achievement of the student with a disability to facilitate the student’s movement from school to post 
                              school activities, including postsecondary education, vocational education, integrated employment (including supported 
                              employment), continuing and adult education, adult services, independent living, or community participation that is based
                               on the individual student’s needs taking into account the student’s strengths, preferences, and interests.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="border top2">
                                            <h2 class="simble">POST SCHOOL GOALS – </h2>
                                            Based on age appropriate assessment, define and project the appropriate measurable postsecondary goals that address education
                              and training, employment, and as needed, independent living. Under each area, list the services/activities and courses of 
                             study that support that goal. Include for each service/activity the location, frequency, projected beginning date, anticipated
                              duration, and person/agency responsible.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="border top2" style="text-align: left">For students in Career and Technology Centers, CIP Code:</td>
                                        <td class="top2">
                                            <asp:Label ID="Label35" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                </table>
                                <div class="clear"></div>
                            </div>
                        </div>

                        <br />

                        <a id="C6"></a>
                        <div id="iep6">


                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>

                                <table class="table" border="0" cellpadding="0" cellspacing="0" class="container">
                                    <tr>
                                        <td colspan="2" class="border top2">
                                            <h2 class="simble">Postsecondary Education and Training Goal – </h2>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td colspan="1" class="border top2" style="text-align: left; width: 58%;"><strong>Postsecondary Education and Training Goal:</strong>
                                            <asp:Label ID="Label36" runat="server" Text=""></asp:Label></td>
                                        <td class="top2">

                                            <table class="smlTbl">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="ChkMeasure1" runat="server" /></td>
                                                    <td>Measurable Annual Goal Yes/No (Document in Section V)</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: left" class="top2">Courses of Study:
                                            <asp:Label ID="Label37" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="top2">
                                            <div>


                                                <div>

                                                    <asp:Repeater ID="Repeater2" runat="server">
                                                        <HeaderTemplate>
                                                            <table id="tableRepeat" class="container" border="0" cellpadding="0" cellspacing="0" style="text-align: left;">
                                                                <tr class="fontBoldMedium">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Service/Activity</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Location</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Frequency</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Projected Beginning Date</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Anticipated Duration</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Person(s)/Agency Responsible</td>

                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="borderBox">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Service")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Location")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Frequency")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.PrjBeginning")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.AnticipatedDur")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Person")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>

                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear"></div>
                            </div>
                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="border " style="text-align: left; width: 41%;"><strong>Employment Goal:</strong>
                                            <asp:Label ID="Label38" runat="server" Text=""></asp:Label></td>
                                        <td>
                                            <table class="smlTbl" style="float: right; width: 68%">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="ChkMeasure2" runat="server" /></td>
                                                    <td>Measurable Annual Goal Yes/No (Document in Section V)</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: left">Courses of Study:
                                            <asp:Label ID="Label39" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>


                                                <div>

                                                    <asp:Repeater ID="Repeater3" runat="server">
                                                        <HeaderTemplate>
                                                            <table id="tableRepeat" border="0" cellpadding="0" cellspacing="0" class="container">
                                                                <tr class="fontBoldMedium">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Service/Activity</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Location</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Frequency</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Projected Beginning Date</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Anticipated Duration</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Person(s)/Agency Responsible</td>

                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table id="tableRepeat" border="0" cellpadding="0" cellspacing="0" class="container">
                                                                <tr class="borderBox">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Service")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Location")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Frequency")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.PrjBeginning")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.AnticipatedDur")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Person")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>

                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear"></div>
                            </div>



                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>


                                <table class="table" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="border top" style="text-align: left; width: 58%;"><strong>Independent Living Goal, if appropriate:</strong>
                                            <asp:Label ID="Label40" runat="server" Text=""></asp:Label></td>
                                        <td class="top righ">

                                            <table class="smlTbl">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="ChkMeasure3" runat="server" /></td>
                                                    <td>Measurable Annual Goal Yes/No (Document in Section V)</td>
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="2" style="text-align: left">Courses of Study:
                                            <asp:Label ID="Label41" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="2">
                                            <div>


                                                <div>

                                                    <asp:Repeater ID="Repeater4" runat="server">
                                                        <HeaderTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="fontBoldMedium">
                                                                    <td class="borderBox top2" style="text-align: center; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Service/Activity</td>
                                                                    <td class="borderBox top2" style="text-align: center; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Location</td>
                                                                    <td class="borderBox top2" style="text-align: center; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Frequency</td>
                                                                    <td class="borderBox top2" style="text-align: center; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Projected Beginning Date</td>
                                                                    <td class="borderBox top2" style="text-align: center; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Anticipated Duration</td>
                                                                    <td class="borderBox top2" style="text-align: center; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Person(s)/Agency Responsible</td>

                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="borderBox">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Service")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Location")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Frequency")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.PrjBeginning")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.AnticipatedDur")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Person")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>

                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear"></div>
                            </div>
                        </div>

                        <br />
                        <br />
                        <a id="C7"></a>
                        <div id="iep7">

                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="header top" style="text-align: left">&nbsp;</td>
                                        <td class="top righ" style="text-align: right">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="border righ" style="text-align: center">
                                            <h2 class="simble">IV. PARTICIPATION IN STATE AND LOCAL ASSESSMENTS</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <strong>Instructions for IEP Teams: </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1 righ" colspan="2">Please check the appropriate assessments. If the student will be assessed using the PSSA or the PSSA-Modified,
                              the IEP Team must choose which assessment will be administered for each content area (Reading, Mathematics, 
                             and Science). For example, a student may take the PSSA-Modified for Reading and the PSSA for Mathematics 
                              Science. If the student will be assessed using the PASA, the IEP Team need not select content areas because 
                             ALL content areas will be assessed using the PASA.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1 righ" colspan="2">
                                            <ul>
                                                <li><strong>PSSA </strong>(Please choose the appropriate option and content areas for the student. A student may be eligible to 
                                     be assessed using the PSSA-Modified assessment for one or more content areas and be assessed using the PSSA 
                                     for other content areas.) 
                                                </li>
                                                <li>
                                                    <strong>PSSA-Modified</strong> (Please choose the appropriate option and content areas for the student. A student may be 
                                     eligible to be assessed using the PSSA-Modified assessment for one or more content areas and be assessed using the PSSA 
                                     for other content areas.)
                                                </li>
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">Allowable accommodations may be found in the PSSA Accommodations Guidelines at: </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ"><a href="http://www.portal.state.pa.us/portal/server.pt/community/testing_accommodations__security/7448">www.portal.state.pa.us/portal/server.pt/community/testing_accommodations__security/7448 </a></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">Criteria regarding PSSA-Modified eligibility may be found in Guidelines for IEP Teams: Assigning Students with IEPs to State Tests (ASIST) at: </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ"><a href="http://www.education.state.pa.us/portal/server.pt/community/special_education/7465/assessment/607491">www.education.state.pa.us/portal/server.pt/community/special_education/7465/assessment/607491</a></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">Criteria regarding PASA eligibility may be found in Guidelines for IEP Teams: Assigning Students with IEPs to State Tests (ASIST) at: </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ"><a href="http://www.education.state.pa.us/portal/server.pt/community/special_education/7465/assessment/607491">www.education.state.pa.us/portal/server.pt/community/special_education/7465/assessment/607491</a></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">Not Assessed (Please select if student is not being assessed by a state assessment this year)</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="IEP8AsmtNotAdministred" runat="server" /></td>
                                        <td class="righ">Assessment is not administered at this student’s grade level</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">Reading (PSSA grades 3-8, 11; PSSA-M grades 4-8, 11)</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="IEP8ReadPartcptPSSAWithoutAcmdtn" runat="server" /></td>
                                        <td class="righ">Student will participate in the PSSA without accommodations</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="IEP8ReadPartcptPSSAWithFollowingAcmdtn" runat="server" /></td>
                                        <td class="righ">Student will participate in the PSSA with the following appropriate accommodations:</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="IEP8ReadPartcptPSSAModiWithoutAcmdtn" runat="server" /></td>
                                        <td class="righ">Student will participate in the PSSA-Modified without accommodations</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="IEP8ReadPartcptPSSAModiWithFollowingAcmdtn" runat="server" /></td>
                                        <td class="righ">Student will participate in the PSSA-Modified with the following appropriate accommodations:</td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="righ">Math (PSSA grades 3-8, 11; PSSA-M grades 4-8, 11)</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="IEP8MathPartcptPSSAWithoutAcmdtn" runat="server" /></td>
                                        <td class="righ">Student will participate in the PSSA without accommodations</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="IEP8MathPartcptPSSAWithFollowingAcmdtn" runat="server" /></td>
                                        <td class="righ">Student will participate in the PSSA with the following appropriate accommodations:</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="IEP8MathPartcptPSSAModiWithoutAcmdtn" runat="server" /></td>
                                        <td class="righ">Student will participate in the PSSA-Modified without accommodations</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="IEP8MathPartcptPSSAModiWithFollowingAcmdtn" runat="server" /></td>
                                        <td class="righ">Student will participate in the PSSA-Modified with the following appropriate accommodations:</td>
                                    </tr>

                                </table>
                                <div class="clear"></div>
                            </div>
                        </div>

                        <br />
                        <br />
                        <a id="C8"></a>
                        <div id="iep8">
                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table container" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="2" class="border righ" style="text-align: center">
                                            <h2 class="simble">Additional Information</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="header top2" style="text-align: left">&nbsp;</td>
                                        <td class="top2" style="text-align: right">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ"><strong>Science (PSSA grades 4, 8, 11; PSSA-M grades 8, 11) </strong></td>
                                    </tr>
                                    <tr>
                                        <td class="top2">
                                            <asp:CheckBox ID="IEP8SciencePartcptPSSAWithoutAcmdtn" runat="server" /></td>
                                        <td class="top2">Student will participate in the PSSA without accommodations</td>
                                    </tr>
                                    <tr>
                                        <td class="top2">
                                            <asp:CheckBox ID="IEP8SciencePartcptPSSAWithFollowingAcmdtn" runat="server" /></td>
                                        <td class="top2">Student will participate in the PSSA with the following appropriate accommodations:</td>
                                    </tr>
                                    <tr>
                                        <td class="top2">
                                            <asp:CheckBox ID="IEP8SciencePartcptPSSAModiWithoutAcmdtn" runat="server" /></td>
                                        <td class="top2">Student will participate in the PSSA-Modified without accommodations</td>
                                    </tr>
                                    <tr>
                                        <td class="top2">
                                            <asp:CheckBox ID="IEP8SciencePartcptPSSAModiWithFollowingAcmdtn" runat="server" /></td>
                                        <td class="top2">Student will participate in the PSSA-Modified with the following appropriate accommodations:</td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="righ"><strong>Writing (PSSA grades 5, 8, 11)</strong></td>
                                    </tr>
                                    <tr>
                                        <td class="top2">
                                            <asp:CheckBox ID="IEP8WritePartcptPSSAWithoutAcmdtn" runat="server" /></td>
                                        <td class="top2">Student will participate in the PSSA without accommodations</td>
                                    </tr>
                                    <tr>
                                        <td class="top2">
                                            <asp:CheckBox ID="IEP8WritePartcptPSSAWithFollowingAcmdtn" runat="server" /></td>
                                        <td class="top2">Student will participate in the PSSA with the following appropriate accommodations:</td>
                                    </tr>


                                    <tr>
                                        <td colspan="2" class="righ"><strong>PASA (PASA grades 3-8, 11 for Reading and Math; Grades 4, 8, 11 for Science)</strong></td>
                                    </tr>
                                    <tr>
                                        <td class="top2">
                                            <asp:CheckBox ID="IEP8PSSAParticipate" runat="server" /></td>
                                        <td class="top2">Student will participate in the PASA</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ"><strong>Explain why the student cannot participate in the PSSA or the PSSA-M for Reading, Math, or Science:</strong></td>
                                    </tr>
                                    <tr>
                                        <td class="top2" colspan="2">
                                            <asp:Label ID="Label42" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ"><strong>Explain why the PASA is appropriate:</strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="top2">
                                            <asp:Label ID="Label43" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ"><strong>Choose how the student’s performance on the PASA will be documented.</strong></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="top2">
                                            <input runat="server" id="chkContent2" type="checkbox" />Videotape (will be kept confidential as all other school records)</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <input runat="server" id="Checkbox13" type="checkbox" />Written narrative (will be kept confidential as all other school records)</td>
                                    </tr>


                                </table>
                                <div class="clear"></div>
                            </div>
                        </div>


                        <br />
                        <br />
                        <a id="C9"></a>
                        <div id="iep9">

                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="header top" style="text-align: left">&nbsp;</td>
                                        <td class="top righ" style="text-align: right">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <h2 class="simble">Local Assessments</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <asp:CheckBoxList ID="CheckBoxListLocalAsssesment" runat="server">
                                                <asp:ListItem Value="A">Local assessment is not administered at this student’s grade level; OR</asp:ListItem>
                                                <asp:ListItem Value="B">Student will participate in local assessments without accommodations; OR</asp:ListItem>
                                                <asp:ListItem Value="C">Student will participate in local assessments with the following accommodations; OR</asp:ListItem>
                                            </asp:CheckBoxList></td>
                                        <%--<td colspan="2"><input runat="server" id="Checkbox14" type="checkbox" />Local assessment is not administered at this student’s grade level; OR</td>
                     </tr>
                     <tr>
                        <td colspan="2"><input runat="server" id="Checkbox15" type="checkbox" />Student will participate in local assessments without accommodations; OR</td>
                     </tr>
                     <tr>
                        <td colspan="2"><input runat="server" id="Checkbox16" type="checkbox" />Student will participate in local assessments with the following accommodations; OR</td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <asp:Label ID="TextBoxDetailsA" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <input runat="server" id="Checkbox17" type="checkbox" />The student will take an alternate local assessment.</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">Explain why the student cannot participate in the regular assessment:</td>
                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:Label ID="TextBoxDetailsB" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">Explain why the alternate assessment is appropriate:</td>
                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:Label ID="TextBoxDetailsC" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                </table>

                                <div class="clear"></div>
                            </div>
                        </div>

                        <br />
                        <br />

                        <a id="C10"></a>
                        <div id="iep10">
                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="header top" style="text-align: left">&nbsp;</td>
                                        <td style="text-align: right" class="righ top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <h2 class="simble">V. GOALS AND OBJECTIVES –</h2>
                                            Include, as appropriate, academic and functional goals. 
                            Use as many copies of this page as needed to plan appropriately. Specially designed instruction may be listed with each 
                            goal/objective or listed in Section VI.</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">Short term learning outcomes are required for students who are gifted. The short term learning outcomes related to 
                             the student’s gifted program may be listed under Goals or Short Term Objectives.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <div>


                                                <div>

                                                    <asp:Repeater ID="Repeater5" runat="server">
                                                        <HeaderTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="fontBoldMedium">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">MEASURABLE ANNUAL GOAL Include: Condition, Name, Behavior, and Criteria (Refer to Annotated IEP for description of these components)</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Describe HOW the student’s progress toward meeting this goal will be measured</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Describe WHEN periodic reports on progress will be provided to parents</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Report of Progress</td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="borderBox">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.MeasureAnualGoal")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.StudentsProgress")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.DescReportProgress")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.ReportProgress")%>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>

                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="righ">
                                            <h2 class="simble">SHORT TERM OBJECTIVES – </h2>
                                            Required for students with disabilities who take 
                            alternate assessments aligned to alternate achievement standards (PASA).</td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="righ">
                                            <asp:Repeater ID="Benchmarkslist" runat="server">
                                                <HeaderTemplate>
                                                    <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                        <tr class="fontBoldMedium">
                                                            <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Short term objectives / Benchmarks</td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                        <tr class="borderBox">
                                                            <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Benchmark")%>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                        </td>
                                    </tr>

                                    <%--<tr>
                        <td colspan="2">
                            <div>
                    

                    <div style="border:1px solid gray">

    <asp:Repeater ID="Repeater6" runat="server">
        <HeaderTemplate>
             <table id="tableRepeat" class="table" style="width:100%;">
            <tr class="fontBoldMedium">
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Short term objectives / Benchmarks</td>
                    </tr>
                 </table>
        </HeaderTemplate>
            <ItemTemplate>
            <table id="tableRepeat" class="table" style="width:100%;">        
                    <tr class="borderBox">
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.IEPGoalNo")%>
                        </td>
                     </tr>
            </table>
                </ItemTemplate>
        </asp:Repeater>

</div>

        </div>
                        </td>
                    </tr>--%>
                                </table>

                                <div class="clear"></div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <a id="C11"></a>
                        <div id="iep11">
                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="header top" style="text-align: left">&nbsp;</td>
                                        <td class="righ top" style="text-align: right">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <h2 class="simble">Special Considerations</h2>
                                            VI. SPECIAL EDUCATION / RELATED SERVICES / SUPPLEMENTARY AIDS AND SERVICES / PROGRAM MODIFICATIONS –Include, as appropriate, for nonacademic and extracurricular services and activities.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <span class="header">A. PROGRAM MODIFICATIONS AND SPECIALLY DESIGNED INSTRUCTION (SDI) </span>
                                            <ul>
                                                <li>SDI may be listed with each goal or as part of the table below.</li>
                                                <li>Include supplementary aids and services as appropriate.</li>
                                                <li>For a student who has a disability and is gifted, SDI also should include adaptations, accommodations, or modifications to the general education curriculum, as appropriate for a student with a disability.</li>
                                            </ul>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td colspan="2" class="righ">
                                            <div>


                                                <div>

                                                    <asp:Repeater ID="Repeater7" runat="server">
                                                        <HeaderTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="fontBoldMedium">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Modifications and SDI</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Location</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Frequency</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Projected Beginning Date</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Anticipated Duration</td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="borderBox">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.SDI")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Location")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Frequency")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.PrjBeginning")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.AnticipatedDur")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>

                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="righ">
                                            <h2 class="simble">B. RELATED SERVICES –</h2>
                                            List the services that the student needs in order to benefit from his/her special education program.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <div>


                                                <div>

                                                    <asp:Repeater ID="Repeater8" runat="server">
                                                        <HeaderTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="fontBoldMedium">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Service</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Location</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Frequency</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Projected Beginning Date</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Anticipated Duration</td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="borderBox">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Service")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Location")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Frequency")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.PrjBeginning")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.AnticipatedDur")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>

                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="righ">
                                            <h2 class="simble">C. SUPPORTS FOR SCHOOL PERSONNEL – </h2>
                                            List the staff to receive the supports and the supports needed to implement the student’s IEP.
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="righ">
                                            <div>


                                                <div>

                                                    <asp:Repeater ID="Repeater9" runat="server">
                                                        <HeaderTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="fontBoldMedium">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">School Personnel to Receive Support</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Support</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Location</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Frequency</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Projected Beginning Date</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Anticipated Duration</td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="borderBox">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.SchoolPerson")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Person")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Location")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Frequency")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.PrjBeginning")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.AnticipatedDur")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>

                                            </div>
                                        </td>
                                    </tr>
                                </table>

                                <div class="clear"></div>
                            </div>
                        </div>
                        <br />

                        <a id="C12"></a>
                        <div id="iep12">
                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="header top" style="text-align: left">&nbsp;</td>
                                        <td class="righ top" style="text-align: right">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <h2 class="simble">GIFTED SUPPORT SERVICES FOR A STUDENT–</h2>
                                            D.IDENTIFIED AS GIFTED WHO ALSO IS IDENTIFIED AS A STUDENT WITH A DISABILITY-Support services are required to assist a gifted student to benefit from gifted education (e.g., psychological services, parent counseling and education, counseling services, transportation to and from gifted programs to classrooms in buildings operated by the school district).
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <div>


                                                <div>
                                                    <asp:Repeater ID="rptrsupportServices" runat="server">
                                                        <HeaderTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="fontBoldMedium">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Support service</td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="borderBox">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.SupportService")%>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>

                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <h2 class="simble">E. EXTENDED SCHOOL YEAR (ESY) –</h2>
                                            The IEP team has considered and discussed ESY services, and determined that:</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <asp:CheckBox ID="CheckBox18" runat="server" Text="Student IS eligible for ESY based on the following information or data reviewed by the IEP team:" /></td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="righ">
                                            <asp:Label ID="Label47" runat="server" Text=""></asp:Label><br />
                                            OR</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <asp:CheckBox ID="CheckBox19" runat="server" Text="As of the date of this IEP, student is NOT eligible for ESY based on the following information or data reviewed by the IEP team:" /></td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="righ">
                                            <asp:Label ID="Label48" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">The Annual Goals and, when appropriate, Short Term Objectives from this IEP that are to be addressed in the student’s ESY Program are:</td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" class="righ">
                                            <asp:Label ID="Label49" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ"> If the IEP team has determined ESY is appropriate, complete the following:</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <div>


                                                <div>

                                                    <asp:Repeater ID="Repeater10" runat="server">
                                                        <HeaderTemplate>
                                                            <table id="tableRepeat" class="table container" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="fontBoldMedium">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">ESY Service to be Provided</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Location</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Frequency</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Projected Beginning Date</td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;">Anticipated Duration</td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table id="tableRepeat" class="table" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="borderBox">
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.ESY")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Location")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.Frequency")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.PrjBeginning", "{0:MM/dd/yyyy}")%>
                                                                    </td>
                                                                    <td class="borderBox top2" style="text-align: left; min-width: 87px; max-width: 87px; table-layout: fixed; word-wrap: break-word;"><%#DataBinder.Eval(Container,"DataItem.AnticipatedDur")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>

                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <br />

                        <a id="C13"></a>
                        <div id="iep13">
                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="header top" style="text-align: left">&nbsp;</td>
                                        <td class="top righ" style="text-align: right">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <h2 class="simble">VII. EDUCATIONAL PLACEMENT-</h2>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td colspan="2" class="righ top"><strong>A. QUESTIONS FOR IEP TEAM</strong>


                                            <br />
                                            <br />
                                            The following questions must be reviewed and discussed by the IEP team prior to providing the explanations regarding participation with students without disabilities.</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">It is the responsibility of each public agency to ensure that, to the maximum extent appropriate, students with disabilities, including those in public or private institutions or other care facilities are educated with students who are not disabled. Special classes, separate schooling or other removal of students with disabilities from the general educational environment occurs only when the nature or severity of the disability is such that education in general education classes, EVEN WITH the use of supplementary aids and services, cannot be achieved satisfactorily.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <ul>
                                                <li>What supplementary aids and services were considered? What supplementary aids and services were rejected? Explain why the supplementary aids and services will or will not enable the student to make progress on the goals and objectives (if applicable) in this IEP in the general education class.</li>
                                                <li>What benefits are provided in the general education class with supplementary aids and services versus the benefits provided in the special education class?</li>
                                                <li>What potentially beneficial effects and/or harmful effects might be expected on the student with disabilities or the other students in the class, even with supplementary aids and services?</li>
                                                <li>To what extent, if any, will the student participate with nondisabled peers in extracurricular activities or other nonacademic activities?</li>
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">Explanation of the extent, if any, to which the student will not participate with students without disabilities in the regular education class:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <asp:Label ID="Label50" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">Explanation of the extent, if any, to which the student will not participate with students without disabilities in the general education curriculum:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ">
                                            <asp:Label ID="Label51" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>

                                <div class="clear"></div>
                            </div>
                        </div>
                        <br />

                        <a id="C14"></a>
                        <div id="iep14">
                            <div class="ContentAreaContainer">
                                <br />
                                <div class="clear"></div>
                                <table class="table" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="righ">
                                            <h2 class="simble">Educational Placement- II</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="righ top"><strong>B. Type of Support</strong></td>
                                    </tr>

                                    <tr>
                                        <td class="righ" colspan="2">
                                            <strong>1. Amount of special education supports</strong>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="2">
                                            <asp:CheckBox ID="chkItinerant" runat="server" Text="Itinerant: Special education supports and services provided by special education personnel for 20% or less of the school day" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ" style="line-height: 20px;" colspan="2">
                                            <asp:CheckBox ID="chkSupplemental" runat="server" Text="Supplemental: Special education supports and services provided by special education personnel for more than 20% of the day but less &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             than 80% of the school day" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="2">
                                            <asp:CheckBox ID="chkFullTime" runat="server" Text="Full-Time: Special education supports and services provided by special education personnel for 80% or more of the school day" />
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="righ" colspan="2">2. Type of special education supports</td>

                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="2">
                                            <asp:CheckBox ID="chkAutistic" runat="server" Text="Autistic Support" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="2">
                                            <asp:CheckBox ID="chkBlind" runat="server" Text="Blind-Visually Impaired Support" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="2">
                                            <asp:CheckBox ID="chkDeaf" runat="server" Text="Deaf and Hard of Hearing Support" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ" colspan="2">
                                            <asp:CheckBox ID="chkEmotional" runat="server" Text="Emotional Support" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:CheckBox ID="chkLearning" runat="server" Text="Learning Support" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:CheckBox ID="chkLifeskills" runat="server" Text="Life Skills Support" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:CheckBox ID="chkMultipleDis" runat="server" Text="Multiple Disabilities Support" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:CheckBox ID="chkPhysical" runat="server" Text="Physical Support" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:CheckBox ID="chkSpeech" runat="server" Text="Speech and Language Support" />
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="righ">C. Location of student’s program</td>

                                    </tr>

                                    <tr>
                                        <td class="righ">Name of School District where the IEP will be implemented&nbsp; :
                        <asp:Label ID="Label53" runat="server" Text=""></asp:Label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ">Name of School Building where the IEP will be implemented :
                        <asp:Label ID="Label52" runat="server" Text=""></asp:Label>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="righ">Is this school the student’s neighborhood school (i.e., the school the student would attend if he/she did not have an IEP)?</td>

                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:CheckBox ID="chkYes" runat="server" Text="Yes" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:CheckBox ID="ChkNo" runat="server" Text="No. If the answer is “no,” select the reason why not." />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:CheckBox ID="ChKSpecialEducation" runat="server" Text="Special education supports and services required in the student’s IEP cannot be provided in the neighborhood school" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:CheckBox ID="chkOther" runat="server" Text="Other. Please explain:" />
                                        </td>

                                    </tr>
                                </table>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <br />

                        <a id="C15"></a>
                        <div id="iep15">




                            <div>
                                <div class="clear"></div>
                                <div class="ContentAreaContainer">
                                    <br />
                                    <div class="clear"></div>
                                    <h2 class="simble">INDIVIDUALIZED EDUCATION PROGRAM (IEP)</h2>
                                    <div class="clear"></div>
                                </div>
                                <div class="ContentAreaContainer">
                                    <br />
                                    <div class="clear"></div>

                                    <table border="0" cellpadding="0" cellspacing="0">

                                        <tr>
                                            <td class="alignLeft righ"><span class="fontBoldSmall"><strong>Student’s Name:</strong></span>
                                                <asp:Label ID="lblstudentnme" runat="server" Text=""></asp:Label>

                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="righ">
                                                <h4>VIII. PENNDATA REPORTING: Educational Environment (Complete either Section A or B; Select only one Educational Environment)</h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="righ">
                                                <p>To calculate the percentage of time inside the regular classroom, divide the number of hours the student spends inside the regular classroom by the total number of hours in the school day (including lunch, recess, study periods). The result is then multiplied by 100.</p>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="righ">
                                                <p><strong>SECTION A: For Students Educated in Regular School Buildings with Nondisabled Peers – Indicate the percentage of time INSIDE the regular classroom for this student:</strong></p>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="righ">
                                                <p>Time spent outside the regular classroom receiving services unrelated to the student’s disability (e.g., time receiving ESL services) should be considered time inside the regular classroom. Educational time spent in age-appropriate community-based settings that include individuals with and without disabilities, such as college campuses or vocational sites, should be counted as time spent inside the regular classroom.</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="righ">
                                                <h3>Calculation for this Student:</h3>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="clear"></div>
                                </div>
                                <div class="ContentAreaContainer">
                                    <br />
                                    <div class="clear"></div>
                                    <table border="0" cellpadding="0" cellspacing="0" class="container">
                                        <tr>
                                            <td class="top2" style="width: 10%;">Column 1</td>
                                            <td class="top2" style="width: 10%;">Column 2</td>
                                            <td class="top2" style="width: 10%;">Calculation	</td>
                                            <td class="top2" style="width: 30%;">Indicate Percentage</td>
                                            <td class="top2" style="width: 30%;">Percentage Category</td>
                                        </tr>
                                        <tr>
                                            <td class="top2" style="width: 10%;">Total hours the student spends in the regular classroom per day</td>
                                            <td class="top2" style="width: 10%;">Total hours in a typical school day (including lunch, recess & study periods)
                                            </td>
                                            <td class="top2" style="width: 10%;">(Hours inside regular classroom ÷ hours in school day) x 100 = % (Column 1 ÷ Column 2) x 100 = %
                                            </td>
                                            <td class="top2" style="width: 30%;">Section A: The percentage of time student spends inside the regular classroom:</td>
                                            <td class="top2 righ" style="width: 30%;">Using the calculation result – select the appropriate percentage category</td>
                                        </tr>
                                        <tr>
                                            <td class="top2"></td>
                                            <td class="top2"></td>
                                            <td class="top2"></td>
                                            <td class="top2">% of the day</td>
                                            <td class="righ">
                                                <asp:CheckBox ID="chkregularcls80" runat="server" CssClass="chkbx" /><label>INSIDE the Regular Classroom 80% or More of the Day</label><div class="clear"></div>
                                                <asp:CheckBox ID="chkregularcls79" runat="server" CssClass="chkbx" /><label>INSIDE the Regular Classroom 79-40% of the Day</label><div class="clear"></div>
                                                <asp:CheckBox ID="chkregularcls40" runat="server" CssClass="chkbx" /><label>INSIDE the Regular Classroom Less Than 40% of the Day</label><div class="clear"></div>

                                            </td>
                                        </tr>

                                    </table>

                                    <div class="clear"></div>
                                </div>
                                <p><strong>SECTION B: This section required only for Students Educated OUTSIDE Regular School Buildings for more than 50% of the day – select and indicate the Name of School or Facility</strong> on the line corresponding with the appropriate selection: (If a student spends less than 50% of the day in one of these locations, the IEP team must do the calculation in Section A)</p>

                                <div class="ContentAreaContainer">
                                    <br />
                                    <div class="clear"></div>
                                    <table border="0" cellpadding="0" cellspacing="0" class="container">
                                        <tr>
                                            <td class="top2" width="15%">
                                                <asp:CheckBox ID="chkApproveprivateschool" runat="server" CssClass="chkbx" /><label>Approved Private School (Non Residential)</label></td>
                                            <td class="top2" width="15%">
                                                <asp:TextBox ID="txtApprovePrivate" runat="server" CssClass="textfield"></asp:TextBox></td>
                                            <td class="top2" width="15%">
                                                <asp:CheckBox ID="chkotherpublic" runat="server" CssClass="chkbx" /><label>Other Public Facility (Non Residential)</label></td>
                                            <td class="top2" width="15%">
                                                <asp:TextBox ID="txtotherpublic" runat="server" CssClass="textfield"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="top2">
                                                <asp:CheckBox ID="chkapproveresidential" runat="server" CssClass="chkbx" /><label>Approved Private School (Residential)</label></td>
                                            <td class="top2">
                                                <asp:TextBox ID="txtapproveresidential" runat="server" CssClass="textfield"></asp:TextBox></td>
                                            <td class="top2">
                                                <asp:CheckBox ID="chkhospital" runat="server" CssClass="chkbx" /><label>Hospital/Homebound</label></td>
                                            <td class="top2">
                                                <asp:TextBox ID="txthospital" runat="server" CssClass="textfield"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="top2">
                                                <asp:CheckBox ID="chkprivatefacility" runat="server" CssClass="chkbx" /><label>Other Private Facility (Non Residential)</label></td>
                                            <td class="top2">
                                                <asp:TextBox ID="txtprivatefacility" runat="server" CssClass="textfield"></asp:TextBox></td>
                                            <td class="top2">
                                                <asp:CheckBox ID="chkcorrectionalfacility" runat="server" CssClass="chkbx" /><label>Correctional Facility</label></td>
                                            <td class="top2">
                                                <asp:TextBox ID="txtcorrectfacility" runat="server" CssClass="textfield"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="top2">
                                                <asp:CheckBox ID="chkprivateresidential" runat="server" CssClass="chkbx" /><label>Other Private Facility (Residential)</label></td>
                                            <td class="top2">
                                                <asp:TextBox ID="txtprivateresidential" runat="server" CssClass="textfield"></asp:TextBox></td>
                                            <td class="top2">
                                                <asp:CheckBox ID="chkoutofstate" runat="server" CssClass="chkbx" /><label>Out of State Facility</label></td>
                                            <td class="top2">
                                                <asp:TextBox ID="txtoutofstate" runat="server" CssClass="textfield"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="top2">
                                                <asp:CheckBox ID="chkpublicfacility" runat="server" CssClass="chkbx" /><label>Other Public Facility (Residential)</label></td>
                                            <td class="top2">
                                                <asp:TextBox ID="txtpublicfacility" runat="server" CssClass="textfield"></asp:TextBox></td>
                                            <td class="top2">
                                                <asp:CheckBox ID="chkInstructionconducted" runat="server" CssClass="chkbx" /><label>Instruction Conducted in the Home</label></td>
                                            <td class="top2">
                                                <asp:TextBox ID="txtinstructionalconduct" runat="server" CssClass="textfield"></asp:TextBox></td>
                                        </tr>
                                    </table>

                                    <div class="clear"></div>
                                </div>
                                <p><strong>EXAMPLES </strong>for Section A: How to Calculate PennData – Educational Environment Percentages</p>

                                <div class="ContentAreaContainer">
                                    <br />
                                    <div class="clear"></div>


                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="top" width="15%"></td>
                                            <td class="top" width="15%">Column 1</td>
                                            <td class="top" width="15%">Column 2</td>
                                            <td class="top" width="15%">Calculation	</td>
                                            <td width="15%" class="top righ">Indicate Percentage</td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>Total hours the student spends in the regular classroom – per day</td>
                                            <td width="15%">Total hours in a typical school day (including lunch, recess & study periods)</td>
                                            <td width="15%">(Hours inside regular classroom ÷ hours in school day) x 100 = % (Column 1 ÷ Column 2) x 100 = %
                                            </td>
                                            <td width="15%" class="righ">Section A: The percentage of time student spends inside the regular classroom:</td>
                                        </tr>
                                        <tr>
                                            <td>Example 1  </td>
                                            <td>5.5</td>
                                            <td width="15%">6.5</td>
                                            <td width="15%">(5.5 ÷ 6.5) x 100 = 85%</td>
                                            <td width="15%" class="righ">85% of the day (Inside 80% or More of Day)</td>
                                        </tr>
                                        <tr>
                                            <td>Example 2  </td>
                                            <td>3</td>
                                            <td width="15%">5</td>
                                            <td width="15%">(3 ÷ 5) x 100 = 60%</td>
                                            <td width="15%" class="righ">60% of the day (Inside 79-40% of Day)</td>
                                        </tr>

                                        <tr>
                                            <td>Example 3  </td>
                                            <td>1</td>
                                            <td width="15%">5</td>
                                            <td width="15%">(1 ÷ 5) x 100 = 20%</td>
                                            <td width="15%" class="righ">20% of the day (Inside less than 40% of Day)</td>
                                        </tr>


                                    </table>

                                    <div class="clear"></div>
                                </div>

                                <div class="clear"></div>
                            </div>
                            <p>For help in understanding this form, an annotated IEP is available on the PaTTAN website at www.pattan.net  Type “Annotated Forms” in the Search feature on the website. If you do not have access to the Internet, you can request the annotated form by calling PaTTAN at 800-441-3215.</p>
                            <div class="clear"></div>
                        </div>

                        <br />
                    </div>
                </div>
            </div>
        </div>
        <div runat="server" id="dddd"></div>
        <div id="overlay" class="web_dialog_overlay"></div>

        <div id="PopDownload" class="web_dialog" style="width: 600px; top: -20%;">

            <div id="Div53" style="width: 700px;">

                <table style="width: 97%">
                    <tr>
                        <td colspan="2" runat="server" id="tdMsg1"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label16" runat="server" Text="Label"></asp:Label>
                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" />
                            <div runat="server" id="tdMsgExportNew" class="tdText" style="height: 50px"></div>
                        </td>
                        <td style="text-align: left">

                            <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" OnClick="btnDone_Click" />

                        </td>
                    </tr>
                </table>

            </div>
        </div>
        <div id="SignPopups" class="web_dialog" style="width: 500px; top: 1%; margin-left: -3%; display: none;">

            <a id="A2" class="close sprited1" href="#" style="margin-top: -5px; margin-right: 0px;">
                <img src="../Administration/images/clb.PNG" onclick="closePOP();" style="float: right; margin-right: -15px; margin-top: -12px; z-index: 300" width="18" height="18" alt="" /></a>

            <div id="Div5" style="width: 100%; margin-left: 10px 0 0 0">
                <h2 style="margin: 10px;">Signed Users</h2>
                <hr />
                <table style="width: 100%">

                    <tr>
                        <td id="tdUSers" runat="server" style="margin: 5px;"></td>
                    </tr>


                </table>
            </div>
        </div>


        <%-- PopUp for BSPForms //hari 04-May-2015--%>

        <div id="divPrmpts" class="web_dialog2">
            <a id="A4" onclick="HidePopup();" href="#" style="margin-top: -13px; margin-right: -14px;">
                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
            <br />
            <h4>BSP Forms</h4>



            <div id="divMessage" runat="server" style="width: 98%"></div>
            <asp:GridView GridLines="none" CellPadding="4" ID="grdFile" PageSize="5" AllowPaging="True" Width="100%" OnRowEditing="grdFile_RowEditing" OnRowCommand="grdFile_RowCommand" AutoGenerateColumns="false" runat="server" OnPageIndexChanging="grdFile_PageIndexChanging" OnRowDataBound="grdFile_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="No" HeaderText="No" HeaderStyle-Width="10px" />
                    <asp:TemplateField HeaderText="Document Name" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label CommandName="lbldownload" ID="lbllnkDownload" Text='<%# Eval("Document") %>' CommandArgument='<%# Eval("BSPDoc") %>' runat="server" Style="float: ">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Download" HeaderStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%--<asp:Button  ID="lnkDownload1" runat="server" CssClass="NFButton" Text="Download"   CommandArgument='<%# Eval("BSPDoc") %>' CommandName="download"  Width="18px" Style="float:right;" />--%>
                            <%--<asp:Button ID="lnkDownload" runat="server" CssClass="NFButton" Text="Dwnld" CommandArgument='<%# Eval("BSPDoc") %>' CommandName="download" Width="18px" Style="float:right;" />--%>
                            <asp:ImageButton ID="lnkDownload" runat="server" Text='<%# Eval("Document") %>' CommandArgument='<%# Eval("BSPDoc") %>' CommandName="download" ImageUrl="~/Administration/images/download_down_arrow.png" Width="18px" Style="float: right;" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
                <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                <RowStyle CssClass="RowStyle" />
                <AlternatingRowStyle CssClass="AltRowStyle" />
                <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                <SelectedRowStyle CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#487575" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#275353" />

            </asp:GridView>

            <hr />

            <br />

        </div>
    </form>
</body>
</html>
