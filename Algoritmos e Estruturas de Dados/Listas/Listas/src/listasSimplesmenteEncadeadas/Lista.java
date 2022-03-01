package listasSimplesmenteEncadeadas;

public class Lista {
	
	private NoLista prim = null;
	
	public Lista() {}
	
	public void insere(int v) {
		NoLista novoNo = new NoLista();
		novoNo.setInfo(v);
		
		novoNo.setProx(prim);
		prim = novoNo;
	}
	
	public void imprime() {
		NoLista no = prim;
		while (no != null) {
			print(no);
			no = no.getProx();
		}
	}
	
	private void print(NoLista no) {
		System.out.println(no.getInfo());
	}
	
	@Override
	public String toString() {
		StringBuffer buffer = new StringBuffer();
		
		NoLista no = prim;
		while (no != null) {
			buffer.append(no.getInfo());
			if(no.getProx() != null)
				buffer.append(", ");
			
			no = no.getProx();
		}
		
		if(buffer.length() == 0)
			return "vazia";
		
		return buffer.toString();
	}
	
	public boolean vazia() {
		return prim == null;
	}
	
	public NoLista busca(int v) {
		
		NoLista no = prim;
		while (no != null) {
			if (no.getInfo() == v)
				return no;
			no = no.getProx();
		}
		
		return null;
	}
	
	public int comprimento() {
		int comprimento = 0;
		
		NoLista no = prim;
		while (no != null) {
			comprimento++;
			no = no.getProx();
		}
		
		return comprimento;
	}
	
	public NoLista ultimo() {		
		NoLista no = prim;		
		while (no != null) {
			if(no.getProx() == null) {
				return no;
			}
			no = no.getProx();
		}
		
		return null;
	}
	
	public void retira(int v) {
		NoLista noAnterior = null;
		NoLista no = prim;		
		while (no != null) {
			if(no.getInfo() == v) {
				if(no == prim)
					prim = null;
				else if(noAnterior != null) {				
					noAnterior.setProx(no.getProx());
					break;
				}
			}
			noAnterior = no;
			no = no.getProx();			
		}
	}
	
	public void libera() {
		prim = null;
	}
	
	public void insereOrdenado(int v) {
		NoLista novo = new NoLista(v);
		
		if(prim == null)
		{
			prim = novo;
			return;
		}
		
		NoLista noAnterior = null;
		NoLista no = prim;		
		while (no != null) {
			if(no.getInfo() > v) {
				
				if(no == prim) {
					prim = novo;
					novo.setProx(no);
				}
				else {
					noAnterior.setProx(novo);
					novo.setProx(no);
				}
				break;
			}
			else if(no.getProx() == null) {
				no.setProx(novo);
				break;
			}
			
			noAnterior = no;
			no = no.getProx();
		}
	}
	
	public boolean igual(Lista l) {
		NoLista no = prim;
		NoLista noOutraLista = l.prim;
		
		while(true){
			if(no == null && noOutraLista == null)
				return true;
			
			if(no == null || noOutraLista == null)
				return false;
			
			if(no.getInfo() != noOutraLista.getInfo())
				return false;
			
			if(no != null)
				no = no.getProx();
			if(noOutraLista != null)
				noOutraLista = noOutraLista.getProx();
		}
	}
	
	public void imprimeRecursivo() {
		imprimeRecursivoAux(prim);
	}
	
	public void imprimeRecursivoAux(NoLista no) {
		if(no == null)
			return;
		
		this.print(no);
		imprimeRecursivoAux(no.getProx());
	}
	
	//TODO: concluir
	public NoLista retiraRecursivo(int v) {
		return retiraRecursivoAux(prim, v);
	}
	
	public NoLista retiraRecursivoAux(NoLista no, int v) {
		if(no == null)
			return null;
		
		if(no.getInfo() == v) {
			return no;
		}
		
		
		 {
			NoLista noRemovido = retiraRecursivoAux(no.getProx(), v);
			
			
			
			if(noRemovido != null)
				no.setProx(noRemovido.getProx());
			return noRemovido;
		}
	}
	
	
	
	
	
}
