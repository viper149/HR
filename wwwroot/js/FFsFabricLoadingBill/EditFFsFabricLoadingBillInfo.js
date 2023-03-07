var rollQtyId = $("#FFsFabricLoadingBill_ROLL_QTY");
var rate = $("#FFsFabricLoadingBill_RATE");
var vehicleId = $("#FFsFabricLoadingBill_VEHICLE_ID");
var startTime = $("#stTime");
var endTime = $("#endTime");
var totalTime = $("#FFsFabricLoadingBill_TOTAL_TIME");
var otherVehicle = $("#FFsFabricLoadingBill_OPT1");
var remarks = $("#FFsFabricLoadingBill_REMARKS");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

$(function () {
    getRollQtyId();
    getTotalTime();

    rollQtyId.on("change", function () {
        getRollQtyId();
    });

    endTime.on("change", function () {
        getTotalTime();
    });

});

function getRollQtyId() {
    if (rollQtyId.val()) {
        (rollQtyId.val() <= 150) ? rate.val(100) : rate.val(200);
    }
}

function getTotalTime() {
    if (endTime.val()) {
        if (startTime.val()) {
            totalTime.val(daysDifference(startTime.val(), endTime.val()));
        }
        else {
            toastr.error(errors[0].message, errors[0].title);
        }
    }
}

function daysDifference(startTime, endTime) {

    var getMillisec = new Date(endTime).getTime() - new Date(startTime).getTime();

    if (getMillisec >= 0) {
        var hours = (getMillisec) / (1000 * 3600);
        return Math.round(hours);
    }
    else {
        toastr.error(errors[1].message, errors[1].title);
        return 0;
    }
}