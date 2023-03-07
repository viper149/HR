
$(function () {

    var btnAdd = $("#btnAdd");
    var btnProgAdd = $("#btnProgAdd");
    var attachTo = $("#RndSampleInfoDetails");
    var attachProgTo = $("#ProgSetInfoDetails");
    
    $.ajax({
      async: true,
      cache: false,
      data: $("#form").serialize(),
      type: "POST",
      url: "/RndSampleInfoDyeing/GetRndSampleInfoDetailsTable",
      success: function (partialView) {
        attachTo.html(partialView);
      },
      error: function () {
        console.log("failed to attach...");
      }
    });
    
    $.ajax({
      async: true,
      cache: false,
      data: $("#form").serialize(),
      type: "POST",
      url: "/RndSampleInfoDyeing/GetProgDetailsTable",
      success: function (partialView) {
        attachProgTo.html(partialView);
      },
      error: function () {
        console.log("failed to attach...");
      }
    });

    btnAdd.on("click", function () {
        $.ajax({
            async: true,
            cache: false,
            data: $("#form").serialize(),
            type: "POST",
            url: "/RndSampleInfoDyeing/AddOrRemoveRndSampleInfoDetailsTable",
            success: function (partialView) {
                attachTo.html(partialView);
            },
            error: function () {
                console.log("failed to attach...");
            }
        });
    });

    btnProgAdd.on("click", function () {
        $.ajax({
            async: true,
            cache: false,
            data: $("#form").serialize(),
            type: "POST",
            url: "/RndSampleInfoDyeing/AddProgNoDetails",
            success: function (partialView) {
              attachProgTo.html(partialView);
            },
            error: function () {
                console.log("failed to attach...");
            }
        });
    });

    $("#PlSampleProgSetup_PROG_NO").on("change", function () {
      var insertedItem = $(this).val();
      $.ajax({
        async: true,
        cache: false,
        data: { "programNo": insertedItem},
        type: "GET",
        url: "/RndSampleInfoDyeing/GetProgramDetails",
          success: function (data) {
              if (data !== null) {
                  $("#PlSampleProgSetup_PROCESS_TYPE").val(data.procesS_TYPE);
                  $("#PlSampleProgSetup_WARP_TYPE").val(data.warP_TYPE);
                  $("#PlSampleProgSetup_TYPE").val(data.type);
              } else {
                  toastrNotification("Error: Please Enter valid Program Number", "error");
              }
          },
        error: function () {
          console.log("failed to attach...");
        }
      });
    });
});



function removeProgFromList(index) {
  var data = $("#form").serializeArray();
    data.push({ name: "removeIndexValue", value: index });
  $.ajax({
    async: true,
    cache: false,
    data: data,
    type: "POST",
    url: "/RndSampleInfoDyeing/RemoveProgNoDetails",
    success: function (partialView) {
        $("#ProgSetInfoDetails").html(partialView);
    },
    error: function (e) {
      console.log(e);
    }
  });
}

function removeFromList(index) {
  var attachTo = $("#RndSampleInfoDetails");
  var fData = $("#form").serializeArray();

  fData.push({ name: "RemoveIndex", value: index });
  fData.push({ name: "IsDelete", value: true });

  $.ajax({
    async: true,
    cache: false,
    data: fData,
    type: "POST",
    url: "/RndSampleInfoDyeing/AddOrRemoveRndSampleInfoDetailsTable",
    success: function (partialView) {
      attachTo.html(partialView);
    },
    error: function () {
      console.log("failed to attach...");
    }
  });
}