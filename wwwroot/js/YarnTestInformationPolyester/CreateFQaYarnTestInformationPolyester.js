$(function () {
    var yRcvId = $("#FQaYarnTestInformationPolyester_YRCVID");
    var yRcvDetailsId = $("#FQaYarnTestInformationPolyester_COUNTID");

    var errors = {
        0: {
            title: "Invalid Submission.",
            message: "We can not process your request. Please try again later."
        },
        1: {
            title: "Empty Selection.",
            message: "Please select a Challan No."
        }
    }

    yRcvId.on("change", function () {

        var formData = {
            "yRcvId": yRcvId.val()
        };

        if (yRcvId.val()) {
            $.post("/FQaYarnTestInformationPolyester/GetCountDetails", formData, function (data) {
                yRcvDetailsId.html('');
                /*yRcvDetailsId.append('<option value="" selected>Select Count Name</option>');*/

                $.each(data.receiveDetailsList, function (id, option) {

                    yRcvDetailsId.append($("<option>",
                        {
                            value: option.trnsid,
                            text: option.prod.countname
                        }));
                });
            }).fail(function () {
                toastr.error(errors[0].title, errors[0].message);
            });
        } else {
            toastr.warning(errors[1].title, errors[1].message);
            attachTo.empty();
        }
    });
});