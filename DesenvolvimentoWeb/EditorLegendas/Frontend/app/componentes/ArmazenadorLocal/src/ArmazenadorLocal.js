var ArmazenadorLocal = (function () {
    "use strict";

    function ArmazenadorLocal (storage) {
        this._storage = storage;
    }

    ArmazenadorLocal.prototype = {
        salvarToken: function (token) {
            this._storage.definir('Token', token);
        },
        obterToken: function () {
            return this._storage.obter('Token').then(
                function (token) {
                    return token;
                }
            );
        },
        removerToken: function () {
          return this._storage.remover('Token').then(
                function (token) {
                    return token;
                }
            );  
        }
    };

    return ArmazenadorLocal;

})();