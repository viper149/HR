$(function () {
    // Get the value from a set of radio buttons
    $(".dev_type").change(function () {
        console.log($(this).val());
    });

    $(".for").change(function () {
        console.log($(this).val());
    });

    $("#MktSdrfInfo_WEIGHT_AW").change(function () {
        const data = $(this).val();
        if (!isNaN(data)) {
            $("#MktSdrfInfo_GSM_AW").val(Math.round(data * 33.93));
        } else {
            $("#MktSdrfInfo_GSM_AW").val(data);
        }
    });

    $("#MktSdrfInfo_WEIGHT_BW").change(function () {
        const data = $(this).val();
        if (!isNaN(data)) {
            $("#MktSdrfInfo_GSM_BW").val(Math.round(data * 33.93));
        } else {
            $("#MktSdrfInfo_GSM_BW").val(data);
        }
    });

    $(".team").change(function () {
        const selectedItem = $(this).val();
        $.ajax({
            type: "GET",
            url: "/MktSdrfInfo/GetTeamMembers",
            data: { "teamid": selectedItem },
            success: function (data) {
                if (data != null) {
                    console.log(data);
                    $("#MktSdrfInfo_MKT_PERSON_ID").html("");
                    $("#MktSdrfInfo_MKT_PERSON_ID").append('<option value="" selected>Select Related Person</option>');

                    $.each(data,
                        function (id, option) {
                            $("#MktSdrfInfo_MKT_PERSON_ID").append($("<option></option>").val(option.mkT_TEAMID).html(option.persoN_NAME));
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

    $("#MktSdrfInfo_AID").change(function () {
        const selectedItem = $(this).val();
        $.ajax({
            type: "GET",
            url: "/MktSdrfInfo/GetAnalysisDetails",
            data: { "aid": selectedItem },
            success: function (data) {
                if (data != null) {
                    console.log(data);
                    $("#MktSdrfInfo_BUYER_REF").val(data.buyeR_REF).css("border", "#006400 solid 1px");
                    $("#MktSdrfInfo_MKT_PERSON_ID").val(data.mkT_PERSON_ID).css("border", "#006400 solid 1px");
                    $("#MktSdrfInfo_BUYERID").val(data.buyerid).css("border", "#006400 solid 1px");
                    $("#MktSdrfInfo_CONSTRUCTION").val(data.construction).css("border", "#006400 solid 1px");
                    $("#MktSdrfInfo_WIDTH").val(data.fN_WIDTH).css("border", "#006400 solid 1px");
                    $("#MktSdrfInfo_WEIGHT_AW").val(data.wA_WEIGHT).css("border", "#006400 solid 1px");
                    $("#MktSdrfInfo_FINID").val(data.finid).css("border", "#006400 solid 1px");
                    $("#MktSdrfInfo_COLOR").val(data.colorcode).css("border", "#006400 solid 1px");
                } else {
                    toastrNotification("Sorry! No Analysis Details Found.", "error");
                    $("#form :input").prop("disabled", true);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                toastrNotification("Error: Failed To Retrieve Analysis Details.", "error");
                $("#form :input").prop("disabled", true);
            }
        });
    });

    $("#MktSdrfInfo_ACTUAL_DATE").change(function () {
        var selectedItem = $(this).val();
        var previousDate = this.defaultValue;
        $.ajax({
            type: "GET",
            url: "/MktSdrfInfo/GetAvailableDate",
            data: { "date": selectedItem },
            success: function (data) {
                if (data) {
                    toastrNotification("Selected date is available", "success");
                } else {
                    if (selectedItem !== previousDate) {
                        toastrNotification("Sorry! Selected date is not available.", "error");
                    }
                    $("#MktSdrfInfo_ACTUAL_DATE").val(previousDate);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (selectedItem !== previousDate) {
                    toastrNotification("Error: Please Contact With Developer.", "error");
                }
                $("#MktSdrfInfo_ACTUAL_DATE").val(previousDate);
            }
        });
    });

});