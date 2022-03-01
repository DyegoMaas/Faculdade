package arvores;

public class ArvoreBinaria {
	private NoArvoreBinaria raiz;
	
	public void insere(int v, NoArvoreBinaria esq, NoArvoreBinaria dir){
		this.raiz = new NoArvoreBinaria(v, esq, dir);
	}
	
	public boolean vazia(){
		return raiz == null;
	}
	
	@Override
	public String toString(){
		return imprimePre(raiz) + " / " + imprimeSim(raiz) + " / " + imprimePos(raiz);
	}
	
	private String imprimePre(NoArvoreBinaria no){
		if(raiz == null)
			return "";
		
		return "<" + raiz.getInfo() + imprimePre(no.getEsq()) + imprimePre(no.getDir()) + ">";
	}
	
	private String imprimeSim(NoArvoreBinaria no){
		if(raiz == null)
			return "";
		
		return "<" + imprimeSim(no.getEsq()) + raiz.getInfo() + imprimeSim(no.getDir()) + ">";
	}
	
	private String imprimePos(NoArvoreBinaria no){
		if(raiz == null)
			return "";
		
		return "<" + imprimePos(no.getEsq()) + imprimePos(no.getDir()) + raiz.getInfo() + ">";
	}
	
	public boolean pertence(int v){
		return pertence(raiz, v);
	}
	
	public boolean pertence(NoArvoreBinaria no, int v){
		if(no == null)
			return false;
		
		return no.getInfo() == v || pertence(no.getEsq(), v) || pertence(no.getDir(), v);
	}
	
	public int pares(){		
		return pares(raiz);
	}
	
	private int pares(NoArvoreBinaria no){
		if(no == null)
			return 0;
		
		int n = 0;
		if(no.getInfo() % 2 == 0)
			n = 1;
		
		return n + pares(no.getEsq()) + pares(no.getDir());		
	}
	
	public int folhas(){		
		return folhas(raiz);
	}
	
	private int folhas(NoArvoreBinaria no){
		if(no == null)
			return 1;
		
		return folhas(no.getEsq()) + folhas(no.getDir());		
	}
	
}
