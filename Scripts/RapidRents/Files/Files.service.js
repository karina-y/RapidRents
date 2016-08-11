(function () {
    "use strict";
    angular.module(APPNAME)
        .factory('$filesService', filesServiceFactory);
    filesServiceFactory.$inject = ['$baseService'];
    function filesServiceFactory($baseService) {
        var aFilesServiceObject = rapidRents.services.files;
        var newService = $baseService.merge(true, {}, aFilesServiceObject, $baseService);
        return newService;
    }
})();
