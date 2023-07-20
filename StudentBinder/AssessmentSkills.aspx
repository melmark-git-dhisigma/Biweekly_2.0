<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssessmentSkills.aspx.cs" Inherits="StudentBinder_AssessmentSkills" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Administration/CSS/DisplayStyles.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="dialog" class="web_dialog">
                            <table style="width: 100%; border: 0px; height: 100%">
                                <tr style="height: 10%">
                                    <td colspan="3" align="right" >
                                        <table><tr><td style="background-color:#03507d;height:25px;width:200px;text-align:center;">
                                        <asp:LinkButton ID="lnk_backtolist" runat="server" OnClick="lnk_backtolist_Click" Font-Underline="False" ForeColor="White" Font-Names="Arial" Font-Size="14px" Font-Bold="true">Back to Assessment View</asp:LinkButton>
                                            </td></tr></table>
                                    </td>
                                </tr>
                                <tr style="height: 10%">
                                    <td colspan="3" align="center" style="font-family: Arial; font-weight: bold; font-size: 16px; color: gray;">
                                        <b>
                                            <asp:Label ID="lbl_AsmntSkills" runat="server"></asp:Label></b>
                                    </td>
                                </tr>
                                <tr style="height: 10%">
                                    <td colspan="3" align="center" class="tdTextCenter">Click the filter button to edit the sections coming under the Skill
                                    </td>
                                </tr>
                                <tr style="height: 70%; vertical-align: top;">
                                    <td></td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:DataList ID="dlSkillView" runat="server" CellSpacing="2" RepeatColumns="1" OnItemDataBound="dlSkillView_ItemDataBound"
                                                        OnItemCommand="dlSkillView_ItemCommand">
                                                        <ItemTemplate>
                                                            <table width="550px" style="border: 1px solid black;">
                                                                <tr>
                                                                    <td style="width: 520px;">
                                                                        <asp:HiddenField ID="hfAsmntID" runat="server" />
                                                                        <asp:Label ID="lblSkill" runat="server" Text='<%# Eval("GoalName") %>' Font-Names="Arial"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 30px;">
                                                                        <asp:ImageButton ID="imgbtnSkill" Width="25px" Height="25px" runat="server" Enabled="true"
                                                                            Visible="true" ImageUrl="~/Administration/images/filter.png" CommandArgument='<%# Eval("GoalName") %>' />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td></td>
                                </tr>
                                
                            </table>
                        </div>
    </div>
    </form>
</body>
</html>
