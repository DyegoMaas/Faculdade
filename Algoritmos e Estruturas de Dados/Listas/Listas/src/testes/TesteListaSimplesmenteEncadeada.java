package testes;

import listasSimplesmenteEncadeadas.Lista;

public class TesteListaSimplesmenteEncadeada implements ITestadorLista {
	
	@Override
	public void testar() {
		System.out.println("[TESTES: LISTA SIMPLESMENTE ENCADEADA]");
				
		testarComprimento();		
		testarImpressao();		
		testarConversaoString();		
		testarUltimo();		
		testarBusca();		
		testarRemocao();		
		testarLiberacao();		
		testarInsercaoOrdenada();		
		testarIgualdade();		
		testarImpressaoRecursiva();
		testarRemocaoRecursiva();
		
	}

	
	
	private void testarComprimento() {
		Lista lista = new Lista();
		
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		
		System.out.println();
		System.out.println("comprimento():");		
		int comprimentoEsperado = 3;
		String mensagem = String.format("comprimento esperado: %s, atual: %s", comprimentoEsperado, lista.comprimento());
		System.out.println(mensagem);
	}
	
	private void testarImpressao() {
		Lista lista = new Lista();
		
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		
		System.out.println();
		System.out.println("imprime():");
		System.out.println("lista: " + lista);
		lista.imprime();
		System.out.println();
	}
	
	private void testarConversaoString() {
		Lista lista = new Lista();
		
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		
		System.out.println("toString(): " + lista);
		System.out.println();
	}
	
	private void testarUltimo() {
		Lista lista = new Lista();
		
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		
		int ultimo = 1;
		
		System.out.println("ultimo():");
		String mensagem = String.format("Último esperado: %s, atual: %s", ultimo, lista.ultimo().getInfo());
		System.out.println(mensagem);
		System.out.println();
	}
	
	private void testarBusca() {
		Lista lista = new Lista();
		
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		
		int valorBuscado = 2;
		
		System.out.println("busca():");
		String mensagem = String.format("Valor buscado: %s, atual: %s", valorBuscado, lista.busca(valorBuscado).getInfo());
		System.out.println(mensagem);
		System.out.println();
	}
	
	private void testarRemocao() {
		Lista lista = new Lista();
		
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);		
		
		System.out.println("retira():");
		System.out.println("lista original: " + lista);
		System.out.println("valor retirado: " + 2);
		lista.retira(2);
		System.out.println("lista resultante: " + lista);
		System.out.println("valor retirado: " + 1);
		lista.retira(1);
		System.out.println("lista resultante: " + lista);
		System.out.println("valor retirado: " + 3);
		lista.retira(3);
		System.out.println("lista resultante: " + lista);
	}
	
	private void testarLiberacao() {
		Lista lista = new Lista();
		
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		
		System.out.println();
		System.out.println("libera:");
		System.out.print("lista original: " + lista);
		lista.libera();
		System.out.println("lista resultante: " + lista);
		System.out.println();
	}
	
	private void testarInsercaoOrdenada() {
		Lista lista = new Lista();
		
		lista.insereOrdenado(1);
		lista.insereOrdenado(2);
		lista.insereOrdenado(3);
		
		System.out.println("insereOrdenado():");
		System.out.println("valores inseridos: 1,2,3");
		System.out.println("lista resultante: " + lista);
		
		lista = new Lista();
		
		lista.insereOrdenado(2);
		lista.insereOrdenado(3);
		lista.insereOrdenado(1);
		
		System.out.println("valores inseridos: 2,3,1");
		System.out.println("lista resultante: " + lista);
		
		lista = new Lista();
		
		lista.insereOrdenado(3);
		lista.insereOrdenado(2);
		lista.insereOrdenado(1);
		
		System.out.println("valores inseridos: 3,2,1");
		System.out.println("lista resultante: " + lista);
	}
	
	private void testarIgualdade() {
		Lista lista = new Lista();
		
		lista.insereOrdenado(1);
		lista.insereOrdenado(2);
		lista.insereOrdenado(3);
		
		Lista lista2 = new Lista();
		
		lista2.insereOrdenado(1);
		lista2.insereOrdenado(2);
		lista2.insereOrdenado(3);
		
		System.out.println();
		System.out.println("igual():");
		System.out.print("lista1: " + lista + "; lista2: " + lista2);
		System.out.println("; iguais? " + lista.igual(lista2));
		
		lista = new Lista();
		
		lista.insereOrdenado(5);
		lista.insereOrdenado(6);
		lista.insereOrdenado(7);
		
		lista2 = new Lista();
		
		lista2.insereOrdenado(6);
		lista2.insereOrdenado(7);
		lista2.insereOrdenado(8);
		
		System.out.print("lista1: " + lista + "; lista2: " + lista2);
		System.out.println("; iguais? " + lista.igual(lista2));		
		System.out.println();
	}
	
	private void testarImpressaoRecursiva() {
		Lista lista = new Lista();
		
		lista.insereOrdenado(1);
		lista.insereOrdenado(2);
		lista.insereOrdenado(3);
		
		System.out.println();
		System.out.println("imprimeRecursivo():");
		System.out.println("lista: " + lista);
		lista.imprimeRecursivo();
		System.out.println();
	}

	private void testarRemocaoRecursiva() {
		Lista lista = new Lista();
				
		lista.insereOrdenado(1);
		lista.insereOrdenado(2);
		lista.insereOrdenado(3);
		
		System.out.println();
		System.out.println("retiraRecursivo():");
		System.out.print("lista: " + lista + "; item removido: 2; ");
		lista.retiraRecursivo(2);
		System.out.println("; lista resultante: " + lista);
		
		lista = new Lista();
		
		lista.insereOrdenado(1);
		lista.insereOrdenado(2);
		lista.insereOrdenado(3);
		
		System.out.print("lista: " + lista + "; item removido: 1; ");
		lista.retiraRecursivo(1);
		System.out.println("; lista resultante: " + lista);
		
		lista = new Lista();
		
		lista.insereOrdenado(1);
		lista.insereOrdenado(2);
		lista.insereOrdenado(3);
		
		System.out.print("lista: " + lista + "; item removido: 3; ");
		lista.retiraRecursivo(3);
		System.out.println("; lista resultante: " + lista);
		System.out.println();
	}
}
