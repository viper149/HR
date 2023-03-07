
var orderId = $("#FFsFabricClearence2NdBeam_ORDERNO");
var setId = $("#FFsFabricClearence2NdBeam_SETID");
var labGId = $("#FFsFabricClearence2NdBeam_LGTEST_ID");
var beamId = $("#FFsFabricClearence2NdBeam_BEAMID");
var labBId = $("#FFsFabricClearence2NdBeam_LBTEST_ID");

var errors = {
    0: {
        title: "Invalid Submission",
        message: "We can not process your data! Please try again."
    }
}


function GetOrderDetails(ordId) {

    if (ordId) {
        $.get("/RndProductionOrder/GetPoDetails", { 'orderNo': ordId }, function (data) {

        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });

        $.get("/FYarnRequirement/GetSetList", { 'poId': ordId }, function (data) {

            setId.html("");
            setId.append('<option value="" selected>Select Set/Prog. No.</option>');

            $.each(data, function (id, option) {
                setId.append($("<option></option>").val(option.id).html(option.name));
            });

        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    } else {
        setId.html("");
        setId.append('<option value="" selected>Select Set/Prog. No.</option>');

        toastr.error(errors[0].message, errors[0].title);
    }
}

function GetGreyLabData(labId) {

    if (labId) {
        $.ajax({
            async: true,
            cache: false,
            data: { "labGId": labId },
            type: "GET",
            url: "/FFsFabricClearence2ndBeam/GetLabGDetails",
            success: function (data, status, xhr) {
                $("#actWidth").text(data.wigrbw);
                $("#actWeight").text(data.wggrbw);
                $("#actSrWarp").text(data.srgrwrap);
                $("#actSrWeft").text(data.srgrweft);
                $("#actStWarp").text(data.sgwarp);
                $("#actStWeft").text(data.sgweft);
            },
            error: function (e) {
                console.log(e);
            }
        });

    } else {

        toastr.error(errors[0].message, errors[0].title);
    }
}


function GetBulkLabData(labId) {

    if (labId) {
        $.ajax({
            async: true,
            cache: false,
            data: { "labBId": labId },
            type: "GET",
            url: "/FFsFabricClearence2ndBeam/GetLabBDetails",
            success: function (data, status, xhr) {

                $("#reqOv").text(data.widtH_OVR);
                $("#reqWi").text(data.widtH_CUT);
                $("#reqWg").text(data.weighT_UW);
                $("#reqGrAw").text(data.weighT_WASH);
                $("#reqUEpi").text(data.acuW_EPI);
                $("#reqUPpi").text(data.acuW_PPI);
                $("#reqSrWarp").text(data.shrinK_WARP);
                $("#reqSrWeft").text(data.shrinK_WEFT);
                $("#reqBs").text(data.sgweft);
                $("#reqStWarp").text(data.strwarP_QA);
                $("#reqStWeft").text(data.strwefT_QA);
                $("#reqGrWarp").text(data.growtH_WARP);
                $("#reqGrWeft").text(data.growtH_WEFT);


                $("#reqOvAw").text(data.widtH_WASH);
                $("#reqAEpi").text(data.acwasH_EPI);
                $("#reqAPpi").text(data.acwasH_PPI);
                $("#grwarp").text(data.growtH_WARP);
                $("#grWeft").text(data.growtH_WEFT);
                $("#uSekw").text(data.skeW_UW);
                $("#aSkew").text(data.skeW_WASH);
                $("#sekw").text(data.skeW_MOVE);
                $("#spiA").text(data.spiR_A);
                $("#spiB").text(data.spiR_B);


            },
            error: function (e) {
                console.log(e);
            }
        });

    } else {

        toastr.error(errors[0].message, errors[0].title);
    }
}

function GetSetDatails(setIdVal) {

    if (setIdVal) {

        $.get("/FFsFabricClearence2ndBeam/GetSetDetails", { "setId": setIdVal }, function (data) {
            debugger;
            var beamVal = $("#FFsFabricClearence2NdBeam_BEAMID").val();
            console.log(data);
            beamId.html('');
            beamId.append('<option value="" selected>Select Beam</option>');


            $.each(data.plProductionSetDistribution.f_PR_WEAVING_PROCESS_MASTER_B, function (id, option) {
                $.each(option.f_PR_WEAVING_PROCESS_BEAM_DETAILS_B, function (idd, opt) {
                    $.each(opt.f_PR_WEAVING_PROCESS_DETAILS_B, function (iddd, o) {
                        if (o.otherS_DOFF === 3) {
                            if (data.plProductionSetDistribution.subgroup.group.rnD_DYEING_TYPE.dtype === 'Rope') {
                                beamId.append($('<option></option>').val(o.trnsid).html(opt.f_PR_SIZING_PROCESS_ROPE_DETAILS.w_BEAM.beaM_NO + "/" + opt.f_PR_SIZING_PROCESS_ROPE_DETAILS.opT1 + "(" + o.lengtH_BULK + " Mtr)"));
                            }
                            else if (data.plProductionSetDistribution.subgroup.group.rnD_DYEING_TYPE.dtype === 'Slasher') {
                                beamId.append($('<option></option>').val(o.trnsid).html(opt.f_PR_SLASHER_DYEING_DETAILS.w_BEAM.beaM_NO + "/" + opt.f_PR_SLASHER_DYEING_DETAILS.opT1 + "(" + o.lengtH_BULK + " Mtr)"));
                            }
                        }
                    });
                });
            });
            if (data !== "undefined") {

                $("#style").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.stylE_NAME);
                $("#totalEnds").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.totalends);
                $("#reed").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.reeD_COUNT);
                $("#dent").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.dent);
                $("#loom").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.loom.looM_TYPE_NAME);
                $("#pi").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.pimaster.pino);
                $("#piQty").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.pimaster.pI_QTY);
                $("#ordQty").text(data.plProductionSetDistribution.opT3);
                $("#FFsFabricClearence2NdBeam_OPT2").val(data.plProductionSetDistribution.opT3);
                $("#buyer").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.pimaster.buyer.buyeR_NAME);
                $("#Brand").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.pimaster.brand.brandname);
                $("#reqWidth").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.wigrbw);
                $("#reqWeight").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.wggrbw);
                $("#reqrSrWarp").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.srgrwarp);
                $("#reqrSrWeft").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.srgrweft);
                $("#reqrStWarp").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.stgrwarp);
                $("#reqrStWeft").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.stgrweft);
                $("#process").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.finisH_ROUTE);
                $("#fnConst").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.fnepi + " X " + data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                        .fabcodeNavigation.fnppi);
                $("#fnEpiPpi").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.fnepi + " X " + data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                        .fabcodeNavigation.fnppi);
                $("#reqFnWidth").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.widec);
                $("#FFsFabricClearence2NdBeam_FN_MC_FI").val(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.rnD_FINISHMC !== null ? data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                        .fabcodeNavigation.rnD_FINISHMC.name : "");
                $("#reqFnWeight").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.wgdec);
                $("#reqFnSrWarp").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.srdecwarp);
                $("#reqFnSrWeft").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.so.style
                    .fabcodeNavigation.srdecweft);
            }

            beamId.val(beamVal).trigger("change");
        });
    } else {

        beamId.html('');
        beamId.append('<option value="" selected>Select Beam</option>');

        toastr.error(errors[0].message, errors[0].title);
    }
}

$(function () {
    orderId.on("change", function () {
        const ordId = orderId.val();
        GetOrderDetails(ordId);
    });

    labGId.on("change",
        function () {
            const labId = labGId.val();
            GetGreyLabData(labId);
        });

    labBId.on("change",
        function () {
            const labId = labBId.val();
            GetBulkLabData(labId);
        });

    setId.on("change", function () {

        const setIdVal = setId.val();
        GetSetDatails(setIdVal);
    });
    
    beamId.on("change", function () {

        var data = beamId.select2('data');

        var beamSl = data[0].text;
        beamSl = beamSl.split("/")[1];
        beamSl = beamSl.split("(")[0];

        $("#FFsFabricClearence2NdBeam_OPT1").val(beamSl);
    });
});