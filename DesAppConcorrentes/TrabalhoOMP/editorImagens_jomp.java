package editorImagens.core;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import java.util.Arrays;

import jomp.runtime.OMP;

public class EditorImagens_jomp implements IEditorImagens_jomp {


	//OK
	public void blur(BufferedImage imagem, int windowWidth, int windowHeight){
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int edgeX = (windowWidth / 2); //rounded down
		int edgeY = (windowHeight / 2); //rounded down

		int xJomp = 0;
		int y = 0;
		
		//vari\u00e1veis do algoritmo que devem ser declaradas aqui fora (do contr\u00e1rio as threads ficam travadas e ocorre uso excessivo de CPU)
		int fx = 0, fy = 0;
		int iArrays = 0;
		int mediana = 0;				
		
		OMP.setNumThreads(windowWidth);

// OMP PARALLEL BLOCK BEGINS
{
  __omp_Class0 __omp_Object0 = new __omp_Class0();
  // shared variables
  __omp_Object0.edgeY = edgeY;
  __omp_Object0.edgeX = edgeX;
  __omp_Object0.imageHeight = imageHeight;
  __omp_Object0.imageWidth = imageWidth;
  __omp_Object0.windowHeight = windowHeight;
  __omp_Object0.windowWidth = windowWidth;
  __omp_Object0.imagem = imagem;
  // firstprivate variables
  try {
    jomp.runtime.OMP.doParallel(__omp_Object0);
  } catch(Throwable __omp_exception) {
    System.err.println("OMP Warning: Illegal thread exception ignored!");
    System.err.println(__omp_exception);
  }
  // reduction variables
  // shared variables
  edgeY = __omp_Object0.edgeY;
  edgeX = __omp_Object0.edgeX;
  imageHeight = __omp_Object0.imageHeight;
  imageWidth = __omp_Object0.imageWidth;
  windowHeight = __omp_Object0.windowHeight;
  windowWidth = __omp_Object0.windowWidth;
  imagem = __omp_Object0.imagem;
}
// OMP PARALLEL BLOCK ENDS
		
		
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

	//OK
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
	}
	
	private Color calcularCorMedia(BufferedImage imagem){
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int somaComponenteR = 0;
		int somaComponenteG = 0;
		int somaComponenteB = 0;
		int x = 0, y = 0;
		int rgb = 0;
				
		OMP.setNumThreads(imageWidth);

// OMP PARALLEL BLOCK BEGINS
{
  __omp_Class4 __omp_Object4 = new __omp_Class4();
  // shared variables
  __omp_Object4.imageHeight = imageHeight;
  __omp_Object4.imageWidth = imageWidth;
  __omp_Object4.imagem = imagem;
  // firstprivate variables
  try {
    jomp.runtime.OMP.doParallel(__omp_Object4);
  } catch(Throwable __omp_exception) {
    System.err.println("OMP Warning: Illegal thread exception ignored!");
    System.err.println(__omp_exception);
  }
  // reduction variables
  somaComponenteR  += __omp_Object4._rd_somaComponenteR;
  somaComponenteG  += __omp_Object4._rd_somaComponenteG;
  somaComponenteB  += __omp_Object4._rd_somaComponenteB;
  // shared variables
  imageHeight = __omp_Object4.imageHeight;
  imageWidth = __omp_Object4.imageWidth;
  imagem = __omp_Object4.imagem;
}
// OMP PARALLEL BLOCK ENDS

		
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
		
		int x = 0, y = 0;
		for (x = 0; x < imageWidth; x++) {
			for (y = 0; y < imageHeight; y++) {
				int rgb = imagem.getRGB(x, y);				
				imagem.setRGB(x, y, inverterCor(rgb));
			}
		}
	}

	public void mediaInvertida(BufferedImage imagem) {
		media(imagem);
		inverterCores(imagem);
	}
	
	private int inverterCor(int rgb){
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
		
//		mediaInvertida(copia);
//		
//		int imageWidth = imagem.getWidth();
//		int imageHeight = imagem.getHeight();
//		
//		int x = 0, y = 0;
//		for (x = 0; x < imageWidth; x++) {
//			for (y = 0; y < imageHeight; y++) {				
//				int rgb = imagem.getRGB(x, y);
//				int rgbCopia = copia.getRGB(x, y);
//				
//				int red = Colors.red(rgb) + Colors.red(rgbCopia);
//				int green = Colors.green(rgb) + Colors.green(rgbCopia);
//				int blue = Colors.blue(rgb) + Colors.blue(rgbCopia);
//				
//				Color novaCor = new Color(red, green, blue);				
//				imagem.setRGB(x, y, novaCor.getRGB());
//			}
//		}
	}

// OMP PARALLEL REGION INNER CLASS DEFINITION BEGINS
private class __omp_Class4 extends jomp.runtime.BusyTask {
  // shared variables
  int imageHeight;
  int imageWidth;
  BufferedImage imagem;
  // firstprivate variables
  // variables to hold results of reduction
  int _rd_somaComponenteR;
  int _rd_somaComponenteG;
  int _rd_somaComponenteB;

  public void go(int __omp_me) throws Throwable {
  // firstprivate variables + init
  // private variables
  int x;
  int y;
  int rgb;
  // reduction variables, init to default
  int somaComponenteR = 0;
  int somaComponenteG = 0;
  int somaComponenteB = 0;
    // OMP USER CODE BEGINS

		{			
			x = OMP.getThreadNum();
			for (y = 0; y < imageHeight; y++) {
				rgb = imagem.getRGB(x, y);
				
				somaComponenteR += Colors.red(rgb);
				somaComponenteG += Colors.green(rgb);
				somaComponenteB += Colors.blue(rgb);
			}						
		}
    // OMP USER CODE ENDS
  // call reducer
  somaComponenteR = (int) jomp.runtime.OMP.doPlusReduce(__omp_me, somaComponenteR);
  somaComponenteG = (int) jomp.runtime.OMP.doPlusReduce(__omp_me, somaComponenteG);
  somaComponenteB = (int) jomp.runtime.OMP.doPlusReduce(__omp_me, somaComponenteB);
  // output to _rd_ copy
  if (jomp.runtime.OMP.getThreadNum(__omp_me) == 0) {
    _rd_somaComponenteR = somaComponenteR;
    _rd_somaComponenteG = somaComponenteG;
    _rd_somaComponenteB = somaComponenteB;
  }
  }
}
// OMP PARALLEL REGION INNER CLASS DEFINITION ENDS



// OMP PARALLEL REGION INNER CLASS DEFINITION BEGINS
private class __omp_Class0 extends jomp.runtime.BusyTask {
  // shared variables
  int edgeY;
  int edgeX;
  int imageHeight;
  int imageWidth;
  int windowHeight;
  int windowWidth;
  BufferedImage imagem;
  // firstprivate variables
  // variables to hold results of reduction

  public void go(int __omp_me) throws Throwable {
  // firstprivate variables + init
  // private variables
  int xJomp;
  int y;
  int fx;
  int fy;
  int iArrays;
  int mediana;
  // reduction variables, init to default
    // OMP USER CODE BEGINS

                          { // OMP FOR BLOCK BEGINS
                          // copy of firstprivate variables, initialized
                          // copy of lastprivate variables
                          // variables to hold result of reduction
                          boolean amLast=false;
                          {
                            // firstprivate variables + init
                            // [last]private variables
                            // reduction variables + init to default
                            // -------------------------------------
                            jomp.runtime.LoopData __omp_WholeData2 = new jomp.runtime.LoopData();
                            jomp.runtime.LoopData __omp_ChunkData1 = new jomp.runtime.LoopData();
                            __omp_WholeData2.start = (long)( 0);
                            __omp_WholeData2.stop = (long)( (imageWidth - edgeX * 2));
                            __omp_WholeData2.step = (long)(1);
                            jomp.runtime.OMP.setChunkStatic(__omp_WholeData2);
                            while(!__omp_ChunkData1.isLast && jomp.runtime.OMP.getLoopStatic(__omp_me, __omp_WholeData2, __omp_ChunkData1)) {
                            for(;;) {
                              if(__omp_WholeData2.step > 0) {
                                 if(__omp_ChunkData1.stop > __omp_WholeData2.stop) __omp_ChunkData1.stop = __omp_WholeData2.stop;
                                 if(__omp_ChunkData1.start >= __omp_WholeData2.stop) break;
                              } else {
                                 if(__omp_ChunkData1.stop < __omp_WholeData2.stop) __omp_ChunkData1.stop = __omp_WholeData2.stop;
                                 if(__omp_ChunkData1.start > __omp_WholeData2.stop) break;
                              }
                              for( xJomp = (int)__omp_ChunkData1.start; xJomp < __omp_ChunkData1.stop; xJomp += __omp_ChunkData1.step) {
                                // OMP USER CODE BEGINS
 {
			int x = xJomp + edgeX;
			for (y = edgeY; y < (imageHeight - edgeY); y++) {
				int[][] colorArray = new int[windowWidth][windowHeight];
				
				for (fx = 0; fx < windowWidth; fx++) {
					for (fy = 0; fy < windowHeight; fy++) {
						colorArray[fx][fy] = imagem.getRGB(x + fx - edgeX, y + fy - edgeY); //&0xff 
					}
				}
				
				Arrays.sort(colorArray, ImageUtil.getIntArrayComparator());
				
				//TODO n\u00e3o deveria ser necess\u00e1rio fazer essa ordena\u00e7\u00e3o
				for (iArrays = 0; iArrays < colorArray.length; iArrays++) {
					Arrays.sort(colorArray[iArrays]);
				}
				
				mediana = colorArray[windowWidth / 2][windowHeight / 2];
				imagem.setRGB(x, y, mediana);
			}	
		}
                                // OMP USER CODE ENDS
                                if (xJomp == (__omp_WholeData2.stop-1)) amLast = true;
                              } // of for 
                              if(__omp_ChunkData1.startStep == 0)
                                break;
                              __omp_ChunkData1.start += __omp_ChunkData1.startStep;
                              __omp_ChunkData1.stop += __omp_ChunkData1.startStep;
                            } // of for(;;)
                            } // of while
                            // call reducer
                            jomp.runtime.OMP.doBarrier(__omp_me);
                            // copy lastprivate variables out
                            if (amLast) {
                            }
                          }
                          // set global from lastprivate variables
                          if (amLast) {
                          }
                          // set global from reduction variables
                          if (jomp.runtime.OMP.getThreadNum(__omp_me) == 0) {
                          }
                          } // OMP FOR BLOCK ENDS

    // OMP USER CODE ENDS
  // call reducer
  // output to _rd_ copy
  if (jomp.runtime.OMP.getThreadNum(__omp_me) == 0) {
  }
  }
}
// OMP PARALLEL REGION INNER CLASS DEFINITION ENDS

}

