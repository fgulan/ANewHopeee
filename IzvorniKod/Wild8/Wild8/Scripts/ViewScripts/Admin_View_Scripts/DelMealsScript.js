
    $(document).on('click', '.del-meal-btn', function (e) {
        e.preventDefault();
        $("#delete-modal").remove();
        $("#del-meal-list").append('<div class="modal fade" id="delete-modal" role="dialog" aria-labelledby="basicModal" aria-hidden="true"></div>');

        $this = $(this);
        var url = $this.data('url');
        var meal_id = $this.data('meal-id');
        var meal_name = $this.data('meal-name');
        var thisdata = $(this).attr('data-meal-name');

        $("#delete-modal").load('/Admin/DeleteModal?Type=jelo&Title=' +encodeURIComponent(meal_name));
        $("#delete-modal").on('click', '#modal-del-btn', function (e) {
            $.ajax({
                type: 'POST',
                cache: false,
                url: url,
                data: { mealId: meal_id },
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
