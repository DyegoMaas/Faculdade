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
		 * A��es poss�veis:
		 *	*Alocar materia em um horario de um dia da semana
		 *	*Desalocar mat�ria de um hor�rio
		 * 
		 * Estado inicial:
		 * json entrada
		 * json saida
		 * 
		 * Objetivo:
		 * *Preencher o m�ximo de mat�rias que conseguir usando os dois hor�rios de cada mat�ria (tenta 5, depois 4... no m�nimo 1)
		 * 
		 * restri��o:
		 * *tem que encaixar a mat�ria nos dois dias/horarios
		 */
		
	}
	private final int SEGUNDA = 1;
	private final int TERCA = 2; 
	private final int QUARTA = 3; 
	private final int QUINTA = 4; 
	private final int SEXTA = 5;
	
	private Materia[] curso = new Materia[] {
		//TODO marcar alguns como conclu�dos
		new MateriaBuilder(1, "11", "Introdu��o � Computa��o").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(1, "12", "Computa��o Digital").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(1, "13", "Programa��o de Computadores").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(1, "14", "Universidade, Ci�ncia e Pesquisa").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(1, "15", "Fundamentos Matem�ticos para Computa��o").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),

		new MateriaBuilder(2, "21", "Arquitetura de Computadores").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(2, "22", "L�gica para Computa��o").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(2, "23", "Linguagem Cient�fica").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(2, "24", "�lgebra Linear para Computa��o").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(2, "25", "Programa��o Orientada a Objetos I").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),

		new MateriaBuilder(3, "31", "Sistemas Operacionais").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(3, "32", "Algoritmos e Estruturas de Dados").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(3, "33", "Teoria da Computa��o").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(3, "34", "Estat�stica Aplicada � Inform�tica").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(3, "35", "Programa��o Orientada a Objetos II").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),

		new MateriaBuilder(4, "41", "Linguagens de Programa��o").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(4, "42", "Teoria dos Grafos").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(4, "43", "Protocolos de Comunica��o de Dados").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(4, "44", "M�todos Quantitativos").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(4, "45", "Desafios Sociais Contempor�neos").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),

		new MateriaBuilder(5, "51", "Redes de Computadores").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(5, "52", "Compiladores").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(5, "53", "Desenvolvimento de Aplica��es Concorrentes e Distribu�das").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(5, "54", "Engenharia de Software").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(5, "55", "Banco de Dados I - SEMIPRESENCIAL-100%").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),

		new MateriaBuilder(6, "61", "Sistemas Distribu�dos").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(6, "62", "Comportamento Organizacional").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(6, "63", "Banco de Dados II").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(6, "64", "Processo de Software I").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(6, "65", "Disciplina Optativa I - PR�TICA EM REDES / GENEXUS").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
			
		new MateriaBuilder(7, "71", "Processo de Software II").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(7, "72", "Disciplina Optativa II�- ROB�TICA").lecionadaNa(TERCA, 1).eNa(TERCA, 2).construir(),
		new MateriaBuilder(7, "73", "Computa��o Gr�fica").lecionadaNa(QUINTA, 2).eNa(SEXTA, 1).construir(),
		new MateriaBuilder(7, "74", "Intelig�ncia Artificial").lecionadaNa(QUARTA, 2).eNa(SEXTA, 2).construir(),
		new MateriaBuilder(7, "75", "Desenvolvimento Web").lecionadaNa(QUARTA, 1).eNa(QUINTA, 1).construir(),

		new MateriaBuilder(8, "81", "Sistemas Multim�dia").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(8, "82", "Legisla��o em Inform�tica").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(8, "83", "Banco de Dados II").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(8, "84", "Empreendedor em Inform�tica").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(8, "85", "Disciplina Optativa III - PROGRAMA��O MICROCON").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir()
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
