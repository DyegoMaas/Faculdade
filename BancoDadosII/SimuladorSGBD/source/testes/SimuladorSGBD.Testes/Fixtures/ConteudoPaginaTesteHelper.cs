using System.Linq;
using System.Text;

namespace SimuladorSGBD.Testes.Fixtures
{
    public class ConteudoPaginaTesteHelper
    {
        public byte ToByte(char caractere)
        {
            return Encoding.UTF8.GetBytes(new[] {caractere})[0];
        }

        public byte[] ToByteArray(string conteudo)
        {
            return Encoding.UTF8.GetBytes(conteudo);
        }

        public byte[] ToByteArray(char[] conteudo)
        {
            return Encoding.UTF8.GetBytes(conteudo);
        }

        public byte[] NewByteArray(int numeroCaracteres, char caractere)
        {
            var bytes = Enumerable.Repeat(caractere, numeroCaracteres).ToArray();
            return Encoding.UTF8.GetBytes(bytes);
        }

        public byte[] NovoConteudo(int numeroCaracteres, char caractere)
        {
            return NewByteArray(numeroCaracteres, caractere);
        }
    }
}