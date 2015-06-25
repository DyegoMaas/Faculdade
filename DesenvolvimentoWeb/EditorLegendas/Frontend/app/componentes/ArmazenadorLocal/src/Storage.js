var Storage = (function () {
    'use strict';

    function Storage() {}

    Storage.prototype = {
        definir: function (chave, valor) {
            if (_ehUmaChromeApp()) {
                var objeto = {};

                objeto[chave] = valor;
                chrome.storage.local.set(objeto);
            }
            else if (_localStorageEstahDisponivel()) {
                localStorage.setItem(chave, valor);
            }
            else {
                throw new Error('Não há como armazenar à informação "' + chave + '" neste PDV.');
            }
        },
        obter: function (chave) {
            var deferred = $.Deferred();

            if (_ehUmaChromeApp()) {
                chrome.storage.local.get(chave, function (objeto) {
                    return deferred.resolve(objeto[chave]);
                });
            }
            else if (_localStorageEstahDisponivel()) {
                var valor = localStorage.getItem(chave);
                return deferred.resolve(valor);
            }

            return deferred.promise();
        },
        remover: function (chave) {
            var deferred = $.Deferred();

            if (_ehUmaChromeApp()) {
                chrome.storage.local.remove(chave, function (objeto) {
                    return deferred.resolve();
                });
            }
            else if (_localStorageEstahDisponivel()) {
                var valor = localStorage.removeItem(chave);
                return deferred.resolve();
            }

            return deferred.promise();
        }
    };

    function _ehUmaChromeApp() {
        return false;
    }

    function _localStorageEstahDisponivel() {
        return localStorage !== undefined;
    }

    return Storage;

})();