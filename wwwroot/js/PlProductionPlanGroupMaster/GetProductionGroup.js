
$(function () {
    console.log(window.location.pathname);
    $('#table').dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [[6, "asc"]],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/PlProductionPlanGroupMaster/GetTableData?path=" + window.location.pathname,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [10],
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
            { "data": "grouP_NO", "name": "GROUP_NO", "autoWidth": true },
            { "data": "optioN3", "name": "SONO", "autoWidth": true },
            { "data": "optioN4", "name": "STYLE", "autoWidth": true },
            { "data": "optioN5", "name": "BUYER", "autoWidth": true },
            { "data": "dyeinG_REFERANCE", "name": "DYEING_REFERANCE", "autoWidth": true },
            {
                "data": "seriaL_NO", "name": "SERIAL_NO", "autoWidth": true,

                "render": function (data, type, row) {
                    return row.optioN2 === "Yes" ? "" : row.seriaL_NO;
                }

            },
            { "data": "rnD_DYEING_TYPE.dtype", "name": "DYEING_TYPE", "autoWidth": true },
            { "data": "optioN1", "name": "SHADE", "autoWidth": true },
            {
                "data": "optioN2", "name": "ISDYEINGCOMPLETE", "autoWidth": true ,
                "render": function (data, type, row) {
                    return row.optioN2 === "Yes" ? `<i class="fa fa-smile-o text-success" aria-hidden="true">Yes</i>` : `<i class="fa fa-frown-o text-danger" aria-hidden="true">No</i>`;
                }
            },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    //debugger;
                    var proDateEntryLink = "";

                    //if (row.productioN_DATE === null) {
                    proDateEntryLink = ` <a class="fa fa-list-ol fa-2x-custom text-info mr-2" data-toggle="modal" data-target="#exampleModal" data-whatever="${row.groupid}"></a>`;
                    //}
                    return `<a href='/PlProductionPlanGroupMaster/DetailsProductionGroup?id=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a> <a href='/PlProductionPlanGroupMaster/EditProductionGroup?id=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning mr-2'></a><a href='/PlProductionPlanGroupMaster/DetailsProductionGroup?id=${row.encryptedId}' class='fa fa-trash fa-2x-custom text-danger mr-2'></a>` + proDateEntryLink;
                }
            }
        ]
    });

    $("#exampleModal").on('show.bs.modal',
        function (event) {
            const button = $(event.relatedTarget); // Button that triggered the modal
            const groupId = button.data('whatever'); // Extract info from data-* attributes
            // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            const modal = $(this);
            //modal.find('.modal-title').text(`Group No: ${groupId}`);
            modal.find('.modal-body #GROUPID').val(groupId);
        });
});