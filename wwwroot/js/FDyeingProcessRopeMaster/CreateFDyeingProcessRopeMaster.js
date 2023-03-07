
$(function () {
    var groupId = $("#FDyeingProcessRopeMaster_GROUPID");

    groupId.select2({
        placeholder: "Select Group",
        allowClear: true,
        ajax: {
            url: "/RopeDyeing/GetGroupNumbers",
            type: "POST",
            data: function (params) {

                const formData = $("#form").serializeArray();

                formData.push({ name: "search", value: params.term });
                formData.push({ name: "page", value: params.page || 1 });

                return formData;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.plProductionPlanMasterList, function (item) {
                        return {
                            id: item.groupid,
                            text: item.grouP_NO
                        };
                    })
                };
            }
        }
    });
});