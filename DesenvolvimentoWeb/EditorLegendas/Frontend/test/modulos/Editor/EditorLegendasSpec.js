describe("--> Editor de legendas", function() {

	var listaLegendas = {
		lista: [],
		adicionar: function(legenda) {
			this.lista.push(legenda);
		},

		reiniciarLista: function () {
			this.lista = [];
		}
	};
	var editor = new EditorLegendas(listaLegendas);

	beforeEach(function () {
		listaLegendas.reiniciarLista();
	});

	describe("-> Adicionando uma legenda", function () {

		it("-> ", function () {
			var novaLegenda = new LegendaViewModel(1, 0, 990, '00:00:00,000', '00:00:00,990', 'Texto 1');
			var novaLegenda2 = new LegendaViewModel(2, 1990, 2990, '00:00:02,000', '00:00:02,990', 'Texto 2');

			editor.adicionar(novaLegenda);
			editor.adicionar(novaLegenda2);

			expect(legendaNoIndice(0)).toBe(novaLegenda);
			expect(legendaNoIndice(1)).toBe(novaLegenda2);
		})
	});

	describe("-> Edição em lote de legendas:", function () {

		beforeEach(function () {
			editor.adicionar(new LegendaViewModel(1, 0, 990, '00:00:00,000', '00:00:00,990', 'Texto 1'));
			editor.adicionar(new LegendaViewModel(2, 1990, 2990, '00:00:02,000', '00:00:02,990', 'Texto 2'));
			editor.adicionar(new LegendaViewModel(3, 49990, 3599990, '00:00:50,000', '00:59:59,990', 'Texto 3'));
		});

		it('Atrasando as legendas em lote', function () {
			var milisegundos = 10;
			editor.atrasarTodasEm(milisegundos);

			expect(legendaNoIndice(0).tempoInicioMs).toBe(10);
			expect(legendaNoIndice(0).tempoFimMs).toBe(1000);
			expect(legendaNoIndice(0).tempoInicio).toBe('00:00:00,010');
			expect(legendaNoIndice(0).tempoFim).toBe('00:00:01,000');

			expect(legendaNoIndice(1).tempoInicioMs).toBe(2000);
			expect(legendaNoIndice(1).tempoFimMs).toBe(3000);
			expect(legendaNoIndice(1).tempoInicio).toBe('00:00:02,000');
			expect(legendaNoIndice(1).tempoFim).toBe('00:00:03,000');

			expect(legendaNoIndice(2).tempoInicioMs).toBe(50000);
			expect(legendaNoIndice(2).tempoFimMs).toBe(3600000);
			expect(legendaNoIndice(2).tempoInicio).toBe('00:00:50,000');
			expect(legendaNoIndice(2).tempoFim).toBe('01:00:00,000');
		});

		it('Adiantando as legendas em lote', function () {
			var milisegundos = 990;
			editor.adiantarTodasEm(milisegundos);

			expect(legendaNoIndice(0).tempoInicioMs).toBe(0);
			expect(legendaNoIndice(0).tempoFimMs).toBe(0);
			expect(legendaNoIndice(0).tempoInicio).toBe('00:00:00,000');
			expect(legendaNoIndice(0).tempoFim).toBe('00:00:00,000');

			expect(legendaNoIndice(1).tempoInicioMs).toBe(1000);
			expect(legendaNoIndice(1).tempoFimMs).toBe(2000);
			expect(legendaNoIndice(1).tempoInicio).toBe('00:00:01,000');
			expect(legendaNoIndice(1).tempoFim).toBe('00:00:02,000');

			expect(legendaNoIndice(2).tempoInicioMs).toBe(49000);
			expect(legendaNoIndice(2).tempoFimMs).toBe(3599000);
			expect(legendaNoIndice(2).tempoInicio).toBe('00:00:49,000');
			expect(legendaNoIndice(2).tempoFim).toBe('00:59:59,000');
		});
	});

	describe('--> Edição de uma legenda', function () {

		beforeEach(function () {
			editor.adicionar(new LegendaViewModel(1, 1990, 2990, '00:00:02,000', '00:00:02,990', 'Texto 1'));
			editor.adicionar(new LegendaViewModel(2, 49990, 3599990, '00:00:50,000', '00:59:59,990', 'Texto 2'));
			editor.adicionar(new LegendaViewModel(3, 3600000, 3601000, '01:00:00,000', '01:00:01,000', 'Texto 3'));
		});

		it('--> Adiantando uma legenda', function () {
			var milisegundos = 990;
			
			editor.adiantar(legendaNoIndice(1), milisegundos);

			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(0), 1990, 2990);
			deveTerAtualizadoOsTemposDaLegenda(legendaNoIndice(1), 49000, 3599000);
			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(2), 3600000, 3601000);
		});

		it('--> Ajustando os ids das legendas ao adiantar uma legenda', function () {
			var milisegundos = legendaNoIndice(2).tempoInicioMs;

			var legendaEditada = legendaNoIndice(2);
			editor.adiantar(legendaEditada, milisegundos);

			deveTerAtualizadoOIdDaLegenda(legendaNoIndice(2), 1);
			deveTerAtualizadoOIdDaLegenda(legendaNoIndice(0), 2);
			deveTerAtualizadoOIdDaLegenda(legendaNoIndice(1), 3);
		});

		it('--> Adiantando o tempo inicial de uma legenda', function () {
			var milisegundos = 990;

			editor.adiantar(legendaNoIndice(1), milisegundos, { fim: false });

			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(0), 1990, 2990);
			deveTerAtualizadoOsTemposDaLegenda(legendaNoIndice(1), 49000, 3599990);
			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(2), 3600000, 3601000);
		});

		it('--> Adiantando o tempo final de uma legenda', function () {
			var milisegundos = 990;

			editor.adiantar(legendaNoIndice(1), milisegundos, { inicio: false });

			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(0), 1990, 2990);
			deveTerAtualizadoOsTemposDaLegenda(legendaNoIndice(1), 49990, 3599000);
			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(2), 3600000, 3601000);
		});

		it('--> Atrasando uma legenda', function () {
			var milisegundos = 10;

			editor.atrasar(legendaNoIndice(1), milisegundos);

			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(0), 1990, 2990);
			deveTerAtualizadoOsTemposDaLegenda(legendaNoIndice(1), 50000, 3600000);
			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(2), 3600000, 3601000);
		});

		it('--> Ajustando os ids das legendas ao atrasar uma legenda', function () {
			var milisegundos = legendaNoIndice(2).tempoInicioMs;

			var legendaEditada = legendaNoIndice(0);
			editor.atrasar(legendaEditada, milisegundos);

			deveTerAtualizadoOIdDaLegenda(legendaNoIndice(1), 1);
			deveTerAtualizadoOIdDaLegenda(legendaNoIndice(2), 2);
			deveTerAtualizadoOIdDaLegenda(legendaNoIndice(0), 3);
		});

		it('--> Atrasando o tempo inicial uma legenda', function () {
			var milisegundos = 10;

			editor.atrasar(legendaNoIndice(1), milisegundos, { fim: false });

			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(0), 1990, 2990);
			deveTerAtualizadoOsTemposDaLegenda(legendaNoIndice(1), 50000, 3599990);
			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(2), 3600000, 3601000);
		});

		it('--> Atrasando o tempo final de uma legenda', function () {
			var milisegundos = 10;

			editor.atrasar(legendaNoIndice(1), milisegundos, { inicio: false });

			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(0), 1990, 2990);
			deveTerAtualizadoOsTemposDaLegenda(legendaNoIndice(1), 49990, 3600000);
			naoDeveTerAlteradoOsTemposDaLegenda(legendaNoIndice(2), 3600000, 3601000);
		});

		it('--> Excluindo uma legenda', function () {
			var legendaParaExcluir = legendaNoIndice(0);
			
			editor.excluir(legendaParaExcluir);

			deveTerExcluidoALegenda(legendaParaExcluir);
			deveTerAtualizadoOIdDaLegenda(legendaNoIndice(0), 1);
			deveTerAtualizadoOIdDaLegenda(legendaNoIndice(1), 2);			
		});

		function naoDeveTerAlteradoOsTemposDaLegenda(legenda, tempoInicioMsEsperado, tempoFimMsEsperado) {
			expect(legenda.tempoInicioMs).toBe(tempoInicioMsEsperado);
			expect(legenda.tempoFimMs).toBe(tempoFimMsEsperado);
		}

		function deveTerAtualizadoOsTemposDaLegenda(legenda, novoTempoInicioMs, novoTempoFimMs) {
			expect(legenda.tempoInicioMs).toBe(novoTempoInicioMs);
			expect(legenda.tempoFimMs).toBe(novoTempoFimMs);			
		}

		function deveTerExcluidoALegenda(legenda) {
			expect(listaLegendas.lista.length).toBe(2);
			expect(listaLegendas.lista.indexOf(legenda)).toBe(-1);
		}

		function deveTerAtualizadoOIdDaLegenda(legenda, novoId) {
			expect(legenda.id).toBe(novoId);
			expect(legenda.id).toBe(novoId);
		}
	});

	describe('--> Obtendo legendas', function () {

		beforeEach(function () {
			editor.adicionar(new LegendaViewModel(1, 1990, 2990, '00:00:02,000', '00:00:02,990', 'Texto 1'));
			editor.adicionar(new LegendaViewModel(2, 49990, 3599990, '00:00:50,000', '00:59:59,990', 'Texto 2'));
			editor.adicionar(new LegendaViewModel(3, 3600000, 3601000, '01:00:00,000', '01:00:01,000', 'Texto 3'));
		});

		it('--> Obtendo a legenda anterior quando existe', function () {
			var legendaAtual = legendaNoIndice(1);

			var legendaAnterior = editor.obterLegendaAnterior(legendaAtual);

			expect(legendaAnterior).toBe(legendaNoIndice(0));
		});

		it('--> Obtendo a legenda anterior quando não existe', function () {
			var legendaAtual = legendaNoIndice(0);

			var legendaAnterior = editor.obterLegendaAnterior(legendaAtual);

			expect(legendaAnterior).toBe(null);
		});

		it('--> Obtendo a próxima legenda quando existe', function () {
			var legendaAtual = legendaNoIndice(1);

			var legendaAnterior = editor.obterLegendaPosterior(legendaAtual);

			expect(legendaAnterior).toBe(legendaNoIndice(2));
		});

		it('--> Obtendo a próxima legenda quando não existe', function () {
			var legendaAtual = legendaNoIndice(2);

			var legendaAnterior = editor.obterLegendaPosterior(legendaAtual);

			expect(legendaAnterior).toBe(null);
		});
	});

	function legendaNoIndice(indice) {
		return listaLegendas.lista[indice];
	}
});