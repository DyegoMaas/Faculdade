var ModalExportacaoLegendas = (function () {
	"use strict";

    function ModalExportacaoLegendas() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {                
            },
            controller: ModalExportacaoLegendasController,
            controllerAs: 'exportador',
            templateUrl: 'app/modulos/Exportador/diretivas/modal-exportacao-legendas/partial/index.html'
        }
    }

	function ModalExportacaoLegendasController (ListaLegendas, Exportador, $timeout) { 
        var listaLegendas = ListaLegendas;
        var exportador = Exportador;

		var viewModel = this;
        viewModel.nomeArquivo = "legenda.srt";
        viewModel.exportacaoEstahHabilidada = _exportacaoEstahHabilidada;
 		viewModel.exportarLegendas = _exportarLegendas;
        viewModel.linkDownloadEstaDisponivel = _linkDownloadEstaDisponivel;
        viewModel.linkDownload = null;
        viewModel.linkDownloadDisponivel = false;
        viewModel.conteudoDownload = null;
        viewModel.baixarArquivo = _desabilitarLinkDownload;

        _adicionarEventosParaArrastarLinkDownload();

        function _exportacaoEstahHabilidada() {
            return viewModel.nomeArquivo != "";
        }

        // Fonte da estratégia de implementação da exportação: http://html5-demos.appspot.com/static/a.download.html
        function _exportarLegendas() {
            var dadosExportacao = exportador.gerarLinkDownload(listaLegendas.lista);
            viewModel.linkDownload = dadosExportacao.linkDownload;
            viewModel.conteudoDownload = dadosExportacao.conteudo;            

            var a = _obterElementoLinkDownload();
            if (a.href) {
                _revogarLink(a.href);
            }   

            a.textContent = 'Pronto para baixar - Arquivo gerado em ' + _momento();
            a.download = viewModel.nomeArquivo;
            a.href = dadosExportacao.linkDownload;
            a.dataset.downloadurl = [dadosExportacao.mimeType, a.download, a.href].join(':');
            a.classList.add('dragout');
            viewModel.linkDownloadDisponivel = true;
        }

        function _linkDownloadEstaDisponivel() {
            return viewModel.linkDownload != null;
        }

        function _desabilitarLinkDownload() {
            console.log('arquivo baixado');

            var a = _obterElementoLinkDownload();
            a.textContent = 'Baixado em ' + _momento();
            a.dataset.disabled = true;

            $timeout(function() {
                _revogarLink(a.href);
            }, 1500);
        }

        function _momento() {
            return moment().format('LLL');
        }

        function _revogarLink(url) {
            window.URL.revokeObjectURL(url);
            viewModel.linkDownloadDisponivel = false;
            console.log('link revogado: ', url);
        }

        function _adicionarEventosParaArrastarLinkDownload() {
            // Rockstars use event delegation!
            document.body.addEventListener('dragstart', function(e) {
                var a = e.target;
                if (a.classList.contains('dragout')) {
                    e.dataTransfer.setData('DownloadURL', a.dataset.downloadurl);
                }
            }, false);

            document.body.addEventListener('dragend', function(e) {
                var a = e.target;
                if (a.classList.contains('dragout')) {
                    _desabilitarLinkDownload();
                }
            }, false);
        }

        function _obterElementoLinkDownload() {
            return document.getElementById('linkDownload');
        }
	}

	return ModalExportacaoLegendas;
})();