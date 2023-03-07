
var piDetails = $("#piDetails");

var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

$(function () {

    var qty = $("#qty");
    var rate = $("#unitPrice");
    var total = $("#total");
    var btnAdd = $("#btnAdd");
    var duration = $("#cOM_EX_PIMASTER_DURATION");
    var validity = $("#cOM_EX_PIMASTER_VALIDITY");
    var piDate = $("#cOM_EX_PIMASTER_PIDATE");
    var netWeight = $("#cOM_EX_PIMASTER_NET_WEIGHT");
    var grsWeight = $("#cOM_EX_PIMASTER_GRS_WEIGHT");

    var exportId = $("#cOM_EX_PIMASTER_EXP_STATUS");

    exportId.on("change", function () {


        var value = exportId.val();

        console.log(value);

        if (value != 1) {

            $(".styleDetails").removeClass('d-none');

        }

        else {
            $(".styleDetails").addClass("d-none");
        }

    });


    $("#netWeight,#grossWeight,#cOM_EX_PIMASTER_NET_WEIGHT,#cOM_EX_PIMASTER_GRS_WEIGHT").on("click change", function () {

        var formData = $("#form").serializeArray();

        $.post("/CommercialExport/GetNetGrossWeight", formData, function (data) {
            netWeight.val(data.neT_WEIGHT);
            grsWeight.val(data.grosS_WEIGHT);
        });
    });


    $("#cOM_EX_PIMASTER_TEAM_PERSONID").change(function () {
        var data = $('#cOM_EX_PIMASTER_TEAM_PERSONID').select2('data');
        $("#cOM_EX_PIMASTER_FLWBY").val(data[0].text);
    });

    duration.on("change", function () {
        $.ajax({
            async: true,
            cache: false,
            data: $("form").serializeArray(),
            type: "POST",
            url: "/ComExPiMaster/AddDays",
            success: function (data) {
                if (data !== null) {
                    validity.val(data);
                }
            },
            error: function () {
                console.log("an error occurred.");
            }
        });
    });

    piDate.on("change", function () {
        $.ajax({
            async: true,
            cache: false,
            data: $("form").serializeArray(),
            type: "POST",
            url: "/ComExPiMaster/AddDays",
            success: function (data) {
                if (data !== null) {
                    validity.val(data);
                }
            },
            error: function () {
                console.log("an error occurred.");
            }
        });
    });


    $("#cOM_EX_PIMASTER_TEAMID").change(function () {
        var selectedItem = $(this).val();
        $.ajax({
            type: "GET",
            url: "/MktSdrfInfo/GetTeamMembers",
            data: { "teamid": selectedItem },
            success: function (data) {
                if (data != null) {

                    $("#cOM_EX_PIMASTER_TEAM_PERSONID").html('');
                    $("#cOM_EX_PIMASTER_TEAM_PERSONID").append('<option value="" selected>Select Marketing Person</option>');
                    $.each(data,
                        function (id, option) {
                            $("#cOM_EX_PIMASTER_TEAM_PERSONID").append($('<option></option>').val(option.mkT_TEAMID).html(option.persoN_NAME));
                        });
                } else {
                    toastrNotification("Sorry! No Team Member.", "error");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                toastrNotification("Error: Failed To Retrieve Team Members.", "error");

            }
        });
    });

    qty.on("change", function () {
        storeMultiplicationResult(qty, rate, total);
    });

    rate.on("change", function () {
        storeMultiplicationResult(qty, rate, total);
    });

    btnAdd.on("click", function () {

        var formData = $("#form").serializeArray();

        $.post("/CommercialExport/AddOrRemovePIDetails", formData, function (partialView) {
            piDetails.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    });

    $("#cOM_EX_PI_DETAILS_STYLEID").on("change", function () {

        var selectedVal = $(this).val();

        $.ajax({
            async: true,
            cache: false,
            data: { "styleId": selectedVal },
            type: "POST",
            url: "/ComExPiMaster/GetCostRefNo",
            success: function (data) {

                $("#fabric").text(data.comExFabStyle.fabcodeNavigation.stylE_NAME);
                $("#color").text(data.comExFabStyle.fabcodeNavigation.colorcodeNavigation.color);
                $("#cOM_EX_PI_DETAILS_COSTID").html('');
                $("#cOM_EX_PI_DETAILS_COSTID").append('<option value="" selected>Select Cost Ref.</option>');

                $.each(data.cosPreCostingMasters, function (id, option) {
                    $("#cOM_EX_PI_DETAILS_COSTID").append($('<option></option>').val(option.csid).html('CS-' + option.csid));
                });

            },
            error: function (e) {
                console.log(e);
            }
        });
    });
});

function RemoveExportPIDetails(index) {

    const formData = $("#form").serializeArray();

    formData.push({ name: "RemoveIndex", value: index });
    formData.push({ name: "IsDelete", value: true });

    $.post("/CommercialExport/AddOrRemovePIDetails", formData, function (partialView) {
        piDetails.html(partialView);
    }).fail(function () {
        toastr.error(errors[0].message, errors[0].title);
    });
}

var updateRow = function (selector) {
    $("[type='hidden']", selector).prop("type", "text").addClass("col-xs-6 col-sm-3 mr-2");
};