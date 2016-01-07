$("#add-addon-form").submit(function (e) {
    e.preventDefault(); //Prevent submition
    var form = $(this);
    var formData = new FormData(this);

    form.validate();

    if (form.valid()) {

        $.ajax({
            type: 'POST',
            url: $(this).attr('action'),
            data: formData,
            success: function (response) {
                var modal = $("#addMealModal");
                var modalHeader = modal.find(".modal-header");
                var headerLabel = modal.find("#add-meal-modal-header-label");
                var bodyHeader = modal.find("#add-meal-modal-body-label");

                modalHeader.toggleClass(".bg-success");
                headerLabel.html("Dodatak dodan");
                bodyHeader.html(response);
                modal.modal('show');
            },
            error: function (xhr, statusCode, errorThrown) {
                var modal = $("#addMealModal");
                var modalHeader = modal.find(".modal-header");
                var headerLabel = modal.find("#add-meal-modal-header-label");
                var bodyHeader = modal.find("#add-meal-modal-body-label");

                modalHeader.toggleClass(".bg-danger");
                headerLabel.html("Problem");
                bodyHeader.html(errorThrown);
                modal.modal('show');
            },

        }).done(function () {
            form.each(function () {
                this.reset();
            });
        });
    }
});