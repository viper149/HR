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
            "url": "/SampleFabricDispatch/GetTableData",
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
            { "data": "gpno", "name": "GPNO", "autoWidth": true },
            {
                "data": "gpdate",
                "name": "GPDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "iS_CANCELLED", "name": "IS_CANCELLED", "autoWidth": true,
                "render": function (data, type, row) {
                    return data ? `<i class="fa fa-ban text-danger" aria-hidden="true"> Cancelled</i>` : `<i class="fa fa-check text-success" aria-hidden="true"> Live</i>`;
                }
            },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var detailsUrl =
                        `<a href='/SampleFabricDispatch/Details/${row.encryptedId
                            }' class='fa fa-info-circle fa-2x-custom text-info'>`;
                    var deleteUrl = `<a class='fa fa-trash fa-lg text-danger' href='/SampleFabricDispatch/Delete/${row.encryptedId}'></a>`;
                    var editUrl =
                        `<a href='/SampleFabricDispatch/Edit/${row.encryptedId
                        }' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                    var reportUrl =
                        `<a target="_blank" title="Sample Bay Report: ${row.dpId}" href="/SampleFabricDispatch/BayDispatchReport/${row.encryptedId}" class="fa fa-print fa-lg text-primary ml-2"></a>`;

                    return (`${detailsUrl} ${reportUrl} ${editUrl} ${deleteUrl}`);

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