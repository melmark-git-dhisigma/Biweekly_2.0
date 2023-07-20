<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgressSummaryReportClinical.aspx.cs" Inherits="StudentBinder_ProgressSummaryReportClinical" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="../Administration/CSS/jsDatePickforGraph.css" rel="stylesheet" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js"></script>
    <script src="../Administration/JS/jquery.min.js"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js"></script>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <style type="text/css">
        #RV_ExcelReport_AsyncWait_Wait {
            top:7% !important;
            left:5% !important;          
        } 
        .bName {
            width:100%;
            align-content:center;
            font-weight:bold;
            font-family:Arial;
            letter-spacing:0px;
            height: 25px;
        }
        .RowStyleN td,th {
        border-style:solid;
        border-width:1px;
        border-color:lightgray;
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

        function showMessage() {
            $('#PSRLoadingImage').show();
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
    <div>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </div>
        <table style="width: 100%">
            <tr>
                <td colspan="6" id="tdMsg" runat="server"></td>
            </tr>
            <tr>
                <td style="width: 10%"></td>
                <td style="width: 15%"></td>
                <td style="width: 10%"></td>
                <td style="width: 15%"></td>
                <td style="width: 19%"></td>
            <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Report Start Date</td>
                <td>
                    <asp:TextBox ID="txtRepStart" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                </td>
                <td>Report End Date </td>
                <td>
                    <asp:TextBox ID="txtrepEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" ></asp:TextBox>
                </td>
            
            <td style="width:200px">

                <asp:button id="btnSessView" runat="server" text="Session View" cssclass="NFButton" tooltip="Show Session View" onclick="btnSessView_Click" OnClientClick="showMessage()"   />

                
                <asp:button id="btnClassicView" runat="server" text="Classic View" cssclass="NFButton" tooltip="Show Classic View" onclick="btnClassicView_Click"   />
                               
                
            </td>
                <td style="width:275px">
                     <asp:ImageButton ID="btnExport" runat="server" style="float: left" ImageUrl="~/Administration/images/Excelexp.png" OnClick="btnExport_Click" ToolTip="Export" />
                    <asp:ImageButton ID="btnRefresh" runat="server" style="float: right" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" />
                </td>

            
            </tr>
            <tr>
                <td colspan="2">
                    
                    
                            <asp:RadioButtonList ID="rbtnClassType" runat="server" RepeatDirection="Horizontal" BorderColor="Black" BorderStyle="None" BorderWidth="1px" Visible="False">
                                <asp:ListItem Value="DAY">Day</asp:ListItem>
                                <asp:ListItem Value="RES">Residence</asp:ListItem>
                                <asp:ListItem Value="DAY,RES" Selected="True">Both</asp:ListItem>
                            </asp:RadioButtonList>
                    
                    
                </td>
                <td id="td1" runat="server" style="width:200px" colspan="2" visible="false">
                        
                    </td>
                <td colspan="4">
                        
                            &nbsp;</td>
            </tr>
            <tr>
                <td colspan="7">
                    <h2 id="PSRLoadingImage" style="font-family: Calibri, sans-serif; color: #6C7598; font-size: 20px; margin-top: 15px; text-align: center; display: none;">Loading......</h2>
                </td>
            </tr>
            <tr>
                <td colspan="6"></td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td>
                    <div style="overflow: visible; width: 99%; height: 100%" id="AcademicGraphReports">
                        <rsweb:ReportViewer ID="RV_ExcelReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="false" ShowCredentialPrompts="false" ShowDocumentMapButton="true" ShowExportControls="true" ShowFindControls="false" ShowPageNavigationControls="true" ShowParameterPrompts="true" ShowPrintButton="false" ShowPromptAreaButton="true" ShowRefreshButton="false" ShowToolBar="true" ShowWaitControlCancelLink="true" ShowZoomControl="false" SizeToReportContent="false" Width="100%" Height="1300px" Visible="false" AsyncRendering="true">
                            <ServerReport ReportServerUrl="<%$ appSettings:ReportUrl %>" />
                        </rsweb:ReportViewer>
                    </div>
                </td>
            </tr>
        </table>

        <%------------------------------------DataList to List the Session Data------------------------------------%>
            <div id="divBehavior" runat="server" style="width: 100%; height: 600px; overflow: auto">
                <asp:DataList ID="dlBehavior" runat="server" Style="vertical-align: top" RepeatDirection="Horizontal" OnItemDataBound="dlBehavior_ItemDataBound" Width="100%">
                    <ItemStyle VerticalAlign="Top" />
                    <ItemTemplate>
                        <div id="divDetails" style="min-width:150px;">
                            <table id="tblData" style="width: 100%;">
                                <tr>
                                    <asp:Label ID="BehaviorId" runat="server" Text='<%# Eval("MeasurementId") %>' Visible="false"></asp:Label>
                                </tr>
                                <tr>
                                    <th id="thNames" runat="server" class='bName' style="background-color: #0D668E; color: white; text-align: center; height: 30px; font-size: 14px"><%# Eval("Behaviour")%></th>
                                    <th id="thSpace" runat="server" style="background-color: white; border-color:white; height: 30px; width: 1px;"> </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gVBehvior" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="Both" BackColor="White" BorderColor="#336666"
                                            BorderStyle="None" BorderWidth="1px" Style="width: 100%" HorizontalAlign="Justify">
                                            <RowStyle CssClass="RowStyle RowStyleN" />
                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" />
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Left" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("BehvDate", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Time" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("BehvTime")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="User" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("Username")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="ClassType" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ClassType")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Duration" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("Duration")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Frequency" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("Frequency")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Yes/No" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("YesOrNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Event Name" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("EventName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="Event Type" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("EventType")%>'></asp:Label>
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
    </div>
    </form>
</body>
</html>
