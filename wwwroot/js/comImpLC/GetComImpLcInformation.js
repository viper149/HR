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
            "url": "/CommercialImportLC/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            { width: 50, targets: 0 },
            { width: 50, targets: 1 },
            { width: 50, targets: 2 },
            { width: 50, targets: 3 },
            { width: 50, targets: 4 },
            { width: 50, targets: 5 },
            { width: 50, targets: 6 },
            { width: 50, targets: 7 },
            { width: 50, targets: 8 },
            { width: 50, targets: 9 },
            { width: 50, targets: 10 },
            { width: 200, targets: 11 },
            { width: 50, targets: 12 },
            { width: 50, targets: 13 },
            { width: 50, targets: 14 },
            {
                "targets": [14],
                "orderable": false,
                "searchable": false
            },
            {
                "targets": [0],
                "visible": false,
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
            { "data": "lC_ID", "name": "LC_ID", "autoWidth": true },
            { "data": "lcno", "name": "LCNO", "autoWidth": true },
            {
                "data": "lcdate",
                "name": "LCDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "comExLcinfo.lcno", "name": "ComExLcinfo.LCNO", "autoWidth": true },
            {
                "data": "comExLcinfo.lcdate",
                "name": "ComExLcinfo.LCDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "comExLcinfo.value", "name": "ComExLcinfo.VALUE", "autoWidth": true },
            { "data": "lienval", "name": "LIENVAL", "autoWidth": true },
            { "data": "coM_IMP_LCTYPE.typename", "name": "TYPENAME", "autoWidth": true },
            { "data": "currency", "name": "CURRENCY", "autoWidth": true },
            {
                "data": "shipdate",
                "name": "SHIPDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "expdate",
                "name": "EXPDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "supp.suppname", "name": "SUPP.SUPPNAME", "autoWidth": true },
            { "data": "cat.category", "name": "CAT.CATEGORY", "autoWidth": true },
            {
                "data": "lcpath",
                "name": "LCPATH",
                "autoWidth": true,
                "render": function (data, type, row) {
                    if (data !== null) {
                        var subStrings = [data.slice(0, data.lastIndexOf("_")), data.slice(data.lastIndexOf("_") + 1)];
                        return `<a target='_blank' class="fa fa-file btn-link" href='/files/lc_files/?fileName=${data}'> ${subStrings[1]}</a>`;
                    } else {
                        return "N/A";
                    }
                }
            },
            {
                "render": function (data, type, row) {
                    var editText = !row.isLocked ? "" : `<a href='/CommercialImportLC/Edit/${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a> <a href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;
                    return `<a href='/CommercialImportLC/Details/${row.encryptedId}' class='fa fa-info-circle fa-lg text-info'></a> ${editText}`;
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
                window.location = `/CommercialImportLC/Delete/${item}`;
            }
        });
}