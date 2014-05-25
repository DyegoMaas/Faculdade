package editorImagens.core;

import java.awt.Color;
import java.awt.image.BufferedImage;

public class EditorImagensFactory {
	
	public IFachadaEdicaoImagens getEditorImagens(boolean versaoJomp){
		IFachadaEdicaoImagens editorImagensX = new EditorImagensX(new EditorImagens(), new EditorImagens_jomp(), versaoJomp);
		return editorImagensX;
	}
	
	private class EditorImagensX implements IFachadaEdicaoImagens {
		private IEditorImagens editor;
		private IEditorImagens editorJomp;
		private boolean versaoJomp;

		public EditorImagensX(EditorImagens editor, EditorImagens_jomp editorJomp, boolean versaoJomp) {
			this.editor = editor;
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
			Color corMedia = editor(versaoJomp).calcularCorMedia(imagem);
			return corMedia;
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
			Color corMediaInvertida = editor(versaoJomp).inverterCor(corMedia);
			return corMediaInvertida;
		}

		/* (non-Javadoc)
		 * @see editorImagens.core.IFachadaEdicaoImagens#desaturarCorMedia(java.awt.image.BufferedImage)
		 */
		@Override
		public void desaturarCorMedia(BufferedImage imagem) {
			editor(versaoJomp).desaturarCorMedia(imagem);
		}

		private IEditorImagens editor(boolean versaoJomp){
			return versaoJomp ? editor : editorJomp;
		}
	}
}
