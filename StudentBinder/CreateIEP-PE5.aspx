<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE5.aspx.cs" Inherits="StudentBinder_CreateIEP_PE5" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.8.0.min.js"></script>
    <link href="CSS/StylePE.css" rel="stylesheet" />
     <link href="CSS/StylePE15.css" rel="stylesheet" />

    <style type="text/css">
        .FreeTextDivContent {
            width: 98%;
            min-height: 100px;
            height: auto;
            padding: 2%;
            border: 1px solid rgb(228, 228, 228);
            border-radius: 8px 8px 8px 8px;
        }
    </style>


    <script type="text/javascript">
        var txt1 = "", text2 = "";
        function GetFreetextval(content, divid) {
            if (divid == 'TextBoxOtherDetails') {
                document.getElementById('<%=TextBoxOtherDetails.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxOtherDetails.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxOtherDetails_hdn.ClientID %>').value =window.escape(content);
                txt1 = content;
            }
            else if (divid == 'TextBoxCIPCode') {
                document.getElementById('<%=TextBoxCIPCode.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxCIPCode.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxCIPCode_hdn.ClientID %>').value =window.escape(content);
                text2 = content;
            }
    }

    function loadValues() {
        if (document.getElementById('<%=TextBoxOtherDetails.ClientID %>').innerHTML != "") {
            txt1 = document.getElementById('<%=TextBoxOtherDetails.ClientID %>').innerHTML;
            txt1 = txt1.replace(/'/g, '##');
            txt1 = txt1.replace(/\\/g, '?bs;');
            }
            if (document.getElementById('<%=TextBoxCIPCode.ClientID %>').innerHTML != "") {
                text2 = document.getElementById('<%=TextBoxCIPCode.ClientID %>').innerHTML;
                text2 = text2.replace(/'/g, '##');
                text2 = text2.replace(/\\/g, '?bs;');
            }
            
     
        }

        function submitClick() {
            loadValues();
            $.ajax(
          {

              type: "POST",
              url: "CreateIEP-PE5.aspx/submitIepPE5",
              data: "{'arg1':'" + txt1 + "','arg2':'" + text2 + "'}",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              async: false,
              success: function (data) {
                  var contents = data.d;


              },
              error: function (request, status, error) {
                  alert("Error");
              }
          });


            //  PageMethods.submitIepPE1(txt1, text2);

        }

        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 100);
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <div id="divIEPP5">
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
                    <td class="righ">Other (specify):</td>
                </tr>
                <tr>
                    <td class="righ">
                        <%-- <asp:TextBox ID="TextBoxOtherDetails" runat="server" TextMode="MultiLine" Width="60%"></asp:TextBox>--%>
                        <asp:TextBox runat="server" id="TextBoxOtherDetails_hdn" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxOtherDetails"  runat="server" class="FreeTextDivContent" style="overflow:scroll;" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE5.aspx',this); "></div>
                    </td>
                </tr>
                <tr>
                    <td class="righ"><h2 class="simble">II. PRESENT LEVELS OF ACADEMIC ACHIEVEMENT AND FUNCTIONAL PERFORMANCE</h2>
                    </td>
                </tr>
                <tr>
                    <td class="righ">Include the following information related to the Individual:
                    </td>
                </tr>
                <tr>
                    <td class="righ">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <%--<td class="top" style="width: 40px;"></td>--%>
                                <td class="righ top">
                                    <ul style="list-style: disc outside;">
                                        <li>Present levels of academic achievement (e.g., most recent evaluation of the Individual, results of formative assessments, curriculum-based
                                            assessments, transition assessments, progress toward current goals)
                                        </li>
                                        <li>Present levels of functional performance (e.g., results from a functional behavioral assessment, results of ecological assessments, progress
                                            toward current goals)
                                        </li>
                                        <li>Present levels related to current postsecondary transition goals if the Individual’s age is 14 or younger if determined appropriate by the IEP team
                                            (e.g., results of formative assessments, curriculum-based assessments, progress toward current goals)
                                        </li>
                                        <li>Parental concerns for enhancing the education of the Individual
                                        </li>
                                        <li>How the Individual’s disability affects involvement and progress in the general education curriculum
                                        </li>
                                        <li>Strengths
                                        </li>
                                        <li>Academic, developmental, and functional needs related to Individual’s disability
                                        </li>
                                    </ul>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
        
                <tr>
                    <td class="righ" style="line-height: 20px;">III. TRANSITION SERVICES – This is required for Individuals age 14 or younger if determined appropriate by the IEP team. If the Individual does not attend the
                     IEP meeting, the school must take other steps to ensure that the Individual’s preferences and interests are considered. Transition services are a coordinated
                     set of activities for a Individual with a disability that is designed to be within a results oriented process, that is focused on improving the academic and
                     functional achievement of the Individual with a disability to facilitate the Individual’s movement from school to post school activities, including postsecondary
                     education, vocational education, integrated employment (including supported employment), continuing and adult education, adult services, independent
                     living, or community participation that is based on the individual Individual’s needs taking into account the Individual’s strengths, preferences, and interests.</td>
                </tr>
       
                <tr>
                    <td class="righ"style="line-height: 20px;">POST SCHOOL GOALS – Based on age appropriate assessment, define and project the appropriate measurable postsecondary goals that address education
                     and training, employment, and as needed, independent living. Under each area, list the services/activities and courses of study that support that goal.
                     Include for each service/activity the location, frequency, projected beginning date, anticipated duration, and person/agency responsible.</td>
                </tr>
         
                <tr>
                   <td class="righ">For Individuals in Career and Technology Centers, CIP Code:</td>
                </tr>
                <tr>
                    <td class="righ" >
                        <%--<asp:TextBox ID="TextBoxCIPCode" runat="server" TextMode="MultiLine" Width="60%"></asp:TextBox>--%>
                        <asp:TextBox ID="TextBoxCIPCode_hdn" Text="" runat="server" style="display:none;"></asp:TextBox>
                        <div id="TextBoxCIPCode" runat="server" class="FreeTextDivContent" style="overflow:scroll;" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE5.aspx',this); "></div>
                    </td>
                </tr>
                <tr>
                    <td  class="righ" colspan="2">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" Text="Save and Continue" OnClick="btnSave_Click" OnClientClick=""/>

                       <%-- <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy" OnClick="btnSave_hdn_Click" OnClientClick="" style="display:none;"/>--%>
                    </td>
                </tr>
            </table>
                 <div class="clear"></div>
                 </div>
        </div>
    </form>
</body>
</html>
