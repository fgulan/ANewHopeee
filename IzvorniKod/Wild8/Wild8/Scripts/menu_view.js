$(document).on('change', '#sel1', function () {
    var url = $("#sel1").data('url');
    var category = $("#sel1").find(":selected").text();
    $.ajax({
        type: "POST",
        url: url,
        data: { categoryName: category },
        success: function (mealList) {
            var meal_arr = JSON.parse(mealList);
            replaceMeals(meal_arr);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            window.alert(textStatus);
        }
    })
});

function addLoader() {
    $(".displayed").append("<tr align='middle'>"
                                    + "<td align='middle' id='mealLoader'><img src='/images/Spinners/coffieLoading.gif' alt='loading' /></td>"
                                    + "</tr>").fadeIn();
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

        $(".displayed").append(meals).animate({
            opacity: 1.0
        },600);
    })
    
}

function addMealToTable(mealWPrice) {
    var meal = mealWPrice["Meal"];
    return '<tr>' +

        //Picture
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
        sizes += '<li><input type="radio" name="' + mealWPrice["Meal"]["Name"]
              + '" checked="checked" value="' + type["Price"]
              + '">' + type["MealTypeName"] + '</li></ul>'
              + '<select class="1-10"></select>';
    }
    return sizes;
}

function addButton() {
    return '<td class="add-col">'
         + '<button type="button" class="btn btn-warning pull-right">Dodaj</button>'
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
        info += '<tr><td><input type="checkbox" name="'+meal["Name"] +'" value="'+addOn["AddOn"]["Price"] + '" >'
             + addOn["AddOn"]["AddOnID"] + '</td><td>' + addOn["AddOn"]["Price"] + 'HRK</td></tr>';
    }

    info += '</tbody></table></div></td>';

    return info;
}