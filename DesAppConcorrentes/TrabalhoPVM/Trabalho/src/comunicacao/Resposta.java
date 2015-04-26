package comunicacao;

import comunicacao.pacotes.Pacote;

public class Resposta {
	private ComandosResposta comandosResposta;
	private Pacote pacote;
	
	public Resposta(ComandosResposta comandosReposta, Pacote pacote){
		this.comandosResposta = comandosReposta;
		this.pacote = pacote;
	}

	public ComandosResposta getTipoResposta(){
		return comandosResposta;
	}
	
	public Pacote getPacote(){
		return pacote;
	}
}
