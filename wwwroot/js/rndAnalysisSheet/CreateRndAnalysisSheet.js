
$(function () {
    $("#RndAnalysisSheetDetails_YARN_FOR").on("change",
        function () {
            const yarnFor = $(this).val();
            yarnFor === "Warp" ? $(".warp").removeClass("d-none") : $(".warp").addClass("d-none");
            yarnFor === "Weft" ? $(".weft").removeClass("d-none") : $(".weft").addClass("d-none");
        });

    $("#ComImpCsRatDetails_RATE").on("change",
        function () {
            $("#ComImpCsRatDetails_TOTAL").val($("#ComImpCsItemDetails_QTY").val() * $("#ComImpCsRatDetails_RATE").val());
        });

    $("#btnAdd").on("click",
        function () {
            $.ajax({
                async: true,
                cache: false,
                data: $("#form").serialize(),
                type: "POST",
                url: "/RndAnalysisSheet/AddYarnDetails",
                success: function (partialView) {
                    $("#yarnDetails").html(partialView);
                },
                error: function (e) {
                    console.log(e);
                }
            });
        });

    $("#RndAnalysisSheet_SWATCH_ID").on("change",
        function () {
            const swatchId = $(this).val();

            $.ajax({
                async: true,
                cache: false,
                data: { "swatchId": swatchId },
                type: "GET",
                url: "/RndAnalysisSheet/GetSwatchCardDetails",
                success: function (data) {
                    console.log(data);

                    $("#RndAnalysisSheet_MKT_PERSON_ID").val(data.mktperson);
                    $("#MKT_PERSON_ID").text(data.team.persoN_NAME);

                    $("#RndAnalysisSheet_MKT_QUERY_NO").val(data.mktquery);
                    $("#MKT_QUERY_NO").text(data.mktquery);
                },
                error: function (e) {
                    console.log(e);
                }
            });
        });

});