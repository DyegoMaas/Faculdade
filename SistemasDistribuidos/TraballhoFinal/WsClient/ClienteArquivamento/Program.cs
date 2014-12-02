using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClienteArquivamento
{
    class Program
    {
        static void Main(string[] args)
        {
            var arquivamentoService = new ServicoArquivamentoService();
            arquivamentoService.autenticarUsuarioCompleted += arquivamentoService_autenticarUsuarioCompleted;

            Task<string> taskAutenticacao = Task<string>.Factory.FromAsync(arquivamentoService.BeginautenticarUsuario,
                arquivamentoService.EndautenticarUsuario, "vader", "luke", null);

            TaskAwaiter<string> taskAwaiter = taskAutenticacao.GetAwaiter();
            taskAwaiter.OnCompleted(() =>
            {
                Console.WriteLine("Resultado: "+taskAutenticacao.Result);
            });

            

            //taskAutenticacao.Start();

//            arquivamentoService.BeginautenticarUsuario("vader", "luke", e => { }, new object());
            //arquivamentoService.BeginautenticarUsuario("vader", "luke", e =>
            //{
            //    var func = e.AsyncState as Func<string, string, string>;
            //    string tokenX = func.EndInvoke(e);
            //    Console.WriteLine(tokenX);
            //}, new object());

            //Console.WriteLine("Token: " + token);

            Console.Read();

            //arquivamentoService.BegincriarPasta("pastinha", )
        }

        static void arquivamentoService_autenticarUsuarioCompleted(object sender, autenticarUsuarioCompletedEventArgs e)
        {
            Console.WriteLine("Token gerado: " + e.Result);
        }
    }
}
