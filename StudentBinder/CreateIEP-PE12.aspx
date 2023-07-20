<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE12.aspx.cs" Inherits="StudentBinder_CreateIEP_PE12" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="CSS/StylePE.css" rel="stylesheet" />
    <link href="CSS/StylePE15.css" rel="stylesheet" />
    <script src="js/jquery-1.8.0.min.js"></script>
    <script src="../Administration/JS/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />
    <style type="text/css">
        .auto-style1 {
            height: 19px;
        }

        .auto-style2 {
            height: 32px;
        }
    </style>


    <%--/*<link href="CSS/StylePE.css" rel="stylesheet" />*/

    /*<script src="js/jquery-1.8.0.min.js"></script>
    <script src="../Administration/JS/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />*/--%>

    <style>
        input[type=text] {
            width: 100% !important;
        }
    </style>

    <script type="text/javascript">
        // when dom is ready we initialize the UpdatePanel requests
        $(document).ready(function () {

            // Place here the first init of the DatePicker           
            $('.date').datepicker();
        });
        function GetFreetextval(content, divid) {
            if (divid == 'TextBox2') {

                document.getElementById('<%=TextBox2.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBox2.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBox2_hdn.ClientID %>').value = window.escape(content);

            }
            else if (divid == 'TextBox3') {

                document.getElementById('<%=TextBox3.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBox3.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBox3_hdn.ClientID %>').value = window.escape(content);

            }
            else if (divid == 'TextBox4') {

                document.getElementById('<%=TextBox4.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBox4.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBox4_hdn.ClientID %>').value = window.escape(content);

            }

}

function ShowQuickMsq(msg) {
    //alert('called me...');
    $('#quickDiv').html(msg);
    $('#quickDiv').show();
    //alert(msg);
    //alert($('#quickDiv').length);
}

function scrollToTop() {
    window.scrollTo(0, 0);
    window.parent.parent.scrollTo(0, 100);
}

    </script>
    <script>
        function allowBackSpace(e, obj) {
            var evt = e || window.event;
            if (evt.keyCode == 8) {
                obj.value = '';
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="quickDiv" class="quickPopUp" style="position: fixed; top: 5px; display: none; width: 300px; left: calc(50%-150px); background-color: white;">
        </div>
        <div id="divIEPP1">
            <div class="ContentAreaContainer">
                <br />
                <div class="clear"></div>

                <table cellpadding="0" cellspacing="0" width="96%">
                    <tr>
                        <td id="tdMsg" runat="server" class="top righ"></td>
                    </tr>

                    <tr>
                        <td class="righ">INDIVIDUALISED EDUCATION PROGRAM(IEP)
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" class="righ">Individual's Name :
                            <asp:Label runat="server" ID="lblStudentName"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="righ">
                            <h2 class="simble">Gifted Support Service For a Individual</h2>
                        </td>
                    </tr>
                    <tr>
                        <tr>
                            <td style="line-height: 20px;" class="righ" colspan="5">D. GIFTED SUPPORT SERVICES FOR A INDIVIDUAL IDENTIFIED AS GIFTED WHO ALSO IS IDENTIFIED AS A INDIVIDUAL WITH A DISABILITY – Support services are
                        required to assist a gifted Individual to benefit from gifted education (e.g., psychological services, parent counseling and education, counseling services,
                        transportation to and from gifted programs to classrooms in buildings operated by the school district).</td>

                        </tr>


                        <tr>
                            <td class="righ" colspan="4">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="top righ">
                                            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="GridViewsupportService" runat="server" ShowFooter="True" Style="border: 0;" AutoGenerateColumns="False" EmptyDataText="No Data Found..." Width="100%" Font-Bold="False" GridLines="None" CellPadding="2" CellSpacing="2" OnDataBound="GridViewsupportService_DataBound" OnRowCommand="GridViewsupportService_RowCommand" OnRowDeleting="GridViewsupportService_RowDeleting" OnSelectedIndexChanged="GridViewsupportService_SelectedIndexChanged" CssClass="gridStyle">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="StdtSprtSrvceId" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_sprtid" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <label>Support Service</label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtSupportService" runat="server" CssClass="textClass" MaxLength="100" Text='<%#Eval("SupportService") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="HeaderStyle" />

                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="top righ">
                                                                                <asp:LinkButton ID="lnk_Delete0" CssClass="clsbbtn" runat="server" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Button ID="ButtonAddA" runat="server" OnClick="ButtonAddA_Click"
                                                                        Text="Add New Row" CssClass="NFButton" />
                                                                </FooterTemplate>
                                                                <HeaderStyle CssClass="HeaderStyle" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>


                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>

                        <tr>
                            <td class="righ" colspan="2">E. EXTENDED SCHOOL YEAR (ESY) – The IEP team has considered and discussed ESY services, and determined that:</td>

                        </tr>

                        <tr>
                            <td class="righ" colspan="2">
                                <table style="margin-left: 15px">
                                    <tr>
                                        <td class="righ">
                                            <asp:CheckBox ID="CheckBox1" runat="server" Text="Individual is eligible for ESY based on the following information or data reviewed by the IEP team:" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:TextBox runat="server" ID="TextBox2_hdn" Text="" Style="display: none;"></asp:TextBox>
                                            <div id="TextBox2" runat="server" class="FreeTextDivContent" style="overflow: scroll; width: 100%" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE12.aspx',this); "></div>

                                            <br />
                                            OR</td>
                                    </tr>
                                    <tr>
                                        <td class="righ">

                                            <asp:CheckBox ID="CheckBox2" runat="server" Text="As of the date of this IEP, Individual is NOT eligible for ESY based on the following information or data reviewed by the IEP team:" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:TextBox runat="server" ID="TextBox3_hdn" Text="" Style="display: none;"></asp:TextBox>
                                            <div id="TextBox3" runat="server" class="FreeTextDivContent" style="overflow: scroll; width: 100%" onclick=" scrollToTop(); parent.freeTextPopup('CreateIEP-PE12.aspx',this);"></div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="righ" style="line-height: 20px;">The Annual Goals and, when appropriate, Short Term Objectives from this IEP that are to be addressed in the Individual’s ESY Program are:</td>
                                    </tr>
                                    <tr>
                                        <td class="righ">
                                            <asp:TextBox runat="server" ID="TextBox4_hdn" Text="" Style="display: none;"></asp:TextBox>
                                            <div id="TextBox4" runat="server" class="FreeTextDivContent" style="overflow: scroll; width: 100%" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE12.aspx',this); "></div>
                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>

                        <tr>
                            <td class="righ" colspan="3">If the IEP team has determined ESY is appropriate, complete the following:</td>

                        </tr>

                        <tr>
                            <td class="righ" colspan="3">
                              <%-- // <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>--%>
                                        <asp:GridView ID="GridViewESY" runat="server" ShowFooter="True" Style="border: 0;" AutoGenerateColumns="False" EmptyDataText="No Data Found..." Width="100%" Font-Bold="False" GridLines="None" CellPadding="2" CellSpacing="2" OnRowCommand="GridViewESY_RowCommand" OnRowDeleting="GridViewESY_RowDeleting" OnSelectedIndexChanged="GridViewESY_SelectedIndexChanged" CssClass="gridStyle">
                                            <Columns>
                                                <asp:TemplateField HeaderText="StdtESYId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <label>ESY Service to be Provided</label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtESY" runat="server" CssClass="textClass" MaxLength="100" Text='<%#Eval("ESY") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <label>Location</label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtLocation" runat="server" CssClass="textClass" MaxLength="100" Text='<%#Eval("Location") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <label>Frequency</label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtFrequency" runat="server" CssClass="textClass" MaxLength="100" Text='<%#Eval("Frequency") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <label>Projected Beginning Date</label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtPrjBegDate" runat="server" class="date" onkeypress="return false" Text='<%#Eval("PrjBegDate") %>'  onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <label>Anticipated Duration</label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtAnticipatedDue" runat="server" CssClass="textClass" MaxLength="100" Text='<%#Eval("AnticipatedDue") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="HeaderStyle" />


                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td class="righ top">
                                                                    <asp:LinkButton ID="lnk_Delete0" runat="server" CssClass="clsbbtn" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAdd_Click"
                                                            Text="Add New Row" CssClass="NFButton" />
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                </asp:TemplateField>

                                            </Columns>

                                        </asp:GridView>
                                    <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>


                            </td>

                        </tr>


                        <tr>
                            <td></td>
                            <td class="righ">&nbsp;</td>
                        </tr>



                        <tr>
                            <td>
                                <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage"
                                    OnClick="btnSave_Click" Text="Save and continue" />

                                <%--  <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_hdn_Click" Text="dummy"   style="display:none;"/>--%>
                            </td>
                            <td class="righ" style="text-align: center">&nbsp;</td>
                        </tr>
                </table>
                <div class="clear">
                    <asp:HiddenField ID="HdnText1" runat="server" />
                    <asp:HiddenField ID="HdnText2" runat="server" />
                    <asp:HiddenField ID="HdnText3" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
