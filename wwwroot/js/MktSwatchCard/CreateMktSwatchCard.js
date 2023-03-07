$(".team").change(function () {

    const selectedItem = $(this).val();

    $.ajax({
        type: "GET",
        url: "/MktSdrfInfo/GetTeamMembers",
        data: { "teamid": selectedItem },
        success: function (data) {
            if (data != null) {
                console.log(data);
                $("#MktSwatchCard_MKTPERSON").html("");
                $("#MktSwatchCard_MKTPERSON").append('<option value="" selected>Select Marketing Person</option>');

                $.each(data,
                    function (id, option) {
                        $("#MktSwatchCard_MKTPERSON").append($("<option></option>").val(option.mkT_TEAMID).html(option.persoN_NAME));
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