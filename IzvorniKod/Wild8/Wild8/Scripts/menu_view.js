$(document).ready(function () {
    fillMissingInfo();
});

$(document).on('change', '#sel1', function () {
    var url = $("#sel1").data('url');
    var category = $("#sel1").val();
    var sort = $("#sel2").val();

    $(".panel-heading>h3").html(category).fadeIn(); //Replace heding 

    $.ajax({
        type: "POST",
        url: url,
        data: { categoryName: category, sort: sort},
        success: function (mealList) {
            var meal_arr = JSON.parse(mealList);
            replaceMeals(meal_arr);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            window.alert(textStatus);
        }
    })
})

function fillMissingInfo() {
    fillQuantity();
    addHotIcon();
    calculateAndAtachPriceListeners();
}

function calculateAndAtachPriceListeners() {

    $(".displayed > tr").each(function () {
        var $this = $(this);

        var strBasePrice = $("input[checked='checked']", this).val();
        if(strBasePrice != undefined) {
            var basePrice = parseFloat(strBasePrice.replace(/,/, '.'));
            var totalAddons = 0;
            var quantity = 1;
            show_updated(quantity, basePrice, totalAddons, $this);
        }
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
                totalAddons -= parseFloat($(this).val().split("#")[0].replace(/,/, '.'));
            } else {
                totalAddons += parseFloat($(this).val().split("#")[0].replace(/,/, '.'));
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

function addHotIcon() {
    $(".meal-hot").prepend("<div class='hot-label'><span class='glyphicon glyphicon-fire'></span> HOT! </div>");
}

function fillQuantity() {
    var $select = $(".1-10");
    for (i = 1; i <= 10; i++) {
        $select.append($('<option></option>').val(i).html(i))
    }
}

function replaceMeals(data) {
    $(".displayed").animate({
        opacity: 0.0
    }, 600, function () {
        $(".displayed").empty();

        var meals = '';
        for (var index in data) {
            meals += addMealToTable(data[index]);
        }

        if (data.length != 0) {
            $(".displayed").append(meals).animate({
                opacity: 1.0
            }, 600, fillMissingInfo());
        }
    })

}

function addMealToTable(mealWPrice) {
    var meal = mealWPrice["Meal"];
    return '<tr>' +

        //Picture TODO: change to food picture
        '<td class="img-col"> <a href="@Url.Action("Details", "Meals", new { id=' + meal["MealID"] + ' })">'
        + '<img class="food-image img-responsive" src="http://lorempixel.com/200/200"></a>' +

        //Name
        addName(mealWPrice) +

        //Description
        '<td class="desc-col">' + meal["Description"] + '</td>' +

        //Sizes
        addSizes(mealWPrice) +

        //Dodaj button
        addButton() +

        //Accordion button
        addAccordionButton(meal) +
        '</tr>' +
        '<tr>' + addAccordionInfo(mealWPrice) + '</tr>';
}

function addName(mealWPrice) {
    if (mealWPrice["isHot"] == true) {
        return '<td class="name-col meal-hot">' + mealWPrice["Meal"]["Name"] + '</td>';
    } else {
        return '<td class="name-col">' + mealWPrice["Meal"]["Name"] + '</td>';
    }
}

function addSizes(mealWPrice) {
    var sizes = '<td class="size-col"><ul>';

    for (var typeIndex in mealWPrice["Types"]) {
        var type = mealWPrice["Types"][typeIndex];

        if (typeIndex == 0) {
            sizes += '<li><input type="radio" name="' + mealWPrice["Meal"]["Name"]
                  + '" checked="checked" value="' + type["Price"] + '#' + type["MealTypeName"]
                  + '">' + type["MealTypeName"] + '</li>';
        } else {
            sizes += '<li><input type="radio" name="' + mealWPrice["Meal"]["Name"]
                  + '" value="' + type["Price"] + '#' + type["MealTypeName"]
                  + '">' + type["MealTypeName"] + '</li>';
        }

    }
    sizes += '</ul><select id="Count" name="Count" form="AddMealForm" class="1-10"></select>';

    return sizes;
}

function addButton() {
    return '<td class="add-col">'
         + '<button type="submit" id="addMealButton" class="btn btn-warning pull-right">Dodaj</button>'
         + '<label class="pull-right"></label>'
         + '</td>'
}

function addAccordionButton(meal) {
    return '<td class="expand-col">'
         + '<span class="glyphicon glyphicon-chevron-down expand accordion-toggle" data-toggle="collapse" data-target="#'
         + meal["Name"] + '"></span></td>'
}

function addAccordionInfo(mealWPrice) {
    var meal = mealWPrice["Meal"];
    var info = '<td colspan="12" class="hiddenRow"><div class="accordian-body collapse" id="' + meal["Name"] + '">'
             + '<table><tbody>'

    for (var addOnIndex in meal["AddOns"]) {
        var addOn = meal["AddOns"][addOnIndex];
        info += '<tr><td><input type="checkbox" name="' + meal["Name"] + '" value="' + addOn["AddOn"]["Price"] + '" >'
             + addOn["AddOn"]["AddOnID"] + '</td><td>' + addOn["AddOn"]["Price"] + 'HRK</td></tr>';
    }

    info += '</tbody></table></div></td>';

    return info;
}
