package comunicacao.utils;

import java.beans.XMLDecoder;
import java.beans.XMLEncoder;
import java.io.ByteArrayOutputStream;
import java.io.InputStream;
import java.io.ByteArrayInputStream;
import java.nio.charset.Charset;

public class ObjectSerializationToXML {

	/**
	* This method saves (serializes) any java bean object into xml file
	*/
	public String toXML(Object objectToSerialize) throws Exception {
		ByteArrayOutputStream stream = new ByteArrayOutputStream();
		
		XMLEncoder encoder = new XMLEncoder(stream);
		encoder.writeObject(objectToSerialize);
		encoder.close();
		
		return new String(stream.toByteArray(), Charset.defaultCharset());
	}

	public Object fromXML(String conteudo) throws Exception {
		InputStream stream = new ByteArrayInputStream(conteudo.getBytes());
		
		XMLDecoder decoder = new XMLDecoder(stream);
		Object deSerializedObject = decoder.readObject();
		decoder.close();
		return deSerializedObject;
	}
}