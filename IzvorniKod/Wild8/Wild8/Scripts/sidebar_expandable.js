(function () {
    $(function () {
        var collapseMyMenu, expandMyMenu, hideMenuTexts, showMenuTexts;
        expandMyMenu = function () {
            return $("nav.sidebar").switchClass("sidebar-menu-collapsed", "sidebar-menu-expanded", 400, function () {
                showMenuTexts();
            });
        };
        collapseMyMenu = function () {
            return $("nav.sidebar").switchClass("sidebar-menu-expanded", "sidebar-menu-collapsed", 400);
        };
        showMenuTexts = function () {
            return $("span.expanded-element").show(400);
        };
        hideMenuTexts = function () {
            return $("span.expanded-element").hide(function () {
                collapseMyMenu();
            });
        };
        return $("#justify-icon").click(function (e) {
            if ($(this).parent("nav.sidebar").hasClass("sidebar-menu-collapsed")) {
                expandMyMenu();
                $("#main-container").animate({ marginLeft: '+=56px'}, 400);
                $(this).css({
                    color: "#000"
                });
            } else if ($(this).parent("nav.sidebar").hasClass("sidebar-menu-expanded")) {
                hideMenuTexts();

                $("#main-container").delay(400).animate({ marginLeft: '-=56px' }, 400);
                $(this).css({
                    color: "#FFF"
                });
            }
            return false;
        });
    });

}).call(this);