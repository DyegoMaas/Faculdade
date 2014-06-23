package comunicacao;

public enum ComandosResposta {
	RespostaXML(3), RespostaJSON(4);

	private int valor;

	private ComandosResposta(int valor) {
		this.valor = valor;
	}

	public int getValor() {
		return this.valor;
	}

	public static ComandosResposta getComandoRespostaPorTag(int valorTag) throws Exception {
		switch (valorTag) {
		case 3:
			return RespostaXML;
		case 4:
			return RespostaJSON;
		default:
			throw new Exception("tipo resposta não encontrado: " + valorTag);
		}
	}
}
