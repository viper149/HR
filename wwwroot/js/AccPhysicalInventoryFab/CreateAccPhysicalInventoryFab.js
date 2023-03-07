var rollNo = $("#AccPhysicalInventoryFab_ROLL_NO");

$(document).ready(function () {
    rollNo.val("");
    rollNo.focus();
});


let timer;
rollNo.on('input',
    code => {
        clearTimeout(timer);
        timer = setTimeout(x => {
            if (rollNo.val()) {
                var manual = $("#AccPhysicalInventoryFab_IS_MANUAL").prop('checked');
                if (manual) {
                    scanRoll(10000);
                } else {
                    scanRoll(500);
                }
            }
        }, 1000, code);
    });


function scanRoll(time) {

    setTimeout(() => {
            var viewModel = $("#form").serializeArray();
            var formData = {
                "rollNo": rollNo.val()
            };
            $.post("/AccPhysicalInventoryFab/IsReceivedOrDuplicate",
                formData,
                function (data) {
                    console.log(data);
                    if (data === true) {
                        $.post("/AccPhysicalInventoryFab/CreateAccPhysicalInventoryFab",
                            viewModel);
                        location.reload(true);
                    } else {
                        toastrNotification(data, "warning");
                        rollNo.val("");
                        rollNo.focus();
                    }
                    
                });
            
        },
        time);
}