using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using uPromise.Example.Http;

public class Jogo : MonoBehaviour
{
    public Text listaJogadoresAtivos;
    public Text pontos;
    public Text mensagens;
    public float intervaloAtualizacaoListaJogadores = 1f;
    private JogoCartas21 jogo;
    private Usuario usuario;

    private ClienteTCP clienteTCP;

	// Use this for initialization
	void Start ()
	{
	    clienteTCP = GetComponent<ClienteTCP>();
	    var clienteUDP = GetComponent<ClienteUDP>();
	    var loginManager = FindObjectOfType<LoginManager>();

	    var conectorJogo = new ConectorJogoCartas21(clienteTCP, clienteUDP);
	    var conectorChat = new ConectorChat(clienteTCP, clienteUDP);
		usuario = new Usuario(loginManager.userId, loginManager.senha);
	    jogo = new JogoCartas21(conectorJogo, conectorChat, usuario);

        EntrarJogo();
	    StartCoroutine(AtualizarJogadoresAtivos());
	}

    // Update is called once per frame
	void Update () {
	
	}

    void OnApplicationQuit()
    {
        SairJogo();
    }

    void EntrarJogo()
    {
        pontos.text = "0";
        jogo.EntrarNoJogo();
    }

    void PararJogo()
    {
        Debug.Log("Parando de jogar");
        jogo.PararDeJogar();
    }

    public Carta PegarCarta()
    {
        if (jogo.PossoPegarCarta())
        {
            var cartaAdquirida = jogo.PegarCarta();
            pontos.text = jogo.Pontuacao.ToString();
            return cartaAdquirida;
        }
        Debug.Log("não podia pegar cartas");
        return null;
    }

    private void SairJogo()
    {
        jogo.SairDoJogo();
    }

    private IEnumerator AtualizarJogadoresAtivos()
    {
        var atualizado = false;
        var jogadores = string.Empty;

        while (true)
        {
            if (!atualizado)
            {
                var thread = new Thread(() =>
                {
                    //TODO remover isso da versão final
                    clienteTCP.EnviarMensagem(string.Format("GET USERS {0}:{1}", usuario.UserId, usuario.Senha));

                    var stringBuilder = new StringBuilder();
                    var jogadoresAtivos = jogo.ObterJogadoresAtivos();
                    foreach (var jogador in jogadoresAtivos)
                    {
                        stringBuilder.AppendFormat("{0} - {1}", jogador.UserId, jogador.Status).AppendLine();
                    }
                    jogadores = stringBuilder.ToString();

                    atualizado = true;
                });
                thread.Start();
                yield return true;
            }
            else
            {
                listaJogadoresAtivos.text = jogadores;
                atualizado = false;
                yield return new WaitForSeconds(1f);   
            }
        }
    }
}

#region Jogo

public class JogoCartas21
{
    private readonly ConectorJogoCartas21 conectorJogo;
    private readonly ConectorChat conectorChat;
    private readonly Usuario usuario;
    private Player jogador;
    public int Pontuacao { get; private set; }
    public bool Ativo { get; private set; }

    public JogoCartas21(ConectorJogoCartas21 conectorJogo, ConectorChat conectorChat, Usuario usuario)
    {
        this.conectorJogo = conectorJogo;
        this.conectorChat = conectorChat;
        this.usuario = usuario;
    }

    public void EntrarNoJogo()
    {
        conectorJogo.EnviarComandoJogo(usuario, ComandosJogo.Enter);
        Pontuacao = 0;
        Ativo = true;
    }

    public void PararDeJogar()
    {
        conectorJogo.EnviarComandoJogo(usuario, ComandosJogo.Stop);
        Ativo = false;
    }

    public void SairDoJogo()
    {
        conectorJogo.EnviarComandoJogo(usuario, ComandosJogo.Quit);
        Pontuacao = 0;
        Ativo = false;
    }

    public IList<Player> ObterJogadoresAtivos()
    {
        var jogadoresAtivos = conectorJogo.GetPlayers(usuario).ToArray();
        jogador = jogadoresAtivos.FirstOrDefault(p => p.UserId == usuario.UserId);

        return jogadoresAtivos;
    }

    public bool PossoPegarCarta()
    {
        if (jogador == null)
            return false;

        return jogador.Status == PlayerStatus.GETTING;
    }

    public Carta PegarCarta()
    {
        if (!PossoPegarCarta())
            throw new InvalidOperationException();

        var carta = conectorJogo.GetCard(usuario);
        Pontuacao += carta.ValorCarta;

        return carta;
    }

    public IEnumerable<UsuarioChat> ObterUsuariosChat()
    {
        return conectorChat.GetUsers(usuario);
    }

    public void EnviarMensagemParaTodos(string mensagem)
    {
        var todos = new Usuario("0", string.Empty);
        EnviarMensagem(mensagem, destinatario:todos);
    }

    public void EnviarMensagem(string mensagem, Usuario destinatario)
    {
        conectorChat.EnviarMensagem(usuario, destinatario, mensagem);
    }
}

public class ConectorChat
{
    private readonly ClienteTCP clienteTcp;
    private readonly ClienteUDP clienteUdp;

    public ConectorChat(ClienteTCP clienteTcp, ClienteUDP clienteUdp)
    {
        this.clienteTcp = clienteTcp;
        this.clienteUdp = clienteUdp;
    }

    public IEnumerable<UsuarioChat> GetUsers(Usuario usuario)
    {
        var mensagem = "GET USERS {0}:{1}".FormatWith(usuario.UserId, usuario.Senha);
        var resposta = clienteTcp.EnviarMensagem(mensagem);

        var listaJogadores = ConverterUsuariosChat(resposta);
        return listaJogadores;
    }

    public MensagemChat GetMessage(Usuario usuario)
    {
        var mensagem = "GET MESSAGE {0}:{1}".FormatWith(usuario.UserId, usuario.Senha);
        var resposta = clienteTcp.EnviarMensagem(mensagem);

        return ConverterMensagem(resposta);
    }

    public void EnviarMensagem(Usuario remetente, Usuario destinatario, string mensagemChat)
    {
        var mensagem = "SEND MESSAGE {0}:{1}:{2}:{3}".FormatWith(remetente.UserId, remetente.Senha, destinatario.UserId, mensagemChat);
        clienteUdp.EnviarMensagem(mensagem);
    }

    private IEnumerable<UsuarioChat> ConverterUsuariosChat(string resposta)
    {
        resposta = resposta.TrimEnd('\n').TrimEnd(':');

        if (string.IsNullOrEmpty(resposta))
            return new UsuarioChat[0];

        var usuarios = new List<UsuarioChat>();
        var usuariosResposta = resposta.Split(':');
        for (int i = 0; i < usuariosResposta.Length; i += 3)
        {
            usuarios.Add(new UsuarioChat
            {
                UserId = usuariosResposta[i],
                UserName = usuariosResposta[i + 1],
                Wins = int.Parse(usuariosResposta[i + 2])
            });
        }
        return usuarios;
    }

    private MensagemChat ConverterMensagem(string resposta)
    {
        resposta = resposta.TrimEnd('\n').TrimEnd(':');

        if (string.IsNullOrEmpty(resposta))
            return null;

        var mensagemResposta = resposta.Split(':');
        var userId = mensagemResposta[0];
        return new MensagemChat
        {
            UserId = userId == "0" ? "Servidor" : userId,
            Mensagem = mensagemResposta[1],
        };
    }
}

public class ConectorJogoCartas21
{
    private readonly ClienteTCP clienteTcp;
    private readonly ClienteUDP clienteUdp;

    public ConectorJogoCartas21(ClienteTCP clienteTcp, ClienteUDP clienteUdp)
    {
        this.clienteTcp = clienteTcp;
        this.clienteUdp = clienteUdp;
    }

    public IEnumerable<Player> GetPlayers(Usuario usuario)
    {
        var mensagem = "GET PLAYERS {0}:{1}".FormatWith(usuario.UserId, usuario.Senha);
        var resposta = clienteTcp.EnviarMensagem(mensagem);

        var listaJogadores = ConverterJogadores(resposta);
        return listaJogadores;
    }

    public Carta GetCard(Usuario usuario)
    {
        var mensagem = "GET CARD {0}:{1}".FormatWith(usuario.UserId, usuario.Senha);
        var resposta = clienteTcp.EnviarMensagem(mensagem).Trim();

        if (resposta == ":")
            throw new InvalidOperationException("Não pode pegar");

        var partesResposta = resposta.Split(':');
        return new Carta(partesResposta[0], partesResposta[1].ToEnum<Naipe>());
    }

    public void EnviarComandoJogo(Usuario usuario, ComandosJogo comando)
    {
        var mensagem = "SEND GAME {0}:{1}:{2}".FormatWith(usuario.UserId, usuario.Senha, ConverterParaString(comando));
        clienteUdp.EnviarMensagem(mensagem);
    }

    private IEnumerable<Player> ConverterJogadores(string resposta)
    {
        resposta = resposta.TrimEnd('\n').TrimEnd(':');

        if (string.IsNullOrEmpty(resposta))
            return new Player[0];

        var players = new List<Player>();
        var partesResposta = resposta.Split(':');
        for (int i = 0; i < partesResposta.Length; i += 3)
        {
            players.Add(new Player
            {
                UserId = partesResposta[i],
                Status = partesResposta[i + 1].ToEnum<PlayerStatus>()
            });
        }
        return players;
    }

    private string ConverterParaString(ComandosJogo comando)
    {
        switch (comando)
        {
            case ComandosJogo.Enter: return "ENTER";
            case ComandosJogo.Quit: return "QUIT";
            default: return "STOP";
        }
    }
}

#endregion

#region Models

public class Carta
{
    private static readonly Dictionary<string, int> ValoresCarta = new Dictionary<string, int>
        {
            {"A", 1},
            {"2", 2},
            {"3", 3},
            {"4", 4},
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8},
            {"9", 9},
            {"10", 10},
            {"J", 10},
            {"Q", 10},
            {"K", 10}
        };

    public string Num { get; private set; }
    public Naipe Suit { get; private set; }

    public Carta(string num, Naipe suit)
    {
        Num = num;
        Suit = suit;
    }

    public int ValorCarta
    {
        get { return ValoresCarta[Num.ToUpper()]; }
    }
}

public enum ComandosJogo
{
    Enter,
    Stop,
    Quit
}

public enum Naipe
{
    CLUB,
    HEART,
    DIAMOND,
    SPADE
}

public class Player
{
    public string UserId { get; set; }
    public PlayerStatus Status { get; set; }
}

public enum PlayerStatus
{
    IDLE,
    PLAYING,
    GETTING,
    WAITING
}

public class Usuario
{
    public string UserId { get; private set; }
    public string Senha { get; private set; }

    public Usuario(string userId, string senha)
    {
        UserId = userId;
        Senha = senha;
    }
}

public class UsuarioChat
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public int Wins { get; set; }
}

public class MensagemChat
{
    public string UserId { get; set; }
    public string Mensagem { get; set; }
}

#endregion

#region Utils

public static class StringExtensions
{
    public static T ToEnum<T>(this string valorString)
    {
        return (T)Enum.Parse(typeof(T), valorString);
    }

    public static string FormatWith(this string @string, params object[] parametros)
    {
        return string.Format(@string, parametros);
    }
}

#endregion
