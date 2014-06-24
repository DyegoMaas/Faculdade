package comunicacao;

import java.util.HashMap;

import jpvm.jpvmBuffer;
import jpvm.jpvmEnvironment;
import jpvm.jpvmException;
import jpvm.jpvmMessage;
import jpvm.jpvmTaskId;

public class Mestre {

	private HashMap<ComandosProcessamento, ListaTarefas> tarefas = new HashMap<>();
	private jpvmEnvironment jpvmEnvironment;

	public Mestre(jpvmEnvironment jpvmEnvironment) {
		this.jpvmEnvironment = jpvmEnvironment;
	}

	public void Adicionar(ComandosProcessamento comando, int numeroEscravos)
			throws jpvmException {
		jpvmTaskId[] idsTarefas = new jpvmTaskId[numeroEscravos];
		jpvmEnvironment.pvm_spawn(comando.getClasseExecucao(), numeroEscravos,
				idsTarefas);

		tarefas.put(comando, new ListaTarefas(comando, idsTarefas));
	}

	public void Enviar(ComandosProcessamento comando, String conteudo)
			throws jpvmException {
		ListaTarefas listaTarefas = tarefas.get(comando);
		jpvmTaskId taskId = listaTarefas.getProximo();

		jpvmBuffer buffer = new jpvmBuffer();
		buffer.pack(conteudo);

		System.out.printf("[MESTRE] Enviando comando %d\n", comando.getValor());
		jpvmEnvironment.pvm_send(buffer, taskId, comando.getValor());
	}

	public Resposta Receber() throws jpvmException, Exception {
		jpvmMessage mensagem = jpvmEnvironment.pvm_recv();

		System.out.printf("[MESTRE] Recebido comando %d\n", mensagem.messageTag);		

		return new Resposta(
				ComandosResposta.getComandoRespostaPorTag(mensagem.messageTag),
				mensagem.buffer.upkstr());
	}
}
