
$(function () {

    var amentValue = $("#cOM_EX_LCINFO_AMENTVALUE");
    var lcInfoValue = $("#cOM_EX_LCINFO_VALUE");
    const btnAdd = $("#btnAdd");
    const piId = $("#comExPIViewModel_PIID");
    var comExPiTotal = $("#comExPIViewModel_TOTAL");
    var attachDetailsTo = $("#AttachPIOtherDetails");
    var attachPi = $("#AttachPI");

    var partyBankId = $("#cOM_EX_LCINFO_BANK_ID");
    var notifyBankId = $("#cOM_EX_LCINFO_NTFYBANKID");

    amentValue.on("change", function () {
        lcInfoValue.val((parseFloat(amentValue.val()) + parseFloat(lcInfoValue.val())).toFixed(4));
        lcInfoValue.addClass("text-danger");
    });

    partyBankId.on("change", function () {
        notifyBankId.val(partyBankId.val()).trigger("change");
    });

    btnAdd.on("click", function () {

        var fdata = new FormData();
        const formData = $("#form").serializeArray();
        const udFileUpload = $("#UDFILEUPLOAD")[0].files[0];
        const upFileUpload = $("#UPFILEUPLOAD")[0].files[0];
        const costSheetFileUpload = $("#COSTSHEETFILEUPLOAD")[0].files[0];
        const piFile = $("#PiFile")[0].files[0];
        
        fdata.append("UDFILEUPLOAD", udFileUpload);
        fdata.append("UPFILEUPLOAD", upFileUpload);
        fdata.append("COSTSHEETFILEUPLOAD", costSheetFileUpload);
        fdata.append("PiFile", piFile);

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
            url: "/ComExLcInfo/AddComExLcInfo",
            success: function (partialView) {
                attachPi.html(partialView);
            },
            error: function (e) {
                console.log(e);
            }
        });
    });

    piId.on("change", function () {
        const selectedItem = $(this).val();
        $.ajax({
            async: true,
            cache: false,
            type: "POST",
            url: "/ComExLcInfo/GetTotalFromPIDetails",
            data: { "piId": selectedItem },
            success: function (data) {
                if (data !== null) {
                    comExPiTotal.val(data);
                } else {
                    toastr.error("Failed To Retrieve Fabric Information.", "error");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                toastr.error("Failed To Retrieve Fabric Information.", "error");
            }
        }).done(function () {
            $.post("/ComExLcInfo/GetPiOtherDetails", { "piId": selectedItem }, function (partialView) {
                attachDetailsTo.html(partialView);
            });
        });
    });

    lcInfoValue.on("change", function () {
        $("#comExPIViewModel_LCVALUE").val($(this).val());
    });
});

function RemoveFromExist(id1, id2) {

    $.ajax({
        async: true,
        cache: false,
        type: "POST",
        url: "/ComExLcInfo/RemoveFromExLcInfoList",
        data: { "x": id1, "y": id2 },
        success: function (partialView) {
            $("#PreviousData").html(partialView);
        },
        error: function (e) {
            console.log(e);
        }
    });
}