var ModalCriacaoLegenda = (function () {
	"use strict";

    function ModalCriacaoLegenda() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {},
            controller: ModalCriacaoLegendaController,
            controllerAs: 'legenda',
            templateUrl: 'app/modulos/Editor/diretivas/modal-criacao-legenda/partial/index.html'
        }
    }

	function ModalCriacaoLegendaController (ListaLegendas, EditorLegendas, LegendaViewModel, ConversorTempoLegenda, LimiaresNovaLegenda, $scope) { 
        var listaLegendas = ListaLegendas,
            editorLegendas = EditorLegendas,
            conversorTempoLegenda = ConversorTempoLegenda,
            limiaresNovaLegenda = LimiaresNovaLegenda;

        var viewModel = this;
        viewModel.idLegenda = 1;
        viewModel.texto = '';
        viewModel.tempoInicio = '00:00:00,000';
        viewModel.tempoFim = '00:00:00,000';

        viewModel.segundosEdicao = 0;
        viewModel.milisegundosEdicao = 500;

        viewModel.adiantarTempoInicio = _adiantarTempoInicio;
        viewModel.adiantarTempoFim = _adiantarTempoFim;

        viewModel.atrasarTempoInicio = _atrasarTempoInicio;
        viewModel.atrasarTempoFim = _atrasarTempoFim;

        viewModel.salvar = _salvar;

        var _legendaMenor = null;

        $scope.$watch(
            function () { 
                return limiaresNovaLegenda.legendaAnterior; 
            },
            function (newValue, oldValue) {
                console.log('anterior', newValue);
                _limparTela();
                _atualizarLegendaMenor(newValue);
            }
        );

        $scope.$watch(
            function () { 
                return limiaresNovaLegenda.legendaPosterior; 
            },
            function (newValue, oldValue) {
                console.log('posterior', newValue);
                _atualizarLegendaMaior(newValue);
            }
        );

        function _limparTela() {
            viewModel.idLegenda = 1;
            viewModel.texto = '';
            viewModel.tempoInicio = '00:00:00,000';
            viewModel.tempoFim = '00:00:00,000';

            viewModel.segundosEdicao = 0;
            viewModel.milisegundosEdicao = 500;
        }

        function _atualizarLegendaMenor(legenda) {
            _legendaMenor = legenda;
            if (legenda) {
                viewModel.tempoInicio = legenda.tempoFim;
            }
            else {
                viewModel.idLegenda = 1;
                viewModel.tempoInicio = '00:00:00,000';
            }
        }

        function _atualizarLegendaMaior(legenda) {
            if (legenda) {
                viewModel.idLegenda = legenda.id;
                viewModel.tempoFim = legenda.tempoInicio;
            }
            else {
                viewModel.idLegenda = editorLegendas.obterProximoId();
                if (_legendaMenor) {
                    viewModel.tempoFim = viewModel.tempoInicio;
                }
            }
        }

//TODO no onchange do tempoInicio, recalcular o novo id da legenda
        function _salvar() {
            //TODO mostrar os erros de forma mais bonita
            if (viewModel.texto == '') {
                alert('O texto da legenda é obrigatório.');
                return;
            }

            if ($('#tempoInicioNovaLegenda').is(':invalid')) {
                alert('O tempo inicial da legenda não está no formato correto.');
                return;   
            }

            if ($('#tempoFimNovaLegenda').is(':invalid')) {
                alert('O tempo final da legenda não está no formato correto.');
                return;   
            }

            if (viewModel.tempoInicio == viewModel.tempoFim) {
                alert('Deve haver um intervalo de tempo entre o início e o fim da legenda.');
                return;      
            }

            var tempoInicioMs = conversorTempoLegenda.converterParaMs(viewModel.tempoInicio);
            var tempoFimMs = conversorTempoLegenda.converterParaMs(viewModel.tempoFim);

            if (tempoFimMs < tempoInicioMs) {
                alert('O tempo de início deve ser maior que tempo de fim da legenda.');
                return;      
            }

            var novaLegenda = new LegendaViewModel(
                viewModel.idLegenda, 
                tempoInicioMs, 
                tempoFimMs, 
                viewModel.tempoInicio, viewModel.tempoFim, 
                viewModel.texto);

            console.log('salvando legenda #', viewModel.idLegenda);
            editorLegendas.adicionar(novaLegenda);

            $('#modalCriacaoLegenda').closeModal();
        }

        function _adiantarTempoInicio() {
            var tempoAjustadoMs = _obterTempoAjustadoComAdiantamento(viewModel.tempoInicio);

            var tempoAjustadoString = conversorTempoLegenda.converterParaString(tempoAjustadoMs);
            viewModel.tempoInicio = tempoAjustadoString;
        }

        function _atrasarTempoInicio() {
            var tempoAjustadoMs =_obterTempoAjustadoComAtraso(viewModel.tempoInicio);

            var tempoFimMs = conversorTempoLegenda.converterParaMs(viewModel.tempoFim);
            if (tempoAjustadoMs > tempoFimMs)
                tempoAjustadoMs = tempoFimMs;

            var tempoAjustadoString = conversorTempoLegenda.converterParaString(tempoAjustadoMs);
            viewModel.tempoInicio = tempoAjustadoString;
        }

        function _adiantarTempoFim() {
            var tempoAjustadoMs = _obterTempoAjustadoComAdiantamento(viewModel.tempoFim);

            var tempoInicioMs = conversorTempoLegenda.converterParaMs(viewModel.tempoInicio);
            if(tempoAjustadoMs < tempoInicioMs)
                tempoAjustadoMs = tempoInicioMs;

            var tempoAjustadoString = conversorTempoLegenda.converterParaString(tempoAjustadoMs);
            viewModel.tempoFim = tempoAjustadoString;
        }

        function _atrasarTempoFim() {
            var tempoAjustadoMs =_obterTempoAjustadoComAtraso(viewModel.tempoFim);

            var tempoAjustadoString = conversorTempoLegenda.converterParaString(tempoAjustadoMs);
            viewModel.tempoFim = tempoAjustadoString;
        }

        function _obterTempoAjustadoComAdiantamento(tempoString) {
            var ajuste = _obterMilisegundosAjuste();
            var tempoMs = conversorTempoLegenda.converterParaMs(tempoString);
            var tempoAjustadoMs = tempoMs - ajuste;

            if(tempoAjustadoMs < 0)
                tempoAjustadoMs = 0;

            return tempoAjustadoMs;
        }

        function _obterTempoAjustadoComAtraso(tempoString) {
            var ajuste = _obterMilisegundosAjuste();
            var tempoMs = conversorTempoLegenda.converterParaMs(tempoString);
            var tempoAjustadoMs = tempoMs + ajuste;
            
            return tempoAjustadoMs;
        }

        function _obterMilisegundosAjuste() {
            var milisegundosAjuste = viewModel.milisegundosEdicao;
            var segundosAjutes = viewModel.segundosEdicao;

            if(milisegundosAjuste <= 0)
                milisegundosAjuste = 0;

            if(segundosAjutes <= 0)
                segundosAjutes = 0;

            return milisegundosAjuste + segundosAjutes * 1000;
        }
	}

	return ModalCriacaoLegenda;
})();