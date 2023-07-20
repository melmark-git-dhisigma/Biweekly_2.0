function StopWatch(element, id, colId, isVisual) {
    var _this = this;
    if (isVisual != '0')
        this.Element = $('<div class="stopwatch" style="text-align:center;;margin-left:4px;"><input id="' + id + '" type="text" value="------" disabled="true" style="width:70px;"  class="Time-Box"/>' +
        '<input type="text" style="width:50px;display:none;" value="0" class="Time-Box2 col' + colId.replace(/ /g, '') + '"/>' +
        '<input type="button"  value="Start" disabled="true" class="btn2 btn-green-dark start-stop" style="height:25px;margin-left:5px;padding: 0 7px 2px;cursor:default;" /></div>');
    else
        this.Element = $('<div class="stopwatch" style="text-align:center;;margin-left:4px;"><input id="' + id + '" type="text" value="------" style="width:70px;"  class="Time-Box"/>' +
            '<input type="text" style="width:50px;display:none;" value="0" class="Time-Box2 col' + colId.replace(/ /g, '') + '"/>' +
            '<input type="button"  value="Start" class="btn2 btn-green-dark start-stop" style="height:25px;margin-left:5px;padding: 0 7px 2px;" /></div>');
    $(element).append(this.Element);
    this.Time = new Date(0);
    var FuncID;
    function reset() {
        _this.Time.setTime(0);
        stop();
        refreshDisplay();

    }
    function start() {
        $(_this.Element).find('.start-stop').val('Stop')
        FuncID = window.setInterval(function () {
            _this.Time.setSeconds(_this.Time.getSeconds() + 1);
            refreshDisplay();
        }, 1000);
    }
    function stop() {
        window.clearInterval(FuncID);
        $(_this.Element).find('.start-stop').val('Start');
        //alert(_this.Time.getTime() / 1000);
        //saveTimeInDB( _this.Time.getTime() / 1000, $(_this.Element).find('.start-stop'));
        refreshDisplay();
    }
    function refreshDisplay() {

        var timeStringSec = _this.Time.getTime() / 1000;
        var timeString = _this.Time.getUTCHours() + ":" + _this.Time.getUTCMinutes() + ":" + _this.Time.getUTCSeconds();

        $(_this.Element).find('.Time-Box').val(timeString);
        $(_this.Element).find('.Time-Box2').val(timeStringSec);

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
    this.Element.find('.reset').click(function () {
        reset();
    })
}
var List = new Array();
function addStopwatch(container, id, colId, isVisual) {
    List.push(new StopWatch($(container), id, colId, isVisual));
}