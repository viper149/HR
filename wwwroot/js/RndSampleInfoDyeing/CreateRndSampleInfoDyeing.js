
$(function () {

    var btnAdd = $("#btnAdd");
    var btnProgAdd = $("#btnProgAdd");
    var attachTo = $("#RndSampleInfoDetails");
    var attachProgTo = $("#ProgSetInfoDetails");

    var totalEnds = $("#RndSampleInfoDyeing_TOTAL_ENDS");
    var ratio = $("#RndSampleInfoDetails_RATIO");
    var ne = $("#RndSampleInfoDetails_NE");
    var countId = $("#RndSampleInfoDetails_COUNTID");
    var colorCode = $("#RndSampleInfoDetails_COLORCODE");
    var lotId = $("#RndSampleInfoDetails_LOTID");
    var supplierId = $("#RndSampleInfoDetails_SUPPID");
    var yarnId = $("#RndSampleInfoDetails_YARNID");
    var sdrfId = $("#RndSampleInfoDyeing_SDRFID");
    var styleRef = $("#RndSampleInfoDyeing_STYLEREF");
    var rndTeam = $("#RndSampleInfoDyeing_RNDTEAM");
    var endsRope = $("#RndSampleInfoDyeing_ENDS_ROPE");
    var noOfRope = $("#RndSampleInfoDyeing_NO_OF_ROPE");

    var targets = [];

    targets.push(
        totalEnds, ratio, ne, countId, lotId,
        supplierId, yarnId
    );

    endsRope.on("change", function () {
        totalEnds.val(endsRope.val() * noOfRope.val());
    });

    noOfRope.on("change", function () {
        totalEnds.val(endsRope.val() * noOfRope.val());
    });

    sdrfId.on("change", function () {
        if (checkErrors({ sdrfId })) {
            $.ajax({
                async: true,
                cache: false,
                data: {
                    "sdrfId": sdrfId.val()
                },
                type: "GET",
                url: "/RndSampleInfoDyeing/GetTeamInfo",
                success: function (data) {
                  console.log(data);
                    resetFields({ rndTeam, styleRef });
                    removeClass({ rndTeam, styleRef });

                    if (data !== null) {
                        rndTeam.val(data.sdrf.teaM_R.teaM_NAME);
                        styleRef.val(data.sdrf.buyeR_REF);
                        addClass({ rndTeam, styleRef });
                    }
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });
        } else {
            resetFields({ rndTeam, styleRef });
            removeClass({ rndTeam, styleRef });
        }
    });

    btnAdd.on("click", function () {

        if (checkErrors(targets)) {
            $.ajax({
                async: true,
                cache: false,
                data: $("#form").serialize(),
                type: "POST",
                url: "/RndSampleInfoDyeing/AddOrRemoveRndSampleInfoDetailsTable",
                success: function (partialView) {
                    attachTo.html(partialView);
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });
        }
    });
});

function removeFromList(index) {

    var attachTo = $("#RndSampleInfoDetails");
    var fData = $("#form").serializeArray();

    fData.push({ name: "RemoveIndex", value: index });
    fData.push({ name: "IsDelete", value: true });

    $.ajax({
        async: true,
        cache: false,
        data: fData,
        type: "POST",
        url: "/RndSampleInfoDyeing/AddOrRemoveRndSampleInfoDetailsTable",
        success: function (partialView) {
            attachTo.html(partialView);
        },
        error: function () {
            console.log("failed to attach...");
        }
    });
}