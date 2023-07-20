<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE6.aspx.cs" Inherits="StudentBinder_CreateIEP_PE6" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/StylePE.css" rel="stylesheet" />
    <script src="js/jquery-1.8.0.min.js"></script>
    <script src="../Administration/JS/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />
     <link href="CSS/StylePE15.css" rel="stylesheet" />
    <style>


        .FreeTextDivContent {
             background-color: #FFFFFF;
    border: 1px solid #DADAC8;
    border-radius: 3px;
    color: #666666;
    float: left;
    font-size: 11px;
    height: 65px;
    margin: 0 !important;
    min-width: 72px !important;
    padding: 2px 4px;
    width: 92%;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            $('.date').datepicker();
        });
    </script>


    <script type="text/javascript">
        // when dom is ready we initialize the UpdatePanel requests
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
    </script>
    <%--    <script type="text/javascript">
        $(function () {
            $("[id$=txtEndDateA]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../StudentBinder/img/Calendar24.png'
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $("[id$=txtStartDateB]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: "../StudentBinder/img/Calendar24.png"
            });
        });
    </script>--%>


    <script type="text/javascript">
        //TextBoxTrainingGoal TextBoxTrainingCourse TextBoxEmploymentGoal TextBoxEmploymentCourse TextBoxIndependentLivingGoal TextBoxIndependentLivingCourse
        var txt1 = "", txt2 = "", txt3 = "", txt4 = "", txt5 = "", txt6 = "";
        function GetFreetextval(content, divid) {
            if (divid == 'TextBoxTrainingGoal') {
                document.getElementById('<%=TextBoxTrainingGoal.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxTrainingGoal.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxTrainingGoal_hdn.ClientID %>').value = window.escape(content);
                txt1 = content;
            }
            else if (divid == 'TextBoxTrainingCourse') {
                document.getElementById('<%=TextBoxTrainingCourse.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxTrainingCourse.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxTrainingCourse_hdn.ClientID %>').value = window.escape(content);
                txt2 = content;
            }
                //        
            else if (divid == 'TextBoxEmploymentGoal') {
                document.getElementById('<%=TextBoxEmploymentGoal.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxEmploymentGoal.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxEmploymentGoal_hdn.ClientID %>').value = window.escape(content);
                txt3 = content;
            }
            else if (divid == 'TextBoxEmploymentCourse') {
                document.getElementById('<%=TextBoxEmploymentCourse.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxEmploymentCourse.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxEmploymentCourse_hdn.ClientID %>').value = window.escape(content);
                txt4 = content;
            }
            else if (divid == 'TextBoxIndependentLivingGoal') {
                document.getElementById('<%=TextBoxIndependentLivingGoal.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxIndependentLivingGoal.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxIndependentLivingGoal_hdn.ClientID %>').value = window.escape(content);
                txt5 = content;
            }
            else if (divid == 'TextBoxIndependentLivingCourse') {
                document.getElementById('<%=TextBoxIndependentLivingCourse.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxIndependentLivingCourse.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxIndependentLivingCourse_hdn.ClientID %>').value = window.escape(content);
                txt6 = content;
            }
}

function loadValues() {
    if (document.getElementById('<%=TextBoxTrainingGoal.ClientID %>').innerHTML != "") {
        txt1 = document.getElementById('<%=TextBoxTrainingGoal.ClientID %>').innerHTML;
        txt1 = txt1.replace(/'/g, '##');
        txt1 = txt1.replace(/\\/g, '?bs;');
    }
    if (document.getElementById('<%=TextBoxTrainingCourse.ClientID %>').innerHTML != "") {
        txt2 = document.getElementById('<%=TextBoxTrainingCourse.ClientID %>').innerHTML;
        txt2 = txt2.replace(/'/g, '##');
        txt2 = txt2.replace(/\\/g, '?bs;');
    }
    if (document.getElementById('<%=TextBoxEmploymentGoal.ClientID %>').innerHTML != "") {
        txt3 = document.getElementById('<%=TextBoxEmploymentGoal.ClientID %>').innerHTML;
        txt3 = txt3.replace(/'/g, '##');
        txt3 = txt3.replace(/\\/g, '?bs;');
    }
    if (document.getElementById('<%=TextBoxEmploymentCourse.ClientID %>').innerHTML != "") {
        txt4 = document.getElementById('<%=TextBoxEmploymentCourse.ClientID %>').innerHTML;
        txt4 = txt4.replace(/'/g, '##');
        txt4 = txt4.replace(/\\/g, '?bs;');
    }
    if (document.getElementById('<%=TextBoxIndependentLivingGoal.ClientID %>').innerHTML != "") {
        txt5 = document.getElementById('<%=TextBoxIndependentLivingGoal.ClientID %>').innerHTML;
        txt5 = txt5.replace(/'/g, '##');
        txt5 = txt5.replace(/\\/g, '?bs;');
    }
    if (document.getElementById('<%=TextBoxIndependentLivingCourse.ClientID %>').innerHTML != "") {
        txt6 = document.getElementById('<%=TextBoxIndependentLivingCourse.ClientID %>').innerHTML;
        txt6 = txt6.replace(/'/g, '##');
        txt6 = txt6.replace(/\\/g, '?bs;');
    }
}

function submitClick() {
    loadValues();

    $.ajax(
  {

      type: "POST",
      url: "CreateIEP-PE6.aspx/submitIepPE6",
      data: "{'arg1':'" + txt1 + "','arg2':'" + txt2 + "','arg3':'" + txt3 + "','arg4':'" + txt4 + "','arg5':'" + txt5 + "','arg6':'" + txt6 + "'}",
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
}

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
        <asp:ScriptManager ID="scm1" runat="server">
        </asp:ScriptManager>
        <div id="divIEPP6">
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
                     <td class="righ" colspan="3"><h2 class="simble">Post Secondary Education and Training Goal</h2>
                   
                    </td>
                </tr>
                <tr>
                    <td>Post Secondary Education and Training Goal:</td>
                    <td>
                        <%--<asp:TextBox ID="TextBoxTrainingGoal" runat="server" CssClass="textClass"></asp:TextBox>--%>
                        <asp:TextBox runat="server" id="TextBoxTrainingGoal_hdn" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxTrainingGoal" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE6.aspx',this); "></div>

                    </td>
                    <td class="righ">Measurable Annual Goal<br />
                        Yes/No<br />
                        (Document in Section V)
                        <br />
                        <asp:CheckBox ID="ChkMeasure1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Courses of Study: </td>
                    <td class="righ" colspan="2">
                        <%--<asp:TextBox ID="TextBoxTrainingCourse" runat="server" CssClass="textClass"></asp:TextBox>--%>
                        <asp:TextBox runat="server" id="TextBoxTrainingCourse_hdn" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxTrainingCourse" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE6.aspx',this); "></div>

                    </td>
           
                </tr>

                <tr>
                    <td colspan="3" class="righ">


                        <asp:UpdatePanel ID="updPanel1" runat="server">

                            <ContentTemplate>
                                <asp:GridView ID="gvDelTypeA" runat="server" AutoGenerateColumns="False" GridLines="None" 
                                    ShowFooter="True" OnRowDataBound="gvDelTypeA_RowDataBound"
                                    OnRowCommand="gvDelTypeA_RowCommand" Style="z-index: 1" CssClass="gridStyle">

                                    <Columns>
                                        <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service/Activity">
                                            <ItemTemplate>
                                                 <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="tdText top righ ">
                                                            <asp:TextBox ID="txtService" MaxLength="100" CssClass="textClass textfield" runat="server" Text='<%# Eval("Service") %>'></asp:TextBox>
                                           
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                 <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="top righ"style="text-align: center;">
                                                            <asp:TextBox ID="txtLocation" MaxLength="100" runat="server" CssClass="textClass textfield" 
                                                                Text='<%#Eval("Location") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Frequency">
                                            <ItemTemplate>
                                                 <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="tdText top righ" style="text-align: center;">
                                                            <asp:TextBox ID="txtFrequency" MaxLength="100" runat="server" CssClass="textClass textfield"
                                                                Text='<%#Eval("Frequency") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Projected Beginning Date">
                                            <ItemTemplate>
                                                 <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="tdText top righ" style="text-align: center;">
                                                            <asp:TextBox ID="txtBeginningDate" runat="server" class="date"  onkeypress="return false" onpaste="return false"
                                                                Text='<%#Eval("BeginningDate") %>' onkeydown='allowBackSpace(event,this)'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Anticipated Duration">
                                            <ItemTemplate>
                                                 <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="top righ">
                                                            <asp:TextBox ID="txtAnticipatedDuration" MaxLength="100" runat="server"  Text='<%#Eval("AnticipatedDuration") %>' CssClass="textClass"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Person(s)/Agency Responsible" ItemStyle-HorizontalAlign="Right">
                                            <FooterTemplate>
                                                <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAdd_Click"
                                                    Text="Add New Row" CssClass="NFButton" />
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                 <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="top righ">
                                                            <asp:TextBox ID="txtAgencyResponsible" MaxLength="100" runat="server" CssClass="textClass" Text='<%#Eval("AgencyResponsible") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="top righ">
                                                            <asp:LinkButton ID="lnk_Delete0" CssClass="clsbbtn" runat="server" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
         
                <tr>
                    <td>Employment Goal:</td>
                    <td>
                        <%--<asp:TextBox ID="TextBoxEmploymentGoal" runat="server" CssClass="textClass"></asp:TextBox>--%>
                        <asp:TextBox runat="server" id="TextBoxEmploymentGoal_hdn" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxEmploymentGoal" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE6.aspx',this); "></div>
                    </td>
                    <td class="righ">Measurable Annual Goal<br />
                        Yes/No<br />
                        (Document in Section V) <br />
                        <asp:CheckBox ID="ChkMeasure2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Courses of Study: </td>
                    <td class="righ" colspan="2"><%--<asp:TextBox ID="TextBoxEmploymentCourse" runat="server" CssClass="textClass"></asp:TextBox>--%>
                        <asp:TextBox runat="server" id="TextBoxEmploymentCourse_hdn" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxEmploymentCourse" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE6.aspx',this); "></div>

                    </td>

                </tr>
       
                <tr>
                    <td colspan="3" class="righ">

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvDelTypeB" runat="server" AutoGenerateColumns="False" Width="100%"
                                    GridLines="None" ShowFooter="True"
                                    OnRowDataBound="gvDelTypeB_RowDataBound"
                                    OnRowCommand="gvDelTypeB_RowCommand" CssClass="gridStyle">

                                    <Columns>
                                        <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service/Activity">
                                            <ItemTemplate>
                                               <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="tdText top righ">
                                                            <asp:TextBox ID="txtService" MaxLength="100" CssClass="textClass textfield" runat="server"  Text='<%# Eval("Service") %>'></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td style="text-align: center;" class="righ top">
                                                            <asp:TextBox ID="txtLocation" MaxLength="100" runat="server" CssClass="textClass textfield" 
                                                                Text='<%#Eval("Location") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Frequency">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="tdText top righ" style="text-align: center;">
                                                            <asp:TextBox ID="txtFrequency" MaxLength="100" runat="server" CssClass="textClass textfield" 
                                                                Text='<%#Eval("Frequency") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Projected Beginning Date">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="tdText top righ" style="text-align: center;">
                                                            <asp:TextBox ID="txtBeginningDate" runat="server" class="date" Width="100px" onkeypress="return false" onpaste="return paste"
                                                                Text='<%#Eval("BeginningDate") %>'  onkeydown='allowBackSpace(event,this)'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Anticipated Duration">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="top righ">
                                                            <asp:TextBox ID="txtAnticipatedDuration" MaxLength="100" runat="server" Width="80px" Text='<%#Eval("AnticipatedDuration") %>' CssClass="textClass textfield"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Person(s)/Agency Responsible" ItemStyle-HorizontalAlign="Right">
                                            <FooterTemplate>
                                                <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAddB_Click"
                                                    Text="Add New Row" CssClass="NFButton" />
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="top righ">
                                                            <asp:TextBox ID="txtAgencyResponsible" runat="server" CssClass="textClass textfield" Width="80px" Text='<%#Eval("AgencyResponsible") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td class="top righ">
                                                            <asp:LinkButton ID="lnk_Delete0" runat="server" CssClass="clsbbtn" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                </tr>
    

                <tr>
                    <td>Independent Living Goal, if appropriate:</td>
                    <td>
                        <%--<asp:TextBox ID="TextBoxIndependentLivingGoal" runat="server" CssClass="textClass"></asp:TextBox>--%>
                        <asp:TextBox runat="server" id="TextBoxIndependentLivingGoal_hdn" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxIndependentLivingGoal" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE6.aspx',this); "></div>

                    </td>
                    <td class="righ">Measurable Annual Goal<br />
                        Yes/No<br />
                        (Document in Section V) <br />
                        <asp:CheckBox ID="ChkMeasure3" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Courses of Study:</td>
                    <td class="righ" colspan="2">
                        <%--    <asp:TextBox ID="TextBoxIndependentLivingCourse" runat="server" CssClass="textClass"></asp:TextBox>--%>
                        <asp:TextBox runat="server" id="TextBoxIndependentLivingCourse_hdn" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxIndependentLivingCourse" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE6.aspx',this); "></div>
                    </td>
                
                </tr>
           
                <tr>
                    <td colspan="3" class="righ">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvDelTypeC" runat="server" ShowFooter="True" AutoGenerateColumns="False" Width="100%"
                                    GridLines="None" OnRowDataBound="gvDelTypeC_RowDataBound"
                                    OnRowCommand="gvDelTypeC_RowCommand" CssClass="gridStyle">

                                    <Columns>
                                        <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service/Activity">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td class="tdText top righ">
                                                            <asp:TextBox ID="txtService" MaxLength="100" CssClass="textClass textfield" runat="server"  Text='<%# Eval("Service") %>'></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="text-align: center;" class="top righ">
                                                            <asp:TextBox ID="txtLocation" MaxLength="100" runat="server" CssClass="textClass textfield" 
                                                                Text='<%#Eval("Location") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Frequency">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td class="tdText top righ" style="text-align: center;">
                                                            <asp:TextBox ID="txtFrequency" MaxLength="100" runat="server" CssClass="textClass textfield" 
                                                                Text='<%#Eval("Frequency") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Projected Beginning Date">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td class="tdText top righ" style="text-align: center;">
                                                            <asp:TextBox ID="txtBeginningDate" runat="server" class="date" Width="100px" onkeypress="return false" onpaste="return false"
                                                                Text='<%#Eval("BeginningDate") %>' onkeydown='allowBackSpace(event,this)'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Anticipated Duration">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td class="top righ">
                                                            <asp:TextBox ID="txtAnticipatedDuration" CssClass="textClass textfield" MaxLength="100" runat="server"  Text='<%#Eval("AnticipatedDuration") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Person(s)/Agency Responsible" ItemStyle-HorizontalAlign="Right">
                                            <FooterTemplate>
                                                <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAddC_Click"
                                                    Text="Add New Row" CssClass="NFButton" />
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td class="top righ">
                                                            <asp:TextBox ID="txtAgencyResponsible" MaxLength="100" runat="server" CssClass="textClass textfield" Width="80px" Text='<%#Eval("AgencyResponsible") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td class="top righ">
                                                            <asp:LinkButton ID="lnk_Delete0" runat="server" CssClass="clsbbtn" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>


                    </td>
                </tr>

                <tr>
                    <td class="righ" colspan="3">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" Text="Save and Continue" OnClick="btnSave_Click" OnClientClick="submitClick()"/>

                        <%--<asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy" OnClick="btnSave_hdn_Click" OnClientClick="submitClick()" style="display:none;"/>--%>
                    </td>
                </tr>
            </table>
                  <div class="clear"></div>
                 </div>
        </div>
    </form>
</body>
</html>
