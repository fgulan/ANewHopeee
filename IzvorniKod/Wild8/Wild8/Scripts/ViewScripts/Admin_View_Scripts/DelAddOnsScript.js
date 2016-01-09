
$(document).on('click', '.del-addon-btn', function (e) {
        $("#delete-modal").remove();
        $("#del-addon-list").append('<div class="modal fade" id="delete-modal" role="dialog" aria-labelledby="basicModal" aria-hidden="true"></div>');

        $this = $(this);
        var url = $this.data('url');
        var addon_id = $this.data('addon-id');
        $("#delete-modal").load('/Admin/DeleteModal?Type=dodatak&Title=' +encodeURIComponent(addon_id));

        $("#delete-modal").on('click', '#modal-del-btn', function (e) {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                cache: false,
                url: url,
                data: { AddOnID: addon_id },
                success: function () {
                    $("#delete-modal").modal("hide");
                    var parent = $this;
                    parent.toggle("slow", function () {
                        parent.remove();
                    });
                },
                error: function () {
                    window.alert("Error");
                }
            });
        });
        $("#delete-modal").modal('show');
    });