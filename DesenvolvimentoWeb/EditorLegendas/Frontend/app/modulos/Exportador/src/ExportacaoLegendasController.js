var ExportacaoLegendasController = (function () {
	"use strict";

	function ExportacaoLegendasController (listaLegendas, exportador) {
		var viewModel = this;
 		viewModel.exportarLegendas = _exportarLegendas;
        viewModel.linkDownloadEstaDisponivel = _linkDownloadEstaDisponivel;
        viewModel.linkDownload = null;
        viewModel.conteudoDownload = null;
        viewModel.arquivoBaixado = _desabilitarLinkDownload;

        // Fonte da estratégia de implementação da exportação: http://html5-demos.appspot.com/static/a.download.html
        function _exportarLegendas() {
            var dadosExportacao = exportador.gerarLinkDownload(listaLegendas.lista);
            _viewModel.linkDownload = dadosExportacao.linkDownload;
            _viewModel.conteudoDownload = dadosExportacao.conteudo;            

            var a = _obterElementoLinkDownload();
            if (a.href) {
                console.log('revogando link ', a.href);
                _revogarLink(a.href);
            }   

            a.textContent = 'Pronto para baixar';
            a.download = 'legenda.srt';
            a.href = dadosExportacao.linkDownload;
            a.dataset.downloadurl = [dadosExportacao.mimeType, a.download, a.href].join(':');
            a.classList.add('dragout');
        }

        function _linkDownloadEstaDisponivel() {
            return _viewModel.linkDownload != null;
        }

        function _desabilitarLinkDownload() {
            console.log('arquivo baixado');

            var a = _obterElementoLinkDownload();
            a.textContent = 'Baixado';
            a.dataset.disabled = true;
            
            setTimeout(function() {
                _revogarLink(a.href);
            }, 1500);
        }

        function _revogarLink(url) {
            window.URL.revokeObjectURL(url);
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

	return ExportacaoLegendasController;
})();