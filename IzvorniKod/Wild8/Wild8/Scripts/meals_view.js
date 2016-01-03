$(document).ready(function () {

    fillQuantity();

    $("#CommentButton").click(function (e) {
        e.preventDefault();
        e.stopPropagation();

        $this = $(this);
        var f = $this.parent("form");
        var url = f.attr('action');
        var formData = f.serialize();
        
        $.post(url, formData, function (partialView) {
            $(".comment-list").prepend(partialView).fadeIn(600);
        });
    })
})

function fillQuantity() {
    $select = $(".1-10");
    for (i = 1; i <= 10; i++) {
        $select.append($('<option></option>').val(i).html(i))
    }
}