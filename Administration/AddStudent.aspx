<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="AddStudent.aspx.cs" Inherits="Admin_AddStudent" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">

    <link href="CSS/AdminTabletHome.css" rel="stylesheet" id="sized" />
    <script src="JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="JS/jsDatePick.min.1.3.js"></script>
    <script src="JS/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="JS/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="JS/jquery.fileupload.js" type="text/javascript"></script>
    <script src="JS/jquery.iframe-transport.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="jsDatePick_ltr.min.css" />
    <script type="text/javascript" src="jsDatePick.min.1.3.js"></script>
    <style type="text/css">
        .web_dialog2 {
            display: none;
            position: fixed;
            min-width: 390px;
            min-height: 200px;
            left: 48%;
            margin-left: -162px;
            margin-top: 15px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 1001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
            top: 40%;
        }
    </style>


    <script type="text/javascript">

        function HidePopup() {
            $("#divPrmpts").hide();
        }
        function ShowPopup() {
            $("#divPrmpts").show();
        }
        function adjustStyle(width) {
            width = parseInt(width);

            if (width >= 1200) {
                $("#sized").attr("href", "CSS/AdminTabletHome.css");
                return;
            }
            if (width < 1200) {
                $("#sized").attr("href", "CSS/AdminTablet.css");
                $('#slider').css("top", '73%');
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
                target: '<%=(txtDOB).ClientID %>',
                dateFormat: "%m-%d-%Y"
            });
            new JsDatePick({
                useMode: 2,
                target: '<%=(txtJoinDt).ClientID %>',
                dateFormat: "%m-%d-%Y"
            });
            new JsDatePick({
                useMode: 2,
                target: '<%=(txtDateofLast).ClientID %>',
                dateFormat: "%m-%d-%Y"
            });
        };
    </script>

    <script lang="javascript" type="text/javascript">
        function validate() {
            if (document.getElementById("<%=txtStudentId.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Student Number Field can not be blank</div> ";
                document.getElementById("<%=txtStudentId.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtSASID.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>SASID Field can not be blank</div> ";
                document.getElementById("<%=txtSASID.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtFirstName.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>First Name Field can not be blank</div> ";
                document.getElementById("<%=txtFirstName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtLastName.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Last Name Field can not be blank</div> ";
                document.getElementById("<%=txtLastName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDOB.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Date of Birth Field can not be blank</div> ";
                document.getElementById("<%=txtDOB.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtJoinDt.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Admission Date Field can not be blank</div> ";
                document.getElementById("<%=txtJoinDt.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlGender.ClientID%>").selectedIndex == 0) {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Select Gender</div> ";
                document.getElementById("<%=ddlGender.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtGrade.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Grade Field can not be blank</div> ";
                document.getElementById("<%=txtGrade.ClientID%>").focus();
                return false;
            }


            <%-- if (document.getElementById("<%=ddlResident.ClientID%>").selectedIndex == ) {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Select Residence</div> ";
                document.getElementById("<%=ddlResident.ClientID%>").focus();
                return false;--%>
            //}

            // if (document.getElementById("<%=ddlCountry.ClientID%>").selectedIndex == 0) {
            //      document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Select Country</div> ";
            //      document.getElementById("<%=ddlCountry.ClientID%>").focus();
            //      return false;
            //  }
            //  if (document.getElementById("<%=ddlState.ClientID%>").selectedIndex == 0) {
            //      document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Select State</div> ";
            //      document.getElementById("<%=ddlState.ClientID%>").focus();
            //      return false;
            //  }

            if (document.getElementById("<%=txtDistrictCode.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>District Code can not be blank</div> ";
                document.getElementById("<%=txtDistrictCode.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=txtEmail.ClientID%>").value != "") {
                var emailPat = /^(\".*\"|[A-Za-z]\w*)@(\[\d{1,3}(\.\d{1,3}){3}]|[A-Za-z]\w*(\.[A-Za-z]\w*)+)$/;
                var emailid = document.getElementById("<%=txtEmail.ClientID %>").value;
                var matchArray = emailid.match(emailPat);
                if (matchArray == null) {
                    document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Your email address seems incorrect. Please try again.</div> ";
                    document.getElementById("<%=txtEmail.ClientID %>").focus();
                    return false;
                }
            }


            return true;
        }
    </script>

    <style type="text/css">
        .ui-datepicker {
            font-size: 8pt !important;
        }
    </style>
    <style type="text/css">
        .style2 {
            width: 1%;
        }

        .style3 {
            width: 6px;
        }

        .style4 {
            width: 6px;
            color: #FF0000;
            width: 150px!important;
            float: left!important;
        }

        .style5 {
            color: #FF0000;
        }

        .style6 {
            color: #fff;
        }

        .rowHeigt {
            height: 36px;
        }

        .validate[required] {
        }



        #content {
            z-index: 0;
            position: relative;
            top: 0px;
            left: 0px;
            width: 63%;
        }

        .tdTextSt {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            font-size: 13px;
        }
    </style>
    <style type="text/css">
        .FileUpload {
            border: 1px solid black;
            padding: 2px;
            background-color: #E0E0E0;
            border-radius: 5px;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            text-align: left;
        }
    </style>

    <style type="text/css">
        #dvPreview {
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=image);
            min-height: 156px;
            min-width: 180px;
            display: none;
        }
    </style>







    <script type="text/javascript">

        function getExt(fileName) {

            var ext = fileName.split('.').pop();
            return ext;
        }

        function assignDOBDate(imgFile) {
            var newPreview = document.getElementById("newPreview");
            newPreview.innerHTML = "<img src='" + imgFile + "'/>";
            //newPreview.style.width = "151px";
            //newPreview.style.height = "151px";
        }

        function changePic(el) {
            var cvr = document.getElementById("cover")
            var dlg = document.getElementById(el)
            // cvr.style.display = "block"
            dlg.style.display = "block"

        }


        function closePic(el) {

            var cvr = document.getElementById("cover")
            var dlg = document.getElementById(el)
            // cvr.style.display = "none"
            dlg.style.display = "none"
        }

        function DetectBrowser(imgFile) {
            var val = navigator.userAgent.toLowerCase();

            if (val.indexOf("firefox") > -1) {
                isFF = true;
            }
            else if (val.indexOf("opera") > -1) {
                isOP = true;
            }
            else if (val.indexOf("msie") > -1) {
                isIE = true;
                // alert(imgFile.value);
                //PreviewImg(imgFile);
            }
            else if (val.indexOf("safari") > -1) {
                isIE = true;
            }

            // var cvr = document.getElementById("cover")
            var dlg = document.getElementById("prevImg")
            // cvr.style.display = "none"
            dlg.style.display = "none"
        }


        function PreviewImg(imgFile) {
            var newPreview = document.getElementById("newPreview");
            newPreview.innerHTML = "";
            newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
            newPreview.style.width = "164px";
            newPreview.style.height = "280px";
        }

        function setImg(imgFile) {
            //$('').attr('src', $('#blah').attr('src'));
            var newPreview = document.getElementById("newPreview");
            newPreview.innerHTML = "";
            newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
            alert(imgFile.value);
            newPreview.style.width = "164px";
            newPreview.style.height = "280px";
        }

        function changeImg(valueTemp) {
            document.getElementById("<%=img.ClientID %>").src = valueTemp;
        }




    </script>

    <style type="text/css">
        #prevImage {
            border: 8px solid #ccc;
            width: 300px;
            height: 200px;
        }

        .auto-style6 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            line-height: 12px;
            height: 10px;
            width: 18%;
        }

        p.MColumnHead {
            margin-bottom: .0001pt;
            page-break-after: avoid;
            punctuation-wrap: simple;
            text-autospace: none;
            font-size: 10.0pt;
            font-family: "Arial","sans-serif";
            font-weight: bold;
            margin-left: 0cm;
            margin-right: 0cm;
            margin-top: 0cm;
        }

        .auto-style12 {
        }

        .auto-style13 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            line-height: 12px;
            height: 10px;
            width: 14%;
        }

        .auto-style17 {
            width: 18%;
        }

        .auto-style18 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            line-height: 12px;
            height: 10px;
            width: 20%;
        }

        .auto-style23 {
            width: 4%;
        }

        .auto-style24 {
        }

        .auto-style26 {
            color: #FF0000;
        }

        .auto-style27 {
            color: #FF0000;
        }

        .auto-style28 {
            width: 384px;
        }

        .auto-style35 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            line-height: 12px;
            height: 10px;
        }

        .auto-style36 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            line-height: 12px;
            height: 10px;
            width: 1%;
        }

        .auto-style37 {
            width: 19%;
        }

        .auto-style42 {
            width: 26%;
        }

        .auto-style43 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            line-height: 12px;
            height: 10px;
            width: 29%;
        }

        .auto-style44 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            line-height: 12px;
            height: 10px;
            width: 26%;
        }

        .previewClass {
            border: 3px solid #b2ccca;
            display: block;
            font: 10pt tahoma;
            height: 152px;
            position: absolute;
            width: 161px;
            z-index: 600;
        }

        .auto-style45 {
            width: 100%;
            float: left;
        }

        .astrix {
            color: red;
            float: right;
        }

        .addStd table tr td {
            width: 25%;
            padding: 3px;
        }

        input[type=text] {
            width: 178px;
        }

        .drpClass {
            width: 195px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PageContent" runat="Server">
    <div style="display: none;">
        <table id="content" style="width: 100%">
        </table>





        <table style="width: 100%;">

            <tr>
                <td colspan="10" class="head_box"></td>
            </tr>
            <tr>
                <td colspan="10" class=""></td>
            </tr>
            <tr>
                <td colspan="10" class="">
                    <br />
                </td>
            </tr>
            <tr>
                <td class="tdText" colspan="2">Grade </td>
                <td style="text-align: right; color: red;" class="auto-style28">*</td>
                <td colspan="3">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">Residence</td>
                <td style="color: red;">*</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText" colspan="2">Citizenship</td>
                <td class="auto-style28">&nbsp;</td>
                <td colspan="3">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">Guardianship Status</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText" colspan="2">Primary Language</td>
                <td class="auto-style28">&nbsp;</td>
                <td colspan="3">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">Gender </td>
                <td style="color: red;">*</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText" colspan="2">Race</td>
                <td class="auto-style28">&nbsp;</td>
                <td colspan="3">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">Height</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText" colspan="2">Weight</td>
                <td class="auto-style28">&nbsp;</td>
                <td colspan="3">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">Hair Color</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText" colspan="2">Eye Color</td>
                <td class="auto-style28">&nbsp;</td>
                <td colspan="3">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">Distinguishing Marks </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText" colspan="2">Case Manager Residential</td>
                <td class="auto-style28">&nbsp;</td>
                <td colspan="3">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">Case Manager Educational</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText" colspan="2">Legal Competency Status</td>
                <td class="auto-style28">&nbsp;</td>
                <td colspan="3">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">Marital Status of Both Parents</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText" colspan="3">Other State Agencies Involved With Student</td>
                <td colspan="3">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>




            <%--hari--%>


            <tr>
                <td align="right" class="auto-style13" valign="top" colspan="2">Class
                </td>

                <td align="right" class="auto-style9" style="color: #FF0000" valign="top">
                    <span style="color: red"></span>
                </td>
                <td colspan="5" align="left">
                    <table>
                        <tr>
                            <td width="100%">&nbsp;</td>
                        </tr>
                    </table>
                </td>

            </tr>

            <%--hari end--%>





            <tr>
                <td class="tdText" colspan="2">&nbsp;</td>
                <td class="auto-style28">&nbsp;</td>
                <td colspan="3">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="head_box" colspan="10">Address Information
                        <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
                <td style="text-align: right;" class="auto-style28">&nbsp;</td>
                <td style="text-align: left;" colspan="3">&nbsp;</td>

                <td class="auto-style24">&nbsp;</td>
                <td style="text-align: right;" class="auto-style42">&nbsp;</td>
                <td style="text-align: left;">&nbsp;</td>
            </tr>


            <tr>
                <td class="tdText" colspan="2">Address Line 1</td>
                <td style="text-align: left;" class="auto-style27" colspan="4">&nbsp; &nbsp;</td>
                <td style="text-align: left;">&nbsp;</td>
            </tr>


            <tr>
                <td class="auto-style18" colspan="2">Address Line 2</td>
                <td style="text-align: left;" class="auto-style12" colspan="4">&nbsp; &nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
                <td style="text-align: right;">&nbsp;</td>
                <td style="text-align: left;">&nbsp;</td>
            </tr>


            <tr>
                <td class="tdText" colspan="2">Address Line 3</td>
                <td style="text-align: left;" class="auto-style26" colspan="4">&nbsp; &nbsp;</td>

                <td class="auto-style24"></td>

                <td class="auto-style44">&nbsp;</td>
                <td style="text-align: right;" class="style5">&nbsp;</td>
                <td style="width: 30%; text-align: left;">&nbsp;</td>
            </tr>

            <tr>
                <td class="auto-style35" colspan="2">Country</td>
                <td class="auto-style27" style="text-align: left;">&nbsp;</td>
                <td style="text-align: left;" class="auto-style23">State</td>
                <td style="text-align: left; color: red" class="style2"></td>
                <td style="text-align: left;" class="auto-style37">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
                <td class="style5" style="text-align: right;">&nbsp;</td>
                <td style="width: 30%; text-align: left;">&nbsp;</td>
            </tr>

            <tr>
                <td class="auto-style35">City</td>
                <td class="auto-style36">&nbsp;</td>
                <td class="auto-style27" style="text-align: center;">&nbsp;</td>
                <td class="auto-style23" style="text-align: left;">Zip </td>
                <td class="style2" style="text-align: left; color: red"></td>
                <td style="text-align: left;" class="auto-style37">&nbsp;</td>
                <td class="auto-style24">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
                <td class="style5" style="text-align: right;">&nbsp;</td>
                <td style="width: 30%; text-align: left;">&nbsp;</td>
            </tr>

            <tr>
                <td class="tdText" colspan="2">&nbsp;</td>
                <td style="text-align: right;" class="auto-style27">&nbsp;</td>
                <td style="text-align: left;" colspan="3">&nbsp;</td>

                <td class="auto-style24"></td>

                <td class="auto-style44">&nbsp;</td>
                <td style="text-align: right;" class="style5">&nbsp;</td>
                <td style="width: 30%; text-align: left;">&nbsp;</td>
            </tr>



            <tr>
                <td class="tdText" colspan="2">&nbsp;</td>
                <td class="auto-style27" style="text-align: right;">&nbsp;</td>
                <td style="text-align: left;" colspan="3">&nbsp;</td>
                <td class="auto-style24"></td>
                <td class="auto-style44">&nbsp;</td>
                <td class="style5" style="text-align: right;">&nbsp;</td>
                <td style="width: 30%; text-align: left;">&nbsp;</td>
            </tr>
        </table>


        <table>
            <tr>

                <td colspan="7" class="head_box">Medical and Insurance</td>
            </tr>

            <tr>
                <td class="tdText">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td class="auto-style13">&nbsp;</td>
                <td class="auto-style17">&nbsp;</td>
                <td></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText">Date of Last Physical Exam</td>
                <td></td>
                <td>&nbsp;</td>
                <td class="auto-style12"></td>
                <td class="auto-style18">Medical Conditions/Diagnosis</td>
                <td></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText">Allergies</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td class="auto-style12"></td>
                <td class="auto-style18">Current Medications</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText">Self Preservation Ability</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td class="auto-style12"></td>
                <td class="auto-style18">Significant Behavior Characteristics</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText">Relevant Capabilities<br />


                </td>
                <td>&nbsp;</td>
                <td class="tdText">&nbsp;</td>
                <td class="auto-style12"></td>
                <td class="auto-style18">Relevant Limitations</td>
                <td class="tdText">
                    <br />


                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText">Relevant Preferences</td>
                <td>&nbsp;</td>
                <td class="tdText">&nbsp;</td>
                <td class="auto-style12"></td>
                <td class="auto-style18">&nbsp;</td>
                <td class="tdText">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="tdText">&nbsp;</td>
                <td>&nbsp;</td>
                <td class="tdText">&nbsp;</td>
                <td class="auto-style12"></td>
                <td class="auto-style18">&nbsp;</td>
                <td class="tdText">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="head_box" colspan="7">District Information
                        <br />
                </td>
            </tr>

            <tr>
                <td style="width: 20%;" class="tdText">&nbsp;</td>
                <td style="width: 5%; text-align: right;" class="style5">&nbsp;</td>
                <td style="width: 20%; text-align: left;">&nbsp;</td>

                <td class="auto-style13">&nbsp;</td>
                <td style="text-align: left;" class="auto-style17">&nbsp;</td>
                <td></td>
                <td style="width: 30%; text-align: left;">&nbsp;</td>
            </tr>

            <tr>
                <td class="tdText" style="width: 20%;">District Code</td>

                <td class="style5" style="width: 5%; text-align: right;">*</td>

                <td style="width: 20%; text-align: left;">&nbsp;</td>
                <td class="auto-style12"></td>
                <td class="auto-style18">District Type</td>
                <td style="text-align: left;"></td>
                <td style="width: 30%; text-align: left;">&nbsp;</td>
            </tr>

            <tr>
                <td style="width: 20%;" class="tdText">District Function</td>
                <td style="width: 5%; text-align: left;"></td>
                <td style="width: 20%; text-align: left;">&nbsp;</td>

                <td class="auto-style12"></td>

                <td class="auto-style18">Contact Person</td>
                <td style="text-align: left;"></td>
                <td style="width: 30%; text-align: left;">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: left;"></td>
                <td style="width: 5%; text-align: left;"></td>
                <td style="width: 20%; text-align: left;"></td>

                <td style="text-align: right;" class="auto-style12"></td>
                <td style="text-align: left;" class="auto-style17"></td>
                <td></td>
                <td style="width: 30%; text-align: left;"></td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: left;"></td>
                <td style="width: 5%; text-align: left;"></td>
                <td style="width: 20%; text-align: left;"></td>

                <td style="text-align: right;" class="auto-style12"></td>
                <td style="text-align: left;" class="auto-style17"></td>
                <td></td>
                <td style="width: 30%; text-align: left;"></td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: left;"></td>
                <td style="width: 5%; text-align: left;"></td>
                <td style="width: 20%; text-align: left;"></td>

                <td style="text-align: right;" class="auto-style12"></td>
                <td style="text-align: left;" class="auto-style17"></td>
                <td></td>
                <td style="width: 30%; text-align: left;"></td>
            </tr>
        </table>







    </div>


    <div class="addStd" style="width: 900px; margin: auto; border: 1px solid #fff;">

        <table class="auto-style45">
            <tr>
                <td runat="server" id="tdMsg" style="color: #FF0000" colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td>Student Number<span class="astrix">*</span></td>
                <td>
                    <asp:TextBox ID="txtStudentId" runat="server" CssClass="textClass"
                        MaxLength="10" TabIndex="1"></asp:TextBox>
                </td>
                <td colspan="2" rowspan="7">

                    <div id="imageContainer" style="margin-left: 0px !important;">
                        <%--<div id="prevImg" class="previewClass" style="border: none">--%>

                        <div id="newPreview"></div>
                        <asp:Image runat="server" ID="img" Style="height: 50px; width: 50px;" Visible="false" />
                        <%--<div id="dvPreview" style="height: 50px; width: 50px;"></div>--%>

                        <%--</div>--%>
                    </div>
                    <div>
                        <asp:FileUpload ID="fileUpl_stdPhoto" CssClass="fileupload" runat="server" />
                    </div>


                </td>
            </tr>
            <tr>
                <td>SASID<span class="astrix">*</span></td>
                <td>
                    <asp:TextBox ID="txtSASID" runat="server" CssClass="textClass"
                        MaxLength="10" TabIndex="2"></asp:TextBox>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="drpClass" TabIndex="7">
                        <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Inactive" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>First Name<span class="astrix">*</span></td>
                <td>
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="textClass" TabIndex="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Last Name<span class="astrix">*</span></td>
                <td>
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="textClass" TabIndex="4"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>DOB<span class="astrix">*</span></td>
                <td>
                    <asp:TextBox ID="txtDOB" runat="server" onkeypress="return false;" TabIndex="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Admission Date<span class="astrix">*</span></td>
                <td>
                    <asp:TextBox ID="txtJoinDt" runat="server" onkeypress="return false;" TabIndex="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblstaus" runat="server" Text="Status"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblmstatus" runat="server" Text="*" ForeColor="#FF3300"></asp:Label></td>
            </tr>
            <tr>
                <td>Grade<span class="astrix">*</span></td>
                <td>
                    <asp:TextBox ID="txtGrade" runat="server" CssClass="textClass" TabIndex="8"></asp:TextBox>
                </td>
                <td>Residence<span class="astrix">*</span></td>
                <td>
                    <asp:DropDownList ID="ddlResident" runat="server" CssClass="drpClass" TabIndex="9" OnSelectedIndexChanged="ddlResident_SelectedIndexChanged" AutoPostBack="true">
                        <%-- <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>--%>
                        <asp:ListItem Text="Day" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Residential" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Citizenship</td>
                <td>
                    <asp:TextBox ID="txtCitizenShip" runat="server" CssClass="textClass" TabIndex="10"></asp:TextBox>
                </td>
                <td>Guardianship Status<span class="astrix">*</span></td>
                <td>
                    <asp:TextBox ID="txtGuardianship" runat="server" CssClass="textClass" TabIndex="11"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Primary Language</td>
                <td>
                    <asp:TextBox ID="txtPrimaryLang" runat="server" CssClass="textClass" TabIndex="12"></asp:TextBox>
                </td>
                <td>Gender</td>
                <td>
                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="drpClass" TabIndex="13">
                        <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Race</td>
                <td>
                    <asp:TextBox ID="txtRace" runat="server" CssClass="textClass" MaxLength="10" TabIndex="14"></asp:TextBox>
                </td>
                <td>Height</td>
                <td>
                    <asp:TextBox ID="txtHeight" runat="server" CssClass="textClass" MaxLength="10" TabIndex="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Weight</td>
                <td>
                    <asp:TextBox ID="txtWeight" runat="server" CssClass="textClass" MaxLength="10" TabIndex="16"></asp:TextBox>
                </td>
                <td>Hair Color</td>
                <td>
                    <asp:TextBox ID="txtHairColor" runat="server" CssClass="textClass" MaxLength="10" TabIndex="17"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Eye Color</td>
                <td>
                    <asp:TextBox ID="txtEyeColor" runat="server" CssClass="textClass" MaxLength="10" TabIndex="18"></asp:TextBox>
                </td>
                <td>Distinguishing Marks</td>
                <td>
                    <asp:TextBox ID="txtDisMarks" runat="server" CssClass="textClass" MaxLength="10" TabIndex="19"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Case Manager Residential</td>
                <td>
                    <asp:TextBox ID="txtCaseManager" runat="server" CssClass="textClass" MaxLength="10" TabIndex="20"></asp:TextBox>
                </td>
                <td>Case Manager Educational</td>
                <td>
                    <asp:TextBox ID="txtCaseManEdu" runat="server" CssClass="textClass" MaxLength="10" TabIndex="21"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Legal Competency Status</td>
                <td>
                    <asp:TextBox ID="txtLegal" runat="server" CssClass="textClass" MaxLength="10" TabIndex="22"></asp:TextBox>
                </td>
                <td>Marital Status of Both Parents</td>
                <td>
                    <asp:TextBox ID="txtMaritulS" runat="server" CssClass="textClass" MaxLength="10" TabIndex="23"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Other State Agencies Involved With Student</td>
                <td>
                    <asp:TextBox ID="txtOtherStage" runat="server" CssClass="textClass" TabIndex="24"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td>Class</td>
                <td colspan="3">
                    <asp:UpdatePanel runat="server" ID="updatePanel2" UpdateMode="Conditional" OnLoad="ddlResident_SelectedIndexChanged">
                        <ContentTemplate>
                            <asp:DataList ID="DLclass" runat="server" RepeatDirection="Horizontal" AlternatingItemStyle-HorizontalAlign="Left" RepeatColumns="2" TabIndex="9">
                                <AlternatingItemStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td width="100%">
                                                <asp:CheckBox ID="chkClass" runat="server" Width="178px" Text='<%# Eval("Name") %>' Style="font-family: Arial,Helvetica,sans-serif; color: #676767; width: 230px;" />
                                                <asp:HiddenField ID="hdnClass" runat="server" Value='<%# Eval("Id") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlResident" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="head_box">Address Information</td>
                <td colspan="2" class="head_box">District Information </td>
            </tr>
            <tr>
                <td>Address Line 1</td>
                <td>
                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="textClass" OnTextChanged="txtAddress1_TextChanged" TabIndex="25"></asp:TextBox>
                </td>
                <td>District Code<span class="astrix">*</span></td>
                <td>
                    <asp:TextBox ID="txtDistrictCode" runat="server" CssClass="textClass" TabIndex="44"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Address Line 2</td>
                <td>
                    <asp:TextBox ID="txtAddress2" runat="server" CssClass="textClass" TabIndex="26"></asp:TextBox>
                </td>
                <td>District Function</td>
                <td>

                    <asp:TextBox ID="txtDistFunct" runat="server" CssClass="textClass" TabIndex="46"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td>Address Line 3</td>
                <td>
                    <asp:TextBox ID="txtAddress3" runat="server" CssClass="textClass" TabIndex="27"></asp:TextBox>
                </td>
                <td>District Type</td>
                <td>
                    <asp:TextBox ID="txtDistType" runat="server" CssClass="textClass" TabIndex="45"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Country</td>
                <td rowspan="2">
                    <asp:UpdatePanel ID="updateall" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" Style="margin-bottom: 5px;" CssClass="drpClass" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" TabIndex="28">
                            </asp:DropDownList>

                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="drpClass" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" TabIndex="29">
                                <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>Contact Person</td>
                <td>

                    <asp:TextBox ID="txtContact" runat="server" CssClass="textClass" TabIndex="47"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td>State</td>

                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>City</td>
                <td>
                    <asp:TextBox ID="txtCity" runat="server" CssClass="textClass" TabIndex="30"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Zip</td>
                <td>
                    <asp:TextBox ID="txtZip" runat="server" CssClass="textClass" MaxLength="5" TabIndex="31"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="4" class="head_box">Medical and Insurance</td>
            </tr>
            <tr>
                <td>Date of Last Physical Exam</td>
                <td>
                    <asp:TextBox ID="txtDateofLast" runat="server" CssClass="textClass" onkeypress="return false;" TabIndex="35"></asp:TextBox>
                </td>
                <td>Medical Conditions/Diagnosis</td>
                <td>
                    <asp:TextBox ID="txtMedicalCon" runat="server" CssClass="textClass" TabIndex="36"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Allergies</td>
                <td>
                    <asp:TextBox ID="txtAllergies" runat="server" CssClass="textClass" TabIndex="37"></asp:TextBox>
                </td>
                <td>Current Medications</td>
                <td>
                    <asp:TextBox ID="txtCurrentMed" runat="server" CssClass="textClass" TabIndex="38"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Self Preservation Ability</td>
                <td>
                    <asp:TextBox ID="txtSelfPres" runat="server" CssClass="textClass" TabIndex="39"></asp:TextBox>
                </td>
                <td>Significant Behavior Characteristics</td>
                <td>
                    <asp:TextBox ID="txtSignBehv" runat="server" CssClass="textClass" TabIndex="40"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Relevant Capabilities</td>
                <td>

                    <asp:TextBox ID="txtCapabilities" runat="server" CssClass="textClass" TabIndex="41"></asp:TextBox>


                </td>
                <td>Relevant Limitations</td>
                <td>
                    <asp:TextBox ID="txtLimitations" runat="server" CssClass="textClass" TabIndex="42"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Relevant Preferences</td>
                <td>
                    <asp:TextBox ID="txtPreferences" runat="server" CssClass="textClass" TabIndex="43"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
        </table>



        <table class="display">
            <tr>
                <td>
                    <div style="margin: auto; width: 100px;">
                        <asp:Button ID="Button_Add" runat="server" CssClass="NFButton" OnClick="Button_Add_Click" OnClientClick=" return validate()"
                            Text="Save" ValidationGroup="a" TabIndex="48" />
                        <div id="divPrmpts" class="web_dialog2">
                            <a id="closeHdr" onclick="HidePopup();" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                            <asp:HiddenField ID="HiddenField1" runat="server" />

                        </div>
                    </div>
                </td>
            </tr>


            <tr>
                <td style="display: none">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="textClass" TabIndex="34" Style="display: none"></asp:TextBox>
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="textClass" TabIndex="33" Style="display: none"></asp:TextBox>
                    <asp:TextBox ID="txtHomePhone" runat="server" CssClass="textClass" MaxLength="15" TabIndex="32" Style="display: none"></asp:TextBox>
                </td>

            </tr>


        </table>



    </div>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $(".fileupload").change(function () {
                $("#newPreview").html("");
                var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
                if (regex.test($(this).val().toLowerCase())) {
                    if ($.browser.msie && parseFloat(jQuery.browser.version) <= 9.0) {
                        $("#newPreview").show();
                        $("#newPreview")[0].filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = $(this).val();
                    }
                    else {
                        if (typeof (FileReader) != "undefined") {
                            $("#newPreview").show();
                            $("#newPreview").append("<img />");
                            var reader = new FileReader();
                            reader.onload = function (e) {
                                $("#newPreview img").attr("src", e.target.result);
                            }
                            reader.readAsDataURL($(this)[0].files[0]);
                        } else {
                            alert("This browser does not support FileReader.");
                        }
                    }
                } else {
                    alert("Please upload a valid image file.");
                }
            });
        });

    </script>
</asp:Content>
