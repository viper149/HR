
$(function () {

    $("#table").dataTable({
        "searchPanes": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "pageLength": 25,
        "order": [0, "desc"],
        "lengthMenu": [10, 25, 50, 100, 200, 500, 1000],
        "autoWidth": false,
        "ajax": {
            "async": "true",
            "url": "/ComExFabStyle/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [
            {
                "targets": [6],
                "orderable": false,
                "searchable": false
            },
            {
                "targets": [0],
                "visible": false,
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
        "columns": [
            { "data": "createD_AT", "name": "CREATED_AT", "autoWidth": true },
            { "data": "stylename", "name": "STYLENAME", "autoWidth": true },
            { "data": "brand.brandname", "name": "BRAND.BRANDNAME", "autoWidth": true },
            { "data": "optioN2", "name": "OPTION2", "autoWidth": true },
            { "data": "hscode", "name": "HSCODE", "autoWidth": true },
            { "data": "status", "name": "STATUS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a href='/ComExFabStyle/DetailsComFabStyleInfo?styleId=${row.encryptedId}' class='fa fa-info-circle fa-lg text-info'></a> <a href='/ComExFabStyle/EditComFabStyleInfo?styleId=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a> <a href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;
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
                window.location = `/ComExFabStyle/DeleteComFabStyleInfo/?styleId=${item}`;
            }
        });
}