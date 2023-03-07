var attachTo = $("#fQaFirsstMtrAnalisysTable");
var submitBtn = $("#btn_submit");
var beamId = $("#FirstMtrAnalysisM_BEAMID");
var setId = $("#FirstMtrAnalysisM_SETID");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

$(function () {
    var btnAdd = $("#btnAdd");

    getBySet();
    getByBeam();

    setId.on("change", function () {
        getBySet();
    });

    beamId.on("change", function () {
        getByBeam();
    });


    btnAdd.on("click",
        function () {
            var formData = $("#form").serializeArray();

            $.post("/FirstMeterAnalysis/AddToList", formData, function (partialView) {
                attachTo.html(partialView);
            }).fail(function () {
                toastr.error(errors[0].title, errors[0].message);
            });
        });
});

function getBySet() {
    if (setId.val()) {
        var formData = {
            "setId": setId.val()
        }

        $.get("/FirstMeterAnalysis/GetByAllSet", formData, function (data) {
            //$.each(data.f_PR_WEAVING_PROCESS_MASTER_B, function (iddd, o) {
            //    $.each(o.f_PR_WEAVING_PROCESS_BEAM_DETAILS_B, function (index, option) {
            //        beamId.append($("<option>",
            //            {
            //                value: option.wV_BEAMID,
            //                text: option.f_PR_SIZING_PROCESS_ROPE_DETAILS.w_BEAM.beaM_NO
            //            }));
            //    });
            //});

            $("#setQty").val(data.proG_.seT_QTY);
            $("#orderQty").val(data.proG_.blK_PROG_.rndProductionOrder.ordeR_QTY_YDS);
            $("#orderNo").val(data.proG_.blK_PROG_.rndProductionOrder.so.sO_NO);
            $("#gEPI").val(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.grepi);
            $("#gPPI").val(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.grppi);
            $("#style").val(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.stylE_NAME);
            $("#piNo").val(data.proG_.blK_PROG_.rndProductionOrder.so.pimaster.pino);
            $("#reqWidth").val(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.wigrbw);
            $("#reqWeight").val(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.wggrbw);
            $("#buyer").val(data.proG_.blK_PROG_.rndProductionOrder.so.pimaster.buyer.buyeR_NAME);
            $("#brand").val(data.proG_.blK_PROG_.rndProductionOrder.so.pimaster.brand.brandname);
            $("#totalEnds").val(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.totalends);
            $("#stdReed").val(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.reeD_COUNT);
            $("#stdDent").val(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.dent);
            $("#stdReedSpace").val(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.reeD_SPACE);
            $("#fWeave").val(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.rnD_WEAVE.name);
            $("#fColor").val(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.colorcodeNavigation.color);
            $("#constGrey").val(`${data.opT1} X ${data.opT2} / ${data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.grepi} X ${data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.grppi}`);
            $("#refLotWeft").val(data.proG_.opT1);
            $("#refSuppWeft").val(data.proG_.opT2);
            $("#rrWeft").val(data.proG_.opT3);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

function getByBeam() {
    if (beamId.val()) {

        var formData = {
            "beamId": beamId.val()
        }

        $.get("/FirstMeterAnalysis/GetAllByBeam", formData, function (data) {
            $("#lengthBeam").val(data.beaM_LENGTH);
            $("#lengthTrial").val(data.f_PR_WEAVING_PROCESS_DETAILS_B[0].lengtH_BULK);
            $("#loomNo").val(data.f_PR_WEAVING_PROCESS_DETAILS_B[0].looM_NONavigation.looM_NO);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

function RemoveDetails(index) {
    swal({
        title: "Please Confirm",
        val: `You won't able to revert, Are you sure to remove item no. - ${index + 1}`,
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {
            if (isConfirm) {

                const formData = $("#form").serializeArray();
                formData.push({ name: "RemoveIndex", value: index });
                formData.push({ name: "IsDelete", value: true });

                $.post("/FirstMeterAnalysis/AddToList", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }
        });
}