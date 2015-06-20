using FluentAssertions;
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

        [Test]
        public void calculando_os_angulos_internos_de_uma_forma_geometrica()
        {
            var zero = new Vector2();
            IList<TrianguloAmostralParaTeste> paresParaTeste = new[]
            {
                new TrianguloAmostralParaTeste(new TrianguloAmostral(zero, new Vector2(1, 1), new Vector2(1, 0)), anguloEsperado:45),
                //new TrianguloAmostralParaTeste(new TrianguloAmostral(zero, new Vector2(0, 0), new Vector2(0, 1)), anguloEsperado:90),
                //new TrianguloAmostralParaTeste(new TrianguloAmostral(zero, new Vector2(0, 0), new Vector2(-1, 1)), anguloEsperado:135),
                //new TrianguloAmostralParaTeste(new TrianguloAmostral(zero, new Vector2(0, 0), new Vector2(-1, 0)), anguloEsperado:180),
                //new TrianguloAmostralParaTeste(new TrianguloAmostral(zero, new Vector2(0, 0), new Vector2(-1, -1)), anguloEsperado:225), //225
                //new TrianguloAmostralParaTeste(new TrianguloAmostral(zero, new Vector2(0, 0), new Vector2(0, -1)), anguloEsperado:270), //270
                //new TrianguloAmostralParaTeste(new TrianguloAmostral(zero, new Vector2(0, 0), new Vector2(1, -1)), anguloEsperado:315) //315
            };

            var pares = paresParaTeste.Select(p => p.Triangulo).ToList();
            IList<int> angulosInternos = extratorRelacaoAngulos.ExtrairRelacaoAngulos(pares);

            angulosInternos.Should().HaveSameCount(paresParaTeste);
            for (var i = 0; i < paresParaTeste.Count; i++)
            {
                var anguloEsperado = paresParaTeste[i].AnguloEsperado;
                angulosInternos[i].Should().Be(anguloEsperado);
            }
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
