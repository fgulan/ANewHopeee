
    $(document).on('click', '.del-meal-btn', function (e) {
        e.preventDefault();
        $this = $(this);
        var url = $this.data('url');
        var meal_id = $this.data('meal-id');
        var meal_name = $this.data('meal-name');

        $("#modal-meal-label").html(meal_name);
        $("#modal-del-btn").click(function () {
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
