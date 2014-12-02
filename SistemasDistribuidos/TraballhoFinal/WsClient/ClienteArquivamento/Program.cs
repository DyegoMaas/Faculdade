using System;
using System.Collections.Generic;
using System.IO;
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

            var token = arquivamentoService.autenticarUsuario("vader", "luke");
            arquivamentoService.criarPasta("pastinha", token);
            arquivamentoService.criarPasta("pastinha2", token);
            arquivamentoService.uploadArquivo(ArquivoImagemEmBase64(), "arquivo.jpg", token);

            var diretorios = arquivamentoService.listarDiretorios(token);
            Console.WriteLine("Diretórios: {0}", diretorios);

            var arquivos = arquivamentoService.listarArquivos(token);
            Console.WriteLine("Arquivos: {0}", arquivos);

            var imagemBase64 = arquivamentoService.downloadArquivo("arquivo.jpg", token);
            var bytesBaixados = Convert.FromBase64String(imagemBase64);
            SalvarImagem(bytesBaixados);
            //Task<string> taskAutenticacao = Task<string>.Factory.FromAsync(arquivamentoService.BeginautenticarUsuario,
            //    arquivamentoService.EndautenticarUsuario, "vader", "luke", null);

            //TaskAwaiter<string> taskAwaiter = taskAutenticacao.GetAwaiter();
            //taskAwaiter.OnCompleted(() =>
            //{
            //    Console.WriteLine("Resultado: " + taskAutenticacao.Result);
                
            //});

            

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

        static string ArquivoImagemEmBase64()
        {
            var arquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagem", "wallpaper.jpg");
            var allBytes = File.ReadAllBytes(arquivo);
            return Convert.ToBase64String(allBytes);
        }

        private static void SalvarImagem(byte[] bytesBaixados)
        {
            var arquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagem", "wallpaper_baixada.jpg");
            File.WriteAllBytes(arquivo, bytesBaixados);
        }
    }
}
