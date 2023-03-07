$(function () {

    var scId = $("#scNo");
    var lcId = $("#ACC_LOCAL_DOMASTER_LCID");
    var isConsumption = $("#IS_CONSUMPTION");
    var styleId = $("#styleID");
    var qty = $("#qty");
    var rate = $("#rate");
    var amount = $("#amount");
    var remarks = $("#remarks");

    isConsumption.on("change", function () {
        if (!this.checked) {
            lcId.val(null).trigger('change');
        }
    });

    styleId.on("change", function () {

        if ($(this).val()) {

            const formData = $("#form").serializeArray();

            $.post("/AccountDO/Local/GetOtherInfo", formData, function (data) {

                qty.val(data.qty);
                rate.val(data.unitprice);
                amount.val(data.total);
                remarks.val(data.remarks);
            });
        }
    });

    lcId.on("change", function () {

        if ($(this).val()) {

            /*var formData = $("#form").serializeArray();*/
            var formData = {
                "lcId": $(this).val()
            }

            $.post("/AccountDO/Local/GetStyles", formData, function (data) {
                styleId.val(null).trigger('change');

                $.each(data, function (iddd, o) {
                    
                    $.each(o.pi.coM_EX_PI_DETAILS, function (index, option) {
                        styleId.append($("<option>",
                            {
                                value: option.trnsid,
                                text: option.style.fabcodeNavigation.stylE_NAME
                            }));
                    });
                });
            });
        } else {
            styleId.val(null).trigger('change');
        }
    });

    lcId.select2({
        ajax: {
            url: "/AccountDO/Local/GetCommercialExportLC",
            type: "POST",
            data: function (params) {

                var formData = $("#form").serializeArray();

                formData.push({ name: "search", value: params.term });
                formData.push({ name: "search", value: params.page || 1 });

                return formData;
            },
            processResults: function (data) {

                return {
                    results: $.map(data.comExLcinfos, function (item) {
                        return {
                            id: item.lcid,
                            text: item.lcno
                        };
                    })
                };
            }
        }
    });

    scId.select2({
        ajax: {
            url: "/AccountDO/Local/GetLocalSaleOrders",
            data: function (params) {

                var query = {
                    search: params.term,
                    page: params.page || 1
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.coM_EX_SCINFOs, function (item) {
                        return {
                            id: item.scid,
                            text: item.scno
                        };
                    })
                };
            }
        }
    });

    $(function () {

        $('#form').submit(function () {
            $(this).find(':submit').attr('disabled', 'disabled');
        });
    });


    $("#qty").on("change", function () {
        $('#amount').val($('#qty').val() * $('#rate').val());
    });

    $("#rate").on("change", function () {
        $('#amount').val($('#qty').val() * $('#rate').val());
    });

    $("#btnAdd").on('click', function () {
        $.ajax({
            async: true,
            cache: false,
            data: $('#form').serializeArray(),
            type: "POST",
            url: '/AccLocalDoMaster/AddDoDetails',
            success: function (partialView) {
                $('#doDetails').html(partialView);
            },
            error: function (e) {
                console.log(e);
            }
        });
    });

    $("#scNo").on("change", function () {

        var selectedItem = $(this).val();

        $.ajax({
            type: "GET",
            url: "/AccLocalDoMaster/GetScInfo",
            data: { "scId": selectedItem },
            success: function (data) {

                if (data != null) {

                    $('#scDate').html(new Date(data.scdate).toISOString().slice(0, 10));

                    $('#seller').html(data.scperson);
                    $('#buyer').html(data.buyer.buyeR_NAME);
                    $('#address').html(data.buyer.address);

                    $("#styleID").html('');

                    $.each(data.coM_EX_SCDETAILS, function (id, option) {
                        $("#styleID").append($('<option></option>').val(option.trnsid).html(option.style.stylename));
                    });


                    $('#qty').val(data.coM_EX_SCDETAILS[0].qty);
                    $('#rate').val(data.coM_EX_SCDETAILS[0].rate);
                    $('#amount').val(data.coM_EX_SCDETAILS[0].amount);
                    if (data.coM_EX_SCDETAILS[0].qty <= 0) {
                        $('#qty').css("border", "2px solid red");
                    }
                } else {
                    toastrNotification("Sorry! No Order Information.", "error");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                toastrNotification("Error: Failed To Retrieve Order Information.", "error");
            }
        });
    });
});