
$(function () {
    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [[0, "desc"]],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/AccLocalDoMaster/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [7],
                "orderable": false,
                "searchable": false
            }
        ],
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
            { "data": "dono", "name": "DONO", "autoWidth": true },
            {
                "data": "dodate",
                "name": "DODATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "comExScinfo.scno", "name": "ComExScinfo.SCNO", "autoWidth": true },
            { "data": "auditby", "name": "AUDITBY", "autoWidth": true },
            {
                "data": "auditon",
                "name": "AUDITON",
                "autoWidth": true,
                "render": function (data, type, row) {
                    if (type === "display" || type === "filter") {
                        return data ? data : "N/A";
                    }
                    return data;
                }
            },
            { "data": "comments", "name": "COMMENTS", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function(data, type, row) {
                    var detailsUrl =
                        `<a href='/AccountDO/Local/Details/${row.encryptedId
                            }' class='fa fa-info-circle fa-2x-custom text-info'>`;
                    var reportUrl =
                        `<a target="_blank" title="After Audit" href="/Account-Finance/LocalDO/After-Audit/GetReports/${
                        row.encryptedId}" class="fa fa-print fa-lg text-primary ml-2"></a>`;
                    var editUrl =
                        `<a href='/AccLocalDoMaster/EditAccDoMaster?transId=${row.encryptedId
                        }' class='fa fa-edit fa-2x-custom text-warning'></a>`;

                    /*var deleteUrl = `<a class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;*/
                    var deleteUrl = `<a class='fa fa-trash fa-lg text-danger' href='/AccountDO/Local/Delete/${row.encryptedId}'></a>`;

                    return (row.auditby === null) ? `${detailsUrl} ${editUrl} ${reportUrl} ${deleteUrl}` : `${detailsUrl} ${reportUrl} ${deleteUrl}`;
                }
            }
        ]
    });
});


//function deleteSwal(e, item) {
//    e.preventDefault();

//    swal({
//        title: "Are you sure?",
//        text: "Once deleted, you will not be able to recover this row!",
//        icon: "warning",
//        buttons: true,
//        dangerMode: true,
//    }).then((willDelete) => {
//        if (willDelete) {

//            swal("Done! Your file has been deleted!", {
//                icon: "success"
//            });

//            post("/AccLocalDoMaster/DeleteAccLocalDoMaster", { doId: item });
//        } else {
//            swal("Your file is safe!");
//        }
//    });
//}