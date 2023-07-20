<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/CreateIEP10.aspx.cs" Inherits="StudentBinder_CreateIEP10" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   


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

    
    <style type="text/css">
        .style1 {
            width: 100%;
        }


        .style2 {
            width: 25%;
        }

        .style4 {
            width: 31%;
        }

        .style5 {
            width: 20%;
        }

        .ui-datepicker {
            font-size: 8pt !important;
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

        .tdText10 {
            color: #666;
            font-family: Arial;
            font-size: 15px;
            height: 25px;
            line-height: 12px;
            padding-right: 1px;
            text-align: left;
        }

        .tdWdth {
            width: 142px;
        }


        .tdText {
            color: #666;
            font-family: Arial;
            font-size: 13px;
            height: 14px;
            line-height: 23px;
            padding-right: 1px;
            text-align: left;
            width: 135px;
        }
    </style>
  
    <script type="text/javascript">
        $(function () {
            $("[id$=TextBoxMeetingDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../StudentBinder/img/Calendar24.png'
            });
        });

        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 100);
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

        

        <div id="divIEPP10" style="width: 97%; border-radius: 3px 3px 3px 3px; padding: 7px;">

            <table id="table1" cellpadding="0" cellspacing="0" width="97%">


                <tr>
                    <td colspan="6" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; padding-top: 10px; padding-bottom: 10px;" class="auto-style6">Administrative Data Sheet
                    </td>
                </tr>
                <tr>
                    <td runat="server" id="tdMsg" style="color: #FF0000" colspan="1"></td>

                </tr>
            </table>
            <h3>STUDENT INFORMATION:
                    
            </h3>

            <table id="table2" cellpadding="0" cellspacing="0" width="97%">
                <tr>
                    <td class="tdText">Full Name:</td>
                    <td class="tdWdth">&nbsp;<asp:Label ID="lblStudentName" runat="server"></asp:Label></td>
                    <td class="tdText">School ID#:</td>
                    <td class="tdWdth">&nbsp;<asp:Label ID="lblStudentSch" runat="server"></asp:Label>
                    </td>
                    <td class="tdText">SASID:</td>
                    <td class="tdWdth">&nbsp;<asp:Label ID="lblsasid" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td class="tdText">Birth Date :</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblDOB" runat="server"></asp:Label>
                    </td>
                    <td class="tdText">Place of Birth:</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblPlace" runat="server"></asp:Label>
                    </td>
                    <td class="tdText">Age:</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblAge" runat="server"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="tdText">Grade/Level:</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblGrade" runat="server"></asp:Label>
                    </td>
                    <td class="tdText">Primary Language:</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblPrLanguage" runat="server"></asp:Label></td>

                    <td class="tdText">Language of Instruction:</td>
                    <td class="tdWdth">
                        <asp:TextBox ID="TextBoxInstruction" CssClass="textfield" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdText">Address:</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                    </td>
                    <td></td>
                    <td></td>
                    <td>Sex:</td>
                    <td>
                        <asp:Label ID="lblSex" runat="server"></asp:Label></td>
                    <%-- <td class="tdText"
                                            style="border-right-style: dashed; border-right-width: thin; border-right-color: #C0C0C0">
                                            <asp:RadioButtonList ID="rblSex" runat="server"
                                                RepeatDirection="Horizontal" CssClass="checkBoxStyle">
                                                <asp:ListItem Value="0">Male</asp:ListItem>
                                                <asp:ListItem Value="1">Female</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>--%>
                </tr>
                <tr>
                    <td class="tdText">Home Telephone:</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblPhoneHome" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">If 18 or older:</td>

                    <td style="text-align: left">
                        <asp:CheckBox ID="chkActingon" runat="server" Text="Acting on Own Behalf"
                            
                             />
                    </td>
                    <td style="text-align: left;">
                        <asp:CheckBox ID="chkCourtAppointed" runat="server" Width="140px" Text="Court Appointed Guardian:"
                            />
                    </td>
                    <td colspan="2">
                        
                        <asp:TextBox ID="txtCourtAppointed" runat="server" Width="226px" CssClass="textClass"
                             MaxLength="50"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td style="text-align: left">
                        <asp:CheckBox ID="chkDecisionMaking" runat="server" Text="Shared Decision-Making"
                             />
                    </td>
                    <td style="text-align: left" colspan="2">
                        <asp:CheckBox ID="ChkDelegateDecision" runat="server" Text="Delegate Decision-Making"
                             />
                    </td>

                </tr>
                <tr>
                    <td style="border-bottom: 2px double #00634d;" colspan="6"></td>
                </tr>
            </table>

            <h3>PARENT/GUARDIAN INFORMATION:
            </h3>


            <table id="table6" cellpadding="0" cellspacing="0" width="97%">
                <tr>
                    <td class="tdText">Name</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblName" runat="server"></asp:Label>
                    </td>
                    <td class="tdText" style="width:140px">Relationship to Student:</td>
                    <td>
                        <asp:Label ID="lblRelationship" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td class="tdText">Address:</td>
                    <td>
                        <asp:Label ID="lblstudAddress" runat="server"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="tdText">Home Telephone:</td>
                    <td>
                        <asp:Label ID="lblstudPhoneHome" runat="server"></asp:Label>
                    </td>

                    <td class="tdText">Other Telephone:</td>
                    <td>
                        <asp:Label ID="lblstudPhoneOther" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText" style="width: 224px;">Primary Language of parent/guardian:</td>
                    <td>
                        <asp:TextBox ID="TextBoxPrLanguageParent" CssClass="textfield" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="border-bottom: 2px double #00634d;padding-top:4px;" colspan="6"></td>
                </tr>

            </table>
            <h3>PARENT/GUARDIAN INFORMATION:
            </h3>

            <table id="table3" cellpadding="0" cellspacing="0" width="97%">
                <tr>
                    <td class="tdText">Name</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblName1" runat="server"></asp:Label>
                    </td>
                    <td class="tdText" style="width:290px">Relationship to Student:</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblRelationship1" runat="server"></asp:Label>
                    </td>


                </tr>
                <tr>
                    <td class="tdText">Address:</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblstudAddress1" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td class="tdText">Home Telephone:</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblstudPhoneHome1" runat="server"></asp:Label>
                    </td>

                    <td class="tdText">Other Telephone:</td>
                    <td class="tdWdth">
                        <asp:Label ID="lblstudPhoneOther1" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText" style="width: 220px;">Primary Language of parent/guardian:</td>
                    <td>
                        <asp:TextBox ID="TextBoxPrLanguageParent1" CssClass="textfield" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="6" style="width:30px"></td>
                </tr>
                <tr>
                    <td style="border-bottom: 2px double #00634d;padding-top:4px;" colspan="6"></td>
                </tr>

            </table>


            <h3>MEETING INFORMATION:
            </h3>

            <table id="table4" cellpadding="0" cellspacing="0" width="97%">

                <tr>
                    <td class="tdText">Date of Meeting:</td>
                    <td>

                        <asp:TextBox ID="TextBoxMeetingDate" runat="server" Width="202px" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox></td>


                    <td class="tdText">Type of Meeting: </td>
                    <td class="tdWdth">
                        <asp:TextBox ID="TextBoxMeetingType" CssClass="textfield" runat="server"></asp:TextBox></td>

                </tr>
                <tr >
                    <td class="tdText"  style="padding-top:4px;" colspan="2">Next Scheduled Annual Review Meeting:</td>
                    <td class="tdWdth" style="padding-top:4px;">
                        <asp:TextBox ID="TextBoxAnnualReview" CssClass="textfield" runat="server"></asp:TextBox></td>
                    </tr>
                 <tr>
                    <td class="tdText" style="padding-top:4px;" colspan="2">Next Scheduled Three Year Reevaluation Meeting:</td>
                    <td class="tdWdth" style="padding-top:4px;">
                        <asp:TextBox ID="TextBoxReevaluation" CssClass="textfield" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="border-bottom: 2px double #00634d;padding-top:4px;" colspan="6"></td>
                </tr>
            </table>


            <h3>ASSIGNED SCHOOL INFORMATION: (Complete after a placement has been made.) </h3>

            <table id="table5" cellpadding="0" cellspacing="0" width="97%">
                <tr>
                    <td class="tdText">School Name:</td>
                    <td class="tdWdth">
                       <!-- <asp:Label ID="lblSchoolName" runat="server"></asp:Label> -->
                        <asp:TextBox ID="txtSchoolName" runat="server"></asp:TextBox>
                    </td>

                    <td class="tdText">&nbsp;&nbsp;Telephone:</td>
                    <td class="tdWdth">
                       <!-- <asp:Label ID="lblSchoolPhone" runat="server"></asp:Label>-->
                        <asp:TextBox ID="txtSchoolPhone" runat="server"></asp:TextBox>
                        </td>
                    <td>
                        <asp:RegularExpressionValidator ID="SchoolPhoneValidator" runat="server" ControlToValidate="txtSchoolPhone" ErrorMessage="Enter a valid phone number. Eg:(999)999-9999" ValidationExpression="^\([0-9]{3}\)[0-9]{3}-[0-9]{4}$"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Address:</td>
                    <td class="tdWdth">
                       <!-- <asp:Label ID="lblSchAddress" runat="server"></asp:Label> -->
                        <asp:TextBox ID="txtSchAddress" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Contact Person:</td>
                    <td>
                        <!--<asp:Label ID="lblSchContact" runat="server"></asp:Label>-->
                        <asp:TextBox ID="txtSchContact" runat="server"></asp:TextBox>
                    </td>

                    <td class="tdText">&nbsp;&nbsp;Role:</td>
                    <td class="tdWdth">

                        <%--<asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="True" CssClass="drpClass" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                            <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                        </asp:DropDownList>--%>
                        <asp:TextBox runat="server" ID="txtDesc"></asp:TextBox>

                    </td>
                    </tr>
                <tr>
                    <td class="tdText">Telephone:</td>
                    <td class="tdWdth">
                      <!--  <asp:Label runat="server" ID="lblSchTelephone"></asp:Label>-->
                        <asp:TextBox runat="server" ID="txtSchTelephone"></asp:TextBox>
                        </td>
                    <td>
                        <asp:RegularExpressionValidator ID="SchTelephoneValidator" runat="server" ControlToValidate="txtSchTelephone" ErrorMessage="Enter a valid phone number. Eg:(999)999-9999" ValidationExpression="^\([0-9]{3}\)[0-9]{3}-[0-9]{4}$"></asp:RegularExpressionValidator>

                    </td>
                </tr>
                <tr>
                    <td class="tdText">Cost-Shared Placement:</td>
                    <td class="tdWdth">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server"
                            RepeatDirection="Horizontal" CssClass="checkBoxStyle">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>

                    <td class="tdText">&nbsp;&nbsp;If yes,specify agency:</td>
                    <td class="tdWdth">
                        <asp:TextBox ID="TextBoxAgency" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="border-bottom: 2px double #00634d;" colspan="6"></td>
                </tr>

                <tr>
                    <td style="font-weight: bold;text-align:center;" colspan="6">After a meeting, attach to an IEP, an IEP Amendment or an Extended Evaluation Form.</td>
                </tr>



                <tr>


                    <td style="text-align: center;padding-top:4px;" colspan="6">&nbsp;
                        <asp:Button ID="Button1" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_Click" Text="Save and continue" OnClientClick="scrollToTop();"/>

                         <%--<asp:Button ID="Button1_hdn" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_hdn_Click" Text="dummy"  style="display:none;"/>--%>
                    </td>

                </tr>
                <%--<tr>
                    <td class="top righ"  colspan="2">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_Click" Text="Save and continue" OnClientClick="submitClick();" />
                    </td>
                </tr>--%>
            </table>
            <div class="clear"></div>
        </div>

    </form>
</body>
</html>
