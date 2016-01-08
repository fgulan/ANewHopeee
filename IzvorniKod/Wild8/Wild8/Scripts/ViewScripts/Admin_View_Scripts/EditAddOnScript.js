$("#edit-addon-form").ajaxForm({
    beforeSubmit: function (formData, jqForm, options) {
        return $("#edit-addon-form").valid();
    },
    resetForm: false,
    cache: false,
    success: function (response) {
        printOnModal("Dodatak spremljen", response);
    },
    error: function (xhr, status, response) {
        printOnModal("Dodatak nije spremljen", response);
    }
});

function printOnModal(title, content) {
    var modal = $("#mealModal");
    var headerLabel = modal.find("#meal-modal-header-label");
    var bodyHeader = modal.find("#meal-modal-body-label");

    headerLabel.html(title);
    bodyHeader.html(content);
    modal.modal('show');
}
