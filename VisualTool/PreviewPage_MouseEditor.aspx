<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreviewPage_MouseEditor.aspx.cs" Inherits="PreviewPage_MouseEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />

    <link href="styles/mouseStyle.css" rel="stylesheet" />
    <link href="scripts/ui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.mouse.js"></script>
    <script type="text/javascript" src="scripts/colorpicker.js"></script>
    <script src="scripts/cookies.js"></script>
    <script type="text/javascript" src="scripts/ui/jquery.ui.resizable.js"></script>
    <script src="scripts/StopWatch.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; height: auto;">
            <div class="previewArea">
                <div id="imgValue">
                    <%--                    <asp:Image ID="imgTOlo" runat="server" ImageUrl="~/StudentsPhoto/Navya_Nair_newqdk5iskg0b5ljslpmcfgechn.jpg" Height="80px" Width="80px" class="a" />--%>
                </div>
            </div>
            <div id="stopWatchArea">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <div id="InnerWatchContainer" style="width: 100%; height: 50px; text-align: left; margin-left: 50px;"></div>
                        </td>

                    </tr>
                </table>
                <%--  --%>
            </div>
            <div class="reinforcement" style="position: absolute; top: 0px; left: 0px; width: 100%; height: 500px; z-index: 1000; display: none;">
                <iframe style="width: 100%; height: 465px;" id="reinfIframe"></iframe>
                <input id="btn_previewOk" type="button" value="Ok" />
            </div>
            <div id="previewClose"></div>
        </div>
    </form>
</body>


<script type="text/javascript">

    var clickTime = 0;
    var nmbrofImage = 0;
    function clickMe(id) {
        var imgId = $('#' + id)
        imgId.hide();
        var count = --nmbrofImage;
        if (count == 0) {
            // alert('Lesson Plan Completed');
            var realPath = 'Repository/reinforcement/2012.swf';
            $('#reinfIframe').attr('src', 'Repository/reinforcement/fireworks.swf');
            $('.reinforcement').show();
        }

    }

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

    function saveTimeInDB(duration) {
        alert(duration);
    }


    $(document).ready(function () {

        //addStopwatch('InnerWatchContainer')

        var setNumber = GetQueryStringParams('setNumber');
        var stepNumber = GetQueryStringParams('stepNumber');
        var speedValue = 0;
        var speedMeasure = 0;

        $.ajax({
            url: "PreviewPage_MouseEditor.aspx/GetElementData",
            data: "{'setNumber':'" + setNumber + "','stepNumber':'" + stepNumber + "' }",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                save = true;
                $('.innerMsgRibbon').text('Saved');
                $('.messageRibbon').show().fadeOut(2000);
                var contents = data.d;
                //  alert(contents);
                var content_speed = contents.split('^');
                var content_Data = contents.split(',');
                speedValue = content_speed[content_speed.length - 1];
                nmbrofImage = content_Data.length - 1;
                if (speedValue == 0) {
                    speedMeasure = 0.1;
                }
                else if (speedValue == 1) {
                    speedMeasure = 0.1;
                }
                else if (speedValue == 2) {
                    speedMeasure = 0.3;
                }
                else if (speedValue == 3) {
                    speedMeasure = 0.5;
                }
                else if (speedValue == 4) {
                    speedMeasure = 0.6;
                }

                //alert(speedMeasure);

                for (var i = 0; i < content_Data.length - 1; i++) {
                    //alert(content_Data[i]);
                    $('.previewArea').append('<li><img class = "a" id = "imgAnimate' + i + '" Height = "100px" Width = "100px" src="' + content_Data[i] + '" onclick="clickMe(this.id)" /></li>');

                    animateDiv(speedMeasure);

                }


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });

        $('#imgAnimate').click(function () {

            //  alert('asdadasd');

        });

        $('#btn_previewOk').click(function () {
            $('.reinforcement').fadeOut('slow', function () {
                $('.reinforcement').fadeOut('slow');
            });
        });

        function makeNewPosition() {
            // Get viewport dimensions (remove the dimension of the div)
            var h = $('.previewArea').height() - 300;
            var w = $('.previewArea').width() - 300;

            var nh = Math.floor(Math.random() * h);
            var nw = Math.floor(Math.random() * w);

            return [nh, nw];

        }

        function animateDiv(speedMeasure) {
            // alert(speedMeasure);
            var newq = makeNewPosition();
            var oldq = $('.a').offset();
            var speed = calcSpeed([oldq.top, oldq.left], newq, speedMeasure);

            $('.a').animate({ top: newq[0], left: newq[1] }, speed, function () {
                animateDiv(speedMeasure);
            });

        }

        function calcSpeed(prev, next, Getspeed) {

            var x = Math.abs(prev[1] - next[1]);
            var y = Math.abs(prev[0] - next[0]);

            var greatest = x > y ? x : y;

            var speedModifier = Getspeed;

            var speed = Math.ceil(greatest / speedModifier);

            return speed;

        }


    });



</script>
</html>
