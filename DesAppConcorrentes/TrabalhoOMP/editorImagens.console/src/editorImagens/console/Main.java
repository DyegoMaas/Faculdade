package editorImagens.console;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import java.awt.image.BufferedImage;

import javax.swing.BoxLayout;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JRadioButton;

import com.towel.swing.img.JImagePanel;

import editorImagens.core.EditorImagensFactory;
import editorImagens.core.IEditorImagens;
import editorImagens.core.IFachadaEdicaoImagens;
import editorImagens.core.ImageUtil;

public class Main {	
	
	private boolean usarJomp = false;
	
	public static void main(String[] args){
		args = new String[]{
				//"C:\\Users\\Dyego\\Pictures\\wallpaper2835791.jpg"
				"C:\\imagensTeste\\mass-effect.jpg"
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
	
	private IFachadaEdicaoImagens getEditorImagens(){
		return new EditorImagensFactory().getEditorImagens(usarJomp);
	}
	
	public void loadAndDisplayImage(Entradas entradas) {
		final BufferedImage imagem = ImageUtil.carregarImagem(entradas.getCaminhoArquivo());
		
		JPanel panelGeral = new JPanel();
			
		JFrame frame = new JFrame("Editor de imagens - JOMP");
		panelGeral.setLayout(new BoxLayout(panelGeral, BoxLayout.Y_AXIS));
		frame.add(panelGeral);
		
		final JImagePanel imagePanel = new JImagePanel(imagem);		
		
		JPanel panelBotoes = new JPanel();
		panelBotoes.setLayout(new BoxLayout(panelBotoes, BoxLayout.X_AXIS));
		
		JRadioButton rbUsarJomp = new JRadioButton("JOMP");
		rbUsarJomp.addItemListener(new ItemListener() {

			@Override
			public void itemStateChanged(ItemEvent e) {
				usarJomp = (e.getStateChange() == ItemEvent.SELECTED);				
			}
		});
		panelBotoes.add(rbUsarJomp);
		
		criarBotao(panelBotoes, imagem, imagePanel, "Blur", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {				
				getEditorImagens().blur(imagem, 9, 9);	
				imagePanel.repaint();
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "Média", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {	
				Color corMedia = getEditorImagens().calcularCorMedia(imagem);
				
				System.out.println("cor média " + corMedia);
				imagePanel.repaint();
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "Inverter", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {				
				getEditorImagens().inverterCores(imagem);	
				imagePanel.repaint();
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "Desaturar cor média", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {				
				getEditorImagens().desaturarCorMedia(imagem);	
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
