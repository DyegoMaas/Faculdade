//TODO ao atualizar uma legenda, alterar diretamente no Cinema (atualizar)
var EditorLegendas = (function() {

    function EditorLegendas(listaLegendas) {
        var _this = this;
        
        var _dicionario = new Array();

        this.adicionar = function (novaLegenda) {
            _dicionario[novaLegenda.id] = novaLegenda;
            listaLegendas.adicionar(novaLegenda);  
        }
        
        this.atrasar = function (legenda, milisegundos, config) {
            _atrasarEm(milisegundos, legenda, config);
        };

        this.adiantar = function (legenda, milisegundos, config) {
            _adiantarEm(milisegundos, legenda, config);
        };

        this.atrasarTodasEm = function (milisegundos) {
            var lista = listaLegendas.lista;
            for (var i = 0; i < lista.length; i++) {
                this.atrasar(lista[i], milisegundos);
            };
        };

        this.adiantarTodasEm = function (milisegundos) {
            var lista = listaLegendas.lista;
            for (var i = 0; i < lista.length; i++) {
                this.adiantar(lista[i], milisegundos);
            };   
        };

        this.excluir = function (legenda) {
            var indice = listaLegendas.lista.indexOf(legenda);            
            if (indice > -1) {                    
                //ajustando índices
                for (var i = indice + 1; i < listaLegendas.lista.length; i++) {
                    var legendaComIndiceEmCorrecao = listaLegendas.lista[i];
                    legendaComIndiceEmCorrecao.id--;

                    //atualizando o dicionário
                    _dicionario[legendaComIndiceEmCorrecao.id] = legendaComIndiceEmCorrecao;
                };

                // excluindo a legenda
                listaLegendas.lista.splice(indice, 1);
            }
        };

        this.obterLegendaAnterior = function (legenda) {            
            var idLegendaAnterior = legenda.id - 1;

            var legendaAnterior = _dicionario[idLegendaAnterior];
            if (typeof legendaAnterior === 'undefined')
                return null;

            return legendaAnterior;
        };

        this.obterLegendaPosterior = function (legenda) {
            var idLegendaPosterior = legenda.id + 1;

            var legendaPosterior = _dicionario[idLegendaPosterior];
            if (typeof legendaPosterior === 'undefined')
                return null;

            return legendaPosterior;
        };

        //TODO implementar teste
        this.obterProximoId = function () {
            var proximoId = 1;
            for (var i = 0; i < listaLegendas.lista.length; i++) {
                var legenda = listaLegendas.lista[i];
                if(legenda.id > proximoId)
                    proximoId = legenda.id + 1;
            };
            return proximoId;
        };

        function _atrasarEm(milisegundos, legenda, config) {
            config = _completarConfig(config);

            if (config.inicio) {
                var tempoInicio = legenda.tempoInicioMs + milisegundos;
                legenda.atualizarTempoInicio(tempoInicio);

                // ajustando os índices
                var idOriginal = legenda.id;
                for (var id = idOriginal + 1; id <= listaLegendas.lista.length; id++) {
                    var proximaLegenda = _dicionario[id];
                    if(legenda.tempoInicioMs > proximaLegenda.tempoInicioMs){
                        _swapIds(legenda, proximaLegenda);
                    }
                    else 
                        break;
                };
            }
            
            if (config.fim) {
                var tempoFim = legenda.tempoFimMs + milisegundos;
                legenda.atualizarTempoFim(tempoFim);
            }
        }

        function _adiantarEm(milisegundos, legenda, config) {
            config = _completarConfig(config);

            if (config.inicio) {
                var tempoInicio = legenda.tempoInicioMs - milisegundos;
                if(tempoInicio < 0)
                    tempoInicio = 0;
                legenda.atualizarTempoInicio(tempoInicio);

                // ajustando os índices
                var idOriginal = legenda.id;
                for (var id = idOriginal - 1; id >= 1; id--) {
                    var legendaAnterior = _dicionario[id];
                    if(legenda.tempoInicioMs < legendaAnterior.tempoInicioMs){
                       _swapIds(legenda, legendaAnterior);
                    }
                    else 
                        break;
                };
            }

            if (config.fim) {
                var tempoFim = legenda.tempoFimMs - milisegundos;
                if(tempoFim < 0)
                    tempoFim = 0;
                legenda.atualizarTempoFim(tempoFim);
            }
        }

        function _completarConfig(config) {
            if(!config) {
                return {
                    inicio: true, 
                    fim: true
                }
            }

            return {
                inicio: ('inicio' in config) ? config.inicio : true, 
                fim: ('fim' in config) ? config.fim : true
            };
        }

        function _swapIds(legenda1, legenda2) {
            //swap ids                    
            var temp = legenda1.id;
            legenda1.id = legenda2.id;
            legenda2.id = temp;

            //atualizando o dicionário
            _dicionario[legenda1.id] = legenda1;
            _dicionario[legenda2.id] = legenda2;
        }
    }

    return EditorLegendas;
})();