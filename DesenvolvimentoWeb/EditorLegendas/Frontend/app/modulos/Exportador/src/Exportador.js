var Exportador = (function () {
	"use strict";

	function Exportador(srtSubtitleParser) {
		this.parser = srtSubtitleParser;
	}

	Exportador.prototype = {
		//TODO usar promises
		//TODO estudar possibilidade de usar https://github.com/inexorabletash/text-encoding
		gerarLinkDownload: function (legendas) {
			if(legendas.length == 0)
				return null;

			var srt = this.parser.toSrt(legendas);	
					
			const MIME_TYPE = 'text/plain';

			var blob = new Blob([srt], { type: MIME_TYPE });
			var linkDownload = window.URL.createObjectURL(blob);
			
			return {
				linkDownload: linkDownload,
				conteudo: srt,
				mimeType: MIME_TYPE
			};
		}
	};

	return Exportador;
})();