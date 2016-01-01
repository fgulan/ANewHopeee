/*
   ___   _  __           __ __
  / _ | / |/ /__ _    __/ // /__  ___  ___ ___ ___
 / __ |/    / -_) |/|/ / _  / _ \/ _ \/ -_) -_) -_)
/_/ |_/_/|_/\__/|__,__/_//_/\___/ .__/\__/\__/\__/
                               /_/
*/

var previousScroll = 0,
    headerOrgOffset = $('#header').height();
$('#header-wrap').height($('#header').height());
$(window).scroll(function() {
    var currentScroll = $(this).scrollTop();
    if (currentScroll > headerOrgOffset) {
        if (currentScroll > previousScroll) {
            $('#header-wrap').slideUp();
        } else {
            $('#header-wrap').slideDown();
        }
    } else {
        $('#header-wrap').slideDown();
    }
    previousScroll = currentScroll;
});