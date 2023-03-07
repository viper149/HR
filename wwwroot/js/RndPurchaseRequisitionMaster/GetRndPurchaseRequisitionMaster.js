
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
            "url": "/RndPurchaseRequisitionMaster/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [9],
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
            { "data": "indslid", "name": "INDSLID", "autoWidth": true },
            { "data": "indenT_SL_NO", "name": "INDENT_SL_NO", "autoWidth": true },
            {
                "data": "indsldate", "name": "INDSLDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "indslno", "name": "INDSLNO", "autoWidth": true },
            { "data": "resemp.firsT_NAME", "name": "RESEMP.FIRST_NAME", "autoWidth": true },
            { "data": "emp.firsT_NAME", "name": "EMP.EMP_NAME", "autoWidth": true },
            { "data": "yarN_FOR", "name": "YARN_FOR", "autoWidth": true },
            { "data": "ordeR_NONavigation.so.sO_NO", "name": "ORDER_NO", "autoWidth": true },
            {
                "data": "status", "name": "STATUS", "autoWidth": true,
                "render": function (data, type, row) {
                    if (data === "1") {
                        return "<span class='text-success'>Item Used In Indent.</span>";
                    } else {
                        return "<span class='text-danger'>Item Not Used Yet.</span>";
                    }
                }
            },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a target='_blank' title='Print Purchase Requisation' href='/Reports/RYarnRequisitionReport?ysrid=${row.indslid}' class='fa fa-print fa-2x-custom text-primary mr-2'></a><a href="/Requisition/Details/${row.encryptedId}" class='fa fa-info-circle fa-2x-custom text-primary mr-2'></a><a href="/Requisition/Edit/${row.encryptedId}" class='fa fa-edit fa-2x-custom text-warning'></a>`;
                }
            },
        ]
    });
});