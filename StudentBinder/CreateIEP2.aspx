<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/CreateIEP2.aspx.cs" Inherits="StudentBinder_CreateIEP2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 20%;
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
    </style>
   <script type="text/javascript">

       function submitClick() {
           var txtAccomodation = document.getElementById('<%=txtAccomodation.ClientID %>').innerHTML;
           var txtDisabilities = document.getElementById('<%=txtDisabilities.ClientID %>').innerHTML;           
           PageMethods.submitIEP2(txtAccomodation, txtDisabilities);
       }
       function GetFreetextval(content, divid) {
           if (divid == 'txtAccomodation') {
               document.getElementById('<%=txtAccomodation.ClientID %>').innerHTML = "";
               document.getElementById('<%=txtAccomodation.ClientID %>').innerHTML = content;
               document.getElementById('<%=txtAccomodation_hdn.ClientID %>').value = window.escape(content);
           }
           else if (divid == 'txtDisabilities') {
               document.getElementById('<%=txtDisabilities.ClientID %>').innerHTML = "";
               document.getElementById('<%=txtDisabilities.ClientID %>').innerHTML = content;
               document.getElementById('<%=txtDisabilities_hdn.ClientID %>').value = window.escape(content);
           }
       }
           


       function chkLen() {
           var val = document.getElementById('txtDisabilities').value;
           var val1 = document.getElementById('txtAccomodation').value;
          
           if ((parseInt(val.length) < 2500) && (parseInt(val1.length) < 2500)) {
               return true
           }
           else {
               tdMsg.innerHTML = '<div class=error_box>Text in Editor should be less than 200 charecters.</div>'
               return false;
           }
       }

       function scrollToTop() {
           window.scrollTo(0, 0);
           window.parent.parent.scrollTo(0, 100);
       }
   </script>
</head>
    
<body>
    <form id="form1" runat="server">

        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ToolkitScriptManager>

        <br />
        <div style="margin-left: 145px;">
        </div>
        <div id="divIep2" style="width: 97%; border-radius: 3px 3px 3px 3px; padding: 7px;">

            <table cellpadding="0" cellspacing="0" width="97%">

                <tr>
                    <td colspan="2" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; height: 25px;">Present Levels of Educational Performance
                    </td>
                </tr>
                <tr>
                    <td colspan="2" runat="server" id="tdMsg">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <h3>A: General Curriculum</h3>
                    </td>
                </tr>

                <tr>
                    <td align="left" style="width: 45%; font-weight: bold; padding-left: 7px;" class="tdText">Check all that apply</td>
                    <td align="left" class="tdText" style="width: 85%; font-weight: bold; padding-left: 3px;">General Curriculum area(s) affected by this student&#39;s disability(ies):?</td>
                </tr>
                <tr>
                    <td>
                        <table class="style1" style="text-align: left">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkEnglishLangArts" runat="server"
                                        Checked='<%# Convert.ToBoolean(Eval("EngLangInd")) %>'
                                        Text="English Language Arts" CssClass="checkBoxStyle" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkHistoryAndSocial" runat="server"
                                        Checked='<%# Convert.ToBoolean(Eval("HistInd")) %>'
                                        Text="History and Social Sciences" CssClass="checkBoxStyle" TabIndex="1" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkScienceAndTech" runat="server"
                                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>'
                                        Text="Science and Technology" CssClass="checkBoxStyle" TabIndex="3" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkMathematics" runat="server"
                                        Checked='<%# Convert.ToBoolean(Eval("MathInd")) %>' Text="Mathematics"
                                        CssClass="checkBoxStyle" TabIndex="4" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkOtherCurriculumActivities" runat="server"
                                        Checked='<%# Convert.ToBoolean(Eval("OtherInd")) %>' CssClass="checkBoxStyle"
                                        Text="Other Curriculum Areas" TabIndex="5" />
                                </td>
                            </tr>
                        </table>

                    </td>
                    <td>

                        <table class="style1" style="text-align: left">
                            <tr style ="height:23px;">
                                <td>Consider the language, composition, literature (include reading) and media 
            strands. 
                                </td>
                            </tr>
                            <tr style ="height:22px;">
                                <td>Consider the history, geography, economics and civics and government 
            strands.</td>
                            </tr>
                            <tr style ="height:22px;">
                                <td>Consider the inquiry, domains of science, technology and science, technology 
            and human affairs strand.</td>
                            </tr>
                            <tr style ="height:22px;">
                                <td>Consider the number sense, patterns, relations and functions, geometry and 
            measurement and statistics
            and probability strands.</td>
                            </tr>
                            <tr>
                                <td>

                                    <table style="width: 100%;">
                                        <tr>
                                            <td style ="width:7%;">Specify :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSpecify" runat="server" CssClass="textClass" Width="600px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>

                                    </table>

                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td align="left" class="tdTextLeft" colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; height: 50px;" class="tdText">How does the disability(ies)  affect progress in the curriculum area(s)?
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtDisabilities_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtDisabilities" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP2.aspx',this); "></div>

                        

                    </td>
                </tr>
                <tr>
                    <td class="tdTextLeft" colspan="2"></td>
                </tr>
                <tr style="height: 37px">
                    <td colspan="2" style="width: 100%; height: 50px;" class="tdText">What type(s) of accommodation, if any, is 
        necessary for the student to make effective progress?
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtAccomodation_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtAccomodation" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP2.aspx',this); "></div>

   
                        

                    </td>
                </tr>
                <tr>
                    <td class="tdText" colspan="2" height="60">
                        <br />
                        What type(s) of specially designed 
        instruction, if any, is necessary for the student to make effective progress? 
        <br />
                        Check the neccessary instructional modification(s) and describe how such 
        modification(s) will be made.</td>
                </tr>
                <tr><td colspan="2" style="height:5px"></td></tr>

                <tr>
                    <td align="left" style="width: 5%" class="tdText">
                        <asp:CheckBox ID="chkContent" runat="server"
                            Checked='<%# Convert.ToBoolean(Eval("EngLangInd")) %>'
                            Text="Content" CssClass="checkBoxStyle" />
                    </td>
                    <td align="left" class="tdText">
                        <asp:TextBox ID="txtContent" runat="server" CssClass="textClass" TextMode="MultiLine"
                            Width="670px" Height="50px" ></asp:TextBox>

                    </td>
                </tr>

                <tr><td colspan="2" style="height:5px"></td></tr>

                <tr>
                    <td align="left" style="width: 5%" height="45" class="tdText">
                        <asp:CheckBox ID="chkMethodology" runat="server"
                            Checked='<%# Convert.ToBoolean(Eval("EngLangInd")) %>'
                            Text="Methodology/Delivery of Instruction" CssClass="checkBoxStyle" />
                    </td>
                    <td align="left" class="tdText">
                        <asp:TextBox ID="txtMethodology" runat="server" CssClass="textClass"
                            Width="670px" TextMode="MultiLine" Height="50px"></asp:TextBox>

                    </td>
                </tr>

                <tr><td colspan="2" style="height:5px"></td></tr>

                <tr>
                    <td align="left" style="width: 5%" class="tdText">
                        <asp:CheckBox ID="chkPerformanceCriteria" runat="server"
                            Checked='<%# Convert.ToBoolean(Eval("EngLangInd")) %>'
                            Text="Performance Criteria" CssClass="checkBoxStyle" />
                    </td>
                    <td align="left" class="tdText">
                        <asp:TextBox ID="txtPerformance" runat="server" CssClass="textClass"
                            Width="670px" TextMode="MultiLine" Height="50px"></asp:TextBox>

                    </td>
                </tr>




                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">

                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" 
                            OnClick="btnSave_Click" Text="Save and continue" OnClientClick="" />
                       <%-- <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" 
                            OnClick="btnSave_hdn_Click" Text="dummy" OnClientClick="submitClick();" style="display:none;" />--%>

                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
