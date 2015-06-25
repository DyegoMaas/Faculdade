(function (angular) {
    "use strict";

    angular.module('modulo.exportacao', [])
        
        .service('Exportador', ['SrtSubtitleParser', function (srtSubtitleParser) {
            return new Exportador(srtSubtitleParser);
        }])		
		.directive('modalExportacaoLegendas', ['ListaLegendas', 'Exportador', '$timeout', ModalExportacaoLegendas]);

})(angular);