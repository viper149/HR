
var setId = $("#FFsClearanceMasterSampleRoll_SETID");
var rollId = $("#FFsClearanceMasterSampleRoll_ROLLID");
var errors = {
    0: {
        title: "Invalid Submission",
        message: "We can not process your data! Please try again."
    }
}

getBySetNo();
getByRollNo();

setId.on("change", function () {
    getBySetNo();
});

rollId.on("change", function () {
    getByRollNo();
});

function getBySetNo() {

    if (setId.val()) {
        var formData = {
            "setId": setId.val()
        };
        $.post("/ClearanceMasterSampleRoll/GetSetDetails", formData, function (data) {
            $("#style").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.stylE_NAME);
            $("#fabWeave").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.rnD_WEAVE.name);
            $("#fabColor").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.colorcodeNavigation.color);
            $("#finRoute").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.finisH_ROUTE);
            $("#reqWeight").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.wgdec);
            $("#reqShrWarp").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.srdecwarp);
            $("#reqShrWeft").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.srdecweft);
            $("#reqEpi").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.fnepi);
            $("#reqPpi").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.fnppi);
            $("#reqStrWarp").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.stdecwarp);
            $("#reqStrWeft").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.stdecweft);
            $("#reqWidth").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.widec);
            $("#reqSpA").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.fabtest.spiralitY_A);
            $("#reqSpB").text(data.proG_.blK_PROG_.rndProductionOrder.so.style
                .fabcodeNavigation.fabtest.spiralirtY_B);
            $("#fabCons").text(data.opT1);

            rollId.html("");
            rollId.append('<option value="" selected>Select Roll No</option>');
            $.each(data.f_PR_INSPECTION_PROCESS_MASTER[0].f_PR_INSPECTION_PROCESS_DETAILS,
                function (index, option) {
                    rollId.append($("<option>",
                        {
                            value: option.rolL_ID,
                            text: option.rollno
                        }));
                });
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}

function getByRollNo() {
    if (rollId.val()) {
        var formData = {
            "rollId": rollId.val()
        };
        $.post("/ClearanceMasterSampleRoll/GetRollDetails", formData, function (data) {
            $("#batch").text(data.batch);
            $("#rollLength").text(data.lengtH_YDS);
        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}