
var rcvdId = $("#HSampleDespatchD_RCVDID");
var qty = $("#HSampleDespatchD_QTY");
var btnAdd = $("#btnAdd");
var detailsList = $("#HSampleDespatchM");
var through = $("#HSampleDespatchM_THROUGH");

const form = $("#form");

var targets = [];
var targets_2 = [];

targets.push(qty);
targets_2.push(rcvdId, qty);

btnAdd.on("click", function () {
    if (checkErrors(targets_2)) {
        $.post("/HSampleDespatchM/AddOrRemoveDispatchDetails", form.serializeArray(), function (partialView) {
            detailsList.html(partialView);
        });
    }
});


rcvdId.on("change", function () {
    $.post("/HSampleDespatchM/GetHoSampleReceiveDetails", { rcvdId: $(this).val() }, function (data) {
        if (data) {
            qty.val(data);
            addClass(targets, "border border-info");
        } else {
            resetFields(targets);
            removeClass(targets, "border border-info");
        }
    });
});

through.on("click focus", function () {
    $(this).val("Messenger");
});

function removeFromList(index) {

    const data = $("#form").serializeArray();

    data.push({ name: "IsDeletable", value: true }, { name: "RemoveIndex", value: index });

    $.post("/HSampleDespatchM/AddOrRemoveDispatchDetails", data, function (result) {
        detailsList.html(result);
    });
}