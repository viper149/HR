
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
            "url": "/FGsGatePass/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [12],
                "orderable": false,
                "searchable": false
            }
        ],
        language: {
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
        columns: [
            { "data": "gpno", "name": "GPNO", "autoWidth": true },
            {
                "data": "gpdate",
                "name": "GPDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "gpt.name", "name": "GPT.NAME", "autoWidth": true },
            { "data": "dept.deptname", "name": "DEPT.DEPTNAME", "autoWidth": true },
            { "data": "sec.secname", "name": "SEC.SECNAME", "autoWidth": true },
            { "data": "emp.firsT_NAME", "name": "EMP.FIRST_NAME", "autoWidth": true },
            { "data": "sendto", "name": "SENDTO", "autoWidth": true },
            { "data": "address", "name": "ADDRESS", "autoWidth": true },
            { "data": "emP_REQBYNavigation.firsT_NAME", "name": "EMP_REQBYNAVIGATION.FIRST_NAME", "autoWidth": true },
            { "data": "v.vnumber", "name": "V.VNUMBER", "autoWidth": true },
            {
                "data": "iS_RETURNABLE",
                "name": "IS_RETURNABLE",
                "autoWidth": true,
                "class": "text-center",
                "render": function (data, type, row) {
                    return data === true ? "<i class=\"text-success fa fa-check\"></i>" : "<i class=\"text-danger fa fa-times\"></i>";
                }
            },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var detailsUrl = `<a href='/FGsGatePass/DetailsFGsGatePass?gpId=${row.encryptedId
                        }' class='fa fa-info-circle fa-2x-custom text-primary'></a>`;
                    var editUrl = `<a href='/FGsGatePass/EditFGsGatePass?gpId=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;

                    return `${detailsUrl} ${editUrl}`;
                }
            }
        ]
    });
});