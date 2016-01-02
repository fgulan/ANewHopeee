$(document).on('click', '#addMealButton', function(e)
{
    e.preventDefault();
    var $this = $(this);
    var f = $this.parents('form');
    var url = f.attr("action");
    var formData = f.serialize();


    $.ajax({
        type: "POST",
        url: url,
        data: formData,
        success: function (count) {
            $("#cartCount").html('<span class="glyphicon glyphicon-shopping-cart"></span> ' + count);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            window.alert(textStatus);
        }
    })
});