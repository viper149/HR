const btnAdd = $("#addToList");
var detailsList = $("#fFsFabricUPDetailsAddItem");
var lcId = $("#FFsFabricDetail_LC_ID");
var expDate = $("#expDate");
var partyName = $("#partyName");
var lcQty = $("#lcQty");
var expDate = $("#expDate");
var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}


$(function () {
    lcId.on("change", function () {
        getLcInfo();
    });

    function getLcInfo() {

        var formData = {
            "lcId": lcId.val()
        }

        if (lcId.val()) {
            $.get("/FFsFabricUP/GetLcInfo", formData, function (data) {
                console.log(data);
                expDate.text(data.eX_DATE);
                partyName.text(data.buyer.buyeR_NAME);
                lcQty.text(data.value);
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    }
    btnAdd.on("click", function () {
        const data = $("#form").serializeArray();
        $.post("/FFsFabricUP/AddOrDeleteFFsFabricUPDetailsTable", data, function (result) {
            detailsList.html(result);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });



});

var removeFromList = function (index) {

    const data = $("#form").serializeArray();

    data.push({ name: "IsDeletable", value: true }, { name: "RemoveIndex", value: index });

    $.post("/FFsFabricUP/AddOrDeleteFFsFabricUPDetailsTable", data, function (result) {
        detailsList.html(result);
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
};


