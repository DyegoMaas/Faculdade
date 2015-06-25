(function (angular) {
    "use strict";

    angular.module('SubtitleCannon', [
        'ngRoute',
        'ui.router',
        'vs-repeat',
        'subtitleCannon.modulos',
        'subtitleCannon.componentes'
    ]).run(['$state', 'armazenadorLocal', function ($state, armazenadorLocal) {
    	armazenadorLocal.obterToken().then(
    		function (token) {
    			if(token)
    				$state.go('editor');
    		}
    	);
    }]);
 
})(angular);