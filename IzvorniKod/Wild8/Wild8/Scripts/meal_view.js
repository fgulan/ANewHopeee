$(document).ready(function () {

    fillQuantity();
    setCommentListener();
    setPriceListener();

})

function setCommentListener() {
    $("#commentButton").click(function (e) {
        e.preventDefault();
        e.stopPropagation();

        $this = $(this);

        var f = $("#commentForm");

        f.validate();
        if (!f.valid()) {
            return;
        }

        var url = f.attr('data-url');
        var data = f.serialize();

        $.ajax({
            type: 'POST',
            cache: false,
            url: url,
            data: data,
            success: function (partialView) {
                //Todo dogovori se dal se appenda ili prependa
                $(partialView).hide().prependTo("#comments").fadeIn(600);

                $('html, body').animate({
                    scrollTop: $("#commentHeader").offset().top
                }, 1000);
            },
            error: function (statusText) {
                window.alert("Error while adding comment.");
            }
        })
    })
}

function setPriceListener() {
    
    var valPrice = $(".col-md-1>ul>li>input[checked ='checked']").val();
    var basePrice = 0;
    if (valPrice != null) {
        basePrice = parseFloat(valPrice.replace(/,/, '.'));
    }
    var quantity = 1;
    var totalAddons = 0;

    show_updated(quantity, basePrice, totalAddons);

    $(".col-md-1>ul>li>input").change(function () {
        basePrice = parseFloat($(this).val().replace(/,/, '.'));
        show_updated(quantity, basePrice, totalAddons);
    });

    $(".1-10").change(function () {
        quantity = parseFloat($(this).val().replace(/,/, '.'));
        show_updated(quantity, basePrice, totalAddons);
    })

    $(".col-md-2>table>tbody>tr>td>input").change(function () {
        if (!$(this).is(":checked")) {
            totalAddons -= parseFloat($(this).val().replace(/,/, '.'));
        } else {
            totalAddons += parseFloat($(this).val().replace(/,/, '.'));
        }
        show_updated(quantity, basePrice, totalAddons);
    })
}

function show_updated(quantity, basePrice, addons) {
    var total = quantity * (basePrice + addons);
    $("#sum").html(total.toFixed(2) + " HRK");
}

function fillQuantity() {
    $select = $(".1-10");
    for (i = 1; i <= 10; i++) {
        $select.append($('<option></option>').val(i).html(i))
    }
}