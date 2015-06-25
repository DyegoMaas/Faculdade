define('modulos', [
    'knockout',
    'PlayerVideo'
    ], 
    function (ko, PlayerVideo) {
        console.log('player-video');        
        ko.components.register('player-video', {
            viewModel: PlayerVideo,
            template: 'player-video'
        });     

        //TODO configurar todos os módulos em um único arquivo. Para isso, exportar o módulo com define('nome', [dependencias], function(d1,d2,d3){})
        ko.applyBindings(new PlayerVideo(ko));

        return {};
    }
);