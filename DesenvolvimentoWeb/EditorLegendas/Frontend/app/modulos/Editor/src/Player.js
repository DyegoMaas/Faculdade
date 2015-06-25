var Player = (function() {
	'use strict'

	function Player(cinema, listaLegendas){
		var _this = this;

		// this._popcorn = null;
		// this.reconfigurar = _reconfigurarLegenda;
		this.play = _play;


		var reader = new FileReader();

		// call initialization file
		if (window.File && window.FileList && window.FileReader) {
			inicializarAPIArquivosParaVideos();			
		}
		else {
			setTimeout(inicializarAPIArquivosParaVideos, 500);
		}

		// initialize
		function inicializarAPIArquivosParaVideos() {

			var fileselect = document.getElementById('fileselectVideo'),
				filedrag = document.getElementById('filedragVideo');
				// submitbutton = document.getElementById('submitbuttonVideo');

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
				// submitbutton.style.display = "none";
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
			if (_ehArquivoVideo(arquivo)) {
				console.log('eh arquivo de vídeo');

				var urlVideo = window.URL.createObjectURL(arquivo);
				console.log('url do vídeo ', urlVideo);
				$('#video').attr('src', urlVideo); //testar
			}
		}

		function _ehArquivoVideo(arquivo) {			
			if(arquivo.type == "video/mp4") // TODO ver MIME type do oggv
				return true;

			if(arquivo.name.indexOf(".mkv") != -1)
				return true;

			return false;
		}



		function _play() {
			cinema.inicializar();
			// _this.popcorn = popcornFactory.criar();

			for (var i = 0; i < listaLegendas.lista.length; i++) {
				var legenda = listaLegendas.lista[i];
				// _reconfigurarLegenda(legenda);
				cinema.agendarLegenda(legenda);
			};
			cinema.play();
			// _this.popcorn.play();
		}

		function _reconfigurarLegenda(legenda){
			cinema.agendarLegenda(legenda);
			// var popcorn = _this.popcorn	
			// popcorn.subtitle({
			// 	id: legenda.id,
   //              start: legenda.tempoInicioMs / 1000,
   //              end: legenda.tempoFimMs / 1000,
   //              text: legenda.texto
   //          });		
   //          var trackId = popcorn.getLastTrackEventId();
   //          debugger;
		}
	}

	return Player;
})();