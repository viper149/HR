
$(function () {

    var btnAdd = $("#btnAdd");
    var attachTo = $("#RndSampleInfoWeavingDetails");
    var rndSampleInfoWeavingProgId = $("#RndFabtestGrey_PROGID");
    var attachPreviousDataTo = $("#PreviousData");

    btnAdd.on("click", function () {
        $.ajax({
            async: true,
            cache: false,
            data: $("#form").serialize(),
            type: "POST",
            url: "/RndFabTestGrey/AddRndSampleInfoWeavingTable",
            success: function (partialView,status, xhr) {
                attachTo.html(partialView);
                $("#RndFabtestGrey_DEVELOPMENTNO").val(xhr.getResponseHeader("DEV_NO"));
            },
            error: function () {
                console.log("failed to attach...");
            }
        });
    });

    rndSampleInfoWeavingProgId.on("change", function () {

        if (rndSampleInfoWeavingProgId.val()) {
            $.ajax({
                async: true,
                cache: false,
                data: { "sdId": $(this).val() },
                type: "GET",
                url: "/RndFabTestGrey/LoadDetails",
                success: function (partialView,status,xhr) {
                    attachPreviousDataTo.html(partialView);
                    toastr.success("Preview pane added below.", "Found!!");
                    $("#RndFabtestGrey_GREPI").val(xhr.getResponseHeader("GREPI"));
                    $("#RndFabtestGrey_GRPPI").val(xhr.getResponseHeader("GRPPI"));
                    $("#RndFabtestGrey_DEVELOPMENTNO").val(xhr.getResponseHeader("DEV_NO"));
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });
        } else {
            attachPreviousDataTo.empty();
            toastr.warning("Please select a Program Number.", "Invalid Input");
        }
    });
});

function removeFromList(index) {

    var attachTo = $("#RndSampleInfoWeavingDetails");
    var fData = $("#form").serializeArray();

    fData.push({ name: "RemoveIndex", value: index });
    fData.push({ name: "IsDelete", value: true });

    $.ajax({
        async: true,
        cache: false,
        data: fData,
        type: "POST",
        url: "/RndFabTestGrey/AddRndSampleInfoWeavingTable",
        success: function (partialView) {
            attachTo.html(partialView);
        },
        error: function () {
            console.log("failed to attach...");
        }
    });
}