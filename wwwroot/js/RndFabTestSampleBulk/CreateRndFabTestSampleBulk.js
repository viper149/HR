
$(function () {

    var styleName = $("#RndFabtestSampleBulk_SFINID");
    var progNo = $("#RndFabtestSampleBulk_PROGNO");

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

        $.get("/RndFabTestSampleBulk/GetProgNoBySfnIdAsync", formData, function (data) {
            progNo.val(data.ltg.prog.proG_NO);
        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });
});