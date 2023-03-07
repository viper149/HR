
$(function () {
    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        order: [0, "desc"],
        filter: true,
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/GeneralStoreReceive/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [8],
                "orderable": false,
                "searchable": false
            }
        ],
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
            { "data": "grcvid", "name": "GRCVID", "autoWidth": true },
            {
                "data": "rcvdate", "name": "RCVDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "rcvt.rcvtype", "name": "RCVT.RCVTYPE", "autoWidth": true },
            { "data": "challaN_NO", "name": "CHALLAN_NO", "autoWidth": true },
            {
                "data": "challaN_DATE", "name": "CHALLAN_DATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "vehicaL_NO", "name": "VEHICAL_NO", "autoWidth": true },
            
            {
                "data": "qcpass", "name": "QCPASS", "autoWidth": true, "class": "text-center",
                "render": function (data, type, row) {
                    return data === null ? "<span class='text-danger'>Not Approved</span>" : `<span class='text-success'>${row.qcpassNavigation.gsqcano}</span>`;
                }
            },
            {
                "data": "mrr", "name": "MRR", "autoWidth": true, "class": "text-center",
                "render": function (data, type, row) {
                    return data === null ? "<span class='text-danger'>Not Created</span>" : `<span class='text-success'>${row.mrrNavigation.gsmrrno}</span>`;
                }
            },
            {
                "render": function (data, type, row) {

                    var detailsUrl = `<a href='/GeneralStoreReceive/Details/${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-primary'></a>`;
                    var editUrl = `<a href='/GeneralStoreReceive/Edit/${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                    var locked = `<i title="This item is locked." class="fa fa-lock fa-2x-custom text-danger" aria-hidden="true"></i>`;

                    return row.isLocked ? `${detailsUrl} ${locked}` : `${detailsUrl} ${editUrl}`;
                }
            }
        ]
    });
});