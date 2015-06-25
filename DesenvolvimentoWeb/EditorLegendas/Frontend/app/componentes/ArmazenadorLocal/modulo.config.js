(function (angular) {
    "use strict";

    angular.module('componentes.armazenadorLocal', [])
        .service('storage', [Storage])
        .service('armazenadorLocal', ['storage', ArmazenadorLocal]);

})(angular);