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
            "url": "/SampleFabric/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [0],
                "visible": false
            },
            {
                "targets": [9],
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
            { "data": "sfiid", "name": "SFIID", "autoWidth": true },
            {
                "data": "reQ_DATE", "name": "REQ_DATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "srno", "name": "SRNO", "autoWidth": true },
            {
                "data": "issuE_DATE", "name": "ISSUE_DATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "buyer.buyeR_NAME", "name": "BUYER.BUYER_NAME", "autoWidth": true,
                "render": function (data, type, row) {
                    return data == null ? "" : data;
                }
            },
            {
                "data": "brand.brandname", "name": "BRAND.BRANDNAME", "autoWidth": true,
                "render": function (data, type, row) {
                    return data == null ? "" : data;
                }
            },
            { "data": "marchandiseR_NAME", "name": "MARCHANDISER_NAME", "autoWidth": true },
            {
                "data": "mkT_TEAM.persoN_NAME", "name": "MKT_TEAM.PERSON_NAME", "autoWidth": true,
                "render": function (data, type, row) {
                    return data == null ? "" : data;
                }
            },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {

                    if (row.isDelete) {
                        return `<a href='/SampleFabric/Edit/${row.encryptedId
                            }' class='fa fa-edit fa-lg text-warning'></a> <a href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${
                            row.encryptedId}');></a>`;
                    } else {
                        return `<a href='/SampleFabric/Edit/${row.encryptedId
                            }' class='fa fa-edit fa-lg text-warning'></a>`;
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
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {

            if (isConfirm) {
                post("/SampleFabric/Delete", { sfiId: item });
            }
        });
}

function post(path, params, method = "post") {

    const form = document.createElement("form");
    form.method = method;
    form.action = path;

    for (const key in params) {
        if (params.hasOwnProperty(key)) {
            const hiddenField = document.createElement("input");
            hiddenField.type = "hidden";
            hiddenField.name = key;
            hiddenField.value = params[key];

            form.appendChild(hiddenField);
        }
    }
    document.body.appendChild(form);
    form.submit();
}