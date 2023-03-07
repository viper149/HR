

$(function () {

    $("#create").prop('disabled', true);
    var soNo = $("#RndProductionOrder_ORDERNO").val();
    getSoDetails(soNo);
    var mid = $("#RndProductionOrder_MID").val();
    masterRollDetails(mid);

    //$.ajax({
    //    async: true,
    //    cache: false,
    //    data: $("#form").serialize(),
    //    type: "POST",
    //    url: "/RndProductionOrder/GetLotNoDetailsTable",
    //    success: function (partialView) {
    //        $("#YarnLotInfoDetails").html(partialView);
    //    },
    //    error: function () {
    //        console.log("failed to attach...");
    //    }
    //});

    $("#RndProductionOrder_ORDERNO").on("change", function () {
        var selectedVal = $(this).val();
        getSoDetails(selectedVal);
    });

    $("#RndProductionOrder_MID").on("change", function () {
        var selectedVal = $(this).val();
        masterRollDetails(selectedVal);
    });

    $("#RndProductionOrder_OTYPEID").on("change", function () {
        var selectedVal = $(this).val();
        $.ajax({
            async: true,
            cache: false,
            data: { "orderType": selectedVal },
            type: "GET",
            url: "/RndProductionOrder/GetOrderNoData",
            success: function (data) {
                if (data != null) {
                    console.log(data);
                    var odderId = $("#RndProductionOrder_ORDERNO");
                    odderId.html('');
                    odderId.append('<option value="" selected>Select Order No</option>');
                    $.each(data, function (id, option) {
                        odderId.append($('<option></option>').val(option.id).html(option.name));
                    });
                }
            },
            error: function () {
                console.log("failed to attach...");
            }
        });
    });

    $("#btnLotAdd").on("click", function () {
        $.ajax({
            async: true,
            cache: false,
            data: $("#form").serialize(),
            type: "POST",
            url: "/RndProductionOrder/AddLotNoDetailsTable",
            success: function (partialView) {
                $("#YarnLotInfoDetails").html(partialView);
            },
            error: function () {
                console.log("failed to attach...");
            }
        });
    });

    $("#btn_count").on("click", function () {
        $.ajax({
            async: true,
            cache: false,
            data: $("#form").serialize(),
            type: "POST",
            url: "/RndProductionOrder/AddRndCountName",
            success: function (data) {
                if (data) {
                    toastrNotification("Successfully Update Count Name", "success");
                    getSoDetails($("#RndProductionOrder_ORDERNO").val());
                } else {
                    toastrNotification("Failed to Update Count Name", "error");
                }
            },
            error: function () {
                console.log("failed to attach...");
            }
        });
    });
});

function removeLotFromList(index) {
    swal({
        title: "Please Confirm",
        text: "Are you sure to Remove?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {
            if (isConfirm) {
                var data = $("#form").serializeArray();
                data.push({ name: "removeIndexValue", value: index });
                $.ajax({
                    async: true,
                    cache: false,
                    data: data,
                    type: "POST",
                    url: "/RndProductionOrder/RemoveLotNoDetailsTable",
                    success: function (partialView) {
                        $("#YarnLotInfoDetails").html(partialView);
                    },
                    error: function (e) {
                        console.log(e);
                    }
                });
            }
        });



}

function masterRollDetails(mid) {
    if (mid === "") {
        return false;
    }
    var mLotNo = $("#mLotNo");
    var mSupplier = $("#mSupplier");
    $.ajax({
        async: true,
        cache: false,
        data: { "mid": mid },
        type: "GET",
        url: "/RndProductionOrder/GetMasterRollDetails",
        success: function (data) {
            //console.log(data);
            mLotNo.text(data.lot.lotno);
            mSupplier.text(data.supp.suppname);
        },
        error: function () {
            console.log("failed to attach...");
        }
    });
}

function getSoDetails(orderNo) {
    if (orderNo === "") {
        return false;
    }

    $("#create").prop('disabled', true);
    var yarnDetails = $("#YarnDetails");
    var orderId = $("#RndProductionOrder_OTYPEID").val();
    var url = "";
    var isSo = 1;

    if (orderId === "401" || orderId === "402" || orderId === "419" || orderId === "422") {
        url = "/RndProductionOrder/GetPoDetails";
        isSo = 1;
    } else {
        url = "/RndProductionOrder/GetRsDetails";
        isSo = 0;
    }
    
    $.ajax({
        async: true,
        cache: false,
        data: { "orderNO": orderNo },
        type: "GET",
        url: url,
        success: function (data) {
            console.log(data);
            if (data != null) {
                if (isSo) {
                    var yarnDataSo = setReturnDataBySo(data);
                    yarnDetails.html("");
                    yarnDetails.html(yarnDataSo);
                    $("#create").prop('disabled', false);
                } else {
                    var yarnDataRs = setReturnDataByRs(data);
                    yarnDetails.html("");
                    yarnDetails.html(yarnDataRs);
                    $("#create").prop('disabled', false);
                }
            }
        },
        error: function () {
            console.log("failed to attach...");
        }
    });
}


function setReturnDataBySo(data) {

    var piNo = $("#piNo");
    var piDate = $("#piDate");
    var validity = $("#validity");
    var delStart = $("#delStart");
    var delClose = $("#delClose");
    var orderYds = $("#RndProductionOrder_ORDER_QTY_YDS");
    var orderMtr = $("#RndProductionOrder_ORDER_QTY_MTR");
    var warpLen = $("#RndProductionOrder_WARP_LENGTH_MTR");
    var greyLen = $("#RndProductionOrder_GREY_LENGTH_MTR");
    var styleNo = $("#styleNo");
    var buyer = $("#buyer");
    var factory = $("#factory");
    var dyeing = $("#dyeing");
    var loom = $("#loom");
    var dyeingInput = $("#RndProductionOrder_DYENG_TYPE");
    var loomInput = $("#RndProductionOrder_LOOM_TYPE");
    var ball = $("#RndProductionOrder_NO_OF_BALL");
    var totalEnds = $("#RndProductionOrder_TOTAL_ENOS");
    var oldCountId = $("#UpdateCountInfoViewModel_OLD_COUNTID");
    oldCountId.html('');
    oldCountId.append('<option value="" selected>Select Old Count Name</option>');

    var shrinkage = ((100 - (((data.rndFabricInfo.fnppi - data.rndFabricInfo.grppi) / data.rndFabricInfo.fnppi) * 100)) / 100);
    var orderQtyYds;
    var orderQtyMtr;

    if (data.comExPiDetails.unit === 7) {

        orderQtyYds = data.comExPiDetails.qty;
        orderQtyMtr = orderQtyYds * 0.9144;

        orderYds.val(orderQtyYds.toFixed(2));
        orderMtr.val(orderQtyMtr.toFixed(2));
    } else {

        orderQtyMtr = data.comExPiDetails.qty;
        orderQtyYds = orderQtyMtr / 0.9144;

        orderYds.val(orderQtyYds.toFixed(2));
        orderMtr.val(orderQtyMtr.toFixed(2));
    }
    var crimp = data.rndFabricInfo.crimP_PERCENTAGE === null ? 12 : data.rndFabricInfo.crimP_PERCENTAGE;
    var crimpPercentage = (100 - crimp) / 100;
    
    var warpLength = (((orderQtyMtr / 0.97) / crimpPercentage) / ((100 - (((data.rndFabricInfo.fnppi - data.rndFabricInfo.grppi) / data.rndFabricInfo.fnppi) * 100)) / 100)).toFixed(0);
    warpLen.val(warpLength);

    var greyLength = warpLength * crimpPercentage;
    greyLen.val(greyLength.toFixed(2));

    var totalRatioWeft = 0;
    var totalRatioWarp = 0;

    piNo.text(data.comExPimaster.pino);
    piDate.text(data.comExPimaster.pidate.substring(0, 10));
    if (data.comExPimaster.validity != null) {
        validity.text(data.comExPimaster.validity.substring(0, 10));
    }
    if (data.comExPimaster.deL_START != null) {
        delStart.text(data.comExPimaster.deL_START.substring(0, 10));
    }
    if (data.comExPimaster.deL_CLOSE != null) {
        delClose.text(data.comExPimaster.deL_CLOSE.substring(0, 10));
    }

    $("#ordRpt").text(data.ordRepeat);
    styleNo.text(data.rndFabricInfo.stylE_NAME);
    factory.text(data.comExPimaster.buyer.buyeR_NAME);
    buyer.text(data.comExPimaster.buyer.buyeR_NAME);

    dyeing.text(data.rndFabricInfo.d.dtype);
    loom.text(data.rndFabricInfo.loom.looM_TYPE_NAME);

    ball.val("");
    totalEnds.val(data.rndFabricInfo.totalends);
    dyeingInput.val(data.rndFabricInfo.d.did);
    loomInput.val(data.rndFabricInfo.loom.loomid);

  
    $.each(data.rndFabricCountInfoViewModels, function (id, option) {
        if (option.rndFabricCountinfo.yarnfor === 1) {
            totalRatioWarp += parseFloat(option.rndFabricCountinfo.ratio);
        } else if (option.rndFabricCountinfo.yarnfor === 2) {
            totalRatioWeft += parseFloat(option.rndFabricCountinfo.ratio);
        }
    });
    var yarnData = "";
    console.log(data.rndFabricCountInfoViewModels);
    $.each(data.rndFabricCountInfoViewModels, function (id, option) {
        var reqKgs = 0;
        if (option.rndFabricCountinfo.yarnfor === 1) {
            reqKgs = (warpLength * data.rndFabricInfo.totalends * option.rndFabricCountinfo.ratio) / (option.rndFabricCountinfo.ne * totalRatioWarp * 768 * 2.2046);
            //reqKgs = warpLength * option.amount;

        } else if (option.rndFabricCountinfo.yarnfor === 2) {
            //reqKgs = greyLength * option.amount;
            if (data.rndFabricInfo.loom.loomid == 1) {
                reqKgs = (greyLength * (data.rndFabricInfo.reeD_SPACE + 3) * data.rndFabricInfo.grppi * option.rndFabricCountinfo.ratio) / (option.rndFabricCountinfo.ne * totalRatioWeft * 768 * 2.2046);
            }
            else if (data.rndFabricInfo.loom.loomid == 2) {
                reqKgs = (greyLength * (data.rndFabricInfo.reeD_SPACE + 6) * data.rndFabricInfo.grppi * option.rndFabricCountinfo.ratio) / (option.rndFabricCountinfo.ne * totalRatioWeft * 768 * 2.2046);
            }
        }
        var tr = "<tr>" +
            " <td> " + option.rndFabricCountinfo.count.rnD_COUNTNAME + " </td > " +
            " <td> " + option.rndFabricCountinfo.lot.lotno + " </td> " +
            " <td> " + option.rndFabricCountinfo.ratio + " </td> " +
            " <td> " + option.rndFabricCountinfo.ne + " </td> " +
            " <td> " + option.rndFabricCountinfo.yarnFor.yarnname + " </td> " +
            " <td> " + reqKgs.toFixed(2) + " </td> " +
            "</tr >";
        yarnData += tr;
        oldCountId.append($('<option></option>').val(option.rndFabricCountinfo.trnsid).html(option.rndFabricCountinfo.count.countname));
    });
    return yarnData;
}

function setReturnDataByRs(data) {
   
    var orderYds = $("#RndProductionOrder_ORDER_QTY_YDS");
    var orderMtr = $("#RndProductionOrder_ORDER_QTY_MTR");
    var warpLen = $("#RndProductionOrder_WARP_LENGTH_MTR");
    var greyLen = $("#RndProductionOrder_GREY_LENGTH_MTR");
    var styleNo = $("#styleNo");
    var buyer = $("#buyer");
    var factory = $("#factory");
    var dyeing = $("#dyeing");
    var loom = $("#loom");
    var dyeingInput = $("#RndProductionOrder_DYENG_TYPE");
    var loomInput = $("#RndProductionOrder_LOOM_TYPE");
    var ball = $("#RndProductionOrder_NO_OF_BALL");
    var totalEnds = $("#RndProductionOrder_TOTAL_ENOS");

    var orderQtyMtr = data.rndSampleInfoDyeing.lengtH_MTR/1.35;
    orderMtr.val(orderQtyMtr.toFixed(2));

    var orderQtyYds = orderQtyMtr / 0.9144;
    orderYds.val(orderQtyYds.toFixed(2));

    var warpLength = data.rndSampleInfoDyeing.lengtH_MTR;
    warpLen.val(warpLength.toFixed(2));

    var greyLength = warpLength * 0.92;
    greyLen.val(greyLength.toFixed(2));

    var totalRatioWeft = 0;
    var totalRatioWarp = 0;

    styleNo.text(data.rndSampleInfoWeaving.fabcode);

    factory.text(data.rndSampleInfoDyeing.sdrf != null ? data.rndSampleInfoDyeing.sdrf.buyer != null ? data.rndSampleInfoDyeing.sdrf.buyer.buyeR_NAME:"":"");

    factory.text(data.rndSampleInfoDyeing.sdrf != null ? data.rndSampleInfoDyeing.sdrf.buyer != null ? data.rndSampleInfoDyeing.sdrf.buyer.buyeR_NAME : "" : "");

    dyeing.text(data.rndSampleInfoDyeing.d.dtype);
    loom.text(data.rndSampleInfoDyeing.loom.looM_TYPE_NAME);

    ball.val(data.rndSampleInfoDyeing.nO_OF_ROPE);
    totalEnds.val(data.rndSampleInfoDyeing.totaL_ENDS);
    dyeingInput.val(data.rndSampleInfoDyeing.d.did);
    loomInput.val(data.rndSampleInfoDyeing.loom.loomid);

    $.each(data.rndSampleInfoDetailsList, function (id, option) {
        if (option.yarnid === 1) {
            totalRatioWarp += parseFloat(option.ratio);
        } else if (option.yarnid === 2) {
            totalRatioWeft += parseFloat(option.ratio);
        }
    });
    debugger;
    var yarnData = "";
    $.each(data.rndSampleInfoDetailsList, function (id, option) {
        var reqKgs = 0;
        if (option.yarnid === 1) {
            reqKgs = (warpLength * data.rndSampleInfoDyeing.totaL_ENDS * option.ratio) / (option.ne * totalRatioWarp * 768 * 2.2046);
        } else if (option.yarnid === 2) {
            reqKgs = (greyLength * parseFloat(data.rndSampleInfoDyeing.reeD_SPACE) * data.rndSampleInfoWeaving.gR_PPI * option.ratio) / (option.ne * totalRatioWeft * 768 * 2.2046);
        }
        var tr = "<tr>" +
            " <td> " + option.count.countname + " </td > " +
            " <td> " + option.lot.lotno + " </td> " +
            " <td> " + option.ratio + " </td> " +
            " <td> " + option.ne + " </td> " +
            " <td> " + option.yarnid + " </td> " +
            " <td> " + reqKgs.toFixed(4) + " </td> " +
            "</tr >";
        yarnData += tr;
    });
    return yarnData;
}