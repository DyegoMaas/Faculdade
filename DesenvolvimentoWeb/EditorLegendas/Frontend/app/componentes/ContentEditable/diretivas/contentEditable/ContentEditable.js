//http://gaboesquivel.com/blog/2014/in-place-editing-with-contenteditable-and-angularjs/
var ContentEditable = (function () {
    "use strict";

    function ContentEditable() {
        return {
		    require: "ngModel",
		    templateUrl: 'app/componentes/ContentEditable/diretivas/contentEditable/partials/index.html',
		    link: function(scope, element, attrs, ngModel) {

		      function read() {
		        ngModel.$setViewValue(element.html());
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

    return ContentEditable;
})();