<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="AddStudentOld.aspx.cs" Inherits="Admin_AddStudent" %>

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

    <script type="text/javascript">


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
           

            if (document.getElementById("<%=ddlResident.ClientID%>").selectedIndex == 0) {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Select Residence</div> ";
                document.getElementById("<%=ddlResident.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlCountry.ClientID%>").selectedIndex == 0) {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Select Country</div> ";
                document.getElementById("<%=ddlCountry.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlState.ClientID%>").selectedIndex == 0) {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Select State</div> ";
                document.getElementById("<%=ddlState.ClientID%>").focus();
                return false;
            }

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
    <script type="text/javascript">

          function getExt(fileName) {

              var ext = fileName.split('.').pop();
              return ext;
          }

          function assignDOBDate(imgFile) {          
              var newPreview = document.getElementById("newPreview");
              newPreview.innerHTML = "<img src='" + imgFile + "' width='150px;' height='150px;' />";
              newPreview.style.width = "151px";
              newPreview.style.height = "151px";
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
                  PreviewImg(imgFile);
              }
              else if (val.indexOf("safari") > -1) {
                  isIE = true;
              }

              var cvr = document.getElementById("cover")
              var dlg = document.getElementById("prevImg")
              cvr.style.display = "none"
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
              var newPreview = document.getElementById("newPreview");
              //newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile;           
              newPreview.style.width = "164px";
              newPreview.style.height = "280px";
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
    p.MColumnHead
	{margin-bottom:.0001pt;
	page-break-after:avoid;
	punctuation-wrap:simple;
	text-autospace:none;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";
	font-weight:bold;
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
            width: 15%;
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
        </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PageContent" runat="Server">
   
    <table id="content" style="width: 100%">
        <tr>
            <td runat="server" id="tdMsg" style="color: #FF0000" colspan="4">&nbsp;</td>
        </tr>
        <%--  <tr>
            <td align="right" colspan="3">&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddress1" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>

            </td>
            <td align="center">&nbsp;
            </td>
        </tr>--%>
        <tr>
            <td rowspan="10" style="width:60%; vertical-align:top; "  align="left">




                
                
                <table>
                    <tr>
                        <td >
                            <div id="imageContainer">
                                <div id="newPreview" style="height: 164px; width: 280px;" onclick="changePic('prevImg');">
                                </div>
                            </div>
                            <strong style="color: green;padding-left:278px;"> Upload Photo</strong>
                        </td>
                    </tr>
                </table>
                <div id="prevImg" class="previewClass" style="display: none;">
                    <a class="clbs" onclick="closePic('prevImg');"></a>

                    <table>
                        <tr>
                            <td>
                                <asp:FileUpload ID="fileUpl_stdPhoto" runat="server" onchange="DetectBrowser(this)" CssClass="FileUpload" />
                            </td>
                            <td>
                        </tr>
                    </table>
                </div>








            </td>
            <td class="auto-style6">Student Number</td>
            <td align="right" class="style5" style="width: 2%;">*
            </td>
            <td align="left" style="width: 50px;">
                <asp:TextBox ID="txtStudentId" runat="server" CssClass="textClass"
                    MaxLength="10" TabIndex="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdText">SASID</td>
            <td align="right" class="style5" >*
            </td>
            <td align="left">
                <asp:TextBox ID="txtSASID" runat="server" CssClass="textClass"
                    MaxLength="10" TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdText">First Name
            </td>
            <td align="right" class="style5" >*
            </td>
            <td align="left">
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="textClass" TabIndex="3"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdText">Last Name
            </td>
            <td align="right" class="style5" >*
            </td>
            <td align="left">
                <asp:TextBox ID="txtLastName" runat="server" CssClass="textClass" TabIndex="4"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdText">DOB
            </td>
            <td align="right" class="style5">*
            </td>
            <td align="left">
                <asp:TextBox ID="txtDOB" runat="server" onkeypress="return false;" TabIndex="5"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdText">Admission Date
            </td>
            <td align="right" class="style5">*
            </td>
            <td align="left">
                <asp:TextBox ID="txtJoinDt" runat="server" onkeypress="return false;" TabIndex="6"></asp:TextBox>
            </td>
        </tr>
        <tr>

            <td class="tdText">
                <asp:Label ID="lblstaus" runat="server" Text="Status"></asp:Label>
            </td>
            <td align="right" class="style5">
                <asp:Label ID="lblmstatus" runat="server" Text="*" ForeColor="#FF3300"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="drpClass" TabIndex="7">
                    <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Inactive" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

        </table>

    <asp:UpdatePanel ID="updateall" runat="server">
        <ContentTemplate>



            <table style="width: 100%;">

                <tr>
                    <td colspan="10" class="head_box"> </td>
                </tr>
                <tr>
                    <td colspan="10" class=""> </td>
                </tr>
                <tr>
                    <td colspan="10" class=""> <br /></td>
                </tr>
                 <tr>
                     <td class="tdText" colspan="2">Grade </td>
                     <td style="text-align:right;color:red; " class="auto-style28">*</td>
                     <td colspan="3">
                         <asp:TextBox ID="txtGrade" runat="server" CssClass="textClass" TabIndex="8"></asp:TextBox>
                     </td>
                     <td class="auto-style24">&nbsp;</td>
                     <td class="auto-style44">Residence</td>
                     <td  style="color:red;">*</td>
                     <td>
                         <asp:DropDownList ID="ddlResident" runat="server" CssClass="drpClass" TabIndex="9">
                             <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                             <asp:ListItem Text="Non Resident" Value="1"></asp:ListItem>
                             <asp:ListItem Text="Resident" Value="2"></asp:ListItem>
                         </asp:DropDownList>
                     </td>
                </tr>
                 <tr>
                     <td class="tdText" colspan="2">Citizenship</td>
                     <td class="auto-style28">&nbsp;</td>
                     <td colspan="3">
                         <asp:TextBox ID="txtCitizenShip" runat="server" CssClass="textClass" TabIndex="10"></asp:TextBox>
                     </td>
                     <td class="auto-style24">&nbsp;</td>
                     <td class="auto-style44">Guardianship Status</td>
                     <td>&nbsp;</td>
                     <td>
                         <asp:TextBox ID="txtGuardianship" runat="server" CssClass="textClass" TabIndex="11"></asp:TextBox>
                     </td>
                </tr>
                <tr>
                    <td class="tdText" colspan="2">Primary Language</td>
                    <td class="auto-style28">&nbsp;</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtPrimaryLang" runat="server" CssClass="textClass" TabIndex="12"></asp:TextBox>
                    </td>
                    <td class="auto-style24">&nbsp;</td>
                    <td class="auto-style44">Gender </td>
                    <td style="color:red;">*</td>
                    <td>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="drpClass" TabIndex="13">
                            <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                            <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                            <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tdText" colspan="2">Race</td>
                    <td class="auto-style28">&nbsp;</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtRace" runat="server" CssClass="textClass" MaxLength="10" TabIndex="14"></asp:TextBox>
                    </td>
                    <td class="auto-style24">&nbsp;</td>
                    <td class="auto-style44">Height</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtHeight" runat="server" CssClass="textClass" MaxLength="10" TabIndex="15"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="tdText" colspan="2">Weight</td>
                    <td class="auto-style28">&nbsp;</td>
                    <td colspan="3" >
                        <asp:TextBox ID="txtWeight" runat="server" CssClass="textClass" MaxLength="10" TabIndex="16"></asp:TextBox>
                     </td>
                    <td class="auto-style24">&nbsp;</td>
                    <td class="auto-style44">Hair Color</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtHairColor" runat="server" CssClass="textClass" MaxLength="10" TabIndex="17"></asp:TextBox>
                     </td>
                </tr>
                 <tr>
                    <td class="tdText" colspan="2">Eye Color</td>
                    <td class="auto-style28">&nbsp;</td>
                    <td colspan="3" >
                        <asp:TextBox ID="txtEyeColor" runat="server" CssClass="textClass" MaxLength="10" TabIndex="18"></asp:TextBox>
                     </td>
                    <td class="auto-style24">&nbsp;</td>
                    <td class="auto-style44">Distinguishing Marks </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtDisMarks" runat="server" CssClass="textClass" MaxLength="10" TabIndex="19"></asp:TextBox>
                     </td>
                </tr>
                 <tr>
                    <td class="tdText" colspan="2">Case Manager Residential</td>
                    <td class="auto-style28">&nbsp;</td>
                    <td colspan="3" >
                        <asp:TextBox ID="txtCaseManager" runat="server" CssClass="textClass" MaxLength="10" TabIndex="20"></asp:TextBox>
                     </td>
                    <td class="auto-style24">&nbsp;</td>
                    <td class="auto-style44">Case Manager Educational</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtCaseManEdu" runat="server" CssClass="textClass" MaxLength="10" TabIndex="21"></asp:TextBox>
                     </td>
                </tr>
                 <tr>
                    <td class="tdText" colspan="2">Legal Competency Status</td>
                    <td class="auto-style28">&nbsp;</td>
                    <td colspan="3" >
                        <asp:TextBox ID="txtLegal" runat="server" CssClass="textClass" MaxLength="10" TabIndex="22"></asp:TextBox>
                     </td>
                    <td class="auto-style24">&nbsp;</td>
                    <td class="auto-style44">Marital Status of Both Parents</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtMaritulS" runat="server" CssClass="textClass" MaxLength="10" TabIndex="23"></asp:TextBox>
                     </td>
                </tr>
                <tr>
                    <td class="tdText" colspan="3">Other State Agencies Involved With Student</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtOtherStage" runat="server" CssClass="textClass" TabIndex="24"></asp:TextBox>
                    </td>
                    <td class="auto-style24">&nbsp;</td>
                    <td class="auto-style44">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
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
                    <td colspan="2" >&nbsp;</td>
                    <td style="text-align: right;" class="auto-style28" >&nbsp;</td>
                    <td style="text-align: left;" colspan="3" >

                        &nbsp;</td>

                    <td class="auto-style24" >&nbsp;</td>
                    <td style="text-align: right;" class="auto-style42">&nbsp;</td>
                    <td style="text-align: left;" >

                        &nbsp;</td>
                </tr>


                <tr>
                    <td class="tdText" colspan="2">Address Line 1</td>
                    <td style="text-align: left;" class="auto-style27" colspan="4">
                         &nbsp; &nbsp;<asp:TextBox ID="txtAddress1" runat="server" CssClass="textClass" OnTextChanged="txtAddress1_TextChanged" TabIndex="25" Width="92%"></asp:TextBox>
                    </td>
                    <td style="text-align: left;">
                        &nbsp;</td>
                </tr>


                <tr>
                    <td class="auto-style18" colspan="2">Address Line 2</td>
                    <td style="text-align: left;" class="auto-style12" colspan="4">
                         &nbsp; &nbsp;<asp:TextBox ID="txtAddress2" runat="server" CssClass="textClass" TabIndex="26" Width="92%"></asp:TextBox>
                    </td>
                    <td class="auto-style24">&nbsp;</td>
                    <td class="auto-style44">&nbsp;</td>
                    <td style="text-align: right;">&nbsp;</td>
                    <td style="text-align: left;">&nbsp;</td>
                </tr>


                <tr >
                    <td class="tdText" colspan="2">Address Line 3</td>
                    <td style="text-align: left;" class="auto-style26" colspan="4">
                         &nbsp; &nbsp;<asp:TextBox ID="txtAddress3" runat="server" CssClass="textClass" TabIndex="27" Width="92%"></asp:TextBox>
                    </td>

                    <td class="auto-style24"></td>

                    <td class="auto-style44">&nbsp;</td>
                    <td style="text-align: right;" class="style5">&nbsp;</td>
                    <td style="width: 30%; text-align: left;">

                        &nbsp;</td>
                </tr>

                <tr>
                    <td class="auto-style35">Country</td>
                    <td class="auto-style36" style="color:red">&nbsp;</td>
                    <td class="auto-style27" style="text-align: left;">
                        *<asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" CssClass="drpClass" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" TabIndex="28" Width="128px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: left;" class="auto-style23">State</td>
                    <td style="text-align: left;color:red" class="style2">*</td>
                    <td style="text-align: left;" class="auto-style37">
                         &nbsp;<asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" CssClass="drpClass" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" TabIndex="29" Width="196px">
                            <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style24">&nbsp;</td>
                    <td class="auto-style44">&nbsp;</td>
                    <td class="style5" style="text-align: right;">&nbsp;</td>
                    <td style="width: 30%; text-align: left;">&nbsp;</td>
                </tr>

                <tr>
                    <td class="auto-style35">City</td>
                    <td class="auto-style36">&nbsp;</td>
                    <td class="auto-style27" style="text-align: center;">
                         &nbsp;<asp:TextBox ID="txtCity" runat="server" CssClass="textClass" TabIndex="30" Width="117px"></asp:TextBox>
                    </td>
                    <td class="auto-style23" style="text-align: left;">Zip </td>
                    <td class="style2" style="text-align: left;color:red">*</td>
                    <td style="text-align: left;" class="auto-style37">
                         &nbsp;<asp:TextBox ID="txtZip" runat="server" CssClass="textClass" MaxLength="5" TabIndex="31" Width="182px"></asp:TextBox>
                    </td>
                    <td class="auto-style24">&nbsp;</td>
                    <td class="auto-style44">&nbsp;</td>
                    <td class="style5" style="text-align: right;">&nbsp;</td>
                    <td style="width: 30%; text-align: left;">&nbsp;</td>
                </tr>

                <tr>
                    <td class="tdText" colspan="2">&nbsp;</td>
                    <td style="text-align: right;" class="auto-style27">&nbsp;</td>
                    <td style="text-align: left;" colspan="3">

                        &nbsp;</td>

                    <td class="auto-style24"></td>

                    <td class="auto-style44">&nbsp;</td>
                    <td style="text-align: right;" class="style5">&nbsp;</td>
                    <td style="width: 30%; text-align: left;">

                        &nbsp;</td>
                </tr>

                <tr>
                    <td class="tdText" colspan="2" style="display:none">Zip

                    </td>
                    <td style="text-align: right;" class="auto-style27">&nbsp;</td>
                    <td style="text-align: left;" colspan="3">

                        &nbsp;</td>

                    <td class="auto-style24"></td>

                    <td class="auto-style44" style="display:none">Home Phone</td>
                    <td style="text-align: right;" class="style5">&nbsp;</td>
                    <td style="width: 30%; text-align: left;">

                        <asp:TextBox ID="txtHomePhone" runat="server" CssClass="textClass" MaxLength="15" TabIndex="32"  style="display:none"></asp:TextBox>

                    </td>
                </tr>

                <tr>
                    <td style="display:none" class="tdText" colspan="2" >Mobile</td>
                    <td style="text-align: right;" class="auto-style27">&nbsp;</td>
                    <td style="text-align: left;" colspan="3">

                        <asp:TextBox ID="txtMobile" runat="server" CssClass="textClass" TabIndex="33"  style="display:none"></asp:TextBox>

                    </td>

                    <td class="auto-style24"></td>

                    <td class="auto-style44"  style="display:none">Email</td>
                    <td style="text-align: right;" class="style5">&nbsp;</td>
                    <td style="width: 30%; text-align: left;">

                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textClass" TabIndex="34"  style="display:none"></asp:TextBox>

                    </td>
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
             </ContentTemplate>
    </asp:UpdatePanel>
            <table>
                <tr>

                    <td colspan="7" class="head_box">Medical and Insurance/td>
                </tr>

                <tr>
                    <td class="tdText">&nbsp;</td>
                     <td>&nbsp;</td>
                     <td>
                         &nbsp;</td>
                    <td class="auto-style13">&nbsp;</td>
                     <td class="auto-style17">&nbsp;</td>
                     <td></td>
                     <td>
                         &nbsp;</td>
                </tr>
                <tr>
                    <td class="tdText">Date of Last Physical Exam</td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtDateofLast" runat="server" CssClass="textClass" onkeypress="return false;" TabIndex="35"></asp:TextBox>
                    </td>
                    <td class="auto-style12"></td>
                    <td class="auto-style18">Medical Conditions/Diagnosis</td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtMedicalCon" runat="server" CssClass="textClass" TabIndex="36"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Allergies</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtAllergies" runat="server" CssClass="textClass" TabIndex="37"></asp:TextBox>
                    </td>
                    <td class="auto-style12"></td>
                    <td class="auto-style18">Current Medications</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtCurrentMed" runat="server" CssClass="textClass" TabIndex="38"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Self Preservation Ability</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtSelfPres" runat="server" CssClass="textClass" TabIndex="39"></asp:TextBox>
                    </td>
                    <td class="auto-style12"></td>
                    <td class="auto-style18">Significant Behavior Characteristics</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtSignBehv" runat="server" CssClass="textClass" TabIndex="40"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">
                        Relevant Capabilities<br />
                        
                                            
                    </td>
                    <td>&nbsp;</td>
                     <td class="tdText">
                        
                        <asp:TextBox ID="txtCapabilities" runat="server" CssClass="textClass" TabIndex="41"></asp:TextBox>

                    
                    </td>
                    <td class="auto-style12"></td>
                    <td class="auto-style18">Relevant Limitations</td>
                    <td class="tdText">
                        <br />
                        
                    
                    </td>
                    <td>
                        <asp:TextBox ID="txtLimitations" runat="server" CssClass="textClass" TabIndex="42"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Relevant Preferences</td>
                    <td>&nbsp;</td>
                    <td class="tdText">
                        <asp:TextBox ID="txtPreferences" runat="server" CssClass="textClass" TabIndex="43"></asp:TextBox>
                    </td>
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
                    <td style="width: 20%; text-align: left;">

                        &nbsp;</td>

                    <td class="auto-style13">&nbsp;</td>
                    <td style="text-align: left;" class="auto-style17">&nbsp;</td>
                    <td></td>
                    <td style="width: 30%; text-align: left;">

                        &nbsp;</td>
                </tr>

                <tr>
                    <td class="tdText" style="width: 20%;">District Code</td>

                    <td class="style5" style="width: 5%; text-align: right;">*</td>

                    <td style="width: 20%; text-align: left;">
                        <asp:TextBox ID="txtDistrictCode" runat="server" CssClass="textClass" TabIndex="44"></asp:TextBox>
                    </td>
                    <td class="auto-style12"></td>
                    <td class="auto-style18">District Type</td>
                    <td style="text-align: left;"></td>
                    <td style="width: 30%; text-align: left;">
                        <asp:TextBox ID="txtDistType" runat="server" CssClass="textClass" TabIndex="45"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td style="width: 20%;" class="tdText">District Function</td>
                    <td style="width: 5%; text-align: left;"></td>
                    <td style="width: 20%; text-align: left;">

                        <asp:TextBox ID="txtDistFunct" runat="server" CssClass="textClass" TabIndex="46"></asp:TextBox>

                    </td>

                    <td class="auto-style12"></td>

                    <td class="auto-style18">Contact Person</td>
                    <td style="text-align: left;"></td>
                    <td style="width: 30%; text-align: left;">

                        <asp:TextBox ID="txtContact" runat="server" CssClass="textClass" TabIndex="47"></asp:TextBox>

                    </td>
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


       


    <table width="100%">
        <tr>
            <td align="center">

                &nbsp;</td>
        </tr>
        <tr>
            <td align="center">

                <asp:Button ID="Button_Add" runat="server" CssClass="NFButton" OnClick="Button_Add_Click" OnClientClick=" return validate()"
                    Text="Save" ValidationGroup="a" TabIndex="48" />


            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:HiddenField ID="HiddenField1" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center"></td>
        </tr>
    </table>


</asp:Content>
