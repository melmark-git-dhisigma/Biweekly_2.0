<%@ Page Language="C#" AutoEventWireup="true" CodeFile="matchingLessonPreview.aspx.cs" Inherits="matchingLessonPreview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="styles/matchingLesson.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .ml-base {
            height: 170px;
            width: 300px;
        }

        .ml-label {
            height: 30px;
        }

        .ml_answerTemplate, .questionTemp {
            float: left;
            width: 415px;
            height: 200px;
        }

        p {
            margin: 0px;
        }
    </style>

    <script src="scripts/jquery-1.8.0.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-weight: 700; width: 100%;">

            <table class="auto-style1">
                <tr>
                    <td style="text-align: center">
                        <div style="height: 200px; width: 415px; margin: 0 auto; padding: 2px; border: 1px dashed red;">
                            <div class="questionTemp">
                                <div class="player" style="float: left; margin-top: -21px;">
                                </div>
                                <div class="ml_base">
                                </div>
                                <div class="ml_baseLable">
                                </div>

                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <div class="distContainer">
                            <asp:HiddenField ID="corrReinTemp" runat="server" Value=""/>
                            <asp:HiddenField ID="wrongReinTemp" runat="server" Value=""/>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="reinforcement" style="position: absolute; top: 0px; left: 0px; width: 100%; height: 500px; z-index: 1000; display: none;">
                <iframe style="width: 100%; height: 500px;" id="reinfIframe"></iframe>
            </div>
        </div>
    </form>
</body>
<script type="text/javascript">
   
    var optList = "";
   
    $(document).ready(function () {
        // fn_randomizer(inputArray);

        fn_getLeOptIds();

        
    });

    //EXTERNAL FUNCTIONS

    function fn_randomizer(inputArray) {

        for (i = 0; i < inputArray.length; i++) {
            if (Math.floor((Math.random() * 2) + 1) == 1) {
                var j = Math.floor((Math.random() * (inputArray.length - 1)) + 1);

                var temp = inputArray[i];
                inputArray[i] = inputArray[j];
                inputArray[j] = temp;

            }
        }

        //alert(inputArray);
        return inputArray;
    }


    function fn_getLeOptIds() {
        $.ajax({
            url: "matchingLessonPreview.aspx/getLeOptId",
            data: "{}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var optList = data.d;
                var optL = optList.split(',');
                var optL2 = new Array();
                var optL22 = new Array();
                var j = 0;

                for (var i = 0; i < optL.length - 1; i++) {

                    var optLstat = optL[i].split('^');

                    optL2[i] = optLstat[0];

                    if (optLstat[1] != "Q") {
                        optL22[j] = optLstat[0];
                        j++;
                        //optL2[i] = optL[i];
                    }
                }

                var optRandList = new Array(optL22.length);

                for (var i = 0; i < optL22.length; i++) {
                    optRandList[i] = i;
                }

                optRandList = fn_randomizer(optRandList);

                for (i = 0; i < optL22.length; i++) {
                    if (optL[optRandList[i]] != "") {
                        var ansTemp = '<div id="ansTemp' + optL22[optRandList[i]] + '" class="ml_answerTemplate"><div class="ml_base"></div><div class="ml_baseLable"></div></div>';

                        $('.distContainer').append(ansTemp);
                    }
                }


                $('.ml_answerTemplate').click(function () {





                    if ($(this).hasClass('C')) {
                        var realPath = 'Repository/reinforcement/2012.swf';
                        $('#reinfIframe').attr('src', 'Repository/reinforcement/fireworks.swf');
                        $('.reinforcement').show();
                    }
                    else {
                        alert('Wrong Answer');
                    }
                });


                fn_getTempData(optL2);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });


        // alert(optList);
    }


    function fn_getTempData(leOptId) {
        $.ajax({
            url: "matchingLessonPreview.aspx/getTempData",
            data: "{'leOptId':'" + leOptId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                var recContent = data.d;

                var recContentArray = recContent.split('☺');

                for (k = 0; k < recContentArray.length - 1; k++) {

                    var recContSubArray = recContentArray[k].split('^');



                    if (recContent.indexOf("System.IndexOutOfRangeException") == -1) {
                        if (recContSubArray[1] != 'Q') {

                            $('#ansTemp' + recContSubArray[2]).addClass(recContSubArray[1]);


                            var listNew = $(recContSubArray[0]).find('.saveElem');
                            var lblContent = $(recContSubArray[0]).find('.label').html();
                            var details = "<div>";

                            var W = '#ansTemp' + recContSubArray[2];


                            // if ($(W).length > 0) {
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
                            // }
                        }

                        else {


                            $('#ansTemp' + recContSubArray[2]).remove();

                            var listNew = $(recContSubArray[0]).find('.saveElem');
                            var lblContent = $(recContSubArray[0]).find('.label').html();
                            var musicFile = $(recContSubArray[0]).find('.music').html();
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
                            $('.player').empty();
                            if (musicFile != '') {
                                $('.player').append(musicFile);
                            }
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);

            }
        });
    }

</script>

</html>
