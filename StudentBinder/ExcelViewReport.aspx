<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExcelViewReport.aspx.cs" Inherits="StudentBinder_ExcelViewReport" %>
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
        .RowStyleN td,th {
        border-style:solid;
        border-width:1px;
        border-color:lightgray;
        }     
    </style>
    <style>
    .nowrapText {
        white-space: nowrap; /* Prevent text wrapping */
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
            $('#ExcelLoadingImage').show();

        }
        function showMessage2() {
            var checkbox = document.getElementById('<%= highcheck.ClientID %>');
            if (checkbox.checked) {
                $('#ExcelLoadingImage').show();
            }
        }
        function closeMessage() {
            $('#ExcelLoadingImage').hide();
        }

        function showOverlay() {
            document.getElementById("overlay").style.display = "block";
        }
        function hideOverlay() {
            document.getElementById("overlay").style.display = "none";
            document.getElementById("downloadPopup").style.display = "block";

        }
        function CloseDownload() {
            document.getElementById("hdnExport").value = "";
            document.getElementById("downloadPopup").style.display = "none";
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
    <table style="width: 100%">
                        <tr>
                            <td colspan="6" id="tdMsg" runat="server"></td>
                        </tr>
        <tr>
            <td style="width:10%"></td>
            <td style="width:15%"></td>
            <td style="width:10%"></td>
            <td style="width:15%"></td>
            <td style="width:20%"></td>
            <td ></td>
        </tr>
         <tr>
            <td >Report Start Date</td>
            <td >
                                <asp:textbox id="txtRepStart" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:textbox>
                                </td>
            <td>Report End Date </td>
            <td>
                                <asp:textbox id="txtrepEdate" runat="server" class="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:textbox>
                            </td>
             <td>
                  <asp:checkbox id="chkLP" runat="server" text="Display LessonPlans" autopostback="false" Checked="true"></asp:checkbox>
                                <br />
                                <asp:checkbox id="chkBehavior" runat="server" text="Display Behaviors" Checked="false"></asp:checkbox>
                 
             </td>
             <td>
                <asp:CheckBox id="highcheck" runat="server"  Text=""></asp:CheckBox>
                <asp:button id="btnsubmit" runat="server" text="" cssclass="showgraph" tooltip="Show Graph" onclick="btnsubmit_Click"  OnClientClick="showMessage2()" />
                  <%--Commented to hide the grid view
                  <asp:button id="btnSession" runat="server" text="Grid View" cssclass="NFButton" tooltip="Show Grid" onclick="btnSession_Click"  OnClientClick="showMessage()" />
                  <asp:button id="btnsubmit" runat="server" text="Excel View" cssclass="NFButton" tooltip="Show Graph" onclick="btnsubmit_Click"   />--%>
             </td>
             <td>
                    <%--hide the grid view export and report button--%>
                     <asp:ImageButton ID="btnExporthigh" runat="server" style="float: left; " ImageUrl="~/Administration/images/Excelexp.png" OnClick="btnExport_Clickhigh" ToolTip="Export" OnClientClick="showOverlay();"  />
                     <asp:ImageButton ID="btnExport" runat="server" style="float: left; " ImageUrl="~/Administration/images/Excelexp.png" OnClick="btnExport_Click" ToolTip="Export" />
                    <asp:ImageButton ID="btnRefresh" runat="server" style="float: right; " Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" />
                </td>
        </tr>
        <tr>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rbtnLsnClassType" runat="server" RepeatDirection="Horizontal" visible="false">
                                    <asp:ListItem Value="Day">Day</asp:ListItem>
                                    <asp:ListItem Value="Residence">Residence</asp:ListItem>
                                    <asp:ListItem Value="Day,Residence" Selected="True">Both</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                   <td id="td1" runat="server" style="width:200px" colspan="2" visible="false"></td>
                    <td colspan="4"></td>
            <%--<td>
            </td>--%>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td><asp:checkbox id="chkFiltrColumn" runat="server" text="Do not Show Blank Columns" Checked="true"></asp:checkbox></td><!-- Excel View Not Show Blank Column [29-Jun-2020] -->
        </tr>
        <tr>
            <td colspan="7">
                <h2 id="ExcelLoadingImage" style="font-family: Calibri, sans-serif; color: #6C7598; font-size: 20px; margin-top: 15px; text-align: center; display: none;">Loading......</h2>
            </td>
        </tr>
        <tr>
            <td colspan="6"></td>
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
            <%------------------------------------DataList to List the Session Data--------------------------------------%>
            <div id="divLesson" runat="server" style="width: 100%; height: 600px; overflow: auto">
                <asp:DataList ID="dlLesson" runat="server" Style="vertical-align: top" RepeatDirection="Horizontal" OnItemDataBound="dlLesson_ItemDataBound" Width="100%">
                    <ItemStyle VerticalAlign="Top" />
                    <ItemTemplate>
                        <div id="divDetails" style="min-width:150px;">
                            <table id="tblSession" style="width: 100%;">
                                <tr>
                                    <asp:Label ID="LessonId" runat="server" Text='<%# Eval("LessonPlanId") %>' Visible="false"></asp:Label>
                                </tr>
                                <tr>
                                    <asp:Label ID="IsLP" runat="server" Text='<%# Eval("IsLP") %>' Visible="false"></asp:Label>
                                </tr>
                                <tr>
                                    <th id="thlNames" runat="server" class='lName' style="background-color: #0D668E; color: white; text-align: center; height: 30px; font-size: 14px"><%# Eval("LessonName")%></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gVSession" runat="server" Width="100%" Height="100%" AutoGenerateColumns="false" GridLines="Both" BackColor="White" BorderColor="#336666"
                                            BorderStyle="None" BorderWidth="3px" Style="width: 100%" HorizontalAlign="Justify">
                                            <RowStyle CssClass="RowStyle RowStyleN" />
                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" />
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Left" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                            
                                            <Columns>
                                                                                                
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
    

        <%------------------------------------ new DataList --------------------------------------%>
                                <asp:label runat="server" text="" id="lbl"  Font-Bold="true" font-size="10px" visible="false"></asp:label>
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
                                                    <tr style="height:61px;">

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
                            <table id="Table1" class="nowrapText" style="width: 100%;">
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
