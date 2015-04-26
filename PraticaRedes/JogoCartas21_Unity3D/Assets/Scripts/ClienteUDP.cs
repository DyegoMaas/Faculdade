using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClienteUDP : MonoBehaviour {
    
    public string hostname;
    public int porta;

    public void EnviarMensagem(string mensagem)
    {
        var udpClient = new UdpClient();

        Debug.Log(string.Format("Transmitindo via UDP: {0}...", mensagem));
        var bytes = Encoding.ASCII.GetBytes(mensagem);
        udpClient.Send(bytes, bytes.Length, hostname, porta);
    }
}
