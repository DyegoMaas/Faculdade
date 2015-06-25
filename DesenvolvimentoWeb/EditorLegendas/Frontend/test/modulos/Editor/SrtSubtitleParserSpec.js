describe("-> Parser de legendas SRT", function() {
	describe("-> Convertendo texto para JSON:", function () {

		var parser = new SrtSubtitleParser(LegendaViewModel);

		it('Convertendo uma fala da legenda para JSON', function () {
			var textoLegenda = hereDoc(function() {
/*!
1
00:00:59,476 --> 00:01:04,138
<b>O PADRINHO</b>
Nomeado para 11 Óscars - Vencedor de 3 Óscars
*/
			});

			var legenda = parser.fromSrt(textoLegenda)[0];

			expect(legenda.id).toBe(1);
			expect(legenda.tempoInicio).toBe('00:00:59,476');
			expect(legenda.tempoInicioMs).toBe(59476);
			expect(legenda.tempoFim).toBe('00:01:04,138');
			expect(legenda.tempoFimMs).toBe(64138);
			expect(legenda.texto).toContain('<b>O PADRINHO</b>');
			expect(legenda.texto).toContain('Nomeado para 11 Óscars - Vencedor de 3 Óscars');
		});
	});

	function hereDoc(f) {
        return f.toString().
            replace(/^[^\/]+\/\*!?/, '').
            replace(/\*\/[^\/]+$/, '');
    }
});