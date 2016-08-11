(function () {
    "use strict";

    angular.module(APPNAME)
     .factory('$messagingService', MessagingServiceFactory);

    MessagingServiceFactory.$inject = ['$baseService'];

    function MessagingServiceFactory($baseService) {
        var aMsgServiceObject = rapidRents.messaging.services.messages;
        var newService = $baseService.merge(true, {}, aMsgServiceObject, $baseService);
        return newService;
    }
})();
