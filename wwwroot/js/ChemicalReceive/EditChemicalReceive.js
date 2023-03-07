
$(function () {

    var chemrcvid = $("#FChemStoreReceiveMaster_CHEMRCVID");

  $.ajax({
    async: true,
    cache: false,
      data: { "id": chemrcvid.val() },
    type: "POST",
      url: '/FChemStoreReceive/GetPreviousChemReceiveDetailsList',
    success: function (partialView) {
      $('#chemReceiveDetailsPrevious').html(partialView);
    },
    error: function (e) {
      console.log(e);
    }
  });

});