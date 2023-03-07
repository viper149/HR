﻿
$(function () {
    $("#table").dataTable({
        "searchPanes": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "pageLength": 25,
        "lengthMenu": [10, 25, 50, 100, 200, 500, 1000],
        "autoWidth": false,
        "ajax": {
            "async": "true",
            "url": "/CosStandardCons/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0, 9],
            "orderable": false,
            "searchable": false
        },
        {
            "targets": [0],
            "visible": false
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
            { "data": "scid", "name": "SCID", "autoWidth": true },
            { "data": "monthlY_COST", "name": "MONTHLY_COST", "autoWidth": true },
            { "data": "prod", "name": "PROD", "autoWidth": true },
            { "data": "overheaD_USD", "name": "OVERHEAD_USD", "autoWidth": true },
            { "data": "loomspeed", "name": "LOOMSPEED", "autoWidth": true },
            { "data": "efficiency", "name": "EFFICIENCY", "autoWidth": true },
            {
                "data": "status", "name": "STATUS", "autoWidth": true,
                "render": function (data, type, row) {
                    if (type === "display" || type === "filter") {
                        return data ? "Active" : "Inactive";
                    }
                    return data;
                }
            },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "data": "username",
                "name": "USERNAME",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? data : "N/A";
                }
            },
            {
                "render": function (data, type, row) {
                    return "<a href='/CosStandardCons/DetailsStandardCost?scId=" + row.encryptedId + "' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a>";
                }
            }
        ]
    });
});