package editorImagens.core.utils;

public class Colors {
	public static int red(int rgb){
		return (rgb >> 16) & 0xFF;
	}
	
	public static int green(int rgb){
		return (rgb >> 8) & 0xFF;
	}
	
	public static int blue(int rgb){
		return (rgb & 0xFF);
	}
}
