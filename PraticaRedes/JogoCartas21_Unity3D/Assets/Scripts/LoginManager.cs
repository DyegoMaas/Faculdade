using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public string userId = "";
    public string senha = "";
    private bool logado;

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

    void OnGUI()
    {
        if (logado)
            return;

        var windowRect = new Rect();
        windowRect.x = Screen.width / 2 - 100;
        windowRect.y = Screen.height / 2 - 50;
        windowRect.width = 200;
        windowRect.height = 100;
        
        GUI.Window(0, windowRect, OnWindowGUI, "Autenticação");
    }

    void OnWindowGUI(int windowID)
    {
        userId = GUILayout.TextField(userId);
        senha = GUILayout.PasswordField(senha, '*');
        if (GUILayout.Button("Login"))
        {
            Login(string.Format("{0}:{1}", userId, senha));
            logado = true;
        }
        GUI.color = Color.white;
    }
}
