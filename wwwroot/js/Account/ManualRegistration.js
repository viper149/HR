var selectItem = $("#revealPassword");
var password = $("#Password");

selectItem.on("click", function () {
    password.get(0).type = $(this).is(":checked") ? "text" : "password";
});