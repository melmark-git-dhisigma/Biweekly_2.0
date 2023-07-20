<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgressSummaryNew.aspx.cs" Inherits="StudentBinder_ProgressSummaryNew" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="../Administration/CSS/jsDatePickforGraph.css" rel="stylesheet" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js"></script>

    <style type="text/css">
        #RV_ExcelReport_AsyncWait_Wait {
            top:7% !important;
            left:5% !important;          
        }
        .ddchkLesson {
            display:inline-block;
            border: 2px solid #808080;
            color: #676767;
            overflow: auto;
            padding: 4px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 13px;
            line-height: 15px;
            border-radius: 3px;
        }
        .lName {
            width:100%;
            align-content:center;
            font-weight:bold;
            font-family:Arial;
            letter-spacing:0px;
            height: 25px;
        }
        .thHead {
            text-align: left;
            font-weight:bold;
            font-family:Arial;
            height: 20px;
        }
        .tdData {            
            font-family:Arial;
            font-size:12px;
            height: 25px;           
        }
        .tbrow {
            background-color:#efeded;
            border-bottom: 0px solid #e9e9e9;            
        }
        #divSession {
            width:100%;
            height:100px;
        }
        
        
    </style>
    <script type="text/javascript">
        window.onload = function () {
            new JsDatePick({
                useMode: 2,
                target: "<%=txtRepStart.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtrepEdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
        };
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
        <div>
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </div>
            <table style="width: 100%">
                <tr>
                    <td colspan="6" id="tdMsg" runat="server" visible="false"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 10%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 20%"></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="tdText" style="text-align: left">Report Start Date</td>
                    <td>
                        <asp:TextBox ID="txtRepStart" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                    </td>
                    <td class="tdText" style="text-align: left">Report End Date </td>
                    <td>
                        <asp:TextBox ID="txtrepEdate" runat="server" class="textClass" onkeypress="return false"  onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnRefresh" runat="server" Style="float: right" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBoxList ID="chkStatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chkStatus_SelectedIndexChanged">
                            <asp:ListItem Selected="True">Active</asp:ListItem>
                            <asp:ListItem Selected="True">Maintenance</asp:ListItem>
                            <asp:ListItem>Inactive</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>

                    <td class="tdText" style="text-align: left">Lesson Plan</td>
                    <td>
                        <asp:DropDownCheckBoxes ID="ddlLessonplan" runat="server" TabIndex="1" AddJQueryReference="True" UseButtons="false" UseSelectAllNode="true">
                            <Style SelectBoxWidth="290px" DropDownBoxBoxWidth="450px" DropDownBoxBoxHeight="350" DropDownBoxCssClass="ddchkLesson" />
                            <Texts SelectBoxCaption="---------------Select Lesson Plan---------------" />
                        </asp:DropDownCheckBoxes>
                    </td>

                    <td style="width: 100px">
                        <asp:Button ID="btnShow" runat="server" Text="" CssClass="showgraph" ToolTip="Show Graph" OnClick="btnShow_Click" />
                        <asp:Button ID="btnExport" runat="server" Text="" ToolTip="Export To Excel"  CssClass="pdfPrint" OnClick="btnExport_Click" Visible="false"/>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                   <td id="td1" runat="server" style="width:200px" colspan="2" visible="false"></td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td colspan="6"></td>
                </tr>
            </table>

            <%------------------------------------DataList to List the Session Data--------------------------------------%>
            <div id="divLesson" runat="server" style="width: 100%; height: 600px; overflow: auto">
                <asp:DataList ID="dlLesson" runat="server" Style="vertical-align: top" RepeatDirection="Horizontal" OnItemDataBound="dlLesson_ItemDataBound" Width="100%">
                    <ItemStyle VerticalAlign="Top" />
                    <ItemTemplate>
                        <div id="divDetails">
                            <table id="tblSession" style="width: 100%;">
                                <tr>
                                    <asp:Label ID="LessonId" runat="server" Text='<%# Eval("LessonPlanId") %>' Visible="false"></asp:Label>
                                </tr>
                                <tr>
                                    <th id="thlNames" runat="server" class='lName' style="background-color: #0D668E; color: white; text-align: center; height: 30px; font-size: 14px"><%# Eval("LessonName")%></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gVSession" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="None" BackColor="White" BorderColor="#336666"
                                            BorderStyle="None" BorderWidth="3px" Style="width: 100%" HorizontalAlign="Justify">
                                            <RowStyle CssClass="RowStyle" />
                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" />
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Left" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                            <%--<HeaderStyle HorizontalAlign="Left" />--%>
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("SessDate", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Time" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("SessTime")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Column Measure" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("columnMeasure")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Session" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("SessNumber")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Score" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("Score")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Is Maintanace" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("IsSetInMNT")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="IOA" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("IOA")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Mistrial" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("MisTrial")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("SetName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Prompt" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("Prompt")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Event Name" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("EventName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Type" HeaderStyle-HorizontalAlign="Left"> 
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("EventType")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <%----------------------------------------------End DataList--------------------------------------------------%>
        </div>
    </form>
</body>
</html>
