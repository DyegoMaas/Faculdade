package editorImagens.core;

import java.awt.image.BufferedImage;

public class EditorImagensFactory {
	
	public IEditorImagens getEditorImagens(boolean versaoJomp){
		EditorImagensX editorImagensX = new EditorImagensX(new EditorImagens(), new EditorImagens_jomp(), versaoJomp);
		return editorImagensX;
	}
	
	private class EditorImagensX implements IEditorImagens{
		private EditorImagens editor;
		private EditorImagens_jomp editorJomp;
		private boolean versaoJomp;

		public EditorImagensX(EditorImagens editor, EditorImagens_jomp editorJomp, boolean versaoJomp) {
			this.editor = editor;
			this.editorJomp = editorJomp;
			this.versaoJomp = versaoJomp;
		}
		
		@Override
		public void blur(BufferedImage imagem, int windowWidth, int windowHeight) {
			if(versaoJomp)
				editorJomp.blur(imagem, windowWidth, windowHeight);
			else
				editor.blur(imagem, windowWidth, windowHeight);
		}

		@Override
		public void media(BufferedImage imagem) {
			if(versaoJomp)
				editorJomp.media(imagem);
			else
				editor.media(imagem);
		}

		@Override
		public void inverterCores(BufferedImage imagem) {
			if(versaoJomp)
				editorJomp.inverterCores(imagem);
			else
				editor.inverterCores(imagem);
		}

		@Override
		public void mediaInvertida(BufferedImage imagem) {
			if(versaoJomp)
				editorJomp.mediaInvertida(imagem);
			else
				editor.mediaInvertida(imagem);
		}

		@Override
		public void desaturarCorMedia(BufferedImage imagem) {
			if(versaoJomp)
				editorJomp.desaturarCorMedia(imagem);
			else
				editor.desaturarCorMedia(imagem);
		}
		
	}
}
