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

    public void Login(string userId, string senha)
    {
        UserId = userId;
        Senha = senha;

        Application.LoadLevel(level);
    }
}
