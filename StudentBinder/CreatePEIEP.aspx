<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreatePEIEP.aspx.cs" Inherits="StudentBinder_CreatePEIEP" %>
<meta http-equiv="X-UA-Compatible" content="IE=9" />
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
        });



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
                dateFormat: "%m-%d-%Y",
            });
            new JsDatePick({
                useMode: 2,
                target: "<%=txtEdate.ClientID%>",
                dateFormat: "%m-%d-%Y",
            });

        };

        function CheckError(txtSubmit, identity) {

            if (identity == 1) {
                document.getElementById('<%=lnkBtnSubmit.ClientID %>').click();
            }
            else {
                document.getElementById('<%=lnkBtnAprove.ClientID %>').click();

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

            // if section is already open, return false
            if ($(h2).is('.active')) {
                $(h2).removeClass("active");
                $(h2).next('div').slideUp('fast');
                $(h2).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
                return false;
            }
            else {
                $(h2).addClass("active");
            }

            // open request and close open
            $(h2).parent().parent().find('.accordion > div').slideUp('fast');
            $(h2).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
            $(h2).next('div').slideToggle('fast');

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
              url: "CreateCustomIEP.aspx/SetIEPId",
              data: "{'ID':'" + IEPID + "'}",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (data) { },
              error: function (request, status, error) {
                  alert("Error");
              }
          });



        }
        function FindIEPPAge(Status, IEPID, IEPURL) {
            if (Status == "Approve") {
                document.getElementById('btnapprove').style.display = "none";
                document.getElementById('btnsubmit').style.display = "none";
            }
            else {
                var permission = document.getElementById('<%=(hfCheckError).ClientID %>').value;
                if (permission == "true") {
                    document.getElementById('btnapprove').style.display = "block";
                    document.getElementById('btnsubmit').style.display = "none";
                }
                else {
                    document.getElementById('btnapprove').style.display = "none";
                    document.getElementById('btnsubmit').style.display = "none";
                }
            }
            SetIepID(IEPID);
            $('#ifrmContent').attr('src', IEPURL);
            document.getElementById('<%=(hdnFieldIep).ClientID %>').value = IEPID;
        }
        function CreateIEPClick() {
            //document.getElementById('btnapprove').style.display = "none";
            //document.getElementById('btnsubmit').style.display = "block";
            //SetIepID(document.getElementById('<%//=(hdnFieldIep).ClientID %>').value);
            //document.getElementById("<%//=tdMsgMain.ClientID%>").innerHTML = "";
            return CreateIEP1();
        }
        function CreateIEP1() {

            
            //var iep = document.getElementById('<%//=(hdnFieldIep).ClientID %>').value;

            //if (parseInt(iep) > 0) {*/
            $('#lnkIEP1').attr('class', 'grmb grimgbAck');

            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE1.aspx');
            //}
            //else {
            //    popupShow();
            //}
            //getIepUdateStatus();
            return false;
        }
        function CreateIEP2() {
            //document.getElementById('btnapprove').style.display = "none";
            //document.getElementById('btnsubmit').style.display = "block";
            //SetIepID(document.getElementById('<%//=(hdnFieldIep).ClientID %>').value);
            //document.getElementById("<%//=tdMsgMain.ClientID%>").innerHTML = "";

            //var iep = document.getElementById('<%//=(hdnFieldIep).ClientID %>').value;
            //if (parseInt(iep) > 0) {
                $('#lnkIEP1').attr('class', 'negative');

                $('#lnkIEP2').attr('class', 'grmb grimgbAck');

                $('#lnkIEP3').attr('class', 'negative');
                $('#lnkIEP4').attr('class', 'negative');
                $('#lnkIEP5').attr('class', 'negative');
                $('#lnkIEP6').attr('class', 'negative');
                $('#lnkIEP7').attr('class', 'negative');
                $('#lnkIEP8').attr('class', 'negative');
                $('#lnkIEP9').attr('class', 'negative');
                $('#lnkIEP10').attr('class', 'negative');
                $('#lnkIEP11').attr('class', 'negative');
                $('#lnkIEP12').attr('class', 'negative');
                $('#lnkIEP13').attr('class', 'negative');
                $('#lnkIEP14').attr('class', 'negative');
                $('#lnkIEP15').attr('class', 'negative');

                $('#ifrmContent').attr('src', 'CreateIEP-PE2.aspx');
            //}
            //else {
            //    popupShow();
            //}
            //getIepUdateStatus();
            return false;
        }
        function CreateIEP3() {
           // document.getElementById('btnapprove').style.display = "none";
           // document.getElementById('btnsubmit').style.display = "block";
           // SetIepID(document.getElementById('<%//=(hdnFieldIep).ClientID %>').value);
            //document.getElementById("<%//=tdMsgMain.ClientID%>").innerHTML = "";
           // var iep = document.getElementById('<%//=(hdnFieldIep).ClientID %>').value;
           // if (parseInt(iep) > 0) {
        

                $('#lnkIEP1').attr('class', 'negative');
                $('#lnkIEP2').attr('class', 'negative');

                $('#lnkIEP3').attr('class', 'grmb grimgbAck');

                $('#lnkIEP4').attr('class', 'negative');
                $('#lnkIEP5').attr('class', 'negative');
                $('#lnkIEP6').attr('class', 'negative');
                $('#lnkIEP7').attr('class', 'negative');
                $('#lnkIEP8').attr('class', 'negative');
                $('#lnkIEP9').attr('class', 'negative');
                $('#lnkIEP10').attr('class', 'negative');
                $('#lnkIEP11').attr('class', 'negative');
                $('#lnkIEP12').attr('class', 'negative');
                $('#lnkIEP13').attr('class', 'negative');
                $('#lnkIEP14').attr('class', 'negative');
                $('#lnkIEP15').attr('class', 'negative');

                $('#ifrmContent').attr('src', 'CreateIEP-PE3.aspx');
           // }
            //else {
            //    popupShow();
            //}
            //getIepUdateStatus();
            return false;
        }
        function CreateIEP4() {
            //document.getElementById('btnapprove').style.display = "none";
            //document.getElementById('btnsubmit').style.display = "block";
            //SetIepID(document.getElementById('<%//=(hdnFieldIep).ClientID %>').value);
            //document.getElementById("<%//=tdMsgMain.ClientID%>").innerHTML = "";
            //var iep = document.getElementById('<%//=(hdnFieldIep).ClientID %>').value;
            //if (parseInt(iep) > 0) {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');

            $('#lnkIEP4').attr('class', 'grmb grimgbAck');

            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE4.aspx');
            //}
            //else {
            //    popupShow();
            //}
            //getIepUdateStatus();
            return false;
        }
        function CreateIEP5() {
            //document.getElementById('btnapprove').style.display = "none";
            //document.getElementById('btnsubmit').style.display = "block";
            //SetIepID(document.getElementById('<%//=(hdnFieldIep).ClientID %>').value);
            //document.getElementById("<%//=tdMsgMain.ClientID%>").innerHTML = "";
            //var iep = document.getElementById('<%//=(hdnFieldIep).ClientID %>').value;
            //if (parseInt(iep) > 0) {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');

            $('#lnkIEP5').attr('class', 'grmb grimgbAck');

            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE5.aspx');
            //}
            //else {
            //    popupShow();
            //}
            //getIepUdateStatus();
            return false;
        }
        function CreateIEP6() {
            //document.getElementById('btnapprove').style.display = "none";
            //document.getElementById('btnsubmit').style.display = "block";
            //SetIepID(document.getElementById('<%//=(hdnFieldIep).ClientID %>').value);
            //document.getElementById("<%//=tdMsgMain.ClientID%>").innerHTML = "";
            //var iep = document.getElementById('<%//=(hdnFieldIep).ClientID %>').value;
            //if (parseInt(iep) > 0) {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');

            $('#lnkIEP6').attr('class', 'grmb grimgbAck');

            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE6.aspx');
            //}
            //else {
            //    popupShow();
            //}
            //getIepUdateStatus();
            return false;
        }
        function CreateIEP7() {
            //document.getElementById('btnapprove').style.display = "none";
            //document.getElementById('btnsubmit').style.display = "block";
            //SetIepID(document.getElementById('<%//=(hdnFieldIep).ClientID %>').value);
          //  document.getElementById("<%//=tdMsgMain.ClientID%>").innerHTML = "";
          //  var iep = document.getElementById('<%//=(hdnFieldIep).ClientID %>').value;
         //   if (parseInt(iep) > 0) {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');

            $('#lnkIEP7').attr('class', 'grmb grimgbAck');

            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE7.aspx');
            //}
            //else {
            //    popupShow();
            //}
            //getIepUdateStatus();
            return false;
        }
        function CreateIEP8() {
         //   document.getElementById('btnapprove').style.display = "none";
           // document.getElementById('btnsubmit').style.display = "block";
         //   SetIepID(document.getElementById('<%//=(hdnFieldIep).ClientID %>').value);
         //   document.getElementById("<%//=tdMsgMain.ClientID%>").innerHTML = "";
          //  var iep = document.getElementById('<%//=(hdnFieldIep).ClientID %>').value;
          //  if (parseInt(iep) > 0) {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');

            $('#lnkIEP8').attr('class', 'grmb grimgbAck');

            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE8.aspx');
            //}
            //else {
            //    popupShow();
            //}
            //getIepUdateStatus();
            return false;
        }
        function CreateIEP9() {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');

            $('#lnkIEP9').attr('class', 'grmb grimgbAck');

            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE9.aspx');
            return false;
        }
        function CreateIEP10() {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');

            $('#lnkIEP10').attr('class', 'grmb grimgbAck');

            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE10.aspx');
            return false;
        }
        function CreateIEP11() {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');

            $('#lnkIEP11').attr('class', 'grmb grimgbAck');

            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE11.aspx');
            return false;
        }
        function CreateIEP12() {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');

            $('#lnkIEP12').attr('class', 'grmb grimgbAck');

            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE12.aspx');
            return false;
        }
        function CreateIEP13() {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');

            $('#lnkIEP13').attr('class', 'grmb grimgbAck');

            $('#lnkIEP14').attr('class', 'negative');
            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE13.aspx');
            return false;
        }
        function CreateIEP14() {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');

            $('#lnkIEP14').attr('class', 'grmb grimgbAck');

            $('#lnkIEP15').attr('class', 'negative');

            $('#ifrmContent').attr('src', 'CreateIEP-PE14.aspx');
            return false;
        }
        function CreateIEP15() {
            $('#lnkIEP1').attr('class', 'negative');
            $('#lnkIEP2').attr('class', 'negative');
            $('#lnkIEP3').attr('class', 'negative');
            $('#lnkIEP4').attr('class', 'negative');
            $('#lnkIEP5').attr('class', 'negative');
            $('#lnkIEP6').attr('class', 'negative');
            $('#lnkIEP7').attr('class', 'negative');
            $('#lnkIEP8').attr('class', 'negative');
            $('#lnkIEP9').attr('class', 'negative');
            $('#lnkIEP10').attr('class', 'negative');
            $('#lnkIEP11').attr('class', 'negative');
            $('#lnkIEP12').attr('class', 'negative');
            $('#lnkIEP13').attr('class', 'negative');
            $('#lnkIEP14').attr('class', 'negative');

            $('#lnkIEP15').attr('class', 'grmb grimgbAck');

            $('#ifrmContent').attr('src', 'CreateIEP-PE15.aspx');
            return false;
        }
        function resizeIframe(obj) {
            obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';


        }
        function popupShow() {
            $('#overlay').fadeIn('fast', function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#close_x').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); });
        }

        function getIepUdateStatus() {
            //start progress here , should keep the de reff of wich progress is started
            var iep = document.getElementById('hdnFieldIep').value;
            $.ajax({
                url: "CreateCustomIEP.aspx/IepUpdateStatus",
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
            $('.loading').show();
            $('#fullContents').hide();

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
                showWait();
                return true;
            }
            else {
                alert("Please Select IEP before Export");
                return false;
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
    </script>

    <style>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
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
            <table style="width: 100%;">
                <tr>
                    <td style="width: 80%">
                        <img runat="server" alt="Please Wait..." id="PlzWait" src="../Administration/images/PleaseWait.gif" style="width: 40px; height: 40px; display: none" />
                    </td>
                    <td></td>
                </tr>

            </table>
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
                                        <h2>2 <span style="width:60%">Customize IEP</span></h2>
                                    </td>
                                    <td style="width: 5%">
                                        
                                    </td>
                                    <td style="width: 5%">
                                        
                                         <input id="btnsubmit" runat="server" type="button" value="Submit" class="NFButton" onclick="javascript: CheckError(this, 1);" style="display: none" />
                                        <input id="btnapprove" runat="server" type="button" value="Approve" class="NFButton" onclick="javascript: CheckError(this, 2); " style="display: none;" /> 
                                    </td>
                                    <td style="width: 5%;text-align:right;">
                                       
                                        <asp:Button ID="btnNewIEP" runat="server" CssClass="NFButton" Text="Create New IEP" Width="175px" OnClick="btnNewIEP_Click"></asp:Button>
                                    </td>
                                    <td style="width: 1%">
                                        
                                        <asp:Button ID="btnIEPExport" runat="server" CssClass="NFButton" Text="Export" OnClick="btnIEPExport_Click" OnClientClick="return checkPostbackExport();"></asp:Button>
                                    </td>
                                    
                                    <td style="width: 3%">
                                        <asp:ImageButton ID="btnRefresh" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" Style="float: right;" />

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
                                <div style="width:100%;overflow-y:auto;height:500px">
                                <asp:HiddenField ID="hdnFieldIep" runat="server" />
                                <asp:LinkButton ID="lnkBtnIEPYear" CssClass="grmb BlueBtnType" runat="server" Text=" " OnClientClick="return false;"></asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP1" runat="server" OnClientClick="return CreateIEPClick();" CssClass="negative">IEP1</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP2" runat="server" OnClientClick="return CreateIEP2();" CssClass="negative">IEP2</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP3" runat="server" OnClientClick="return CreateIEP3();" CssClass="negative">IEP3</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP4" runat="server" OnClientClick="return CreateIEP4();" CssClass="negative">IEP4</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP5" runat="server" OnClientClick="return CreateIEP5();" CssClass="negative">IEP5</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP6" runat="server" OnClientClick="return CreateIEP6();" CssClass="negative">IEP6</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP7" runat="server" OnClientClick="return CreateIEP7();" CssClass="negative">IEP7</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP8" runat="server" OnClientClick="return CreateIEP8();" CssClass="negative">IEP8</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP9" runat="server" OnClientClick="return CreateIEP9();" CssClass="negative">IEP9</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP10" runat="server" OnClientClick="return CreateIEP10();" CssClass="negative">IEP10</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP11" runat="server" OnClientClick="return CreateIEP11();" CssClass="negative">IEP11</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP12" runat="server" OnClientClick="return CreateIEP12();" CssClass="negative">IEP12</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP13" runat="server" OnClientClick="return CreateIEP13();" CssClass="negative">IEP13</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP14" runat="server" OnClientClick="return CreateIEP14();" CssClass="negative">IEP14</asp:LinkButton>
                                <asp:LinkButton ID="lnkIEP15" runat="server" OnClientClick="return CreateIEP15();" CssClass="negative">IEP15</asp:LinkButton>
                                    </div>
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

        <div id="overlay" class="web_dialog_overlay"></div>

        <div id="dialog" class="web_dialog" style="width: 900px; margin-left: -22%;display:none;">

            <div id="sign_up5" style="width: 100%; margin-left: 10px">
                <table style="width: 100%">
                    <tr>
                        <td runat="server" id="tdMessage" class="tdText" colspan="5"></td>
                    </tr>


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
                        <td class="tdText" style="text-align: center" colspan="6"></td>

                    </tr>
                    <tr>
                        <td class="tdText" style="text-align: center" colspan="6">
                            <asp:Button ID="btnGenIED" runat="server" Text="Generate IEP" OnClick="btnGenIED_Click" CssClass="NFButton" />
                            <input type="button" id="CancalGen" class="NFButton" value="Cancel" />
                        </td>
                    </tr>

                </table>
            </div>
        </div>



        <div id="DilogAprove" class="web_dialog" style="width: 710px; top: -20%;display:none;">

            <div id="Div3" style="width: 700px; margin-top: 16px">

                <a id="A1" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
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
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="NFButton" OnClick="btnAdd_Click" />
                            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="NFButton" OnClick="btnReject_Click" />
                        </td>
                    </tr>
                </table>

                <asp:Panel ID="recentnote" runat="server" Visible="false">
                    <h3 style="color: White">Recent Notes</h3>
                    <table>
                        <tr>
                            <asp:DataList ID="Dlnotes" runat="server" Width="100%" BackColor="Silver">
                                <ItemTemplate>
                                    <table width="100%" align="left">
                                        <tr>

                                            <td width="100%">
                                                <asp:Label ID="lbl_date" runat="server" Text='<%# Eval("RecentDate") %>' ForeColor="#FF3300"></asp:Label>:
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("RecentNote") %>' ForeColor="#333300"></asp:Label>
                                            </td>

                                        </tr>
                                    </table>
                                    <br>
                                    <br>
                                </ItemTemplate>

                            </asp:DataList>
                        </tr>
                    </table>


                </asp:Panel>


            </div>
        </div>



        <div id="PopDownload" class="web_dialog" style="width: 600px; top: -20%;display:none;">

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

                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" />

                        </td>
                        <td style="text-align: left">

                            <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" OnClick="btnDone_Click" />

                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <div id="FreetextPopup" class="web_dialog" style="display:none">

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
</body>
</html>
