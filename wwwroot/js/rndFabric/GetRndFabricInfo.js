
function deleteSwal(e, item) {
    e.preventDefault();
    swal({
            title: "Please Confirm",
            text: "Are you sure to Mute?",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes",
            cancelButtonText: "Not at all"
        },
        function (isConfirm) {
            if (isConfirm) {
                window.location = "/RndFabricInfo/DeleteRndFabricInfo?rndFabricInfoId=" + item;
            }
        });
}

$(function () {
    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        filter: true,
        pageLength: 25,
        order: [[0, "asc"]],
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/RndFabricInfo/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [8],
                "orderable": false,
                "searchable": false
            }
        ],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": '<i class="fa fa-2x fa-search" aria-hidden="true"></i>',
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
            { "data": "fabcode", "name": "FABCODE", "autoWidth": true },
            {
                "data": "stylE_NAME",
                "name": "FABCODE",
                "autoWidth": true,
                "render": function(data, type, row) {
                    //if (!row.approved) {
                    //    return "<a class='fa fa-edit fa-2x-custom text-warning editYarnConsumption' onclick=doApprove('" + row.encryptedId + "');></a>";
                    //} else {
                    //    return "<div class=\"round_gol\"><input type=\"checkbox\" name=\"" + row.encryptedId + "\" checked disabled/><label for=\"" + row.encryptedId + "\"></label></div>";
                    //}

                if (row.approved) {
                        return "  <div class=\"round_gol\"><input type=\"checkbox\" name=\"" + row.encryptedId + "\" id=\"" + row.encryptedId + "\" checked disabled/><label style='margin-left:-0.5rem;margin-top:.2rem' for=\"" + row.encryptedId + "\"></label>" + "<span class=\"text-center\">" + data + "</span></div>" + "</div>";
                    }
                    else {
                        return "  <div class=\"round_gol\"><input onclick=\"doApprove('" + row.encryptedId + "')\" type=\"checkbox\" name=\"" + row.encryptedId + "\" id=\"" + row.encryptedId + "\"/><label style='margin-left:-0.5rem;margin-top:.2rem' for=\"" + row.encryptedId + "\"></label>" + "<span class=\"text-center\">" + data + "</span></div>" + "</div>";
                    }
                }
            },
            { "data": "devid", "name": "DEVID", "autoWidth": true },
            { "data": "progno", "name": "PROGNO", "autoWidth": true },
            { "data": "d.dtype", "name": "D.DTYPE", "autoWidth": true },
            { "data": "colorcodeNavigation.color", "name": "COLORCODENavigation.COLOR", "autoWidth": true },
            { "data": "loom.looM_TYPE_NAME", "name": "LOOM.LOOM_TYPE_NAME", "autoWidth": true },
            {
                "data": "buyer.buyeR_NAME", "name": "BUYER.BUYER_NAME", "autoWidth": true,
                "render": function(data, type, row) {
                    return row.buyer === null ? "" : row.buyer.buyeR_NAME;
                }
            },
            {
                "render": function (data, type, row) {
                    
                    var proDateEntryLink = "";
                    if (row.crimP_PERCENTAGE === null) {
                        proDateEntryLink = ` <a class="fa fa-percent fa-2x-custom text-success mr-2" data-toggle="modal" data-target="#exampleModal" data-whatever="${row.fabcode}"></a>`;
                    }
                    var copyStyle = ` <a class="fa fa-clone fa-2x-custom text-success mr-2" data-toggle="modal" data-target="#exampleModalCopy" data-whatever="${row.fabcode}"></a>`;

                    if (row.approved) {
                        return `<a href='/Reports/RGisReport/?style=${row.stylE_NAME}' target='_blank' class='fa fa-print fa-2x-custom text-primary mr-2'  title='GIS of ${row.stylE_NAME}'></a >` + "<a href='/RndFabricInfo/DetailsRndFabricInfo?rndFabricInfoId=" + row.encryptedId + "' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a>" +
                            "<a href='/RndFabricInfo/DeleteRndFabricInfo/?rndFabricInfoId=" + row.encryptedId + "' class='fa fa-minus-circle fa-2x-custom text-danger'></a >" + proDateEntryLink + copyStyle;
                    } else {
                        return `<a href='/Reports/RGisReport/?style=${row.stylE_NAME}' target='_blank' class='fa fa-print fa-2x-custom text-primary mr-2'  title='GIS of ${row.stylE_NAME}'></a >`+"<a href='/RndFabricInfo/EditRndFabricInfo?rndFabricInfoId=" + row.encryptedId + "' class='fa fa-edit fa-2x-custom text-warning mr-2'></a> " +
                            "<a href='/RndFabricInfo/DetailsRndFabricInfo?rndFabricInfoId=" + row.encryptedId + "' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a>" +
                            "<a href='/RndFabricInfo/DeleteRndFabricInfo/?rndFabricInfoId=" + row.encryptedId + "' class='fa fa-minus-circle fa-2x-custom text-danger'></a >" + proDateEntryLink + copyStyle;
                        
                        //<a class='fa fa-minus-circle fa-2x-custom text-danger' onclick=deleteSwal(event, '" + row.encryptedId + "');></a >
                    }
                }
            }
        ]
    });


    $("#exampleModal").on('show.bs.modal',
        function (event) {
            const button = $(event.relatedTarget); // Button that triggered the modal
            const fabcode = button.data('whatever'); // Extract info from data-* attributes
            // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            const modal = $(this);
            //modal.find('.modal-title').text(`Group No: ${groupId}`);
            modal.find('.modal-body #FABCODE').val(fabcode);
        });

    $("#exampleModalCopy").on('show.bs.modal',
        function (event) {
            const button = $(event.relatedTarget); // Button that triggered the modal
            const fabcode = button.data('whatever'); // Extract info from data-* attributes
            // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            const modal = $(this);
            //modal.find('.modal-title').text(`Group No: ${groupId}`);
            modal.find('.modal-body #FABCODE').val(fabcode);
        });
});


function doApprove(devId) {
    if (devId !== undefined) {
        $.ajax({
            async: true,
            cache: false,
            data: {
                "devId": devId
            },
            type: "GET",
            url: "/RndFabricInfo/DoApprove",
            success: function (data) {

                if (data) {
                    toastr.success("Approved", "Success");
                } else {
                    toastr.error("You Can Not Approve", "Invalid Submission");
                }

                reloadDataTable();
            },
            error: function () {
                toastr.error("No Access.", "Invalid Submission");
            }
        });
    }
}

function reloadDataTable() {
    var myTable = $("#table");
    myTable.DataTable().ajax.reload();
}