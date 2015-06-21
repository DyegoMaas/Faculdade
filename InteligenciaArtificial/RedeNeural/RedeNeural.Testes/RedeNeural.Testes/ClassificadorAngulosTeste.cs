using FluentAssertions;
using NUnit.Framework;
using RedeNeural.Core.Classificacao.Entradas;

namespace RedeNeural.Testes
{
    public class ClassificadorAngulosTeste
    {
        [TestCase(0, 5)]
        [TestCase(89, 5)]
        [TestCase(90, 2)]
        [TestCase(119, 2)]
        [TestCase(120, 1)]
        [TestCase(180, 1)]
        public void configurando_as_entradas_(int angulo, int classificacaoEsperada)
        {
            var classificadorAngulos = ClassificadorAngulos.Abordagem1();

            var classificacao = classificadorAngulos.Classificar(angulo);

            classificacao.Should().Be(classificacaoEsperada);
        }
    }
}