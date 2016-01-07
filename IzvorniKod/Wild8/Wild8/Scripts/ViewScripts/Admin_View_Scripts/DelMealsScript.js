
    $(document).on('click', '.del-meal-btn', function () {
        $this = $(this);
        var url = $this.data('url');
        var meal_id = $this.data('meal-id');
        var meal_name = $this.data('meal-name');

        window.alert(url + meal_id + meal_name);

        $("#modal-meal-label").html(meal_name);
        $("#modal-del-btn").click(function () {
            $.ajax({
                type: 'POST',
                url: url,
                data: { mealId: meal_id },
                success: function () {
                    $("#delete-modal").modal("hide");
                    var parent = $this.parent().parent(); //Get li not span
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
