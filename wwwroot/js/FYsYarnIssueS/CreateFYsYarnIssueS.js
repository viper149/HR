
var btnAdd = $("#addToList");
var attachTo = $("#yarnIssueDetailsPartialView");
var issueTYpe = $("#YarnIssueMasterS_ISSUEID");
var ysrid = $("#YarnIssueMasterS_YSRID");
var countId = $("#YarnIssueDetailsS_COUNTID");
var refCountId = $("#YarnIssueDetailsS_MAIN_COUNTID");
var lotId = $("#YarnIssueDetailsS_LOTID");
var req_details = $("#YarnIssueDetailsS_REQ_DET_ID");
var yarnReqDD = $("#yarnReqDD ");
var orderNoDiv = $("#orderNoDiv ");
var reqQtyDiv = $("#reqQtyDiv ");
var isreturnableDev = $("#isreturnableDev ");
var purposeDEV = $("#purposeDEV ");
var issuetoDev = $("#issuetoDev ");
//var orderno = $("#YarnIssueDetails_ORDERNO");
var deptnameDiv = $("#deptname ");
var secnameDiv = $("#secname");
//var orderNoId = $("#YarnIssueDetails_TRANS_ORDERNO");
var reqQty = $("#YarnIssueDetailsS_TRANS_REQ_QTY");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request! Please try again later."
    }
}

var removeIndent = function (index) {

    const formData = $("#form").serializeArray();

    formData.push({ name: "RemoveIndex", value: index });
    formData.push({ name: "IsDelete", value: true });

    $.post("/FYsYarnIssueS/AddToList", formData, function (partialView) {
        attachTo.html(partialView);
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}


function GetYarnListOthers() {
    const ysrId = ysrid.val();
    const formData = {
        "ysrId": ysrId
    };
    $.get("/YarnIssue/GetYarnListOthers",
        formData,
        function (data) {
            console.log(data);
            refCountId.html("");
            refCountId.append('<option value="" selected>Select Count Name</option>');
            var table = "";
            $.each(data,
                function (index, value) {
                    table += `<tr>
                    <td>${value.sO_NO}</td>
                    <td>${value.countname}</td>
                    <td>${value.rnD_COUNTNAME}</td>
                    <td>${value.reQ_QTY}</td>
                    </tr>`;

                    refCountId.append($("<option>",
                        {
                            value: value.countid,
                            text: `${value.sO_NO} - ${value.countname} - Qty: ${value.reQ_QTY}`
                        }));
                });
            $("#countlist").html(table);
        });
}

//function GetYarnList() {
//    // 300001 => Loan
//    // 300002 => Inspection
//    if (issueTYpe.val() === "300001" || issueTYpe.val() === "300002") {
//        //orderno.val("");
//        reqQty.val("");
//        orderNoDiv.addClass("d-none");
//        reqQtyDiv.addClass("d-none");
//        yarnReqDD.addClass("d-none");
//        deptnameDiv.addClass("d-none");
//        secnameDiv.addClass("d-none");
//        isreturnableDev.removeClass("d-none");
//        purposeDEV.removeClass("d-none");
//        issuetoDev.removeClass("d-none");

//        $.get("/YarnIssue/GetBasCountList", null, function (data) {

//            refCountId.html("");
//            refCountId.append('<option value="" selected>Select Count Name</option>');

//            $.each(data, function (index, value) {
//                refCountId.append($("<option>",
//                    {
//                        value: value.countid,
//                        text: `${value.countname}`
//                    }));
//            });

//        }).fail(function () {
//            toastr.warning(errors[0].message, errors[0].title);
//        });

//    } else {
//        //orderno.html("");
//        //orderno.append('<option value="" selected>Select Product</option>');
//        //orderNoDiv.removeClass("d-none");
//        reqQtyDiv.removeClass("d-none");
//        yarnReqDD.removeClass("d-none");
//        deptnameDiv.removeClass("d-none");
//        secnameDiv.removeClass("d-none");
//        isreturnableDev.addClass("d-none");
//        purposeDEV.addClass("d-none");
//        issuetoDev.addClass("d-none");
//        var ysridval = ysrid.val();
//        console.log(ysridval);
//        $.get("/YarnIssueS/GetYarnList", null, function (data) {
//            ysrid.html("");
//            ysrid.append('<option value="" selected>Select Requirement No.</option>');
//            $.each(data, function (index, value) {
//                ysrid.append($("<option>",
//                    {
//                        value: value.ysrid,
//                        text: `${value.ysrno}`
//                    }));
//            });

//            ysrid.val(ysridval).trigger("change");
//        }).fail(function () {
//            toastr.warning(errors[0].message, errors[0].title);
//        });
//    }
//}

$(function () {
    var pathname = window.location.pathname;
    if (!pathname.includes("Edit")) {
        GetYarnList();
    }
    GetYarnListOthers();

    btnAdd.on("click", function () {
        const formData = $("#form").serializeArray();
        $.post("/FYsYarnIssueS/AddToList", formData, function (partialView, status, xhr) {
            attachTo.html(partialView);
            const addStatus = xhr.getResponseHeader("Status");
            if (addStatus === "Error") {
                toastr.error("Duplicate Entry", "Error!");
            }
            if (addStatus === "Stock Alert") {
                toastr.error("Stock Alert!", "Error!");
            } else {
                toastr.success("Successfully Add/Delete Count", "Success");
            }
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });

    issueTYpe.on("change", function () {
        GetYarnList();
    });

    ysrid.on("change", function () {
        GetYarnListOthers();
    });


    refCountId.on("change",
        function () {
            countId.val(refCountId.val()).trigger("change");
        });

    refCountId.on("change", function () {
        var d = refCountId.select2('data');
        var count = d[0].text;
        var qty = $.trim(count.substr(count.indexOf(": ") + 1));

        const formData = {
            'countId': $(this).val(),
            'qty': qty
        };
        if (issueTYpe.val() === "300000") {
            $.get("/FYsYarnIssueS/GetYarnReqDetails", formData, function (data) {

                if (typeof data !== "undefined") {
                    reqQty.val(qty);
                    req_details.val(data.trnsid);
                }
            }).fail(function () {
                //toastr.warning(errors[0].message, errors[0].title);
            });
        }

        $.get("/FYsYarnIssueS/GetYarnLotDetails", formData, function (data) {
            if (typeof data !== "undefined") {
                lotId.html("");
                lotId.append('<option value="" selected>Select Lot No</option>');
                $.each(data, function (index, value) {
                    lotId.append($("<option>",
                        {
                            value: value.lot.lotid,
                            text: `${value.lot.lotno} - ${value.lot.brand} - ${value.balance} Kg`
                        }));
                });
            } else {
                toastr.warning(errors[0].message, errors[0].title);
            }
        }).fail(function () {
            toastr.warning(errors[0].message, errors[0].title);
        });
    });

    countId.on("change", function () {
        var d = countId.select2('data');
        var count = d[0].text;
        var qty = $.trim(count.substr(count.indexOf(": ") + 1));

        const formData = {
            'countId': $(this).val(),
            'qty': qty
        };

        $.get("/FYsYarnIssueS/GetYarnLotDetails", formData, function (data) {
            if (typeof data !== "undefined") {
                lotId.html("");
                lotId.append('<option value="" selected>Select Lot No</option>');
                $.each(data, function (index, value) {
                    lotId.append($("<option>",
                        {
                            value: value.lot.lotid,
                            text: `${value.lot.lotno} - ${value.lot.brand} - ${value.balance} Kg`
                        }));
                });
            } else {
                toastr.warning(errors[0].message, errors[0].title);
            }
        }).fail(function () {
            toastr.warning(errors[0].message, errors[0].title);
        });
    });
});