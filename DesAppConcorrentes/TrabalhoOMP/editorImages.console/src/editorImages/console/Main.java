package editorImages.console;

import java.awt.image.BufferedImage;

import editorImagens.core.EditorImagens;

public class Main {
	public static void main(String[] args){
		ProcessadorEntradas processadorEntradas = new ProcessadorEntradas();
		
		try{
			Entradas entradas = processadorEntradas.ProcessarEntradas(args);
			
			EditorImagens editor = new EditorImagens();
			BufferedImage imagem = editor.carregarImagem(entradas.getCaminhoArquivo());
			
			System.out.printf("%dx%d", imagem.getWidth(), imagem.getHeight());
		}
		catch(Exception excecao){
			System.out.println("Erro: " + excecao);
		}
	}	

}
