using System;
using System.Collections.Concurrent;
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
            public int NumeroProdutores { get; set; }
            public int NumeroConsumidores { get; set; }
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
                NumeroProdutores = ValorInteiro(TbProdutores),
                NumeroConsumidores = ValorInteiro(TbConsumidores),
                DelayAgendamento = TimeSpan.FromMilliseconds(ValorInteiro(TbDelayAgendamento)),
                DelayEntregas = TimeSpan.FromMilliseconds(ValorInteiro(TbDelayEntrega)),
            };

            Task.Run(() => Executar(configuracoes));
        }
        
        private void Executar(Configuracoes configuracoes)
        {
            //using (var filaEntrega = new FilaEntregas(configuracoes.CapacidadeMaxima, configuracoes.DelayAgendamento, configuracoes.DelayEntregas))
            //{
            //    var produzir = new Task(() =>
            //    {
            //        for (int i = 0; i < configuracoes.NumeroTotalProdutos; i++)
            //        {
            //            var produto = new Produto(NomeAleatorio());
            //            filaEntrega.AgendarEntrega(produto);
            //        }
            //    });

            //    var consumir = new Task(() =>
            //    {
            //        for (int i = 0; i < configuracoes.NumeroTotalProdutos; i++)
            //        {
            //            Produto produtoEntrege = filaEntrega.EfetuarEntrega();
            //            Console.WriteLine("Produto {0} entregue.", produtoEntrege);
            //        }
            //    });

            //    var duracaoLoopGrafico = TimeSpan.FromMilliseconds(50);
            //    var visualizar = new Task(() =>
            //    {
            //        while (filaEntrega.QuantidadeEntregue < configuracoes.NumeroTotalProdutos)
            //        {
            //            AtualizarGraficos(filaEntrega.ObterQuantidadeEntregasPendentes(), duracaoLoopGrafico);
            //            Thread.Sleep(duracaoLoopGrafico.Milliseconds);
            //        }
            //    });

            //    produzir.Start();
            //    consumir.Start();
            //    visualizar.Start();

            //    Task.WaitAll(produzir, consumir, visualizar);
            //    AtualizarGraficos(filaEntrega.ObterQuantidadeEntregasPendentes(), duracaoLoopGrafico);
            //}

            using (var filaEntregas = new BlockingCollection<Produto>(new ConcurrentQueue<Produto>(), configuracoes.CapacidadeMaxima))
            {
                var produzir = new Action(() =>
                {
                    for (int i = 0; i < configuracoes.NumeroTotalProdutos; i++)
                    {
                        var produto = new Produto(NomeAleatorio());
                        filaEntregas.Add(produto);
                        Console.WriteLine("Produto {0} agendado para entrega.", produto);
                        Thread.Sleep(configuracoes.DelayAgendamento);
                    }
                    filaEntregas.CompleteAdding();
                });

                var consumir = new Action(() =>
                {
                    foreach (var produto in filaEntregas.GetConsumingEnumerable())
                    {
                        Produto produtoEntrege = filaEntregas.Take();
                        Console.WriteLine("Produto {0} entregue.", produtoEntrege);
                        Thread.Sleep(configuracoes.DelayEntregas);
                    }
                });

                var duracaoLoopGrafico = TimeSpan.FromMilliseconds(50);
                var visualizador = new Task(() =>
                {
                    int pendentes = 0;
                    while ((pendentes = filaEntregas.Count) < configuracoes.NumeroTotalProdutos)
                    {
                        AtualizarGraficos(pendentes, duracaoLoopGrafico);
                        Thread.Sleep(duracaoLoopGrafico.Milliseconds);
                    }
                });

                var tasks = new List<Task>();
                for (int i = 0; i < configuracoes.NumeroProdutores; i++)
                {
                    tasks.Add(new Task(produzir));
                }
                for (int i = 0; i < configuracoes.NumeroConsumidores; i++)
                {
                    tasks.Add(new Task(consumir));
                }
                tasks.Add(visualizador);

                tasks.ForEach(t => t.Start());
                Task.WaitAll(tasks.ToArray());
                AtualizarGraficos(0, duracaoLoopGrafico);
            }
        }

        private string NomeAleatorio()
        {
            var random = new Random(DateTime.Now.Millisecond);
            return random.Next(1, 1000).ToRoman();
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

            LblContador.Dispatcher.BeginInvoke(new Action(() =>
            {
                string descricaoCapacidade = "{0}/{1}".FormatWith(aguardandoEntrega.ToString("D3"), configuracoes.CapacidadeMaxima.ToString("D3"));
                LblContador.Content = descricaoCapacidade;
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
