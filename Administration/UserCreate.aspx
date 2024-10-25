<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="UserCreate.aspx.cs" Inherits="Admin_UserCreate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <script type="text/javascript">
        $(document).ready(function () {

            $('#btnReset').click(function () {
                HideDialog();
            });

            var querystring = location.search.replace('?', '').split('&');
            // declare object
            var queryObj = {};
            // loop through each name-value pair and populate object
            for (var i = 0; i < querystring.length; i++) {
                // get name and value
                var name = querystring[i].split('=')[0];
                var value = querystring[i].split('=')[1];
                // populate object
                queryObj[name] = value;
            }

            // ***now you can use queryObj["<name>"] to get the value of a url
           // alert(queryObj["Type"]);
            // ***variable
            if (queryObj["Type"] === "Update") {

                //   $('#btnReset').fadeIn('fast');
               
            }
            else {
                //  $('#btnReset').fadeOut('fast');
                
            }
        });

        function HideDialog() {
            $('#overlay').fadeIn('slow', function () {
                $('#divHderDtls').fadeIn('slow');
            });
            $('#closeXX').click(function () {
                $('#divHderDtls').fadeOut('slow');
                $('#overlay').fadeOut('slow');

            });

            return true;
        }
    </script>
    <script type="text/javascript">
        var multiClickUpload = false;
        function enableButton() {
            multiClickUpload = false;
        }
        function multiClickCheckUpload(btn) {
            if (!multiClickUpload) {
                multiClickUpload = true;
                btn.style.opacity = '0.5';
                return true;
            }
            return false;
        }
        function changePassword() {
            var usrHdnval = $("#<%=UserIdToUpdate.ClientID %>");
            $.ajax({
                type: "POST",
                url: "UserCreate.aspx/changePassword",
                //data: '{currPwd: "' + document.getElementById('<%= txtCurrentpsword.ClientID %>').value + '",newPwd: "' + document.getElementById('<%=txtnewpaswrd.ClientID %>').value + '", conPwd:"' + document.getElementById('<%=txtconfirmnewPasword.ClientID %>').value + '"}',
                data: '{currPwd: "' + document.getElementById('<%= txtCurrentpsword.ClientID %>').value + '",newPwd: "' + document.getElementById('<%=txtnewpaswrd.ClientID %>').value + '", conPwd:"' + document.getElementById('<%=txtconfirmnewPasword.ClientID %>').value + '",usrid:"' + usrHdnval.val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function OnSuccess(response) {
                    alert(response.d);
                    //alert($('.resetclass').val())
                    $('.resetclass').val("");

                    //HideDialog();
                },
                failure: function (response) {
                    alert(response.d);
                    HideDialog();
                }


            });
        }
        function resetPassword() {
            var usrHdnval = $("#<%=UserIdToUpdate.ClientID %>");     
            $.ajax({
                type: "POST",
                url: "UserCreate.aspx/ResetPassword",
                //data: '{status:"reset"}',
                data: '{status:"reset",usrid:"' + usrHdnval.val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function OnSuccess(response) {
                    resetbtnhide();
                //    alert(response.d);
                    //alert($('.resetclass').val())
                  

                   
                },
                failure: function (response) {
                    alert(response.d);
                    resetdialog();
                }


            });
        }
        function resetdialog() {
            $(".fullOverlay").show();
            $(".pwdialog").show();

        }
        function resetbtnhide() {
            $('.btnReset1').hide();
            $('.LBpwrest').html("Password will be reset by user upon next login.");
            hidereset();
          
        }
        function resetbtnhide1() {
         //   $('.btnReset1').parent.append("Password Reset Requested");
            $('.btnReset1').hide();
                      
        }
        function hidereset() {

            $(".fullOverlay").hide();
            $(".pwdialog").hide();
        }
        function popPrompts() {

            $(".fullOverlay").show();
            $(".web_dialog2").show();
        }

        function displaydup() {
            $('#<%=lnkDupDownload.ClientID%>').hide();
        }
        function removeDiv() {
            $("#<%=divMessage.ClientID%>").hide();
            $("#<%=divMessageDel.ClientID%>").hide();
        }

        function popPromptsDel() {

            $(".fullOverlay").show();
            $(".web_dialog3").show();
        }

        function HidePopup() {
            $(".fullOverlay").hide();
            $(".web_dialog2").hide();
            $("#<%=divMessage.ClientID%>").hide();
            $("#<%=divMessageDel.ClientID%>").hide();
            $('.drpClass').prop('selectedIndex', 0);
        }
        function HidePopupDel() {
            $(".fullOverlay").hide();
            $(".web_dialog3").hide();
            $("#<%=divMessageDel.ClientID%>").hide();
            $("#<%=divMessage.ClientID%>").hide();

        }

    </script>

    <style type="text/css">
        a.sprited {
            display: block;
            width: 30px;
            height: 30px;
            position: absolute;
            z-index: 999;
            left: 500px;
            top: -18px;
        }

        hr {
            margin: 5px 0 10px 0;
            padding: 0;
            border: 1px solid #b2ccca;
            line-height: 1px;
        }

        .web_dialog_overlay {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background-color: gray;
            opacity: .8;
            filter: alpha(opacity=.8);
            -moz-opacity: .8;
            z-index: 101;
            display: none;
        }

        .web_dialog {
            display: none;
            position: fixed;
            width: 500px;
            height: auto;
            left: 50%;
            margin-left: -277px;
            margin-top: 160px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 102;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
            top: 0px;
        }
        .pwdialog
        {
             
            display: none;
            position: fixed;
            width: 500px;
            height: auto;
            left: 50%;
            margin-left: -277px;
            margin-top: 160px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 2001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
            top: 0px;
        }
        .web_dialog_title a {
            color: White;
            text-decoration: none;
        }

        .auto-style9 {
            height: 26px;
        }

        .textPasswrd {
            border: 1px solid #d7cece;
            background-color: white;
            width: 225px;
            float: left;
            height: 23px;
            color: #676767;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 13px;
            border-radius: 5px;
            padding: 0 5px 0 10px;
        }

        .auto-style13 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            font-size: 13px;
            line-height: 12px;
            height: 10px;
            width: 35px;
        }

        .fullOverlay {
            display: none;
            top: 0px;
            left: 0px;
            position: fixed;
            z-index: 2000;
            width: 100%;
            height: 100%;
            background-image: url("../Administration/images/overlay.png");
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
            width: 700px;
            top: 2%;
            z-index: 9999;
        }

            .web_dialog2 hr {
                background-color: #B6D1DD;
                margin-top: 5px;
                padding: 1px;
            }

        .web_dialog3 {
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
            width: 700px;
            top: 2%;
            z-index: 9999;
        }

            .web_dialog3 hr {
                background-color: #B6D1DD;
                margin-top: 5px;
                padding: 1px;
            }

        .newBtn {
        }


        /*start*/

        .newBtn {
            background-color: #03507d;
            background-position: 0 0;
            border: medium none;
            border-radius: 4px;
            color: #fff;
            cursor: pointer;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: bold;
            height: 27px;
            text-decoration: none;
            width: 106px;
        }

            .newBtn:visited {
                color: #fff;
            }

        .newBtn:hover {
            background-color: #169F4F;
        }




        /*End*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">

    <%--<asp:Button ID="btnDownload" runat="server" CssClass="NFButton" Text="Template Download" OnClick="btnDownload_Click" />--%>
    <div class="fullOverlay">
    </div>

    <div id="divPrmpts" class="web_dialog2">
        <a id="A2" onclick="HidePopup();" href="#" style="margin-top: -13px; margin-right: -14px;">
            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>

        <h4 style="font-size: 15px; text-decoration-line: underline;">Upload Users</h4>

        <br />

        <tr>
            <td>Select Group</td>
            <td>
                <asp:DropDownList ID="drpGrp" runat="server" CssClass="drpClass" AutoPostBack="true" OnSelectedIndexChanged="drpGrp_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Select Role</td>
            <td>
                <asp:DropDownList ID="drpRol" runat="server" CssClass="drpClass" AutoPostBack="true" OnSelectedIndexChanged="drpRol_SelectedIndexChanged">
                    <asp:ListItem Value="0">----------Select----------</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <br />
        <br />
        <div style="width: 100%; padding-left: 10px;">
            <asp:FileUpload ID="fileBulk" runat="server" />
            <asp:Button ID="btUpload" runat="server" Text="Upload" OnClientClick="return multiClickCheckUpload(this);" OnClick="btn_bulkUp_Click" CssClass="NFButton" />

            <asp:LinkButton ID="btnDownload" runat="server" Style="padding-left: 42px;" Text="Download Template" OnClick="btnDownload_Click"></asp:LinkButton>
            <br />
            <asp:Label ID="lMsg" runat="server" />

        </div>
         <asp:LinkButton ID="lnkDupDownload" runat="server" Style="padding-left: 42px;display:none" Text="Download Duplicate List" OnClick="btnDupDownload_Click" ></asp:LinkButton>
            <br />
        <div id="divMessage" runat="server" style="width: 98%"></div>
        <div id="nameCheck" runat="server" style="width: 98%;"></div>
         

        <%--<input type="button" id="btnReset"  runat="server" value="reset password" onclick="resetdialog();" class="newBtn btnReset1" />--%>
    </div>



    <div id="divPrmpts3" class="web_dialog3">
        <a id="A3" onclick="HidePopupDel();" href="#" style="margin-top: -13px; margin-right: -14px;">
            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>

        <h4 style="font-size: 15px; text-decoration-line: underline;">Delete Users</h4>

        <br />


        <div style="width: 100%; padding-left: 10px;">
            <asp:FileUpload ID="fileUploadDel" runat="server" />
            <asp:Button ID="btnUpDel" runat="server" Text="Upload" OnClientClick="return multiClickCheckUpload(this);" OnClick="btnDelUser_Click" CssClass="NFButton" />
            <br />
        </div>
        <div id="divMessageDel" runat="server" style="width: 98%"></div>

    </div>



    <div id="overlay" class="web_dialog_overlay">
    </div>
    <div id="divHderDtls" class="web_dialog">
        <div id="divDtls" style="width: 500px;">

            <a id="closeXX" class="close sprited" href="#">
                <img src="../Administration/images/clb.PNG" style="border: 0px; float: right; margin: 5px 5px 0 0;" width="25px" /></a>

            <table>
                <tr>
                    <td class="tdText">
                        <h2>Reset Password
                        </h2>
                    </td>
                    <td style="text-align: right"></td>
                </tr>
                <tr>
                    <td class="tdText"></td>
                    <td style="text-align: right"></td>
                </tr>
                <tr>
                    <td class="tdText">Current Password:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCurrentpsword" runat="server" CssClass="textClass resetclass" MaxLength="30"
                            TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">New Password
                    </td>
                    <td>
                        <asp:TextBox ID="txtnewpaswrd" runat="server" CssClass="textClass resetclass" MaxLength="30" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdText">Confirm New Password
                    </td>
                    <td>
                        <asp:TextBox ID="txtconfirmnewPasword" runat="server" CssClass="textClass resetclass" MaxLength="30"
                            TextMode="Password"></asp:TextBox>
                    </td>
                </tr>



                <tr>
                    <td colspan="2" style="text-align: Center;">
                        <input id="btnClose" type="button" class="NFButton" value="Submit" style="margin-left: -51px;" onclick="javascript: changePassword();" />
                        <!--<asp:Button ID="BtnSubmit" runat="server" CssClass = "NFButton" Text="Submit" 
        OnClientClick="return HideDialog();" onclick="BtnSubmit_Click" />-->
                    </td>
                </tr>
            </table>
        </div>
    </div>

     <div id="pwresetoverlay" class="pwdialog">
    </div>
    <div id="divHderDtls1" class="web_dialog pwdialog">
        <div id="divDtls1" style="width: 500px;">

            <a id="A1" onclick=" hidereset();" class="close sprited" href="#">
                <img src="../Administration/images/clb.PNG" style="border: 0px; float: right; margin: 5px 5px 0 0;" width="25px" /></a>

            <table>
                <tr>
                    <td class="tdText">
                        <h2>Reset Password
                        </h2>
                    </td>
                    <td style="text-align: right"></td>
                </tr>
                <tr>
                    <td class="tdText"></td>
                    <td style="text-align: right"></td>
                </tr>
                <tr>

                 <td colspan="2"  style="text-align: Center;"> 
                 Password will be reset by user upon next login 
                 </td>
               </tr>
                <tr>
                    <td class="tdText"></td>
                    <td style="text-align: right"></td>
                </tr>

                <tr>
                    <td  style="text-align: Center;">
                        <input id="Button1" type="button" class="NFButton" value="OK" style="margin-left: -51px;" onclick="javascript: resetPassword();" />
                      
                    </td>
                     <td  style="text-align: Center;">
                        <input id="Button2" type="button" class="NFButton" value="Cancel" style="margin-left: -51px;" onclick="javascript: hidereset();" />
                      
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">

                <tr>
                    <td runat="server" id="tdMsg" style="color: #FF0000" colspan="8"></td>
                </tr>

                <tr>
                    <td align="right" class="tdText" style="width: 6%!important">First Name
                    </td>
                    <td align="right" style="color: #FF0000">
                        <span style="color: red">*</span>
                    </td>
                    <td align="left">

                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="30" CssClass="textClass" TabIndex="1"></asp:TextBox>
                    </td>


                    <td align="right" class="tdText" style="width: 6%!important">Login ID</td>
                    <td align="right" class="style5" style="color: #FF0000">
                        <span style="color: red">*</span>
                    </td>
                    <td align="left">

                        <asp:TextBox ID="txtLogin" runat="server" MaxLength="30" CssClass="textClass" TabIndex="6"></asp:TextBox>

                    </td>
                     <td>
                        <%--<input type="button" id="btnReset"  runat="server" value="reset password" onclick="resetdialog();" class="newBtn btnReset1" />--%>
                     <asp:Button ID="btnReset" runat="server" CssClass="NFButton newBtn btnReset1" Text="Reset Password" OnClientClick="resetdialog();" />
                     
                     </td>
                    <td>
                        <input type="button" id="btnNew" runat="server" value="Add User in Bulk" onclick="displaydup(); popPrompts();" class="newBtn" />
                    </td>
                    <td>
                        <input type="button" id="btnDelUser" runat="server" value="Delete User" onclick="popPromptsDel()" class="newBtn" />
                        <%--<asp:Button ID="btnDelUser" Text="Delete" runat="server" CssClass="NFButton" OnClick="btnDelUser_Click" />--%>
                    </td>

                    <%--<td>
                        <div style="width: 100%; padding-left: 10px;">
                            <asp:FileUpload ID="fileBulk" runat="server" />
                            <asp:Button ID="btn_bulkUp" runat="server" Text="Upload" OnClick="btn_bulkUp_Click" CssClass="NFButton" />
                            <br />
                            <asp:Label ID="lMsg" runat="server" />
                        </div>
                    </td>--%>
                </tr>
                <tr>
                    <td runat="server" id="tdMsgFileUpload" style="color: #FF0000" colspan="8"></td>
                </tr>
                <tr>
                    <td align="right" class="tdText" style="width: 6%!important">Last Name
                    </td>
                    <td align="right" class="style5" style="color: #FF0000">
                        <span style="color: red">*</span> </td>
                    <td align="left">
                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="30" CssClass="textClass" TabIndex="2"></asp:TextBox>
                    </td>
                    <td align="right" class="tdText" style="width: 6%!important">
                        <asp:Label ID="lblpsword" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td align="right" class="style5" style="color: #FF0000">
                        <span id="star2" runat="server" style="color: red">*</span>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="30" CssClass="textPasswrd" TextMode="Password" TabIndex="7"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdText" style="width: 6%!important">Position</td>
                    <td align="right" style="color: #FF0000">
                        <span style="color: red">*</span></td>
                    <td align="left">

                        <asp:TextBox ID="txtposition" runat="server" CssClass="textClass" TabIndex="3"></asp:TextBox>

                        <asp:AutoCompleteExtender ID="txtposition_AutoCompleteExtender" runat="server" DelimiterCharacters="" Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtposition" UseContextKey="True" MinimumPrefixLength="0">
                        </asp:AutoCompleteExtender>

                    </td>

                    <td align="right" class="tdText" style="width: 10%!important">
                        <asp:Label ID="lblConfrmPassword" runat="server" Text="Confirm Password"></asp:Label>
                    </td>
                    <td align="right" class="style5" style="color: #FF0000">
                        <span id="star" runat="server" style="color: red">*</span>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" MaxLength="30" CssClass="textPasswrd"
                            TextMode="Password" TabIndex="8"></asp:TextBox>
                        <%--<input id="btnReset" type="button" value="Reset Password" style="display: none; background: none; border: 0; color: blue; cursor: pointer; font-weight: bold" />--%>
                     <b>
                        <asp:Label ID="LBpwrest" runat="server" CssClass="LBpwrest" style="color:darkgreen" Text =""> </asp:Label>
                     </b>
                            </td>

                </tr>
                <tr>
                    <td align="right" class="tdText" style="width: 6%!important">User Initial</td>
                    <td align="right" class="style5" style="color: #FF0000">
                        <span style="color: red">*</span>
                    </td>
                    <td align="left">

                        <asp:TextBox ID="txtUserNo" runat="server" MaxLength="8" CssClass="textClass" TabIndex="5"></asp:TextBox>
                    </td>
                    <td align="right" class="tdText" style="width: 10%!important">
	                    <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                    </td>
                    <td align="right" class="style5" style="color: #FF0000">
	                    <span id="Span1" runat="server" style="color: red"></span>
                    </td>
                    <td align="left">
	                    <asp:TextBox ID="txtEmail2" runat="server" MaxLength="30" TabIndex="8"></asp:TextBox>
                     </td>
                </tr>
                <tr>
                    <td class="tdText"></td>
                </tr>
                <tr>
                    <td align="right" class="tdText" style="width: 6%!important">Select Groups</td>
                    <td align="right" style="color: #FF0000">
                        <span style="color: red">*</span>
                    <td align="left" colspan="2">

                        <asp:DropDownList ID="ddlGroups" runat="server" AutoPostBack="True" CssClass="drpClass" OnSelectedIndexChanged="ddlGroups_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdText" style="width: 6%!important">Description</td>
                    <td align="right" class="style5">&nbsp;</td>
                    <td align="left">
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="textClass" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdText" style="width: 6%!important">
                        <asp:Label ID="lblrole" runat="server" Text="Select Role" Visible="False"></asp:Label>
                    </td>
                    <td align="right" style="color: #FF0000" valign="top">
                        <span id="rolestar" runat="server" style="color: red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:CheckBoxList ID="chkRole" runat="server" CssClass="textClass" Width="243px">
                        </asp:CheckBoxList>
                    </td>
                    <td align="right" class="tdText">&nbsp;</td>
                    <td align="right" class="style5">&nbsp;</td>
                    <td align="left">&nbsp;</td>
                </tr>

                <tr>
                    <td align="right" class="tdText">

                        <asp:Label ID="lblStatus" runat="server" Text="Status" Visible="False"></asp:Label>
                    </td>
                    <td align="right" class="style5"></td>
                    <td align="left">

                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="drpClass" Visible="False">
                            <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="auto-style13" colspan="2"></td>
                    <td align="right" style="color: #FF0000"></td>
                    <td align="left" colspan="2"></td>
                    <td align="right" class="tdText" style="display: none">Gender</td>
                    <td align="right" class="style5" style="color: #FF0000"></td>
                    <td align="left" style="display: none">

                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="drpClass" TabIndex="4">
                            <%--<asp:ListItem Value="0">Male</asp:ListItem>--%>
                            <asp:ListItem Text="Male" Value="Male">Male</asp:ListItem>
                            <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                        </asp:DropDownList>
                    </td>

                </tr>
                <tr>
                    <td align="right" class="tdText" valign="top" style="width: 10%; vertical-align: middle !important">&nbsp;</td>
                    <td align="right" class="style5" style="color: #FF0000" valign="top">
                        &nbsp;</td>
                    <td colspan="5" align="left">
                        &nbsp;<asp:CheckBox ID="Chkselall" runat="server" AutoPostBack="True" ForeColor="#666666" OnCheckedChanged="Chkselall_CheckedChanged" Text=" Select All" />
                        &nbsp;</td>

                </tr>
                <tr>
                    <td align="right" class="tdText" style="width: 10%; vertical-align: middle !important" valign="top">Class </td>
                    <td align="right" class="style5" style="color: #FF0000" valign="top"><span style="color: red"></span></td>
                    <td align="left" colspan="5">
                        <table width="92%">
                            <tr>
                                <td width="100%">
                                    <asp:DataList ID="DLclass" runat="server" AlternatingItemStyle-HorizontalAlign="Left" RepeatColumns="4" RepeatDirection="Horizontal" TabIndex="9">
                                        <AlternatingItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td width="100%">
                                                        <asp:CheckBox ID="chkClass" runat="server" Style="font-family: Arial,Helvetica,sans-serif; color: #676767; width: 130px;" Text='<%# Eval("Name") %>' Width="178px" />
                                                        <asp:HiddenField ID="hdnClass" runat="server" Value='<%# Eval("Id") %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="auto-style13" colspan="2">&nbsp;</td>
                    <td align="right" style="color: #FF0000">&nbsp;</td>
                    <td colspan="2">&nbsp;</td>
                    <td align="right" class="tdText">&nbsp;</td>
                    <td align="right" class="style5">&nbsp;</td>
                    <td align="left">&nbsp;</td>
                </tr>


            </table>


            <table style="display: none">

                <tr>
                    <td align="left" class="head_box" colspan="2" style="width: auto;">Address Information
                        <asp:TextBox ID="txtAddressId" runat="server" CssClass="textClass"
                            Visible="False"></asp:TextBox>
                    </td>
                </tr>


                <tr>
                    <td></td>
                    <td></td>
                </tr>


                <tr>
                    <td class="tdText" style="width: 250px">Address Line1</td>
                    <td>
                        <asp:TextBox ID="txtAddress1" runat="server" CssClass="textClass" MaxLength="30" TabIndex="10" Width="108%"></asp:TextBox></td>
                </tr>

                <tr>
                    <td class="tdText">Address Line2</td>
                    <td>
                        <asp:TextBox ID="txtAddress2" runat="server" CssClass="textClass" MaxLength="30" TabIndex="11" Width="108%"></asp:TextBox></td>
                </tr>

                <tr>
                    <td class="tdText">Address Line3</td>
                    <td>
                        <asp:TextBox ID="txtAddress3" runat="server" CssClass="textClass" MaxLength="30" TabIndex="12" Width="108%"></asp:TextBox></td>
                </tr>

                <tr>
                    <td></td>
                    <td>

                        <table>
                            <tr>
                                <td class="tdText" style="width: 80px;">Country</td>
                                <td style="width: 150px;">
                                    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" Width="200px" CssClass="drpClass" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" TabIndex="13">
                                    </asp:DropDownList></td>
                                <td class="tdText" style="padding-left: 25px;">State<span style="color: red"> </span></td>
                                <td>
                                    <asp:DropDownList ID="ddlState" runat="server" CssClass="drpClass" TabIndex="14" Width="230px">
                                        <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="tdText" style="width: 80px;">City</td>
                                <td>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="textClass" MaxLength="30" Width="180px" TabIndex="15"></asp:TextBox></td>
                                <td class="tdText" style="padding-left: 25px;">Zip<span style="color: red"> </span></td>
                                <td>
                                    <asp:TextBox ID="txtZip" runat="server" CssClass="textClass" MaxLength="15" Width="210px" TabIndex="16"></asp:TextBox></td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td></td>
                </tr>

                <tr>
                    <td></td>
                    <td></td>
                </tr>


            </table>



            <table>


                <tr style="display: none">
                    <td align="right" class="auto-style13" colspan="2" hidden="hidden">&nbsp;</td>
                    <td align="right" style="color: #FF0000" hidden="hidden"></td>
                    <td align="left" colspan="2" hidden="hidden">&nbsp;</td>
                    <td align="right" class="tdText" hidden="hidden">Land Line</td>
                    <td align="right" class="style5" hidden="hidden"></td>
                    <td align="left" hidden="hidden">

                        <asp:TextBox ID="txtHomePhone" runat="server" MaxLength="30" CssClass="textClass" TabIndex="17"></asp:TextBox>
                    </td>
                </tr>
                <tr style="display: none">
                    <td align="right" class="auto-style13" colspan="2" hidden="hidden">Mobile
                    </td>
                    <td align="right" style="color: #FF0000" hidden="hidden">&nbsp;</td>
                    <td align="left" colspan="2" hidden="hidden">

                        <asp:TextBox ID="txtMobile" runat="server" MaxLength="13" CssClass="textClass" TabIndex="18"></asp:TextBox>

                    </td>
                    <td align="right" class="tdText" hidden="hidden">Email 
                    </td>
                    <td align="right" class="style5" hidden="hidden">&nbsp;</td>
                    <td align="left" hidden="hidden">

                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" CssClass="textClass" TabIndex="19"></asp:TextBox>
                    </td>
                </tr>




            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%">
    </table>
    <table width="100%">
        <tr>
            <td align="center" colspan="6">
                <div class="rounded-corners" id="submitdiv" runat="server">
                    <asp:HiddenField runat="server" ID="UserIdToUpdate" />
                    <asp:HiddenField runat="server" ID="SessnIDToUpdate" />
                    <asp:Button ID="btnAdd" runat="server" CssClass="NFButton" OnClientClick="return multiClickCheckUpload(this);" OnClick="btnAdd_Click"
                        Text="Save" ValidationGroup="a" TabIndex="20" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="NFButton" Text="Cancel" OnClick="btnCancel_Click" />
                </div>
            </td>
        </tr>

        <tr>
            <td align="center" colspan="6"></td>
        </tr>

        <tr>
            <td align="center" colspan="6"></td>
        </tr>
        <tr>
            <td align="center" colspan="6"></td>
        </tr>
    </table>
</asp:Content>
