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
using System.Windows.Media.Animation;
using System.Threading;

namespace Entregas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private class Configuracoes{
            public int CapacidadeMaxima { get; set; }
            public int NumeroTotalProdutos { get; set; }
            public TimeSpan DelayAgendamento { get; set; }
            public TimeSpan DelayEntregas { get; set; }
        }

        private Configuracoes configuracoes;

        public MainWindow()
        {
            InitializeComponent();
        }

        private int ValorInteiro(TextBox textBox)
        {
            return int.Parse(textBox.Text);
        }

        private void BtIniciar_Click(object sender, RoutedEventArgs e)
        {
            configuracoes = new Configuracoes {
                CapacidadeMaxima = ValorInteiro(TbCapacidade),
                NumeroTotalProdutos = ValorInteiro(TbItens),
                DelayAgendamento = TimeSpan.FromMilliseconds(ValorInteiro(TbDelayAgendamento)),
                DelayEntregas = TimeSpan.FromMilliseconds(ValorInteiro(TbDelayEntrega)),
            };

            Task.Run(() => Executar(configuracoes));
        }
        
        private void Executar(Configuracoes configuracoes)
        {
            using (var filaEntrega = new FilaEntregas(configuracoes.CapacidadeMaxima, configuracoes.DelayAgendamento, configuracoes.DelayEntregas))
            {
                var produzir = new Task(() =>
                {
                    for (int i = 0; i < configuracoes.NumeroTotalProdutos; i++)
                    {
                        var produto = new Produto(NomeAleatorio());
                        filaEntrega.AgendarEntrega(produto);
                    }
                });

                var consumir = new Task(() =>
                {
                    for (int i = 0; i < configuracoes.NumeroTotalProdutos; i++)
                    {
                        Produto produtoEntrege = filaEntrega.EfetuarEntrega();
                        Console.WriteLine("Produto {0} entregue.", produtoEntrege);
                    }
                });

                var visualizar = new Task(() =>
                {
                    var duracaoLoop = TimeSpan.FromMilliseconds(50);
                    while (true)
                    {

                        AtualizarGraficos(filaEntrega.ObterQuantidadeEntregasPendentes(), duracaoLoop);
                        Thread.Sleep(duracaoLoop.Milliseconds);
                    }
                });

                produzir.Start();
                consumir.Start();
                visualizar.Start();

                Task.WaitAll(produzir, consumir, visualizar);

                Dispatcher.BeginInvoke(new Action(() => 
                {
                    MessageBox.Show("concluido com {0} itens".FormatWith(filaEntrega.ObterQuantidadeEntregasPendentes()), "concluido");
                }));
            }
        }

        private string NomeAleatorio()
        {
            var random = new Random(DateTime.Now.Millisecond);
            return random.Next(1000).ToRoman();
        }

        private void AtualizarGraficos(int aguardandoEntrega, TimeSpan duracaoLoop)
        {
            BarraCapacidade.Dispatcher.BeginInvoke(new Action(() =>
            {
                const int alturaMaximaBarraCapacidade = 238;
                const int alturaMinimaBarraCapacidade = 30;

                var alturaBarra = (double)aguardandoEntrega / configuracoes.CapacidadeMaxima * alturaMaximaBarraCapacidade;
                alturaBarra = alturaBarra > alturaMinimaBarraCapacidade ? alturaBarra : alturaMinimaBarraCapacidade;

                var animacao = new DoubleAnimation(BarraCapacidade.Height, alturaBarra, new Duration(duracaoLoop));
                BarraCapacidade.BeginAnimation(Rectangle.HeightProperty, animacao);
            }));
        }

        private void TbDelayEntrega_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CheckIsNumeric(e);
        }

        private void TbCapacidade_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CheckIsNumeric(e);
        }

        private void TbDelayAgendamento_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CheckIsNumeric(e);
        }

        private void TbItens_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CheckIsNumeric(e);
        }

        private void CheckIsNumeric(TextCompositionEventArgs e)
        {
            int result;

            if (!(int.TryParse(e.Text, out result) || e.Text == "."))
            {
                e.Handled = true;
            }
        }

    }
}
