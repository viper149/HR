
$(function () {
    $("#table").dataTable({
        "searchPanes": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "order": [[0, "asc"]],
        "pageLength": 25,
        "lengthMenu": [10, 25, 50, 100, 200, 500, 1000],
        "autoWidth": false,
        "ajax": {
            "async": "true",
            "url": "/FFsRollReceive/GetRollTableData",
            "type": "POST",
            "datatype": "json"
        },
        "language": {
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
        "columns": [
            {
                "data": "rcv.rcvdate",
                "name": "RCV.RCVDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "fabcodeNavigation.stylE_NAME", "name": "FABCODE", "autoWidth": true },
            { "data": "sO_NONavigation.sO_NO", "name": "SO_NO", "autoWidth": true },
            { "data": "pO_NONavigation.pino", "name": "PO_NO", "autoWidth": true },
            { "data": "rolL_.rollno", "name": "ROLL_ID", "autoWidth": true },
            { "data": "rolL_.lengtH_YDS", "name": "QTY_YARDS", "autoWidth": true },
            { "data": "rolL_.remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    if (row.iS_QC_APPROVE) {
                        return `<div class="round_gol"><input type="checkbox" name="${row.encryptedId}_${row.rcv.rcvdate}" id="${row.encryptedId}_${row.rcv.rcvdate}" checked disabled/><label for="${row.encryptedId}_${row.rcv.rcvdate}"></label></div>`;
                    } else {
                        if (row.iS_QC_REJECT) {
                            return `<div class="round_gol"><input type="checkbox" name="${row.encryptedId}_${row.rcv.rcvdate}" id="${row.encryptedId}_${row.rcv.rcvdate}" disabled/><label for="${row.encryptedId}_${row.rcv.rcvdate}"></label></div>`;
                        }
                        return `<div class="round_gol"><input onclick="doApprove('${row.encryptedId}')" type="checkbox" name="${row.encryptedId}_${row.rcv.rcvdate}" id="${row.encryptedId}_${row.rcv.rcvdate}"/><label for="${row.encryptedId}_${row.rcv.rcvdate}"></label></div>`;
                    }
                }
            },
            {
                "render": function (data, type, row) {
                    if (row.iS_QC_REJECT) {
                        return ` <div class="round_gol"><input type="checkbox" name="${row.encryptedId}__${row.rcv.rcvdate}" id="${row.encryptedId}__${row.rcv.rcvdate}" checked disabled/><label for="${row.encryptedId}__${row.rcv.rcvdate}"></label></div>`;
                    } else {
                        if (row.iS_QC_APPROVE) {
                            return `<div class="round_gol"><input type="checkbox" name="${row.encryptedId}__${row.rcv.rcvdate}" id="${row.encryptedId}__${row.rcv.rcvdate}" disabled/><label for="${row.encryptedId}__${row.rcv.rcvdate}"></label></div>`;
                        }
                        return `<div class="round_gol"><input onclick="doReject('${row.encryptedId}')" type="checkbox" name="${row.encryptedId}__${row.rcv.rcvdate}" id="${row.encryptedId}__${row.rcv.rcvdate}"/><label for="${row.encryptedId}__${row.rcv.rcvdate}"></label></div>`;
                    }
                }
            }
        ]
    });
});

var doApprove = function (encryptedId) {
    if (encryptedId !== undefined) {
        $.get("/FFsRollReceive/DoApprove", { "id": encryptedId }, function (data) {
            if (data) {
                toastr.success("Approved", "Success");
                $("#table").DataTable().ajax.reload();
            } else {
                toastr.error("You Can Not Approve", "Invalid Submission");
            }
        }).fail(function () {
            toastr.error("No Access.", "Invalid Submission");
        });
    }
}

var doReject = function (encryptedId) {
    if (encryptedId !== undefined) {

        $.get("/FFsRollReceive/DoReject", { "id": encryptedId }, function (data) {
            if (data) {
                toastr.success("Approved", "Success");
                $("#table").DataTable().ajax.reload();
            } else {
                toastr.error("You Can Not Approve", "Invalid Submission");
            }
        }).fail(function () {
            toastr.error("No Access.", "Invalid Submission");
        });
    }
}