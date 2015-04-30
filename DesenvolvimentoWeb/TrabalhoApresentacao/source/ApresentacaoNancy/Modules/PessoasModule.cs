using ApresentacaoNancy.Models;
using Core;
using Core.Repositorios;
using Core.UnitOfWork;
using Dominio.Pessoas;
using Nancy;
using Nancy.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace ApresentacaoNancy.Modules
{
    public class PessoasModule : NancyModule
    {
        public PessoasModule(IRepositorio repositorio, IUnitOfWorkFactory unitOfWorkFactory) 
            : base("/pessoas")
        {
            Get["/"] = _ => repositorio.ObterTodos<Pessoa>();

            Get["/{id:long}"] = _ =>
            {
                var id = (long)_.id;
                var pessoa = repositorio.Obter<Pessoa>(id);

                return ResultadoOperacao.ComSucesso(pessoa);
            };

            Post["/"] = _ =>
            {
                var model = this.BindAndValidate<PessoaModel>();
                if (!ModelValidationResult.IsValid)
                {
                    var erros = ObterErros();
                    return ResultadoOperacao.ComErros(erros.ToArray());
                }

                if (ExisteOutraPessoaComCpf(repositorio, model))
                {
                    throw new ErroNegocioException(string.Format("Já existe uma pessoa cadastrada com o CPF {0}", model.Cpf.Value));
                }

                using (var unitOfWork = unitOfWorkFactory.Criar())
                {
                    var pessoa = new Pessoa
                    {
                        Cpf = model.Cpf.Value,
                        Nome = model.Nome
                    };
                    unitOfWork.ExecutarEmTransacao(() => repositorio.SalvarOuAtualizar(pessoa));

                    return ResultadoOperacao.ComSucesso(pessoa.Id);
                }
            };

            Put["/{id:long}"] = _ =>
            {
                var id = (long)_.id;
                var model = this.BindAndValidate<PessoaModel>();
                if (!ModelValidationResult.IsValid)
                {
                    var erros = ObterErros();
                    return ResultadoOperacao.ComErros(erros.ToArray());
                }

                if (ExisteOutraPessoaComCpf(repositorio, model))
                {
                    throw new ErroNegocioException(string.Format("Já existe uma pessoa cadastrada com o CPF {0}", model.Cpf));
                }

                using (var unitOfWork = unitOfWorkFactory.Criar())
                {
                    var pessoa = repositorio.Obter<Pessoa>(id);
                    pessoa.Nome = model.Nome;
                    pessoa.Cpf = model.Cpf.Value;

                    unitOfWork.ExecutarEmTransacao(() => repositorio.SalvarOuAtualizar(pessoa));
                }

                return ResultadoOperacao.ComSucesso(true);
            };

            Delete["/{id:long}"] = _ =>
            {
                var id = (long)_.id;

                using (var unitOfWork = unitOfWorkFactory.Criar())
                {
                    unitOfWork.ExecutarEmTransacao(() => repositorio.Remover<Pessoa>(id));
                }

                return ResultadoOperacao.ComSucesso(true);
            };
        }

        private IEnumerable<string> ObterErros()
        {
            foreach (var erro in ModelValidationResult.Errors)
            {
                foreach (var modelValidationError in erro.Value)
                {
                    yield return modelValidationError.ErrorMessage;
                }
            }
        }

        private static bool ExisteOutraPessoaComCpf(IRepositorio repositorio, PessoaModel model)
        {
            var cpf = model.Cpf.Value;
            var pessoa = repositorio.BuscarUm<Pessoa>(p => p.Cpf == cpf);
            if (pessoa == null)
                return false;

            if (!model.Id.HasValue)
                return true;
            return model.Id.Value != pessoa.Id;
        }
    }
}