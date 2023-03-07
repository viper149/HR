$(function () {
    var invoiceId = $("#ComExCertificateManagement_INVID");

    var errors = {
        0: {
            title: "Invalid Submission",
            message: "We can not process your data! Please try again."
        }
    }

    getByInvId();

    invoiceId.on("change", function () {
        getByInvId();
    });

    function getByInvId() {
        if (invoiceId.val()) {

            var formData = {
                "id": invoiceId.val()
            }

            $.get("/ComExCertificateManagement/GetAllByINVNO", formData, function (data) {

                let sumRoll = 0;
                let sumQty = 0;
                let sumAmount = 0;

                console.log(data);
                
                $("#lcno").text(data.lc.lcno);

                $("#fileno").text(data.lc.fileno);

               

                if (data.deldate !== null) {

                    $("#invdate").text(moment(data.invdate).format('DD-MM-YYYY'));
                }
                else {
                    $("#invdate").text("");
                }


                $("#invduration").text(data.invduration);
                $("#pdocno").text(data.pdocno);
                $("#doC_NOTES").text(data.doC_NOTES);

                if (data.negodate !== null) {

                    $("#negodate").text(moment(data.negodate).format('DD-MM-YYYY'));
                }
                else {
                    $("#negodate").text("");
                }

                $("#fileno").text(data.fileno);

                if (data.prcdate !== null) {

                    $("#prcdate").text(moment(data.prcdate).format('DD-MM-YYYY'));
                }
                else {
                    $("#prcdate").text("");
                }


                if (data.deldate !== null) {

                    $("#deldate").text(moment(data.deldate).format('DD-MM-YYYY'));
                }
                else {
                    $("#deldate").text("");
                }
                

                if (data.doC_RCV_DATE !== null) {

                    $("#doC_RCV_DATE").text(moment(data.doC_RCV_DATE).format('DD-MM-YYYY'));
                }
                else {
                    $("#doC_RCV_DATE").text("");
                }


                if (data.doC_SUB_DATE !== null) {

                    $("#doC_SUB_DATE").text(moment(data.doC_SUB_DATE).format('DD-MM-YYYY'));
                }
                else {
                    $("#doC_SUB_DATE").text("");
                }

                if (data.bilL_DATE !== null) {

                    $("#bilL_DATE").text(moment(data.bilL_DATE).format('DD-MM-YYYY'));
                }
                else {
                    $("#bilL_DATE").text("");
                }

                if (data.bnK_SUB_DATE !== null) {

                    $("#bnK_SUB_DATE").text(moment(data.bnK_SUB_DATE).format('DD-MM-YYYY'));
                }
                else {
                    $("#bnK_SUB_DATE").text("");
                }

                if (data.matudate !== null) {

                    $("#matudate").text(moment(data.matudate).format('DD-MM-YYYY'));
                }
                else {
                    $("#matudate").text("");
                }

                $("#banK_REF").text(data.banK_REF);
                $("#bnK_ACC_DATE").text(moment(data.bnK_ACC_DATE).format('DD-MM-YYYY'));

                if (data.bnK_ACC_DATE !== null) {

                    $("#bnK_ACC_DATE").text(moment(data.bnK_ACC_DATE).format('DD-MM-YYYY'));
                }
                else {
                    $("#bnK_ACC_DATE").text("");
                }

                $("#discrepancy").text(data.discrepancy);
                $("#usd").text(data.amounT_EURO);
                $("#usd").text(data.amounT_BDT);
                $("#buyername").text(data.lc.buyer.buyeR_NAME);
                var table = "";

                table += `<tr>

                   <th>FabCode</th>
                   <th>Style Name</th>
                   <th>Brand</th>
                   <th>Roll</th>
                   <th>Qty</th>
                   <th>Rate</th>
                   <th>Amount</th>
                  </tr>`;

                if (data != null) {
                    $.each(data.comExInvdetailses,
                        function (index, value) {

                            table += `<tr>

                    <td>${value.comExFabstyle.fabcode}</td>
                    <td>${value.comExFabstyle.fabcodeNavigation.stylE_NAME}</td>
                    <td>${value.comExFabstyle.brand.brandname}</td>
                    <td>${value.roll}</td>
                    <td>${value.qty}</td>
                    <td>${value.rate}</td>       
                    <td>${value.amount}</td>       
                    </tr>`;

                            sumRoll += value.roll;
                            sumQty += value.qty;
                            sumAmount += value.amount;                    
                        });

                    table += `<tr>
                    <td></td>
                    <td></td>
                    <td>Total:</td>
                    <td>${sumRoll}</td>
                    <td>${sumQty}</td>
                    <td></td>
                    <td>${sumAmount}</td>
                    </tr>`;
                }
                $("#rollList").html(table);
                
            }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        }
    }
});
