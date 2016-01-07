$(document).on('click', '.delete', function (e) {
    if ($('.MealType').length == 1) return;
    $(this).closest('.MealType').fadeOut('fast', function () {
        $(this).remove();
    });
});

$("#add-meal-form").ajaxForm({
    beforeSubmit: function (formData, jqForm, options) {
        return $("#add-meal-form").valid();
    },
    resetForm: true,
    success: function (response) {
        var modal = $("#addMealModal");
        var headerLabel = modal.find("#add-meal-modal-header-label");
        var bodyHeader = modal.find("#add-meal-modal-body-label");
       
        headerLabel.html("Jelo dodano");
        bodyHeader.html(response);
        modal.modal('show');
    },
    error: function (response) {
        var modal = $("#addMealModal");
        var headerLabel = modal.find("#add-meal-modal-header-label");
        var bodyHeader = modal.find("#add-meal-modal-body-label");

        headerLabel.html("Problem");
        bodyHeader.html(errorThrown);
        modal.modal('show');
    }
});

$("#upload").fileinput({
    browseClass: "btn btn-primary btn-block",
    showCaption: false,
    showRemove: false,
    showUpload: false
});

function AddMealTypeInput() {
    var f = document.getElementsByClassName('MealType')[0].cloneNode(true).outerHTML;
    $(f).hide().appendTo($("#MealTypes")).fadeIn(600);
}