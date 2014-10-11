using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClienteTCP : MonoBehaviour {
    
    public string hostname;
    public int porta;

    private const char CaractereFimMensagem = '\r';

    public string EnviarMensagem(string mensagem)
    {
        using (var tcpClient = new TcpClient())
        {
            Debug.Log("Conectando via TCP...");

            tcpClient.Connect(hostname, porta);

            Debug.Log("Conectado");
            var stream = tcpClient.GetStream();
            var byteArray = Encoding.ASCII.GetBytes(mensagem);
            Debug.Log(string.Format("Transmitindo {0}...", mensagem));

            stream.Write(byteArray, 0, byteArray.Length);

            using (var memStream = new MemoryStream())
            {
                while (true)
                {
                    var @byte = (byte)stream.ReadByte();
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
