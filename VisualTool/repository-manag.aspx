<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/VisualTool/repository-manag.aspx.cs"
    Inherits="repository_manag" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajasp" %>
<!DOCTYPE html>

<html>
<head>
     
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
     <title runat="server" id="TitleName">Melmark Pennsylvania</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <script src="scripts/swfobject.js" type="text/javascript"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/script.js"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="scripts/jquery.fileupload.js" type="text/javascript"></script>
    <script src="scripts/jquery.iframe-transport.js" type="text/javascript"></script>
    <script src="scripts/cookies.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/swfobject.js"></script>
    <script src="scripts/ajaxfileupload.js"></script>
    <script src="../Administration/JS/StopWatch.js"></script>
    <script src="../Administration/JS/jquery.jclock-1.3.js"></script>
    <script type="text/javascript">
        $(function ($) {
            $('.jclock').jclock();
        });


    </script>

    
    <link href="../Administration/CSS/homestyle.css" rel="stylesheet" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    
    <link href="styles/ContentPage.css" rel="stylesheet" type="text/css" />
    <link href="styles/repmanage.css" rel="stylesheet" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" type="text/css" />

     <style type="text/css" media="screen">
        html, body {
            height: 100%;
            background-color: white;
        }

        body {
            margin: 0;
            padding: 0;
           
        }
    </style>
    

    <style>
        #dashboard-RHSVis {
            background-color: #FFFFFF;
            border: 2px solid #B6D1DD;
            border-radius: 5px 5px 5px 5px !important;
            box-shadow: 0 1px 5px 3px #BFC0C1;
            margin: 0 auto 15px;
            padding: 5px;
            width: 90%;
        }

        div.commonContainer {
            width: 62%;
            height: 325px;
            margin: 120px auto 120px auto;
        }

            div.commonContainer div.gryContainer {
                width: 222px;
                height: 283px;
                background: #dddddd;
                float: left;
                margin: 0 10px 0 0;
                padding: 20px 10px 10px 10px;
            }

            div.commonContainer div.nomarg {
                margin: 0;
            }

            div.commonContainer div.gryContainer:hover {
                background: #95cd80;
                color: #000;
            }

        .langinFieldset legend {
            padding: 2px;
        }

        .langinFieldset {
            width: 100%;
            padding: 0px;
            border: 0;
            margin: 35px auto 0 auto;
        }

        .opt-header {
            border-bottom: 1px solid #998A60;
            border-top: 1px solid #998A60;
            color: #333332;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 17px;
            font-weight: normal;
            margin-top: 106px;
            padding: 6px 0;
            text-align: center;
        }
    </style>
    <style type="text/css">
        /*/*.style {
            width: 138px;
        }

        .style2 {
        }

        .style3 {
            width: 77px;
        }

        .style4 {
            width: 100%;
        }*/*/

        .imgthumbHover {
            cursor: pointer;
        }

        .iframePopUp {
            /*background-color: black;
            border: 1px groove black;
            display: none;
            left: 40%;
            position: absolute;
            top: 210px;
            z-index: 1001;*/
            position: fixed;
            background-color: #000000;
            border: 1px groove #000000;
            display: none;
            left: 29.9%;
            padding-left: 42px;
            margin-top: -50px;
            width: 315px;
            z-index: 1001;
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

        #dt_mediaList td {
            width: 250px;
        }

        .selectedItem a {
            background: url("images/hovarro.png") no-repeat scroll left 3px transparent;
            font-size: 14px;
            font-weight: bold;
            color: rgb(255, 153, 0);
            padding: 0 0 0 23px;
            text-decoration: none;
            width: 98%;
            background-position: 0 -21px;
            transition: background 1s ease 0s;
            display: block;
        }

        .unselectedItem a {
            background: url("images/hovarro.png") no-repeat scroll left 3px transparent;
            color: #686A6A;
            display: block;
            font-size: 14px;
            font-weight: normal;
            padding: 0 0 0 23px;
            text-decoration: none;
            width: 98%;
            background-position: 0 -21px;
            transition: background 1s ease 0s;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function validate() {
            if (document.getElementById("<%=txt_fName.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Enter your Lesson Plan !!!!</dv> ";
                document.getElementById("<%=txt_fName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txt_descriptions.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Enter your Lesson Plan !!!!</dv> ";
                document.getElementById("<%=txt_descriptions.ClientID%>").focus();
                return false;
            }

            return true;

        }

    </script>

    <script type="text/javascript">
        var fileValid = false;
       
        function checkUplaod(elem, extList) {

            //var extList='jpg,gif';
            //alert(extension);
            var elemName = (elem.value).toLowerCase();
            var ext = elemName.split('.').pop();

            if (extList.indexOf(ext) != -1) {
                fileValid = true;
                getPostedFile();
            }
            else {
                fileValid = false;
                alert('Invalid file');
                //document.getElementById("file_upload").value = "";
                document.getElementById('file_upload').parentNode.innerHTML = document.getElementById('file_upload').parentNode.innerHTML;
            }
        }
        function checkValid() {
            if (fileValid) {
                // alert("true");
                return true;
            }
            else {
                //alert("false");
                return false;
            }
        }
    </script>

   


    <script type="text/javascript">

        function clearData() {
            $('#txt_fName,#txt_descriptions,#txt_keywords,#fileupload,#thumbupload').val('');
        }

        function getPostedFile() {

            var browser = navigator.appName;
            var filename = document.getElementById('file_upload').value;

            if (browser == "Microsoft Internet Explorer") {

                $.ajaxFileUpload
                (
                    {
                        url: 'repository-manag.aspx',
                        secureuri: false,
                        fileElementId: 'file_upload',
                        dataType: 'json',
                    }
                );
                //$("#file_upload").val(filename);
                isIE = 1;
                document.getElementById('spanFile').innerHTML = filename.split('\\')[filename.split('\\').length - 1];
            }
            else {
                document.getElementById('spanFile').innerHTML = filename.split('\\')[filename.split('\\').length - 1];
                var form = document.getElementById('form1');
                var file = document.getElementById('file_upload');
                var fd = new FormData();

                for (var i = 0; i < file.files.length; i++) {
                    fd.append('file_upload', file.files[i]);
                }

                var xhr = new XMLHttpRequest();
                //xhr.open(form.method, form.action, true);
                xhr.open("POST", "repository-manag.aspx", true);

                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 4 && xhr.status == 200) {
                        //alert('2');
                        //alert(xhr.responseText);
                    }
                };
                xhr.send(fd);
            }
        }
        function uploadVid() {

            //var filename = 'file';
            var file = document.getElementById('file_upload');
            //checkUplaod(file, extension);
            if ((file.value != "") || (isIE == 1) || (file.value != null)) {

                var div = document.getElementById('thumbnails');
                var btnUpd = document.getElementById('btn_upload');
                btnUpd.disabled = true;
                btnUpd.value = "Please Wait";
                div.innerHTML = "<img src='images/ajax-loader.gif' alt='loading' />";


                file.disabled = true;
                document.getElementById('img_PopClose').style.display = "none";
                PageMethods.UploadVid(SuccessUpVideo, FailurUpVideo);

            } else {
                alert("Select a file");
            }
        }
        function SuccessUpVideo(data) {
            //alert(data);
            var div = document.getElementById('thumbnails');
            var file = document.getElementById('file_upload');
            var btnUpd = document.getElementById('btn_upload');
            file.disabled = false;
            document.getElementById('img_PopClose').style.display = "block";
            var paths = data.split("*");

            //var div = document.getElementById('thumbnails');
            div.innerHTML = "";
            for (var count = 0; count < paths.length - 1; count++) {
                paths[count] = paths[count].replace("\\", "/");
                div.innerHTML += "<img src='Repository/videos/" + paths[count] + "' height='70px' width='70px' style='border:1.5px solid White;' class='imgthumbHover' onclick='selectThumb(this,this.src);' />    ";
            }
            btnUpd.disabled = false;
            btnUpd.value = "Upload";
        }
        function FailurUpVideo(error) {
            if (error) {

                var file = document.getElementById('file_upload');
                alert('Something went wrong!... Try Again');
                file.disabled = false;
                document.getElementById('img_PopClose').style.display = "block";
            }
        }
        function selectThumb(selImg, src) {

            var selSrc = $(selImg).attr('src');
            $.ajax({
                type: "POST",
                url: "repository-manag.aspx/setSelThumbNail",
                data: '{src: "' + selSrc + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function OnSuccess(response) {
                    
                    if (response.d == "success") {

                        document.getElementById('tbl_Submit').style.display = "block";
                        var imgUrl = selImg.attributes["src"].nodeValue;
                        $('#repository_thumb_Path').val(imgUrl);

                        //setCookie("thumb_Path", imgUrl, 1);
                        selImg.style.border = '1.5px solid Blue';
                        var siblings = selImg.parentNode.getElementsByTagName('img');
                        for (var i = 0; i < siblings.length; i++) {
                            if (siblings[i] != selImg) {
                                siblings[i].style.border = '1.5px solid White';
                            }
                        }



                    }
                    else {

                        alert("Please select your thumbnail again");

                    }
                }
         ,
                failure: function (response) {
                   
                    alert('Failed');

                }


            });






        }
       
        function RollBackAction() {

            var div = document.getElementById('thumbnails');
            div.innerHTML = "";
            document.getElementById('tbl_Submit').style.display = "none";
            PageMethods.DeleteIfCancel(function (data) { }, function (data) {
                //alert('Something went wrong!... Try Again'); 
            });

        }
        function PopUp(file) {
            $('.overlay').fadeIn('slow', function () {
                //$('#previewSWF').fadeIn('slow');
                if (swfobject.hasFlashPlayerVersion("6")) {

                    //file = "Repository/reinforcement/tangramone.swf";
                    // check if SWF hasn't been removed, if this is the case, create a new alternative content container
                    var c = document.getElementById("content");
                    if (!c) {
                        var d = document.createElement("div");
                        d.setAttribute("id", "content");
                        document.getElementById("content-container").appendChild(d);
                    }
                    var extension = file.substr((file.lastIndexOf('.') + 1));
                    switch (extension) {
                        case 'swf':// create SWF
                            file = file.replace('~/VisualTool/', '');
                            $('#previewBoard1').fadeIn('slow');
                            $('#previewBoard1').css("margin-left", "35%");
                            $('#previewBoard1').css("top", "30%");
                            var att = { data: file, width: "325", height: "250" };
                            var par = { menu: "false" };
                            var id = "content";
                            swfobject.createSWF(att, par, id);
                            break;
                        case 'gif':// create img for gif
                            $('#previewBoard1').fadeIn('slow');
                            $('#previewBoard1').css("margin-left", "35%");
                            $('#previewBoard1').css("top", "30%");
                            var c = document.getElementById("content");
                            if (c == null) {
                                var d = document.createElement("div");
                                d.setAttribute("id", "content");
                                document.getElementById("content-container").appendChild(d);
                                c = document.getElementById("content");
                            }
                            c.innerHTML = "<img src='" + file + "' height='250px' width='325px' />";
                            break;
                        default:
                            alert('Invalid file format');
                            $('.overlay').fadeOut('fast');
                    }

                }
            });
        }
       
        function loadmaster() {

            self.location = '../../LoginContinue.aspx';



        }
</script>


</head>

<body>
    <!-- top panel -->
    <form runat="server" id="form1">
      
       

        <div id="dashboard-top-panel" style="height:47px;margin:0 auto;padding:0; width:100%;">
            <div id="top-panel-container" >
                <ul style="margin-top:1%;margin-bottom:0px; margin-left:0px; margin-right:0px;">
                    <li class="user" style="width: 50%;">
                        <asp:Label ID="lblLoginName" runat="server" Text=""></asp:Label>
                    </li>
                    <li class="timeSs">
                        <div>
                            <div style="float: left; width: auto;"></div>
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
            <!-- header -->
            <div id="header-panel">
                <div class="Dashboard-logo">
                    <img src="../Administration/images/student-logo.jpg" alt="">

                    <a href="homePage.aspx" class="homecls" title="Home"></a>
                    <a href="LessonManagement.aspx" class="lessnClass" title="Lesson Management"></a>

                </div>

            </div>


            <!-- content panel -->
            <div id="dashboard-content-panel">
                <!-- content Left side -->


                <!-- content right side -->
                <div id="dashboard-RHS" style="padding-right:30px;">

                    <div class="dashboard-RHS-content" style="background-color:#FFFFFF;padding-top:15px;padding-bottom:15px;padding-left:15px;padding-right:15px;">

                        <div id="main-heading-panel" style="background: none repeat scroll 0 0 rgba(0, 0, 0, 0); color: #666666;font-family: Arial,Helvetica,sans-serif;
                         font-size: 18px; font-weight: bold;height: 16px; line-height: 0;margin: 0 auto; padding: 17px 0 0 1px;text-align: left; width: 98%;">Repository Management</div>
                        <div class="clear"></div>
                        <hr />

                        <!-- CHANGABLE -->

                        <ajasp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true"></ajasp:ToolkitScriptManager>
                        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>

                        <table style="width: 100%; margin-top: -10px;">
                            <tr>
                                <td style="width: 20%; height: 300px; border-right-style: dashed; border-right: 1px solid #d9d9d9; vertical-align: top;">
                                    <asp:BulletedList ID="nonAcc_menu" runat="server" class="nonAcc-menu" Style="margin: 0px;"
                                        DisplayMode="LinkButton" OnClick="nonAcc_menu_Click1">
                                        <asp:ListItem Value="images" class="selectedItem">Image</asp:ListItem>
                                        <asp:ListItem Value="videos" class="unselectedItem">Video</asp:ListItem>
                                        <asp:ListItem Value="audios" class="unselectedItem">Audio</asp:ListItem>
                                        <asp:ListItem Value="reinforcement" class="unselectedItem">Reinforcement</asp:ListItem>
                                    </asp:BulletedList>


                                </td>
                                <td>
                                    <div id="opt-container">
                                        <div class="option-icons-search" style="float: left; width: 90%; height: 32px;">

                                            <table style="width: 100%;">
                                                <tr>

                                                    <td style="width: 60%;">
                                                        <asp:Label ID="lbl_type" runat="server" Style="display: block; margin: 10px 0 0 0"></asp:Label></td>
                                                    <td style="width: 32%;">
                                                        <asp:TextBox ID="txt_search" runat="server"></asp:TextBox>
                                                        <ajasp:TextBoxWatermarkExtender ID="txt_search_TextBoxWatermarkExtender" runat="server"
                                                            Enabled="True" TargetControlID="txt_search" WatermarkCssClass="watermark" WatermarkText="SEARCH">
                                                        </ajasp:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="opt_search" runat="server" ImageUrl="~/VisualTool/icons/lenes.png" OnClick="opt_search_Click" /></td>
                                                    <td></td>


                                                </tr>

                                            </table>



                                        </div>
                                        <div class="option-icons">
                                            <asp:ImageButton runat="server" ID="opt_trash" Style="margin-top: 6px;" Height="27px" Width="27px" ImageUrl="~/VisualTool/images/del.png" OnClientClick="return checkDelete();" OnClick="opt_trash_Click" />
                                        </div>
                                        <div class="option-icons">
                                            <img id="opt-add" src="images/ad.png" style="height: 27px; width: 27px; margin: 0px; margin-top: 6px;" />
                                        </div>


                                    </div>
                                    <div id="thumb-container">
                                        <asp:UpdatePanel runat="server" ID="upPnl_dtMediaList" OnLoad="upPnl_dtMediaList_Load"
                                            RenderMode="Inline" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DataList ID="dt_mediaList" runat="server" RepeatColumns="4" Width="100%" RepeatDirection="Horizontal">
                                                    <ItemTemplate>
                                                        <div class="thumb-container-images">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <%--<td colspan="3">#<asp:Label ID="lbl_mediaId" runat="server" Font-Italic="True" Text='<%# Eval("MediaId") %>'></asp:Label>
                                                                    </td>--%>
                                                                    <td colspan="3">
                                                                        <asp:HiddenField ID="hf_mediaId" Value='<%# Eval("MediaId") %>' runat="server" />
                                                                        <img src="icons/edit.png" height="20px" width="20px" alt='<%# Eval("MediaId") %>' onclick="editItems(this);" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" style="text-align: center;">
                                                                        <asp:Image ID="Image1" CssClass="noclass" runat="server" ImageUrl='<%# Eval("Thumbnail") %>' Style="margin: 0px; height: 100px; width: 100px;"
                                                                            ToolTip='<%# "Name: "+Eval("Name")+"\nDescription: "+Eval("Description")+"\nKeywords: "+Eval("Keyword") %>' AlternateText='<%# Eval("path") %>' />
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5%">
                                                                        <input id="delete" type="checkbox" runat="server" class="styled" />
                                                                    </td>
                                                                    <td style="text-align: center">
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("name") %>' Font-Bold="True"
                                                                            ForeColor="#FF9900"></asp:Label>
                                                                    </td>
                                                                    <td width="10%">&nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div align="center">
                                            <asp:Label ID="lbl_message" runat="server" Visible="False" Font-Bold="True" Font-Italic="True"
                                                Font-Size="Large" ForeColor="#FF3300"></asp:Label>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>

                        

                        <div class="overlay">
                        </div>
                        <div id="loading">
                            <span>Please Wait</span> <span class="l-1"></span><span class="l-2"></span><span
                                class="l-3"></span><span class="l-4"></span><span class="l-5"></span><span class="l-6"></span>
                        </div>
                        <!-- TO SHOW THE POPUP BOX -->
                        <div id="image-upload" class="web_dialog" style="top: 10%; z-index: 1001; display: none; margin-left: -230px; width: 700px;">

                            <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img id="img_PopClose" src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" onclick="RollBackAction();" /></a>


                            <%--<div class="box" align="center">--%>


                            <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" ViewStateMode="Enabled">
                            <ContentTemplate>--%>
                            <table style="height: 440px; width: 100%;">
                                <tr>
                                    <td align="left" colspan="2">
                                        <h2 id="rein_head"></h2>
                                        <hr>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" runat="server" id="tdMsg" style="text-align: center;"></td>

                                </tr>
                                <tr>
                                    <td class="tdText">File Name: 
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txt_fName" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdText">Description: 
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txt_descriptions" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdText">Keywords: 
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txt_keywords" runat="server" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                        <span class="tdText" style="vertical-align: top;">(eg: Coin, Match, etc.)</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdText">
                                        <%-- <asp:FileUpload ID="file_photos" runat="server" Height="20px" Width="193px" />--%>File: </td>
                                    <td class="style1">
                                        <%--<span id="span_Record">
                                            <input id="btnRecordPop" type="button" value="RECORD" class="NFButton" />
                                            OR 

                                        </span>--%>
                                        <input type="file" id="file_upload" name="file" onchange="checkUplaod(this,extension)" />
                                        <span id="spanFile" style="text-align: center; margin-right: 30px;"></span>
                                        <br />
                                        <span id="spn_supprtedFormats" class="tdText"></span>

                                        <div id="flashContent" class="iframePopUp">
                                            <a id="rcrd_close" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: -4px; margin-top: -5px; z-index: 300" width="18" height="18" alt="" /></a>

                                            <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" width="300" height="165" id="VOCWordToYourMp33" align="middle">
                                                <param name="movie" value="VOCWordToYourMp3.swf" />
                                                <param name="allowScriptAccess" value="sameDomain" />
                                                <embed play="false" allowscriptaccess="sameDomain" name="VOCWordToYourMp3" id="VOCWordToYourMp3" src="VOCWordToYourMp3.swf" quality="high"
                                                    width="258" height="165" type="application/x-shockwave-flash"
                                                    name="myFlashMovie"
                                                    flashvars="URL=repository-manag.aspx&convertToMp3=true"></embed>
                                            </object>
                                            <%--     <input id="closeBtn" type="button" value="Close" />--%>
                                        </div>
                                    </td>
                                    <%--<asp:FileUpload ID="fileupload" runat="server" onchange="checkUplaod(this,extension)" />--%>
                                    <%--  <input type="file" id="fileupload" name="file" multiple="multiple" />--%>
                                </tr>
                                <tr>
                                    <td class="style2" colspan="2">
                                        <span id="span_thunbNail" style="display: block; text-align: center;">
                                            <br />
                                            <%--<input type="file" id="thumbupload" name="file" multiple="multiple" />--%>
                                            <%--<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />--%>
                                            <input id="btn_upload" type="button" value="Upload" class="NFButton" onclick='uploadVid()' />
                                            <%--<asp:FileUpload ID="thumbupload" runat="server" onchange="checkUplaod(this,ext_image)" />--%>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" colspan="2">
                                        <div id="thumbnails" style="text-align: center; height: 75px;">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"></td>
                                </tr>
                                <tr>
                                    <td class="style2" colspan="2" align="center">
                                        <table id="tbl_Submit" class="style4" style="display: none;">
                                            <tr>
                                                <td style="width: 50%;" align="center">
                                                    <input id="btn_save" type="button" value="Save" class="NFButton" />

                                                    <%--<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />--%>
                                                    <%--<asp:Button ID="btn_save" class="button" runat="server" Text="Save" OnClientClick="return checkValid();" OnClick="btn_save_Click" />--%>
                                                </td>
                                                <td align="center">
                                                    <%--<asp:Button ID="btn_cancle" class="button" runat="server" Text="Cancel" OnClientClick="RollBackAction();" />--%>
                                                    <input id="btn_cancle" type="button" value="Cancel" class="NFButton" onclick="RollBackAction();" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <%--</div>--%>
                            <%--   </ContentTemplate>
                          
                        </asp:UpdatePanel>--%>
                        </div>
                    </div>
                    <div id="previewBoard1">
                        <img id="previewImg" style="height: 500px; width: 800px;" />
                        <div class="videoPlayerDiv"></div>
                        <%--<video id="previewVideo" style="display:none;" width='100%'controls='controls'><source src='' type='video/mp4'></source>Video not supported</video>--%>
                        <div id="content-container" style="width: 100%; height: 255px; margin-top: 0px;">
                            <div id="content"></div>
                        </div>
                        <div id="audioPlayerDiv"></div>
                        <%--<input id="previewClose1" type="button" value="Close" />--%>

                        <a id="previewClose1" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 20px; margin-top: 12px; z-index: 300" width="18" height="18" alt="" /></a>
                    </div>

                    <%--<div id="flashContent" class="iframePopUp">
                        <a id="rcrd_close" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                            <img src="../Administration/images/clb.PNG" style="float: right; margin-right: -4px; margin-top: -5px; z-index: 300" width="18" height="18" alt="" /></a>

                        <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" width="300" height="165" id="VOCWordToYourMp33" align="middle">
                            <param name="movie" value="VOCWordToYourMp3.swf" />
                            <param name="allowScriptAccess" value="sameDomain" />
                            <embed play="false" allowscriptaccess="sameDomain" name="VOCWordToYourMp3" id="VOCWordToYourMp3" src="VOCWordToYourMp3.swf" quality="high"
                                width="258" height="165" type="application/x-shockwave-flash"
                                name="myFlashMovie"
                                flashvars="URL=repository-manag.aspx&convertToMp3=true"></embed>
                        </object>
                    </div>--%>


                    <div class="overlay">
                    </div>
                    <div id="previewSWF" class="popUp">
                        <div class="close-popUp">
                            <img alt="" src="icons/close.png" />
                        </div>
                        <div class="box">
                            <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" ViewStateMode="Enabled">
                            <ContentTemplate>--%>
                            <table style="width: 100%;">
                                <tr>
                                    <td></td>
                                </tr>

                            </table>
                            <%--   </ContentTemplate>
                          
                        </asp:UpdatePanel>--%>
                        </div>
                    </div>

                    <div class="overlay">
                        <div id="divEditItem" class="web_dialog" style="top: 30%; z-index: 1001; display: none; margin-left: -105px; width: 400px;">

                            <a id="editItemClose" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img id="img_editItemClose" src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>

                            <table style="width: 100%;">
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="tdText">Name : </td>
                                    <td>
                                        <asp:TextBox ID="txtEditName" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="tdText">Keyword : </td>
                                    <td>
                                        <asp:TextBox ID="txtEditKeyword" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <input id="btn_UpdateItem" type="button" value="Update" class="NFButton" /></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>

                            </table>
                        </div>
                    </div>


                    <!-- CHANGEABLE -->
                    <style type="text/css">
                        .loading {
                            display: none;
                            position: fixed;
                            overflow: auto;
                            top: 50%;
                            left: 50%;
                            font-size: 100%;
                            font: 13px/20px "Helvetica Neue", Helvetica, Arial, sans-serif;
                            color: black;
                            z-index: 102;
                            background: none;
                        }
                    </style>
                    <div id="load" class="loading">
                        <img src="images/ajax-loader.gif" />
                    </div>
                </div>

            </div>
        </div>


        <!-- footer -->
        <div id="footer-panel">            
            &copy; Copyright 2015, Melmark, Inc. All rights reserved.
        </div>


        

        <asp:HiddenField ID="repository_repCurrId" runat="server" />
        <asp:HiddenField ID="repository_thumb_Path" runat="server" />
        <asp:HiddenField ID="repository_type" runat="server" />
    </form>


</body>
    <script type="text/javascript">
        $(document).ready(function () {
            //setCookie('type', 'images', 1);
           // alert($('#opt-add').length);

            $('#opt-add').click(function () {

                type = $('#repository_type').val();//getCookie('type');


                //alert($('#repository_type').val());

               // alert('fileforma:'+fileformats+"|| extension:"+extension);

                var reinhead = document.getElementById('rein_head');
                if (reinhead != null) {
                    reinhead.innerHTML = "Add " + type;
                }
                clearData();
                document.getElementById('file_upload').value = '';
                document.getElementById('spanFile').innerHTML = '';
                document.getElementById('spn_supprtedFormats').innerHTML = '<i>Supported file formats</i> ' + fileformats + ' <i>only.</i>';

                $('.overlay').fadeIn('slow', function () {
                    $('#image-upload').fadeIn('slow');

                    if (type == "videos") {
                        document.getElementById('thumbnails').innerHTML = "";
                        $('#span_thunbNail').show();
                        $('#span_Record').hide();
                        document.getElementById('tbl_Submit').style.display = "none";
                    }
                    if (type == "images") {
                        $('#span_thunbNail').hide();
                        $('#span_Record').hide();
                        document.getElementById('tbl_Submit').style.display = "block";
                    }
                    if (type == "reinforcement") {
                        $('#span_thunbNail').hide();
                        $('#span_Record').hide();
                        document.getElementById('tbl_Submit').style.display = "block";
                    }
                    if (type == "audios") {
                        $('#span_thunbNail').hide();
                        $('#span_Record').show();
                        document.getElementById('tbl_Submit').style.display = "block";
                    }
                });
            });

            $('#btn_save').click(function () {
                //alert(isIE);
                if ((document.getElementById('file_upload').value != '') || (record == 1) || (isIE == 1)) {

                    var txtNameValue = document.getElementById("txt_fName").value;
                    if (txtNameValue == '') valid = false;
                    else if (txtNameValue == null) valid = false;
                    else valid = true;
                    //document.getElementById('tbl_Submit').innerHTML = "<img src='../loading' alt='Saving'></img>";
                    if (valid) {
                        //var temp = document.getElementById('tbl_Submit').innerHTML;
                        //document.getElementById('tbl_Submit').innerHTML = "";
                        //document.getElementById('tbl_Submit').style.textAlign = "center";
                        //alert(isIE);
                        $('#image-upload').fadeOut('slow', function () {
                            $('#flashContent').fadeOut();

                        });
                        document.getElementById('load').style.display = "block";
                        record = 0;
                        isIE = 0;
                        $.ajax({
                            type: "POST",
                            url: "repository-manag.aspx/SaveData",
                            data: '{name: "' + txtNameValue + '",description: "' + document.getElementById('txt_descriptions').value + '",keyword: "' + document.getElementById('txt_keywords').value + '",sType:"' + $('#repository_type').val() + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function OnSuccess(response) {

                                //document.getElementById('file_upload').value = "";
                                document.getElementById('file_upload').disabled = false;
                                document.getElementById('load').style.display = "none";
                                $('.overlay').fadeOut('slow');
                                //document.getElementById('tbl_Submit').innerHTML = temp;
                                document.getElementById('img_PopClose').style.display = "block";
                                document.getElementById('spanFile').innerHTML = '';
                                $('#opt_search').click();
                            }
        ,
                            failure: function (response) {
                                alert('Something went wrong!... Try Again');

                                document.getElementById('file_upload').disabled = false;
                                document.getElementById('load').style.display = "none";
                                $('.overlay').fadeOut('slow');
                                //document.getElementById('tbl_Submit').innerHTML = temp;
                                document.getElementById('img_PopClose').style.display = "block";
                                document.getElementById('spanFile').innerHTML = '';
                                //document.getElementById('tbl_Submit').innerHTML = temp;


                            }


                        });

                    }
                    else {
                        alert('Enter a filename');
                    }
                }
                else {
                    alert('Select a File');
                }
            });

            $('#close_x,#btn_cancle').click(function () {
                $('#image-upload').fadeOut('slow', function () {
                    $('#flashContent').fadeOut();
                    $('.overlay').fadeOut('slow');
                    document.getElementById('file_upload').value = "";
                    document.getElementById('spanFile').innerHTML = '';
                    record = 0;
                    isIE = 0;
                });
            });

            $('#opt-search').click(function () {
                $('.overlay').fadeIn('slow', function () {
                    $('#loading').fadeIn('slow');
                });
            });
            $('#opt-grid').click(function () {
                window.location = "homePage.aspx";
            });

            $('#txt_search').focus(function () {
                $('.option-icons-search').addClass('expand');
            });

            //$('#txt_search').focusout(function () {
            //    $('.option-icons-search').removeClass('expand');
            //});

            $('.thumb-container-images .noclass').click(function () {

                type = $('#repository_type').val();

                $('#previewVideo').hide();
                $('#previewImg').hide();
                $('#content-container').hide();
                var path = $(this).attr('alt');
                path = path.replace('~/VisualTool/', '');
                //  alert(path);

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
                if (type == "images") {
                    $('#previewImg').attr('src', path);
                    $('#previewBoard1').css("left", "20%");
                    $('#previewBoard1').css("top", "10%");
                    $('#previewBoard1').css("height", "auto");

                    var imgHeight = $('#previewImg').attr('height');
                    var imgWidth = $('#previewImg').clientWidth;
                    //alert(imgWidth);
                    imgHeight = (imgHeight > 500) ? 500 : imgHeight;
                    imgWidth = (imgWidth > 500) ? 500 : imgWidth;

                    var imgleft = (pageWidth / 2) - (imgWidth / 2);

                    //$('#previewBoard1').css({ left: imgleft, height: imgHeight, width: imgWidth });
                }
                else if (type == "reinforcement") {
                    PopUp(path);
                }

                $('.overlay').fadeIn('slow', function () {
                    $('#previewBoard1').fadeIn('fast', function () {
                        if (type == "images") {
                            $('#previewImg').show();
                        }
                        else if (type == 'videos') {
                            $('#previewBoard1').css("left", "30%");
                            $('#previewBoard1').css("top", "10%");
                            $('#previewBoard1').css("height", "auto");
                            path = path.replace('Repository/videos/', '');
                            var videoElement = '<div class="videoPlayerDiv"><object type="application/x-shockwave-flash" data="Repository/videos/player_flv_maxi.swf" width="520" height="420"><param name="movie" value="player_flv_maxi.swf" /><param name="FlashVars" value="flv=' + path + '" /></object></div>';
                            $('.videoPlayerDiv').remove();

                            $('#previewBoard1').append(videoElement);

                            //  $('#previewBoard1').append(previewClose1);


                            //$('#previewVideo').show();
                            //$('#previewVideo').attr('src', path);
                        }
                        else if (type == 'audios') {
                            $('#previewBoard1').css("left", "40%");
                            $('#previewBoard1').css("top", "45%");
                            $('#previewBoard1').css("height", "20px");
                            path = path.replace('~/VisualTool/', '');

                            var audioElement = '<div id="audioPlayerDiv"><object type="application/x-shockwave-flash" data="PseudoEngine/player_mp3_maxi.swf" width="200" height="20"> <param name="movie" value="PseudoEngine/player_mp3_maxi.swf" /><param name="FlashVars" value="mp3=' + path + '&autoplay=1" /></object></div>';
                            $('#audioPlayerDiv').remove();
                            $('#previewBoard1').append(audioElement);
                        }
                        else if (type == 'reinforcement') {
                            $('#content-container').show();
                        }

                    });
                });
            });

            $('#previewClose1').click(function () {
                $('#previewBoard1').fadeOut('slow', function () {
                    $('.overlay').fadeOut('slow');
                    $('#audioPlayerDiv').remove();
                    $('.videoPlayerDiv').remove();
                });
            });

            $('#btnRecordPop').click(function () {
                $('#flashContent').fadeIn();
                //$('.web_dialog').css('z-index', '999');

                record = 1;
            });

            $('#rcrd_close').click(function () {
                $('#flashContent').fadeOut();
                //$('.web_dialog').css('z-index', '1002');
            });

            $('#editItemClose,#img_editItemClose').click(function () {

                $('#divEditItem').fadeOut('slow', function () {
                    $('.overlay').fadeOut('slow');
                });
            });

            $('#btn_UpdateItem').click(function () {
                if (itemid > 0) {
                    var itemName = document.getElementById('txtEditName').value;
                    var itemkeywrd = document.getElementById('txtEditKeyword').value;
                    PageMethods.UpdateItemData(itemid, itemName, itemkeywrd, OnSuccessEditItemsUpdate);
                }
                else {
                    alert("Updation Failed. Try Again");
                }
            });
            function OnSuccessEditItemsUpdate(result) {
                if (result > 0) {
                    $('#opt_search').click();
                }
                else {
                    alert("Updation Failed");
                }
                $('#divEditItem').fadeOut('slow', function () {
                    $('.overlay').fadeOut('slow');
                });
            }
        });
    </script>


     <script type="text/javascript">

         var ext_image = "jpg,jpeg,png,gif";
         var ext_video = "wmv,flv,mp4";
         var ext_audio = "mp3";
         var ext_rein = "gif,swf";
         var folderName = '';
         var extension = '';
         var fileformats = '';
         var imgFrmts = ".jpg, .jpeg, .png, .gif";
         var videoFrmts = ".wmv ,.flv, .mp4";
         var audioFrmts = ".mp3";
         var reinFrmts = ".gif, .swf";
         var type = '';
         var ext = '';
         var ext2 = '';
         var valid = true;
         var record = 0;
         var isIE = 0;
         $(function () {
             type = $('#repository_type').val();//getCookie("type");
             //alert(type);
             if (type == "") {
                 $('#repository_type').val('images');
                 type = "images";
             }

             extension = '';
             switch (type) {
                 case "images": {
                     extension = ext_image;
                     fileformats = imgFrmts;
                     break;
                 }
                 case "videos": {
                     extension = ext_video;
                     fileformats = videoFrmts;
                     break;
                 }
                 case "audios": {
                     extension = ext_audio;
                     fileformats = audioFrmts;
                     break;
                 }
                 case "reinforcement": {
                     extension = ext_rein;
                     fileformats = reinFrmts;
                     break;
                 }
             }
             var reinhead = document.getElementById('rein_head');
             if (reinhead != null) {
                 reinhead.innerHTML = "Add " + type;
             }
             setCookie('ext', ext, 1);
             setCookie('ext2', ext, 1);

             <%--  $('#fileupload').fileupload({
                replaceFileInput: false,
                dataType: 'json',
                url: '<%= ResolveUrl("AjaxFileHandler.ashx?type=' + type + '&id=' + getCookie('repCurrId') + '")%>',
                add: function (e, data) {

                    $.each(data.files, function (index, file) {

                        ext = getExt(file.name.toLowerCase());

                        if (extension.indexOf(ext) != -1) {
                            valid = true;
                            data.submit();
                        }
                        else {
                            valid = false;
                            alert('It seems you have selected wrong type of file. Allowed extensions: ' + extension);
                        }
                    });

                },
                done: function (e, data) {
                }
            });

            $('#thumbupload').fileupload({
                replaceFileInput: false,
                dataType: 'json',
                url: '<%= ResolveUrl("AjaxFileHandler.ashx?type=thumbnails&id=' + getCookie('repCurrId') + '")%>',
                add: function (e, data) {

                    $.each(data.files, function (index, file) {

                        ext2 = getExt(file.name.toLowerCase());
                        var thumb_extension = ext_image;

                        if (thumb_extension.indexOf(ext2) != -1) {
                            valid = true;
                            data.submit();
                        }
                        else {
                            valid = false;
                            alert('It seems you have selected wrong type of file. Allowed extensions: ' + thumb_extension);
                        }
                    });

                },
                done: function (e, data) {
                }
            });
--%>

             showDummyAudio();
        });


         function getExt(fileName) {

             var ext = fileName.split('.').pop();
             return ext;
         }

         function showDummyAudio() {

             var audioThumbs = $('img.noclass');

             for (var i = 0; i < audioThumbs.length; i++) {

                 var thumbsSrc = $(audioThumbs[i]).attr('src');

                 if (thumbsSrc == "icons/audio%20(2).png")
                 {
                     $(audioThumbs[i]).attr('src', 'icons/audio2.png')
                 }

                 if (thumbsSrc == "icons/flashicon.png") {
                     $(audioThumbs[i]).attr('src', 'icons/flashicon2.png')
                 }
             }
         }

         $(window).bind('beforeunload', closeevent);

         function closeevent() {
             $.ajax({
                 type: "POST",
                 url: "repository-manag.aspx/closeEvent",
                 data: {},
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function () {
                     return true;
                 }
             });
         }

         var itemid = 0;
         function editItems(id) {
             $('.overlay').fadeIn('slow', function () {
                 $('#divEditItem').fadeIn('fast', function () {

                     PageMethods.GetItemsData(id.alt, OnSuccessEditItemsReturn);
                 });
             });
             itemid = id.alt;
         }
         function OnSuccessEditItemsReturn(result) {
             var item_name = result.split('*')[0];
             var item_keywrd = result.split('*')[1];
             document.getElementById('txtEditName').value = item_name;
             document.getElementById('txtEditKeyword').value = item_keywrd;
         }

         function checkDelete() {

             $('#repository_type').val(type);

             var checkboxes = $('.thumb-container-images input[type="checkbox"]');
             for (var i = 0; i < checkboxes.length; i++) {
                 if (checkboxes[i].checked == true) {
                     return confirm('Are you sure you want to delete this file?');
                 }
             }
             alert('Select any files');
             return false;
         }
    </script>


</html>

