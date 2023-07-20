<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE10.aspx.cs" Inherits="StudentBinder_CreateIEP_PE10" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        input[type=text] {
            width: 100% !important;
        }
    </style>
    <link href="CSS/StylePE.css" rel="stylesheet" />
    <script src="js/jquery-1.8.0.min.js"></script>
    <link href="CSS/StylePE15.css" rel="stylesheet" />

    <script type="text/javascript">
        //TextBoxTrainingGoal TextBoxTrainingCourse TextBoxEmploymentGoal TextBoxEmploymentCourse TextBoxIndependentLivingGoal TextBoxIndependentLivingCourse
        var txt1 = "";
        

        function submitClick() {
            loadValues();

            $.ajax(
          {

              type: "POST",
              url: "CreateIEP-PE10.aspx/submitIepPE10",
              data: "{'arg1':'" + txt1 + "'}",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              async: false,
              success: function (data) {

                  var contents = data.d;

              },
              error: function (request, status, error) {
                  //alert("Error");
              }
          });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div id="divIEPP1">

              <div class="ContentAreaContainer">
                 <br />
                 <div class="clear"></div>
             <table cellpadding="0" cellspacing="0" width="96%">
                 <tr>
                    <td id="tdMsg" runat="server" class="top righ"></td>
                </tr>

                <tr>
                    <td class="righ" >INDIVIDUALISED EDUCATION PROGRAM(IEP)
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0"> 
                <tr>
                    <td colspan="2" class="righ">Individual's Name : <asp:Label runat="server" id="lblStudentName"></asp:Label></td>
                </tr>
				<tr>
                 <td class="righ"><h2 class="simble">GOALS AND OBJECTIVES</h2>
                    </td>
					</tr>
                <tr>
                    <td class="righ " style="line-height: 20px;">GOALS AND OBJECTIVES – Include, as appropriate, academic and functional goals. Use as many copies of this page as needed to plan appropriately.Specially designed instruction may be listed with each goal/objective or listed in Section VI.</td>
                </tr>
                <tr>
                    <td class="auto-style1 righ">&nbsp;</td>
                </tr>
                <tr>
                    <td style="line-height: 20px;" class="righ">Short term learning outcomes are required for Individuals who are gifted. The short term learning outcomes related to the Individual’s gifted program may be listed under Goals or Short Term Objectives.</td>
                </tr>
                <tr>
                    <td class="righ">

                        <asp:UpdatePanel ID="updPanel1" runat="server">

                            <ContentTemplate>
                                <asp:GridView ID="gvDelTypeA" runat="server" AutoGenerateColumns="False" GridLines="None" 
                                    ShowFooter="True" OnRowDataBound="gvDelTypeA_RowDataBound"
                                    OnRowCommand="gvDelTypeA_RowCommand" Style="z-index: 1" CssClass="gridStyle">

                                    <Columns>
                                        <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <label>
                                                    MEASURABLE ANNUAL GOAL<br />
                                                    Include: Condition, Name, Behavior, and Criteria<br />
                                                    (Refer to Annotated IEP for description of these components)</label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <br />
                                                <asp:DropDownList ID="ddlGoals" runat="server">
                                                </asp:DropDownList>
                                                 <asp:HiddenField ID="hfGoalID" runat="server" Value='<%# Bind("GoalID") %>' />
                                                <table>
                                                    <tr>
                                                        <td class="tdText">
                                                            <asp:TextBox ID="TxtMeasureAnualGoal" CssClass="textClass" runat="server" MaxLength="100" Text='<%#Eval("MeasureAnualGoal") %>'></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <label>
                                                    Describe HOW the
                                            <br />
                                                    Individual’s progress toward
                                            <br />
                                                    meeting this goal will be measured</label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="text-align: center;" class="righ">
                                                            <asp:TextBox ID="TxtStudentsProgress" runat="server" MaxLength="100" CssClass="textClass" Text='<%#Eval("StudentsProgress") %>'></asp:TextBox>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <label>
                                                    Describe WHEN periodic
                                            <br />
                                                    reports on progress will be<br />
                                                    provided to parents</label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td class="tdText top righ" style="text-align: center;">
                                                            <asp:TextBox ID="TxtDescReportProgress" runat="server" CssClass="textClass" MaxLength="100" Text='<%#Eval("DescReportProgress") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <label>Report of Progress</label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td class="tdText top righ" style="text-align: center;">
                                                            <asp:TextBox ID="TxtReportProgress" runat="server" CssClass="textClass" MaxLength="100" Text='<%#Eval("ReportProgress") %>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                           
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td class="top righ">
                                                            <asp:LinkButton ID="lnk_Delete0" runat="server" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                             <FooterTemplate>
                                                <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAdd_Click"
                                                    Text="Add New Row" CssClass="NFButton" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>



                    </td>
                </tr>
                
                <tr>
                    <td class="righ" style="line-height: 20px;">SHORT TERM OBJECTIVES – Required for Individuals with disabilities who take alternate assessments aligned to alternate achievement standards (PASA).</td>
                </tr>
                
                <tr>
                    <td class="righ">
                       
                                    <%--<div id="TextBoxBenchmarks" runat="server" class="FreeTextDivContent" onclick="parent.freeTextPopup('CreateIEP-PE10.aspx',this);"></div>--%>
                             <table border="0" cellpadding="0" cellspacing="0" >
                            <tr>
                                <td class="top righ">
                                    
                                    <asp:GridView ID="GridViewBenchmark" runat="server" ShowFooter="True"  Style="border: 0;" AutoGenerateColumns="False" EmptyDataText="No Data Found..." Width="100%"  Font-Bold="False" GridLines="None"  CellPadding="2" CellSpacing="2" OnDataBound="GridViewBenchmark_DataBound" OnRowCommand="GridViewBenchmark_RowCommand" OnRowDataBound="GridViewBenchmark_RowDataBound" CssClass="gridStyle">
                            <Columns>
                                <asp:TemplateField HeaderText="StdtSprtSrvceId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_sprtid" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField >
                                   <HeaderTemplate>
                                       <label>Short term objectives / Benchmarks</label>                                       
                                   </HeaderTemplate>
                                   <ItemTemplate>
                                       <asp:TextBox ID="TxtSupportService" runat="server" CssClass="textClass" MaxLength="100" Text='<%#Eval("Benchmark") %>'></asp:TextBox>
                                   </ItemTemplate> 
                                      <HeaderStyle CssClass="HeaderStyle" />
                                                                      
                               </asp:TemplateField> 
                                <asp:TemplateField>
                                    <ItemTemplate>
                                         <table border="0" cellpadding="0" cellspacing="0" >
                                            <tr>
                                                <td class="top righ">
                                                    <asp:LinkButton ID="lnk_Delete0" CssClass="clsbbtn" runat="server" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HeaderStyle" />
                                     <FooterTemplate>
                                           <asp:Button ID="ButtonAddA" runat="server" OnClick="ButtonAddA_Click"
                                                    Text="Add New Row" CssClass="NFButton" />
                                    </FooterTemplate>  
                                </asp:TemplateField>
                             </Columns>
                            
                        </asp:GridView>
                               
                                </td>
                            </tr>
                        </table>        
                    </td>
                </tr>
              

                <tr>
                    <td class="righ">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_Click" Text="Save and continue"/>

                      <%--  <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_hdn_Click" Text="dummy" style="display:none;"/>--%>
                    </td>
                </tr>
            </table>
                   <div class="clear"></div>
                 </div>
        </div>
    </form>
</body>
</html>
