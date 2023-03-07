
var btnAdd = $("#btnAdd");
var buyerId = $("#FSampleDespatchDetails_BYERID");
var detailsList = $("#FSampleDesPatchMaster");
var fSampleDespatchDetailsTrnsId = $("#FSampleDespatchDetails_TRNSID");
var itemAvailability = $("#ItemAvailability");
var barcode = $("#FSampleDespatchDetails_TRNS_BARCODE");

var inputArray = [];

inputArray.push(fSampleDespatchDetailsTrnsId, buyerId);

btnAdd.on("click", function () {
    addItems();
});

fSampleDespatchDetailsTrnsId.on("change", function () {
    const selectedItemsValue = $(this).val();

    if (checkErrors({ fSampleDespatchDetailsTrnsId })) {
        $.post("/FSampleDesPatchMaster/GetNumberOfTotalItems", { "trnsId": selectedItemsValue }, function (result) {
            itemAvailability.html(result);
        });
    } else {
        $("#FSampleDespatchDetails_REQ_QTY").removeAttr("min max").val("");
        $("#FSampleDespatchDetails_DEL_QTY").removeAttr("min max").val("");
        itemAvailability.empty();
    }
});

barcode.on("keydown", function () {

    setTimeout(() => {
        getObjectsByBarcode();
    }, 50);
});

var getObjectsByBarcode = function () {
    const data = $("#form").serializeArray();

    $.post("/FSampleDesPatchMaster/GetObjectsByBarcode", data, function (result) {

        if (!$.isEmptyObject(result)) {
            if (result.trnsid !== null) {
                fSampleDespatchDetailsTrnsId.val(result.trnsid).trigger("change");
            }
        }
    }).done(function () {
        barcode.val("");
    });
};

var addItems = function () {
    const data = $("#form").serializeArray();

    if (checkErrors(inputArray)) {
        $.post("/FSampleDesPatchMaster/AddOrDeleteFSampleDesPatchMasterDetailsTable", data, function (result) {
            detailsList.html(result);
        });
    }
};

var removeFromList = function (index) {

    const data = $("#form").serializeArray();

    data.push({ name: "IsDeletable", value: true }, { name: "RemoveIndex", value: index });

    $.post("/FSampleDesPatchMaster/AddOrDeleteFSampleDesPatchMasterDetailsTable", data, function (result) {
        detailsList.html(result);
    });
};

$(function () {
    barcode.focus();
});