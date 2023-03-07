
function emptyElements(inputArray = []) {
    $.each(inputArray, function (key, value) {
        value.empty();
    });
}

function resetFields(inputArray = []) {
    $.each(inputArray, function (key, value) {
        value.val("");
        value.val("").trigger("change");
    });
}

function addClass(inputArray = [], name = "border border-info") {
    $.each(inputArray, function (key, value) {
        value.addClass(name);
    });
}

function removeClass(inputArray = [], name = "border border-info") {
    $.each(inputArray, function (key, value) {
        value.removeClass(name);
    });
}

function checkErrors(inputArray = []) {
    var count = 0;
    $.each(inputArray, function (key, value) {
        if (!value.val()) {
            const response = $(`label[for="${value.attr("id")}"]`).html();
            if (response === undefined) {
                toastr.warning($(`label[for="${value.parent().find("label").attr("for")}"]`).html() + " Field Can Not Be Empty.", "Invalid Input");
            } else {
                toastr.warning(`${response} Field Can Not Be Empty.`, "Invalid Input");
            }
            count += 1;
        }
    });

    if (count > 0) {
        return false;
    } else {
        return true;
    }
}