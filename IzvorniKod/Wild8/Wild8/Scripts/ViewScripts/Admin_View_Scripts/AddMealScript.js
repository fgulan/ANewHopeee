$(document).on('click', '.delete', function (e) {
    if ($('.MealType').length == 1) return;
    $(this).closest('.MealType').fadeOut('fast', function () {
        $(this).remove();
    });
});

$("#add-meal-form").submit(function (e) {
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
                headerLabel.html("Jelo dodano");
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

$("#input-20").fileinput({
    browseClass: "btn btn-primary btn-block",
    showCaption: false,
    showRemove: false,
    showUpload: false
});

function AddMealTypeInput() {
    var f = document.getElementsByClassName('MealType')[0].cloneNode(true).outerHTML;
    $(f).hide().appendTo($("#MealTypes")).fadeIn(600);
}