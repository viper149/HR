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
            "url": "/FYsPartyInfo/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [5],
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

            { "data": "partY_NAME", "name": "PARTY_NAME", "autoWidth": true },
            { "data": "contracT_PERSON", "name": "CONTRACT_PERSON", "autoWidth": true },
            { "data": "address", "name": "ADDRESS", "autoWidth": true },
            { "data": "celL_NO", "name": "CELL_NO", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {

                    var editUrl = `<a href='/FYsPartyInfo/EditFYsPartyInfo?lbId=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a>`
                   // var deleteUrl = `<a href="/FFsWastageParty/DeleteWastageParty?id=${row.encryptedId}" class="fa fa-trash fa-2x-custom text-danger"></a>`;


                   return `${editUrl}`
                }
            }]
    });
});