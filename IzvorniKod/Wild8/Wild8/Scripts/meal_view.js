$(document).ready(function () {

    fillQuantity();

    $("#commentButton").click(function (e) {
        e.preventDefault();
        e.stopPropagation();

        $this = $(this);

        var f = $("#commentForm");

        f.validate();
        if (!f.valid()) {
            return;
        }

        var url = f.attr('action');
        var data = f.serialize();

        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            success: function (partialView) {
                //Todo dogovori se dal se appenda ili prependa
                $(partialView).hide().prependTo("#comments").fadeIn(600);

                $('html, body').animate({
                    scrollTop: $("#commentHeader").offset().top
                }, 1000);
            },
            error: function (statusText) {
                window.alert("Error while adding comment.");
            }
        })
    })
})

function fillQuantity() {
    $select = $(".1-10");
    for (i = 1; i <= 10; i++) {
        $select.append($('<option></option>').val(i).html(i))
    }
}