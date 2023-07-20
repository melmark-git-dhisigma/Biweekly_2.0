function StopWatch(element, measurementId, c_studentId) {
    var _this = this;
    this.Element = $('<div class="tapCount" style="height:40px;width:125px;"><input type="text" onKeyPress="return isNumber(event);" value="0:0:0" class="Time-Box"/><input type="button" id="btnStartStopTimer" value="Start" class="btrn start-stop" /><div class="retId" style="display:none;"></div><span style="display:none;" class="measurementId">' + measurementId + '</span><span class="stpSeconds" style="display:none;">0</span><span style="display:none;" class="c_studentId">' + c_studentId + '</span></div>');
    $(element).append(this.Element);
    this.Time = new Date(0);
    var FuncID;
    function reset() {
        _this.Time.setTime(0);
        //stop();
        refreshDisplay();


    }
    function start() {
        $(_this.Element).find('.start-stop').val('Stop')
        FuncID = window.setInterval(function () {
            _this.Time.setSeconds(_this.Time.getSeconds() + 1);
            refreshDisplay();
        }, 1000);

        // saveStartTime(_this.Element);
    }
    function stop() {
       
        window.clearInterval(FuncID);
        $(_this.Element).find('.start-stop').val('Start');
        var behaviourId = $(_this.Element).find('.retId').html();
        $(_this.Element).find('.stpSeconds').html(_this.Time.getTime() / 1000);

       //alert($(_this.Element).html());
       
        refreshDisplay();
    }
    function refreshDisplay() {
        var timeString = _this.Time.getUTCHours() + ":" + _this.Time.getUTCMinutes() + ":" + _this.Time.getUTCSeconds();

        $(_this.Element).find('.Time-Box').val(timeString);
    }
    this.removeElement = function () {
        window.clearInterval(FuncID);
        _this.Element.remove();
    }
    this.Element.find('.start-stop').click(function () {
        if ($(this).val() == 'Start') {
            start();
        }
        else {
            stop();
            //reset();
        }
    })
    //$(this.Element).parent().parent().find('.reset').click(function () {
    $(this.Element).parents('.behavCont').find('.reset').click(function () {
        fieldReset(this);
    })

    function fieldReset(ths) {

        if ($(ths).parents('.behavCont').find('.txtTime').length > 0) {
            $(ths).parents('.behavCont').find('.txtTime').val('0');
        }
        if ($(ths).parents('.behavCont').find('.btnYesClick').length > 0) {
            $(ths).parents('.behavCont').find('.btnYesClick').removeClass().addClass("btnNothingClick");
        }
        if ($(ths).parents('.behavCont').find('.btnNoClick').length > 0) {
            $(ths).parents('.behavCont').find('.btnNoClick').removeClass().addClass("btnNothingClick");
        }

        $(ths).parents('.behavCont').find('.totSec').val('');
        $(ths).parents('.behavCont').find('.splitSec').val('');
        $(ths).parents('.behavCont').find('.lblFreq').text('0');
        $(ths).parents('.behavCont').find('.txtFreq').val('');
       
        $(ths).parents('.behavCont').find('.txDrp').val('');
        $(ths).parents('.behavCont').find('.stpSeconds').text('0');
      
        if ($(ths).hasClass('savDurReset')) {
            $(ths).parents('.behavCont').find('.totSec').val('');
            $(ths).parents('.behavCont').find('.splitSec').val('');
            $(ths).parents('.behavCont').find('.lblFreq').text('0');
            $(ths).parents('.behavCont').find('.txtFreq').val('');
            $(ths).parents('.behavCont').find('.txDrp').val('');
            $(ths).parents('.behavCont').find('.stpSeconds').text('0');

            resetOther(ths);
        }

        reset();
    }

    $(this.Element).parent().parent().parent().find('.save').click(function () {
        var clockStatus = $(this).parent().parent().parent().find('.start-stop').val();

        if ($(this).hasClass('savDurReset')) {
            clockStatus = $(this).parent().parent().parent().find('.start-stop').val();
        }

        if (clockStatus == "Start") {
            ////////////// Saving functions //////////////////////////

            var saveIdsting = $(this).attr('id');
            var splitSaveIdList = saveIdsting.split(',');

            switch (splitSaveIdList[0]) {
                case 'btnDuration':

                    saveDuration(splitSaveIdList[1], this);
                    fieldReset(this);


                    break;
                case 'saveDuraFreq':
                    if ($(this).hasClass('savDurReset')) {
                        saveDuraFreq(splitSaveIdList[1], $(this).parent());
                    }
                    else {
                        saveDuraFreq(splitSaveIdList[1], this);
                    }
                    fieldReset(this);
                    break;
                default:
                    reset();
                    break;
            }
        }
        else {
            var alertBox = "<div class='alertBox' style='height:20px; border:1px solid black; text-align:center; font-weight:bold; padding:3px; position: relative;color:#FF0000; background-color:pink; top:10px; '>Stopwatch Running</div>";
            $('body').append(alertBox);
            $('.alertBox').fadeOut(5000, function () {
                $(this).remove();
            });
        }


        //////////////end saving functions ///////////////////////////




    })
}
var List = new Array();
function addStopwatch(container, measurementId, c_studentId) {
    List.push(new StopWatch($('#' + container), measurementId, c_studentId));
}