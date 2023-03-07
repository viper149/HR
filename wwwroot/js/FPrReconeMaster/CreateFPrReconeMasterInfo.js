var setId = $("#F_PR_RECONE_MASTER_SET_NO");
/*var unit = $("#unitId");*/
var attachTo = $("#FPrReconeDetailsTable");
var attachTo = $("#FPrConsumptionTable");






var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    },
    1: {
        title: "No Qty Selected!!",
        message: "Please Select Qty First."
    }
}

//$(function () {
//    getBysetId();
//    wpId.on("change", function () {
//        getBysetId();
//    });
//});

//function getBysetId() {
//    var formData = {
//        "setId": setId.val()
//    }

//    if (setId.val()) {
//        $.get("/FPrReconeMaster/GetAllBysetId",
//            formData,
//            function (data) {
//                console.log(data);


//                unit.text(data.uname);

//            }).fail(function () {
//                toastr.error(errors[0].message, errors[0].title);
//            });
//    } else {
//        unit.text("");
//    }
//}

$(function () {
    var btnAdd = $("#addToListBtn");


    btnAdd.on("click",
        function () {
            var reconeSetId = $("#F_PR_RECONE_MASTER_SET_NO");
            if (reconeSetId[0].selectedIndex === 0) {
                toastr.warning(errors[0].message, errors[0].title);
                return false;
            }

            var formData = $("#form").serializeArray();

            $.post("/FPrReconeMaster/AddFPrReconeDetails",
                formData,
                function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
        });
});

$(function () {
    var btnAddYC = $("#addToListBtnYC");


    btnAddYC.on("click",
        function () {
            var reconeSetId = $("#F_PR_RECONE_MASTER_SET_NO");
            if (reconeSetId[0].selectedIndex === 0) {
                toastr.warning(errors[0].message, errors[0].title);
                return false;
            }

            var formData = $("#form").serializeArray();

            $.post("/FPrReconeMaster/AddFPrConsumptionDetails",
                formData,
                function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                toastr.error(errors[0].title, errors[0].message);
            });
        });
});

function RemoveFPrReconeDetails(index) {
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

                $.post("/FPrReconeMaster/AddFPrReconeDetails", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}

function RemoveFPrYarnConsumption(index) {
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

                $.post("/FPrReconeMaster/AddFPrConsumptionDetails", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}