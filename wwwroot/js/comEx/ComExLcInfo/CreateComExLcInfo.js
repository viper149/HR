
var errors = {
    0: {
        title: "Invalid Submission.",
        message: "Please try again later."
    }
}
$(function () {

    $('#form').submit(function () {
        $(this).find(':submit').attr('disabled', 'disabled');
    });
});
var attachDetailsTo = $("#AttachPIOtherDetails");
var attachTo = $("#AttachPI");

$(function () {

    const btnAdd = $("#btnAdd");
    var piId = $("#ComExLcdetails_PIID");
    var piFile = $("#PiFile");
    var partyBankId = $("#ComExLcinfo_BANK_ID");
    var notifyBankId = $("#ComExLcinfo_NTFYBANKID");
    var prevYear = $("#PREV_YEAR");
    var fileNo = $(".fileNo");

    var targets = [];
    targets.push(piId);

    piId.select2({
        ajax: {
            url: "/CommercialExportLC/GetPiList",
            type: "POST",
            data: function (params) {

                var formData = $("#form").serializeArray();

                formData.push({ name: "search", value: params.term });
                formData.push({ name: "page", value: params.page || 1 });

                return formData;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.cOM_EX_PIMASTERs, function (item) {
                        return {
                            id: item.piid,
                            text: item.pino
                        };
                    })
                };
            }
        }
    });

    partyBankId.on("change", function () {
        notifyBankId.val(partyBankId.val()).trigger("change");
    });

    prevYear.on("change", function () {

        var formData = $("#form").serializeArray();

        $.post("/CommercialExportLC/GetFileNo", formData, function (data) {
            fileNo.val(data);
            fileNo.text(data);
        });
    });

    piId.on("change", function () {

        var formData = {
            "piId": $(this).val()
        }

        $.post("/ComExLcInfo/GetPiOtherDetails", formData, function (partialView) {
            attachDetailsTo.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });

    btnAdd.on("click", function () {

        if (checkErrors(targets)) {

            var gdata = new FormData();
            const formData = $("#form").serializeArray();

            gdata.append("PiFile", piFile[0].files[0]);

            $.each(formData, function (key, input) {
                gdata.append(input.name, input.value);
            });

            $.ajax({
                async: true,
                cache: false,
                type: "POST",
                url: "/ComExLcInfo/AddComExLcInfo",
                data: gdata,
                contentType: false,
                processData: false,
                success: function (partialView) {
                    attachTo.html(partialView);
                },
                error: function (err) {
                    toastr.error("Failed To Add Export LC Information.", "Error Occurred!");
                }
            });
        }
    });
});

function RemoveLCDetails(index) {

    var formData = $("#form").serializeArray();

    formData.push({ name: "RemoveIndex", value: index });
    formData.push({ name: "IsDelete", value: true });

    $.post("/ComExLcInfo/AddComExLcInfo", formData, function (partialView) {
        attachTo.html(partialView);
    }).fail(function () {
        toastr.error("Failed To Add Export LC Information.", "Error Occurred!");
    });

}


//$(function () {

//    const btnAdd = $("#btnAdd");
//    const piId = $("#comExPIViewModel_PIID");
//    const lcInfoValue = $("#cOM_EX_LCINFO_VALUE");

//    var attachTo = $("#AttachPI");
//    var total = $("#comExPIViewModel_TOTAL");
//    var lcValue = $("#comExPIViewModel_LCVALUE");
//    var piFile = $("#PiFile");
//    var attachDetailsTo = $("#AttachPIOtherDetails");

//    var partyBankId = $("#cOM_EX_LCINFO_BANK_ID");
//    var notifyBankId = $("#cOM_EX_LCINFO_NTFYBANKID");

//    var targets = [];
//    targets.push(piId);

//    partyBankId.on("change", function () {
//        notifyBankId.val(partyBankId.val()).trigger("change");
//    });

//    btnAdd.on("click", function () {

//        if (checkErrors(targets)) {

//            var gdata = new FormData();
//            const formData = $("#form").serializeArray();

//            gdata.append("PiFile", piFile[0].files[0]);

//            $.each(formData,
//                function (key, input) {
//                    gdata.append(input.name, input.value);
//                });

//            $.ajax({
//                async: true,
//                cache: false,
//                type: "POST",
//                url: "/ComExLcInfo/AddComExLcInfo",
//                data: gdata,
//                contentType: false,
//                processData: false,
//                success: function (partialView) {
//                    attachTo.html(partialView);
//                },
//                error: function (err) {
//                    toastr.error("Failed To Add Export LC Information.", "Error Occurred!");
//                }
//            });
//        }
//    });


//    $("#cOM_EX_LCINFO_LC_STATUS").on("change",
//        function () {
//            console.log($(this).val());
//        });

//    piId.on("change", function () {

//        const selectedItem = $(this).val();

//        if (selectedItem) {
//            $.ajax({
//                async: true,
//                cache: false,
//                type: "POST",
//                url: "/ComExLcInfo/GetTotalFromPIDetails",
//                data: { "piId": selectedItem },
//                success: function (data) {
//                    if (data !== null) {
//                        console.log(data);
//                        total.val(data);
//                    }
//                    else {
//                        toastr.error("Failed To Retrieve Fabric Information.", "error");
//                    }
//                },
//                error: function (err) {
//                    toastr.error("Failed To Retrieve Fabric Information.", "error");
//                }
//            }).done(function (data) {
//                $.post("/ComExLcInfo/GetPiOtherDetails", { "piId": selectedItem }, function (partialView) {
//                    attachDetailsTo.html(partialView);
//                });
//            });
//        } else {
//            resetFields({ total });
//            emptyElements({ attachDetailsTo });
//        }
//    });

//    lcInfoValue.on("change", function () {
//        lcValue.val($(this).val());
//    });
//});