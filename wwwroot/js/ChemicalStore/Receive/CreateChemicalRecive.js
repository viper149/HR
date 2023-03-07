
var attachTo = $("#chemicalReceiveDetailsTable");

$(function () {
    var invid = $("#FChemStoreReceiveMaster_INVID");
    var cindid = $("#FChemStoreReceiveDetails_CINDID");
    var cinddate = $("#FChemStoreReceiveDetails_CINDDATE");
    var productId = $("#FChemStoreReceiveDetails_PRODUCTID");
    var batchNo = $("#FChemStoreReceiveDetails_BATCHNO");
    var freshQty = $("#FChemStoreReceiveDetails_FRESH_QTY");
    var unit = $("#FChemStoreReceiveDetails_UNIT");
    var unitName = $("#FChemStoreReceiveDetails_FBasUnits_UNAME");
    var attachComImpInvoice = $("#ComImpInvoice");

    var rcvType = $("#FChemStoreReceiveMaster_RCVTID");
    var adjustedWith = $("#FChemStoreReceiveDetails_ADJUSTED_WITH");

    var btnAdd = $("#addToList");

    var errors = {
        0: {
            title: "Invalid Submission.",
            message: "We can not process your request. Please try again later."
        },

        1: {
            title: "Product must be selected!!.",
            message: "Please Select Product!!!"
        },
        2: {
            title: "Batch No Can not be empty!!.",
            message: "Please Enter Batch No!!!"
        },
        3: {
            title: "Quantity Can not be empty!!.",
            message: "Please Enter Quantity!!!"
        }
    }

    rcvType.on("change", function () {

        var rcvTypeVal = rcvType.val();

        var targetItems = [
            "20002",
            "20004"
        ];

        if ($.inArray(rcvTypeVal, targetItems) > -1) {
            adjustedWith.parents('div').eq(1).addClass("d-none");
        } else {
            adjustedWith.parents('div').eq(1).removeClass("d-none");
        }
    });

    invid.on("change", function () {

        if (invid.val()) {

            var formData = {
                "id": invid.val()
            };

            $.get("/ChemicalReceive/GetInvoiceDetails", formData, function (partialView) {

                attachComImpInvoice.html(partialView);

                //invAmount.val(data.comImpInvdetails.amount);
                //invQty.val(data.comImpInvdetails.qty);
                //invRate.val(data.comImpInvdetails.rate);
                //invCurrency.val(data.currency);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });

    cindid.on("change", function () {

        var selectedCindid = cindid.val();

        $.get("/ChemicalReceive/GetIndentMaster", { "id": selectedCindid }, function (data) {

            cinddate.val(new Date(data.cinddate).toISOString().slice(0, 10));

        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });

    productId.on("change", function () {

        if (productId.val()) {

            var formData = {
                "id": productId.val()
            }

            $.get("/ChemicalReceive/GetProductDetails", formData, function (data) {

                unit.val(data.uid);
                unitName.val(data.uname);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });


    btnAdd.on("click", function () {
        if (productId[0].selectedIndex <= 0) {
            toastr.warning(errors[1].message, errors[1].title);
            return false;
        }

        if (!batchNo.val()) {
            toastr.warning(errors[2].message, errors[2].title);
            return false;
        }
        else if (freshQty.val() <= 0) {
            toastr.warning(errors[3].message, errors[3].title);
            return false;
        }

        var formData = $("#form").serializeArray();

        $.post("/ChemicalReceive/AddOrRemoveFromReceiveDetails", formData, function (partialView) {
            attachTo.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });
});

function RemoveIndent(index) {
    swal({
        title: "Please Confirm",
        text: `You won't able to revert, Are you sure to remove item no. - ${index}`,
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

                $.post("/ChemicalReceive/AddOrRemoveFromReceiveDetails", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }
        });
}