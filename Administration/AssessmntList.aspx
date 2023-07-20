<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="AssessmntList.aspx.cs" Inherits="Admin_AssessmntList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">




    <link href="CSS/popupStyle1.css" rel="stylesheet" />
    <link href="CSS/style2.css" rel="stylesheet" />
    <script src="JS/jq1.js"></script>
    <script src="JS/jquery-1.8.3.js"></script>


    <script type="text/javascript">
        function ShowDialog(modal) {
            var popUpContent = $('#ctl00_PageContent_dlAsmntView');
            var popUpSpan = $(popUpContent).find("span");
            for (var i = 0; i < popUpSpan.length; i++) {

                var inner = $(popUpSpan[i]).html().length;
                $(popUpSpan[i]).attr($(popUpSpan[i]).html());

                if (inner > 20) {
                    $(popUpSpan[i]).html($(popUpSpan[i]).html().substring(0, 20) + "...");

                }

            }


            $("#overlay").show();
            $("#dialog").fadeIn(300);

            if (modal) {
                $("#overlay").unbind("click");
            }
            else {
                $("#overlay").click(function (e) {
                    HideDialog();
                });
            }
        }
        function ShowDialogPreview(modal) {
            $("#overlay").show();
            $("#dialog2").fadeIn(300);


            if (modal) {
                $("#overlay").unbind("click");
            }
            else {
                $("#overlay").click(function (e) {
                    HideDialog();
                });
            }
        }
        function HideDialogPrvw() {
            $("#overlay").hide();
            $("#dialog2").fadeOut(300);

        }

        function HideDialog() {
            $("#overlay").hide();
            $("#dialog").fadeOut(300);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">

                <tr>
                    <td colspan="3">
                        <asp:Panel ID="Panel1" Width="100%" runat="server" Style="overflow: auto">
                            <!--<div class="divGrid">-->
                            <div id="Msg" runat="server" style="font-size: larger; font-weight: bold; color: Blue; text-align: center;"></div>
                            <asp:GridView ID="grdAssessmnts" Width="100%" runat="server" HorizontalAlign="Justify"
                                Style="margin-left: 0px;" AutoGenerateColumns="False" GridLines="none" OnRowCommand="grdAssessmnts_RowCommand"
                                OnRowDeleting="grdAssessmnts_RowDeleting" OnRowDataBound="grdAssessmnts_RowDataBound" EmptyDataText="No Data Found...">
                                <Columns>
                                    <asp:BoundField HeaderText="Assessments" DataField="AsmntName"></asp:BoundField>
                                    <asp:BoundField HeaderText="Modified By" DataField="CreatedBy"></asp:BoundField>
                                    <asp:BoundField HeaderText="Modified On" DataField="ModifiedOn" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Skills" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnView" CommandArgument='<%# Eval("AsmntId") %>' CommandName="View"
                                                runat="server" ImageUrl="~/Administration/Images/view_02.png" class="btn btn-purple" Height="20px" Width="20px" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Preview" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnPrview" runat="server" CommandArgument='<%# Eval("AsmntId") %>'
                                                CommandName="Preview" Height="28px" Width="30px" ImageUrl="~/Administration/images/Skills.png" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDelete" CommandArgument='<%# Eval("AsmntId") %>' CommandName="Delete"
                                                runat="server" ImageUrl="~/Administration/images/trash.png" class="btn btn-red" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                                <RowStyle CssClass="RowStyle" />
                                <AlternatingRowStyle CssClass="AltRowStyle" />
                                <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                <SortedAscendingHeaderStyle BackColor="#487575" />
                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                <SortedDescendingHeaderStyle BackColor="#275353" />
                            </asp:GridView>
                            <!--</div>-->
                            <br />
                            <br />
                        </asp:Panel>
                        <div id="overlay" class="web_dialog_overlay">
                        </div>
                        <div id="dialog" class="web_dialog" style="top: 20%; display: none">
                            <a id="btnClose" class="close sprited1 sprited2" href="javascript:HideDialog()">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-top: -16px; margin-right: -17px;" width="18" height="18" alt="" />
                            </a>
                            <table class="popsecondver" style="width: 100%; border: 0px; height: auto">
                                <tr>
                                    <td colspan="4" align="right"></td>
                                </tr>
                                <tr>

                                    <td colspan="4" align="left">
                                        <h3>Assessment Details</h3>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdText" style="width: 95px">&nbsp;</td>
                                    <td class="tdText" style="width: 198px">Assessment Name</td>
                                    <td class="tdText">
                                        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td class="tdText nobdr">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="tdText">&nbsp;</td>
                                    <td class="tdText">Description</td>
                                    <td class="tdText">
                                        <asp:Label ID="lblDesc" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td class="tdText nobdr">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="tdText">&nbsp;</td>
                                    <td class="tdText">Created By</td>
                                    <td class="tdText">
                                        <asp:Label ID="lblby" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td class="tdText nobdr">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="tdText">&nbsp;</td>
                                    <td class="tdText">Created On</td>
                                    <td class="tdText">
                                        <asp:Label ID="lblOn" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td class="tdText nobdr">&nbsp;</td>
                                </tr>

                                <tr>
                                    <td colspan="4" align="center" valign="middle" class="tdText nobdr">
                                        <table width="100%">
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <br />
                                                    <br />
                                                    <asp:DataList ID="dlAsmntView" runat="server" CellSpacing="2" RepeatColumns="3">
                                                        <ItemTemplate>
                                                            <table width="200px">
                                                                <tr>
                                                                    <td style="width: 20px;">
                                                                        <asp:Image ID="imgSkill" Width="15px" Height="15px" runat="server" ImageUrl='<%# Eval("Url") %>' /></td>
                                                                    <td align="left" class="btmbig">
                                                                        <%--GoalCode is used to display the Edited goals--%>
                                                                        <asp:Label ID="lblSkill" runat="server" title='<%#Eval("GoalCode") %>' Text='<%# Eval("GoalCode") %>'></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="dialog2" class="web_dialog" style="top: 20%; display: none">
                            <div class="close-popUp">
                                <img src="../Administration/images/clb.PNG" alt="" onclick="javascript:HideDialogPrvw()" style="float: right; width: 18px; height: 18px; margin-right: 10px; margin-top: 16px">
                            </div>
                            <%-- <table style="width:100%"><tr>
                                    <td colspan="3" align="right">
                                        <a href="javascript:HideDialogPrvw()" id="A1"><%--<b>X</b>
                                            <img src="images/DeleteGray.png" width="20px" height="20px"/>
                                        </a>
                                    </td>
                                </tr></table>--%>

                            <table style="width: 100%; border: 0px; height: auto">

                                <tr>
                                    <td colspan="3" align="left">
                                        <h3>Assessment Preview</h3>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <div style="height: 350px; overflow-x: auto;">
                                            <asp:DataList ID="dl_Sections" runat="server" Width="90%">
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnlClick" runat="server" CssClass="pnlCSS" HorizontalAlign="Center">
                                                        <div style="background-image: url('green_bg.gif'); height: 22px; vertical-align: middle; text-align: left;">
                                                            <div style="color: White; cursor: pointer; padding: 0; text-align: left; border-bottom: 2px solid #fff;">
                                                                <table width="100%" style="background: #ededed url(../StudentBinder/images/topbtmline.JPG) right top repeat-y; border-bottom: 1px dotted #116c90; margin-top: 5px;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lb_Section" runat="server" CommandName='<%# Eval("name") %>'
                                                                                Text='<%# Eval("name") %>' Font-Names="Arial" Font-Size="12px" Font-Bold="true" Enabled="false"></asp:LinkButton>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Image ID="imgArrows" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlCollapsable" runat="server" Height="0" CssClass="pnlCSS">
                                                        <asp:DataList ID="dl_SubSections" runat="server" Style="text-align: left; margin-top: 5px;" Visible="True"
                                                            Width="100%">
                                                            <ItemTemplate>
                                                                <asp:Panel ID="pnlClickB" runat="server" CssClass="pnlCSS">
                                                                    <div style="background-image: url('green_bg.gif'); height: 22px; vertical-align: left;">
                                                                        <div style="color: White; cursor: pointer; padding-right: 5px; text-align: left;">
                                                                            <table width="100%" style="background: #ededed url(../StudentBinder/images/topbtmline.JPG) right top repeat-y; border-bottom: 1px dotted #116c90; margin-top: 5px;">
                                                                                <tr>
                                                                                    <td style="background: url('../Administration/images/dotblk.JPG') no-repeat scroll 0 center transparent">
                                                                                        <asp:LinkButton ID="lb_SubSection" runat="server" CommandName='<%# Eval("name") %>'
                                                                                            Text='<%# Eval("name") %>' Enabled="false" Font-Names="Arial" Font-Size="12px" Font-Bold="true" Style="padding: 0 0 5px 10px;"></asp:LinkButton>
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:Image ID="imgArrows" runat="server" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCollapsableB" runat="server" Height="0" CssClass="pnlCSS">
                                                                    <asp:GridView ID="grd_Questions" runat="server" GridLines="none" AutoGenerateColumns="False" HorizontalAlign="Justify"
                                                                        Style="margin-left: 0px; text-align: left;" ShowHeader="true" Visible="True"
                                                                        Width="99%">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Code" HeaderText="Question" ItemStyle-Width="440px">
                                                                                <ItemStyle Width="440px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Box" HeaderText="Box" />
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td colspan="100%">
                                                                                            <div id="<%# Eval("ID") %>" class="divGrid1">
                                                                                                <asp:GridView ID="grd_SubQuestion" runat="server" GridLines="None" ShowHeader="true" AutoGenerateColumns="False"
                                                                                                    DataKeyNames="ID" Width="600px">
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText="Question" InsertVisible="False" SortExpression="ID">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkChoice" runat="server" onclick="chkGroup(this)" />
                                                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle Width="432px" />
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                    <HeaderStyle CssClass="HeaderStyle" Height="25px" Font-Bold="True" ForeColor="White" />

                                                                                                    <RowStyle CssClass="RowStyle" />
                                                                                                    <AlternatingRowStyle CssClass="AltRowStyle" />
                                                                                                    <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                                                                                    <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                                                                    <PagerStyle CssClass="PagerStyle" BackColor="#ccccff" HorizontalAlign="Center" />
                                                                                                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                                                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                                                    <SortedAscendingHeaderStyle BackColor="#487575" />
                                                                                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                                                    <SortedDescendingHeaderStyle BackColor="#275353" />
                                                                                                </asp:GridView>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle CssClass="HeaderStyle" Height="25px" Font-Bold="True" ForeColor="White" />

                                                                        <RowStyle CssClass="RowStyle" />
                                                                        <AlternatingRowStyle CssClass="AltRowStyle" />
                                                                        <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                                                        <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                                        <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                                                        <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                        <SortedAscendingHeaderStyle BackColor="#487575" />
                                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                        <SortedDescendingHeaderStyle BackColor="#275353" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" CollapseControlID="pnlClickB"
                                                                    Collapsed="true" ExpandControlID="pnlClickB" TextLabelID="lblMessage" CollapsedText="Show"
                                                                    ExpandedText="Hide" ImageControlID="imgArrows" CollapsedImage="~/Administration/Images/downarrow.jpg"
                                                                    ExpandedImage="~/Administration/Images/uparrow.jpg" ExpandDirection="Vertical" TargetControlID="pnlCollapsableB"
                                                                    ScrollContents="false">
                                                                </asp:CollapsiblePanelExtender>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                        <asp:GridView ID="grd_SubSections" runat="server" GridLines="none" AutoGenerateColumns="False" Width="99%"
                                                            HorizontalAlign="Justify" Style="margin-left: 0px">
                                                            <Columns>
                                                                <asp:BoundField DataField="Code" HeaderText="Code" />
                                                                <asp:BoundField DataField="name" HeaderText="Question" Visible="false" />
                                                                <asp:BoundField DataField="Box" HeaderText="Box" />
                                                            </Columns>
                                                            <HeaderStyle CssClass="HeaderStyle" Height="25px" Font-Bold="True" ForeColor="White" />

                                                            <RowStyle CssClass="RowStyle" />
                                                            <AlternatingRowStyle CssClass="AltRowStyle" />
                                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#487575" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#275353" />
                                                        </asp:GridView>

                                                    </asp:Panel>
                                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" CollapseControlID="pnlClick"
                                                        Collapsed="true" ExpandControlID="pnlClick" TextLabelID="lblMessage" CollapsedText="Show"
                                                        ExpandedText="Hide" ImageControlID="imgArrows" CollapsedImage="~/Administration/Images/downarrow.jpg"
                                                        ExpandedImage="~/Administration/Images/uparrow.jpg" ExpandDirection="Vertical" TargetControlID="pnlCollapsable"
                                                        ScrollContents="false">
                                                    </asp:CollapsiblePanelExtender>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



