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
    cache : false,
    success: function (response) {
        printOnModal("Jelo dodano", response);
    },
    error: function (xhr, status, response) {
        printOnModal("Jelo nije dodano", response);
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

function printOnModal(title, content) {
    var modal = $("#mealModal");
    var headerLabel = modal.find("#meal-modal-header-label");
    var bodyHeader = modal.find("#meal-modal-body-label");

    headerLabel.html(title);
    bodyHeader.html(content);
    modal.modal('show');
}