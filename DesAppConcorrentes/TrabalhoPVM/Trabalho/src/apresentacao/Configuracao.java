package apresentacao;

import comunicacao.ComandosProcessamento;

public class Configuracao{
	private int numTarefas;
	private ComandosProcessamento comando = ComandosProcessamento.ProcessarJSON;
	
	public Configuracao(ComandosProcessamento comando){
		this.comando = comando;
	}
			
	public int getNumTarefas(){
		return numTarefas;
	}
	
	public void incNumTarefas(){
		numTarefas++;
	}		
	
	public ComandosProcessamento getComando(){
		return comando;
	}
}
