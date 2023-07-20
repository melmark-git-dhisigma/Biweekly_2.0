/* HOME */

jQuery(function () {

    var isiPad = navigator.userAgent.match(/iPad/i);

    if (isiPad == null) {
        jQuery("div#makeMeScrollable").smoothDivScroll({ mouseDownSpeedBooster: 1, scrollInterval: 30 });
    }
    else {

        var isScrolling = false;
        var isScrolling2 = false;



        var animationHandler;
        function slideLeft() {
            var stdContLeft = parseFloat($("#studentContainer").css("margin-left").replace('px', '')) - 10;
            $("#studentContainer").css("margin-left", stdContLeft + "px")
        };
        function slideRight() {
            var stdContLeft = parseFloat($("#studentContainer").css("margin-left").replace('px', '')) + 10;
            $("#studentContainer").css("margin-left", stdContLeft + "px")
        };

        var animationHandler2;
        function slideLeft2() {
            var stdContLeft = parseFloat($(".lowerScroll .scrollWrapper .scrollableArea").css("margin-left").replace('px', '')) - 10;
            $(".lowerScroll .scrollWrapper .scrollableArea").css("margin-left", stdContLeft + "px")
        };
        function slideRight2() {
            var stdContLeft = parseFloat($(".lowerScroll .scrollWrapper .scrollableArea").css("margin-left").replace('px', '')) + 10;
            $(".lowerScroll .scrollWrapper .scrollableArea").css("margin-left", stdContLeft + "px")
        };




        $('.upperScroll .scrollingHotSpotLeft').bind("click", function (e) {
            if (isScrolling) {
                window.clearInterval(animationHandler);
                isScrolling = false;
            }
            else {
                animationHandler = window.setInterval(slideRight, 100);
                isScrolling = true;
            }
        });

        $('.upperScroll .scrollingHotSpotRight').bind("click", function (e) {


            if (isScrolling) {
                window.clearInterval(animationHandler);
                isScrolling = false;
            }
            else {
                animationHandler = window.setInterval(slideLeft, 100);
                isScrolling = true;
            }
        });

        $('#studentContainer').bind("click", function (e) {


            if (isScrolling) {
                window.clearInterval(animationHandler);
                isScrolling = false;
            }
        });


        $('.lowerScroll .scrollingHotSpotLeft').bind("click", function (e) {
            if (isScrolling2) {
                window.clearInterval(animationHandler2);
                isScrolling2 = false;
            }
            else {
                animationHandler2 = window.setInterval(slideRight2, 100);
                isScrolling2 = true;
            }
        });

        $('.lowerScroll .scrollingHotSpotRight').bind("click", function (e) {


            if (isScrolling2) {
                window.clearInterval(animationHandler2);
                isScrolling2 = false;
            }
            else {
                animationHandler2 = window.setInterval(slideLeft2, 100);
                isScrolling2 = true;
            }
        });

        $('.lowerScroll .scrollWrapper .scrollableArea').bind("click", function (e) {


            if (isScrolling2) {
                window.clearInterval(animationHandler2);
                isScrolling2 = false;
            }
        });
    }
});

