(function (angular) {
    "use strict";

    angular.module('componentes.editorHttp', [])
    
        .service('editorHttp', ['$http', EditorHttp]);

})(angular);