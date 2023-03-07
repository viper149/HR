var piId = $("#f_FS_FABRIC_RETURN_RECEIVE_PI_NO");
var styleId = $("#f_FS_FABRIC_RETURN_RECEIVE_FABCODE");

$(function () {


    //if (piId.val()) {

    //    GetStyleByPi();

    //}

    piId.on("change", function () {

        GetStyleByPi();

    });


    function GetStyleByPi() {

        styleId.html('');

        if (piId.val()) {

            var formData = {
                "pi": piId.val()
            }

            $.get("/FFsFabricReturnReceive/GetStyleByPi", formData, function (data) {

                /*  console.log(data);*/



                $.each(data[0].coM_EX_PI_DETAILS,
                    function (id, option) {
                        console.log(option);
                        styleId.append($('<option></option>').val(option.style.fabcodeNavigation.fabcode).html(option.style.fabcodeNavigation.stylE_NAME));

                    });


                /*styleId.val(ddata.coM_EX_PI_DETAILS[0].style.fabcodeNavigation.fabcode).trigger("change");*/

            }).fail(function () {

            });

        }
    }

});