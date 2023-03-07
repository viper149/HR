var fwpId = $("#FFsWastageIssueD_WPID");
var unit = $("#unitId");
var attachTo = $("#FFsWastageIssueTable");





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

    fwpId.on("change", function () {
        getByfwpId();
    });
});

function getByfwpId() {
    var formData = {
        "wpId": fwpId.val()
    }

    if (fwpId.val()) {
        $.get("/FFsWastageIssue/GetAllByfwpId",
            formData,
            function (data) {
                console.log(data);


                unit.text(data.uname);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
    } else {
        unit.text("");
    }
}

$(function () {
    var btnAdd = $("#addToListBtn");


    btnAdd.on("click",
        function () {
            var prodId = $("#FFsWastageIssueD_WPID");

            if (prodId[0].selectedIndex === 0) {
                toastr.warning(errors[0].message, errors[0].title);
                return false;
            }

            var formData = $("#form").serializeArray();

            $.post("/FFsWastageIssue/AddFfsWastageIssueDetails",
                formData,
                function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
        });
});

function RemoveFFsWastageIssue(index) {
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

                $.post("/FFsWastageIssue/AddFfsWastageIssueDetails", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}