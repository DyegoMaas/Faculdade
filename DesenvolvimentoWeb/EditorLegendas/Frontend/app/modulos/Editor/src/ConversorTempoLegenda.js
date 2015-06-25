var ConversorTempoLegenda = (function () {
	"use strict";

	function ConversorTempoLegenda() {

	}

	ConversorTempoLegenda.prototype = {
		converterParaMs: function (tempoString) {
			var regex = /(\d+):(\d{2}):(\d{2}),(\d{3})/;
            var parts = regex.exec(tempoString);

            if (parts === null) {
                return 0;
            }

            for (var i = 1; i < 5; i++) {
                parts[i] = parseInt(parts[i], 10);
                if (isNaN(parts[i])) parts[i] = 0;
            }

            // hours + minutes + seconds + ms
            return parts[1] * 3600000 + parts[2] * 60000 + parts[3] * 1000 + parts[4];
		},

		converterParaString: function (tempoMs) {
			var measures = [ 3600000, 60000, 1000 ]; 
            var time = [];

            for (var i in measures) {
                var res = (tempoMs / measures[i] >> 0).toString();
                
                if (res.length < 2) res = '0' + res;
                tempoMs %= measures[i];
                time.push(res);
            }

            var ms = tempoMs.toString();
            if (ms.length < 3) {
                for (i = 0; i <= 3 - ms.length; i++) ms = '0' + ms;
            }

            return time.join(':') + ',' + ms;
		}
	};

	return ConversorTempoLegenda;
})();