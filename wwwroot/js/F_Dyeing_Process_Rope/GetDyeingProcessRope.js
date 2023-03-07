
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
            "url": "/FDyeingProcessRopeMaster/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            { width: 500, targets: 3 },
            {
                "targets": [8],
                "orderable": false,
                "searchable": false
            },
            {
                "targets": [0],
                "visible": false,
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
            { "data": "ropE_DID", "name": "ROPE_DID", "autoWidth": true },
            {
                "data": "trnsdate", "name": "TRNSDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "group.grouP_NO", "name": "GROUP.GROUPID", "autoWidth": true },
            { "data": "group.optioN1", "name": "GROUP.OPTION1", "autoWidth": true },
            { "data": "dyeinG_CODE", "name": "DYEING_CODE", "autoWidth": true },
            { "data": "grouP_LENGTH", "name": "GROUP_LENGTH", "autoWidth": true },
            { "data": "dyeinG_LENGTH", "name": "DYEING_LENGTH", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {

                    var isChemUse = row.f_DYEING_PROCESS_ROPE_CHEM.length > 0 ? `<i title='Chemical Used' class="fa fa-tint text-success" aria-hidden="true"></i>` : ``;

                    return `<a target='_blank' title='Print Dyeing Delivery Report' href='/FDyeingProcessRopeMaster/RDyeingDeliveryReport?groupNo=${row.group.grouP_NO}' class='fa fa-print fa-2x-custom text-primary'></a> <a target='_blank' title='Print Dyeing Sticker Report' href='/FDyeingProcessRopeMaster/RDyeingStickerReport?groupNo=${row.group.grouP_NO}' class='fa fa-print fa-2x-custom text-primary'></a> <a href='/FDyeingProcessRopeMaster/EditDyeingProcessRope?id=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a> <a href='/FDyeingProcessRopeMaster/DetailsDyeingProcessRope?id=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info'></a> <a href='#' class='fa fa-trash fa-2x-custom text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a> ${isChemUse}`;
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
                window.location = `/FDyeingProcessRopeMaster/DeleteDyeingProcessRope?id=${item}`;
            }
        });
}