using UnityEngine;
using System.Collections;

public class Deck : MonoBehaviour {

    public Color corHighlight = Color.blue;
    private Color startcolor;
    private Jogo jogo;
    
    void Start()
    {
        jogo = FindObjectOfType<Jogo>();
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
        jogo.SendMessage("PegarCarta");    
    }
}
