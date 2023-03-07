
var poid = $("#f_PR_WEAVING_OS_POID");


$(function () {


    if (poid.val()) {
        getSoDetails();
    }

    function getSoDetails() {
        const Poid = poid.val();
        if (Poid) {
            $.get("/YarnRequirement/GetCountList", { 'poId': Poid }, function (data) {


                $("#fabcode").text(data.piDetails.style.fabcodeNavigation.stylE_NAME);
                $("#buyer").text(data.piDetails.pimaster.buyer.buyeR_NAME);

            }).fail(function () {

            });
        }
    }

    poid.on("change", function () {

        getSoDetails();

    });

});