package editorImagens.core;

import java.awt.Color;
import java.awt.image.BufferedImage;

public interface IFachadaEdicaoImagens {

	public abstract void blur(BufferedImage imagem, int windowWidth,
			int windowHeight);

	public abstract Color calcularCorMedia(BufferedImage imagem);

	public abstract void inverterCores(BufferedImage imagem);

	public abstract Color mediaInvertida(BufferedImage imagem);

	public abstract void xorBlendingComCorMedia(BufferedImage imagem);
	
	public abstract void mosaico(BufferedImage imagem, int tamanhoCelulas);
	
	public abstract void desaturar(BufferedImage imagem, float percentual);
	
	public abstract void distorcerCores(BufferedImage imagem, float distorcaoR, float distorcaoG, float distorcaoB);
	
	public abstract void outraDistorcao(BufferedImage imagem);

}