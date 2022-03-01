package L1211AXX;

import java.util.Scanner;

/**
 * 
 * @author Dyego
 *
 */
public class Filinhas {
	
	static FilaCircular<String> filaNomes = null;
	static FilaCircular<Integer> filaInteiros = null;
	static FilaCircular<EMontadora> filaMontadoras = null;
	
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		boolean finalizarPrograma = false;
		
		Scanner scanner = new Scanner(System.in);
		while(!finalizarPrograma) {
			ImprimirMenu1();
			
			try {
				int opcao = scanner.nextInt();
				
				switch(opcao) {
				case 1:
				case 2:
				case 3:
					ExecutarMenu2(opcao);					
					break;				
				case 9:
					finalizarPrograma = true;
					break;
				}
			}
			catch(Exception ex) {
				continue;
			}
		}
	}
	
	private static void ImprimirMenu1() {
		System.out.println("1 – fila de nomes");
		System.out.println("2 – fila de inteiros");
		System.out.println("3 – fila de montadoras");
		System.out.println("9 – finaliza programa");
		System.out.println("Filas existentes: " + GetFilasExistentes());
	}

	private static void ExecutarMenu2(int pTipoFila) {
		ImprimirMenu2(pTipoFila);
		
		Scanner scanner = new Scanner(System.in);
		int opcao = scanner.nextInt();
		
		switch(opcao) {
		case 1:
			boolean filaCriada = CriarFila(pTipoFila);
			
			if(filaCriada)
				ExecutarMenu2(pTipoFila);
			
			break;
		case 2:
			if(!FilaCriada(pTipoFila)) {
				System.out.println("A fila deve ser criada primeiro.");
				ExecutarMenu2(pTipoFila);
				return;
			}
			
			DestuirFila(pTipoFila);
			ExecutarMenu2(pTipoFila);
			
			break;
		case 3:
			if(!FilaCriada(pTipoFila)) {
				System.out.println("A fila deve ser criada primeiro.");
				ExecutarMenu2(pTipoFila);
				return;
			}
			
			InserirNafila(pTipoFila);
			ExecutarMenu2(pTipoFila);
			
			break;
		case 4:
			if(!FilaCriada(pTipoFila)) {
				System.out.println("A fila deve ser criada primeiro.");
				ExecutarMenu2(pTipoFila);
				return;
			}
			
			ListarFila(pTipoFila);
			ExecutarMenu2(pTipoFila);
			
			break;
		case 5:
			if(!FilaCriada(pTipoFila)) {
				System.out.println("A fila deve ser criada primeiro.");
				ExecutarMenu2(pTipoFila);
				return;
			}
			
			Excluir(pTipoFila);
			ExecutarMenu2(pTipoFila);
			
			break;
		}
	}
	
	private static String GetFilasExistentes() {
		StringBuffer existentes = new StringBuffer();
		
		if(filaNomes != null)
			existentes.append("nomes, ");
		
		if(filaInteiros != null)
			existentes.append("inteiros, ");
		
		if(filaMontadoras != null)
			existentes.append("montadoras");
		
		return existentes.toString();
	}
	
	private static void ImprimirMenu2(int pTipo) {	
		DescreverFila(pTipo);
		System.out.println("1 – criar fila");
		System.out.println("2 – destruir fila");
		System.out.println("3 – inserir");
		System.out.println("4 – mostrar");
		System.out.println("5 – excluir");
		System.out.println("9 – retornar ao menu 1");
	}
	
	private static void DescreverFila(int pTipo) {
		System.out.println("Tipo da Fila: " + GetNomeFila(pTipo));
		System.out.println("Total de Elementos: " + GetTotalElementosFila(pTipo));
		System.out.println("Tamanho da Fila: " + GetTamanhoFila(pTipo));
	}
	
	private static String GetNomeFila(int pTipo) {
		switch(pTipo) {
		case 1: return "nomes";
		case 2: return "inteiros";
		case 3: return "montadoras";
		default: return "";
		}
	}
	
	private static String GetTotalElementosFila(int pTipo) {
		switch(pTipo) {
		case 1: return FilaCriada(pTipo) ? String.valueOf(filaNomes.getTotalElementos()) : "não criada";
		case 2: return FilaCriada(pTipo) ? String.valueOf(filaInteiros.getTotalElementos()) : "não criada";
		case 3: return FilaCriada(pTipo) ? String.valueOf(filaMontadoras.getTotalElementos()) : "não criada";
		default: return "";
		}
	}
	
	private static String GetTamanhoFila(int pTipo) {
		switch(pTipo) {
		case 1: return FilaCriada(pTipo) ? String.valueOf(filaNomes.getTamanho()) : "não criada";
		case 2: return FilaCriada(pTipo) ? String.valueOf(filaInteiros.getTamanho()) : "não criada";
		case 3: return FilaCriada(pTipo) ? String.valueOf(filaMontadoras.getTamanho()) : "não criada";
		default: return "";
		}
	}
	
	private static boolean FilaCriada(int pTipo) {
		switch(pTipo) {
		case 1: return filaNomes != null;
		case 2: return filaInteiros != null;
		case 3: return filaMontadoras != null;
		default: return false;
		}
	}
	
	
	
	
	
	private static boolean CriarFila(int pTipo) {
		if(FilaCriada(pTipo)) {
			System.out.println("Fila já existe, não pode ser criada novamente.");
			return false;
		}
		
		System.out.println("Qual o tamanho da lista?");
		Scanner scanner = new Scanner(System.in);
		int tamanho = scanner.nextInt();
		
		switch(pTipo) {
		case 1: 
			filaNomes = new FilaCircular<String>(tamanho);
			break;
		case 2: 
			filaInteiros = new FilaCircular<Integer>(tamanho);
			break;
		case 3: 
			filaMontadoras = new FilaCircular<EMontadora>(tamanho);
			break;
		default:
			return false;
		}
		
		return true;
	}
	
	private static void DestuirFila(int pTipoFila) {
		switch(pTipoFila) {
		case 1:
			if(filaNomes.getTotalElementos() == 0)			
				filaNomes = null;
			else {
				System.out.println("Existem elementos na fila.");
				ExecutarMenu2(pTipoFila);
			}
				
			break;
		case 2:
			if(filaInteiros.getTotalElementos() == 0)			
				filaInteiros = null;
			else {
				System.out.println("Existem elementos na fila.");
				ExecutarMenu2(pTipoFila);
			}
			
			break;
		case 3:
			if(filaMontadoras.getTotalElementos() == 0)			
				filaMontadoras = null;
			else {
				System.out.println("Existem elementos na fila.");
				ExecutarMenu2(pTipoFila);
			}
			
			break;
		}
	}
	
	private static void InserirNafila(int pTipoFila) {
		//ROQUE: A VERIFICAÇÃO DE FILA CHEIA NO MOMENTO DA INSERÇÃO É FEITO DENTRO DA UNIDADE Inserir(valor) PARA EVITAR A ATIVAÇÃO DE 
		//DUAS OUTRAS UNIDADES (GetTamanho() e GetTotalElementos()). UMA NOVA FUNÇÃO PODERIA SER CRIADA PARA REDUZIR A QUANTIDADE DE 
		//CHAMADAS: Vazia(). ISTO É CONSEQUÊNCIA DO ENCAPSULAMENTO DA CLASSE
		
		System.out.println("Qual o valor? ");
		Scanner scanner = new Scanner(System.in);
		
		switch(pTipoFila) {
		case 1:			
			String nome = scanner.next();
			filaNomes.inserir(nome);
			
			break;
		case 2:
			try {
				int inteiro = scanner.nextInt();
				filaInteiros.inserir(inteiro);
			}
			catch(Exception ex) {
				InserirNafila(pTipoFila);
			}
			
			break;
		case 3:
			EMontadora[] montadoras = EMontadora.values();
			
			//lista as montadoras
			for(EMontadora m : montadoras) {
				System.out.println(m.ordinal() + " - " + m.toString());
			}
			
			try
			{
				int opcaoMontadora = scanner.nextInt();
				EMontadora montadoraEscolhida = montadoras[opcaoMontadora];
				
				filaMontadoras.inserir(montadoraEscolhida);				
			}
			catch(Exception ex) {
				InserirNafila(pTipoFila); //tenta novamente
			}
			break;
		}
	}
	
	private static void ListarFila(int pTipo) {
		if(FilaCriada(pTipo)) {
			String descricao = "";
			
			switch(pTipo) {
			case 1: 
				descricao = filaNomes.toString();
				break;
			case 2: 
				descricao = filaInteiros.toString();
				break;
			case 3: 
				descricao = filaMontadoras.toString();
				break;
			}
			
			System.out.println(descricao);
		} else {
			System.out.println("A fila não existe.");
		}
	}
	
	private static void Excluir(int pTipo) {
		if(FilaCriada(pTipo)) {
			String descricao = "";
			
			switch(pTipo) {
			case 1: 
				filaNomes.remover();
				break;
			case 2: 
				filaInteiros.remover();
				break;
			case 3: 
				filaMontadoras.remover();
				break;
			}
			
			System.out.println(descricao);
		} else {
			System.out.println("A fila não existe.");
		}
	}
}
