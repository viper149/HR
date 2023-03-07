var btnAdd = $("#btnAdd");
var attachTo = $("#orderItemsContainer");
var wvId = $("#rND_FABRICINFO_WVID");
var sFinId = $("#rND_FABRICINFO_SFINID");
var epiPpiGrEpi = $("#rND_FABRICINFO_GREPI");
var epiPpigrPpi = $("#rND_FABRICINFO_GRPPI");
var weaveName = $("#weaveName");
var totalEnds = $("#rND_FABRICINFO_TOTALENDS");
var totalEndsSample = $("#rND_FABRICINFO_TOTAL_ENDS_SAMPLE");
var endsPerDent = $("#rND_FABRICINFO_ENDS");
var reedSpace = $("#rND_FABRICINFO_REED_SPACE");
var reedCount = $("#rND_FABRICINFO_REED_COUNT");
var reedDent = $("#rND_FABRICINFO_DENT");
var dyeingCode = $("#rND_FABRICINFO_DYCODE");
var fabCode = $("#rND_FABRICINFO_FABCODE");
var dyeingName = $("#dyeingName");
var colorName = $("#colorName");
var colorFnName = $("#colorFnName");
var concept = $("#rND_FABRICINFO_CONCEPT");
var totalRope = $("#rND_FABRICINFO_TOTALROPE");
var previousList = $("#secret");
var fnEpi = $("#rND_FABRICINFO_FNEPI");
var fnPpi = $("#rND_FABRICINFO_FNPPI");
var afterWashEpi = $("#rND_FABRICINFO_AWEPI");
var afterWashPpi = $("#rND_FABRICINFO_AWPPI");
var beforeWashEpi = $("#rND_FABRICINFO_BWEPI");
var beforeWashPpi = $("#rND_FABRICINFO_BWPPI");
var weightBeforeWash = $("#rND_FABRICINFO_WGGRBW");
var weightAfterWash = $("#rND_FABRICINFO_WGGRAW");
var widthBeforeWash = $("#rND_FABRICINFO_WIGRBW");
var widthAfterWash = $("#rND_FABRICINFO_WIGRAW");
var weightFinishBeforeWash = $("#rND_FABRICINFO_WGFNBW");
var weightFinishAfterWash = $("#rND_FABRICINFO_WGFNAW");
var widthFinishBeforeWash = $("#rND_FABRICINFO_WIFNBW");
var widthFinishAfterWash = $("#rND_FABRICINFO_WIFNAW");
var widthFinishCutable = $("#rND_FABRICINFO_WIFNCUT");
var shrinkageGreigeWarp = $("#rND_FABRICINFO_SRGRWARP");
var shrinkageGreigeWeft = $("#rND_FABRICINFO_SRGRWEFT");
var shrinkageFinishWarp = $("#rND_FABRICINFO_SRFNWARP");
var shrinkageFinishWeft = $("#rND_FABRICINFO_SRFNWEFT");

var stretchGreigeWarp = $("#rND_FABRICINFO_STGRWARP");
var stretchGreigeWeft = $("#rND_FABRICINFO_STGRWEFT");

var stretchFinishWarp = $("#rND_FABRICINFO_STFNWARP");
var stretchFinishWeft = $("#rND_FABRICINFO_STFNWEFT");
var growthFinishWarp = $("#rND_FABRICINFO_GRFNWARP");
var growthFinishWeft = $("#rND_FABRICINFO_GRFNWEFT");
var skewMove = $("#rND_FABRICINFO_SKEW_FN");
var slippageWarp = $("#rND_FABRICINFO_SLPWARPFN");
var slippageWeft = $("#rND_FABRICINFO_SLPWEFTFN");
var tensileWarp = $("#rND_FABRICINFO_TNWARPFN");
var tensileWeft = $("#rND_FABRICINFO_TNWEFTFN");
var tearWarp = $("#rND_FABRICINFO_TRWARPFN");
var tearWeft = $("#rND_FABRICINFO_TRWEFTFN");
var colorFatDry = $("#rND_FABRICINFO_CFATDRY");
var colorFatWet = $("#rND_FABRICINFO_CFATWET");
var ph = $("#rND_FABRICINFO_PHFN");
var composition = $("#rND_FABRICINFO_COMPOSITION");
var labTestNo = $("#rND_FABRICINFO_LSBTESTNO");
var sprA = $("#rND_FABRICINFO_SPR_A_FIN");
var sprB = $("#rND_FABRICINFO_SPR_B_FIN");
var rubDryFn = $("#rND_FABRICINFO_RUBDRYFN");
var rubWetFn = $("#rND_FABRICINFO_RUBWETFN");



var afterWashEpiPro = $("#rND_FABRICINFO_PROAWEPI");
var afterWashPpiPro = $("#rND_FABRICINFO_PROAWPPI");
var beforeWashEpiPro = $("#rND_FABRICINFO_PROBWEPI");
var beforeWashPpiPro = $("#rND_FABRICINFO_PROBWPPI");
var weightBeforeWashPro = $("#rND_FABRICINFO_WGBWPRO");
var weightAfterWashPro = $("#rND_FABRICINFO_WGAWPRO");
var widthBeforeWashPro = $("#rND_FABRICINFO_WIBWPRO");
var widthAfterWashPro = $("#rND_FABRICINFO_WIAWPRO");
var widthCutablePro = $("#rND_FABRICINFO_WICUTPRO");
var shrinkageWarpPro = $("#rND_FABRICINFO_SRWARPPRO");
var shrinkageWeftPro = $("#rND_FABRICINFO_SRWEFTPRO");

var stretchWarpPro = $("#rND_FABRICINFO_STWARPPRO");
var stretchWeftPro = $("#rND_FABRICINFO_STWARPPRO");
var growthWarpPro = $("#rND_FABRICINFO_GRWARPPRO");
var growthWeftPro = $("#rND_FABRICINFO_GRWEFTPRO");
var skewMovePro = $("#rND_FABRICINFO_SKEW_PRO");
var slippageWarpPro = $("#rND_FABRICINFO_SLIWARPPRO");
var slippageWeftPro = $("#rND_FABRICINFO_SLIWEFTPRO");
var tensileWarpPro = $("#rND_FABRICINFO_TNWARPPRO");
var tensileWeftPro = $("#rND_FABRICINFO_TNWEFTPRO");
var tearWarpPro = $("#rND_FABRICINFO_TEARWARPPRO");
var tearWeftPro = $("#rND_FABRICINFO_TEARWEFTPRO");
var colorFatDryPro = $("#rND_FABRICINFO_CFATDRY");
var colorFatWetPro = $("#rND_FABRICINFO_CFATWET");
var phPro = $("#rND_FABRICINFO_PHPRO");
var compositionPro = $("#rND_FABRICINFO_COMPOSITION_PRO");
//var labTestNoPro = $("#rND_FABRICINFO_LSBTESTNO");
var sprAPro = $("#rND_FABRICINFO_SPR_A_PRO");
var sprBPro = $("#rND_FABRICINFO_SPR_B_PRO");
var rubDryPro = $("#rND_FABRICINFO_RUBDRYPRO");
var rubWetPro = $("#rND_FABRICINFO_RUBWETPRO");



var hiddenWeaveId = $("#hiddenWeaveId");
var hiddenDId = $("#hiddenDId");
var hiddenLoomId = $("#rND_FABRICINFO_LOOMID");
var hiddenColorCode = $("#hiddenColorCode");
var progNo = $("#rND_FABRICINFO_PROGNO");
var loomType = $("#rND_FABRICINFO_LOOMID");
var pickLength = $("#rND_FABRICINFO_PICKLENGHT");
var finishRoute = $("#rND_FABRICINFO_FINISH_ROUTE");
var ltsid = $("#rND_FABRICINFO_LTSID");
var ltsidPro = $("#rND_FABRICINFO_PROTOCOL_NO");
var totalWeft = $("#rND_FABRICINFO_TOTALWEFT");
var shadeColor = $("#rND_FABRICINFO_COLORCODE");
var lsgTestNo = $("#rND_FABRICINFO_LSGTESTNO");

var targets = [];
var removedItems = [];

targets.push(
    totalEnds, totalEndsSample, epiPpiGrEpi, epiPpigrPpi, weaveName, hiddenWeaveId, reedSpace, endsPerDent,
    reedCount, reedDent, dyeingCode, fabCode, dyeingName, hiddenDId,
    loomType, colorName, colorFnName, hiddenLoomId, hiddenColorCode, totalRope,
    fnEpi, fnPpi, afterWashEpi, afterWashPpi, beforeWashEpi, beforeWashPpi, weightBeforeWash, weightAfterWash,
    widthBeforeWash, widthAfterWash, weightFinishBeforeWash, weightFinishAfterWash,
    widthFinishBeforeWash, widthFinishAfterWash, widthFinishCutable, shrinkageGreigeWarp,
    shrinkageGreigeWeft, shrinkageFinishWarp, shrinkageFinishWeft, stretchGreigeWarp, stretchGreigeWeft,
    stretchFinishWarp, stretchFinishWeft, growthFinishWarp, growthFinishWeft, skewMove,
    slippageWarp, slippageWeft, tensileWarp, tensileWeft, tearWarp, tearWeft,
    colorFatDry, colorFatWet, ph, composition, labTestNo, progNo, pickLength, finishRoute, ltsid, totalWeft, sprA, sprB, rubDryFn, rubWetFn, lsgTestNo
);

var targetsPro = [];

targetsPro.push(
    totalEnds, totalEndsSample, epiPpiGrEpi, epiPpigrPpi, weaveName, hiddenWeaveId, reedSpace, endsPerDent,
    reedCount, reedDent, dyeingCode, fabCode, dyeingName, hiddenDId,
    loomType, colorName, colorFnName, hiddenLoomId, hiddenColorCode, totalRope,
    fnEpi, fnPpi, afterWashEpiPro, afterWashPpiPro, beforeWashEpiPro, beforeWashPpiPro,
    weightBeforeWashPro, weightAfterWashPro,
    widthBeforeWashPro, widthAfterWashPro, widthCutablePro, shrinkageWarpPro, shrinkageWeftPro, stretchWarpPro, stretchWeftPro, growthWarpPro, growthWeftPro, skewMovePro, slippageWarpPro, slippageWeftPro, tensileWarpPro, tensileWeftPro, tearWarpPro, tearWeftPro, colorFatDryPro, colorFatWetPro, phPro, sprAPro, sprBPro, rubDryPro, rubWetPro
);

$(function () {

    

    wvId.on("change", function () {

        var selectedItemVal = $(this).val();

        if (selectedItemVal) {
            $.ajax({
                async: true,
                cache: false,
                data: {
                    "wvId": selectedItemVal
                },
                type: "GET",
                url: "/RndFabricInfo/GetAssociateObjects",
                success: function (data) {
                    console.log(data);

                    if (data !== null && data !== undefined) {

                        resetFields(targets);
                        addClass(targets);

                        if (data.rndSampleInfoWeaving !== null) {
                            totalEnds.val(data.rndSampleInfoWeaving.proG_.sd.totaL_ENDS);
                            epiPpiGrEpi.val(data.rndSampleInfoWeaving.gR_EPI);
                            epiPpigrPpi.val(data.rndSampleInfoWeaving.gR_PPI);
                            fnPpi.val(data.rndSampleInfoWeaving.fnppi);
                            weaveName.val(data.rndSampleInfoWeaving.weaveNavigation.name);
                            hiddenWeaveId.val(data.rndSampleInfoWeaving.weaveNavigation.wid);
                            reedSpace.val(data.rndSampleInfoWeaving.proG_.sd.reeD_SPACE);
                            reedCount.val(data.rndSampleInfoWeaving.reeD_COUNT);
                            reedDent.val(data.rndSampleInfoWeaving.reeD_DENT);
                            dyeingCode.val(data.rndSampleInfoWeaving.proG_.sd.dyeingcode);
                            fabCode.val(data.rndSampleInfoWeaving.fabcode);
                            dyeingName.val(data.rndSampleInfoWeaving.proG_.sd.d.dtype);
                            hiddenDId.val(data.rndSampleInfoWeaving.proG_.sd.d.did);
                            colorName.val(data.rndSampleInfoWeaving.proG_.sd.sdrf.color);
                            totalRope.val(data.rndSampleInfoWeaving.proG_.sd.nO_OF_ROPE);
                        } else {
                            toastr.warning("Rnd Sample Info Weaving", "Not Found");
                        }

                        if (data.rndSampleInfoDyeing !== null) {
                            ////loomType.val(data.rndSampleInfoDyeing.loom.looM_TYPE_NAME);
                            ////hiddenLoomId.val(data.rndSampleInfoDyeing.loom.loomid);
                            //loomType.html("");
                            //loomType.append('<option value="" selected>Select Loom Type</option>');
                            //loomType.append($("<option>",
                            //    {
                            //        value: option.rndSampleInfoDyeing.loom.loomid,
                            //        text: option.rndSampleInfoDyeing.loom.looM_TYPE_NAME
                            //    }));
                            //loomType.html("");
                            //loomType.append('<option value="" selected>Select Sub-Category</option>');

                            //$.each(data,
                            //    function (id, option) {
                            //        loomType.append($("<option>",
                            //            {
                            //                value: option.ltg.prog.rnD_SAMPLE_INFO_WEAVING.loom.loomid,
                            //                text: option.ltg.prog.rnD_SAMPLE_INFO_WEAVING.loom.looM_TYPE_NAME
                            //            }));
                            //    });
                            loomType.val(data.ltg.prog.rnD_SAMPLE_INFO_WEAVING.loom.loomid).trigger("change");
                            loomType.text(data.ltg.prog.rnD_SAMPLE_INFO_WEAVING.loom.looM_TYPE_NAME).trigger("change");
                        } else {
                            toastr.warning("Rnd Sample Info Dyeing", "Not Found");
                        }

                        if (data.fnEpiPpi !== null) {
                            fnEpi.val(data.fnEpiPpi.fnEpi);
                            //fnPpi.val(data.fnEpiPpi.fnpi);
                            afterWashEpi.val(data.fnEpiPpi.washEpi);
                            afterWashPpi.val(data.fnEpiPpi.washPpi);
                        } else {
                            toastr.warning("Finish EPI X PPI", "Not Found");
                        }

                        if (data.rndFabtestGrey !== null) {
                            weightBeforeWash.val(data.rndFabtestGrey.wggrbw);
                            weightAfterWash.val(data.rndFabtestGrey.wggraw);
                            widthBeforeWash.val(data.rndFabtestGrey.wigrbw);
                            widthAfterWash.val(data.rndFabtestGrey.wigraw);
                            shrinkageGreigeWarp.val(data.rndFabtestGrey.srgrwrap);
                            shrinkageGreigeWeft.val(data.rndFabtestGrey.srgrweft);
                        } else {
                            toastr.warning("Rnd Fab Test Grey", "Not Found");
                        }

                        if (data.rndFabtestSample !== null) {
                            weightFinishBeforeWash.val(data.rndFabtestSample.wgfnbw);
                            weightFinishAfterWash.val(data.rndFabtestSample.wgfnaw);
                            widthFinishBeforeWash.val(data.rndFabtestSample.wifnbw);
                            widthFinishAfterWash.val(data.rndFabtestSample.wifnaw);
                            widthFinishCutable.val(data.rndFabtestSample.wifncut);
                            shrinkageFinishWarp.val(data.rndFabtestSample.srfnwarp);
                            shrinkageFinishWeft.val(data.rndFabtestSample.srfnweft);
                            //stretchGreigeWarp.val(data.rndFabtestSample);
                            //stretchGreigeWeft.val(data.rndFabtestSample);
                            stretchFinishWarp.val(data.rndFabtestSample.stfnwarp);
                            stretchFinishWeft.val(data.rndFabtestSample.stfnweft);
                            growthFinishWarp.val(data.rndFabtestSample.grfnwarp);
                            growthFinishWeft.val(data.rndFabtestSample.grfnweft);
                            skewMove.val(data.rndFabtestSample.skewmove);
                            slippageWarp.val(data.rndFabtestSample.slpwarp);
                            slippageWeft.val(data.rndFabtestSample.slpweft);
                            tensileWarp.val(data.rndFabtestSample.tnwarp);
                            tensileWeft.val(data.rndFabtestSample.tnweft);
                            tearWarp.val(data.rndFabtestSample.trwarp);
                            tearWeft.val(data.rndFabtestSample.trweft);
                            colorFatDry.val(data.rndFabtestSample.cfatdry);
                            colorFatWet.val(data.rndFabtestSample.cfatnet);
                            ph.val(data.rndFabtestSample.ph);
                            composition.val(data.rndFabtestSample.fabcomp);
                            labTestNo.val(data.rndFabtestSample.ltsid);
                        } else {

                            removedItems.push(
                                weightFinishBeforeWash, weightFinishAfterWash, widthFinishBeforeWash, widthFinishAfterWash, widthFinishCutable, shrinkageFinishWarp,
                                shrinkageFinishWeft, stretchFinishWarp, stretchFinishWeft, growthFinishWarp, growthFinishWeft, skewMove,
                                slippageWarp, slippageWeft, tensileWarp, tensileWeft, tearWarp, tearWeft,
                                colorFatDry, colorFatWet, ph, composition, labTestNo, sprA, sprB, rubDryFn, rubWetFn
                            );

                            removeClass(removedItems);
                            toastr.warning("Rnd Fabric Test Sample", "Not Found");
                        }
                    }
                },
                complete: function () {
                    $.ajax({
                        async: true,
                        cache: false,
                        data: $('#form').serialize(),
                        type: "POST",
                        url: "/RndFabricInfo/GetRndFabricCountInfoTable",
                        success: function (partialView) {
                            previousList.html(partialView);
                        }
                    });
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });
        } else {
            resetFields(targets);
            removeClass(targets);
            previousList.empty();
            toastr.warning("Please Select A Valid Option From Dropdown", "Invalid Option");
        }
    });

    sFinId.on("change", function () {

        var selectedItemVal = $(this).val();
        console.log(selectedItemVal);
        if (selectedItemVal !== "") {
            $.ajax({
                async: true,
                cache: false,
                data: {
                    "sFinId": selectedItemVal
                },
                type: "GET",
                url: "/RndFabricInfo/GetAssociateObjectsBySFinId",
                success: function (data) {
                    console.log(data);

                    if (data !== null && data !== undefined) {
                        resetFields(targets);
                        addClass(targets);
                        colorFnName.val(data.color.color);

                        shadeColor.val(data.colorcode).trigger("change");
                        if (data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0] !== null) {
                            finishRoute.val(data.finisH_ROUTE);
                        } else {
                            toastr.warning("Rnd Sample Info Finishing", "Not Found");
                        }

                        if (data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0] !== null) {
                            weaveName.val(data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0].weaveNavigation.name);
                            hiddenWeaveId.val(data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0].weaveNavigation.wid);
                            reedCount.val(data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0].reeD_COUNT);
                            reedDent.val(data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0].reeD_DENT);
                            fabCode.val(data.stylE_NAME);
                            loomType.val(data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0].loom.looM_TYPE_NAME);
                            $("#rND_FABRICINFO_DEVID").val(data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0].fabcode);
                            hiddenLoomId.val(data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0].loom.loomid);
                            concept.val(data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0].concept);
                            getRs();
                        } else {
                            toastr.warning("Rnd Sample Info Weaving", "Not Found");
                        }

                        if (data.ltg.prog.sd !== null) {

                            dyeingName.val(data.ltg.prog.sd.d.dtype);
                            hiddenDId.val(data.ltg.prog.sd.d.did);
                            colorName.val(data.color.color);
                            totalRope.val(data.ltg.prog.sd.nO_OF_ROPE);
                            dyeingCode.val(data.ltg.prog.sd.dyeingcode);
                            dyeingCode.val(data.ltg.prog.sd.dyeingcode);
                            reedSpace.val(data.ltg.prog.sd.reeD_SPACE);
                            totalEnds.val(data.ltg.prog.sd.totaL_ENDS);
                            totalEndsSample.val(data.ltg.prog.sd.totaL_ENDS);
                            progNo.val(data.ltg.prog.sd.dyeinG_REF);
                            //if (data.ltg.prog.sd.reeD_SPACE !== null && data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0].loom.looM_TYPE_NAME !== null) {
                            //    var loom = data.ltg.prog.rnD_SAMPLE_INFO_WEAVING[0].loom.looM_TYPE_NAME;
                            //    pickLength.val(loom === "Rapier" ? parseFloat(data.ltg.prog.sd.reeD_SPACE) + 5 : parseFloat(data.ltg.prog.sd.reeD_SPACE) + 3);
                            //}

                            getRs();

                        } else {
                            toastr.warning("Rnd Sample Info Dyeing", "Not Found");
                        }
                        if (data.ltg !== null) {
                            epiPpiGrEpi.val(data.ltg.grepi);
                            epiPpigrPpi.val(data.ltg.grppi);
                            weightBeforeWash.val(data.ltg.wggrbw);
                            weightAfterWash.val(data.ltg.wggraw);
                            widthBeforeWash.val(data.ltg.wigrbw);
                            widthAfterWash.val(data.ltg.wigraw);
                            shrinkageGreigeWarp.val(data.ltg.srgrwrap);
                            shrinkageGreigeWeft.val(data.ltg.srgrweft);
                            stretchGreigeWarp.val(data.ltg.sgwarp);
                            stretchGreigeWeft.val(data.ltg.sgweft);
                            lsgTestNo.val(data.ltg.laB_NO).trigger("change");
                        } else {
                            toastr.warning("Rnd Fab Test Grey", "Not Found");
                        }

                        //if (data.rnD_FABTEST_SAMPLE !== null) {

                        //    ltsid.html('');
                        //    ltsid.append('<option value="" selected>Select A Lab Test No.</option>');
                        //    $.each(data.rnD_FABTEST_SAMPLE,
                        //    function (id, option) {
                        //        ltsid.append($('<option></option>').val(option.ltsid).html(option.ltsid));
                        //    });


                        //    fnEpi.val(data.rnD_FABTEST_SAMPLE[0].fnepi);
                        //    fnPpi.val(data.rnD_FABTEST_SAMPLE[0].fnppi);
                        //    weightFinishBeforeWash.val(data.rnD_FABTEST_SAMPLE[0].wgfnbw);
                        //    weightFinishAfterWash.val(data.rnD_FABTEST_SAMPLE[0].wgfnaw);
                        //    widthFinishBeforeWash.val(data.rnD_FABTEST_SAMPLE[0].wifnbw);
                        //    widthFinishAfterWash.val(data.rnD_FABTEST_SAMPLE[0].wifnaw);
                        //    widthFinishCutable.val(data.rnD_FABTEST_SAMPLE[0].wifncut);
                        //    shrinkageFinishWarp.val(data.rnD_FABTEST_SAMPLE[0].srfnwarp);
                        //    shrinkageFinishWeft.val(data.rnD_FABTEST_SAMPLE[0].srfnweft);
                        //    stretchFinishWarp.val(data.rnD_FABTEST_SAMPLE[0].stfnwarp);
                        //    stretchFinishWeft.val(data.rnD_FABTEST_SAMPLE[0].stfnweft);
                        //    growthFinishWarp.val(data.rnD_FABTEST_SAMPLE[0].grfnwarp);
                        //    growthFinishWeft.val(data.rnD_FABTEST_SAMPLE[0].grfnweft);
                        //    skewMove.val(data.rnD_FABTEST_SAMPLE[0].skewmove);
                        //    slippageWarp.val(data.rnD_FABTEST_SAMPLE[0].slpwarp);
                        //    slippageWeft.val(data.rnD_FABTEST_SAMPLE[0].slpweft);
                        //    tensileWarp.val(data.rnD_FABTEST_SAMPLE[0].tnwarp);
                        //    tensileWeft.val(data.rnD_FABTEST_SAMPLE[0].tnweft);
                        //    tearWarp.val(data.rnD_FABTEST_SAMPLE[0].trwarp);
                        //    tearWeft.val(data.rnD_FABTEST_SAMPLE[0].trweft);
                        //    colorFatDry.val(data.rnD_FABTEST_SAMPLE[0].cfatdry);
                        //    colorFatWet.val(data.rnD_FABTEST_SAMPLE[0].cfatnet);
                        //    ph.val(data.rnD_FABTEST_SAMPLE[0].ph);
                        //    composition.val(data.rnD_FABTEST_SAMPLE[0].fabcomp);
                        //    labTestNo.val(data.rnD_FABTEST_SAMPLE[0].ltsid);
                        //} else {

                        //    removedItems.push(
                        //        weightFinishBeforeWash, weightFinishAfterWash, widthFinishBeforeWash, widthFinishAfterWash, widthFinishCutable, shrinkageFinishWarp,
                        //        shrinkageFinishWeft, stretchFinishWarp, stretchFinishWeft, growthFinishWarp, growthFinishWeft, skewMove,
                        //        slippageWarp, slippageWeft, tensileWarp, tensileWeft, tearWarp, tearWeft,
                        //        colorFatDry, colorFatWet, ph, composition, labTestNo
                        //    );

                        //    removeClass(removedItems);
                        //    toastr.warning("Rnd Fabric Test Sample", "Not Found");
                        //}
                    }

                },
                //complete: function () {
                //    $.ajax({
                //        async: true,
                //        cache: false,
                //        data: $('#form').serialize(),
                //        type: "POST",
                //        url: "/RndFabricInfo/GetRndFabricCountInfoTableBySFinId",
                //        success: function (partialView) {
                //            previousList.html(partialView);
                //            const ttlWeft = xhr.getResponseHeader("TotalWeft");
                //            totalWeft.val(ttlWeft);
                //        }
                //    });
                //},
                error: function () {
                    console.log("failed to load Data...");
                }
            });
        } else {
            resetFields(targets);
            removeClass(targets);
            previousList.empty();
            toastr.warning("Please Select A Valid Option From Dropdown", "Invalid Option");
        }
    });

    ltsid.on("change", function () {
        var selectedItemVal = $(this).val();
        if (selectedItemVal === "") {
            return false;
        }

        $.ajax({
            async: true,
            cache: false,
            data: {
                "ltsId": selectedItemVal
            },
            type: "GET",
            url: "/RndFabricInfo/GetLabTestResult",
            success: function (data) {
                console.log(data);
                if (data !== null) {
                    fnEpi.val(data.fnepi);
                    fnPpi.val(data.fnppi);

                    afterWashEpi.val(data.washepi);
                    afterWashPpi.val(data.washppi);
                    beforeWashEpi.val(data.fnepi);
                    beforeWashPpi.val(data.fnppi);
                    weightFinishBeforeWash.val(data.wgfnbw);
                    weightFinishAfterWash.val(data.wgfnaw);
                    widthFinishBeforeWash.val(data.wifnbw);
                    widthFinishAfterWash.val(data.wifnaw);
                    widthFinishCutable.val(data.wifncut);
                    shrinkageFinishWarp.val(data.srfnwarp);
                    shrinkageFinishWeft.val(data.srfnweft);
                    stretchFinishWarp.val(data.stfnwarp);
                    stretchFinishWeft.val(data.stfnweft);
                    growthFinishWarp.val(data.grfnwarp);
                    growthFinishWeft.val(data.grfnweft);
                    skewMove.val(data.skewmove);
                    slippageWarp.val(data.slpwarp);
                    slippageWeft.val(data.slpweft);
                    tensileWarp.val(data.tnwarp);
                    tensileWeft.val(data.tnweft);
                    tearWarp.val(data.trwarp);
                    tearWeft.val(data.trweft);
                    colorFatDry.val(data.cfatdry);
                    colorFatWet.val(data.cfatnet);
                    ph.val(data.ph);
                    sprA.val(data.spiralitY_A);
                    sprB.val(data.spiralitY_A);
                    rubDryFn.val(data.cfatdry);
                    rubWetFn.val(data.cfatnet);
                    composition.val(data.fabcomp);
                    labTestNo.val(data.ltsid);
                } else {

                    removedItems.push(
                        weightFinishBeforeWash, weightFinishAfterWash, widthFinishBeforeWash, widthFinishAfterWash, widthFinishCutable, shrinkageFinishWarp,
                        shrinkageFinishWeft, stretchFinishWarp, stretchFinishWeft, growthFinishWarp, growthFinishWeft, skewMove,
                        slippageWarp, slippageWeft, tensileWarp, tensileWeft, tearWarp, tearWeft,
                        colorFatDry, colorFatWet, ph, composition, labTestNo, sprA, sprB, rubDryFn, rubWetFn
                    );

                    removeClass(removedItems);
                    toastr.warning("Rnd Fabric Test Sample", "Not Found");
                }
            },
            //complete: function () {
            //    GetCountList();
            //},
            error: function () {
                console.log("failed to attach...");
            }
        });

    });


    ltsidPro.on("change", function () {
        debugger;
        var selectedItemVal = $(this).val();
        if (selectedItemVal === "") {
            return false;
        }
        console.log(selectedItemVal);
        $.ajax({
            async: true,
            cache: false,
            data: {
                "ltsId": selectedItemVal
            },
            type: "GET",
            url: "/RndFabricInfo/GetLabTestResult",
            success: function (data) {
                console.log(data);
                if (data !== null) {
                    //fnEpi.val(data.fnepi);
                    //fnPpi.val(data.fnppi);

                    afterWashEpiPro.val(data.washepi);
                    afterWashPpiPro.val(data.washppi);
                    beforeWashEpiPro.val(data.fnepi);
                    beforeWashPpiPro.val(data.fnppi);
                    weightBeforeWashPro.val(data.wgfnbw);
                    weightAfterWashPro.val(data.wgfnaw);
                    widthBeforeWashPro.val(data.wifnbw);
                    widthAfterWashPro.val(data.wifnaw);
                    widthCutablePro.val(data.wifncut);
                    shrinkageWarpPro.val(data.srfnwarp);
                    shrinkageWeftPro.val(data.srfnweft);
                    stretchWarpPro.val(data.stfnwarp);
                    stretchWeftPro.val(data.stfnweft);
                    growthWarpPro.val(data.grfnwarp);
                    growthWeftPro.val(data.grfnweft);
                    skewMovePro.val(data.skewmove);
                    slippageWarpPro.val(data.slpwarp);
                    slippageWeftPro.val(data.slpweft);
                    tensileWarpPro.val(data.tnwarp);
                    tensileWeftPro.val(data.tnweft);
                    tearWarpPro.val(data.trwarp);
                    tearWeftPro.val(data.trweft);
                    colorFatDryPro.val(data.cfatdry);
                    colorFatWetPro.val(data.cfatnet);
                    phPro.val(data.ph);
                    sprAPro.val(data.spiralitY_A);
                    sprBPro.val(data.spiralitY_A);
                    rubDryPro.val(data.cfatdry);
                    rubWetPro.val(data.cfatnet);
                    compositionPro.val(data.fabcomp);
                    /*labTestNo.val(data.ltsid);*/
                } else {

                    removedItems.push(
                        afterWashEpiPro, afterWashPpiPro, beforeWashEpiPro, beforeWashPpiPro,
                        weightBeforeWashPro, weightAfterWashPro,
                        widthBeforeWashPro, widthAfterWashPro, widthCutablePro, shrinkageWarpPro, shrinkageWeftPro, stretchWarpPro, stretchWeftPro, growthWarpPro, growthWeftPro, skewMovePro, slippageWarpPro, slippageWeftPro, tensileWarpPro, tensileWeftPro, tearWarpPro, tearWeftPro, colorFatDryPro, colorFatWetPro, phPro, sprAPro, sprBPro, rubDryPro, rubWetPro
                    );

                    removeClass(removedItems);
                    toastr.warning("Rnd Fabric Test Sample", "Not Found");
                }
            },
            error: function () {
                console.log("failed to attach...");
            }
        });

    });


    btnAdd.on("click", function () {
        if (totalEnds.val()) {
            $.ajax({
                async: true,
                cache: false,
                data: $("#form").serialize(),
                type: "POST",
                url: "/RndFabricInfo/AddFabricCountinfo",
                success: function (partialView) {
                    attachTo.html(partialView);
                },
                error: function () {
                    console.log("failed to attach...");
                }
            });
        } else {
            toastr.warning("Please Enter Total Ends First", "warning");
        }
    });

    fnPpi.on("change", function () {
        GetCountList();
    });
    epiPpigrPpi.on("change", function () {
        GetCountList();
    });
});


function GetCountList() {
    if (fnPpi.val() && epiPpigrPpi.val()) {
        $.ajax({
            async: true,
            cache: false,
            data: $('#form').serialize(),
            type: "POST",
            url: "/RndFabricInfo/GetRndFabricCountInfoTableBySFinId",
            success: function (partialView, status, xhr) {
                previousList.html(partialView);
                const ttlWeft = xhr.getResponseHeader("TotalWeft");
                totalWeft.val(ttlWeft);
            }
        });
    }
}