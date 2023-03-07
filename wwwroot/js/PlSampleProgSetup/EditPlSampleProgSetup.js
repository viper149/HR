
function GetLastSetNo() {
    $.ajax({
        async: true,
        cache: false,
        data: $('#form').serialize(),
        type: "POST",
        url: "/PlSampleProgSetup/GetLastSetNo",
        success: function (data) {
            if (data === undefined) {
                $("#lastSetNoS").text("");
                toastrNotification("Error: Invalid Response", "error");
            } else {
                $("#lastSetNoS").text(data);
            }
        },
        error: function () {
            $("#lastSetNoS").text("");
            toastrNotification("Error: Please Enter valid Set No.", "error");
        }
    });
}


$(function () {
    getSoDetails($("#PlBulkProgSetupM_ORDERNO").val());
    AddProgramSetNoDetails();

    $("#PlBulkProgSetupM_ORDERNO").on("change", function () {
        const selectedVal = $(this).val();
        getSoDetails(selectedVal);
    });

    $("#PlBulkProgSetupD_SET_QTY").on("change", function () {
        const selectedVal = $(this).val();
        if (selectedVal <= 0) {
            toastrNotification("Error: Set Qty can not be less then or equal 0.", "error");
            $(this).val(0);
            return false;
        }
        return false;
    });


    $("#PlSampleProgSetupD_PROG_NO").on("keyup", function () {
        const insertedItem = $(this).val();

        if (insertedItem.length === 4) {
            GetLastSetNo();
        }
    });

    $("#PlSampleProgYarnD_LOTID").on("change", function () {
        const selectedVal = $(this).val();
        if (selectedVal === "") {
            $("#supplier").text("");
            //$("#yarnFor").text("");
            return false;
        }
        $.ajax({
            async: true,
            cache: false,
            data: { "lotId": selectedVal },
            type: "GET",
            url: "/PlSampleProgSetup/GetLotDetails",
            success: function (data) {
                console.log(data);
                if (data === undefined) {
                    $("#supplier").text("");
                    //$("#yarnFor").text("");
                    toastrNotification("Error: Please Select valid Lot No.", "error");
                } else {
                    $("#supplier").text(data.brand);
                    //$("#yarnFor").text(data.yarnfor.yarnname);
                }
            },
            error: function () {
                $("#supplier").text("");
                //$("#yarnFor").text("");
                toastrNotification("Error: Please Enter valid Set No.", "error");
            }
        });
    });


    $("#PlBulkProgYarnD_COUNTID").on("change", function () {
        const selectedVal = $(this).val();
        if (selectedVal === "") {
            $("#yarnFor").text("");
            return false;
        }
        $.ajax({
            async: true,
            cache: false,
            data: { "countId": selectedVal },
            type: "GET",
            url: "/PlSampleProgSetup/GetCountDetails",
            success: function (data) {

                console.log(data);
                if (data === undefined) {
                    $("#yarnFor").text("");
                    toastrNotification("Error: Please Select valid Count", "error");
                } else {
                    $("#yarnFor").text(data.yarnFor.yarnname);
                }
            },
            error: function () {
                $("#yarnFor").text("");
                toastrNotification("Error: Please Enter valid Set No.", "error");
            }
        });
    });

    $("#PlBulkProgSetupD_PROG_NO").on("change", function () {

        const insertedItem = $(this).val();

        $.ajax({
            async: true,
            cache: false,
            data: { "programNo": insertedItem },
            type: "GET",
            url: "/RndSampleInfoDyeing/GetProgramDetails",
            success: function (data) {
                console.log(data);
                if (data === undefined || data === null) {
                    $("#PlBulkProgSetupD_PROCESS_TYPE").val("");
                    $("#PlBulkProgSetupD_WARP_TYPE").val("");
                    $("#PlBulkProgSetupD_PROGRAM_TYPE").val("");
                    toastrNotification("Error: Please Enter valid Set No.", "error");
                } else {
                    $("#PlBulkProgSetupD_PROCESS_TYPE").val(data.procesS_TYPE);
                    $("#PlBulkProgSetupD_WARP_TYPE").val(data.warP_TYPE);
                    $("#PlBulkProgSetupD_PROGRAM_TYPE").val(data.type);
                }
            },
            error: function () {
                $("#PlBulkProgSetupD_PROCESS_TYPE").val("");
                $("#PlBulkProgSetupD_WARP_TYPE").val("");
                $("#PlBulkProgSetupD_PROGRAM_TYPE").val("");
                toastrNotification("Error: Please Enter valid Set No.", "error");
            }
        });
    });

    $("#btnAdd").on("click", function () {
        AddProgramSetNoDetails();
    });
});

function AddProgramSetNoDetails() {

    var formData = $("#form").serializeArray();

    $.post("/PlSampleProgSetup/AddProgramSetNoDetails", formData, function (partialView, status, xhr) {
        setQty(xhr);
        $("#ProgramSetDetails").html(partialView);
    }).fail(function () {

    });
}

function setQty(xhr) {
    const pending = xhr.getResponseHeader("Pending");
    const production = xhr.getResponseHeader("Production");

    $("#pend").text(pending).trigger("change");
    $("#prod").text(production);

    if ($("#pend").text() < -50000) {
        $("#pending").addClass("text-danger");
        $("#btn_submit").prop("disabled", true);
    } else {
        $("#pending").removeClass("text-danger");
        $("#btn_submit").prop("disabled", false);
    }
}

function getSoDetails(orderNo) {
    if (orderNo === "") {
        return false;
    }

    const url = "/PlSampleProgSetup/GetRndPoDetails";

    $.ajax({
        async: true,
        cache: false,
        data: { "orderNO": orderNo },
        type: "GET",
        url: url,
        success: function (data) {
            console.log(data);
            if (data != null) {
                setReturnDataBySo(data);
            }
        },
        error: function () {
            console.log("failed to attach...");
        }
    });

    return false;
}

function setReturnDataBySo(data) {
    const piNo = $("#piNo");
    const delStart = $("#delStart");
    const delClose = $("#delClose");
    const orderYds = $("#ordrYds");
    const orderMtr = $("#ordrMtr");
    const warpLen = $("#PlBulkProgSetupM_WARP_QTY");
    const greyLen = $("#greyQty");
    const styleNo = $("#styleNo");
    const buyer = $("#buyer");
    var countId = $("#PlBulkProgYarnD_COUNTID");
    var lotId = $("#PlBulkProgYarnD_LOTID");

    orderYds.text(Math.ceil(data.rndProductionOrder.ordeR_QTY_YDS.toFixed(2)));

    orderMtr.text(Math.ceil(data.rndProductionOrder.ordeR_QTY_MTR.toFixed(2)));

    warpLen.val(Math.ceil(data.rndProductionOrder.warP_LENGTH_MTR.toFixed(2)));

    greyLen.text(Math.ceil(data.rndProductionOrder.greY_LENGTH_MTR.toFixed(2)));
    //$("#prod").text(Math.ceil(orderQtyMtr.toFixed(2)));

    $("#pend").text(Math.ceil(data.rndProductionOrder.warP_LENGTH_MTR.toFixed(2)));

    piNo.text(data.comExPiDetails.pino);
    if (data.comExPimaster.deL_START != null) {
        delStart.text(data.comExPimaster.deL_START.substring(0, 10));
    }
    if (data.comExPimaster.deL_CLOSE != null) {
        delClose.text(data.comExPimaster.deL_CLOSE.substring(0, 10));
    }
    if (data.rndFabricInfo == null) {
        styleNo.text(data.rndFabricInfo.stylE_NAME);
    }
    if (data.comExPimaster == null) {
        buyer.text(data.comExPimaster.buyer.buyeR_NAME);
    }

    //countId.html("");
    //countId.append('<option value="" selected>Select Count</option>');
    //$.each(data.rndFabricCountInfos, function (id, option) {
    //    countId.append($("<option></option>").val(option.trnsid).html(option.count.rnD_COUNTNAME));
    //});

    //lotId.html("");
    //lotId.append('<option value="" selected>Select Lot</option>');
    //$.each(data.basYarnLotInfos, function (id, option) {
    //    lotId.append($("<option></option>").val(option.lotid).html(option.lotno));
    //});
}