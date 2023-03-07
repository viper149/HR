
var attachTo = $("#fGenSDetailsTable");
var submitBtn = $("#btn_submit");
var indslId = $("#FGenSIndentmaster_INDSLID");
var fGenSProductId = $("#FGenSIndentdetails_PRODUCTID");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    },
    1: {
        title: "Issue Quantity Exceeded!!.",
        message: "Issue Quantity is greater than Requested Quantity!! Please decrease your Issue Quantity"
    },
    2: {
        title: "No Quantity!!.",
        message: "Please Enter Quantity"
    },
    3: {
        title: "No Product Selected!!",
        message: "Please select product first."
    },
    4: {
        title: "No Purchase Requisition Selected!!",
        message: "Please select purchase requisition no. first."
    },
}

$(function () {
    var fGenSQty = $("#FGenSIndentdetails_QTY");
    var fIndQty = $("#FGenSIndentdetails_FULL_QTY");
    var fGenSUnit = $("#unit");
    var lastInd = $("#FGenSIndentdetails_LASTIND");
    var lastIndDate = $("#FGenSIndentdetails_LASTIND_DATE");
    var indentDetailsIndslId = $("#FGenSIndentdetails_INDSLID");
    var indentDetailsTrnsId = $("#FGenSIndentdetails_TRNSID");
    var btnAdd = $("#btnAdd");

    getRequisition();

    indslId.on("change", function () {
        getRequisition();
    });

    fGenSProductId.on("change", function () {

        if (indslId.val() && fGenSProductId.val()) {

            const formData = {
                "id": indslId.val(),
                "prdId": fGenSProductId.val()
            };

            $.post("/GeneralStoreIndent/GetFGenSIndentDetails/", formData, function (data) {
                indentDetailsIndslId.val(data.indslid);
                indentDetailsTrnsId.val(data.trnsid);
                fGenSQty.val(data.qty);
                fGenSUnit.text(data.product.unitNavigation.uname);
                lastInd.text(data.lastind);
                lastIndDate.text(data.lastinD_DATE);

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }
    });

    btnAdd.on("click", function () {
        if (indslId[0].selectedIndex <= 0) {
            toastr.warning(errors[4].message, errors[4].title);
            return false;
        }
        else if (fGenSProductId[0].selectedIndex <= 0) {
            toastr.warning(errors[3].message, errors[3].title);
            return false;
        }
        else if (parseInt(fIndQty.val()) > parseInt(fGenSQty.val())) {
            toastr.error(errors[1].message, errors[1].title);
            return false;
        }
        else if (!parseInt(fIndQty.val()) > 0) {
            toastr.error(errors[2].message, errors[2].title);
            return false;
        }

        var formData = $("#form").serializeArray();

        $.post("/GeneralStoreIndent/AddOrRemoveFGenSIndentDetailsList", formData, function (partialView) {
            attachTo.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });
});

function getRequisition() {
    if (indslId.val()) {
        var indentBy = $("#FGenSIndentmaster_CN_PERSON");
        var indentTo = $("#FGenSIndentmaster_EMP");

        var formData = {
            "id": indslId.val()
        };

        $.post("/GeneralStoreIndent/GetFGenSRequirements",
            formData,
            function (data) {
                console.log(data);

                attachTo.html("");

                //dept_sec.text(`${data.dept.deptname} (${data.sec.secname})`);
                indentBy.text(data.cN_PERSONNavigation.firsT_NAME);
                indentTo.text(data.emp.firsT_NAME);

                fGenSProductId.html("");
                fGenSProductId.append('<option value="" selected>Select Product</option>');

                $.each(data.f_GEN_S_INDENTDETAILS,
                    function (index, option) {
                        fGenSProductId.append($("<option>",
                            {
                                value: option.product.prodid,
                                text: `${option.product.prodid} - ${option.product.prodname} - ${option.product.partno}`
                            }));
                    });

            }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

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

                $.post("/GeneralStoreIndent/AddOrRemoveFGenSIndentDetailsList", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }
        });
}