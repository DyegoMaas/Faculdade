package editorImagens.core;

import java.awt.image.BufferedImage;
import java.io.File;

import javax.imageio.ImageIO;

public class ImageUtil {
	public static BufferedImage carregarImagem(String caminho){
		BufferedImage bimg = null;
		try {

			bimg = ImageIO.read(new File(caminho));
		} catch (Exception e) {
			e.printStackTrace();
		}
		return bimg;
	}
}
