package editorImages.console;

public class ProcessadorEntradas{
	
	public Entradas ProcessarEntradas(String[] args){
		return new Entradas(args[0]);
	}	
}
