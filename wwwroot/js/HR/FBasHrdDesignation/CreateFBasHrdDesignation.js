function Reload() {
    const grade = $("#FBasHrdDesignation_GRADEID");
    const errors = {
        0: {
            title: "Reload failed.",
            message: "We cannot process your request. Please try again later."
        }
    };

    $.get("/Grade/GetGrades", (data) => {
        grade.html(`<option value="" selected>Select Grade</option>`);

        $.each(data, (id, option) => {
            const gradeOption = `<option value="${option.gradeid}"> ${option.gradE_NAME}</option>`;
            grade.append($(gradeOption));
        });
    }).fail(() => {
        toastr.error(errors[0].message, errors[0].title);
    });
}