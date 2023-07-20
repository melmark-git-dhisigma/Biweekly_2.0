<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClinicalCoversheet.aspx.cs" Inherits="StudentBinder_ClinicalCoversheet" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            height: 23px;
        }
    p.MHead1
	{margin: 3.0pt 0in;
            page-break-after:avoid;
	        punctuation-wrap:simple;
	        text-autospace:none;
	        font-size:9.0pt;
	        font-family:"Arial Black","sans-serif";
	        text-decoration:underline;
	        text-underline:single;
        }
        .auto-style3 {
            height: 19px;
        }
        .auto-style4 {
            width: 20%;
            height: 19px;
        }
        .auto-style5 {
            width: 80%;
            height: 19px;
        }
    </style>
     <script>
         function allowBackSpace(e, obj) {
             var evt = e || window.event;
             if (evt.keyCode == 8) {
                 obj.value = '';
             }
         }
    </script>
</head>
<link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />

    <script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />
    <script type="text/javascript" src="../Administration/JS/jquery-1.8.0.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.datepicker.js"></script>
    <link href="../Administration/CSS/demos.css" type="text/css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />

    <link href="../Administration/CSS/jquery.ui.base.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/jquery.ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="../Administration/JS/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.fileupload.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.iframe-transport.js" type="text/javascript"></script>

    <script src="../Administration/JS/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />

       <script type="text/javascript">
           window.onload = function () {
               $(function () {
                   $(".DatePick").datepicker();
               });
           };
               function loadDateJqry() {
                   $(function () {
                       $(".DatePick").datepicker();
                   });
               }
               
               function scrollToTop() {
                   window.scrollTo(0, 0);
                   window.parent.parent.scrollTo(0, 0);
               }

    </script>



    <style type="text/css">
        .ui-datepicker {
            font-size: 8pt !important;
        }
    </style>

    <style type="text/css">
        hr {
            border: 1px dashed gray;
        }

        .style1 {
            width: 864px;
        }

        .style2 {
            width: 20%;
        }

        .widthSettd {
            width: 20px;
        }
        .auto-style6 {
            width: 100%;
            height: 19px;
        }
    </style>



   

        </script>
<body>
    <form id="form1" runat="server">
      
        <div>
           
            <table style="width:100%">
                <tr>
                    <td class="auto-style3">

                                <asp:DropDownList ID="drpSelectDate" runat="server">
                                </asp:DropDownList>
                                <asp:Button ID="btnLoadDate" runat="server" Text="Load Report" CssClass="NFButtonWithNoImage" OnClick="btnLoadDate_Click" />

                                <asp:Button ID="btnTodayzReport" runat="server" CssClass="NFButtonWithNoImage" Text="Load Todays Report" OnClick="btnTodayzReport_Click" visible="false"/>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="PanelMain" runat="server">
              <div>
        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
        <table style="width:100%;">
            <tr>
                <td id="tdMsg" colspan="3" runat="server" class="auto-style6">
                    
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="width:100%">
                        <tr>
                            <td style="width:50%">
                                <asp:HiddenField ID="hdFldCvid" runat="server" />
                            </td>
                            <td style="width:50%">
                                 <asp:Button ID="btnExport" runat="server" Text="IEP Export" CssClass="NFButton" style="float:right" OnClick="btnExport_Click"/>
                            </td>
                        </tr>
                    </table>
                       
                </td>
            </tr>
            <tr>
                <td colspan="3" class="head_box">
                    
                        Behavior Data Summary
                        :</td>
            </tr>
            <tr>
                <td colspan="3">
        <asp:GridView ID="gVBehavior" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Identify behaviors targeted<br/>for deceleration and acceleration<br/>(List IEP objectives first)</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblBehavior" runat="server" Text='<%# Eval("Behaviour") %>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField >
                    <HeaderTemplate ></HeaderTemplate>
                    <HeaderStyle Width="20px"/>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate></HeaderTemplate>
                    <HeaderStyle Width="20px"/>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate></HeaderTemplate>
                    <HeaderStyle Width="20px"/>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate></HeaderTemplate>
                    <HeaderStyle Width="20px"/>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Number of<br/>days of<br/>data and<br/>summary<br/>unit</HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>IEP objective (copied from<br/>IEP document)</HeaderTemplate>
                </asp:TemplateField>
                  <asp:TemplateField>
                    <HeaderTemplate>Baseline or<br/>current<br/>performance<br/>level</HeaderTemplate>
                </asp:TemplateField>
                    <asp:TemplateField>
                    <HeaderTemplate>Last two IOA<br/>points</HeaderTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="head_box" colspan="3">Setting Events and Program Changes :</td>
            </tr>
            <tr>
                <td colspan="3"><i>Briefly identify variables or factors in the following areas that may help explain behavior change (Include dates and list most recent first).</i></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr >
                <td class="auto-style4"><b>Type</b></td>
                <td colspan="2" class="auto-style5"><b>Description</b></td>
            </tr>
            <tr>
                <td class="auto-style3">Phaselines</td>
                <td colspan="2" class="auto-style3">
                    <asp:Label ID="lblPhaseLine" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">Condition lines</td>
                <td colspan="2" class="auto-style3">
                    <asp:Label ID="lblConditionLines" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Arrow notes</td>
                <td colspan="2">
                    <asp:Label ID="lblArrowNotes" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2"></td>
                <td class="auto-style2"></td>
                <td class="auto-style2"></td>
            </tr>
            <tr>
                <td class="head_box" colspan="3">Assessment Tool Usage :</td>
            </tr>
            <tr>
                <td colspan="3"><i>Identify last three assessment tools that were implemented (most recent first) and apparent functions.</i></td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="width:100%;">
                        <tr>
                            <td style="width:20%;"><b>Target Behavior</b></td>
                            <td style="width:20%;"><b>Function(s)</b></td>
                            <td style="width:60%;"><b>Analysis Tools and Dates</b></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server"><ContentTemplate>
                    <asp:GridView ID="gVAssmntTool" runat="server" AutoGenerateColumns="False" ShowFooter="True" OnRowCommand="gVAssmntTool_RowCommand" OnRowDeleting="gVAssmntTool_RowDeleting" GridLines="None">
                        <Columns>
                            <asp:TemplateField HeaderText="Target Behavior">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTargetBehavior" runat="server" text='<%# Eval("TargetBehavior") %>' ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Function(s)">
                                 <ItemTemplate>
                                      <asp:TextBox ID="txtFunction" runat="server" Text='<%# Eval("Function") %>'></asp:TextBox>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Analysis Tools and Dates">
                                 <ItemTemplate>
                                      <asp:TextBox ID="txtAnalysisTools" runat="server" Text='<%# Eval("AnalysisTools") %>'></asp:TextBox>
                                 </ItemTemplate>
                                 <FooterStyle Height="40px"/>
                                 <FooterTemplate >                                    
                                     <asp:Button ID="btnAddRow" runat="server"  Text="Add Row" CommandName="AddRow" CssClass="NFButton" />
                                 </FooterTemplate>
                             </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton id="btnDelRow" runat="server" Text="X" CommandName="delete"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        
                    </asp:GridView>
                     </ContentTemplate></asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" class="head_box">Preference Assessments and Reinforcement Surveys :</td>
            </tr>
            <tr>
                <td colspan="3"><i>Identify date of most recent preference assessment or reinforcement survey.</i></td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>
                    <asp:GridView ID="gVAssmtReinfo" runat="server" AutoGenerateColumns="False" ShowFooter="True" OnRowCommand="gVAssmtReinfo_RowCommand" OnRowDeleting="gVAssmtReinfo_RowDeleting" GridLines="None">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>Date</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDate" runat="server" text='<%# Eval("Date") %>' Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>Tool Utilized</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtToolUtilized" runat="server" text='<%# Eval("ToolUtilized") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterStyle Height="40px"/>
                                 <FooterTemplate >                                     
                                     <asp:Button ID="btnAddRow" runat="server" Text="Add Row" CommandName="AddRow" CssClass="NFButton" />
                                 </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton id="btnDelRow" runat="server" Text="X" Width="10px" CommandName="delete"/>
                                </ItemTemplate>
                               
                              
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                        </ContentTemplate></asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3"  class="head_box">Follow-Up :</td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="3">
                     <asp:TextBox ID="txtFollowUp" runat="server" Rows="5"
                            TextMode="MultiLine" Width="80%" ></asp:TextBox>
                        <ajax:HtmlEditorExtender ID="ConcernsEditorExtender" runat="server" TargetControlID="txtFollowUp" ><Toolbar><ajax:Bold /><ajax:Italic /><ajax:Underline /><ajax:JustifyLeft /><ajax:JustifyCenter /><ajax:JustifyRight /><ajax:JustifyFull /><ajax:InsertOrderedList /><ajax:InsertUnorderedList /><ajax:RemoveFormat /><ajax:ForeColorSelector /><ajax:FontSizeSelector /></Toolbar></ajax:HtmlEditorExtender>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" class="head_box">Proposals :</td>
            </tr>
            <tr>
                <td colspan="3">
                <asp:TextBox ID="txtPreposals" runat="server" Rows="5"
                            TextMode="MultiLine" Width="80%" ></asp:TextBox>
                        <ajax:HtmlEditorExtender ID="HtmlEditorExtender1" runat="server" TargetControlID="txtPreposals" ><Toolbar><ajax:Bold /><ajax:Italic /><ajax:Underline /><ajax:JustifyLeft /><ajax:JustifyCenter /><ajax:JustifyRight /><ajax:JustifyFull /><ajax:InsertOrderedList /><ajax:InsertUnorderedList /><ajax:RemoveFormat /><ajax:ForeColorSelector /><ajax:FontSizeSelector /></Toolbar></ajax:HtmlEditorExtender>   
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" class="head_box">Recommended Changes :</td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:GridView ID="gVRecChange" runat="server" AutoGenerateColumns="False" ShowFooter="True" OnRowCommand="gVRecChange_RowCommand" OnRowDeleting="gVRecChange_RowDeleting" GridLines="None">
                        <Columns>
                            <asp:TemplateField HeaderText="Recommendation">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRecomd" runat="server" text='<%# Eval("Recommendation") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Timeline">
                                 <ItemTemplate>
                                      <asp:TextBox ID="txtTimeLine" runat="server" Text='<%# Eval("Timeline") %>'></asp:TextBox>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Person Responsible">
                                 <ItemTemplate>
                                      <asp:TextBox ID="txtPerRes" runat="server" Text='<%# Eval("Person Responsible") %>'></asp:TextBox>
                                 </ItemTemplate>
                                 <FooterStyle Height="40px"/>
                                 <FooterTemplate >                                    
                                     <asp:Button ID="btnAddRow" runat="server"  Text="Add Row" CommandName="AddRow" CssClass="NFButton" />
                                 </FooterTemplate>
                             </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton id="btnDelRow" runat="server" Text="X" CommandName="delete"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        
                    </asp:GridView>
                        </ContentTemplate></asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" class="head_box">Bi-Weekly Clinical Signatures of Review :</td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="width:100%;">
                        <tr>
                            <td style="width:30%">Program Coordinator</td>
                            <td style="width:30%">
                                <asp:TextBox ID="txtPgmCordntr" runat="server"></asp:TextBox>
                            </td>
                            <td style="width:10%">Date</td>
                            <td style="width:30%">
                                <asp:TextBox ID="txtDatePgnCord" runat="server" Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3" >Educational Coordinator / Lead Teacher</td>
                            <td class="auto-style3" >
                                <asp:TextBox ID="txteduCodntr" runat="server"></asp:TextBox>
                            </td>
                            <td class="auto-style3" >Date</td>
                            <td class="auto-style3" >
                                <asp:TextBox ID="txtDateEduCord" runat="server" Class="DatePick" onkeypress="return false"  onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>BCBA Clinician</td>
                            <td>
                                <asp:TextBox ID="txtBCBA" runat="server"></asp:TextBox>
                            </td>
                            <td>Date</td>
                            <td>
                                <asp:TextBox ID="txtDateBCBACord" runat="server" Class="DatePick" onkeypress="return false"  onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" style="text-align:center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="80px" CssClass="NFButton" OnClick="btnSave_Click" OnClientClick="javascript: scrollToTop();"/></td>

            </tr>
        </table>
        <br />
    </div>  
        </asp:Panel>
                    </td>
                </tr>
            </table>
        
     </div>
       
    </form>
</body>
</html>
