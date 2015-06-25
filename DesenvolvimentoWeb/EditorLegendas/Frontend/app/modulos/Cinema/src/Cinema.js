var Cinema = (function () {
	"use strict";

	function Cinema() {

	}

	Cinema.prototype = {
		_popcorn: null,
		
		inicializar: function () {
			var popcorn = new Popcorn("#video", {
                defaults: {
                    subtitle: {
                        target: "legenda"
                    }
                }
            });
            this._popcorn = popcorn;
		},

		agendarLegenda: function (legenda) {
			this._popcorn.subtitle({
				id: legenda.id,
                start: legenda.tempoInicioMs / 1000,
                end: legenda.tempoFimMs / 1000,
                text: legenda.texto
            });		
		},

		atualizarLegenda: function (legenda) {
            this._popcorn.subtitle({
				id: legenda.id,
                start: legenda.tempoInicioMs / 1000,
                end: legenda.tempoFimMs / 1000,
                text: legenda.texto
            });
		},

		play: function () {
			if (this._popcorn)
				this._popcorn.play();
		}
	};

	return Cinema;
})();