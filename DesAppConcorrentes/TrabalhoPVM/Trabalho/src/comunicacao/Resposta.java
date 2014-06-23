package comunicacao;

public class Resposta {
	private ComandosResposta comandosResposta;
	private String conteudo;
	
	public Resposta(ComandosResposta comandosReposta, String conteudo){
		this.comandosResposta = comandosReposta;
		this.conteudo = conteudo;
	}
	
	public ComandosResposta getTipoResposta(){
		return comandosResposta;
	}
	
	public String getConteudo(){
		return conteudo;
	}
}
