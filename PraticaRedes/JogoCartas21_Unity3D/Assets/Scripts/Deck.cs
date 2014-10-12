using System;
using UnityEngine;
using System.Collections;

public class Deck : MonoBehaviour {

    public Color corHighlight = Color.blue;
    public Transform destinoCarta;
    private Color startcolor;
    private Jogo jogo;
    
    void Start()
    {
        jogo = FindObjectOfType<Jogo>();

        InstanciarCarta(new Carta("A", Naipe.CLUB));
    }

    void OnMouseEnter()
    {
        startcolor = renderer.material.color;
        renderer.material.color = corHighlight;
    }

    void OnMouseExit()
    {
        renderer.material.color = startcolor;
    }

    void OnMouseUp()
    {
        var cartaAdquirida = jogo.PegarCarta();
        if (cartaAdquirida != null)
        {
            InstanciarCarta(cartaAdquirida);
        }
    }

    private void InstanciarCarta(Carta cartaAdquirida)
    {
        var caminhoResource = MontarCaminhoResource(cartaAdquirida);
        try
        {
            var carta = Instantiate(Resources.Load<GameObject>(caminhoResource)) as GameObject;
            carta.transform.parent = null;

            Transform targetTrasform = transform.parent;
            carta.transform.position = targetTrasform.position;
            carta.transform.rotation = targetTrasform.rotation;
            carta.transform.localScale = new Vector3(targetTrasform.localScale.x, targetTrasform.localScale.y, carta.transform.localScale.z);

            var posicaoFinal = destinoCarta.position;
            iTween.MoveTo(carta, iTween.Hash("position", posicaoFinal, "easeType", "easeInOutExpo", "time", 1f));

            var rotacaoFinal = destinoCarta.rotation.eulerAngles + new Vector3(0f, 180f);
            iTween.RotateTo(carta, iTween.Hash("rotation", rotacaoFinal, "easeType", "easeInOutBack", "delay", .1));
        }
        catch (Exception execao)
        {
            Debug.LogError("Erro ao carregar o recurso:" + caminhoResource);
        }
    }

    private string MontarCaminhoResource(Carta cartaAdquirida)
    {
        var naipe = string.Empty;
        switch (cartaAdquirida.Suit)
        {
            case Naipe.CLUB:
                naipe = "Club";
                break;
            case Naipe.DIAMOND:
                naipe = "Diamond";
                break;
            case Naipe.HEART:
                naipe = "Heart";
                break;
            case Naipe.SPADE:
                naipe = "Spade";
                break;
        }
        return string.Format("{0}s/{0}_{1}", naipe, cartaAdquirida.Num);
    }
}
