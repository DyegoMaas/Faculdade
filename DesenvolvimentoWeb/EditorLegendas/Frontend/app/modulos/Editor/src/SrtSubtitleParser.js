var SrtSubtitleParser = (function() {

    function SrtSubtitleParser(LegendaViewModel) {
        /**
         * Converts SubRip subtitles into array of objects
         * [{
         *     id:        `Number of subtitle`
         *     startTime: `Start time of subtitle`
         *     endTime:   `End time of subtitle
         *     text: `Text of subtitle`
         * }]
         *
         * @param  {String}  data SubRip suntitles string
         * @return {Array}  
         */
        this.fromSrt = function(data) {
            data = data.replace(/\r/g, '');
            var regex = /(\d+)\n(\d{2}:\d{2}:\d{2},\d{3}) --> (\d{2}:\d{2}:\d{2},\d{3})/g;
            data = data.split(regex);
            data.shift();

            var items = [];
            for (var i = 0; i < data.length; i += 4) {
                var id = parseInt(data[i].trim());
                var startTime = data[i + 1].trim();
                var startTimeMs = _converterParaMs(startTime);
                var endTime = data[i + 2].trim();
                var endTimeMs = _converterParaMs(endTime);
                var text = data[i + 3].trim();
                items.push(new LegendaViewModel(id, startTimeMs, endTimeMs, startTime, endTime, text));
            }

            return items;
        };

        /**
         * Converts Array of objects created by this module to SubRip subtitles
         * @param  {Array}  data
         * @return {String}      SubRip subtitles string
         */
        this.toSrt = function(legendas) {
            if (!legendas instanceof Array) return '';
            var res = '';

            for (var i = 0; i < legendas.length; i++) {
                var legenda = legendas[i];
                var quebraLinha = '\r\n';

                res += legenda.id + quebraLinha;
                res += legenda.tempoInicio + ' --> ' + legenda.tempoFim + quebraLinha;
                res += legenda.texto
                    .replace('<div>', quebraLinha)
                    .replace('</div>', '')
                    .replace('&nbsp;', ' ')
                    .replace('\n', quebraLinha) + quebraLinha + quebraLinha;
            }

            return res;
        };

        var _converterParaMs = function(tempoString) {
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
        };

        var _converterDeMsParaTempoString = function(tempoMs) {
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
        };

    }

    return SrtSubtitleParser;
})();