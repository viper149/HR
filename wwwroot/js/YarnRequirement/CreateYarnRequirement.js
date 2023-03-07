
const btnAdd = $("#addToList");

var detailsList = $("#yarnRequirementDetailsAddItem");
var deptId = $("#FYarnRequirementMaster_DEPTID");
var orderId = $("#FYarnRequirementDetails_ORDERNO");
var rsId = $("#FYarnRequirementDetails_RSID");
var countId = $("#FYarnRequirementDetails_COUNTID");
var countIdRs = $("#FYarnRequirementDetails_COUNTID_RS");
var setId = $("#FYarnRequirementDetails_SETID");
var lotNo = $("#lot");
var lotNoNew = $("#FYarnRequirementDetailsList_1__LOTID");
var reqQty = $("#FYarnRequirementDetails_REQ_QTY");


$(function () {

    var errors = {
        0: {
            title: "Invalid Submission",
            message: "We can not process your data! Please try again."
        }
    }

    var ordertype = $("#FYarnRequirementDetails_ORDER_TYPE");


    btnAdd.on("click", function () {

        const data = $("#form").serializeArray();
        $.post("/FYarnRequirement/AddOrDeleteFYarnRequirementDetailsTable", data, function (result) {
            detailsList.html(result);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });


    getByOrderType();

    ordertype.on("change", function () {
        getByOrderType();
    });

    function getByOrderType() {

        //var formData = {
        //    "id": ordertype.val()
        //}

        // For Order
        if (ordertype.val() == "OrderNo") {

            countId.html("");

            $("#orderNoInfo").removeClass("d-none");
            $("#buyerName").removeClass("d-none");
            $("#sizingSlasherLength").removeClass("d-none");
            $("#lotPrevious").removeClass("d-none");
            $("#count").removeClass("d-none");
            $("#progNo").removeClass("d-none");
            $("#reqKgs, #remKgs, #reqKgsSet, #remKgsSet").removeClass("d-none");


            $("#RSNoInfo").addClass("d-none");
            $("#countRs").addClass("d-none");


            $("#buyer").text("");
            $("#fabcode").text("");
            $("#sizingLength").text("");
            $("#FYarnRequirementDetails_REMARKS").text("");
            $("#FYarnRequirementDetails_REQ_QTY").text("");
            $("#reqKgs, #remKgs, #reqKgsSet, #remKgsSet").text("");
            $("#sizingLength").text("");
            lotNo.text("");


            orderId.on("change", function () {
                lotNo.text("");
                const ordId = orderId.val();
                if (ordId) {
                    $.get("/YarnRequirement/GetCountList", { 'poId': ordId }, function (data) {
                        var tableRows = "";
                        var tableRowsRem = "";
                        countId.html("");
                        countId.append('<option value="" selected>Select Count Name</option>');
                        $.each(data.count, function (id, option) {
                            countId.append($("<option></option>").val(option.trnsid).html(option.count.rnD_COUNTNAME));
                        });
                        if (ordId != 304644) {
                            $.each(data.dynamic,
                                function (index, value) {
                                    tableRows += (`<ul class="list-group list-group-flush"><li class="list-group-item">${value
                                        .count.rnD_COUNTNAME} : ${value.amount} Kgs</li></ul>`);
                                });

                            $.each(data.dynamic,
                                function (index, value) {
                                    tableRowsRem += (`<ul class="list-group list-group-flush"><li class="list-group-item">${value.count.rnD_COUNTNAME} : ${value.remaining != null ? value.remaining.toFixed(2) : 0
                                        } Kgs (${value.rcv.toFixed(2)})</li></ul>`);
                                });

                            $("#fabcode").text(data.piDetails.style.fabcodeNavigation.stylE_NAME);
                            $("#buyer").text(data.piDetails.pimaster.buyer.buyeR_NAME);
                            $("#reqKgs")
                                .html(`<div class="card"><div class="card-header">Orderwise Budget</div>${tableRows}</div>`);
                            $("#remKgs")
                                .html(`<div class="card"><div class="card-header">Orderwise Remaining</div>${tableRowsRem
                                    }</div>`);
                        } else {

                            $("#fabcode").text("Leadline");
                        }

                    }).fail(function () {
                        toastr.error(errors[0].message, errors[0].title);
                    });

                } else {

                    countId.html("");
                    $("#fabcode").text("");
                    $("#buyer").text("");
                    lotNo.text("");

                    toastr.error(errors[0].message, errors[0].title);
                }
            });

            setId.on("change", function () {

                lotNo.text("");
                const setIdVal = setId.val();

                if (setIdVal) {
                    $.get("/YarnRequirement/GetCountConsumpList", { 'setId': setIdVal }, function (data) {
                        var tableRows = "";
                        var tableRowsRem = "";
                        $.each(data.dynamic, function (index, value) {
                            tableRows += (`<ul class="list-group list-group-flush"><li class="list-group-item">${value.count.rnD_COUNTNAME} : ${value.amount} Kgs</li></ul>`);
                        });
                        $('#sizingLength').text(data.dynamic[0].qty);

                        $("#reqKgsSet").html(`<div class="card"><div class="card-header">Setwise Budget</div>${tableRows}</div>`);

                        $.each(data.dynamic, function (index, value) {
                            tableRowsRem += (`<ul class="list-group list-group-flush"><li class="list-group-item">${value.count.rnD_COUNTNAME} : ${value.remaining != null ? value.remaining.toFixed(2) : 0} Kgs </li></ul>`);
                        });

                        $("#remKgsSet").html(`<div class="card"><div class="card-header">Setwise Remaining</div>${tableRowsRem}</div>`);

                    }).fail(function () {
                        toastr.error(errors[0].message, errors[0].title);
                    });

                } else {

                    $("#reqKgsSet").html("");

                    toastr.error(errors[0].message, errors[0].title);
                }
            });




            countId.on("change", function () {
                /*debugger;*/
                let formData = {
                    "poId": orderId.val(),
                    "countId": countId.val()
                };

                if (formData.poId && formData.countId) {

                    $.get("/YarnRequirement/GetRequiredAmountWithLot", formData, function (data) {

                        lotNo.text(data.lotno);
                        //reqQty.attr({ "max": data.requireD_KGS });
                    }).fail(function () {
                        toastr.error(errors[0].message, errors[0].title);
                    });
                }
            });




        }

        // For RS No

        else {

            $("#buyer").text("");
            $("#fabcode").text("");
            $("#sizingLength").text("");
            $("#FYarnRequirementDetails_REMARKS").val("");
            $("#FYarnRequirementDetails_REQ_QTY").val("");
            lotNo.text("");


            $("#RSNoInfo").removeClass("d-none");
            $("#countRs").removeClass("d-none");

            $("#orderNoInfo").addClass("d-none");
            $("#buyerName").addClass("d-none");
            $("#sizingSlasherLength").addClass("d-none");
            $("#lotPrevious").addClass("d-none");
            $("#progNo").addClass("d-none");
            $("#count").addClass("d-none");
            $("#reqKgs, #remKgs, #reqKgsSet, #remKgsSet").addClass("d-none");

            $.get("/YarnRequirement/GetCountList2", function (data) {

                countIdRs.html("");
                countIdRs.append('<option value="" selected>Select Count Name</option>');

                $.each(data,
                    function (id, option) {
                        countIdRs.append($("<option>",
                            {
                                value: option.countid,
                                text: option.rnD_COUNTNAME
                            }));
                    });

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });

            


            rsId.on("change", function () {

                if (rsId.val()) {

                    var formdata = {
                        "rsId": rsId.val()
                    }
                    $.get("/YarnRequirement/GetStyleByRs",
                        formdata,
                        function (data) {
                            $("#fabcode").text(data.styleref);
                        }).fail(function () {
                            toastr.error(errors[0].message, errors[0].title);
                        });
                }

            });

        }

    }

});

var removeFromList = function (index) {

    const data = $("#form").serializeArray();

    data.push({ name: "IsDeletable", value: true }, { name: "RemoveIndex", value: index });

    $.post("/FYarnRequirement/AddOrDeleteFYarnRequirementDetailsTable", data, function (result) {
        detailsList.html(result);
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
};