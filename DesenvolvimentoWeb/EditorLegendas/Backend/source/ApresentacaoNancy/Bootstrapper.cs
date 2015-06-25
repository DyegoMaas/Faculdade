using Core;
using log4net;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Responses;
using Nancy.Responses.Negotiation;
using Nancy.TinyIoc;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace ApresentacaoNancy
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(x =>
                {
                    x.ResponseProcessors = new[]
                    {
                        typeof(JsonProcessor)
                    };
                });
            }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            ConfigurarIoCContainer(container);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
        }

        private void ConfigurarIoCContainer(TinyIoCContainer container)
        {
            TinyIoCInstaller.Instalar(container);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                return null;
            });
            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx => ConfigurarCabecalhos(ctx.Response));



            pipelines.OnError.AddItemToEndOfPipeline((ctx, excecao) =>
            {
                log.Error(excecao.Message, excecao);

                Response response;
                if (excecao is ErroNegocioException)
                {
                    var resultadoOperacao = ResultadoOperacao.ComErros(excecao.Message);
                    response = new JsonResponse(resultadoOperacao, new DefaultJsonSerializer());
                }
                else
                {
                    response = new Response
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ReasonPhrase = excecao.Message
                    };
                }

                ConfigurarCabecalhos(response);

                return response;
            });
        }

        private void ConfigurarCabecalhos(Response response)
        {
            response
                .WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE")
                .WithHeader("Access-Control-Allow-Headers", ObterParametrosPermitidosNoCabecalho());
        }

        private string ObterParametrosPermitidosNoCabecalho()
        {
            var parametrosFixos = new[]
            {
                "Accept",
                "Origin",
                "Content-type"
            };
            return string.Join(", ", parametrosFixos);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            var tokenizer = container.Resolve<ITokenizer>();
            TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(tokenizer));
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/", @"static"));
        }
    }
}