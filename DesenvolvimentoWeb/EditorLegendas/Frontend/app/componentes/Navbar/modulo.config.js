(function (angular) {
    "use strict";

    angular.module('componentes.navbar', [])
    
        .directive('navbar', ['servicoAutenticacao', Navbar]);

})(angular);