package comunicacao;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.HashMap;
import java.util.zip.GZIPInputStream;
import java.util.zip.GZIPOutputStream;

import javax.xml.bind.DatatypeConverter;

import comunicacao.pacotes.Pacote;
import jpvm.jpvmBuffer;
import jpvm.jpvmEnvironment;
import jpvm.jpvmException;
import jpvm.jpvmMessage;
import jpvm.jpvmTaskId;

public class Mestre {

	private HashMap<ComandosProcessamento, ListaTarefas> tarefas = new HashMap<>();
	private jpvmEnvironment jpvmEnvironment;

	private int numeroEnvios, numeroRecebimentos;
	
	public Mestre(jpvmEnvironment jpvmEnvironment) {
		this.jpvmEnvironment = jpvmEnvironment;
	}

	public void adicionar(ComandosProcessamento comando, int numeroTarefas) throws jpvmException {
		int numeroEscravos = numeroEscravos(numeroTarefas);
		
		jpvmTaskId[] idsTarefas = new jpvmTaskId[numeroEscravos];
		jpvmEnvironment.pvm_spawn(comando.getClasseExecucao(), numeroEscravos, idsTarefas);

		tarefas.put(comando, new ListaTarefas(comando, idsTarefas));
	}

	public void enviar(ComandosProcessamento comando, Pacote pacote) throws jpvmException, Exception {
		ListaTarefas listaTarefas = tarefas.get(comando);
		jpvmTaskId taskId = listaTarefas.getProximo();

		jpvmBuffer buffer = new jpvmBuffer();
		
		String conteudo = serializeObjectToString(pacote);
		buffer.pack(conteudo);

		System.out.printf("[MESTRE] Enviando comando [%d] do tipo [%d] para o arquivo %s\n", ++numeroEnvios, comando.getValor(), pacote.cabecalho.nomeArquivo);
		jpvmEnvironment.pvm_send(buffer, taskId, comando.getValor());
	}

	public Resposta receber() throws jpvmException, Exception {
		jpvmMessage mensagem = jpvmEnvironment.pvm_recv();

		System.out.printf("[MESTRE] Recebido comando [%d] do tipo [%d] do escravo [%s]\n", ++numeroRecebimentos, mensagem.messageTag, mensagem.sourceTid);		

		String retorno = mensagem.buffer.upkstr();
		Pacote pacoteRetorno = (Pacote)deserializeObjectFromString(retorno);
		
		//System.out.printf("[MESTRE] Mensagem %d\n", mensagem.messageTag);		
		return new Resposta(ComandosResposta.getComandoRespostaPorTag(mensagem.messageTag), pacoteRetorno);
	}

	public String serializeObjectToString(Object object) throws Exception 
    {
        ByteArrayOutputStream arrayOutputStream = new ByteArrayOutputStream();
        GZIPOutputStream gzipOutputStream = new GZIPOutputStream(arrayOutputStream);
        ObjectOutputStream objectOutputStream = new ObjectOutputStream(gzipOutputStream);

        objectOutputStream.writeObject(object);

        objectOutputStream.flush();
        gzipOutputStream.close();
        arrayOutputStream.close();
        objectOutputStream.close();
        
        String objectString = DatatypeConverter.printBase64Binary(arrayOutputStream.toByteArray());

        return objectString;
    }

    public static Object deserializeObjectFromString(String objectString) throws Exception 
    {
        ByteArrayInputStream arrayInputStream = new ByteArrayInputStream(DatatypeConverter.parseBase64Binary(objectString));
        GZIPInputStream gzipInputStream = new GZIPInputStream(arrayInputStream);
        ObjectInputStream objectInputStream = new ObjectInputStream(gzipInputStream);

        Object object = objectInputStream.readObject();

        objectInputStream.close();
        gzipInputStream.close();
        arrayInputStream.close();

        return object;
    }
	
	private int numeroEscravos(int numeroTarefas) {
		return numeroTarefas;
//		int numeroEscravos = numeroTarefas / 2;
//		if(numeroEscravos == 0)
//			numeroEscravos = 1;
//		
//		return numeroEscravos;
	}

	public void finalizar() throws jpvmException {
		this.jpvmEnvironment.pvm_exit();
	}
}
