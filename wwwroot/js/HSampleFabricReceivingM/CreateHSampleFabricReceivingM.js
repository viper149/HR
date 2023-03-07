
var attachTo = $("#HSampleFabricReceivingM");

$(function () {

    const dpId = $("#HSampleFabricReceivingM_DPID");

    dpId.on("change", function () {

        const formData = $("#form").serializeArray();

        $.post("/SampleFabric/HO/Receive/GetHSampleFabricReceiveDetails", formData, function (partialView) {
            attachTo.html(partialView);
        });
    });
});


function removeFromList(index) {

    const formData = $("#form").serializeArray();

    formData.push(
        { name: "IsDelete", value: true },
        { name: "RemoveIndex", value: index }
    );

    $.post("/SampleFabric/HO/Receive/AddOrRemoveDispatchDetails", formData, function (partialView) {
        attachTo.html(partialView);
    });
}