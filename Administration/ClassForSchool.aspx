<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="ClassForSchool.aspx.cs" Inherits="Admin_ClassForSchool" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType virtualpath="~/Administration/AdminMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="CSS/tabmenu.css" rel="stylesheet" type="text/css" />
    <script src="JS/tabber.js" type="text/javascript"></script>
    <link rel="stylesheet" href="CSS/tabmenu.css" type="text/css" media="screen" />
    <link href="CSS/style2.css" rel="stylesheet" />


    <style type="text/css">
        .chkStyle {
            width: 100%;
            height: 200px;
            padding: 10px 10px 10px 10px;
            text-align: center;
            white-space: nowrap;
            overflow: hidden;
            width: -moz-available !important;
            float: left;
        }

            .chkStyle tr {
                background-color: Transparent !important;
            }

            .chkStyle input {
                width: auto !important;
                float: left !important;
                margin-right: 5px;
                padding-right: 10px;
            }

            .chkStyle label {
                width: auto !important;
                float: left !important;
            }

        .tdtextlbl {
            font-family: Arial;
            color: red;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
        }


        .auto-style1 {
            height: 21px;
        }
    </style>
    <%--</div>--%>
    <script type="text/javascript">
        $(document).ready(function () {

            //$('#btnSubmit').click(function () {
            //  alert('dfdfdf');
            //    $('#dialog').animate({ top: "-300%" }, function () {
            //        $('#overlay').fadeOut('slow');
            //    });

            //});

            $('#close_x img').click(function () {
                alert('close');
                $('#dialog').animate({ top: "-300%" }, function () {
                    $('#overlay').fadeOut('slow');
                });
            });

            $('#close_x1').click(function () {
                $('#dialog1').animate({ top: "-300%" }, function () {
                    $('#overlay1').fadeOut('slow');
                });
            });



            $('#lbtnClassforstudent').click(function () {

                if (validate()) {
                    $('#overlay').fadeIn('slow', function () {
                        $('#dialog').animate({ top: '20%' }, { duration: 'slow', easing: 'linear' });
                    });
                }
            });
            $('#lbtnclassforusers').click(function () {

                if (validate()) {
                    $('#overlay1').fadeIn('slow', function () {
                        $('#dialog1').animate({ top: '20%' }, { duration: 'slow', easing: 'linear' });
                    });
                }
            });


        });
    </script>
    <script language="javascript" type="text/javascript">
        function validate() {
            var btnValue=$('#<%=btnSave.ClientID %>').val();
            if (btnValue == "Delete") {
                flag = confirm("Are you sure to Delete?");
                return flag;
            }
            else {
                if (document.getElementById("<%=txtClassCode.ClientID%>").value == "") {
                    document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Enter Class Code</dv> ";
                    document.getElementById("<%=txtClassCode.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                    document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Enter Class Name</dv> ";
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtDescription.ClientID%>").value == "") {
                    document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Enter Class Description</dv> ";
                    document.getElementById("<%=txtDescription.ClientID%>").focus();
                    return false;
                }

                return true;
            }
        }

        function OnTextChanged() {
            alert('adsa');
        }

        function fadeOutBtn() {
            $('#dialog').animate({ top: "-300%" }, function () {
                $('#overlay').fadeOut('slow');
            });

        }

        function fadeOutBtnUser() {
            $('#dialog1').animate({ top: "-300%" }, function () {
                $('#overlay1').fadeOut('slow');
            });

        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <%--</div>--%>


    <table width="99%">
        <tr>
            <td id="tdMsg" runat="server" align="right">&nbsp;</td>

            <td align="right">        
                
        </tr>





        <tr>
            <td class="tdText">&nbsp;</td>

            <td align="right">&nbsp;</td>
        </tr>





        <tr>
            <td class="tdText" colspan="2">
                <table width="100%" style="border: solid; border-color: gray; border-width: 2px">
                    <tr>
                        <td class="tdText">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="tdText">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>


                    <tr>
                        <td class="tdText">&nbsp;&nbsp;Class Code </td>
                        <td style="text-align:right;color:red;">*</td>
                        <td>
                            <asp:TextBox ID="txtClassCode" runat="server" CssClass="textClass" MaxLength="50" Width="220px"></asp:TextBox>
                        </td>
                        <td class="tdText">&nbsp;&nbsp;Class Description </td>
                        <td style="text-align:right; width:1%;color:red;">*</td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="textClass" MaxLength="99" Width="220px"></asp:TextBox>
                        </td>
                    </tr>


                    <tr>
                        <td class="tdText">&nbsp;&nbsp;Class Name &nbsp; </td>
                        <td style="text-align:right; width:1%;color:red;">*</td>
                        <td>
                            <asp:TextBox ID="txtCode" runat="server" CssClass="textClass" MaxLength="50" Width="220px"></asp:TextBox>
                        </td>
                        <td class="tdText">&nbsp;&nbsp;Residence/Day</td>
                        <td style="text-align:right;color:red;">*</td>
                        <td>
                            <asp:RadioButtonList ID="rdoRadio" runat="server" RepeatDirection="Horizontal" Width="236px">
                                <asp:ListItem Value="0">Day</asp:ListItem>
                                <asp:ListItem Value="1">Residence</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        </tr>
                    <tr>
                       
                             <td class="tdText">&nbsp;&nbsp;Maximum Students</td>
                      <td></td>
                       <%-- <td style="text-align:right;color:red;">*</td>--%>
                        <td>
                            <asp:TextBox ID="txtMaxStudents" runat="server" onKeyPress="return isNumber(event);"  CssClass="textClass" MaxLength="3" Width="30px"></asp:TextBox>
                        </td>
                    </tr>


                    <tr>
                        <td class="tdText">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="tdText">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>


                    <tr>
                        <td class="tdText" colspan="6" style="text-align: center">
                            <asp:Button ID="btnSave" runat="server" CssClass="NFButton" OnClick="btnSave_Click" Text="Save" OnClientClick=" return validate()" />
                        </td>
                    </tr>


                    <tr>
                        <td class="tdText">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="tdText">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>












        <tr>
            <td style="text-align: center" colspan="2">


                <table style="width: 25%; float: right; text-align: right; margin-left: 15%;">
                    <tr>
                        <td class="tdText">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <input id="lbtnClassforstudent" style="width: 120px" type="hidden" value="Assign Students" class="NFButtonWithNoImage" /></td>
                        <td>
                            <input id="lbtnclassforusers" style="width: 120px" type="button" value="Assign Users" class="NFButton" /></td>
                        <td>
                            <asp:Button ID="btnAdd" runat="server" Width="120px" CssClass="NFButton" OnClick="btnAdd_Click" Text="Add New"  Visible="False" /></td>
                    </tr>
                </table>
            </td>
        </tr>


        <tr>
            <td colspan="2">
                <table style="float: right">
                    <tr align="right">
                        <td class="auto-style1">
                            <asp:LinkButton ID="lnk_active" runat="server" OnClick="lnk_active_Click">Active</asp:LinkButton>
                        </td>
                        <td class="auto-style1">|</td>
                        <td class="auto-style1">
                            <asp:LinkButton ID="lnk_inactive" runat="server" OnClick="lnk_inactive_Click">Inactive</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>


        <tr>
            <td colspan="2">
                <asp:GridView ID="grdGroup" runat="server" AutoGenerateColumns="False" CellPadding="4" Width="100%"
                    AllowPaging="True" OnPageIndexChanging="grdGroup_PageIndexChanging" OnRowCommand="grdGroup_RowCommand"
                    OnRowDataBound="grdGroup_RowDataBound" OnRowDeleting="grdGroup_RowDeleting" OnRowEditing="grdGroup_RowEditing"
                    GridLines="none" BackColor="White" BorderColor="White" BorderStyle="None">
                    <Columns>
                        <asp:BoundField DataField="ClassCd" HeaderText="Class Code" />
                        <asp:BoundField DataField="ClassName" HeaderText="Class Name" />
                        <asp:BoundField DataField="ClassDesc" HeaderText="Class Description" />
                        <asp:BoundField DataField="ModifiedUser" HeaderText="Modified By" />
                        <asp:BoundField DataField="ModifiedOn" HeaderText="Modified On" />
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="lb_Edit" runat="server" AlternateText="Edit" CommandArgument='<%# Eval("ClassId") %>' CommandName="Edit" Height="20px" ImageUrl="~/Administration/images/user_edit.png" class="btn btn-blue" Width="20px" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="lb_delete" runat="server" CommandArgument='<%# Eval("ClassId") %>' CommandName="Delete" ImageUrl="~/Administration/images/trash.png" class="btn btn-red"  />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Left" VerticalAlign="Middle" />

                    <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                    <RowStyle CssClass="RowStyle" />
                    <AlternatingRowStyle CssClass="AltRowStyle" />
                    <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="PagerStyle" ForeColor="White" Font-Bold="true" HorizontalAlign="Center" />
                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                </asp:GridView>

            </td>
        </tr>


        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>











    </table>


    <%--<div style="width:400px; position:relative; ">
                                
                                    </div>--%>
    <div id="overlay1" class="web_dialog_overlay">
    </div>
    <div id="dialog1" class="web_dialog" style="width: 656px;">
        <div id="sign_up5" style="width: 656px;">
           
            <h3>Assign Users For the Class:
                <asp:Label ID="lblclass" runat="server" Text=""></asp:Label></h3>
            <hr />
          
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td style="text-align: right" class="tdText">User Name</td>
                            <td>
                                <asp:TextBox ID="txtSearchUser" runat="server" CssClass="textClass" MaxLength="50" Width="200px" AutoCompleteType="None" AutoPostBack="true" OnTextChanged="txtSearchUser_TextChanged"></asp:TextBox>

                                <asp:AutoCompleteExtender ID="txtSearchUser_AutoCompleteExtender" runat="server" ServiceMethod="GetCompletionList2" TargetControlID="txtSearchUser" UseContextKey="True" CompletionInterval="500" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>

                            </td>
                            <td>
                                <asp:Button ID="btnSearchUser" runat="server" Text="Search" OnClick="btnSearchUser_Click" CssClass="NFButton" /></td>
                        </tr>
                    </table>

                    <table width="100%" cellpadding="0" cellspacing="0" style="text-align: left;">
                        <tr>
                            <td width="100%" align="center">
                                <%--</div>--%>
                                <%--</div>--%><%-- <div style="width: 85%; height: 100%; scrollbar-arrow-color: white; position: relative; overflow: auto;">--%>
                                <div style="width: 100%; height: 350px; scrollbar-arrow-color: white; position: relative; overflow: scroll; overflow-x: hidden; font-family: Arial,Helvetica,sans-serif; color: #676767;">
                                    <asp:CheckBoxList ID="chkUesrs" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" Width="100%" CssClass="chkStyle">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;">

                                <table style="width: 100%;">
                                    <tr>
                                       
                                        <td >
                                            <asp:Label ID="Label1" runat="server" CssClass="tdtextlbl" Text="Please Update the class to save the changes"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >
                                            


                                            <asp:Button ID="btnDone" runat="server" Text="Done"  class="NFButton" OnClientClick="fadeOutBtnUser();"/>




                                        </td>
                                        
                                    </tr>
                                </table>


                            </td>
                        </tr>
                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>
    <div id="overlay" class="web_dialog_overlay">
    </div>
    <%--<div style="width:400px; position:relative; ">
                               
                                    </div>--%>
    <div id="dialog" class="web_dialog" style="width: 800px; left: 38%">
        <div id="sign_up5" style="width: 800px">
            <h3>Assign Students For the Class:
                <asp:Label ID="lblclass2" runat="server" Text=""></asp:Label></h3>
            <hr style="background-color: #B6D1DD;" />

            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td style="text-align: right" class="tdText">Student Name</td>
                            <td>
                                <asp:TextBox ID="txtsearch" runat="server" CssClass="textClass" MaxLength="50" Width="200px" AutoCompleteType="None" AutoPostBack="true" onchanged="OnTextChanged();" OnTextChanged="txtsearch_TextChanged"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txtsearch_AutoCompleteExtender" runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtsearch" UseContextKey="True" CompletionInterval="500" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="NFButton" /></td>
                        </tr>
                    </table>


                    <table width="100%" cellpadding="0" cellspacing="0" style="text-align: left;">
                        <tr>
                            <td width="100%" colspan="2" align="center" class="tdText">
                                <%--</div>--%>
                                <%--</div>--%>
                                <div style="width: 100%; height: 200px; scrollbar-arrow-color: white; position: relative; overflow: scroll; overflow-x: hidden; font-family: Arial,Helvetica,sans-serif; color: #676767;">
                                    <asp:CheckBoxList ID="chkStudents" RepeatDirection="Horizontal" RepeatColumns="2" runat="server" Width="80%" CssClass="chkStyle" ForeColor="#676767" CellPadding="1" CellSpacing="1">
                                    </asp:CheckBoxList>
                                </div>
                                <%--<div style="width:400px; position:relative; ">
                                
                                    </div>--%>
                                <%--</div>--%>
                            </td>
                        </tr>
                        <tr>
                           
                            <td>
                                <asp:Label ID="Label2" runat="server" CssClass="tdtextlbl" Text="Please Update the class to save the changes"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 60%; text-align: right;">
                              

                                <asp:Button style="margin-left:20px;margin-top:5px; float:left;" ID="btnSubmit" runat="server" Text="Done" OnClientClick="fadeOutBtn();"  class="NFButton"/>




                            </td>
                           
                        </tr>


                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>
 
    

</asp:Content>
