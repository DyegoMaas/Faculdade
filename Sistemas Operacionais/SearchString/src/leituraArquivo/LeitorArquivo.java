package leituraArquivo;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.RandomAccessFile;

/**
 * 
 * @author Dyego Alekssander Maas
 *
 */
public class LeitorArquivo {

	private static RandomAccessFile file = null;
	
	public void OpenFile(String pArquivo) throws FileNotFoundException{
		file = new RandomAccessFile(pArquivo, "r");
	}
	
	public void CloseFile(){
		try
		{
			if(file != null)
				file.close();
		}
		catch(IOException ex){
			ex.printStackTrace();
		}
	}
	
	public boolean isOpen(){
		return file != null;
	}
	
	public void SearchString(String pString) throws Exception{
		if(file == null)
			throw new Exception("A busca não pode ser efetuada sem antes abrir o arquivo.");
		
		SearchStringAux(pString);
	}
	
	private void SearchStringAux(String pString) throws IOException{
		System.out.println("Iniciando leitura do arquivo");
		
		file.seek(0);
		boolean read = true;
		int lineNumber = 0;
		while(read){
			String line = file.readLine();
			read = file.getFilePointer() < file.length();
			lineNumber++;
			
			if(line.equals(""))
				continue;
			
			if(line.indexOf(pString) > -1){
				showResult(pString, line, lineNumber);
			}
			
			if(lineNumber % 50000 == 0)
				System.out.println(lineNumber + " linhas lidas.");
		}
		
		System.out.println("Leitura do arquivo completa.\n");
	}
	
	private void showResult(String pStringFound, String pFullLine, int pLineNumber){
		System.out.println("A string \"" + pStringFound + "\" foi encontrada na linha " + pLineNumber + 
				", na posição " + (pFullLine.indexOf(pStringFound) + 1));
	}
}
