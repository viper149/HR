
$(function () {

    var totalEnds = $("#rND_FABRICINFO_TOTALENDS");
    var reedSpace = $("#rND_FABRICINFO_REED_SPACE");
    var reedCount = $("#rND_FABRICINFO_REED_COUNT");
    var reedDent = $("#rND_FABRICINFO_DENT");
    var fnPpi = $("#rND_FABRICINFO_FNPPI");
    var countId = $("#rND_FABRIC_COUNTINFO_COUNTID");
    var lotId = $("#rND_FABRIC_COUNTINFO_LOTID");
    var supplierId = $("#rND_FABRIC_COUNTINFO_SUPPID");
    var yarnFor = $("#rND_FABRIC_COUNTINFO_YARNFOR");
    var ratio = $("#rND_FABRIC_COUNTINFO_RATIO");
    var ne = $("#rND_FABRIC_COUNTINFO_NE");
    var trnsId = $("#rND_FABRIC_COUNTINFO_TRNSID");
    var loom = $("#rND_FABRICINFO_LOOMID");
    var pickLength = $("#rND_FABRICINFO_PICKLENGHT");
    var wrapRatiion = $("#rND_FABRICINFO_OPT2");
    var wrapWeaving = $("#rND_FABRICINFO_OPT3");

    //$("#rND_FABRIC_COUNTINFO_COLORCODE").on("change", function () {
    //    var selectedVal = $(this).val();
    //    var blackShade = [8670, 8671, 8673, 8674, 8678, 8687, 8698, 8703, 8720, 8727, 8729, 8740, 8743, 8750, 8763, 8766, 8770, 8799, 8800, 8805, 8809, 8814, 8815];
    //    if ($.inArray(parseInt(selectedVal), blackShade) !== -1) {
    //        $("#rND_FABRICINFO_REMARKS").val($("#rND_FABRICINFO_REMARKS").val()+" Need well contamination controlled yarn");
    //    }
    //});

    $("#btnAdd").on("click", function () {
        if (!totalEnds.val()) {
            toastr.warnig("Please Enter Total Ends First", "Invalid Operation");
        } else if (!fnPpi.val()) {
            toastr.warning("Please Enter Finish PPI Value First", "Invalid Operation");
        }
        else if (!countId.val()) {
            toastr.warning("Please Select A Count Name First", "Invalid Operation");
        }
        else if (!lotId.val()) {
            toastr.warning("Please Select A Lot No First", "Invalid Operation");
        }
        else if (!supplierId.val()) {
            toastr.warning("Please Select A Supplier Name First", "Invalid Operation");
        }
        else if (!yarnFor.val()) {
            toastr.warning("Please Select A Yarn For First", "Invalid Operation");
        }
        else if (!ratio.val()) {
            toastr.warning("Field Ratio Can Not Be Empty", "Invalid Operation");
        }
        else if (!ne.val()) {
            toastr.warning("Field NE Can Not Be Empty", "Invalid Operation");
        }

        else if (trnsId.val()) {
          swal({
                title: "Please Confirm",
                text: "Data Will Directly Updated To Database",
                type: "info",
                showCancelButton: true,
                confirmButtonText: "OK",
                cancelButtonText: "Cancel"
            }, function (isConfirm) {
                if (isConfirm) {
                    $.ajax({
                        async: true,
                        cache: false,
                        data: $("#form").serialize(),
                        type: "POST",
                        url: "/RndFabricInfo/FEAddFabricCountinfo",
                        success: function (partialView) {
                            $("#PreviousData").html(partialView);
                        },
                        error: function () {
                            console.log("failed to add...");
                        }
                    });
                }
            });
        }
        else {
            $.ajax({
                async: true,
                cache: false,
                data: $("#form").serialize(),
                type: "POST",
                url: "/RndFabricInfo/FEAddFabricCountinfo",
                success: function (partialView) {
                    $("#PreviousData").html(partialView);
                },
                error: function () {
                    console.log("failed to add...");
                }
            });
        }
    });
});








function UpdateConsumptionData() {

    $.ajax({
        async: true,
        cache: false,
        type: "POST",
        data: $("#form").serialize(),
        url: "/RndFabricInfo/UpdateConsumptionData",
        success: function (partialView) {

            $("#PreviousData").html(partialView);

            //if (data === "Success") {
            //    toastrNotification("Consumption Updated Successfully", "success");
            //}
            //else if (data === "Failed")
            //{
            //    toastrNotification("Consumption Update Failed", "error");
            //}
        },
        error: function () {
            toastrNotification("Consumption Update Failed(E)", "error");
        }
    });
}

function removeFromDynamicList(removeIndex) {

    if (removeIndex) {
        console.log(removeIndex);
        var data = $("#form").serializeArray();
        data.push({ name: "RemoveIndex", value: removeIndex });

        $.ajax({
            async: true,
            cache: false,
            type: "POST",
            data: data,
            url: "/RndFabricInfo/RemoveFabricCountinfoAndYarnConsumption",
            success: function (partialView) {
                $("#PreviousData").html(partialView);
            },
            error: function () {
                console.log("failed to remove...");
            }
        });
    } else {
        toastrNotification("Invalid operation.", "warning");
    }
}