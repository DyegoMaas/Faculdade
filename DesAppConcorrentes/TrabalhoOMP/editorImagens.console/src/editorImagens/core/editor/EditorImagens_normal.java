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
 * Esta classe centraliza as operações complexas de edição de imagem
 * 
 * IMPORTANTE: de preferência manter a implementação original como comentário para servir de referência
 */
public class EditorImagens_normal implements IEditorImagens{
	
	/**
	 * Foi utilizado o algoritmo de blur baseado no cálculo da mediana.
	 * Utilizamos um for paralelo com 10 threads e agendamento dinâmico em chuncks de 5 execuções por thread
	 */
	public void blur(BufferedImage imagem, int windowWidth, int windowHeight){
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int edgeX = (windowWidth / 2); //rounded down
		int edgeY = (windowHeight / 2); //rounded down

		int xJomp = 0;
		int y = 0;
		
		//variáveis do algoritmo que devem ser declaradas aqui fora (do contrário as threads ficam travadas e ocorre uso excessivo de CPU)
		int fx = 0, fy = 0;
		int iArrays = 0;
		int mediana = 0;				
		
		OMP.setNumThreads(10);
		//omp parallel for private(xJomp, y, fx, fy, iArrays, mediana) schedule(dynamic, 5)
		for (xJomp = 0; xJomp < (imageWidth - edgeX * 2); xJomp++) {
			int x = xJomp + edgeX;
			for (y = edgeY; y < (imageHeight - edgeY); y++) {
				int[][] colorArray = new int[windowWidth][windowHeight];
				
				for (fx = 0; fx < windowWidth; fx++) {
					for (fy = 0; fy < windowHeight; fy++) {
						colorArray[fx][fy] = imagem.getRGB(x + fx - edgeX, y + fy - edgeY); //&0xff 
					}
				}
				
				//ordenação horizontal
				Arrays.sort(colorArray, ImageUtil.getIntArrayComparator());
				
				//ordenação vertical
				for (iArrays = 0; iArrays < colorArray.length; iArrays++) {
					Arrays.sort(colorArray[iArrays]);
				}
				
				mediana = colorArray[windowWidth / 2][windowHeight / 2];
				imagem.setRGB(x, y, mediana);
			}	
		}		
		
		//PSEUDO CÓDIGO
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
	 * Utilização de um bloco paralelo simples para o cálculo do mosaico. Cada coluna de quadrados
	 * do mosaico é executada por uma thread.
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
		
		int x = 0, y = 0;
		int w = 0, h = 0;
		
		int numThreads = imageWidth / tamanhoCelulas + 1;
		OMP.setNumThreads(numThreads);
		//omp parallel private(x,y,w,h)
		{
			x = OMP.getThreadNum() * tamanhoCelulas;	
			System.out.println("x: " + x);
			for (y = 0; y < imageHeight; y += tamanhoCelulas) {
				w = (x + tamanhoCelulas > imageWidth)  ? imageWidth - x : tamanhoCelulas;
				h = (y + tamanhoCelulas > imageHeight)  ? imageHeight - y : tamanhoCelulas;		
				//System.out.printf("x: %d, w: %d, y: %d, h: %d \n", x, w, y, h);
				
				setarCor(imagem, calcularCorMediaSingleThread(imagem.getSubimage(x, y, w, h)),
						x, y, w, h);
			}
		}
	}
	
	/**
	 * Uso de um bloco paralelo com redução de soma dos componentes das cores da imagem 
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
		//omp parallel private(inicioX,threadId,x,y,rgb) reduction(+:somaComponenteR,somaComponenteG,somaComponenteB)
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
		//atingida uma barreira implicita onde todas as threads sincronizam
		
		int numPixels = imageWidth * imageHeight;
		int mediaR = somaComponenteR /= numPixels;
		int mediaG = somaComponenteG /= numPixels;
		int mediaB = somaComponenteB /= numPixels;
		Color corMedia = new Color(mediaR, mediaG, mediaB);
		
		return corMedia;
	}

	/**
	 * Versão single thread do cálculo de cor média para ser utilizado em outros algoritmos paralelizados.
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

	public void dessaturar(BufferedImage imagem, float fatorDesaturacao){	
		//calculo paralelo
		Color corMedia = calcularCorMedia(imagem);
				
		int rgb = corMedia.getRGB();
		int r = Colors.red(rgb);
		int g = Colors.green(rgb);
		int b = Colors.blue(rgb);
		
		int rDiff = (int)(r * fatorDesaturacao);
		int gDiff = (int)(g * fatorDesaturacao);
		int bDiff = (int)(b * fatorDesaturacao);
		
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();

		//TODO: poderia ser um parallel for
		//desaturando a imagem
		for (int x = 0; x < imageWidth; x++) {
			for (int y = 0; y < imageHeight; y++) {
				
				rgb = imagem.getRGB(x, y);
				r = Math.max(0, Colors.red(rgb) - rDiff);
				g = Math.max(0, Colors.green(rgb) - gDiff);
				b = Math.max(0, Colors.blue(rgb) - bDiff);
				
				rgb = new Color(r, g, b).getRGB();
				imagem.setRGB(x, y, rgb);
			}
		}	
	}

	/**
	 * Uma das threads calcula a cor media da imagem original. 
	 * Enquanto isso, as outras threads distorcem a cor original da imagem para deixa-la avermelhada.
	 * Em seguida, todas as threads sincronizam e entao a cor media da imagem original eh utilizada 
	 * para distorcer a cor da imagem.
	 */
	public void outraDistorcao(BufferedImage imagem){		
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();	
	
		int larguraBloco = imageWidth / 10;
		
		int corMedia = 0;
		int threadId = 0;
		int inicioX = 0;
		int x = 0, y = 0;
		int rgb = 0;
		int r = 0, g = 0, b = 0;
		
		BufferedImage copiaImagem = ImageUtil.criarCopia(imagem);
		
		OMP.setNumThreads(10);
		//omp parallel private(corMedia,threadId,inicioX,x,y,rgb,r,g,b)
		{
			
			//tem que estar em um
			//omp single
			{
				corMedia = calcularCorMediaSingleThread(copiaImagem).getRGB();
			}
			
			threadId = OMP.getThreadNum();
			inicioX = threadId * larguraBloco;
			
			//TODO converter para omp for
			for(x = inicioX; x < inicioX + larguraBloco; x++){			
				for (y = 0; y < imageHeight; y++) {
					//pixel avermelhado
					rgb = distorcerCor(imagem.getRGB(x, y), 1f, .5f, .5f);
					imagem.setRGB(x, y, rgb);					
				}
			}
			
			//omp barrier

			//TODO converter para omp for
			for(x = inicioX; x < inicioX + larguraBloco; x++){			
				for (y = 0; y < imageHeight; y++) {
					rgb = imagem.getRGB(x, y);

					//componentes da cor avermelhada
					r = Colors.red(rgb);
					g = Colors.green(rgb);
					b = Colors.blue(rgb);
					
					//inversao da cor media
					rgb = ImageUtil.inverterCor(corMedia);
					
					//subtracao da media invertida da cor avermelhada
					r = Math.abs(r - (int)(Colors.red(rgb)));
					g = Math.abs(g - (int)(Colors.green(rgb)));
					b = Math.abs(b - (int)(Colors.blue(rgb)));
					
					//criacao da nova cor
					rgb = new Color(r, g, b).getRGB();
					imagem.setRGB(x, y, rgb);					
				}
			}
		}
	}

	/**
	 * Cada bloco distorce um dos componentes de cor da imagem. As seções críticas são utilizadas para 
	 * evitar que duas seções atuem sobre a cor original de um pixel. Isto porque a intenção é que cada 
	 * pixel resultante tenha cada um de seus componentes distorcidos. 
	 */
	public void distorcerCores(BufferedImage imagem, float distorcaoR, float distorcaoG, float distorcaoB){		
		OMP.setNumThreads(3);
		
		int imageWidth = imagem.getWidth();
		int imageHeight = imagem.getHeight();
		
		int x = 0, y = 0;
		int rgb = 0;
		int r = 0, g = 0, b = 0;
		
		//omp parallel private(x,y,rgb,r,g,b)
		{
			//omp sections
			{
				//omp section
				{
					for (x = 0; x < imageWidth; x++) {
						for (y = 0; y < imageHeight; y++) {
							rgb = imagem.getRGB(x, y);
							r = Colors.red(rgb);
							g = Colors.green(rgb);
							b = Colors.blue(rgb);
							
							//distorção do vermelho
							r = Math.min(255, Math.max(0, (int)(r * distorcaoR)));							
							rgb = new Color(r, g, b).getRGB();
							
							//omp critical
							{
								imagem.setRGB(x, y, rgb);
							}
						}
					}
				}
				
				//omp section
				{
					for (x = 0; x < imageWidth; x++) {
						for (y = 0; y < imageHeight; y++) {
							rgb = imagem.getRGB(x, y);
							r = Colors.red(rgb);
							g = Colors.green(rgb);
							b = Colors.blue(rgb);
							
							//distorção do verde
							g = Math.min(255, Math.max(0, (int)(g * distorcaoG)));							
							rgb = new Color(r, g, b).getRGB();
							
							//omp critical
							{
								imagem.setRGB(x, y, rgb);
							}
						}
					}
				}
				
				//omp section
				{
					for (x = 0; x < imageWidth; x++) {
						for (y = 0; y < imageHeight; y++) {
							rgb = imagem.getRGB(x, y);
							r = Colors.red(rgb);
							g = Colors.green(rgb);
							b = Colors.blue(rgb);
							
							//distorção do azul
							b = Math.min(255, Math.max(0, (int)(b * distorcaoB)));							
							rgb = new Color(r, g, b).getRGB();
							
							//omp critical
							{
								imagem.setRGB(x, y, rgb);
							}
						}
					}
				}
			}
		}		
	}
	
	private int distorcerCor(int rgb, float distorcaoR, float distorcaoG, float distorcaoB){
		int r = Colors.red(rgb);
		int g = Colors.green(rgb);
		int b = Colors.blue(rgb);
		
		//distorção do vermelho
		r = Math.min(255, Math.max(0, (int)(r * distorcaoR)));
		
		//distorção do verde
		g = Math.min(255, Math.max(0, (int)(g * distorcaoG)));							
		
		//distorção do azul
		b = Math.min(255, Math.max(0, (int)(b * distorcaoB)));
		
		return new Color(r, g, b).getRGB();
	}
	
	public void setarCor(BufferedImage imagem, Color novaCor) {
		setarCor(imagem, novaCor, 0, 0, imagem.getWidth(), imagem.getHeight());
	}
	
	public void setarCor(BufferedImage imagem, Color novaCor, int x, int y, int w, int h){
		Graphics g = imagem.getGraphics();
		g.setColor(novaCor);
		g.fillRect(x, y, w, h);
	}
}
