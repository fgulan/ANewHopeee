$(document).ready(function () {
    $("#login-btn").click(function () {
        var btn  = $(this).button('loading');
        var form = $("#login-form");

        form.validate();
        if (!form.valid()) {
            btn.button('reset');
            return;
        }

        var formData = form.serialize();
        var url = form.data("url");
        var redirectTo = form.data("redirect-to");

        $.ajax({
            type: 'POST',
            cache: false,
            url: url,
            data: formData,
            success: function (logedIn) {
                if (logedIn == 'True') { //some js nonsense
                    window.location.href = redirectTo;
                } else {
                    btn.button('reset');
                    form.popover({
                        title: 'Greška',
                        content: 'Krivo korisničko ime ili zaporka',
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