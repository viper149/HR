$(function () {
    var startTime = $("#FBasHrdShift_TIME_START");
    var endTime = $("#FBasHrdShift_TIME_END");
    var duration = $("#duration");

    [startTime, endTime].forEach(function (element) {
        formatTime(element);
        element.on("change", GetDuration);
    });

    GetDuration();
    startTime.add(endTime).on("change", GetDuration);

    function GetDuration() {
        if (startTime.val() && endTime.val()) {
            var diff = new Date(`1970-01-01T${endTime.val()}:00Z`) - new Date(`1970-01-01T${startTime.val()}:00Z`);
            if (diff < 0) {
                toastr.error("End time should be greater than start time.", "Invalid time combination.");
                return;
            }
            var hours = Math.floor(diff / 3600000);
            var minutes = Math.floor((diff % 3600000) / 60000);
            var hhmm = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;

            duration.html(hhmm);
        }
    }
});

function formatTime (timeString) {
    var timeParts = timeString.val().split(':');
    timeString.val((timeParts.length == 3) ? `${timeParts[0]}:${timeParts[1]}` : timeString);
}