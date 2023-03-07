var styleId = $("#RndBom_FABCODE");
var finishType = $("#finishType");
var color = $("#color");
var setNo = $("#setNo");
var construction = $("#construction");
var totalEnds = $("#totalEnds");
var finishWeight = $("#finishWeight");
var width = $("#width");
var reqQty = $("#RndBomMaterialsDetails_REQ_QTY");
var dosing = $("#RndBomMaterialsDetails_DOSING");
var conc = $("#RndBomMaterialsDetails_CONC");
var speed = $("#RndBomMaterialsDetails_SPEED");
var noOfSets = $("#RndBomMaterialsDetails_NO_OF_SETS");
var add10 = $("#RndBomMaterialsDetails_ADD_10_FOR_BOX");



var errors = {
    0: {
        title: "Invalid Submission.",
        message: "We can not process your request. Please try again later."
    }
}

//Dosing * Conc./ Speed / No of Set
//"rqty" + "rqty" * 10 %

function getReqQty() {
    var dosingVal = dosing.val();
    var concVal = conc.val();
    var speedVal = speed.val();
    var noOfSetsVal = noOfSets.val();

    if (speedVal !== "" && noOfSetsVal !== "" && concVal !== "" && dosingVal !== "") {
        var reqQtyVal = ((dosingVal * concVal) / speedVal) / noOfSetsVal;
        var add10Val = reqQtyVal + ((reqQtyVal*10)/100);
        reqQty.val(reqQtyVal);
        add10.val(add10Val);
    }
}


$(function () {
    getByStyle();

    dosing.on("change", function () {
        getReqQty();
    });

    conc.on("change", function () {
        getReqQty();
    });

    speed.on("change", function () {
        getReqQty();
    });

    noOfSets.on("change", function () {
        getReqQty();
    });


    styleId.on("change", function () {
        getByStyle();
    });
    
    $("#addToList").on('click',
        function () {
            $.ajax({
                async: true,
                cache: false,
                data: $('#form').serialize(),
                type: "POST",
                url: "/RndBOM/AddMaterialList",
                success: function (partialView, status, xhr) {
                    $('#materialPartialView').html(partialView);
                },
                error: function (e) {
                    console.log(e);
                }
            });
        });

});

function getByStyle() {
    var formData = {
        "styleId": styleId.val()
    }
    
    if (styleId.val()) {
        $.get("/RndBOM/GetAllByStyleId", formData, function (data) {
            console.log(data);

            finishType.text(data.rnD_FINISHTYPE.typename);
            $("#RndBom_FINISH_TYPE").val(data.rnD_FINISHTYPE.finid);
            color.text(data.colorcodeNavigation.color);
            $("#RndBom_COLOR").val(data.colorcodeNavigation.colorcode);
            setNo.text(data.progno);
            $("#RndBom_PROG_NO").val(data.progno);
            construction.text(data.opT1);
            $("#RndBom_CONSTRUCTION").val(data.opT1);
            totalEnds.text(data.totalends);
            $("#RndBom_TOTAL_ENDS").val(data.totalends);
            finishWeight.text(data.wgfnbw);
            $("#RndBom_FINISH_WEIGHT").val(data.wgfnbw);
            $("#lotRatio").text(data.opT2);
            $("#RndBom_LOT_RATIO").val(data.opT2);
            width.text(data.widec);
            $("#RndBom_WIDTH").val(data.widec);
            //totalEnds.text(data.totalends);
            //totalEnds.text(data.totalends);

        }).fail(function () {
            toastr.error(errors[0].message, errors[0].title);
        });
    }
}
