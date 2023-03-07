
var progNo = $("#RndFabtestBulk_PROG_NO");
var trollyNo = $("#RndFabtestBulk_TROLLEY_NO");
var errors = {
    0: {
        title: "Invalid Submission",
        message: "We can not process your data! Please try again."
    }
}
$(function () {
    getByProgNoEdit();
    getByTrollyNo();
});

progNo.on("change", function () {
    getByProgNo();
});

trollyNo.on("change", function () {
    getByTrollyNo();
});

function getByProgNo() {

    if (progNo.val()) {
        var formData = {
            "setId": progNo.val()
        };
        $.post("/RndFabTestBulk/GetSetDetailsBySetId", formData, function (data) {
            console.log(data);
            $("#style").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.stylE_NAME);
            $("#pRoute").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.finisH_ROUTE);
            $("#buyer").text(data.proG_.blK_PROG_.rndProductionOrder.so.pimaster.buyer.buyeR_NAME);
            $("#order").text(data.proG_.blK_PROG_.rndProductionOrder.so.sO_NO);


            $("#shinkagewarp").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.srdecwarp);

            $("#shinkageweft").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.srdecweft);

            $("#stretchweft").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.stdecweft);

            $("#finishepi").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.fnepi);

            $("#finishppi").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.fnppi);

            $("#weight").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.wgdec);

            $("#width").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.widec);

            $("#growthwarp").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.grdecwarp);
            $("#growthweft").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.grdecweft);

            trollyNo.html("");
            trollyNo.append('<option value="" selected>Select Trolly No</option>');
            $.each(data.f_PR_FINISHING_PROCESS_MASTER, function (iddd, o) {
                $.each(o.f_PR_FINISHING_FNPROCESS, function (index, option) {
                    trollyNo.append($("<option>",
                        {
                            value: option.fiN_PROCESSID,
                            text: option.trollnoNavigation.name
                        }));
                });
            });
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}


function getByProgNoEdit() {

    if (progNo.val()) {
        var formData = {
            "setId": progNo.val()
        };
        $.post("/RndFabTestBulk/GetSetDetailsBySetId", formData, function (data) {
            console.log(data);
            $("#style").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.stylE_NAME);
            $("#pRoute").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.finisH_ROUTE);
            $("#buyer").text(data.proG_.blK_PROG_.rndProductionOrder.so.pimaster.buyer.buyeR_NAME);
            $("#order").text(data.proG_.blK_PROG_.rndProductionOrder.so.sO_NO);
            $("#shinkagewarp").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.srdecwarp);

            $("#shinkageweft").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.srdecweft);

            $("#stretchweft").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.stdecweft);

            $("#finishepi").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.fnepi);

            $("#finishppi").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.fnppi);

            $("#weight").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.wgdec);

            $("#width").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.widec);

            $("#growthwarp").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.grdecwarp);
            $("#growthweft").text(data.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.grdecweft);

        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

function getByTrollyNo() {
    if (trollyNo.val()) {
        var formData = {
            "id": trollyNo.val()
        };
        $.post("/RndFabTestBulk/GetFnProcessDetailsById", formData, function (data) {
            console.log(data);
            $("#lengthS").text(data.lengtH_OUT);
            $("#machine").text(data.fN_MACHINE.name);
            $("#loom").text(data.fN_PROCESS.doff.looM_NONavigation.looM_NO);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}