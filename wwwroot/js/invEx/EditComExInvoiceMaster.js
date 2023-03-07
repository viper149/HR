var comExDtTrnsId = $("#ComExInvdetails_TRNSID");
var errors = {
    0: {
        title: "Invalid Submission",
        message: "Please try again later."
    },
    0: {
        title: "Invoice Details Not Found!!",
        message: "Please try again later."
    }
}

$(function () {

    $('#form').submit(function () {
        $(this).find(':submit').attr('disabled', 'disabled');
    });
}); balance


/*var lcno = "086222043422";*/

$(function () {

    
    var hQty = $("#ComExInvdetails_PREV_QTY");
    var qty = $("#ComExInvdetails_QTY");
    var rate = $("#ComExInvdetails_RATE");
    var amount = $("#ComExInvdetails_AMOUNT");
    var btnAdd = $("#btnAdd");
    var bnkAccDate = $("#ComExInvoicemaster_BNK_ACC_DATE");
    var matuDate = $("#ComExInvoicemaster_MATUDATE");
    var invIdHiddenEncrypted = $("#ComExInvoicemaster_EncryptedId");
    var baDifference = $("#BADIFFERENCE");
    var invDuration = $("#ComExInvoicemaster_INVDURATION");
    var negoDate = $("#ComExInvoicemaster_NEGODATE");
    var lcId = $("#ComExInvoicemaster_LCID");
    var styleId = $("#ComExInvdetails_PIIDD_TRNSID");
    var prcAmount = $("#ComExInvoicemaster_PRCAMOUNT");
    var amount_euro = $("#amount_euro");

    prcAmount.on("change", function () {
        //var remaining = $("#remaining").html();
        debugger;
        $("#amount_doc_value").val($("#tot_inv_value").html());
        var doc_val=$("#amount_doc_value").val();
        //remaining = parseFloat(remaining.replace(/,/g, ''));
        var balance = prcAmount.val() - doc_val;
        $("#balance").val(balance.toFixed(2));
    });

    amount_euro.on("change", function () {
        $("#amount_doc_value").val($("#document_value").html());
        var doc_val=$("#amount_doc_value").val();
        var balance = doc_val - amount_euro.val();
        $("#balance").val(balance.toFixed(2));
    });
    bnkAccDate.on("change", function () {

        var _bnkAccDate = bnkAccDate.val();
        var _matuDate = matuDate.val();
        var _invIdHiddenEncrypted = invIdHiddenEncrypted.val();

        if (_bnkAccDate && _matuDate) {
            $.ajax({
                async: true,
                cache: false,
                data: {
                    "BnkAccDate": _bnkAccDate,
                    "MatuDate": _matuDate,
                    "InvId": _invIdHiddenEncrypted
                },
                type: "GET",
                url: "/ComExInvoiceMaster/GetBaDifference",
                success: function (data) {
                    console.log(data);
                    if (data !== null) {
                        baDifference.val(data._BaDifference);
                    }
                },
                error: function () {
                    console.log("failed to load...");
                }
            });
        }
    });

    matuDate.on("change", function () {

        var _bnkAccDate = bnkAccDate.val();
        var _matuDate = matuDate.val();
        var _invIdHiddenEncrypted = invIdHiddenEncrypted.val();

        if (_bnkAccDate && _matuDate) {
            $.ajax({
                async: true,
                cache: false,
                data: {
                    "BnkAccDate": _bnkAccDate,
                    "MatuDate": _matuDate,
                    "InvId": _invIdHiddenEncrypted
                },
                type: "GET",
                url: "/ComExInvoiceMaster/GetBaDifference",
                success: function (data) {
                    console.log(data);
                    if (data !== null) {
                        baDifference.val(data._BaDifference);
                    }
                },
                error: function () {
                    console.log("failed to load...");
                }
            });
        }
    });

    // ADD TO LIST
    btnAdd.on("click", function () {

        var formData = $("#form").serializeArray();

        $.post("/ComExInvoiceMaster/AddComExpInvDetails", formData, function (partialView) {
            $("#AttachToExportInvoice").html(partialView);
            $("#amount_doc_value").val($("#document_value").html());
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });

    invDuration.on("change keyup", function () {

        if (invDuration.val()) {
            var date = new Date();
            date.setDate(date.getDate() + parseInt(invDuration.val()));
            negoDate.val(date.toISOString().slice(0, 10));
        }
    });

    styleId.on("change", function () {

        var formData = {
            "trnsId": styleId.val(),
            "lcId": lcId.val()
        }

        if (styleId.val()) {
            $.post("/CommercialExportInvoice/GetInvBalance", formData, function (data2) {
                hQty.val(parseFloat(data2));
                //console.log("1");
                $.post("/CommercialExportInvoice/GetStyleInfo", formData, function (data) {
                    qty.val(data.qty - hQty.val());
                    rate.val(data.unitprice);
                    amount.val((qty.val() * rate.val()).toFixed(2));
                    //console.log("2");
                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });

    // QTY CHANGE
    qty.on("change", function () {
        StoreMultiplicationResult(qty, rate, amount);
    });

    // RATE CHNAGE
    rate.on("change", function () {
        StoreMultiplicationResult(qty, rate, amount);
    });

    GetAmountByLc();

});


function GetAmountByLc() {

   /* $("#remaining").text("kkkk");*/

    var fileno = $("#fileno").text();

    console.log(fileno);

   


    if (fileno) {

        var formData = {
            "id": fileno
        }

        $.get("/CommercialExportInvoice/GetInvoicAmount", formData, function (data) {

            console.log(data);

            let inv_amount = 0;

            let total = 0;

            let remaining = 0;



            if (data != null) {

                total = data.value;

                $.each(data.coM_EX_INVOICEMASTER,
                    function (index, value) {

                        inv_amount += value.inV_AMOUNT;

                    }

                )

                remaining = total - inv_amount;

                console.log(total);

                console.log(inv_amount);

                console.log(remaining);

                $("#remaining").text(remaining);
            };





        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

 




// STORE THE RESULT
function StoreMultiplicationResult(x, y, out) {
    out.val((x.val() * y.val()).toFixed(2));
}

//Edit Partial VIew
function EditInvInfo(trnsId) {

    $.ajax({
        async: true,
        cache: false,
        type: "GET",
        data: { trnsId: trnsId },
        url: '/ComExInvoiceMaster/GetSingleInvDetails',
        success: function (data) {
            if (data) {

                console.log(data);
                comExDtTrnsId.val(data.trnsid);

                $('#ComExInvdetails_PIIDD_TRNSID').val(data.piDetails.trnsid).trigger("change");
                $('#ComExInvdetails_QTY').val(data.qty).trigger("change");
                $('#ComExInvdetails_RATE').val(data.rate).trigger("trigger");
                $('#ComExInvdetails_AMOUNT').val(data.amount);
                $('#ComExInvdetails_ROLL').val(data.roll);
                $('#ComExInvdetails_REMARKS').val(data.remarks);
            } else {
                toastr.error(errors[1].message, errors[0].title);
            }
        },
        error: function (e) {
            toastr.error(errors[1].message, errors[1].title);
        }
    });
};

// REMOVE FROM THE LIST
function RemoveExportInvoiceDetails(x) {

    var data = $("#form").serializeArray();
    data.push({ name: "RemoveIndex", value: x });

    $.ajax({
        async: true,
        cache: false,
        type: "POST",
        url: "/ComExInvoiceMaster/RemoveExportInvoiceDetails",
        data: data,
        success: function (partialView) {
            $("#AttachToExportInvoice").html(partialView);
        },
        error: function (e) {
            toastr.error(errors[0].message, errors[0].title);
        }
    });
}