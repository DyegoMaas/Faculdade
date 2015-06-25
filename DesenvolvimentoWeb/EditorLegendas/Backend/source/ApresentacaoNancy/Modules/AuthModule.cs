using Nancy;
using Nancy.Authentication.Token;
using Nancy.Security;
using System.Collections.Generic;
using System.Linq;

namespace ApresentacaoNancy.Modules
{
    public class AuthModule : NancyModule
    {
        public AuthModule(ITokenizer tokenizer)
            : base("/auth")
        {
            Post["/"] = x =>
            {
                string nomeUsuario = Request.Form.nomeUsuario;
                string senha = Request.Form.senha;

                var identidadeUsuario = RepositorioUsuarios.ValidarUsuario(nomeUsuario, senha);
                if (identidadeUsuario == null)
                {
                    return HttpStatusCode.Unauthorized;
                }

                var token = tokenizer.Tokenize(identidadeUsuario, Context);
                return new
                {
                    Token = token,
                };
            };

            Get["/validation"] = _ =>
            {
                this.RequiresAuthentication();
                return "Yay! You are authenticated!";
            };

            Get["/admin"] = _ =>
            {
                this.RequiresClaims(new[] { Permissoes.PermissaoGerenciarProjetos, Permissoes.PermissaoGerenciarUsuarios });
                return "Yay! You are authorized!";
            };
        }
    }

    public class RepositorioUsuarios
    {
        private static readonly List<Usuario> Usuarios = new List<Usuario>
        {
            new Usuario("admin", "admin", Perfil.Administrador),
            new Usuario("dyego.maas@gmail.com", "dyego123", Perfil.Usuario)
        };

        public static IUserIdentity ValidarUsuario(string nomeUsuario, string senha)
        {
            if (string.IsNullOrWhiteSpace(nomeUsuario) || string.IsNullOrWhiteSpace(senha))
                return null;

            var usuario = Usuarios.FirstOrDefault(u => u.NomeUsuario == nomeUsuario && u.Senha == senha);
            if (usuario == null)
                return null;

            return usuario.Perfil == Perfil.Administrador 
                ? IdentidadeUsuarioFactory.Admin(usuario.NomeUsuario) 
                : IdentidadeUsuarioFactory.Usuario(usuario.NomeUsuario);
        }
    }

    public class Usuario
    {
        public string NomeUsuario { get; private set; }
        public string Senha { get; private set; }
        public Perfil Perfil { get; private set; }

        public Usuario(string nomeUsuario, string senha, Perfil perfil)
        {
            NomeUsuario = nomeUsuario;
            Senha = senha;
            Perfil = perfil;
        }
    }

    public enum Perfil
    {
        Administrador = 1,
        Usuario = 2
    }

    public class IdentidadeUsuario : IUserIdentity
    {
        public IdentidadeUsuario(string nomeUsuario)
        {
            UserName = nomeUsuario;
        }

        public string UserName { get; private set; }
        public IEnumerable<string> Claims { get; private set; }

        public void PossuiPermissoes(IEnumerable<string> permissoes)
        {
            Claims = permissoes;
        }
    }

    public class Permissoes
    {
        public const string PermissaoGerenciarUsuarios = "GerenciarUsuarios";
        public const string PermissaoGerenciarProjetos = "GerenciarProjetos";   
    }

    public class IdentidadeUsuarioFactory
    {
        public static IUserIdentity Admin(string nomeUsuario)
        {
            var administrador = new IdentidadeUsuario(nomeUsuario);
            administrador.PossuiPermissoes(new []
            {
                Permissoes.PermissaoGerenciarUsuarios,
                Permissoes.PermissaoGerenciarProjetos
            });
            return administrador;
        }

        public static IUserIdentity Usuario(string nomeUsuario)
        {
            var usuarioComum = new IdentidadeUsuario(nomeUsuario);
            usuarioComum.PossuiPermissoes(new[]
            {
                Permissoes.PermissaoGerenciarProjetos
            });
            return usuarioComum;
        }
    }
}