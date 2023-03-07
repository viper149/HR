"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

var messageLabel = $(".override-message-level");
var messageHistory = $("#msg_history");
var sendButton = $("#sendButton");

function start() {
    connection.start().then(function () {
        sendButton.prop("disabled", true);
        onConnect();
    }).catch(function (err) {
        onDisconnect();
        setTimeout(start, 5000);
        return console.error(err.toString());
    });
}

function onConnect() {
    messageLabel.html("Message <h6 class=\"text-info\"><i class=\"fa fa-check text-info\" aria-hidden=\"true\"></i> Connected...</h6>");
    setTimeout(function () {
        messageLabel.html("Message");
    }, 5000);
}

function onDisconnect() {
    messageLabel.html("Message <h6 class=\"text-warning\"><i class=\"fa fa-spinner fa-pulse text-danger\"></i> Disconnected... We are trying to connect you again.</h6>");
}

function onConnectOrDisconnect() {
    //
}

start();
connection.onclose(start);

connection.on("ReceiveMessage", function (userId, userName, message, sendAt, isCurrentUser, senderPhotoPath, receiverPhotoPath) {

    const photoPath = senderPhotoPath !== null ? `/Files/ProfilePhotos/?fileName=${senderPhotoPath}` : "/images/img.jpg";
    const incomingMessageInnerHtml = `<div class=\"incoming_msg\"><div class=\"incoming_msg_img\"><img alt=\"${userName}\" class=\"img-thumbnail rounded-circle\" src=\"${photoPath}\")></div><div class=\"received_msg\"><div class=\"received_withd_msg\"><div class=\"bg-dark text-light p-2 rounded\">${message}</div><span class=\"time_date\"> ${sendAt} </span></div></div></div>`;
    const outGoingMessageInnerHtml = `<div class=\"outgoing_msg\"><div class=\"sent_msg\"><div class=\"border border-dark p-2 rounded\">${message}</div> <span class=\"time_date\"> ${sendAt} </span></div></div>`;
    const container = $("#msg_history");

    $(".active_chat .chat_date").html(sendAt);
    $(".active_chat .last_text").html(message);

    $("#msg_history").append(userId === currentUserId ? outGoingMessageInnerHtml : incomingMessageInnerHtml);

    container.scrollTop(container.get(0).scrollHeight);
});

sendButton.on("click", function (event) {

    const userId = $("#userInput").val();
    const userName = $("#userInput option:selected").text();
    const messageInput = editor.getData();

    connection.invoke("SendMessage", userId, userName, messageInput).then(function () {
        editor.setData("");
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function chooseSpecificUser(userId) {
    $.post("/Home/GetSpecificUserMessage", { userId: userId }, function (partialView) {
        messageHistory.html(partialView);
    });
}

$(function () {
    var selectOption = $("#userInput");
    var recipientInputField = $("#recipient");
    var searchRecipeint = $("#searchRecipient");
    var searchRecipientResult = $("#searchRecipientResult");
    const container = $("#msg_history");

    container.scrollTop(container.get(0).scrollHeight);
    
    searchRecipeint.on("keyup", function () {
        if (searchRecipeint.val()) {
            $.get("/Home/GetUsersName/", { recipient: searchRecipeint.val() }, function (partialView) {
                searchRecipientResult.removeClass("d-none");
                searchRecipientResult.html(partialView);
            });
        } else {
            searchRecipientResult.addClass("d-none");
            searchRecipientResult.empty();
            toastr.warning("Please type a name.", "Invalid Input");
        }
    });
    
    $(".chat_list").on("click", function () {
        $(".chat_list").removeClass("active_chat");
        $(this).addClass("active_chat");

        location.reload();
    });

    selectOption.on("change", function () {
        if (selectOption.val()) {
            recipientInputField.val(selectOption.find("option:selected").text());
        } else {
            recipientInputField.val("");
        }
    });
});