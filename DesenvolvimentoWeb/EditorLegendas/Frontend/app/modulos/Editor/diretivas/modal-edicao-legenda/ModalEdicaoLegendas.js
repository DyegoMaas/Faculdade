var ModalEdicaoLegenda = (function () {
	"use strict";

    function ModalEdicaoLegenda() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {
                legenda: '='
            },
            controller: ModalEdicaoLegendaController,
            controllerAs: 'editor',
            templateUrl: 'app/modulos/Editor/diretivas/modal-edicao-legenda/partial/index.html'
        }
    }

	function ModalEdicaoLegendaController (ListaLegendas, EditorLegendas, LegendaViewModel, $scope) { 
        var listaLegendas = ListaLegendas,
            editorLegendas = EditorLegendas;

        var viewModel = this;
        viewModel.segundosEdicaoIndividual = 0;
        viewModel.milisegundosEdicaoIndividual = 500;
        viewModel.legendaEmEdicao = $scope.legenda;

        viewModel.adiantarTempoInicio = _adiantarTempoInicio;
        viewModel.adiantarTempoFim = _adiantarTempoFim;

        viewModel.atrasarTempoInicio = _atrasarTempoInicio;
        viewModel.atrasarTempoFim = _atrasarTempoFim;

        $scope.$watch(
            function(scope) { 
                return scope.legenda; 
            },
            function (newValue, oldValue) {

                //copiar a legenda
                viewModel.legendaEmEdicao = newValue;
                //viewModel.legendaEmEdicao= new LegendaViewModel();
            }
        );

        function _adiantarTempoInicio() {
            var ajuste = _obterMilisegundosAjuste();
            editorLegendas.adiantar(viewModel.legendaEmEdicao, ajuste, { fim: false });
        }

        function _atrasarTempoInicio() {
            var ajuste = _obterMilisegundosAjuste();
            editorLegendas.atrasar(viewModel.legendaEmEdicao, ajuste, { fim: false });
        }

        function _adiantarTempoFim() {
            var ajuste = _obterMilisegundosAjuste();
            editorLegendas.adiantar(viewModel.legendaEmEdicao, ajuste, { inicio: false });
        }

        function _atrasarTempoFim() {
            var ajuste = _obterMilisegundosAjuste();
            editorLegendas.atrasar(viewModel.legendaEmEdicao, ajuste, { inicio: false });
        }

        function _obterMilisegundosAjuste() {
            var milisegundosAjuste = viewModel.milisegundosEdicaoIndividual;
            var segundosAjutes = viewModel.segundosEdicaoIndividual;

            if(milisegundosAjuste <= 0)
                milisegundosAjuste = 0;

            if(segundosAjutes <= 0)
                segundosAjutes = 0;

            return milisegundosAjuste + segundosAjutes * 1000;
        }
	}

	return ModalEdicaoLegenda;
})();