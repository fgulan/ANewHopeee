$("#edit-addon-form").ajaxForm({
    beforeSubmit: function (formData, jqForm, options) {
        return $("#edit-addon-form").valid();
    },
    resetForm: false,
    cache: false,
    success: function (response) {
        $("#OldName").val($("#Name").val());
        printOnModal("Dodatak spremljen", response);
    },
    error: function (xhr, status, response) {
        printOnModal("Dodatak nije spremljen", response);
    }
});

function printOnModal(title, content) {
    var modal = $("#addOnModal");
    var headerLabel = modal.find("#addon-meal-modal-header-label");
    var bodyHeader = modal.find("#addon-meal-modal-body-label");

    headerLabel.html(title);
    bodyHeader.html(content);
    modal.modal('show');
}
