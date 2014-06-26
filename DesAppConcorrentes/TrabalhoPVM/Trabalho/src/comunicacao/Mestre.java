package comunicacao;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutput;
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

	public Mestre(jpvmEnvironment jpvmEnvironment) {
		this.jpvmEnvironment = jpvmEnvironment;
	}

	public void Adicionar(ComandosProcessamento comando, int numeroEscravos) throws jpvmException {
		jpvmTaskId[] idsTarefas = new jpvmTaskId[numeroEscravos];
		jpvmEnvironment.pvm_spawn(comando.getClasseExecucao(), numeroEscravos, idsTarefas);

		tarefas.put(comando, new ListaTarefas(comando, idsTarefas));
	}

	public void Enviar(ComandosProcessamento comando, Pacote pacote) throws jpvmException, Exception {
		ListaTarefas listaTarefas = tarefas.get(comando);
		jpvmTaskId taskId = listaTarefas.getProximo();

		jpvmBuffer buffer = new jpvmBuffer();
		
		String conteudo = serializeObjectToString(pacote);
		buffer.pack(conteudo);

		System.out.printf("[MESTRE] Enviando comando %d\n", comando.getValor());
		jpvmEnvironment.pvm_send(buffer, taskId, comando.getValor());
	}

	public Resposta Receber() throws jpvmException, Exception {
		jpvmMessage mensagem = jpvmEnvironment.pvm_recv();

		System.out.printf("[MESTRE] Recebido comando %d\n", mensagem.messageTag);		

		String retorno = mensagem.buffer.upkstr();
		Pacote pacoteRetorno = (Pacote)deserializeObjectFromString(retorno);
		
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
        //String objectString = new String(base64.encode(arrayOutputStream.toByteArray()));

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
	
	private static byte[] prepararPacoteParaEnvio(Pacote pacote) {
		ByteArrayOutputStream bos = new ByteArrayOutputStream();
		ObjectOutput out = null;
		try {
			out = new ObjectOutputStream(bos);   
			out.writeObject(pacote);
			byte[] yourBytes = bos.toByteArray();
			return yourBytes;
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			try {
				if (out != null) {
					out.close();
				}
			} catch (IOException ex) {
			}
			try {
				bos.close();
			} catch (IOException ex) {
			}
		}
		return null;
	}
}
