<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true" CodeFile="LookUpConf.aspx.cs" Inherits="Administration_LookUpConf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="CSS/jquery-ui.css" rel="stylesheet" type="text/css" media="all" />
    <script src="JS/jquery-1.8.0.js"></script>
    <script src="JS/jquery-ui.js"></script>
    <script type="text/javascript">
        function ValidateAlpha(evt) {
            var keyCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : ((evt.which) ? evt.which : 0));
            if (keyCode > 31 && (keyCode < 65 || keyCode > 90) && (keyCode < 45 || keyCode > 46) && (keyCode < 97 || keyCode > 122) && (keyCode < 48 || keyCode > 57) && keyCode!=32 && keyCode != 95)
                {
                    return false;
                }
            
            return true;

            
        }
    </script>
    <style type="text/css">
        .tabTable {
            width: 100%;
            border: 1px solid #CCCCCC;
            padding: 3px;
        }

        .txtSearch, .txtAdd {
            /*border-width:0px !important;
        border-bottom:1px solid #1E8EFF !important;*/
        }

        .btnAdd, .btnSearch {
            /*border: 1px solid rgb(168, 211, 255) !important;
        margin-left: -22px !important;
        color:#939493 !important;
        padding:1px;*/
        }

            .btnAdd:hover, .btnSearch:hover {
                /*background-color: #A8D3FF;
              cursor:pointer;
              color:#000 !important;*/
            }

        .lnkBtnDelete1 {
            /*border: 1px solid #FC3B3B !important;*/
            margin-left: -22px !important;
            color: #F10303 !important;
            padding: 1px;
            font-family: 'Lucida Console';
            font-size: 12px;
            padding: 3px;
        }

            .lnkBtnDelete1:hover {
                background-color: #FED6D6;
            }

        .grdTable tr td {
            border: 1px solid #C1C1C1;
        }

        .innerTable tr td {
            border-width: 0px !important;
        }

        .pagerStyle table td {
            padding: 3px;
        }

            .pagerStyle table td:hover {
                background-color: #A8D3FF;
            }

        .pnlMessage_red {
            background-color: #FED6D6;
            padding: 3px;
            text-align: center;
            width: 100%;
            font-family: 'Lucida Console';
            color: red;
        }

        .pnlMessage_green {
            background-color: #9AFC91;
            padding: 3px;
            text-align: center;
            width: 100%;
            font-family: 'Lucida Console';
            color: #0A5E02;
        }

        .pagerStyle span {
            background-color: #ccc;
        }

        .bg_black {
            background-color: #000;
        }

        .pnlEditBox {
            width: 350px;
            position: fixed;
            border: 5px solid #8E8E8E;
            background-color: #fff;
            margin-left: 33%;
            position: absolute;
            z-index: 10;
            padding: 5px;
        }

        .cover {
            position: fixed;
            top: 0;
            left: 0;
            background-color: #fff;
            opacity: 0.6;
            z-index: 5;
            width: 100%;
            height: 100%;
        }

        .closeImage {
            float: right;
            margin-top: -21px;
            margin-right: -23px;
        }

        .goalName {
            width: 400px;
            overflow: hidden;
            /*border:1px solid red;*/
            float: left;
        }

        .goalCode {
            color: #6B6B6B;
            font-style: italic;
            width: 400px;
            overflow: hidden;
            /*border:1px solid green;*/
            float: left;
        }

        .italicsTxt {
            color: #6B6B6B;
            font-style: italic;
        }
    </style>

    <script src="JS/jquery-1.8.0.js"></script>
    <script src="JS/jquery-ui.js"></script>
    


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">

    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Teaching Procedure</a></li>
            <li><a href="#tabs-2">Goals</a></li>
            <li><a href="#tabs-3">Assessments</a></li>
        </ul>

        <div id="tabs-1">

            <asp:UpdateProgress ID="UpdateProgress1" runat="server"></asp:UpdateProgress>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlOverlay" CssClass="cover" Visible="false"></asp:Panel>
                    <asp:Panel runat="server" ID="pnlTeachingProcEdit" CssClass="pnlEditBox" Visible="false">
                        <table class="display">
                            <tr>
                                <td><span style="font-weight: bold;">Edit Teaching Procedure</span>
                                    <asp:ImageButton runat="server" ID="imbBtnClose" CssClass="closeImage" ImageUrl="~/Administration/images/closebtn.png" OnClick="imbBtnClose_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtEditTeachingProc" runat="server" onfocus="javacript: $(this).select();" MaxLength="50" onkeypress="return ValidateAlpha(event);"></asp:TextBox>
                                    <asp:Button ID="btnEditTeachingProc" runat="server" CssClass="NFButton" Text="Save" OnClick="btnEditTeachingProc_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEditTeachingProcMessage" runat="server" ForeColor="#FF3300"></asp:Label>
                                    <asp:HiddenField ID="hdnTeachProcId" runat="server" />
                                    <asp:HiddenField ID="hdnTeachProcName" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <div class="tabContainer">

                        <table class="tabTable">
                            <tr>
                                <td style="width: 340px;">

                                    <asp:TextBox ID="txtAdd" runat="server" CssClass="txtAdd txtClass" onfocus="javacript: $(this).select();"  MaxLength="50" ToolTip="Teaching Procedure Name" onkeypress="return ValidateAlpha(event);"/>
                                    <asp:Button ID="btnAdd" runat="server" CssClass="btnAdd NFButton" OnClick="btnAdd_Click" Text="Add" ToolTip="Add" />
                                </td>
                                <td>
                                    <asp:Panel ID="pnlMessage" runat="server" CssClass="pnlMessage" onclick="javascript:$(this).remove();" ToolTip="Click to close">
                                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    </asp:Panel>
                                </td>
                                <td style="width: 340px;">
                                    <input type="text" class="txtSearch" value="Search" style="display: none" />
                                    <input type="button" class="btnSearch" value="?" style="display: none;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="padding-top: 10px;">
                                    <asp:HiddenField ID="hdnTeachProcStatus" Value="" runat="server" />
                                    <asp:GridView ID="grdViewTeachingProc" runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="grdTable" DataKeyNames="LookupId" DataSourceID="SqlDataSource1" OnRowCommand="grdViewTeachingProc_RowCommand" PageSize="10" ShowHeader="False" Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <table class="display innerTable">
                                                        <tr>
                                                            <td>
                                                                <div style="float: left; width: 50px;">
                                                                    <span style="font-style: italic; font-size: 11px; color: rgb(173, 109, 0);">#</span><asp:Label ID="Label1" runat="server" Style="font-style: italic; font-size: 11px; color: rgb(173, 109, 0);" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                                </div>
                                                                <div style="float: left;">
                                                                    <asp:HiddenField runat="server" ID="hdnTPId" Value='<%# Eval("LookupId") %>' />
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("LookupCode") %>'></asp:Label>
                                                                </div>
                                                                <div style="float: right;">
                                                                    <asp:ImageButton ID="lnkBtnDelete" runat="server" CommandArgument='<%# Eval("LookupId") %>' CommandName="DeleteTeachProc" CssClass="lnkBtnDelete bg_black"  Visible='<%# (hdnTeachProcStatus.Value.ToString()=="false")? false: Eval("isDynamic") %>' ImageUrl="~/Administration/images/trash.png"></asp:ImageButton>
                                                                    <asp:ImageButton ID="lnlBtnEdit" runat="server" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' CommandName="EditTeachProc" CssClass="lnkBtnDelete bg_black" ImageUrl="~/Administration/images/edit-icon.png" Visible='<%# (hdnTeachProcStatus.Value.ToString()=="false")? false: true %>'></asp:ImageButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <table class="display">
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td style="text-align: right">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" />
                                        <PagerStyle CssClass="pagerStyle" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:dbConnectionString %>" SelectCommand="SELECT [LookupId], [LookupName],[LookupCode], [isDynamic], [SchoolId] FROM [LookUp] WHERE ([LookupType] = @LookupType) AND ([ActiveInd]='A') ORDER BY [LookupId] DESC">
                                        <DeleteParameters>
                                            <asp:Parameter Name="original_LookupId" Type="Int32" />
                                            <asp:Parameter Name="original_LookupName" Type="String" />
                                            <asp:Parameter Name="original_isDynamic" Type="Boolean" />
                                            <asp:Parameter Name="original_SchoolId" Type="Int32" />
                                        </DeleteParameters>
                                        <InsertParameters>
                                            <asp:Parameter Name="LookupName" Type="String" />
                                            <asp:Parameter Name="isDynamic" Type="Boolean" />
                                            <asp:Parameter Name="SchoolId" Type="Int32" />
                                        </InsertParameters>
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="Datasheet-Teaching Procedures" Name="LookupType" Type="String" />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="LookupName" Type="String" />
                                            <asp:Parameter Name="isDynamic" Type="Boolean" />
                                            <asp:Parameter Name="SchoolId" Type="Int32" />
                                            <asp:Parameter Name="original_LookupId" Type="Int32" />
                                            <asp:Parameter Name="original_LookupName" Type="String" />
                                            <asp:Parameter Name="original_isDynamic" Type="Boolean" />
                                            <asp:Parameter Name="original_SchoolId" Type="Int32" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </div>


                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <div id="tabs-2">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlOverlay2" CssClass="cover" Visible="false"></asp:Panel>
                    <asp:Panel runat="server" ID="pnlGoalEditBox" CssClass="pnlEditBox" Visible="false">
                        <table class="display">
                            <tr>
                                <td><span style="font-weight: bold;">Edit Goal</span>
                                    <asp:ImageButton runat="server" ID="imbBtnClose1" CssClass="closeImage" ImageUrl="~/Administration/images/closebtn.png" OnClick="imbBtnClose1_Click"  />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="italicsTxt">Goal Code:</span><asp:Label ID="lblGoalCode_popup" CssClass="italicsTxt" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtEditGoal" runat="server" onfocus="javacript: $(this).select();" MaxLength="50" onkeypress="return ValidateAlpha(event);"></asp:TextBox>
                                    <asp:Button ID="btnEditGoal" runat="server" CssClass="NFButton" Text="Save" OnClick="btnEditGoal_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEditGoal" runat="server" ForeColor="#FF3300"></asp:Label>
                                    <asp:HiddenField ID="hdnEditGoal" runat="server" />
                                    <asp:HiddenField ID="hdnEditGoalName" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <div class="tabContainer">

                        <table class="tabTable">
                            <tr>
                                <td style="width: 340px;">

                                    <asp:TextBox ID="txtAddGoal" runat="server" CssClass="txtAdd txtClass" onfocus="javacript: $(this).select();" MaxLength="50" ToolTip="Gaol Name" onkeypress="return ValidateAlpha(event);" />
                                    <asp:Button ID="btnAddGoal" runat="server" CssClass="btnAdd NFButton" Text="Add" OnClick="btnAddGoal_Click" ToolTip="Add Goal" />
                                </td>
                                <td>
                                    <asp:Panel ID="pnlMessageGoal" runat="server" CssClass="pnlMessage" onclick="javascript:$(this).remove();" ToolTip="Click to close">
                                        <asp:Label ID="lblMessageGoal" runat="server"></asp:Label>
                                    </asp:Panel>
                                </td>
                                <td style="width: 340px;">
                                    <input type="text" class="txtSearch" value="Search" style="display: none;" />
                                    <input type="button" class="btnSearch" value="?" style="display: none;" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3" style="padding-top: 10px;">
                                    <asp:GridView ID="grdViewGoals" runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="grdTable" DataKeyNames="GoalId" DataSourceID="SqlDataSource2" PageSize="10" Width="100%" OnRowCommand="grdViewGoals_RowCommand">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <div style="float: left; width: 50px;">
                                                        <span style="font-style: italic; font-size: 11px; color: rgb(173, 109, 0);">#</span><asp:Label ID="lblGoalNo" runat="server" Style="font-style: italic; font-size: 11px; color: rgb(173, 109, 0);" Text="Sl No"></asp:Label>
                                                    </div>
                                                    <div style="float: left;">
                                                        <asp:Label ID="lblGoalName" runat="server" CssClass="goalName" Text="Goal Name"></asp:Label>
                                                        <asp:Label ID="lblGoalCode" runat="server" CssClass="goalCode" Text="Goal Code"></asp:Label>
                                                    </div>
                                                    <div style="float: right;">
                                                        Options
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="display innerTable">
                                                        <tr>
                                                            <td>
                                                                <div style="float: left; width: 50px;">
                                                                    <span style="font-style: italic; font-size: 11px; color: rgb(173, 109, 0);">#</span><asp:Label ID="lblGoalNo" runat="server" Style="font-style: italic; font-size: 11px; color: rgb(173, 109, 0);" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                                </div>
                                                                <div style="float: left;">
                                                                    <asp:HiddenField runat="server" ID="hdnGoalId" Value='<%# Eval("GoalId") %>' />
                                                                    <asp:Label ID="lblGoalName" CssClass="goalName" runat="server" Text='<%# Bind("GoalCode") %>'></asp:Label>
                                                                    <asp:Label ID="lblGoalCode" CssClass="goalCode" runat="server" Text='<%# Bind("GoalName") %>'></asp:Label>
                                                                </div>
                                                                <div style="float: right;">
                                                                    <asp:ImageButton ID="lnkBtnDeleteGoal" runat="server" CommandArgument='<%# Eval("GoalId") %>' CommandName="DeleteGoal" CssClass="lnkBtnDelete bg_black"  ImageUrl="~/Administration/images/trash.png"  Visible='<%# (hdnTeachProcStatus.Value.ToString()=="false")? false: true %>'></asp:ImageButton>
                                                                    <asp:ImageButton ID="lnlBtnEditGoal" runat="server" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' CommandName="EditGoal" CssClass="lnkBtnDelete bg_black" ImageUrl="~/Administration/images/edit-icon.png"  Visible='<%# (hdnTeachProcStatus.Value.ToString()=="false")? false: true %>'></asp:ImageButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <table class="display">
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td style="text-align: right">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" />
                                        <PagerStyle CssClass="pagerStyle" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:dbConnectionString %>" SelectCommand="SELECT [GoalId], [SchoolId], [GoalName], [GoalCode], [GoalDesc], [GoalPic], [CreatedBy], [CreatedOn], [isDynamic], [ActiveInd] FROM [Goal] WHERE ([ActiveInd]='A') ORDER BY [GoalId] DESC">
                                        <DeleteParameters>
                                            <asp:Parameter Name="original_GoalId" Type="Int32" />
                                            <asp:Parameter Name="original_SchoolId" Type="Int32" />
                                            <asp:Parameter Name="original_GoalName" Type="String" />
                                            <asp:Parameter Name="original_GoalDesc" Type="String" />
                                            <asp:Parameter Name="original_GoalPic" Type="String" />
                                            <asp:Parameter Name="original_CreatedBy" Type="String" />
                                            <asp:Parameter Name="original_CreatedOn" Type="DateTime" />
                                            <asp:Parameter Name="original_isDynamic" Type="Boolean" />
                                            <asp:Parameter Name="original_ActiveInd" Type="String" />
                                        </DeleteParameters>
                                        <InsertParameters>
                                            <asp:Parameter Name="SchoolId" Type="Int32" />
                                            <asp:Parameter Name="GoalName" Type="String" />
                                            <asp:Parameter Name="GoalDesc" Type="String" />
                                            <asp:Parameter Name="GoalPic" Type="String" />
                                            <asp:Parameter Name="CreatedBy" Type="String" />
                                            <asp:Parameter Name="CreatedOn" Type="DateTime" />
                                            <asp:Parameter Name="isDynamic" Type="Boolean" />
                                            <asp:Parameter Name="ActiveInd" Type="String" />
                                        </InsertParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="SchoolId" Type="Int32" />
                                            <asp:Parameter Name="GoalName" Type="String" />
                                            <asp:Parameter Name="GoalDesc" Type="String" />
                                            <asp:Parameter Name="GoalPic" Type="String" />
                                            <asp:Parameter Name="CreatedBy" Type="String" />
                                            <asp:Parameter Name="CreatedOn" Type="DateTime" />
                                            <asp:Parameter Name="isDynamic" Type="Boolean" />
                                            <asp:Parameter Name="ActiveInd" Type="String" />
                                            <asp:Parameter Name="original_GoalId" Type="Int32" />
                                            <asp:Parameter Name="original_SchoolId" Type="Int32" />
                                            <asp:Parameter Name="original_GoalName" Type="String" />
                                            <asp:Parameter Name="original_GoalDesc" Type="String" />
                                            <asp:Parameter Name="original_GoalPic" Type="String" />
                                            <asp:Parameter Name="original_CreatedBy" Type="String" />
                                            <asp:Parameter Name="original_CreatedOn" Type="DateTime" />
                                            <asp:Parameter Name="original_isDynamic" Type="Boolean" />
                                            <asp:Parameter Name="original_ActiveInd" Type="String" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </div>


                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="tabs-3">

            <asp:UpdateProgress ID="UpdateProgress3" runat="server"></asp:UpdateProgress>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlOverlay3" CssClass="cover" Visible="false"></asp:Panel>
                    <asp:Panel runat="server" ID="pnlAssessmentEditBox" CssClass="pnlEditBox" Width="430px" Visible="false">

                        <table class="display">
                            <tr>
                                <td><span style="font-weight: bold;">Edit Assessment</span>
                                    <asp:ImageButton runat="server" ID="imbBtnClose2" CssClass="closeImage" ImageUrl="~/Administration/images/closebtn.png" OnClick="imbBtnClose2_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="italicsTxt" style="vertical-align: top">Assessment Name:</span><asp:Label ID="Label3" CssClass="italicsTxt" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtEditAssessment" runat="server" onfocus="javacript: $(this).select();" MaxLength="50" onkeypress="return ValidateAlpha(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="italicsTxt">Assessment Description:</span><asp:Label ID="Label4" CssClass="italicsTxt" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtEditAssDesc" TextMode="multiline" Columns="30" Rows="3" runat="server" onfocus="javacript: $(this).select();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnEditAssessment" runat="server" CssClass="NFButton" Text="Save" OnClick="btnEditAssessment_Click" />
                                </td>

                            </tr>
                            <tr>

                                <td>
                                    <asp:Label ID="lblEditAssessmentMessage" runat="server" ForeColor="#FF3300"></asp:Label>
                                    <asp:HiddenField ID="hdnAssessmentId" runat="server" />
                                    <asp:HiddenField ID="hdnAssessmentName" runat="server" />
                                    <asp:HiddenField ID="hdnAssessmentDesc" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <div class="tabContainer">

                        <table class="tabTable">
                            <tr>
                                <td style="width: 140px; vertical-align: top">
                                    <asp:Label ID="lblAsmtName" runat="server" Text="Assessment Name"></asp:Label>
                                    <asp:TextBox ID="txtAddAssessment" runat="server" CssClass="txtAdd txtClass" onfocus="javacript: $(this).select();" MaxLength="50" ToolTip="Assessment Name" onkeypress="return ValidateAlpha(event);" />
                                </td>

                                <td style="width: 100px;">
                                    <asp:Label ID="lblAsmtCode" runat="server" Text="Assessment Description"></asp:Label>
                                    <asp:TextBox ID="txtAddAssessDesc" runat="server" CssClass="txtAdd txtClass" onfocus="javacript: $(this).select();" TextMode="multiline" Columns="30" Rows="3" ToolTip="Assessment Desc" />

                                </td>

                                <td style="width: 340px;">
                                    <input type="text" class="txtSearch" value="Search" style="display: none" />
                                    <input type="button" class="btnSearch" value="?" style="display: none;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Button ID="btnAddAssessment" runat="server" CssClass="btnAdd NFButton" OnClick="btnAddAssessment_Click" Text="Add" ToolTip="Add" />
                                    <asp:Panel ID="pnlMessageAssessment" runat="server" CssClass="pnlMessage" onclick="javascript:$(this).remove();" ToolTip="Click to close">
                                        <asp:Label ID="lblMessageAssessment" runat="server"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="padding-top: 10px;">
                                    <asp:GridView ID="grdAssessment" runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="grdTable" DataKeyNames="LookupId" DataSourceID="SqlDataSource3" Width="100%" OnRowCommand="grdAssessment_RowCommand">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <div style="float: left; width: 50px;">
                                                        <span style="font-style: italic; font-size: 11px; color: rgb(173, 109, 0);">#</span><asp:Label ID="lblGoalNo" runat="server" Style="font-style: italic; font-size: 11px; color: rgb(173, 109, 0);" Text="Sl No"></asp:Label>
                                                    </div>
                                                    <div style="float: left;">
                                                        <asp:Label ID="lblAsmtName" runat="server" CssClass="goalName" Text="Assessment Name"></asp:Label>
                                                        <asp:Label ID="lblAsmtCode" runat="server" CssClass="goalCode" Text="Assessment Description"></asp:Label>
                                                    </div>
                                                    <div style="float: right;">
                                                        Options
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="display innerTable">
                                                        <tr>
                                                            <td>
                                                                <div style="float: left; width: 50px;">
                                                                    <span style="font-style: italic; font-size: 11px; color: rgb(173, 109, 0);">#</span><asp:Label ID="lblGoalNo" runat="server" Style="font-style: italic; font-size: 11px; color: rgb(173, 109, 0);" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                                </div>
                                                                <div style="float: left;">
                                                                    <asp:HiddenField runat="server" ID="hdnLookupId" Value='<%# Eval("LookupId") %>' />
                                                                    <asp:Label ID="lblAsmtName" CssClass="goalName" runat="server" Text='<%# Bind("LookupName") %>'></asp:Label>
                                                                    <asp:Label ID="lblAsmtCode" CssClass="goalCode" runat="server" Text='<%# Bind("LookupDesc") %>'></asp:Label>
                                                                </div>
                                                                <div style="float: right;">
                                                                    <asp:ImageButton ID="lnkBtnDeleteAssessment" runat="server" CommandArgument='<%# Eval("LookupId") %>' CommandName="DeleteAssessment" CssClass="lnkBtnDelete bg_black"  ImageUrl="~/Administration/images/trash.png"  Visible='<%# (hdnTeachProcStatus.Value.ToString()=="false")? false: true %>'></asp:ImageButton>
                                                                    <asp:ImageButton ID="lnlBtnEditAssessment" runat="server" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' CommandName="EditAssessment" CssClass="lnkBtnDelete bg_black"  Visible='<%# (hdnTeachProcStatus.Value.ToString()=="false")? false: true %>' ImageUrl="~/Administration/images/edit-icon.png"></asp:ImageButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <table class="display">
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td style="text-align: right">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" />
                                        <PagerStyle CssClass="pagerStyle" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnectionString %>" SelectCommand="SELECT * FROM [LookUp] WHERE ([LookupType] = @LookupType) AND ([ActiveInd]='A') ORDER BY LookupId DESC">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="Assessment Name" Name="LookupType" Type="String" />
                                        </SelectParameters>

                                    </asp:SqlDataSource>
                                    <tr>
                                        <td colspan="3" style="padding-top: 10px;"></td>
                                    </tr>
                                    <caption>
                                        <br />
                                    </caption>
                                </td>
                            </tr>
                        </table>
                    </div>


                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>

    <script>
        $(document).ready(function () {
            $("#tabs").tabs();
        });
    </script>
</asp:Content>

