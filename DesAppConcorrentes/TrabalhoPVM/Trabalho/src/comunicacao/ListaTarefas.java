package comunicacao;

import jpvm.jpvmTaskId;

public class ListaTarefas {

	private ComandosProcessamento comando;
	private jpvmTaskId[] idsTarefas;
	private int contador;

	public ListaTarefas(ComandosProcessamento comando, jpvmTaskId[] idsTarefas){
		this.comando = comando;
		this.idsTarefas = idsTarefas;
		this.contador = idsTarefas.length;
	}

	public jpvmTaskId getProximo(){
		int proximo = (contador + 1) % idsTarefas.length;
		contador++;
		
		return idsTarefas[proximo];
	}
	
}
