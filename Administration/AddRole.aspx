<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="AddRole.aspx.cs" Inherits="Admin_AddRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        function Delete() {
            var flag;
            var buttons = $('#<%=btnSave.ClientID %>').val();
            if (buttons == "Delete") {
                flag = confirm("Are you sure to Delete?");
                return flag;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

           

            <table width="99%">
                <tr><td colspan="11" id="tdMsg" runat="server"></td></tr>
                <tr>
                    <td class="tdText">Select Group</td>
                    <td style="color:red">*</td>
                    <td>
                        <asp:DropDownList ID="ddlGroup" runat="server" CssClass="drpClass" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" ></asp:DropDownList></td>
                    <td class="tdText">Role Code</td>
                    <td style="color:red">*</td>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server" CssClass="textClass" MaxLength="30" Width="90px"></asp:TextBox></td>
                    <td class="tdText">Role Description</td>
                    <td style="color:red">*</td>
                    <td style="width:10px">
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="textClass" MaxLength="50" Width="240px"></asp:TextBox></td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Width="80" CssClass="NFButton" OnClick="btnSave_Click" OnClientClick="javascript: return Delete();" Text="Save" /></td>
                    <td>
                        <asp:Button ID="btnNext" runat="server" Width="80" CssClass="NFButton" OnClick="btnNext_Click" Text="Next" /></td>
                    

                </tr>

            </table>



            <table style="width: 99%;">
                </tr>
                    <tr>
                        <td colspan="4" width="20%">
                            <asp:GridView ID="grdRole" runat="server" AllowPaging="True" AutoGenerateColumns="False" EmptyDataText="No Data Found...." GridLines="none" OnPageIndexChanging="grdRole_PageIndexChanging" OnRowCommand="grdRole_RowCommand" OnRowDataBound="grdRole_RowDataBound" OnRowDeleting="grdRole_RowDeleting" OnRowEditing="grdRole_RowEditing" Width="100%">
                                <HeaderStyle CssClass="HeaderStyle" />
                                <RowStyle CssClass="RowStyle" />
                                <FooterStyle CssClass="FooterStyle" />
                                <SelectedRowStyle CssClass="SelectedRowStyle" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                <Columns>
                                    <asp:BoundField DataField="GroupName" HeaderText="Group Name" />
                                    <asp:BoundField DataField="RoleCode" HeaderText="Role Code" />
                                    <asp:BoundField DataField="RoleDesc" HeaderText="Role Description" />
                                    <asp:BoundField DataField="ModifiedUser" HeaderText="Modified By" />
                                    <asp:BoundField DataField="ModifiedDate" HeaderText="Modified On" />
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lb_Edit" runat="server" class="btn btn-blue" CommandArgument='<%# Eval("RoleId") %>' CommandName="Edit" ImageUrl="~/Administration/images/user_edit.png" Width="18px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lb_delete" runat="server" class="btn btn-red" CommandArgument='<%# Eval("RoleId") %>' CommandName="Delete" ImageUrl="~/Administration/images/trash.png"  Width="18px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                                <RowStyle CssClass="RowStyle" />
                                <AlternatingRowStyle CssClass="AltRowStyle" />
                                <FooterStyle CssClass="FooterStyle"  ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#339966" CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                            </asp:GridView>
                        </td>
                </tr>
                
            </table>
             <asp:HiddenField ID="hidRoleId" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>

      
</asp:Content>
