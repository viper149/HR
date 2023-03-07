
$(function () {
    var gspNo = $("#ComExGspInfo_GSPNO");
    var invDate = $("#INVDATE");
    var expLcNo = $("#ComExGspInfo_EXPLCNO");
    var expLcDate = $("#ComExGspInfo_EXPLCDATE");
    var lcAmdDate = $("#ComExGspInfo_LCAMDDATE");
    var expItem = $("#ComExGspInfo_EXPITEMS");
    var fabDescription = $("#ComExGspInfo_FABDESCRIPTION");
    var itemQtyYds = $("#ComExGspInfo_ITEMQTY_YDS");
    var itemQtyMts = $("#ComExGspInfo_ITEMQTY_MTS");
    var issueDate = $("#ComExGspInfo_ISSUEDATE");

    gspNo.on("change", function () {

        var formData = $("#form").serializeArray();

        $.post("/CommercialExport/GSP/GetForGSPInformation", formData, function (data) {
            invDate.html(data.invdate.substring(0, 10));
            expLcNo.val(data.explcno);
            expLcDate.val(data.explcdate.substring(0, 10));
            (data.amtdate ? lcAmdDate.val(`${data.lcdate.substring(0, 10)}, AMD: ${data.amtno}, DT. ${data.amtdate.substring(0, 10)}`) : lcAmdDate.val(`${data.lcdate.substring(0, 10)}`));
            expItem.val(data.expitems);
            fabDescription.val(data.fabdescription);
            itemQtyYds.val(Math.round(data.itemqtY_YDS));
            itemQtyMts.val(Math.round(data.itemqtY_MTS));
            issueDate.val(new Date().toISOString().slice(0, 10));
           
        });
        $.post("/CommercialExport/GSP/GetGSPNo", formData, function (data) {
            
        });

    });
    $(function () {

        $('#form').submit(function () {
            $(this).find(':submit').attr('disabled', 'disabled');
        });
    });

    $("#ComExGspInfo_INVID").change(function () {

        var selectedItem = $(this).val();

        $.get("/ComExGspInfo/GetInvoiceInfo", { "invId": selectedItem }, function (data) {

            if (data.comExInvoiceMaster.invdate) {
                var invDate = data.comExInvoiceMaster.invdate.substring(0, data.comExInvoiceMaster.invdate.indexOf('T'));
                $('#invoiceDate').html(invDate);
                $('#dateOfDelivery').val(invDate);

            }
            
            $('#fileNo').html(data.comExInvoiceMaster.lc.fileno);
            $('#duration').html(data.comExInvoiceMaster.invduration);
            $('#lcNo').html(data.comExInvoiceMaster.lc.lcno);
            $('#lcDate').html(`${data.comExInvoiceMaster.lc.lcdate.substring(0,10)}`);
            $('#lcValue').html(data.comExInvoiceMaster.lc.value);
            $('#invValue').html(data.comExInvoiceMaster.inV_AMOUNT);
            $('#party').html(data.comExInvoiceMaster.buyer.buyeR_NAME);
            $('#docNo').html(data.comExInvoiceMaster.pdocno);
            $('#notes').html(data.comExInvoiceMaster.doC_NOTES);
            
        });
    });

});