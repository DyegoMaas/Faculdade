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
	
}
