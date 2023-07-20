<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClinicalSheetNew.aspx.cs" Inherits="StudentBinder_ClinicalSheetNew" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <%-- <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />--%>
    <title>Untitled Document</title>
    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <script src="jsScripts/jq1.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="jsScripts/eye.js"></script>
    <script type="text/javascript" src="jsScripts/layout.js"></script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />


    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="../Administration/JS/jquery-1.8.0.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.widget.js"></script>



    <link href="../Administration/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/jsDatePick.min.1.3.js"></script>
    <link href="../Administration/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/jsDatePick.jquery.min.1.3.js"></script>

    <link href="../Administration/CSS/demos.css" type="text/css" rel="stylesheet" />

    <link href="../Administration/CSS/jquery.ui.base.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/jquery.ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="../Administration/JS/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.fileupload.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.iframe-transport.js" type="text/javascript"></script>

    <script src="../Administration/JS/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />

    <link href="CSS/ClinicalSheet.css" rel="stylesheet" />


    <script type="text/javascript">


        window.onload = function () {
            

            $(function () {
                $(".DatePick").datepicker();
            });

        };



        function freeTextPopup(Divcontent) {

            $('#overlay').fadeIn('fast', function () {
                $('#FreetextPopup').css('top', '5%');
                $('#FreetextPopup').css('left', '33%');
                $('#FreetextPopup').show();
            });
            $('#hdndivname').val(Divcontent.id);
            $('#hdncontent').val(FTB_API['<%=FreeTextBox1.ClientID %>'].SetHtml(Divcontent.innerHTML));
        }

        function loadDateJqry() {
            $(function () {
                $(".DatePick").datepicker();
            });
        }
        //function popupShow() {
        //    $('#overlay').fadeIn('fast', function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#close_x').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); });
        //}

        $(document).ready(function () {
            $("#txtSdate").datepicker({
                dateFormat: "mm/dd/yy",
                maxDate: '0'
            });

            $('#txtEdate').datepicker({
                dateFormat: "mm/dd/yy",
                maxDate: '0'                    
            });

            $('#close_x').click(function () { $('#PopDownload').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); });


            // code by pramod
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
            if ($('#hfFollowUp').length > 0) {
                if ($('#<%=hfFollowUp.ClientID %>').val() != "") {
                    var followup = $('#<%=hfFollowUp.ClientID %>').val();
                    $('#<%=hfFollowUp.ClientID %>').val(followup.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }
            }
            if ($('#hfPreposals').length > 0) {
                if ($('#<%=hfPreposals.ClientID %>').val() != "") {
                    var proposal = $('#<%=hfPreposals.ClientID %>').val();
                    $('#<%=hfPreposals.ClientID %>').val(proposal.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }
            }
            if ($('#hfAcademic').length > 0) {
                if ($('#<%=hfAcademic.ClientID %>').val() != "" && $('#<%=hfAcademic.ClientID %>').length > 0) {
                    var academic = $('#<%=hfAcademic.ClientID %>').val();
                    $('#<%=hfAcademic.ClientID %>').val(academic.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }
            }
            if ($('#hfClinical').length > 0) {
                if ($('#<%=hfClinical.ClientID %>').val() != "" && $('#<%=hfClinical.ClientID %>').length > 0) {
                    var clinical = $('#<%=hfClinical.ClientID %>').val();
                    $('#<%=hfClinical.ClientID %>').val(clinical.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }
            }
            if ($('#hfCommunity').length > 0) {
                if ($('#<%=hfCommunity.ClientID %>').val() != "" && $('#<%=hfCommunity.ClientID %>').length > 0) {
                    var community = $('#<%=hfCommunity.ClientID %>').val();
                    $('#<%=hfCommunity.ClientID %>').val(community.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }
            }
            if ($('#hfOther').length > 0) {
                if ($('#<%=hfOther.ClientID %>').val() != "" && $('#<%=hfOther.ClientID %>').length > 0) {
                    var other = $('#<%=hfOther.ClientID %>').val();
                    $('#<%=hfOther.ClientID %>').val(other.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }
            }



        });
        function HideWait() {
            $('#fullContents').fadeIn('fast');
            $('.loading').fadeOut('fast');

            $('#overlay').fadeIn('fast', function () {
                $('#PopDownload').css('top', '5%');
                $('#PopDownload').show();
            });


        }
        function showWait() {
            $('#fullContents').fadeOut('fast');
            $('.loading').fadeIn('fast');

        }

        function txtfree() {
            $('#FreetextPopup').animate({ top: "-300%" }, function () {
                $('#overlay').fadeOut('slow');
            });

            var content1 = FTB_API['<%=FreeTextBox1.ClientID %>'].GetHtml();
            var divid = document.getElementById('hdndivname').value;
            GetFreetextval(content1, divid);
            FTB_API['<%=FreeTextBox1.ClientID %>'].SetHtml("");
        }


        function GetFreetextval(content, divid) {
            if (divid == 'txtFollowUp') {
                document.getElementById('<%=txtFollowUp.ClientID %>').innerHTML = "";
                document.getElementById('<%=txtFollowUp.ClientID %>').innerHTML = content;
                $('#<%=hfFollowUp.ClientID %>').val(content.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
            }
            else if (divid == 'txtPreposals') {
                document.getElementById('<%=txtPreposals.ClientID %>').innerHTML = "";
                    document.getElementById('<%=txtPreposals.ClientID %>').innerHTML = content;
                    $('#<%=hfPreposals.ClientID %>').val(content.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }
                else if (divid == 'txtAcademic') {
                    document.getElementById('<%=txtAcademic.ClientID %>').innerHTML = "";
                    document.getElementById('<%=txtAcademic.ClientID %>').innerHTML = content;
                    $('#<%=hfAcademic.ClientID %>').val(content.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }
                else if (divid == 'txtClinical') {
                    document.getElementById('<%=txtClinical.ClientID %>').innerHTML = "";
                    document.getElementById('<%=txtClinical.ClientID %>').innerHTML = content;
                    $('#<%=hfClinical.ClientID %>').val(content.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }
                else if (divid == 'txtCommunity') {
                    document.getElementById('<%=txtCommunity.ClientID %>').innerHTML = "";
                    document.getElementById('<%=txtCommunity.ClientID %>').innerHTML = content;
                    $('#<%=hfCommunity.ClientID %>').val(content.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }
                else if (divid == 'txtOther') {
                    document.getElementById('<%=txtOther.ClientID %>').innerHTML = "";
                    document.getElementById('<%=txtOther.ClientID %>').innerHTML = content;
                    $('#<%=hfOther.ClientID %>').val(content.replace(/</g, '&lt;').replace(/>/g, "&gt;"));
                }


            }
            function Delete() {
                var flag;
                flag = confirm("Are you sure you want to delete this coversheet?");
                return flag;

            }
    </script>


    <script type="text/javascript">

        $(document).ready(function () {
            $('#CancalGen').click(function () {
                $('#dialog').animate({ top: "-300%" }, function () {
                    $('#overlay').fadeOut('slow');
                });
            });



        });

        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 275);
        }

        //$(function () {
        //    // Patch fractional .x, .y form parameters for IE10.
        //    if (typeof (Sys) !== 'undefined' && Sys.Browser.agent === Sys.Browser.InternetExplorer && Sys.Browser.version === 10) {
        //        Sys.WebForms.PageRequestManager.getInstance()._onFormElementActive = function Sys$WebForms$PageRequestManager$_onFormElementActive(element, offsetX, offsetY) {
        //            if (element.disabled) {
        //                return;
        //            }
        //            this._activeElement = element;
        //            this._postBackSettings = this._getPostBackSettings(element, element.name);
        //            if (element.name) {
        //                var tagName = element.tagName.toUpperCase();
        //                if (tagName === 'INPUT') {
        //                    var type = element.type;
        //                    if (type === 'submit') {
        //                        this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
        //                    }
        //                    else if (type === 'image') {
        //                        this._additionalInput = encodeURIComponent(element.name) + '.x=' + Math.floor(offsetX) + '&' + encodeURIComponent(element.name) + '.y=' + Math.floor(offsetY);
        //                    }
        //                }
        //                else if ((tagName === 'BUTTON') && (element.name.length !== 0) && (element.type === 'submit')) {
        //                    this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
        //                }
        //            }
        //        };
        //    }
        //});
        function h2click(h2, IEPid) {
            document.getElementById('<%=tdMsg.ClientID %>').innerHTML = "";
            // if section is already open, return false
            if ($(h2).is('.active')) {
                //$(h2).removeClass("active");
                //$(h2).next('div').slideToggle('fast');
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

            //SetIepID(IEPid);
            return false;
        }

        function generateReport(Date) {



            //$.ajax(
            //{
                //type: "POST",
                //url: '<%= ResolveUrl("ClinicalSheetNew.aspx/generateReportNew")%>',
                //data: "{'ID':'" + Date + "'}",
                //contentType: "application/json; charset=utf-8",
                //dataType: "json",
                //success: function (data) { },
                //error: function (request, status, error) {
                //    alert("Report Failed To Load. Error:"+error);
                //}
            //});
            var dataString = {'value':Date}
            $.ajax(
                 {
                     type: "POST",
                     url: "ClinicalSheetNew.aspx/generateReportNew",
                     data: dataString,
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,
                     success: function (data) {
                     },
                     error: function (request, status, error) {
                         alert("Error:"+error);
                     }
                 });
        }

    </script>
</script>
    <script>
        function allowBackSpace(e, obj) {
            var evt = e || window.event;
            if (evt.keyCode == 8) {
                obj.value = '';
            }
        }
    </script>


    <style type="text/css">
        .FreeTextDivContent {
            width: 96%;
            min-height: 100px;
            height: auto;
            padding: 2%;
            border: 1px solid rgb(228, 228, 228);
            border-radius: 8px 8px 8px 8px;
        }

            .FreeTextDivContent ul li {
                list-style: disc outside none !important;
                margin: 0;
                padding: 0;
                text-align: left;
            }

            .FreeTextDivContent ol li {
                list-style: decimal outside none !important;
                margin: 0;
                padding: 0;
                text-align: left;
            }

        ol, ul {
            padding-left: 0;
            width: 100%;
        }

        .auto-style2 {
            height: 23px;
        }

        p.MHead1 {
            margin: 3.0pt 0in;
            page-break-after: avoid;
            punctuation-wrap: simple;
            text-autospace: none;
            font-size: 9.0pt;
            font-family: "Arial Black","sans-serif";
            text-decoration: underline;
            text-underline: single;
        }

        .auto-style3 {
            height: 19px;
        }

        .auto-style4 {
            width: 20%;
            height: 19px;
        }

        .auto-style5 {
            width: 80%;
            height: 19px;
        }

        .mainheadingpanel {
            color: #4F5050;
            float: left;
            font-family: Oswald;
            font-size: 17px;
            font-weight: normal;
            height: 35px;
            line-height: 35px;
            margin: 0;
            text-align: left;
        }

        .web_dialog_datepop {
            /*display: block;
    position: fixed;
    top: -100%;
    left: 40%;
    margin-left: -190px;
    z-index: 102;
        */
            display: block;
            position: fixed;
            width: 800px;
            height: auto;
            top: -100%;
            left: 40%;
            margin-left: -190px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 999;
            background: #f8f7fc url(../images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
        }
    </style>



    <style type="text/css">
        .ui-datepicker {
            font-size: 8pt !important;
        }
         .toolTipImg{  
           position: absolute;
           display: inline-block; 
            margin-top: -3px;
           }
       .tooltiptext {
            visibility: hidden;
            width: 540px;
            background-color: whitesmoke;
            color: black;
            text-align: center;
            padding: 5px 0;
            position:fixed;
            z-index: 1;
            border-radius:3px 3px;
            margin-left:10px;
            font-weight:normal;
            font-size:12px;
           
        }
        .toolTipImg:hover + .tooltiptext {
            visibility: visible;
        }
        .toolTipImg:hover  .tooltiptext {
            visibility: visible;
        }
    </style>

    <style type="text/css">
        hr {
            border: 1px dashed gray;
        }

        .style1 {
            width: 864px;
        }

        .style2 {
            width: 20%;
        }

        .widthSettd {
            width: 20px;
        }

        .auto-style6 {
            width: 100%;
            height: 19px;
        }

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

        table {
        }

        input[type="text"] {
            width: 80% !important;
        }

        .RowStyle1 {
            width: 100%;
        }

            .RowStyle1 td {
                border-bottom: 0 solid #e9e9e9;
                text-align: center;
            }

        .header-height
        {
            height:15px;
            padding-left:5px;
        }
        .grmb {
            background: url("images/lbtngrngray1.PNG") no-repeat scroll 0 0 rgba(0, 0, 0, 0);
            height: 22px;
            padding-top: 8px;
            padding-left: 20px;
            text-decoration: none;
            width: 210px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
        <div>
            <div>
                <div class="loading">
                    <div class="innerLoading">
                        <img src="../Administration/images/load.gif" alt="loading" />
                        Please Wait...
                    </div>
                </div>
                <div id="fullContents">
                    <div class="boxContainerContainer">
                        <div class="clear"></div>

                        <div class="clear"></div>
                        <!-------------------------Top Container Start----------------------->
                        <div class="itlepartContainer">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 20%;">
                                        <h2>1 <span>Choose Date</span></h2>
                                    </td>
                                    <td style="width: 25%;">
                                        <h2>2 <span>Clinical Coversheet Report</span></h2>
                                    </td>
                                    <td style="width: 30%; text-align: right;">
                                        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="NFButton" Style="float: right" OnClick="btnExport_Click" OnClientClick="showWait();" />

                                    </td>
                                    <td>
                                        <asp:Button ID="btnGenDelete" runat="server" Text="Delete" CssClass="NFButton" Style="float: right;" OnClick="btnGenDelete_Click" OnClientClick="javascript:return Delete();"/>
									</td>
                                    <td>
                                        <asp:Button ID="btnGenNewSheet" runat="server" CssClass="NFButton" ToolTip="Create new sheet" alt="Create New Document" OnClick="btnGenNewClinicalSheet_Click" Text="Create New Coversheet" Style="width: auto;" />
                                        
                                    </td>
                                </tr>
                            </table>



                        </div>
                        <!-------------------------Top Container End----------------------->

                        <!-------------------------Middle Container start----------------------->
                        <div class="btContainerPart">
                            <!------------------------------------MContainer Start----------------------------------->

                            <div class="mBxContainer">
                                <h3>Date List</h3>
                                <div class="nobdrrcontainer">
                                    <%--      <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>--%>
                                    <asp:DataList ID="DataList1" runat="server" onitemdatabound="DataList1_ItemDataBound">
                                    <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem,"EDate") %>' style="display:none;"></asp:Label>
                                        <li style='position: static;' class='accordion'>
                                            <h2 class='BG' onclick='h2click(this,'<%# DataBinder.Eval(Container.DataItem,"EDate") %>');'><a id='<%# DataBinder.Eval(Container.DataItem,"EDate") %>' class='kk' href='#' ><%# DataBinder.Eval(Container.DataItem,"EDate") %></a></h2>
                                                <div style='display: none;' class='wrapper nomar'>
                                                    <div class='nobdrrcontainer'>
                                                        <asp:DataList ID="dlDateList" runat="server" OnItemCommand="dlDateList_ItemCommand">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDate" CssClass="grmb" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                            <div class='clear'></div>
                                                    </div>
                                                            <div class='clear'></div>
                                                </div>
                                       </li>
                                    </ItemTemplate>
                                    </asp:DataList>

                                    <%--<asp:DataList ID="dlDateList" runat="server" OnItemCommand="dlDateList_ItemCommand">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDate" CssClass="grmb" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>'></asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:DataList>--%>
                                    <%--   </ContentTemplate>
                            </asp:UpdatePanel>--%>
                                </div>




                                <div class="clear"></div>
                            </div>
                            <!------------------------------------MContainer End----------------------------------->

                            <!------------------------------------End Container Start----------------------------------->



                            <div class="righMainContainer large" id="MainDiv" runat="server" visible="true">



                                <table style="width: 100%">
                                    <tr>
                                        <td id="tdMsg" runat="server"></td>                                        
                                    </tr>
                                    <tr>
                                    <td id="tdMsgDel" runat="server"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PanelMain" runat="server">
                                                <div>
                                                    <asp:HiddenField ID="hdFldCvid" runat="server" />

                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="ClientName" HeaderStyle-CssClass="header-height">
					                                                                <ItemTemplate>
						                                                                <table class="RowStyle1">
							                                                                <tr><td style="text-align: left;"><asp:Label ID="Label2" runat="server" Text='<%# Eval("StdName") %>' ></asp:Label></td></tr>
						                                                                </table>
					                                                                </ItemTemplate>					                                                                
					                                                                <HeaderStyle Width="20%"  BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
					                                                                <ItemStyle Width="20%" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
				                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Location" HeaderStyle-CssClass="header-height">
					                                                                <ItemTemplate>
						                                                                <table class="RowStyle1">
							                                                                <tr><td style="text-align: left;"><asp:Label ID="Label4" runat="server" Text='<%# Eval("Location") %>' ></asp:Label></td></tr>
						                                                                </table>
					                                                                </ItemTemplate>					                                                                
					                                                                <HeaderStyle Width="20%" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid" />
					                                                                <ItemStyle Width="20%" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
				                                                                </asp:TemplateField>
				                                                                <asp:TemplateField HeaderText="Program" HeaderStyle-CssClass="header-height">
					                                                                <ItemTemplate>
						                                                                <table class="RowStyle1">
							                                                                <tr><td style="text-align: left;"><asp:Label ID="Label6" runat="server" Text='<%# Eval("Program") %>' ></asp:Label></td></tr>
						                                                                </table>
					                                                                </ItemTemplate>					                                                                
					                                                                <HeaderStyle Width="20%" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
					                                                                <ItemStyle Width="20%"  BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
				                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="IEP Start and End Dates" HeaderStyle-CssClass="header-height">
                                                                                    <HeaderTemplate>
                                                                                        <table class="RowStyle1">
							                                                                <tr><td style="text-align: center;"><asp:Label ID="Label13" runat="server" Text="IEP Start and End Dates" ></asp:Label></td></tr>
						                                                                </table>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td style="text-align: center;border-right: 1px black solid;">
                                                                                                    <asp:TextBox ID="txtDate1" runat="server" Text='<%# Eval("IepStDate") %>' Class="DatePick" onkeydown="return false" onpaste="return false"></asp:TextBox>
                                                                                                    <asp:HiddenField ID="iepstdid" runat="server" Value='<%# Eval("StdtId") %>' ></asp:HiddenField>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtDate2" runat="server" Text='<%# Eval("IepEnDate") %>' Class="DatePick" onkeydown="return false" onpaste="return false"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>                                                                                    
					                                                                <HeaderStyle Width="24%"  BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
					                                                                <ItemStyle Width="24%" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Period of Assessment" HeaderStyle-CssClass="header-height">
                                                                                    <HeaderTemplate>
                                                                                        <table class="RowStyle1">
							                                                                <tr><td style="text-align: center;"><asp:Label ID="Label11" runat="server" Text="Period of Assessment" ></asp:Label></td></tr>
						                                                                </table>
                                                                                    </HeaderTemplate>
					                                                                <ItemTemplate>
						                                                                <table class="RowStyle1">
							                                                                <tr>
								                                                                <td style="text-align: center;border-right: 1px black solid;">
                                                                                                     <asp:Label ID="Label9" runat="server" Text='<%# Eval("AssmntStdate") %>' ></asp:Label>
								                                                                </td>
								                                                                <td style="text-align: center;">
                                                                                                     <asp:Label ID="Label10" runat="server" Text='<%# Eval("AssmntEnddate") %>' ></asp:Label>
								                                                                </td>
							                                                                </tr>
						                                                                </table>
					                                                                </ItemTemplate>
					                                                                <HeaderStyle Width="16%" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
					                                                                <ItemStyle Width="16%" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
				                                                                </asp:TemplateField>
                                                                            </Columns>

                                                                            <EditRowStyle BackColor="#2461BF" />
			                                                                <HeaderStyle BackColor="#03507d" Font-Bold="True" ForeColor="White" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
			                                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
			                                                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
			                                                                <RowStyle CssClass="RowStyle" BackColor="#EFF3FB" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid"/>
			                                                                <AlternatingRowStyle CssClass="AltRowStyle" BackColor="White" />
			                                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
			                                                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
			                                                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
			                                                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
			                                                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                                            <RowStyle CssClass="RowStyle" />
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" class="mainheadingpanel">Behavior Data Summary:</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">

                                                                <asp:GridView ID="gVBehavior" runat="server" Style="border: 0; font-size: 10px;" AutoGenerateColumns="False" EmptyDataText="No Data Found..." Width="100%" CellPadding="4" Font-Bold="False" ForeColor="#333333" GridLines="None">


                                                                    <AlternatingRowStyle BackColor="White" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Identify behaviors targeted for deceleration and acceleration (List IEP objectives first)">

                                                                            <ItemTemplate>
                                                                                <table class="RowStyle1">
                                                                                    <tr>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Behaviour") %>' Font-Bold="True"></asp:Label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:Label ID="Label2" runat="server" Text="Frequency"></asp:Label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: left;">
                                                                                            <asp:Label ID="Label3" runat="server" Text="Duration (Minutes)"></asp:Label></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <FooterStyle Width="20%" />
                                                                            <HeaderStyle Width="20%" />
                                                                            <ItemStyle Width="20%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <div style="text-align: center">
                                                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/StudentBinder/images/Level1.jpg" />
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div style="text-align: center">
                                                                                    <table class="RowStyle1">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label runat="server" Text=""></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("FRQSharpDecrease")  %>'></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("DURSharpDecrease") %>'></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <div style="text-align: center">
                                                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/StudentBinder/images/Level2.jpg" />
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div style="text-align: center">
                                                                                    <table class="RowStyle1">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label runat="server" Text=""></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label6" runat="server" Text='<%#Eval("FRQSlightDecrease")  %>'></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label7" runat="server" Text='<%#Eval("DURSlightDecrease") %>'></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <div style="text-align: center">
                                                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/StudentBinder/images/Level3.jpg" />
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div style="text-align: center">
                                                                                    <table class="RowStyle1">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label runat="server" Text=""></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label8" runat="server" Text='<%#Eval("FRQStable") %>'></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label9" runat="server" Text='<%#Eval("DURStable") %>'></asp:Label></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <div style="text-align: center">
                                                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/StudentBinder/images/Level4.jpg" />
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div style="text-align: center">
                                                                                    <table class="RowStyle1">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label runat="server" Text=""></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label10" runat="server" Text='<%#Eval("FRQSlightIncrease")  %>'></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label11" runat="server" Text='<%#Eval("DURSlightIncrease")  %>'></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <div style="text-align: center">
                                                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/StudentBinder/images/Level5.jpg" />
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div style="text-align: center">
                                                                                    <table class="RowStyle1">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label runat="server" Text=""></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label12" runat="server" Text='<%#Eval("FRQSharpIncrease")  %>'></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label13" runat="server" Text='<%# Eval("DURSharpIncrease")  %>'></asp:Label></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField >
                                                                             <HeaderTemplate ><img class="toolTipImg" src="images/toolTipQMark.png" style="height:15px;width:15px;margin-left:-8px"  />
                                                                                 <span class="tooltiptext">Trend selected with an "X" represents the slope of the most recent trendline segment for the selected date range. If the date range includes condition lines or breaks in data over 3 days, the trend represents only the most recent period of unbroken data. 
                                                                                 </span>
                                                                                     <br />
                                                                                 <br />
​                                                                                  
                                                                            </HeaderTemplate>
                                                                             </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Number of days of data<br />  and summary unit">

                                                                            <ItemTemplate>
                                                                                <table class="RowStyle1">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label14" runat="server" Text='<%# Eval("DAYCOUNT") %>'></asp:Label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label15" runat="server" Text='<%# Eval("FRQSlope") %>'></asp:Label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label16" runat="server" Text='<%# Eval("DURSlope") %>'></asp:Label></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <FooterStyle Width="15%" />
                                                                            <HeaderStyle Width="15%" />
                                                                            <ItemStyle Width="15%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="IEP Objective">       
                                                                            <ItemTemplate>
                                                                                <table class="RowStyle1">
                                                                                    <tr>
                                                                                        <td>                                                                                            
                                                                                            <asp:TextBox ID="TextArea1" runat="server" TextMode="multiline" Columns="50" Rows="50" Width="120px" Height="80px" Text='<%# Eval("IEPObj") %>' style="margin-left:-20px;resize:none;overflow-y:scroll"></asp:TextBox>
                                                                                            <asp:HiddenField ID="behvid" runat="server" Value='<%# Eval("MeasurementId") %>' ></asp:HiddenField>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <FooterStyle Width="16%" />
                                                                            <HeaderStyle Width="16%" />
                                                                            <ItemStyle Width="16%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText=" Baseline of current <br />performance level">
                                                                            <ItemTemplate>
                                                                                <table class="RowStyle1">
                                                                                    <tr>
                                                                                        <td>                                                                                            
                                                                                            <asp:TextBox ID="TextArea2" runat="server" TextMode="multiline" Columns="50" Rows="5" Width="120px" Height="80px" Text='<%# Eval("BasPerlvl") %>' style="margin-left:-20px;resize:none;overflow-y:scroll"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <FooterStyle Width="16%" />
                                                                            <HeaderStyle Width="16%" />
                                                                            <ItemStyle Width="16%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Last two IOA points" >
                                                                            <ItemTemplate>
                                                                                   <table class="RowStyle1">
                                                                                       <tr>
                                                                                           <td>
                                                                                               <asp:Label ID="Label17" runat="server" Text='<%# Eval("IOAPOINTS").ToString().Split(Convert.ToChar("-"),Convert.ToChar("_")).Length >= 1 ? Eval("IOAPOINTS").ToString().Split(Convert.ToChar("-"),Convert.ToChar("_"))[0] : "" %>' Font-Size="8pt" Font-Underline="true"></asp:Label>
                                                                                               <asp:Label ID="Label18" runat="server" Text='<%# Eval("IOAPOINTS").ToString().Split(Convert.ToChar("-"),Convert.ToChar("_")).Length >= 2 ? Eval("IOAPOINTS").ToString().Split(Convert.ToChar("-"),Convert.ToChar("_"))[1] : "" %>' Font-Size="8pt"></asp:Label></br></br>
                                                                                               <asp:Label ID="Label19" runat="server" Text='<%# Eval("IOAPOINTS").ToString().Split(Convert.ToChar("-"),Convert.ToChar("_")).Length >= 3 ? Eval("IOAPOINTS").ToString().Split(Convert.ToChar("-"),Convert.ToChar("_"))[2] : "" %>' Font-Size="8pt" Font-Underline="true"></asp:Label></br>
                                                                                               <asp:Label ID="Label20" runat="server" Text='<%# Eval("IOAPOINTS").ToString().Split(Convert.ToChar("-"),Convert.ToChar("_")).Length >= 4 ? Eval("IOAPOINTS").ToString().Split(Convert.ToChar("-"),Convert.ToChar("_"))[3] : "" %>' Font-Size="8pt"></asp:Label>
                                                                                           </td>
                                                                                       </tr>
                                                                                   </table>
                                                                            </ItemTemplate>
                                                                            <FooterStyle Width="10%" />
                                                                            <HeaderStyle Width="10%" />
                                                                            <ItemStyle Width="10%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EditRowStyle BackColor="#2461BF" />
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <HeaderStyle BackColor="#03507d" Font-Bold="True" ForeColor="White" />
                                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />

                                                                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />

                                                                    <RowStyle CssClass="RowStyle" BackColor="#EFF3FB" />
                                                                    <AlternatingRowStyle CssClass="AltRowStyle" BackColor="White" />

                                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />


                                                                </asp:GridView>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="mainheadingpanel" colspan="3">Setting Events and Program Changes :</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" class="tdText">Briefly identify variables or factors in the following areas that may help explain behavior change (Include dates and list most recent first).</td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>

                                                        <tr>
                                                            <td colspan="3">

                                                                <asp:GridView ID="grdGraphData" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found..." Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Type" HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderStyle-CssClass="HeaderStyle" ItemStyle-CssClass="tdText">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("Eventname") %>'></asp:Label>
                                                                            </ItemTemplate>

                                                                            <HeaderStyle CssClass="HeaderStyle" Width="30%" />
                                                                            <ItemStyle CssClass="tdText" Width="30%" />

                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="70%" ItemStyle-Width="70%" HeaderStyle-CssClass="HeaderStyle" ItemStyle-CssClass="tdText">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Eventdata") %>'></asp:Label>
                                                                            </ItemTemplate>

                                                                            <HeaderStyle CssClass="HeaderStyle" Width="70%" />
                                                                            <ItemStyle CssClass="tdText" Width="70%" />

                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <EditRowStyle BackColor="#2461BF" />
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />

                                                                    <RowStyle CssClass="RowStyle" BackColor="#EFF3FB" />
                                                                    <AlternatingRowStyle CssClass="AltRowStyle" BackColor="White" />
                                                                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />

                                                                </asp:GridView>

                                                            </td>
                                                        </tr>

                                                        <%-- <tr>
                                                        <td class="tdText"><b>Type</b></td>
                                                        <td colspan="2" class="tdText"><b>Description</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdText">Phaselines</td>
                                                        <td colspan="2" class="tdText">
                                                            <asp:Label ID="lblPhaseLine" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdText">Condition lines</td>
                                                        <td colspan="2" class="tdText">
                                                            <asp:Label ID="lblConditionLines" runat="server"></asp:Label>%
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdText">Arrow notes</td>
                                                        <td colspan="2" class="tdText">
                                                            <asp:Label ID="lblArrowNotes" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="auto-style2"></td>
                                                        <td class="auto-style2"></td>
                                                        <td class="auto-style2"></td>
                                                    </tr>--%>
                                                        <%--start--%>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr runat="server" id="hdTxtBx1">
                                                            <td colspan="3" class="mainheadingpanel">Academic Status :</td>
                                                        </tr>
                                                        <tr runat="server" id="hdTxtBx2">
                                                            <td class="auto-style2" colspan="3">

                                                                <div id="txtAcademic" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); freeTextPopup(this);"></div>
                                                                <asp:HiddenField ID="hfAcademic" Value="" runat="server"></asp:HiddenField>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr runat="server" id="hdTxtBx3">
                                                            <td colspan="3" class="mainheadingpanel">Clinical Status :</td>
                                                        </tr>
                                                        <tr runat="server" id="hdTxtBx4">
                                                            <td class="auto-style2" colspan="3">

                                                                <div id="txtClinical" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); freeTextPopup(this);"></div>
                                                                <asp:HiddenField ID="hfClinical" Value="" runat="server"></asp:HiddenField>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr runat="server" id="hdTxtBx5">
                                                            <td colspan="3" class="mainheadingpanel">Community Outings :</td>
                                                        </tr>
                                                        <tr runat="server" id="hdTxtBx6">
                                                            <td class="auto-style2" colspan="3">

                                                                <div id="txtCommunity" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); freeTextPopup(this);"></div>
                                                                <asp:HiddenField ID="hfCommunity" Value="" runat="server"></asp:HiddenField>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr runat="server" id="hdTxtBx7">
                                                            <td colspan="3" class="mainheadingpanel">Other :</td>
                                                        </tr>
                                                        <tr runat="server" id="hdTxtBx8">
                                                            <td class="auto-style2" colspan="3">

                                                                <div id="txtOther" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); freeTextPopup(this);"></div>
                                                                <asp:HiddenField ID="hfOther" Value="" runat="server"></asp:HiddenField>
                                                            </td>
                                                        </tr>
                                                        <%--end--%>

                                                        <tr>
                                                            <td class="mainheadingpanel" colspan="3">Assessment Tool Usage :</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" class="tdText">Identify last three assessment tools that were implemented (most recent first) and apparent functions.</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td style="width: 20%;" class="tdText"><b>Target Behavior</b></td>
                                                                        <td style="width: 20%;" class="tdText"><b>Function(s)</b></td>
                                                                        <td style="width: 60%;" class="tdText"><b>Analysis Tools and Dates</b></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <br />
                                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:GridView ID="gVAssmntTool" runat="server" AutoGenerateColumns="False" ShowFooter="True" OnRowCommand="gVAssmntTool_RowCommand" OnRowDeleting="gVAssmntTool_RowDeleting" GridLines="None" CellPadding="4" Width="100%">
                                                                                        <AlternatingRowStyle CssClass="AltRowStyle" />
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Target Behavior">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtTargetBehavior" runat="server" Text='<%# Eval("TargetBehavior") %>'></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Function(s)">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtFunction" runat="server" Text='<%# Eval("Function") %>'></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Analysis Tools and Dates">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtAnalysisTools" runat="server" Text='<%# Eval("AnalysisTools") %>'></asp:TextBox>
                                                                                                </ItemTemplate>

                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton ID="btnDelRow" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" />
                                                                                                    <asp:ImageButton ID="btnAddRow" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>



                                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                                        <HeaderStyle CssClass="HeaderStyle" ForeColor="White" />



                                                                                        <RowStyle CssClass="RowStyle" />



                                                                                    </asp:GridView>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" class="mainheadingpanel">Preference Assessments and Reinforcement Surveys :</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" class="tdText">Identify date of most recent preference assessment or reinforcement survey.</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gVAssmtReinfo" runat="server" AutoGenerateColumns="False" ShowFooter="True" OnRowCommand="gVAssmtReinfo_RowCommand" OnRowDeleting="gVAssmtReinfo_RowDeleting" GridLines="None" Width="100%">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderStyle-Width="45%">
                                                                                    <HeaderTemplate>Date</HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtDate" runat="server" Text='<%# Eval("Date") %>' Class="DatePick" onkeypress="return false"  onpaste="return false" onkeydown='allowBackSpace(event,this)'></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="45%">
                                                                                    <HeaderTemplate>Tool Utilized</HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtToolUtilized" runat="server" Text='<%# Eval("ToolUtilized") %>'></asp:TextBox>
                                                                                    </ItemTemplate>

                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="btnDelRow" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" />
                                                                                        <asp:ImageButton ID="btnAddRow" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />
                                                                                    </ItemTemplate>


                                                                                </asp:TemplateField>
                                                                            </Columns>

                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                            <HeaderStyle CssClass="HeaderStyle" ForeColor="White" />



                                                                            <RowStyle CssClass="RowStyle" />
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" class="mainheadingpanel">Follow-Up :</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="auto-style2" colspan="3">

                                                                <div id="txtFollowUp" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); freeTextPopup(this);"></div>
                                                                <asp:HiddenField ID="hfFollowUp" Value="" runat="server"></asp:HiddenField>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" class="mainheadingpanel">Proposals :</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">

                                                                <div id="txtPreposals" runat="server" class="FreeTextDivContent" onclick="scrollToTop(); freeTextPopup(this);"></div>
                                                                <asp:HiddenField ID="hfPreposals" Value="" runat="server"></asp:HiddenField>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" class="mainheadingpanel">Recommended Changes :</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="auto-style2" colspan="3">
                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gVRecChange" runat="server" AutoGenerateColumns="False" ShowFooter="True" OnRowCommand="gVRecChange_RowCommand" OnRowDeleting="gVRecChange_RowDeleting" GridLines="None" Width="100%">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Recommendation">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtRecomd" runat="server" Text='<%# Eval("Recommendation") %>'></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Timeline">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtTimeLine" runat="server" Text='<%# Eval("Timeline") %>'></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Person Responsible">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtPerRes" runat="server" Text='<%# Eval("Person Responsible") %>'></asp:TextBox>
                                                                                    </ItemTemplate>

                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="btnDelRow" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" />
                                                                                        <asp:ImageButton ID="btnAddRow" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                            <HeaderStyle CssClass="HeaderStyle" ForeColor="White" />
                                                                            <RowStyle CssClass="RowStyle" />
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" class="mainheadingpanel">Bi-Weekly Clinical Signatures of Review :</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td style="width: 30%" class="tdText">Program Coordinator</td>
                                                                        <td style="width: 33%" class="tdText">
                                                                            <asp:TextBox ID="txtPgmCordntr" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td style="width: 10%" class="tdText">Date</td>
                                                                        <td style="width: 30%" class="tdText">
                                                                            <asp:TextBox ID="txtDatePgnCord" runat="server" Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="tdText">Educational Coordinator / Lead Teacher</td>
                                                                        <td class="tdText">
                                                                            <asp:TextBox ID="txteduCodntr" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td class="tdText">Date</td>
                                                                        <td class="tdText">
                                                                            <asp:TextBox ID="txtDateEduCord" runat="server" Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="tdText">BCBA Clinician</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtBCBA" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td class="tdText">Date</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDateBCBACord" runat="server" Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" style="text-align: center">
                                                                <asp:Button ID="btnSave" runat="server" Text="Save" Width="80px" CssClass="NFButton" OnClick="btnSave_Click" OnClientClick="javascript: scrollToTop();"/></td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>

                                <div id="Div7">
                                </div>

                                <div id="overlay" class="web_dialog_overlay"></div>

                                <div id="dialog" class="web_dialog_datepop" style="width: 900px; margin-left: -22%;">

                                    <div id="sign_up5" style="width: 100%; margin-left: 10px">
                                        <table style="width: 100%">
                                            <tr>
                                                <td runat="server" id="tdMessage" class="tdText" colspan="5"></td>
                                            </tr>


                                            <tr>
                                                <td style="width: 20%" class="tdText">Start Date:</td>
                                                <td style="width: 1%"><span style="color: red">*</span></td>
                                                <td style="width: 35%" class="tdText">
                                                    <asp:TextBox ID="txtSdate" runat="server" class="datepicker" CssClass="textClass" Width="250px" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox></td>
                                                <td style="width: 10%" class="tdText">End Date:</td>
                                                <td style="width: 1%"><span style="color: red">*</span></td>
                                                <td style="width: 35%" class="tdText">
                                                    <asp:TextBox ID="txtEdate" runat="server" CssClass="textClass" Width="250px" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox></td>
                                            </tr>

                                            <tr>
                                                <td class="tdText" style="text-align: center" colspan="6"></td>

                                            </tr>
                                            <tr>
                                                <td class="tdText" style="text-align: center" colspan="6">
                                                    <asp:Button ID="btnGenCLS" runat="server" Text="Generate" OnClick="btnGenCLS_Click" CssClass="NFButton" />
                                                    <input type="button" id="CancalGen" class="NFButton" value="Cancel" />
                                                </td>
                                            </tr>

                                        </table>
                                    </div>
                                </div>





                                <div class="clear"></div>
                            </div>







                        </div>


                    </div>
                </div>


                <div id="PopDownload" class="web_dialog" style="width: 600px; top: -20%;">

                    <div id="Div53" style="width: 700px;">


                        <table style="width: 97%">
                            <tr>
                                <td colspan="2">
                                    <table style="width: 85%">
                                        <tr>
                                            <td runat="server" id="tdMsgExport" style="height: 50px"></td>
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

            </div>
        </div>

        <div id="FreetextPopup" class="web_dialog">


            <ftb:freetextbox id="FreeTextBox1" focus="true" supportfolder="../FreeTextBox" width="99%" height="500px"
                javascriptlocation="ExternalFile" buttonimageslocation="ExternalFile" toolbarimageslocation="ExternalFile"
                toolbarstyleconfiguration="OfficeXP" toolbarlayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,                                   
FontForeColorPicker,FontBackColorsMenu,FontBackColorPicker,InsertTable,Bold,Italic,Underline,RemoveFormat,JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList"
                runat="Server" designmodecss="designmode.css" buttonset="Office2000" backcolor="White" enablehtmlmode="true" />
            <div style="width: 100%; text-align: center">
                <input type="button" id="btnFreeTextBox" onclick="txtfree();" value="Done" class="NFButton" />
                <input type="hidden" id="hdndivname" value="" />
                <input type="hidden" id="hdncontent" value="" />

            </div>
        </div>

    </form>
</body>
</html>
