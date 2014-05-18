package editorImagens.core;

import java.awt.image.BufferedImage;
import java.io.File;
import java.util.Comparator;

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
	
	public static Comparator<int[]> getIntArrayComparator(){	
		return new Comparator<int[]>() {
		    public int compare(int[] a, int[] b) {
		        return Integer.compare(a[0], b[0]);
		    }
		};
	}
}
