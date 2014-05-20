package editorImagens.core;

import java.awt.image.BufferedImage;

public interface IEditorImagens {
	void blur(BufferedImage imagem, int windowWidth, int windowHeight);
	void media(BufferedImage imagem);
	void inverterCores(BufferedImage imagem);
	void mediaInvertida(BufferedImage imagem);
}
