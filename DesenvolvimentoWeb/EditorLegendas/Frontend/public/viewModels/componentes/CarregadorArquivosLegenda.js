//FONTE:
// http://www.sitepoint.com/html5-file-drag-and-drop/
// http://www.sitepoint.com/html5-javascript-open-dropped-files/

//TODO carregar apenas quando o template do editor tiver sido carregado
define(["knockout", "knockout-amd-helpers"], function (ko) {

	var legendaCarregada = null;

	function CarregadorLegendas() {
		var vm = this;

		vm.nomeArquivo = ko.observable();
		vm.tamanhoArquivo = ko.observable();
		vm.conteudoArquivo = ko.observable();
	}
	var carregadorLegendas = new CarregadorLegendas();



	// call initialization file
	if (window.File && window.FileList && window.FileReader) {
		setTimeout(inicializarFileAPI, 1000); //TODO verificar como ter certeza de que o DOM já carregou...
	}

	// initialize
	function inicializarFileAPI() {

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

		// process all File objects
		for (var i = 0, f; f = files[i]; i++) {
			parseFile(f);
		}
	}

	function parseFile(file) {
		//TODO repassar para a API do editor
		console.log(
			"<p>File information: <strong>" + file.name +
			"</strong> type: <strong>" + file.type +
			"</strong> size: <strong>" + file.size +
			"</strong> bytes</p>"
		);

		// display text
		if (file.name.indexOf(".srt") != -1) {

			var reader = new FileReader();
			reader.onload = function(e) {
				console.log('arquivo carregado com sucesso: ' + file.name);

				carregadorLegendas.nomeArquivo(file.name);
				carregadorLegendas.tamanhoArquivo(file.size);
				carregadorLegendas.conteudoArquivo(e.target.result);
			}
			reader.readAsText(file);
		}
	}

	ko.components.register('carregador-legendas', {
        viewModel: CarregadorLegendas,
        template: 'carregador-legendas'
    });     

    //TODO configurar todos os módulos em um único arquivo. Para isso, exportar o módulo com define('nome', [dependencias], function(d1,d2,d3){})    
    //ko.applyBindings(carregadorLegendas);

    return CarregadorLegendas;
});