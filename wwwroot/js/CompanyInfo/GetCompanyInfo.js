
$(function () {
    $("#table").dataTable({
        "searchPanes": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "order": [0, "desc"],
        "pageLength": 25,
        "lengthMenu": [10, 25, 50, 100, 200, 500, 1000],
        "autoWidth": false,
        "ajax": {
            "async": "true",
            "url": "/Company/GetData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [6],
            "orderable": false,
            "searchable": false
        }],
        "language": {
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
        "columns": [
            { "data": "id", "name": "ID", "autoWidth": true, "visible": false},
            {
                "name": "LOGO", "autoWidth": true,
                "render": function (data, type, row) {
                    return `<img style='height:3rem;' alt='${row.companY_NAME}' src='${row.logoBase64}'>`;
                }
            },
            { "data": "companY_NAME", "name": "COMPANY_NAME", "autoWidth": true},
            //{ "data": "tagline", "name": "TAGLINE", "autoWidth": true },
            //{ "data": "factorY_ADDRESS", "name": "FACTORY_ADDRESS", "autoWidth": true },
            //{ "data": "headofficE_ADDRESS", "name": "HEADOFFICE_ADDRESS", "autoWidth": true },
            //{ "data": "weB_ADDRESS", "name": "WEB_ADDRESS", "autoWidth": true },
            { "data": "email", "name": "EMAIL", "autoWidth": true },
            { "data": "phonE1", "name": "PHONE1", "autoWidth": true },
            //{ "data": "phonE2", "name": "PHONE2", "autoWidth": true },
            //{ "data": "phonE3", "name": "PHONE3", "autoWidth": true },
            //{ "data": "biN_NO", "name": "BIN_NO", "autoWidth": true },
            //{ "data": "etiN_NO", "name": "ETIN_NO", "autoWidth": true },
            //{ "data": "description", "name": "DESCRIPTION", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var editUrl = `<a href='/Company/Edit?id=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                    var detailsUrl = `<a href='/Company/Details?id=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-primary'></a>`;
                    var deleteUrl = `<a href='#' class='fa fa-trash fa-2x-custom text-danger mr-2' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;

                    return `${detailsUrl} ${editUrl} ${deleteUrl}`;
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
                window.location = '/Company/Delete/?id=' + item;
            }
        });
}