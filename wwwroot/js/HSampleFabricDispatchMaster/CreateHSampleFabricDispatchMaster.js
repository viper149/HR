
var attachTo = $("#HSampleFabricDispatchDetails");

$(function () {

    var btnAdd = $("#btnAdd");
    var buyerId = $("#HSampleFabricDispatchMaster_BUYERID");
    var brandId = $("#HSampleFabricDispatchMaster_BRANDID");
    var merchId = $("#HSampleFabricDispatchMaster_MERCID");
    var rcvdId = $("#HSampleFabricDispatchDetails_RCVDID");
    var delQty = $("#HSampleFabricDispatchDetails_DEL_QTY");

    btnAdd.on("click", function () {

        const data = $("#form").serializeArray();

        $.post("/SampleFabric/HO/Dispatch/AddOrRemoveDetailsList", data, function (partialView) {
            attachTo.html(partialView);
        }).fail(function () {
            
        });
    });

    rcvdId.on("change", function () {

        if (rcvdId.val()) {

            var formData = $("#form").serializeArray();

            $.post("/SampleFabric/HO/Dispatch/GetOtherInfo", formData, function (data) {
                delQty.val(data.qty);
            });
        } else {
            delQty.val('');
        }
    });

    rcvdId.select2({
        placeholder: "Select an Item",
        allowClear: true,
        ajax: {
            url: "/SampleFabric/HO/Dispatch/GetAvailableItems",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            id: item.rcvdid,
                            text: item.dpd.fSampleFabricRcvD.sitem.name
                        };
                    })
                };
            }
        }
    });

    merchId.select2({
        placeholder: "Select a Merchandiser",
        allowClear: true,
        ajax: {
            url: "/SampleFabric/HO/Dispatch/GetMerchandisers",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            id: item.mercid,
                            text: item.merchandiseR_NAME
                        };
                    })
                };
            }
        }
    });

    brandId.select2({
        placeholder: "Select a Brand",
        allowClear: true,
        ajax: {
            url: "/SampleFabric/HO/Dispatch/GetBrands",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            id: item.brandid,
                            text: item.brandname
                        };
                    })
                };
            }
        }
    });

    buyerId.select2({
        placeholder: "Select a Buyer",
        allowClear: true,
        ajax: {
            url: "/SampleFabric/HO/Dispatch/GetBuyers",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            id: item.buyerid,
                            text: item.buyeR_NAME
                        };
                    })
                };
            }
        }
    });
});

function removeFromList(index) {

    const data = $("#form").serializeArray();

    data.push(
        { name: "IsDelete", value: true },
        { name: "RemoveIndex", value: index }
    );

    $.post("/SampleFabric/HO/Dispatch/AddOrRemoveDetailsList", data, function (partialView) {
        attachTo.html(partialView);
    });
}