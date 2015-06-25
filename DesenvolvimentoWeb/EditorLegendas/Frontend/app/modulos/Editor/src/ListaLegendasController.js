var ListaLegendasController = (function () {
    "use strict";

    function ListaLegendasController(listaLegendas, editorLegendas, exportador, limiaresNovaLegenda) {
        var viewModel = this;
        viewModel.legendas = listaLegendas.lista;
        viewModel.legendaEmEdicao = null;
        viewModel.editarLegenda = _editarLegenda;
        viewModel.excluirLegenda = _excluirLegenda;

        viewModel.segundosEdicao = 0;
        viewModel.milisegundosEdicao = 500;
        viewModel.atrasarEmLote = _atrasarEmLote;
        viewModel.adiantarEmLote = _adiantarEmLote;
        viewModel.acoesEmLoteHabilitadas = _acoesEmLoteHabilitadas;

        viewModel.exibirBotoesAdicao = _exibirBotoesAdicao;
        viewModel.ativarBotoesAdicao = _ativarBotoesAdicao;
        viewModel.inativarBotoesAdicao = _inativarBotoesAdicao;
        viewModel.criarPrimeiraLegenda = _criarPrimeiraLegenda;
        viewModel.criarLegendaAntes = _criarLegendaAntes;
        viewModel.criarLegendaDepois = _criarLegendaDepois;
        // viewModel.legendaAnterior = null;
        // viewModel.legendaAtual = null;
        // viewModel.legendaPosterior = null;

        var _legendaEmFoco = null;

        function _atrasarEmLote(){
            var milisegundos =_obterMilisegundosAjuste();
        	editorLegendas.atrasarTodasEm(milisegundos);
        }

        function _adiantarEmLote() {
            var milisegundos =_obterMilisegundosAjuste();
			editorLegendas.adiantarTodasEm(milisegundos);
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

        function _editarLegenda(legenda) {
            viewModel.legendaEmEdicao = legenda;
            $('#modalEdicaoLegenda').openModal();
        }

        function _excluirLegenda(legenda) {
            console.log('excluindo legenda #' + legenda.id);

            viewModel.legendaEmEdicao = null;
            editorLegendas.excluir(legenda);
        };

        function _acoesEmLoteHabilitadas() {
            return listaLegendas.lista.length > 0;
        }

        function _ativarBotoesAdicao(legenda) {
            _legendaEmFoco = legenda;
        }

        function _inativarBotoesAdicao(legenda) {
            _legendaEmFoco = null;
        }

        function _exibirBotoesAdicao(legenda) {
            return _legendaEmFoco == legenda;
        }

        function _criarPrimeiraLegenda() {
            limiaresNovaLegenda.legendaAnterior = null;
            limiaresNovaLegenda.legendaPosterior = null;
            _abrirModalCriacaoLegenda();
        }

        function _criarLegendaAntes(legenda) {
            limiaresNovaLegenda.legendaAnterior = editorLegendas.obterLegendaAnterior(legenda);
            limiaresNovaLegenda.legendaPosterior = legenda;
            _abrirModalCriacaoLegenda();
        }

        function _criarLegendaDepois(legenda) {
            limiaresNovaLegenda.legendaAnterior = legenda;
            limiaresNovaLegenda.legendaPosterior = editorLegendas.obterLegendaPosterior(legenda);
            _abrirModalCriacaoLegenda();
        }

        function _abrirModalCriacaoLegenda() {
            $('#modalCriacaoLegenda').openModal();
        }
    }

    return ListaLegendasController;
})();