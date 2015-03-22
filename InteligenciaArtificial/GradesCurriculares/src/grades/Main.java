package grades;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;

import com.google.gson.Gson;
import com.google.gson.JsonIOException;
import com.google.gson.JsonSyntaxException;

import grades.entradas.GradeCurricular;


public class Main {

	public static void main(String[] args) throws JsonSyntaxException, JsonIOException, FileNotFoundException {
		Gson gson = new Gson();
		GradeCurricular grade = gson.fromJson(new FileReader(new File("input.json")), GradeCurricular.class);
		
		System.out.println(grade);
		//System.out.println(t2);
		
		
		
		
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
	
}
