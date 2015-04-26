using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace JogoCartas21.Core.IO
{
    public class ClienteTCP
    {
        private const char CaractereFimMensagem = '\r';
        private readonly string hostname;
        private readonly int porta;

        public ClienteTCP(string hostname, int porta)
        {
            this.hostname = hostname;
            this.porta = porta;
        }

        public string EnviarMensagem(string mensagem)
        {
            using (var tcpClient = new TcpClient())
            {
                Console.WriteLine("Conectando...");

                tcpClient.Connect(hostname, porta);

                Console.WriteLine("Conectado");
                var stream = tcpClient.GetStream();
                var byteArray = Encoding.ASCII.GetBytes(mensagem);
                Console.WriteLine("Transmitindo {0}...", mensagem);

                stream.Write(byteArray, 0, byteArray.Length);

                using (var memStream = new MemoryStream())
                {
                    while (true)
                    {
                        var @byte = (byte) stream.ReadByte();
                        if (Convert.ToChar(@byte) == CaractereFimMensagem)
                            break;

                        memStream.WriteByte(@byte);
                    }

                    var bytesRecebidos = memStream.ToArray();
                    if (bytesRecebidos.Any())
                        return Encoding.UTF8.GetString(bytesRecebidos);

                    return string.Empty;
                }
            }
        }
    }
}