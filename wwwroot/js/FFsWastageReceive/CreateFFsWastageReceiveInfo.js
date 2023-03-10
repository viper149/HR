var fwpId = $("#FFsWastageReceiveD_WPID");
var unitId = $("#unitId");
var attachTo = $("#FFsWastageRcvTable");





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

$(function () {
    getByfwpId();
    fwpId.on("change", function () {
        getByfwpId();
    });
});

function getByfwpId() {
    var formData = {
        "fwpId": fwpId.val()
    }

    if (fwpId.val()) {
        $.get("/FFsWastageReceive/GetAllBywpId",
            formData,
            function (data) {
                console.log(data);


                unitId.text(data.uname);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
    } else {
        unitId.text("");
    }
}

$(function () {
    var btnAdd = $("#addToListBtn");


    btnAdd.on("click",
        function () {
            var fsprodId = $("#FFsWastageReceiveD_WPID");
            //var prdQtyId = $("#FGsWastageReceiveD_RCV_QTY");

            if (fsprodId[0].selectedIndex === 0) {
                toastr.warning(errors[0].message, errors[0].title);
                return false;
            }

            var formData = $("#form").serializeArray();

            $.post("/FFsWastageReceive/AddFfsWastageRcvDetails",
                formData,
                function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
        });
});

function RemoveFFsWastageRcv(index) {
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

                $.post("/FFsWastageReceive/AddFfsWastageRcvDetails", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}