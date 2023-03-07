
$(function () {
    $("#table").dataTable({
        "searchPanes": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "order":[[0,"desc"]],
        "pageLength": 25,
        "lengthMenu": [10, 25, 50, 100, 200, 500, 1000],
        "autoWidth": false,
        "ajax": {
            "async": "true",
            "url": "/CosPreCostingMaster/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [
            {
                "targets": [6],
                "orderable": false,
                "searchable": false
            }
        ],
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
            { "data": "csid", "name": "CSID", "autoWidth": true },
            { "data": "csdatestring", "name": "CSDATESTRING", "autoWidth": true },
            { "data": "fabcodeNavigation.stylE_NAME", "name": "FABCODE", "autoWidth": true },
            { "data": "rate", "name": "RATE", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            { "data": "username", "name": "USERNAME", "autoWidth": true },
            {
                "render": function (data, type, row) {

                    var details = `<a target='_blank' title='Print Cost Sheet' href='/CosPreCostingMaster/RPreCostingReport?csId=${row.csid}' class='fa fa-print fa-lg text-primary'></a>`;
                    var editButton = row.optioN1 === "0" ? ` <a href='/CosPreCostingMaster/EditPreCostingMaster?id=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a> <a href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>` : "";
                    
                    if (row.hasAccess) {
                        return `${details} <a href='/CosPreCostingMaster/DetailsPreCostingMaster?csId=${row.encryptedId}' class='fa fa-info-circle fa-lg text-info'></a>` + editButton + ` <a href='#' class='fa fa-repeat fa-lg text-success' onclick=amentSwal(event,'${row.encryptedId}');></a>`;

                    } else {
                        return details;
                    }
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
      confirmButtonText: "Yes",
      cancelButtonText: "Not at all"
    },
    function (isConfirm) {
      if (isConfirm) {
          window.location = `/CosPreCostingMaster/DeletePreCostingMaster?id=${item}`;
      }
    });
}
function amentSwal(e, item) {

  e.preventDefault();

  swal({
      title: "Please Confirm",
      text: "Are you sure to do Pre-Costing Again?",
      type: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes",
      cancelButtonText: "Not at all"
    },
    function (isConfirm) {
      if (isConfirm) {
          window.location = `/CosPreCostingMaster/RePreCostingMaster?csId=${item}`;
      }
    });
}