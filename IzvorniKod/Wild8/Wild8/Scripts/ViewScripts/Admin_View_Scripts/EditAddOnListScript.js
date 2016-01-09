    $(document).on('click', '.edit-addon-btn', function () {
        var url = $(this).data('url');
        var addon_id = $(this).data('addon-id');

        $.ajax({
            type: 'GET',
            url: url,
            cache: false,
            data: { id: addon_id },
            success: function (content) {
                var listContainer = $('#edit-addon-list').parent();
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