<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgressSummaryReport.aspx.cs" Inherits="StudentBinder_ProgressSummaryReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        .RowStyleN td,th {
        border-style:solid;
        border-width:1px;
        border-color:lightgray;
        }
        
        .auto-style1
        {
            width: 1312px;
        }
        .auto-style4
        {
            width: 255px;
        }
        .auto-style5
        {
            width: 164px;
        }
        .auto-style6
        {
            width: 32px;
        }
        .auto-style7
        {
            width: 217px;
        }
        .auto-style8
        {
            width: 301px;
        }
        
    </style>
    <style>
        .nowrapText {
            white-space: nowrap; /* Prevent text wrapping */
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

        function closeMessage() {
            $('#PSRLoadingImage').hide();
        }
        function showMessage2() {
            var checkbox = document.getElementById('<%= highcheck.ClientID %>');
            if (checkbox.checked) {
                $('#PSRLoadingImage').show();
            }
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
     <form id="form1" runat="server" >
    <div>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
        </div>
    <table class="auto-style1">
                        <tr>
                            <td colspan="8" id="tdMsg" runat="server"></td>
                        </tr>
        <tr>
            <td class="auto-style5"></td>
            <td class="auto-style5"></td>
            <td class="auto-style5"></td>
            <td class="auto-style6"></td>
            <td style="width:20%" colspan="2"></td>
            <td class="auto-style4" colspan="2" ></td>
        </tr>
         <tr>
            <td class="tdText" style="text-align: left">Report Start Date</td>
            <td >
                <asp:textbox id="txtRepStart" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:textbox>
            </td>
            <td class="tdText" style="text-align: left">Report End Date </td>
            <td>
                <asp:textbox id="txtrepEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:textbox>
            </td>
            <td colspan="2">
                <%--<asp:button id="btnsubmit" runat="server" text="" cssclass="showgraph" tooltip="Show Graph" onclick="btnsubmit_Click"   />--%>
            </td>
            <td class="auto-style4">
                    <asp:ImageButton ID="btnRefresh" runat="server" style="float: right" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" />
            </td>
            <td class="auto-style4">
                    &nbsp;</td>
        </tr>
        <tr>            
             <td colspan="2" rowspan="2">
                 <asp:CheckBoxList ID="chkStatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chkStatus_SelectedIndexChanged" >
                                <asp:ListItem Selected="True">Active</asp:ListItem> 
                                <asp:ListItem Selected="True">Maintenance</asp:ListItem>
                                <asp:ListItem>Inactive</asp:ListItem>
                            </asp:CheckBoxList>
                        
                            <asp:RadioButtonList ID="rbtnClassType" runat="server" RepeatDirection="Horizontal" BorderColor="Black" BorderStyle="None" BorderWidth="1px" Visible="False" style="margin-top: 0px">
                                <asp:ListItem Value="DAY">Day</asp:ListItem>
                                <asp:ListItem Value="RES">Residence</asp:ListItem>
                                <asp:ListItem Value="DAY,RES" Selected="True">Both</asp:ListItem>
                            </asp:RadioButtonList>
                 <table id="tableDurFormat" runat="server" visible="false">
                     <tr>
                         <td>
                 <asp:Label ID="LblDurationFormat" runat ="server" Text ="Duration Format: " AssociatedControlID="radBtnDurationFormat" style="display:inline-block"></asp:Label>
                         </td>
                         <td>
                     <asp:RadioButtonList ID="radBtnDurationFormat"  runat="server" RepeatDirection="Horizontal" BorderColor="Black" BorderStyle="None" BorderWidth="1px" style="display:inline-block; margin-top: 0px">
                     <asp:ListItem Value="0" Selected="True">HH:MM:SS </asp:ListItem>
                     <asp:ListItem Value="1">Minutes</asp:ListItem>
                     <asp:ListItem Value="2">Seconds</asp:ListItem>
                 </asp:RadioButtonList>
                             </td>
                         </tr>
                     </table>
             </td>
            
            <td class="tdText" style="text-align: left">Lesson Plan</td>
            <td>                
                <asp:DropDownCheckBoxes ID="ddlLessonplan" runat="server" TabIndex="1" AddJQueryReference="True" UseButtons="false" UseSelectAllNode="true">
                    <Style SelectBoxWidth="290px" DropDownBoxBoxWidth="450px" DropDownBoxBoxHeight="350" DropDownBoxCssClass="ddchkLesson"/>
                    <Texts SelectBoxCaption="---------------Select Lesson Plan---------------"/>
                </asp:DropDownCheckBoxes>
             </td>
            
            <td>
                <asp:CheckBox id="highcheck" runat="server"  Text=""></asp:CheckBox>
                <asp:button id="Button2" runat="server" text="Session View" cssclass="NFButton" tooltip="Show Session View" onclick="btnShow_Click" OnClientClick="showMessage()"   />

                
                <asp:button id="Button1" runat="server" text="Classic View" cssclass="NFButton" tooltip="Show Classic View" onclick="btnsubmit_Click" OnClientClick="showMessage2()" />
                
            </td>

            
            <td>
                        <asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/Administration/images/Excelexp.png" OnClick="btnExport_Click1" ToolTip="Export" />

                    <td class="auto-style4" colspan="2"></td>
                </tr>
                <tr>
                    <td></td>
                   <!-- <td id="td1a" runat="server" colspan="2" visible="false" class="auto-style7">
                        No leson selected was here
                            &nbsp;</td>-->
                    <td></td> <!-- this has been added to avoid that the above-->
                    <td colspan="3"></td>
            <%--<td>
            </td>--%>
        </tr>
        <tr>
            <td colspan="2">
                <h2 id="PSRLoadingImage" style="font-family: Calibri, sans-serif; color: #6C7598; font-size: 20px; margin-top: 15px; text-align: center; display: none;">Loading......</h2>

            </td>
            <td id="td1" runat="server" colspan="2" visible="false" class="auto-style8"> <!-- my code -->
                &nbsp;</td>
            <td colspan="4">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="8"></td>
        </tr>
        </table>
        <table style="width:100%">
        <tr>
            <td >
                 <div style="overflow: visible; width: 99%;height:100%" id="AcademicGraphReports">
                                <rsweb:reportviewer id="RV_ExcelReport" runat="server" processingmode="Remote" waitmessagefont-names="Verdana" waitmessagefont-size="14pt" showbackbutton="false" showcredentialprompts="false" showdocumentmapbutton="true" showexportcontrols="true" showfindcontrols="false" showpagenavigationcontrols="true" showparameterprompts="true" showprintbutton="false" showpromptareabutton="true" showrefreshbutton="false" showtoolbar="true" showwaitcontrolcancellink="true" showzoomcontrol="false" sizetoreportcontent="false" Width="100%" Height="1300px"  visible="false" asyncrendering="true">

                                    <ServerReport     ReportServerUrl="<%$ appSettings:ReportUrl %>" />

                              <%--    ReportPath="<%$ appSettings:ReportPath %>"--%>

                                </rsweb:reportviewer>
                            </div>
            </td>
        </tr>
        </table>
          <%------------------------------------DataList to List the Classic  Data--------------------------------------%>

         <div style="display: flex; align-items: flex-start; gap: 0; margin: 0; padding: 0;">
                       <table>
                        
                               <tr>
                                   <td>
  <asp:GridView ID="Gvclsdate" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="Both" BackColor="White" BorderColor="#336666" 
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
                                                    <tr style="height:93px;">

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
                                    <asp:Label ID="lessid" runat="server" Text='<%# Eval("LessonPlanId") %>' Visible="false"></asp:Label>
                                </tr>
                                <tr>
                                    <th id="th1" runat="server" class='bName' style="background-color: #4498c2; color: black; text-align: center; height: 30px; font-size: 14px"><%# Eval("LessonName")%></th>
                                    <th id="th2" runat="server" style="background-color: white; border-color:white; height: 30px; width: 1px;"> </th>
                                </tr>
                               
                                <tr>
                                    <td>
                                        <asp:GridView ID="Gvclsless" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="Both" BackColor="White" BorderColor="#336666"
                                            BorderStyle="None" BorderWidth="1px" Style="width: 100%" HorizontalAlign="Justify" >
                                            <RowStyle CssClass="RowStyle RowStyleN" />
                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" />
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Left" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                 </div>
             </div>

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
                                    <th id="thSpace" runat="server" style="background-color: white; border-color:white; height: 30px; width: 1px;"> </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gVSession" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="Both" BackColor="White" BorderColor="#336666"
                                            BorderStyle="None" BorderWidth="3px" Style="width: 100%" HorizontalAlign="Justify">
                                            <RowStyle CssClass="RowStyle RowStyleN" />
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
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="User Name" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server"
                                                            Text='<%# Eval("username")%>'></asp:Label>
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
                                                <asp:TemplateField ItemStyle-Width="50px" HeaderText="ClassType" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("classtype")%>'></asp:Label>
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
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Mistrial Reason" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server"
                                                            Text='<%# Eval("MisTrialRsn")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Set" HeaderStyle-HorizontalAlign="Left">
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
        <%-- Datalist for Excel Export --%>
        <div id="divexport" runat="server" style="width: 100%; height: 600px; overflow: auto" visible="false" >
            <asp:DataList ID="DataListexport" runat="server" Style="vertical-align: top" RepeatDirection="Horizontal" OnItemDataBound="DataListexport_ItemDataBound" Width="100%">
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
                                    <asp:GridView ID="gVSessionexport" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="Both" BackColor="White" BorderColor="#336666"
                                        BorderStyle="None" BorderWidth="1px" Style="width: 100%" HorizontalAlign="Justify">
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
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Set" HeaderStyle-HorizontalAlign="Left">
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
        <%-- End --%>
    </div>
    </form>
</body>
</html>

