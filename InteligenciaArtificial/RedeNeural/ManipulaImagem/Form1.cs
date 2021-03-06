﻿using Emgu.CV;
using Emgu.CV.Structure;
using RedeNeural.Core;
using RedeNeural.Core.Classificacao;
using RedeNeural.Core.Classificacao.Entradas;
using RedeNeural.Core.Classificacao.Saidas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ManipulaImagem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btArquivoClassificacao_Click(object sender, EventArgs e)
        {
            var resultado = openFileDialog1.ShowDialog();
            if (resultado == DialogResult.OK || resultado == DialogResult.Yes)
            {
                caminhoArquivo.Text = openFileDialog1.FileName;
            }
        }

        private DiretorioTreinamento diretorioCarregado;
        private void caminhoArquivo_TextChanged(object sender, System.EventArgs e)
        {
            caminhoArquivo.Text = openFileDialog1.FileName;

            //while (diretorioCarregado == null)
            //{
            //    var resultado = folderBrowserDialog1.ShowDialog();
            //    if (resultado == DialogResult.OK || resultado == DialogResult.Yes)
            //    {
            //        diretorioCarregado = new DiretorioTreinamento(folderBrowserDialog1.SelectedPath);
            //    }
            //}
            diretorioCarregado = new DiretorioTreinamento(@"C:\temp\FormasGeometricas");

            var bits = ClassificarImagem(caminhoArquivo.Text);
            var classeGeometrica = RedeIdentificadoraFormasGeometricas.Computar(bits, diretorioCarregado);
            MessageBox.Show("A figura é: " + classeGeometrica);
        }

        private void btDiretorioTreinamento_Click(object sender, EventArgs e)
        {
            var resultado = folderBrowserDialog1.ShowDialog();
            if (resultado == DialogResult.OK || resultado == DialogResult.Yes)
            {
                tbDiretorioTreinamento.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void tbDiretorioTreinamento_TextChanged(object sender, EventArgs e)
        {
            var diretorio = tbDiretorioTreinamento.Text;
            if (string.IsNullOrWhiteSpace(diretorio) || !Directory.Exists(diretorio))
                return;

            var diretorioTreinamento = new DiretorioTreinamento(diretorio);
            if (diretorioTreinamento.ArquivoTreinamento.Exists && !cbSobrescreverTreinamento.Checked)
                return;

            Escrever(lbGeracaoDados, "Gerando dados de treinamento...");
            var dadosTreinamento = GerarDadosTreinamento(diretorioTreinamento);
            ExportarArquivoTreinamento(dadosTreinamento, diretorioTreinamento);
            Escrever(lbGeracaoDados, "Dados de treinamento gerados com sucesso.");
        }

        private void btTreinar_Click(object sender, EventArgs e)
        {
            var diretorio = tbDiretorioTreinamento.Text;
            if (string.IsNullOrWhiteSpace(diretorio) || !Directory.Exists(diretorio))
                return;

            var diretorioTreinamento = new DiretorioTreinamento(diretorio);
            if (!diretorioTreinamento.ArquivoTreinamento.Exists)
            {
                Escrever(lbStatusTreinamento, "Ainda não foi gerado um arquivo de treinamento.");
                return;
            }

            Escrever(lbStatusTreinamento, "Treinando rede neural.");
            var redeNeural = new RedeIdentificadoraFormasGeometricas(diretorioTreinamento);
            redeNeural.Treinar();
            Escrever(lbStatusTreinamento, "Treinamento concluído.");
        }

        private void Escrever(Label label, string texto)
        {
            label.Text = texto;
            Application.DoEvents();
        }

        private IEnumerable<ResultadoIdeal> GerarDadosTreinamento(DiretorioTreinamento diretorioTreinamento)
        {
            var datasetResultadosEsperados = new List<ResultadoIdeal>();

            datasetResultadosEsperados.AddRange(diretorioTreinamento.Elipses.GetFiles().Select(fi => fi.FullName)
                .Select(ClassificarImagem)
                .Select(bits => new ResultadoIdeal(bits, ClasseGeometrica.Elipse)));

            datasetResultadosEsperados.AddRange(diretorioTreinamento.Retangulos.GetFiles().Select(fi => fi.FullName)
                .Select(ClassificarImagem)
                .Select(bits => new ResultadoIdeal(bits, ClasseGeometrica.Retangulo)));

            datasetResultadosEsperados.AddRange(diretorioTreinamento.Triangulos.GetFiles().Select(fi => fi.FullName)
                .Select(ClassificarImagem)
                .Select(bits => new ResultadoIdeal(bits, ClasseGeometrica.Triangulo)));

            return datasetResultadosEsperados;
        }

        private void ExportarArquivoTreinamento(IEnumerable<ResultadoIdeal> datasetResultadosEsperados, DiretorioTreinamento diretorioTreinamento)
        {
            var arquivoTreinamento = diretorioTreinamento.ArquivoTreinamento;
            if(!arquivoTreinamento.Directory.Exists)
                arquivoTreinamento.Directory.Create();

            using (var arquivo = arquivoTreinamento.CreateText())
            {
                foreach (var resultadosEsperado in datasetResultadosEsperados)
                {
                    var bits = string.Join("", resultadosEsperado.Bits);
                    var linha = String.Format("{0}={1}", bits, (int) resultadosEsperado.Classe);
                    arquivo.WriteLine(linha);
                }
            }
        }

        private int[] ClassificarImagem(string caminhoImagem)
        {
            //AdicionarFundoPreto(caminhoImagem);
            //return null;

            //Carrega a imagem
            Image<Bgr, Byte> img =
                new Image<Bgr, byte>(caminhoImagem)
                .Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);

            //Converte pra escala de cinza
            Image<Gray, Byte> gray = img.Convert<Gray, Byte>();
            CvInvoke.cvCvtColor(img, gray, Emgu.CV.CvEnum.COLOR_CONVERSION.BGR2GRAY);

            //Encontra o contorno
            CvInvoke.cvThreshold(gray, gray, 127, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY);
            Contour<Point> pontosContorno = gray.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE);

            var centro = EncontrarCentroDaFormaGeometrica(pontosContorno);
            var bitmap = img.ToBitmap();
            const double stepAngular = GrupoAmostralAmostral.DiferencaAngular;

            var pontosEncontrados = EncontrarPontosInteresseNoContorno(centro, bitmap, img, stepAngular);

            CvInvoke.cvCircle(img.Ptr, centro, 4, new Bgr(Color.Black).MCvScalar, 5, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);
            foreach (var pontoEncontrado in pontosEncontrados)
            {
                var pontoDesenho = new Point((int)pontoEncontrado.X, (int)pontoEncontrado.Y);
                CvInvoke.cvCircle(img.Ptr, pontoDesenho, 4, new Bgr(Color.Blue).MCvScalar, 5, Emgu.CV.CvEnum.LINE_TYPE.CV_AA, 0);
            }
            imagem.Image = img;
            
            //TODO refatorar para garantir o encadeamento
            var classificador = ClassificadorFuzzy.Abordagem3();
            var angulos = EncontrarAngulosDosPares(pontosEncontrados, centro);
            var classificacoes = angulos.Select(classificador.Classificar).ToList();

            var geradorValor = new GeradorValoresRede();
            var bits = geradorValor.GerarValor(classificacoes);

            return bits;
        }

        private static void AdicionarFundoPreto(string caminhoImagem)
        {
            using (var memoryStream = new MemoryStream())
            {
                var bitmap1 = new Bitmap(caminhoImagem);
                using (var graphics = Graphics.FromImage(bitmap1))
                {
                    for (int i = 0; i < bitmap1.Width; i++)
                    {
                        for (int j = 0; j < bitmap1.Height; j++)
                        {
                            var pixel = bitmap1.GetPixel(i, j);
                            if (pixel.A == 0)
                                graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(i, j, 1, 1));
                        }
                    }
                    bitmap1.Save(memoryStream, ImageFormat.Png);
                    bitmap1.Dispose();
                }

                using (var fileStream = new FileStream(caminhoImagem, FileMode.Create, FileAccess.ReadWrite))
                {
                    var bytes = memoryStream.ToArray();
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        private List<PointF> EncontrarPontosInteresseNoContorno(Point centro, Bitmap bitmap, Image<Bgr, byte> img, double stepAngular)
        {
            var pontosEncontrados = new List<PointF>();

            double angulo = 0;
            while (angulo < 360)
            {
                var anguloRad = (angulo * Math.PI / 180);

                const double stepDistancia = 1d;
                double distancia = 0;
                while (distancia < 1000)
                {
                    var x = distancia * Math.Sin(anguloRad);
                    var y = distancia * Math.Cos(anguloRad);

                    var pontoB = new Point(centro.X + (int) x, centro.Y + (int) y);
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
            return pontosEncontrados;
        }

        private static Point EncontrarCentroDaFormaGeometrica(Contour<Point> pontosContorno)
        {
            var totalPontos = pontosContorno.Total;

            int somatorioX = 0,
                somatorioY = 0;

            for (var i = 0; i < totalPontos; i++)
            {
                var ponto = pontosContorno.Pop();

                somatorioX += ponto.X;
                somatorioY += ponto.Y;
            }

            var resultadoX = somatorioX / totalPontos;
            var resultadoY = somatorioY / totalPontos;

            var centro = new Point(resultadoX, resultadoY);
            return centro;
        }

        private IEnumerable<int> EncontrarAngulosDosPares(IList<PointF> pontosEncontrados, Point centro)
        {
            var lista = new List<int>();
            var extrator = new ExtratorRelacaoAngulos();

            var linkedList = new LinkedList<Vector2>();
            var listaVector2 = pontosEncontrados.Select(p => new Vector2(p.X, p.Y)).ToList();
            for (int i = 0; i < pontosEncontrados.Count; i++)
            {
                linkedList.AddLast(listaVector2[i]);
            }

            for (int i = 0; i < pontosEncontrados.Count; i++)
            {
                var nodoPontoA = linkedList.Find(listaVector2[i]);
                var pontoCentral = new Vector2(centro.X, centro.Y);
                var pontoA = i == 0 ? linkedList.Last.Value : nodoPontoA.Previous.Value;
                var pontoB = nodoPontoA.Value;
                var pontoC = i == pontosEncontrados.Count - 1 ? linkedList.First.Value : nodoPontoA.Next.Value;

                var trianguloAmostral = new GrupoAmostralAmostral(pontoCentral, pontoA, pontoB, pontoC);
                var angulo = extrator.ExtrairRelacaoAngulos(trianguloAmostral);
                lista.Add(angulo);
            }

            return lista;
        }

        private bool EncontrouBorda(Point ponto, Bitmap imagem)
        {
            var rect = new Rectangle(0, 0, imagem.Width, imagem.Height);
            if (!rect.Contains(ponto))
                return true;

            var pixel = imagem.GetPixel(ponto.X, ponto.Y);

            return pixel.R == 0 &&
                pixel.G == 0 &&
                pixel.B == 0;
        }
    }
}
