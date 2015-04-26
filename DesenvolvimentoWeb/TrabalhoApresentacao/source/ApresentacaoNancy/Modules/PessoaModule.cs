using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;

namespace ApresentacaoNancy.Modules
{
    public class PessoaModule : NancyModule
    {
        static readonly List<Pessoa> Pessoas = new List<Pessoa>
        {
            new Pessoa
            {
                Id = 1,
                Cpf = 7172233964,
                Nome = "Dyego"
            },
            new Pessoa
            {
                Id = 2,
                Cpf = 67533777352,
                Nome = "Maria"
            },
            new Pessoa
            {
                Id = 3,
                Cpf = 48725773968,
                Nome = "João"
            }
        };

        public PessoaModule() : base("/pessoas")
        {
            Get["/"] = _ => Pessoas;

            Get["/{id:long}"] = _ =>
            {
                var id = (long)_.id;
                var pessoa = Pessoas.FirstOrDefault(p => p.Id == id);
                if(pessoa == null)
                    throw new Exception(string.Format("Pessoa com id {0} não encontrada.", id));

                return pessoa;
            };

            Post["/"] = _ =>
            {
                var pessoa = this.BindAndValidate<Pessoa>();
                pessoa.Id = Pessoas.Max(p => p.Id + 1);

                Pessoas.Add(pessoa);
                return true;
            };

            Put["/{id:long}"] = _ =>
            {
                var id = (long)_.id;
                var pessoa = Pessoas.FirstOrDefault(p => p.Id == id);
                if (pessoa == null)
                    throw new Exception(string.Format("Pessoa com id {0} não encontrada.", id));

                var model = this.BindAndValidate<Pessoa>();
                pessoa.Nome = model.Nome;
                pessoa.Cpf = model.Cpf;

                return true;
            };

            Delete["/{id:long}"] = _ =>
            {
                var id = (long)_.id;
                var pessoa = Pessoas.FirstOrDefault(p => p.Id == id);
                if (pessoa == null)
                    throw new Exception(string.Format("Pessoa com id {0} não encontrada.", id));

                Pessoas.Remove(pessoa);
                return true;
            };
        }
    }

    public class Pessoa
    {
        public long? Id { get; set; }
        public string Nome { get; set; }
        public long Cpf { get; set; }
    }
}