var LoginController = (function () {
	"use strict";

	function LoginController($scope, $state, servicoAutenticacao, editorHttp) {
        var viewModel = this;

        viewModel.nomeUsuario = '';
        viewModel.senha = '';

        viewModel.autenticar = function () {

            servicoAutenticacao.login(viewModel.nomeUsuario, viewModel.senha).then(
                function (resultadoAutenticacao) {
                	$state.go('editor');
                	
              //   	editorHttp.get('/auth/validation').then(
              //   		function (resultado) {
                			
              //   		}
            		// );					
                }
            );
        };

        viewModel.logoff = function () {
        	debugger;
        	servicoAutenticacao.logoff();
        };
    }

	return LoginController;
})();