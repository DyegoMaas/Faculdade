using Nancy;

namespace ApresentacaoNancy.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ => Response.AsFile("Static/index.html");
        }
    }
}