package editorImagens.core;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.image.BufferedImage;

public class EditorImagens {

	public void avacalharImagem(BufferedImage imagem){
		Graphics g = imagem.getGraphics();
		g.setColor(Color.GREEN);
		g.fillOval(10, 20, 300, 100);
		g.drawOval(10, 10, 300, 100);
	}

}
