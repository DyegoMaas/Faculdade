var LoginController = (function () {
	"use strict";

	function LoginController($scope) {
		var viewModel = this;

		console.log('controller de login');

		viewModel.nomeUsuario = 'asdf';
		viewModel.senha = 'asdf';
		viewModel.autenticar = _autenticar;

		$scope.$watch(
			function() { return viewModel.senha;}, 
			function (newValue, old) {console.log(newValue, old);})

		var _autenticar = function () {
			console.log('autenticando como ',viewModel.nomeUsuario,viewModel.senha);
		};
	}

	return LoginController;
})();