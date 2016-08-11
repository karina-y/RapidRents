(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('messagingController', MessagingController);

    MessagingController.$inject = ['$scope', '$baseController', '$uibModal', "$messagingService"];

    function MessagingController(
        $scope
        , $baseController
        , $uibModal
        , $messagingService) {


        var vm = this;

        vm.newMessage = null;
        vm.items = null;
        vm.messagingForm = null;
        vm.showFormErrors = false;

        vm.$messagingService = $messagingService;
        vm.$scope = $scope;
        vm.$uibModal = $uibModal;

        vm.modalErrorMsg = "Something went wrong, please try again.";
        vm.openModal = _openModal;

        vm.ajaxSuccess = _ajaxSuccess;
        vm.ajaxError = _ajaxError;
        vm.submitForm = _submitForm;

        vm.header = "CONTACT US!";

        $baseController.merge(vm, $baseController);
        vm.notify = vm.$messagingService.getNotifier($scope);


        function _openModal() {
            var modalInstance = vm.$uibModal.open({
                animation: true,
                templateUrl: 'modalContent.html',
                controller: 'modalController as mc',
                windowClass: vm.modalClass,
                size: 'open',
                resolve: {
                    modalItem: function () {
                        return vm.myMsg;
                    }
                }
            });
        }


        function _ajaxSuccess(data) {
            vm.notify(function () {
                vm.items = data.items;
            });
            vm.myMsg = "Thank you for the message! A representative will contact you shortly.";
            vm.modalClass = 'notError';
            _openModal();
        }


        function _ajaxError(data, status, xhr) {
            vm.myMsg = "Something went wrong, please try again.";
            vm.modalClass = 'error';
            _openModal(vm.myMsg);
            console.log("Ajax error thrown");
        }


        function _submitForm() {
            vm.showFormErrors = true;

            if (vm.messagingForm.$valid) {
                vm.newMessage.typeId = 54;  //need to change to actual contact us type id, will be based on the page
                vm.$messagingService.post(vm.newMessage, vm.ajaxSuccess, vm.ajaxError);
            }

            else {
                vm.modalClass = 'error';
                vm.myMsg = "Something went wrong, please try again.";
                _openModal();
                console.log("Form submission is invalid.");
            }
        }
    }
})();


(function () {
    "use strict";

    angular.module(APPNAME)
    .controller('modalController', ModalController);

    ModalController.$inject = ['$scope', '$baseController', '$uibModalInstance', 'modalItem']

    function ModalController(
        $scope
        , $baseController
        , $uibModalInstance
        , modalItem
        ) {

        var vm = this;

        $baseController.merge(vm, $baseController);

        vm.$scope = $scope;
        vm.$uibModalInstance = $uibModalInstance;
        vm.modalMsg = modalItem;

        vm.ok = function () {
            vm.$uibModalInstance.close();
        };
    }
})();
