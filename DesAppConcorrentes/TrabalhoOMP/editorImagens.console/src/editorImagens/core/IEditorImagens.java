package editorImagens.core;

import java.awt.image.BufferedImage;

public interface IEditorImagens {
	void blur(BufferedImage imagem, int windowWidth, int windowHeight);
	void mediana(BufferedImage imagem);
}
