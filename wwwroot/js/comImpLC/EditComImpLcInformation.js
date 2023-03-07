
var attachToImportLc = $("#AttachToImportLC");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

$(function () {

    var lcDetailsQty = $("#cOM_IMP_LCDETAILS_QTY");
    var lcDetailsRate = $("#cOM_IMP_LCDETAILS_RATE");
    var lcDetailsTotal = $("#cOM_IMP_LCDETAILS_TOTAL");
    var comImpLcInformationLcNo = $("#cOM_IMP_LCINFORMATION_LCNO");
    var comImpLcDetailsLcNo = $("#cOM_IMP_LCDETAILS_LCNO");
    var btnAdd = $("#btnAdd");
    var ltId = $("#cOM_IMP_LCINFORMATION_COM_IMP_LCTYPE_TYPENAME");

    var lienVal = $("#cOM_IMP_LCINFORMATION_LIENVAL");
    var lcDate = $("#ComExLcinfo_LCDATE");
    var exValue = $("#ComExLcinfo_VALUE");
    var amentDate = $("#ComExLcinfo_AMENTDATE");
    var amentValue = $("#ComExLcinfo_AMENTVALUE");
    var lienValueBalance = $("#cOM_IMP_LCINFORMATION_BALANCE");
    var bankPartyBank = $("#ComExLcinfo_BANK__PARTY_BANK");
    var expDate = $("#cOM_IMP_LCINFORMATION_EXPDATE");
    var previousList = $("#previousList");

    var masterLcNo = $("#masterLcNo");

    if (ltId.val() === 'BTB') {

        $(".styleDetails").removeClass('d-none');

    } else {

        $(".styleDetails").addClass("d-none");

    }

    masterLcNo.on("change", function () {
        getValueByMasterno();
    });

    function getValueByMasterno() {
        _masterLcNo = masterLcNo.val();
        _expDate = expDate.val();
        

        if (!_masterLcNo) {
            //clearFields();
            previousList.empty();
            toastrNotification("Please select master LC number.😢", "warning");

        } else if (!_expDate) {
            masterLcNo.val('');
            toastrNotification("Please select LC Expire date 😢", "warning");
        } else {
            $.ajax({
                async: true,
                cache: false,
                data: $("#form").serialize(),
                type: "POST",
                url: "/ComImpLcInformation/GetExportLcZenbuJyuhou",
                success: function (data) {
                    $.ajax({
                        async: true,
                        cache: false,
                        data: { "masterLcNo": _masterLcNo },
                        type: "GET",
                        url: "/ComImpLcInformation/LoadPreviousList",
                        success: function (partialView) {
                            $("#previousList").html(partialView);
                        },
                        error: function () {
                            console.log("An error occurred during your submission.");
                        }
                    });

                    if (data !== null) {
                        if (!data.override) {
                            lienVal.val(((data.value || 0 - data.amentvalue || 0) * 0.7).toFixed(4));
                            exValue.val(data.value || 0);
                            amentValue.val(data.amentvalue || 0);

                            if (data.banK_ !== null) {
                                bankPartyBank.val(data.banK_.partY_BANK);
                            }

                            if (data.lcdate !== null) {
                                lcDate.val(data.lcdate.substring(0, data.lcdate.indexOf('T')));
                            } else {
                                lcDate.val('');
                            }

                            if (data.amentdate !== null) {
                                amentDate.val(data.amentdate.substring(0, data.amentdate.indexOf('T')));
                            } else {
                                amentDate.val('');
                            }
                            
                            if ($("#sumOfTotal").text() !== null) {
                                lienValueBalance.val(((parseFloat(data.value || 0) * 0.7) - parseFloat($("#sumOfTotal").text())).toFixed(4));
                            }

                            toastrNotification("Procced To Work😊...", "success");

                        } else {

                            lienVal.val(((data.comExLcInfo.value ?? 0 - data.comExLcInfo.amentvalue ?? 0) * 0.7).toFixed(4));
                            exValue.val(data.comExLcInfo.value ?? 0);
                            amentValue.val(data.comExLcInfo.amentvalue ?? 0);

                            if (data.comExLcInfo.banK_ !== null) {
                                bankPartyBank.val(data.comExLcInfo.banK_.partY_BANK);
                            }

                            if (data.comExLcInfo.lcdate !== null) {
                                lcDate.val(data.comExLcInfo.lcdate.substring(0, data.comExLcInfo.lcdate.indexOf('T')));
                            } else {
                                lcDate.val('');
                            }

                            if (data.comExLcInfo.amentdate !== null) {
                                amentDate.val(data.comExLcInfo.amentdate.substring(0, data.comExLcInfo.amentdate.indexOf('T')));
                            } else {
                                amentDate.val('');
                            }

                            if ($("#sumOfTotal").text() !== null) {

                                //console.log("Previous amount: " + (isNaN(data.comExLcInfo.previousAmount) ? 0 : data.comExLcInfo.previousAmount));

                                lienValueBalance.val((((data.comExLcInfo.value || 0 - data.comExLcInfo.amentvalue || 0) * 0.7) - parseFloat($("#sumOfTotal").text()) - data.comExLcInfo.previousAmount || 0)).toFixed(4);
                            }

                            toastrNotification("Procced To Work 😉 ...", "success");

                        }
                    } else {
                        //clearFields();
                        toastrNotification("Possibility: <br />1. 90 Days Exceeded Compare To Export LC Date.<br />2. Select Import LC Date or Doesn't Have Any Export LC Date. 😢", "warning");
                    }
                },
                error: function (e) {
                    console.log("failed to add partialView...");
                }
            });
        }

    }


    

    lcDetailsQty.on("change", function () {
        storeMultiplicationResult(lcDetailsQty, lcDetailsRate, lcDetailsTotal);
    });

    lcDetailsRate.on("change", function () {
        storeMultiplicationResult(lcDetailsQty, lcDetailsRate, lcDetailsTotal);
    });

    comImpLcInformationLcNo.on("change", function () {
        comImpLcDetailsLcNo.val($(this).val());
    });

    btnAdd.on("click", function () {

        var gdata = new FormData();
        var formData = $("#form").serializeArray();
        var lcPath = $("#LCPATH")[0].files[0];
        var piPath = $("#PIPATH")[0].files[0];


        gdata.append("PIPATH", piPath);
        gdata.append("LCPATH", lcPath);

        $.each(formData, function (key, input) {
            gdata.append(input.name, input.value);
        });
        $.ajax({
            async: true,
            cache: false,
            data: gdata,
            type: "POST",
            contentType: false,
            processData: false,
            url: "/ComImpLcInformation/AddComImpLcDetails",
            success: function (partialView) {
                attachToImportLc.html(partialView);
            },
            error: function () {
                //toastr.error(errors[0].message, errors[0].title);
            }
        });
    });

    
});

/*$('#cOM_EX_PI_DETAILS_COSTID').val(data.costid).trigger("trigger");*/


function RemoveImportLCDetails(index) {

    var formData = $("#form").serializeArray();

    formData.push({ name: "RemoveIndex", value: index });
    formData.push({ name: "IsDelete", value: true });

    $.post("/ComImpLcInformation/AddComImpLcDetails", formData, function (partialView) {
        attachToImportLc.html(partialView);
    }).fail(function () {
        //toastr.error(errors[0].message, errors[0].title);
    });
}