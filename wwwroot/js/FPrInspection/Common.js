
var targets = [];

targets.push(
    //$("#FPrInspectionProcessDetails_BATCH"),
    //$("#FPrInspectionProcessDetails_ROLLNO"),
    $("#FPrInspectionProcessDetails_LENGTH_1"),
    $("#FPrInspectionProcessDetails_LENGTH_2"),
    $("#FPrInspectionDefectPoint_POINT1"),
    $("#FPrInspectionDefectPoint_POINT2"),
    $("#FPrInspectionDefectPoint_POINT3"),
    $("#FPrInspectionDefectPoint_POINT4"),
    $("#FPrInspectionProcessDetails_ACT_WIDTH_INCH"),
    $("#FPrInspectionProcessDetails_CUT_WIDTH_INCH"),
    $("#FPrInspectionProcessDetails_NET_WEIGHT_KG"),
    $("#FPrInspectionProcessDetails_LENGTH_MTR"),
    $("#FPrInspectionProcessDetails_LENGTH_YDS"),
    //$("#FPrInspectionProcessDetails_TOTAL_DEFECT"),
    $("#FPrInspectionProcessDetails_PICES"),
    //$("#FPrInspectionProcessDetails_POINT_100SQ"),
    $("#FPrInspectionProcessDetails_WEIGHT_DEDUCT"),
    $("#FPrInspectionProcessDetails_GR_WEIGHT_KG"),
    $("#FPrInspectionProcessDetails_CUTPCS_YDS")
);


function GetTrollyDetails(selectedVal) {

    $.ajax({
        async: true,
        cache: false,
        data: { "trollyId": selectedVal, "setId": $("#FPrInspectionProcessMaster_SETID").val() },
        type: "GET",
        url: "/FPrInspectionProcess/GetTrollyDetails",
        success: function (data, status, xhr) {
            /*console.log(data.fN_PROCESS.doff.wv.fabcode);*/
            $("#trollyLength").text(data.lengtH_OUT);
            $("#thisSetLength").text(data.opT1);
            $("#restTrollyLength").text(data.opT2);

            $("#machineName").text(data.fN_MACHINE.name);
            $("#date").text(data.createD_AT.split('T')[0]);
            $("#process").text(data.fiN_PRO_TYPE.name);
            $("#qtyM").text(data.lengtH_OUT);
            $("#qtyYd").text((data.lengtH_OUT * 1.094).toFixed(2));
            $("#shift").text(data.shift);
            $("#shrinkage").text(data.shrinkage - (data.shrinkage * 2));
            $("#operator").text(data.procesS_BYNavigation.firsT_NAME);
            $("#reqWidth").text(data.fN_PROCESS.fabricinfo.widec);
            $("#weight").text(data.fN_PROCESS.fabricinfo.wgdec);

            $("#loomNo").text(data.fN_PROCESS.doff.looM_NONavigation.looM_NO);
            $("#dateWv").text(data.fN_PROCESS.doff.dofF_TIME.split('T')[0]);
            $("#beamNo").text(data.opT3);
            $("#shiftWv").text(data.fN_PROCESS.doff.shift);
            $("#length").text(data.fN_PROCESS.doff.lengtH_BULK);
            if (data.fN_PROCESS.doff.doffeR_NAMENavigation != null) {
                $("#operatorWv").text(data.fN_PROCESS.doff.doffeR_NAMENavigation.firsT_NAME);
            }
            if (data.fN_PROCESS.doff.wv != null) {
                $("#FPrInspectionProcessDetails_OPT4").val(data.fN_PROCESS.doff.wv.fabcode);
            }
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function AddPoints() {
    $.ajax({
        async: true,
        cache: false,
        data: $('#form').serialize(),
        type: "POST",
        url: "/FPrInspectionProcess/AddDetailsList",
        success: function (partialView, status, xhr) {
            $('#defectDetails').html(partialView);
            //resetFields(targets);
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function GetPointInfo() {
    $.post("/FPrInspectionProcess/GetPointInfo",
        $('#form').serialize(),
        function (data, status) {
            $("#FPrInspectionProcessDetails_TOTAL_DEFECT").val(data.fPrInspectionProcessDetails.totaL_DEFECT);
            $("#FPrInspectionProcessDetails_POINT_100SQ").val(data.fPrInspectionProcessDetails.poinT_100SQ);
        });
}

function GetStyleDetails() {
    $.get("/FPrInspectionProcess/GetRollListByStyle",
        { "fabcode": $("#FPrInspectionDefectPoint_StyleName").val() },
        function (data, status) {
            $("#FPrInspectionDefectPoint_RollFId").html('');
            $("#FPrInspectionDefectPoint_RollFId").append('<option value="" selected>Find Roll</option>');
            $.each(data,
                function (id, option) {
                    $("#FPrInspectionDefectPoint_RollFId").append($('<option></option>').val(option.rolL_ID).html(option.rollno));
                });

            console.log(data);
        });
}

function GetDateDetails() {
    $.get("/FPrInspectionProcess/GetRollListByDate",
        { "fabcode": $("#FPrInspectionDefectPoint_FindDate").val() },
        function (data, status) {
            $("#FPrInspectionDefectPoint_RollFCId").html('');
            $("#FPrInspectionDefectPoint_RollFCId").append('<option value="" selected>Find Roll Copy</option>');
            $.each(data,
                function (id, option) {
                    $("#FPrInspectionDefectPoint_RollFCId").append($('<option></option>').val(option.rolL_ID).html(option.rollno));
                });

            console.log(data);
        });
}

function deleteSwal(e) {
    e.preventDefault();
    swal({
        title: 'Please Confirm',
        text: 'Are you sure to delete this roll?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, Confirm!!',
        cancelButtonText: 'Not at all'
    },
        function (isConfirm) {
            if (isConfirm) {
                var roll = $("#FPrInspectionProcessDetails_ROLLNO").val();
                if (roll !== "") {
                    //window.location = '@Url.Action("", "")?=' + roll;
                    $.get("/FPrInspectionProcess/DeleteRoll/?rollNo=" + $("#FPrInspectionProcessDetails_ROLLNO").val(), function (data, status) {
                        if (data === "Success") {
                            toastrNotification("Delete Success!!", "success");
                            window.location = '/FPrInspectionProcess/CreateInspectionProcess';
                        } else {
                            toastrNotification(data, "error");
                        }
                    });

                } else {
                    toastrNotification("Roll No Required!!", "error");
                }

            }
        });
}
$(function () {

    var tubeFabric = $("#FPrInspectionProcessDetails_GR_WEIGHT_KG").val();
    var tubeWeight = $("#FPrInspectionProcessDetails_OPT2").val();
    var grossWeight = $("#FPrInspectionProcessDetails_OPT1").val();
    if (!grossWeight) {
        var polyWeight = 0;
        $("polygram").val(polyWeight.toFixed(2));
        console.log(polyWeight);
    } else {
        var polyWeight = grossWeight - tubeFabric;
        var netWeight = grossWeight - tubeWeight - polyWeight;

        $("#FPrInspectionProcessDetails_NET_WEIGHT_KG").val(netWeight.toFixed(2));
        $("#polygram").val(polyWeight.toFixed(2));
        console.log(polyWeight);
    }
    var polyWeight = grossWeight - tubeFabric;
    var netWeight = grossWeight - tubeWeight - polyWeight;

    $("#FPrInspectionProcessDetails_NET_WEIGHT_KG").val(netWeight.toFixed(2));


    if ($("#FPrInspectionProcessMaster_SETID").val()) {
        GetSetDataEdit($("#FPrInspectionProcessMaster_SETID").val());
    }
    GetStyleDetails();
    GetDateDetails();

    if (window.location.pathname.includes('/FPrInspectionProcess/RollFindCopyProcess')) {
        $('#FPrInspectionProcessDetails_BATCH').get(0).focus();
    }

    $.get("/FPrInspectionProcess/GetRollConfirm/?roll=" + $("#FPrInspectionProcessDetails_ROLLNO").val(), function (data, status) {
        if (!data) {
            resetFields(targets);
            $("#FPrInspectionProcessDetails_FAB_GRADE").val("A").trigger("change");
        }
    });

    $('#form').on('focus', ':input', function () {
        $(this).attr('autocomplete', 'off');
    });

    $('select').select2({
        selectOnClose: true
    });

    $("#FPrInspectionProcessDetails_TOTAL_DEFECT").on('change',
        function () {
            $.post("/FPrInspectionProcess/GetPointInfo",
                $('#form').serialize(),
                function (data, status) {
                    //$("#FPrInspectionProcessDetails_TOTAL_DEFECT").val(data.fPrInspectionProcessDetails.totaL_DEFECT);
                    $("#FPrInspectionProcessDetails_POINT_100SQ").val(data.fPrInspectionProcessDetails.poinT_100SQ);
                });
        });


    $("#FPrInspectionDefectPoint_RollFId").on('change',
        function () {
            if ($("#FPrInspectionDefectPoint_RollFId").val() != null) {
                window.open('/FPrInspectionProcess/RollFindProcess?id=' + $("#FPrInspectionDefectPoint_RollFId").val(), "_self");
            }
        });


    $("#FPrInspectionDefectPoint_RollFCId").on('change',
        function () {
            if ($("#FPrInspectionDefectPoint_RollFCId").val() != null) {
                window.open('/FPrInspectionProcess/RollFindCopyProcess?id=' + $("#FPrInspectionDefectPoint_RollFCId").val(), "_self");
            }
        });
    $("#FPrInspectionDefectPoint_StyleName").on('change',
        function () {
            GetStyleDetails();
        });

    $("#FPrInspectionDefectPoint_FindDate").on('change',
        function () {
            GetDateDetails();
        });

    $("#FPrInspectionDefectPoint_POINT1").on('change',
        function () {
            GetPointInfo();
        });

    $("#FPrInspectionDefectPoint_POINT2").on('change',
        function () {
            GetPointInfo();

        });

    $("#FPrInspectionDefectPoint_POINT3").on('change',
        function () {
            GetPointInfo();

        });

    $("#FPrInspectionDefectPoint_POINT4").on('change',
        function () {
            GetPointInfo();

        });

    $("#FPrInspectionProcessDetails_ROLLNO").on('change',
        function () {

            resetFields(targets);

            $("#FPrInspectionProcessDetails_POINT_100SQ").val("");
            $("#FPrInspectionProcessDetails_TOTAL_DEFECT").val("");


            $("#FPrInspectionProcessDetails_FAB_GRADE").val("A").trigger("change");
        });




    $("#FPrInspectionProcessMaster_INSPDATE").keyup(function (e) {
        if (e.keyCode === 37) {
            $(this).prev('#FPrInspectionProcessMaster_INSPDATE').focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessMaster_SETID').select2('focus');
        }
    });

    $("#FPrInspectionProcessDetails_OPERATOR_ID").on('change', function (e) {
        $('#FPrInspectionProcessDetails_SHIFT').select2('focus');
    });

    $("#FPrInspectionProcessDetails_SHIFT").on('change', function (e) {
        $('#FPrInspectionProcessDetails_MACHINE_ID').select2('focus');
    });


    $("#FPrInspectionProcessDetails_MACHINE_ID").on('select2:select', function (e) {
        $('#purpose1').focus();
    });

    $("#purpose1").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_MACHINE_ID').select2('focus');
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_BATCH').get(0).focus();
        }
    });

    $("#FPrInspectionProcessDetails_BATCH").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_MACHINE_ID').select2('focus');
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_ROLLNO').get(0).focus();
        }
    });

    $("#FPrInspectionProcessDetails_ROLLNO").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_BATCH').get(0).focus();
        }
        else if (e.keyCode === 13) {
            //var pathname = window.location.pathname;
            //if (pathname === '/FPrInspectionProcess/RollFindProcess') {
            //$.ajax({
            //    async: true,
            //    cache: false,
            //    data: $('#form').serialize(),
            //    type: "POST",
            //    url: "/FPrInspectionProcess/IsRollNoInUse",
            //    success: function (data, status, xhr) {
            //        if (data) {
            //            toastrNotification("Duplicate Roll Entry!!!", "error");
            //            $('#FPrInspectionProcessDetails_ROLLNO').focus();
            //            var tmpStr = $('#FPrInspectionProcessDetails_ROLLNO').val();
            //            $('#FPrInspectionProcessDetails_ROLLNO').val('');
            //            $('#FPrInspectionProcessDetails_ROLLNO').val(tmpStr);
            //            return false;
            //        }
            //    },
            //    error: function (e) {
            //        console.log(e);
            //    }
            //});
            //}
            //if (pathname === '/FPrInspectionProcess/RollFindCopyProcess') {
            $.ajax({
                async: true,
                cache: false,
                data: $('#form').serialize(),
                type: "POST",
                url: "/FPrInspectionProcess/IsRollNoInUse",
                success: function (data, status, xhr) {
                    if (data) {
                        toastrNotification("Duplicate Roll Entry!!!", "error");
                        $('#FPrInspectionProcessDetails_ROLLNO').focus();
                        var tmpStr = $('#FPrInspectionProcessDetails_ROLLNO').val();
                        $('#FPrInspectionProcessDetails_ROLLNO').val('');
                        $('#FPrInspectionProcessDetails_ROLLNO').val(tmpStr);
                        return false;
                    }
                },
                error: function (e) {
                    console.log(e);
                }
            });
            //}



            $('#FPrInspectionProcessDetails_LENGTH_1').get(0).focus();
        }
    });

    $("#FPrInspectionProcessDetails_LENGTH_1").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_ROLLNO').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_LENGTH_2').get(0).focus();
            if ($("#FPrInspectionProcessDetails_LENGTH_1").val() === "") {
                $("#FPrInspectionProcessDetails_LENGTH_1").val(0);
            }
        }
    });

    $("#FPrInspectionProcessDetails_LENGTH_2").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_LENGTH_1').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_ACT_WIDTH_INCH').get(0).focus();
            if ($("#FPrInspectionProcessDetails_LENGTH_2").val() === "") {
                $("#FPrInspectionProcessDetails_LENGTH_2").val(0);
            }
        }
    });

    $("#FPrInspectionProcessDetails_ACT_WIDTH_INCH").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_LENGTH_2').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_CUT_WIDTH_INCH').get(0).focus();
        }
    });

    $("#FPrInspectionProcessDetails_CUT_WIDTH_INCH").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_ACT_WIDTH_INCH').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_GR_WEIGHT_KG').get(0).focus();
        }
    });

    $("#FPrInspectionProcessDetails_CUTPCS_YDS").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_GR_WEIGHT_KG').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_DEFECT_NAME').get(0).focus();
        }
    });

    $("#FPrInspectionProcessDetails_DEFECT_NAME").on('select2:select', function (e) {
        $('#purpose4').get(0).focus();
    });

    $("#purpose4").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_DEFECT_NAME').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_DEF_PCS').get(0).focus();
        }
    });
    $("#FPrInspectionProcessDetails_DEF_PCS").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_DEFECT_NAME').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_ACT_CONS').get(0).focus();
        }
    });

    $("#FPrInspectionProcessDetails_FAB_GRADE").on('select2:select', function (e) {
        $('#purpose3').focus();
    });

    $("#purpose3").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_GR_WEIGHT_KG').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_ACT_CONS').get(0).focus();
        }
    });

    //$("#FPrInspectionProcessDetails_FAB_GRADE").keyup(function (e) {
    //    if (e.keyCode === 37) {
    //        $('#FPrInspectionProcessDetails_GR_WEIGHT_KG').get(0).focus();
    //    }
    //    else if (e.keyCode === 13) {
    //        $('#FPrInspectionProcessDetails_ACT_CONS').get(0).focus();
    //    }
    //});
    $("#FPrInspectionProcessDetails_ACT_CONS").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_GR_WEIGHT_KG').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_REMARKS').get(0).focus();
        }
    });
    $("#FPrInspectionProcessDetails_REMARKS").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_ACT_CONS').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionDefectPoint_DEF_TYPEID').get(0).focus();
        }
    });

    $("#FPrInspectionDefectPoint_DEF_TYPEID").on('select2:select', function (e) {
        $('#purpose2').focus();
    });
    $("#purpose2").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionDefectPoint_DEF_TYPEID').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionDefectPoint_POINT1').get(0).focus();
        }
    });
    $("#FPrInspectionDefectPoint_POINT1").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionDefectPoint_DEF_TYPEID').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionDefectPoint_POINT2').get(0).focus();
            if ($("#FPrInspectionDefectPoint_POINT1").val() === "") {
                $("#FPrInspectionDefectPoint_POINT1").val(0);
            }
        }
    });
    $("#FPrInspectionDefectPoint_POINT2").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionDefectPoint_POINT1').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionDefectPoint_POINT3').get(0).focus();
            if ($("#FPrInspectionDefectPoint_POINT2").val() === "") {
                $("#FPrInspectionDefectPoint_POINT2").val(0);
            }
        }
    });
    $("#FPrInspectionDefectPoint_POINT3").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionDefectPoint_POINT2').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionDefectPoint_POINT4').get(0).focus();
            if ($("#FPrInspectionDefectPoint_POINT3").val() === "") {
                $("#FPrInspectionDefectPoint_POINT3").val(0);
            }
        }
    });

    $("#FPrInspectionDefectPoint_POINT4").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionDefectPoint_POINT3').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $.ajax({
                async: true,
                cache: false,
                data: $('#form').serialize(),
                type: "POST",
                url: "/FPrInspectionProcess/AddDetailsList",
                success: function (partialView, status, xhr) {
                    $('#defectDetails').html(partialView);
                    $("#FPrInspectionDefectPoint_POINT1").val("");
                    $("#FPrInspectionDefectPoint_POINT2").val("");
                    $("#FPrInspectionDefectPoint_POINT3").val("");
                    $("#FPrInspectionDefectPoint_POINT4").val("");
                    $("#FPrInspectionDefectPoint_DEF_TYPEID").val("").trigger("change");
                    var pathname = window.location.pathname;

                    if (pathname === '/FPrInspectionProcess/CreateInspectionProcess') {
                        if (xhr.getResponseHeader("Id") !== "") {
                            window.open('/FPrInspectionProcess/EditInspectionProcess?id=' + xhr.getResponseHeader("Id"), "_self");
                        }
                    }
                    GetPointInfo();
                    $("#FPrInspectionDefectPoint_DEF_TYPEID").select2('focus');
                    //resetFields(targets);
                },
                error: function (e) {
                    console.log(e);
                }
            });

            $('#FPrInspectionDefectPoint_DEF_TYPEID').get(0).focus();
            if ($("#FPrInspectionDefectPoint_POINT4").val() === "") {
                $("#FPrInspectionDefectPoint_POINT4").val(0);
            }
        }
    });

    $("#FPrInspectionProcessDetails_GR_WEIGHT_KG").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_CUT_WIDTH_INCH').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_OPT1').get(0).focus();
        }
    });
    $("#FPrInspectionProcessDetails_OPT1").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_GR_WEIGHT_KG').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_OPT2').get(0).focus();
        }
    });
    $("#FPrInspectionProcessDetails_OPT2").keyup(function (e) {
        if (e.keyCode === 37) {
            $('#FPrInspectionProcessDetails_OPT1').get(0).focus();
        }
        else if (e.keyCode === 13) {
            $('#FPrInspectionProcessDetails_ACT_CONS').get(0).focus();
        }
    });


    $("#btnAddNew").on('click',
        function () {
            debugger;
            $('#FPrInspectionProcessDetails_ROLLNO').focus();
            var tmpStr = $('#FPrInspectionProcessDetails_ROLLNO').val();
            $('#FPrInspectionProcessDetails_ROLLNO').val('');
            $('#FPrInspectionProcessDetails_ROLLNO').val(tmpStr);
            $("#FPrInspectionProcessDetails_POINT_100SQ").val("");
            $("#FPrInspectionProcessDetails_TOTAL_DEFECT").val("");
            resetFields(targets);
            //GetPointInfo();

            $.ajax({
                async: true,
                cache: false,
                data: $('#form').serialize(),
                type: "POST",
                url: "/FPrInspectionProcess/ResetDetailsList",
                success: function (partialView, status, xhr) {
                    console.log(partialView);
                    $("#FPrInspectionProcessDetails_POINT_100SQ").val("");
                    $("#FPrInspectionProcessDetails_TOTAL_DEFECT").val("");
                    $('#defectDetails').html(partialView);
                },
                error: function (e) {
                    console.log(e);
                }
            });

            $("#FPrInspectionProcessDetails_FAB_GRADE").val("A").trigger("change");
        });
    $("#btnNew").on('click',
        function () {
            window.open("/FPrInspectionProcess/CreateInspectionProcess", "_self");
        });

    //$("#btnAddRoll").on('click',
    //    function () {
    //        if ($("#FPrInspectionProcessDetails_ROLLNO").val().length < 11) {
    //            toastrNotification("Please enter valid roll no!!", "warning");
    //            return false;
    //        }

    //        if ($("#FPrInspectionProcessDetails_TROLLEYNO").val() === "") {
    //            toastrNotification("Please Select Trolly First!!", "error");
    //            return false;
    //        }

    //        if ($("#FPrInspectionProcessMaster_SETID").val() === "") {
    //            toastrNotification("Please Select Set/Prog. No First!!", "error");
    //            return false;
    //        }


    //        $.ajax({
    //            async: true,
    //            cache: false,
    //            data: $('#form').serialize(),
    //            type: "POST",
    //            url: "/FPrInspectionProcess/AddRollList",
    //            success: function (partialView, status, xhr) {
    //                $('#rollDetails').html(partialView);
    //                $("#FPrInspectionDefectPoint_POINT1").val("");
    //                $("#FPrInspectionDefectPoint_POINT2").val("");
    //                $("#FPrInspectionDefectPoint_POINT3").val("");
    //                $("#FPrInspectionDefectPoint_POINT4").val("");
    //                var pathname = window.location.pathname;

    //                if (pathname === '/FPrInspectionProcess/CreateInspectionProcess') {
    //                    if (xhr.getResponseHeader("Id") !== "") {
    //                        window.open('/FPrInspectionProcess/EditInspectionProcess?id=' + xhr.getResponseHeader("Id"), "_self");
    //                    }
    //                }
    //                $("#FPrInspectionDefectPoint_DEF_TYPEID").select2('focus');
    //                //resetFields(targets);
    //            },
    //            error: function (e) {
    //                console.log(e);
    //            }
    //        });
    //    });



    $("#btnAddRoll").on('click',
        function () {
            if ($("#FPrInspectionProcessDetails_PROCESS_TYPE").val() === "1") {
                if ($("#FPrInspectionProcessDetails_ROLLNO").val().length < 11) {
                    toastrNotification("Please enter valid roll no!!", "warning");
                    return false;
                }

                if ($("#FPrInspectionProcessDetails_TROLLEYNO").val() === "") {
                    toastrNotification("Please Select Trolly First!!", "error");
                    return false;
                }

                if ($("#FPrInspectionProcessMaster_SETID").val() === "") {
                    toastrNotification("Please Select Set/Prog. No First!!", "error");
                    return false;
                }
            }


            $.ajax({
                async: true,
                cache: false,
                data: $('#form').serialize(),
                type: "POST",
                url: "/FPrInspectionProcess/IsRollNoInUse",
                success: function (data, status, xhr) {
                    if (data) {
                        swal({
                            title: 'Please Confirm',
                            text: 'Roll already exists. Are you sure to update?',
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonText: 'Yes, Confirm!!',
                            cancelButtonText: 'Not at all'
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    $.ajax({
                                        async: true,
                                        cache: false,
                                        data: $('#form').serialize(),
                                        type: "POST",
                                        url: "/FPrInspectionProcess/AddDetailsList",
                                        success: function (partialView, status, xhr) {
                                            $('#defectDetails').html(partialView);
                                            GetPointInfo();
                                            $("#FPrInspectionDefectPoint_POINT1").val("");
                                            $("#FPrInspectionDefectPoint_POINT2").val("");
                                            $("#FPrInspectionDefectPoint_POINT3").val("");
                                            $("#FPrInspectionDefectPoint_POINT4").val("");
                                            $("#FPrInspectionDefectPoint_DEF_TYPEID").val("").trigger("change");
                                            var pathname = window.location.pathname;

                                            if (pathname === '/FPrInspectionProcess/CreateInspectionProcess') {
                                                if (xhr.getResponseHeader("Id") !== "") {
                                                    window.open('/FPrInspectionProcess/EditInspectionProcess?id=' + xhr.getResponseHeader("Id"), "_self");
                                                }
                                            }
                                            $("#FPrInspectionDefectPoint_DEF_TYPEID").select2('focus');
                                            //resetFields(targets);
                                            toastrNotification("Save Success", "success");
                                        },
                                        error: function (e) {
                                            console.log(e);
                                        }
                                    });
                                }
                            });
                    } else {






                        $.ajax({
                            async: true,
                            cache: false,
                            data: $('#form').serialize(),
                            type: "POST",
                            url: "/FPrInspectionProcess/AddDetailsList",
                            success: function (partialView, status, xhr) {
                                $('#defectDetails').html(partialView);
                                GetPointInfo();
                                $("#FPrInspectionDefectPoint_POINT1").val("");
                                $("#FPrInspectionDefectPoint_POINT2").val("");
                                $("#FPrInspectionDefectPoint_POINT3").val("");
                                $("#FPrInspectionDefectPoint_POINT4").val("");
                                $("#FPrInspectionDefectPoint_DEF_TYPEID").val("").trigger("change");
                                var pathname = window.location.pathname;

                                if (pathname === '/FPrInspectionProcess/CreateInspectionProcess') {
                                    if (xhr.getResponseHeader("Id") !== "") {
                                        window.open('/FPrInspectionProcess/EditInspectionProcess?id=' + xhr.getResponseHeader("Id"), "_self");
                                    }
                                }
                                $("#FPrInspectionDefectPoint_DEF_TYPEID").select2('focus');
                                //resetFields(targets);
                                toastrNotification("Save Success", "success");
                            },
                            error: function (e) {
                                console.log(e);
                            }
                        });
                    }
                },
                error: function (e) {
                    console.log(e);
                }
            });

        });



    $("#btnPrintRollA").on('click',
        function () {

            if ($("#FPrInspectionProcessDetails_PROCESS_TYPE").val() === "1") {
                if ($("#FPrInspectionProcessDetails_ROLLNO").val().length < 11) {
                    toastrNotification("Please enter valid roll no!!", "warning");
                    return false;
                }

                if ($("#FPrInspectionProcessDetails_TROLLEYNO").val() === "") {
                    toastrNotification("Please Select Trolly First!!", "error");
                    return false;
                }

                if ($("#FPrInspectionProcessMaster_SETID").val() === "") {
                    toastrNotification("Please Select Set/Prog. No First!!", "error");
                    return false;
                }

                if ($("#FPrInspectionProcessDetails_ROLLNO").val() === "") {
                    toastrNotification("Please Enter Roll No First!!", "error");
                    return false;
                }
            }
            var rollNO = $("#FPrInspectionProcessDetails_ROLLNO").val();

            //resetFields(targets);
            $("#FPrInspectionProcessDetails_FAB_GRADE").val("A").trigger("change");
            var new_window = window.open(`/FPrInspectionProcess/RRollStickerReport?rollId=${rollNO}`, '_blank');

        });




    $("#btnPrintRollLO").on('click',
        function () {

            if ($("#FPrInspectionProcessDetails_PROCESS_TYPE").val() === "1") {
                if ($("#FPrInspectionProcessDetails_ROLLNO").val().length < 11) {
                    toastrNotification("Please enter valid roll no!!", "warning");
                    return false;
                }

                if ($("#FPrInspectionProcessDetails_TROLLEYNO").val() === "") {
                    toastrNotification("Please Select Trolly First!!", "error");
                    return false;
                }

                if ($("#FPrInspectionProcessMaster_SETID").val() === "") {
                    toastrNotification("Please Select Set/Prog. No First!!", "error");
                    return false;
                }

                if ($("#FPrInspectionProcessDetails_ROLLNO").val() === "") {
                    toastrNotification("Please Enter Roll No First!!", "error");
                    return false;
                }
            }
            var rollNO = $("#FPrInspectionProcessDetails_ROLLNO").val();

            //resetFields(targets);
            $("#FPrInspectionProcessDetails_FAB_GRADE").val("A").trigger("change");
            var new_window = window.open(`/FPrInspectionProcess/RRollStickerLoReport?rollId=${rollNO}`, '_blank');

        });

    $("#FPrInspectionProcessDetails_DEFECT_NAME").on('change',
        function () {
            var selectedVal = $(this).val();

            var clearance = ['2000076'];
            var Dyeing = ['2000089', '2000090', '2000091', '2000092', '2000008', '2000009', '2000010', '2000011', '2000012', '2000013', '2000138'];
            var Finishing = ['2000028', '2000033', '2000034', '2000035', '2000036', '2000037', '2000038', '2000082', '2000083', '2000084', '2000085', '2000086', '2000087', '2000088', '2000069', '2000047', '2000048', '2000049', '2000050', '2000051', '2000052', '2000053', '2000054', '2000055', '2000056', '2000057', '2000058', '2000059', '2000060', '2000061', '2000062', '2000063', '2000106', '2000107', '2000117', '2000118', '2000119', '2000126', '2000124', '2000130', '2000134'];
            var lcb = ['2000073', '2000096'];
            var others = ['2000109'];
            var rnd = ['2000114'];
            var sizing = ['2000115', '2000072', '2000093', '2000094', '2000095', '2000015', '2000016', '2000017', '2000018'];
            var stopMark = ['2000110'];
            var warping = ['2000120'];
            var weaving = ['2000075', '2000123', '2000125', '2000127', '2000128', '2000129', '2000108', '2000102', '2000103', '2000104', '2000105', '2000135', '2000136', '2000137', '2000132', '2000019', '2000020', '2000021', '2000022', '2000023', '2000024', '2000025', '2000039', '2000029', '2000030', '2000031', '2000032', '2000014', '2000027', '2000070', '2000071', '2000077', '2000078', '2000079', '2000080', '2000081', '2000064', '2000065', '2000066', '2000067', '2000068'];
            var yarn = ['2000040', '2000041', '2000042', '2000043', '2000044', '2000045', '2000046', '2000026', '2000000', '2000001', '2000002', '2000003', '2000004', '2000005', '2000006', '2000007', '2000133', '2000097', '2000098', '2000099', '2000100', '2000101', '2000074', '2000111', '2000112', '2000113', '2000131', '2000116', '2000121', '2000122'];


            if (jQuery.inArray(selectedVal, clearance) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(166).trigger("change");
            } else if (jQuery.inArray(selectedVal, Dyeing) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(158).trigger("change");

            } else if (jQuery.inArray(selectedVal, Finishing) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(152).trigger("change");

            } else if (jQuery.inArray(selectedVal, lcb) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(159).trigger("change");

            } else if (jQuery.inArray(selectedVal, others) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(189).trigger("change");

            } else if (jQuery.inArray(selectedVal, rnd) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(171).trigger("change");

            } else if (jQuery.inArray(selectedVal, sizing) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(160).trigger("change");

            } else if (jQuery.inArray(selectedVal, stopMark) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(158).trigger("change");

            } else if (jQuery.inArray(selectedVal, warping) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(161).trigger("change");

            } else if (jQuery.inArray(selectedVal, weaving) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(165).trigger("change");

            } else if (jQuery.inArray(selectedVal, yarn) !== -1) {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val(179).trigger("change");

            } else {
                $("#FPrInspectionProcessDetails_CUT_PCS_SECTION").val("").trigger("change");
            }

        });


    $("#FPrInspectionProcessDetails_LENGTH_1").on('change',
        function () {
            var totalYds = parseFloat($(this).val()) + parseFloat($("#FPrInspectionProcessDetails_LENGTH_2").val() !== "" ? $("#FPrInspectionProcessDetails_LENGTH_2").val() : 0);
            var totalMtr = totalYds * 0.9144;
            $("#FPrInspectionProcessDetails_LENGTH_YDS").val(totalYds);
            $("#FPrInspectionProcessDetails_LENGTH_MTR").val(totalMtr.toFixed(2));
            GetNoOfPcs();

        });

    $("#FPrInspectionProcessDetails_LENGTH_2").on('change',
        function () {
            var totalYds = parseFloat($(this).val()) + parseFloat($("#FPrInspectionProcessDetails_LENGTH_1").val() !== "" ? $("#FPrInspectionProcessDetails_LENGTH_1").val() : 0);
            var totalMtr = totalYds * 0.9144;
            $("#FPrInspectionProcessDetails_LENGTH_YDS").val(totalYds);
            $("#FPrInspectionProcessDetails_LENGTH_MTR").val(totalMtr.toFixed(2));
            GetNoOfPcs();
        });

    $("#rollReload").on('click',
        function () {
            $.ajax({
                async: true,
                cache: false,
                data: $('#form').serialize(),
                type: "POST",
                url: "/FPrInspectionProcess/GetRollNoBySetId",
                success: function (data, status, xhr) {
                    console.log(data);
                    $("#FPrInspectionProcessDetails_ROLLNO").val(data);
                },
                error: function (e) {
                    console.log(e);
                }
            });
        });

    $("#FPrInspectionProcessMaster_SETID").on('change',
        function () {
            if ($("#FPrInspectionProcessMaster_SETID").val()) {
                GetSetDataEdit($("#FPrInspectionProcessMaster_SETID").val());
                $('#FPrInspectionProcessDetails_TROLLEYNO').select2('focus');
            }
        });


    $("#FPrInspectionProcessDetails_TROLLEYNO").on('change',
        function () {
            var selectedVal = $("#FPrInspectionProcessDetails_TROLLEYNO").val();

            if (selectedVal === "") {
                $("#trollyLength").text("");
                $("#thisSetLength").text("");
                $("#restTrollyLength").text("");
                return false;
            }
            GetTrollyDetails(selectedVal);
            $('#FPrInspectionProcessDetails_OPERATOR_ID').select2('focus');
        });
    $("#FPrInspectionProcessDetails_ACT_WIDTH_INCH").on('change',
        function () {
            var insertedVal = $("#FPrInspectionProcessDetails_ACT_WIDTH_INCH").val();

            if ($("#FPrInspectionProcessDetails_TOTAL_DEFECT").val() !== 0) {
                GetPointInfo();
            }

            if (insertedVal === "") {
                $("#FPrInspectionProcessDetails_WEIGHT_DEDUCT").val("");
                return false;
            }
            var weightDeduct = 0;
            if (insertedVal >= 40.1 && insertedVal <= 57) {
                weightDeduct = 0.650;
            } else if (insertedVal > 57 && insertedVal <= 61) {
                weightDeduct = 0.700;
            } else if (insertedVal > 61 && insertedVal <= 65) {
                weightDeduct = 0.720;
            } else if (insertedVal > 65 && insertedVal <= 67) {
                weightDeduct = 0.750;
            } else if (insertedVal > 67 && insertedVal <= 71) {
                weightDeduct = 0.800;
            } else if (insertedVal > 71 && insertedVal <= 100) {
                weightDeduct = 0.850;
            } else {
                weightDeduct = 0;
            }
            weightDeduct += 0.100;
            $("#FPrInspectionProcessDetails_WEIGHT_DEDUCT").val(weightDeduct);


            var totalLength = $("#FPrInspectionProcessDetails_LENGTH_YDS").val();
            var actWidth = $("#FPrInspectionProcessDetails_ACT_WIDTH_INCH").val();

            var weight = $("#weight").text();
            var gross = ((weight * totalLength * actWidth) / 1267.2);
            //$("#FPrInspectionProcessDetails_GR_WEIGHT_KG").val(gross.toFixed(2));

            var grossWeight = $("#FPrInspectionProcessDetails_GR_WEIGHT_KG").val();
            GetNetWeight(grossWeight === "" ? 0 : grossWeight, totalLength === "" ? 0 : totalLength, actWidth === "" ? 0 : actWidth, weightDeduct);

        });
    $("#FPrInspectionProcessDetails_OPT2, #FPrInspectionProcessDetails_GR_WEIGHT_KG, #FPrInspectionProcessDetails_OPT1").change(function () {

        var tubeFabric = $("#FPrInspectionProcessDetails_GR_WEIGHT_KG").val();
        var tubeWeight = $("#FPrInspectionProcessDetails_OPT2").val();
        var grossWeight = $("#FPrInspectionProcessDetails_OPT1").val();
        if (!grossWeight) {
            var polyWeight = 0;
            $("polygram").val(polyWeight.toFixed(2));
            console.log(polyWeight);
        } else {
            var polyWeight = grossWeight - tubeFabric;
            var netWeight = grossWeight - tubeWeight - polyWeight;

            $("#FPrInspectionProcessDetails_NET_WEIGHT_KG").val(netWeight.toFixed(2));
            $("#polygram").val(polyWeight.toFixed(2));
            console.log(polyWeight);
        }
        var polyWeight = grossWeight - tubeFabric;
        var netWeight = grossWeight - tubeWeight - polyWeight;

        $("#FPrInspectionProcessDetails_NET_WEIGHT_KG").val(netWeight.toFixed(2));
        
        
    });

    //$("#FPrInspectionProcessDetails_GR_WEIGHT_KG").on('change',
    //    function () {

    //        var weightDeduct = $("#FPrInspectionProcessDetails_WEIGHT_DEDUCT").val();
    //        var grossWeight = $("#FPrInspectionProcessDetails_GR_WEIGHT_KG").val();
    //        var totalLength = $("#FPrInspectionProcessDetails_LENGTH_YDS").val();
    //        var actWidth = $("#FPrInspectionProcessDetails_ACT_WIDTH_INCH").val();
    //        GetNetWeight(grossWeight === "" ? 0 : grossWeight, totalLength === "" ? 0 : totalLength, actWidth === "" ? 0 : actWidth, weightDeduct);

    //    });

});



//function GetNetWeight(grossWeight, totalLength, actWidth, weightDeduct) {
//    //var netWeight = (grossWeight * totalLength * actWidth) / 1267.2;
//    //netWeight = netWeight - weightDeduct;

//    var weight = $("#weight").text();
//    var netWeight = ((actWidth / 36) * weight * 0.02834 * totalLength);
//    netWeight -= weightDeduct;
//    $("#FPrInspectionProcessDetails_NET_WEIGHT_KG").val(netWeight.toFixed(2));
//}

function GetNoOfPcs() {
    $("#FPrInspectionProcessDetails_PICES").val("");
    var noOfPcs = 0;
    var length1 = $("#FPrInspectionProcessDetails_LENGTH_1").val();
    var length2 = $("#FPrInspectionProcessDetails_LENGTH_2").val();

    if (length1 !== '0' && length1 !== "") {
        noOfPcs += 1;
    }
    if (length2 !== '0' && length2 !== "") {
        noOfPcs += 1;
    }
    $("#FPrInspectionProcessDetails_PICES").val(noOfPcs);
}

//function GetSetData(selectedVal) {
//    //$.ajax({
//    //    async: true,
//    //    cache: false,
//    //    data: { "setId": selectedVal },
//    //    type: "GET",
//    //    url: "/FPrInspectionProcess/GetSetDetails",
//    //    success: function (data, status, xhr) {
//    //        console.log(data);
//    //        $("#style").text(data.comExPiDetails.style.fabcodeNavigation.stylE_NAME);
//    //        $("#FPrInspectionProcessMaster_FABCODE").val(data.comExPiDetails.style.fabcodeNavigation.fabcode);
//    //        $("#rndConst").text(data.comExPiDetails.style.fabcodeNavigation.fnepi + 'X' + data.comExPiDetails.style.fabcodeNavigation.fnppi);
//    //        $("#const").text(data.plProductionSetDistribution.opT1);
//    //        $("#salesPerson").text(data.comExPiDetails.pimaster.flwby);
//    //        $("#buyerBrand").text(data.comExPiDetails.pimaster.brand.brandname);
//    //        $("#buyer").text(data.comExPiDetails.pimaster.buyer.buyeR_NAME);
//    //        $("#color").text(data.comExPiDetails.style.fabcodeNavigation.colorcodeNavigation.color);
//    //        $("#loomType").text(data.comExPiDetails.style.fabcodeNavigation.loom.looM_TYPE_NAME);
//    //        $("#orderNo").text(data.comExPiDetails.sO_NO);
//    //        $("#ratio").text(data.plProductionSetDistribution.subgroup.ratio);
//    //        $("#totalEnds").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.totaL_ENOS);
//    //        $("#setLength").text(data.plProductionSetDistribution.proG_.seT_QTY);
//    //        $("#poNo").text(data.comExPiDetails.pimaster.pino);
//    //        $("#weight").text(data.comExPiDetails.style.fabcodeNavigation.wgdec);
//    //        var pathname = window.location.pathname;

//    //        if (pathname === '/FPrInspectionProcess/CreateInspectionProcess') {
//    //            $("#FPrInspectionProcessDetails_ROLLNO").val(data.plProductionSetDistribution.opT3);
//    //        }
//    //    },
//    //    error: function (e) {
//    //        console.log(e);
//    //    }
//    //});
//    //var trollyNo = $("#FPrInspectionProcessDetails_TROLLEYNO").val();
//    $.ajax({
//        async: true,
//        cache: false,
//        data: { "setId": selectedVal },
//        type: "GET",
//        url: "/FPrInspectionProcess/GetTrollyListBySetId",
//        success: function (data, status, xhr) {
//            $("#FPrInspectionProcessDetails_TROLLEYNO").html('');
//            $("#FPrInspectionProcessDetails_TROLLEYNO").append('<option value="" selected>Select Trolly No.</option>');
//            $.each(data,
//                function (id, option) {
//                    $("#FPrInspectionProcessDetails_TROLLEYNO").append($('<option></option>').val(option.fiN_PROCESSID).html(option.trollnoNavigation.name));
//                });

//            var pathname = window.location.pathname;
//            if (pathname.includes('/FPrInspectionProcess/EditInspectionProcess')) {
//                $("#FPrInspectionProcessDetails_TROLLEYNO").val(trollyNo).trigger("change");
//            }
//        },
//        error: function (e) {
//            console.log(e);
//        }
//    });
//}

function GetSetDataEdit(selectedVal) {
    $.ajax({
        async: true,
        cache: false,
        data: { "setId": selectedVal },
        type: "GET",
        url: "/FPrInspectionProcess/GetSetDetails",
        success: function (data, status, xhr) {
            console.log(data);
            $("#style").text(data.comExPiDetails.style.fabcodeNavigation.stylE_NAME);
            $("#FPrInspectionProcessMaster_FABCODE").val(data.comExPiDetails.style.fabcodeNavigation.fabcode);
            $("#rndConst").text(data.comExPiDetails.style.fabcodeNavigation.fnepi + 'X' + data.comExPiDetails.style.fabcodeNavigation.fnppi);
            $("#const").text(data.plProductionSetDistribution.opT1);
            $("#salesPerson").text(data.comExPiDetails.pimaster.flwby);
            $("#buyerBrand").text(data.comExPiDetails.pimaster.brand.brandname);
            $("#buyer").text(data.comExPiDetails.pimaster.buyer.buyeR_NAME);
            $("#color").text(data.comExPiDetails.style.fabcodeNavigation.colorcodeNavigation.color);
            $("#loomType").text(data.comExPiDetails.style.fabcodeNavigation.loom.looM_TYPE_NAME);
            $("#orderNo").text(data.comExPiDetails.sO_NO);
            $("#ratio").text(data.plProductionSetDistribution.subgroup.ratio);
            $("#totalEnds").text(data.plProductionSetDistribution.proG_.blK_PROG_.rndProductionOrder.totaL_ENOS);
            $("#setLength").text(data.plProductionSetDistribution.proG_.seT_QTY);
            $("#poNo").text(data.comExPiDetails.pimaster.pino);
            $("#weight").text(data.comExPiDetails.style.fabcodeNavigation.wgdec);
            var pathname = window.location.pathname;

            if (pathname === '/FPrInspectionProcess/CreateInspectionProcess') {
                $("#FPrInspectionProcessDetails_ROLLNO").val(data.plProductionSetDistribution.opT3);
            }
        },
        error: function (e) {
            console.log(e);
        }
    });
    var trollyNo = $("#FPrInspectionProcessDetails_TROLLEYNO").val();
    $.ajax({
        async: true,
        cache: false,
        data: { "setId": selectedVal },
        type: "GET",
        url: "/FPrInspectionProcess/GetTrollyListBySetIdEdit",
        success: function (data, status, xhr) {
           $("#FPrInspectionProcessDetails_TROLLEYNO").find("option:not(:first)").remove();
            $("#FPrInspectionProcessDetails_TROLLEYNO").html('');
            $("#FPrInspectionProcessDetails_TROLLEYNO").append('<option value="" selected>Select Trolly No.</option>');
            $.each(data,
                function (id, option) {
                    $("#FPrInspectionProcessDetails_TROLLEYNO").append($('<option></option>').val(option.fiN_PROCESSID).html(option.trollnoNavigation.name));
                });

            var pathname = window.location.pathname;
            if (pathname.includes('/FPrInspectionProcess/EditInspectionProcess') || pathname.includes('/FPrInspectionProcess/RollFindProcess') || pathname.includes('/FPrInspectionProcess/RollFindCopyProcess') ) {
                $("#FPrInspectionProcessDetails_TROLLEYNO").val(trollyNo).trigger("change");
            }
        },
        error: function (e) {
            console.log(e);
        }
    });

   
}