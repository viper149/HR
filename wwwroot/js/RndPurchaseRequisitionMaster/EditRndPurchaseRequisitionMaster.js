
var attachTo = $("#yarnDetailsTable");
var btnAdd = $("#btnAdd");

var countId = $("#FysIndentDetails_PRODID");
//var countId = $("#");

var yarnForD = $("#FysIndentDetails_YARN_FOR");
var yarnForm = $("#FysIndentDetails_YARN_FROM");
var etr = $("#FysIndentDetails_ETR");
var orderQty = $("#FysIndentDetails_ORDER_QTY");
var slubCode = $("#FysIndentDetails_SLUB_CODE");
var raw = $("#FysIndentDetails_RAW");
var lot = $("#FysIndentDetails_PREV_LOTID");
var remarks = $("#FysIndentDetails_REMARKS");



var buyerNameDiv = $("#buyerName");
var orderNoDiv = $("#orderNoDiv");
var fabcodeDiv = $("#fabcode");
var orderNoSampleDiv = $("#orderNoSampleDiv");
var yarnFor = $("#RndPurchaseRequisitionMaster_YARN_FOR");
var orderNo = $("#RndPurchaseRequisitionMaster_ORDER_NO");
var sdrfNo = $("#RndPurchaseRequisitionMaster_ORDERNO_S");
var fabcode = $("#RndPurchaseRequisitionMaster_FABCODE");
var sampleL = $("#sampleLDiv");
var costRef = $("#costRefDiv");
var reqNo = $("#RndPurchaseRequisitionMaster_INDSLNO");

var targets = [];
var targets = [];

targets.push(yarnForD, yarnForm, etr, orderQty, slubCode, raw, lot, remarks);



$(function () {

    var errors = {
        0: {
            title: "Invalid Submission.",
            message: "We can not process your request. Please try again later."
        }
    }
    GetTypeDetails();
    $("#rev").change(function () {
        reqNo.removeAttr("readonly", true);
        $(".form-check-input").hide();
    });

    yarnFor.on("change", function () {

        if (yarnFor.val() === "Sample") {

            orderNoDiv.addClass("d-none");
            buyerNameDiv.addClass("d-none");
            fabcodeDiv.addClass("d-none");
            costRef.addClass("d-none");
            orderNoSampleDiv.removeClass("d-none");
            sampleL.removeClass("d-none");
            //fabcode.attr('selectedIndex', '-1').find("option:selected").removeAttr("selected");

            $.get("/Requisition/GetListOfSDRF", function (data) {
                orderNo.html("");
                orderNo.append('<option value="" selected>Select PO No</option>');

                sdrfNo.html("");
                sdrfNo.append('<option value="" selected>Select SDRF No</option>');

                $.each(data, function (id, option) {
                    sdrfNo.append($("<option>",
                        {
                            value: option.sdrfid,
                            text: option.sdrF_NO
                        }));
                });
            });
        }

        if (yarnFor.val() === "Export" || yarnFor.val() === "Projection" || yarnFor.val() === "Lease Yarn" || yarnFor.val() === "Leader") {
            buyerNameDiv.addClass("d-none");
            fabcodeDiv.addClass("d-none");
            orderNoSampleDiv.addClass("d-none");
            orderNoDiv.removeClass("d-none");
            //fabcode.attr('selectedIndex', '-1').find("option:selected").removeAttr("selected");

            $.get("/Requisition/GetListOfSO", function (data) {

                orderNo.html("");
                orderNo.append('<option value="" selected>Select PO No</option>');

                $.each(data, function (id, option) {
                    orderNo.append($("<option>",
                        {
                            value: option.id,
                            text: option.name
                        }));
                });
            });

            if (yarnFor.val() === "Projection" || yarnFor.val() === "Lease Yarn" || yarnFor.val() === "Leader") {
                orderNo.val("");
                GetBulkOrderCount(0);
                orderNoDiv.addClass("d-none");
                orderNoSampleDiv.addClass("d-none");
                buyerNameDiv.removeClass("d-none");
                fabcodeDiv.removeClass("d-none");
            }
        }


    });
    //For Edit Page Count Get
    // GetBulkOrderCount(orderNo.val());

    orderNo.on("change",
        function () {

            //if (yarnFor.val() === "Sample") {

            //    orderNoDiv.addClass("d-none");
            //    buyerNameDiv.addClass("d-none");
            //    orderNoSampleDiv.removeClass("d-none");

            //    $.get("/Requisition/GetListOfSDRF", function (data) {
            //        orderNo.html("");
            //        orderNo.append('<option value="" selected>Select PO No</option>');

            //        sdrfNo.html("");
            //        sdrfNo.append('<option value="" selected>Select SDRF No</option>');

            //        $.each(data, function (id, option) {
            //            sdrfNo.append($("<option>",
            //                {
            //                    value: option.sdrfid,
            //                    text: option.sdrF_NO
            //                }));
            //        });
            //    });
            //}

            if (yarnFor.val() === "Export") {
                var ordId = $(this).val();
                GetBulkOrderCount(ordId);
            }
            if (yarnFor.val() === "Projection" || yarnFor.val() === "Lease Yarn" || yarnFor.val() === "Leader") {


                //if (yarnFor.val() === "Projection") {

                //    orderNoDiv.addClass("d-none");
                //    orderNoSampleDiv.addClass("d-none");
                //    buyerNameDiv.removeClass("d-none");
                //}

                $.get("/RndPurchaseRequisitionMaster/GetCountList",
                    { 'poId': ordId },
                    function (data) {
                        countId.html("");
                        countId.append('<option value="" selected>Select Count Name</option>');

                        $.each(data.basCount,
                            function (id, option) {
                                countId.append($("<option>",
                                    {
                                        value: option.countid,
                                        text: option.countname
                                    }));
                            });
                    });
            }
        });

});
btnAdd.on("click", function () {

    if (orderQty.val() && countId.val()) {

        const formData = $("#form").serializeArray();

        $.post("/RndPurchaseRequisitionMaster/AddIndentDetails", formData, function (partialView) {
            attachTo.html(partialView);
            resetFields(targets);
        }).fail(function () {
            toastr.warning(errors[0].message, errors[0].title);
        });
    }
});

function GetBulkOrderCount(ordId) {
    $.get("/RndPurchaseRequisitionMaster/GetCountList", { 'poId': ordId }, function (data) {
        countId.html("");
        countId.append('<option value="" selected>Select Count Name</option>');
        if (ordId !== 0) {
            $.each(data.count, function (id, option) {
                countId.append($("<option>",
                    {
                        value: option.count.countid,
                        text: option.count.countname
                    }));
            });
        }
        else {
            console.log(data);
            $.each(data.basCount, function (id, option) {
                countId.append($("<option>",
                    {
                        value: option.countid,
                        text: option.countname
                    }));
            });
        }
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}

function RemoveIndent(removeIndexValue) {
    swal({
        title: "Please Confirm",
        text: `You won't able to revert, Are you sure to remove item no. - ${removeIndexValue + 1}`,
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {
            if (isConfirm) {
                const data = $("#form").serializeArray();

                data.push({ name: "RemoveIndex", value: removeIndexValue });
                data.push({ name: "IsDeletable", value: true });

                $.ajax({
                    async: true,
                    cache: false,
                    data: data,
                    type: "POST",
                    url: "/RndPurchaseRequisitionMaster/AddIndentDetails",
                    success: function (partialView) {
                        attachTo.html(partialView);
                    },
                    error: function (e) {
                        console.log(e);
                    }
                });
            }
        });
}


function GetTypeDetails() {
    if (yarnFor.val() === "Sample") {
        orderNoDiv.addClass("d-none");
        buyerNameDiv.addClass("d-none");
        fabcodeDiv.addClass("d-none");
        orderNoSampleDiv.removeClass("d-none");
        sampleL.removeClass("d-none");
    }

    if (yarnFor.val() === "Export") {

        buyerNameDiv.addClass("d-none");
        fabcodeDiv.addClass("d-none");
        orderNoSampleDiv.addClass("d-none");
        orderNoDiv.removeClass("d-none");
    }

    if (yarnFor.val() === "Projection" || yarnFor.val() === "Lease Yarn" || yarnFor.val() === "Leader") {
        orderNo.val("");
        orderNoDiv.addClass("d-none");
        orderNoSampleDiv.addClass("d-none");
        buyerNameDiv.removeClass("d-none");
        fabcodeDiv.removeClass("d-none");
    }
}

function EditYarnInfo(trnsId) {

    $.ajax({
        async: true,
        cache: false,
        type: "GET",
        data: { trnsId: trnsId },
        url: '/Requisition/GetYarnDetails',

        success: function (data) {
            if (data) {

                console.log(data);
                $("#FysIndentDetails_TRNSID").val(data.trnsid);
                $('#FysIndentDetails_PRODID').val(data.prodid).trigger("change");
                $('#FysIndentDetails_YARN_FOR').val(data.yarN_FOR).trigger("change");
                $('#FysIndentDetails_YARN_FROM').val(data.yarN_FROM).trigger("change");
                $('#FysIndentDetails_PREV_LOTID').val(data.preV_LOTID).trigger("change");
                $('#FysIndentDetails_RAW').val(data.raw).trigger("change");
                $('#FysIndentDetails_SLUB_CODE').val(data.sluB_CODE).trigger("change");
                $('#FysIndentDetails_ORDER_QTY').val(data.ordeR_QTY);
                $('#FysIndentDetails_REMARKS').val(data.remarks);
                $('#FysIndentDetails_NO_OF_CONE').val(data.nO_OF_CONE);
                $('#FysIndentDetails_ETR').val(moment(data.etr).format('YYYY-MM-DD'));
            } else {
                toastr.error(errors[1].message, errors[0].title);
            }
        },
        error: function (e) {
            toastr.error(errors[1].message, errors[1].title);
        }
    });
};