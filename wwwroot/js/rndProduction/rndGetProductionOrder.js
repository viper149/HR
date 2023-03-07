
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
            "url": "/RndProductionOrder/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [8],
                "orderable": false,
                "searchable": false
            },
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [7],
                render: $.fn.dataTable.render.ellipsis(25)
            }
        ],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": '<i class="fa fa-2x fa-search" aria-hidden="true"></i>',
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
            { "data": "createD_AT", "name": "CREATED_AT", "autoWidth": true },
            { "data": "otype.otypename", "name": "OTYPE.OTYPENAME", "autoWidth": true },
            { "data": "opT3", "name": "POID", "autoWidth": true },
            { "data": "opT1", "name": "ORDERNO", "autoWidth": true },
            { "data": "opT2", "name": "FABCODE", "autoWidth": true },
            { "data": "orpt.orptname", "name": "ORPT.ORPTNAME", "autoWidth": true },
            { "data": "ordeR_QTY_YDS", "name": "ORDER_QTY_YDS", "autoWidth": true },
            { "data": "ordeR_QTY_MTR", "name": "ORDER_QTY_MTR", "autoWidth": true },
            {
                "data": "remarks", "name": "REMARKS", "autoWidth": true,
                //  "render": function (data, type, row) {
                //  return data!==null && type === "display" && data.length > 25 ?
                //    data.substr(0, 25) + "…" : data;
                //}
            },
            {
                "render": function (data, type, row) {
                    var orderNo = 0;
                    if (row.otypeid === 401 ||
                        row.otypeid === 402 ||
                        row.otypeid === 419 ||
                        row.otypeid === 422) {
                        if (row.so.sO_NO != null) {
                            orderNo = row.so.sO_NO;
                        }
                    } else {
                        if (row.rs != null) {
                            orderNo = row.rs.dyeingcode === null ? "" : row.rs.dyeingcode;
                        }
                    }
                    return "<a target='_blank' title='Print Production Order Sheet' href='/Reports/RProductionOrderReport?soNo=" + orderNo + "' class='fa fa-print fa-2x-custom text-primary mr-2'></a>" +
                        "<a href='/RndProductionOrder/EditProductionOrder?id=" + row.encryptedId + "' class='fa fa-edit fa-2x-custom text-warning mr-2'></a>" +
                        "<a href='/RndProductionOrder/DetailsProductionOrder?id=" + row.encryptedId + "' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a>"
                        //+ "<a href='#' class='fa fa-trash fa-2x-custom text-danger' onclick=deleteSwal(event,'" + row.encryptedId + "');></a>"
                        ;
                }
            }
        ]
    });
});

function deleteSwal(e, item) {
    e.preventDefault();
    swal({
        title: "Please Confirm",
        text: "Are you sure to delete?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {
            if (isConfirm) {
                window.location = "/RndProductionOrder/DeleteProductionOrder?id=" + item;
            }
        });
}