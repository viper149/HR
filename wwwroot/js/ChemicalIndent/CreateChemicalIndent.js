
var attachTo = $("#chemicalDetailsTable");

$(function () {

    var indslId = $("#FChemStoreIndentmaster_INDSLID");
    var dept_sec = $("#FChemStoreIndentmaster_DEPT_SEC");
    var indentBy = $("#FChemStoreIndentmaster_INDENTBY");
    var indentTo = $("#FChemStoreIndentmaster_INDENTTO");

    var chemProductId = $("#FChemStoreIndentdetails_PRODUCTID");
    var chemUnit = $("#FChemStoreIndentdetails_UNIT");
    var chemQty = $("#FChemStoreIndentdetails_QTY");
    var lastInd = $("#FChemStoreIndentdetails_LASTIND");
    var lastIndDate = $("#FChemStoreIndentdetails_LASTIND_DATE");
    var indentDetailsIndslId = $("#FChemStoreIndentdetails_INDSLID");
    var indentDetailsTrnsId = $("#FChemStoreIndentdetails_TRNSID");
    var unitName = $("#FChemStoreIndentdetails_FBasUnits_UNAME");
    var btnAdd = $("#btnAdd");

    var errors = {
        0: {
            title: "Invalid Submission.",
            message: "We can not process your request. Please try again later."
        }
    }

    indslId.on("change", function () {

        if (indslId.val()) {

            const formData = {
                "id": indslId.val()
            };

            $.post("/ChemicalIndent/GetChemicalRequirements",
                formData,
                function (data) {

                    attachTo.html("");

                    dept_sec.val(`${data.fBasDepartment.deptname} (${data.fBasSection.secname})`);
                    indentBy.val(data.concernEmployee.f_NAME);
                    indentTo.val(data.employee.f_NAME);

                    chemProductId.html("");
                    chemProductId.append('<option value="" selected>Select Product</option>');

                    $.each(data.f_CHEM_STORE_INDENTDETAILS,
                        function (index, option) {

                            chemProductId.append($("<option>",
                                {
                                    value: option.product.productid,
                                    text: option.product.productname
                                }));
                        });

                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });

    chemProductId.on("change", function () {

        if (indslId.val() && chemProductId.val()) {

            const formData = {
                "id": indslId.val(),
                "prdId": chemProductId.val()
            };

            $.post("/ChemicalIndent/GetChemicalIndentDetails/", formData, function (data) {
                indentDetailsIndslId.val(data.indslid);
                indentDetailsTrnsId.val(data.trnsid);
                chemUnit.val(data.unit);
                unitName.val(data.fBasUnits.uname);
                chemQty.val(data.qty);
                lastInd.val(data.lastind);
                lastIndDate.val(data.lastinD_DATE);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });

    btnAdd.on("click", function () {

        var formData = $("#form").serializeArray();

        $.post("/ChemicalIndent/AddOrRemoveChemIndentDetailsList", formData, function (partialView) {
            attachTo.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
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

                const formData = $("#form").serializeArray();
                formData.push({ name: "RemoveIndex", value: index });
                formData.push({ name: "IsDelete", value: true });

                $.post("/ChemicalIndent/AddOrRemoveChemIndentDetailsList", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }
        });
}