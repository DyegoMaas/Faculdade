var ServicoAutenticacao = (function () {
	"use strict";

	function ServicoAutenticacao(editorHttp, armazenadorLocal) {
		this._editorHttp = editorHttp;
		this._armazenadorLocal = armazenadorLocal;
	}

	ServicoAutenticacao.prototype = {
		login: function (nomeUsuario, senha) {
			return this._editorHttp.post('/auth', {
                nomeUsuario: nomeUsuario,
                senha: senha
            }).then(
                function (resultadoAutenticacao) {
                	this._armazenadorLocal.salvarToken(resultadoAutenticacao.token);                					
                }.bind(this),
                function () {
					this._armazenadorLocal.removerToken();
                }.bind(this)
            );
		},
		logoff: function () {
			this._armazenadorLocal.removerToken();
		}
	};

	return ServicoAutenticacao;
})();