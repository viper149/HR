
$(function () {
    var ltgid = $("#RndSampleinfoFinishing_LTGID");
    GetPreviousData(ltgid.val());

    ltgid.on("change", function () {
        GetPreviousData(ltgid.val());
    });
});

function GetPreviousData(ltgId) {
    debugger;
    if (ltgId === "") {
        return false;
    }

    var styleName = $("#RndSampleinfoFinishing_STYLE_NAME");
    var devNo = $("#RndSampleinfoFinishing_DEV_NO");
    $.ajax({
        async: true,
        cache: false,
        data: {
            "ltgId": ltgId
        },
        type: "GET",
        url: "/RndSampleInfoFinish/GetPreviousData",
        success: function (partialView, status, xhr) {
            $("#PreviousData").html(partialView);
            const style = xhr.getResponseHeader("StyleName");
            styleName.val(style);
            devNo.val(style);
        },
        error: function () {
            console.log("failed to attach...");
        }
    });
}