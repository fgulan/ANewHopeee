$(document).on('click', '.edit-meal-btn', function () {
    var url = $(this).data('url');
    var meal_id = $(this).data('meal-id');

    $.ajax({
        type: 'GET',
        cache: false,
        url: url,
        data: { id: meal_id },
        success: function (content) {
            var listContainer = $('#edit-meal-list').parent();
            listContainer.fadeOut(600, function () {
                listContainer.empty();
                listContainer.append(content).fadeIn(600);
            });
        },
        error: function () {
            window.alert("Error");
        }
    })
});