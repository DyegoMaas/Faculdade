using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Entregas.Core;
using Humanizer;

namespace Entregas
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

        private void BtIniciar_Click(object sender, RoutedEventArgs e)
        {
            int capacidade = int.Parse(TbCapacidade.Text);
            Executar(capacidade, 50);
        }

        private void Executar(int capacidade, int quantidadeProdutos)
        {
            using (var filaEntrega = new FilaEntregas(capacidade, 5.Milliseconds()))
            {
                var produzir = new Task(() =>
                {
                    for (int i = 0; i < quantidadeProdutos; i++)
                    {
                        var produto = new Produto(NomeAleatorio());
                        filaEntrega.AgendarEntrega(produto);
                    }
                });

                var consumir = new Task(() =>
                {
                    for (int i = 0; i < quantidadeProdutos; i++)
                    {
                        Produto produtoEntrege = filaEntrega.EfetuarEntrega();
                        Console.WriteLine("Produto {0} entregue.", produtoEntrege);
                    }
                });

                produzir.Start();
                consumir.Start();

                //Parallel.Invoke(produzir, consumir);
                Task.WaitAll(produzir, consumir);
                MessageBox.Show("concluido com {0} itens".FormatWith(filaEntrega.ObterQuantidadeEntregasPendentes()), "concluido");
            }
        }

        private static string NomeAleatorio()
        {
            var random = new Random(DateTime.Now.Millisecond);
            return random.Next(1000).ToRoman();
        }
    }
}
