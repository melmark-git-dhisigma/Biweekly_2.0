<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
     CodeFile="AddSchool.aspx.cs" Inherits="Admin_AddSchool" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">



    <script type="text/javascript">

        window.onload = function () {
            new JsDatePick({
                useMode: 2,
                target: '<%=(txtHolidayDate).ClientID %>',
                dateFormat: "%m/%d/%Y",
            });
            loadJquery();
        };

        var stat;
        var delay = 2000;

        function Show() {

            var tagmoveop2 = document.getElementById("divClass").style;
            tagmoveop2.display = "block";
        }

        function Hide() {

            var tagmoveop2 = document.getElementById("divClass").style;
            tagmoveop2.display = "none";
        }


        function Count(text, long) {
            var maxlength = new Number(long); // Change number to your max length.
            if (text.value.length > maxlength) {
                text.value = text.value.substring(0, maxlength);
                var lbl = document.getElementById('<%=lblError.ClientID %>');
                lbl.innerHTML = "Maximum Allowed only 100 Characters ";
                lbl.visible = true;
                //alert(lbl.value);
                //alert(" Only " + long + " characters");

            }
        }
        function EnableTxt(thisChk) {
           
            //alert($(thisChk).attr('checked'));
            if ($(thisChk).attr('checked') == 'checked') {
                //   if (  $('#'+thisChk.id).attr('checked', true);) {
                var parnt = $(thisChk).parent().parent();
                var textBoxes = parnt.find('input:text').removeAttr('disabled');
            }
            else {
                var parnt = $(thisChk).parent().parent();
                var textBoxes = parnt.find('input:text').attr('disabled', 'disabled');
                
            }
            
           
        }



    </script>
    <style type="text/css">
        #divClass
        {
            margin-top: 6px;
            display: none;
        }
        .popup
        {
            -webkit-border-radius: 7px 15px 4px 13px;
            -moz-border-radius: 7px 15px 4px 13px;
            border-radius: 7px 15px 4px 13px;
            background-color: #DDD;
            height: 50px;
            width: 50px;
            border: 2px solid #0FA1D0;
            position: absolute;
            display: block;
            font-family: Verdana, Geneva, sans-serif;
            font-size: small;
            text-align: justify;
            padding: 5px;
            overflow: auto;
            z-index: 2;
        }
        .popup_bg
        {
            position: absolute;
            visibility: hidden;
            height: 100%;
            width: 100%;
            left: 0px;
            top: 0px;
            filter: alpha(opacity=70); /* for IE */
            opacity: 0.7; /* CSS3 standard */
            background-color: #999;
            z-index: 1;
        }
        .close_button
        {
            font-family: Verdana, Geneva, sans-serif;
            font-size: medium;
            font-weight: bold;
            float: right;
            color: #666;
            display: block;
            text-decoration: none;
            padding: 0px 3px 0px 3px;
        }
        .web_dialog_overlayMy {
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

        .web_dialogMy {
            display: none;
            position:absolute;
            top:20px;
            left:25%;
            padding:3px;
            width:612px;
            height: auto;
            font-size: 100%;
            font: 13px/20px "Helvetica Neue", Helvetica, Arial, sans-serif;
            color: White;
            z-index: 102;
            -moz-border-radius: 6px;
            background-color: white;
            -webkit-border-radius: 6px;
            border: 1px solid #536376;
        }
        #tblClass
        {
            height: 46px;
            width: 282px;
        }
        #tblDivSet
        {
            display: block;
        }
            
        .lstbx {
            border: 1px solid #d7cece;
            background-color: white;
            width: 596px;
            height: 80px;
            color: #676767;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 13px;
            line-height: 26px;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            padding: 0px 5px 0px 10px;
        }
        .auto-style4 {
            font-family: Arial;
            color: #000;
            line-height: 22px;
            font-weight: normal;
            font-size: 12px;
            padding-right: 1px;
            text-align: left;
            height: 35px;
            width: 21%;
        }
        .auto-style5 {
            height: 35px;
        }
        .auto-style6 {
            width: 300px;
            height: 35px;
        }
        </style>
    <script src="JS/tabber.js"></script>
    <link href="CSS/tabmenu.css" rel="stylesheet" />    
    <script src="JS/jquery-ui.min.js"></script>

    <script src="JS/jquery-1.8.0.min.js"></script>

   
    <script src="../StudentBinder/timePicker/jquery.timeentry.min.js"></script>
    <link href="../StudentBinder/timePicker/jquery.timeentry.css" rel="stylesheet" />
    <script src="../StudentBinder/timePicker/jquery.mousewheel.js"></script>

  
    

    <script src="../Administration/jsDatePick.jquery.min.1.3.js"></script>
    <script src="../Administration/jsDatePick.min.1.3.js"></script>
    <link href="../Administration/jsDatePick_ltr.min.css" rel="stylesheet" />
    <link href="../Administration/jsDatePick_ltr.css" rel="stylesheet" />

    <script type="text/javascript">
        //multipleCal

        function addlistItemsTohd() {
            document.getElementById('<%=(lstbxHolidayDate).ClientID %>').value = '';

           var list = document.getElementById('<%=(lstbxHolidayDate).ClientID %>');
            for (var i = 0; i < list.options.length; ++i) {
                document.getElementById('<%=(HdFldListItems).ClientID %>').value = document.getElementById('<%=(HdFldListItems).ClientID %>').value + ',' + list.options[i].text;
            }

        }
        function AddValToLiast() {

            var flagChkDate = 0;

            $("#<%=(lstbxHolidayDate).ClientID %> > option").each(function () {

                if (this.value == $('#<%=(txtHolidayDate).ClientID %>').val()) {
                    // do stuff if selected
                    alert('Date Allready Present');
                    flagChkDate = 1;
                }
            });
           
            //   $("#sjf option[value='0']").remove();
            // $("input#text1").val($("#ListBox1 option:selected").val());

            if (flagChkDate == 0) {
                if (($('#<%=(txtHolidayDate).ClientID %>').val() != '') && ($('#<%=(txtHolidayName).ClientID %>').val() != '')) {
                    $('#<%=(lstbxHolidayDate).ClientID %>').append('<option value="' + $('#<%=(txtHolidayDate).ClientID %>').val() + '">' + $('#<%=(txtHolidayName).ClientID %>').val() + '&nbsp;&nbsp;&nbsp;-' + $('#<%=(txtHolidayDate).ClientID %>').val() + '</option>');
                    $('#<%=(txtHolidayDate).ClientID %>').val('');
                    $('#<%=(txtHolidayName).ClientID %>').val('');
                }
                else {
                }
            }
        }
       
        function delDate() {

            if ($('#<%=(lstbxHolidayDate).ClientID %> :selected').val() != '') {

                $("#<%=(lstbxHolidayDate).ClientID %> option[value='" + $('#<%=(lstbxHolidayDate).ClientID %> :selected').val() + "']").remove();
            }
            else {
                $("#sjf option[value='0']").remove();
                alert('Select a Date');
            }


        }



      

        function loadJquery() {
            
            $('.txtTime').timeEntry({ showSeconds: true, beforeShow: customRange });


            function customRange(input) {
                                
                if ((input.id == '<%=(txtResMondayStart).ClientID %>') || (input.id == '<%=(txtResMondayEnd).ClientID %>')) {
                   return {
                        
                        minTime: (input.id == '<%=(txtResMondayEnd).ClientID %>' ?
                    $('#<%=(txtResMondayStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtResMondayStart).ClientID %>' ?
                    $('#<%=(txtResMondayEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtResTueStart).ClientID %>') || (input.id == '<%=(txtResTueEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtResTueEnd).ClientID %>' ?
                   $('#<%=(txtResTueStart).ClientID %>').timeEntry('getTime') : null),
                       maxTime: (input.id == '<%=(txtResTueStart).ClientID %>' ?
                   $('#<%=(txtResTueEnd).ClientID %>').timeEntry('getTime') : null)

                   };
                }
                if ((input.id == '<%=(txtResWedStart).ClientID %>') || (input.id == '<%=(txtResWedEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtResWedEnd).ClientID %>' ?
                   $('#<%=(txtResWedStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtResWedStart).ClientID %>' ?
                    $('#<%=(txtResWedEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtResThuStart).ClientID %>') || (input.id == '<%=(txtResThuEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtResThuEnd).ClientID %>' ?
                   $('#<%=(txtResThuStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtResThuStart).ClientID %>' ?
                    $('#<%=(txtResThuEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtResFriStart).ClientID %>') || (input.id == '<%=(txtResFriEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtResFriEnd).ClientID %>' ?
                   $('#<%=(txtResFriStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtResFriStart).ClientID %>' ?
                    $('#<%=(txtResFriEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtResSatStart).ClientID %>') || (input.id == '<%=(txtResSatEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtResSatEnd).ClientID %>' ?
                   $('#<%=(txtResSatStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtResSatStart).ClientID %>' ?
                    $('#<%=(txtResSatEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtResSunStart).ClientID %>') || (input.id == '<%=(txtResSunEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtResSunEnd).ClientID %>' ?
                   $('#<%=(txtResSunStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtResSunStart).ClientID %>' ?
                    $('#<%=(txtResSunEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtDayMondayStart).ClientID %>') || (input.id == '<%=(txtDayMondayEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtDayMondayEnd).ClientID %>' ?
                   $('#<%=(txtDayMondayStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtDayMondayStart).ClientID %>' ?
                    $('#<%=(txtDayMondayEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtDayTueStart).ClientID %>') || (input.id == '<%=(txtDayTueEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtDayTueEnd).ClientID %>' ?
                   $('#<%=(txtDayTueStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtDayTueStart).ClientID %>' ?
                    $('#<%=(txtDayTueEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtDayWedStart).ClientID %>') || (input.id == '<%=(txtDayWedEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtDayWedEnd).ClientID %>' ?
                   $('#<%=(txtDayWedStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtDayWedStart).ClientID %>' ?
                    $('#<%=(txtDayWedEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtDayThuStart).ClientID %>') || (input.id == '<%=(txtDayThuEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtDayThuEnd).ClientID %>' ?
                   $('#<%=(txtDayThuStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtDayThuStart).ClientID %>' ?
                    $('#<%=(txtDayThuEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtDayFriStart).ClientID %>') || (input.id == '<%=(txtDayFriEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtDayFriEnd).ClientID %>' ?
                   $('#<%=(txtDayFriStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtDayFriStart).ClientID %>' ?
                    $('#<%=(txtDayFriEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtDaySatStart).ClientID %>') || (input.id == '<%=(txtDaySatEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtDaySatEnd).ClientID %>' ?
                   $('#<%=(txtDaySatStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtDaySatStart).ClientID %>' ?
                    $('#<%=(txtDaySatEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
                if ((input.id == '<%=(txtDaySunStart).ClientID %>') || (input.id == '<%=(txtDaySunEnd).ClientID %>')) {
                    return {

                        minTime: (input.id == '<%=(txtDaySunEnd).ClientID %>' ?
                   $('#<%=(txtDaySunStart).ClientID %>').timeEntry('getTime') : null),
                        maxTime: (input.id == '<%=(txtDaySunStart).ClientID %>' ?
                    $('#<%=(txtDaySunEnd).ClientID %>').timeEntry('getTime') : null)

                    };
                }
            }

            $('#BtnTimein').click(function () {

                $('#overlayTime').fadeIn('slow', function () {
                    $('#dialogTime').fadeIn('slow');
                });

            });

            $('#close_xMy').click(function () {
                $('#dialogTime').fadeOut('slow');
                $('#overlayTime').fadeOut('slow');

            });


            //dialogHoliday

            $('#BtnCal').click(function () {

                $('#<%=(txtHolidayDate).ClientID %>').val('');
                $('#<%=(txtHolidayName).ClientID %>').val('');

                $('#overlayTime').fadeIn('slow', function () {
                    $('#dialogHoliday').fadeIn('slow');
                });

            });

            $('#close_xDate').click(function () {
                $('#dialogHoliday').fadeOut('slow');
                $('#overlayTime').fadeOut('slow');

            });
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td colspan="4" runat="server" id="tdMsg">
                       
                    </td>
                </tr>
                
                </table>
            


             <table width="100%" id="content">
               
                <tr>
                    <td runat="server" id="td1" style="color: #FF0000" colspan="8">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" colspan="6">&nbsp;
                        <asp:HiddenField ID="HdFldListItems" runat="server" />
                    </td>
                    <td align="right" >
                        <input   type="button" id="BtnTimein" class="NFButton" value="Timing"/>
                        <input   type="button" id="BtnCal" class="NFButton" value="Holiday"/>
                       
                    </td>
                </tr>
                <tr>
                    <td  class="tdText" style="vertical-align:middle;" align="right" style="width:21%">School Name
                    </td>
                    <td align="right" class="style4"><span style="color: #FF0000">*</span>
                    </td>
                    <td align="left" style="width:300px">
                        <asp:TextBox ID="txtSchoolName" runat="server" CssClass="textClass" MaxLength="30" TabIndex="1"></asp:TextBox>
                    </td>
                    
                    <td></td><td colspan="2"></td>
                </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">School Description</td>
                     <td align="right" class="style4"><span style="color: #FF0000">*</span></td>
                     <td align="left" colspan="6">
                         <asp:TextBox ID="txtSchoolDesc" runat="server" Columns="4" CssClass="textClass" onchange="Count(this,100)" onKeyUp="Count(this,100)" Rows="4"  Width="530px" TabIndex="2"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td align="right" class="auto-style4"></td>
                     <td align="right" class="auto-style5"></td>
                     <td align="left" colspan="2" class="auto-style5"><asp:Label ID="lblError" runat="server" Text=""></asp:Label></td>
                     <td colspan="2" class="auto-style5"></td>
                     <td colspan="2" class="auto-style5"></td>
                 </tr>
                 <tr>
                     <td class="head_box" colspan="8">
                        Address Information
                    </td>
                 </tr>
                <tr>
                    <td align="right" class="tdText">Address Line 1</td>
                    <td align="right" class="auto-style5">
                        <span style="color: #FF0000">*</span>
                    </td>
                    <td align="left" class="auto-style6">
                        <asp:TextBox ID="txtAddress1" runat="server" CssClass="textClass" MaxLength="30" TabIndex="3"></asp:TextBox>
                        </td>

                    <td class="tdText">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Address Line 2</td>

                    <td class="auto-style5" style="text-align:right" colspan="2"><span style="color: #FF0000"></span></td>                       
                        <td colspan="2" class="auto-style5">
                            <asp:TextBox ID="txtAddress2" runat="server" CssClass="textClass" MaxLength="40" TabIndex="4"></asp:TextBox>
                    </td>
                </tr>
               
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">Address Line 3</td>
                     <td align="right" class="style4">&nbsp;</td>
                     <td align="left"  style="width:300px">
                         <asp:TextBox ID="txtAddress3" runat="server" CssClass="textClass" MaxLength="40" TabIndex="5" ></asp:TextBox>
                     </td>

                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Country</td>

                     <td class="style5" colspan="2" style="text-align:right"><span style="color: #FF0000">*</span></td>
                     <td colspan="2">
                         <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" cssClass="drpClass" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" TabIndex="6">
                         </asp:DropDownList>
                     </td>
                 </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">State</td>
                     <td align="right" class="style4"><span style="color: #FF0000">*</span></td>
                     <td align="left"  style="width:300px">
                         <asp:DropDownList ID="ddlState" runat="server" cssClass="drpClass" TabIndex="7">
                             <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                         </asp:DropDownList>
                     </td>

                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; City</td>

                     <td class="style5" colspan="2"><span style="color: #FF0000"></span></td>
                     <td colspan="2">
                         <asp:TextBox ID="txtCity" runat="server" CssClass="textClass" TabIndex="8"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">Phone</td>
                     <td align="right" class="style4">&nbsp;</td>
                     <td align="left"  style="width:300px">
                         <asp:TextBox ID="txtHomePhone" runat="server" CssClass="textClass" MaxLength="14" TabIndex="9"></asp:TextBox>
                     </td>

                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Email</td>

                     <td class="style5" colspan="2">&nbsp;</td>
                     <td colspan="2">
                         <asp:TextBox ID="txtEmail" runat="server" CssClass="textClass" MaxLength="25" TabIndex="10"></asp:TextBox>
                     </td>
                 </tr>
               
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">Mobile</td>
                     <td align="right" class="style4">&nbsp;</td>
                     <td align="left"  style="width:300px">
                         <asp:TextBox ID="txtMobile" runat="server" CssClass="textClass" MaxLength="13" OnTextChanged="txtMobile_TextChanged" TabIndex="11"></asp:TextBox>
                     </td>

                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Zip</td>

                     <td class="style5" colspan="2">&nbsp;</td>
                     <td colspan="2">
                         <asp:TextBox ID="txtZip" runat="server" CssClass="textClass" MaxLength="5" TabIndex="12"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;</td>
                     <td align="right" class="style4">&nbsp;</td>
                     <td align="left"  style="width:300px">&nbsp;</td>
                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;</td>
                     <td class="style5" colspan="2">&nbsp;</td>
                     <td colspan="2">&nbsp;</td>
                 </tr>
               
                 <tr>
                     <td class="head_box" colspan="8">
                        District Information
                    </td>
                 </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">District Name</td>
                     <td align="right" class="style4"><span style="color: #FF0000">*</span></td>
                     <td align="left"  style="width:300px">
                         <asp:TextBox ID="txtDisName" runat="server" CssClass="textClass" MaxLength="40" TabIndex="13"></asp:TextBox>
                     </td>

                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; District Contact Person</td>

                     <td class="style5" colspan="2" style="text-align:right"><span style="color: #FF0000">*</span></td>
                     <td colspan="2">
                         <asp:TextBox ID="txtDisContactName" runat="server" CssClass="textClass" MaxLength="40" TabIndex="14"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">District Phone Number</td>
                     <td align="right" class="style4"><span style="color: #FF0000">*</span></td>
                     <td align="left">
                         <asp:TextBox ID="txtDisPhoneNum" runat="server" CssClass="textClass" MaxLength="20" TabIndex="15"></asp:TextBox>
                     </td>

                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Address Line 1</td>

                     <td class="style5" colspan="2" style="text-align:right">&nbsp;</td>
                     <td colspan="2">
                         <asp:TextBox ID="txtDistAddress1" runat="server" CssClass="textClass" MaxLength="40" TabIndex="16"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">Address Line 2</td>
                     <td align="right" class="style4">&nbsp;</td>
                     <td align="left">
                         <asp:TextBox ID="txtDisAddress2" runat="server" CssClass="textClass" MaxLength="40" TabIndex="17"></asp:TextBox>
                     </td>

                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Address Line 3</td>

                     <td class="style5" colspan="2">&nbsp;</td>
                     <td colspan="2">
                         <asp:TextBox ID="txtDisAddress3" runat="server" CssClass="textClass" MaxLength="40" TabIndex="18"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">Country </td>
                     <td align="right" class="style4"><span style="color: #FF0000">*</span></td>
                     <td align="left">
                         <asp:DropDownList ID="ddlDisCountry" runat="server" AutoPostBack="True" cssClass="drpClass" OnSelectedIndexChanged="ddlDisCountry_SelectedIndexChanged" TabIndex="19">
                         </asp:DropDownList>
                     </td>

                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; State </td>

                     <td class="style5" colspan="2" style="text-align:right"><span style="color: #FF0000">*</span></td>
                     <td colspan="2">
                         <asp:DropDownList ID="ddlDisState" runat="server" cssClass="drpClass" TabIndex="20">
                             <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                         </asp:DropDownList>
                     </td>
                 </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">City<span style="color: #FF0000">&nbsp;</span> </td>
                     <td align="right" class="style4">&nbsp;</td>
                     <td align="left">
                         <asp:TextBox ID="txtDisCity" runat="server" CssClass="textClass" TabIndex="21"></asp:TextBox>
                     </td>

                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Phone</td>

                     <td class="style5" colspan="2">&nbsp;</td>
                     <td colspan="2"><asp:TextBox ID="txtDistHomePhone" runat="server"  CssClass="textClass"  MaxLength="15" TabIndex="22"></asp:TextBox></td>
                 </tr>
               
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">Email</td>
                     <td align="right" class="style4">&nbsp;</td>
                     <td align="left"><asp:TextBox ID="txtDistEmail" runat="server"  CssClass="textClass"  MaxLength="25" TabIndex="23"></asp:TextBox></td>

                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Mobile</td>

                     <td class="style5" colspan="2">&nbsp;</td>
                     <td colspan="2"><asp:TextBox ID="txtDistMobile" runat="server"  CssClass="textClass"  OnTextChanged="txtMobile_TextChanged"
                            MaxLength="13" TabIndex="24"></asp:TextBox></td>
                 </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">Zip</td>
                     <td align="right" class="style4">&nbsp;</td>
                     <td align="left">
                         <asp:TextBox ID="txtDistZip" runat="server" CssClass="textClass" MaxLength="5" TabIndex="25"></asp:TextBox>
                     </td>
                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;</td>
                     <td class="style5" colspan="2">&nbsp;</td>
                     <td colspan="2">&nbsp;</td>
                 </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;</td>
                     <td align="right" class="style4">&nbsp;</td>
                     <td align="left">&nbsp;</td>
                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;</td>
                     <td class="style5" colspan="2">&nbsp;</td>
                     <td colspan="2">&nbsp;</td>
                 </tr>
                 <tr>
                    <td align="center" colspan="8">
                       <asp:Button ID="Button_Add" runat="server" CssClass="NFButton" 
                                                    Text="Save" OnClick="Button_Add_Click" OnClientClick="addlistItemsTohd()" TabIndex="26"/>
                        &nbsp&nbsp<asp:Button ID="btnCancel" runat="server"  CssClass="NFButton" Text="Cancel" OnClick="btnCancel_Click" margin="0 0 0 45% !important" display="block"/>
                    </td>
                </tr>
                 <tr>
                     <td align="right"  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;</td>
                     <td align="right" class="style4">&nbsp;</td>
                     <td align="left">&nbsp;</td>
                     <td  class="tdText" style="vertical-align:middle;" style="width:21%">&nbsp;</td>
                     <td class="style5" colspan="2">&nbsp;</td>
                     <td colspan="2">&nbsp;</td>
                 </tr>
               
            </table>

        

        </ContentTemplate>
    </asp:UpdatePanel>
       <div id="overlayTime" class="web_dialog_overlayMy">
        </div>
        <div id="dialogTime" class="web_dialog" style="display:none;margin-top:170px;width:620px;top:-20%;" >
            <div id="sign_up5" style="width: 600px;">

              <h3>  School Timing</h3>
                <hr />

         <div class="tabber" style="width:100%;" id="tabformgoal" runat="server">
                       
                                 <div class="tabbertab" id="formdiv" runat="server" >
                                         <h2 >Residence </h2> 
                                         <asp:UpdatePanel ID="UpdatePanel3" runat="server"><ContentTemplate>   
                                                             
                                             <table style="width: 100%;">
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;" colspan="4">Residence Calendar</td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;" colspan="2">&nbsp;</td>
                                                     <td>Start Time</td>
                                                     <td>End Time</td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Monday<asp:HiddenField ID="HdnResMon" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkResMon" type="checkbox" runat="server" onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResMondayStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResMondayEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Tuesday<asp:HiddenField ID="HdnResTue" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkResTue" type="checkbox" runat="server" onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResTueStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResTueEnd" runat="server" MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Wednesday<asp:HiddenField ID="HdnResWed" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkResWed" type="checkbox" runat="server" onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResWedStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResWedEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Thursday<asp:HiddenField ID="HdnResThu" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkResThu" type="checkbox" runat="server" onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResThuStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResThuEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Friday<asp:HiddenField ID="HdnResFri" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkResFri" type="checkbox" runat="server" onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResFriStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResFriEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Saturday<asp:HiddenField ID="HdnResSat" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkResSat" type="checkbox" runat="server" onclick="EnableTxt(this)" />
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResSatStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResSatEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">Sunday<asp:HiddenField ID="HdnResSun" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkResSun" type="checkbox" runat="server" onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResSunStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtResSunEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;" colspan="2">&nbsp;</td>
                                                     <td>&nbsp;</td>
                                                     <td>&nbsp;</td>
                                                 </tr>
                                             </table>
    
                                         </ContentTemplate></asp:UpdatePanel>                        
                                 </div>       
       
                                 <div class="tabbertab" id="goaldiv" runat="server" >                       
                                        <h2 >                                        
                                          Day
                                        </h2>                           
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server"><ContentTemplate>      
                                                          <table style="width: 100%;">
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;" colspan="4">Day Calendar</td>
                                                 </tr>
                                                              <tr>
                                                                  <td  class="tdText" style="vertical-align:top;" colspan="2">&nbsp;</td>
                                                                  <td>Start Time</td>
                                                                  <td>End Time</td>
                                                              </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Monday<asp:HiddenField ID="HdnDayMon" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkDayMon" type="checkbox" runat="server" onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDayMondayStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false" ></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDayMondayEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Tuesday<asp:HiddenField ID="HdnDayTue" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkDayTue" type="checkbox" runat="server"  onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDayTueStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDayTueEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Wednesday<asp:HiddenField ID="HdnDayWed" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkDayWed" type="checkbox" runat="server" onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDayWedStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDayWedEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Thursday<asp:HiddenField ID="HdnDayThu" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkDayThu" type="checkbox" runat="server"  onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDayThuStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDayThuEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Friday<asp:HiddenField ID="HdnDayFri" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkDayFri" type="checkbox" runat="server"  onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDayFriStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDayFriEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Saturday<asp:HiddenField ID="HdnDaySat" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkDaySat" type="checkbox" runat="server" onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDaySatStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDaySatEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td  class="tdText" style="vertical-align:top;">Sunday<asp:HiddenField ID="HdnDaySun" runat="server" />
                                                     </td>
                                                     <td  class="tdText" style="vertical-align:top;" style="vertical-align:top;">
                                                         <input id="chkDaySun" type="checkbox" runat="server" onclick="EnableTxt(this)"/>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDaySunStart" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                     <td>
                                                         <asp:TextBox ID="txtDaySunEnd" runat="server"  MaxLength="20" Class="txtTime" Enabled="false"></asp:TextBox>
                                                     </td>
                                                 </tr>
                                                              <tr>
                                                                  <td  class="tdText" style="vertical-align:top;" colspan="4"></td>
                                                               
                                                              </tr>
                                             </table>   
                                        </ContentTemplate></asp:UpdatePanel>
                                   </div>  
                        
                   
                        </div>
              
            </div>

         <table style="width:100%"><tr><td style="text-align:center;"><input id="close_xMy" type="button" value="Done" class="NFButton" /></td></tr></table>
        </div>

        <div id="dialogHoliday" class="web_dialog"  style="display:none;margin-top:170px;width:620px;top:-20%;">
            <div id="Div2" style="width: 600px;">

               <h3>Holiday</h3>
                <hr />
             
                   <table style="width: 100%;">
                       <tr>
                           <td colspan="3">
                               <asp:ListBox ID="lstbxHolidayDate" runat="server" CssClass="lstbx" ></asp:ListBox>                               
                           </td>
                           
                       </tr>
                       <tr>
                           <td>Holiday Name</td>
                           <td>Holiday Date</td>
                           <td>&nbsp;</td>
                       </tr>
                       <tr>
                           <td>
                               <asp:TextBox ID="txtHolidayName" runat="server"></asp:TextBox>
                           </td>
                           <td>
                               <asp:TextBox ID="txtHolidayDate" runat="server" onkeydown='allowBackSpace(event,this)' onpaste="return false" onkeypress="return false"></asp:TextBox>                               
                           </td>
                           <td>
                               <asp:Button ID="btnAdd" runat="server" CssClass="NFButton" Text="Add" OnClientClick="AddValToLiast();return false" />
                           </td>
                           
                       </tr>
                       <tr>
                           <td>&nbsp;</td>
                           <td>&nbsp;</td>
                           <td>&nbsp;</td>
                       </tr>
                   </table>
                 
            </div>
          
                               
                         
             <table style="width:100%"><tr><td style="text-align:center;"><input id="close_xDate" type="button" value="Done" class="NFButton" /></td></tr></table>
        </div>
     
    
</asp:Content>
