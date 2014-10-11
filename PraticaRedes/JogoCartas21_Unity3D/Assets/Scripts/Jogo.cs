using System;
using System.Collections.Generic;
using UnityEngine;

public class Jogo : MonoBehaviour
{
    private ClienteTCP clienteTCP;
    private ClienteUDP clienteUDP;

	// Use this for initialization
	void Start ()
	{
	    clienteTCP = GetComponent<ClienteTCP>();
	    clienteUDP = GetComponent<ClienteUDP>();
	}

    // Update is called once per frame
	void Update () {
	
	}
}

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