
var attachTo = $("#fGenSPurchaseRequisitionTable");
var submitBtn = $("#btn_submit");
var empId = $("#FGenSPurchaseRequisitionMaster_EMPID");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    },
    1: {
        title: "No Quantity!!.",
        message: "Please Enter Quantity"
    },
    2: {
        title: "Invalid Product!!.",
        message: "Please Enter Valid Product"
    },
    3: {
        title: "No Product Selected!!",
        message: "Please select product first."
    }

}

$(function () {
    var productId = $("#FGenSIndentdetails_PRODUCTID");
    var fGenSUnit = $("#unit");
    var qty = $("#FGenSIndentdetails_QTY");
    var btnAdd = $("#btnAdd");

    getEmpData();

    empId.on("change", function () {
        getEmpData();
    });

    btnAdd.on("click", function () {
        if (productId[0].selectedIndex <= 0) {
            toastr.warning(errors[3].message, errors[3].title);
            return false;
        }
        else if (!parseInt(qty.val()) > 0) {
            toastr.error(errors[1].message, errors[1].title);
            return false;
        }

        var formData = $("#form").serializeArray();
        $.post("/GeneralStoreRequisition/AddOrRemoveFromList",
            formData,
            function (partialView) {
                attachTo.html(partialView);
            }).fail(function () {
                toastr.error(errors[0].title, errors[0].message);
            });

        //var formData = $("#form").serializeArray();

        //var xhr = $.post("/GeneralStoreRequisition/AddOrRemoveFromList", formData, function (partialView) {

        //    if (xhr.getResponseHeader("HasItems") === "True") {
        //        submitBtn.prop("disabled", false);
        //    } else {
        //        submitBtn.prop("disabled", true);
        //    }

        //    attachTo.html(partialView);
        //}).fail(function () {
        //    toastr.error(errors[0].message, errors[0].title);
        //});
    });

    productId.on("change", function () {

        if (productId.val()) {

            qty.html("");

            var formData = {
                "id": productId.val()
            }

            $.get("/GeneralStoreRequisition/GetProductDetails", formData, function (data) {

                fGenSUnit.text(data.unitNavigation.uname);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });
});

function RemoveIndex(index) {
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

                $.post("/GeneralStoreRequisition/AddOrRemoveFromList", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}

function getEmpData() {
    var deptId = $("#deptn");
    var secId = $("#secn");
    //var subSectionId = $("#ssecn");
    var deptH = $("#FGenSPurchaseRequisitionMaster_DEPTID");
    var secH = $("#FGenSPurchaseRequisitionMaster_SECID");
    var ssecH = $("#FGenSPurchaseRequisitionMaster_SSECID");

    if (empId.val()) {

        var formData = {
            "id": empId.val()
        }

        $.get("/GeneralStoreRequisition/GetEmpInfo", formData, function (data) {

            deptId.text(data.dept.deptname);
            secId.text(data.sec.secname);
            //subSectionId.text(data.ssec.ssecname);
            deptH.val(data.deptid);
            secH.val(data.secid);
            ssecH.val(data.ssecid);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}