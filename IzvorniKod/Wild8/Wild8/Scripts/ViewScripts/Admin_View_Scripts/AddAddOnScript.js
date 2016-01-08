$("#add-addon-form").ajaxForm({
    beforeSubmit: function (formData, jqForm, options) {
        return $("#add-addon-form").valid();
    },
    resetForm: true,
    success: function (response) {
        printOnModal("Jelo dodano", response);
    },
    error: function (xhr, status, response) {
        printOnModal("Jelo nije dodano", response);
    }
});

function printOnModal(title, content) {
    var modal = $("#addOnmealModal");
    var headerLabel = modal.find("#addon-meal-modal-header-label");
    var bodyHeader = modal.find("#addon-meal-modal-body-label");

    headerLabel.html(title);
    bodyHeader.html(content);
    modal.modal('show');
}
