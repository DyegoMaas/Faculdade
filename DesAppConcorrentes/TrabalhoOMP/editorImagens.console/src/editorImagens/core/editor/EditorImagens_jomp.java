package editorImagens.core.editor;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import java.util.Arrays;

import editorImagens.core.utils.Colors;
import editorImagens.core.utils.ImageUtil;
import jomp.runtime.OMP;


/**
 * Esta classe centraliza as opera\u00e7\u00f5es complexas de edi\u00e7\u00e3o de imagem
 * 
 * IMPORTANTE: de prefer\u00eancia manter a implementa\u00e7\u00e3o original como coment\u00e1rio para servir de refer\u00eancia
 */
public class EditorImagens_jomp implements IEditorImagens {

	
	/**
	 * Foi utilizado o algoritmo de blur baseado no c\u00e1lculo da mediana.
	 * Utilizamos um for paralelo com 50 threads e agendamento para no m\u00ednimo 3 execu\u00e7\u00f5es por
	 */
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
		
		OMP.setNumThreads(10);

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
		
		
		//PSEUDO C\u00d3DIGO
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
	
	/**
	 * Utiliza\u00e7\u00e3o de um bloco paralelo simples para o c\u00e1lculo do mosaico. Cada coluna de quadrados
	 * do mosaico \u00e9 executada por uma thread.
	 */
	public void mosaico(BufferedImage imagem, int tamanhoCelulas){
//		int imageWidth = imagem.getWidth();
//		int imageHeight = imagem.getHeight();
//		
//		for (int x = 0; x < imageWidth; x += tamanhoCelulas) {
//			for (int y = 0; y < imageHeight; y += tamanhoCelulas) {
//				int w = (x + tamanhoCelulas > imageWidth)  ? imageWidth - x : tamanhoCelulas;
//				int h = (y + tamanhoCelulas > imageHeight)  ? imageHeight - y : tamanhoCelulas;		
//				//System.out.printf("x: %d, w: %d, y: %d, h: %d \n", x, w, y, h);
//				
//				BufferedImage subimage = imagem.getSubimage(x, y, w, h);
//				Color corMedia = calcularCorMediaSingleThread(subimage);
//				setarCor(imagem, corMedia, x, y, w, h);
//			}
//		}
		
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int x = 0;
		
		int numThreads = imageWidth / tamanhoCelulas + 1;
		OMP.setNumThreads(numThreads);

// OMP PARALLEL BLOCK BEGINS
{
  __omp_Class4 __omp_Object4 = new __omp_Class4();
  // shared variables
  __omp_Object4.numThreads = numThreads;
  __omp_Object4.imageHeight = imageHeight;
  __omp_Object4.imageWidth = imageWidth;
  __omp_Object4.tamanhoCelulas = tamanhoCelulas;
  __omp_Object4.imagem = imagem;
  // firstprivate variables
  try {
    jomp.runtime.OMP.doParallel(__omp_Object4);
  } catch(Throwable __omp_exception) {
    System.err.println("OMP Warning: Illegal thread exception ignored!");
    System.err.println(__omp_exception);
  }
  // reduction variables
  // shared variables
  numThreads = __omp_Object4.numThreads;
  imageHeight = __omp_Object4.imageHeight;
  imageWidth = __omp_Object4.imageWidth;
  tamanhoCelulas = __omp_Object4.tamanhoCelulas;
  imagem = __omp_Object4.imagem;
}
// OMP PARALLEL BLOCK ENDS

	}
	
	/**
	 * Uso de um bloco paralelo com redu\u00e7\u00e3o de soma dos componentes das cores da imagem 
	 */
	public Color calcularCorMedia(BufferedImage imagem){
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int somaComponenteR = 0;
		int somaComponenteG = 0;
		int somaComponenteB = 0;
		int x = 0, y = 0;
		int rgb = 0;
		int threadId = 0;
		int larguraBloco = imageWidth / 10;
		int inicioX = 0;
		
		OMP.setNumThreads(10);

// OMP PARALLEL BLOCK BEGINS
{
  __omp_Class5 __omp_Object5 = new __omp_Class5();
  // shared variables
  __omp_Object5.larguraBloco = larguraBloco;
  __omp_Object5.imageHeight = imageHeight;
  __omp_Object5.imageWidth = imageWidth;
  __omp_Object5.imagem = imagem;
  // firstprivate variables
  try {
    jomp.runtime.OMP.doParallel(__omp_Object5);
  } catch(Throwable __omp_exception) {
    System.err.println("OMP Warning: Illegal thread exception ignored!");
    System.err.println(__omp_exception);
  }
  // reduction variables
  somaComponenteR  += __omp_Object5._rd_somaComponenteR;
  somaComponenteG  += __omp_Object5._rd_somaComponenteG;
  somaComponenteB  += __omp_Object5._rd_somaComponenteB;
  // shared variables
  larguraBloco = __omp_Object5.larguraBloco;
  imageHeight = __omp_Object5.imageHeight;
  imageWidth = __omp_Object5.imageWidth;
  imagem = __omp_Object5.imagem;
}
// OMP PARALLEL BLOCK ENDS

		
		int numPixels = imageWidth * imageHeight;
		int mediaR = somaComponenteR /= numPixels;
		int mediaG = somaComponenteG /= numPixels;
		int mediaB = somaComponenteB /= numPixels;
		Color corMedia = new Color(mediaR, mediaG, mediaB);
		
		return corMedia;
	}

	/**
	 * Vers\u00e3o single thread do c\u00e1lculo de cor m\u00e9dia para ser utilizado em outros algoritmos paralelizados.
	 */
	private Color calcularCorMediaSingleThread(BufferedImage imagem){
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int somaComponenteR = 0;
		int somaComponenteG = 0;
		int somaComponenteB = 0;
				
		for(int x = 0; x < imageWidth; x++)
		{			
			for (int y = 0; y < imageHeight; y++) {
				int rgb = imagem.getRGB(x, y);
				
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
		
		int x = 0, y = 0;
		for (x = 0; x < imageWidth; x++) {
			for (y = 0; y < imageHeight; y++) {
				int rgb = imagem.getRGB(x, y);				
				imagem.setRGB(x, y, ImageUtil.inverterCor(rgb));
			}
		}
	}

	public void xorBlendingComCorMedia(BufferedImage imagem) {
		BufferedImage copia = new BufferedImage(imagem.getWidth(), imagem.getHeight(), BufferedImage.TYPE_INT_RGB);
		Graphics2D g = copia.createGraphics();
		
		int corMediaInvertida = ImageUtil.inverterCor(calcularCorMedia(imagem).getRGB());
		g.setXORMode(new Color(
				Colors.red(corMediaInvertida), 
				Colors.green(corMediaInvertida), 
				Colors.blue(corMediaInvertida), 0));
		g.drawImage(imagem, null, 0, 0);
		g.dispose();
		
		Graphics g2 = imagem.getGraphics();
		g2.drawImage(copia, 0, 0, null);
	}

	public void desaturar(BufferedImage imagem, float percentual){		
//		//descobrir o componente de cor saturado
//		Color corMedia = calcularCorMedia(imagem);		
//		int[] componentes = ImageUtil.arrayComponentes(corMedia.getRGB());		
//		int maxValue = -1;
//		int indexCorSaturada = 0;
//		for (int i = 0; i < componentes.length; i++) {
//			if(componentes[i] > maxValue){
//				maxValue = componentes[i];
//				indexCorSaturada = i;
//			}
//		}
//		
//		int imageWidth = imagem.getWidth();
//		int imageHeight = imagem.getHeight();
//				
//		//desaturando a imagem
//		for (int x = 0; x < imageWidth; x++) {
//			for (int y = 0; y < imageHeight; y++) {
//				
//				int rgb = imagem.getRGB(x, y);
//				componentes = ImageUtil.arrayComponentes(rgb);
//				
//				int componenteDesaturado = (int)(componentes[indexCorSaturada] * (1f - percentual));
//				componentes[indexCorSaturada] = Math.max(0, componenteDesaturado);
//				
//				Color corDesaturada = ImageUtil.converterParaRGB(componentes);
//				rgb = corDesaturada.getRGB();
//				
//				imagem.setRGB(x, y, rgb);
//			}
//		}	
		
		//descobrir o componente de cor saturado
		Color corMedia = calcularCorMedia(imagem);		
		int[] componentes = ImageUtil.arrayComponentes(corMedia.getRGB());		
		int maxValue = -1;
		int indexCorSaturada = 0;
		for (int i = 0; i < componentes.length; i++) {
			if(componentes[i] > maxValue){
				maxValue = componentes[i];
				indexCorSaturada = i;
			}
		}
		
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
				
		//desaturando a imagem
		for (int x = 0; x < imageWidth; x++) {
			for (int y = 0; y < imageHeight; y++) {
				
				int rgb = imagem.getRGB(x, y);
				componentes = ImageUtil.arrayComponentes(rgb);
				
				int componenteDesaturado = (int)(componentes[indexCorSaturada] * (1f - percentual));
				componentes[indexCorSaturada] = Math.max(0, componenteDesaturado);
				
				Color corDesaturada = ImageUtil.converterParaRGB(componentes);
				rgb = corDesaturada.getRGB();
				
				imagem.setRGB(x, y, rgb);
			}
		}	
	}

	/**
	 * Cada bloco distorce um dos componentes de cor da imagem. As se\u00e7\u00f5es cr\u00edticas s\u00e3o utilizadas para 
	 * evitar que duas se\u00e7\u00f5es atuem sobre a cor original de um pixel. Isto porque a inten\u00e7\u00e3o \u00e9 que cada 
	 * pixel resultante tenha cada um de seus componentes distorcidos. 
	 */
	public void distorcerCores(BufferedImage imagem, float distorcaoR, float distorcaoG, float distorcaoB){		
		OMP.setNumThreads(3);
		
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int x = 0, y = 0;
		int rgb = 0;
		int r = 0, g = 0, b = 0;

// OMP PARALLEL BLOCK BEGINS
{
  __omp_Class6 __omp_Object6 = new __omp_Class6();
  // shared variables
  __omp_Object6.imageHeight = imageHeight;
  __omp_Object6.imageWidth = imageWidth;
  __omp_Object6.distorcaoB = distorcaoB;
  __omp_Object6.distorcaoG = distorcaoG;
  __omp_Object6.distorcaoR = distorcaoR;
  __omp_Object6.imagem = imagem;
  // firstprivate variables
  try {
    jomp.runtime.OMP.doParallel(__omp_Object6);
  } catch(Throwable __omp_exception) {
    System.err.println("OMP Warning: Illegal thread exception ignored!");
    System.err.println(__omp_exception);
  }
  // reduction variables
  // shared variables
  imageHeight = __omp_Object6.imageHeight;
  imageWidth = __omp_Object6.imageWidth;
  distorcaoB = __omp_Object6.distorcaoB;
  distorcaoG = __omp_Object6.distorcaoG;
  distorcaoR = __omp_Object6.distorcaoR;
  imagem = __omp_Object6.imagem;
}
// OMP PARALLEL BLOCK ENDS
		
	}
	
	public void setarCor(BufferedImage imagem, Color novaCor) {
		setarCor(imagem, novaCor, 0, 0, imagem.getWidth(), imagem.getHeight());
	}
	
	public void setarCor(BufferedImage imagem, Color novaCor, int x, int y, int w, int h){
		Graphics g = imagem.getGraphics();
		g.setColor(novaCor);
		g.fillRect(x, y, w, h);
	}

// OMP PARALLEL REGION INNER CLASS DEFINITION BEGINS
private class __omp_Class6 extends jomp.runtime.BusyTask {
  // shared variables
  int imageHeight;
  int imageWidth;
  float distorcaoB;
  float distorcaoG;
  float distorcaoR;
  BufferedImage imagem;
  // firstprivate variables
  // variables to hold results of reduction

  public void go(int __omp_me) throws Throwable {
  // firstprivate variables + init
  // private variables
  int x;
  int y;
  int rgb;
  int r;
  int g;
  int b;
  // reduction variables, init to default
    // OMP USER CODE BEGINS

		{
                         { // OMP SECTIONS BLOCK BEGINS
                         // copy of firstprivate variables, initialized
                         // copy of lastprivate variables
                         // variables to hold result of reduction
                         boolean amLast=false;
                         {
                           // firstprivate variables + init
                           // [last]private variables
                           // reduction variables + init to default
                           // -------------------------------------
                           __ompName_7: while(true) {
                           switch((int)jomp.runtime.OMP.getTicket(__omp_me)) {
                           // OMP SECTION BLOCK 0 BEGINS
                             case 0: {
                           // OMP USER CODE BEGINS

				{
					for (x = 0; x < imageWidth; x++) {
						for (y = 0; y < imageHeight; y++) {
							rgb = imagem.getRGB(x, y);
							r = Colors.red(rgb);
							g = Colors.green(rgb);
							b = Colors.blue(rgb);
							
							//distor\u00e7\u00e3o do vermelho
							r = Math.min(255, Math.max(0, (int)(r * distorcaoR)));							
							rgb = new Color(r, g, b).getRGB();
                                                         // OMP CRITICAL BLOCK BEGINS
                                                         synchronized (jomp.runtime.OMP.getLockByName("")) {
                                                         // OMP USER CODE BEGINS

							{
								imagem.setRGB(x, y, rgb);
							}
                                                         // OMP USER CODE ENDS
                                                         }
                                                         // OMP CRITICAL BLOCK ENDS

						}
					}
				}
                           // OMP USER CODE ENDS
                             };  break;
                           // OMP SECTION BLOCK 0 ENDS
                           // OMP SECTION BLOCK 1 BEGINS
                             case 1: {
                           // OMP USER CODE BEGINS

				{
					for (x = 0; x < imageWidth; x++) {
						for (y = 0; y < imageHeight; y++) {
							rgb = imagem.getRGB(x, y);
							r = Colors.red(rgb);
							g = Colors.green(rgb);
							b = Colors.blue(rgb);
							
							//distor\u00e7\u00e3o do verde
							g = Math.min(255, Math.max(0, (int)(g * distorcaoG)));							
							rgb = new Color(r, g, b).getRGB();
                                                         // OMP CRITICAL BLOCK BEGINS
                                                         synchronized (jomp.runtime.OMP.getLockByName("")) {
                                                         // OMP USER CODE BEGINS

							{
								imagem.setRGB(x, y, rgb);
							}
                                                         // OMP USER CODE ENDS
                                                         }
                                                         // OMP CRITICAL BLOCK ENDS

						}
					}
				}
                           // OMP USER CODE ENDS
                             };  break;
                           // OMP SECTION BLOCK 1 ENDS
                           // OMP SECTION BLOCK 2 BEGINS
                             case 2: {
                           // OMP USER CODE BEGINS

				{
					for (x = 0; x < imageWidth; x++) {
						for (y = 0; y < imageHeight; y++) {
							rgb = imagem.getRGB(x, y);
							r = Colors.red(rgb);
							g = Colors.green(rgb);
							b = Colors.blue(rgb);
							
							//distor\u00e7\u00e3o do azul
							b = Math.min(255, Math.max(0, (int)(b * distorcaoB)));							
							rgb = new Color(r, g, b).getRGB();
                                                         // OMP CRITICAL BLOCK BEGINS
                                                         synchronized (jomp.runtime.OMP.getLockByName("")) {
                                                         // OMP USER CODE BEGINS

							{
								imagem.setRGB(x, y, rgb);
							}
                                                         // OMP USER CODE ENDS
                                                         }
                                                         // OMP CRITICAL BLOCK ENDS

						}
					}
				}
                           // OMP USER CODE ENDS
                           amLast = true;
                             };  break;
                           // OMP SECTION BLOCK 2 ENDS

                             default: break __ompName_7;
                           } // of switch
                           } // of while
                           // call reducer
                           jomp.runtime.OMP.resetTicket(__omp_me);
                           jomp.runtime.OMP.doBarrier(__omp_me);
                           // copy lastprivate variables out
                           if (amLast) {
                           }
                         }
                         // update lastprivate variables
                         if (amLast) {
                         }
                         // update reduction variables
                         if (jomp.runtime.OMP.getThreadNum(__omp_me) == 0) {
                         }
                         } // OMP SECTIONS BLOCK ENDS

		}
    // OMP USER CODE ENDS
  // call reducer
  // output to _rd_ copy
  if (jomp.runtime.OMP.getThreadNum(__omp_me) == 0) {
  }
  }
}
// OMP PARALLEL REGION INNER CLASS DEFINITION ENDS



// OMP PARALLEL REGION INNER CLASS DEFINITION BEGINS
private class __omp_Class5 extends jomp.runtime.BusyTask {
  // shared variables
  int larguraBloco;
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
  int inicioX;
  int threadId;
  int x;
  int y;
  int rgb;
  // reduction variables, init to default
  int somaComponenteR = 0;
  int somaComponenteG = 0;
  int somaComponenteB = 0;
    // OMP USER CODE BEGINS

		{			
			threadId = OMP.getThreadNum();
			inicioX = threadId * larguraBloco;
			
			//System.out.printf("threadId: %d, bloco: %d, x(inicial): %d\n", threadId, larguraBloco, inicioX);			
			for(x = inicioX; x < inicioX + larguraBloco; x++)			
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
private class __omp_Class4 extends jomp.runtime.BusyTask {
  // shared variables
  int numThreads;
  int imageHeight;
  int imageWidth;
  int tamanhoCelulas;
  BufferedImage imagem;
  // firstprivate variables
  // variables to hold results of reduction

  public void go(int __omp_me) throws Throwable {
  // firstprivate variables + init
  // private variables
  int x;
  // reduction variables, init to default
    // OMP USER CODE BEGINS

		{
			x = OMP.getThreadNum() * tamanhoCelulas;	
			System.out.println("x: " + x);
			for (int y = 0; y < imageHeight; y += tamanhoCelulas) {
				int w = (x + tamanhoCelulas > imageWidth)  ? imageWidth - x : tamanhoCelulas;
				int h = (y + tamanhoCelulas > imageHeight)  ? imageHeight - y : tamanhoCelulas;		
				//System.out.printf("x: %d, w: %d, y: %d, h: %d \n", x, w, y, h);
				
				setarCor(imagem, calcularCorMediaSingleThread(imagem.getSubimage(x, y, w, h)),
						x, y, w, h);
			}
		}
    // OMP USER CODE ENDS
  // call reducer
  // output to _rd_ copy
  if (jomp.runtime.OMP.getThreadNum(__omp_me) == 0) {
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
                            __omp_WholeData2.chunkSize = (long)( 5);
                            jomp.runtime.OMP.initTicket(__omp_me, __omp_WholeData2);
                            while(!__omp_ChunkData1.isLast && jomp.runtime.OMP.getLoopDynamic(__omp_me, __omp_WholeData2, __omp_ChunkData1)) {
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
				
				//ordena\u00e7\u00e3o horizontal
				Arrays.sort(colorArray, ImageUtil.getIntArrayComparator());
				
				//ordena\u00e7\u00e3o vertical
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
                            } // of while
                            // call reducer
                            jomp.runtime.OMP.resetTicket(__omp_me);
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

