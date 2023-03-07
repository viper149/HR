
$(function (e){
    var deleteSweetalert = function (e) {
        e.preventDefault();

        swal({
            title: 'Are you sure to delete?',
            text: "You won't be able to revert this!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        },
            function (isConfirm) {
                if (isConfirm) {
                    //user selected "Yes", Let's submit the form
                    return true;
                }
            }
        );
    };
});