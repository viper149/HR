function Reload() {
    const loc = $("#FBasHrdDepartment_LOCATIONID");
    const errors = {
        0: {
            title: "Reload failed.",
            message: "We cannot process your request. Please try again later."
        }
    };

    $.get("/Location/GetLocations", (data) => {
        loc.html(`<option value="" selected>Select Location</option>`);

        $.each(data, (id, option) => {
            const locOption = `<option value="${option.locid}"> ${option.loC_NAME}</option>`;
            loc.append($(locOption));
        });
    }).fail(() => {
        toastr.error(errors[0].message, errors[0].title);
    });
}