
var attachTo = $("#fGsGatepassReturnRcvTable");
var gpId = $("#FGsGatepassReturnRcvMaster_GPID");
var productid = $("#FGsGatepassReturnRcvDetails_PRODID");


var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    },
    1: {
        title: "No Quantity!!.",
        message: "Please Enter Receive Quantity"
    }
}

$(function () {
    var unit = $("#unit");
    var btnAdd = $("#addToList");
    var rcvQty = $("#FGsGatepassReturnRcvDetails_RCV_QTY");

    getByGpId();

    gpId.on("change", function () {
        getByGpId();
    });

    productid.on("change", function () {
        if (productid.val()) {

            var formData = {
                "prodId": productid.val()
            }

            $.get("/FGsGatepassReturnRcv/GetProductDetails", formData, function (data) {

                unit.text(data.unitNavigation.uname);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });

    btnAdd.on("click", function () {
        if (!parseInt(rcvQty.val()) > 0) {
            toastr.error(errors[1].message, errors[1].title);
            return false;
        }

        var formData = $("#form").serializeArray();

        $.post("/FGsGatepassReturnRcv/AddToList",
            formData,
            function (partialView) {
                attachTo.html(partialView);
            }).fail(function () {
                toastr.error(errors[0].title, errors[0].message);
            });
    });
});

function getByGpId() {
    if (gpId.val()) {
        var formData = {
            "gpId": gpId.val()
        };

        $.get("/FGsGatepassReturnRcv/GetProductList", formData, function (data) {

            productid.html("");
            productid.append('<option value="" selected>Select Product</option>');

            $.each(data[0].fGsGatepassInformationDList,
                function (id, option) {
                    productid.append($("<option>",
                        {
                            value: option.prod.prodid,
                            text: option.prod.prodname
                        }));
                });
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

function RemoveIndent(index) {
    swal({
        title: "Please Confirm",
        text: `You won't able to revert, Are you sure to remove item no. - ${index + 1}`,
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {
            if (isConfirm) {

                const formData = $("#form").serializeArray();
                formData.push({ name: "RemoveIndex", value: index });
                formData.push({ name: "IsDelete", value: true });

                $.post("/FGsGatepassReturnRcv/AddToList", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }
        });
}