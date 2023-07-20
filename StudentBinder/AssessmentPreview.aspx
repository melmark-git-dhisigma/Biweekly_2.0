<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssessmentPreview.aspx.cs" Inherits="StudentBinder_AssessmentPreview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="dialog2" class="web_dialog2">
                <table style="width: 100%; border: 0px; height: 100%">
                    <tr style="height: 10%">
                        <td colspan="3" align="right">
                            <table>
                                <tr>
                                    <td style="background-color: #03507d; height: 25px; width: 200px; text-align: center;">

                                        <asp:LinkButton ID="lnk_backtolist" runat="server" OnClick="lnk_backtolist_Click" Font-Underline="False" ForeColor="White" Font-Names="Arial" Font-Size="14px" Font-Bold="true">Back to Assessment View</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <tr style="height: 10%">
                        <td colspan="3" align="center" style="font-family: Arial; font-weight: bold; font-size: 16px; color: gray;">
                            <b>Assessment Preview</b>
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
                                        <asp:GridView ID="grdAsmntPreview" runat="server" GridLines="none" Font-Names="Consolas" Font-Size="Small" AllowSorting="True" OnSorting="grdAsmntPreview_Sorting">
                                            <HeaderStyle CssClass="HeaderStyle" Height="25px" ForeColor="White" Width="5%" />

                                            <RowStyle CssClass="RowStyle" Width="5px" />
                                            <AlternatingRowStyle CssClass="AltRowStyle" />
                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle CssClass="PagerStyle" BackColor="#FFFFFF" HorizontalAlign="Center" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                            <SortedAscendingHeaderStyle BackColor="#487575" />
                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                            <SortedDescendingHeaderStyle BackColor="#275353" />
                                        </asp:GridView>
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
