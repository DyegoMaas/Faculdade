package editorImagens.core.editor;

import java.awt.Color;
import java.awt.image.BufferedImage;

public interface IEditorImagens {
	void blur(BufferedImage imagem, int windowWidth, int windowHeight);
	void setarCor(BufferedImage imagem, Color novaCor);
	void inverterCores(BufferedImage imagem);
	void desaturarCorMedia(BufferedImage imagem);
	Color calcularCorMedia(BufferedImage imagem);
	
	Color inverterCor(Color cor);
	int inverterCor(int rgb);
	
}
