<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true" CodeFile="LessonPlanTemplate.aspx.cs" Inherits="Administration_AAa" EnableEventValidation="false" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--EnableEventValidation="false"--%>

    <script src="../StudentBinder/jsScripts/jq1.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="../StudentBinder/jsScripts/eye.js"></script>
    <script type="text/javascript" src="../StudentBinder/jsScripts/layout.js"></script>
    <script src="../Administration/JS/jquery-1.8.0.js"></script>

    <link href="../StudentBinder/CustomLessons.css" rel="stylesheet" />
    <script src="JS/ajaxfileupload.js"></script>

    <style type="text/css">
        .divLoadClpToAdmin {
            z-index: 1;
            position: absolute;
            top: 300px;
            left: 1px;            
            display: none;
            width: 100%; 
            height: 900px; 
            overflow-y: auto;
        }
        .btContainerPart {
            display:block;
        }
        .mBxContainer1 {
            width:1px;
            height:1px;
            padding:1px;
            background-color:white;
            border:none;
            display:none;
            visibility:hidden;
        }
        .ddchkLessonStatus {
            width:150px;
            Height:300px;
            height: 21px !important;
            color: #676767 !important;
            border-radius: 3px !important;
            padding: 0 3px 3px 2px!important;
            background-image: url(./images/statusdownarow.png) !important;
            background-size: 14px 17px !important;
            background-position-x: 97%!important;
        }
        .ddchkLessonYear {
            width:150px;
            Height:300px;
            height: 21px !important;
            color: #676767 !important;
            border-radius: 3px !important;
            padding: 0 3px 3px 2px!important;
            background-image: url(./images/statusdownarow.png) !important;
            background-size: 14px 17px !important;
            background-position-x: 97%!important;
        }
        div.dd_chk_select div#caption{
            top: 3px !important;
        }
        div.dd_chk_drop div#checks {
            Height: 150px;
            width:150px;
            background-color: #F2EEEE;
        }
        div.dd_chk_drop {
             Height: 150px;
        }

        .PagerStyle td a {
            width: 8px;
            height: 15px;
        }
        .web_dialog {
            display:none;
        }
        </style>
  
    <script type="text/javascript">
                
        function LoadAdminLPs() {            
            $('#ifrmAdminLps').attr('src', '../StudentBinder/CustomizeTemplateEditor.aspx?admin=true');
            $('#divLoadClpToAdmin').fadeIn();
        }

        function chageStyle() {
            var iframe = document.getElementById('ifrmAdminLps');
            var element = iframe.contentWindow.document.getElementById('MainDiv');
            var ele = document.getElementById('ddlGoal');
        }

        function HideClpFromAdmin() {   
            $("#divLoadClpToAdmin").hide();
        }

        //Databank Preview
        function LoadLessonView(LpId, goalId) {     
            $('#ifrmAdminLps').attr('src', '../StudentBinder/LessonPlanAttributes.aspx?lessonId=' + LpId + '&goalId=' + goalId + '');
            $('#divLoadClpToAdmin').fadeIn();            
        }

        function LessonExport(exportflag, LpId, DSTempHdrId) {
            $('#ifrmAdminLps').attr('src', '../StudentBinder/LessonPlanAttributes.aspx?export='+ exportflag + '&lessonId=' + LpId + '&DSTempHdrId=' + DSTempHdrId + '');
        }

        function LessonExportNew(exportflag, viewtype, LpId, DSTempHdrId) {
            $('#ifrmAdminLps').attr('src', '../StudentBinder/LessonPlanAttributes.aspx?export=' + exportflag + '&typeview=' + viewtype + '&lessonId=' + LpId + '&DSTempHdrId=' + DSTempHdrId + '');
        }

        function deleteConfirm() {
            var flag;
            flag = confirm("Are you sure you want to delete this event?");
            return flag;
        }

        //Open or Edit
        function LoadUpdateLesson(DSTempHdrId) {
            $('#ifrmAdminLps').attr('src', '../StudentBinder/CustomizeTemplateEditor.aspx?admin=true&DSTempHdrId=' + DSTempHdrId + '&DatabankMode=OpenOrEdit');
            $('#divLoadClpToAdmin').fadeIn();
        }

        //Client_View Preview
        function LoadClientLessonView(LpId, goalId, studid) {
            $('#ifrmAdminLps').attr('src', '../StudentBinder/LessonPlanAttributes.aspx?lessonId=' + LpId + '&goalId=' + goalId + '&studid=' + studid + '');
            $('#divLoadClpToAdmin').fadeIn();
        }        

        function LoadCopyToDatabank() {            
            $("#<%= divCopyToDatabank.ClientID %>").css("display", "block");            
            $("#<%= divCopyToDatabank.ClientID %>").fadeIn('slow');
            $("#<%= tdMsgExprt.ClientID %>").empty(); 
            var Lesson = $("#<%= hdnLessonName.ClientID %>").val();
            $("#<%= txtCopyLessonName.ClientID %>").val(Lesson)        
        }   

        function ExecuteLPExist(){
            if (LPExist() == true) {
                var Name = $("#<%= txtCopyLessonName.ClientID %>").val();
                $("#<%= hdnLessonName.ClientID %>").val(Name);
                return true;
            }
            else {
                $("#<%= hdnLessonName.ClientID %>").val("");
                return false;
            }
        }
        
        function LPExist() {
            var Name = $("#<%= txtCopyLessonName.ClientID %>").val();    
            if (Name == "") {
                $("#<%= tdMsgExprt.ClientID %>").html("<div class='warning_box'>Please enter lesson plan name.</div>")
                return false;
            }
            else {
                var dataresult = false;
                $.ajax(
                 {
                     type: "POST",
                     url: "LessonPlanTemplate.aspx/SearchLessonPlanList",
                     data: "{'Name':'" + Name + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,
                     success: function (data) {
                         if (data.d == "0") {
                             dataresult = true;
                         }
                         else {
                             $("#<%= tdMsgExprt.ClientID %>").html("<div class='warning_box'>Lesson plan name already exist. Please enter another name...</div>");
                             dataresult = false;
                         }
                     },
                     error: function (request, status, error) {
                         //alert("Error");
                     }
                 });
                return dataresult;
            }
        }

        function closePOP() {
            $("#<%= divCopyToDatabank.ClientID %>").fadeOut('slow');
            $("#<%= tdMsgExprt.ClientID %>").empty();
        }

        function showDivcls(el) {
            var gbtn = document.getElementById('GridVisibleDiv'); 
            var btnShw = document.getElementById('btnShowOrHideJS');
            var expanded = true;

            if (gbtn.style.height === 0 + "px") {                                
                gbtn.style.removeProperty("height");                
                gbtn.style.height = 630 + "px";
                expanded = false;
            }
            else {
                var height = gbtn.offsetHeight;
                gbtn.style.height = height + "px";
            }

            if (expanded) {
                height = gbtn.offsetHeight;
                gbtn.style.height = 0 + "px";
                expanded = false;
            } else {
                gbtn.style.height = height + "px";
                expanded = true;
            }

            if (el.value === "Show")
                el.value = "Hide";
            else
                el.value = "Show";
        }
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="server">  

    <table style="width: 100%;">
        <tr>
            <td style="width:10%">
              <h3 style="color:black"> Overview: </h3> 
            </td>
            <td style="width:50%">
                <asp:Button ID="btnShowOrHide" runat="server" Text="Show"  BorderStyle="None" Visible="true" style="display:none" CssClass="NFButton"  OnClick="btnShowOrHide_Click" />
                <asp:Button ID="btnShowOrHideJS" runat="server" Text="Show"  BorderStyle="None" Visible="true"  CssClass="NFButton" UseSubmitBehavior="false" OnClientClick="showDivcls(this); return false;" />
            </td>            
            <td style="width:35%"></td>
            <td style="width:5%; float:left;">
                <asp:ImageButton ID="btnRefresh" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" 
                    ToolTip="Refresh"   />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width:60%">
                <div id="GridVisibleDiv" style="transition: height 0.75s ease-in-out;overflow:scroll;height:0px;">
                    <asp:GridView ID="GrdOverview" runat="server" AutoGenerateColumns="False" Width="100%"
                        GridLines="None" CellPadding="4" ForeColor="#333333" EmptyDataText="No Data Found....."
                        Visible="true" OnPreRender="GridView_PreRender">
                        <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                        <RowStyle CssClass="RowStyle" />
                        <FooterStyle CssClass="FooterStyle" Font-Bold="True" ForeColor="White" />
                        <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <AlternatingRowStyle CssClass="AltRowStyle" />
                        <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                        <Columns>
                            <asp:BoundField DataField="GoalName" HeaderText="Goal Name" />
                            <asp:BoundField DataField="ClientLessonCount" HeaderText="Client Lessons" />
                            <asp:BoundField DataField="DatabankLessonCount" HeaderText="Databank Lessons" />
                        </Columns>
                        </asp:GridView>
                    <br>
                   
                     <div  id="OrgStatsHeader" runat="server" class='redbanner' style="width:960px;height:30px;
                            line-height:30px;text-align:left;padding: 0 8px!important;background-color:#03507d; color:white ">Organizational Statistics (for demographic reporting on Clients, please use the Reporting page of Client Database)
                     </div>
                       
                        <asp:GridView ID="GrdOrgStats" runat="server" AutoGenerateColumns="false" Width="100%"
                        GridLines="None" CellPadding="4" ForeColor="#333333" EmptyDataText="No Data Found....."
                        Visible="true" OnPreRender="GridView_PreRender">
                        <%--<HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />--%>
                        <RowStyle CssClass="RowStyle"/>
                        <FooterStyle CssClass="FooterStyle" Font-Bold="True" ForeColor="White" />
                        <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <AlternatingRowStyle CssClass="AltRowStyle"/>
                        <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                        <Columns>
                             <asp:BoundField DataField="Category" HeaderText="" />
                            <asp:BoundField DataField="Count" HeaderText="" />
                        </Columns>
                        </asp:GridView>
                    <br>
                </div>
            </td>
            <td style="width:35%"></td>
            <td style="width:5%"></td>
        </tr>
        </table>

    <br /><br />

    <div style="width: 100%;">
        <h3 style="color:black">View, Modify and Add Lessons to Databank:</h3><br />
        <asp:RadioButtonList ID="RbtnLessonView" runat="server" RepeatDirection="Horizontal" width="300px" AutoPostBack="true"
            OnSelectedIndexChanged="RbtnLessonView_SelectedIndexChanged">                            
            <asp:ListItem Value="DatabankView" Selected="True">Databank View </asp:ListItem>
            <asp:ListItem Value="ClientView" >Client View</asp:ListItem>
        </asp:RadioButtonList>
    </div>

    <br /><br />

    <table style="width: 100%;">
         <tr>
             <td id="tdMsg" colspan="6" runat="server" style="width: 75%"></td>
             <td> <asp:HiddenField ID="hdnLessonName" runat="server" /></td>
         </tr>
        <tr>
            <td colspan="4"><b> Filter: </b></td>
            <td><p id= "iepPtag" runat="server"><%--<b> IEP Start Year: </b>--%></p></td>
            <td></td>
            <td><b> Search Lesson Name: </b></td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlClientName" runat="server" CssClass="drpClass" Height="26px" Width="150px"  OnSelectedIndexChanged="ddlClientName_SelectedIndexChanged"
                     AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlGoal" runat="server" CssClass="drpClass" Height="26px" Width="150px"  
                    OnSelectedIndexChanged="ddlGoal_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </td>

            <td>
                <asp:DropDownList ID="ddlLesson" runat="server" CssClass="drpClass" Height="26px" Width="150px" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlLesson_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlTeachingMethod" runat="server" CssClass="drpClass" Height="26px" Width="150px" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlTeachingMethod_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                <%--<asp:DropDownList ID="ddlIepYear" runat="server" CssClass="drpClass" Height="26px" Width="150px" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlIepYear_SelectedIndexChanged">
                </asp:DropDownList>--%>
                <asp:DropDownCheckBoxes ID="ddlIepYear" runat="server" CssClass="ddchkLessonYear" UseButtons="false" UseSelectAllNode="false" Height="26px" Width="150px" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlIepYear_SelectedIndexChanged">
                    <Texts SelectBoxCaption="IEP Start Year"/>
                </asp:DropDownCheckBoxes>
            </td>
            <td>
                <asp:DropDownCheckBoxes ID="ddlLessonStatus" runat="server" UseButtons="false" UseSelectAllNode="false" CssClass="ddchkLessonStatus" 
                    AutoPostBack="true" AddJQueryReference="False" 
                    OnSelectedIndexChanged="ddlLessonStatus_SelectedIndexChanged" >  
                    <Texts SelectBoxCaption="Status"/>
                    <Items>
                        <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Pending Approval" Value="2"></asp:ListItem>
                        <asp:ListItem Text="In Progress" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Rejected" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Maintenance" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Inactive" Value="6"></asp:ListItem>
                    </Items>
                </asp:DropDownCheckBoxes>
            </td>
            <td>
                <asp:TextBox ID="txtLessonName" runat="server" CssClass="textClass" ></asp:TextBox>
            </td>
            <td><asp:Button ID="btnGo" runat="server" Text="Go"  BorderStyle="None" Visible="true" CssClass="NFButton" width="25px" style="margin-left: 3px;" OnClick="btnGo_Click"/></td>
            <td><asp:Button ID="btnPDF" runat="server" Text="Export to PDF"  BorderStyle="None" Visible="true" CssClass="NFButton" OnClick="btnPDF_Click" /></td>
            <td><asp:Button ID="btnExcel" runat="server" Text="Export to Excel"  BorderStyle="None" Visible="true" CssClass="NFButton" OnClick="btnExcel_Click" /></td>
            <td><asp:Button ID="btnAdd" runat="server" Text="Add New"  BorderStyle="None" Visible="true" CssClass="NFButton" OnClick="btnAdd_Click" /></td>
        </tr>
    </table>

    <div style="visibility:visible">
        <asp:GridView ID="grdDatabankView" runat="server" AutoGenerateColumns="False" Width="100%"
            GridLines="None" CellPadding="4" ForeColor="#333333" EmptyDataText="No Data Found....."
            Visible="true" AllowPaging="True" OnPageIndexChanging="grdDatabankView_PageIndexChanging" OnRowCommand="grdDatabankView_RowCommand" 
            OnRowDeleting="grdDatabankView_RowDeleting">
            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
            <RowStyle CssClass="RowStyle" />
            <FooterStyle CssClass="FooterStyle" Font-Bold="True" ForeColor="White" />
            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <AlternatingRowStyle CssClass="AltRowStyle" />
            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
            <EditRowStyle BackColor="#7C6F57" />
            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
            <Columns>
                <asp:BoundField DataField="GoalName" HeaderText="Goal" />
                <asp:BoundField DataField="LessonPlanName" HeaderText="Lesson Name" />
                <asp:BoundField DataField="TeachingMethod" HeaderText="Teaching Method" />

                <asp:TemplateField HeaderText="Preview" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="lb_Preview" CommandName="preview" runat="server" class="btn btn-purple" Width="20px" 
                            ImageUrl="~/Administration/Images/view_02.png"      
                            CommandArgument='<%# Eval("LessonPlanId")+","+Eval("GoalId")+","+Eval("DSTempHdrId") %>' >
                        </asp:ImageButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Open/Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="lb_OpenorEdit" CommandName="OpenOrEdit" runat="server" class="btn btn-blue" Width="18px" 
                            ImageUrl="~/Administration/images/user_edit.png" 
                           
                            CommandArgument='<%# Eval("DSTempHdrId") %>' >
                        </asp:ImageButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="lb_delete" CommandName="Delete" runat="server" class="btn btn-red" Width="18px"
                            ImageUrl="~/Administration/images/trash.png" 
                            OnClientClick="javascript: return deleteConfirm();"
                            CommandArgument='<%# Eval("DSTempHdrId")+","+Eval("StudentId")+","+Eval("LessonPlanId") %>' >
                        </asp:ImageButton>                            
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Export" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="lb_datbnk_exprt" CommandName="Export" runat="server" Width="30px" Style="margin-top:3px;"
                            ImageUrl="~/Administration/Images/exportwordLessontemplate.png"      
                            CommandArgument='<%# Eval("LessonPlanId")+","+Eval("DSTempHdrId") %>' >
                        </asp:ImageButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
            <RowStyle CssClass="RowStyle" />
            <AlternatingRowStyle CssClass="AltRowStyle" />
            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
            <SortedAscendingCellStyle BackColor="#F7F7F7" />
            <SortedAscendingHeaderStyle BackColor="#487575" />
            <SortedDescendingCellStyle BackColor="#E5E5E5" />
            <SortedDescendingHeaderStyle BackColor="#275353" />
        </asp:GridView>
                <br />
        <asp:GridView ID="grdClientView" runat="server" AutoGenerateColumns="False" Width="100%"
            GridLines="None" CellPadding="4" ForeColor="#333333" EmptyDataText="No Data Found....."
            Visible="true" AllowPaging="True" OnPageIndexChanging="grdClientView_PageIndexChanging" OnRowCommand="grdClientView_RowCommand">
            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
            <RowStyle CssClass="RowStyle" />
            <FooterStyle CssClass="FooterStyle" Font-Bold="True" ForeColor="White" />
            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <AlternatingRowStyle CssClass="AltRowStyle" />
            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
            <Columns>
                <asp:BoundField DataField="StudentName" HeaderText="Client Name" />
                <asp:BoundField DataField="GoalName" HeaderText="Goal Name" />
                <asp:BoundField DataField="LessonName" HeaderText="Lesson Name" />
                <asp:BoundField DataField="IEPSDate" HeaderText="IEP Start Date" />
                <asp:BoundField DataField="IEPEDate" HeaderText="IEP End Date" />
                <asp:BoundField DataField="LessonStatus" HeaderText="Status" />

                <asp:TemplateField HeaderText="Preview" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="lb_Preview_Client" CommandName="preview" runat="server" class="btn btn-purple" Width="20px" 
                            ImageUrl="~/Administration/Images/view_02.png"      
                            CommandArgument='<%# Eval("LessonPlanId")+","+Eval("DSTempHdrId")+","+Eval("GoalId")+","+Eval("StudentId") %>' >
                        </asp:ImageButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField>  
                            <ItemTemplate>  
                                <asp:LinkButton ID="copyToBank" runat="server" Text="Copy to Bank" ForeColor="#000099" CommandName="copyToBank"
                                    CommandArgument='<%# Eval("DSTempHdrId")+","+Eval("GoalId")+","+Eval("StudentId")+","+Eval("LessonName") %>'>                                    
                                </asp:LinkButton>  
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Export" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="lb_clnt_exprt" CommandName="Export" runat="server" Width="30px" Style="margin-top:3px;"
                            ImageUrl="~/Administration/Images/exportwordLessontemplate.png"      
                            CommandArgument='<%# Eval("LessonPlanId")+","+Eval("DSTempHdrId")+","+Eval("StudentId") %>' >
                        </asp:ImageButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
            <RowStyle CssClass="RowStyle" />
            <AlternatingRowStyle CssClass="AltRowStyle" />
            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
            <SortedAscendingCellStyle BackColor="#F7F7F7" />
            <SortedAscendingHeaderStyle BackColor="#487575" />
            <SortedDescendingCellStyle BackColor="#E5E5E5" />
            <SortedDescendingHeaderStyle BackColor="#275353" />
        </asp:GridView>
    </div>

    <div id ="paginationBtns" runat="server">
    <table style="width: 100%;">
        <tr>
            <td style="text-align:right">
                <b>Show </b>
                <asp:Button ID="btn10" runat="server" Text="10" Width="30px" ForeColor="Blue" Font-Size="10pt" style="border:none" OnClick="btn10_Click" />
                <asp:Button ID="btn20" runat="server" Text="20" Width="30px" ForeColor="Blue" Font-Size="10pt" style="border:none" OnClick="btn20_Click" />
                <asp:Button ID="btn50" runat="server" Text="50" Width="30px" ForeColor="Blue" Font-Size="10pt" style="border:none" OnClick="btn50_Click" />
                <asp:Button ID="btn100" runat="server" Text="100" Width="30px" ForeColor="Blue" Font-Size="10pt" style="border:none" OnClick="btn100_Click" />
                <asp:Button ID="btnAll" runat="server" Text="All" Width="30px" ForeColor="Blue" Font-Size="10pt" style="border:none" OnClick="btnAll_Click" />
                &nbsp;
                <b> per pages</b>
            </td>
        </tr>
    </table>
    </div>

    <div  id="divCopyToDatabank" class="web_dialog" runat="server" style="left: 500px; top: 200px; width: 700px; height: 100px;">  
        <a id="closCopyToDatabank" onclick="closePOP();" href="#" style="margin-top: -13px; margin-right: -14px;">
            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" />
        </a>
        <table >
            <tr><td colspan="3" style="height:20px"></td></tr>
            <tr>
                <td id="tdMsgExprt" colspan="3" runat="server"></td>
            </tr>
            <tr>
                <td>Lesson Plan Name</td>
                <td><span style="color: red">*</span></td>
                <td>
                    <asp:TextBox id="txtCopyLessonName" runat="server" style="width: 180px;" ></asp:TextBox>
                </td>
            </tr>
            <tr><td colspan="3" style="height:20px"></td></tr>
            <tr>
                <td colspan="3" style="float:right">
                    <asp:Button ID="btnCopyToDataBank" runat="server" CssClass="NFButton" Style="width: 180px" Text="Copy Lesson Plan" OnClientClick="return ExecuteLPExist();" OnClick="btnCopyToDataBank_Click"  /><%----%>
                </td>
            </tr>
        </table>
    </div>

    <div id="divLoadClpToAdmin" class="divLoadClpToAdmin" style="position:absolute; left:100px; top:220px; width:80%; height: 700px; background-color:white;overflow: hidden">
        <a id="A1" onclick="HideClpFromAdmin();" href="#" style="margin-top: 0px; margin-right: 0px;">
            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="25" height="25" alt="" />
        </a>
       <iframe id="ifrmAdminLps" style="width: 100%; height: 670px; overflow-y: auto; border:none"></iframe>
    </div>  
                         
</asp:Content>