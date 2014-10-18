using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Jogo : MonoBehaviour
{
    public Text listaJogadoresAtivos;
    public Text pontos;
    public Text mensagens;
    public InputField inputMensagens;
    public float intervaloAtualizacaoListaJogadores = 1f;
    private JogoCartas21 jogo;
    private Usuario usuario;
    
    private ConectorJogoCartas21 conectorJogo;
    private ConectorChat conectorChat;

    // Use this for initialization
	void Start ()
	{
	    var clienteTCP = GetComponent<ClienteTCP>();
	    var clienteUDP = GetComponent<ClienteUDP>();
	    var loginManager = FindObjectOfType<LoginManager>();

	    conectorJogo = new ConectorJogoCartas21(clienteTCP, clienteUDP);
	    conectorChat = new ConectorChat(clienteTCP, clienteUDP);
		usuario = new Usuario(loginManager.userId, loginManager.senha);
	    jogo = new JogoCartas21(conectorJogo, conectorChat, usuario);

        EntrarJogo();
	    StartCoroutine(AtualizarJogadoresAtivos());
        StartCoroutine(AtualizarMensagens());

        inputMensagens.onSubmit.AddListener(mensagem =>
        {
            var destinatario = Usuario.Servidor();

            var matchUserId = Regex.Match(mensagem, "(\\d+):");
            if (matchUserId.Success)
            {
                Debug.Log("mensagem antes_" + mensagem);
                destinatario = new Usuario(matchUserId.Value.TrimEnd(':'), string.Empty);
                mensagem = mensagem.Substring(matchUserId.Value.Length + 1).Trim();
                Debug.Log("match: " + matchUserId.Value);
                Debug.Log("mensagem depois_" + mensagem);
            }

            Debug.Log("mensagem enviada_" + mensagem);
            jogo.EnviarMensagem(mensagem, destinatario);
        });
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
        jogo.EntrarNoJogo();
        AtualizarPontuacaoNaTela();
    }

    void PararJogo()
    {
        jogo.PararDeJogar();
        AtualizarPontuacaoNaTela();
    }

    void SairJogo()
    {
        jogo.SairDoJogo();
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

    private IEnumerator AtualizarJogadoresAtivos()
    {
        var atualizado = false;
        bool iniciado = false;
        var jogadores = string.Empty;

        while (true)
        {
            if (!atualizado)
            {
                var thread = new Thread(() =>
                {
                    var stringBuilder = new StringBuilder();

                    var usuariosAtivos = jogo.ObterUsuariosChat();
                    var jogadoresAtivos = jogo.ObterJogadoresAtivos();

                    foreach (var usuarioChat in usuariosAtivos)
                    {
                        var jogador = jogadoresAtivos.FirstOrDefault(j => j.UserId == usuarioChat.UserId);
                        stringBuilder.AppendFormat("{0} ({1})", usuarioChat.UserId, usuarioChat.Wins);
                        if (jogador != null)
                        {
                            stringBuilder.AppendFormat(" - {0}", jogador.Status);
                        }
                        stringBuilder.AppendLine();
                    }
                    jogadores = stringBuilder.ToString();

                    atualizado = true;
                });
                if (!iniciado)
                {
                    thread.Start();
                    iniciado = true;
                }
                yield return true;
            }
            else
            {
                listaJogadoresAtivos.text = jogadores;
                atualizado = false;
                iniciado = false;
                yield return new WaitForSeconds(1f);   
            }
        }
    }

    private IEnumerator AtualizarMensagens()
    {
        var atualizado = false;
        var iniciado = false;
        var pilhaMensagens = new Stack<string>();

        var ultimaMensagem = string.Empty;

        while (true)
        {
            if (!atualizado)
            {
                var thread = new Thread(() =>
                {
                    var mensagem = jogo.ObterUltimaMensagem();
                    if (mensagem != null && mensagem.Mensagem != ultimaMensagem)
                    {
                        pilhaMensagens.Push("{0}: {1}".FormatWith(mensagem.UserId, mensagem.Mensagem));
                    }

                    atualizado = true;
                });
                if (!iniciado)
                {
                    thread.Start();
                    iniciado = true;
                }
                yield return true;
            }
            else
            {
                var stringBuilder = new StringBuilder();
                foreach (var msg in pilhaMensagens.Take(10))
                {
                    stringBuilder.AppendLine(msg);
                }
                mensagens.text = stringBuilder.ToString();

                atualizado = false;
                iniciado = false;
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void AtualizarPontuacaoNaTela()
    {
        pontos.text = jogo.Pontuacao.ToString();
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
        Pontuacao = 0;
        Ativo = false;
    }

    public void SairDoJogo()
    {
        Console.WriteLine("Sair jogo");
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
        EnviarMensagem(mensagem, destinatario:Usuario.Todos());
    }

    public void EnviarMensagem(string mensagem, Usuario destinatario)
    {
        conectorChat.EnviarMensagem(usuario, destinatario, mensagem);
    }

    public MensagemChat ObterUltimaMensagem()
    {
        return conectorChat.GetMessage(usuario);
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
        Debug.Log("no chat -> " + mensagemChat);
        Debug.Log("no chat (dest) -> " + destinatario.UserId);
        //var mensagem = "SEND MESSAGE {0}:{1}:{2}:{3}".FormatWith(remetente.UserId, remetente.Senha, destinatario.UserId, mensagemChat);
        var mensagem = string.Format("SEND MESSAGE {0}:{1}:{2}:{3}", remetente.UserId, remetente.Senha, destinatario.UserId, mensagemChat);
        Debug.Log("msg montada no chat -> " + mensagem);
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

    public static Usuario Servidor()
    {
        return new Usuario("0", string.Empty);
    }

    public static Usuario Todos()
    {
        return new Usuario("0", string.Empty);
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
