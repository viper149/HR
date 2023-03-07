
$(function () {

    var styleName = $("#RndFabtestSample_SFINID");
    var progNo = $("#RndFabtestSample_PROGNO");

    var errors = {
        0: {
            title: "Invalid Submission.",
            message: "We can not process your request. Please try again later."
        }
    }
    styleName.on("change", function () {

        var formData = {
            "sfnId": styleName.val()
        }

        $.get("/RndFabTestSample/GetProgNoBySfnIdAsync", formData, function (data) {
            progNo.val(data.ltg.prog.proG_NO);
        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });

});