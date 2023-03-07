$(function () {

    $("#table").dataTable({
        searchPane: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [[0, "desc"]],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/FPrInspectionRejectionB/GetTableData",
            "type": "POST",
            "datatype": "json",
            /*"success": function (data) { console.log(data) }*/
        },

        columnDefs: [
            {
                "targets": [11],
                "orderable": false,
                "searchable": false
            }
        ],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": "<strong>Search By Date</strong> <i class=\"fa fa-2x fa-search\" aria-hidden=\"true\"></i>",
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
            { "data": "ibR_ID", "name": "IBR_ID", "autoWidth": true, "visible": false },
            {
                "data": "tranS_DATE", "name": "TRANS_DATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "dofF_.wV_BEAM.wV_PROCESS.set.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.stylE_NAME",
                "name": "STYLE_NAME", "autoWidth": true
            },
            { "data": "dofF_.wV_BEAM.wV_PROCESS.set.proG_.proG_NO", "name": "PROG_NO", "autoWidth": true },
            { "data": "dofF_.looM_NONavigation.looM_NO", "name": "LOOM_NO", "autoWidth": true },
            { "data": "redectioN_YDS", "name": "REDECTION_YDS", "autoWidth": true },
            { "data": "sectioN_.secname", "name": "SECNAME", "autoWidth": true },
            { "data": "shiftNavigation.shift", "name": "SHIFT", "autoWidth": true },
            { "data": "defecT_.name", "name": "NAME", "autoWidth": true },
            {
                "data": "doffingG_DATE", "name": "DOFFING_DATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "doffinG_LENGTH", "name": "DOFFING_LENGTH", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var editUrl = `<a href='/FPrInspectionRejectionB/EditFPrInspectionRejectionB?ibrId=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a> <a href='#' class='fa fa-trash fa-2x-custom text-danger mr-2' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;

                    return `${editUrl}`;
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
                window.location = "/FPrInspectionRejectionB/DeleteFPrInspectionRejectionB/?id=" + item;
            }
        });
}