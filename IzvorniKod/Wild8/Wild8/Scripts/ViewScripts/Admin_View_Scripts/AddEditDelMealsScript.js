$(document).on("click", ".meal-submenu", function () {
    $this = $(".meal-submenu");
    var url = $(this).data('url');

    $.ajax({
        type: 'GET',
        cache: false,
        url: url,
        success: function (content) {
            contentDiv = $("#meal-content");
            contentDiv.fadeOut(600, function () {
                contentDiv.empty();
                contentDiv.append(content).fadeIn(600);
            })
        },
        error: function () {
            window.alert('error');
        }
    });
});
