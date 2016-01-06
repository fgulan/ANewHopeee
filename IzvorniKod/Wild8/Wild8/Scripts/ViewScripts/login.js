$(document).ready(function () {
    $("#login-btn").click(function () {
        $(this).button('loading');
        var form = $("#login-form");

        form.validate();
        if (!form.valid()) {
            return;
        }

        var formData = form.serialize();
        var url = form.data("url");
        var redirectTo = form.data("redirect-to");


        $.ajax({
            type: 'POST',
            url: url,
            data: formData,
            success: function (logedIn) {
                if (logedIn || logedIn == 'True') { //some js nonsance
                    window.location.href = redirectTo;
                } else {
                    $("#login-btn").button('reset');
                    form.popover({
                        title: 'Greška',
                        content: 'Krivo korisničko ime ili zaporka',
                        trigger: 'focus',
                        placement: 'left',
                        viewport: {
                             selector: 'body', padding: 15
                        }
                    });
                    form.popover('show');
                }
            }
        })
    });
})