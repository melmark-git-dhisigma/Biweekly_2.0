<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/CreateIEP1.aspx.cs" Inherits="StudentBinder_CreateIEP1"  %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
     <script src="../Administration/JS/jquery-1.8.0.js" type="text/javascript"></script>
   
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
            var txtConcerns = document.getElementById('<%=txtConcerns.ClientID %>').innerHTML;
            var txtStrengths = document.getElementById('<%=txtStrengths.ClientID %>').innerHTML;
            var txtVision = document.getElementById('<%=txtVision.ClientID %>').innerHTML;     
           
            PageMethods.submitIEP1(txtConcerns, txtStrengths, txtVision, sucess);
         
        }
        function submitClick_hdn() {
            var txtConcerns = document.getElementById('<%=txtConcerns.ClientID %>').innerHTML;
               var txtStrengths = document.getElementById('<%=txtStrengths.ClientID %>').innerHTML;
               var txtVision = document.getElementById('<%=txtVision.ClientID %>').innerHTML;
               PageMethods.submitIEP1_hdn(txtConcerns, txtStrengths, txtVision);

           }
        function sucess(result) {
            parent.CreateIEP2();
        }
        function GetFreetextval(content,divid) {
            if (divid == 'txtConcerns') {
                document.getElementById('<%=txtConcerns.ClientID %>').innerHTML = "";
                document.getElementById('<%=txtConcerns.ClientID %>').innerHTML = content;      
                document.getElementById('<%=txtConcerns_hdn.ClientID %>').value = window.escape(content);
            }
            else if (divid == 'txtStrengths') {
                document.getElementById('<%=txtStrengths.ClientID %>').innerHTML = "";
                document.getElementById('<%=txtStrengths.ClientID %>').innerHTML = content;
                document.getElementById('<%=txtStrengths_hdn.ClientID %>').value = window.escape(content);
            }
            else if (divid == 'txtVision') {
                document.getElementById('<%=txtVision.ClientID %>').innerHTML = "";
                document.getElementById('<%=txtVision.ClientID %>').innerHTML = content;
                document.getElementById('<%=txtVision_hdn.ClientID %>').value = window.escape(content);
            }

        }

        function chkLen() {
            var val = document.getElementById('txtConcerns').value;
            var val1 = document.getElementById('txtStrengths').value;
            var val2 = document.getElementById('txtVision').value;
            if ((parseInt(val.length) < 2500) && (parseInt(val1.length) < 2500) && (parseInt(val2.length) < 2500)) {
                return true
            }
            else {
                tdMsg.innerHTML='<div class=error_box>Text in Editor should be less than 200 charecters.</div>'
                return false;
            }
        }

        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0,100);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">


       
        <div style="height:40px;">


        </div>
        <div id="divIEP1" style="width: 97%; padding: 10px; border-radius: 3px 3px 3px 3px;">
            <table cellpadding="0" cellspacing="0" width="96%">
                <tr>
                    <td style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; height: 25px;">
                        Individualized Education Program
                    </td>
                </tr>
                <tr>
                    <td width="85%">
                        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
                        </ajax:ToolkitScriptManager>
                    </td>
                </tr>
                <tr>
                    <td width="85%" runat="server" id="tdMsg"></td>
                </tr>

                <tr>
                    <td align="left" class="tdTextLeft">&nbsp;</td>
                </tr>
                <tr style="height:30px;">
                    <td class="tdText" style="text-align:center;"><b>Parent and/or Student Concerns</b><br />What concern(s) does the parent and/or student want to see addressed in this IEP to enhance the student's education?
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtConcerns_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtConcerns" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP1.aspx',this);" ></div>
                        
                    </td>
                </tr>

                <tr>
                    <td class="tdTextLeft">&nbsp;</td>
                </tr>
                <tr style ="height:30px;">
                    <td class="tdText" style="text-align:center;"><b>Student Strengths and Key Evaluation Results Summary</b><br /> What are the student’s educational strengths, interest areas, significant personal attributes and personal accomplishments?<br />
 What is the student’s type of disability(ies), general education performance<br />
 including MCAS/district test results, achievement towards goals and lack of expected progress, if any?

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtStrengths_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtStrengths" runat="server" class="FreeTextDivContent" onclick=" scrollToTop(); parent.freeTextPopup('CreateIEP1.aspx',this);"></div>
                        
                       


                    </td>
                </tr>
                <tr>
                    <td class="tdTextLeft">&nbsp;</td>
                </tr>
                <tr style ="height:30px;">
                    <td class="tdText" style="text-align:center;"><b>Vision Statement:</b>&nbsp;&nbsp;What is the vision of this student<br />Consider the next 1 to 5 year period when developing this statement. Beginning no later than age 14,<br />
the statement should be based on the student’s preferences and interest,<br />
and should include desired outcomes in adult living, post-secondary and working environments.

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtVision_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                         <div id="txtVision" runat="server" class="FreeTextDivContent" onclick=" scrollToTop(); parent.freeTextPopup('CreateIEP1.aspx',this);"></div>
                        
                    </td>
                </tr>
                <tr>
                    <td class="tdText">&nbsp;</td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Button ID="btnSubmitIEP1" runat="server" CssClass="NFButtonWithNoImage" Text="Save and continue" OnClick="btnSubmitIEP1_Click1" />    
                                          
                         <%--<asp:Button ID="btnSubmitIEP1_hdn" runat="server" CssClass="NFButtonWithNoImage" 
                            OnClientClick="submitClick_hdn();" Text="Dummy" style="display:none;"/>  --%>    
                    </td>

                </tr>
                <tr>
                    <td class="tdText">
                        <asp:HiddenField ID="hidIEPId" runat="server" />                        
                    </td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
