
$(function () {

    const departmentId = $("#FHrEmpOfficialInfo_DEPTID");
    const btnAddEducation = $("#btnAddEdu");
    const btnAddFamilyDetails = $("#btnAddFamDetails");
    const profilePhoto = $("#FHrEmployee_IMAGE");
    const joiningSalary = $("#FHrEmpSalarySetup_JOINING_SALARY");
    const presentSalary = $("#FHrEmpSalarySetup_PRESENT_SALARY");
    const basicSalary = $("#FHrEmpSalarySetup_BASIC");

    var sectionId = $("#FHrEmpOfficialInfo_SECID");
    var sSectionId = $("#FHrEmpOfficialInfo_SSECID");
    var perDistrict = $("#FHrEmployee_PER_DISTRICT");
    var perDistrictBengali = $("#FHrEmployee_PER_DISTRICT_BNG");
    var sameAsPresentAddress = $("#FHrEmployee_SAME_AS_PRESENT_ADDRESS");
    var perPoliceStation = $("#FHrEmployee_PER_THANA");
    var perPoliceStationBengali = $("#FHrEmployee_PER_THANA_BNG");
    var presentAddress = $("#FHrEmployee_PRE_DISTRICT");
    var presentAddressBengali = $("#FHrEmployee_PRE_DISTRICT_BNG");
    var presentAddressPoliceStation = $("#FHrEmployee_PRE_THANA");
    var presentAddressPoliceStationBengali = $("#FHrEmployee_PRE_THANA_BNG");
    var educationDetails = $("#eduDetails");
    var familyDetails = $("#familyDetails");
    var profilePhotoPreview = $("#employeeProfilePicture");
    var permanentAddressPostOffice = $("#FHrEmployee_PER_PO");
    var permanentAddressPostOfficeBengali = $("#FHrEmployee_PER_PO_BNG");
    var presentAddressPostOffice = $("#FHrEmployee_PRE_PO");
    var presentAddressPostOfficeBengali = $("#FHrEmployee_PRE_PO_BNG");
    var permanentAddressVillage = $("#FHrEmployee_PER_VILLAGE");
    var presentAddressVillage = $("#FHrEmployee_PRE_VILLAGE");
    var permanentAddressVillageBengali = $("#FHrEmployee_PER_VILLAGE_BNG");
    var presentAddressVillageBengali = $("#FHrEmployee_PRE_VILLAGE_BNG");

    joiningSalary.on("keyup", function () {
        presentSalary.val($(this).val());
        basicSalary.val($(this).val() * 0.6).trigger("change");
    });

    presentSalary.on("change keyup", function () {
        basicSalary.val($(this).val() * 0.6);
    });

    departmentId.on("change", function () {

        $.get(`/FHrEmployee/GetSectionByDepartmentId?id=${$(this).val()}`, function (data) {
            sectionId.html("");
            sectionId.append('<option value="" selected>Select Section</option>');
            $.each(data.f_BAS_SECTION, function (id, option) {
                sectionId.append($("<option></option>").val(option.secid).html(option.secname));
            });
        });
    });

    sectionId.on("change", function () {

        $.get(`/FHrEmployee/GetSubSectionBySectionId?id=${$(this).val()}`, function (data) {
            sSectionId.html("");
            sSectionId.append('<option value="" selected>Select Sub-Section</option>');

            $.each(data.f_BAS_SUBSECTION, function (id, option) {
                sSectionId.append($("<option></option>").val(option.ssecid).html(option.ssecname));
            });
        });
    });

    perDistrict.on("change", function () {
        perDistrictBengali.val($(this).val()).trigger("change");

        if (sameAsPresentAddress.val()) {
            return false;
        }

        $.get(`/FHrEmployee/GetThanaByDistrictId?id=${$(this).val()}`, function (data) {
            perPoliceStation.html("");
            perPoliceStation.append('<option value="" selected>Select Police Station</option>');

            $.each(data, function (id, option) {
                perPoliceStation.append($("<option></option>").val(option.id).html(option.name));
            });

            perPoliceStationBengali.html("");
            perPoliceStationBengali.append('<option value="" selected>থানা নির্বাচন করুন</option>');

            $.each(data, function (id, option) {
                perPoliceStationBengali.append($("<option></option>").val(option.id).html(option.bN_NAME));
            });
        });

        return true;
    });

    presentAddress.on("change", function () {

        presentAddressBengali.val($(this).val()).trigger("change");

        $.get(`/FHrEmployee/GetThanaByDistrictId?id=${$(this).val()}`, function (data) {
            presentAddressPoliceStation.html("");
            presentAddressPoliceStation.append('<option value="" selected>Select Police Station</option>');

            $.each(data, function (id, option) {
                presentAddressPoliceStation.append($("<option></option>").val(option.id).html(option.name));
            });

            presentAddressPoliceStationBengali.html("");
            presentAddressPoliceStationBengali.append('<option value="" selected>থানা নির্বাচন করুন</option>');

            $.each(data, function (id, option) {
                presentAddressPoliceStationBengali.append($("<option></option>").val(option.id).html(option.bN_NAME));
            });
        });
    });

    presentAddressPoliceStation.on("change", function () {
        presentAddressPoliceStationBengali.val($(this).val()).trigger("change");
    });

    perPoliceStation.on("change", function () {
        perPoliceStationBengali.val($(this).val()).trigger("change");
    });

    btnAddEducation.on("click", function () {

        const data = $("#form").serialize();

        $.post("/FHrEmployee/AddOrRemoveEduDetails", data, function (partialView) {
            educationDetails.html(partialView);
        });
    });

    btnAddFamilyDetails.on("click", function () {

        const data = $("#form").serializeArray();

        $.post("/FHrEmployee/AddOrRemoveFamilyDetails", data, function (partialView) {
            familyDetails.html(partialView);
        });
    });

    profilePhoto.change(function () {
        profilePhotoPreview.html("");
        const regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
        if (regex.test($(this).val().toLowerCase())) {

            if (typeof (FileReader) != "undefined") {
                profilePhotoPreview.show();
                // $("#employeeProfilePicture").append("<img />");
                const reader = new FileReader();
                reader.onload = function (e) {
                    profilePhotoPreview.attr("src", e.target.result);
                    //debugger;
                    const arrayBuffer = this.result;
                    const array = new Uint8Array(arrayBuffer);
                    const binaryString = String.fromCharCode.apply(null, array);

                    //alert(arrayBuffer);
                    // console.log(binaryString);
                    // document.getElementById("byteArray").innerHTML = binaryString;
                }
                reader.readAsDataURL($(this)[0].files[0]);
            } else {
                toastr.warning("This browser does not support FileReader.", "Not Supported!");
            }

        } else {
            toastr.warning("Please upload a valid image file.", "Invalid File!");
        }
    });

    sameAsPresentAddress.on("change", function () {
        if (this.checked) {
            perDistrict.prop("disabled", true);
            perPoliceStation.prop("disabled", true);
            perDistrict.val(presentAddress.val()).trigger("change");
            perDistrictBengali.val(presentAddressBengali.val()).trigger("change");
            perPoliceStation.val(presentAddressPoliceStation.val()).trigger("change");
            perPoliceStationBengali.val(presentAddressPoliceStationBengali.val()).trigger("change");
            permanentAddressPostOffice.val(presentAddressPostOffice.val()).trigger("change");
            permanentAddressPostOfficeBengali.val(presentAddressPostOfficeBengali.val()).trigger("change");
            permanentAddressVillage.val(presentAddressVillage.val()).trigger("change");
            permanentAddressVillageBengali.val(presentAddressVillageBengali.val()).trigger("change");
        } else {
            perDistrict.prop("disabled", false);
            perPoliceStation.prop("disabled", false);
            perDistrict.val("").trigger("change");
            perDistrictBengali.val("").trigger("change");
            perPoliceStation.val("").trigger("change");
            perPoliceStationBengali.val("").trigger("change");
            permanentAddressPostOffice.val("").trigger("change");
            permanentAddressPostOfficeBengali.val("").trigger("change");
            permanentAddressVillage.val("").trigger("change");
            permanentAddressVillageBengali.val("").trigger("change");
        }
    });
});