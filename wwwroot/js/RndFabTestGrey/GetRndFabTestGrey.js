$(function () {

    $("#table").dataTable({

        searchPane: true,
        processing: true,
        serverSide: true,
        filter: true,
        order:[[0,"desc"]],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/RndFabTestGrey/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [5],
            "orderable": false,
            "searchable": false
        }],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": "<i class=\"fa fa-2x fa-search\" aria-hidden=\"true\"></i>",
            "emptyTable": "<h3>No data available</h3>",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoFiltered": "(filtered from _MAX_ total entries)",
            "thousands": ",",
            "processing": "<i class=\"fa fa-spinner fa-spin fa-3x fa-fw\"></i>",
            "zeroRecords": "<h3>No matching records found</h3>",
            "paginate": {
                "first": "<i class='fa fa-angle-double-left' aria-hidden='true'></i>",
                "last": "<i class='fa fa-angle-double-right' aria-hidden='true'></i>",
                "next": "<i class='fa fa-angle-right' aria-hidden='true'></i>",
                "previous": "<i class='fa fa-angle-left' aria-hidden='true'></i>"
            },
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            }
        },
        columns: [
            { "data": "laB_NO", "name": "LAB_NO", "autoWidth": true },
            {
                "data": "ltgdate",
                "name": "LTGDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return moment(data).format(row.dateFormat);
                }
            },
            { "data": "optioN1", "name": "OPTION1", "autoWidth": true },
            { "data": "progn.opT1", "name": "Set", "autoWidth": true },
            { "data": "emP_WASHEDBY.firsT_NAME", "name": "EMP_WASHEDBY.FIRST_NAME", "autoWidth": true },
            { "data": "emP_UNWASHEDBY.firsT_NAME", "name": "EMP_UNWASHEDBY.FIRST_NAME", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var detailsUrl = `<a href='/RndFabTestGrey/DetailsRndFabTestGrey?ltgId=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a>`;

                    return `${detailsUrl}`;
                }
            }]
    });
});