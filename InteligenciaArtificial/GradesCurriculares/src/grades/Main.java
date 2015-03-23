package grades;

import grades.entradas.Materia;
import java.util.List;
import busca.Estado;

public class Main {

	private class Grade implements Estado {

		@Override
		public int custo() {
			return 1;
		}

		@Override
		public boolean ehMeta() {
			// TODO Auto-generated method stub
			return false;
		}

		@Override
		public List<Estado> sucessores() {
			// TODO Auto-generated method stub
			return null;
		}
		
	}
	
	public static void main(String[] args) {		
		Main main = new Main();
		main.executar();
		
		
		/**
		 * Agentes:
		 * 
		 * 
		 * Ações possíveis:
		 *	*Alocar materia em um horario de um dia da semana
		 *	*Desalocar matéria de um horário
		 * 
		 * Estado inicial:
		 * json entrada
		 * json saida
		 * 
		 * Objetivo:
		 * *Preencher o máximo de matérias que conseguir usando os dois horários de cada matéria (tenta 5, depois 4... no mínimo 1)
		 * 
		 * restrição:
		 * *tem que encaixar a matéria nos dois dias/horarios
		 */
		
	}
	private final int SEGUNDA = 1;
	private final int TERCA = 2; 
	private final int QUARTA = 3; 
	private final int QUINTA = 4; 
	private final int SEXTA = 5;
	
	private Materia[] curso = new Materia[] {
		//TODO marcar alguns como concluídos
		new MateriaBuilder(1, "11", "Introdução à Computação").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(1, "12", "Computação Digital").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(1, "13", "Programação de Computadores").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(1, "14", "Universidade, Ciência e Pesquisa").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(1, "15", "Fundamentos Matemáticos para Computação").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),

		new MateriaBuilder(2, "21", "Arquitetura de Computadores").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(2, "22", "Lógica para Computação").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(2, "23", "Linguagem Científica").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(2, "24", "Álgebra Linear para Computação").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(2, "25", "Programação Orientada a Objetos I").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),

		new MateriaBuilder(3, "31", "Sistemas Operacionais").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(3, "32", "Algoritmos e Estruturas de Dados").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(3, "33", "Teoria da Computação").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(3, "34", "Estatística Aplicada à Informática").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(3, "35", "Programação Orientada a Objetos II").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),

		new MateriaBuilder(4, "41", "Linguagens de Programação").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(4, "42", "Teoria dos Grafos").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(4, "43", "Protocolos de Comunicação de Dados").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(4, "44", "Métodos Quantitativos").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(4, "45", "Desafios Sociais Contemporâneos").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),

		new MateriaBuilder(5, "51", "Redes de Computadores").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(5, "52", "Compiladores").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(5, "53", "Desenvolvimento de Aplicações Concorrentes e Distribuídas").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(5, "54", "Engenharia de Software").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(5, "55", "Banco de Dados I - SEMIPRESENCIAL-100%").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),

		new MateriaBuilder(6, "61", "Sistemas Distribuídos").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(6, "62", "Comportamento Organizacional").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(6, "63", "Banco de Dados II").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(6, "64", "Processo de Software I").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(6, "65", "Disciplina Optativa I - PRÁTICA EM REDES / GENEXUS").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
			
		new MateriaBuilder(7, "71", "Processo de Software II").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(7, "72", "Disciplina Optativa II - ROBÓTICA").lecionadaNa(TERCA, 1).eNa(TERCA, 2).construir(),
		new MateriaBuilder(7, "73", "Computação Gráfica").lecionadaNa(QUINTA, 2).eNa(SEXTA, 1).construir(),
		new MateriaBuilder(7, "74", "Inteligência Artificial").lecionadaNa(QUARTA, 2).eNa(SEXTA, 2).construir(),
		new MateriaBuilder(7, "75", "Desenvolvimento Web").lecionadaNa(QUARTA, 1).eNa(QUINTA, 1).construir(),

		new MateriaBuilder(8, "81", "Sistemas Multimídia").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(8, "82", "Legislação em Informática").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(8, "83", "Banco de Dados II").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(8, "84", "Empreendedor em Informática").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(8, "85", "Disciplina Optativa III - PROGRAMAÇÃO MICROCON").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir()
	};
	
	private void executar() {		
		
		//TODO processar o algoritmo
	}
	
	private class MateriaBuilder {
		private int semestre = 1;
		private int dia1 = 1, dia2 = 1;
		private int horarioDia1 = 1, horarioDia2 = 2;
		private String id, nome;
		private boolean completada;
		private String preRequisito;
		
		public MateriaBuilder(int semestre, String id, String nome){
			this.semestre = semestre;
			this.id = id;
			this.nome = nome;
		}
		
		public MateriaBuilder completada() {
			this.completada = true;
			return this;
		}
		
		public MateriaBuilder lecionadaNa(int dia, int horario) {
			this.dia1 = dia;
			this.horarioDia1 = horario;
			return this;
		}
		
		public MateriaBuilder eNa(int dia, int horario) {
			this.dia2 = dia;
			this.horarioDia2 = horario;
			return this;
		}
		
		public MateriaBuilder comPreRequisito(String preRequisito) {
			this.preRequisito = preRequisito;
			return this;
		}
		
		public Materia construir() {
			Materia materia = new Materia();
			materia.semestre = semestre;
			materia.dia1 = dia1;
			materia.dia2 = dia2;
			materia.horarioDia1 = horarioDia1;
			materia.horarioDia2 = horarioDia2;
			materia.completada = completada;
			materia.preRequisito = preRequisito;
			
			return materia;
		}
	}
	
	
}
