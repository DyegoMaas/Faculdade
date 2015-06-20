using Emgu.CV;
using Emgu.CV.Structure;
using RedeNeural.Core.Classificacao;
using RedeNeural.Core.Classificacao.Entradas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ManipulaImagem
{
    public partial class Form1 : Form
    {
        private const int qtdArestas = 32;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var resultado = openFileDialog1.ShowDialog();
            if (resultado == DialogResult.OK || resultado == DialogResult.Yes)
            {
                caminhoArquivo.Text = openFileDialog1.FileName;
            }
        }

        private void iniciaTratamentoImagem()
        {
            //Carrega imagem
            Image<Bgr, Byte> img =
                new Image<Bgr, byte>(caminhoArquivo.Text)
                .Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);

            //Converte pra escala de zinza
            Image<Gray, Byte> gray = img.Convert<Gray, Byte>();
            CvInvoke.cvCvtColor(img, gray, Emgu.CV.CvEnum.COLOR_CONVERSION.BGR2GRAY);

            CvInvoke.cvThreshold(gray, gray, 127, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY);
            
            Contour<Point> contornos = gray.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE);

            var totalPontos = contornos.Total;

            int somatorioX = 0, 
                somatorioY = 0, 
                resultadoX, resultadoY;

            for (int i = 0; i < totalPontos; i++)
            {
                var ponto = contornos.Pop();

                somatorioX += ponto.X;
                somatorioY += ponto.Y;
            }

            resultadoX = somatorioX / totalPontos;
            resultadoY = somatorioY / totalPontos;

            var centro = new Point(resultadoX, resultadoY);
            var color = new Bgr(Color.Black);
            var bitmap = img.ToBitmap();
            var stepAngular = (360d / qtdArestas);

            List<PointF> pontosEncontrados = new List<PointF>();

            double angulo = 0;
            while (angulo < 360)
            {
                double anguloRad = (angulo * Math.PI / 180);

                const double stepDistancia = 1d;
                double distancia = 0;
                while (distancia < 1000)
                {
                    var x = distancia * Math.Sin(anguloRad);
                    var y = distancia * Math.Cos(anguloRad);

                    var pontoB = new Point(centro.X + (int)x, centro.Y + (int)y);
                    if (EncontrouBorda(pontoB, bitmap))
                    {
                        pontosEncontrados.Add(new PointF(pontoB.X, pontoB.Y));

                        CvInvoke.cvLine(img.Ptr, centro, pontoB, new MCvScalar(1), 1, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);
                        break;
                    }

                    distancia += stepDistancia;
                }

                angulo += stepAngular;
            }

            CvInvoke.cvCircle(img.Ptr, centro, 4, color.MCvScalar, 5, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);
            foreach (var pontoEncontrado in pontosEncontrados)
            {
                var pontoDesenho = new Point((int)pontoEncontrado.X, (int)pontoEncontrado.Y);
                CvInvoke.cvCircle(img.Ptr, pontoDesenho, 4, new Bgr(Color.Blue).MCvScalar, 5, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);
            }

            var classificador = ClassificadorAngulos.Abordagem1();
            var angulos = EncontrarAngulosDosPares(pontosEncontrados);
            var classificacoes = angulos.Select(a => classificador.Classificar(a)).ToList();

            var geradorValor = new GeradorValoresRede();
            var bits = geradorValor.GerarValor(classificacoes);

            imagem.Image = img;
        }

        private IList<int> EncontrarAngulosDosPares(IList<PointF> pontosEncontrados)
        {
            var lista = new List<int>();
            var extrator = new ExtratorRelacaoAngulos();
            for (int i = 1; i < pontosEncontrados.Count; i++)
            {
                var pontoA = new Vector2(pontosEncontrados[i - 1].X, pontosEncontrados[i - 1].Y);
                var pontoB = new Vector2(pontosEncontrados[i].X, pontosEncontrados[i].Y);

                var angulo = extrator.ExtrairRelacaoAngulos(new ParAmostral(pontoA, pontoB));
                lista.Add(angulo);
            }

            return lista;
        }

        private bool EncontrouBorda(Point ponto, Bitmap imagem)
        {
            var rect = new Rectangle(0, 0, imagem.Width, imagem.Height);
            if (!rect.Contains(ponto))
                return false;

            var pixel = imagem.GetPixel(ponto.X, ponto.Y);

            return pixel.R == 0 &&
                pixel.G == 0 &&
                pixel.B == 0;
        }
    }
}
