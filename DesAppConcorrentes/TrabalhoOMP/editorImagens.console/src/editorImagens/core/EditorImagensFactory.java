package editorImagens.core;

public class EditorImagensFactory {
	
	public static IEditorImagens getEditorImagens(boolean versaoJomp){
		if(versaoJomp)
			return new EditorImagens_jomp();
	
		return new EditorImagens();
	}
}
