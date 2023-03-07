
var attachTo = $("#doDetails");
var lcId = $("#lcNo");
var styleId = $("#ACC_EXPORT_DODETAILS_STYLEID");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    },
    1: {
        title: "Sorry!!",
        message: "No L/C Information."
    },
    2: {
        title: "No L/C Selected!!",
        message: "Please select L/C information first."
    },
    3: {
        title: "No Style Name Selected!!",
        message: "Please select  Style Name first."
    },
    4: {
        title: "Sorry!!",
        message: "No PI Information."
    }
}


$(function () {
    
    var btnAdd = $("#btnAdd");

    getByLC();
    getByStyle();

    lcId.on("change", function () {
        getByLC();
    });

    styleId.on("change", function () {
        getByStyle();
    });

    btnAdd.on("click", function () {

        var formData = $("#form").serializeArray();

        $.post("/AccExportDoMaster/AddDoDetails", formData, function (partialView) {
            attachTo.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });
});

function getByLC() {

    var lcDate = $("#lcDate");
    var lcFileNo = $("#lcFile");
    var buyerName = $("#buyer");
    var buyerAddress = $("#address");

    if (lcId.val()) {

        var formData = {
            "lcId": lcId.val()
        };

        $.post("/AccExportDoMaster/GetFabStyleListByLcId", formData, function (data) {

            styleId.html("");
            styleId.append('<option value="" selected>Select Style Name</option>');

            $.each(data.coM_EX_LCDETAILS, function (index, option) {

                $.each(option.pi.coM_EX_PI_DETAILS, function (x, y) {
                    
                    styleId.append($("<option>",
                        {
                            value: y.trnsid,
                            text: y.style.fabcodeNavigation.stylE_NAME
                        }));
                });
            });

        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });

        $.post("/AccExportDoMaster/GetLcInfo", formData, function (data) {

            if (data != null) {
                lcDate.text(data.lcdate.substring(0, data.lcdate.indexOf('T')));
                lcFileNo.text(data.fileno);
                buyerName.text(data.buyer.buyeR_NAME);
                buyerAddress.text(data.buyer.address);
            }
            else {
                toastr.error(errors[1].message, errors[1].title);
            }
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

function getByStyle() {

    var qty = $("#qty");
    var uPrice = $("#rate");
    var amount = $("#amount");
    var piRemarks = $("#remarks");
    var piNo = $("#piNo");

    if (styleId.val()) {

        var formData = $("#form").serializeArray();

        var piIdH = $("#ACC_EXPORT_DODETAILS_PIID");

        $.post("/AccExportDoMaster/GetPiDetails", formData, function (data) {

            if (data != null) {
                piIdH.val(data.pimaster.piid);
                piNo.text(data.pimaster.pino);
                qty.val(data.qty);
                uPrice.val(data.unitprice);
                amount.val(data.total);
                piRemarks.val(data.remarks);
            } else {
                toastr.error(errors[4].message, errors[4].title);
            }
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

                var formData = $('#form').serializeArray();

                formData.push({ name: "IsDelete", value: true });
                formData.push({ name: "RemoveIndex", value: index });

                $.post("/AccExportDoMaster/AddDoDetails", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}