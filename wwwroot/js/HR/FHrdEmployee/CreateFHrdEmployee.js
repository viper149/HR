const eduDetails = $("#eduDetailsTable");
const spouseDetails = $("#spouseDetailsTable");
const errors = {
    0: {
        title: "Invalid Submission.",
        message: "We cannot process your request. Please try again later."
    },
    1: {
        title: "Reload failed."
    },
    2: {
        title: "Bank Salary can't be greater than Present Salary",
        message: "Please decrease Bank salary."
    },
    3: {
        title: "Degree must be selected.",
        message: "Please select Degree."
    },
    4: {
        title: "Group/Subject can't be Empty",
        message: "Please Input."
    },
    5: {
        title: "Board/University can't be Empty"
    },
    6: {
        title: "Passing Date can't be Empty"
    },
    7: {
        title: "CGPA/out of can't be Empty"
    }
};

$(function () {
    const nationality = $("#FHrdEmployee_NATIONALITY_ID");
    const divPre = $("#FHrdEmployee_THANAID_PRE");
    const divPer = $("#FHrdEmployee_THANAID_PER");
    const distPre = $("#FHrdEmployee_DISTID_PRE");
    const distPer = $("#FHrdEmployee_DISTID_PER");
    const thanaPre = $("#FHrdEmployee_THANAID_PRE");
    const thanaPer = $("#FHrdEmployee_THANAID_PER");
    const unionPre = $("#FHrdEmployee_UNIONID_PRE");
    const unionPer = $("#FHrdEmployee_UNIONID_PER");
    const salJoin = $("#FHrdEmployee_SALARY_JOINING");
    const salPre = $("#FHrdEmployee_SALARY_PRE");
    const salBank = $("#FHrdEmployee_SALARY_BANK");
    const salCash = $("#FHrdEmployee_SALARY_CASH");
    const grad = $("#FHrdEducation_IS_GRADUATE");
    const resultFields = $(".resultField");
    const btnAddEdu = $("#btnAddEdu");
    const btnAddSpouse = $("#btnAddSpouse");

    nationality.on("change", () => {
        loadData("Division", "/Employee/GetDivisions", divPre, "divid", "diV_NAME", { nationId: nationality.val() });
        loadData("Division", "/Employee/GetDivisions", divPer, "divid", "diV_NAME", { nationId: nationality.val() });
    });

    divPre.on("change", () => {
        loadData("District", "/Employee/GetDistricts", distPre, "distid", "disT_NAME", { divId: divPre.val() });
    });

    divPer.on("change", () => {
        loadData("District", "/Employee/GetDistricts", distPer, "distid", "disT_NAME", { divId: divPer.val() });
    });

    distPre.on("change", () => {
        loadData("Thana/Upazila", "/Employee/GetThana", thanaPre, "thanaid", "thanA_NAME", { distId: distPre.val() });
    });

    distPer.on("change", () => {
        loadData("Thana/Upazila", "/Employee/GetThana", thanaPer, "thanaid", "thanA_NAME", { distId: distPer.val() });
    });

    thanaPre.on("change", () => {
        loadData("Union/Municipality", "/Employee/GetUnion", unionPre, "unionid", "unioN_NAME", { thanaId: thanaPre.val() });
    });

    thanaPer.on("change", () => {
        loadData("Union/Municipality", "/Employee/GetUnion", unionPer, "unionid", "unioN_NAME", { thanaId: thanaPer.val() });
    });

    salJoin.on("change", () => {
        if (!salPre.val()) {
            salPre.val(salJoin.val());
        }
    });

    salBank.on("change", () => {
        var result = salPre.val() - salBank.val();
        result >= 0 ? salCash.val(result) : toastr.error(errors[2].message, errors[2].title);
    });

    grad.on("change", function () {
        $(this).is(":checked") ? resultFields.removeClass("d-none") : resultFields.addClass("d-none");
    });

    btnAddEdu.on("click", function () {
        const degree = $("#FHrdEducation_DEGID");
        const subject = $("#FHrdEducation_MAJOR");
        const board = $("#FHrdEducation_BOARD_UNI");
        const passDate = $("#FHrdEducation_PASS_DATE");
        const cgpa = $("#FHrdEducation_CGPA");
        const outOf = $("#FHrdEducation_OUTOF");

        if (degree[0].selectedIndex <= 0) {
            toastr.error(errors[3].message, errors[3].title);
            return false;
        }
        if (!subject.val()) {
            toastr.error(errors[4].message, errors[4].title);
            return false;
        }
        if (!board.val()) {
            toastr.error(errors[4].message, errors[5].title);
            return false;
        }
        if (grad.is(":checked")) {
            if (!passDate.val()) {
                toastr.error(errors[4].message, errors[6].title);
                return false;
            }
            if (!cgpa.val()) {
                toastr.error(errors[4].message, errors[7].title);
                return false;
            }
            if (!outOf.val()) {
                toastr.error(errors[4].message, errors[7].title);
                return false;
            }
        }

        $.post("/Employee/AddOrRemoveEducation", $("#form").serializeArray(), function (partialView) {
            eduDetails.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });

    btnAddSpouse.on("click", function () {
        var formData = $("#form").serializeArray();

        $.post("/Employee/AddOrRemoveSpouse", formData, function (partialView) {
            spouseDetails.html(partialView);
        }).fail(function () {
            toastr.error(errors[0].title, errors[0].message);
        });
    });
});

function Reload(item) {
    const dept = $("#FHrdEmployee_DEPTID");
    const sec = $("#FHrdEmployee_SECID");
    const ssec = $("#FHrdEmployee_SUBSECID");
    const des = $("#FHrdEmployee_DESIGID");
    const bank = $("#FHrdEmployee_BANKID");
    const empType = $("#FHrdEmployee_EMPTYPEID");
    const degree = $("#FHrdEducation_DEGID");

    switch (item) {
        case 'Department':
            loadData(item, "/Department/GetDepartments", dept, "deptid", "deptname");
            break;

        case 'Section':
            loadData(item, "/Section/GetSections", sec, "secid", "seC_NAME");
            break;

        case 'Sub-Section':
            loadData(item, "/Sub-Section/GetSubSections", ssec, "subsecid", "subseC_NAME");
            break;

        case 'Designation':
            loadData(item, "/Designation/GetDesignations", des, "desid", "deS_NAME");
            break;

        case 'Bank Name (Beneficiary)':
            loadData(item, "/BeneficiaryBank/GetBenBanks", bank, "bankid", "beN_BANK");
            break;

        case 'Employee Type':
            loadData(item, "/EmployeeType/GetEmpTypes", empType, "typeid", "typE_NAME");
            break;

        case 'Degree':
            loadData(item, "/EduDegree/GetEduDegrees", degree, "degid", "degname");
            break;

        default:
            toastr.error(errors[0].message, errors[1].title);
    }
};

function loadData(initText, url, selectElement, optionValueProp, optionTextProp, parameter = null) {

    $.get(url, parameter, (data) => {
        selectElement.html(`<option value="" selected>Select ${initText}</option>`);
        console.log(data);

        $.each(data, (id, option) => {
            const optionValue = option[optionValueProp];
            const optionText = option[optionTextProp];
            const optionHtml = `<option value="${optionValue}">${optionText}</option>`;
            selectElement.append($(optionHtml));
        });
    }).fail(() => {
        toastr.error(errors[0].message, errors[1].title);
    });
};

function RemoveEdu(index) {
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

                $.post("/Employee/AddOrRemoveEducation", formData, function (partialView) {
                    eduDetails.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
};

function RemoveSpouse(index) {
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

                $.post("/Employee/AddOrRemoveSpouse", formData, function (partialView) {
                    spouseDetails.html(partialView);
                }).fail(function () {
                    toastr.error(errors[0].title, errors[0].message);
                });
            }
        });
};