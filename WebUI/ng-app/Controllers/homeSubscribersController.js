(function () {
    'use strict';

    ngApp.controller('homeSubscribersController', controller);

    controller.$inject = ['$scope', 'dataHomeSubscribers', 'Notification'];

    function controller($scope, dataHomeSubscribers, Notification) {

        $scope.page = 1;
        $scope.ItemsPerPage = 20;
        $scope.maxSize = 3;
        $scope.items = [];
        $scope.isBusy = true;

        ($scope.getResultsPage = function () {
            dataHomeSubscribers.getData().then(
                function (response) {
                    angular.copy(response.data, $scope.items);
                    $scope.displayItems = $scope.items.slice(0, $scope.ItemsPerPage);
                    $scope.isBusy = false;
                });
        })();

        $scope.pageChanged = function () {
            var startPos = ($scope.page - 1) * $scope.ItemsPerPage;
        };

        $scope.saveSubscriber = function (subscriber, antiForgeryToken) {
            $scope.isBusy = true;
            dataHomeSubscribers.saveSubscriber(subscriber, antiForgeryToken).then(function () {
                $scope.isBusy = false;
                Notification.success('Сохранение успешно.');
            }, function () {
                Notification.error('Ошибка при сохранении.');
                $scope.isBusy = false;
            });
        };

        $scope.deleteSubscriber = function (subscriber, antiForgeryToken) {
            $scope.isBusy = true;
            dataHomeSubscribers.deleteSubscriber(subscriber, antiForgeryToken).then(function () {
                $scope.items.splice($scope.items.indexOf(subscriber), 1);
                $scope.isBusy = false;
                Notification.warning('Подписчик удален.');
            }, function () {
                Notification.error('Ошибка при удалении.');
                $scope.isBusy = false;
            });
        };
    }
})();