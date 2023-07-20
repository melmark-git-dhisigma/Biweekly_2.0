<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE4.aspx.cs" Inherits="StudentBinder_CreateIEP_PE4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.8.0.min.js"></script>
    <link href="CSS/StylePE.css" rel="stylesheet" />
     <link href="CSS/StylePE15.css" rel="stylesheet" />
    <script type="text/javascript">
        //function SetSel(elem)
        //{
        //    var elems = document.getElementsByTagName("input");
        //    var currentState = elem.checked;
        //    var elemsLength = elems.length;

        //    for(i=0; i<elemsLength; i++)
        //    {
        //        if(elems[i].type === "checkbox")
        //        {
        //            elems[i].checked = false;   
        //        }
        //    }

        //    elem.checked = currentState;
        //}​
        $(function () {
            $("input:checkbox.Blind").click(function () {
                $('input:checkbox.Blind').not($(this)).removeAttr("checked");
                $(this).attr("checked", $(this).attr("checked"));
            });

            $("input:checkbox.Deaf").click(function () {
                $('input:checkbox.Deaf').not($(this)).removeAttr("checked");
                $(this).attr("checked", $(this).attr("checked"));
            });

            $("input:checkbox.Communication").click(function () {
                $('input:checkbox.Communication').not($(this)).removeAttr("checked");
                $(this).attr("checked", $(this).attr("checked"));
            });

            $("input:checkbox.AssistiveServices").click(function () {
                $('input:checkbox.AssistiveServices').not($(this)).removeAttr("checked");
                $(this).attr("checked", $(this).attr("checked"));
            });

            $("input:checkbox.Englishproficiency").click(function () {
                $('input:checkbox.Englishproficiency').not($(this)).removeAttr("checked");
                $(this).attr("checked", $(this).attr("checked"));
            });

            $("input:checkbox.Behaviors").click(function () {
                $('input:checkbox.Behaviors').not($(this)).removeAttr("checked");
                $(this).attr("checked", $(this).attr("checked"));
            });
        })
    </script>
    <style>
        div.ContentAreaContainer table td {
            width:auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="HiddenFieldBlind" runat="server" />
        <asp:HiddenField ID="HiddenFieldDeaf" runat="server" />
        <asp:HiddenField ID="HiddenFieldCommunication" runat="server" />
        <asp:HiddenField ID="HiddenFieldAssistiveServices" runat="server" />
        <asp:HiddenField ID="HiddenFieldEnglishproficiency" runat="server" />
        <asp:HiddenField ID="HiddenFieldBehaviors" runat="server" />
       
        <div>
        </div>
        <div id="divIEPP4">
             <div class="ContentAreaContainer">
                 <br />
                 <div class="clear"></div>
             <table cellpadding="0" cellspacing="0" width="96%">
                 <tr>
                    <td id="tdMsg" runat="server" class="top righ"></td>
                </tr>

                <tr>
                    <td class="righ" >INDIVIDUALISED EDUCATION PROGRAM(IEP)
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0"> 
                <tr>
                    <td colspan="2" class="righ">Individual's Name : <asp:Label runat="server" id="lblStudentName"></asp:Label></td>
                </tr>
                <tr>
                    <td style="line-height: 20px;" class="righ">
                        <h2 class="simble">I. SPECIAL CONSIDERATIONS THE IEP TEAM MUST CONSIDER BEFORE DEVELOPING THE IEP. ANY FACTORS CHECKED AS “YES” MUST BE ADDRESSED IN THE IEP.</h2>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="righ" colspan="2">Is the Individual blind or visually impaired?</td>
      
                </tr>
                <tr>
                    <td style="width: 100px;">
                        <input type="checkbox" id="CheckBoxBlindYes" runat="server" class="Blind" value="Yes" />Yes </td>
                    <td class="righ" style="line-height: 20px;">The IEP must include a description of the instruction in Braille and the use of Braille unless the IEP team determines, after an
                              evaluation of the Individual’s reading and writing skills, needs, and appropriate reading and writing media (including an
                              evaluation of the Individual’s future needs for instruction in Braille or the use of Braille), that instruction in Braille or the use of
                              Braille is not appropriate for the Individual.</td>
                </tr>
                <tr>
                     <td class="righ" >
                        <input type="checkbox" id="CheckBoxBlindNo" title="No" value="No" runat="server" class="Blind" />No</td>
                   <td></td>
                </tr>
                <tr>
                    <td class="righ" colspan="2">Is the Individual deaf or hard of hearing?</td>
         
                </tr>
                <tr>
                    <td>
                        <input type="checkbox" id="CheckBoxDeafYes" value="Yes" runat="server" class="Deaf" />Yes</td>
                    <td class="righ" style="line-height: 20px;">The IEP must include a communication plan to address the following: language and communication needs; opportunities for
                                direct communications with peers and professional personnel in the Individual’s language and communication mode; academic
                                level; full range of needs, including opportunities for direct instruction in the Individual’s language and communication mode;
                                and assistive technology devices and services. Indicate in which section of the IEP these considerations are addressed. The
                                Communication Plan must be completed and is available at <a href="https://www.pattan.net" target="_blank">www.pattan.net</a></td>
                </tr>
                <tr>
                    <td class="righ" >
                        <input type="checkbox" id="CheckBoxDeafNo" value="No" runat="server" class="Deaf" />No</td>
   <td></td>
                </tr>
                <tr>
                    <td class="righ" colspan="2">Does the Individual have communication needs?</td>
           
                </tr>
                <tr>
                     <td class="righ" >
                        <input type="checkbox" id="CheckBoxCommunicationYes" value="Yes" runat="server" class="Communication" />Yes</td>
                    <td>
                        Individual needs must be addressed in the IEP (i.e., present levels, specially designed instruction (SDI), annual goals, etc.)
                    </td>
                   
                </tr>
                <tr>
                     <td class="righ" >
                        <input type="checkbox" id="CheckBoxCommunicationNo" value="No" runat="server" class="Communication" />No</td>
              <td></td>
                </tr>
                <tr>
                    <td class="righ" colspan="2">Does the Individual need assistive technology devices and/or services?</td>
           
                </tr>
                <tr>
                     <td class="righ" >
                        <input type="checkbox" id="CheckBoxAssistiveServicesYes" value="Yes" runat="server" class="AssistiveServices" />Yes</td>
                  <td>
                      Individual needs must be addressed in the IEP (i.e., present levels, specially designed instruction, annual goals, etc.)
                  </td>
                </tr>
                <tr>
                     <td class="righ" >
                        <input type="checkbox" id="CheckBoxAssistiveServicesNo" value="No" runat="server" class="AssistiveServices" />No</td>
           <td></td>
                </tr>
                <tr>
                    <td class="righ" colspan="2">Does the Individual have limited English proficiency?</td>
                </tr>
                <tr>
                    <td>
                        <input type="checkbox" id="CheckBoxEnglishproficiencyYes" value="Yes" runat="server" class="Englishproficiency" />Yes</td>
                    <td class="righ">The IEP team must address the Individual’s language needs and how those needs relate to the IEP.</td>
                </tr>
                <tr>
                    <td class="righ" >
                        <input type="checkbox" id="CheckBoxEnglishproficiencyNo" value="No" runat="server" class="Englishproficiency" />No</td>
            <td></td>
                </tr>
                <tr>
                    <td class="righ" colspan="2">Does the Individual exhibit behaviors that impede his/her learning or that of others?</td>
   
                </tr>
                <tr>
                    <td>
                        <input type="checkbox" id="CheckBoxBehaviorsYes" value="Yes" runat="server" class="Behaviors" />Yes</td>
                    <td class="righ"style="line-height: 20px;">The IEP team must develop a Positive Behavior Support Plan that is based on a functional assessment of behavior and that
                             utilizes positive behavior techniques. Results of the functional assessment of behavior may be listed in the Present Levels
                             section of the IEP with a clear measurable plan to address the behavior in the Goals and Specially Designed Instruction
                             sections of the IEP or in the Positive Behavior Support Plan if this is a separate document that is attached to the IEP. A Positive
                             Behavior Support Plan and a Functional Behavioral Assessment form are available at <a href="https://www.pattan.net" target="_blank">www.pattan.net</a></td>
                </tr>
                <tr>
                    <td class="righ" >
                        <input type="checkbox" id="CheckBoxBehaviorsNo" value="No" runat="server" class="Behaviors" />No</td>
             <td></td>
                </tr>
                <tr>
                    <td class="righ" colspan="2">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" Text="Save and continue" OnClick="btnSave_Click" />

                        <%-- <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy" OnClick="btnSave_hdn_Click"  style="display:none;"/>--%>
                    </td>
                  
                </tr>
            </table>
                  <div class="clear"></div>
                 </div>
        </div>
    </form>
</body>
</html>
