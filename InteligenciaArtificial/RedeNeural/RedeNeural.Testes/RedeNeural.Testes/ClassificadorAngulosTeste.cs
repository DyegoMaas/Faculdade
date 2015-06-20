using FluentAssertions;
using NUnit.Framework;
using RedeNeural.Core.Classificacao;
using RedeNeural.Core.Classificacao.Entradas;

namespace RedeNeural.Testes
{
    public class ClassificadorAngulosTeste
    {
        [TestCase(0, 5)]
        [TestCase(90, 5)]
        [TestCase(91, 2)]
        [TestCase(120, 2)]
        [TestCase(121, 1)]
        [TestCase(150, 1)]
        [TestCase(180, 1)]
        public void configurando_as_entradas_(int angulo, int classificacaoEsperada)
        {
            var classificadorAngulos = ClassificadorAngulos.Abordagem1();

            var classificacao = classificadorAngulos.Classificar(angulo);

            classificacao.Should().Be(classificacaoEsperada);
        }
    }
}