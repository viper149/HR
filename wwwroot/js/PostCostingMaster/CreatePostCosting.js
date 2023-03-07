$(function() {
    $("#CosPostcostingMaster_SO_NO").on("change",function() {

        $.ajax({
            async: true,
            cache: false,
            data: $('#form').serialize(),
            type: "POST",
            url: "/CosPostCostingMaster/AddCountList",
            success: function (partialView, status, xhr) {
                $('#yarnConsumption').html(partialView);
                //console.log(partialView);
            },
            error: function (e) {
                console.log(e);
            }
        });

        $.ajax({
            async: true,
            cache: false,
            data: $('#form').serialize(),
            type: "POST",
            url: "/CosPostCostingMaster/AddChemicalList",
            success: function (partialView, status, xhr) {
                $('#ChemConsumption').html(partialView);
                //console.log(partialView);
            },
            error: function (e) {
                console.log(e);
            }
        });

        $.ajax({
            async: true,
            cache: false,
            data: {"soId":$(this).val()},
            type: "GET",
            url: "/CosPostCostingMaster/GetSoDetails",
            success: function (data, status, xhr) {
                //$("#CosPostcostingMaster_PRODUCTION_QTY").val(data.comExPiDetails.qty);
                console.log(data);
            },
            error: function (e) {
                console.log(e);
            }
        });

        $.ajax({
            async: true,
            cache: false,
            data: {"soId":$(this).val()},
            type: "GET",
            url: "/CosPostCostingMaster/GetSoEliteQty",
            success: function (data, status, xhr) {
                $("#CosPostcostingMaster_PRODUCTION_QTY").val(data);
                console.log(data);
            },
            error: function (e) {
                console.log(e);
            }
        });
    });
});