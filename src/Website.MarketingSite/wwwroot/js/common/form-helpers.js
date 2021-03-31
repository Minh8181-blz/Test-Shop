var FormSubmitter = {
    SubmitUrlEncodedForm: function (formElm, callback) {
        if (!formElm || formElm.length < 1) {
            if (callback instanceof Function) {
                callback(false);
            }
        }

        $.ajax({
            method: formElm.attr('method'),
            url: formElm.attr('action'),
            data: formElm.serialize(),
            success: function (data) {
                return callback(true, data);
            },
            error: function (err) {
                return callback(false, err);
            }
        })
    }
};