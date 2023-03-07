
$(function () {
    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [[0, "desc"]],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/ChemicalTransaction/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [12],
            "orderable": false,
            "searchable": false
        },
        {
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": '<i class="fa fa-2x fa-search" aria-hidden="true"></i>',
            "emptyTable": "<h3>No data available</h3>",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoFiltered": "(filtered from _MAX_ total entries)",
            "thousands": ",",
            "processing": "<i class='fa fa-4x fa-spinner' aria-hidden='true'></i>",
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
            { "data": "ctrid", "name": "CTRID", "autoWidth": true },
            { "data": "product.productname", "name": "PRODUCT.PRODUCTNAME", "autoWidth": true },
            { "data": "crcv.batchno", "name": "CRCV.BATCHNO", "autoWidth": true },
            {
                "data": "ctrdate", "name": "CTRDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "crcvid", "name": "CRCVID", "autoWidth": true },
            { "data": "rcvt.rcvtype", "name": "RCVT.RCVTYPE", "autoWidth": true },
            { "data": "cissueid", "name": "CISSUEID", "autoWidth": true },
            { "data": "issue.issutype", "name": "ISSUE.ISSUTYPE", "autoWidth": true },
            { "data": "oP_BALANCE", "name": "OP_BALANCE", "autoWidth": true },
            { "data": "rcV_QTY", "name": "RCV_QTY", "autoWidth": true },
            { "data": "issuE_QTY", "name": "ISSUE_QTY", "autoWidth": true },
            { "data": "balance", "name": "BALANCE", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true }
        ]
    });
});