$(function () {

    $("#table").dataTable({

        searchPane: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [[1, "desc"]],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/HSampleDespatchM/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [8],
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
            "processing": "<i class='fa fa-4x fa-spinner text-info' aria-hidden='true'></i>",
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
                "data": "sddate",
                "name": "SDDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "gpno", "name": "GPNO", "autoWidth": true },
            {
                "data": "gpdate",
                "name": "GPDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "hsp.buyeR_NAME", "name": "HSP.BUYER_NAME", "autoWidth": true },
            { "data": "brand.brandname", "name": "BRAND.BRANDNAME", "autoWidth": true },
            //{ "data": "hst.team.teaM_NAME", "name": "HST.TEAM.TEAM_NAME", "autoWidth": true },
            { "data": "through", "name": "THROUGH", "autoWidth": true },
            { "data": "cosT_STATUS", "name": "COST_STATUS", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a href="#" class='fa fa-print fa-2x-custom text-primary mr-2'>Coming soon...</a><a href='/HSampleDespatchM/DetailsHSampleDespatchM?hsdId=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a><a href='/HSampleDespatchM/EditHSampleDespatchM?hsdId=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning mr-2'></a><a href='#' class='fa fa-trash fa-2x-custom text-danger mr-2' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;
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
                window.location = `/HSampleDespatchM/DeleteHSampleDespatchM?hsdId=${item}`;
            }
        });
}