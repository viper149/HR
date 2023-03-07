
var loomId = $("#RndSampleInfoWeaving_LOOMID");
var btnAdd = $("#btnAdd");
var attachTo = $("#RndSampleInfoWeavingDetails");
var reedSpace = $("#RndSampleInfoWeaving_REED_SPACE");
var totalEnds = $("#RndSampleInfoWeaving_TOTAL_ENDS");
var reedCount = $("#RndSampleInfoWeaving_REED_COUNT");
var reedDent = $("#RndSampleInfoWeaving_REED_DENT");
var fnPpi = $("#RndSampleInfoWeaving_FNPPI");
var countId = $("#RndSampleInfoWeavingDetails_COUNTID");
var colorCode = $("#RndSampleInfoWeavingDetails_COLORCODE");
var lotId = $("#RndSampleInfoWeavingDetails_LOTID");
var supplierId = $("#RndSampleInfoWeavingDetails_SUPPID");
var yarnId = $("#RndSampleInfoWeavingDetails_YARNID");
var ratio = $("#RndSampleInfoWeavingDetails_RATIO");
var ne = $("#RndSampleInfoWeavingDetails_NE");
var progId = $("#RndSampleInfoWeaving_PROG_ID");
var setNo = $("#RndSampleInfoWeaving_SETNO");
var beamNo = $("#RndSampleInfoWeaving_BEAMID");
var sBeamNo = $("#RndSampleInfoWeaving_SBEAMID");
var grEpi = $("#RndSampleInfoWeaving_GR_EPI");

function getReedSpace(x, y) {
    return x / y;
}

function getPickleLength(x, y) {
    if (loomId.val() === 1) {
        return x / y + 3;
    } else {
        return x / y + 5;
    }
}


$(function () {

    $("#sBeam").hide();
    $("#beam").hide();


    var targets = [];

    targets.push(reedSpace, loomId, countId, lotId,
        supplierId, yarnId, ratio, ne);

    btnAdd.on("click", function () {
        if (checkErrors(targets)) {
            $.ajax({
                async: true,
                cache: false,
                data: $("#form").serialize(),
                type: "POST",
                url: "/RndSampleInfoWeaving/AddRndSampleInfoWeavingTable",
                success: function (partialView) {
                    attachTo.html(partialView);
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });
        }
    });


    totalEnds.on("change", function () {
        var _totalEnds = totalEnds.val();
        var _grepi = (reedCount.val() * reedDent.val()) / 39.37;

        if (_totalEnds && _grepi) {
            reedSpace.val(getReedSpace(_totalEnds, _grepi).toFixed(2));
            grEpi.val(_grepi.toFixed(0));
            //pickleLength.val(getPickleLength(_totalEnds, _grepi).toFixed(2));
        }
    });

    grEpi.on("change", function () {
        var _totalEnds = totalEnds.val();
        var _grepi = (reedCount.val() * reedDent.val()) / 39.37;

        if (_totalEnds && _grepi) {
            reedSpace.val(getReedSpace(_totalEnds, _grepi).toFixed(2));
            //pickleLength.val(getPickleLength(_totalEnds, _grepi).toFixed(2));
        }
    });


    progId.on("change", function () {

        if (progId.val()) {
            $.ajax({
                async: true,
                cache: false,
                data: { "sdId": $(this).val() },
                type: "GET",
                url: "/RndSampleInfoWeaving/LoadDetails",
                success: function (partialView) {
                    $("#PreviousData").html(partialView);
                    //toastr.success("Preview pane added below.", "Found!!");
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });

            $.ajax({
                async: true,
                cache: false,
                data: { "sdId": $(this).val() },
                type: "GET",
                url: "/RndSampleInfoWeaving/LoadDetailsWarping",
                success: function (partialView) {
                    $("#warpingCount").html(partialView);
                    //toastr.success("Preview pane added below.", "Found!!");
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });
        } else {
            $("#PreviousData").empty();
            toastr.warning("Please select a Program Number.", "Invalid Input");
        }
    });

    setNo.on("change", function () {

        if (setNo.val()) {

            $.ajax({
                async: true,
                cache: false,
                data: { "sdId": $(this).val() },
                type: "GET",
                url: "/RndSampleInfoWeaving/LoadDetails",
                success: function (partialView) {
                    $("#PreviousData").html(partialView);
                    //toastr.success("Preview pane added below.", "Found!!");
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });

            $.ajax({
                async: true,
                cache: false,
                data: { "sdId": $(this).val() },
                type: "GET",
                url: "/RndSampleInfoWeaving/LoadDetailsWarping",
                success: function (partialView) {
                    $("#warpingCount").html(partialView);
                    //toastr.success("Preview pane added below.", "Found!!");
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });

            $.ajax({
                async: true,
                cache: false,
                data: { "setId": $(this).val() },
                type: "GET",
                url: "/RndSampleInfoWeaving/LoadSetDetails",
                success: function (data, status, xhr) {
                    console.log(data);
                    if (data.f_PR_SIZING_PROCESS_ROPE_MASTER.length !== 0) {
                        $("#beam").show();
                        $("#sBeam").hide();
                        beamNo.html('');
                        beamNo.append('<option value="" selected>Select Beam</option>');
                        $.each(data.f_PR_SIZING_PROCESS_ROPE_MASTER, function (id, option) {
                            $.each(option.f_PR_SIZING_PROCESS_ROPE_DETAILS, function (i, opt) {
                                beamNo.append($('<option></option>').val(opt.sdid).html(opt.w_BEAM.beaM_NO));
                            });
                        });
                    }

                    if (data.f_PR_SLASHER_DYEING_MASTER.length !== 0) {
                        $("#sBeam").show();
                        $("#beam").hide();
                        sBeamNo.html("");
                        sBeamNo.append('<option value="" selected>Select Beam</option>');
                        $.each(data.f_PR_SLASHER_DYEING_MASTER, function (id, option) {
                            $.each(option.f_PR_SLASHER_DYEING_DETAILS, function (i, opt) {
                                sBeamNo.append($('<option></option>').val(opt.sldid).html(opt.w_BEAM.beaM_NO));
                            });
                        });
                    }

                    //toastr.success("Success", "Found!!");
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });
        } else {
            toastr.warning("Please select a Program Number.", "Invalid Input");
        }
    });
});

function removeFromList(index) {

    var attachTo = $("#RndSampleInfoWeavingDetails");
    var fData = $("#form").serializeArray();

    fData.push({ name: "RemoveIndex", value: index });
    fData.push({ name: "IsDelete", value: true });

    $.ajax({
        async: true,
        cache: false,
        data: fData,
        type: "POST",
        url: "/RndSampleInfoWeaving/AddRndSampleInfoWeavingTable",
        success: function (partialView) {
            attachTo.html(partialView);
        },
        error: function () {
            console.log("failed to attach...");
        }
    });
}