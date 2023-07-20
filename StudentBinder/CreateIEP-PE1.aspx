<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="CreateIEP-PE1.aspx.cs" Inherits="StudentBinder_CreateIEP_PE1" ValidateRequest="false" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<meta http-equiv="X-UA-Compatible" content="IE=9" />--%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <%--<script src="js/jquery-1.8.0.min.js"></script>
    <link href="CSS/StylePE.css" rel="stylesheet" />

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
    <script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />--%>

     <script src="js/jquery-1.8.0.min.js"></script>
    <script src="../Administration/JS/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />
     <link href="CSS/StylePE15.css" rel="stylesheet" />

     <script type="text/javascript">
         $(function () {
             $('.date').datepicker();
         });
    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);

            // Place here the first init of the DatePicker           
            $('.date').datepicker();
        });

        function InitializeRequest(sender, args) {
            // make unbind before update the dom, to avoid memory leaks.
            $('.date').unbind();
        }

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the DatePicker
            $('.date').datepicker();
        }

        

        function submitClick() {
            loadValues();
            $.ajax(
          {

              type: "POST",
              url: "CreateIEP-PE1.aspx/submitIepPE1",
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


        function loadValues() {
            if (document.getElementById('<%=txtOtherInformation.ClientID %>').innerHTML != "") {
                txt1 = document.getElementById('<%=txtOtherInformation.ClientID %>').innerHTML;

                txt1 = txt1.replace(/'/g, '##');
                txt1 = txt1.replace(/\\/g, '?bs;');
            }
            if (document.getElementById('<%=TextBoxDocumentedBy.ClientID %>').innerHTML != "") {
                text2 = document.getElementById('<%=TextBoxDocumentedBy.ClientID %>').innerHTML;
                text2 = text2.replace(/'/g, '##');
                text2 = text2.replace(/\\/g, '?bs;');
            }
        }

    </script>

    <style type="text/css">
        .FreeTextDivContent {
           background-color: #FFFFFF;
    border: 1px solid #DADAC8;
    border-radius: 3px;
    color: #666666;
    float: left;
    font-size: 11px;
    height: 65px;
    margin: 0 0 0 5px;
    min-width: 72px !important;
    padding: 2px 4px;
    width: 92%;
        }
    </style>

    <style type="text/css">
        .ui-datepicker {
            font-size: 8pt !important;
        }
    </style>
    <script type="text/javascript">
        var txt1 = "", text2 = "";
        function GetFreetextval(content, divid) {
            if (divid == 'txtOtherInformation') {
                document.getElementById('<%=txtOtherInformation.ClientID %>').innerHTML = "";
                document.getElementById('<%=txtOtherInformation.ClientID %>').innerHTML = content;
                document.getElementById('<%=hdnOtherInformationTxt.ClientID %>').value = window.escape(content);
                txt1 = content;

            }
            else if (divid == 'TextBoxDocumentedBy') {
                document.getElementById('<%=TextBoxDocumentedBy.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxDocumentedBy.ClientID %>').innerHTML = content;
                document.getElementById('<%=hdnFieldDocByText.ClientID %>').value = window.escape(content);
                text2 = content;


            }

    }



    $(function () {
        $('#TextBoxMeetingDate').datepicker({
            showOn: 'button',
            buttonImageOnly: true,
            buttonImage: '../StudentBinder/img/Calendar24.png'
        });
        $('#TextBoxImplementationDate').datepicker({
            showOn: 'button',
            buttonImageOnly: true,
            buttonImage: '../StudentBinder/img/Calendar24.png'
        });
        $('#TextBoxGraduationYear').datepicker({
            showOn: 'button',
            buttonImageOnly: true,
            buttonImage: '../StudentBinder/img/Calendar24.png'
        });
       
        
    });

    function scrollToTop() {
        window.scrollTo(0, 0);
        window.parent.parent.scrollTo(0, 100);
    }
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
        <div>
        </div>
         <asp:ScriptManager ID="scm1" runat="server">
        </asp:ScriptManager>
        <div id="divIEPP1">
<div class="ContentAreaContainer">
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
                    <td colspan="2" class="righ">Individual's Name : <asp:Label runat="server" id="lblStudName"></asp:Label></td>
                </tr>
  
                <tr>
                   <td class="righ" colspan="2">
                     <h2 class="simble">INDIVIDUALISED EDUCATION PROGRAM(IEP)</h2></td>
                   
                </tr>
                <tr>
                    <td class="tdText">Individual's Name:</td>
                    <td class="righ">&nbsp;<asp:Label ID="lblStudentName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">IEP Team Meeting Date (mm/dd/yy):</td>
                    <td class="righ">
                        <asp:TextBox ID="TextBoxMeetingDate" CssClass="textfield" runat="server" onkeypress="return false" onpaste="return false" onkeydown='allowBackSpace(event,this)'></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdText">IEP Implementation Date (Projected Date when Services and Programs Will Begin):</td>
                    <td class="righ">
                        <asp:TextBox ID="TextBoxImplementationDate" CssClass="textfield" runat="server" onkeypress="return false" onpaste="return false" onkeydown='allowBackSpace(event,this)'></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdText">Anticipated Duration of Services and Programs:</td>
                    <td class="righ">
                        <asp:TextBox ID="TextBoxSeviceDuration" CssClass="textfield" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdText">Date of Birth:</td>
                    <td class="righ">
                        <asp:Label ID="lblDOB" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Age:</td>
                    <td class="righ">
                        <asp:Label ID="lblAge" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Grade:</td>
                    <td class="righ">
                        <asp:Label ID="lblGrade" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Anticipated Year of Graduation:</td>
                    <td class="righ">
                        <asp:TextBox ID="TextBoxGraduationYear" CssClass="textfield" runat="server" onkeypress="return false" onpaste="return false" onkeydown='allowBackSpace(event,this)'></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdText">Local Education Agency (LEA):</td>
                    <td class="righ">
                        <asp:TextBox ID="TextBoxLEA" CssClass="textfield" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdText" >County of Residence:</td>
                   <td class="righ">
                        <asp:TextBox ID="TextBoxResidenceCountry" CssClass="textfield" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdText">Name and Address of Parent/Guardian/Surrogate:</td>
                    <td class="righ">
                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Phone (Home):</td>
                    <td class="righ">
                        <asp:Label ID="lblPhoneHome" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Phone (Work):</td>
                    <td class="righ">
                        <asp:Label ID="lblPhoneWork" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Other Information:</td>
                    <td class="righ">
                        <%--<asp:TextBox ID="txtOtherInformation" runat="server" Height="50px" TextMode="MultiLine" Width="100%"></asp:TextBox>--%>
                        <asp:HiddenField ID="hdnFieldOther" runat="server" />
                        <asp:TextBox ID="hdnOtherInformationTxt" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="txtOtherInformation" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE1.aspx',this); "></div>
                    </td>
                </tr>
           

                <tr>
                    <td colspan="2" class="tdText righ" >The LEA and parent have agreed to make the following changes to the IEP without convening an IEP meeting, as documented by:</td>
                </tr>
                <tr>
                    <td colspan="2" class="righ">
                        <%-- <asp:TextBox ID="TextBoxDocumentedBy" runat="server" Height="50px" TextMode="MultiLine" Width="100%"></asp:TextBox>--%>
                        <asp:HiddenField ID="hdnFieldDocBy" runat="server" />
                          <asp:TextBox id="hdnFieldDocByText" runat="server" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxDocumentedBy" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE1.aspx',this); "></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="top righ"></td>
                </tr>
                <tr>
                    <td colspan="2" class="tdText top righ">
                       <%-- <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>--%>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvDelTypeA" runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%"
                                    ShowFooter="True" OnRowDataBound="gvDelTypeA_RowDataBound"
                                    OnRowCommand="gvDelTypeA_RowCommand" Style="z-index: 1" CssClass="gridStyle">
                                    <HeaderStyle CssClass="HeaderStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("IEPPA1ExtensionId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Width="15px">
                                            <HeaderTemplate>
                                                Date of Revision(s)                                    
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRevisionDate" class="date" runat="server" onkeypress="return false" Text='<%# Eval("DateOfRevisions") %>' onpaste="return false" onkeydown='allowBackSpace(event,this)'></asp:TextBox>

                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ControlStyle-Width="50px">
                                            <HeaderTemplate>
                                                Participants/Roles                                   
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:TextBox ID="txtRoles" CssClass="textClass" Width="310px" runat="server" Text='<%# Eval("Participants") %>'></asp:TextBox>

                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                IEP Section(s) Amended                                    
                                            </HeaderTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAdd_Click"
                                                    Text="Add New Row" CssClass="NFButton" />
                                            </FooterTemplate>
                                            <ItemTemplate>

                                                <asp:TextBox ID="txtSection" Width="310px" CssClass="textClass" runat="server" Text='<%# Eval("IEPSections") %>'></asp:TextBox>

                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Delete                
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:LinkButton ID="lnk_Delete0" class="clsbbtn" runat="server" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>

                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                </tr>
    
                <tr>
                    <td class="top righ"  colspan="2">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_Click" Text="Save and continue" OnClientClick="" />

<%--                        <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_hdn_Click" Text="dummy" OnClientClick="" style="display:none;"/>--%>
                    </td>
                </tr>
            </table>
    <div class="clear"></div>
    </div>
        </div>
    </form>
</body>
</html>
