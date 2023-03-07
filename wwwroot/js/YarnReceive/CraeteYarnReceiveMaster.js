
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
var yarnType = $("#FYsYarnReceiveDetail_YARN_TYPE");

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

    $.post("/YarnReceive/AddOrRemoveFromDetails", data, function (partialView) {
        attachTo.html(partialView);
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}

var ApproveYarnDetails = function (i, trnsid) {
    $.post("/FysYarnReceiveMaster/QcApprove", { "id": trnsid }, function (result) {
        if (result) {
            location.reload();
        }
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}

createMRR.on("click", function (index = 0) {

    const data = $("#form").serializeArray();

    $.post("/FysYarnReceiveMaster/CreateMrr", data, function (result) {
        $("#mrrModel").modal("hide");
        location.reload();
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
});
function GetIndDetails() {


    var formData = {
        "indId": indentId.val()
                }


    $.get("/YarnReceive/GetCountName/", formData, function (data) {

        prodId.html("");
        prodId.append('<option value="" selected>Select Count Name</option>');

        var content = "";

        if (data.length !== 0) {
            content = "<table class=\"table table-bordered table-hover\"><thead class=\"thead-light\"><tr><th scope=\"col\">#</th><th scope=\"col\">Count name</th><th scope=\"col\">Raw</th><th scope=\"col\">Slub</th><th scope=\"col\">Lot Number</th><th scope=\"col\">Order Quantity</th><th scope=\"col\">Consumption Amount</th></tr></thead><tbody>";
        }

        $.each(data, function (index, value) {

            prodId.append($("<option>",
                {
                    value: value.countid,
                    text: value.rnD_COUNTNAME
                }));

            content += `<tr><td>${index + 1}</td><td>${value.rnD_COUNTNAME}</td><td>${value.raw}</td><td>${value.slub}</td><td>${value.lotno}</td><td>${value.ordeR_QTY}</td></td><td>${value.cnsmP_AMOUNT}</td></tr>`;
        });

        if (data.length !== 0) {
            content += `<tr><td colspan=\"5\">This indent used: ${data[0].prevchallandetails.length} times</td></tr>`;
        }

        content += "</tbody></table>";
        targetDetails.html(content);

    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}

$(function () {
    if (indentId.val()) {
        GetIndDetails();
    }


    rcvQty.on("change", function () {
        challanQty.val($(this).val());
    });

    indentId.on("change", function () {

        if (indentId.val()) {
            GetIndDetails();

        } else {
            targetDetails.empty();
            toastr.warning(errors[1].message, errors[1].title);
        }
    });

   /* hideSections();*/

    //rCVTID.on("change", function () {

    //    hideSections();

        
    //});

    //function hideSections() {

    //    if (rCVTID.val()) {

    //        var value = rCVTID.val();

    //        console.log(value);

    //        if (value == 20001 || value == 20003) {
    //            $(".section").removeClass('d-none');

    //        } else {
    //            $(".section").addClass("d-none");
    //        }

    //        if (value == 20000 || value == 20002 || value == 20004) {
    //            $(".section2").removeClass('d-none');

    //        } else {
    //            $(".section2").addClass("d-none");
    //        }


    //    }
    //}

    prodId.on("change", function () {
        debugger;
        const formData = {
            "indId": indentId.val(),
            "prodId": $(this).val()
        };

        if (formData.indId && formData.prodId) {
            $.get("/YarnReceive/GetYarnFor", formData, function (data) {
                yarnType.val(data);
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[1].message, errors[1].title);
        }
    });


    getDataByInvoice();

    invoiceId.on("change", function () {

        getDataByInvoice();


    });

    

    function getDataByInvoice() {

        if (invoiceId.val()) {

            var formData = {
                "invId": invoiceId.val()
            }

        //const formData = {
        //    "invId": $(this).val()
        //};

            $.get("/YarnReceive/GetInvoiceDetails", formData, function (data) {
                lcNo.text(data.lcNo);
                supplier.text(data.supplier);
                /*rcvFrom.val(data.supplier);*/
                console.log(data);
                rcvFrom.val(data.suppid).trigger("change");
                console.log(data);
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        }

        //else {
        //    toastr.error(errors[1].message, errors[1].title);
        //}

    }

  

    btnAdd.on("click", function () {

        const data = $("#form").serializeArray();

        $.post("/YarnReceive/AddOrRemoveFromDetails", data, function (partialView) {
            attachTo.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });
});