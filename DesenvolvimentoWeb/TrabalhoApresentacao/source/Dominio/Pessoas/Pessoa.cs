using Core;
using Core.Entidades;

namespace Dominio.Pessoas
{
    public class Pessoa : Entidade
    {
        public virtual string Nome { get; set; }
        public virtual long Cpf { get; set; }
    }
}