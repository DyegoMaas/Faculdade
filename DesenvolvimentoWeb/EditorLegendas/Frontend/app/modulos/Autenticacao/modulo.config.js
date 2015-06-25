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

                    //FUNCIONA
                    // controller: function ($scope) {
                    //     console.log('lalala');
                    //     this.nomeUsuario = 'titia';
                    //     this.senha = '123';

                    //     this.autenticar = function() {
                    //         console.log('hohoho');
                    //     }
                    // },

                    controller: function ($scope, editorHttp) {
                        var viewModel = this;

                        viewModel.nomeUsuario = 'admin';
                        viewModel.senha = 'admin';

                        viewModel.autenticar = function () {
                            editorHttp.post('/auth', {
                                nomeUsuario: viewModel.nomeUsuario,
                                senha: viewModel.senha
                            }).then(
                                function (resultado) {
                                    console.log(resultado)
                                }
                            );
                        };

                        // $scope.$watch(
                        //     function() { return viewModel.senha;}, 
                        //     function (newValue, old) {console.log(newValue, old);})

                        // var _autenticar = function () {
                        //     console.log('autenticando como ',viewModel.nomeUsuario,viewModel.senha);
                        // };
                    },
                    controllerAs: 'login'

                    // POR QUE NÃO FUNCIONA
                    // controller: LoginController,
                    // controllerAs: 'login'
                });
        }])

        // .controller('LoginController', ['$scope', LoginController])
        ;

})(angular);