
$(function () {

    $("#addToUpdate").on("click", function () {

        console.log("kk");

        var formData = $("#form").serializeArray();


        $.post("/YarnReceiveForOthers/AddToList",
            formData,
            function (partialView) {
                $("#yarnReceiveForOthersDetailsTableRetrieve").html(partialView);
            }).fail(function () {

            });
    });



});

function RemoveDS(index) {
    swal({
        title: "Please Confirm",
        text: `You won't able to revert, Are you sure to remove item no. - ${index + 1}`,
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {
            if (isConfirm) {

                var formData = $('#form').serializeArray();

                formData.push({ name: "IsDelete", value: true });
                formData.push({ name: "RemoveIndex", value: index });

                $.post("/YarnReceiveForOthers/AddToList", formData, function (partialView) {
                    $("#yarnReceiveForOthersDetailsTableRetrieve").html(partialView);
                }).fail(function () {

                });
            }
        });
}