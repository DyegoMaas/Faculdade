package comunicacao;

import jpvm.jpvmBuffer;
import jpvm.jpvmEnvironment;
import jpvm.jpvmException;
import jpvm.jpvmMessage;
import jpvm.jpvmTaskId;

public class Escravo {
	private ComandosProcessamento comando;
	private jpvmEnvironment jpvmEnvironment;

	public Escravo(ComandosProcessamento comando, jpvmEnvironment jpvmEnvironment){
		this.comando = comando;
		this.jpvmEnvironment = jpvmEnvironment;		
	}
	
	public void Enviar(ComandosResposta comandoRecepcao, String conteudo) throws jpvmException{
		jpvmBuffer buffer = new jpvmBuffer();
		buffer.pack(conteudo);	
		
		jpvmTaskId idMestre = jpvmEnvironment.pvm_parent();
		jpvmEnvironment.pvm_send(buffer, idMestre, comandoRecepcao.getValor());
		
		System.out.printf("[MESTRE] Enviando comando %d\n", comando.getValor());
	}
	
	public String Receber() throws jpvmException{
		jpvmMessage mensagem = jpvmEnvironment.pvm_recv();
		
		if(souResponsavel(mensagem)){
			return mensagem.buffer.upkstr();
		}		
		return null;
	}
	
	private boolean souResponsavel(jpvmMessage mensagem){
		return mensagem.messageTag == comando.getValor();
	}
}
