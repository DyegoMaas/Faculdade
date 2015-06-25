var Navbar = (function () {
    "use strict";

    function Navbar() {
        return {
            restrict: 'E',
            templateUrl: 'app/componentes/Navbar/diretivas/navbar/partials/index.html',
            controller: NavbarController,
            controlleAs: 'navbar'
        }
    }

    function NavbarController(servicoAutenticacao) {
    	var vm = this;

    	vm.logoff = function () {
    		console.log('sdfsdfsdfsd');
			servicoAutenticacao.logoff();
    	};
    }

    return Navbar;
})();