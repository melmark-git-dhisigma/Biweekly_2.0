<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/CreateIEP12.aspx.cs" Inherits="StudentBinder_CreateIEP12" %>

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


    <style type="text/css">
        .FreeTextDivContent {
            width: 98%;
            min-height: 200px;
            height: 200px;
            padding: 2%;
            border: 1px solid rgb(228, 228, 228);
            border-radius: 8px 8px 8px 8px;
            overflow:auto;
        }

        .ui-datepicker {
            font-size: 8pt !important;
        }

        .tdText12 {
    color: #666;
    font-family: Arial;
    font-size: 15px;
    height: 25px;
    line-height: 12px;
    padding-right: 1px;
    text-align: left;
}
    </style>
    <script type="text/javascript">



        function submitClick() {

            var txtRejectedPortions = document.getElementById('<%=txtRejectedPortions.ClientID %>').innerHTML;
            var txtParentComment = document.getElementById('<%=txtParentComment.ClientID %>').innerHTML;

            PageMethods.submitIEP12(txtRejectedPortions, txtParentComment);
        }
        function GetFreetextval(content, divid) {
            if (divid == 'txtRejectedPortions') {
                document.getElementById('<%=txtRejectedPortions.ClientID %>').innerHTML = "";
                document.getElementById('<%=txtRejectedPortions.ClientID %>').innerHTML = content;
                document.getElementById('<%=txtRejectedPortions_hdn.ClientID %>').value = window.escape(content);
                }
                else if (divid == 'txtParentComment') {
                    document.getElementById('<%=txtParentComment.ClientID %>').innerHTML = "";
                    document.getElementById('<%=txtParentComment.ClientID %>').innerHTML = content;
                    document.getElementById('<%=txtParentComment_hdn.ClientID %>').value = window.escape(content);
                }
        }

        function chkLen() {
            var val = document.getElementById('txtRejectedPortions').value;
            var val1 = document.getElementById('txtParentComment').value;

            if ((parseInt(val.length) < 200) && (parseInt(val1.length) < 200)) {
                return true
            }
            else {
                tdMsg.innerHTML = '<div class=error_box>Text in Editor should be less than 200 charecters.</div>'
                return false;
            }
        }

        </script>
    
        
      <script type="text/javascript">
          $(function () {
              $("[id$=dteSigRep]").datepicker({
                  showOn: 'button',
                  buttonImageOnly: true,
                  buttonImage: '../StudentBinder/img/Calendar24.png'
              });
          });
    </script>
    <script type="text/javascript">
        $(function () {
            $("[id$=dteSigPrnt]").datepicker({
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








    <title></title>

</head>
<body>
    <form id="form1" runat="server">



        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />

        <div id="diviep12" style="width: 97%;height:auto; border-radius: 3px 3px 3px 3px; padding: 7px;">
            <table cellpadding="0" cellspacing="0" width="97%">
                
                <tr>
                    <td colspan="3" style="border-bottom: 2px double gray; text-align: center; padding-top: 5px; padding-bottom: 6px; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; height: 25px;">Response Section<br />
                    </td>
                </tr>
               

                <tr>
                    <td runat="server" id="tdMsg" style="color: #FF0000" colspan="3"></td>

                </tr>
                
                <tr>
                    <td class="tdText12" colspan="3"  style="text-align: center;background-color:#E3E3E3">
                       <strong> School Assurance</strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><br />I certify that the goals in this IEP are those recommended by the Team and that the indicated services will be provided. </td>
                </tr>
                <tr>
                    <td  colspan="2">Signature and Role of LEA Representative&nbsp;&nbsp;<asp:TextBox ID="txtSigRep" runat="server"></asp:TextBox>
                    </td>

                    <td>Date<asp:TextBox ID="dteSigRep" runat="server" Style="width:135px;" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" ></asp:TextBox> <br /> </td>

                </tr>
                <tr>
                    <td class="tdText12" colspan="3"  style="text-align: center;background-color:#E3E3E3">
                        <strong> Options / Responses</strong>
                    </td>

                </tr>
                <tr>
                    <td colspan="3" style="font-weight: bold"><br />It is important that the district knows your decision as soon as possible. Please indicate your response by checking at least one (1) box and returning a signed copy to the district. Thank you.<br /></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="acceptIepDeveloped" runat="server" Text="I accept the IEP as developed." />
                    </td>
                    <td>
                        <asp:CheckBox ID="rejectIepDeveloped" runat="server" Text="I reject the IEP as developed" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:CheckBox ID="deleteFollowingPortions" runat="server"
                            
                            Text="I reject the following portions of the IEP with the understanding that any portion(s) that I do not reject will be considered accepted and implemented immediately. Rejected portions are as follows<br />" CssClass="checkBoxStyle" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:TextBox ID="txtRejectedPortions_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtRejectedPortions" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP12.aspx',this); "></div>



                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <br />
                        <asp:CheckBox ID="RejectionMeeting" runat="server"
                            
                            Text="I request a meeting to discuss the rejected IEP or rejected portion(s).<br />" CssClass="checkBoxStyle" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Signature of Parent, Guardian, Educational Surrogate Parent, Student 18 and Over*<asp:TextBox ID="txtSigPrnt" runat="server"></asp:TextBox>
                    </td>
                    <td>Date<asp:TextBox ID="dteSigPrnt" runat="server" Style="width: 135px;" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                        <br />
                    </td>

                </tr>
                <tr>

                    <td colspan="2">
                        <br />
                        *Required signature once a student reaches 18 unless there is a court appointed guardian.
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                        Parent Comment: I would like to make the following comment(s) but realize any comment(s) made that suggest changes to the proposed IEP will not be implemented unless the IEP is amended.<br /></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:TextBox ID="txtParentComment_hdn" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtParentComment" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP12.aspx',this); "></div>



                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" colspan="3" >&nbsp;
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
