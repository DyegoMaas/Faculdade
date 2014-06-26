package comunicacao;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.zip.GZIPInputStream;
import java.util.zip.GZIPOutputStream;

import javax.xml.bind.DatatypeConverter;

import comunicacao.pacotes.Pacote;
import jpvm.jpvmBuffer;
import jpvm.jpvmEnvironment;
import jpvm.jpvmException;
import jpvm.jpvmMessage;
import jpvm.jpvmTaskId;

public class Escravo {

	private ComandosProcessamento comando;
	private jpvmEnvironment jpvmEnvironment;

	public Escravo(ComandosProcessamento comando, jpvmEnvironment jpvmEnvironment) {
		this.comando = comando;
		this.jpvmEnvironment = jpvmEnvironment;
	}

	public void Enviar(ComandosResposta comandoRecepcao, Pacote pacote) throws jpvmException, Exception {
		jpvmBuffer buffer = new jpvmBuffer();
		String conteudo = serializeObjectToString(pacote);
		buffer.pack(conteudo);

		jpvmTaskId idMestre = jpvmEnvironment.pvm_parent();
		jpvmEnvironment.pvm_send(buffer, idMestre, comandoRecepcao.getValor());

		System.out.printf("[MESTRE] Enviando comando %d\n", comando.getValor());
	}

	public Pacote Receber() throws jpvmException, Exception {
		jpvmMessage mensagem = jpvmEnvironment.pvm_recv();

		if (souResponsavel(mensagem)) {
			String retorno = mensagem.buffer.upkstr();
			Pacote pacoteRetorno = (Pacote)deserializeObjectFromString(retorno);
			return pacoteRetorno;
		}
		return null;
	}

	private boolean souResponsavel(jpvmMessage mensagem) {
		return mensagem.messageTag == comando.getValor();
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
}
