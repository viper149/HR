
var yRCVDATE = $("#FYsYarnReceiveMaster_YRCVDATE");
var indentId = $("#FYsYarnReceiveMaster_INDID");
var rCVTID = $("#FYsYarnReceiveMaster_RCVTID");
var invoiceId = $("#FYsYarnReceiveMaster_INVID");
var cHALLANNO = $("#FYsYarnReceiveMaster_CHALLANNO");
var cHALLANDATE = $("#FYsYarnReceiveMaster_CHALLANDATE");
var tUCK_NO = $("#FYsYarnReceiveMaster_TUCK_NO");
var iSRETURNABLE = $("#FYsYarnReceiveMaster_ISRETURNABLE");
var cOMMENTS = $("#FYsYarnReceiveMaster_COMMENTS");
var g_ENTRY_NO = $("#FYsYarnReceiveMaster_G_ENTRY_NO");
var oRDER_NO = $("#FYsYarnReceiveMaster_ORDER_NO");
var rEMARKS = $("#FYsYarnReceiveMaster_REMARKS");
var prodId = $("#FYsYarnReceiveDetail_PRODID");
var challanQty = $("#FYsYarnReceiveDetail_INV_QTY");
var rcvQty = $("#FYsYarnReceiveDetail_RCV_QTY");
var attachTo = $("#yarnReceiveDetailsTableRetrieve");
var btnAdd = $("#addToUpdate");
var createMRR = $(".createButton");
var targetDetails = $("#TargetDetails");
var lcNo = $("#lcNO");
var supplier = $("#Supplier");
var rcvFrom = $("#FYsYarnReceiveMaster_RCVFROM");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your data! Please try again."
    },
    1: {
        title: "Empty Input.",
        message: "Please select a valid option."
    }
}

var RemoveFromList = function (index) {

    const data = $("#form").serializeArray();

    data.push({ name: "RemoveIndex", value: index });
    data.push({ name: "IsDelete", value: true });

    $.post("/YarnReceiveS/AddToUpdateList", data, function (partialView) {
        attachTo.html(partialView);
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}

var ApproveYarnDetails = function (i, trnsid) {

    $.post("/YarnReceiveS/QcApprove", { "id": trnsid }, function (result) {
        if (result) {
            location.reload();
        }
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}

createMRR.on("click", function (index = 0) {

    const data = $("#form").serializeArray();

    $.post("/YarnReceiveS/CreateMrr", data, function (result) {
        $("#mrrModel").modal("hide");
        location.reload();
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
});

$(function () {

    challanQty.on("change", function () {
        rcvQty.val($(this).val());
    });

    indentId.on("change", function () {
        const formData = {
            "indId": indentId.val()
        };

        if (indentId.val()) {

            $.get("/YarnReceiveS/GetCountName", formData, function (data) {

                var content = "";

                if (data.length !== 0) {
                    content = "<table class=\"table table-bordered table-hover\"><thead class=\"thead-light\"><tr><th scope=\"col\">#</th><th scope=\"col\">Count name</th><th scope=\"col\">Raw</th><th scope=\"col\">Slub</th><th scope=\"col\">Lot Number</th><th scope=\"col\">Order Quantity</th><th scope=\"col\">Consumption Amount</th></tr></thead><tbody>";
                }

                $.each(data, function (index, value) {
                    content += `<tr><td>${index + 1}</td><td>${value.countname}</td><td>${value.raw}</td><td>${value.slub}</td><td>${value.lotno}</td><td>${value.ordeR_QTY}</td></td><td>${value.cnsmP_AMOUNT}</td></tr>`;
                });

                if (data.length !== 0) {
                    content += `<tr><td colspan=\"5\">This indent used: ${data[0].prevchallandetails.length} times</td></tr>`;
                }

                content += "</tbody></table>";
                targetDetails.html(content);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });

        } else {
            targetDetails.empty();
            toastr.warning(errors[1].message, errors[1].title);
        }
    });

    invoiceId.on("change", function () {

        const formData = {
            "invId": $(this).val()
        };

        if (formData.invId) {
            $.get("/YarnReceiveS/GetInvoiceDetails", formData, function (data) {

                lcNo.text(data.lcNo);
                supplier.text(data.supplier);
                rcvFrom.val(data.supplier);
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[1].message, errors[1].title);
        }

    });

    btnAdd.on("click", function () {

        const data = $("#form").serializeArray();

        $.post("/YarnReceiveS/AddOrRemoveFromDetails", data, function (partialView) {
            attachTo.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });
});