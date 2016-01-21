$(document).ready(function () {

    fillQuantity();
    setCommentListener();
    setPriceListener();
    setAddToCartListener();
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

      
        $('.collapse').collapse('hide');




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
    
    var valPrice = $(".mealtypes>table>tbody>tr>td>input[checked ='checked']").val();
    var basePrice = 0;
    if (valPrice != null) {
        basePrice = parseFloat(valPrice.replace(/,/, '.'));
    }
    var quantity = 1;
    var totalAddons = 0;

    show_updated(quantity, basePrice, totalAddons);

    $(".mealtypes>table>tbody>tr>td>input").change(function () {
        basePrice = parseFloat($(this).val().replace(/,/, '.'));
        show_updated(quantity, basePrice, totalAddons);
    });

    $(".1-10").change(function () {
        quantity = parseFloat($(this).val().replace(/,/, '.'));
        show_updated(quantity, basePrice, totalAddons);
    })

    $(".addons>table>tbody>tr>td>input").change(function () {
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

function setAddToCartListener() {
    $('#addMealButton').click(function () {

        var url = $("#meal-form").attr('action');
        var mealID = $("#mealID").val();
        var mealTypeName = $(".mealtypes>table>tbody>tr>td>input[checked ='checked']").val().split("#")[1];
        var count = $(".1-10 option:selected").val();
        var addons = [];
        $(".AddOn[checked ='checked']").each(function (index) {
            addons[index] = $(this).val();
        });
        
        $.ajax({
            type: 'POST',
            url: url,
            data: { count: count,mealID: mealID ,mealTypeName: mealTypeName, addOnNames: addons },
            success: function(count) {
                $("#cartCount").html('<span class="glyphicon glyphicon-shopping-cart"></span> ' + count);
            },
            error: function() {
                
            }
        });
    });
}