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
using Autofac.Core;
using BufferMonitor.Configuracao;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;

namespace BufferMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IBufferChangeObserver
    {
        private readonly SGBB sgbd = GerenciadorSGBD.Inicializar();

        public MainWindow()
        {
            InitializeComponent();
            sgbd.GerenciadorBuffer.Registrar(this);
        }

        public void NotificarAlteracaoBuffer()
        {
            sgbd.GerenciadorBuffer.ListarPaginas();
        }
    }
}
