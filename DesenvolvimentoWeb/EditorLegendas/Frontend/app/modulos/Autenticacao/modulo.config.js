(function (angular) {
    "use strict";

    angular.module('modulo.auth', [])

		.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

            // For any unmatched url, send to /login
            // $urlRouterProvider.otherwise("/login"); //estava fazendo com que sempre voltasse para o login.

            $stateProvider
                .state('login', {
                    url: '/login',  
                    templateUrl: "app/modulos/Autenticacao/view/index.html",
                    controller: 'LoginController',
                    controllerAs: 'login'
                });
        }])
        .service('servicoAutenticacao', ['editorHttp', 'armazenadorLocal', ServicoAutenticacao])
        .controller('LoginController', ['$scope', '$state', 'servicoAutenticacao', 'editorHttp', LoginController]);

})(angular);