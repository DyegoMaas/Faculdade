(function (angular) {
    "use strict";

    angular.module('modulo.editor', [])
        .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

            // For any unmatched url, send to /route1
            // $urlRouterProvider.otherwise("/")

            $urlRouterProvider.when('/editor', ['$state', function ($state) {
                $state.go('editor');console.log('asfadffsdf');
            }]);
            // $stateProvider.state('editor', {
            //     url: '/editor',
            //     templateUrl: 'app/modulos/Editor/view/index.html'
            // }).state('editor.listaLegendas', {
            //     templateUrl: "app/modulos/Editor/view/partials/listaLegendas.html",
            //     controller: 'ListaLegendasController',
            //     controllerAs: 'lista'
            // }).state('editor.player', { 
            //     templateUrl: "app/modulos/Editor/view/partials/player.html" 
            // }).state('editor.carregadorLegendas', { 
            //     templateUrl: "app/modulos/Editor/view/partials/carregadorLegendas.html",
            //     controller: 'CarregadorLegendasController',
            //     controllerAs: 'carregadorLegendas'
            // });
            $stateProvider.state('editor', {
                url: '/editor',
                // templateUrl: 'app/modulos/Editor/view/index.html',
                views: {
                    "topoEditor": { 
                        template: "<navbar></navbar>",
                    },
                    "listaLegendas": { 
                        templateUrl: "app/modulos/Editor/view/partials/listaLegendas.html",
                        controller: 'ListaLegendasController',
                        controllerAs: 'lista'
                    },
                    "player": { templateUrl: "app/modulos/Editor/view/partials/player.html" },
                    "carregadorLegendas": { 
                        templateUrl: "app/modulos/Editor/view/partials/carregadorLegendas.html",
                        controller: 'CarregadorLegendasController',
                        controllerAs: 'carregadorLegendas'
                    }
                }
            });
        }])

        .factory('ListaLegendas', [function (player) {
            var legendas = {};

            legendas.lista = [
                // {
                //     id:'1', 
                //     tempoInicioMs: 0, 
                //     tempoFimMs: 1000, 
                //     tempoInicio: '00:00:00.000', 
                //     tempoFim: '00:00:01.000', 
                //     texto:'lalalala'
                // }
            ];

            legendas.adicionar = function (legenda) {
                legendas.lista.push(legenda);
            };            

            return legendas;
        }])
        
        .service('Player', [
            'Cinema',
            'ListaLegendas',
            function(popcornFactory, listaLegendas) {
                return new Player(popcornFactory, listaLegendas);
            }
        ])
        .service('LegendaViewModel', [function() {
            return LegendaViewModel;
        }])
        .service('SrtSubtitleParser', ['LegendaViewModel', function (LegendaViewModel) {
            return new SrtSubtitleParser(LegendaViewModel);
        }])   

        .service('EditorLegendas', ['ListaLegendas', function (listaLegendas) {
            return new EditorLegendas(listaLegendas);
        }])
        .service('ConversorTempoLegenda', [function () {
            return new ConversorTempoLegenda();
        }])

        .service('LimiaresNovaLegenda', [function () {
            return {
                legendaAnterior: null,
                legendaPosterior: null
            }
        }])

        .directive('modalEdicaoLegenda', ['ListaLegendas', 'EditorLegendas', 'LegendaViewModel', ModalEdicaoLegenda])
        .directive('modalCriacaoLegenda', ['ListaLegendas', 'EditorLegendas', 'LegendaViewModel', 'ConversorTempoLegenda', 'LimiaresNovaLegenda', ModalCriacaoLegenda])
        
        .controller('ListaLegendasController', [
            'ListaLegendas',
            'EditorLegendas',
            'Exportador',
            'LimiaresNovaLegenda',
            ListaLegendasController
        ])
        .controller('PlayerController', [
            'Player',
            PlayerController
        ])
        .controller('CarregadorLegendasController', [
            '$timeout',
            'SrtSubtitleParser',
            'EditorLegendas',
            CarregadorLegendasController
        ]);

})(angular);