package listas;

public class Hashtable {

	private Aluno[] alunos = null;
	
	public Hashtable(int tam){
		alunos = new Aluno[tam];
	}
	
	private int hash(int matricula){
		return (matricula + 1) % alunos.length;
	}
	
	public void set(int matricula, String nome, float mediaGeral){		
		int h = hash(matricula);
		
		Aluno aluno = alunos[h];
		
		// procura aluno
		while(aluno != null){
			if(aluno.getMatricula() == matricula)
				break;
			aluno = aluno.getProx();
		}
		
		//não encontrou o aluno
		if(aluno == null){
			aluno = new Aluno();
			aluno.setMatricula(matricula);
			aluno.setProx(alunos[h]);
			alunos[h] = aluno;
		}
		
		//atribui/modifica a informação
		aluno.setNome(nome);
		aluno.setMediaGeral(mediaGeral);
	}
	
	public Aluno get(int matricula){
		int i = hash(matricula);
		
		if(alunos[i] == null)
			return null;
		
		Aluno al = alunos[i];
		do
		{
			if(al.getMatricula() == matricula)
				return al;
			al = al.getProx();
		}
		while(al != null);
		
		return null;
	}
	
	public void remove(int matricula){		
		int h = hash(matricula);
		
		Aluno alunoAnt = null; 
		Aluno aluno = alunos[h];
		
		// procura aluno
		while(aluno != null && aluno.getMatricula() != matricula){
			alunoAnt = aluno;
			aluno = aluno.getProx();			
		}
		
		//não encontrou o aluno
		if(aluno == null){
			return;
		}
		
		//remove
		if(alunoAnt == null)
			alunos[h] = aluno.getProx();
		else
			alunoAnt.setProx(aluno.getProx());
	}
	
	@Override
	public String toString(){
		StringBuilder s = new StringBuilder();
		
		for(Aluno lista : this.alunos){
			s.append("[");
			
			Aluno al = lista;
			while(al != null){
				s.append(al.toString());
				if(al.getProx() != null)
					s.append(",");
				al = al.getProx();
			}			
			
			s.append("]");
		}
		
		return s.toString();
	}
	
}
