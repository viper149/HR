
var attachTo = $("#chemIssueDetailsPartialView");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

$(function () {

    var issueId = $("#FChemIssueMaster_ISSUEID");
    var csrid = $("#FChemIssueMaster_CSRID");

    var chemReqDDDiv = $("#chemReqDDDiv ");
    var isreturnableDiv = $("#isreturnableDiv ");
    var purposeDiv = $("#purposeDiv ");
    var issuetoDiv = $("#issuetoDiv ");

    var unit = $("#unit_chem");
    var req_Qty = $("#req_qty ");
    var creqDetId = $("#FChemIssueDetails_CREQ_DET_ID ");

    var csrdateDiv = $("#csrdateDiv ");
    var deptnameDiv = $("#deptnameDiv ");
    var secnameDiv = $("#secnameDiv");

    var csrdate = $("#FChemReqMaster_CSRDATE ");
    var deptname = $("#FChemReqMaster_DEPT_DEPTNAME");
    var secname = $("#FChemReqMaster_SECNAME");
    var dyeingType = $("#FChemReqMaster_D_DTYPE");

    var productid = $("#FChemIssueDetails_PRODUCTID");
    var batchNo = $("#FChemIssueDetails_CRCVIDD");
    var remainingAmountByBatch = $("#FChemIssueDetails_REMAINING_AMOUNT");

    var btnAdd = $("#addToList");

    var issueQty = $("#FChemIssueDetails_ISSUE_QTY");
    var adjuctWith = $("#adjust");

    req_Qty.change("", function () {
        
    });

    issueId.on("change", function () {

        if (issueId.val() === "300000") {
            adjuctWith.hide();
        } else {
            adjuctWith.show();
        }

        if (issueId.val() === "300001") {

            chemReqDDDiv.addClass("d-none");
            csrdateDiv.addClass("d-none");
            deptnameDiv.addClass("d-none");
            secnameDiv.addClass("d-none");

            csrdate.val("");
            deptname.val("");
            secname.val("");
            dyeingType.val("");

            isreturnableDiv.removeClass("d-none");
            purposeDiv.removeClass("d-none");
            issuetoDiv.removeClass("d-none");

            $.get("/ChemicalIssue/GetProducts", {}, function (data) {

                console.log(data);

                productid.html("");
                productid.append('<option value="" selected>Select Product</option>');

                $.each(data, function (id, option) {
                    productid.append($("<option>",
                        {
                            value: option.trnsid,
                            text: option.productname
                        }));
                });

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });

        } else {

            productid.html("");
            productid.append('<option value="" selected>Select Product</option>');

            chemReqDDDiv.removeClass("d-none");

            csrdateDiv.removeClass("d-none");
            deptnameDiv.removeClass("d-none");
            secnameDiv.removeClass("d-none");

            isreturnableDiv.addClass("d-none");
            purposeDiv.addClass("d-none");
            issuetoDiv.addClass("d-none");

            $.get("/ChemicalIssue/GetRequirementDD", {}, function (data) {

                var pathname = window.location.pathname;
                if (!pathname.includes("Edit")) {
                    csrid.html("");
                    csrid.append('<option value="" selected>Select Requirement Number</option>');

                    $.each(data,
                        function(index, option) {
                            csrid.append($("<option>",
                                {
                                    value: option.csrid,
                                    text: option.csrno
                                }));
                        });
                }

            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        }
    });

    csrid.on("change", function () {

        var formData = {
            "id": csrid.val()
        }
        
        $.get("/ChemicalIssue/GetChemicalMasterList", formData, function (data) {

            csrdate.val(new Date(data[0].fChemReqMaster.csrdate).toISOString().slice(0, 10));
            deptname.val(data[0].fChemReqMaster.dept.deptname);
            secname.val(data[0].fChemReqMaster.secname);

            productid.html("");
            productid.append('<option value="" selected>Select Product</option>');

            $.each(data[0].fChemReqDetailsesList, function (id, option) {
                productid.append($("<option>",
                    {
                        value: option.product.productid,
                        text: option.product.productname
                    }));
            });

        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });

    productid.on("change", function () {

        var formData = {
            'csrid': csrid.val(),
            'productid': productid.val()
        }

        $.get("/ChemicalIssue/GetSingleChemReqDetailsAsync", formData, function (data) {
            console.log(data);
            if (issueId.val() !== "300001") {

                unit.text(data[0].fChemStoreProductinfo.unitnavigation.uname);

                $.each(data[0].fChemStoreProductinfo.f_CHEM_REQ_DETAILS, function (index, option) {
                    //console.log(option.csrid);
                    //console.log(option.csrid);
                    //console.log($("#FChemIssueMaster_CSRID").val());
                    if (option.csrid == $("#FChemIssueMaster_CSRID").val()) {
                        req_Qty.text(option.reQ_QTY);
                        creqDetId.val(option.crqid);
                    }
                });
            }

            remainingAmountByBatch.val("");

            batchNo.html("");
            batchNo.append('<option value="" selected>Select Batch</option>');

            console.log(data);

            $.each(data, function (index, option) {

                batchNo.append($("<option>",
                    {
                        value: option.trnsid,
                        text: `${option.batchno}`
                    }));
            });

        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });

    batchNo.on("change", function () {

        var formData = {
            'productId': productid.val(),
            'batchNo': batchNo.children("option").filter(":selected").text()
        }

        $.get("/ChemicalIssue/GetRemainingBalanceByBatchId", formData, function (data) {
            remainingAmountByBatch.val(data);
            issueQty.attr({
                "max": data
            });

        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });

    btnAdd.on("click", function () {

        var formData = $("#form").serializeArray();
        formData.push({ name: "BB", value: batchNo.val() });

        $.post("/ChemicalIssue/AddOrRemoveFromChemIssueDetails", formData, function (partialView) {
            attachTo.html(partialView);
            var remaining = remainingAmountByBatch.val() - issueQty.val();
            remainingAmountByBatch.val(remaining);
            issueQty.val("");
            $("#FChemIssueDetails_REMARKS").val("");
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

                var formData = $("#form").serializeArray();

                formData.push({ name: "IsDelete", value: true });
                formData.push({ name: "RemoveIndex", value: index });

                $.post("/ChemicalIssue/AddOrRemoveFromChemIssueDetails", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }
        });
}