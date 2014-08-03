using System.Linq;

namespace SimuladorSGBD.Testes.Fixtures
{
    public static class ConteudoPaginaTesteHelper
    {
        public static char[] NovoConteudo(int numeroCaracteres, char caractere)
        {
            return Enumerable.Repeat(caractere, numeroCaracteres).ToArray();
        }
    }
}