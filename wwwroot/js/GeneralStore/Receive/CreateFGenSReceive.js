
var attachTo = $("#fGenSReceiveDetailsTable");
var submitBtn = $("#btn_submit");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    },
    1: {
        title: "No Quantity!!.",
        message: "Please Enter Quantity"
    },
    2: {
        title: "No Product Selected!!",
        message: "Please select product first."
    },
    3: {
        title: "No Indent No. Selected!!",
        message: "Please select indent no. first."
    },
    4: {
        title: "QC Approved!",
        message: "Click UPDATE to Confirm Approval."
    },
    5: {
        title: "MRR Created!",
        message: "Click UPDATE to Confirm Creation."
    }
}

$(function () {
    var gindid = $("#FGenSReceiveDetails_GINDID");
    var ginddate = $("#gindDate");
    var productid = $("#FGenSReceiveDetails_PRODUCTID");
    var fGenSUnit = $("#unit");
    var adjustedWith = $("#FGenSReceiveDetails_ADJUSTED_WITH");
    var rcvType = $("#FGenSReceiveMaster_RCVTID");
    var btnAdd = $("#addToList");
    var pino = $("#FGenSReceiveMaster_LC_ID");
    var lcno = $("#lcNo");
    var frQty = $("#FGenSReceiveDetails_FRESH_QTY");
    var qc = $("#approveQc");
    var mrr = $("#createMRR");
    var qcH = $("#FGenSReceiveMaster_QCPASS");
    var mrrH = $("#FGenSReceiveMaster_MRR");

    getLc();

    qc.on("click",
        function () {
            debugger;
            const data = $("#form").serializeArray();

            $.post("/GeneralStoreReceive/QcApprove", data, function (result) {

            }).success(function () {
                $.post("/GeneralStoreReceive/GetQc", data, function (xc) {
                    qcH.val(xc.gsqcaid);
                    toastr.success(errors[4].message, errors[4].title);
                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        });

    mrr.on("click",
        function () {
            const data = $("#form").serializeArray();

            $.post("/GeneralStoreReceive/CreateMrr", data, function (result) {

            }).success(function () {
                $.post("/GeneralStoreReceive/GetMrr", data, function (xc) {
                    mrrH.val(xc.gsmrrid);
                    toastr.success(errors[5].message, errors[5].title);
                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        });

    rcvType.on("change",
        function () {

            var rcvTypeVal = rcvType.val();

            var targetItems = [
                "20000",
                "20001"
            ];

            if ($.inArray(rcvTypeVal, targetItems) > -1) {
                adjustedWith.parents('div').eq(1).addClass("d-none");
            } else {
                adjustedWith.parents('div').eq(1).removeClass("d-none");
            }
        });

    gindid.on("change",
        function () {

            var selectedGindid = gindid.val();

            $.get("/GeneralStoreReceive/GetIndentMaster",
                { "id": selectedGindid },
                function (data) {

                    ginddate.text(new Date(data.ginddate).toISOString().slice(0, 10));

                }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
        });

    pino.on("change", function () {
        getLc();
    });

    //productid.on("change", function () {

    //    if (productid.val()) {

    //        var formData = {
    //            "productId": productid.val()
    //        }

    //        $.get("/GeneralStoreReceive/GetProductDetails", formData, function (data) {

    //            fGenSUnit.text(data.unitNavigation.uname);

    //        }).fail(function () {
    //            toastr.error(errors[0].message, errors[0].title);
    //        });
    //    } else {
    //        toastr.error(errors[0].message, errors[0].title);
    //    }
    //});

    productid.on("change",
        function () {

            if (productid.val()) {

                var formData = {
                    "productId": productid.val()
                };

                $.post("/GeneralStoreReceive/GetIndentByProduct",
                    formData,
                    function (data) {
                        gindid.html("");
                        gindid.append('<option value="" selected>Select Indent No</option>');

                        $.each(data.f_GEN_S_INDENTDETAILS,
                            function (index, option) {

                                fGenSUnit.text(data.unitNavigation.uname);

                                gindid.append($("<option>",
                                    {
                                        value: option.gind.gindid,
                                        text: option.gind.gindno
                                    }));
                            });

                    }).fail(function () {
                        toastr.error(errors[0].message, errors[0].title);
                    });
            } else {
                toastr.error(errors[0].message, errors[0].title);
            }
        });


    btnAdd.on("click",
        function () {
            if (productid[0].selectedIndex <= 0) {
                toastr.warning(errors[2].message, errors[2].title);
                return false;
            } else if (gindid[0].selectedIndex <= 0) {
                toastr.warning(errors[3].message, errors[3].title);
                return false;
            } else if (!parseInt(frQty.val()) > 0) {
                toastr.error(errors[1].message, errors[1].title);
                return false;
            }
            var formData = $("#form").serializeArray();

            $.post("/GeneralStoreReceive/AddOrRemoveFromReceiveDetails",
                formData,
                function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });

            //if (productid.val()) {

            //    const formData = $("#form").serializeArray();

            //    var xhr = $.post("/GeneralStoreReceive/AddOrRemoveFromReceiveDetails", formData, function (partialView) {

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

    function getLc() {
        var formData = {
            "id": pino.val()
        }

        $.get("/GeneralStoreReceive/GetLC",
            formData,
            function (data) {

                lcno.text(data.lc.lcno);

            }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
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

                $.post("/GeneralStoreReceive/AddOrRemoveFromReceiveDetails", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}