<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="StudentMenu.aspx.cs" Inherits="Admin_StudentMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#close_x').click(function () {
                $('#dialog').animate({ top: "-300%" }, function () {
                    $('#overlay').fadeOut('slow');
                });
            });
        });
    </script>
    <script type="text/javascript">

        function deleteSystem() {
            var flag;
            flag = confirm("Are you sure you want to delete this Student ?");
            return flag;
        }

    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <table width="99%">
        <tr>
            <td width="30%" id="tdMsg" align="center" colspan="2" runat="server">&nbsp;</td>
        </tr>
        <tr>
            <td align="left" class="" width="70%">
                 <table style="width: 35%">

                            <tr>
                                <td class="tdText" style="padding-right: 13px">Name</td>
                                <td>
                                    <asp:TextBox ID="TextBox_StudentName" runat="server" CssClass="textClass" MaxLength="30"></asp:TextBox></td>
                                <td>
                                    <asp:Button ID="Button_Search" runat="server" CssClass="btn btn-orange" OnClick="Button_Search_Click" Text="" /></td>
                                <td>

                                   <asp:Button ID="btnAdd" runat="server" CssClass="NFButton" OnClick="btnAdd_Click" Text="Add" />

                                </td>
                            </tr>

                        </table>

            </td>
            <td width="30%" align="right">
                 

                        <asp:HiddenField ID="HdFldActiveInactive" runat="server" />

                <table style="width:0;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" style="width: 25%" valign="bottom">
                            <asp:LinkButton ID="linkActive" runat="server" OnClick="linkActive_Click" Visible="False">Active</asp:LinkButton>
                        </td>
                        <td>|</td>
                        <td align="left" valign="bottom">
                            <asp:LinkButton ID="lnkInactive" runat="server" OnClick="lnkInactive_Click" Visible="False">InActive</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:GridView ID="GV_Student" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    AllowPaging="True" OnRowDeleting="GV_Student_RowDeleting"
                    OnRowUpdating="GV_Student_RowUpdating" Width="100%" OnRowCommand="GV_Student_RowCommand"
                    OnPageIndexChanging="GV_Student_PageIndexChanging" 
                    OnRowDataBound="GV_Student_RowDataBound" EmptyDataText="No Data Found..."
                    GridLines="none">

                                          <Columns>
                                <asp:BoundField DataField="StudentNbr" HeaderText="Student No:" SortExpression="StudentNbr" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                    FooterStyle-Width="10%"></asp:BoundField>
                                <asp:BoundField HeaderText="Last Name" DataField="StudentLname" SortExpression="StudentLname" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                    FooterStyle-Width="20%" />
                                <asp:BoundField DataField="StudentFname" HeaderText="First Name" SortExpression="StudentFname" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                    FooterStyle-Width="20%" />
                                              <asp:BoundField HeaderText="Middle Name" DataField="MiddleName" SortExpression="MiddleName" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                    FooterStyle-Width="20%" />
                                <asp:BoundField DataField="ModifiedUser" HeaderText="Modified By" SortExpression="ModifiedUser" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                    FooterStyle-Width="20%" />
                                <asp:BoundField DataField="ModifiedDate" HeaderText="Modified On" SortExpression="ModifiedDate" DataFormatString="{0:d}" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                    FooterStyle-Width="10%" />
                                <asp:TemplateField HeaderText="Intake Assessment" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                    FooterStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkIntake" runat="server" CommandArgument='<%# Eval("StudentId") %>'
                                            CommandName="IntakeVal" Font-Underline="True" ForeColor="#0066FF">Complete Record</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                    FooterStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lnkIntake1" runat="server" CommandArgument='<%# Eval("StudentId") %>'
                                        ImageUrl="~/Administration/images/user_edit.png" class="btn btn-blue"    CommandName="Edit" Font-Underline="True" ForeColor="#0066FF"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="clickMe" runat="server" Style="cursor: pointer;" CommandName="View"
                                            Height="20px" Width="18px" ImageUrl="~/Administration/Images/view_02.png" class="btn btn-purple"
                                            CommandArgument='<%# Eval("StudentId") %>' AlternateText="View" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lb_Edit" CommandName="Edit" runat="server" CommandArgument='<%# Eval("StudentId") %>'
                                            ImageUrl="~/Administration/images/user_edit.png" class="btn btn-blue" AlternateText="Edit"
                                            Height="20px" Width="18px"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lb_delete" OnClientClick="javascript:return deleteSystem();"
                                            CommandName="Delete" runat="server" CommandArgument='<%# Eval("StudentId") %>'
                                            ImageUrl="~/Administration/images/trash.png" class="btn btn-red" AlternateText="Delete"
                                            Height="20px" Width="18px" ></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                    <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                    
                    <RowStyle CssClass="RowStyle"  />
                    <AlternatingRowStyle CssClass="AltRowStyle"  />
                    <FooterStyle CssClass="FooterStyle"  ForeColor="#333333" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="PagerStyle" BackColor="#ccccff" HorizontalAlign="Center" />
                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class="tdText" colspan="2">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center">
               
                    &nbsp;</td>
        </tr>
    </table>
    <div id="overlay" class="web_dialog_overlay">
    </div>
   <div id="dialog" class="web_dialog" style="width: 711px;">
                <div id="sign_up5">
                    <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                        <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                    <h3>Details Of
                <asp:Label ID="lblStudentHead" runat="server" Text=""></asp:Label></h3>
                    <hr />



                    <table cellpadding="0" cellspacing="5" style="text-align: left;" width="100%">
                        <tr>
                            <td class="tdText" width="20%">Student Number</td>
                            <td runat="server" id="lblNumber" width="30%" class="tdText">
                                <asp:Label ID="lblNumber1" runat="server" Text=""></asp:Label></td>
                            <td class="tdText" width="20%">Student Name</td>
                            <td class="tdText" width="20%">
                                <asp:Label ID="lblStudent" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdText">Address</td>
                            <td class="tdText">
                                <asp:Label ID="lblAddr" runat="server" Text=""></asp:Label></td>
                            <td class="tdText">Gender</td>
                            <td class="tdText">
                                <asp:Label ID="lblGender" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="tdText">DOB</td>
                            <td class="tdText">
                                <asp:Label ID="lblDob" runat="server" Text=""></asp:Label></td>
                            <td class="tdText">Joining Date</td>
                            <td class="tdText">
                                <asp:Label ID="lblJoin" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="tdText">City</td>
                            <td class="tdText">
                                <asp:Label ID="lblCity" runat="server"></asp:Label>
                            </td>
                            <td class="tdText">Grade</td>
                            <td class="tdText">
                                <asp:Label ID="lblGrade" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="tdText">Country</td>
                            <td class="tdText">
                                <asp:Label ID="lblCountry" runat="server"></asp:Label>
                            </td>
                            <td class="tdText">State</td>
                            <td class="tdText">
                                <asp:Label ID="lblState" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="tdText">Zip</td>
                            <td class="tdText">
                                <asp:Label ID="lblZip" runat="server"></asp:Label>
                            </td>
                            <td class="tdText">E-Mail</td>
                            <td class="tdText">
                                <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="tdText">Phone</td>
                            <td class="tdText">
                                <asp:Label ID="lblPhone" runat="server"></asp:Label>
                            </td>
                            <td class="tdText">Mobile</td>
                            <td class="tdText">
                                <asp:Label ID="lblMobile" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="tdText">&nbsp;Modified By</td>
                            <td class="tdText">
                                <asp:Label ID="lblModifiedBy" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="tdText">Modified On
                            </td>
                            <td class="tdText">
                                <asp:Label ID="lblModifiedOn" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdText">&nbsp;</td>
                            <td class="tdText">&nbsp;</td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>
</asp:Content>
