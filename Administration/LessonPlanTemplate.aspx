<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true" CodeFile="LessonPlanTemplate.aspx.cs" Inherits="Administration_AAa" EnableEventValidation="false" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--EnableEventValidation="false"--%>

    <script src="../StudentBinder/jsScripts/jq1.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="../StudentBinder/jsScripts/eye.js"></script>
    <script type="text/javascript" src="../StudentBinder/jsScripts/layout.js"></script>
    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/2.0.5/FileSaver.min.js"></script>

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

        .popupOverlay
{
    display:none;
    position:fixed;
    top:0;
    left:0;
    width:100%;
    height:100%;
    background:rgba(0,0,0,0.3);
    z-index:9999;
}

.popupBox
{
    position:absolute;
    top:150px;
    left:50%;
    transform:translateX(-50%);
    width:500px;
    background:#f2f2f2;
    border:4px solid #9cc1ba;
    padding:25px;
    box-shadow:0px 0px 10px #666;
}

.closeBtn
{
    float:right;
    font-size:20px;
    cursor:pointer;
    color:red;
}

.popupTextbox
{
    width:220px;
    height:25px;
}

.searchIcon
{
    width:24px;
    cursor:pointer;
    vertical-align:middle;
    margin-left:5px;
}

.bottomLogo
{
    position:absolute;
    bottom:10px;
    right:15px;
    width:120px;
}
.popup-box {
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);

    width: 600px;
    max-width: 90%;

    max-height: 80vh;   /* important */
    overflow-y: auto;   /* scroll enabled */

    background: #fff;
    border-radius: 8px;
    box-shadow: 0 0 15px rgba(0,0,0,0.4);

    z-index: 9999;
}

/* Header */
.popup-header {
    position: sticky;   /* stays visible while scrolling */
    top: 0;
    background: #f5f5f5;

    padding: 10px;
    text-align: right;

    border-bottom: 1px solid #ddd;
    z-index: 1;
}

/* Close Button */
.close-btn {
    font-size: 30px;
    font-weight: bold;
    cursor: pointer;
    color: #444;
}

.close-btn:hover {
    color: red;
}

/* Content */
.popup-content {
    padding: 20px;
}
/* Overlay */
.loader-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0,0,0,0.5);

    display: flex;
    justify-content: center;
    align-items: center;

    z-index: 99999;
}

/* Loader Box */
.loader-box {
    background: white;
    padding: 25px 35px;
    border-radius: 10px;
    text-align: center;
    font-size: 18px;
    box-shadow: 0 0 15px rgba(0,0,0,0.3);
}

/* Spinner */
.spinner {
    width: 50px;
    height: 50px;
    border: 5px solid #ddd;
    border-top: 5px solid #0078d7;
    border-radius: 50%;

    margin: auto;
    margin-bottom: 15px;

    animation: spin 1s linear infinite;
}

@keyframes spin {

    100% {
        transform: rotate(360deg);
    }
}
        </style>
  
    <script type="text/javascript">
        <%--window.onfocus = function () {
            //if (document.getElementById("loaderOverlay")) {
            //    hideLoader();
            //}
            if (document.getElementById('<%= exportstatus.ClientID %>').value == "1") {
                hideLoader();
                document.getElementById('<%= exportstatus.ClientID %>').value = "";
            }
        };--%>
        function templateChanged(chk) {
            if (chk.checked) {
                document.getElementById('lblCopyTo').style.display = 'none';
                document.getElementById('<%= txtIndividualName.ClientID %>').style.display = 'none';
                document.getElementById('imgSearch').style.display = 'none';
                document.getElementById('<%=DlStudent.ClientID %>').style.display = 'none';
                document.getElementById('lblSelectedStudent').style.display = 'none';
                checkJson('3');
            }
            else {
                document.getElementById('lblCopyTo').style.display = 'block';
                document.getElementById('<%= txtIndividualName.ClientID %>').style.display = 'block';
                document.getElementById('imgSearch').style.display = 'block';
                document.getElementById('lblCopyTo').style.display = 'block';
                document.getElementById('lblSelectedStudent').style.display = 'block';


                checkJson('2');

            }
        }
        function hideButton() {
            var txt2 = document.getElementById('<%= btnUploadJson.ClientID %>');
            txt2.style.display = "none";
            var txt = document.getElementById('<%= txtlessname.ClientID %>');
            txt.style.display = "none";
        }
        function clearHiddenField(e) {
            if (e.key === "Backspace") {
                document.getElementById('<%= txtIndividualName.ClientID %>').value = "";
                document.getElementById('<%= hfSelectedStudentId.ClientID %>').value = "";
                document.getElementById('lblSelectedStudent').innerHTML = "";
                var txt2 = document.getElementById('<%= btnUploadJson.ClientID %>');
                txt2.style.display = "none";
                checkJson('2');
            }
            if (e.key === "Enter") {
                //e.preventDefault(); 
                //e.stopPropagation();
                searchIndividual();
                return false;
            }
        }
        function downloadJsonFile(jsonData) {

            try {

                var blob = new Blob(
                    [jsonData],
                    { type: "application/json;charset=utf-8" }
                );

                saveAs(blob, "lessonexport.json");
            }
            finally {

                hideLoader();
            }
        }
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
        function showbeforeExportPopup() {
            var txt2 = document.getElementById('<%= btnUploadJson.ClientID %>');
            txt2.style.display = "none";
            document.getElementById('lblSelectedStudent').innerHTML = "";
            document.getElementById('<%= txtIndividualName.ClientID %>').value = "";
            var resultBox =
                document.getElementById('<%=DlStudent.ClientID %>');
            resultBox.style.display = 'none';
            var txt3 = document.getElementById('<%= lesscount.ClientID %>');
            txt3.style.display = "none";
            document.getElementById(
                        '<%= hfSelectedStudentId.ClientID %>'
            ).value = "";
            document.getElementById('<%= chkTemplate.ClientID %>').checked = false;
            document.getElementById('<%= fileJsonUpload.ClientID %>').value = '';
            document.getElementById('<%= lblUploadStatus.ClientID %>').value = '';
            document.getElementById('<%= lesscount.ClientID %>').value = '';
            showExportPopup()
        }

            function showExportPopup()
            {
                document.getElementById("exportPopup").style.display = "block";
            }

            function closeExportPopup()
            {
                document.getElementById("exportPopup").style.display = "none";
            }
        function searchIndividual() {

            var name =
                document.getElementById('<%= txtIndividualName.ClientID %>').value;


            if (name.length < 4)
                return;

            $.ajax({

                type: "POST",

                url: "LessonPlanTemplate.aspx/GetIndividualNames",

                data: JSON.stringify({ searchText: name }),

                contentType: "application/json; charset=utf-8",

                dataType: "json",

                success: function (response) {
                    var data = response.d;

                    var resultBox =
                        document.getElementById('<%=DlStudent.ClientID %>');

    resultBox.innerHTML = "";

    if (!data || data.length === 0) {
        var div = document.createElement("div");
        div.innerHTML = "Student not found";
        div.style.color = "red";
        div.style.padding = "5px";

        resultBox.appendChild(div);
        resultBox.style.display = "block";
        return;
    }

    var lbl = document.createElement("div");
    lbl.innerHTML = "<strong>Choose Name:</strong>";
    lbl.style.padding = "5px";
    resultBox.appendChild(lbl);

    data.forEach(function (item) {
        var div = document.createElement("div");

        div.innerHTML = item.Name;

        div.onclick = function () {
            document.getElementById('<%= txtIndividualName.ClientID %>').value = item.Name;

            selectStudentId(item.Id, item.Name);

            resultBox.style.display = "none";
        };

        resultBox.appendChild(div);
    });

                  resultBox.style.display = "block";
              }

            });
        }
        function selectStudentId(studentId,studname) {
            document.getElementById(
        '<%= hfSelectedStudentId.ClientID %>'
            ).value = studentId;
            document.getElementById('lblSelectedStudent').innerHTML =
                'Selected student is: ' + studname;
            checkJson('2');
        }
        function showDuplicatePopup() {
                var modal = document.getElementById("duplicateModal");
                if (modal) {
                    modal.style.display = "block";
                }
            
        }

        function continueProcess() {
            document.getElementById("duplicateModal").style.display = "none";
            showLoader();
            __doPostBack('ContinueProcess', '');

        }

        function cancelProcess() {
            document.getElementById("duplicateModal").style.display = "none";
        }

        function showLoader() {

            document.getElementById("loaderOverlay").style.display = "flex";
        }
        function hideLoader() {

            document.getElementById("loaderOverlay").style.display = "none";
        }

        function checkJson(numb) {
            if (numb == "1") {
                var fileInput = document.getElementById('<%= fileJsonUpload.ClientID %>');

                if (!fileInput || fileInput.files.length === 0) {
                    alert("Please select a JSON file");
                    return;
                }
                document.getElementById('<%= lesscount.ClientID %>').innerText = "";
                var txt = document.getElementById('<%= txtlessname.ClientID %>');
                txt.style.display = "none";
                txt.value = "";

                var file = fileInput.files[0];
                var reader = new FileReader();

                reader.onload = function (e) {
                    try {
                        var jsonData = JSON.parse(e.target.result);

                        var lesson = jsonData.LessonNameAndGoal;

                        if (jsonData && lesson) {

                            var length = jsonData.LessonNameAndGoal.length;

                            if (length == 1) {

                                jsonData.LessonNameAndGoal.forEach(function (item, index) {
                                    var txt = document.getElementById('<%= txtlessname.ClientID %>');
                                    txt.style.display = "block";
                                    txt.value = item.DSTemplateName;

                                });
                            }
                            else {
                                document.getElementById('<%= lesscount.ClientID %>').style.display = 'block';
                                document.getElementById('<%= lesscount.ClientID %>').innerText = length + '  Lessons selected.Continue?';

                            }
                            if (document.getElementById('<%= hfSelectedStudentId.ClientID %>').value != "") {
                                var txt2 = document.getElementById('<%= btnUploadJson.ClientID %>');
                                txt2.style.display = "block";
                            }
                            else {
                                var chk = document.getElementById('<%= chkTemplate.ClientID %>');

                                if (chk.checked) {
                                    var txt2 = document.getElementById('<%= btnUploadJson.ClientID %>');
                                    txt2.style.display = "block";
                                }

                            }

                        } else {
                            alert("LessonNameAndGoal not found ");
                            var txt2 = document.getElementById('<%= btnUploadJson.ClientID %>');
                            txt2.style.display = "none";
                        } 

                    } catch (err) {
                        alert("Invalid JSON file");
                        var txt2 = document.getElementById('<%= btnUploadJson.ClientID %>');
                        txt2.style.display = "none";
                    }
                };

                reader.readAsText(file);
            }
            else {
                if (numb == "2") {
                    var fileInput = document.getElementById('<%= fileJsonUpload.ClientID %>');
                    if (fileInput && fileInput.files.length > 0) {
                        if (document.getElementById('<%= hfSelectedStudentId.ClientID %>').value != "") {
                            var txt2 = document.getElementById('<%= btnUploadJson.ClientID %>');
                            txt2.style.display = "block";
                        }
                    }

                }
                if (numb == "3") {
                    var fileInput = document.getElementById('<%= fileJsonUpload.ClientID %>');
                    if (fileInput && fileInput.files.length > 0) {
         var txt2 = document.getElementById('<%= btnUploadJson.ClientID %>');
         txt2.style.display = "block";
     
 }
                }
            }
        }

        function showLessonsDiv() {

            document.getElementById("lessonPopup").style.display = "block";
        }

        function closeLessonsDiv() {

            document.getElementById("lessonPopup").style.display = "none";
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
        <td id="tdMsg" colspan="6" runat="server" style="width:75%"></td>
        <td>
            <asp:HiddenField ID="hdnLessonName" runat="server" />
        </td>
    </tr>

    <!-- Filter Row -->
    <tr>
        <td colspan="4">
            <b>Filter:</b>
        </td>

        <td>
            <p id="iepPtag" runat="server"></p>
        </td>

        <td></td>

        <td>
            <b>Search Lesson Name:</b>
        </td>

        <td>
            <input type="button"
                id="btnimpMEDS"
                runat="server"
                value="Import LPs"
                class="NFButton"
                visible="false"
                onclick="showbeforeExportPopup();" />
        </td>

        <td>
            <asp:Button
                ID="btnexp"
                runat="server"
                Text="Export LPs"
                Visible="false"
                CssClass="NFButton"
                OnClientClick="showLoader()"
                OnClick="buttonexp_Click" />
        </td>

        <td colspan="2"></td>
    </tr>

    <!-- Controls Row -->
    <tr>
        <td>
            <asp:DropDownList ID="ddlClientName" runat="server"
                CssClass="drpClass"
                Height="26px"
                Width="150px"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlClientName_SelectedIndexChanged">
            </asp:DropDownList>
        </td>

        <td>
            <asp:DropDownList ID="ddlGoal" runat="server"
                CssClass="drpClass"
                Height="26px"
                Width="150px"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlGoal_SelectedIndexChanged">
            </asp:DropDownList>
        </td>

        <td>
            <asp:DropDownList ID="ddlLesson" runat="server"
                CssClass="drpClass"
                Height="26px"
                Width="150px"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlLesson_SelectedIndexChanged">
            </asp:DropDownList>
        </td>

        <td>
            <asp:DropDownList ID="ddlTeachingMethod" runat="server"
                CssClass="drpClass"
                Height="26px"
                Width="150px"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlTeachingMethod_SelectedIndexChanged">
            </asp:DropDownList>
        </td>

        <td>
            <asp:DropDownCheckBoxes ID="ddlIepYear" runat="server"
                CssClass="ddchkLessonYear"
                UseButtons="false"
                UseSelectAllNode="false"
                Height="26px"
                Width="150px"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlIepYear_SelectedIndexChanged">
                <Texts SelectBoxCaption="IEP Start Year" />
            </asp:DropDownCheckBoxes>
        </td>

        <td>
            <asp:DropDownCheckBoxes ID="ddlLessonStatus" runat="server"
                CssClass="ddchkLessonStatus"
                UseButtons="false"
                UseSelectAllNode="false"
                AutoPostBack="true"
                AddJQueryReference="False"
                OnSelectedIndexChanged="ddlLessonStatus_SelectedIndexChanged">
                <Texts SelectBoxCaption="Status" />
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
            <asp:TextBox ID="txtLessonName" runat="server"
                CssClass="textClass">
            </asp:TextBox>
        </td>

        <!-- All action buttons together -->
        <td colspan="4" style="white-space:nowrap;">
            <asp:Button ID="btnGo" runat="server"
                Text="Go"
                BorderStyle="None"
                CssClass="NFButton"
                Width="25px"
                Style="margin-left:3px;"
                OnClick="btnGo_Click" />

            <asp:Button ID="btnPDF" runat="server"
                Text="Export to PDF"
                BorderStyle="None"
                CssClass="NFButton"
                OnClick="btnPDF_Click" />

            <asp:Button ID="btnExcel" runat="server"
                Text="Export to Excel"
                BorderStyle="None"
                CssClass="NFButton"
                OnClick="btnExcel_Click" />

            <asp:Button ID="btnAdd" runat="server"
                Text="Add New"
                BorderStyle="None"
                CssClass="NFButton"
                OnClick="btnAdd_Click" />
        </td>
    </tr>

</table>


<asp:HiddenField ID="hfSelectedStudentId" runat="server" />
<asp:HiddenField ID="hdnPopupValue" runat="server" />
<asp:HiddenField ID="hfpopupshow" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="exportparameter1" runat="server" />
<asp:HiddenField ID="exportparameter2" runat="server" />
<asp:HiddenField ID="exportparameter3" runat="server" />


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
            Visible="true" AllowPaging="True" OnPageIndexChanging="grdClientView_PageIndexChanging" OnRowCommand="grdClientView_RowCommand" OnRowDataBound="grdClientView_RowDataBound">
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
                 <asp:TemplateField HeaderText="MEDS Export" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
     <ItemTemplate>
         <asp:ImageButton ID="lb_clnt_exprtmds" CommandName="MEDS Export" runat="server" Width="30px" Style="margin-top:3px;"
             ImageUrl="~/Administration/Images/logoexp.png"      
             CommandArgument='<%# Eval("LessonPlanId")+","+Eval("DSTempHdrId")+","+Eval("StudentId") %>' >
         </asp:ImageButton>
     </ItemTemplate>
     <HeaderStyle HorizontalAlign="Center" />
     <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
           <div id="exportPopup"  class="popupOverlay">

    <div class="popupBox">

        <!-- CLOSE BUTTON -->
        <span class="closeBtn" onclick="closeExportPopup()">×</span>

        <br />

        <label>
            <input type="checkbox" id="chkTemplate" runat="server" onchange="templateChanged(this);"/>
            Copy Lesson Plan as Template
        </label>

        <br /><br />

       <div id="copyContainer" style="display:flex; align-items:center; gap:8px;">
    <label id="lblCopyTo">Copy Lesson Plan to</label>

    <input type="text"
           id="txtIndividualName"
           runat="server"
           class="popupTextbox"
           placeholder="Please enter at least 4 characters."
           autocomplete="off"
           onkeyup="clearHiddenField(event);"
         onkeydown="if(event.keyCode==13) return false;"/>

    <img id="imgSearch"
         src="images/ex.png"
         class="searchIcon"
         onclick="searchIndividual()" />
</div>

       <div id="DlStudent" class="searchDropdown" runat="server"></div>
        <br />
        <span id="lblSelectedStudent" style="color:green;"></span>
        <br />

    <!-- JSON FILE UPLOAD CONTROL -->
    <asp:FileUpload ID="fileJsonUpload"
        runat="server"
        CssClass="popupTextbox" onchange="hideButton();" />
        <asp:Button ID="btnCheckJson" runat="server"
    Text="Verify File"
                    CssClass="NFButton"
    OnClientClick="checkJson('1'); return false;" />
    <br /><br />
         <asp:Label ID="lesscount"
     runat="server"
     ForeColor="Green" />
         <br /><br />


      <input type="text"
 id="txtlessname"
 runat="server"
 style="display:none;"
 class="popupTextbox"
 placeholder="Lesson Name" />
         <br /><br />

    <!-- UPLOAD BUTTON -->
    <asp:Button ID="btnUploadJson"
        runat="server"
        Text="Import"
         style="display:none;"
        OnClientClick="showLoader()"
        CssClass="NFButton"
        OnClick="btnUploadJson_Click" />

    <br /><br />

    <!-- STATUS LABEL -->
    <asp:Label ID="lblUploadStatus"
        runat="server"
        ForeColor="Red" />

        <!-- BOTTOM RIGHT LOGO -->
        <img src="images/ex2.png"
             class="bottomLogo" />

    </div>
               </div>              


             <div id="duplicateModal" 
     style="display:none; position:fixed; top:50%; left:50%;
            transform:translate(-50%, -50%);
            background:white; padding:20px;
            border:1px solid #ccc; border-radius:8px;
            box-shadow:0 0 10px rgba(0,0,0,0.3);
            z-index:9999; width:300px;">

    <h3>⚠ Warning</h3>
    <p>This lesson name already exists. Do you want to continue?</p>

    <a href="javascript:void(0);" onclick="showLessonsDiv()">
    Click here to see existing lessons
</a>


    <br /><br />

    <button onclick="continueProcess()" class="NFButton">OK</button>
    <button onclick="cancelProcess()"    class="NFButton">Cancel</button>
</div>
          <div id="lessonPopup" class="popup-box" style="display:none;">

    <div class="popup-header">
        <span class="close-btn" onclick="closeLessonsDiv()">×</span>
    </div>

    <!-- Dynamic Content Area -->
    <div class="popup-content" id="lesslist" runat="server">
    </div>

</div>
  <div id="loaderOverlay" class="loader-overlay" style="display:none;">
    
    <div class="loader-box">
        <div class="spinner"></div>
        <div>Please wait...</div>
    </div>

</div>
       <asp:Button ID="btnExport"
    runat="server"
    Text="Export"
    CssClass="NFButton"
    OnClick="btnExport_Click"
    Style="display:none;" />

</asp:Content>