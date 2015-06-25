var CarregadorLegendasController = (function () {
    "use strict";

    function CarregadorLegendasController($timeout, srtSubtitleParser, editorLegendas) {

        var _viewModel = this;
        _viewModel.nomeArquivo = "";
        _viewModel.tamanhoArquivo = "";

        const charset = "ISO-8859-1";
        var reader = new FileReader();

		// call initialization file
		if (window.File && window.FileList && window.FileReader) {
			inicializarAPIArquivosParaLegendas();			
		}
		else {
			setTimeout(inicializarAPIArquivosParaLegendas, 500);
		}

		// initialize
		function inicializarAPIArquivosParaLegendas() {

			var fileselect = document.getElementById('fileselect'),
				filedrag = document.getElementById('filedrag'),
				submitbutton = document.getElementById('submitbutton');

			// file select
			fileselect.addEventListener("change", fileSelectHandler, false);

			// is XHR2 available?
			var xhr = new XMLHttpRequest();
			if (xhr.upload) {
			
				// file drop
				filedrag.addEventListener("dragover", fileDragHover, false);
				filedrag.addEventListener("dragleave", fileDragHover, false);
				filedrag.addEventListener("drop", fileSelectHandler, false);
				filedrag.style.display = "block";
				
				// remove submit button
				submitbutton.style.display = "none";
			}
		}

		// file drag hover
		function fileDragHover(e) {
			e.stopPropagation();
			e.preventDefault();
			e.target.className = (e.type == "dragover" ? "hover" : "");
		}

		// file selection
		function fileSelectHandler(e) {

			// cancel event and hover styling
			fileDragHover(e);

			// fetch FileList object
			var files = e.target.files || e.dataTransfer.files;
			var arquivo = files[0]

			console.log(
				"<p>File information: <strong>" + arquivo.name +
				"</strong> type: <strong>" + arquivo.type +
				"</strong> size: <strong>" + arquivo.size +
				"</strong> bytes</p>"
			);

			// processa o arquivo de legenda
			if (_ehArquivoLegenda(arquivo)) {
				_parseFile(arquivo);
			}
		}

		function _ehArquivoLegenda(arquivo) {			
			return arquivo.name.indexOf(".srt") != -1;
		}

		function _parseFile(arquivo) {

			var reader = new FileReader();
			reader.onload = function(e) {
				console.log('arquivo carregado com sucesso: ' + arquivo.name);
				var legendas = srtSubtitleParser.fromSrt(e.target.result);
				
				$timeout(function() {
					$.each(legendas, (function(i, legenda) {
						editorLegendas.adicionar(legenda);
					}));
				}, 100);					
			};
			reader.readAsText(arquivo, charset);
		}
    }

    return CarregadorLegendasController;

})();