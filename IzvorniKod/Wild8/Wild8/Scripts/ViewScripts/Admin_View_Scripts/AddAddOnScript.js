$("#add-addon-form").ajaxForm({
    beforeSubmit: function (formData, jqForm, options) {
        return $("#add-addon-form").valid();
    },
    resetForm: true,
    cache : false,
    success: function (response) {
        printOnModal("Dodatak dodan.", response);
    },
    error: function (xhr, status, response) {
        printOnModal("Dodatak nije dodan", response);
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
