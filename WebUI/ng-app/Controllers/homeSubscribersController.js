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
        $scope.isBusyF = false;
        $scope.EMAIL_REGEXP = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,5})$/;
        $scope.submitted = false;

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