<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IEPView.aspx.cs" Inherits="StudentBinder_IEPView" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <script src="../Administration/JS/jquery-1.8.0.min.js"></script>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <title></title>
    <script type="text/javascript">

        function showWait() {


            $('#btnExport').attr('disable', 'disable');
            //$('.loading').show();
            //$('#fullContents').hide();
        }
        function HideWait() {
            $('.loading').hide();
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
        function Prompt() {
            $('#overlay').show();
            $('.web_dialog2').show();
        }

        function HidePopup() {
            $('#overlay').hide();
            $('.web_dialog2').hide();
        }


        function checkPostbackExport() {

            showWait();
            return true;
        }

        function closePOP() {
            $('#SignPopups').fadeOut('slow', function () {
                $('#overlay').fadeOut('fast');
            });

        }

        function listUsers() {
            $('#overlay').fadeIn('slow', function () {
                $('#SignPopups').fadeIn();
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
        body {
        }

        .grmb {
            background: url("images/lbtngrngray1.PNG") no-repeat scroll 0 0 rgba(0, 0, 0, 0);
            height: 22px;
            padding-top: 8px;
            padding-left: 20px;
            text-decoration: none;
            width: 210px;
        }

        .table {
            width: 80%;
            border: 1px;
        }

        .border {
            border-top: 1px solid gray;
        }

        .borderBox {
            border-style: solid;
            border-width: 0;
            padding: 1px 4px;
            background-color: #FFFFFF;
        }

        .alignLeft {
            text-align: left;
        }

        .alignCenterWithBox {
            border-style: solid;
            border-width: 1px;
            padding: 1px 4px;
            text-align: center;
        }

        .fontBoldSmall {
            font-size: small;
            font-weight: bold;
            border-left-color: #A0A0A0;
            border-right-color: #C0C0C0;
            border-top-color: #A0A0A0;
            border-bottom-color: #C0C0C0;
            padding: 1px;
        }

        .fontBoldMedium {
            font-size: 13px;
            font-weight: bold;
            color: gray;
        }

        .header {
            font-size: x-large;
            font-weight: 700;
        }

        .blueFont {
            text-align: center;
            font-size: 19px;
            font-weight: 700;
        }

        .style1 {
            width: 9px;
        }

        .border {
        }

        .borderLeft {
            border-left: thin #CCC solid;
        }
        /* FOR LOADING IMAGE AT IEP EXPORT */
        .loading {
            display: none;
            position: absolute;
            width: 100%;
            height: 800px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            background-image: url("../images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
        }

        #fullContents {
            display: block;
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

        .FreeTextDivContent {
            width: 96%;
            min-height: 100px;
            height: auto;
            padding: 2%;
            border: 1px solid rgb(228, 228, 228);
            border-radius: 8px 8px 8px 8px;
        }

            .FreeTextDivContent ul li {
                list-style: disc outside none !important;
                margin: 0;
                padding: 0;
                text-align: left;
            }

            .FreeTextDivContent ol li {
                list-style: decimal outside none !important;
                margin: 0;
                padding: 0;
                text-align: left;
            }

        ol, ul {
            padding-left: 20px;
            width: 100%;
        }


        ul {
            list-style: disc;
        }

        a.tpContant:link, a.tpContant:visited, a.tpContant:visited {
            border: 1px solid;
            width: 50px;
            margin: 2px;
            float: left;
        }

        .tpselected {
            border: 1px solid;
            width: 50px;
            margin: 2px;
            float: left;
            background-color: #D2D2D2;
        }

        .AltRowStyle td {
            border-bottom: 1px solid #e9e9e9;
            color: #666;
            font-family: Arial;
            font-size: 12px;
            height: 30px;
            padding: 0 8px;
            text-align: left;
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

        /*LOADING IMAGE CLOSE */
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div id="test" runat="server"></div>
        <div id="loadDiv" class="loading" runat="server">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
        <div id="fullContents" runat="server">
            <div>
                <div style="width: 100%">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <table style="width: 100%">
                        <tr style="height: 40px">
                            <td>
                                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="NFButton" OnClick="btnExport_Click" OnClientClick="return checkPostbackExport();" Style="float: right; margin-left: 6px;" />
                                <asp:Button ID="btnSign" runat="server" Text="Sign" CssClass="NFButton" Style="float: right; margin-left: 6px;" OnClick="btnSign_Click" />
                                <asp:Button ID="btnSignDetails" runat="server" Text="Sign Details" CssClass="NFButton" Style="float: right; margin-left: 6px;" OnClick="btnSignDetails_Click" />
                                <asp:Button ID="btnBSP" runat="server" Text="BSP Forms" CssClass="NFButton" OnClick="btnBSP_Click" Style="float:right;" Visible="false" />
                            </td>
                            <td style="width: 1%">
                                <asp:ImageButton ID="btnRefresh" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" Style="float: right" />
                            </td>

                        </tr>
                    </table>
                </div>
                <center>

 
            
             <div id="iepsidemenu" style="text-align: center; width: 99px; margin-left: 6px; position: fixed;">

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

            </div>
            <div id="content" style="height:auto;border:1px solid #000;width:90%;margin-top:.2%;padding-top:.2%;">

   
       
        <a id="C1"></a>
                 <div id="iep1" tabindex="-1">    
        <table class="table" style="text-align:center;width:100%;">
            <tr>
                <td class="alignLeft"><span class="fontBoldSmall">School District Name:</span>
                    <asp:Label ID="lblSclDistName" runat="server" Text=""></asp:Label>
                 
                </td>
                
            </tr>
            <tr>
                <td class="alignLeft"><span class="fontBoldSmall">School District Address:</span>
                    <asp:Label ID="lblSclDistAdd" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="alignLeft"><span class="fontBoldSmall">School District Contact Person/Phone #:</span>
                    <asp:Label ID="lblSclDistContPerson" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="border" style="text-align:center"><span class="header" >Individualized Education Program</span></td>
            </tr>
            <tr>
                <td>IEP Dates:From:
                    <asp:Label ID="lblDateFrom1" runat="server" Text=""></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; to:
                    <asp:Label ID="lblDateTo1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="border">Student Name:<asp:Label ID="lblStName1" runat="server" Text=""></asp:Label>
                &nbsp;&nbsp; DOB:<asp:Label ID="lblDOB1" runat="server" Text=""></asp:Label>
                &nbsp;&nbsp; ID#:<asp:Label ID="lblID1" runat="server" Text=""></asp:Label>
                &nbsp;&nbsp; Grade/Level:<asp:Label ID="lblGrade1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="border"><span class="fontBoldMedium"> Parent and/or Student Concerns</span></td>
            </tr>
            <tr>
                <td>What concern(s) does the parent and/or student want to see addressed in this IEP to enhance the student&#39;s education?</td>
            </tr>
            <tr>
                <td style="text-align:left"><asp:Label ID="lblParConcerns1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="border"><span class="fontBoldMedium">Student Strengths and Key Evaluation Results Summary</span></td>
            </tr>
            <tr>
                <td>What are the student’s educational strengths, interest areas, significant personal attributes and personal accomplishments?
                    <br />
                    What is the student’s type of disability(ies), general education performance
                    <br />
                    including MCAS/district test results, achievement towards goals and lack of expected progress, if any? </td>
            </tr>
            <tr>
                <td style="text-align:left"><asp:Label ID="lblStStrength1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="border"><span class="fontBoldMedium">Vision Statement: What is the vision for this student?</span></td>
            </tr>
            <tr>
                <td>Consider the next 1 to 5 year period when developing this statement. Beginning no later than age 14,
                    <br />
                    the statement should be based on the student’s preferences and interest,
                    <br />
                    and should include desired outcomes in adult living, post-secondary and working environments. </td>
            </tr>
            <tr>
                <td style="text-align:left"><asp:Label ID="lblVision1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            </table>    
    </div>
 
            <br /><br />
<a id="C2"></a>
            <div id="iep2">

                 <table class="table" style="text-align:left;width:100%;">
                    <tr>
                        <td class="header" style="text-align:left">&nbsp;</td>
                        <td style="text-align:right">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border" style="text-align:center"><span class="header">Present Levels of Educational Performance</span></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border" style="text-align:center"><span class="blueFont">A: General Curriculum</span></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border"><span class="fontBoldMedium">Check all that apply.</span></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td class="fontBoldMedium">General curriculum area(s) affected by this student’s disability(ies):</td>
                    </tr>
                    <tr>
                        <td>
                            <input runat="server" id="chkEngLangArts2" type="checkbox" />English Language Arts</td>
                        <td>Consider the language, composition, literature (including reading) and media strands.</td>
                    </tr>
                    <tr>
                        <td>
                            <input runat="server" id="chkHistAndSS" type="checkbox" />History and Social Sciences</td>
                        <td>Consider the history, geography, economic and civics and government strands.</td>
                    </tr>
                    <tr>
                        <td>
                            <input runat="server" id="chkScAndTech2" type="checkbox" />Science and Technology</td>
                        <td>Consider the inquiry, domains of science, technology and science, technology and human affairs<br />
                            strand.</td>
                    </tr>
                    <tr>
                        <td>
                            <input runat="server" id="chkMaths2" type="checkbox" />Mathematics</td>
                        <td>Consider the number sense, patterns, relations and functions, geometry and measurement and statistics 
                            <br />
                            and probability strands.</td>
                    </tr>
                   
                    <tr>
                        <td>
                            <input runat="server" id="chkOtherCurr2" type="checkbox" />Other Curriculum Areas</td>
                        <td>Specify: <asp:Label ID="lblSpec" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                   
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border">How does the disability(ies) affect progress in the curriculum area(s)?</td>
                    </tr>
                    <tr>
                        <td colspan="2"> <asp:Label ID="lblDisabilities2" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border">What type(s) of accommodation, if any, is necessary for the student to make effective progress?</td>
                    </tr>
                    <tr>
                        <td colspan="2"> <asp:Label ID="lblAccomodation2" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border">What type(s) of specially designed instruction, if any, is necessary for the student to make effective progress?</td>
                    </tr>
                    <tr>
                        <td colspan="2">Check the necessary instructional modification(s) and describe how such modification(s) will be made.</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input runat="server" id="chkContent2" type="checkbox" />Content: <asp:Label ID="lblContent2" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input runat="server" id="chkMethodology2" type="checkbox" />Methodology/Delivery of Instruction: <asp:Label ID="lblMethodology2" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input runat="server" id="chkPerformance2" type="checkbox" />Performance Criteria: <asp:Label ID="lblPerformance2" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
   
            <br /><br />
<a id="C3"></a>
            <div id="iep3">
                  <table class="table" style="text-align:left;width:100%;">
                    <tr>
                        <td class="header" style="text-align:left" colspan="2">&nbsp;</td>
                        <td style="text-align:right">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3" class="border" style="text-align:center"><span class="header">Present Levels of Educational Performance</span></td>
                    </tr>
                    <tr>
                        <td colspan="3" class="border" style="text-align:center"><span class="blueFont">B.Other Educational Needs</span></td>                        
                    </tr>
                     <tr>
                        <td colspan="3" class="border"><span class="fontBoldMedium">Check all that apply.</span></td>
                    </tr>
                     <tr>
                        <td colspan="3" style="text-align:center">General Considerations</td>
                    </tr>
                     <tr>
                        <td>
                            <input runat="server" id="chkAdPhyEdu3" type="checkbox" />Adapted physical education</td>
                        <td colspan="1">
                            <input runat="server" id="chkAssTechDevice3" type="checkbox" />Assistive tech devices/services</td>
                        <td>
                            <input runat="server" id="chkBehavior3" type="checkbox" />Behavior</td>
                    </tr>
                     <tr>
                        <td>
                            <input runat="server" id="chkBrailleNeeds3" type="checkbox" />Braille needs (blind/visually impaired)</td>
                        <td colspan="1">
                            <input runat="server" id="chkCommunicationAll3" type="checkbox" />Communication (all students)</td>
                        <td>
                            <input runat="server" id="chkCommunicationDeaf3" type="checkbox" />Communication (deaf/hard of hearing students)</td>
                    </tr>
                     <tr>
                        <td>
                            <input runat="server" id="chkExtraCurrAct3" type="checkbox" />Extra curriculum activities</td>
                        <td colspan="1">
                            <input runat="server" id="chkLangNeeds3" type="checkbox" />Language needs (LEP students)</td>
                        <td>
                            <input runat="server" id="chkNonAcadActivities3" type="checkbox" />Nonacademic activities</td>
                    </tr>
                     <tr>
                        <td>
                            <input runat="server" id="chkSocial3" type="checkbox" />Social/emotional needs</td>
                        <td colspan="1">
                            <input runat="server" id="chkTravelTraining3" type="checkbox" />Travel training</td>
                        <td>
                            <input runat="server" id="chkVocatEducation3" type="checkbox" />Vocational education</td>
                    </tr>
                     <tr>
                        <td colspan="3">
                            <input runat="server" id="chkOther3" type="checkbox" />Other: <asp:Label ID="lblOther3" runat="server" Text=""></asp:Label>
                         </td>
                    </tr>
                     <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                     <tr>
                        <td colspan="3">
                            <input runat="server" id="chkChild3to5" type="checkbox" />For children ages 3 to 5 — participation in appropriate activities</td>
                    </tr>
                     <tr>
                        <td colspan="3">
                            <input runat="server" id="chkChild14" type="checkbox" />For children ages 14+ (or younger if appropriate) — student’s course of study</td>
                    </tr>
                     <tr>
                        <td colspan="3">
                            <input runat="server" id="chkChild16" type="checkbox" />For children ages 16 (or younger if appropriate) to 22 — transition to post-school activities including community experiences, employment
                            <br />
&nbsp;&nbsp;&nbsp;&nbsp; objectives, other post&nbsp;&nbsp; school adult living and, if appropriate, daily living skills</td>
                    </tr>
                     <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                     <tr>
                        <td colspan="3" class="border">How does the disability(ies) affect progress in the indicated area(s) of other educational needs?</td>
                    </tr>
                     <tr>
                        <td colspan="3"> <asp:Label ID="lblDisabilities3" runat="server" Text=""></asp:Label>
                         </td>
                    </tr>
                     <tr>
                        <td colspan="3" class="border">What type(s) of accommodation, if any, is necessary for the student to make effective progress?</td>
                    </tr>
                     <tr>
                        <td colspan="3"> <asp:Label ID="lblAccomodation3" runat="server" Text=""></asp:Label>
                         </td>
                    </tr>
                     <tr>
                        <td colspan="3" class="border">What type(s) of specially designed instruction, if any, is necessary for the student to make effective progress?<br />
                            Check the necessary instructional modification(s) and describe how such modification(s) will be made.</td>
                    </tr>
                     <tr>
                        <td colspan="3">
                            <input runat="server" id="chkContent3" type="checkbox" />Content: <asp:Label ID="lblContent3" runat="server" Text=""></asp:Label>
                         </td>
                    </tr>
                     <tr>
                        <td colspan="3">
                            <input runat="server" id="chkMethodology3" type="checkbox" />Methodology/Delivery of Instruction: <asp:Label ID="lblMethodology3" runat="server" Text=""></asp:Label>
                         </td>
                    </tr>
                     <tr>
                        <td colspan="3">
                            <input runat="server" id="chkPerformance3" type="checkbox" />Performance Criteria: <asp:Label ID="lblPerformance3" runat="server" Text=""></asp:Label>
                         </td>
                    </tr>
                     </table>
            </div>
<asp:HiddenField ID="GoalId" Value="" runat="server" />
            <br /><br />
 <a id="C4"></a>
            <div id="iep4">
                 <table class="table" style="text-align:center;width:100%;">
                    <tr>
                        <td class="header" style="text-align:left">&nbsp;</td>
                        <td style="text-align:right">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border" style="text-align:center"><span class="header">Current Performance Levels/Measurable Annual Goals</span></td>
                    </tr>
                    <tr>
                        <td>
                <asp:Repeater ID="RepPage4" runat="server" OnItemDataBound="RepPage4_ItemDataBound"> 
            <ItemTemplate>
                <table id="tableRepeat" class="table" style="width:100%;">
                                    

                <tr style="border-style: solid;">
                        <td runat="server" id="divGoalNo" class="borderBox" style="width:12%; text-align:left; max-width:87px;">
                            Goal#: <%#DataBinder.Eval(Container,"DataItem.GoalId")%>
                        </td>
                        <td class="borderBox" colspan="3"  style="table-layout:fixed; max-width:70%">
                            Specific Goal Focus: <%#DataBinder.Eval(Container,"DataItem.Title")%>
                        </td>
                   <tr>
                       <td style="text-align:left" colspan="4"><%#Eval("GoalIEPNote") %></td>
                   </tr>
                    <tr>
                      <td class="border" style="text-align:left" colspan="4"><strong>Current Performance Level:</strong> What can the student currently do?</td>
                    </tr>
                    <asp:HiddenField ID="hfGoalId" runat="server" Value='<%#Eval("GoalId") %>' />
                    <asp:HiddenField ID="hfLessonPlanId" runat="server" Value='<%# Eval("LessonPlanId") %>' />
                    </tr>
                    
                    
                    <tr style="border-style: solid;">
                        <td style="text-align:left;" colspan="4">
                            <asp:Repeater ID="RepPage4Inside" runat="server" OnItemDataBound="RepPage4Inside_ItemDataBound">
                                <ItemTemplate>
                                    <table id="tableRepeat4Inside" class="table" style="width:100%;">
                                        <tr>
                                            <td class="border" style="text-align:left"><strong>
                                                <%#DataBinder.Eval(Container,"DataItem.LessonPlanName")%>: </strong>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"><%#DataBinder.Eval(Container,"DataItem.Objective1")%></td>
                                            <asp:HiddenField ID="hfStdtLessonPlanId" runat="server" Value='<%#Eval("StdtLessonPlanId") %>' />
                                            <asp:HiddenField ID="hfGoalId2" runat="server" Value='<%#Eval("GoalId") %>' />
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                        
                    </tr>

                    <tr>
                        <td colspan="4" class="border" style="text-align:left"><strong>Measurable Annual Goal:</strong> What challenging, yet attainable, goal can we expect the student to meet by the end of this IEP period? How will we know that the student has reached this goal? </td>
                    </tr>
                    <tr>
                        <td colspan="4"  class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"><%#DataBinder.Eval(Container,"DataItem.Objective2")%></td>
                    </tr>
                    <tr>
                        <td colspan="4" class="border" style="text-align:left"><strong>Benchmark/Objectives:</strong> What will the student need to do to complete this goal?</td>
                    </tr>
                    <tr>
                        <td colspan="4"  class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"><%#DataBinder.Eval(Container,"DataItem.Objective3")%></td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    </table>
                </ItemTemplate>
        </asp:Repeater>
                </td></tr></table>
            </div>
            <br /><br />
       <a id="C5"></a>
            <div id="iep5">
                 <table class="table" style="text-align:center;width:100%;">
                    <tr>
                        <td class="header" style="text-align:left" colspan="4">&nbsp;</td>
                        <td style="text-align:right" colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="6" class="border" style="text-align:center"><span class="header">Service Delivery</span></td>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align:center">What are the total service delivery needs of this student?</td>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align:center">Include services, related services, program modifications and supports (including positive behavioral supports, school personnel and/or parent<br />
                            training/supports). Services should assist the student in reaching IEP goals, to be involved and progress in the general curriculum, to participate in extracurricular/nonacademic activities and to allow the student to participate with nondisabled students while working towards IEP goals.</td>
                    </tr>
                    <tr>
                        <td style="text-align:center">School District Cycle:</td>
                        <td style="text-align:center">
                            <input runat="server" id="chk5DayCycle5" type="checkbox" />5 day cycle</td>
                        <td style="text-align:center">
                            <input runat="server" id="chk6DayCycle5" type="checkbox" />6 day cycle</td>
                        <td style="text-align:center">
                            <input runat="server" id="chk10DayCycle5" type="checkbox" />10 day cycle</td>
                        <td style="text-align:center">
                            <input runat="server" id="chkOther5" type="checkbox" />Other: <asp:Label ID="lblOther5" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="text-align:center" class="style1">&nbsp;</td>
                    </tr>                    
                    </table>
                <div>
                    

                    <div style="border:1px solid gray">

    <asp:Repeater ID="Rep1" runat="server">
        <HeaderTemplate>
             <table id="tableRepeat" class="table" style="width:100%;">
                 <tr>
                        <td colspan="6" class="fontBoldMedium" style="text-align:center; color: #FFFFFF; background-color: #03507D;">A. Consultation (Indirect Services to School Personnel and Parents)</td>
                    </tr>
            <tr class="fontBoldMedium">
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;" >Focus on Goal #</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Type of Service</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Type of Personnel</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Frequency and Duration/Per Cycle</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Start Date</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">End Date</td>
                    </tr>
                 </table>
        </HeaderTemplate>
            <ItemTemplate>
        <table id="tableRepeat" class="table" style="width:100%;">        
                    <tr class="borderBox">
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.IEPGoalNo")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.SvcTypDesc")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.PersonalTypDesc")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.FreqDurDesc")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.StartDate")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.EndDate")%>
                        </td>
                    </tr>
            </table>
                </ItemTemplate>
        </asp:Repeater>

</div>

        </div>


        
                                <div style="border:1px solid gray">

    <asp:Repeater ID="Rep2" runat="server">
        <HeaderTemplate>
             <table id="tableRepeat" class="table" style="width:100%;">
                 <tr>
                        <td colspan="6" class="fontBoldMedium" style="text-align:center; color: #FFFFFF; background-color: #03507D;">B. Special Education and Related  Services in General Education Classroom (Direct Service)</td>
                    </tr>
            <tr class="fontBoldMedium">
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;" >Focus on Goal #</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Type of Service</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Type of Personnel</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Frequency and Duration/Per Cycle</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Start Date</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">End Date</td>
                    </tr>
                 </table>
        </HeaderTemplate>
            <ItemTemplate>
        <table id="tableRepeat" class="table" style="width:100%;">        
                    <tr class="borderBox">
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.IEPGoalNo")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.SvcTypDesc")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.PersonalTypDesc")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.FreqDurDesc")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.StartDate")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.EndDate")%>
                        </td>
                    </tr>
            </table>
                </ItemTemplate>
        </asp:Repeater>

        </div>


                            <div style="border:1px solid gray">

    <asp:Repeater ID="Rep3" runat="server">
        <HeaderTemplate>
             <table id="tableRepeat" class="table" style="width:100%;">
                 <tr>
                        <td colspan="6" class="fontBoldMedium" style="text-align:center; color: #FFFFFF; background-color: #03507D;">C. Special Education and Related Services in Other Settings (Direct Service)</td>
                    </tr>
            <tr class="fontBoldMedium">
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;" >Focus on Goal #</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Type of Service</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Type of Personnel</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Frequency and Duration/Per Cycle</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">Start Date</td>
                        <td class="borderBox" style="text-align:center;  min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;">End Date</td>
                    </tr>
                 </table>
        </HeaderTemplate>
            <ItemTemplate>
        <table id="tableRepeat" class="table"  style="width:100%;">        
                    <tr class="borderBox">
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.IEPGoalNo")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.SvcTypDesc")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.PersonalTypDesc")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.FreqDurDesc")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.StartDate")%>
                        </td>
                        <td class="borderBox" style="text-align:left; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.EndDate")%>
                        </td>
                    </tr>
            </table>
                </ItemTemplate>
        </asp:Repeater>

        </div>

            </div>
                   
            <br /><br />

          <a id="C6"></a>
            <div id="iep6">
 <table class="table" style="text-align:center;width:100%;">
                    <tr>
                        <td class="header" style="text-align:left" colspan="3">&nbsp;</td>
                        <td style="text-align:right" colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="5" class="border" style="text-align:center"><span class="header">Nonparticipation Justification</span></td>
                    </tr>
                    <tr>
                        <td colspan="5">Is the student removed from the general education classroom at any time? (Refer to IEP 5—Service Delivery, Section C.)</td>
                    </tr>

                    <tr>
                        <td colspan="5">
                            <input runat="server" id="chkRemoveNo6" type="checkbox" />No
                            <input runat="server" id="chkRemoveYes6" type="checkbox" />Yes&nbsp;&nbsp;&nbsp;&nbsp; If yes, why is removal considered critical to the student’s program?
</td>
                    </tr>

                    <tr>
                        <td colspan="5"><asp:Label ID="lblRemove6" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="5">&nbsp;</td>
                    </tr>

                    <tr>
                        <td colspan="5" class="borderBox" style="text-align:center">IDEA ’97 Regulation §300.550(b)(2): “... removal of children with disabilities from the regular educational environment occurs only if the<br />
                            nature or severity of the disability is such that education in regular classes with the use of supplementary aids and services cannot be<br />
                            achieved satisfactorily.” (Emphasis added.)</td>
                    </tr>

                    <tr>
                        <td colspan="5" >&nbsp;</td>
                    </tr>

                    <tr>
                        <td colspan="5" class="border" style="text-align:center"><span class="header">Schedule Modification</span></td>
                    </tr>

                    <tr>
                        <td colspan="5"><strong>Shorter: </strong>Does this student require a shorter school day or shorter school year?</td>
                    </tr>

                    <tr>
                        <td>
                            <input runat="server" id="chkShorterNo6" type="checkbox" />No</td>
                        <td>
                            <input runat="server" id="chkShorterYesDay6" type="checkbox" />Yes — shorter day</td>
                        <td colspan="2">
                            <input runat="server" id="chkShorterYesYear6" type="checkbox" />Yes — shorter year</td>
                        <td>If yes, answer the questions below.</td>
                    </tr>

                    <tr>
                        <td colspan="5" style="border-top-style: solid; border-top-width: 1px; padding: 1px 4px"><strong>Longer:</strong> Does this student require a longer school day or a longer school year to prevent substantial loss of previously learned skills and / or substantial difficulty in relearning skills?</td>
                    </tr>

                     <tr>
                        <td>
                            <input runat="server" id="chkLongerNo6" type="checkbox" />No</td>
                        <td>
                            <input runat="server" id="chkLongerYesShDay6" type="checkbox" />Yes — shorter day</td>
                        <td colspan="2">
                            <input runat="server" id="chkLongerYesShYear6" type="checkbox" />Yes — shorter year</td>
                        <td>If yes, answer the questions below.</td>
                    </tr>

                    <tr>
                        <td colspan="5" style="border-top-style: solid; border-top-width: 1px; padding: 1px 4px">How will the student’s schedule be modified? Why is this schedule modification being recommended?
If a longer day or year is recommended, how will the school district coordinate services across program components?
</td>
                    </tr>

                    <tr>
                        <td colspan="5"><asp:Label ID="lblSchedule6" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="5" style="text-align:center">&nbsp;</td>
                    </tr>

                   <tr>
                        <td colspan="5" class="border" style="text-align:center"><span class="header">Transportation Services</span></td>
                    </tr>

                    <tr>
                        <td colspan="5">Does the student require transportation as a result of the disability(ies)?</td>
                    </tr>

                    <tr>
                        <td colspan="5">
                            <input runat="server" id="chkTransportationNo6" type="checkbox" />No&nbsp;&nbsp;&nbsp;&nbsp; Regular transportation will be provided in the same manner as it would be provided for students without disabilities. If the child is placed away from the&nbsp;&nbsp;&nbsp;&nbsp;
                            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; local school, transportation will be provided.</td>
                    </tr>

                    <tr>
                        <td colspan="5">
                            <input runat="server" id="chkTransportationYes6" type="checkbox" />Yes&nbsp;&nbsp;&nbsp; Special transportation will be provided in the following manner:</td>
                    </tr>

                    <tr>
                        <td colspan="5">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input runat="server" id="chkTransportationYesRegular6" type="checkbox" />on a regular transportation vehicle with the following modifications and/or specialized equipment and precautions:</td>
                    </tr>

                    <tr>
                        <td colspan="5">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label ID="lblTransportationYesRegular6" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="5">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input runat="server" id="chkTransportationYesSpecial6" type="checkbox" />on a special transportation vehicle with the following modifications and/or specialized equipment and precautions:</td></td>
                    </tr>

                    <tr>
                        <td colspan="5">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label ID="lblTransportationYesSpecial6" runat="server" Text=""></asp:Label></td>
                    </tr>

                    <tr>
                        <td colspan="5" class="borderBox">After the team makes a transportation decision and after a placement decision has been made, a parent may choose to provide transportation and may be eligible for reimbursement under certain circumstances. Any parent who plans to transport their child to school should notify the school district contact person.</td>
                    </tr>

</table>
            </div>
                    
            <br /><br />
                <a id="C7"></a>
            <div id="iep7">
                 <table class="table" style="text-align:center;width:100%;">
                    <tr>
                        <td class="header" style="text-align:left" colspan="4">&nbsp;</td>
                        <td style="text-align:right" colspan="3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="7" class="border" style="text-align:center"><span class="header">State or District-Wide Assessment</span></td>
                    </tr>
                    <tr>
                        <td colspan="7">Identify state or district-wide assessments planned during this IEP period:</td>
                    </tr>
                    <tr>
                        <td colspan="7"> <asp:Label ID="lblAsmntPlanned7" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px">Fill out the table below. Consider any state or district-wide assessment to be administered during the time span covered by this IEP. For each content area, identify the student’s assessment participation status by putting an “X” in the corresponding box for column 1,2, or 3.</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="2">1. Assessment participation: 
                            <br />
                            Student participates in 
                            <br />
                            on-demand testing under routine<br />
&nbsp;conditions in this content area
</td>
                        <td colspan="3">2. Assessment participation:<br />
                            Student participates in<br />
                            on-demand testing with<br />
                            accommodations in this content<br />
&nbsp;area. (See <strong>(i)</strong> below)
</td>
                        <td>3. Assessment participation:<br />
                            Student participates in alternate<br />
                            assessment in this content area.<br />
                            (See <strong>(ii)</strong> below)</td>
                    </tr>
                    <tr class="fontBoldMedium" style="text-align:center; color: #FFFFFF; background-color: #03507D;">
                        <td>CONTENT AREAS</td>
                        <td colspan="2">COLUMN 1</td>
                        <td colspan="3">COLUMN 2</td>
                        <td>COLUMN 3</td>
                    </tr>
                    <tr>
                        <td class="borderBox">English Language Arts</td>
                        <td colspan="2" class="alignCenterWithBox">
                            <input runat="server" id="chkEngLangArt1" type="checkbox" /></td>
                        <td colspan="3" class="alignCenterWithBox">
                            <input runat="server" id="chkEngLangArt2" type="checkbox" /></td>
                        <td class="alignCenterWithBox">
                            <input runat="server" id="chkEngLangArt3" type="checkbox" /></td>
                    </tr>
                    <tr>
                        <td class="borderBox">History and Social Sciences</td>
                        <td colspan="2" class="alignCenterWithBox">
                            <input runat="server" id="chkHistAndSS1" type="checkbox" /></td>
                        <td colspan="3" class="alignCenterWithBox">
                            <input runat="server" id="chkHistAndSS2" type="checkbox" /></td>
                        <td class="alignCenterWithBox">
                            <input runat="server" id="chkHistAndSS3" type="checkbox" /></td>
                    </tr>
                    <tr>
                        <td class="borderBox">Mathematics</td>
                        <td colspan="2" class="alignCenterWithBox">
                            <input runat="server" id="chkMathematics1" type="checkbox" /></td>
                        <td colspan="3" class="alignCenterWithBox">
                            <input runat="server" id="chkMathematics2" type="checkbox" /></td>
                        <td class="alignCenterWithBox">
                            <input runat="server" id="chkMathematics3" type="checkbox" /></td>
                    </tr>
                    <tr>
                        <td class="borderBox">Science and Technology</td>
                        <td colspan="2" class="alignCenterWithBox">
                            <input runat="server" id="chkScienceAndTech1" type="checkbox" /></td>
                        <td colspan="3" class="alignCenterWithBox">
                            <input runat="server" id="chkScienceAndTech2" type="checkbox" /></td>
                        <td class="alignCenterWithBox">
                            <input runat="server" id="chkScienceAndTech3" type="checkbox" /></td>
                    </tr>
                    <tr>
                        <td class="borderBox">Reading</td>
                        <td colspan="2" class="alignCenterWithBox">
                            <input runat="server" id="chkReading1" type="checkbox" /></td>
                        <td colspan="3" class="alignCenterWithBox">
                            <input runat="server" id="chkReading2" type="checkbox" /></td>
                        <td class="alignCenterWithBox">
                            <input runat="server" id="chkReading3" type="checkbox" /></td>
                    </tr>
                    <tr>
                        <td colspan="7" style="border-top-style: solid; border-top-width: 1px; padding: 1px 4px"><strong>(i)</strong> For each content area identified by an X in the column 2 above: note in the space below, the content area and describe the accommodations necessary for participation in the on-demand testing. Any accommodations used for assessment purposes should be closely modeled on the accommodations that are provided to the student as part of his/her instructional program. </td>
                    </tr>
                    <tr>
                        <td colspan="7"> <asp:Label ID="lblInfoi" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="7" style="border-top-style: solid; border-top-width: 1px; padding: 1px 4px"><strong>(ii)</strong> For each content area identified by an X in column 3 above: note in the space below, the content area, why the on-demand assessment is not appropriate and how that content area will be alternately assessed. Make sure to include the learning standards that will be addressed in each content area, the recommended assessment method(s) and the recommended evaluation and reporting method(s) for the student’s performance on the alternate assessment.</td>
                    </tr>
                    <tr>
                        <td colspan="7"> <asp:Label ID="lblInfoii" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;</td>
                        <td class="fontBoldMedium" style="text-align:center; color: #FFFFFF; background-color: #03507D;">NOTE</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="text-align:center; border-left-style: solid; border-right-style: solid; border-bottom-style: solid;">When state model(s)<br />
                            for alternate 
                            <br />
                            assessment are 
                            <br />
                            adopted, the district 
                            <br />
                            may enter use of state 
                            <br />
                            model(s) for how 
                            <br />
                            content area(s) will 
                            <br />
                            be assessed.</td>
                    </tr>
                    </table>
            </div>
                    
            <br /><br />
                <a id="C8"></a>
            <div id="iep8">
                 <table class="table" style="text-align:center;width:100%;">
                    <tr>
                        <td class="header" style="text-align:left">&nbsp;</td>
                        <td style="text-align:right">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border" style="text-align:center"><span class="header">Additional Information</span></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input runat="server" id="chkAddInfo1" type="checkbox" />Include the following transition information: the anticipated graduation date; a statement of interagency responsibilities or 
                            <br />
&nbsp;&nbsp;&nbsp;&nbsp; needed linkages; the discussion of transfer of rights at least one year before age of majority; and a recommendation for 
                            <br />
&nbsp;&nbsp;&nbsp;&nbsp; Chapter 688 Referral.</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input runat="server" id="chkAddInfo2" type="checkbox" />Document efforts to obtain participation if a parent and if student did not attend meeting or provide input.</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input runat="server" id="chkAddInfo3" type="checkbox" />Record other relevant IEP information not previously stated.</td>
                    </tr>
                    <tr>
                        <td colspan="2"> <asp:Label ID="lblAddInfo8" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    </table>
            </div>





                <br /><br />
                 <a id="C9"></a>
                <div id="iep9">
                    <table class="table" style="text-align:left;width:100%;">
                        <tr>
                        <td class="header" style="text-align:left">&nbsp;</td>
                        <td style="text-align:right">&nbsp;</td>
                    </tr>

                        <tr>
                    <td colspan="2" class="border" style="text-align:center"><span class="header">Placement Consent Form - PL1: 3-5 year olds</span>
                    </td>

                </tr>
                        <tr>
                        <td colspan="7" style="text-align:center;">Use either section 1, 2 or 3 as appropriate to the child’s educational placement.</td>
                    </tr>

                        <tr>
                        <td colspan="7" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">1.The child attends an inclusive early childhood program that includes children with and without disabilities.</td>
                    </tr>

                        <tr>
                             <td style="text-align:left;">The child attends an early childhood program and special education services are provided:</td>
                            <td style="text-align:left;">
                        <asp:CheckBox ID="chkEarlyChild" runat="server" Text=" " CssClass="" />In the early childhood program<br />
                        <asp:CheckBox ID="chkSeparate" runat="server" Text=" " />Separate from the early childhood program<br />
                        <asp:CheckBox ID="chkBoth" runat="server" Text=" " />Both in and out of the early childhood program<br />
                        <br />
                    </td>
                        </tr>

                        <tr>
                            <td style="text-align:left;">Hours per week in the early childhood program
                                <asp:Label ID="lblStateOrDistrict" runat="server"></asp:Label>
                    </td>
                    <td style="text-align:left;">
                        <br />
                        <asp:CheckBox ID="chkEnrolled" runat="server" Text=" " />Enrolled by the parent<br />
                        <asp:CheckBox ID="chkPlaced" runat="server" Text=" " />Placed by the Team<br />
                        <br />
                    </td>

                        </tr>
                        <tr>
                            <td style="text-align:left;">All together the child will be participating in an inclusive environment (taking into account the early childhood program and special education services):</td>
                    <td style="text-align:left;">
                        <br />
                        <asp:CheckBox ID="chkTimeMore" runat="server" Text=" " />80% of the time or more<br />
                        <asp:CheckBox ID="chkOfTime" runat="server" Text=" " />40 – 79% of the time<br />
                        <asp:CheckBox ID="chk39Time" runat="server" Text=" " />0 – 39% of the time<br />
                        <br />
                    </td>
                        </tr>
                        <tr>
                            <td colspan="7" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">2. The child does not attend an inclusive early childhood program.</td>
                        </tr>
                        <tr>
                            <td style="text-align:left;">
                        <br />
                        The Team identified that the child should attend a special education class that only serves children with disabilities.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkSubstancialy" runat="server" Text=" " />Substantially Separate Class<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;">
                        <br />
                        The Team identified that the child should attend a full-day special education program in a public or private separate day school that only serves children with disabilities.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <br />
                        <asp:CheckBox ID="chkDaySchool" runat="server" Text=" " />Separate Day School<br />
                        <span style="margin-left: 25px;">
                            <asp:CheckBox ID="chkPublic" runat="server" Text=" " />Public or</span>
                        <asp:CheckBox ID="chkPrivate" runat="server" Text=" " />Private<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;">
                        <br />
                        The Team identified that the child should attend a special education program in a residential facility that only serves children with disabilities.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <br />
                        <asp:CheckBox ID="chkResidential" runat="server" Text=" " />Residential Facility<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;">
                        <br />
                        The Team identified IEP services to be provided in a program in the home for a child who is 3 to 5 years of age.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <br />
                        <asp:CheckBox ID="chkHome" runat="server" Text=" " />Home<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;">
                        <br />
                        The Team identified IEP services to be provided outside the home in a clinicians office, school office, hospital facility, or other community location.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkService" runat="server" Text=" " />Service Provider Location<br />
                        <br />
                    </td>

                        </tr>
                        <tr>
                            <td colspan="7" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">3. Other Authority Required Placements</td>
                        </tr>
                        <tr>
                            <td colspan="7">Note: These non-educational placements are not determined by the Team and therefore service delivery may be limited.</td>

                        </tr>

                        <tr>
                            <td style="text-align:left;">The placement has been made by a state agency to an institutionalized setting for non-educational reasons.</td>
                    <td style="text-align:left;">
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
                        <tr>
                    <td style="text-align:left;">
                        <br />
                        A doctor has determined that the child must be served in a home setting.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <br />
                        <asp:CheckBox ID="chkHmeBasedPgm" runat="server" Text=" " />Home-based Program<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;">
                        <br />
                        A doctor has determined that the child must be served in a hospital setting.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkHospital" runat="server" Text=" " />Hospital-based Program<br />
                        <br />
                    </td>
                </tr>
                        <tr>
                            <td colspan="2" class="border" style="text-align:center"><span class="header">Placement Consent Form</span></td>
                        </tr>

                        <tr>
                            <td colspan="7">Location(s) for Service Provision and Dates:<asp:Label ID="lblLocationService" runat="server"></asp:Label></td>

                        </tr>
                        <tr>
                            <td colspan="7" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px">Parent Options / Responses</td>
                        </tr>
                        <tr>
                            <td colspan="7">It is important that the district knows your decision as soon as possible. Please indicate your response by checking at least one (1) box and returning a signed copy to the district along with your response to the IEP. Thank you.</td>

                        </tr>

                        <tr>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkConsent2" runat="server" Text=" " />
                        I consent to the placement.</td>
                </tr>
                        <tr>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkRefuse2" runat="server" Text=" " />
                        I refuse the placement.</td>
                </tr>
                        <tr>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkPlacement2" runat="server" Text=" " />I request a meeting to discuss the refused placement. </td>
                </tr>
                        <tr>
                    <td>Signature of Parent, Guardian, Educational Surrogate Parent, Student 18 and Over<span>*</span>
                        *Required signature once a student reaches 18 unless there is a court appointed guardian.
                        <asp:Label ID="lblSignParentTwo" runat="server"></asp:Label>
                    </td>
                            <td style="float:right;">
                        Date
                                <asp:Label ID="lblDateTwo" runat="server"></asp:Label>
                    </td>
                </tr>
                        <tr>
                    <td colspan="2" class="border" style="text-align:center"><span class="header">Placement Consent Form – PL1: 6-21 year olds</span>
                    </td>

                </tr>
                        <tr>
                            <td colspan="7" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px; float:left;">Team Recommended Educational Placements</td>
                            <td style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;">Corresponding Placement</td>
                        </tr>
                        <tr>
                            <td style="text-align:left;">
                        <br />
                        The Team identified that IEP services are provided outside the general education classroom less than 21% of the time (80% inclusion).<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkFullPgm" runat="server" Text=" " />Full Inclusion Program<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;">
                        <br />
                        The Team identified that IEP services are provided outside the general education classroom at least 21% of the time, but no more than 60% of the time.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkPartialPgm" runat="server" Text=" " />Partial Inclusion Program<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;">
                        <br />
                        The Team identified that IEP services are provided outside the general education classroom for more than 60% of the time.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkSepClass" runat="server" Text=" " />Substantially Separate Classroom<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;">
                        <br />
                        The Team identified that all IEP services should be provided outside the general education classroom and in a public or private separate school that only serves students with disabilities.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <br /><asp:CheckBox ID="chkSepDaySchool" runat="server" Text=" " />Separate Day School
                        <br />
                        <span style="margin-left: 15px;">
                            <asp:CheckBox ID="chkPublicSep" runat="server" Text=" " />Public or</span>
                        <asp:CheckBox ID="chkPrivateSep" runat="server" Text=" " />Private<br />
                        <br />

                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;">
                        <br />
                        The Team identified that IEP services require a 24-hour special education program.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <br /><asp:CheckBox ID="chkResSchool" runat="server" Text=" " />Residential School<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;">
                        <br />
                        The Team has identified a mix of IEP services that are not provided in primarily school-based settings but are in a neutral or community-based setting.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkOtherSep" runat="server" Text=" " />Other:<asp:Label ID="lblOtherDesc" runat="server"></asp:Label><br />
                        <br />
                    </td>

                        </tr>

                        <tr>
                            <td colspan="7" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">Other Authority Required Placements</td>
                        </tr>
                        <tr>
                        <td colspan="7">Note: These non-educational placements are not determined by the Team and therefore service delivery may be limited.</td>
                    </tr>

                        <tr>
                    <td>The placement has been made by a state agency to an institutionalized setting for non-educational reasons.</td>
                    <td style="text-align:left;">
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
                <tr>
                    <td>
                        <br />
                        A doctor has determined that the student must be served in a home setting.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <br /><asp:CheckBox ID="chkHomeDoctor" runat="server" Text=" " />Home-based Program<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        A doctor has determined that the student must be served in a hospital setting.<br />
                        <br />
                    </td>
                    <td style="text-align:left;">
                        <br /><asp:CheckBox ID="chkHospitalDoctor" runat="server" Text=" " />Hospital-based Program<br />
                        <br />
                    </td>
                </tr>

                        <tr>

                            <td colspan="2" class="border" style="text-align:center"><span class="header">Placement Consent Form</span></td>
                        </tr>

                        <tr>
                            <td colspan="7">Location(s) for Service Provision and Dates:<asp:Label ID="lblLocationService2" runat="server"></asp:Label></td>

                        </tr>

                        <tr>
                            <td colspan="7" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">Parent Options / Responses</td>
                        </tr>
                        <tr>
                            <td colspan="7">It is important that the district knows your decision as soon as possible. Please indicate your response by checking at least one (1) box and returning a signed copy to the district along with your response to the IEP. Thank you.</td>

                        </tr>

                        <tr>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkConcent" runat="server" Text=" " />
                        I consent to the placement.</td>
                </tr>
                        <tr>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkPlacement" runat="server" Text=" " />
                        I refuse the placement.</td>
                </tr>
                        <tr>
                    <td style="text-align:left;">
                        <asp:CheckBox ID="chkRefused" runat="server" Text=" " />I request a meeting to discuss the refused placement. </td>
                </tr>
                        <tr>
                    <td>Signature of Parent, Guardian, Educational Surrogate Parent, Student 18 and Over<span>*</span>
                        *Required signature once a student reaches 18 unless there is a court appointed guardian.
                        <asp:Label ID="lblSignParentOne" runat="server"></asp:Label>
                    </td>
                            <td style="float:right;">
                        Date
                                <asp:Label ID="lblDateOne" runat="server"></asp:Label>
                    </td>
                </tr>
                        </table>
               </div>

                <a id="C10"></a>
                <div id="iep10">

                    <table class="table" style="text-align:left;width:100%;">

                        <tr>
                        <td colspan="11" class="border" style="text-align:center"><span class="header">Administrative Data Sheet</span></td>
                    </tr>
                        <tr>
                            <td colspan="11" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">STUDENT INFORMATION</td>
                        </tr>

                        <tr>
                            <td style="text-align:left;">Full Name</td>
                            <td style="text-align:left;"><asp:Label ID="lblStudentNameIep10" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left; width:500px">School ID#:</td>
                            <td><asp:Label ID="lblStudentSchIep10" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left; width:100px">SASID:</td>
                            <td><asp:Label ID="lblsasidIep10" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="text-align:left;">Birth Date :</td>
                            <td><asp:Label ID="lblBirthDate" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Place of Birth:</td>
                            <td><asp:Label ID="lblPlace" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Age:</td>
                            <td><asp:Label ID="lblAge" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Grade/Level:</td>
                            <td><asp:Label ID="lblGrade" runat="server"></asp:Label></td>
                        </tr>

                        <tr>
                            <td style="text-align:left;">Primary Language:</td>
                            <td><asp:Label ID="lblPrLanguage" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Language of Instruction:</td>
                            <td><asp:Label ID="lblInstruction" runat="server"></asp:Label></td>
                        </tr>

                        <tr>
                            <td style="text-align:left;">Address:</td>
                            <td><asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Sex:</td>
                            <td><asp:Label ID="lblSex" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="text-align:left;">Home Telephone:</td>
                            <asp:Label ID="lblPhoneHome" runat="server"></asp:Label>
                        </tr>

                        <tr>
                            <td style="text-align:left;">If 18 or older:</td>
                              <td colspan="2" style="text-align:left;">  
                                  <asp:CheckBox ID="chkActingon" runat="server" Text=" " />Acting on Own Behalf
                            </td>
                            <td colspan="5"><asp:CheckBox ID="chkCourtAppointed" runat="server"  Text="Court Appointed Guardian:"
                             CssClass="checkBoxStyle" /></td>
                            <td>
                                <asp:Label ID="lblCourtAppointed" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td></td>
                            <td style="text-align:left;" colspan="2"><asp:CheckBox ID="chkDecisionMaking" runat="server" Text="Shared Decision-Making"
                             CssClass="checkBoxStyle" /></td>
                            <td colspan="5"><asp:CheckBox ID="ChkDelegateDecision" runat="server" Text="Delegate Decision-Making"
                             CssClass="checkBoxStyle" /></td>
                        </tr>
                        <tr>
                            <td colspan="11" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">PARENT/GUARDIAN INFORMATION:</td>
                        </tr>

                        <tr>
                            <td style="text-align:left;">Name</td>
                            <td><asp:Label ID="lblName" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Relationship to Student:</td>
                            <td><asp:Label ID="lblRelationship" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Address:</td>
                            <td><asp:Label ID="lblstudAddress" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Home Telephone:</td>
                            <td><asp:Label ID="lblstudPhoneHome" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Other Telephone:</td>
                            <td><asp:Label ID="lblstudPhoneOther" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Primary Language of parent/guardian:</td>
                            <td><asp:Label ID="lblPrLanguageParent" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="11" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">PARENT/GUARDIAN INFORMATION:</td>
                        </tr>
                        <tr>
                            <td style="text-align:left;">Name</td>
                            <td><asp:Label ID="lblName2" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Relationship to Student:</td>
                            <td><asp:Label ID="lblRelationship2" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Address:</td>
                            <td><asp:Label ID="lblstudAddress2" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Home Telephone:</td>
                            <td><asp:Label ID="lblstudPhoneHome2" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Other Telephone:</td>
                            <td><asp:Label ID="lblstudPhoneOther2" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Primary Language of parent/guardian:</td>
                            <td><asp:Label ID="lblPrLanguageParent2" runat="server"></asp:Label></td>

                        </tr>
                        <tr>
                            <td colspan="11" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">MEETING INFORMATION:</td>
                        </tr>
                        <tr>
                            <td style="text-align:left;">Date of Meeting:</td>
                            <td><asp:Label ID="lblMeetingDate" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Type of Meeting:</td>
                            <td><asp:Label ID="lblMeetingType" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Next Scheduled Annual Review Meeting:</td>
                            <td><asp:Label ID="lblAnnualReview" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Next Scheduled Three Year Reevaluation Meeting:</td>
                            <td><asp:Label ID="lblReevaluation" runat="server"></asp:Label></td>
                        </tr>

                        <tr>
                            <td colspan="11" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">ASSIGNED SCHOOL INFORMATION: (Complete after a placement has been made.)</td>
                        </tr>
                        <tr>
                            <td style="text-align:left;">School Name:</td>
                            <td><asp:Label ID="lblSchoolName" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Telephone:</td>
                            <td><asp:Label ID="lblSchoolPhone" runat="server"></asp:Label></td>
                            </tr>
                        <tr>

                            <td style="text-align:left;">Address:</td>
                            <td><asp:Label ID="lblSchAddress" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Contact Person:</td>
                            <td><asp:Label ID="lblSchContact" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Role:</td>
                            <td><asp:Label ID="lblRole" runat="server"></asp:Label></td>
                            </tr>
                        <tr>
                            <td style="text-align:left;">Telephone:</td>
                            <td><asp:Label runat="server" ID="lblSchTelephone"></asp:Label></td>
                            </tr>
                        <tr>

                            <td style="text-align:left;">Cost-Shared Placement:</td>
                            <td colspan="2"><asp:CheckBox ID="chkYes" runat="server" Text="Yes"
                             CssClass="checkBoxStyle" />
                                <asp:CheckBox ID="chkNo" runat="server" Text="No"
                             CssClass="checkBoxStyle" />
                            </td>
                            </tr>
                        <tr>
                            <td>If yes, specify agency:</td>
                            <td><asp:Label ID="lblAgency" runat="server"></asp:Label></td>
                        </tr>

                        <tr>
                            <td>After a meeting, attach to an IEP, an IEP Amendment or an Extended Evaluation Form.</td>
                        </tr>

                    </table>


                    </div>

                <a id="C11"></a> 
                <div id="iep11">

                    <table class="table" style="text-align:left;width:100%;">

                        <tr>
                    <td colspan="8" class="border" style="text-align:center"><span class="header">Attendance Sheet</span>
                    </td>

                </tr>
                        <tr>
                            <td colspan="8" style="text-align:center;">Special Education Team Meeting</td>
                        </tr>
                        <tr>
                        <td style="text-align:left;"><b>Date: </b>
                            <asp:Label ID="lblDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;"><b>Student Name: </b>
                            <asp:Label ID="lblStudentName" runat="server"></asp:Label>
                        </td>
                        <td><b>DOB: </b>
                            <asp:Label ID="lblDOB" runat="server" ></asp:Label>
                        </td>
                        <td><b>ID#: </b>
                            <asp:Label ID="lblID" runat="server" ></asp:Label>
                        </td>
                    </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                        <tr>
                    <td style="text-align:left;">
                        <b>Purpose of Meeting:</b> Check all boxes that apply.
                    </td>
                </tr>

                        <tr>
                                <td style="text-align:left;">
                                    <asp:CheckBox ID="chkEliDeter" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Eligibility Determination" Width="200px"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left;">
                                    <asp:CheckBox ID="chkInitEval" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Initial Evaluation"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left;">
                                    <asp:CheckBox ID="chkReeval" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Reevaluation"/>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>

                        <tr>
                                <td style="text-align:left;">
                                    <asp:CheckBox ID="chkIEPDev" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="IEP Development" Width="200px"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left;">
                                    <asp:CheckBox ID="chkInit" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Initial"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left;">
                                    <asp:CheckBox ID="chkAnnRev" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Annual Review"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left;">
                                    <asp:CheckBox ID="chkOther" runat="server" 
                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Other: "/>
                                    <asp:Label ID="lblOther" runat="server"></asp:Label>
                                </td>
                            </tr>
                        <tr>
                                <td style="text-align:left;">
                                    <asp:CheckBox ID="chkPlacementData" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Placement" Width="200px"/>
                                </td>
                            </tr>


                        <asp:Repeater ID="RepIep10" runat="server">
        <HeaderTemplate>
             <table id="tableRepeat" class="table" style="width:100%;">
                 
            <tr class="fontBoldMedium" style="text-align:center; color: #FFFFFF; background-color: #03507D;">
                        <td>Print Names of Team Members</td>
                        <td>Print Roles of Team Members</td>
                        <td>Initial if in attendance</td>
                 </table>
        </HeaderTemplate>
            <ItemTemplate>
        <table id="tableRepeat" class="table" style="width:100%;">        
                    <tr class="borderBox">
                        <td class="borderBox" style="text-align:center; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.TMName")%>
                        </td>
                        <td class="borderBox" style="text-align:center; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.TMRole")%>
                        </td>
                        <td class="borderBox" style="text-align:center; min-width:87px; max-width:87px; table-layout:fixed; word-wrap:break-word;"> <%#DataBinder.Eval(Container,"DataItem.InitialIfInAttn")%>
                        </td>
                    </tr>
            </table>
                </ItemTemplate>
        </asp:Repeater>

                        <tr>
                            <td>
                                Attachment to N3
                            </td>
                        </tr>

                        <tr>
                            <td>Massachusetts DOE / Attendance Sheet</td>
                            <td>N 3A</td>
                        </tr>









                    </table>


                    </div>

                <a id="C12"></a>
                <div id="iep12">

                    <table class="table" style="text-align:left;width:100%;">

                        <tr>
                    <td colspan="9" class="border" style="text-align:center"><span class="header">Response Section</span>
                    </td>

                </tr>

                        <tr>
                        <td colspan="9" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">School Assurance</td>
                    </tr>


                        <tr>
                        <td colspan="9">I certify that the goals in this IEP are those recommended by the Team and that the indicated services will be provided.</td>
                    </tr>

                        <tr>
                            <td>Signature and Role of LEA Representative<asp:Label ID="lblSigRep" runat="server"></asp:Label></td>
                            <td>Date<asp:Label ID="lblSigRepDate" runat="server"></asp:Label></td>
                        </tr>

                        <tr>
                        <td colspan="9" style="border-top-style: solid; border-top-width: 1px; border-bottom-style: solid; border-bottom-width: 1px; padding: 1px 4px;text-align:center;">Options / Responses</td>
                    </tr>
                        <tr>
                        <td colspan="9">It is important that the district knows your decision as soon as possible. Please indicate your response by checking at least one (1) box and returning a signed copy to the district. Thank you.</td>
                    </tr>
                        <tr>
                    <td> 
                        <asp:CheckBox ID="acceptIepDeveloped" runat="server"
                            Checked='<%# Convert.ToBoolean(Eval("EngLangInd")) %>'
                            Text="I accept the IEP as developed." />
                    </td>
                    <td>
                        <asp:CheckBox ID="rejectIepDeveloped" runat="server"
                            Checked='<%# Convert.ToBoolean(Eval("EngLangInd")) %>'
                            Text="I reject the IEP as developed" />
                    </td>
                </tr>

                        <tr>
                    <td colspan="8">
                        <asp:CheckBox ID="deleteFollowingPortions" runat="server"
                            Checked='<%# Convert.ToBoolean(Eval("EngLangInd")) %>'
                            Text="I reject the following portions of the IEP with the understanding that any portion(s) that I do not reject will be considered accepted and implemented immediately. Rejected portions are as follows<br />" CssClass="checkBoxStyle" />
                    </td>
                </tr>
                        <tr>
                        <td colspan="8"> <asp:Label ID="lblRejectedPortions" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>

                        <tr>
                    <td colspan="8">
                        <br />
                        <asp:CheckBox ID="RejectionMeeting" runat="server"
                            Checked='<%# Convert.ToBoolean(Eval("EngLangInd")) %>'
                            Text="I request a meeting to discuss the rejected IEP or rejected portion(s).<br />" CssClass="checkBoxStyle" />
                    </td>
                </tr>

                        <tr>
                    <td colspan="8">Signature of Parent, Guardian, Educational Surrogate Parent, Student 18 and Over*<asp:Label ID="lblSigPrnt" runat="server"></asp:Label>
                    </td>
                    <td>Date:&nbsp;<asp:Label ID="lbldteSigPrnt" runat="server"></asp:Label>
                        <br />
                    </td>
                    
                </tr>

                        <tr>

                    <td colspan="8">
                        <br />
                        *Required signature once a student reaches 18 unless there is a court appointed guardian.
                    </td>
                </tr>

                        <tr>
                    <td colspan="8">
                        <br />
                        Parent Comment: I would like to make the following comment(s) but realize any comment(s) made that suggest changes to the proposed IEP will not be implemented unless the IEP is amended.<br /></td>
                </tr>

                        <tr>
                        <td colspan="8"> <asp:Label ID="lblParentComment" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>


                    </table>


                    </div>

                



                </div>
              
        </center>
            </div>
        </div>


        <%--hari start--%>

                



                <%--hari Ends--%>




        <div id="overlay" class="web_dialog_overlay"></div>

        <div id="PopDownload" class="web_dialog" style="width: 600px; top: -20%;">

            <div id="Div53" style="width: 700px;">


                <table style="width: 97%">
                    <tr>
                        <td colspan="2">
                            <table style="width: 85%">
                                <tr>
                                    <td runat="server" id="tdMsgExport" class="tdText" style="height: 50px"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">

                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" />

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
        <%--Start--%>
        <%--PopUp to show BSP Form 05-05-2015 Hari--%>

        <div id="divPrmpts" class="web_dialog2">
            <a id="A4" onclick="HidePopup();" href="#" style="margin-top: -13px; margin-right: -14px;">
                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
            <br />
            <h4>BSP Forms</h4>

            
                
                    <div id="divMessage" runat="server" style="width: 98%"></div>
                    <asp:GridView GridLines="none" CellPadding="4" ID="grdFile" PageSize="5" AllowPaging="True" Width="100%" OnRowEditing="grdFile_RowEditing" OnRowCommand="grdFile_RowCommand" AutoGenerateColumns="false" runat="server" OnPageIndexChanging="grdFile_PageIndexChanging" OnRowDataBound="grdFile_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="No" HeaderText="No" HeaderStyle-Width="10px" />
                            <asp:TemplateField HeaderText="Document Name" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Label CommandName="lbldownload" ID="lbllnkDownload" Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("BSPDoc") %>' ToolTip='<%# Eval("Document") %>' Style="cursor:pointer;" runat="server">
                                    </asp:Label>
                                    </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Download" HeaderStyle-HorizontalAlign="Right" >
                                <ItemTemplate>
                                    <asp:ImageButton  ID="lnkDownload" runat="server" Text='<%# Eval("Document") %>' CommandArgument='<%# Eval("BSPDoc") %>' CommandName="download" ImageUrl="~/Administration/images/download_down_arrow.png" Width="18px" Style="float:left;" />
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

        <%--End--%>
    </form>
</body>
</html>
