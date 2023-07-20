<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP11.aspx.cs" Inherits="StudentBinder_CreateIEP11" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
     <script src="../Administration/JS/jquery-1.8.0.js" type="text/javascript"></script>
    
   

    
    <title></title>
    <style type="text/css">

        .HeaderStyle {
            background: none repeat scroll 0 0 #7EABBE;
            color: #FFFFFF !important;
            font-weight: bold;
            font-family: Arial;
            font-size: 12px;
            height: 30px;
            letter-spacing: 0;
            line-height: 12px;
            padding: 0 8px !important;
            text-align: left;
            text-decoration: none;
            vertical-align: middle;
        }

       

        .gridStyle {
            border: 1px solid #d8dbdb;
            margin: 10px 0 10px 0;
            border-spacing: 0;
            border-collapse: collapse;
        }
        

        
        .auto-style7 {
            width: 30px;
        }
        .auto-style8 {
            width: 25px;
        }
        .auto-style9 {
            width: 84px;
        }

        
        .auto-style13 {
            width: 350px;
        }

        
        .auto-style14 {
            width: 735px;
        }

        
        .auto-style15 {
            width: 475px;
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
<body>
    <form id="form1" runat="server">


        <div id="divIep2" style="width: 95%; border-radius: 3px 3px 3px 3px; padding: 7px;">

            


            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="4" style="text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px;">Attendance Sheet
                    </td>
                </tr>
                <tr>
                    <td id="tdMsg" runat="server" colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-size: 18px;">Special Education Team Meeting
                    </td>
                </tr>
                <tr>
                    <td style="border-bottom:2px double gray;">&nbsp;</td>
                </tr>
               
            </table>
            <br />

            <table>
                    <tr>
                        <!--<td><b>Date: </b>
                            <asp:Label ID="lblDate" runat="server"></asp:Label>
                        </td>-->
                         <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
                        Date:<asp:TextBox ID ="txtdate" runat="server" Style="width:135px" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" ></asp:TextBox>
                        
                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdate"></ajax:CalendarExtender>
                    </tr>
                    <tr>
                        <td><b>Student Name: </b>
                            <asp:Label ID="lblStudentName" runat="server" Width="300px"></asp:Label>
                        </td>
                        <td><b>DOB: </b>
                            <asp:Label ID="lblDOB" runat="server" Width="200px"></asp:Label>
                        </td>
                        <td><b>ID#: </b>
                            <asp:Label ID="lblID" runat="server" Width="200px"></asp:Label>
                        </td>
                    </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>


            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <b>Purpose of Meeting:</b> Check all boxes that apply.
                    </td>
                </tr>
            </table>

            <table>
                <tr>
                    <td style="vertical-align: top;">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkEliDeter" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Eligibility Determination" Width="200px"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkInitEval" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Initial Evaluation"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkReeval" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Reevaluation"/>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top;">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkIEPDev" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="IEP Development" Width="200px"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkInit" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Initial"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkAnnRev" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Annual Review"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkOther" runat="server" 
                        Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Other: "/>
                        <asp:TextBox ID="txtOther" runat="server" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top;">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkPlacement" runat="server" 
                            Checked='<%# Convert.ToBoolean(Eval("TechInd")) %>' Text="Placement" Width="200px"/>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>


            <table width="100%">
                <tr>
                    <td>

                            <asp:GridView ID="gvAttnSheet" runat="server" AutoGenerateColumns="False" CssClass="gridStyle" Width="100%"
                                        GridLines="None" ShowFooter="True"
                                        OnRowDataBound="gvAttnSheet_RowDataBound"
                                        OnRowCommand="gvAttnSheet_RowCommand" Style="z-index: 1">

                <HeaderStyle CssClass="HeaderStyle" />
                                    <Columns>

                                       <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("TableId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Print Names of Team Members
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTMName" runat="server" Text='<%# Eval("TMName") %>' Width="100%"></asp:TextBox>
                                                
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Print Roles of Team Members
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTMRole" runat="server" Text='<%# Eval("TMRole") %>' Width="100%"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Initial if in attendance
                                            </HeaderTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="btnAddRow" runat="server" Text="Add New Row" OnClick="btnAddRow_Click" CssClass="NFButton"/>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInitialIfInAttn" runat="server" Text='<%# Eval("InitialIfInAttn") %>' Width="100%"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HeaderStyle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                                <ItemTemplate>

                                                    <table style="margin-left: 15px;">
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ID="lnk_Delete0" runat="server" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </ItemTemplate>


                                            </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                    </td>
                </tr>
            </table>


            






            

                <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" style="font-weight:bold">
                        Attachment to N3
                    </td>
                </tr>
                </table>
            <p></p>
            <table width="100%" cellpadding="0" cellspacing="0">

                
                
               <tr>
                   <td style="width:450px"> Massachusetts DOE / Attendance Sheet</td>
                   <td class="auto-style14"> N 3A</td>
                   <td></td>
                   <td></td>
                   <td></td>
                   <td></td>
               </tr>
                <tr>
                    <td style="text-align: center" colspan="2">

                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" 
                                        OnClick="btnSave_Click" Text="Save and continue" />

                         <%--<asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" 
                                        OnClick="btnSave_hdn_Click" Text="dummy"  style="display:none;"/>--%>
                    </td>
                </tr>
                
            </table>


        </div>
    </form>
    <p>&nbsp;</p>
</body>
</html>
