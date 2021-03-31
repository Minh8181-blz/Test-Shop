(function ($) {
    function SignUp() {
        var form = $('#frm_signup');

        form.submit(function (e) {
            e.preventDefault();

            if (validateForm()) {
                console.log('ok');

                FormSubmitter.SubmitUrlEncodedForm(form, function (success, data) {
                    if (success) {
                        console.log('s', data);
                    }
                    else {
                        console.log('f', data);
                    }
                });
            }
            else {
                console.log('not ok');
            }
        });

        function validateForm() {
            return form.valid();
        }
    }

    $(document).ready(function () {
        new SignUp();
    });
})(jQuery);

