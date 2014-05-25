package editorImagens.core;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import java.util.Arrays;

import jomp.runtime.OMP;


/**
 * IMPORTANTE: de prefer�ncia manter a implementa��o original como coment�rio para servir de refer�ncia 
 *
 */
public class EditorImagens_normal implements IEditorImagens{

	//NOK
	public void blur(BufferedImage imagem, int windowWidth, int windowHeight){
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int edgeX = (windowWidth / 2); //rounded down
		int edgeY = (windowHeight / 2); //rounded down

		int xJomp = 0;
		int y = 0;
		
		//vari�veis do algoritmo que devem ser declaradas aqui fora (do contr�rio as threads ficam travadas e ocorre uso excessivo de CPU)
		int fx = 0, fy = 0;
		int iArrays = 0;
		int mediana = 0;				
		
		OMP.setNumThreads(windowWidth);
		//omp parallel for private(xJomp, y, fx, fy, iArrays, mediana)
		for (xJomp = 0; xJomp < (imageWidth - edgeX * 2); xJomp++) {
			int x = xJomp + edgeX;
			for (y = edgeY; y < (imageHeight - edgeY); y++) {
				int[][] colorArray = new int[windowWidth][windowHeight];
				
				for (fx = 0; fx < windowWidth; fx++) {
					for (fy = 0; fy < windowHeight; fy++) {
						colorArray[fx][fy] = imagem.getRGB(x + fx - edgeX, y + fy - edgeY); //&0xff 
					}
				}
				
				Arrays.sort(colorArray, ImageUtil.getIntArrayComparator());
				
				//TODO n�o deveria ser necess�rio fazer essa ordena��o
				for (iArrays = 0; iArrays < colorArray.length; iArrays++) {
					Arrays.sort(colorArray[iArrays]);
				}
				
				mediana = colorArray[windowWidth / 2][windowHeight / 2];
				imagem.setRGB(x, y, mediana);
			}	
		}		
		
		//PSEUDO C�DIGO
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

//		int imageWidth = imagem.getWidth();
//		int imageHeight = imagem.getHeight();
////		int imageWidth = imagem.getWidth();
////		int imageHeight = imagem.getHeight();
//	
//		int x = 0, y = 0;
//		for (x = 0; x < imageWidth; x++) {
//			for (y = 0; y < imageHeight; y++) {				
//				imagem.setRGB(x, y, corMedia.getRGB());
//			}
//		}
//		Graphics g = imagem.getGraphics();
//		g.setColor(corMedia);
//		g.fillRect(0, 0, 0, 0);
//		
////		int x = 0, y = 0;
////		for (x = 0; x < imageWidth; x++) {
////			for (y = 0; y < imageHeight; y++) {				
////				imagem.setRGB(x, y, corMedia.getRGB());
////			}
////		}
	
	public Color calcularCorMedia(BufferedImage imagem){
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int somaComponenteR = 0;
		int somaComponenteG = 0;
		int somaComponenteB = 0;
		int x = 0, y = 0;
		int rgb = 0;
				
		OMP.setNumThreads(imageWidth);
		//omp parallel private(x,y,rgb) reduction(+:somaComponenteR,somaComponenteG,somaComponenteB)
		{			
			x = OMP.getThreadNum();
			for (y = 0; y < imageHeight; y++) {
				rgb = imagem.getRGB(x, y);
				
				somaComponenteR += Colors.red(rgb);
				somaComponenteG += Colors.green(rgb);
				somaComponenteB += Colors.blue(rgb);
			}						
		}
		
		int numPixels = imageWidth * imageHeight;
		int mediaR = somaComponenteR /= numPixels;
		int mediaG = somaComponenteG /= numPixels;
		int mediaB = somaComponenteB /= numPixels;
		Color corMedia = new Color(mediaR, mediaG, mediaB);
		
		return corMedia;
	}

	public void inverterCores(BufferedImage imagem) {
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
			
//		int primeiroQuadranteX = imageWidth / 2;
//		int primeiroQuadranteY = imageHeight / 2;
		
////		int segundoQuadrante X = ima
//		
//		//omp sections
//		{
//			
//			
//			//omp section
//			{
//				for (int x = 0; x < primeiroQuadranteX; x++) {
//					
//				}
//			}
//		}
//		imagem.getTile(tileX, tileY)(x, y, w, h)
		int x = 0, y = 0;
		for (x = 0; x < imageWidth; x++) {
			for (y = 0; y < imageHeight; y++) {
				int rgb = imagem.getRGB(x, y);				
				imagem.setRGB(x, y, inverterCor(rgb));
			}
		}
	}
	
	public Color inverterCor(Color cor){
		int corInvertida = inverterCor(cor.getRGB());		
		return new Color(Colors.red(corInvertida), Colors.green(corInvertida), Colors.blue(corInvertida));
	}
	
	public int inverterCor(int rgb){
		int red = 255 - Colors.red(rgb);
		int green = 255 - Colors.green(rgb);
		int blue = 255 - Colors.blue(rgb);
		
		Color corInvertida = new Color(red, green, blue);		
		return corInvertida.getRGB();
	}

	//NOK
	public void desaturarCorMedia(BufferedImage imagem) {
		BufferedImage copia = new BufferedImage(imagem.getWidth(), imagem.getHeight(), BufferedImage.TYPE_INT_RGB);
		Graphics2D g = copia.createGraphics();
		
		int corMediaInvertida = inverterCor(calcularCorMedia(imagem).getRGB());
		g.setXORMode(new Color(
				Colors.red(corMediaInvertida), 
				Colors.green(corMediaInvertida), 
				Colors.blue(corMediaInvertida), 0));
		g.drawImage(imagem, null, 0, 0);
		g.dispose();
		
		Graphics g2 = imagem.getGraphics();
		g2.drawImage(copia, 0, 0, null);
	}

	public void setarCor(BufferedImage imagem, Color novaCor) {
		Graphics g = imagem.getGraphics();
		g.setColor(novaCor);
		g.fillRect(0, 0, imagem.getWidth(), imagem.getHeight());
	}
	
	/*//OK
	public void media(BufferedImage imagem) {
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		Color corMedia = calcularCorMedia(imagem);
		int x = 0, y = 0;
		for (x = 0; x < imageWidth; x++) {
			for (y = 0; y < imageHeight; y++) {				
				imagem.setRGB(x, y, corMedia.getRGB());
			}
		}
	}*/
}