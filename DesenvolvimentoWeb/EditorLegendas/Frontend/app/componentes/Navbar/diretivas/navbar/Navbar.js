var Navbar = (function () {
    "use strict";

    function Navbar() {
        return {
            restrict: 'E',
            controller: function (servicoAutenticacao, $scope) {
            	$scope.logoff = function () {
					servicoAutenticacao.logoff();
            	}
            },
            templateUrl: 'app/componentes/Navbar/diretivas/navbar/partials/index.html'
        }
    }

    return Navbar;
})();