function StopWatch(element,measurementId,c_studentId) {
    var _this = this;
    this.Element = $('<div class="stopwatch" style="height:38px;"><input type="text" style="text-align:center;width:75px; border:none; background-color:#06678A;color:white;height:100%;" value="0:0:0" class="Time-Box"/><input type="button" value="Start" class="btn btn-green-dark start-stop" /><div class="retId" style="display:none;"></div><span style="display:none;" class="measurementId">' + measurementId + '</span><span style="display:none;" class="c_studentId">' + c_studentId + '</span></div>');
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

        saveStartTime(_this.Element);
    }
    function stop() {
        window.clearInterval(FuncID);
        $(_this.Element).find('.start-stop').val('Start');
        var behaviourId = $(_this.Element).find('.retId').html();
       // alert(behaviourId);
        //alert(_this.Time.getTime() / 1000);
        saveTimeInDB( _this.Time.getTime() / 1000, $(_this.Element).find('.start-stop'),behaviourId);
        refreshDisplay();
    }
    function refreshDisplay() {
      var  timeString = _this.Time.getUTCHours() + ":" + _this.Time.getUTCMinutes() + ":" + _this.Time.getUTCSeconds();
      
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
            reset();
        }
    })
    this.Element.find('.reset').click(function () {
        reset();
    })
}
var List = new Array();
function addStopwatch(container,measurementId,c_studentId) {
    List.push(new StopWatch($('#'+container),measurementId,c_studentId));
}