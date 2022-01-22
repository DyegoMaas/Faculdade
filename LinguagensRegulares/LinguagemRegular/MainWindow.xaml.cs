using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Analisador.Excecoes.Finalizacao;
using Analisador.Linguagens;

namespace Analisador
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btAnalisar_Click(object sender, RoutedEventArgs e)
        {
            AnalisarPalavras();
        }

        private void AnalisarPalavras()
        {
            List<ResultadoAnalise> resultados = new List<ResultadoAnalise>();

            string[] linhas = new string[txtEntrada.LineCount];
            for (int i = txtEntrada.LineCount - 1; i >= 0; i--)
            {
                linhas[i] = txtEntrada.GetLineText(i);

                string[] palavras = SepararPalavras(linhas[i]);
                for (int j = 0; j < palavras.Length; j++)
                {
                    string palavra = palavras[j];    
                    if (palavra == string.Empty && (j == palavras.Length - 1))
                        break;

                    try
                    {
                        Processador processador = new Processador(palavra, LinguagensSuportadas.G2);
                        processador.Processar();
                    }
                    catch (ProcessingEndException ex)
                    {
                        ResultadoAnalise resultado = ex.Resultado;
                        resultado.Linha = i;
                        resultados.Add(resultado);
                    }
                }
            }
            dgDados.ItemsSource = resultados.OrderBy(r => r.Linha);
            dgDados.UpdateLayout();
        }

        private string[] SepararPalavras(string linha)
        {
            if (linha == string.Empty)
                linha = " ";

            linha = linha.Replace(Environment.NewLine, string.Empty);
            linha = linha.Replace('\t', ' ');
            while (linha.Contains("  "))
                linha = linha.Replace("  ", " ");

            return linha.Split(' '); ;
        }

        private void btEquipe_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("Dyego Alekssander Maas");
            s.Append("Tiago Justen Costa");

            MessageBox.Show(s.ToString(), "Equipe");
        }

        private void btLimpar_Click(object sender, RoutedEventArgs e)
        {
            txtEntrada.Clear();
            dgDados.ItemsSource = null;
            dgDados.UpdateLayout();
        }
    }
}
