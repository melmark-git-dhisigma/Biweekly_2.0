<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="AddGroup.aspx.cs" Inherits="Admin_AddGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function Delete() {
            var flag;
            var buttons = $('#<%=btnSave.ClientID %>').val();
            if(buttons=="Delete")
             {
                flag = confirm("Are you sure to Delete?");
                return flag;
            }
        }
    </script>
   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 99%;">
                <tr>
                    <td id="tdMsg" runat="server" colspan="11"></td>
                </tr>
                <tr>
                    <td class="tdText">Group Code</td><td style="color:red">*</td>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server" CssClass="textClass" Width="210px"></asp:TextBox></td>
                    <td class="tdText">Group Name</td><td style="color:red">*</td>
                    <td>
                        <asp:TextBox ID="txtname" runat="server" CssClass="textClass" Width="210px"></asp:TextBox></td>
                    <td class="tdText">Group Description</td><td style="color:red">*</td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="textClass" Width="270px" MaxLength="50"></asp:TextBox></td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButton" OnClick="btnSave_Click" OnClientClick="javascript:return Delete();" Text="Save" /></td>
                    <td>
                        <asp:Button ID="btnNext" runat="server" CssClass="NFButton" OnClick="btnNext_Click" Text="Next" /></td>
                   
                </tr>
                <tr>
                    <td colspan="9" align="center">
                        <div>
                            <asp:GridView ID="grdGroup" runat="server" AutoGenerateColumns="False" Width="100%"
                                AllowPaging="True" OnPageIndexChanging="grdGroup_PageIndexChanging" OnRowCommand="grdGroup_RowCommand"
                                OnRowDeleting="grdGroup_RowDeleting" OnRowEditing="grdGroup_RowEditing"
                                GridLines="none" CellPadding="4" ForeColor="#333333" OnRowDataBound="grdGroup_RowDataBound">
                                <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                                <RowStyle CssClass="RowStyle" />
                                <FooterStyle CssClass="FooterStyle" Font-Bold="True" ForeColor="White" />
                                <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                
                                <EditRowStyle BackColor="#7C6F57" />
                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />

                                <Columns>
                                    <asp:BoundField DataField="GroupCode" HeaderText="Group Code"  HeaderStyle-Width="120px"/>
                                    <asp:BoundField DataField="GroupName" HeaderText="Group Name" /> 
                                    <asp:BoundField DataField="GroupDesc" HeaderText="Group Description" />                                    
                                    <asp:BoundField DataField="ModifiedUser" HeaderText="Modified By" />
                                    <asp:BoundField DataField="ModifiedDate" HeaderText="Modified On" />
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lb_Edit" CommandName="Edit" runat="server" CommandArgument='<%# Eval("GroupId") %>'
                                                ImageUrl="~/Administration/images/user_edit.png" class="btn btn-blue" Width="18px"></asp:ImageButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lb_delete"
                                                CommandName="Delete" runat="server" CommandArgument='<%# Eval("GroupId") %>'
                                                ImageUrl="~/Administration/images/trash.png" class="btn btn-red" Width="18px"></asp:ImageButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                                <RowStyle CssClass="RowStyle" />
                                <AlternatingRowStyle CssClass="AltRowStyle" />
                                <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="PagerStyle"  HorizontalAlign="Center" />
                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                <SortedAscendingHeaderStyle BackColor="#487575" />
                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                <SortedDescendingHeaderStyle BackColor="#275353" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hidGroupId" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
