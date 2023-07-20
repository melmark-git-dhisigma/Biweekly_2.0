(function($) {
    $.fn.watch = function(props, callback, timeout){
        if(!timeout)
            timeout = 10;
        return this.each(function(){
            var el = $(this),
            func = function(){
                __check.call(this, el)
            },
            data = {
                props: 	props.split(","),
                func: 	callback,
                vals: 	[]
            };
            $.each(data.props, function(i, prop) {
                //data.vals[i] = el.css(data.props[i]);
                data.vals[i] = el.attr(prop);
            });
            el.data(data);
            if (typeof (this.onpropertychange) == "object"){
                el.bind("propertychange", callback);
            } else if ($.browser.mozilla){
                el.bind("DOMAttrModified", callback);
            } else {
                setInterval(func, timeout);
            }
        });
        function __check(el) {
            var data 	= el.data(),
            changed = false,
            temp	= "";
            for(var i=0;i < data.props.length; i++) {
                //temp = el.css(data.props[i]);
                temp = el.attr(data.props[i]);
                if(data.vals[i] != temp){
                    data.vals[i] = temp;
                    changed = true;
                    break;
                }
            }
            if(changed && data.func) {
                data.func.call(el, data);
            }
        }
    };



  $.fn.imageradio = function(options) {
    var defaults = {
          hidden: true
        },
        opts;
    if (options != undefined) {
      opts = $.extend($.fn.imageradio.defaults, options);
    }else{
      opts =  defaults;
    }
    var $this = $(this),
        $radios = $this.find('input[type=radio]'),
        $imgRadios = [],
        $myRadios = [];
    return $radios.each(function() {
      var $this = $(this),
          thisChecked = $this.attr('checked'),
          thisClass = $this.attr('class'),
          json = eval('('+ thisClass +')'),
          src = json.src,
          checked = json.checked,
          disabled = json.disabled,
          hov = json.hover,
          tooltipText = $this.attr('title'),
          $imgRadio = $('<img src="'+ src +'" alt="'+ tooltipText +'" style="cursor: pointer" title="'+ tooltipText +'" />');
      $this.watch('disabled', function(){
          var disabled = $(this).attr('disabled'),
              src = json.src,
              d = json.disabled;
          $imgRadio.attr('src', disabled ? d || src : src);
      }, 300);
      $this.data({
        src: src,
        checked: checked,
        hov: hov
      });
      var change = function($radio, $img) {
        if ($radio.attr('disabled')) {
            return;
        }
        for (var i = -1, leni = $imgRadios.length; ++i < leni;) {
            var $imgRadio = $myRadios[i];
            if (!$imgRadio.attr('disabled')) {
                $imgRadios[i].attr('src', $myRadios[i].data('src'));
            }
        }
        $radio.attr('checked',true);
        $img.attr('src', checked);
      };
      var imgChange = function() {
        var $img = $(this),
            $radio = $img.prev('input[type=radio]');
        change($radio, $img);
        $radio.trigger('change');
      };
      var radioChange = function() {
        var $radio = $(this),
            $img = $radio.next('img');
        change($radio, $img);
      };
      $imgRadio.click(imgChange);
      $this.click(radioChange);
      $imgRadio.data({
        radio: $this
      });
      $this.data('imgRadio', $imgRadio);
      $imgRadio.hover(function() {
        var $myRadio = $(this).data('radio');
        if ($myRadio.attr('disabled')) {
            return;
        }
        if (!$myRadio.attr('checked')) {
          $(this).attr('src',hov);
        }
      },function() {
        var $myRadio = $(this).data('radio');
        if ($myRadio.attr('disabled')) {
            return;
        }
        if (!$myRadio.attr('checked')) {
          $(this).attr('src',src);
        }
      });
      $this.after($imgRadio);
      if (opts.hidden) {
        $this.hide();
      }
      if (thisChecked) {
        $imgRadio.attr('src', checked);
      }
      $imgRadios.push($imgRadio);
      $myRadios.push($this);
    });
  };
})(jQuery);
