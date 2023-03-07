
$(function () {
    $('#table').dataTable({
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
            "url": "/ProductionRopeWarping/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [
            {
                "targets": [8],
                "orderable": false,
                "searchable": false
            }
        ],
        "language": {
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
        "columns": [
            { "data": "opT3", "name": "OPT3", "autoWidth": true },
            { "data": "opT4", "name": "OPT4", "autoWidth": true },
            {
                "data": "deliverY_DATE", "name": "DELIVERY_DATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "timE_START", "name": "TIME_START", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "timE_END", "name": "TIME_END", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "opT1", "name": "opT1", "autoWidth": true },
            { "data": "opT2", "name": "opT2", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    //if (row.iS_DECLARE) {
                    //    //return  "<a target='_blank' title='Print Warp Delivery Report' href='/FPrWarpingProcessRopeMaster/RWarpingDeliveryReport?groupNo=" + row.opT3+ "' class='fa fa-print fa-2x-custom text-primary mr-2'></a>" + "<a href='/FPrWarpingProcessRopeMaster/DetailsWarpingProcessRope?id=" + row.encryptedId + "' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a>";
                    //    return `<a target='_blank' title='Print Warp Delivery Report' href='/FPrWarpingProcessRopeMaster/RWarpingDeliveryReport?groupNo=${row.opT3}' class='fa fa-print fa-lg text-primary'></a> <a target='_blank' title='Print Warp Delivery Sticker' href='/FPrWarpingProcessRopeMaster/RWarpingStrickerReport' class='fa fa-print fa-lg text-primary'></a> <a href='/FPrWarpingProcessRopeMaster/DetailsWarpingProcessRope?id=${row.encryptedId}' class='fa fa-info-circle fa-lg text-info'></a>`;
                    //} else {
                        //return "<a target='_blank' title='Print Warp Delivery Report' href='/FPrWarpingProcessRopeMaster/RWarpingDeliveryReport?groupNo=" + row.opT3 + "' class='fa fa-print fa-2x-custom text-primary mr-2'></a>" + "<a href='/FPrWarpingProcessRopeMaster/DetailsWarpingProcessRope?id=" + row.encryptedId + "' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a> <a href='/FPrWarpingProcessRopeMaster/EditWarpingProcessRope?id=" + row.encryptedId + "' class='fa fa-edit fa-2x-custom text-warning'></a>";
                    return `<a target='_blank' title='Print Warp Delivery Report' href='/FPrWarpingProcessRopeMaster/RWarpingDeliveryReport?groupNo=${row.opT3}' class='fa fa-print fa-lg text-primary'></a> <a target='_blank' title='Print Warp Delivery Sticker' href='/FPrWarpingProcessRopeMaster/RWarpingStrickerReport' class='fa fa-print fa-lg text-primary'></a> <a href='/FPrWarpingProcessRopeMaster/DetailsWarpingProcessRope?id=${row.encryptedId}' class='fa fa-info-circle fa-lg text-info'></a> <a href='/FPrWarpingProcessRopeMaster/EditWarpingProcessRope?id=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a>`;
                    //}
                }
            }
        ]
    });
});