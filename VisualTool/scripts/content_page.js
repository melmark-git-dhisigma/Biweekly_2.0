

    var selected;
    $(document).ready(function () {



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

        $('#workSpace').animate({ height: '590px' }, 1000, 'easeOutBounce', function () {
            $('#rightPanel1,#rightPanel2').animate({ height: '290px' }, 1000, 'easeOutBounce');
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

            $('#workSpace').append("<table class='workSpaceTable'><tr height=" + rowHeight + "px><td class='selectable dropable' width=" + col1Width + "%></td><td class='selectable dropable' width=" + col2Width + "%></td></tr></table>");
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

            $('.popup_msg').animate({ top: newTop, left: newLeft }, 1000, 'easeOutElastic');
        });

        $('.close_msg').click(function () {
            $('.popup_msg').animate({ top: buffPointerY, left: buffPointerX }, 500, 'linear', function () { $(this).fadeOut('fast'); });
            document.getElementById("txtRowHeight").value = "20";
            document.getElementById("txtCol1Width").value = "50";
            document.getElementById("txtCol2Width").value = "50";
        });


        $('#smallMenuDelete').click(function () {
            alert('deleted');
        });
        $('#smallMenuEdit').click(function () {
            alert('deleted');
        });

        var previewWidth = $('#previewBoard').width();
        var txtEditorWidth = $('#textEditorDiv').width();

        var previewLeft = (pageWidth / 2) - (previewWidth / 2) + 'px';
        var textEditorLeft = (pageWidth / 2) - (txtEditorWidth / 2) + 'px';

        $('#previewBoard').css({ left: previewLeft });
        $('#textEditorDiv').css({ left: previewLeft });


        $('#btn_preview').click(function () {
            $('.fullOverlay').fadeIn('slow', function () {
                showHtml();
                $('#previewBoard').show();
                $('#textEditorDiv').hide();
                $('#previewBoard').animate({ top: '20px' }, 1000, 'easeOutElastic');
            });
        });

        $('#text').dblclick(function () {
            if (selected && selected.toString() == '[object HTMLTableCellElement]') {
                $('.fullOverlay').fadeIn('slow', function () {
                    $('#previewBoard').hide();
                    $('#textEditorDiv').show();
                    $('#textEditorDiv').animate({ top: '20px' }, 1000, 'easeOutElastic');
                });
            }
            else {
                alert('select a cell');
            }
        });

        $('#previewBoard').click(function () {
            $('#previewBoard').animate({ top: '-610px' }, 500, 'linear', function () {
                $('.fullOverlay').fadeOut('slow');
            });

        });
        $('#closeTextEditor').click(function () {
            $('#textEditorDiv').animate({ top: '-610px' }, 500, 'linear', function () {
                $('.fullOverlay').fadeOut('slow');
            });

        });
        bindSelectableHandler();

        $('#btn_imgPropDone').click(function () {
            $(selected).css({ 'height': $('#txtImgPropHeight').val(), 'width': $('#txtImgPropWidth').val() });

            $('#topRibbon').slideUp('slow', 'linear');
        });

        $('.deleteElement').click(function () {
            if (confirm('Delete it?')) {
                $(selected).remove();

                $('#topRibbon').slideUp('slow', 'linear');
            }
        });

        $('#close_ribbon').click(function () {
            $('#topRibbon').slideUp('slow', 'linear');
        });

        $('#video').dblclick(function () {
            if (selected) {
                if (selected.toString() == '[object HTMLTableCellElement]') {
                    var videoElement = "<video class='selectable' width='100%'controls='controls'><source src='' type='video/mp4'></source>Video not supported</video>";
                    $(selected).append(videoElement);
                }
            }
            else {
                alert('select any area');
            }
        });

        $('#btn_tdPropDone').click(function () {
            $(selected).css('height', $('#txtTdHeight').val() + 'px');
            $(selected).css('width', $('#txtTdWidth').val() + 'px !important');

            $('#topRibbon').slideUp('slow', 'linear');
        });

    });

    

    //EXTERNAL FUNCTIONS
    function bindSelectableHandler() {
        $('#workSpace').find('.selectable').click(function (event) {
            event.stopPropagation();
            selected = event.target;
           
            $('#workSpace').find('.-r').removeClass('-r');
            $('#workSpace').find('.selected').removeClass('selected');
            $(event.target).addClass('selected');


            $('#toolBox').css('left', event.pageX);
            $('#toolBox').css('top', event.pageY);


        });
      

        $('#workSpace').find('.selectable').dblclick(function (event) {
            event.stopPropagation();
            selected = event.target;
            $('#workSpace .selectable').removeClass('selected');
            $(event.target).addClass('selected');



            //alert(event.target);
            if (event.target.toString() == '[object HTMLImageElement]') {
                $('#topRibbon div').not('#close_ribbon').hide();
                $('#imgProp').show();
            }
            if (event.target.toString() == '[object HTMLTableCellElement]') {

                $('#topRibbon div').not('#close_ribbon').hide();
                $('#tdProp').show();
                $('#topRibbon').slideDown('slow', 'linear');

            }



            if (event.target.toString() == '[object HTMLDivElement]') {

               // alert($(selected).attr('class'));

                //CLEAR THE TEXT EDITOR AND FILLING IT WITH THE TD CONTENTS
                var insideContent = $(selected).find('.txtContainer').html();
                tinymce.get('elm1').setContent('');
                tinyMCE.execCommand('mceInsertContent', false, insideContent);

                //SHOW THE TEXT EDITOR
                $('.fullOverlay').fadeIn('slow', function () {
                    $('#previewBoard').hide();
                    $('#textEditorDiv').show();
                    $('#textEditorDiv').animate({ top: '20px' }, 1000, 'easeOutElastic');
                });
            }
        });

        $(".draggable").draggable({ revert: "invalid",
            helper: "clone",
            cursor: "move"
        });

        $(".dropable").droppable({
            accept: ".draggable",
            drop: function (event, ui) {
                $(this).append(ui.draggable.clone());
            }
        });
    }

    function showHtml() {
        var details = document.getElementById('workSpace').innerHTML;
        var showLbl = document.getElementById('previewBoard');
        showLbl.innerHTML = details;
        //$('#previewBoard').prepend(details);
    }

    function insertText(contents) {
        $(selected).html('<div class="txtContainer selectable">' + contents + '</div>');
        $('#textEditorDiv').animate({ top: '-610px' }, 500, 'linear', function () {
            $('.fullOverlay').fadeOut('slow');
        });
    }

    function showToolBox(e) {
        $('#toolBox').css('left', e.pageX);

    }
   
