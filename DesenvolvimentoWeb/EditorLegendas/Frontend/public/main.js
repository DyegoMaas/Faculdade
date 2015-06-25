require.config({
	paths: {
	//	"knockout": "vendor/knockout/dist/knockout",
	//	"knockout-amd-helpers": "vendor/knockout-amd-helpers/build/knockout-amd-helpers.min",
		"text": "vendor/requirejs-text/text",
		//"pagerjs": "vendor/pagerjs/dist/pager.min",
		"materialize": "vendor/materialize/dist/js/materialize.min",
		//"Popcorn": "vendor/popcorn-js/popcorn"
	},
	shim: {
		//"popx": ["Popcorn"]
	},
	baseUrl: "./public"
});

require([
		//"knockout",
		"viewModels/componentes/PlayerViewModel",
		"viewModels/componentes/CarregadorArquivosLegenda",
		"knockout-amd-helpers"
	],
	function (ko, PlayerVideo, CarregadorLegendas) {
		
		function RootModel() {
			var vm = this;
			vm.player = ko.observable();	
			vm.carregadorLegendas = ko.observable();

			this.carregar = function () {
				console.log('carregando view models dos componentes');
				vm.player(new PlayerVideo());
				vm.carregadorLegendas(new CarregadorLegendas());
			};
		}

		var rootModel = new RootModel();
		ko.applyBindings(rootModel);

		setTimeout(function () {
			rootModel.carregar(); 
		}, 10);
	}
);

require(["viewModels/Config"]);
require(["viewModels/componentes/PlayerViewModel"]);
require(["viewModels/componentes/CarregadorArquivosLegenda"]);