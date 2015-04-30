using Nancy;

namespace ApresentacaoNancy.Modules
{
    public class ExemploViewModule : NancyModule
    {
        public ExemploViewModule()
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
        }
    }
}