using UnityEngine;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public string level = "jogo";

    public string UserId { get; private set; }
    public string Senha { get; private set; }

	// Use this for initialization
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
	void Update () {
	
	}

    public void Login(string login)
    {
        var partes = login.Trim().Split(':');
        UserId = partes[0];
        Senha = partes[1];

        Application.LoadLevel(level);
    }
}
