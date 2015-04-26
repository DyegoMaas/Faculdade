namespace Entregas.Core
{
    public class Produto
    {
        public string Nome { get; private set; }

        public Produto(string nome)
        {
            Nome = nome;
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}