var LoginController = (function () {
	"use strict";

	function LoginController($scope, editorHttp, armazenadorLocal) {
        var viewModel = this;

        viewModel.nomeUsuario = '';
        viewModel.senha = '';

        viewModel.autenticar = function () {
            editorHttp.post('/auth', {
                nomeUsuario: viewModel.nomeUsuario,
                senha: viewModel.senha
            }).then(
                function (resultadoAutenticacao) {
                	armazenadorLocal.salvarToken(resultadoAutenticacao.token);

                	editorHttp.get('/auth/validation').then(
                		function (resultado) {
                			console.log(resultado);
                		}
            		);					
                }
            );
        };
    }

	return LoginController;
})();