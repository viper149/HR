
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
            "url": "/ChemicalRequisition/GetTableData",
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
            {
                "data": "indsldate", "name": "INDSLDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "employee.firsT_NAME", "name": "Employee.FIRST_NAME", "autoWidth": true },
            { "data": "fBasDepartment.deptname", "name": "FBasDepartment.DEPTNAME", "autoWidth": true },
            { "data": "fBasSection.secname", "name": "FBasSection.SECNAME", "autoWidth": true },
            {
                "data": "fBasSubsection.ssecname", "name": "FBasSubSection.SSECNAME", "autoWidth": true,
                "render": function (data, type, row) {
                    return data === null ? "N/A" : data;
                }
            },
            { "data": "concernEmployee.firsT_NAME", "name": "ConcernEmployee.FIRST_NAME", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            { "data": "status", "name": "STATUS", "autoWidth": true },
            {
                "render": function (data, type, row) {

                    var detailsUrl = `<a href='/ChemicalRequisition/Details/${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-primary'></a>`;
                    var editUrl = `<a href='/ChemicalRequisition/Edit/${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                    var lock = `<i title="This item is locked." class="fa fa-lock fa-2x-custom text-danger" aria-hidden="true"></i>`;
                    var reportUrl = `<a target='_blank' title='Report Of Requisition No: ${row.indslid}' href='/Reports/ChemicalPurReqRpt/${row.indslid}' class='fa fa-print fa-lg text-primary'></a>`;

                    return row.isLocked ? `${detailsUrl} ${lock} ${reportUrl}` : `${detailsUrl} ${editUrl} ${reportUrl}`;
                }
            }
        ]
    });
});