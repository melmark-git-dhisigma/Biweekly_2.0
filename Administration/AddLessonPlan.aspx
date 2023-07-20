<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddLessonPlan.aspx.cs" Inherits="LessonPlanDetailsAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <style type="text/css">
        .innerdiv {
            width: 100%;
            height: 100%;
        }

        .outerdiv {
            max-width: 98%;
            height: auto;
            margin-left: auto;
            margin-right: auto;
            padding: 5px;
            /*background-color:#bbb;*/
            min-width: 1000px;
        }

        .header {
            height: 25px;
            background-color: Green;
            font-size: 15px;
            color: White;
            letter-spacing: 2px;
            font-weight: bold;
            padding-left: 5px;
            padding-top: 5px;
        }

        .common {
            height: auto;
            /*background-color:#eee;*/
            /*font-size:15px;*/
            color: White;
            /*font-weight:bold;*/
            padding-left: 5px;
            padding-top: 5px;
        }

        table {
            width: 100%;
            border: 0;
        }

            table tr {
            }

                table tr td {
                    color: #666666;
                    /*font-weight: bold;*/
                    font-size: 11px;
                }

        h2 {
            text-align: center;
            font-size: 16px;
            font-weight: lighter;
            color: #666666;
            background-color: #E6E6E6;
        }

        .checkedlist {
            /*border: 1px solid #d7cece;*/
            padding: 2px 0 2px 0;
            margin: 5px 5px 2px 0;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
        }

        .style1 {
            width: 100%;
        }

        .textBoxNonEditable {
            background-color: transparent;
            border: 1px none #D7CECE;
            padding: 0 4px 0 0;
            border-radius: 3px 3px 3px 3px;
            color: #676767;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 13px;
            height: 25px;
            line-height: 26px;
            width: 225px;
        }

        .auto-style1 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            height: 20px;
        }

        .auto-style2 {
            height: 20px;
        }
    </style>
    <script src="../Administration/JS/jquery-1.8.0.js" type="text/javascript"></script>
    <script type="text/javascript">
        function initilize() {
            if (($('#txtNoofTrail').val().toString() == '0') || ($('#txtNoofTrail').val().valueOf() == '') || ($('#txtNoofTrail').val().valueOf() == null)) {
                $('.showtrail').hide();
                $('#taskAnalysis').show();
            }
            else {
                $('#taskAnalysis').hide();
            }
        }

        function changediptrial(element) {
            if (element.value == 0) {
                $('.showtrail').show();
                $('#taskAnalysis').hide();
            }
            else {
                $('.showtrail').hide();
                $('#taskAnalysis').show();
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            //alert(charCode);
            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                return false;
            else if (charCode == 46)
                return false;
            // if ((charCode < 48 || charCode > 57 )&& charCode > 31)
            //      return false;
            return true;
        }
    </script>

</head>
<body onload="initilize();">
    <form id="form1" runat="server">
        <div class="outerdiv">
            <div class="innerdiv">
             
                <div class="common">


                    <table cellpadding="-1" cellspacing="2">
                        <tr>
                            <td id="tdMsg" runat="server" colspan="6" style="font-weight: normal;"></td>
                        </tr>
                        <tr id="stdLevel" runat="server">
                            <td class="tdText">Student Name:</td>
                            <td colspan="2" class="tdText">
                                <asp:Label ID="labStudentname" runat="server"></asp:Label>
                            </td>
                            <td class="tdText">IEP Dates:
                            </td>
                            <td class="tdText">
                                <asp:Label ID="Lbl_IEPDates" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr id="lessonLevel" runat="server">
                            <td class="tdText">Lesson Plan Name</td>
                            <td colspan="2" class="tdText">
                                <asp:TextBox ID="txtLessonName" runat="server" MaxLength="40" CssClass="textBoxNonEditable"></asp:TextBox>
                                <%--<asp:TextBox ID="txtLessonName" runat="server" Text="" CssClass="textBoxNonEditable"></asp:TextBox>--%>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdText">Goal:</td>
                            <td colspan="4" class="tdText">
                                <div style="width: 99%; max-height: 80px; overflow: auto; text-align: left;" class="checkedlist">

                                    <asp:CheckBoxList ID="chkgoal" runat="server" AutoPostBack="False"
                                        BorderStyle="None" CellPadding="0" CellSpacing="0" EnableTheming="True"
                                        RepeatColumns="3">
                                    </asp:CheckBoxList>
                                    <asp:Label ID="labgoal" runat="server"></asp:Label>


                                </div>
                            </td>
                        </tr>
                        <tr id="objective" runat="server">
                            <td class="tdText">Objective:</td>
                            <td colspan="4" class="tdText">
                                <div id="txtObjective" class="checkedlist" runat="server" style="width: 99%;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdText">Framework and Strand:</td>
                            <td colspan="2">
                                <%-- <asp:TextBox ID="" runat="server" MaxLength="100" CssClass="textBoxNonEditable"></asp:TextBox>--%>

                                <asp:Label ID="txtFramandStrand" runat="server" Text="" CssClass="textBoxNonEditable"></asp:Label>
                            </td>
                            <td class="tdText">Specific Standard:</td>
                            <td>
                                <%-- <asp:TextBox ID="" runat="server" MaxLength="100" CssClass="textBoxNonEditable"></asp:TextBox>--%>
                                <asp:Label ID="txtSpecStd" runat="server" Text="" CssClass="textBoxNonEditable"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%" colspan="3">
                                <h2 style="margin-right: 5px;">Type of Instruction</h2>
                            </td>
                            <td class="tdText">Specific Entry Point:</td>
                            <td>
                                <asp:TextBox ID="txtspecEntrypoint" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td class="auto-style1">Teaching Procedure
                            </td>
                            <td colspan="2" class="auto-style2">
                                <asp:DropDownList CssClass="drpClass" ID="drpTeachingProc" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td rowspan="4" class="tdText">Major Setting :</td>
                            <td rowspan="4">
                                <asp:TextBox ID="txtmajorset"
                                    Style="resize: vertical; max-height: 200px; min-height: 50px" Width="93%"
                                    runat="server" TextMode="MultiLine"
                                    Height="70px" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td rowspan="3" class="tdText">Datasheet Style
                            </td>
                            <td colspan="2" class="tdText">
                                <asp:RadioButtonList ID="rdolistDatasheet" runat="server" Style="font-family: Arial;"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem onclick="changediptrial(this)" Selected="True" Value="1">Task Analysis</asp:ListItem>
                                    <asp:ListItem id="disc" onclick="changediptrial(this)" Value="0">Discrete Trial</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" class="tdText" style="height: 38px">
                                <div id="taskAnalysis" runat="server">
                                    <asp:DropDownList ID="drpTasklist" runat="server" CssClass="drpClass" Style="width: 130px">
                                        <asp:ListItem Value="0">---- Select ----</asp:ListItem>
                                        <asp:ListItem>Forward chain</asp:ListItem>
                                        <asp:ListItem>Backward chain</asp:ListItem>
                                        <asp:ListItem>Total Task</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div id="showtrailid" runat="server" class="showtrail tdText">
                                    No of Trials
            <asp:TextBox ID="txtNoofTrail" onkeypress="return isNumberKey(event)" runat="server" Width="50px" MaxLength="2"></asp:TextBox>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2"></td>
                        </tr>

                        <tr>
                            <td colspan="2" class="tdText">Prompt Procedure</td>
                            <td>
                                <asp:DropDownList CssClass="drpClass" ID="drpPromptproc" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td rowspan="2" class="tdText">Minor Setting:</td>
                            <td rowspan="2">
                                <asp:TextBox ID="txtminorset"
                                    Style="resize: vertical; max-height: 200px; min-height: 50px" Width="93%"
                                    runat="server" TextMode="MultiLine"
                                    Height="70px" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" class="tdText">Prompt Used</td>
                            <td>
                                <div style="width: 99%; height: 120px; overflow: auto; text-align: left; border: 1px solid #d7cece;" class="checkedlist">
                                    <asp:CheckBoxList ID="chkpromptused" runat="server" RepeatColumns="2">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" class="tdText">Pre-requisite Skills:</td>
                            <td colspan="3">
                                <asp:TextBox Style="resize: vertical; max-height: 200px; min-height: 50px"
                                    ID="txtPrerequistskill" runat="server" TextMode="MultiLine"
                                    Width="97%" Height="50px" MaxLength="400"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" class="tdText">Materials:</td>
                            <td colspan="3">
                                <asp:TextBox Style="resize: vertical; max-height: 200px; min-height: 50px"
                                    ID="txtMaterials" runat="server" TextMode="MultiLine" Width="97%"
                                    Height="50px" MaxLength="400"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" rowspan="5" class="tdText">Measurement System:
                            </td>
                            <td rowspan="5">
                                <asp:ListBox ID="lstMeasurement" runat="server" Height="95%" Width="100%"
                                    Enabled="False"></asp:ListBox>
                            </td>
                            <td colspan="2">
                                <h2>Response Definitions:</h2>
                            </td>
                        </tr>

                        <tr>
                            <td class="tdText">Correct Response:</td>
                            <td>
                                <asp:TextBox ID="txtCorretResp" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>

                        <tr>
                            <td class="tdText">Incorrect Response:</td>
                            <td>
                                <strong>
                                    <asp:TextBox ID="txtIncorrResp" runat="server" MaxLength="100"></asp:TextBox>
                                </strong></td>
                        </tr>

                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>




                </div>








                <div class="head_box">LESSON PROCEDUREE</div>
                <div class="common">

                    <table cellpadding="0" cellspacing="3">
                        <tr>
                            <td style="text-align: center; width: 30%" colspan="2" class="tdText">
                                <h2>Teacher</h2>
                                (Sd/Instruction)</td>
                            <td style="text-align: center; width: 30%" colspan="2" class="tdText">
                                <h2>Student</h2>
                                (response/desired outcome)</td>
                            <td style="text-align: center; width: 40%" colspan="2" class="tdText">
                                <h2>Consequence</h2>
                                (R+ delivery or correction procedure)</td>
                        </tr>
                        <tr>
                            <td class="tdText">Lesson Delivery Instructions:</td>
                            <td class="tdText">
                                <asp:TextBox ID="txtLessondelivery" runat="server" MaxLength="100"></asp:TextBox>&nbsp; &nbsp;
                            </td>
                            <td class="tdText">Student Readiness Criteria:</td>
                            <td class="tdText">
                                <asp:TextBox ID="txtStdread" runat="server" MaxLength="100"></asp:TextBox>

                            </td>
                            <td class="tdText">Teacher Response 
        <br />
                                to Readiness:</td>
                            <td class="tdText">
                                <asp:TextBox ID="txtTeacherResp" runat="server"
                                    MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" class="tdText">Step Description(s):</td>
                            <td class="tdText">
                                <div runat="server" id="Stepdes" style="width: 100%; height: 80px; overflow: auto; text-align: left; font-weight: normal">
                                </div>
                            </td>
                            <td class="tdText">Correct Response:</td>
                            <td class="tdText">
                                <asp:TextBox ID="txtCorrtlessResp" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                            <td class="tdText">Reinforcement<br />
                                Procedure:</td>
                            <td class="tdText">
                                <asp:TextBox ID="txtReinfoProce" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" class="tdText">Set Description(s):</td>
                            <td class="tdText">
                                <div id="Setdes" runat="server"
                                    style="width: 100%; height: 80px; overflow: auto; text-align: left; font-weight: normal">
                                </div>
                            </td>
                            <td class="tdText">Incorrect Response:</td>
                            <td class="tdText">
                                <asp:TextBox ID="txtincorrtlessResp" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                            <td class="tdText">Correction Procedure:</td>
                            <td class="tdText">
                                <asp:TextBox ID="txtCorretProce" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                </div>








                <div class="head_box">CRITERIA</div>
                <div class="common">
                    <table style="font-size: 20px;">
                        <tr>
                            <td valign="top" style="width: 30%; overflow: auto;" class="tdText">To Advance a Prompt:
                    <div id="txtadvprompt" style="width: 100%; text-align: left; font-weight: normal" runat="server">
                        <br />
                    </div>
                            </td>
                            <td valign="top" style="width: 30%; overflow: auto;" class="tdText">To Move Back:
                    <div id="txtpromptmoveback" style="width: 100%; text-align: left; font-weight: normal" runat="server">
                    </div>
                            </td>
                            <td valign="top" style="width: 30%; overflow: auto;" class="tdText">For Modification: 
                    <div id="txtpromptmodification" style="width: 100%; text-align: left; font-weight: normal" runat="server">
                    </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" class="tdText">To Advance a Step:
                    <div id="txtadvstep" style="width: 100%; text-align: left; font-weight: normal" runat="server">
                        <br />
                    </div>
                            </td>
                            <td valign="top" class="tdText">To Move Back:
                    <div id="txtstepmoveback" style="width: 100%; text-align: left; font-weight: normal" runat="server">
                    </div>
                            </td>
                            <td valign="top" class="tdText">For Modification:
                    <div id="txtstepmodification" style="width: 100%; text-align: left; font-weight: normal" runat="server">
                    </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" class="tdText">To Advance a Set:
                    <div id="txtadvSet" style="width: 100%; text-align: left; font-weight: normal" runat="server">
                    </div>
                            </td>
                            <td valign="top" class="tdText">To Move Back:
                    <div id="txtsetmoveback" style="width: 100%; text-align: left; font-weight: normal" runat="server">
                    </div>
                            </td>
                            <td valign="top" class="tdText">For Modification: 
                    <div id="txtSetmodifia" style="width: 100%; text-align: left; font-weight: normal" runat="server">
                    </div>
                            </td>
                        </tr>
                    </table>
                </div>





                <div class="head_box">BASELINE PROCEDURE</div>
                <div class="common">
                    <table class="style1">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtBaselineProc" runat="server" Height="50px"
                                    Style="resize: vertical; max-height: 200px; min-height: 50px"
                                    TextMode="MultiLine" Width="98%" MaxLength="400"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>






                <div class="head_box">DATES OF REVISIONS</div>
                <div class="common"></div>
                <br />
                <table width="100%">
                        <tr>
                            <td style="width: 100%" align="center">

                                <asp:Button ID="butSave" runat="server" CssClass="NFButton"
                                    Text="Save" OnClick="butSave_Click" />

                            </td>
                          
                        </tr>
                    </table>


            </div>


        </div>
    </form>
</body>
</html>
