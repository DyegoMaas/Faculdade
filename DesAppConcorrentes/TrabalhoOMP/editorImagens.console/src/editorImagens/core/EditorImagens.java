package editorImagens.core;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.image.BufferedImage;
import java.awt.image.WritableRaster;
import java.util.Arrays;
import java.util.Comparator;

public class EditorImagens {

	public void avacalharImagem(BufferedImage imagem){
		Graphics g = imagem.getGraphics();
		g.setColor(Color.GREEN);
		g.fillOval(10, 20, 300, 100);
		g.drawOval(10, 10, 300, 100);
	}

	public void mediana(BufferedImage imagem, int windowWidth, int windowHeight){
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int edgeX = (windowWidth / 2); //rounded down
		int edgeY = (windowHeight / 2); //rounded down
		for (int x = edgeX; x < (imageWidth - edgeX); x++) {
			for (int y = edgeY; y < (imageHeight - edgeY); y++) {
				int[][] colorArray = new int[windowWidth][windowHeight];
				
				for (int fx = 0; fx < windowWidth; fx++) {
					for (int fy = 0; fy < windowHeight; fy++) {
						colorArray[fx][fy] = imagem.getRGB(x + fx - edgeX, y + fy - edgeY); //&0xff 
					}
				}
				
				for (int[] array : colorArray) {
					java.util.Arrays.sort(array);	
				}
				
				Arrays.sort(colorArray, new Comparator<int[]>() {
		            @Override
		            public int compare(final int[] entry1, final int[] entry2) {
		                final Integer time1 = entry1[0];
		                final Integer time2 = entry2[0];
		                return time1.compareTo(time2);
		            }
		        });
				
				int mediana = colorArray[windowWidth / 2][windowHeight / 2];
				imagem.setRGB(x, y, mediana);
			}	
		}		
		
//		allocate outputPixelValue[image width][image height]
//				   edgex := (window width / 2) rounded down
//				   edgey := (window height / 2) rounded down
//				   for x from edgex to image width - edgex
//				       for y from edgey to image height - edgey
//				           allocate colorArray[window width][window height]
//				           for fx from 0 to window width
//				               for fy from 0 to window height
//				                   colorArray[fx][fy] := inputPixelValue[x + fx - edgex][y + fy - edgey]
//				           sort all entries in colorArray[][]
//				           outputPixelValue[x][y] := colorArray[window width / 2][window height / 2]
	}
}

/*int rgb = img.getRGB(x, y);
int r = (rgb >> 16) & 0xFF;
int g = (rgb >> 8) & 0xFF;
int b = (rgb & 0xFF);*/
