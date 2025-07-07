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
               .nowrap {
        white-space: nowrap;
    }
               </style>
                <style>
        /* Overlay Styles */
         #overlay {
             display: none; /* Hidden by default */
             position: fixed;
             top: 0;
             left: 0;
             width: 100%;
             height: 100%;
             background-color: rgba(0, 0, 0, 0.7);
             color: white;
             text-align: center;
             font-size: 24px;
             z-index: 9999;
             padding-top: 200px; /* Center the text vertically */
         }
         .web_dialog11 {
            background: url("../images/smalllgomlmark.JPG") no-repeat scroll right bottom #F8F7FC;
            border: 5px solid #B2CCCA;
            color: #333333;
            display: block;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 100%;
            height: auto;
            left: 30%;
            top: 5%;
            margin-left: -190px;
            padding: 5px 5px 30px;
            position: fixed;
            display: none;            
            width: 800px;
            z-index: 102;
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
        function showMessage2() {
            var checkbox = document.getElementById('<%= highcheck.ClientID %>');
            if (checkbox.checked) {
                $('#PSRLoadingImage').show();
            }
        }
        function showMessage3() {
            var checkbox = document.getElementById('<%= highcheck.ClientID %>');
            var dlBehavior = document.getElementById('<%= dlBehavior.ClientID %>');
            if (checkbox.checked && !dlBehavior) {
                $('#PSRLoadingImage').show();
            }
        }
       
        function hideOverlay() {
            document.getElementById("downloadPopup").style.display = "block";
            $('#PSRLoadingImage').hide();

        }
        function CloseDownload() {
            document.getElementById("hdnExport").value = "";
            document.getElementById("downloadPopup").style.display = "none";
            var checkbox = document.getElementById('<%= highcheck.ClientID %>');
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
                <asp:CheckBox id="highcheck" runat="server"  Text=""></asp:CheckBox>
                <asp:button id="btnSessView" runat="server" text="Session View" cssclass="NFButton" tooltip="Show Session View" onclick="btnSessView_Click" OnClientClick="showMessage()"   />

                
                <asp:button id="btnClassicView" runat="server" text="Classic View" cssclass="NFButton" tooltip="Show Classic View" onclick="btnClassicView_Click" OnClientClick="showMessage2()"  />
                               
                
            </td>
                <td style="width:275px">
                     <asp:ImageButton ID="btnExport" runat="server" style="float: left" ImageUrl="~/Administration/images/Excelexp.png" OnClick="btnExport_Click" ToolTip="Export" OnClientClick="showMessage3()" />
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
                    <h2 id="PSRLoadingImage" runat="server" style="font-family: Calibri, sans-serif; color: #6C7598; font-size: 20px; margin-top: 15px; text-align: center; display: none;">Loading......</h2>
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

<%------------------------------------DataList to List the Classic Data------------------------------------%>
<br />
                   <div style="display: flex; align-items: flex-start; gap: 0; margin: 0; padding: 0;">
                       <table>
                          
                               <tr>
                                   <td>
  <asp:GridView ID="Gvclsbehadate" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="Both" BackColor="White" BorderColor="#336666" 
                                            BorderStyle="None" BorderWidth="1px" Style="width: 100%" HorizontalAlign="Justify" >
                                            <RowStyle CssClass="RowStyle RowStyleN" />
                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#4498c2" Font-Bold="True" />
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Left" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                           <Columns>
  <asp:TemplateField>
            <HeaderTemplate>
                <table>
                    <tr style="height:70px;">

                        <th style="border: none; text-align: center;">Date</th>
                    </tr>
                   
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="dateid2" runat="server" Text='<%# Eval("Date", "{0:MM/dd/yyyy}") %>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle BackColor="#4498c2" ForeColor="black" />
            <ItemStyle BackColor="#4498c2" ForeColor="black" CssClass="nowrap" />
        </asp:TemplateField>
    </Columns>
                                        </asp:GridView>
                                       </td>
                               </tr>
                           </table>
<div id="clsview" runat="server" style="width: 100%;  overflow: auto" visible="false">
                <asp:DataList ID="clslist" runat="server" Style="vertical-align: top" CellPadding="0" CellSpacing="0"  RepeatDirection="Horizontal" OnItemDataBound="clslist_ItemDataBound" Width="100%">
                    
                    <ItemStyle VerticalAlign="Top" />
                    <ItemTemplate>
                         

                        <div id="div2" >
                            <table id="Table1" style="width: 100%;">
                                <tr>
                                    <asp:Label ID="bhid" runat="server" Text='<%# Eval("MeasurementId") %>' Visible="false"></asp:Label>
                                </tr>
                                <tr>
                                    <th id="th1" runat="server" class='bName' style="background-color: #4498c2; color: black; text-align: center; height: 30px; font-size: 14px"><%# Eval("Behaviour")%></th>
                                    <th id="th2" runat="server" style="background-color: white; border-color:white; height: 30px; width: 1px;"> </th>
                                </tr>
                               
                                <tr>
                                    <td>
                                        <asp:GridView ID="Gvclsbeha" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="Both" BackColor="White" BorderColor="#336666"
                                            BorderStyle="None" BorderWidth="1px" Style="width: 100%" HorizontalAlign="Justify">
                                            <RowStyle CssClass="RowStyle RowStyleN" />
                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" />
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Left" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="80px" HeaderText="Date" HeaderStyle-HorizontalAlign="Left" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="dateid" runat="server" Text='<%# Eval("EvntDate", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                     <HeaderStyle BackColor="#0D668E" ForeColor="black"/>
                                                       <ItemStyle BackColor="#0D668E" ForeColor="black" CssClass="nowrap"/> 
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="200px" HeaderStyle-Width="200px" HeaderText="Time" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Timeid" runat="server" Text='<%# Eval("Time")%>'></asp:Label>
                                                    </ItemTemplate>
                                                     <HeaderStyle BackColor="#FFDEAD" ForeColor="black" />
                                                       <ItemStyle  ForeColor="black" CssClass="nowrap"/> 
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="200px" HeaderStyle-Width="200px" HeaderText="User" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="userid" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                                    </ItemTemplate>
                                                     <HeaderStyle BackColor="#FFDEAD" ForeColor="black" />
                                                       <ItemStyle  ForeColor="black" CssClass="nowrap"/> 
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="200px" HeaderStyle-Width="200px" HeaderText="Frequency" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("Frequency")%>'></asp:Label>
                                                    </ItemTemplate>
                                                     <HeaderStyle BackColor="#FFDEAD"  ForeColor="black" />
                                                       <ItemStyle  ForeColor="black" CssClass="nowrap"/> 
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="200px" HeaderStyle-Width="200px" HeaderText="Duration" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("Duration")%>'></asp:Label>
                                                    </ItemTemplate>
                                                     <HeaderStyle BackColor="#FFDEAD" ForeColor="black" />
                                                       <ItemStyle  ForeColor="black" CssClass="nowrap"/> 
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="200px" HeaderStyle-Width="200px" HeaderText="Yes/No" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("YesOrNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                     <HeaderStyle BackColor="#FFDEAD" ForeColor="black" />
                                                       <ItemStyle  ForeColor="black" CssClass="nowrap"/> 
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Condition Lines/ Arrow Notes" ItemStyle-Width="400px" HeaderStyle-Width="400px">
            <HeaderTemplate>
                <table style="width:100%; border-collapse:collapse;table-layout:fixed;">
                    <tr  >
                        <th colspan="2" style="text-align:center; white-space:nowrap;"border: none;">Condition Lines/ Arrow Notes</th>
                    </tr>
                    <tr>
                        <th style="text-align:center;width:180px;">Name</th>
                        <th style="text-align:center;width:180px;">Type</th>
                    </tr>
                </table>
            </HeaderTemplate>
           <ItemTemplate>
        <table style="width:100%; border-collapse:collapse;border-spacing: 0; border: none;table-layout:fixed;">
            <tr>
                <td style="text-align:center;color:black; width:180px;background-color:#dbe4c0;border: none;border-right: 1px solid #d3d3d3 ;">
                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("EventName") %>' CssClass="nowrap"></asp:Label>
                </td>
                <td style="text-align:center;color:black;width:180px; background-color:#dbe4c0;border: none;">
                    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("StdtSessEventType") %>' CssClass="nowrap"></asp:Label>
                </td>
            </tr>
        </table>
    </ItemTemplate>
                       <HeaderStyle BackColor="#dbe4c0" ForeColor="black" />
                        <ItemStyle BackColor="#dbe4c0" ForeColor="black"/> 
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
         <div id="overlay" runat="server">
            
            <p> please wait...</p>
                                 
        </div>
        <asp:GridView ID="exportgrid" runat="server" AutoGenerateColumns="False" 
                OnDataBound="SummaryBound" >
                

                <Columns>
                </Columns>
            </asp:GridView>
        <div id="downloadPopup" class="web_dialog11" style="width: 600px;">
             
            <div id="Div53" style="width: 700px;">


                <table style="width: 97%">
                    <tr>
                        <td colspan="2">
                            <table style="width: 85%">
                                <tr>
                                    <td runat="server" id="tdMsgExport" style="height: 50px"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">

                                    <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" OnClientClick="CloseDownload();" />

                        </td>
                        <td style="text-align: left">
                             <input type="button" value="Done" class="NFButton" id="btnDone" onclick="CloseDownload();" />
                           <%-- <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" OnClientClick="CloseDownload();" />--%>

                        </td>
                    </tr>
                </table>

            </div>
        </div>
                <asp:hiddenfield id="hdnExport" runat="server" value="" />

    </div>
    </form>
</body>
</html>
