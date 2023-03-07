

$(function () {

    var gpId = $('#FGsReturnableGpRcvM_GPID');

    gpId.on("change",
    function () {
      var selectedGpId = $(this).val();

      $.ajax({
        async: true,
        cache: false,
          data: { 'gpId': selectedGpId },
        type: "GET",
          url: '/FGsReturnGpRcv/GetGsGatePassInfoByGpId',
        success: function (data) {
          console.log(data);

          //indentPartialView.html("");
          //section.html('');
          //section.append('<option value="" selected>Select Section</option>');

          //$.each(data, function (id, option) {
          //  section.append($('<option></option>').val(option.secid).html(option.secname));
          //});
        },
        error: function (e) {
          console.log(e);
        }
      });
    });

  $("#addToList").on('click', function () {
    $.ajax({
      async: true,
      cache: false,
      data: $('#form').serialize(),
      type: "POST",
        url: '/FGsReturnGpRcv/AddToList',
      success: function (partialView) {
        console.log(partialView);
        $('#gSGatePassReturnRcvDetailsTable').html(partialView);
      },
      error: function (e) {
        console.log(e);
      }
    });
  });

});