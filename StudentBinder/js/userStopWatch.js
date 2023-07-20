function StopWatch(element) {
    var _this = this;
    this.Element = $('<div class="container"><input type="button" class="addBtn addBtnG" value="🕐" style="height: 31px; width:25px;"><input type="text" onclick="$(this).select();" value="#' + $(element).attr('id') + '" class="lblText"/><input type="text" onKeyPress="return false;" value="0:0:0" class="Time-Box"/><input type="button" value="►" class="start-stop" /><input type="button" value="↺" class="reset"/><input type="button" value="X" class="timDel" onclick="delTimer(this)"/></div>');
    $(element).append(this.Element);
    this.Time = new Date(0);
    var FuncID;
    function reset() {
        _this.Time.setTime(0);
        refreshDisplay();


    }
    function start() {
        $(_this.Element).find('.start-stop').val('■')
        FuncID = window.setInterval(function () {
            _this.Time.setSeconds(_this.Time.getSeconds() + 1);
            refreshDisplay();
        }, 1000);

        // saveStartTime(_this.Element);
    }
    function stop() {
        window.clearInterval(FuncID);
        $(_this.Element).find('.start-stop').val('►');
       
        
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
        if ($(this).val() == '►') {
            start();
        }
        else {
            stop();
            //reset();
        }
    })
    $(this.Element).parent().parent().find('.reset').click(function () {
        // alert($(this).parent().find('.start-stop').val());
        _this.Time.setSeconds(0);
        if ($(this).parent().find('.start-stop').val() == '►') {
            $(this).parent().find('.Time-Box').val("0:0:0");
            window.clearInterval(FuncID);
            _this.Time.setSeconds(0);
        }
        else {
            //$(this).parent().find('.start-stop').css('border', '1px solid red');
            var $el = $(this).parent().find('.start-stop'), x = 2000,
            originalColor = "#ffffff";//$el.css("background");

            $el.css("background", "#FF8989");
            setTimeout(function () {
                $el.css("background", originalColor);
            }, x);
        }
    })

   
}
var List = new Array();
function addStopwatch(container) {
    List.push(new StopWatch($('#' + container)));
}