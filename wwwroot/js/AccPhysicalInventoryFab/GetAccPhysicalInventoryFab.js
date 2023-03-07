$(function () {
    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [0, "desc"],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/AccPhysicalInventoryFab/GetTableData",
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
            { "data": "fpI_ID", "name": "FPI_ID", "autoWidth": true, "visible": false },
            { "data": "index", "name": "INDEX", "autoWidth": true},
            {
                "data": "fpI_DATE", "name": "FPI_DATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "rolL_.rolL_.rollno", "name": "ROLL_ID", "autoWidth": true },
            { "data": "rolL_.fabcodeNavigation.stylE_NAME", "name": "STYLE_NAME", "autoWidth": true },
            { "data": "rolL_.rolL_.faB_GRADE", "name": "FAB_GRADE", "autoWidth": true }
            //{
            //    "render": function (data, type, row) {
            //        var deleteUrl = `<a href='#' class='fa fa-trash fa-2x-custom text-danger mr-2' onclick=deleteSwal('${row
            //            .encryptedId}');></a >`;

            //        return `${deleteUrl}`;
            //    }
            //}
        ]
    });
});

function deleteSwal(item) {
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
                window.location = `/AccPhysicalInventoryFab/DeleteAccPhysicalInventoryFab?id=${item}`;
            }
        });
}