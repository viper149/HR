
var attachTo = $("#comImpWorkOrderDetailsTable");
var submitBtn = $("#btn_submit");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    },
    1: {
        title: "No Indent Selected!!",
        message: "Please select Indent No. first."
    },
    2: {
        title: "No Count Selected!!",
        message: "Please select Yarn Count first."
    }
}

$(function () {
    var indId = $("#ComImpWorkOrderMaster_INDID");
    var countId = $("#ComImpWorkOrderDetails_COUNTID");
    var unitId = $("#unit");
    var lotId = $("#lot")
    var btnAdd = $("#addToList");
    var orderQty = $("#ComImpWorkOrderDetails_QTY");
    var lotIdH = $("#ComImpWorkOrderDetails_LOTID");
    var slubCode = $("#slubCode");
    var nCone = $("#cone");
    var indRemarks = $("#indRemarks");

    getCount();

    indId.on("change", function () {
        getCount();
    });

    countId.on("change", function () {
        if (countId.val()) {

            var formData = {
                "transId": countId.val()
            }

            $.get("/ComImpWorkOrder/GetAllByCount", formData, function (data) {

                lotIdH.val(data.lot.lotid);
                lotId.text(data.lot.lotno);
                orderQty.val(data.ordeR_QTY);
                unitId.text(data.bascountinfo.unit);
                slubCode.text(data.sluB_CODENavigation.name);
                nCone.text(data.nO_OF_CONE);
                indRemarks.text(data.remarks);
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });

    btnAdd.on("click",
        function () {
            if (indId[0].selectedIndex <= 0) {
                toastr.error(errors[1].message, errors[1].title);
                return false;
            }
            if (countId[0].selectedIndex <= 0) {
                toastr.error(errors[2].message, errors[2].title);
                return false;
            }


            var formData = $("#form").serializeArray();

            $.post("/ComImpWorkOrder/AddToList",
                formData,
                function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
        });

    function getCount() {
        if (indId.val()) {
            var formData = {
                "indId": indId.val()
            }

            $.get("/ComImpWorkOrder/GetCountInfo", formData, function (data) {

                countId.html("");
                countId.append('<option value="" selected>Select Count</option>');

                $.each(data,
                    function (id, option) {
                        countId.append($("<option>",
                            {
                                value: option.trnsid,
                                text: option.bascountinfo.rnD_COUNTNAME
                            }));
                    });
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        }
    }
});

function RemoveIndent(index) {
    swal({
        title: "Please Confirm",
        text: `You won't able to revert, Are you sure to remove item no. - ${index + 1}`,
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {
            if (isConfirm) {

                var formData = $('#form').serializeArray();

                formData.push({ name: "IsDelete", value: true });
                formData.push({ name: "RemoveIndex", value: index });

                $.post("/ComImpWorkOrder/AddToList",
                    formData,
                    function (partialView) {
                        attachTo.html(partialView);
                    }).fail(function () {
                        toastr.error(errors[0].title, errors[0].message);
                    });
            }
        });
}

function CalculateTotalPrice(index) {
    var totalPrice = $(`#ComImpWorkOrderDetailsList_${index}__TOTAL`);
    var formData = $("#form").serializeArray();


    formData.push({ name: "RemoveIndex", value: index });

    $.post("/ComImpWorkOrder/GetCalulatedFieldsValue", formData, function (data) {
        totalPrice.val(data.total);
    });
}