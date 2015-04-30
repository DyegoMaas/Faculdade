using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

namespace ApresentacaoNancy
{
    public class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "ApresentacaoNancy";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "ApresentacaoNancy";
        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }
}