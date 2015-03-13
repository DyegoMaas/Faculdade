package exemplos;

import java.util.LinkedList;
import java.util.List;

import busca.BuscaLargura;
import busca.Estado;
import busca.Nodo;

public class HLCA implements Estado {	
	private char homem;
	private char lobo;
	private char carneiro;
	private char alface;
	private String nomeAcao;
	private int custoAnterior;

	public HLCA(char ladoHomem, char ladoLobo, char ladoCarneiro, char ladoAlface, String nomeEstado, int custoAnterior) {
		this.homem = ladoHomem;
		this.lobo = ladoLobo;
		this.carneiro = ladoCarneiro;
		this.alface = ladoAlface;
		this.nomeAcao = nomeEstado;
		this.custoAnterior = custoAnterior;
	}
	
	@Override
	public int custo() {
		return custoAnterior + 1;
	}

	@Override
	public boolean ehMeta() {
		return homem == 'd' && lobo == 'd' && carneiro == 'd' && alface == 'd';
	}

	@Override
	public List<Estado> sucessores() {
		List<Estado> sucessores = new LinkedList<Estado>();
		
		levarHomem(sucessores);
		levarLobo(sucessores);
		levarCarneiro(sucessores);
		levarAlface(sucessores);
		
		return sucessores;
	}
	
	private void levarAlface(List<Estado> sucessores) {
		char margemOposta = ladoOposto(homem);
		
		HLCA novoEstado = new HLCA(margemOposta, lobo, carneiro, margemOposta, "levarAlface", custo());
		
		if(ehValido(novoEstado)){
			sucessores.add(novoEstado);
		}
	}

	private void levarCarneiro(List<Estado> sucessores) {
		char margemOposta = ladoOposto(homem);
		
		HLCA novoEstado = new HLCA(margemOposta, lobo, margemOposta, alface, "levarCarneiro", custo());
		
		if(ehValido(novoEstado)){
			sucessores.add(novoEstado);
		}
	}

	private void levarLobo(List<Estado> sucessores) {
		char margemOposta = ladoOposto(homem);
		
		HLCA novoEstado = new HLCA(margemOposta, margemOposta, carneiro, alface, "levarLobo", custo());
		
		if(ehValido(novoEstado)){
			sucessores.add(novoEstado);
		}
	}

	private void levarHomem(List<Estado> sucessores) {
		char margemOposta = ladoOposto(homem);
		
		HLCA novoEstado = new HLCA(margemOposta, lobo, carneiro, alface, "levarHomem", custo());
		
		if(ehValido(novoEstado)){
			sucessores.add(novoEstado);
		}
	}

	private boolean ehValido(HLCA novoEstado) {
		if(lobo == carneiro && lobo != homem)
			return false;
		
		if(carneiro == alface && carneiro != homem)
			return false;
		
		return true;
	}

	private char ladoOposto(char lado) {
		return (lado == 'e') ? 'd': 'e';
	}

	@Override
	public boolean equals(Object o) {
		if(o instanceof HLCA){
			HLCA outro = (HLCA)o;
			
			return 
					outro.alface == this.alface &&
					outro.carneiro == this.carneiro &&
					outro.homem == this.homem &&
					outro.lobo == this.lobo;
		}
		return false;
	}
	
	@Override
	public int hashCode() {
		return ("" + homem + lobo + carneiro + alface).hashCode(); 
	}
	
	@Override
	public String toString() {
		StringBuffer direita = new StringBuffer();
		StringBuffer esquerda = new StringBuffer();

		if(homem == 'e')
			esquerda.append('h');
		else
			direita.append('h');
		
		if(lobo == 'e')
			esquerda.append('l');
		else
			direita.append('l');
		
		if(carneiro == 'e')
			esquerda.append('c');
		else
			direita.append('c');
		
		if(alface == 'e')
			esquerda.append('a');
		else
			direita.append('a');
		
		return esquerda + "'---" + direita;
	}

	public static void main(String[] args) {
		HLCA problema = new HLCA('e', 'e', 'e', 'e', "inicial", 1);
		
		BuscaLargura buscaLargura = new BuscaLargura();
		Nodo nodo = buscaLargura.busca(problema);
		
		if(nodo == null)
			System.out.println("Problema sem solução");
		else
			System.out.println("Solução:\n" + nodo.montaCaminho() + "\n");
	}

}
