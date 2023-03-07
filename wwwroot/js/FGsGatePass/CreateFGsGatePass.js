var attachTo = $("#gsGatepassInformationDTable");

$(function () {
    var deptid = $('#FGsGatepassInformationM_DEPTID');
    var secid = $('#FGsGatepassInformationM_SECID');
    var productId = $('#FGsGatepassInformationD_PRODID');
    var unit = $('#FGsGatepassInformationD_UNIT_NM');
    const btnAdd = $("#addToList");
    var errors = {
        0: {
            title: "Invalid Submission.",
            message: "We can not process your request. Please try again later."
        }
    }

    deptid.on("change", function () {
        var formData = {
            "id": deptid.val()
        }
        $.get("/FGsGatePass/GetSectionByDeptid", formData, function (data) {
            secid.html("");
            secid.append('<option value="" selected>Select Section</option>');
            $.each(data, function (id, option) {
                secid.append($("<option>",
                    {
                        value: option.secid,
                        text: option.secname
                    }));
            });

        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });

    productId.on("change", function () {
        var formData = {
            "id": productId.val()
        }
        $.get("/FGsGatePass/GetSingleProductByProductId", formData, function (data) {
            unit.val(data.unitNavigation.uname);
        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });

    btnAdd.on("click", function () {

        var formData = $("#form").serializeArray();

        $.post("/FGsGatePass/AddToList", formData, function (partialView) {
            attachTo.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });
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

                $.post("/FGsGatePass/AddToList", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}