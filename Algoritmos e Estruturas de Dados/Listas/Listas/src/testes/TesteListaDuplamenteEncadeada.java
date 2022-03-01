package testes;

import listasDuplasEncadeadas.ListaDupla;
import listasDuplasEncadeadas.NoListaDupla;

public class TesteListaDuplamenteEncadeada implements ITestadorLista{

	@Override
	public void testar() {
		System.out.println("[TESTES: LISTA DUPLAMENTE ENCADEADA]");		
		
		testarImpressao();
		testarConversaoString();
		testarVazia();		
		testarBusca();
		testarBuscaPorIndice();
		testarRemocao();		
		testarLiberacao();		
		testarIgualdade();		
		testarMerge();		
		testarInsercaoOrdenada();
		testarSeparacao();
	}	
	
	private void testarImpressao() {
		ListaDupla lista = new ListaDupla();
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		lista.insere(4);
		lista.insere(5);
		
		lista.imprime();
	}
	
	private void testarConversaoString() {
		ListaDupla lista = new ListaDupla();
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		lista.insere(4);
		lista.insere(5);
		
		System.out.println();
		System.out.println("toString(): " + lista);
	}
	
	private void testarVazia() {
		ListaDupla lista = new ListaDupla();
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		lista.insere(4);
		lista.insere(5);
		
		System.out.println();
		System.out.println("vazia():");
		System.out.println("conteúdo: " + lista + "; vazia: " + lista.vazia());
		
		ListaDupla listaVazia = new ListaDupla();
		System.out.println("conteúdo: " + listaVazia + "; vazia: " + listaVazia.vazia());
		
		
		ListaDupla listaRemocao = new ListaDupla();
		listaRemocao.insere(1);
		listaRemocao.insere(2);
		listaRemocao.insere(3);
		
		System.out.print("conteúdo antes: " + listaRemocao);
		listaRemocao.retira(1);
		System.out.print("; após remover 1: " + listaRemocao);
		listaRemocao.retira(2);
		System.out.print("; após remover 2: " + listaRemocao);
		listaRemocao.retira(3);
		System.out.print("; após remover 3: " + listaRemocao);
		System.out.println("; vazia: " + listaRemocao.vazia());
	}
	
	private void testarBusca() {
		ListaDupla lista = new ListaDupla();
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		lista.insere(4);
		lista.insere(5);
		
		System.out.println();
		System.out.println("busca():");
		
		NoListaDupla no = lista.busca(3);
		System.out.println("valor buscado: 3; valor recuperado: " + no.getInfo());
		no = lista.busca(1);
		System.out.println("valor buscado: 1; valor recuperado: " + no.getInfo());
		no = lista.busca(6);
		boolean valorNaoEncontrado = no == null;
		System.out.println("valor buscado: 6; não encontrado: " + valorNaoEncontrado);
	}
	
	private void testarBuscaPorIndice() {
		ListaDupla lista = new ListaDupla();
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		lista.insere(4);
		lista.insere(5);
		
		System.out.println();
		System.out.println("buscaIndice():");
		
		NoListaDupla no = lista.buscaIndice(3);
		System.out.println("índice buscado: 3; valor esperado: 2; valor recuperado: " + no.getInfo());
		no = lista.buscaIndice(1);
		System.out.println("índice buscado: 1; valor esperado: 4; valor recuperado: " + no.getInfo());
		no = lista.buscaIndice(0);
		System.out.println("índice buscado: 0; valor esperado: 5; valor recuperado: " + no.getInfo());
		no = lista.buscaIndice(5);
		boolean valorNaoEncontrado = no == null;
		System.out.println("índice buscado: 5; não encontrado: " + valorNaoEncontrado);
	}
	
	private void testarRemocao() {
		ListaDupla lista = new ListaDupla();
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		lista.insere(4);
		lista.insere(5);
		
		System.out.println();
		System.out.println("retira(): ");
		System.out.println("valores removidos: 1, 3 e 5");
		System.out.println("valores antes: " + lista);
		lista.retira(1);
		lista.retira(3);
		lista.retira(5);
		System.out.println("valores depois: " + lista);
	}
	
	private void testarLiberacao() {
		ListaDupla lista = new ListaDupla();
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		lista.insere(4);
		lista.insere(5);
		
		System.out.println();
		System.out.println("libera():");
		System.out.print("conteúdo antes: " + lista);
		lista.libera();
		System.out.println("; depois: " + lista);
	}
	
	private void testarIgualdade() {
		ListaDupla lista = new ListaDupla();
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		lista.insere(4);
		lista.insere(5);
		
		ListaDupla lista2 = new ListaDupla();
		lista2.insere(1);
		lista2.insere(2);
		lista2.insere(3);
		lista2.insere(4);
		lista2.insere(5);
		
		System.out.println();
		System.out.println("lista1: " + lista + "; lista2: " + lista2 + "; são iguais? " + lista.igual(lista2));
		lista2 = new ListaDupla();
		System.out.println("lista1: " + lista + "; lista2: " + lista2 + "; são iguais? " + lista.igual(lista2));
		lista = new ListaDupla();
		System.out.println("lista1: " + lista + "; lista2: " + lista2 + "; são iguais? " + lista.igual(lista2));
	}
	
	private void testarMerge() {
		ListaDupla lista = new ListaDupla();
		lista.insere(1);
		lista.insere(2);
		lista.insere(3);
		
		ListaDupla lista2 = new ListaDupla();
		lista2.insere(4);
		lista2.insere(5);
		
		ListaDupla lista3 = new ListaDupla();
		
		System.out.println();
		System.out.println("merge():");
		System.out.println(lista + " + " + lista2 + " = " + lista.merge(lista2));
		System.out.println(lista + " + " + lista3 + " = " + lista.merge(lista3));
	}
	
	private void testarInsercaoOrdenada() {
		ListaDupla lista = new ListaDupla();
		lista.insereOrdenado(5);
		lista.insereOrdenado(3);
		lista.insereOrdenado(2);
		lista.insereOrdenado(1);
		lista.insereOrdenado(4);
		
		System.out.println();
		System.out.println("insereOrdenado():");
		System.out.println("valores inseridos: 5,3,2,1,4; lista resultante: " + lista);
		
		lista.libera();
		lista.insereOrdenado(1);
		lista.insereOrdenado(2);
		lista.insereOrdenado(3);
		lista.insereOrdenado(4);
		lista.insereOrdenado(5);
		
		System.out.println("valores inseridos: 1,2,3,4,5; lista resultante: " + lista);
	}
	
	private void testarSeparacao() {
		ListaDupla lista = new ListaDupla();
		lista.insereOrdenado(1);
		lista.insereOrdenado(2);
		lista.insereOrdenado(3);
		lista.insereOrdenado(4);
		lista.insereOrdenado(5);
		
		System.out.println();
		System.out.println("separa():");
		System.out.println("lista antes: " + lista);	
		
		ListaDupla parteDois = lista.separa(3);
		System.out.println("lista depois da separação por 3: lista original -> " + lista + "; nova -> " + parteDois);
	}
	
}
