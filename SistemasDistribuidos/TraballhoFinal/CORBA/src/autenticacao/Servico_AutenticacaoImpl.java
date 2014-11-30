package autenticacao;

import java.util.ArrayList;
import java.util.List;

import org.omg.CosNaming.*;
import org.omg.CORBA.*;

class Servico_AutenticacaoImpl extends Servico_AutenticacaoPOA
{
	private static List<Usuario> usuarios;
	private static List<String> tokensAtivos;
	
	static  {
		usuarios = new ArrayList<Usuario>();
		tokensAtivos = new ArrayList<String>();
		
		//pré-cadastro de usuários
		cadastrarUsuario("vader", "luke");
	}

	@Override
	public String autenticar_usuario(String usuario, String senha) {
		if(usuarioExiste(usuario, senha)) {
			String token = gerarToken();				
			Servico_AutenticacaoImpl.tokensAtivos.add(token);
			
			return token;
		}
		return "";
	}
	
	private boolean usuarioExiste(String usuario, String senha){
		for(Usuario user : Servico_AutenticacaoImpl.usuarios){
			if(user.getNome().equals(usuario) && user.getSenha().equals(senha)) {
				return true;
			}
		}
		return false;
	}

	@Override
	public String cadastrar_usuario(String usuario, String senha) {
		cadastrarUsuario(usuario, senha);
		return autenticar_usuario(usuario, senha);
	}

	private static void cadastrarUsuario(String usuario, String senha) {
		usuarios.add(new Usuario(usuario, senha));
	}  

	private String gerarToken(){
		return java.util.UUID.randomUUID().toString();
	}
}