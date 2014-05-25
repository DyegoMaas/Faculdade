package editorImagens.core.utils;

import java.awt.Color;
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
		
	public static Color inverterCor(Color cor){
		int corInvertida = inverterCor(cor.getRGB());		
		return new Color(Colors.red(corInvertida), Colors.green(corInvertida), Colors.blue(corInvertida));
	}
	
	public static int inverterCor(int rgb){
		int red = 255 - Colors.red(rgb);
		int green = 255 - Colors.green(rgb);
		int blue = 255 - Colors.blue(rgb);
		
		Color corInvertida = new Color(red, green, blue);		
		return corInvertida.getRGB();
	}
}
