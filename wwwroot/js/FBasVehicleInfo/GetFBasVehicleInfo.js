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
            "url": "/FBasVehicleInfo/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [3],
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
            { "data": "vnumber", "name": "VNUMBER", "autoWidth": true },
            { "data": "vehiclE_TYPE", "name": "VEHICLE_TYPE", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a target='_blank' title='Sample Bay Report: ${row.dpId}' href='/SampleFabricDispatch/BayDispatchReport/${row.encryptedId}' class='fa fa-print fa-lg text-primary'></a> <a href='/SampleFabricDispatch/Details/${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info'></a> <a href='/SampleFabricDispatch/Edit/${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a> <a href='#' class='fa fa-trash fa-2x-custom text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;
                    
                }
            }]
    });
});

//function deleteSwal(e, item) {

//    e.preventDefault();

//    swal({
//        title: "Please Confirm",
//        text: "Are you sure to delete?",
//        type: "warning",
//        showCancelButton: true,
//        confirmButtonText: "Yes, sir",
//        cancelButtonText: "Not at all"
//    },
//        function (isConfirm) {
//            if (isConfirm) {
//                window.location = `/SampleFabricDispatch/Delete/${item}`;
//            }
//        });
//}