$(function () {

    $("#table").dataTable({

        searchPane: true,
        processing: true,
        serverSide: true,
        filter: true,
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/RndSampleInfoFinish/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [6],
            "orderable": false,
            "searchable": false
        }],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": "<i class=\"fa fa-2x fa-search\" aria-hidden=\"true\"></i>",
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
            {
                "data": "finishdate",
                "name": "FINISHDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                  return moment(data).format(row.dateFormat);
                }
            },
            { "data": "stylE_NAME", "name": "STYLE_NAME", "autoWidth": true },
            { "data": "finisH_ROUTE", "name": "FINISH_ROUTE", "autoWidth": true },
            { "data": "processeD_LENGTH", "name": "PROCESSED_LENGTH", "autoWidth": true },
            { "data": "washpick", "name": "WASHPICK", "autoWidth": true },
            { "data": "grconst", "name": "GRCONST", "autoWidth": true },
            { "data": "bwgbw", "name": "BWGBW", "autoWidth": true },
            { "data": "bwgaw", "name": "BWGAW", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return "<a href='/RndSampleInfoFinish/DetailsRndSampleInfoFinish?fnId=" + row.encryptedId + "' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a>" +
                        "<a href='/RndSampleInfoFinish/EditRndSampleInfoFinish?fnId=" + row.encryptedId + "' class='fa fa-edit fa-2x-custom text-warning mr-2'></a>" +
                        "<a href='#' class='fa fa-trash fa-2x-custom text-danger' onclick=deleteSwal(event,'" + row.encryptedId + "');></a>";
                }
            }]
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
                window.location = "/RndSampleInfoFinish/DeleteRndSampleInfoFinish?fnId=" + item;
            }
        });
}