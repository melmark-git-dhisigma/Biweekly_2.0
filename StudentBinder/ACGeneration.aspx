<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ACGeneration.aspx.cs" Inherits="Administration_ACGeneration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../Administration/JS/jquery-1.8.0.min.js"></script>
     <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
      <script type="text/javascript">

          function showWait() {
              $('#PlzWait').show();
          }
          function HideWait() {
              $('#PlzWait').hide();
          }
      </script>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            height: 23px;
        }
        
        #divAllContainer {
            width:100%;
            margin-left:auto;
            margin-right:auto;
            min-width:900px;
        }
        .gridAlgn {
            width:100%;
            /*margin-left:4%;*/
            margin-right:auto;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width:100%">
            <tr>
                <td style="width:13%;"></td>

                <td style="width:70%;">
                     <img runat="server" alt="Please Wait..." id="PlzWait" src="../Administration/images/PleaseWait.gif" style="width: 40px; height: 40px; display: none" />
                </td>
                <td style="width:10%;">
                   <asp:Button ID="btnImport" style="float:right;display:inline" runat="server" CssClass="NFButtonWithNoImage" OnClientClick="showWait();"  OnClick="btnImport_Click" Text="Export To Word" />                                                
                </td>                
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="2">
   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      
            <div id="divAllContainer">
        <table class="auto-style1" style="width:100%">
            <tr>
                <td  id="tdMsg" runat="server">&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <table style="width:100%;">
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:MultiView ID="MultiView1" runat="server">
                                    <asp:View ID="first" runat="server">
                                        <table style="width:100%;">
                                            <tr>
                                                <td colspan="3" style="text-align:center;">
                                                    
                                                    <asp:Button ID="btnLoadData" runat="server" CssClass="NFButtonWithNoImage" OnClick="btnLoadData_Click" Text="Load Report" />
                                                    <asp:Button ID="btnBack" runat="server" CssClass="NFButtonWithNoImage" OnClick="btnBack_Click" Text="Cancel" />
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
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:GridView ID="GridViewAccSheet" CssClass="gridAlgn" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridViewAccSheet_RowDataBound" ShowHeader="False" BorderColor="#D7CECE" BorderStyle="Solid" BorderWidth="3px" GridLines="Horizontal">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <table style="width:100%;">
                                                                        <tr>
                                                                            <td>Goal Area :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblGoalArea" runat="server" Text='<%#Eval("LessonPlanName") %>'></asp:Label>
                                                                            </td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Goal :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblGoal" runat="server" Text='<%#Eval("GoalName") %>'></asp:Label>
                                                                            </td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Benchmarks/Objectives :</td>
                                                                            <td colspan="3">
                                                                                <asp:TextBox ID="txtbenchaMark" runat="server" Enabled="false" Height="50px" Text='<%#Eval("Objective3") %>' TextMode="MultiLine" Width="640"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4">
                                                                                <table id="tabValReview" cellpadding="0" cellspacing="0" style="width:100%;">
                                                                                    <tr class="HeaderStyle">
                                                                                        <td>Review Period :</td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod1" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod2" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod3" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod4" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod5" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod6" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod7" runat="server"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>Type of Instruction : </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns1" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns2" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns3" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns4" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns5" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns6" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns7" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="background-color:#F2F0F0">
                                                                                        <td>Stimulus Set :</td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet1" runat="server" Text='<%#Eval("set1") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet2" runat="server" Text='<%#Eval("set2") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet3" runat="server" Text='<%#Eval("set3") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet4" runat="server" Text='<%#Eval("set4") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet5" runat="server" Text='<%#Eval("set5") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet6" runat="server" Text='<%#Eval("set6") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet7" runat="server" Text='<%#Eval("set7") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>Prompt level :</td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl1" runat="server" Text='<%#Eval("ProptLevel1") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl2" runat="server" Text='<%#Eval("ProptLevel2") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl3" runat="server" Text='<%#Eval("ProptLevel3") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl4" runat="server" Text='<%#Eval("ProptLevel4") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl5" runat="server" Text='<%#Eval("ProptLevel5") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl6" runat="server" Text='<%#Eval("ProptLevel6") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl7" runat="server" Text='<%#Eval("ProptLevel7") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="background-color:#F2F0F0">
                                                                                        <td>IOA (date/init/%) :</td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA1" runat="server" Text='<%#Eval("IOAPer1") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA2" runat="server" Text='<%#Eval("IOAPer2") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA3" runat="server" Text='<%#Eval("IOAPer3") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA4" runat="server" Text='<%#Eval("IOAPer4") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA5" runat="server" Text='<%#Eval("IOAPer5") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA6" runat="server" Text='<%#Eval("IOAPer6") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA7" runat="server" Text='<%#Eval("IOAPer7") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td># times run out of possible :</td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos1" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos2" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos3" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos4" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos5" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos6" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos7" runat="server"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="background-color:#F2F0F0">
                                                                                        <td>Program Changes : </td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>&nbsp;</td>
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
                                                                            <td>&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                            <td>Proposals and Discussion :</td>
                                                                            <td>Persons Responsible and Deadlines :</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFreetxt" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPersDissc" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtResAndDeadline" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="auto-style2"></td>
                                                                            <td class="auto-style2"></td>
                                                                            <td class="auto-style2"></td>
                                                                            <td class="auto-style2"></td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="text-align:center;">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="NFButton" OnClick="Save_Click" Text="Save" Visible="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="View2" runat="server">
                                        <table style="width:100%;">
                                           
                                            <tr>
                                                <td colspan="3" style="text-align:center;">
                                                    <asp:DropDownList ID="ddlDate" runat="server" CssClass="drpClass">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlStudentEdit" runat="server" Visible="False" CssClass="drpClass">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnLoadDataEdit" runat="server" CssClass="NFButton" OnClick="btnLoadDataEdit_Click" Text="Load Prior Report" Width="175px"/>
                                                    <asp:Button ID="btnGenNewSheet0" runat="server" CssClass="NFButtonWithNoImage" OnClick="btnPreAccSheet1_Click" Text="Generate New Sheets" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:GridView ID="GridViewAccSheetedit" CssClass="gridAlgn" runat="server" AutoGenerateColumns="False" ShowHeader="False" BorderColor="#D7CECE" BorderStyle="Solid" BorderWidth="3px" GridLines="Horizontal">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    
                                                                    <table style="width:100%;">
                                                                        <tr>
                                                                            <td>Goal Area :</td>
                                                                            <td>
                                                                                <asp:HiddenField ID="hdFldAcdId" runat="server" Value='<%#Eval("AccSheetId") %>'/>
                                                                                <asp:Label ID="lblGoalAreaedit" runat="server" Text='<%#Eval("GoalArea") %>'></asp:Label>
                                                                            </td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Goal :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblGoaledit" runat="server" Text='<%#Eval("Goal") %>'></asp:Label>
                                                                            </td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Benchmarks/Objectives :</td>
                                                                            <td colspan="3">
                                                                                <asp:TextBox ID="txtbenchaMarkedit" runat="server" Enabled="false" Height="50px" Text='<%#Eval("Benchmarks") %>' TextMode="MultiLine" Width="640"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4">
                                                                                <table id="tabValReviewedit" cellpadding="0" cellspacing="0" style="width:100%;">
                                                                                    <tr class="HeaderStyle">
                                                                                        <td>Review Period :</td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod8" runat="server" Text='<%#Eval("Period1") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod9" runat="server" Text='<%#Eval("Period2") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod10" runat="server" Text='<%#Eval("Period3") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod11" runat="server" Text='<%#Eval("Period4") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod12" runat="server" Text='<%#Eval("Period5") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod13" runat="server" Text='<%#Eval("Period6") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPeriod14" runat="server" Text='<%#Eval("Period7") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>Type of Instruction : </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns8" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns9" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns10" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns11" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns12" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns13" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTypOfIns14" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="background-color:#F2F0F0">
                                                                                        <td>Stimulus Set :</td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet8" runat="server" Text='<%#Eval("Set1") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet9" runat="server" Text='<%#Eval("Set2") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet10" runat="server" Text='<%#Eval("Set3") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet11" runat="server" Text='<%#Eval("Set4") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet12" runat="server" Text='<%#Eval("Set5") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet13" runat="server" Text='<%#Eval("Set6") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblStmlsSet14" runat="server" Text='<%#Eval("Set7") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>Prompt level :</td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl8" runat="server" Text='<%#Eval("Prompt1") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl9" runat="server" Text='<%#Eval("Prompt2") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl10" runat="server" Text='<%#Eval("Prompt3") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl11" runat="server" Text='<%#Eval("Prompt4") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl12" runat="server" Text='<%#Eval("Prompt5") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl13" runat="server" Text='<%#Eval("Prompt6") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblprmtLvl14" runat="server" Text='<%#Eval("Prompt7") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="background-color:#F2F0F0">
                                                                                        <td>IOA (date/init/%) :</td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA8" runat="server" Text='<%#Eval("IOA1") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA9" runat="server" Text='<%#Eval("IOA2") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA10" runat="server" Text='<%#Eval("IOA3") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA11" runat="server" Text='<%#Eval("IOA4") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA12" runat="server" Text='<%#Eval("IOA5") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA13" runat="server" Text='<%#Eval("IOA6") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblIOA14" runat="server" Text='<%#Eval("IOA7") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td># times run out of possible :</td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos8" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos9" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos10" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos11" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos12" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos13" runat="server"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNoOfPos14" runat="server"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="background-color:#F2F0F0">
                                                                                        <td>Program Changes : </td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>&nbsp;</td>
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
                                                                            <td>&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                            <td>Proposals and Discussion :</td>
                                                                            <td>Persons Responsible and Deadlines :</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFreetxtedit" runat="server" TextMode="MultiLine" Text='<%#Eval("FeedBack") %>' Width="200px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPersDisscedit" runat="server" TextMode="MultiLine" Text='<%#Eval("PreposalDiss") %>' Width="200px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtResAndDeadlineedit" runat="server" TextMode="MultiLine" Text='<%#Eval("PersonResNdDeadline") %>'  Width="200px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="auto-style2"></td>
                                                                            <td class="auto-style2"></td>
                                                                            <td class="auto-style2"></td>
                                                                            <td class="auto-style2"></td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="text-align:center;">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="text-align:center;">
                                                    <asp:Button ID="btnUpdate" runat="server" CssClass="NFButton" OnClick="btnUpdate_Click" Text="update" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                </asp:MultiView>
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
                <td >
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:center" >
                    &nbsp;</td>
            </tr>
            <tr>
                <td >
                    &nbsp;</td>
            </tr>
        </table>
                </div>
  
          
                </td>                
            </tr>
        </table>
     

                           <div id="overlay" class="web_dialog_overlay"></div>
                <div id="PopDownload" class="web_dialog" style="width: 600px; top: -20%;">

            <div id="Div53" style="width: 700px;">


                <table style="width: 97%">
                    <tr>
                        <td colspan="2">
                        <table style="width:85%">
                            <tr>
                                    <td runat="server" id="tdMsgExport"   style="height: 50px"></td>
                            </tr>
                        </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">

                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" />

                        </td>
                        <td style="text-align: left">

                            <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" OnClick="btnDone_Click" />

                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </div>

    </form>
</body>
</html>
