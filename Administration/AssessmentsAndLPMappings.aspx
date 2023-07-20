<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true" CodeFile="AssessmentsAndLPMappings.aspx.cs" Inherits="Administration_AssessmentsAndLPMappings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
    <title></title>
    <script type="text/javascript" src="JS/tabber.js"></script>
    <link rel="stylesheet" href="CSS/tabmenu.css" type="text/css" media="screen" />
    <script type="text/javascript" src="JS/jquery-ui.min.js"></script>
    <script type="text/javascript">



        function deleteSystem() {
            var flag;
            flag = confirm("Are you sure you want to delete this LessonPlan?");
            return flag;
        }

        function deleteSystem1() {
            var flag;
            flag = confirm("Are you sure you want to delete this Assessment details?");
            return flag;
        }


        $(document).ready(function () {
            $('#close_x').click(function () {
                $('#dialog').animate({ top: "-300%" }, function () {
                    $('#overlay').fadeOut('slow');
                });
            });
        });

        $(document).ready(function () {
            $('#close_x1').click(function () {
                $('#dialog1').animate({ top: "-300%" }, function () {
                    $('#overlay1').fadeOut('slow');
                });
            });
        });
    </script>
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            font-family: Calibri;
            color: Black;
            line-height: 22px;
            font-weight: bold;
            font-size: 13px;
            padding-right: 1px;
            text-align: right;
            width: 50%;
        }
        /*.auto-style1 {
            height: 43px;
        }*/
        .auto-style1 {
            font-family: Arial;
            color: #000;
            line-height: 22px;
            font-weight: normal;
            font-size: 12px;
            padding-right: 1px;
            text-align: left;
            height: 35px;
            width: 100%;
        }

        .Initial {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            /*background: url("../Images/InitialImage.png") no-repeat right top;*/
            background-color: #0054A0;
            color: Black;
            font-weight: bold;
        }

            .Initial:hover {
                color: White;
                /*background: url("../Images/SelectedButton.png") no-repeat right top;*/
                background-color: #0099B5;
            }

        .Clicked {
            float: left;
            display: block;
            /*background: url("../Images/SelectedButton.png") no-repeat right top;*/
            background-color: #1EB53A;
            padding: 4px 18px 4px 18px;
            color: Black;
            font-weight: bold;
            color: White;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function resizeIframe(obj) {
            obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <%-- <table width="100%">
        
        <tr>
            <td>

                <div class="tabber" style="width: 100%;" id="tabformgoal" runat="server">


                    <div class="tabbertab" id="formdiv" runat="server">

                        <h2>Assessment To Lesson Plan Mapping</h2>
                        

                    </div>


                    <div class="tabbertab" id="goaldiv" runat="server">

                        <h2>Lesson Plan to Assessment Mapping
                        </h2>

                       
                    </div>


                </div>

            </td>
        </tr>
       
    </table>--%>






   <%-- /////////////////////////////////////////////////////////--%>
    <table style="width:100%" >
        <tr>
            <td id="tdMsg" runat="server"></td>

        </tr>
            <tr>
                <td>
                    <asp:Button Text="Assessment To Lesson Plan Mapping" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
                        OnClick="Tab1_Click" ForeColor="White" />
                    &nbsp;&nbsp; 
          <asp:Button Text="Lesson Plan to Assessment Mapping" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
              OnClick="Tab2_Click" ForeColor="White" />

                    <asp:MultiView ID="MainView" runat="server">
                        <asp:View ID="View1" runat="server">
                            <table style="width: 100%;  border-width: 1px; border-color: #666; border-style: solid">
                                <tr>
                                    <td>

                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>

                                <table>

                                    <tr>
                                        <td width="30%" align="left" class="tdText">Assessment&nbsp;&nbsp;&nbsp;&nbsp;
                                           <asp:DropDownList ID="ddlAssessment" runat="server" CssClass="drpClass">
                                            </asp:DropDownList>
                                        </td>
                                      <td style="width:10%"></td>
                                        <td style="width:30%" align="right" class="tdText">Lesson Plan Name&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtassessment" runat="server" CssClass="textClass"></asp:TextBox>
                                        </td>
                                        <td style="width:5%"></td>
                                        <td width="10%" align="center" class="tdText">
                                            <asp:Button ID="Button_Search" runat="server" OnClick="Button_Search_Click" Text="Search"
                                                CssClass="NFButton" />
                                        </td>
                                        
                                        <td width="25%" align="left"></td>
                                    </tr>
                                </table>


                                <table width="100%" cellpadding="0" cellspacing="0" style="height: auto">


                                    <tr>
                                        <td align="right"></td>
                                        <td align="right"></td>
                                        <td align="right"></td>

                                        <td width="30%" align="right" colspan="3">
                                            <table width="50%" cellpadding="0" cellspacing="0">
                                                <tr align="right">
                                                    <td align="center" style="width: 0%" valign="bottom" width="50%">
                                                        <asp:LinkButton ID="lbtnAll" runat="server" OnClick="lbtnAll_Click">All </asp:LinkButton>
                                                    </td>
                                                    <td></td>
                                                    <td align="center" style="width: 25%; color: #000000; font-weight: bold;"
                                                        valign="bottom" width="10%">|
                                                    </td>
                                                    <td></td>
                                                    <td align="center" valign="bottom" width="20%">
                                                        <asp:LinkButton ID="lbtnmapped" runat="server" OnClick="lbtnmapped_Click"> Mapped </asp:LinkButton>
                                                    </td>
                                                    <td></td>
                                                    <td align="center" style="width: 25%; color: #000000; font-weight: bold;"
                                                        valign="bottom" width="10%">|
                                                    </td>
                                                    <td></td>
                                                    <td align="center" valign="bottom" width="20%">
                                                        <asp:LinkButton ID="lbtnunmapped" runat="server" OnClick="lbtnunmapped_Click"> UnMapped</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">



                                            <asp:GridView ID="grd_assessmentQues" runat="server" AutoGenerateColumns="False" OnRowDataBound="grd_assessmentQues_RowDataBound" Width="100%"
                                                AllowPaging="True" OnPageIndexChanging="grd_assessmentQues_PageIndexChanging"
                                                EmptyDataText="No Data Found.." PageSize="5"
                                                OnRowCommand="grd_assessmentQues_RowCommand"
                                                GridLines="none">

                                                <Columns>
                                                    <asp:BoundField DataField="GoalName" HeaderText="Goal Name" />
                                                    <asp:TemplateField HeaderText="Assessment Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_assessname" runat="server" Text='<%# Bind("AsmntName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Category">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_category" runat="server" Text='<%# Bind("AsmntCat") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sub Category">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_subcategory" runat="server" Text='<%# Bind("AsmntSubCat") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Questions">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_question" runat="server" Text='<%# Bind("AsmntQId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Lesson Plans" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:GridView ID="grd_assessment" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="False" Width="100%" Font-Names="Arial" Font-Size="Small"
                                                                HorizontalAlign="Justify" OnRowCommand="grd_assessment_RowCommand"
                                                                OnRowDeleting="grd_assessment_RowDeleting" GridLines="None">



                                                                <Columns>
                                                                    <asp:BoundField DataField="LessonPlanName">
                                                                        <ItemStyle Width="500px" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="lb_delete" OnClientClick="javascript:return deleteSystem();"
                                                                                CommandName="Delete" runat="server"
                                                                                CommandArgument='<%# Eval("AsmntLPRelId") %>'
                                                                                ImageUrl="~/Administration/images/trash.png" class="btn btn-red"></asp:ImageButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle Height="0px" />
                                                                <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                                                                <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                                                                <RowStyle CssClass="RowStyle" />
                                                                <AlternatingRowStyle CssClass="AltRowStyle" />
                                                                <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                                                <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                                <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                            </asp:GridView>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Add" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="clickMe" runat="server" Style="cursor: pointer;" CommandName="ADD"
                                                                runat="server" ImageUrl="~/Administration/images/add-icon.png" CssClass="btn btn-green" />
                                                            <asp:HiddenField ID="hdnsortorder" runat="server" Value='<%# Eval("SortOrder") %>' />
                                                        </ItemTemplate>

                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />

                                                    </asp:TemplateField>

                                                </Columns>
                                                <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                                                <RowStyle CssClass="RowStyle" />
                                                <AlternatingRowStyle CssClass="AltRowStyle" />
                                                <FooterStyle CssClass="FooterStyle" />
                                                <SelectedRowStyle CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                <SortedAscendingHeaderStyle BackColor="#487575" />
                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                <SortedDescendingHeaderStyle BackColor="#275353" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>

                            </ContentTemplate>
                        </asp:UpdatePanel>


                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                <tr>
                                    <td>

                                        
                                         <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>




                                <table>

                                    <tr>
                                        <td width="30%" align="left" class="tdText">Assessment&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlBindAssessment" runat="server" class="drpClass">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:10%"></td>
                                        <td style="width:30%" align="right" class="tdText">Lesson Plan Name&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtSearchLessonPlan" runat="server" CssClass="textClass" ToolTip="Lesson Plan Name"></asp:TextBox>
                                        </td>
                                        <td width="5%" align="center" class="tdText">
                                            &nbsp;</td>
                                        <td  align="left">
                                            <asp:Button ID="btn_search_AssessLP" runat="server" CssClass="NFButton" OnClick="btn_search_AssessLP_Click" Text="Search" />
                                        </td>
                                    </tr>

                                </table>
                                <table width="100%" cellpadding="0" cellspacing="0" style="height: auto">

                                    <tr>
                                        <td align="right"></td>
                                        <td align="right"></td>
                                        <td align="right"></td>

                                        <td width="30%" align="right" colspan="3">
                                            <table width="50%" cellpadding="0" cellspacing="0">
                                                <tr align="right">
                                                    <td align="center" style="width: 0%" valign="bottom" width="50%">
                                                        <asp:LinkButton ID="lbtnLPAll" runat="server" OnClick="lbtnLPAll_Click">All </asp:LinkButton>
                                                    </td>
                                                    <td></td>
                                                    <td align="center" style="width: 25%; color: #000000; font-weight: bold;"
                                                        valign="bottom" width="10%">|
                                                    </td>
                                                    <td></td>
                                                    <td align="center" valign="bottom" width="20%">
                                                        <asp:LinkButton ID="lbtnLPmapped" runat="server" OnClick="lbtnLPmapped_Click"> Mapped </asp:LinkButton>
                                                    </td>
                                                    <td></td>
                                                    <td align="center" style="width: 25%; color: #000000; font-weight: bold;"
                                                        valign="bottom" width="10%">|
                                                    </td>
                                                    <td></td>
                                                    <td align="center" valign="bottom" width="20%">
                                                        <asp:LinkButton ID="lbtnLPunmapped" runat="server" OnClick="lbtnLPunmapped_Click"> UnMapped</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="6">
                                            <asp:UpdatePanel ID="gridcontents" runat="server">
                                                <ContentTemplate>



                                                    <asp:GridView ID="grd_LPassessmentQues" runat="server" AutoGenerateColumns="False"
                                                        OnRowDataBound="grd_LPassessmentQues_RowDataBound" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="grd_LPassessmentQues_PageIndexChanging"
                                                        EmptyDataText="No Data Found.." PageSize="5"
                                                        OnRowCommand="grd_LPassessmentQues_RowCommand"
                                                        GridLines="None">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Lesson Plans">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_lessonplan" runat="server" Text='<%# Bind("LessonPlanName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="&lt;table style='width:100%' &gt;&lt;tr&gt;&lt;td style='width:20%;color:white'&gt;Goal Name&lt;/td&gt;&lt;td style='width:15%;color:white'&gt;  Assessment  &lt;/td&gt;&lt;td style='width:25%;color:white'&gt;  Category  &lt;/td&gt;&lt;td style='width:25%;color:white'&gt;   Sub Category &lt;/td&gt;&lt;td style='width:10%;color:white'&gt;    Questions&lt;/td&gt;&lt;td style='width:5%;color:white'&gt;Delete&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:GridView ID="grd_LPassessment" runat="server" AutoGenerateColumns="False"
                                                                        ShowHeader="False" Width="100%" Font-Names="Arial" Font-Size="Small"
                                                                        HorizontalAlign="Justify"
                                                                        CellPadding="3" OnRowCommand="grd_LPassessment_RowCommand"
                                                                        OnRowDeleting="grd_LPassessment_RowDeleting" GridLines="None"
                                                                        BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px">


                                                                        <Columns>
                                                                            <asp:BoundField DataField="GoalName" HeaderText="Goal Name">
                                                                                <ItemStyle Width="20%" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="AsmntName" HeaderText="Assessment">
                                                                                <ItemStyle Width="15%" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="AsmntCat" HeaderText="Category">
                                                                                <ItemStyle Width="25%" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="AsmntSubCat" HeaderText="Sub Category">
                                                                                <ItemStyle Width="25%" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="AsmntQId" HeaderText="Question">
                                                                                <ItemStyle Width="10%" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="lb_delete" OnClientClick="javascript:return deleteSystem1();"
                                                                                        CommandName="Delete" runat="server"
                                                                                        CommandArgument='<%# Eval("AsmntLPRelId") %>'
                                                                                        ImageUrl="~/Administration/images/trash.png" class="btn btn-red"></asp:ImageButton>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle Height="0px" />
                                                                        <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                                                                        <RowStyle CssClass="RowStyle" />
                                                                        <AlternatingRowStyle CssClass="AltRowStyle" />
                                                                        <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                                                        <SelectedRowStyle CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                                                        <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                                                                        <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                        <SortedAscendingHeaderStyle BackColor="#487575" />
                                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                        <SortedDescendingHeaderStyle BackColor="#275353" />
                                                                    </asp:GridView>
                                                                </ItemTemplate>

                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Add" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="clickMe" runat="server" Style="cursor: pointer;" CommandName="ADD"
                                                                        runat="server" ImageUrl="~/Administration/images/add-icon.png" CssClass="btn btn-green" CommandArgument='<%# Eval("LessonPlanId") %>' />
                                                                </ItemTemplate>

                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />

                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                                                        <RowStyle CssClass="RowStyle" />
                                                        <AlternatingRowStyle CssClass="AltRowStyle" />
                                                        <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                                        <SelectedRowStyle CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                                                        <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                        <SortedAscendingHeaderStyle BackColor="#487575" />
                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                        <SortedDescendingHeaderStyle BackColor="#275353" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                                    </td>
                                </tr>
                                
                            </table>
                        </asp:View>

                    </asp:MultiView>
                </td>
            </tr>
         <tr>
            <td>
                <div id="overlay" class="web_dialog_overlay">
                </div>
                <div id="dialog" class="web_dialog" style="width: 675px;">
                    <div id="sign_up5" style="width: 675px;">
                        <h3>Add Mapping</h3>
                        <hr />
                        <a id="close_x" class="close sprited" href="#" style="margin-top: -13px; margin-right: -14px;">
                            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>

                                <table width="100%" cellpadding="0" cellspacing="0" style="text-align: left;">
                                    <tr>
                                        <td class="auto-style1" colspan="4">
                                            <asp:Label ID="lbl_msg" runat="server" Text="" ForeColor="Orange"></asp:Label></td>

                                    </tr>
                                    <tr>
                                        <td style="width: 10%;"></td>
                                        <td class="tdText" style="width: 25%; text-align: left">Assessment Name
                                        </td>
                                        <td style="width: 5%; text-align: center"><strong>:</strong></td>
                                        <td class="tdText" style="width: 50%; text-align: left">
                                            <asp:Label ID="lbl_assess" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%;"></td>
                                        <td class="tdText" style="width: 25%; text-align: left">Category</td>

                                        <td style="width: 5%; text-align: center"><strong>:</strong></td>
                                        <td class="tdText" style="width: 50%; text-align: left">
                                            <asp:Label ID="lbl_category" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="width: 10%;"></td>
                                        <td class="tdText" style="width: 25%; text-align: left">Sub Category</td>
                                        <td style="width: 5%; text-align: center"><strong>:</strong></td>
                                        <td class="tdText" style="width: 50%; text-align: left">
                                            <asp:Label ID="lbl_subcategory" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%;"></td>
                                        <td class="tdText" style="width: 25%; text-align: left">Question</td>
                                        <td style="width: 5%; text-align: center"><strong>:</strong></td>
                                        <td class="tdText" style="width: 50%; text-align: left">
                                            <asp:Label ID="lbl_question" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%;"></td>
                                        <td class="tdText" style="width: 25%; text-align: left">Lesson Plan Name</td>
                                        <td style="width: 5%; text-align: center"><strong>:</strong></td>
                                        <td style="width: 50%; text-align: left">
                                            <table style="width: 50%">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtLP" runat="server" ToolTip="Lesson Plan Name"
                                                            Width="200px" CssClass="textClass" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>

                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="img_LPSearch" runat="server" CssClass="btn btn-orange" OnClick="img_LPSearch_Click" />

                                                    </td>
                                                </tr>
                                            </table>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%;"></td>
                                        <td class="tdText" style="width: 25%; text-align: left">Lesson Plan</td>
                                        <td style="width: 5%; text-align: center"><strong>:</strong></td>
                                        <td class="tdText" style="width: 50%; text-align: left">
                                            <asp:ListBox ID="lstLP" runat="server" SelectionMode="Multiple" Width="300px" Height="200px" CssClass="textClass"
                                                BorderStyle="Solid" BorderWidth="1px"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdnsort" runat="server" />
                                        </td>
                                    </tr>
                                </table>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <table width="100%">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="text-align: center" colspan="2">
                                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" Width="100px"
                                        CssClass="NFButton" /></td>

                            </tr>
                        </table>



                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div id="overlay1" class="web_dialog_overlay">
                </div>
                <div id="dialog1" class="web_dialog" style="width: 675px;">
                    <div id="Div1" style="width: 675px;">
                        <h3>Add Mapping</h3>
                        <hr />
                        <a id="close_x1" class="close sprited" href="#" style="margin-top: -13px; margin-right: -14px;">
                            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>


                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>

                                <table width="100%" cellpadding="0" cellspacing="0" style="text-align: left;">
                                    <tr>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td class="tdText" width="50%">Lesson Plan Name
                                        </td>
                                        <td class="tdText" width="50%">
                                            <asp:Label ID="lbl_lesson" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdText" width="50%">Goal Name</td>
                                        <td class="tdText" width="50%">
                                            <span style="color:red">*</span><asp:DropDownList ID="ddlgoal" runat="server" CssClass="drpClass" AutoPostBack="true" OnSelectedIndexChanged="ddlgoal_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdText" width="50%">Assessment Name</td>
                                        <td class="tdText" width="50%">
                                            <span style="color:red">*</span><asp:DropDownList ID="ddlassess" runat="server" CssClass="drpClass" AutoPostBack="true" OnSelectedIndexChanged="ddlassess_SelectedIndexChanged">
                                                <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdText" width="50%">Category</td>
                                        <td class="tdText" width="50%">
                                           <span style="color:red">*</span><asp:DropDownList ID="ddlcategory" runat="server" CssClass="drpClass" AutoPostBack="true" OnSelectedIndexChanged="ddlcategory_SelectedIndexChanged">
                                                <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdText" width="50%">Sub Category</td>
                                        <td class="tdText" width="50%">
                                            &nbsp;<asp:DropDownList ID="ddlsubcategory" runat="server" CssClass="drpClass" AutoPostBack="true" OnSelectedIndexChanged="ddlsubcategory_SelectedIndexChanged">
                                                <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdText" width="50%">Question</td>
                                        <td class="tdText" width="50%">
                                           <span style="color:red">*</span><asp:DropDownList ID="ddlquestion" runat="server" CssClass="drpClass">
                                                <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>


                                </table>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <div style="text-align: center">
                                        <asp:Button ID="btnAssessAdd" runat="server" OnClick="btnAssessAdd_Click" Text="Add" Width="100px"
                                            CssClass="NFButton" />
                                    </div>
                                </td>

                            </tr>
                        </table>


                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField runat="server" ID="HFCurrTabIndex" Value="" />
                                 
            </td>
        </tr>
        </table>

</asp:Content>

