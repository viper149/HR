
$(function () {

    var indentNo = $("#ProcWorkOrderDetails_INDENTNO");
    var prodId = $("#ProcWorkOrderDetails_PRODID");
    var prodName = $("#ProcWorkOrderDetails_PRODNAME");

    indentNo.on("change", function () {

        var formData = $("#form").serializeArray();
        
        $.post("/ProcWorkOrder/GetIndentProductInfo", formData, function (data) {

            //console.log(data);

            prodId.val(data.f_GS_PRODUCT_INFORMATION[0].prodid);
            prodName.val(data.f_GS_PRODUCT_INFORMATION[0].prodname);
        });
    });

});