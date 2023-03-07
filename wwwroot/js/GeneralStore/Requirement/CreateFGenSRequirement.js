
var attachTo = $("#fGenSReqDetailsTable");
var submitBtn = $("#btn_submit");
var empId = $("#FGenSReqMaster_REQUISITIONBY");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    },
    1: {
        title: "Stock Alert!!.",
        message: "Insufficient Stock!! Please decrease your Requested Quantity."
    },
    2: {
        title: "No Quantity!!.",
        message: "Please Enter Quantity"
    },
    3: {
        title: "No Product Selected!!",
        message: "Please select product first."
    }
}

$(function () {
    var productId = $("#FGenSReqDetails_PRODUCTID");
    var unit = $("#unit");
    var btnAdd = $("#addToList");
    var reqQty = $("#FGenSReqDetails_REQ_QTY");
    var balance = $("#balance");

    getEmpData();

    empId.on("change", function () {
        getEmpData();
    });

    btnAdd.on("click", function () {
        if (productId[0].selectedIndex <= 0) {
            toastr.warning(errors[3].message, errors[3].title);
            return false;
        }

        if (parseInt(reqQty.val()) > parseInt(balance.text())) {
            toastr.error(errors[1].message, errors[1].title);
            return false;
        }
        else if (!parseInt(reqQty.val()) > 0) {
            toastr.error(errors[2].message, errors[2].title);
            return false;
        }

        var formData = $("#form").serializeArray();

            $.post("/GeneralStoreRequirement/AddOrRemoveFromList",
                formData,
                function(partialView) {
                    attachTo.html(partialView);
                }).fail(function() {
                toastr.error(errors[0].title, errors[0].message);
            });
    });

    productId.on("change", function () {

        if (productId.val()) {

            var formData = {
                "id": productId.val()
            }

            $.get("/GeneralStoreRequirement/GetProductDetails", formData, function (data) {

                unit.text(data.unitNavigation.uname);
                balance.text(data.balance);
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });
});

function getEmpData() {
    if (empId.val()) {
        var deptId = $("#deptn");
        var secId = $("#secn");
        var deptH = $("#FGenSReqMaster_DEPTID");
        var secH = $("#FGenSReqMaster_SECID");
        var ssecH = $("#FGenSReqMaster_SSECID");
        var formData = {
            "id": empId.val()
        }

        $.get("/GeneralStoreRequirement/GetEmpInfo", formData, function (data) {

            deptId.text(data.dept.deptname);
            secId.text(data.sec.secname);
            /*subSectionId.text(data.ssec.ssecname);*/
            deptH.val(data.deptid);
            secH.val(data.secid);
            ssecH.val(data.ssecid);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

function RemoveIndent(index) {
    swal({
        title: "Please Confirm",
        text: `You won't able to revert, Are you sure to remove?`,
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

                $.post("/GeneralStoreRequirement/AddOrRemoveFromList", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}