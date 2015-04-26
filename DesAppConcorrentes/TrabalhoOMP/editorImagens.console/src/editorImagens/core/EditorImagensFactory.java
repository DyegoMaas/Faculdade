package editorImagens.core;

import java.awt.Color;
import java.awt.image.BufferedImage;

import editorImagens.core.editor.EditorImagens_jomp;
import editorImagens.core.editor.EditorImagens_normal;
import editorImagens.core.editor.IEditorImagens;
import editorImagens.core.utils.ImageUtil;

public class EditorImagensFactory {
	
	public IFachadaEdicaoImagens getEditorImagens(boolean versaoJomp){
		IFachadaEdicaoImagens editorImagensX = new EditorImagensX(new EditorImagens_normal(), new EditorImagens_jomp(), versaoJomp);
		return editorImagensX;
	}
	
	private class EditorImagensX implements IFachadaEdicaoImagens {
		private IEditorImagens editorNormal;
		private IEditorImagens editorJomp;
		private boolean versaoJomp;

		public EditorImagensX(EditorImagens_normal editor, EditorImagens_jomp editorJomp, boolean versaoJomp) {
			this.editorNormal = editor;
			this.editorJomp = editorJomp;
			
			this.versaoJomp = versaoJomp;
		}
		
		/* (non-Javadoc)
		 * @see editorImagens.core.IFachadaEdicaoImagens#blur(java.awt.image.BufferedImage, int, int)
		 */
		@Override
		public void blur(BufferedImage imagem, int windowWidth, int windowHeight) {			
			editor(versaoJomp).blur(imagem, windowWidth, windowHeight);
		}

		/* (non-Javadoc)
		 * @see editorImagens.core.IFachadaEdicaoImagens#calcularCorMedia(java.awt.image.BufferedImage)
		 */
		@Override
		public Color calcularCorMedia(BufferedImage imagem) {			
			return editor(versaoJomp).calcularCorMedia(imagem);
		}

		/* (non-Javadoc)
		 * @see editorImagens.core.IFachadaEdicaoImagens#inverterCores(java.awt.image.BufferedImage)
		 */
		@Override
		public void inverterCores(BufferedImage imagem) {
			editor(versaoJomp).inverterCores(imagem);
		}

		/* (non-Javadoc)
		 * @see editorImagens.core.IFachadaEdicaoImagens#mediaInvertida(java.awt.image.BufferedImage)
		 */
		@Override
		public Color mediaInvertida(BufferedImage imagem) {
			Color corMedia = editor(versaoJomp).calcularCorMedia(imagem);
			return ImageUtil.inverterCor(corMedia);
		}

		/* (non-Javadoc)
		 * @see editorImagens.core.IFachadaEdicaoImagens#desaturarCorMedia(java.awt.image.BufferedImage)
		 */
		@Override
		public void xorBlendingComCorMedia(BufferedImage imagem) {
			editor(versaoJomp).xorBlendingComCorMedia(imagem);
		}

		@Override
		public void mosaico(BufferedImage imagem, int tamanhoCelulas) {
			editor(versaoJomp).mosaico(imagem, tamanhoCelulas);			
		}

		@Override
		public void dessaturar(BufferedImage imagem, float percentual) {
			editor(versaoJomp).dessaturar(imagem, percentual);	
		}		

		@Override
		public void distorcerCores(BufferedImage imagem, float distorcaoR, float distorcaoG, float distorcaoB) {
			editor(versaoJomp).distorcerCores(imagem, distorcaoR, distorcaoG, distorcaoB);			
		}

		@Override
		public void outraDistorcao(BufferedImage imagem) {
			editor(versaoJomp).outraDistorcao(imagem);
		}
		
		private IEditorImagens editor(boolean versaoJomp){
			return versaoJomp ? editorJomp : editorNormal;
		}
	}
}
