package listasDuplasEncadeadas;

public class NoListaDupla {
	private int info;
	private NoListaDupla ant;
	private NoListaDupla prox;

	public NoListaDupla(){
		
	}
	
	public NoListaDupla(int info){
		this.info = info;
	}
	
	public void setInfo(int info) {
		this.info = info;
	}

	public int getInfo() {
		return info;
	}

	public void setAnt(NoListaDupla ant) {
		this.ant = ant;
	}

	public NoListaDupla getAnt() {
		return ant;
	}

	public void setProx(NoListaDupla prox) {
		this.prox = prox;
	}

	public NoListaDupla getProx() {
		return prox;
	}
}
