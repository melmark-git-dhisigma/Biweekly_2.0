<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master"
    AutoEventWireup="true" CodeFile="~/Administration/AddAssessments.aspx.cs" Inherits="Admin_AddAssessments" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
    <title></title>
    <script type="text/javascript" src="JS/tabber.js"></script>
    <link rel="stylesheet" href="CSS/tabmenu.css" type="text/css" media="screen" />
    <%--<link href="CSS/popupStyle1.css" rel="stylesheet" />
    <link href="CSS/style2.css" rel="stylesheet" />--%>
    <script type="text/javascript">

        /* Optional: Temporarily hide the "tabber" class so it does not "flash"
        on the page as plain HTML. After tabber runs, the class is changed
        to "tabberlive" and it will appear. */

        document.write('<style type="text/css">.tabber{display:none;}<\/style>');
    </script>
    <style type="text/css">
        .divGrid {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: auto;
            height: auto;
        }

        .divBackgrnd {
            padding: 26px 16px 16px 16px;
            width: 90%;
            height: 250px;
            overflow-y: auto;
            overflow-x: hidden;
            -webkit-border-radius: 24px 24px 24px 24px;
            -moz-border-radius: 24px 24px 24px 24px;
            border-radius: 24px 24px 24px 24px;
            background: rgba(87,197,239,0.2);
            -webkit-box-shadow: #68A1B3 8px 8px 8px;
            -moz-box-shadow: #68A1B3 8px 8px 8px;
            box-shadow: #68A1B3 8px 8px 8px;
        }

        .web_dialog_overlay {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000000;
            opacity: .15;
            filter: alpha(opacity=15);
            -moz-opacity: .15;
            z-index: 101;
            display: none;
            text-align: center;
        }

        .web_dialog {
            background:url("../Administration/images/smalllgomlmark.JPG") no-repeat scroll right bottom #F8F7FC;
            display: none;
            position: fixed;
            width: 1040px;
            height: 550px;
            overflow: auto;
            top: 50%;
            left: 50%;
            margin-left: -520px;
            margin-top: -275px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 102;
            background: #f8f7fc url(../images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
        }
    </style>
    <script type="text/javascript">



        function ShowDialog(modal) {
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


        function HideDialog() {
            $("#overlay").hide();
            $("#dialog").fadeOut(300);

        }

    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PageContent" runat="Server">

    <table width="100%">
        <tr>
            <td colspan="2" id="tdmsg" runat="server">
                <asp:Label ID="lblSkills" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="">&nbsp;
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="tdText">
                <label for="email">
                    Assessment Name:</label>
            </td>
            <td>
                <asp:DropDownList ID="ddlAssess" runat="server" CssClass="drpClass">
                </asp:DropDownList>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="tdText" style="width: 100px">
                <label for="email">
                    Description:</label>
            </td>
            <td style="width: 640px">
                <asp:TextBox ID="txt_AssessDesc" runat="server" TextMode="MultiLine" Rows="5" Columns="5"
                    onKeyUp="Count(this,100)" onchange="Count(this,100)" Width="630px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdText">
                <label for="email">
                    Select Xml :</label>
            </td>
            <td>
                <asp:FileUpload ID="fu_AssessXML" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
        </tr>
        <tr>
            <td></td>
            <td>


                <asp:Button ID="btnSubmit" runat="server" ValidationGroup="a" CssClass="NFButton"
                    Text="Save" OnClick="btn_Submit_Click" />

            </td>
        </tr>
        <tr>
            <td colspan="2" id="lbl_Msg" runat="server" style="text-align: center"></td>
        </tr>
        <tr>
            <td colspan="2" id="td" runat="server" class="tdText" align="center">&nbsp;
            </td>
        </tr>
        <tr>
            <td></td>
            <td class="submit">


                <div id="overlay" class="web_dialog_overlay">
                </div>
                <div id="dialog" class="web_dialog">
                    <table style="width: 100%; height: 530px;">
                        <tr>

                            <td colspan="4" align="left">
                                <h3><span id="assmnt" runat="server"></span></h3>
                                <hr />
                            </td>
                        </tr>
                        <tr style="height: 50px;">
                            <td colspan="3" align="right"></td>
                        </tr>
                        <tr style="vertical-align: top; height: 380px;">
                            <td colspan="3" align="center">
                                <div class="tabber" style="width: 100%;">
                                    <div class="tabbertab">
                                        <h2 class="style5">Unmapped Questions</h2>
                                        <div>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td runat="server" id="tdTemp" colspan="3" align="center">Questions in the XML which does not mapped to a Lesson Plan in the Assessment-LessonPlan
                                                                Relationship Table. Click Submit to insert it to the Table.
                                                            </td>
                                                        </tr>
                                                        <tr style="vertical-align: top;" align="center">
                                                            <td colspan="3" align="center" style="overflow: auto;">
                                                                <asp:GridView ID="grdTempQtns" GridLines="none" runat="server" Width="700px" HorizontalAlign="Justify"
                                                                    Style="margin-left: 0px;" EnableSortingAndPagingCallbacks="true" OnPageIndexChanging="grdTempQtns_PageIndexChanging"
                                                                    PageSize="10" AllowPaging="True">
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
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="tabbertab">
                                        <h2 class="style5">Active Questions</h2>
                                        <div>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td runat="server" id="tdLP" colspan="3" align="center">Questions in the Assessment-LessonPlan Relationship Table which is not in the XML.
                                                                Click Submit to Inactive it
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" align="center" style="overflow: auto;">
                                                                <asp:GridView ID="grdLPQtns" runat="server" GridLines="none" Width="700px" HorizontalAlign="Justify"
                                                                    Style="margin-left: 0px;" EnableSortingAndPagingCallbacks="false" OnPageIndexChanging="grdLPQtns_PageIndexChanging"
                                                                    PageSize="10" AllowPaging="True">
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
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="tabbertab">
                                        <h2 class="style5">InActive Questions</h2>
                                        <div>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td runat="server" id="tdInactive" colspan="3" align="center">Questions in the Assessment-LessonPlan Relationship Table which is already in the
                                                                XML but InActive. Click Submit to Active
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" align="center" style="overflow: auto;">
                                                                <asp:GridView ID="grdInActive" runat="server" GridLines="none" HorizontalAlign="Justify" Style="margin-left: 0px;"
                                                                    Width="700px" PageSize="10" AllowPaging="True" OnPageIndexChanging="grdInActive_PageIndexChanging">
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
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 100px;">
                            <td align="center">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </td>
                            <td align="left">

                                <asp:Button ID="btnSbmit" runat="server" Text="Submit" CssClass="NFButton" OnClick="btnSbmit_Click" />

                            </td>
                            <td align="left">

                                <asp:Button ID="btn_Closepop" runat="server" Text="Close" CssClass="NFButton" OnClientClick="javascript:HideDialog();"
                                    OnClick="btn_Closepop_Click" />

                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
