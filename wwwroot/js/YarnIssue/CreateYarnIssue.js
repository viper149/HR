var btnAdd = $("#addToList");
var attachTo = $("#yarnIssueDetailsPartialView");
var issueTYpe = $("#YarnIssueMaster_ISSUEID");
var ysrid = $("#YarnIssueMaster_YSRID");
var countId = $("#YarnIssueDetails_COUNTID");
var refCountId = $("#YarnIssueDetails_MAIN_COUNTID");
var reqDId = $("#YarnIssueDetails_REQ_DET_ID");
var lotId = $("#YarnIssueDetails_LOTID");
//var req_details = $("#YarnIssueDetails_REQ_DET_ID");
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
var reqQty = $("#YarnIssueDetails_TRANS_REQ_QTY");
var rcvId = $("#YarnIssueDetails_RCVDID");
var soNo = $("#YarnIssueDetails_SO_NO");


var errors = {
    0: {
        title: "Invalid Submission!",
        message: "We can not process your request! Please try again later."
    },
    1: {
        title: "No Indent Selected!",
        message: "Please Select Indent First."
    },
    2: {
        title: "No Main Count Selected!",
        message: "Please Select Main Count First."
    }
}

var removeIndent = function (index) {

    const formData = $("#form").serializeArray();

    formData.push({ name: "RemoveIndex", value: index });
    formData.push({ name: "IsDelete", value: true });

    $.post("/FYsYarnIssue/AddToList", formData, function (partialView) {
        attachTo.html(partialView);
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}

function GetYarnListOthers() {
    const ysrId = ysrid.val();
    if (ysrid.val()) {

        const formData = {
            "ysrId": ysrId
        };
        $.get("/YarnIssue/GetYarnListOthers",
            formData,
            function (data) {
                refCountId.html("");
                refCountId.append('<option value="" selected>Select Count Name</option>');
                reqDId.html("");
                reqDId.append('<option value="" selected>Select Count Name</option>');
                var table = "";
                var sum = 0;


                table += `<tr>

                   <th>Order No</th>
                   <th>Count Name</th>
                   <th>Required Lot</th>
                   <th>Required Qty</th>
                  </tr>`;

                $.each(data,
                    function (index, value) {
                        table += `<tr>
                    <td>${value.sO_NO}</td>                    
                    <td>${value.rnD_COUNTNAME}</td>
                    <td>${value.lot}</td>
                    <td>${value.reQ_QTY}</td>
                    </tr>`;
                        sum += value.reQ_QTY;
                        reqDId.append($("<option>",
                            {
                                value: value.trnsid,
                                text: `${value.sO_NO} - ${value.rnD_COUNTNAME} - Qty: ${value.reQ_QTY}`
                            }));
                        refCountId.append($("<option>",
                            {
                                value: value.countid,
                                text: `${value.sO_NO} - ${value.rnD_COUNTNAME} - Qty: ${value.reQ_QTY}`
                            }));
                    });


                table += `<tr class="bg-info">
                    <td></td>
                    <td></td>
                    <td>Sum Of Total :</td>
                    <td>${sum}</td>
                    </tr>`;
                $("#countlist").html(table);
            });

    }
 
}

function GetYarnList() {
    // 300001 => Loan
    // 300002 => Inspection
    if (issueTYpe.val() === "300001" || issueTYpe.val() === "300002") {
        //orderno.val("");
        reqQty.val("");
        orderNoDiv.addClass("d-none");
        reqQtyDiv.addClass("d-none");
        yarnReqDD.addClass("d-none");
        deptnameDiv.addClass("d-none");
        secnameDiv.addClass("d-none");
        isreturnableDev.removeClass("d-none");
        purposeDEV.removeClass("d-none");
        issuetoDev.removeClass("d-none");

        $.get("/YarnIssue/GetBasCountList", null, function (data) {

            refCountId.html("");
            refCountId.append('<option value="" selected>Select Count Name</option>');

            $.each(data, function (index, value) {
                refCountId.append($("<option>",
                    {
                        value: value.countid,
                        text: `${value.rnD_COUNTNAME}`
                    }));
            });

        }).fail(function () {
            toastr.warning(errors[0].message, errors[0].title);
        });

    } else {
        //orderno.html("");
        //orderno.append('<option value="" selected>Select Product</option>');
        //orderNoDiv.removeClass("d-none");
        reqQtyDiv.removeClass("d-none");
        yarnReqDD.removeClass("d-none");
        deptnameDiv.removeClass("d-none");
        secnameDiv.removeClass("d-none");
        isreturnableDev.addClass("d-none");
        purposeDEV.addClass("d-none");
        issuetoDev.addClass("d-none");
        var ysridval = ysrid.val();

        $.get("/YarnIssue/GetYarnList", null, function (data) {
            ysrid.html("");
            ysrid.append('<option value="" selected>Select Requirement No.</option>');
            $.each(data, function (index, value) {
                ysrid.append($("<option>",
                    {
                        value: value.ysrid,
                        text: `${value.ysrno}`
                    }));
            });

            /*ysrid.val(ysridval).trigger("change");*/
        }).fail(function () {
            toastr.warning(errors[0].message, errors[0].title);
        });
    }
}

$(function () {
    //var pathname = window.location.pathname;
    //if (!pathname.includes("Edit")) {
    //    GetYarnList();
    //}

   GetYarnListOthers();

    rcvId.on("change",
        function () {
            if (rcvId.val()) {
                var formData = {
                    "id": rcvId.val()
                }
                $.get("/FYsYarnIssue/GetAllByIndent",
                    formData,
                    function (data) {
                        lotId.val(data.lot);
                    }).fail(function () {
                        toastr.error(errors[0].message, errors[0].title);
                    });
            }
        });

    btnAdd.on("click", function () {

        if (reqDId[0].selectedIndex <= 0) {
            toastr.warning(errors[2].message, errors[2].title);
            return false;
        }
        if (rcvId[0].selectedIndex <= 0) {
            toastr.warning(errors[1].message, errors[1].title);
            return false;
        }

        const formData = $("#form").serializeArray();
        $.post("/FYsYarnIssue/AddToList", formData, function (partialView, status, xhr) {
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



    function getValueByYsrid() {

        GetYarnListOthers();
        if (ysrid.val()) {

            const formData = {
                "issueDate": $("#YarnIssueMaster_YISSUEDATE").val()
            };

            $.get("/FYsYarnIssue/GetCount", formData, function (data) {
                countId.html("");
                countId.append('<option value="" selected>Select Count</option>');

                $.each(data, function (index, option) {
                    countId.append($("<option>",
                        {
                            value: option.prod.countid,
                            text: option.prod.rnD_COUNTNAME
                        }));
                });
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        } else {
            toastr.error(errors[0].message, errors[0].title);
        }

    }


    ysrid.on("change", function () {
        getValueByYsrid();
    });


    refCountId.on("change",
        function () {
            countId.val(refCountId.val()).trigger("change");
        });

    //refCountId.on("change",
    //    function() {
    //        var d = refCountId.select2('data');
    //        var count = d[0].text;
    //        var qty = $.trim(count.substr(count.indexOf(": ") + 1));

    //        const formData = {
    //            'countId': $(this).val(),
    //            'qty': qty
    //        };
    //        if (issueTYpe.val() === "300000") {
    //            $.get("/YarnIssue/GetYarnReqDetails",
    //                formData,
    //                function(data) {

    //                    if (typeof data !== "undefined") {
    //                        reqQty.val(qty);
    //                        req_details.val(data.trnsid);
    //                    } else {
    //                        //toastr.warning(errors[0].message, errors[0].title);
    //                    }
    //                }).fail(function() {
    //                //toastr.warning(errors[0].message, errors[0].title);
    //            });
    //        }
    //    });


    reqDId.on("change", function () {
            $.get("/FYsYarnIssue/GetCountIdByReqDId",
                { 'reqId': $(this).val() }, function (data) {
                    if (typeof data !== "undefined") {
                        refCountId.val(data).trigger("change");
                    }
                });
    });

    reqDId.on("change", function () {
        var d = reqDId.select2('data');
        var count = d[0].text;
        var qty = $.trim(count.substr(count.indexOf(": ") + 1));

        reqQty.val(qty);

        //$.get("/FYsYarnIssue/GetYarnLotDetailsByReqId",
        //    { "countId": $(this).val() }, function (data) {
        //        if (typeof data !== "undefined") {
        //            lotId.html("");
        //            lotId.append('<option value="" selected>Select Lot No</option>');
        //            $.each(data, function (index, value) {
        //                lotId.append($("<option>",
        //                    {
        //                        value: value.lot.lotid,
        //                        text: `${value.lot.lotno} - ${value.lot.brand} - ${value.balance} Kg`
        //                    }));
        //            });
        //        } else {
        //            toastr.warning(errors[0].message, errors[0].title);
        //        }
        //    }).fail(function () {
        //        toastr.warning(errors[0].message, errors[0].title);
        //    });
    });

    countId.on("change", function () {
        var d = countId.select2('data');
        var count = d[0].text;
        var qty = $.trim(count.substr(count.indexOf(": ") + 1));

        const formData = {
            'countId': countId.val(),
            /*'qty': qty,*/
            "indentType": $("#YarnIssueMaster_INDENT_TYPE").val(),
            "issueDate": $("#YarnIssueMaster_YISSUEDATE").val()
        };

        //$.get("/FYsYarnIssue/GetYarnLotDetailsByCount", formData, function (data) {
        //    if (typeof data !== "undefined") {
        //        lotId.html("");
        //        lotId.append('<option value="" selected>Select Lot No</option>');
        //        $.each(data, function (index, value) {
        //            lotId.append($("<option>",
        //                {
        //                    value: value.lot.lotid,
        //                    text: `${value.lot.lotno} - ${value.lot.brand} - ${value.balance} Kg`
        //                }));
        //        });
        //    } else {
        //        toastr.warning(errors[0].message, errors[0].title);
        //    }
        //}).fail(function () {
        //    toastr.warning(errors[0].message, errors[0].title);
        //});

        $.get("/FYsYarnIssue/GetYarnIndentDetails", formData, function (data) {
            if (typeof data !== "undefined") {
                rcvId.html("");
                rcvId.append('<option value="" selected>Select Indent No</option>');
                $.each(data, function (index, value) {
                    rcvId.append($("<option>",
                        {
                            value: value.trnsid,
                            text: value.opT1
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