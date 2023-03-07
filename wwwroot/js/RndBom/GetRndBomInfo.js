$(function () {

    $("#table").dataTable({

        searchPane: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [0, "desc"],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/RndBOM/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [10],
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
            {
                "data": "tranS_DATE", "name": "TRNS_DATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "opT1", "name": "FABCODE", "autoWidth": true },
            { "data": "finisH_TYPENavigation.typename", "name": "FINISH_TYPE", "autoWidth": true },
            { "data": "colorNavigation.color", "name": "COLOR", "autoWidth": true },
            { "data": "totaL_ENDS", "name": "TOTAL_ENDS", "autoWidth": true },
            { "data": "loT_RATIO", "name": "LOT_RATIO", "autoWidth": true },
            { "data": "width", "name": "WIDTH", "autoWidth": true },
            { "data": "setno", "name": "SETNO", "autoWidth": true },
            { "data": "proG_NO", "name": "PROG_NO", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    /* return `<a href="" class="fa fa-info-circle fa-2x-custom text-primary mr-2"></a><a href="" class="fa fa-edit fa-2x-custom text-warning mr-2"></a><a href="" class="fa fa-trash fa-2x-custom text-danger"></a>`;*/
                    var editUrl = `<a href='/RndBOM/EditRndBOM?rbId=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a>`;
                    /*var deleteUrl = `<a href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;*/

                    return `${editUrl}`;
                }
            }]
    });
});