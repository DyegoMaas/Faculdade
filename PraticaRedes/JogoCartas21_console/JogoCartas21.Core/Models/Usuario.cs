namespace JogoCartas21.Core.Models
{
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