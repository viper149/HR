
var btnAdd = $("#btnAdd");
var detailsList = $("#FSampleGarmentRcvD");
var itemId = $("#FSampleGarmentRcvD_SITEMID");
var colorId = $("#FSampleGarmentRcvD_COLORID");
var fabCode = $("#FSampleGarmentRcvD_FABCODE");
var locationId = $("#FSampleGarmentRcvD_LOCID");
var styleInfo = $("#StyleInfo");
var buyerId = $("#FSampleGarmentRcvD_BUYERID");
var productionDate = $("#ProductionDate");
var sectionId = $("#FSampleGarmentRcvM_SECID");
var btnAddInspection = $("#addInspection");
var errors = {
    0: {
        title: "Invalid Operation",
        message: "Select Inspection Section"
    },
    1: {
        title: "Invalid Operation",
        message: "Select Inspection Section"
    }
}

var inputArray = [];

inputArray.push(itemId, fabCode, locationId, buyerId);

btnAddInspection.on("click", function () {
    loadInspectionData();
});

var loadInspectionData = function () {

    // 167 => Inspection
    if (sectionId.val() === '167') {

        var formData = $("#form").serializeArray();

        $.post("/SampleGarments/GetDetailsFormInspection", formData, function (partialView) {
            detailsList.html(partialView);
        }).fail(function () {

        });
    }
}

$("#FSampleGarmentRcvM_EMPID").select2({
    ajax: {
        url: '/SampleGarments/GetEmployees',
        data: function (params) {

            var query = {
                search: params.term,
                page: params.page || 1
            }

            return query;
        },
        processResults: function (data) {
            return {
                results: $.map(data.fHrEmployees, function (item) {
                    return {
                        id: item.empid,
                        text: item.firsT_NAME
                    };
                })
            };
        }
    }
});

$("#FSampleGarmentRcvM_SECID").select2({
    ajax: {
        url: '/SampleGarments/GetSections',
        data: function (params) {

            var query = {
                search: params.term,
                page: params.page || 1
            }

            return query;
        },
        processResults: function (data) {
            return {
                results: $.map(data.fBasSections, function (item) {
                    return {
                        id: item.secid,
                        text: item.secname
                    };
                })
            };
        }
    }
});

btnAdd.on("click", function () {

    const data = $("#form").serialize();

    if (checkErrors(inputArray)) {
        $.post("/FSampleGarmentRcvM/AddOrDeleteFSampleGarmentRcvMDetailsTable", data, function (result) {
            detailsList.html(result);
        });
    }
});

fabCode.on("change", function () {
    if (fabCode.val()) {
        $.post("/FSampleGarmentRcvM/GetStyleInfo", { "fabCode": $(this).val() }, function (partialView) {
            styleInfo.html(partialView);
        });
    } else {
        styleInfo.empty();
    }
});

function removeFromList(index) {

    const data = $("#form").serializeArray();

    data.push({ name: "IsDeletable", value: true }, { name: "RemoveIndex", value: index });

    $.post("/FSampleGarmentRcvM/AddOrDeleteFSampleGarmentRcvMDetailsTable", data, function (result) {
        detailsList.html(result);
    });
}

var updateRow = function (selector) {
    $("[type='hidden']", selector).prop("type", "text").addClass("col-xs-6 col-sm-3 mr-2");
};
