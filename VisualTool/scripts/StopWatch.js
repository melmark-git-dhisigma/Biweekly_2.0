function StopWatch(element) {
    var _this = this;
    this.Element = $('<div class="stopwatch" style="height:35px;"><input type="text" style="text-align:center;width:125px; border:none; background-color:#E8C9E1;color:black;height:100%;" class="Time-Box" readonly ="true"/><input type="button" value="Start" class="btn btn-green-dark start-stop" /></div>');
    $(element).append(this.Element);
    this.Time = new Date(0);
    var FuncID;
    start();

    function reset() {
        _this.Time.setTime(0);
        //stop();
        refreshDisplay();
       
    }
    function start() {
      //  $(_this.Element).find('.start-stop').val('Stop')
        FuncID = window.setInterval(function () {
            _this.Time.setSeconds(_this.Time.getSeconds() + 1);
            refreshDisplay();
        }, 1000);
    }
    function stop() {
        window.clearInterval(FuncID);
        $(_this.Element).find('.start-stop').val('Start');
        //alert(_this.Time.getTime() / 1000);
        saveTimeInDB( _this.Time.getTime() / 1000);
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
function addStopwatch(container) {
    List.push(new StopWatch($('#'+container)));
}