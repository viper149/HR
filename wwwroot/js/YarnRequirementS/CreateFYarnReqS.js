
const btnAdd = $("#addToList");

var detailsList = $("#yarnRequirementDetailsSAddItem");
var deptId = $("#FYarnRequirementMasterS_DEPTID");
var orderId = $("#FYarnRequirementDetailsS_ORDERNO");
var countId = $("#FYarnRequirementDetailsS_COUNTID");
var setId = $("#FYarnRequirementDetailsS_SETID");
var lotNo = $("#lot");
var reqQty = $("#FYarnRequirementDetailsS_REQ_QTY");

var errors = {
    0: {
        title: "Invalid Submission",
        message: "We can not process your data! Please try again."
    }
}

var removeFromList = function (index) {

    const data = $("#form").serializeArray();

    data.push({ name: "IsDeletable", value: true }, { name: "RemoveIndex", value: index });

    $.post("/YarnRequirementS/AddToList", data, function (result) {
        detailsList.html(result);
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
};

$(function () {

    btnAdd.on("click", function () {

        const data = $("#form").serializeArray();
        $.post("/YarnRequirementS/AddToList", data, function (result) {
            detailsList.html(result);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });

    orderId.on("change", function () {

        lotNo.text("");
        const ordId = orderId.val();
        debugger;
        if (ordId) {
            $.get("/YarnRequirementS/GetCountList", { 'poId': ordId }, function (data) {
                console.log(data);
                var tableRows = "";
                var tableRowsRem = "";
                countId.html("");
                countId.append('<option value="" selected>Select Count Name</option>');

                $.each(data.count, function (id, option) {
                    countId.append($("<option></option>").val(option.trnsid).html(option.count.countname));
                });

                $.each(data.dynamic, function (index, value) {
                    tableRows += (`<ul class="list-group list-group-flush"><li class="list-group-item">${value.count.countname} : ${value.amount} Kgs</li></ul>`);
                });

                $.each(data.dynamic, function (index, value) {
                    tableRowsRem += (`<ul class="list-group list-group-flush"><li class="list-group-item">${value.count.countname} : ${value.remaining != null ? value.remaining.toFixed(2) : 0} Kgs</li></ul>`);
                });

                $("#fabcode").text(data.piDetails.style.fabcodeNavigation.stylE_NAME);
                $("#buyer").text(data.piDetails.pimaster.buyer.buyeR_NAME);
                $("#reqKgs").html(`<div class="card"><div class="card-header">Required Kgs(Order Wise)</div>${tableRows}</div>`);
                $("#remKgs").html(`<div class="card"><div class="card-header">Remaining Kgs(Order Wise)</div>${tableRowsRem}</div>`);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });

            $.get("/YarnRequirementS/GetSetList", { 'poId': ordId }, function (data) {

                setId.html("");
                setId.append('<option value="" selected>Select Set/Prog. No.</option>');

                $.each(data, function (id, option) {
                    setId.append($("<option></option>").val(option.id).html(option.name));
                });

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
            $.get("/YarnRequirementS/GetCountConsumpList", { 'setId': setIdVal }, function (data) {
                debugger;
                var tableRows = "";
                var tableRowsRem = "";
                console.log(data);
                $.each(data.dynamic, function (index, value) {
                    tableRows += (`<ul class="list-group list-group-flush"><li class="list-group-item">${value.count.countname} : ${value.amount} Kgs</li></ul>`);
                });
                $('#sizingLength').text(data.dynamic[0].qty);

                $("#reqKgsSet").html(`<div class="card"><div class="card-header">Required Kgs(Set Wise)</div>${tableRows}</div>`);

                $.each(data.dynamic, function (index, value) {
                    tableRowsRem += (`<ul class="list-group list-group-flush"><li class="list-group-item">${value.count.countname} : ${value.remaining != null ? value.remaining.toFixed(2) : 0} Kgs</li></ul>`);
                });

                $("#remKgsSet").html(`<div class="card"><div class="card-header">Remaining Kgs(Set Wise)</div>${tableRowsRem}</div>`);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });

        } else {

            $("#reqKgsSet").html("");

            toastr.error(errors[0].message, errors[0].title);
        }
    });

    countId.on("change", function () {

        const formData = {
            "poId": orderId.val(),
            "countId": countId.val()
        };

        if (formData.poId && formData.countId) {

            $.get("/YarnRequirementS/GetRequiredAmountWithLot", formData, function (data) {

                lotNo.text(data.lotno);
                //reqQty.attr({ "max": data.requireD_KGS });
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });
});