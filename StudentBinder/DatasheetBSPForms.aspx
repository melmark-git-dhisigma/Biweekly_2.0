<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DatasheetBSPForms.aspx.cs" Inherits="StudentBinder_DatasheetBSPForms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<head id="head1" runat="server">

    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <script src="../Administration/JS/jquery-ui.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.min.js"></script>

    <style type="text/css">
        .divGrid {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: auto;
            height: auto;
        }

        .divGrid1 {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: auto;
            height: auto;
            display: none;
        }

        .divBackgrnd {
            padding: 26px 16px 16px 16px;
            width: 90%;
            height: 400px;
            overflow-y: scroll;
            overflow-x: hidden;
            -webkit-border-radius: 24px 24px 24px 24px;
            -moz-border-radius: 24px 24px 24px 24px;
            border-radius: 24px 24px 24px 24px;
            background: rgba(87,197,239,0.42);
            -webkit-box-shadow: #68A1B3 8px 8px 8px;
            -moz-box-shadow: #68A1B3 8px 8px 8px;
            box-shadow: #68A1B3 8px 8px 8px;
        }

        .pnlCSS {
            padding-top: 2px;
        }
    </style>
    <script type="text/javascript">

        function deleteDoc() {
            var flag;
            flag = confirm("Are you sure do you want to Delete?")
            return flag;
        }

        </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td style="padding-left: 118px">
                        <div id="divMessage" runat="server" style="width: 98%">
                        </div>
                        <br />
                        <div style="width: 100%; padding-left: 10px;">
                            <br />
                            <asp:FileUpload ID="fupDoc" runat="server" Style="width: 400px;" />
                            <asp:Button ID="btUpload" runat="server" Text="Upload" OnClick="btUpload_Click" CssClass="NFButton" />
                            <br />
                            <br />
                            <asp:Label ID="lMsg" runat="server" />

                        </div>
                        <asp:Panel Width="100%" Height="600px" HorizontalAlign="Center" ID="divPanel" runat="server"
                            ScrollBars="Both" Style="overflow: auto">
                            <div id="div_Grid" style="width: 98%;" runat="server">
                                <asp:GridView ID="grdBSPView" CellPadding="4" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found.." GridLines="None" BorderColor="#336666" OnRowEditing="grdBSPView_RowEditing" OnRowCommand="grdBSPView_RowCommand" OnPageIndexChanging="grdBSPView_PageIndexChanging"  Width="100%" AllowPaging="True" PageSize="10">

                                    <Columns>
                                        <asp:BoundField DataField="slno" ItemStyle-Width="150px" HeaderText="No" />

                                        <asp:TemplateField HeaderText="Document Name" ItemStyle-Width="440px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label CommandName="lbldownload" ID="lbllnkDownload" Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("BSPDoc") %>' ToolTip='<%# Eval("BSPDocUrl") %>' Style="cursor: pointer;" runat="server">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CreatedOn" ItemStyle-Width="250px" HeaderText="UploadedOn" />
                                        <asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkDownload" ItemStyle-Width="50px" runat="server" Text='<%# Eval("BSPDocUrl") %>' CommandArgument='<%# Eval("BSPDoc") %>' CommandName="view" Height="18px" Width="18px" ImageUrl="~/Administration/Images/view_02.png" class="btn btn-purple" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="15px">
                                            <ItemTemplate>
                                                <asp:ImageButton OnClientClick="javascript:return deleteDoc();" ID="lb_edit" runat="server" class="btn btn-red" CommandArgument='<%# Eval("BSPDoc") %>' CommandName="Edit" ImageUrl="~/Administration/images/trash.png" Height="18px" Width="18px" BackColor="Black" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>
                                    <HeaderStyle CssClass="HeaderStyle" />

                                    <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                                    <RowStyle CssClass="RowStyle" />
                                    <AlternatingRowStyle CssClass="AltRowStyle" />
                                    <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#339966" CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>

                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
