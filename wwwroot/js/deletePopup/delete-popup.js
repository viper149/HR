

function deleteSwal(e, returnUrl) {

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
            window.location = returnUrl;
        }
    });
}
