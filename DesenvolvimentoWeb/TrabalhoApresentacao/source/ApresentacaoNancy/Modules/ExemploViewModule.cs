using Core.Repositorios;
using Dominio.Pessoas;
using Nancy;

namespace ApresentacaoNancy.Modules
{
    public class ExemploViewModule : NancyModule
    {
        public ExemploViewModule(IRepositorio repositorio)
            : base("/index")
        {
            Get["/"] = _ =>
            {
                return View["Index", new
                {
                    Valor = "a",
                    Quantidade = 1000
                }];
            };

            Get["/pessoas"] = _ =>
            {
                var pessoas = repositorio.ObterTodos<Pessoa>();
                return View["Pessoas", new { Pessoas = pessoas }];
            };
        }
    }
}