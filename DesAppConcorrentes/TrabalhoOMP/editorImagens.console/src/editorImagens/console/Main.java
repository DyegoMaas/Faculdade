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

import editorImagens.core.EditorImagensFactory;
import editorImagens.core.IEditorImagens;
import editorImagens.core.ImageUtil;

public class Main {
	
	private IEditorImagens editor = EditorImagensFactory.getEditorImagens(false);
	
	public static void main(String[] args){
		args = new String[]{
				"C:\\Users\\Dyego\\Pictures\\wallpaper2835791.jpg"
		};
				
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
		
		JPanel panelBotoes = new JPanel();
		panelBotoes.setLayout(new BoxLayout(panelBotoes, BoxLayout.X_AXIS));		
		
		criarBotao(panelBotoes, imagem, imagePanel, "Blur", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {				
				editor.blur(imagem, 9, 9);	
				imagePanel.repaint();
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "Média", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {				
				editor.media(imagem);	
				imagePanel.repaint();
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "Inverter", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {				
				editor.inverterCores(imagem);	
				imagePanel.repaint();
			}
		});
		
		panelGeral.add(panelBotoes);
		panelGeral.add(imagePanel);
		
        frame.setPreferredSize(new Dimension(imagem.getWidth(), imagem.getHeight() + 50));
        frame.add(panelGeral);
        frame.pack();
        frame.setLocationRelativeTo(null);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setVisible(true);
	}
	
	private void criarBotao(JPanel panel, final BufferedImage imagem, final JImagePanel imagePanel, String texto, ActionListener acao){
		JButton btAvacalhar = new JButton(texto);
		btAvacalhar.setPreferredSize(new Dimension(100, 30));		
		btAvacalhar.addActionListener(acao);
		
		panel.add(btAvacalhar);
	}
}
