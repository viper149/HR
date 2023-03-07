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
            "url": "/Employee/GetData",
            "type": "POST",
            "datatype": "json",
        },
        columnDefs: [{
            "targets": [14],
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
            { "data": "empid", "name": "EMPID", "autoWidth": true, "visible": false },
            { "data": "empno", "name": "EMPNO", "autoWidth": true },
            { "data": "proX_CARD", "name": "PROX_CARD", "autoWidth": true },
            { "data": "name", "name": "NAME", "autoWidth": true },
            { "data": "namE_BN", "name": "NAME_BN", "autoWidth": true },
            { "data": "gender.genname", "name": "GENDER", "autoWidth": true },
            { "data": "religion.relegioN_NAME", "name": "RELIGION", "autoWidth": true },
            { "data": "bldgrp.bldgrP_NAME", "name": "BLDGRP", "autoWidth": true },
            //{ "data": "company.companY_NAME", "name": "COMPANY", "autoWidth": true },
            { "data": "desig.deS_NAME", "name": "DESIG", "autoWidth": true },
            { "data": "dept.deptname", "name": "DEPT", "autoWidth": true },
            { "data": "sec.seC_NAME", "name": "SEC", "autoWidth": true },
            { "data": "subsec.subseC_NAME", "name": "SUBSEC", "autoWidth": true },
            { "data": "od.oD_FULL_NAME", "name": "OFF_DAY", "autoWidth": true },
            { "data": "niD_NO", "name": "NID_NO", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var editUrl = `<a href='/Employee/Edit?id=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                    var deleteUrl = `<a href='#' class='fa fa-trash fa-2x-custom text-danger mr-2' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;

                    return `${editUrl} ${deleteUrl}`;
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
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {
            if (isConfirm) {
                window.location = '/Employee/Delete/?id=' + item;
            }
        });
}