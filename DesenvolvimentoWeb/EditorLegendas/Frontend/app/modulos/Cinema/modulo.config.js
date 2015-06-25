(function (angular) {
    "use strict";

    angular.module('modulo.cinema', [])
        
        .service('Cinema', [function () {
            return new Cinema();
        }]);

})(angular);