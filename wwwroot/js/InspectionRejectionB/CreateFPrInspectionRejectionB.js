
var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

$(function() {
    var doffId = $("#FPrInspectionRejectionB_DOFF_ID");
    var searchDate = $("#FPrInspectionRejectionB_SearchDate");


    //$("#FPrInspectionRejectionB_DOFF_ID").on('change', function (e) {
    //});

    getBywdId();
    
    doffId.on('select2:select', function (e) {
        $('#purpose1').focus();
    });

    $("#purpose1").keyup(function (e) {
        if (e.keyCode === 37) {
            doffId.select2('focus');
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionRejectionB_REDECTION_YDS').get(0).focus();
        }
    });
    

    $("#FPrInspectionRejectionB_REDECTION_YDS").keyup(function (e) {
        if (e.keyCode === 37) {
            doffId.select2('focus');
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionRejectionB_SECTION_ID').focus();
        }
    });


    $('#FPrInspectionRejectionB_SECTION_ID').on('select2:select', function (e) {
        $('#FPrInspectionRejectionB_DEFECT_ID').focus();
    });

    $('#FPrInspectionRejectionB_DEFECT_ID').on('select2:select', function (e) {
        $('#FPrInspectionRejectionB_SHIFT').focus();
    });

    $('#FPrInspectionRejectionB_DEFECT_ID').on('select2:select', function (e) {
        $('#FPrInspectionRejectionB_SHIFT').focus();
    });


    $('#FPrInspectionRejectionB_SHIFT').on('select2:select', function (e) {
        $('#purpose2').focus();
    });

    $("#purpose2").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionRejectionB_DEFECT_ID').select2('focus');
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionRejectionB_REMARKS').get(0).focus();
        }
    });


    searchDate.on("change",
        function () {
            var data = $('#form').serializeArray();
            $.get("/FPrInspectionRejectionB/GetDoffByInspectionDate",
                data,
                function (data) {
                    var doff = $("#FPrInspectionRejectionB_DOFF_ID").val();

                    $("#FPrInspectionRejectionB_DOFF_ID").html('');
                    $("#FPrInspectionRejectionB_DOFF_ID").append('<option value="" selected>Select Style Name</option>');
                    $.each(data.styleSetLoomList,
                        function (id, option) {
                            $("#FPrInspectionRejectionB_DOFF_ID").append($('<option></option>').val(option.id).html(option.name));
                        });

                    $("#FPrInspectionRejectionB_DOFF_ID").val(doff).trigger("change");
                }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });


            //$.get("/FPrInspectionRejectionB/GetFPrInspectionRejectionB",
            //    data,
            //    function (partialView,type,xhr) {
            //        $('#getList').html(partialView);

            //    }).fail(function () {
            //    toastr.error(errors[0].message, errors[0].title);
            //});

        });
    doffId.on("change",
        function () {
            getBywdId();
        });



    function getBywdId() {
        var fOperator = $("#fOperator");
        var color = $("#color");
        var fMachine = $("#fMachine");

        if (doffId.val()) {

            var formData = {
                "wdId": doffId.val()
            }

            $.get("/FPrInspectionRejectionB/GetAllByDoffId",
                formData,
                function (data) {
                    console.log(data);
                    fOperator.text(data.f_PR_FINISHING_PROCESS_MASTER[0] == null ? "" : data.f_PR_FINISHING_PROCESS_MASTER[0].f_PR_FINISHING_FNPROCESS[0].procesS_BYNavigation.firsT_NAME);
                    color.text(data.f_PR_FINISHING_PROCESS_MASTER[0] == null ? "" : data.f_PR_FINISHING_PROCESS_MASTER[0].fabricinfo.colorcodeNavigation.color);
                    fMachine.text(data.f_PR_FINISHING_PROCESS_MASTER[0] == null ? "" : data.f_PR_FINISHING_PROCESS_MASTER[0].f_PR_FINISHING_FNPROCESS[0].fN_MACHINE.name);

                    $("#program").text(data.wV_BEAM.wV_PROCESS.set.proG_.proG_NO);
                    $("#loom").text(data.looM_NONavigation.looM_NO);
                    $("#doffingDate").text(data.dofF_TIME.split('T')[0]);
                    $("#doffingLength").text(data.lengtH_BULK);
                    $("#finishDate").text(data.f_PR_FINISHING_PROCESS_MASTER[0].f_PR_FINISHING_FNPROCESS[0].fiN_PROCESSDATE.split('T')[0]);
                    $("#finishOperator").text(data.f_PR_FINISHING_PROCESS_MASTER[0].f_PR_FINISHING_FNPROCESS[0].procesS_BYNavigation.firsT_NAME);
                    $("#finishMc").text(data.f_PR_FINISHING_PROCESS_MASTER[0].f_PR_FINISHING_FNPROCESS[0].fN_MACHINE.name);
                    $("#color").text(data.f_PR_FINISHING_PROCESS_MASTER[0].fabricinfo.colorcodeNavigation.color);

                }).fail(function () {
                toastr.error(errors[0].message, errors[0].title);
            });
        }
    }
});