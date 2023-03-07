
var attachTo = $("#chemReqDetailsTable");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

$(function () {

    var deptId = $("#FChemReqMaster_DEPTID");
    var secId = $("#FChemReqMaster_SECID");
    var subSectionId = $("#FChemReqMaster_SSECID");
    var productId = $("#FChemReqDetails_PRODUCTID");
    var unit = $("#FChemReqDetails_PRODUCT_UNITNAVIGATION_UNAME");
    var btnAdd = $("#addToList");
    var reqQty = $("#FChemReqDetails_REQ_QTY");

    secId.on("change", function () {

        var formData = {
            "sectionId": secId.val()
        }

        $.get("/ChemicalRequirement/GetSubSections", formData, function (data) {

            subSectionId.html("");
            subSectionId.append('<option value="" selected>Select Sub Section</option>');

            $.each(data, function (id, option) {
                subSectionId.append($("<option>",
                    {
                        value: option.ssecid,
                        text: option.ssecname
                    }));
            });

        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });

    btnAdd.on("click", function () {

        if ($("#FChemReqDetails_PRODUCTID").val() === "" || $("#FChemReqDetails_PRODUCTID").val() === null) {
            toastrNotification('Please Select Product First', 'error');
            return false;
        }

        var formData = $("#form").serializeArray();

        $.post("/ChemicalRequirement/AddOrRemoveFromList", formData, function (partialView) {
            attachTo.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });

    deptId.on("change", function () {

        var formData = {
            "id": deptId.val()
        }

        $.get("/ChemicalRequirement/GetSections", formData, function (data) {

            secId.html("");
            secId.append('<option value="" selected>Select Section</option>');

            $.each(data, function (id, option) {
                secId.append($("<option>",
                    {
                        value: option.secid,
                        text: option.secname
                    }));
            });
        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });

    //productId.on("change", function () {

    //    var formData = {
    //        "id": productId.val()
    //    }

    //    $.get("/ChemicalRequirement/GetUnitWithBalance", formData, function (data) {

    //        unit.val(data.uname);
    //        reqQty.attr({
    //            "max": data.balance
    //        });

    //    }).fail(function () {
    //        toastr.error(errors[0].title, errors[0].message);
    //    });
    //});

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

                $.post("/ChemicalRequirement/AddOrRemoveFromList", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}