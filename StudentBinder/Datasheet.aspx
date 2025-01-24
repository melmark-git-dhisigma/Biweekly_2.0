<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/Datasheet.aspx.cs" Inherits="StudentBinder_Datasheet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Datasheet Print</title>


    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <script type="text/javascript" src="js/eye.js"></script>
    <script type="text/javascript" src="js/layout.js"></script>
    <link id="sizestylesheet" rel="stylesheet" type="text/css" href="../Administration/CSS/BehaviorScoreTab.css" />

    <script type="text/javascript" src="js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="js/jquery.smartTab.js"></script>

    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/DatasheetStyle.css" rel="stylesheet" id="sized" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />

    <script src="js/fastclick.js"></script>
     <script>
         window.location.hash = "no-back-button";
         window.location.hash = "Again-No-back-button";//again because google chrome don't insert first hash into history
         window.onhashchange = function () { window.location.hash = "no-back-button"; }
    </script> 
    <script>
        //Function for printing datasheet [01-07-2020] Dev1 start--
        function printpage() {
            window.print();
            setTimeout(function () { window.close(); printpage2(); }, 10000);
            document.getElementById('<%= printPrev.ClientID%>').value = '1';
        }

        var multiClickSubmit = false;
        var multiCLickPrint = false;
        var multiCLickDiscard = false;
        var multiCLickViewPrior = false;
        function enableButton() {
            multiClickSubmit = false;
            multiCLickPrint = false;
            multiCLickDiscard = false;
            multiCLickViewPrior = false;
        }
        function closecheck() {
            sessionStorage.setItem('DataCl', 'true');
            Checkclose.value = 'true';
        }
        function closecheckSubmit() {
                $('#div_overlay').show();
                sessionStorage.setItem('DataCl', 'true');
                Checkclose.value = 'true';
        }
        function printpage1() {
            document.getElementById('<%= sessDate.ClientID%>').style.visibility = "visible";
            jQuery('#imgtest').removeAttr('src');
            jQuery('#imgtest').show();
            var x = document.getElementById('<%= leftNav.ClientID%>');
            if (x) x.style.visibility = "hidden";
            var y = document.getElementById('<%= rightNav.ClientID%>');
            if (y) y.style.visibility = "hidden";
            UpperMenuButtonPanel.style.visibility = "hidden";
            SaveBeforePrint.style.visibility = "hidden";
        }
        function printpage2() {
            printComplete.style.visibility = "visible";
        }
        function SaveData() {
            SaveBeforePrint.style.visibility = "visible";
        }

        function printpageNO() {
            window.print();
            setTimeout(function () { window.close(); printpage2(); }, 10000)
            document.getElementById('<%= printPrev.ClientID%>').value = '0';
        }
        function printpage1NO() {
            jQuery('#imgtest').removeAttr('src');
            jQuery('#imgtest').show();
            var x = document.getElementById('<%= leftNav.ClientID%>');
            if (x) x.style.visibility = "hidden";
            var y = document.getElementById('<%= rightNav.ClientID%>');
            if (y) y.style.visibility = "hidden";
            UpperMenuButtonPanel.style.visibility = "hidden";
            SaveBeforePrint.style.visibility = "hidden";
        }
        //Function for printing datasheet [01-07-2020] Dev1 END--
    </script>

    <style type="text/css">
        input[type="text"] {
            width: 60%;
        }

        input[type="submit"] {
            -webkit-appearance: button;
            height: 40px !Important;
            font-size: 15px;
        }

        input[type="button"] {
            -webkit-appearance: button;
            height: 40px !Important;
            font-size: 15px;
        }

        .RowStyle td {
            padding: 0;
        }

        .MisClass {
            background-color: #FF0000;
            border-radius: 3px 3px 3px 3px;
            color: #FFFFFF;
            font-size: 15px;
            font-weight: bold;
            height: 19px !important;
            position: relative;
        }


        .web_dialog2 {
            display: none;
            position: fixed;
            min-width: 290px;
            min-height: 200px;
            left: 50%;
            margin-left: -162px;
            margin-top: 15px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 1001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
        }

        .web_dialog22 {
            display: none;
            position: fixed;
            min-width: 630px;
            min-height: 200px;
            left: 0;
            top: 0;
            margin: 1%;
            margin-top: 15px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 1001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
            width: 92%;
            height: auto;
            top: 9%;
        }

        .web_dialog {
            display: none;
            position: fixed;
            min-width: 350px;
            min-height: 200px;
            left: 30%;
            margin-left: -162px;
            margin-top: 15px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 1001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
            width: 550px;
        }


        #divOver label {
            background-color: transparent !important;
            border-radius: 0 !important;
            border-width: 0 !important;
            color: #000 !important;
            cursor: pointer !important;
            font-size: 12px !important;
            line-height: 15px !important;
            margin-right: 21px !important;
            padding-right: 1px !important;
            text-align: center !important;
            vertical-align: middle !important;
        }

        #divOver input[type="radio"] {
            display: block;
            float: left;
        }

        #divOver td {
            border: none;
        }
        .topTabtd {
        background-color: #0D668E;
        color:#ffffff;
        cursor: pointer;
        border-radius: 5px 5px 5px 5px;
        padding:5px;
        margin-left:5px;
        margin-bottom:10px;
        }
       
        .topTabtd:hover {
            background-color: #09646A;
            color: #ffffff;
        }
        .topTabtdSelected {
            background-color: #60c658;
            color: #ffffff;
            cursor: pointer;
            border-radius: 10px 10px 0 0;
        }
   .redBanner {
    width: 94%;
    clear: both;
    /*background: url(../images/warning.png) no-repeat left #fcfae9;*/
    border: 1px #e9e6c7 solid;
    background-position: 10px 1px;
    padding-left: 50px;
    padding-top: 10px;
    padding-bottom: 5px;
    text-align: left;
    color: white;
    background-color:red;
}
   .modal {
            display: none;
            position: fixed;
            z-index: 1;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgb(0,0,0);
            background-color: rgba(0,0,0,0.4);
        }
        .modal-content {
            background-color: #fefefe;
            margin: 15% auto;
            padding: 20px;
            border: 1px solid #888;
            width: 80%;
        }
        .close {
            color: #aaa;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }
        .close:hover,
        .close:focus {
            color: black;
            text-decoration: none;
            cursor: pointer;
        }
        .modal-header {
            background-color: #0d668e;
            color: white;
            padding: 10px;
            text-align: left;
        }
        .modal-footer {
            padding: 10px;
            text-align: center;
        }
        .btn {
            padding: 10px 20px;
            margin: 10px;
            cursor: pointer;
        }
        .btn-success {
            background-color: #28a745;
            color: white;
            border: none;
        }
        .btn-danger {
            background-color: #dc3545;
            color: white;
            border: none;
        }
    </style>

    <script type="text/javascript">

        //Make a grey color in the Inactive field

        $(document).ready(function () {

            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad != null) {
                window.addEventListener('load', function () {
                    new FastClick(document.body);
                }, false);
            }



            ColorIt();
            var setName = document.getElementById('<%=lblSet.ClientID%>');
            if (setName != null) {
                var set_Name = setName.innerHTML;
                var setNameSep = set_Name.split(',').join(', ');
                document.getElementById('<%=lblSet.ClientID%>').innerHTML = setNameSep;
            }




            var lftPartContainer_W = $('.lftPartContainer').width() + 15;
            // var lftPartContainer_L = $('.lftPartContainer').position().left;

            parent.setBehavPanelLeft(lftPartContainer_W);
        });

        function showTempOverrideCheck() {
            $('#btn_continue').trigger("click");
        }

        function showConfirmPopUp(param1, param2) {
            $('#lblNACols').text(param1);
            if (param2 == "Submit Scores") {
                $('#btnTSAndR').hide();
                $('#btnTSubmit').show();
            }
            else {
                $('#btnTSubmit').hide();
                $('#btnTSAndR').show();
            }
            $('#dashMain').fadeIn();
            $('#div_overlay').hide();
            sessionStorage.setItem('DataCl', 'false');
            Checkclose.value = 'false';
        }

        function hideConfirmPopUp() {
            $('#dashMain').fadeOut();
        }

        
        function ioatrueDiscard(ioastatu)
        {
            if (ioastatu == "True")
            {
                document.getElementById('popupioa').style.display = 'block';
                document.getElementById('btnDiscardYes').style.display = 'none';
                document.getElementById('discardMain').style.display = 'none';
            }
            else {
                document.getElementById('popupioa').style.display = 'none';
                document.getElementById('btnDiscardYes').style.display = 'block';
                document.getElementById('discardMain').style.display = 'block';
            }
                
        }

		function DisableButton()
		{
			document.getElementById('div_tempOverride').style.display = 'none';
		}
       
        function showDiscardPopUp() {
           
            $('#btnDiscardYes').show();
            $('#discardMain').fadeIn();
        }

      

        function hideDiscardPopUp() {
            $('#discardMain').fadeOut();
        }

        function hideConfirmPopUp2() {
            printComplete.style.visibility = "hidden";
            UpperMenuButtonPanel.style.visibility = "visible";
        }
        function triggerSubmitClick() {
            $('#btnSubmit1').trigger("click");
        }
        function triggerSaveConfirmClick() {
            $('#btnSaveConfirm').trigger("click");
        }
        function triggerSubmitConfirmClick() {
            $('#btnSubmitConfirm').trigger("click");
        }
        function triggerSubmitAndRepeatConfirmClick() {
            $('#btnSubmitAndRepeatConfirm').trigger("click");
        }
        function triggerSubmitAndRepeatClick() {
            $('#btnSubmitAndRepeat1').trigger("click");
        }
        function checkclN() {
            sessionStorage.setItem('DataCl', 'false');
        }

        function checkcl() {
            var clch = document.getElementById('Checkclose').value;
            if (clch == "true") {
                sessionStorage.setItem('DataCl', 'true');
            }
            else {
                sessionStorage.setItem('DataCl', 'false');
            }
        }
        
            function triggerDiscardClick(qStat) {
			//$('#btnDiscard').trigger("click");
		    
		    if (qStat != 0) {


		        alert("Discarded Successfully");
		        sessionStorage.setItem('DataCl', 'false');
		    }
            document.getElementById('discardMain').style.display = 'none';
            <%clsSession oSession = (clsSession)Session["UserSession"];%>
            var frameId = "<%=oSession.StudentId.ToString()%>";
            var name = frameId;
            //alert("TriggerDiscard:" + frameId);
            var newName = "sheetDiv-" + name + "-" + "0";
            var isSheet = $('#divContentPages').find('#divTF' + "0").find('#LeftDiv' + "0").children().size();




            var sheetId;
            parent.$("#divContentPages").find('#divTF' + name).find('#LeftDiv' + name).children().each(function (n, i) {

                if (parent.$(this).css('display') == 'block') {
                    sheetId = $(this).attr('id');
                    var valNew = sheetId.split('-');
                    sheetId = valNew[2];
                }

            })
            var newName;
            //alert("sheetId:" + sheetId);
            if (sheetId != null) { newName = "sheetDiv-" + name + "-" + sheetId.toString(); }

            

            parent.$('#contactForm').hide();
            
            parent.setName("");


            parent.$('.alpha').removeAttr('id');

            parent.$('.fillLessons').hide();
            parent.$('.fillLessons2').hide();

            parent.resetOverrideSessionfn(sheetId);
            closeIframe1(frameId);
            parent.listLessonPlan();
            HideHdrDiv();
            //parent.$('#divContentPages').find('#divTF' + frameId).remove();
            //parent.$('#ulStudents').find('#divStud' + frameId).remove();
            //parent.$('#ulStudents').find('#divAll' + frameId).remove();
		    // $('#btnDiscard').trigger("click");
          
			
		}
       

        function tempOverrideCheckBox_CheckedChanged() {
            var checkedTemp = false;
            var collection = document.getElementById('div_tempOverride').getElementsByTagName('input');
            for (var x = 0; x < collection.length; x++) {
                if (collection[x].type.toUpperCase() == 'CHECKBOX')
                    if (collection[x].checked == true) {
                        checkedTemp = true;
                    }
            }
            if (checkedTemp == true) {
                //$('#btn_continue')
                document.getElementById('btn_continue').style.display = 'inherit';
                //document.getElementById('<%=btn_continue.ClientID %>').style.display = 'inherit';
            }
            else {
                document.getElementById('btn_continue').style.display = 'none';
                //document.getElementById('<%=btn_continue.ClientID %>').style.visibility = "none";
            }
        }

        function UncheckAll() {
            document.getElementById('<%=cb_StudentBehavior.ClientID%>').checked = false;
            document.getElementById('<%=cb_Training.ClientID%>').checked = false;
            document.getElementById('<%=cb_Probe.ClientID%>').checked = false;
            document.getElementById('<%=cb_LessonIncomplete.ClientID%>').checked = false;
            document.getElementById('continue_btn').style.display = 'none';
        }

        function reasonCheckBox_CheckedChanged(id,obj,reason) {
            //alert(id);
            document.getElementById('continue_btn').style.display = 'none';
            if (id == 'cb_StudentBehavior') {
                //alert('cb_StudentBehavior');
                if (document.getElementById('<%=cb_StudentBehavior.ClientID%>').checked) {
                    document.getElementById('<%=cb_Training.ClientID%>').checked = false;
                    document.getElementById('<%=cb_Probe.ClientID%>').checked = false;
                    document.getElementById('<%=cb_LessonIncomplete.ClientID%>').checked = false;
                    document.getElementById('continue_btn').style.display = 'inherit';
                    $('#hdnChkdRsn').val(reason);
                    //$('#mistrialRsn').text(reason);
                }
            } else if (id == 'cb_Training') {
                //alert('cb_Training');
                if (document.getElementById('<%=cb_Training.ClientID%>').checked) {
                    document.getElementById('<%=cb_StudentBehavior.ClientID%>').checked = false;
                    document.getElementById('<%=cb_Probe.ClientID%>').checked = false;
                    document.getElementById('<%=cb_LessonIncomplete.ClientID%>').checked = false;
                    document.getElementById('continue_btn').style.display = 'inherit';
                    $('#hdnChkdRsn').val(reason);
                    //$('#mistrialRsn').text(reason);
                }
            } else if (id == 'cb_Probe') {
                //alert('cb_Probe');
                if (document.getElementById('<%=cb_Probe.ClientID%>').checked) {
                    document.getElementById('<%=cb_Training.ClientID%>').checked = false;
                    document.getElementById('<%=cb_StudentBehavior.ClientID%>').checked = false;
                    document.getElementById('<%=cb_LessonIncomplete.ClientID%>').checked = false;
                    document.getElementById('continue_btn').style.display = 'inherit';
                    $('#hdnChkdRsn').val(reason);
                    //$('#mistrialRsn').text(reason);
                }
            } else if (id == 'cb_LessonIncomplete') {
                if (document.getElementById('<%=cb_LessonIncomplete.ClientID%>').checked) {
                    document.getElementById('<%=cb_Training.ClientID%>').checked = false;
                    document.getElementById('<%=cb_StudentBehavior.ClientID%>').checked = false;
                    document.getElementById('<%=cb_Probe.ClientID%>').checked = false;
                    document.getElementById('continue_btn').style.display = 'inherit';
                    $('#hdnChkdRsn').val(reason);
                    //$('#mistrialRsn').text(reason);
                }
            }
            if (!document.getElementById('<%=cb_StudentBehavior.ClientID%>').checked && !document.getElementById('<%=cb_Training.ClientID%>').checked && !document.getElementById('<%=cb_Probe.ClientID%>').checked && !document.getElementById('<%=cb_LessonIncomplete.ClientID%>').checked) {
                $('#hdnChkdRsn').val("");
                //$('#mistrialRsn').text("");
            }
            
        }

        function reasonClose_Click() {
            $('#div_reason').hide(); $('.fullOverlay').fadeOut('fast');
            var reasonVal = $('#hdnMissTrialRsn').val();
            //alert(reasonVal);
            if (reasonVal == "") {
                //chkSessMistrial.checked = false;
                document.getElementById('<%=chkSessMistrial.ClientID%>').checked = false;
            } else {
                $('#mistrialRsn').text(reasonVal);
            }
            UncheckAll();
        }

        function ClosePromptPop() {

            $("#divPrmpts").hide();
        }

        function ColorIt() {
            // console.log("enter");
            var tblr = $('#grdDataSht').find('tr');

            // console.log("tablelength : " + tblr.length + ", " + "grid len : " + $('#grdDataSht').length);
            for (i = 0; i < tblr.length; i++) {
                var tableTd = $(tblr[i]).find('td');

                for (var j = 0; j < tableTd.length; j++) {

                    var input = $(tableTd[j]).find('input[type=radio]');
                    var inputTxt = $(tableTd[j]).find('input[type=text]')
                    if (input.length > 0) {
                        if ($(input).attr('disabled') == 'disabled') {
                            $(input).closest('td').find('label').css('color', '#ccc');
                            $(input).parent().parent().find('td').css('background-color', '#ccc');
                            $(input).parent().parent().find('textarea').css('background-color', '#ccc');
                        }
                    }
                    if (inputTxt.length > 0) {
                        if ($(inputTxt).attr('disabled') == 'disabled') {
                            $(inputTxt).closest('td').find('label').css('color', '#ccc');
                            $(inputTxt).parent().parent().find('td').css('background-color', '#ccc');
                            $(inputTxt).parent().parent().find('textarea').css('background-color', '#ccc');

                        }
                    }
                    input = $(tableTd[j]).find('select');




                    if (input.length > 0) {
                        if ($(input).attr('disabled') == 'disabled') {
                            $(input).parent().parent().find('td').css('background-color', '#ccc');
                            $(input).parent().parent().find('textarea').css('background-color', '#ccc');
                        }



                    }
                }


            }
        }




        function viewDet() {
            $('#viewDettab').slideToggle();

            //if ($('#viewDettab').is(':visible')) {
            //    $("#viewDettab").css("display", "none");
            //    $('#viewDettab').fadeOut();
            //} else {
            //    $("#viewDettab").css("display", "block");
            //    $('#viewDettab').fadeIn();
            //}
        }




    </script>
    <script type="text/javascript">

        var txtTextID;
        function adjustStyle(width) {
            width = parseInt(width);

            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad != null) {
                $("#sizestylesheet").attr("href", "../Administration/CSS/BehaviorScoreTab.css");
            }
            else {
                $("#sizestylesheet").attr("href", "../Administration/CSS/BehaviorScore.css");
            }

            if (isiPad == null) {
                $("#sized").attr("href", "../Administration/CSS/DatasheetStyle.css");

                return;
            }
            if (isiPad != null) {
                $("#sized").attr("href", "../Administration/CSS/DatasheetStyleTab.css");
                $("#HdrDiv").css("left", '14%');

                $("#IfrmTimer").css("min-height", "615px");
                $('#IfrmTimer').css("height", "auto");

                //$('#viewDocbtn').css("display", "none");
                $('#divDoc').css('width', '500px');
                $('.tablePart').css('width', '700px');
                $('.rfrContainer').css('width', '220px');
                $('.rfrContainer').css('float', 'left');
                $('#btnAddTrial').css('float', 'left');
                $('#btnAddTrial').css('margin-left', '155px');
                return;
            }
        }
        $(function () {
            adjustStyle($(this).width());
            $(window).resize(function () {
                adjustStyle($(this).width());
            });
        });

        function closePOP() {
            $('#HdrDiv').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast');
            });

        }
        function showNewPopup() {
            $('#newPopup').show();
        }
        function showPop(type) {
            if (type == 1) {
                $('#tblNewIOA').css('display', 'none');
                $('#tblIOAUser').css('display', 'none');
                $('#tblIOAndNorm').css('display', 'inline-table');
                $('#tblVTSelectr').css('display', 'none');
            }
            if (type == 2) {
                $('#tblIOAndNorm').css('display', 'none');
                $('#tblIOAUser').css('display', 'none');
                $('#tblNewIOA').css('display', 'inline-table');
                $('#tblVTSelectr').css('display', 'none');
            }
            if (type == 3) {
                $('#tblIOAndNorm').css('display', 'none');
                $('#tblIOAUser').css('display', 'none');
                $('#tblNewIOA').css('display', 'none');
                $('#tblVTSelectr').css('display', 'inline-table');
            }
            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('slow', function () {
                $('#HdrDiv').fadeIn();
                $('.btn_cont').hide();
                //$('#btnNoIOA').trigger('click');

            });
        }

        function showPopPreview(type) {
            if (type == 1) {
                $('#tblNewIOA').css('display', 'none');
                $('#tblIOAUser').css('display', 'none');
                $('#tblIOAndNorm').css('display', 'inline-table');
                $('#tblVTSelectr').css('display', 'none');
            }
            if (type == 2) {
                $('#tblIOAndNorm').css('display', 'none');
                $('#tblIOAUser').css('display', 'none');
                $('#tblNewIOA').css('display', 'inline-table');
                $('#tblVTSelectr').css('display', 'none');
            }
            if (type == 3) {
                $('#tblIOAndNorm').css('display', 'none');
                $('#tblIOAUser').css('display', 'none');
                $('#tblNewIOA').css('display', 'none');
                $('#tblVTSelectr').css('display', 'inline-table');
            }
            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('slow', function () {
                $('#btnNoIOA').trigger('click');
                $('.btn_cont').hide();
            });
        }
        function showIOAExist() {
            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('slow', function () {
                alert("IOA Submission Pending.");
            });
        }

        function showTempOverride() {


            $('.fullOverlay').empty();
            $('.fullOverlay').fadeIn('slow', function () {
                $('#div_tempOverride').fadeIn('fast', function () {
                    $('.tempOverClose').hide();
                });
            });
        }

        function showReason(obj) {

            if (obj.checked) {
                $('.fullOverlay').empty();
                $('.fullOverlay').fadeIn('slow', function () {
                    $('#div_reason').fadeIn('fast', function () {
                        //$('.reasonClose').hide();
                    });
                });
            } else {
                $('#mistrialRsn').empty();
                $('#hdnMissTrialRsn').val('');
            }
        }

        function GetParamValue() {

            var query = window.location;

            var parms = query.split('&');

            for (var i = 0; i < parms.length; i++) {

                var pos = parms[i].split('=');

                var key = pos[0];

                var val = pos[1];

                //alert(key);

                if (key == "AddTrial") {
                    return true;
                    break;
                }
                else {
                    return false;
                }

            }

        }


        function SelectIOA() {

            $("#button").attr("disabled", true);
            $('#tblIOAndNorm').css('display', 'none');
            $('#tblNewIOA').css('display', 'none');
            $('#tblIOAUser').css('display', 'inline-table');

				$('.fullOverlay').empty();
				$('.fullOverlay').fadeIn('slow', function () {
					$('#HdrDiv').fadeIn();
					$('.btn_cont').hide();
					//$('#btnNoIOA').trigger('click');

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
        var id_array = new Array();
        var Timerid_array = new Array();
        var RetWindwIntrvlId = "";
        function stopwatch(btn) {
            var txtdur = $(btn).prev()[0];
            var hidden = $(btn).next()[0];
            if (btn.value.trim() == 'Edit') {
                var id = btn.id;               
                var spliArr = id.split("_");                
                var idSetTextbox = spliArr[0] + "_" + spliArr[1] + "_" + "txtTimerId_" + spliArr[3];
                var StrtStpButonIdval = document.getElementById('' + idSetTextbox).value;
                var intrvliD = "";
                if (StrtStpButonIdval != "" && StrtStpButonIdval != null && StrtStpButonIdval != "undefined")
                {
                    intrvliD = StrtStpButonIdval.split("/")[0];
                    for (var id = 0; id < id_array.length; id++) {
                        if (id_array[id] == intrvliD) {
                            window.clearInterval(id_array[id]);
                           // hidden.value = "00:00:00";
                            var index = id_array.indexOf(id_array[id]);
                            id_array.splice(index, 1);
                        }
                    }                         
                }
                              
                var txtid = spliArr[0] + "_" + spliArr[1] + "_" + "txtDuratn_" + spliArr[3];                
                var HidnId = spliArr[0] + "_" + spliArr[1] + "_" + "hfDuration_" + spliArr[3];
                var strtbtnid = intrvliD+"/"+spliArr[0] + "_" + spliArr[1] + "_" + "btnDuration_" + spliArr[3];
                var strtbtnName = spliArr[0] + "$" + spliArr[1] + "$" + "btnDuration_" + spliArr[3];                 
               // document.getElementById('' + txtid).style.display = 'none';               
               // document.getElementById('' + strtbtnid).style.display = 'none';               
                var hrid = spliArr[0] + "_" + spliArr[1] + "_" + "txtHrDuratn_" + spliArr[3];
                var secondid = spliArr[0] + "_" + spliArr[1] + "_" + "txtMinDuratn_" + spliArr[3];
                var minid = spliArr[0] + "_" + spliArr[1] + "_" + "txtSecDuratn_" + spliArr[3];
                document.getElementById('' + hrid).style.display = "inline";
                document.getElementById('' + secondid).style.display = "inline";
                document.getElementById('' + minid).style.display = "inline";               
                document.getElementById('' + minid).style.display = "inline";
               // document.getElementById('' + idSetTextbox).style.display = "inline";
                document.getElementById('' + hrid).value = "";
                document.getElementById('' + secondid).value = "";
                document.getElementById('' + minid).value = "";
                document.getElementById('' + txtid).value = "00:00:00";
                var HidnId = spliArr[0] + "_" + spliArr[1] + "_" + "hfDuration_" + spliArr[3];
                document.getElementById('' + HidnId).value = "00:00:00";
                if (intrvliD != "" && intrvliD != null && intrvliD != "undefined")
                    document.getElementById('' + strtbtnid).value = "Start";
                btn.value = 'Set';               
                temp();
            }
            else if (btn.value.trim() == 'Set') {
                var id = btn.id;               
                var spliArr = id.split("_");
                var hrid = spliArr[0] + "_" + spliArr[1] + "_" + "txtHrDuratn_" + spliArr[3];
                var secondid = spliArr[0] + "_" + spliArr[1] + "_" + "txtMinDuratn_" + spliArr[3];
                var minid = spliArr[0] + "_" + spliArr[1] + "_" + "txtSecDuratn_" + spliArr[3];
                var idSetTextbox = spliArr[0] + "_" + spliArr[1] + "_" + "txtTimerId_" + spliArr[3];
                var StrtStpButonIdval = document.getElementById('' + idSetTextbox).value;
                var intrvliD = "";
                if (StrtStpButonIdval != "" && StrtStpButonIdval != null && StrtStpButonIdval != "undefined") {
                    intrvliD = StrtStpButonIdval.split("/")[0];
                    for (var id = 0; id < id_array.length; id++) {
                        if (id_array[id] == intrvliD) {
                            window.clearInterval(id_array[id]);
                            // hidden.value = "00:00:00";
                            var index = id_array.indexOf(id_array[id]);
                            id_array.splice(index, 1);
                        }
                    }
                }
                var txtid = spliArr[0] + "_" + spliArr[1] + "_" + "txtDuratn_" + spliArr[3];
                var strtbtnid = intrvliD + "/" + spliArr[0] + "_" + spliArr[1] + "_" + "btnDuration_" + spliArr[3];
                document.getElementById('' + txtid).value = "00:00:00";
                if (intrvliD != "" && intrvliD != null && intrvliD != "undefined")
                    document.getElementById('' + strtbtnid).value = "Start";
                var hrvalue = 0;
                var minvalue = 0;
                var secvalue = 0;
                
                 hrvalue  =  (document.getElementById('' + hrid).value);
                 minvalue = document.getElementById('' + secondid).value;
                 secvalue = document.getElementById('' + minid).value;                
                 if (isNaN(hrvalue) || isNaN(minvalue) || isNaN(secvalue)) {                     
                     alert("Only numbers allowed");
                     return;
                 }
                 if (hrvalue == "" || hrvalue == null|| hrvalue == "undefined") {
                     hrvalue = 0; 
                 }
                 if (minvalue == "" || minvalue == null||hrvalue == "undefined") {
                     minvalue = 0; 
                 }
                 if (secvalue == "" || secvalue == null||hrvalue == "undefined") {
                     secvalue = 0; 
                 }
                 var ahr = parseInt(hrvalue);                
                var amin = parseInt(minvalue);
                var asec = parseInt(secvalue);
                if ((ahr > 24)  ||(amin > 60)||(asec > 60))                    
                     {
                    alert("Keep Time Formate");
                    return;
                }
                if ((ahr <0) || (amin <0) || (asec <0)) {
                    alert("Keep Time Formate");
                    return;
                }
                //if ((ahr == 0) && (amin == 0) && (asec == 0)) {
                //    alert("Keep Time Formate");
                //    return;
                //}
                var displayval = "";
                var dishr = hrvalue, dismin = minvalue; dissec = secvalue;
                if (ahr == 0)
                    dishr = "00";
                if (amin == 0)
                    dismin = "00";
                if (asec == 0)
                    dissec = "00";
                displayval = dishr + ":" + dismin + ":" + dissec;
                //displayval = hrvalue + ":" + minvalue + ":" + secvalue;          
                var txtid = spliArr[0] + "_" + spliArr[1] + "_" + "txtDuratn_" + spliArr[3];
                var HidnId = spliArr[0] + "_" + spliArr[1] + "_" + "hfDuration_" + spliArr[3];
                document.getElementById('' + txtid).value = displayval;                
                document.getElementById('' + HidnId).value = displayval;
                var strtbtnid = spliArr[0] + "_" + spliArr[1] + "_" + "btnDuration_" + spliArr[3];
                for (var id = 0; id < id_array.length; id++) {
                    if (id_array[id] == btn.id) {
                       window.clearInterval(id_array[id]);
                      //  hidden.value = txtdur.value;
                        var index = id_array.indexOf(id_array[id]);
                        id_array.splice(index, 1);
                    }
                }
                btn.value = 'Clear';
                document.getElementById('' + hrid).disabled = "true";
                document.getElementById('' + secondid).disabled = "true";
                document.getElementById('' + minid).disabled = "true";               
                if(dishr == "00" &&  dismin == "00" && dissec == "00"){
                    resetTimeManual(txtid);
                }
                else{
                    temp();
                }               
                //resetTimeWhileEdit(txtid);
            }
            else if (btn.value.trim() == 'Clear') {
                var id = btn.id;
                var spliArr = id.split("_");                
                var hrid = spliArr[0] + "_" + spliArr[1] + "_" + "txtHrDuratn_" + spliArr[3];
                var secondid = spliArr[0] + "_" + spliArr[1] + "_" + "txtMinDuratn_" + spliArr[3];
                var minid = spliArr[0] + "_" + spliArr[1] + "_" + "txtSecDuratn_" + spliArr[3];                
                var idSetTextbox = spliArr[0] + "_" + spliArr[1] + "_" + "txtTimerId_" + spliArr[3];
                var StrtStpButonIdval = document.getElementById('' + idSetTextbox).value;
                var intrvliD = "";
                if (StrtStpButonIdval != "" && StrtStpButonIdval != null && StrtStpButonIdval != "undefined") {
                    intrvliD = StrtStpButonIdval.split("/")[0];
                    for (var id = 0; id < id_array.length; id++) {
                        if (id_array[id] == intrvliD) {
                            window.clearInterval(id_array[id]);
                            // hidden.value = "00:00:00";
                            var index = id_array.indexOf(id_array[id]);
                            id_array.splice(index, 1);
                        }
                    }
                }
                var txtid = spliArr[0] + "_" + spliArr[1] + "_" + "txtDuratn_" + spliArr[3];
                var HidnId = spliArr[0] + "_" + spliArr[1] + "_" + "hfDuration_" + spliArr[3];
                var strtbtnid = intrvliD + "/" + spliArr[0] + "_" + spliArr[1] + "_" + "btnDuration_" + spliArr[3];
                document.getElementById('' + txtid).value = "00:00:00";
                document.getElementById('' + HidnId).value = "00:00:00";
                if (intrvliD != "" && intrvliD != null && intrvliD != "undefined")
                    document.getElementById('' + strtbtnid).value = "Start";
                document.getElementById('' + hrid).value = "";
                document.getElementById('' + secondid).value = "";
                document.getElementById('' + minid).value = "";
                var txtid = spliArr[0] + "_" + spliArr[1] + "_" + "txtDuratn_" + spliArr[3];
                document.getElementById('' + txtid).value = "00:00:00";
                resetTimeManual(txtid);
                document.getElementById('' + hrid).removeAttribute("disabled");
                document.getElementById('' + minid).removeAttribute("disabled");
                document.getElementById('' + secondid).removeAttribute("disabled");
                btn.value = 'Set';                
            }
			 else {  
	            //var txtdur = $(btn).prev()[0];
                //var hidden = $(btn).next()[0];
                var id = btn.id;                
            var lblTotDur = document.getElementById('<%=lblTotDur.ClientID%>');
	            if (btn.value.trim() == 'Start') {
                btn.value = 'Stop';
	                var btnid = btn.id;
	                var Rettxtid = "";
	                var slashArry = new Array();
	                var spliArr = new Array();
	                //if (btnid.includes("/")) {
	                if (btnid.indexOf("/") != -1) {
	                    slashArry = btnid.split("/");
	                    var beforesplit = slashArry[1];
	                    spliArr = beforesplit.split("_");
	                }
	                else{
	                    spliArr = btnid.split("_");
	                }

	                Timerid_array = spliArr.slice();
	                    Rettxtid = Timerid_array[0] + "_" + Timerid_array[1] + "_" + "txtTimerId_" + Timerid_array[3];
	                    document.getElementById('' + Rettxtid).value = Timerid_array[0] + "_" + Timerid_array[1] + "_" + Timerid_array[3] ;
	                    var TimersetId = spliArr[0] + "_" + spliArr[1] + "_" + "txtDuratn_" + spliArr[3];
	               // }    
	                var retval = window.setInterval(function () { settime(txtdur) }, 1000);	               
	                btn.id = retval + "/" + spliArr[0] + "_" + spliArr[1] + "_" + "btnDuration_" + spliArr[3];
	                //RetWindwIntrvlId = retval;
	                var hrid = spliArr[0] + "_" + spliArr[1] + "_" + "txtHrDuratn_" + spliArr[3];
	                var secondid = spliArr[0] + "_" + spliArr[1] + "_" + "txtMinDuratn_" + spliArr[3];
	                var minid = spliArr[0] + "_" + spliArr[1] + "_" + "txtSecDuratn_" + spliArr[3];
	                document.getElementById('' + hrid).value = "";
	                document.getElementById('' + secondid).value = "";
	                document.getElementById('' + minid).value = "";
	                document.getElementById('' + Rettxtid).value = retval + "/" + spliArr[0] + "_" + spliArr[1] + "_" + "btnDuration_" + spliArr[3];;
	                id_array.push(retval);
	            } else if (btn.value.trim() == 'Stop') {	                
	                var popid = "";
	                var strtbtnidArr = new Array();
	                //if (btn.id.includes("/")) {
	                var spliArr = new Array();
	                if (btn.id.indexOf("/") != -1){
	                    var slashArry = btn.id.split("/");
	                    popid = slashArry[0];

	                    var beforesplit = slashArry[1];
	                    spliArr = beforesplit.split("_");
	                }
                for (var id = 0; id < id_array.length; id++) {
	                   //  if (id_array[id] == btn.id) {
                    if (id_array[id] == popid) {
                        window.clearInterval(id_array[id]);
                        hidden.value = txtdur.value;
                        var index = id_array.indexOf(id_array[id]);
                        id_array.splice(index, 1);
                    }
                }
                btn.value = 'Start';
	                var hrid = spliArr[0] + "_" + spliArr[1] + "_" + "txtHrDuratn_" + spliArr[3];
	                var secondid = spliArr[0] + "_" + spliArr[1] + "_" + "txtMinDuratn_" + spliArr[3];
	                var minid = spliArr[0] + "_" + spliArr[1] + "_" + "txtSecDuratn_" + spliArr[3];
	                document.getElementById('' + hrid).value = "";
	                document.getElementById('' + secondid).value = "";
	                document.getElementById('' + minid).value = "";
                temp();
            }
        }
        }
        //function stopwatch(btn) {	
		//	if (btn.value == 'Edit') {               
        //        var id = btn.id;               
        //        var spliArr = id.split("_");                
        //        var txtid = spliArr[0] + "_" + spliArr[1] + "_" + "txtDuratn_" + spliArr[3];                
        //        var strtbtnid = spliArr[0] + "_" + spliArr[1] + "_" + "btnDuration_" + spliArr[3];               
            //    document.getElementById('' + txtid).style.display = 'none';               
            //    document.getElementById('' + strtbtnid).style.display = 'none';               
            //    var hrid = spliArr[0] + "_" + spliArr[1] + "_" + "txtHrDuratn_" + spliArr[3];
            //    var secondid = spliArr[0] + "_" + spliArr[1] + "_" + "txtMinDuratn_" + spliArr[3];
            //    var minid = spliArr[0] + "_" + spliArr[1] + "_" + "txtSecDuratn_" + spliArr[3];
            //    document.getElementById('' + hrid).style.display = "inline";
            //    document.getElementById('' + secondid).style.display = "inline";
            //    document.getElementById('' + minid).style.display = "inline";               
            //    document.getElementById('' + hrid).value = "";
            //    document.getElementById('' + secondid).value = "";
            //    document.getElementById('' + minid).value = "";
            //    id_array.push(btn.id);                          
            //    btn.value = 'Set';               
            //}
			//else if (btn.value == 'Set') {
            //    var id = btn.id;               
            //    var spliArr = id.split("_");
            //    var hrid = spliArr[0] + "_" + spliArr[1] + "_" + "txtHrDuratn_" + spliArr[3];
            //    var secondid = spliArr[0] + "_" + spliArr[1] + "_" + "txtMinDuratn_" + spliArr[3];
            //    var minid = spliArr[0] + "_" + spliArr[1] + "_" + "txtSecDuratn_" + spliArr[3];
                             
            //    var hrvalue = 0;
            //    var minvalue = 0;
            //    var secvalue = 0;
                
            //     hrvalue  =  (document.getElementById('' + hrid).value);
            //     minvalue = document.getElementById('' + secondid).value;
            //     secvalue = document.getElementById('' + minid).value;                
            //     if (isNaN(hrvalue) || isNaN(minvalue) || isNaN(secvalue)) {                     
            //         alert("Only numbers allowed");
            //         return;
            //     }
            //     if (hrvalue == "" || hrvalue == null|| hrvalue == "undefined") {
            //         hrvalue = 0; 
            //     }
            //     if (minvalue == "" || minvalue == null||hrvalue == "undefined") {
            //         minvalue = 0; 
            //     }
            //     if (secvalue == "" || secvalue == null||hrvalue == "undefined") {
            //         secvalue = 0; 
            //     }
                // var ahr = parseInt(hrvalue);                
                //var amin = parseInt(minvalue);
                //var asec = parseInt(secvalue);
                //if ((ahr > 24)  ||(amin > 60)||(asec > 60))                    
                //     {
                //    alert("Keep Time Formate");
                //    return;
                //}
                //if ((ahr <0) || (amin <0) || (asec <0)) {
                //    alert("Keep Time Formate");
                //    return;
                //}
                //if ((ahr == 0) && (amin == 0) && (asec == 0)) {
                //    alert("Keep Time Formate");
                //    return;
                //}
                //var displayval = "";
                //var dishr = hrvalue, dismin = minvalue; dissec = secvalue;
                //if (ahr == 0)
                //    dishr = "00";
                //if (amin == 0)
                //    dismin = "00";
                //if (asec == 0)
                //    dissec = "00";
                //displayval = ahr + ":" + dismin + ":" + dissec;
                //displayval = hrvalue + ":" + minvalue + ":" + secvalue;          
                //var txtid = spliArr[0] + "_" + spliArr[1] + "_" + "txtDuratn_" + spliArr[3];
                //document.getElementById('' + txtid).value = displayval;                
                //var strtbtnid = spliArr[0] + "_" + spliArr[1] + "_" + "btnDuration_" + spliArr[3];
                //for (var id = 0; id < id_array.length; id++) {
                //    if (id_array[id] == btn.id) {
                //       window.clearInterval(id_array[id]);
                      //  hidden.value = txtdur.value;
            //            var index = id_array.indexOf(id_array[id]);
            //            id_array.splice(index, 1);
            //        }
            //    }
            //    btn.value = 'Clear';
            //    document.getElementById('' + hrid).disabled = "true";
            //    document.getElementById('' + secondid).disabled = "true";
            //    document.getElementById('' + minid).disabled = "true";               
            //    if(dishr == "00" &&  dismin == "00" && dissec == "00"){
            //        resetTimeManual(txtid);
            //    }
            //    else{
            //        temp();
            //    }               
            //}
            //else if (btn.value == 'Clear') {                
            //    var id = btn.id;
            //    var spliArr = id.split("_");                
            //    var hrid = spliArr[0] + "_" + spliArr[1] + "_" + "txtHrDuratn_" + spliArr[3];
            //    var secondid = spliArr[0] + "_" + spliArr[1] + "_" + "txtMinDuratn_" + spliArr[3];
            //    var minid = spliArr[0] + "_" + spliArr[1] + "_" + "txtSecDuratn_" + spliArr[3];                
            //    document.getElementById('' + hrid).value = "";
            //    document.getElementById('' + secondid).value = "";
            //    document.getElementById('' + minid).value = "";
            //    var txtid = spliArr[0] + "_" + spliArr[1] + "_" + "txtDuratn_" + spliArr[3];
                //document.getElementById('' + txtid).value = "00:00:00";
               // resetTimeManual(txtid);
            //    document.getElementById('' + hrid).removeAttribute("disabled");
            //    document.getElementById('' + minid).removeAttribute("disabled");
            //    document.getElementById('' + secondid).removeAttribute("disabled");
            //    btn.value = 'Set';                
            //}
			// else {  
            //var txtdur = $(btn).prev()[0];
            //var hidden = $(btn).next()[0];
           // var lblTotDur = document.getElementById('<%=lblTotDur.ClientID%>');
            //if (btn.value == 'Start') {
            //    btn.value = 'Stop';
	        //        var btnid = btn.id;
	                //if (btnid.includes("_")) {
	        //        if (btnid.indexOf("_") !== -1) {
	        //            var spliArr = btnid.split("_");
	        //            var editbtnid = spliArr[0] + "_" + spliArr[1] + "_" + "btnEditDuration_" + spliArr[3];

	        //            document.getElementById('' + editbtnid).style.display = 'none';
	        //        }
            //    btn.id = window.setInterval(function () { settime(txtdur) }, 1000);
            //    id_array.push(btn.id);
            //} else if (btn.value == 'Stop') {
            //    for (var id = 0; id < id_array.length; id++) {
            //        if (id_array[id] == btn.id) {
            //            window.clearInterval(id_array[id]);
            //            hidden.value = txtdur.value;
            //            var index = id_array.indexOf(id_array[id]);
            //            id_array.splice(index, 1);
            //        }
            //    }
        //        btn.value = 'Start';
        //        temp();
        //    }
        //}
        //}
        function multiClickCheckDiscard(btn) {
            if (!multiCLickDiscard) {
                multiCLickDiscard = true;
                btn.style.opacity = '0.5';
                return true;
            }
            return false;
        }
        function multiClickCheckPrint(btn) {
            if (!multiCLickPrint) {
                multiCLickPrint = true;
                btn.style.opacity = '0.5';
                return true;
            }
            return false;
        }
        function multiClickCheckViewPrior(btn) {
            if (!multiCLickViewPrior) {
                multiCLickViewPrior = true;
                btn.style.opacity = '0.5';
                return true;
            }
            return false;
        }
        function loadBeforeSave(type) {
            if (!multiClickSubmit) {
                var grid = document.getElementById('<%=grdDataSht.ClientID%>');
                var rowcount = parseInt(grid.rows.length - 1);
                for (var i = 1; i <= rowcount; i++) {
                    var txtdur = $(grid.rows[i]).find(".clsDuratn");
                    for (var j = 0; j < txtdur.length; j++) {
                        var btn = $(txtdur[j]).next()[0];
                        var hidden = $(btn).next()[0];

                        window.clearInterval(btn.id);
                        txtdur = $(grid.rows[i]).find(".clsDuratn")[j];
                        hidden.value = txtdur.value;
                        btn.value = 'Start';
                        var index = id_array.indexOf(btn.id);
                        id_array.splice((i - 1), 1);
                    }
                }

                temp();
                if (type == 'Submit') {
                    //return confirm('Are you sure you want to submit ?');                
                    var temp1 = confirm('Are you sure you want to submit ?');
                    if (temp1.toString() == "true") {
                        $("#btnSubmit2").attr({ "disabled": true, "value": "Submitting" }).removeClass("NFButtonNew").addClass("NFButtonNew1");
                        $("#btnSubmit").attr({ "disabled": true, "value": "Submitting" }).removeClass("NFButtonNew").addClass("NFButtonNew1");
                        if (IOAtype != "Y") {           //check the IOA user Or Normal User
                            $('#div_overlay').show();
                        }
                        $("body").css("cursor", "progress");
                        multiClickSubmit = true;
                        return true;
                    }
                    else {
                        $('#div_overlay').hide();
                        sessionStorage.setItem('DataCl', 'false');
                        Checkclose.value = 'false';
                        return false;
                    }
                }
                else if (type == 'Save') {
                    multiClickSubmit = true;
                    document.getElementById('btnSave').style.opacity = '0.5';
                    document.getElementById('btnSave1').style.opacity = '0.5';
                    return true;
                }
                else {
                    multiClickSubmit = true;
                    document.getElementById('btnAddTrial').style.opacity = '0.5';
                    return true;
                }
            }
            return false;
        }


        function settime(txt) {
            txt.value = formatTime(spiltTime(txt.value))
        }
        function formatTime(time) {
            var h = m = s = ms = 0;
            var newTime = '';

            h = Math.floor(time / (60 * 60));
            time = time % (60 * 60);
            m = Math.floor(time / (60));
            time = time % (60);
            s = Math.floor(time);

            newTime = pad(h, 2) + ':' + pad(m, 2) + ':' + pad(s, 2);
            return newTime;
        }
        function spiltTime(txt) {
            time = txt;
            var split = time.split(':');
            var seconds = (parseInt(split[0]) * 60 * 60) + (parseInt(split[1]) * 60) + parseInt(split[2]) + 1;

            return seconds;
        }
        function pad(num, size) {
            var s = "0000" + num;
            return s.substr(s.length - size);
        }

        function resetTime(txt) {
           
            // if (btn.id.includes("txtHrDuratn_")) {
            if ((txt.id.indexOf("txtHrDuratn_") != -1) || (txt.id.indexOf("txtMinDuratn_") != -1) || (txt.id.indexOf("txtSecDuratn_") != -1)) {
               var id = txt.id;
                var spliArr = id.split("_");
              var hrid = spliArr[0] + "_" + spliArr[1] + "_" + "txtHrDuratn_" + spliArr[3];
                var secondid = spliArr[0] + "_" + spliArr[1] + "_" + "txtMinDuratn_" + spliArr[3];
                var minid = spliArr[0] + "_" + spliArr[1] + "_" + "txtSecDuratn_" + spliArr[3];
                var TimerId = spliArr[0] + "_" + spliArr[1] + "_" + "txtTimerId_" + spliArr[3];
                var StrtbtnId = (document.getElementById('' + TimerId).value);
                var hrvalue = 0;
                var minvalue = 0;
                var secvalue = 0;

                hrvalue = (document.getElementById('' + hrid).value);
                minvalue = document.getElementById('' + secondid).value;
                secvalue = document.getElementById('' + minid).value;
                var StrtStpButonIdval = (document.getElementById('' + TimerId).value);

                var intrvliD = "";
                if (StrtStpButonIdval != "" && StrtStpButonIdval != null && StrtStpButonIdval != "undefined") {
                    document.getElementById('' + StrtbtnId).value = "Start";
                    intrvliD = StrtStpButonIdval.split("/")[0];
                    for (var id = 0; id < id_array.length; id++) {
                        if (id_array[id] == intrvliD) {
                            window.clearInterval(id_array[id]);
                            // hidden.value = "00:00:00";
                            var index = id_array.indexOf(id_array[id]);
                            id_array.splice(index, 1);
                        }
                    }
                }


                if (isNaN(hrvalue) || isNaN(minvalue) || isNaN(secvalue)) {
                    alert("Only numbers allowed");
                    return;
                }
                if (hrvalue == "" || hrvalue == null || hrvalue == "undefined") {
                    hrvalue = 0;
                }
                if (minvalue == "" || minvalue == null || hrvalue == "undefined") {
                    minvalue = 0;
                }
                if (secvalue == "" || secvalue == null || hrvalue == "undefined") {
                    secvalue = 0;
                }
                var ahr = parseInt(hrvalue);
                var amin = parseInt(minvalue);
                var asec = parseInt(secvalue);
                if ((ahr > 24) || (amin > 60) || (asec > 60)) {
                    alert("Keep Time Formate");
                    return;
                }
                if ((ahr < 0) || (amin < 0) || (asec < 0)) {
                    alert("Keep Time Formate");
                    return;
                }
                //if ((ahr == 0) && (amin == 0) && (asec == 0)) {
                //    alert("Keep Time Formate");
                //    return;
                //}
                var displayval = "";
                var dishr = hrvalue, dismin = minvalue; dissec = secvalue;
                if (ahr ==0 || ahr==""|| ahr==null)
                    dishr = "00";
                if (amin == 0)
                    dismin = "00";
                if (asec == 0)
                    dissec = "00";
                displayval = dishr + ":" + dismin + ":" + dissec;
                var txtid = spliArr[0] + "_" + spliArr[1] + "_" + "txtDuratn_" + spliArr[3];
                document.getElementById('' + txtid).value = displayval;
                var HidnId = spliArr[0] + "_" + spliArr[1] + "_" + "hfDuration_" + spliArr[3];
                document.getElementById('' + HidnId).value = displayval;
                if (dishr == "00" && dismin == "00" && dissec == "00") {
                    resetTimeManual(txtid);
                }
                else {
                    temp();
                }
            }
            else {
            var btn = $(txt).next()[0];
            var hidden = $(btn).next()[0];
                var txtId = $(hidden).next()[0];
                var edtBtn = $(txtId).next()[0];
                var Hr = $(edtBtn).next()[0];
                var Min = $(Hr).next()[0];
                var Sec = $(Min).next()[0];
            txt.value = '00:00:00';
            hidden.value = '00:00:00';
                Hr.value = ""; Min.value = ""; Sec.value = "";
            btn.value = 'Start';
            temp();
            for (var id = 0; id < id_array.length; id++) {
                   if (id_array[id] == txtId.value) {
                    window.clearInterval(id_array[id]);
                        if (btn.value.trim() == 'Stop') {
                        btn.value = 'Start';
                        txt.value = '00:00:00';
                    }
                    var index = id_array.indexOf(id_array[id]);
                    id_array.splice(index, 1);
                }
            }
        }
        }
//---------------------------------------------------------------------------
//        function resetTime(txt) {
//            var btn = $(txt).next()[0];
//            var hidden = $(btn).next()[0];
//            txt.value = '00:00:00';
//            hidden.value = '00:00:00';
//            btn.value = 'Start';
//            temp();
//            for (var id = 0; id < id_array.length; id++) {
//                if (id_array[id] == btn.id) {
//                    window.clearInterval(id_array[id]);
//                    if (btn.value == 'Stop') {
//                        btn.value = 'Start';
//                        txt.value = '00:00:00';
//                    }
//                    var index = id_array.indexOf(id_array[id]);
//                    id_array.splice(index, 1);
//                }
//            }
//        }
		 function resetTimeManual(txt) {
           
           // var btn = $(txt).next()[0];
           // var hidden = $(btn).next()[0];
            //txt.value = '00:00:00';
            //document.getElementById('' + txt).value = "0:0:0";
            document.getElementById('' + txt).value = "00:00:00";            
            //hidden.value = '00:00:00';
           // btn.value = 'Start';
            temp();           
            for (var id = 0; id < id_array.length; id++) {
                if (id_array[id] == txt) {
                    window.clearInterval(id_array[id]);
                    //if (btn.value == 'Stop') {
                    //    btn.value = 'Start';
                    //    txt.value = '00:00:00';
                    //}
                    var index = id_array.indexOf(id_array[id]);
                    id_array.splice(index, 1);
                }
            }
        }
        function txtTextChange(id) {
            //alert(id)
            var templateId = $('#<%=hdnTemplateId.ClientID%>').val();
            grid = document.getElementById('<%=grdDataSht.ClientID%>');
            $.ajax({
                type: "POST",
                url: "Datasheet.aspx/txtText_TextChanged1",
                data: "{'templateId':'" + templateId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                },
                Error: function () {
                }
            });


        }
        function scoreForText(cust_score) {
            document.getElementById('<%=hfTextScore.ClientID%>').value = "";
            var grid = document.getElementById('<%=grdDataSht.ClientID%>');
            if (grid != null) {
                var rowcount = parseInt(grid.rows.length - 1);
                var colcount = parseInt(grid.rows[0].cells.length);
                var totSteps = rowcount;
                var items;
                //alert(cust_score);
                if (cust_score.length > 2)
                    items = cust_score.split('|');

                //for (var i = 1; i <= colcount - 3; i++) {
                //    var cType = $(grid.rows[0].cells[i]).find("span")[0].innerHTML;
                //   var colmnName = $(grid.rows[0].cells[i]).find("b")[0].id;
                //   alert(cust_score);
                //   if (cType == 'Text') {
                //       var lblTxt = $('#tbl_Measure').find('#Customize_' + txtTextID)[0];
                //       if (lblTxt != null) {
                //            lblTxt.innerHTML = cust_score;
                //           document.getElementById('<%=hfTextScore.ClientID%>').value = cust_score;
                //        }
                //   }
                //}
                if (items != null) {
                    for (var i = 0; i <= items.length; i++) {
                        //var cType = $(grid.rows[0].cells[i]).find("span")[0].innerHTML;
                        if (items[i] != null) {
                            var colmnName = items[i].split('*')[0];
                            //alert(colmnName);
                            var lblTxt = $('#tbl_Measure').find('#Customize_' + colmnName)[0];
                            if (lblTxt != null) {

                                if (items[i].split('*')[1].indexOf("-") > -1) {
                                    lblTxt.innerHTML = "N/A";
                                }
                                else {
                                    lblTxt.innerHTML = items[i].split('*')[1];
                                }
                                //document.getElementById('<%=hfTextScore.ClientID%>').value = items[i].split('*')[1];


                                var hfCustomize = document.getElementById('<%=hfTextScore.ClientID%>');
                                hfCustomize.value = resetHFvalue(colmnName, lblTxt.innerHTML, hfCustomize.value);

                            }

                        }
                    }
                }
            }
        }

        function scoreForText1(cust_score) {
            document.getElementById('<%=hfTextScore.ClientID%>').value = "";
            var grid = document.getElementById('<%=grdDataSht.ClientID%>');
            if (grid != null) {
                var rowcount = parseInt(grid.rows.length - 1);
                var colcount = parseInt(grid.rows[0].cells.length);
                var totSteps = rowcount;
                var items;
                //alert(cust_score);
                if (cust_score.length > 2)
                    items = cust_score.split('|');
                else
                    items = cust_score;
                //for (var i = 1; i <= colcount - 3; i++) {
                //    var cType = $(grid.rows[0].cells[i]).find("span")[0].innerHTML;
                //   var colmnName = $(grid.rows[0].cells[i]).find("b")[0].id;
                //   alert(cust_score);
                //   if (cType == 'Text') {
                //       var lblTxt = $('#tbl_Measure').find('#Customize_' + txtTextID)[0];
                //       if (lblTxt != null) {
                //            lblTxt.innerHTML = cust_score;
                //           document.getElementById('<%=hfTextScore.ClientID%>').value = cust_score;
                //        }
                //   }
                //}
                for (var i = 0; i <= items.length; i++) {
                    //var cType = $(grid.rows[0].cells[i]).find("span")[0].innerHTML;
                    if (items[i] != null) {
                        var colmnName = items[i].split('*')[0];
                        //alert(colmnName);
                        var lblTxt = $('#tbl_Measure').find('#Customize_' + colmnName)[0];
                        if (lblTxt != null) {
                            if (items[i].split('*')[1].indexOf("-") > -1) {
                                lblTxt.innerHTML = "N/A";
                            }
                            else {
                                lblTxt.innerHTML = items[i].split('*')[1];
                            }

                            //document.getElementById('<%=hfTextScore.ClientID%>').value = items[i].split('*')[1];


                            var hfCustomize = document.getElementById('<%=hfTextScore.ClientID%>');
                            hfCustomize.value = resetHFvalue(colmnName, lblTxt.innerHTML, hfCustomize.value);

                        }
                    }
                }
            }
        }

        //hari
        function probe() {

            var hfprob = document.getElementById('<%=hfProbe.ClientID%>');
            var grid = document.getElementById('<%=grdDataSht.ClientID%>');
            if (grid != null) {
                hfprob.value = "Probe";
                var rowcount = parseInt(grid.rows.length - 1);
                var colcount = parseInt(grid.rows[0].cells.length);
                var totSteps = rowcount;
                for (var r = 1; r <= rowcount; r++) {
                    var txtnotes = $(grid.rows[r]).find("textarea")[0];

                    if (txtnotes != null) {
                        if (txtnotes.disabled == true) {
                            txtnotes.disabled = false;

                        }
                    }

                }
                for (var i = 1; i <= colcount - 3; i++) {
                    var cType = $(grid.rows[0].cells[i]).find("span")[0].innerHTML;
                    var colmnName = $(grid.rows[0].cells[i]).find("b")[0].id;
                    for (var j = 1; j <= rowcount; j++) {
                        var chkmis = $(grid.rows[j]).find("input:checkbox")[0];


                        if ((chkmis != null) && (chkmis.disabled == true)) {   //dont do anything for this row..... 
                            chkmis.disabled = false;

                        }
                        if (cType == '+/-') {
                            /*  Count Calc for Resp  */
                            var resp = $(grid.rows[j].cells[i]).find("input:radio");
                            if (resp[0].disabled == true) resp[0].disabled = false;
                            if (resp[1].disabled == true) resp[1].disabled = false;
                        }
                        if (cType == 'Prompt') {
                            /*  Count Calc for Prompt  */
                            var ddl = grid.rows[j].cells[i].getElementsByTagName('select')[0];
                            if (ddl.disabled == true) ddl.disabled = false;
                        }
                        if (cType == 'Frequency') {
                            /* Count Calc for Frequency  */
                            var txtfrq = $(grid.rows[j].cells[i]).find(".clsFreq")[0];
                            if (txtfrq.disabled == true) txtfrq.disabled = false;
                        }
                        if (cType == 'Duration') {
                            /* Time Calc for Duration */
                            var txtdur = $(grid.rows[j].cells[i]).find(".clsDuratn")[0];
                            if (txtdur.disabled == true) txtdur.disabled = false;

                            var durBtn = $(grid.rows[j].cells[i]).find(".NFButtonWithNoImage")[0];
                            if (durBtn.disabled == true) durBtn.disabled = false;
                        }
                        if (cType == 'Text') {
                            var txtcust = $(grid.rows[j].cells[i]).find(".clsText")[0];
                            if (txtcust.disabled == true) txtcust.disabled = false;
                        }

                    }
                    //var colmnName = $(grid.rows[0].cells[i]).find("b")[0].innerHTML.replace(/ /g, "");


                }
            }
            var tblr = $('#grdDataSht').find('tr');
            for (i = 0; i < tblr.length; i++) {
                var tableTd = $(tblr[i]).find('td');

                for (var j = 0; j < tableTd.length; j++) {

                    var input = $(tableTd[j]).find('input[type=radio]');

                    if (input.length > 0) {


                        $(input).closest('td').find('label').css('color', '#f9f9f9');
                        $(input).parent().parent().find('td').css('background-color', '#f9f9f9');
                        $(input).parent().parent().find('textarea').css('background-color', '#f9f9f9');


                    }
                    input = $(tableTd[j]).find('select');

                    if (input.length > 0) {

                        $(input).parent().parent().find('td').css('background-color', '#f9f9f9');
                        $(input).parent().parent().find('textarea').css('background-color', '#f9f9f9');


                    }
                }


            }






            temp();

            //var tblr = $('#grdDataSht').find('tr');
            //for (i = 0; i < tblr.length; i++) {
            //    var tableTd = $(tblr[i]).find('td');

            //    for (var j = 0; j < tableTd.length; j++) {

            //        var input = $(tableTd[j]).find('input[type=radio]');
            //        if (input.length > 0) {
            //           // if ($(input).attr('disabled') == 'disabled') {
            //                (input).closest('td').find('label').css('color', '#ffffff');
            //                $(input).parent().parent().find('td').css('background-color', '#ffffff');
            //                $(input).parent().parent().find('textarea').css('background-color', '#ffffff');
            //            //}

            //        }
            //    }


            //}


        }




        function onchangeTxt(id) {
            txtTextID = id;
        }
        function rdoSampleClick(id) {
            var tblr = $('#grdDataSht').find('tr');

            hdnSelectedSampleTest.value = sample;
            //alert(sample);

        }

        function rdouncheck(id) {

            // alert(id.value)
            var $radio = $(id);
            //if (id.checked==true ) {
            //    id.checked = false;
            //}
            if ($radio.data('waschecked') == true) {
                $radio.prop('checked', false);
                $radio.data('waschecked', false);
                temp();
            }
            else {
                $radio.prop('checked', true);
                $radio.data('waschecked', true);
            }

            // remove was checked from other radios
            $radio.siblings('input[type="radio"]').data('waschecked', false);


        }
        function rdouncheck2(elem) {

            // alert(elem.id)

            var samText = $(elem).parent().find('label').html();

            var hdnid = $(elem).parent().parent().parent().find('td:first').find('input[type=hidden]').eq(2).attr('id');

            $('#' + hdnid).val(samText);

            var $radio = $(elem);
            //if (id.checked==true ) {
            //    id.checked = false;
            //}
            //if ($radio.data('waschecked') == true) {
            //    $radio.prop('checked', false);
            //    $radio.data('waschecked', false);
            //    temp();
            //}
            //else {
            //    $radio.prop('checked', true);
            //    $radio.data('waschecked', true);
            //}

            //// remove was checked from other radios
            //$radio.siblings('input[type="radio"]').data('waschecked', false);


        }
        var IOAtype;
        function temp() {
            clearAllHf();
            <%clsDataSheet oDS = (clsDataSheet)Session["DataSht_Sess-" + hdnTemplateId.Value];%>
            var chaintype = "<%=oDS.SkillType.ToString()%>";
            var cntPrompt = "<%=oDS.CrntPrompt.ToString()%>";
            var prmpProcedure = "<%=oDS.PromptProc.ToString()%>";
            var totTask = "<%=oDS.ChainType.ToString()%>";
            var StepLevelPrompt = "<%=Session["StepLevelPrompt"]%>";
            var grid = document.getElementById('<%=grdDataSht.ClientID%>');
            var ChainingSubType = "<%=oDS.ChainType.ToString()%>";
            IOAtype = "<%=oDS.IOAInd.ToString()%>";
            if (grid != null) {
                var rowcount = parseInt(grid.rows.length - 1);
                var colcount = parseInt(grid.rows[0].cells.length);
                var totSteps = rowcount;
                for (var r = 1; r <= rowcount; r++) {
                    var txtnotes = $(grid.rows[r]).find("textarea")[0];
                    if (txtnotes != null) {
                        if (txtnotes.disabled == true) {
                            totSteps--;

                        }
                    }

                }


                var stepPrompts = document.getElementById('<%=hfcrntPrompt.ClientID%>').value;
                var obj = {};

                if (stepPrompts.length > 0) {
                    //var Prompts = stepPrompts.split(',');
                    //var selPrmptID = parseInt(Control.options[Control.selectedIndex].value);
                    var colPrmpt = stepPrompts.split('|');
                    for (var j = 0; j < colPrmpt.length - 1; j++) {
                        var Prompts = new Array();
                        for (var i = 0; i < colPrmpt[j].split('*')[1].split(',').length - 1; i++) {
                            Prompts[i] = colPrmpt[j].split('*')[1].split(',')[i];

                        }
                        var colId = colPrmpt[j].split('*')[0];
                        obj[colId] = Prompts;
                    }
                }
                var corrResp = document.getElementById('<%=hfPlusMinusResp.ClientID%>').value;
                var colDdlcnt = 0;
                for (var i = 1; i <= colcount - 3; i++) {
                    var PrmtAcc = 0, PrmtInd = 0; PrmtFail = 0; RespAcc = 0; RespInd = 0; FreqCount = 0; Secnds = 0, Txt = "", RespInCorrect = 0;
                    var PrmtExcAcc = 0, RespExcAcc = 0;
                    var trainingStep = 0; //Training step (in the case of Previously Learned Steps)
                    var trainingStep1 = 0;
                    var TxtArry = new Array();
                    var cType = $(grid.rows[0].cells[i]).find("span")[0].innerHTML;
                    var colmnName = $(grid.rows[0].cells[i]).find("b")[0].id;
                    totSteps = rowcount;
                    for (var j = 1; j <= rowcount; j++) {
                        /* Check mistrial checkbox is checked or not */
                        var isScored = 0;
                        //var chkmis = $(grid.rows[j]).find("input:checkbox")[0];
                        //if ((chkmis != null) && (chkmis.checked == true)) {   //dont do anything for this row..... 
                        //    if (i == 1)
                        //        totSteps--;
                        //}
                        //else {
                        if (cType == '+/-') {
                            /*  Count Calc for Resp  */
                            var resp = $(grid.rows[j].cells[i]).find("input:radio:checked").val();
                            if (resp != null) {
                                isScored++;
                            }
                            if (resp == corrResp) {
                                RespAcc++;
                                if ($('#lblPromptData').html() == 'Independent')
                                    RespInd++;
                            }
                            else if (resp != null) {

                                RespInCorrect++;
                            }
                            var lblExclude = $('#tbl_Measure').find('#AccuracyatPreviouslyLearnedSteps_' + colmnName)[0];
                            if (lblExclude != null) {
                                var exlHdns = $(grid.rows[j].cells[0]).find("input:hidden");

                                if (exlHdns != null) {
                                    if (exlHdns.length > 1) {
                                        var exlhdnstp = exlHdns[1].value;
                                        var crntStep = document.getElementById('<%=hfcrntStep.ClientID%>').value;

                                        if (crntStep != exlhdnstp) {
                                            if (resp == corrResp) {
                                                RespExcAcc++;
                                            }
                                            if (isScored == 0) {
                                                lblExclude.innerHTML = "-";
                                            }
                                        }
                                        else {
                                            if (resp != null) {
                                                trainingStep = 1;                                                
                                            }                                            
                                        }
                                    }
                                }
                            }
                            var lblStepAcc = $('#tbl_Measure').find('#AccuracyatTrainingStep_' + colmnName)[0];
                            if (lblStepAcc != null) {
                                var hdns = $(grid.rows[j].cells[0]).find("input:hidden");

                                if (hdns != null) {
                                    if (hdns.length > 1) {
                                        var hdnstp = hdns[1].value;
                                        var crntStep = document.getElementById('<%=hfcrntStep.ClientID%>').value;

                                        if (crntStep == hdnstp) {
                                            if (resp == corrResp) {
                                                lblStepAcc.innerHTML = '100%';
                                            } else {
                                                lblStepAcc.innerHTML = '0%';
                                            }

                                            if (isScored == 0) {
                                                lblStepAcc.innerHTML = "-";
                                            }
                                            var hfrsltStep = document.getElementById('<%=hfResultStep_Acc.ClientID%>');
                                            //document.getElementById('<%=hfResultStep_Acc.ClientID%>').value = lblStepAcc.innerHTML;
                                            hfrsltStep.value = resetHFvalue(colmnName, lblStepAcc.innerHTML, hfrsltStep.value);
                                            //alert(hfrsltStep.value);
                                        }
                                        else {
                                            if (isScored == 1 && lblStepAcc.innerHTML.toString().trim() != '100%') {
                                                //lblStepAcc.innerHTML = '0%';
                                                lblStepAcc.innerHTML = "-";
                                            }
                                        }


                                    }
                                }
                            }
                        }
                        if (cType == 'Prompt') {
                            /*  Count Calc for Prompt  */
                            var crntPrmptIndex = 0;
                            var ddl = grid.rows[j].cells[i].getElementsByTagName('select')[0];
                            if (ddl.options.selectedIndex > 0) {
                                //alert((ddl.options.selectedIndex);
                                isScored++;
                            }
                            var crntStep = document.getElementById('<%=hfcrntStep.ClientID%>').value;
                            for (var ddlIndex = 0; ddlIndex < ddl.options.length; ddlIndex++) {
                                if (parseInt(ddl.options[ddlIndex].value) == parseInt(obj[colmnName][j - 1])) {
                                    crntPrmptIndex = ddlIndex;

                                }
                            }
                            if (chaintype == "Chained") {
                                if (totTask == "Total Task") {
                                    if (StepLevelPrompt == "True") {
                                        var prmptID = parseInt(ddl.options[ddl.selectedIndex].value);
                                        if (parseInt(ddl.options[ddl.selectedIndex].value) >= Prompts[j - 1]) {
                                            PrmtAcc++;
                                        }
                                        if (ddl.options[ddl.selectedIndex].text == "Independent") {
                                            PrmtInd++;
                                        }
                                        if (ddl.options[ddl.selectedIndex].text == "Fail") {
                                            PrmtFail++;
                                        }
                                    }
                                    else {
                                        var prmptID = parseInt(ddl.options[ddl.selectedIndex].value);
                                        var SortOrder = SelectSortOrder(prmptID);
                                        var Prompt = ddl.options[ddl.selectedIndex].text;
                                        var CurrentPrompt = SelectSortOrder(cntPrompt);
                                        if (SortOrder > 0 || (SortOrder == 0 && Prompt == "Prompt A")) {
                                            if (SortOrder >= CurrentPrompt) {
                                                PrmtAcc++;
                                            }
                                        }
                                        if (ddl.options[ddl.selectedIndex].text == "Independent") {
                                            PrmtInd++;
                                        }
                                        if (ddl.options[ddl.selectedIndex].text == "Fail") {
                                            PrmtFail++;
                                        }
                                    }


                                }
                                else {

                                    var iCurrentRow = j;
                                    if (ChainingSubType == "Backward chain")
                                        iCurrentRow = rowcount - j + 1;

                                    if (crntStep == iCurrentRow) {
                                        if ((prmpProcedure == "Least-to-Most")||(prmpProcedure=="Graduated Guidance")) {
                                            var prmptID = parseInt(ddl.options[ddl.selectedIndex].value);
                                            if ((parseInt(ddl.selectedIndex) > 0) && parseInt(ddl.selectedIndex) <= crntPrmptIndex) {
                                                PrmtAcc++;
                                            }
                                            if (ddl.options[ddl.selectedIndex].text == "Independent") {
                                                PrmtInd++;
                                            }
                                            if (ddl.options[ddl.selectedIndex].text == "Fail") {
                                                PrmtFail++;
                                            }
                                        }
                                        else {
                                            var prmptID = parseInt(ddl.options[ddl.selectedIndex].value);
                                            if (parseInt(ddl.selectedIndex) >= crntPrmptIndex && prmptID > 0) {
                                                PrmtAcc++;
                                            }
                                            if (ddl.options[ddl.selectedIndex].text == "Independent") {
                                                PrmtInd++;
                                            }
                                            if (ddl.options[ddl.selectedIndex].text == "Fail") {
                                                PrmtFail++;
                                            }
                                        }


                                    }
                                    else {
                                        if ((prmpProcedure == "Least-to-Most")||(prmpProcedure=="Graduated Guidance")) {
                                            crntPrmptIndex = parseInt(ddl.options[1].index);
                                        }
                                        else {
                                            var length = ddl.options.length;
                                            crntPrmptIndex = parseInt(ddl.options[length - 2].index);
                                        }

                                        var prmptID = parseInt(ddl.options[ddl.selectedIndex].value);
                                        if (parseInt(ddl.selectedIndex) == crntPrmptIndex) {
                                            PrmtAcc++;

                                        }
                                        if (ddl.options[ddl.selectedIndex].text == "Independent") {
                                            PrmtInd++;
                                        }
                                        if (ddl.options[ddl.selectedIndex].text == "Fail") {
                                            PrmtFail++;
                                        }
                                    }
                                }
                            }
                            else {
                                //var prmptID = parseInt(ddl.options[ddl.selectedIndex].value);
                                //if (parseInt(ddl.selectedIndex) >= crntPrmptIndex) {
                                //    PrmtAcc++;
                                //}
                                //if (ddl.options[ddl.selectedIndex].text == "Independent") {
                                //    PrmtInd++;
                                //}
                                var prmptID = parseInt(ddl.options[ddl.selectedIndex].value);
                                if (prmptID >= cntPrompt) {
                                    PrmtAcc++;
                                }
                                if (ddl.options[ddl.selectedIndex].text == "Independent") {
                                    PrmtInd++;
                                }
                                if (ddl.options[ddl.selectedIndex].text == "Fail") {
                                    PrmtFail++;
                                }
                            }


                            var lblExcludePrmt = $('#tbl_Measure').find('#AccuracyatPreviouslyLearnedSteps_' + colmnName)[0];
                            if (lblExcludePrmt != null) {
                                var exlHdnsPrmt = $(grid.rows[j].cells[0]).find("input:hidden");

                                if (exlHdnsPrmt != null) {
                                    if (exlHdnsPrmt.length > 1) {
                                        var exlhdnstp = exlHdnsPrmt[1].value;
                                        var crntStep = document.getElementById('<%=hfcrntStep.ClientID%>').value;

                                            if (crntStep != exlhdnstp) {
                                                if ((prmpProcedure == "Least-to-Most")||(prmpProcedure=="Graduated Guidance")) {
                                                    if (ddl.selectedIndex != 0) {
                                                        if (parseInt(ddl.selectedIndex) <= crntPrmptIndex) {
                                                            PrmtExcAcc++;
                                                            trainingStep1 = 1;

                                                        }
                                                    }
                                                }
                                                else {
                                                    if (ddl.selectedIndex != 0) {
                                                        if (parseInt(ddl.selectedIndex) >= crntPrmptIndex) {
                                                            PrmtExcAcc++;
                                                            trainingStep1 = 1;

                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                trainingStep1 = 0;

                                                }
                                        }
                                    }
                                }
                                var lblStepPrmt = $('#tbl_Measure').find('#AccuracyatTrainingStep_' + colmnName)[0];
                                if (lblStepPrmt != null) {
                                    var hdns = $(grid.rows[j].cells[0]).find("input:hidden");

                                    if (hdns != null) {
                                        if (hdns.length > 1) {
                                            var hdnstp = hdns[1].value;
                                            var crntStep = document.getElementById('<%=hfcrntStep.ClientID%>').value;
                                            if (crntStep == hdnstp) {
                                                if ((prmpProcedure == "Least-to-Most")||(prmpProcedure=="Graduated Guidance")) {
                                                    if (ddl.selectedIndex != 0)
                                                    {
                                                        if (ddl.options[ddl.selectedIndex].text == "Fail") {
                                                            lblStepPrmt.innerHTML = '0%';
                                                            
                                                        }
                                                        else if (parseInt(ddl.selectedIndex) <= crntPrmptIndex) {
                                                            lblStepPrmt.innerHTML = '100%';
                                                        } else {
                                                            lblStepPrmt.innerHTML = '0%';
                                                        }
                                                    }
                                                    else {
                                                        lblStepPrmt.innerHTML = '-';
                                                    }
                                                }
                                                else {

                                                    if (ddl.selectedIndex != 0) {

                                                        if (ddl.options[ddl.selectedIndex].text == "Fail") {
                                                            lblStepPrmt.innerHTML = '0%';

                                                        }
                                                       else if (parseInt(ddl.selectedIndex) >= crntPrmptIndex) {
                                                            lblStepPrmt.innerHTML = '100%';
                                                        } else {
                                                            lblStepPrmt.innerHTML = '0%';
                                                        }
                                                    }
                                                    else {
                                                        lblStepPrmt.innerHTML = '-';
                                                    }
                                                }

                                                var hfrsltStep = document.getElementById('<%=hfResultStep_Prmpt.ClientID%>');
                                                //document.getElementById('<%=hfResultStep_Prmpt.ClientID%>').value = lblStepAcc.innerHTML;
                                                hfrsltStep.value = resetHFvalue(colmnName, lblStepPrmt.innerHTML, hfrsltStep.value);
                                                //alert(hfrsltStep.value);
                                            }

                                            else {
                                                if (isScored == 1 && lblStepPrmt.innerHTML.toString().trim() != '100%') {
                                                    lblStepPrmt.innerHTML = '0%';
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (cType == 'Frequency') {
                                /* Count Calc for Frequency  */

                                var txtfrq = $(grid.rows[j].cells[i]).find(".clsFreq")[0];
                                if (txtfrq.value != '') {
                                    FreqCount += parseInt(txtfrq.value);
                                    isScored++;
                                }
                            }
                            if (cType == 'Duration') {
                                /* Time Calc for Duration */

                                var txtdur = $(grid.rows[j].cells[i]).find(".clsDuratn")[0];
                                Secnds += spiltTime(txtdur.value) - 1;
                                if (txtdur.value != "00:00:00") {
                                    isScored++;
                                }
                            }
                            if (cType == 'Text') {
                                var txtcust = $(grid.rows[j].cells[i]).find(".clsText")[0];
                                if (txtcust.value == '') {
                                    txtcust.value = '';
                                    isScored++;
                                }

                            }
                        //alert(isScored);
                            if (isScored == 0) {
                                //alert(totSteps)
                                totSteps--;
                            }

                        // }

                        }
                    //var colmnName = $(grid.rows[0].cells[i]).find("b")[0].innerHTML.replace(/ /g, "");

                        if (cType == '+/-') {
                            var Resp_AccPerc = 0;
                            var Resp_IndPerc = 0;
                            var Resp_IndPercAll = 0;
                            var Resp_AccExc = 0;
                            var Check_na = 0;
                            if (totSteps > 0) {
                                Resp_AccPerc = parseInt((RespAcc / totSteps) * 100);
                                Resp_IndPerc = parseInt((RespInd / totSteps) * 100);
                                Resp_IndPercAll = parseInt((RespInd / rowcount) * 100);
                                //alert('totStep:' + totSteps);
                                //alert(RespExcAcc);
                                if ((totSteps > 1) && (trainingStep == 1)) {
                                    Resp_AccExc = parseInt((RespExcAcc / (totSteps - 1)) * 100);
                                    Check_na = 1;
                                }
                                else if ((totSteps >= 1) && (trainingStep == 0)) {
                                    Resp_AccExc = parseInt((RespExcAcc / (totSteps)) * 100);
                                    Check_na = 1;

                                }
                                else {
                                    Resp_AccExc = 0;
                                }
                            }


                            var lblAcc = $('#tbl_Measure').find('#Accuracy_' + colmnName)[0];
                            var lblInd = $('#tbl_Measure').find('#Independent_' + colmnName)[0];
                            var lblIndAll = $('#tbl_Measure').find('#IndependentofAllSteps_' + colmnName)[0];
                            var lblPrmt = $('#tbl_Measure').find('#Prompted_' + colmnName)[0];
                            //liju
                            var lblTotalCorrect = $('#tbl_Measure').find('#TotalCorrect_' + colmnName)[0];
                            var lblTotalInCorrect = $('#tbl_Measure').find('#TotalIncorrect_' + colmnName)[0];
                            var lblExcludePrev = $('#tbl_Measure').find('#AccuracyatPreviouslyLearnedSteps_' + colmnName)[0];
                            if (lblExcludePrev != null) {
                                if (Check_na == 1) {
                                    lblExcludePrev.innerHTML = Resp_AccExc + '%';
                                }
                                else {
                                    lblExcludePrev.innerHTML = "-";
                                }
                                var hfexclude = document.getElementById('<%=hfRslt1_ExcludeCrntStep_Acc.ClientID%>');
                                hfexclude.value = resetHFvalue(colmnName, lblExcludePrev.innerHTML, hfexclude.value);
                            }
                            var lblStepAcc_1 = $('#tbl_Measure').find('#AccuracyatTrainingStep_' + colmnName)[0];
                            var hfrsltStep_1 = document.getElementById('<%=hfResultStep_Acc.ClientID%>');
                            //else
                            //{
                            //    if (lblAcc != null) lblAcc.innerHTML = Resp_AccPerc + '%';
                            //}


                             if (lblAcc != null) lblAcc.innerHTML = Resp_AccPerc + '%';
                             if (lblInd != null) lblInd.innerHTML = Resp_IndPerc + '%';
                             if (lblIndAll != null) lblIndAll.innerHTML = Resp_IndPercAll + '%';
                            //if (lblPrmt != null) lblPrmt.innerHTML = (100 - parseInt(Resp_IndPerc)) + '%';  // previous code

                             if (lblPrmt != null) lblPrmt.innerHTML = (parseInt(Resp_AccPerc) - parseInt(Resp_IndPerc)) + '%';   // modified code. %P = %A - %I

                             if (lblAcc != null) var hfrsltacc1 = document.getElementById('<%=hfRslt1_Acc.ClientID%>');
                            if (lblInd != null) var hfrsltind1 = document.getElementById('<%=hfRslt1_Ind.ClientID%>');
                            if (lblIndAll != null) var hfrsltind1All = document.getElementById('<%=hfRslt1_IndAll.ClientID%>');
                            if (lblPrmt != null) var hfrsltpmt1 = document.getElementById('<%=hfRslt1_Prmt.ClientID%>');
                            if (lblAcc != null) hfrsltacc1.value = resetHFvalue(colmnName, lblAcc.innerHTML, hfrsltacc1.value);
                            if (lblInd != null) hfrsltind1.value = resetHFvalue(colmnName, lblInd.innerHTML, hfrsltind1.value);
                            if (lblIndAll != null) hfrsltind1All.value = resetHFvalue(colmnName, lblIndAll.innerHTML, hfrsltind1All.value);
                            if (lblPrmt != null) hfrsltpmt1.value = resetHFvalue(colmnName, lblPrmt.innerHTML, hfrsltpmt1.value);
                            //liju
                            //if (lblTotalCorrect != null) lblTotalCorrect.innerHTML = RespAcc; document.getElementById('<%=hfTotCorct.ClientID%>').value = RespAcc;
                            //if (lblTotalInCorrect != null) lblTotalInCorrect.innerHTML = RespInCorrect; document.getElementById('<%=hfInTotCorct.ClientID%>').value = RespInCorrect;

                            if (lblTotalCorrect != null) {
                                lblTotalCorrect.innerHTML = RespAcc;
                                var hfTotCorct = document.getElementById('<%=hfTotCorct.ClientID%>');
                                if (hfTotCorct != null) {
                                    hfTotCorct.value = RespAcc;
                                }
                            }

                            if (lblTotalInCorrect != null) {
                                lblTotalInCorrect.innerHTML = RespInCorrect;
                                var hfInTotCorct = document.getElementById('<%=hfInTotCorct.ClientID%>');
                                if (hfInTotCorct != null) {
                                    hfInTotCorct.value = RespInCorrect;
                                }
                            }
                            //alert(" hfRslt1_Acc before totSteps--" + hfrsltacc1.value)
                            //document.getElementById('<%=hfRslt1_Acc.ClientID%>').value = Resp_AccPerc + '%';
                            //document.getElementById('<%=hfRslt1_Ind.ClientID%>').value = Resp_IndPerc + '%';
                            //document.getElementById('<%=hfRslt1_Prmt.ClientID%>').value = (100 - parseInt(Resp_IndPerc)) + '%';


                            if (totSteps == 0) {
                                //alert("totSteps----" + totSteps)
                                if (lblAcc != null) lblAcc.innerHTML = "N/A";
                                var hfRslt1_Acc = document.getElementById('<%=hfRslt1_Acc.ClientID%>');
                                 //hfRslt1_Acc.value = "N/A";
                                 if (lblAcc != null) {
                                     hfRslt1_Acc.value = removeLastHfValue(hfRslt1_Acc.value);
                                     hfRslt1_Acc.value = resetHFvalue(colmnName, lblAcc.innerHTML, hfRslt1_Acc.value);
                                 }


                                 //alert("After reset--" + hfRslt1_Acc.value);
                                 if (lblInd != null) lblInd.innerHTML = "N/A";
                                 var hfRslt1_Ind = document.getElementById('<%=hfRslt1_Ind.ClientID%>');
                                if (lblInd != null) {
                                    hfRslt1_Ind.value = removeLastHfValue(hfRslt1_Ind.value);
                                    hfRslt1_Ind.value = resetHFvalue(colmnName, lblInd.innerHTML, hfRslt1_Ind.value);
                                }

                                if (lblTotalCorrect != null) lblTotalCorrect.innerHTML = "N/A";
                                var hfTotCorct = document.getElementById('<%=hfTotCorct.ClientID%>');
                            if (lblTotalCorrect != null) {
                                hfTotCorct.value = removeLastHfValue(hfTotCorct.value);
                                hfTotCorct.value = resetHFvalue(colmnName, lblTotalCorrect.innerHTML, hfTotCorct.value);
                            }


                            if (lblTotalInCorrect != null) lblTotalInCorrect.innerHTML = "N/A";
                            var hfInTotCorct = document.getElementById('<%=hfInTotCorct.ClientID%>');
                            if (lblTotalInCorrect != null) {
                                hfInTotCorct.value = removeLastHfValue(hfInTotCorct.value);
                                hfInTotCorct.value = resetHFvalue(colmnName, lblTotalInCorrect.innerHTML, hfInTotCorct.value);
                            }

                            if (lblPrmt != null) lblPrmt.innerHTML = "N/A";
                            var hfRslt1_Prmt = document.getElementById('<%=hfRslt1_Prmt.ClientID%>');
                            if (lblPrmt != null) {
                                hfRslt1_Prmt.value = removeLastHfValue(hfRslt1_Prmt.value);
                                hfRslt1_Prmt.value = resetHFvalue(colmnName, lblPrmt.innerHTML, hfRslt1_Prmt.value);
                            }

                            if (lblIndAll != null) lblIndAll.innerHTML = "N/A";
                            var hfRslt1_IndAll = document.getElementById('<%=hfRslt1_IndAll.ClientID%>');
                            if (lblIndAll != null) {
                                hfRslt1_IndAll.value = removeLastHfValue(hfRslt1_IndAll.value);
                                hfRslt1_IndAll.value = resetHFvalue(colmnName, lblIndAll.innerHTML, hfRslt1_IndAll.value);
                            }

                            if (lblExcludePrev != null) lblExcludePrev.innerHTML = "N/A";
                            var hfRslt1_ExcludeCrntStep_Acc = document.getElementById('<%=hfRslt1_ExcludeCrntStep_Acc.ClientID%>');
                            if (lblExcludePrev != null) {
                                hfRslt1_ExcludeCrntStep_Acc.value = removeLastHfValue(hfRslt1_ExcludeCrntStep_Acc.value);
                                hfRslt1_ExcludeCrntStep_Acc.value = resetHFvalue(colmnName, lblExcludePrev.innerHTML, hfRslt1_ExcludeCrntStep_Acc.value);
                            }

                            var hfResultStep_Acc = document.getElementById('<%=hfResultStep_Acc.ClientID%>');
                            hfResultStep_Acc.value = "N/A";
                            if (lblStepAcc_1 != null) lblStepAcc_1.innerHTML = "N/A";
                            hfrsltStep_1.value = "N/A";

                        }

                    }
                    if (cType == 'Prompt') {
                        /*  Percentage Calc for Prompt  */
                        var Prmt_AccPerc = 0;
                        var Prmt_IndPerc = 0;
                        var Prmt_IndPercForAll = 0;

                        var Resp_AccExc = 0;
                        if (totSteps > 0) {
                            Prmt_AccPerc = parseInt((PrmtAcc / totSteps) * 100);

                            Prmt_IndPerc = parseInt((PrmtInd / totSteps) * 100);
                            Prmt_IndPercForAll = parseInt((PrmtInd / rowcount) * 100);
                            //if (totSteps > 1) {
                            //    Resp_AccExc = parseInt((PrmtExcAcc / (totSteps - 1)) * 100);
                            //} else {
                            //    Resp_AccExc = 0;
                            //}


                            if (totSteps > 1){
                                Resp_AccExc = parseInt((PrmtExcAcc / (totSteps - 1)) * 100);
                                
                            }
                            else if (totSteps >= 1) {
                                Resp_AccExc = parseInt((PrmtExcAcc / (totSteps)) * 100);
                                

                            }
                            else {
                                Resp_AccExc = 0;
                            }

                        }
                        var lblAcc = $('#tbl_Measure').find('#Accuracy_' + colmnName)[0];
                        var lblInd = $('#tbl_Measure').find('#Independent_' + colmnName)[0];
                        var lblIndAll = $('#tbl_Measure').find('#IndependentofAllSteps_' + colmnName)[0];
                        var lblPrmt = $('#tbl_Measure').find('#Prompted_' + colmnName)[0];//Independent of All Steps
                        if (lblAcc != null) lblAcc.innerHTML = Prmt_AccPerc + '%';
                        if (lblInd != null) lblInd.innerHTML = Prmt_IndPerc + '%';
                        if (lblIndAll != null) lblIndAll.innerHTML = Prmt_IndPercForAll + '%';
                        //if (lblPrmt != null) lblPrmt.innerHTML = (100 - parseInt(Prmt_IndPerc)) + '%'; // old calculation-without "Fail" option

                        /// New calculation - included "Fail" count.
                        if (lblPrmt != null) lblPrmt.innerHTML = ((parseInt(totSteps) - parseInt(PrmtInd) - parseInt(PrmtFail)) / parseInt(totSteps)) * 100 + '%';


                        var lblExclude2 = $('#tbl_Measure').find('#AccuracyatPreviouslyLearnedSteps_' + colmnName)[0];
                        if (lblExclude2 != null) {
                            if (trainingStep1 == 1) {

                                lblExclude2.innerHTML = Resp_AccExc + '%';
                            }
                            else {
                                lblExclude2.innerHTML = '-';

                            }
                            var hfexclude = document.getElementById('<%=hfRslt2_ExcludeCrntStep_Acc.ClientID%>');
                            hfexclude.value = resetHFvalue(colmnName, lblExclude2.innerHTML, hfexclude.value);
                        }

                        var lblStepPrmt_1 = $('#tbl_Measure').find('#AccuracyatTrainingStep_' + colmnName)[0];
                        var hfrsltStep_1 = document.getElementById('<%=hfResultStep_Prmpt.ClientID%>');

                        if (lblAcc != null) var hfrsltacc2 = document.getElementById('<%=hfRslt2_Acc.ClientID%>');
                        if (lblInd != null) var hfrsltind2 = document.getElementById('<%=hfRslt2_Ind.ClientID%>');
                        if (lblIndAll != null) var hfrsltind2All = document.getElementById('<%=hfRslt2_IndAll.ClientID%>');
                        if (lblPrmt != null) var hfrsltpmt2 = document.getElementById('<%=hfRslt2_Prmt.ClientID%>');
                        if (lblAcc != null) hfrsltacc2.value = resetHFvalue(colmnName, lblAcc.innerHTML, hfrsltacc2.value);
                        if (lblInd != null) hfrsltind2.value = resetHFvalue(colmnName, lblInd.innerHTML, hfrsltind2.value);
                        if (lblIndAll != null) hfrsltind2All.value = resetHFvalue(colmnName, lblIndAll.innerHTML, hfrsltind2All.value);
                        if (lblPrmt != null) hfrsltpmt2.value = resetHFvalue(colmnName, lblPrmt.innerHTML, hfrsltpmt2.value);



                        if (totSteps == 0) {
                            var hfRslt2_ExcludeCrntStep_Acc = document.getElementById('<%=hfRslt2_ExcludeCrntStep_Acc.ClientID%>');
                            var hfRslt2_Acc = document.getElementById('<%=hfRslt2_Acc.ClientID%>');
                            var hfRslt2_Prmt = document.getElementById('<%=hfRslt2_Prmt.ClientID%>');
                            var hfRslt2_Ind = document.getElementById('<%=hfRslt2_Ind.ClientID%>');
                            var hfRslt2_IndAll = document.getElementById('<%=hfRslt2_IndAll.ClientID%>');
                            var hfResultStep_Prmpt = document.getElementById('<%=hfResultStep_Prmpt.ClientID%>');
                            hfRslt2_ExcludeCrntStep_Acc.value = "N/A";
                            hfRslt2_Acc.value = "N/A";
                            hfRslt2_Prmt.value = "N/A";
                          //  hfRslt2_Ind.value = "N/A";
                            hfRslt2_IndAll.value = "N/A";
                            hfResultStep_Prmpt.value = "N/A";
                            if (lblAcc != null) {
                                lblAcc.innerHTML = "N/A";
                                hfRslt2_Acc.value = removeLastHfValue(hfRslt2_Acc.value);
                                hfRslt2_Acc.value = resetHFvalue(colmnName, lblAcc.innerHTML, hfRslt2_Acc.value);
                            }
                            if (lblInd != null) {
                                lblInd.innerHTML = "N/A";
                                hfRslt2_Ind.value = removeLastHfValue(hfRslt2_Ind.value);
                                hfRslt2_Ind.value = resetHFvalue(colmnName, lblInd.innerHTML, hfRslt2_Ind.value);
                            }
                            if (lblPrmt != null) {
                                lblPrmt.innerHTML = "N/A";
                                hfRslt2_Prmt.value = removeLastHfValue(hfRslt2_Prmt.value);
                                hfRslt2_Prmt.value = resetHFvalue(colmnName, lblPrmt.innerHTML, hfRslt2_Prmt.value);
                            }
                            if (lblIndAll != null) {
                                lblIndAll.innerHTML = "N/A";
                                hfRslt2_IndAll.value = removeLastHfValue(hfRslt2_IndAll.value);
                                hfRslt2_IndAll.value = resetHFvalue(colmnName, lblIndAll.innerHTML, hfRslt2_IndAll.value);
                            }
                            if (lblExclude2 != null) {
                                lblExclude2.innerHTML = "N/A";
                                hfRslt1_ExcludeCrntStep_Acc.value = removeLastHfValue(hfRslt1_ExcludeCrntStep_Acc.value);
                                hfRslt1_ExcludeCrntStep_Acc.value = resetHFvalue(colmnName, lblExclude2.innerHTML, hfRslt1_ExcludeCrntStep_Acc.value);
                                //-----
                                //hfRslt1_ExcludeCrntStep_Acc.value = resetHFvalue(colmnName, lblExcludePrev.innerHTML, hfRslt1_ExcludeCrntStep_Acc.value);
                            }
                            if (lblStepPrmt != null) {
                                lblStepPrmt.innerHTML = "N/A";
                                hfResultStep_Prmpt.value = removeLastHfValue(hfResultStep_Prmpt.value);
                                hfResultStep_Prmpt.value = resetHFvalue(colmnName, lblStepPrmt.innerHTML, hfResultStep_Prmpt.value);
                                //-----
                                //hfResultStep_Prmpt.value = resetHFvalue(colmnName, lblExcludePrev.innerHTML, hfResultStep_Prmpt.value);
                            }
                            if (lblStepPrmt_1 != null) lblStepPrmt_1.innerHTML = "N/A";
                            hfrsltStep_1.value = "N/A";
                        }


                        var promptIDs = 0;

                        for (var r = 1; r <= rowcount; r++) {
                            var ddlprmpt = $(grid.rows[r]).find("select")[colDdlcnt];
                            var selVal = parseInt(ddlprmpt.options[ddlprmpt.selectedIndex].value);
                            if (selVal == -2) {
                                promptIDs = promptIDs + 1;
                            }
                        }
                        colDdlcnt = colDdlcnt + 1;
                        if (promptIDs == rowcount) {
                            var hfRslt2_ExcludeCrntStep_Acc = document.getElementById('<%=hfRslt2_ExcludeCrntStep_Acc.ClientID%>');
                            var hfRslt2_Acc = document.getElementById('<%=hfRslt2_Acc.ClientID%>');
                            var hfRslt2_Prmt = document.getElementById('<%=hfRslt2_Prmt.ClientID%>');
                            var hfRslt2_Ind = document.getElementById('<%=hfRslt2_Ind.ClientID%>');
                            var hfRslt2_IndAll = document.getElementById('<%=hfRslt2_IndAll.ClientID%>');
                            var hfResultStep_Prmpt = document.getElementById('<%=hfResultStep_Prmpt.ClientID%>');
                            hfRslt2_ExcludeCrntStep_Acc.value = "0%";
                            hfRslt2_Acc.value = "0%";
                            //hfRslt2_Prmt.value = "0%";
                            hfRslt2_Ind.value = "0%";
                            hfRslt2_IndAll.value = "0%";
                            hfResultStep_Prmpt.value = "0%";
                            if (lblAcc != null) lblAcc.innerHTML = "0%";
                            if (lblInd != null) lblInd.innerHTML = "0%";
                            //if (lblPrmt != null) lblPrmt.innerHTML = "0%";
                            if (lblIndAll != null) lblIndAll.innerHTML = "0%";
                            if (lblExclude2 != null) lblExclude2.innerHTML = "0%";
                            if (lblStepPrmt != null) lblStepPrmt.innerHTML = "0%";
                            if (lblStepPrmt_1 != null) lblStepPrmt_1.innerHTML = "0%";
                            hfrsltStep_1.value = "0%";
                        }

                        //document.getElementById('<%=hfRslt2_Acc.ClientID%>').value = Prmt_AccPerc + '%';
                        //document.getElementById('<%=hfRslt2_Ind.ClientID%>').value = Prmt_IndPerc + '%';
                        //document.getElementById('<%=hfRslt2_Prmt.ClientID%>').value = (100 - parseInt(Prmt_IndPerc)) + '%';
                    }
                    if (cType == 'Frequency') {
                        var lblFreq = $('#tbl_Measure').find('#Frequency_' + colmnName)[0];
                        /*  Display Frequency Count  */
                        if (lblFreq != null) lblFreq.innerHTML = FreqCount;


                        if (lblFreq != null) {
                            var hffreq = document.getElementById('<%=hf_Freq.ClientID%>');
                            hffreq.value = resetHFvalue(colmnName, lblFreq.innerHTML, hffreq.value);
                        }
                        if (totSteps == 0) {
                            if (lblFreq != null) {
                                lblFreq.innerHTML = "N/A";
                                var hf_freq = document.getElementById('<%=hf_Freq.ClientID%>');
                                hf_freq.value = removeLastHfValue(hf_freq.value);
                                hf_freq.value = resetHFvalue(colmnName, lblFreq.innerHTML, hf_freq.value);
                            }
                        }
                        //document.getElementById('<%=hf_Freq.ClientID%>').value = FreqCount;

                    }
                    if (cType == 'Duration') {
                        var lblAvg = $('#tbl_Measure').find('#AvgDuration_' + colmnName)[0];
                        var lblTot = $('#tbl_Measure').find('#TotalDuration_' + colmnName)[0];
                        /*  Time Calc for Duration  */
                        var avgTime = '00:00:00';
                        if (totSteps > 0) {
                            avgTime = formatTime(parseInt(Secnds / totSteps));
                        }
                        if (lblAvg != null) lblAvg.innerHTML = avgTime;
                        if (lblTot != null) lblTot.innerHTML = formatTime(Secnds);

                        if (lblAvg != null) var hfAvgdur = document.getElementById('<%=hfAvgDur.ClientID%>');
                        if (lblTot != null) var hfTotdur = document.getElementById('<%=hfTotDur.ClientID%>');
                        if (lblAvg != null) hfAvgdur.value = resetHFvalue(colmnName, lblAvg.innerHTML, hfAvgdur.value);
                        if (lblTot != null) hfTotdur.value = resetHFvalue(colmnName, lblTot.innerHTML, hfTotdur.value);

                        if (totSteps == 0) {
                            var hf_Avgdur = document.getElementById('<%=hfAvgDur.ClientID%>');
                            var hf_Totdur = document.getElementById('<%=hfTotDur.ClientID%>');
                            if (lblAvg != null) {
                                lblAvg.innerHTML = "N/A";
                                hf_Avgdur.value = removeLastHfValue(hf_Avgdur.value);
                                hf_Avgdur.value = resetHFvalue(colmnName, lblAvg.innerHTML, hf_Avgdur.value);
                            }

                            if (lblTot != null) {
                                lblTot.innerHTML = "N/A";
                                hf_Totdur.value = removeLastHfValue(hf_Totdur.value);
                                hf_Totdur.value = resetHFvalue(colmnName, lblTot.innerHTML, hf_Totdur.value);
                            }
                        }
                        //document.getElementById('<%=hfAvgDur.ClientID%>').value = avgTime;
                        //document.getElementById('<%=hfTotDur.ClientID%>').value = formatTime(Secnds);
                    }
                    if (cType == 'Text') {

                        var lblTxt = $('#tbl_Measure').find('#Customize_' + colmnName)[0];

                        if (lblTxt != null) {
                            var hf_Customize = document.getElementById('<%=hfTextScore.ClientID%>');
                            hf_Customize.value = resetHFvalue(colmnName, lblTxt.innerHTML, hf_Customize.value);
                            if (hf_Customize.value != null || hf_Customize.value != "") {

                                if (hf_Customize.value.split('|')[0].split('*')[1] == 0) {
                                    lblTxt.innerHTML = "N/A";
                                    hf_Customize.value = resetHFvalue(colmnName, lblTxt.innerHTML, hf_Customize.value);
                                }
                            }
                        }
                       <%-- if (totSteps == 0) {
                            if (lblTxt != null) {
                                lblTxt.innerHTML = "N/A";
                                var hf_Customize = document.getElementById('<%=hfTextScore.ClientID%>');
                                hf_Customize.value = resetHFvalue(colmnName, lblTxt.innerHTML, hf_Customize.value);
                            }
                        }--%>

                        var lblNA = $('#tbl_Measure').find('#NA_' + colmnName)[0];
                        if (lblNA != null) lblNA.innerHTML = "N/A";


                        var IsExist = 0;
                        var IdVal = 0;
                        var links = document.getElementsByClassName("clsText");
                        var IDData = [].map.call(links, function (el) {
                            var Idstring = el.getAttribute("id");
                            if (Idstring.indexOf(colmnName) > -1) {
                                IdVal++;
                                if ($('#' + Idstring).val() == "") {
                                    IsExist++;
                                }
                            }
                            return el.getAttribute("id");
                        });


                        if (IdVal == IsExist) {
                            var lblTxt = $('#tbl_Measure').find('#Customize_' + colmnName)[0];
                            if (lblTxt != null) {
                                lblTxt.innerHTML = "N/A";
                            }
                        }

                        //for (var i = 0; i < values.length; i++) {
                        //    if (values[i].indexOf(colmnName) > -1) {
                        //        IdVal = IdVal + 1;
                        //        alert(values[i]);
                        //    }
                        //}

                        //var lblTxt = $('#tbl_Measure').find('#Customize_' + colmnName)[0];
                        //PageMethods.ScoreForText(TxtArry, colmnName, function (data) {
                        //    lblTxt.innerHTML = data;
                        //});
                    }


                }
            }

        }
        function clearAllHf() {
            document.getElementById('<%=hfRslt1_Acc.ClientID%>').value = "";
            document.getElementById('<%=hfRslt1_Ind.ClientID%>').value = "";
            document.getElementById('<%=hfRslt1_Prmt.ClientID%>').value = "";

            document.getElementById('<%=hfRslt2_Acc.ClientID%>').value = "";
            document.getElementById('<%=hfRslt2_Ind.ClientID%>').value = "";
            document.getElementById('<%=hfRslt2_IndAll.ClientID%>').value = "";
            document.getElementById('<%=hfRslt2_Prmt.ClientID%>').value = "";

            document.getElementById('<%=hf_Freq.ClientID%>').value = "";

            document.getElementById('<%=hfAvgDur.ClientID%>').value = "";
            document.getElementById('<%=hfTotDur.ClientID%>').value = "";
            document.getElementById('<%=hfTotCorct.ClientID%>').value = "";
            document.getElementById('<%=hfInTotCorct.ClientID%>').value = "";

            document.getElementById('<%=hfResultStep_Acc.ClientID%>').value = "";
            document.getElementById('<%=hfResultStep_Prmpt.ClientID%>').value = "";

        }

        function removeLastHfValue(hfvalue) {
            hfvalue = hfvalue.substring(0, hfvalue.length - 2);
            var hfArr = hfvalue.split('|');
            hfvalue = "";
            if (hfArr.length > 0) {

                for (var i = 0; i < hfArr.length - 1; i++) {
                    hfvalue += hfArr[i] + "|";
                }
            }
            return hfvalue;


        }

        function resetHFvalue(colname, value, hf) {
            //var items = hf.split('|');
            //if (hf.length == 0) {
            hf += colname + '*' + value + '|';
            //}
            //else {
            //hf = '';

            //for (var i = 0; i < items.length; i++) {
            //    var col = items[i].split('*')[0];
            //    var val = items[i].split('*')[1];
            //    if (col == colname) {
            //        hf += colname + '*' + value + '|';
            //    }
            //    else {
            //        hf += col + '*' + val + '|';
            //    }
            //}

            //}

            //hf = hf.substring(0, hf.length - 1);
            return hf;
        }
        function getscore() {
            var grid = document.getElementById('<%=grdDataSht.ClientID%>');
        if (grid != null) {
            var rowcount = parseInt(grid.rows.length - 1);
            var totSteps = rowcount;
            var stepPrompts = parseInt(document.getElementById('<%=hfcrntPrompt.ClientID%>').value);
            //var selPrmptID = parseInt(Control.options[Control.selectedIndex].value);

            var corrResp = document.getElementById('<%=hfPlusMinusResp.ClientID%>').value;

            var PrmtAcc = 0, PrmtInd = 0; RespAcc = 0; RespInd = 0; FreqCount = 0; Secnds = 0, RespInCorrect = 0;
            for (var i = 1; i <= rowcount; i++) {
                /* Check mistrial checkbox is checked or not */
                var chkmis = $(grid.rows[i]).find("input:checkbox")[0];
                if (chkmis.checked == false) {
                    /*  Count Calc for Prompt  */
                    var ddl = grid.rows[i].getElementsByTagName('select')[0];
                    var prmptID = parseInt(ddl.options[ddl.selectedIndex].value);
                    if (prmptID >= stepPrompts) {
                        PrmtAcc++;
                    }
                    if (ddl.options[ddl.selectedIndex].text == "Independent") {
                        PrmtInd++;
                    }

                    /*  Count Calc for Resp  */
                    //var radio = grid.rows[i].getElementsByTagName('input:radio');
                    var resp = $(grid.rows[i]).find("input:radio:checked").val();
                    if (resp == corrResp) {
                        RespAcc++;
                        RespInd++;
                    }
                    else if (resp != null) {
                        RespInCorrect++;
                    }
                    /* Count Calc for Frequency  */
                    var txtfrq = $(grid.rows[i]).find(".clsFreq")[0];
                    if (txtfrq.value != '') {
                        FreqCount += parseInt(txtfrq.value);
                    }
                    /* Time Calc for Duration */
                    var txtdur = $(grid.rows[i]).find(".clsDuratn")[0];
                    Secnds += spiltTime(txtdur.value) - 1;
                } else {
                    totSteps--;
                }

            }
            /*  Display Frequency Count  */
            document.getElementById('<%=lbl_Freq.ClientID%>').innerHTML = FreqCount;
                document.getElementById('<%=hf_Freq.ClientID%>').value = FreqCount;

            /*  Time Calc for Duration  */
            var avgTime = '00:00:00';
            if (totSteps > 0) {
                avgTime = formatTime(parseInt(Secnds / totSteps));
            }
            document.getElementById('<%=lblAvgDur.ClientID%>').innerHTML = avgTime;
                document.getElementById('<%=lblTotDur.ClientID%>').innerHTML = formatTime(Secnds);

            document.getElementById('<%=hfAvgDur.ClientID%>').value = avgTime;
            document.getElementById('<%=hfTotDur.ClientID%>').value = formatTime(Secnds);
            /*  Percentage Calc for Prompt  */
            var Prmt_AccPerc = 0;
            var Prmt_IndPerc = 0;
            if (totSteps > 0) {
                Prmt_AccPerc = parseInt((PrmtAcc / totSteps) * 100);
                Prmt_IndPerc = parseInt((PrmtInd / totSteps) * 100);
            }
            document.getElementById('<%=lblRslt2_Acc.ClientID%>').innerHTML = Prmt_AccPerc + '%';
                document.getElementById('<%=lblRslt2_Ind.ClientID%>').innerHTML = Prmt_IndPerc + '%';
            document.getElementById('<%=lblRslt2_Prmt.ClientID%>').innerHTML = (100 - parseInt(Prmt_IndPerc)) + '%';

            document.getElementById('<%=hfRslt2_Acc.ClientID%>').value = Prmt_AccPerc + '%';
            document.getElementById('<%=hfRslt2_Ind.ClientID%>').value = Prmt_IndPerc + '%';
            document.getElementById('<%=hfRslt2_Prmt.ClientID%>').value = (100 - parseInt(Prmt_IndPerc)) + '%';
            /*  Percentage Calc for Response  */
            var Resp_AccPerc = 0;
            var Resp_IndPerc = 0;
            if (totSteps > 0) {
                Resp_AccPerc = parseInt((RespAcc / totSteps) * 100);
                Resp_IndPerc = parseInt((RespInd / totSteps) * 100);
            }
            document.getElementById('<%=lblRslt1_Acc.ClientID%>').innerHTML = Resp_AccPerc + '%';
                document.getElementById('<%=lblRslt1_Ind.ClientID%>').innerHTML = Resp_IndPerc + '%';
            document.getElementById('<%=lblRslt1_Prmt.ClientID%>').innerHTML = (100 - parseInt(Resp_AccPerc)) + '%';

            document.getElementById('<%=hfRslt1_Acc.ClientID%>').value = Resp_AccPerc + '%';
            document.getElementById('<%=hfRslt1_Ind.ClientID%>').value = Resp_IndPerc + '%';
            document.getElementById('<%=hfRslt1_Prmt.ClientID%>').value = (100 - parseInt(Resp_AccPerc)) + '%';
        }
    }
    function setTimer() {
        var hf = document.getElementById('<%=hfAutoSaveCount.ClientID%>');
            hf.value = 1;
            if (parseInt(hf.value) > 0) {
                var timer = $find("<%=Timer1.ClientID%>");
                timer._startTimer();
            }
        }
        function stopTimer() {
            var timer = $find("<%=Timer1.ClientID%>");
            timer._stopTimer();
            var hf = document.getElementById('<%=hfAutoSaveCount.ClientID%>');
            if (parseInt(hf.value) > 0) {
                $('#autoSave_Msg').html('Data Saving...');
                $('#autoSave_Msg').fadeIn('slow');
                window.setTimeout(function () { $('#autoSave_Msg').html('Data Saved'); }, 1000);
                window.setTimeout(function () { $('#autoSave_Msg').fadeOut('slow'); }, 2000);
            }
        }
        function loadStopwatch(div) {
            addStopwatch(div, 0);
        }
        function refreshPage() {
            var divDtSht = parent.document.getElementById('DATASHEETS');
            divDtSht.click();
        }

        var mouseX, mouseY, windowWidth, windowHeight;
        var popupLeft, popupTop;
        var endTime;
        var editEndtime;
        //function to find the position of mouse pointer
        $(document).ready(function () {
            $(document).mousemove(function (e) {
                mouseX = e.pageX;
                mouseY = e.pageY;
                //To Get the relative position
                if (this.offsetLeft != undefined)
                    mouseX = e.pageX - this.offsetLeft;
                if (this.offsetTop != undefined)
                    mouseY = e.pageY; -this.offsetTop;

                if (mouseX < 0)
                    mouseX = 0;
                if (mouseY < 0)
                    mouseY = 0;

                windowWidth = $(window).width() + $(window).scrollLeft();
                windowHeight = $(window).height() + $(window).scrollTop();
            });

        });
        function popPrompts() {

            $("#divPrmpts").show();

            var popupWidth = $("#divPrmpts").outerWidth();
            var popupHeight = $("#divPrmpts").outerHeight();

            if (mouseX + popupWidth > windowWidth)
                popupLeft = mouseX - popupWidth;
            else
                popupLeft = mouseX;

            if (mouseY + popupHeight > windowHeight)
                popupTop = mouseY - popupHeight;
            else
                popupTop = mouseY;

            if (popupLeft < $(window).scrollLeft()) {
                popupLeft = $(window).scrollLeft();
            }

            if (popupTop < $(window).scrollTop()) {
                popupTop = $(window).scrollTop();
            }

            if (popupLeft < 0 || popupLeft == undefined)
                popupLeft = 0;
            if (popupTop < 0 || popupTop == undefined)
                popupTop = 0;

            $("#divPrmpts").offset({ top: popupTop, left: popupLeft });

        }
        function popOverride() {
            if (IOAExist() == true) {
                $("#tdMsg").html("<div class='warning_box'> Override not available during IOA session.</div>");
                return false;
            }
            else{
                $("#divOverride").show();
                return true;
            }           
        }
        function IOAExist() {
            var dataresult = false;
            $.ajax(
                 {
                     type: "POST",
                     url: "Datasheet.aspx/SearchIOA",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,
                     success: function (data) {
                         if (data.d == "1") {
                             dataresult = true;
                         }
                         else {
                             dataresult = false;
                         }
                     }
                 });
            return dataresult;
        }

        function HidePopup() {
            $("#divPrmpts").hide();
        }
        function HidePoupOverride() {
            $("#divOverride").hide();
        }
        function HideHdrDiv() {
            <%--clsSession oSession = (clsSession)Session["UserSession"];--%>
            var studId = "<%=oSession.StudentId.ToString()%>";
            closeIframe1(studId);
        }
        function closeIframe(StudentId) {

            var divcontent = parent.document.getElementById('divContentPages');

            var sheetId;
            $(divcontent).find('#divTF' + StudentId).find('#LeftDiv' + StudentId).children().each(function (n, i) {

                if ($(this).css('display') == 'block') {

                    sheetId = $(this).attr('id');
                    var valNew = sheetId.split('-');
                    sheetId = valNew[2];
                    StudentId = valNew[1];
                }

            })


            var isSheet = $(divcontent).find('#divTF' + StudentId).find('#LeftDiv' + StudentId).children().size();

            var newName = "sheetDiv-" + StudentId + "-" + sheetId;


            var divUl = parent.document.getElementById('ulStudents');
            var isActive = $(divUl).find('#divStud' + StudentId).children().size();
            if (isActive == 2) {
                $(divUl).find('#divStud' + StudentId).remove();
            } else {
                $(divUl).find('#divStud' + StudentId).find('#StuddivTF' + StudentId + sheetId).remove();
            }
            $(divcontent).children().hide();
            $(divcontent).find('#divTFGraph').show();
            if (isSheet == 1) {
                $(divcontent).find('#divTF' + StudentId).remove();
            }
            else {
                $(divcontent).find('#divTF' + StudentId).find('#LeftDiv' + StudentId).find('#' + newName).remove();
            }









        }

        function closeIframe1(StudentId) {
            var divcontent = parent.document.getElementById('divContentPages');

            var sheetId;
            $(divcontent).find('#divTF' + StudentId).find('#LeftDiv' + StudentId).children().each(function (n, i) {

                if ($(this).css('display') == 'block') {

                    sheetId = $(this).attr('id');
                    var valNew = sheetId.split('-');
                    sheetId = valNew[2];
                    StudentId = valNew[1];
                }

            })


            var isSheet = $(divcontent).find('#divTF' + StudentId).find('#LeftDiv' + StudentId).children().size();

            var newName = "sheetDiv-" + StudentId + "-" + sheetId;


            var divUl = parent.document.getElementById('ulStudents');
            var isActive = $(divUl).find('#divStud' + StudentId).children().size();
            var pageType = "DS";
            if (isActive == 3) {
                $(divUl).find('#divStud' + StudentId).remove();
                parent.removeFrame1(StudentId, sheetId, pageType);
            } else {
                $(divUl).find('#divStud' + StudentId).find('#StuddivTF' + StudentId + sheetId).remove();
                $(divcontent).find('#divTF' + StudentId).find('#LeftDiv' + StudentId).find('#' + newName).remove();

                
            }
            $(divcontent).children().hide();
            //$(divcontent).find('#divTFGraph').show();

            parent.listLessonPlan();

            if (isSheet == 1) {            
                setTimeout(function () {
                    $(divcontent).find('#divTF' + StudentId).remove();
                }, 1000);
            }
            else {
                setTimeout(function () {
                    $(divcontent).find('#divTF' + StudentId).find('#LeftDiv' + StudentId).find('#' + newName).remove();
                }, 1000);
               
            }



        }
        $(document).mouseup(function (e) {
            var container = $("#divPrmpts");

            if (!container.is(e.target) // if the target of the click isn't the container...
                && container.has(e.target).length === 0) // ... nor a descendant of the container
            {
                container.hide();
            }
        });

        function closePOPDoc() {
            $('#divDoc').fadeOut('slow', function () {

            });

        }
        function viewDoc() {
            $('#divDoc').fadeIn('slow', function () {

            });
        }
        function closeViewPlan() {
            $('#divViewPlan').fadeOut('slow', function () {
            });
        }
        function viewPlan() {
            $('#divViewPlan').fadeIn('slow', function () {
            });
        }
        function CloseViewLP() {
            $('#divViewLesson').fadeOut('slow', function () {
            });
        }

        function ViewLp() {
            var isiPad = navigator.userAgent.match(/iPad/i);
            if (isiPad != null) {
                document.getElementById('divViewLesson').style.top = '5%';
				document.getElementById('divViewLesson').style.marginLeft = '-210px';
				document.getElementById('divViewLesson2').style.overflowX = 'hidden';
                document.getElementById('divViewLesson2').style.overflowY = 'scroll';
            }
            else {
                document.getElementById('divViewLesson').style.top = '7%';
                document.getElementById('divViewLesson').style.marginLeft = '-277px';
                document.getElementById('divViewLesson2').style.overflowY = 'hidden';
            }
            $('#divViewLesson').fadeIn('slow', function () {
                var SessHdr = document.getElementById('hdnSessionHdr').value;
                $.ajax(
                 {
                     type: "POST",
                     url: "Datasheet.aspx/ViewLessonPlanData",
                     data: "{'Id':'" + SessHdr + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,
                     success: function (data) {
                         $('#stViewLP').attr('src', data.d);
                         $('#divViewPlan').fadeOut('slow', function () {
                         });
                     },
                     error: function (request, status, error) {
                         alert("Error");
                     }
                 });
            });

        }

        function ViewSupportStrategy() {
            var isiPad = navigator.userAgent.match(/iPad/i);
            if (isiPad != null) {
                document.getElementById('divViewLesson').style.top = '5%';
                document.getElementById('divViewLesson').style.marginLeft = '-210px';
                document.getElementById('divViewLesson2').style.overflowX = 'hidden';
                document.getElementById('divViewLesson2').style.overflowY = 'scroll';
            }
            else {
                document.getElementById('divViewLesson').style.top = '7%';
                document.getElementById('divViewLesson').style.marginLeft = '-277px';
                document.getElementById('divViewLesson2').style.overflowY = 'hidden';
            }
            $('#divViewLesson').fadeIn('slow', function () {
                var SessHdr = document.getElementById('hdnSessionHdr').value;
                $.ajax(
                 {
                     type: "POST",
                     url: "Datasheet.aspx/ViewSupportStrategyData",
                     data: "{'Id':'" + SessHdr + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,
                     success: function (data) {
                         $('#stViewLP').attr('src', data.d);
                         $('#divViewPlan').fadeOut('slow', function () {
                         });
                     },
                     error: function (request, status, error) {
                         alert("Error");
                     }
                 });
            });

        }

        function fillMatchtosapmpleval() {
            var grid = document.getElementById('<%=grdDataSht.ClientID%>');
             if (grid != null) {
                 var rowcount = parseInt(grid.rows.length - 1);
                 var colcount = parseInt(grid.rows[0].cells.length);
                 var totSteps = rowcount;
                 //alert(rowcount+","+colcount)
                 for (var r = 1; r <= rowcount; r++) {
                     var txtnotes = $(grid.rows[r]).find('td:first').find('input[type=hidden]').eq(2).val();

                     for (var k = 1; k <= colcount; k++) {
                         var radio = $(grid.rows[r].cells[k]).find('input[type=radio]');
                         var samText = $(radio).parent().find('label').html();
                         //alert(txtnotes + "," + samText)
                         if (txtnotes == samText) {
                             //alert("equal"+txtnotes + "," + samText)
                             radio.attr('checked', 'checked');
                             radio.css('background-color', '#2b982b');
                         }

                     }



                     //var hdnid = $(elem).parent().parent().parent().find('td:first').find('input[type=hidden]').eq(2).attr('id');

                     //$('#' + hdnid).val(samText);
                 }
             }
         }



         function submitMe(data) {
             $('#btnSubmitAndRepeat3').attr({ "disabled": true, "value": "Submitting" }).removeClass("NFButtonNew").addClass("NFButtonNew1");
             $('#btnSubmitAndRepeat2').attr({ "disabled": true, "value": "Submitting" }).removeClass("NFButtonNew").addClass("NFButtonNew1");
             $('#div_overlay').show();
             $("body").css("cursor", "progress");
             return true;
         }

         function submitDisable(data) {
             $('#btnDiscardDatasheet').attr({ "disabled": true, "value": "Submitting" }).removeClass("NFButtonNew").addClass("NFButtonNew1");
             $('#div_overlay').show();
             $("body").css("cursor", "progress");
             return true;
         }


        function hideOverlay() {
            $('#div_overlay').hide();
            $("body").css("cursor", "default");
        }

		 function DiscardCheck(ioa_stat)
		 {
			 if (ioa_stat == "IOA")
			 {
				 document.getElementById('popupioa').style.display = 'block';
				 document.getElementById('btnDiscardYes').style.display = 'none';
				 document.getElementById('discardMain').style.display = 'none';
				 sessionStorage.setItem('DataCl', 'false');
			 }
			 else {
				 document.getElementById('popupioa').style.display = 'none';
				 document.getElementById('btnDiscardYes').style.display = 'block';
				 document.getElementById('discardMain').style.display = 'block';
			 }
			 
		 }
	   

         $("#btnSubmit1").hover(function () {
             $(this).css("background-color", "yellow");
         }, function () {
             $(this).css("background-color", "pink");
         });


         function scrollToTop() {
             window.scrollTo(0, 0);
             window.parent.parent.scrollTo(0, 100);
         }

        //Check whether the normal user and IOA user are same
        function CheckIOAUser(btn) {
            if (!multiClickSubmit) {
                if (checkUser() == "1") {
                    $("#tdMsg2").show();
                    return false;
                }
                else if (checkUser() == "2") {     // There is no normal session associated with this IOA
                    $("#tdMsg3").show();
                    return false;
                }
                else {
                    multiClickSubmit = true;
                    btn.style.opacity = '0.5';
                    return true;
                }

            }
            return false
         }
         function CheckFirstClick() {
             $("#btnIOASelect").hide();
             return true;
         }
         function checkUser() {
             var dataresult = "0";
             var SessHdr = document.getElementById('hdnSessionHdr').value;
             $.ajax(
                  {
                      type: "POST",
                      url: "Datasheet.aspx/chkIOAUserAndNrmlSession",
                      data: "{'sessHdr':'" + SessHdr + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,
                      success: function (data) {
                          dataresult = data.d;
                      }
                  });
             return dataresult;
         }
         function SelectSortOrder(PromptId)
         {
             var dataresult = false;
             var PromptId1 = PromptId;
             $.ajax(
                  {
                      type: "POST",
                      url: "Datasheet.aspx/SelectSortOrder",
                      data: "{'PromptId':'" + PromptId1 + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: false,
                      success: function (data) {
                          if (data.d >0) {
                              dataresult = data.d;
                          }
                          else {
                              dataresult = 0;
                          }
                      }
                  });
             return dataresult;
         }
    </script>
    <script>
        function countCharacters() {
            var textBox = document.getElementById('<%= txtNote.ClientID %>');
            var charCountSpan = document.getElementById('charCount');
            var remainingCount = 250 - textBox.value.length;

            charCountSpan.innerText = remainingCount >= 0 ? remainingCount : 0;

            if (remainingCount < 0) {
                textBox.value = textBox.value.substring(0, 250);
                charCountSpan.innerText = ' 0  characters remaining';
            } else {
                charCountSpan.innerText = remainingCount + '  characters remaining';
            }
        }
        function showCharCount() {
            var textBox = document.getElementById('<%= txtNote.ClientID %>');
             var charCountSpan = document.getElementById('charCount');

             if (textBox.value.length > 0) {
                 charCountSpan.style.display = 'inline';
                 charCountSpan.style.color = 'gray';
                 charCountSpan.style.fontSize = '10px';
             } else {
                 charCountSpan.style.display = 'none';
             }
             countCharacters();
         }
         function hideCharCount() {
             var charCountSpan = document.getElementById('charCount');
             charCountSpan.style.display = 'none';
         }
         function openPopup(message, operation) {
             // Set custom message
             document.getElementById('customMessage').innerText = message;
             // Store the operation in a hidden field or variable
             document.getElementById('hiddenOperation').value = operation;
             document.getElementById('decisionModal').style.display = 'block';
         }

         function closePopup(decision) {
             document.getElementById('decisionModal').style.display = 'none';
             if (decision === false) {
                 alert("In order to view the submitted data, please close and reopen the datasheet.");
                 sessionStorage.setItem('DataCl', 'false');
             }
         }
         function closedatasheet() {
            hideOverlay();
             sessionStorage.setItem('DataCl', 'false');
         }

         function setDecision(decision) {
             var operation = document.getElementById('hiddenOperation').value;
             console.log("Decision:", decision);
             console.log("Operation:", operation);
             $.ajax({
                 type: "POST",
                 url: "Datasheet.aspx/SetDecisionOperation",
                 data: JSON.stringify({ decision: decision, operation: operation }),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (response) {
                     closePopup(decision);
                     if (response.d) {
                         // Continue with the process
                         //__doPostBack('btnContinueProcess', '');
                         //$('#btnContinueProcess').trigger("click");
                         //document.getElementById('btnContinueProcess').click();
                         var btnContinue = document.getElementById('btnContinueProcess');
                         if (btnContinue) {
                             $('#btnContinueProcess').trigger('click');
                         } else {
                             //console.log("btnContinueProcess not found.");
                         }
                     } else {
                         // Exit the process
                     }
                 },
                 error: function (xhr, status, error) {
                     alert("An error occurred: " + xhr.responseText);
                 }
             });
         }
         //function popUpTriggerClick() {
         //    $('#btnTriggerProcess').trigger("click");
         //}
         //function popUpTriggerAnotherClick() {
         //    $('#btnAnotherOperation').trigger("click");
         //}
    </script>
</head>
<body>
    <script>
        // Event listeners to reset the idle timer on user activity without closing the popup or resetting the countdown timer
        window.onload = parent.resetIdleTimer;
        window.onmousemove =  parent.resetIdleTimer;
        window.onmousedown = parent.resetIdleTimer; // Catches touchscreen presses as well
        window.ontouchstart = parent.resetIdleTimer;
        window.onclick = parent.resetIdleTimer;     // Catches touchpad clicks as well
        window.onkeydown = parent.resetIdleTimer;
        window.addEventListener('scroll', parent.resetIdleTimer, true); // Catches scrolling
        </script>
    <form id="form1" runat="server">
        <div class="container mt-5" style="display:none">
           <%-- <asp:Button ID="btnTriggerProcess" runat="server" Text="Start Process" OnClick="btnTriggerProcess_Click" CssClass="btn btn-primary" />
            <asp:Button ID="btnAnotherOperation" runat="server" Text="Another Operation" OnClick="btnAnotherOperation_Click" CssClass="btn btn-primary" />--%>
            <asp:Button ID="btnContinueProcess" runat="server" Text="Continue Process" OnClick="btnContinueProcess_Click" style="display:none;" />
        </div>

        <!-- Modal -->
        <div id="decisionModal" class="modal">
            <div class="modal-content">
                <div class="modal-header">
                    <span class="close" onclick="closePopup(false)">&times;</span>
                    <span class="popup-headder" style="font-size:x-large;">Notice</span>
                </div>
                <div class="modal-body">
                    <p id="customMessage">A new data update has occurred since your last fetch. Please reload the datasheet to avoid overwriting the latest update with your data.</p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClientClick="setDecision(true); return false;" CssClass="btn btn-success" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="setDecision(false); return false;" CssClass="btn btn-danger" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hiddenOperation" runat="server" />
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
            <asp:Timer ID="Timer1" runat="server" Enabled="false" Interval="5000" OnTick="Timer1_Tick"></asp:Timer>
            <asp:HiddenField runat="server" ID="hdn_isMaintainance" />
            <asp:HiddenField runat="server" ID="hdn_currTempSetNmbr" />
            <asp:HiddenField runat="server" ID="hdn_currTempSet" />
            <asp:HiddenField runat="server" ID="hdnTemplateId" />
            <asp:HiddenField runat="server" ID="hdnSelectedSampleTest" />
            <asp:HiddenField runat="server" ID="hdnSessionHdr" />
            <asp:HiddenField runat="server" ID="hdnRadBtnSet" />
            <asp:HiddenField runat="server" ID="hdnRadBtnStep" />
            <asp:HiddenField runat="server" ID="hdnRadBtnPrompt" />
            <asp:HiddenField runat="server" ID="hdnChkdRsn" />
            <asp:HiddenField runat="server" ID="hdnMissTrialRsn" />
            <asp:HiddenField runat="server" ID="Checkclose" />
            <div class="containerMain">
                <div class="lftPartContainer">
                    <div class="mainHeader">
                        <div runat="server" style="min-width: 40%; float: left;" id="h_LPname">
                        </div>

                    </div>
                    <%--<div class="mainHeaderImages" style="display: inline;">--%>
                    <div class="mainHeaderImages" style="display: inline; height: 100px;">

                        <%--<asp:Button ID="btnInactive" runat="server" Style="margin-top: 0.2%" Visible="false" Text="InActivate" BackColor="#0D668E" Font-Bold="True" ForeColor="White" />--%>
                        <%--moved from line 1515--%>


                        <table cellpading="0" cellspacing="0" style="width: 100%; margin-bottom: .5%;" border="0">

<tr><div  id="banner" runat="server" class='redbanner' style="width:960px;height:30px;line-height:30px;text-align:center;background-color:red; "> This lesson has met criteria for modification, please notify curriculum staff.                               
    <asp:LinkButton ID="Linkbtnserch" runat="server" OnClick="Dismissbtn" style="float:right;color: black;padding-right:20px;" > Dismiss 🞮</asp:LinkButton>
    </div>

</tr>
    
    <tr>
        <div  id="LessonBanner" runat="server" class='redbanner' style="width:960px;height:30px;line-height:30px;text-align:center;background-color:red; ">                              
            <asp:label runat="server" text="" id="LblBanner"></asp:label>
            <asp:LinkButton ID="LBannerBtn" runat="server" OnClick="LessonBannerDismissBtn" style="float:right;color: black;padding-right:20px;" > Dismiss 🞮</asp:LinkButton>

        </div>
    </tr>

                           <tr id="UpperMenuButtonPanel">
                                <td colspan="5">


                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                                        <ContentTemplate>
                                            <%--<asp:Button ID="btnPriorSessn1" CssClass="NFButtonNew" runat="server" Text="Prior Sessions" OnClick="btnPriorSessn_Click" Style="height: 30px !important; font-size: 10px;" />--%>
                                            <asp:Button ID="btnSave1" runat="server" Text="Save Draft" CssClass="NFButtonNew" Style="font-size: 12px" OnClick="btnSave_ClickCheck" />
                                            <asp:Button ID="btnSaveConfirm" runat="server" Text="Save Draft" CssClass="NFButtonNew" Style="font-size: 12px;display: none;" OnClientClick="loadBeforeSave('Save');" OnClick="btnSave_Click"/>
                                            <asp:HiddenField ID="printPrev" runat="server" Value="0" />
                                            <%--<asp:Button ID="btnSubmit1" runat="server" Text="Submit Scores" CssClass="NFButtonNew" style="font-size:12px; display:none;" OnClientClick="return loadBeforeSave('Submit');" OnClick="btnSubmit_Click" />                                     
                                            <asp:Button ID="btnSubmit2" runat="server" Text="Submit Scores" CssClass="NFButtonNew" style="font-size:12px" OnClientClick="return submitMe(this);" OnClick="ConfirmSubmission" />
                                            <asp:Button ID="btnSubmitAndRepeat1" runat="server" Text="Submit & Repeat" CssClass="NFButtonNew" style="font-size:12px; display:none;" OnClick="btnSubmitAndRepeat_Click" />
                                            <asp:Button ID="btnSubmitAndRepeat3" runat="server" Text="Submit & Repeat" CssClass="NFButtonNew" style="font-size:12px" OnClientClick="return submitMe(this);" OnClick="ConfirmSubmission" />--%>



                                            <asp:Button ID="btnSubmit1" runat="server" Text="Submit Scores" CssClass="NFButtonNew" Style="font-size: 12px; display: none;" OnClientClick="return loadBeforeSave('Submit');" OnClick=" btnSubmit_Click" />
                                            <asp:Button ID="btnSubmit2" runat="server" Text="Submit Scores" CssClass="NFButtonNew" Style="font-size: 10px" OnClick="ConfirmSubmissionCheck" />
                                            <asp:Button ID="btnSubmitConfirm" runat="server" Text="Submit Scores" CssClass="NFButtonNew" Style="font-size: 10px;display: none;" OnClientClick="closecheck(); " OnClick="ConfirmSubmission" />
                                            <asp:Button ID="btnSubmitAndRepeat1" runat="server" Text="Submit & Repeat" CssClass="NFButtonNew" Style="font-size: 10px; display: none;" OnClientClick="return submitMe(this);" OnClick="btnSubmitAndRepeat_Click" />
<%--                                            <asp:Button ID="btnDiscard" runat="server" Text="Discard" CssClass="NFButtonNew" Style="font-size: 12px; display: none;" OnClientClick="return submitDisable(this);" OnClick="btnDiscard_ok_Click" />--%>
                                            <asp:Button ID="btnSubmitAndRepeat3" runat="server" Text="Submit & Repeat" CssClass="NFButtonNew" Style="font-size: 10px" OnClick="ConfirmSubmissionCheck" />
                                            <asp:Button ID="btnSubmitAndRepeatConfirm" runat="server" Text="Submit & Repeat" CssClass="NFButtonNew" Style="font-size: 10px;display: none;" OnClientClick="closecheck(); " OnClick="ConfirmSubmission"/>
                                           <asp:Button ID="Print1" runat="server" Text="Print Blank" CssClass="NFButtonNew" Style="font-size: 12px" onclick ="btnSave2_Click" /> <!--OnClientClick="printpage1();SaveData();"-->
                                            <asp:Button ID="print2" runat="server" Text="Print" CssClass="NFButtonNew" Style="font-size: 12px" Visible="false" OnClientClick="printpage1(); printpage(); " OnClick="btnNotSave_Click"   />
<%--                                        <asp:Button ID="btnDiscardDatasheet" Text="Discard Datasheet" runat="server" CssClass="NFButtonNew" Style="font-size: 12px" onclick="btnDiscardDatasheet_Click"/>--%>
											<asp:Button ID="btnDiscardDatasheet" Text="Discard Datasheet" runat="server" CssClass="NFButtonNew" Style="font-size: 12px" OnClick="btnDiscardDatasheet_Click"/>

                                            
                                            <%--<asp:Button ID="btnProbe1" runat="server" Text="Probe Mode" CssClass="NFButtonNew" OnClientClick="probe();" OnClick="btnProbe_Click" Style="height: 30px !important; font-size: 10px;" />--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td id="tdMsg" runat="server" style="text-align: center"></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                                <td colspan="3">
                                    <img id="imgtest" src="../Administration/images/Viewdetails.png" onclick="viewDet()" alt="View Details" style="margin-top: -20px" />
                                </td>
                            </tr>
                            <asp:Label ID="sessDate" runat="server" Text=" " style="visibility:hidden" />
                            <%--<tr>
                                <td colspan="7" style="border: 0;">
                                    <table style="width: 100%;" cellpading="0" cellspacing="0" border="0">

                                        <tr>

                                            <td style="border: 0;">
                                                
                                            </td>
                                            <td style="border: 0;"></td>
                                            <td style="text-align: right; border: 0;">
                                                </td>


                                        </tr>


                                    </table>



                                </td>
                            </tr>--%>

                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class="tdText" style="width: 7%;">
                                    <asp:Label ID="lblSubmitAndRepeatText" runat="server" Text="Repeat Count:" Visible="false"></asp:Label>
                                </td>
                                <td class="tdText" style="font-weight: bold; width: 3%;">
                                    <asp:Label ID="lblSubmitAndRepeatCount" runat="server" Text="" Visible="false"></asp:Label>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>

                                <td class="tdText" style="width: 4%;">Set: </td>
                                <td class="tdText" style="font-weight: bold; width: 200px; border-right: 1px groove">
                                    <asp:Label ID="lblSet" runat="server" Text="-"></asp:Label>
                                    <asp:Label ID="lblRsn" runat="server" style="margin-top:20px;font-size:smaller;color:black"></asp:Label>
                                </td>
                                <td class="tdText" style="width: 13%;">STEP/SAMPLE/Sd:</td>
                                <td class="tdText" style="font-weight: bold; width: 185px; border-right: 1px groove">
                                    <asp:Label ID="lblStep_Sample" runat="server" Text="-"></asp:Label>
                                </td>
                                <td class="tdText" style="width: 8%;">PROMPT:</td>
                                <td class="tdText" style="font-weight: bold; width: 160px; border-right: 1px groove">
                                    <asp:Label ID="lblPromptData" runat="server"></asp:Label>
                                    <asp:Label ID="lblPromt" runat="server" Text="-"></asp:Label>
                                    <div id="div_StepPrompts" runat="server" style="border: 1px solid; cursor: pointer; background-color: rgb(13, 102, 142); color: white; width: 75px; text-align: center;" onclick="popPrompts();">View Prompts</div>
                                </td>
                                <td class="tdText" style="width: 7%;">Session:</td>
                                <td class="tdText" style="font-weight: bold; width: 3%;">
                                    <asp:Label ID="lblSession" runat="server" Text="-"></asp:Label>
                                </td>
                                <td class="tdText" style="width: 7%; border-left: 1px groove" >
                                    <asp:Label ID="lblModifiedOn" runat="server" Visible="false" Text="Submitted/Modified Date:"></asp:Label>
                                </td>
                                <td class="tdText" style="font-weight: bold; width: 10%;">
                                    <asp:Label ID="lblModifiedDate" runat="server" Visible="false" Text=" "></asp:Label>
                                </td>
                                <td style="min-width: 75px;">
                                    <asp:CheckBox ID="chkSessMistrial" runat="server" Text="Mistrial" CssClass="MisClass" onclick="showReason(this)" style="position:absolute"></asp:CheckBox>
                                    <asp:Label ID="mistrialRsn" runat="server" style="position:absolute;margin-top:20px;font-size:smaller;color:black"></asp:Label>
                                </td>
                            </tr>

                        </table>
                    </div>
                    <div class="clear"></div>
                    <div class="topper">
                        <asp:HiddenField ID="hfcrntPrompt" runat="server" />
                        <asp:HiddenField ID="hfPlusMinusResp" runat="server" />
                        <asp:HiddenField ID="hfAutoSaveCount" runat="server" Value="0" />
                        <asp:HiddenField ID="hfcrntStep" runat="server" />
                        
                        <div style="width: 20px;" class="btContainer">
                        </div>

                        <div class="tabsContent" id="viewDettab" style="display: none; width: 99%!important; z-index: 9999">
                            <img onclick="viewDet();" style="position: absolute; width: 25px; height: 25px; z-index: 9999; left: 96.5%; margin-top: 1px;" src="../Administration/images/button_red_close.png">
                            <div class="tab" style="display: block;">


                                <b style="font-size: 14px;">Current Teaching Data</></b>
                                <b style="font-size: 14px; padding-left: 160px">Current Status</b>
                                <br />
                                <br />


                                <table style="width: 40%; margin-left: -5px;" border="2" cellpadding="0" cellspacing="0">


                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">Prep :</td>
                                        <td>
                                            <asp:Label ID="lblLessonPrep" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">Type Of Instruction :</td>
                                        <td>
                                            <asp:Label ID="lblTypOfIns" runat="server" Text=""></asp:Label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">IOA :</td>
                                        <td>
                                            <asp:Label ID="lblIOA" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="st" style="width: 20%; background-color: #03507d; color: white;">Prompt Procedure :</td>
                                        <td>
                                            <asp:Label ID="lblPromtProc" runat="server" Text="-"></asp:Label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="st" style="width: 40%; background-color: #03507d; color: white;">Sd/Instruction :</td>
                                        <td style="width: 50%;">
                                            <asp:Label ID="lblSd" runat="server" Text="-"></asp:Label>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">Materials  :</td>
                                        <td>
                                            <asp:Label ID="lblMaterials" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">Modification Status :</td>
                                        <td>
                                            <asp:Label ID="lblModificationStat" runat="server" Text="-"></asp:Label>
                                        </td>

                                    </tr>

                                    <%--<tr>
                                        <td class="st" style="background-color:#03507d;color:white;" >Response Definitions :</td>
                                        <td style="width: 30%;">
                                            <%--<asp:Label ID="lblResDef" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblResIncorrect" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>--%>
                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">Correct Response :</td>
                                        <td>
                                            <asp:Label ID="lblCorrectRespData" runat="server" Text=""></asp:Label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">Incorrect Response :</td>
                                        <td>
                                            <asp:Label ID="lblInCorrectRespData" runat="server" Text=""></asp:Label>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">Mistrial :</td>
                                        <td>
                                            <asp:Label ID="lblMistrial" runat="server" Text=""></asp:Label>
                                        </td>

                                    </tr>


                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">Measurement System:</td>
                                        <td>
                                            <asp:Label ID="lblResDef" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblResIncorrect" runat="server" Text=""></asp:Label>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">Correction Procedure:</td>
                                        <td>
                                            <asp:Label ID="lblcorrectionproc" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="st" style="background-color: #03507d; color: white;">Reinforcement Procedure:</td>
                                        <td>
                                            <asp:Label ID="lblreinforcementproc" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>

                                    <tr style="width: 27%;">

                                        <td></td>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td></td>


                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>


                                    </tr>
                                </table>


                                <table style="width: 59%; margin-left: 5px;" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="3">
                                            <div style="max-height: 500px; overflow-y: auto; overflow-x: hidden;">
                                                <asp:GridView ID="grdSetDetails" runat="server">

                                                    <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                                                    <RowStyle CssClass="RowStyle" />
                                                    <AlternatingRowStyle CssClass="AltRowStyle" />
                                                </asp:GridView>

                                            </div>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <input id="viewDocbtn" type="button" class="NFButtonWithNoImage" onclick="viewDoc(); scrollToTop();" value="View Attachments" style="float: right;" />
                                        </td>
                                        <td style="width: 10%">
                                            <input id="btnViewPlan" type="button" class="NFButtonWithNoImage" onclick="viewPlan(); scrollToTop();" value="View Plan" style="float: right;" />
                                        </td>
                                    </tr>

                                </table>

                            </div>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div style="width:99%">
                        <p>
                           <asp:LinkButton ID="leftNav"  runat="server" OnClick="right_View" visible="false" CssClass="topTabtd"  style="float:left"><b>&nbsp Previous Datasheet &nbsp</b> </asp:LinkButton>
                           <asp:LinkButton ID="rightNav"  runat="server" OnClick="left_View"  visible="false" CssClass="topTabtd" style="float:right"><b>&nbsp Next Datasheet &nbsp</b> </asp:LinkButton>
                         </p>
                    </div>

                    <div class="tablePart" style="overflow-x: scroll">
                        
                        <%--  <asp:GridView ID="grdSteps" runat="server" AutoGenerateColumns="False" GridLines="None" OnRowDataBound="grdSteps_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Step / Sample / Sd" ItemStyle-CssClass="clr" HeaderStyle-Width="17%">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfSessStepID" runat="server" Value='<%# Eval("SessStepID") %>' />
                                        <%# Eval("StepName") %>
                                    </ItemTemplate>


                                    <ItemStyle CssClass="clr"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="clr" HeaderText="Prompt" HeaderStyle-Width="14%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPrompt" Enabled="false" runat="server" onchange="temp();"></asp:DropDownList>
                                    </ItemTemplate>

                                    <HeaderStyle Width="14%"></HeaderStyle>

                                    <ItemStyle CssClass="clr"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="clr" HeaderText="+/-" HeaderStyle-Width="8%">
                                    <ItemTemplate>
                                        <%--<img src="images/plusminusbtn.PNG" width="97" height="44" />--%>                        <%--<asp:RadioButtonList ID="rdbResponse" runat="server" Width="100px" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="+" Value="+"></asp:ListItem>
                                            <asp:ListItem Text="-" Value="-"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <input type='radio' runat="server" disabled="true" id="rdbRespPlus" style="width: 15px; border: 0;" name='choices' value='+' onchange="temp();" /><label>+</label>
                                        <input type='radio' runat="server" disabled="true" id="rdbRespMinus" style="width: 15px; border: 0" name='choices' value='-' onchange="temp();" /><label>-</label>
                                    </ItemTemplate>

                                    <HeaderStyle Width="8%"></HeaderStyle>

                                    <ItemStyle CssClass="clr"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="clr" HeaderText="Text" HeaderStyle-Width="12%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtText" Enabled="false" onchange="setTimer();" runat="server"></asp:TextBox>
                                    </ItemTemplate>


                                    <ItemStyle CssClass="clr"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="clr" HeaderText="Duration" HeaderStyle-Width="17%">
                                    <ItemTemplate>
                                        <input type="text" title="Click to Reset" readonly="true" class="clsDuratn" style="text-align: center; cursor: pointer; width: 47%;" value="00:00:00" id="time" onclick="resetTime(this);" />
                                        <input type="button" runat="server" disabled="true" value="Start" style="width: 33%; border-radius: 5px 5px 5px 5px; cursor: pointer; height: 48px;" onclick="stopwatch(this);" />
                                        <asp:HiddenField ID="hfDuration" Value="00:00:00" runat="server" />
                                        <%--<input type="button" value="Reset" style="width:23%;">
                                    </ItemTemplate>


                                    <ItemStyle CssClass="clr"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="clr" HeaderText="Frequency" HeaderStyle-Width="12%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFrequency" CssClass="clsFreq" onkeypress="return isNumber(event)" onchange="temp();" onpaste="return false" Enabled="false" runat="server"></asp:TextBox>
                                    </ItemTemplate>


                                    <ItemStyle CssClass="clr"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="clr" HeaderText="MisTrial" HeaderStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkMistrial" Enabled="false" Style="border: 0;" onchange="temp();" runat="server" />
                                    </ItemTemplate>

                                    <HeaderStyle Width="6%"></HeaderStyle>

                                    <ItemStyle CssClass="clr"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="nobdr" HeaderStyle-CssClass="nobdr" HeaderText="Notes" HeaderStyle-Width="12%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtStepNotes" TextMode="MultiLine" runat="server" onchange="setTimer();" Width="90%" Style=""></asp:TextBox>
                                        <div class="slideThree">
                                            <input type="checkbox" runat="server" value="None" checked="false" onclick="checkClick(this);" id="slideThree" name="check" />
                                            <label for="slideThree"></label>
                                        </div>
                                    </ItemTemplate>

                                    <HeaderStyle CssClass="nobdr" Width="12%"></HeaderStyle>

                                    <ItemStyle CssClass="nobdr"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>--%>                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>--%>

                      
                        <asp:GridView ID="grdDataSht" runat="server" GridLines="None" AutoGenerateColumns="false" OnRowDataBound="grdDataSht_RowDataBound" OnRowDeleting="grdDataSht_RowDeleting">
                            <Columns>
                            </Columns>
                            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                            <RowStyle CssClass="RowStyle" />
                            <AlternatingRowStyle CssClass="AltRowStyle" />
                        </asp:GridView>

                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate></ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <%--<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <th scope="col" width="20%">Step / Sample / Sd</th>
    <th scope="col" width="20%">Prompt Level</th>
    <th scope="col" width="20%">Responded Correctly</th>
    <th scope="col" width="20%">Total Words Correct</th>
    <th class="nobdrr" scope="col" width="20%">Notes</th>
  </tr>
  <tr>
    <td class="clr">Step Text</td>
    <td><select name="">&nbsp;</select></td>
    <td class="clr"><img src="images/plusminusbtn.PNG" width="97" height="44" /></td>
    <td><select name="">&nbsp;</select></td>
    <td class="nobdr"><input name="" type="text" /></td>
  </tr>
  <tr>
    <td class="clr">Step Text</td>
    <td><select name="">&nbsp;</select></td>
    <td class="clr"><img src="images/plusminusbtn.PNG" width="97" height="44" /></td>
    <td><select name="">&nbsp;</select></td>
    <td class="nobdr"><input name="" type="text" /></td>
  </tr>
  <tr>
    <td class="clr">Step Text</td>
    <td><select name="">&nbsp;</select></td>
    <td class="clr"><img src="images/plusminusbtnR.PNG" width="97" height="44" /></td>
    <td><select name="">&nbsp;</select></td>
    <td class="nobdr"><input name="" type="text" /></td>
  </tr>
  <tr>
    <td class="clr">Step Text</td>
    <td><select name="">&nbsp;</select></td>
    <td class="clr"><img src="images/plusminusbtn.PNG" width="97" height="44" /></td>
    <td><select name="">&nbsp;</select></td>
    <td class="nobdr"><input name="" type="text" /></td>
  </tr>
  <tr>
    <td class="clr">Step Text</td>
    <td><select name="">&nbsp;</select></td>
    <td class="clr"><img src="images/plusminusbtnR.PNG" width="97" height="44" /></td>
    <td><select name="">&nbsp;</select></td>
    <td class="nobdr"><input name="" type="text" /></td>
  </tr>
  <tr>
    <td class="clr">Step Text</td>
    <td><select name="">&nbsp;</select></td>
    <td class="clr"><img src="images/plusminusbtn.PNG" width="97" height="44" /></td>
    <td><select name="">&nbsp;</select></td>
    <td class="nobdr"><input name="" type="text" /></td>
  </tr>
</table>--%>
                        <div class="clear"></div>
                    </div>
                    <div class="clear"></div>
                    <div class="bottom">
                        <div class="lfrContainer">
                            <table id="tbl_Measure" runat="server" width="100%" border="0"></table>
                            <table width="100%" style="display: none;" border="0">
                                <tr>
                                    <th scope="col" width="30%">Measurement Label</th>
                                    <th scope="col" width="20%">Formula</th>
                                    <th scope="col" width="10%">+/-</th>
                                    <th scope="col" width="10%">Prompt</th>
                                    <th scope="col" width="13%">Frequency</th>
                                    <th scope="col" width="12%">Duration</th>
                                    <th scope="col" width="12%">Total Correct</th>
                                    <th scope="col" width="12%">Total Correct</th>

                                </tr>

                                <tr>
                                    <td>Frequency (Words Correct)</td>
                                    <td>Total</td>
                                    <td></td>
                                    <td></td>
                                    <td align="center">
                                        <asp:Label ID="lbl_Freq" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hf_Freq" runat="server" />
                                        <asp:HiddenField ID="hfTextScore" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Percent (Successful)</td>
                                    <td>%Accuracy</td>
                                    <td>
                                        <asp:Label ID="lblRslt1_Acc" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfRslt1_Acc" runat="server" />

                                        <asp:HiddenField ID="hfResultStep_Acc" runat="server" />
                                        <asp:HiddenField ID="hfResultStep_Prmpt" runat="server" />
                                        <asp:HiddenField ID="hfRslt1_ExcludeCrntStep_Acc" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRslt2_Acc" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfRslt2_Acc" runat="server" />

                                        <asp:HiddenField ID="hfRslt2_ExcludeCrntStep_Acc" runat="server" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Percent (Independent)</td>
                                    <td>%Independence</td>
                                    <td>
                                        <asp:Label ID="lblRslt1_Ind" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfRslt1_Ind" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRslt1_IndAll" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfRslt1_IndAll" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRslt2_Ind" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfRslt2_Ind" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRslt2_IndAll" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfRslt2_IndAll" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Percent (Needed)</td>
                                    <td>%Prompted</td>
                                    <td>
                                        <asp:Label ID="lblRslt1_Prmt" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfRslt1_Prmt" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRslt2_Prmt" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfRslt2_Prmt" runat="server" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Duration (hh:mm:ss)</td>
                                    <td>Total Duration</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td align="left">
                                        <asp:Label ID="lblTotDur" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfTotDur" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Duration (hh:mm:ss)</td>
                                    <td>Avg Duration</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td align="left">
                                        <asp:Label ID="lblAvgDur" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfAvgDur" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total Correct</td>
                                    <td>Total Correct</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td align="left">
                                        <asp:Label ID="lblTotCorct" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfTotCorct" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total Correct</td>
                                    <td>Total Correct</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td align="left">
                                        <asp:Label ID="lblTotInCorct" Text="" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfInTotCorct" runat="server" />
                                    </td>
                                </tr>

                            </table>
                        </div>
                        <div class="rfrContainer">
                            Note: (250 characters max)
                            <br />
                            <span id="charCount"></span>
                            <br />
                            <asp:TextBox ID="txtNote" runat="server" Width="92%" Height="115px" TextMode="MultiLine" oninput="showCharCount()" onkeyup="countCharacters()" onfocus="showCharCount()" onblur="hideCharCount()"  MaxLength="250"></asp:TextBox>
                        </div>

                    </div>
                    <div class="clear"></div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <asp:Button ID="ImgBtn_Inactive" CssClass="NFButtonNew" Text="Inactivate" runat="server" OnClientClick="javascript: return confirm('Are you sure you want to Inactivate?')" OnClick="ImgBtn_Inactive_Click" Style="background-color: red; color: white; font-size: 12px" />
                            <asp:Button ID="btnPriorSessn" CssClass="NFButtonNew" Style="font-size: 12px" runat="server" Text="View Prior Sessions" OnClick="btnPriorSessn_Click" OnClientClick="scrollToTop()" />
                            <asp:Button ID="btnSave" runat="server" Text="Save Draft" Style="font-size: 12px" CssClass="NFButtonNew" OnClick="btnSave_ClickCheck" OnClientClick="scrollToTop();" />
                            <asp:Button ID="btnSaveBottomConfirm" runat="server" Text="Save Draft" Style="font-size: 12px;display: none;" CssClass="NFButtonNew" OnClientClick="loadBeforeSave('Save'); scrollToTop();" OnClick="btnSave_Click" />
                            <asp:Button ID="btnProbe" runat="server" Text="Probe Mode" Style="font-size: 12px" CssClass="NFButtonNew" OnClientClick="probe();" OnClick="btnProbe_Click" />
                            <asp:Button ID="ImgBtn_Override" runat="server" Style="font-size: 12px" BackColor="#03507d" CssClass="NFButtonNew" Text="Override" OnClientClick="scrollToTop(); return popOverride(); " />
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit Scores" Style="font-size: 12px" CssClass="NFButtonNew" OnClick="ConfirmSubmissionCheck" OnClientClick="scrollToTop();" />
                            <asp:Button ID="btnSubmitBottomConfirm" runat="server" Text="Submit Scores" Style="font-size: 12px;display: none;" CssClass="NFButtonNew" OnClick="ConfirmSubmission" OnClientClick="scrollToTop();" />
                            <asp:Button ID="btnSubmitAndRepeat2" runat="server" Style="font-size: 12px" Text="Submit & Repeat" CssClass="NFButtonNew" OnClick="ConfirmSubmissionCheck" OnClientClick="scrollToTop();" />
                            <asp:Button ID="btnSubmitAndRepeatBottomConfirm" runat="server" Style="font-size: 12px;display: none;" Text="Submit & Repeat" CssClass="NFButtonNew" OnClick="ConfirmSubmission" OnClientClick="scrollToTop();"/>
                            <asp:Button ID="btnAddTrial" runat="server" Text="Add Trials" CssClass="NFButtonNew" OnClick="btnAddTrial_Click" Style="float: right; font-size: 12px" OnClientClick="loadBeforeSave('Save'); scrollToTop();" />
                            <asp:Label ID="LabelvisualToolEdit" runat="server" Text="Label" Visible="false" Style="color: red; font-size: 17px; padding: 5px;"></asp:Label>



                            <%--<asp:Button ID="btnMistrial" runat="server" Text="Mistrial" CssClass="NFButtonNew" OnClick="btnMistrial_Click" ></asp:Button>--%>
                            <%--<input type="button" class="NFButtonNew" value="check" width="50px" onclick="closeIframe();" />--%>
                            <asp:HiddenField ID="hfProbe" runat="server" />
                            <%--<input type="button" class="NFButtonNew" value="ProbeMode" width="50px" onclick="probe();" />--%>
                            <%--<table style="width: 40%;">
                                <tr>
                                    <td id="tdMsg" runat="server" style="text-align: center"></td>
                                </tr>
                            </table>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
    
                    <div class="clear"></div>
                </div>

                <%--<div class="rightPartContainer">
                    <iframe id="IfrmTimer" scrolling="no" frameborder="0" style="min-height: 615px; width: 100%;" runat="server"></iframe>
                </div>--%>





                <div class="clear"></div>
            </div>
            <div id="HdrContainer" style="">
                <asp:HiddenField runat="server" ID="hdn_displayTempOverride" Value="false" />
                <div id="div_tempOverride" class="div_tempOverride">
                    <div class="tempOverClose" onclick="$('#div_tempOverride').hide();">✕</div>
                    <asp:Repeater ID="rptr_tempOverride" runat="server" Visible="true" OnItemDataBound="rptr_tempOverride_ItemDataBound">

                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblGroupName" Visible="false"  Text='<%# Eval("GroupName") %>' style="float: left;margin-left:5px; font-weight: bold;"/>
                            <asp:Panel ID="pnl_set" runat="server" CssClass="to_stepDiv">
                               <%-- <div style="width: 25px; float: left; text-align: left;"><%# Container.ItemIndex + 1 %></div> --%>
                                <div style="width: 25px; float: left; text-align: left;"> <%#  Eval("SortOrder") %>. </div> 
                                <div style="width: 200px; float: left; text-align: left;">
                                    <b>
                                        <%# Eval("SetCd")%>

                                    </b>
                                    <asp:HiddenField runat="server" ID="hdn_currSetId" Value='<%# Eval("currSet") %>' />
                                    <asp:HiddenField runat="server" ID="hdn_tempSetId" Value='<%# Eval("DSTempSetId") %>' />
                                    <asp:HiddenField runat="server" ID="hdn_currSetNmbr" Value='<%# Eval("currSetNmbr") %>' />
                                    <asp:HiddenField runat="server" ID="hdn_tempSetNmbr" Value='<%# Eval("SortOrder") %>' />
                                </div>
                                <div style="float: right">
                                    <asp:CheckBox ID="tempOverrideCheckBox" runat="server" onchange="tempOverrideCheckBox_CheckedChanged();" />
                                    <%--ashin--%>
                                </div>
                                <div style="float: right">
                                    <asp:Button CssClass="btn_list" ToolTip="Click to excecute this set" runat="server" Text="►" ID="btn_go" Style="height: 25px !important;" OnClick="btn_go_Click" Visible="false" />
                                    <asp:Label CssClass="lblCurrent" ToolTip="Current Set" runat="server" Text="✔" ID="btn_list_crr" Style="color: green; font-size: 18px; font-weight: bold; font-family: Ariel;" Visible="false" />
                                    <asp:Label CssClass="lblDraft" ToolTip="Draft Available" runat="server" Text="Draft" ID="lblDraft" Style="float: left; color: blue; font-size: 10px; margin-right: 2px;" Visible="false" />
                                </div>
                                <%--★☀✔--%>
                            </asp:Panel>
                        </ItemTemplate>

                    </asp:Repeater>
                    <div class="specialOptions">
                        <asp:Label CssClass="to_stepDiv" Style="color: red; text-align: center; font-size: 15px;" ID="lblDefMsg" runat="server" Text="No previous set(s) available." Visible="false"></asp:Label>
						<asp:Button CssClass="to_stepDiv btn_cont" Style="width: 98%; display: none;" runat="server" ID="btn_continue" OnClick="btn_new_continue_Click" Text="Continue" OnClientClick="DisableButton();" />
                        <asp:Button CssClass="to_stepDiv btn_cont" Style="width: 98%;" runat="server" ID="btn_contIOASess" OnClick="btn_contIOASess_Click" Text="Continue IOA Session" Visible="false" />


                        <asp:HiddenField runat="server" ID="hdn_StdtSessionHdrId" />
                        <asp:HiddenField runat="server" ID="hdn_IOASessionHdrId" />
                    </div>
                </div>

                <asp:HiddenField runat="server" ID="hdn_displayReason" Value="false" />
                <div id="div_reason" class="div_reason">
                    <div class="reasonClose" onclick="reasonClose_Click();">✕</div>
                    <asp:Panel ID="Panel1" runat="server" CssClass="to_stepDiv">
                                <div style="width: 25px; float: left; text-align: left;"> 1. </div> 
                                <div style="width: 200px; float: left; text-align: left;">
                                    <b>Student Behavior</b>
                                </div>
                                <div style="float: right">
                                    <asp:CheckBox ID="cb_StudentBehavior" runat="server" onchange="reasonCheckBox_CheckedChanged('cb_StudentBehavior',this,'Student Behavior');" />
                                </div>
                            </asp:Panel>
                    <asp:Panel ID="Panel2" runat="server" CssClass="to_stepDiv">
                                <div style="width: 25px; float: left; text-align: left;"> 2. </div> 
                                <div style="width: 200px; float: left; text-align: left;">
                                    <b>Training/Practice</b>
                                </div>
                                <div style="float: right">
                                    <asp:CheckBox ID="cb_Training" runat="server" onchange="reasonCheckBox_CheckedChanged('cb_Training',this,'Training/Practice');" />
                                </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel3" runat="server" CssClass="to_stepDiv">
                                <div style="width: 25px; float: left; text-align: left;"> 3. </div> 
                                <div style="width: 200px; float: left; text-align: left;">
                                    <b>Probe</b>
                                </div>
                                <div style="float: right">
                                    <asp:CheckBox ID="cb_Probe" runat="server" onchange="reasonCheckBox_CheckedChanged('cb_Probe',this,'Probe');" />
                                </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel4" runat="server" CssClass="to_stepDiv">
                                <div style="width: 25px; float: left; text-align: left;"> 4. </div> 
                                <div style="width: 200px; float: left; text-align: left;">
                                    <b>Lesson Incomplete</b>
                                </div>
                                <div style="float: right">
                                    <asp:CheckBox ID="cb_LessonIncomplete" runat="server" onchange="reasonCheckBox_CheckedChanged('cb_LessonIncomplete',this,'Lesson Incomplete');" />
                                </div>
                    </asp:Panel>
                    <div>
                        <div class="specialOptions">
                        <asp:Button CssClass="to_stepDiv btn_cont" Style="width: 98%; display: none;" runat="server" ID="continue_btn" OnClick="continue_btn_Click" Text="Continue" />

                        </div>
                    </div>
                </div>

                <%--ashin--%>
                <%--                <div id="dashMain" style="display:none; position:fixed; top:135px; left:150px; background-color:white; border: 2px solid black;">
                    <div id="dashSub" style="width:500px; height:120px; margin:10px;">
                        <table>
                            <tr>
                                <td>
                                <b style="font-size: 15px; color: #17A14C">Move criteria is based on column(s) <label runat="server" id="lblNACols"></label>, which you left blank. This can cause unintended moves. Are you sure you want to submit?</b>
                                    <br /><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="button" id="btnTSubmit" class="NFButtonWithNoImage" style="width:100px; position:relative; left:125px;" value="Yes" onclick="triggerSubmitClick();" />
                                    <input type="button" id="btnTSAndR" class="NFButtonWithNoImage" style="width:100px; position:relative; left:125px;" value="Yes" onclick="triggerSubmitAndRepeatClick();" />
                                    <input type="button" class="NFButtonWithNoImage" style="width:100px; position:relative; left:175px;" value="No" onclick="hideConfirmPopUp();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>--%>

                <div id="dashMain" runat="server" class="dashMain">
                    <div id="dashSub" class="dashSub">
                        <table>
                            <tr>
                                <td>
                                    <b style="font-size: 15px; color: #17A14C">Move criteria is based on column(s)
                                        <label runat="server" id="lblNACols"></label>
                                        , which you left blank. This can cause unintended moves. Are you sure you want to submit?</b>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="button" id="btnTSubmit" class="NFButtonWithNoImage" style="width: 100px; position: relative; left: 125px;" value="Yes" onclick="triggerSubmitClick();" />
                                    <input type="button" id="btnTSAndR" class="NFButtonWithNoImage" style="width: 100px; position: relative; left: 125px;" value="Yes" onclick="triggerSubmitAndRepeatClick();" />
                                    <input type="button" class="NFButtonWithNoImage" style="width: 100px; position: relative; left: 175px;" value="No" onclick="hideConfirmPopUp();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="discardMain" class="dashMain" style="display:none">
                    <div id="discardSub" class="dashSub">
                        <table>
                            <tr>
                                <td>
                                    <b style="font-size: 15px; color: #17A14C">This datasheet and any data taken will be deleted, continue?</b>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--<input type="button" id="btnDisContinue" class="NFButtonWithNoImage" style="width: 100px; position: relative; left: 125px;" value="Yes" onclick="triggerSubmitClick();" />--%>
									<%--<asp:Button id="btnDiscardYes" runat="server" CssClass="NFButtonWithNoImage" style="width: 100px; position: relative;" Text="Yes" OnClientClick="triggerDiscardClick();" OnClick="btnPriorSessn_Click" />--%>
									<asp:Button ID="btnDiscardYes" runat="server" Text="Yes" style="width: 100px; position: relative;" CssClass="NFButtonWithNoImage" OnClientClick="return multiClickCheckDiscard(this);"  OnClick="btnDiscard_ok_Click "/>
                                    </td>
                                <td>
                                    <input type="button" class="NFButtonWithNoImage" style="width: 100px; position: relative;  " value="No" onclick="hideDiscardPopUp();"  />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="printComplete" runat="server" class="PrintDiv">
                    <div id="Div2" class="dashSub">
                        <table>
                            <tr>
                                <td>
                                  <b style="font-size: 13px"> Printing Completed press OK to exit....  </b><br /><br /><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button id="Button2" runat="server" CssClass="NFButtonWithNoImage" style="width: 100px; position: relative; left: 180px;" Text="OK"  onclick="printCompleteFunction" OnClientclick="hideConfirmPopUp2();"/>
                                    <%--<input type="button" class="NFButtonWithNoImage" style="width: 100px; position: relative; left: 175px;" value="Continue" onclick="hideConfirmPopUp2();" />--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="SaveBeforePrint" runat="server" class="PrintDiv" >
                    <div id="Div3" class="dashSub">
                        <table>
                            <tr>
                                <td>
                                  <b style="font-size: 13px"> Keep datasheet open after printing blank?  </b><br /><br /><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                  <%--  <asp:Button id="Button3" runat="server" CssClass="NFButtonWithNoImage" style="width: 100px; position: relative; left: 125px;" Text="Yes"   /> <!--OnClientClick="printpage1();printpage();" onclick="btnSave2_Click"-->
                                     <asp:Button id="Button4" runat="server" CssClass="NFButtonWithNoImage" style="width: 100px; position: relative; left: 125px;" Text="No"  onclick="btnNotSave_Click" />--%>
                                    <input type="button" id="Button5" runat="server" class="NFButtonWithNoImage" style="width: 100px; position: relative; left: 125px;" value="Yes"  onclick="printpage1(); printpage();" />
                                    <input type="button" class="NFButtonWithNoImage" style="width: 100px; position: relative; left: 125px;" value="No" onclick="printpage1NO(); printpageNO();" />                                    
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <%--ashin--%>

                <div id="HdrDiv" class="web_dialog" style="top: 7%;">
                    <img onclick="HideHdrDiv();" style="position: absolute; width: 25px; height: 25px; z-index: 9999; left: 96%; top: 0.5%; margin-top: 1px;" src="../Administration/images/button_red_close.png">
                    <div class="prevSetBtn" style="padding: 3px; visibility: hidden; vertical-align: middle; position: absolute; right: 0px; text-align: center; font-size: 11px; font-weight: bold; text-decoration: underline; cursor: pointer; color: red; font-family: Arial;" onclick="toggleSetList();">Execute Previous Sets</div>

                    <div id="Hdr_Stat">

                        <table id="tblNewIOA" style="display: inline-table; width: 100%; border: 0px; height: auto" class="popsecondver">


                            <tr>
                                <td align="left" colspan="2">
                                    <table style="width: 95%;">

                                        <tr>
                                            <td class="auto-style2" style="text-align: center;" colspan="3">
                                                <b style="font-size: 15px; color: #17A14C">Previous session was not submitted. </b>
                                                <br />
                                                <hr />
                                            </td>
                                            <%# Eval("SetCd")%>
                                        </tr>
                                        <tr>
                                            <td class="tdText" style="text-align: center; width: 49%;">&nbsp;</td>
                                            <td style="text-align: center; width: 2%;">&nbsp;</td>
                                            <td style="text-align: left; width: 49%;">&nbsp;</td>
                                        </tr>
                                        <style>
                                            .contWrapper div {
                                                float: left;
                                            }

                                            .dllInd {
                                                border-left: 1px solid;
                                            }

                                            .div_tempOverride {
                                                display: none;
                                                position: absolute;
                                                margin-top: 41px;
                                            }

                                            .div_reason {
                                                display: none;
                                                position: absolute;
                                                margin-top: 41px;
                                            }
                                            .tempOverClose {
                                                width: 16px;
                                                float: right;
                                                margin-top: -16px;
                                                padding: 1px;
                                                border: 1px solid #536376;
                                                border-bottom: 1px solid white;
                                                color: #536376;
                                                text-align: center;
                                                background-color: white;
                                                cursor: pointer;
                                                font-size: 8px;
                                            }

                                            .reasonClose {
                                                width: 16px;
                                                float: right;
                                                /*margin-top: -16px;*/
                                                padding: 1px;
                                                border: 1px solid #536376;
                                                border-bottom: 1px solid white;
                                                color: #536376;
                                                text-align: center;
                                                background-color: white;
                                                cursor: pointer;
                                                font-size: 8px;
                                            }

                                                .tempOverClose:hover {
                                                    color: red;
                                                }
                                                .reasonClose:hover {
                                                    color: red;
                                                }
                                        </style>
                                        <script>
                                            function toggleSetList() {
                                                $('.div_tempOverride').toggle();

                                            }
                                        </script>
                                        <tr>
                                            <td style="text-align: center; width: 51%;">
                                                <div class="contWrapper">
                                                    <div class="btnPack">
                                                        <asp:Button ID="btnNoIOA" CssClass="NFButtonWithNoImage" Width="150px" runat="server" Text="Continue" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" OnClick="btnNoIOA_Click" />
                                                    </div>

                                                </div>
                                                <asp:HiddenField ID="HiddenField2" runat="server" />

                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">

												<input type="button" style="width: 100px;" class="NFButtonWithNoImage" value="Start IOA" onserverclick="LoadDropdown" runat="server"/>



                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">


                                                <b>Normal Session</b> :
                                            
                                            </td>
                                            <td style="text-align: center; width: 2%;">&nbsp;</td>
                                            <td style="text-align: left; width: 49%;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 51%;">


                                                <label class="tdText">User:</label>
                                                <label class="txtnormal" runat="server" id="lblUName1">-</label></td>
                                            <td style="text-align: center; width: 2%;">&nbsp;</td>
                                            <td style="text-align: left; width: 49%;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 51%;">


                                                <label style="padding-right: 46px;" class="tdText">Start Time:</label>
                                                <label runat="server" class="txtnormal" id="LblStrtTime1"></label>
                                            </td>
                                            <td style="text-align: center; width: 2%;">&nbsp;</td>
                                            <td style="text-align: left; width: 49%;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 51%;">


                                                <label class="tdText" style="padding-right: 51px;">
                                                    End Time:</label>
                                                <label id="lblEndTime1" runat="server" class="txtnormal">
                                                </label>
                                            </td>
                                            <td style="text-align: center; width: 2%;">&nbsp;</td>
                                            <td style="text-align: left; width: 49%;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 51%;">
                                                <label style="padding-right: 12px;" class="tdText">Session Number:</label>
                                                <label runat="server" id="lblSessNo1" class="txtnormal"></label>
                                            </td>
                                            <td style="text-align: center; width: 2%;">&nbsp;</td>
                                            <td style="text-align: left; width: 49%;">&nbsp;</td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                            <%--ashin--%>
                        </table>
                        <table id="tblIOAUser" style="width: 100%; display: none;">

                            <tr>
                                <td style="" colspan="6" align="center" colspan="3"><b style="font-size: 15px; color: #17A14C">Initiating IOA</b><br />
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <table style="width: 95%;">

                                        <tr>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="text-align: center"><b style="font-size: 15px; color: #17A14C">Select your name then press "GO"</b></td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: left;">


                                                <%--★☀✔--%>

                                            </td>
                                            <td style="text-align: right; width: 77%">
                                                <asp:DropDownList ID="ddlIOAUsers" Width="300px" runat="server" CssClass="drpClass"></asp:DropDownList>

                                            </td>
                                            <td style="text-align: left;" colspan="2">
                                                <asp:Button ID="btnIOASelect" CssClass="NFButtonWithNoImage" Width="50px" runat="server" Text="Go" OnClientClick="var a = CheckIOAUser(this);CheckFirstClick(); return a" OnClick="btnIOASelect_Click" />
                                                <asp:HiddenField ID="Hiddenfield3" runat="server" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="text-align: left;" colspan="6">&nbsp;</td>
                                        </tr>
                                        <%--<a id="closeHdr" onclick="closePOP();" href="#" style="margin-top: -13px; margin-right: -14px;">
                            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>--%>
                                    </table>
                                </td>
                            </tr>
                             <tr>
                                 <td style="width:100px"></td>
                                <td>  
                                    <div  id="tdMsg2" class='warning_box' style="width:350px; display:none;"> Same user can't open both Normal session and IOA session.</div>
                                    <div  id="tdMsg3" class='warning_box' style="width:350px; display:none;"> This IOA session can't open. There is no normal session associated with this IOA.</div>
                                </td>
                            </tr>

                        </table>
                        <table id="tblIOAndNorm" style="width: 100%; display: none;">
                            <tr>

                                <td align="left" colspan="3">
                                    <b>Select IOA or Normal Session</b>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3"><b style="font-size: 13px;">Previous session and IOA was not submitted. Do you want to continue taking the data or the IOA ?</b></td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <table style="width: 88%;">
                                        <tr>
                                            <td style="text-align: left; width: 49%;">
                                                <b>IOA Session</b>
                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">
                                                <b>Normal Session</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 88px;" class="tdText">User:</label>
                                                <label class="txtnormal" runat="server" id="lblIOAUsr">Username</label>
                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 80px;" class="tdText">User:</label>
                                                <label class="txtnormal" runat="server" id="lblNormalUsr">Username</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 54px;" class="tdText">Start Time:</label>
                                                <label runat="server" id="lblIOAStime">10:00</label>
                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 46px;" class="tdText">Start Time:</label>
                                                <label runat="server" class="txtnormal" id="lblNormalStime">10:00</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 59px;" class="tdText">End Time:</label>
                                                <label runat="server" class="txtnormal" id="lblIOAEtime"></label>
                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 51px;" class="tdText">End Time:</label>
                                                <label runat="server" class="txtnormal" id="lblNormalEtime"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 18px;" class="tdText">Session Number:</label></label>
                                                <label runat="server" class="txtnormal" id="lblIOASessNo">1200</label>
                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">
                                                <label style="padding-right: 12px;" class="tdText">Session Number:</label>
                                                <label runat="server" id="lblSessNo" class="txtnormal">2300</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;"></td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;"></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 49%;">

                                                <asp:Button ID="btnIOA" CssClass="NFButtonWithNoImage" Width="150px" runat="server" Text="Open IOA Session" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" OnClick="btnIOA_Click" />
                                                <asp:HiddenField ID="hfSessIDIOA" runat="server" />

                                            </td>
                                            <td style="text-align: center; width: 2%;"></td>
                                            <td style="text-align: left; width: 49%;">

                                                <asp:Button CssClass="NFButtonWithNoImage" Width="150px" ID="btnNormal" runat="server" Text="Continue" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" OnClick="btnNormal_Click" />
                                                <asp:HiddenField ID="hfSessIDNorm" runat="server" />

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>


                        </table>
                        <table id="tblVTSelectr" style="width: 100%; display: none;">
                            <tr>
                                <td align="right" colspan="3"></td>
                            </tr>
                            <tr>

                                <td align="left" colspan="3">
                                    <b></b>
                                    <hr />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdText" colspan="3" style="text-align: center;"><b></b></td>

                            </tr>
                            <tr>
                                <td colspan="3">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:Button ID="btnExecute" CssClass="NFButtonWithNoImage" Width="100px" runat="server" Text="Execute" OnClick="btnExecute_Click" />
                                    <%--<td style="text-align: left; width: 49%;"></td>--%>
                                    <input type="button" style="width: 100px;" class="NFButtonWithNoImage" value="Enter Data" onclick="temp(); closePOP();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="fullOverlay">
            </div>

            <div id="divPrmpts" class="web_dialog2">
                <a onclick="ClosePromptPop();" href="#" style="margin-top: -13px; margin-right: -14px;">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                <table width="100%">
                    <tr>
                        <td>
                            <div id="divStpPrmpts" runat="server"></div>
                        </td>
                    </tr>
                </table>
            </div>


            <div id="divOverride" class="web_dialog22">
                <img onclick="HidePoupOverride();" style="position: absolute; width: 25px; height: 25px; z-index: 9999; left: 96.5%; margin-top: 1px;" src="../Administration/images/button_red_close.png">
                <div>
                    <table width="100%">
                        <tr>
                            <td colspan="3">
                                <div id="divOver" runat="server" style="height: 300px; overflow-y: auto; overflow-x: hidden;">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="totalTaskOverride">
                                                <table style="width: 100%">
                                                    <thead>
                                                        <tr>
                                                            <td style="text-align: center; width: 33%; min-width: 200px;">Set(s) </td>
                                                            <td style="text-align: center; width: 33%; min-width: 200px;">Step(s) </td>
                                                            <td style="text-align: center; width: 33%; min-width: 200px;">Prompt(s) </td>
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
                                            <asp:Panel runat="server" ID="normalOverride">
                                                <table style="width: 100%">
                                                    <thead>
                                                        <tr>
                                                            <td style="text-align: center; width: 33%; min-width: 200px;">Set(s) </td>
                                                            <td style="text-align: center; width: 33%; min-width: 200px;">Step(s) </td>
                                                            <td style="text-align: center; width: 33%; min-width: 200px;">Prompt(s) </td>
                                                        </tr>
                                                    </thead>
                                                    <tr>
                                                        <td style="vertical-align: top">
                                                            <asp:RadioButtonList ID="RadioButtonListSets" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListSets_SelectedIndexChanged"></asp:RadioButtonList></td>
                                                        <td style="vertical-align: top">
                                                            <asp:RadioButtonList ID="RadioButtonListSteps" runat="server"></asp:RadioButtonList></td>
                                                        <td style="vertical-align: top">
                                                            <asp:RadioButtonList ID="RadioButtonListPrompts" runat="server"></asp:RadioButtonList></td>
                                                    </tr>



                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnOverride" />
                                        </Triggers>
                                    </asp:UpdatePanel>


                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; width: 33%; min-width: 200px;"></td>
                            <td style="text-align: center; width: 33%; min-width: 200px;">
                                <asp:Button ID="btnOverride" class="NFButtonNew" runat="server" Text="Save" Style="height: 32px !important; margin-left: 85px; text-align: center; width: 75px !important;" OnClientClick="$('#divOverride').hide();" OnClick="btnOverride_Click" />
                            </td>
                            <td style="text-align: center; width: 33%; min-width: 200px;"></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="divViewPlan" class="web_dialog" style="top: 7%; left:50%; z-index: 10000; width: 350px; min-height:75px;">
            <a id="A2" onclick="closeViewPlan();" href="#" style="margin-top: -13px; margin-right: -14px; float: right">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" />
            </a>
                <td>
                                            <input id="BtnViewSupStgy" type="button" class="NFButtonWithNoImage" onclick="ViewSupportStrategy(); scrollToTop();" value="Support Strategy" style="width: 134px; margin-top:25px; margin-left:29px; margin-right: 10px;" />
                                        </td>
                                        <td style="width: 10%">
                                            <input id="BtnViewLP" type="button" class="NFButtonWithNoImage" onclick="ViewLp(); scrollToTop();" value="Lesson Plan" style="margin-top:25px;margin-left:10px;width: 134px;margin-right: 25px;"/>
                                        </td>
            </div>

            <div id="divDoc" class="web_dialog" style="top: 7%; z-index: 10000">


                <a id="closeHdrDoc" onclick="closePOPDoc();" href="#" style="margin-top: -13px; margin-right: -14px; float: right">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                <h4>Documents/Videos</h4>
                <div id="divMessage" runat="server" style="width: 98%"></div>


                <div runat="server" style="width: 100%; height: 400px; overflow-x: hidden; overflow-y: scroll">
                    <table id="Table1" style="display: inline-table; width: 100%; border: 0px; height: auto" class="popsecondver">
                        <tr>
                            <td align="left" colspan="2" style="border: none">
                                <asp:UpdatePanel ID="updateFile" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>


                                        <asp:GridView GridLines="none" CellPadding="4" ID="grdFile" PageSize="5" AllowPaging="False" Width="100%" OnRowEditing="grdFile_RowEditing" OnRowCommand="grdFile_RowCommand" AutoGenerateColumns="false" runat="server" OnPageIndexChanging="grdFile_PageIndexChanging" OnRowDataBound="grdFile_RowDataBound" EmptyDataText="No data available">
                                            <Columns>

                                                <asp:BoundField DataField="No" HeaderText="No" HeaderStyle-Width="10px" />

                                                <asp:TemplateField HeaderText="Document/Video Name">
                                                    <ItemTemplate>
                                                        <asp:LinkButton CommandName="download" ID="lnkDownload" Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("LPDoc") %>' ToolTip='<%#Eval("Document") %>' runat="server" OnClientClick="document.forms[0].target = '_blank';"></asp:LinkButton>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />


                                            <RowStyle CssClass="RowStyle" />
                                            <AlternatingRowStyle CssClass="AltRowStyle" />
                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" Width="65%" />
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
                                        <asp:PostBackTrigger ControlID="grdFile" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>

                    </table>
                </div>

            </div>


            
			
			<div id="divViewLesson" class="web_dialog" style=" z-index: 10000; min-height: 80%; max-height: 80%; width: 96%; position: absolute">


                <a id="A1" onclick="CloseViewLP();" href="#" style="right: 10px; position: absolute">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
				<div id="divViewLesson2" style=" min-height: 100%; width: 96%; -webkit-overflow-scrolling: touch; position: absolute">
					<iframe id="stViewLP" src="" frameborder="0" style="width: 100%; height:98%; position: absolute"></iframe>
				</div>

            </div>

            <div id="popupioa"  class="white_content">
    <table>
                            <tr>
                                <td>
                                    <center>
                                <p style="font-size:16px;color:green;"><b>  Discard is not possible after an IOA has been started. Instead, you may mistrial both the Teaching and IOA sessions.</b></p>
                                        </center>
                    </td>
                </tr>
        <br />
                            <tr>
                                <td>
                                    <input id="sub" type="button" value="OK" class="NFButtonWithNoImage"  style="width: 100px; position: relative; left: 170px;"  onclick="document.getElementById('popupioa').style.display = 'none';" />
                                    
                                </td>
                            </tr>
            </table>

</div>


            <style>
                /*--------------Alphonsa---------------*/
                .dashMain {
                    display: none;
                    position: fixed;
                    top: 110px;
                    left: calc(50% - 250px);
                    background-color: white;
                    border: 2px solid black;
                }

                .dashSub {
                    width: 500px;
                    height: 120px;
                    margin: 10px;
                }

               .PrintDiv {
                    visibility:hidden;
                    position: fixed;
                    top: 110px;
                    left: calc(50% - 250px);
                    background-color: white;
                    border: 2px solid black;
                }

                /*--------------Alphonsa---------------*/

                .to_stepDiv {
                    border: 1px solid;
                    padding: 3px;
                    margin: 3px;
                    min-height: 25px;
                    width: 95%;
                    float: left;
                }

                .to_stepDiv_current {
                    border: 1px solid;
                    padding: 3px;
                    margin: 3px;
                    background-color: #84FF84;
                    min-height: 25px;
                    float: left;
                    width: 95%;
                }


                #div_tempOverride {
                    width: 300px;
                    background-color: white;
                    border: 1px solid;
                    position: fixed;
                    top: 5px;
                    left: 300px;
                    z-index: 10001;
                    max-height: 500px;
                    overflow-y: auto;
                }
      
                #div_reason {
                    width: 300px;
                    background-color: white;
                    border: 1px solid;
                    position: fixed;
                    top: 5px;
                    left: 300px;
                    z-index: 10001;
                    max-height: 500px;
                    overflow-y: auto;
                }

                .btn_list {
                    font-size: 10px !important;
                    border: 1px solid #adadad;
                    cursor: pointer;
                }

                    .btn_list:hover {
                        background-color: #adadad;
                    }

                .div_overlay {
                    position: fixed;
                    top: 0;
                    left: 0;
                    width: 100%;
                    height: calc(100%);
                    background-color: white;
                    opacity: 0.5;
                    z-index: 100;
                    display: none;
                }

                .imghrgls {
                    margin: 0 auto;
                    margin-left: calc(50%-79px);
                    margin-top: calc(50%-79px);
                    left: calc(50%-79px);
                    top: calc(50%-79px);
                }

                .white_content {
                          display: none;
                          position: absolute;
                          top: 10%;
                          left: 50%;
                          width: 450px;
                          height: 150px;
                          padding: 16px;
                          border: 1px solid black;
                         
                          background-color: white;
                          z-index: 1002;
                          overflow: auto;
}
            </style>


        </div>

        <%--        <asp:Panel runat="server" ID="pnl_overlay" CssClass="pnl_overlay">
            <img runat="server" id="imghrgls" cssclass="imghrgls" src="../Administration/images/hourglass.gif" />
        </asp:Panel>--%>
        <div id="div_overlay" class="div_overlay">
        </div>



    </form>
</body>
</html>
