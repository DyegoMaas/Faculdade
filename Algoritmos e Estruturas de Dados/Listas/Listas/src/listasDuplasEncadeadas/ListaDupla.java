package listasDuplasEncadeadas;

public class ListaDupla {
	private NoListaDupla prim = null;
	
	public ListaDupla(){		
	
	}
	
	public void insere(int v){
		NoListaDupla novo = new NoListaDupla(v);
		novo.setProx(prim);
		novo.setAnt(null);
		
		if(prim != null)
			prim.setAnt(novo);
		
		this.prim = novo;		
	}	
	
	public void insereOrdenado(int v){		
		NoListaDupla novo = new NoListaDupla(v);
		if(prim == null){
			prim = novo;
			return;
		}
		
		NoListaDupla no = this.prim;
		while(no != null){
			if(v < no.getInfo()){				
				novo.setAnt(no.getAnt());
				
				if(no.getAnt() != null)
					no.getAnt().setProx(novo);
				
				novo.setProx(no);
				no.setAnt(novo);
				
				break;
			}
			
			if(no.getProx() == null){
				no.setProx(novo);
				novo.setAnt(no);
				
				return;
			}
			
			no = no.getProx();
		}
		
		if(no.equals(prim))
			this.prim = novo;
	}
	
	public void imprime(){
		NoListaDupla no = this.prim;
		while(no != null){
			System.out.println(no.getInfo());
			no = no.getProx();
		}
	}
	
	@Override
	public String toString(){
		StringBuffer texto = new StringBuffer();
		
		NoListaDupla no = this.prim;
		while(no != null){
			texto.append(no.getInfo()).append(',');
			no = no.getProx();
		}
		
		if(texto.length() > 0)
			texto.deleteCharAt(texto.length() - 1);
		else
			texto.append("empty");
		
 		return texto.toString();
	}
	
	public boolean vazia(){
		return prim == null;
	}
	
	public NoListaDupla busca(int v){
		NoListaDupla no = this.prim;
		while(no != null){
			if(no.getInfo() == v)
				return no;

			no = no.getProx();
		}
		return null;
	}
	
	public NoListaDupla buscaIndice(int i){
		int j = 0;
		NoListaDupla no = this.prim;
		while(no != null){
			if(i == j)
				return no;
			
			j++;
			no = no.getProx();
		}
		return no;
	}
	
	public void retira(int v){		
		NoListaDupla no = this.prim;
		while(no != null){
			if(no.getInfo() == v){
				
				if(no.getAnt() != null)
					no.getAnt().setProx(no.getProx());
				
				if(no.getProx() != null)
					no.getProx().setAnt(no.getAnt());
				
				if(no == prim)
					prim = no.getProx();
				
				return;
			}
			no = no.getProx();
		}
	}
	
	public void libera(){
		this.prim = null;
	}
	
	public boolean igual(ListaDupla outraLista){
		NoListaDupla no = this.prim;
		NoListaDupla noOutraLista = outraLista.buscaIndice(0);
		
		while(true){
			
			if(no == null && noOutraLista == null)
				return true;
			
			if(no == null || noOutraLista == null)
				return false;
			
			if(no.getInfo() != noOutraLista.getInfo())
				return false;
			
			no = no.getProx();
			noOutraLista = noOutraLista.getProx();
		}
	}
	
	public ListaDupla merge(ListaDupla outraLista){
		NoListaDupla no = this.prim;
		NoListaDupla noOutraLista = outraLista.buscaIndice(0);
		
		ListaDupla novaLista = new ListaDupla();
		while(true){
			
			if(no == null && noOutraLista == null)
				break;
			
			if(no != null) {
				novaLista.insere(no.getInfo());
				no = no.getProx();
			}
			
			if(noOutraLista != null) {
				novaLista.insere(noOutraLista.getInfo());
				noOutraLista = noOutraLista.getProx();
			}
		}
		
		return novaLista;		
	}
	
	public ListaDupla separa(int v) {
		NoListaDupla no = this.prim;
		while(no != null){
			if(no.getInfo() == v) {				
				break;
			}
			
			no = no.getProx();
		}
		
		if(no == null)
			return new ListaDupla();
		
		ListaDupla novaLista = new ListaDupla();
		novaLista.insereNo(no.getProx());
		no.setProx(null);
		
		return novaLista;
	}
	
	private void insereNo(NoListaDupla novo){
		novo.setAnt(null);				
		this.prim = novo;		
	}
}
