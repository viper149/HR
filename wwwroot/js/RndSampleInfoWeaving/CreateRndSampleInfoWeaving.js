
$(function () {
    var rndSampleInfoWeavingSdId = $("#RndSampleInfoWeaving_SDID");
    var attachPreviousDataTo = $("#PreviousData");
    var totalEnds = $("#RndSampleInfoWeaving_TOTALENDS");
    var reedSpace = $("#RndSampleInfoWeaving_REED_SPACE");
    var reedCount = $("#RndSampleInfoWeaving_REED_COUNT");
    var reedDent = $("#RndSampleInfoWeaving_DENT");

    totalEnds.on("change", function () {
        if (totalEnds.val() && reedCount.val() && reedDent.val()) {
            var rs = (totalEnds.val() / (reedCount.val() * reedDent.val())) * 39.37;
            reedSpace.val(rs);
        }
    });
    
    rndSampleInfoWeavingSdId.on("change", function () {
        if (rndSampleInfoWeavingSdId.val()) {
            $.ajax({
                async: true,
                cache: false,
                data: { "sdId": $(this).val() },
                type: "GET",
                url: "/RndSampleInfoWeaving/LoadDetails",
                success: function (partialView) {
                    attachPreviousDataTo.html(partialView);
                    toastr.success("Preview pane added below.", "Found!!");
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });
        } else {
            attachPreviousDataTo.empty();
            toastr.warning("Please select a Program Number.", "Invalid Input");
        }
    });
});