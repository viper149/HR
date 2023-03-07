
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
            "url": "/ChemicalRequirement/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [7],
            "orderable": false,
            "searchable": false
        }, {
            "targets": [0],
            "searchable": false,
            "visible":false
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
            { "data": "csrid", "name": "CSRID", "autoWidth": true },
            { "data": "csrno", "name": "CSRNO", "autoWidth": true },
            {
                "data": "csrdate", "name": "CSRDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "dept.deptname", "name": "DEPT.DEPTNAME", "autoWidth": true },
            { "data": "fBasSection.secname", "name": "FBasSection.SECNAME", "autoWidth": true },
            { "data": "fBasSubsection.ssecname", "name": "FBasSubsection.SSECNAME", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {

                    var detailsUrl = `<a target='_blank' title='Print Store Purchase Requirement' href='/Reports/RChemRequirmentReport?reqNo=${row.csrno}' class='fa fa-print fa-lg text-primary'></a> <a href='/ChemicalRequirement/Details/${row.encryptedId}' class='fa fa-info-circle fa-lg text-primary'></a>`;
                    var editUrl = `<a href='/ChemicalRequirement/Edit/${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a>`;

                    return !row.isLocked ? `${detailsUrl} <i title="This item is locked." class="fa fa-lock fa-2x-custom text-danger" aria-hidden="true"></i>` : `${detailsUrl} ${editUrl}`;
                }
            }
        ]
    });
});