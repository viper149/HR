$(function () {

    var fabcode = $("#LoomSettingStyleWiseM_FABCODE");

    getStyleDetails();

    $("#btnAddChannel").on('click',
        function () {
            $.ajax({
                async: true,
                cache: false,
                data: $('#form').serialize(),
                type: "POST",
                url: "/LoomSettingStyleWise/AddChannel",
                success: function (partialView, status, xhr) {
                    //console.log(partialView);
                    $('#channelDetails').html(partialView);
                },
                error: function (e) {
                    console.log(e);
                }
            });
        });



    $("#LoomSettingStyleWiseM_BREAKS_CMPX_WARP").on('change',
        function () {

            GetCrimpxTotal($("#LoomSettingStyleWiseM_BREAKS_CMPX_WARP").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_WEFT").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_CATCH_CORD").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_OTHERS_BREAKS").val());
        });

    $("#LoomSettingStyleWiseM_BREAKS_CMPX_WEFT").on('change',
        function () {

            GetCrimpxTotal($("#LoomSettingStyleWiseM_BREAKS_CMPX_WARP").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_WEFT").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_CATCH_CORD").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_OTHERS_BREAKS").val());
        });

    $("#LoomSettingStyleWiseM_BREAKS_CMPX_CATCH_CORD").on('change',
        function () {

            GetCrimpxTotal($("#LoomSettingStyleWiseM_BREAKS_CMPX_WARP").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_WEFT").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_CATCH_CORD").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_OTHERS_BREAKS").val());
        });

    $("#LoomSettingStyleWiseM_BREAKS_CMPX_OTHERS_BREAKS").on('change',
        function () {


            GetCrimpxTotal($("#LoomSettingStyleWiseM_BREAKS_CMPX_WARP").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_WEFT").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_CATCH_CORD").val(), $("#LoomSettingStyleWiseM_BREAKS_CMPX_OTHERS_BREAKS").val());
        });

    fabcode.on('change',
        function () {
            getStyleDetails();
        });


    $("#LoomSettingChannelInfo_COUNT").on('change',
        function () {
            var selectedVal = $(this).val();
            $.ajax({
                async: true,
                cache: false,
                data: { "countId": selectedVal },
                type: "GET",
                url: "/LoomSettingStyleWise/GetCountDetails",
                success: function (data, status, xhr) {
                    console.log(data);
                    $("#LoomSettingChannelInfo_LOT").val(data.lotid).trigger("change");
                    $("#LoomSettingChannelInfo_SUPPLIER").val(data.suppid).trigger("change");
                    $("#LoomSettingChannelInfo_RATIO").val(data.ratio);

                },
                error: function (e) {
                    console.log(e);
                }
            });
        });

    function getStyleDetails() {

        var selectedVal = fabcode.val();
        $.ajax({
            async: true,
            cache: false,
            data: { "fabcode": selectedVal },
            type: "GET",
            url: "/LoomSettingStyleWise/GetStyleDetails",
            success: function (data, status, xhr) {
                $("#LoomSettingStyleWiseM_LOOM_TYPE").val(data.loomid).trigger("change");
                $("#weave").html(data.rnD_WEAVE.name);
                $("#dobby").html(data.dobby);
                $("#construction").html(data.remarks);

                $("#LoomSettingChannelInfo_COUNT").html('');
                $("#LoomSettingChannelInfo_COUNT").append('<option value="" selected>Select Count</option>');
                $.each(data.rnD_FABRIC_COUNTINFO,
                    function (id, option) {
                        if (option.yarnfor === "2") {
                            $("#LoomSettingChannelInfo_COUNT").append($('<option></option>').val(option.countid).html(option.count.countname));
                        }
                    });
            },
            error: function (e) {
                console.log(e);
            }
        });
    }
});


function GetCrimpxTotal(warp, weft, cord, breaks) {

    debugger;
    warp = warp === "" ? 0 : warp;
    weft = weft === "" ? 0 : weft;
    cord = cord === "" ? 0 : cord;
    breaks = breaks === "" ? 0 : breaks;

    $("#LoomSettingStyleWiseM_BREAKS_CMPX_TOTAL").val(parseFloat(warp) + parseFloat(weft) + parseFloat(cord) + parseFloat(breaks));
}