package arvores;

public class Arvore extends NoArvore{
	private NoArvore prim;
	
	public Arvore()
	{
		super(0);
	}
	
	public Arvore(int v){
		super(v);
	}
	
	public NoArvore criaNo(int v){
		return new NoArvore(v);
	}
	
	public void insereFilho(NoArvore no, NoArvore sa){
		
	}
}
