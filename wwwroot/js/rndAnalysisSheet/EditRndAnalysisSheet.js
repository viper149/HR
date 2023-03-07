
$(function () {

    const btnAdd = $("#btnAdd");
    const yarnFor = $("#RndAnalysisSheetDetails_YARN_FOR");
    const userDefinedTotalEnds = $("#RndAnalysisSheet_UTOTAL_ENDS");
    const userDefinedReedSpace = $("#RndAnalysisSheet_UREED_SPACE");
    const superiorDefinedTotalEnds = $("#RndAnalysisSheet_TOTAL_ENDS");
    const superiorDefinedReedSpace = $("#RndAnalysisSheet_REED_SPACE");
    const submitBtn = $("button[type='submit']");
    const customDataGroup = $(".custom-data-group");

    submitBtn.prop("disabled", true);

    customDataGroup.on("change keyup", function () {

        if (userDefinedTotalEnds.val() &&
            superiorDefinedTotalEnds.val() &&
            userDefinedReedSpace.val() &&
            superiorDefinedReedSpace.val() &&
            userDefinedTotalEnds.val() === superiorDefinedTotalEnds.val() &&
            userDefinedReedSpace.val() === superiorDefinedReedSpace.val()) {

            submitBtn.prop("disabled", false);
        } else {
            submitBtn.prop("disabled", true);
        }
    });

    yarnFor.on("change", function () {
        $(this).val() === "Warp" ? $(".warp").removeClass("d-none") : $(".warp").addClass("d-none");
        $(this).val() === "Weft" ? $(".weft").removeClass("d-none") : $(".weft").addClass("d-none");
    });

    btnAdd.on("click", function () {
        const data = $("#form").serializeArray();

        $.post("/RndAnalysisSheet/AddYarnDetails", data, function (partialView) {
            $("#yarnDetails").html(partialView);
        });
    });

    $("#RndAnalysisSheet_SWATCH_ID").on("change", function () {
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