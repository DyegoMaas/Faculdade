package editorImagens.core.editor;

import java.awt.Color;
import java.awt.image.BufferedImage;

public interface IEditorImagens {
	void blur(BufferedImage imagem, int windowWidth, int windowHeight);
	void setarCor(BufferedImage imagem, Color novaCor);
	void inverterCores(BufferedImage imagem);
	void xorBlendingComCorMedia(BufferedImage imagem);
	Color calcularCorMedia(BufferedImage imagem);
	void mosaico(BufferedImage imagem, int tamanhoCelulas);
	void dessaturar(BufferedImage imagem, float percentual);	
	void distorcerCores(BufferedImage imagem, float distorcaoR, float distorcaoG, float distorcaoB);
	void outraDistorcao(BufferedImage imagem);
}
