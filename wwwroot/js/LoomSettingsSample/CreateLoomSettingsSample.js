
var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

$(function () {
    var devId = $("#LoomSettingsSample_DEV_ID");

    getByDevId();

    devId.on("change",
        function() {
            getByDevId();
        });

    function getByDevId() {
        if (devId.val()) {
            var formData = {
                "devId": devId.val()
            }
            $.get("/LoomSettingsSample/GetAllByDevId",
                formData,
                function(data) {
                    $("#setNo").text(data.setNo);
                    $("#color").text(data.color);
                    $("#LoomSettingsSample_GRCONST").val(`${data.warpCount} X ${data.weftCount} / ${data.grepi} X ${data.grppi}`);
                    $("#LoomSettingsSample_WEFT_COUNT").val(data.weftCount);
                    $("#LoomSettingsSample_WEFT_RATIO").val(data.ratio);
                    $("#LoomSettingsSample_WEFT_LOT").val(data.lot);
                    $("#LoomSettingsSample_WEFT_SUPP").val(data.supplier);
                    $("#loom").text(data.loomType);
                    $("#weave").text(data.weave);
                    $("#reedCount").text(data.reed);
                    $("#totalEnds").text(data.totalEnds);
                    $("#rs").text(data.rs);
                }).fail(function() {
                toastr.error(errors[0].message, errors[0].title);
            });
        }
    }
});