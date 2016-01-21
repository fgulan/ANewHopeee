$(document).ready(function () {
    if (typeof (Storage) == "undefined") {
        //Print to user that he can not use this browser 
    }
    ajaxCall($("#meal-menu"));    
    setStatisticMenuListener();
    setWorkersMenuListener();
    setStaticInfoMenuListener();
    setLogoutBtnListener();
});

function setStatisticMenuListener() {
    var caller = $("#statistic-menu");
    if (caller != undefined) {
        ajaxCall(caller);
    }
};

function setWorkersMenuListener() {
    var caller = $("#employee-menu");
    if (caller != undefined) {
        ajaxCall(caller);
    }
};

function setStaticInfoMenuListener() {
    var caller = $("#static-info-menu");
    if (caller != undefined) {
        ajaxCall(caller);
    }
}

function setLogoutBtnListener() {
    $("#logout").one('click', function () {
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

$(document).on('click', '#editOwnerInfoBtn', function (e) {
    e.preventDefault();
    $("#edit-owner-info-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#edit-owner-info-form").valid();
        },
        resetForm: false,
        cache: false,
        success: function (response) {
            printOnModal("Informacije spremljene", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Informacije nisu spremljene", response);
        }
    });
    $("#edit-owner-info-form").submit();
});

$(document).on('click', '#editRestaurauntInfo', function (e) {
    e.preventDefault();
    $("#edit-restauraunt-info-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#edit-restauraunt-info-form").valid();
        },
        resetForm: false,
        cache: false,
        success: function (response) {
            printOnModal("Informacije spremljene", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Informacije nisu spremljene", response);
        }
    });
    $("#edit-restauraunt-info-form").submit();
});

$(document).on('click', '#addCategoryBtn', function (e) {
    e.preventDefault();
    $("#add-category-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#add-category-form").valid();
        },
        resetForm: false,
        cache: false,
        success: function (response) {
            printOnModal("Kategorija spremljena", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Kategorija nije spremljena", response);
        }
    });
    $("#add-category-form").submit();
});

$(document).on('click', '#editCategoryBtn', function (e) {
    e.preventDefault();
    $("#edit-category-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#edit-category-form").valid();
        },
        resetForm: false,
        cache: false,
        success: function (response) {
            printOnModal("Kategorija spremljena", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Kategorija nije spremljena", response);
        }
    });
    $("#edit-category-form").submit();
});

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
            printOnModal("Jelo spremljeno", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Jelo nije spremljeno", response);
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
        resetForm: false,
        cache: false,
        success: function (response) {
            printOnModal("Jelo spremljeno", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Jelo nije spremljeno", response);
        }
    });
    $("#edit-meal-form").submit();
});

$(document).on('click', '#editEmployeeBtn', function (e) {
    e.preventDefault();
    $("#edit-employee-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#edit-employee-form").valid();
        },
        resetForm: false,
        cache: false,
        success: function (response) {
            printOnModal("Djelatnik spremljen", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Djelatnik nije spremljen", response);
        }
    });
    $("#edit-employee-form").submit();
});

$(document).on('click', '#addEmployeeBtn', function (e) {
    e.preventDefault();
    $("#add-employee-form").ajaxForm({
        beforeSubmit: function (formData, jqForm, options) {
            return $("#add-employee-form").valid();
        },
        resetForm: false,
        cache: false,
        success: function (response) {
            printOnModal("Djelatnik spremljen", response);
        },
        error: function (xhr, status, response) {
            printOnModal("Djelatnik nije spremljen", response);
        }
    });
    $("#add-employee-form").submit();
});

$(document).on('click', '.edit-employee-btn', function () {
    var url = $(this).data('url');
    var employee_id = $(this).data('employee-id');

    $.ajax({
        type: 'GET',
        url: url,
        cache: false,
        data: { EmployeeID: employee_id },
        success: function (content) {
            var listContainer = $('#employee-list').parent();
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

$(document).on("click", ".employee-submenu", function () {
    $this = $(".employee-submenu");
    var url = $(this).data('url');

    $.ajax({
        type: 'GET',
        cache: false,
        url: url,
        success: function (content) {
            contentDiv = $("#employee-content");
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

$(document).on('click', '.del-meal-btn', function (e) {
    e.preventDefault();
    $("#delete-modal").remove();
    $("#del-meal-list").append('<div class="modal fade" id="delete-modal" role="dialog" aria-labelledby="basicModal" aria-hidden="true"></div>');

    $this = $(this);
    var parent = $this;
    var url = $this.data('url');
    var meal_id = $this.data('meal-id');
    var meal_name = $this.data('meal-name');
    $("#delete-modal").load('/Admin/DeleteModal?Type=jelo&Title=' + encodeURIComponent(meal_name));

    $("#delete-modal").modal('show');

    $("#delete-modal").on('click', '#modal-del-btn', function (e) {
        e.preventDefault();
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
    });
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

$(document).on('click', '.deleteMealType', function (e) {
    if ($('.MealType').length == 1) return;
    $(this).closest('.MealType').fadeOut('fast', function () {
        $(this).remove();
    });;
});

$(document).on('click', '.del-category-btn', function (e) {
    e.preventDefault();
    $("#delete-modal").remove();
    $("#del-category-list").append('<div class="modal fade" id="delete-modal" role="dialog" aria-labelledby="basicModal" aria-hidden="true"></div>');

    $this = $(this);
    var parent = $this;
    var url = $this.data('url');
    var category_id = $this.data('category-id');
    var category_name = $this.data('category-name');
    $("#delete-modal").load('/Admin/DeleteModal?Type=kategorija&Title=' + encodeURIComponent(category_name));

    $("#delete-modal").modal('show');

    $("#delete-modal").on('click', '#modal-del-btn', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            cache: false,
            url: url,
            data: { id: category_id },
            success: function () {
                parent.toggle("slow", function () {
                    parent.remove();
                });
            },
            error: function () {
                window.alert("Error");
            }
        });
    });
});

$(document).on('click', '.edit-category-btn', function () {
    var url = $(this).data('url');
    var category_id = $(this).data('category-id');

    $.ajax({
        type: 'GET',
        url: url,
        cache: false,
        data: { id: category_id },
        success: function (content) {
            var listContainer = $('#edit-category-list').parent();
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

function AddMealTypeInput() {
    var f = document.getElementsByClassName('MealType')[0].cloneNode(true);
    var elements = $(f).find('.form-control').each(function (index) {
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

$(document).on('click', "#own-info-form", function(e) {
    var url = $(this).data('url');
    var contentParent = $("#static-info-content");
    replaceContent(url, contentParent);
}); 

$(document).on('click', "#own-pic-form", function() {
    var url = $(this).data('url');
    var contentParent = $("#static-info-content");
    replaceContent(url, contentParent);
});

$(document).on('click', "#restaurant-info-form", function() {
    var url = $(this).data('url');
    var contentParent = $("#static-info-content");
    replaceContent(url, contentParent);
});

$(document).on('click', "#restaurant-pic-form", function () {
    var url = $(this).data('url');
    var contentParent = $("#static-info-content");
    replaceContent(url, contentParent);
});


function replaceContent(url, contentParent, data) {
    $.ajax({
        type: 'GET',
        url: url,
        data: data,
        cache: false,
        success: function(content) {
            contentParent.fadeOut(600 , function(){
                contentParent.empty();
                contentParent.append(content).hide().fadeIn(600);
            });
        }
    });
}

