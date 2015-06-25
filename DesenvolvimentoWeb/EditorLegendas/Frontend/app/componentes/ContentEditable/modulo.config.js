(function (angular) {
    "use strict";

    angular.module('componentes.contenteditable', [])
    
        .directive('contenteditable', [           
            ContentEditable
        ])
        .directive('contenteditablecomvalidacao', [           
            ContentEditableComValidacao
        ]);

})(angular);