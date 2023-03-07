
var detailsList = $("#FSampleFabricRcvD");
var secId = $("#FSampleFabricRcvM_SECID");
var btnAddInspection = $("#addInspection");
var FabRcvDate = $("#FSampleFabricRcvM_SFRDATE");
var SftrDate = $("#FSampleFabricRcvM_SFTRDATE");
var ProdDate = $("#ProductionDate");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    },
    1: {
        title: "No Fabric Code Selected!!.",
        message: "Please Enter Fabric Code."
    },
    2: {
        title: "No Fabric Receive Date is selected!!. ",
        message:"Please select Fabric Receive Date "
    }
}

btnAddInspection.on("click", function () {
    loadInspectionData();
});

var loadInspectionData = function () {

    // 167 => Inspection
    var formData;

    if (secId.val() === "167") {

        formData = $("#form").serializeArray();

        $.post("/SampleFabricReceive/GetDetailsFormInspection", formData, function (partialView) {
            detailsList.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    } else if (secId.val() === "166") {

        formData = $("#form").serializeArray();

        $.post("/SampleFabricReceive/GetDetailsFromClearance", formData, function (partialView) {
            detailsList.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

$(function () {

    var sItem = $("#FSampleFabricRcvD_SITEMID");
    var empId = $("#FSampleFabricRcvM_EMPID");
    var fabcode = $("#FSampleFabricRcvD_FABCODE");
    var btnAdd = $("#btnAdd");
    var setId = $("#FSampleFabricRcvD_SETID");

    btnAdd.on("click", function () {

        if (fabcode[0].selectedIndex <= 0) {
            toastr.warning(errors[1].message, errors[1].title);
            return false;
        }

        const data = $("#form").serialize();

        $.post("/SampleFabricReceive/AddOrRemoveDetailsList", data, function (result) {
            detailsList.html(result);
        });
    });

    setId.select2({
        ajax: {
            url: "/SampleFabricReceive/GetPrograms",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.plProductionSetdistributions, function (item) {
                        return {
                            id: item.setid,
                            text: item.proG_.proG_NO
                        };
                    })
                };
            }
        }
    });

    fabcode.select2({
        ajax: {
            url: "/SampleGarments/GetRndFabrics",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.rndFabricinfos, function (item) {
                        return {
                            id: item.fabcode,
                            text: item.stylE_NAME
                        };
                    })
                };
            }
        }
    });

    sItem.select2({
        ajax: {
            url: "/SampleGarments/GetSampleItems",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.fSampleItemDetailses, function (item) {
                        return {
                            id: item.sitemid,
                            text: item.name
                        };
                    })
                };
            }
        }
    });

    empId.select2({
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


    secId.select2({
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
});

function removeFromList(index) {

    const data = $("#form").serializeArray();

    data.push(
        { name: "IsDeletable", value: true },
        { name: "RemoveIndex", value: index }
    );

    $.post("/SampleFabricReceive/AddOrRemoveDetailsList", data, function (result) {
        detailsList.html(result);
    });
}

$(function () {
    getByFabRcvDate();
    FabRcvDate.on("change", function () {
        getByFabRcvDate();
    });
});

function getByFabRcvDate() {

    SftrDate.val(FabRcvDate.val());
    ProdDate.val(FabRcvDate.val());

}
