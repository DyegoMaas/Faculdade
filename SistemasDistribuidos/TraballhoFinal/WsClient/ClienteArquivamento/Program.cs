using System;
using System.IO;
using System.Threading.Tasks;

namespace ClienteArquivamento
{
    class Program
    {
        private static readonly ServicoArquivamentoService Servico = new ServicoArquivamentoService();

        static void Main(string[] args)
        {
            var resultado = VersaoAssincrona();
            Console.WriteLine(resultado.Result);

            Console.Read();
        }

        private static void VersaoSincrona()
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
        }

        private static async Task<string> VersaoAssincrona()
        {
            var token = Servico.autenticarUsuario("vader", "luke");

            const string nomeArquivoImagem = "arquivo.jpg";
            var grupoTasks1 = new[]
            {
                new Action(() => Console.WriteLine(Servico.criarPasta("pastinha", token))),
                new Action(() => Console.WriteLine(Servico.criarPasta("pastinha2", token))),
                new Action(() =>
                {
                    Servico.uploadArquivo(ArquivoImagemEmBase64(), nomeArquivoImagem, token);
                    Console.WriteLine("Arquivo {0} enviado com sucesso!", nomeArquivoImagem);
                })
            };

            var grupoTasks2 = new[]
            {
                new Action(() =>
                {
                    var imagemBase64 = Servico.downloadArquivo(nomeArquivoImagem, token);
                    Console.WriteLine("Arquivo {0} baixado com sucesso!", nomeArquivoImagem);

                    var bytesBaixados = Convert.FromBase64String(imagemBase64);
                    SalvarImagem(bytesBaixados);
                })
            };

            await Task.WhenAll(Task.Run(grupoTasks1[0]), Task.Run(grupoTasks1[1]), Task.Run(grupoTasks1[2]));
            await Task.WhenAll(Task.Run(grupoTasks2[0]));

            return "sucesso";
        }

        static string ArquivoImagemEmBase64()
        {
            var arquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagem", "wallpaper.jpg");
            var allBytes = File.ReadAllBytes(arquivo);
            return Convert.ToBase64String(allBytes);
        }

        private static void SalvarImagem(byte[] bytesBaixados)
        {
            var caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagem", "wallpaper_baixada.jpg");
            File.WriteAllBytes(caminhoArquivo, bytesBaixados);

            Console.WriteLine("Arquivo salvo em {0}", caminhoArquivo);
        }
    }
}
