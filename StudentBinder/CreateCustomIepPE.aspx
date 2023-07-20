<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateCustomIepPE.aspx.cs" Inherits="StudentBinder_CreateCustomIepPE" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Untitled Document</title>
    <script src="../Administration/JS/jquery-1.8.0.min.js"></script>
    <script src="jsScripts/jq1.js" type="text/javascript" charset="utf-8"></script>
    <script type="jsScripts/text/javascript" src="eye.js"></script>
    <script type="jsScripts/text/javascript" src="layout.js"></script>
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />

    <link href="../Administration/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/jsDatePick.min.1.3.js"></script>
    <link href="../Administration/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/jsDatePick.jquery.min.1.3.js"></script>

    <link href="CSS/AsmntReviewHome.css" rel="stylesheet" id="sized" />

    <link href="CSS/CreateIEP.css" rel="stylesheet" />


    <script src="js_asmnt/jquery-1.8.3.js"></script>
    <script src="js_asmnt/jq1.js"></script>

    <script type="text/javascript">

        var currTab = 0;
        var nextTab = 0;

        $(document).ready(function () {
            $('#CancalGen').click(function () {
                $('#dialog').animate({ top: "-300%" }, function () {
                    $('#overlay').fadeOut('slow');
                  });
            });

            $(".Leftaccord .accord-header").click(function () {
                if ($(this).next("div").is(":visible")) {
                    $(this).next("div").slideUp("slow");
                } else {
                    $(".Leftaccord .accord-content").slideUp("slow");
                    $(this).next("div").slideToggle("slow");
                }
            });

            var pageUrl = document.URL;
            var stringToDelete = "";
            var pageUrl_split = pageUrl.split('/');

            for (i = 0; i < pageUrl_split.length; i++) {

                var splitValue = pageUrl_split[i];
                if (splitValue.indexOf('(S(') > -1) {
                    stringToDelete = splitValue.toLowerCase() + '/';
                }

            }
            //alert(stringToDelete);

            var freeTextPopup = $('#FreetextPopup');

            var freeTextPopup_imgList = $(freeTextPopup).find('img');

            for (i = 0; i < freeTextPopup_imgList.length; i++) {
                var oldSrc = $(freeTextPopup_imgList[i]).attr('src');
                // alert(oldSrc.indexOf(stringToDelete));
                var newSrc = oldSrc.replace(stringToDelete, '');
                //alert(newSrc);
                $(freeTextPopup_imgList[i]).attr('src', newSrc);
            }


            // end
        });

        //Function to disable the Approve button and hide the reject button(Grey Out)
        function disableApprove(elem) {
            $(elem).removeClass('NFButton').addClass('NFButton_disabled').val('Please Wait...');
            $('#btnReject').hide();
            return true;
        }


        function adjustStyle(width) {

            width = parseInt(width);

            if (width >= 988) {
                $("#sized").attr("href", "CSS/CreateIEP.css");
                return;
            }
            if (width < 988) {
                $("#sized").attr("href", "CSS/CreateIEPTab.css");
                return;
            }
        }

        function ConfirmDelete()
        {
            var flag;
            flag = confirm("Are you sure do you want to Delete?")
            return flag;
        }

        function Prompt() {
            $(".fullOverlay").show();
            $(".web_dialog2").show();
        }
        function HidePopup() {
            $(".fullOverlay").hide();
            $(".web_dialog2").hide();
        }

        function deleteDoc() {
            var flag;
            flag = confirm("Are you sure do you want to Delete?")
            return flag;
        }


        function freeTextPopup(PageName, Divcontent) {

            $('#overlay').fadeIn('fast', function () {
                $('#FreetextPopup').css('top', '5%');
                $('#FreetextPopup').css('left', '33%');
                $('#FreetextPopup').show();
            });
            if (PageName == "CreateIEP4.aspx") {
                var arr = $(Divcontent).find('div').attr('id');
                var divdata = $(Divcontent).find('div').html();
                var textdata = $(Divcontent).find('textarea').attr('id');
                $('#hdnpagename').val(PageName);
                $('#hdndivname').val(arr);
                $('#hdntext').val(textdata);
                $('#hdncontent').val(FTB_API['<%=FreeTextBox1.ClientID %>'].SetHtml(divdata));
            }
            else {
                $('#hdnpagename').val(PageName);
                $('#hdndivname').val(Divcontent.id);
                $('#hdncontent').val(FTB_API['<%=FreeTextBox1.ClientID %>'].SetHtml(Divcontent.innerHTML));
            }

        }

        $(function () {
            adjustStyle($(this).width());
            $(window).resize(function () {
                adjustStyle($(this).width());
            });
        });



        window.onload = function () {
            new JsDatePick({
                useMode: 2,
                target: "<%=txtSdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtEdate.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });

        };

        function CheckError(txtSubmit, identity) {

            if (identity == 1) {
                document.getElementById('<%=lnkBtnSubmit.ClientID %>').click();
                document.getElementById('btnIEPExport').style.display = "block";
            }
            else {
                document.getElementById('<%=lnkBtnAprove.ClientID %>').click();
                document.getElementById('btnIEPExport').style.display = "block";
                showWait();
               
            }

        }

        function txtfree() {
            $('#FreetextPopup').animate({ top: "-300%" }, function () {
                $('#overlay').fadeOut('slow');
            });
            var pagename = document.getElementById('hdnpagename').value;
            if (pagename == "CreateIEP4.aspx") {
                var content1 = FTB_API['<%=FreeTextBox1.ClientID %>'].GetHtml();
                var divid = document.getElementById('hdndivname').value;
                var textid = document.getElementById('hdntext').value;
                document.getElementById('ifrmContent').contentWindow.GetFreetextval(content1, divid, textid);
            }
            else {
                var content1 = FTB_API['<%=FreeTextBox1.ClientID %>'].GetHtml();
                var divid = document.getElementById('hdndivname').value;
                document.getElementById('ifrmContent').contentWindow.GetFreetextval(content1, divid);
            }

        }

        //function h2click(h2, IEPid) {
        //    // if section is already open, return false
        //    if ($(h2).is('.active')) {
        //        $(h2).next('div').slideUp('fast');
        //        $(h2).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
        //        return false;
        //    }

        //    // open request and close open
        //    $(h2).parent().parent().find('.accordion > div').slideUp('fast');
        //    $(h2).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
        //    $(h2).addClass("active");
        //    $(h2).next('div').slideToggle('fast');

        //    // fix IE 6 bug.
        //    if (jQuery.browser.msie && jQuery.browser.version < 7) {
        //        $('.accordion div').addClass('iefix');
        //    }
        //    SetIepID(IEPid);
        //    return false;
        //}

        function h2click(h2, IEPid) {
            document.getElementById('<%=tdMsg.ClientID %>').innerHTML = "";
            // if section is already open, return false
            if ($(h2).is('.active')) {
                //$(h2).removeClass("active");
                //$(h2).next('div').slideUp('fast');
                //$(h2).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
                return false;
            }
            else {
                $(h2).addClass("active");
            }

            // open request and close open
            //$(h2).parent().parent().find('.accordion > div').slideUp('fast');
            $(h2).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
            //$(h2).next('div').slideDown('fast');

            // fix IE 6 bug.
            if (jQuery.browser.msie && jQuery.browser.version < 7) {
                $('.accordion div').addClass('iefix');
            }

            SetIepID(IEPid);
            return false;
        }

        function SetIepID(IEPID) {

            $.ajax(
          {
              type: "POST",
              url: "CreateCustomIepPE.aspx/SetIEPId",
              data: "{'ID':'" + IEPID + "'}",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (data) { },
              error: function (request, status, error) {
                  alert("No IEP is created");
              }
          });
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "none");


        }
        function FindIEPPAge(Status, IEPID, IEPURL) {
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "none");
            document.getElementById('btnIEPExport').style.display = "block";
            if (Status == "Approve") {
                document.getElementById('btnapprove').style.display = "none";
                document.getElementById('btnsubmit').style.display = "none";
                document.getElementById('btnIEPExport').style.display = "block";
            }
            else {
                var permission = document.getElementById('<%=(hfCheckError).ClientID %>').value;
                if (permission == "true") {
                    document.getElementById('btnapprove').style.display = "block";
                    document.getElementById('btnsubmit').style.display = "none";
                    document.getElementById('btnIEPExport').style.display = "block";
                    
                }
                else {
                    document.getElementById('btnapprove').style.display = "none";
                    document.getElementById('btnsubmit').style.display = "none";
                    document.getElementById('btnIEPExport').style.display = "block";
                }
            }
            SetIepID(IEPID);
            $('#ifrmContent').attr('src', IEPURL);

            //code to highlight the left panel with selected page

            var opendWrapper = $('.wrapper');

            for (var i = 0; i < opendWrapper.length; i++) {

                var pageNo = parseInt(IEPURL.replace("CreateIEP-PE", "").replace(".aspx", ""));
                $(opendWrapper[i]).find('.nobdrrcontainer').find('.grmb').removeClass('grimgbAck');
                $(opendWrapper[i]).find('.nobdrrcontainer').find('.grmb').eq(pageNo - 1).addClass('grimgbAck');
            }



            //end


            document.getElementById('<%=(hdnFieldIep).ClientID %>').value = IEPID;
        }

        // FUNCTION TO AUTO SAVE THE EXISTING FORM, WHEN A LEFT BUTTON IS CLICKED.
        function submitTheForm(clickedTab) {
            
            nextTab = clickedTab;

            if (currTab != nextTab && currTab > 0) {


                var subButton = $('#ifrmContent').contents().find('#form1').find('input[type=submit]'); // TO GET THE BUTTON INSIDE THE CURRENT FORM.
                //var buttonList = ["btnSubmitIEP1_hdn", "btnSave_hdn", "btn_Update_hdn", "btnUpdate_hdn", "btnSaveIep9_hdn", "Button1_hdn"];
                var buttonList = ["btnSubmitIEP1", "btnSave", "btn_Update", "btnUpdate", "btnSaveIep9", "Button1"];

                if (subButton.length > 0) {
                    for (var i = 0; i < subButton.length; i++) {
                        if ($.inArray($(subButton[i]).attr('id'), buttonList) > -1) {

                            $(subButton[i]).trigger('click');               // INDUCING CURRENT FORM'S SUBMIT BUTTON'S CLICK WHEN CLICKING ANY LEFT BUTTON.
                        }
                    }
                }


            }
            if (currTab == 0) {
                moveToNextTab(nextTab);
            }

            return false;
        }
        
        //FUNCTION TO MOVE THE FOCUS TO NEXT TAB OR THE TAB CLICKED FROM LEFT MENU.
        function moveToNextTab(nextTab_est) {

            var result = false;

            if (currTab == nextTab) {

                nextTab = nextTab_est;
            }
            
            
            switch (nextTab) {//pramod
                case 1:
                    result = CreateIEPClick();
                    break;
                case 2:
                    result = CreateIEP2('saved');
                    break;
                case 3:
                    result = CreateIEP3('saved')
                    break;
                case 4:
                    result = CreateIEP4('saved')
                    break;
                case 5:
                    result = CreateIEP5('saved')
                    break;
                case 6:
                    result = CreateIEP6('saved')
                    break;
                case 7:
                    result = CreateIEP7('saved')
                    break;
                case 8:
                    result = CreateIEP8('saved')
                    break;
                case 9:
                    result = CreateIEP9('saved')
                    break;
                case 10:
                    result = CreateIEP10('saved')
                    break;
                case 11:
                    result = CreateIEP11('saved')
                    break;
                case 12:
                    result = CreateIEP12('saved')
                    break;
                case 13:
                    result = CreateIEP13('saved')
                    break;
                case 14:
                    result = CreateIEP14('saved')
                    break;
                case 15:
                    result = CreateIEP15('saved')
                    break;

            }

            currTab = nextTab;

            return result;
        }


        function CreateIEPClick() {

            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            return CreateIEP1();
        }
        function OnSucess(response) {
            document.getElementById('<%=(hdnFieldIep).ClientID %>').value = response;
        }
        function OnFailure(error) {
            if (error) {
                alert(error._message);
            }
        }
        function CreateIEP1() {

            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;

            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'grmb grimgbAck');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');

                $('#ifrmContent').attr('src', 'CreateIEP-PE1.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }
        function CreateIEP2(status) {

            //submitTheForm();

            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'grmb grimgbAck');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnPrestLvlEdu2').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE2.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }
        function CreateIEP3(status) {

            //submitTheForm();

            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'grmb grimgbAck');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnCurPre').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE3.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }
        function CreateIEP4(status) {

            //submitTheForm();

            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'grmb grimgbAck');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#LnkBtnSerDel').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE4.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }
        function CreateIEP5(status) {

            //submitTheForm();

            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {

                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'grmb grimgbAck');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnNonParJus').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE5.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }
        function CreateIEP6(status) {

            //submitTheForm();

            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'grmb grimgbAck');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnStateOfDist').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE6.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }
        function CreateIEP7(status) {
            //submitTheForm();

            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'grmb grimgbAck');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnAddInfo').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE7.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }
        function CreateIEP8(status) {
            //submitTheForm();

            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'grmb grimgbAck');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnLocalAsmnt').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE8.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }
        function CreateIEP9(status) {
            //submitTheForm();
            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'grmb grimgbAck');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnGoalndObj').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE9.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }

        function CreateIEP10(status) {
            //submitTheForm();
            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'grmb grimgbAck');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnSpclConsider').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE10.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }

        function CreateIEP11(status) {
            //submitTheForm();
            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'grmb grimgbAck');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnGftdSupport').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE11.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }

        function CreateIEP12(status) {
            //submitTheForm();
            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'grmb grimgbAck');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnEdulPlcmnt').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE12.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }

        function CreateIEP13(status) {
            //submitTheForm();
            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'grmb grimgbAck');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'negative');
                // $('#lnkBtnEdulPlcmnt2').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE13.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }

        function CreateIEP14(status) {
            //submitTheForm();
            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'grmb grimgbAck');
                $('#lnkBtnPennData').attr('class', 'negative');
                //$('#lnkBtnPennData').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE14.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }
        function CreateIEP15(status) {
            //submitTheForm();
            //if (status == "saved") {
            //    alert("Data Successfully Updated");
            //}
            PageMethods.GetInProgressIEPId(OnSucess, OnFailure);
            document.getElementById('btnapprove').style.display = "none";
            document.getElementById('btnsubmit').style.display = "block";
            document.getElementById('btnIEPExport').style.display = "block";
            SetIepID(document.getElementById('<%=(hdnFieldIep).ClientID %>').value);
            document.getElementById("<%=tdMsgMain.ClientID%>").innerHTML = "";
            $('#' + '<%=(btndeleteIEP).ClientID %>').css("display", "block");
            var iep = document.getElementById('<%=(hdnFieldIep).ClientID %>').value;
            if (parseInt(iep) > 0) {
                $('#lnkBtnIndiviEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu').attr('class', 'negative');
                $('#lnkBtnPrestLvlEdu2').attr('class', 'negative');
                $('#lnkBtnCurPre').attr('class', 'negative');
                $('#LnkBtnSerDel').attr('class', 'negative');
                $('#lnkBtnNonParJus').attr('class', 'negative');
                $('#lnkBtnStateOfDist').attr('class', 'negative');
                $('#lnkBtnAddInfo').attr('class', 'negative');
                $('#lnkBtnLocalAsmnt').attr('class', 'negative');
                $('#lnkBtnGoalndObj').attr('class', 'negative');
                $('#lnkBtnSpclConsider').attr('class', 'negative');
                $('#lnkBtnGftdSupport').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt').attr('class', 'negative');
                $('#lnkBtnEdulPlcmnt2').attr('class', 'negative');
                $('#lnkBtnPennData').attr('class', 'grmb grimgbAck');

                $('#ifrmContent').attr('src', 'CreateIEP-PE15.aspx');
            }
            else {
                popupShow();
            }
            getIepUdateStatus();
            return false;
        }

        function resizeIframe(obj) {
            //alert(obj.style.height);
            obj.style.height = "900px";//obj.contentWindow.document.body.scrollHeight + 'px';


        }
        function popupShow() {
            $('#overlay').fadeIn('fast', function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#close_x').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); });
        }
        function hideApprRej() {
            $('#DilogAprove').fadeOut('slow', function () {
                $('#overlay').fadeOut('fast');
            });
        }
        function showApprRej() {
            $('#ajaxloader').fadeOut();
            $('#DilogAprove').show();
            $('#DilogAprove').css("top", "5%");
            $('#overlay').show();
        }

        function hideRecentNotes() {
            $('#ViewRecentNotes').fadeOut('slow', function () {
                $('#overlay').fadeOut('fast');
            });
        }

        function ViewNotes() {
            $('#overlay').show();
            $('#ViewRecentNotes').show();
            $('#ViewRecentNotes').css("top", "5%");

        }

        function getIepUdateStatus() {
            //start progress here , should keep the de reff of wich progress is started
            var iep = document.getElementById('hdnFieldIep').value;
            $.ajax({
                url: "CreateCustomIepPE.aspx/IepUpdateStatus",
                data: "{ 'testVal': '" + iep + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {

                    var contents = data.d;

                    if (contents.indexOf("System.IndexOutOfRangeException") == -1) {
                        if (contents != "") {

                            var testval = contents.split(':');
                            if (testval[0] == 'True') {
                                if ($('#lnkBtnIndiviEdu').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnIndiviEdu').removeClass('negative');
                                    $('#lnkBtnIndiviEdu').addClass('gtic');
                                }
                            }
                            else {
                                if ($('#lnkBtnIndiviEdu').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnIndiviEdu').attr('CssClass', 'negative');
                                }
                            }


                            if (testval[1] == 'True') {
                                if ($('#lnkBtnPrestLvlEdu').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnPrestLvlEdu').removeClass('negative');
                                    $('#lnkBtnPrestLvlEdu').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnPrestLvlEdu').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnPrestLvlEdu').attr('CssClass', 'negative');
                                }
                            }


                            if (testval[2] == 'True') {
                                if ($('#lnkBtnPrestLvlEdu2').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnPrestLvlEdu2').removeClass('negative');
                                    $('#lnkBtnPrestLvlEdu2').addClass('gtic');
                                }
                            }
                            else {
                                if ($('#lnkBtnPrestLvlEdu2').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnPrestLvlEdu2').attr('CssClass', 'negative');
                                }
                            }


                            if (testval[3] == 'True') {
                                if ($('#lnkBtnCurPre').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnCurPre').removeClass('negative');
                                    $('#lnkBtnCurPre').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnCurPre').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnCurPre').attr('CssClass', 'negative');
                                }
                            }


                            if (testval[4] == 'True') {
                                if ($('#LnkBtnSerDel').attr('class') != 'grmb grimgbAck') {
                                    $('#LnkBtnSerDel').removeClass('negative');
                                    $('#LnkBtnSerDel').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#LnkBtnSerDel').attr('class') != 'grmb grimgbAck') {
                                    $('#LnkBtnSerDel').attr('CssClass', 'negative');
                                }
                            }


                            if (testval[5] == 'True') {
                                if ($('#lnkBtnNonParJus').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnNonParJus').removeClass('negative');
                                    $('#lnkBtnNonParJus').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnNonParJus').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnNonParJus').attr('CssClass', 'negative');
                                }
                            }


                            if (testval[6] == 'True') {
                                if ($('#lnkBtnStateOfDist').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnStateOfDist').removeClass('negative');
                                    $('#lnkBtnStateOfDist').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnStateOfDist').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnStateOfDist').attr('CssClass', 'negative');
                                }
                            }
                            //lnkBtnLocalAsmnt

                            if (testval[7] == 'True') {
                                if ($('#lnkBtnAddInfo').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnAddInfo').removeClass('negative');
                                    $('#lnkBtnAddInfo').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnAddInfo').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnAddInfo').attr('CssClass', 'negative');
                                }
                            }

                            if (testval[8] == 'True') {
                                if ($('#lnkBtnLocalAsmnt').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnLocalAsmnt').removeClass('negative');
                                    $('#lnkBtnLocalAsmnt').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnLocalAsmnt').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnLocalAsmnt').attr('CssClass', 'negative');
                                }
                            }
                            if (testval[9] == 'True') {
                                if ($('#lnkBtnGoalndObj').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnGoalndObj').removeClass('negative');
                                    $('#lnkBtnGoalndObj').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnGoalndObj').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnGoalndObj').attr('CssClass', 'negative');
                                }
                            }

                            if (testval[10] == 'True') {
                                if ($('#lnkBtnSpclConsider').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnSpclConsider').removeClass('negative');
                                    $('#lnkBtnSpclConsider').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnSpclConsider').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnSpclConsider').attr('CssClass', 'negative');
                                }
                            }
                            if (testval[11] == 'True') {
                                if ($('#lnkBtnGftdSupport').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnGftdSupport').removeClass('negative');
                                    $('#lnkBtnGftdSupport').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnGftdSupport').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnGftdSupport').attr('CssClass', 'negative');
                                }
                            }
                            if (testval[12] == 'True') {
                                if ($('#lnkBtnEdulPlcmnt').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnEdulPlcmnt').removeClass('negative');
                                    $('#lnkBtnEdulPlcmnt').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnEdulPlcmnt').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnEdulPlcmnt').attr('CssClass', 'negative');
                                }
                            }
                            if (testval[13] == 'True') {
                                if ($('#lnkBtnEdulPlcmnt2').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnEdulPlcmnt2').removeClass('negative');
                                    $('#lnkBtnEdulPlcmnt2').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnEdulPlcmnt2').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnEdulPlcmnt2').attr('CssClass', 'negative');
                                }
                            }
                            if (testval[14] == 'True') {
                                if ($('#lnkBtnPennData').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnPennData').removeClass('negative');
                                    $('#lnkBtnPennData').addClass('gtic');
                                }

                            }
                            else {
                                if ($('#lnkBtnPennData').attr('class') != 'grmb grimgbAck') {
                                    $('#lnkBtnPennData').attr('CssClass', 'negative');
                                }
                            }


                        }
                        //end progess here
                    }
                    else {
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }

            });
        }

        function showWait() {
            $('#btnapprove').attr('disable', 'disable');
            document.getElementById('btnapprove').style.display = "none";
            $('#ajaxloader').fadeIn();
            //alert("Please wait some moments.. Approval In progress..");

            //$('.loading').show();
            //$('#fullContents').hide();

        }
        function HideWait() {
            $('.loading').hide();
            $('#fullContents').show();
        }
        function showMessage(msg) {
            alert(msg);
        }

        function DownloadDone() {
            CreateIEP1();
            HideWait();
        }

        function checkPostback() {
            var iep = document.getElementById('hdnFieldIep').value;
            if (parseInt(iep) > 0) {
                return true;
            }
            else {
                return false;
            }
        }
        function checkPostbackExport() {
            var iep = document.getElementById('hdnFieldIep').value;
            if (parseInt(iep) > 0) {
                return true;
            }
            else {
                alert("Please Select IEP before Export");
                return false;
            }
        }


        function scrollToTop() {
            $('#dialog').hide();
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 100);           
        }


    </script>
    <script>
        function allowBackSpace(e, obj) {
            var evt = e || window.event;
            if (evt.keyCode == 8) {
                obj.value = '';
            }
        }
    </script>

    <style>
         .wrap { white-space: normal; width: 100px; }
        .SubmitApprove {
            display: none;
        }

        .accord-content {
            display: none;
        }

        .FreeTextDivContent {
            width: 98%;
            min-height: 200px;
            height: auto;
            padding: 2%;
            border: 1px solid rgb(228, 228, 228);
            border-radius: 8px 8px 8px 8px;
        }

        .FreeTextBox1_OuterTable select {
            border-radius: 2px !important;
            border: 1px solid #808080;
            background-color: white;
        }

        /* FOR LOADING IMAGE AT IEP EXPORT */
        .loading {
            display: none;
            position: absolute;
            width: 100%;
            height: 800px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            background-image: url("../images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
        }

        #fullContents {
            display: block;
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

        input.NFButton1 {
            background-color: #03507D;
            width: 87%;
            height: 26px;
            color: #fff;
            padding: 0 10px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: bold;
            text-decoration: none;
            background-position: 0 0;
            border: none;
            cursor: pointer;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 3px;
            margin: 0 0 5px;
        }

            input.NFButton1:hover {
                background-color: #09646A;
            }
             

    </style>
    <style type="text/css">
        #ajaxloader {
            display: none;
            position: fixed;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            background: rgba(255,255,255, .8 ) url('../StudentBinder/images/load.gif') 50% 20% no-repeat;
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
         .NFButton_disabled {
            background-color: #CCCCCC;
            background-position: 0 0;
            border: medium none;
            border-radius: 5px;
            color: #fff;
            cursor: pointer;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: bold;
            height: 26px;
            text-decoration: none;
            width: 91px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ajaxloader"></div>
        <div class="loading">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
        <div id="fullContents">
            <div>
                <table style="width: 100%">
                    <tr>
                        <td id="tdMsgMain" runat="server"></td>
                    </tr>

                    <tr>
                        <td>
                            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

                        </td>

                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 80%">
                                        <img runat="server" alt="Please Wait..." id="PlzWait" src="../Administration/images/PleaseWait.gif" style="width: 40px; height: 40px; display: none" />
                                    </td>
                                    <td></td>
                                </tr>

                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <div style="text-align: right">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100%" runat="server" id="tdMsg"></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="boxContainerContainer">
                                <!-------------------------Top Container Start----------------------->
                                <div class="itlepartContainer">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 25%">
                                                <h2>1 <span>IEP Navigator</span></h2>

                                            </td>
                                            <td style="width: 45%">
                                                <h2>2 <span style="width: 60%">Customize IEP</span></h2>
                                            </td>
                                            <td style="width: 4%"></td>
                                            <td style="width: 5%">

                                                <input id="btnsubmit" runat="server" type="button" value="Submit" class="NFButton" onclick="javascript: CheckError(this, 1);" style="display: none" />
                                                <input id="btnapprove" runat="server" type="button" value="Approve" class="NFButton" onclick="javascript: CheckError(this, 2); " style="display: none;" />
                                            </td>

                                            <td style="width: 1%">
                                                <input type="button" value="Recent Notes" onclick="ViewNotes();" class="NFButton" />
                                            </td>
                                            <td style="width: 1%">
                                                <asp:Button ID="btndeleteIEP" runat="server" Text="Delete IEP" CssClass="NFButton" OnClientClick="return ConfirmDelete();" OnClick="btndeleteIEP_Click"></asp:Button>
                                            </td>
                                            <td style="width:1%;">
                                                <%--<input id="btnUploadBSP" runat="server" type="button" value="UploadBSP" class="NFButton" onclick="" />--%>
                                                <asp:Button ID="btnUploadBSP" runat="server" Text="Upload BSP" CssClass="NFButton" OnClick="btnUploadBSP_Click"  Visible="false"/>
                                            </td>
                                            <td style="width: 1%">

                                                <asp:Button ID="btnIEPExport" runat="server" CssClass="ExportWord" ToolTip="Export To Word" Text="" OnClick="btnIEPExport_Click" OnClientClick="return checkPostbackExport();"></asp:Button>
                                            </td>

                                            <td style="width: 3%">
                                                <%--<asp:ImageButton ID="btnRefresh" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" Style="float: right;" />--%>
                                                <asp:Button ID="btnRefresh" runat="server" CssClass="refresh" Text="" ToolTip="Refresh" OnClick="btnRefresh_Click"></asp:Button>

                                            </td>
                                        </tr>

                                    </table>






                                </div>
                                <!-------------------------Top Container End----------------------->

                                <!-------------------------Middle Container start----------------------->
                                <%--  --%>
                                <div class="btContainerPart">
                                    <!------------------------------------MContainer Start----------------------------------->

                                    <div class="mBxContainer">
                                        <div>

                                            <h3>IEP - Approved </h3>
                                            <%--<div id="ApprovedIEP" runat="server" class="Leftaccord"></div>--%>
                                            <div id="Div7">
                                                <div id="Div8">
                                                    <ul id="ul_ApprovedIEP" runat="server">
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>



                                        <div>

                                            <h3>IEP - Pending Approval </h3>
                                            <%-- <div id="PendingIEP" runat="server" class="Leftaccord"></div>--%>
                                            <div id="Div1">

                                                <div id="Div2">
                                                    <ul id="ul_PendingIEP" runat="server">
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>


                                        <h3>New IEP - In Progress</h3>
                                        
                                        <asp:HiddenField ID="hdnFieldIep" runat="server" />
                                        <asp:LinkButton ID="lnkBtnIEPYear" CssClass="grmb BlueBtnType" runat="server" Text=" " OnClientClick="return false;"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnIndiviEdu" runat="server" OnClientClick="return submitTheForm(1);" CssClass="negative" ToolTip="Individualized Education">Individualized Education</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnPrestLvlEdu" runat="server" OnClientClick="return submitTheForm(2);" CssClass="negative" ToolTip="IEP Team/Signatures">IEP Team/Signatures</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnPrestLvlEdu2" runat="server" OnClientClick="return submitTheForm(3);" CssClass="negative" ToolTip="Procedural Safeguards Notice">Procedural Safeguards Notice </asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnCurPre" runat="server" OnClientClick="return submitTheForm(4);" CssClass="negative" ToolTip="Special Considerations">Special Considerations</asp:LinkButton>
                                        <asp:LinkButton ID="LnkBtnSerDel" runat="server" OnClientClick="return submitTheForm(5);" CssClass="negative" ToolTip="Present Levels Of Academic Achievement And Functional Performance">Present Levels Of Academic..</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnNonParJus" runat="server" OnClientClick="return submitTheForm(6);" CssClass="negative" ToolTip="Postsecondary Education and Training Goal">Postsecondary Education and ..</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnStateOfDist" runat="server" OnClientClick="return submitTheForm(7);" CssClass="negative" ToolTip="Participation In State And Local Assessments">Participation In State And ..</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnAddInfo" runat="server" OnClientClick="return submitTheForm(8);" CssClass="negative" ToolTip="Additional Information">Additional Information</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnLocalAsmnt" runat="server" OnClientClick="return submitTheForm(9);" CssClass="negative" ToolTip="Local Assesments">Local Assessments</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnGoalndObj" runat="server" OnClientClick="return submitTheForm(10);" CssClass="negative" ToolTip="Goals And Objectives">Goals And Objectives</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnSpclConsider" runat="server" OnClientClick="return submitTheForm(11);" CssClass="negative" ToolTip="Special Considerations">Special Considerations</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnGftdSupport" runat="server" OnClientClick="return submitTheForm(12);" CssClass="negative" ToolTip="Gifted Support Service For a Student">Gifted Support Service For..</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnEdulPlcmnt" runat="server" OnClientClick="return submitTheForm(13);" CssClass="negative" ToolTip="Educational Placement">Educational Placement</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnEdulPlcmnt2" runat="server" OnClientClick="return submitTheForm(14);" CssClass="negative" ToolTip="Educational Placement- II">Educational Placement- II</asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnPennData" runat="server" OnClientClick="return submitTheForm(15);" CssClass="negative" ToolTip="Penndata Reporting">Penndata Reporting</asp:LinkButton>
                                        <asp:Button ID="Button1" runat="server" type="button" Text="Create new IEP" CssClass="NFButton1" ToolTip="Create new IEP" OnClick="btnNewIEP_Click" OnClientClick="$('#sign_up5').hide();"></asp:Button>
                                        <asp:Button ID="Button2" runat="server" type="button" Text="Create a new version of an existing IEP" CssClass="NFButton1 wrap" ToolTip="Create a new version of an existing IEP" OnClick="btnNewExistIEP_Click" Height="40px" OnClientClick="scrollToTop()"></asp:Button>
                                        
                                        <br clear="all" />
                                        <br clear="all" />
                                        <%--<h3>Quick Links</h3>
                                <a id="AsmntReview.aspx" onclick="parent.selSubmenu(this,0,0);" class="blButton">Choose Goals And Lesson Plans</a>
                                <a href="CustomizeTemplateEditor.aspx" class="blButton">Customize lesson plan</a>

                                <asp:LinkButton ID="lnkBtnExport" runat="server" CssClass="blButton" Text="Export To Word" OnClick="lnkBtnExport_Click" OnClientClick="return checkPostbackExport();"></asp:LinkButton>
                                        --%>



                                        <div class="clear"></div>
                                    </div>
                                    <!------------------------------------MContainer End----------------------------------->

                                    <!------------------------------------End Container Start----------------------------------->
                                    <div class="righMainContainer large">
                                        <div class="clear">
                                            <iframe style="border-style: 0; border-width: 0px; width: 100%" id="ifrmContent" onload='javascript:resizeIframe(this);'></iframe>
                                        </div>
                                    </div>

                                    <!------------------------------------End Container End----------------------------------->

                                    <div class="clear"></div>
                                </div>

                                <!-------------------------Middle Container End----------------------->


                                <div class="clear"></div>
                            </div>

                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <%--PopUp to Upload BSP Form 05-05-2015 Hari--%>

        <div id="divPrmpts" class="web_dialog2">
            <a id="A4" onclick="HidePopup();" href="#" style="margin-top: -13px; margin-right: -14px;">
                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
            <br />
            <h4>Upload BSP</h4>
            <hr />
            <br />
            <asp:UpdatePanel runat="server" ID="updateFile" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divMessage" runat="server" style="width: 98%"></div>
                    <div style="width: 100%; padding-left: 10px;">
                        <asp:FileUpload ID="fupDoc" runat="server" Style="width:300px;" />
                        <asp:Button ID="btUpload" runat="server" Text="Upload" OnClick="btUpload_Click" CssClass="NFButton" />
                        <br />
                        <asp:Label ID="lMsg" runat="server" />

                    </div>
                    <br />
                    <asp:GridView GridLines="none" CellPadding="4" ID="grdFile" PageSize="5" AllowPaging="True" Width="100%" OnRowEditing="grdFile_RowEditing" OnRowCommand="grdFile_RowCommand" AutoGenerateColumns="false" runat="server" OnPageIndexChanging="grdFile_PageIndexChanging" OnRowDataBound="grdFile_RowDataBound">
                        <Columns>


                            <asp:BoundField DataField="No" HeaderText="No" HeaderStyle-Width="10px" />

                            <asp:TemplateField HeaderText="Document Name">
                                <ItemTemplate>
                                    <asp:Label  ID="lnkDownload" ToolTip='<%# Eval("Document") %>' Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("BSPDoc") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="15px">
                                <ItemTemplate>
                                    <asp:ImageButton OnClientClick="javascript:return deleteDoc();" ID="lb_edit" runat="server" class="btn btn-red" CommandArgument='<%# Eval("BSPDoc") %>' CommandName="Edit" ImageUrl="~/Administration/images/trash.png" Width="18px" BackColor="Black" />
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

        <%--End--%>

        <div id="overlay" class="web_dialog_overlay"></div>

        <div id="dialog" class="web_dialog" style="width: 900px; margin-left: -22%;">
            
            <div id="message" style="text-align: center">
            </div>
            <asp:Panel runat="server" ID="pnl_createNewIEPmsg" class="okdiv" Style="text-align: center" Visible="false">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 20%" class="tdText">
                            <pre> This will create a new IEP, not a revision of the current IEP, even if you choose the same Academic Years.</pre>
                        </td>
                    </tr>
                </table>

                <input type="button" value="OK" class="NFButton" onclick='javascript: $("#sign_up5").show(); $(".okdiv").hide();' />&nbsp;&nbsp;&nbsp;
               <input type="button" value="Cancel" class="NFButton" onclick='javascript: $("#CancalGen").trigger("click"); ' />
            </asp:Panel>


            <div id="sign_up5" style="width: 100%; margin-left: 10px;display: none;">


                <table style="width: 100%">
                    <tr>
                        <td runat="server" id="tdMessage" class="tdText" colspan="6"></td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Panel ID="pnl_currIEPdetails" runat="server" Visible="false" style="text-align:center;">
                                <span>The current IEP is </span><asp:Label ID="lbl_CurrYear" runat="server" Style="font-weight: bold;" Text="20XX-20XX Version X.XX"></asp:Label>
                                <span>. Are you sure you want to Revise this IEP?</span>
                            </asp:Panel>
                            <asp:Panel ID="pnl_iepDatePicker" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 20%" class="tdText">IEP Start Date:</td>
                                        <td style="width: 1%"><span style="color: red">*</span></td>
                                        <td style="width: 35%" class="tdText">
                                            <asp:TextBox ID="txtSdate" runat="server" CssClass="textClass" Width="250px" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox></td>
                                        <td style="width: 10%" class="tdText">IEP End Date:</td>
                                        <td style="width: 1%"><span style="color: red">*</span></td>
                                        <td style="width: 35%" class="tdText">
                                            <asp:TextBox ID="txtEdate" runat="server" CssClass="textClass" Width="250px" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox></td>
                                    </tr>
                                     <tr>
                                        <td colspan="6">Choose Generate Option</td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                        <asp:RadioButtonList ID="lList" runat="server"  RepeatDirection="Vertical">
                                             <asp:ListItem Enabled="true" Selected="True" Value="1">List all prior lessons as Active on the new IEP</asp:ListItem>
                                             <asp:ListItem Enabled="true" Value="2">List all prior lessons as Active on the new IEP AND start new versions of all current lessons In Progress.</asp:ListItem>
                                             
                                         </asp:RadioButtonList>
                                            </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>

                    </tr>

                    <tr>
                        <td class="tdText" style="text-align: center" colspan="6"></td>

                    </tr>
                    <tr>
                        <td class="tdText" style="text-align: center" colspan="6">
                            <asp:Button ID="btnGenIED" runat="server" Text="Generate IEP" OnClick="btnGenIED_Click" CssClass="NFButton" />
                            <%--not visible--%>
                            <asp:Button ID="btnGennewIEP" runat="server" Text="Generate IEP" OnClick="btnGennewIEP_Click" CssClass="NFButton" />
                            <input type="button" id="CancalGen" class="NFButton" value="Cancel" />
                        </td>
                    </tr>

                </table>


            </div>
        </div>



        <div id="DilogAprove" class="web_dialog" style="width: 710px; top: -20%;">

            <div id="Div3" style="width: 700px; margin-top: 16px">

                <a id="A1" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;" onclick="hideApprRej();">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: -12px; margin-top: -20px; z-index: 300" width="18" height="18" alt="" /></a>
                <h3>Notes</h3>
                <hr />
                <table width="100%" cellpadding="0" cellspacing="0" style="text-align: left;">

                    <tr>
                        <td width="20%">Reason
                        </td>
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
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="NFButton" OnClick="btnAdd_Click" OnClientClick="return disableApprove(this);" />
                            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="NFButton" OnClick="btnReject_Click" />
                        </td>
                    </tr>
                </table>



            </div>
        </div>

        <div id="ViewRecentNotes" class="web_dialog" style="width: 710px; top: -23%;">
            <div id="Div4" style="width: 700px; margin-top: 16px">
                <a id="A3" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;" onclick="hideRecentNotes();">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: -12px; margin-top: -20px; z-index: 300" width="18" height="18" alt="" /></a>

                <h3 style="color: White">Recent Notes</h3>
                <asp:UpdatePanel runat="server" ID="recentnotesid">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="Dlnotes" runat="server" AutoGenerateColumns="False"
                                        Width="100%" PageSize="5" AllowPaging="True"
                                        OnPageIndexChanging="Dlnotes_PageIndexChanging" GridLines="None" CellPadding="4" EmptyDataText="No data available">
                                        <Columns>
                                            <asp:BoundField DataField="Version" HeaderText="IEP Version">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Type" HeaderText="Type">
                                                <ItemStyle Width="5%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RecentNote" HeaderText="Reason">
                                                <ItemStyle Width="55%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RecentDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date" HtmlEncode="false">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
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
                                </td>

                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>

        <div id="PopDownload" class="web_dialog" style="width: 600px; top: -20%;">

            <div id="Div53" style="width: 700px;">


                <table style="width: 97%">
                    <tr>
                        <td colspan="2">
                            <table style="width: 85%">
                                <tr>
                                    <td runat="server" id="tdMsgExport" class="tdText" style="height: 50px"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">

                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" />

                        </td>
                        <td style="text-align: left">

                            <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" />

                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <div id="FreetextPopup" class="web_dialog">

            <FTB:FreeTextBox ID="FreeTextBox1" Focus="true" SupportFolder="../FreeTextBox/" Width="99%" Height="500px"
                JavaScriptLocation="ExternalFile" ButtonImagesLocation="ExternalFile" ToolbarImagesLocation="ExternalFile"
                ToolbarStyleConfiguration="OfficeXP" ToolbarLayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,                                   
FontForeColorPicker,FontBackColorsMenu,FontBackColorPicker,Bold,Italic,Underline,RemoveFormat,JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList"
                runat="Server" DesignModeCss="designmode.css" ButtonSet="Office2000" BackColor="White" EnableHtmlMode="true" />
            <div style="width: 100%; text-align: center">
                <input type="button" id="btnFreeTextBox" onclick="txtfree();" value="Done" class="NFButton" />
                <input type="hidden" id="hdnpagename" value="" />
                <input type="hidden" id="hdndivname" value="" />
                <input type="hidden" id="hdncontent" value="" />
                <input type="hidden" id="hdntext" value="" />
            </div>
        </div>
        <div>
            <asp:HiddenField ID="hdFieldSucc" runat="server" />
        </div>
        <div>
            <asp:HiddenField ID="hfCheckError" runat="server" />
            <asp:Button ID="lnkBtnSubmit" runat="server" CssClass="SubmitApprove" Text="Submit" OnClick="lnkBtnSubmit_Click"></asp:Button><%--OnClientClick="return checkPostback();"--%>
            <asp:Button ID="lnkBtnAprove" runat="server" CssClass="SubmitApprove" Text="Approve" OnClick="lnkBtnAprove_Click"></asp:Button>
        </div>
    </form>
    <script type="text/javascript">
        FTB_FreeTextBox.prototype.CancelEvent = function (ev) {
            try {
                ev.preventDefault();
                ev.stopPropagation();
            }
            catch (e) {
                if (FTB_Browser.isIE) {
                    ev.cancelBubble = true;
                    ev.returnValue = false;
                }
            }
        };
    </script>
</body>
</html>
