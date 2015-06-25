var EditorHttp = (function () {
    "use strict";

    function EditorHttp($http, armazenadorLocal) {
        var $public = {};
        var $private = {};

        $public.get = function (url, dados, callback) {
            return $private.requisicao('GET', {
                url: url,
                dados: dados
            }, callback);
        };

        $public.post = function (url, dados, callback) {
            return $private.requisicao('POST', {
                url: url,
                dados: dados
            }, callback);
        };

        $public.put = function (url, dados, callback) {
            return $private.requisicao('PUT', {
                url: url,
                dados: dados
            }, callback);
        };

        $public.delete = function (url, callback) {
            return $private.requisicao('DELETE', {
                url: url
            }, callback);
        };

        $private.requisicao = function (metodo, dadosRequisicao, callback) {
            var deferred = $.Deferred();

            $private.montarRequisicao(metodo, dadosRequisicao).then(
                function (requisicao) {
                    $http(requisicao).then(
                        function (resultado) {
                            if (callback) {
                                return callback(resultado.data);
                            } else {
                                if (resultado.data.sucesso) {
                                    deferred.resolve(resultado.data.dados);
                                } else {
                                	alert(resultado.data.erros);

                                    throw resultado.data.erros;
                                }
                            }
                        },
                        function (razao) {
                            if (callback) {
                                return callback(razao);
                            } else {
                            	if(razao.status = 401) //não autorizado
                            		alert('Usuário ou senha incorretos.');
                            	else
                            		alert('Houve algum erro ao tentar realizar esta ação. Por favor, tente novamente.');
                            }
                        }
                    );
                }
            );

            return deferred.promise();
        };

        $private.montarRequisicao = function (metodo, dadosRequisicao) {     
            return armazenadorLocal.obterToken().then(
                function (token) {
                	var urlBase = $private.obterUrlBase();
                    var requisicao = {
                        method: metodo,
                        url: urlBase + dadosRequisicao.url,
                        headers: {
                            "Authentication": "Token " + token
                        }
                    };

                    if ($private.ehPost(metodo) || $private.ehPut(metodo)) {
                        requisicao.data = dadosRequisicao.dados;
                    } else {
                        requisicao.params = dadosRequisicao.dados;
                    }

                    return requisicao;
                }
            );
        };

        $private.obterUrlBase = function () {
        	return 'http://' + window.location.hostname + ':3579';
        };

        $private.ehPost = function (metodo) {
            return metodo == "POST";
        };

        $private.ehPut = function (metodo) {
            return metodo == "PUT";
        };

        $private.valorEhValido = function (valor) {
            return valor !== undefined
                && valor !== null
                && valor !== '';
        };

        return $public;
    }

    return EditorHttp;
})();