rapidRents.services.company = rapidRents.services.company || {};

rapidRents.services.company.add = function (companyData, onSuccess, onError) {

    var url = "/api/company";
    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: companyData
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "POST"
    };
    $.ajax(url, settings);
};

rapidRents.services.company.update = function (companyId, companyData, onSuccess, onError) {
    var url = "/api/company/" + companyId
    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: companyData
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "PUT"
    };
    $.ajax(url, settings);
};

rapidRents.services.company.get = function (onSuccess, onError) {
    var url = "/api/company/";
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , success: onSuccess
            , error: onError
            , type: "GET"
    };
    $.ajax(url, settings);

}

rapidRents.services.company.getById = function (companyId, onSuccess, onError) {
    var url = "/api/company/" + companyId
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , success: onSuccess
            , error: onError
            , type: "GET"
    };
    $.ajax(url, settings);

}

rapidRents.services.company.deleteById = function (companyId, onSuccess, onError) {
    var url = "/api/company/" + companyId
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , success: function () {
                onSuccess(companyId);
            }
            , error: onError
            , type: "DELETE"
    };
    $.ajax(url, settings);
}
