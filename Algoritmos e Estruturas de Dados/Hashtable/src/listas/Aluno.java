package listas;

public class Aluno {

	private String nome;
	private int matricula;
	private float mediaGeral;
	private Aluno prox = null;
	
	public Aluno(){		
	}
	
	public Aluno(int matricula, String nome, float mediaGeral){
		this.matricula = matricula;
		this.nome = nome;
		this.mediaGeral = mediaGeral;
	}
	
	public String getNome() {
		return nome;
	}
	public void setNome(String nome) {
		this.nome = nome;
	}
	public int getMatricula() {
		return matricula;
	}
	public void setMatricula(int matricula) {
		this.matricula = matricula;
	}
	public float getMediaGeral() {
		return mediaGeral;
	}
	public void setMediaGeral(float mediaGeral) {
		this.mediaGeral = mediaGeral;
	}
	public Aluno getProx() {
		return prox;
	}
	public void setProx(Aluno prox) {
		this.prox = prox;
	}	
	
	@Override
	public String toString(){
		StringBuilder s = new StringBuilder();
		s.append("{");
		s.append(getMatricula()).append(",").append(getNome()).append(",").append(getMediaGeral());
		s.append("}");
		return s.toString();
	}
	
}
