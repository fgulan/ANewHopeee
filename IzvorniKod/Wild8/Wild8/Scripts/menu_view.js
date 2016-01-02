$(document).on('change', '#sel1', function () {
    var url = $("#sel1").data('url');
    var category = $("#sel1").find(":selected").text();

    $.ajax({
        type: "POST",
        url: url,
        data: { categoryName: category },
        success: function (mealList) {
            var meal_arr = JSON.parse(mealList);
            alert(meal_arr.length);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            window.alert(textStatus);
        }
    })
});

