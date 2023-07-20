<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE11.aspx.cs" Inherits="StudentBinder_CreateIEP_PE11" %>

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
        input[type=text] {
            width: 100% !important;
        }
    </style>

       <script type="text/javascript">
           // when dom is ready we initialize the UpdatePanel requests
           $(document).ready(function () {
              
               // Place here the first init of the DatePicker           
               $('.date').datepicker();
           });

           
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
        <div id="divIEPP1">
            <div class="ContentAreaContainer">
                 <br />
                 <div class="clear"></div>
           <table cellpadding="0" cellspacing="0" width="96%">
                 <tr>
                    <td id="tdMsg" style="color:red;" runat="server" class="top righ"></td>
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
                 <td class="righ"><h2 class="simble">Special Considerations</h2>
                    </td>
					</tr>
                <tr>
                    <td style="line-height: 20px;" class="righ">VI. SPECIAL EDUCATION / RELATED SERVICES / SUPPLEMENTARY AIDS AND SERVICES / PROGRAM MODIFICATIONS – Include, as appropriate, for nonacademic
                     and extracurricular services and activities.</td>
                </tr>
                <tr>
                    <td class="auto-style1 righ">&nbsp;</td>
                </tr>
                <tr>
                    <td style="line-height: 20px;" class="righ">A. PROGRAM MODIFICATIONS AND SPECIALLY DESIGNED INSTRUCTION (SDI)
                        <ul style="list-style: disc outside;">

                            <li>SDI may be listed with each goal or as part of the table below.</li>
                            <li>Include supplementary aids and services as appropriate.</li>
                            <li style="line-height: 20px;">For a Individual who has a disability and is gifted, SDI also should include adaptations, accommodations, or modifications to the general education curriculum,as appropriate for a Individual with a disability.</li>

                        </ul>
                    </td>
                </tr>
               
                <tr>
                    <td class="righ">

                        <asp:GridView ID="GridViewModSDI" runat="server" ShowFooter="True" Style="border: 0;" AutoGenerateColumns="False" EmptyDataText="No Data Found..." Width="100%" Font-Bold="False" GridLines="None" OnRowCommand="GridViewModSDI_RowCommand" OnRowDeleting="GridViewModSDI_RowDeleting" CellPadding="2" CellSpacing="2" OnRowDataBound="GridViewModSDI_RowDataBound" CssClass="gridStyle">
                            <Columns>
                                <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Modifications and SDI">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td class="tdText top righ">
                                                    <asp:TextBox ID="TxtModsdi" CssClass="textClass" MaxLength="100" runat="server" Width="150px" Text='<%# Eval("Modsdi") %>'></asp:TextBox>
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
                                                    <asp:TextBox ID="TxtLocation" runat="server" MaxLength="100" CssClass="textClass" Width="120px"
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
                                                    <asp:TextBox ID="TxtFrequency" runat="server" MaxLength="100" CssClass="textClass" Width="120px"
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
                                                    <asp:TextBox ID="TxtPrjBegDate" runat="server" class="date" Width="100px" onkeypress="return false" Text='<%#Eval("PrjBegDate") %>' onpaste="return false" onkeydown='allowBackSpace(event,this)'></asp:TextBox>
                                                        Text='<%#Eval("PrjBegDate") %>'></asp:TextBox>
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
                                                    <asp:TextBox ID="TxtAnticipatedDue" runat="server" MaxLength="100" Width="80px" Text='<%#Eval("AnticipatedDue") %>' CssClass="textClass"></asp:TextBox>
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
                                                <td class="righ top">
                                                    <asp:LinkButton ID="lnk_Delete0" runat="server" CssClass="clsbbtn" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                           <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAdd_Click"
                                                    Text="Add New Row" CssClass="NFButton" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="HeaderStyle" />
                                </asp:TemplateField>
                            </Columns>


                        </asp:GridView>

                    </td>
                </tr>
                
                <tr>
                    <td class="righ" style="line-height: 20px;">B. RELATED SERVICES – List the services that the Individual needs in order to benefit from his/her special education program.</td>
                </tr>
                
                <tr>
                    <td class="righ">
                        <asp:GridView ID="GridViewRelServ" runat="server" ShowFooter="True" Style="border: 0;" AutoGenerateColumns="False" EmptyDataText="No Data Found..." Width="100%" Font-Bold="False" GridLines="None" OnRowCommand="GridViewRelServ_RowCommand" OnRowDeleting="GridViewRelServ_RowDeleting" CellPadding="2" CellSpacing="2" OnRowDataBound="GridViewRelServ_RowDataBound" CssClass="gridStyle">
                            <Columns>
                                <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Service">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td class="tdText righ">
                                                    <asp:TextBox ID="TxtService" CssClass="textClass" MaxLength="100" runat="server" Width="150px" Text='<%# Eval("Service") %>'></asp:TextBox>
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
                                                    <asp:TextBox ID="TxtLocation" runat="server" MaxLength="100" CssClass="textClass" Width="120px"
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
                                                    <asp:TextBox ID="TxtFrequency" runat="server" MaxLength="100" CssClass="textClass" Width="120px"
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
                                                    <asp:TextBox ID="TxtPrjBegDate" runat="server" class="date" Width="100px" onkeypress="return false"
                                                        Text='<%#Eval("PrjBegDate") %>' onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>'
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HeaderStyle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Anticipated Duration">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td class="top righ">
                                                    <asp:TextBox ID="TxtAnticipatedDue" runat="server" MaxLength="100" Width="80px" Text='<%#Eval("AnticipatedDue") %>' CssClass="textClass"></asp:TextBox>
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
                                    <FooterTemplate>
                                         <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAddB_Click"
                                                    Text="Add New Row" CssClass="NFButton" />
                                    </FooterTemplate>
                                     <HeaderStyle CssClass="HeaderStyle" />
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </td>
                </tr>
                

                <tr>
                    <td style="line-height: 20px;" class="righ">C. SUPPORTS FOR SCHOOL PERSONNEL – List the staff to receive the supports and the supports needed to implement the Individual’s IEP.</td>
                </tr>

               

                <tr>
                    <td class="righ">

                        <asp:GridView ID="GridViewSpprtSchol" runat="server" ShowFooter="True" Style="border: 0;" AutoGenerateColumns="False" EmptyDataText="No Data Found..." Width="100%" Font-Bold="False" GridLines="None" OnRowCommand="GridViewSpprtSchol_RowCommand" OnRowDeleting="GridViewSpprtSchol_RowDeleting" CellPadding="2" CellSpacing="2" OnRowDataBound="GridViewSpprtSchol_RowDataBound" CssClass="gridStyle">
                     

                            <Columns>
                                <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="School Personnel to Receive Support">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td class="tdText top righ">
                                                    <asp:TextBox ID="TxtScholPer" CssClass="textClass" MaxLength="100" runat="server" Width="150px" Text='<%# Eval("ScholPer") %>'></asp:TextBox>
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                     <HeaderStyle CssClass="HeaderStyle" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Support">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td style="text-align: center;" class="righ top">
                                                    <asp:TextBox ID="TxtService" runat="server" MaxLength="100" CssClass="textClass" Width="120px"
                                                        Text='<%#Eval("Support") %>'></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                     <HeaderStyle CssClass="HeaderStyle" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td style="text-align: center;" class="righ top">
                                                    <asp:TextBox ID="TxtLocation" runat="server" MaxLength="100" CssClass="textClass" Width="120px"
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
                                                    <asp:TextBox ID="TxtFrequency" runat="server" MaxLength="100" CssClass="textClass" Width="120px"
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
                                                    <asp:TextBox ID="TxtPrjBegDate" runat="server" class="date" Width="100px" onkeypress="return false"
                                                        Text='<%#Eval("PrjBegDate") %>' onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
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
                                                    <asp:TextBox ID="TxtAnticipatedDue" runat="server" MaxLength="100" Width="80px" Text='<%#Eval("AnticipatedDue") %>' CssClass="textClass"></asp:TextBox>
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
                                                <td class="righ top">
                                                    <asp:LinkButton ID="lnk_Delete0" runat="server" CssClass="clsbbtn" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                           <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAddC_Click"
                                                    Text="Add New Row" CssClass="NFButton" />
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="HeaderStyle" />
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </td>
                </tr>

                
                <tr>
                    <td class="righ">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_Click" Text="Save and continue" />

                         <%-- <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_hdn_Click" Text="dummy"  style="display:none;"/>--%>
                    </td>
                </tr>
            </table>
                  <div class="clear"></div>
                 </div>
        </div>
    </form>
</body>
</html>
