(function () {
    'use strict';

    ngApp.controller('homeManagePartnersController', controller);

    controller.$inject = ['$scope', 'dataHomePartners'];

    function controller($scope, dataHomePartners) {

        $scope.page = 1;
        $scope.ItemsPerPage = 20;
        $scope.maxSize = 3;
        $scope.items = [];
        $scope.isBusy = true;

        ($scope.getResultsPage = function () {
            dataHomePartners.getData().then(
                function (response) {
                    angular.copy(response.data, $scope.items);
                    $scope.displayItems = $scope.items.slice(0, $scope.ItemsPerPage);
                    $scope.isBusy = false;
                });
        })();

        $scope.pageChanged = function () {
            var startPos = ($scope.page - 1) * $scope.ItemsPerPage;
        };
    }
})();