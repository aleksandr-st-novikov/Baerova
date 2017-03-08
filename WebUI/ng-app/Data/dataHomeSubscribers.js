(function () {
    'use strict';

    ngApp.factory('dataHomeSubscribers', factory);

    factory.$inject = ['$http'];

    function factory($http) {

        function getData() {
            return $http.get("/home/apigetsubscribers");
        };

        function saveSubscriber(subscriber, antiForgeryToken) {
            return $http({
                method: 'POST',
                url: '/home/apisavesubscriber',
                data: subscriber,
                headers: {
                    'RequestVerificationToken': antiForgeryToken
                }
            });
        };

        function addSubscriber(email, antiForgeryToken) {
            return $http({
                method: 'POST',
                url: '/home/apiaddsubscriber',
                data: { 'email': email },
                headers: {
                    'RequestVerificationToken': antiForgeryToken
                }
            });
        };

        function deleteSubscriber(subscriber, antiForgeryToken) {
            return $http({
                method: 'POST',
                url: '/home/apideletesubscriber',
                data: subscriber,
                headers: {
                    'RequestVerificationToken': antiForgeryToken
                }
            });
        };

        var service = {
            getData: getData,
            saveSubscriber: saveSubscriber,
            addSubscriber: addSubscriber,
            deleteSubscriber: deleteSubscriber
        };

        return service;
    }
})();