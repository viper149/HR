
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
            "url": "/FYsGp/GetTableData",
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
            { "data": "gpid", "name": "GPID", "autoWidth": true },
            { "data": "gpno", "name": "GPNO", "autoWidth": true },
            { "data": "gptype", "name": "GPTYPE", "autoWidth": true },
            {
                "data": "gpdate", "name": "GPDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },

            {
                "render": function (data, type, row) {

                    //var EditUrl = `<a href='/FYsGp/EditFYsGp?dsId=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'> </a>`;

                    var deleteUrl = `<a href='#' class='fa fa-trash fa-2x-custom text-danger mr-2' onclick=deleteSwal(event, '${row
                        .encryptedId}');></a >`;

                    //return `${EditUrl} ${DeleteUrl}`;

                    return ` <a  href='/FYsGp/EditFYsGp?dsId=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a> <a  href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;
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
                window.location = `/FYsGp/DeleteFYsGp?dsId=${item}`;
            }
        });
}