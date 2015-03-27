package grades;

import grades.entradas.Materia;

import java.util.ArrayList;
import java.util.List;

import busca.AEstrela;
import busca.BuscaLargura;
import busca.Estado;
import busca.MostraStatusConsole;
import busca.Nodo;

public class Main {
	
	private final int NUMERO_MATERIAS_ALOCADAS = 5;
	
	private class Grade implements Estado {
		private String[] segunda = new String[2];
		private String[] terca = new String[2];
		private String[] quarta = new String[2];
		private String[] quinta = new String[2];
		private String[] sexta = new String[2];
		private int materiasAlocadas;
		
		public Grade() {
		}
		
		@Override
		public Grade clone(){
			Grade grade = new Grade();
			grade.materiasAlocadas = materiasAlocadas;
			grade.segunda = segunda.clone();
			grade.terca = terca.clone();
			grade.quarta = quarta.clone();
			grade.quinta = quinta.clone();
			grade.sexta = sexta.clone();
			
			return grade;
		}
		
		private void adicionarMateria(Materia materia) {
			String[] dia1DaSemana = obterDia(materia.dia1);
			String[] dia2DaSemana = obterDia(materia.dia2);
						
			dia1DaSemana[materia.horarioDia1 - 1] = materia.id;
			dia2DaSemana[materia.horarioDia2 - 1] = materia.id;
			materiasAlocadas++;
		}
		
		private String[] obterDia(int dia) {
			switch(dia){
			case 1:
				return segunda;
			case 2:
				return terca;
			case 3:
				return quarta;
			case 4:
				return quinta;
			default:
				return sexta;			
			}
		}
		
		@Override
		public int custo() {
			return 1;
		}

		@Override
		public boolean ehMeta() {
			return materiasAlocadas == NUMERO_MATERIAS_ALOCADAS;
		}		

		@Override
		public List<Estado> sucessores() {			
			List<Estado> sucessores = new ArrayList<Estado>();
			
			for(Materia materia : curso) {
				boolean horario1Disponivel = estahDisponivel(materia.dia1, materia.horarioDia1);
				boolean horario2Disponivel = estahDisponivel(materia.dia2, materia.horarioDia2);
				
				if(!horario1Disponivel || !horario2Disponivel)
					continue;
				
				if(materia.preRequisito != null && !materiasConcluidas.contains(materia.preRequisito)) {
					continue;
				}
				
				Grade grade = this.clone();
				grade.adicionarMateria(materia);
				
				sucessores.add(grade);
			}
			
			return sucessores;
		}

		private boolean estahDisponivel(int dia, int horario) {
			String[] diaDaSemana = obterDia(dia);
			return diaDaSemana[horario - 1] == null;
		}
		
	    /**
	     * verifica se um estado e igual a outro
	     */
	    public boolean equals(Object o) {
	    	if (o instanceof Grade) {
	    		Grade e = (Grade)o;
	            for (int dia = 1; dia <= 5; dia++) {
					String[] diaSemanaOutro = e.obterDia(dia);
					String[] diaSemana = this.obterDia(dia);
					if(diaSemanaOutro[0] != diaSemana[0]) 
						return false;
					
					if(diaSemanaOutro[1] != diaSemana[1]) 
						return false;
				}
	            return true;
	        }
	        return false;
	    }
	    
	    public int hashCode() {
	        return toString().hashCode();
	    }
	    
	    public String toString() {
	    	StringBuffer stringBuffer = new StringBuffer();
	    	for (int dia = 1; dia <= 5; dia++) {
				String[] diaSemana = this.obterDia(dia);
				for (int horario = 0; horario < diaSemana.length; horario++) {
					stringBuffer
						.append(diaSemana[horario])
						.append(" no dia ").append(dia)
						.append(" no horário ").append(horario + 1)
						.append(" / ");	
				}
				stringBuffer.append('\r');
			}
	            
	        return stringBuffer.toString();
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
		new MateriaBuilder(1, "12", "Computação Digital").lecionadaNa(TERCA, 1).eNa(TERCA, 2).construir(),
		new MateriaBuilder(1, "13", "Programação de Computadores").lecionadaNa(QUARTA, 1).eNa(QUARTA, 2).construir(),
		new MateriaBuilder(1, "14", "Universidade, Ciência e Pesquisa").lecionadaNa(QUINTA, 1).eNa(QUINTA, 2).construir(),
		new MateriaBuilder(1, "15", "Fundamentos Matemáticos para Computação").lecionadaNa(SEXTA, 1).eNa(SEXTA, 2).construir(),

		new MateriaBuilder(2, "21", "Arquitetura de Computadores").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(2, "22", "Lógica para Computação").lecionadaNa(TERCA, 1).eNa(TERCA, 2).construir(),
		new MateriaBuilder(2, "23", "Linguagem Científica").lecionadaNa(QUARTA, 1).eNa(QUARTA, 2).construir(),
		new MateriaBuilder(2, "24", "Álgebra Linear para Computação").lecionadaNa(QUINTA, 1).eNa(QUINTA, 2).construir(),
		new MateriaBuilder(2, "25", "Programação Orientada a Objetos I").lecionadaNa(SEXTA, 1).eNa(SEXTA, 2).construir(),

		new MateriaBuilder(3, "31", "Sistemas Operacionais").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(3, "32", "Algoritmos e Estruturas de Dados").lecionadaNa(TERCA, 1).eNa(TERCA, 2).construir(),
		new MateriaBuilder(3, "33", "Teoria da Computação").lecionadaNa(QUARTA, 1).eNa(QUARTA, 2).construir(),
		new MateriaBuilder(3, "34", "Estatística Aplicada à Informática").lecionadaNa(QUINTA, 1).eNa(QUINTA, 2).construir(),
		new MateriaBuilder(3, "35", "Programação Orientada a Objetos II").lecionadaNa(SEXTA, 1).eNa(SEXTA, 2).construir(),

		new MateriaBuilder(4, "41", "Linguagens de Programação").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(4, "42", "Teoria dos Grafos").lecionadaNa(TERCA, 1).eNa(TERCA, 2).construir(),
		new MateriaBuilder(4, "43", "Protocolos de Comunicação de Dados").lecionadaNa(QUARTA, 1).eNa(QUARTA, 2).construir(),
		new MateriaBuilder(4, "44", "Métodos Quantitativos").lecionadaNa(QUINTA, 1).eNa(QUINTA, 2).construir(),
		new MateriaBuilder(4, "45", "Desafios Sociais Contemporâneos").lecionadaNa(SEXTA, 1).eNa(SEXTA, 2).construir(),

		new MateriaBuilder(5, "51", "Redes de Computadores").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(5, "52", "Compiladores").lecionadaNa(TERCA, 1).eNa(TERCA, 2).construir(),
		new MateriaBuilder(5, "53", "Desenvolvimento de Aplicações Concorrentes e Distribuídas").lecionadaNa(QUARTA, 1).eNa(QUARTA, 2).construir(),
		new MateriaBuilder(5, "54", "Engenharia de Software").lecionadaNa(QUINTA, 1).eNa(QUINTA, 2).construir(),
		new MateriaBuilder(5, "55", "Banco de Dados I - SEMIPRESENCIAL-100%").lecionadaNa(SEXTA, 1).eNa(SEXTA, 2).construir(),

		new MateriaBuilder(6, "61", "Sistemas Distribuídos").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(6, "62", "Comportamento Organizacional").lecionadaNa(TERCA, 1).eNa(TERCA, 2).construir(),
		new MateriaBuilder(6, "63", "Banco de Dados II").lecionadaNa(QUARTA, 1).eNa(QUARTA, 2).construir(),
		new MateriaBuilder(6, "64", "Processo de Software I").lecionadaNa(QUINTA, 1).eNa(QUINTA, 2).construir(),
		new MateriaBuilder(6, "65", "Disciplina Optativa I - PRÁTICA EM REDES / GENEXUS").lecionadaNa(SEXTA, 1).eNa(SEXTA, 2).construir(),
			
		new MateriaBuilder(7, "71", "Processo de Software II").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(7, "72", "Disciplina Optativa II - ROBÓTICA").lecionadaNa(TERCA, 1).eNa(TERCA, 2).construir(),
		new MateriaBuilder(7, "73", "Computação Gráfica").lecionadaNa(QUINTA, 2).eNa(SEXTA, 1).construir(),
		new MateriaBuilder(7, "74", "Inteligência Artificial").lecionadaNa(QUARTA, 2).eNa(SEXTA, 2).construir(),
		new MateriaBuilder(7, "75", "Desenvolvimento Web").lecionadaNa(QUARTA, 1).eNa(QUINTA, 1).construir(),

		new MateriaBuilder(8, "81", "Sistemas Multimídia").lecionadaNa(SEGUNDA, 1).eNa(SEGUNDA, 2).construir(),
		new MateriaBuilder(8, "82", "Legislação em Informática").lecionadaNa(TERCA, 1).eNa(TERCA, 2).construir(),
		new MateriaBuilder(8, "83", "Banco de Dados II").lecionadaNa(QUARTA, 1).eNa(QUARTA, 2).construir(),
		new MateriaBuilder(8, "84", "Empreendedor em Informática").lecionadaNa(QUINTA, 1).eNa(QUINTA, 2).construir(),
		new MateriaBuilder(8, "85", "Disciplina Optativa III - PROGRAMAÇÃO MICROCON").lecionadaNa(SEXTA, 1).eNa(SEXTA, 2).construir()
	};
	
	private ArrayList<String> materiasConcluidas = new ArrayList<String>();
	
	private void executar() {	
		//monta matérias concluídas
		for(Materia materia : curso){
			if(materia.completada)
				materiasConcluidas.add(materia.id);
		}
		
		Grade gradeVazia = new Grade();
		/* if (! gradeVazia.temSolucao()) {
	            System.out.println(gradeVazia+"nao tem solucao!");
	            return;
	        }*/
	        
	        //Nodo s = new AEstrela().busca(e8);
	        Nodo s = new BuscaLargura(new MostraStatusConsole()).busca(gradeVazia);
	        //Nodo s = new AEstrela(new MostraStatusConsole()).busca(gradeVazia);
	        //Nodo s = new BuscaIterativo(new MostraStatusConsole()).busca(e8);
	        //Nodo s = new BuscaProfundidade(25,new MostraStatusConsole()).busca(e8);
	        //Nodo s = new BuscaBidirecional(new MostraStatusConsole()).busca(e8, getEstadoMeta());
	        if (s != null) {
	            System.out.println("solucao ("+s.getProfundidade()+")= "+s.montaCaminho());
	        }        
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
			materia.id = id;
			materia.semestre = semestre;
			materia.nome = nome;
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
