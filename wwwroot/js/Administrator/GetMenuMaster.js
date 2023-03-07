
$(function () {
    $("#table").dataTable({
        searchPane: true,
        processing: true,
        serverSide: true,
        filter: true,
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/Administrator/GetMenuMasterTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [6],
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
            { "data": "menuID", "name": "MenuID", "autoWidth": true },
            { "data": "menuName", "name": "MenuName", "autoWidth": true },
            { "data": "parent_MenuID", "name": "Parent_MenuID", "autoWidth": true },
            { "data": "menuFileName", "name": "MenuFileName", "autoWidth": true },
            { "data": "menuURL", "name": "MenuURL", "autoWidth": true },
            { "data": "priority", "name": "Priority", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a href='/Administrator/EditMenuMaster?menuIdentityId=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning mr-2'></a><a href='#' class='fa fa-trash fa-2x-custom text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;
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
                post("/Administrator/DeleteMenuMaster", { menuIdentityId: item });
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