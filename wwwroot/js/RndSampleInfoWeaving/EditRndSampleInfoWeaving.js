
$(function () {

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

});