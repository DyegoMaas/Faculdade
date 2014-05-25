package conversaoJomp;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.channels.FileChannel;

import jomp.compiler.*;

public class ConversorJomp4 {
	
	

	// Changelog
	// 4.0  - Agpoa está convertendo todos os .java que terminam com o valor da variavel keyName
	// 3.1b - Adicionado uma validação do arquivo da existencia do arquivo .jomp
	// 3.1a - Adicionado função para deletar o .jomp Criado
	
	// Mude o nome da variavel abaixo para o padrão utilizado
	private final static String keyName = "_normal.java";

	public static void main(String[] args) {
		try {
			
		
		File folder = new File(System.getProperty("user.dir"));
		listaArquivos(folder);
		
		} catch (Exception e) {
			System.out.println(e.getMessage());
		}finally{
			System.out.println("Conversão Concluida!");
		}
		
	}
	
	public static void criaAquivo(File file){
		
		String fileName = file.getPath();
		System.out.println("Origem - " + fileName);
		File oldFile = new File(fileName);

		fileName = fileName.split(keyName)[0] + "_jomp";
		File newFile = new File(fileName + ".jomp");

		copyFile2(oldFile, newFile);

		// compila arquivo .jomp
		String[] s = new String[1];
		s[0] = fileName;
		Jomp.main(s);
		System.out.println("Destino - " + fileName + ".java");

		deleteFile(newFile);
	}
	
	public static void listaArquivos(File folder) {
		File[] listOfFiles = folder.listFiles();
		for (int i = 0; i < listOfFiles.length; i++) {
			if (listOfFiles[i].isFile() && listOfFiles[i].getName().endsWith(keyName)) {
				criaAquivo(listOfFiles[i]);
			} else if (listOfFiles[i].isDirectory()) {
				listaArquivos(listOfFiles[i].getAbsoluteFile());
			}
		}
	}

	public static void deleteFile(File file) {

		try {
			if (file.exists())
				if (file.delete()) {
					System.out.println(file.getName() + " foi deletado!");
				} else {
					System.err.println("Falha ao deletar.");
				}
			else
				System.err.println("Arquivo " + file.getName()
						+ " não encontrado!");
		} catch (Exception e) {
			System.err.println(e.getMessage());
		}
	}

	public static void copyFile(File sourceFile, File destFile)
			throws IOException {
		if (!destFile.exists()) {
			destFile.createNewFile();
		}

		FileChannel source = null;
		FileChannel destination = null;
		try {
			source = new FileInputStream(sourceFile).getChannel();
			destination = new FileOutputStream(destFile).getChannel();
			destination.transferFrom(source, 0, source.size());
		} finally {
			if (source != null) {
				source.close();
			}
			if (destination != null) {
				destination.close();
			}
		}
	}

	public static void copyFile2(File sourceFile, File destFile) {

		String className = null;
		BufferedReader reader;
		try {
			reader = new BufferedReader(new FileReader(sourceFile));

			BufferedWriter writer = new BufferedWriter(new FileWriter(destFile));

			// ... Loop as long as there are input lines.
			String line = null;
			while ((line = reader.readLine()) != null) {

				if (line.contains("public class")) {
					className = line.split(" ")[2];
					
				}

				if (className != null && line.contains(className)) {
					//System.out.println("subst className " + line);
					line = line.replace(className, className.replace("_normal", "_jomp"));
				}

				writer.write(line);
				writer.newLine(); // Write system dependent end of line.
			}

			// ... Close reader and writer.
			reader.close(); // Close to unlock.
			writer.close(); // Close to unlock and flush to disk.

		} catch (IOException e) {
			e.printStackTrace();
		}
	}

}