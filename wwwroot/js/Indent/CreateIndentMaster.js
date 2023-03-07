
var unit = $("#FysIndentDetails_UNIT");
var sectionName = $("#FysIndentDetails_SEC_SECNAME");
var secid = $("#FysIndentDetails_SECID");
var cnsmp_amount = $("#FysIndentDetails_CNSMP_AMOUNT");
var etr = $("#FysIndentDetails_ETR");
var remarks = $("#FysIndentDetails_REMARKS");

var indslId = $("#FysIndentDetails_INDSLID");
var transId = $("#FysIndentDetails_TRNSID");
var fysim = $("#FysIndentMaster_INDSLID");
var yarnDetails = $("#yarnDetailsTableRetrieve");
var yarnProductName = $("#FysIndentDetails_PRODID");
var otherDetails = $("#OtherDetails");

var transDate = $("#FysIndentDetails_TRNSDATE");
var slubCode = $("#FysIndentDetails_SLUB_CODE");
var raw = $("#FysIndentDetails_RAW");
var prevLot = $("#FysIndentDetails_PREV_LOT");
var stckAmnt = $("#FysIndentDetails_STOCK_AMOUNT");
var orderQty = $("#FysIndentDetails_ORDER_QTY");
var yarnFor = $("#FysIndentDetails_YARN_FOR");
var yarnType = $("#FysIndentDetails_YARN_TYPE");
var lastIndNo = $("#FysIndentDetails_LAST_INDENT_NO");
var lastIndDate = $("#FysIndentDetails_LAST_INDENT_DATE");
var yarnDetailsByCount = $("#yarnDetails");
var btnAdd = $("#addToUpdate");

var targets = [];
targets.push(unit);

$(function () {

    fysim.on("change",
        function() {
            const data = $("#form").serializeArray();
            $.post("/FysIndentMaster/GetIndentCountList", data, function (partialView) {
                yarnDetails.html(partialView);
            });
        });

    //fysim.on("change", function () {

    //    const formData = {
    //        "id": $(this).val()
    //    };

    //    $.get("/FysIndentMaster/GetCountNameListById", formData, function (data) {

    //        toastr.options = {
    //            "closeButton": false,
    //            "positionClass": "toast-bottom-full-width"
    //        }

    //        if (data.comExPiDetailseList !== null) {
                
    //            $.post("/FysIndentMaster/GetOtherDetails", formData, function (partialView) {
    //                otherDetails.html(partialView);
    //            });

    //            yarnProductName.html("");
    //            yarnProductName.append('<option value="" selected>Select A Count Name.</option>');

    //            $.each(data.fYsIndentDetailsList, function (id, option) {
    //                yarnProductName.append($("<option></option>").val(option.bascountinfo.countid).html(option.bascountinfo.countname));
    //            });

    //            toastr.success("We Found Something Please Check.", "Request Accepted");

    //            //pino.text(data.comExPiDetailseList[0].pimaster.pino);
    //            //pidate.text(data.comExPiDetailseList[0].pimaster.pidate);
    //            //buyerName.text(data.comExPiDetailseList[0].pimaster.buyer.buyeR_NAME);
    //            //buyer_address.text(data.comExPiDetailseList[0].pimaster.buyer.address);

    //            //ref_person.text(data.fYsIndentDetailsList[0].rnD_PURCHASE_REQUISITION_MASTER.emp.firsT_NAME);
    //            //section.text(data.fYsIndentDetailsList[0].sec.secname);

    //            return true;
    //        } else {
    //            //section.text("");

    //            transId.val("");
    //            sectionName.val("");
    //            secid.val("");
    //            indslId.val("");
    //            cnsmp_amount.val("");
    //            unit.val("");
    //            etr.val("");
    //            remarks.val("");

    //            transDate.val("");
    //            slubCode.val("");
    //            raw.val("");
    //            prevLot.val("");
    //            stckAmnt.val("");
    //            orderQty.val("");
    //            yarnFor.val("");
    //            $("#yarnFor").text("");
    //            $("#yarnType").text("");
    //            yarnType.val("");
    //            lastIndNo.val("");
    //            lastIndDate.val("");

    //            yarnProductName.html("");
    //            yarnProductName.append('<option value="" selected>Select Product</option>');

    //            toastr.warning("No Data.", "Error");

    //            return false;
    //        }
    //    });
    //});

    yarnProductName.on("change", function () {

        const data = {
            "id": fysim.val(),
            "prdId": yarnProductName.val()
        };

        $.get("/FysIndentMaster/GetPreviousIndentDetailsById", data, function (partialView) {
            if (partialView) {
                yarnDetailsByCount.html(partialView);
            } else {
                toastr.warning("The Selected Count Name Has No Associated Objects.", "Not Found!");
            }
        });
    });

    btnAdd.on("click", function () {

        const data = $("#form").serializeArray();

        $.post("/FysIndentMaster/AddOrRemoveFromDetailsList", data, function (partialView) {
            yarnDetails.html(partialView);
        });
    });
});

var removeFromList = function (index) {

    const data = $("#form").serializeArray();
    data.push({ name: "RemoveIndex", value: index });
    data.push({ name: "IsDeletable", value: true });

    $.post("/FysIndentMaster/AddOrRemoveFromDetailsList", data, function (partialView) {
        yarnDetails.html(partialView);
    });
}

var updateRow = function (selector) {
    $("[type='hidden']", selector).prop("type", "text").addClass("col-xs-6 col-sm-3 mr-2");
};