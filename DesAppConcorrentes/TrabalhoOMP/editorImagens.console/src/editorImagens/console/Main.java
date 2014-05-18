package editorImagens.console;

import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.image.BufferedImage;

import javax.swing.BoxLayout;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JPanel;

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
						
			main.loadAndDisplayImage(entradas);			
		}
		catch(Exception excecao){
			System.out.println("Erro: " + excecao);
		}
	}	
	
	public void loadAndDisplayImage(Entradas entradas) {
		final BufferedImage imagem = ImageUtil.carregarImagem(entradas.getCaminhoArquivo());
		
		JPanel panelGeral = new JPanel();
		
		JFrame frame = new JFrame("Tutorials");
		panelGeral.setLayout(new BoxLayout(panelGeral, BoxLayout.Y_AXIS));
		frame.add(panelGeral);
		
		final JImagePanel imagePanel = new JImagePanel(imagem);
		panelGeral.add(imagePanel);
		
		JButton btAvacalhar = new JButton("Mediana");
		btAvacalhar.setPreferredSize(new Dimension(100, 30));
		panelGeral.add(btAvacalhar);
		btAvacalhar.addActionListener(new ActionListener() {
			
			@Override
			public void actionPerformed(ActionEvent e) {				
				editor.mediana(imagem, 5, 5);	
				imagePanel.repaint();
			}
		});		

		
        frame.setPreferredSize(new Dimension(imagem.getWidth(), imagem.getHeight() + 50));
        frame.add(panelGeral);
        frame.pack();
        frame.setLocationRelativeTo(null);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setVisible(true);
	}
	
}
