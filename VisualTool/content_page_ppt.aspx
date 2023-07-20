<%@ Page Language="C#" AutoEventWireup="true" CodeFile="content_page_ppt.aspx.cs" Inherits="content_page_ppt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Melmark Pennsylvania</title>
      <meta http-equiv="X-UA-Compatible" content="IE=10,9" /> 

    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">

    <link href="styles/ContentPage.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" media="screen" type="text/css" href="styles/colorpicker.css" />
    <link href="styles/jquery.ui.resizable.css" rel="stylesheet" />

    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <script type="text/javascript" src="scripts/colorpicker.js"></script>
    <script src="scripts/cookies.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.resizable.js"></script>

    <link href="styles/jquery.mCustomScrollbar.css" rel="stylesheet" />

    <script src="scripts/jsForTextEditor/tiny_mce/tiny_mce.js" type="text/javascript"></script>
    <%--<script src="scripts/content_page.js" type="text/javascript"></script>--%>

    <style type="text/css">
        .style1
        {
            width: 454px;
        }

        .style2
        {
            width: 675px;
        }

        #Text1
        {
            width: 53px;
        }

        #Text2
        {
            width: 53px;
        }

        #Text3
        {
            width: 53px;
        }

        .style3
        {
            height: 236px;
        }

        .style4
        {
            height: 21px;
        }

        .style5
        {
            height: 26px;
        }

        .style7
        {
            height: 26px;
            width: 80px;
        }

        .style8
        {
            height: 21px;
            width: 80px;
        }

        .style9
        {
            width: 80px;
        }

        .style10
        {
            width: 50px;
        }


        .auto-style1
        {
            width: 128px;
        }

        .auto-style2
        {
            height: 26px;
            width: 128px;
        }

        .auto-style3
        {
            height: 21px;
            width: 128px;
        }

        .auto-style4
        {
            width: 127px;
        }

        #Text2
        {
            width: 53px;
        }

        #Text3
        {
            width: 53px;
        }

        .thin
        {
            font-size: small;
            font-weight: bold;
            color: red;
        }

        .medium
        {
        }

        .thick
        {
        }

        iframe
        {
            border: medium none;
            height: 560px;
            overflow: hidden;
            width: 100%;
        }

        .resizable td > div
        {
            height: 100%;
        }

        .resizable
        {
            width: 100%;
        }
		
        #rightPanel1 {
            height:182px;
            overflow: auto;
			width:180px;
            position:fixed;
            top:236px;
            width: 16%;
			margin-bottom:2px;
        }

        #rightPanel2 {

            height:176px;
            overflow-x:hidden;
            position:fixed;
            width: 16%;
			top:395px;
			background-color:transparent;
			margin:8px 0 0 0 ;
        }
		#rightPanel2 div.demo{ margin:7px 0 0 0;}
#dashboard-RHSVis 
				{
				background-color: white;
				border: 2px solid #B6D1DD;
				border-radius: 5px 5px 5px 5px !important;
				box-shadow: 3px 2px 6px 7px #BFC0C1;
				padding: 5px;
				width: 90%;
				margin: 0 auto;
				margin-bottom: 15px;
				}

       
    </style>

    <script type="text/javascript">

        $(function () {
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

</script>

    <script type="text/javascript">
        $(function () {
            window.setInterval(function () {
                $('.selected').toggleClass('-r');
            }, 1000);

            //$(".resizable td:first>div").resizable({
            //    resize: function (event, ui) {
            //        ui.helper.parent().css('width', ui.size.width + 'px');
            //    },
            //    handles: "e"
            //});
        });
    </script>
    <script type="text/javascript">
        function txtMaxVal(maxVal, e) {
            var value = $(e).val();
            value = value.replace('px', '');
            // alert(value);
            if (value > maxVal) {
                $(e).val(maxVal + 'px');
            }
        }
    </script>
    <script type="text/javascript">
        tinyMCE.init({
            // General options
            mode: "textareas",
            theme: "advanced",
            plugins: "lists,style,layer,table,save,advhr,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,searchreplace,contextmenu,paste,directionality,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,autosave,visualblocks",

            // Theme options
            theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
            theme_advanced_buttons2: "cut,copy,paste,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,|,insertdate,inserttime,preview,|,forecolor,backcolor",
            theme_advanced_buttons3: "hr,|,sub,sup,|,charmap,emotions,iespell,advhr,|del,ins,attribs,|restoredraft",

            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_statusbar_location: "bottom",
            theme_advanced_resizing: true,

            // Example content CSS (should be your site CSS)
            //content_css: "css/content.css",

            // Drop lists for link/image/media/template dialogs
            template_external_list_url: "lists/template_list.js",
            external_link_list_url: "lists/link_list.js",
            external_image_list_url: "lists/image_list.js",
            media_external_list_url: "lists/media_list.js",

            // Style formats
            style_formats: [
			{ title: 'Bold text', inline: 'b' },
			{ title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
			{ title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
			{ title: 'Example 1', inline: 'span', classes: 'example1' },
			{ title: 'Example 2', inline: 'span', classes: 'example2' },
			{ title: 'Table styles' },
			{ title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
            ],

            // Replace values for the template plugin
            template_replace_values: {
                username: "Some User",
                staffid: "991234"
            }
        });
    </script>
</head>

<body>
    <!-- top panel -->
    <form runat="server" id="form1">
        <div id="dashboard-top-panel">
            <div id="top-panel-container">
                <ul>
                    <li class="user">
                        <img src="images/admin-icon.png" width="16" height="16" align="baseline">Super Admin</li>
                    <li>
                        <img src="images/time-icon.png" width="14" height="14" align="baseline">5:41 PM</li>
                    <li><a href="#">
                        <img src="images/srch-icon.png" width="16" height="16" align="baseline">Search</a></li>
                    <li></li>
                </ul>
            </div>
        </div>

        <!-- dashboard container panel -->
        <div id="db-container">
            <!-- header -->
            <div id="header-panel">
                <div class="Dashboard-logo">
                    <img src="images/dashboard-logo.jpg" class="dash-logo">
                </div>
                <div class="header-links">
                    <ul>
                        <li><a href="homePage.aspx">
                            <img src="icons/home.png" width="28" height="25"><br>
                            Home</a></li>
                    </ul>
                </div>
            </div>


            <!-- content panel -->
            <div id="dashboard-content-panel">
                <!-- content Left side -->


                <!-- content right side -->
                <div id="dashboard-RHSVis">

                    <div class="dashboard-RHS-content">



                        <!-- CHANGABLE -->


                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <div id="mainContainer">

                          <div id="topRibbon">
                                <div id="imgProp">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="auto-style1">&nbsp;</td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">
                                                <div class="container">HEIGHT</div>
                                            </td>
                                            <td>
                                                <input id="txtImgPropHeight" type="text" style="width: 100px" value="20" onblur="txtMaxVal(500,this)" /></td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2">
                                                <div class="container">WIDTH</div>
                                            </td>
                                            <td class="style5">
                                                <input id="txtImgPropWidth" type="text" style="width: 100px" value="20" onblur="txtMaxVal(500,this)" /></td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <div class="container">BORDER</div>
                                            </td>
                                            <td class="style4" style="color: wheat;">
                                                <input id="chk_isBorder" type="checkbox" />
                                                Width: 
                        <select id="ddl_borderWidth" disabled="disabled">
                            <option style="font-size: small" value="1">Thin</option>
                            <option style="font-size: medium" value="3">Medium</option>
                            <option style="font-size: large" value="5">Large</option>

                        </select>


                                                Style:   
                                <select id="ddl_borderStyle" disabled="disabled">
                                    <option value="groove">Groove</option>
                                    <option value="dashed">Dashed</option>
                                    <option value="dotted">Dotted</option>


                                </select>
                                                Color:<input id="txt_borderColor" type="text" disabled="disabled" style="width: 50px" />
                                            </td>
                                        </tr>

                                    </table>
                                    <input id="btn_imgPropDone" type="button" class="close_ribbon" value="Done" />
                                    <input id="btn_delete" class="deleteElement close_ribbon" type="button" value="Delete" />
                                </div>
                                <!-- IMAGE PROPERTIES -->
                                <div id="tdProp">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="auto-style4">&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style4">
                                                <div class="container">HEIGHT</div>
                                            </td>
                                            <td>
                                                <input id="txtTdHeight" type="text" style="width: 100px" value="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style4">
                                                <div class="container">WIDTH</div>
                                            </td>
                                            <td>
                                                <input id="txtTdWidth" type="text" style="width: 100px" value="50" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td class="auto-style4">
                                                <div class="container">BORDER</div>
                                            </td>
                                            <td style="color: wheat;">
                                                <input id="chk_tdIsBorder" type="checkbox" />
                                                Width: 
                        <select id="ddl_tdBorderWidth" disabled="disabled" name="D1">
                            <option style="font-size: small" value="1">Thin</option>
                            <option style="font-size: medium" value="3">Medium</option>
                            <option style="font-size: large" value="5">Large</option>

                        </select>


                                                Style:   
                                <select id="ddl_tdBorderStyle" disabled="disabled" name="D2">
                                    <option value="groove">Groove</option>
                                    <option value="dashed">Dashed</option>
                                    <option value="dotted">Dotted</option>

                                </select>
                                                Color:<input id="txt_tdBorderColor" type="text" disabled="disabled" style="width: 50px" />
                                            </td>
                                        </tr>


                                    </table>
                                    <input id="btn_tdPropDone" class="close_ribbon" type="button" value="Done" />
                                </div>
                                <!-- TD PROPERTIES -->
                                <div id="vidProp">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="auto-style4">&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style4">
                                                <div class="container">HEIGHT</div>
                                            </td>
                                            <td>
                                                <input id="txtVidPropHeight" type="text" style="width: 100px" value="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style4">
                                                <div class="container">WIDTH</div>
                                            </td>
                                            <td>
                                                <input id="txtVidPropWidth" type="text" style="width: 100px" value="50" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="auto-style4">
                                                <div class="container">VIDEO</div>
                                            </td>
                                            <td style="height: 120px;">
                                                <div id="ribbonVideoListContainer">
                                                    <div id="ribbonVideoList">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" style="display: none;">
                                                            <ContentTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <%-- <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" ServiceMethod="GetCompletionList2" ServicePath="" TargetControlID="txtSearchVideo" UseContextKey="True">
                                                        </asp:AutoCompleteExtender>--%>
                                                                        <td>
                                                                            <%--  <asp:TextBox ID="txtSearchVideo" runat="server" Width="170px" Style="float: left;"></asp:TextBox>

                                                            <asp:ImageButton ID="imgSearchVideo" runat="server" class="rightPanelSearch" AlternateText="" ImageUrl="~/images/Black_Search.png" OnClick="imgSearchVideo_Click" />--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="Div1" class="loadingDiv" style="width: 204px; height: 290px;">
                                                                            </div>
                                                                            <asp:DataList ID="DataList2" runat="server" RepeatDirection="Horizontal">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image2" runat="server" Width="50px" AlternateText='<%# Eval("Path") %>' CssClass="videoStyle" Height="50px" ImageUrl='<%# Eval("Thumbnail") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:DataList></td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div id="dragDown"></div>
                                                </div>
                                            </td>
                                        </tr>


                                    </table>
                                    <input id="btn_vidPropDone" class="close_ribbon" type="button" value="Done" />
                                </div>
                                <!-- VIDEO PROPERTIES -->
                                <input id="close_ribbon" class="close_ribbon" type="button" value="Cancel" />
                            </div>

                            <table class="style1" style="width: 100%; height: 100%;">
                            <tr>
                            <td rowspan="4" style="vertical-align: top; width:15px;">
                                <div id="btn_profile" title="Profile" class="with-tip">
                                    <div class="btn btn-blue">
                                    </div>
                                </div>
                                <div id="btn_preview" style="margin-top: 2px;" title="View" class="with-tip">
                                    <div class="btn btn-purple">
                                    </div>
                                </div>
                                <div id="btn_finish" style="margin-top: 2px;" title="Save" class="with-tip">
                                    <div class="btn btn-black">
                                    </div>
                                </div>
                                <div id="btn_close" style="margin-top: 2px;" title="Close" class="with-tip">
                                    <div class="btn btn-redd">
                                    </div>
                                </div>
                               



                            </td>
                          </tr>
                                <tr>
                                    <td class="style2" rowspan="2" style="width: 50px; border-right-style: dashed; border-right-width: thin; border-right-color: #666666;">
                                        <div class="slideHeader">
                                            SLIDES
                                        </div>
                                        <div id="slideDiv">
                                            <div id="slide1" class="slides slideSelected" onclick="slideClick(this)">1</div>
                                        </div>
                                        <%--<div class="slideFooter" style="padding-top: 15px; text-align: center;">
                                            <img src="icons/Trash.png" id="" />
                                        </div>--%>
                                    </td>
                                    <td class="style2" rowspan="2">
                                        <div id="workSpace-ppt">
                                        </div>
                                    </td>
                                    <td width="200px" class="style3">
                                        <div id="rightPanel1">
                                            <div id="loadRight1" class="loadingDiv" style="width: 204px; height: 290px;">
                                            </div>
                                            <ul id="elementList">
                                                <li id="row">
                                                    <table style="width: 100%; height: 40px;">
                                                        <tr>
                                                            <td class="style10">
                                                                <img alt="Row" src="icons/newLayout.png" /></td>
                                                            <td><h3 class="rbx">New Row</h3><br />
                                                                <span class="rb">Click to add a new row</span></td>
                                                        </tr>
                                                    </table>
                                                </li>
                                                <li id="video">
                                                    <table style="width: 100%; height: 40px;">
                                                        <tr>
                                                            <td class="style10">
                                                                <img alt="Video" src="icons/videoPlayer.png" /></td>
                                                            <td><h3 class="rbx">Videos</h3><br />
                                                                <span class="rb">Double click to add a video control</span></td>
                                                        </tr>
                                                    </table>
                                                </li>
                                               <li id="audio">
                                                    <table style="width: 100%; height: 40px;">
                                                        <tr>
                                                            <td class="style10">
                                                                <img alt="Image" src="icons/images.png" /></td>
                                                            <td><h3 class="rbx">Audios</h3><br />
                                                                <span class="rb">Click to add an audio</span>
                                                                <div id="palyer" style="visibility:visible; float:right">

                                                                                   <object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20">
                                                                                        <param name="movie" value="PseudoEngine/player_mp3_maxi.swf" />
                                                                                        <param name="FlashVars" value="mp3=&showslider=0&width=25" />
                                                                                       <param name="wmode" value="transparent" />
                                                                                    </object>

                                                                                </div>
                                                                <div id="audioPlayer" style="width:233px;">
                                                                    
                                                                            <div id="musicList">
                                                                                <asp:DataList ID="dl_musicList" runat="server" Width="100%">
                                                                                    <ItemTemplate>
                                                                                        <div class="dl_musicList_td">
                                                                                        <table class="display">
                                                                                            <tr>
                                                                                                <td style="width: 30px">
                                                                                                    
                                                                                                    <asp:Image ID="ImageButton2" runat="server" CssClass="imageIcon" AlternateText='<%# Eval("Path") %>' Height="30px" ImageUrl='<%# Eval("Thumbnail") %>' Width="30px"/>
                                                                                                       
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>

                                                                                             </div>
                                                                                    </ItemTemplate>
                                                                                </asp:DataList>


                                                                                
                                                                            </div>
                                                                       

                                                                </div>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </li>
                                                <li id="text">
                                                    <table style="width: 100%; height: 40px;">
                                                        <tr>
                                                            <td class="style10">
                                                                <img alt="" src="icons/text2.png" style="height: 25px; width: 25px; margin-left: 10px;" /></td>
                                                            <td><h3 class="rbx">Text</h3><br />
                                                                <span>Double click to add some texts</span></td>
                                                        </tr>
                                                    </table>
                                                </li>
                                            </ul>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="200px">
                                        <div id="rightPanel2">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>


                                                   
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtSearchImage" runat="server" Width="170px" Style="float: left;"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtSearchImage_AutoCompleteExtender" runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSearchImage" UseContextKey="True">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:ImageButton ID="ImageButton1" runat="server" class="rightPanelSearch" AlternateText="" ImageUrl="~/images/Black_Search.png" OnClick="ImageButton1_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="loadRight2" class="loadingDiv" style="width: 204px; height: 290px;">
                                                                </div>
                                                                <asp:DataList ID="DataList1" runat="server" RepeatColumns="3"
                                                                    Width="100%" RepeatDirection="Horizontal">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Thumbnail") %>' Width="50px" Height="50px" Style="margin: 3px 10px 3px 10px;" class="selectable draggable" AlternateText='<%# Eval("MediaId") %>' ToolTip='<%# "Name: "+Eval("Name")+"\nDescription: "+Eval("Description") %>' />
                                                                    </ItemTemplate>
                                                                </asp:DataList></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>



                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--<div id="finalButtons">
                                <input id="btn_addNew" type="button" value="Add New slide" class="finalButtons" />
                                <input id="btn_finish" type="button" value="Save" class="finalButtons" />
                                <input id="btn_close" type="button" value="close" class="finalButtons" />
                            </div>--%>
                        </div>

                        <div class="popup_msg">
                            <div class="close_msg">
                                X
                            </div>
                            <div id="insertRow">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Height
                                        </td>
                                        <td>
                                            <input id="txtRowHeight" type="text" style="width: 100px" value="20" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Col 1 Width
                                        </td>
                                        <td>
                                            <input id="txtCol1Width" type="text" style="width: 100px" value="50" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Col 2 width
                                        </td>
                                        <td>
                                            <input id="txtCol2Width" type="text" style="width: 100px" value="50" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <input id="btn_done" type="button" value="Done" />
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </div>


                        <div id="previewBoard">
                            <iframe id="previewFrame"></iframe>
                            <input id="btn_previewPrev" type="button" value="Prev" />
                            <input id="btn_previewNext" type="button" value="Next" />
                            <input id="btn_previewOk" type="button" value="Ok" />
                        </div>
                        <div id="textEditorDiv" style="height: 610px">
                            <textarea id="elm1" name="elm1" style="width: 100%; height: 70%;">
				
			</textarea>
                            <input type="button" class="close_ribbon" onclick="insertText(tinyMCE.get('elm1').getContent()); return false;" value="Done" />
                            <input type="button" class="close_ribbon" id="closeTextEditor" value="close" />
                            <%--<input type="reset" name="reset" value="Reset" onclick="resetTesting()" />--%>
                        </div>

                        <div class="fullOverlay">
                        </div>
                        <div id="toolBox">
                            <div id="smallMenuClose">X</div>
                            <img id="smallMenuEdit" alt="" src="icons/1348574209_doc_edit.png" /><img id="smallMenuDelete" alt="" src="icons/Trash.png" />
                        </div>
                        <%--<div id="slideWarning">
            <div id="warningMsg">Warning</div>
            <img id="warningImg" src="icons/warning.png" />

        </div>--%>
                        <div class="descriptionBox">
                            <table style="width: 100%">
                                <tr>
                                    <td>File Id</td>
                                    <td><span id="fileId"></span></td>
                                </tr>
                                <tr>
                                    <td>File Name</td>
                                    <td><span id="fileName"></span></td>
                                </tr>
                                <tr>
                                    <td>File Size</td>
                                    <td><span id="fileSize"></span></td>
                                </tr>
                                <tr>
                                    <td>Duration</td>
                                    <td><span id="duration"></span></td>
                                </tr>
                                <tr>
                                    <td colspan="2">Description<div class="descriptionDiv"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <input id="descBtnPlay" type="button" value="Play" />
                                        <input id="descBtnClose" type="button" value="Close" />
                                        <input id="descBtnOk" type="button" value="Ok" /></td>
                                </tr>
                            </table>
                        </div>
                        <div class="messageRibbon">
                            <div class="innerMsgRibbon"></div>
                        </div>

                        <!-- CHANGEABLE -->
                    </div>
<div class="clear"></div>
                </div>



            <!-- footer -->
            <div id="footer-panel">
                <ul>
                    <li>COPYRIGHT &copy; 2012 Melmark Inc. All rights reserved</li>
                </ul>
            </div>


        </div>
    </form>

    <!-- mousewheel plugin -->
    <script src="scripts/jquery.mousewheel.min.js"></script>
    <!-- custom scrollbars plugin -->
    <script src="scripts/jquery.mCustomScrollbar.js"></script>

    <div id="loadingOverlay"></div>
    <div id="loading">
        <div>Please Wait...</div>
        <img src="images/29.gif" style="width: 50px; height: 50px;" />
    </div>
</body>

<script type="text/javascript">

    var selected;
    var selectedTxt;
    var selectedSlide = 1;
    var currSlide = 1;
    var slideNo = 1;
    var musicFile = '';

    //Code Starts
    function GetQueryStringParams(sParam) {
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == sParam) {
                return sParameterName[1];
            }
        }
    }
    //Code Ends

    $(document).ready(function () {

        var edit = GetQueryStringParams('edit');
        $('#palyer').empty();

        if (edit == '1') {

            var slideNumbers = getCookie("slides");
            var slideArray = slideNumbers.split(',');


            $('#slideDiv').empty();

            for (var i = 0; i < slideArray.length - 1; i++) {

                if (i == 0) {
                    $('#slideDiv').append('<div id="slide' + slideArray[i] + '" class="slides slideSelected" onclick="slideClick(this)">' + slideArray[i] + '</div>');
                    selectedSlide = slideArray[i];
                }
                else {
                    $('#slideDiv').append('<div id="slide' + slideArray[i] + '" class="slides" onclick="slideClick(this)">' + slideArray[i] + '</div>');
                }

            }

            $('#loadingOverlay').show();
            $('#loading').show();

            $.ajax({
                url: "content_page_ppt.aspx/getSlideDatas",
                data: "{ 'slideNo': '" + selectedSlide + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) { 
                    var contents = data.d;
                    var content_array = contents.split('^');
                    //alert(contents);
                    try {
                        $('#workSpace-ppt').html(content_array[0]);
                        bindSelectableHandler();
                        musicFile = content_array[1];

                        var newElem = '<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="wmode" value="transparent" /><param name="FlashVars" value="mp3=' + musicFile + '&showslider=0&width=25" /></object>';
                        $('#palyer').empty();
                        $('#palyer').append(newElem);

                        //$(contents).find('.resizable').find('td:first div').resizable({
                        //    resize: function (event, ui) {
                        //        ui.helper.parent().css('width', ui.size.width + 'px');
                        //    },
                        //    handles: "e"
                        //});

                        $(contents).find('.dropable').droppable({
                            accept: ".draggable",
                            drop: function (event, ui) {
                               // alert('hello');
                                var n = $(this).children().not('.ui-resizable-handle').length;
                                if (n <= 0) {
                                    var newElement = ui.draggable.clone();
                                    $(newElement).removeClass("draggble");
                                    $(newElement).removeClass("ui-draggable");
                                    $(newElement).appendTo(this);
                                }
                                else {
                                    $('.innerMsgRibbon').text('Only one item is allowed<br/>Delete existing item');
                                    $('.messageRibbon').show().fadeOut(3000);
                                }
                            }
                        });
                    }
                    catch (exp) {

                    }

                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);

                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                }
            });
            save = true;
        }


        var buffPointerX = 0;
        var buffPointerY = 0;

        var pageWidth = window.innerWidth;
        var pageHeight = window.innerHeight;



        if (typeof pageWidth != "number") {
            if (document.compatMode == "CSS1Compat") {
                pageWidth = document.documentElement.clientWidth;
                pageHeight = document.documentElement.clientHeight;
            }
            else {
                pageWidth = document.body.clientWidth;
                pageHeight = document.body.clientHeight;
            }
        }

        $('.dl_musicList_td').click(function () {

            var elem = $(this);
            //alert(elem.find('.imageIcon').attr('alt').replace('~/', ''));
            musicFile = elem.find('.imageIcon').attr('alt').replace('~/', '');

            var newElem = '<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="wmode" value="transparent" /><param name="FlashVars" value="mp3=' + musicFile + '&amp;autoplay=1&showslider=0&width=25" /></object>';
            $('#palyer').empty();
            $('#palyer').append(newElem);
        });

        $('#btn_done').click(function () {

            var rowHeight = document.getElementById("txtRowHeight").value;
            var col1Width = document.getElementById("txtCol1Width").value;
            var col2Width = document.getElementById("txtCol2Width").value;

            if (rowHeight == "" || rowHeight == NaN) { rowHeight = 20; }
            if (col1Width == "" || col1Width == NaN) { col1Width = 50; }
            if (col2Width == "" || col2Width == NaN) { col2Width = 50; }

            document.getElementById("txtRowHeight").value = "20";
            document.getElementById("txtCol1Width").value = "50";
            document.getElementById("txtCol2Width").value = "50";
            // var a = $("<table class='workSpaceTable resizable'><tr><td><div class='dropable selectable'></div></td><td><div style='height:100%;' class='dropable selectable'></div></td></tr></table>");
            var a = $("<table class='workSpaceTable'><tr height=" + rowHeight + "px><td style='vertical-align:top;' class='selectable dropable' width=" + col1Width + "%></td><td style='vertical-align:top;' class='selectable dropable' width=" + col2Width + "%></td></tr></table>");
            $('#workSpace-ppt').append(a);
            a.find('.dropable').droppable({
                accept: ".draggable",
                drop: function (event, ui) {

                    var n = $(this).children().not('.ui-resizable-handle').length;
                    if (n <= 0) {
                        var newElement = ui.draggable.clone();
                        $(newElement).removeClass("draggble");
                        $(newElement).removeClass("ui-draggable");
                        $(newElement).appendTo(this);
                    }
                }
            });
            //a.find('td:first div').resizable({
            //    resize: function (event, ui) {
            //        ui.helper.parent().css('width', ui.size.width + 'px');
            //    },
            //    handles: "e"
            //});
            bindSelectableHandler();
            $('.popup_msg').fadeOut('slow');

        });

        $('#txtCol1Width').keyup(function () {
            var col1Width = document.getElementById("txtCol1Width").value;
            var col2Width = 0;

            if (col1Width != NaN) {
                col2Width = 100 - col1Width;
                document.getElementById("txtCol2Width").value = col2Width;
            }
        });

        $('#row').click(function (e) {
            //getting height and width of the message box
            var boxHeight = $('.popup_msg').height();
            var boxWidth = $('.popup_msg').width();
            //calculating offset for displaying popup message
            leftVal = e.pageX - (boxWidth / 2) + "px";
            topVal = e.pageY - (boxHeight / 2) + "px";
            //show the popup message and hide with fading effect
            $('.popup_msg').css({ left: leftVal, top: topVal }).show(); //.fadeOut(1500);

            buffPointerX = e.pageX;
            buffPointerY = e.pageY;

            var newLeft = (pageWidth / 2) - (boxWidth / 2) + "px";
            var newTop = (pageHeight / 2) - (boxHeight / 2) + "px";

            $('.popup_msg').css({ top: newTop, left: newLeft }).show();
        });

        $('.close_msg').click(function () {
            $('.popup_msg').fadeOut();
            document.getElementById("txtRowHeight").value = "20";
            document.getElementById("txtCol1Width").value = "50";
            document.getElementById("txtCol2Width").value = "50";
        });



        $('#smallMenuEdit').click(function (event) {

            var elemHeight = $(selected).css('height');
            var elemWidth = $(selected).css('width');

            if ($(selected).hasClass('videoDiv')) {
                $('#topRibbon div').not('#close_ribbon').hide();
                $('#vidProp,#ribbonVideoListContainer,#UpdatePanel2,#ribbonVideoList,#dragDown').show();
                $('#txtVidPropHeight').val('240px');
                $('#txtVidPropWidth').val('320px');
                $('#topRibbon').slideDown('slow', 'linear', function () {
                    $('.container').show();
                    var container = $(".container")
                    container.shuffleLetters();
                });

            }

            if (selected.toString() == '[object HTMLImageElement]') {
                $('#topRibbon div').not('#close_ribbon').hide();
                $('#imgProp').show();
                $('#txtImgPropHeight').val(elemHeight);
                $('#txtImgPropWidth').val(elemWidth);
                $('#topRibbon').slideDown('slow', 'linear', function () {
                    $('.container').show();
                    var container = $(".container")
                    container.shuffleLetters();
                });
            }


            if (selected.toString() == '[object HTMLDivElement]') {
                //CLEAR THE TEXT EDITOR AND FILLING IT WITH THE TD CONTENTS
                var insideContent = $(this).find('.txtContainer').html();
                tinymce.get('elm1').setContent('');
                tinyMCE.execCommand('mceInsertContent', false, insideContent);

                //SHOW THE TEXT EDITOR
                $('.fullOverlay').fadeIn('slow', function () {
                    $('#previewBoard').hide();
                    $('#textEditorDiv').show();
                    $('#textEditorDiv').fadeIn('slow');
                });
            }
            if (selected.toString() == '[object HTMLVideoElement]') {

                $('#topRibbon div').not('#close_ribbon').hide();
                $('#vidProp,#ribbonVideoListContainer,#UpdatePanel2,#ribbonVideoList,#dragDown').show();
                $('#txtVidPropHeight').val(elemHeight);
                $('#txtVidPropWidth').val(elemWidth);
                $('#topRibbon').slideDown('slow', 'linear', function () {
                    $('.container').show();
                    var container = $(".container")
                    container.shuffleLetters();
                });

            }
        });


        $('#smallMenuClose').click(function () {
            $('#toolBox').hide();
        });

        var selectedVideo = '';
        $('#DataList2 img').click(function () {
            $('#DataList2 img').css('border', '0px groove Black')
            $(this).css('border', '1px groove Red');

            selectedVideo = $(this).attr('alt');
            selectedVideo = selectedVideo.replace('~/Repository/videos/', '');
        });

        var previewWidth = $('#previewBoard').width();
        var txtEditorWidth = $('#textEditorDiv').width();

        var previewLeft = (pageWidth / 2) - (previewWidth / 2) + 'px';
        var textEditorLeft = (pageWidth / 2) - (txtEditorWidth / 2) + 'px';

        $('#previewBoard').css({ left: previewLeft });
        $('#textEditorDiv').css({ left: previewLeft });

        var previewIndex = selectedSlide;
        $('#btn_preview').click(function () {
            if (save == true) {
                $('.fullOverlay').fadeIn('slow', function () {
                    //showHtml();
                    $('#previewFrame').attr('src', 'previewPage.aspx?pageNo=' + previewIndex);
                    $('#previewBoard,#btn_previewPrev,#btn_previewNext').show();
                    $('#textEditorDiv').hide();
                    $('#previewBoard').fadeIn('slow');
                });
            }
            else {

                saveFn();

            }
        });

        $('#btn_previewPrev').click(function () {
            var x = parseInt(previewIndex);
            x = x - 1;
            previewIndex = x;
            $('#previewFrame').attr('src', 'previewPage.aspx?pageNo=' + previewIndex);
        });

        $('#btn_previewNext').click(function () {
            var x = parseInt(previewIndex);
            x = x + 1;
            previewIndex = x;
            $('#previewFrame').attr('src', 'previewPage.aspx?pageNo=' + previewIndex);
        });

        $('#btn_previewOk').click(function () {
            $('#previewBoard').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('slow');
            });
        });

        $('#btn_profile').click(function () {
            $('.fullOverlay').fadeIn('slow', function () {
                $('#previewFrame').attr('src', 'profilePreview.aspx');
                $('#previewBoard').show();
                $('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
                $('#previewBoard').fadeIn('slow');
            });
        });

        $('#btn_finish').click(function () {
            var details = document.getElementById('workSpace-ppt').innerHTML;


            $('#loadingOverlay').show();
            $('#loading').show();

            $.ajax({
                url: "content_page_ppt.aspx/saveFile_ppt",
                data: "{ 'contents': '" + details + "','slideNo':'" + selectedSlide + "','MusicFile':'"+musicFile+"' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    $('#loadingOverlay').hide();
                    $('#loading').hide();

                    save = true;
                    $('.innerMsgRibbon').text('Saved');
                    $('.messageRibbon').show().fadeOut(2000);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                }
            });


        });
        $('#btn_close').click(function () {
            if (confirm('Any unsaved changes will be discarded...\nSure you want to close?')) {
                window.location = 'LessonManagement.aspx';
            }
        });

        $('#text').dblclick(function () {
            selectedTxt = selected;
            tinymce.get('elm1').setContent('');
            if (selected && $(selected).hasClass('dropable')) {
                $('.fullOverlay').fadeIn('slow', function () {
                    $('#previewBoard').hide();
                    $('#textEditorDiv').show();
                    $('#textEditorDiv').fadeIn('slow');
                });
            }
            else {
                $('.innerMsgRibbon').text('select a cell');
                $('.messageRibbon').show().fadeOut(2000);
            }
        });


        $('#closeTextEditor').click(function () {
            $('#textEditorDiv').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('slow');
            });

        });
        bindSelectableHandler();

        $('#btn_imgPropDone').click(function () {
            styleBuilder(selected, 'img');
            $('#topRibbon').slideUp('slow', 'linear');
        });

        $('.deleteElement, #smallMenuDelete').click(function () {
            if (confirm('Delete it?')) {

                if (selected.toString() == '[object HTMLParagraphElement]') {
                    var parentTxt = $(selected).parent('td');
                    $(parentTxt).empty();

                }
                else {
                    $(selected).remove();
                    $('#topRibbon').slideUp('slow', 'linear');
                    $('#toolBox').hide();
                }
            }
        });

        $('#close_ribbon').click(function () {
            $('#topRibbon').slideUp('slow', 'linear');
        });

        $('#video').dblclick(function () {
            if (selected) {
                if ($(selected).children().not('.ui-resizable-handle').length <= 0) {
                    if ($(selected).hasClass('dropable')) {
                        var videoElement = '<div class="selectable videoDiv" style="padding:3px;"><object type="application/x-shockwave-flash" data="Repository/videos/player_flv_maxi.swf" width="320" height="240"><param name="movie" value="player_flv_maxi.swf" /><param name="wmode" value="transparent" /><param name="FlashVars" value="flv=123.flv" /></object></div>';
                        $(selected).append(videoElement);
                    }
                }
            }
            else {
                $('.innerMsgRibbon').text('Select any area');
                $('.messageRibbon').show().fadeOut(2000);
            }
        });
        $('#videoDiv').click(function () {

            slected = $(this);
        });
        $('#audio').click(function () {
            // alert($('#audioPlayer').css('display'));
            if ($('#audioPlayer').css('display') == 'none') {
                $('#audioPlayer').fadeIn('slow');
            }
            else {
                $('#audioPlayer').fadeOut('slow');
            }

        });
        $('#btn_tdPropDone').click(function () {
            styleBuilder(selected, 'td');
            $('#topRibbon').slideUp('slow', 'linear');
        });

        $('#btn_vidPropDone').click(function () {
            var width = '320';
            var height = '240';
            if ($('#txtVidPropHeight').val() != '') {
                height = $('#txtVidPropHeight').val() + 'px';
            }
            if ($('#txtVidPropWidth').val() != '') {
                width = $('#txtVidPropWidth').val() + 'px !important';
            }
            var videoElement = '<object type="application/x-shockwave-flash" data="Repository/videos/player_flv_maxi.swf" width="' + width + '" height="' + height + '"><param name="wmode" value="transparent" /><param name="movie" value="player_flv_maxi.swf" /><param name="FlashVars" value="flv=' + selectedVideo + '" /></object>';
            $(selected).empty();
            $(selected).append(videoElement);

            $('#topRibbon').slideUp('slow', 'linear');
        });

        //DRAGGING AND DROPING EVENTS
        $(".draggable").draggable({
            revert: "invalid",
            helper: "clone",
            cursor: "move"
        });
        

        $(".dropable").droppable({
            accept: ".draggable",
            drop: function (event, ui) {

                var n = $(this).children().length;
                if (n <= 0) {
                    var newElement = ui.draggable.clone();
                    $(newElement).css('margin', '0px');
                    $(newElement).appendTo(this);
                    $(newElement).removeClass("draggble");
                    $(newElement).removeClass("ui-draggable");

                }
                else {
                    $('.innerMsgRibbon').text('Only one item is allowed<br/>Delete existing item');
                    $('.messageRibbon').show().fadeOut(2000);
                }

            }
            //DRAGGING AND DROPPIN EVENTS END
        });

        $('#warningImg').mouseenter(function () {
            $('#slideWarning').css('right', '0px');
            var container = $(".errorSuggestion");
            container.shuffleLetters();
        });
        $('#warningImg').mouseleave(function () {
            $('#slideWarning').css('right', '-202px');
        });

        $('#chk_isBorder').change(function () {
            if ($('#chk_isBorder').is(':checked')) {
                $('#ddl_borderWidth,#ddl_borderStyle,#txt_borderColor').removeAttr('disabled');
            }
            else {
                $('#ddl_borderWidth,#ddl_borderStyle,#txt_borderColor').attr('disabled', 'disabled');
            }

        });
        $('#chk_tdIsBorder').change(function () {
            if ($('#chk_tdIsBorder').is(':checked')) {
                $('#ddl_tdBorderWidth,#ddl_tdBorderStyle,#txt_tdBorderColor').removeAttr('disabled');
            }
            else {
                $('#ddl_tdBorderWidth,#ddl_tdBorderStyle,#txt_tdBorderColor').attr('disabled', 'disabled');
            }

        });
        $('#descBtnClose').click(function () {
            $('.descriptionBox').hide();
        });

        $('#btn_addNew').click(function () {

            slideNo = $('#slideDiv').find('.slides').length;
            var details = document.getElementById('workSpace-ppt').innerHTML;

            $('#loadingOverlay').show();
            $('#loading').show();

            $.ajax({
                url: "content_page_ppt.aspx/saveFile_ppt",
                data: "{ 'contents': '" + details + "','slideNo':'" + slideNo + "', 'MusicFile':'"+musicFile+"' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    save = false;
                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);

                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                }
            });

            slideNo++;
            currSlide = slideNo;
            selectedSlide = slideNo;
            musicFile = '';
            $('#palyer').empty();
            $('.slides').removeClass('slideSelected');
            $('#slideDiv').append('<div id="slide' + slideNo + '" class="slides slideSelected" onclick="slideClick(this)">' + slideNo + '</div>');
            $('#workSpace-ppt').empty();
            save = false;
            // CustomScrollbar();
        });



        $('#btn_deleteSlide').click(function () {
            $('#slide' + selectedSlide).hide();

            $('#loadingOverlay').show();
            $('#loading').show();

            $.ajax({
                url: "content_page_ppt.aspx/deleteSlide_ppt",
                data: "{'slideNo':'" + selectedSlide + "' }",  
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    save = true;
                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);

                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                }
            });
        });
    });


    //////////////////////
    //EXTERNAL FUNCTIONS
    //////////////////////
    var save = false;

    function saveFn() {
        var details = document.getElementById('workSpace-ppt').innerHTML;
        var retValue = false;
        $('#loadingOverlay').show();
        $('#loading').show();

        //alert(currSlide);
        $.ajax({
            url: "content_page_ppt.aspx/saveFile_ppt",
            data: "{ 'contents': '" + details + "','slideNo':'" + currSlide + "','MusicFile':'" + musicFile + "' }",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                $('.innerMsgRibbon').text('Saved');
                $('.messageRibbon').show().fadeOut(2000);
                save = true;
                retValue = true;

                $('#loadingOverlay').hide();
                $('#loading').hide();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('.innerMsgRibbon').text('Something went wrong!! Try again...');
                $('.messageRibbon').show().fadeOut(2000);

                $('#loadingOverlay').hide();
                $('#loading').hide();
            }
        });

        return retValue;

    }

    function slideClick(e) {
        selectedSlide = $(e).attr('id');
        selectedSlide = selectedSlide.replace('slide', '');
       // alert(selectedSlide);

        if (save == false) {



            if (saveFn()) {

                $('.slides').removeClass('slideSelected');
                $(e).addClass('slideSelected');


                $('#loadingOverlay').show();
                $('#loading').show();

                $.ajax({
                    url: "content_page_ppt.aspx/getSlideDatas",
                    data: "{ 'slideNo': '" + selectedSlide + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        var contents = data.d;
                        var content_array = contents.split('^');
                        //alert(contents);
                        try {
                            $('#workSpace-ppt').html(content_array[0]);
                            bindSelectableHandler();
                            musicFile = content_array[1];

                            var newElem = '<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="wmode" value="transparent" /><param name="FlashVars" value="mp3=' + musicFile + '&showslider=0&width=25" /></object>';
                            $('#palyer').empty();
                            $('#palyer').append(newElem);

                            //$(contents).find('.resizable').find('td:first div').resizable({
                            //    resize: function (event, ui) {
                            //        ui.helper.parent().css('width', ui.size.width + 'px');
                            //    },
                            //    handles: "e"
                            //});

                            $(contents).find('.dropable').droppable({
                                accept: ".draggable",
                                drop: function (event, ui) {
                                   // alert('hello');
                                    var n = $(this).children().not('.ui-resizable-handle').length;
                                    if (n <= 0) {
                                        var newElement = ui.draggable.clone();
                                        $(newElement).removeClass("draggble");
                                        $(newElement).removeClass("ui-draggable");
                                        $(newElement).appendTo(this);
                                    }
                                    else {
                                        $('.innerMsgRibbon').text('Only one item is allowed<br/>Delete existing item');
                                        $('.messageRibbon').show().fadeOut(3000);
                                    }
                                }
                            });
                        }
                        catch (exp) {

                        }

                        $('#loadingOverlay').hide();
                        $('#loading').hide();
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            }

        }
        else {

            $('.slides').removeClass('slideSelected');
            $(e).addClass('slideSelected');

            $('#loadingOverlay').show();
            $('#loading').show();

            $.ajax({
                url: "content_page_ppt.aspx/getSlideDatas",
                data: "{ 'slideNo': '" + selectedSlide + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    var contents = data.d;
                    var content_array = contents.split('^');
                    //alert(contents);
                    try {
                        $('#workSpace-ppt').html(content_array[0]);
                        bindSelectableHandler();
                        musicFile = content_array[1];

                        var newElem = '<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="wmode" value="transparent" /><param name="FlashVars" value="mp3=' + musicFile + '&showslider=0&width=25" /></object>';
                        $('#palyer').empty();
                        $('#palyer').append(newElem);

                        //$(contents).find('.resizable').find('td:first div').resizable({
                        //    resize: function (event, ui) {
                        //        ui.helper.parent().css('width', ui.size.width + 'px');
                        //    },
                        //    handles: "e"
                        //});

                        $(contents).find('.dropable').droppable({
                            accept: ".draggable",
                            drop: function (event, ui) {
                                alert('hello');
                                var n = $(this).children().not('.ui-resizable-handle').length;
                                if (n <= 0) {
                                    var newElement = ui.draggable.clone();
                                    $(newElement).removeClass("draggble");
                                    $(newElement).removeClass("ui-draggable");
                                    $(newElement).appendTo(this);
                                }
                                else {
                                    $('.innerMsgRibbon').text('Only one item is allowed<br/>Delete existing item');
                                    $('.messageRibbon').show().fadeOut(3000);
                                }
                            }
                        });
                    }
                    catch (exp) {

                    }

                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);

                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                }
            });
        }
    }

    function bindSelectableHandler() {
        $('#workSpace-ppt').find('.selectable').click(function (event) {
            event.stopPropagation();
            if ($(event.target).hasClass('selectable')) {
                selected = event.target;

                $('#workSpace-ppt').find('.-r').removeClass('-r');
                $('#workSpace-ppt').find('.selected').removeClass('selected');
                $(event.target).addClass('selected');



                $('#toolBox').show();
                $('#toolBox').css('left', $(selected).position().left);
                $('#toolBox').css('top', $(selected).position().top);
                $('#toolBox').removeClass('selected');
                $('#toolBox').removeClass('-r');
                $('#toolBox').removeClass('selectable');
            }

        });


        $('#workSpace-ppt').find('.selectable').dblclick(function (event) {
            event.stopPropagation();
            selected = event.target;

           // alert(selected);
            //alert($(selected).parent('td'));

            var elemHeight = $(selected).css('height');
            var elemWidth = $(selected).css('width');

            $('#workSpace-ppt .selectable').removeClass('selected');
            $(event.target).addClass('selected');

            if ($(selected).hasClass('videoDiv')) {
                $('#topRibbon div').not('#close_ribbon').hide();
                $('#vidProp,#ribbonVideoListContainer,#UpdatePanel2,#ribbonVideoList,#dragDown').show();
                $('#txtVidPropHeight').val('240px');
                $('#txtVidPropWidth').val('320px');
                $('#topRibbon').slideDown('slow', 'linear', function () {
                    $('.container').show();
                    var container = $(".container")
                    container.shuffleLetters();
                });

            }


            if (event.target.toString() == '[object HTMLImageElement]') {
                $('#topRibbon div').not('#close_ribbon').hide();
                $('#imgProp').show();
                $('#txtImgPropHeight').val(elemHeight);
                $('#txtImgPropWidth').val(elemWidth);
                $('#topRibbon').slideDown('slow', 'linear', function () {
                    $('.container').show();
                    var container = $(".container")
                    container.shuffleLetters();
                });
            }

           

            if (event.target.toString() == '[object HTMLVideoElement]') {

                $('#topRibbon div').not('#close_ribbon').hide();
                $('#vidProp').show();
                $('#txtVidPropHeight').val(elemHeight);
                $('#txtVidPropWidth').val(elemWidth);
                $('#ribbonVideoListContainer').show();
                $('#ribbonVideoList').show();
                $('#dragDown').show();
                $('#topRibbon').slideDown('slow', 'linear', function () {
                    $('.container').show();
                    var container = $(".container")
                    container.shuffleLetters();
                });

            }


            if (event.target.toString() == '[object HTMLParagraphElement]') {

                selectedTxt = $(event.target).parent('td');

                //CLEAR THE TEXT EDITOR AND FILLING IT WITH THE TD CONTENTS
                var insideContent = $(selectedTxt).html();
                tinymce.get('elm1').setContent('');
                tinyMCE.execCommand('mceInsertContent', false, insideContent);

                //SHOW THE TEXT EDITOR
                $('.fullOverlay').fadeIn('slow', function () {
                    $('#previewBoard').hide();
                    $('#textEditorDiv').show();
                    $('#textEditorDiv').fadeIn('slow');
                });
            }

            
        });

        $(".dropable").droppable({
            accept: ".draggable",
            drop: function (event, ui) {

                var n = $(this).children().length;
                if (n <= 0) {
                    var newElement = ui.draggable.clone();
                    $(newElement).css('margin', '0px');
                    $(newElement).removeClass("draggble");
                    $(newElement).removeClass("ui-draggable");
                    $(newElement).appendTo(this);
                }

            }
            //DRAGGING AND DROPPIN EVENTS END
        });

        $('#images').click(function () {
            var container = $(".container")
            container.shuffleLetters();
        });


        //TD PROPERTIES
        $('#txt_borderColor,#txt_tdBorderColor').ColorPicker({
            onSubmit: function (hsb, hex, rgb, el) {
                $(el).val(hex);
                $(el).ColorPickerHide();
            },
            onBeforeShow: function () {
                $(this).ColorPickerSetColor(this.value);
            }
        }).bind('keyup', function () {
            $(this).ColorPickerSetColor(this.value);
        });



        $('#dragDown').click(function () {

            var boxHeight = $('#ribbonVideoList').css('height');
            if (boxHeight == '300px') {
                $('#ribbonVideoList').animate({ height: '100px' }, 'slow', 'linear', function () {
                    $('#ribbonVideoListContainer').css('position', 'relative');
                    $('#dragDown').css("background-image", "url('images/down.png')");
                });
            }
            else {
                $('#ribbonVideoListContainer').css({ 'position': 'absolute' });
                $('#ribbonVideoList').animate({ height: '300px' }, 'slow', 'linear');
                $('#dragDown').css("background-image", "url('images/up.png')");

            }
        });
    }

    function showHtml() {
        var details = document.getElementById('workSpace').innerHTML;
        var showLbl = document.getElementById('previewBoard');
        showLbl.innerHTML = details;


    }

    function insertText(contents) {

       // $(contents).find('p').addClass('selectable');
        $(selectedTxt).html(contents);
        $(selectedTxt).find('p').addClass('selectable');
        $('#textEditorDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('slow');
        });
    }

    function showToolBox(e) {
        $('#toolBox').css('left', e.pageX);

    }

    function styleBuilder(controlName, controlType) {

        ////////////////////////
        //FOR IMAGE CONTROLLS//
        ///////////////////////

        if (controlType == 'img') {
            if ($('#txtImgPropHeight').val() != '') {
                $(controlName).css({ 'height': $('#txtImgPropHeight').val() });
            }
            if ($('#txtImgPropWidth').val() != '') {
                $(controlName).css({ 'width': $('#txtImgPropWidth').val() });
            }

            if ($('#chk_isBorder').is(':checked')) {


                $(controlName).css({ 'border-width': $('#ddl_borderWidth').val() + 'px' });
                $(controlName).css({ 'border-Style': $('#ddl_borderStyle').val() });
                if ($('#txtImgPropWidth').val() != '') {
                    $(controlName).css({ 'border-color': '#' + $('#txt_borderColor').val() });
                }
            }
        }

        /////////////////////////
        //FOR TD CONTROLLS    //
        ///////////////////////


        if (controlType == 'td') {
            if ($('#txtTdPropHeight').val() != '') {
                $(selected).css('height', $('#txtTdHeight').val() + 'px');
            }
            if ($('#txtTdPropWidth').val() != '') {
                $(selected).css('width', $('#txtTdWidth').val() + 'px !important');
            }
            if ($('#chk_isTdBorder').is(':checked')) {


                $(controlName).css({ 'border-width': $('#ddl_tdBorderWidth').val() + 'px !important' });
                $(controlName).css({ 'border-Style': $('#ddl_tdBorderStyle').val() + '!important' });
                if ($('#txtImgPropWidth').val() != '') {
                    $(controlName).css({ 'border-color': '#' + $('#txt_tdBorderColor').val() + '!important' });
                }
            }
        }

        /////////////////////////
        //FOR VIDEO CONTROLLS    //
        ///////////////////////


        if (controlType == 'video') {
            if ($('#txtVidPropHeight').val() != '') {
                $(selected).css('height', $('#txtVidPropHeight').val() + 'px');
            }
            if ($('#txtVidPropWidth').val() != '') {
                $(selected).css('width', $('#txtVidPropWidth').val() + 'px !important');
            }

        }
    }


</script>
<script type="text/javascript" src="scripts/jquery.shuffleLetters.js"></script>
</html>
