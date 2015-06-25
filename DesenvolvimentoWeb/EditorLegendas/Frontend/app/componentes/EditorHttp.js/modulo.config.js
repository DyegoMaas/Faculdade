(function (angular) {
    "use strict";

    angular.module('componentes.editorHttp', [])
    
        .service('editorHttp', ['$http', 'armazenadorLocal', EditorHttp]);

})(angular);