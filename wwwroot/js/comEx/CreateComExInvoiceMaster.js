
var attachTo = $("#AttachToExportInvoice");
let fileno;

var temp;

var errors = {
    0: {
        title: "Invalid Submission",
        message: "Please try again later."
    }
}

$(function () {

    $('#form').submit(function () {
        $(this).find(':submit').attr('disabled', 'disabled');
    });
});

$(function () {

    var hQty = $("#ComExInvdetails_PREV_QTY");
    var qty = $("#ComExInvdetails_QTY");
    var rate = $("#ComExInvdetails_RATE");
    var amount = $("#ComExInvdetails_AMOUNT");
    var invNo = $("#ComExInvoicemaster_INVNO");
    var lcId = $("#ComExInvoicemaster_LCID");
    var btnAdd = $("#btnAdd");
    var buyerIdHiddenField = $("#ComExInvoicemaster_BUYERID");
    var buyerName = $("#ComExInvoicemaster_BUYER_BUYER_NAME");
    var pDocNoField = $("#ComExInvoicemaster_PDOCNO");
    var styleId = $("#ComExInvdetails_STYLEID");
    var balanceValue = $("#ComExInvdetails_BALANCEVALUE");
    var invDate = $("#ComExInvoicemaster_INVDATE");
    var invDuration = $("#ComExInvoicemaster_INVDURATION");
    var negoDate = $("#ComExInvoicemaster_NEGODATE");

    var invoiceListId = $("#InvoiceList");
    var lcValue = $("#ComExInvoicemaster_LC_VALUE");

    

    // ADD TO LIST
    btnAdd.on("click", function () {

        var temp2 = $("#ComExInvdetails_AMOUNT").val();

        console.log("temp:" + temp);

        temp = (temp - temp2).toFixed(2);

        console.log("temp2:" + temp);

        $("#remaining").text(temp);



        var formData = $("#form").serializeArray();

        $.post("/ComExInvoiceMaster/AddComExpInvDetails", formData, function (partialView) {
            attachTo.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });

    // QTY CHANGE
    qty.on("change", function () {

        if (!lcId.val()) {
            toastr.warning("Please Select a LC First", "Incomplete Input");
        } else {
            if (balanceValue.val() && rate.val()) {
                balanceValue.val(parseFloat(balanceValue.val()) - parseFloat(qty.val()) * parseFloat(rate.val()));
            }

            StoreMultiplicationResult(qty, rate, amount);
        }
    });

    // RATE CHNAGE
    rate.on("change", function () {

        if (!lcId.val()) {
            toastr.warning("Please Select a LC First", "Incomplete Input");
        } else {
            if (balanceValue.val() && qty.val()) {
                balanceValue.val(parseFloat(balanceValue.val()) - parseFloat(qty.val()) * parseFloat(rate.val()));
            }
            StoreMultiplicationResult(qty, rate, amount);
        }
    });

    invNo.on("change", function () {

        var userValue = $(this).val();

        if (userValue) {
            $.ajax({
                async: true,
                cache: false,
                data: { "lastPart": userValue },
                type: "GET",
                url: "/ComExInvoiceMaster/GetFormatForInvNo",
                success: function (data) {
                    invNo.val(data);
                }
            });
        }
    });

    invDuration.on("change keyup", function () {

        if (invDuration.val()) {
            var date = new Date(invDate.val());
            date.setDate(date.getDate() + parseInt(invDuration.val()));
            negoDate.val(date.toISOString().slice(0, 10));
        }
    });

    // SELECT BUYER AND PDOCNO 
    lcId.on("change", function () {

        var formData = {
            "lcId": lcId.val()
        }

        if (lcId.val()) {

            $.ajax({
                async: true,
                cache: false,
                data: formData,
                type: "GET",
                url: "/ComExInvoiceMaster/GetBuyerAndPDocNo",
                success: function (data) {
                    buyerIdHiddenField.val(data.buyerId);
                    buyerName.val(data.buyerName);
                    pDocNoField.val(data.pDocNo);
                    lcValue.val(data.value === null ? 0 : data.value);
                    balanceValue.val(data.value);

                    $("#value").text(data.value === null ? 0 : data.value);

                    

                    console.log(data);

                    fileno = data.fileno;
                    console.log(fileno);
                    GetAmountByLc();

                    attachTo.empty();
                }
            }).done(function () {
                $.ajax({
                    async: true,
                    cache: false,
                    data: formData,
                    type: "GET",
                    url: "/ComExInvoiceMaster/GetInvoiceList",
                    success: function (partialView) {
                        invoiceListId.html(partialView);
                        toastrNotification("We have found previous invoices against this LC.", "success");
                    }
                });
            });

            $.post("/CommercialExportInvoice/GetFabricStyles", formData, function (data) {

                styleId.html("");
                styleId.append('<option value="" selected>Select a Style Name</option>');

                $.each(data[0].coM_EX_LCDETAILS, function (id, option) {

                    $.each(option.pi.coM_EX_PI_DETAILS, function (id1, option1) {
                        styleId.append($("<option>",
                            {
                                value: option1.trnsid,
                                text: `${option.pi.pino} - ${option1.style.stylename} (${option1.style.fabcodeNavigation.stylE_NAME})`
                            }));
                    });
                });
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });


        } else {
            invoiceListId.empty();
            balanceValue.val("");
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
});

// STORE THE RESULT
function StoreMultiplicationResult(x, y, out) {
    out.val(x.val() * y.val());
}

// REMOVE FROM THE LIST
function RemoveExportInvoiceDetails(index) {

    var formData = $("#form").serializeArray();

    formData.push({ name: "RemoveIndex", value: index });
    formData.push({ name: "IsDelete", value: true });

    $.post("/ComExInvoiceMaster/AddComExpInvDetails", formData, function (partialView) {
        attachTo.html(partialView);
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}

function GetAmountByLc() {

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

                temp = remaining;

                console.log("temp:" + temp);



            };
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}