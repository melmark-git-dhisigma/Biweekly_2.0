<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="ViewLessonplanList.aspx.cs" Inherits="Admin_ViewLessonplanList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function deleteSystem() {
            var flag;
            flag = confirm("Are you sure you want to delete this Lesson Plan?");
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
    <style type="text/css">
        .style1 {
            font-family: Calibri;
            color: Black;
            line-height: 22px;
            font-weight: bold;
            font-size: 13px;
            padding-right: 1px;
            text-align: right;
            width: 21%;
        }

        .style2 {
            font-family: Calibri;
            color: Black;
            line-height: 22px;
            font-weight: bold;
            font-size: 13px;
            padding-right: 1px;
            text-align: right;
            width: 49%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <div id="overlay" class="web_dialog_overlay">
    </div>
    <div id="dialog" class="web_dialog" style="width:700px">
        <div id="sign_up5">
            <h3 >View LessonPlan Details</h3><hr />
            <a id="close_x" class="close sprited1" href="#" style="margin-top:-13px;margin-right:-14px;"><img src="../Administration/images/clb.PNG" style="float: right;margin-right:0px;margin-top:0px;z-index:300" width="18" height="18" alt=""  /></a>
            <table width="100%" cellpadding="0" cellspacing="5" style="text-align: left;" class="display">
                <tr>
                    <td class="tdText" width="15%">LessonPlan Name
                    </td>
                    <td width="40%" class="tdText">
                        <asp:Label ID="lblLessonName" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText" width="15%">PreRequisite Skills
                    </td>
                    <td width="40%" class="tdText">
                        <asp:Label ID="lblPrerequisite" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Teacher SD
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblTeacherSd" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Teacher Instructions
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblTeacherInst" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Consequence
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblConsequence" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Materials
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblMaterials" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tdText" valign="top">Selected Goals
                    </td>
                    <td class="tdText">
                        <asp:Label ID="lblGoals" runat="server" Text=""></asp:Label>
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <table width="100%">
        <tr>
            <td width="50%" id="tdMsg" colspan="2" runat="server" style="width: 100%">&nbsp;
            </td>
        </tr>
        <tr>
            <td width="50%">
                <table width="100%">
                    <tr>
                        <td class="tdText" width="50%">Select Goals:</td>
                        <td align="left" width="50%">
                            <asp:DropDownList ID="ddlSelectGoals" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSelectGoals_SelectedIndexChanged1" CssClass="drpClass">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="50%">

                <table width="100%">
                    <tr>
                        <td class="tdText">LessonPlan:</td>
                        <td align="center" width="25%">


                            <asp:TextBox ID="txtSearchLesson" runat="server" CssClass="textClass"></asp:TextBox>


                        </td>
                        <td width="15%" align="left">

                            <div class="rounded-corners" align="left"><asp:Button ID="Button_Search" runat="server" OnClick="Button_Search_Click" Text="Search"
                                CssClass="NFButton" /></div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td width="50%">&nbsp;
            </td>
            <td>

                        <asp:HiddenField ID="HdFldActiveInactive" runat="server" />

                <table style="width:0; float:right">
                    <td width="60%">&nbsp;</td>
                    <td width="40%" style="width: 0%">

                        <asp:LinkButton ID="linkActive" runat="server" OnClick="linkActive_Click">Active</asp:LinkButton>

                    </td>

                    <td width="40%" style="width: 20%">

                        <asp:LinkButton ID="lnkInactive" runat="server" OnClick="lnkInactive_Click">InActive</asp:LinkButton>

                    </td>

                </table>

            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:GridView ID="gvLessonData" runat="server" AutoGenerateColumns="False" OnRowCommand="gvLessonData_RowCommand"
                    OnSelectedIndexChanged="gvLessonData_SelectedIndexChanged" OnPageIndexChanging="gvLessonData_PageIndexChanging"
                    AllowPaging="True" EmptyDataText="Data Not Found...." GridLines="none" OnRowDataBound="gvLessonData_RowDataBound"
                    Width="100%">
                    <%--//   <AlternatingRowStyle CssClass="AltRowStyle"  Font-Size = "Medium" Width = "30%" />--%>
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                      <AlternatingRowStyle CssClass="AltRowStyle"  />
                    <Columns>
                        <asp:BoundField DataField="LessonPlanName" HeaderText="Lesson Plan Name"
                            ItemStyle-Width="25%">
                            <ItemStyle Width="35%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Modified By" DataField="ModifiedUser"
                            ItemStyle-Width="30%">
                            <ItemStyle Width="30%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Modified On" DataField="ModifiedDate"
                            ItemStyle-Width="10%" DataFormatString="{0:d}">
                            <ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:ImageButton ID="clickMe" runat="server" Style="cursor: pointer;" runat="server"
                                    CommandName="View" Height="20px" Width="18px" ImageUrl="~/Administration/Images/view_02.png" class="btn btn-purple"
                                    CommandArgument='<%# Eval("LessonPlanId") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("LessonPlanId") %>'
                                    CommandName="editValue" ImageUrl="~/Administration/images/user_edit.png" class="btn btn-blue"
                                    Height="20px" Width="20px" Enabled='<%# (HdFldActiveInactive.Value.ToString()=="1")? true:false %>'/>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("LessonPlanId") %>'
                                    CommandName="deleteValue" ImageUrl="~/Administration/images/trash.png" class="btn btn-red"
                                    OnClientClick="javascript:return deleteSystem();" Height="20px" Width="20px" Enabled='<%# (HdFldActiveInactive.Value.ToString()=="1")? true:false %>'/>
                            </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                    <FooterStyle CssClass="FooterStyle"  ForeColor="#333333" />
                    <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                    <PagerStyle CssClass="PagerStyle"  ForeColor="White" HorizontalAlign="Center" />

                         <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                    
                    <RowStyle CssClass="RowStyle"  />
                    <AlternatingRowStyle CssClass="AltRowStyle"  />
                    <FooterStyle CssClass="FooterStyle"  ForeColor="#333333" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="PagerStyle"  HorizontalAlign="Center" />
                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class="style1">&nbsp;
            </td>
            <td class="tdText">&nbsp;
            </td>
            <td>&nbsp;
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="4" style="text-align:center"><asp:Button ID="BtnInsert" runat="server" OnClick="Button_Search_Click" Text="Add" CssClass="NFButton" />
            </td>
        </tr>
        </table>
</asp:Content>
