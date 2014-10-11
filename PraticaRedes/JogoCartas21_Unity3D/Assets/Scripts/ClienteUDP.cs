using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClienteUDP : MonoBehaviour {
    
    private readonly string hostname;
    private readonly int porta;

    public ClienteUDP(string hostname, int porta)
    {
        this.hostname = hostname;
        this.porta = porta;
    }

    public void EnviarMensagem(string mensagem)
    {
        var udpClient = new UdpClient();

        var bytes = Encoding.ASCII.GetBytes(mensagem);
        udpClient.Send(bytes, bytes.Length, hostname, porta);
    }
}
