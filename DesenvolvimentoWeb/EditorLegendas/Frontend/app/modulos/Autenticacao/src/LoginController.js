var LoginController = (function () {
	"use strict";

	function LoginController() {
		var viewModel = this;

		console.log('controller de login');

		viewModel.nomeUsuario = 'asdf';
		viewModel.senha = 'asdf';
		viewModel.autenticar = _autenticar;

		var _autenticar = function () {
			console.log('autenticando como ',viewModel.nomeUsuario,viewModel.senha);
		};
	}

	return LoginController;
})();