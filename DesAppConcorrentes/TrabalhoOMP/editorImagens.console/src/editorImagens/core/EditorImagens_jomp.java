package editorImagens.core;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.image.BufferedImage;
import java.util.Arrays;

import jomp.runtime.OMP;

public class EditorImagens_jomp implements IEditorImagens{


	public void avacalharImagem(BufferedImage imagem){
		Graphics g = imagem.getGraphics();
		g.setColor(Color.GREEN);
		g.fillOval(10, 20, 300, 100);
		g.drawOval(10, 10, 300, 100);
	}

	public void mediana(BufferedImage imagem, int windowWidth, int windowHeight){
		OMP.setNumThreads(windowWidth);
		
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int edgeX = (windowWidth / 2); //rounded down
		int edgeY = (windowHeight / 2); //rounded down

		for (int x = edgeX; x < (imageWidth - edgeX); x++) {
			for (int y = edgeY; y < (imageHeight - edgeY); y++) {
				int[][] colorArray = new int[windowWidth][windowHeight];
				
				int fx = 0;

// OMP PARALLEL BLOCK BEGINS
{
  __omp_Class0 __omp_Object0 = new __omp_Class0();
  // shared variables
  __omp_Object0.colorArray = colorArray;
  __omp_Object0.y = y;
  __omp_Object0.x = x;
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
  colorArray = __omp_Object0.colorArray;
  y = __omp_Object0.y;
  x = __omp_Object0.x;
  edgeY = __omp_Object0.edgeY;
  edgeX = __omp_Object0.edgeX;
  imageHeight = __omp_Object0.imageHeight;
  imageWidth = __omp_Object0.imageWidth;
  windowHeight = __omp_Object0.windowHeight;
  windowWidth = __omp_Object0.windowWidth;
  imagem = __omp_Object0.imagem;
}
// OMP PARALLEL BLOCK ENDS

				
				Arrays.sort(colorArray, ImageUtil.getIntArrayComparator());
				
				//TODO n\u00e3o deveria ser necess\u00e1rio fazer essa ordena\u00e7\u00e3o
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

// OMP PARALLEL REGION INNER CLASS DEFINITION BEGINS
private class __omp_Class0 extends jomp.runtime.BusyTask {
  // shared variables
  int [ ] [ ] colorArray;
  int y;
  int x;
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
  int fx;
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
                                            __omp_WholeData2.stop = (long)( windowWidth);
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
                                              for( fx = (int)__omp_ChunkData1.start; fx < __omp_ChunkData1.stop; fx += __omp_ChunkData1.step) {
                                                // OMP USER CODE BEGINS
 {
					for (int fy = 0; fy < windowHeight; fy++) {
						colorArray[fx][fy] = imagem.getRGB(x + fx - edgeX, y + fy - edgeY); //&0xff 
					}
				}
                                                // OMP USER CODE ENDS
                                                if (fx == (__omp_WholeData2.stop-1)) amLast = true;
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


/*int rgb = img.getRGB(x, y);
int r = (rgb >> 16) & 0xFF;
int g = (rgb >> 8) & 0xFF;
int b = (rgb & 0xFF);*/
