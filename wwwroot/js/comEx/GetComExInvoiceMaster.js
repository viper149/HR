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
            "url": "/ComExInvoiceMaster/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [11],
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
            { "data": "invno", "name": "INVNO", "autoWidth": true },
            {
                "data": "invdate", "name": "INVDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "lc.lcno", "name": "LC.LCNO", "autoWidth": true },
            { "data": "lc.fileno", "name": "LC.FILENO", "autoWidth": true },
            {
                "data": "inV_QTY", "name": "INV_QTY", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? data.toString().match(/\d+(\.\d{1,2})?/g)[0] : "N/A";
                }
            },
            {
                "data": "inV_AMOUNT", "name": "INV_AMOUNT", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? data.toString().match(/\d+(\.\d{1,2})?/g)[0] : "N/A";
                }
            },
            { "data": "buyer.buyeR_NAME", "name": "BUYER_NAME", "autoWidth": true },
            { "data": "lc.coM_EX_LCDETAILS[0].pi.brand.brandname", "name": "BRANDNAME", "autoWidth": true },
            { "data": "banK_REF", "name": "BANK_REF", "autoWidth": true },
            {
                "data": "status", "name": "STATUS", "autoWidth": true,
                "render": function (data, type, row) {
                    switch (data) {
                        case "Realized":
                            return `<i class="fa fa-bullhorn text-success" aria-hidden="true"> ${data}</i>`;
                        case "Submitted":
                            return `<i class="fa fa-bullhorn text-danger" aria-hidden="true"> ${data}</i>`;
                        default:
                            return `<i class="fa fa-bullhorn text-success text-primary" aria-hidden="true"> ${data}</i>`;
                    }
                }
            },
            {
                "data": "iS_FINAL", "name": "IS_FINAL", "autoWidth": true, "class": "text-center",
                "render": function (data, type, row) {
                    return data === true ? "<i class=\"text-success fa fa-check\"></i>" : "<i class=\"text-danger fa fa-times\"></i>";
                }
            },
            {
                "render": function (data, type, row) {

                    if (row.readOnly === true) {
                        return `<a title='Details Of Invoice NO: ${row.invno}' href='/CommercialExportInvoice/Details/${row.encryptedId}' class='fa fa-info-circle fa-lg text-info'></a>`;
                    } else {
                        return `<a title='Details Of Invoice NO: ${row.invno}' href='/CommercialExportInvoice/Details/${row.encryptedId}' class='fa fa-info-circle fa-lg text-info'></a> <a title='Modify Invoice NO: ${row.invno}' href='/CommercialExportInvoice/Edit/${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a> <a title='Delete Invoice NO: ${row.invno}' href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;
                    }
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
                post("/CommercialExportInvoice/Delete", { invId: item });
            }
        });
}

function post(path, params, method = "post") {

    var form = document.createElement("form");
    form.method = method;
    form.action = path;

    for (const key in params) {
        if (params.hasOwnProperty(key)) {
            var hiddenField = document.createElement("input");
            hiddenField.type = "hidden";
            hiddenField.name = key;
            hiddenField.value = params[key];

            form.appendChild(hiddenField);
        }
    }
    document.body.appendChild(form);
    form.submit();
}