<%@ Page Language="C#" AutoEventWireup="true" CodeFile="canvas_testing.aspx.cs" Inherits="canvas_testing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE HTML>
<%--<html>
  <head>
    <style>
      body {
        margin: 0px;
        padding: 0px;
      }
      canvas {
        border: 1px solid #9C9898;
      }
    </style>
    <script src="http://www.html5canvastutorials.com/libraries/kinetic-v4.0.0.js"></script>
    <script>
        function update(group, activeAnchor) {
            var topLeft = group.get(".topLeft")[0];
            var topRight = group.get(".topRight")[0];
            var bottomRight = group.get(".bottomRight")[0];
            var bottomLeft = group.get(".bottomLeft")[0];
            var image = group.get(".image")[0];

            // update anchor positions
            switch (activeAnchor.getName()) {
                case "topLeft":
                    topRight.attrs.y = activeAnchor.attrs.y;
                    bottomLeft.attrs.x = activeAnchor.attrs.x;
                    break;
                case "topRight":
                    topLeft.attrs.y = activeAnchor.attrs.y;
                    bottomRight.attrs.x = activeAnchor.attrs.x;
                    break;
                case "bottomRight":
                    bottomLeft.attrs.y = activeAnchor.attrs.y;
                    topRight.attrs.x = activeAnchor.attrs.x;
                    break;
                case "bottomLeft":
                    bottomRight.attrs.y = activeAnchor.attrs.y;
                    topLeft.attrs.x = activeAnchor.attrs.x;
                    break;
            }

            image.setPosition(topLeft.attrs.x, topLeft.attrs.y);

            var width = topRight.attrs.x - topLeft.attrs.x;
            var height = bottomLeft.attrs.y - topLeft.attrs.y;
            if (width && height) {
                image.setSize(width, height);
            }
        }
        function addAnchor(group, x, y, name) {
            var stage = group.getStage();
            var layer = group.getLayer();

            var anchor = new Kinetic.Circle({
                x: x,
                y: y,
                stroke: "#666",
                fill: "#ddd",
                strokeWidth: 2,
                radius: 8,
                name: name,
                draggable: true
            });

            anchor.on("dragmove", function () {
                update(group, this);
                layer.draw();
            });
            anchor.on("mousedown touchstart", function () {
                group.setDraggable(false);
                this.moveToTop();
            });
            anchor.on("dragend", function () {
                group.setDraggable(true);
                layer.draw();
            });
            // add hover styling
            anchor.on("mouseover", function () {
                var layer = this.getLayer();
                document.body.style.cursor = "pointer";
                this.setStrokeWidth(4);
                layer.draw();
            });
            anchor.on("mouseout", function () {
                var layer = this.getLayer();
                document.body.style.cursor = "default";
                this.setStrokeWidth(2);
                layer.draw();
            });

            group.add(anchor);
        }
        function loadImages(sources, callback) {
            var images = {};
            var loadedImages = 0;
            var numImages = 0;
            for (var src in sources) {
                numImages++;
            }
            for (var src in sources) {
                images[src] = new Image();
                images[src].onload = function () {
                    if (++loadedImages >= numImages) {
                        callback(images);
                    }
                };
                images[src].src = sources[src];
            }
        }
        function initStage(images) {
            var stage = new Kinetic.Stage({
                container: "container",
                width: 578,
                height: 400
            });
            var darthVaderGroup = new Kinetic.Group({
                x: 270,
                y: 100,
                draggable: true
            });
            var yodaGroup = new Kinetic.Group({
                x: 100,
                y: 110,
                draggable: true
            });
            var layer = new Kinetic.Layer();

            /*
            * go ahead and add the groups
            * to the layer and the layer to the
            * stage so that the groups have knowledge
            * of its layer and stage
            */
            layer.add(darthVaderGroup);
            layer.add(yodaGroup);
            stage.add(layer);

            // darth vader
            var darthVaderImg = new Kinetic.Image({
                x: 0,
                y: 0,
                image: images.darthVader,
                width: 200,
                height: 138,
                name: "image"
            });

            darthVaderGroup.add(darthVaderImg);
            addAnchor(darthVaderGroup, 0, 0, "topLeft");
            addAnchor(darthVaderGroup, 200, 0, "topRight");
            addAnchor(darthVaderGroup, 200, 138, "bottomRight");
            addAnchor(darthVaderGroup, 0, 138, "bottomLeft");

            darthVaderGroup.on("dragstart", function () {
                this.moveToTop();
            });
            // yoda
            var yodaImg = new Kinetic.Image({
                x: 0,
                y: 0,
                image: images.yoda,
                width: 93,
                height: 104,
                name: "image"
            });

            yodaGroup.add(yodaImg);
            addAnchor(yodaGroup, 0, 0, "topLeft");
            addAnchor(yodaGroup, 93, 0, "topRight");
            addAnchor(yodaGroup, 93, 104, "bottomRight");
            addAnchor(yodaGroup, 0, 104, "bottomLeft");

            yodaGroup.on("dragstart", function () {
                this.moveToTop();
            });

            stage.draw();
        }

        window.onload = function () {
            var sources = {
                darthVader: "http://www.html5canvastutorials.com/demos/assets/darth-vader.jpg",
                yoda: "http://www.html5canvastutorials.com/demos/assets/yoda.jpg"
            };
            loadImages(sources, initStage);
        };

    </script>
  </head>
  <body onmousedown="return false;">
    <div id="container"></div>
  </body>
</html>--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>Home</title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="styles/cloud.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="styles/style.css" type="text/css" media="screen" />
    <!--[if IE 6]><link rel="stylesheet" href="styles/style.ie6.css" type="text/css" media="screen" /><![endif]-->
    <!--[if IE 7]><link rel="stylesheet" href="styles/style.ie7.css" type="text/css" media="screen" /><![endif]-->
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
    <script type="text/javascript" src="scripts/script.js"></script>
    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            width: 138px;
        }
        .style2
        {
        }
        .style3
        {
            width: 77px;
        }
        .style4
        {
            width: 100%;
        }
         #myCanvas {
        border: 1px solid #9C9898;
      }
       #prevImage {
        border: 8px solid #ccc;
        width: 300px;
        height: 200px;
    }
    </style>
    
    <script>
        window.onload = function () {
            var canvas = document.getElementById("myCanvas");
            var context = canvas.getContext("2d");


            // Check for the various File API support.
            if (window.File && window.FileReader && window.FileList && window.Blob) {
                alert('Great success! All the File APIs are supported.');
            }
            else {
                alert('The File APIs are not fully supported in this browser.');
            }

        }
    </script>

    <script type="text/javascript">
        function setImage(file) {
            if (document.all)
                document.getElementById('prevImage').src = file.value;
            else
                document.getElementById('prevImage').src = file.files.item(0).getAsDataURL();
            if (document.getElementById('prevImage').src.length > 0)
                document.getElementById('prevImage').style.display = 'block';
        }
</script>
</head>
<body>
    <form id="form1" runat="server">

    
        <input type="file" name="myImage" onchange="setImage(this);" />

<img id="prevImage" style="display:none;" />

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<asp:UpdatePanel ID="up_container" runat="server">
   <ContentTemplate>--%>
    <div id="art-page-background-glare">
        <div id="art-page-background-glare-image">
            <div id="art-main">
                <div class="art-sheet">
                    
                    <div class="art-sheet-body">
                        <div class="art-nav">
                            <ul class="art-menu">
                                <li><a href="#" class="active"><span class="l"></span><span class="r"></span><span
                                    class="t">Home</span></a></li>
                                <li><a href="#"><span class="l"></span><span class="r"></span><span class="t">Whats
                                    new</span></a></li>
                                <li><a href="#"><span class="l"></span><span class="r"></span><span class="t">Contact
                                    Us</span></a></li>
                            </ul>
                        </div>
                        <div class="art-content-layout">
                            <div class="art-content-layout-row">
                                <div class="art-layout-cell art-content">
                                    <div class="art-post">
                                        <div class="art-post-tl">
                                        </div>
                                        <div class="art-post-tr">
                                        </div>
                                        <div class="art-post-bl">
                                        </div>
                                        <div class="art-post-br">
                                        </div>
                                        <div class="art-post-tc">
                                        </div>
                                        <div class="art-post-bc">
                                        </div>
                                        <div class="art-post-cl">
                                        </div>
                                        <div class="art-post-cr">
                                        </div>
                                        <div class="art-post-cc">
                                        </div>
                                        <div class="art-post-body">
                                            <div class="art-post-inner art-article">
                                                <div class="art-postcontent">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 200px; height: 300px; border-right-style: dashed; border-right-width: medium;
                                                                border-right-color: #666666;">
                                                                <div id="leftPane"><img id="drag1" src="firefox_.jpg" draggable="true"
ondragstart="drag(event)" />
<img id="Img1" src="icons/1342777509_ordinateur off.png" draggable="true"
ondragstart="drag(event)" />
<img id="Img2" src="icons/1344229480_package_settings.png" draggable="true"
ondragstart="drag(event)"/>
<img id="Img3" src="icons/1344315942_spreadsheet.png" draggable="true"
ondragstart="drag(event)"/></div></td>
                                                            <td>
                                                                <canvas id="myCanvas" height="300" width="659"  ondrop="drop(event)"
ondragover="allowDrop(event)></canvas></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="cleared">
                                                </div>
                                            </div>
                                            <div class="cleared">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="cleared">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="cleared">
                        </div>
                        <div class="art-footer">
                            <div class="art-footer-body">
                                <div class="art-footer-text">
                                    <p>
                                        Copyright © 2012, Melmark. All Rights Reserved.</p>
                                </div>
                                <div class="cleared">
                                </div>
                            </div>
                        </div>
                        <div class="cleared">
                        </div>
                    </div>
                </div>
                <div class="cleared">
                </div>
                <p class="art-page-footer">
                    Powered by <a href="#">www.m2ctech.com</a> and Template created by Pramod Kumar</p>
            </div>
        </div>
    </div>
    <%--</ContentTemplate>
   </asp:UpdatePanel>--%>
    <div class="overlay">
    </div>
    <div id="loading">
        <span>Please Wait</span> <span class="l-1"></span><span class="l-2"></span><span
            class="l-3"></span><span class="l-4"></span><span class="l-5"></span><span class="l-6">
            </span>
    </div>
    <!-- TO SHOW THE LOADING ANIMATION -->
    <div id="image-upload" class="popUp">
        <div class="close-popUp">
            <img alt="" src="icons/close.png" /></div>
        <div class="box">
            <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" ViewStateMode="Enabled">
                            <ContentTemplate>--%>
            <table style="width: 100%; height: 154px;">
                <tr>
                    <td class="style3">
                        File Name
                    xt_fName" runat="server" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Description
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txt_descriptions" runat="server" Height="45px" TextMode="MultiLine"
                            Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Keywords
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txt_keywords" runat="server" Height="45px" TextMode="MultiLine"
                            Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2" colspan="2">
                        <%-- <asp:FileUpload ID="file_photos" runat="server" Height="20px" Width="193px" />--%>
                        <input type="file" id="fileupload" name="file" multiple="multiple" />&nbsp;
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="style2" colspan="2">
                        <table class="style4">
                            <tr>
                                <td style="width: 50%;">
                                    <input id="btn_save" type="button" value="Save" class="button" />
                                </td>
                                <td>
                                    <input id="btn_cancle" type="button" value="Cancel" class="button" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <%--   </ContentTemplate>
                          
                        </asp:UpdatePanel>--%>
        </div>
    </div>


     
    
    </form>
</body>

</html>
