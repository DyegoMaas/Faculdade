(function (angular) {
    "use strict";

    angular.module('modulo.auth', [])

		.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

            // For any unmatched url, send to /login
            $urlRouterProvider.otherwise("/login")

            $stateProvider
                .state('login', {
                    url: '/login',  
                    templateUrl: "app/modulos/Autenticacao/view/index.html",
                    controller: 'LoginController',
                    controllerAs: 'login'
                });
        }])

        .controller('LoginController', ['$scope', 'editorHttp', 'armazenadorLocal', LoginController]);

})(angular);