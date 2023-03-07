$(function () {

    $("#table").dataTable({

        searchPane: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [0, "desc"],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/ComExCertificateManagement/GetTableData",
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



            { "data": "inv.invno", "name": "INVNO", "autoWidth": true },
            { "data": "organiC_TYPE", "name": "ORGANIC_TYPE", "autoWidth": true },
            { "data": "organiC_REF", "name": "ORGANIC_REF", "autoWidth": true },
            { "data": "iC_TYPE", "name": "IC_TYPE", "autoWidth": true },
            { "data": "iC_REF", "name": "IC_REF", "autoWidth": true },
            { "data": "rcS_REF", "name": "RCS_REF", "autoWidth": true },
            { "data": "grS_REF", "name": "GRS_REF", "autoWidth": true },
            { "data": "pscP_REF", "name": "PSCP_REF", "autoWidth": true },

            {
                "render": function (data, type, row) {



                    var editUrl = `<a href='/ComExCertificateManagement/EditComExCertificateManagement?lbId=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a>`

                    return `${editUrl}`
                }
            }]
    });
});