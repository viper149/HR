var doId = $("#FFsDeliveryChallanPackMaster_DOID");
var delType = $("#FFsDeliveryChallanPackMaster_DELIVERY_TYPE");

$(function () {

    GetDivByType();

    getPiidAndTrnsId();

   function getPiidAndTrnsId()
   {
       if ($("#FFsDeliveryChallanPackMaster_PIID").val() && $("#FFsDeliveryChallanPackMaster_SO_NO").val()) {

           var selectedVal1 = $("#FFsDeliveryChallanPackMaster_PIID").val();
           var selectedVal2 = $("#FFsDeliveryChallanPackMaster_SO_NO").val();
           GetPiBalance(selectedVal1, selectedVal2);
       }

    }

   

    $("#getChallanNo").on('click', function () {
        console.log(delType.val());
        if (delType.val() !== "") {
            if (delType.val() === "5") {
                toastrNotification("Not valid for internal challan!!", "warning");
                return false;
            }
            var selectedVal = delType.val();
            $.ajax({
                async: true,
                cache: false,
                data: { "delType": selectedVal },
                type: "GET",
                url: "/FFsPackingList/GetChallanNo",
                success: function (data, status, xhr) {
                    $("#FFsDeliveryChallanPackMaster_DC_NO").val(data.challan);
                    $("#FFsDeliveryChallanPackMaster_GP_NO").val(data.gatePass);
                },
                error: function (e) {
                    console.log(e);
                }
            });
        }
        else {
            toastrNotification("Please Select Delivery Type First", "error");
            return false;
        }
    });


    $('#form').submit(function () {

        if ($("#FFsDeliveryChallanPackMaster_PIID").val() === "" && $("#FFsDeliveryChallanPackMaster_ISSUE_TYPE").val() === "1") {
            toastrNotification("Please Select a PI First", "error");
            return false;
        }
        if ($("#FFsDeliveryChallanPackMaster_SO_NO").val() === "" && $("#FFsDeliveryChallanPackMaster_ISSUE_TYPE").val() === "1") {
            toastrNotification("Please Select a Style First", "error");
            return false;
        }



        //if ($("#FFsDeliveryChallanPackMaster_DELIVERY_TYPE").val() !== "5" && $("#FFsDeliveryChallanPackMaster_DC_NO").val() === "") {
        //    toastrNotification("Please Enter Challan No First", "error");
        //    return false;
        //}

        $(this).find(':submit').attr('disabled', 'disabled');
    });
    $('#rollReload').val($('#rollReload').val() + 1);




    $("#btnAddRoll").on('click',
        function () {
            $.ajax({
                async: true,
                cache: false,
                data: $('#form').serialize(),
                type: "POST",
                url: "/FFsPackingList/AddRollList",
                success: function (partialView, status, xhr) {
                    $('#rollDetails').html(partialView);
                },
                error: function (e) {
                    console.log(e);
                }
            });
        });

    $("#FPrInspectionProcessDetails_LENGTH_1").on('change',
        function () {
            var totalYds = parseFloat($(this).val()) + parseFloat($("#FPrInspectionProcessDetails_LENGTH_2").val());
            var totalMtr = totalYds / 1.094;
            $("#FPrInspectionProcessDetails_LENGTH_YDS").val(totalYds);
            $("#FPrInspectionProcessDetails_LENGTH_MTR").val(totalMtr.toFixed(2));
        });

    $("#FPrInspectionProcessDetails_LENGTH_2").on('change',
        function () {
            var totalYds = parseFloat($(this).val()) + parseFloat($("#FPrInspectionProcessDetails_LENGTH_1").val());
            var totalMtr = totalYds / 1.094;
            $("#FPrInspectionProcessDetails_LENGTH_YDS").val(totalYds);
            $("#FPrInspectionProcessDetails_LENGTH_MTR").val(totalMtr.toFixed(2));
        });

    doId.on('change',
        function () {
            GetDoDetails(doId.val());
            GetDoBalance(doId.val());
            GetPackingList();
        });

    $("#FFsDeliveryChallanPackMaster_PIID").on('change',
        function () {
            debugger;
            var selectedVal = $(this).val();

            GetPiDetails(selectedVal);
        });



    //$("#FFsDeliveryChallanPackMaster_SO_NO").on('change',
    //    function () {
    //        var selectedVal1 = $("#FFsDeliveryChallanPackMaster_PIID").val();
    //        var selectedVal2 = $("#FFsDeliveryChallanPackMaster_SO_NO").val();
    //        GetPiBalance(selectedVal1, selectedVal2);

    //    });

    $("#FFsDeliveryChallanPackMaster_SO_NO").on('change',
        function () {
            var selectedVal = $(this).val();
            GetSoBalance(selectedVal);
            getPiidAndTrnsId();

        });

    $("#FFsDeliverychallanPackDetails_ROLL_NO").on('change',
        function () {
            var selectedVal = $(this).val();
            $.ajax({
                async: true,
                cache: false,
                data: { "rollId": selectedVal },
                type: "GET",
                url: "/FFsRollReceive/GetRollIDetails",
                success: function (data, status, xhr) {
                    console.log(data);
                    $("#qty_y").text(data.balancE_QTY);
                    $("#qty_m").text((data.balancE_QTY / 1.094).toFixed(2));
                    $("#FFsDeliverychallanPackDetails_LENGTH1").val(data.balancE_QTY);
                },
                error: function (e) {
                    console.log(e);
                }
            });
        });

    $("#FFsDeliverychallanPackDetails_LENGTH1").on('change',
        function () {
            var selectedVal = $(this).val();
            GetRollBalance(parseFloat(selectedVal) + parseFloat($("#FFsDeliverychallanPackDetails_LENGTH2").val()));
        });

    $("#FFsDeliverychallanPackDetails_LENGTH2").on('change',
        function () {
            var selectedVal = $(this).val();
            GetRollBalance(parseFloat(selectedVal) + parseFloat($("#FFsDeliverychallanPackDetails_LENGTH1").val()));
        });

    let timer;
    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").on('input',
        code => {
            clearTimeout(timer);
            timer = setTimeout(x => {
                getRollDetailsByScan($(this).val());
            }, 1000, code);
        });

    delType.on('change',
        function () {
            GetDivByType();
        });


    //var typingTimer;
    //$("#FFsDeliverychallanPackDetails_ROLL_NO_N").on('keyup', function () {

    //    //if ($(this).val().length > 5) {
    //    //    clearTimeout(typingTimer);
    //    //    typingTimer = setTimeout(() => {
    //    //        getRollDetailsByScan($(this).val());
    //    //    },
    //    //        500);
    //    //}

    //    clearTimeout(typingTimer);
    //    typingTimer = setTimeout(getRollDetailsByScan($(this).val()), 1000);

    //    //if ($(this).val().length > 11) {
    //    //    toastrNotification("Please Scan Correct Roll No.", "error");
    //    //    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
    //    //    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
    //    //    return false;
    //    //}
    //    //if ($(this).val().length === 11) {
    //    //    if (!$("#FFsDeliveryChallanPackMaster_PIID").val()) {
    //    //        toastrNotification("Please select PO No First then Scan", "error");
    //    //        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
    //    //        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
    //    //        return false;
    //    //    }
    //    //    if (!$("#FFsDeliveryChallanPackMaster_SO_NO").val()) {
    //    //        toastrNotification("Please select Order No First then Scan", "error");
    //    //        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
    //    //        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
    //    //        return false;
    //    //    }


    //    //    getRollDetailsByScan($(this).val());
    //    //    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
    //    //    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();

    //    //} else {
    //    //    return false;
    //    //}
    //});
});

function GetPiDetails(selectedVal) {
    $.ajax({
        async: true,
        cache: false,
        data: { "piId": selectedVal },
        type: "POST",
        url: "/ComExPiMaster/GetPIDetailsListData",
        success: function (data, status, xhr) {
            //console.log(data);

            $("#FFsDeliveryChallanPackMaster_SO_NO").html('');
            $("#FFsDeliveryChallanPackMaster_SO_NO").append('<option value="" selected>Select Order No</option>');
            $.each(data,
                function (id, option) {
                    $("#FFsDeliveryChallanPackMaster_SO_NO").append($('<option></option>').val(option.trnsid).html(option.sO_NO));
                });
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function GetDoDetails(selectedVal) {
    $.ajax({
        async: true,
        cache: false,
        data: { "doId": selectedVal },
        type: "GET",
        url: "/AccExportDoMaster/GetDoDetails",
        success: function (data, status, xhr) {
            //console.log(data);
            $("#lc").text(data.comExLcInfo.lcno);
            $("#lcDate").text(data.comExLcInfo.lcdate.substring(0, 10));
            $("#buyer").text(data.comExLcInfo.buyer.buyeR_NAME);

            $("#FFsDeliveryChallanPackMaster_PIID").html('');
            $("#FFsDeliveryChallanPackMaster_PIID").append('<option value="" selected>Select PI No.</option>');
            $.each(data.comExLcInfo.coM_EX_LCDETAILS,
                function (id, option) {
                    $("#FFsDeliveryChallanPackMaster_PIID").append($('<option></option>').val(option.pi.piid).html(option.pi.pino));
                });
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function GetSoBalance(selectedVal) {
    $.ajax({
        async: true,
        cache: false,
        data: { "soId": selectedVal },
        type: "GET",
        url: "/FFsPackingList/GetSoBalance",
        success: function (data, status, xhr) {
            console.log(data);
            $("#orderBalance").val(data);
        },
        error: function (e) {
            console.log(e);
        }
    });
}

debugger;
function GetPiBalance(selectedVal, selectedVal2) {
    $.ajax({
        
        async: true,
        cache: false,
        data: { "piId": selectedVal, "trnsId": selectedVal2},
        type: "GET",
        url: "/FFsPackingList/GetPiBalance",
        success: function (data, status, xhr) {
            console.log(data);
            console.log(data.pidel);
            $("#piBalance").val(data.blnce);
            $("#piDelivery").text(data.pidel);
            $("#piQtyAsSoNum").text(data.piqtyasso);
            $("#piBalance1").text(data.pibalance);
        },
        error: function (e) {
            console.log(e);
        }
    });
}


function GetDoBalance(selectedVal) {
    $.ajax({
        async: true,
        cache: false,
        data: { "doId": selectedVal },
        type: "GET",
        url: "/AccExportDoMaster/GetDoDetails",
        success: function (data, status, xhr) {
            $("#lc").text(data.comExLcInfo.lcno);
            $("#lcDate").text(data.comExLcInfo.lcdate.substring(0, 10));
            $("#buyer").text(data.comExLcInfo.buyer.buyeR_NAME);
        },
        error: function (e) {
            console.log(e);
        }
    });


    $.ajax({
        async: true,
        cache: false,
        data: { "doId": selectedVal },
        type: "GET",
        url: "/FFsPackingList/GetDoBalance",
        success: function (data, status, xhr) {
            console.log(data);
            $("#doBalance").val(data);
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function GetPackingList() {
    const formData = {
        "doId": doId.val()
    };
    $.get("/FFsPackingList/GetPackingRollList",
        formData,
        function (data) {
            var table = "";
            if (data != null) {
                $.each(data.fFsDeliveryChallanPackDetailsList,
                    function (index, value) {
                        console.log(value);
                        table += `<tr>
                    <td>${value.roll.pO_NONavigation.pino}</td>
                    <td>${value.roll.sO_NONavigation.style.stylename}</td>
                    <td>${value.roll.sO_NONavigation.qty}</td>
                    <td>N/A</td>
                    <td>N/A</td>
                    <td>${value.roll.sO_NONavigation.f_BAS_UNITS.uname}</td>
                    </tr>`;
                    });
            }
            $("#packingList").html(table);
        }).error(function () {

            $("#packingList").html("");
        });
}


function GetRollBalance(fullLength) {
    var qty_y = $("#qty_y").text();
    if (fullLength > qty_y) {
        toastrNotification('Total Length Must be less than Balance', 'error');
        $('#FFsDeliverychallanPackDetails_LENGTH1').addClass('border-danger');
        $('#FFsDeliverychallanPackDetails_LENGTH2').addClass('border-danger');
        $('#btnAddRoll').prop('disabled', true);
        //$('#btn_submit').prop('disabled', true);
    } else {
        $('#FFsDeliverychallanPackDetails_LENGTH1').removeClass('border-danger');
        $('#FFsDeliverychallanPackDetails_LENGTH2').removeClass('border-danger');
        $('#btnAddRoll').prop('disabled', false);
        //$('#btn_submit').prop('disabled', false);
    }
}

function GetDivByType() {

    if (delType.val()) {
        if (delType.val() == 4) {
            $("#expDiv").addClass("d-none");
            $("#localDiv1").removeClass("d-none");
            $("#localDiv2").removeClass("d-none");
        } else {
            $("#expDiv").removeClass("d-none");
            $("#localDiv1").addClass("d-none");
            $("#localDiv2").addClass("d-none");
        }
    }
}

function getRollDetailsByScan(selectedVal) {

    var data = $('#form').serializeArray();
    console.log(delType.val());
    if (delType.val() != 4) {
        if ($("#FFsDeliveryChallanPackMaster_PIID").val() === "" && $("#FFsDeliveryChallanPackMaster_ISSUE_TYPE").val() === "1") {
            toastrNotification("Please Select a PI First", "error");
            return false;
        }

        if ($("#FFsDeliveryChallanPackMaster_SO_NO").val() === "" && $("#FFsDeliveryChallanPackMaster_ISSUE_TYPE").val() === "1") {
            toastrNotification("Please Select a Style First", "error");
            return false;
        }
    }

    $.ajax({
        async: true,
        cache: false,
        data: data,
        type: "POST",
        url: "/FFsPackingList/RollDetailsListByScan",
        success: function (partialView, status, xhr) {

            var menual = $("#FFsDeliverychallanPackDetails_ISMANUAL").prop('checked');
            var result = xhr.getResponseHeader("Status");

            $('#rollDetails').html(partialView);

            if (result === "Success") {
                toastrNotification("Roll Added", "success");
                if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                }
            } else if (result === "Null") {
                toastrNotification("Roll not found. Please Scan correct roll!!!", "error");

                if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        500);
                }

            } else if (result === "Error") {
                toastrNotification("You have already added this roll!!!", "warning"); if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                }
            } else if (result === "Wrong Order") {
                toastrNotification("Wrong Order No...This Roll is not for this order!!!", "error");
                if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                }
            } else if (result === "Wrong PI") {
                toastrNotification("Wrong PO...This Roll is not for this PO!!!", "error");
                if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                }
            } else if (result === "DB") {
                toastrNotification("You have already Delivered this roll!!!", "warning");
                if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                }
            } else if (result === "SHADE") {
                toastrNotification("Shade Required to Deliver this roll!!!", "warning");
                if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                }
            } else if (result === "DO OVER") {
                toastrNotification("DO Limit Alert!!!", "warning");
                if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                }
            } else if (result === "PI OVER") {
                toastrNotification("PI Limit Alert!!!", "warning");
                if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                }
            } else if (result === "SO OVER") {
                toastrNotification("Order Limit Alert!!!", "warning");
                if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                }
            } else if (result === "STYLE") {
                toastrNotification("Style Not Matched. Please Check the Roll Properly!!!", "error");
                if (menual) {
                    setTimeout(() => {
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                        $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                    },
                        10000);
                } else {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                }
            }

        },
        error: function (e) {
            var menual = $("#FFsDeliverychallanPackDetails_ISMANUAL").prop('checked');
            console.log(e);
            if (menual) {
                setTimeout(() => {
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                    $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
                },
                    10000);
            } else {
                $("#FFsDeliverychallanPackDetails_ROLL_NO_N").val("");
                $("#FFsDeliverychallanPackDetails_ROLL_NO_N").focus();
            }
        }
    });
}
