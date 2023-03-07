
var detailsList = $("#FabricIssueDetils");

$(function () {

    var teamPersonId = $("#FSampleFabric_MKT_TEAMID");
    var addPrefix = $("#SRNO_Prefix");
    var btnAdd = $("#btnAdd");

    btnAdd.on("click", function () {

        const data = $("#form").serialize();

        $.post("/SampleFabric/AddOrRemoveFromDetailsList", data, function (result) {
            detailsList.html(result);
        });
    });


    teamPersonId.on("change", function () {

        var formData = $("#form").serializeArray();

        $.post("/SampleFabric/GetSrNoPrefix", formData, function (data) {
            addPrefix.text(data);
        });

    });
});

function removeFromList(index) {

    const data = $("#form").serializeArray();

    data.push({ name: "IsDelete", value: true }, { name: "RemoveIndex", value: index });

    $.post("/SampleFabric/AddOrRemoveFromDetailsList", data, function (result) {
        detailsList.html(result);
    });
}

var updateRow = function (selector) {
    $("[type='hidden']", selector).prop("type", "text").addClass("col-xs-6 col-sm-3 mr-2");
};

function check_uncheck_checkbox(isChecked) {
    if (isChecked) {
        $('input[name="removedItem"]').each(function () {
            this.checked = true;
        });
    } else {
        $('input[name="removedItem"]').each(function () {
            this.checked = false;
        });
    }
}