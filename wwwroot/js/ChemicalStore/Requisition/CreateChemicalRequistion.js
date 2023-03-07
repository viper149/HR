
var attachTo = $("#chemPurchaseTable");
var submitBtn = $("#btn_submit");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

$(function () {

    var productId = $("#FChemStoreIndentdetails_PRODUCTID");
    var qty = $("#FChemStoreIndentdetails_QTY");
    var btnAdd = $("#btnAdd");

    btnAdd.on("click", function () {

        if (productId.val() && qty.val()) {

            const formData = $("#form").serializeArray();

            var xhr = $.post("/ChemicalRequisition/AddOrRemoveFromList", formData, function (partialView) {

                if (xhr.getResponseHeader("HasItems") === "True") {
                    submitBtn.prop("disabled", false);
                } else {
                    submitBtn.prop("disabled", true);
                }

                attachTo.html(partialView);
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });

});

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

                var xhr = $.post("/ChemicalRequisition/AddOrRemoveFromList", formData, function (partialView) {

                    if (xhr.getResponseHeader("HasItems") === "True") {
                        submitBtn.prop("disabled", false);
                    } else {
                        submitBtn.prop("disabled", true);
                    }

                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }
        });
}