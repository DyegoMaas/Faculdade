package arvores;

public class NoArvoreBinaria {
	private int info;
	private NoArvoreBinaria esq;
	private NoArvoreBinaria dir;
	
	public NoArvoreBinaria(int v)
	{
		this.info = v;
		this.esq = null;
		this.dir = null;
	}
	
	public NoArvoreBinaria(int v, NoArvoreBinaria esq, NoArvoreBinaria dir)
	{
		this.info = v;
		this.esq = esq;
		this.dir = dir;
	}

	public int getInfo() {
		return info;
	}

	public void setInfo(int info) {
		this.info = info;
	}

	public NoArvoreBinaria getEsq() {
		return esq;
	}

	public void setEsq(NoArvoreBinaria esq) {
		this.esq = esq;
	}

	public NoArvoreBinaria getDir() {
		return dir;
	}

	public void setDir(NoArvoreBinaria dir) {
		this.dir = dir;
	}
	
}
