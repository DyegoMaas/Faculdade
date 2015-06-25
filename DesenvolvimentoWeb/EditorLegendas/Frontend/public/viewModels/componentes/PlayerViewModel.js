define([
        "knockout",
        "knockout-amd-helpers"
    ], 
    function (ko) {

        var play = function (){
            var popcorn = new Popcorn("#video", {
                defaults: {
                    subtitle: {
                        target: "legenda"
                    }
                }
            });
            popcorn.subtitle({
                start: 2,
                end: 5,
                text: "primeiro texto!"
            });

            popcorn.subtitle({
                start: 7,
                end: 9,
                text: "segundo texto!"
            });
            popcorn.play();
        };

    	function PlayerViewModel() { 
        	var vm = this;

        	vm.texto = ko.observable('texto da legenda');
            vm.play = play;
        }
        
        ko.components.register('player-video', {
            viewModel: PlayerViewModel,
            template: 'player-video'
        });
        console.log('registrei o componente player-video');

        //TODO configurar todos os módulos em um único arquivo. Para isso, exportar o módulo com define('nome', [dependencias], function(d1,d2,d3){})
        //ko.applyBindings(new PlayerViewModel());

        return PlayerViewModel;
    }
);








// define('playerVideo', [], function () {
//     function PlayerVideo(ko) {
//         this._ko = ko;
//     }

//     PlayerVideo.prototype = {
//         play: function () {
//             console.log('Da o play macaco!');
//         }
//     };

//     return PlayerVideo;
// });

// define('listaLegendas', [], function () {

// });

// define('moduloRegistrador', [
//     'knockout',
//     'PlayerVideo',
//     'listaLegendas'
//     ], 
//     function (ko, PlayerVideo, listaLegendas) {
        
//          ko.components.register('player-video', {
//             viewModel: PlayerVideo,
//             template: 'player-video'
//         });     

//         //TODO configurar todos os módulos em um único arquivo. Para isso, exportar o módulo com define('nome', [dependencias], function(d1,d2,d3){})
//         ko.applyBindings(new PlayerVideo(ko));

//     }
// );