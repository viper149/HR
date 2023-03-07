
var attachTo = $("#comExAdvDelTable");
var submitBtn = $("#btn_submit");
var buyerId = $("#ComExAdvDeliverySchMaster_BUYER_ID");
var piId = $("#ComExAdvDeliverySchDetails_PIID");
var styleId = $("#ComExAdvDeliverySchDetails_STYLE_ID");
var bAddress = $("#bAddress");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    },
    1: {
        title: "No PI Selected!!",
        message: "Please select PI first."
    },
    2: {
        title: "No Style Selected!!",
        message: "Please select style first."
    },
    3: {
        title: "No Quantity!!.",
        message: "Please Enter Quantity."
    }
}

$(function () {
    //var styleId = $("#ComExAdvDeliverySchDetails_STYLE_ID");
    var qty = $("#ComExAdvDeliverySchDetails_QTY");

    GetBuyerAddress();
    GetPIByBuyer();

    buyerId.on("change",
        function () {


            GetBuyerAddress();
            GetPIByBuyer();
        });

    piId.on("change",
        function () {

            if ($(this).val()) {

                var formData = {
                    "piId": $(this).val()
                };

                $.post("/AdvanceDeliverySchedule/GetStyle",
                    formData,
                    function (data) {

                        styleId.html("");
                        styleId.append('<option value="" selected>Select Style</option>');
                        $.each(data,
                            function (index, option) {
                                styleId.append($("<option>",
                                    {
                                        value: option.trnsid,
                                        text: option.style.fabcodeNavigation.stylE_NAME
                                    }));
                            });

                    }).fail(function () {
                        toastr.error(errors[0].message, errors[0].title);
                    });
            } else {
                toastr.error(errors[0].message, errors[0].title);
            }
        });

    styleId.on("change",
        function () {
            if ($(this).val()) {
                var formData = {
                    "id": $(this).val()
                }
                $.get("/AdvanceDeliverySchedule/GetUnit",
                    formData,
                    function (data) {
                        $("#unit").text(data);
                    }).fail(function () {
                    toastr.error(errors[0].message, errors[0].title);
                });
            }
        });


    $("#addToList").on("click",
        function () {
            if (piId[0].selectedIndex <= 0) {
                toastr.warning(errors[1].message, errors[1].title);
                return false;
            } else if (styleId[0].selectedIndex <= 0) {
                toastr.warning(errors[2].message, errors[2].title);
                return false;
            } else if (!parseInt(qty.val()) > 0) {
                toastr.warning(errors[3].message, errors[3].title);
                return false;
            }
            var formData = $("#form").serializeArray();

            $.post("/AdvanceDeliverySchedule/AddOrRemoveFromList",
                formData,
                function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
        });
});

function GetBuyerAddress() {
    if (buyerId.val()) {
        var formData = {
            "buyerId": buyerId.val()
        }
        bAddress.text("");
        $.get("/AdvanceDeliverySchedule/GetAddress",
            formData,
            function (data) {
                bAddress.text(data);
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
    }

   
        
    
}

function GetPIByBuyer() {
    if (buyerId.val()) {

        var formData = {
            "buyerId": buyerId.val()
        };

        $.post("/AdvanceDeliverySchedule/GetPI",
            formData,
            function (data) {

                piId.html("");
                styleId.html("");
                piId.append('<option value="" selected>Select PI</option>');
                $.each(data,
                    function (index, option) {
                        piId.append($("<option>",
                            {
                                value: option.piid,
                                text: option.pino
                            }));
                    });

            }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

function RemoveDS(index) {
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

                $.post("/AdvanceDeliverySchedule/AddOrRemoveFromList", formData, function (partialView) {
                    attachTo.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
}