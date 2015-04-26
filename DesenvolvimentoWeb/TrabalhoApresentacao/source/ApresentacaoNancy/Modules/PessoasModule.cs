using System;
using System.Collections.Generic;
using System.Linq;
using Core.Repositorios;
using Core.UnitOfWork;
using Dominio.Pessoas;
using Nancy;
using Nancy.ModelBinding;

namespace ApresentacaoNancy.Modules
{
    public class PessoasModule : NancyModule
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

        public PessoasModule(IRepositorio repositorio) : base("/pessoas")
        {
            Get["/"] = _ => repositorio.ObterTodos<Pessoa>();

            Get["/{id:long}"] = _ =>
            {
                var id = (long)_.id;
                return repositorio.Obter<Pessoa>(id);
            };

            Post["/"] = _ =>
            {
                var pessoa = this.BindAndValidate<Pessoa>();

                using (var uow = new UnitOfWork())
                {
                    throw new Exception();
                    repositorio.SalvarOuAtualizar(pessoa);
                }

                return pessoa.Id;
            };

            Put["/{id:long}"] = _ =>
            {
                var id = (long)_.id;
                var model = this.BindAndValidate<Pessoa>();

                var pessoa = repositorio.Obter<Pessoa>(id);
                pessoa.Nome = model.Nome;
                pessoa.Cpf = model.Cpf;
                repositorio.SalvarOuAtualizar(pessoa);

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
}