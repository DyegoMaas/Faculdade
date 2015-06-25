var ContentEditableComValidacao = (function () {
    "use strict";

    function ContentEditableComValidacao() {
    	var valorOriginal = '';
        return {
		    require: "ngModel",
		    templateUrl: 'app/componentes/ContentEditable/diretivas/contentEditableComValidacao/partials/index.html',
		    scope: {
		    	validacao: '&'
		    },
		    link: function(scope, element, attrs, ngModel) {

		      function read() {
		      	
		      	var valor = element.html();
		      	console.log('valor', valor);
		      	if ($scope.validacao(valor)) {
			      	valorOriginal = valor;
			        ngModel.$setViewValue(valor);	
		      	}
		      	else {
		      		ngModel.$setViewValue(valorOriginal);	
		      	}
		      }

		      ngModel.$render = function() {
		        element.html(ngModel.$viewValue || "");
		      };

		      element.bind("blur keyup change", function() {		      	
	        	scope.$apply(read);
		      });
		    }
		};
	}

    return ContentEditableComValidacao;
})();