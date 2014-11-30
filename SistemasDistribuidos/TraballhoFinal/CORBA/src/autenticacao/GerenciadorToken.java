package autenticacao;

import java.util.UUID;

public class GerenciadorToken {
	
	public static void validarToken(String token){
	  try{
	      UUID.fromString(token);
	  }
	  catch(IllegalArgumentException e) {
		  System.out.println("Token inv�lido : " + e) ;
	        e.printStackTrace(System.out);
	  }
	}
  
  	public static String gerarToken(){
		return java.util.UUID.randomUUID().toString();
	}
}
