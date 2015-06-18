using System;   
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RedeNeural.Classificacao;

namespace RedeNeural.Testes
{
    public class UtilsTeste
    {
        private class ParAmostralParaTeste
        {
            public ParAmostral Par { get; private set; }
            public int AnguloEsperado { get; private set; }

            public ParAmostralParaTeste(ParAmostral par, int anguloEsperado)
            {
                Par = par;
                AnguloEsperado = anguloEsperado;
            }
        }

        [Test]
        public void calculando_os_angulos_internos_de_uma_forma_geometrica()
        {
            IList<ParAmostralParaTeste> paresParaTeste = new[]
            {
                new ParAmostralParaTeste(new ParAmostral(new Vector2(0, 0), new Vector2(1, 0)), anguloEsperado:0),
                new ParAmostralParaTeste(new ParAmostral(new Vector2(0, 0), new Vector2(1, 1)), anguloEsperado:45),
                new ParAmostralParaTeste(new ParAmostral(new Vector2(0, 0), new Vector2(0, 1)), anguloEsperado:90),
                new ParAmostralParaTeste(new ParAmostral(new Vector2(0, 0), new Vector2(-1, 1)), anguloEsperado:135),
                new ParAmostralParaTeste(new ParAmostral(new Vector2(0, 0), new Vector2(-1, 0)), anguloEsperado:180),
                new ParAmostralParaTeste(new ParAmostral(new Vector2(0, 0), new Vector2(-1, -1)), anguloEsperado:225),
                new ParAmostralParaTeste(new ParAmostral(new Vector2(0, 0), new Vector2(0, -1)), anguloEsperado:270),
                new ParAmostralParaTeste(new ParAmostral(new Vector2(0, 0), new Vector2(1, -1)), anguloEsperado:315)
            };

            var pares = paresParaTeste.Select(p => p.Par).ToList();
            IList<int> angulosInternos = Utils.ExtrairRelacaoAngulos(pares);

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
            var parAmostralComPontosIguais = new ParAmostral(new Vector2(), new Vector2());

            Action acaoExtrairRelacaoAngulos = () => Utils.ExtrairRelacaoAngulos(new[] {parAmostralComPontosIguais});

            acaoExtrairRelacaoAngulos.ShouldThrow<Exception>().WithMessage("Não é possível calcular o ângulo entre dois pontos iguais");
        }
    }
}
