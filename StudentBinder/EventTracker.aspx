<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EventTracker.aspx.cs" Inherits="StudentBinder_Event" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="../Administration/CSS/jsDatePick_ltr.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />

    <script type="text/javascript" src="../Administration/JS/jquery-1.8.0.min.js"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    




    <script type="text/javascript">

        $(function () {
            // Patch fractional .x, .y form parameters for IE10.
            if (typeof (Sys) !== 'undefined' && Sys.Browser.agent === Sys.Browser.InternetExplorer && Sys.Browser.version === 10) {
                Sys.WebForms.PageRequestManager.getInstance()._onFormElementActive = function Sys$WebForms$PageRequestManager$_onFormElementActive(element, offsetX, offsetY) {
                    if (element.disabled) {
                        return;
                    }
                    this._activeElement = element;
                    this._postBackSettings = this._getPostBackSettings(element, element.name);
                    if (element.name) {
                        var tagName = element.tagName.toUpperCase();
                        if (tagName === 'INPUT') {
                            var type = element.type;
                            if (type === 'submit') {
                                this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                            }
                            else if (type === 'image') {
                                this._additionalInput = encodeURIComponent(element.name) + '.x=' + Math.floor(offsetX) + '&' + encodeURIComponent(element.name) + '.y=' + Math.floor(offsetY);
                            }
                        }
                        else if ((tagName === 'BUTTON') && (element.name.length !== 0) && (element.type === 'submit')) {
                            this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                        }
                    }
                };
            }
        });

    </script>

    <script type="text/javascript" language="javascript">

        window.onload = function () {
            //$(document).ready(function () {
            new JsDatePick({
                useMode: 2,
                target: "<%=txtSdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });

            //var theControl = document.getElementById("txtEdate");
            //if (theControl.style.display == "block") {
                new JsDatePick({
                    useMode: 2,
                    target: "<%=txtEdate.ClientID%>",
                    dateFormat: "%m/%d/%Y",
                });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtSdate0.ClientID%>",
                    dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtEdate1.ClientID%>",
                    dateFormat: "%m/%d/%Y",
             });
            //}
        };

        function disp() {
           
            new JsDatePick({
                useMode: 2,
                target: "<%=txtSdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtEdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
             });
        }
        function btnenter() {
        }
        function btnsearch() {
        }

        function deleteConfirm() {
            var flag;
            flag = confirm("Are you sure you want to delete this event?");
             window.parent.parent.scrollTo(0, 0);
            return flag;
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

    <style type="text/css">
        .table {
            width: 100%;
        }
    </style>

    <style type="text/css">
        .Initial {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            /*background: url("../Images/InitialImage.png") no-repeat right top;*/
            background-color: #0054A0;
            color: Black;
            font-weight: bold;
        }

            .Initial:hover {
                color: White;
                /*background: url("../Images/SelectedButton.png") no-repeat right top;*/
                background-color: #0099B5;
            }

        .Clicked {
            float: left;
            display: block;
            /*background: url("../Images/SelectedButton.png") no-repeat right top;*/
            background-color: #1EB53A;
            padding: 4px 18px 4px 18px;
            color: Black;
            font-weight: bold;
            color: White;
        }
        .auto-style2 {
            width: 78px;
            height: 47px;
        }
        .auto-style4 {
            width: 151px;
            height: 47px;
        }
        .auto-style6 {
            height: 19px;
        }
        .auto-style7 {
            width: 119px;
            height: 41px;
        }
        .auto-style8 {
            width: 92px;
            height: 41px;
        }
        .auto-style10 {
            height: 40px;
            width: 92px;
        }
        .auto-style11 {
            width: 202px;
            height: 41px;
        }
        .auto-style12 {
            height: 19px;
            width: 70px;
        }
        .auto-style13 {
            width: 202px;
            height: 47px;
        }
        .auto-style14 {
            width: 119px;
            height: 47px;
        }
        .auto-style15 {
            height: 47px;
        }
        .auto-style16 {
            height: 41px;
        }
        .auto-style17 {
            height: 40px;
        }
        .auto-style18 {
            height: 40px;
            width: 70px;
        }        
    </style>

</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <table style="width: 100%;" class="table">


            <tr>
                <td id="td1" runat="server" style="width: 97%">
                   <asp:LinkButton ID="Linkbtnenter" runat="server" onclick="btn" OnClientClick="btnenter()">Enter New Event |</asp:LinkButton>
                    <%--<td | </td>--%>
                        <asp:LinkButton ID="Linkbtnserch" runat="server" onclick="btn1" OnClientClick="btnsearch()" > Search Events</asp:LinkButton>
                    </td>
                <td style="width: 3%; text-align: right">
                    &nbsp;</td>
            </tr>


            <tr>
                <td id="tdMsg" runat="server" style="width: 97%"></td>
                <td style="width: 3%; text-align: right">
                    <asp:ImageButton ID="btnRefresh" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" />
                </td>
            </tr>

            <tr>
                <td colspan="2">

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <table style="width: 100%;">
                                <tr>

                                    <td class="tdText" style="text-align: left">Event Name

                                    </td>
                                    <td valign="top" align="left">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 1%;">
                                                    <span style="color: red">*</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txteventname" runat="server" CssClass="textClass"
                                                        Width="275px" ></asp:TextBox>
                                                </td>
                                            </tr>

                                        </table>


                                    </td>

                                    <td style="text-align: left" class="tdText">
                                        <asp:Label ID="lblcomment" runat="server" Text="Comments"></asp:Label>
                                    </td>
                                    <td valign="top" align="left">

                                        <table style="width: 100%;">

                                            <tr>
                                                <td style="width: 1%;">
                                                    <%--<span style="color: red">*</span>--%>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtComment" runat="server" Width="350"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>

                                </tr>
                                <tr>

                                    <td class="tdText" style="text-align: left">Type
                            
                                    </td>
                                    <td valign="top" align="left">
                                        <table style="width: 100%;">

                                            <tr>
                                                <td style="width: 1%;">
                                                    <span style="color: red">*</span>
                                                </td>
                                                <td class="tdText">
                                                    <asp:RadioButtonList ID="ddlType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" RepeatColumns="4" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                        <asp:ListItem Text="Major" Value="0">Major</asp:ListItem>
                                                        <asp:ListItem Text="Minor" Value="1">Minor</asp:ListItem>
                                                        <asp:ListItem Text="Arrow" Value="2">Arrow notes</asp:ListItem>
                                                        <asp:ListItem Text="Medication" Value="3">Medication</asp:ListItem>
                                                    </asp:RadioButtonList>



                                                </td>
                                            </tr>
                                        </table>


                                    </td>

                                    <td style="text-align: left" class="tdText">

                                        <asp:Label ID="lblSdate" runat="server" Text="Date"></asp:Label>

                                    </td>
                                    <td valign="top" align="left">

                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 1%;"><span style="color: red">*</span>
                                                </td>
                                                <td style="width: 10%;">

                                                    <asp:TextBox ID="txtSdate" runat="server" Width="100px" CssClass="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>


                                    </td>

                                </tr>
                                <tr>

                                    <td class="tdText" style="text-align: left">Lesson Plan</td>
                                    <td valign="top" align="left">

                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 1%;"><span style="color: white">*</span></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlLessonplan" runat="server" CssClass="drpClass"
                                                        Height="26px" Width="290px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                        </table>


                                    </td>

                                    <td style="text-align: left" class="tdText" id="lbl_edate" valign="middle">
                                        <asp:Label ID="lbledate" runat="server"  Text="End Date"></asp:Label>
                                    </td>

                                    <td valign="top" align="left">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 1%;"><span style="color: white">*</span></td>
                                                <td style="width: 10%;">

                                                    <asp:TextBox ID="txtEdate" runat="server" Width="100px"  CssClass="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                <tr>

                                    <td class="tdText" style="text-align: left">Behavior</td>
                                    <td valign="top" align="left">

                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 1%;"><span style="color: white">*</span></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBehavior" runat="server" CssClass="drpClass"
                                                        Height="26px" Width="290px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                        </table>


                                    </td>

                                    <td style="text-align: right" valign="top">&nbsp;</td>
                                    <td valign="top" align="left">&nbsp;</td>

                                </tr>
                            </table>


                        </ContentTemplate>
                    </asp:UpdatePanel>




                </td>
            </tr>

            <tr>
                <td align="center" colspan="2">


                    <asp:Button ID="btnSave" runat="server" CssClass="NFButton" OnClick="btnSave_Click" Text="Save" />


                </td>


            </tr>

            <tr>
                <td align="center" colspan="2">


                    <asp:Panel ID="Panel1" runat="server">
                        <table class="table">
                            <tr>
                                <td class="auto-style2">Lesson Plan</td>
                                <td class="auto-style13">
                                    <asp:DropDownList ID="ddlLessonplan0" runat="server" CssClass="drpClass" Height="26px" Width="290px">
                                    </asp:DropDownList>
                                </td>
                                <td class="auto-style14">&nbsp;&nbsp;&nbsp; Start Date&nbsp; </td>
                                <td class="auto-style4" colspan="2">
                                    <asp:TextBox ID="txtSdate0" runat="server" CssClass="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" Width="100px"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="auto-style15"></td>
                            </tr>
                            <tr>
                                <td class="auto-style16">Behavior&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
                                <td class="auto-style11">
                                    <asp:DropDownList ID="ddlBehavior1" runat="server" CssClass="drpClass" Height="26px" Width="290px">
                                    </asp:DropDownList>
                                </td>
                                <td class="auto-style7">&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lbledate0" runat="server" Text="End Date"></asp:Label>
                                    &nbsp;</td>
                                <td class="auto-style8">
                                    <asp:TextBox ID="txtEdate1" runat="server" CssClass="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" Width="100px"></asp:TextBox>
                                </td>
                                <td colspan="2" class="auto-style16"></td>
                            </tr>
                            <tr>
                                <td class="auto-style17" colspan="3">
                                    <asp:RadioButtonList ID="RadioButtonevent" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem>Major</asp:ListItem>
                                        <asp:ListItem>Minor</asp:ListItem>
                                        <asp:ListItem>Arrow Notes</asp:ListItem>
                                        <asp:ListItem>LP Modified</asp:ListItem>
                                        <asp:ListItem>Medication</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td class="auto-style10">&nbsp;</td>
                                <td class="auto-style18">&nbsp;</td>
                                <td class="auto-style17">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style17" colspan="3">&nbsp;</td>
                                <td class="auto-style10">&nbsp;</td>
                                <td class="auto-style18">&nbsp;</td>
                                <td class="auto-style17">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style6" colspan="2">&nbsp;</td>
                                <td class="auto-style6">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="NFButton" OnClick="btnsearch_Click" Text="Search" />
                                </td>
                                <td class="auto-style6">
                                    <asp:ImageButton ID="btnExport0" runat="server" ImageUrl="~/Administration/images/Excelexp.png" OnClick="btnExport_Click" style="float: left; " />
                                </td>
                                <td class="auto-style12">&nbsp;</td>
                                <td class="auto-style6">&nbsp;</td>
                            </tr>
                        </table>
                    </asp:Panel>


                </td>


            </tr>
            <tr>

                <td colspan="2" align="right">
                    <asp:HiddenField ID="hdneventId" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:HiddenField ID="hdnStdtSessionHdrId" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:HiddenField ID="hdnBehaviorIOAId" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2"></td>
            </tr>



        </table>


        <table width="100%">
            <tr>
                <td>
                    <asp:Button Text="Events" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
                        OnClick="Tab1_Click" ForeColor="White" />
                    &nbsp;&nbsp; 
          <asp:Button Text="Medication" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
              OnClick="Tab2_Click" ForeColor="White" />

                    <asp:MultiView ID="MainView" runat="server">
                        <asp:View ID="View1" runat="server">
                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                <tr>
                                    <td>
                                        
                                        <asp:GridView ID="grdGroup" runat="server" AutoGenerateColumns="False" Width="100%"
                                            AllowPaging="True" OnPageIndexChanging="grdGroup_PageIndexChanging" OnRowCommand="grdGroup_RowCommand"
                                            OnRowDeleting="grdGroup_RowDeleting" OnRowEditing="grdGroup_RowEditing"
                                            GridLines="None" CellPadding="4" ForeColor="#333333" EmptyDataText="No Data Found.....">
                                            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                                            <RowStyle CssClass="RowStyle" />
                                            <FooterStyle CssClass="FooterStyle" Font-Bold="True" ForeColor="White" />
                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                            <EditRowStyle BackColor="#7C6F57" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />

                                            <Columns>

                                                <asp:BoundField DataField="EventName" HeaderText="Event Name" />
                                                <asp:BoundField DataField="StdtSessEventType" HeaderText="Event Type" />
                                                <asp:BoundField DataField="Comment" HeaderText="Comment" />
                                                <asp:BoundField DataField="LessonPlanName" HeaderText="Lesson Plan" />
                                                <asp:BoundField DataField="Behaviour" HeaderText="Behavior" />
                                                <asp:BoundField DataField="EvntTs" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date" />
                                                <asp:BoundField DataField="CreatedOn" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Created On" />
                                                <asp:BoundField DataField="ModifiedOn" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Modified On" />
                                                <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lb_Edit" CommandName="Edit" runat="server" CommandArgument='<%# Eval("StdtSessEventId")+","+Eval("LessonPlanId")+","+Eval("MeasurementId")+","+Eval("StdtSessionHdrId") %>'
                                                            ImageUrl="~/Administration/images/user_edit.png" class="btn btn-blue" Width="18px" onclick="loadenterdata" OnClientClick="javascript: window.parent.parent.scrollTo(0, 0);"></asp:ImageButton>

                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lb_delete"
                                                            CommandName="Delete" runat="server" OnClientClick="javascript: return deleteConfirm();" CommandArgument='<%# Eval("StdtSessEventId")+","+Eval("StdtSessionHdrId")+","+Eval("BehaviorIOAId") %>'
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
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                            <SortedAscendingHeaderStyle BackColor="#487575" />
                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                            <SortedDescendingHeaderStyle BackColor="#275353" />
                                        </asp:GridView>

                                          

                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                <tr>
                                    <td>

                                        <asp:GridView ID="GrdMedication" runat="server" AutoGenerateColumns="False" Width="100%"
                                            AllowPaging="True" OnPageIndexChanging="GrdMedication_PageIndexChanging" OnRowCommand="GrdMedication_RowCommand"
                                            OnRowDeleting="GrdMedication_RowDeleting" OnRowEditing="GrdMedication_RowEditing"
                                            GridLines="None" CellPadding="4" ForeColor="#333333" EmptyDataText="No Data Found.....">
                                            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                                            <RowStyle CssClass="RowStyle" />
                                            <FooterStyle CssClass="FooterStyle" Font-Bold="True" ForeColor="White" />
                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                            <EditRowStyle BackColor="#7C6F57" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />

                                            <Columns>

                                                <asp:BoundField DataField="EventName" HeaderText="Name" />
                                                <asp:BoundField DataField="Comment" HeaderText="Dosage" />
                                                <asp:BoundField DataField="EvntTs" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                                                <asp:BoundField DataField="EndTime" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
                                                <asp:BoundField DataField="CreatedOn" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Created On" />
                                                <asp:BoundField DataField="ModifiedOn" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Modified On" />
                                                <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lb_Edit" CommandName="Edit" runat="server" CommandArgument='<%# Eval("StdtSessEventId") %>'
                                                            ImageUrl="~/Administration/images/user_edit.png" class="btn btn-blue" Width="18px" OnClientClick="javascript: window.parent.parent.scrollTo(0, 0);"></asp:ImageButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lb_delete"
                                                            CommandName="Delete" runat="server" OnClientClick="javascript: return deleteConfirm();" CommandArgument='<%# Eval("StdtSessEventId") %>'
                                                            ImageUrl="~/Administration/images/trash.png" class="btn btn-red" Width="18px" ></asp:ImageButton>
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
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                            <SortedAscendingHeaderStyle BackColor="#487575" />
                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                            <SortedDescendingHeaderStyle BackColor="#275353" />
                                        </asp:GridView>


                                    </td>
                                </tr>
                            </table>
                        </asp:View>

                    </asp:MultiView>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
