
var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

$(function () {

    var catId = $("#FGsProductInformation_CATID");
    var scatId = $("#FGsProductInformation_SCATID");

    catId.on("change", function () {

        var formData = {
            "catId": catId.val()
        }

        $.get("/FGsProductInformation/GetSubCat", formData, function (data) {

            scatId.html("");
            scatId.append('<option value="" selected>Select Sub-Category</option>');

            $.each(data, function (id, option) {
                scatId.append($("<option>",
                    {
                        value: option.scatid,
                        text: option.scatname
                    }));
            });

        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });
});