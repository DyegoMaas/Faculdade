using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Jogo : MonoBehaviour
{
    public Text listaJogadoresAtivos;
    private JogoCartas21 jogo;
    private Usuario usuario;

	// Use this for initialization
	void Start ()
	{
	    var clienteTCP = GetComponent<ClienteTCP>();
	    var clienteUDP = GetComponent<ClienteUDP>();
	    var loginManager = FindObjectOfType<LoginManager>();

	    var conector = new ConectorJogoCartas21(clienteTCP, clienteUDP);
		usuario = new Usuario(loginManager.userId, loginManager.senha);
	    jogo = new JogoCartas21(conector, usuario);

	    clienteTCP.EnviarMensagem(string.Format("GET USERS {0}:{1}", usuario.UserId, usuario.Senha)); //TODO remover isso da versão final
        EntrarJogo();
        InvokeRepeating("AtualizarJogadoresAtivos", .5f, 1f);
	}

    // Update is called once per frame
	void Update () {
	
	}

    void EntrarJogo()
    {
        jogo.EntrarNoJogo();
    }

    void PararJogo()
    {
        jogo.PararDeJogar();
    }

    private void SairJogo()
    {
        jogo.SairDoJogo();
    }

    void AtualizarJogadoresAtivos()
    {
        if (jogo.Ativo)
        {
            var stringBuilder = new StringBuilder();
            var jogadoresAtivos = jogo.ObterJogadoresAtivos();
            foreach (var jogador in jogadoresAtivos)
            {
                stringBuilder.AppendFormat("{0}:{1}", jogador.UserId, jogador.Status).AppendLine();
            }
            listaJogadoresAtivos.text = stringBuilder.ToString();

            Debug.Log("Numero de jogadores: " + jogadoresAtivos.Count);
        }
    }
}

#region Jogo

public class JogoCartas21
{
    private readonly ConectorJogoCartas21 conector;
    private readonly Usuario usuario;
    private Player jogador;
    public int Pontuacao { get; private set; }
    public bool Ativo { get; private set; }

    public JogoCartas21(ConectorJogoCartas21 conector, Usuario usuario)
    {
        this.conector = conector;
        this.usuario = usuario;
    }

    public void EntrarNoJogo()
    {
        conector.EnviarComandoJogo(usuario, ComandosJogo.Enter);
        Pontuacao = 0;
        Ativo = true;
    }

    public void PararDeJogar()
    {
        conector.EnviarComandoJogo(usuario, ComandosJogo.Stop);
        Pontuacao = 0;
        Ativo = false;
    }

    public void SairDoJogo()
    {
        conector.EnviarComandoJogo(usuario, ComandosJogo.Quit);
        Pontuacao = 0;
        Ativo = false;
    }

    public IList<Player> ObterJogadoresAtivos()
    {
        var jogadoresAtivos = conector.GetPlayers(usuario).ToArray();
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

        var carta = conector.GetCard(usuario);
        Pontuacao += carta.ValorCarta;

        return carta;
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
        var resposta = clienteTcp.EnviarMensagem(mensagem);

        if (resposta == ":")
            throw new InvalidOperationException("resposta inválida (timeout?)");

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
        get { return ValoresCarta[Num]; }
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
