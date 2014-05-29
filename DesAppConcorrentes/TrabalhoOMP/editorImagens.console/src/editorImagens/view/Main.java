package editorImagens.view;

import java.awt.Color;
import java.awt.Dialog;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Rectangle;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import java.awt.image.BufferedImage;

import javax.swing.BoxLayout;
import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JFrame;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JRadioButton;

import com.towel.swing.img.JImagePanel;

import editorImagens.core.EditorImagensFactory;
import editorImagens.core.IFachadaEdicaoImagens;
import editorImagens.core.utils.ImageUtil;

public class Main {	
	
	private boolean usarJomp = false;
	
	public static void main(String[] args){
		args = new String[]{
				"C:\\dev\\java\\OMP\\imagemTeste01.jpg"
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
	
	private static WindowListener closeWindow = new WindowAdapter() {
        public void windowClosing(WindowEvent e) {
            e.getWindow().dispose();
        }
    };
	
	private IFachadaEdicaoImagens getEditorImagens(){
		return new EditorImagensFactory().getEditorImagens(usarJomp);
	}
	
	public void loadAndDisplayImage(Entradas entradas) {
		final BufferedImage imagem = ImageUtil.carregarImagem(entradas.getCaminhoArquivo());
		
		JPanel panelGeral = new JPanel();
			
		final JFrame frame = new JFrame("Editor de imagens - JOMP");
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
				
				BufferedImage imagemCor = new BufferedImage(220, 100, BufferedImage.TYPE_INT_RGB);
				Graphics g = imagemCor.getGraphics();
				g.setColor(corMedia);
				g.fillRect(0, 0, imagemCor.getWidth(), imagemCor.getHeight());
				
				exibirCaixaCor(imagemCor);
			}
			
			private void exibirCaixaCor(BufferedImage imagemCor){
				final JImagePanel imagePanel = new JImagePanel(imagemCor);
				
				JDialog dialogCor = new JDialog(frame, "Cor média", Dialog.ModalityType.MODELESS);
				dialogCor.add(imagePanel);
				dialogCor.setBounds(new Rectangle(imagemCor.getWidth(), imagemCor.getHeight()));
				dialogCor.addWindowListener(closeWindow);
				dialogCor.setVisible(true);
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "Inverter", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {				
				getEditorImagens().inverterCores(imagem);	
				imagePanel.repaint();
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "XOR com a cor média", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {				
				getEditorImagens().xorBlendingComCorMedia(imagem);	
				imagePanel.repaint();
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "Mosaico", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {				
				int tamanhoCelulas = showInputDialog("Tamanho das células: ");			
				
				getEditorImagens().mosaico(imagem, tamanhoCelulas);	
				imagePanel.repaint();
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "Dessaturar", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {
				int percentual = showInputDialog("Percentual de desaturação (inteiro): ");
				getEditorImagens().dessaturar(imagem, percentual / 100f);	
				imagePanel.repaint();
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "Distorcer cores", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {
				getEditorImagens().distorcerCores(imagem, .8f, 1.2f, .2f);	
				imagePanel.repaint();
			}
		});
		
		criarBotao(panelBotoes, imagem, imagePanel, "Distorcer cores 2", new ActionListener() {			
			@Override
			public void actionPerformed(ActionEvent e) {
				getEditorImagens().outraDistorcao(imagem);	
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
	
	private int showInputDialog(String titulo){
		int saida = 0;
		String entrada = JOptionPane.showInputDialog(titulo);
		try {
			saida = Integer.parseInt(entrada);
		} catch (Exception e2) {
			System.out.println(e2);
		}		
		return saida;
	}
}
