<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentBinder_CoinLessons.aspx.cs" Inherits="Phase002_1_StudentBinder_CoinLessons" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" id="sized" />

    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/matchingLesson.css" rel="stylesheet" type="text/css" />
    <link href="styles/jquery.ui.resizable.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="styles/commonStyle.css" rel="stylesheet" />

    <style>
        .searchBtn1 {
            float: right;
            margin: 0px;
        }

        /*select {
            border: none;
        }

            select option {
                background-color: #A2F0AF;
                border: 1px groove #10641E;
                margin: 2px;
                padding: 3px;
                cursor: pointer;
            }*/

                select option:hover {
                    background-color: #6EE782;
                }

        iframe {
            border: medium none;
            height: 465px;
            overflow: auto;
            width: 100%;
        }

        .menuBox {
            width: 110px;
            border: 1px groove red;
            position: absolute;
            background-color: white;
        }

            .menuBox .rightMenuList li {
                padding: 2px;
                margin: 1px;
                list-style: none;
            }

                .menuBox .rightMenuList li:hover {
                    background-color: #c3c1c1;
                    cursor: pointer;
                }

        .renameDiv {
            position: absolute;
            z-index: 1001;
            margin-top: 50px;
        }

        .renameSetOk, .renameSetNo, .renameStepOk, .renameStepNo {
            background-color: #C9C9C9;
            border: 1px groove #8C8C8C;
            margin: 2px;
            font-size: 11px;
        }

            .renameSetOk:hover, .renameSetNo:hover, .renameStepOk:hover, .renameStepNo:hover {
                background-color: #E8E8E8;
                cursor: pointer;
            }

        .renameText {
            height: 20px;
            border: 1px groove black;
            color: blue;
        }

        .auto-style1 {
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

    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>

    <script type="text/javascript" src="scripts/ui/jquery.ui.resizable.js"></script>

    <script src="scripts/jsForTextEditor/tiny_mce/tiny_mce.js" type="text/javascript"></script>

    <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>
    <script type="text/javascript">
        $(function ($) {
            $('.jclock').jclock();
        });


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
    </script>

    <!-- SCRIPT TO RENAME STEP AND SETS -->
    <script type="text/javascript">
        var selOption;
        var selSetStep;
        var selText;
        function makeAllDraggable() {
            $('.draggable').draggable({
                revert: "invalid",
                helper: "clone",
                cursor: "move"
            });
        }
        function makeAllDroppable() {
            $('.droppable').droppable({
                accept: ".images,.audios",
                drop: function (event, ui) {

                    if ($(ui.draggable).hasClass('images')) {


                        var newElement = ui.draggable.clone();

                        if ($(this).find('.vidDiv').length == 0) {
                            if ((newElement).hasClass('draggable')) {
                                $(newElement).removeClass('draggable');
                                $(newElement).css({ 'margin': '0px', 'height': '100%', 'width': '100%' });
                                newElement = $('<div style="position:absolute;width:50px;height:50px;" class="demo"><div  class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div></div>').append(newElement);
                                $(newElement).appendTo(this);
                                $(newElement).draggable({ containment: "parent" }).resizable({ maxWidth: 410, maxHeight: 170 });

                                hoverMenu();

                            }
                        }
                    }
                    if ($(ui.draggable).hasClass('audios')) {
                        if ($(this).find('.player').length > 0) {


                            var newElement = ui.draggable;
                            musicFile = $(newElement).attr('alt').replace('~/VisualTool/', '');


                            $(this).find('.player').empty();
                            $(this).find('.player').append('<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=' + musicFile + '&showslider=0&width=25" /><param name="wmode" value="transparent" /></object>');

                        }
                        else {
                            alert('Enable audio first.');
                        }
                    }

                }
            });
        }
        $(document).ready(function () {
            listBoxMenu();
            makeAllDraggable();
            makeAllDroppable();
        });


        //EXTERNAL FUNCTIONS//

        function listBoxMenu() {
            $('#ListBox1,#ListBox2').bind("contextmenu", function (event) {
                event.preventDefault();

                selctList = $(this).attr('id');


                if (selctList == 'ListBox1') {
                    selOption = $('#ListBox1 option:selected');

                }
                else {
                    selOption = $('#ListBox2 option:selected');
                }
                selSetStep = selOption.attr('value');
                selText = selOption.html();
                var listboxId = selOption.parent().attr('id');



                $('.menuBox').remove();
                var menuBox = "<div class='menuBox' style='top:" + event.pageY + "px;left:" + event.pageX + "px'><ul class='rightMenuList'><li class='menuRename'>Rename</li></ul></div>"

                $(menuBox).appendTo("body");


                $('.menuRename').click(function (event) {
                    if (listboxId == "ListBox1") {
                        $('.renameSetText').val(selText);
                        $('.renameAreaSet').fadeIn();

                        $('.renameSetText').focus();
                        $('.renameSetText').select();
                    }
                    if (listboxId == "ListBox2") {
                        $('.renameStepText').val(selText);
                        $('.renameAreaStep').fadeIn();

                        $('.renameStepText').focus();
                        $('.renameStepText').select();

                    }
                });



            });


            $('.renameSetNo').click(function () {
                $('.renameSetText').val('');
                $('.renameAreaSet').fadeOut();

            });

            $('.renameSetOk').click(function () {

                var e = $("#ListBox2");
                var selStep = e.val();
                var value = $('.renameSetText').val();

                fn_renamer(selSetStep, value, selOption);
            });

            $('.renameStepNo').click(function () {
                $('.renameStepText').val('');
                $('.renameAreaStep').fadeOut();

            });

            $('.renameStepOk').click(function () {

                var e = $("#ListBox1");
                var selSet = e.val();
                var value = $('.renameStepText').val();

                fn_renamer(selSetStep, value, selOption);
            });

            $(document).bind("click", function (event) {
                $('.menuBox').remove();
            });
        }


        function stripIt(x) {
            x.value = x.value.replace(/['"]/g, '');
        };

        function fn_renamer(S_No, value, control) {
            $.ajax({
                url: "coinLessons.aspx/rename",
                data: "{'S_No':'" + S_No + "','value':'" + value + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {

                    $('.renameSetText').val('');
                    $('.renameAreaSet').fadeOut();

                    $('.renameStepText').val('');
                    $('.renameAreaStep').fadeOut();


                    control.html(value);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);

                }
            });
        }


    </script>
</head>
<body>
    <!-- top panel -->
    <form runat="server" id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- dashboard container panel -->
        <div id="db-container">
            <!-- header -->
            <!-- content panel -->

            <!-- content Left side -->

            <div class="clear"></div>
            <!-- content right side -->
            <div id="dashboard-RHSVin">

                <div class="dashboard-RHS-content">
                    <h2 class="cokin">Coin

                        <span id="td_LP" runat="server" class="txtCSS"></span>
                    </h2>
                    <div class="clear"></div>
                    <hr />











                    <!-- NEW CODES -->
                    <table style="width: 100%; vertical-align: top;" cellpadding="0" cellspacing="0">
                        <%--<tr>
                            <td id="td_LP" runat="server" colspan="2" class="txtCSS"></td>
                        </tr>--%>
                        <div class="clear"></div>
                        <tr>
                            <td width="3%" rowspan="2" style="vertical-align: top;">

                                <div class="btn btn-blue"></div>
                                <div id="btn_preview" style="margin-top: 2px;" title="Preview" class="with-tip">
                                    <div class="btn btn-purple"></div>
                                </div>
                                <div id="btn_finish" style="margin-top: 2px;" title="Save" class="with-tip">
                                    <div class="btn btn-black"></div>
                                </div>
                                <div id="btn_close" style="margin-top: 2px;" title="Close" class="with-tip">
                                    <div class="btn btn-redd"></div>
                                </div>



                                <div id="addDist" style="margin-top: 2px;" title="Add Distractor" class="with-tip">
                                    <div class="btn btn-green1"></div>
                                    <div id="deleteDist" style="margin-top: 2px;" title="Delete Distractor" class="with-tip">
                                        <div class="btn btn-redd1"></div>
                                    </div>
                                </div>
                            </td>
                            <td style="width: 15%; border-right: 1px solid #E8E8E8; padding: 10px;">
                                <asp:UpdatePanel runat="server" ID="up_setStep" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="set-step" style="width: 100%;">
                                            <div class="setDiv">
                                                <div class="renameAreaSet" style="display: none;">
                                                    <div class="waitBlank rename" style="width: 110px; height: 207px; display: block; opacity: .8;">
                                                    </div>
                                                    <div class="renameDiv">

                                                        <input type="text" class="renameText renameSetText" style="width: 100px;" onblur='stripIt(this);' /><br />
                                                        <input type="button" class="renameSetOk" value="DONE" />
                                                        <input type="button" class="renameSetNo" value="CANCEL" />
                                                    </div>
                                                </div>
                                                <asp:UpdateProgress runat="server" ID="UpdateProgress2" AssociatedUpdatePanelID="up_setStep" DisplayAfter="100">
                                                    <ProgressTemplate>
                                                        <div class="waitBlank" style="width: 100px; height: 207px;">
                                                            <div style="margin: 75% auto auto 33%;">
                                                                <asp:Image ID="Image2" runat="server" src="icons/wait(s).gif" />

                                                            </div>
                                                        </div>
                                                    </ProgressTemplate>

                                                </asp:UpdateProgress>
                                                <table class="display">
                                                    <tr>
                                                        <td class="spanSet">SETS</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="float: left; width: 90%">
                                                                <asp:ListBox ID="ListBox1" runat="server" Width="100%" Height="130px" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                                                            </div>
                                                            <div class="arrowBox">
                                                                <div class="upArrow">
                                                                    <asp:ImageButton ID="btn_setUp" runat="server" ImageUrl="~/VisualTool/icons/arrow_up.png" OnClick="btn_setUp_Click" />
                                                                </div>

                                                                <div class="downArrow">
                                                                    <asp:ImageButton ID="btn_setDown" runat="server" ImageUrl="~/VisualTool/icons/arrow_down.png" OnClick="btn_setDown_Click" />
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div title="Search" class="with-tip searchBtn1">
                                                                <div class="btn btn-red" style="margin-left: 25px;">
                                                                    <asp:ImageButton ID="imgBtn_setDelete" ImageUrl="~/VisualTool/images/DeleteSetLight.JPG" runat="server" OnClick="imgBtn_setDelete_Click" />
                                                                </div>
                                                            </div>
                                                            <div title="Search" class="with-tip searchBtn1">
                                                                <div class="btn btn-green" style="margin-left: 25px;">
                                                                    <asp:ImageButton ID="imgBtn_setAdd" ImageUrl="~/VisualTool/images/AddSetLight.JPG" runat="server" OnClick="imgBtn_setAdd_Click" />
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>


                                            </div>
                                            <div class="stepDiv">
                                                <%-- <asp:UpdatePanel runat="server" ID="UpdatePanel3"><ContentTemplate>--%>
                                                <div class="renameAreaStep" style="display: none;">
                                                    <div class="waitBlank rename" style="width: 110px; height: 207px; display: block; opacity: .8;">
                                                    </div>
                                                    <div class="renameDiv">

                                                        <input type="text" class="renameText renameStepText" style="width: 100px;" onblur='stripIt(this);' /><br />
                                                        <input type="button" class="renameStepOk" value="DONE" />
                                                        <input type="button" class="renameStepNo" value="CANCEL" />
                                                    </div>
                                                </div>
                                                <asp:UpdateProgress runat="server" ID="updateProgress3" AssociatedUpdatePanelID="up_setStep" DisplayAfter="100">
                                                    <ProgressTemplate>
                                                        <div class="waitBlank" style="width: 100px; height: 202px;">
                                                            <div style="margin: 75% auto auto 33%;">
                                                                <asp:Image ID="Image3" runat="server" src="icons/wait(s).gif" />

                                                            </div>
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                <table class="display">
                                                    <tr>
                                                        <td class="spanSet">STEPS</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="float: left; width: 90%">
                                                                <asp:ListBox ID="ListBox2" runat="server" Width="100%" Height="130px" OnSelectedIndexChanged="ListBox2_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                                                            </div>
                                                            <div class="arrowBox">
                                                                <div class="upArrow">
                                                                    <asp:ImageButton ID="btn_stepUp" runat="server" ImageUrl="~/VisualTool/icons/arrow_up.png" OnClick="btn_stepUp_Click" />
                                                                </div>

                                                                <div class="downArrow">
                                                                    <asp:ImageButton ID="btn_stepDown" runat="server" ImageUrl="~/VisualTool/icons/arrow_down.png" OnClick="btn_stepDown_Click" />
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div title="Delete Step" class="with-tip searchBtn1">
                                                                <div class="btn btn-red" style="margin-left: 25px;">
                                                                    <asp:ImageButton ID="imgBtn_stepDelete" ImageUrl="~/VisualTool/images/DeleteSetLight.JPG" runat="server" OnClick="imgBtn_stepDelete_Click" />
                                                                </div>
                                                            </div>
                                                            <div title="Add Step" class="with-tip searchBtn1">
                                                                <div class="btn btn-green" style="margin-left: 25px;">
                                                                    <asp:ImageButton ID="imgBtn_stepAdd" ImageUrl="~/VisualTool/images/AddSetLight.JPG" runat="server" OnClick="imgBtn_stepAdd_Click" />
                                                                </div>
                                                            </div>

                                                        </td>
                                                    </tr>
                                                </table>

                                                <%-- </ContentTemplate></asp:UpdatePanel>--%>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="width: 40%">
                                <div class="workSpace">
                                    <div class="ml_questionTemplate">
                                        <div class="ml_questionSet">
                                            <div class="ml_base droppable">
                                                <div class="tempMenu">
                                                    <img class="optText" onclick="insertTextBox(this,1)" src="icons/text-a.png" />
                                                    <img class="optAddVid" onclick="fn_optAddVid(this);" src="icons/video(tv).png" />
                                                    <img class="optAddAud" onclick="fn_optAddAud(this);" src="icons/audio(fade).png" />
                                                </div>
                                            </div>
                                            <div class="ml_baseLable"></div>
                                        </div>
                                    </div>
                                    <div class="ml_answerTemplate">
                                        <div class="ml_base droppable">
                                            <div class="tempMenu">
                                                <img class="optText" onclick="insertTextBox(this,0)" src="icons/text-a.png" />
                                                <div class="statusDiv">
                                                    <div class="status-btn wrong">
                                                        <img id="btn_ansImg" src="images/disable-icon.png" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <span class="leOptId" style="display: none;">0</span>
                                        <div class="ml_baseLable"></div>
                                    </div>
                                </div>
                            </td>
                            <td style="width: 50%">
                                <div class="repContainer">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <div class="elemListCont">
                                                <table class="display">
                                                    <tr>
                                                        <td style="width: 22%;">
                                                            <div class="elemOption">
                                                                <asp:ImageButton runat="server" ID="optImage" Style="padding-right: 10px;" ToolTip="Images" ImageUrl="~/VisualTool/images/image1.JPG" OnClick="optImage_Click" />
                                                            </div>
                                                            <div class="elemOption">
                                                                <asp:ImageButton runat="server" ID="optVideo" ToolTip="Videos" Style="padding-right: 10px;" ImageUrl="~/VisualTool/images/Video1.JPG" OnClick="optVideo_Click" />
                                                            </div>
                                                            <div class="elemOption">
                                                                <asp:ImageButton runat="server" ID="optAudio" ToolTip="Audios" ImageUrl="~/VisualTool/images/Audio.JPG" OnClick="optAudio_Click" />
                                                            </div>

                                                            <script type="text/javascript">


                                                            </script>

                                                        </td>
                                                        <td style="width: 5%;">
                                                            <asp:UpdateProgress runat="server" ID="updateProgress1" AssociatedUpdatePanelID="UpdatePanel1">
                                                                <ProgressTemplate>
                                                                    <div>
                                                                        <asp:Image ID="rightWait2" runat="server" src="icons/wait.gif" />
                                                                    </div>
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>

                                                        </td>
                                                        <td>
                                                            <div title="Search" class="with-tip searchBtn">
                                                                <div class="btn btn-orange">
                                                                    <asp:ImageButton ID="imgSearch" ImageUrl="~/VisualTool/images/grayzoom.png" runat="server" OnClick="imgSearch_Click" />
                                                                </div>
                                                            </div>
                                                            <asp:TextBox ID="TextBox1" runat="server" CssClass="searchTextBox" value="Search File name" onBlur="if(this.value=='') this.value='Search File name'" autocomplete="off" onFocus="if(this.value =='Search File name' ) this.value=''"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <div class="datalistDiv">

                                                                <asp:DataList ID="DataList1" runat="server" RepeatColumns="5"
                                                                    Width="100%" RepeatDirection="Horizontal">
                                                                    <ItemTemplate>
                                                                        <div class="dl_images">
                                                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Thumbnail") %>' Width="50px" Height="50px" Style="margin: 3px 10px 3px 10px;" CssClass='<%# "selectable draggable " + Eval("Type")%>' AlternateText='<%# Eval("Path") %>' ToolTip='<%# "Name: "+Eval("Name")+"\nDescription: "+Eval("Description") %>' />
                                                                            <asp:Label ID="lblname" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">



                                <div class="distractorContainer"></div>
                            </td>
                        </tr>
                    </table>
                    <div class="messageRibbon">
                        <div class="innerMsgRibbon"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>



            <!-- footer -->


        </div>


        <div id="previewBoardContainer" style="width: 100%; height: 100%;">
            <div id="previewBoard" class="web_dialog" style="top: 10%; left: 18%; display: none;">

                <div id="sign_up5">
                    <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                        <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                    <h3>Lesson Profile
               
                    </h3>
                    <hr />
                    <iframe id="previewFrame" style="width: 100%" scrolling="auto"></iframe>
                    <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                </div>
                <div id="previewClose"></div>

            </div>
        </div>


        <div id="prevBoxContainer" style="width: 100%; height: 100%;">
            <div id="previewBox" class="web_dialog" style="top: 7%; left: 1%; display: none;">

                <div id="signupNew">
                    <a id="closeNew" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                        <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                    <h3>Coin Editor Preview
               
                    </h3>
                    <hr />
                    <iframe id="prevboxFrame" style="width: 100%; height: 550px;"></iframe>
                </div>


            </div>

        </div>




        <%--  <div id="previewBoardContainer" style="left: 343px; width: 730px;">
            <div id="previewBoard" style="width: 1310px;">
                <iframe id="previewFrame" style="width: 100%" scrolling="auto"></iframe>
                <input id="btn_previewOk" type="button" value="Ok" />
            </div>
            <div id="previewClose"></div>

        </div>--%>
        <div id="textEditorDiv" style="height: 610px; margin-left: 25%">
            <textarea id="elm1" name="elm1" style="width: 100%; height: 70%;">
				
			</textarea>
            <input type="button" class="close_ribbon" onclick="insertText(tinyMCE.get('elm1').getContent()); return false;" value="Done" />
            <input type="button" class="close_ribbon" id="closeTextEditor" value="close" />
            <%--<input type="reset" name="reset" value="Reset" onclick="resetTesting()" />--%>
        </div>

        <div class="fullOverlay">
        </div>
    </form>
</body>

<script type="text/javascript">

    var saveQuestion = false;
    var saveDist = false;
    var selectedDist;
    var questionId;
    var selDistId;
    var musicFile = "";
    var curentLessonId = 0;

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
        curentLessonId = GetQueryStringParams('curlesId');

        makeAllDraggable();
        makeAllDroppable();

        listBoxMenu();

        if (edit == '1') {

            //var lessonId = 1;
            // var lessDetId = 1;
            var optList = "";

            fn_getAllLeOptIds();


        }
        else {
            fn_addDist('Q');
            fn_addDist('W');
        }
        saveDist = true;

        $('#closeTextEditor').click(function () {
            $('#textEditorDiv').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('slow');
            });

        });

        $('#btn_close').click(function () {
            if ($($('.distTemplateSmall').find('.correct')).length > 0) {
                window.location = '../StudentBinder/CustomizeTemplateEditor.aspx?vlessonId=' + curentLessonId + '';
            }
            else {
                alert('Please select one correct answer');
            }
        });

        $('.ml_questionTemplate, .ml_answerTemplate').click(function () {
            $('.ml_questionTemplate, .ml_answerTemplate').css('border', 'none');
            $(this).css('border', '1px dashed #CC0000');

            if ($(this).attr('class') == 'ml_questionTemplate') {
                saveQuestion = false;
            }
            if ($(this).attr('class') == 'ml_answerTemplate') {
                saveDist = false;
            }
        });


        $('#addDist').click(function () {

            var newDistId = fn_addDist('W');
        });

        $('#deleteDist').click(function () {
            var selDistId = selectedDist.replace('opt-', '');

            deleteDist(selDistId);
        });



        $('.ml_questionTemplate,.ml_answerTemplate').mouseenter(function () {
            $(this).find('.tempMenu').show();
        });
        $('.ml_questionTemplate,.ml_answerTemplate').mouseleave(function () {
            $(this).find('.tempMenu').hide();
        });




        $('.statusDiv').click(function () {

            var selDivSmall = $('#' + selectedDist).find('.statusDivSmall');

            if ($(this).find('.status-btn').hasClass('correct')) {
                $(this).empty();
                $(this, selDivSmall).append('<div class="status-btn wrong"><img ID="btn_ansImg" src="images/disable-icon.png"/></div>');
                $(this, selDivSmall).find('.status-btn').css('background-color', 'red');

                $(selDivSmall).empty();
                $(selDivSmall).append('<div class="status-btn wrong"><img ID="btn_ansImg" src="images/disable-icon.png"/></div>');
                $(selDivSmall).find('.status-btn').css('background-color', 'red');
            }
            else {
                $(this).empty();
                $(this).append('<div class="status-btn correct"><img ID="btn_ansImg" src="images/enable-icon.png"/></div>');
                $(this).find('.status-btn').css('background-color', 'green');

                $(selDivSmall).empty();
                $(selDivSmall).append('<div class="status-btn correct"><img ID="btn_ansImg" src="images/enable-icon.png"/></div>');
                $(selDivSmall).find('.status-btn').css('background-color', 'green');
            }

        });

        $('#btn_finish').click(function () {

            fn_saveAnswer();

            //SAVING FUNTION
            //if (saveDist == false) {
            // EXTRACTING DATAS
            var details = "<div>";
            var W = $('.ml_questionTemplate');
            var Woff = $(W).offset();

            var label = $('.ml_questionTemplate').find('.ml_baseLable').html();
            list = $('.ml_questionTemplate').find('.demo');
            var distNo = questionId;
            var x = list.length;

            for (var i = 0; i < list.length; i++) {
                var height = $(list[i]).css('height');
                var width = $(list[i]).css('width');
                var left = $(list[i]).offset().left - Woff.left;
                var top = $(list[i]).offset().top - Woff.top;
                var innerContent = $(list[i]);
                var dataFull = $(innerContent).clone();
                $(dataFull).find('.ui-resizable-handle').remove();

                var data = $(dataFull).html();

                details = details + '<div class = "saveElem"><div class = "height">' + height + '</div><div class = "width">' + width + '</div><div class = "top">' + top + '</div><div class = "left">' + left + '</div><div class = "data"> ' + data + '</div></div>';

            }
            details = details + '<div class="music">' + musicFile + '</div>';
            details = details + '<div class="label">' + label + '</div>';
            details = details + "</div>";

            //EXTRACTING DATA END

            fn_saveTemp(details, "Q", questionId);
            //saveDist = true;
        });


        $('#btn_preview').click(function () {
            $('.fullOverlay').fadeIn('slow', function () {
                //showHtml();
                $('#prevboxFrame').attr('src', 'matchingLessonPreview.aspx?pageNo=1' + '&type=content-single');
                $('#previewBox').fadeIn();
                $('#textEditorDiv').hide();
            });

        });



        $('#btn_previewOk').click(function () {
            $('#previewBoard').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('slow');
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


        $('#btn_profile').click(function () {
            $('.fullOverlay').fadeIn('slow', function () {
                $('#previewFrame').attr('src', 'profilePreview.aspx');
                $('#previewBoard').fadeIn();
            });
        });



    });



    //External Functions//


    function fn_optAddAud(e) {

        if ($(e).parents('.ml_base').find('.vidDiv ').length == 0) {
            if ($(e).parents('.ml_base').find('.player').length == 0) {
                $(e).parents('.ml_base').append('<div class="player" style="float:left;margin-top:-21px;"><object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=&showslider=0&width=25" /><param name="wmode" value="transparent" /></object></div>');
                $(e).attr('src', 'icons/audio(solid).png');
            }
            else {
                $(e).parents('.ml_base').find('.player').remove();
                $(e).attr('src', 'icons/audio(fade).png');
            }
        }
        else {
            alert('Remove the video first');
        }
    }

    function fn_optAddVid(e) {
        if ($('.player').length == 0) {
            var videoElement = '<div class="selectable demo" style ="width:408px; height:165px; padding:3px; position: absolute;"> <div class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div><div class="vidDiv" style="height:100%"><object type="application/x-shockwave-flash" data="Repository/videos/player_flv_maxi.swf" width="100%" height="100%"><param name="wmode" value="transparent" /><param name="movie" value="player_flv_maxi.swf" /><param name="FlashVars" value="flv=123.flv" /></object></div></div>';

            if ($(e).parents('.ml_base').find('.demo').length == 0) {
                $(e).parents('.ml_base').append(videoElement);
            }
            else {
                alert('Please delete the existing items.');
            }

            hoverMenu();

            $('.vidDiv').droppable({
                accept: ".videos",
                drop: function (event, ui) {
                    var newElement = ui.draggable;
                    var fileName = $(newElement).attr('alt').replace('~/VisualTool/Repository/videos/', '');

                    //alert(fileName);
                    $(this).empty();
                    $(this).append('<object type="application/x-shockwave-flash" data="Repository/videos/player_flv_maxi.swf" width="100%" height="100%"><param name="wmode" value="transparent" /><param name="movie" value="player_flv_maxi.swf" /><param name="FlashVars" value="flv=' + fileName + '" /></object>');
                }
            });
        }
        else {
            alert('Remove the audio first');
        }
    }

    function insertTextBox(e, type) {

        if (type == 1)//type 1 is Question
        {
            if ($('.player').length != 0) {
                alert('Remove the audio first');
                return;
            }
            if ($(e).parents('.ml_base').find('.vidDiv ').length != 0) {
                alert('Remove the video first');
                return;
            }


        }

        var divVal = '<div class = "demo" style = "position:absolute; height:20px;width:75px;"> <div class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div> <div class = "selectable" style = "border:1px dashed red; width:100%; height:100%; background-color:white;overflow:hidden;" ondblclick ="textBoard(this);"></div> </div>'

        if ($(e).parents('.ml_base').find('.vidDiv').length == 0) {
            $(e).parents('.ml_base').append(divVal);
            hoverMenu();
            // $('.demo').find(".ui-resizable-handle").remove();
            $('.demo').draggable({ containment: "parent" }).resizable({ maxWidth: 410, maxHeight: 170 });

        }
    }

    function fn_clearAll() {

        $('.ml_questionSet .ml_base,.ml_questionSet .ml_baseLable,.ml_answerTemplate .ml_base,.ml_answerTemplate .ml_baseLable,.distractorContainer').empty();

        $('.ml_questionSet .ml_base').append('<div class="tempMenu"><img class="optText" onclick="insertTextBox(this,1)" src="icons/text-a.png" /><img class="optAddVid" onclick="fn_optAddVid(this)" src="icons/video(tv).png" /><img class="optAddAud" onclick="fn_optAddAud(this)" src="icons/audio(fade).png" /></div>');

        $('.ml_questionTemplate,.ml_answerTemplate').mouseenter(function () {
            $(this).find('.tempMenu').show();
        });
        $('.ml_questionTemplate,.ml_answerTemplate').mouseleave(function () {
            $(this).find('.tempMenu').hide();
        });

        //musicFile = "";
    }


    function fn_getAllLeOptIds() {
        fn_clearAll();


        $.ajax({
            url: "coinLessons.aspx/getLeOptId",
            data: "{}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var optList = data.d;

                var optL = optList.split(',');
                var i;
                for (i = 0; i < optL.length - 1 ; i++) {


                    var subOptList = optL[i].split('^');

                    if (subOptList[1] != 'Q') {
                        var distNo = $('.distractorContainer').find('.distTemplateSmall').length;
                        distNo++;
                        var statusString = "";

                        if (subOptList[1] == 'W') {
                            statusString = '<div class="status-btn wrong"><img src="images/disable-icon.png" /></div>';
                        }
                        else {
                            statusString = '<div class="status-btn correct"><img src="images/enable-icon.png" /></div>';
                        }


                        var newDist = '<div id="opt-' + subOptList[0] + '" class="distTemplateSmall"><div class="statusDivSmall">' + statusString + '</div><div class="tempB">' + distNo + '</div><div class="tempL"><span class="leOptId">' + subOptList[0] + '</span></div></div>';
                        $('.distractorContainer').append(newDist);


                        bindDistTempSmallClick('opt-' + subOptList[0]);

                    }

                    if (subOptList[1] == 'Q' || i == optL.length - 2) {
                        fn_fillTemplates(subOptList[0], subOptList[1]);
                    }

                }


                hoverMenu();



            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });


        // alert(optList);
    }

    function fn_fillTemplates(optId, status) {
        if (status != 'Q') {

            $('.ml_answerTemplate ml_base,.ml_answerTemplate ml_baseLable').empty();

            fn_distFillClick($('#opt-' + optId));
            //saveDist = false;
        }
        else {
            questionId = optId;
            saveQuestion = true;

            fn_fillQuestion(optId);
        }
    }


    function fn_fillQuestion(leOptId) {
        $.ajax({
            url: "coinLessons.aspx/getTempData",
            data: "{'leOptId':'" + leOptId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var recContent = data.d;
                var recContentArray = recContent.split('^');
                musicFile = "";

                if (recContent.indexOf("System.IndexOutOfRangeException") == -1) {

                    var listNew = $(recContentArray[0]).find('.saveElem');
                    var lblContent = $(recContentArray[0]).find('.label').html();
                    var musicFile = $(recContentArray[0]).find('.music').html();
                    var details = "<div>";
                    var W = '.ml_questionTemplate';
                    var Woff = $(W).offset();

                    for (var i = 0; i < listNew.length; i++) {

                        var height = $(listNew[i]).find('.height').html();
                        var width = $(listNew[i]).find('.width').html();
                        var top = parseFloat($(listNew[i]).find('.top').html()) + Woff.top;
                        var left = parseFloat($(listNew[i]).find('.left').html()) + Woff.left;
                        var data = $(listNew[i]).find('.data').html();


                        data = data.replace(/&gt;/g, '>');
                        data = data.replace(/&lt;/g, '<');


                        $(W + ' .ml_base').append('<div class = "demo" style = "height:' + height + '; width:' + width + ';top:' + top + 'px;left:' + left + 'px; position:absolute;">' + data + '</div>');

                    }

                    $('.ml_questionTemplate .ml_base .demo').draggable({ containment: "parent" }).resizable();

                    $(W + ' .ml_baseLable').append(lblContent);

                    if (musicFile != "") {

                        $('.player').remove();

                        $('.optAddAud').parents('.ml_base').append('<div class="player" style="float:left;margin-top:-21px;"></div>');
                        $('.optAddAud').attr('src', 'icons/audio(solid).png');


                        $('.player').empty();
                        $('.player').append('<object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="25" height="20"><param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=' + musicFile + '&showslider=0&width=25" /><param name="wmode" value="transparent" /></object>');
                    }
                }


                hoverMenu();

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });
    }

    function fn_getTempData(leOptId) {
        $.ajax({
            url: "coinLessons.aspx/getAllTempData",
            data: "{'leOptId':'" + leOptId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var recContent = data.d;

                var recContentArray = recContent.split('☺');

                // alert(recContentArray.length);
                for (k = 0; k < recContentArray.length - 1; k++) {

                    var recContSubArray = recContentArray[k].split('^');



                    if (recContent.indexOf("System.IndexOutOfRangeException") == -1) {
                        if (recContSubArray[1] != 'Q') {

                            var listNew = $(recContSubArray[0]).find('.saveElem');
                            var lblContent = $(recContSubArray[0]).find('.label').html();
                            var details = "<div>";
                            var W = '#ansTemp' + recContSubArray[2];
                            var Woff = $(W).offset();

                            for (var i = 0; i < listNew.length; i++) {

                                var height = $(listNew[i]).find('.height').html();
                                var width = $(listNew[i]).find('.width').html();
                                var top = parseFloat($(listNew[i]).find('.top').html()) + Woff.top;
                                var left = parseFloat($(listNew[i]).find('.left').html()) + Woff.left;
                                var data = $(listNew[i]).find('.data').html();


                                data = data.replace(/&gt;/g, '>');
                                data = data.replace(/&lt;/g, '<');


                                $('#ansTemp' + recContSubArray[2] + ' .ml_base').append('<div class = "demo" style = "height:' + height + '; width:' + width + ';top:' + top + 'px;left:' + left + 'px; position:absolute;">' + data + '</div>');

                            }



                            $('#ansTemp' + recContSubArray[2] + ' .ml_baseLable').append(lblContent);

                        }

                        else {
                            var listNew = $(recContSubArray[0]).find('.saveElem');
                            var lblContent = $(recContSubArray[0]).find('.label').html();
                            var details = "<div>";
                            var W = '.questionTemp';
                            var Woff = $(W).offset();

                            for (var i = 0; i < listNew.length; i++) {

                                var height = $(listNew[i]).find('.height').html();
                                var width = $(listNew[i]).find('.width').html();
                                var top = parseFloat($(listNew[i]).find('.top').html()) + Woff.top;
                                var left = parseFloat($(listNew[i]).find('.left').html()) + Woff.left;
                                var data = $(listNew[i]).find('.data').html();


                                data = data.replace(/&gt;/g, '>');
                                data = data.replace(/&lt;/g, '<');


                                $(W + ' .ml_base').append('<div class = "demo" style = "height:' + height + '; width:' + width + ';top:' + top + 'px;left:' + left + 'px; position:absolute;">' + data + '</div>');

                            }



                            $(W + ' .ml_baseLable').append(lblContent);

                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });
    }




    function fillTemplates(lessDetId) {
        fn_getTempData(tempOptId);
    }

    function fn_getLeOptIds(lessId) {
        $.ajax({
            url: "coinLessons.aspx/getLeOptId",
            data: "{'lessId':'" + lessId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var optList = data.d;
                var optL = optList.split(',');

                fn_getTempData(optList);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });


        // alert(optList);
    }

    function deleteDist(distId) {
        $.ajax({
            url: "coinLessons.aspx/deleteDist",
            data: "{'distId':'" + distId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var response = data.d;


                $('#' + selectedDist).hide();

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });
    }

    function fn_saveAnswer() {

        // EXTRACTING DATAS
        var details = "<div>";
        var W = $('.ml_answerTemplate');
        var Woff = $(W).offset();

        var label = $('.ml_answerTemplate').find('.ml_baseLable').html();
        list = $('.ml_answerTemplate').find('.demo');
        var distNo = $('.ml_answerTemplate').find('.leOptId').html();
        var x = list.length;

        for (var i = 0; i < list.length; i++) {
            var height = $(list[i]).css('height');
            var width = $(list[i]).css('width');
            var left = $(list[i]).offset().left - Woff.left;
            var top = $(list[i]).offset().top - Woff.top;
            var innerContent = $(list[i]);
            var dataFull = $(innerContent).clone();
            $(dataFull).find('.ui-resizable-handle').remove();

            var data = $(dataFull).html();

            details = details + '<div class = "saveElem"><div class = "height">' + height + '</div><div class = "width">' + width + '</div><div class = "top">' + top + '</div><div class = "left">' + left + '</div><div class = "data"> ' + data + '</div></div>';

        }
        details = details + '<div class="music"></div>';
        details = details + '<div class="label">' + label + '</div>';
        details = details + "</div>";

        //EXTRACTING DATA END

        var leoptId = $('.ml_answerTemplate').find('.leOptId').html();
        var status = 'W';
        if ($('.statusDiv').find('.status-btn').hasClass('correct')) {
            status = 'C';
        }

        fn_saveTemp(details, status, distNo);


    }

    function fn_distClick(e) {
        selectedDist = $(e).attr('id');

        $('.distTemplateSmall').css({ 'border': '4px groove #E8E8E8' });
        $(e).css('border', '4px solid #f00');
        var selDistId = selectedDist.replace('opt-', '');

        // fn_saveAnswer();

        fn_getTempData(selDistId);

        var distStatus = $(e).find('.status-btn');
        if ($(distStatus).hasClass('correct')) {
            $('.statusDiv').empty();
            $('.statusDiv').append('<div class="status-btn correct"><img id="btn_ansImg" src="images/enable-icon.png"></div>');
        }
        else {
            $('.statusDiv').empty();
            $('.statusDiv').append('<div class="status-btn wrong"><img id="btn_ansImg" src="images/disable-icon.png"></div>');
        }

        //alert(selDistId);

        $('.ml_answerTemplate').find('.leOptId').html(selDistId);
    }

    function fn_distFillClick(e) {
        selectedDist = $(e).attr('id');

        $('.distTemplateSmall').css({ 'border': '1px groove #000000' });
        $(e).css('border', '1px dashed #CC0095');
        var selDistId = selectedDist.replace('opt-', '');

        //fn_saveAnswer();

        fn_getTempData(selDistId);
        //alert(selDistId);

        $('.ml_answerTemplate').find('.leOptId').html(selDistId);
    }

    function bindDistTempSmallClick(tempId) {
        $('#' + tempId).click(function () {
            fn_distClick(this);
        });
    }

    function fn_addDist(dist_status) {
        $.ajax({
            url: "coinLessons.aspx/createDist",
            data: "{'status':'" + dist_status + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var optId = data.d;
                if (dist_status != 'Q') {
                    var distNo = $('.distractorContainer').find('.distTemplateSmall').length;
                    distNo++;

                    var newDist = '<div id="opt-' + optId + '" class="distTemplateSmall"><div class="statusDivSmall"><div class="status-btn wrong"><img src="images/disable-icon.png" /></div></div><div class="tempB">' + distNo + '</div><div class="tempL"><span class="leOptId">' + optId + '</span></div></div>';
                    $('.distractorContainer').append(newDist);

                    $('.ml_answerTemplate .ml_base,.ml_answerTemplate .ml_baseLable').empty();


                    fn_distClick('#opt-' + optId);

                    bindDistTempSmallClick('opt-' + optId);

                    //saveDist = false;
                }
                else {
                    questionId = optId;
                    saveQuestion = true;

                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });
    }

    function fn_saveTemp(objects, dist_status, leOptId) {
        $.ajax({
            url: "coinLessons.aspx/saveDist",
            data: "{'objects':'" + objects + "','status':'" + dist_status + "','leOptId':'" + leOptId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {

                $('.innerMsgRibbon').text('Saved');
                $('.messageRibbon').show().fadeOut(2000);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });
    }

    function fn_getTempData(leOptId) {
        $.ajax({
            url: "coinLessons.aspx/getTempData",
            data: "{'leOptId':'" + leOptId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var recContent = data.d;
                var recContentArray = recContent.split('^');

                $('.ml_answerTemplate .ml_base').empty();
                $('.ml_answerTemplate .ml_base').append('<div class="tempMenu" style="display: none;"><img src="icons/text-a.png" class="optText" onclick="insertTextBox(this,0);"><div class="statusDiv"></div></div></div>');
                $('.ml_answerTemplate .ml_baseLable').empty();

                if (recContent.indexOf("System.IndexOutOfRangeException") == -1) {
                    var recContentArray = recContent.split('^');
                    if (recContentArray[1] != 'Q') {

                        var listNew = $(recContentArray[0]).find('.saveElem');
                        var lblContent = $(recContentArray[0]).find('.label').html();
                        var details = "<div>";
                        var W = $('.ml_answerTemplate');
                        var Woff = $(W).offset();

                        for (var i = 0; i < listNew.length; i++) {

                            var height = $(listNew[i]).find('.height').html();
                            var width = $(listNew[i]).find('.width').html();
                            var top = parseFloat($(listNew[i]).find('.top').html()) + Woff.top;
                            var left = parseFloat($(listNew[i]).find('.left').html()) + Woff.left;
                            var data = $(listNew[i]).find('.data').html();


                            data = data.replace(/&gt;/g, '>');
                            data = data.replace(/&lt;/g, '<');

                            //alert($(data).find('.ui-resizable-handle').length);


                            $('.ml_answerTemplate .ml_base').append('<div class = "demo" style = "height:' + height + '; width:' + width + ';top:' + top + 'px;left:' + left + 'px; position:absolute;"><div  class="menuButtons"><img class="smallDelete" src="images/delete-icon.png" onclick="deleteItem(this)" /></div>' + data + '</div>');

                            hoverMenu();
                        }

                        $('.ml_answerTemplate .ml_baseLable').append(lblContent);

                        if (recContentArray[1] == 'W') {

                            $('.ml_answerTemplate .ml_base').find('.statusDiv').append('<div class="status-btn wrong"><img src="images/disable-icon.png" id="btn_ansImg">');
                        }
                        else {
                            $('.ml_answerTemplate .ml_base').find('.statusDiv').append('<div class="status-btn correct"><img src="images/enable-icon.png" id="btn_ansImg">');
                        }

                        $('.ml_answerTemplate .ml_base .demo').draggable({ containment: "parent" }).resizable();

                        $('.statusDiv').click(function () {

                            var selDivSmall = $('#' + selectedDist).find('.statusDivSmall');

                            if ($(this).find('.status-btn').hasClass('correct')) {
                                $(this).empty();
                                $(this, selDivSmall).append('<div class="status-btn wrong"><img ID="btn_ansImg" src="images/disable-icon.png"/></div>');
                                $(this, selDivSmall).find('.status-btn').css('background-color', 'red');

                                $(selDivSmall).empty();
                                $(selDivSmall).append('<div class="status-btn wrong"><img ID="btn_ansImg" src="images/disable-icon.png"/></div>');
                                $(selDivSmall).find('.status-btn').css('background-color', 'red');
                            }
                            else {
                                $(this).empty();
                                $(this).append('<div class="status-btn correct"><img ID="btn_ansImg" src="images/enable-icon.png"/></div>');
                                $(this).find('.status-btn').css('background-color', 'green');

                                $(selDivSmall).empty();
                                $(selDivSmall).append('<div class="status-btn correct"><img ID="btn_ansImg" src="images/enable-icon.png"/></div>');
                                $(selDivSmall).find('.status-btn').css('background-color', 'green');
                            }

                        });

                    }
                    else {
                        alert('No slide to display');
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });
    }




    $('.ml_baseLable').click(function () {

        textBoard(this);
        $('#elm1_ifr').css('height', '470px');
    });


    function insertText(contents) {


        var x = $(selected).html(contents);
        $('.demo selectable').find('p').addClass('selectable');
        $('#textEditorDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('slow');
        });


    }


    function textBoard(e) {
        selected = $(e);

        //CLEAR THE TEXT EDITOR AND FILLING IT WITH THE TD CONTENTS
        var insideContent = $(selected).html();

        tinymce.get('elm1').setContent('');
        tinyMCE.execCommand('mceInsertContent', false, insideContent);



        $('.fullOverlay').fadeIn('slow', function () {
            $('#previewBoard').hide();
            $('#textEditorDiv').fadeIn();
            // $('.demo').draggable().resizable();
        });



    }


    function hightlightSet(setNo) {
        alert(setNo);
        $("#ListBox1 option[value='" + setNo + "']").css("background-color", "Pink");
    }



    function setSelectedTab(type) {

        switch (type) {

        }
    }


    function deleteItem(e) {
        if (confirm('Do you want to delete?')) {
            $(e).parents('div.demo').remove();
        }
    }
    function hoverMenu() {
        $('.demo').mouseover(function () {
            $(this).find('.menuButtons').show();
        });

        $('.demo').mouseout(function () {
            $(this).find('.menuButtons').hide();
        });

    }

    function deleteFun() {
        alert('No deletion posible in this criteria. it needs minimum one set and one step !!! ');
    }
</script>


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
