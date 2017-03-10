(function () {
    'use strict';

    ngApp.controller('homeSubscriberController', controller);

    controller.$inject = ['$scope', 'dataHomeSubscribers', 'Notification'];

    function controller($scope, dataHomeSubscribers, Notification) {

        $scope.isBusyF = false;
        $scope.submitted = false;
        $scope.EMAIL_REGEXP = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,5})$/;

        $scope.submitAddSubscriberForm = function (antiForgeryToken) {
            if ($scope.AddSubscriberForm.$invalid) {
                $scope.submitted = true;
                return;
            };
            $scope.submitted = true;
            $scope.isBusyF = true;
            dataHomeSubscribers.addSubscriber($scope.email, antiForgeryToken).then(
                function () {
                    // on success
                    $scope.isBusyF = false;
                    $scope.submitted = false;
                    $scope.email = '';
                    Notification.success({ message: 'В ближайшее время Вы начнете получать наши новости.', title: 'Спасибо за подписку!', delay: 10000 });
                }, function () {
                    $scope.submitted = false;
                    $scope.isBusyF = false;
                    Notification.error('Ошибка при сохранении.');
                });
        };
    }
})();