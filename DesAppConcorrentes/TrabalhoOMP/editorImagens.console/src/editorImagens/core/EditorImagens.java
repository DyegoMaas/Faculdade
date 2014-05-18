package editorImagens.core;

import java.awt.image.BufferedImage;
import java.util.Arrays;

import jomp.runtime.OMP;

public class EditorImagens implements IEditorImagens{

	public void mediana(BufferedImage imagem, int windowWidth, int windowHeight){
		OMP.setNumThreads(windowWidth);
		
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int edgeX = (windowWidth / 2); //rounded down
		int edgeY = (windowHeight / 2); //rounded down

		int xJomp = 0;
		//omp parallel for private(xJomp)
		for (xJomp = 0; xJomp < (imageWidth - edgeX * 2); xJomp++) {
			int x = xJomp + edgeX;
			for (int y = edgeY; y < (imageHeight - edgeY); y++) {
				int[][] colorArray = new int[windowWidth][windowHeight];
				
				int fx = 0;
				for (fx = 0; fx < windowWidth; fx++) {
					for (int fy = 0; fy < windowHeight; fy++) {
						colorArray[fx][fy] = imagem.getRGB(x + fx - edgeX, y + fy - edgeY); //&0xff 
					}
				}
				
				Arrays.sort(colorArray, ImageUtil.getIntArrayComparator());
				
				//TODO n�o deveria ser necess�rio fazer essa ordena��o
				for (int i = 0; i < colorArray.length; i++) {
					Arrays.sort(colorArray[i]);
				}
				
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
