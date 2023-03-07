
$(function () {

    $("#table").dataTable({
        "searchPanes": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "order": [[0, "desc"]],
        "pageLength": 25,
        "lengthMenu": [10, 25, 50, 100, 200, 500, 1000],
        "autoWidth": false,
        "ajax": {
            "async": "true",
            "url": "/ComExScInfo/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [7],
            "orderable": false,
            "searchable": false
        }],
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
            { "data": "scno", "name": "SCNO", "autoWidth": true },
            {
                "data": "scdate",
                "name": "SCDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "scperson", "name": "SCPERSON", "autoWidth": true },
            { "data": "bcperson", "name": "BCPERSON", "autoWidth": true },
            {
                "data": "deldate",
                "name": "DELDATE",
                "autoWidth": true,
                "defaultContent": "N/A",
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "paydate",
                "name": "PAYDATE",
                "autoWidth": true,
                "defaultContent": "N/A",
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "paymode", "name": "PAYMODE", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var detailsUrl = `<a href='/ComExScInfo/DetailsScInfo?scNo=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info'></a>`;
                    var editUrl = `<a href='/ComExScInfo/EditScInfo?scNo=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                    var reportUrl = `<a target='_blank' title='Report Of Sales Contact No: ${row.scno}' href='/Reports/SalesContactInfoRpt/${row.scid}' class='fa fa-print fa-lg text-primary'></a>`;
                    
                    //var locked =
                    //    `<i title="This item is locked." class="fa fa-lock fa-2x-custom text-danger" aria-hidden="true"></i>`;

                    return `${reportUrl} ${detailsUrl} ${editUrl}`;

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
                window.location = "/ComExScInfo/DeleteScInfo/?scNo=" + item;
            }
        });
}