
$(function () {
    $(".custom-file-input").on("change", function () {
        console.log($(this));
        var fileName = $(this).val().split("\\").pop();
        $(this).next(".custom-file-label").html(fileName);
    });
});