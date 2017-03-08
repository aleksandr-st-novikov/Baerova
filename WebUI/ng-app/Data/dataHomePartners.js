(function () {
    'use strict';

    ngApp.factory('dataHomePartners', factory);

    factory.$inject = ['$http'];

    function factory($http) {

        function getData() {
            return $http.get("/home/apigetpartners");
        };

        var service = {
            getData: getData
        };

        return service;
    }
})();