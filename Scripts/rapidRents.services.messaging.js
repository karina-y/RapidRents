if (!rapidRents.messaging) {
    rapidRents.messaging = { services: { messages: {} } };
}

rapidRents.messaging.services.messages.post = function (messageData, onSuccess, onError) {

    var url = "/api/messages";
    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: messageData
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "POST"
    };
    $.ajax(url, settings);
};
