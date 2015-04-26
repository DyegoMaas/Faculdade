package comunicacao;

public enum ComandosProcessamento {

	ProcessarXML(1, "apresentacao.ProcessadorXML"),
	ProcessarJSON(2, "apresentacao.ProcessadorJSON");
	
	private int valor;
	private String classeExecucao;
	
	private ComandosProcessamento(int valor, String classeExecucao){
		this.valor = valor;
		this.classeExecucao = classeExecucao;
	}

	public int getValor(){
		return this.valor;
	}

	public String getClasseExecucao(){
		return this.classeExecucao;
	}
	
	public static ComandosProcessamento getComandoPorExtensao(String extensao) throws Exception{
		switch (extensao) {
		case "xml":
			return ProcessarXML;
		case "json":
			return ProcessarJSON;
		default:
			throw new Exception("extensão não suportada: " + extensao);
		}
	}
}
