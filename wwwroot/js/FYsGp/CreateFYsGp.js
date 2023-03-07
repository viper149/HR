
var lcid = $("#f_YS_GP_MASTER_LC_ID");

var countId = $("#f_YS_GP_DETAILS_COUNTID");

var lotId = $("#f_YS_GP_DETAILS_LOTID");

var indslId = $("#f_YS_GP_DETAILS_INDSLID");


$(function () {

    lcid.on("change", function () {

        if (lcid.val()) {

            var formData = {
                "id": lcid.val()
            }

            $.get("/FYsGp/GetLcInfo", formData, function (data) {

                console.log(data);

                $("#lcdate").text(moment(data.lcdate).format('DD-MM-YYYY'));

                $("#supp").text(data.supp.suppname);

                

            }).fail(function () {

            });

        }

    });






    countId.on("change", function () {

        if (countId.val()) {

            var formData = {
                "countId": countId.val()
            }

            $.get("/FYsGp/GetYarnIndentDetails", formData, function (data) {

                if (typeof data !== "undefined") {
                    indslId.html("");
                    indslId.append('<option value="" selected>Select Lot No</option>');
                    $.each(data, function (index, value) {
                        /*console.log(data[0].basYarnLotinfo.lotid);*/
                        //lotId.val(data[0].basYarnLotinfo.lotid).trigger("change");
                        
                        indslId.append($("<option>",
                            {
                                value: value.trnsid,
                                text: value.opT1
                            }));

                        /*console.log(value.basYarnLotinfo.lotid);*/

                        lotId.append($("<option>",
                            {
                                

                                value: value.basYarnLotinfo.lotid,
                                text: value.basYarnLotinfo.lotno
                            }));
                    });
                }
            }).fail(function () {

            });

        }

    });



    indslId.on("change", function () {

        if (indslId.val()) {

            var formData = {
                "lotid": indslId.val()
            }

            $.get("/FYsGp/GetLotId", formData, function (data) {

                console.log(data);
                console.log(data[0].trnsid);
                lotId.val(data[0].basYarnLotinfo.lotid).trigger("change");


            }).fail(function () {

            });

        }

    });



    $("#addToList").on("click", function () {

    var formData = $("#form").serializeArray();


        $.post("/FYsGp/AddToList",
            formData,
            function (partialView) {
                $("#FysGpPartialId").html(partialView);
            }).fail(function () {
               
            });
    });



});

function RemoveDS(index) {
    swal({
        title: "Please Confirm",
        text: `You won't able to revert, Are you sure to remove item no. - ${index + 1}`,
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {
            if (isConfirm) {

                var formData = $('#form').serializeArray();

                formData.push({ name: "IsDelete", value: true });
                formData.push({ name: "RemoveIndex", value: index });

                $.post("/FYsGp/AddToList", formData, function (partialView) {
                    $("#FysGpPartialId").html(partialView);
                }).fail(function () {
                    
                });
            }
        });
}