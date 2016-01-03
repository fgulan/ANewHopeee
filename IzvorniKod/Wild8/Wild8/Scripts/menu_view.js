$(document).ready(function () {
    registerListeners();
    calcualtePrice();
});

function calcualtePrice() {
    
    var basePrice = [];
    var totalAddons = [];
    var quantity = [];
    $(".displayed > tr").each(function (i) {
        var $this = $(this);
       
        if (i % 2 == 0) { //Not accordian
            var index = i / 2;

            quantity[index] = 1;
            totalAddons[index] = 0;

            var valPrice = $(".size-col>ul>li>input[checked ='checked']", this).val();
            basePrice[index] = parseFloat(valPrice.replace(/,/, '.'));
            show_updated(quantity[index], basePrice[index], totalAddons[index], $this);

            $(".size-col>ul>li>input", this).change(function () {
                basePrice[index] = parseFloat($(this).val().replace(/,/, '.'));
                show_updated(quantity[index], basePrice[index], totalAddons[index], $this);
            });

            $(".1-10", this).change(function () {
                quantity[index] = $(this).val();
                show_updated(quantity[index], basePrice[index], totalAddons[index], $this);
            });
        } else { //Accordian
            var index = Math.floor(i / 2); 

            $(".accordian-body>table>tbody>tr>td>input", this).change(function () {
                if (!$(this).is(":checked")) {
                    totalAddons[index] -= parseFloat($(this).val().replace(/,/, '.'));
                } else {
                    totalAddons[index] += parseFloat($(this).val().replace(/,/, '.'));
                }
                show_updated(quantity[index], basePrice[index], totalAddons[index], $this.prev());
            });
        }
    });
}

function show_updated(quant, base, addons, $caller) {
    $price = $(".add-col > label", $caller);
    var total = parseFloat(quant) * (parseFloat(base) + addons);
    $price.html(total.toFixed(2) + " HRK");
}

function registerListeners() {

    $(document).on('click', '.expand', function(){
        $("span[aria-expanded='true']").removeClass("glyphicon-chevron-down");
        $("span[aria-expanded='true']").addClass("glyphicon-chevron-up");
    });
    $(document).on('click', '.accordion-toggle', function(){
        $("span[aria-expanded='false']").removeClass("glyphicon-chevron-up");
        $("span[aria-expanded='false']").addClass("glyphicon-chevron-down");
    });

    $("#ddlCategory").change(function (event) { loadMeals(event); });
    $("#sortSel").change(function (event) { loadMeals(event); }); 
}

function loadMeals(event) {
        event.preventDefault();
        event.stopPropagation();

        var categoryId = $('#ddlCategory').val();    //Selected category
        var categoryText = $('#ddlCategory :selected').text();
        var sort = $('#sortSel :selected').val();    //Selected sort
        var url = $('#selectionForm').data('url');   //Get url

        $.ajax({
            type: 'POST',
            url: url,
            data: { categoryId: categoryId, sort: sort },
            success: function (partialView) {
                //Replace with partial view
                $(".displayed").animate({ //Fade out 
                    opacity: 0.0
                },
                600, //600 ms fade out
                function () { //Complete function 
                    $(".displayed").empty();
                    $(".displayed").append(partialView);
                    $(".panel-heading").html("<h3>"+categoryText+"</h3>").fadeIn();
                    calcualtePrice();
                    $(".displayed").animate({ //Fade in
                        opacity: 1.0
                    },
                    600); //600 ms fade in
                })
            },
            error(xhr, status) {
                window.alert('Error. Status message: ' + status);
            }
        });
}
