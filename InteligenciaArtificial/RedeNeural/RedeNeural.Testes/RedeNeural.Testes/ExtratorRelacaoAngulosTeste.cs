﻿using FluentAssertions;
using NUnit.Framework;
using RedeNeural.Core.Classificacao.Entradas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RedeNeural.Testes
{
    public class ExtratorRelacaoAngulosTeste
    {
        private ExtratorRelacaoAngulos extratorRelacaoAngulos;

        private class TrianguloAmostralParaTeste
        {
            public TrianguloAmostral Triangulo { get; private set; }
            public int AnguloEsperado { get; private set; }

            public TrianguloAmostralParaTeste(TrianguloAmostral triangulo, int anguloEsperado)
            {
                Triangulo = triangulo;
                AnguloEsperado = anguloEsperado;
            }
        }

        [SetUp]
        public void SetUp()
        {
            extratorRelacaoAngulos = new ExtratorRelacaoAngulos();
        }

        [TestCase(1,1, 1,0, 45)]
        [TestCase(2,1, 2,0, 63)]
        public void calculando_os_angulos_internos_de_uma_forma_geometrica(int x1, int y1, int x2, int y2, int anguloEsperado)
        {
            var zero = new Vector2();
            IList<TrianguloAmostralParaTeste> paresParaTeste = new[]
            {
                new TrianguloAmostralParaTeste(new TrianguloAmostral(zero, new Vector2(x1, y1), new Vector2(x2, y2)), anguloEsperado)
            };

            var pares = paresParaTeste.Select(p => p.Triangulo).ToList();
            IList<int> angulosInternos = extratorRelacaoAngulos.ExtrairRelacaoAngulos(pares);

            angulosInternos.Should().HaveSameCount(paresParaTeste);
            angulosInternos[0].Should().Be(anguloEsperado);
        }
        
        [Test]
        public void calculando_o_angulo_entre_dois_pontos_iguais()
        {
            var parAmostralComPontosIguais = new TrianguloAmostral(new Vector2(), new Vector2(), new Vector2());

            Action acaoExtrairRelacaoAngulos = () => extratorRelacaoAngulos.ExtrairRelacaoAngulos(new[] { parAmostralComPontosIguais });

            acaoExtrairRelacaoAngulos.ShouldThrow<Exception>().WithMessage("Não é possível calcular o ângulo entre dois pontos iguais");
        }
    }
}
