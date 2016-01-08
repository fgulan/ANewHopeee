$(".open-btn").click(function () {
    var target = $($(this).data('target'));
    if (!target.hasClass('collapsing')) {
        var icon = $(this).children().first();
        if (icon.hasClass('glyphicon-menu-up')) {
            icon.removeClass('glyphicon-menu-up').addClass('glyphicon-menu-down');
        } else {
            icon.removeClass('glyphicon-menu-down').addClass('glyphicon-menu-up');
        }
    }
});