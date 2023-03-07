
$(function () {

    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        order: [0, 'desc'],
        filter: true,
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/CommercialExport/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
                "targets": [10],
                "orderable": false,
                "searchable": false
            }],
        language: {
            "searchPlaceholder": "search by date, date-range, fields values, etc",
            "search": '<i class="fa fa-2x fa fa-print text-primary" aria-hidden="true" onclick="ShowReportsForThis()"></i> <i class="fa fa-2x fa fa-search-plus" aria-hidden="true"></i>',
            "emptyTable": "<h3>No data available</h3>",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoFiltered": "(filtered from _MAX_ total entries)",
            "thousands": ",",
            "processing": "<i class=\"fa fa-refresh fa-spin fa-3x fa-fw text-danger\"></i>" + "<br />Loading...",
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
            { "data": "piid", "name": "PIID", "autoWidth": true, "visible": false },
            { "data": "pino", "name": "PINO", "autoWidth": true },
            {
                "data": "pidate",
                "name": "PIDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return moment(data).format(row.dateFormat);
                }
            },
            { "data": "lcNoPi", "name": "LcNoPi", "autoWidth": true },
            { "data": "fileNo", "name": "FileNo", "autoWidth": true },
            { "data": "pI_QTY", "name": "PI_QTY", "autoWidth": true },
            {
                "data": "pI_TOTAL_VALUE", "name": "PI_TOTAL_VALUE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data != null ? data.toString().match(/\d+(\.\d{1,2})?/g)[0] : data;
                }
            },
            { "data": "buyer.buyeR_NAME", "name": "BUYER.BUYER_NAME", "autoWidth": true, "defaultContent": "N/A" },
            { "data": "bank.beN_BANK", "name": "BANK.BEN_BANK", "autoWidth": true, "defaultContent": "N/A" },
            { "data": "personMktTeam.persoN_NAME", "name": "PersonMktTeam.PERSON_NAME", "autoWidth": true, "defaultContent": "N/A" },
            {
                "render": function (data, type, row) {
                    var poRptUrl =
                        `<a target='_blank' title='Production Order' href='/CommercialExport/ProductionOrder/${
                        row.encryptedId}' class='fa fa-print fa-lg text-primary'>PO</a>`;
                    var piRptUrl =
                        `<a target='_blank' title='Proforma Invoice' href='/CommercialExport/ProformaInvoice/${
                        row.encryptedId}' class='fa fa-print fa-lg text-primary'>PI</a>`;
                    var fpiRptUrl =
                        `<a target='_blank' title='Foreign Proforma Invoice' href='/CommercialExport/FrnProformaInvoice/${row.encryptedId}' class='fa fa-print fa-lg text-primary'>FPI</a>`;
                    var detailsUrl =
                        `<a title='Details Of PI NO: ${row.pino}' href='/CommercialExport/Details/${row.encryptedId
                        }' class='fa fa-info-circle fa-lg text-info'></a>`;
                    var editUrl =
                        `<a title='Modify PI NO: ${row.pino}' href='/CommercialExport/Edit/${row.encryptedId
                        }' class='fa fa-edit fa-lg text-warning'></a>`;
                    var deleteUrl =
                        `<a title='Delete PI NO: ${row.pino
                            }' href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row
                            .encryptedId}');></a>`;
                    var locked =
                        `<i title="This item is locked." class="fa fa-lock fa-2x-custom text-danger" aria-hidden="true"></i>`;

                    if (row.noN_EDITABLE === true || row.readOnly === true) {
                        return `${poRptUrl} ${piRptUrl} ${fpiRptUrl} ${detailsUrl} ${locked}`;
                    }
                    else {
                        return `${poRptUrl} ${piRptUrl} ${fpiRptUrl} ${detailsUrl} ${editUrl}`;
                    }
                }
            }
        ]
    });
});
//    filterStrings.on("change", function () {
//        table.DataTable().ajax.reload();
//    });

//    table.on('select deselect draw', function () {
//        var all = table.api().rows({ search: 'applied' }).count(); // get total count of rows
//        var selectedRows = table.rows({ selected: true, search: 'applied' }).count(); // get total count of selected rows

//        if (selectedRows < all) {
//            $('#MyTableCheckAllButton i').attr('class', 'fa fa-square');
//        } else {
//            $('#MyTableCheckAllButton i').attr('class', 'fa fa-check-square');
//        }

//    });

//    $('#MyTableCheckAllButton').click(function () {
//        var all = table.api().rows({ search: 'applied' }).count(); // get total count of rows
//        var selectedRows = table.api().rows({ selected: true, search: 'applied' }).count(); // get total count of selected rows


//        if (selectedRows < all) {
//            //Added search applied in case user wants the search items will be selected
//            table.api().rows({ search: 'applied' }).deselect();
//            table.api().rows({ search: 'applied' }).select();
//        } else {
//            table.api().rows({ search: 'applied' }).deselect();
//        }
//    });

//});

//function ShowReportsForThis(value) {

//    var searchResult = $("#table_filter input").val();

//    if (searchResult) {
//        window.open(`/CommercialExport/AuditSummary?searchValue=${searchResult}`, "_blank");
//    }
//}

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
                window.location = `/ComExPiMaster/DeletePIInfo?piId=${item}`;
            }
        });
}