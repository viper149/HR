
var detailsList = $("#FSampleFabricDispatchDetails");

$(function () {

    var btnAdd = $("#btnAdd");
    var dispatchItemId = $("#FSampleFabricDispatchDetails_TRNSID");
    var reqQty = $("#FSampleFabricDispatchDetails_REQ_QTY");
    var delQty = $("#FSampleFabricDispatchDetails_DEL_QTY");
    var typeId = $("#FSampleFabricDispatchMaster_TYPEID");
    var gpTypeId = $("#FSampleFabricDispatchMaster_GPTYPEID");
    var drId = $("#FSampleFabricDispatchMaster_DRID");
    var vId = $("#FSampleFabricDispatchMaster_VID");

    vId.select2({
        ajax: {
            url: "/SampleFabricDispatch/GetVehicleInfo",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.fBasVehicleInfos, function (item) {
                        return {
                            id: item.vid,
                            text: item.vnumber
                        };
                    })
                };
            }
        }
    });

    drId.select2({
        ajax: {
            url: "/SampleFabricDispatch/GetDriverInfo",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.fBasDriverinfos, function (item) {
                        return {
                            id: item.drid,
                            text: item.driveR_NAME
                        };
                    })
                };
            }
        }
    });

    gpTypeId.select2({
        ajax: {
            url: "/SampleFabricDispatch/GetGatePassType",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.gatepassTypes, function (item) {
                        return {
                            id: item.gptypeid,
                            text: item.gptypename
                        };
                    })
                };
            }
        }
    });

    typeId.select2({
        ajax: {
            url: "/SampleFabricDispatch/GetGatePassFor",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.fSampleDespatchMasterTypes, function (item) {
                        return {
                            id: item.typeid,
                            text: item.typename
                        };
                    })
                };
            }
        }
    });

    dispatchItemId.on("change", function () {

        if (dispatchItemId.val()) {

            const data = $("#form").serializeArray();

            $.post("/SampleFabricDispatch/GetQty", data, function (result) {

                reqQty.val(result.qty);
                //delQty.val(result.qty);

            }).fail(function (ex) {
                console.log(ex);
            });
        } else {
            reqQty.val("");
            //delQty.val("");
        }
    });

    btnAdd.on("click", function () {

        const data = $("#form").serializeArray();

        $.post("/SampleFabricDispatch/AddOrRemoveDetailsList", data, function (result) {
            detailsList.html(result);
        }).fail(function (ex) {
            console.log(ex);
        });
    });
});

function removeFromList(index) {

    const data = $("#form").serializeArray();

    data.push(
        { name: "IsDeletable", value: true },
        { name: "RemoveIndex", value: index }
    );

    $.post("/SampleFabricDispatch/AddOrRemoveDetailsList", data, function (result) {
        detailsList.html(result);
    });
}