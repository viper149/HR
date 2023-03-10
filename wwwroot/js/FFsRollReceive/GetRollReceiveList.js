
$(function () {
    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [[0, "asc"]],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/FFsRollReceive/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [3],
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
            {
                "data": "rcvdate",
                "name": "RCVDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "name": "OPT1", "autoWidth": true,
                "render": function (data, type, row) {
                    var noOfRolls = row.f_FS_FABRIC_RCV_DETAILS.length;
                    return `${noOfRolls}`;
                }
            },
            {
                "data": "sec.secname", "name": "SEC.SECNAME", "autoWidth": true,
                "render": function (data, type, row) {
                    var sec = row.sec == null ? "" : row.sec.secname;
                    return `${sec}`;
                }
            },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a href='/FFsRollReceive/DetailsRollReceive?id=${row.encryptedId
                        }' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a>`;

                    //<a href='/FFsRollReceive/EditRollReceive?id=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning mr-2'></a><a href='/FFsRollReceive/DeleteRollReceive?id=${row.encryptedId}' class='fa fa-trash fa-2x-custom text-danger mr-2'></a>`;
                }
            }
        ]
    });
});