
var attachTo = $("#fGenSIssueDetailsTable");
var submitBtn = $("#btn_submit");
var gsrid = $("#FGenSIssueMaster_GSRID");
var productid = $("#FGenSIssueDetails_PRODUCTID");


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
    }
}

$(function () {
    var issueId = $("#FGenSIssueMaster_ISSUEID");
    var unit = $("#unit");
    var req_Qty = $("#req_qty ");
    var issueQty = $("#FGenSIssueDetails_ISSUE_QTY ");
    var gsreqDetId = $("#FGenSIssueDetails_GREQ_DET_ID ");
    var lastBalance = $("#balance");
    var btnAdd = $("#addToList");

    getByGsrId();

    issueId.on("change",
        function () {

            productid.html("");
            productid.append('<option value="" selected>Select Product</option>');

            $.get("/GeneralStoreIssue/GetRequirementDD",
                {},
                function (data) {

                    gsrid.html("");
                    gsrid.append('<option value="" selected>Select Requirement No</option>');

                    $.each(data,
                        function (index, option) {
                            gsrid.append($("<option>",
                                {
                                    value: option.gsrid,
                                    text: option.gsrno
                                }));
                        });

                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
        });

    gsrid.on("change", function () {
        getByGsrId();
    });

    productid.on("change", function () {

        var formData = {
            'gsrid': gsrid.val(),
            'productid': productid.val()
        };
        var proId = {
            'productId': productid.val()
        };

        $.get("/GeneralStoreIssue/GetFGenSReqDetails",
            formData,
            function (data) {
                unit.text(data.product.unitNavigation.uname);
                req_Qty.text(data.reQ_QTY);
                gsreqDetId.val(data.grqid);
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });

        $.get("/GeneralStoreIssue/GetBalance",
            proId,
            function (data) {
                lastBalance.text(data.balance);
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
    });

    btnAdd.on("click", function () {
        if (productid[0].selectedIndex <= 0) {
            toastr.warning(errors[3].message, errors[3].title);
            return false;
        }
        if (parseInt(issueQty.val()) > parseInt(req_Qty.text())) {
            toastr.error(errors[1].message, errors[1].title);
            return false;
        }
        else if (!parseInt(issueQty.val()) > 0) {
            toastr.error(errors[2].message, errors[2].title);
            return false;
        }

        var formData = $("#form").serializeArray();

        $.post("/GeneralStoreIssue/AddOrRemoveFromDetails", formData,
            function (partialView) {
                attachTo.html(partialView);
            }).fail(function () {
                toastr.error(errors[0].title, errors[0].message);
            });

        //if (productId.val()) {

        //    const formData = $("#form").serializeArray();

        //    var xhr = $.post("/GeneralStoreIssue/AddOrRemoveFromDetails", formData, function (partialView) {

        //        if (xhr.getResponseHeader("HasItems") === "True") {
        //            submitBtn.prop("disabled", false);
        //        } else {
        //            submitBtn.prop("disabled", true);
        //        }
        //        attachTo.html(partialView);
        //    }).fail(function () {
        //        toastr.error(errors[0].message, errors[0].title);
        //    });
        //} else {
        //    toastr.error(errors[0].message, errors[0].title);
        //}
    });
});

function getByGsrId() {
    if (gsrid.val()) {
        var gsrdate = $("#FGenSReqMaster_GSRDATE ");
        var reqBy = $("#reqEmp");
        var deptname = $("#deptn");
        var secname = $("#secn");
        var formData = {
            "id": gsrid.val()
        };

        $.get("/GeneralStoreIssue/GetFGenSMasterList",
            formData,
            function (data) {
                console.log(data);
                gsrdate.val(new Date(data[0].fGenSReqMaster.gsrdate).toISOString().slice(0, 10));
                reqBy.text(data[0].fGenSReqMaster.emp.empno);
                deptname.text(data[0].fGenSReqMaster.emp.f_HR_EMP_OFFICIALINFO[0].dept.deptname);
                secname.text(data[0].fGenSReqMaster.emp.f_HR_EMP_OFFICIALINFO[0].sec.secname);

                productid.html("");
                productid.append('<option value="" selected>Select Product</option>');

                $.each(data[0].fGenSReqDetailsesList,
                    function (id, option) {
                        productid.append($("<option>",
                            {
                                value: option.product.prodid,
                                text: option.product.prodname
                            }));
                    });
            }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

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

                $.post("/GeneralStoreIssue/AddOrRemoveFromDetails", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}