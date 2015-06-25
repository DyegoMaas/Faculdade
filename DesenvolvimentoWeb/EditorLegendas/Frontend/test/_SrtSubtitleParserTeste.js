'use strict';

var should = require("should");
var SrtSubtitleParser = require("../app/modulos/Editor/src/SrtSubtitleParser.js");

suite('Parser de legendas SRT', function () {
	var parser;
	suiteSetup(function () {
		console.log(SrtSubtitleParser);
		// parser = new SrtSubtitleParser();
		parser = SrtSubtitleParser;
	});

	// setup(function () {		
	// });

	test('fazendo uma tarefa importante', function () {

		var legenda = hereDoc(function() {
		/*!
		1
		00:00:59,476 --> 00:01:04,138
		<b>O PADRINHO</b>
		Nomeado para 11 Óscars - Vencedor de 3 Óscars
		*/});

		var json = parser.fromSrt(legenda);		
	});

	test('fazendo outra tarefa menos importante', function () {
		(1).should.be.exactly(2);
	});	

	teardown(function () {
		console.log('teardown');
	});

	suiteTeardown(function () {
		console.log('suite teardown');
	});

	function hereDoc(f) {
	  return f.toString().
	      replace(/^[^\/]+\/\*!?/, '').
	      replace(/\*\/[^\/]+$/, '');
	}
});