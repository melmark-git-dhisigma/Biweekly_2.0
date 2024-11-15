<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomizeTemplateEditor.aspx.cs" Inherits="StudentBinder_CustomizeTemplateEditor" ValidateRequest="false" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Untitled Document</title>
    <script src="js/jquery-1.8.0.min.js"></script>
    <script src="js/jquery-2.1.0.js"></script>
    <%--<script src="../Administration/JS/jquery-1.8.0.js"></script>   --%>
    <%--  <script src="jsScripts/jQuery.js" type="text/javascript"></script>--%>
    <style type="text/css">


    .frmtchngpopup-content {
        display:none;
    background-color: white;
    width: 400px;
    height: 120px;
    padding: 20px;
    border-color:#03507D;
    border-width: 3px;
    border-style: solid;
    border-radius: 5px;
    position: absolute;
    top: 25%;
    left: 50%;
    transform: translate(-50%, -50%);
    }
        .warning_box {
            width: 20%;
            clear: both;
            background: url(../Administration/images/warning.png) no-repeat left #fcfae9;
            border: 1px #e9e6c7 solid;
            background-position: 10px 1px;
            padding-left: 10px;
            padding-top: 10px;
            padding-bottom: 5px;
            text-align: left;
            color: Red;
        }
        .PanelDiv {
        float:left;
        }
        .toolTipImg{  
           position: absolute;
           display: inline-block; 
            margin-top: -3px;
           }
       .tooltiptext {
            visibility: hidden;
            width: 340px;
            background-color: whitesmoke;
            color: black;
            text-align: center;
            padding: 5px 0;
            position:fixed;
            z-index: 1;
            border-radius:3px 3px;
            margin-left:10px;
            font-weight:normal;
           
        }
        .toolTipImg:hover + .tooltiptext {
            visibility: visible;
        }
        .toolTipImg:hover  .tooltiptext {
            visibility: visible;
        }
        #popupEmailMain {
            background-color: #FFFFFF;
            display: none;
            height: 550px;
            margin: auto;
            padding: 5px;
            position: absolute;
            top: 10px;
            width: 974px;
            z-index: 10000;
        }
        .ddchkLessonYear {
            width:130px;
            Height:300px;
            height: 19px !important;
            color: #676767 !important;
            border-radius: 3px !important;
            padding: 3px 3px 3px 3px!important;
            background-image: url(./images/statusdownarow.png) !important;
            background-size: 14px 17px !important;
            background-position-x: 97%!important;
        }
        div.dd_chk_select div#caption{
            top: 3px !important;
        }
        div.dd_chk_drop div#checks {
            Height: 150px;
            width:128px;
            background-color: #F2EEEE;
        }
        div.dd_chk_drop {
            margin-top: 10px;
            Height: 150px;
        }
    </style>
    <script>

        //Multiple click prevention
        var multiClickSet = false;
        var multiClickStep = false;
        var multiClickCriteria = false;
        var multiClickSubmit = false;
        var multiClickDelete = false;
        var multiClickCopy = false;
        var multiClickVersion = false;
        var multiClickActive = false;
        var multiClickReject = false;
        function enableButton() {
            multiClickSet = false;
            multiClickStep = false;
            multiClickCriteria = false;
            multiClickSubmit = false;
            multiClickDelete = false;
            multiClickCopy = false;
            multiClickVersion = false;
            multiClickActive = false;
            multiClickReject = false;
        }

        function PlusminusAoTb1() {
            var x = document.getElementById('<%= aoTb.ClientID %>');
            z = x.style.display;
            if (z== "none") {
                x.style.display = "block";
                return false;
              }
            else 
            {
                x.style.display = "none";
                return false;
            }
          return true;
       }
     function PlusminusAoTb2() {
         var x = document.getElementById('<%= aoTb2.ClientID %>');
         var y = document.getElementById('<%= opdiv1.ClientID %>');
         if (x.style.display === "none")
         {
               x.style.display = "block";
               y.style.display = "block";
               return false;
           }
           else {
               x.style.display = "none";
               y.style.display = "none";
               return false;
           }
           return true;
     }
        function PromptAoTbdisplay() {
         var x = document.getElementById('<%= promptAOTb.ClientID %>');
         if (x.style.display === "none") {
             x.style.display = "block";
             return false;
         }
         else {
             x.style.display = "none";
             return false;
         }
         return true;
     }
     function PromptAoTbdisplay2() {
         var x = document.getElementById('<%= promptAOTb2.ClientID %>');
         var y = document.getElementById('<%= opdiv2.ClientID %>');
         if (x.style.display === "none") {
             x.style.display = "block";
             y.style.display = "block";
             return false;
         }
         else {
             x.style.display = "none";
             y.style.display = "none";
             return false;
         }
         return true;
     }
     function TextDivDisplay() {
         var x = document.getElementById('<%= textAOTb.ClientID %>');
         if (x.style.display === "none") {
             x.style.display = "block";
             return false;
         }
         else {
             x.style.display = "none";
             return false;
         }
         return true;
     }
     function DurationDivdisplay() {
         var x = document.getElementById('<%= durationAOTb.ClientID %>');
         if (x.style.display === "none") {
             x.style.display = "block";
             return false;
         }
         else {
             x.style.display = "none";
             return false;
         }
         return true;
     }
     function FrequencyDivdisplay() {
         var x = document.getElementById('<%= frequencyAOTb.ClientID %>');
         if (x.style.display === "none") {
             x.style.display = "block";
             return false;
         }
         else {
             x.style.display = "none";
             return false;
         }
         return true;
     }
     
    </script>
    <script src="jsScripts/jq1.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="jsScripts/eye.js"></script>
    <script type="text/javascript" src="jsScripts/layout.js"></script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="CSS/CustomizeTemplate.css" rel="stylesheet" id="sized" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" type="text/css" />
     <link href="../Administration/CSS/jsDatePickforGraph.css" rel="stylesheet" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js"></script>
   
    <%--  --%>
 

     <script type="text/javascript">
         window.onmousedown = function () {
             new JsDatePick({
                 useMode: 2,
                 target: "<%=lessonEDate.ClientID%>",
                 dateFormat: "%m/%d/%Y",
             });
             new JsDatePick({
                 useMode: 2,
                 target: "<%=lessonSDate.ClientID%>",
                 dateFormat: "%m/%d/%Y",
             });
         } 
        
      </script>
    <script type="text/javascript">
        
  <%--      $(document).ready(function () {
            $('#chkCpyStdtTemplate').click(function () {
                alert($(this).prop("checked"));
                if ($(this).prop("checked") == true) {
                    $("#<%=btnCopyTempAdmin.ClientID%>").css("display", "block");
                $("#divCopyTempAdmin").css("display", "block");
                var Lesson = $("#hdDefaultName").val();
                $("#txtCopyTempAdmin").val(Lesson);
            }
            else if ($(this).prop("checked") == false) {
                $("#<%=btnCopyTempAdmin.ClientID%>").css("display", "none");
                        $("#divCopyTempAdmin").css("display", "none");
                        var Lesson = $("#hdDefaultName").val();
                        $("#txtCopyTempAdmin").val(Lesson);
                    }

        });

        });--%>

        $(document).ready(function () {
            $("#dlLPforSorting").find('br').remove();
        });

        function ValidateEmail() {
            $('#divApprMsg').css("display", "none");
            $('#divApprMsg').html('');
            var isChecked = false;
            var panel = document.getElementById("<%= normalOverride.ClientID %>");
            if (panel) {
                var RB1 = document.getElementById("<%=RadioButtonListSets.ClientID%>");
                var radio = RB1.getElementsByTagName("input");
                isChecked = false;
                for (var i = 0; i < radio.length; i++) {
                    if (radio[i].checked) {
                        isChecked = true;
                        break;
                    }
                }
                if (!isChecked) {
                    $('#divApprMsg').css("display", "block");
                    $('#divApprMsg').html('You must complete your selections above before hitting Save');
                }
                if (isChecked) {
                    var RB2 = document.getElementById("<%=RadioButtonListSteps.ClientID%>");
                    if (RB2 != null) {
                        isChecked = false;
                        var radio1 = RB2.getElementsByTagName("input");
                        for (var i = 0; i < radio1.length; i++) {
                            if (radio1[i].checked) {
                                isChecked = true;
                                break;
                            }
                        }
                        if (!isChecked) {
                            $('#divApprMsg').css("display", "block");
                            $('#divApprMsg').html('You must complete your selections above before hitting Save');
                        }
                    }
                    var RB3 = document.getElementById("<%=RadioButtonListPrompts.ClientID%>");
                    if (RB3 != null) {
                        isChecked = false;
                        var radio2 = RB3.getElementsByTagName("input");
                        for (var i = 0; i < radio2.length; i++) {
                            if (radio2[i].checked) {
                                isChecked = true;
                                break;
                            }
                        }
                        if (!isChecked) {
                            $('#divApprMsg').css("display", "block");
                            $('#divApprMsg').html('You must complete your selections above before hitting Save');
                        }
                    }
                }
            }
            else {
                isChecked = false;
                var RB4 = document.getElementById("<%=RadioButtonListSets_tt.ClientID%>");
                if (RB4 != null) {
                    var radio = RB4.getElementsByTagName("input");
                    for (var i = 0; i < radio.length; i++) {
                        if (radio[i].checked) {
                            isChecked = true;
                            break;
                        }
                    }
                    if (!isChecked) {
                        $('#divApprMsg').css("display", "block");
                        $('#divApprMsg').html('You must complete your selections above before hitting Save');
                    }
                    if (isChecked) {
                        if ($(".stepDiv").length > 0) {
                            if ($(".stepDiv_sel").length == 0) {
                                isChecked = false;
                                $('#divApprMsg').css("display", "block");
                                $('#divApprMsg').html('You must complete your selections above before hitting Save');
                            }
                        }
                    }
                }

            }
            return isChecked;
        }
        function multiClickCheck(btn) {
            if (!multiClickSubmit) {
                btn.style.opacity = '0.5';
                multiClickSubmit = true;
                return true;
            }
            return false;
        }
        function multiClickCheckVersion(btn){
            if (!multiClickVersion) {
                btn.style.opacity = '0.5';
                multiClickVersion = true;
                return true;
            }
            return false;
        }
        function multiClickCheckActive(btn) {
            if (!multiClickActive) {
                btn.style.opacity = '0.5';
                multiClickActive = true;
                return true;
            }
            return false;
        }
        function multiClickCheckReject(btn) {
            if (!multiClickReject) {
                btn.style.opacity = '0.5';
                multiClickReject = true;
                return true;
            }
            return false;
        }
        function ValidateRadio(btn) {
            if (!multiClickSubmit) {
                $('#divApprMsg').css("display", "none");
                $('#divApprMsg').html('');
                var isChecked = false;
                var panel = document.getElementById("<%= normalOverride.ClientID %>");
                if (panel) {
                    var RB1 = document.getElementById("<%=RadioButtonListSets.ClientID%>");
                    var radio = RB1.getElementsByTagName("input");
                    isChecked = false;
                    for (var i = 0; i < radio.length; i++) {
                        if (radio[i].checked) {
                            isChecked = true;
                            break;
                        }
                    }
                    if (!isChecked) {
                        $('#divApprMsg').css("display", "block");
                        $('#divApprMsg').html('You must complete your selections above before hitting Save');
                    }
                    if (isChecked) {
                        var RB2 = document.getElementById("<%=RadioButtonListSteps.ClientID%>");
                        if (RB2 != null) {
                            isChecked = false;
                            var radio1 = RB2.getElementsByTagName("input");
                            for (var i = 0; i < radio1.length; i++) {
                                if (radio1[i].checked) {
                                    isChecked = true;
                                    break;
                                }
                            }
                            if (!isChecked) {
                                $('#divApprMsg').css("display", "block");
                                $('#divApprMsg').html('You must complete your selections above before hitting Save');
                                if (isChecked) {
                                    multiClickSubmit = true;
                                    btn.style.opacity = '0.5';
                                }
                                return isChecked;
                            }
                        }
                        var RB3 = document.getElementById("<%=RadioButtonListPrompts.ClientID%>");
                        if (RB3 != null) {
                            isChecked = false;
                            var radio2 = RB3.getElementsByTagName("input");
                            for (var i = 0; i < radio2.length; i++) {
                                if (radio2[i].checked) {
                                    isChecked = true;
                                    break;
                                }
                            }
                            if (!isChecked) {
                                $('#divApprMsg').css("display", "block");
                                $('#divApprMsg').html('You must complete your selections above before hitting Save');
                            }
                        }
                    }
                }
                else {
                    isChecked = false;
                    var RB4 = document.getElementById("<%=RadioButtonListSets_tt.ClientID%>");
                    if (RB4 != null) {
                        var radio = RB4.getElementsByTagName("input");
                        for (var i = 0; i < radio.length; i++) {
                            if (radio[i].checked) {
                                isChecked = true;
                                break;
                            }
                        }
                        if (!isChecked) {
                            $('#divApprMsg').css("display", "block");
                            $('#divApprMsg').html('You must complete your selections above before hitting Save');
                        }
                        if (isChecked) {
                            if ($(".stepDiv").length > 0) {
                                if ($(".stepDiv_sel").length == 0) {
                                    isChecked = false;
                                    $('#divApprMsg').css("display", "block");
                                    $('#divApprMsg').html('You must complete your selections above before hitting Save');
                                }
                            }
                        }
                    }

                }
                //if (isChecked) {
                //    loadEmailOrder(this.id);
                //}
                if (isChecked) {
                    multiClickSubmit = true;
                    btn.style.opacity = '0.5';
                }
                return isChecked;
            }
            return false;
        }


        function setUploadButtonState() {
            document.getElementById("divMessage").innerHTML = "";
            var maxFileSize = 22020096; 
            var fileUpload = $('#fupDoc');

            if (fileUpload.val() == '') {
                return false;
            }
            else {
                if (fileUpload[0].files[0].size < maxFileSize) {
                    $('#btUpload').prop('disabled', false);
                    return true;
                } else {         
                    //Clearfile();
                    //alert("Failed to upload due to large file size...");
                    alert("The selected file size exceeds the permitted limit...");
                    Clear();
                    HidePopup1();
                    return false;
                }
            }
        }
        function Clear() {
            //Reference the FileUpload and get its Id and Name.
            var fileUpload = document.getElementById("<%=fupDoc.ClientID %>");
            var id = fileUpload.id;
            var name = fileUpload.name;

            //Create a new FileUpload element.
            var newFileUpload = document.createElement("INPUT");
            newFileUpload.type = "FILE";

            //Append it next to the original FileUpload.
            fileUpload.parentNode.insertBefore(newFileUpload, fileUpload.nextSibling);

            //Remove the original FileUpload.
            fileUpload.parentNode.removeChild(fileUpload);

            //Set the Id and Name to the new FileUpload.
            newFileUpload.id = id;
            newFileUpload.name = name;
            return false;
        }
        function Clearfile() {
                    var fil = document.getElementById('<%=fupDoc.ClientID %>');
                    fil.select();
                    n = fil.createTextRange(); 
                    n.execCommand('delete'); 
                    fil.focus();
        }
        function HidePopup1() {
            $(".fullOverlay").hide();
            $(".web_dialog2").hide();
            $("#<%=divMessage.ClientID%>").hide();
            $(".fullOverlay").show();
            $(".web_dialog2").show();
            $("#<%=divMessage.ClientID%>").show();
            }
        function ViewLessonName() {
            if ($("#txtCopyTempAdmin").val() == "") {
                var LName = $('#<%=txtLessonName.ClientID %>').val();
                $("#txtCopyTempAdmin").val(LName);
            }
        }
        function TempSelected() {
            if ($('#chkCpyStdtTemplate').prop("checked") == true) {
                $("#<%=btnCopyTempAdmin.ClientID%>").css("display", "block");
                $("#divCopyTempAdmin").css("display", "block");
                var Lesson = $("#hdDefaultName").val();
                $("#txtCopyTempAdmin").val(Lesson);
                studclsSearch();
            }
            else if ($('#chkCpyStdtTemplate').prop("checked") == false) {
                $("#<%=btnCopyTempAdmin.ClientID%>").css("display", "none");
                    $("#divCopyTempAdmin").css("display", "none");
                    var Lesson = $("#hdDefaultName").val();
                    $("#txtCopyTempAdmin").val(Lesson);
                    $('.studentDiv').show();
                    $('#txtSname').val('Individual Name');
                    $('#DlStudent').html("");
                }

            ViewLessonName();
        }

        function ExecuteLPExist(btn) {
            if (!multiClickCopy) {
                if (LPExist() == true) {
                    var Name = $("#txtCopyTempAdmin").val();
                    $("#hdLessonName").val(Name);
                    btn.style.opacity = '0.5';
                    multiClickCopy = true;
                    return true;
                }
                else {
                    $("#hdLessonName").val("");
                    return false;
                }
            }
            return false;
        }

        function LPExist() {

            var Name = $("#txtCopyTempAdmin").val();
            var clrName=Name.replace("'", "\\\'");
            var StudId = $("#hfSelectedStudent").val();
            if ($("#chkCpyStdtTemplate").prop('checked') == true) {
                StudId = "";
            }

            if (Name == "") {
                $("#tdMsgExprt").html("<div class='warning_box'>Please enter lesson plan name.</div>");
                return false;
            }
            else {
                var dataresult = false;
                $.ajax(
                 {
                     type: "POST",
                     url: "CustomizeTemplateEditor.aspx/SearchLessonPlanList",
                     //data: "{'Name':'" + Name + "','StudId':'" + StudId + "'}",
                     data: "{'Name':'" + clrName + "','StudId':'" + StudId + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,
                     success: function (data) {
                         if (data.d == "0") {
                             dataresult = true;
                         }
                         else {
                             $("#tdMsgExprt").html("<div class='warning_box'>Lesson plan name already exist. Please enter another name...</div>");
                             dataresult = false;
                         }
                     },
                     error: function (request, status, error) {
                         alert("Error");
                     }
                 });

                return dataresult;
            }

        }
        function LoadDatasheetView() {
            //alert(LpId);
            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('slow', function () {
                $('#ifrmeDatasheet').attr('src', 'DatasheetPreview.aspx');
                $('#DatasheetContainerDiv').fadeIn();

                var $iFrameContents = $('#ifrmeDatasheet').contents(),
                $entryContent = $iFrameContents.find('#DatasheetContainerDiv');



                $entryContent.find('input[type = button]').hide();
            });


            //$('input[type=button]').attr('disabled', true);


        }

        function checkLbvals() {
            var statFlag = false;
            var listb1 = document.getElementById("<%= lstSelectdEmails.ClientID %>");
            var len1 = listb1.options.length;

            var listb2 = document.getElementById("<%= lstSelecteUserNames.ClientID %>");
            var len2 = listb2.options.length;

            if (len1 > 0 && len2 > 0) {
                var delCon = "";
                var selInd2 = listb2.selectedIndex;
                //console.log(selInd2);
                if (selInd2 > -1) {
                    for (var i = 0; i < len2; i++) {
                        if (listb2.options[i].selected) {
                            delCon = confirm('Are you sure you want remove the selected user?');
                            statFlag = delCon;
                            //console.log(delCon);
                        }
                    }
                }
                else { 
                    //console.log(statFlag);
                    if (!statFlag) {
                        //console.log(statFlag);
                        $('#msgAlert').css("display", "block");
                        $('#msgAlert').html('Please select a username to remove');
                        setTimeout(function () {
                            $("#msgAlert").css("display", "none");
                        }, 4000);
                        statFlag = false;
                    }
                }
            }
            else {
                if (!statFlag) {
                    $('#msgAlert').css("display", "block");
                    $('#msgAlert').html('Please add emails');
                    setTimeout(function () {
                        $("#msgAlert").css("display", "none");
                    }, 4000);
                    statFlag = false;
                }
            }
            return statFlag;
        }
        function onEnterKeyPress(event) {
            if (event.keyCode === 13) { 
                event.preventDefault(); 
               document.getElementById('<%= btnSrchEmail.ClientID %>').click(); 
        }
    }
        function uncheckEmailSelection() {
            var RB1 = document.getElementById("<%=dlEmailforSorting.ClientID%>");
            var radio = RB1.getElementsByTagName("input");
            for (var li = 0; li < radio.length; li++) {
                var email = radio[li + 1];//.valueOf("hdn_EmailValue");
                //alert(email.defaultValue);
                radio[li].checked = false;
            }

            var selectBox = document.getElementById("<%= lstSelecteUserNames.ClientID%>");
            selectBox.innerHTML = "";

            var selectBox2 = document.getElementById("<%= lstSelectdEmails.ClientID%>");
            selectBox2.innerHTML = "";
        }

        function loadEmailOrder(clicked_id) {
            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('slow', function () {
                $('#popupEmailMain').fadeIn();
                document.getElementById('OptMessage').value = "";
            });
            if (clicked_id == 'BtnEmail') {
                //uncheckEmailSelection();
                $('#<%=hdn_statusFlag.ClientID %>').val('true');
                $('#BtnEmailApproveSend').hide();
                $('#BtnEmailSend').show();
                $('#BtnSkip').hide();
            } else {
                $('#<%=hdn_statusFlag.ClientID %>').val('false');
                $('#BtnEmailSend').hide();
                $('#BtnEmailApproveSend').show();
                $('#BtnSkip').show();
            }
        }

        function loadEmailOrder2(statusflag) {
            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('fast', function () {
                $('#popupEmailMain').fadeIn();                
                //document.getElementById('OptMessage').value = "";
            });
            if (statusflag == 'true') {                
                $('#BtnEmailApproveSend').hide();
                $('#BtnEmailSend').show();
                $('#BtnSkip').hide();
            } else {
                $('#BtnEmailSend').hide();
                $('#BtnEmailApproveSend').show();
                $('#BtnSkip').show();
            }
        }

        function LoadAdminTemplateView(flag) {
            var entryContent = $('.navigationTabs').find('li').find('a');
            if (flag == 0) {

                
                var entryContent1 = $('.navigationTabs').find('li').find('a').eq(0);

                entryContent.hide();
                //alert(entryContent.length)
                entryContent1.show();
            }
            else {
                
                entryContent.show();
                //alert($entryContent.find('a').length);
            }
        }


        function EscapeField() {
            document.getElementById("txtTeacherDo").value = chkTextCustomize
            escape(document.getElementById("txtTeacherDo").value);
        }
        function ModificationExit() {
            alert('Modification defined for this lesson plan...');
        }

        function popPrompts() {

            $(".fullOverlay").show();
            $(".web_dialog2").show();

        }
        function HidePopup() {
            //Clearfile();
            Clear();
            document.getElementById("divMessage").innerHTML = "";
            $(".fullOverlay").hide();
            $(".web_dialog2").hide();
            $("#<%=divMessage.ClientID%>").hide();
        }

        function deleteDoc(btn) {
            if (!multiClickDelete) {
                var flag;
                flag = confirm("Are you sure you want to delete this document?");
                if (flag) {
                    multiClickDelete = true;
                    btn.style.opacity = '0.5';
                }
                return flag;
            }
            return false;
        }

       

        function hideRejectedNotes() {
            $('#ViewRejectedNotes').fadeOut('slow', function () {
                $('#overlay').fadeOut('fast');
            });
        }

        function ShowRejectedNote() {
            $('#overlay').fadeIn('fast', function () {
                $('#ViewRejectedNotes').css('top', '5%');
                $('#ViewRejectedNotes').show();
            });
        }
        function checkNull(btn) {
            if (!multiClickCriteria) {
                var text = document.getElementById("txtApLPReason").value;
                if (text == "") {
                    document.getElementById("NewLPValidMsg").style.visibility = "visible";
                    return false;
                }
                else {
                    document.getElementById("NewLPValidMsg").style.visibility = "hidden";
                    multiClickCriteria = true;
                    btn.style.opacity = '0.5';
                    return true;
                }
            }
            return false;
        }

        function checkForIIG(elm) {

            //var elmId = $(elm).find("input").attr('id');
            //var elmChk = $(elm).find("input").attr('checked');
            //var elmIIGId = elmId + "IIG";
            //if (elmChk == "checked") {
            //    //document.getElementById(elmIIGId).disabled = false;
            //    //$('#'+elmIIGId).prop("disabled", false);
            //    //alert($('#' + elmIIGId).length);

            //    //$(elmIIGId).removeAttr("disabled");
            //    //$('#' + elmIIGId).removeAttr("disabled");


            //} else {
            //    document.getElementById(elmIIGId).checked = false;
            //    //document.getElementById(elmIIGId).disabled = true;
            //    $('#'+elmIIGId).prop("disabled", true);
            //}

            var elmId = $(elm).find("input").attr('id');
            var elmChk = $(elm).find("input").attr('checked');
            var elmIIGId = elmId + "IIG";
            if (elmChk == "checked") {
                document.getElementById(elmIIGId).checked = true;
            }
            else {
                document.getElementById(elmIIGId).checked = false;
            }
        }

        function loadLPOrder() {
            $('#popupMain').fadeIn();
        }

        function goUp(sortId) {
            var thisContainer = $(sortId).parents('.LPBox');
            var clkSortId = $(thisContainer).find('.lblSort').val();
            var nextSortId = parseInt(clkSortId) - 1;
            if (nextSortId > 0) {
                var allTxtBoxes = $("#dlLPforSorting").find(".lblSort");
                for (var i = 0; i < allTxtBoxes.length; i++) {
                    if ($(allTxtBoxes[i]).val() == nextSortId) {
                        $(allTxtBoxes[i]).val(clkSortId);
                        break;
                    }
                }
                $(thisContainer).find('.lblSort').val(nextSortId);
                var $divs = $("div.LPBox").parent();
                var numericallyOrderedDivs = $divs.sort(function (a, b) {
                    return parseInt($(a).find(".lblSort").val()) - parseInt($(b).find(".lblSort").val());
                });
                $("#dlLPforSorting").html(numericallyOrderedDivs);
            }
        }

        function goDown(sortId) {
            var thisContainer = $(sortId).parents('.LPBox');
            var clkSortId = $(thisContainer).find('.lblSort').val();
            var nextSortId = parseInt(clkSortId) + 1;
            var allTxtBoxes = $("#dlLPforSorting").find(".lblSort");
            if (nextSortId <= allTxtBoxes.length) {
                for (var i = 0; i < allTxtBoxes.length; i++) {
                    if ($(allTxtBoxes[i]).val() == nextSortId) {
                        $(allTxtBoxes[i]).val(clkSortId);
                        break;
                    }
                }
                $(thisContainer).find('.lblSort').val(nextSortId);
                var $divs = $("div.LPBox").parent();
                var numericallyOrderedDivs = $divs.sort(function (a, b) {
                    return parseInt($(a).find(".lblSort").val()) - parseInt($(b).find(".lblSort").val());
                });
                $("#dlLPforSorting").html(numericallyOrderedDivs);
            }
        }
    </script>

    <script type="text/javascript">

        var setIntervalId = 0;
        var timeStart = "";
        var timeStop = "";

        //function adjustStyle(width) {
        //    width = parseInt(width);

        //    if (width >= 988) {
        //        $("#sized").attr("href", "CSS/CustomizeTemplate.css");
        //        return;
        //    }
        //    if (width < 988) {
        //        $("#sized").attr("href", "CSS/CustomizeTemplateTab.css");
        //        return;
        //    }
        //}

        //$(function () {
        //    adjustStyle($(this).width());
        //    $(window).resize(function () {
        //        adjustStyle($(this).width());
        //    });
        //});

        function timerStart() {
            if (setIntervalId == 0) {
                timeStart = setInterval(function () { copyPopup() }, 1000);
                setIntervalId = 1;
            }
        }

        function copyPopup() {

            $('#popupDiv').fadeIn('slow');

            timeStop = setInterval(function () { closeCopyPopup() }, 4000);


        }

        function closeCopyPopup() {

            $('#popupDiv').fadeOut('slow');
            clearInterval(timeStart);
            clearInterval(timeStop);

        }


        function isNumeric(keyCode) {
            var r = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8)
            if (!r) {
                alert("Invalid number");
            }
            return r;
        }

        function PopupReject() {
            $('#overlay').fadeIn('fast', function () {
                $('#DilogAprove').css('top', '5%');
                $('#DilogAprove').show();
            });

        }
        function closeReject() {
            $('#DilogAprove').hide();
            $('#overlay').fadeOut('slow');
        }

        function ExportTemplatePopup() {
                $('#LBLClassnotfound').text('');
                $('#txtSname').val('Individual Name');
                $('#<%=btnCopyTempAdmin.ClientID%>').hide();
                $('#divCopyTempAdmin').hide();

                $('#HdrExportTemplate').fadeIn('slow');
                //$('#HdrExportTemplate').css('display', 'block');
                $('#classDivs').empty();
                $('#chkCpyStdtTemplate').attr('checked', false);
                $('#tdMsgExprt').empty();
                TempSelected();
                $('#<%=txtSname.ClientID %>').show();
                $('#<%=imgsearch.ClientID %>').show();
        }

        function closePOP() {

            $('#<%=txtSname.ClientID %>').val("");
            $('#HdrExportTemplate').fadeOut('slow');
            $('#tdMsgExprt').empty();
            $('#<%=hfSelectedStudent.ClientID%>').val('');
            //$(this).closest('tr').hide().find('class:cpyTemp').html
            $('.studentDiv').show();

        }
        function studclsSearch() {
            $('.studentDiv').hide();
            $('#<%=txtSname.ClientID %>').val("");
            $('#<%=btnCopyTempAdmin.ClientID %>').show();
            $('#<%=divCopyTempAdmin.ClientID %>').show();
            var Lesson = $("#hdDefaultName").val();
            $("#txtCopyTempAdmin").val(Lesson);
        }
        function selectStudentId(studentId) {
            $('#' + studentId).css('background', 'rgba(0, 0, 0, 0) url("images/dwngrenbg.png") no-repeat scroll 0 0');
            $('#' + studentId).siblings().css('background', 'rgba(0, 0, 0, 0) url("images/lbtngrngray.png") no-repeat scroll 0 0');
            var hfstud = document.getElementById('<%=hfSelectedStudent.ClientID%>');
            hfstud.value = studentId;
            if (hfstud.value != null) {
                $("#<%=btnCopyTempAdmin.ClientID%>").show();
                $('#divCopyTempAdmin').show();
                var Lesson = $("#hdDefaultName").val();
                $("#txtCopyTempAdmin").val(Lesson);
            }
            else {
                $("#<%=btnCopyTempAdmin.ClientID%>").hide();
                $('#divCopyTempAdmin').hide();
                var Lesson = $("#hdDefaultName").val();
                $("#txtCopyTempAdmin").val(Lesson);
            }

        }
        function AlertCopySelectMsg() {
            //alert('Data Updated Successfully...');
            alertMessage('Please select a student...', 'red');
        }

        function hideApprRej() {
            $('#DilogAprove').fadeOut('slow', function () {
                $('#overlay').fadeOut('fast');
            });
        }

        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 0);
        }

        function CheckMatchtoSample() {
            var txt = document.getElementById("txtMatcSamples").value;            
            var sel = document.getElementById("lstMatchSamples");
            var listLength = sel.options.length;
            //alert(listLength);

            for (var i = 0; i < listLength; i++) {
                if (sel.options[i].text.toLowerCase() == txt.toLowerCase()) {
                    document.getElementById('lbl_txt').innerHTML = 'Already Exists.';
                    return false;
                }
            }

        }
    </script>



    <style type="text/css">
        .lessonUpload {
            float: left !important;
            margin: 8px 10px 0 0;
        }

        .spnClass {
        }

        .btnnwstyil {
            background: url(../Administration/images/submit.PNG) left top;
            font-size: 0;
            width: 91px !important;
            height: 26px !important;
            padding: 0;
            margin: 0 6px 0 0 !important;
            display: block;
            float: right;
            border: none;
            cursor: pointer;
        }

        #DatasheetContainerDiv {
            background-color: #fff;
            padding: 5px;
            position: absolute;
            top: 10px;
            background-color: White;
            display: none;
            margin: auto;
            z-index: 10000;
            width: 88%;
            min-height: 500px;
            height: auto !important;
        }

        .btnnwstyil:hover {
            background-position: 0 -31px !important;
            background-color: none;
        }


        .btnnwstyilst {
            background: url(../Administration/images/masterbtnbg.png) left top;
            font-size: 0;
            width: 91px !important;
            height: 26px !important;
            padding: 0;
            margin: 0 6px 0 0 !important;
            display: block;
            float: right;
            border: none;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: bold;
            color: #fff;
            cursor: pointer;
        }

        .tableMeasure {
            width: 100%;
        }

            .tableMeasure td {
                border-bottom: 2px double lightblue;
                height: 40px;
            }

        .Nobrdr {
            border-right: 1px double silver;
        }
        /*-------------------------------------------search---------------------------------------*/
        .imgBtn {
            width: 27px;
            height: 28px;
            float: left;
            background: url(images/zoomlens.png) left top no-repeat;
            cursor: pointer;
            background-position: 0 -0;
            float: right !important;
            border: none;
            margin: -1px 0 0 0;
        }

            .imgBtn:hover {
                background-position: 0 -31px;
            }

        /*-------------------------------------------Edit----------------------------------------*/
        #idLessonType {
            height: 30px;
            width: 160px;
            text-align: left;
            text-transform: none;
        }

            #idLessonType span.icon {
                font-family: Arial, Helvetica, sans-serif;
                font-size: 12px;
                font-weight: bold;
                color: #0d668e;
                background: url(img/txedticon.png) left top no-repeat;
                background-position: 0 -21px;
                text-transform: none;
                letter-spacing: 1px;
                width: 105px;
                height: 20px;
                display: block;
                padding: 0 0 0 23px;
                text-align: left;
                cursor: pointer;
                margin: 20px 0 0 0;
            }

                #idLessonType span.icon:hover {
                    background-position: 0 -0;
                    color: #666;
                }



        /*------------------------EDIT----------------------------------*/
        .tooltip {
            position: relative;
            padding: 1px 20px 5px 0;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: bold;
            top: -20px;
            color: #f00;
            width: 70px;
            height: 25px;
            margin: 0;
        }

        div.tooltip1 span.cs {
            position: absolute;
            width: 400px;
            height: 25px;
            padding: 1px 0;
            font: 12px Arial, Helvetica, sans-serif;
            display: none;
            left: 130px;
            top: 12px;
            bottom: 0;
            color: #FFF;
            display: none;
        }

        div.tooltip1:hover span.cs {
            display: block;
        }

        #idOption {
            height: 26px;
            position: absolute;
            width: auto;
            z-index: 1009;
            display: block;
            background: #60c658;
            padding: 3px 8px;
            border-radius: 3px;
        }

        input.XCNFButton {
            background-color: transparent;
            height: 20px;
            color: #000;
            padding: 0 10px;
            margin-bottom: 8px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 10px;
            font-weight: normal;
            text-decoration: none;
            border: none;
            cursor: pointer;
            float: right;
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
        }

            input.XCNFButton:hover {
                background: #0d668e !important;
                color: #fff;
            }

        #popupDiv {
            background-color: #ada3a3;
            display: none;
            height: 26px;
            margin: auto;
            padding: 5px;
            position: absolute;
            top: 10px;
            width: 355px;
            font-size: 18px;
            color: black;
            z-index: 10000;
            left: 400px;
            font-family: Arial;
        }

        .noneditbleTb {
            background-color: #ECE4EB;
        }

        .web_dialog2 {
            display: none;
            position: fixed;
            min-width: 290px;
            min-height: 325px;
            left: 21%;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 1001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
            width: 650px;
            top: 2%;
            z-index: 9999;
        }

        #HdrExportTemplate {
            display: none;
        }

        .searchbtnimg {
            border-width: 0px;
            margin: 0px 0px -7px;
        }

        .cpyTemp {
            background: rgba(0, 0, 0, 0) url(images/lbtngrngray.png) left top no-repeat;
            width: 204px;
            height: 21px;
            font-size: 11px;
            font-weight: normal;
            color: #0d668e;
            background-position: 0 -0;
            display: block;
            text-decoration: none;
            padding: 7px 0 0 19px;
            float: left;
            margin: 0 0 3px 0;
        }
        .loading2 {
            display: none;
            position: absolute;
            width: 100%;
            height: 900px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            background-image: url("images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
        }

        .loading {
            display: none;
            position: absolute;
            width: 100%;
            height: 800px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            background-image: url("images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
        }

        .innerLoading {
            margin: auto;
            height: 50px;
            width: 250px;
            text-align: center;
            font-weight: bold;
            font-size: large;
        }

            .innerLoading img {
                margin-top: 200px;
                height: 10px;
                width: 100px;
            }

        .LPBox {
            border: 1px solid black;
            margin-left: 5px;
            margin-right: 5px;
        }
    </style>

    <style>
        body {
            width: 100%;
            min-height: 500px;
            height: auto !important;
            height: 500px;
            margin: 0 auto;
            font-family: Arial, Helvetica, sans-serif;
        }

        .clear {
            margin: 0;
            padding: 0;
            border: 0;
            clear: both;
        }




        div.lBxpartContainer {
            width: 18%;
            min-height: 300px;
            height: auto !important;
            height: 300px;
            float: left;
            padding: 10px;
            background: #f7f5f5;
            border: 5px solid #e4e4e4;
            margin: 0 10px 0 0;
        }

            div.lBxpartContainer h3 {
                font-size: 12px;
                font-weight: bold;
                color: #0d668e;
                margin: 5px 0 5px 0;
            }

            div.lBxpartContainer input.radio {
                float: left !important;
                margin: 5px 0 0 0;
                padding: 0;
                width: 20px;
                height: 20px;
                display: block;
            }

            div.lBxpartContainer a.lpb {
                background: url(images/greenbtn.png) left top no-repeat;
                width: 176px;
                height: 22px;
                font-size: 12px;
                font-weight: normal;
                color: #444545;
                background-position: 0 -0;
                display: block;
                text-decoration: none;
                padding: 6px 0 0 25px;
                float: right;
                margin: 0 0 3px 0;
            }

                div.lBxpartContainer a.lpb span {
                    font-size: 11px;
                    font-weight: bold;
                }

            div.lBxpartContainer a.grb {
                background: url(images/graybtn.png) left top no-repeat;
                width: 176px;
                height: 23px;
                font-size: 12px;
                font-weight: normal;
                color: #444545;
                background-position: 0 -0;
                display: block;
                text-decoration: none;
                padding: 6px 0 0 25px;
                float: right;
                margin: 0 0 3px 0;
            }

            div.lBxpartContainer input.txtfld {
                border-radius: 3px;
                border: 1px solid #c8cfcf;
                margin: 0 0 0 44px;
                height: 24px;
                color: #666;
                font-size: 12px;
                width: 158px;
                padding: 0 3px;
            }

            div.lBxpartContainer input.btn {
                width: 27px;
                height: 28px;
                float: left;
                background: url(images/zoomlens.png) left top no-repeat;
                cursor: pointer;
                background-position: 0 -0;
                float: right !important;
                border: none;
                margin: -1px 0 0 0;
            }

                div.lBxpartContainer input.btn:hover {
                    background-position: 0 -31px;
                }

            div.lBxpartContainer p {
                font-size: 12px;
                color: #505050;
                margin: 5px 0 5px 42px;
            }

            div.lBxpartContainer input.smltxtfld {
                border-radius: 3px;
                border: 1px solid #c8cfcf;
                margin: 0 5px 0 44px;
                height: 24px;
                color: #666;
                font-size: 12px;
                width: 67px;
                padding: 0 3px;
                float: left;
            }

            div.lBxpartContainer input.smltxt {
                border-radius: 3px;
                border: 1px solid #c8cfcf;
                margin: 0 5px 0 5px;
                height: 24px;
                color: #666;
                font-size: 12px;
                width: 68px;
                padding: 0 3px;
                float: left;
            }

        div.mBxContainer {
            width: 18%;
            height: 530px;
            float: left;
            padding: 10px;
            background: #f7f5f5;
            border: 5px solid #e4e4e4;
            margin: 0 10px 0 0;
            overflow-y: auto;
            overflow-x: hidden;
        }

            div.mBxContainer h3 {
                font-size: 12px;
                font-weight: bold;
                color: #0d668e;
                margin: 5px 0 5px 0;
            }

            div.mBxContainer a.gmb {
                background: url(images/lbtngrn.png) left top no-repeat;
                width: 200px;
                height: 22px;
                font-size: 12px;
                font-weight: normal;
                color: #444545;
                background-position: 0 -0;
                display: block;
                text-decoration: none;
                padding: 7px 0 0 25px;
                float: left;
                margin: 0 0 3px 0;
            }

            div.mBxContainer a.grbmb {
                background: url(images/lbtngrngray.png) left top no-repeat;
                width: 200px;
                height: 22px;
                font-size: 12px;
                font-weight: normal;
                color: #444545;
                background-position: 0 -0;
                display: block;
                text-decoration: none;
                padding: 6px 0 0 25px;
                float: left;
                margin: 0 0 3px 0;
            }

            div.mBxContainer a.grmb {
                background: url(images/lbtngrngray.png) left top no-repeat;
                width: 200px;
                height: 22px;
                font-size: 11px;
                font-weight: normal;
                color: #0d668e;
                background-position: 0 -0;
                display: block;
                text-decoration: none;
                padding: 6px 0 0 25px;
                float: left;
                margin: 0 0 3px 0;
            }

            div.mBxContainer input.butn {
                width: 220px;
                height: 34px;
                background: url(images/blbtn.png) left top no-repeat;
                color: #fff;
                font-weight: bold;
                font-size: 11px;
                border: none;
                cursor: pointer;
            }

        div.righMainContainer {
            width: 55%;
            min-height: 300px;
            height: auto !important;
            height: 300px;
            float: right;
            padding: 10px;
            background: #fff;
            border: 5px solid #e4e4e4;
            margin: 0 8px 0 0;
        }

            div.righMainContainer h3 {
                font-size: 12px;
                font-weight: bold;
                color: #0d668e;
                margin: 5px 0 5px 0;
            }



        div.mBxContainer div.grmb {
            background: url(images/lbtngrngray.png) left top no-repeat;
            width: 200px;
            height: 22px;
            background-position: 0 -0;
            text-decoration: none;
            padding: 6px 0 0 25px;
            float: left;
            margin: 0 0 3px 0;
        }

            div.mBxContainer div.grmb a {
                margin: 0;
                padding: 0;
                text-decoration: none;
                font-size: 11px;
                font-weight: normal;
                color: #0d668e;
                display: block;
                float: left;
            }

            div.mBxContainer div.grmb div.grapgContainer {
                width: 50px;
                height: 18px;
                border: 1px solid #9ba1a1;
                border-radius: 3px;
                float: right;
                margin: -2px 5px 0 0;
                font-size: 9px;
                font-weight: normal;
                color: #0d668e;
            }

                div.mBxContainer div.grmb div.grapgContainer span {
                    width: 16px;
                    height: 16px;
                    margin: 0 0 0 3px;
                    float: left;
                    background: url(images/loder.png) left top no-repeat;
                    background-position: -21px 0;
                }

                div.mBxContainer div.grmb div.grapgContainer p {
                    margin: 2px 0 0 22px;
                    margin: 5px 0 0 22px\9;
                    hyphens: auto;
                }


        /*-----------------------------------------------------------JQuery Start-------------------------------------------*/

        h1, h2, h3, h4, h5, h6, p, ul, li {
            margin: 0;
            padding: 0;
            border: 0;
            vertical-align: baseline;
            list-style: none;
        }

            h2 a {
                padding: 7px 7px 7px 20px;
            }

            h3 a {
                padding: 7px 7px 7px 30px;
            }

            h2 a, h3, a {
                display: block;
                text-decoration: none;
            }

            .accordion li, li.accordion {
                border: none;
            }

        .accordion h2 a, .accordion h3 a {
            text-decoration: none;
            background: url(images/computer.png) 5px 6px no-repeat !important;
            z-index: 999;
            padding: 9px 0 2px 30px;
            font-size: 11px;
            font-weight: bold;
            color: #1f1f1f;
            width: 48%;
            float: left;
        }

            .accordion h2 a.kk, .accordion h3 a.kk {
                text-decoration: none;
                background: none !important;
                z-index: 999;
                padding: 2px 0 6px 0;
                font-size: 12px;
                font-weight: normal;
                color: #444545;
                width: 78%;
                float: left;
                border: none;
                text-align: left;
            }

        .accordion .wrapper {
            padding: 5px;
            line-height: 18px;
            width: 92%;
        }

        .accordion .nomar {
            padding: 0px;
            width: 100%;
        }

        .accordion h2,
        .accordion h3 {
            height: 31px;
            background: url(images/longgreenbar.png) right top no-repeat;
            font-size: 12px;
            font-weight: bold;
            color: #000;
            margin: 0 0 2px 0;
            padding: 0;
            width: 100%;
        }

            .accordion h2 span.dd,
            .accordion h3 span.dd {
                width: 4px;
                height: 31px;
                float: left;
                margin: 0;
                padding: 0;
                background: url(images/llbar.jpg) left top no-repeat;
                display: block;
            }

            .accordion h2.BG,
            .accordion h3.BG {
                height: 23px;
                background: url(images/dwngrenbg.png) left top no-repeat;
                width: 230px;
                font-size: 12px;
                font-weight: normal;
                color: #444545;
                margin: 0 0 3px 0;
                padding: 6px 0 0 25px;
                border: none;
                background-position: 0 -0;
            }

                .accordion h2.BG:hover,
                .accordion h3.BG:hover {
                    background-position: 0 -32px;
                    color: #000 !important;
                }

            .accordion h2.tBG,
            .accordion h3.tBG {
                height: 18px;
                background: none;
                width: 230px;
                margin: 0 0 3px 0;
                padding: 2px 0 0 5px;
                border: none;
                background-position: 0 -0;
            }

                .accordion h2.tBG:hover,
                .accordion h3.tBG:hover {
                    background-position: 0 -32px;
                    color: #000 !important;
                }

            .accordion h2 a.jj, .accordion h3 a.jj {
                text-decoration: none;
                background: url(images/updwnarrow.jpg) left 5px no-repeat !important;
                z-index: 999;
                padding: 2px 0 2px 25px;
                font-size: 12px;
                font-weight: bold;
                color: #0d668e;
                width: 65%;
                float: left;
                border: none;
                text-align: left;
            }

                .accordion h2 a.jj:hover, .accordion h3 a.jj:hover {
                    color: #666;
                }



            .accordion h2 div.container, .accordion h3 div.container {
                float: right;
                width: 35%;
                height: 20px;
                margin: 9px 0 0 0;
                padding: 0;
                background: none;
            }

                .accordion h2 div.container span, .accordion h3 div.container span {
                    float: left;
                    font-size: 12px;
                    font-weight: bold;
                    color: #1f1f1f;
                    display: block;
                    z-index: 999;
                    margin: 0 0 0 0;
                    width: 180px;
                }

                    .accordion h2 div.container span input.rdo, .accordion h3 div.container span input.rdo {
                        float: left !important;
                        display: block;
                        width: 10%;
                        height: 20px;
                        margin: -2px 10px 0 0;
                        margin: -6px 10px 0 0 \9;
                    }

                .accordion h2 div.container a.dlt, .accordion h3 div.container a.dlt {
                    float: right;
                    width: 20px;
                    height: 17px;
                    display: block;
                    margin: 0 15px 0 0;
                    padding: 0;
                    background: url(images/delet0_2.png) left top no-repeat !important;
                    background-position: 0 -0;
                    border-bottom: 0;
                }

                    .accordion h2 div.container a.dlt:hover, .accordion h3 div.container a.dlt:hover {
                        background-position: 0 -21px !important;
                    }


        .accordion .wrapper div.ingrContainer {
            border-radius: 5px;
            margin: 5px 0 0 0;
            min-height: 75px;
            height: auto !important;
            height: 75px;
            width: 104%;
            border: 1px solid #a1a1a1;
            background: #efefef;
            padding: 10px;
            font-size: 12px;
            font-weight: normal;
            color: #1f1f1f;
        }



        /*-----------------------------------------------------------JQuery End-------------------------------------------*/

        .topMessageBox_green {
            background-color: #cff6d6;
            border: 1px solid #1db23a;
            font-size: 15px;
            font-weight: bold;
            height: 20px;
            padding: 5px;
            position: fixed;
            top: 0;
        }

        .topMessageBox_red {
            background-color: #fbe0d9;
            border: 1px solid #e43d12;
            font-size: 15px;
            font-weight: bold;
            height: 20px;
            padding: 5px;
            position: fixed;
            top: 0;
        }

        .web_dialog22 {
            background: #f8f7fc url("../Administration/images/smalllgomlmark.JPG") no-repeat scroll right bottom;
            border: 5px solid #b2ccca;
            color: #333;
            display: none;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 100%;
            height: auto;
            left: 125px;
            margin: 15px 1% 1%;
            min-height: 200px;
            min-width: 630px;
            padding: 5px 5px 30px;
            position: fixed;
            top: 0;
            width: 80%;
            z-index: 1001;
        }
        fieldset {
        border:1px solid;
        text-align:left;
        float:left;
        }
        .mts {
        width:100%;
        }
            .mts tr td {
            width:100px;
        text-align:left;
            }
    </style>

    <script type="text/javascript">
        function AlertSuccessMsg() {
            alertMessage('Data Updated Successfully...', 'green');
        }

        function alertMessage(message, type) {
            var window_width = $(window).width();

            if (type == "green") {
                var box = $("<div class='topMessageBox_green'>" + message + "</div>");
                $('body').append(box);

                var boxWidth = $(box).width();
                var boxMargin = (window_width / 2) - (boxWidth / 2);

                $('.topMessageBox_green').css('margin-left', boxMargin);

                setTimeout(function () {
                    $(".topMessageBox_green").remove()
                }, 5000);
            }
            if (type == "red") {

                var box = $("<div class='topMessageBox_red'>" + message + "</div>");
                $('body').append(box);

                var boxWidth = $(box).width();
                var boxMargin = (window_width / 2) - (boxWidth / 2);

                $('.topMessageBox_red').css('margin-left', boxMargin);

                setTimeout(function () {
                    $(".topMessageBox_red").remove()
                }, 5000);
            }

        }
    </script>
    <script>
        function allowBackSpace(e, obj) {
            var evt = e || window.event;
            if (evt.keyCode == 8) {
                obj.value = '';
            }
        }

        function showPopup() {
            document.getElementById('formatchngloader').style.display='block';
            var popup = document.getElementById('frmtchngpopup');
            popup.style.display = 'block';
              
            
        }
        
    </script>

</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="210000"></asp:ScriptManager>
        <div style="text-align: right">
        </div>
        <div>

            <div class="boxContainerContainer">
                <table style="width: 100%;">
                    <tr>
                        <td id="tdReadMsg" runat="server"></td>
                    </tr>
                </table>
                <div class="clear"></div>
                <!-------------------------Top Container Start----------------------->
                <div class="itlepartContainer">

                    <table style="width: 100%;">
                        <tr>
                            <%-- <td style="width: 21%;">
                                <h2>1 <span>Choose Lesson Plan</span></h2>
                            </td>--%>
                            <td style="width: 15%; padding-left: 16px;">
                                <asp:DropDownCheckBoxes ID="ddlIepYear" runat="server" CssClass="ddchkLessonYear" UseButtons="false" UseSelectAllNode="false" Height="26px" Width="150px" AutoPostBack="true" 
                                     OnSelectedIndexChanged="ddlIepYear_SelectedIndexChanged">
                    <Texts SelectBoxCaption="IEP Start Year"/>
                </asp:DropDownCheckBoxes>
                                <asp:Button ID="Button_Search" runat="server" CssClass="btn btn-orange" Text="" />
                                <%--<div class="tooltip">
                                    <div class="tooltip1">

                                        <div id="idLessonType" class="" runat="server">
                                            <asp:Label ID="lblLessonType" CssClass="icon" runat="server" Text="LessonType"></asp:Label>
                                        </div>

                                        <span class="cs">
                                            <div id="idOption" class="clsselctCat" runat="server">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 25%;">

                                                            <asp:Button ID="btneditMode" runat="server" Text="Edit" CssClass="XCNFButton" OnClick="btneditMode_Click" />
                                                        </td>
                                                        <td style="width: 50%;">
                                                            <asp:Button ID="btnconvrtLessonPlan" runat="server" Text="Convert" CssClass="XCNFButton" OnClick="btnconvrtLessonPlan_Click" />
                                                        </td>
                                                        <td style="width: 25%;">
                                                            <asp:Button ID="btnreasignNewLsn" runat="server" Text="Re-Asign" CssClass="XCNFButton" OnClick="btnreasignNewLsn_Click" />
                                                        </td>

                                                    </tr>
                                                </table>
                                            </div>
                                        </span>
                                    </div>

                                </div>--%>

                            </td>
                            <td>
                                <h3 class="cf" style="margin:0 0 0 10px;float:none">

                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblCaptnLesson" Style="float:left;" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblcurrntLessonName" Style="float: left;" runat="server"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Panel runat="server" ID="panelButtons">
                                        <span style="float: right; margin: -20px 0 0 23px;" class="spnClass">
                                            <asp:Button ID="btnFromRejectDup" runat="server" CssClass="NFButton" Width="140px" Text="Move to Drafts" Style="margin: 8px 10px 0 0; float: left;" OnClick="drpTasklist_SelectedIndexChanged1" OnClientClick="alert('Cannot be moved to Drafts because another version is already available');" title="After reviewing the Rejected Note, staff choose Move to Drafts to start working on fixing it up." />
                                            <asp:Button ID="btnFromReject" runat="server" CssClass="NFButton" Width="140px" Text="Move to Drafts" Style="margin: 8px 10px 0 0; float: left;" OnClick="btnFromReject_Click" OnClientClick="return multiClickCheckReject(this);" title="After reviewing the Rejected Note, staff choose Move to Drafts to start working on fixing it up." />
                                            <%--<input type="button" id="BtnEmail" runat="server" class="NFButton" value="Email for review" Visible="false" Style="Width:110px;margin: 8px 10px 0 0; float: left;" title="Email review" onclick="loadEmailOrder();" />--%>
                                            <asp:Button ID="BtnEmail" runat="server" class="NFButton" Text="Email for review" Visible="false" Style="Width:110px;margin: 8px 10px 0 0; float: left;" title="Email review" OnClick="LoadEmail_Click" OnClientClick="return multiClickCheck(this);"/>
                                            <asp:Button ID="BtnPreview" runat="server" CssClass="NFButton" Text="Preview" Visible="false" Style="margin: 8px 10px 0 0; float: left;" title="This will give a simplified preview of the custom datasheet for this lesson. You must complete the mandatory fields in these tabs for it to proceed (don't forget IEP start/end dates and setting move criteria for all Steps/Sets/Prompts. You must set criteria for up and down even if you are using NA so the application knows your decision is intentional.  The application has automatically built this datasheet based on all the answers you've provided in these tabs and will change as you change the requirements." OnClick="BtnPreview_Click" OnClientClick="return multiClickCheck(this);" />
                                            <asp:Button ID="BtnSubmit" runat="server" CssClass="lessonUpload NFButton" Text="Submit"  title="After previewing the datasheet and reviewing all the fields, if you are complete you hit Submit. The lesson will move out of the 'Drafts' box and into the 'Pending Approval' box for an approver to review and approve/reject." OnClick="BtnSubmit_Click" OnClientClick="return autoSave(false)"/>
                                            <asp:Button ID="BtnApproval_hdn" runat="server" Text="Approve" Visible="false" CssClass="NFButton" Style="margin: 8px 10px 0 0; float: left;" OnClick="BtnApproval_Click" />
                                            <asp:Button ID="BtnApproval" runat="server" UseSubmitBehavior="false" Text="Approve" OnClientClick="return showOverride();" Visible="false" CssClass="NFButton" Style="margin: 8px 10px 0 0; float: left;" title="Approve moves the lesson to Approved and it is immediately available for teaching. If this is a new version, the old version immediately moves to Inactive and this new version takes its place. If you want to delay releasing the lesson until a certain date, do not hit Approve, just leave the lesson in Pending Approval until the date you want to release it."/>
                                            <asp:Button ID="BtnReject" runat="server" Text="Reject" CssClass="NFButton" Visible="false" Style="margin: 8px 10px 0 0; float: left;" OnClick="BtnReject_Click1"  title="Reject immediately moves the lesson to Rejected and unlocks it so staff can continue to work on it and resubmit it. You can leave Approval Notes on each page specifying the issues and you can also specify the issues on the Reason pop-up after hitting Reject. The user can see the Approval Notes and the Reason. "/>
                                            <asp:Button ID="BtnMaintenance" runat="server" Text="Maintenance" CssClass="NFButton" Visible="false" Style="margin: 8px 10px 0 0; float: left;" OnClick="BtnMaintenanace_Click" OnClientClick ="return multiClickCheckVersion(this);" title="Moves lesson to Maintenance. Lesson will still be available to be taught for maintenance; the button will be yellow instead of green on the datasheet page to indicate it's a maintenance program versus active lesson." />
                                            <asp:Button ID="BtnInactive" runat="server" Text="Inactive" CssClass="NFButton" Visible="false" Style="margin: 8px 10px 0 0; float: left;" OnClick="BtnInactive_Click" OnClientClick="return multiClickCheckActive(this);" title="Moves lesson to Inactive, removes lesson from default datasheet tab." />
                                            <asp:Button ID="BtnActive" runat="server" Text="Activate" CssClass="NFButton" Visible="false" Style="margin: 8px 10px 0 0; float: left;" OnClick="BtnActive_Click" OnClientClick="return multiClickCheckActive(this);" title="Moves the lesson back to Approved and active."/>
                                            <asp:Button ID="BtnMakeRegular" runat="server" Text="Make Active " CssClass="NFButton" Visible="false" Width="140px" Style="margin: 8px 10px 0 0; float: left;" OnClick="BtnMakeRegular_Click" OnClientClick="return multiClickCheckActive(this);" title="Moves it from Maintenance to Active (Approved) again."/>
                                            <asp:Button ID="btnrejectedNotes" runat="server" Text="Rejected Note" CssClass="NFButton" Visible="false" Style="margin: 8px 10px 0 0; float: left;" OnClick="btnrejectedNotes_Click"  title="Choose this first to read the rejected reason notes from the approver, then Move to Drafts to make the changes."/>
                                            <input type="button" id="btndoc" runat="server" value="Add Doc/Video" onclick="popPrompts()" class="lessonUpload NFButton" style="margin: 8px 10px 0 0; width:98px; float: left;" title="If you have additional files that you want to give the teacher access to when opening this datasheet, attach them here." />
                                            <asp:Button ID="BtnCopyTemplate" runat="server" Text="" CssClass="newDoc" OnClick="BtnCopyTemplate_Approve" OnClientClick="return multiClickCheck(this);" Visible="false" Style="float: right;" ToolTip="Create new version" />
                                            <asp:Button ID="BtnExportTemplate" runat="server" CssClass="lessonUpload NFButton" Enabled="True" OnClientClick="ExportTemplatePopup()" Text="Copy" Visible="False" style="margin: 8px 10px 0 0; float: left;" title="Copy lesson to either the Curriculum Databank by checking 'Copy Lesson as Template' or to another specific student. To copy to another student, type the name, click on the face button to search, then select the name to copy. The lesson will appear in the Drafts box of the student to which you copied it." />
                                            <asp:Button ID="btnDelLp" runat="server" Text="Delete" CssClass="NFButton" Visible="false" Style="margin: 8px 10px 0 0; float: left;" OnClientClick="javascript: return deleteDoc(this);" OnClick="btnDelLp_Click1" title="Deletes the lesson you are currently working on."/>
                                            <img src="images/sort.png" style="height: 25px; width: 25px; cursor: pointer; margin: 0 10px 3px 0;" onclick="loadLPOrder();" />
                                            <asp:ImageButton ID="btnRefresh" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" ToolTip="Refresh" title="Refresh." />



                                        </span>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="panelButtonsAdmin" Visible="false">
                                        <span style="float: right; margin: -20px 0 0 23px;" class="spnClass">
                                            <asp:Button ID="BtnAdminPreview" runat="server" CssClass="NFButton" Text="Preview" Visible="false" Style="margin: 8px 10px 0 0; float: left;" title="" OnClick="BtnAdminPreview_Click" />
                                            <input type="button" id="btnNew" runat="server" value="Add Doc/Video" onclick="popPrompts()" class="lessonUpload NFButton" style="display: none; margin: 8px 10px 0 0; float: left;width:100px! important" />
                                            <asp:Button ID="BtnAddNewLessonPlan" runat="server" Text="Add New" CssClass="NFButton" Style="margin: 8px 10px 0 0; float: left;" OnClick="BtnAddNewLessonPlan_Click" />
                                            <asp:Button ID="btnDeleteLPAdmin" runat="server" Text="Delete" CssClass="NFButton" Style="margin: 8px 10px 0 0; float: left;" OnClick="btnDeleteLPAdmin_Click" Visible="false" OnClientClick="javascript: return deleteDoc(this);" />
                                        </span>
                                    </asp:Panel>
                                </h3>
                            </td>
                        </tr>

                    </table>

                    <div id="popupDiv">
                        Visual lesson data copied successfully.                           

                    </div>




                    <%-- <h2>2 <span>Customize Lesson Plan</span></h2>--%>
                </div>
                <!-------------------------Top Container End----------------------->

                <!-------------------------Middle Container start----------------------->
                <div class="btContainerPart">
                    <!------------------------------------MContainer Start----------------------------------->

                    <div class="mBxContainer" id="mBxContainer" runat="server">

                        <div id="page">
                            <asp:Panel ID="panelContent" runat="server">
                                <div id="content" style="display: block">
                                    <ul>
                                        <li style="position: static;" class="accordion">
                                            <h2 class="BG"><a class="kk" href="#our-company">Lessons - Approved</a></h2>
                                            <div style="display: none;" class="wrapper nomar">
                                                <div class="nobdrrcontainer">
                                                    <asp:DataList ID="dlApprovedLessons" runat="server" OnItemDataBound="dlApprovedLessons_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkApprovedLessons" CssClass="grmb" runat="server" OnClick="lnkApprovedLessons_Click" Text='<%# Eval("Name") %>' ToolTip='<%# Eval("Name") %>' CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                        </li>
                                        <li style="position: static;" class="accordion">
                                            <h2 class="BG"><a class="kk" href="#our-company">Lessons - Pending Approval</a></h2>
                                            <div style="display: none;" class="wrapper nomar">
                                                <div class="nobdrrcontainer">
                                                    <asp:DataList ID="dlCompltdLessonPlans" runat="server" OnItemDataBound="dlCompltdLessonPlans_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="grmb" ID="lnkCompltdLessonPlan" runat="server" OnClick="lnkCompltdLessonPlan_Click" Text='<%# Eval("Name") %> ' CommandArgument='<%# Eval("ID") %>' ToolTip='<%# Eval("Name") %> '>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                        </li>
                                        <li style="position: static;" class="accordion">
                                            <h2 class="BG"><a class="kk" href="#our-company">Lessons - Drafts</a></h2>
                                            <div style="display: none;" class="wrapper nomar">
                                                <div class="nobdrrcontainer">
                                                    <asp:DataList ID="dlLP" runat="server" OnItemDataBound="dlLP_ItemDataBound">
                                                        <HeaderTemplate><div style="background-color:#80ccff;color:#0D668E;font-weight:bold;text-align:center;vertical-align:middle">Drafts</div></HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="grmb" ID="lnkLessonPlan" runat="server" OnClick="lnkLessonPlan_Click" Text='<%# Eval("Name") %> ' CommandArgument='<%# Eval("ID") %>' ToolTip='<%# Eval("Name") %> '>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <asp:DataList ID="dlLPMailed" runat="server" OnItemDataBound="dlLPMailedItemDataBound">
                                                        <HeaderTemplate><div style="background-color:#80ccff;color:#0D668E;font-weight:bold;text-align:center;vertical-align:middle">Drafts - emailed for review</div></HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="grmb" ID="lnkLessonPlanMail" runat="server" OnClick="lnkLessonPlan_Click" Text='<%# Eval("Name") %> ' CommandArgument='<%# Eval("ID") %>' ToolTip='<%# Eval("Name") %> '>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                        </li>
                                        <li style="position: static;" class="accordion">
                                            <h2 class="BG"><a class="kk" href="#our-company">Lessons - Rejected</a></h2>
                                            <div style="display: none;" class="wrapper nomar">
                                                <div class="nobdrrcontainer">
                                                    <asp:DataList ID="dlRejectedLp" runat="server" OnItemDataBound="dlRejectedLp_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="grmb" ID="lnkRejectedLp" runat="server" OnClick="lnkRejectedLp_Click" Text='<%# Eval("Name") %> ' CommandArgument='<%# Eval("ID") %>' ToolTip='<%# Eval("Name") %> '>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                        </li>


                                        <li style="position: static;" class="accordion">
                                            <h2 class="BG"><a class="kk" href="#our-company">Lessons - Maintenance</a></h2>
                                            <div style="display: none;" class="wrapper nomar">
                                                <div class="nobdrrcontainer">
                                                    <asp:DataList ID="dlMaintenanceLp" runat="server" OnItemDataBound="dlMaintenanceLp_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="grmb" ID="lnkMaintenanceLp" runat="server" OnClick="lnkMaintenanceLp_Click1" Text='<%# Eval("Name") %> ' CommandArgument='<%# Eval("ID") %>' ToolTip='<%# Eval("Name") %> '>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                        </li>


                                        <li style="position: static;" class="accordion">
                                            <h2 class="BG"><a class="kk" href="#our-company">Lessons - Inactive</a></h2>
                                            <div style="display: none;" class="wrapper nomar">
                                                <div class="nobdrrcontainer">


                                                    <asp:DataList ID="dlInactiveLp" runat="server" OnItemDataBound="dlInactiveLp_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="grmb" ID="lnkInactiveLp" runat="server" OnClick="lnkInactiveLp_Click" Text='<%# Eval("Name") %> ' CommandArgument='<%# Eval("ID") %>' ToolTip='<%# Eval("Name") %> '>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                        </li>


                                    </ul>
                                </div>
                            </asp:Panel>
                        </div>


                        <div class="clear"></div>
                        <asp:Panel ID="panelAdminTemp" runat="server" Visible="false">
                            <div id="mBxContainer1">


                                <h3>Lesson Plans </h3>

                                <asp:TextBox ID="txtSearcLesson" runat="server" Width="170px" MaxLength="20"></asp:TextBox>
                                <asp:Button ID="BtnSearchLesson" runat="server" CssClass="imgBtn" OnClick="BtnSearchLesson_Click" />

                                <br />
                                <br />
                                <div class="nobdrrcontainer">
                                    <%--      <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>--%>

                                    <asp:DataList ID="dlApprovedLessonsAdmin" runat="server" OnItemDataBound="dlApprovedLessonsAdmin_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkApprovedLessonsAdmin" CssClass="grmb" runat="server" OnClick="lnkApprovedLessonsAdmin_Click" Text='<%# Eval("Name") %>' ToolTip='<%# Eval("Name") %>' CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:DataList>
                                    <%--   </ContentTemplate>
                            </asp:UpdatePanel>--%>
                                </div>

                                <div class="clear"></div>
                            </div>
                        </asp:Panel>
                    </div>
                    <!------------------------------------MContainer End----------------------------------->

                    <!------------------------------------End Container Start----------------------------------->





                    <div class="righMainContainer large" id="MainDiv" runat="server" visible="true">
                        <div class="topper">
                            <ul class="navigationTabs">
                                <span class="bgb"></span>
                                <li><a href="#" rel="info" style="width: 70px !Important;" onclick="autoSave(true)" >Lesson Info</a></li>
                                <li><a href="#" rel="about" style="width: 100px !Important;" onclick="autoSave(true)">Type of Instruction</a></li>
                                <li><a href="#" rel="download" style="width: 85px !Important;" onclick="autoSave(true)">Measurement Systems</a></li>
                                <li><a href="#" rel="implement" style="width: 60px !Important;" onclick="autoSave(true)">Sets</a></li>
                                <li><a href="#" rel="implement" style="width: 60px;" onclick="autoSave(true)">Steps</a></li>
                                <li><a href="#" rel="download" style="width: 60px;" onclick="autoSave(true)">Prompts</a></li>
                                <li class="nobg"><a href="#" rel="implement" style="width: 110px !Important;" onclick="autoSave(true)">Lesson procedure</a></li>
                            </ul>

                            <div class="tabsContent">


                                <div class="tab">

                                    <asp:UpdatePanel ID="UpdatePanel44" runat="server">
                                        <ContentTemplate>


                                            <div class="clear"></div>

                                            
                                            <div class="itomesContainer">

                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>

                                                    <tr>
                                                        <td title="This is the goal area selected for this lesson, the overarching category this lesson falls under." >Goals
                                                        </td>
                                                        <panel id="panelGoal" runat="server" visible="true">
                                                        <td style="width: 150px">
                                                            <asp:Label ID="lblGoalName" runat="server"></asp:Label>
                                                        </td>
                                                       
                                                       </panel>
                                                        <panel id="panelGoalAdmin" runat="server" visible="false">
                                                            <td  >
                                                        <div style=" max-height: 80px; overflow: auto; text-align: left;" class="checkedlist">
                                                            <asp:CheckBoxList ID="chkgoal"  runat="server" AutoPostBack="False" BorderStyle="None" CellPadding="0" CellSpacing="0" EnableTheming="True" RepeatColumns="2">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                                 
                                                    </td>
                                                        </panel>
                                                        <td title=" What are the goals for this lesson from the IEP/ISP? What specific milestones need to be met? Optional">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Lesson Plan Goal
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtLessonPlanGoal" runat="server" MaxLength="300" Width="260px" TextMode="MultiLine" Rows="3" Columns="5"></asp:TextBox><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span> What are the goals for this lesson from the IEP/ISP? What specific milestones need to be met? Optional</span></div>                                                           
                                                        </td>
                                                    </tr>



                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>

                                                    <tr>
                                                        <td title="Title of this Lesson Plan. If you change the title it will change throughout the program">Lesson Plan</td>
                                                        <td>
                                                           <asp:TextBox ID="txtLessonName" runat="server" MaxLength="300" Width="260px"></asp:TextBox><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div> <div class="tooltiptext"><span> Title of this Lesson Plan. If you change the title it will change throughout the program</span></div></td>
                                                        <td title="If required, the framework and strand from your state regulations. Optional.">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Framework and Strand</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFramework" runat="server" MaxLength="300" Width="260px" TextMode="MultiLine" Rows="5" Columns="5"></asp:TextBox><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>If required, the framework and strand from your state regulations. Optional.</span></div></td>
                                                    </tr>
                                                    <tr>
                                                        <td title="If you know how many times you will require staff to run this lesson each day, put it here. Otherwise leave it at zero and you will run it as often as you want or per the block schedule (elsewhere in the program).">Expected number of times
                                                            <br />
                                                            to run the lesson  
                                                        </td>
                                                        <td>
                                                            <%--<asp:TextBox ID="txtNoofTimesTried" runat="server" MaxLength="300" Width="260px" onkeypress="return event.charCode >= 48 && event.charCode <= 57"></asp:TextBox><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"> <span>If you know how many times you will require staff to run this lesson each day, put it here. Otherwise leave it at zero and you will run it as often as you want or per the block schedule (elsewhere in the program).</span></div></td>--%>
                                                            <asp:TextBox ID="txtNoofTimesTried" runat="server" MaxLength="300" Width="30px" onkeypress="return event.charCode >= 48 && event.charCode <= 57"></asp:TextBox> per <asp:DropDownList ID="noofTimesTriedPer" runat="server" AutoPostBack="true" CssClass="drpClass" Style="width: 200px;" OnSelectedIndexChanged="drpTasklist_SelectedIndexChanged1">
                                                                                <asp:ListItem Value="0">choose Day, Week, or Month----</asp:ListItem>
                                                                                <asp:ListItem>Day</asp:ListItem>
                                                                                <asp:ListItem>Week</asp:ListItem>
                                                                                <asp:ListItem>Month</asp:ListItem>
                                                                            </asp:DropDownList><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"> <span>If you know how many times you will require staff to run this lesson each day, put it here. Otherwise leave it at zero and you will run it as often as you want or per the block schedule (elsewhere in the program).</span></div></td>
                                                         </td>
                                
                                                        <td>
                                                        </td>
                                                        <td>
                                                          </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                           IEP/ISP/LP Start Date * 
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="lessonSDate" runat="server" class="textClass"  MaxLength="300" Width="260px" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" ></asp:TextBox></td>
                                                        <td>
                                                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; IEP/ISP/LP End Date *   
                                                        </td>
                                                         <td>
                                                            <asp:TextBox ID="lessonEDate" runat="server" class="textClass"  MaxLength="300" Width="260px" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" ></asp:TextBox></td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>

                                                    <tr>
                                                        <td title="As dictated and if required by state regulations. Optional.">Specific Standard</td>
                                                        <td>
                                                            <asp:TextBox ID="txtSpecStandrd" runat="server" Width="260px" TextMode="MultiLine" Rows="5" Columns="5"></asp:TextBox><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>As dictated and if required by state regulations. Optional.</span></div></td>
                                                        <td title="As dictated and if required by state regulations. Optional.">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Specific Entry Point</td>
                                                        <td>
                                                            <asp:TextBox ID="txtSpecEntrypoint" runat="server" Width="260px" TextMode="MultiLine" Rows="5" Columns="5"></asp:TextBox><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>As dictated and if required by state regulations. Optional.</span></div></td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>

                                                    <tr>
                                                        <td title="What foundational skills are required before being able to start this lesson plan? Optional.">Pre-requisite Skills</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPreSkills" runat="server" Width="260px" TextMode="MultiLine" Rows="5" Columns="5"></asp:TextBox><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What foundational skills are required before being able to start this lesson plan? Optional.</span></div></td>
                                                        <td title="What materials are required to run this program (pens, paper, fork, etc.)? Optional">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Materials
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMaterials" runat="server" Width="260px" TextMode="MultiLine" Rows="5" Columns="5"></asp:TextBox><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What materials are required to run this program (pens, paper, fork, etc.)? Optional.</span></div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="cbx_deletesess" runat="server" Text="Delete Session if not submitted by 2AM" ToolTip="Often people pull a datasheet for a lesson but don't Submit it by the end of their shift. When this option is selected, any incomplete datasheets for this lesson will be deleted at 2AM so the next shift doesn’t have to contend with mistrialing or otherwise cleaning up unfinished datasheets from the day before."/><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Often people pull a datasheet for a lesson but don't Submit it by the end of their shift. When this option is selected, any incomplete datasheets for this lesson will be deleted at 2AM so the next shift doesn’t have to contend with mistrialing or otherwise cleaning up unfinished datasheets from the day before.</span></div>
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td>Baseline Procedures</td>
                                                        <td>
                                                            <asp:TextBox ID="txtBaseline" runat="server" ReadOnly="true" CssClass="noneditbleTb" Width="260px" TextMode="MultiLine" Rows="5" Columns="5"></asp:TextBox></td>
                                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Goal objective
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtobjective" ReadOnly="true" runat="server" CssClass="noneditbleTb" Width="260px" TextMode="MultiLine" Rows="5" Columns="5"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>--%>

                                                    <tr>
                                                        <td title="This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter.">
                                                            <asp:Label ID="lblCommentLessonInfo" runat="server" Text="Approval Note" Visible="false"></asp:Label></td>
                                                        <td colspan="3">
                                                           <asp:TextBox ID="txtCommentLessonInfo" runat="server" Width="90.5%" TextMode="MultiLine" Rows="5" Columns="5" Visible="false"></asp:TextBox><div class="toolTipImg" id="LessoninfoApptooltip" runat="server" visible="false"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div>
                                                             <div class="tooltiptext" style="margin-left:400px" ><span>This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter.</span></div></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" style="text-align: right">
                                                            <asp:Button ID="btnCommentLessonInfo" CssClass="bTnn" runat="server" Text="Update Approval Note" Style="margin-right: 20px; width: 140px" OnClick="btnCommentLessonInfo_Click" Visible="false" OnClientClick="scrollToTop();"/>
                                                            <asp:Button ID="HiddenAppr" runat="server" OnClick="btnCommentLessonInfo_Click" style="visibility: hidden; display: none;" />


                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td style="text-align: right;">


                                                            <asp:Button ID="BtnUpdateLessonPlan" CssClass="bTnn" runat="server" Text="Update" Style="margin-right: 20px" OnClick="BtnUpdateLessonPlan_Click" OnClientClick="scrollToTop();" ToolTip="Press to Save your work on this tab" />


                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td>* Mandatory Fields</td>
                                                        <td></td>
                                                    </tr>

                                                </table>

                                            </div>

                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblloadAlert" runat="server" CssClass="tdText" Style="color: #0D668E" Text="Please select any Lesson Plan to start editing template"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>

                                            <br />


                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>


                                <div class="tab">
                                    <h2>Type Of Instruction *</h2>
                                    <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                        <ContentTemplate>

                                            <div class="itomesContainer">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            
                                                            <fieldset title="Top-level categorization of teaching method. Choose if you want to teach this as a Task Analysis, Discrete Trial, or an Incidental Program (which can be TA or Discrete Trial). Each of these categories results in a different datasheet design and different options for the next box (Teaching Method), so it's important to choose the correct one.">
                                                                <legend>
                                                                    Teaching Format
                                                                </legend>
                                                                <asp:DropDownList Width="150px" CssClass="drpClass" ID="drp_teachingFormat" runat="server" OnSelectedIndexChanged="drp_teachingFormat_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            </fieldset>
                                                            <fieldset title="Choose the ABA teaching method you will use for this lesson. The options show in this drop-down are filtered based on your previous selection of Teaching Method. Your answer here is not just a label… it automatically dictates how the lesson will be run, what the datasheet will look like, and how the lesson will be scored because the program knows how to run and score each teaching method.">
                                                                <legend>
                                                                    Teaching Method
                                                                </legend>
                                                                 <asp:DropDownList Width="150px" CssClass="drpClass" ID="drpTeachingProc" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpTeachingProc_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            </fieldset>
                                                            <fieldset>
                                                                <legend>Options</legend>
                                                                <asp:Panel runat="server" CssClass="PanelDiv" ID="pnl_discrete" Enabled="false">
                                                                <fieldset title="Type the number of trials you will run to equal one discrete trail teaching session. This box is only available if you selected a discrete trial method.">
                                                                    <legend>
                                                                        Discrete
                                                                    </legend>
                                                                    <asp:CheckBox ID="chkDiscrete" runat="server" Text="Discrete" AutoPostBack="True" OnCheckedChanged="chkDiscrete_CheckedChanged" style="display:none;" />
                                                                    <div id="showtrailid" runat="server">
                                                                            No of Trials<br />
            <asp:TextBox ID="txtNoofTrail" onkeypress="return isNumber(event)" onpaste="return false" runat="server" Width="50px" MaxLength="2"></asp:TextBox>
                                                                        </div>
                                                                </fieldset>
                                                                </asp:Panel>
                                                            <asp:Panel runat="server" CssClass="PanelDiv" ID="pnl_taskAnalysis">
                                                                <fieldset title="A task analysis is a list of steps. This option presents those steps scrambled in a new random order each time the lesson is run. This option is rarely used. This option is only available for Task Analysis lessons.">
                                                                    <legend>
                                                                        Task Analysis
                                                                    </legend>
                                                                    <asp:CheckBox ID="chkTotalRandom" runat="server" RepeatDirection="Horizontal" Text="Randomized" Checked="false"></asp:CheckBox>
                                                                      <asp:DropDownList ID="drpTasklist" runat="server" AutoPostBack="true" CssClass="drpClass" Style="width: 150px;display:none" OnSelectedIndexChanged="drpTasklist_SelectedIndexChanged1">
                                                                                <asp:ListItem Value="0">---- Select ----</asp:ListItem>
                                                                                <asp:ListItem>Forward chain</asp:ListItem>
                                                                                <asp:ListItem>Backward chain</asp:ListItem>
                                                                                <asp:ListItem>Total Task</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                    <div id="promptLevel" runat="server">
                                                                            <asp:RadioButtonList ID="rbtnPromptLevel" runat="server" RepeatDirection="Vertical" Style="display:none;">
                                                                                <%--<asp:ListItem Value="1" Selected="True">Step-by-Step</asp:ListItem>--%>
                                                                                <asp:ListItem Value="0" Selected="True">Total Task</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </div>
                                                                </fieldset>
                                                                </asp:Panel>
                                                            <asp:Panel runat="server" CssClass="PanelDiv" ID="pnl_matchToSample">
                                                                <fieldset>
                                                                    <legend>
                                                                        Match - to - Sample
                                                                    </legend>
                                                                     <asp:RadioButtonList ID="rbtnMatchToSampleExpressive" runat="server" CssClass="mts" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnMatchToSampleExpressive_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Value="0" Selected="True" title="This option chooses a 'Receptive' datasheet design that visually arranges your sets of teaching targets on the screen, creating both a guide for the teacher to follow when placing the targets on the desk and the buttons to record which target the client chose, correct or incorrect and keeps score automatically.">Receptive</asp:ListItem> 
                                                                <asp:ListItem Value="1" title="This option chooses an 'Expressive' datasheet design, which offers a basic discrete trial training design for lessons that do not require specific targets to be arranged for the client.">Expressive</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                                  <hr style="margin:0px;"/>
                                                                     <asp:RadioButtonList ID="rdoRandomMoveover" runat="server"  CssClass="mts" RepeatDirection="Horizontal" AutoPostBack="false">
                                                                <asp:ListItem Selected="True" Value="1" title="This option specifies that for match-to-sample programs, the targets will be truly randomized each trial.">Randomized</asp:ListItem>
                                                                <asp:ListItem Value="0" title="This option specifies that for match-to-sample programs, the targets will be moved one space to the right from trial to trial instead of completely randomized. Before this program, it was difficult to truly randomize your samples, but since the program takes care of it for you, this move-over method may be used less.">Move Over</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                                </fieldset>
                                                                </asp:Panel>
                                                                <asp:Panel runat="server" CssClass="PanelDiv" ID="pnl_selTeachProc" Visible="false">
                                                                    <asp:Label ID="lbl_selteacProc" Text="Select Teaching Method" runat="server"></asp:Label>
                                                                </asp:Panel>
                                                            </fieldset>
                                                        </td>
                                                    </tr>

                                                    
                                                    <tr>
                                                        <td>
                                                           
                                                            
                                                           
                                                        </td>
                                                        <td>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                       
                                                                    </td>
                                                                    <td>
                                                                        <div id="taskAnalysis" runat="server">
                                                                          
                                                                        </div>
                                                                        
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblPromptLevel" runat="server" Font-Bold="true" Visible="false">Prompt Level</asp:Label>
                                                                        
                                                                    </td>
                                                                </tr>

                                                            </table>






                                                            <%--    <asp:RadioButtonList ID="rdolistDatasheet" runat="server"
                                                                RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdolistDatasheet_SelectedIndexChanged">
                                                                <asp:ListItem Selected="True" Value="1">Task Analysis</asp:ListItem>
                                                                <asp:ListItem Value="0">Discrete Trial</asp:ListItem>
                                                            </asp:RadioButtonList>--%>



                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                           
                                                        </td>

                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <%-- <label>Specific Entry Point</label>--%></td>
                                                        <td></td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%-- <asp:TextBox ID="txtspecEntrypoint" runat="server" MaxLength="100"></asp:TextBox>--%></td>
                                                        <td></td>

                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>

                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <label title="Where will this be taught generally? Optional.">Major Setting :</label><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Where will this be taught generally? Optional.</span></div> </td>
                                                        <td>
                                                            <label  title="Where will this be taught specifically? Optional.">Minor Setting :</label><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Where will this be taught generally? Optional.</span></div> </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>

                                                    </tr>

                                                    <tr>
                                                        <td>

                                                            <asp:TextBox ID="txtmajorset" runat="server" TextMode="MultiLine" Width="93%" Rows="7" Columns="5"></asp:TextBox>
                                                        </td>
                                                        <td>

                                                            <asp:TextBox ID="txtminorset" runat="server" TextMode="MultiLine" Width="93%" Rows="7" Columns="5"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label  title="Describe what baseline work was done before arriving at this lesson plan.  Optional">Baseline Procedures :</label><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Describe what baseline work was done before arriving at this lesson plan.  Optional.</span></div> </td>
                                                        <td>
                                                            <label  title="What are the objectives of this program based on the IEP/ISP? Optional">Lesson Objectives :</label><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What are the objectives of this program based on the IEP/ISP? Optional.</span></div> </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>

                                                    </tr>

                                                    <tr>
                                                        <td>

                                                            <asp:TextBox ID="txtBaseline" runat="server" TextMode="MultiLine" Width="93%" Rows="7" Columns="5"></asp:TextBox>
                                                        </td>
                                                        <td>

                                                            <asp:TextBox ID="txtobjective" runat="server" TextMode="MultiLine" Width="93%" Rows="7" Columns="5"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label title="What procedure will be used to generalize this skill once mastered in the teaching setting? Optional.">Generalization Procedure :</label><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What procedure will be used to generalize this skill once mastered in the teaching setting? Optional.</span></div> </td>
                                                        <td></td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>

                                                    </tr>
                                                    <tr>
                                                        <td>

                                                            <asp:TextBox ID="txtGenProce" runat="server" TextMode="MultiLine" Width="93%" Rows="7" Columns="5"></asp:TextBox>
                                                        </td>
                                                        <td></td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                        <td></td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCommentTypeofInstr" runat="server" Text="Approval Note" Visible="false" title="This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter."></asp:Label><div class="toolTipImg" id="TypeInstructTooltip" runat="server" visible="false"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter.</span></div></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtCommentTypeofInstr" runat="server" Width="96.5%" TextMode="MultiLine" Rows="5" Columns="5" Visible="false"></asp:TextBox></td>

                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="2" style="text-align: right">
                                                            <asp:Button ID="btnCommentTypeofInstr" CssClass="bTnn" runat="server" Text="Update Approval Note" Style="margin-right: 20px; width: 140px" OnClick="btnCommentLessonInfo_Click" Visible="false" OnClientClick="scrollToTop();"/>
                                                        </td>
                                                    </tr>
                                                    <%--        <tr>
                                                        <td width="25%">
                                                            <label>Pre-requisite Skills:</label></td>
                                                        <td width="25%">
                                                            <label>Meterials:</label></td>
                                                        <td width="25%"></td>
                                                    </tr>--%>
                                                    <%-- <tr>
                                                        <td width="25%">
                                                            <asp:TextBox Style="resize: vertical; max-height: 200px; min-height: 50px"
                                                                ID="txtPrerequistskill" runat="server" TextMode="MultiLine"
                                                                Width="97%" Height="50px" MaxLength="400"></asp:TextBox></td>
                                                        <td width="25%">
                                                            <asp:TextBox Style="resize: vertical; max-height: 200px; min-height: 50px"
                                                                ID="txtMaterials" runat="server" TextMode="MultiLine" Width="97%"
                                                                Height="50px" MaxLength="400"></asp:TextBox></td>
                                                        <td width="25%"></td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td></td>
                                                        <td style="margin-right: 20px;">
                                                            <asp:Button ID="BtnUpdate" CssClass="bTnn" runat="server" Text="Update" OnClick="BtnUpdate_Click" OnClientClick="scrollToTop();" style="float:right;" ToolTip="Press to Save your work on this tab"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                             <div class="loading2" id="formatchngloader" runat="server">
                                              <div class="innerLoading">
                                                    <img src="../Administration/images/load.gif" alt="loading" />
                                                    Please Wait...
                                                </div>
                                                    <div class="frmtchngpopup-content" id="frmtchngpopup">
        <p style="font-size: 15px; text-align:center; color:black">Changing method will delete any steps/Trials previously entered.</p>
    <br />
       <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" OnClientClick="showload();" CssClass="NFButton" style="margin-left:50px;"/>
       <asp:Button ID="cancelbtn" runat="server" Text="Cancel" OnClick="cancelbtn_Click" OnClientClick="showload();"  CssClass="NFButton" style="margin-left:100px;"/>
    </div>
                                                </div>
                                            </div>


                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="clear"></div>
                                    <br />
                                </div>


                                <div class="tab">

                                    <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                        <ContentTemplate>
                                            <h2>Measurement Systems *</h2>



                                            <asp:DataList ID="dlMeasureData" runat="server" OnItemDataBound="dlMeasureData_ItemDataBound" Width="99%">
                                                <ItemTemplate>
                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="width: 70%">
                                                                    <table style="width: 100%;">


                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <br />
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td style="width: 40%;"><span title="The title of the measurement column on the datasheet">Column Name</span>
                                                                            <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>The title of the measurement column on the datasheet.</span></div></td>
                                                                            <td style="width: 60%;">
                                                                                <asp:HiddenField ID="hdnColId" Value='<%#Eval("DSTempSetColId") %>' runat="server" />
                                                                                <p>
                                                                                    <asp:Label ID="lblColumnName" runat="server" Text='<%#Eval("ColName") %>'></asp:Label>
                                                                                </p>

                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <br />
                                                                            </td>
                                                                        </tr>


                                                                        <tr>
                                                                            <td><span title="The measurement type for this column on the datasheet.">Column Type </span>
                                                                              <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>The measurement type for this column on the datasheet.</span></div> 
                                                                            </td>
                                                                            <td>
                                                                                <p>

                                                                                    <asp:Label ID="lblColumnType" runat="server" Text='<%#Eval("ColTypeCd") %>'></asp:Label>

                                                                                </p>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <br />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><span title="What text or number will be scored correct?">Correct Response Data</span>
                                                                              <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored correct?</span></div>  
                                                                            </td>
                                                                            <td>
                                                                                <p>
                                                                                    <asp:Label ID="lblCorrectResponse" runat="server" Text='<%#Eval("CorrRespDesc") %>'></asp:Label>
                                                                                </p>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <br />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><span title="What text or number will be scored incorrect?">Incorrect Response Data</span>
                                                                              <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored incorrect?</span></div> 
                                                                            </td>
                                                                            <td>
                                                                                <p>
                                                                                    <asp:Label ID="lblIncorrectResponse" runat="server" Text='<%#Eval("InCorrRespDesc") %>'></asp:Label>
                                                                                </p>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <br />
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="display: none">
                                                                            <td>Mistrial
                                                                               
                                                                            </td>
                                                                            <td>
                                                                                <p>
                                                                                    <asp:Label ID="lblMistrialDesc" runat="server" Text='<%#Eval("MisTrialDesc") %>'></asp:Label>
                                                                                </p>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <br />
                                                                            </td>
                                                                        </tr>



                                                                    </table>

                                                                    <table style="width: 86%">


                                                                        <tr>
                                                                            <td><span title="The names of all the calculations used to calculate this column of data are listed here.">Summary</span>
                                                                             <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>The names of all the calculations used to calculate this column of data are listed here.</span></div>   
                                                                            </td>
                                                                            <td>
                                                                                <p>

                                                                                    <asp:Literal ID="ltMeasureCaegory" runat="server"></asp:Literal>


                                                                                    <%-- <asp:Label ID="lblData1" runat="server"></asp:Label>  <br />

                                                                <asp:Label ID="lblData2" runat="server"></asp:Label>  <br />

                                                                <asp:Label ID="lblData3" runat="server"></asp:Label>--%>
                                                                                </p>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <br />
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                </td>
                                                                <td style="width: 30%; vertical-align: top">
                                                                    <table style="width: 100%;">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                                    <ContentTemplate>

                                                                                        <asp:Button ID="btnEditMeasure" runat="server" CssClass="smlbt" Text="Edit" OnClick="btnEditMeasure_Click" CommandArgument='<%# Eval("DSTempSetColId") %>' OnClientClick="window.parent.parent.scrollTo(0, 200);"/>

                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                                    <ContentTemplate>

                                                                                        <asp:Button ID="BtnRemove" runat="server" CssClass="smlbt" Text="Remove" OnClick="BtnRemove_Click" OnClientClick="javascript:return deleteSystem();" CommandArgument='<%# Eval("DSTempSetColId") %>' />

                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>


                                                                </td>
                                                            </tr>

                                                        </table>
                                                        <div class="clear"></div>
                                                    </div>

                                                </ItemTemplate>


                                            </asp:DataList>



                                            <asp:Button ID="BtnAddMeasure" runat="server" class="rbtn" Text="Add Measure" OnClick="BtnAddMeasure_Click" OnClientClick="return multiClickCheckVersion(this);"/>


                                            <asp:Button ID="BtnVTSavePrompt" runat="server" Text="Add Prompt" class="rbtn" OnClick="BtnVTSavePrompt_Click" Visible="false" OnClientClick="return confirm($(this).val()+'\nClick OK to proceed?');" />
                                            

                                            <%-- <input id="BtnAddMeasure" class="rbtn" type="button" value="Add Measure" />--%>

                                            <%--     <input class="rbtn" name="" value="Add Measure" type="button" />--%>
                                            <div class="clear"></div>


                                            <br />
                                            <div>

                                                <asp:Label ID="lblMeasureStart" runat="server" CssClass="tdText" Style="margin-left: 745px; color: #0D668E" Text="Please Select Add Measure to begin"></asp:Label>



                                            </div>
                                            <div>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblMeasurementSystems" runat="server" Text="Approval Note" Visible="false" title="This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter."></asp:Label><div class="toolTipImg" id="measurementsystemtooltip" runat="server" visible="false"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter.</span></div> </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>

                                                        <td>
                                                            <asp:TextBox ID="txtMeasurementSystems" runat="server" Width="96.5%" TextMode="MultiLine" Rows="5" Columns="5" Visible="false"></asp:TextBox></td>

                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Button ID="btnMeasurementSystems" CssClass="bTnn" runat="server" Text="Update Approval Note" Style="margin-right: 20px; width: 140px" OnClick="btnCommentLessonInfo_Click" Visible="false" OnClientClick="scrollToTop();" />
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td colspan="3" style="text-align: left;">
                                                            <asp:Button ID="btUpdateMeasurement" runat="server" class="rbtn" Text="Update" OnClick="btUpdateMeasurement_Click" OnClientClick="scrollToTop();" ToolTip="Press to Save your work on this tab"/>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>


                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <div class="tab">

                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <%--<h2>
                                        <asp:Label ID="lblLessonNameSet" runat="server" Text=""></asp:Label></h2>--%>
                                            <h3>Sets *</h3>
                                            <hr />
                                            <div class="clear"></div>

                                            <div class="loading">
                                                <div class="innerLoading">
                                                    <img src="../Administration/images/load.gif" alt="loading" />
                                                    Please Wait...
                                                </div>
                                            </div>

                                            <asp:DataList ID="dlSetDetails" runat="server" OnItemDataBound="dlSetDetails_ItemDataBound" Width="99%">
                                                <ItemTemplate>
                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="20%">
                                                                    <h3>
                                                                        <asp:Label ID="lblSetName" runat="server" Text='<%# Eval("SetCd") %>'></asp:Label></h3>
                                                                </td>
                                                                <td width="50%">
                                                                    <p>
                                                                        <asp:Label ID="lblSetDesc" runat="server" Text='<%# Eval("SetName") %>'></asp:Label>

                                                                    </p>
                                                                </td>
                                                                <td style="width: 2%">
                                                                    <asp:ImageButton ID="imgUp" runat="server" CommandArgument='<%# Eval("DSTempSetId") %>' OnClick="imgUp_Click" ImageUrl="~/StudentBinder/images/UPArrow.png" />

                                                                </td>
                                                                <td style="width: 2%">
                                                                    <asp:ImageButton ID="imgDown" runat="server" CommandArgument='<%# Eval("DSTempSetId") %>' OnClick="imgDown_Click" ImageUrl="~/StudentBinder/images/DownArrow.png" />

                                                                </td>

                                                                <td width="8%">

                                                                    <asp:Button ID="btnEditSet" runat="server" CssClass="smlbt" Text="Edit" OnClick="btnEditSet_Click" CommandArgument='<%# Eval("DSTempSetId") %>' />

                                                                    <%-- <a class="smlbt" href="#">Edit</a>--%>



                                                                </td>
                                                                <td width="8%">


                                                                    <asp:Button ID="btnRemoveSet" runat="server" Text="Remove" CssClass="smlbt" OnClientClick="javascript:return deleteSet();" OnClick="btnRemoveSet_Click" CommandArgument='<%# Eval("DSTempSetId") %>' />

                                                                    <%-- <a class="smlbt" href="#">Remove</a>--%>


                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </ItemTemplate>


                                            </asp:DataList>
                                            <div runat="server" id="setPanel">
                                                <div id="AddSetDiv1" class="" style="top: 6%; left: 6%;">
                                                    <div id="sign_up5">
                                                        <%--  <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>--%>
                                                        <br />
                                                        <h3>
                                                            <asp:Label ID="lblAddOrUpdateSet" Text="Add Set" runat="server" title="A Set is group of steps. Sometimes when you are writing a lesson you want to group your steps into chunks and master each chunk before meeting criteria to move to the next chunk. For example, if you have 40 steps to learn an entire task analysis, it may make sense to break it into 4 sets of 10 steps each rather than attacking 40 steps at once. Tip! If you don't need Sets, just put in a placeholder here called Step A, for example, and attach ALL of your steps to that one set, because the system requires at least one set."></asp:Label>
                                                            <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>A Set is group of steps. Sometimes when you are writing a lesson you want to group your steps into chunks and master each chunk before meeting criteria to move to the next chunk. For example, if you have 40 steps to learn an entire task analysis, it may make sense to break it into 4 sets of 10 steps each rather than attacking 40 steps at once. Tip! If you don't need Sets, just put in a placeholder here called Step A, for example, and attach ALL of your steps to that one set, because the system requires at least one set.</span></div>
                                                        </h3>
                                                        <hr />
                                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                            <ContentTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <span id="tdMsgSet" runat="server"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 30%;" class="tdText" title="What do you want to call this set?">Set Name:
                                                                        </td>
                                                                        <td style="text-align: left;">
                                                                            <asp:TextBox ID="txtBoxAddSet" runat="server" Width="450px" TabIndex="1"></asp:TextBox>
                                                                             <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What do you want to call this set?</span></div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="tdText" title="How do you describe this set?">Set Description:
                                                                        </td>
                                                                        <td style="text-align: left;">

                                                                            <asp:TextBox ID="txtSetDescription" runat="server" TextMode="MultiLine" Width="450px" TabIndex="2"></asp:TextBox>
                                                                             <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>How do you describe this set?</span></div>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="tdText">

                                                                            <asp:Label ID="lblMatchtoSamples" runat="server" Text="Target Name" Visible="false" ToolTip="Type a short target name, then press green button to save it. Continue to type in target names one at a time until all of your match-to-sample targets are entered. All targets you list here will be run on the same datasheet, so if you want to teach only 3 at a time, you would create new sets for each group of 3, not put them all in one long list."></asp:Label>

                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtMatcSamples" runat="server" Visible="false" MaxLength="30" Width="300px" TabIndex="3"></asp:TextBox>
                                                                            <asp:ImageButton ID="BtnAddSamples" runat="server" ImageUrl="~/StudentBinder/img/AddMatch.png" OnClick="BtnAddSamples_Click" Visible="false" Height="20px" Width="20px" TabIndex="4" style="margin-bottom: -5px;" OnClientClick="return CheckMatchtoSample();" />
                                                                            <div class="toolTipImg" runat="server" visible="false" id="MatchSampleTooltip"><img src="images/toolTipQMark.png" style="height:14px;width:14px;margin-top:22px" /><div class="tooltiptext"><span>Type a short target name, then press green button to save it. Continue to type in target names one at a time until all of your match-to-sample targets are entered. All targets you list here will be run on the same datasheet, so if you want to teach only 3 at a time, you would create new sets for each group of 3, not put them all in one long list.</span></div></div>
                                                                            <asp:CheckBox runat="server" visible="false" ID="chk_distractor" Text="Distractor?" style="margin-left:25px" ToolTip=""/>
                                                                            <div class="toolTipImg" runat="server" visible="false" id="distractortooltip"><img src="images/toolTipQMark.png" style="height:14px;width:14px;margin-top:10px;margin-left:10px" /><div class="tooltiptext"><span>Check "Distractor" before hitting the green Add button if the target you are entering is a distractor. A distractor is a target that will not be presented as a correct answer; it only serves to distract from the correct answer. TIP: If you don't want to name your distractor,just check the Distractor box, leave the Target Name blank, and hit the green Add button.</span></div></div>
                                                                            <asp:Label ID="lbl_txt" Width="100px" runat="server" Style="color: red;margin-left: 35px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="tdText"></td>
                                                                        <td>
                                                                            <asp:ListBox ID="lstMatchSamples" runat="server" CssClass="textClass" Style="color: black; font-weight: normal; height: 120px; width: 317px;" Visible="false" TabIndex="5"></asp:ListBox>
                                                                            <asp:ListBox ID="Disitem" runat="server" CssClass="textClass" Style="color: black; font-weight: normal; height: 120px; width: 317px;" Visible="false" TabIndex="5"></asp:ListBox>
                                                                            <asp:ImageButton ID="btnDeltSamples" runat="server" ImageUrl="~/StudentBinder/img/DeleteMatch.png" OnClick="btnDeltSamples_Click" Visible="false" Height="20px" Width="20px" Style="vertical-align: top;" TabIndex="6" />
                                                                             
                                                                           
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" style="text-align: center;">
                                                                            <asp:Button ID="btnAddSetDetails" runat="server" Text="Save" CssClass="NFButton" OnClick="btnAddSetDetails_Click" OnClientClick=" return ValidateSet(this)" TabIndex="7" />

                                                                            <asp:Button ID="BtnUpdateSetDetails" runat="server" Text="Update" CssClass="NFButton" Visible="false" OnClick="BtnUpdateSetDetails_Click" OnClientClick=" return ValidateSet(this)" TabIndex="7" />

                                                                            <asp:Button ID="btnAddSet" class="rbtn" runat="server" Text="Clear" OnClick="btnAddSet_Click" Style="float: none;" TabIndex="8" />

                                                                        </td>

                                                                    </tr>

                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                                                    </div>
                                                    <div id="previewClose"></div>
                                                </div>

                                                <%--</asp:Panel>--%>
                                            </div>

                                            <br clear="all" />

                                            <%--ashin--%>






                                            <%--   <input id="btnAddSet" class="rbtn" type="button" value="Add Set" />--%>

                                            <%--  <input class="rbtn" name="" value="Add Set" type="button" />--%>
                                            <div class="clear"></div>

                                            <div>


                                                <asp:Label ID="lblSetStart" runat="server" Visible="false" CssClass="tdText" Style="margin-left: 770px; color: #0D668E" Text="Please Select Add Set to begin"></asp:Label>


                                            </div>
                                            <hr />
                                            <h3>Set Criteria *</h3>



                                            <asp:DataList ID="dlSetCriteria" runat="server" OnItemDataBound="dlSetCriteria_ItemDataBound" Width="99%">
                                                <ItemTemplate>
                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="20%">
                                                                    <h3>
                                                                        <asp:HiddenField ID="hdnSetCritVal" runat="server" Value='<%#Eval("DSTempRuleId") %>' />
                                                                        <asp:Label ID="lblCriteriaType" runat="server"></asp:Label></h3>
                                                                </td>
                                                                <td width="70%">
                                                                    <p>


                                                                        <asp:Label ID="lblCriteriaDef" runat="server"></asp:Label>
                                                                    </p>
                                                                </td>
                                                                <td style="width: 4%"></td>
                                                                <td style="width: 8%">

                                                                    <asp:Button ID="BtnEditSetCriteria" runat="server" CssClass="smlbt" Text="Edit" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnEditSetCriteria_Click" OnClientClick="javascript: window.parent.parent.scrollTo(0, 150);"/>

                                                                    <%--<a class="smlRbt" href="#">Edit</a>--%>



                                                                </td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnRemoveSetCriteria" runat="server" CssClass="smlbt" Text="Remove" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnRemoveSetCriteria_Click" OnClientClick="javascript:return deleteSetCriteria();" />



                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </div>

                                                </ItemTemplate>


                                            </asp:DataList>






                                            <br clear="all" />
                                            <%--   <div class="itomesContainer">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="20%">
                                                    <h3>To Move Back</h3>
                                                </td>
                                                <td width="70%">
                                                    <p>1. Scores at or Below 60% Accuracy For 3 Consecutive Sessions.</p>
                                                </td>
                                                <td width="4%"></td>
                                                <td width="8%"><a class="smlRbt" href="#">Edit</a></td>
                                                <td width="8%"><a class="smlRbt" href="#">Remove</a></td>
                                            </tr>
                                        </table>
                                    </div>--%>



                                           <asp:Button ID="btnAddSetCriteria" runat="server" title="Add move-up or move-down criteria so the program can move the client up and down between sets based on his/her performance. Both move-up and move-down criteria are required to submit this lesson, so if you do not want to set criteria, add a move-up and move-down anyway and check off the NA checkbox. Tip: if you set up all your Sets, Steps, and Prompts first, then you can set your criterias for all three at once by using the copy option in the pop-up. " Text="Add Criteria" CssClass="rbtn" OnClick="btnAddSetCriteria_Click" OnClientClick="javascript: window.parent.parent.scrollTo(0, 150);"/>






                                            <%--   <input id="btnAddSetCriteria" class="rbtn" name="" value="Add Criteria" type="button" onclic />--%>
                                            <div class="clear"></div>
                                            <div>

                                                <asp:Label ID="lblSetCriStart" runat="server" CssClass="tdText" Style="margin-left: 749px; color: #0D668E" Text="Please select Add Criteria to begin"></asp:Label>


                                            </div>
                                            <div>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblcommentset" runat="server" Text="Approval Note" Visible="false" title="This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter."></asp:Label>
                                                             <div class="toolTipImg" id="commentSetTooltip" runat="server" visible="false"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter.</span></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>

                                                        <td>
                                                            <asp:TextBox ID="txtcommentset" runat="server" Width="96.5%" TextMode="MultiLine" Rows="5" Columns="5" Visible="false"></asp:TextBox></td>

                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Button ID="btncommentset" CssClass="bTnn" runat="server" Text="Update Approval Note" Style="margin-right: 20px; width: 140px" OnClick="btnCommentLessonInfo_Click" Visible="false" OnClientClick="scrollToTop();" />
                                                        </td>
                                                    </tr>
                                                                                                         <tr>
                                                        <td colspan="3" style="text-align: left;">
                                                            <asp:Button ID="btnUpdateset" runat="server" class="rbtn" Text="Update" OnClick="btnUpdateset_Click" OnClientClick="scrollToTop();" ToolTip="Press to Save your work on this tab"/>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <%-- <input class="rbtn" name="" value="Next >>" type="button" />--%>
                                            <div class="clear"></div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>


                                <div class="tab">
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>

                                            <%-- <h2>
                                        <asp:Label ID="lblLessonNameStep" runat="server" Text=""></asp:Label></h2>--%>
                                            <h3>Steps</h3>
                                            <hr />
                                            <div class="clear"></div>
                                            <div class="loading">
                                                <div class="innerLoading">
                                                    <img src="../Administration/images/load.gif" alt="loading" />
                                                    Please Wait...
                                                </div>
                                            </div>

                                            <div class="itomesContainer">
                                                <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 30px;"><b>Master<br /> Order</b></td>
                                                        <td style="width: 20%"><b>Steps</b></td>
                                                        <td style="width: 30%"><b>Description</b></td>
                                                        <td style="width: 20%"><b>Sets</b></td>
                                                        <td style="width: 20%"></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:DataList ID="dlStepDetails" runat="server" OnItemDataBound="dlStepDetails_ItemDataBound" Width="99%">
                                                <ItemTemplate>

                                                    <div class="itomesContainer">
                                                        
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                           
                                                            <tr>
                                                                <td style="width: 30px;">
                                                                    <h3>#<asp:Label ID="Label2" runat="Server" Text='<%# Container.ItemIndex + 1 %>' />
                                                                    </h3>
                                                                </td>
                                                                <td style="width: 20%">
                                                                    <h3>
                                                                        <asp:Label ID="lblStepName" runat="server" Text='<%#Eval("StepCd") %>'></asp:Label>

                                                                    </h3>
                                                                </td>

                                                                <td style="width: 30%">
                                                                    <p>

                                                                        <asp:Label ID="lblStepDesc" runat="server" Text='<%#Eval("StepName") %>'></asp:Label>
                                                                    </p>
                                                                </td>

                                                                <td style="width: 20%">
                                                                    <p style="text-align: left">
                                                                        <b>Set Name:</b>
                                                                        <%-- <asp:Label ID="lblParntSet" runat="server" Text='<%#Eval("SetNames") %>'></asp:Label>--%>
                                                                        <asp:Label ID="lblParntSet" runat="server" Text='<%#Eval("SetIds") %>'></asp:Label>
                                                                    </p>
                                                                </td>
                                                                <td style="width: 2%">
                                                                    <%-- <asp:ImageButton ID="imgUp" runat="server" CommandArgument='<%# Eval("DSTempParentStepId") %>' OnClick="imgUp_Click" ImageUrl="~/StudentBinder/images/UPArrow.png" Visible="false" />--%>

                                                                </td>
                                                                <td style="width: 2%">
                                                                    <%--<asp:ImageButton ID="imgDown" runat="server" CommandArgument='<%# Eval("DSTempParentStepId") %>' OnClick="imgDown_Click" ImageUrl="~/StudentBinder/images/DownArrow.png" Visible="false" />--%>

                                                                </td>
                                                                <td style="width: 8%">

                                                                    <asp:Button ID="btnEditStep" runat="server" Text="Edit" CssClass="smlbt" OnClick="btnEditStep_Click" CommandArgument='<%# Eval("DSTempParentStepId") %>' OnClientClick="javascript: window.parent.parent.scrollTo(0, 150);"/>

                                                                    <%--  <a class="smlbt" href="#">Edit</a>--%>


                                                                </td>
                                                                <td style="width: 8%">


                                                                    <asp:Button ID="btnRemoveStep" runat="server" Text="Remove" CssClass="smlbt" OnClick="btnRemoveStep_Click" CommandArgument='<%# Eval("DSTempParentStepId") %>' OnClientClick="javascript:return deleteStep();" />
                                                                    <%-- <a class="smlbt" href="#">Remove</a>--%>


                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </ItemTemplate>
                                            </asp:DataList>

                                            <div runat="server" id="stepPanel">
                                                <div id="AddStepDiv1" class="" style="top: 6%; left: 6%;">
                                                    <div id="SignUp_Step">
                                                        <br />
                                                        <h3>
                                                            <asp:Label ID="lblAddorUpdateStep" Text="Add Step" runat="server"></asp:Label>
                                                        </h3>
                                                        <hr />


                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>

                                                                <table style="width: 100%;">


                                                                    <tr>
                                                                        <td colspan="2">


                                                                            <span id="tdMsgStep" runat="server"></span>


                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td style="width: 30%;" class="tdText" title="Which SET do you want this STEP to run in? You can assign a step to one set, multiple sets, or all sets. Think of sets as containers that are filled with steps. You can easily design cascading lessons, where each set adds a few more steps but still runs all the prior sets, or lessons where each set has it's own unique steps.">Select Parent Set:
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownCheckBoxes ID="ddchkCountry" runat="server" TabIndex="1"
                                                                                AddJQueryReference="True" UseButtons="false" UseSelectAllNode="True" OnSelectedIndexChanged="ddchkCountry_SelectedIndexChanged" AutoPostBack="true">
                                                                                <Style SelectBoxWidth="318" DropDownBoxBoxWidth="318" DropDownBoxBoxHeight="130" />
                                                                                <Texts SelectBoxCaption="-----Select-----" />
                                                                            </asp:DropDownCheckBoxes>
                                                                             <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Which SET do you want this STEP to run in? You can assign a step to one set, multiple sets, or all sets. Think of sets as containers that are filled with steps. You can easily design cascading lessons, where each set adds a few more steps but still runs all the prior sets, or lessons where each set has it's own unique steps.</span></div>

                                                                            <%-- <asp:DropDownList ID="ddlSetData" runat="server" CssClass="drpClass" Width="318px">
                                                    <asp:ListItem Value="0">---------------ALL--------------</asp:ListItem>

                                                </asp:DropDownList>--%>

                                                                            <%--------------------------------------------------------------%>

                                                                            <%--     <asp:ListBox ID="lstSetData" runat="server" SelectionMode="Multiple"></asp:ListBox>--%>


                                                                            <%--------------------------------------------------------------%>
                                          
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td style="width: 30%;" class="tdText" title="Sets in which this step will appear. If it says 'Not Assigned to Any Sets', then the step you're working on will not actually show up on the datasheet. You must specify which set or sets you want each step to appear in.">Selected Parent Sets:
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblSelParentSets" runat="server"></asp:Label>
                                                                             <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Sets in which this step will appear. If it says 'Not Assigned to Any Sets', then the step you're working on will not actually show up on the datasheet. You must specify which set or sets you want each step to appear in.</span></div>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td style="width: 30%;" class="tdText" title="A brief name your step. Steps are auto-numbered by the system, so avoiding numbering here may save you some work later on if you rearrange your step order at a later date">Step Name:
                                                                        </td>
                                                                        <td style="text-align: left;">

                                                                            <asp:TextBox ID="txtStepName" runat="server" Width="450px" TabIndex="2"></asp:TextBox>

                                                                             <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>A brief name your step. Steps are auto-numbered by the system, so avoiding numbering here may save you some work later on if you rearrange your step order at a later date</span></div>
                                                                        </td>
                                                                    </tr>




                                                                    <tr>
                                                                        <td class="tdText" title="A little more detail about the step. Will show up on datasheet, so put something short but useful to help the teacher know what you expect them to do on this step">Step Description:
                                                                        </td>

                                                                        <td style="text-align: left;">

                                                                            <asp:TextBox ID="txtStepDesc" runat="server" TextMode="MultiLine" Width="450px" TabIndex="3"></asp:TextBox>
                                                                             <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>A little more detail about the step. Will show up on datasheet, so put something short but useful to help the teacher know what you expect them to do on this step</span></div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="tdText"><span title="Specify if your step will go to the bottom of the steps list or inserted somewhere in between.">Position:</span>
                                                                         <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px;margin-top: -5px;" /></div><div class="tooltiptext"><span>Specify if your step will go to the bottom of the steps list or inserted somewhere in between.</span></div>
                                                                        </td>

                                                                        <td style="text-align: left;">
                                                                            <asp:CheckBox ID="chkEnd" runat="server" Text="At the End" Checked="true" OnCheckedChanged="chkEnd_CheckedChanged"  AutoPostBack="true" ToolTip="If you want to re-arrange your steps or insert steps, just specify where you want to place the step by choosing which step to put it before."/><div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Check to add the step to the end of the steps list (default).</span></div>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                                            
                                                                            <span title="If you want to re-arrange your steps or insert steps, just specify where you want to place the step by choosing which step to put it before">Insert Before Step</span>                                             
                                                                                <asp:DropDownList ID="ddlSortOrder" runat="server" CssClass="drpClass" width="100px" Enabled="false">                                                                               
                                                                                <asp:ListItem Value="0">---Select---</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                             <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>If you want to re-arrange your steps or insert steps, just specify where you want to place the step by choosing which step to put it before.</span></div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" style="text-align: center;">



                                                                            <asp:Button ID="btnAddStepDetails" runat="server" Text="Save" CssClass="NFButton" OnClick="btnAddStepDetails_Click" OnClientClick="return ValidateStep(this);" TabIndex="4" />

                                                                            <asp:Button ID="BtnUpdateStep" runat="server" Text="Update" CssClass="NFButton" Visible="false" OnClick="BtnUpdateStep_Click" OnClientClick="scrollToTop(); return ValidateStep(this); " TabIndex="4" />



                                                                            <asp:Button ID="BtnAddStep" runat="server" Text="Clear" CssClass="rbtn" OnClick="BtnAddStep_Click" Style="float: none;" TabIndex="5" />
                                                                            <asp:ImageButton ID="BtnAddSort" runat="server" OnClick="BtnAddSort_Click" ImageUrl="~/StudentBinder/images/sort.png" Height="25px" Width="25px" TabIndex="6" style="display:none"/>


                                                                        </td>

                                                                    </tr>



                                                                </table>


                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>


                                                        <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                                                    </div>
                                                    <div id="Div3"></div>

                                                </div>

                                                <%--</asp:Panel>--%>
                                            </div>

                                            <br clear="all" />

                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="text-align: right; width: 90%;">



                                                        <%-- <asp:Button ID="BtnAddSort" runat="server" Text="Add Step" CssClass="rbtn" OnClick="BtnAddSort_Click" />--%>


                                                    </td>
                                                    <td style="text-align: left; width: 10%;"></td>
                                                </tr>
                                            </table>







                                            <div class="clear"></div>

                                            <div>


                                                <asp:Label ID="lblStepStart" runat="server" CssClass="tdText" Style="margin-left: 757px; color: #0D668E" Text="Please Select Add Step to begin"></asp:Label>


                                            </div>
                                            <hr />
                                            <h3>Step Criteria</h3>






                                            <asp:DataList ID="dlStepCriteria" runat="server" OnItemDataBound="dlStepCriteria_ItemDataBound" Width="99%">
                                                <ItemTemplate>
                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="20%">
                                                                    <h3>
                                                                        <asp:HiddenField ID="hdnStepCritVal" runat="server" Value='<%#Eval("DSTempRuleId") %>' />
                                                                        <asp:Label ID="lblCriteriaTypeStep" runat="server"></asp:Label></h3>
                                                                </td>
                                                                <td width="70%">
                                                                    <p>


                                                                        <asp:Label ID="lblCriteriaDefStep" runat="server"></asp:Label>
                                                                    </p>
                                                                </td>
                                                                <td width="4%"></td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnEditStepCriteria" runat="server" CssClass="smlbt" Text="Edit" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnEditStepCriteria_Click" />

                                                                    <%--<a class="smlRbt" href="#">Edit</a>--%>



                                                                </td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnRemoveStepCriteria" runat="server" CssClass="smlbt" Text="Remove" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnRemoveStepCriteria_Click" OnClientClick="javascript:return deleteStepCriteria();" />



                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </div>

                                                </ItemTemplate>


                                            </asp:DataList>




                                            <br clear="all" />




                                            <asp:Button ID="btnAddStepCriteria" runat="server" Text="Add Criteria" CssClass="rbtn" OnClick="btnAddStepCriteria_Click" ToolTip="Add move-up or move-down criteria so the program can move the client up and down between steps based on his/her performance. Both move-up and move-down criteria are required to submit this lesson, so if you do not want to set criteria, add a move-up and move-down anyway and check off the NA checkbox. Tip: if you set up all your Sets, Steps, and Prompts first, then you can set your criterias for all three at once by using the copy option in the pop-up." />
                                            <asp:HiddenField ID="HdfAddstep" runat="server" Value="" />

                                            <%-- <input class="rbtn" name="" value="Add Criteria" type="button" />--%>
                                            <div class="clear"></div>
                                            <%--       <input class="rbtn" name="" value="Next >>" type="button" />--%>
                                            <div>

                                                <asp:Label ID="lblStepCriStart" runat="server" CssClass="tdText" Style="margin-left: 749px; color: #0D668E" Text="Please Select Add Criteria to begin"></asp:Label>




                                            </div>
                                            <div>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblcommentStep" runat="server" Text="Approval Note" Visible="false" ToolTip="This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter."></asp:Label>
                                                             <div class="toolTipImg" id="commentStepTooltip" runat="server" visible="false"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter.</span></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>

                                                        <td>
                                                            <asp:TextBox ID="txtcommentStep" runat="server" Width="96.5%" TextMode="MultiLine" Rows="5" Columns="5" Visible="false"></asp:TextBox></td>

                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Button ID="btncommentStep" CssClass="bTnn" runat="server" Text="Update Approval Note" Style="margin-right: 20px; width: 140px" OnClick="btnCommentLessonInfo_Click" Visible="false" OnClientClick="scrollToTop();"/>

                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td colspan="3" style="text-align: left;">
                                                            <asp:Button ID="btnUpdatesteps" runat="server" class="rbtn" Text="Update" OnClick="btnUpdatesteps_Click" OnClientClick="scrollToTop();" ToolTip="Press to Save your work on this tab"/>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="clear"></div>



                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>


                                <div class="tab">

                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                        <ContentTemplate>
                                            <%--  <h2>
                                        <asp:Label ID="lblLessonNamePrompt" runat="server" Text=""></asp:Label></h2>--%>
                                            <h3>Add Prompt</h3>
                                            <hr />


                                            <div class="itomesContainer">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <%--   <tr>
                                                <td width="25%">
                                                    <label>Prompt Required</label></td>
                                                <td width="5%">
                                                    <input name="" class="rdoo" type="radio" value="" /><label>Yes</label></td>
                                                <td width="25%">
                                                    <input name="" class="rdoo" type="radio" value="" /><label>No</label></td>
                                                <td width="25%"></td>
                                            </tr>--%>
                                                </table>
                                                <br clear="all" />
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td width="17%">
                                                            <label title="Choose your prompt procedure. These are intelligent - the application knows the difference between the procedures and runs them accordingly automatically.">Prompt Procedure</label></td>
                                                        <td width="45%">
                                                            <asp:DropDownList ID="ddlPromptProcedure" runat="server" CssClass="drpClass" OnSelectedIndexChanged="ddlPromptProcedure_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Choose your prompt procedure. These are intelligent - the application knows the difference between the procedures and runs them accordingly automatically.</span></div>
                                                            <td></td>
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="chk_stepBystep" Text="Step By Step"  ToolTip="Advanced use only. Step by Step means the application will track a current prompt for each step in a chain. Normally, a single prompt will be current for the whole chain, and when all steps achieve that prompt all steps move to the next prompt. In Step By Step, the prompts move up and down on per-step basis, so step 1 may be independent and step 3 verbal at the same time on a total task. Only when all the steps achieve independence will the whole chain succeed. NOTE: This is not to be confused with the fact that even without Step By Step, forward and backward chains do track performance of prior learned steps for the purpose of automatically dropping back to a step that begins to fail."/>
                                                                <div class="toolTipImg" id="chk_stepBysteptooltip" runat="server"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Advanced use only. Step by Step means the application will track a current prompt for each step in a chain. Normally, a single prompt will be current for the whole chain, and when all steps achieve that prompt all steps move to the next prompt. In Step By Step, the prompts move up and down on per-step basis, so step 1 may be independent and step 3 verbal at the same time on a total task. Only when all the steps achieve independence will the whole chain succeed. NOTE: This is not to be confused with the fact that even without Step By Step, forward and backward chains do track performance of prior learned steps for the purpose of automatically dropping back to a step that begins to fail.</span></div>
                                                            </td>
                                                    </tr>
                                                </table>
                                                <br clear="all" />
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td width="9.8%">
                                                            <%--<label>Select Prompt</label>--%>

                                                            <asp:Label ID="lblSelctPrompt" runat="server" Text="Select Prompt"></asp:Label>

                                                        </td>
                                                        <td width="22%">

                                                            <asp:ListBox ID="lstCompletePrompts" runat="server" CssClass="textClass" Style="color: black; font-weight: normal; height: 120px; width: 100%;"></asp:ListBox>
                                                            <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" id="CompletePromptTool" runat="server" /></div><div class="tooltiptext"><span>All available prompts. Move them to the selected prompt box on the right with the arrow buttons. The application knows the heirarchy of the prompts from Least to Most; that is hard-coded and you cannot change it, cosequently the order in which you list them on the right is meaningless. Prompts A-E are generic prompts if the prompt you need is not on the list. If you use prompts A-E you must clearly define them in the Sd/instruction box on the Lesson Procedure tab so the teacher has enough information to carry out the lesson.</span></div>
                                                        </td>
                                                        <td width="10%">

                                                            <asp:Button ID="BtnAddPromptSelctd" runat="server" CssClass="smlRbt" Text="&gt;" OnClick="BtnAddPromptSelctd_Click" />
                                                            <asp:Button ID="BtnAddAllPrompt" runat="server" CssClass="smlRbt" Text="&gt;&gt;" OnClick="BtnAddAllPrompt_Click" />
                                                            <asp:Button ID="BtnRemvePrmptSelctd" runat="server" CssClass="smlRbt" Text="&lt;" OnClick="BtnRemvePrmptSelctd_Click" />
                                                            <asp:Button ID="BtnRemoveAllPrmpt" runat="server" CssClass="smlRbt" Text="&lt;&lt;" OnClick="BtnRemoveAllPrmpt_Click" />

                                                            <%--     <a class="smlRbt" href="#">>> </a>
                                                            <a class="smlRbt" href="#">> </a>

                                                            <a class="smlRbt" href="#"><< </a>
                                                            <a class="smlRbt" href="#">< </a>--%>
                                                        </td>
                                                        <td width="20%">

                                                            <asp:ListBox ID="lstSelectedPrompts" runat="server" CssClass="textClass" Style="color: black; font-weight: normal; height: 120px; width: 95%;"></asp:ListBox>
                                                            <%-- <textarea class="srBox" name="" cols="" rows=""></textarea>--%>
                                                            <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px"  id="SelectedPromptTool" runat="server"/></div><div class="tooltiptext"><span>All prompts selected go here. The application knows the heirarchy of the prompts from Least to Most; that is hard-coded and you cannot change it, cosequently the order in which you list them on the right is meaningless.</span></div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="clear"></div>


                                                <asp:Button ID="BtnSavePrompt" class="rbtn" runat="server" Text="Save" OnClick="BtnSavePrompt_Click" />
                                                <%-- <input class="rbtn" name="" value="Save" type="button" />--%>
                                                <div class="clear"></div>
                                            </div>


                                            <div class="clear"></div>
                                            <hr />
                                            <h3>Prompt Criteria</h3>



                                            <asp:DataList ID="dlPromptCriteria" runat="server" OnItemDataBound="dlPromptCriteria_ItemDataBound" Width="99%">
                                                <ItemTemplate>
                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="20%">
                                                                    <h3>
                                                                        <asp:HiddenField ID="hdnPromptCritVal" runat="server" Value='<%#Eval("DSTempRuleId") %>' />
                                                                        <asp:Label ID="lblCriteriaTypePrompt" runat="server"></asp:Label></h3>
                                                                </td>
                                                                <td width="70%">
                                                                    <p>


                                                                        <asp:Label ID="lblCriteriaDefPrompt" runat="server"></asp:Label>
                                                                    </p>
                                                                </td>
                                                                <td width="4%"></td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnEditPromptCriteria" runat="server" CssClass="smlbt" Text="Edit" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnEditPromptCriteria_Click" OnClientClick="javascript: window.parent.parent.scrollTo(0, 150);"/>

                                                                    <%--<a class="smlRbt" href="#">Edit</a>--%>



                                                                </td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnRemovePromptCriteria" runat="server" CssClass="smlbt" Text="Remove" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnRemovePromptCriteria_Click" OnClientClick="javascript:return deletePromptCriteria();" />



                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </div>

                                                </ItemTemplate>


                                            </asp:DataList>







                                            <asp:Button ID="btnAddPrompt" runat="server" Text="Add Prompt Criteria" CssClass="rbtn" OnClick="btnAddPrompt_Click" Style="width: auto;" OnClientClick="javascript: window.parent.parent.scrollTo(0, 150);" ToolTip="Add move-up or move-down criteria so the program can move the client up and down between prompts based on his/her performance. Both move-up and move-down criteria are required to submit this lesson, so if you do not want to set criteria, add a move-up and move-down anyway and check off the NA checkbox. Tip: if you set up all your Sets, Steps, and Prompts first, then you can set your criterias for all three at once by using the copy option in the pop-up."/>



                                            <br clear="all" />

                                            <div class="clear">
                                                <div>

                                                    <asp:Label ID="lblPromptCriStart" runat="server" CssClass="tdText" Style="margin-left: 749px; color: #0D668E" Text="Please Select Add Criteria to begin"></asp:Label>


                                                </div>

                                                <div>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblcommentPrompt" runat="server" Text="Approval Note" Visible="false" ToolTip="This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter."></asp:Label>
                                                                <div class="toolTipImg" id="commentPromptTooltp" runat="server" visible="false"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter.</span></div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                        </tr>
                                                        <tr>

                                                            <td>
                                                                <asp:TextBox ID="txtcommentPrompt" runat="server" Width="96.5%" TextMode="MultiLine" Rows="5" Columns="5" Visible="false"></asp:TextBox></td>

                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <asp:Button ID="btncommentPrompt" CssClass="bTnn" runat="server" Text="Update Approval Note" Style="margin-right: 20px; width: 140px" OnClick="btnCommentLessonInfo_Click" Visible="false" OnClientClick="scrollToTop();"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                        <td colspan="3" style="text-align: left;">
                                                            <asp:Button ID="btnupdateprompts" runat="server" class="rbtn" Text="Update" OnClick="btnupdateprompts_Click" OnClientClick="scrollToTop();" ToolTip="Press to Save your work on this tab"/>

                                                        </td>
                                                    </tr>
                                                    </table>
                                                </div>

                                            </div>


                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>

                                <div class="tab">

                                    <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                        <ContentTemplate>
                                            <%-- <h2>
                                        <asp:Label ID="lblLessonNameProcedure" runat="server" Text=""></asp:Label></h2>--%>
                                            <h3>Add Lesson Procedure</h3>
                                            <hr />



                                            <div class="itomesContainer">

                                                <table style="width: 100%;">

                                                    <tr>
                                                        <td style="width: 30%; text-align: center;" class="border">
                                                            <strong>TEACHER 
                                                            </strong>
                                                            <br />
                                                            (Sd/instruction)
                                                            <br />
                                                            <br />
                                                            What does teacher do to prepare to teach lesson?
                                                        </td>
                                                        <td style="width: 30%; text-align: center;" class="border">
                                                            <strong>INDIVIDUAL 
                                                            </strong>
                                                            <br />
                                                            (Response/Desired Outcome)
                                                            <br />
                                                            <br />
                                                            What does Individual need to do to prepare for being taught this lesson?
                                                        </td>
                                                        <td style="width: 30%; text-align: center;">
                                                            <strong>CONSEQUENCE
                                                            </strong>
                                                            <br />
                                                            (R+ delivery or correction procedure)
                                                            <br />
                                                            <br />
                                                            What does the teacher do or say in response to the Individual being ready for lesson?
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td class="border">

                                                            <asp:TextBox ID="txtTeacherDo" runat="server" MaxLength="100" Width="230px" TextMode="MultiLine" Rows="7" Columns="5"></asp:TextBox></td>

                                                        <td class="border">
                                                            <asp:TextBox ID="txtStudentDo" runat="server" MaxLength="100" Width="230px" TextMode="MultiLine" Rows="7" Columns="5"></asp:TextBox></td>
                                                        <td>
                                                            <asp:TextBox ID="txtConsequenceDO" runat="server" MaxLength="100" Width="230px" TextMode="MultiLine" Rows="7" Columns="5"></asp:TextBox></td>
                                                    </tr>

                                                    <tr>
                                                        <td style="border-top: thin solid #CCCCCC; color: #514F4F">Teacher (Sd/instruction)</td>
                                                        <td style="border-top: thin solid #CCCCCC; color: #514F4F">Correct Response (if applicable)</td>
                                                        <td style="border-top: thin solid #CCCCCC; color: #514F4F">Reinforcement procedure</td>
                                                    </tr>

                                                    <tr>
                                                        <td class="border" rowspan="5" style="vertical-align: top;">
                                                            <asp:TextBox ID="txtSDInstruction" runat="server" MaxLength="100" Width="230px" TextMode="MultiLine" Rows="28" Columns="5"></asp:TextBox></td>
                                                        <td class="border">
                                                            <asp:TextBox ID="txtResponseOutcome" runat="server" MaxLength="100" Width="230px" TextMode="MultiLine" Rows="7" Columns="5"></asp:TextBox></td>
                                                        <td>
                                                            <asp:TextBox ID="txtReinforcementProc" runat="server" MaxLength="100" Width="230px" TextMode="MultiLine" Rows="7" Columns="5"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>

                                                        <td class="border">
                                                            <label class="ii" style="width: 170px;">Incorrect Response (if applicable)</label></td>
                                                        <td>
                                                            <label class="ii" style="width: 170px;">Correction Procedure</label></td>
                                                    </tr>
                                                    <tr>

                                                        <td class="border">
                                                            <asp:TextBox ID="txtIncorrectResponse" runat="server" MaxLength="100" Width="230px" TextMode="MultiLine" Rows="7" Columns="5"></asp:TextBox>

                                                            <%-- <asp:Literal ID="litrlCrctResp" runat="server"></asp:Literal>--%>

                                                            <%-- <asp:Label ID="lblCorrctResp" runat="server" Text="" CssClass="tdText"></asp:Label>--%>

                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCorrectionProcedure" runat="server" MaxLength="100" Width="230px" TextMode="MultiLine" Rows="7" Columns="5"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>

                                                        <td class="border">
                                                            <label class="ii" style="width: 170px;">Mistrial Response</label></td>
                                                        <td>


                                                            <label class="ii" style="width: 170px;">Mistrial Procedure</label>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td class="border">

                                                            <%--   <asp:Literal ID="ltrlIncrctResp" runat="server"></asp:Literal>--%>

                                                            <%--    <asp:Label ID="lblIncrctResp" runat="server" Text="" CssClass="tdText"></asp:Label>--%>
                                                            <asp:TextBox ID="txtMistrial" runat="server" MaxLength="100" Width="230px" TextMode="MultiLine" Rows="7" Columns="5"></asp:TextBox>


                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMistrialProcedure" runat="server" MaxLength="100" Width="230px" TextMode="MultiLine" Rows="7" Columns="5"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="border" runat="server" id="tdSet" style="word-wrap: break-word; text-align: left; vertical-align: top;" title="Automatically lists all the SETS. If you need to change or add SETS, go back to the SETS tab."><strong>Set Description(s): </strong>
                                                       </td>
                                                        <td class="border" runat="server" id="tdStep" style="word-wrap: break-word; text-align: left; vertical-align: top;" title="Automatically lists all the STEPS. If you need to change or add STEPS,  go back to the STEPS tab."><strong>Step Description(s):</strong>
                                                         
                                                        </td>

                                                    </tr>

                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblcommentLessonProcedure" runat="server" Text="Approval Note" Visible="false" ToolTip="This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter."></asp:Label>
                                                             <div class="toolTipImg" id="commentProceduretooltp" runat="server" visible="false"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>This is an area for general internal notes on each tab, it is not part of the lesson plan and is not mandatory. Usually used for communication between the approver and the submitter.</span></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>

                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtcommentLessonProcedure" runat="server" Width="92%" TextMode="MultiLine" Rows="5" Columns="5" Visible="false"></asp:TextBox></td>

                                                    </tr>

                                                    <tr>
                                                        <td colspan="3" style="text-align: right">
                                                            <asp:Button ID="btncommentLessonProcedure" CssClass="bTnn" runat="server" Text="Update Approval Note" Style="margin-right: 20px; width: 140px" OnClick="btnCommentLessonInfo_Click" Visible="false" OnClientClick="scrollToTop();"/>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="3" style="text-align: left;">
                                                            <asp:Button ID="btUpdateLessonProc" runat="server" class="rbtn" Text="Update" OnClick="btUpdateLessonProc_Click" OnClientClick="scrollToTop();" ToolTip="Press to Save your work on this tab"/>

                                                        </td>
                                                    </tr>

                                                </table>






                                                <%--  <input class="rbtn" name="" value="Summary" type="button" />--%>
                                                <div class="clear"></div>
                                            </div>

                                            <div class="clear"></div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>


                            </div>

                        </div>
                        <div id="Div7">
                        </div>






                        <div class="clear"></div>
                    </div>




                    <!------------------------------------End Container End----------------------------------->

                    <div class="clear"></div>
                </div>
                <!-------------------------Middle Container End----------------------->


                <!------------------------Pop up Windows----------------------->


                <%--ashin--%>
                <div id="SetContainer" style="width: 100%; height: 100%;">
                </div>

                <%-----copy template to student or as template-----%>

                <div id="HdrExport" style="width: 100%; height: 100%;">
                    <div id="HdrExportTemplate" class="web_dialog" style="top: 7%; height: auto;">


                        <div id="Hdr_Stat">
                            <a id="closeHdr" onclick="closePOP();" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>

                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <table style="align-content: center">
                                        <tr>
                                            <td id="tdMsgExprt" runat="server"></td>
                                        </tr>
                                        <tr>
                                            <%--<td>
                                                <asp:Label ID="lblCopyLessonplan" runat="server" Text="Copy Lesson Plan Template"></asp:Label>
                                            </td>--%>
                                            <caption>
                                                <br />
                                                <br />
                                                <tr>
                                                    <td>
                                                       <asp:CheckBox ID="chkCpyStdtTemplate" runat="server" AutoPostBack="false" onclick="TempSelected();" Checked="false" Text="Copy Lesson Plan as Template" Enabled ="False" />
                                                    </td>
                                                </tr>
                                            </caption>

                                        </tr>

                                        <tr>
                                            <td>
                                                <div class="studentDiv">


                                                    <%--<asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>--%>
                                                    <asp:Label ID="Label1" runat="server">Copy Lesson Plan to</asp:Label>
                                                    <asp:TextBox ID="txtSname" runat="server" CssClass="textClass" Width="170px" value="Individual Name" onBlur="if(this.value=='') this.value='Individual Name'" onFocus="if(this.value =='Individual Name' ) this.value=''"></asp:TextBox>
                                                    <asp:ImageButton ID="imgsearch" runat="server" CssClass="searchbtnimg" ImageUrl="img/searchbtn.png" OnClick="imgsearch_Click" />
                                                    <br />
                                                    <asp:Label ID="LBLClassnotfound" runat="server" ForeColor="Red"></asp:Label>
                                                    <div style="height: auto;" id="classDivs">

                                                        <div style="width: 100%; margin-top: 10px;" id="DlStudent"></div>
                                                    </div>
                                                    <%--  </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hfSelectedStudent" runat="server" />
                                                <asp:HiddenField ID="hdDefaultName" runat="server" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkStudentList" runat="server" Visible="false" /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>

                                    </table>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div style="display: none" id="divCopyTempAdmin" runat="server">
                                <table>
                                    <tr>
                                        <td>Lesson Plan Name</td>
                                        <td><span style="color: red">*</span></td>
                                        <td>
                                            <input type="text" id="txtCopyTempAdmin" style="width: 180px;" /></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>

                            <asp:Button ID="btnCopyTempAdmin" runat="server" CssClass="NFButton" Style="width: 180px; display: none" Text="Copy Lesson Plan" OnClick="btnCopyTempAdmin_click" OnClientClick="return ExecuteLPExist(this);" />

                        </div>
                    </div>
                </div>



                <%--previous step container--%>


                <div id="CriteriaContainer" style="width: 100%; height: 100%;">
                    <div id="AddCriteriaDiv" class="web_dialog" style="top: 6%; left: 6%;">

                        <div id="signUp_Criteria">
                            <a id="close_CriteriaPopup" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                            <h3>&nbsp;
                                <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblCriteriaName" runat="server"></asp:Label>
                                    </ContentTemplate>

                                </asp:UpdatePanel>


                            </h3>
                            <hr />


                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                            <ContentTemplate>

                                                <table style="width: 100%;">

                                                    <tr>
                                                        <td colspan="4">
                                                            <span id="tdMsgCriteria" runat="server"></span>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdText"><span title="Choose move-up, move-down, or modification for this criteria. Both move-up and move-down criteria are required to submit this lesson, so if you do not want to set criteria, add a move-up and move-down anyway and check off the NA checkbox. The modification criteria is optional and for information only; it is usually not created."> Criteria Type</span>
                                                        <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Choose move-up, move-down, or modification for this criteria. Both move-up and move-down criteria are required to submit this lesson, so if you do not want to set criteria, add a move-up and move-down anyway and check off the NA checkbox. The modification criteria is optional and for information only; it is usually not created.</span></div>                                 
                                                        </td>

                                                        <td colspan="1">
                                                            <asp:DropDownList ID="ddlCriteriaType" runat="server" CssClass="drpClass" AutoPostBack="True" OnSelectedIndexChanged="ddlCriteriaType_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">..............Select.................</asp:ListItem>
                                                                <asp:ListItem>MOVE UP</asp:ListItem>
                                                                <asp:ListItem>MOVE DOWN</asp:ListItem>
                                                                <asp:ListItem>MODIFICATION</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 20%;" class="tdText"><span title="If Yes is checked, when the client meets move-up criteria the program will wait for a different instructor to deliver the same lesson with similar results  before making the move. This ensures the client's readiness to move is not linked to a specific staff person."> Multiteacher Required </span>
                                                           <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>If Yes is checked, when the client meets move-up criteria the program will wait for a different instructor to deliver the same lesson with similar results  before making the move. This ensures the client's readiness to move is not linked to a specific staff person.</span></div>                                 
                                                        </td>
                                                        <td style="width: 30%;">
                                                            <asp:RadioButtonList ID="rbtnMultitchr" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>



                                                    <tr>
                                                        <td style="width: 20%;" class="tdText"><span title="If Yes is checked, when the client meets move-up criteria the program will wait for an Inter-observer Agreement session to be run before making the move. IOA is when two people record data for the same lesson delivery simultaneously to confirm the accuracy of the data."> IOA Required </span>
                                                         <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>If Yes is checked, when the client meets move-up criteria the program will wait for an Inter-observer Agreement session to be run before making the move. IOA is when two people record data for the same lesson delivery simultaneously to confirm the accuracy of the data.</span></div>
                                                        </td>
                                                        <td style="width: 30%;">
                                                            <asp:RadioButtonList ID="rbtnIoaReq" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>

                                                            </asp:RadioButtonList>
                                                        </td>

                                                        <td class="tdText"><span title="If Yes is chosen, the Required Score must be attained consecutively the number of sessions below."> Consecutive Session </span>
                                                           <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>If Yes is chosen, the Required Score must be attained consecutively the number of sessions below.</span></div>                                 
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rbtnConsectiveSes" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbtnConsectiveSes_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdText"><span title="The name of the measurement column you want to use to measure this criteria.">Template Column </span> 
                                                           <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>The name of the measurement column you want to use to measure this criteria.</span></div>                                 
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTempColumn" runat="server" CssClass="drpClass" OnSelectedIndexChanged="ddlTempColumn_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Value="0">---------------Select Column--------------</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="tdText"><span title="If Yes is chosen, the Required Score must be attained as an average over the number of sessions below."> Consecutive Average </span> <!--- [New Criteria] May 2020 - (Start)--->
                                                           <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>If Yes is chosen, the Required Score must be attained as an average over the number of sessions below.</span></div>                                 
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rbtnConsectiveAvg" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbtnConsectiveAvg_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:RadioButtonList> <!--- [New Criteria] May 2020 - (End) --->
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdText"><span title="The specific formula assigned to the selected measurement above"> Measure</span>
                                                           <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>The specific formula assigned to the selected measurement above.</span></div>                                 
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTempMeasure" runat="server" CssClass="drpClass">
                                                                <asp:ListItem Value="0">---------------Select Measure--------------</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="tdText"><span title="If Yes was chosen for Consecutive sessions, then fill in how many. For example, 80% x 5 consective sessions would be 5."> Number of Sessions </span>
                                                           <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>If Yes was chosen for Consecutive sessions, then fill in how many. For example, 80% x 5 consective sessions would be 5.</span></div>                                 
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNumbrSessions" runat="server" ReadOnly="true" Width="75px" onkeypress="return isNumber(event)" onpaste="return false" MaxLength="2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdText"><span title="What score is this criteria using?"> Required Score </span>
                                                          <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What score is this criteria using?</span></div>                                 
                                                             </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRequiredScore" runat="server" Width="75px" onkeypress="return isNumber(event)" onpaste="return false" MaxLength="3"></asp:TextBox>
                                                        </td>
                                                        <td class="tdText"><span title="Use to specify x out of y sessions. For example, to meet criteria the client must score 80% 4 out of 5 sessions.">Instance </span>
                                                           <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Use to specify x out of y sessions. For example, to meet criteria the client must score 80% 4 out of 5 sessions.</span></div>                                 
                                                        </td>
                                                        <td style="border: 0px;">
                                                            <table style="width: 100%;" cellpading="0px" cellspacing="0px">
                                                                <tr>
                                                                    <td style="width: 30%;">
                                                                        <asp:TextBox ID="txtIns1" runat="server" Width="75px" onkeypress="return isNumber(event)" onpaste="return false" MaxLength="2"></asp:TextBox>
                                                                    </td>
                                                                    <td class="tdText" style="width: 14%; padding-left: 4px;">out of
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtIns2" runat="server" Width="75px" onkeypress="return isNumber(event)" onpaste="return false" MaxLength="6"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <tr id="trNACriteria" runat="server">
                                                        <td></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkNACriteria" Text="NA" runat="server" />
                                                        </td>

                                                    </tr>





                                                    <tr id="trModification" runat="server" style="visibility: hidden;">
                                                        <td class="tdText">For Modification<asp:HiddenField ID="IsComments" Value="1" runat="server" />
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:HiddenField ID="hfLessonResult" runat="server" />
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkIsComments" runat="server" OnCheckedChanged="chkIsComments_CheckedChanged" AutoPostBack="True" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtModComments" runat="server" Width="663px" onpaste="return false"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                    </tr>

                                                    <tr id="trModification1" runat="server" style="visibility: hidden;">
                                                        <td class="tdText"></td>
                                                        <td colspan="3">

                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkScore" runat="server" OnCheckedChanged="chkScore_CheckedChanged" AutoPostBack="True" />Repetition of previous step more 
                                                                        <asp:TextBox ID="txtModNo" runat="server" Width="25px" onkeypress="return isNumber(event)" onpaste="return false" MaxLength="3"></asp:TextBox>
                                                                        times with no progress.
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                    </tr>





                                                    <tr>
                                                        <td class="tdText">
                                                            <asp:Label ID="lblCopyTo" CssClass="tdText" runat="server" Text="Copy to" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table style="width: 100%;" cellpading="0px" cellspacing="0px">
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCpySetCri" runat="server" Visible="false" /><asp:Label ID="lblCpySetCri" CssClass="tdText" runat="server" Text="Set Criteria" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCpyStepCri" runat="server" Visible="false" /><asp:Label ID="lblCpyStepCri" runat="server" CssClass="tdText" Text="Step Criteria" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>


                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCpyPromptCri" runat="server" Visible="false" /><asp:Label ID="lblCpyPromptCri" runat="server" CssClass="tdText" Text="Prompt Criteria" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>

                                                            </table>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="4" style="text-align: center;">
                                                            <asp:Button ID="BtnAddSetDCriteria" runat="server" Text="Save" CssClass="NFButton" OnClick="BtnAddSetDCriteria_Click" OnClientClick=" return ValidateCriteria(this)" />
                                                            <asp:Button ID="BtnAddStepDCriteria" runat="server" Text="Save" CssClass="NFButton" OnClick="BtnAddStepDCriteria_Click" OnClientClick=" return ValidateCriteria(this)" />
                                                            <asp:Button ID="BtnAddPromptDCriteria" runat="server" Text="Save" CssClass="NFButton" OnClick="BtnAddPromptDCriteria_Click" OnClientClick=" return ValidateCriteria(this)" />
                                                            <asp:Button ID="BtnUpdateSetDCriteria" runat="server" Text="Update" CssClass="NFButton" Visible="false" OnClick="BtnUpdateSetDCriteria_Click" OnClientClick=" return ValidateCriteria(this)" />
                                                            <asp:Button ID="BtnUpdateStepDCriteria" runat="server" Text="Update" CssClass="NFButton" Visible="false" OnClick="BtnUpdateStepDCriteria_Click" OnClientClick=" return ValidateCriteria(this)" />
                                                            <asp:Button ID="BtnUpdatePromptDCriteria" runat="server" Text="Update" CssClass="NFButton" Visible="false" OnClick="BtnUpdatePromptDCriteria_Click" OnClientClick=" return ValidateCriteria(this)" />
                                                        </td>
                                                    </tr>
													

                                                </table>

                                            </ContentTemplate>


                                        </asp:UpdatePanel>



                                    </td>
                                </tr>

                            </table>




                            <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                        </div>
                        <div id="Div5"></div>

                    </div>
                    
                </div>

               <div id="EmailContainer" style="width: 100%; height: 100%;">
                    <div id="popupEmailMain" class="web_dialog" style="display: none;top: 6%; left: 20%; ">
                            <asp:updatepanel ID="Updatepanel15" runat="server">
                            <ContentTemplate>
                                <a class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                    <img src="../Administration/images/button_red_close.PNG" onclick="javascript:$('#popupEmailMain').fadeOut('slow', function () {$('.fullOverlay').fadeOut('fast');});uncheckEmailSelection();" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300; display:none" width="18" height="18" alt="" />
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Administration/images/button_red_close.PNG" OnClick="emailFullReset"  Style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" Width="18" Height="18" alt="" TabIndex="6" OnClientClick="javascript:$('#popupEmailMain').fadeOut('slow', function () {$('.fullOverlay').fadeOut('fast');});uncheckEmailSelection();" />
                                </a>
                            </ContentTemplate>
                            </asp:updatepanel>
                            <div style="margin: 20px 10px 10px 10px;position:relative;">
                            <h2>Email for review</h2>
                        </div>
                        
                        <div style="margin: 0 10px 10px 10px;">
                            <h4>Select one or more people to notify that this lesson is ready for review.</h4>
                        </div>
                        <div  id="msgAlert" class='warning_box' style="display:none;position:absolute;"></div>
                        <div id="wrapper">    
                            <div id="popupEmailSub" style="width: auto; height: 200px;margin-right:10px;padding: inherit;">
                                <asp:updatepanel ID="Updatepanel13" runat="server">
                                <ContentTemplate>
                                 <div style="float:left;border: 1px solid #909095;border-radius:5px;padding-right: 10px;padding-bottom: 10px;margin-left: 5px;">
                                     <div style="margin-top:5px;margin-left:10px">
                                        <asp:Label ID="lblEmailSrch" CssClass="tdText" runat="server" Text="Enter Keyword for Search Emails" Visible="true"></asp:Label>
                                        <asp:Textbox ID="txtEmailSrch" CssClass="tdText" runat="server" onkeydown="onEnterKeyPress(event)" ></asp:Textbox>                                         
                                        <asp:Button ID="btnSrchEmail" runat="server" Text="Search Email" CssClass="NFButton" Visible="true" OnClick="btnSrchEmail_Click" />
                                        <hr />
                                     </div>
                                     <div style="width: auto; height: 148px; overflow-y: scroll;margin-left:10px;margin-right:1px;padding: inherit;border: 1px solid black;">
                                        <asp:DataList ID="dlEmailforSorting" runat="server" RepeatLayout="Flow">
                                            <ItemTemplate>
                                                <div>
                                                    <asp:HiddenField runat="server" ID="hdn_usrID" Value='<%# Eval("UsrID") %>' />
                                                    <asp:HiddenField runat="server" ID="hdn_EmailValue" Value='<%# Eval("Email") %>' />
                                                    <asp:CheckBox ID="emailCheckBox" runat="server" Text='<%# Eval("UserName") %>' /> <!-- AutoPostBack = "true" OnCheckedChanged="emailCheckBox_CheckedChanged2" -->
                                                </div>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label Visible='<%#bool.Parse((dlEmailforSorting.Items.Count==0).ToString())%>' runat="server" ID="lblNoEmailRecord" Text="&nbsp;&nbsp;&nbsp;No Data Found!"></asp:Label>
                                            </FooterTemplate>
                                        </asp:DataList>
                                        <asp:DataList ID="dlEmailforSorting_Copy" runat="server" RepeatLayout="Flow">
                                            <ItemTemplate>
                                                <div>
                                                    <asp:HiddenField runat="server" ID="hdn_usrID_Copy" Value='<%# Eval("UsrID") %>' />
                                                    <asp:HiddenField runat="server" ID="hdn_EmailValue_Copy" Value='<%# Eval("Email") %>' />
                                                    <asp:CheckBox ID="emailCheckBox_Copy" CommandName="checkval" runat="server" Text='<%# Eval("UserName") %>'/> <!-- AutoPostBack = "true" OnCheckedChanged="emailCheckBox_CheckedChanged" -->
                                                </div>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label Visible='<%#bool.Parse((dlEmailforSorting_Copy.Items.Count==0).ToString())%>' runat="server" ID="Label3" Text="&nbsp;&nbsp;&nbsp;No Data Found!"></asp:Label>
                                            </FooterTemplate>
                                        </asp:DataList>
                                        <div style="margin-bottom: 10%;"></div>
                                    </div>                                    
                                    <asp:Button ID="btnEmailSelction" runat="server" Text="Confirm Email Selection" CssClass="NFButton" Visible="true" style="float: right;width:auto !important;margin-bottom: 2%;margin-top: 10px;" OnClick="btnEmailSelction_Click" />
                                    <asp:HiddenField runat="server" ID="hdn_statusFlag" Value='' />
                                </div>
                                <div style="margin-left:10px;margin-right:13px;float:right">
                                    <div style="float:right">
                                        <input type="button" runat="server" class="NFButton" id="btnRstmails" value="Reset" onclick="uncheckEmailSelection();"/>
                                    </div><br />
                                    <h4 style="padding-top:25px">Selected Usernames for Sending Email</h4>
                                    <asp:ListBox ID="lstSelectdEmails" runat="server" CssClass="textClass" Style="color: black; font-weight: normal; height: 212px; width: 333px;display:none;"></asp:ListBox>
                                    <asp:ListBox ID="lstSelecteUserNames" runat="server" CssClass="textClass" Style="color: black; font-weight: normal; height: 212px; width: 333px;"></asp:ListBox>
                                    <input type="image" name="btnDeltmails" id="delmailImage1" tabindex="6" src="img/DeleteMatch.png" style="height:20px;width:20px;border-width:0px;vertical-align: top;display:none" onclick="deleteSelect();">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/StudentBinder/img/DeleteMatch.png" OnClick="btnDeltMails_Click" Visible="true" Height="20px" Width="20px" Style="vertical-align: top;" TabIndex="6" OnClientClick="return checkLbvals()" />
                                </div>
                            </div>
                            </ContentTemplate>
                            </asp:updatepanel>
                        </div>
                        <div style="margin: 0 10px 15px 10px;">
                            <br />
                            <h4 style="margin-top:55px !important">Optional message:</h4>
                            <asp:TextBox ID="OptMessage" runat="server" Width="96.5%" TextMode="MultiLine" Rows="4" Columns="5" style = "resize:none"></asp:TextBox>
                        </div>
                        <div style="text-align: center; margin: 10px 0 10px 0">
                            <asp:Button runat="server" class="NFButton" ID="BtnEmailSend" Text="Send" OnClick="btnEmailSend_Click" OnClientClick="return checkEmailSelection(this);"/>
                            <asp:Button runat="server" class="NFButton" ID="BtnEmailApproveSend" Text="Send" OnClick="btnEmailSend_Click" OnClientClick="return checkEmailSelection(this);"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <span style="display:inline-block">
                            <asp:updatepanel ID="Updatepanel14" runat="server">
                            <ContentTemplate>
                            <asp:Button runat="server" CssClass="NFButton" ID="BtnEmailCancel" Text="Cancel" OnClientClick="javascript: $('#popupEmailMain').fadeOut('slow', function () { $('.fullOverlay').fadeOut('fast'); });" OnClick="emailFullReset" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </ContentTemplate>
                            </asp:updatepanel>
                            </span>
                            <asp:Button runat="server" class="NFButton" ID="BtnSkip" Text="Skip" OnClick="btnPriorLevel_Click" OnClientClick="return multiClickCheck(this);" />
                        </div>
                    </div>
               </div>


                 <div id="MeasureContainer" style="width: 100%; height: 100%;">
                    <div id="AddMeasureDiv" class="web_dialog" style="top: 6%; left: 29%; width:602px">

                        <div id="SignUpNew">
                            <a id="Close1" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;" onclick="close1Click()">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                           <div style="background-color: #d8edf3;height:60px;font-weight:bold"> <p style="padding-top:1em;text-align:center;font-size:x-large;padding-bottom:2em">Add a Measurement Column to the Datasheet</p><br /><br /><br />                         
                           </div>
                            <br/>
                            <%-- <hr />--%>

                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" style="margin-left: 15px;">
                                <ContentTemplate>

                                    <table style="width: 100%; margin-left:-1% ">
                                        <tr style="height:40px">
                                       
                                            <td colspan="2" class="auto-style1" style="background-color:lightblue" title="The title of the measurement column on the datasheet. Be brief but specific enough to help the person delivering the lesson know what they are recording.">

                                                <h2><font color=black>Title</font> </h2> <i>What do you want to title this column of the datasheet?</i>

                                            </td>


                                        </tr>
                                        <tr>

                                            <td colspan="2">

                                                <span id="tdMsgMeasure" runat="server"></span>

                                            </td>


                                        </tr>

                                        <tr>
                                            <td style="width: 220px;" class="tdText" id="tdMsg" runat="server">&nbsp;
                                            </td>
                                            <td>

                                                <asp:TextBox ID="txtColumnName" runat="server" MaxLength="50"></asp:TextBox>
                                                <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>The title of the measurement column on the datasheet. Be brief but specific enough to help the person delivering the lesson know what they are recording.</span></div>
                                            </td>

                                        </tr>
                                        <tr style="height:40px">
                                            <td class="auto-style2" colspan="2" style="background-color:lightblue"><h2 style="color:black" title="The measurement type for this column on the datasheet. Options are +/-, Prompt (will make a dropdown of prompts on the datasheet for each step), text, duration and frequency. If you choose only Prompt, it will be able to score it fine because it knows the prompt heirarachy and what constitutes correct/incorrect. If you choose just +/-, it will work as well but the actual prompt achieved will not be recorded. If you choose +/- and prompt, that is also fine but is a little more recording work for the teacher with no advantage over just prompt.">Measurement</h2><i>Choose +/-, prompt, duration or text for this column of the datasheet</i></td>
                                            <td class="auto-style2">


                                                </td>
                                        </tr>

                                        <tr>
                                            <td class="tdText">&nbsp;
                                            </td>
                                            <td>


                                                <asp:DropDownList ID="ddlColumnType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlColumnType_SelectedIndexChanged" CssClass="drpClass">
                                                    <asp:ListItem>+/-</asp:ListItem>
                                                    <asp:ListItem>Prompt</asp:ListItem>
                                                    <asp:ListItem>Text</asp:ListItem>
                                                    <asp:ListItem>Duration</asp:ListItem>
                                                    <%--<asp:ListItem>Frequency</asp:ListItem>--%>
                                                    <asp:ListItem Value="Frequency">Frequency / Number</asp:ListItem>
                                                </asp:DropDownList> 
                                                <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>The measurement type for this column on the datasheet. Options are +/-, Prompt (will make a dropdown of prompts on the datasheet for each step), text, duration and frequency. If you choose only Prompt, it will be able to score it fine because it knows the prompt heirarachy and what constitutes correct/incorrect. If you choose just +/-, it will work as well but the actual prompt achieved will not be recorded. If you choose +/- and prompt, that is also fine but is a little more recording work for the teacher with no advantage over just prompt.</span></div>

                                            </td>
                                        </tr>
                                        

                                        <tr>
                                            <td colspan="2">

                                                <div id="PlusMinusDiv" runat="server" visible="true">
                                                   <asp:LinkButton ID="LinkButton1" runat="server"   OnClientClick=" return PlusminusAoTb1()" style="color:black" Text="Advanced Option"><img src="images/A3.png" alt="Advanced Options"  border="0" style="width:10px;height:10px;"/>&nbsp;Advanced Options</asp:LinkButton> 
                                                    <table id="aoTb" style="width: 95%; display :none" runat="server">
                                                        
                                                        <tr>
                                                            <td style="width: 40%; color:black; text-align:right" class="tdText">Is (+) scored as correct (+) or incorrect (-)?</td>
                                                            <td style="width: 60%;">
                                                                <asp:RadioButtonList ID="rdbplusMinus" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem>+</asp:ListItem>
                                                                    <asp:ListItem>-</asp:ListItem>
                                                                </asp:RadioButtonList>


                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="tdText" style="vertical-align: middle;text-align:right;color:black" title="What text or number will be scored correct?">What text or number will be scored correct?: </td>
                                                            <td>
                                                                <asp:TextBox ID="txtplusCorrectResponse" runat="server"></asp:TextBox>
                                                                 <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored correct?</span></div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText" style="vertical-align: middle;text-align:right;color:black" title="What text or number will be scored incorrect?">What text or number will be scored incorrect?:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPlusIncorrectResp" runat="server"></asp:TextBox>
                                                                 <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored incorrect?</span></div>
                                                            </td>
                                                        </tr>

                                                        <tr style="display: none">
                                                            <td class="tdText">
                                                                <b>Mistrial</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Mistrial label</b>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td>
                                                                <asp:CheckBox ID="chkplusIncludeMistrial" Text="Include Mistrial" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtplusIncludeMistrial" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div style="background-color:lightblue">
                                                        <p style="padding-top:1%;"><h2 style="color:black">Scoring</h2><i><b><font color="black"> Must </font> </b> choose one or more - &nbsp<b><font color="black">these are intelligent &nbsp</font></b>-&nbsp they calculate properly based on the teaching method</i><p>
                                                            </p>
                                                            <br />
                                                        </p>
                                                            
                                                    </div>
                                                    <table class="tableMeasure" ><%--table 1 --%>

                                                        <caption>
                                                            <br />
                                                            <tr>
                                                                <td class="tdText" style="border-right: 1px double silver;color:black ; text-align:center"><b>Formulas</b> </td>
                                                                <td class="tdText" style="text-align:center;color:black ; width:330px"><b>What do you want to call this<br /> measure?</b>  <br />     
                                                                    
                                                                    <i>Leave blank for default name</i>       
                                                                    <br>
                                                                    <br></br>
                                                                    </br>
                                                                </td>
                                                                <td class="tdText"><b>Include in <br />Graph</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-right: 1px double silver; width:31%">
                                                                    <asp:CheckBox ID="chkplusAccuracy" runat="server" CssClass="tdText" onchange="checkForIIG(this);" Text="%Accuracy" />
                                                                    <br />
                                                                    <span title="Use whenever the formula (Total Correct/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task."> (Total Correct/Total)*100 </span>
                                                                     <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Use whenever the formula (Total Correct/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task.</span></div>
                                                                </td>
                                                                <td style="text-align:center">
                                                                    <asp:TextBox ID="txtplusAccuracy" runat="server" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkplusAccuracyIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdText" style="border-right: 1px double silver">
                                                                    <asp:CheckBox ID="chkPlusPromptPerc" runat="server" onchange="checkForIIG(this);" Text="%Prompted" />
                                                                    <br /> 
                                                                    <span title="Use whenever the formula (Total Prompted/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task.">(Total Prompted/Total)*100 </span> 
                                                                     <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Use whenever the formula (Total Prompted/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task.</span></div>
                                                                </td>
                                                                <td style="text-align:center">
                                                                    <asp:TextBox ID="txtPlusPromptPerc" runat="server" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkPlusPromptPercIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-right: 1px double silver">
                                                                    <asp:CheckBox ID="chkplusindependent" runat="server" CssClass="tdText" onchange="checkForIIG(this);" Text="%Independent" />
                                                                    <br />
                                                                    <span title="Use whenever the formula (Total Independent/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task."> (Total Ind/Total )*100 </span>
                                                                     <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Use whenever the formula (Total Independent/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task.</span></div>
                                                                </td>
                                                                <td style="text-align:center">
                                                                    <asp:TextBox ID="txtplusIndependent" runat="server" CssClass="tdText" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkplusindependentIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-right: 1px double silver">
                                                                    <asp:CheckBox ID="chkplustotalcorrect" runat="server" CssClass="tdText" onchange="checkForIIG(this);" Text="Total Correct" ToolTip="Calculates the total amount of steps that are correct. Only results in a number" />
                                                                     <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Calculates the total amount of steps that are correct. Only results in a number.</span></div>
                                                                </td>
                                                                <td style="text-align:center">
                                                                    <asp:TextBox ID="txtplustotalcorrect" runat="server" CssClass="tdText" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkplustotalcorrectIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-right: 1px double silver">
                                                                    <asp:CheckBox ID="chkplustotalIncorrect" runat="server" CssClass="tdText" onchange="checkForIIG(this);" Text="Total Incorrect" ToolTip="Calculates the total amount of steps that are incorrect. Only results in a number" />
                                                                     <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Calculates the total amount of steps that are incorrect. Only results in a number.</span></div>
                                                                </td>
                                                                <td style="text-align:center">
                                                                    <asp:TextBox ID="txtPlustotalIncorrect" runat="server" CssClass="tdText" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkplustotalIncorrectIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                                </td>
                                                            </tr>
                                                        </caption>
                                                    </table>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="return PlusminusAoTb2()" style="color:black" Text="Advanced Option"><img src="images/A3.png" alt="Advanced Options"  border="0" style="width:10px;height:10px;"/>&nbsp;Advanced Options</asp:LinkButton> 
                                                    <div id="opdiv1" runat="server" style="background-color:#ffffb3 ; display:none" >
                                                        <h2 style="color:black">Optional scores</h2>
                                                        <p><i>Some extra measure to display on graph/reports. Not available for criteria-the primary formulas above<br /> intelligently score based on the teaching method </i></p>
                                                        
                                                    </div>
                                                     <table class="tableMeasure" id="aoTb2" runat="server" style="display:none"  ><%--newly created tb --%>
                                                         
                                                          <tr>
                                                            <td style="border-right: 1px double silver;width:31%">
                                                                <asp:CheckBox ID="chkplusindependentForAll" runat="server" Text="%Independent of All Steps" CssClass="tdText" onchange="checkForIIG(this);" /><br />
                                                                (Total Independent Trials/Total Trials)*100
                                                            </td>
                                                            <td style="text-align:center; width:58%">
                                                                <asp:TextBox ID="txtplusIndependentForAll" runat="server" CssClass="tdText" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkplusindependentForAllIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style="border-right: 1px double silver">
                                                                <%--<asp:CheckBox ID="chkpluslearnedStep" runat="server" Text="%Accuracy For Learned Step" CssClass="tdText" />--%>
                                                                <asp:CheckBox ID="chkpluslearnedStep" runat="server" Text="%Accuracy at Training Step" CssClass="tdText" onchange="checkForIIG(this);" ToolTip="Extra measure, does not drive move-up/down criteria moves. Use in addition to an automatic measurement to ensure moves occur." />
                                                                 <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Extra measure, does not drive move-up/down criteria moves. Use in addition to an automatic measurement to ensure moves occur.</span></div>

                                                            </td>
                                                            <td style="text-align:center">
                                                                <asp:TextBox ID="txtpluslearnedStep" runat="server" CssClass="tdText" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkpluslearnedStepIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="border-right: 1px double silver">
                                                                <%--<asp:CheckBox ID="chkCurrentStep" runat="server" Text="%Accuracy Excluding CurrentStep" CssClass="tdText" />--%>
                                                                <asp:CheckBox ID="chkCurrentStep" runat="server" Text="%Accuracy at Previously Learned Steps" CssClass="tdText" onchange="checkForIIG(this);" ToolTip="Extra measure, does not drive move-up/down criteria moves. Use in addition to an automatic measurement to ensure moves occur."/>
                                                                 <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Extra measure, does not drive move-up/down criteria moves. Use in addition to an automatic measurement to ensure moves occur.</span></div>

                                                            </td>
                                                            <td style="text-align:center">
                                                                <asp:TextBox ID="txtExCurrentStep" runat="server" CssClass="tdText" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkCurrentStepIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </div>

                                                <div id="promptDiv" runat="server" visible="false">
                                                    <asp:LinkButton ID="LinkButton3" runat="server" OnClientClick="return PromptAoTbdisplay()" style="color:black" Text="Advanced Option"><img src="images/A3.png" alt="Advanced option"  border="0" style="width:10px;height:10px;"/>&nbsp;Advanced Options</asp:LinkButton> 
                                                    <table id="promptAOTb" runat="server" style="width: 95%; display:none">

                                                        <tr>

                                                            <td style="width: 40%; vertical-align: top;"></td>
                                                            <td>
                                                                <asp:CheckBox ID="chkCurrentPrompt" runat="server" Text="Current Prompt" AutoPostBack="true" OnCheckedChanged="chkCurrentPrompt_CheckedChanged" />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <div id="divCurrentPrompt" runat="server" visible="false">
                                                                    <asp:DropDownList ID="ddlPromptList" runat="server" CssClass="drpClass"></asp:DropDownList>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 30%; vertical-align: top; text-align:right" class="tdText" title="What text or number will be scored correct?">What text or number will be scored correct?:
                                                            </td>
                                                            <td>
                                                                <%--<asp:CheckBox ID="chkCurrentPrompt" Text="Current Prompt" runat="server" />--%>

                                                                <asp:TextBox ID="txtpromptSelectPrompt" runat="server"></asp:TextBox>
                                                                   <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored correct?</span></div>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td style="width: 30%; vertical-align: top; text-align:right " class="tdText" title="What text or number will be scored incorrect?">What text or number will be scored incorrect?:
                                                            </td>
                                                            <td>
                                                                <%--<asp:CheckBox ID="chkCurrentPrompt" Text="Current Prompt" runat="server" />--%>

                                                                <asp:TextBox ID="txtPromptIncrctResp" runat="server"></asp:TextBox>
                                                                   <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored incorrect?</span></div>
                                                            </td>

                                                        </tr>
                                                     
                                                        <%--  <tr>
                                                            <td class="tdText" style="width: 30%;">Select Prompt:
                                                            </td>
                                                            <td style="width: 30%;">
                                                                <asp:DropDownList ID="ddlPromptList" CssClass="drpClass" runat="server"></asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                 <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                            </td>

                                                        </tr>--%>
                                                        <tr style="display: none">
                                                            <td class="tdText">
                                                                <b>Mistrial</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Mistrial Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td>
                                                                <asp:CheckBox ID="chkPromptInclMisTrial" Text="Inc.Mis trial" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPromptIncMisTrial" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                     <div style="background-color:lightblue">
                                                        <p style="padding-top:1%;"><h2 style="color:black">Scoring</h2><i><b><font color="black"> Must </font> </b> choose one or more - &nbsp<b><font color="black">these are intelligent &nbsp</font></b>-&nbsp they calculate properly based on the teaching method</i><p>
                                                            </p>
                                                            <br />
                                                         </p>
                                                            
                                                    </div>
                                                    <table class="tableMeasure" >

                                                        <caption>
                                                            <br />
                                                            <tr>
                                                                <td class="tdText" style="border-right: 1px double silver; color:black; text-align:center"><b>Formulas</b> </td>
                                                                <td class="tdText" style="text-align:center; color:black"><b>What do you want to call this<br /> measure?</b>
                                                                  <br/>
                                                                   <i> Leave blank for default name</i>
                                                                    
                                                                </td>
                                                                <td class="tdText"><b>Include in <br /> Graph</b> </td>
                                                            </tr>
                                                            <tr> 
                                                                <td style="border-right: 1px double silver">
                                                                    <asp:CheckBox ID="chkPrompPercAccuracy" runat="server" onchange="checkForIIG(this);" Text="%Accuracy" />
                                                                    <br />
                                                                    <span title="Use whenever the formula (Total Correct/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task."> (Total Correct/Total)*100 </span>
                                                                       <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Use whenever the formula (Total Correct/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task.</span></div>
                                                                </td>
                                                                <td style="text-align:center">
                                                                    <asp:TextBox ID="txtPromptAccuracy" runat="server" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkPrompPercAccuracyIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-right: 1px double silver">
                                                                    <asp:CheckBox ID="chkPromptPercPrompt" runat="server" onchange="checkForIIG(this);" Text="%Prompted" />
                                                                    <br />
                                                                   <span title="Use whenever the formula (Total Prompted/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task."> (Total Prompted/Total)*100 </span>
                                                                       <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Use whenever the formula (Total Prompted/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task.</span></div>
                                                                </td>
                                                                <td style="text-align:center">
                                                                    <asp:TextBox ID="txtPromptpecPrompt" runat="server" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkPromptPercPromptIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-right: 1px double silver">
                                                                    <asp:CheckBox ID="chkPercIndependent" runat="server" onchange="checkForIIG(this);" Text="%Independent" />
                                                                    <br />
                                                                    <span title="Use whenever the formula (Total Independent/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task.">(Total Ind/Total)*100</span>
                                                                       <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Use whenever the formula (Total Independent/Total) x 100 is appropriate. This is an intelligent measurement - it will caculated properly based on the teaching method. For example, it knows to calculate the teaching step on a forward chain and all steps on a total task.</span></div>
                                                                </td>
                                                                <td style="text-align:center">
                                                                    <asp:TextBox ID="txtPromptIndependent" runat="server" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkPercIndependentIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                                </td>
                                                            </tr>
                                                        </caption>

                                                    </table>
                                                    <asp:LinkButton ID="LinkButton4" runat="server" OnClientClick=" return PromptAoTbdisplay2()" style="color:black" Text="Advanced Option"><img src="images/A3.png" alt="Advanced Options"  border="0" style="width:10px;height:10px;"/>&nbsp;Advanced Options</asp:LinkButton> 
                                                    <div id="opdiv2" runat="server" style="background-color:#ffffb3; display:none" >
                                                        <h2 style="color:black">Optional scores</h2>
                                                        <p><i>Some extra measure to display on graph/reports. Not available for criteria-the primary formulas above<br /> intelligently score based on the teachingmethod </i></p><br />
                                                        
                                                    </div>
                                                    <table id="promptAOTb2" runat="server" class="tableMeasure;" style="display:none">
                                                        <tr>
                                                            <td style="border-right: 1px double silver;width:32%">
                                                                <asp:CheckBox ID="chkPercIndependentForAll" Text="%Independent of All Steps" runat="server" onchange="checkForIIG(this);" /><br />
                                                                (Total Independent Trials/Total Trials)*100
                                                            </td>
                                                            <td style="width:54% ; text-align:center">
                                                                <asp:TextBox ID="txtPromptIndependentForAll" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkPercIndependentForAllIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="border-right: 1px double silver">
                                                                <%--<asp:CheckBox ID="chkPromptAccLearnedStep" runat="server" Text="%Accuracy For Learned Step" CssClass="tdText" />--%>
                                                                <asp:CheckBox ID="chkPromptAccLearnedStep" runat="server" Text="%Accuracy at Training Step" CssClass="tdText" onchange="checkForIIG(this);"  ToolTip="Extra measure, does not drive move-up/down criteria moves. Use in addition to an automatic measurement to ensure moves occur."/>
                                                                   <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Extra measure, does not drive move-up/down criteria moves. Use in addition to an automatic measurement to ensure moves occur.</span></div>
                                                            </td>
                                                            <td style="text-align:center">
                                                                <asp:TextBox ID="txtPromptAccLearnedStep" runat="server" CssClass="tdText" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkPromptAccLearnedStepIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="border-right: 1px double silver">
                                                                <%--<asp:CheckBox ID="chkPromptAccExcluseCrntStep" runat="server" Text="%Accuracy Excluding CurrentStep" CssClass="tdText" />--%>
                                                                <asp:CheckBox ID="chkPromptAccExcluseCrntStep" runat="server" Text="%Accuracy at Previously Learned Steps" CssClass="tdText" onchange="checkForIIG(this);" ToolTip="Extra measure, does not drive move-up/down criteria moves. Use in addition to an automatic measurement to ensure moves occur."/>
                                                                   <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>Extra measure, does not drive move-up/down criteria moves. Use in addition to an automatic measurement to ensure moves occur.</span></div>
                                                            </td>
                                                            <td style="text-align:center">
                                                                <asp:TextBox ID="txtPromptAccExcluseCrntStep" runat="server" CssClass="tdText" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkPromptAccExcluseCrntStepIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </div>


                                                <div id="TextDiv" runat="server" visible="false">
                                                    <asp:LinkButton ID="LinkButton5" runat="server" OnClientClick="return TextDivDisplay()" style="color:black" Text="Advanced Option"><img src="images/A3.png" alt="Advanced option"  border="0" style="width:10px;height:10px;"/>&nbsp;Advanced Options</asp:LinkButton> 

                                                    <table id="textAOTb" runat="server" style="width: 95%; display:none" >
                                                        <tr>
                                                            <td style="width: 40%; vertical-align: top; text-align:right" class="tdText" title="What text or number will be scored correct?">What text or number will be scored correct?:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTextCrctResponse" runat="server"></asp:TextBox>
                                                                 <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored correct?</span></div>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style="width: 30%; vertical-align: top; text-align:right" class="tdText" title="What text or number will be scored Incorrect?">What text or number will be scored incorrect?:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTextInCrctResp" runat="server"></asp:TextBox>
                                                                 <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored incorrect?</span></div>
                                                            </td>
                                                        </tr>

                                                        <tr style="display: none">
                                                            <td class="tdText">
                                                                <b>Mistrial</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Mistrial Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td>
                                                                <asp:CheckBox ID="chkTxtIncMisTrial" Text="Inc.Mis trial" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTxtIncMisTrial" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                     <div style="background-color:lightblue">
                                                        <p style="padding-top:1%;"><h2 style="color:black">Scoring</h2><i><b><font color="black"> Must </font> </b> choose one or more - &nbsp<b><font color="black">these are intelligent &nbsp</font></b>-&nbsp they calculate properly based on the teaching method</i><p>
                                                            </p>
                                                            <br />
                                                         </p>
                                                            
                                                    </div>
                                                    <table class="tableMeasure" >
                                                        <tr>
                                                            <td class="tdText" style="width: 172px; border-right: 1px groove; text-align:center; color:black">
                                                                <b>Formulas</b>
                                                            </td>
                                                            <td class="tdText" style="text-align:center; color:black">
                                                                <b>What do you want to call this<br /> measure</b><br />
                                                                <i>Leave Blank for default name</i>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Include in<br /> Graph</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="border-right: 1px double silver; text-align:center">
                                                                <asp:CheckBox ID="chkTxtNa" Text="NA" runat="server" onchange="checkForIIG(this);" />
                                                            </td>
                                                            <td style="text-align:center">
                                                                <asp:TextBox ID="txtTxtNA" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkTxtNaIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" class="tdText">No Calculation
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="border-right: 1px double silver ; text-align:center">
                                                                <asp:CheckBox ID="chkTextCustomize" Text="Customize" runat="server" onchange="checkForIIG(this);" />
                                                            </td>
                                                            <td style="text-align:center">
                                                                <asp:TextBox ID="txtTxtCustomize" runat="server" MaxLength="50"></asp:TextBox>
                                                                <asp:ImageButton ID="imgCreateEqutn" runat="server" Visible="false" Height="20px" Width="20px" Style="vertical-align: middle;" ImageUrl="~/StudentBinder/img/Plus.png" OnClick="imgCreateEqutn_Click" />
                                                                <asp:ImageButton ID="imageCollapseDiv" Height="20px" Visible="false" Width="20px" Style="vertical-align: middle;" ImageUrl="~/StudentBinder/img/minus.png" runat="server" OnClick="imageCollapseDiv_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkTextCustomizeIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblCalcuType">Calculation Type</asp:Label>
                                                            </td>
                                                            <td style="text-align:center">
                                                                <div id="CalcuType" runat="server">
                                                                    <asp:RadioButtonList ID="rbtnCalcuType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnCalcuType_SelectedIndexChanged" AutoPostBack="true">
                                                                        <asp:ListItem Value="0">Custom Formula</asp:ListItem>
                                                                        <asp:ListItem Value="1" Selected="True">Open Text</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td style="text-align:center">
                                                                <asp:TextBox ID="txtCalcuType" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">Customized Calculation
                                                            </td>

                                                            <td>
                                                                <div id="divBtn" style="width: 550px; min-height: 70px; height: auto !important; height: 70px; padding: 10px; margin: 25px auto 15px auto; border: 5px solid #b2ccca; background: #EFEFEF; float: left;" runat="server" visible="false">


                                                                    <%--<input type="button" id="Button19" style="width: 162px" value="OK" alt="OK" />--%>


                                                                    <div id="divColumn">
                                                                    </div>

                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </div>


                                                <div id="DurationDiv" runat="server" visible="false">
                                                    <asp:LinkButton ID="LinkButton6" runat="server" OnClientClick="return DurationDivdisplay()" style="color:black" Text="Advanced Option"><img src="images/A3.png" alt="Advanced option"  border="0" style="width:10px;height:10px;"/>&nbsp;Advanced Options</asp:LinkButton> 

                                                    <table id="durationAOTb" runat="server" style="width: 95%; display:none" >
                                                        <tr>
                                                            <td style="width: 30%; vertical-align: top; text-align:right" class="tdText" title="What text or number will be scored correct?">What text or number will be scored correct?:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDurCorrectResponse" runat="server"></asp:TextBox>
                                                                 <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored correct?</span></div>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style="width: 40%; vertical-align: top; text-align:right" class="tdText" title="What text or number will be scored incorrect?">What text or number will be scored incorrect?:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDurIncrctResp" runat="server"></asp:TextBox>
                                                                 <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored Incorrect?</span></div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                             <td style="width: 40%; vertical-align: central; text-align:right" class="tdText" title="Select Move up Option">Select Move Up option:
                                                            </td>
                                                            <td> <asp:DropDownList ID="MoveupOpt1" runat="server" style="height:24px;width:75%;border:1px solid #d7cece;border-radius:3px"><asp:ListItem Enabled="true" Text="Less than" Value="1"></asp:ListItem>
                                                                       <asp:ListItem Text="More than" Value="0"></asp:ListItem></asp:DropDownList></td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td class="tdText">
                                                                <b>Mistrial</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Mistrial Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td>
                                                                <asp:CheckBox ID="chkDurIncludeMistrial" Text="Inc.Mis trial" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDurInclMisTrial" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                     <div style="background-color:lightblue">
                                                        <p style="padding-top:1%;"><h2 style="color:black">Scoring</h2><i><b><font color="black"> Must </font> </b> choose one or more - &nbsp<b><font color="black">these are intelligent &nbsp</font></b>-&nbsp they calculate properly based on the teaching method</i><p>
                                                            </p>
                                                            <br />
                                                         </p>
                                                            
                                                    </div>
                                                    <table class="tableMeasure">
                                                        <tr>
                                                            <td style="border-right: 1px double silver; text-align:center; width:33%">
                                                                <b>Formulas</b>
                                                            </td>
                                                            <td class="tdText" style="text-align:center; color: black">
                                                                <b>What do you want to call this <br />measure</b>
                                                                <br /><i>Leave blank for default name </i>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Include in <br />Graph</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="border-right: 1px double silver; text-align:center">
                                                                <asp:CheckBox ID="chkDurAverage" Text="Avg Duration" runat="server" onchange="checkForIIG(this);" />
                                                            </td>
                                                            <td style="text-align:center">
                                                                <asp:TextBox ID="txtDurAverage" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkDurAverageIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style="border-right: 1px double silver; text-align:center">
                                                                <asp:CheckBox ID="chkDurTotalDur" Text="Total Duration" runat="server" onchange="checkForIIG(this);" />
                                                            </td>
                                                            <td style="text-align:center">
                                                                <asp:TextBox ID="txtDurTotalDuration" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkDurTotalDurIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>


                                                    </table>

                                                </div>


                                                <div id="FrequencyDiv" runat="server" visible="false">
                                                    <asp:LinkButton ID="LinkButton7" runat="server" OnClientClick=" return FrequencyDivdisplay()" style="color:black" Text="Advanced Option"><img src="images/A3.png" alt="Advanced option"  border="0" style="width:10px;height:10px;"/>&nbsp;Advanced Options</asp:LinkButton> 

                                                    <table id="frequencyAOTb" runat="server" style="width: 95% ;display:none" >
                                                        <tr>
                                                            <td style="width: 40%; vertical-align: top; text-align:right" class="tdText" title="What text or number will be scored correct?">What text or number will be scored correct?:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFreqCorrectResponse" runat="server"></asp:TextBox>
                                                                <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored correct?</span></div>
                                                            </td>
                                                        </tr>


                                                        <tr>
                                                            <td style="width: 30%; vertical-align: top; text-align:right" class="tdText" title="What text or number will be scored Incorrect?">What text or number will be scored incorrect?:
                                                            </td>
                                                            <td >
                                                                <asp:TextBox ID="txtfreqIncrctResp" runat="server"></asp:TextBox>
                                                                <div class="toolTipImg"><img src="images/toolTipQMark.png" style="height:14px;width:14px" /></div><div class="tooltiptext"><span>What text or number will be scored incorrect?</span></div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                             <td style="width: 40%; vertical-align: central; text-align:right" class="tdText" title="Select Move up Option">Select Move Up option:
                                                            </td>
                                                            <td> <asp:DropDownList ID="MoveupOpt" runat="server" style="height:24px;width:75%;border:1px solid #d7cece;border-radius:3px"><asp:ListItem Enabled="true" Text="Less than" Value="1"></asp:ListItem>
                                                                       <asp:ListItem Text="More than" Value="0"></asp:ListItem></asp:DropDownList></td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td class="tdText">
                                                                <b>Mistrial</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Mistrial Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td>
                                                                <asp:CheckBox ID="chkFreqIncludeMistrial" Text="Include Mistrial" runat="server" />
                                                            </td>
                                                            <td >
                                                                <asp:TextBox ID="txtFreqIncludeMistrial" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div style="background-color:lightblue">
                                                        <p style="padding-top:1%;"><h2 style="color:black">Scoring</h2><i><b><font color="black"> Must </font> </b> choose one or more - &nbsp<b><font color="black">these are intelligent &nbsp</font></b>-&nbsp they calculate properly based on the teaching method</i><p>
                                                            </p>
                                                            <br />
                                                        </p>
                                                            
                                                    </div>
                                                    <table class="tableMeasure">
                                                        <tr>
                                                            <td style="border-right: 1px double silver; text-align:center; color:black; width:33%">
                                                                <b>Formulas</b>
                                                            </td>
                                                            <td class="tdText" style=" text-align:center; color: black">
                                                                <b>What do you want to call this<br /> measure</b><br />
                                                               <i> Leave blank for default name </i>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Include in <br /> Graph</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="border-right: 1px double silver;text-align:center">
                                                                <asp:CheckBox ID="chkFrequency" Text="Frequency" runat="server" onchange="checkForIIG(this);" />
                                                            </td>
                                                            <td style="text-align:center">
                                                                <asp:TextBox ID="txtFrequency" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkFrequencyIIG" runat="server" CssClass="tdText" Enabled="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td colspan="2" style="padding-left: 230px">



                                                <asp:Button ID="BtnSaveMeasure" runat="server" CssClass="NFButton" Text="Save" OnClick="BtnSaveMeasure_Click" OnClientClick=" return ValidateMeasure(this)" />
                                                <asp:Button ID="BtnUpdateMeasure" runat="server" CssClass="NFButton" Text="Update" Visible="false" OnClick="BtnUpdateMeasure_Click" OnClientClick=" return ValidateMeasure(this)" />
                                                <asp:HiddenField ID="Hdfsavemeasure" runat="server" Value="" />



                                            </td>

                                        </tr>






                                    </table>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>


                    </div>

                </div>





                <div id="SortingDiv" style="width: 100%; height: 100%;">
                    <div id="AddSortDiv" class="web_dialog" style="top: 6%; left: 6%;">

                        <div id="signup_sort">
                            <a id="close_sort" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                            <h3>Sorting Container
               
                            </h3>
                            <hr />

                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>

                                    <table style="width: 100%;">


                                        <tr>

                                            <td class="tdText" style="width: 15%;">Select Parent Set
                                            </td>
                                            <td style="width: 50%;">

                                                <asp:DropDownList ID="ddlparentSetlist" runat="server" CssClass="drpClass" Width="318px" OnSelectedIndexChanged="ddlparentSetlist_SelectedIndexChanged" AutoPostBack="True">
                                                    <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>

                                                </asp:DropDownList>

                                            </td>

                                            <td style="width: 35%;"></td>
                                        </tr>


                                        <%--  <tr>
                                            <td class="tdText">Steps
                                            </td>

                                            <td>
                                                <asp:DropDownList ID="ddlStepList" runat="server" CssClass="drpClass" Width="318px">
                                                    <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>

                                                </asp:DropDownList>
                                            </td>

                                        </tr>--%>

                                        <tr>
                                            <td colspan="3" style="width: 100%;">

                                                <asp:DataList ID="dlistStepSorting" runat="server" Width="99%" OnItemDataBound="dlistStepSorting_ItemDataBound">
                                                    <ItemTemplate>

                                                        <div class="itomesContainer">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>

                                                                    <td style="width: 35%;">
                                                                        <asp:HiddenField ID="hdnStepID" Value='<%# Eval("DSTempStepId") %>' runat="server" />
                                                                        <h3>
                                                                            <asp:Label ID="lblStepName" runat="server" Text='<%#Eval("StepCd") %>'></asp:Label>

                                                                        </h3>
                                                                    </td>

                                                                    <td width="35%">
                                                                        <p>

                                                                            <asp:Label ID="lblStepDesc" runat="server" Text='<%#Eval("StepName") %>'></asp:Label>
                                                                        </p>
                                                                    </td>

                                                                    <td style="width: 30%;">

                                                                        <asp:DropDownList ID="ddlSortingstep" runat="server" CssClass="drpClass" Width="250px">
                                                                        </asp:DropDownList>
                                                                    </td>


                                                                </tr>

                                                            </table>
                                                        </div>

                                                    </ItemTemplate>
                                                </asp:DataList>

                                            </td>
                                        </tr>


                                        <tr>

                                            <td>
                                                <asp:Button ID="BtnUpdateSortorder" runat="server" Text="Update" CssClass="NFButton" OnClick="BtnUpdateSortorder_Click" />
                                            </td>
                                        </tr>





                                    </table>



                                </ContentTemplate>
                            </asp:UpdatePanel>


                            <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                        </div>



                        <div id="Div6"></div>

                    </div>

                </div>



                <div id="loadingDiv" style="height: 200px; width: 60%; left: 20%; display: none;">
                    <img src="img/Loadimg.gif" style="height: 50px; width: 50px;" />
                </div>


                <div class="fullOverlay">
                </div>

                <div class="clear"></div>
            </div>
        </div>
        <div id="overlay" class="web_dialog_overlay"></div>

        <%--new--%>

        <div id="DilogAproveLP" runat="server" class="web_dialog" style="width: 710px; top: -20%;">

            <div id="DivApLp" style="width: 700px; margin-top: 16px">

                <%--<a id="ApLp" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;" onclick="hideApprRej();">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: -12px; margin-top: -20px; z-index: 300" width="18" height="18" alt="" /></a>--%>
                <h3>Lesson Plan New Version</h3>
                <hr />
                <table width="100%" cellpadding="0" cellspacing="0" style="text-align: left;">

                    <tr>
                        <td width="20%">Reason
                        </td>
                        <td width="80%">
                            <asp:TextBox ID="txtApLPReason" runat="server" CssClass="textClass" Rows="5"
                                TextMode="MultiLine" Width="80%" Height="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 20px"></td>
                        <td style="height: 20px">
                            <div id="NewLPValidMsg" style="visibility: hidden; color: red;">
                                Reason cannot be empty
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%"></td>
                        <td width="80%">
                            <asp:Button ID="btnAddApLp" runat="server" Text="Add" CssClass="NFButton" OnClick="btnAddApLp_Click" OnClientClick="return checkNull(this)" />
                            <asp:Button ID="btnRjctApLp" runat="server" Text="Cancel" CssClass="NFButton" OnClick="btnRjctApLp_Click" />
                        </td>
                    </tr>
                </table>



            </div>
        </div>

        <div id="DivAlertAPPLP" runat="server" class="web_dialog" style="width: 410px; top: -20%;">

            <div id="Div10" style="width: 400px; margin-top: 16px">
                <table>
                    <tr>
                        <td colspan="2">A new version of this lesson is already started, you can find it in the ‘Lessons-Drafts’ or 'Lessons-Pending Approval' section.</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="OK" CssClass="NFButton" OnClientClick="" /></td>
                    </tr>
                </table>




            </div>
        </div>


        <%--new end--%>


        <div id="DilogAprove" class="web_dialog" style="width: 710px; top: -25%;">

            <div id="Div1" style="width: 700px; margin-top: 16px">

                <a id="A1" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;" onclick="closeReject();">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: -12px; margin-top: -20px; z-index: 300" width="18" height="18" alt="" /></a>

                <hr />
                <table width="100%" cellpadding="0" cellspacing="0" style="text-align: left;">

                    <tr>
                        <td width="20%">Reason
                        
                       
                        >
                        <td width="80%">
                            <asp:TextBox ID="txtReason" runat="server" CssClass="textClass" Rows="5"
                                TextMode="MultiLine" Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 20px"></td>
                    </tr>
                    <tr>
                        <td width="20%"></td>
                        <td width="80%">
                            <asp:Button ID="btnRejectConfirm" runat="server" Text="Reject" CssClass="NFButton" OnClick="BtnReject_Click" OnClientClick="return multiClickCheckReject(this);"/>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="NFButton" OnClientClick="closeReject();" />

                        </td>
                    </tr>
                </table>
            </div>
        </div>


        <div id="ViewRejectedNotes" class="web_dialog" style="width: 710px; top: -23%;">
            <div id="Div4" style="width: 700px; margin-top: 16px">
                <a id="A3" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;" onclick="hideRejectedNotes();">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: -12px; margin-top: -20px; z-index: 300" width="18" height="18" alt="" /></a>

                <h3 style="color: #6B6E74" >Rejected Notes</h3>
                <asp:UpdatePanel runat="server" ID="recentnotesid">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdRejectedNote" runat="server" AutoGenerateColumns="False"
                                        Width="100%" PageSize="5" AllowPaging="True"
                                        GridLines="None" CellPadding="4" EmptyDataText="No data available" OnPageIndexChanging="GrdRejectedNote_PageIndexChanging">
                                        <AlternatingRowStyle CssClass="AltRowStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="Note" HeaderText="Note">
                                                <ItemStyle Width="80%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RejectedDate" HeaderText="Date">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                        <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                        <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                                        <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />

                                        <RowStyle CssClass="RowStyle" />
                                        <SelectedRowStyle CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
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

            </div>
        </div>
        <div id="divPrmpts" class="web_dialog2">
            <a id="A2" onclick="HidePopup();" href="#" style="margin-top: -13px; margin-right: -14px;">
                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
            <%-- <asp:UpdatePanel ID="UpdatePanel38" runat="server" UpdateMode="conditional">
            <Triggers>
                <asp:PostBackTrigger ControlID="btUpload" />
            </Triggers>
            <ContentTemplate>--%>
            <br />
            <h4>Add Lesson Plan Documents/Video*</h4><h5><p style="color:seagreen">[*Maximum video size permitted is 20MB or less.Supporting extensions like.pdf and .gif. Work is in progress for other file extensions]</p></h5>

            <hr />

            <br />
            <asp:UpdatePanel runat="server" ID="updateFile" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divMessage" runat="server" style="width: 98%"></div>
                    <div style="width: 100%; padding-left: 10px;">
                        <asp:FileUpload ID="fupDoc" runat="server"/>
                        <%--<asp:Button ID="btRefresh" runat="server" Text="Upload" OnClick="btRefresh_Click" />--%>
                        <asp:CustomValidator ID="customValidatorUpload" runat="server" ErrorMessage="" ControlToValidate="fupDoc" ClientValidationFunction="setUploadButtonState();" />
                        <asp:Button ID="btUpload" runat="server" Text="Upload" OnClick="btUpload_Click" CssClass="NFButton"  />                        <%--<asp:Button ID="btRefresh" runat="server" Text="Upload" OnClick="btRefresh_Click" />--%>
                        <br />
                        <asp:Label ID="lMsg" runat="server" />

                    </div>
                    <br />
                    <asp:GridView GridLines="none" CellPadding="4" ID="grdFile" PageSize="5" AllowPaging="True" Width="100%" OnRowEditing="grdFile_RowEditing" OnRowCommand="grdFile_RowCommand" AutoGenerateColumns="false" runat="server" OnPageIndexChanging="grdFile_PageIndexChanging" OnRowDataBound="grdFile_RowDataBound">
                        <Columns>


                            <asp:BoundField DataField="No" HeaderText="No" HeaderStyle-Width="10px" />

                            <asp:TemplateField HeaderText="Document/Video Name">
                                <ItemTemplate>
                                    <asp:LinkButton CommandName="download" ID="lnkDownload" Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("LPDoc") %>' ToolTip='<%# Eval("Document")%>' runat="server"></asp:LinkButton>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="15px">
                                <ItemTemplate>
                                    <asp:ImageButton OnClientClick="javascript:return deleteDoc(this);" ID="lb_edit" runat="server" class="btn btn-red" CommandArgument='<%# Eval("LPDoc") %>' CommandName="Edit" ImageUrl="~/Administration/images/trash.png" Width="18px" />
                                </ItemTemplate>
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
                <Triggers>
                    <asp:PostBackTrigger ControlID="btUpload" />
                    <asp:PostBackTrigger ControlID="grdFile" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="DatasheetContainer" style="width: 100%; height: 100%;">
            <div id="DatasheetContainerDiv" class="web_dialog" style="top: 2%; left: 2%; padding-top: 1%;">

                <div id="Div2">
                    <a id="close" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;" onclick="closeDatasheet();">
                        <img src="../Administration/images/clb.PNG" style="position: absolute; left: 99%; top: -9px;" width="18" height="18" alt="" />
                    </a>
                    <h3>Datasheet Preview
               
                                    <iframe id="ifrmeDatasheet" style="width: 100%; height: 900px; overflow-y: auto;" scrolling="auto"></iframe>
                </div>
                <div id="Div8"></div>

            </div>

        </div>

        <div id="popupMain" style="display: none; position: fixed; top: 0; left: 350px; background-color: whitesmoke; border: 2px solid black">
            <div style="width: 100%; height: 30px;">
                <a>
                    <img src="../Administration/images/button_red_close.PNG" onclick="javascript:$('#popupMain').fadeOut();" style="float: right; height: 25px; width: 25px" /></a>
            </div>
            <div id="popupSub" style="width: 650px; height: 450px; overflow-y: scroll;">
                <div style="margin: 0 10px 10px 10px;">
                    <h2>Lesson Plan Order</h2>
                </div>
                <div style="text-align: left; margin: 10px  10px ">

                 <span>Lesson Plan Name</span>                                             
                    <asp:DropDownList ID="ddlLessonname" style="text-align: left" runat="server" CssClass="drpClass" width="240px" Enabled="true">                                                                               
                    <%--<asp:ListItem Value="0">---Select Lesson Plan---</asp:ListItem>--%>
                    </asp:DropDownList>&nbsp;&nbsp;
                
                    <span>Insert Before Lesson Plan</span>                                             
                    <asp:DropDownList ID="ddlLessonorder" style="text-align: left" runat="server" CssClass="drpClass" width="110px" Enabled="true">                                                                               
                    <%--<asp:ListItem Value="0">---Select Order---</asp:ListItem>--%>
                    </asp:DropDownList>
                  
                    </div>
                <asp:DataList ID="dlLPforSorting" runat="server" RepeatLayout="Flow">
                    <ItemTemplate>
                        <div class="LPBox">
                            <asp:Label ID="Label3" runat="server" Width="25px" style="text-align: right" Text='<%# Eval("checkmaint") %>' ></asp:Label>
                            <asp:TextBox class="lblSort" ID="lblSNo" runat="server" BackColor="WhiteSmoke" BorderColor="WhiteSmoke" Height="25px" Width="25px" Style="text-align: left" Text='<%# Eval("SNo") %>'></asp:TextBox>
                            <asp:Label ID="lblLPs" runat="server" Width="365px" Text='<%# Eval("DSTemplateName") %>'></asp:Label>
                            <img src="images/UPArrow.png" style="height: 25px; width: 25px; cursor: pointer;" onclick="goUp(this);" />
                            <img src="images/DownArrow.png" style="height: 25px; width: 25px; cursor: pointer;" onclick="goDown(this);" />
                            <asp:HiddenField ID="hfLPOrder" runat="server" Value='<%#Eval("LessonOrder") %>' />
                            <asp:HiddenField ID="hfLPId" runat="server" Value='<%#Eval("LessonPlanId") %>' />
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label Visible='<%#bool.Parse((dlLPforSorting.Items.Count==0).ToString())%>' runat="server" ID="lblNoRecord" Text="&nbsp;&nbsp;&nbsp;No Data Found!"></asp:Label>
                    </FooterTemplate>
                </asp:DataList>
            </div>
            <div style="text-align: center; margin: 10px 0 10px 0">
                <asp:Button runat="server" class="NFButton" ID="btnSaveOrder" Text="Save Order" OnClick="btnSaveOrder_Click" OnClientClick="return multiClickCheck(this);" />
            </div>
            <div style="text-align: right;margin-right:5px;margin-bottom:5px;font-weight:bold;">*Maintenance Lesson</div>
        </div>
        <asp:HiddenField ID="hdLessonName" runat="server" />

        <div id="divOverride" class="web_dialog22">
            <div class="hdnFields">
                <asp:HiddenField runat="server" ID="hdnRadBtnStep" />
                <asp:HiddenField runat="server" ID="hdnRadBtnSet" />
                <asp:HiddenField runat="server" ID="hdnRadBtnPrompt" />
            </div>
            <img onclick="HidePoupOverride();" style="position: absolute; width: 25px; height: 25px; z-index: 9999; left: 96.5%; margin-top: 1px;" src="../Administration/images/button_red_close.png" />
            <div>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td colspan="3">
                                    <div id="divOver" runat="server" style="height: 300px; overflow-y: auto; overflow-x: hidden;">

                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <div  id="divApprMsg" class='warning_box' style="display:none"></div>
                                                </td>
                                            </tr>
                                            <tr style="font-size: small;">
                                                <td><h3>The prior revision of this lesson plan was on:</h3><br />
                                                    <br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;Set :
                                                    <asp:Label ID="lblsetrev" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;Step :
                                                    <asp:Label ID="lblsteprev" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;Prompt :
                                                    <asp:Label ID="lblpromptrev" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            
                                                <td>
                                                    <asp:Panel runat="server" ID="totalTaskOverride">
                                                        <table style="width: 100%">
                                                            <thead>
                                                                  <tr style="font-size: small;">
                                                                       <td colspan="2"><h3>Please choose the starting levels for this lesson:</h3><br></br>
                                       
                                                  
                                                                <tr>
                                                                    <td style="text-align: left; width: 33%; min-width: 200px;">Set(s) </td>
                                                                    <td style="text-align: left; width: 33%; min-width: 200px;">Step(s) </td>
                                                                    <td style="text-align: left; width: 33%; min-width: 200px;">Prompt(s) </td>
                                                                </tr>
                  
                                                             </tr>
                                                            </thead>

                                                            <style>
                                                                .setDiv, .stepDiv {
                                                                    background-color: #cdcdcd;
                                                                    border: 1px solid #a8a8a8;
                                                                    padding: 3px;
                                                                    float: left;
                                                                    width: 100%;
                                                                }

                                                                .stepDiv_sel {
                                                                    background-color: pink;
                                                                }

                                                                .setCheckbox, .stepCheckbox {
                                                                    display: none;
                                                                    float: left;
                                                                }

                                                                .stepSpan {
                                                                    float: left;
                                                                    width: 315px;
                                                                }

                                                                .stepDDL {
                                                                    float: right;
                                                                    width: 150px;
                                                                }
                                                            </style>
                                                            <script>
                                                                function setChanged(elem) {
                                                                    var prevVal = $(elem).parent().find('.promptId').val();
                                                                    var newVal = $(elem).val();
                                                                    if (prevVal == newVal) {
                                                                        $(elem).parent().find('.stepCheckbox').find('[type=checkbox]').prop('checked', false);
                                                                        $(elem).parent().removeClass('stepDiv_sel');
                                                                    }
                                                                    else {
                                                                        $(elem).parent().find('.stepCheckbox').find('[type=checkbox]').prop('checked', true);
                                                                        $(elem).parent().addClass('stepDiv_sel');
                                                                    }
                                                                }
                                                            </script>
                                                            <tr>

                                                                <td style="vertical-align: top;">
                                                                    <asp:RadioButtonList ID="RadioButtonListSets_tt" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListSets_tt_SelectedIndexChanged"></asp:RadioButtonList>
                                                                    <asp:Repeater ID="rptr_ListSets" runat="server" Visible="false">
                                                                        <HeaderTemplate>
                                                                            <table style="width: 250px;">
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:LinkButton ID="btn_sets" runat="server" CssClass="setDiv" OnClick="btn_sets_Click" ToolTip='<%# Eval("DSTempSetId") %>'><%# Eval("SetName") %></asp:LinkButton>
                                                                                    <asp:HiddenField runat="server" ID="hdn_stepId" Value='<%# Eval("DSTempSetId") %>' />

                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>

                                                                    </asp:Repeater>
                                                                </td>
                                                                <td colspan="2" style="vertical-align: top;">
                                                                    <asp:Repeater ID="rptr_listStep" runat="server" OnItemDataBound="rptr_listStep_ItemDataBound">
                                                                        <HeaderTemplate>
                                                                            <table style="width: 100%;">
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="stepDiv">
                                                                                        <asp:HiddenField runat="server" ID="hdn_dsTempHdrId" Value='<%# Eval("DSTempHdrId") %>' />
                                                                                        <asp:HiddenField runat="server" ID="hdn_stepId" Value='<%# Eval("DSTempStepId") %>' />
                                                                                        <asp:HiddenField runat="server" ID="hdn_stepDDLValue" Value='<%# Eval("PromptId") %>' />
                                                                                        <input type="text" class="promptId" style="display: none;" value='<%# Eval("PromptId") %>' />

                                                                                        <asp:CheckBox ID="stepCheckBox" runat="server" CssClass="stepCheckbox" Text='<%# Eval("DSTempStepId") %>' />
                                                                                        <div class="stepSpan"><%# Eval("StepCd") %></div>
                                                                                        <asp:DropDownList ID="stepDDL" runat="server" CssClass="stepDDL" onchange="setChanged(this);"></asp:DropDownList>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel runat="server" ID="normalOverride">
                                                        <table style="width: 100%">
                                                            <thead>
                                                                 <tr style="font-size: small;">
                                                                       <td colspan="2"><h3>Please choose the starting levels for this lesson:</h3><br></br>
                                   
                                                                <tr>
                                                                    <td style="text-align: left; width: 33%; min-width: 200px;">Set(s) </td>
                                                                    <td style="text-align: left; width: 33%; min-width: 200px;">Step(s) </td>
                                                                    <td style="text-align: left; width: 33%; min-width: 200px;">Prompt(s) </td>
                                                                </tr>
                                                                           </td>
                                                                     </tr>
                                                            </thead>
                                                            <tr>
                                                                <td style="vertical-align: top">
                                                                    <asp:RadioButtonList ID="RadioButtonListSets" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListSets_SelectedIndexChanged"></asp:RadioButtonList></td>
                                                                <td style="vertical-align: top">
                                                                    <asp:RadioButtonList ID="RadioButtonListSteps" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListSteps_SelectedIndexChanged" runat="server"></asp:RadioButtonList></td>
                                                                <td style="vertical-align: top">
                                                                    <asp:RadioButtonList ID="RadioButtonListPrompts" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListPrompts_SelectedIndexChanged" runat="server"></asp:RadioButtonList></td>
                                                            </tr>
                                                            



                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr style="text-align: center">
                                                <td></td>
                                            </tr>
                                        </table>





                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; width: 33%; min-width: 200px;"></td>
                                <td style="text-align: center; width: 33%; min-width: 200px;">
                                   
                                   

                                    <%--<asp:Button ID="btnPriorLevel" Width="75px" runat="server" Text="Save" CssClass="NFButton" OnClick="btnPriorLevel_Click" OnClientClick="return ValidateRadio();" />--%>
                                    <asp:Button ID="btnPriorLevel" Width="75px" runat="server" Text="Save" CssClass="NFButton" OnClick="btnEmailClick" OnClientClick="return ValidateRadio(this);" />
                                    <asp:Button ID="btnResetLevel" Width="150px" runat="server" Text="Start from beginning" CssClass="NFButton" OnClick="btnResetLevel_Click" style="display: none;"  />

                                </td>
                                <td style="text-align: center; width: 33%; min-width: 200px;"></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnPriorLevel" />
                        <asp:PostBackTrigger ControlID="btnResetLevel" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>


<script type="text/javascript">
    //$('#btnAddSet').click(function () {

    //    $('.fullOverlay').empty();
    //    $('.fullOverlay').fadeIn('slow', function () {
    //        //document.getElementById('previewFrame').style.height = 1000;          
    //        //  $('#previewBoard').css('display', 'block');
    //        $('#AddSetDiv').fadeIn();
    //        //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
    //        //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
    //    });
    //});




    $('#close_x').click(function () {
        $('#AddSetDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    });

    $('#close_Step').click(function () {
        $('#AddStepDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    });


    $('#close_sort').click(function () {
        $('#AddSortDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    });



    //$('#BtnAddMeasure').click(function () {

    //    $('.fullOverlay').empty();
    //    $('.fullOverlay').fadeIn('slow', function () {
    //        //document.getElementById('previewFrame').style.height = 1000;          
    //        //  $('#previewBoard').css('display', 'block');
    //        $('#AddMeasureDiv').fadeIn();
    //        //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
    //        //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
    //    });
    //});
    //$('#Close1').click(function () {
    //    $('#AddMeasureDiv').fadeOut('slow', function () {
    //        $('.fullOverlay').fadeOut('fast');
    //    });
    //});
    function close1Click() {
        $('#AddMeasureDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }

    $('#close_CriteriaPopup').click(function () {
        $('#AddCriteriaDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    });

    $('#close_email').click(function () {
        $('#popupEmailMain').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    });

    $('#lnkApprovedLessons').click(function () {
        // alert('asdasdasd');

    });


    //$('#BtnAddMatchSamples').click(function () {
    //    var txtValue = $('#txtMatchtoSamples').val();
    //    var nwDiv = '<div class="mSamples" style="height:auto;width:auto;border:1px black groove;float:left;background-color:gray; margin:4px; font-color:white;">' + txtValue + '</div>';
    //    $('#divMatchtoSamples').append(nwDiv);
    //});

    //$('#btnAddSetCriteria').click(function () {

    //    $('.fullOverlay').empty();
    //    $('.fullOverlay').fadeIn('slow', function () {
    //        //document.getElementById('previewFrame').style.height = 1000;          
    //        //  $('#previewBoard').css('display', 'block');
    //        $('#AddCriteriaDiv').fadeIn();
    //        //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
    //        //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
    //    });

    //});



</script>
<script type="text/javascript">


    //function tabChange() {




    //}


    function CloseMeasure() {

        $('#AddMeasureDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }

    function EditMeasurePopup() {

        $('.fullOverlay').empty();
        $('.fullOverlay').fadeIn('slow', function () {
            //document.getElementById('previewFrame').style.height = 1000;          
            //  $('#previewBoard').css('display', 'block');
            $('#AddMeasureDiv').fadeIn();
            //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });
    }


    function AddSet() {

        $('.fullOverlay').empty();
        $('.fullOverlay').fadeIn('slow', function () {
            //document.getElementById('previewFrame').style.height = 1000;          
            //  $('#previewBoard').css('display', 'block');
            $('#AddSetDiv').fadeIn();
            //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });
    }


    function AddSort() {

        $('.fullOverlay').empty();
        $('.fullOverlay').fadeIn('slow', function () {
            //document.getElementById('previewFrame').style.height = 1000;          
            //  $('#previewBoard').css('display', 'block');
            $('#AddSortDiv').fadeIn();
            //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });
    }

    function AddStep() {

        $('.fullOverlay').empty();
        $('.fullOverlay').fadeIn('slow', function () {
            //document.getElementById('previewFrame').style.height = 1000;          
            //  $('#previewBoard').css('display', 'block');
            $('#AddStepDiv').fadeIn();
            //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });
    }

    function CloseSetPopup() {
        $('#AddSetDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }


    function CloseSorttPopup() {
        $('#AddSortDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }

    function CloseStepPopup() {
        $('#AddStepDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }

    function CloseCriteriaPopup() {
        $('#AddCriteriaDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }

    function CloseEmailPopup() {
        $('#popupEmailMain').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }

    function CloseSortingPopup() {
        $('#AddSortDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }


    function AlertNotDelete() {
        // alert('No Deletion Possible in this Criteria!!!');

        //alertMessage('No Deletion Possible in this Criteria!!!', 'red');
        alert('Cannot delete measurement because it is used  in Criteria Calculations. Please remove the measurement from each criteria first by deleting or changing the measurement of the criteria.');

    }
    
    //function AlertSuccessMsg() {
    //    //alert('Data Updated Successfully...');
    //    alertMessage('Data Updated Successfully...', 'green');
    //    document.getElementById("<%=txtBoxAddSet.ClientID%>").focus();
    //}   

    function AlertConvertDisc() {
        // alert('Sorry..Current lessonplan cannot convert back to chained type..Its defined some values in step details');
        alertMessage('Sorry..Current lessonplan cannot convert back to chained type..Its defined some values in step details', 'red');
    }

    function AlertConvertTotalTask() {
        //alert('Sorry..Current lessonplan cannot convert back.Its defined some values in step criteria details');
        alertMessage('Sorry..Current lessonplan cannot convert back.Its defined some values in step criteria details', 'red');
    }


    function AlertSelectPrompt() {
        // alert('Select any Prompt Procedure');
        alertMessage('Select any Prompt Procedure', 'red');
    }

    function AlertFailedMsg() {
        //alert('Sorry..Data Updation Failed...');
        alertMessage('Sorry..Data Updation Failed...', 'red');
    }
    function ValidateNoofTrials() {
        // alert('Please enter no: of trials..');
        alertMessage('Please enter no: of trials..', 'red');
    }

    function ValidateTeachingProc() {
        // alert('Please select any Teaching Procedure..');
        alertMessage('Please select any Teaching Procedure..', 'red');
    }

    function AlertPromptValid() {
        // alert('Sorry.. Current LessonPlan is assigned with a prompt measure.\n So deletion not possible...');
        alertMessage('Sorry.. Current LessonPlan is assigned with a prompt measure.\n So deletion not possible...', 'red');
    }



    function ValidateDrpclass() {
        // alert('Please select Chain Type..');
        alertMessage('Please select Chain Type..', 'red');
    }

    function AlreadyPromptDef() {
        // alert('Already Prompt defined');
        alertMessage('Already Prompt defined', 'red');
    }

    function ValidateSubmit() {
        // alert('Please Complete Template Details before submitting');
        alertMessage('Please Complete Template Details before submitting', 'red');
    }
    function SubmitSuccess() {
        // alert('Template Editor Successfully Submitted...');
        alertMessage('Template Editor Successfully Submitted...', 'green');
    }

    function AproveSuccess() {
        // alert('Template Editor Successfully Approved...');
        alertMessage('Template Editor Successfully Approved...', 'green');
    }

    function MsgSessExprd() {
        // alert('Sorry...Session Expired...Please try again!!!');
        alertMessage('Sorry...Session Expired...Please try again!!!', 'red');
    }

    function MsgLPNameAlreadyExists() {
        alertMessage('Lesson plan name already exist. Please enter another name...', 'red');
    }
    function NoDate() {
        alertMessage('Please enter Lesson Date ...', 'red');
    }
    function deleteSystem() {
        var flag;
        flag = confirm("Are you sure you want to delete this Column ?");
        return flag;
    }


    function deleteSet() {
        var flag;
        flag = confirm("Are you sure you want to delete this Set ?");
        if (flag.toString() == "true") {
            $('.loading').css("display", "block");
        }
        return flag;
    }
    function deleteStep() {
        var flag;
        flag = confirm("Are you sure you want to delete this Step ?");
        if (flag.toString() == "true") {
            $('.loading').css("display", "block");
        }
        return flag;
    }
    function deleteStepCriteria() {
        var flag;
        flag = confirm("Are you sure you want to delete this StepCriteria ?");
        return flag;
    }
    function deletePromptCriteria() {
        var flag;
        flag = confirm("Are you sure you want to delete this PromptCriteria ?");
        return flag;
    }
    function deleteSetCriteria() {
        var flag;
        flag = confirm("Are you sure you want to delete this SetCriteria ?");
        return flag;
    }

    function AddCriteriaPopup() {
        $('.fullOverlay').empty();
        $('.fullOverlay').fadeIn('slow', function () {
            //document.getElementById('previewFrame').style.height = 1000;          
            //  $('#previewBoard').css('display', 'block');
            $('#AddCriteriaDiv').fadeIn();
            //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });
    }



    //function to Restrict input to textbox: allowing only numbers...
    function isNumber(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    // function to validate Measure Data.

    function ValidateMeasure(btn) {
        if (!multiClickCriteria) {
            if (document.getElementById("<%=txtColumnName.ClientID%>").value == "") {
                document.getElementById("<%=tdMsgMeasure.ClientID%>").innerHTML = "<div class='warning_box'>Column Name Field can not be blank</div> ";
                document.getElementById("<%=txtColumnName.ClientID%>").focus();
                return false;
            }
            multiClickCriteria = true;
            btn.style.opacity = '0.5';
            return true;
        }
        return false;
    }


    function ValidateSet(btn) {
        if (!multiClickSet) {
            var stName = document.getElementById("<%=txtBoxAddSet.ClientID%>").value;
            if (stName.trim() == "") {
                document.getElementById("<%=tdMsgSet.ClientID%>").innerHTML = "<div class='warning_box'>Set Name Field can not be blank</div> ";
                document.getElementById("<%=txtBoxAddSet.ClientID%>").focus();
                return false;
            }
            multiClickSet = true;
            btn.style.opacity = '0.5';
            return true;
        }
        document.getElementById("<%=tdMsgSet.ClientID%>").innerHTML = "<div class='warning_box'>Please wait...</div> ";
        return false;
    }

    function ValidateStep(btn) {
        if (!multiClickStep) {
            var stpName = document.getElementById("<%=txtStepName.ClientID%>").value;
            if (stpName.trim() == "") {
                document.getElementById("<%=tdMsgStep.ClientID%>").innerHTML = "<div class='warning_box'>Step Name Field can not be blank</div> ";
                document.getElementById("<%=txtStepName.ClientID%>").focus();
                return false;
            }
            multiClickStep = true;
            btn.style.opacity = '0.5';
            return true;
        }
        document.getElementById("<%=tdMsgStep.ClientID%>").innerHTML = "<div class='warning_box'>Please wait...</div> ";
        return false;
    }


    function ValidateCriteria(btn) {
        if (!multiClickCriteria) {
            if (document.getElementById("<%=ddlCriteriaType.ClientID%>").value == "MODIFICATION");
            {
                multiClickCriteria = true;
                btn.style.opacity = '0.5';
                return true;
            }

            if (document.getElementById("<%=ddlCriteriaType.ClientID%>").value == "") {
                document.getElementById("<%=tdMsgCriteria.ClientID%>").innerHTML = "<div class='warning_box'>Select any Criteria Type</div> ";
                document.getElementById("<%=ddlCriteriaType.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlTempColumn.ClientID%>").value == "") {
                document.getElementById("<%=tdMsgCriteria.ClientID%>").innerHTML = "<div class='warning_box'>Select any Column  Type</div> ";
                document.getElementById("<%=ddlTempColumn.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtRequiredScore.ClientID%>").value == "") {
                document.getElementById("<%=tdMsgCriteria.ClientID%>").innerHTML = "<div class='warning_box'>Required Score Field can not be blank</div> ";
                document.getElementById("<%=txtRequiredScore.ClientID%>").focus();
                return false;
            }
            else {
                var value = parseInt(document.getElementById("<%=txtRequiredScore.ClientID%>").value);
                if (value < 0 || value > 100) {
                    document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Enter Score Between 0 and 100</div> ";

                    return false;
                }
            }
            multiClickCriteria = true;
            return true;
        }
        document.getElementById("<%=tdMsgCriteria.ClientID%>").innerHTML = "<div class='warning_box'>Please wait...</div> ";
        return false;
    }

    function checkEmailSelection(btn) {
        if (!multiClickSubmit) {
            var isChecked = false;
            var emails = "";
            var emailMissing = "";
            var lbMsg = "";
            var RB1 = document.getElementById("<%=dlEmailforSorting.ClientID%>");
            var radio = RB1.getElementsByTagName("input");
            var persons = RB1.getElementsByTagName("label");
            for (var li = 0; li < radio.length; li++) {
                var email = radio[li + 1];//.valueOf("hdn_EmailValue");
                //alert(email.defaultValue);
                if (radio[li].checked && email.defaultValue != "") {
                    //alert(email);
                    isChecked = true;
                    //break;
                }
                if (radio[li].checked && radio[li - 1].defaultValue === "") {
                    emailMissing = emailMissing + persons[li - 1].textContent + ",";;
                }
            }
            if (emailMissing != null && emailMissing.length > 1) {
                emailMissing = emailMissing.substring(0, emailMissing.length - 1);
            }

            var listb1 = document.getElementById("<%= lstSelectdEmails.ClientID %>");
            var len1 = listb1.options.length;

            var listb2 = document.getElementById("<%= lstSelecteUserNames.ClientID %>");
            var len2 = listb2.options.length;

            if (len1 > 0 && len2 > 0) {
                isChecked = true;
            }
            else {
                if (isChecked) {
                    lbMsg = "Please confirm the selected emails";
                    isChecked = false;
                }
            }

            if (!isChecked) {
                $('#msgAlert').css("display", "block");
                if (lbMsg == "") {
                    $('#msgAlert').html('Please select a person with email to send');
                }
                else {
                    $('#msgAlert').html(lbMsg);
                }
                setTimeout(function () {
                    $("#msgAlert").css("display", "none");
                }, 4000);
            } else if (emailMissing != "") {
                var confirmVal = confirm('Following persons (' + emailMissing + ') emails are missing.do you want to continue?');
                if (confirmVal) {
                    multiClickSubmit = true;
                    btn.style.opacity = '0.5';
                }
                return confirmVal;
            }
            if (isChecked) {
                multiClickSubmit = true;
                btn.style.opacity = '0.5';
            }
            return isChecked;
        }
        return false;
    }

    function getValueBtn(attrbte) {

        var value = attrbte;
        var textValue = document.getElementById("<%=txtCalcuType.ClientID%>").value;
        if (textValue == 0) {
            textValue = "";
            document.getElementById("<%=txtCalcuType.ClientID%>").value = textValue + attrbte;
        }
        else {
            document.getElementById("<%=txtCalcuType.ClientID%>").value += attrbte;
        }

    }

    function backspace() {
        var length = document.getElementById("<%=txtCalcuType.ClientID%>").style.length;

        var newtext = document.getElementById("<%=txtCalcuType.ClientID%>").value;
        if (newtext.length > 0) {
            newtext = newtext.substring(0, newtext.length - 1);

            document.getElementById("<%=txtCalcuType.ClientID%>").value = newtext;
        }

    }
    function cAsgn() {
        document.getElementById("<%=txtCalcuType.ClientID%>").value = 0;

    }
    function closeDatasheet() {
        $('#DatasheetContainerDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }






</script>



<script type="text/javascript">


    $(function () {
        $('#ddlSetData').css('display', 'none');
        // Patch fractional .x, .y form parameters for IE10.
        if (typeof (Sys) !== 'undefined' && Sys.Browser.agent === Sys.Browser.InternetExplorer && Sys.Browser.version === 10) {
            Sys.WebForms.PageRequestManager.getInstance()._onFormElementActive = function Sys$WebForms$PageRequestManager$_onFormElementActive(element, offsetX, offsetY) {
                if (element.disabled) {
                    return;
                }
                this._activeElement = element;
                this._postBackSettings = this._getPostBackSettings(element, element.name);
                if (element.name) {
                    var tagName = element.tagName.toUpperCase();
                    if (tagName === 'INPUT') {
                        var type = element.type;
                        if (type === 'submit') {
                            this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                        }
                        else if (type === 'image') {
                            this._additionalInput = encodeURIComponent(element.name) + '.x=' + Math.floor(offsetX) + '&' + encodeURIComponent(element.name) + '.y=' + Math.floor(offsetY);
                        }
                    }
                    else if ((tagName === 'BUTTON') && (element.name.length !== 0) && (element.type === 'submit')) {
                        this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                    }
                }
            };
        }

    });

    function showOverride() {
        $("#divOverride").show();
    }
    function HidePoupOverride() {
        $('#divApprMsg').css("display", "none");
        $("#divOverride").hide();
    }

    function autoSave(status) {
        //status will be false only for BtnSubmit click
        if (status) {
            multiClickSubmit = false;
            document.getElementById("<%=BtnSubmit.ClientID%>").style.opacity = 1;
        }

        if (!multiClickSubmit) {
            var window_width = $(window).width();
            var box_width = (window_width / 2) - 100;

            if ($('#btnAddSet').is(":visible")) {
                $("#btnAddSet").trigger("click");
            }


            if ($('#BtnUpdate').is(":visible")) {

                $("#BtnUpdate").trigger("click");
                //$('body').append("<div class='topMessageBox' style='margin-left:" + box_width + "px'>Types of Instruction Saved</div>");
                //setTimeout(function () {
                //    $(".topMessageBox").remove()
                //}, 2000);
            }
            if ($('#btUpdateLessonProc').is(":visible")) {
                $("#btUpdateLessonProc").trigger("click");
                //$('body').append("<div class='topMessageBox' style='margin-left:" + box_width + "px'>Lesson Procedure Saved</div>");
                //setTimeout(function () {
                //    $(".topMessageBox").remove()
                //}, 2000);
            }
            if ($('#btUpdateMeasurement').is(":visible")) {
                $("#btUpdateMeasurement").trigger("click");
            }
            if ($('#btnUpdateset').is(":visible")) {
                $("#btnUpdateset").trigger("click");
            }
            if ($('#btnUpdatesteps').is(":visible")) {
                $("#btnUpdatesteps").trigger("click");
            }
            if ($('#btnupdateprompts').is(":visible")) {
                $("#btnupdateprompts").trigger("click");
            }

            if ($('#BtnUpdateLessonPlan').is(":visible")) {
                $("#BtnUpdateLessonPlan").trigger("click");

            }

            if ($('#btnCommentLessonInfo').is(":visible")) {
                $("#btnCommentLessonInfo").trigger("click");

            }

            if ($('#btnMeasurementSystems').is(":visible")) {
                $("#btnCommentLessonInfo").trigger("click");

            }
            if ($('#btnCommentTypeofInstr').is(":visible")) {
                $("#btnCommentLessonInfo").trigger("click");

            }
            if ($('#btncommentLessonProcedure').is(":visible")) {
                $("#btnCommentLessonInfo").trigger("click");

            }
            if ($('#btncommentPrompt').is(":visible")) {
                $("#btnCommentLessonInfo").trigger("click");

            }
            if ($('#btncommentStep').is(":visible")) {
                $("#btnCommentLessonInfo").trigger("click");

            }
            if ($('#btncommentset').is(":visible")) {
                $("#btnCommentLessonInfo").trigger("click");

            }
            if ($('#BtnSavePrompt').is(":visible")) {
                $("#BtnSavePrompt").trigger("click");

            }
            if ($('#btnsubmit').is(":visible")) {
                $("#HiddenAppr").trigger("click");
            }
            if ($('#<%=txtBoxAddSet.ClientID%>').length > 0) {
                document.getElementById("<%=txtBoxAddSet.ClientID%>").focus();
            }
            if (!status) {
                multiClickSubmit = true;
                document.getElementById("<%=BtnSubmit.ClientID%>").style.opacity = '0.5';
            }
            return true;
        }
        return false;
    }
    function showload() {
        document.getElementById('formatchngloader').style.display = 'block';
        var popup = document.getElementById('frmtchngpopup');
        popup.style.display = 'none';
    }
    function Hideload() {
        document.getElementById('formatchngloader').style.display = 'none';
        var popup = document.getElementById('frmtchngpopup');
        popup.style.display = 'none';
    }



</script>



</html>
