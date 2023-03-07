
$(function () {

    var mushakDate = $("#ComExCashInfo_VCDATE");
    var deliveryDate = $("#ComExCashInfo_DELIVERY_DATE");

    mushakDate.on("change keyup", function () {
        deliveryDate.val(mushakDate.val());
    });

});