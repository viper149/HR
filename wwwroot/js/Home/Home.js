$(function () {

    $("#welcomeNotificationModal").modal("show");

    const chart1 = new CanvasJS.Chart("piChartContainer", {
        exportEnabled: true,
        animationEnabled: true,
        title: {
            text: "Summary > RnD, L/C & PI"
        },
        subtitles: [{
            text: "Test Data Analysis",
            fontSize: 13
        }],
        data: [{
            type: "pie",
            startAngle: 60,
            indexLabelFontSize: 16,
            toolTipContent: "<b>{label}</b>: {y}",
            showInLegend: "true",
            legendText: "{label}",
            indexLabel: "{label} - {y}",
            dataPoints: myData
        }]
    });

    const chart2 = new CanvasJS.Chart("funnelChartContainer", {
        exportEnabled: true,
        animationEnabled: true,
        zoomEnabled: true,
        title: {
            text: "Summary > Sample Garments"
        },
        subtitles: [{
            text: "Sample Garments Analysis",
            fontSize: 16
        }],
        axisX: {
            title: "Factory > Sample Garments Dispatched.",
            titleFontSize: 16
        },
        axisY: {
            title: "Head Office > Sample Garments Dispatched.",
            includeZero: true,
            titleFontSize: 16
        },
        data: [{
            type: "scatter",
            toolTipContent: "<b>HOSGD: </b>{x}.<br/><b>FSGD: </b> {y}.",
            dataPoints: myData2
        }]
    });

    var chart3 = new CanvasJS.Chart("barChartContainer", {
        exportEnabled: true,
        animationEnabled: true,
        title: {
            text: "Monthly Average Temperature Variation in Bangladesh"
        },
        axisX: {
            valueFormatString: "MMMM"
        },
        axisY: {
            title: "Temperature (°C)",
            suffix: " °C"
        },
        data: [{
            type: "rangeSplineArea",
            indexLabel: "{y[#index]}°",
            xValueFormatString: "MMMM YYYY",
            toolTipContent: "{x} </br> <strong>Temperature: </strong> </br> Min: {y[0]} °C, Max: {y[1]} °C",
            dataPoints: [
                { x: new Date(2016, 00, 01), y: [7, 18] },
                { x: new Date(2016, 01, 01), y: [11, 23] },
                { x: new Date(2016, 02, 01), y: [15, 28] },
                { x: new Date(2016, 03, 01), y: [22, 36] },
                { x: new Date(2016, 04, 01), y: [26, 39] },
                { x: new Date(2016, 05, 01), y: [27, 37] },
                { x: new Date(2016, 06, 01), y: [27, 34] },
                { x: new Date(2016, 07, 01), y: [26, 33] },
                { x: new Date(2016, 08, 01), y: [24, 33] },
                { x: new Date(2016, 09, 01), y: [19, 31] },
                { x: new Date(2016, 10, 01), y: [13, 27] },
                { x: new Date(2016, 11, 01), y: [08, 21] }
            ]
        }]
    });

    var chart4 = new CanvasJS.Chart("chartContainer", {
        animationEnabled: true,

        title: {
            text: "Export Country"
        },
        axisX: {
            interval: 1
        },
        axisY2: {
            interlacedColor: "rgba(1,77,101,.2)",
            gridColor: "rgba(1,77,101,.1)",
            title: "Number of Companies"
        },
        data: [{
            type: "bar",
            name: "companies",
            axisYType: "secondary",
            color: "#014D65",
            dataPoints: [
                { y: 3, label: "Sweden" },
                { y: 7, label: "Taiwan" },
                { y: 5, label: "Russia" },
                { y: 9, label: "Spain" },
                { y: 7, label: "Brazil" },
                { y: 7, label: "India" },
                { y: 9, label: "Italy" },
                { y: 8, label: "Australia" },
                { y: 11, label: "Canada" },
                { y: 15, label: "South Korea" },
                { y: 12, label: "Netherlands" },
                { y: 15, label: "Switzerland" },
                { y: 25, label: "Britain" },
                { y: 28, label: "Germany" },
                { y: 29, label: "France" },
                { y: 52, label: "Japan" },
                { y: 103, label: "China" },
                { y: 134, label: "US" }
            ]
        }]
    });

    var chart5 = new CanvasJS.Chart("bubbleChartContainer", {
        animationEnabled: true,
        title: {
            text: "X vs Y"
        },
        axisX: {
            title: "Land Area (million sq. km)",
            maximum: 17,
            minimum: -.1
        },
        axisY: {
            title: "X",
            gridColor: "lightgrey",
            tickColor: "lightgrey",
            lineThickness: 0,
            valueFormatString: "#,##0 km,.",
            interval: 50000
        },
        data: [{
            type: "bubble",
            markerType: "triangle",
            toolTipContent: "<b>{label}</b><br/><b>Land Area:</b> {x}mn sq. km <br/><b>Rail Road:</b> {y} km<br/> <b>Population:</b>{z}mn",
            dataPoints: [
                { x: 9.14, y: 228513, z: 309.34, label: "US" },
                { x: 16.37, y: 85292, z: 141.92, label: "Russia" },
                { x: 9.327, y: 66239, z: 1337, label: "China" },
                { x: 9.09, y: 58345, z: 34.12, label: "Canada" },
                { x: 8.45, y: 29817, z: 194.94, label: "Brazil" },
                { x: 7.68, y: 8615, z: 22.29, label: "Australia" },
                { x: 2.97, y: 63974, z: 1224.61, label: "India" },
                { x: 2.73, y: 25023, z: 40.41, label: "Argentina" },
                { x: 1.94, y: 26704, z: 113.42, label: "Mexico" },
                { x: 1.21, y: 22051, z: 49.99, label: "SA" },
                { x: .547, y: 33608, z: 65.07, label: "France" },
                { x: .241, y: 31471, z: 62.23, label: "U.K" },
                { x: .348, y: 33708, z: 81.77, label: "Germany" },
                { x: .364, y: 20035, z: 127.45, label: "Japan" },
                { x: .995, y: 5195, z: 81.12, label: "Egypt" },
                { x: .743, y: 5352, z: 17.11, label: "Chile" }
            ]
        }]
    });

    chart1.render();
    chart2.render();
    chart3.render();
    chart4.render();
    chart5.render();
});