package listasSimplesmenteEncadeadas;

public class NoLista {
	
	private int info;
	private NoLista prox;
	
	public NoLista() {}
	
	public NoLista(int info) {
		setInfo(info);
	}
	
	public void setInfo(int info) {
		this.info = info;
	}
	
	public int getInfo() {
		return info;
	}
	
	public void setProx(NoLista prox) {
		this.prox = prox;
	}
	
	public NoLista getProx() {
		return prox;
	}
	
	@Override
	public String toString() {
		return String.valueOf(getInfo());
	}
}
