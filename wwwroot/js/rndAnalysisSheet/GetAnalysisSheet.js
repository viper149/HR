
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
            "url": "/RndAnalysisSheet/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [4],
            "orderable": false,
            "searchable": false
        }],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": '<i class="fa fa-2x fa-search" aria-hidden="true"></i>',
            "emptyTable": '<h3>No data available</h3>',
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
            { "data": "mkT_QUERY_NO", "name": "MKT_QUERY_NO", "autoWidth": true },
            { "data": "rnD_QUERY_NO", "name": "RND_QUERY_NO", "autoWidth": true },
            { "data": "buyeR_REF", "name": "BUYER_REF", "autoWidth": true },
            {
                "data": "rnD_HEAD_APPROVE", "name": "RND_HEAD_APPROVE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data ? "<span class='text-success'>Approved</span>" : "<span class='text-danger'>Not Approved</span>";
                }
            },
            {
                "render": function (data, type, row) {
                    if (!row.rnD_HEAD_APPROVE) {
                        return `<a href='#' class='fa fa-trash fa-2x-custom text-danger mr-2' onclick=deleteSwal(event,'${row.encryptedId}');></a><a href='/RndAnalysisSheet/DetailsAnalysisSheetInfo?id=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a><a href='/RndAnalysisSheet/EditAnalysisSheetInfo?id=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                    } else {
                        return `<a href='/RndAnalysisSheet/DetailsAnalysisSheetInfo?id=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a>`;
                    }

                }
            },
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
                window.location = `/RndAnalysisSheet/DeleteAnalysisSheetInfo/?id=${item}`;
            }
        });
}