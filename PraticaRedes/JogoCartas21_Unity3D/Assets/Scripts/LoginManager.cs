using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public string userId = "";
    public string senha = "";

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
        userId = partes[0];
        senha = partes[1];

        Application.LoadLevel(1);
    }
}
