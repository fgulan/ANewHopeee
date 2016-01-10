$(document).ready(function () {
    window.alert("Ready funkcija");
    ajaxCall($("#orders-menu"));
    ajaxCall($("#meal-menu"));    
    setStatisticMenuListener();
    setWorkersMenuListener();
    setLogoutBtnListener();
});

function setStatisticMenuListener() {
    var caller = $("#statistic-menu");
    if (caller != undefined) {
        ajaxCall(caller);
    }
};

function setWorkersMenuListener() {
    var caller = $("#workers-menu");
    if (caller != undefined) {
        ajaxCall(caller);
    }
};

function setLogoutBtnListener() {
    $("#logout").click(function () {
        var actionUrl = $(this).data('url');
        
        $.ajax({
            type: 'POST',
            cache: false,
            url: actionUrl,
            success: function () {
                var redirectUrl = $("#logout").data('index-url');
                window.location.href = redirectUrl;
            }
        })
    });
};

function replaceMainContent(content) {
    $(".main-content").animate({
        opacity: 0.0
    }, 600, function () {
        $(this).empty();
        $(this).html(content);
        $(this).animate({
            opacity: 1.0
        }, 600);
    });
}

function notImplementedAlert(xhr, status, errorMsg) {
    alert("Not implemented yet \n" + status + "\n" + errorMsg );
}

function ajaxCall(caller) {
    caller.click(function () {
        var url = $(this).data('url');

        $.ajax({
            type: 'GET',
            cache: false,
            url: url,
            success:  function (content) { replaceMainContent(content); },
            error: function (xhr, status, errorMsg) { notImplementedAlert(xhr,status, errorMsg)}
        })
    })
}

$(document).on('change', '#upload', function (input) {
    readURL(this);
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#previewImage').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    } else {
        $('#previewImage').attr('src', "http://ingridwu.dmmdmcfatter.com/wp-content/uploads/2015/01/placeholder.png");
    }
}

$(document).on('click', '#addAddOnBtn', function (e) {
    e.preventDefault();
    $("#add-addon-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#add-addon-form").valid();
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
    $("#add-addon-form").submit();
});

$(document).on('click', '#editAddOnBtn', function (e) {
    e.preventDefault();
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
    $("#edit-addon-form").submit();
});

$(document).on('click', '#addMealBtn', function (e) {
    e.preventDefault();
    $("#add-meal-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#add-meal-form").valid();
        },
        resetForm: true,
        cache: false,
        success: function (response) {
            printOnModal("Jelo dodano", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Jelo nije dodano", response);
        }
    });
    $("#add-meal-form").submit();
});

$(document).on('click', '#editMealBtn', function (e) {
    e.preventDefault();
    $("#edit-meal-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#edit-meal-form").valid();
        },
        resetForm: true,
        cache: false,
        success: function (response) {
            printOnModal("Jelo dodano", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Jelo nije dodano", response);
        }
    });
    $("#edit-meal-form").submit();
});

$(document).on("click", ".meal-submenu", function () {
    $this = $(".meal-submenu");
    var url = $(this).data('url');

    $.ajax({
        type: 'GET',
        cache: false,
        url: url,
        success: function (content) {
            contentDiv = $("#meal-content");
            contentDiv.fadeOut(600, function () {
                contentDiv.empty();
                contentDiv.append(content).fadeIn(600);
            })
        },
        error: function () {
            window.alert('error');
        }
    });
});

function AddMealTypeInput() {
    var f = document.getElementsByClassName('MealType')[0].cloneNode(true).outerHTML;
    $(f).hide().appendTo($("#MealTypes")).fadeIn(600);
}

$(document).on('click', '.del-addon-btn', function (e) {
    $("#delete-modal").remove();
    $("#del-addon-list").append('<div class="modal fade" id="delete-modal" role="dialog" aria-labelledby="basicModal" aria-hidden="true"></div>');

    $this = $(this);
    var url = $this.data('url');
    var addon_id = $this.data('addon-id');
    $("#delete-modal").load('/Admin/DeleteModal?Type=dodatak&Title=' + encodeURIComponent(addon_id));

    $("#delete-modal").on('click', '#modal-del-btn', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            cache: false,
            url: url,
            data: { AddOnID: addon_id },
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

$(document).on('click', '.edit-addon-btn', function () {
    var url = $(this).data('url');
    var addon_id = $(this).data('addon-id');

    $.ajax({
        type: 'GET',
        url: url,
        cache: false,
        data: { id: addon_id },
        success: function (content) {
            var listContainer = $('#edit-addon-list').parent();
            listContainer.fadeOut(600, function () {
                listContainer.empty();
                listContainer.append(content).fadeIn(600);
            });
        },
        error: function () {
            window.alert("Error");
        }
    })
});

$(document).on('click', '.edit-meal-btn', function () {
    var url = $(this).data('url');
    var meal_id = $(this).data('meal-id');

    $.ajax({
        type: 'GET',
        cache: false,
        url: url,
        data: { id: meal_id },
        success: function (content) {
            var listContainer = $('#edit-meal-list').parent();
            listContainer.fadeOut(600, function () {
                listContainer.empty();
                listContainer.append(content).fadeIn(600);
            });
        },
        error: function () {
            window.alert("Error");
        }
    })
});

$(document).on('click', '.delete', function (e) {
    if ($('.MealType').length == 1) return;
    $(this).closest('.MealType').fadeOut('fast', function () {
        $(this).remove();
    });;
});

function AddMealTypeInput() {
    var f = document.getElementsByClassName('MealType')[0].cloneNode(true);
    var elements = $(f).find('.form-control').each(function (index) {
        // IE fuck you
        $(this).attr('value', '');
        $(this).val('');
        $(this).attr('defaultValue', '');
    });
    $(f).hide().appendTo($("#MealTypes")).fadeIn(600);
}

function printOnModal(title, content) {
    var modal = $("#modalInfoView");
    var headerLabel = modal.find("#modal-header-label");
    var bodyHeader = modal.find("#modal-body-label");

    headerLabel.html(title);
    bodyHeader.html(content);
    modal.modal('show');
}

$(document).on('click', '.open-btn', function (e) {
    var target = $($(this).data('target'));
    if (!target.hasClass('collapsing')) {
        var icon = $(this).children().first();
        if (icon.hasClass('glyphicon-menu-up')) {
            icon.removeClass('glyphicon-menu-up').addClass('glyphicon-menu-down');
        } else {
            icon.removeClass('glyphicon-menu-down').addClass('glyphicon-menu-up');
        }
    }
});

$("#dialog").dialog({
    autoOpen: false,
    show: {
        duration: 500
    },
    hide: {
        duration: 500
    }
});

$(document).on('click', '.del-meal-btn', function (e) {
    e.preventDefault();

    $this = $(this);
    var parent = $this;
    var url = $this.data('url');
    var meal_id = $this.data('meal-id');
    var meal_name = $this.data('meal-name');

    $("#dialog").dialog({
        autoOpen: false,
        title: "Obriši " + meal_name,
        show: {
            duration: 500
        },
        hide: {
            duration: 500
        },
        buttons: {
            "Odustani": function () {
                $(this).dialog("close");
            },
            "Obriši jelo": function () {
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: url,
                    data: { mealId: meal_id },
                    success: function () {
                        parent.toggle("slow", function () {
                            parent.remove();
                        });
                    },
                    error: function () {
                        window.alert("Error");
                    }
                });
                $(this).dialog("close");
            }
        }
    });
    $("#dialog").dialog("open");
});
