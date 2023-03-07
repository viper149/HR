
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
    var comImpLcDetailsLcNo = $("#lcNo");
    var btnAdd = $("#btnAdd");
    var catId = $("#cOM_IMP_LCINFORMATION_CATID");
    var ltId = $("#cOM_IMP_LCINFORMATION_LTID");
    var includeInStepper = $("#includeInStepper");
    var parentContainer = $("#parentContainer");

    var masterLcNo = $("#masterLcNo");
    var lienVal = $("#cOM_IMP_LCINFORMATION_LIENVAL");
    var lcDate = $("#ComExLcinfo_LCDATE");
    var exValue = $("#ComExLcinfo_VALUE");
    var amentDate = $("#ComExLcinfo_AMENTDATE");
    var amentValue = $("#ComExLcinfo_AMENTVALUE");
    var lienValueBalance = $("#cOM_IMP_LCINFORMATION_BALANCE");
    var bankPartyBank = $("#ComExLcinfo_BANK__PARTY_BANK");
    var expDate = $("#cOM_IMP_LCINFORMATION_EXPDATE");
    var previousList = $("#previousList");




    ltId.on("change", function () {

        //var _ltId = ltId.val();
        //var formData = $("#form").serializeArray();
        //formData.push({ name: "lcType", value: _ltId });

        var gdata = new FormData();
        var formData = $("#form").serializeArray();

        $.each(formData,
            function (key, input) {
                gdata.append(input.name, input.value);
            });

        if (ltId.find(":selected").text() === "BTB") {

            $(".styleDetails").removeClass('d-none');
           
        } else {

            $(".styleDetails").addClass("d-none");
            
        }
    });


    masterLcNo.on("change", function () {
        _masterLcNo = masterLcNo.val();
        _expDate = expDate.val();

        if (!_masterLcNo) {

            clearFields();
            previousList.empty();
            toastrNotification("Please select master LC number. 😢", "warning");

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
                            lienVal.val(((data.value - data.amentvalue) * 0.7).toFixed(4));
                            exValue.val(data.value);
                            amentValue.val(data.amentvalue);

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
                                lienValueBalance.val(((data.value * 0.7) - $("#sumOfTotal").text()).toFixed(4));
                            }

                            toastrNotification("Procced To Work...", "success");

                        } else {

                            console.log(data);

                            lienVal.val(((data.comExLcInfo.value - data.comExLcInfo.amentvalue) * 0.7).toFixed(4));
                            exValue.val(data.comExLcInfo.value);
                            amentValue.val(data.comExLcInfo.amentvalue);

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

                                lienValueBalance.val((((data.comExLcInfo.value - data.comExLcInfo.amentvalue) * 0.7) - $("#sumOfTotal").text() - (isNaN(data.comExLcInfo.previousAmount) ? 0 : data.comExLcInfo.previousAmount)).toFixed(4));
                            }

                            toastrNotification("Procced To Work...😊", "success");

                        }
                    } else {
                        clearFields();
                        toastrNotification("Possibility: <br />1. 90 Days Exceeded Compare To Export LC Date.<br />2. Select Import LC Date or Doesn't Have Any Export LC Date. 😢", "warning");
                    }
                },
                error: function (e) {
                    console.log("failed to add partialView...");
                }
            });
        }

    });






    //catId.on("change", function () {

    //    var _catId = catId.val();

    //    if (!_catId) {
    //        toastrNotification("Please choose at least one from this list to add File Number.", "warning");
    //    } else {
    //        $.ajax({
    //            async: true,
    //            cache: false,
    //            data: { "categoryId": _catId },
    //            type: "GET",
    //            url: "/ComImpLcInformation/GetFileNumberByCategoryId",
    //            success: function (data) {

    //                $("#cOM_IMP_LCINFORMATION_FILENO").val(data);
    //            },
    //            error: function (e) {
    //                console.log("failed to add...");
    //            }
    //        });
    //    }
    //});

    lcDetailsQty.on("change",
        function () {
            StoreMultiplicationResult(lcDetailsQty, lcDetailsRate, lcDetailsTotal);
        });

    lcDetailsRate.on("change",
        function () {
            StoreMultiplicationResult(lcDetailsQty, lcDetailsRate, lcDetailsTotal);
        });

    comImpLcInformationLcNo.keyup(function () {
        comImpLcDetailsLcNo.html($(this).val());
    });

    btnAdd.on("click",
        function () {

            var fdata = new FormData();
            var formData = $("#form").serializeArray();
            var lcPath = $("#LCPATH")[0].files[0];
            var piPath = $("#PIPATH")[0].files[0];

            fdata.append("PIPATH", piPath);
            fdata.append("LCPATH", lcPath);

            $.each(formData,
                function (key, input) {
                    fdata.append(input.name, input.value);
                });

            $.ajax({
                async: true,
                cache: false,
                data: fdata,
                type: "POST",
                contentType: false,
                processData: false,
                url: "/ComImpLcInformation/AddComImpLcDetails",
                success: function (partialView) {
                    attachToImportLc.html(partialView);
                },
                error: function () {
                    toastr.error(errors[0].message, errors[0].title);
                }
            });
        });
});




function StoreMultiplicationResult(x, y, out) {
    out.val(x.val() * y.val());
}

function RemoveImportLCDetails(index) {

    const formData = $("#form").serializeArray();

    formData.push({ name: "RemoveIndex", value: index });
    formData.push({ name: "IsDelete", value: true });

    $.post("/ComImpLcInformation/AddComImpLcDetails", formData, function (partialView) {
        attachToImportLc.html(partialView);
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}