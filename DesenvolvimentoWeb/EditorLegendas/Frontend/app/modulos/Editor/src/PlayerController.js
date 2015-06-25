var PlayerController = (function () {
    "use strict";

    // TODO fazer o carregamento do vídeo com drag'n drop. 
    // Exemplo rodando: http://jsfiddle.net/dsbonev/cCCZ2/
    // Exemplo: http://stackoverflow.com/questions/8885701/play-local-hard-drive-video-file-with-html5-video-tag
    function PlayerController(player) {
        var _viewModel = this;
        var _this = this;
        this._player = player;
    }

    PlayerController.prototype = {
        play: function () {
            this._player.play();
        },

        estahVisivel: function () {
            return false; 
        }
    };

    return PlayerController;

})();