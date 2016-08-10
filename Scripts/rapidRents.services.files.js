rapidRents.services.files = rapidRents.services.files || {};

rapidRents.services.files.post = function (fileData, onSuccess, onError) {
    var url = "/api/files/upload"
    var settings = {
        cache: false
        , contentType: false
        , data: fileData
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "POST"
        , processData: false
    };
    $.ajax(url, settings);
};

rapidRents.services.files.update = function (fileId, fileData, onSuccess, onError) {
    var url = "/api/files/" + fileId;
    var settings = {
        cache: false
       , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
       , data: fileData
       , dataType: "json"
       , success: onSuccess
       , error: onError
       , type: "PUT"
    }
    $.ajax(url, settings);
}

rapidRents.services.files.get = function (onSuccess, onError) {
    var url = "/api/files";
    var settings = {
        cache: false
       , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
       , dataType: "json"
       , success: onSuccess
       , error: onError
       , type: "GET"
    }
    $.ajax(url, settings);
}

rapidRents.services.files.getById = function (fileServiceData, onSuccess, onError) {
    var url = "/api/files/" + fileServiceData;
    var settings = {
        cache: false
        , contentType: "application/x-wwwa-form-urlencoded; charset=UTF-8"
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "GET"
    };
    $.ajax(url, settings);
}

rapidRents.services.files.deleteById = function (deleteFileId, onSuccess, onError) {
    var url = "/api/files/" + deleteFileId;
    var settings = {
        cache: false
       , contentType: false
       , dataType: "json"
       , success: onSuccess
       , error: onError
       , type: "DELETE"
    };
    $.ajax(url, settings);
}

rapidRents.services.files.uploadProfilePhoto = function (fileData, onSuccess, onError) {
    var url = "/api/files/avatar/upload"
    var settings = {
        cache: false
        , contentType: false
        , data: fileData
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "Put"
        , processData: false
    };
    $.ajax(url, settings);
};
