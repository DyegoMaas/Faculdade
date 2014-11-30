package autenticacao;

public class Usuario {
	private String nome;
	private String senha;
	
	public String getNome() {
		return nome;
	}
	public String getSenha() {
		return senha;
	}
	
	public Usuario(String nome, String senha){
		this.nome = nome;
		this.senha = senha;
	}
}
