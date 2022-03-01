using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace L1211B09
{
    /// <summary>
    /// Classe principal
    /// </summary>
    /// <author>Dyego Alekssander Maas</author>
    class Program
    {
        static FilaCircular<String> filaNomes = null;
        static FilaCircular<Int32> filaInteiros = null;
        static FilaCircular<EMontadora> filaMontadoras = null;

        static void Main(string[] args)
        {
            Console.WriteLine("L1211B09 - Dyego Alekssander Maas");
            Console.WriteLine();

            bool finalizarPrograma = false;
		
		    while(!finalizarPrograma) {
			    ImprimirMenu1();

                try
                {
                    int opcao;
                    if (!int.TryParse(Console.In.ReadLine(), out opcao))
                    {
                        continue;
                    }

                    switch (opcao)
                    {
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
                catch
                {
                    continue;
                }
		    }
        }

        private static void ImprimirMenu1() {
            Console.WriteLine();
            Console.WriteLine("O que você deseja fazer?");
		    Console.WriteLine("1 – fila de nomes");
		    Console.WriteLine("2 – fila de inteiros");
		    Console.WriteLine("3 – fila de montadoras");
		    Console.WriteLine("9 – finaliza programa");
		    Console.WriteLine("Filas existentes: " + GetFilasExistentes());
	    }

        private static void ExecutarMenu2(int pTipoFila) {
		    ImprimirMenu2(pTipoFila);

            try
            {
                int opcao = int.Parse(Console.In.ReadLine());
                switch (opcao)
                {
                    case 1:
                        bool filaCriada = CriarFila(pTipoFila);

                        if (filaCriada)
                            ExecutarMenu2(pTipoFila);

                        break;
                    case 2:
                        if (!FilaCriada(pTipoFila))
                        {
                            ImprimirErro("A fila deve ser criada primeiro.");
                            ExecutarMenu2(pTipoFila);
                            return;
                        }

                        DestuirFila(pTipoFila);
                        ExecutarMenu2(pTipoFila);

                        break;
                    case 3:
                        if (!FilaCriada(pTipoFila))
                        {
                            ImprimirErro("A fila deve ser criada primeiro.");
                            ExecutarMenu2(pTipoFila);
                            return;
                        }

                        InserirNafila(pTipoFila);
                        ExecutarMenu2(pTipoFila);

                        break;
                    case 4:
                        if (!FilaCriada(pTipoFila))
                        {
                            ImprimirErro("A fila deve ser criada primeiro.");
                            ExecutarMenu2(pTipoFila);
                            return;
                        }

                        ListarFila(pTipoFila);
                        ExecutarMenu2(pTipoFila);

                        break;
                    case 5:
                        if (!FilaCriada(pTipoFila))
                        {
                            ImprimirErro("A fila deve ser criada primeiro.");
                            ExecutarMenu2(pTipoFila);
                            return;
                        }

                        Excluir(pTipoFila);
                        ExecutarMenu2(pTipoFila);

                        break;
                }
            }
            catch(IndexOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                ExecutarMenu2(pTipoFila);
            }
	    }

        private static String GetFilasExistentes()
        {
            StringBuilder existentes = new StringBuilder();

            if (filaNomes != null)
                existentes.Append("nomes, ");

            if (filaInteiros != null)
                existentes.Append("inteiros, ");

            if (filaMontadoras != null)
                existentes.Append("montadoras");

            return existentes.ToString();
        }

        private static void ImprimirMenu2(int pTipo) {	
		    DescreverFila(pTipo);
		    Console.WriteLine("1 – criar fila");
		    Console.WriteLine("2 – destruir fila");
		    Console.WriteLine("3 – inserir");
		    Console.WriteLine("4 – mostrar");
		    Console.WriteLine("5 – excluir");
		    Console.WriteLine("9 – retornar ao menu 1");
	    }

        private static void DescreverFila(int pTipo) {
            Console.WriteLine();
		    Console.WriteLine("Tipo da Fila: " + GetNomeFila(pTipo));
		    Console.WriteLine("Total de Elementos: " + GetTotalElementosFila(pTipo));
		    Console.WriteLine("Tamanho da Fila: " + GetTamanhoFila(pTipo));
	    }

        private static String GetNomeFila(int pTipo)
        {
            switch (pTipo)
            {
                case 1: return "nomes";
                case 2: return "inteiros";
                case 3: return "montadoras";
                default: return "";
            }
        }

        private static String GetTotalElementosFila(int pTipo)
        {
            switch (pTipo)
            {
                case 1: return FilaCriada(pTipo) ? filaNomes.GetTotalElementos().ToString() : "não criada";
                case 2: return FilaCriada(pTipo) ? filaInteiros.GetTotalElementos().ToString() : "não criada";
                case 3: return FilaCriada(pTipo) ? filaMontadoras.GetTotalElementos().ToString() : "não criada";
                default: return "";
            }
        }

        private static String GetTamanhoFila(int pTipo)
        {
            switch (pTipo)
            {
                case 1: return FilaCriada(pTipo) ? filaNomes.GetTamanho().ToString() : "não criada";
                case 2: return FilaCriada(pTipo) ? filaInteiros.GetTamanho().ToString() : "não criada";
                case 3: return FilaCriada(pTipo) ? filaMontadoras.GetTamanho().ToString() : "não criada";
                default: return "";
            }
        }

        private static bool FilaCriada(int pTipo)
        {
            switch (pTipo)
            {
                case 1: return filaNomes != null;
                case 2: return filaInteiros != null;
                case 3: return filaMontadoras != null;
                default: return false;
            }
        }

        private static bool CriarFila(int pTipo) {
		    if(FilaCriada(pTipo)) {
                ImprimirErro("Fila já existe, não pode ser criada novamente.");
			    return false;
		    }

            Console.WriteLine();
		    Console.WriteLine("Qual o tamanho da lista?");
		    int tamanho = int.Parse(Console.In.ReadLine());
		
		    switch(pTipo) {
		    case 1: 
			    filaNomes = new FilaCircular<String>(tamanho);
			    break;
		    case 2: 
			    filaInteiros = new FilaCircular<Int32>(tamanho);
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
			    if(filaNomes.GetTotalElementos() == 0)			
				    filaNomes = null;
			    else {
                    ImprimirErro("Existem elementos na fila.");
				    ExecutarMenu2(pTipoFila);
			    }
				
			    break;
		    case 2:
			    if(filaInteiros.GetTotalElementos() == 0)			
				    filaInteiros = null;
			    else {
                    ImprimirErro("Existem elementos na fila.");
				    ExecutarMenu2(pTipoFila);
			    }
			
			    break;
		    case 3:
			    if(filaMontadoras.GetTotalElementos() == 0)			
				    filaMontadoras = null;
			    else {
                    ImprimirErro("Existem elementos na fila.");
				    ExecutarMenu2(pTipoFila);
			    }
			
			    break;
		    }
	    }

        private static void InserirNafila(int pTipoFila) {		
		    Console.WriteLine("Qual o valor? ");
		
		    switch(pTipoFila) {
		    case 1:			
			    String nome = Console.In.ReadLine();
			    filaNomes.Inserir(nome);
			
			    break;
		    case 2:
                int inteiro;
                if (int.TryParse(Console.In.ReadLine(), out inteiro))
                {
				    filaInteiros.Inserir(inteiro);
			    }
			    else
                {
				    InserirNafila(pTipoFila);
			    }
			
			    break;
		    case 3:
			    EMontadora[] montadoras = (EMontadora[])Enum.GetValues(typeof(EMontadora));
			
			    foreach(EMontadora m in montadoras) {
				    Console.WriteLine((int)m + " - " + m.ToString());
			    }
			
			    try
			    {
				    int opcaoMontadora = int.Parse(Console.In.ReadLine());
				    EMontadora montadoraEscolhida = montadoras[opcaoMontadora];
				
				    filaMontadoras.Inserir(montadoraEscolhida);				
			    }
			    catch (Exception) {
				    InserirNafila(pTipoFila);
			    }
			    break;
		    }
	    }

        private static void ListarFila(int pTipo) {
		    if(FilaCriada(pTipo)) {
			    String descricao = "";
			
			    switch(pTipo) {
			    case 1: 
				    descricao = filaNomes.ToString();
				    break;
			    case 2: 
				    descricao = filaInteiros.ToString();
				    break;
			    case 3: 
				    descricao = filaMontadoras.ToString();
				    break;
			    }
			
			    Console.WriteLine(descricao);
		    } else {
			    ImprimirErro("A fila não existe.");
		    }
	    }

        private static void Excluir(int pTipo) {
		    if(FilaCriada(pTipo)) {
			    String descricao = "";
			
			    switch(pTipo) {
			    case 1: 
				    filaNomes.Remover();
				    break;
			    case 2: 
				    filaInteiros.Remover();
				    break;
			    case 3: 
				    filaMontadoras.Remover();
				    break;
			    }
			
			    Console.WriteLine(descricao);
		    } 
            else 
            {
                ImprimirErro("A fila não existe.");
		    }
	    }

        private static void ImprimirErro(string pMensagem)
        {
            Console.WriteLine();
            Console.WriteLine(pMensagem);
            Console.WriteLine();
        }
    }
}
