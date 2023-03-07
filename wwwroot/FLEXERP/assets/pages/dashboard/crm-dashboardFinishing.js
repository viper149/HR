"use strict";
$(document).ready(function () {
    function e() {
        return {
            title: { display: !1 },
            tooltips: { intersect: !1, mode: "nearest", xPadding: 10, yPadding: 10, caretPadding: 10 },
            legend: { display: !1 },
            hover: { mode: "index" },
            scales: { xAxes: [{ display: !1, gridLines: !1, scaleLabel: { display: !0, labelString: "Month" } }], yAxes: [{ display: !1, gridLines: !1, scaleLabel: { display: !0, labelString: "Value" }, ticks: { min: 1, max: 50 } }] },
            elements: { point: { radius: 4, borderWidth: 12 } },
            layout: { padding: { left: 0, right: 0, top: 0, bottom: 0 } }
        }
    }

    function t(e, t, a) {
        return (
            null == a && (a = "rgba(0,0,0,0)"),
            {
                labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"],
                datasets: [
                    {
                        label: "",
                        borderColor: e,
                        borderWidth: 3,
                        hitRadius: 30,
                        pointRadius: 0,
                        pointHoverRadius: 4,
                        pointBorderWidth: 2,
                        pointHoverBorderWidth: 12,
                        pointBackgroundColor: Chart.helpers.color("#000000").alpha(0).rgbString(),
                        pointBorderColor: e,
                        pointHoverBackgroundColor: e,
                        pointHoverBorderColor: Chart.helpers.color("#000000").alpha(.1).rgbString(),
                        fill: !0, lineTension: 0,
                        backgroundColor: Chart.helpers.color(a).alpha(.7).rgbString(),
                        data: t,
                    },
                ]
            }

        );
    }

    jQuery.ajax({
        url: "/Chart/WarpingProductionList",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        success: function (resultData) {
            /*console.log(resultData);*/
            AmCharts.makeChart("Warping", {
                type: "serial",
                hideCredits: !0,
                theme: "light",
                dataDateFormat: "YYYY-MM-DD",
                precision: 2,
                valueAxes: [
                    {
                        id: "v1",
                        title: "Production",
                        position: "left",
                        autoGridCount: !1,
                        labelFunction: function (e) {
                            return "Yds " + Math.round(e);
                        },
                    },
                    { id: "v2", title: "Grade %", gridAlpha: 0, position: "right", autoGridCount: !1 },
                ],
                graphs: [
                    {
                        id: "g3",
                        valueAxis: "v1",
                        lineColor: "#feb798",
                        fillColors: "#feb798",
                        fillAlphas: 1,
                        type: "column",
                        title: "Total Production",
                        valueField: "totalWarping",
                        clustered: !1,
                        columnWidth: 0.5,
                        legendValueText: "[[value]] Yds",
                        balloonText: "[[title]]<br /><b style='font-size: 130%'>[[value]] Yds</b>",
                    },
                    //{
                    //    id: "g4",
                    //    valueAxis: "v1",
                    //    lineColor: "#fe9365",
                    //    fillColors: "#fe9365",
                    //    fillAlphas: 1,
                    //    type: "column",
                    //    title: "A2",
                    //    valueField: "sales1",
                    //    clustered: !1,
                    //    columnWidth: 0.3,
                    //    legendValueText: "[[value]] Yds",
                    //    balloonText: "[[title]]<br /><b style='font-size: 130%'>[[value]] Yds</b>",
                    //},
                    //{
                    //    id: "g1",
                    //    valueAxis: "v2",
                    //    bullet: "round",
                    //    bulletBorderAlpha: 1,
                    //    bulletColor: "#FFFFFF",
                    //    bulletSize: 5,
                    //    hideBulletsCount: 50,
                    //    lineThickness: 2,
                    //    lineColor: "#0df3a3",
                    //    type: "smoothedLine",
                    //    title: "Production",
                    //    useLineColorForBulletBorder: !0,
                    //    valueField: "market1",
                    //    balloonText: "[[title]]<br /><b style='font-size: 130%'>[[value]] Yds</b>",
                    //},
                    //{
                    //    id: "g2",
                    //    valueAxis: "v2",
                    //    bullet: "round",
                    //    bulletBorderAlpha: 1,
                    //    bulletColor: "#FFFFFF",
                    //    bulletSize: 5,
                    //    hideBulletsCount: 50,
                    //    lineThickness: 2,
                    //    lineColor: "#fe5d70",
                    //    dashLength: 5,
                    //    title: "Non Production",
                    //    useLineColorForBulletBorder: !0,
                    //    valueField: "market2",
                    //    balloonText: "[[title]]<br /><b style='font-size: 130%'>[[value]] Yds</b>",
                    //},
                ],
                chartCursor: { pan: !0, valueLineEnabled: !0, valueLineBalloonEnabled: !0, cursorAlpha: 0, valueLineAlpha: 0.2 },
                categoryField: "date1",
                categoryAxis: { parseDates: !0, dashLength: 1, minorGridEnabled: !0 },
                legend: { useGraphSettings: !0, position: "top" },
                balloon: { borderThickness: 1, cornerRadius: 5, shadowAlpha: 0 },
                dataProvider: resultData,
            }),
                document.getElementById("newuserchart").getContext("2d")
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });

    jQuery.ajax({
        url: "/Chart/RopeWarpingProductionList",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        success: function (resultData) {
            debugger;
            console.log(resultData);

            var o = document.getElementById("tot-lead").getContext("2d"),

                o = new Chart(o, {
                    type: "line",
                    data: t("#01a9ac", [30, 15, 25, 35, 30, 20, 15, 20, 25, 40, 25, 30, 22, 31], "#01a9ac"),
                    options: e()
                });
            /*document.getElementById("monthly-graph").getContext("2d")*/
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });

    jQuery.ajax({
        url: "/Chart/DirectWarpingProductionList",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        success: function (resultData) {
            debugger;
            console.log(resultData);

            var o = document.getElementById("tot-vendor").getContext("2d"),
                o = new Chart(o, {
                    type: "line",
                    data: t("#01a9ac", [30, 15, 25, 35, 30, 20, 15, 20, 25, 40, 25, 30, 22, 31], "#01a9ac"),
                    options: e()
                });
            /*document.getElementById("monthly-graph").getContext("2d")*/
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });

    jQuery.ajax({
        url: "/Chart/EcruWarpingProductionList",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        success: function (resultData) {
            debugger;
            console.log(resultData);

            var o = document.getElementById("invoice-gen").getContext("2d"),
                o = new Chart(o, {
                    type: "line",
                    data: t("#b71c1c", [30, 15, 25, 35, 30, 20, 15, 20, 25, 40, 25, 30, 22, 31], "#b71c1c"),
                    options: e()
                });
            /*document.getElementById("monthly-graph").getContext("2d")*/
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });

});