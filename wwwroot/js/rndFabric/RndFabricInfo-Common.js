
var totalEnds = $("#rND_FABRICINFO_TOTALENDS");
var grepi = $("#rND_FABRICINFO_GREPI");
var reedSpace = $("#rND_FABRICINFO_REED_SPACE");
var pickleLength = $("#rND_FABRICINFO_PICKLENGHT");
var reedCount = $("#rND_FABRICINFO_REED_COUNT");
var reedDent = $("#rND_FABRICINFO_DENT");
var loom = $("#rND_FABRICINFO_LOOMID");
var endsPerRope = $("#rND_FABRICINFO_ENDS");
var totalRope = $("#rND_FABRICINFO_TOTALROPE");

$(function () {
    totalEnds.on("change", function () {
        getRs();
    });
    grepi.on("change", function () {
        getRs();
    });
    loom.on("change", function () {
        getRs();
    });
});

function getRs() {

    var _totalEnds = totalEnds.val();
    var _totalRope = totalRope.val();
    var _grepi = (reedCount.val() * reedDent.val()) / 39.37;

    if (_totalEnds && _grepi) {
        reedSpace.val(getReedSpace(_totalEnds, _grepi).toFixed(2));
        pickleLength.val(getPickleLength(_totalEnds, _grepi).toFixed(2));
    }
    if (_totalEnds && _totalRope) {
        endsPerRope.val(getReedSpace(_totalEnds, _totalRope).toFixed(2));
    }
}

function getReedSpace(x, y) {
    return x / y;
}

function getPickleLength(x, y) {
    if (loom.val() === "1") {
        return x / y + 3;
    } else if (loom.val() === "2") {
        return x / y + 6;
    } else {
        return "";
    }
}