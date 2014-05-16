package editorImages.console;

import java.awt.Dimension;
import java.awt.image.BufferedImage;

import javax.swing.JFrame;

import com.towel.swing.img.JImagePanel;

import editorImagens.core.EditorImagens;
import editorImagens.core.ImageUtil;

public class Main {
	
	private EditorImagens editor = new EditorImagens();
	
	public static void main(String[] args){
		Main main = new Main();
		ProcessadorEntradas processadorEntradas = new ProcessadorEntradas();
		
		try{
			Entradas entradas = processadorEntradas.ProcessarEntradas(args);
			
//			 editor.carregarImagem(entradas.getCaminhoArquivo());
//			
//			System.out.printf("%dx%d", imagem.getWidth(), imagem.getHeight());
			
			
			JFrame frame = new JFrame("Tutorials");
			main.loadAndDisplayImage(frame, entradas);			
		}
		catch(Exception excecao){
			System.out.println("Erro: " + excecao);
		}
	}	
	
	public void loadAndDisplayImage(JFrame frame, Entradas entradas) {
		BufferedImage imagem = ImageUtil.carregarImagem(entradas.getCaminhoArquivo());

		JImagePanel panel = new JImagePanel(imagem);
        frame.setPreferredSize(new Dimension(imagem.getWidth(), imagem.getHeight()));
        frame.add(panel);
        frame.pack();
        frame.setLocationRelativeTo(null);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setVisible(true);
	}
	
}
