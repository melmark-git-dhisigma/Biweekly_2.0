<%@ Page Language="C#" AutoEventWireup="true" CodeFile="content_page_pptNew.aspx.cs" Inherits="content_page_pptNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
     <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="styles/ContentPage.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" media="screen" type="text/css" href="styles/colorpicker.css" />
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />
    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>

    <link href="styles/jquery.ui.resizable.css" rel="stylesheet" />
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <script type="text/javascript" src="scripts/colorpicker.js"></script>
    <script src="scripts/cookies.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.resizable.js"></script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />

    <link href="styles/jquery.mCustomScrollbar.css" rel="stylesheet" />
    <link href="styles/commonStyle.css" rel="stylesheet" />

    <script src="scripts/jsForTextEditor/tiny_mce/tiny_mce.js" type="text/javascript"></script>
    <%--<script src="scripts/content_page.js" type="text/javascript"></script>--%>
     <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>
    <script type="text/javascript">
        $(function ($) {
            $('.jclock').jclock();
        });


    </script>
    <style type="text/css">
        .style1 {
            width: 454px;
        }

        .style2 {
            width: 675px;
        }

        #Text1 {
            width: 53px;
        }

        #Text2 {
            width: 53px;
        }

        #Text3 {
            width: 53px;
        }

        .style3 {
            height: 236px;
        }

        .style4 {
            height: 21px;
        }

        .style5 {
            height: 26px;
        }

        .style7 {
            height: 26px;
            width: 80px;
        }

        .style8 {
            height: 21px;
            width: 80px;
        }

        .style9 {
            width: 80px;
        }

        .style10 {
            width: 50px;
        }


        .auto-style1 {
            width: 128px;
        }

        .auto-style2 {
            height: 26px;
            width: 128px;
        }

        .auto-style3 {
            height: 21px;
            width: 128px;
        }

        .auto-style4 {
            width: 127px;
        }

        #Text2 {
            width: 53px;
        }

        #Text3 {
            width: 53px;
        }

        .thin {
            font-size: small;
            font-weight: bold;
            color: red;
        }

        .medium {
        }

        .thick {
        }

        iframe {
            border: medium none;
            height: 560px;
            overflow: hidden;
            width: 100%;
        }

        .resizable td > div {
            height: 100%;
        }

        .resizable {
            width: 100%;
        }

        #btnSave {
            height: 22px;
            width: 30px;
        }

        #btnClear {
            height: 15px;
            width: 28px;
        }

        #btnRegen {
            height: 17px;
            width: 30px;
        }

        #closeNew {
            display: block;
            height: 23px;
            overflow: hidden;
            position: absolute;
            right: 4px;
            top: 2px;
            width: 24px;
        }

        a {
            text-decoration: none;
        }

        #dashboard-RHSVis {
            background-color: #FFFFFF;
            border: 2px solid #B6D1DD;
            border-radius: 5px 5px 5px 5px !important;
            box-shadow: 0 1px 5px 3px #BFC0C1;
            margin: 0 auto 15px auto;
            padding: 5px;
            width: 90%;
        }

        #rightPanel1 {
            height: 182px;
            overflow: auto;
            width: 180px;
            position: absolute;
            top: 220px;
            width: 16%;
            /*margin: 0 0 2px 50px;*/
            /*left: 982px;*/
            bottom: 367px;
            margin:3px;
        }

        #rightPanel2 {
            height: 290px;
            overflow-x: hidden;
            position: absolute;
            width: 16%;
            top: 375px;
            background-color: white;
            margin: 8px 0 0 0;
        }


        a.homecls, a.homecls:link, a.homecls:visited {
            background: url("images/hme.png")repeat scroll 0 0 transparent;
            float: right;
            height: 50px;
            margin: 0 2% 0 0;
            padding: 0;
            width: 48px;
            cursor: pointer;
            display: block;
        }

            a.homecls:hover {
                background-position: 0 -57px;
            }


        a.lessnClass, a.lessnClass:link, a.lessnClass:visited {
            background: url("images/lessonmanagement.png") repeat scroll 0 0 transparent;
            float: right;
            height: 50px;
            margin: 0 1% 0 0;
            padding: 0;
            width: 48px;
            cursor: pointer;
            display: block;
        }

            a.lessnClass:hover {
                background-position: 0 -57px;
            }

        a.repocls, a.repocls:link, a.repocls:visited {
            background: url("images/repo.png") repeat scroll 0 0 transparent;
            float: right;
            height: 50px;
            margin: 0 1% 0 0;
            padding: 0;
            width: 48px;
            cursor: pointer;
            display: block;
        }

            a.repocls:hover {
                background-position: 0 -57px;
            }
    </style>




    <script type="text/javascript">
        $(function () {
            window.setInterval(function () {
                $('.selected').toggleClass('-r');
            }, 1000);


        });
    </script>
    <script type="text/javascript">
        function txtMaxVal(maxVal, e) {
            var value = $(e).val();
            value = value.replace('px', '');

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
 function loadmaster() {

            self.location = '../../LoginContinue.aspx';



        }
    </script>
   
</head>
<body>
    <!-- top panel -->
    <form runat="server" id="form1">
        <asp:HiddenField ID="hd_slideNo" runat="server" />
        <div id="dashboard-top-panel">
            <div id="top-panel-container">
                <ul>
                    <li class="user" style="width: 50%;">
                        <asp:Label ID="lblLoginName" runat="server" Text="Label"></asp:Label>
                    </li>
                     <li class="timeSs">
                        <div>
                            <div style="float: left; width: auto;">
                            </div>
                            <div style="float: left; width: auto;" class="jclock"></div>
                        </div>
                    </li>
                     <li>
                        <a href="#" title="StartUp Page" onclick="loadmaster();">
                            <img src="images/StartHome.png" width="10" height="10" align="baseline" />
                            Landing Portal</a>

                    </li>
                    <li class="box40">
                        <a href="../Administration/AdminHome.aspx">Administration</a>
                    </li>
                   
                </ul>

            </div>
        </div>
        <!-- dashboard container panel -->
        <div id="db-container">
            <div id="header-panel">
                <div class="Dashboard-logo">
                    <img src="../Administration/images/student-logo.jpg" alt="">

                    <a href="homePage.aspx" class="homecls" title="Home"></a>
                    <a href="LessonManagement.aspx" class="lessnClass" title="Lesson Management"></a>
                    <a href="repository-manag.aspx" class="repocls" title="Repository Management"></a>

                </div>


            </div>


            <!-- content panel -->

            <!-- content Left side -->


            <!-- content right side -->
            <div class="clear"></div>
            <div id="dashboard-RHSVis">
                <h2 class="clk">Content - PPT

                     <span id="td_LP" runat="server"></span>
                </h2>

                <hr />




                <!-- CHANGABLE -->


                <asp:ToolkitScriptManager runat="server" ID="tsm" ></asp:ToolkitScriptManager>

                <tfoot>
                    <tr class="odd">
                        <th colspan="8">





                            <%--   <div id="divSave" style="margin-top: 2px;" title="Close" class="with-tip">
                                            <input id="btnSave" type="button" value="SAVE" />
                                        </div>
                                        <div id="divClear" style="margin-top: 2px;" title="Close" class="with-tip">
                                            <input id="btnClear" type="button" value="CLEAR" />
                                        </div>
                                        <div id="divRegen" style="margin-top: 2px;" title="Close" class="with-tip">
                                            <input id="btnRegen" type="button" value="Regenerate" />
                                        </div>--%>

                        </th>
                    </tr>
                </tfoot>



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
                        <div id="vidProp">
                            <table style="width: 100%;">

                                <tr>
                                    <td class="auto-style4">
                                        <div class="container">VIDEO</div>
                                    </td>
                                    <td style="height: 120px;">
                                        <div id="ribbonVideoListContainer" style="background-color: #ffffff;">
                                            <div id="ribbonVideoList">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td>
                                                                    <div id="Div1" class="loadingDiv" style="width: 204px; height: 290px;">
                                                                    </div>
                                                                    <asp:DataList ID="DataList2" runat="server" RepeatColumns="3"
                                                                        Width="100%" RepeatDirection="Horizontal">
                                                                        <ItemTemplate>
                                                                            <div id="imgDiv" class="demo1">
                                                                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Thumbnail") %>' Width="50px" Height="50px" Style="margin: 3px 10px 3px 10px;" class="selectable" AlternateText='<%# Eval("Path") %>' ToolTip='<%# "Name: "+Eval("Name")+"\nDescription: "+Eval("Description") %>' />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:DataList>


                                                                </td>
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
                            <td rowspan="4" style="vertical-align: top; width: 15px;">
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
                            <td class="style2" rowspan="2" style="width: 50px;">
                                <div class="slideHeader">
                                    SLIDES
                                </div>
                                <div id="btn_addNew" title="Add Slide" class="with-tip" style="margin-top: 2px; width: 15px; height: 15px; float: left; cursor: pointer;">
                                    <div class="btn btn-green">
                                        <img src="images/singleadd.png" width="16" height="16">
                                    </div>
                                </div>
                                <div id="btn_deleteSlide" style="margin: 2px 0 0 30px; width: 15px; height: 15px; float: left; cursor: pointer;" title="Delete Slide" class="with-tip">
                                    <div class="btn btn-red">
                                        <img src="images/singledelete.png" width="14" height="15">
                                    </div>
                                </div>
                                <div class="clear"></div>
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
                                    <ul id="elementList">

                                        <li id="video">
                                            <table style="width: 100%; height: 40px;">
                                                <tr>
                                                    <td class="style10">
                                                        <img alt="Video" src="images/playbtn.png" /></td>
                                                    <td>
                                                        <h3 class="rbx">Videos</h3>
                                                        <br />
                                                        <span class="rb">Double click to add control</span></td>
                                                </tr>
                                            </table>
                                        </li>
                                        <li id="audio">
                                            <table style="width: 100%; height: 40px;">
                                                <tr>
                                                    <td class="style10">
                                                        <img alt="Image" src="images/speaker.png" /></td>
                                                    <td>
                                                        <h3 class="rbx">Audios</h3>
                                                        <br />
                                                        <span class="rb">Click to add an audio</span>
                                                        <div id="palyer" style="visibility: visible; float: right;">

                                                            <object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20">
                                                                <param name="movie" value="PseudoEngine/player_mp3_maxi.swf" />
                                                                <param name="FlashVars" value="mp3=&showslider=0&width=25" />
                                                                <param name="wmode" value="transparent" />
                                                            </object>

                                                        </div>
                                                        <div id="audioPlayer" style="width: 233px; z-index: 200;">

                                                            <div id="musicList">
                                                                <asp:DataList ID="dl_musicList" runat="server" Width="100%">
                                                                    <ItemTemplate>
                                                                        <div class="dl_musicList_td">
                                                                            <table class="display">
                                                                                <tr>
                                                                                    <td style="width: 30px">

                                                                                        <asp:Image ID="ImageButton2" runat="server" CssClass="imageIcon" AlternateText='<%# Eval("Path") %>' Height="30px" ImageUrl='<%# Eval("Thumbnail") %>' Width="30px" />

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
                                                        <img alt="" src="images/text.png" /></td>
                                                    <td>
                                                        <h3 class="rbx">Text</h3>
                                                        <br />
                                                        <span class="rb">Double click to add texts</span>

                                                    </td>
                                                </tr>
                                            </table>
                                        </li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td width="200px">
                                <div id="rightPanel2" style="display: block;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtSearchImage" runat="server" Width="139px" Style="float: left; margin-bottom: 5px;" value="Search Image" onBlur="if(this.value=='') this.value='Search Image'" onFocus="if(this.value =='Search Image' ) this.value=''"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="txtSearchImage_AutoCompleteExtender" runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSearchImage" UseContextKey="True">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:ImageButton ID="ImageButton1"  Width="30px" Height="25px" runat="server" class="rightPanelSearch pnlsearch" AlternateText="" ImageUrl="images/zoombt.PNG" OnClick="ImageButton1_Click" />
                                                       
                                                    </td>
                                                </tr>
                                                <tr>

                                                    <td>
                                                        <div id="loadRight2" class="loadingDiv" style="width: 204px; height: 290px;">
                                                        </div>

                                                        <asp:DataList ID="DataList1" runat="server" RepeatColumns="3"
                                                            Width="100%" RepeatDirection="Horizontal">
                                                            <ItemTemplate>
                                                                <div id="imgDiv" class="demo" style="text-align:center">
                                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Thumbnail") %>' Width="50px" Height="50px" Style="margin: 3px 10px 3px 10px;" class="selectable draggable" AlternateText='<%# Eval("MediaId") %>' ToolTip='<%# "Name: "+Eval("Name")+"\nDescription: "+Eval("Description") %>' />
                                                                    <asp:Label ID="lblname" runat="server" Text='<%# Eval("Name") %>' ></asp:Label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:DataList>

                                                    </td>

                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <div class="clear"></div>
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


                <div id="previewBoard" class="web_dialog" style="top: 10%; left: 18%; display: none;">

                    <div id="sign_up5">
                        <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                        <h3>Lesson Profile
               
                        </h3>
                        <hr />
                        <%--     <iframe class="iframeCls" id="previewFrame" style="width: 100%" scrolling="auto"></iframe>--%>
                        <iframe id="previewFrame" class="iframeCls"></iframe>
                        <input id="btn_previewPrev" type="button" value="Prev" />
                        <input id="btn_previewNext" type="button" value="Next" />
                        <input id="btn_previewOk" type="button" value="Ok" />
                        <span id="slideNoDiv" style="float: right;"></span>

                        <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                    </div>
                    <div id="previewClose"></div>

                </div>


                <div id="prevBoxContainer" style="width: 100%; height: 100%;">
                    <div id="previewBox" class="web_dialog" style="top: 6%; left: 6%; display: none;">
                        <div id="signupNew">
                            <a id="closeNew" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300"
                                    width="18" height="18" alt="" /></a>
                            <h3>Lesson Profile
                            </h3>
                            <hr />
                            <iframe id="prevboxFrame" style="width: 100%; height: 550px;"></iframe>
                        </div>
                    </div>
                </div>


                <%--  <div id="previewBoard" style="width: 810px;">
                            <iframe id="previewFrame"></iframe>
                            <input id="btn_previewPrev" type="button" value="Prev" />
                            <input id="btn_previewNext" type="button" value="Next" />
                            <input id="btn_previewOk" type="button" value="Ok" />
                            <span id="slideNoDiv" style="float: right;"></span>
                        </div>--%>
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



                <div class="clear"></div>
            </div>


            <!-- footer -->
            <div id="footer-panel">
                <ul>
                    <li>&copy; Copyright 2015, Melmark, Inc. All rights reserved.</li>
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


            $('#loadingOverlay').show();
            $('#loading').show();

            var slideNumbers = $('#hd_slideNo').val(); // getCookie("slides");
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


            $.ajax({
                url: "content_page_pptNew.aspx/getSlideDatas",
                data: "{ 'slideNo': '" + selectedSlide + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    var contents = data.d;

                    if (contents != "") {

                        var content_array = contents.split('^');
                        currSlide = selectedSlide;

                        var listNew = $(content_array[0]).find('.saveElem');
                        var W = $('#workSpace-ppt').offset();


                        for (var i = 0; i < listNew.length; i++) {

                            var height = $(listNew[i]).find('.height').html();
                            var width = $(listNew[i]).find('.width').html();
                            var top = parseFloat($(listNew[i]).find('.top').html());
                            var left = parseFloat($(listNew[i]).find('.left').html());
                            var data = $(listNew[i]).find('.data').html();

                            data = data.replace(/&gt;/g, '>');
                            data = data.replace(/&lt;/g, '<');

                            $('#workSpace-ppt').append('<div class = "demo" style = "position:absolute; height:' + height + '; width:' + width + ';top:' + (top + W.top) + 'px;left:' + (left + W.left) + 'px">' + data + '</div>');


                        }

                        hoverMenu();
                        $('.demo').find(".ui-resizable-handle").remove();
                        $('.demo').draggable({ containment: "parent" }).resizable();
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

            if (elem.find('.imageIcon').attr('alt')) {
                musicFile = elem.find('.imageIcon').attr('alt').replace('~/', '');
            }
            else {
                musicFile = '';
            }


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
        $('#btn_preview').click(function btnPreview() {
            if (save == true) {
                $('#btn_previewPrev').attr("disabled", true);
                slideNo = $('#slideDiv').find('.slides').length;
                if (slideNo <= 1) {
                    $('#btn_previewNext').attr("disabled", true);
                }
                $('.fullOverlay').fadeIn('slow', function () {
                    //showHtml();
                    $('#previewFrame').attr('src', 'previewPage.aspx?pageNo=' + previewIndex + '&type=content-ppt');
                    $('#previewBoard,#btn_previewPrev,#btn_previewNext').show();
                    $('#textEditorDiv').hide();
                    $('#previewBoard').fadeIn('slow');

                    $('#slideNoDiv').html('Slide No: ' + previewIndex);
                });
            }
            else {

                saveFn();
                btnPreview();

            }
        });

        $('#btn_previewPrev').click(function () {
            $('#btn_previewNext').attr("disabled", false);
            if (previewIndex > 0) {
                var x = parseInt(previewIndex);
                x = x - 1;
                previewIndex = x;
                $('#previewFrame').attr('src', 'previewPage.aspx?pageNo=' + previewIndex + '&type=content-ppt');
                $('#slideNoDiv').html('Slide No: ' + previewIndex);
                $('#btn_previewPrev').attr("disabled", false);
            }
            else {
                $('#btn_previewPrev').attr("disabled", true);
            }


            if (previewIndex == 1) {
                $('#btn_previewPrev').attr("disabled", true);
            }
        });

        $('#btn_previewNext').click(function () {
            slideNo = $('#slideDiv').find('.slides').length;
            $('#btn_previewPrev').attr("disabled", false);
            var x = parseInt(previewIndex);
            x = x + 1;
            previewIndex = x;
            if (previewIndex >= slideNo) {
                $('#btn_previewNext').attr("disabled", true);
            }
            else {
                $('#btn_previewNext').attr("disabled", false);
            }
            $('#previewFrame').attr('src', 'previewPage.aspx?pageNo=' + previewIndex + '&type=content-ppt');
            $('#slideNoDiv').html('Slide No: ' + previewIndex);
        });

        $('#btn_previewOk').click(function () {
            $('#previewBoard').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('slow');
            });
        });

        $('#btn_profile').click(function () {
            $('.fullOverlay').fadeIn('slow', function () {
                $('#prevboxFrame').attr('src', 'profilePreview.aspx');
                $('#previewBox').show();
                $('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
                $('#previewBox').fadeIn('slow');
            });
        });



        $('#close_x').click(function () {
            $('#previewBoard').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast');
            });
        });


        $('#closeNew').click(function () {
            $('#previewBox').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast');
            });
        });

        $('#btn_finish').click(function () {


            var details = "<div>";
            var W = $('#workSpace-ppt').offset();
            list = $('#workSpace-ppt').find('.demo');
            var x = list.length;
            for (var i = 0; i < list.length; i++) {
                var height = $(list[i]).css('height');
                var width = $(list[i]).css('width');
                var left = $(list[i]).offset().left - W.left;
                var top = $(list[i]).offset().top - W.top;
                var innerContent = $(list[i]);
                var data = $(innerContent).html();


                details = details + '<div class = "saveElem"><div class = "height">' + height + '</div><div class = "width">' + width + '</div><div class = "top">' + top + '</div><div class = "left">' + left + '</div><div class = "data"> ' + data + '</div></div>';

            }
            details = details + "</div>";


            $.ajax({
                url: "content_page_pptNew.aspx/saveFile_ppt",
                data: "{ 'contents': '" + details + "','slideNo':'" + currSlide + "','MusicFile':'" + musicFile + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    save = true;
                    $('.innerMsgRibbon').text('Saved');
                    $('.messageRibbon').show().fadeOut(2000);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                }
            });



        });
        $('#btn_close').click(function () {
            if (confirm('Any unsaved changes will be discarded...\nSure you want to close?')) {
                window.location = 'LessonManagement.aspx';
            }
        });

        $('#text').dblclick(function () {
            var divVal = '<div class = "demo" style = "position:absolute; height:200px;width:300px;"> <div class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div> <div class = "selectable" style = "border:1px dashed red; width:100%; height:100%; background-color:white;overflow:hidden;" ondblclick ="textBoard(this);"></div> </div>'

            $('#workSpace-ppt').append(divVal);
            hoverMenu();
            $('.demo').draggable({ containment: "parent" }).resizable();


            $('.selectable').dblclick(function () {

                textBoard(this);
                $('#elm1_ifr').css('height', '470px');


            });


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
            var videoElement = '<div class="selectable demo" style ="width:320px; height:200px; padding:3px;position:absolute;"> <div class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /><img class="smallEdit" src="images/edit-icon.png" onclick="editVideo(this)" /></div><div class="vidDiv" style="height:100%"><object type="application/x-shockwave-flash" data="Repository/videos/player_flv_maxi.swf" width="100%" height="100%"><param name="wmode" value="transparent" /><param name="movie" value="player_flv_maxi.swf" /><param name="FlashVars" value="flv=" /></object></div></div>';

            $('#workSpace-ppt').append(videoElement);

            hoverMenu();
            $('.demo').draggable({ containment: "parent" }).resizable();

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

        $('#btn_vidPropDone').click(function () {
            selectedVideo = selectedVideo.replace('Repository/videos/', '');
            var videoElement = '<div class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /><img class="smallEdit" src="images/edit-icon.png" onclick="editVideo(this)" /></div><object type="application/x-shockwave-flash" data="Repository/videos/player_flv_maxi.swf" width="100%" height="100%"><param name="movie" value="player_flv_maxi.swf" /><param name="FlashVars" value="flv=' + selectedVideo + '" /></object>';
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


        $("#workSpace-ppt").droppable({
            accept: ".draggable",
            drop: function (event, ui) {

                var W = $('#workSpace-ppt').offset();


                var newElement = ui.draggable.clone();
               // var oldSrc = $(newElement).attr('src');
                //var newSrc = oldSrc.replace('thumbnails', 'images');
                if ((newElement).hasClass('draggable')) {
                    $(newElement).removeClass('draggable');
                    $(newElement).removeClass('draggable2');
                   // $(newElement).attr('src', newSrc);
                    $(newElement).css({ height: '100%', width: '100%', margin: '0px' });
                    newElement = $('<div style="position:absolute;height:200px;width:200px;top:' + (W.top + 5) + 'px;left:' + (W.left + 5) + 'px;" class="demo"><div  class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div></div>').append(newElement);
                    $(newElement).appendTo(this);
                    $('.demo').draggable({ containment: "parent" }).resizable();

                    hoverMenu();

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
            var W = $('#workSpace-ppt').offset();

            if (save == false) {

                var details = "<div>";

                list = $('#workSpace-ppt').find('.demo');
                var x = list.length;

                for (var i = 0; i < list.length; i++) {
                    var height = $(list[i]).css('height');
                    var width = $(list[i]).css('width');
                    var left = $(list[i]).offset().left - W.left;
                    var top = $(list[i]).offset().top - W.top;
                    var innerContent = $(list[i]);
                    $(innerContent).find('.ui-resizable-handle').remove();
                    $(innerContent).find('selectable ui-draggable').remove();
                    var data = $(innerContent).html();

                    details = details + '<div class = "saveElem"><div class = "height">' + height + '</div><div class = "width">' + width + '</div><div class = "top">' + top + '</div><div class = "left">' + left + '</div><div class = "data"> ' + data + '</div></div>';

                }
                details = details + "</div>";

                $('#loadingOverlay').show();
                $('#loading').show();


                $.ajax({
                    url: "content_page_pptNew.aspx/saveFile_ppt",
                    data: "{ 'contents': '" + details + "','slideNo':'" + currSlide + "', 'MusicFile':'" + musicFile + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        currSlide = selectedSlide;
                        $('#loadingOverlay').hide();
                        $('#loading').hide();
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                        save = false;
                        $('#loadingOverlay').hide();
                        $('#loading').hide();
                    }
                });
            }
            slideNo++;
            currSlide = slideNo;
            selectedSlide = slideNo;
            musicFile = '';
            $('#palyer').empty();
            $('.slides').removeClass('slideSelected');
            $('#slideDiv').append('<div id="slide' + slideNo + '" class="slides slideSelected" onclick="slideClick(this)">' + slideNo + '</div>');
            $('#workSpace-ppt').empty();
            save = false;
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
                    $('#workSpace-ppt').empty();
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

    $(document).ready(function () {


        $('#btnSave').click(function () {

            var details = "<div>";
            list = $('#workSpace-ppt').find('.demo');
            var x = list.length;
            for (var i = 0; i < list.length; i++) {
                var height = $(list[i]).css('height');
                var width = $(list[i]).css('width');
                var top = $(list[i]).css('top');
                var left = $(list[i]).css('left');
                var innerContent = $(list[i]);
                $(innerContent).find('.ui-resizable-handle').remove();
                $(innerContent).find('selectable ui-draggable').remove();
                var data = $(innerContent).html();


                details = details + '<div class = "saveElem"><div class = "height">' + height + '</div><div class = "width">' + width + '</div><div class = "top">' + top + '</div><div class = "left">' + left + '</div><div class = "data"> ' + data + '</div></div>';

            }
            details = details + "</div>";
            $.ajax({
                url: "content_page_pptNew.aspx/saveFile_ppt",
                //data: "{ 'contents':'" + details + "','musicFile':'" + musicFile + "' }",
                data: "{ 'contents': '" + details + "','slideNo':'" + currSlide + "','MusicFile':'" + musicFile + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    save = false;
                    $('.innerMsgRibbon').text('Saved');
                    $('.messageRibbon').show().fadeOut(2000);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

        });

        $('#btnClear').click(function () {

            $('#workSpace-ppt').empty();

        });

        $('#btnRegen').click(function () {

            var listNew = $('#resultDiv').find(".saveElem");

            for (var i = 0; i < listNew.length ; i++) {

                var height = $(listNew[i]).find('.height').html();
                var width = $(listNew[i]).find('.width').html();
                var top = parseFloat($(listNew[i]).find('.top').html());
                var left = parseFloat($(listNew[i]).find('.left').html());
                var data = $(listNew[i]).find('.data').html();

                data = data.replace(/&gt;/g, '>');
                data = data.replace(/&lt;/g, '<');

                $('#workSpace-ppt').append('<div class = "demo" style = "position:absolute; height:' + height + '; width:' + width + ';top:' + (top + W.top) + 'px;left:' + (left + W.left) + 'px">' + data + '</div>');




            }

            $('.demo').draggable({ containment: "parent" }).resizable();


        });

    });


    //////////////////////
    //EXTERNAL FUNCTIONS
    //////////////////////
    var save = false;

    function editVideo(e) {

        selected = $(e).parents('.demo');

        $('#topRibbon > div').not('#close_ribbon').hide();
        $('#vidProp,#ribbonVideoListContainer,#UpdatePanel2,#ribbonVideoList,#dragDown').show();

        $('#topRibbon').slideDown('slow', 'linear', function () {
            $('.container').show();
            var container = $(".container")
            container.shuffleLetters();
        });

    }


    function deleteItem(e) {
        if (confirm('Do you want to delete?')) {
            $(e).parents('div.demo').remove();
        }
    }

    function hoverMenu() {
        $('.demo').mouseover(function () {
            $(this).find('.menuButtons').show();
            //$(this).css('border', '1px dotted Red');
        });

        $('.demo').mouseout(function () {
            $(this).find('.menuButtons').hide();
            //$(this).css('border', 'none');
        });

    }

    function saveFn() {

        var retValue = false;
        var details;
        $('#loadingOverlay').show();
        $('#loading').show();
        list = $('#workSpace-ppt').find('.demo');
        var x = list.length;
        for (var i = 0; i < list.length; i++) {
            var height = $(list[i]).css('height');
            var width = $(list[i]).css('width');
            var top = $(list[i]).css('top');
            var left = $(list[i]).css('left');
            var innerContent = $(list[i]);
            $(innerContent).find('.ui-resizable-handle').remove();
            $(innerContent).find('selectable ui-draggable').remove();
            var data = $(innerContent).html();


            details = details + '<div class = "saveElem"><div class = "height">' + height + '</div><div class = "width">' + width + '</div><div class = "top">' + top + '</div><div class = "left">' + left + '</div><div class = "data"> ' + data + '</div></div>';


        }

        $.ajax({
            url: "content_page_pptNew.aspx/saveFile_ppt",
            //data: "{ 'contents':'" + details + "','musicFile':'" + musicFile + "' }",
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
        var W = $('#workSpace-ppt').offset();


        //alert(save);
        if (save == false) {

            var details = "<div>";

            list = $('#workSpace-ppt').find('.demo');
            var x = list.length;

            for (var i = 0; i < list.length; i++) {
                var height = $(list[i]).css('height');
                var width = $(list[i]).css('width');
                var left = $(list[i]).offset().left - W.left;
                var top = $(list[i]).offset().top - W.top;
                var innerContent = $(list[i]);
                $(innerContent).find('.ui-resizable-handle').remove();
                $(innerContent).find('selectable ui-draggable').remove();
                var data = $(innerContent).html();

                details = details + '<div class = "saveElem"><div class = "height">' + height + '</div><div class = "width">' + width + '</div><div class = "top">' + top + '</div><div class = "left">' + left + '</div><div class = "data"> ' + data + '</div></div>';


            }
            details = details + "</div>";


            $('#loadingOverlay').show();
            $('#loading').show();

            //alert(slideNo + "/" + currSlide);

            $.ajax({
                url: "content_page_pptNew.aspx/saveFile_ppt",
                data: "{ 'contents': '" + details + "','slideNo':'" + currSlide + "', 'MusicFile':'" + musicFile + "' }",
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

                    save = false;
                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                }
            });


            $('.slides').removeClass('slideSelected');
            $(e).addClass('slideSelected');


            $('#loadingOverlay').show();
            $('#loading').show();
            $('#workSpace-ppt').empty();

            $.ajax({
                url: "content_page_pptNew.aspx/getSlideDatas",
                data: "{ 'slideNo': '" + selectedSlide + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    var contents = data.d;
                    var content_array = contents.split('^');
                    currSlide = selectedSlide;
                    var listNew = $(content_array[0]).find('.saveElem');
                    var W = $('#workSpace-ppt').offset();

                    $('#workSpace-ppt').empty();
                    for (var i = 0; i < listNew.length; i++) {

                        var height = $(listNew[i]).find('.height').html();
                        var width = $(listNew[i]).find('.width').html();
                        var top = parseFloat($(listNew[i]).find('.top').html());
                        var left = parseFloat($(listNew[i]).find('.left').html());
                        var data = $(listNew[i]).find('.data').html();

                        data = data.replace(/&gt;/g, '>');
                        data = data.replace(/&lt;/g, '<');

                        $('#workSpace-ppt').append('<div class = "demo" style = "position:absolute; height:' + height + '; width:' + width + ';top:' + (top + W.top) + 'px;left:' + (left + W.left) + 'px">' + data + '</div>');



                    }
                    hoverMenu();
                    $('.demo').draggable({ containment: "parent" }).resizable();



                    $('#loadingOverlay').hide();
                    $('#loading').hide();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {


                }
            });


        }
        else {

            $('.slides').removeClass('slideSelected');
            $(e).addClass('slideSelected');


            $('#loadingOverlay').show();
            $('#loading').show();

            $.ajax({
                url: "content_page_pptNew.aspx/getSlideDatas",
                data: "{ 'slideNo': '" + selectedSlide + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    var contents = data.d;
                    var content_array = contents.split('^');
                    currSlide = selectedSlide;
                    var listNew = $(content_array[0]).find('.saveElem');

                    $('#workSpace-ppt').empty();
                    for (var i = 0; i < listNew.length; i++) {

                        var height = $(listNew[i]).find('.height').html();
                        var width = $(listNew[i]).find('.width').html();
                        var top = parseFloat($(listNew[i]).find('.top').html());
                        var left = parseFloat($(listNew[i]).find('.left').html());
                        var data = $(listNew[i]).find('.data').html();

                        data = data.replace(/&gt;/g, '>');
                        data = data.replace(/&lt;/g, '<');

                        $('#workSpace-ppt').append('<div class = "demo" style = "position:absolute; height:' + height + '; width:' + width + ';top:' + (top + W.top) + 'px;left:' + (left + W.left) + 'px">' + data + '</div>');




                    }
                    hoverMenu();
                    $('.demo').draggable({ containment: "parent" }).resizable();




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


        var x = $(selected).html(contents);
        $('.demo selectable').find('p').addClass('selectable');
        $('#textEditorDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('slow');
        });


    }

</script>
<script type="text/javascript">
    function textBoard(e) {
        selected = $(e);

        //CLEAR THE TEXT EDITOR AND FILLING IT WITH THE TD CONTENTS
        var insideContent = $(selected).html();

        tinymce.get('elm1').setContent('');
        tinyMCE.execCommand('mceInsertContent', false, insideContent);



        $('.fullOverlay').fadeIn('slow', function () {
            $('#previewBoard').hide();
            $('#textEditorDiv').fadeIn();
            $('.demo').draggable({ containment: "parent" }).resizable();
            //$('#textEditorDiv').animate({ top: '20px' }, 800, 'linear');
        });



    }

</script>
<script type="text/javascript" src="scripts/jquery.shuffleLetters.js"></script>


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
</html>
