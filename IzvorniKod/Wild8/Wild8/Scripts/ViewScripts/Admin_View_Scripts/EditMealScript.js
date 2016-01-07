$(document).on('click', '.delete', function (e) {

    if ($('.MealType').length == 1) return;
    $(this).closest('.MealType').fadeOut('fast', function () {
        $(this).remove();
    });;
});

$("#input-20").fileinput({
    browseClass: "btn btn-primary btn-block",
    showCaption: false,
    showRemove: false,
    showUpload: false
});

function AddMealTypeInput() {
    var f = document.getElementsByClassName('MealType')[0].cloneNode(true);

    var elements = $(f).find('.form-control').each(function (index) {
        // IE fuck you
        $(this).attr('value', '');
        $(this).val('');
        $(this).attr('defaultValue', '');
    });;
    $(f).hide().appendTo($("#MealTypes")).fadeIn(600);
}