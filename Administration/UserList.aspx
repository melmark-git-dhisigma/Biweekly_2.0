<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="UserList.aspx.cs" Inherits="Admin_UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function deleteSystem() {
            var flag;
            flag = confirm("Are you sure you want to delete this User?");
            return flag;
        }




        $(document).ready(function () {
            $('#close_x').click(function () {
                $('#dialog').animate({ top: "-300%" }, function () {
                    $('#overlay').fadeOut('slow');
                });
            });
        });
    </script>





</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">


    <div id="overlay" class="web_dialog_overlay">
    </div>
    <div id="dialog" class="web_dialog" style="width: 700px;">
        <div id="sign_up5">
            <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
            <h3>View User </h3>
            <hr />

            <table width="100%" cellpadding="0" cellspacing="5" style="text-align: left;">
                <tr>
                    <td class="tdText" width="15%">User ID 
                    </td>
                    <td width="40%" class="tdText">
                        <asp:Label ID="lblNumber" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tdText" width="15%">Address 
                    </td>
                    <td>
                        <asp:Label ID="lblAddr" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Name 
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tdText">City 
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblCity" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText" width="15%">UserInitial 
                    </td>
                    <td width="35%" class="tdText">
                        <asp:Label ID="lblClass" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tdText">State 
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblState" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    
                    <td class="tdText">Country 
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tdText" style="display:none">Gender 
                    </td>
                    <td class="tdText" style="display:none">
                        <asp:Label ID="lblGender" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Zip 
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblZip" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tdText">Mobile 
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblMobile" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Phone 
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tdText">E-mail 
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Modified By
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblModifiedBy" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tdText">Modified On
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblModifiedOn" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>


        </div>
    </div>




    <table width="99%">


        <tr>
            <td id="tdMsg" runat="server" colspan="2"></td>
        </tr>


        <tr>
            <td align="left" class="style1">
                <table style="width: 35%">

                    <tr>
                        <td class="tdText" style="padding-right: 13px">Name</td>
                        <td>
                            <asp:TextBox ID="TextBox_StudentName" runat="server" CssClass="textClass" MaxLength="30"></asp:TextBox></td>
                        <td>
                            <asp:Button ID="Button_Search" runat="server" CssClass="btn btn-orange" Text="" OnClick="Button_Search_Click" /></td>
                        <td>

                            <asp:Button ID="Button_Add" runat="server" CssClass="NFButton" Text="Add" OnClick="Button_Add_Click" />

                        </td>
                    </tr>

                </table>
            </td>
            <td width="8%" align="right">

                        <asp:HiddenField ID="HdFldActiveInactive" runat="server" />

                <table style="width: 0;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" style="width: 25%" valign="bottom">
                            <asp:LinkButton ID="linkActive" runat="server" OnClick="linkActive_Click">Active</asp:LinkButton>
                        </td>
                        <td>|</td>
                        <td align="center" valign="bottom">
                            <asp:LinkButton ID="lnkInactive" runat="server" OnClick="lnkInactive_Click">InActive</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">

                <asp:GridView ID="grd_Users" runat="server" AutoGenerateColumns="False"
                    OnRowDataBound="grd_Users_RowDataBound" Width="100%"
                    OnRowDeleting="grd_Users_RowDeleting"
                    EmptyDataText="No Data Found.." PageSize="12" AllowPaging="True"
                    OnRowCommand="grd_Users_RowCommand" OnPageIndexChanging="grd_Users_PageIndexChanging" GridLines="none" CellPadding="4">



                    <Columns>
                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="ModBy" HeaderText="Modified By"></asp:BoundField>
                        <asp:BoundField DataField="ModifiedDate" HeaderText="Modified On" />



                        <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="clickMe" runat="server" Style="cursor: pointer;" runat="server" CommandName="View" Height="20px" Width="18px"
                                    ImageUrl="~/Administration/Images/view_02.png" class="btn btn-purple" CommandArgument='<%# Eval("UserId") %>' />
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="lb_Edit" CommandName="Edit" runat="server" CommandArgument='<%# Eval("UserId") %>'
                                    ImageUrl="~/Administration/images/user_edit.png" class="btn btn-blue" Height="20px" Width="18px"
                                    AlternateText="Edit" Enabled='<%# (HdFldActiveInactive.Value.ToString()=="1")? true:false %>'></asp:ImageButton>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="lb_delete" OnClientClick="javascript:return deleteSystem();"
                                    CommandName="Delete" runat="server"
                                    CommandArgument='<%# Eval("UserId") %>' ImageUrl="~/Administration/images/trash.png" class="btn btn-red"
                                    AlternateText="Delete" Height="20px" Width="18px" Enabled='<%# (HdFldActiveInactive.Value.ToString()=="1")? true:false %>'></asp:ImageButton>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>

                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                    <RowStyle CssClass="RowStyle" />
                    <AlternatingRowStyle CssClass="AltRowStyle" />
                    <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />

                </asp:GridView>

            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;
                
            </td>
        </tr>
        

    </table>
</asp:Content>
