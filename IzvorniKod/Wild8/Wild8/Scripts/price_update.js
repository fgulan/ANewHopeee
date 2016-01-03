$(document).ready(function () {
    calcualtePrice();
});

$(document).ajaxComplete(function () {
    calcualtePrice();
})

function calcualtePrice() {

    var basePrice = parseFloat($("input[checked='checked']").val().replace(/,/, '.'));
    var totalAddons = 0;
    var quantity = 1;
    show_updated(quantity, basePrice, totalAddons, $(".displayed > tr"));

    $(".displayed > tr").each(function () {
        var $this = $(this);
        $(".size-col>ul>li>input", this).change(function () {
            basePrice = parseFloat($(this).val().replace(/,/, '.'));
            show_updated(quantity, basePrice, totalAddons, $this);
        });

        $(".1-10", this).change(function () {
            quantity = $(this).val();
            show_updated(quantity, basePrice, totalAddons, $this);
        });

        $(".accordian-body>table>tbody>tr>td>input", this).change(function () {
            if (!$(this).is(":checked")) {
                totalAddons -= parseFloat($(this).val().replace(/,/, '.'));
            } else {
                totalAddons += parseFloat($(this).val().replace(/,/, '.'));
            }
            show_updated(quantity, basePrice, totalAddons, $this.prev());
        });

    });
}

function show_updated(quant, base, addons, $caller) {
    $price = $(".add-col > label", $caller);
    var total = parseFloat(quant) * (parseFloat(base) + addons);
    $price.html(total.toFixed(2) + " HRK");
}