$(function () {

    var invDetailsQty = $("#ComImpInvdetails_QTY");
    var invDetailsRate = $("#ComImpInvdetails_RATE");
    var invDetailsAmount = $("#ComImpInvdetails_AMOUNT");
    var comImpInvoiceInfo = $("#ComImpInvoiceinfo_INVNO");
    var comImpLcDetailsLcNo = $("#ComImpInvdetails_INVNO");
    var lcId = $("#ComImpInvoiceinfo_LC_ID");
    var prodId = $("#ComImpInvdetails_PRODID");
    var chemProdId = $("#ComImpInvdetails_CHEMPRODID");
    var yarnLotId = $("#ComImpInvdetails_YARNLOTID");

    var btnAdd = $("#btnAdd");
    var customFileInput = $(".custom-file-input");

    var errors = {
        0: {
            title: "Invalid Submission.",
            message: "We can not process your request. Please try again later."
        }
    }

    prodId.select2({
        placeholder: "Select a Product Name",
        allowClear: true,
        ajax: {
            url: "/CommercialImport/GetProducts",
            data: function (params) {

                var query = {
                    search: params.term,
                    lcId: lcId.val(),
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.basProductinfos, function (item) {
                        return {
                            id: item.prodid,
                            text: item.prodname
                        };
                    })
                };
            }
        }
    });

    var removeSelectionData = function () {
        prodId.html("");
        prodId.append('<option value="" selected>Select a Product Name</option>');

        yarnLotId.html("");
        yarnLotId.append('<option value="" selected>Select a Yarn Lot Number</option>');

        chemProdId.html("");
        chemProdId.append('<option value="" selected>Select a Chemical Product Name</option>');
    }

    //lcId.on("change", function () {

    //    const formData = $("#form").serializeArray();

    //    $.post("/ComImpInvoiceInfo/GetProductInfo", formData, function (data) {

    //        if (typeof data !== "undefined") {

    //            if (Array.isArray(data.prodinfo) && data.prodinfo.length > 0) {

    //                removeSelectionData();
    //                prodId.attr('disabled', false);

    //                $.each(data.prodinfo, function (index, option) {
    //                    prodId.append($("<option>",
    //                        {
    //                            value: option.prodid,
    //                            text: option.prodname
    //                        }));
    //                });

    //                prodId.focus();
    //                yarnLotId.attr('disabled', true);
    //                chemProdId.attr('disabled', true);

    //            } else if (Array.isArray(data.yarnlotinfo) && data.yarnlotinfo.length > 0) {

    //                removeSelectionData();
    //                yarnLotId.attr('disabled', false);

    //                $.each(data.yarnlotinfo, function (index, option) {
    //                    yarnLotId.append($("<option>",
    //                        {
    //                            value: option.lotid,
    //                            text: option.lotno
    //                        }));
    //                });

    //                yarnLotId.focus();
    //                prodId.attr('disabled', true);
    //                chemProdId.attr('disabled', true);

    //            } else if (Array.isArray(data.chemstoreprodinfo) && data.chemstoreprodinfo.length > 0) {

    //                removeSelectionData();
    //                chemProdId.attr('disabled', false);

    //                $.each(data.chemstoreprodinfo, function (index, option) {
    //                    chemProdId.append($("<option>",
    //                        {
    //                            value: option.productid,
    //                            text: option.productname
    //                        }));
    //                });

    //                chemProdId.focus();
    //                prodId.attr('disabled', true);
    //                yarnLotId.attr('disabled', true);

    //            }
    //        }

    //    }).fail(function () {
    //        toastr.error(errors[0].title, errors[0].message);
    //    });
    //});


    customFileInput.on("change", function () {
        console.log($(this));
        var fileName = $(this).val().split("\\").pop();
        $(this).next(".custom-file-label").html(fileName);
    });

    // QTY CHANGE
    invDetailsQty.on("change", function () {
        StoreMultiplicationResult(invDetailsQty, invDetailsRate, invDetailsAmount);
    });

    // RATE CHNAGE
    invDetailsRate.on("change", function () {
        StoreMultiplicationResult(invDetailsQty, invDetailsRate, invDetailsAmount);
    });

    // SHOW IT TO ANOTHER FIELD
    comImpInvoiceInfo.on("change", function () {
        comImpLcDetailsLcNo.val($(this).val());
    });

    // ADD TO LIST
    btnAdd.on("click", function () {

        var fdata = new FormData();
        var formData = $("#form").serializeArray();
        var invPath = $("#INVPATH")[0].files[0];
        var blPath = $("#BLPATH")[0].files[0];

        fdata.append("INVPATH", invPath);
        fdata.append("BLPATH", blPath);

        $.each(formData, function (key, input) {
            fdata.append(input.name, input.value);
        });

        $.ajax({
            async: true,
            cache: false,
            data: fdata,
            type: "POST",
            contentType: false,
            processData: false,
            url: "/ComImpInvoiceInfo/AddComImpInvDetails",
            success: function (partialView) {
                $("#AttachToImportInvoice").html(partialView);
            },
            error: function () {
                console.log("failed to upload...");
            }
        });
    });
});

// STORE THE RESULT
function StoreMultiplicationResult(x, y, out) {
    out.val(x.val() * y.val());
}

// REMOVE FROM THE LIST
function RemoveImportInvoiceDetails(x) {

    var data = $("#form").serializeArray();

    data.push({ name: "x", value: x });

    $.ajax({
        async: true,
        cache: false,
        type: "POST",
        url: "/ComImpInvoiceInfo/RemoveImportInvoiceDetails",
        data: data,
        success: function (partialView) {
            $("#AttachToImportInvoice").html(partialView);
        },
        error: function (e) {
            console.log("failed.");
        }
    });
}