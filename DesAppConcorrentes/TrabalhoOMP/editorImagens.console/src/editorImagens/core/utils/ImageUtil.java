package editorImagens.core.utils;

import java.awt.Color;
import java.awt.Graphics;
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
	
	public static BufferedImage criarCopia(BufferedImage imagem){
		BufferedImage copia = new BufferedImage(imagem.getWidth(), imagem.getHeight(), BufferedImage.TYPE_INT_RGB);
		
		Graphics g = copia.getGraphics();
		g.drawImage(imagem, 0, 0, null);
		g.dispose();
		
		return copia;
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
	
	public static int[] arrayComponentes(int rgb){
		return new int[]{
				Colors.red(rgb),
				Colors.green(rgb),
				Colors.blue(rgb)
		};
	}
	
	public static Color converterParaRGB(int[] componentes){
		return new Color(componentes[0], componentes[1], componentes[2]);
	}
}
