<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="ListSchool.aspx.cs" Inherits="Admin_ListSchool" %>

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
            flag = confirm("Are you sure you want to delete this School ?");
            return flag;
        }

    </script>
    <style type="text/css">
        .auto-style1 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            height: 19px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <table width="99%">
        <tr>
            <td width="30%" id="tdMsg" align="center" colspan="2" runat="server">&nbsp;</td>
        </tr>
         <tr>
            <td width="30%" id="td1" align="center" colspan="2" runat="server">&nbsp;</td>
        </tr>
        <tr>
            <td align="left" class="" width="70%">
                           
                    <asp:Button ID="Button_Add" runat="server" Text="Add" OnClick="Button_Add_Click"
                        CssClass="NFButton" /> 

            </td>
            <td width="30%" align="right">
                <%--<table width="50%" cellpadding="0" cellspacing="0">
                    <td width="50%" valign="bottom" align="right" style="width: 0%">
                        <asp:LinkButton ID="linkActive" runat="server" OnClick="linkActive_Click">Active</asp:LinkButton>
                    </td>
                    <td width="10%" valign="bottom" align="center" style="width: 25%; color: #000000; font-weight: bold;">|
                    </td>
                    <td valign="bottom" align="center" width="20%">
                        <asp:LinkButton ID="lnkInactive" runat="server" OnClick="lnkInactive_Click">InActive</asp:LinkButton>
                    </td>
                </table>--%>

                        <asp:HiddenField ID="HdFldActiveInactive" runat="server" />

                <table style="width:0;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" style="width: 25%" valign="bottom">
                            <asp:LinkButton ID="linkActive" runat="server" OnClick="linkActive_Click">Active</asp:LinkButton>
                        </td>
                        <td>|</td>
                        <td align="left" valign="bottom">
                            <asp:LinkButton ID="lnkInactive" runat="server" OnClick="lnkInactive_Click">InActive</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:GridView ID="GV_Student" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    AllowPaging="True" AllowSorting="True" OnRowDeleting="GV_Student_RowDeleting"
                    OnRowUpdating="GV_Student_RowUpdating" Width="100%" OnRowCommand="GV_Student_RowCommand"
                    OnPageIndexChanging="GV_Student_PageIndexChanging" OnSelectedIndexChanged="GV_Student_SelectedIndexChanged"
                    OnRowDataBound="GV_Student_RowDataBound" EmptyDataText="No Data Found..."
                    GridLines="none">

                    <Columns>
                        <asp:BoundField DataField="SchoolId" HeaderText="School Number" />

                        <asp:BoundField DataField="SchoolName" HeaderText="School Name" />

                        <asp:BoundField DataField="DistrictName" HeaderText="District Name" />

                        <asp:BoundField DataField="LookupName" HeaderText="District State" />

                        <asp:BoundField DataField="ModifiedUser" HeaderText="ModifiedBy" />

                        <asp:BoundField DataField="ModifiedDate" HeaderText="Modified On" />

                        <asp:TemplateField HeaderText="View">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="View" Height="18px" 
                                    Width="20px" ImageUrl="~/Administration/Images/view_02.png"  class="btn btn-purple"
                                    CommandArgument='<%# Eval("SchoolId") %>' AlternateText="View" />
                            </ItemTemplate>


                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="lb_Edit" CommandName="Edit" runat="server" CommandArgument='<%# Eval("SchoolId") %>'
                                    ImageUrl="~/Administration/images/user_edit.png"  class="btn btn-blue" AlternateText="Edit"
                                    Height="20px" Width="18px" Enabled='<%# (HdFldActiveInactive.Value.ToString()=="1")? true:false %>'></asp:ImageButton>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="lb_delete" OnClientClick="javascript:return deleteSystem();"
                                    CommandName="Delete" runat="server" CommandArgument='<%# Eval("SchoolId") %>'
                                    ImageUrl="~/Administration/images/trash.png" class="btn btn-red"  AlternateText="Delete"
                                    Height="20px" Width="18px" Enabled='<%# (HdFldActiveInactive.Value.ToString()=="1")? true:false %>'></asp:ImageButton>
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
    <div id="dialog" class="web_dialog" style="width:700px;" >
         <div id="sign_up5">
            <a id="close_x" class="close sprited1" href="#" style="margin-top:-13px;margin-right:-14px;"><img src="../Administration/images/clb.PNG" style="float: right;margin-right:0px;margin-top:0px;z-index:300" width="18" height="18" alt=""  /></a>
            <h3>Details Of
                <asp:Label ID="lblSchool" runat="server" Text=""></asp:Label>
            </h3><hr />

            <table cellpadding="0" cellspacing="5"  >
                <tr>
                    <td colspan="4"></td>
            
                </tr>
                <tr>
                    <td class="tdText" width="120">School Name
                    </td>
                    <td  align="left" class="tdText" width="250px">
                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                    </td>
                    <td  align="left" class="tdText" width="96xpx">
                        Address</td>
                    <td  align="left" class="tdText" width="250px">
                        <asp:Label ID="lblAddr" runat="server" Text=""></asp:Label>
                    </td>
           
                </tr>
                <tr>
                    <td class="tdText">Country
                    </td>
                    <td  class="tdText">
                        <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label>
                    </td>
                    <td  class="tdText">
                        State
                    </td>
                    <td  class="tdText">
                        <asp:Label ID="lblState" runat="server" Text=""></asp:Label>
                    </td>
     
                </tr>
                <tr>
                    <td class="tdText">City
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblCity" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tdText">
                        Zip
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblZip" runat="server" Text=""></asp:Label>
                    </td>
        
                </tr>
                <tr>
                    <td class="tdText">Mobile
                    </td>
                    <td  class="tdText">
                        <asp:Label ID="lblMobile" runat="server" Text=""></asp:Label>
                    </td>
                    <td  class="tdText">
                        Phone</td>
                    <td  class="tdText">
                        <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label>
                    </td>
             
                </tr>
                <tr>
                    <td class="tdText">E-mail
                    </td>
                    <td  class="tdText">
                        <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
                    </td>
                    <td  class="tdText">
                        Modified By</td>
                    <td  class="tdText">
                        <asp:Label ID="lblModifiedBy" runat="server" Text=""></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td class="tdText">Modified On
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblModifiedOn" runat="server" Text=""></asp:Label>
                    </td>
            

                    <td class="tdText">
                        &nbsp;</td>
            

                    <td class="tdText">
                        &nbsp;</td>
            

                </tr>

            </table>

        </div>
    </div>
</asp:Content>
