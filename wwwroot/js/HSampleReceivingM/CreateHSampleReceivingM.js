
var dpId = $("#HSampleReceivingM_DPID");
var partial = $("#HSampleReceivingM");

dpId.on("change", function () {
    $.post("/HSampleReceivingM/GetFactoryGatePassReceiveDetails", { dpId: $(this).val() }, function (data) {
        partial.html(data);
    });
});